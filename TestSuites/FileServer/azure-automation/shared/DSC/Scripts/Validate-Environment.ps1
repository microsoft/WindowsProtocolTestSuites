# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
#############################################################################
#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           Validate-Environment.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012 +
## Purpose:        Validate environment and modify ptfconfig to inconclusive cases not applicable
##
##############################################################################
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'Password originates from Azure deployment config; no interactive prompt available.')]
param($ContextName = "")
# For FileSharing, the context name should be any of below
# Samba_Workgroup_SMB2002
# Samba_Workgroup_SMB21
# Samba_Workgroup_SMB30
# Win2008R2_Domain_NonCluster_SMB21
# Win2008R2_Workgroup_SMB21
# Win2008_Domain_NonCluster_SMB2002
# Win2008_Workgroup_SMB2002
# Win2012R2_Domain_Cluster_SMB302
# Win2012R2_Domain_NonCluster_SMB302
# Win2012R2_Workgroup_SMB302
# Win2012_Domain_Cluster_SMB30
# Win2012_Domain_NonCluster_SMB30
# Win2012_Workgroup_SMB30
# Win10_Domain_Cluster_SMB311
# Win10_Domain_NonCluster_SMB311
# Win10_Domain_Cluster_SMB311_ForPG
# Win10_Domain_Cluster_SMB311_BVTForPG

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath"

$logPath = $env:SystemDrive + "\Test\TestLog"

if (!(Test-Path -Path $logPath)) {
    mkdir $logPath
}

#----------------------------------------------------------------------------
# Read Config.json -- derive package root from script location
# Script lives in DSC/Scripts/; Config.json is at the package root (two levels up).
#----------------------------------------------------------------------------
$packageRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$configJsonPath = Join-Path $packageRoot "Config.json"
if (-not (Test-Path -Path $configJsonPath)) {
    $configJsonPath = "$env:SystemDrive\Temp\Config.json"
}

$VMConfig = Get-Content $configJsonPath -Raw | ConvertFrom-Json
$global:userNameInVM = $VMConfig.Core.Username
$global:userPwdInVM = $VMConfig.Core.Password

# Prefer Domain.NetBiosName (e.g., CONTOSO) for credential construction.
# FQDN format (contoso.com\user) works with Kerberos but NetBIOS is more reliable.
$global:domainInVM = if ($VMConfig.Domain -and $VMConfig.Domain.NetBiosName) {
    $VMConfig.Domain.NetBiosName
} elseif ($VMConfig.Core.DomainName) {
    $VMConfig.Core.DomainName
} else { "" }

# Resolve endPointPath from Tools.json
$toolsJsonPath = Join-Path (Split-Path $configJsonPath -Parent) "Tools.json"
if (Test-Path $toolsJsonPath) {
    $tools = Get-Content $toolsJsonPath -Raw | ConvertFrom-Json
    $endPointPath = [Environment]::ExpandEnvironmentVariables($tools.DriverComputer.TestsuiteZips[0].targetFolder)
}
else {
    $endPointPath = "$env:SystemDrive\FileServer-TestSuite-ServerEP"
}

# Get test suite path
$binDir = "$endPointPath\bin"

#Get ptfconfig path
$CommonTestSuitePtfConfig = "$binDir\CommonTestSuite.deployment.ptfconfig"
$Smb2TestSuitePtfConfig = "$binDir\MS-SMB2_ServerTestSuite.deployment.ptfconfig"
$serverFailoverPtfConfig = "$binDir\ServerFailoverTestSuite.deployment.ptfconfig"
$SMB2ModelPtfConfig = "$binDir\MS-SMB2Model_ServerTestSuite.deployment.ptfconfig"
$rsvdPtfConfig = "$binDir\MS-RSVD_ServerTestSuite.deployment.ptfconfig"
$sqosPtfConfig = "$binDir\MS-SQOS_ServerTestSuite.deployment.ptfconfig"

