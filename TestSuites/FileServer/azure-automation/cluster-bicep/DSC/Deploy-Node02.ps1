# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Deploy-ClusterNode.ps1') `
    -NodeRole 'Node02' -WorkingPath $WorkingPath
