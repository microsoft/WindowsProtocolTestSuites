######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: End Task and restart branch cache service on the computer
#           that is designated from parameter $SUTName's value. 
# Step 1:   End schedule task "Welcome".
# Step 2:   Kill the branch cache service process designated from parameter 
#           $ProcessName's value.
# Step 3:   Restart the branch cache service designeted from parameter $serviceName's value.
# Step 4:   Clean cache including branch cache's local cache and download or upload cache.
#-------------------------------------------------------------------------------------

param(
[string]$ServerComputerName = $SUTName,
[string]$serviceName = $Service,
[string]$ProcessName = $processName,
[string]$usr = $usrInVM,
[string]$pwd = $pwdInVM,
[string]$path = $path
)

cmd /c exit