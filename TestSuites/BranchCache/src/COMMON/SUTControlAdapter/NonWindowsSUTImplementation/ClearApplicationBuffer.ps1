######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Clear the internet browser cache on the computer that is designated from parameter 
#           $computerName's value. 
# Step 1:   Kill internet browser process.
# Step 2:   Delete all files and settings of the browsing history.
#-------------------------------------------------------------------------------------

param(
[string]$remoteComputer = $computerName,
[string]$usr = $user,
[string]$pwd = $password
)

cmd /c exit