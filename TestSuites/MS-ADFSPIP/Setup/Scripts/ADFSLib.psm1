#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Sripting
## File         :   ADFSLib.PSM1
## Requirements :   Windows Powershell 3.0
## Supported OS :   Windows Server 2012
##
##############################################################################

function Install-DomainController {
##
#.SYNOPSIS
# Install ADDS feature on the server and promote it to DC.
#.DESCRIPTION
# A reboot is needed after promoting to DC.
#.PARAMETER DomainName
# The name of the domain.
#.PARAMETER AdminPassword
# The password of the domain Administrator.
#.PARAMETER IsPrimary
# If it is the primary DC in the foreast.
#.PARAMETER ForestMode
# Forest Mode.
##
[CmdletBinding()]
	param (
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$DomainName, 	    
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$AdminPassword,
	    [Parameter(Mandatory=$false)][ValidateNotNullOrEmpty()][bool] $IsPrimary = $true,
	    [Parameter(Mandatory=$false)][ValidateNotNullOrEmpty()][string]$ForestMode
	)
	    
	process {
	    Write-Verbose "Installing Windows Feature"
	    Install-WindowsFeature -Name AD-Domain-Services `
	                           -IncludeAllSubFeature `
	                           -IncludeManagementTools 
	    Write-Verbose "Install Windows Feature Succeed"

	    Import-Module ADDSDeployment
	    $SecurePwd = ConvertTo-SecureString $AdminPassword -AsPlainText -Force
		
	    if ($IsPrimary) {
			Write-Verbose "Installing ADDS Forest"
	        if ([System.String]::IsNullOrEmpty($ForestMode)) {
	            Install-ADDSForest -DomainName $domainName -InstallDns `
	                               -SafeModeAdministratorPassword $SecurePwd `
	                               -NoRebootOnCompletion -Force
	        } else {
	            Install-ADDSForest -ForestMode $ForestMode -DomainMode $ForestMode `
	                               -DomainName $domainName -InstallDns `
	                               -SafeModeAdministratorPassword $SecurePwd `
	                               -NoRebootOnCompletion -Force
	        }
			Write-Verbose "Install ADDS Forest Succeed"
	    } else {
	        $cred = New-Object System.Management.Automation.PSCredential `
					"$domainName\Administrator", $SecurePwd 
			Write-Verbose "Installing ADDS Domain Controller"
	        Install-ADDSDomainController -DomainName $domainName -Credential $cred -InstallDNS `
	                                     -SafeModeAdministratorPassword $SecurePwd `
	                                     -NoRebootOnCompletion -Force
			Write-Verbose "Install ADDS Domain Controller Succeed"
	    }
	}
}

function Join-Domain {
##
#.SYNOPSIS
# Join the computer to a domain.
#.DESCRIPTION
# A reboot is needed after joining the domain.
#.PARAMETER Domain
# The domain needs to be joined.
#.PARAMETER Username
# The user name with the permission to join the domain.
#.PARAMETER Password
# The password for the username.
##
[CmdletBinding()]
	param (
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$Domain,
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$Username,
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$Password
	)
    process {
	    $credential = New-Object System.Management.Automation.PSCredential `
	                  -ArgumentList "$domain\$username", `
					  (ConvertTo-SecureString $password -AsPlainText -Force) 
	    Write-Verbose "Adding computer to the domain"
	    Add-Computer -Credential $credential -DomainName $domain -Restart:$false -Force 
		Write-Verbose "Add computer succeed"
	}
}

function Add-AdfsDnsRecord {
##
#.SYNOPSIS
# Add Adfs DNS information to the DNS server.
##
[CmdletBinding()]
	param (
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$ZoneName,
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$IPv4Address
	)
	process {
		Add-DnsServerResourceRecord -A -ZoneName $ZoneName -Name "adfs" -IPv4Address $IPv4Address
		Add-DnsServerResourceRecord -A -ZoneName $ZoneName -Name "webapp" -IPv4Address $IPv4Address
		Add-DnsServerResourceRecord -CName -ZoneName $ZoneName -Name "enterpriseregistration" -HostNameAlias "adfs.$ZoneName"
	}
}

function Install-WebApplicationProxy {
##
#.SYNOPSIS
# Install Web Application Proxy role on this server.
##
[CmdletBinding()]
    param ()
    process {
        Install-WindowsFeature Web-Application-Proxy -IncludeManagementTools
    }
}

function Set-NetworkLocation {
##
#.SYNOPSIS
# Set network location to public or private.
#.PARAMETER Domain
# Indicate the network location which.
# This value MUST be either public or private.
##
[CmdletBinding()]
	param (
	    [parameter(mandatory=$true)][ValidateNotNullOrEmpty()]
        [ValidateScript({$_.ToLower() -match "^private$|^public$"})]
        [string]$NetworkLocation
	)
	process {	    
	    if ("private" -eq $NetworkLocation.ToLower()) { $category = 1 }
		else { $category = 3 }
		
	    # Network location setting only available after Windows Vista
	    if([environment]::OSVersion.Version.Major -lt 6){ return }

	    # Skip network location setting if local machine is joined to a domain
	    if(1,3,4,5 -contains (Get-WmiObject win32_computersystem).DomainRole) { return }

	    # Get network connections
	    $NetworkListManager = [Activator]::CreateInstance([Type]::GetTypeFromCLSID([Guid]"{DCB00C01-570F-4A9B-8D69-199FDBA5723B}"))
	    $Connections = $NetworkListManager.GetNetworkConnections()

	    # Set network location
	    $Connections | foreach {$_.GetNetwork().SetCategory($category) }
	}
}

function Set-AutoLogon {
##
#.SYNOPSIS
# Set the computer to be able to auto logon by the 
# providing username and pasword.
##
[CmdletBinding()]
	param (
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][String]$Domain,
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][String]$Username,
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][String]$Password,
	    [string]$Count = 999
	)
	process {
	    $regPath = "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"
	    try {
		Set-ItemProperty -Path $regPath -Name "AutoAdminLogon" -Value "1" -Force -ErrorAction Stop
		Set-ItemProperty -Path $regPath -Name "DefaultDomainName" -Value $Domain -Force -ErrorAction Stop
		Set-ItemProperty -Path $regPath -Name "DefaultUserName" -Value $Username -Force -ErrorAction Stop
		Set-ItemProperty -Path $regPath -Name "DefaultPassword" -Value $Password -Force -ErrorAction Stop
		Set-ItemProperty -Path $regPath -name "AltDefaultUserName" -value "$Username" -Force -ErrorAction Stop
		Set-ItemProperty -Path $regPath -name "AltDefaultDomainName" -value "$Domain" -Force -ErrorAction Stop
		Set-ItemProperty -Path $regPath -Name "AutologonCount" -Value $Count -Force -ErrorAction Stop
	    }
	    catch {
		throw "Unable to set auto logon for domain $Domain user $Username with password $Password and logon count $Count. Error happened: "+$_.Exception.Message
	    }
        Get-ItemProperty -Path $regPath -Name "AutoAdminLogon" | Out-File $ENV:HOMEDRIVE\temp1.txt
        Get-ItemProperty -Path $regPath -Name "AutologonCount" | Out-File $ENV:HOMEDRIVE\temp2.txt
	}
}

