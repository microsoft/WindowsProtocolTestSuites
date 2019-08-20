# MS-FSA Protocol Server Test Design Specification 

## Contents

* [Introduction](#_Toc427488645)
* [MS-FSA Overview](#_Toc427488646)
* [Test Approach](#_Toc427488647)
    * [Test Suite Overview](#_Toc427488648)
    * [Test initialization and cleanup](#_Toc427488649)
* [MS-FSA Test Environment](#_Toc427488650)
* [Scenario and Test Case Summary](#_Toc427488651)
    * [Traditional Test cases](#_Toc427488652)
    * [MBT Test cases](#_Toc427488653)
* [Traditional Test Scenarios Design](#_Toc427488654)
    * [Scenarios for FileInformation](#_Toc427488822)
        * [FileInfo_IsCompressionSupported](#_Toc427488665)
        * [FileInfo_IsEASupported](#_Toc427488666)
        * [FileInfo_IsIntegritySupported](#_Toc427488667)
        * [FileInfo_IsFileLinkInfoSupported](#_Toc427488668)
        * [FileInfo_IsFileValidDateLengthInfoSupported](#_Toc427488669)    
        * [FileInfo_IsShortNameSupported](#_Toc427488663)
        * [FileInfo_FileIdInformationSupported](#FileInfo_FileIdInformationSupported)        
    * [Scenarios for FileSystemInformation](#_Toc427488823)
        * [FsInfo_FileFsAttributeInformation](#_Toc427488662)
        * [FsInfo_IsObjectIdSupported](#_Toc427488664)
        * [FsInfo_FileFsSizeInformation](#_Toc427488670)    
        * [FsInfo_Query_FileFsSectorSizeInformation](#_Toc427488660)        
    * [Scenarios for FsControlRequest](#_Toc427488824)   
        * [FsCtl_Get_IntegrityInformation](#_Toc427488656)
        * [FsCtl_Set_IntegrityInformation](#_Toc427488657)
        * [FsCtl_Offload_Read](#_Toc427488658)
        * [FsCtl_Offload_Write](#_Toc427488659)    
        * [FsCtl_IsEncryptionSupported](#_Toc427488672)
        * [FsCtl_IsAllocatedRangesSupported](#_Toc427488673)
        * [FsCtl_IsReparsePointSupported](#_Toc427488674)
        * [FsCtl_IsSparseFileSupported](#_Toc427488675)
        * [FsCtl_IsZeroDataSupported](#_Toc427488676)
        * [FsCtl_IsDuplicateExtentsToFileExSupported](#_Toc437488670)        
        * [FsCtl_Get_Volume_Data](#FsCtl-Get-Volume-Data)
        * [FsCtl_Query_File_Regions](#Fsctl-Query-File-Regions)
        * [FsCtl_Set_Compression](#FsCtl_Set_Compression)
    * [Scenarios for QuotaInformation](#Scenarios-for-QuotaInformation)       
        * [QuotaInfo_IsQuotaInfoSupported](#_Toc427488671)
    * [Scenarios for Alternate Data Stream](#_Toc427488826)
        * [AlternateDataStream_CreateStream](#_Toc427488823)
        * [AlternateDataStream_ListStreams](#_Toc427488824)
        * [AlternateDataStream_DeleteStream](#_Toc427488825)
        * [AlternateDataStream_RenameStream](#_Toc427488826)
        * [AlternateDataStream_WriteAndRead](#_Toc427488827)
        * [AlternateDataStream_LockAndUnlock](#_Toc427488828)
        * [AlternateDataStream_QueryAndSet_FileInformation](#_Toc427488829)
        * [AlternateDataStream_FsControl](#_Toc427488830)
    * [Other Scenarios](#Other-scenarios)
        * [CreateFile](#Other-scenarios-CreateFile)
* [Traditional Test Case Design](#_Toc427488677)
    * [Test cases for FileInformation](#Test-cases-for-FileInformation)
        * [IsEASupported](#_Toc427488721)
            * [FileInfo_Set_FileFullEaInformation_File_IsEASupported](#_Toc427488722)
            * [FileInfo_Set_FileFullEaInformation_Dir_IsEASupported](#_Toc427488723)
            * [FileInfo_Query_FileFullEaInformation_File_IsEASupported](#_Toc427488724)
            * [FileInfo_Query_FileFullEaInformation_Dir_IsEASupported](#_Toc427488725)
            * [FileInfo_Set_FileEaInformation_File_IsEASupported](#_Toc427488726)
            * [FileInfo_Set_FileEaInformation_Dir_IsEASupported](#_Toc427488727)
            * [FileInfo_Query_FileEaInformation_File_IsEASupported](#_Toc427488728)
            * [FileInfo_Query_FileEaInformation_Dir_IsEASupported](#_Toc427488729)    
        * [IsFileLinkInfoSupported](#_Toc427488739)
            * [FileInfo_Set_FileLinkInfo_File_IsFileLinkInfoSupported](#_Toc427488740)
            * [FileInfo_Set_FileLinkInfo_DIr_IsFileLinkInfoSupported](#_Toc427488741)     
        * [IsShortNameSupported](#IsShortNameSupported)
            * [FileInfo_Set_FileShortNameInfo_File_IsShortNameSupported](#_Toc427488703)
            * [FileInfo_Set_FileShortNameInfo_Dir_IsShortNameSupported](#_Toc427488704)
        * [IsCompressionSupported](#IsCompressionSupported)
            * [FileInfo_Query_FileCompressionInfo_File_IsCompressionSupported](#_Toc427488715)
            * [FileInfo_Query_FileCompressionInfo_Dir_IsCompressionSupported](#_Toc427488716)
        * [IsFileValidDataLengthInfoSupported](#_Toc427488742)
            * [FileInfo_Set_FileValidDataLengthInformation_File_IsSupported](#_Toc427488743)
            * [FileInfo_Set_FileValidDataLengthInformation_Dir_IsSupported](#_Toc427488744)     
        * [IsIntegritySupported](#_Toc427488730)                   
            * [FileInfo_Query_FileBasicInfo_File_IsIntegritySupported (BVT)](#_Toc427488733)
            * [FileInfo_Query_FileBasicInfo_Dir_IsIntegritySupported](#_Toc427488734)
            * [FileInfo_Query_FileAttributeTagInfo_File_IsIntegritySupported](#_Toc427488735)
            * [FileInfo_Query_FileAttributeTagInfo_Dir_IsIntegritySupported](#_Toc427488736)
            * [FileInfo_Query_FileNetworkOpenInfo_File_IsIntegritySupported](#_Toc427488737)
            * [FileInfo_Query_FileNetworkOpenInfo_Dir_IsIntegritySupported](#_Toc427488738)    
        * [IsFileIdInformationSupported](#IsFileIdInformationSupported)
            * [FileInfo_Query_FileIdInformation_File](#FileInfo_Query_FileIdInformation_File)
            * [FileInfo_Query_FileIdInformation_Dir](#FileInfo_Query_FileIdInformation_Dir)
    * [Test cases for FileSystemInformation](#Test-cases-for-FileSystemInformation)  
        * [IsObjectIdSupported](#_Toc427488705)
            * [FsInfo_Query_FileFsObjectIdInformation_File_IsObjectIdSupported (BVT)](#_Toc427488708)
            * [FsInfo_Query_FileFsObjectIdInformation_Dir_IsObjectIdSupported](#_Toc427488709)   
        * [IsFsSectorSizeInformationSupported](#IsFsSectorSizeInformationSupported)
            * [FsInfo_Query_FileFsSectorSizeInformation_File_OutputBufferSize(3TCs, 1BVT)](#_Toc427488695)
            * [FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputBufferSize(3TCs)](#_Toc427488696)
            * [FsInfo_Query_FileFsSectorSizeInformation_File_OutputValue_Common](#_Toc427488697)
            * [FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputValue_Common](#_Toc427488698)
            * [FsInfo_Set_FileFsSectorSizeInformation_File_InvalidInfoClass](#_Toc427488699)
            * [FsInfo_Set_FileFsSectorSizeInformation_Dir_InvalidInfoClass](#_Toc427488700)        
        * [IsFsAttributeInformationSupported](#IsFsAttributeInformationSupported)      
            * [FsInfo_Query_FileFsAttributeInformation_File_IsCompressionSupported(BVT)](#_Toc427488713)
            * [FsInfo_Query_FileFsAttributeInformation_Dir_IsCompressionSupported](#_Toc427488714)
            * [FsInfo_Query_FileFsAttributeInformation_File_IsIntegritySupported(BVT)](#_Toc427488731)
            * [FsInfo_Query_FileFsAttributeInformation_Dir_IsIntegritySupported](#_Toc427488732)     
            * [FsInfo_Query_FileFsAttributeInformation_File_IsEncryptionSupported(BVT)](#_Toc427488752)
            * [FsInfo_Query_FileFsAttributeInformation_Dir_IsEncryptionSupported](#_Toc427488753)            
            * [FsInfo_Query_FileFsAttributeInformation_File_IsObjectIdSupported(BVT)](#_Toc427488706)
            * [FsInfo_Query_FileFsAttributeInformation_Dir_IsObjectIdSupported](#_Toc427488707)            
        * [IsFsSizeInformationSupported](#IsFsSizeInformationSupported)
            * [FsInfo_Query_FileFsSizeInformation_File_SectorsPerAllocationUnit (BVT)](#_Toc427488746)
            * [FsInfo_Query_FileFsSizeInformation_Dir_SectorsPerAllocationUnit](#_Toc427488747)
    * [Test cases for FsControlRequest](#Test-cases-for-FsControlRequest)
        * [IsAllocatedRangesSupported](#_Toc427488756)
            * [FsCtl_Query_AllocatedRanges_File_IsAllocatedRangesSupported](#_Toc427488757)
            * [FsCtl_Query_AllocatedRanges_Dir_IsAllocatedRangesSupported](#_Toc427488758)
        * [IsReparsePointSupported](#_Toc427488759)
            * [FsCtl_Get_ReparsePoint_IsReparsePointSupported](#_Toc427488760)
        * [IsSparseFileSupported](#_Toc427488762)
            * [FsCtl_Set_Sparse_File_IsSparseFileSupported](#_Toc427488763)
            * [FsCtl_Set_Sparse_Dir_IsSparseFileSupported](#_Toc427488764)
        * [IsZeroDataSupported](#_Toc427488765)
            * [FsCtl_Set_ZeroData_File_IsSetZeroDataSupported](#_Toc427488766)
            * [FsCtl_Set_ZeroData_Dir_IsZeroDataSupported](#_Toc427488767)
        * [FileNormalizedNameInformation](#_Toc427488831)
            * [FileInfo_Query_FileNormalizedNameInfo_File](#_Toc427488832)
            * [FileInfo_Query_FileNormalizedNameInfo_Dir](#_Toc427488833)
        * [IsDuplicateExtentsToFileExSupported](#_Toc437488671)
            * [BVT_FsCtl_DuplicateExtentsToFileEx_IsBasicSupported](#_Toc437488672)
            * [BVT_FsCtl_DuplicateExtentsToFileEx_IsSourceAtomicSupported](#_Toc437488673)    
        * [IsGetIntegrityInformationSupported](#IsIntegrityInformationSupported)
            * [FsCtl_Get_IntegrityInformation_File_IsIntegritySupported (BVT)](#_Toc427488679)
            * [FsCtl_Get_IntegrityInformation_Dir_IsIntegritySupported (BVT)](#_Toc427488680)
            * [FsCtl_Get_IntegrityInformation_File_InvalidParameter(3TCs)](#_Toc427488681)
            * [FsCtl_Get_IntegrityInformation_Dir_InvalidParameter(3TCs)](#_Toc427488682)
            * [FsCtl_Get_IntegrityInformation_File_OutputValue(2TCs)](#_Toc427488683)
            * [FsCtl_Get_IntegrityInformation_Dir_OutputValue(2TCs)](#_Toc427488684)
        * [IsSetIntegrityInformationSupported](#IsSetIntegrityInformationSupported)   
            * [FsCtl_Set_IntegrityInformation_File_IsIntegritySupported (BVT)](#_Toc427488685)
            * [FsCtl_Set_IntegrityInformation_Dir_IsIntegritySupported (BVT)](#_Toc427488686)
            * [FsCtl_Set_IntegrityInformation_File_InvalidParameter(2TCs)](#_Toc427488687)
            * [FsCtl_Set_IntegrityInformation_Dir_InvalidParameter(2TCs)](#_Toc427488688)
            * [FsCtl_Set_IntegrityInformation_File_WriteProtected](#_Toc427488689)
            * [FsCtl_Set_IntegrityInformation_Dir_WriteProtected](#_Toc427488690)
            * [FsCtl_Set_IntegrityInformation_File_ChecksumAlgorithm(2TCs)](#_Toc427488691)
            * [FsCtl_Set_IntegrityInformation_Dir_ChecksumAlgorithm(2TCs)](#_Toc427488692)
        * [IsOffloadReadSupported](#IsOffloadReadSupported)
            * [FsCtl_Offload_Read_File_IsOffloadSupported (BVT)](#_Toc427488693)      
        * [IsOffloadWriteSupported](#IsOffloadWriteSupported)      
            * [FsCtl_Offload_Write_File_IsOffloadSupported (BVT)](#_Toc427488694)        
        * [IsSetCompressionSupported](#IsSetCompressionSupported)
            * [FsCtl_Set_Compression_File_IsCompressionSupported](#_Toc427488719)
            * [FsCtl_Set_Compression_Dir_IsCompressionSupported](#_Toc427488720)    
        * [IsGetCompressionSupported](#IsGetCompressionSupported)
            * [FsCtl_Get_Compression_File_IsCompressionSupported](#_Toc427488717)
            * [FsCtl_Get_Compression_Dir_IsCompressionSupported](#_Toc427488718)          
        * [IsGetVolumeDataSupported](#IsGetVolumeDataSupported)
            * [FsCtl_Get_REFS_Volume_Data_File](#FsCtl_Get_REFS_Volume_Data_File)
            * [FsCtl_Get_REFS_Volume_Data_Dir](#FsCtl_Get_REFS_Volume_Data_Dir)
            * [FsCtl_Get_NTFS_Volume_Data_File](#FsCtl_Get_NTFS_Volume_Data_File)
            * [FsCtl_Get_NTFS_Volume_Data_Dir](#FsCtl_Get_NTFS_Volume_Data_Dir)
        * [IsQueryFileRegionsSupported](#IsQueryFileRegionsSupported)
            * [BVT_FsCtl_Query_File_Regions](#BVT_FsCtl_Query_File_Regions)
            * [BVT_FsCtl_Query_File_Regions_WithInputData](#BVT_FsCtl_Query_File_Regions_WithInputData)
    * [Test cases for QuotaInformation](#Test-cases-for-QuotaInformation)
        * [IsQuotaInfoSupported](#_Toc427488748)
            * [QuotaInfo_Query_QuotaInformation_IsQuotaInfoSupported](#_Toc427488749)
            * [QuotaInfo_Set_QuotaInformation_IsQuotaInfoSupported](#_Toc427488750)
    * [Test cases for Alternate Data Stream](#_Toc427488768)
        * [IsCreateStreamSupported](#IsCreateStreamSupported)
            * [BVT_AlternateDataStream_CreateStream_File (BVT)](#_Toc427488770)
            * [BVT_AlternateDataStream_CreateStream_Dir (BVT)](#_Toc427488771)
        * [IsListStreamsSupported](#IsListStreamsSupported)
            * [BVT_AlternateDataStream_ListStreams_File (BVT)](#_Toc427488773)
            * [BVT_AlternateDataStream_ListStreams_Dir (BVT)](#_Toc427488774)
        * [IsDeleteStreamSupported](#IsDeleteStreamSupported)
            * [BVT_AlternateDataStream_DeleteStream_File (BVT)](#_Toc427488776)
            * [BVT_AlternateDataStream_DeleteStream_Dir (BVT)](#_Toc427488777)
        * [IsRenameStreamSupported](#IsRenameStreamSupported)
            * [BVT_AlternateDataStream_RenameStream_File (BVT)](#_Toc427488779)
            * [BVT_AlternateDataStream_RenameStream_Dir (BVT)](#_Toc427488780)
        * [IsWriteAndReadSupported](#IsWriteAndReadSupported)
            * [BVT_AlternateDataStream_WriteAndRead_File (BVT)](#_Toc427488782)
            * [BVT_AlternateDataStream_WriteAndRead_Dir (BVT)](#_Toc427488783)
        * [IsLockAndUnlockSupported](#IsLockAndUnlockSupported)
            * [BVT_AlternateDataStream_LockAndUnlock_File (BVT)](#_Toc427488785)
            * [BVT_AlternateDataStream_LockAndUnlock_Dir (BVT)](#_Toc427488786)
        * [IsQueryAndSetFileInformationSupported](#IsQueryAndSetFileInformationSupported)
            * [AlternateDataStream_Query_FileAccessInformation_File](#_Toc427488788)
            * [AlternateDataStream_Query_FileAccessInformation_Dir](#_Toc427488789)
            * [AlternateDataStream_Query_FileBasicInformation_File](#_Toc427488790)
            * [AlternateDataStream_Query_FileBasicInformation_Dir](#_Toc427488791)
            * [AlternateDataStream_Query_FileCompressionInformation_File](#_Toc427488792)
            * [AlternateDataStream_Query_FileCompressionInformation_Dir](#_Toc427488793)
            * [AlternateDataStream_Query_FileNetworkOpenInformation_File](#_Toc427488794)
            * [AlternateDataStream_Query_FileNetworkOpenInformation_Dir](#_Toc427488795)
            * [AlternateDataStream_Query_FileStandardInformation_File](#_Toc427488796)
            * [AlternateDataStream_Query_FileStandardInformation_Dir](#_Toc427488797)
            * [AlternateDataStream_Set_FileEaInformation_File](#_Toc427488798)
            * [AlternateDataStream_Set_FileEaInformation_Dir](#_Toc427488799)
            * [AlternateDataStream_Set_FileShortNameInformation_File](#_Toc427488800)
            * [AlternateDataStream_Set_FileShortNameInformation_Dir](#_Toc427488801)
            * [AlternateDataStream_Set_FileValidDataLengthInformation_File](#_Toc427488802)
            * [AlternateDataStream_Set_FileValidDataLengthInformation_Dir](#_Toc427488803)
        * [IsFsControlSupported](#IsFsControlSupported)
            * [AlternateDataStream_FsCtl_Get_Compression_File](#_Toc427488805)
            * [AlternateDataStream_FsCtl_Get_Compression_Dir](#_Toc427488806)
            * [AlternateDataStream_FsCtl_Get_IntegrityInformation_File](#_Toc427488807)
            * [AlternateDataStream_FsCtl_Get_IntegrityInformation_Dir](#_Toc427488808)
            * [AlternateDataStream_FsCtl_Query_AllocatedRanges_File](#_Toc427488809)
            * [AlternateDataStream_FsCtl_Query_AllocatedRanges_Dir](#_Toc427488810)
            * [AlternateDataStream_FsCtl_Set_Compression_File](#_Toc427488811)
            * [AlternateDataStream_FsCtl_Set_Compression_Dir](#_Toc427488812)
            * [AlternateDataStream_FsCtl_Set_ZeroData_File](#_Toc427488813)
            * [AlternateDataStream_FsCtl_Set_ZeroData_Dir](#_Toc427488814)
    * [Test cases for Create and Query Directory](#Test-cases-for-create-and-query-directory)
        * [QueryDirectory](#QueryDirectory)
            * [Fs_CreateDirectory_QueryDirectory_Suffix_I30_INDEX_ALLOCATION](#_Toc427488836)
            * [Fs_CreateDiretory_QueryDirectory_Suffix_INDEX_ALLOCATION](#_Toc427488837)
    * [Test cases for Other Scenarios](#Test-cases-for-other-scenarios)
        * [IsCreateFileSupported](#IsCreateFileSupported)
            * [CreateFile_InvalidStreamName](#_Toc427488835)
* [MBT Test Design](#_Toc427488815)
    * [Model Design](#_Toc427488816)
    * [Adapter Design](#_Toc427488817)
        * [Protocol Adapter](#_Toc427488818)
        * [Transport Adapter](#_Toc427488819)
        * [Message Sequence](#_Toc427488820)
    * [Scenarios](#_Toc427488821)

## <a name="_Toc427488645"/>Introduction
This document provides information about how MS-FSA test suite is designed to test MS-FSA technical document usability and accuracy. It gives the analysis of MS-FSA technical document content, and describes test assumptions, scope and constraints of the test suite. It also specifies test approach, test scenarios, detail test cases, test suite architecture and adapter design.

## <a name="_Toc427488646"/>MS-FSA Overview

MS-FSA is algorithm technical document, with detail algorithm implementation. Below diagram shows the basic 11 operations for MS-FSA.

![image6.png](./image/MS-FSA_ServerTestDesignSpecification/image6.png)

## <a name="_Toc427488647"/>Test Approach
In MS-FSA test suite, MBT test method and traditional approach will work together.

* **MBT**

* Model-based test will mostly cover the return codes for the algorithm operations. 



* **Traditional**

* Traditional test is used to cover some negative test cases, and the object store changes during operations, and make sure if-condition are covered.

* This document will cover only traditional scenario and test cases.

### <a name="_Toc427488648"/>Test Suite Overview
The following flow charts show the overview for MS-FSA test suite design.

![image7.png](./image/MS-FSA_ServerTestDesignSpecification/image7.png)

### <a name="_Toc427488649"/>Test initialization and cleanup
In the test design, MS-FSA uses a common test initialization and cleanup method, which will be used commonly in the test cases:

* FsaInitial: It includes select transport, connect, negotiate, session setup and tree connect.

* Cleanup: Logoff and disconnect from server.




## <a name="_Toc427488650"/>MS-FSA Test Environment
The following diagram shows the basic test environment for MS-FSA. The DC01 is optional.


![image2.png](./image/MS-FSA_ServerTestDesignSpecification/image2.png)




## <a name="_Toc427488651"/>Scenario and Test Case Summary

### <a name="_Toc427488652"/>Traditional Test cases
Traditional Test cases are designed specific to Win8 new algorithms, REFS file system and Alternate Data Stream.
There are 126 test cases in total:


|  **Category**|  **Scenarios**|  **Test cases (BVT)**| 
| -------------| -------------| ------------- |
| Scenarios for FileInformation| 7| 26 (5)| 
| Scenarios for FileSytemInformation| 4| 22 (7)| 
| Scenarios for FsControlRequest| 13| 44 (14)| 
| Scenarios for Alternate Data Stream| 19|38 (12)| 
| Scenarios for QuotaInformation| 1|2(0)|
| Other Scenarios| 1| 1|



### <a name="_Toc427488653"/>MBT Test cases
Model-based test cases are designed to cover most of algorithm details.
There are 344 test cases in total:


| **Category** | **Test Cases** | 
| ------------ | -------------- |
| Open | 31 | 
| Create | 25 | 
| Read | 3 | 
| Write | 1 | 
| Close | 1 | 
| Flush Cached Data | 1 | 
| Lock and Unlock | 2 | 
| Change Notification | 2 | 
| IoCtl Request | 71 | 
| Query Directory | 20 | 
| Query FileInfo | 44 | 
| Query FsInfo | 24 | 
| Query SecurityInfo | 12 | 
| Set FileInfo | 89 | 
| Set SecurityInfo | 18 | 


## <a name="_Toc427488654"/>Traditional Test Scenarios Design

### <a name="_Toc427488655"/>Scenarios for FileInformation


#### <a name="_Toc427488665"/>FileInfo_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FileInfoClass: FileCompressionInformation| 
| | If the object store does not implement this functionality, the operation MUST be failed with **STATUS_** **INVALID_PARAMETER**| 
| | If supported, The operation returns **STATUS_SUCCESS**.| 
| Message Sequence| CreateFile | 
| | SetInfo for FileInfoClass.FileCompressionInformation| 
| | Verify server return with **STATUS_SUCCESS** for supported file system| 
| | Or failed the request with **STATUS_NOT_SUPPORTED**| 



#### <a name="_Toc427488666"/>FileInfo_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.| 
| | Note: Only NTFS implements EAs.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Info class: FileFullEaInformation, FileEaInformation| 
| | If the object store does not support extended attribute, the EA size should be 0. | 
| | If supported, the EA size should be greater than 0.| 
| Message Sequence| CreateFile | 
| | SetInfo for FileInfoClass.FileFullEaInformation or FileEaInformation| 
| | Verify server return EA size  &#62; 0 for supported file system| 
| | Or EA size == 0 for unsupported file system.| 



#### <a name="_Toc427488667"/>FileInfo_IsIntegritySupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if integrity is supported for different file systems.| 
| | Note: Only ReFS supports integrity.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Info class: FileBasicInformation, FileAttributeTagInformation, FileNetworkOpenInformation| 
| | If the object store support this functionality, FILE_ATTRIBUTE_INTEGRITY_STREAM  is set in OutputBuffer.FileAttributes.| 
| | If not support, FILE_ATTRIBUTE_INTEGRITY_STREAM  is not set in OutputBuffer.FileAttributes.| 
| Message Sequence| CreateFile| 
| | QueryInfo with FileInfoClass = FileAttributeTagInformation/FileBasicInformation/FileNetworkOpenInformation| 
| | Verify the FILE_ATTRIBUTE_INTEGRITY_STREAM  is set or not in OutputBuffer.FileAttributes.| 


#### <a name="_Toc427488668"/>FileInfo_IsFileLinkInfoSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FileLinkInformation is supported for different file systems.| 
| | Note: Only NTFS supports FileLinkInformation| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Info class: FileLinkInformation| 
| | If the object store does not support this functionality, the operation failed with STATUS_NOT_SUPPORTED| 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | SetInfo with FileInfoClass.FileLinkInformation | 
| | Verify server responses accordingly.| 


#### <a name="_Toc427488669"/>FileInfo_IsFileValidDateLengthInfoSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test the FileValidDataLengthInformation for different file systems.| 
| | Test environment: NTFS, ReFS, FAT32| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Info class: FileValidDataLengthInformation| 
| | If the object store does not support this functionality, the operation failed with STATUS_INVALID_PARAMETER.| 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | SetInfo with FileInfoClass = FileValidDataLengthInformation | 
| | Verify server responses accordingly.| 



#### <a name="_Toc427488663"/>FileInfo_IsShortNameSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if short name supported for different file system.| 
| | Note: ReFS does not implement short names.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FileInfoClass: FileShortNameInformation| 
| | If not supported, the operation failed with STATUS_INVALID_PARAMETER| 
| | If supported, The operation returns **STATUS_SUCCESS**. |
| Message Sequence| CreateFile | 
| | SetInfo with FileInfoClass.FileShortNameInformation| 
| | Verify server response with ShortName info for supported file system| 
| | Or failed the request for unsupported file system.| 


#### <a name="FileInfo_FileIdInformationSupported"/>FileInfo_FileIdInformationSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FileIdInformation supported for different file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FileInfoClass: FileIdInformation| 
| | If supported, The operation returns **STATUS_SUCCESS**.| 
| | And FileId of FileIdInformation is verified by sending FSCTL_READ_FILE_USN_DATA, VolumeSerialNumber of FileIdInformation is verified by sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA. | 
| Message Sequence| CreateFile | 
| | QueryInfo with FileInfoClass.FileIdInformation|
| | Get FileReferenceNumber by sending FSCTL_READ_FILE_USN_DATA to server.| 
| | Verify FileId when querying FileIdInformation should be the same with FileReferenceNumber when sending FSCTL_READ_FILE_USN_DATA.|
| | Get VolumeSerialNumber by sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA to server.|
| | Verify VolumeSerialNumber when querying FileIdInformation should be the same with VolumeSerialNumber when sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA.|



### <a name="_Toc427488823"/>Scenarios for FileSystemInformation


#### <a name="_Toc427488662"/>FsInfo_FileFsAttributeInformation

| &#32;| &#32; |
| -------------| ------------- |
| Description| To query FileFsAttributeInformation for supported features in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage for File attributes| 
| | FILE_SUPPORTS_ENCRYPTION| 
| | FILE_SUPPORTS_OBJECT_IDS| 
| | FILE_SUPPORTS_REPARSE_POINTS| 
| | FILE_SUPPORTS_SPARSE_FILES| 
| | FILE_FILE_COMPRESSION| 
| | FILE_SUPPORT_INTEGRITY_STREAMS| 
| Message Sequence| CreateFile | 
| | QueryInfo with FsInfoClass.FileFsAttributeInformation| 
| | Verify the file system attribute is set correctly.| 


#### <a name="_Toc427488664"/>FsInfo_IsObjectIdSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if object id is supported for different file systems.| 
| | Note: ReFS does not support object IDs.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FsInfoClass: FileObjectIdInformation| 
| | If the object store does not implement this functionality, the operation MUST be failed with **STATUS_INVALID_PARAMETER**| 
| | If supported, the operation returns **STATUS_SUCCESS**.| 
| Message Sequence| CreateFile | 
| | SetInfo with FsInfoClass.FileObjectIdInformation| 
| | Verify server return with **STATUS_SUCCESS** for supported file system| 
| | Or failed the request with **STATUS_INVALID_PARAMETER**| 



#### <a name="_Toc427488670"/>FsInfo_FileFsSizeInformation_ClusterSize

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test cluster size for different file system.| 
| | For ClusterSize:| 
| |     NTFS uses a default cluster size of 4k | 
| | ReFS uses a default cluster size of 64k| 
| | For LogicalBytesPerSector: | 
| | MUST be greater than or equal to 512 bytes| 
| | For FileFsSizeInformation:| 
| | OutputBuffer.SectorsPerAllocationUnit set to Open.File.Volume.ClusterSize / Open.File.Volume.LogicalBytesPerSector| 
| | So for NTFS, SectorsPerAllocationUnit = 4K/512 bytes = 8| 
| | For ReFS, SectorsPerAllocationUnit = 64K/512 bytes = 128| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Info class: FileValidDataLengthInformation| 
| | For NTFS, SectorsPerAllocationUnit = 4K/512 bytes = 8| 
| | For ReFS, SectorsPerAllocationUnit = 64K/512 bytes = 128.| 
| Message Sequence| CreateFile| 
| | QueryInfo with FsInfoClass. FileFsSizeInformation| 
| | Verify the outputbuffer. SectorsPerAllocationUnit is correctly set according to default cluster size.| 




#### <a name="_Toc427488660"/>FsInfo_Query_FileFsSectorSizeInformation

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test query file system info with FsInfoClass.FileFsSectorSizeInformation| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Input parameter test: | 
| | If OutputBufferSize is smaller than sizeof(FILE_FS_SECTOR_SIZE_INFORMATION), the operation MUST be failed with **STATUS_INFO_LENGTH_MISMATCH** | 
| | Operation test: | 
| | Upon successful completion of the operation, returns **STATUS_SUCCESS**.| 
| | Verify each data element in OutputBuffer for FILE_FS_SECTOR_SIZE_INFORMATION structure.| 
| Message Sequence| CreateFile | 
| | QueryInfo with FsInfoClass.FileFsSectorSizeInformation.| 
| | Verify server responses accordingly to the input parameters.| 



### <a name="_Toc427488824"/>Scenarios for FsControlRequest


#### <a name="_Toc427488656"/>FsCtl_Get_IntegrityInformation

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  FSCTL request:**FSCTL_GET_INTEGRITY_INFORMATION**| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Supporting test: | 
| | If not implemented, failed with **STATUS_INVALID_DEVICE_REQUEST**.| 
| | Input parameter test:| 
| | The operation MUST be failed with **STATUS_INVALID_PARAMETER** under any of the following conditions:| 
| | OutputBufferSize is less thansizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER).| 
| | Open.Stream.StreamType is not DirectoryStream or FileStream.| 
| | Open.File.FileAttributes.FILE_ATTRIBUTE_SYSTEM is TRUE.| 
| | Operation test: | 
| | Upon successful completion of the operation, server returns **STATUS_SUCCESS**.| 
| | Verify OutputBuffer.CheckSumAlgorithm is set correctly.| 
| | Verify OutputBuffer.ChecksumChunkShift is set correctly.| 
| | Verify OutputBuffer.ClusterShift is set correctly.| 
| | Verify OutputBuffer.Flags for CHECKSUM_ENFORCEMENT_OFF is set correctly.| 
| | Verify ByteCount ==sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER).| 
| Message Sequence| CreateFile| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Verify server responses accordingly to input parameters.| 


#### <a name="_Toc427488657"/>FsCtl_Set_IntegrityInformation

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  FSCTL request: FSCTL_GET_INTEGRITY_INFORMATION| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Supporting test: | 
| | If not implemented, failed with **STATUS_INVALID_DEVICE_REQUEST**.| 
| | Input parameter test:| 
| | The operation MUST be failed with **STATUS_INVALID_PARAMETER** under any of the following conditions:| 
| | OutputBufferSize is less thansizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER).| 
| | Open.Stream.StreamType is not DirectoryStream or FileStream.| 
| | Open.File.FileAttributes.FILE_ATTRIBUTE_SYSTEM is TRUE.| 
| | Operation test: | 
| | Upon successful completion of the operation, server returns **STATUS_SUCCESS**.| 
| | Verify OutputBuffer.CheckSumAlgorithm is set correctly.| 
| | Verify OutputBuffer.ChecksumChunkShift is set correctly.| 
| | Verify OutputBuffer.ClusterShift is set correctly.| 
| | Verify OutputBuffer.Flags for CHECKSUM_ENFORCEMENT_OFF is set correctly.| 
| | Verify ByteCount ==sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER).| 
| Message Sequence| CreateFile | 
| | FSCTL request with FSCTL_SET_INTEGRITY_INFORMATION| 
| | Verify server responses accordingly to input parameters.| 

#### <a name="_Toc427488658"/>FsCtl_Offload_Read

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  FSCTL request: **FSCTL_OFFLOAD_READ**| 
| | Note:Support for this read operation is optional.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Supporting test: | 
| | If not implement, failed with **STATUS_INVALID_DEVICE_REQUEST**.| 
| | Input parameter test: | 
| | Test with different parameter, verify server returns different status code accordingly: **STATUS_NOT_SUPPORTED**, **STATUS_BUFFER_TOO_SMALL**, **STATUS_INVALID_PARAMETER**, **STATUS_SUCCESS**, **STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED**, **STATUS_FILE_DELETED** **,**  **STATUS_END_OF_FILE** **,**  **OFFLOAD_READ_FLAG_ALL_ZERO_BEYOND_CURRENT_RANGE**| 
| | Operation test: | 
| | Upon successful completion of the operation, returns **STATUS_SUCCESS**.| 
| | **BytesReturned** set to OutputBufferLength| 
| Message Sequence| CreateFile | 
| | FSCTL request with  FSCTL_OFFLOAD_READ| 
| | Verify server responses accordingly to input parameters.| 


#### <a name="_Toc427488659"/>FsCtl_Offload_Write

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  FSCTL request: FSCTL_OFFLOAD_WRITE| 
| | Note:Support for this read operation is optional.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | Supporting test: | 
| | If not implement, failed with STATUS_INVALID_DEVICE_REQUEST.| 
| | Input parameter test: | 
| | Test with different parameter, verify server returns different status code accordingly:**STATUS_NOT_SUPPORTED**, **STATUS_BUFFER_TOO_SMALL**, **STATUS_INVALID_PARAMETER**, **STATUS_SUCCESS**, **STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED**, **STATUS_FILE_DELETED** **,**  **STATUS_END_OF_FILE** **,**  **OFFLOAD_READ_FLAG_ALL_ZERO_BEYOND_CURRENT_RANGE**| 
| | Operation test: | 
| | Upon successful completion of the operation, returns **STATUS_SUCCESS**.| 
| | **BytesReturned** set to OutputBufferLength| 
| Message Sequence| CreateFile | 
| | FSCTL request with  FSCTL_OFFLOAD_Write| 
| | Verify server responses correctly.| 


#### <a name="_Toc427488672"/> FsCtl_IsEncryptionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if encryption is supported for different file system| 
| | Note: This is only implemented by the NTFS file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_SET_ENCRYPTION| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | FsCtl request with FSCTL_SET_ENCRYPTION| 
| | Verify server returns STATUS_SUCCESS for supported file system| 
| | Or returns STATUS_INVALID_DEVICE_REQUEST for unsupported file system.| 



#### <a name="_Toc427488673"/> FsCtl_IsAllocatedRangesSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_QUERY_ALLOCATED_RANGES is supported for different file systems.| 
| | Note: This is only implemented by the ReFS and NTFS file systems.| 
| | Test environment: NTFS, ReFS, FAT32| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_QUERY_ALLOCATED_RANGES| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | FsCtl request with FSCTL_QUERY_ALLOCATED_RANGES| 
| | Verify server returns STATUS_SUCCESS for supported file system| 
| | Or returns STATUS_INVALID_DEVICE_REQUEST for unsupported file system.| 



#### <a name="_Toc427488674"/> FsCtl_IsReparsePointSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_GET_REPARSE_POINT is supported for different file systems.| 
| | Note: This is only implemented by the ReFS and NTFS file systems.| 
| | Test environment: NTFS, ReFS, FAT32| 
| | Test object: DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_GET_REPARSE_POINT| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | FsCtl request with FSCTL_GET_REPARSE_POINT| 
| | Verify server returns STATUS_SUCCESS for supported file system| 
| | Or returns STATUS_INVALID_DEVICE_REQUEST for unsupported file system.| 



#### <a name="_Toc427488675"/> FsCtl_IsSparseFileSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_SET_SPARSE is supported for different file systems.| 
| | Note: This is only implemented by the **NTFS** file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_SET_SPARSE| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | FsCtl request with FSCTL_SET_SPARSE| 
| | Verify server returns STATUS_SUCCESS for supported file system| 
| | Or returns STATUS_INVALID_DEVICE_REQUEST for unsupported file system.| 



#### <a name="_Toc427488676"/> FsCtl_IsZeroDataSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_SET_ZERO_DATA is supported for different file systems.| 
| | Note: This is only implemented by the **NTFS** file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_SET_ZERO_DATA| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | FsCtl request with FSCTL_SET_ZERO_DATA| 
| | Verify server returns STATUS_SUCCESS for supported file system| 
| | Or returns STATUS_INVALID_DEVICE_REQUEST for unsupported file system.| 




#### <a name="_Toc437488670"/> FsCtl_IsDuplicateExtentsToFileExSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX with DUPLICATE_EXTENTS_DATA_EX_SOURCE_ATOMIC is supported.| 
| | Note: This is only implemented by the **REFS** file system.| 
| | Test environment: ReFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX| 
| Message Sequence| Create test file and fill up first two clusters with random data.| 
| | FSCTL request with FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX and different additional flags to duplicate the file extent of first cluster to the second.| 
| | Verify returned NTSTATUS code and file content by checking the following:| 
| | 1. The returned status is STATUS_SUCCESS.| 
| | 2. The file content of the second cluster is the same as the first.| 


#### <a name="FsCtl_Get_Volume_Data"/> FsCtl_Get_Volume_Data

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_GET_REFS_VOLUME_DATA/FSCTL_GET_NTFS_VOLUME_DATA is supported.| 
| | Note: This is only implemented by the **REFS** file system and **NTFS** file system.| 
| | Test environment: ReFS and NTFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_GET_REFS_VOLUME_DATA and FSCTL_GET_NTFS_VOLUME_DATA| 
| Message Sequence| Create test file.| 
| | Query FileIdInformation.|
| | Query FileFsFullSizeInformation.|
| | FSCTL request with FSCTL_GET_NTFS_VOLUME_DATA/FSCTL_GET_REFS_VOLUME_DATA.| 
| | Verify returned STATUS_SUCCESS and Verify returned REFS_VOLUME_DATA_BUFFER or NTFS_VOLUME_DATA_BUFFER:| 
| | 1. VolumeSerialNumber of VOLUME_DATA_BUFFER should be the same with VolumeSerialNumber of FileIdInformation.| 
| | 2. TotalClusters of VOLUME_DATA_BUFFER should be the same with TotalAllocationUnits of FileFsFullSizeInformation.| 
| | 3. BytesPerSector of VOLUME_DATA_BUFFER should be the same with BytesPerSector of FileFsFullSizeInformation.|


#### <a name="FsCtl_Query_File_Regions"/> FsCtl_Query_File_Regions

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_QUERY_FILE_REGIONS is supported.| 
| | Note: This is only implemented by the **REFS** file system and **NTFS** file system.| 
| | Test environment: ReFS and NTFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_QUERY_FILE_REGIONS| 
| Message Sequence| Create test file and fill it up with random data.| 
| | FSCTL request with FSCTL_QUERY_FILE_REGIONS (containing or not containing input data).| 
| | Verify returned STATUS_SUCCESS and Verify returned FILE_REGION_OUTPUT:| 
| | 1. RegionEntryCount of the output data element should be 1.| 
| | 2. TotalRegionEntryCount of the output data element should be 1.| 
| | 3. FileOffset of the FILE_REGION_INFO data element should be zero.|
| | 4. Length of the FILE_REGION_INFO data element should be the size we wrote to the file.|
| | 5. FILE_REGION_USAGE_VALID_CACHED_DATA should be set for NTFS, or FILE_REGION_USAGE_VALID_NONCACHED_DATA should be set for REFS.|


### <a name="Scenarios-for-QuotaInformation"/>Scenarios for QuotaInformation


#### <a name="_Toc427488671"/> QuotaInfo_IsQuotaInfoSupported

| &#32;| &#32; |
| -------------| ------------- |
| Scenario|  **QuotaInfo_IsSupported**| 
| Description| To test if quota info is supported for different file systems.| 
| | Note: Only NTFS supports quotas.| 
| | Test environment: NTFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | Query Quota Information| 
| | Verify server responses STATUS_SUCCESS/ STATUS_INVALID_DEVICE_REQUEST accordingly.| 



### <a name="_Toc427488822"/>Scenarios for Alternate Data Stream


#### <a name="_Toc427488823"/>AlternateDataStream_CreateStream

| &#32;| &#32; |
| -------------| ------------- |
| Description| To create Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Verify server return with **STATUS_SUCCESS** for supported file system|  


#### <a name="_Toc427488824"/>AlternateDataStream_ListStreams

| &#32;| &#32; |
| -------------| ------------- |
| Description| To list the Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify server return with streamname and streamsize|


#### <a name="_Toc427488825"/>AlternateDataStream_DeleteStream

| &#32;| &#32; |
| -------------| ------------- |
| Description| To delete the Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Delete the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|


#### <a name="_Toc427488826"/>AlternateDataStream_RenameStream

| &#32;| &#32; |
| -------------| ------------- |
| Description| To rename the Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|
| | Rename the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|


#### <a name="_Toc427488827"/>AlternateDataStream_WriteAndRead

| &#32;| &#32; |
| -------------| ------------- |
| Description| To write and read from the Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Read the bytes from the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify the bytes read and the bytes written on this stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Read the bytes from the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify the bytes read and the bytes written on this stream|


#### <a name="_Toc427488828"/>AlternateDataStream_LockAndUnlock

| &#32;| &#32; |
| -------------| ------------- |
| Description| To lock and unlock a byte range of the Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Lock a byte range of the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Unlock the byte range of the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|


#### <a name="_Toc427488829"/>AlternateDataStream_QueryAndSet_FileInformation

| &#32;| &#32; |
| -------------| ------------- |
| Description| To query or set the file information of the Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|


#### <a name="_Toc427488830"/>AlternateDataStream_FsControl

| &#32;| &#32; |
| -------------| ------------- |
| Description| To request a FsControl on the Alternate Data Streams on a file in the file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile, DirectoryFile| 
| | Test coverage:|
| Message Sequence| CreateFile | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|


### <a name="Other-Scenarios"/>Other Scenarios

#### <a name="Other-scenarios-CreateFile"/>CreateFile

| &#32;| &#32; |
| -------------| ------------- |
| Description| To create a file in the file system.| 
| | Test environment: NTFS, ReFS, FAT32| 
| | Test object: DataFile, DirectoryFile| 
| Message Sequence| CreateFile | 
| | Verify server return with **STATUS_SUCCESS** for supported file system|



## <a name="_Toc427488677"/>Traditional Test Case Design

### <a name="Test-cases-for-FileInformation"/>Test cases for FileInformation

#### <a name="_Toc427488721"/>IsEASupported

##### <a name="_Toc427488722"/>FileInfo_Set_FileFullEaInformation_File_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DataFile)
| | SetInfo with FileInfoClass.FileFullEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488723"/>FileInfo_Set_FileFullEaInformation_Dir_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DirectoryFile)
| | SetInfo with FileInfoClass.FileFullEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488724"/>FileInfo_Query_FileFullEaInformation_File_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DataFile)
| | QueryInfo with FileInfoClass.FileFullEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488725"/>FileInfo_Query_FileFullEaInformation_Dir_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DirectoryFile)
| | QueryInfo with FileInfoClass.FileFullEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488726"/>FileInfo_Set_FileEaInformation_File_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DataFile)
| | SetInfo with FileInfoClass.FileEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




#### <a name="_Toc427488727"/>FileInfo_Set_FileEaInformation_Dir_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DirectoryFile)
| | SetInfo with FileInfoClass.FileEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488728"/>FileInfo_Query_FileEaInformation_File_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DataFile)
| | QueryInfo with FileInfoClass.FileEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488729"/>FileInfo_Query_FileEaInformation_Dir_IsEASupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if extended attribute is supported for different file systems.
| | Note: Only NTFS implements EAs.
| | Test environment: NTFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsEASupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsEASupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile (DirectoryFile)
| | QueryInfo with FileInfoClass.FileEaInformation
| | If (IsEASupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




#### <a name="_Toc427488739"/>IsFileLinkInfoSupported

##### <a name="_Toc427488740"/>FileInfo_Set_FileLinkInfo_File_IsFileLinkInfoSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FileLinkInformation is supported for different file systems.
| | Note: Only NTFS supports FileLinkInformation
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Input parameter: FileInfoClass.FileLinkInformation
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsFileLinkInfoSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsFileLinkInfoSupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile
| | SetInfo with FileInfoClass.FileLinkInformation 
| | If (IsFileLinkInfoSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488741"/>FileInfo_Set_FileLinkInfo_DIr_IsFileLinkInfoSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FileLinkInformation is supported for different file systems.
| | Note: Only NTFS supports FileLinkInformation
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Input parameter: FileInfoClass.FileLinkInformation
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsFileLinkInfoSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsFileLinkInfoSupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile
| | SetInfo with FileInfoClass.FileLinkInformation 
| | If (IsFileLinkInfoSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }


#### <a name="_Toc427488702"/>IsShortNameSupported

##### <a name="_Toc427488703"/>FileInfo_Set_FileShortNameInfo_File_IsShortNameSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if short name supported for different file system.
| | Note: ReFS does not implement short names.
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsShortNameSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsShortNameSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile 
| | SetInfo with FileInfoClass.FileShortNameInformation
| | If (IsShortNameSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488704"/>FileInfo_Set_FileShortNameInfo_Dir_IsShortNameSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if short name supported for different file system.
| | Note: ReFS does not implement short names.
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsShortNameSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsShortNameSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile 
| | SetInfo with FileInfoClass.FileShortNameInformation
| | If (IsShortNameSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }



#### <a name="_Toc427488712"/>IsCompressionSupported


##### <a name="_Toc427488715"/>FileInfo_Query_FileCompressionInfo_File_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsCompressionSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsCompressionSupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile 
| | QueryInfo for FileInfoClass.FileCompressionInformation
| | If (IsCompressionSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488716"/>FileInfo_Query_FileCompressionInfo_Dir_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsCompressionSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsCompressionSupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile 
| | QueryInfo for FileInfoClass.FileCompressionInformation
| | If (IsCompressionSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }



#### <a name="_Toc427488742"/>IsFileValidDataLengthInfoSupported

##### <a name="_Toc427488743"/>FileInfo_Set_FileValidDataLengthInformation_File_IsSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test the FileValidDataLengthInformation for different file systems.
| | Test environment: NTFS, ReFS, FAT32
| | Test object: DataFile, DirectoryFile
| | Input parameter: FileInfoClass.FileValidDataLengthInformation
| | Parameter combination.
| | Parameter&emsp;&emsp;&nbsp;&#124;&nbsp;Expected Result
| | IsFileValidDataLengthInfoSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsFileValidDataLengthInfoSupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile
| | SetInfo with FileInfoClass = FileValidDataLengthInformation 
| | If (IsFileValidDataLengthInfoSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }




##### <a name="_Toc427488744"/>FileInfo_Set_FileValidDataLengthInformation_Dir_IsSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test the FileValidDataLengthInformation for different file systems.
| | Test environment: NTFS, ReFS, FAT32
| | Test object: DataFile, DirectoryFile
| | Input parameter: FileInfoClass.FileValidDataLengthInformation
| | Parameter combination.
| | Parameter&emsp;&emsp;&nbsp;&#124;&nbsp;Expected Result
| | IsFileValidDataLengthInfoSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsFileValidDataLengthInfoSupported == False &nbsp;&#124;&nbsp;STATUS_NOT_SUPPORTED
| Message Sequence| CreateFile
| | SetInfo with FileInfoClass = FileValidDataLengthInformation 
| | If (IsFileValidDataLengthInfoSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_NOT_SUPPORTED**,ActualResult);
| | }



#### <a name="_Toc427488730"/>IsIntegritySupported

##### <a name="_Toc427488733"/>FileInfo_Query_FileBasicInfo_File_IsIntegritySupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if integrity is supported for different file systems.| 
| | Note: Only ReFS supports integrity.| 
| | Test environment: ReFS| 
| | Input parameter: FileInfoClass. FileBasicInformation| 
| | Expected Result:| 
| | If the object store support this functionality, FILE_ATTRIBUTE_INTEGRITY_STREAM  is set in OutputBuffer.FileAttributes.| 
| | If not support, FILE_ATTRIBUTE_INTEGRITY_STREAM  is not set in OutputBuffer.FileAttributes.| 
| Message Sequence| CreateFile DataFile with **FileAttribute**.INTEGRITY_STREAM| 
| | QueryInfo with FileInfoClass = FileBasicInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is set.| 
| | } Else {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is not set.| 
| | }| 




##### <a name="_Toc427488734"/>FileInfo_Query_FileBasicInfo_Dir_IsIntegritySupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if integrity is supported for different file systems.| 
| | Note: Only ReFS supports integrity.| 
| | Test environment: ReFS| 
| | Input parameter: FileInfoClass. FileBasicInformation| 
| | Expected Result:| 
| | If the object store support this functionality, FILE_ATTRIBUTE_INTEGRITY_STREAM  is set in OutputBuffer.FileAttributes.| 
| | If not support, FILE_ATTRIBUTE_INTEGRITY_STREAM  is not set in OutputBuffer.FileAttributes.| 
| Message Sequence| CreateFile DirectoryFile with **FileAttribute**.INTEGRITY_STREAM| 
| | QueryInfo with FileInfoClass = FileBasicInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is set.| 
| | } Else {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is not set.| 
| | }| 




##### <a name="_Toc427488735"/>FileInfo_Query_FileAttributeTagInfo_File_IsIntegritySupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if integrity is supported for different file systems.| 
| | Note: Only ReFS supports integrity.| 
| | Test environment: ReFS| 
| | Input parameter: FileInfoClass.FileAttributeTagInformation| 
| | Expected Results:| 
| | If the object store support this functionality, FILE_ATTRIBUTE_INTEGRITY_STREAM  is set in OutputBuffer.FileAttributes.| 
| | If not support, FILE_ATTRIBUTE_INTEGRITY_STREAM  is not set in OutputBuffer.FileAttributes.| 
| Message Sequence| CreateFile DataFile with **FileAttribute**.INTEGRITY_STREAM| 
| | QueryInfo with FileInfoClass = FileAttributeTagInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is set.| 
| | } Else {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is not set.| 
| | }| 




##### <a name="_Toc427488736"/>FileInfo_Query_FileAttributeTagInfo_Dir_IsIntegritySupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if integrity is supported for different file systems.| 
| | Note: Only ReFS supports integrity.| 
| | Test environment: ReFS| 
| | Input parameter: FileInfoClass.FileAttributeTagInformation| 
| | Expected Results:| 
| | If the object store support this functionality, FILE_ATTRIBUTE_INTEGRITY_STREAM  is set in OutputBuffer.FileAttributes.| 
| | If not support, FILE_ATTRIBUTE_INTEGRITY_STREAM  is not set in OutputBuffer.FileAttributes.| 
| Message Sequence| CreateFile DirectoryFile with **FileAttribute**.INTEGRITY_STREAM| 
| | QueryInfo with FileInfoClass = FileAttributeTagInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is set.| 
| | } Else {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is not set.| 
| | }| 




##### <a name="_Toc427488737"/>FileInfo_Query_FileNetworkOpenInfo_File_IsIntegritySupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if integrity is supported for different file systems.| 
| | Note: Only ReFS supports integrity.| 
| | Test environment: ReFS| 
| | Input parameter: FileInfoClass.FileNetworkOpenInformation| 
| | Expected Results:| 
| | If the object store support this functionality, FILE_ATTRIBUTE_INTEGRITY_STREAM  is set in OutputBuffer.FileAttributes.| 
| | If not support, FILE_ATTRIBUTE_INTEGRITY_STREAM  is not set in OutputBuffer.FileAttributes.| 
| Message Sequence| CreateFile DataFile with **FileAttribute**.INTEGRITY_STREAM| 
| | QueryInfo with FileInfoClass = FileNetworkOpenInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is set.| 
| | } Else {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is not set.| 
| | }| 




##### <a name="_Toc427488738"/>FileInfo_Query_FileNetworkOpenInfo_Dir_IsIntegritySupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if integrity is supported for different file systems.| 
| | Note: Only ReFS supports integrity.| 
| | Test environment: ReFS| 
| | Input parameter: FileInfoClass.FileNetworkOpenInformation| 
| | Expected Results:| 
| | If the object store support this functionality, FILE_ATTRIBUTE_INTEGRITY_STREAM  is set in OutputBuffer.FileAttributes.| 
| | If not support, FILE_ATTRIBUTE_INTEGRITY_STREAM  is not set in OutputBuffer.FileAttributes.| 
| Message Sequence| CreateFile DirectoryFile with **FileAttribute**.INTEGRITY_STREAM| 
| | QueryInfo with FileInfoClass = FileNetworkOpenInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is set.| 
| | } Else {| 
| | Verify OutputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM is not set.| 
| | }| 


#### <a name="IsFileIdInformationSupported"/>IsFileIdInformationSupported

##### <a name="FileInfo_Query_FileIdInformation_File">FileInfo_Query_FileIdInformation_File


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FileIdInformation supported for different file system.| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: FileInfoClass.FileIdInformation| 
| | Expected Results:| 
| | If supported, The operation returns **STATUS_SUCCESS**.| 
| | And FileId of FileIdInformation is verified by sending FSCTL_READ_FILE_USN_DATA, VolumeSerialNumber of FileIdInformation is verified by sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA. | 
| Message Sequence| CreateFile DataFile| 
| | QueryInfo with FileInfoClass.FileIdInformation|
| | Get FileReferenceNumber by sending FSCTL_READ_FILE_USN_DATA to server.| 
| | Verify FileId when querying FileIdInformation should be the same with FileReferenceNumber when sending FSCTL_READ_FILE_USN_DATA.|
| | Get VolumeSerialNumber by sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA to server.|
| | Verify VolumeSerialNumber when querying FileIdInformation should be the same with VolumeSerialNumber when sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA.|


##### <a name="FileInfo_Query_FileIdInformation_Dir">FileInfo_Query_FileIdInformation_Dir


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FileIdInformation supported for different file system.| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: FileInfoClass.FileIdInformation| 
| | Expected Results:| 
| | If supported, The operation returns **STATUS_SUCCESS**.| 
| | And FileId of FileIdInformation is verified by sending FSCTL_READ_FILE_USN_DATA, VolumeSerialNumber of FileIdInformation is verified by sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA. | 
| Message Sequence| CreateFile DirectoryFile| 
| | QueryInfo with FileInfoClass.FileIdInformation|
| | Get FileReferenceNumber by sending FSCTL_READ_FILE_USN_DATA to server.| 
| | Verify FileId when querying FileIdInformation should be the same with FileReferenceNumber when sending FSCTL_READ_FILE_USN_DATA.|
| | Get VolumeSerialNumber by sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA to server.|
| | Verify VolumeSerialNumber when querying FileIdInformation should be the same with VolumeSerialNumber when sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA.|


### <a name="Test-cases-for-FileSystemInformation">Test cases for FileSystemInformation



#### <a name="_Toc427488705"/>IsObjectIdSupported


##### <a name="_Toc427488708"/>FsInfo_Query_FileFsObjectIdInformation_File_IsObjectIdSupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if object id is supported for different file systems.
| | Note: ReFS does not support object IDs.
| | Test environment: NTFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsObjectIdSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsObjectIdSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_PARAMETER
| Message Sequence| CreateFile 
| | QueryInfo with FsInfoClass.FileFsObjectIdInformation
| | If (IsObjectIdSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);
| | }




##### <a name="_Toc427488709"/>FsInfo_Query_FileFsObjectIdInformation_Dir_IsObjectIdSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if object id is supported for different file systems.
| | Note: ReFS does not support object IDs.
| | Test environment: NTFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsObjectIdSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsObjectIdSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_PARAMETER
| Message Sequence| CreateFile 
| | QueryInfo with FsInfoClass.FileFsObjectIdInformation
| | If (IsObjectIdSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);
| | }



### <a name="IsFsSectorSizeInformationSupported"/>IsFsSectorSizeInformationSupported



#### <a name="_Toc427488695"/>FsInfo_Query_FileFsSectorSizeInformation_File_OutputBufferSize(3TCs, 1BVT)
Parameter combination


| &#32;| &#32; |
| -------------| ------------- |
| Parameter| Return Code| 
| OutputBufferSize  = sizeof(| STATUS_INFO_LENGTH_MISMATCH| 
| FILE_FS_SECTOR_SIZE_INFORMATION) -1| | 
| OutputBufferSize  = sizeof(| STATUS_SUCCESS| 
| FILE_FS_SECTOR_SIZE_INFORMATION)| | 
| OutputBufferSize  = sizeof(| STATUS_SUCCESS| 
| FILE_FS_SECTOR_SIZE_INFORMATION) +1| | 



#### <a name="_Toc427488696"/> FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputBufferSize(3TCs)
Parameter combination


| &#32;| &#32; |
| -------------| ------------- |
| Parameter| Return Code| 
| OutputBufferSize  = sizeof(| STATUS_INFO_LENGTH_MISMATCH| 
| FILE_FS_SECTOR_SIZE_INFORMATION) -1| | 
| OutputBufferSize  = sizeof(| STATUS_SUCCESS| 
| FILE_FS_SECTOR_SIZE_INFORMATION)| | 
| OutputBufferSize  = sizeof(| STATUS_SUCCESS| 
| FILE_FS_SECTOR_SIZE_INFORMATION) +1| | 



#### <a name="_Toc427488697"/>FsInfo_Query_FileFsSectorSizeInformation_File_OutputValue_Common

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test the output buffer for FILE_FS_SECTOR_SIZE_INFORMATION.| 
| | Test environment: NTFS, ReFS| 
| | Test object: file, directory| 
| Message Sequence| CreateFile (DataFile)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation | 
| | Assert.AreEqual(**STATUS_** **SUCCESS**,ActualResult);| 
| | Assert.AreEqual(sizeof(FILE_FS_SECTOR_SIZE_INFORMATION), **ByteCount**);| 
| | Verify ByteCount is set to the size of the FILE_FS_SECTOR_SIZE_INFORMATION structure| 
| | Verify LogicalBytesPerSector/PhysicalBytesPerSector/ SystemPageSize are correctly set.| 




#### <a name="_Toc427488698"/>FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputValue_Common

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test the output buffer for FILE_FS_SECTOR_SIZE_INFORMATION.| 
| | Test environment: NTFS, ReFS| 
| | Test object: file, directory| 
| Message Sequence| CreateFile (Directory)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation | 
| | Assert.AreEqual(**STATUS_** **SUCCESS**,ActualResult);| 
| | Assert.AreEqual(sizeof(FILE_FS_SECTOR_SIZE_INFORMATION), **ByteCount**);| 
| | Verify ByteCount is set to the size of the FILE_FS_SECTOR_SIZE_INFORMATION structure| 
| | Verify LogicalBytesPerSector/PhysicalBytesPerSector/ SystemPageSize are correctly set.| 




#### <a name="_Toc427488699"/>FsInfo_Set_FileFsSectorSizeInformation_File_InvalidInfoClass

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test invalid operation for setting FILE_FS_SECTOR_SIZE_INFORMATION.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (DataFile)| 
| | SetInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation | 
| | Assert.AreEqual(**STATUS_ INVALID_INFO_CLASS**, actualResult);| 




#### <a name="_Toc427488700"/>FsInfo_Set_FileFsSectorSizeInformation_Dir_InvalidInfoClass

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test invalid operation for setting FILE_FS_SECTOR_SIZE_INFORMATION.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (DirectoryFile)| 
| | SetInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation | 
| | Assert.AreEqual(**STATUS_ INVALID_INFO_CLASS**, actualResult);| 


### <a name="IsFsAttributeInformationSupported"/>IsFsAttributeInformationSupported

##### <a name="_Toc427488713"/>FsInfo_Query_FileFsAttributeInformation_File_IsCompressionSupported(BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsCompressionSupported == True) {| 
| | Verify FileAttribute.FILE_FILE_COMPRESSION is set.| 
| | } Else {| 
| |     Verify FileAttribute.FILE_FILE_COMPRESSION is not set.}| 




##### <a name="_Toc427488714"/>FsInfo_Query_FileFsAttributeInformation_Dir_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsCompressionSupported == True) {| 
| | Verify FileAttribute.FILE_FILE_COMPRESSION is set.| 
| | } Else {| 
| |     Verify FileAttribute.FILE_FILE_COMPRESSION is not set.| 
| | }| 


##### <a name="_Toc427488731"/>FsInfo_Query_FileFsAttributeInformation_File_IsIntegritySupported(BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Integrity is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify FileAttribute.FILE_SUPPORT_INTEGRITY_STREAMS is set.| 
| | } Else {| 
| |     Verify FileAttribute.FILE_SUPPORT_INTEGRITY_STREAMS is not set.}| 




##### <a name="_Toc427488732"/>FsInfo_Query_FileFsAttributeInformation_Dir_IsIntegritySupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Integrity is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsIntegritySupported == True) {| 
| | Verify FileAttribute.FILE_SUPPORT_INTEGRITY_STREAMS is set.| 
| | } Else {| 
| |     Verify FileAttribute.FILE_SUPPORT_INTEGRITY_STREAMS is not set.}| 


##### <a name="_Toc427488752"/>FsInfo_Query_FileFsAttributeInformation_File_IsEncryptionSupported(BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Encryption is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsEncryptionSupported == True) {| 
| | Verify FileAttribute.FILE_SUPPORTS_ENCRYPTION is set.| 
| | } Else {| 
| |     Verify FileAttribute.FILE_SUPPORTS_ENCRYPTION is not set.}| 



##### <a name="_Toc427488753"/>FsInfo_Query_FileFsAttributeInformation_Dir_IsEncryptionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Encryption is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsEncryptionSupported == True) {| 
| | Verify FileAttribute.FILE_SUPPORTS_ENCRYPTION is set.| 
| | } Else {| 
| |     Verify FileAttribute.FILE_SUPPORTS_ENCRYPTION is not set.}| 



##### <a name="_Toc427488706"/>FsInfo_Query_FileFsAttributeInformation_File_IsObjectIDsSupported(BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if ObjectIDs is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsObjectIDsSupported == True) {| 
| | Verify FileAttribute.FILE_FILE_SUPPORTS_OBJECT_IDS is set.| 
| | } Else {| 
| |     Verify FileAttribute..FILE_ FILE_SUPPORTS_OBJECT_IDS is not set.}| 




##### <a name="_Toc427488707"/>FsInfo_Query_FileFsAttributeInformation_Dir_IsObjectIDsSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if ObjectIDs is supported for different file systems.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile | 
| | QueryInfo for FileInfoClass. FileFsAttributeInformation| 
| | If (IsObjectIDsSupported == True) {| 
| | Verify FileAttribute.FILE_FILE_SUPPORTS_OBJECT_IDS is set.| 
| | } Else {| 
| |     Verify FileAttribute..FILE_FILE_SUPPORTS_OBJECT_IDS is not set.}| 



#### <a name="IsFsSizeInformationSupported"/>IsFsSizeInformationSupported

##### <a name="_Toc427488746"/>FsInfo_Query_FileFsSizeInformation_File_SectorsPerAllocationUnit (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test cluster size for different file system.
| | For ClusterSize:
| |     NTFS uses a default cluster size of 4k 
| | ReFS uses a default cluster size of 64k
| | For LogicalBytesPerSector: 
| | MUST be greater than or equal to 512 bytes
| | For FileFsSizeInformation:
| | OutputBuffer.SectorsPerAllocationUnit set to Open.File.Volume.ClusterSize / Open.File.Volume.LogicalBytesPerSector
| | So for NTFS, SectorsPerAllocationUnit = 4K/512 bytes = 8
| | For ReFS, SectorsPerAllocationUnit = 64K/512 bytes = 128
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | FileSystem == NTFS&nbsp;&#124;&nbsp;SectorsPerAllocationUnit = 8
| | FileSystem == ReFS&nbsp;&#124;&nbsp;SectorsPerAllocationUnit = 128
| Message Sequence| CreateFile
| | QueryInfo with FsInfoClass. FileFsSizeInformation
| | Verify the outputbuffer.SectorsPerAllocationUnit  is correctly set according to default cluster size.
| | If (FileSystem == NTFS) {
| | expectedUnits = NTFS.DefaultClusterSize / LogicalBytesPerSector
| |     Assert.AreEqual(expectedUnits, outputbuffer.SectorsPerAllocationUnit);
| | } Else If (FileSystem == ReFS) {
| | expectedUnits = ReFS.DefaultClusterSize / LogicalBytesPerSector
| |     Assert.AreEqual(expectedUnits, outputbuffer.SectorsPerAllocationUnit);
| | }




##### <a name="_Toc427488747"/>FsInfo_Query_FileFsSizeInformation_Dir_SectorsPerAllocationUnit

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test cluster size for different file system.
| | For ClusterSize:
| |     NTFS uses a default cluster size of 4k 
| | ReFS uses a default cluster size of 64k
| | For LogicalBytesPerSector: 
| | MUST be greater than or equal to 512 bytes
| | For FileFsSizeInformation:
| | OutputBuffer.SectorsPerAllocationUnit set to Open.File.Volume.ClusterSize / Open.File.Volume.LogicalBytesPerSector
| | So for NTFS, SectorsPerAllocationUnit = 4K/512 bytes = 8
| | For ReFS, SectorsPerAllocationUnit = 64K/512 bytes = 128
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | FileSystem == NTFS&nbsp;&#124;&nbsp;SectorsPerAllocationUnit = 8
| | FileSystem == ReFS&nbsp;&#124;&nbsp;SectorsPerAllocationUnit = 128
| Message Sequence| CreateFile
| | QueryInfo with FsInfoClass. FileFsSizeInformation
| | Verify the outputbuffer.SectorsPerAllocationUnit  is correctly set according to default cluster size.
| | If (FileSystem == NTFS) {
| | expectedUnits = NTFS.DefaultClusterSize / LogicalBytesPerSector
| |     Assert.AreEqual(expectedUnits, outputbuffer.SectorsPerAllocationUnit);
| | } Else If (FileSystem == ReFS) {
| | expectedUnits = ReFS.DefaultClusterSize / LogicalBytesPerSector
| |     Assert.AreEqual(expectedUnits, outputbuffer.SectorsPerAllocationUnit);
| | }



### <a name="Test-cases-for-FsControlRequest"/>Test cases for FsControlRequest


#### <a name="_Toc427488756"/> IsAllocatedRangesSupported

##### <a name="_Toc427488757"/>FsCtl_Query_AllocatedRanges_File_IsAllocatedRangesSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_QUERY_ALLOCATED_RANGES is supported for different file systems.
| | Note: This is only implemented by the ReFS and NTFS file systems.
| | Test environment: NTFS, ReFS, FAT32
| | FsCtl Request: FSCTL_QUERY_ALLOCATED_RANGES
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsAllocatedRangesSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsAllocatedRangesSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DataFile)
| | FsCtl request with FSCTL_QUERY_ALLOCATED_RANGES
| | If (IsAllocatedRangesSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488758"/>FsCtl_Query_AllocatedRanges_Dir_IsAllocatedRangesSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_QUERY_ALLOCATED_RANGES is supported for different file systems.
| | Note: This is only implemented by the ReFS and NTFS file systems.
| | Test environment: NTFS, ReFS, FAT32
| | FsCtl Request: FSCTL_QUERY_ALLOCATED_RANGES
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsAllocatedRangesSupported == True&nbsp;&#124;&nbsp;STATUS_INVALID_PARAMETER
| | IsAllocatedRangesSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DirectoryFile)
| | FsCtl request with FSCTL_QUERY_ALLOCATED_RANGES
| | If (IsAllocatedRangesSupported == True) {
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }


#### <a name="_Toc427488759"/> IsReparsePointSupported

##### <a name="_Toc427488760"/>FsCtl_Get_ReparsePoint_IsReparsePointSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_GET_REPARSE_POINT is supported for different file systems.
| | Note: This is only implemented by the ReFS and NTFS file systems.
| | Test environment: NTFS, ReFS, FAT32
| | Test object: DirectoryFile
| | FsCtl Request: FSCTL_GET_REPARSE_POINT
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsReparsePointSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsReparsePointSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile
| | FsCtl request with FSCTL_GET_REPARSE_POINT
| | If (IsReparsePointSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }


#### <a name="IsSparseFileSupported"/>IsSparseFileSupported

##### <a name="FsCtl_Set_Sparse_File_IsSparseFileSupported"/>FsCtl_Set_Sparse_File_IsSparseFileSupported


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_SET_SPARSE is supported for different file systems.| 
| | Note: This is only implemented by the **NTFS** file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_SET_SPARSE| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | FsCtl request with FSCTL_SET_SPARSE| 
| | Verify server returns STATUS_SUCCESS for supported file system| 
| | Or returns STATUS_INVALID_DEVICE_REQUEST for unsupported file system.| 


##### <a name="FsCtl_Set_Sparse_Dir_IsSparseFileSupported"/>FsCtl_Set_Sparse_Dir_IsSparseFileSupported


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_SET_SPARSE is supported for different file systems.| 
| | Note: This is only implemented by the **NTFS** file system.| 
| | Test environment: NTFS, ReFS| 
| | Test object: DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_SET_SPARSE| 
| | If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST | 
| | If support, returns STATUS_SUCCESS.| 
| Message Sequence| CreateFile| 
| | FsCtl request with FSCTL_SET_SPARSE| 
| | Verify server returns STATUS_SUCCESS for supported file system| 
| | Or returns STATUS_INVALID_DEVICE_REQUEST for unsupported file system.| 


#### <a name="_Toc427488765"/> IsZeroDataSupported

##### <a name="_Toc427488766"/>FsCtl_Set_ZeroData_File_IsSetZeroDataSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_SET_ZERO_DATA is supported for different file systems.
| | Note: This is only implemented by the ReFS and NTFS file systems.
| | Test environment: NTFS, ReFS
| | FsCtl Request: FSCTL_SET_ZERO_DATA
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsSetZeroDataSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsSetZeroDataSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DataFile)
| | FsCtl request with FSCTL_SET_ZERO_DATA
| | If (IsZeroDataSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488767"/>FsCtl_Set_ZeroData_Dir_IsZeroDataSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_SET_ZERO_DATA is supported for different file systems.
| | Note: This is only implemented by the ReFS and NTFS file systems.
| | Test environment: NTFS, ReFS
| | FsCtl Request: FSCTL_SET_ZERO_DATA
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsSetZeroDataSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsSetZeroDataSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DirectoryFile)
| | FsCtl request with FSCTL_SET_ZERO_DATA
| | If (IsSetZeroDataSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




#### <a name="_Toc427488831"/> FileNormalizedNameInformation

##### <a name="_Toc427488832"/>FileInfo_Query_FileNormalizedNameInfo_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if query FileNormalizedNameInformation on file is supported for different file systems.
| | Note: Windows 10 v1709, Windows Server v1709 operating system and prior do not support the FileNormalizedNameInformation information class.
| | File information class: FileNormalizedNameInformation
| Message Sequence| CreateFile (DataFile)
| | QUERY_INFO request with FileNormalizedNameInformation
| | If (status == **STATUS_NOT_SUPPORTED**) {
| | Assert.Inconclusive();
| | } Else If (status == **STATUS_SUCCESS**) {
| | // Verify outputBuffer as FileNameInformation.
| | }
| | Else {
| | Assert.Fail();
| | }




##### <a name="_Toc427488833"/>FileInfo_Query_FileNormalizedNameInfo_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if query FileNormalizedNameInformation on directory is supported for different file systems.
| | Note: Windows 10 v1709, Windows Server v1709 operating system and prior do not support the FileNormalizedNameInformation information class.
| | File information class: FileNormalizedNameInformation
| Message Sequence| CreateFile (DirectoryFile)
| | QUERY_INFO request with FileNormalizedNameInformation
| | If (status == **STATUS_NOT_SUPPORTED**) {
| | Assert.Inconclusive();
| | } Else If (status == **STATUS_SUCCESS**) {
| | // Verify outputBuffer as FileNameInformation.
| | }
| | Else {
| | Assert.Fail();
| | }




#### <a name="_Toc437488671">IsDuplicateExtentsToFileExSupported

##### <a name="_Toc437488672"/> BVT_FsCtl_DuplicateExtentsToFileEx_IsBasicSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX without additional flags is supported.| 
| | Note: This is only implemented by the **REFS** file system.| 
| | Test environment: ReFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX| 
| Message Sequence| Create test file and fill up first two clusters with random data.| 
| | FSCTL request with FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX and no additional flags to duplicate the file extent of first cluster to the second.| 
| | Verify returned NTSTATUS code and file content by checking the following:| 
| | 1. The returned status is STATUS_SUCCESS.| 
| | 2. The file content of the second cluster is the same as the first.| 




##### <a name="_Toc437488673"/> BVT_FsCtl_DuplicateExtentsToFileEx_IsSourceAtomicSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX with DUPLICATE_EXTENTS_DATA_EX_SOURCE_ATOMIC is supported.| 
| | Note: This is only implemented by the **REFS** file system.| 
| | Test environment: ReFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX| 
| Message Sequence| Create test file and fill up first two clusters with random data.| 
| | FSCTL request with FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX and DUPLICATE_EXTENTS_DATA_EX_SOURCE_ATOMIC to duplicate the file extent of first cluster to the second.| 
| | Verify returned NTSTATUS code and file content by checking the following:| 
| | 1. The returned status is STATUS_SUCCESS.| 
| | 2. The file content of the second cluster is the same as the first.| 



#### <a name="IsGetIntegrityInformationSupported"/>IsGetIntegrityInformationSupported


##### <a name="_Toc427488679"/>FsCtl_Get_IntegrityInformation_File_IsIntegritySupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  if FSCTL request **FSCTL_GET_INTEGRITY_INFORMATION** is supported
| | Note: Only ReFS supports integrity
| | Test environment: NTFS, ReFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsIntegritySupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsIntegritySupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DataFile)
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION
| | If (IsIntegritySupported == True) {
| | Assert.AreEqual(STATUS_SUCCESS,ActualResult);
| | } Else {
| | Assert.AreEqual(STATUS_INVALID_DEVICE_REQUEST,ActualResult);
| | }



##### <a name="_Toc427488680"/>FsCtl_Get_IntegrityInformation_Dir_IsIntegritySupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  if FSCTL request **FSCTL_GET_INTEGRITY_INFORMATION** is supported
| | Note: Only ReFS supports integrity
| | Test environment: NTFS, ReFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsIntegritySupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsIntegritySupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DirectoryFile)
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION
| | If (IsIntegritySupported == True) {
| | Assert.AreEqual(STATUS_SUCCESS,ActualResult);
| | } Else {
| | Assert.AreEqual(STATUS_INVALID_DEVICE_REQUEST,ActualResult);
| | }




##### <a name="_Toc427488681"/>FsCtl_Get_IntegrityInformation_File_InvalidParameter(3TCs)
Parameter combination: (Expected results: STATUS_INVALID_PARAMETER)


| &#32;| &#32; |
| -------------| ------------- |
|  | Parameter| 
| 1| OutputBufferSize   &#60;  sizeof(| 
| | FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)| 
| 2| Open.Stream.StreamType != FileStream| 
| | && Open.Stream.StreamType != DirectoryStream| 
| 3| Open.File.FileAttributes.FILE_ATTRIBUTE_SYSTEM == TRUE| 


| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **FsCtl_Get** **_** **Integrity** **Information_** **File** **_** **OutputBu** **fferSizeLessThanIntegrityBuffer**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (DataFile)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| |  **OutputBufferSize**  **=**  **sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)**  **-1**| 
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);| 
| Test case|  **FsCtl_Get** **_** **Integrity** **Information_** **File_** **OpenStreamTypeIsNull**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (DataFile)| 
| | FsCtl request with **Open.Stream.StreamType**  **=**  **NULL**| 
| | Assert.AreEqual(STATUS_INVALID_PARAMETER,ActualResult);| 
| Test case|  **FsCtl_Get** **_** **Integrity** **Information_** **File_** **SystemFile**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: | 
| | Open.File.FileAttributes.FILE_ATTRIBUTE_SYSTEM == TRUE | 
| | Expected Result: | 
| | STATUS_INVALID_PARAMETER| 
| Message Sequence| CreateFile (DataFile) with FileAttributes.FILE_ATTRIBUTE_SYSTEM| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION | 
| | Assert.AreEqual(STATUS_INVALID_PARAMETER,ActualResult);| 


##### <a name="_Toc427488682"/>FsCtl_Get_IntegrityInformation_Dir_InvalidParameter(3TCs)
Parameter combination: (Expected results: STATUS_INVALID_PARAMETER)


| &#32;| &#32; |
| -------------| ------------- |
|  | Parameter| 
| 1| OutputBufferSize   &#60;  sizeof(| 
| | FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)| 
| 2| Open.Stream.StreamType != FileStream| 
| | && Open.Stream.StreamType != DirectoryStream| 
| 3| Open.File.FileAttributes.FILE_ATTRIBUTE_SYSTEM == TRUE| 


| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **FsCtl_Get** **_** **Integrity** **Information_** **Dir_** **OutputBu** **fferSizeLessThanIntegrityBuffer**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| |  **OutputBufferSize**  **=**  **sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)**  **-1**| 
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);| 
| Test case|  **FsCtl_Get** **_** **Integrity** **Information_** **Dir_** **OpenStreamTypeIsNull**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory)| 
| | FsCtl request with **Open.Stream.StreamType**  **=**  **NULL**| 
| | Assert.AreEqual(STATUS_INVALID_PARAMETER,ActualResult);| 
| Test case|  **FsCtl_Get** **_** **Integrity** **Information_** **Dir_** **SystemFile**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: | 
| | Open.File.FileAttributes.FILE_ATTRIBUTE_SYSTEM == TRUE | 
| | Expected Result: | 
| | STATUS_INVALID_PARAMETER| 
| Message Sequence| CreateFile (DirectoryFile) with FileAttributes.FILE_ATTRIBUTE_SYSTEM| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION | 
| | Assert.AreEqual(STATUS_INVALID_PARAMETER,ActualResult);| 




##### <a name="_Toc427488683"/>FsCtl_Get_IntegrityInformation_File_OutputValue(2TCs)

| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **FsCtl_Get** **_** **IntegrityInformation_File_OutputValue_** **Common**| 
| Description| To test the output value for FSCTL_GET_INTEGRITY_INFORMATION.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: ReFS| 
| Message Sequence| CreateFile (DataFile)| 
| | Send a valid FsControl request with  FSCTL_GET_INTEGRITY_INFORMATION| 
| | Verify server returns STATUS_SUCCESS| 
| | Verify the following values are correct| 
| | OutputBuffer.CheckSumAlgorithm is one of the values for ChecksumAlgorithm| 
| | OutputBuffer.ClusterShift is the base-2 logarithm of Open.File.Volume.ClusterSize| 
| | OutputBuffer.Flags is not CHECKSUM_ENFORCEMENT_OFF| 
| Test case|  **FsCtl_Get** **_** **Integri** **tyInformation_File_OutputValue_ChecksumEnforcement**| 
| Description| To test the OutputBuffer.Flags for CHECKSUM_ENFORCEMENT_OFF.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: ReFS| 
| Message Sequence| CreateFile (DataFile, StreamType is DataStream)| 
| | FsControl request with  FSCTL_SET_INTEGRITY_INFORMATION| 
| |     With Flags set to CHECKSUM_ENFORCEMENT_OFF| 
| | FsControl request with  FSCTL_GET_INTEGRITY_INFORMATION| 
| | OutputBuffer.Flags is CHECKSUM_ENFORCEMENT_OFF| 




##### <a name="_Toc427488684"/>FsCtl_Get_IntegrityInformation_Dir_OutputValue(2TCs)

| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **FsCtl_GetIntegr** **ityInformation_Dir_OutputValue_Common**| 
| Description| To test the output value for FSCTL_GET_INTEGRITY_INFORMATION.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: ReFS| 
| Message Sequence| CreateFile (Directory)| 
| | Send a valid FsControl request with  FSCTL_GET_INTEGRITY_INFORMATION| 
| | Verify server returns STATUS_SUCCESS| 
| | Verify the following values are correct| 
| | OutputBuffer.CheckSumAlgorithm is one of the values for ChecksumAlgorithm| 
| | OutputBuffer.ClusterShift is the base-2 logarithm of Open.File.Volume.ClusterSize| 
| | OutputBuffer.Flags is **not** CHECKSUM_ENFORCEMENT_OFF| 
| Test case|  **FsCtl_Get** **_** **IntegrityInformation_Dir_OutputValue_** **ChecksumEnforcement**| 
| Description| To test the OutputBuffer.Flags for directory:| 
| |     It is not CHECKSUM_ENFORCEMENT_OFF even Open.Stream.ChecksumEnforcementOff is TRUE.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: ReFS| 
| Message Sequence| CreateFile (Directory)| 
| | FsControl request with  FSCTL_SET_INTEGRITY_INFORMATION| 
| |     With Flags set to CHECKSUM_ENFORCEMENT_OFF| 
| | FsControl request with  FSCTL_GET_INTEGRITY_INFORMATION| 
| | OutputBuffer.Flags is **not** CHECKSUM_ENFORCEMENT_OFF| 



#### <a name="IsSetIntegrityInformationSupported"/>IsSetIntegrityInformationSupported


##### <a name="_Toc427488685"/>FsCtl_Set_IntegrityInformation_File_IsIntegritySupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  if FSCTL request **FSCTL_** **S** **ET_INTEGRITY_INFORMATION** is supported
| | Note: Only ReFS supports integrity
| | Test environment: NTFS, ReFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsIntegritySupported == True &nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsIntegritySupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DataFile)
| | FSCTL request with FSCTL_SET_INTEGRITY_INFORMATION
| | If (IsIntegritySupported == True) {
| | assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488686"/>FsCtl_Set_IntegrityInformation_Dir_IsIntegritySupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test  if FSCTL request **FSCTL_** **S** **ET_INTEGRITY_INFORMATION** is supported
| | Note: Only ReFS supports integrity
| | Test environment: NTFS, ReFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsIntegritySupported == True &nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsIntegritySupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (Directory)
| | FSCTL request with FSCTL_SET_INTEGRITY_INFORMATION
| | If (IsIntegritySupported == True) {
| | assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488687"/>FsCtl_Set_IntegrityInformation_File_InvalidParameter(2TCs)
Parameter combination: (Expected results: STATUS_INVALID_PARAMETER)


| &#32;| &#32; |
| -------------| ------------- |
|  | Parameter| 
| 1| InputBufferSize   &#60;  sizeof(| 
| | FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)| 
| 2| InputBuffer.ChecksumAlgorithm which is not one of CHECKSUM_TYPE_NONE, CHECKSUM_TYPE_CRC64, CHECKSUM_TYPE_UNCHANGED| 
| 3| The operation is attempting to change the checksum state of a non-empty file; the integrity status of files can be changed only when they have not yet been written to.| 







| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **Fs** **Ctl** **_Set** **_** **Integrity** **Information_** **File_** **InputBu** **fferSizeLessThanIntegrityBuffer**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (DataFile)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| |  **In** **putBufferSize**  **=**  **sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)**  **-1**| 
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);| 
| Test case|  **FsC** **tl** **_Set** **_** **Integrity** **Information_** **File_** **Undefined** **ChecksumAlgorithm**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (DataFile)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| | InputBuffer.ChecksumAlgorithm = 0x0003| 
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);| 
| Test case|  **FsCtl_Set_IntegrityInformation_InvalidParameter_NonEmptyFile**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (DataFile)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| | InputBuffer.ChecksumAlgorithm = CHECKSUM_TYPE_CRC64| 
| | Write 1 KB data into this file to make sure it is not empty.| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| | InputBuffer.ChecksumAlgorithm = CHECKSUM_TYPE_NONE| 
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);| 




##### <a name="_Toc427488688"/>FsCtl_Set_IntegrityInformation_Dir_InvalidParameter(2TCs)
Parameter combination: (Expected results: STATUS_INVALID_PARAMETER)


| &#32;| &#32; |
| -------------| ------------- |
|  | Parameter| 
| 1| OutputBufferSize   &#60;  sizeof(| 
| | FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)| 
| 2| InputBuffer.ChecksumAlgorithm which is not one of CHECKSUM_TYPE_NONE, CHECKSUM_TYPE_CRC64, CHECKSUM_TYPE_UNCHANGED| 







| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **FsC** **tl** **_Set** **_Integrity** **Information_** **Dir_InputBu** **fferSizeLessThanIntegrityBuffer**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| |  **OutputBufferSize**  **=**  **sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER)**  **-1**| 
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);| 
| Test case|  **FsC** **t** **l_Set** **_Integrity** **Information_** **Dir_** **Undefined** **ChecksumAlgorithm**| 
| Description| To test parameter check for invalid parameters.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION with| 
| | InputBuffer.ChecksumAlgorithm = 0x0003| 
| | Assert.AreEqual(**STATUS_INVALID_PARAMETER**,ActualResult);| 




##### <a name="_Toc427488689"/>FsCtl_Set_IntegrityInformation_File_WriteProtected

| &#32;| &#32; |
| -------------| ------------- |
| Description| Try to set integrity information for a read only volume, it should fail with STATUS_MEDIA_WRITE_PROTECTED.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: ReFS| 
| Message Sequence| Connect to a read only volume| 
| | OpenFile (DataFile)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(**STATUS_MEDIA_WRITE_PROTECTED**,ActualResult);| 




##### <a name="_Toc427488690"/>FsCtl_Set_IntegrityInformation_Dir_WriteProtected

| &#32;| &#32; |
| -------------| ------------- |
| Description| Try to set integrity information for a read only volume, it should fail with STATUS_MEDIA_WRITE_PROTECTED.| 
| | Note: Only ReFS supports integrity| 
| | Test environment: ReFS| 
| Message Sequence| Connect to a read only volume| 
| | OpenFile (Directory)| 
| | FsCtl request FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(**STATUS_MEDIA_WRITE_PROTECTED**,ActualResult);| 




##### <a name="_Toc427488691"/>FsCtl_Set_IntegrityInformation_File_ChecksumAlgorithm(2TCs)
Test matrix for InputBuffer.ChecksumAlgorithm


| &#32;| &#32; |
| -------------| ------------- |
| ChecksumAlgorithm| ExpectedAlgorithm| 
| CHECKSUM_TYPE_NONE| CHECKSUM_TYPE_NONE| 
| CHECKSUM_TYPE_CRC64| CHECKSUM_TYPE_CRC64| 
| CHECKSUM_TYPE_UNCHANGED| CHECKSUM_TYPE_NONE| 
| CHECKSUM_TYPE_UNCHANGED| CHECKSUM_TYPE_CRC64| 









| &#32;| &#32; |
| -------------| ------------- |
| Test Case|  **FsCtl_Set_IntegrityInformation_File_ChecksumType** **NoneAnd** **Unchanged**| 
| Description| To set integrity information with different checksumAlgorithms.| 
| | Note: Only ReFS supports integrity| 
| | To cover CHECKSUM_TYPE_NONE and CHECKSUM_TYPE_UNCHANGED| 
| Message Sequence| CreateFile| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_NONE| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_NONE, OutputBuffer.CheckSumAlgorithm).| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_UNCHANGED| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_NONE, OutputBuffer.CheckSumAlgorithm).| 
| Test Case|  **FsCtl_Set_IntegrityInformation_File_ChecksumType** **Crc64And** **Unchanged**| 
| Description| To set integrity information with different checksumAlgorithms.| 
| | Note: Only ReFS supports integrity| 
| | To cover CHECKSUM_TYPE_CRC64 and CHECKSUM_TYPE_UNCHANGED| 
| Message Sequence| CreateFile| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_CRC64| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_CRC64, OutputBuffer.CheckSumAlgorithm).| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_CRC64| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_CRC64, OutputBuffer.CheckSumAlgorithm).| 




##### <a name="_Toc427488692"/>FsCtl_Set_IntegrityInformation_Dir_ChecksumAlgorithm(2TCs)
Test matrix for InputBuffer.ChecksumAlgorithm


| &#32;| &#32; |
| -------------| ------------- |
| ChecksumAlgorithm| ExpectedAlgorithm| 
| CHECKSUM_TYPE_NONE| CHECKSUM_TYPE_NONE| 
| CHECKSUM_TYPE_CRC64| CHECKSUM_TYPE_CRC64| 
| CHECKSUM_TYPE_UNCHANGED| CHECKSUM_TYPE_NONE| 
| CHECKSUM_TYPE_UNCHANGED| CHECKSUM_TYPE_CRC64| 









| &#32;| &#32; |
| -------------| ------------- |
| Test Case|  **FsC** **tl_Set_IntegrityInformation_Dir** **_ChecksumType** **NoneAnd** **Unchanged**| 
| Description| To set integrity information with different checksumAlgorithms.| 
| | Note: Only ReFS supports integrity| 
| | To cover CHECKSUM_TYPE_NONE and CHECKSUM_TYPE_UNCHANGED| 
| Message Sequence| CreateFile| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_NONE| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_NONE, OutputBuffer.CheckSumAlgorithm).| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_UNCHANGED| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_NONE, OutputBuffer.CheckSumAlgorithm).| 
| Test Case|  **FsC** **tl_Set_IntegrityInformation_Dir** **_ChecksumType** **Crc64And** **Unchanged**| 
| Description| To set integrity information with different checksumAlgorithms.| 
| | Note: Only ReFS supports integrity| 
| | To cover CHECKSUM_TYPE_CRC64 and CHECKSUM_TYPE_UNCHANGED| 
| Message Sequence| CreateFile| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_CRC64| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_CRC64, OutputBuffer.CheckSumAlgorithm).| 
| | FSCTL request FSCTL_SET_INTEGRITY_INFORMATION  with InputBuffer.ChecksumAlgorithm set to CHECKSUM_TYPE_CRC64| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION| 
| | Assert.AreEqual(STATUS_SUCCESS,actualResult)| 
| | Assert.AreEqual(CHECKSUM_TYPE_CRC64, OutputBuffer.CheckSumAlgorithm).| 



#### <a name="IsOffloadReadSupported"/>IsOffloadReadSupported

##### <a name="_Toc427488693"/>FsCtl_Offload_Read_File_IsOffloadSupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| Support for this read operation is optional. If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.
| | Note: Only ReFS supports integrity
| | Test object: DataFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsOffloadSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsOffloadSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DataFile)
| | FSCTL request with SCTL_OFFLOAD_READ
| | If (IsOffloadSupported == True) {
| | Assert.AreEqual(STATUS_SUCCESS,ActualResult);
| | } Else {
| | Assert.AreEqual(STATUS_INVALID_DEVICE_REQUEST,ActualResult);
| | }



#### <a name="IsOffloadWriteSupported"/>IsOffloadWriteSupported


##### <a name="_Toc427488694"/>FsCtl_Offload_Write_File_IsOffloadSupported (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| Support for this read operation is optional. If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.
| | Note: Only ReFS supports integrity
| | Test object: DataFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsOffloadSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsOffloadSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DataFile)
| | FSCTL request with SCTL_OFFLOAD_WRITE
| | If (IsOffloadSupported == True) {
| | Assert.AreEqual(STATUS_SUCCESS,ActualResult);
| | } Else {
| | Assert.AreEqual(STATUS_INVALID_DEVICE_REQUEST,ActualResult);
| | }











| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **FsInfo_Query_FileFsSectorSizeInformation_** **File_** **OutputBufferSizeLessThanSectorSizeInfo**| 
| Description| To test parameter check for invalid parameters.| 
| | Test environment: NTFS, ReFS| 
| | Test object: file, directory| 
| | Input parameter: | 
| | OutputBufferSize  = sizeof(| 
| | FILE_FS_SECTOR_SIZE_INFORMATION) -1| 
| | Expected Result: STATUS_INFO_LENGTH_MISMATCH| 
| Message Sequence| CreateFile (DataFile)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation, | 
| |  **OutputBufferSize**  **=**  **sizeof(** **FILE_FS_SECTOR_SIZE_INFORMATION** **)**  **-1**| 
| | Assert.AreEqual(**STATUS_INFO_LENGTH_MISMATCH**,ActualResult);| 
| Test case|  **FsInfo_Query_FileFsSectorSizeInformation_** **File_** **OutputBufferSizeEqualToSectorSizeInfo** **(BVT)**| 
| Description| To test parameter check for invalid parameters.| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: | 
| | OutputBufferSize  = sizeof(| 
| | FILE_FS_SECTOR_SIZE_INFORMATION)| 
| | Expected Result: STATUS_SUCCESS| 
| Message Sequence| CreateFile (DataFile)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation, | 
| |  **OutputBufferSize**  **=**  **sizeof(** **FILE_FS_SECTOR_SIZE_INFORMATION** **)**| 
| | Assert.AreEqual(**STATUS_** **SUCCESS**,ActualResult);| 
| Test case|  **FsInfo_Query_FileFsSectorSizeInformation_** **File_** **OutputBufferSizeGreaterThanSectorSizeInfo**| 
| Description| To test parameter check for invalid parameters.| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: | 
| | OutputBufferSize  = sizeof(| 
| | FILE_FS_SECTOR_SIZE_INFORMATION) +1| 
| | Expected Result: STATUS_SUCCESS| 
| Message Sequence| CreateFile (DataFile)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation, | 
| |  **OutputBufferSize**  **=**  **sizeof(** **FILE_FS_SECTOR_SIZE_INFORMATION** **)** **+** **1**| 
| | Assert.AreEqual(**STATUS_** **SUCCESS**,ActualResult);| 



| &#32;| &#32; |
| -------------| ------------- |
| Test case|  **FsInfo_Query_FileFsSectorSizeInformation_** **Dir_** **OutputBufferSizeLessThanSectorSizeInfo**| 
| Description| To test parameter check for invalid parameters.| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: | 
| | OutputBufferSize  = sizeof(| 
| | FILE_FS_SECTOR_SIZE_INFORMATION) -1| 
| | Expected Result: STATUS_INFO_LENGTH_MISMATCH| 
| Message Sequence| CreateFile (DirectoryFile)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation, | 
| |  **OutputBufferSize**  **=**  **sizeof(** **FILE_FS_SECTOR_SIZE_INFORMATION** **)**  **-1**| 
| | Assert.AreEqual(**STATUS_INFO_LENGTH_MISMATCH**,ActualResult);| 
| Test case|  **FsInfo_Query_FileFsSectorSizeInformation_** **Dir_** **OutputBufferSizeEqualToSectorSizeInfo**| 
| Description| To test parameter check for invalid parameters.| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: | 
| | OutputBufferSize  = sizeof(| 
| | FILE_FS_SECTOR_SIZE_INFORMATION)| 
| | Expected Result:  STATUS_SUCCESS| 
| Message Sequence| CreateFile (DirectoryFile)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation, | 
| |  **OutputBufferSize**  **=**  **sizeof(** **FILE_FS_SECTOR_SIZE_INFORMATION** **)**| 
| | Assert.AreEqual(**STATUS_** **SUCCESS**,ActualResult);| 
| Test case|  **FsInfo_Query_FileFsSectorSizeInformation_** **Dir_** **OutputBufferSizeGreaterThanSectorSizeInfo**| 
| Description| To test parameter check for invalid parameters.| 
| | Test environment: NTFS, ReFS| 
| | Input parameter: | 
| | OutputBufferSize  = sizeof(| 
| | FILE_FS_SECTOR_SIZE_INFORMATION) +1| 
| | Expected Result:  STATUS_SUCCESS| 
| Message Sequence| CreateFile (DirectoryFile)| 
| | QueryInfo  with FileSystemInfoClass  = FileFsSectorSizeInformation, | 
| |  **OutputBufferSize**  **=**  **sizeof(** **FILE_FS_SECTOR_SIZE_INFORMATION** **)** **+** **1**| 
| | Assert.AreEqual(**STATUS_** **SUCCESS**,ActualResult);| 




#### <a name="IsSetCompressionSupported"/>IsSetCompressionSupported


##### <a name="_Toc427488719"/>FsCtl_Set_Compression_File_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.
| | Test environment: NTFS, ReFS
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsCompressionSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsCompressionSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile (DataFile)
| | FsCtl request with FSCTL_SET_COMPRESSION
| | If (IsCompressionSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488720"/>FsCtl_Set_Compression_Dir_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsCompressionSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsCompressionSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile  (DirectoryFile)
| | FsCtl request with FSCTL_SET_COMPRESSION
| | If (IsCompressionSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }



#### <a name="IsGetCompressionSupported"/>IsGetCompressionSupported


##### <a name="_Toc427488717"/>FsCtl_Get_Compression_File_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsCompressionSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsCompressionSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile 
| | FsCtl request with FSCTL_GET_COMPRESSION
| | If (IsCompressionSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488718"/>FsCtl_Get_Compression_Dir_IsCompressionSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if compression is supported for different file systems.
| | Test environment: NTFS, ReFS
| | Test object: DataFile, DirectoryFile
| | Parameter combination.
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsCompressionSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsCompressionSupported == False &nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| CreateFile 
| | FsCtl request with FSCTL_GET_COMPRESSION
| | If (IsCompressionSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




#### <a name="IsGetVolumeDataSupported"/>IsGetVolumeDataSupported

##### <a name="FsCtl_Get_REFS_Volume_Data_File"/>FsCtl_Get_REFS_Volume_Data_File


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_GET_REFS_VOLUME_DATA is supported.| 
| | Note: This is only implemented by the **REFS** file system file system.| 
| | Test environment: ReFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_GET_REFS_VOLUME_DATA| 
| Message Sequence| Create test file.| 
| | Query FileIdInformation.|
| | Query FileFsFullSizeInformation.|
| | FSCTL request with FSCTL_GET_REFS_VOLUME_DATA.| 
| | Verify returned STATUS_SUCCESS and Verify returned REFS_VOLUME_DATA_BUFFER:| 
| | 1. VolumeSerialNumber of REFS_VOLUME_DATA_BUFFER should be the same with VolumeSerialNumber of FileIdInformation.| 
| | 2. TotalClusters of REFS_VVOLUME_DATA_BUFFER should be the same with TotalAllocationUnits of FileFsFullSizeInformation.| 
| | 3. BytesPerSector of REFS_VVOLUME_DATA_BUFFER should be the same with BytesPerSector of FileFsFullSizeInformation.|



##### <a name="FsCtl_Get_REFS_Volume_Data_Dir"/>FsCtl_Get_REFS_Volume_Data_Dir


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_GET_REFS_VOLUME_DATA is supported.| 
| | Note: This is only implemented by the **REFS** file system file system.| 
| | Test environment: ReFS| 
| | Test object: DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_GET_REFS_VOLUME_DATA| 
| Message Sequence| Create test file directory.| 
| | Query FileIdInformation.|
| | Query FileFsFullSizeInformation.|
| | FSCTL request with FSCTL_GET_REFS_VOLUME_DATA.| 
| | Verify returned STATUS_SUCCESS and Verify returned REFS_VOLUME_DATA_BUFFER:| 
| | 1. VolumeSerialNumber of REFS_VOLUME_DATA_BUFFER should be the same with VolumeSerialNumber of FileIdInformation.| 
| | 2. TotalClusters of REFS_VVOLUME_DATA_BUFFER should be the same with TotalAllocationUnits of FileFsFullSizeInformation.| 
| | 3. BytesPerSector of REFS_VVOLUME_DATA_BUFFER should be the same with BytesPerSector of FileFsFullSizeInformation.|



##### <a name="FsCtl_Get_NTFS_Volume_Data_File"/>FsCtl_Get_NTFS_Volume_Data_File


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_GET_NTFS_VOLUME_DATA is supported.| 
| | Note: This is only implemented by the **NTFS** file system file system.| 
| | Test environment: NTFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_GET_NTFS_VOLUME_DATA| 
| Message Sequence| Create test file.| 
| | Query FileIdInformation.|
| | Query FileFsFullSizeInformation.|
| | FSCTL request with FSCTL_GET_NTFS_VOLUME_DATA.| 
| | Verify returned STATUS_SUCCESS and Verify returned NTFS_VOLUME_DATA_BUFFER:| 
| | 1. VolumeSerialNumber of NTFS_VOLUME_DATA_BUFFER should be the same with VolumeSerialNumber of FileIdInformation.| 
| | 2. TotalClusters of NTFS_VVOLUME_DATA_BUFFER should be the same with TotalAllocationUnits of FileFsFullSizeInformation.| 
| | 3. BytesPerSector of NTFS_VVOLUME_DATA_BUFFER should be the same with BytesPerSector of FileFsFullSizeInformation.|



##### <a name="FsCtl_Get_NTFS_Volume_Data_Dir"/>FsCtl_Get_NTFS_Volume_Data_Dir


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_GET_NTFS_VOLUME_DATA is supported.| 
| | Note: This is only implemented by the **NTFS** file system file system.| 
| | Test environment: NTFS| 
| | Test object: DirectoryFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_GET_NTFS_VOLUME_DATA| 
| Message Sequence| Create test file directory.| 
| | Query FileIdInformation.|
| | Query FileFsFullSizeInformation.|
| | FSCTL request with FSCTL_GET_NTFS_VOLUME_DATA.| 
| | Verify returned STATUS_SUCCESS and Verify returned NTFS_VOLUME_DATA_BUFFER:| 
| | 1. VolumeSerialNumber of NTFS_VOLUME_DATA_BUFFER should be the same with VolumeSerialNumber of FileIdInformation.| 
| | 2. TotalClusters of NTFS_VVOLUME_DATA_BUFFER should be the same with TotalAllocationUnits of FileFsFullSizeInformation.| 
| | 3. BytesPerSector of NTFS_VVOLUME_DATA_BUFFER should be the same with BytesPerSector of FileFsFullSizeInformation.|


#### <a name="IsQueryFileRegionsSupported"/>IsQueryFileRegionsSupported

##### <a name="BVT_FsCtl_Query_File_Regions"/> BVT_FsCtl_Query_File_Regions


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_QUERY_FILE_REGIONS is supported.| 
| | Note: This is only implemented by the **REFS** file system and **NTFS** file system.| 
| | Test environment: ReFS and NTFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_QUERY_FILE_REGIONS| 
| Message Sequence| Create test file and fill it up with random data.| 
| | FSCTL request with FSCTL_QUERY_FILE_REGIONS, not containing input data.| 
| | Verify returned STATUS_SUCCESS and Verify returned FILE_REGION_OUTPUT:| 
| | 1. RegionEntryCount of the output data element should be 1.| 
| | 2. TotalRegionEntryCount of the output data element should be 1.| 
| | 3. FileOffset of the FILE_REGION_INFO data element should be zero.|
| | 4. Length of the FILE_REGION_INFO data element should be the size we wrote to the file.|
| | 5. FILE_REGION_USAGE_VALID_CACHED_DATA should be set for NTFS, or FILE_REGION_USAGE_VALID_NONCACHED_DATA should be set for REFS.|


##### <a name="BVT_FsCtl_Query_File_Regions_WithInputData"/> BVT_FsCtl_Query_File_Regions_WithInputData


| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if FSCTL_QUERY_FILE_REGIONS is supported.| 
| | Note: This is only implemented by the **REFS** file system and **NTFS** file system.| 
| | Test environment: ReFS and NTFS| 
| | Test object: DataFile| 
| | Test coverage:| 
| | FsCtl: FSCTL_QUERY_FILE_REGIONS| 
| Message Sequence| Create test file and fill it up with random data.| 
| | FSCTL request with FSCTL_QUERY_FILE_REGIONS, containing input data.| 
| | Verify returned STATUS_SUCCESS and Verify returned FILE_REGION_OUTPUT:| 
| | 1. RegionEntryCount of the output data element should be 1.| 
| | 2. TotalRegionEntryCount of the output data element should be 1.| 
| | 3. FileOffset of the FILE_REGION_INFO data element should be zero.|
| | 4. Length of the FILE_REGION_INFO data element should be the size we wrote to the file.|
| | 5. FILE_REGION_USAGE_VALID_CACHED_DATA should be set for NTFS, or FILE_REGION_USAGE_VALID_NONCACHED_DATA should be set for REFS.|



### <a name="Test-cases-for-QuotaInformation"/>Test cases for QuotaInformation


#### <a name="_Toc427488748"/> IsQuotaInfoSupported

##### <a name="_Toc427488749"/>QuotaInfo_Query_QuotaInformation_IsQuotaInfoSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if quota info is supported for different file systems.
| | Note: Only NTFS supports quotas.
| | Test environment: NTFS
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsQuotaInfoSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsQuotaInfoSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| Open Quota file
| | QueryInfo with FileInfoClass.FileQuotaInformation
| | If (IsQuotaInfoSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




##### <a name="_Toc427488750"/>QuotaInfo_Set_QuotaInformation_IsQuotaInfoSupported

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if quota info is supported for different file systems.
| | Note: Only NTFS supports quotas.
| | Test environment: NTFS
| | Parameter combination
| | Parameter&nbsp;&#124;&nbsp;Expected Result
| | IsQuotaInfoSupported == True&nbsp;&#124;&nbsp;STATUS_SUCCESS
| | IsQuotaInfoSupported == False&nbsp;&#124;&nbsp;STATUS_INVALID_DEVICE_REQUEST
| Message Sequence| Open Quota file
| | SetInfo with FileInfoClass.FileQuotaInformation
| | If (IsQuotaInfoSupported == True) {
| | Assert.AreEqual(**STATUS_SUCCESS**,ActualResult);
| | } Else {
| | Assert.AreEqual(**STATUS_INVALID_DEVICE_REQUEST**,ActualResult);
| | }




### <a name="_Toc427488768"/>Test cases for Alternate Data Stream

#### <a name="IsCreateStreamSupported"/>IsCreateStreamSupported


##### <a name="_Toc427488770"/>BVT_AlternateDataStream_CreateStream_File (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream create is supported on a Data file|
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (DataFile)|
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Verify server return with **STATUS_SUCCESS** for supported file system|  




##### <a name="_Toc427488771"/>BVT_AlternateDataStream_CreateStream_Dir (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream create is supported on a Directory file|
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (DirectoryFile)|
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Verify server return with **STATUS_SUCCESS** for supported file system|  


#### <a name="IsListStreamSupported"/>IsListStreamSupported

##### <a name="_Toc427488773"/>BVT_AlternateDataStream_ListStreams_File (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream list is supported on a Data file|
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (DataFile)| 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify server return with streamname and streamsize|




##### <a name="_Toc427488774"/>BVT_AlternateDataStream_ListStreams_Dir (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream list is supported on a Directory file|
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (Directory)| 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify server return with streamname and streamsize|


#### <a name="IsDeleteStreamSupported"/>IsDeleteStreamSupported

##### <a name="_Toc427488776"/>BVT_AlternateDataStream_DeleteStream_File (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream delete is supported on a Data file| 
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (Data)|
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Delete the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|




##### <a name="_Toc427488777"/>BVT_AlternateDataStream_DeleteStream_Dir (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream delete is supported on a Directory file| 
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (Directory)|
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Delete the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|


#### <a name="IsRenameStreamSupported"/>IsRenameStreamSupported

##### <a name="_Toc427488779"/>BVT_AlternateDataStream_RenameStream_File (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream rename is supported on a Data file|
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|
| | Rename the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|




##### <a name="_Toc427488780"/>BVT_AlternateDataStream_RenameStream_Dir (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Alternate Data Stream rename is supported on a Directory file|
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|
| | Rename the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | List all the Alternate Data Streams created on this file|
| | Verify server return with streamname and streamsize|


#### <a name="IsWriteAndReadSupported"/>IsWriteAndReadSupported


##### <a name="_Toc427488782"/>BVT_AlternateDataStream_WriteAndRead_File (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if write and read from the Alternate Data Streams is supported on a Data file| 
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Read the bytes from the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify the bytes read and the bytes written on this stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Read the bytes from the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify the bytes read and the bytes written on this stream|




##### <a name="_Toc427488783"/>BVT_AlternateDataStream_WriteAndRead_Dir (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if write and read from the Alternate Data Streams is supported on a Directory file| 
| | Test environment: NTFS, ReFS|
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Read the bytes from the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify the bytes read and the bytes written on this stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Read the bytes from the Alternate Data Streams created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Verify the bytes read and the bytes written on this stream|



#### <a name="IsLockAndUnlockSupported"/>IsLockAndUnlockSupported


##### <a name="_Toc427488785"/>BVT_AlternateDataStream_LockAndUnlock_File (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if lock and unlock a byte range of the Alternate Data Streams is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Lock a byte range of the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Unlock the byte range of the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488785"/>BVT_AlternateDataStream_LockAndUnlock_File (BVT)

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if lock and unlock a byte range of the Alternate Data Streams is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Create another Alternate Data Stream and write 4096 bytes to the stream|
| | Lock a byte range of the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Unlock the byte range of the second Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|


#### <a name="IsQueryAndSetFileInformationSupported"/>IsQueryAndSetFileInformationSupported

##### <a name="_Toc427488788"/>AlternateDataStream_Query_FileAccessInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileAccessInformation query of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488789"/>AlternateDataStream_Query_FileAccessInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileAccessInformation query of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488790"/>AlternateDataStream_Query_FileBasicInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileBasicInformation query of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488791"/>AlternateDataStream_Query_FileBasicInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileBasicInformation query of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488792"/>AlternateDataStream_Query_FileCompressionInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileCompressionInformation query of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488793"/>AlternateDataStream_Query_FileCompressionInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileCompressionInformation query of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488794"/>AlternateDataStream_Query_FileNetworkOpenInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileNetworkOpenInformation query of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488795"/>AlternateDataStream_Query_FileNetworkOpenInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileNetworkOpenInformation query of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488796"/>AlternateDataStream_Query_FileStandardInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileStandardInformation query of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488797"/>AlternateDataStream_Query_FileStandardInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileStandardInformation query of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488798"/>AlternateDataStream_Set_FileEaInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileEaInformation set of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488799"/>AlternateDataStream_Set_FileEaInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileEaInformation set of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488798"/>AlternateDataStream_Set_FileShortNameInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileShortNameInformation set of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488799"/>AlternateDataStream_Set_FileShortNameInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileShortNameInformation set of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488802"/>AlternateDataStream_Set_FileValidDataLengthInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileValidDataLengthInformation set of the Alternate Data Streams on a file is supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488803"/>AlternateDataStream_Set_FileValidDataLengthInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FileValidDataLengthInformation set of the Alternate Data Streams on a file is supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Query or set the file information on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|


#### <a name="IsFsControlSupported"/>IsFsControlSupported

##### <a name="_Toc427488805"/>AlternateDataStream_FsCtl_Get_Compression_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_GET_COMPRESSION request and response on the Alternate Data Streams are supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488806"/>AlternateDataStream_FsCtl_Get_Compression_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_GET_COMPRESSION request and response on the Alternate Data Streams are supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488807"/>AlternateDataStream_FsCtl_Get_IntegrityInformation_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_GET_INTEGRITY_INFORMATION request and response on the Alternate Data Streams are supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488808"/>AlternateDataStream_FsCtl_Get_IntegrityInformation_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_GET_INTEGRITY_INFORMATION request and response on the Alternate Data Streams are supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488809"/>AlternateDataStream_FsCtl_Query_AllocatedRanges_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_QUERY_ALLOCATED_RANGES request and response on the Alternate Data Streams are supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488810"/>AlternateDataStream_FsCtl_Query_AllocatedRanges_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_QUERY_ALLOCATED_RANGES request and response on the Alternate Data Streams are supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488811"/>AlternateDataStream_FsCtl_Set_Compression_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_SET_COMPRESSION request and response on the Alternate Data Streams are supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488812"/>AlternateDataStream_FsCtl_Set_Compression_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_SET_COMPRESSION request and response on the Alternate Data Streams are supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|




##### <a name="_Toc427488813"/>AlternateDataStream_FsCtl_Set_ZeroData_File

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_SET_ZERO_DATA request and response on the Alternate Data Streams are supported on a Data file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Data) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|



##### <a name="_Toc427488814"/>AlternateDataStream_FsCtl_Set_ZeroData_Dir

| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if the FSCTL_SET_ZERO_DATA request and response on the Alternate Data Streams are supported on a Directory file| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) | 
| | Create an Alternate Data Stream and write 2048 bytes to the stream|
| | Request a FsControl on this Alternate Data Stream created on this file|
| | Verify server return with **STATUS_SUCCESS** for supported file system|

### <a name="Test-cases-for-other-scenarios"/>Test cases for Create and Query Directory Scenarios

#### <a name="QueryDirectory"/>QueryDirectory

##### <a name="_Toc427488836"/> Fs_CreateDirectory_QueryDirectory_Suffix_I30_INDEX_ALLOCATION
| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Create and Query directory with suffix :$I30:$INDEX_ALLOCATION is supported.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) with suffix :$I30:$INDEX_ALLOCATION| 
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Query server directory with created directory name|
| | Verify server return with **STATUS_SUCCESS** for supported file system|

##### <a name="_Toc427488837"/> Fs_CreateDirectory_QueryDirectory_Suffix_INDEX_ALLOCATION
| &#32;| &#32; |
| -------------| ------------- |
| Description| To test if Create and Query directory with suffix ::$INDEX_ALLOCATION is supported.| 
| | Test environment: NTFS, ReFS| 
| Message Sequence| CreateFile (Directory) with suffix ::$INDEX_ALLOCATION| 
| | Verify server return with **STATUS_SUCCESS** for supported file system|
| | Query server directory with created directory name|
| | Verify server return with **STATUS_SUCCESS** for supported file system|


### <a name="Test-cases-for-other-scenarios"/>Test cases for Other Scenarios

#### <a name="IsCreateFileSupported"/>IsCreateFileSupported

##### <a name="_Toc427488835"/> CreateFile_InvalidStreamName

| &#32;| &#32; |
| -------------| ------------- |
| Description| Try to create a directory with invalid stream name and expect failure.
| | Note: Only the NTFS and ReFS file systems support complex name suffixes and StreamTypeNames. File systems that do not support this return STATUS_OBJECT_NAME_INVALID.
| | Test object: DirectoryFile
| Message Sequence| CreateFile (DirectoryFile)
| | If (FileSystem == NTFS \|\| FileSystem == REFS) {
| | Assert.AreEqual(STATUS_INVALID_PARAMETER,ActualResult);
| | } Else {
| | Assert.AreEqual(STATUS_OBJECT_NAME_INVALID,ActualResult);
| | }




## <a name="_Toc427488815"/>MBT Test Design

### <a name="_Toc427488816"/>Model Design
Here is a list for Win8 newly added algorithms, the designed scenarios are based on them.



### <a name="_Toc427488817"/>Adapter Design
The MS-FSA test suite implements 2 adapters: 

* Protocol Adapter 

* Transport adapter (SMB_TransportAdapter or SMB2_TransportAdapter).



#### <a name="_Toc427488818"/>Protocol Adapter
The MS-FSA Protocol adapter is called FSAAdapter in the Test Suite. It implements the interface IFSAAdapter. Its functionality is to communicate with the transport adapter, where called relevant methods to interact with the file system. Another functionality of the Protocol Adapter is when the server replies, it parses the messages, set the output value for test.
Class Diagram


![image3.png](./image/MS-FSA_ServerTestDesignSpecification/image3.png)



#### <a name="_Toc427488819"/>Transport Adapter
There are 3 transport adapters implement the interface ITransportAdapter.

* SMBTransportAdapter

* It uses SMB SDK to send/receive SMB packet to/from SMB server.

* It uses SMB dialects for negotiation.



* SMB2TransportAdapter

* It uses SMB2 SDK to send/receive SMB packet to/from SMB server.

* It uses SMB2 dialects for negotiation including 0x0202 and 0x0210.



* SMB3TransportAdapter

* It uses SMB3 SDK to send/receive SMB packet to/from SMB server.

* It uses SMB3 dialects for negotiation including 0x0224 and 0x0300.



Class Diagram




![image4.png](./image/MS-FSA_ServerTestDesignSpecification/image4.png)





#### <a name="_Toc427488820"/>Message Sequence
Below diagram shows the message sequence between FSA adapter, transport adapter and SMB server.


![image5.png](./image/MS-FSA_ServerTestDesignSpecification/image5.png)


### <a name="_Toc427488821"/>Scenarios
Here is a list for MBT scenarios:


|  **Scenario**|  **Description**| 
| -------------| ------------- |
| S1_MS-FSA_CreateFile| To test all requirements of creating a file.| 
| S2_MS-FSA_OpenFile| To test all requirements of opening a file.| 
| S3_MS-FSA_ReadFile| To test all requirements of reading a file.| 
| S4_MS-FSA_WriteFile| To test all requirements of writing a file.| 
| S5_MS-FSA_CloseOpen| To test all requirements of closing a file.| 
| S6_MS-FSA_QueryDirectory| To test all requirements of querying a directory.| 
| S7_MS-FSA_FlushCache| To test all requirements of flushing cache.| 
| S8_MS-FSA_ByteRangeLockAndUnlock| To test all requirements of locking and unlocking Byte Range.| 
| S9_MS-FSA_FsControl| To test all requirements of Controlling File System.| 
| S10_MS-FSA_ChangeNotification| To test all requirements of change notifications.| 
| S11_MS-FSA_QueryFileInfo| To test all requirements of querying file information.| 
| S12_MS-FSA_QueryFSInfo| To test all requirements of querying file system information.| 
| S13_MS-FSA_QuerySecurityInfo| To test all requirements of querying security information.| 
| S14_MS-FSA_SetFileInfo| To test all requirements of setting file information.| 
| S15_MS-FSA_SetFSInfo| To test all requirements of setting file system information.| 
| S16_MS-FSA_SetSecurityInfo| To test all requirements of setting security information.| 
| S17_MS-FSA_Oplock| To test all requirements of oplock.| 
