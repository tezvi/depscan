using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DepScan
{
    internal static class Program
    {
        public const string AppTitle = "DepScan";
        public const string AppName = "DepScan";
        public static long Build = 20220321002;
        public static string LogFile;
        public static TextWriter LogWriter;
        private static ThreadExceptionEventHandler _exceptionHandler;
        private static UnhandledExceptionEventHandler _unhandledExceptionHandler;
        public static FormWait WaitDialog;
        public static FormMain MainForm;
        public static DepRegistry Registry;
        public static readonly HashSet<string> CmdArgs = new HashSet<string>();

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // exception handling
            _exceptionHandler = Application_ThreadException;
            _unhandledExceptionHandler = CurrentDomain_UnhandledException;
            Application.ThreadException += _exceptionHandler;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += _unhandledExceptionHandler;
            WaitDialog = new FormWait();
            Registry = new DepRegistry();
            
            InitLog();
            ReadCmdLineArguments(args);

            MainForm = new FormMain
            {
                Text = AppTitle
            };
            MainForm.CreateControl();

            WriteLog("Application started");

            Application.Run(MainForm);
            AppExit();
        }

        private static void ReadCmdLineArguments(IReadOnlyCollection<string> args)
        {
            if (args.Count <= 0) return;

            foreach (var arg in args)
            {
                if (arg.IndexOf('-') == 0)
                {
                    CmdArgs.Add(arg.Substring(1).ToLower());
                }
            }
            DebugMode = CmdArgs.Contains("debug");

            DebugLog("Reading cmd line arguments: " + string.Join("; ", CmdArgs));
        }

        public static bool DebugMode { get; set; }


        public static void ErrorBox(string msg, string title = "")
        {
            ShowError(null, msg, title, null);
        }

        private static void ShowError(Form owner, string msg, string title, Exception ex)
        {
            if (title == "") title = AppTitle + ": Error";
            if (ex != null) WriteLog(ex);
            if (owner == null && MainForm != null) owner = MainForm;
            if (owner != null && owner.InvokeRequired)
            {
                owner.Invoke(new MethodInvoker(() =>
                {
                    MessageBox.Show(owner, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
            else
            {
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void InitLog()
        {
            var baseDirectory = Utils.AssemblyDirectory;
            LogFile = Path.Combine(baseDirectory, AppName + ".log");

            try
            {
                var append = File.Exists(LogFile) && new FileInfo(LogFile).Length < 10 * 1024 * 1024;
                LogWriter = new StreamWriter(LogFile, append);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogFile = Path.GetTempFileName();
                LogWriter = new StreamWriter(LogFile, false);
                MessageBox.Show(
                    $"Unable to create default program log file in directory {baseDirectory}. Using tmp path {LogFile}",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void SafeMsgBox(string text, MessageBoxIcon icon)
        {
            if (MainForm != null && MainForm.InvokeRequired)
            {
                MainForm.Invoke(new MethodInvoker(() =>
                {
                    MessageBox.Show(MainForm, text, AppTitle, MessageBoxButtons.OK, icon);
                }));
            }
            else
            {
                MessageBox.Show(text, AppTitle, MessageBoxButtons.OK, icon);
            }
        }

        public static void AppExit()
        {
            LogWriter?.Close();
            Application.Exit();
            Application.ExitThread();
            Application.ThreadException -= _exceptionHandler;
        }

        public static void WriteLog(Exception ex)
        {
            WriteLog(ex.ToString());
        }


        public static void DebugLog(string message)
        {
            if (!DebugMode) return;
            WriteLog("[DEBUG]: " + message);
            Console.WriteLine(message);
        }

        public static void WriteLog(string message)
        {
            if (LogWriter == null) return;
            try
            {
                lock (LogWriter)
                {
                    LogWriter.WriteLine(DateTime.Now + ">  " + message);
                    LogWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logger has gone away: " + ex);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                var ex = e.Exception;
                Console.WriteLine(ex);
                WriteLog("ThreadException: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                new FormError { Exception = e.Exception }.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ErrorBox("Thread exception has occurred: " + ex.Message);
            }
            finally
            {
                AppExit();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception ?? new Exception();
                Console.WriteLine(ex);
                WriteLog("UnhandledException: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                new FormError { Exception = ex }.ShowDialog();
                AppExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ErrorBox("Unhandled exception: " + ex.Message, "Fatal non UI error");
            }
            finally
            {
                AppExit();
            }
        }
    }
}