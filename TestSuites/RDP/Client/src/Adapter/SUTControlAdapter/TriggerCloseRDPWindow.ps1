# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This script is used to trigger RDP client initiate a disconnection of current session.

# Run task to simulate a client initiated disconnect request
# Trigger the client to send a Client_Shutdown_Request_Pdu PDU by $PtfProp_TriggerCloseRDPWindow_Task.
# Note: we don't use TriggerClientDisconnectAll.ps1 here, because TriggerClientDisconnectAll.ps1 will force terminate all RDP client processes for cleanup and won't trigger the required User-Initiated on Client disconnection sequence.
$returnValue = ./Run-TaskWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_TriggerCloseRDPWindow_Task $PtfProp_SUTUserName
return $returnValue
