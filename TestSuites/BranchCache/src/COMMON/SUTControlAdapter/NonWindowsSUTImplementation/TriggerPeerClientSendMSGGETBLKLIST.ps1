######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Trigger peer client send gGETBLKLIST message.
#           The peer client is designated from parameter $SUTName's value. 
# Step 1:   Clear the cache including browser cache and branch cache through
#           invoking RemoteExecuteCommand script on the peer client.
# Step 2:   Create a download job batch file invoking RemoteExecuteCommand to 
#           write into the command about transferring the file which is specified by parameter 
#           $path's value on the computer which is designated from parameter 
#           $ContentComputerName's value to the appointed file on peer client.
# Step 3:   Create a schedule task "Welcome" to execute the created bache file created 
#           in Step 2 on peer client.
# Step 4:   Run the schedule task "Welcome" created in Step 3 to execute the created  
#           bache file on peer client.
#-------------------------------------------------------------------------------------

param(
[string]$ServerComputerName = $SUTName,
[string]$ContentServerName = $ContentComputerName,
[string]$path = $path,
[string]$usr = $usrInVM,
[string]$pwd = $pwdInVM,
[string]$time = $time
)

cmd /c exit