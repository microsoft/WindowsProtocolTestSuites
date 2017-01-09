#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#----------------------------------------------------------------------------------
# Script function description:
# Step 1: This script's first step is to check the value $objectPath is available 
#         or not,then use $objectPath to determine the $objectInstance's value.
#         the $objectInstance's value is recompose to ADsPath.
# Step 2: Second step is to verify the parameter $attribute is available.
#         if it's available ,then give the $attribute to $value,otherwise, return 
#         null
#----------------------------------------------------------------------------------

cmd /c exit