##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
##################################################################################

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