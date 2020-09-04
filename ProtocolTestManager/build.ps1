Param(
    [string]$Configuration="Release",
    [string]$OutDir
)

Write-Host ======================================
Write-Host          Start to build PtmCli
Write-Host ======================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$OutDir
dotnet publish "$InvocationPath/PtmCli.sln" -c $Configuration -o $OutDir/Bin
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build FileServer test suite"
    exit 1
}

Write-Host ==========================================================
Write-Host          Build PtmCli successfully         
Write-Host ==========================================================