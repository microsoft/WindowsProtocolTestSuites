# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Fallback VM sizes per role, in preference order, shared by all scenarios
# (Workgroup, Domain, Cluster). Used when the preferred size (from a scenario's
# bicepparam file) is unavailable or capacity-constrained in the target region.
# deploy.ps1 statically filters this list against the region's SKUs up front
# (Resolve-AvailableVmSize) and retries through it at deployment time on
# SkuNotAvailable/AllocationFailed (Invoke-DeploymentWithSkuFallback).
@{
    DC     = @(
        'Standard_D2s_v6'
        'Standard_D2as_v6'
        'Standard_D2s_v5'
        'Standard_D2as_v5'
        'Standard_B2s_v2'
        'Standard_D2s_v4'
    )
    Driver = @(
        'Standard_F4s_v2'
        'Standard_D4s_v6'
        'Standard_D4as_v6'
        'Standard_D4s_v5'
        'Standard_D4as_v5'
    )
    SUT    = @(
        'Standard_D8s_v6'
        'Standard_D8as_v6'
        'Standard_D8s_v5'
        'Standard_D8as_v5'
        'Standard_D8s_v4'
    )
}
