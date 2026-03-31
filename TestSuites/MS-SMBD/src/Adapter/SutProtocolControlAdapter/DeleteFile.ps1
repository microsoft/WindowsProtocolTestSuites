# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
[string]$fileName
[string]$domain = $PtfProp_DomainName
[string]$userName = $PtfProp_SutUserName
[string]$password = $PtfProp_SutPassword

$share = "\\" + $PtfProp_SutComputerName + "\" + $PtfProp_ShareFolder

if($Domain -eq $null -or $Domain.trim() -eq "")
{
    $account = $UserName
}

$account = "$Domain\$UserName"
$SecurePassword = New-Object System.Security.SecureString

for($i=0; $i -lt $Password.Length; $i++)
{
    $SecurePassword.AppendChar($Password[$i])
}

$credential = New-Object system.Management.Automation.PSCredential($account,$SecurePassword)

if ($env:OS -eq "Windows_NT") {
	try
	{
  	  net use /del * /y
  	  New-PSDrive -psProvider FileSystem -name TestDrive -root $share -credential $credential
  	  Remove-Item TestDrive:\$fileName -force
  	  Remove-PSDrive -name TestDrive
	}
	catch
	{
  	  net use /del * /y
	}
} else {
	try {
		if (-Not (Test-Path /mnt/$PtfProp_ShareFolder)) {
    			sudo mkdir -p /mnt/$PtfProp_ShareFolder
		}
		
		$osRelease = Get-Content /etc/os-release
		$osID = $osRelease | Where-Object { $_ -match "^ID=" } | ForEach-Object { $_.Split('=')[1].Trim('"') }

		if ($osID -match "debian") {
    		sudo apt-get install -y cifs-utils
		} elseif ($osID -match "rhel" -or $osID -match "centos" -or $osID -match "fedora") {
    		sudo dnf install -y cifs-utils
		} elseif ($osID -match "suse") {
			sudo zypper install -y cifs-utils
		} else {
    		Write-Host "Unknown system type."
		}

		sudo bash -c "mount | grep cifs | awk '{print $3}' | xargs -I {} sudo umount {}"
		sudo bash -c "mount -t cifs //$PtfProp_SutComputerName/$PtfProp_ShareFolder /mnt/$PtfProp_ShareFolder -o username='$userName',password='$password'"
		sudo rm -f "/mnt/$PtfProp_ShareFolder/$fileName"
		sudo umount /mnt/$PtfProp_ShareFolder
	} catch {
        sudo bash -c "mount | grep cifs | awk '{print $3}' | xargs -I {} sudo umount {}"
	}
}