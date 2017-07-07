#addin "NuGet.Core"
#addin "Cake.ExtendedNuGet"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
#addin "Cake.Npm"

var target = Argument("target", "Default");
var configuration =
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") :
    "Release";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var apiDir = Directory("./Intranet.API/Intranet.API");
var apiTestsDir = Directory("./Intranet.API/Intranet.API.UnitTests");

var webDir = Directory("./Intranet.Web/Intranet.Web");
var webTestsDir = Directory("./Intranet.Web/Intranet.Web.UnitTests");

Func<String, String> GetBuildDirectory = (dir) => Directory(dir) + Directory("bin") + Directory(configuration);

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

Task("API:Clean")
    .Does(() =>
{
    CleanDirectory(GetBuildDirectory(apiDir));
    CleanDirectory(GetBuildDirectory(apiTestsDir));
});

Task("Web:Clean")
    .Does(() =>
{
    CleanDirectory(GetBuildDirectory(webDir));
    CleanDirectory(GetBuildDirectory(webTestsDir));
});

Task("API:Restore-NuGet-Packages")
    .IsDependentOn("API:Clean")
    .Does(() =>
{
      DotNetCoreRestore(apiDir);
      DotNetCoreRestore(apiTestsDir);
});

Task("Web:Restore-NuGet-Packages")
    .IsDependentOn("Web:Clean")
    .Does(() =>
{
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
        Version = "1.0.0-alpha9-00171",
        Source = new string[] { imageSharpSource, "https://api.nuget.org/v3/index.json" }
      });

      DotNetCoreRestore(webDir);
      DotNetCoreRestore(webTestsDir);

      CleanDirectory(tempCachePath);
      DeleteDirectory(tempCachePath);
});

Task("API:Build")
    .IsDependentOn("API:Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(apiDir, buildSettings);
    DotNetCoreBuild(apiTestsDir, buildSettings);
});

Task("Web:Build")
    .IsDependentOn("Web:Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(webDir, buildSettings);
    NpmInstall(settings => settings.FromPath("./Intranet.Web/Intranet.Web").WithLogLevel(NpmLogLevel.Warn));
    DotNetCoreBuild(webTestsDir, buildSettings);
});

Task("API:Run-Unit-Tests")
    .IsDependentOn("API:Build")
    .Does(() =>
{
    DotNetCoreTest("./Intranet.API/Intranet.API.UnitTests/Intranet.API.UnitTests.csproj", testSettings);
});

Task("Web:Run-Unit-Tests")
    .IsDependentOn("Web:Build")
    .Does(() =>
{
    DotNetCoreTest("./Intranet.Web/Intranet.Web.UnitTests/Intranet.Web.UnitTests.csproj", testSettings);
    NpmRunScript("test", settings => settings.FromPath("./Intranet.Web/Intranet.Web"));
});

Task("API:Publish")
    .IsDependentOn("API:Run-Unit-Tests")
    .Does(() =>
{
    DotNetCorePublish("./Intranet.API/Intranet.API");
});

Task("Web:Publish")
    .IsDependentOn("Web:Run-Unit-Tests")
    .Does(() =>
{
    DotNetCorePublish("./Intranet.Web/Intranet.Web");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("API:Run-Unit-Tests")
    .IsDependentOn("Web:Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
