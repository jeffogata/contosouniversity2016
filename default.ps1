$script:project_config = "Release"

properties {

  Framework '4.5.1'

  $project_name = "ContosoUniversity2016"

  if(-not $version)
  {
    $version = "0.0.0.1"
  }

  $date = Get-Date

  $ReleaseNumber =  $version

  Write-Host "**********************************************************************"
  Write-Host "Release Number: $ReleaseNumber"
  Write-Host "**********************************************************************"

  $base_dir = Resolve-Path .

  $build_dir = "$base_dir\build"     
  $source_dir = "$base_dir\src"

  $db_scripts_dir = "$base_dir\src\DatabaseMigration"
  $roundhouse_dir = "$base_dir\tools\roundhouse"
  $roundhouse_output_dir = "$roundhouse_dir\output"
  $roundhouse_exe_path = "$roundhouse_dir\rh.exe"
  
  $connectionString = "server=localhost;database=ContosoUniversity2016;trusted_connection=true;"

  $application_name = "cu2016"
  $company = "Jeff Ogata"
}

task default -depends InitialPrivateBuild
task dev -depends DeveloperBuild
task udb -depends UpdateDatabase
task rdb -depends RebuildDatabase
task ? -depends help

task help {
   Write-Help-Header
   Write-Help-Section-Header "Comprehensive Building"
   Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
   Write-Help-For-Alias "dev" "Optimized for local dev; Most noteably UPDATES databases instead of REBUILDING"
   Write-Help-Section-Header "Database Maintence"
   Write-Help-For-Alias "udb" "Update the Database to the latest version (leave db up to date with migration scripts)"
   Write-Help-For-Alias "rdb" "Rebuild database to the latest version from scratch (useful while working on the schema)"
   Write-Help-Footer
   exit 0
}

task InitialPrivateBuild -depends Compile, RebuildDatabase

task RebuildDatabase {
  deploy-database "Rebuild" $connectionString $db_scripts_dir
}

task Compile -depends Clean { 
    #exec { & $nuget_exe restore $source_dir\$project_name.sln }
    exec { msbuild.exe /t:build /v:q /p:Configuration=$project_config /p:Platform="Any CPU" $source_dir\$project_name.sln }
}

task Clean {
    exec { msbuild /t:clean /v:q /p:Configuration=$project_config /p:Platform="Any CPU" $source_dir\$project_name.sln }
}

# --------------------------------------------------------------------------------------------------------------
# generalized functions added by Headspring for Help Section
# --------------------------------------------------------------------------------------------------------------

function Write-Help-Header($description) {
   Write-Host ""
   Write-Host "********************************" -foregroundcolor DarkGreen -nonewline;
   Write-Host " HELP " -foregroundcolor Green  -nonewline;
   Write-Host "********************************"  -foregroundcolor DarkGreen
   Write-Host ""
   Write-Host "This build script has the following common build " -nonewline;
   Write-Host "task " -foregroundcolor Green -nonewline;
   Write-Host "aliases set up:"
}

function Write-Help-Footer($description) {
   Write-Host ""
   Write-Host " For a complete list of build tasks, view default.ps1."
   Write-Host ""
   Write-Host "**********************************************************************" -foregroundcolor DarkGreen
}

function Write-Help-Section-Header($description) {
   Write-Host ""
   Write-Host " $description" -foregroundcolor DarkGreen
}

function Write-Help-For-Alias($alias,$description) {
   Write-Host "  > " -nonewline;
   Write-Host "$alias" -foregroundcolor Green -nonewline;
   Write-Host " = " -nonewline;
   Write-Host "$description"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions
# -------------------------------------------------------------------------------------------------------------

function deploy-database($action, $connectionString, $scripts_dir) {
    write-host "action: $action"
    write-host "connectionString: $connectionString"
    write-host "scripts_dir: $scripts_dir"

    $up_dir = "$scripts_dir\up"

    if ($action -eq "Rebuild"){
       exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 --silent -drop -o $roundhouse_output_dir }
    }

    exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 --up $up_dir --silent -o $roundhouse_output_dir --transaction }
}
