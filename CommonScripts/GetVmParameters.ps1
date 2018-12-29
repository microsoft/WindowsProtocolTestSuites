#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
# 
# Microsoft Windows Powershell Script
# File     : GetVmParameters.ps1
# Usage    : This file is for Lab Winblue regression run only. Use this file
#            to read parameters from VSTORMLITE XML file by specifying the 
#            VM name and the reference to an associate array.
# Params   : -VMName <String>    : The name of the VM. The parameters related
#                                  to this VM will be read to an array.
#            -RefParamArray <Ref>: The reference to an an associate array 
#                                  which is used to store the parameters.
#            -ConfigFile <String>: The config file from which parameters will
#                                  be read.
##############################################################################
param
(
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$VMName,

    [parameter(Mandatory=$true)]
    [ValidateScript({$_.Value.GetType().Name -eq "Hashtable"})]
    [ref]$RefParamArray,

    [parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [String]$ConfigFile = ".\Protocol.xml" # Default config file for lab regression run
)
   
$paramArray = $RefParamArray.Value
[xml]$content = Get-Content $ConfigFile -ErrorAction Stop

try 
{
    $currentVM = $content.SelectSingleNode("//vm[hypervname=`'$VMName`']")

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
