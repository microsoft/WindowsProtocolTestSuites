*************************************************************************************
Copyright (c) Microsoft Corporation. All rights reserved.
*************************************************************************************

Version: 


I. Introduction
----------------------------
The MS-ADOD protocol Test Suite package includes MS-ADOD test suite installer and the other installers for dependencies required for the Test Suite Client. This document also provides instructions on how to setup System under Test (Windows Server 2008 R2). For Non-Windows implementation of SUT, please refer to MS-RDPBCGR_QuickStartGuide.docx under the test suite installation location.


II. System Requirements
----------------------------
Please run MS-ADOD test suite in workgroup or domain environment. 
   o  Three computers (physical or virtual computer) are required to run test suite in workgroup environment.
      - One computer setup as test Driver computer running on Windows 8 or latest service pack
      - One computer setup as test Client computer running on Windows 8 or latest service pack
      - One computer setup as test DC computer running on Windows 8 Server or Non-Windows DC Implementation

Following are the requirements of minimal computer configuration.
   o  Test Driver computer 
      o Minimum Memory: 2GB
      o Operating System: Windows 8 or latest service pack
      o Minimum Hard Drive: 60G
   o  Test Client computer 
      o Minimum Memory: 2GB
      o Operating System: Windows 8 or latest service pack / Non-Windows Client Implementation
      o Minimum Hard Drive: 60G
   o  Test DC computer
      o Minimum Memory: 1GB
      o Operating System: Windows 8 Server / Non-Windows DC Implementation
      o Minimum Hard Drive: 60G
      o Software/Service: Active Directory Domain Service or LDAP Server

III. Software Prerequisites
----------------------------
The following software is required on the Test Driver computer prior to installation of the MS-ADOD Test Suite. 
   o  Visual Studio 2010 Ultimate (not included in the zip package)
   o  Protocol Test Suites Library (1.0.1379.0) 
   o  Spec Explorer (3.2.2498.0)

The following software is required on the Test Client Computer to analyze the network traces during the execution of test suite.
   o  Network Monitor (3.3.1641.0)
   o  Network Monitor Parser (03.04.2131.0001)


IV. Steps to setup software on Test Driver computer
----------------------------
Copy and extract test suite package onto the Test Driver computer.

On Test Suite Driver computer:
   o  Install Visual Studio 2010 Ultimate
   o  Install Microsoft Network Monitor (run Netmon.msi, optional)
   o  Install Microsoft Network Parser Package (run Microsoft_Parsers.msi, optional)
   o  Install Microsoft Protocol Test Suites Library (run ProtocolTestSuitesLibrary.msi)
   o  Install Microsoft Spec Explorer (run SpecExplorer.msi)
   o  Run MS-ADOD-TestSuite-ODEP.msi on Test Driver computer
      o Select "Install Test Suite on Test Driver Computer." during the installation
   Note: Microsoft Visual Studio 2010, Microsoft Protocol Test Suite Library and Microsoft Spec Explorer are required to be installed before running MS-RDPBCGR-TestSuite-ClientEP.msi on Test Client Computer.


V. Steps to setup software on client computer
----------------------------
Option 1: Steps to install Windows Client
   o Copy MS-ADOD-TestSuite-ODEP.msi onto the Windows Client Computer.
   o On Windows Client: 
     - Run MS-ADOD-TestSuite-ClientEP.msi on Client computer
     - Select "Install and configure Windows System Under Test (SUT)." during the installation
Option 2: Setup Non-Windows SUT
   o Please refer to MS-ADOD_QuickStartGuide.docx under Test Client computer C:\MicrosoftProtocolTests\MS-ADOD\OD-Endpoint\1.0.1398.0\docs\ to complete the configuration of Non-Windows SUT.


VI. Installation Contents
----------------------------
After installation, the following folders are created on the Test Driver computer under C:\MicrosoftProtocolTests\MS-ADOD\OD-Endpoint\1.0.1398.0\
   o  Batch 
      - Command files to run individual test cases or all test cases
   o  Bin 
      - Test Suite Binaries and config file
   o  Docs
        o [MS-ADOD].pdf 
          - Version of Technical Document the Test Suite is based on
        o MS-ADOD_ODQuickStartGuide.docx 
          - Steps to configure Test Client computer and SUT computer, and run test cases
        o MS-ADOD_ODTestEnvironmentDeploymentGuide.docx 
          - Deployment Guide to manually set up and configure the Windows SUT
        o MS-ADOD_ODReleaseNotes.docx  
          - Test Suite Release notes provides details on late Breaking Changes, Known Issues and other related information
        o MS-ADOD_ODRequirementSpec.xlsm 
          - The RS lists all of the requirements discovered during TD doc review. It discusses their validation and thus the Derived, Scenarios, Validation, and Validation Comments
        o MS-ADOD_ODTestDesignSpecification.docx 
          - It is an overview document that shows an understanding of the protocol and how it might be used, including relationships to other protocols, message flow, state diagrams, a short list of protocol properties, a test approach including justification and Adapter approach
	Note: Test Client Computer should have Microsoft Office or equivalent programs to view the documents. If not, please copy the docs to a computer that is able to read these files.	
   o  Scripts 
      - Scripts used to set up and configure Test Client computer and Windows SUT
   o  Source
      - Test suite source code
   o  EULA.rtf

After installation, the following folders are created on the Windows SUT under the root folder C:\MicrosoftProtocolTests\MS-ADOD\OD-Endpoint\1.0.1398.0\
   o  Scripts
      - Scripts used to set up and configure SUT machine
   o  EULA.rtf



VII. Configure and Run Test Cases
----------------------------
Please refer to MS-ADOD_ODQuickStartGuide.docx to run test cases.


VIII. Known Issues 
----------------------------  
Please refer to MS-ADOD_ODReleaseNote.docx for more details.


IX. License
----------------------------
Please refer to End User License Agreement (EULA).


X. Help
----------------------------
Any issues please contact dochelp@microsoft.com.


