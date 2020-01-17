#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################
param($workingDir = "$env:SystemDrive\Temp")
#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"
Push-Location $workingDir
#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Common Functions
#----------------------------------------------------------------------------
function StartService($serviceName)
{
    $service = Get-Service -Name $serviceName
    $retryTimes = 0
    while($service.Status -ne "Running" -and $retryTimes -lt 6)
    {
        .\Write-Info.ps1 "Start $serviceName service."
        Start-Service -InputObj $service -ErrorAction Continue
        Sleep 10
        $retryTimes++ 
        $service = Get-Service -Name $serviceName
    }

    if($retryTimes -ge 6)
    {
        Write-Error.ps1 "Start $serviceName service failed within 1 minute."
    }
    else
    {
        .\Write-Info.ps1 "Service $serviceName is Running."
    }
}

Function CreateADGroup([String]$groupName, [String]$parentGroupName)
{
    .\Write-Info.ps1 "Create ADGroup $groupName from AD if NOT exist."
    $adGroup = Get-ADGroup -Filter * | where {$_.Name -eq $groupName}
    if($adGroup -eq $null)
    {
        .\Write-Info.ps1 "ADGroup $groupName does not exist in AD, create a new one." 
        New-ADGroup -Name $groupName -GroupScope Global -GroupCategory Security
    }
    else
    {
        .\Write-Info.ps1 "Group $groupName already exist in AD."
    }

    if([System.String]::IsNullOrEmpty($parentGroupName))
    {
        .\Write-Info.ps1 "Group $groupName is primary group. ADGroup creation completed."      
    }
    else
    {
        $parentGroup = Get-ADGroup -Filter * | where {$_.Name -eq $parentGroupName}
        if($parentGroup -eq $null)
        {
            .\Write-Info.ps1 "Parent group $parentGroupName does not exist, create a new one." -ForegroundColor yellow
            New-ADGroup -Name $parentGroupName -GroupScope Global -GroupCategory Security
        }

        .\Write-Info.ps1 "Add ADGroup $groupName to its parent group: $parentGroupName"
        Add-ADGroupMember -Identity $parentGroupName -Members $groupName
    }
}

Function CreateClaimTypes([String]$DisplayName, [String[]]$Values)
{
    $adClaimType = Get-ADClaimType -Filter * | Where {$_.DisplayName -eq "$DisplayName"}
    if($adClaimType -ne $null)
    {
        .\Write-Info.ps1 "Claim Type $DisplayName already exists, don't need to create it." 
        return
    }

    $suggestedValues = @()
    foreach ($value in $Values)
    {
        .\Write-Info.ps1 "Add suggested value: $value"
        $suggestedValues += New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value, $value,"");  
    }

    .\Write-Info.ps1 "Create Claim Type $displayName"
    New-ADClaimType -AppliesToClasses @("User", "Computer") -DisplayName $DisplayName -SourceAttribute $DisplayName -SuggestedValues $suggestedValues -ID "ad://ext/$DisplayName"

    .\Write-Info.ps1 "Claim Type $displayName has been created successfully." -ForegroundColor Green
}

