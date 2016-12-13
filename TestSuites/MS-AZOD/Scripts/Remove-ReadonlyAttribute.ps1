## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param (
    [Parameter(Mandatory=$true)]
    [string]$Path
    )

$Files = Get-ChildItem $Path -Recurse
ForEach ($File in $Files) 
{
    if ($File.IsReadOnly -eq $true )
    {
        Set-ItemProperty -path $File.FullName -name IsReadOnly -value $false 
        write-host "file:" $File.FullName "is now writable."
    }
}