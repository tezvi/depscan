using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace DepScan
{
    internal class DepRegistry
    {
        public class Entry
        {
            public string Url = "";
            public string Name = "";
            public string Title = "";
            public string RepoName = "";
            public string Description = "";
            public string Owner = "";
            public IList<Maintainer> Maintainers = new List<Maintainer>();
            public IList<string> LanguageTags = new List<string>();
            public int WatchersCount = 0;
            public int ForksCount = 0;
            public int OpenIssuesCount = 0;
            public bool Archived = false;
            public bool HasIssues = false;
            public string License = "";
            public string CountryOrigin = "";
            public string UpdatedAt = "";
        }

        public class Maintainer
        {
            public string Name = "";
            public string User = "";
            public string Url = "";
            public string Location = "";
            public string Email = "";
        }

        public readonly IList<Entry> Entries = new List<Entry>();

        public void LoadXmlData(string xmlFilePath)
        {
            Entries.Clear();
            var doc = new XmlDocument();
            doc.Load(xmlFilePath);

            Debug.Assert(doc.DocumentElement != null, "doc.DocumentElement != null");
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var repoName = node.Attributes?["name"].Value;

                if (repoName == null)
                {
                    Program.WriteLog("XML Node does not define attribute name");
                    continue;
                }

                if (Entries.Any(item => item.RepoName.Equals(repoName)))
                {
                    Program.WriteLog($"Repo {repoName} already loaded");
                    continue;
                }

                var entry = new Entry();
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    LoadNode(entry, childNode);
                }

                Entries.Add(entry);
            }
        }

        private static void LoadNode(Entry entry, XmlNode node)
        {
            switch (node.Name)
            {
                case "Url":
                    entry.Url = node.InnerText;
                    break;
                case "Name":
                    entry.Name = node.InnerText;
                    break;
                case "Title":
                    entry.Title = node.InnerText;
                    break;
                case "RepoName":
                    entry.RepoName = node.InnerText;
                    break;
                case "Description":
                    entry.Description = node.InnerText;
                    break;
                case "Owner":
                    entry.Owner = node.InnerText;
                    break;
                case "Maintainers":
                    node.ChildNodes.OfType<XmlNode>().ToList()
                        .ForEach(xmlNode => LoadMaintainersNode(entry, xmlNode));

                    break;
                case "LanguageTags":
                    node.InnerText?.Split(';')
                        .ToList()
                        .ForEach(value => entry.LanguageTags.Add(value.Trim()));
                    break;
                case "WatchersCount":
                    entry.WatchersCount = int.Parse(node.InnerText);
                    break;
                case "ForksCount":
                    entry.ForksCount = int.Parse(node.InnerText);
                    break;
                case "Archived":
                    entry.Archived = bool.Parse(node.InnerText);
                    break;
                case "HasIssues":
                    entry.HasIssues = bool.Parse(node.InnerText);
                    break;
                case "License":
                    entry.License = node.InnerText;
                    break;
                case "CountryOrigin":
                    entry.CountryOrigin = node.InnerText;
                    break;
                case "UpdatedAt":
                    entry.UpdatedAt = node.InnerText;
                    break;
            }
        }

        private static void LoadMaintainersNode(Entry entry, XmlNode node)
        {
            var maintainer = new Maintainer();
            foreach (XmlNode itemNode in node.ChildNodes)
            {
                switch (itemNode.Name)
                {
                    case "Name":
                        maintainer.Name = itemNode.InnerText;
                        break;
                    case "User":
                        maintainer.User = itemNode.InnerText;
                        break;
                    case "Url":
                        maintainer.Url = itemNode.InnerText;
                        break;
                    case "Location":
                        maintainer.Location = itemNode.InnerText;
                        break;
                    case "Email":
                        maintainer.Email = itemNode.InnerText;
                        break;
                }
            }
            entry.Maintainers.Add(maintainer);
        }
    }
}