Function CreateADUser([String]$Username, [String]$Password, [String]$Group, [String]$CountryCode, [String]$Department, [String]$Company)
{
    # Variables
    $secPassword = ConvertTo-SecureString $password -AsPlainText -Force

    # Create AD User
    .\Write-Info.ps1 "Create ADUser $Username if NOT exist in AD"
    $adUser = Get-ADUser  -Filter * | Where {$_.Name -eq $Username}
    if($adUser -eq $null)
    {
        .\Write-Info.ps1 "ADUser $Username does not exist in AD, create a new one."
        New-ADUser -Name $Username -AccountPassword $secPassword -CannotChangePassword $true -DisplayName $Username  -Enabled $true -PasswordNeverExpires $true -KerberosEncryptionType DES,RC4,AES128,AES256
        $adUser = Get-ADUser  -Filter * | Where {$_.Name -eq $Username}
    }
    else
    {
        .\Write-Info.ps1 "ADUser $Username already exist in AD."
    }

    # Add user to group
    if(![System.String]::IsNullOrEmpty($Group))
    {
        $adGroup = Get-ADGroup -Filter * | where {$_.Name -eq $Group}
        if($adGroup -eq $null)
        {
            CreateADGroup -groupName $Group
            $adGroup = Get-ADGroup -Filter * | where {$_.Name -eq $Group}
        }
        .\Write-Info.ps1 "Add ADUser $Username to group $Group."
        Add-ADGroupMember -Identity $adGroup -Members $aduser.DistinguishedName
    }

    # Set country Code
    if(![System.String]::IsNullOrEmpty($CountryCode))
    {
        .\Write-Info.ps1 "Set CountryCode to $CountryCode"
        Set-ADUser $Username -replace @{Countrycode=$CountryCode} 
    }

    # Set department
    if(![System.String]::IsNullOrEmpty($Department))
    {
        .\Write-Info.ps1 "Set Department to $Department"
        Set-ADUser $Username -Department $Department
    }

    # Set company
    if(![System.String]::IsNullOrEmpty($Company))
    {
        .\Write-Info.ps1 "Set Company to $Company"
        Set-ADUser $Username -Company $Company
    }
}

Function CreateResourceProperty([String]$DisplayName, [String]$ValueType, [String[]]$Values)
{
    $suggestedValues = @()
    foreach ($value in $Values)
    {
        .\Write-Info.ps1 "Add suggested value: $value"
        $suggestedValues += New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value, $value,"");  
    }

    $adResourceProperty = Get-ADResourceProperty -Filter * | Where {$_.DisplayName -eq "$DisplayName"}
    if($adResourceProperty -ne $null)
    {
        .\Write-Info.ps1 "Resource Property $DisplayName already exists."
        .\Write-Info.ps1 "Set Resource Property with suggested values."
        Set-ADResourceProperty $adResourceProperty -SuggestedValues $suggestedValues
    }
    else
    {                
        .\Write-Info.ps1 "Create ADResourceProperty $displayName"
        New-ADResourceProperty -DisplayName $displayName -IsSecured $true -ResourcePropertyValueType $valuetype -SuggestedValues $suggestedValues -ID ($displayName.Trim() + "_MS")
    }

    .\Write-Info.ps1 "Add ADResourcePropertyListMember $displayName"
    Add-ADResourcePropertyListMember "Global Resource Property List" -Members $displayName            
              
    .\Write-Info.ps1 "ADResourceProperty $displayName has been created successfully." -ForegroundColor Green
}

Function CreateADCentralAccessRule([String]$Name, [String]$RuleItem)
{
    $acl = "O:SYG:SYD:AR(A;;FA;;;OW)(A;;FA;;;BA)(A;;FA;;;SY)"+ $RuleItem

    $adCentralAccessRule = Get-ADCentralAccessRule -Filter * | Where {$_.Name -eq "$Name"}
    if($adCentralAccessRule -ne $null)
    {
        .\Write-Info.ps1 "ADCentralAccessRule $Name already exists."

        .\Write-Info.ps1 "Set ADCentralAccessRule with rule item $RuleItem"
        Set-ADCentralAccessRule $adCentralAccessRule -CurrentAcl $acl
    }
    else
    {
        .\Write-Info.ps1 "Create ADCentralAccessRule $Name"
        .\Write-Info.ps1 "Adding rule item $RuleItem"
        New-ADCentralAccessRule -Name $Name -CurrentAcl $acl
    }
}

