######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Start a service on a remote computer which is designated from parameter 
#           $remoteComputerName's value. The service is specified by $serviceName's value. 
# Step 1:   Start the remote computer's specified service.
# Step 2:   Using GC to garbage collection.
#-------------------------------------------------------------------------------------

param(
[string]$computerName = $remoteComputerName,
[string]$service = $serviceName,
[string]$usr = $user,
[string]$pwd = $password
)

cmd /c exit