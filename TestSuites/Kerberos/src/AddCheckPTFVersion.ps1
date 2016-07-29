param(
[string]$WxsFile=$(throw "Parameter missing: -WxsFile is missing")
)

## Check if wxs file is exists
if(-not (Test-Path $WxsFile))
{
    #add prefix
    $runningDir = split-path -parent $MyInvocation.MyCommand.Definition | select -first 1
    $WxsFile = $runningDir + $WxsFile

    Write-Host "WixFilePath $WxsFile"

    if(-not (Test-Path $WxsFile))
    {
        throw "$WxsFile does not exists";
    }
}

# query from registry to find out PTF version
$ptfInfo = Get-ItemProperty "HKLM:\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework\"
$CurrentBuildMachineVersion = $ptfInfo.PTFVersion
Write-Host "Current PTF Version: $CurrentBuildMachineVersion"

# read content from wxs file
$content = Get-Content $WxsFile -Raw

$newVersionDelcare = '<?define CurrentPTFVersion="{0}" ?>';
$newVersionDelcare = $newVersionDelcare -f $CurrentBuildMachineVersion


$newContent = ""
if ($content -match '(?<verstart>\<\?define\s+CurrentPTFVersion=)"(?<verNum>[\d\.]+)"\s+\?\>') 
{  
    # matched, this means already added PTF version check
    # 1. Replace version define
    Write-Host "PTF Version check already exists, Starting Update version"
    $newContent = $content -replace '(?<verstart>\<\?define\s+CurrentPTFVersion=)"(?<verNum>[\d\.]+)"\s+\?\>', $newVersionDelcare
}else
{
    # not match, PTF version check is not added.
    # Add $newVersionDelcare under before <Product xxxxx>
    
    $newContent = $content -replace '(?<wixNode>\<Wix\s+xmlns="http://schemas.microsoft.com/wix/2006/wi")', ($newVersionDelcare +'
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi"')

    # Add version check under package node

    $ptfVersionCheck = '
    <Property Id="PTFVERSION">
      <RegistrySearch Id="PTFVersionSearch" Root="HKLM" Win64="yes" Key="SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework" Name="PTFVersion" Type="raw">
      </RegistrySearch>
    </Property>

    <Condition Message="Protocol Test Framework does not installed on current machine.">
      <![CDATA[Installed OR PTFVERSION]]>
    </Condition>

    <Condition Message="Required version $(var.CurrentPTFVersion) of Protocol Test Framework does not installed on current machine.">
      <![CDATA[Installed OR PTFVERSION << "$(var.CurrentPTFVersion)"]]>
    </Condition>'

    $newContent = $newContent -replace "(<\/Product>)", ($ptfVersionCheck + '
  </Product>')
    
}
$newContent = $newContent.TrimEnd()
$newContent | Out-File -Encoding "UTF8" $WxsFile