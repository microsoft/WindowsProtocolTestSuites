# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
[string]$computer, # host name or ip address
[string]$userName,
[string]$userPassword,
[string]$batchPath
)

# Run a case repeatedly to ensure SUT control adapter ready
cd $batchPath

$maxRetryTimes = 20
$tryTime = 0
while($tryTime -lt $maxRetryTimes){
    ./RunTestCasesByFilter.ps1 S1_ConnectionTest_BasicSettingExchange_PositiveTest_EncryptionMethodandLevel
    start-sleep 15
    $filenames = Get-ChildItem .\..\TestResults
    foreach($filename in $filenames){
        if($filename.Name.Contains(".trx")){
            $trxFile = "$batchPath\..\TestResults\$filename"
            break
        }
    }
    $result = Select-String '<ResultSummary outcome="Completed">' $trxFile
    Remove-Item .\..\TestResults -Recurse -Force
    if($result.count -gt 0){
        Write-Host "$sutComputerIP task scheduler ready." -foregroundcolor Green
        cmd /C ECHO SUTADAPTER FINISHED > $env:HOMEDRIVE\SUTAdapter.finished.signal
        break
    }
    $tryTime++
    Write-Host "$sutComputerIP task scheduler not ready. Try again." -foregroundcolor Red
    $pwdConverted = ConvertTo-SecureString $userPassword -AsPlainText -Force
    $cred = New-Object System.Management.Automation.PSCredential $userName, $pwdConverted -ErrorAction Stop
    Invoke-Command -ComputerName $computer -credential $cred -ScriptBlock {cmd /c schtasks /run /TN CSharpAgent}
    start-sleep 120
}

if($tryTime -ge $maxRetryTimes){
    Write-Host "$sutComputerIP task scheduler Failed." -foregroundcolor Red
    cmd /C ECHO SUTADAPTER ERROR > $env:HOMEDRIVE\SUTAdapter.error.signal
}