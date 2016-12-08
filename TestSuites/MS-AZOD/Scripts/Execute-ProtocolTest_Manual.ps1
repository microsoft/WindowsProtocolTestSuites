########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Execute-ProtocolTest.ps1
## Purpose:        Protocol Test Suite Entry Point for MS-ADOD
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2
##
##############################################################################
Param(
    [string]$protocolName          = "MS-AUTHSOD",
    [string]$WorkingDir            = "D:\WinBlueRegressionTest", 
    [string]$testResultDir         = "D:\WinBlueRegressionTest\TestResults\$protocolName",
    [string]$UserNameInVM          = "administrator",
    [string]$UserPwdInVM           = "Password01!",
    [string]$DomainInVM            = "contoso.com",
    [string]$TestDirInVM           = "C:\Test"
	)
Write-Host protocolName= $protocolName
Write-Host WorkingDir= $WorkingDir
Write-Host testResultDir= $testResultDir
Write-Host UserNameInVM= $UserNameInVM
Write-Host UserPwdInVM= $UserPwdInVM
Write-Host DomainInVM= $DomainInVM
Write-Host TestDirInVM= $TestDirInVM
#-------------------------------------------------------------------------------
# Function: Get-VMParameters
# Usage   : Get VM configure parameters for specific VM
#-------------------------------------------------------------------------------
Function Get-VMParameters(
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$ConfigPath = "D:\WinBlueRegressionTest\MS-AUTHSOD\Scripts\protocol.xml",
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$VMName,
    [parameter(Mandatory=$true)]
    [ValidateScript({$_.Value.GetType().Name -eq "Hashtable"})]
    [ref]$RefParamArray)
{
    Write-Host ConfigPath= $ConfigPath
    Write-Host VMName= $VMName
    
    $paramArray = $RefParamArray.Value
    [xml]$content = Get-Content $ConfigPath -ErrorAction Stop

    try 
    {
        $currentVM = $content.SelectSingleNode("//vm[hypervname=`'$VMName`']")

        foreach ($paramNode in $currentVM.ChildNodes)
        {
            $paramArray[$paramNode.Name] = $paramNode.InnerText
        }
    }
    catch
    {
        [String]$Emsg = "Unable to read parameters from protocol.xml. Error happened: " + $_.Exception.Message
        throw $Emsg
    }
}

#----------------------------------------------------------------------------
# Define call stack functions
#----------------------------------------------------------------------------

function global:EnterCallStack($scriptName)
{
    if($scriptName -ne "Execute-ProtocolTest.ps1")
    {
	    $global:indentControl++
    }
    $tab = ""
    for ($i=1; $i -le $global:indentControl; $i++)
    {
	    $tab += "    "
    }
    $date = get-date -Format "M/d/yyyy hh:mm:ss"
    ("") | Out-File -FilePath $callStackLogFile -Append -Force
    ($tab + "Start $scriptName    " + $date) | Out-File -FilePath $callStackLogFile -Append -Force
}

function global:ExitCallStack($scriptName)
{

    $tab = ""
    for ($i=1; $i -le $global:indentControl; $i++)
    {
	    $tab += "    "
    }
    if($scriptName -ne "Execute-ProtocolTest.ps1")
    {
	    $global:indentControl--
    }
    $date = get-date -Format "M/d/yyyy hh:mm:ss"
    ($tab + "Exit $scriptName    " + $date) | Out-File -FilePath $callStackLogFile -Append -Force
}

#-------------------------------------------------------------------------------
# Function: Main
# Remark  : Main script starts here.
#-------------------------------------------------------------------------------

[int]$global:indentControl = 0
$global:callStackLogFile = "$testResultDir\Execute-ProtocolTest.ps1.CallStack.log"


if($function:EnterCallStack -ne $null)
{
   EnterCallStack "Execute-ProtocolTest.ps1"
}

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
Stop-Transcript -ErrorAction Continue | Out-Null
Start-Transcript -Path "$testResultDir\Execute-ProtocolTest.ps1.log" -Append -Force

