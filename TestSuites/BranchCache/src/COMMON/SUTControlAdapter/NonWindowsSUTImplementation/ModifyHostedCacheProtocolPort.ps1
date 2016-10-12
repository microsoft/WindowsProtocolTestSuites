######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Fuction: Modify the hosted cache protocol port which is designated by the parameter
#          $newPort's value.
#          Including two parts: modify the hosted cache client connect port; Modify the
#          hosted cache server listen port.
# Step 1:  Modify the hosted cache connect port on the hosted cache client which is
#          designated by the parameter $hostedClientComputerName's value.
#          Or modify the hosted cache listen port on the hosted cache server which is
#          designated by the parameter $hostedServerComputerName's value through updating
#          the registry remotely from a 32 bit application.
#-------------------------------------------------------------------------------------

param(
[string]$hostedClient = $hostedClientComputerName,
[string]$hostedServer = $hostedServerComputerName,
[string]$port = $newPort
)

cmd /c exit