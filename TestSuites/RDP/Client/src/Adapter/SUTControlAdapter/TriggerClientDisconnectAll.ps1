# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This method is used to trigger RDP client to close all RDP connection to server for clean up.

cmd /c net use \\$PtfProp_SUTName\$PtfProp_SUTSystemDrive$ $PtfProp_SUTUserPassword /user:$PtfProp_SUTUserName
cmd /c del \\$PtfProp_SUTName\$PtfProp_SUTSystemDrive$\DisconnectAll.signal /F

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfProp_SUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to simulate a client initiated disconnect request
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_TriggerClientDisconnectAll_Task $PtfProp_SUTUserName $PtfProp_SUTUserPassword
if($returnValue -lt 0) {return $returnValue}

$count = 0
while (!(test-path \\$PtfProp_SUTName\$PtfProp_SUTSystemDrive$\DisconnectAll.signal) -and $count -lt 20)
{
    $count++
    sleep 1
}

cmd /c net use \\$PtfProp_SUTName\$PtfProp_SUTSystemDrive$ /delete /y

return 0  #operation succeed