#----------------------------------------------------------------------------
# Verify parameters
#----------------------------------------------------------------------------
if ($protocolName -ne "MS-AUTHSOD")
{
  Throw "$protocolName is not correct. Only MS-AUTHSOD is allowed."
}

Set-StrictMode -v 2

#----------------------------------------------------------------------------
# Define source folders in VM Host
#----------------------------------------------------------------------------
$srcScriptLibPathOnHost = $workingDir + "\ScriptLib\"
$srcToolPathOnHost      = $workingDir + "\Tools\"
$srcDataPathOnHost      = $workingDir + "\$protocolName\Data\"
$srcMSIInstallOnHost    = $workingDir + "\$protocolName\Deploy\"
$srcScriptPathOnHost    = $workingDir + "\$protocolName\Scripts\"
$srcTestSuitePathOnHost = $workingDir + "\$protocolName\Bin"
$requirementSpecPath    = $workingDir + "\$protocolName\Scripts"
$VMConfigFile 		    = $srcScriptPathOnHost + "\$protocolName.xml"

#----------------------------------------------------------------------------
# Define test suite endpoint, should be "Server" or "Client"
#----------------------------------------------------------------------------
$global:testResultDir = $testResultDir

#----------------------------------------------------------------------------
# Get VM folder name and computer name
#----------------------------------------------------------------------------
Push-Location $srcScriptPathOnHost

Write-Host "Get VM folder name and computer name..."  -foregroundcolor Yellow
$KDCParamArray = @{}
$ASParamArray = @{}
$PDCParamArray = @{}
$ClientParamArray = @{}
$DriverParamArray = @{}
[xml]$VMConfig = Get-Content -Path $VMConfigFile
$KDCVMName = "MS-AUTHSOD-KDC"
Get-VMParameters -ConfigPath $VMConfigFile -VMName $KDCVMName -RefParamArray ([ref]$KDCParamArray)
$KDCComputerIP = $KDCParamArray["ip"]
$KDCComputerName = $KDCParamArray["name"]
$ASVMName = "MS-AUTHSOD-AS"
Get-VMParameters -ConfigPath $VMConfigFile -VMName $ASVMName -RefParamArray ([ref]$ASParamArray)
$ASComputerIP = $ASParamArray["ip"]
$ASComputerName = $ASParamArray["name"]
$PDCVMName = "MS-AUTHSOD-PDC"
Get-VMParameters -ConfigPath $VMConfigFile -VMName $PDCVMName -RefParamArray ([ref]$PDCParamArray)
$PDCComputerIP = $PDCParamArray["ip"]
$PDCComputerName = $PDCParamArray["name"]
$ClientVMName = "MS-AUTHSOD-Client"
Get-VMParameters -ConfigPath $VMConfigFile -VMName $ClientVMName -RefParamArray ([ref]$ClientParamArray)
$ClientComputerIP = $ClientParamArray["ip"]
$ClientComputerName = $ClientParamArray["name"]
$DriverVMName = "MS-AUTHSOD-Driver"
Get-VMParameters -ConfigPath $VMConfigFile -VMName $DriverVMName -RefParamArray ([ref]$DriverParamArray)
$DriverComputerIP = $DriverParamArray["ip"]
$DriverComputerName  = $DriverParamArray["name"]
Write-Host "$protocolName is executing on $PDCVMName, $ClientVMName and $DriverVMName..." -foregroundcolor Yellow
Pop-Location

Push-Location $srcScriptLibPathOnHost

if(!(Test-Path -Path $testResultDir))
{
    md $testResultDir
}

#----------------------------------------------------------------------------
# Verify we have the hyperv module to import
#----------------------------------------------------------------------------
Write-Host "Verify we have the hyperv module to import ..." -foregroundcolor Yellow
if ([bool](Get-Module -ListAvailable hyperv) -eq $false){
   Robocopy $WorkingDir\hyperv $env:SystemDrive\Windows\System32\WindowsPowerShell\v1.0\Modules\hyperv /mir 
   }
if ([bool](Get-Module hyperv) -eq $false){
   Import-Module Hyperv;
   Write-Host "Importing Hyperv Module" -ForegroundColor Green
}

