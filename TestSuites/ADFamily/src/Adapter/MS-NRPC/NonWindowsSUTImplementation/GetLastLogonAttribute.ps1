#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Step 1: This script's first step is recompose the parameter $objectInstance's value
#         to ADsPath's format,then check it.
# Step 2: Second step is to verify the parameter $attribute is available.
#         if it's available ,then give the $attribute to $value.
# Step 3: Third step is to recompose the format of $lowPart and $highPart by using 
#         these two parameters to assemble like $lowPart+$highPart.
#-------------------------------------------------------------------------------------

cmd /c exit