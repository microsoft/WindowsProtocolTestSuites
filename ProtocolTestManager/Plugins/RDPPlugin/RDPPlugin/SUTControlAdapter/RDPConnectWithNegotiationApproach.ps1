##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
##################################################################################
# This script is used to trigger client to initiate a RDP connection from RDP client, 
# and client should use Negotiation-Based Approach to advertise the support for TLS, 
# CredSSP or RDP standard security protocol.

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfPropSUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to start RDP connection remotely
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfPropSUTName $PtfPropRDPConnectWithNegotiationApproach_Task $PtfPropSUTUserName $PtfPropSUTUserPassword
return $returnValue
