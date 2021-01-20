# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
if ($IsWindows) {
    Write-Host "Flush dns on Windows"
    ipconfig /flushdns
}