#----------------------------------------------------------------------------
# Create virtual network according to VMConfig if not exist
#----------------------------------------------------------------------------
Write-Host "Create virtual network according to VMConfig if not exist ..." -foregroundcolor Yellow
$VMConfig.lab.network.vnet | Foreach {

   # Create the network if it doesn't exitst
   If ([bool](Get-VMSwitch $_.name) -eq $false)
    {
	    # Create the new switch
	    Write-Host ("Creating Switch" + $_.name)
	    if($_.type -eq "Internal") 
        {
		    New-VMInternalSwitch -VirtualSwitchName $_.name
	    } 
        elseif ($_.type -eq "Private")
        {
		    New-VMPrivateSwitch -VirtualSwitchName $_.name
	    } 
        else 
        {
		    Write-Error "Create External network are not supported."
	    }			
   }
}

# Set automation controller
$VMConfig.lab.network.vnet | Foreach {

   $adapterName = $_.name		
   $internalEthernetPort = Get-WMIObject -namespace "root\virtualization" -query "SELECT * FROM Msvm_InternalEthernetPort" | where {$_.ElementName -eq $adapterName}	
   $networkAdapter = Get-WmiObject -Class win32_networkadapter | where {$_.GUID -eq $internalEthernetPort.DeviceID}
   if($networkAdapter -ne $null)
   {
	    # Set the IP and subnet to the values in the XML
	    netsh interface ipv4 set address $networkAdapter.InterfaceIndex static $_.ip $_.subnet $_.gateway
   }
}

##----------------------------------------------------------------------------
## Run VMs
##----------------------------------------------------------------------------
$DriverFullUsername = "$DriverComputerIP\$UserNameInVM"
$DriverSystemDrive  = "C:"
.\WaitFor-ComputerReady.ps1 $DriverComputerIP $DriverFullUsername $UserPwdInVM

If($TestDirInVM -match "SYSTEMDRIVE")
{
    $VmSystemDrive = .\Get-RemoteSystemDrive.ps1 $DriverComputerIP $DriverFullUsername $UserPwdInVM
    $TestDirInVM = $TestDirInVM.Replace("SYSTEMDRIVE", $VmSystemDrive )
}

#----------------------------------------------------------------------------
# Get Timestamp of start configuration
#----------------------------------------------------------------------------
Write-Host "Initial time stamp and start config test ENV ..." -ForegroundColor Yellow
.\Get-Timestamp.ps1 $protocolName initial $testResultDir
.\Get-Timestamp.ps1 $protocolName startconfigAUTHSOD $testResultDir

#----------------------------------------------------------------------------
# Config AutoLogon For Server & Client VMs
#----------------------------------------------------------------------------
#Write-Host "Start to set autologon for VMs..."  -foregroundcolor Yellow
#Write-Host "Start to set autologon for $ClientComputerIP ..."
#.\Config-AutoLogon.ps1 $ClientComputerIP "$ClientComputerIP\$UserNameInVM" "$UserPwdInVM"
#Write-Host "Start to set autologon for $DriverComputerIP ..."
#.\Config-AutoLogon.ps1 $DriverComputerIP "$DriverComputerIP\$UserNameInVM" "$UserPwdInVM"

#.\WaitFor-ComputerReady.ps1 $ClientComputerIP "$ClientComputerIP\$UserNameInVM" "$UserPwdInVM"
#.\WaitFor-ComputerReady.ps1 $DriverComputerIP "$DriverComputerIP\$UserNameInVM" "$UserPwdInVM"

#----------------------------------------------------------------------------
# Copy test contents to VMs
#----------------------------------------------------------------------------
Write-Host "Copy test files..." -foregroundcolor Yellow
.\Copy-TestFile.ps1 $srcScriptLibPathOnHost $srcScriptPathOnHost $srcToolPathOnHost $srcTestSuitePathOnHost $DriverComputerIP $testDirInVM "$DriverFullUsername" "$UserPwdInVM"

