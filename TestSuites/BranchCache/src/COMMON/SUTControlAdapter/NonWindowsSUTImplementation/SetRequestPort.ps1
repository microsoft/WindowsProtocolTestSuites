######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Set request port about pccrr on a remote computer which is designated 
#           from parameter $computerName's value. The port is specified by parameter 
#           $port's value.
# Step 1:   Set request port about pccrr through updating the registry remotely from 
#           a 32 bit application.
#-------------------------------------------------------------------------------------

param(
[string]$value = $port,
[string]$ServerComputerName = $computerName,
[string]$usr = $user,
[string]$pwd = $password
)

cmd /c exit