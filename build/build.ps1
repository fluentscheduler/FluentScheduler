param (
	[string]$BuildVersionNumber=$(throw "-BuildVersionNumber is required."),
	[string]$TagVersionNumber
)

# set version number in package.json
Get-ChildItem -Path $PSScriptRoot\..\src -Filter project.json -Recurse | ForEach-Object{ 
    $ProjectJsonPath =  $_.FullName
    if ($TagVersionNumber){
        (gc -Path $ProjectJsonPath) `
	        -replace "(?<=`"version`":\s`")[.\w-]*(?=`",)", "$TagVersionNumber" |
	            sc -Path $ProjectJsonPath -Encoding UTF8
    }
    else{
        (gc -Path $ProjectJsonPath) `
	        -replace "(?<=`"version`":\s`")[.\w-]*(?=`",)", "$BuildVersionNumber" |
	            sc -Path $ProjectJsonPath -Encoding UTF8
    }
}

# run restore on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Get-ChildItem -Path $PSScriptRoot\..\src -Filter project.json -Recurse | ForEach-Object { & dotnet restore $_.FullName 2>1 }

# run pack on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Get-ChildItem -Path $PSScriptRoot\..\src -Filter project.json -Recurse | ForEach-Object { & dotnet pack $_.FullName -c Release 2>1 }
