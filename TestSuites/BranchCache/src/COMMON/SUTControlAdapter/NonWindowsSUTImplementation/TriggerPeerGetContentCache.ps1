######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Function: Trigger the peer client to get content from
#           the content server which is designated from parameter $contentServerName's
#           value. The peer client is specified by the parameter $peerServerName's value.
#           The content is specified by the parameter $contentUri's value.
# Step 1:   Clear the cache including browser cache and branch cache on the peer client.
# Step 2:   Open the browser to get the content from the content server on the peer client.
#-------------------------------------------------------------------------------------

param(
[string]$ServerComputerName = $peerServerName,
[string]$ContentServerName = $contentServerName,
[string]$path = $contentUri,
[string]$usr = $user,
[string]$pwd = $password
)

cmd /c exit