Function CreateADCentralAccessPolicy([String]$Name, [String[]]$Rules)
{
    $adCentralAccessPolicy = Get-ADCentralAccessPolicy -Filter * | Where {$_.Name -eq "$Name"}
    if($adCentralAccessPolicy -ne $null)
    {
        .\Write-Info.ps1 "ADCentralAccessPolicy $Name already exists."

        .\Write-Info.ps1 "Set ADCentralAccessPolicy with rules $Rules"
        Add-ADCentralAccessPolicyMember $adCentralAccessPolicy -Members $Rules
    }
    else
    {
        .\Write-Info.ps1 "Create ADCentralAccessPolicy $Name"
        New-ADCentralAccessPolicy -Name $Name

        .\Write-Info.ps1 "Adding rules $Rules"
        Sleep 5
        $adCentralAccessPolicy = Get-ADCentralAccessPolicy -Filter * | Where {$_.Name -eq "$Name"}
        Add-ADCentralAccessPolicyMember $adCentralAccessPolicy -Members $Rules
    }
}

Function EnableCbacAndArmor()
{
    .\Write-Info.ps1 "Add KDC Registry Key items:"
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC /f
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /f
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v CbacAndArmorLevel /t REG_DWORD /d 2 /f
    .\Write-Info.ps1 "Check KDC Registry Key items:"
    get-childitem "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\*"

    .\Write-Info.ps1 "Add kerberos Registry Key items:"
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos /f
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /f
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f
    CMD /C REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v Supportedencryptiontypes /t REG_DWORD /d 0x7fffffff /f
    .\Write-Info.ps1 "Check kerberos Registry Key items:"
    get-childitem "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\*"
}

#----------------------------------------------------------------------------
# Start required services
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Check and start Active Directory Domain Services"
StartService "NTDS"

.\Write-Info.ps1 "Check and start Active Directory Web Services"
StartService "ADWS"


#----------------------------------------------------------------------------
# Create CBAC ENV
#----------------------------------------------------------------------------
$domainName = (Get-WmiObject win32_computersystem).Domain

# Retry to wait until the ADWS can respond to PowerShell commands correctly
$retryTimes = 0
$domain = $null
while ($retryTimes -lt 30) {
    $domain = Get-ADDomain $domainName
    if ($domain -ne $null) {
        break;
    }
    else {
        Start-Sleep 10
        $retryTimes += 1
    }
}

if ($domain -eq $null) {
    .\Write-Error.ps1 "Failed to get correct responses from the ADWS service after strating it for 5 minutes."
}

.\Write-Info.ps1 "Create ADGroups"
CreateADGroup -groupName "Payroll"
CreateADGroup -groupName "Payroll Admins" -parentGroupName "Payroll"
CreateADGroup -groupName "IT"
CreateADGroup -groupName "IT Admins" -parentGroupName "IT"

.\Write-Info.ps1 "Create Claim Types"
CreateClaimTypes -DisplayName "Department" -Values "IT","Payroll"
CreateClaimTypes -DisplayName "Company" -Values $domain.name,"Kerb"
CreateClaimTypes -DisplayName "CountryCode" -Values "840","392","156"

.\Write-Info.ps1 "Create ADUsers"
CreateADUser -Username "claimuser" -Password "Password01!" -Company $domain.name
CreateADUser -Username "noclaimuser" -Password "Password01!"

CreateADUser -Username "ITadmin01" -Password "Password01!" -Group "IT Admins" -CountryCode "156" -Department "IT"
CreateADUser -Username "ITmember01" -Password "Password01!" -Group "IT" -CountryCode "392" -Department "IT"

CreateADUser -Username "Payrolladmin01" -Password "Password01!" -Group "Payroll" -CountryCode "840" -Department "Payroll"
CreateADUser -Username "Payrollmember01" -Password "Password01!" -Group "Payroll" -CountryCode "156" -Department "Payroll" -Company $domain.name
CreateADUser -Username "Payrollmember02" -Password "Password01!" -Group "Payroll" -CountryCode "840" -Department "Payroll"
CreateADUser -Username "Payrollmember03" -Password "Password01!" -Group "Payroll" -CountryCode "392" -Department "Payroll"