function New-SharedFolder {
##
#.SYNOPSIS
# Create a folder and share it to everyone.
#.PARAMETER ShareFolderPath
# The folder path to be shared. Must be absolute path.
##
[CmdletBinding()]
	param (
	    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
	    [ValidateScript({[System.IO.Path]::IsPathRooted($_)})]
	    [String]$ShareFolderPath
	)
	process { 
	    # Create new folder
	    if(!(Test-Path -Path $ShareFolderPath)) {
	        New-Item -Path $ShareFolderPath -ItemType Directory -ErrorAction Stop
	    }

	    # User the folder name as the share name
	    $ShareName = [System.IO.Path]::GetFileName($ShareFolderPath)

	    # Share the folder to everyone
	    $Command = "net.exe share $ShareName=$ShareFolderPath /grant:everyone,FULL"
	    cmd /c  $Command 2>&1 | Write-Host
	    # If the folder has already been shared, net share will return 2
	    if($LASTEXITCODE -eq 2)
	    { Write-Host "The name has already been shared."}
	    elseif($LASTEXITCODE -ne 0)
	    { throw "Error happened in executing $Command. Return value: $LASTEXITCODE"}
        else {}
	}
}

function Enable-Remoting {
##
#.SYNOPSIS
# Enable PSRemoting and set trust to all hosts.
##
[CmdletBinding()]
	param()
	process {
		Write-Verbose "Set network location to private"
		Set-NetworkLocation -NetworkLocation "Private"
		
		Write-Verbose "Enable PSRemoting"
		Enable-PSRemoting -SkipNetworkProfileCheck -Force
	    # Add all computers to the list of trusted hosts
	    # This is requires for pssession remoting
		Write-Verbose "Set TrustedHosts"
	    Set-Item WSMan:\localhost\Client\TrustedHosts -Value * -Force
	}
}

