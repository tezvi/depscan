using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DepScan
{
    internal class Utils
    {
        public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public class NoSystemMenuException : Exception
        {
        }

        // Values taken from MSDN.
        public enum ItemFlags
        {
            // The item ...
            MfUnchecked = 0x00000000, // ... is not checked
            MfString = 0x00000000, // ... contains a string as label
            MfDisabled = 0x00000002, // ... is disabled
            MfGrayed = 0x00000001, // ... is grayed
            MfChecked = 0x00000008, // ... is checked
            MfPopup = 0x00000010, // ... Is a popup menu. Pass the

            //     menu handle of the popup
            //     menu into the ID parameter.
            MfBarBreak = 0x00000020, // ... is a bar break
            MfBreak = 0x00000040, // ... is a break
            MfByPosition = 0x00000400, // ... is identified by the position
            MfByCommand = 0x00000000, // ... is identified by its ID

            MfSeparator = 0x00000800 // ... is a seperator (String and
            //     ID parameters are ignored).
        }

        public enum WindowMessages
        {
            WmSysCommand = 0x0112
        }

        /// <summary>
        /// A class that helps to manipulate the system menu
        /// of a passed form.
        /// 
        /// Written by Florian "nohero" Stinglmayr
        /// </summary>
        public class SystemMenu
        {
            // I havn't found any other solution than using plain old
            // WinAPI to get what I want.
            // If you need further information on these functions, their
            // parameters, and their meanings, you should look them up in
            // the MSDN.

            // All parameters in the [DllImport] should be self explanatory.
            // NOTICE: Use never stdcall as a calling convention, since Winapi
            // is used.
            // If the underlying structure changes, your program might cause
            // errors that are hard to find.

            // First, we need the GetSystemMenu() function.
            // This function does not have an Unicode counterpart
            [DllImport("USER32", EntryPoint = "GetSystemMenu", SetLastError = true, CharSet = CharSet.Unicode,
                ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            private static extern IntPtr apiGetSystemMenu(IntPtr windowHandle, int bReset);

            // And we need the AppendMenu() function. Since .NET uses Unicode,
            // we pick the unicode solution.
            [DllImport("USER32", EntryPoint = "AppendMenuW", SetLastError = true, CharSet = CharSet.Unicode,
                ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            private static extern int apiAppendMenu(IntPtr menuHandle, int flags, int newId, String item);

            // And we also may need the InsertMenu() function.
            [DllImport("USER32", EntryPoint = "InsertMenuW", SetLastError = true, CharSet = CharSet.Unicode,
                ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            private static extern int apiInsertMenu(IntPtr hMenu, int position, int flags, int newId, String item);

            private IntPtr _mSysMenu = IntPtr.Zero; // Handle to the System Menu

            // Insert a separator at the given position index starting at zero.
            public bool InsertSeparator(int pos)
            {
                return InsertMenu(pos, ItemFlags.MfSeparator | ItemFlags.MfByPosition, 0, "");
            }

            // Simplified InsertMenu(), that assumes that Pos is a relative
            // position index starting at zero
            public bool InsertMenu(int pos, int id, String item)
            {
                return InsertMenu(pos, ItemFlags.MfByPosition | ItemFlags.MfString, id, item);
            }

            // Insert a menu at the given position. The value of the position
            // depends on the value of Flags. See the article for a detailed
            // description.
            public bool InsertMenu(int pos, ItemFlags flags, int id, String item)
            {
                return apiInsertMenu(_mSysMenu, pos, (Int32)flags, id, item) == 0;
            }

            // Appends a seperator
            public bool AppendSeparator()
            {
                return AppendMenu(0, "", ItemFlags.MfSeparator);
            }

            // This uses the ItemFlags.mfString as default value
            public bool AppendMenu(int id, String item)
            {
                return AppendMenu(id, item, ItemFlags.MfString);
            }

            // Superseded function.
            public bool AppendMenu(int id, String item, ItemFlags flags)
            {
                return apiAppendMenu(_mSysMenu, (int)flags, id, item) == 0;
            }

            // Retrieves a new object from a Form object
            public static SystemMenu FromForm(Form frm)
            {
                var cSysMenu = new SystemMenu
                {
                    _mSysMenu = apiGetSystemMenu(frm.Handle, 0)
                };

                if (cSysMenu._mSysMenu == IntPtr.Zero)
                {
                    // Throw an exception on failure
                    throw new NoSystemMenuException();
                }

                return cSysMenu;
            }

            // Reset's the window menu to it's default
            public static void ResetSystemMenu(Form frm)
            {
                apiGetSystemMenu(frm.Handle, 1);
            }

            // Checks if an ID for a new system menu item is OK or not
            public static bool VerifyItemId(int id)
            {
                return id < 0xF000 && id > 0;
            }
        }

        public static int CountFilesRecursively(string path, IList<string> allowedPatterns,
            BackgroundWorker backgroundWorker = null)
        {
            var itemCount = 0;
            try
            {
                foreach (var entryPath in Directory.EnumerateFileSystemEntries(path))
                {
                    if (backgroundWorker != null && backgroundWorker.CancellationPending)
                    {
                        throw new Scanner.WorkCanceledException();
                    }

                    FileAttributes attr;
                    try
                    {
                        attr = File.GetAttributes(entryPath);
                    }
                    catch (Exception exception)
                    {
                        if (!IsFileAccessException(exception)) throw;
                        Program.WriteLog($"Unable to read path {entryPath} with error {exception.Message}");
                        continue;
                    }

                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        itemCount += CountFilesRecursively(entryPath, allowedPatterns, backgroundWorker);
                    }
                    else if (PatternMatches(entryPath, allowedPatterns))
                    {
                        itemCount++;
                    }
                }
            }
            catch (Exception exception)
            {
                if (!IsFileAccessException(exception)) throw;
                Program.WriteLog($"Unable to read from {path}: exception: {exception}");
            }

            return itemCount;
        }

        public static bool IsFileAccessException(Exception exception)
        {
            return exception is NotSupportedException || exception is UnauthorizedAccessException ||
                   exception is PathTooLongException || exception is FileNotFoundException;
        }

        public static bool PatternMatches(string path, IEnumerable<string> patterns)
        {
            return patterns.Any(pattern => pattern.Equals("*") || path.EndsWith(pattern, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool IsBinary(FileInfo file)
        {
            var length = file.Length;
            if (length == 0) return false;

            using (var stream = new StreamReader(file.FullName))
            {
                int ch;
                while ((ch = stream.Read()) != -1)
                {
                    if (IsControlChar(ch))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsControlChar(int ch)
        {
            return (ch > Chars.Nul && ch < Chars.Bs)
                   || (ch > Chars.Cr && ch < Chars.Sub);
        }

        public static class Chars
        {
            public static char Nul = (char)0; // Null char
            public static char Bs = (char)8; // Back Space
            public static char Cr = (char)13; // Carriage Return
            public static char Sub = (char)26; // Substitute
        }

        public static string CreateMd5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string AssemblyDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; // Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + suf[place];
        }
    }
}
