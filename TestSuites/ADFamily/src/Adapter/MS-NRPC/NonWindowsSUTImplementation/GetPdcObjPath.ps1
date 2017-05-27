#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------------------
# Script function description:
# Step  : This script's function is recompose the format of the parameter 
#         $objectPath.It must recompose the value for $objectPath to be 
#         ActiveDirectory's DN format,and give parameter $pdcNetBiosName's 
#         value to CN=$pdcNetBiosName;
#         parameter $domainNetBiosName's value to dc=$domainNetBiosName;
#         parameter $domainNameSuffix's value to dc=$domainNameSuffix;
#         The last format of $objectPath is like this:
#         cn=$pdcNetBiosName, cn=computers, dc=$domainNetBiosName, dc=$domainNameSuffix
#-----------------------------------------------------------------------------------------

cmd /c exit