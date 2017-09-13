#addin "NuGet.Core"
#addin "Cake.ExtendedNuGet"
#addin "Cake.Npm"
#addin "Cake.DocFx"
#tool "docfx.console"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration =
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") :
    "Release";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var solution = "./Intranet.sln";
var unitTestProjects = GetFiles("./**/*.UnitTests.csproj");
var webDir = Directory("./Intranet.Web/");
var directoriesToClean = GetDirectories("./**/bin/");

// Define settings.
var buildSettings = new DotNetCoreBuildSettings
{
    Configuration = configuration,
};

var testSettings = new DotNetCoreTestSettings
{
    Configuration = configuration,
    NoBuild = true
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories(directoriesToClean);
});

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
      Information("Running hack for ImageSharp to be able to install it later.");
      // TODO: This should live in it's own NuGet.config but that won't work at the moment:
      //       See https://github.com/NuGet/Home/issues/4907
      var imageSharpSource = "https://www.myget.org/F/imagesharp/api/v3/index.json";
      var tempCachePath = MakeAbsolute(Directory("temp-nuget-cache"));

      if (!NuGetHasSource(imageSharpSource))
      {
        NuGetAddSource(
            name: "ImageSharp Nightly",
            source: imageSharpSource
        );
      }

      NuGetInstall("ImageSharp", new NuGetInstallSettings {
        OutputDirectory = tempCachePath,
        Version = "1.0.0-alpha9-00194",
        Source = new string[] { imageSharpSource, "https://api.nuget.org/v3/index.json" }
      });
      Information("Hack for ImageSharp complete.");

      Information("Restoring Nuget packages.");
      DotNetCoreRestore(solution);
      Information("All Nuget packages restored.");

      Information("Cleaning temp folder.");
      CleanDirectory(tempCachePath);

      Information("Deleting temp folder.");
      DeleteDirectory(tempCachePath);

      Information("Restoring NPM packages.");

      NpmInstall(settings => settings.FromPath("./Intranet.Web").WithLogLevel(NpmLogLevel.Warn));
});

Task("Build")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
{
    Information("Building: {0}", "Intranet.Web");
    NpmRunScript("build", settings => settings.FromPath("./Intranet.Web"));
    DotNetCoreBuild(webDir, buildSettings);

    foreach(var project in unitTestProjects)
    {
        Information("Building: {0}", project.GetFilenameWithoutExtension());
        DotNetCoreBuild(project.FullPath, buildSettings);
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var file in unitTestProjects)
    {
        Information("Running unit test from: {0}", file.GetFilenameWithoutExtension());
        DotNetCoreTest(file.FullPath, testSettings);
    }
});

Task("Generate-Documentation")
  .Does(() => DocFxBuild("../docfx.json"));

Task("Publish")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Generate-Documentation")
    .Does(() =>
{
    DotNetCorePublish("./Intranet.Web");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
