using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DepScan.ExportFormatter;
using DepScan.Properties;

namespace DepScan
{
    public partial class FormScan : Form
    {
        private bool _scanning;
        private long _secondsTicker;
        private readonly ResultManager _resultManager;
        private int _listViewLastIndex;
        private const double ScreenScaleFactor = 0.65;
        private const double ScreenDivider = 1.66;
        private const double ScreenMinInitWidth = 1000;
        private const double ScreenMinInitHeight = 600;

        public bool Scanning
        {
            get => _scanning;
            set
            {
                _scanning = value;
                if (Visible && IsHandleCreated)
                {
                    Invoke(new MethodInvoker(EnableControls));
                }
            }
        }

        public FormScan(ResultManager resultManager)
        {
            Icon = Resources.MainFormIcon;
            InitializeComponent();
            
            var screenBounds = Screen.PrimaryScreen.Bounds;
            var sizeCalc = new Size
            {
                Width = Convert.ToInt32(
                    Math.Min(
                        Math.Max(screenBounds.Width * ScreenScaleFactor, ScreenMinInitWidth),
                        screenBounds.Width
                    )
                )
            };

            sizeCalc.Height = Convert.ToInt32(
                Math.Min(
                    Math.Max(sizeCalc.Width / ScreenDivider, ScreenMinInitHeight),
                    screenBounds.Height
                )
            );

            Size = sizeCalc;
            
            _resultManager = resultManager;
        }

        public MethodInvoker CancelInvoker { set; get; }

        public partial class ListViewBuffered : ListView
        {
            public ListViewBuffered()
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            }
        }

        private void EnableControls()
        {
            btnCancel.Enabled = Scanning;
            toolStripBtnAutoScroll.Enabled = Scanning;
            toolStripBtnExport.Enabled = !Scanning && _resultManager.Matches.Count > 0;
            Text = Program.AppTitle + (Scanning ? ": Scanning ..." : "");
            timerScan.Enabled = Scanning;
            progressBar.Visible = Scanning;
            lblPercentage.Visible = Scanning;
            if (!Scanning)
            {
                txtContext.Text = "";
                txtSubContext.Text = "";
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel scan process?", "Scan", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK)
            {
                CancelInvoker?.Invoke();
            }
        }

        public void NotifyStatus(Scanner.WorkStatus status)
        {
            Invoke(new MethodInvoker(() => UpdateStatus(status)));
        }

        private void UpdateStatus(Scanner.WorkStatus status)
        {
            if (status.CurrentFileCount > 0)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                lblPercentage.Text = $@"{status.Percentage.ToString(CultureInfo.InvariantCulture)}%";
            }

            progressBar.Value = status.Percentage;

            txtContext.Text = status.ContextPath;
            txtSubContext.Text = status.CurrentPath;
            toolStripErrors.Text = "Errors: " + status.ErrorCount.ToString(CultureInfo.InvariantCulture);
            toolStripMatches.Text = "Matches: " + status.Matches.ToString(CultureInfo.InvariantCulture);
            toolStripScanned.Text = "Scanned files: " + status.CurrentItemNum.ToString(CultureInfo.InvariantCulture);

            UpdateScanResultsView();
        }

        private void UpdateScanResultsView()
        {
            if (_listViewLastIndex >= _resultManager.Matches.Count - 1)
            {
                return;
            }

            Program.DebugLog("Updating UI result view with new matches");
            listViewResults.BeginUpdate();

            for (var index = _listViewLastIndex; index < _resultManager.Matches.Count; index++)
            {
                _listViewLastIndex = index;
                var scannedFile = _resultManager.Matches[index];
                RenderListViewGroup(scannedFile);
            }

            listViewResults.EndUpdate();

            if (listViewResults.Items.Count > 0 && toolStripBtnAutoScroll.Checked)
            {
                listViewResults.Items[listViewResults.Items.Count - 1].EnsureVisible();
            }

            Application.DoEvents();
        }

        private void RenderListViewGroup(ResultManager.ScannedFile scannedFile)
        {
            if (listViewResults.Groups.OfType<ListViewGroup>().Any(group => group.Tag.Equals(scannedFile.FilePath)))
            {
                return;
            }

            var lstGroup = new ListViewGroup(
                $"{scannedFile.FilePath} [context/dir {scannedFile.ContextPath}] - total matches {scannedFile.Matches.Count}")
            {
                Tag = scannedFile.FilePath
            };
            listViewResults.Groups.Add(lstGroup);

            foreach (var match in scannedFile.Matches)
            {
                var depEntry = Program.Registry.Entries.FirstOrDefault(entry => entry.RepoName.Equals(match.RepoName));
                var owner = "";

                if (depEntry != null)
                {
                    owner = string.Join("; ", depEntry.Maintainers
                        .Select(maintainer =>
                            (maintainer.Name ?? maintainer.User) + (maintainer.Email.Length > 0 ? $" <{maintainer.Email}>" : "")));

                    if (owner.Length == 0 || !owner.Contains(depEntry.Owner))
                    {
                        owner = depEntry.Owner + (owner.Length > 0 ? "; " + owner : "");
                    }
                }

                var listItem = new ListViewItem
                {
                    Group = lstGroup,
                    Text = new FileInfo(scannedFile.FilePath).Name + $" at position [{match.Index}]",
                    SubItems =
                        {
                            new ListViewItem.ListViewSubItem { Text = match.RepoName },
                            new ListViewItem.ListViewSubItem { Text = depEntry?.CountryOrigin },
                            new ListViewItem.ListViewSubItem { Text = owner },
                            new ListViewItem.ListViewSubItem { Text = match.RuleName },
                            new ListViewItem.ListViewSubItem { Text = match.Keyword },
                            new ListViewItem.ListViewSubItem
                            {
                                Text = depEntry != null ? string.Join(", ", depEntry.LanguageTags) : ""
                            }
                        }
                };

                listViewResults.Items.Add(listItem);
            }
        }

        private void FormScan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Scanning) return;
            e.Cancel = true;
            BtnCancel_Click(sender, e);
        }

        private void FormScan_Load(object sender, EventArgs e)
        {
            listViewResults.Focus();
            toolStripRules.Text =
                "Loaded rules: " + Program.Registry.Entries.Count.ToString(CultureInfo.InvariantCulture);
            lblPercentage.Text = "";
            EnableControls();
        }

        private void TimerScan_Tick(object sender, EventArgs e)
        {
            _secondsTicker++;
            var t = TimeSpan.FromSeconds(_secondsTicker);
            lblTimer.Text = t.Hours > 0
                ? $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}"
                : $"{t.Minutes:D2}:{t.Seconds:D2}";
        }

        private void ToolStripBtnExport_Click(object sender, EventArgs e)
        {
            if (exportFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var exporter = new TextFormatter();
                Program.WaitDialog.SafeShowWithJob(() => exporter.Export(exportFileDialog.FileName, _resultManager), this, "Saving results to text file.");
        }
    }
}