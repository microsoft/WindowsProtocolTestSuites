# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Configures TLS cipher suite order on DC and SUT VMs.

.DESCRIPTION
    Sets the cipher suite priority order via registry to match the pipeline's
    TLS 1.2 configuration. Applied to DC and Node/SUT; Client/Driver requires
    no change.

    The cipher suites are set via Group Policy registry key. Changes take
    effect on the next TLS handshake without requiring a reboot.
#>

[CmdletBinding()]
param()

$cipherSuiteOrder = @(
    'TLS_AES_256_GCM_SHA384'
    'TLS_CHACHA20_POLY1305_SHA256'
    'TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384'
    'TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256'
    'TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384'
    'TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256'
    'TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA384'
    'TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA256'
    'TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384'
    'TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256'
)

$regPath = 'HKLM:\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002'

try {
    if (-not (Test-Path $regPath)) {
        New-Item -Path $regPath -Force | Out-Null
    }

    $newValue = $cipherSuiteOrder -join ','
    $currentValue = (Get-ItemProperty -Path $regPath -Name 'Functions' -ErrorAction SilentlyContinue).Functions

    if ($currentValue -eq $newValue) {
        Write-Output "[OK] TLS cipher suite order already configured"
        return $true
    }

    Set-ItemProperty -Path $regPath -Name 'Functions' -Value $newValue
    Write-Output "[OK] TLS cipher suite order configured ($($cipherSuiteOrder.Count) suites)"
    return $true
}
catch {
    Write-Warning "Failed to configure TLS cipher suites: $($_.Exception.Message)"
    return $false
}
