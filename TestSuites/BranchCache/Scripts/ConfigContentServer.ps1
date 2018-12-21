#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
try { Stop-Transcript -ErrorAction SilentlyContinue } catch {} # Ignore Stop-Transcript error messages
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Install the necessary features for ContentServer
#----------------------------------------------------------------------------
Import-Module ServerManager
Write-Info.ps1 "Install Windows features: FS-FileServer,FS-BranchCache,Storage-Services,BranchCache"
Add-Windowsfeature FS-FileServer,FS-BranchCache,Storage-Services,BranchCache -IncludeAllSubFeature -IncludeManagementTools

Write-Info.ps1 "Install Windows features: Web-Common-Http"
Add-Windowsfeature Web-Common-Http -IncludeAllSubFeature -IncludeManagementTools

Write-Info.ps1 "Install Windows features: Web-Stat-Compression,Web-Filtering,Web-Mgmt-Console"
Add-Windowsfeature Web-Stat-Compression,Web-Filtering

$osType = Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name InstallationType | Select-Object -ExpandProperty InstallationType
if($osType -ne "Server Core"){
    Add-Windowsfeature  Web-Mgmt-Console
}

#----------------------------------------------------------------------------
# Set the server secret
#----------------------------------------------------------------------------
Write-Info.ps1 "Set the server secret"
Netsh.exe branchcache set key "server secret" 2>&1 | Write-Info.ps1

Write-Info.ps1 "Restart service: peerdistsvc"
restart-service peerdistsvc

#----------------------------------------------------------------------------
# Enable SMB hash for branch cache for both V1 and V2
#----------------------------------------------------------------------------
Write-Info.ps1 "Enable SMB hash for branch cache for both V1 and V2 by adding registry keys."
reg add "HKLM\SOFTWARE\POLICIES\Microsoft\Windows\LanmanServer" /v HashPublicationForPeerCaching /t REG_DWORD /d 0x2 /f
reg add "HKLM\SOFTWARE\POLICIES\Microsoft\Windows\LanmanServer" /v HashSupportVersion /t REG_DWORD /d 0x3 /f

Write-Info.ps1 "Update group policy by gpupdate command."
gpupdate /target:computer /force 2>&1 | Write-Info.ps1


#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed config content server."
Stop-Transcript
exit 0