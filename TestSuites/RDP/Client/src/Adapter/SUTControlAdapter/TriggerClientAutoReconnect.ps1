# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This script is used to trigger RDP client to start an Auto-Reconnect sequence after a network interruption.

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfProp_SUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to initial an Auto-reconnecton sequence
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_TriggerClientAutoReconnect_Task $PtfProp_SUTUserName $PtfProp_SUTUserPassword
return $returnValue
