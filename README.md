# AutoUpdateFramework

A basic framework written in C# for automatic updates.

```C#
// Create a new UpdateChecker that queries the specified URI with the CurrentVersion defined as 1.0
UpdateChecker checker = new UpdateChecker("http://convex.st4r.io/sample/version.manifest");

// Check for an update, and print.
Console.WriteLine(checker.CheckForUpdates() ? "Update is available!" : "Latest version is in use.");
```