$securePwd = ConvertTo-SecureString "$userPwdInVM" -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential("$DomainInVM\$userNameInVM", $securePwd )

# Domain/Cluster context variables -- populated from config when available
$DC01IP = if ($VMConfig.Machines.DC) { $VMConfig.Machines.DC.IpConfig[0].Ip } else { $null }
$DC01ComputerName = if ($VMConfig.Machines.DC) { $VMConfig.Machines.DC.ComputerName } else { $null }
$Storage01IP = if ($VMConfig.Machines.Storage) { $VMConfig.Machines.Storage.IpConfig[0].Ip } else { $null }
$Node02IP1 = if ($VMConfig.Machines.Node02) { $VMConfig.Machines.Node02.IpConfig[0].Ip } else { $null }
$Node02IP2 = if ($VMConfig.Machines.Node02 -and $VMConfig.Machines.Node02.IpConfig.Count -gt 1) { $VMConfig.Machines.Node02.IpConfig[1].Ip } else { $null }
$Node02ComputerName = if ($VMConfig.Machines.Node02) { $VMConfig.Machines.Node02.ComputerName } else { $null }

# All environments require Node01 (Cluster uses .Node01, Domain/Workgroup uses .SUT)
$sutMachine = if ($VMConfig.Machines.SUT) { $VMConfig.Machines.SUT } else { $VMConfig.Machines.Node01 }
$Node01IP1 = $sutMachine.IpConfig[0].Ip
if ($sutMachine.IpConfig.Count -gt 1) {
    $Node01IP2 = $sutMachine.IpConfig[1].Ip
}
$Node01ComputerName = $sutMachine.ComputerName

# Driver computer
$Client01IP1 = $VMConfig.Machines.DriverComputer.IpConfig[0].Ip
if ($VMConfig.Machines.DriverComputer.IpConfig.Count -gt 1) {
    $Client01IP2 = $VMConfig.Machines.DriverComputer.IpConfig[1].Ip
}

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
Start-Transcript -Path "$logPath\Validate-Environment.ps1.log" -Append -Force

#----------------------------------------------------------------------------
# Common function
#----------------------------------------------------------------------------
function ExitCode() {
    return $MyInvocation.ScriptLineNumber
}

function ReadPtfconfigProperty($ptfconfigFile, $propertyName) {
    [xml]$configContent = Get-Content $ptfconfigFile
    $propertyNodes = $configContent.GetElementsByTagName("Property")
    foreach ($node in $propertyNodes) {
        if ($node.GetAttribute("name") -eq $propertyName) {
            return $node.GetAttribute("value")
        }
    }
}

#----------------------------------------------------------------------------
# Connection Validation function
#----------------------------------------------------------------------------
function ValidateConnection([string]$remoteServer, [int]$attempt = 10, [string]$source = "127.0.0.1") {
    Write-Info.ps1 "Validate the connection from $source to $remoteServer."

    $connected = $false
    try {
        while (!$connected -and $attempt -gt 0) {
            if ($PSVersionTable.PSVersion.Major -ge 7) {
                $result = Test-Connection -TargetName $remoteServer -Count 1 -ErrorAction SilentlyContinue
                $success = ($null -ne $result) -and ($result.Status -eq [System.Net.NetworkInformation.IPStatus]::Success)
            }
            else {
                $result = Test-Connection -Source $source -ComputerName $remoteServer -Count 1 -ErrorAction SilentlyContinue
                $success = ($null -ne $result) -and ($result.StatusCode -eq 0)

            }
            if ($success) {
                Write-Info.ps1 "$remoteServer is connected. Validation passed."
                $connected = $true
                break;
            }
            else {
                $connected = $false
                $attempt--
                Write-Info.ps1 "$remoteServer is not connected." -ForegroundColor yellow
                Start-Sleep 10
            }
        }
        if (!$connected) {
            Write-Info.ps1 "$remoteServer is still can not be connected. Will give up retrying and skip the cases related to $remoteServer."
            return $false
        }
    }
    catch {
        return $false
    }
    return $true
}


