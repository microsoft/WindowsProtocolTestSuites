######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

$credential = ./WindowsSUTControlAdapter/CreatePSCredential.ps1
Invoke-Command -ComputerName $computerName {restart-service peerdistsvc} -Credential $credential
