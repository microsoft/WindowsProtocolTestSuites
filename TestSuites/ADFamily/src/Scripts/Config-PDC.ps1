#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-PDC.ps1
## Purpose:        Configure Primary DC for Active Directory test suites.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2
##
##############################################################################
Param
(
    [alias("h")][switch]$help,
    [string]$VMName = "AD_PDC", # The Virtual Machine's name
    [int]$Step      = 1         # Current step for configuration
)

if($help)
{
$helpmsg = @"
Post script to config Primary DC.

Usage:
    .\Config-PDC.ps1 [-VMName <vmname>] [-Step <step>] [-h | -help]

VMName: The name of the VM to be created. The default value is AD_PDC.
Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg "`r`n"  
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
# Read Config Parameters
#-----------------------------------------------------------------------------
Function ReadConfig()
{
    Write-Log "Getting the parameters from config file" -ForegroundColor Yellow
    .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
    $ParamArray
}

#-----------------------------------------------------------------------------
# Check signal file and switch to script path
#-----------------------------------------------------------------------------
Function Prepare()
{
    Write-Log "Executing [$ScriptName]" -ForegroundColor Cyan

    # Check completion signal file. If signal file exists, exit with 0
    if(Test-Path -Path $ScriptSignalFullPath){
        Write-Log "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    Write-Log "Switching to $ScriptPath" -ForegroundColor Yellow
    Push-Location $ScriptPath   
}

#-----------------------------------------------------------------------------
# Create Log 
#-----------------------------------------------------------------------------
Function SetLog()
{
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
        Write-Output "********************** "
        Write-Output "Dump local variables "
        Write-Output "********************** "
        Format-Table Name,Value -wrap -autosize -inputobject $vars
        Stop-Transcript
        throw "Error in line $line."
    }
}

#-----------------------------------------------------------------------------
# Set Alternate DNS 
#-----------------------------------------------------------------------------
Function SetAlternateDNS
{
	$nics = Get-WmiObject -Class Win32_NetworkAdapterConfiguration -Filter IPEnabled=TRUE | where {$_.ServiceName -eq "netvsc" -or $_.ServiceName -eq "dc21x4VM"} | sort MACAddress
    foreach($nic in $nics){
        netsh interface ipv4 add dnsservers $nic.interfaceindex $ParamArray["alternateDNS"] index=2
    }
}

#-----------------------------------------------------------------------------
# Phase1: SetNetworkConfiguration; SetAutoLogon; PromoteDomainController
#-----------------------------------------------------------------------------
Function Phase1
{
    Write-Log "Entering Phase1..."

    # Change execution policy
    Set-ExecutionPolicy Unrestricted
        
    # Set Network
    Write-Log "Setting network configuration`n" -ForegroundColor Yellow    
    .\SetNetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] -Gateway $ParamArray["gateway"] -DNS ($ParamArray["dns"].split(';'))
      
    # Set Auto Logon
    Write-Log "Setting auto logon`n" -ForegroundColor Yellow
    .\SetAutoLogon.ps1 -domain $ParamArray["domain"] -user $ParamArray["username"] -pwd $ParamArray["password"]

    # Turn off UAC [MS-ADTS-Security]
    Write-Log "Turning off UAC`n" -ForegroundColor Yellow
    set-itemproperty -path  HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System -name "EnableLUA" -value "0"

    # Promote DC
    Write-Log "Promoting this computer to DC`n" -ForegroundColor Yellow    
    .\PromoteDomainController.ps1 -DomainName $ParamArray["domain"] -AdminPwd $ParamArray["password"]

    # Set Alternate DNS
    SetAlternateDNS

    if(Test-Path -Path "c:\temp\scripts\TTT\tttracer.exe" -PathType Leaf)
    {
        .\scripts\registerwdbgsvr.ps1
    }
    sleep 15
}

