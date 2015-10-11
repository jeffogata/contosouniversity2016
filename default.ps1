$script:project_config = "Release"

properties {

  Framework '4.5.1'

  $project_name = "ContosoUniversity2016"
  $test_project_name = "ContosoUniversity.Tests"

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
  $test_dir = "$build_dir\test"  

  $nuget_exe = "$base_dir\tools\nuget\nuget.exe"

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

  $test_assembly_pattern = @("*Tests.dll")

  $application_name = "cu2016"
  $company = "Jeff Ogata"
}

task default -depends InitialPrivateBuild
task dev -depends DeveloperBuild
task rat -depends RunTests
task rdb -depends RebuildDatabases
task udb -depends UpdateDatabases
task ? -depends help

task help {
   Write-Help-Header
   Write-Help-Section-Header "Comprehensive Building"
   Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
   Write-Help-For-Alias "dev" "Optimized for local dev; Most noteably UPDATES databases instead of REBUILDING"
   Write-Help-Section-Header "Database Maintence"
   Write-Help-For-Alias "udb" "Update the Database to the latest version (leave db up to date with migration scripts)"
   Write-Help-For-Alias "rdb" "Rebuild database to the latest version from scratch (useful while working on the schema)"
   Write-Help-Section-Header "Running Tests"
   Write-Help-For-Alias "rat" "Run all tests"
   Write-Help-Footer
   exit 0
}

task InitialPrivateBuild -depends SetReleaseBuild, Compile, RebuildDatabases, RunTests
task DeveloperBuild -depends SetDebugBuild, Compile, UpdateDatabases, RunTests

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task RebuildDatabases {
  Deploy-Database "Rebuild" $dev_connection_string $db_scripts_dir "DEV"
  #Deploy-Database "Rebuild" $test_connection_string $db_scripts_dir "TEST"
}

task UpdateDatabases {
  Deploy-Database "Update" $dev_connection_string $db_scripts_dir "DEV"
  #Deploy-Database "Update" $test_connection_string $db_scripts_dir "TEST"
}

task Compile -depends Clean { 
    #exec { & $nuget_exe restore $source_dir\$project_name.sln }
    Exec { dnu restore }
    Exec { msbuild.exe /t:build /v:q /p:Configuration=$project_config /p:Platform="Any CPU" $source_dir\$project_name.sln }
}

task Clean {
    
    # clear the artifacts folder from prior builds or this one will fail when doing a 'dnu restore'
    # specifically, the 'ContosoUniversity.Tests' project fails on 'dnu restore' with the following message:
    #    "Errors in D:\Projects\ContosoUniversity2016\src\artifacts\bin\ContosoUniversity.Tests\Debug\app\project.json"
    #    "Unable to locate ContosoUniversity.Tests >= 1.0.0"

    Remove-Item $source_dir\artifacts -Force -Recurse
    Exec { msbuild /t:clean /v:q /p:Configuration=$project_config /p:Platform="Any CPU" $source_dir\$project_name.sln }
}

task RunTests { #, PokeTestDBConnectionString {
    Run-Tests
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

function Deploy-Database($action, $connectionString, $scripts_dir, $env) {
    Write-Host "action: $action"
    Write-Host "connectionString: $connectionString"
    Write-Host "scripts_dir: $scripts_dir"
    Write-Host "env: $env"

    $up_dir = "$scripts_dir\up"

    if ($action -eq "Rebuild"){
       Exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 --env $env --silent -drop -o $roundhouse_output_dir }
    }

    Exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 --env $env --up $up_dir --silent -o $roundhouse_output_dir --transaction }
}

function Run-Tests() {
   $source = "$source_dir\artifacts\bin\$test_project_name\$project_config"
   $include = @("*.exe","*.dll","*.config","*.pdb","*.sql","*.xlsx","*.csv")
   $destination = $test_dir

   # copy test assemblies
   Write-Host "Copy test assemblies from $source"
   If (!(Test-Path -Path $destination -PathType Container)) {
      New-Item -ItemType directory -Path $destination
   }
   Get-ChildItem $source -Include $include -Recurse | Copy-Item -Destination $destination

   # run tests
   # - not able to run Fixie tests at the moment
   # - also not able to run xunit tests against copied artifacts

   #Run-Fixie "ContosoUniversity.Tests.dll"
   Run-Xunit
}

function Run-Xunit() {
    Exec { dnx -p $source_dir\$test_project_name test }
}

function Run-Fixie ($test_assembly) {
   $assembly_to_test = $test_dir + "\" + $test_assembly
   $results_output = $result_dir + "\" + $test_assembly + ".xml"

   Write-Host "Running Fixie Tests in: $test_assembly"
   Write-Host "tools\fixie\Fixie.Console.exe $assembly_to_test --NUnitXml $results_output --TeamCity off"

   & tools\fixie\Fixie.Console.exe $assembly_to_test --NUnitXml $results_output --TeamCity off

   if ($lastexitcode -gt 0)
   {
       throw "{0} tests in $test_assembly failed." -f $lastexitcode
   }
   if ($lastexitcode -lt 0)
   {
       throw "$test_assembly run was terminated by a fatal error."
   }
}
