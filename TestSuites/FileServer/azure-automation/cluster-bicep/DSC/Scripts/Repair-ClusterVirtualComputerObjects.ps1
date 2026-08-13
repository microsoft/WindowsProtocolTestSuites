# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$ConfigFile,

    [Parameter(Mandatory)]
    [string]$ResultPath
)

$ErrorActionPreference = 'Stop'

if (-not ('ProtocolTestSuites.ClusterVcoRepair' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Runtime.InteropServices;

namespace ProtocolTestSuites
{
    public static class ClusterVcoRepair
    {
        [DllImport("clusapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenCluster(string clusterName);

        [DllImport("clusapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenClusterResource(IntPtr cluster, string resourceName);

        [DllImport("clusapi.dll", SetLastError = true)]
        public static extern uint ClusterResourceControl(
            IntPtr resource,
            IntPtr hostNode,
            uint controlCode,
            IntPtr inputBuffer,
            uint inputBufferSize,
            IntPtr outputBuffer,
            uint outputBufferSize,
            out uint bytesReturned);

        [DllImport("clusapi.dll")]
        public static extern bool CloseClusterResource(IntPtr resource);

        [DllImport("clusapi.dll")]
        public static extern bool CloseCluster(IntPtr cluster);
    }
}
'@
}

function Get-ExpectedResourceGuid {
    param([guid]$ObjectGuid)

    return (($ObjectGuid.ToByteArray() | ForEach-Object { $_.ToString('x2') }) -join '')
}

function Get-VcoState {
    param(
        [Parameter(Mandatory)] [string]$VcoName,
        [Parameter(Mandatory)] [string]$ResourceName
    )

    $resource = Get-ClusterResource -Name $ResourceName -ErrorAction Stop
    $parameters = @($resource | Get-ClusterParameter -ErrorAction Stop)
    $computer = @(Get-ADComputer -Filter "Name -eq '$VcoName'" `
        -Properties Enabled, DNSHostName, ObjectGUID, ServicePrincipalName `
        -ErrorAction Stop) | Select-Object -First 1
    $resourceGuid = [string](($parameters | Where-Object Name -eq 'ObjectGUID').Value)
    $expectedGuid = if ($computer) { Get-ExpectedResourceGuid -ObjectGuid $computer.ObjectGUID } else { '' }

    [pscustomobject]@{
        Resource = $resource
        Computer = $computer
        Healthy = $null -ne $computer -and
            $computer.Enabled -and
            @($computer.ServicePrincipalName).Count -gt 0 -and
            $resource.State -eq 'Online' -and
            $resourceGuid -eq $expectedGuid -and
            (($parameters | Where-Object Name -eq 'StatusDNS').Value -eq 0) -and
            (($parameters | Where-Object Name -eq 'StatusKerberos').Value -eq 0)
    }
}

function Invoke-NativeVcoRepair {
    param([Parameter(Mandatory)] [string]$ResourceName)

    $clusterHandle = [IntPtr]::Zero
    $resourceHandle = [IntPtr]::Zero
    try {
        $clusterHandle = [ProtocolTestSuites.ClusterVcoRepair]::OpenCluster($null)
        if ($clusterHandle -eq [IntPtr]::Zero) {
            throw "OpenCluster failed with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
        }
        $resourceHandle = [ProtocolTestSuites.ClusterVcoRepair]::OpenClusterResource(
            $clusterHandle,
            $ResourceName)
        if ($resourceHandle -eq [IntPtr]::Zero) {
            throw "OpenClusterResource failed for '$ResourceName' with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
        }

        [uint32]$bytesReturned = 0
        $returnCode = [ProtocolTestSuites.ClusterVcoRepair]::ClusterResourceControl(
            $resourceHandle,
            [IntPtr]::Zero,
            [uint32]0x0100018D,
            [IntPtr]::Zero,
            0,
            [IntPtr]::Zero,
            0,
            [ref]$bytesReturned)
        if ($returnCode -ne 0) {
            throw "VCO repair control for '$ResourceName' returned Win32 error $returnCode."
        }
    } finally {
        if ($resourceHandle -ne [IntPtr]::Zero) {
            [void][ProtocolTestSuites.ClusterVcoRepair]::CloseClusterResource($resourceHandle)
        }
        if ($clusterHandle -ne [IntPtr]::Zero) {
            [void][ProtocolTestSuites.ClusterVcoRepair]::CloseCluster($clusterHandle)
        }
    }
}

try {
    Import-Module ActiveDirectory -ErrorAction Stop
    Import-Module FailoverClusters -ErrorAction Stop
    $config = Get-Content -LiteralPath $ConfigFile -Raw | ConvertFrom-Json -ErrorAction Stop
    $domain = Get-ADDomain -ErrorAction Stop
    $cnoName = $config.Endpoints.Cluster.Name
    $cno = Get-ADComputer -Identity $cnoName -Properties SID -ErrorAction Stop
    $mappings = @(
        [pscustomobject]@{
            VcoName = $config.Endpoints.GeneralFS.Name
            ResourceName = $config.Endpoints.GeneralFS.Name
            GroupName = $config.Endpoints.GeneralFS.Name
        }
        [pscustomobject]@{
            VcoName = $config.Endpoints.ScaleoutFS.Name
            ResourceName = $config.Endpoints.ScaleoutFS.Name
            GroupName = $config.Endpoints.ScaleoutFS.Name
        }
    )
    if ($config.Endpoints.InfrastructureFS -and
        -not [string]::IsNullOrWhiteSpace($config.Endpoints.InfrastructureFS.Name)) {
        $mappings += [pscustomobject]@{
            VcoName = $config.Endpoints.InfrastructureFS.Name
            ResourceName = 'Infrastructure File Server Name'
            GroupName = 'Infrastructure File Server'
        }
    }

    $outcomes = foreach ($mapping in $mappings) {
        $state = Get-VcoState -VcoName $mapping.VcoName -ResourceName $mapping.ResourceName
        if ($state.Healthy) {
            "$($mapping.VcoName)=Healthy"
            continue
        }

        if (-not $state.Computer) {
            New-ADComputer -Name $mapping.VcoName `
                -SamAccountName "$($mapping.VcoName)$" `
                -DNSHostName "$($mapping.VcoName).$($domain.DNSRoot)" `
                -Path $domain.ComputersContainer -Enabled $true -ErrorAction Stop
        } elseif (-not $state.Computer.Enabled) {
            Enable-ADAccount -Identity $state.Computer -ErrorAction Stop
        }

        $computer = Get-ADComputer -Identity $mapping.VcoName -ErrorAction Stop
        $acl = Get-Acl -Path "AD:$($computer.DistinguishedName)" -ErrorAction Stop
        $rule = [DirectoryServices.ActiveDirectoryAccessRule]::new(
            $cno.SID,
            [DirectoryServices.ActiveDirectoryRights]::GenericAll,
            [Security.AccessControl.AccessControlType]::Allow)
        [void]$acl.AddAccessRule($rule)
        Set-Acl -Path "AD:$($computer.DistinguishedName)" -AclObject $acl -ErrorAction Stop

        try {
            Stop-ClusterResource -Name $mapping.ResourceName -Wait 60 -ErrorAction Stop | Out-Null
            Invoke-NativeVcoRepair -ResourceName $mapping.ResourceName
        } finally {
            Start-ClusterGroup -Name $mapping.GroupName -Wait 120 -ErrorAction Stop | Out-Null
        }

        $state = Get-VcoState -VcoName $mapping.VcoName -ResourceName $mapping.ResourceName
        if (-not $state.Healthy) {
            throw "VCO '$($mapping.VcoName)' did not satisfy AD, SPN, GUID, DNS, Kerberos, and online postconditions after repair."
        }
        "$($mapping.VcoName)=Repaired"
    }

    "SUCCESS|$($outcomes -join ';')" | Set-Content -LiteralPath $ResultPath -Encoding UTF8
} catch {
    "ERROR|$($_.Exception.Message)" | Set-Content -LiteralPath $ResultPath -Encoding UTF8
    throw
}