#----------------------------------------------------------------------------
# Start to run test suite on VM client
#----------------------------------------------------------------------------
$RSPathInVM       = "$testDirInVM\Bin"
$testCaseScript = "MS-AUTHSOD-RunAllTestCases.ps1"
$isTestSuiteInstalled = $true
$testCaseTimeout = 3600
$runTestCmd       = "cmd /c powershell $testDirInVM\Scripts\$testCaseScript"
$createTaskCmd    = "cmd /c powershell schtasks /Create /RU $UserNameInVM /SC ONCE /ST 00:00 /TN RunAllCases /TR `'$runTestCmd`' /IT /F"

Write-Host "Create running Test Suite task and restart..." -foregroundcolor Yellow
.\Get-Timestamp.ps1 $protocolName startRunTestAUTHSOD $testResultDir
.\RemoteExecute-Command.ps1 $DriverComputerIP "$testDirInVM\Scripts\RestartAndRun.bat `"$createTaskCmd`"" $DriverFullUsername $UserPwdInVM

Write-Host "Wait for computer to be ready..." -foregroundcolor Yellow
Start-Sleep 20 #wait for restart
.\WaitFor-ComputerReady.ps1 $DriverComputerIP "$DriverFullUsername" "$UserPwdInVM"
Start-Sleep 600 #wait for network stable

Write-Host "Start running Test Suite ..." -foregroundcolor Yellow
cmd /c schtasks /S $DriverComputerIP /U $UserNameInVM /P $UserPwdInVM /Run /TN RunAllCases

#----------------------------------------------------------------------------
# Wait for test done
#----------------------------------------------------------------------------
.\WaitFor-ComputerReady.ps1 $DriverComputerIP $DriverFullUsername $userPwdInVM $testDirInVM "test.started.signal" 120
Write-Host "Wait for testing done ..." -foregroundcolor Yellow
.\WaitFor-ComputerReady.ps1 $DriverComputerIP $DriverFullUsername $UserPwdInVM $testDirInVM "test.finished.signal" $testCaseTimeout

#----------------------------------------------------------------------------
# Get Timestamp of test done
#----------------------------------------------------------------------------
.\Get-Timestamp.ps1 $protocolName testdoneAUTHSOD $testResultDir

#----------------------------------------------------------------------------
# Copy result to host from VM 
#----------------------------------------------------------------------------
Write-Host "Copy test result from test VM to host machine ..." -foregroundcolor Yellow

.\Copy-TestResult $DriverComputerIP "$DriverSystemDrive\*.signal" "$testResultDir" $DriverFullUsername $userPwdInVM
.\Copy-TestResult $DriverComputerIP "$DriverSystemDrive\*.log" "$testResultDir" $DriverFullUsername $userPwdInVM
.\Copy-TestResult $DriverComputerIP "$testDirInVM\TestResults" "$testResultDir" $DriverFullUsername $userPwdInVM
.\Copy-TestResult $DriverComputerIP "$testDirInVM\TestLog" "$testResultDir" $DriverFullUsername $userPwdInVM
# Clean up signals after copy out from VM
.\RemoteExecute-Command.ps1 $DriverComputerIP "$testDirInVM\Scripts\CleanupSignals.cmd" $DriverFullUsername $userPwdInVM

Push-Location $srcScriptPathOnHost
.\Fix-XmlLogFile -logPath "$testResultDir"
Pop-Location

Push-Location $testResultDir
Get-ChildItem -Recurse -Force -Include "Documents and Settings"| Remove-Item -Force -Recurse
Pop-Location

#----------------------------------------------------------------------------
# Generate report in HTML format.
#----------------------------------------------------------------------------
#Write-Host "Generate report in HTML format ..." -foregroundcolor Yellow
#.\Generate-Report.ps1 "$protocolName" "$requirementSpecPath" "$testResultDir" "Client+Both" "Server" "$srcScriptLibPathOnHost" "$srcToolPathOnHost"
#Write-Host "$protocolName execute completed." -foregroundcolor Green

Pop-Location

#----------------------------------------------------------------------------
# Stop logging and exit
#----------------------------------------------------------------------------
Stop-Transcript -ErrorAction Continue | Out-Null
# Write Call Stack
if($function:ExitCallStack -ne $null)
{
   ExitCallStack "Execute-ProtocolTest.ps1"
}


exit 0
