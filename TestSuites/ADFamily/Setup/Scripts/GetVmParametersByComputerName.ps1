#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

param
(
    [parameter(Mandatory=$true)]
    [ValidateScript({$_.Value.GetType().Name -eq "Hashtable"})]
    [ref]$RefParamArray,

    [parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [String]$ConfigFile = "c:\temp\Protocol.xml" # Default config file for lab regression run
)
   
$paramArray = $RefParamArray.Value
[xml]$content = Get-Content $ConfigFile -ErrorAction Stop
$VMName = Get-Content c:\temp\name.txt

try 
{
    $currentCore = $content.lab.core
    foreach($paramNode in $currentCore.ChildNodes)
    {
        $paramArray[$paramNode.Name] = $paramNode.InnerText
    }

    $currentVM = $content.SelectSingleNode("//vm[name=`'$VMName`']")

    foreach($paramNode in $currentVM.ChildNodes)
    {
        $paramArray[$paramNode.Name] = $paramNode.InnerText
    }
}
catch
{
    [String]$Emsg = "Unable to read parameters from protocol.xml. Error happened: " + $_.Exception.Message
    throw $Emsg
}