#-----------------------------------------------------------------------------
# Phase2: TurnoffFirewall; ChangePassword; InstallCAService; InstallADLDS
#-----------------------------------------------------------------------------
Function Phase2
{
    Write-Log "Entering Phase2..."

    # Wait for computer to be stable
    Sleep 30   

    # Turn off firewall
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Output

    # Disable auto password change [MS-DRSR][MS-FRS2]
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon\Parameters /v DisablePasswordChange /t REG_DWORD /d 1 /f

    # Refuse password change request [MS-NRPC]
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon\Parameters /v RefusePasswordChange /t REG_DWORD /d 1 /f 
    
    # Configure the Netlogon service to depend on the DNS service
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon /v DependOnService /t REG_MULTI_SZ /d LanmanWorkstation\0LanmanServer\0DNS /f

    # Disable auto password change (Group Policy) [MS-DRSR] and set domain admins only to create computer object [MS-ADTS-LDAP] 
    Write-Log "Replace domain policy file to disable password change" -ForegroundColor Yellow
    $DomainPolicyId = (Get-GPO -Name "Default Domain Policy").id
    $policyFIlePath = "$env:SystemDrive\Windows\SYSVOL\domain\Policies\{$DomainPolicyId}\MACHINE\Microsoft\Windows NT\SecEdit\GptTmpl.inf"
	$GptTmplPath = "GptTmpl.txt"
	if(-not (Test-Path $GptTmplPath))
	{
		$GptTmplPath = ".\VSTORMLITEFiles\PostScript\GptTmpl.txt"
	}
    $text = Get-Content $GptTmplPath
    $domainAdmins = Get-ADGroup -Identity "Domain Admins"
    $gpSid = $domainAdmins.Sid
    $text = $text.Replace("%DomainAdminsGroupSid%",$gpSid)
    Set-Content $policyFIlePath $text 
         
    # Get OS Version and Forest Functional Level
    Write-Log "Getting OS Version" -ForegroundColor Yellow
    $osVersion   = .\Get-OSVersion.ps1
    if($osVersion -eq $null)
    {
        Write-Log "Unable to get OS Version and set as default value" -ForegroundColor Red
        $osVersion = "Win2012R2" 
    }      
    switch -Exact ($osVersion)
    {
        "Win2016" {
            $forestFunctionLv = "7"
            }
        "Win2012R2" {
            $forestFunctionLv = "6"
            }
        "Win2012" {
            $forestFunctionLv = "5"
            }
        "Win2008R2" {
            $forestFunctionLv = "4"
            }
        "Win2008" {
            $forestFunctionLv = "3"
            }
        default {
            Write-Log "Unknown OS Version." -ForegroundColor Red
            $forestFunctionLv = "6"
            }
    }
    # Log OS Version and Domain Functional Level in TXT
    $osPath = "$env:SystemDrive\osversion.txt"
    $osVersion>>$osPath
    $domainPath = "$env:SystemDrive\domainfunctionallv.txt"
    $forestFunctionLv>>$domainPath 
            
    # Install Active Directory Certificate Services [MS-ADTS-Security]
    Write-Log "Installing Active Directory Certificate Services"
    Import-Module ServerManager
    Add-WindowsFeature Adcs-Cert-Authority -confirm:$false
    Install-AdcsCertificationAuthority -CAType $ParamArray["CARoot"] -Confirm:$false

    # Install AD LDS [MS-ADTS-*]
    Write-Log "Installing Active Directory Lightweight Directory Services"
    Add-WindowsFeature ADLDS -IncludeAllSubFeature -Confirm:$false

    # Install DFS Management tools [MS-FRS2]
    Write-Log "Installing DFS Management tools"
    Add-WindowsFeature RSAT-DFS-Mgmt-Con -IncludeAllSubFeature -Confirm:$false

	# Set DFS Replication debug log level to 5(max)
	wmic /namespace:\\root\microsoftdfs path dfsrmachineconfig set debuglogseverity=5

    # Install IIS
    Write-Log "Installing WEB-Server" -foregroundcolor cyan
    Add-Windowsfeature -name WEB-Server -confirm:$false
    sleep 10

    $domainName = $ParamArray["domain"]
    $hostName = $ParamArray["name"]
    $Clientuser = $ParamArray["clientuser"]
    $userPassword = $ParamArray["userpassword"]
    $domainNC = "DC=" + $domainName.ToString().Replace(".", ",DC=")
    
    # Set password for local computer
    ksetup /SetComputerPassword $ParamArray["password"]

    # Set computer account password [MS-DRSR][MS-FRS2]
    $dcADSI=[ADSI]"LDAP://CN=$hostName,OU=Domain Controllers,$domainNC"
    $dcADSI.SetPassword($ParamArray["password"])
    $dcADSI.SetInfo()
    
    # Create client user for AD LDS instance       
    Write-Log "Creating client user account" -foregroundcolor cyan
    $Clientuser = $ParamArray["clientuser"] 
    net.exe user $Clientuser $userPassword /add /domain
    net.exe localgroup "Administrators" $Clientuser /add
    
    Write-Log "Adding to groups..." -foregroundcolor cyan
    net.exe group "Domain Admins" $Clientuser /add
    net.exe group "Domain Users" $Clientuser /add
    net.exe group "Enterprise Admins" $Clientuser /add
    net.exe group "Schema Admins" $Clientuser /add
    net.exe group "Group Policy Creator Owners" $Clientuser /add     
    # Set UPN for clientuser
    $UPN = [ADSI]"LDAP://CN=$Clientuser,CN=Users,$domainNC"
    $UPN.userPrincipalName = "$Clientuser@$domainName"
    $UPN.Setinfo()   
             
    # Install new AD LDS instance under client user [MS-ADTS-Security]
    Write-Log "Installing AD LDS instance under client user" -ForegroundColor Yellow
    $ldsnewapplicationpartitiontocreate = $ParamArray["ldsnewapplicationpartitiontocreate"] 
    $clientUserName = $domainName + "\" + $Clientuser
                                                                            
    [string]$ImportLDIFFiles = '"MS-AdamSyncMetadata.LDF" "MS-ADLDS-DisplaySpecifiers.LDF" "MS-AZMan.LDF" "MS-InetOrgPerson.LDF" "MS-User.LDF" "MS-UserProxy.LDF" "MS-UserProxyFull.LDF"'
    if($forestFunctionLv -ge "6")
    {
        $ImportLDIFFiles = '"MS-AdamSyncMetadata.LDF" "MS-ADLDS-DisplaySpecifiers.LDF" "MS-AZMan.LDF" "MS-InetOrgPerson.LDF" "MS-User.LDF" "MS-UserProxy.LDF" "MS-UserProxyFull.LDF" "MS-MembershipTransitive.LDF" "MS-ParentDistname.LDF" "MS-ReplValMetadataExt.LDF" "MS-SecretAttributeCARs.LDF" "MS-SetOwnerBypassQuotaCARs.LDF"'
    }

    $portCount = .\GetAvailablePort.ps1 $ParamArray["ldsldapport"]
    $sslportCount = $portCount +1
    $sslportCount = .\GetAvailablePort.ps1 $sslportCount

    Write-Log "Port: $portCount SSL Port: $sslportCount"

    .\InstallADLDS.ps1 -InstallType Unique `
                       -InstanceName $ParamArray["ldsinstancename"] `
                       -LocalLDAPPortToListenOn $portCount `
                       -LocalSSLPortToListenOn $sslportCount `
                       -NewApplicationPartitionToCreate "$ldsnewapplicationpartitiontocreate,$domainNC" `
                       -ServiceAccount $clientUserName `
                       -ServicePassword $userPassword `
                       -Administrator $clientUserName `
                       -SourceUserName $clientUserName `
                       -SourcePassword $userPassword `
                       -ImportLDIFFiles $ImportLDIFFiles

    sleep 15            
    
    # Log AD LDS info in TXT [MS-ADTS-Security]
    $portPath = "$env:SystemDrive\port.txt"
    $sslportPath = "$env:SystemDrive\sslport.txt"
    if(Test-Path -Path $portPath)
    {
        Remove-item $portPath
    }
    if(Test-Path -Path $sslportPath)
    {
        Remove-item $sslportPath
    }
    $portFile = New-Item -Type file -Path $portPath
    $sslportFile = New-Item -Type file -Path $sslportPath
    $portCount>>$portFile
    $sslportCount>>$sslportFile  
       
    # Enable Optional Feature [MS-ADTS-Security][MS-ADTS-Schema]
    Enable-ADOptionalFeature -Identity "CN=Recycle Bin Feature,CN=Optional Features,CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration, $domainNC" -Scope ForestOrConfigurationSet -Target "$domainName" -Confirm:$false
    sleep 10
        
    # Set msDS-Other-Settings (AD) [MS-ADTS-LDAP]
    Write-Log "Writing ADAMAllowADAMSecurityPrincipalsInConfigPartition=1" -ForegroundColor Yellow
    $DataPath = ".\data.txt"
    if(Test-Path -Path $DataPath)
    {
        Remove-item $DataPath
    }
    $DataFile = New-Item -Type file -Path $DataPath
    "dn:cn=directory service,cn=windows NT,cn=services,cn=configuration,$DomainNC">>$DataFile
    "changetype:modify">>$DataFile
    "replace:msds-other-settings">>$DataFile
    "msds-other-settings:ADAMAllowADAMSecurityPrincipalsInConfigPartition=1">>$DataFile
    "msds-other-settings:DisableVLVSupport=0">>$DataFile
    "msds-other-settings:DynamicObjectDefaultTTL=86400">>$DataFile
    "msds-other-settings:DynamicObjectMinTTL=900">>$DataFile
    "-">>$DataFile
    cmd.exe /c ldifde -v -i -f $DataPath | Write-Output
    CheckReturnValue

    # Update msDS-Behavior-Version (AD) [MS-ADTS-LDAP]
    $currentLevel = Get-ADObject -Identity "CN=Partitions,CN=Configuration,$DomainNC" -Properties * | Select "msds-Behavior-Version" 
    $level = $currentLevel.'msds-Behavior-Version' 
    Write-Log "Current msds-Behavior-Version level of forest is`: $level" 
    if($level -ne $forestFunctionLv)
    {
        Write-Log "Update msDS-Behavior-Version of forest to $forestFunctionLv" -ForegroundColor Yellow
        Remove-item $DataPath
        $DataFile = New-Item -Type file -Path $DataPath
        "dn:cn=partitions,cn=configuration,$DomainNC">>$DataFile
        "changetype:modify">>$DataFile
        "replace:msDS-Behavior-Version">>$DataFile
        "msDS-Behavior-Version:$forestFunctionLv">>$DataFile
        "-">>$DataFile
        cmd.exe /c ldifde -v -i -f $DataPath | Write-Output
    }
    CheckReturnValue       
          
    # Set msDS-AdditionalDnsHostName (AD) [MS-ADTS-Security][MS-ADTS-Schema]
    Write-Log "Setting msDS-AdditonalDnsHostName"
    Set-ADComputer "$hostName" -Replace @{"msDS-AdditionalDnsHostName"="$hostName.$domainName"}
    CheckReturnValue

    # Log ActualDomainGuid [MS-ADTS-PublishDCScenario]   
    $domainEntry = [ADSI]"LDAP://$domainNC"
    $domainGuid = (New-Object guid(,$domainEntry.objectGUID.Value)).ToString()
    Write-Log "Server GUID: $domainGuid"
    Set-Content "$Env:SystemDrive\SrvGuidForm.txt" $domainGuid    
    
    # Get AD LDS instance ID
    $instanceName = $ParamArray["ldsinstancename"]
    $NC = get-itemproperty -path  HKLM:\SYSTEM\ControlSet001\Services\ADAM_$instanceName\Parameters -name "Configuration NC"
    $Instance = $NC."Configuration NC"
    $ADAMDomainDN = $Instance.Split(",")[1] #instance ID (CN=xxx)
    Write-Log "Get instance id of $instanceName`: $ADAMDomainDN"
    # Set msDS-Other-Settings (AD LDS) [MS-ADTS-Security][MS-ADTS-Schema]
    $SecurePwd = ConvertTo-SecureString $userPassword -AsPlainText -Force
    $cred = New-Object System.Management.Automation.PSCredential $clientUserName, $SecurePwd
    Write-Log "Set msDS-Other-Settings for AD LDS instance"
    [string[]]$msDSOtherSettings=("ADAMAllowADAMSecurityPrincipalsInConfigPartition=1",`
                              "ADAMDisableLogonAuditing=0","ADAMDisablePasswordPolicies=0",`
                              "ADAMDisableSPNRegistration=0","ADAMLastLogonTimestampWindow=7",`
                              "DisableVLVSupport=0","DynamicObjectDefaultTTL=86400",`
                              "DynamicObjectMinTTL=900","MaxReferrals=3","ReferralRefreshInterval=5",`
                              "RequireSecureProxyBind=1","RequireSecureSimpleBind=0","SelfReferralsOnly=0")    
    for($i=0;$i -lt 5;$i++)
    {
        try
        {                        
            Set-ADObject -Identity "CN=Directory Service, CN=Windows NT, CN=Services,CN=Configuration,$ADAMDomainDN" `
                    -Replace @{'msDS-Other-Settings'=$msDSOtherSettings} -Server localhost:$portCount -Credential $cred -Confirm:$false
            break
        }
        catch
        {
            Write-Log "Can't set msDS-Other-Settings for AD LDS instance, retry. "
        } 
        sleep 15
    }

    # Create Managed Service Account [MS-APDS]
    Write-Log "Create Managed Service Account"
    $ManagedServiceAccountName = $ParamArray["msaname"]
    $ManagedServiceAccountPwd = $ParamArray["msapassword"]
    Write-Log "Account name`: $ManagedServiceAccountName"
    Write-Log "Account password`: $ManagedServiceAccountPwd"

    Add-KdsRootKey -EffectiveTime ((Get-Date).AddHours(-10))
    New-ADServiceAccount -Name $ManagedServiceAccountName -RestrictToSingleComputer -AccountPassword $(ConvertTo-SecureString -AsPlainText $ManagedServiceAccountPwd -Force) -Enabled $true
    $ServiceAccount = Get-ADServiceAccount -Identity $ManagedServiceAccountName
    $ComputerInstance = Get-ADComputer -Identity $ParamArray["clientname"]
    Add-ADComputerServiceAccount -Identity $ComputerInstance -ServiceAccount $ServiceAccount
}

