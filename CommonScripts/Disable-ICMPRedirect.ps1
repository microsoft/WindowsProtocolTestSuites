#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-------------------------------------------------------------------------------
# Function: Disable-ICMPRedirect
# Remark  : Disable ICMP Redirect
#-------------------------------------------------------------------------------
$regKey = "HKLM:\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"
if(Test-Path -Path $regKey)
{
    Set-ItemProperty -Path $regKey -Name EnableICMPRedirect -value 0
}