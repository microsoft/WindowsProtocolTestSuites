#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-ENDPOINT.ps1
## Purpose:        Configure ENDPOINT for Active Directory test suites.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2
##
##############################################################################
Param
(
    [alias("h")][switch]$help,
    [string]$VMName = "AD_ENDPOINT", # The Virtual Machine's name
    [int]$Step      = 1,             # Current step for configuration
    [int]$IsADTS    = 0              # If this is for ADTS-Security, use client user account
)

if($help)
{
$helpmsg = @"
Post script to configure ENDPOINT.

Usage:
    .\Config-ENDPOINT.ps1 [-VMName <vmname>] [-Step <step>] [-h | -help]

VMName: The name of the VM to be created. The default value is AD_ENDPOINT.
Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg
    return
}

#-----------------------------------------------------------------------------
# Global function: Split file name and directory path from a full path
#-----------------------------------------------------------------------------
Function Get-SplitFileName([string]$FullPathName)
{
    $Pieces = $FullPathName.split("\") 
    $NumberOfPieces = $Pieces.Count 
    $FileName = $Pieces[$NumberOfPieces - 1] 
    $DirectoryPath = $FullPathName.Substring(0, $FullPathName.Length - $FileName.Length - 1)

    return $FileName, $DirectoryPath
}

#-----------------------------------------------------------------------------
# Global variables
#-----------------------------------------------------------------------------
$ScriptFullPath          = $MyInvocation.MyCommand.Definition                # Current Working Script Full Path
$ScriptName, $ScriptPath = Get-SplitFileName -FullPathName $ScriptFullPath   # Current Working Script Name
                                                                             # Current Working Script Path
$ScriptSignalFullPath    = "$ScriptFullPath.finished.signal"                 # Current Working Script Completion Signal File
$LogPath                 = "$ScriptPath"                                     # Current Working Script Log Path
$LogFile                 = "$LogPath\$ScriptName.log"                        # Current Working Script Log File
$ParamArray              = @{}                                               # Parameters from the config file

#-----------------------------------------------------------------------------
# Check signal file and switch to script path
#-----------------------------------------------------------------------------
Function Prepare(){

    Write-Log "Executing [$ScriptName] ..." -ForegroundColor Cyan

    # Check completion signal file. If signal file exists, exit with 0
    if(Test-Path -Path $ScriptSignalFullPath){
        Write-Log "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    Write-Log "Switching to $ScriptPath" -ForegroundColor Yellow
    Push-Location $ScriptPath
}

#-----------------------------------------------------------------------------
# Read Config Parameters
#-----------------------------------------------------------------------------
Function ReadConfig()
{
    Write-Log "Getting the parameters from config file ..." -ForegroundColor Yellow
    .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
    $ParamArray
}

#-----------------------------------------------------------------------------
# Create Log 
#-----------------------------------------------------------------------------
Function SetLog(){

    if(!(Test-Path -Path $LogPath)){
        New-Item -ItemType Directory -Path $LogPath -Force
    }

    if(!(Test-Path -Path $LogFile)){
        New-Item -ItemType File -path $LogFile -Force
    }
    Start-Transcript $LogFile -Append 2>&1 | Out-Null
}

Function Write-Log
{
    Param ([Parameter(ValueFromPipeline=$true)] $text,
    $ForegroundColor = "Green"
    )

    $date = Get-Date
    Write-Output "`r`n$date $text"
}

Function CheckReturnValue()
{
    if( -not $?)
    {
	    $vars = Get-Variable
        $date = Get-Date
        $line = $MyInvocation.ScriptLineNumber.ToString()
        Write-Output "`r`n$date Error in line $line."
        Write-Output "**********************"
        Write-Output "Dump local variables"
        Write-Output "**********************"
        Format-Table Name,Value -wrap -autosize -inputobject $vars
        Stop-Transcript
        throw "Error in line $line."
    }
}

#-----------------------------------------------------
# Function: Modify-PTFConfig
# Usage: Modify PTF configure file
#-----------------------------------------------------
Function Modify-PTFConfig(
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$PTFConfigPath,
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$nodeName,
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$value
    )
{
    Write-Log "Turn off Read-only arribute for $PTFConfigPath"
    Set-ItemProperty -Path $PTFConfigPath -Name IsReadOnly -Value $false

    Write-Log "Modify node: $nodeName to value: $value"
    [xml]$configContent = Get-Content $PTFConfigPath
    $PropertyNodes = $configContent.GetElementsByTagName("Property")
    foreach($node in $PropertyNodes)
    {
        if($node.GetAttribute("name") -eq $nodeName)
        {
            $node.SetAttribute("value", $value)
            $IsFindNode = $true
            break
        }
    }

    if($IsFindNode)
    {
        $configContent.Save($PTFConfigPath)
    }
    else
    {
        throw "Setting PTFConfig failed: Cannot find the node whose name attribute is $nodeName"
    }

    Write-Log "Turn on Read-only attribute for $PTFConfigPath"
    Set-ItemProperty -Path $PTFConfigPath -Name IsReadOnly -Value $true
}

#-----------------------------------------------------------------------------
# Phase1: SetNetworkConfiguration; JoinDomain
#-----------------------------------------------------------------------------
Function Phase1
{
    Write-Log "Entering Phase1..."

    # Set Network
    Write-Log "Setting network configuration" -ForegroundColor Yellow    
    .\SetNetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] -Gateway $ParamArray["gateway"] -DNS ($ParamArray["dns"].split(';'))
    Sleep 60
    
    # Turn off UAC
    Write-Log "Turn off UAC" -ForegroundColor Yellow
    set-itemproperty -path  HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System -name "EnableLUA" -value "0"
      
    # Set Auto Logon using domain admin account
    Write-Log "Set Auto Logon by Domain Admin"
    $domainuser = $ParamArray["username"]
    $userpassword = $ParamArray["password"]
    # Use client user account to run ADTS related test suites
    if($IsADTS)
    {
        $domainuser = $ParamArray["clientuser"]
        $userpassword = $ParamArray["userpassword"]   
    }
    .\SetAutoLogon.ps1 -domain $ParamArray["domain"] -user $domainuser -pwd $userpassword
    
    # Join Domain
    Write-Log "Join Domain" -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["primarydc"] -usr $ParamArray["username"] -pwd $ParamArray["password"]
    $workgroupDomain = "Domain"
    .\Join-Domain.ps1 $workgroupDomain $ParamArray["domain"] $ParamArray["username"] $ParamArray["password"] $LogPath 2>&1 | Write-Output  
}

Function Phase2
{
    Write-Log "Entering Phase2..."

    # Turn off firewall
    Write-Log "Turn off windows firewall"
    netsh advfirewall set allprofiles state off 2>&1 | Write-Output
        
    # Disable auto password change [MS-LSAT]
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon\Parameters /v DisablePasswordChange /t REG_DWORD /d 1 /f

    # Reset computer password [MS-LSAT][MS-NRPC] & set msDS-AdditionalDnsHostName [MS-ADTS-LDAP] 
	$remoteServer = $ParamArray["primarydc"]
    $domain   = $ParamArray["domain"]
    $username = $ParamArray["username"]
    $password = $ParamArray["password"]  
    $domainNC = "DC=" + $domain.ToString().Replace(".", ",DC=")
        
    Write-Log "Set computer password"
	ksetup /SetComputerPassword $ParamArray["temppassword"]    
    
    $cred=New-Object System.Management.Automation.PSCredential -ArgumentList "$domain\$username", (ConvertTo-SecureString "$password" -AsPlainText -Force) -ErrorAction Stop	

    Invoke-Command -ComputerName $remoteServer -Credential $cred -ScriptBlock {
        Param($hostName, $domainNC, $tempPassword, $WorkingPath)
		$dcADSI=[ADSI]"LDAP://CN=$hostName,CN=Computers,$domainNC"
		$dcADSI.SetPassword($tempPassword)
		$dcADSI.SetInfo()
        
        & "$WorkingPath\Config-PDC.ps1" -Step 4
	} -ArgumentList $ParamArray["name"], $domainNC, $ParamArray["temppassword"], $ScriptPath

    # Copy and install cert from PDC [MS-ADTS-Security]
    $rHOMEDRIVE = $env:HOMEDRIVE -replace ":","$"
    $DCComputerName = $ParamArray["primarydc"]
    xcopy \\$DCComputerName\$rHOMEDRIVE\$DCComputerName.CER $ScriptPath\ /y
    Write-Log "Import cert from PDC"
    certutil.exe -addstore "MY" "$ScriptPath\$DCComputerName.CER" 2>&1 | Write-Output
    certutil.exe -addstore root "$ScriptPath\$DCComputerName.CER" 2>&1 | Write-Output
    CheckReturnValue     
}

Function Phase3
{
    Write-Log "Entering Phase3..."	

    # Reset computer password
	$remoteServer = $ParamArray["primarydc"]
    $domain   = $ParamArray["domain"]
    $username = $ParamArray["username"]
    $password = $ParamArray["password"]  
    $domainNC = "DC=" + $domain.ToString().Replace(".", ",DC=")

    Write-Log "Set computer password again"
    ksetup /SetComputerPassword $password

    # Set ENDPOINT password on PDC [MS-LSAT] and restart DFSR on PDC [MS-FRS2]
    Write-Log "Setting ENDPOINT password and restart DFSR on PDC" 
    $cred=New-Object System.Management.Automation.PSCredential -ArgumentList "$domain\$username", (ConvertTo-SecureString $password -AsPlainText -Force) -ErrorAction Stop	
	Invoke-Command -ComputerName $remoteServer -Credential $cred -ScriptBlock {
        Param($hostName, $domainNC, $passWord)
		$dcADSI=[ADSI]"LDAP://CN=$hostName,CN=Computers,$domainNC"
		$dcADSI.SetPassword($password)
		$dcADSI.SetInfo()
        Restart-Service DFSR
	} -ArgumentList $ParamArray["name"], $domainNC, $password
    
    # Remotely restart DFSR service on SDC if SDC exists [MS-FRS2]
    [xml]$content = Get-Content ".\Protocol.xml" -ErrorAction Stop
    $sdcNode = $content.SelectSingleNode("//vm[hypervname=`'AD_SDC`']")
    if($sdcNode.HasChildNodes -and $ParamArray.ContainsKey("secondarydc"))
    {
        Write-Log "Restarting DFSR"
        $sdcServer = $ParamArray["secondarydc"]
        Invoke-Command -ComputerName $sdcServer -Credential $cred -ScriptBlock {
            Restart-Service DFSR
	    }   
    } 
}

Function Phase4
{
    Write-Log "Entering Phase4..."	
   
    ##########################################################
    # Update PTFConfig - PDC
    ##########################################################
    $rHOMEDRIVE = $env:HOMEDRIVE -replace ":","$"
    $DCComputerName = $ParamArray["primarydc"]
    # Search AD config files under test suite installation folder. 
    Write-Log "Modify PTF configure file"
    $ptfconfigs = dir "$env:HOMEDRIVE\MicrosoftProtocolTests" -Recurse | where{$_.Name -eq "AD_ServerTestSuite.deployment.ptfconfig"}
    if($ptfconfigs.Length -eq 0)
    {
        Write-Log "There is no PTF config file."
        return
    }
    foreach($ptfconfig in $ptfconfigs)
    {
        Write-Log $ptfconfig.FullName
    }   
    # update PTFconfig with LDS ports [MS-ADTS-Security][MS-ADTS-LDAP][MS-ADTS-Schema]    
    $ldsldapport = Get-Content -Path "\\$DCComputerName\$rHOMEDRIVE\port.txt" -ErrorAction Stop
    Write-Log "LDS port for MS-ADTS-Security is $ldsldapport"
    $ldssslport = Get-Content -Path "\\$DCComputerName\$rHOMEDRIVE\sslport.txt" -ErrorAction Stop
    Write-Log "LDS SSL port for MS-ADTS-Security is $ldssslport"
    foreach($ptfconfig in $ptfconfigs)
    {
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "ADLDSPortNum" -value $ldsldapport
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "ADLDSSSLPortNum" -value $ldssslport
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "certFilewithPathSpec" -value "$ScriptPath\$DCComputerName.CER"
    }   
    # update PTFconfig with OSVersion, DomainFunctionLevel, DCFunctionLevel [MS-ADTS-LDAP]
    $domainfunctionallv = Get-Content -Path "\\$DCComputerName\$rHOMEDRIVE\domainfunctionallv.txt" -ErrorAction Stop
    $osversion = Get-Content -Path "\\$DCComputerName\$rHOMEDRIVE\osversion.txt" -ErrorAction Stop 
    Write-Log "OS Version for PDC is $osversion, Domain Functional Level is $domainfunctionallv"
    foreach($ptfconfig in $ptfconfigs)
    {
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "WritableDC1.OSVersion" -value $osversion
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "DomainFunctionLevel" -value $domainfunctionallv
    }
    # update PTFconfig with ServerGUID, SID [MS-ADTS-PublishDCScenario][MS-ADTS-LSAD]
    $SRVGuidForm = Get-Content -Path "\\$DCComputerName\$rHOMEDRIVE\SrvGuidForm.txt" -ErrorAction Stop
    Write-Log "Domain Server GUID: $SRVGuidForm"
    # Get Primary Domain SID        
    $domain   = $ParamArray["domain"]
    $username = $ParamArray["username"]
    $objUser = New-Object System.Security.Principal.NTAccount($domain, $userName) 
    $strSID  = $objUser.Translate([System.Security.Principal.SecurityIdentifier])
    $PrimaryDomainSidValue = $strSID.Value       
    $PrimaryDomainSidValue = $PrimaryDomainSidValue.Replace("-500","") 
    Write-Log "Domain Sid: $PrimaryDomainSidValue"

    foreach($ptfconfig in $ptfconfigs)
    {
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "PrimaryDomain.ServerGUID" -value $SRVGuidForm
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "PrimaryDomain.SID" -value $PrimaryDomainSidValue
    }

    # update PTFconfig with TD XML paths [MS-ADTS-Schema]
    foreach($ptfconfig in $ptfconfigs)
    {
        if($ptfconfig.DirectoryName.Contains("Server\TestCode\TestSuite"))
        {
            $ParentPath = $ptfconfig.DirectoryName.Replace("\Source\Server\TestCode\TestSuite","")
            $TDXmlPath = "$ParentPath\Docs\Common-TD-XML\MS-ADA1\*,$ParentPath\Docs\Common-TD-XML\MS-ADA2\*,$ParentPath\Docs\Win8-TD-XML\MS-ADA2\*,$ParentPath\Docs\Common-TD-XML\MS-ADA3\*,$ParentPath\Docs\Common-TD-XML\MS-ADSC\*,$ParentPath\Docs\Win8-TD-XML\MS-ADSC\*"
            $LdsTDXmlPath = "$ParentPath\Docs\Common-TD-XML\MS-ADLS\*,$ParentPath\Docs\Win8-TD-XML\MS-ADLS\* "
            $OpenXmlPath2016 = "$ParentPath\Docs\Win2016-TD-XML\DS\*"
            $LdsOpenXmlPath2016 = "$ParentPath\Docs\Win2016-TD-XML\LDS\*"
        }
        else
        {
            $ParentPath = $ptfconfig.DirectoryName.Replace("\Bin","")
            $TDXmlPath = "$ParentPath\Docs\Common-TD-XML\MS-ADA1\*,$ParentPath\Docs\Common-TD-XML\MS-ADA2\*,$ParentPath\Docs\Win8-TD-XML\MS-ADA2\*,$ParentPath\Docs\Common-TD-XML\MS-ADA3\*,$ParentPath\Docs\Common-TD-XML\MS-ADSC\*,$ParentPath\Docs\Win8-TD-XML\MS-ADSC\*"
            $LdsTDXmlPath = "$ParentPath\Docs\Common-TD-XML\MS-ADLS\*,$ParentPath\Docs\Win8-TD-XML\MS-ADLS\* "
            $OpenXmlPath2016 = "$ParentPath\Docs\Win2016-TD-XML\DS\*"
            $LdsOpenXmlPath2016 = "$ParentPath\Docs\Win2016-TD-XML\LDS\*"
        }
        if($domainfunctionallv -ge "6")
        {
            $TDXmlPath = $TDXmlPath.Replace("Win8","WinBlue")
            $LdsTDXmlPath = $LdsTDXmlPath.Replace("Win8","WinBlue")
        }
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "TDXmlPath" -value $TDXmlPath
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "LdsTDXmlPath" -value $LdsTDXmlPath 
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "OpenXmlPath2016" -value $OpenXmlPath2016
        Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "LdsOpenXmlPath2016" -value $LdsOpenXmlPath2016
    }
       	
    ##########################################################
    # Update PTFConfig - SDC, RODC, CDC, TDC
    ##########################################################
    $DCComputerName = $ParamArray["secondarydc"]
    if([System.IO.File]::Exists("\\$DCComputerName\$rHOMEDRIVE\osversion.txt"))
    {
        $osversionSDC = Get-Content -Path "\\$DCComputerName\$rHOMEDRIVE\osversion.txt" -ErrorAction Stop 
        Write-Log "OS Version for $DCComputerName is $osversionSDC"
        foreach($ptfconfig in $ptfconfigs)
        {
            Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "WritableDC2.OSVersion" -value $osversionSDC
        }
    }
    $DCComputerName = $ParamArray["readonlydc"]
    if([System.IO.File]::Exists("\\$DCComputerName\$rHOMEDRIVE\osversion.txt"))
    {
        $osversionRODC = Get-Content -Path "\\$DCComputerName\$rHOMEDRIVE\osversion.txt" -ErrorAction Stop 
        Write-Log "OS Version for RODC is $osversionRODC"
        foreach($ptfconfig in $ptfconfigs)
        {
            Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "RODC.OSVersion" -value $osversionRODC
        }  
    }
    $DCComputerIP = $ParamArray["childdc"]
    if([System.IO.File]::Exists("\\$DCComputerIP\$rHOMEDRIVE\osversion.txt"))
    {
        $osversionCDC = Get-Content -Path "\\$DCComputerIP\$rHOMEDRIVE\osversion.txt" -ErrorAction Stop 
        Write-Log "OS Version for CDC is $osversionCDC"
        foreach($ptfconfig in $ptfconfigs)
        {
            Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "CDC.OSVersion" -value $osversionCDC
        }  
    }
    $DCComputerIP = $ParamArray["trustdc"]
    if([System.IO.File]::Exists("\\$DCComputerIP\$rHOMEDRIVE\osversion.txt"))
    {
        $osversionTDC = Get-Content -Path "\\$DCComputerIP\$rHOMEDRIVE\osversion.txt" -ErrorAction Stop 
        Write-Log "OS Version for TDC is $osversionTDC"
        foreach($ptfconfig in $ptfconfigs)
        {
            Modify-PTFConfig -PTFConfigPath $ptfconfig.FullName -nodeName "TDC.OSVersion" -value $osversionTDC
        }      
    }  
}

#-----------------------------------------------------------------------------
# Restart and Resume
#-----------------------------------------------------------------------------
Function RestartAndResume
{
    $NextStep = $Step + 1

    .\RestartAndRun.ps1 -ScriptPath $ScriptFullPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

#-----------------------------------------------------------------------------
# Finish Script
#-----------------------------------------------------------------------------
Function Finish
{
    # Write signal file
    Write-Log "Write signal file: $ScriptName.finished.signal to system drive."
    cmd /C ECHO CONFIG FINISHED > $ScriptSignalFullPath

    # Ending script
    Write-Log "Config finished."
    Write-Log "EXECUTE [$ScriptName] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    .\RestartAndRunFinish.ps1
}

#-----------------------------------------------------------------------------
# Main Function
#-----------------------------------------------------------------------------
Function Main(){

    Prepare
    ReadConfig
    SetLog

    switch ($Step)
    {
        1 { Phase1; RestartAndResume; }
        2 { Phase2; RestartAndResume; }
        3 { Phase3; RestartAndResume; }
        4 { Phase4; Finish; }
        default 
        {
            Write-Log "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
}

Main
