###########################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
###########################################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-ENDPOINT.ps1
## Purpose:        Configure Endpoint Server (Test Driver) in Local Domain for Active Directory Family Test Suite.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016, and later.
##
###########################################################################################

#------------------------------------------------------------------------------------------
# Parameters:
# Help: whether to display the help information
# Step: Current step for configuration
#------------------------------------------------------------------------------------------
Param
(
    [alias("h")]
    [switch]
    $Help,

    [switch]
    $EnableDebugging,
    
    [int]
    $Step = 1,

    [String]
    $ConfigFile = "c:\temp\Protocol.xml"
)

#------------------------------------------------------------------------------------------
# Global Variables:
# ScriptFileFullPath: Full Path of this script file
# ScriptName:         File Name of this script file 
# ScriptPath:         File Path of this script file
# SignalFileFullPath: Full Path of the completion signal file for this script file
# LogFileFullPath:    Full Path of the log file for this script file
# Parameters:         Parameters read from the config file
#------------------------------------------------------------------------------------------
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$ScriptPath              = Split-Path $ScriptFileFullPath
$SignalFileFullPath      = "$ScriptPath\post.finished.signal"
$LogFileFullPath         = "$ScriptFileFullPath.log"
$Parameters              = @{}
$IsAzure                 = $false

try {
    [xml]$content = Get-Content $ConfigFile -ErrorAction Stop
    $currentCore = $content.lab.core
    if(![string]::IsNullOrEmpty($currentCore.regressiontype) -and ($currentCore.regressiontype -eq "Azure")){
        $IsAzure = $true;
        $SignalFileFullPath = "C:\PostScript.Completed.signal"
    }
}
catch {
    
}
#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure Endpoint Server (Test Driver) in Local Domain for Active Directory Family Test Suite.

Usage:
    .\Config-DM.ps1 [-Step <step>] [-h | -help]

Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg "`r`n"
    exit 0
}

#------------------------------------------------------------------------------------------
# Function: Start-ConfigLog
# Create log file and start logging
#------------------------------------------------------------------------------------------
Function Start-ConfigLog()
{
    if(!(Test-Path -Path $LogFileFullPath)){
        New-Item -ItemType File -path $LogFileFullPath -Force
    }
    Start-Transcript $LogFileFullPath -Append 2>&1 | Out-Null
}

#------------------------------------------------------------------------------------------
# Function: Write-ConfigLog
# Write information to log file
#------------------------------------------------------------------------------------------
Function Write-ConfigLog
{
    Param (
        [Parameter(ValueFromPipeline=$true)] $text,
        $ForegroundColor = "Green"
    )

    $date = Get-Date -f MM-dd-yyyy_HH_mm_ss
    Write-Output "[$date] $text"
}

#------------------------------------------------------------------------------------------
# Function: Read-ConfigParameters
# Read Config Parameters
#------------------------------------------------------------------------------------------
Function Read-ConfigParameters()
{
    Write-ConfigLog "Getting the parameters from config file..." -ForegroundColor Yellow
    if($IsAzure)
    {
        .\GetVmParametersByComputerName.ps1 -RefParamArray ([ref]$Parameters)
    } else {
    $VMName = .\GetVMNameByComputerName.ps1
        .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$Parameters)
    }
    $Parameters
}

#------------------------------------------------------------------------------------------
# Function: Init-Environment
# Start logging, check signal file, switch to script path and read the config parameters
#------------------------------------------------------------------------------------------
Function Init-Environment()
{
    # Start logging
    Start-ConfigLog

    # Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan

    # Check completion signal file. If signal file exists, exit with 0
    if(Test-Path -Path $SignalFileFullPath){
        Write-ConfigLog "The script execution has been completed." -ForegroundColor Red
        exit 0
    }

    # Switch to the script path
    Write-ConfigLog "Switching to $ScriptPath..." -ForegroundColor Yellow
    Push-Location $ScriptPath

    # Read the config parameters
    Read-ConfigParameters
}

