# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
[string]$computer, # host name or ip address
[string]$taskName,
[string]$userName,
[string]$userPassword
)

schtasks /run /s $computer /u $userName /p $userPassword /tn $taskName >> ".\RunTask_$taskName.log"