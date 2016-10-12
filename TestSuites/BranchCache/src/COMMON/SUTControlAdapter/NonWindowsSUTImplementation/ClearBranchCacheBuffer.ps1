######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: This script's function is to clear the branch cache buffer on the remote
#           server that is designated from parameter $computerName's value.
# Step 1:   stop the branch cache service on the remote server.
# Step 2:   Clear the branch cache's local cache on the remote server.
# Step 3:   Start the branch cache service on the remote server.
#-------------------------------------------------------------------------------------

param(
[string]$ServerComputerName = $computerName,
[string]$usr = $user,
[string]$pwd = $password
)

cmd /c exit