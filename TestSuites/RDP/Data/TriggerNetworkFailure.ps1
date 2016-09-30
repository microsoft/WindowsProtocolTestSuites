# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#=========================================================
# This scripts is used to disable and then enable the local network.
# It is only for Windows OSs and it only works in computers that have single network adapter.
#=========================================================

$networkAdapterConfig = Get-WmiObject Win32_NetworkAdapterConfiguration | where {$_.IPEnabled -eq $true}
$networkAdapter = Get-WmiObject Win32_NetworkAdapter | where {$_.Index -eq $networkAdapterConfig.Index}
$networkAdapter.Disable()
Start-Sleep 1
$networkAdapter.Enable()
