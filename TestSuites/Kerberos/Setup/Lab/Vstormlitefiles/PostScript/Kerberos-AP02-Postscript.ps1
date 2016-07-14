#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
#############################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Kerberos-AP02-Postscript.ps1
## Requirements:   Windows PowerShell 2.0
## Supported OS:   Windows Server 2012 or later versions
##
##############################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$env:Path += ";c:\temp;c:\temp\Scripts"
[string]$VMName = "Kerberos-OSS-AP02"

$WorkingPath = "c:\temp"
Push-Location $WorkingPath

# Parameters from the config file
$ParamArray = @{} 

# Get parameters
Write-Info.ps1 "Trying to get parameters from config file..." -ForegroundColor Yellow
GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
$ParamArray

# Set Network
Write-Info.ps1 "Setting network configuration..." -ForegroundColor Yellow
SetNetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] `
    -Gateway $ParamArray["gateway"] -DNS $ParamArray["dns"]

# Get domain account
Write-Info.ps1 "Trying to get the domain account" -ForegroundColor Yellow
$DomainParamArray = @{}
GetDomainControllerParameters.ps1 -DomainName $ParamArray["domain"] -RefParamArray ([ref]$DomainParamArray)

# Join Domain
Write-Info.ps1 "Joining the computer to domain" -ForegroundColor Yellow
JoinDomain.ps1 -Domain $DomainParamArray["domain"] -Username $DomainParamArray["username"] `
                 -Password $DomainParamArray["password"]

# Use domain administrator to logon
SetAutoLogon.ps1 -Domain $DomainParamArray["domain"] -User $DomainParamArray["username"] `
                   -Pwd $DomainParamArray["password"]
Sleep 5
Restart-Computer
