######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Get the status whether the hash computation is completed on the remote
#           computer which is designated from the parameter $sutName's value. 
# Step 1:   Invoking the script whose path is designated from parameter 
#           $sutScriptPath's value on remote computer $sutName's value through
#           invoking RemoteExecuteCommand script.
# Step 2:   Get the content from the specified file 
#           \\$ServerComputerName\MS-PCCRTP\IsFinished.txt
#-------------------------------------------------------------------------------------

param(
[string]$ServerComputerName = $sutName,
[string]$usr = $userName,
[string]$pwd = $password,
[string]$scriptPath = $sutScriptPath
)

cmd /c exit