#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Execute-ProtocolTest.ps1
## Purpose:        Protocol Test Suite Entry Point for FileSharing
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 8.1
## Copyright (c) Microsoft Corporation. All rights reserved.
##
##############################################################################

param(
[string]$protocolName          = "MS-AZOD", 
[string]$WorkingDir            = "D:\WinteropProtocolTesting", 
[string]$testResultDir         = "D:\WinteropProtocolTesting\TestResults\MS-AZOD",
[string]$ContextName           = "",
[string]$UserNameInVM          = "administrator",
[string]$userPwdInVM           = "Password01!",
[string]$DomainInVM            = "contoso.com",
[string]$TestDirInVM           = "C:\Test"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath"

#----------------------------------------------------------------------------
# Define call stack functions
#----------------------------------------------------------------------------
[int]$global:indentControl = 0
$global:callStackLogFile = "$testResultDir\Execute-ProtocolTest.ps1.CallStack.log"
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
# Define source folders
#----------------------------------------------------------------------------
Write-Info.ps1 "Define source folders..." -foregroundcolor Yellow
$srcScriptLibPathOnHost = $workingDir + "\ScriptLib"
$srcToolPathOnHost      = $workingDir + "\Tools"
$srcScriptPathOnHost    = $workingDir + "\$protocolName\Scripts"
$requirementSpecPath     = $workingDir + "\$protocolName\Scripts"
$srcDeployPathOnHost    = $workingDir + "\$protocolName\Deploy"
$srcTestSuitePathOnHost = $workingDir + "\$protocolName\Bin"
$global:testResultDir   = $testResultDir # This variable must be set as global because it will be used in other scripts.

$VMConfigFile = "$srcScriptPathOnHost\$protocolName.xml"

# The following parameters are special for MS-AZOD
Push-Location $srcScriptLibPathOnHost
$testCaseScript = .\Get-Parameter.ps1 -sourceFileName $VMConfigFile -attrName "TestCase"
$testCaseTimeout = .\Get-Parameter.ps1 -sourceFileName $VMConfigFile -attrName "TestCaseTimeout"

#----------------------------------------------------------------------------
# Get VM configuration 
#----------------------------------------------------------------------------
Write-Info.ps1 "Get VM configuration ..." -foregroundcolor Yellow
Set-StrictMode -v 2
[xml]$VMConfig = get-content $VMConfigFile

$global:userNameInVM    = .\Get-Parameter.ps1 -sourceFileName $VMConfigFile -attrName "userName"
$global:userPwdInVM     = .\Get-Parameter.ps1 -sourceFileName $VMConfigFile -attrName "password"
$global:domainInVM      = .\Get-Parameter.ps1 -sourceFileName $VMConfigFile -attrName "domain"
$global:testDirInVM     = .\Get-Parameter.ps1 -sourceFileName $VMConfigFile -attrName "testDirInVM"

# Get Local User name and Password
$LocalUserNameInVM          = $VMConfig.lab.core.username
$LocaluserPwdInVM           = $VMConfig.lab.core.password

# Get VM IPs
$DC01Setting = $VMConfig.lab.servers.vm | where {$_.name -match "DC01"}
$DC02Setting = $VMConfig.lab.servers.vm | where {$_.name -match "DC02"}
$AP01Setting = $VMConfig.lab.servers.vm | where {$_.name -match "AP01"}
$AP02Setting = $VMConfig.lab.servers.vm | where {$_.name -match "AP02"}
$CLIENT01Setting = $VMConfig.lab.servers.vm | where {$_.name -match "CLIENT01"}

$DC01IP = $DC01Setting.ip
$DC02IP = $DC02Setting.ip
$AP01IP = $AP01Setting.ip
$AP02IP = $AP02Setting.ip
$CLIENT01IP = $CLIENT01Setting.ip

# Create test result Dir if it is not existed
if(!(Test-Path $testResultDir))
{
	md $testResultDir
}

#----------------------------------------------------------------------------
# Get remote computer's system drive share when they are starting up here
#----------------------------------------------------------------------------
Write-Info.ps1 "Get remote computer's system drive share when they are starting up here" -foregroundcolor Yellow
$DC01SystemDrive = .\Get-RemoteSystemDrive.ps1 $DC01IP "$DC01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
$DC02SystemDrive = .\Get-RemoteSystemDrive.ps1 $DC02IP "$DC02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
$AP01SystemDrive = .\Get-RemoteSystemDrive.ps1 $AP01IP "$AP01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
$AP02SystemDrive = .\Get-RemoteSystemDrive.ps1 $AP02IP "$AP02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
$CLIENT01SystemDrive = .\Get-RemoteSystemDrive.ps1 $CLIENT01IP "$CLIENT01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"

$testDirInDC01VM = $testDirInVM.Replace("SYSTEMDRIVE", $DC01SystemDrive)
$testDirInDC02VM = $testDirInVM.Replace("SYSTEMDRIVE", $DC02SystemDrive)
$testDirInAP01VM = $testDirInVM.Replace("SYSTEMDRIVE", $AP01SystemDrive)
$testDirInAP02VM = $testDirInVM.Replace("SYSTEMDRIVE", $AP02SystemDrive)
$testDirInCLIENT01VM = $testDirInVM.Replace("SYSTEMDRIVE", $CLIENT01SystemDrive)

#----------------------------------------------------------------------------
# Ensure VMs have install tools and test suite
#----------------------------------------------------------------------------
Write-Info.ps1 "Ensure VMs have install tools and test suite" -foregroundcolor Yellow
.\WaitFor-ComputerReady.ps1 $DC01IP "$DC01IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $DC01SystemDrive MSIInstalled.signal 600
.\WaitFor-ComputerReady.ps1 $DC02IP "$DC02IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $DC02SystemDrive MSIInstalled.signal 600
.\WaitFor-ComputerReady.ps1 $AP01IP "$AP01IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $AP01SystemDrive MSIInstalled.signal 600
.\WaitFor-ComputerReady.ps1 $AP02IP "$AP02IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $AP02SystemDrive MSIInstalled.signal 600
.\WaitFor-ComputerReady.ps1 $CLIENT01IP "$CLIENT01Setting\$LocalUserNameInVM" "$LocaluserPwdInVM" $CLIENT01SystemDrive MSIInstalled.signal 600

#----------------------------------------------------------------------------
# Initial time stamp and start config test environment.
#----------------------------------------------------------------------------
Write-Info.ps1 "Initial time stamp and start config test ENV ..." -foregroundcolor Yellow

#----------------------------------------------------------------------------
# Copy test files.
#----------------------------------------------------------------------------
Write-Info.ps1 "Copy test files..." -foregroundcolor Yellow
Write-Info.ps1 "Copy test files from $DC01IP" -foregroundcolor Yellow
.\Copy-TestFile.ps1 $srcScriptLibPathOnHost $srcScriptPathOnHost $srcToolPathOnHost $srcTestSuitePathOnHost $DC01IP $testDirInDC01VM "$DC01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Copy test files from $DC02IP" -foregroundcolor Yellow
.\Copy-TestFile.ps1 $srcScriptLibPathOnHost $srcScriptPathOnHost $srcToolPathOnHost $srcTestSuitePathOnHost $DC02IP $testDirInDC02VM "$DC02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Copy test files from $AP01IP" -foregroundcolor Yellow
.\Copy-TestFile.ps1 $srcScriptLibPathOnHost $srcScriptPathOnHost $srcToolPathOnHost $srcTestSuitePathOnHost $AP01IP $testDirInAP01VM "$AP01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Copy test files from $AP02IP" -foregroundcolor Yellow
.\Copy-TestFile.ps1 $srcScriptLibPathOnHost $srcScriptPathOnHost $srcToolPathOnHost $srcTestSuitePathOnHost $AP02IP $testDirInAP02VM "$AP02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Copy test files from $CLIENT01IP" -foregroundcolor Yellow
.\Copy-TestFile.ps1 $srcScriptLibPathOnHost $srcScriptPathOnHost $srcToolPathOnHost $srcTestSuitePathOnHost $CLIENT01IP $testDirInCLIENT01VM "$CLIENT01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"

#----------------------------------------------------------------------------
# Kick off configurations on VM client(s)
#----------------------------------------------------------------------------
# Configure DC01
Write-Info.ps1 "Kick off configurations on DC01 ..." 
.\RemoteExecute-Command.ps1 $DC01IP "$testDirInDC01VM\Scripts\RestartAndRun.bat `"cmd /c powershell $testDirInDC01VM\Scripts\Start-MIPConfiguration.ps1 $testDirInDC01VM Config-DC01.ps1`"" "$DC01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Waiting for DC01 configuration done..." -foregroundcolor Yellow
.\WaitFor-ComputerReady.ps1 $DC01IP "$DC01IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $DC01SystemDrive "config.finished.signal" 2400

# Configure DC02
Write-Info.ps1 "Kick off configurations on DC02 ..." 
.\RemoteExecute-Command.ps1 $DC02IP "$testDirInDC02VM\Scripts\RestartAndRun.bat `"cmd /c powershell $testDirInDC02VM\Scripts\Start-MIPConfiguration.ps1 $testDirInDC02VM Config-DC02.ps1`"" "$DC02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Waiting for DC02 configuration done..." -foregroundcolor Yellow
.\WaitFor-ComputerReady.ps1 $DC02IP "$DC02IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $DC02SystemDrive "config.finished.signal" 2400

# Configure AP01
Write-Info.ps1 "Kick off configurations on AP01 ..." 
.\RemoteExecute-Command.ps1 $AP01IP "$testDirInAP01VM\Scripts\RestartAndRun.bat `"cmd /c powershell $testDirInAP01VM\Scripts\Start-MIPConfiguration.ps1 $testDirInAP01VM Config-AP01.ps1`"" "$AP01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Waiting for AP01 configuration done..." -foregroundcolor Yellow
.\WaitFor-ComputerReady.ps1 $AP01IP "$AP01IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $AP01SystemDrive "config.finished.signal" 1800

# Configure AP02
Write-Info.ps1 "Kick off configurations on AP02 ..." 
.\RemoteExecute-Command.ps1 $AP02IP "$testDirInAP02VM\Scripts\RestartAndRun.bat `"cmd /c powershell $testDirInAP02VM\Scripts\Start-MIPConfiguration.ps1 $testDirInAP02VM Config-AP02.ps1`"" "$AP02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Waiting for AP02 configuration done..." -foregroundcolor Yellow
.\WaitFor-ComputerReady.ps1 $AP02IP "$AP02IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $AP02SystemDrive "config.finished.signal" 1800

# Configure CLIENT01
Write-Info.ps1 "Kick off configurations on Client01 ..." 
.\RemoteExecute-Command.ps1 $CLIENT01IP "$testDirInCLIENT01VM\Scripts\RestartAndRun.bat `"cmd /c powershell $testDirInCLIENT01VM\Scripts\Start-MIPConfiguration.ps1 $testDirInCLIENT01VM Config-Client01.ps1`"" "$CLIENT01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
Write-Info.ps1 "Waiting for Client01 configuration done..." -foregroundcolor Yellow
.\WaitFor-ComputerReady.ps1 $CLIENT01IP "$CLIENT01IP\$LocalUserNameInVM" "$LocaluserPwdInVM" $CLIENT01SystemDrive "config.finished.signal" 2400

#----------------------------------------------------------------------------
# Run test cases on test driver
#----------------------------------------------------------------------------
Write-Info.ps1 "Run test cases on test driver ..." -foregroundcolor Yellow

# Create task for execute protocol test suite
$taskName = "ExecuteProtocolTestSuite"
if($ContextName -eq $null -or $ContextName.trim() -eq "")
{
    $task = "PowerShell $testDirInCLIENT01VM\Scripts\$testCaseScript"
}
else
{
    $task = "PowerShell $testDirInCLIENT01VM\Scripts\$testCaseScript $ContextName"
}
$createTask = "CMD /C schtasks /Create /RU Administrators /SC ONCE /ST 00:00 /TN $TaskName /TR `"$Task`" /IT /F"
$exeTask = "cmd /c schtasks /Run /TN $TaskName"

# Execute the task to execute protocol test suite
for($i=0;$i -lt 5;$i++) 
{
    Write-Info.ps1 "Create Schedule Task with command: $createTask "
    .\RemoteExecute-Command.ps1 $CLIENT01IP "$createTask" "$DomainInVM\$userNameInVM" "$userPwdInVM"
    Start-sleep 20 # wait for task being created
    Write-Info.ps1 "Execute Scheduled Task to kick off test case run with command: $exeTask "
    .\RemoteExecute-Command.ps1 $CLIENT01IP "$exeTask" "$DomainInVM\$userNameInVM" "$userPwdInVM"
    Start-sleep 20 # wait for task being created
    .\WaitFor-ComputerReady.ps1 $CLIENT01IP "$DomainInVM\$userNameInVM" "$userPwdInVM" $testDirInVM "test.started.signal" 120
    if($lastexitcode -eq 0)
    {
        break
    }
}

Start-sleep 120 #wait for test to start
.\WaitFor-ComputerReady.ps1 $CLIENT01IP "$DomainInVM\$userNameInVM" "$userPwdInVM" $testDirInCLIENT01VM "test.finished.signal" $testCaseTimeout

#----------------------------------------------------------------------------
# Copy results from VM 
#----------------------------------------------------------------------------
Write-Info.ps1 "Copy results from VM ..." -foregroundcolor Yellow
.\Copy-TestResult.ps1 $CLIENT01IP "$testDirInCLIENT01VM\TestResults" "$testResultDir" "$DomainInVM\$userNameInVM" "$userPwdInVM"
.\Copy-TestResult.ps1 $CLIENT01IP "$testDirInCLIENT01VM\TestLog" "$testResultDir\Client01" "$DomainInVM\$userNameInVM" "$userPwdInVM"

.\Copy-TestResult.ps1 $DC01IP "$testDirInDC01VM\TestLog" "$testResultDir\DC01" "$DC01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
.\Copy-TestResult.ps1 $DC02IP "$testDirInDC02VM\TestLog" "$testResultDir\DC02" "$DC02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
.\Copy-TestResult.ps1 $AP01IP "$testDirInAP01VM\TestLog" "$testResultDir\AP01" "$AP01IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
.\Copy-TestResult.ps1 $AP02IP "$testDirInAP02VM\TestLog" "$testResultDir\AP02" "$AP02IP\$LocalUserNameInVM" "$LocaluserPwdInVM"
# Clean up signals after copy out from VM
.\RemoteExecute-Command.ps1 $CLIENT01IP "$testDirInCLIENT01VM\Scripts\CleanupSignals.cmd" "$DomainInVM\$userNameInVM" "$userPwdInVM"

Pop-Location

#----------------------------------------------------------------------------
# Stop logging and exit
#----------------------------------------------------------------------------
Stop-Transcript
# Write Call Stack
if($function:ExitCallStack -ne $null)
{
	ExitCallStack "Execute-ProtocolTest.ps1"
}
exit 0
