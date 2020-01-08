#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           UpdatePTFConfig.ps1
## Purpose:        Update PTFConfig file for MS-ADFSPIP test suite.
## Requirements:   Windows PowerShell 2.0
## Supported OS:   Windows 7 or later versions
##
##############################################################################
Param(
	[string]$WorkingPath = "C:\temp"     # Script working path
)
$ScriptsSignalFile = "$WorkingPath\updateptfconfig.finished.signal"
if (Test-Path -Path $ScriptsSignalFile)
{
    Write-Host "The script execution is complete." -foregroundcolor Red
    exit 0
}

#Check working path exists or not.
if(!(Test-Path -Path $WorkingPath))
{
    Write-Host "Error:'$WorkingPath' was not found,Please check the working path parameter you set is right or not."  -foregroundcolor Red
    Write-Host "Warning:This script was not executed completely because of the error above." -foregroundcolor Yellow
    exit 0
}

$ScriptPath= Split-Path $MyInvocation.MyCommand.Definition
Write-Host "Put current dir as $ScriptPath."
pushd $ScriptPath

[XML]$vmConfig = Get-Content "$WorkingPath\Protocol.xml"
$driverComputerSettting = $vmConfig.lab.servers.vm | where {$_.role -eq "driver"}
$dcComputerSettting = $vmConfig.lab.servers.vm | where {$_.role -eq "dc"}
$proxyComputerSettting = $vmConfig.lab.servers.vm | where {$_.role -eq "proxy"}

Function GetDriverOSVersion()
{
    $osVersion = "Win2012R2"
	$osObj = get-wmiobject win32_operatingsystem
    switch -Wildcard ($osObj.Version)
    {
        "5.1.2600" { $osVersion = "XP" }
        "5.1.3790" { $osVersion = "W2K3" }
        "6.1.6001" 
        {
            if($osObj.ProductType -eq 1)
            {
                $osVersion = "VISTA"
            }
            else
            {
                $osVersion = "Win2008"
            }
        }
        "6.1.7600"
        {
            if($osObj.ProductType -eq 1)
            {
                $osVersion = "Win7"
            }
            else
            {
                $osVersion = "Win2008R2"
            }
        }
        "6.1.7601"
        {
            if($osObj.ProductType -eq 1)
            {
                $osVersion = "Win7SP1"
            }
            else
            {
                $osVersion = "Win2008R2SP1"
            }
        }     
        "6.2.*"
        {
            if($osObj.ProductType -eq 1)
            {
                $osVersion = "Win8"
            }
            else
            {
                $osVersion = "Win2012"
            }
        }
        "6.3.*"
        {
            if($osObj.ProductType -eq 1)
            {
                $osVersion = "Win8.1"
            }
            else
            {
                $osVersion = "Win2012R2"
            }
         }
         "10.0.*"
         {
            if($osObj.ProductType -eq 1)
            {
                $osVersion = "Win10"
            }
            else
            {
                $osVersion = "Win2016"
            }
         }
    }
	return $osVersion;
}