workflow Restart-AndResume {
##
#.SYNOPSIS
# Restart the computer and resume the unfinished scripts.
#.DESCRIPTION
# This cmdlet must be called from a workflow. It restarts 
# the computer and the unfinished scripts in the calling
# workflow will be resumed after rebooting.
#requires -version 3.0
##
[CmdletBinding()]
    param()
    $tsname = "Restart-And-Resume"
    # After rebooting, the unfinished workflow scripts will 
    # be suspended as a job. Schedule a task to resume the job.
    InlineScript {
        $pstart = "Powershell.exe"
        $action = "-NonInteractive -NoExit -Command `"Get-Job -State Suspended|Resume-Job`""
        $act  = New-ScheduledTaskAction -Execute $pstart -Argument $action
        $trig = New-ScheduledTaskTrigger -AtLogOn
        Register-ScheduledTask -TaskName $using:tsname -Action $act -Trigger $trig -RunLevel Highest
    }
    # it is the -wait parameter which does the trick
    Restart-Computer -Force -Wait
    InlineScript {
        Get-Job -State Running   | Wait-Job
        Get-Job -State Completed | Remove-Job -Confirm:$false
        Unregister-ScheduledTask -TaskName $using:tsname -Confirm:$false
    }
}


function Read-VMParameters {
##
# 
#.SYNOPSIS
# Read parameters related to the current VM in the protocol config file.
#.DESCRIPTION
# Used for Lab Winblue regression run only. Use this function to read 
# parameters from VSTORMLITE XML file by specifying the VM name and the 
# reference to an associate array.
#.PARAMETER VMName
# The name of the VM. The parameters related to this VM will be read to an array.
#.PARAMETER RefParamArray
# The reference to an an associate array which is used to store the parameters.
#.PARAMETER ConfigFile
# The config file from which parameters will be read.
##
[CmdletBinding()]
    param (
        [parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][String]$VMName,
        [parameter(Mandatory=$true)][ValidateScript({$_.Value.GetType().Name -eq "Hashtable"})]
        [ref]$RefParamArray,
        [parameter(Mandatory=$false)][ValidateNotNullOrEmpty()]
        [String]$ConfigFile = "MS-ADFSPIP.XML" # Default config file 
    )
    process {
        $paramArray = $RefParamArray.Value
        [xml]$content = Get-Content $ConfigFile

        $currentVM = $content.SelectSingleNode("//vm[hypervname=`'$VMName`']")

        foreach($paramNode in $currentVM.ChildNodes) {
            $paramArray[$paramNode.Name] = $paramNode.InnerText
        }

		if ($paramArray -eq $null) {
			Write-Error "Failed to read parameters from the config file"
		}
    }
}

function Read-DCParameters {
##
# 
#.SYNOPSIS
# Read domain controller parameters from the config file.
#.DESCRIPTION
# Used for Lab Winblue regression run only. Use this function to read 
# domain controller parameters from VSTORMLITE XML file by specifying 
# the domain name and the reference to an associate array.
#.PARAMETER DomainName
# The name of the domain. 
#.PARAMETER RefParamArray
# The reference to an an associate array which is used to store the parameters.
#.PARAMETER ConfigFile
# The config file from which parameters will be read.
##
[CmdletBinding()]
    param (
        [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$DomainName,
        [parameter(Mandatory=$true)][ValidateScript({$_.Value.GetType().Name -eq "Hashtable"})]
        [ref]$RefParamArray,
        [string]$ConfigFile = "MS-ADFSPIP.XML" # Default config file
    )
    process {
        [xml]$Content = Get-Content $ConfigFile
        $AllVMs = $Content.SelectNodes("//vm")

        $ResultArray = @{}

        foreach ($VM in $AllVMs){
            $ParamArray = @{}

            foreach ($Node in $VM.ChildNodes){
                $ParamArray[$Node.Name] = $Node.InnerText
            }

            if ($ParamArray["isdc"].ToString().ToLower() -eq "true"){
                if ($ParamArray["domain"].ToString().ToLower() -eq $DomainName.ToLower()){
                    $ResultArray = $ParamArray
                    break
                }
            }  
        }

        if ($ResultArray -eq $null){
            Write-Error "Failed to read parameters from the config file"
        }

        $RefParamArray.Value = $ResultArray
    }
}

function Show-PromptMessage {
##
#.SYNOPSIS
# Show a prompt message to the host.
##
[CmdletBinding()]
    param (
        [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$Message
    )
    process {
        Write-Host $Message -ForegroundColor Yellow
    }
}

function Set-EthernetSettings {
##
#.SYNOPSIS
# Set a single static IP address and DNS to the ethernet.
##
[CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$IPAddress,
        [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$DnsServer
    )
    begin {
        Import-Module NetTCPIP
        Import-Module DnsClient
    }
    process {
        Write-Verbose "Remove all existing static IPs"
        Get-NetIPAddress -InterfaceAlias Ethernet | Remove-NetIPAddress -Confirm:$false
        Write-Verbose "Add new static IP address"
        New-NetIPAddress -InterfaceAlias Ethernet -IPAddress $IPAddress -PrefixLength 24
        Write-Verbose "Set DNS server address"
        Set-DnsClientServerAddress -InterfaceAlias Ethernet -ServerAddresses $DnsServer
    }
}

# export all the functions in this module
Export-ModuleMember -Function *
