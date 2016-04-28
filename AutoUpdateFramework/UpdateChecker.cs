using System;
using System.Collections.Generic;
using System.Net;
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
}