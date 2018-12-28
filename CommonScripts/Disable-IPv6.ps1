#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-----------------------------------------------------------------------------
# Function: Disable-IPv6
# Usage   : Disable IPv6 for this machine
# Params  : 
# Remark  : 
#-----------------------------------------------------------------------------

$Adapter = Get-NetAdapter
$Name = $Adapter.Name

Write-Host "Disable IPv6 on $Name ..." -ForegroundColor Yellow
Disable-NetAdapterBinding -Name $Name -ComponentID ms_tcpip6