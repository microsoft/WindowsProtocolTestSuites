##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Get-OsVersion.ps1
## Purpose:        Get version of current operating system and log it to system drive.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

param(
[string]$computerName,
[string]$userName,
[string]$password,
[switch]$log
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Get-OsVersion.ps1] ..." -foregroundcolor cyan
Write-Host "`$computerName = $computerName"
Write-Host "`$userName     = $userName"
Write-Host "`$password     = $password"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: Get version of current operating system."
    Write-host "       Return value will be one of the following:"
    Write-host "       `"XP`", `"VISTA`", `"W2K3`", `"W2K8`", `"W2K8R2`", `"W2K12`", `"W2K12R2`", `$NULL if fail to get OS version."
    Write-host
    Write-host "Example: $osVer = Get-OsVersion.ps1 SUT01 Contoso.com\administrator Password01!"
    Write-host
}

#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ($computerName -eq $null -or $computerName -eq "")
{
    $computerName = $env:COMPUTERNAME
}

#----------------------------------------------------------------------------
# Using global username/password when caller doesnot provide.
#----------------------------------------------------------------------------
if ($computerName -ne $env:COMPUTERNAME)
{
    if ($userName -eq $null -or $userName -eq "")
    {
        $userName = $global:usr
        $password = $global:pwd
    }
}

#----------------------------------------------------------------------------
# Make username prefixed with domain/computername
#----------------------------------------------------------------------------
if ($computerName -ne $env:COMPUTERNAME)
{
    if ($userName.IndexOf("\") -eq -1)
    {
        if ($global:domain  -eq $null -or $global:domain -eq "")
        {
            $userName = "$computerName\$userName"
        }
        else
        {
            $userName = "$global:domain\$userName"
        }
    }
}

#----------------------------------------------------------------------------
# Convert the password to a SecureString
#----------------------------------------------------------------------------
if ($computerName -ne $env:COMPUTERNAME)
{
    $securePwd  = New-Object System.Security.SecureString
    for ($i = 0; $i -lt $password.Length; $i++)
    {
        $securePwd.AppendChar($password[$i]);
    }
    $credential = New-Object System.Management.Automation.PSCredential($userName, $securePwd) 
}

#----------------------------------------------------------------------------
# Wait the computer is started up
#----------------------------------------------------------------------------
if ($computerName -ne $env:COMPUTERNAME)
{
    $disconnectCmd = "net.exe use \\$computerName\IPC$ /delete /y      1>>netusesuc.tmp.log 2>>netuseerr.tmp.log"
    $connectCmd    = "net.exe use \\$computerName\IPC$ $password /User:$userName 1>>netusesuc.tmp.log 2>>netuseerr.tmp.log"
    cmd /c $disconnectCmd 
    cmd.exe /c $connectCmd
    if ($lastExitCode -ne 0)
    {
        Write-Host "$computerName is not started yet..."  -foregroundcolor Yellow
        cmd /c $disconnectCmd 
        .\WaitFor-ComputerReady.ps1 $computerName  $userName $password 
    }
    cmd /c $disconnectCmd 
}

#----------------------------------------------------------------------------
# Wait the computer RPCServer is online
#----------------------------------------------------------------------------
Write-Host "Try to connect to computer $computerName ..."
$waitTimeout = 600
$osObj = $null
$retryCount = 0
for (; $retryCount -lt $waitTimeout/2; $retryCount++ ) 
{
    if($computerName -ieq $env:COMPUTERNAME)
    {
        $osObj = get-wmiobject win32_operatingsystem
    }
    else
    {
        $osObj = get-wmiobject win32_operatingsystem -computer $computerName -Credential $credential 
    }
    if($osObj -ne $null)
    {
        break;  
    }
    
    $NoNewLineIndicator = $True
    if ( $retryCount % 60 -eq 59 )
    {
       $NoNewLineIndicator = $False
    }
    Write-host "." -NoNewLine:$NoNewLineIndicator -foregroundcolor White
    
    Start-Sleep -s 2  # Sleep for 2 seconds [System.Threading.Thread]::Sleep(2000)
}
if ($osObj -eq $null)
{
    Throw "Connect to computer $computerName failed."
}

Write-host "." -foregroundcolor Green
Write-Host "Connection to computer $computerName created."

#----------------------------------------------------------------------------
# Get domain or workgroup name
#----------------------------------------------------------------------------
if ($osObj -eq $null)
{
    Throw "Error: Cannot get WMI object."
}

$caption = $osObj.Caption
$version = $osObj.Version
$buildNum= $osObj.BuildNumber
$result  = $null
switch -Wildcard ($osObj.Version)
{
    "5.1.2600" { $result = "XP" }
    "5.1.3790" { $result = "W2K3" }
    "6.1.6001" 
    {
        if($osObj.ProductType -eq 1)
        {
            $result = "VISTA"
        }
        else
        {
            $result = "Win2008"
        }
    }
    "6.1.7600"
    {
        if($osObj.ProductType -eq 1)
        {
            $result = "Win7"
        }
        else
        {
            $result = "Win2008R2"
        }
    }
    "6.1.7601"
    {
        if($osObj.ProductType -eq 1)
        {
            $result = "Win7SP1"
        }
        else
        {
            $result = "Win2008R2SP1"
        }
    }     
    "6.2.*"
    {
        if($osObj.ProductType -eq 1)
        {
            $result = "Win8"
        }
        else
        {
            $result = "Win2012"
        }
    }
    "6.3.*"
    {
        if($osObj.ProductType -eq 1)
        {
            $result = "Win8.1"
        }
        else
        {
            $result = "Win2012R2"
        }
     }
    "10.0.*"
    {
        if($osObj.ProductType -eq 1)
        {
            $result = "Win10"
        }
        else
        {
            $result = "Win2016"
        }
     }
}

if($result -eq $null)
{
    Write-Host "Unrecognized OS version: $osObj.Version"
}

#----------------------------------------------------------------------------
# Print exit information
#----------------------------------------------------------------------------
Write-Host "OS version is: $result" -foregroundcolor Green
Write-Host "EXECUTE [Get-OSVersion.ps1] SUCCEED." -foregroundcolor Green

if($log)
{
    $result > "$env:SystemDrive\osversion.txt"
}

return $result
