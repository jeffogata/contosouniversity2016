@echo off
cd %~dp0/src/ContosoUniversity

rem : starting dnx using PS because when run in the same session as dnx was installed, the Path will not be refreshed to include the
rem : path to dnx.  i can set the path in the powershell command, but that is only visible within the command

powershell -command "set-item -path env:Path -value $([System.Environment]::GetEnvironmentVariable('Path','Machine')+ ';' + [System.Environment]::GetEnvironmentVariable('Path','User')); start-process dnx web"
timeout 5
start "" http://localhost:5000

