# File Server Protocol Family Setup Script User Guide

## 1. Introduction

This guide provides instructions for using the automated setup scripts to configure test environments for the File Server Protocol Family Test Suite. These scripts automate many of the manual steps described in the [FileServerUserGuide.md](https://github.com/microsoft/WindowsProtocolTestSuites/blob/main/TestSuites/FileServer/docs/FileServerUserGuide.md).

The scripts support three environment configurations:
- **WORKGROUP** - Simple peer-to-peer environment with two machines
- **DOMAIN** - Domain environment with three or more machines. Most closely resembles a real world environment containing a Domain Controller
- **CLUSTER** - Domain environment with failover clustering for high availability testing

![](./image/FileServerUserGuide/image1.png)**Tip**

To learn more about the common environment in which test cases are run for all **Test Suites**, see the [Protocol Test Framework User Guide](https://github.com/microsoft/ProtocolTestFramework/blob/main/docs/PTFUserGuide.md).


## 2. Prerequisites

Before using these scripts, ensure you have:
- Windows systems with supported operating systems
  - SUT: Windows Server 2012 R2 or later
  - Driver: Operating system that can install .NET 5.0
  - Domain Controller (if applicable): Windows Server 2012 R2 or later
- Network connectivity between all machines
- Administrator access on all machines
- Sufficient disk space (at least 60 GB per machine)
- Windows PowerShell 5.1 or later
- 
### Network Setup

### Workgroup Network Environment

-- **REFER to [FileServerUserGuide.md](https://github.com/microsoft/WindowsProtocolTestSuites/blob/main/TestSuites/FileServer/docs/FileServerUserGuide.md)** --

### Computer Setup

### Workgroup Environment
The WORKGROUP environment consists of two test machines as follows:

- Driver computer - System on which the test suites will be installed and run
- SUT computer - **S**ystem **U**nder **T**est

Before you begin, create your configuration file. This will be used to configure your machines and the test suites. An example is provided in TestSuites\FileServer\Setup\Workgroup\BaseConfig.json

Create Workgroup-Package.zip by running TestSuites\FileServer\Setup\Create-Package.ps1 -Scenario Workgroup -ConfigPath "Path to your\Config.json"

Perform the following steps to set up the **SUT Computer**:
1. Set up network as described in [Workgroup Network Environment](https://github.com/microsoft/WindowsProtocolTestSuites/blob/main/TestSuites/FileServer/docs/FileServerUserGuide.md#4.1)
2. Enable Administrator Account, run these scripts using Admin Account
3. Set Powershell Execution Policy Unrestricted  [NOTE] Temporary Unrestricted while I restore signing pipeline
      ``` 
      Set-ExecutionPolicy -ExecutionPolicy Unrestricted
      ```
4. Copy Workgroup-Package.zip into your SUT
5. Extract in your working directory
6. Run Configure_SUT.ps1

Perform the following steps to set up the **Driver Computer**:
1. Set up network as described in [Workgroup Network Environment](https://github.com/microsoft/WindowsProtocolTestSuites/blob/main/TestSuites/FileServer/docs/FileServerUserGuide.md#4.1)
2. Enable Administrator Account, run these scripts using Admin Account
3. Set Powershell Execution Policy
4. Copy Workgroup-Package.zip into your Driver computer
5. Extract in your working directory
6. Run Configure_Driver.ps1


### Domain Environment

The DOMAIN environment consists of at least three test machines as follows:

- Domain Controller - Responds to security authentication requests such as logging in, checking permissions, and so on, within the domain.
- Driver computer - System on which the test suites will be installed and run
- SUT computer - **S**ystem **U**nder **T**est

Before you begin, create your configuration file. This will be used to configure your machines and the test suites. An example is provided in TestSuites\FileServer\Setup\Domain\BaseConfig.json

Create Domain-Package.zip by running TestSuites\FileServer\Setup\Create-Package.ps1 -Scenario Domain -ConfigPath "Path to your\Config.json"


Perform the following steps to set up the Domain Controller:

1. Set up network as described in Domain Network Environment
2. Set Powershell Execution Policy

      `Set-ExecutionPolicy -ExecutionPolicy Unrestricted`

3. Copy Domain-Package.zip into your DC
4. Extract in your working directory
5. Run Configure_DC.ps1

Your computer will restart several times but if configured correctly, will automatically log in and continue in the background.

Ensure your domain controller is fully set up before setting up the other machines.


Perform the following steps to set up the SUT Computer:

1. Set up network as described in Domain Network Environment
2. Enable Administrator Account, run these scripts using Admin Account
3. Set Powershell Execution Policy Unrestricted [NOTE] Temporary Unrestricted while I restore signing pipeline

      `Set-ExecutionPolicy -ExecutionPolicy Unrestricted`

4. Copy Domain-Package.zip into your SUT
5. Extract in your working directory
6. Run Configure_SUT.ps1

Perform the following steps to set up the Driver Computer:

1. Set up network as described in Domain Network Environment
2. Enable Administrator Account, run these scripts using Admin Account
3. Set Powershell Execution Policy
4. Copy Domain-Package.zip into your Driver computer
5. Extract in your working directory
6. Run Configure_Driver.ps1


### Cluster Environment

The CLUSTER environment consists of the following test machines:

- Domain Controller - Responds to security authentication requests such as logging in, checking permissions, and so on, within the domain.
- Driver computer - System on which the test suites will be installed and run
- SUT computer - **S**ystem **U**nder **T**est

Before you begin, create your configuration file. This will be used to configure your machines and the test suites. An example is provided in TestSuites\FileServer\Setup\Domain\BaseConfig.json

Create Domain-Package.zip by running TestSuites\FileServer\Setup\Create-Package.ps1 -Scenario Domain -ConfigPath "Path to your\Config.json"


Perform the following steps to set up the Domain Controller:

1. Set up network as described in Domain Network Environment
2. Set Powershell Execution Policy

      `Set-ExecutionPolicy -ExecutionPolicy Unrestricted`

3. Copy Domain-Package.zip into your DC
4. Extract in your working directory
5. Run Configure_DC.ps1

Your computer will restart several times but if configured correctly, will automatically log in and continue in the background.

Ensure your domain controller is fully set up before setting up the other machines.


Perform the following steps to set up the SUT Computer:

1. Set up network as described in Domain Network Environment
2. Enable Administrator Account, run these scripts using Admin Account
3. Set Powershell Execution Policy Unrestricted [NOTE] Temporary Unrestricted while I restore signing pipeline

      `Set-ExecutionPolicy -ExecutionPolicy Unrestricted`

4. Copy Domain-Package.zip into your SUT
5. Extract in your working directory
6. Run Configure_SUT.ps1

Perform the following steps to set up the Driver Computer:

1. Set up network as described in Domain Network Environment
2. Enable Administrator Account, run these scripts using Admin Account
3. Set Powershell Execution Policy
4. Copy Domain-Package.zip into your Driver computer
5. Extract in your working directory
6. Run Configure_Driver.ps1