# Start service and its dependent services remotely
function StartServices($computerName, $serviceName, [int]$attempt = 3) {
    $status = $false

    while (!$status -and $attempt -gt 0) {
        # Get-Service -ComputerName was removed in PowerShell 7; use Invoke-Command instead
        $serviceInfo = Invoke-Command -ComputerName $computerName -ScriptBlock {
            Get-Service $using:serviceName -ErrorAction SilentlyContinue
        }

        if ($serviceInfo.status -ne "Running") {
            Write-Info.ps1 "The Service $serviceName is not running. Try to start the service." -ForegroundColor yellow
            Invoke-Command -ComputerName $computerName -ScriptBlock {
                Get-Service $using:serviceName | Start-Service -ErrorAction Continue
            }
            $attempt--
            Start-Sleep 5
        }
        else {
            $status = $true
            Write-Info.ps1 "The Service $serviceName is running. No need to start."
            break;

        }
    }
    return $status
}

function ValidateShareFolder($share) {
    Write-Info.ps1 "Check the existence of the shared folder path $share"
    $isExist = Test-Path -Path $share -PathType Container

    if ($null -eq $isExist -or $isExist -eq $false) {
        Write-Info.ps1 "Check the existence of the shared folder path $share failed."  -ForegroundColor yellow
        return $false
    }
    else {
        Write-Info.ps1 "Check the existence of the shared folder path $share succeeded."
    }

    try {
        $guid = [guid]::NewGuid().guid;
        $fileName = "testfile_$guid.txt"
        Write-Info.ps1 "Write a file $fileName to $share"

        $createdItem = New-Item -path "$share\$fileName" -type file -force -value "This is a test file to validate the shared folder connection."
        if ($createdItem.Exists -eq $true) {
            Write-Info.ps1 "Remove the $fileName from $share"
            Remove-Item -Path "$share\$fileName" -force
            return $true
        }
        else {
            return $false
        }
    }
    catch {
        Write-info.ps1 "Failed to create a file to $share with error: $_.Exception.Message."
        return $false
    }

}

function ValidateNodeConnection($dest, $source, $share) {
    #----------------------------------------------------------------------------
    #validate the connection to Node for Workgroup environment
    #----------------------------------------------------------------------------
    Write-Info.ps1 "#----------------------------------------------------------------------------"
    Write-Info.ps1 "Validate the connection to $dest"

    $isConnected = $false
    $isConnected = ValidateConnection -remoteServer $dest -source $source
    if ($isConnected -eq $false) {
        Write-Info.ps1 "Environment is not ready for running cases." -ForegroundColor yellow
        return $false
    }

    $succeed = ValidateShareFolder -share  $share
    if ($succeed -eq $false) {
        Write-Info.ps1 "Environment is not ready for running cases." -ForegroundColor yellow
        return $false
    }

    return $true
}

