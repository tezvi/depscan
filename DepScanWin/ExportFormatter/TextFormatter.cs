using System.IO;
using System.Linq;

namespace DepScan.ExportFormatter
{
    public class TextFormatter : IExportFormatter
    {
        private static readonly string HeaderLine = new string('-', 50);
        private static readonly string MatchEntry = new string(' ', 2) + "* ";
        private static readonly string MatchIndent = new string(' ', 4);
        private static readonly string MatchIndent2 = new string(' ', 7);
        public void Export(string filePath, ResultManager resultManager)
        {
            using (var writer = File.CreateText(filePath))
            {
                resultManager.Matches.ForEach(scannedFile => OutputScanResult(scannedFile, writer));
                writer.Flush();
            }
        }

        public void OutputScanResult(ResultManager.ScannedFile scannedFile, TextWriter writer)
        {
            writer.WriteLine(HeaderLine);
            writer.WriteLine($"Matches for file: {scannedFile.FilePath}");
            writer.WriteLine($"[context/dir {scannedFile.ContextPath}]");
            writer.WriteLine($"Matches (total {scannedFile.Matches.Count})");

            foreach (var match in scannedFile.Matches)
            {
                var depEntry = Program.Registry.Entries.FirstOrDefault(entry => entry.RepoName.Equals(match.RepoName));
                var owners = "";

                if (depEntry != null)
                {
                    owners = string.Join("; ", depEntry.Maintainers
                        .Select(maintainer =>
                            (maintainer.Name ?? maintainer.User) + (maintainer.Email.Length > 0 ? $" <{maintainer.Email}>" : "")));

                    if (owners.Length == 0 || !owners.Contains(depEntry.Owner))
                    {
                        owners = depEntry.Owner + (owners.Length > 0 ? "; " + owners : "");
                    }
                }
                
                writer.WriteLine($"{MatchEntry}Matched dependency/repo name \"{match.RepoName}\" at char index {match.Index}");
                writer.WriteLine($"{MatchIndent}Matched by rule \"{match.RuleName}\" with value \"{match.Keyword}\"");

                writer.WriteLine($"{MatchIndent2}Owner / maintainers: {owners}");

                if (depEntry == null) continue;

                writer.WriteLine($"{MatchIndent2}Country origin: {depEntry.CountryOrigin}");
                writer.WriteLine($"{MatchIndent2}Name: {depEntry.Name}");
                writer.WriteLine($"{MatchIndent2}Title: {depEntry.Title}");
                writer.WriteLine($"{MatchIndent2}URL: {depEntry.Url}");
                writer.WriteLine($"{MatchIndent2}Description: {depEntry.Description}");
                writer.WriteLine($"{MatchIndent2}Archived: {depEntry.Archived}");
                writer.WriteLine($"{MatchIndent2}Number of forks: {depEntry.ForksCount}");
                writer.WriteLine($"{MatchIndent2}Has posted issues: {depEntry.HasIssues}");
                writer.WriteLine($"{MatchIndent2}Number of opened issues: {depEntry.OpenIssuesCount}");
                writer.WriteLine($"{MatchIndent2}Number of watchers: {depEntry.WatchersCount}");
                writer.WriteLine($"{MatchIndent2}Last update: {depEntry.UpdatedAt}");
                writer.WriteLine($"{MatchIndent2}License: {depEntry.License}");
            }

            writer.WriteLine("\n");
        }
    }
}
