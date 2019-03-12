#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           Config-client01.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012 +
##
##############################################################################
Function ConfigDriverComputer
{
    $endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\MS-AZOD\OD-Endpoint"    
    $azodTestSuites = Get-ChildItem -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites\MS-AZOD-OD-*'
    $azodTestSuite = $azodTestSuites[0]
    $version = $azodTestSuite.Name.Substring(80, $azodTestSuite.Name.Length-80)

    $dataFile = "$endPointPath\$version\scripts\Config.xml"    
    $logPath = $env:SystemDrive
    $logFile = $MyInvocation.MyCommand.Name + ".log"
    $domainName = "contoso.com"
    $domainAdmin 	= "administrator"
    $domainAdminPwd 	= "Password01!"
    $capturePath = "c:\test\testlog\MA"

    if(Test-Path -Path $dataFile)
    {
        try
        {
	        [xml]$configFile = Get-Content -Path $dataFile
	        $logPath = $configFile.Parameters.LogPath
	        $logFile = $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"

	        $domainName = $configFile.Parameters.LocalRealm.DomainName
            $domainAdmin = $configFile.Parameters.LocalRealm.DomainAdministrator.UserName
            $domainAdminPwd = $configFile.Parameters.LocalRealm.DomainAdministrator.Password
            $capturePath = $configFile.Parameters.CapturePath
        }
        catch
        {
            .\Write-Info.ps1 "Failed to read data file $dataFile. Please check the file content. Error happened: $_.Exception.Message"
            return
        }
    }
    else
    {
	    .\Write-Info.ps1 "$dataFile not found. Will keep the default setting of all the test context info..."
    }

    # Search AD config files under test suite installation folder. 
    .\Write-Info.ps1 "Modify PTF configure file"
    $ptfconfigs = dir "$env:HOMEDRIVE\MicrosoftProtocolTests" -Recurse | where{$_.Name -eq "MS-AZOD_ODTestSuite.deployment.ptfconfig"}
    if($ptfconfigs.Length -eq 0)
    {
        .\Write-Info.ps1 "There is no PTF config file."
        return
    }
    foreach($ptfconfig in $ptfconfigs)
    {
        .\Write-Info.ps1 $ptfconfig.FullName
        $ApplicationServerName = $configFile.Parameters.LocalRealm.FileServers.FileServer.Name
        $ApplicationServerIP = $configFile.Parameters.LocalRealm.FileServers.FileServer.IP
        $CrossForestName = $configFile.Parameters.TrustRealm.DomainName
        $CrossForestApplicationServerName = $configFile.Parameters.TrustRealm.FileServers.FileServer.Name
        $CrossForestApplicationServerIP = $configFile.Parameters.TrustRealm.FileServers.FileServer.IP 
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "KdcDomainName" $domainName
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "KdcName" $configFile.Parameters.LocalRealm.DC.Name
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "KDCIP" $configFile.Parameters.LocalRealm.DC.IP
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "ApplicationServerName" $ApplicationServerName
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "ApplicationServerIP" $ApplicationServerIP
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "FQDNUncPath" "\\$ApplicationServerName.$domainName\AzodShare"
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "UncPath" "\\$ApplicationServerIP\AzodShare"
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "ClientComputerName" $configFile.Parameters.LocalRealm.Client.Name
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "ClientComputerIp" $configFile.Parameters.LocalRealm.Client.IP
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "CrossForestName" $CrossForestName
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "CrossForestDCName" $configFile.Parameters.TrustRealm.DC.Name
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "CrossForestDCIP" $configFile.Parameters.TrustRealm.DC.IP
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "CrossForestApplicationServerName" $CrossForestApplicationServerName
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "CrossForestApplicationServerIP" $configFile.Parameters.LocalRealm.Client.IP
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "CrossForestApplicationServerShareFolder" "\\$CrossForestApplicationServerName.$CrossForestName\AzodShare"
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "ScriptPath" "$endPointPath\$version\Scripts\"
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "ExpectedSequenceFilePath" "$endPointPath\$version\Source\OD\TestCode\TestSuite\ExpectFrames"
    }  

    # Enable Powershell Remoting
    Enable-PSRemoting -Force -ErrorAction SilentlyContinue
}

#----------------------------------------------------------------------------
# Start logging using start-transcript cmdlet
#----------------------------------------------------------------------------
$rootPath = Split-Path $MyInvocation.MyCommand.Definition -parent
Push-Location $rootPath 
$logFile =  "$rootPath\" + $MyInvocation.MyCommand.Name + ".log"
$dataFile = "$rootPath\Config.xml"
if(Test-Path -Path $dataFile)
{
    try
    {
        [xml]$configFile = Get-Content -Path $dataFile
	    $logPath = $configFile.Parameters.LogPath
        if(!(Test-Path -Path $logPath))
        {
            cmd /c mkdir $logPath 2>&1 | Write-Host
        }
	    $logFile = $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"
    }
    catch
    {
        .\Write-Info.ps1 "Read config file $dateFile failed, Exception: $_.Exception.Message.`n"
    }
}
.\Write-Info.ps1 "Use $logFile as log file. `n"

Start-Transcript -Path "$logFile" -Append -Force
	
ConfigDriverComputer

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript
