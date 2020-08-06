# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$remoteComputerName
[string]$remoteUsername
[string]$remotePassword

.\Run-TaskOnRemoteComputer $remoteComputerName $PtfProp_SetSupportedEncryptionTypesAsRc4_Task $remoteUsername $remotePassword