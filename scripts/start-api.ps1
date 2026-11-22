$ErrorActionPreference = 'Stop'
Set-Location (Resolve-Path "$PSScriptRoot/..")
dotnet run --project src/AddressBook.Api --launch-profile http
