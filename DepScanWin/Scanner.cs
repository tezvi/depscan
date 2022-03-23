using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DepScan.ExportFormatter;

namespace DepScan
{
    public class Scanner
    {
        private readonly Config.FormData _scanConfig;
        private readonly FormScan _scanForm;
        private readonly BackgroundWorker _worker;
        private Dictionary<string, Dictionary<string, RegexPattern>> _cachedPatterns;
        private IList<string> AllowedPatterns { get; set; }
        private bool ScanArchives { get; set; }
        private bool ScanMaintainers { get; set; }
        private WorkStatus LastStatus { get; set; }
        private static readonly List<string> ArchivePatterns;
        private const int MinNameCharacters = 4;
        private const int MaxFileLength = 2 * 1024 * 1024; // 2 MB
        private readonly ResultManager _resultManager;
        private readonly IExportFormatter _exporter;

        private int _flushLastIndex = -1;
        private const int FlushThreshold = 100;
        private const int FlushWaitSeconds = 30;
        private DateTime _flushLastTime;
        private StreamWriter _reportWriter;
        private Task _reportWriterTask;

        public class WorkCanceledException : Exception
        {
        }

        public class WorkStatus
        {
            public long CurrentFileCount { get; set; }
            public string ContextPath { get; set; }
            public string CurrentPath { get; set; }
            public long CurrentItemNum { get; set; }
            public long ErrorCount { get; set; }
            public long Matches { get; set; }
            public int Percentage { get; set; }
        }

        public class ItemContext
        {
            public string ContextPath { get; set; }
            public string CurrentPath { get; set; }
            public List<string> ArchiveContext { get; set; } = new List<string>();
            public double FileSize { get; set; }
            public bool IsBinary { get; set; }
        }