function ValidateDCServices {
    #----------------------------------------------------------------------------
    #validate the Routing and remote access service is started on DC01
    #----------------------------------------------------------------------------

    Write-Info.ps1 "#----------------------------------------------------------------------------"
    Write-Info.ps1 "Validate the Routing and remote access service is started on DC01"

    Write-Info.ps1 "Check Routing and Remote Access service on DC01 and try to start it if it's stopped."

    $service1Status = StartServices -computerName $DC01ComputerName  -serviceName "sstpsvc" -attempt 5
    $service2Status = StartServices -computerName $DC01ComputerName  -serviceName "rasman" -attempt 5
    $service4Status = StartServices -computerName $DC01ComputerName  -serviceName "Remoteaccess" -attempt 5

    Write-Info.ps1 "Validate the connection from Client01 IP1 to Node01 IP1."
    $firstConnection = ValidateConnection -remoteServer $Node01IP1 -source $Client01IP1 -attempt 4

    Write-Info.ps1 "Validate the connection from Client01 IP1 to Node01 IP2."
    $secondConnection = ValidateConnection -remoteServer $Node01IP2 -source $Client01IP1 -attempt 4

    Write-Info.ps1 "Validate the connection from Client01 IP2 to Node01 IP1."
    $thirdConnection = ValidateConnection -remoteServer $Node01IP1 -source $Client01IP2 -attempt 4

    Write-Info.ps1 "Validate the connection from Client01 IP2 to Node01 IP2."
    $fourthConnection = ValidateConnection -remoteServer $Node01IP2 -source $Client01IP2 -attempt 4

    if ( $firstConnection -eq $false) {
        Write-Info.ps1 "Client01 cannot connect to Node01. The environment is not ready for test cases run." -ForegroundColor red
    }

    if ($secondConnection -eq $false) {
        Write-Info.ps1 "Will udpate the SutAlternativeIPAddress for multiple channel cases." -ForegroundColor yellow
        Modify-ConfigFileNode.ps1 $Smb2TestSuitePtfConfig "SutAlternativeIPAddress" $Node01IP2
    }

    if ($thirdConnection -eq $false) {
        Write-Info.ps1 "Will udpate the ClientNic2IPAddress for multiple channel cases." -ForegroundColor yellow
        Modify-ConfigFileNode.ps1 $CommonTestSuitePtfConfig "ClientNic2IPAddress" $Client01IP2
    }

    if (($fourthConnection -eq $false) -or ($service4Status -eq $false)) {
        Write-Info.ps1 "Will udpate the SutAlternativeIPAddress and ClientNic2IPAddress for multiple channel cases." -ForegroundColor yellow
        Modify-ConfigFileNode.ps1 $Smb2TestSuitePtfConfig "SutAlternativeIPAddress" $Node01IP2
        Modify-ConfigFileNode.ps1 $CommonTestSuitePtfConfig "ClientNic2IPAddress" $Client01IP2
    }

    return ($service1Status -and $service2Status -and $service4Status -and $firstConnection -and $secondConnection -and $thirdConnection -and $fourthConnection)
}

