$ErrorActionPreference = 'Stop'
Set-Location (Resolve-Path "$PSScriptRoot/..")
dotnet run --project src/AddressBook.Web --launch-profile http
