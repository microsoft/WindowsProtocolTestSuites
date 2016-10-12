######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#-------------------------------------------------------------------------------------
# Script function description:
# Funtion: Set request timer about pccrr on a remote computer which is designated from 
#          parameter $computerName's value. The timer is specified by parameter 
#          $timerInMilliseconds's value. 
# Step 1:  Set request timer about pccrr through updating the registry remotely from 
#          a 32 bit application.
#-------------------------------------------------------------------------------------

param(
[string]$value = $timerInMilliseconds,
[string]$ServerComputerName = $computerName,
[string]$usr = $user,
[string]$pwd = $password
)

cmd /c exit