        public Scanner(Config.FormData scanConfig, FormScan scanForm, ResultManager resultManager)
        {
            _scanConfig = scanConfig;
            _scanForm = scanForm;
            _resultManager = resultManager;
            _exporter = new TextFormatter();

            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private class RegexPattern
        {
            public Regex Pattern { get; }
            public string RawValue { get; }

            public RegexPattern(Regex pattern, string rawValue)
            {
                Pattern = pattern;
                RawValue = rawValue;
            }
        }

        static Scanner()
        {
            ArchivePatterns = new List<string> { ".jar", ".war", ".ear", ".zip" };
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var message = "Scan completed.";
            var messageBoxIcon = MessageBoxIcon.Information;
            if (e.Cancelled)
            {
                message = "Scan cancelled!";
            }
            else if (e.Error != null)
            {
                message = $"Scan failed with error '{e.Error.Message}'";
                Program.WriteLog(e.Error);
                messageBoxIcon = MessageBoxIcon.Error;
            }

            _scanForm.Scanning = false;
            Program.WriteLog(message);
            MessageBox.Show(this._scanForm, message, Program.AppName, MessageBoxButtons.OK, messageBoxIcon);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            NotifyFormUpdate((WorkStatus)e.UserState);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.WriteLog("Started scan process");
            ScanArchives = _scanConfig.Options.Contains(Config.ConfigArchive);
            ScanMaintainers = _scanConfig.Options.Contains(Config.ConfigMaintainers);
            AllowedPatterns = GetAllowedFilePatterns();
            InitializeSearchPatterns();

            try
            {
                LastStatus = new WorkStatus { CurrentFileCount = CountItems() };

                foreach (var dirPath in _scanConfig.Directories)
                {
                    ScanItems(dirPath);
                }

                FlushMatchesToFile(true);

                while (!_reportWriterTask.IsCompleted)
                {
                }

                HandleReportTaskExceptions();
            }
            catch (Exception exception)
            {
                if (exception.GetType() == typeof(WorkCanceledException))
                {
                    Program.WriteLog("Canceled background worker");
                    e.Cancel = true;
                }
                else
                {
                    Program.DebugLog($"Worker received exception: {exception}");
                    throw;
                }
            }
            finally
            {
                DisposeResources();
            }
        }

        private void HandleReportTaskExceptions()
        {
            if (_reportWriterTask.Status != TaskStatus.Faulted || _reportWriterTask.Exception == null) return;
            var exception = _reportWriterTask.Exception.InnerExceptions.First();
            Program.DebugLog(
                $"Caught internal worker exception and trying to recover.\nException: {_scanConfig.OutputReportPath}");
            throw new ReportWorkerException($"Report worker thread exception has occurred: {exception.Message}",
                exception);
        }

        public class ReportWorkerException : Exception
        {
            public ReportWorkerException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }

        private IList<string> GetAllowedFilePatterns()
        {
            var patterns = new List<string>();

            foreach (var scanConfigOption in _scanConfig.Options)
            {
                switch (scanConfigOption)
                {
                    case Config.ConfigJavaScript:
                        patterns.Add("package.json");
                        patterns.Add("package-lock.json");
                        break;
                    case Config.ConfigJava:
                        patterns.Add("pom.xml");
                        patterns.Add(".gradle");
                        break;
                    case Config.ConfigPhp:
                        patterns.Add("composer.json");
                        patterns.Add("composer.lock");
                        break;
                    case Config.ConfigPython:
                        patterns.Add("requirements.txt");
                        break;
                    case Config.ConfigArchive:
                        patterns.AddRange(ArchivePatterns);
                        break;
                    case Config.ConfigExtended:
                        patterns.Add("*");
                        break;
                }
            }

            Program.DebugLog($"Configured allowed file patterns (total: {patterns.Count})");

            return patterns;
        }

        private void ScanItems(string path)
        {
            IEnumerable<string> items;
            try
            {
                Program.DebugLog($"Enumerating directory items in path: {path}");
                items = Directory.EnumerateFileSystemEntries(path);
            }
            catch (Exception exception)
            {
                if (!Utils.IsFileAccessException(exception)) throw;
                LastStatus.ErrorCount++;
                Program.WriteLog($"Unable to read path {path} with error {exception.Message}");
                return;
            }

            foreach (var entryPath in items)
            {
                if (_worker.CancellationPending)
                {
                    throw new WorkCanceledException();
                }

                Program.DebugLog($"Scanning directory items in path: {path}");

                FileAttributes attr;
                try
                {
                    attr = File.GetAttributes(entryPath);
                }
                catch (Exception exception)
                {
                    if (!Utils.IsFileAccessException(exception)) throw;
                    LastStatus.ErrorCount++;
                    Program.WriteLog($"Unable to read path {entryPath} with error {exception.Message}");
                    continue;
                }

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    ScanItems(entryPath);
                    continue;
                }

                if (!Utils.PatternMatches(entryPath, AllowedPatterns))
                {
                    Program.DebugLog($"Skipping file \"{entryPath}\", does not match allowed patterns.");
                    continue;
                }

                var file = new FileInfo(entryPath);
                if (file.Length == 0)
                {
                    Program.DebugLog($"Skipping zero length file: {entryPath}");
                    LastStatus.CurrentItemNum++;
                    continue;
                }

                var context = new ItemContext
                {
                    FileSize = file.Length,
                    IsBinary = Utils.IsBinary(file),
                    CurrentPath = file.FullName,
                    ContextPath = file.DirectoryName
                };

                try
                {
                    ScanFile(file, context);
                }
                catch (Exception exception)
                {
                    if (!Utils.IsFileAccessException(exception)) throw;
                    Program.WriteLog($"Unable to read {entryPath}: exception: {exception.Message}");
                    // TODO track error in listView
                    LastStatus.ErrorCount++;
                }

                _worker.ReportProgress(0, LastStatus);
            }
        }

