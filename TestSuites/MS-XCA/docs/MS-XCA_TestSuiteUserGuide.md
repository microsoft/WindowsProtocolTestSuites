# MS-XCA Test Suite User Guide

## Table of Contents

- [MS-XCA Test Suite User Guide](#ms-xca-test-suite-user-guide)
  - [Table of Contents](#table-of-contents)
  - [1 Test Environment Requirements](#1-test-environment-requirement)
    - [1.1 Generic Requirements](#11-generic-requirements)
  - [2 Test Suite Deployment](#2-test-suite-deployment)
    - [2.1 How to configure Driver Computer](#21-how-to-configure-driver-computer)
    - [2.2 How to configure the SUT Application](#22-how-to-configure-sut)
    - [2.3 How to configure the Test Suite](#23-how-to-configure-test-suite)
    - [2.4 How to run the Test Suite](#24-how-to-run-test-suite)
      - [2.4.1 Scripts](#241-scripts)
      - [2.4.2 Protocol Test Manager(PTM) Service](#242-ptm-service)
  - [3 Debugging Test Cases](#3-debugging-test-cases)
    
## 1 Test Environment Requirements

### 1.1 Generic Requirements

The MS-XCA Test Suite requires a test environment including

- 1 machine acting as the Driver computer to run the Test Suite
- XCA protocol implementation as an SUT application

| **Device Type** | **Role** | **Description** |
| ----------------|----------|-----------------|
| Hardware        | Driver Computer | Driver computer to run the Test Suite |
| Software        | SUT             | An implementation of MS-XCA protocols |

## 2 Test Suite Deployment

### 2.1 How to configure Driver Computer

The following software needs to be installed on the Driver Computer.

- [.NET SDK](https://dotnet.microsoft.com/download/dotnet) of the same version used to build the Test Suite
- [MS-XCA Test Suite](https://github.com/microsoft/windowsprotocoltestsuites/releases)

Optional software include

- [PTM Service](https://github.com/microsoft/windowsprotocoltestsuites/wiki/PTMService)

The MS-XCA Test Suite can be executed using one of two ways
1. By extracting the compressed release .zip or .tar.gz to an available path on the Driver Computer, and running the scripts in the Batch folder.
2. By using the PTM Service. Install the PTM Service, and then install the MS-XCA Test Suite on the PTM Service.
 
### 2.2 How to configure the SUT Application

The SUT application needs to be executable on the Driver Computer, and it should be located on a path that can be accessed by the TestSuite. This would enable the Test Suite call it with the appropriate input and output files for automatic comparison. The Test Suite comes with a sample CLI SUT application (XcaTestApp) to demonstrate using the TestSuite with a configured SUT. The app can be run standalone, with arguments for the input/output files and compression algorithm to use for the operation.

### 2.3 How to configure the Test Suite

The only file that needs modification to configure the Test Suite is the **MS-XCA_TestSuite.deployment.ptfconfig** file under the Bin directory in the TestSuite. If you're using the PTMService, the editable values are shown while setting up for the MS-XCA Test.

On installation, the Test Suite will use the bundled XcaTestApp as the SUT, and all input and output folders will be sub folders in the TestSuite.

All the properties under the **SUT** group may need to be updated when testing a different SUT from the bundled XcaTestApp, some of which are shown below.

  ```xml
    <Group name="SUT">
        <Property name="WorkingDirectory" />
        <Property name="PLAIN_LZ77_COMPRESSION_COMMAND" />
        <Property name="PLAIN_LZ77_COMPRESSION_ARGUMENTS" />
        <Property name="PLAIN_LZ77_DECOMPRESSION_COMMAND" />
        <Property name="PLAIN_LZ77_DECOMPRESSION_ARGUMENTS" />
    </Group>
  ```

There is a description for each property in the **MS-XCA_TestSuite.deployment.ptfconfig**, and the values should be modified according to the descriptions and the current test environment.

### 2.4 How to run the Test Suite

The MS-XCA Test Suite can be run using any of the following

- Protocol Test Manager (PTM) Service
- Scripts

#### 2.4.1 Scripts

The **MS-XCA Test Suites** include executable scripts that can be used to run test cases. These files are located in the Batch\\ folder, and there are both Powershell and Bash scripts available.

On how to run these batch files, please refer to this [guide](https://github.com/microsoft/WindowsProtocolTestSuites#run-test-suite-by-batch).

#### 2.4.2 Protocol Test Manager (PTM) Service

The PTM Service is a user interface based tool that provides an easy and visual method for configuring and running the Test Suites.

On how to use the PTMService to run Test Suite tests, please refer to this [guide](https://github.com/microsoft/windowsprotocoltestsuites/wiki/PTMService#usage)

## 3 Debugging Test Cases

You can use the Visual Studio solution (.sln) file included with the  **MS-XCA Test Suite** to debug the test cases that you run against your own protocol implementations. The **MS-XCA.sln** file is located [here](https://github.com/microsoft/WindowsProtocolTestSuites/tree/main/TestSuites/MS-XCA/src).


To debug a test case, perform the steps that follow.

1. On the Driver computer, use Microsoft® Visual Studio® to open the following solution file: [MS-XCA.sln](https://github.com/microsoft/WindowsProtocolTestSuites/tree/main/TestSuites/MS-XCA/src).

2. In Visual Studio, in the Solution Explorer window, right-click the **Solution MS-XCA**, and then select **Build Solution**.

3. When you build the test project, the tests appear in **Test Explorer**. If Test Explorer is not visible, choose **Test** on the Visual Studio menu, select **Windows**, and then select **Test Explorer**.

4. Select your test cases from Test Explorer and run or debug them.
