#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Config-DC01.ps1
## Purpose:        Configure local realm DC for MS-AZOD test suite.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012 +
##
##############################################################################
Param
(
    [int]$Step = 1
)

Function Phase1
{
	$endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\MS-AZOD\OD-Endpoint"
    $azodTestSuites = Get-ChildItem -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites\MS-AZOD-OD-*'
    $azodTestSuite = $azodTestSuites[0]
    $version = $azodTestSuite.Name.Substring(80, $azodTestSuite.Name.Length-80)

    $dataFile = "$endPointPath\$version\scripts\Config.xml"    
    $logPath = $env:SystemDrive
    $logFile = $MyInvocation.MyCommand.Name + ".log"
    $domainName = "contoso.com"
    $domainAdmin 	= "administrator"
    $domainAdminPwd 	= "Password01!"

    if(Test-Path -Path $dataFile)
    {
        try
        {
	        [xml]$configFile = Get-Content -Path $dataFile
	        $logPath	= $configFile.Parameters.LogPath
	        $logFile	= $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"

	        $domainName 	= $configFile.Parameters.LocalRealm.DomainName
            $domainAdmin 	= $configFile.Parameters.LocalRealm.DomainAdministrator.UserName
            $domainAdminPwd 	= $configFile.Parameters.LocalRealm.DomainAdministrator.Password
        }
        catch
        {
            .\Write-Info.ps1 "Failed to read data file $dataFile. Please check the file content."
            return
        }
    }
    else
    {
	    .\Write-Info.ps1 "$dataFile not found.  Will keep the default setting of all the test context info..."
    }

    # Turn off firewall
    .\Write-Info.ps1 "Turning off the firewall"
    Turnoff-FireWall       

    # Autologon config
    Autologon -Domain $domainName -Username $domainAdmin -Password $domainAdminPwd
    
    # Promote DC
    .\Write-Info.ps1 "Promoting this computer to DC" -ForegroundColor Yellow
    PromptDC -DomainName $domainName -AdminPwd  $domainAdminPwd

    sleep 15
}

#-----------------------------------------------------------------------------------------------
# Set Autologon user, domain and password.
#-----------------------------------------------------------------------------------------------
Function Autologon(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]    
    [string]$Domain,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()] 
    [string]$Username,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()] 
    [string]$Password
)
{
    .\Write-Info.ps1 "set autologon account"    
    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v AutoAdminLogon /t REG_SZ /d 1 /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v AutoAdminLogon /t REG_SZ /d 1 /f 2>&1 | Write-Host

    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultDomainName /t REG_SZ /d  $Domain /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultDomainName /t REG_SZ /d  $Domain /f 2>&1 | Write-Host

    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultUserName /t REG_SZ /d $Username /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultUserName /t REG_SZ /d $Username /f 2>&1 | Write-Host

    .\Write-Info.ps1 "cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultPassword /t REG_SZ /d $Password /f`n"
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultPassword /t REG_SZ /d $Password /f 2>&1 | Write-Host
}
#-----------------------------------------------------------------------------------------------
# Turn off windows firewall
#-----------------------------------------------------------------------------------------------
Function Turnoff-FireWall()
{
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Host
}

#-----------------------------------------------------------------------------
# Function: PromoteDomainController
# Usage   : Install ADDS feature on the server and promote it to DC.
# Params  : [string]$DomainName: The name of the domain.
#           [string]$AdminPwd  : The password of the Administrator.
# Remark  : A reboot is needed after promoting to DC.
#-----------------------------------------------------------------------------
Function PromptDC(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$DomainName, 
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$AdminPwd)
{
    
    Try
    {
        # Install ADDS
        Install-WindowsFeature -Name AD-Domain-Services -IncludeManagementTools -ErrorAction Stop
        Import-Module ADDSDeployment -ErrorAction Stop

        # Promote to DC
        Install-ADDSForest -DomainName $DomainName -InstallDns `
            -SafeModeAdministratorPassword (ConvertTo-SecureString $adminPwd -AsPlainText -Force) `
            -NoRebootOnCompletion -ErrorAction Stop -Force           
     
    }
    catch
    {
        throw "Unable to promote DC. Error happened: " + $_.Exception.Message
    }
}

#-----------------------------------------------------------------------------
# Function: RestartAndResume
# Usage   : Restart the computer and run the specified script.
#-----------------------------------------------------------------------------
Function RestartAndResume(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [String]$ScriptPath,
        
        [Parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [String]$PhaseIndicator,

        [Parameter(Mandatory=$false)]
        [String]$ArgumentList,

        [Parameter(Mandatory=$false)]
        [bool]$AutoRestart = $false)
   
{       

    # Only absolute path is accepted, because relative path may cause trouble after rebooting.
    if([System.IO.Path]::IsPathRooted($ScriptPath) -eq $False)
    {
        throw "Argument ScriptPath must be absolute path"
    }

    $private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
    $private:regKeyName = "TKFRSAR"

    # If the key has already been set, remove it
    if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
    {
	    Remove-ItemProperty -Path $regRunPath -Name $regKeyName
    }

    try
    {
        Set-ItemProperty -Path $regRunPath -Name $regKeyName `
                            -Value "cmd /c powershell $ScriptPath $PhaseIndicator $ArgumentList" `
                            -Force -ErrorAction Stop
    }
    catch
    {
        throw "Unable to restart. Error happened: $_.Exception.Message"
    }

    .\Write-Info.ps1 "The computer is going to restart..." -ForegroundColor Yellow
    if($AutoRestart -eq $false) 
    { 
        # Waiting for key stroke
        Pause 
    } 

    # Restart
    Restart-Computer  
}
Function Phase2
{   
    $endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\MS-AZOD\OD-Endpoint"
    $azodTestSuites = Get-ChildItem -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites\MS-AZOD-OD-*'
    $azodTestSuite = $azodTestSuites[0]
    $version = $azodTestSuite.Name.Substring(80, $azodTestSuite.Name.Length-80)

    $dataFile = "$endPointPath\$version\scripts\Config.xml"    
    $logPath = $env:SystemDrive
    $logFile = $MyInvocation.MyCommand.Name + ".log"

    if(Test-Path -Path $dataFile)
    {
	    [xml]$configFile = Get-Content -Path $dataFile
        $logPath = $configFile.Parameters.LogPath
	    $logFile = $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"

        $dcName = $configFile.Parameters.LocalRealm.DC.Name
        $dcIP = $configFile.Parameters.LocalRealm.DC.IP
        $dcAdmin = $configFile.Parameters.LocalRealm.DC.Username
        $dcAdminPwd = $configFile.Parameters.LocalRealm.DC.Password

        $clientName = $configFile.Parameters.LocalRealm.Client.Name
        $clientIP = $configFile.Parameters.LocalRealm.Client.IP
        $clientAdmin = $configFile.Parameters.LocalRealm.Client.Username
        $clientAdminPwd = $configFile.Parameters.LocalRealm.Client.Password

        # Create groups in AD
        .\Write-Info.ps1 "Creating groups in AD"
        foreach ($group in $configFile.Parameters.LocalRealm.Groups.group)
        {
            $name = $group.Name
            $parent = $group.Parent
            
            .\Write-Info.ps1 "Creating group $name in AD" 

            try
            {
                Get-ADGroup -Identity $name
                .\Write-Info.ps1 "Group $name exists in AD. Will Remove the group and create a new one." -ForegroundColor yellow
                Remove-ADGroup -Identity $name -Confirm:$false
            }
            catch
            {}
          
            New-ADGroup -Name $name -GroupScope Global -GroupCategory Security
            if($parent -ne $null)
            {
             
                try
                {
                    $parentGroup = Get-ADGroup -Identity $parent -ErrorAction Ignore
                    if( $parentGroup -eq $null)
                    {
                        New-ADGroup -Name $parent -GroupScope Global -GroupCategory Security
                    }

                    Add-ADGroupMember -Identity $parent -Members $name
                }
                catch
                {
                    .\Write-Info.ps1 "Add group $name to group $parent failed. Error happened: $_.Exception.Message" -BackgroundColor Red
                }
            }
             
            
        }

        # Create users in AD
        .\Write-Info.ps1 "Creating users in AD."
    
        foreach ($user in $configFile.Parameters.LocalRealm.Users.user)
        {
            $name = $user.Username
            $password = $user.Password
            $pwd = ConvertTo-SecureString $password -AsPlainText -Force
            $group = $user.Group
            $countryCode = $user.CountryCode
            $Department =  $user.Department
            $company =  $user.Company
            .\Write-Info.ps1 "Creating user $name in AD" 
            try
            {
                Get-ADUser -Identity $name
                .\Write-Info.ps1 "User $name exists in AD. Will Remove the user and create a new one." -ForegroundColor yellow
                Remove-ADUser -Identity $name -Confirm:$false 
            }
            catch
            {}


            try
            {                            
                New-ADUser -Name $name -AccountPassword $pwd -CannotChangePassword $true -DisplayName $name -Enabled $true -PasswordNeverExpires $true -KerberosEncryptionType DES,RC4,AES128,AES256
                $aduser = Get-ADUser -Identity $name

                if($group -ne $Null)
                {
                    Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName
                }

                if($countryCode -ne $Null)
                {
                    Set-ADUser $name -replace @{Countrycode=$Countrycode} 
                }

                if($company -ne $Null)
                {
                    Set-ADUser $name -company $company
                }

                if($department -ne $Null)
                {
                    Set-ADUser $name -department $department
                }
            
	        
            }
            catch
            {
	             .\Write-Info.ps1 "Error occur during create user $name£¡Error happened: $_.Exception.Message" -ForegroundColor Red
            }               

        }

        #-----------------------------------------------------------------------------------------------
        # Create claimtypes in AD
        #-----------------------------------------------------------------------------------------------
        foreach ($claimtype in $configFile.Parameters.LocalRealm.ClaimTypes.ClaimType)
        {
            $displayName = $claimtype.DisplayName
            .\Write-Info.ps1 "Create claim $displayName"
            try
            {
                Get-ADClaimType $displayName
                .\Write-Info.ps1 "Claim Type $displayName already exists, don't need to create it. Will skip the claim type." 
            }
            catch
            {
                $suggestedValues = @()
                try
                {
                    foreach ($value in $claimtype.Values.Value)
                    {
                        $suggestedValues += New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value, $value,"");  
                    }
            
                    New-ADClaimType -AppliesToClasses @("User", "Computer") -DisplayName $displayName -SourceAttribute $displayName -SuggestedValues $suggestedValues -ID "ad://ext/$displayName"
                    Get-ADClaimType $displayName
                    .\Write-Info.ps1 "New Claim Type Created Successfully. DisplayName $displayName" -ForegroundColor Green
                }
                catch
                {
	               .\Write-Info.ps1 "Error occur during create claim type $displayName£¡Error happened: $_.Exception.Message" -ForegroundColor Red
                } 
            }
        }

        #-----------------------------------------------------------------------------------------------
        # Create Resource Property in AD
        #-----------------------------------------------------------------------------------------------
        foreach ($rp in $configFile.Parameters.LocalRealm.ResourceProperties.ResourceProperty)
        {
            $displayName = $rp.DisplayName
            .\Write-Info.ps1 "Create Resource Property $displayName" 
            try
            {
                Get-ADResourceProperty -Identity $displayName
                .\Write-Info.ps1 "Resource Property $displayName already exists, don't need to create it. Will skip the resource property."
            }
            catch
            {
                $valuetype = $rp.ValueType
                $suggestedValues = @()
                try
                {
                    foreach ($value in $rp.Values.Value)
                    {
                        $suggestedValues += New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value, $value,"");  
                    }
               
                    New-ADResourceProperty -DisplayName $displayName -IsSecured $true -ResourcePropertyValueType $valuetype -SuggestedValues $suggestedValues -ID $displayName"_MS" #-ProtectedFromAccidentalDeletion $true 
                    Add-ADResourcePropertyListMember "Global Resource Property List" -Members $displayName            
              
                    .\Write-Info.ps1 "New Resource Property $displayName created Successfully." -ForegroundColor Green
                }
                catch
                {
	                .\Write-Info.ps1 "Error occur during create Resource Property $displayName£¡Error happened: $_.Exception.Message" -ForegroundColor Red
                }  
            }  
        }

        #-----------------------------------------------------------------------------------------------
        # Create CAR in AD
        #-----------------------------------------------------------------------------------------------
        foreach ($rule in $configFile.Parameters.LocalRealm.Rules.Rule)
        {
            $displayName = $rule.Name
            $ruleItem = $rule.RuleItem        

            .\Write-Info.ps1 "Create CAR $displayName"

            try
            {
                Get-ADCentralAccessRule -Identity $displayName
                .\Write-Info.ps1 "CAR $displayName exists in AD, will remove it." -ForegroundColor Yellow
                Remove-ADCentralAccessRule -Identity $displayName -Confirm:$false
            }
            catch
            {}

            if($ruleItem -match "Less than or equals")
            {
                    $ruleItem = $ruleItem -replace "Less than or equals", "<="
            }

            if($ruleItem -match "Less than")
            {
                $ruleItem = $ruleItem -replace "Less than", "<"
            }
            if($ruleItem -match "Greater than or equals")
            {
                $ruleItem = $ruleItem -replace "Greater than or equals", ">="
            }

            if($ruleItem -match "Greater than")
            {
                $ruleItem = $ruleItem -replace "Greater than", ">"
            }

            if($ruleItem -match " And ")
            {
                $ruleItem = $ruleItem -replace " And ", " && "
            }

            $condition= $rule.ResourceCondition   
            $acl = "O:SYG:SYD:AR(A;;FA;;;OW)(A;;FA;;;BA)(A;;FA;;;SY)"+ $ruleItem  
       
            .\Write-Info.ps1 "Creating new Access Rules $rules with rule item $ruleItem"  -ForegroundColor Green

            try
            {
                New-ADCentralAccessRule -Name $displayName -ResourceCondition $condition -CurrentAcl $acl #-ProtectedFromAccidentalDeletion $true
                .\Write-Info.ps1 "Create new Access Rules $rules successfully" -ForegroundColor Green
            }
            catch
            {
                .\Write-Info.ps1 "Failed to create new Access Rules $rules. Error happened: $_.Exception.Message" -ForegroundColor Red
            }     
            

        }
     
        #-----------------------------------------------------------------------------------------------
        # Create CAP in AD
        #-----------------------------------------------------------------------------------------------        
        foreach ($policy in $configFile.Parameters.LocalRealm.Policies.Policy)
        {
            $displayName = $policy.Name
            .\Write-Info.ps1 "Create new Access policy $displayName"             

            try
            {
                Get-ADCentralAccessPolicy -Identity $displayName
                .\Write-Info.ps1 "CAP $displayName exists in AD, will remove it." -ForegroundColor Yellow
                Remove-ADCentralAccessPolicy -Identity $displayName -Confirm:$false
            }
            catch
            {
            }

            try
            {
                New-ADCentralAccessPolicy -Name $displayName

                foreach($rule in $policy.Rules.Rule)
                {
                    Add-ADCentralAccessPolicyMember $displayName -Members $rule
                }               
            
                .\Write-Info.ps1 "Create new Access policy $displayName successfully." -ForegroundColor Green
            }
            catch
            {
                .\Write-Info.ps1 "Failed to create new Access policy $displayName. Error happened: $_.Exception.Message" -ForegroundColor Red
            }        

        }

	    #-----------------------------------------------------------------------------------------------
        # Enable Claims for this Realm
	    # FAST is not enabled for initial environment. It will be enabled in test suite
        #-----------------------------------------------------------------------------------------------
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC /f 2>&1 | Write-Host
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /f 2>&1 | Write-Host
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f 2>&1 | Write-Host
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v CbacAndArmorLevel /t REG_DWORD /d 2 /f 2>&1 | Write-Host
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos /f 2>&1 | Write-Host
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /f 2>&1 | Write-Host
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f 2>&1 | Write-Host
        REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v Supportedencryptiontypes /t REG_DWORD /d 0x7fffffff /f 2>&1 | Write-Host
    }
    else
    {
	    .\Write-Info.ps1 "$dataFile not found. Will keep the default setting of all the test context info..."
        return
    } 
}
Function Phase3
{
    $endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\MS-AZOD\OD-Endpoint"
    $azodTestSuites = Get-ChildItem -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites\MS-AZOD-OD-*'
    $azodTestSuite = $azodTestSuites[0]
    $version = $azodTestSuite.Name.Substring(80, $azodTestSuite.Name.Length-80) 

    #-----------------------------------------------------------------------------------------------
    # Configure Group Policy for Claims
    #-----------------------------------------------------------------------------------------------
    .\Write-Info.ps1 "Extract GPOBackup files"
    .\Extract-ZipFile.ps1 -ZipFile $endPointPath\$version\Scripts\DC01GPO.zip -Destination $endPointPath\$version\Scripts\DC01GPO

    .\Write-Info.ps1 "Configuring Group Policy"
    Import-GPO -BackupId 9DA4066D-33CD-455E-B336-F2A426956D65 -TargetName "Default Domain Policy" -Path "$endPointPath\$version\Scripts\DC01GPO\" -CreateIfNeeded

    gpupdate /force 
}

