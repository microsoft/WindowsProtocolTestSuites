#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-AP01.ps1
## Purpose:        Configure local realm AP for MS-AZOD test suite.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012 +
##
##############################################################################

# Input the amount of time you want to sleep
Function Sleep_Progress {
	Param($sleeptime)

	# Loop Number of seconds you want to sleep
	For ($i=0;$i -le $sleeptime;$i++){
		$timeleft = ($sleeptime - $i);
		# Progress bar showing progress of the sleep
		Write-Progress "Sleeping" "$Timeleft More Seconds" -PercentComplete (($i/$sleeptime)*100);
		If ($i -lt $sleeptime){sleep 1}
	}
}

Function ConfigFileServer
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
        
    if(Test-Path -Path $dataFile)
    {
        try
        {
	        [xml]$configFile = Get-Content -Path $dataFile
	        $logPath	= $configFile.Parameters.LogPath
	        $logFile	= $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"

	        $domainName 	= $configFile.Parameters.LocalRealm.DomainName
            $domainAdmin 	= $configFile.Parameters.LocalRealm.DomainAdministrator.UserName
            $domainAdminPwd 	= $configFile.Parameters.LocalRealm.DomainAdministrator.Password
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

    Update-FSRMClassificationpropertyDefinition

    # Create shares with RP and CAP
    if(Test-Path -Path $dataFile)
    {
	    [xml]$configFile = Get-Content -Path $dataFile
        foreach($fileserver in $configFile.Parameters.LocalRealm.FileServers.FileServer)
        {
            $fileServerName = $fileserver.Name
            $fileServerIp = $fileserver.IP
            $fileServerAdmin = $fileserver.admin
            $fileServerAdminPwd = $fileServer.adminPassword
            $shareRootPath = $fileserver.shares.SharesRootPath
    

            foreach ($share in $fileserver.shares.share)
            {
                
                $sharePath = $shareRootPath  + $share.Name
                $policy = $share.policy
                $rpName = $share.ResourcePropertyName
                $rpvalue = $share.ResourcePropertyValue

                # Create folder
                if(Test-Path -Path $sharePath)
                {
                    Remove-Item -Path $sharePath -force

                }

                .\Write-Info.ps1 "Create folder $sharePath."
                New-Item -Type Directory -Path $sharePath -Force

                # Share to everyone
                .\Write-Info.ps1 "Share folder $sharePath to every one"
                New-SmbShare -Name $share.Name -Path $sharePath -FullAccess Everyone

                # Apply RP
                if(($rpName -ne $null) -and ($rpvalue -ne $null))
                {
                    .\Write-Info.ps1 "Apply ResourceProperty $rpName to share $sharePath."
                    try
                    {                        
                        .\Write-Info.ps1 "Get-FsrmClassificationPropertyDefinition $rpName, "
                        $id = Get-FsrmClassificationPropertyDefinition $rpName
                        Write-Host "$id.Name"

                        .\Write-Info.ps1 "New-Object -ComObject Fsrm.FsrmClassificationManage"
                        $cls = New-Object -ComObject Fsrm.FsrmClassificationManage

                        .\Write-Info.ps1 "SetFileProperty"
                        $cls.SetFileProperty($sharePath,$id.Name,$rpvalue)
                    }
                    catch
                    {
                        .\Write-Info.ps1 "Failed to apply RP $rpName to share folder $sharePath. Error happened: $_.Exception.Message"
                    }
                }
        

                # Apply CAP  
                $isApplied = $false
                $i =0
                if($policy -ne $null)     
                {        
                    .\Write-Info.ps1 "Apply cap $policy to share $sharePath."
                    while(($isApplied -eq $false) -and ($i -lt 60))
                    {
                        try
                        {
                            .\Write-Info.ps1 "GetAccessControl for $sharePath."
                            $acl = (Get-Item $sharePath).GetAccessControl("Access")
                            .\Write-Info.ps1 "Set-Acl $sharePath $policy."
                            Set-Acl $sharePath $acl $policy
                            $isApplied = $true
                        }
                        catch
                        {
                            .\Write-Info.ps1 "Failed to apply CAP $policy to share folder $sharePath. Error happened: $_.Exception.Message"
                            .\Write-Info.ps1 "Will try again."   
                            $i++
                            Sleep_Progress 30                        
                        }
                    }
                }

            }

        }
    }
      

    #-----------------------------------------------------------------------------------------------
    # Enable Claims for this Realm
	# FAST is not enabled for initial environment. It will be enabled in test suite
    #-----------------------------------------------------------------------------------------------
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos /f 2>&1 | Write-Host
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /f 2>&1 | Write-Host
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f 2>&1 | Write-Host
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v Supportedencryptiontypes /t REG_DWORD /d 0x7fffffff /f 2>&1 | Write-Host
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
	
ConfigFileServer

# Stop logging
Stop-Transcript