Function ConfigPTFConfig
{
    $fullDomainName        = $dcComputerSettting.domain
    $domainName = $fullDomainName.Substring(0,$fullDomainName.IndexOf("."))
    $fullDomainUserName   = $domainName + "\" + $dcComputerSettting.username
    $domainAdminUserPwd    = $dcComputerSettting.password

    $proxyIp               = $proxyComputerSettting.ip
    $proxyName             = $proxyComputerSettting.name
    $proxyUserName         = $proxyComputerSettting.username
    $proxyUserPwd          = $proxyComputerSettting.password


    $driverName            = $driverComputerSettting.name
    $driverIP              = $driverComputerSettting.ip
    $driverUserName        = $driverComputerSettting.username
    $driverUserPwd         = $driverComputerSettting.password

    # Update deployment.ptfconfig
    
    [string]$testSettingPath = [System.IO.Directory]::GetFiles("$env:HOMEDRIVE\MicrosoftProtocolTests",`
                        "ClientLocal.TestSettings",[System.IO.SearchOption]::AllDirectories)
    $DepPtfConfigFolder = Split-Path $testSettingPath
    $DepPtfConfig = "$DepPtfConfigFolder\MS-ADFSPIP_ClientTestSuite.deployment.ptfconfig"
    write-host $DepPtfConfig

    Write-Host "Start to configure deployment.ptfconfig" -ForegroundColor Yellow
    .\TurnOff-FileReadonly.ps1 $DepPtfConfig

    if($fullDomainName -ne $null -and $fullDomainName -ne "")
    {
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "Domain" -nodeName "DomainName" -newContent $fullDomainName
        $fullAdfsDns = "adfs."+$fullDomainName
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "ADFS" -nodeName "AdfsDns" -newContent $fullAdfsDns
        $fullAdfsUrl = "https://" + $fullAdfsDns
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "ADFS" -nodeName "AdfsUrl" -newContent $fullAdfsUrl
        $webAppHostName = "webapp."+$fullDomainName
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "WebApp" -nodeName "ProxyHostName" -newContent $webAppHostName
        $app1Url = "https://"+$webAppHostName+"/fed1/"
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "WebApp" -nodeName "App1Url" -newContent $app1Url
        $app2Url = "https://"+$webAppHostName+"/fed2/"
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "WebApp" -nodeName "App2Url" -newContent $app2Url
        $appUserUpn = "adfsuser@"+$fullDomainName
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "WebApp" -nodeName "AppUserUpn" -newContent $appUserUpn
        $appUserName = $domainName + "\adfsuser"
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "WebApp" -nodeName "AppUserName" -newContent $appUserName
    }

    if($fullDomainUserName -ne $null -and $fullDomainUserName -ne "")
    {
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "Domain" -nodeName "Username" -newContent $fullDomainUserName
    }

    if($domainAdminUserPwd -ne $null -and $domainAdminUserPwd -ne "")
    {
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "Domain" -nodeName "Password" -newContent $domainAdminUserPwd
    }

    if($proxyIp -ne $null -and $proxyIp -ne "")
    {
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "SUT" -nodeName "SutIPAddress" -newContent $proxyIp
        $sutShareFolder = "\\"+$proxyIp+"\temp\"
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "Common" -nodeName "SutShareFolder" -newContent $sutShareFolder
    }

    if($proxyName -ne $null -and $proxyName -ne "" -and $fullDomainName -ne $null -and $fullDomainName -ne "")
    {
        $sutDns = $proxyName + "." + $fullDomainName
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "SUT" -nodeName "SutDns" -newContent $sutDns
    }

    if($proxyName -ne $null -and $proxyName -ne "" -and $proxyUserName -ne $null -and $proxyUserName -ne "")
    {
        $proxyFullUserName = $proxyName + "\" + $proxyUserName
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "SUT" -nodeName "Username" -newContent $proxyFullUserName
    }

    if($proxyUserPwd -ne $null -and $proxyUserPwd -ne "")
    {
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "SUT" -nodeName "Password" -newContent $proxyUserPwd
    }

    if($driverIP -ne $null -and $driverIP -ne "")
    {
        $driverShareFolder = "\\" + $driverIP + "\temp\"
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "Common" -nodeName "DriverShareFolder" -newContent $driverShareFolder
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "Common" -nodeName "RealADFSIP" -newContent $driverIP
    }

    #Update configure file, set IsWin2016 to false if it's in 2012R2 envrionment
    $DriverOSVersion = GetDriverOSVersion
	
    if($DriverOSVersion -in "Win2012R2","Win8.1")
    {
        .\Modify-ConfigGroupFileNode.ps1 -sourceFileName $DepPtfConfig -groupName "ADFS" -nodeName "IsWin2016" -newContent "false"
    }
}

ConfigPTFConfig