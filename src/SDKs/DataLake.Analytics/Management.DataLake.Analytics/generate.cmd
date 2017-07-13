::
:: Microsoft Azure SDK for Net - Generate library code
:: Copyright (C) Microsoft Corporation. All Rights Reserved.
::

@echo off
setlocal

if not "%1" == "" (set specsRepoUser="%1")
if not "%2" == "" (set specsRepoBranch="%2")
if "%specsRepoUser%" == ""   (set specsRepoUser="Azure")
if "%specsRepoBranch%" == "" (set specsRepoBranch="current")
set specFile1="https://github.com/%specsRepoUser%/azure-rest-api-specs/blob/%specsRepoBranch%/specification/datalake-analytics/resource-manager/readme.md"
set specFile2="https://github.com/%specsRepoUser%/azure-rest-api-specs/blob/%specsRepoBranch%/specification/datalake-analytics/data-plane/readme.md"

set autoRestVersion=1.2.0
set sdksRoot=%~dp0..\..

if "%3" == "" (call npm i -g autorest)
rd /S /Q %~dp0Generated

@echo on
call autorest %specFile1% --csharp --csharp-sdks-folder=%sdksRoot% --version=%autoRestVersion%
call autorest %specFile2% --csharp --csharp-sdks-folder=%sdksRoot% --version=%autoRestVersion% --package-catalog
call autorest %specFile2% --csharp --csharp-sdks-folder=%sdksRoot% --version=%autoRestVersion% --package-job

endlocal