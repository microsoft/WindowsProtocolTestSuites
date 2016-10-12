######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Restart a service on a remote computer which is designated from parameter 
#           $remoteComputerName's value. The service is specified by $serviceName's value. 
# Step 1:   Restart the service.
#-------------------------------------------------------------------------------------

param(
[string]$ServerComputerName = $remoteComputerName,
[string]$service = $serviceName,
[string]$usr = $user,
[string]$pwd = $password
)

cmd /c exit