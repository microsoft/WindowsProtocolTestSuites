#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------------------------
# Configure Group Policy for Claims
#-----------------------------------------------------------------------------------------------
Param
(
    [string]$capName
)

Write-Host "Configuring Group Policy for Claims ..." -ForegroundColor Yellow

# Configure Central Access Policy
Write-Host "Configuring Central Access Policy ..." -ForegroundColor Yellow
$id = (Get-GPO -Name "Default Domain Policy").id
$caps = Get-ADCentralAccessPolicy -Identity $capName
$path = $env:windir + "\SYSVOL\domain\Policies\{$id}\MACHINE\Microsoft\Windows NT\Cap"
$file = $path + "\cap.inf"
if(!(Test-Path -Path $path))
{
    New-Item -ItemType Directory -Path $path -Force
}
if(Test-Path -Path $file)
{
    Remove-Item -Path $file -Force
}
New-Item -ItemType File -Path $file -Force
echo "" >> $file
echo "[Version]" >> $file
echo "Signature=`"`$Windows NT`$`"" >> $file
echo "[CAPS]" >> $file
echo "`"$caps`"" >> $file

# Configure Audit
Write-Host "Configuring Audit ..." -ForegroundColor Yellow
$id = (Get-GPO -Name "Default Domain Policy").id
$path = $env:windir + "\SYSVOL\domain\Policies\{$id}\MACHINE\Microsoft\Windows NT\Audit"
$file = $path + "\audit.csv"
if(!(Test-Path -Path $path))
{
    New-Item -ItemType Directory -Path $path -Force
}
if(Test-Path -Path $file)
{
    Remove-Item -Path $file -Force
}
New-Item -ItemType File -Path $file -Force
echo "Machine Name,Policy Target,Subcategory,Subcategory GUID,Inclusion Setting,Exclusion Setting,Setting Value" >> $file
echo ",System,Audit Central Access Policy Staging,{0cce9246-69ae-11d9-bed3-505054503030},Success and Failure,,3" >> $file

# Enable FAST and Claims for this Realm
Write-Host "Enable FAST and Claims for this Realm ..." -ForegroundColor Yellow
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v CbacAndArmorLevel /t REG_DWORD /d 2 /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v SupportedEncryptionTypes /t REG_DWORD /d 0x7fffffff /f

gpupdate /force
Write-Host "Configure Group Policy for Claims Successfully." -ForegroundColor Green
