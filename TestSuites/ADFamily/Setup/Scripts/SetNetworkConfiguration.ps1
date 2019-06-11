#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string[]]$IPAddress,

    [AllowNull()]
    [AllowEmptyCollection()]
    [string[]]$SubnetMask,

    [AllowNull()]
    [AllowEmptyCollection()]
    [string[]]$Gateway,

    [AllowNull()]
    [AllowEmptyCollection()]
    [string[]]$DNS
)

Function ValidateArgument([string[]]$StringCollection)
{
    if (($StringCollection -eq $null) -or ($StringCollection.Count -eq 0))
    {
        return $null
    }

    [bool]$EmptyCollection = $true

    foreach ($item in $StringCollection)
    {
        if($item -ne "")
        {
            $EmptyCollection = $false
            # Validate IP Address format
            $Temp = New-Object System.Net.IPAddress -ArgumentList 0
            $IsValid = [System.Net.IPAddress]::TryParse($item, ([ref]$Temp))
            if($IsValid -eq $false)
            {
                throw "Invalid argument"
            }
        }
    }

    if($EmptyCollection -eq $true)
    {
        return $null
    }
    return $StringCollection
}

try
{
    # Convert to array
    [String[]]$IPAddressArray  = ValidateArgument -StringCollection $IPAddress
    [String[]]$SubnetMaskArray = ValidateArgument -StringCollection $SubnetMask
    [String[]]$GatewayArray    = ValidateArgument -StringCollection $Gateway
    [String[]]$DNSArray        = ValidateArgument -StringCollection $DNS

    # If no subnet mask specified, take the default value.
    if ($SubnetMaskArray -eq $null)
    {
        foreach ($IP in $IPAddressArray)
        {
            $SubnetMaskArray += "255.255.255.0"
        }
    }
    # If the number of the subnet masks does not match the number of IP addresses,
    # make them the same length.
    else
    {
        if($SubnetMaskArray.Length -lt $IPAddressArray.Length)
        {
            $LastValue = $SubnetMaskArray[$SubnetMaskArray.Length - 1]
            for ($count = $SubnetMaskArray.Length; $count -lt $IPAddressArray.Length; $count ++)
            {
                $SubnetMaskArray += $LastValue
            }
        }
        elseif($SubnetMaskArray.Length -gt $IPAddressArray.Length)
        {
            $Temp = @()
            for ($count = 0; $count -lt $IPAddressArray.Length; $count ++)
            {
                $Temp += $SubnetMaskArray[$count]
            }
            $SubnetMaskArray = $Temp
        }
    }
}
catch
{
    throw "Invalid arguments. "+$_.Exception.Message
}

# WMI will return an array of values.
# If any of these values is zero, return zero;
# Or return the last return value.
Function GetExitCode($Obj)
{
    $ReturnValues = [int[]]$Obj.ReturnValue

    foreach ($Value in $ReturnValues)
    {
        if ($Value -eq 0)
        {
            $ExitCode = 0
            break
        }
        else
        {
            $ExitCode = $Value
        }
    }
    
    return $ExitCode
}

# Get WMI Object
$NetObj = Get-WmiObject Win32_NetworkAdapterConfiguration -ErrorAction Stop
$ExitCode = 0

# Set IP Addresses
$ReturnObj = $NetObj.EnableStatic($IPAddressArray,$SubnetMaskArray)
$ExitCode += GetExitCode $ReturnObj

# Set Gateways
if ($GatewayArray -ne $null)
{
    $ReturnObj = $NetObj.SetGateways($GatewayArray)
    $ExitCode += GetExitCode $ReturnObj
}
       
# Set DNS
if($DNSArray -ne $null)
{
    $ReturnObj = $NetObj.SetDNSServerSearchOrder($DNSArray)
    $ExitCode += GetExitCode $ReturnObj
}

# If any of the tasks above failed, throw an exception
if($ExitCode -ne 0)
{
    throw "Error happened in setting the computer IP and DNS."
}
