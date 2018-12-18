README

This Powershell/batch scripts and configuraiton files are used to download and install prerequisite software dependencies for test suites. 

It contains one parameter "Category", which is the name of test suite and you can find it in PrerequisitesConfig.xml.
.PARAMETER Category
    The Category is used to specify which set of tools need to be downloaded and installed, based on different test suite names, such as "FileServer", Categories are defined in PrerequisitesConfig.xml
    and you can update this configure file to achieve your requirement.
    Currently we support the following Categories:
    * FileServer
    * Kerberos
    * SMB
    * SMBD
    * RDP
    * BranchCache
    * ADFamily
    * AZOD
    * ADFSPIP
    * ADOD
.PARAMETER ConfigPath
    The ConfigPath is used to specify prerequisites configure file path, default value is ".\PrerequisitesConfig.xml".

.EXAMPLE
    C:\PS>.\InstallPrerequisites.ps1 -Category 'FileServer' -ConfigPath ".\PrerequisitesConfig.xml"
    The PS script will get all tools defined under FileServer node in PrerequisitesConfig.xml, then download and install these tools.

Usage:
Run as Administrator to avoid the access deny and installation failure.    
    1. cd to the script location
    2. Run InstallPrerequisites.ps1 as administrator
    3. Input the parameter Category, for example "FileServer", "Kerberos", etc.
    4. Monitor the script and interact with the prompt dialog if any.
 

System Requirements:
    Windows 7 SP1 (with latest Windows Updates) or later
    Microsoft .NET Framework 4.5 or later
    Powershell 4.0 or later

Known issue:
    1. PTF and Wix cannot be downloaded by powershell 2.0:  The TLS 1.2 secure channel is required for Github, while powershell 2.0 does not support TLS. Install WMF 4.0 and upgrade the Powershell to 4.0 will fix this issue.
   
