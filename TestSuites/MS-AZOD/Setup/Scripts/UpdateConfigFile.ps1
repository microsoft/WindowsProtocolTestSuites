#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
param(
    [string] $protocolName = "MS-AZOD",
    # The name of the XML file, indicating which environment you want to configure
    # For example:"MS-AZOD-SingleDomain-Lab.xml", "MS-AZOD-CrossDomain-Lab.xml", "RDPClient.xml",...
    # You could rename the xml file in use to the default name "protocol.xml" manually or vice versa
    [string]$EnvXmlName    = "protocol.xml"    
)

$SUTParamArray = @{}
#-------------------------
# Execute Configure Script
#-------------------------
$rootPath = Split-Path $MyInvocation.MyCommand.Definition -parentPush-Location $rootPath 
#----------------------------------------------------------------------------
# Start logging using start-transcript cmdlet
#----------------------------------------------------------------------------
$rootPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$logFile =  $rootPath +"\"+$MyInvocation.MyCommand.Name + ".log"

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

#------------------------------------------------------------------------------------------
# Write a piece of error message to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteError {
    Param (
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message,
    [switch]$Exit)

    Write-TestSuiteInfo -Message "[ERROR]: $Message" -ForegroundColor Red -BackgroundColor Black
    if ($Exit) {exit 1}
}

Function UpdateConfigFile
{

    Write-TestSuiteInfo "Start to update config file."
    $EnvXmlPath="$rootPath\$EnvXmlName"
    if(!(Test-Path $EnvXmlPath)) 
    {
    Write-TestSuiteError "$EnvXmlName was not found.please check the name or path of the xml file is right or not." -Exit
    }
    [string] $ProtocolXmlConfigFile = $EnvXmlPath
    [xml]$XmlContent = Get-Content $ProtocolXmlConfigFile -ErrorAction Stop

    try 
    {
        #-------------------------
        # Get Config.xml Path
        #-------------------------
        $endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\$protocolName\OD-Endpoint"
        $azodTestSuites = Get-ChildItem -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites\MS-AZOD-OD-*'
        $azodTestSuite = $azodTestSuites[0]
        $version = $azodTestSuite.Name.Substring(80, $azodTestSuite.Name.Length-80)
        $configPath = "$endPointPath\$version\Scripts\Config.xml"    
    
        Set-ItemProperty $configPath -name IsReadOnly -value $false
        $protocolXmlConfigContent =  [xml] ( Get-Content $ProtocolXmlConfigFile)
        $configContent =  [xml] ( Get-Content $configPath)

        $LocalDCVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"localdomaincontroller`"]") 
        [string]$LocalDCIP                       = $LocalDCVM.ip
        [string]$LocalDCComputerName             = $LocalDCVM.name
        [string]$LocalDomainName                 = $LocalDCVM.domain
        [string]$LocalDomainUser                 = $LocalDCVM.adminname
        [string]$LocalDomainUserPassword         = $LocalDCVM.adminpassword
    
        $LocalAP01TVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"localap01`"]")   
        [string]$localAp01IP                          = $LocalAP01TVM.IP
        [string]$localAp01ComputerName                = $LocalAP01TVM.name
    
        $DriverVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"drivercomputer`"]")   
        [string]$client01IP                          = $DriverVM.IP
        [string]$client01ComputerName                = $DriverVM.name
    
        $TrustDCVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"trustdomaincontroller`"]") 
        [string]$TrustDCIP                       = $TrustDCVM.ip
        [string]$TrustDCComputerName             = $TrustDCVM.name
        [string]$TrustDomainName                 = $TrustDCVM.domain
        [string]$TrustDomainUser                 = $TrustDCVM.adminname
        [string]$TrustDomainUserPassword         = $TrustDCVM.adminpassword
    
        $TrustAP01TVM = $protocolXmlConfigContent.SelectSingleNode("//vm[translate(role,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')= `"trustap01`"]")   
        [string]$trustAp01IP                          = $TrustAP01TVM.IP
        [string]$trustAp01ComputerName                = $TrustAP01TVM.name
    

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

        $node = $configContent.parameters.LocalRealm
        $node.DomainName = $LocalDomainName

        $node = $configContent.parameters.LocalRealm.DomainAdministrator
        $node.Username = $LocalDomainUser

        $node = $configContent.parameters.LocalRealm.DomainAdministrator
        $node.Password = $LocalDomainUserPassword
    
        $node = $configContent.parameters.LocalRealm.DC
        $node.Username = "" + $LocalDomainName +"\" +$LocalDomainUser
        $node.Password = $LocalDomainUserPassword
        $node.Name = $LocalDCComputerName
        $node.IP = $LocalDCIP

        $node = $configContent.parameters.LocalRealm.Client 
        $node.Name = $client01ComputerName
        $node.IP = $client01IP
        $node.user = "" + $LocalDomainName +"\" +$LocalDomainUser
        $node.Password = $LocalDomainUserPassword

        $node = $configContent.parameters.LocalRealm.FileServers.FileServer
        $node.Name = $localAp01ComputerName
        $node.IP = $localAp01IP
        $node.admin = "" + $LocalDomainName +"\" +$LocalDomainUser
        $node.adminPassword = $LocalDomainUserPassword
        
        $node = $configContent.parameters.TrustRealm
        $node.DomainName = $TrustDomainName

        $node = $configContent.parameters.TrustRealm.DomainAdministrator
        $node.Username = $TrustDomainUser

        $node = $configContent.parameters.TrustRealm.DomainAdministrator
        $node.Password = $TrustDomainUserPassword
    
        $node = $configContent.parameters.TrustRealm.Trust
        $node.TrustedRealmName = $LocalDomainName

        $node = $configContent.parameters.TrustRealm.DC
        $node.Username = "" + $TrustDomainName +"\" +$TrustDomainUser
        $node.Password = $TrustDomainUserPassword
        $node.Ip = $TrustDCIP
        $node.Name = $TrustDCComputerName

        $node = $configContent.parameters.TrustRealm.FileServers.FileServer
        $node.Name = $trustAp01ComputerName
        $node.IP = $trustAp01IP
        $node.admin = "" + $TrustDomainName +"\" +$TrustDomainUser
        $node.adminPassword = $TrustDomainUserPassword

        $node = $configContent.parameters.TrustRealm.ClaimTransformPolicies.ClaimTransformPolicy | where {$_.Name -eq 'DenyAllExceptCompanyPolicy'}
        $node.Server = $TrustDomainName 

        $configContent.Save($configPath)

        Write-TestSuiteInfo "Finish to update config file successfully."
    }
    catch
    {
        [String]$Emsg = "Unable to read parameters from $EnvXmlName. Error happened: " + $_.Exception.Message
        Write-TestSuiteInfo $Emsg
    }
}

UpdateConfigFile

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript

