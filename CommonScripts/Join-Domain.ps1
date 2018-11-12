#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

##############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Join-Domain.ps1
## Purpose:        Join the machine to the specified domain.
## Version:        1.1 (26 June, 2008)
##          
##############################################################################

Param(
[string]$domainWorkgroup   = "Domain",          #Workgroup
[string]$domainName        = "contoso.com",
[string]$userName          = "Administrator", 
[string]$userPassword      = "Password01!",
[string]$testResultsPath   = "C:\Test\TestResults"
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$logFile = $testResultsPath + "\Join-Domain.ps1.log"
Start-Transcript $logFile -force

Write-Host "EXECUTING [Join-Domain.ps1]..." -foregroundcolor cyan
Write-Host "`$domainWorkgroup  = $domainWorkgroup"
Write-Host "`$domainName       = $domainName"
Write-Host "`$userName         = $userName"
Write-Host "`$userPassword     = $userPassword"
Write-Host "`$testResultsPath  = $testResultsPath"

#----------------------------------------------------------------------------
# If it requires a workgroup environment, exit
#----------------------------------------------------------------------------
if ($domainWorkgroup -eq "Workgroup")
{
    Write-Host "Do nothing, because test suite requires a workgroup environment."
    return
}

#----------------------------------------------------------------------------
# If it is already in a domain, exit
#----------------------------------------------------------------------------
if ($env:USERDNSDOMAIN -ne $null)
{
    Write-Host "This computer is already joined to a domain."
    return
}

#----------------------------------------------------------------------------
#Function: Show-ScriptUsage
#Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This script joins the machine to the specified domain."
    Write-host
    Write-host "Options:"
    Write-host
    Write-host "Domain Name: The name of the domain to which you want to join."
    Write-host "User Name  : The name of the User for connecting to the domain controller."
    Write-host "Password   : The password to use when connecting to the domain controller." 
    Write-host
    Write-host "Example:"
    Write-host "Join_Domain.ps1 mydomain.com administrator p@ssw0rd C:\Test\TestResults"
    Write-host
}

#----------------------------------------------------------------------------
#Verify help parameters
#----------------------------------------------------------------------------

if ($args[0] -match '-(\?|(h|(help)))')
{
    write-host 
    show-scriptusage 
    return
}

#----------------------------------------------------------------------------
#Check for required input parameters
#----------------------------------------------------------------------------
if (!$domainName -or !$userName -or !$userPassword )
{
    Write-host
    Write-host "Please check the input parameters!" -foregroundcolor Red
    Write-host 
    Show-ScriptUsage
    return
}

#--------------------------------------------------------------------------
# Concat the user name with domain name.
#--------------------------------------------------------------------------
$userName = $domainName + "\" + $userName

#--------------------------------------------------------------------------
#Flags to define domain joining options
#--------------------------------------------------------------------------
$JOIN_DOMAIN             = 1
$ACCT_CREATE             = 2                    
$ACCT_DELETE             = 4
$WIN9X_UPGRADE           = 16
$DOMAIN_JOIN_IF_JOINED   = 32
$JOIN_UNSECURE           = 64
$MACHINE_PASSWORD_PASSED = 128
$DEFERRED_SPN_SET        = 256
$INSTALL_INVOCATION      = 262144

#-----------------------------------------------------------------------
#Get the WMI object of the Win32_ComputerSystem class
#-----------------------------------------------------------------------
$objComputer = Get-WmiObject  -Class "Win32_ComputerSystem" 

#-----------------------------------------------------------------------
#Join to domain
#-----------------------------------------------------------------------

$retryCount = 0
WHILE($retryCount -lt 10)
{
	$JoinObj = $objComputer.JoinDomainOrWorkgroup($domainName, $userPassword, $userName, $Null, $JOIN_DOMAIN + $ACCT_CREATE )
	#-----------------------------------------------------------------------
	#Result logging
	#-----------------------------------------------------------------------
	Write-Host "Run WMI Query to verify the domain:"
	get-WmiObject win32_computersystem

	Write-Host "Checking return value from function of JoinDomainOrWorkgroup."
	if ($JoinObj.Returnvalue -eq 0)
	{
		Write-host "Successfully joined to the domian." -foregroundcolor Green
		$retryCount = 10
	}
	else
	{
		$retryCount++;
		$errorcode = $JoinObj.Returnvalue
        
        IF($errorcode -eq 2691){
            Write-host "Already joined to the domian." -foregroundcolor Green
		    $retryCount = 10
        }

		IF($retryCount -gt 10){
			throw  "Joining domain failed after retry 10 times!, Error code: $errorcode"
		}
		else{
            Write-Host  "Joining Domain Failed!, Error code: $errorcode" -foregroundcolor Yellow
            Write-Host  "Start sleep 180 seconds then try again, retry count: $retryCount" -foregroundcolor Yellow
			Start-Sleep -s 180
		}
	}
}

#----------------------------------------------------------------------------
# Update the autologon account
#----------------------------------------------------------------------------
Write-Host "Update autologon account to domain administrator"

$path="HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"
$name1="DefaultUserName"
$name2="AltDefaultUserName"
$name3="DefaultPassword"
$name4="AutoAdminLogon"
$name5="DisableCAD"

set-itemproperty -path $path -name $name1 -value "$userName"   
set-itemproperty -path $path -name $name2 -value "$userName"
set-itemproperty -path $path -name $name3 -value "$userPassword"
set-itemproperty -path $path -name $name4 -value 1

## here ensures the WinXP & Win2003 autologon
## use reg.exe add method 'cause set-itemproperty cmdlet uses type of REG_SZ only
cmd /c reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" /v $name5 /t REG_DWORD /d 1 /f   

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "EXECUTE [Join-Domain.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor yellow
Stop-Transcript

return 0
