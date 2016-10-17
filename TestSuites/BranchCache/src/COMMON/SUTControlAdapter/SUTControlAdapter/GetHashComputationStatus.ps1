######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

$isFinished = $false

$cacheInfo = netsh bra show publicationcache
if($cacheInfo -eq $null)
{
	Write-Host "Cannot get the status of the publication cache."
	return
}

$strVar = "Active Current Cache Size"
[string]$str
[string]$interface
foreach ($str in $cacheInfo)
{                
	if($str -ne $null)
	{
		if($str.contains($strVar))
		{
			[string[]] $strArray = $str.split('=')
			if ($strArray -gt 1)
			{
				$sizeStr = $strArray[1].trim()
					
			}
		}

	}
} 

$sizeStrArray = $sizeStr.split(' ')
[int] $size = $sizeStrArray[0]     

if ($size -gt 0)
{
	$isFinished = $true
}   
else
{
	$isFinished = $false
}  
 
New-Item -Type Directory -Path $ENV:HOMEDRIVE\MS-PCCRTP -Force
echo $isFinished > $ENV:HOMEDRIVE\MS-PCCRTP\isFinished.txt
cmd /c "net share MS-PCCRTP=$ENV:HOMEDRIVE\MS-PCCRTP /grant:everyone,FULL"




