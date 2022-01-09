# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to trigger client to initiate a RDP connection from RDP client, 
# and the client should use Direct Approach with TLS as the security protocol.

# Run task to start RDP connection (Direct Tls) remotely
$returnValue = . "../SUTControlAdapter/Run-TaskWithPSRemoting.ps1" $PtfProp_SUTName $PtfProp_MaximizeRDPClientWindow_Task $PtfProp_SUTUserName
return $returnValue