function ValidateCluster($nodeName, $cred ) {
    #----------------------------------------------------------------------------
    #Validate the cluster service from Client01
    #----------------------------------------------------------------------------
    Write-Info.ps1 "#----------------------------------------------------------------------------"
    Write-Info.ps1 "Validate the cluster service on $nodeName"

    Write-Info.ps1 "Get-Cluster on $nodeName"
    $cluster = Invoke-Command -ScriptBlock { Get-Cluster } -ComputerName $nodeName -Credential $cred
    if ($null -eq $cluster ) {
        Write-Info.ps1 "Get-Cluster on $nodeName failed. Will skip the casess related to $nodeName." -ForegroundColor yellow
        return $false
    }

    Write-Info.ps1 ($cluster | Format-Table Name, Domain, SharedVolumesRoot, EnableSharedVolumes | Out-String)

    Write-Info.ps1 "Get-ClusterGroup on $nodeName"
    $clusterGroup = Invoke-Command -ScriptBlock { Get-ClusterGroup } -ComputerName $nodeName -Credential $cred
    if ($null -eq $clusterGroup ) {
        Write-Info.ps1 "Get-ClusterGroup on $nodeName failed. Will skip the casess related to $nodeName." -ForegroundColor yellow
        return $false
    }
    Write-Info.ps1 ($clusterGroup | Format-Table Name, State, GroupType, OwnerNode, Priority | Out-String)

    Write-Info.ps1 "Get-ClusterSharedVolume on $nodeName"
    $clusterSharedVolume = Invoke-Command -ScriptBlock { Get-ClusterSharedVolume } -ComputerName $nodeName -Credential $cred
    if ($null -eq $clusterSharedVolume ) {
        Write-Info.ps1 "Get-ClusterSharedVolume on $nodeName failed. Will skip the casess related to $nodeName."  -ForegroundColor yellow
        return $false
    }
    Write-Info.ps1 ($clusterSharedVolume | Format-Table Name, State, OwnerNode | Out-String)

    Write-Info.ps1 "Get-ClusterResource on $nodeName"
    $clusterResource = Invoke-Command -ScriptBlock { Get-ClusterResource } -ComputerName $nodeName -Credential $cred
    if ($null -eq $clusterResource ) {
        Write-Info.ps1 "Get-ClusterResource on $nodeName failed. Will skip the casess related to $nodeName."  -ForegroundColor yellow
        return $false
    }
    Write-Info.ps1 ($clusterResource | Sort-Object Name | Format-Table Name, PsComputerName, Cluster, OwnerNode, ResourceType, State, OwnerGroup | Out-String)

    Write-Info.ps1 "Get-ClusterNetwork on $nodeName"
    $clusterNetwork = Invoke-Command -ScriptBlock { Get-ClusterNetwork } -ComputerName $nodeName -Credential $cred
    if ($null -eq $clusterNetwork ) {
        Write-Info.ps1 "Get-ClusterNetwork on $nodeName failed. Will skip the casess related to $nodeName."  -ForegroundColor yellow
        return $false
    }
    Write-Info.ps1 ($clusterNetwork | Format-Table Name, State, Role, Address | Out-String)

    Write-Info.ps1 "Get-ClusterNetworkInterface on $nodeName"
    $clusterNetworkInterface = Invoke-Command -ScriptBlock { Get-ClusterNetworkInterface } -ComputerName $nodeName -Credential $cred
    if ($null -eq $clusterNetworkInterface ) {
        Write-Info.ps1 "Get-ClusterNetworkInterface on $nodeName failed. Will skip the casess related to $nodeName."  -ForegroundColor yellow
        return $false
    }
    Write-Info.ps1 ($clusterNetworkInterface | Sort-Object Name | Format-Table Node, Name, State, Network | Out-String)

    Write-Info.ps1 "Get-ClusterAccess on $nodeName"
    $clusterAccess = Invoke-Command -ScriptBlock { Get-ClusterAccess } -ComputerName $nodeName -Credential $cred
    if ($null -eq $clusterAccess ) {
        Write-Info.ps1 "Get-ClusterAccess on $nodeName failed. Will skip the casess related to $nodeName."  -ForegroundColor yellow
        return $false
    }
    Write-Info.ps1 ($clusterAccess | Out-String)
    return $true
}

Function ValidateDC01Connection {
    Write-info.ps1 "Validate the connection from Client01 IP1 to DC01."
    $status = ValidateConnection -remoteServer $DC01IP -source $Client01IP1
    if ($status -eq $false) {
        Write-info.ps1 "DC01 cannot be connected. All domain related cases will be excluded."  -ForegroundColor red
    }
    return $status
}

Function ValidateNode01Connection {
    Write-info.ps1 "Validate connection from Client01 IP1 to Node01 IP1."
    if ($ContextName -imatch "WorkGroup") {

        $status = ValidateNodeConnection -dest $Node01IP1 -source $Client01IP1 -share "\\$Node01IP1\SMBBasic"
    }
    else {
        $status = ValidateNodeConnection -dest $Node01IP1 -source $Client01IP1 -share "\\$Node01ComputerName\SMBBasic"
    }

    if ($status -eq $false) {
        Write-info.ps1 "Node01 cannot be connected. All case should be excluded."  -ForegroundColor red
    }
    return $status
}

Function ValidateNode02Connection {
    Write-info.ps1 "Validate connection from Client01 IP1 to Node02 IP1."
    $status = ValidateNodeConnection -dest $Node02IP1 -source $Client01IP1 -share "\\$Node02ComputerName\SMBBasic"
    if ($status -eq $false) {
        Write-info.ps1 "Node02 cannot be connected. Cluster cases should be excluded."  -ForegroundColor red
    }
    return $status
}

