# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Trigger Close RDP Window
# Note: we don't use TriggerClientDisconnectAll.ps1 here, because TriggerClientDisconnectAll.ps1 will force terminate all RDP client processes for cleanup and won't trigger the required User-Initiated on Client disconnection sequence.
$processes = Get-Process mstsc -ErrorAction SilentlyContinue
$processes | ForEach-Object { $_.CloseMainWindow(); }