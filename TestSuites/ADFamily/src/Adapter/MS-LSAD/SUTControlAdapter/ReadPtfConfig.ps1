#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

# Read the property value which has the given property name from ptfconfig.
# param string $name The name of the property to find.
# return string The value of found property. If property is not found,
#     return $null.
param ($name)

$scriptPath = Split-Path $MyInvocation.MyCommand.Definition
	
[xml] $dom = get-content "$scriptPath\..\..\..\AD_ServerTestSuite.deployment.ptfconfig"

$properties = $dom.DocumentElement.FirstChild

# Find the Property node which has the given name and get its value.
$value = $null
foreach ($group in $properties.ChildNodes)
{    
    foreach ($prop in $group.ChildNodes)
	{
	    if ($name -eq $prop.name)
        {
            $value = $prop.value
            break
	    }
    }
}

return $value