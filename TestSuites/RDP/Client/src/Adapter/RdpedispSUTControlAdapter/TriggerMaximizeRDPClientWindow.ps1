﻿# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to trigger client to initiate a RDP connection from RDP client, 
# and the client should use Direct Approach with TLS as the security protocol.

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfPropSUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to start RDP connection (Direct Tls) remotely
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfPropSUTName $PtfPropMaximizeRDPClientWindow_Task $PtfPropSUTUserName $PtfPropSUTUserPassword
return $returnValue

