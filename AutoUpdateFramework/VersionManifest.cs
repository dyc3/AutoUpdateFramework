using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AutoUpdate.Framework
{
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
}