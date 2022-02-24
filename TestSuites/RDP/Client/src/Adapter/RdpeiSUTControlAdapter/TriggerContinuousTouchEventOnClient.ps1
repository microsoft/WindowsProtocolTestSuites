# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to trigger one touch event on client.

$scriptblock = {
    param([string]$PtfProp_SUTName, [string]$PtfProp_SUTUserName, [string]$PtfProp_SUTUserPassword, [string]$PtfProp_TriggerContinuousTouchEvent_Task)
    cmd /c schtasks /run /s $PtfProp_SUTName /U $PtfProp_SUTUserName /P $PtfProp_SUTUserPassword /TN $PtfProp_TriggerContinuousTouchEvent_Task
}
	
$cmdError = Invoke-Command -HostName $PtfProp_SUTName -UserName $PtfProp_SUTUserName -ScriptBlock $scriptblock -ArgumentList ($PtfProp_SUTName, $PtfProp_SUTUserName, $PtfProp_SUTUserPassword, $PtfProp_TriggerContinuousTouchEvent_Task) 2>&1 | Out-File "./TriggerContinuousTouchEventOnClient.txt"

$cmdError = Get-Content "./TriggerContinuousTouchEventOnClient.txt"
if ($cmdError -ne $null) {
    if ($cmdError.GetType().IsArray) {
        $cmdError = $cmdError[0]
    }
    if ($cmdError.ToUpper().Contains("ERROR")) {
        return -1 # operation failed
    }
}

Start-Sleep 1
return 0  #operation succeed
