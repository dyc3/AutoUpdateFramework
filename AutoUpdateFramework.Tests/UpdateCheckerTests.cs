using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AutoUpdate.Framework.Tests
{
    [TestClass]
    public class UpdateCheckerTests
    {
        [TestMethod]
        public void Test_PrintSampleJson()
        {
            Console.WriteLine(UpdateChecker.GenerateSampleJson());
        }

        [TestMethod]
        public void Test_SampleSerialize()
        {
            string json = UpdateChecker.GenerateSampleJson();
            VersionManifest manifest = JsonConvert.DeserializeObject<VersionManifest>(json);
            Console.WriteLine(manifest);
        }

        [TestMethod]
        public void Test_Compare_Entry_FullObject_Equal()
        {
            VersionManifestEntry entryA = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4),
                DownloadUri = "http://update.test.com/download.zip",
                InfoUri = "http://test.com/update_changelog.html"
            };
            VersionManifestEntry entryB = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4),
                DownloadUri = "http://update.test.com/download.zip",
                InfoUri = "http://test.com/update_changelog.html"
            };

            if (entryA == entryB)
                Console.WriteLine("success");
            else
                throw new Exception("failed");
        }

        [TestMethod]
        public void Test_Compare_Entry_FullObject_NotEqual()
        {
            VersionManifestEntry entryA = new VersionManifestEntry
            {
                Version = new Version(1, 2),
                DownloadUri = "http://update.test.com/download.zip",
                InfoUri = "http://test.com/update_changelog.html"
            };
            VersionManifestEntry entryB = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4),
                DownloadUri = "http://update.test.com/download.zip",
                InfoUri = "http://test.com/update_changelog.html"
            };

            if (entryA != entryB)
                Console.WriteLine("success");
            else
                throw new Exception("failed");
        }

        [TestMethod]
        public void Test_Compare_Entry_PartObject_Equal()
        {
            VersionManifestEntry entryA = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4),
                DownloadUri = "http://update.test.com/download.zip"
            };
            VersionManifestEntry entryB = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4),
                DownloadUri = "http://update.test.com/download.zip"
            };

            if (entryA == entryB)
                Console.WriteLine("success");
            else
                throw new Exception("failed");
        }

        [TestMethod]
        public void Test_Compare_Entry_PartObject_NotEqual()
        {
            VersionManifestEntry entryA = new VersionManifestEntry
            {
                Version = new Version(1, 2),
                DownloadUri = "http://update.test.com/download.zip"
            };
            VersionManifestEntry entryB = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4),
                InfoUri = "http://test.com/update_changelog.html"
            };

            if (entryA != entryB)
                Console.WriteLine("success");
            else
                throw new Exception("failed");
        }

        [TestMethod]
        public void Test_Compare_Entry_MinObject_Equal()
        {
            VersionManifestEntry entryA = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4)
            };
            VersionManifestEntry entryB = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4)
            };

            if (entryA == entryB)
                Console.WriteLine("success");
            else
                throw new Exception("failed");
        }

        [TestMethod]
        public void Test_Compare_Entry_MinObject_NotEqual()
        {
            VersionManifestEntry entryA = new VersionManifestEntry
            {
                Version = new Version(1, 2)
            };
            VersionManifestEntry entryB = new VersionManifestEntry
            {
                Version = new Version(1, 2, 3, 4)
            };

            if (entryA != entryB)
                Console.WriteLine("success");
            else
                throw new Exception("failed");
        }
    }
}
