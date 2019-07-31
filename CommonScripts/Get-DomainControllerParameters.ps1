#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           SetAutoLogonWithDomainAccount.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012
##
##############################################################################
Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$DomainName,

    [parameter(Mandatory=$true)]
    [ValidateScript({$_.Value.GetType().Name -eq "Hashtable"})]
    [ref]$RefParamArray,

    [string]$ConfigFile = "protocol.xml"
)
$ResultArray = @{}

try
{
    [xml]$Content = Get-Content $ConfigFile -ErrorAction Stop
    $AllVMs = $Content.SelectNodes("//vm")
}
catch
{
    throw "Failed to read config file"
}

foreach ($VM in $AllVMs)
{
    $ParamArray = @{}

    foreach ($Node in $VM.ChildNodes)
    {
        $ParamArray[$Node.Name] = $Node.InnerText
    }

    if ($ParamArray["isdc"].ToString().ToLower() -eq "true")
    {
        if ($ParamArray["domain"].ToString().ToLower() -eq $DomainName.ToLower())
        {
            $ResultArray = $ParamArray
            break
        }
    }  
}

if ($ResultArray -eq $null)
{
    throw "Failed to find the domain controller in the config file"
}

$currentCore = $Content.lab.core
if(![string]::IsNullOrEmpty($currentCore.regressiontype) -and ($currentCore.regressiontype -eq "Azure")){
    foreach($paramNode in $currentCore.ChildNodes)
    {
        $ResultArray[$paramNode.Name] = $paramNode.InnerText
    }
}

$RefParamArray.Value = $ResultArray