Function ValidateNode01Cluster {
    $status = ValidateCluster -nodeName $Node01ComputerName -cred $credential
    if ($status -eq $false) {
        Write-info.ps1 "Cluster is not prepared well. Cluster cases should be excluded."  -ForegroundColor red
    }
    return $status
}

Function ValidateGeneralFS {
    #----------------------------------------------------------------------------
    #Validate the General FS connection
    #----------------------------------------------------------------------------

    Write-Info.ps1 "#----------------------------------------------------------------------------"
    Write-Info.ps1 "Validate the General FS connection"

    $excludeClusterTestCases = $false

    $generalFSPath = ReadPtfconfigProperty -ptfconfigFile $serverFailoverPtfConfig -propertyName "ClusteredFileServerName"
    if ([System.String]::IsNullOrEmpty($generalFSPath)) {
        $excludeClusterTestCases = $true
    }
    else {
        $validateSMBClusteredShare = ValidateShareFolder -share "\\$generalFSPath\SMBClustered"
        if ($validateSMBClusteredShare -eq $false) {
            $excludeClusterTestCases = $true
        }
    }

    if ($excludeClusterTestCases)
    {
        Write-Info.ps1 "Failed when check existence of the General FS path. Will exclude GeneralFS related cases."
        #Modify ptfconfig to exclude the persistent handle, Replay durable handle, and some Appinstance ID cases which access the GeneralFS.
        Modify-ConfigFileNode.ps1 $CommonTestSuitePtfConfig "CAShareServerName" ""
        Modify-ConfigFileNode.ps1 $CommonTestSuitePtfConfig "CAShareName" ""
        Modify-ConfigFileNode.ps1 $serverFailoverPtfConfig "CAShareWithDataEncryption" ""
        Modify-ConfigFileNode.ps1 $serverFailoverPtfConfig "ClusteredFileServerName" ""
        return $false
    }

    # Have updated the ptfconfig to conclusive general fs cases. Dont need to exclude these cases.
    return $true
}

Function ValidateScaleoutFS {
    #----------------------------------------------------------------------------
    #Validate the Scale-out FS
    #----------------------------------------------------------------------------

    Write-Info.ps1 "#----------------------------------------------------------------------------"
    Write-Info.ps1 "Validate the Scale-out FS"

    $excludeClusterTestCases = $false

    $scaleoutFSPath = ReadPtfconfigProperty -ptfconfigFile $serverFailoverPtfConfig -propertyName "ClusteredScaleOutFileServerName"
    if ([System.String]::IsNullOrEmpty($scaleoutFSPath))

    {
        $excludeClusterTestCases = $true
    }
    else {
        $validateSMBClusteredShare = ValidateShareFolder -share "\\$scaleoutFSPath\SMBClustered"
        $validateSMBClusteredForceLevel2Share = ValidateShareFolder -share "\\$scaleoutFSPath\SMBClusteredForceLevel2"
        if ($validateSMBClusteredShare -eq $false -or $validateSMBClusteredForceLevel2Share -eq $false) {
            $excludeClusterTestCases = $true
        }
    }

    if ($excludeClusterTestCases) {
        Write-Info.ps1 "Failed when check existence of the Scale out FS path. Will exclude Scale-out related cases."  -ForegroundColor yellow
        Modify-ConfigFileNode.ps1 $serverFailoverPtfConfig "ClusteredScaleOutFileServerName" ""
        Modify-ConfigFileNode.ps1 $serverFailoverPtfConfig "OptimumNodeOfAsymmetricShare" ""
        Modify-ConfigFileNode.ps1 $serverFailoverPtfConfig "NonOptimumNodeOfAsymmetricShare" ""

        Modify-ConfigFileNode.ps1 $SMB2ModelPtfConfig "ScaleOutFileServerName" ""
        Modify-ConfigFileNode.ps1 $rsvdPtfConfig "ShareContainingSharedVHD" ""
        Modify-ConfigFileNode.ps1 $sqosPtfConfig "ShareContainingSharedVHD" ""
        return $false
    }

    # Have updated the ptfconfig to conclusive scale-out fs cases. Dont need to exclude these cases.
    return $true
}