        private void ScanFile(FileInfo file, ItemContext context, string virtualFilePath = null)
        {
            Program.DebugLog($"Scanning file \"{file.FullName}\".");

            LastStatus = new WorkStatus
            {
                CurrentPath = virtualFilePath ?? file.FullName,
                ContextPath = context.ArchiveContext.Count > 0
                    ? string.Join(" -> ", context.ArchiveContext)
                    : context.ContextPath,
                CurrentFileCount = LastStatus.CurrentFileCount,
                CurrentItemNum = LastStatus.CurrentItemNum + 1,
                ErrorCount = LastStatus.ErrorCount,
                Matches = LastStatus.Matches
            };

            _worker.ReportProgress(0, LastStatus);
            var isArchive = context.IsBinary && Utils.PatternMatches(file.FullName, ArchivePatterns);
            if (context.IsBinary && !(ScanArchives && isArchive))
            {
                Program.DebugLog($"Skipping file \"{file.FullName}\", not a binary or archive allowed.");
                return;
            }

            if (isArchive)
            {
                context.ArchiveContext.Add(virtualFilePath ?? file.FullName);
                ProcessArchiveFile(file, context);
                return;
            }

            if (file.Length > MaxFileLength)
            {
                Program.WriteLog($"Skipping file {file.FullName} larger than {Utils.BytesToString(MaxFileLength)}");
                return;
            }

            var contents = File.ReadAllText(file.FullName);
            var scannedFile = new ResultManager.ScannedFile(LastStatus.CurrentPath, LastStatus.ContextPath);

            Program.DebugLog(
                $"Searching file \"{file.FullName}\" contents (length: {file.Length}) for match keywords.");

            foreach (var cachedPatterns in _cachedPatterns)
            {
                if (_worker.CancellationPending)
                {
                    throw new WorkCanceledException();
                }

                foreach (var pair in cachedPatterns.Value)
                {
                    var match = pair.Value.Pattern.Match(contents);
                    if (!match.Success) continue;

                    LastStatus.Matches++;
                    var resultMatch = new ResultManager.Match
                    {
                        Index = match.Index,
                        Keyword = pair.Value.RawValue,
                        RepoName = cachedPatterns.Key,
                        RuleName = pair.Key
                    };

                    scannedFile.Matches.Add(resultMatch);
                    Program.DebugLog($"Matched file \"{file.FullName}\" with rule name {resultMatch.RuleName}.");

                    // No need for further matching if this dependency has been matched by current rule.
                    break;
                }
            }

            if (scannedFile.Matches.Count <= 0) return;
            _resultManager.Matches.Add(scannedFile);
            FlushMatchesToFile();
        }

        private void FlushMatchesToFile(bool forceFlush = false)
        {
            var matchCount = _resultManager.Matches.Count;
            if (!forceFlush)
            {
                if (_flushLastIndex + 1 >= matchCount)
                {
                    return;
                }

                if (_flushLastTime.AddSeconds(FlushWaitSeconds) >= DateTime.Now &&
                    (matchCount - _flushLastIndex + 1) <= FlushThreshold)
                {
                    return;
                }
            }

            Program.DebugLog("Flushing matches to report file.");
            _reportWriterTask = Task.Run(SafeWriteMatches);
            HandleReportTaskExceptions();
        }

        private void SafeWriteMatches()
        {
            lock (_reportWriter)
            {
                for (var index = _flushLastIndex + 1; index < _resultManager.Matches.Count; index++)
                {
                    _exporter.OutputScanResult(_resultManager.Matches[index], _reportWriter);
                    _flushLastIndex = index;
                }

                _reportWriter.Flush();
                _flushLastTime = DateTime.Now;
            }
        }

        private void ProcessArchiveFile(FileSystemInfo file, ItemContext context)
        {
            Program.DebugLog($"Processing archive \"{file.FullName}\".");
            using (var archive = ZipFile.OpenRead(file.FullName))
            {
                foreach (var entry in archive.Entries)
                {
                    if (_worker.CancellationPending)
                    {
                        break;
                    }

                    if (entry.Length == 0 || entry.Name.Length == 0 || entry.FullName.EndsWith("/"))
                    {
                        continue;
                    }

                    if (!Utils.PatternMatches(entry.FullName, AllowedPatterns))
                    {
                        continue;
                    }

                    ScanArchiveFile(entry, file, context);
                }
            }
        }

