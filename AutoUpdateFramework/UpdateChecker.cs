using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace AutoUpdate.Framework
{
    /// <summary>
    /// Used to check for software updates.
    /// </summary>
    /// <example>
    /// 
    /// // Create a new UpdateChecker that queries the specified URI with the CurrentVersion defined as 1.0
    /// UpdateChecker checker = new UpdateChecker("http://convex.st4r.io/sample/version.manifest");
    /// 
    /// // Check for an update, and print.
    /// Console.WriteLine(checker.CheckForUpdates() ? "Update is available!" : "Latest version is in use.");
    /// 
    /// </example>
    public class UpdateChecker
    {
        /// <summary>
        /// Gets or sets the URI to use in an update query.
        /// </summary>
        public Uri QueryUri { get; set; }

        /// <summary>
        /// Gets or sets the current software version to use for comparison.
        /// </summary>
        public Version CurrentVersion { get; set; }
        
        /// <summary>
        /// Gets or sets the header values that are sent with an update query.
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// Gets the latest version. CheckForUpdates() must be called first.
        /// </summary>
        public Version LatestVersion => versionManifest.LatestVersion;

        private VersionManifest versionManifest;

        /// <summary>
        /// Creates a new UpdateChecker with default values: 
        /// QueryUri = http://localhost/
        /// CurrentVersion = 1.0
        /// </summary>
        public UpdateChecker()
        {
            QueryUri = new Uri("http://localhost/");
            CurrentVersion = new Version(1, 0);
        }

        /// <summary>
        /// Creates a new UpdateChecker with default values, unless otherwise specified.
        /// </summary>
        public UpdateChecker(string uri = "http://localhost/", string currentVersion = "1.0")
        {
            QueryUri = new Uri(uri);
            CurrentVersion = Version.Parse(currentVersion);
        }

        /// <summary>
        /// Creates a new UpdateChecker with the URI specified and default version unless otherwise specified.
        /// </summary>
        public UpdateChecker(Uri uri, string currentVersion = "1.0")
        {
            QueryUri = uri;
            CurrentVersion = Version.Parse(currentVersion);
        }

        /// <summary>
        /// Updates the internal version manifest by querying the QueryUri.
        /// </summary>
        /// <returns>True if the current version is outdated.</returns>
        /// <example>
        /// 
        /// // Create a new UpdateChecker that queries the specified URI with the CurrentVersion defined as 1.0
        /// UpdateChecker checker = new UpdateChecker("http://convex.st4r.io/sample/version.manifest");
        /// 
        /// // Check for an update, and print.
        /// Console.WriteLine(checker.CheckForUpdates() ? "Update is available!" : "Latest version is in use.");
        /// 
        /// </example>
        public bool CheckForUpdates()
        {
            WebClient client = new WebClient();
            if (Headers != null)
                client.Headers = Headers;
            string manifest = client.DownloadString(QueryUri);
            versionManifest = JsonConvert.DeserializeObject<VersionManifest>(manifest);
            return CurrentVersion < LatestVersion;
        }

        /// <summary>
        /// Gets the information URI for a specified version.
        /// </summary>
        /// <param name="version">A software version.</param>
        /// <returns>The information URI.</returns>
        public string GetInfoUri(Version version) => versionManifest.Versions.Find(v => v.Version == version)?.InfoUri;

        /// <summary>
        /// Gets the download URI for a specified version.
        /// </summary>
        /// <param name="version">A software version.</param>
        /// <returns>The download URI.</returns>
        public string GetDownloadUri(Version version) => versionManifest.Versions.Find(v => v.Version == version)?.DownloadUri;

        /// <summary>
        /// Generates a sample version manifest JSON text.
        /// </summary>
        /// <returns>A sample version manifest in JSON format</returns>
        public static string GenerateSampleJson()
        {
            VersionManifest manifest = new VersionManifest
            {
                Versions = new List<VersionManifestEntry>
                {
                    new VersionManifestEntry
                    {
                        Version = new Version(1, 0),
                        InfoUri = "http://example.com/v1.0/whats_new.html",
                        DownloadUri = "http://example.com/v1.0/release.zip"
                    },
                    new VersionManifestEntry
                    {
                        Version = new Version(1, 0, 3, 2),
                        DownloadUri = "http://example.com/v1.0.3.2/release.tar.gz"
                    },
                    new VersionManifestEntry
                    {
                        Version = new Version(1, 1),
                        InfoUri = "http://example.com/v1.1/whats_new.php"
                    },
                    new VersionManifestEntry
                    {
                        Version = new Version(1, 1, 5)
                    }
                }
            };
            return JsonConvert.SerializeObject(manifest, Formatting.Indented);
        }
    }
    
    /// <summary>
    /// Represents a list of software versions that are available.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VersionManifest : IEquatable<VersionManifest>
    {
        /// <summary>
        /// Gets the latest version currently available.
        /// </summary>
        [JsonIgnore]
        public Version LatestVersion => Versions.Max().Version;

        /// <summary>
        /// Gets a list of all of the available versions of the software.
        /// </summary>
        [JsonProperty("versions", Required = Required.Always)]
        public List<VersionManifestEntry> Versions { get; set; } = new List<VersionManifestEntry>();

        public static bool operator ==(VersionManifest man1, VersionManifest man2)
        {
            return man1?.Equals(man2) ?? false;
        }

        public static bool operator !=(VersionManifest man1, VersionManifest man2)
        {
            return !(man1 == man2);
        }

        public bool Equals(VersionManifest other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VersionManifest)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((LatestVersion?.GetHashCode() ?? 0) * 397) ^ (Versions?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (VersionManifestEntry entry in Versions)
                sb.AppendLine(entry.ToString());
            return sb.ToString();
        }
    }

    /// <summary>
    /// Represents an entry in a VersionManifest.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VersionManifestEntry : IEquatable<VersionManifestEntry>, IComparable<VersionManifestEntry>, ICloneable
    {
        [JsonProperty("version", Order = 0, Required = Required.Always)]
        private string _version = "1.0";
        /// <summary>
        /// Gets or sets the version that is being represented.
        /// </summary>
        [JsonIgnore]
        public Version Version
        {
            get { return Version.Parse(_version); }
            set {  _version = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the URI that contains information about an update, such as a changelog.
        /// </summary>
        [JsonProperty("info", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string InfoUri { get; set; }

        /// <summary>
        /// Gets or sets the URI that is a direct link to download an update, such as a zip file, or an installer.
        /// </summary>
        [JsonProperty("download", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public string DownloadUri { get; set; }

        public static bool operator ==(VersionManifestEntry entry1, VersionManifestEntry entry2)
        {
            return entry1?.Equals(entry2) ?? false;
        }

        public static bool operator !=(VersionManifestEntry entry1, VersionManifestEntry entry2)
        {
            return !(entry1 == entry2);
        }

        public static implicit operator string(VersionManifestEntry entry) => entry.ToString();

        public bool Equals(VersionManifestEntry other) => this.GetHashCode() == other.GetHashCode();

        public int CompareTo(VersionManifestEntry other) => this.Version.CompareTo(other.Version);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VersionManifestEntry)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Version?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (InfoUri?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (DownloadUri?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
        
        /// <summary>
        /// Creates a new VersionManifestEntry with identical values to the current instance.
        /// </summary>
        /// <returns>An identical VersionManifestEntry</returns>
        public object Clone()
        {
            return new VersionManifestEntry
            {
                Version = Version,
                InfoUri = InfoUri,
                DownloadUri = DownloadUri
            };
        }

        public override string ToString() => $"[{Version} - info={InfoUri} download={DownloadUri}]";
    }
}