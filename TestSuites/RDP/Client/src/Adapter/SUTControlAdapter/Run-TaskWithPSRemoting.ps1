# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to run a scheduled job on remote machine
# Return Value: 0 indicates task is started successfully; -1 indicates failed to run the specified task

Param(
[string]$computer, # host name or ip address
[string]$taskName,
[string]$userName
)

# Run Task to start RDP connection remotely
$cmdOutput = ""
$retryCount = 10

$userPwdInTCEn = ConvertTo-SecureString $ptfprop_SUTUserPassword -AsPlainText -Force
$Credential = New-Object System.Management.Automation.PSCredential($ptfprop_SUTUserName,$userPwdInTCEn)

$sessionM = $null;

try
{
	$sessionM = New-PSSession -ComputerName $ptfprop_SUTName -Credential $Credential
}
catch
{
}

try
{
    while($cmdOutput -eq "" -and ($retryCount -ne 0)) {

        if ($sessionM -eq $null) 
        {
            $cmdOutput = Invoke-Command -HostName $ptfprop_SUTName -UserName $ptfprop_SUTUserName -ScriptBlock {param([string]$taskName) cmd /c schtasks /run /TN $taskName} -ArgumentList $taskName
        }
        else
        {
            $cmdOutput = Invoke-Command -Session $sessionM -ScriptBlock {param([string]$taskName) cmd /c schtasks /run /TN $taskName} -ArgumentList $taskName
        }
        
        $retryCount--
    }
}
catch
{
    if ($sessionM -ne $null) {
        Remove-PSSession $sessionM
    }
    return -1 # connect failed
}
$cmdOutput | out-file "./RunTask_$taskName.log"

if($cmdOutput -ne $null)
{
    if($cmdOutput.GetType().IsArray)
    {
        $cmdOutput = $cmdOutput[0]
    }
    if($cmdOutput.ToUpper().Contains("ERROR"))
    {
        if ($sessionM -ne $null) {
            Remove-PSSession $sessionM
        }
        
        return -1 # operation failed
    }
}

if ($sessionM -ne $null) {
    Remove-PSSession $sessionM
}

return 0  #operation succeed
