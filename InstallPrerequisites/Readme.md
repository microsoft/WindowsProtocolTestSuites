# Install Prerequisites

These PowerShell/batch scripts and configuraiton files are used to download and install prerequisite software dependencies for test suites. It contains two parameters, details see below.

## Parameters

1. The parameter **Category** is used to specify which set of tools need to be downloaded and installed, based on different test suite names, such as `FileServer`, Categories are defined in PrerequisitesConfig.xml
    and you can update this configure file to achieve your requirement.
    Currently we support the following Categories:

    * **BuildTestSuites**
      Choose this category if you want to build the test suites/PTM from source code. To build the test suites of ADFamily/MS-AZOD/MS-ADOD, you need to choose the specific test suite name and run InstallPrerequisites.ps1 again.
    * **FileServer**
      Choose this category if you want to install the prerequisites to run FileServer test suite.
    * **Kerberos**
      Choose this category if you want to install the prerequisites to run Kerberos test suite.
    * **SMB**
      Choose this category if you want to install the prerequisites to run MS-SMB test suite.
    * **SMBD**
      Choose this category if you want to install the prerequisites to run MS-SMBD test suite.
    * **RDP**
      Choose this category if you want to install the prerequisites to run RDP Client/Server test suite.
    * **BranchCache**
      Choose this category if you want to install the prerequisites to run BranchCache test suite.
    * **ADFamily**
      Choose this category if you want to install the prerequisites to run ADFamily test suite.
    * **AZOD**
      Choose this category if you want to install the prerequisites to run MS-AZOD test suite.
    * **ADFSPIP**
      Choose this category if you want to install the prerequisites to run MS-ADFSPIP test suite.
    * **ADOD**
      Choose this category if you want to install the prerequisites to run MS-ADOD test suite.


2. The parameter **ConfigPath** is used to specify prerequisites configure file path, default value is ".\PrerequisitesConfig.xml".

## Example

1. Go to the script folder.
2. Run InstallPrerequisites.ps1 as administrator.

    ```
    C:\PS>.\InstallPrerequisites.ps1 -Category FileServer -ConfigPath ".\PrerequisitesConfig.xml"
    ```

    The PowerShell script will get all tools defined under `FileServer` node in PrerequisitesConfig.xml, then download and install these tools.
3. Monitor the script and interact with the prompt dialog if any.

## System Requirements

* Windows 7 SP1 (with latest Windows Updates) or later
* Microsoft .NET Framework 4.5 or later
* PowerShell 4.0 or later

