# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.


# Fetch ParamConfig.json from the public asset source if it isn't already present.
# The one-click Deploy-to-Azure package omits ParamConfig.json (it holds test-account
# credentials and the GitHub Release asset is public), so the VM retrieves it here at
# deploy time. deploy.ps1 bakes it into its private package, so this fetch is skipped.
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'Test-account passwords are read from ParamConfig.json -- well-known, throwaway credentials for an isolated, disposable test environment -- and must be converted to a SecureString to pass to New-ADUser/Set-ADAccountPassword. The source is already plaintext config, so there is no encrypted string to convert from; this is provisioning of test accounts, not handling of production secrets.')]
param(
    $workingDir = $PSScriptRoot,
    $protocolConfigFile = "$workingDir\Config.json",
    $parameterConfigFile = "$workingDir\ParamConfig.json",
    $paramConfigSourceUrl = "https://ptsresources-czfwdxa0fdbychcp.b01.azurefd.net/configs/ParamConfig.json",
    [switch]$NoTranscript
)

# Ensure net.exe stderr (e.g. invalid usernames with '@') does not become a
# terminating error when the caller sets $ErrorActionPreference = 'Stop'.
$ErrorActionPreference = 'Continue'

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;"

Push-Location $workingDir
#----------------------------------------------------------------------------
# if working dir is not exists. it will use scripts path as working path
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    $workingDir = $scriptPath
}

if(!(Test-Path "$protocolConfigFile"))
{
    $protocolConfigFile = "$workingDir\Config.json"
    if(!(Test-Path "$protocolConfigFile")) 
    {
        Write-Error.ps1 "No Config file found."
        return $false
    }
}

if(!(Test-Path "$parameterConfigFile"))
{
    $parameterConfigFile = "$workingDir\ParamConfig.json"
    if(!(Test-Path "$parameterConfigFile"))
    {
        # Not baked into the package (public one-click path): fetch it now.
        if ($paramConfigSourceUrl) {
            .\Write-Info.ps1 "ParamConfig.json not found locally; downloading from source..."
            . "$scriptPath\Get-RemoteFile.ps1"
            # BITS-based download (follows 307/308 redirects, unlike Invoke-WebRequest on PS 5.1).
            if (-not (Get-RemoteFile -Url $paramConfigSourceUrl -OutputPath $parameterConfigFile)) {
                .\Write-Error.ps1 "Failed to download ParamConfig.json from '$paramConfigSourceUrl'."
            }
        }
        if(!(Test-Path "$parameterConfigFile"))
        {
            .\Write-Error.ps1 "No ParamConfig.json found."
            return $false
        }
    }
}
#----------------------------------------------------------------------------
# Start logging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
$transcriptStarted = $false
if (-not $NoTranscript) {
    Start-Transcript -Path "$logFile" -Append -Force
    $transcriptStarted = $true
}

function StartService($serviceName)
{
    $service = Get-Service -Name $serviceName
    $retryTimes = 0
    while($service.Status -ne "Running" -and $retryTimes -lt 6)
    {
        .\Write-Info.ps1 "Start $serviceName service."
        Start-Service -InputObj $service -ErrorAction Continue
        Start-Sleep 10
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

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
$config = $null
try {
    $config = Get-Content -Path $protocolConfigFile -Raw | ConvertFrom-Json
}
catch {
    Write-Error.ps1 "Failed to parse config file: $_"
    return $false
}

$params = $null
try {
    $params = Get-Content -Path $parameterConfigFile -Raw | ConvertFrom-Json
}
catch {
    Write-Error.ps1 "Failed to parse parameter config file: $_"
    return $false
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$password = $config.Core.Password
if([System.String]::IsNullOrEmpty($password))
{
    .\Write-Error.ps1 "Config.json Core.Password is empty -- cannot set Guest password."
    return $false
}

$azgroups = $params.Parameters.Groups
$users =  $params.Parameters.Users
$isDomainEnv = (Get-CimInstance Win32_ComputerSystem).PartOfDomain

# One-click Deploy-to-Azure path: the operator picks an arbitrary admin password and
# the test framework logs in ALL accounts with PasswordForAllUsers (= that password).
# ParamConfig.json bakes a fixed per-account password, so secondary accounts (e.g.
# nonadmin) would fail logon. When Config.json opts in (Core.UsePasswordForAllUsers),
# create every account with Core.Password so all logons are consistent. Both the
# domain (dsadd) and local (net.exe) creation loops below read $user.Password, so
# overwriting it here covers both. Pipeline/CLI leave the flag unset -> unchanged.
if ("$($config.Core.UsePasswordForAllUsers)" -eq "true") {
    .\Write-Info.ps1 "UsePasswordForAllUsers=true: creating all test accounts with the unified admin password."
    foreach ($u in $users.User) {
        # Accounts flagged KeepPassword must retain their ParamConfig password. The
        # special-character Kerberos accounts (Auth SpecialUserNames) are logged in by
        # the Auth ptfconfig with their own SpecialUserPasswords (e.g. 'Password01^'),
        # NOT PasswordForAllUsers -- unifying them would break KerbAuth_UserName_With_
        # Special_Characters. Flag them KeepPassword=true in ParamConfig.json.
        if ("$($u.KeepPassword)" -eq "true") {
            .\Write-Info.ps1 "Keeping ParamConfig password for '$($u.Username)' (KeepPassword=true)."
            continue
        }
        $u.Password = $config.Core.Password
    }
}

#----------------------------------------------------------------------------
# Start required services
#----------------------------------------------------------------------------
if($isDomainEnv -eq $true)
{
    .\Write-Info.ps1 "Check and start Active Directory Domain Services"
    StartService "NTDS"

    .\Write-Info.ps1 "Check and start Active Directory Web Services"
    StartService "ADWS"
}
else
{
    .\Write-Info.ps1 "Workgroup env, skip checking Active Directory Services"
}

#----------------------------------------------------------------------------
# Create CBAC ENV
#----------------------------------------------------------------------------
$domainName = (Get-CimInstance Win32_ComputerSystem).Domain

# Retry to wait until the ADWS can respond to PowerShell commands correctly
if($isDomainEnv -eq $true)
{
    $retryTimes = 0
    $domain = $null
    while ($retryTimes -lt 30) {
        $domain = Get-ADDomain $domainName
        if ($null -ne $domain) {
            break;
        }
        else {
            Start-Sleep 10
            $retryTimes += 1
        }
    }

    if ($null -eq $domain) {
        .\Write-Error.ps1 "Failed to get correct responses from the ADWS service after strating it for 5 minutes."
    }
}
else
{
    .\Write-Info.ps1 "Workgroup env, skip checking ADWS Services"
}

#----------------------------------------------------------------------------
# Create and active test accounts
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Create and active test accounts"
if ($isDomainEnv -eq $true)
{
    $domainAdmin = $config.Core.Username

    $adminDN = dsquery user -name $domainAdmin

    # Cache of all domain users keyed by SamAccountName, shared across the user loop
    # below. Filled lazily (one Get-ADUser -Filter * for the whole run instead of one
    # per attempt) and invalidated on a failed attempt so the retry sees any account a
    # half-failed create actually left behind.
    $domainUserCache = $null

    foreach($group in $azgroups)
    {        
        .\Write-Info.ps1 "Create group: $($group.Group.GroupName)"
        $azGroupDN = $group.Group.GroupName
        $existingGroup = Get-ADGroup -Filter * -ErrorAction Stop |
            Where-Object { $_.SamAccountName -eq $azGroupDN } |
            Select-Object -First 1
        if ($null -eq $existingGroup) {
            New-ADGroup -Name $azGroupDN -GroupScope Global -GroupCategory Security `
                -ErrorAction Stop
        } else {
            .\Write-Info.ps1 "AD group $azGroupDN already exists."
        }
    }

    foreach($user in $users.User)
    {
        .\Write-Info.ps1 "Create user: $($user.Username)"
        # -Name / -SamAccountName / -AccountPassword pass values as literal parameters, so
        # usernames with special characters (the Kerberos SpecialUserNames such as
        # '$I1Q73_VjdSJ!vGn7Q' or '9L7!MNZ%}wq4iZ') survive the JSON -> PowerShell path.
        # Wrap the whole create/repair in a retry loop: a freshly-promoted Server 2025 DC's
        # ADWS intermittently throws "server is not operational" for minutes, and without a
        # retry the account is silently dropped (WARN), later surfacing as a missing-account
        # Kerberos failure. Look accounts up by enumerating and matching CLIENT-SIDE (never a
        # server-side -Identity / -LDAPFilter on the raw name, which chokes on '{'/'(' with
        # "search filter cannot be recognized" and on some chars ADWS mis-parses under load).
        $domainDN  = "DC=" + $domainName.Replace(".", ",DC=")
        $securePwd = ConvertTo-SecureString -String $user.Password -AsPlainText -Force
        $userOk = $false
        for ($try = 1; $try -le 5 -and -not $userOk; $try++) {
            try {
                if ($null -eq $domainUserCache) {
                    $domainUserCache = @{}
                    Get-ADUser -Filter * -ErrorAction Stop |
                        ForEach-Object { $domainUserCache[$_.SamAccountName] = $_ }
                }
                $existingUser = $domainUserCache[$user.Username]

                if ($null -eq $existingUser) {
                    # Set the full Kerberos enc-type set explicitly. On a Server 2025 KDC, RC4
                    # is restricted and an account created without -KerberosEncryptionType may
                    # not advertise the etype the client offers -> KDC_ERR_ETYPE_NOTSUPP.
                    # Mirrors Create-CbacObjectsInDC.ps1; no-op on 2022/below.
                    $existingUser = New-ADUser -Name $user.Username -SamAccountName $user.Username -DisplayName $user.Username `
                        -AccountPassword $securePwd -Enabled $true -PasswordNeverExpires $true `
                        -CannotChangePassword $true -Path "CN=Users,$domainDN" `
                        -KerberosEncryptionType DES,RC4,AES128,AES256 -PassThru -ErrorAction Stop
                    $domainUserCache[$user.Username] = $existingUser
                } else {
                    # Repair via the objectGUID, and NEVER through Set-ADAccountPassword /
                    # Enable-ADAccount: even with a GUID identity, ADWS's account-management
                    # operations re-reference the object by DN in an unescaped server-side
                    # LDAP filter, so a CN containing '('/')' fails with "search filter
                    # cannot be recognized" / "directory service is unavailable". Set-ADUser
                    # (plain WS-Transfer Put) is safe once resolved by GUID, and the password
                    # reset goes through an ADSI LDAP://<GUID=...> binding, which bypasses
                    # ADWS entirely. Set enc-types BEFORE resetting the password so the
                    # Kerberos key set is regenerated for all etypes, and enable LAST: a
                    # half-created account may have no password yet, and AD refuses to
                    # enable a passwordless account ("password does not meet the...
                    # requirement of the domain").
                    $userGuid = $existingUser.ObjectGUID
                    Set-ADUser -Identity $userGuid -KerberosEncryptionType DES,RC4,AES128,AES256 -ErrorAction Stop
                    $adsiUser = [ADSI]"LDAP://<GUID=$userGuid>"
                    $adsiUser.psbase.Invoke('SetPassword', $user.Password)
                    Set-ADUser -Identity $userGuid -Enabled $true -ErrorAction Stop
                }

                if ($null -ne $user.Group) {
                    # Reference the member by objectGUID, never by name or DN string (see above).
                    Add-ADGroupMember -Identity $user.Group -Members $existingUser.ObjectGUID -ErrorAction Stop
                }
                $userOk = $true
            } catch {
                .\Write-Info.ps1 "  User '$($user.Username)' attempt $try/5 failed in $($_.CategoryInfo.Activity): $($_.Exception.Message)" -ForegroundColor DarkGray
                $domainUserCache = $null
                if ($try -lt 5) { Start-Sleep 20 }
            }
        }
        if ($userOk) { .\Write-Info.ps1 "[OK] User '$($user.Username)' ready" -ForegroundColor Green }
        else { .\Write-Info.ps1 "[WARN] Failed to create/repair domain user '$($user.Username)' after retries -- dependent tests will fail." -ForegroundColor Yellow }
    }

    .\Write-Info.ps1 "Enable Guest account"
    try {
        $guestPassword = ConvertTo-SecureString -String $password -AsPlainText -Force
        $guestUser = Get-ADUser -Identity 'Guest' -ErrorAction Stop
        Set-ADAccountPassword -Identity $guestUser.ObjectGUID -Reset `
            -NewPassword $guestPassword -ErrorAction Stop
        Enable-ADAccount -Identity $guestUser.ObjectGUID -ErrorAction Stop
        Set-ADUser -Identity $guestUser.ObjectGUID -PasswordNeverExpires $true `
            -CannotChangePassword $true -ErrorAction Stop
        .\Write-Info.ps1 "[OK] Domain Guest account enabled" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[WARN] Failed to configure domain Guest account: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    .\Write-Info.ps1 "Setting password never expires"
    dsquery user -samid * | dsmod user -pwdneverexpires yes -mustchpwd no 2>&1 | .\Write-Info.ps1
    dsquery user -samid * | dsget user -samid -pwdneverexpires 2>&1 | .\Write-Info.ps1
}
else
{
    foreach($group in $azgroups)
    {
        .\Write-Info.ps1 "Create group: $($group.Group.GroupName)"
        $azGroupDN = $group.Group.GroupName
        try {
            if (-not (Get-LocalGroup -Name $azGroupDN -ErrorAction SilentlyContinue)) {
                New-LocalGroup -Name $azGroupDN -ErrorAction Stop | Out-Null
            }
            .\Write-Info.ps1 "[OK] Local group '$azGroupDN' ready" -ForegroundColor Green
        } catch {
            .\Write-Info.ps1 "[WARN] Failed to create group '$azGroupDN': $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    foreach($user in $users.User)
    {
        .\Write-Info.ps1 "Create user account: $($user.Username)"
        try {
            $securePassword = ConvertTo-SecureString -String $user.Password -AsPlainText -Force
            $localUser = Get-LocalUser -Name $user.Username -ErrorAction SilentlyContinue
            if ($null -eq $localUser) {
                $localUser = New-LocalUser -Name $user.Username -Password $securePassword `
                    -AccountNeverExpires -PasswordNeverExpires -UserMayNotChangePassword `
                    -ErrorAction Stop
            } else {
                Set-LocalUser -InputObject $localUser -Password $securePassword `
                    -PasswordNeverExpires $true -UserMayChangePassword $false -ErrorAction Stop
                Enable-LocalUser -InputObject $localUser -ErrorAction Stop
            }

            if($null -ne $user.Group)
            {
                $isMember = @(Get-LocalGroupMember -Name $user.Group -ErrorAction Stop |
                    Where-Object { $_.SID -eq $localUser.SID }).Count -gt 0
                if (-not $isMember) {
                    Add-LocalGroupMember -Name $user.Group -Member $localUser -ErrorAction Stop
                }
            }
            .\Write-Info.ps1 "[OK] Local user '$($user.Username)' ready" -ForegroundColor Green
        } catch {
            .\Write-Info.ps1 "[WARN] Failed to create user '$($user.Username)': $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    .\Write-Info.ps1 "Enable Guest account"
    try {
        $guestPassword = ConvertTo-SecureString -String $password -AsPlainText -Force
        $guestUser = Get-LocalUser -Name 'Guest' -ErrorAction Stop
        Set-LocalUser -InputObject $guestUser -Password $guestPassword `
            -PasswordNeverExpires $true -UserMayChangePassword $false -ErrorAction Stop
        Enable-LocalUser -InputObject $guestUser -ErrorAction Stop
        .\Write-Info.ps1 "[OK] Guest account enabled" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[WARN] Failed to configure Guest account: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    .\Write-Info.ps1 "Setting password never expires"
    Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:ComputerName'" | ForEach-Object { Set-CimInstance -InputObject $_ -Property @{PasswordExpires = $false} }
    Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:ComputerName'" | Format-Table Caption,PasswordExpires   
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Completed create test accounts."
Pop-Location
if ($transcriptStarted) {
    Stop-Transcript
}
return $true