# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param
(
    [string]$Workspace,
    # The virtual hard disk for creating a server VM
    [string]$VHDName = "18362.1.amd64fre.19h1_release.190318-1202_server_serverdatacenter_en-us_vl_VS2017.vhd",
    # The path
    [string]$VHDPath = "\\pet-storage-04\PrototestRegressionShare\VHDShare"
)

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$WinteropProtocolTesting = "$Workspace\..\..\WinteropProtocolTesting"
$HostOsBuildNumber       = "" + [Environment]::OSVersion.Version.Major + "." + [Environment]::OSVersion.Version.Minor
Write-Host "=============================================="
Write-Host "InvocationPath: $InvocationPath"
Write-Host "Workspace： $Workspace"
Write-Host "WinteropProtocolTesting: $WinteropProtocolTesting"
Write-Host "=============================================="

function Read-Configurationfile {
    Write-Host "Read and parse the XML configuration file."
    if(!(Test-Path -Path ".\InstallPrerequisites.xml")){
        Write-Error "Cannot find InstallPrerequisites.xml" Exit
    }
    [Xml]$Script:Setup = Get-Content ".\InstallPrerequisites.xml"
    $Script:Setup | Format-TestSuiteXml -Indent 4
    $Script:VM = $Script:Setup.lab.servers.vm
}

#------------------------------------------------------------------------------------------
# Download VHD from share folder to workspace
#------------------------------------------------------------------------------------------

function Download-VHD {
    if(!(Test-Path "$VHDPath\$VHDName")){
        Write-Host "Cannot find the VHD on $VHDPath\$VHDName" Exit
    }
    Write-Host "Copy VHD from $VHDPath\$VHDName to $WinteropProtocolTesting\VM\InstallPrerequisites..."
    if(!(Test-Path "$WinteropProtocolTesting\VM\InstallPrerequisites")) {
        mkdir "$WinteropProtocolTesting\VM\InstallPrerequisites"
    }
    if(Test-Path "$WinteropProtocolTesting\VM\InstallPrerequisites\InstallPrerequisites.vhd") {
        Remove-Item "$WinteropProtocolTesting\VM\InstallPrerequisites\InstallPrerequisites.vhd" -Force
    }
    Copy-Item "$VHDPath\$VHDName" -Destination "$WinteropProtocolTesting\VM\InstallPrerequisites\" -Force
    Rename-Item "$WinteropProtocolTesting\VM\InstallPrerequisites\$VHDName" -NewName "InstallPrerequisites.vhd" -Force
    Write-Host "Copy VHD finished"
}

#------------------------------------------------------------------------------------------
# Check the prerequisites of the host machine before setup test suite environment
#------------------------------------------------------------------------------------------
Function Check-HostPrerequisites {

    Write-Host "Check prerequisites of the host for test suite environment setup"

    Write-Host "Check if the host operating system version is supported or not."
    if ([Double]$Script:HostOsBuildNumber -le [Double]"6.1") {
        Write-Host "Unsupported operating system version $Script:HostOsBuildNumber. Must be larger than 6.1." -BackgroundColor "Red" -Exit
    }
    else {
        Write-Host "Supported operating system version $Script:HostOsBuildNumber."
    }

    Write-Host "Check if the host has enabled router by registry key."
    # http://technet.microsoft.com/en-us/library/cc962461.aspx
    If ((Get-ItemProperty -path HKLM:\system\CurrentControlSet\services\Tcpip\Parameters -name IpEnableRouter -ErrorAction Silentlycontinue).ipenablerouter -ne 1) {
        Write-Host "Router is disabled. Registry key IpEnableRouter under path HKLM:\system\CurrentControlSet\services\Tcpip\Parameters is not set to 1. Set it now..."
        Set-ItemProperty -Path HKLM:\system\CurrentControlSet\services\Tcpip\Parameters -Name IpEnableRouter -Value 1
    }
    else {
        Write-Host "Router is enabled."
    }

    Write-Host "Check if `"RSAT-Hyper-V-Tools`" feature is installed or not."
    Write-Host "Import ServerManager module if not imported."
    Import-Module ServerManager
    $FeatureName = "RSAT-Hyper-V-Tools"
    $Feature = Get-WindowsFeature | Where { $_.Name -eq "$FeatureName" }
    if($Feature.Installed -eq $false) {
        Write-Host "Feature not installed. Install it now..."
        Add-WindowsFeature -Name $FeatureName -IncludeAllSubFeature -IncludeManagementTools
        Wait-TestSuiteActivityComplete -ActivityName "Install $FeatureName" -TimeoutInSeconds 5
    }
    else {
        Write-Host "Feature already installed." -BackgroundColor "Blue"
    }
    
    Write-Host "Check if `"Hyper-V v3.0 PowerShell Module`" is imported:"
    if (!(Get-Module -ListAvailable Hyper-V)) {
        Write-Host "Module not imported. Import it now..." -BackgroundColor "Yellow"
        Import-Module Hyper-V
    } 
    else {
        Write-Host "Module already imported."
    }
}

function Clean-VM {
    Write-Host "Clean up the VM. VMName: $($Script:VM.Name)"
    $HostVms = (Get-VM  | Select-Object -Property Name)
    $HostVms | ForEach-Object {
        if($_.Name -eq $Script:VM.Name){
            Write-Host "The VM already exist, will be deleted."
            Remove-VM -Name $TestVMName -Force
        }
    }
}

function Deploy-VirtualNetworkSwitches {
    Write-Host "Deploy virtual network switches for this test suite."
    $VNetName = $Script:Setup.lab.network.name
    $VmNetworkAdapter = Get-VMNetworkAdapter -All | Where { $_.Name -eq $VNetName }
    if ($VmNetworkAdapter -eq $null) {
        Write-Host "Create a new internal virtual switch."
        New-VMSwitch -Name $VNetName -SwitchType Internal
        # Wait for the operating system to refresh the newly created network adapter
        Wait-TestSuiteActivityComplete -ActivityName "virtual switch $($VNetName)" -TimeoutInSeconds 5
    }
    $VmNetworkAdapter = Get-VMNetworkAdapter -All | Where { $_.Name -eq $Vnet.name }
}

function Main {    
    Read-Configurationfile
    Download-VHD
    Check-HostPrerequisites
    Clean-VM
    Deploy-VirtualNetworkSwitches
}

Main