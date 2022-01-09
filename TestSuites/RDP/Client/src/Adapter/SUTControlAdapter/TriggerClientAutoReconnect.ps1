# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This script is used to trigger RDP client to start an Auto-Reconnect sequence after a network interruption.

# Run task to initial an Auto-reconnecton sequence
$returnValue = ./Run-TaskWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_TriggerClientAutoReconnect_Task $PtfProp_SUTUserName
return $returnValue
