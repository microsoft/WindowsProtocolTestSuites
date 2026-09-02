# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Verify-ClusterDeployment.ps1
# Checks if all VMs in the cluster deployment have completed their configuration

param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$false)]
    [int]$TimeoutMinutes = 60,

    [Parameter(Mandatory=$false)]
    [int]$PollIntervalSeconds = 30,

    [Parameter(Mandatory=$false)]
    [string]$SubscriptionId,

    [Parameter(Mandatory=$false)]
    [int]$ProbeTimeoutSeconds = 120,

    [Parameter(Mandatory=$false)]
    [datetime]$NotBeforeUtc,

    [Parameter(Mandatory=$false)]
    [switch]$WaitForTests,

    [Parameter(Mandatory=$false)]
    [switch]$DeferTestFailure,

    [Parameter(Mandatory=$false)]
    [string[]]$ExpectedRoles,

    [Parameter(Mandatory=$false)]
    [int]$TestTimeoutMinutes = 180,

    [Parameter(Mandatory=$false)]
    [string]$ResultsStorageAccountName
)

$sharedVerifier = Join-Path $PSScriptRoot '..\..\shared\scripts\Verify-Deployment.ps1'
$verificationParams = @{
    ResourceGroupName = $ResourceGroupName
    Scenario = 'Cluster'
    TimeoutMinutes = $TimeoutMinutes
    PollIntervalSeconds = $PollIntervalSeconds
    ProbeTimeoutSeconds = $ProbeTimeoutSeconds
    TestTimeoutMinutes = $TestTimeoutMinutes
}
if ($SubscriptionId) { $verificationParams['SubscriptionId'] = $SubscriptionId }
if ($PSBoundParameters.ContainsKey('NotBeforeUtc')) { $verificationParams['NotBeforeUtc'] = $NotBeforeUtc }
if ($WaitForTests) { $verificationParams['WaitForTests'] = $true }
if ($DeferTestFailure) { $verificationParams['DeferTestFailure'] = $true }
if ($ExpectedRoles) { $verificationParams['ExpectedRoles'] = $ExpectedRoles }
if ($ResultsStorageAccountName) { $verificationParams['ResultsStorageAccountName'] = $ResultsStorageAccountName }

& $sharedVerifier @verificationParams
return

Write-Output "🔍 Verifying cluster deployment completion..."
Write-Output "Resource Group: $ResourceGroupName"
Write-Output "Timeout: $TimeoutMinutes minutes"
Write-Output "Poll Interval: $PollIntervalSeconds seconds"
Write-Output ""

# Define expected VMs and their signal files
$vmConfigs = @(
    @{ Name = "*-dc01"; SignalFile = "C:\Cluster-Package\DSC\Deploy-DC.Completed.signal"; Role = "Domain Controller" }
    @{ Name = "*-storage01"; SignalFile = "C:\Cluster-Package\DSC\Deploy-Storage.Completed.signal"; Role = "Storage Server" }
    @{ Name = "*-node01"; SignalFile = "C:\Cluster-Package\DSC\Deploy-Node01.Completed.signal"; Role = "Cluster Node 1" }
    @{ Name = "*-node02"; SignalFile = "C:\Cluster-Package\DSC\Deploy-Node02.Completed.signal"; Role = "Cluster Node 2" }
    @{ Name = "*-client01"; SignalFile = "C:\Cluster-Package\DSC\Deploy-Driver.Completed.signal"; Role = "Driver Computer" }
)

$startTime = Get-Date
$timeoutTime = $startTime.AddMinutes($TimeoutMinutes)
$allComplete = $false