#------------------------------------------------------------------------------------------
# Function: Proceed-ScriptWithRestart
# Restart computer and proceed the script to the next step
#------------------------------------------------------------------------------------------
Function Proceed-ScriptWithRestart
{
    $NextStep = $Step + 1

    # add a schedule task to execute the script next step after restart
    .\RestartAndRun.ps1 -ScriptPath $ScriptFileFullPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

#------------------------------------------------------------------------------------------
# Function: Remove-ScriptFromTask
# Remove the sceduled task to execute the script next step
#------------------------------------------------------------------------------------------
Function Remove-ScriptFromTask
{
    # remove the schedule task to execute the script next step after restart
    .\RestartAndRunFinish.ps1
}

#------------------------------------------------------------------------------------------
# Function: Complete-Configure
# Write signal file, stop the transcript logging and remove the scedule task
#------------------------------------------------------------------------------------------
Function Complete-Configure
{
    # Write signal file
    Write-ConfigLog "Write signal file`: post.finished.signal to hard drive."
    cmd /C ECHO CONFIG FINISHED > $SignalFileFullPath

    # Ending script
    Write-ConfigLog "Config finished."
    Write-ConfigLog "EXECUTE [$ScriptName] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    # remove the schedule task to execute the script next step after restart
    .\RestartAndRunFinish.ps1
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase1
# Configure the environment phase 1:
# 1.  Set execution policy as unrestricted
# 2.  Set network configurations, for example, ip addresses, subnet mask, gateway and dns
# 3.  Set autologon
# 4.  [MS-ADTS-Security] Turn off UAC
# 5.  Join this computer to the local domain
#------------------------------------------------------------------------------------------
Function Config-Phase1()
{
    Write-ConfigLog "Entering Phase 1..."

    # Set execution policy as unrestricted
    Write-ConfigLog "Setting execution policy..." -ForegroundColor Yellow
    .\Set-ExecutionPolicy-Unrestricted.ps1
    if(-not $IsAzure)
    {
        # Set network configurations
        Write-ConfigLog "Setting network configurations..." -ForegroundColor Yellow
        .\Set-NetworkConfiguration.ps1 -IPAddress $Parameters["ip"] -SubnetMask $Parameters["subnet"] -Gateway $Parameters["gateway"] -DNS ($Parameters["dns"].Split(';'))
    }
    # Set autologon
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon.ps1 -Domain $Parameters["domain"] -Username $Parameters["username"] -Password $Parameters["password"]

    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    .\Turnoff-UAC.ps1

    # Join Local Domain
    Write-ConfigLog "Join this computer to the local domain..." -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["primarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["secondarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]

    # Kept receiving 53, if not stabilize during domain join
    Sleep 60
    .\Join-Domain.ps1 -domainWorkgroup "Domain" -domainName $Parameters["domain"] -userName $Parameters["username"] -userPassword $Parameters["password"] -testResultsPath $ScriptPath 2>&1 | Write-Output

    # Update host files
    if($IsAzure)
    {
        $file = "$env:windir\System32\drivers\etc\hosts"
        foreach ($Vm in $content.lab.servers.vm) {
            $currIp = $Vm.ip
            $currName = $Vm.name
            "$currIp $currName" | Add-Content -PassThru $file
        }
    }
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase2
# Configure the environment phase 2:
# 1.  Turn off firewall
# 2.  Set password change, netlogon, create object rights in registry key and group policy: 
#      (a) [MS-DRSR][MS-FRS2] disable auto password change (Registry Key)
#      (b) [MS-NRPC] refuse password change request (Registry Key)
#      (c) [MS-NRPC] configure netlogon service to depend on the DNS service (Registry Key)
#      (d) [MS-DRSR] disable auto password change (Group Policy)
#      (e) [MS-ADTS-LDAP] set only domain admins group can create computer object (Group Policy)
# 3.  [MS-DRSR] Set computer password for this domain member server to the first value, which will be changed to the second value in the next phase
# 4.  [MS-ADTS-LDAP] Set msDS-AdditonalDnsHostName on Domain Service object cn=<Hostname>,ou=domain controllers,<DomainNc> by triggering Phase 4 of Config-PDC.ps1 on <PDC>
# 5.  [MS-ADTS-Security] Copy and install certificate from <PDC>
#------------------------------------------------------------------------------------------
Function Config-Phase2()
{
    Write-ConfigLog "Entering Phase 2..."

    # Turn off firewall
    Write-ConfigLog "Turn off firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Set password change, netlogon, rights in registry key and group policy
    Write-ConfigLog "Setting password change, netlogon, create object rights in registry key and group policy..." -ForegroundColor Yellow
    .\Set-NetlogonRegKeyAndPolicy.ps1

    # Set computer password
    Write-ConfigLog "Setting computer password to the first value, which will be changed to the second value in the next phase..." -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["primarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["secondarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\Set-AdComputerPassword.ps1 -Domain $Parameters["domain"] -Name $Parameters["name"] -Password $Parameters["temppassword"] -DcName $Parameters["primarydc"]

    # Set msDS-AdditonalDnsHostName on Domain Service object cn=<Hostname>,ou=domain controllers,<DomainNc> by triggering Phase 4 of Config-PDC.ps1 <PDC>
    Write-ConfigLog "Setting msDS-AdditonalDnsHostName on Domain Service object cn=<Hostname>,ou=domain controllers,<DomainNc> by triggering Phase 4 of Config-PDC.ps1 on <PDC>..." -ForegroundColor Yellow
    .\Execute-RemoteScript.ps1 -RemoteHost $Parameters["primarydc"] -Username $($Parameters["domain"] + "\" + $Parameters["username"]) -Password $Parameters["password"] `
                               -ScriptPath $ScriptPath -ScriptName ".\Config-PDC.ps1" -ScriptArgs "-Step 4"

    # Copy and install certificate from <PDC>
    Write-ConfigLog "Copying and installing certificate from <PDC>..." -ForegroundColor Yellow
    Robocopy.exe $("\\" + $Parameters["primarydc"] + "\" + ($env:SystemDrive).Replace(":", "$")) $($env:SystemDrive + "\") $($Parameters["primarydc"] + ".cer") /NP /NFL /NDL
    .\Install-SelfSignedCert.ps1 -CertFile $($env:SystemDrive + "\" + $Parameters["primarydc"] + ".cer")
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase3
# Configure the environment phase 3:
# 1.  [MS-DRSR] Set computer password for this domain member server to the second value
# 2.  [MS-FRS2] Remotely restart DFS replication service on <SDC>
# 3.  Get and log operating system version
#------------------------------------------------------------------------------------------
Function Config-Phase3()
{
    Write-ConfigLog "Entering Phase 3..."

    # Set computer password
    Write-ConfigLog "Setting computer password..." -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["primarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["secondarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\Set-AdComputerPassword.ps1 -Domain $Parameters["domain"] -Name $Parameters["name"] -Password $Parameters["password"] -DcName $Parameters["primarydc"]

    # Remotely restart DFS replication service on <SDC>
    Write-ConfigLog "Remotely restarting DFS replication service on <SDC>..." -ForegroundColor Yellow
    .\Execute-RemoteScript.ps1 -RemoteHost $Parameters["secondarydc"] -Username $($Parameters["secondarydc"] + "\" + $Parameters["username"]) -Password $Parameters["password"] `
                               -ScriptPath $ScriptPath -ScriptName "Restart-Service" -ScriptArgs "DFSR"

    # Get OS Version
    Write-ConfigLog "Getting Operating System Version..." -ForegroundColor Yellow
    $OsVersion = .\Get-OsVersion.ps1 -log
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase4
# Configure the environment phase 4:
# 1.  Find PtfConfig files in both \bin and source\server\testcode\testsuite folders
# 2.  Modify PtfConfig file according to information retrieved from <PDC>
#     (a) [MS-ADTS-Security][MS-ADTS-LDAP][MS-ADTS-Schema] Update LDS ports
#     (b) [MS-ADTS-LDAP] Update domain functional level
#     (c) [MS-ADTS-PublishDCScenario][MS-ADTS-LSAD] Update domain netbios name, dns name, domain objectGuid, domain objectSid, and replication directory name
#     (d) [MS-ADTS-LDAP] Update netbios name, ip address and operating system version
# 3.  Modify PtfConfig file according to information retrieved from <SDC>
#     (a) [MS-ADTS-LDAP] Update netbios name, ip address and operating system version
# 4.  Modify PtfConfig file according to information retrieved from <RODC>
#     (a) [MS-ADTS-LDAP] Update netbios name, ip address and operating system version
# 5.  Modify PtfConfig file according to information retrieved from <CDC>
#     (a) Update child domain netbios name and dns name
#     (b) [MS-ADTS-LDAP] Update netbios name, ip address and operating system version
# 6.  Modify PtfConfig file according to information retrieved from <TDC>
#     (a) Update trusted domain netbios name and dns name
#     (b) [MS-ADTS-LDAP] Update netbios name, ip address and operating system version
# 7.  Modify PtfConfig file according to information retrieved from <DM>
#     (a) [MS-ADTS-LDAP] Update netbios name, ip address
# 8.  Modify PtfConfig file other settings
#     (a) [MS-ADTS-Security] Update certificate file full path
#     (b) [MS-ADTS-Schema] Update TD XML paths
#------------------------------------------------------------------------------------------
Function Config-Phase4()
{
    Write-ConfigLog "Entering Phase 4..."

    # Find PtfConfig files in both \bin and source\server\testcode\testsuite folders
    Write-ConfigLog "Finding PtfConfig files in both \bin and source\server\testcode\testsuite folders..." -ForegroundColor Yellow
    $PtfFiles = .\Find-PtfConfigFiles.ps1 -FileName "AD_ServerTestSuite.deployment.ptfconfig"

    ##########################################################################
    # Modify PtfConfig file according to information retrieved from <Common> #
    ##########################################################################
    Write-ConfigLog "Updating Common..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "DomainAdministratorName" -ProperyValue $Parameters["username"]
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "DomainUserPassword" -ProperyValue $Parameters["password"]

    #######################################################################
    # Modify PtfConfig file according to information retrieved from <PDC> #
    #######################################################################
    Write-ConfigLog "Modifying PtfConfig file according to information retrieved from <PDC>..." -ForegroundColor Yellow    
    $PdcShare =  "\\" + $Parameters["primarydcip"] + "\" + ($env:SystemDrive).Replace(":", "$")

    # (a) Update LDS ports
    Write-ConfigLog "Updating LDS ports..." -ForegroundColor Yellow
    $LdsLdapPort = Get-Content -Path "$PdcShare\port.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "ADLDSPortNum" -ProperyValue $LdsLdapPort
    $LdsSslPort = Get-Content -Path "$PdcShare\sslport.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "ADLDSSSLPortNum" -ProperyValue $LdsSslPort
    $LdsDefaultNc = "CN=ApplicationNamingContext,DC=" + $Parameters["domain"].ToString().Replace(".", ",DC=")
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "LDSApplicationNC" -ProperyValue $LdsDefaultNc

    # (b) Update domain functional level
    Write-ConfigLog "Updating domain functional level and operating system version..." -ForegroundColor Yellow
    $DomainFuncLvl = Get-Content -Path "$PdcShare\forestfunclvl.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "DomainFunctionLevel" -ProperyValue $DomainFuncLvl

    # (c) Update domain netbios name, dns name, domain objectGuid, domain objectSid, and replication directory name
    Write-ConfigLog "Updating domain netbios, objectGuid, domain objectSid..." -ForegroundColor Yellow
    $PrimaryDomainNetbios = Get-Content -Path "$PdcShare\domainNetbios.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "PrimaryDomain.NetBiosName" -ProperyValue $PrimaryDomainNetbios
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "PrimaryDomain.DNSName" -ProperyValue $Parameters["domain"]
    $DomainGuid = Get-Content -Path "$PdcShare\domainGuid.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "PrimaryDomain.ServerGUID" -ProperyValue $DomainGuid
    $DomainSid = Get-Content -Path "$PdcShare\domainSid.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "PrimaryDomain.SID" -ProperyValue $DomainSid
    $ReplicaDirName = "\\" + $Parameters["domain"] + "\SYSVOL\" + $Parameters["domain"] + "\scripts"
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "ReplicationDirectoryName" -ProperyValue $ReplicaDirName

    # (d) Update netbios name, ip address and operating system version
    Write-ConfigLog "Updating netbios name, ip address and operating system version..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "WritableDC1.NetbiosName" -ProperyValue $Parameters["primarydc"].ToString().Split('.')[0].ToUpper()
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "WritableDC1.IPAddress" -ProperyValue $Parameters["primarydcip"]
    $PdcOsVersion = Get-Content -Path "$PdcShare\osversion.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "WritableDC1.OSVersion" -ProperyValue $PdcOsVersion
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "SUT.Server.Computer.Name" -ProperyValue $Parameters["primarydc"].ToString().Split('.')[0].ToUpper()
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "SUT.SubnetNames.IP.V4" -ProperyValue $Parameters["subnet"]

    #######################################################################
    # Modify PtfConfig file according to information retrieved from <SDC> #
    #######################################################################
    Write-ConfigLog "Modifying PtfConfig file according to information retrieved from <SDC>..." -ForegroundColor Yellow
    $SdcShare =  "\\" + $Parameters["secondarydcip"] + "\" + ($env:SystemDrive).Replace(":", "$")

    # (a) Update netbios name, ip address and operating system version
    Write-ConfigLog "Updating netbios name, ip address and operating system version..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "WritableDC2.NetbiosName" -ProperyValue $Parameters["secondarydc"].ToString().Split('.')[0].ToUpper()
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "WritableDC2.IPAddress" -ProperyValue $Parameters["secondarydcip"]
    $SdcOsVersion = Get-Content -Path "$SdcShare\osversion.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "WritableDC2.OSVersion" -ProperyValue $SdcOsVersion

    ########################################################################
    # Modify PtfConfig file according to information retrieved from <RODC> #
    ########################################################################
    Write-ConfigLog "Modifying PtfConfig file according to information retrieved from <RODC>..." -ForegroundColor Yellow
    $RodcShare =  "\\" + $Parameters["readonlydcip"] + "\" + ($env:SystemDrive).Replace(":", "$")

    # (a) Update netbios name, ip address and operating system version
    Write-ConfigLog "Updating netbios name, ip address and operating system version..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "RODC.NetbiosName" -ProperyValue $Parameters["readonlydc"].ToString().Split('.')[0].ToUpper()
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "RODC.IPAddress" -ProperyValue $Parameters["readonlydcip"]
    $RodcOsVersion = Get-Content -Path "$RodcShare\osversion.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "RODC.OSVersion" -ProperyValue $RodcOsVersion

    #######################################################################
    # Modify PtfConfig file according to information retrieved from <CDC> #
    #######################################################################
    Write-ConfigLog "Modifying PtfConfig file according to information retrieved from <CDC>..." -ForegroundColor Yellow
    $CdcShare =  "\\" + $Parameters["childdcip"] + "\" + ($env:SystemDrive).Replace(":", "$")

    # (a) Update child domain netbios name and dns name
    Write-ConfigLog "Updating child domain netbios name and dns name..." -ForegroundColor Yellow
    $ChildDomainNetbios = Get-Content -Path "$CdcShare\domainNetbios.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "ChildDomain.NetBiosName" -ProperyValue $ChildDomainNetbios
    $ChildDomain = $Parameters["childdc"].Remove(0, $Parameters["childdc"].ToString().IndexOf('.') + 1)
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "ChildDomain.DNSName" -ProperyValue $ChildDomain

    # (b) Update netbios name, ip address and operating system version
    Write-ConfigLog "Updating netbios name, ip address and operating system version..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "CDC.NetbiosName" -ProperyValue $Parameters["childdc"].ToString().Split('.')[0].ToUpper()
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "CDC.IPAddress" -ProperyValue $Parameters["childdcip"]
    $CdcOsVersion = Get-Content -Path "$CdcShare\osversion.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "CDC.OSVersion" -ProperyValue $CdcOsVersion

    #######################################################################
    # Modify PtfConfig file according to information retrieved from <TDC> #
    #######################################################################
    Write-ConfigLog "Modifying PtfConfig file according to information retrieved from <TDC>..." -ForegroundColor Yellow
    $TdcShare =  "\\" + $Parameters["trustdcip"] + "\" + ($env:SystemDrive).Replace(":", "$")

    # (a) Update trusted domain netbios name and dns name
    Write-ConfigLog "Updating trusted domain netbios name and dns name..." -ForegroundColor Yellow
    $TrustDomainNetbios = Get-Content -Path "$TdcShare\domainNetbios.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "TrustDomain.NetBiosName" -ProperyValue $TrustDomainNetbios
    $TrustDomain = $Parameters["trustdc"].Remove(0, $Parameters["trustdc"].ToString().IndexOf('.') + 1)
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "TrustDomain.DNSName" -ProperyValue $TrustDomain

    # (b) Update netbios name, ip address and operating system version
    Write-ConfigLog "Updating netbios name, ip address and operating system version..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "TDC.NetbiosName" -ProperyValue $Parameters["trustdc"].ToString().Split('.')[0].ToUpper()
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "TDC.IPAddress" -ProperyValue $Parameters["trustdcip"]
    $TdcOsVersion = Get-Content -Path "$TdcShare\osversion.txt" -ErrorAction Stop
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "TDC.OSVersion" -ProperyValue $TdcOsVersion

    ######################################################################
    # Modify PtfConfig file according to information retrieved from <DM> #
    ######################################################################
    Write-ConfigLog "Modifying PtfConfig file according to information retrieved from <DM>..." -ForegroundColor Yellow

    # (a) Update netbios name, ip address
    Write-ConfigLog "Updating netbios name, ip address..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "DM.NetbiosName" -ProperyValue $Parameters["dm"].ToString().Split('.')[0].ToUpper()
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "DM.IPAddress" -ProperyValue $Parameters["dmip"]
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "DMAdminName" -ProperyValue $Parameters["username"]

    ############################################################################
    # Modify PtfConfig file according to information retrieved from <ENDPOINT> #
    ############################################################################
    Write-ConfigLog "Modifying PtfConfig file according to information retrieved from <ENDPOINT>..." -ForegroundColor Yellow

    # (a) Update operating system version
    Write-ConfigLog "Updating operating system version..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "ENDPOINT.NetbiosName" -ProperyValue $Parameters["name"]
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "ENDPOINT.IPAddress" -ProperyValue $Parameters["ip"]

    ########################################
    # Modify PtfConfig file other settings #
    ########################################
    Write-ConfigLog "Modifying PtfConfig file other settings..." -ForegroundColor Yellow
    
    # (a) Update certificate file full path
    Write-ConfigLog "Updating certificate file full path..." -ForegroundColor Yellow
    .\Modify-PtfConfigFiles.ps1 -Files $PtfFiles -ProperyName "certFilewithPathSpec" -ProperyValue $($env:SystemDrive + "\" + $Parameters["primarydc"] + ".cer")

    # (b) Update TD XML paths [MS-ADTS-Schema]
    foreach($PtfFile in $PtfFiles)
    {
        if($PtfFile.DirectoryName.Contains("Server\TestCode\TestSuite"))
        {
            $ParentPath = $PtfFile.DirectoryName.Replace("\Source\Server\TestCode\TestSuite","")
            $TDXmlPath = "$ParentPath\Data\Common-TD-XML\MS-ADA1\*,$ParentPath\Data\Common-TD-XML\MS-ADA2\*,$ParentPath\Data\Win8-TD-XML\MS-ADA2\*,$ParentPath\Data\Common-TD-XML\MS-ADA3\*,$ParentPath\Data\Common-TD-XML\MS-ADSC\*,$ParentPath\Data\Win8-TD-XML\MS-ADSC\*"
            $LdsTDXmlPath = "$ParentPath\Data\Common-TD-XML\MS-ADLS\*,$ParentPath\Data\Win8-TD-XML\MS-ADLS\* "
            $OpenXmlPath2016 = "$ParentPath\Data\Win2016-TD-XML\DS\*"
            $LdsOpenXmlPath2016 = "$ParentPath\Data\Win2016-TD-XML\LDS\*"
        }
        else
        {
            $ParentPath = $PtfFile.DirectoryName.Replace("\Bin","")
            $TDXmlPath = "$ParentPath\Data\Common-TD-XML\MS-ADA1\*,$ParentPath\Data\Common-TD-XML\MS-ADA2\*,$ParentPath\Data\Win8-TD-XML\MS-ADA2\*,$ParentPath\Data\Common-TD-XML\MS-ADA3\*,$ParentPath\Data\Common-TD-XML\MS-ADSC\*,$ParentPath\Data\Win8-TD-XML\MS-ADSC\*"
            $LdsTDXmlPath = "$ParentPath\Data\Common-TD-XML\MS-ADLS\*,$ParentPath\Data\Win8-TD-XML\MS-ADLS\* "
            $OpenXmlPath2016 = "$ParentPath\Data\Win2016-TD-XML\DS\*"
            $LdsOpenXmlPath2016 = "$ParentPath\Data\Win2016-TD-XML\LDS\*"
        }
        if($DomainFuncLvl -ge "6")
        {
            $TDXmlPath = $TDXmlPath.Replace("Win8","WinBlue")
            $LdsTDXmlPath = $LdsTDXmlPath.Replace("Win8","WinBlue")
        }
        .\Modify-PtfConfigFiles.ps1 -Files @($PtfFile) -ProperyName "TDXmlPath" -ProperyValue $TDXmlPath
        .\Modify-PtfConfigFiles.ps1 -Files @($PtfFile) -ProperyName "LdsTDXmlPath" -ProperyValue $LdsTDXmlPath
        .\Modify-PtfConfigFiles.ps1 -Files @($PtfFile) -ProperyName "OpenXmlPath2016" -ProperyValue $OpenXmlPath2016
        .\Modify-PtfConfigFiles.ps1 -Files @($PtfFile) -ProperyName "LdsOpenXmlPath2016" -ProperyValue $LdsOpenXmlPath2016
    }
}

#------------------------------------------------------------------------------------------
# Function: Config-Environment
# Control the overall workflow of all configuration phases
#------------------------------------------------------------------------------------------
Function Config-Environment
{
    # Start configure
    switch($Step)
    {
        1 { Config-Phase1; Proceed-ScriptWithRestart; }
        2 { Config-Phase2; Proceed-ScriptWithRestart; }
        3 { Config-Phase3; Proceed-ScriptWithRestart; }
        4 { Config-Phase4; Complete-Configure; }
        default
        {
            Write-ConfigLog "Fail to execute the script!" -ForegroundColor Red
            break
        }
    }
}

#------------------------------------------------------------------------------------------
# Main Function
#------------------------------------------------------------------------------------------
Function Main
{
    # Display help information
    if($Help)
    {
        Display-Help
        return
    }

    # Initialize configure environment
    Init-Environment    

    # Complete configure
    Config-Environment
}

Main