.\Write-Info.ps1 "Create Resource Properties"
CreateResourceProperty -DisplayName "Company" -ValueType "MS-DS-SinglevaluedChoice" -Values $domain.name,"kerb"
CreateResourceProperty -DisplayName "SecurityLevel" -ValueType "MS-DS-SinglevaluedChoice" -Values "HBI","MBI","LBI"
CreateResourceProperty -DisplayName "Department" -ValueType "MS-DS-SinglevaluedChoice" -Values "Payroll","IT"
CreateResourceProperty -DisplayName "CountryCode" -ValueType "MS-DS-SinglevaluedChoice" -Values "156","840","392"

.\Write-Info.ps1 "Create ADCentralAccessRule"
$PETRule = '(XA;;FA;;;AU;(@USER.ad://ext/Company == "'+$domain.name+'"))'
CreateADCentralAccessRule -Name "PET-AccessRule" -RuleItem $PETRule 
CreateADCentralAccessRule -Name "CountryCodeEquals156Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode == 156))'
CreateADCentralAccessRule -Name "CountryCodeNotEquals156Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode != 156))'
CreateADCentralAccessRule -Name "CountryCodeLessThan392Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode < 392))'
CreateADCentralAccessRule -Name "CountryCodeLessThanOrEquals392Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode <= 392))'
CreateADCentralAccessRule -Name "CountryCodeGreaterThan392Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode > 392))'
CreateADCentralAccessRule -Name "CountryCodeGreaterThanOrEquals392Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode >= 392))'
CreateADCentralAccessRule -Name "CountryCodeAnyOf156Or840Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode Any_of {156,840}))'
CreateADCentralAccessRule -Name "CountryCodeNotAnyOf156Or840Rule" -RuleItem '(XA;;FA;;;AU;(@USER.ad://ext/CountryCode Not_Any_of {156,840}))'
CreateADCentralAccessRule -Name "CountryCodeEquals156AndITDepartmentRule" -RuleItem '(XA;;FA;;;AU;((@USER.ad://ext/CountryCode == 156) && (@USER.ad://ext/Department == "IT")))'
CreateADCentralAccessRule -Name "CountryCodeEquals156OrITDepartmentRule" -RuleItem '(XA;;FA;;;AU;((@USER.ad://ext/CountryCode == 156) || (@USER.ad://ext/Department == "IT")))'

.\Write-Info.ps1 "Create ADCentralAccessPolicy"
CreateADCentralAccessPolicy -Name "PET-AccessPolicy" -Rules "PET-AccessRule"
CreateADCentralAccessPolicy -Name "CountryCodeEquals156Policy" -Rules "CountryCodeEquals156Rule"
CreateADCentralAccessPolicy -Name "CountryCodeNotEquals156Policy" -Rules "CountryCodeNotEquals156Rule"
CreateADCentralAccessPolicy -Name "CountryCodeLessThan392Policy" -Rules "CountryCodeLessThan392Rule"
CreateADCentralAccessPolicy -Name "CountryCodeLessThanOrEquals392Policy" -Rules "CountryCodeLessThanOrEquals392Rule"
CreateADCentralAccessPolicy -Name "CountryCodeGreaterThan392Policy" -Rules "CountryCodeGreaterThan392Rule"
CreateADCentralAccessPolicy -Name "CountryCodeGreaterThanOrEquals392Policy" -Rules "CountryCodeGreaterThanOrEquals392Rule"
CreateADCentralAccessPolicy -Name "CountryCodeAnyOf156Or840Policy" -Rules "CountryCodeAnyOf156Or840Rule"
CreateADCentralAccessPolicy -Name "CountryCodeNotAnyOf156Or840Policy" -Rules "CountryCodeNotAnyOf156Or840Rule"
CreateADCentralAccessPolicy -Name "CountryCodeEquals156AndITDepartmentPolicy" -Rules "CountryCodeEquals156AndITDepartmentRule"
CreateADCentralAccessPolicy -Name "CountryCodeEquals156OrITDepartmentPolicy" -Rules "CountryCodeEquals156OrITDepartmentRule"

.\Write-Info.ps1 "Enable Cbac And Armor from registry key"
EnableCbacAndArmor

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Pop-Location
Stop-Transcript
exit 0