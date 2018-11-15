#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           GetVMNameByComputerName.ps1
## Purpose:        Get the hypervname of current vm from the test suite config file by the computer name.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012, Windows Server 2012 R2, Windows Server 2016, and later.
##
###########################################################################################

Param
(
    # The full path of the xml file for the Test Suite
    [String]$ConfigFile = ".\Protocol.xml" # Default config file for lab regression run   
)

# The VM Name that will be returned. Use the computer name by default
$VMName = $env:computername

# Build Number for the operating system of vm
$HostOsBuildNumber = ""+ [Environment]::OSVersion.Version.Major + "." + [Environment]::OSVersion.Version.Minor

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
    if([Double]$HostOsBuildNumber -eq [Double]"6.3") {
        ((Get-Date).ToString() + ": $Message") | Out-Host
    }
    else {
        Write-Host ((Get-Date).ToString() + ": $Message") -ForegroundColor $ForegroundColor -BackgroundColor $BackgroundColor
    }
}


#------------------------------------------------------------------------------------------
# Write a piece of warning message to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteWarning {
    Param (
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message,
    [switch]$Exit)

    Write-TestSuiteInfo -Message "[WARNING]: $Message" -ForegroundColor Yellow -BackgroundColor Black
    if ($Exit) {exit 1}
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

#------------------------------------------------------------------------------------------
# Write a piece of success message to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteSuccess {
    Param (
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message)

    Write-TestSuiteInfo -Message "[SUCCESS]: $Message" -ForegroundColor Green -BackgroundColor DarkBlue
}

#------------------------------------------------------------------------------------------
# Write a piece of step message to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteStep {
    Param (
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message)

    Write-TestSuiteInfo -Message "[STEP]: $Message" -ForegroundColor Yellow -BackgroundColor DarkBlue
}

Write-TestSuiteStep "Check if the XML configuration file exist or not."       
    

# If $ConfigFile is not found, prompt a list of choices for user to choose
if(!(Test-Path -Path $ConfigFile)) {
    Write-TestSuiteWarning "$ConfigFile file not found. "    
}
else {
    Write-TestSuiteSuccess "$ConfigFile file found."
}

# Read contents from the XML file
Write-TestSuiteStep "Read contents from the XML configuration file."
    
[xml]$content = Get-Content $ConfigFile -ErrorAction Stop

$ComputerName = $env:computername.ToLower()

try 
{    
    #Xpath is case sensitive, so make the name node as lower case when match the lower case computername
    $currentVM = $content.SelectSingleNode("//vm[translate(name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=`'$ComputerName`']")
    
    $VMName = $currentVM.hypervname    

    if([string]::IsNullOrWhiteSpace($VMName)) 
    {
        $ErrorMsg = "Cannot get the VMName with computer name " + $ComputerName
        Write-TestSuiteError $ErrorMsg 
    }

    $SuccessMsg = "VMName for the current VM is: " + $VMName +"."
    Write-TestSuiteSuccess $SuccessMsg
}
catch
{
    [String]$Emsg = "Unable to read VM name from protocol.xml. Error happened: " + $_.Exception.Message
    Write-TestSuiteError $Emsg    
}

return $VMName
