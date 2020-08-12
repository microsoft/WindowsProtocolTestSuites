# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$RemotePassword = $PtfProp_LocalRealm_Users_Admin_Password
[string]$RemoteUserName = $PtfProp_LocalRealm_Users_Admin_Username
[string]$RemoteComputerName = $PtfProp_LocalRealm_KDC01_FQDN

.\Run-TaskOnRemoteComputer $RemoteComputerName $PtfProp_ClearTrustRealmEncType_Task $RemoteUserName $RemotePassword
