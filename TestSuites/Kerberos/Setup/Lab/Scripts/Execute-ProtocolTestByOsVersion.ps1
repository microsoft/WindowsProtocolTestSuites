###############################################################
## Copyright (c) Microsoft Corporation. All rights reserved. ##
###############################################################


# Find specified file or directory from the MicrosoftProtocolTests folder on 
# the computer. The folder will be created after the test suite MSI is installed.
function GetItemInTestSuite($Name)
{
    

    # Try if the name specified is a directory
    [string]$Path = [System.IO.Directory]::GetDirectories("$env:HOMEDRIVE\MicrosoftProtocolTests",`
                   $Name,[System.IO.SearchOption]::AllDirectories)
 
    if(($Path -eq $null) -or ($Path -eq ""))
    {
        # Try if the name specified is a file
        [string]$Path = [System.IO.Directory]::GetFiles("$env:HOMEDRIVE\MicrosoftProtocolTests",`
                        $Name,[System.IO.SearchOption]::AllDirectories)
    }

    return $Path
}

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$WorkingPath = "c:\temp"
$env:Path += ";c:\temp;c:\temp\Scripts"

#-----------------------------------------------------------------------------------------------
# Create $logFile if not exist
#-----------------------------------------------------------------------------------------------
$logFile = ".\" + $MyInvocation.MyCommand.Name + ".log"
if(!(Test-Path -Path $logFile))
{
	New-Item -Type File -Path $logFile -Force
}
Start-Transcript $logFile -Append

# Get parameters
$VMName = "Kerberos-OSS-AP02"
$ParamArray = @{} 
Write-Info.ps1 "Trying to get parameters from config file..." -ForegroundColor Yellow
GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
# Try to access AP02 to make sure the trust is solid between DC01 and DC02.
$Ap02Name = $ParamArray["name"]
$Ap02Domain = $ParamArray["domain"]

[String]$ShareOnAp02 = [String]"\\$Ap02Name.$Ap02Domain\Share"
$RetryTimes = 90

while($RetryTimes -ge 0)
{
    $IfExist = Test-path $ShareOnAp02
    if ($IfExist -eq $true)
    {
        Write-Info.ps1 "Can connect to $ShareOnAp02"
        break;
    }
    else
    {
        Write-Info.ps1 "Cannot connect to $ShareOnAp02, retry it later"
        $RetryTimes--;
        sleep 10
    }
}

if ($RetryTimes -le 0)
{
    Write-Info.ps1 "Cannot connect to $ShareOnAp02 in 15 minutes, quit current script"
    exit 1
}

$SutOsVersion = Invoke-Command -ComputerName "dc01" -ScriptBlock {"" + [System.Environment]::OSVersion.Version.Major + "." + [System.Environment]::OSVersion.Version.Minor}

$BatchFolderOnVM = GetItemInTestSuite "Batch"
$0S2012R2 = "6.3"

pushd $BatchFolderOnVM   
if([double]$SutOSVersion -ge [double]$0S2012R2)
{
    Write-Info.ps1 "SUT Os version is $SutOSVersion, larger or equal to 2012R2"
    Write-Info.ps1 "Start to run all test cases..."
    Stop-Transcript
    $BatchToRunAllCase = "Domain_RunAllTestCases.cmd" 
    cmd /c $BatchFolderOnVM\$BatchToRunAllCase
}
else
{
    Write-Info.ps1 "SUT Os version is $SutOSVersion, lower than 2012R2"
    Write-Info.ps1 "Start to run test cases except 2K12R2 cases..."
    Stop-Transcript
    cmd /c "$env:VS110COMNTOOLS..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "..\Bin\Kerberos_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx /TestCaseFilter:"TestCategory!=DFL2K12R2"
}

