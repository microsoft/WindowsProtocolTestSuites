# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to change screen resolution on remote machine
# Return Value: 0 indicates task is started successfully; -1 indicates failed to run the specified task


# Run Task to change remote screen orientation
$pwdConverted = ConvertTo-SecureString $ptfpropSUTUserPassword -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential $ptfpropSUTUserName, $pwdConverted -ErrorAction Stop
$buildfolder = Get-ChildItem "C:\MicrosoftProtocolTests\RDP\Client-Endpoint"
$buildNo = $buildfolder[0].Name
$path = "C:\MicrosoftProtocolTests\RDP\Client-Endpoint\" + $buildNo + "\Scripts"
$scriptblock = {
	param([int]$width, [int]$height, [int]$orientation, [string]$path)
    $taskname = "ChangeResolution" + $width + "X" + $height
	$script = $path + "\ChangeResolution.ps1"
	cmd /c schtasks /Create /SC Weekly /TN $taskname /TR "powershell $script $width $height" /IT /F
	cmd /c schtasks /Run /TN $taskname
	cmd /c schtasks /Delete /TN $taskname /F
	$taskname = "ChangeOrientation" + $orientation
	$script = $path + "\ChangeOrientation.ps1"
	cmd /c schtasks /Create /SC Weekly /TN $taskname /TR "powershell $script $orientation" /IT /F
	cmd /c schtasks /Run /TN $taskname
	cmd /c schtasks /Delete /TN $taskname /F
	}
	
$cmdOutput = Invoke-Command -ComputerName $ptfpropSUTName -credential $cred -ScriptBlock $scriptblock -ArgumentList ($width, $height, $orientation, $path)

$cmdOutput | out-file ".\InitialDisplaySetting.log"

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
