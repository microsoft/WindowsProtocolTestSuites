# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to trigger client to initiate a RDP connection from RDP client, 
# and the client should use Direct Approach with CredSSP as the security protocol.

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfProp_SUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to start RDP connection (Direct CSSP) remotely
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_RDPConnectWithDirectCredSSP_Task $PtfProp_SUTUserName $PtfProp_SUTUserPassword
return $returnValue