Function Finish
{
    # Finish script    
    RestartAndRunFinish
    .\Write-Info.ps1 "DONE" -ForegroundColor Green

	#-----------------------------------------------------
	# Finished to config Terminal Client
	#-----------------------------------------------------
	.\Write-Info.ps1 "Write signal file: config.finished.signal to system drive."
	cmd /C ECHO CONFIG FINISHED>$env:HOMEDRIVE\config.finished.signal

	# Restart
    Restart-Computer
}

#-----------------------------------------------------------------------------
# Function: RestartAndRunFinish
# Usage   : To clean up the registry entry
#-----------------------------------------------------------------------------
Function RestartAndRunFinish()
{

    $private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
    $private:regKeyName = "TKFRSAR"

    if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
    {
	    Remove-ItemProperty -Path $regRunPath -Name $regKeyName
    }
}

#-----------------------------------------------------------------------------
# Main Script
#-----------------------------------------------------------------------------
Function Main
{ 
    switch($Step)
    {
        1 {`
	        Phase1
            $invocation = (get-variable MyInvocation -Scope 1).Value            
            $NextStep =$step+1
            RestartAndResume -ScriptPath  $invocation.MyCommand.Path `
                        -PhaseIndicator "-Step $NextStep" -AutoRestart:$true
        }
        2 {
            Phase2
            $invocation = (get-variable MyInvocation -Scope 1).Value            
            $NextStep =$step+1
            RestartAndResume -ScriptPath  $invocation.MyCommand.Path `
                        -PhaseIndicator "-Step $NextStep" -AutoRestart:$true	        
        }
        3{
            phase3
            Finish
        }
    }	
}
  
#----------------------------------------------------------------------------
# Start logging using start-transcript cmdlet
#----------------------------------------------------------------------------
$rootPath = Split-Path $MyInvocation.MyCommand.Definition -parent
Push-Location $rootPath 
$logFile =  "$rootPath\" + $MyInvocation.MyCommand.Name + ".log"
$dataFile = "$rootPath\Config.xml"
if(Test-Path -Path $dataFile)
{
    try
    {
        [xml]$configFile = Get-Content -Path $dataFile
	    $logPath = $configFile.Parameters.LogPath
        if(!(Test-Path -Path $logPath))
        {
            cmd /c mkdir $logPath 2>&1 | Write-Host
        }
	    $logFile = $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"
    }
    catch
    {
        .\Write-Info.ps1 "Read config file $dateFile failed, Exception: $_.Exception.Message.`n"
    }
}
.\Write-Info.ps1 "Use $logFile as log file."
Start-Transcript -Path "$logFile" -Append -Force
	
Main

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript
