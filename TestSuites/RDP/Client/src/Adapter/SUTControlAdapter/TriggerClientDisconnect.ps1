# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This script is used to trigger RDP client initiate a disconnection of current session.

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfProp_SUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to simulate a client initiated disconnect request
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_TriggerClientDisconnectAll_Task $PtfProp_SUTUserName $PtfProp_SUTUserPassword
return $returnValue
