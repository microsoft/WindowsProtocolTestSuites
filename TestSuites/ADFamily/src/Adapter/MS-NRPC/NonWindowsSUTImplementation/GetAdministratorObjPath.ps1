#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#----------------------------------------------------------------------------------
# Script function description:
# Step  : This script's function is recompose the format of the parameter 
#         $objectPath. It must recompose the value for $objectPath to be 
#         ActiveDirectory's DN format,and give paraemter $domainNetBiosName's 
#         value to dc=$domainNetBiosName;
#         give paraemter $domainNameSuffix's value to dc=$domainNameSuffix;
#         The last format of $objectPath is this:
#         cn=Administrator, cn=Users, dc=$domainNetBiosName, dc=$domainNameSuffix
#----------------------------------------------------------------------------------

cmd /c exit