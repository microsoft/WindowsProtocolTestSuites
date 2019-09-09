#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
param(
    [string]$WorkingPath = "C:\temp"                                    # Script working path
)

$SUTParamArray = @{}
#-------------------------
# Execute Configure Script
#-------------------------

#----------------------------------------------------------------------------
# Start logging using start-transcript cmdlet
#----------------------------------------------------------------------------
$logFile =  $WorkingPath + "\"+$MyInvocation.MyCommand.Name + ".log"

Start-Transcript -Path "$logFile" -Append -Force
	
#------------------------------------------------------------------------------------------
# Write a piece of information to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteInfo {
    Param(
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message,
    [string]$ForegroundColor = "White",
    [string]$BackgroundColor = "DarkBlue")

    # WinBlue issue: Start-Transcript cannot write the log printed out by Write-Host, as a workaround, use Write-output instead
    # Write-Output does not support color
    if([Double]$Script:HostOsBuildNumber -eq [Double]"6.3") {
        ((Get-Date).ToString() + ": $Message") | Out-Host
    }
    else {
        Write-Host ((Get-Date).ToString() + ": $Message") -ForegroundColor $ForegroundColor -BackgroundColor $BackgroundColor
    }
}

Function UpdateConfigFile
{

    Write-TestSuiteInfo "Start to update config file."
    [string] $ProtocolXmlConfigFile = "protocol.xml"
    if(Test-Path -Path $WorkingPath)
    {
        $ProtocolXmlConfigFile = "$WorkingPath\protocol.xml"
    }
    [xml]$XmlContent = Get-Content $ProtocolXmlConfigFile -ErrorAction Stop
    try {
        $currentCore = $XmlContent.lab.core
    }
    catch {
        
    }

    try 
    {
        #-------------------------
        # Get Config.xml Path
        #-------------------------
        $configPath = "$WorkingPath\Scripts\ParamConfig.xml"    
    
        Set-ItemProperty $configPath -name IsReadOnly -value $false
        $protocolXmlConfigContent =  [xml] ( Get-Content $ProtocolXmlConfigFile)
        $configContent =  [xml] ( Get-Content $configPath)

        $PDC01VM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"pdc01`"]") 
        [string]$LocalDCIP                       = $PDC01VM.ip
        [string]$LocalDCComputerName             = $PDC01VM.name
        [string]$LocalDomainName                 = $PDC01VM.domain
        [string]$LocalDomainUser                 = $PDC01VM.username
        [string]$LocalDomainUserPassword         = $PDC01VM.password
        if($null -eq $PDC01VM.username)
        {
            [string]$LocalDomainUser             = $currentCore.username
            [string]$LocalDomainUserPassword     = $currentCore.password
        }
    
        $AP01TVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"ap01`"]")   
        [string]$localAp01IP                          = $AP01TVM.ip
        [string]$localAp01ComputerName                = $AP01TVM.name
        [string]$localAp01Password                    = $AP01TVM.password
        if($null -eq $AP01TVM.password)
        {
            [string]$localAp01Password                = $currentCore.password
        }
    
        $DriverVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"drivercomputer`"]")   
        [string]$client01IP                          = $DriverVM.ip
        [string]$client01ComputerName                = $DriverVM.name
        [string]$client01Password                    = $DriverVM.password
        if($null -eq $DriverVM.password)
        {
            [string]$client01Password                = $currentCore.password
        }
    
        $TrustDCVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"tdc01`"]") 
        [string]$TrustDCIP                       = $TrustDCVM.ip
        [string]$TrustDCComputerName             = $TrustDCVM.name
        [string]$TrustDomainName                 = $TrustDCVM.domain
        [string]$TrustDomainUser                 = $TrustDCVM.username
        [string]$TrustDomainUserPassword         = $TrustDCVM.password
        if($null -eq $TrustDCVM.username)
        {
            [string]$TrustDomainUser             = $currentCore.username
            [string]$TrustDomainUserPassword     = $currentCore.password
        }
    
        $TrustAP01TVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"ap02`"]")   
        [string]$trustAp01IP                          = $TrustAP01TVM.ip
        [string]$trustAp01ComputerName                = $TrustAP01TVM.name
        [string]$trustAp01Password                    = $TrustAP01TVM.password
        if($null -eq $TrustAP01TVM.password)
        {
            [string]$trustAp01Password                = $currentCore.password
        }

        $PROXY01TVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"proxy01`"]")   
        [string]$proxy01IP                          = $PROXY01TVM.ip
        [string]$proxy01ComputerName                = $PROXY01TVM.name
    

        if([string]::IsNullOrEmpty($LocalDomainName))
        {
            $LocalDomainName = "contoso.com"        
        }

        if([string]::IsNullOrEmpty($LocalDomainUser))
        {
            $LocalDomainUser = "administrator"        
        }

        if([string]::IsNullOrEmpty($LocalDomainUserPassword))
        {
            $LocalDomainUserPassword = "Password01!"        
        }

        if([string]::IsNullOrEmpty($localAp01ComputerName))
        {
            $localAp01ComputerName = "AP01"        
        }
    
        if([string]::IsNullOrEmpty($TrustDomainName))
        {
            $TrustDomainName = "kerb.com"        
        }

        if([string]::IsNullOrEmpty($TrustDomainUser))
        {
            $TrustDomainUser = "administrator"        
        }

        if([string]::IsNullOrEmpty($TrustDomainUserPassword))
        {
            $TrustDomainUserPassword = "Password01!"        
        }

        if([string]::IsNullOrEmpty($trustAp01ComputerName))
        {
            $trustAp01ComputerName = "AP01"        
        }

        Write-Host "Configure parameters"
        $KDCPServerUrl = "https://$proxy01ComputerName.$LocalDomainName/KdcProxy"
        $node = $configContent.parameters
        $node.KKDCPServerUrl = $KDCPServerUrl

        Write-Host "Configure LocalRealm"
        $node = $configContent.parameters.LocalRealm
        $node.RealmName = $LocalDomainName

        Write-Host "Configure LocalRealm.KDC"
        $node = $configContent.parameters.LocalRealm.KDC
        $node.FQDN = "$LocalDCComputerName.$LocalDomainName"
        $node.NetBiosName = $LocalDCComputerName + '$'
        $node.Password = $LocalDomainUserPassword
        $node.IPv4Address = $LocalDCIP

        Write-Host "Configure LocalRealm.ClientComputer"
        $node = $configContent.parameters.LocalRealm.ClientComputer
        $node.FQDN = "$client01ComputerName.$LocalDomainName"
        $node.NetBiosName = $client01ComputerName + '$'
        $node.Password = $client01Password
        $node.IPv4Address = $client01IP
        $node.DefaultServiceName = "host/$client01ComputerName.$LocalDomainName".ToLower()
        $node.ServiceSalt = $LocalDomainName.ToUpper() + "host$client01ComputerName.$LocalDomainName".ToLower()
    
        Write-Host "Configure LocalRealm.FileShare"
        $node = $configContent.parameters.LocalRealm.FileShare
        $node.FQDN = "$localAp01ComputerName.$LocalDomainName"
        $node.NetBiosName = $localAp01ComputerName + '$'
        $node.IPv4Address = $localAp01IP
        $node.DefaultServiceName = "host/$localAp01ComputerName.$LocalDomainName".ToLower()
        $node.ServiceSalt = $LocalDomainName.ToUpper() + "host$localAp01ComputerName.$LocalDomainName".ToLower()
        $node.Smb2ServiceName = "cifs/$localAp01ComputerName.$LocalDomainName".ToLower()

        Write-Host "Configure LocalRealm.LdapServer"
        $node = $configContent.parameters.LocalRealm.LdapServer
        $node.FQDN = "$LocalDCComputerName.$LocalDomainName"
        $node.NetBiosName = $LocalDCComputerName + '$'
        $node.IPv4Address = $LocalDCIP
        $node.DefaultServiceName = "ldap/$LocalDCComputerName.$LocalDomainName".ToLower()
        $node.ServiceSalt = $LocalDomainName.ToUpper() + "host$LocalDCComputerName.$LocalDomainName".ToLower()
        $node.LdapServiceName = "ldap/$LocalDCComputerName.$LocalDomainName".ToLower()

        Write-Host "Configure LocalRealm.WebServer"
        $node = $configContent.parameters.LocalRealm.WebServer 
        $node.FQDN = "$localAp01ComputerName.$LocalDomainName"
        $node.NetBiosName = $localAp01ComputerName + '$'
        $node.user = "" + $LocalDomainName +"\test01"
        $node.IPv4Address = $localAp01IP
        $node.DefaultServiceName = "host$localAp01ComputerName.$LocalDomainName".ToLower()
        $node.ServiceSalt = $LocalDomainName.ToUpper() + "host$localAp01ComputerName.$LocalDomainName".ToLower()
        $node.HttpServiceName = 'http/' + "$localAp01ComputerName.$LocalDomainName"
        $node.HttpUri = 'http://' + "$localAp01ComputerName.$LocalDomainName"

        Write-Host "Configure LocalRealm.AuthNotRequired"
        $node = $configContent.parameters.LocalRealm.AuthNotRequired
        $node.FQDN = "AuthNotRequired.$LocalDomainName"

        Write-Host "Configure LocalRealm.LocalResource01"
        $node = $configContent.parameters.LocalRealm.LocalResource01
        $node.NetBiosName = 'localResource01$'
        $node.FQDN = "localResource01.$LocalDomainName"

        Write-Host "Configure LocalRealm.LocalResource02"
        $node = $configContent.parameters.LocalRealm.LocalResource02
        $node.NetBiosName = 'LocalResource02$'
        $node.FQDN = "LocalResource02.$LocalDomainName"

        Write-Host "Configure LocalRealm.Administrator"
        $node = $configContent.parameters.LocalRealm.Administrator
        $node.Username = $LocalDomainUser
        $node.Password = $LocalDomainUserPassword

        ## Trust Realm
        Write-Host "Configure parameters.TrustRealm"
        $node = $configContent.parameters.TrustRealm
        $node.RealmName = $TrustDomainName

        Write-Host "Configure parameters.TrustRealm.KDC"
        $node = $configContent.parameters.TrustRealm.KDC
        $node.FQDN = "$TrustDCComputerName.$TrustDomainName"
        $node.NetBiosName = $TrustDCComputerName + '$'
        $node.IPv4Address = $TrustDCIP
        $node.DefaultServiceName = "krbtgt/" + $TrustDomainName.ToUpper()

        Write-Host "Configure parameters.TrustRealm.FileShare"
        $node = $configContent.parameters.TrustRealm.FileShare
        $node.FQDN = "$trustAp01ComputerName.$TrustDomainName"
        $node.IPv4Address = $trustAp01IP
        $node.NetBiosName = $trustAp01ComputerName + '$'
        $node.DefaultServiceName = "host$trustAp01ComputerName.$TrustDomainName".ToLower()
        $node.ServiceSalt = $TrustDomainName.ToUpper() + "host$trustAp01ComputerName.$TrustDomainName".ToLower()
        $node.Smb2ServiceName = "cifs/$trustAp01ComputerName.$TrustDomainName".ToLower()

        Write-Host "Configure parameters.TrustRealm.WebServer"
        $node = $configContent.parameters.TrustRealm.WebServer
        $node.FQDN = "$trustAp01ComputerName.$TrustDomainName"
        $node.IPv4Address = $trustAp01IP
        $node.NetBiosName = $trustAp01ComputerName + '$'
        $node.DefaultServiceName = "host/$trustAp01ComputerName.$TrustDomainName".ToLower()
        $node.ServiceSalt = $TrustDomainName.ToUpper() + "host$trustAp01ComputerName.$TrustDomainName".ToLower()
        $node.HttpServiceName = 'http/' + "$trustAp01ComputerName.$TrustDomainName"
        $node.HttpUri = 'http://' + "$trustAp01ComputerName.$TrustDomainName"

        Write-Host "Configure parameters.TrustRealm.Administrator"
        $node = $configContent.parameters.TrustRealm.Administrator
        $node.Username = $TrustDomainUser
        $node.Password = $TrustDomainUserPassword

        Write-Host "Configure parameters.TrustRealm.LdapServer"
        $node = $configContent.parameters.TrustRealm.LdapServer
        $node.FQDN = "$TrustDCComputerName.$TrustDomainName"
        $node.NetBiosName = $TrustDCComputerName + '$'
        $node.IPv4Address = $TrustDCIP
        $node.DefaultServiceName = "ldap/$TrustDCComputerName.$TrustDomainName".ToLower()
        $node.ServiceSalt = $TrustDomainName.ToUpper() + "host$TrustDCComputerName.$TrustDomainName".ToLower()
        $node.LdapServiceName = "ldap/$TrustDCComputerName.$TrustDomainName".ToLower()

        
        $configContent.Save($configPath)

        Write-TestSuiteInfo "Finish to update config file successfully."
    }
    catch
    {
        [String]$Emsg = "Unable to read parameters from protocol.xml. Error happened: " + $_.Exception.Message
        Write-TestSuiteInfo $Emsg
    }
}

UpdateConfigFile

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript
