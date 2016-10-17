######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Execute a command on a remote computer which is designated from the parameter 
#           $computerName's value. The command is specified by $cmdLine's value. 
# Step 1:   Connect to the remote computer.
# Step 2:   Remote execute command.
#-------------------------------------------------------------------------------------

param(
[string]$computerName, 
[string]$cmdLine, 
[string]$usr, 
[string]$pwd
)

cmd /c exit