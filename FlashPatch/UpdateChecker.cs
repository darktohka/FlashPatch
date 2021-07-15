using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Linq;

namespace FlashPatch {
    public class Version {
        private string name;
        private string version;
        private string url;

        public Version(string name, string version, string url) {
            this.name = name;
            this.version = version;
            this.url = url;
        }

        public string GetName() {
            return name;
        }

        public string GetVersion() {
            return version;
        }

        public string GetUrl() {
            return url;
        }
    }

    public class UpdateChecker {
        private static readonly string VERSION = "v1.9";
        private static readonly string AUTHOR = "darktohka";
        private static readonly string REPO = "FlashPatch";
        private static readonly string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";

        public static string GetAPILink() {
            return "https://api.github.com/repos/" + AUTHOR + "/" + REPO + "/releases/latest";
        }

        public static string GetCurrentVersion() {
            return VERSION;
        }

        private static Version GetLatestVersionUnsafe() {
            using (WebClient client = new WebClient()) {
                client.Headers.Add("user-agent", USER_AGENT);

                string release = client.DownloadString(GetAPILink());
                XmlDictionaryReader jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(release), new XmlDictionaryReaderQuotas());
                XElement root = XElement.Load(jsonReader);

                string name = root.Descendants("name").FirstOrDefault().Value;
                string tag = root.Descendants("tag_name").FirstOrDefault().Value;
                string url = root.Descendants("html_url").FirstOrDefault().Value;

                if (!url.StartsWith("https://")) {
                    // This is a suspicious URL... We shouldn't trust it.
                    return null;
                }

                return new Version(name, tag, url);
            }
        }

        public static Version GetLatestVersion() {
            try {
                return GetLatestVersionUnsafe();
            } catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
