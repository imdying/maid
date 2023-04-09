$ErrorActionPreference = "Stop";

# Reserved variables
$BuildNumber    ??= 0;
$BuildVersion   ??= '0.0.0';
$BuildVersionId ??= 'Gold';

# User variables
$cwd = $pwd.Path;

# User code
Write-Host "Hello World";