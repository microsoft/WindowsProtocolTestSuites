#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

$rootPath = Split-Path $MyInvocation.MyCommand.Definition -parent

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
Stop-Transcript -ErrorAction Continue | Out-Null
Start-Transcript -Path "$rootPath\Enable-USBRedirection.ps1.log" -Append -Force
#-------------------------------------
# Enable USB redirection.
#-------------------------------------
Write-Host "Enable USB redirection."

New-ItemProperty HKLM:\SOFTWARE\Policies\Microsoft\"Windows NT"\"Terminal Services"\Client fUsbRedirectionEnableMode -value 2 -PropertyType DWORD -Force

cmd /c rundll32.exe TsUsbRedirectionGroupPolicyExtension.dll,ExecuteProcessGroupPolicyEx

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript

exit 0