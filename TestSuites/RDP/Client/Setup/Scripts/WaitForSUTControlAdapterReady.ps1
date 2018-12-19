# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
[string]$computer, # host name or ip address
[string]$userName,
[string]$userPassword,
[string]$batchPath
)

# Run Task to start RDP connection remotely
$pwdConverted = ConvertTo-SecureString $userPassword -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential $userName, $pwdConverted -ErrorAction Stop
$tryTime = 0
while($tryTime -lt 10){
	Invoke-Command -ComputerName $computer -credential $cred -ScriptBlock {cmd /c schtasks /run /TN Negotiate_RDPConnect}
	sleep 5
	$process = Invoke-Command -ComputerName $computer -credential $cred -ScriptBlock {Get-Process mstsc}
	Invoke-Command -ComputerName $computer -credential $cred -ScriptBlock {cmd /c schtasks /run /TN DisconnectAll}
	if($process -ne $null -and $process.Name -eq "mstsc"){
		Write-Host "$sutComputerIP task scheduler ready." -foregroundcolor Green
        cmd /C ECHO MSTSC FINISHED > $env:HOMEDRIVE\mstsc.finished.signal
		break
	}
	$tryTime++
    Write-Host "$sutComputerIP task scheduler not ready. Try again." -foregroundcolor Red
	sleep 300
}
if($tryTime -ge 10){
	Write-Host "$sutComputerIP task scheduler Failed." -foregroundcolor Red
    cmd /C ECHO MSTSC ERROR > $env:HOMEDRIVE\mstsc.error.signal
}

sleep 30

# Run a case repeatedly to ensure SUT control adapter ready
cd $batchPath

$maxRetryTimes = 20
$tryTime = 0
while($tryTime -lt $maxRetryTimes){
    cmd /c CommonRunSingleCase.cmd BVT_ConnectionTest_ConnectionInitiation_PositiveTest
    sleep 15
    $filenames = Get-ChildItem .\TestResults
    foreach($filename in $filenames){
        if($filename.Name.Contains(".trx")){
            $trxFile = "$batchPath\TestResults\$filename"
            break
        }
    }
    $result = Select-String '<ResultSummary outcome="Completed">' $trxFile
    Remove-Item .\TestResults -Recurse -Force
    if($result.count -gt 0){
        Write-Host "$sutComputerIP task scheduler ready." -foregroundcolor Green
        cmd /C ECHO SUTADAPTER FINISHED > $env:HOMEDRIVE\SUTAdapter.finished.signal
        break
    }
    $tryTime++
    Write-Host "$sutComputerIP task scheduler not ready. Try again." -foregroundcolor Red
    sleep 120
}

if($tryTime -ge $maxRetryTimes){
    Write-Host "$sutComputerIP task scheduler Failed." -foregroundcolor Red
    cmd /C ECHO SUTADAPTER ERROR > $env:HOMEDRIVE\SUTAdapter.error.signal
}






