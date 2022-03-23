using System.Collections.Generic;

namespace DepScan
{
    public class ResultManager
    {
        public List<ScannedFile> Matches { get; } = new List<ScannedFile>();

        public class ScannedFile
        {
            public string FilePath { get; }
            public string ContextPath { get; }
            public readonly List<Match> Matches = new List<Match>();

            public ScannedFile(string filePath, string contextPath)
            {
                FilePath = filePath;
                ContextPath = contextPath;
            }
        }

        public class Match
        {
            public string RepoName { get; set; }
            public string RuleName { get; set; }
            public string Keyword { get; set; }
            public int Index { get; set; }
        }
    }
}