#-----------------------------------------------------------------------------
# Phase3: [MS-NRPC] CreateLocalSideForestTrust
#-----------------------------------------------------------------------------
Function Phase3
{
    Write-Log "Entering Phase3..."
    Write-Log "Create forest trust relationship on local side" -ForegroundColor Yellow
    .\CreateLocalSideForestTrust.ps1 -TargetForestName $ParamArray["trusttargetdomain"] -TrustPassword $ParamArray["trustpassword"]
}

#-----------------------------------------------------------------------------
# Phase4: [MS-ADTS-LDAP] 
#-----------------------------------------------------------------------------
Function Phase4
{
    Write-Log "Entering Phase4, called by ENDPOINT..."
    # Update msDS-AdditionalDnsHostName (client machine needs to join domain) [MS-ADTS-LDAP]
    $DataPath = ".\data.txt"
    if(Test-Path -Path $DataPath)
    {
        Remove-item $DataPath
    }
    $ClientName = $ParamArray["clientname"]
    $domainNC = "DC=" + $ParamArray["domain"].ToString().Replace(".", ",DC=")  
    Write-Log "Setting msDS-AdditionalDnsHostName of Client machine to $ClientName" -ForegroundColor Yellow
    $DataFile = New-Item -Type file -Path $DataPath
    "dn:cn=$ClientName,cn=computers,$domainNC">>$DataFile
    "changetype:modify">>$DataFile
    "replace:msDS-AdditionalDnsHostName">>$DataFile
    "msDS-AdditionalDnsHostName:$ClientName">>$DataFile
    "-">>$DataFile
    cmd.exe /c ldifde -v -i -f $DataPath | Write-Output

    # Export SSL certificate [MS-ADTS-Security]
    Write-Log "Creating a SSL certificate"
    $dataFolder = "$ScriptPath\Data"
    if(!(Test-Path -Path $dataFolder)){
        New-Item -ItemType Directory -path $dataFolder -Force
    }
    $hostName = $ParamArray["name"]
    $domainName = $ParamArray["domain"]
    $OID_Server = "1.3.6.1.5.5.7.3.1"
    $CERT_SSL = "$hostName"+ ".$domainName"
    $myToolsPath = $dataFolder
    
    # Create cert    
	Write-Log "Creating self-signed certificate" -foregroundcolor Yellow
	$cmdLine = "$myToolsPath\makecert -r -pe -n `"CN=$CERT_SSL`" -ss my -sr LocalMachine -a sha1 -sky exchange -eku $OID_server -sp `"Microsoft RSA SChannel Cryptographic Provider`" -sy 12 $dataFolder\$CERT_SSL.CER"
	cmd.exe /c $cmdLine 2>&1 | Write-Output
    xcopy $dataFolder\$CERT_SSL.CER $env:HOMEDRIVE\ /f
    # Install CA cert
    certutil.exe -addstore MY $dataFolder\$CERT_SSL.CER 
	certutil.exe -addstore root $dataFolder\$CERT_SSL.cer
	# Configure SSl binding for IIS website
	Write-Log "Get the thumbprint of the certificate (Certificate Hash)..."
	$originalCertHash =  .\Get-CertHash.ps1 $CERT_SSL "MY"
	$consecCertHash  = $originalCertHash.Replace(" ","")
	$guid = [Guid]::NewGuid()
	$appId = $guid.ToString()
	netsh http add sslcert ipport=0.0.0.0:443 certhash=$consecCertHash appid="{$appId}" 2>&1 | Write-Output
    CheckReturnValue
    cmd.exe /c "$env:windir\system32\inetsrv\appcmd.exe" set site "Default Web site" /+"bindings.[protocol='https',bindingInformation='*:443:']"  2>&1 | Write-Output
    CheckReturnValue
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
# RestartAndWait
#-----------------------------------------------------------------------------
Function RestartAndWait
{
    .\RestartAndRunFinish.ps1 -AutoRestart $true
}

#-----------------------------------------------------------------------------
# Main Script
#-----------------------------------------------------------------------------
Function Main
{
    Prepare
    ReadConfig
    SetLog

    switch($Step)
    {
        1 { Phase1; RestartAndResume; }
        2 { Phase2; RestartAndWait; }
        3 { Phase3; }
        4 { Phase4; Finish; }
        default
        {
            Write-Log "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
}

Main

