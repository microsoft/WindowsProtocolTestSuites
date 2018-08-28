##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
##################################################################################

Param (
    [Parameter(Mandatory=$true)]
    [string]$ZipFile,
    [Parameter(Mandatory=$true)]
    [string]$Destination
    )

# check dotnet version
if($PSVersionTable.CLRVersion.Major -ge 4){
	[System.Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem")
	[System.IO.Compression.ZipFile]::ExtractToDirectory($ZipFile, $Destination)
}else
{
	$shell = New-Object -com shell.application
	$zip = $shell.NameSpace($ZipFile)
	if(!(Test-Path -Path $Destination))
	{
		New-Item -ItemType directory -Path $Destination
	}
	$shell.Namespace($Destination).CopyHere($zip.items(), 0x14)
}


