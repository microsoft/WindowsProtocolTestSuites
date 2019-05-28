#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

$MachineName      = $PTFProp_Common_WritableDC1_NetbiosName
$DomainName       = $PTFProp_Common_PrimaryDomain_DNSName
$AdminUserName    = $PTFProp_Common_DomainAdministratorName
$AdminPassword    = $PTFProp_Common_DomainUserPassword

# the target account on which the A2A2 will apply
# this is the driver computer name, A2A2 must apply
# to server principle account, or it won't work
$TargetAccount    = $PTFProp_Common_ENDPOINT_NetbiosName

# RestrictedPrinciple will be passed in from Adapter

# create the credential to remote
$credential = New-Object System.Management.Automation.PSCredential `
              -ArgumentList "$DomainName\$AdminUserName", `
              $(ConvertTo-SecureString -AsPlainText $AdminPassword -Force)

# create authentication policy A2A2
$AddA2A2 = [scriptblock] {
    Param ($target, $restrictedPrinciple)

    $policyName   = "ComputerRestrictedPolicy"

    # retrieve the sid of the restricted principle
    $principleObj = New-Object System.Security.Principal.NTAccount($restrictedPrinciple)
    $principleSid = $principleObj.Translate([System.Security.Principal.SecurityIdentifier]).Value

    # create sddl string to use in UserAllowedToAuthenticateTo value
    # this value can also be created by Show-ADAuthenticationPolicyExpression
    # this will restrict the $restrictedPrinciple to authenticate to $target
    $sddlString   = "O:SYG:SYD:(XA;OICI;CR;;;WD;(Not_Member_of_any{SID($principleSid)}))"
             
    New-ADAuthenticationPolicy -Name $policyName `
                               -ComputerAllowedToAuthenticateTo $sddlString `
                               -Enforce

    # apply the policy on the target computer
    Set-ADComputer -Identity $target -AuthenticationPolicy $policyName
}


# remove all authentication policy on the remote computer
$RemoveA2A2 = [scriptblock] {
    Get-ADAuthenticationPolicy -Filter * | Remove-ADAuthenticationPolicy -Confirm:$false
}

# script block to run on the remote machine
$command = if ($RestrictedPrinciple -ne $null) { $AddA2A2 } else { $RemoveA2A2 }

Invoke-Command -ComputerName $MachineName -Credential $credential -ScriptBlock $command `
               -ArgumentList $TargetAccount, $RestrictedPrinciple -ErrorAction Stop


