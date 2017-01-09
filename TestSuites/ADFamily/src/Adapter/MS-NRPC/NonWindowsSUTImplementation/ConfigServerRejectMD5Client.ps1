#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#----------------------------------------------------------------------------------
# Script function description:
# Step 1: This script's first step is to change the status of the value 
#         "RefusePasswordChange".If the $rejectMD5Clients's status is false,
#         Then change the value "RejectMD5Clients" to 1,
#         else change the value "RejectMD5Clients" to 0.
# Step 2: Restart the NetLogon service
#----------------------------------------------------------------------------------

cmd /c exit