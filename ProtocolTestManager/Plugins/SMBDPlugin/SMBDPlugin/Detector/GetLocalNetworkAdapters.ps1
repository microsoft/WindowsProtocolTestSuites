# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to test the credential of SUT computer
# Return Value: 0/-1 indicates OK/NG

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
	$result = "" | Select-Object -Property Name,IpAddress
	$result.Name = $adapter.Name
	$result.IpAddress = $ipSettings[0].IpAddress
	$results += $result
}

return $results