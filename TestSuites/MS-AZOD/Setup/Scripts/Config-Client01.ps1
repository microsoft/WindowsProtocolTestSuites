#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Sripting
## File:           Config-client01.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012 +
##
##############################################################################
Param
(        
    [int]$Step = 1
)


#-------------------------------------------------------------------------------
# Function: Phase1
#-------------------------------------------------------------------------------

Function Phase1
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

    # Turn off firewall
    .\Write-Info.ps1 "Turning off the firewall"
    Turnoff-FireWall       

    # Change execution policy
    .\Write-Info.ps1 "Set-ExecutionPolicy Unrestricted."
    Set-ExecutionPolicy Unrestricted
   
    # Create folders for capture files and test logs
    .\Write-Info.ps1 "Create folders for logging and capture files." -ForegroundColor Yellow
    New-Item -Path $logPath -ItemType Directory -Force
    New-Item -Path $capturePath -ItemType Directory -force    
    
    # Autologon config
    Autologon -Domain $domainName -Username $domainAdmin -Password $domainAdminPwd

    #-----------------------------------------------------------------------------------------------
    # Enable Claims 	
    #-----------------------------------------------------------------------------------------------
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos /f 2>&1 | Write-Host
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /f 2>&1 | Write-Host
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f 2>&1 | Write-Host
    REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v Supportedencryptiontypes /t REG_DWORD /d 0x7fffffff /f 2>&1 | Write-Host
        
    # Join Domain
    .\Write-Info.ps1 "Joining the computer to domain"
    JoinDomain  -Domain $domainName -Username $domainAdmin -Password $domainAdminPwd   
}
   

#-------------------------------------------------------------------------------
# Function: Turnoff-FireWall
# Usage  : Turn off windows firewall
#-------------------------------------------------------------------------------
Function Turnoff-FireWall()
{
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Host
}

#-----------------------------------------------------------------------------
# Function: JoinDomain, try several times if join domain failed.
# Usage   : Join the computer to a domain.
# Params  : [string]$domain  : The name of the domain to join.
#           [string]$username: The user name with the permission to join the domain.
#           [string]$password: The password for the username.
# Remark  : A reboot is needed after joining the domain.
#-----------------------------------------------------------------------------
Function JoinDomain(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]    
    [string]$Domain,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()] 
    [string]$Username,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()] 
    [string]$Password
)
{
    $succeed = $false
	$triedCount = 0
	while(!$succeed -and $triedCount -lt 10)
	{
		$triedCount++
		try
		{
			$credential = New-Object System.Management.Automation.PSCredential `
							-ArgumentList "$domain\$username", (ConvertTo-SecureString $password -AsPlainText -Force) `
							-ErrorAction Stop		
            .\Write-Info.ps1 "Start to join domain $Domain, Credential: $Username, $Password."           
			Add-Computer -Credential $credential -DomainName $domain -Restart:$false -Force -ErrorAction Stop
			$succeed = $true
		}
		catch
		{
			.\Write-Info.ps1 "Failed to join domain. Error happened: " + $_.Exception.Message            
			$succeed = (gwmi win32_computersystem).partofdomain
            if(!$succeed)
            {
                Sleep 5
            }
		}
	}
}
#-----------------------------------------------------------------------------------------------
# Set Autologon user, domain and password.
#-----------------------------------------------------------------------------------------------
Function Autologon(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]    
    [string]$Domain,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()] 
    [string]$Username,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()] 
    [string]$Password
)
{
    .\Write-Info.ps1 "set autologon account"    

    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v AutoAdminLogon /t REG_SZ /d 1 /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v AutoAdminLogon /t REG_SZ /d 1 /f 2>&1 | Write-Host

    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultDomainName /t REG_SZ /d  $Domain /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultDomainName /t REG_SZ /d  $Domain /f 2>&1 | Write-Host

    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultUserName /t REG_SZ /d $Username /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultUserName /t REG_SZ /d $Username /f 2>&1 | Write-Host

    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultPassword /t REG_SZ /d $Password /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultPassword /t REG_SZ /d $Password /f 2>&1 | Write-Host
}

#-----------------------------------------------------------------------------
# Function: RestartAndResume
# Usage   : Restart the computer and run the specified script.
#-----------------------------------------------------------------------------
Function RestartAndResume(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [String]$ScriptPath,
        
        [Parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [String]$PhaseIndicator,

        [Parameter(Mandatory=$false)]
        [String]$ArgumentList,

        [Parameter(Mandatory=$false)]
        [bool]$AutoRestart = $false)   
{        

    # Only absolute path is accepted, because relative path may cause trouble after rebooting.
    if([System.IO.Path]::IsPathRooted($ScriptPath) -eq $False)
    {
        throw "Argument ScriptPath must be absolute path"
    }

    $private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
    $private:regKeyName = "TKFRSAR"

    # If the key has already been set, remove it
    if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
    {
	    Remove-ItemProperty -Path $regRunPath -Name $regKeyName
    }

    try
    {
        Set-ItemProperty -Path $regRunPath -Name $regKeyName `
                            -Value "cmd /c powershell $ScriptPath $PhaseIndicator $ArgumentList" `
                            -Force -ErrorAction Stop
    }
    catch
    {
        throw "Unable to restart. Error happened: $_.Exception.Message"
    }

    .\Write-Info.ps1 "The computer is going to restart..." -ForegroundColor Yellow
    if($AutoRestart -eq $false) 
    { 
        # Waiting for key stroke
        Pause
    } 

    # Restart
    Restart-Computer
    
}

Function Phase2
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
        .\Modify-ConfigFileNode.ps1 $ptfconfig.FullName "ExpectedSequenceFilePath" "$endPointPath\$version\Bin"
    }  

    # Enable Powershell Remoting
    Enable-PSRemoting -Force -ErrorAction SilentlyContinue
}

Function Finish
{    
    # Clean up the registry entry after calling RestartAndRun.ps1.
    $private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
    $private:regKeyName = "TKFRSAR"

    if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
    {
	    Remove-ItemProperty -Path $regRunPath -Name $regKeyName
    }

	# Finished to config Terminal Client
	.\Write-Info.ps1 "Write signal file: config.finished.signal to system drive."
	cmd /C ECHO CONFIG FINISHED>$env:HOMEDRIVE\config.finished.signal

    # Stop logging
    Stop-Transcript
}

#-------------------------------------------------------------------------------
# Function: Main
# Remark  : Main script starts here.
#-------------------------------------------------------------------------------
Function Main()
{	
    switch($Step)
    {
        1 {
	        
            Phase1
            $invocation = (get-variable MyInvocation -Scope 1).Value            
            $NextStep =$step+1
            RestartAndResume -ScriptPath  $invocation.MyCommand.Path `
                        -PhaseIndicator "-Step $NextStep" -AutoRestart:$true
        }
        2 {
            Phase2
            Finish
        }
    }
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
	
Main
