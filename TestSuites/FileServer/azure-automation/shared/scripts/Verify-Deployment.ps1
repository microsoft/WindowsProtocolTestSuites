# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Verify-Deployment.ps1
# Polls all VMs in a deployment resource group for completion signal files.
# Works for Domain, Cluster, and Workgroup scenarios by auto-detecting VMs.

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$false)]
    [int]$TimeoutMinutes = 60,

    [Parameter(Mandatory=$false)]
    [int]$PollIntervalSeconds = 30
)

# VM name patterns → expected signal file and role label.
# Ordered by typical completion time (fastest first) so early polls show progress.
$vmSignalMap = @(
    @{ Pattern = '*-storage01'; Signal = 'Deploy-Storage.Completed.signal'; Role = 'Storage Server' }
    @{ Pattern = '*-dc01';      Signal = 'Deploy-DC.Completed.signal';      Role = 'Domain Controller' }
    @{ Pattern = '*-client01';  Signal = 'Deploy-Driver.Completed.signal';  Role = 'Driver Computer' }
    @{ Pattern = '*-node01';    Signal = 'Deploy-SUT.Completed.signal';     Role = 'SUT' }
    @{ Pattern = '*-node02';    Signal = 'Deploy-Node02.Completed.signal';  Role = 'Cluster Node 2' }
)

# Node01 can be either a standalone SUT (Domain/Workgroup) or a cluster node.
# If both node01 and node02 exist, node01 is a cluster node with a different signal file.
$clusterNode01Override = @{
    Pattern = '*-node01'
    Signal  = 'Deploy-Node01.Completed.signal'
    Role    = 'Cluster Node 1'
}

Write-Output "Verifying deployment completion..."
Write-Output "Resource Group: $ResourceGroupName"
Write-Output "Timeout: $TimeoutMinutes minutes | Poll interval: $PollIntervalSeconds seconds"
Write-Output ""

# Discover VMs in the resource group
$allVms = Get-AzVM -ResourceGroupName $ResourceGroupName -ErrorAction Stop
if ($allVms.Count -eq 0) {
    Write-Error "No VMs found in resource group '$ResourceGroupName'."
    exit 1
}

# If node02 exists, this is a cluster scenario — adjust node01's signal file
$hasNode02 = $allVms | Where-Object { $_.Name -like '*-node02' }
if ($hasNode02) {
    $vmSignalMap = $vmSignalMap | ForEach-Object {
        if ($_.Pattern -eq '*-node01') { $clusterNode01Override } else { $_ }
    }
}

# Match patterns to actual VMs, skip patterns with no matching VM
$targets = @()
foreach ($entry in $vmSignalMap) {
    $vm = $allVms | Where-Object { $_.Name -like $entry.Pattern } | Select-Object -First 1
    if ($vm) {
        $targets += @{
            VMName     = $vm.Name
            Signal     = $entry.Signal
            Role       = $entry.Role
        }
    }
}

if ($targets.Count -eq 0) {
    Write-Error "No VMs matched the expected naming patterns (*-dc01, *-node01, *-client01, etc.)."
    exit 1
}

# Detect the package folder name by checking the DC or first available VM.
# Each scenario uses a different package name (Domain-Package, Cluster-Package, Workgroup-Package).
$packageFolder = $null
$probeVm = $targets[0]
try {
    $probeResult = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
        -VMName $probeVm.VMName -CommandId 'RunPowerShellScript' `
        -ScriptString "Get-ChildItem C:\*-Package -Directory -Name -ErrorAction SilentlyContinue" `
        -ErrorAction Stop
    $packageFolder = ($probeResult.Value[0].Message -split "`n" | Where-Object { $_ -match '-Package$' } | Select-Object -First 1).Trim()
} catch {
    Write-Warning "Could not detect package folder on $($probeVm.VMName): $_"
}
if (-not $packageFolder) { $packageFolder = '*-Package' }

Write-Output "Detected package folder: $packageFolder"
Write-Output "Monitoring $($targets.Count) VMs..."
Write-Output ""

$startTime = Get-Date
$timeoutTime = $startTime.AddMinutes($TimeoutMinutes)
$allComplete = $false

while (-not $allComplete -and (Get-Date) -lt $timeoutTime) {
    $elapsed = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
    Write-Output "[$elapsed min] Checking VM configuration status..."

    $allComplete = $true
    $statusList = @()

    foreach ($target in $targets) {
        Write-Output "  Checking $($target.VMName) ($($target.Role))..."

        try {
            $checkScript = "Get-ChildItem 'C:\$packageFolder\DSC\$($target.Signal)' -ErrorAction SilentlyContinue | Select -ExpandProperty Name"
            $result = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                -VMName $target.VMName -CommandId 'RunPowerShellScript' `
                -ScriptString $checkScript -ErrorAction Stop

            $output = $result.Value[0].Message

            if ($output -match $target.Signal) {
                Write-Output "    [OK] COMPLETE"
                $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'COMPLETE' }
            } else {
                Write-Output "    ... IN PROGRESS"
                $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'IN PROGRESS' }
                $allComplete = $false
            }
        } catch {
            Write-Output "    [!] ERROR: $($_.Exception.Message)"
            $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'ERROR' }
            $allComplete = $false
        }
    }

    Write-Output ""

    if (-not $allComplete) {
        Write-Output "Not all VMs complete. Next check in $PollIntervalSeconds seconds (timeout at $(Get-Date $timeoutTime -Format 'HH:mm:ss'))..."
        Write-Output ""
        Start-Sleep -Seconds $PollIntervalSeconds
    }
}

# Final report
Write-Output "======================================================="
Write-Output "FINAL STATUS"
Write-Output "======================================================="

$completeCount = 0
foreach ($s in $statusList) {
    $icon = switch ($s.Status) {
        'COMPLETE'    { '[OK]'; $completeCount++; break }
        'IN PROGRESS' { '[..]'; break }
        'ERROR'       { '[!!]'; break }
    }
    Write-Output "  $icon $($s.Role) ($($s.VM)): $($s.Status)"
}

$totalTime = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
Write-Output ""
Write-Output "  $completeCount/$($statusList.Count) VMs completed in $totalTime minutes"
Write-Output "======================================================="

if ($allComplete) {
    Write-Output ""
    Write-Output "All VMs have completed their configuration."

    if ($hasNode02) {
        Write-Output ""
        Write-Output "Next: Connect to Node01 via Bastion and follow the"
        Write-Output "post-deployment cluster setup steps in the README."
    }

    exit 0
} else {
    Write-Output ""
    Write-Output "TIMEOUT: Some VMs have not completed configuration."
    Write-Output ""
    Write-Output "Troubleshooting:"
    Write-Output "  1. Re-run this script to continue polling"
    Write-Output "  2. Connect to VMs via Bastion and check logs:"
    Write-Output "     C:\$packageFolder\DSC\Deploy-*.log"
    Write-Output "  3. Check deploy step progress:"
    Write-Output "     Get-ItemProperty HKLM:\SOFTWARE\ProtocolTestSuites"
    exit 1
}
