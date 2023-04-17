$ErrorActionPreference = "Stop";

# Reserved variables
$BuildNumber    ??= 0;
$BuildVersion   ??= '0.0.0';
$BuildVersionId ??= 'Gold';

# User variables
$Project = 'Maid.sln';

# User code
dotnet test @(
    "$Project"
    '-s'
    './.runsettings'
);

if ($LASTEXITCODE -ne 0) {
    throw;
}

dotnet pack @(
    "$Project"
    "/p:Version=$BuildVersion"
    '/p:Configuration=Release'
);

if ($LASTEXITCODE -ne 0) {
    throw;
}

# dotnet nuget push