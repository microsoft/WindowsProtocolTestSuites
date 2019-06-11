# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to trigger RDP client to send the following input events to server:
# 1) Keyboard Event or Unicode Keyboard Event
# 2) Mouse Event or Extended Mouse Event
# 3) Synchronize Event
# 4) Client Refresh Rect
# 5) Client Suppress Output (including display off and on)

# On Windows Operationg System, these events can be triggered by following operations:
# Step 1: keystrokes
# Step 2: Mouse clicking or moving
# Step 3: trun on/off the keyboard toggle keys, suc as NUM Lock.
# Step 4: Maximize and minimize the remote desktop

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfProp_SUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to trigger different input events
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_TriggerInputEvents_Task $PtfProp_SUTUserName $PtfProp_SUTUserPassword
sleep 20 # take couple of seconds to generate input events
return $returnValue
