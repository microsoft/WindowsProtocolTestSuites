#############################################################################
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
##############################################################################

Param(
[String]$PTFconfigFilePathOnVM
)

# Update Basic RDP SUT Control Adapters
$ptfconfigXml = [xml](Get-Content $PTFconfigFilePathOnVM)
$updateTag = $ptfconfigXml.TestSite.Adapters.ChildNodes|Where{$_.name -eq 'IRdpSutControlAdapter' -and ($_.type -eq 'powershell')}
if($updateTag)
{
    $updateTag.ParentNode.RemoveChild($updateTag)
    $ptfconfigXml.Save($PTFconfigFilePathOnVM)
}

$oldString = '<!--<Adapter xsi:type="managed" name="IRdpSutControlAdapter" adaptertype="Microsoft.Protocols.TestSuites.Rdp.ProtocolBasedRdpSUTControlAdapter"/>-->'
$newString = '<Adapter xsi:type="managed" name="IRdpSutControlAdapter" adaptertype="Microsoft.Protocols.TestSuites.Rdp.ProtocolBasedRdpSUTControlAdapter"/>'
."$PSScriptRoot/Replace-FileContent.ps1" -FileName $PTFconfigFilePathOnVM -OldString $oldString -NewString $newString

# Update RDPEI SUT Control Adapters
$ptfconfigXml = [xml](Get-Content $PTFconfigFilePathOnVM)
$updateTag = $ptfconfigXml.TestSite.Adapters.ChildNodes|Where{$_.name -eq 'IRdpeiSUTControlAdapter' -and ($_.type -eq 'powershell')}
if($updateTag)
{
    $updateTag.ParentNode.RemoveChild($updateTag)
    $ptfconfigXml.Save($PTFconfigFilePathOnVM)
}

$oldString = '<!--<Adapter xsi:type="managed" name="IRdpeiSUTControlAdapter" adaptertype="Microsoft.Protocols.TestSuites.Rdp.ProtocolBasedRdpeiSUTControlAdapter"/>-->'
$newString = '<Adapter xsi:type="managed" name="IRdpeiSUTControlAdapter" adaptertype="Microsoft.Protocols.TestSuites.Rdp.ProtocolBasedRdpeiSUTControlAdapter"/>'
."$PSScriptRoot/Replace-FileContent.ps1" -FileName $PTFconfigFilePathOnVM -OldString $oldString -NewString $newString

# Update RDPEDISP SUT Control Adapters
$ptfconfigXml = [xml](Get-Content $PTFconfigFilePathOnVM)
$updateTag = $ptfconfigXml.TestSite.Adapters.ChildNodes|Where{$_.name -eq 'IRdpedispSUTControlAdapter' -and ($_.type -eq 'powershell')}
if($updateTag)
{
    $updateTag.ParentNode.RemoveChild($updateTag)
    $ptfconfigXml.Save($PTFconfigFilePathOnVM)
}
$oldString = '<!--<Adapter xsi:type="managed" name="IRdpedispSUTControlAdapter" adaptertype="Microsoft.Protocols.TestSuites.Rdp.ProtocolBasedRdpedispSUTControlAdapter"/>-->'
$newString = '<Adapter xsi:type="managed" name="IRdpedispSUTControlAdapter" adaptertype="Microsoft.Protocols.TestSuites.Rdp.ProtocolBasedRdpedispSUTControlAdapter"/>'
."$PSScriptRoot/Replace-FileContent.ps1" -FileName $PTFconfigFilePathOnVM -OldString $oldString -NewString $newString