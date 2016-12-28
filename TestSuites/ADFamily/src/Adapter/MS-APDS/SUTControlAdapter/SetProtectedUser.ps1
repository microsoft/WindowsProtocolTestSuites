#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

# parameters passed by PTF
# parameters used in remoting
function GetPtfVariable
{
    param($name)
	$v = Get-Variable -Name ("PTFProp"+$name)
	return $v.Value
}

$MachineName = GetPtfVariable "Common.TDC.NetbiosName"
$DomainName  = GetPtfVariable "Common.TrustDomain.DNSName"
$AdminUserName = GetPtfVariable "Common.DomainAdministratorName"
$AdminPassword = GetPtfVariable "Common.DomainUserPassword"

# UserName will be passed in from Adapter

# create the credential to remote
$credential = New-Object System.Management.Automation.PSCredential `
              -ArgumentList "$DomainName\$AdminUserName", `
              $(ConvertTo-SecureString -AsPlainText $AdminPassword -Force)

# add the user to Protected Users Group
$AddProtectedUser = [scriptblock] {
   param ([string] $username )

   # retrieve Protected Users Group object
   $group = Get-ADGroup -Filter { Name -eq "Protected Users" }

   # if $username contains domain, remove it, just get the pure user name
   $temp = $username.Split('\'); $username = $temp[$temp.Count - 1]

   # retrieve the target user object
   $user  = Get-ADUser -Filter  { Name -eq $username }

   # add the user to Protected Users Group
   Add-ADGroupMember $group -Members $user
}

# remove all user s in Protected Users Group 
$RemoveProtectedUser = [scriptblock] {
    $group = Get-ADGroup -Filter { Name -eq "Protected Users" }
    
    # get all members in the Protected Users Group
    $members = Get-ADGroupMember -Identity $group

    # remove all members
    Remove-ADGroupMember $group -Members $members -Confirm:$false
}

# script block to run on the remote machine
$command = if ($UserName -ne $null) { $AddProtectedUser } else { $RemoveProtectedUser }

Invoke-Command -ComputerName $MachineName -Credential $credential -ScriptBlock $command `
               -ArgumentList $UserName -ErrorAction Stop


