#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#--------------------------------------------------------------------------------------
# Script function description:
# Step 1: This script's function is change the service's state
#         that designated from parameter $computerName's value.First is make sure the 
#         $computerName's value,it's determine by parameter $sutType,
#         If $sutType is equal to "PrimaryDc" the $computerName is equal to it's value,
#         else if $sutType is equal to "TrustDc" the $computerName is equal to it's 
#         value.It must be the computer's name that have NetLogon service.then pause 
#         the appointed computer's netlogon service.
# Step 2: Using GC to garbage collection.
#--------------------------------------------------------------------------------------

cmd /c exit