        private void ScanArchiveFile(ZipArchiveEntry entry, FileSystemInfo file, ItemContext context)
        {
            var testFile = new FileInfo(entry.FullName);
            var destinationPath = Path.Combine(Path.GetTempPath(),
                Program.AppName + "_" + Utils.CreateMd5(entry.FullName) + testFile.Extension);

            Program.DebugLog($"Processing archive \"{file.FullName}\" entry file \"{entry.FullName}\".");

            try
            {
                entry.ExtractToFile(destinationPath, true);
                var entryFile = new FileInfo(destinationPath);
                LastStatus.CurrentFileCount++;

                ScanFile(entryFile,
                    new ItemContext
                    {
                        ContextPath = context.CurrentPath,
                        CurrentPath = destinationPath,
                        FileSize = entryFile.Length,
                        IsBinary = Utils.IsBinary(entryFile),
                        ArchiveContext = context.ArchiveContext.ToList()
                    },
                    entry.FullName);
            }
            catch (Exception exception)
            {
                LastStatus.ErrorCount++;
                Program.WriteLog(
                    $"Error processing JAR {file.FullName} file entry {entry.FullName}: {exception.Message}");
                throw;
            }
            finally
            {
                if (File.Exists(destinationPath))
                {
                    Program.DebugLog($"Deleting archive temporary file \"{destinationPath}\".");
                    File.Delete(destinationPath);
                }
            }
        }

        private void InitializeSearchPatterns()
        {
            _cachedPatterns = new Dictionary<string, Dictionary<string, RegexPattern>>();
            foreach (var rule in Program.Registry.Entries)
            {
                var searchTerms = new Dictionary<string, RegexPattern>
                {
                    { "RepoName", PrepareRegexRule(rule.RepoName) }
                };

                if (ScanMaintainers && rule.Owner.Length >= MinNameCharacters)
                {
                    searchTerms.Add("Owner", PrepareRegexRule(rule.Owner));
                }

                foreach (var maintainer in rule.Maintainers)
                {
                    if (ScanMaintainers && !maintainer.Name.Equals(rule.Name, StringComparison.CurrentCultureIgnoreCase)
                                        && maintainer.Name.Length >= MinNameCharacters)
                    {
                        searchTerms.Add($"Maintainer[{maintainer.Name}]", PrepareRegexRule(maintainer.Name));
                    }

                    if (maintainer.Email.Length >= MinNameCharacters)
                    {
                        searchTerms.Add($"Maintainer[{maintainer.Email}]", PrepareRegexRule(maintainer.Email));
                    }
                }

                _cachedPatterns.Add(rule.RepoName, searchTerms);
            }

            Program.DebugLog($"Initialized {_cachedPatterns.Count} search patterns");
        }

        private static RegexPattern PrepareRegexRule(string value)
        {
            var pattern = (@"\b(" + Regex.Escape(value) + @")\b")
                .Replace("/", "\\.");

            var regex = new Regex(pattern,
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            return new RegexPattern(regex, value);
        }

        private void NotifyFormUpdate(WorkStatus status)
        {
            if (status == null) return;

            status.Percentage = (int)Math.Round(100 * Convert.ToDecimal(status.CurrentItemNum) /
                                                Convert.ToDecimal(status.CurrentFileCount));
            if (status.Percentage > 100)
            {
                status.Percentage = 100;
            }

            _scanForm.NotifyStatus(status);
        }

        private long CountItems()
        {
            return _scanConfig.Directories.Sum(directory =>
                Utils.CountFilesRecursively(directory, AllowedPatterns, _worker));
        }

        public void Start()
        {
            try
            {
                Program.DebugLog("Opening report output FileStream");
                _reportWriter = new StreamWriter(_scanConfig.OutputReportPath, false, Encoding.UTF8);
                _reportWriter.Flush();
                _flushLastTime = DateTime.Now;
            }
            catch (Exception exception)
            {
                Program.DebugLog($"Unable to create report file {_scanConfig.OutputReportPath}");
                Program.SafeMsgBox(
                    $"Unable to create report file {_scanConfig.OutputReportPath}. Please check your input.\n\n"
                    + "Error: " + exception.Message, MessageBoxIcon.Error);
                return;
            }

            Program.DebugLog("Starting main scan async worker");
            _scanForm.Scanning = true;
            _worker.RunWorkerAsync();

            _scanForm.CancelInvoker = () => _worker.CancelAsync();
            _scanForm.ShowDialog(Program.MainForm);
        }

        private void DisposeResources()
        {
            Program.DebugLog("Disposing resources");
            _reportWriter?.Close();
        }
    }
}