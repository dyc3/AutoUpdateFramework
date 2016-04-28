using System;
using Newtonsoft.Json;

namespace AutoUpdate.Framework
{
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