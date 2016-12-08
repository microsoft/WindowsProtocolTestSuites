#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
#############################################################

Param
(
[string]$endpoint = "SUT",
[string]$testDirInServerVM
)

#--------------------------------------------------------------------------------------------------------------------------------
#  Function: Write  MSI full path to the finished signal file($env:HOMEDRIVE\MSIInstalled.signal)
#--------------------------------------------------------------------------------------------------------------------------------
Function WriteFinishSignal
{
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Host
    Write-Host  "Write signal file to system drive."
    $MSIScriptsFile = [System.IO.Directory]::GetFiles("$env:HOMEDRIVE\MicrosoftProtocolTests", "Config-Driver.ps1", [System.IO.SearchOption]::AllDirectories)
    [String]$MSIFullPath = [System.IO.Directory]::GetParent($MSIScriptsFile)
    cmd /c ECHO $MSIFullPath >$env:HOMEDRIVE\MSIInstalled.signal
}

cmd /c ECHO "install.started.signal" > $env:HOMEDRIVE\install.started.signal

if(Test-Path -Path $env:HOMEDRIVE\install.finished.signal)
{
cmd /c DEL -f -q $env:HOMEDRIVE\install.finished.signal
}

if(Test-Path -Path $testDirInServerVM\Deploy\ProtocolTestFramework.msi)
{
cmd /c msiexec -i $testDirInServerVM\Deploy\ProtocolTestFramework.msi -q
}

if(Test-Path -Path $testDirInServerVM\Deploy\Kerberos-TestSuite-ServerEP.msi)
{
cmd /c msiexec -i $testDirInServerVM\Deploy\Kerberos-TestSuite-ServerEP.msi -q TARGET_ENDPOINT=$endpoint
}

WriteFinishSignal
