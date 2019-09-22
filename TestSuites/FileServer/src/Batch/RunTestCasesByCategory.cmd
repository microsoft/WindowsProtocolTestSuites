:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
call RefreshTestConfig.cmd
echo:
echo Please select test categories below
echo    BVT                           All BVT test cases
echo    Smb302                        Test cases for features in SMB 3.02 dialect
echo    Smb30                         Test cases for features in SMB 3.0 dialect
echo    Smb21                         Test cases for features in SMB 2.1 dialect
echo    Smb2002                       Test cases for features in SMB 2.002 dialect
echo    Model                         All Model Based Test cases
echo    AppInstanceId                 Test cases for Create Context SMB2_CREATE_APP_INSTANCE_ID
echo    CreateClose                   Test cases for Create/Close
echo    Credit                        Test cases for Credit Management
echo    Encryption                    Test cases for Encryption
echo    DurableHandleV1BatchOplock    Test cases for Durable Handle v1 with batch oplock
echo    DurableHandleV1LeaseV1        Test cases for Durable Handle v1 with create context lease v1
echo    DurableHandleV2BatchOplock    Test cases for Durable Handle v2 with batch oplock
echo    DurableHandleV2LeaseV1        Test cases for Durable Handle v2 with create context lease v1
echo    DurableHandleV2LeaseV2        Test cases for Durable Handle v2 with create context lease v2
echo    PersistentHandle              Test cases for Persistent Handle
echo    LeasingV1                     Test cases for Leasing v1
echo    LeasingV2                     Test cases for Leasing v2
echo    DirectoryLeasing              Test cases to test server capability SMB2_GLOBAL_CAP_DIRECTORY_LEASING
echo    Negotiate                     Test cases Negotiate
echo    Replay                        Test cases Replay
echo    Session                       Test cases Session Management
echo    Signing                       Test cases Signing
echo    Tree                          Test cases Tree Management
echo    MultipleChannel               Test cases to test server capability SMB2_GLOBAL_CAP_MULTI_CHANNEL
echo    DFSC                          Test cases to test MS-DFSC protocol
echo    FSRVP                         Test cases to test MS-FSRVP protocol
echo    SWN                           Test cases to test MS-SWN protocol
echo    RsvdVersion1                  Test cases to test MS-RSVD version 1
echo    RsvdVersion2                  Test cases to test MS-RSVD version 2
echo    SQOS                          Test cases to test MS-SQOS protocol
echo    Auth                          Test cases to test all Authentication and Authorization scenarios
echo    KerberosAuthentication        Test cases for Kerberos Authentication
echo    FileAccessCheck               Test cases to test File Access Check scenario
echo    FolderAccessCheck             Test cases to test Folder Access Check scenario
echo    ShareAccessCheck              Test cases to test Share Access Check scenario
echo    CBAC                          Test cases to test Claim-Based Access Control scenario
echo    Cluster                       Test cases that require cluster environment
echo    FsctlLmrRequestResiliency     Test cases to test FSCTL/IOCTL code FSCTL_LMR_REQUEST_RESILIENCY
echo    FsctlFileLevelTrim            Test cases to test FSCTL/IOCTL code FSCTL_FILE_LEVEL_TRIM
echo    FsctlValidateNegotiateInfo    Test cases to test FSCTL/IOCTL code FSCTL_VALIDATE_NEGOTIATE_INFO
echo    FsctlOffloadReadWrite         Test cases to test FSCTL/IOCTL code FSCTL_OFFLOAD_READ and FSCTL_OFFLOAD_WRITE
echo    CombinedFeature               Test cases which cover more than 1 feature
echo    OperateOneFileFromTwoNodes    Test cases to test operate the same file from two nodes of the scaleout file server
echo    FsctlSetGetIntegrityInformation     Test cases to test FSCTL/IOCTL code FSCTL_GET_INTEGRITY_INFORMATION and FSCTL_SET_INTEGRITY_INFORMATION
echo    OplockOnShareWithoutForceLevel2OrSOFS  Test cases for Oplock on the share that does not include ForceLevel2Oplock or STYPE_CLUSTER_SOFS
echo    OplockOnShareWithoutForceLevel2WithSOFS  Test cases for Oplock on the share that does not include ForceLevel2Oplock but include STYPE_CLUSTER_SOFS
echo    OplockOnShareWithForceLevel2WithoutSOFS  Test cases for Oplock on the share that includes ForceLevel2Oplock but not include STYPE_CLUSTER_SOFS
echo    OplockOnShareWithForceLevel2AndSOFS  Test cases for Oplock on the share that includes ForceLevel2Oplock and STYPE_CLUSTER_SOFS

echo: 
echo To combine multiple categories, please use logical operators "|", "!" and "&":
echo Examples:
echo  BVT^&FSRVP    Execute test cases have categories "BVT" and "FSRVP"
echo  SWN^|Smb30    Execute test cases have categories "SWN" or "Smb30"
echo  BVT^&!SWN    Execute test cases have category "BVT" but do NOT have category "SWN"
echo:
set /P category=Set test categories you want to execute, default is "BVT" if nothing input:
if ("%category%") == ("") (
    set category=BVT
)
echo Test Category is "%category%"

set category="%category%"

REM Replace "!" to "#" because "!" is special char.
set category=%category:!=#%

REM Below codes translate format "BVT&FSRVP" to "TestCategory=BVT&TestCategory=FSRVP" as TestCaseFilter for VSTest.Console.exe
Setlocal ENABLEDELAYEDEXPANSION

set TestCaseFilter=
set IsCategoryChar=FALSE

:striploop  
    REM Get first char, like substring(0,1) in C#
    set stripchar=!category:~0,1!
    REM Get substring containing all chars after the first one, like substring(1) in C#
    set category=!category:~1!

    REM If last char is empty or NULL, goto ENDLOOP
    REM When last char is NULL, "set stripchar=!category:~0,1!" returns "~0,1"
    if "!stripchar!" EQU " " goto ENDLOOP
    if "!stripchar!" EQU "~0,1" goto ENDLOOP

    if '!stripchar!' EQU '^"' goto APPENDDELIMS
    if "!stripchar!" EQU "(" goto APPENDDELIMS
    if "!stripchar!" EQU ")"  goto APPENDDELIMS
    if "!stripchar!" EQU "&"  goto APPENDDELIMS
    if "!stripchar!" EQU "|"  goto APPENDDELIMS

    goto APPENDCATEGORY

REM append delims, such as "(", ")", "&", "|"
:APPENDDELIMS
    set TestCaseFilter=!TestCaseFilter!!stripchar!
    set IsCategoryChar=FALSE
    goto striploop 

REM append category
:APPENDCATEGORY
    if "!stripchar!" EQU "#" (
        set TestCaseFilter=!TestCaseFilter!TestCategory#=
        set IsCategoryChar=TRUE
        goto striploop
    )
    REM First category char
    if "!IsCategoryChar!" EQU "FALSE" (set TestCaseFilter=!TestCaseFilter!TestCategory=!stripchar!)
    if "!IsCategoryChar!" EQU "TRUE" (set TestCaseFilter=!TestCaseFilter!!stripchar!)
    set IsCategoryChar=TRUE
    goto striploop  

:ENDLOOP

setlocal disableDelayedExpansion

REM Replace "#" back to "!"
set TestCaseFilter=%TestCaseFilter:#=!%

echo TestCaseFilter is %TestCaseFilter%
echo.

REM Run test suite
%RunFileSharingTestSuite% /TestCaseFilter:%TestCaseFilter%

endlocal
pause