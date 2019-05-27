#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

$MachineName      = $PTFProp_Common_TDC_NetbiosName
$DomainName       = $PTFProp_Common_TrustDomain_DNSName
$AdminUserName    = $PTFProp_Common_DomainAdministratorName
$AdminPassword    = $PTFProp_Common_DomainUserPassword

# the target account on which the A2AF will apply
$TargetAccount    = $PTFProp_Common_DomainAdministratorName

# RestrictedPrinciple will be passed in from Adapter

# create the credential to remote
$credential = New-Object System.Management.Automation.PSCredential `
              -ArgumentList "$DomainName\$AdminUserName", `
              $(ConvertTo-SecureString -AsPlainText $AdminPassword -Force)

# create authentication policy A2AF
$AddA2AF = [scriptblock] {
    Param ($targetAccount, $restrictedPrinciple)

    $policyName   = "A2AF"

    # retrieve the sid of the target account
    $principleObj = New-Object System.Security.Principal.NTAccount($restrictedPrinciple)
    $principleSid = $principleObj.Translate([System.Security.Principal.SecurityIdentifier]).Value

    # create sddl string to use in UserAllowedToAuthenticateFrom value
    # this value can also be created by Show-ADAuthenticationPolicyExpression
    # this will restrict the $targetAccount to be authenticated from $restrictedPrinciple
    $sddlString   = "O:SYG:SYD:(XA;OICI;CR;;;WD;(Not_Member_of_any{SID($principleSid)}))"
             
    New-ADAuthenticationPolicy -Name $policyName `
                               -UserAllowedToAuthenticateFrom $sddlString `
                               -Enforce

    # apply the policy on the target account
    Set-ADUser -Identity $targetAccount -AuthenticationPolicy $policyName
}


# remove all authentication policy on the remote computer
$RemoveA2AF = [scriptblock] {
    Get-ADAuthenticationPolicy -Filter * | Remove-ADAuthenticationPolicy -Confirm:$false
}

# script block to run on the remote machine
$command = if ($RestrictedPrinciple -ne $null) { $AddA2AF } else { $RemoveA2AF }

Invoke-Command -ComputerName $MachineName -Credential $credential -ScriptBlock $command `
               -ArgumentList $TargetAccount, $RestrictedPrinciple -ErrorAction Stop


