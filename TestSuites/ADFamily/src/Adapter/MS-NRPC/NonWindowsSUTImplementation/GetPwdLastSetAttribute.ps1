#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#--------------------------------------------------------------------------------------
# Script function description:
# Step 1: This script's function is give the parameter $objectPath's value from 
#         script "GetAdministratorObjPath.ps1" then give it to $objectInstance to 
#         recompose the $objectInstance's format to ADsPath's format,then check it is
#         available.
# Step 2: Verify the parameter $adsLargeInteger is available.
# Step 3: Third step is to recompose the format of $lowPart and $highPart by using 
#         these two parameters to assemble like $lowPart+$highPart.
#--------------------------------------------------------------------------------------

cmd /c exit