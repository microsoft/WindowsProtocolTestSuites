# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to get the information of all local network interfaces which are connected and of type IPv4.
# Return Value: an array including information of all satisfied local network interfaces.

$results = @()
$adapters = Get-NetAdapter
foreach($adapter in $adapters) {
	if($adapter.Status -ne "Up") {
		continue
	}
	$ipSettings = Get-NetIPAddress -ifIndex $adapter.ifIndex | Where-Object { $_.AddressFamily -eq "IPv4" }
	if($ipSettings -eq $null){
		continue
	}
	$result = "" | Select-Object -Property Name, IpAddress, Description, RDMACapable
	$result.Name = $adapter.Name
	$result.IpAddress = $ipSettings[0].IpAddress
	$result.Description = $adapter.InterfaceDescription
	$rdma = Get-NetAdapterRdma -Name $adapter.Name
	if($rdma -ne $null) {
		$result.RDMACapable = $rdma.Enabled
	}
	else {
		$result.RDMACapable = $false
	}
	$results += $result
}

return $results