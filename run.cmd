@echo off
cd %~dp0/src/ContosoUniversity
start "" dnx web
timeout 5
start "" http://localhost:5000

