using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DepScan
{
    public class Config
    {
        public const string ConfigArchive = "archive";
        public const string ConfigJavaScript = "javascript";
        public const string ConfigPython = "python";
        public const string ConfigJava = "java";
        public const string ConfigPhp = "php";
        public const string ConfigExtended = "extended";
        public const string ConfigMaintainers = "maintainers";

        public static IList<ConfigItem> OptionList = new List<ConfigItem>
        {
            new ConfigItem(ConfigArchive, "Follow compressed files (jar, ear, war, zip)"),
            new ConfigItem(ConfigJavaScript, "Scan Javascript project metadata files (NPM)"),
            new ConfigItem(ConfigPython, "Scan Python project metadata files (requirements)"),
            new ConfigItem(ConfigJava, "Scan Java project metadata files (POM, Gradle)"),
            new ConfigItem(ConfigPhp, "Scan PHP project metadata files (Composer)"),
            new ConfigItem(ConfigExtended, "Scan all textual files with extended regex", false),
            new ConfigItem(ConfigMaintainers, "Scan for extra package maintainer details", false)
        };

        public static IList<string> GetDefaultDirs()
        {
            var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var dirList = new List<string>
            {
                Path.Combine(homeDir, ".m2"),
                Path.Combine(homeDir, ".composer"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Composer"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Composer"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Composer")
            };

            return dirList.Where(Directory.Exists).ToList();
        }

        public class ConfigItem
        {
            public string Key { get; }
            public string Value { get; }

            public bool DefaultChecked { get; }

            public ConfigItem(string key, string value, bool defaultChecked = true)
            {
                Key = key;
                Value = value;
                DefaultChecked = defaultChecked;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        public class FormData
        {
            public readonly IList<string> Options;
            public readonly IList<string> Directories;
            public readonly string OutputReportPath;
            public FormData(IList<string> options, IList<string> directories, string outputReportPath)
            {
                Options = options;
                Directories = directories;
                OutputReportPath = outputReportPath;
            }
        }
    }
}