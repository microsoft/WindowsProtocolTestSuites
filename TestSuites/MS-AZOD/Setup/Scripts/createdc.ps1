#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Createdc.ps1
## Purpose:        Script to create domain controller.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows server 2012 and later
## 
##############################################################################

Param
(
    # The name of the XML file, indicating which environment you want to configure
    # For example:"MS-AZOD-SingleDomain-Lab.xml", "MS-AZOD-CrossDomain-Lab.xml", "RDPClient.xml",...
    # You could rename the xml file in use to the default name "setup.xml" manually or vice versa
    [string]$EnvXmlName    = "setup.xml"
)

# Function to Control Writing Information to the screen
Function Write-Log
{
    Param ([Parameter(ValueFromPipeline=$true)] $text	)

    $timeString = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
    $message = "[$timeString] $text"
    $osbuildnum= "" + [Environment]::OSVersion.Version.Major + "." + [Environment]::OSVersion.Version.Minor
    if([double]$osbuildnum -eq [double]"6.3")
    {
        # WinBlue issue: Start-Transcript cannot write the log printed out by Write-Host, as a workaround, use Write-output instead
        # Write-Output does not support color
        "$message" | Out-Host
    }
    else
    {
        Write-Host -NoNewline "$message`r`n" 
    }

}

#------------------------------------------------------------------------------------------
# Write a piece of information to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteInfo {
    Param(
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message,
    [string]$ForegroundColor = "White",
    [string]$BackgroundColor = "DarkBlue")

    # WinBlue issue: Start-Transcript cannot write the log printed out by Write-Host, as a workaround, use Write-output instead
    # Write-Output does not support color
    if([Double]$Script:HostOsBuildNumber -eq [Double]"6.3") {
        ((Get-Date).ToString() + ": $Message") | Out-Host
    }
    else {
        Write-Host ((Get-Date).ToString() + ": $Message") -ForegroundColor $ForegroundColor -BackgroundColor $BackgroundColor
    }
}

#------------------------------------------------------------------------------------------
# Write a piece of error message to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteError {
    Param (
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message,
    [switch]$Exit)

    Write-TestSuiteInfo -Message "[ERROR]: $Message" -ForegroundColor Red -BackgroundColor Black
    if ($Exit) {exit 1}
}

# Main body of the script
#=================================================

$InvocationPath  = Split-Path -Parent $MyInvocation.MyCommand.Definition

$logFile = $MyInvocation.MyCommand.Name + ".log"

# Start Logging
Start-Transcript -Path $logFile -Append -Force

# Get the XML file
$setupPath=$InvocationPath+"\"+$EnvXmlName
if(!(Test-Path $setupPath)) 
{
 Write-TestSuiteError "$EnvXmlName was not found.please check the name or path of the xml file is right or not." -Exit
}
[xml]$setup = Get-Content $setupPath

# Determine our Server
$name = (Get-WmiObject Win32_computerSystem).Name
$server = $setup.lab.servers.vm | where {$_.name -eq $name}
if([System.String]::IsNullOrEmpty($server.domain))
{   
     $domainName = "contoso.com"    
}
else
{
	$domainName = $server.domain
}

if([System.String]::IsNullOrEmpty($server.username ))
{
    if([System.String]::IsNullOrEmpty($setup.lab.core.username))
    {	
        $userName = "$domainName\administrator"
    }
    else
    {
        $userName = $setup.lab.core.username
        $userName = "$domainName\$userName"
    }
}
else
{
	$userName = "$domainName\"+$server.username
}

if([System.String]::IsNullOrEmpty($server.password))
{
    if([System.String]::IsNullOrEmpty($setup.lab.core.password))
    {	
        $adminPwd = "Password01!"
    }
    else
    {
        $adminPwd = $setup.lab.core.password
    }	

}
else
{
	$adminPwd = $server.password
}

pushd $InvocationPath

# Promote DC
Write-Log "Promoting this computer to DC." 
.\PromoteDomainController.ps1 -DomainName $domainName -AdminPwd $adminPwd

Write-Log "Setting auto logon." 
.\Set-AutoLogon.ps1 -Domain $domainName -Username $userName -Password $adminPwd