$environmentValidateStatusFile = "$logPath\EnvironmentStatus.txt"
New-Item $environmentValidateStatusFile -type file -Force

#Domain cluster
if ($ContextName -imatch "Domain_Cluster") {
    #Validate DC connection
    if (ValidateDC01Connection -eq $true) {
        $environmentStatus = "DCConnected"
        [string]::Format("{0}|{1}|{2}", "DC01", "DC01 Connection", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
    else {
        [string]::Format("{0}|{1}|{2}", "DC01", "DC01 Connection", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
    }

    #Validate Node01 connection
    if (ValidateNode01Connection -eq $true) {
        $environmentStatus += "|Node01Connected"
        [string]::Format("{0}|{1}|{2}", "Node01", "Node01 Connection", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
    else {
        [string]::Format("{0}|{1}|{2}", "Node01", "Node01 Connection", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
    }

    #Validate DC Routing and Remote Access Services
    if (ValidateDCServices -eq $true) {
        $environmentStatus += "|DCRemoteAndRoutingServiceUp"
        [string]::Format("{0}|{1}|{2}", "DC01", "RemoteAndRoutingService", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
    else {
        [string]::Format("{0}|{1}|{2}", "DC01", "RemoteAndRoutingService", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
    }

    #Validate cluster
    $clusterStatus = ValidateNode01Cluster
    $node2Status = ValidateNode02Connection
    $generalFSStatus = ValidateGeneralFS
    $scaleoutFSStatus = ValidateScaleoutFS
    if (($clusterStatus -eq $true) -and ($node2Status -eq $true) -and ($generalFSStatus -eq $true) -and ($scaleoutFSStatus -eq $true)) {
        $environmentStatus += "|ClusterUp"
        [string]::Format("{0}|{1}|{2}", "Node01", "Cluster", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
        [string]::Format("{0}|{1}|{2}", "Node01", "GeneralFS", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
        [string]::Format("{0}|{1}|{2}", "Node01", "Scale-out FS", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
    else {
        [string]::Format("{0}|{1}|{2}", "Node01", "Cluster", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
        [string]::Format("{0}|{1}|{2}", "Node01", "GeneralFS", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
        [string]::Format("{0}|{1}|{2}", "Node01", "Scale-out FS", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
}

#Domain noncluster
if ($ContextName -imatch "Domain_NonCluster") {
    #Validate DC connection
    if (ValidateDC01Connection -eq $true) {
        $environmentStatus += "DCConnected"
        [string]::Format("{0}|{1}|{2}", "DC01", "DC01 Connection", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
    else {
        [string]::Format("{0}|{1}|{2}", "DC01", "DC01 Connection", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
    }

    #Validate Node01 connection
    if (ValidateNode01Connection -eq $true) {
        $environmentStatus += "|Node01Connected"
        [string]::Format("{0}|{1}|{2}", "Node01", "Node01 Connection", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
    else {
        [string]::Format("{0}|{1}|{2}", "Node01", "Node01 Connection", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
}

#Workgroup
if ($ContextName -imatch "WorkGroup") {
    #Validate Node01 connection
    if (ValidateNode01Connection -eq $true) {
        $environmentStatus += "Node01Connected"
        [string]::Format("{0}|{1}|{2}", "Node01", "Node01 Connection", "On") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
    else {
        [string]::Format("{0}|{1}|{2}", "Node01", "Node01 Connection", "Down") | Out-File -Append -FilePath $environmentValidateStatusFile
    }
}

Write-Info.ps1 "#----------------------------------------------------------------------------"
Write-Info.ps1 "Validate environment finished."
Write-Info.ps1 "#----------------------------------------------------------------------------"

Stop-Transcript -ErrorAction Continue

return $environmentStatus
