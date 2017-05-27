#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Step 1: This script's first step is to Check the ADAM service is available, 
#         if the parameter $machineObj is null, means the service is not available;
#         then execute scripts "ChangeNonDCMachineAccountStatus.ps1" failed.
# Step 2: Configure the Account is enable or disable, if the parameter $isEnable
#         is true, then change the ADAM's value "AccountDisabled"'s status to 0,
#         otherwise,change it to 1.
#-------------------------------------------------------------------------------------

cmd /c exit