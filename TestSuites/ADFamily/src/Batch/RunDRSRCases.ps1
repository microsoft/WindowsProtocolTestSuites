
switch -Wildcard ([environment]::OSVersion.Version)
{
    "10.0.*"
    {
        powershell "$PSScriptRoot\RunDRSRCases_Threshold.ps1"
        return
    }
}
powershell "$PSScriptRoot\RunDrsrCases_Win8.1.ps1"