while (-not $allComplete -and (Get-Date) -lt $timeoutTime) {
    $elapsed = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
    Write-Output "[$elapsed min] Checking VM configuration status..."

    $allComplete = $true
    $statusSummary = @()

    foreach ($vmConfig in $vmConfigs) {
        # Find VM matching the pattern
        $vm = Get-AzVM -ResourceGroupName $ResourceGroupName | Where-Object { $_.Name -like $vmConfig.Name } | Select-Object -First 1

        if (-not $vm) {
            Write-Output "⚠️  VM matching pattern '$($vmConfig.Name)' not found"
            $statusSummary += @{ VM = $vmConfig.Name; Status = "NOT FOUND"; Role = $vmConfig.Role }
            $allComplete = $false
            continue
        }

        Write-Output "  Checking $($vm.Name) ($($vmConfig.Role))..."

        # Use Azure Run Command to check for signal file
        try {
            $command = "Test-Path '$($vmConfig.SignalFile)'"
            $result = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                -VMName $vm.Name `
                -CommandId 'RunPowerShellScript' `
                -ScriptString $command `
                -ErrorAction Stop

            $output = $result.Value[0].Message

            if ($output -match "True") {
                Write-Output "    ✅ COMPLETE - Signal file found"
                $statusSummary += @{ VM = $vm.Name; Status = "COMPLETE"; Role = $vmConfig.Role }
            } else {
                Write-Output "    ⏳ IN PROGRESS - Signal file not found yet"
                $statusSummary += @{ VM = $vm.Name; Status = "IN PROGRESS"; Role = $vmConfig.Role }
                $allComplete = $false
            }
        } catch {
            Write-Output "    ⚠️  ERROR checking status: $($_.Exception.Message)"
            $statusSummary += @{ VM = $vm.Name; Status = "ERROR"; Role = $vmConfig.Role }
            $allComplete = $false
        }
    }

    Write-Output ""

    if (-not $allComplete) {
        Write-Output "⏳ Not all VMs are complete. Waiting $PollIntervalSeconds seconds before next check..."
        Write-Output "   (Will timeout at $(Get-Date $timeoutTime -Format 'HH:mm:ss'))"
        Write-Output ""
        Start-Sleep -Seconds $PollIntervalSeconds
    }
}

# Final status report
Write-Output "═══════════════════════════════════════════════════════"
Write-Output "FINAL STATUS REPORT"
Write-Output "═══════════════════════════════════════════════════════"

$completeCount = 0
$totalCount = 0

foreach ($status in $statusSummary) {
    $totalCount++
    $icon = switch ($status.Status) {
        "COMPLETE" { "✅"; $completeCount++; break }
        "IN PROGRESS" { "⏳"; break }
        "ERROR" { "❌"; break }
        "NOT FOUND" { "⚠️ "; break }
    }
    Write-Output "$icon $($status.Role) ($($status.VM)): $($status.Status)"
}

Write-Output ""
Write-Output "Summary: $completeCount/$totalCount VMs completed"
Write-Output "Total time: $([math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)) minutes"
Write-Output "═══════════════════════════════════════════════════════"

if ($allComplete) {
    Write-Output ""
    Write-Output "🎉 SUCCESS! All VMs have completed their configuration."
    Write-Output ""
    Write-Output "Next Steps:"
    Write-Output "  1. Connect to cluster nodes via Azure Bastion"
    Write-Output "  2. Create the failover cluster using the provided scripts"
    Write-Output "  3. Configure Scale-Out File Server"
    Write-Output "  4. Run your FileServer test suite"
    exit 0
} else {
    Write-Output ""
    Write-Output "⏰ TIMEOUT or INCOMPLETE"
    Write-Output ""
    Write-Output "Some VMs have not completed configuration. You can:"
    Write-Output "  1. Run this script again to continue checking"
    Write-Output "  2. Connect to VMs via Bastion to check logs:"
    Write-Output "     - DC01: C:\dc-extension-setup.log"
    Write-Output "     - Storage01: C:\storage-extension-setup.log"
    Write-Output "     - Node01/02: C:\node01-extension-setup.log, C:\node02-extension-setup.log"
    Write-Output "     - Client01: C:\driver-extension-setup.log"
    Write-Output "  3. Check for signal files manually on each VM"
    exit 1
}
