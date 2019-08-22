# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Check or extract header files from Network Direct DDK

param (
	[ValidateSet("Check", "Install")]
	[string]$Action,
	[string]$DownloadedArtifact	# Path to the downloaded Network Direct DDK zip file
)

Function Check-HeaderExistance {

    $RDMASDKExisted = Test-Path -Path "..\ProtoSDK\RDMA"
    if (!$RDMASDKExisted)
    {
        Write-Host "The source code of RDMA SDK is not downloaded. Please download the source code first and then rerun the script." -ForegroundColor Yellow
        return $true # return true to avoid download the Network Direct DDK zip file 
    }

    $HeaderFolderPath = (Get-Item -Path "..\ProtoSDK\RDMA").FullName

    $HeaderExisted = Test-Path -Path $HeaderFolderPath\include\ndspi.h
    if ($HeaderExisted)
    {
        $HeaderExisted = Test-Path -Path $HeaderFolderPath\include\ndstatus.h
    }

    if ($HeaderExisted)
    {
        Write-Host "The two header files of Network Direct DDK already exist in the folder $HeaderFolderPath."
        return $true
    }
    else
    {
        Write-Host "The two header files of Network Direct DDK do not exist in the folder $HeaderFolderPath" -ForegroundColor Yellow
        return $false
    }
}

Function Extract-Header{
    try {
        $ArtifactFolder = (Get-Item -Path $DownloadedArtifact).Directory.FullName
        Expand-Archive $DownloadedArtifact -DestinationPath $ArtifactFolder
        robocopy $ArtifactFolder\NetDirect\include\ "..\ProtoSDK\RDMA\include\"        
    }
    catch {
        Write-Host "Extract header files from Network Direct DDK failed." -ForegroundColor Red
        return $false
    }

    return $true
}

switch($Action) {
	"Check" {
		$isInstalled = Check-HeaderExistance
		return $isInstalled
	}

	"Install" {
		$result = Extract-Header
		return $result
	}
}