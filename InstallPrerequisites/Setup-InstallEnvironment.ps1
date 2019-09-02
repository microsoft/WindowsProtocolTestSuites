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
$Script:VmDirPath = "$WinteropProtocolTesting\VM\InstallPrerequisites\"

Write-Host "=========================================================="
Write-Host "InvocationPath: $InvocationPath"
Write-Host "Workspace： $Workspace"
Write-Host "WinteropProtocolTesting: $WinteropProtocolTesting"
Write-Host "VmDirPath: $Script:VmDirPath"
Write-Host "=========================================================="

#------------------------------------------------------------------------------------------
# Sleeping for a particular amount of time to wait for an activity to be completed
#------------------------------------------------------------------------------------------
Function Wait-TestSuiteActivityComplete {
    Param(
    [Parameter(ValueFromPipeline=$True)]
    [string]$ActivityName,
    [int]$TimeoutInSeconds = 0)

    for ([int]$Tick = 0; $Tick -le $TimeoutInSeconds; $Tick++) {
        Write-Progress -Activity "Wait for $ActivityName ..." -SecondsRemaining ($TimeoutInSeconds - $Tick) -PercentComplete (($Tick / $TimeoutInSeconds) * 100)
        if ($Tick -lt $TimeoutInSeconds) { Start-Sleep 1 }
    }
    Write-Progress -Activity "Wait for $ActivityName ..." -Completed
}
#------------------------------------------------------------------------------------------
# Format the input xml file and display it to the screen
#------------------------------------------------------------------------------------------
Function Format-TestSuiteXml {
    Param(
    [Parameter(ValueFromPipeline=$True)]
    [xml]$Xml,
    [int]$Indent = 2)

    Process {
        $StringWriter = New-Object System.IO.StringWriter
        $XmlWriter = New-Object System.Xml.XmlTextWriter $StringWriter
        $XmlWriter.Formatting = "indented"
        $XmlWriter.Indentation = $Indent
        [xml]$Xml.WriteContentTo($XmlWriter)
        $XmlWriter.Flush()
        $StringWriter.Flush()

        # Output the result
        Write-Output $("`n" + $StringWriter.ToString())
    }
}
function Read-Configurationfile {
    Write-Host "Read and parse the XML configuration file."
    if(!(Test-Path -Path "$InvocationPath\InstallPrerequisites.xml")){
        Write-Error "Cannot find InstallPrerequisites.xml" Exit
    }
    [Xml]$Script:Setup = Get-Content "$InvocationPath\InstallPrerequisites.xml"
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
    $VNetName = $Script:Setup.lab.network.vnet.name
    Write-Host "VNetName: $VNetName"
    $VmNetworkAdapter = Get-VMNetworkAdapter -All | Where { $_.Name -eq $VNetName }
    if ($VmNetworkAdapter -eq $null) {
        Write-Host "Create a new internal virtual switch. Name:$VNetName"
        New-VMSwitch -Name $VNetName -SwitchType Internal
        # Wait for the operating system to refresh the newly created network adapter
        Wait-TestSuiteActivityComplete -ActivityName "virtual switch $($VNetName)" -TimeoutInSeconds 5
    }
    $VmNetworkAdapter = Get-VMNetworkAdapter -All | Where { $_.Name -eq $Vnet.name }
    if ($VmNetworkAdapter -eq $null) {
        Write-Host $("No virtual network adapter found by the newly created virtual switch's name - " + $Vnet.name) -Exit
    }
    else {
        $VmNetworkAdapter | Format-Table -Property Name, SwitchName, DeviceID, MacAddress, Status -AutoSize

        Write-Host "Set the statistic IP address and subnet to this physical network adapter."
        if($([IPAddress]$Vnet.ip).AddressFamily -eq "InterNetwork") {
            netsh interface ipv4 set address $NetworkAdapter.InterfaceIndex static $Vnet.ip $Vnet.subnet
            Write-TestSuiteSuccess $("IP address - " + $Vnet.ip + " and subnet - " + $Vnet.subnet + " have been updated.")
        }
        else {
            netsh interface ipv6 set address $NetworkAdapter.interfaceindex $Vnet.ip
            Write-TestSuiteSuccess $("IP address - " + $Vnet.ip + " has been updated.")
        }
    }
}

