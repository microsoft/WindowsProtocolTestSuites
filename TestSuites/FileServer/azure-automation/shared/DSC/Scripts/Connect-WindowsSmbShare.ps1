# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'CredentialPassword',
    Justification = 'The password is passed directly to the Windows network provider and never placed on a process command line.')]
[CmdletBinding()]
param(
    [Parameter(Mandatory)] [string]$RemotePath,
    [Parameter(Mandatory)] [string]$CredentialUser,
    [Parameter(Mandatory)] [string]$CredentialPassword
)

$ErrorActionPreference = 'Stop'
if (-not ('ProtocolTestSuites.NetworkConnection' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Runtime.InteropServices;

namespace ProtocolTestSuites
{
    public static class NetworkConnection
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct NetResource
        {
            public int Scope;
            public int Type;
            public int DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        public static extern int WNetAddConnection2(
            ref NetResource netResource,
            string password,
            string userName,
            int flags);
    }
}
'@
}

$resource = [ProtocolTestSuites.NetworkConnection+NetResource]::new()
$resource.Type = 1
$resource.RemoteName = $RemotePath
$result = [ProtocolTestSuites.NetworkConnection]::WNetAddConnection2(
    [ref]$resource,
    $CredentialPassword,
    $CredentialUser,
    0)
if ($result -eq 1219) {
    $exception = [ComponentModel.Win32Exception]::new($result)
    throw "WNetAddConnection2 returned 1219 ($($exception.Message)). Windows already has a connection to the same server using different credentials, so the requested credential session was not established. Clear the conflicting connection and retry."
}
if ($result -ne 0) {
    throw [ComponentModel.Win32Exception]::new($result)
}
return $result
