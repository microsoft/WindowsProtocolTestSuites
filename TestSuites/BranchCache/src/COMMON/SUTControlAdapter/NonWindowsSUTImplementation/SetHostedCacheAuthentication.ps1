######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Set hosted cache client authentication mechanism 
#           on a remote computer which is designated from parameter $sutComputer's value. 
#           The client authentication is enable or not is specified by the parameter 
#           $isAuthentication's value. 
# Step 1:   Execute the client authentication set command line on the remote computer 
#           through invoking RemoteExecuteCommand script.
#-------------------------------------------------------------------------------------

param(
[string]$isAuthentication = $isAuthentication,
[string]$ComputerName = $sutComputer,
[string]$usr = $userName,
[string]$pwd = $password)

cmd /c exit