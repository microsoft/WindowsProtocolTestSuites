## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
[string]$protocolName          = "MS-AZOD"
)

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
Stop-Transcript -ErrorAction Continue | Out-Null
$logPath = Join-Path -Path $env:SystemDrive -ChildPath "Test\TestLog"
if ((Test-Path -Path $logPath) -eq $false)
{
    md $logPath
}
Start-Transcript -Path "$logPath\MS-AZOD-RunAllTestCases.ps1.log" -Append -Force

#-------------------------
# Execute Test Suite
#-------------------------
$endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\MS-AZOD\OD-Endpoint"
$version = Get-ChildItem $endPointPath | where {$_.Attributes -eq "Directory" -and $_.Name -match "\d+\.\d+\.\d+\.\d+"} | Sort-Object Name -descending | Select-Object -first 1

$binDir = "$endPointPath\$version\bin"
$mstest = ($env:VS110COMNTOOLS + "mstest.exe").Replace("Tools","IDE")
$testDir = "$env:SystemDrive\Test"
if(!(Test-Path $testDir))
{
	md $testDir
}

$testResultDir = $testDir + "\TestResults"
if(Test-Path $testResultDir)
{
	remove-item -path $testResultDir -Filter "*.*" -Recurse
}
md $testResultDir


Push-Location $testDir\Scripts


$finishSignalFile = "$testDir\test.finished.signal"
if(Test-Path $finishSignalFile)
{
	Remove-Item $finishSignalFile
}

$startSignalFile = "$testDir\test.started.signal"
echo "test.started.signal" > $startSignalFile

# Load xml files
$ptfconfig = "$binDir\MS-AZOD_ODTestSuite.deployment.ptfconfig"
CMD /C attrib -R $ptfconfig

#Get OS Version
[int]$major = [Environment]::OSVersion.Version.Major
[int]$minor = [Environment]::OSVersion.Version.Minor

$OSVersionNumber = [double]("$major" + "." + "$minor")

#Modify pftconfig file
if($OSVersionNumber -lt 6.3)
{
    .\Modify-ConfigFileNode.ps1 $ptfconfig "MaxSMB2DialectSupported" "768"
}
elseif($OSVersionNumber -eq 6.3)
{
    .\Modify-ConfigFileNode.ps1 $ptfconfig "MaxSMB2DialectSupported" "770"
}
elseif($OSVersionNumber -gt 6.3)
{
    .\Modify-ConfigFileNode.ps1 $ptfconfig "MaxSMB2DialectSupported" "785"
}

# Start to run test cases
& $mstest /testcontainer:"$binDir\MS-AZOD_ODTestSuite.dll" /category:"BVT" /runconfig:"$binDir\ODLocalTestRun.testrunconfig" /resultsfile:"$testResultDir\MS_AZOD_TestResults.trx" 

echo "test.finished.signal" > $finishSignalFile

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")