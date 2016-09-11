# run restore on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Get-ChildItem -Path $PSScriptRoot\..\test -Filter project.json -Recurse | ForEach-Object { & dotnet restore $_.FullName 2>&1 }

# run tests
Get-ChildItem -Path $PSScriptRoot\..\test -Filter project.json -Recurse | ForEach-Object { & dotnet test -c Release $_.FullName 2>&1 }