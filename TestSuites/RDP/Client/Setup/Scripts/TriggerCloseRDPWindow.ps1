# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Trigger Close RDP Window
# We don't use DisconnectAll.bat to close rdp connection, because DisconnectAll.bat doesn't send Client_Shutdown_Request_Pdu pdu to the driver.
# And we use CloseMainWindow to close rdp connection that sends Client_Shutdown_Request_Pdu pdu to the driver.
$processes = Get-Process mstsc -ErrorAction SilentlyContinue
$processes | ForEach-Object {$_.CloseMainWindow();}