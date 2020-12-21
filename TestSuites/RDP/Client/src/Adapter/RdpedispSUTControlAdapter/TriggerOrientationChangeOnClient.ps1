# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to change screen orientaion on remote machine
# Return Value: 0 indicates task is started successfully; -1 indicates failed to run the specified task

# Run Task to change remote screen orientation
$path = "/RDP-TestSuite-ClientEP/Scripts"
$scriptblock = {
	param([int]$orientation, [string]$path)
    $taskname = "ChangeOrientation" + $orientation
    $systemDrive = $env:SystemDrive
	$script =  $systemDrive + $path + "/ChangeOrientation.ps1"
	cmd /c schtasks /Create /SC Weekly /TN $taskname /TR "powershell $script $orientation" /IT /F
	cmd /c schtasks /Run /TN $taskname
	cmd /c schtasks /Delete /TN $taskname /F
	}
	
$cmdOutput = Invoke-Command -HostName $ptfprop_SUTName -UserName $ptfprop_SUTUserName -ScriptBlock $scriptblock -ArgumentList ($orientation, $path)

$cmdOutput | out-file "./ChangeOrientation.log"

if($cmdOutput -ne $null)
{
    if($cmdOutput.GetType().IsArray)
    {
        $cmdOutput = $cmdOutput[0]
    }
    if($cmdOutput.ToUpper().Contains("ERROR"))
    {
        return -1 # operation failed
    }
}

return 0  #operation succeed