#------------------------------------------------------------------------------------------
# Create virtual machine
#------------------------------------------------------------------------------------------
Function Create-TestSuiteVM {
    Param(
    [Parameter(ValueFromPipeline=$True)]
    $Vm)
    
    Process {

        Write-Host "Start creating VM for $($Vm.hypervname)."

        Write-Host "Create a new virtual machine with name - $($Vm.hypervname) under location - $($Script:VmDirPath)."
        New-VM -Name $Vm.hypervname -Path $Script:VmDirPath

        Write-Host "Configure the CPU for this virtual machine to - $($Vm.cpu)."
        Set-VM -Name $Vm.hypervname -ProcessorCount $Vm.cpu

        $VmMem = [int]$Vm.memory * 1024 * 1024
        Write-Host "Configure the memory for this virtual machine to - $($Vm.memory) MB ($VmMem Bytes)."
        if (($Vm.minimumram -ne $null) -and ($Vm.maximumram -ne $null)) {
            $MinMem = [int]$Vm.minimumram * 1024 * 1024
            $MaxMem = [int]$Vm.maximumram * 1024 * 1024
            Write-Host "Minimum memory - $($Vm.minimumram) MB ($MinMem Bytes) and Maximum memory - $($Vm.maximumram) MB ($MaxMem Bytes)."
            Set-VM -Name $Vm.hypervname -DynamicMemory -MemoryStartupBytes $VmMem -MemoryMinimumBytes $MinMem -MemoryMaximumBytes $MaxMem
        }
        else {
            Set-VM -Name $Vm.hypervname -StaticMemory -MemoryStartupBytes $VmMem
        }

        Write-Host "Remove the existing virtual network adapters of this virtual machine."
        Remove-VMNetworkAdapter -VMName $Vm.hypervname
        
        Write-Host "Add a new virtual network adapter to this virtual machine, and connect it to the following virtual switches."
        $NicNumber = 0;
        [array]$ServerVnet    = $Vm.vnet
        $VirtualSwitch = $ServerVnet[0]
        foreach($ip in $Vm.ip) {
            if($ServerVnet.Count -gt 1)
            {
                $VirtualSwitch = $ServerVnet[$NicNumber]
            }
            
            Write-Host "set virtual network adapter for ip:$ip - $VirtualSwitch"
            Add-VMNetworkAdapter -VMName $Vm.hypervname -SwitchName $VirtualSwitch
            $NicNumber++;
        }
        
        Write-Host "Check whether VHD file exists or not."
        $Vm.disk = "$WinteropProtocolTesting\VM\InstallPrerequisites\\InstallPrerequisites.vhd"
        Write-Host "Vm.disk: $Vm.disk"
        if (!(Test-Path $Vm.disk)) {
            Write-Host "$($Vm.disk) file not found." -Exit
        }

        Write-Host "Attach VHD to this virtual machine."
        Add-VMHardDiskDrive -VMName $Vm.hypervname -ControllerType IDE -ControllerNumber 0 -ControllerLocation 0 -Path $Vm.disk

        Write-Host "Set the VM note with the Current User, Computer Name and IP Addresses (The note will be shown in VStorm Portal as VM Description)."
        $VmNote = $env:USERNAME + ": " + $Vm.name + ": " + $Vm.ip
        Set-VM -VMName $Vm.hypervname -Notes $VmNote
    }
}
Function Deploy-TestSuiteVirtualMachines {
    Download-VHD
    $Script:VM | Sort -Property installorder | Create-TestSuiteVM
}
function Main {    
    Read-Configurationfile
    Check-HostPrerequisites
    Clean-VM
    Deploy-VirtualNetworkSwitches
    Deploy-TestSuiteVirtualMachines
    Deploy-TestSuiteVirtualMachines
}

Main