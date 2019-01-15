#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-----------------------------------------------------
# Function: Set-StartupScript
# Usage   : Set startup script in Group Policy
#-----------------------------------------------------
Param
(
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$Script
)

Write-Host "Run script for the first time..." -ForegroundColor Yellow
& $Script

Write-Host "Set Group Policy for Startup Script at every startup..." -ForegroundColor Yellow
cmd /c regedit /s .\addStartupScript.reg
$regVal = $Script
$regKey = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Startup\0\0"
if(Test-Path -path $regKey) 
{
    Set-ItemProperty -path $regKey -name Script -value $regVal
}
$regKey = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\State\Machine\Scripts\Startup\0\0"
if(Test-Path -path $regKey) 
{
    Set-ItemProperty -path $regKey -name Script -value $regVal
}
$regKey = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Startup\0\0"
if(Test-Path -path $regKey) 
{
    Set-ItemProperty -path $regKey -name Script -value $regVal
}
$regKey = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Group Policy\State\Machine\Scripts\Startup\0\0"
if(Test-Path -path $regKey) 
{
    Set-ItemProperty -path $regKey -name Script -value $regVal
}