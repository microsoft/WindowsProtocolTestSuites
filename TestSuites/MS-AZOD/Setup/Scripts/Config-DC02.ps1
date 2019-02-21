#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-DC02.ps1
## Purpose:        Configure trust realm DC for MS-AZOD test suite.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012 +
##
##############################################################################

Function SetupForestTransitiveTrust(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [String]$targetDomainName,
        
        [Parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [String]$userName,

        [Parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [String]$userPwd
)
{
    #----------------------------------------------------------------------------
    # Create trust relationship
    #----------------------------------------------------------------------------
    .\Write-Info.ps1 " Create trust relationship between TDC and PDC"
    $succeed = $false
	$triedCount = 0
	while(!$succeed -and $triedCount -lt 10)
    {
        $triedCount++
		try
		{
            .\Write-Info.ps1 "new-object System.DirectoryServices.ActiveDirectory.DirectoryContext([System.DirectoryServices.ActiveDirectory.DirectoryContextType]::Forest,$targetDomainName,$userName,$userPwd) `n"
            $DirectoryContext = new-object System.DirectoryServices.ActiveDirectory.DirectoryContext([System.DirectoryServices.ActiveDirectory.DirectoryContextType]::Forest,$targetDomainName,$userName,$userPwd)
            .\Write-Info.ps1 "[System.DirectoryServices.ActiveDirectory.Forest]::getForest($DirectoryContext) `n"
            $targetForest = [System.DirectoryServices.ActiveDirectory.Forest]::getForest($DirectoryContext)
            .\Write-Info.ps1 "[System.DirectoryServices.ActiveDirectory.Forest]::getcurrentForest() `n"
            $sourceForest = [System.DirectoryServices.ActiveDirectory.Forest]::getcurrentForest()
            .\Write-Info.ps1 "CreateTrustRelationship($targetForest,[System.DirectoryServices.ActiveDirectory.TrustDirection]::Bidirectional) `n"
            $sourceForest.CreateTrustRelationship($targetForest,[System.DirectoryServices.ActiveDirectory.TrustDirection]::Bidirectional)

            $succeed = $true
        }
        catch
		{
			.\Write-Info.ps1 "Failed to create trust relationship. Error happened: " + $_.Exception.Message			
            Sleep 5            
		}
    }
}

Function ConfigTrustDomain
{

    $endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\MS-AZOD\OD-Endpoint"
    $azodTestSuites = Get-ChildItem -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites\MS-AZOD-OD-*'
    $azodTestSuite = $azodTestSuites[0]
    $version = $azodTestSuite.Name.Substring(80, $azodTestSuite.Name.Length-80)

    $dataFile = "$endPointPath\$version\scripts\Config.xml"     
    
    if(Test-Path -Path $dataFile)
    {
        try
        {
	        [xml]$configFile = Get-Content -Path $dataFile
	        $logPath	= $configFile.Parameters.LogPath
	        $logFile	= $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"

	        $domainName 	= $configFile.Parameters.TrustRealm.DomainName
                $domainAdmin 	= $configFile.Parameters.TrustRealm.DomainAdministrator.UserName
                $domainAdminPwd 	= $configFile.Parameters.TrustRealm.DomainAdministrator.Password

                $trustedDomainName = $configFile.Parameters.TrustRealm.Trust.TrustedRealmName
                $trustedDomainAdmin = $configFile.Parameters.TrustRealm.Trust.TrustedRealmAdmin
                $trustedDomainAdminPwd = $configFile.Parameters.TrustRealm.Trust.TrustedRealmAdminPwd
        }
        catch
        {
            .\Write-Info.ps1 " Failed to read data file $dataFile. Please check the file content. Error happened: $_.Exception.Message"
            return
        }    

        # Setup forest trust between contoso and kerb
        SetupForestTransitiveTrust $trustedDomainName $trustedDomainAdmin $trustedDomainAdminPwd


        #-----------------------------------------------------------------------------------------------
        # Define Claim Type
        #-----------------------------------------------------------------------------------------------
      
        foreach ($claimtype in $configFile.Parameters.TrustRealm.ClaimTypes.ClaimType)
        {
            $displayName = $claimtype.DisplayName
            .\Write-Info.ps1 " Create claim $displayName"
            try
            {
                Get-ADClaimType $displayName
                .\Write-Info.ps1 "Claim Type $displayName already exists, don't need to create it."
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

                    .\Write-Info.ps1 "Creating new Claim Type $displayName"
                    New-ADClaimType -AppliesToClasses @("User", "Computer") -DisplayName $displayName -SourceAttribute $displayName -SuggestedValues $suggestedValues -ID "ad://ext/$displayName"
                    Get-ADClaimType $displayName
                    .\Write-Info.ps1 "New Claim Type Created Successfully. DisplayName $displayName" -ForegroundColor Green
                }
                catch
                {
	                .\Write-Info.ps1 "Error occur during create claim type $displayName, Exception: $_.Exception.Message" -ForegroundColor Red
                }  
            }
        }

        #-----------------------------------------------------------------------------------------------
        # Create CAR in AD
        #-----------------------------------------------------------------------------------------------
        foreach ($rule in $configFile.Parameters.TrustRealm.Rules.Rule)
        {
            $displayName = $rule.Name
            $ruleItem = $rule.RuleItem        

            .\Write-Info.ps1 "Create CAR $displayName"

            try
            {
                Get-ADCentralAccessRule -Identity $displayName
                .\Write-Info.ps1 "CAR $displayName exists in AD, will remove it." -ForegroundColor Green
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
                .\Write-Info.ps1 "New-ADCentralAccessRule -Name $displayName  -ResourceCondition $condition -CurrentAcl $acl" 
                New-ADCentralAccessRule -Name $displayName  -ResourceCondition $condition -CurrentAcl $acl 
                .\Write-Info.ps1 "Create new Access Rules $rules successfully"  -ForegroundColor Green
            }
            catch
            {
                .\Write-Info.ps1 "Failed to create new Access Rules $rules, Exception: $_.Exception.Message" -ForegroundColor Red
            }     
            

        }
     
        #-----------------------------------------------------------------------------------------------
        # Create CAP in AD
        #-----------------------------------------------------------------------------------------------        
        foreach ($policy in $configFile.Parameters.TrustRealm.Policies.Policy)
        {           
            $displayName = $policy.Name
            .\Write-Info.ps1 "Create new Access policy $displayName"              

            try
            {
                Get-ADCentralAccessPolicy -Identity $displayName
                .\Write-Info.ps1 "CAP $displayName exists in AD, will remove it." -ForegroundColor Green
                Remove-ADCentralAccessPolicy -Identity $displayName -Confirm:$false
            }
            catch
            {}

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
                .\Write-Info.ps1 "Failed to create new Access policy $displayName, Exception: $_.Exception.Message" -ForegroundColor Red
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

Function DeployCAP
{
    $endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\MS-AZOD\OD-Endpoint"
    $azodTestSuites = Get-ChildItem -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites\MS-AZOD-OD-*'
    $azodTestSuite = $azodTestSuites[0]
    $version = $azodTestSuite.Name.Substring(80, $azodTestSuite.Name.Length-80)  
    $scriptLocation  = "$endPointPath\$version\scripts"
    Push-Location $scriptLocation

    #-----------------------------------------------------------------------------------------------
    # Configure Group Policy for Claims
    #-----------------------------------------------------------------------------------------------
    Write-Host "Extract GPOBackup files"
    .\Extract-ZipFile.ps1 -ZipFile $endPointPath\$version\Scripts\DC02GPO.zip -Destination $endPointPath\$version\Scripts\DC02GPO

    Write-Host "Configuring Group Policy"
    Import-GPO -BackupId 7D5F951C-D924-4118-80C9-DFCEC3B2FD08 -TargetName "Default Domain Policy" -Path "$endPointPath\$version\Scripts\DC02GPO\" -CreateIfNeeded

    gpupdate /force 

    # Create claim transformation policy
    $dataFile = "$endPointPath\$version\scripts\Config.xml"     
    $logPath = $env:SystemDrive
    $logFile = $MyInvocation.MyCommand.Name + ".log"
   
    $ClaimTransformPolicy  = $null
    $ClaimTransformPolicyDesc  = $null
    $ClaimTransformPolicyDenyAllExcept  = $null
    $ClaimTransformPolicyServer  = $null

    if(Test-Path -Path $dataFile)
    {
        try
        {
	        [xml]$configFile = Get-Content -Path $dataFile
	        $logPath	= $configFile.Parameters.LogPath
	        $logFile	= $logPath + "\" + $MyInvocation.MyCommand.Name + ".log"

	        $domainName 	= $configFile.Parameters.TrustRealm.DomainName
            $domainAdmin 	= $configFile.Parameters.TrustRealm.DomainAdministrator.UserName
            $domainAdminPwd 	= $configFile.Parameters.TrustRealm.DomainAdministrator.Password

            $ClaimTransformPolicy  = $configFile.Parameters.TrustRealm.DomainName
            $ClaimTransformPolicyDesc  = $null
            $ClaimTransformPolicyDenyAllExcept  = $null
            $ClaimTransformPolicyServer  = $null
        }
        catch
        {
            .\Write-Info.ps1 "Failed to read data file $dataFile. Please check the file content, Exception: $_.Exception.Message"
            return
        }    
         
        foreach ($policy in $configFile.Parameters.TrustRealm.ClaimTransformPolicies.ClaimTransformPolicy)
        {
            $CTPName = $policy.Name
            $CTPDesc = $policy.Description
            $CTPDenyAllExcept = $policy.DenyAllExcept
            $CTPServer = $policy.Server

            .\Write-Info.ps1 "Create new AD Claim Transform Policy $CTPName."
            
            try
            {
                Get-ADClaimTransformPolicy -Identity $CTPName
                .\Write-Info.ps1 "Claim transform policy $CTPName exists in AD, will remove it." -ForegroundColor Yellow
                Remove-ADCentralAccessPolicy -Identity $CTPName -Confirm:$false
            }
            catch
            {}

            try
            {
                New-ADClaimTransformPolicy -Description:$CTPDesc -Name:$CTPName -DenyAllExcept:$CTPDenyAllExcept -Server:$CTPServer       

                .\Write-Info.ps1 "Create new claim transfrom policy $CTPName successfully." -ForegroundColor Green
                 
                #create claim transformation link: ToBeFixed
                #Set-ADClaimTransformLink -Identity:$domainName -Policy:$CTPName -TrustRole:Trusting
            }
            catch
            {
                .\Write-Info.ps1 "Failed to create new claim transfrom policy or claim transform link $CTPName, Exception: $_.Exception.Message"  -ForegroundColor Red
            }    
        }
 
    }

    # Publish the Group Policy
    gpupdate /force 
}

#-----------------------------------------------------------------------------
# Main Script
#-----------------------------------------------------------------------------
Function Main
{
    ConfigTrustDomain
    DeployCAP
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
        .\Write-Info.ps1 " Read config file $dateFile failed, Exception: $_.Exception.Message.`n"
    }
}
.\Write-Info.ps1 " Use $logFile as log file. `n"
Start-Transcript -Path "$logFile" -Append -Force
    
Main

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript
