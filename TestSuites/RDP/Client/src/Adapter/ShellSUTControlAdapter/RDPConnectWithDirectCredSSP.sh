# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to trigger client to initiate a RDP connection from RDP client,
# and the client should use Direct Approach with CredSSP as the security protocol.

# The following example code invokes xfreerdp on the SUT machine through SSH.
# Note that the commands here MUST NOT block the script execution.
# So there is a & in the end of the command to run the xfreerdp as a background process
#
# cmd="DISPLAY=:10.0 nohup xfreerdp /t:RDPClient /rfx /u:$PTFProp_RDP_ServerUserName /p:'$PTFProp_RDP_ServerUserPassword' /v:$PTFProp_RDP_ServerDomain:$PTFProp_RDP_ServerPort > /dev/null 2>&1 &"
# ssh $PTFProp_SUTUserName@$PTFProp_SUTName $cmd

# Print an integer at the end of script.
# This number will be parsed as the return value of function
# RDPConnectWithDirectCredSSP(string caseName)
# Using a positive value to indicate success
echo 1
