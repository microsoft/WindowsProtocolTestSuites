# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$results = @()

if ($IsWindows) {
    $adapters = Get-NetAdapter
    foreach($adapter in $adapters) {
        if($adapter.Status -ne "Up") { continue }
        
        $ipSettings = Get-NetIPAddress -ifIndex $adapter.ifIndex | Where-Object { $_.AddressFamily -eq "IPv4" }
        if($null -eq $ipSettings){ continue }

        $result = "" | Select-Object -Property Name, IpAddress, Description, RDMACapable
        $result.Name = $adapter.Name
        $result.IpAddress = $ipSettings[0].IPAddress
        $result.Description = $adapter.InterfaceDescription
        
        $rdma = Get-NetAdapterRdma -Name $adapter.Name
        $result.RDMACapable = if($rdma -ne $null) { $rdma.Enabled } else { $false }
        
        $results += $result
    }
}
else {
    $ipLink = ip -j link show | ConvertFrom-Json
    $ipAddr = ip -j -4 addr show | ConvertFrom-Json

    foreach($link in $ipLink) {
        if($link.ifname -eq "lo" -or $link.operstate -ne "UP") { continue }
        $addrInfo = $ipAddr | Where-Object { $_.ifname -eq $link.ifname }
        if($null -eq $addrInfo -or $addrInfo.addr_info.Count -eq 0) { continue }

        $result = "" | Select-Object -Property Name, IpAddress, Description, RDMACapable
        $result.Name = $link.ifname
        $result.IpAddress = $addrInfo.addr_info[0].local
        $result.Description = "Linux Network Interface: " + $link.ifname

        $sysPath = "/sys/class/net/$($link.ifname)/device/infiniband"
        if (Test-Path $sysPath) {
            $result.RDMACapable = $true
        }
        else {
            $result.RDMACapable = $false
            if (Get-Command rdma -ErrorAction SilentlyContinue) {
                $rdmaCheck = rdma link show | Select-String -Pattern $link.ifname
                if ($null -ne $rdmaCheck) { $result.RDMACapable = $true }
            }
        }

        $results += $result
    }
}

return $results | ConvertTo-Json