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
  
  $dev_connection_string_name = "$project_name.ConnectionString"
  $test_connection_string_name = "$project_name.Tests.ConnectionString"

  $db_server = if ($env:db_server) { $env:db_server } else { "localhost" }
  $db_name = if ($env:db_name) { $env:db_name } else { "ContosoUniversity2016" }
  $test_db_name = if ($env:test_db_name) { $env:test_db_name } else { "$db_name.Tests" }

  $dev_connection_string = if(test-path env:$dev_connection_string_name) { (get-item env:$dev_connection_string_name).Value } else { "Server=$db_server;Database=$db_name;Trusted_Connection=True;MultipleActiveResultSets=true" }
  $test_connection_string = if(test-path env:$test_connection_string_name) { (get-item env:$test_connection_string_name).Value } else { "Server=$db_server;Database=$test_db_name;Trusted_Connection=True;MultipleActiveResultSets=true" }

  $application_name = "cu2016"
  $company = "Jeff Ogata"
}

task default -depends InitialPrivateBuild
task dev -depends DeveloperBuild
task udb -depends UpdateDatabases
task rdb -depends RebuildDatabases
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

task InitialPrivateBuild -depends SetReleaseBuild, Compile, RebuildDatabases
task DeveloperBuild -depends SetDebugBuild, Compile, UpdateDatabases

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task RebuildDatabases {
  deploy-database "Rebuild" $dev_connection_string $db_scripts_dir "DEV"
  deploy-database "Rebuild" $test_connection_string $db_scripts_dir "TEST"
}

task UpdateDatabases {
  deploy-database "Update" $dev_connection_string $db_scripts_dir "DEV"
  deploy-database "Update" $test_connection_string $db_scripts_dir "TEST"
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

function deploy-database($action, $connectionString, $scripts_dir, $env) {
    write-host "action: $action"
    write-host "connectionString: $connectionString"
    write-host "scripts_dir: $scripts_dir"
    write-host "env: $env"

    $up_dir = "$scripts_dir\up"

    if ($action -eq "Rebuild"){
       exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 --env $env --silent -drop -o $roundhouse_output_dir }
    }

    exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 --env $env --up $up_dir --silent -o $roundhouse_output_dir --transaction }
}
