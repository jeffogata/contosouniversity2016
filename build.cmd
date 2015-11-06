@echo off

rem %~dp0 expands into the directory of the current file
cd %~dp0

rem SETLOCAL sets context of changes to environment values to be local to this script
SETLOCAL
	
SET NUGET_VERSION=latest
SET CACHED_NUGET=%LocalAppData%\NuGet\nuget.%NUGET_VERSION%.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/%NUGET_VERSION%/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
IF EXIST packages\Sake goto getdnx
.nuget\NuGet.exe install Sake -ExcludeVersion -Source https://www.nuget.org/api/v2/ -Out packages

:getdnx
echo Installing DNX - 1.0.0-beta8 clr x86
CALL dnvm install 1.0.0-beta8 -runtime CLR -arch x86 -alias default

:build
echo Build...
packages\Sake\tools\Sake.exe -f makefile.shade