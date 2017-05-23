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
var apiDir = Directory("./Intranet.API/Intranet.API");
var apiDomainDir = Directory("./Intranet.API/Intranet.API.Domain");
var apiTestsDir = Directory("./Intranet.API/Intranet.API.UnitTests");
var webDir = Directory("./Intranet.Web/Intranet.Web");
var webTestsDir = Directory("./Intranet.Web/Intranet.Web.UnitTests");

var apiBuildDir = Directory(apiDir) + Directory("bin") + Directory(configuration);
var apiDomainBuildDir = Directory(apiDomainDir) + Directory("bin") + Directory(configuration);
var apiTestsBuildDir = Directory(apiTestsDir) + Directory("bin") + Directory(configuration);
var webBuildDir = Directory(webDir) + Directory("bin") + Directory(configuration);
var webTestsBuildDir = Directory(webTestsDir) + Directory("bin") + Directory(configuration);

var apiBuildDirs = new [] { apiBuildDir, apiDomainBuildDir, apiTestsBuildDir };
var apiSrcDirs = new [] { apiDir, apiDomainDir, apiTestsDir };
var webBuildDirs = new [] { webBuildDir, webTestsBuildDir };
var webSrcDirs = new [] { webDir, webTestsDir };


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
    foreach (var dir in apiBuildDirs)
    {
        CleanDirectory(dir);
    };
});

Task("Web:Clean")
    .Does(() =>
{
    foreach (var dir in webBuildDirs)
    {
        CleanDirectory(dir);
    };
});

Task("API:Restore-NuGet-Packages")
    .IsDependentOn("API:Clean")
    .Does(() =>
{
    foreach (var dir in apiSrcDirs)
    {
        DotNetCoreRestore(dir);
    };
});

Task("Web:Restore-NuGet-Packages")
    .IsDependentOn("Web:Clean")
    .Does(() =>
{
    foreach (var dir in webSrcDirs)
    {
        DotNetCoreRestore(dir);
    };
});

Task("API:Build")
    .IsDependentOn("API:Restore-NuGet-Packages")
    .Does(() =>
{
    foreach (var dir in apiSrcDirs)
    {
        DotNetCoreBuild(dir, buildSettings);
    };
});

Task("Web:Build")
    .IsDependentOn("Web:Restore-NuGet-Packages")
    .Does(() =>
{
    foreach (var dir in webSrcDirs)
    {
        DotNetCoreBuild(dir, buildSettings);
    };
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