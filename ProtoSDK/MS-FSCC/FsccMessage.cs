// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    #region 2.3   FSCTL Structures

    #region FsControlCommand (FSCTL name, FSCTL function number)

    /// <summary>
    /// File System Control command. A process invokes an FSCTL on a handle to perform some action against the file  
    /// or directory associated with the handle. When a server receives an FSCTL request, it SHOULD use the  
    /// information in the request, which includes a handle and, optionally, an input data buffer, to perform the  
    /// requested action. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum FsControlCommand : int
    {
        /// <summary>
        /// This message requests that the server return the object identifier for the file or directory associated   
        /// with the handle on which this FSCTL was invoked. If no object identifier exists, the server MUST create   
        /// one. 
        /// </summary>
        FSCTL_CREATE_OR_GET_OBJECT_ID = 0x900c0,

        /// <summary>
        /// This message requests that the server remove the object identifier from the file or directory associated   
        /// with the handle on which this FSCTL was invoked. The underlying object MUST NOT be deleted. If the file or 
        ///  directory has no object identifier, the request MUST be considered successful. 
        /// </summary>
        FSCTL_DELETE_OBJECT_ID = 0x900a0,

        /// <summary>
        /// This message requests that the server delete the reparse point from the file or directory associated with  
        ///  the handle on which this FSCTL was invoked. The underlying file or directory MUST NOT be deleted. 
        /// </summary>
        FSCTL_DELETE_REPARSE_POINT = 0x900ac,

        /// <summary>
        /// The FSCTL_DUPLICATE_EXTENTS_TO_FILE request message requests that the server copy the specified portion of one file 
        /// (that is the source file) into a specified portion of another file (target file) on the same volume. 
        /// </summary>
        FSCTL_DUPLICATE_EXTENTS_TO_FILE = 0x98344,

        /// <summary>
        /// The FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX request message requests that the server copy the specified 
        /// portion of the source file into a specified portion of the target file on the same volume. 
        /// </summary>
        FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX = 0x983E8,

        /// <summary>
        /// The FSCTL_FILE_LEVEL_TRIM operation informs the underlying storage medium that the contents of the given range of the file no longer needs to be maintained. 
        /// </summary>
        FSCTL_FILE_LEVEL_TRIM = 0x98208,

        /// <summary>
        /// This message requests that the server return the statistical information of the file system such as Type,  
        ///  Version, and so on, as specified in FSCTL_FILESYSTEM_GET_STATISTICS reply, for the file or directory   
        /// associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_FILESYSTEM_GET_STATISTICS = 0x90060,

        /// <summary>
        /// The FSCTL_FIND_FILES_BY_SID request message requests that the server return a list of the files (in the  
        /// directory associated with the handle on which this FSCTL was invoked) whose owner matches the specified   
        /// security identifier (SID). This message contains a FIND_BY_SID_DATA data element. 
        /// </summary>
        FSCTL_FIND_FILES_BY_SID = 0x9008f,

        /// <summary>
        /// This message requests that the server return the current compression state of the file or directory   
        /// associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_COMPRESSION = 0x9003c,

        /// <summary>
        /// FSCTL function number for FSCTL_GET_INTEGRITY_INFORMATION.
        /// </summary>
        FSCTL_GET_INTEGRITY_INFORMATION = 0x9027c,

        /// <summary>
        /// This message requests that the server return information about the NTFS file system volume that contains   
        /// the file or directory that is associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_NTFS_VOLUME_DATA = 0x90064,

        /// <summary>
        /// This message requests that the server return information about the ReFS file system volume that contains
        /// the file or directory that is associated with the handle on which this FSCTL was invoked.
        /// </summary>
        FSCTL_GET_REFS_VOLUME_DATA = 0X902D8,

        /// <summary>
        /// This message requests that the server return the object identifier for the file or directory associated  
        /// with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_OBJECT_ID = 0x9009c,

        /// <summary>
        /// This message requests that the server return the reparse point data for the file or directory associated   
        /// with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_REPARSE_POINT = 0x900a8,

        /// <summary>
        /// The FSCTL_GET_RETRIEVAL_POINTERS request message requests that the server return the virtual cluster  
        /// number to the logical cluster number mapping information that describes the location the on disk of the  
        /// extents within the file or directory associated with the handle on which this FSCTL was invoked. This  
        /// mapping  information is most commonly used by defragmentation utilities. This message contains a   
        /// STARTING_VCN_INPUT_BUFFER data element. 
        /// </summary>
        FSCTL_GET_RETRIEVAL_POINTERS = 0x90073,

        /// <summary>
        /// The FSCTL_IS_PATHNAME_VALID request message requests that the server indicate whether the specified   
        /// pathname is well-formed (of acceptable length, with no invalid characters, and so on - see section 2.1.5)  
        ///  with respect to the volume that contains the file or directory associated with the handle on which this   
        /// FSCTL was invoked. 
        /// </summary>
        FSCTL_IS_PATHNAME_VALID = 0x9002c,

        /// <summary>
        /// The FSCTL_LMR_SET_LINK_TRACKING_INFORMATION request message sets Distributed Link Tracking information  
        /// such as file system type, volume ID, object ID, and destination computer's NetBIOS name for the file or   
        /// directory associated with the handle on which this FSCTL was invoked. The message contains a   
        /// REMOTE_LINK_TRACKING_INFORMATION data element. For more information about Distributed Link Tracking, see  
        /// [MS-DLTW] section 3.1.6. 
        /// </summary>
        FSCTL_LMR_SET_LINK_TRACKING_INFORMATION = 0x1400ec,

        /// <summary>
        /// FSCTL function number for FSCTL_OFFLOAD_READ.
        /// </summary>
        FSCTL_OFFLOAD_READ = 0x94264,

        /// <summary>
        /// FSCTL function number for FSCTL_OFFLOAD_WRITE.
        /// </summary>
        FSCTL_OFFLOAD_WRITE = 0x98268,

        /// <summary>
        /// The FSCTL_PIPE_PEEK request requests that the server copy a named pipe's data into a buffer for preview   
        /// without removing it. The FSCTL_PIPE_PEEK request message is issued to invoke a reply, and does not have an 
        ///  associated data structure. 
        /// </summary>
        FSCTL_PIPE_PEEK = 0x11400c,

        /// <summary>
        /// The FSCTL_PIPE_TRANSCEIVE request is used to send and receive data from an open pipe. Any bytes in the   
        /// FSCTL input buffer are written as a binary large object (BLOB) to the input buffer of the pipe server. 
        /// </summary>
        FSCTL_PIPE_TRANSCEIVE = 0x11c017,

        /// <summary>
        /// The FSCTL_PIPE_WAIT request requests that the server wait until either a time-out interval elapses or an   
        /// instance of the specified named pipe is available for connection. 
        /// </summary>
        FSCTL_PIPE_WAIT = 0x110018,

        /// <summary>
        /// This message requests that the server return the first 0x24 bytes of sector 0 for the volume that contains 
        ///   the file or directory associated with the handle on which this FSCTL was invoked. The first 0x24 bytes 
        /// of   sector 0 are known as the FAT BIOS Parameter Block (BPB), which contains hardware-specific bootstrap  
        ///  information. 
        /// </summary>
        FSCTL_QUERY_FAT_BPB = 0x90058,

        /// <summary>
        /// The FSCTL_QUERY_ALLOCATED_RANGES request message requests that the server scan a file or alternate stream  
        ///  looking for byte ranges that may contain nonzero data, and then return information on those ranges. Only  
        /// sparse files can have zeroed ranges known to the operating system. For other files, the server will return 
        ///  only a single range that contains the starting point and the length requested. The message contains a   
        /// FILE_ALLOCATED_RANGE_BUFFER data element. 
        /// </summary>
        FSCTL_QUERY_ALLOCATED_RANGES = 0x940cf,

        /// <summary>
        /// This message requests UDF-specific volume information for the volume that contains the file or directory   
        /// associated with the handle on which this FSCTL was invoked. This FSCTL is only valid on UDF file systems.  
        /// All other File Systems will treat this as an invalid FSCTL. 
        /// </summary>
        FSCTL_QUERY_ON_DISK_VOLUME_INFO = 0x9013c,

        /// <summary>
        /// Retrieves the defect management properties of the volume that contains the file or directory associated   
        /// with the handle on which this FSCTL was invoked. This FSCTL is only valid on UDF file systems. All other  
        /// File Systems will treat this as an invalid FSCTL. 
        /// </summary>
        FSCTL_QUERY_SPARING_INFO = 0x90138,

        /// <summary>
        /// This message requests that the server return the most recent change journal USN for the file or directory  
        ///  associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_READ_FILE_USN_DATA = 0x900eb,

        /// <summary>
        /// This message requests that the server recall the file (associated with the handle on which this FSCTL was  
        ///  invoked) from storage media that Remote Storage manages. This FSCTL is not valid for directories. 
        /// </summary>
        FSCTL_RECALL_FILE = 0x90117,

        /// <summary>
        /// The FSCTL_SET_COMPRESSION request message requests that the server set the compression state of the file  
        /// or directory associated with the handle on which this FSCTL was invoked. The message contains a 16-bit  
        /// unsigned integer. 
        /// </summary>
        FSCTL_SET_COMPRESSION = 0x9c040,

        /// <summary>
        /// Sets the software defect management state for the specified file associated with the handle on which this  
        /// FSCTL was invoked. Used for UDF file systems. This FSCTL is only valid on UDF file systems. All other file 
        ///  systems will treat this as an invalid FSCTL. 
        /// </summary>
        FSCTL_SET_DEFECT_MANAGEMENT = 0x98134,

        /// <summary>
        /// The FSCTL_SET_ENCRYPTION request sets the encryption for the file or directory associated with the given   
        /// handle. The message contains an encryption buffer which indicates whether to encrypt/decrypt a file or an  
        /// individual stream. 
        /// </summary>
        FSCTL_SET_ENCRYPTION = 0x900D7,

        /// <summary>
        /// FSCTL function number for FSCTL_SET_INTEGRITY_INFORMATION.
        /// </summary>
        FSCTL_SET_INTEGRITY_INFORMATION = 0x9C280,

        /// <summary>
        /// This message sets the object identifier for the file or directory associated with the handle on which this 
        ///   FSCTL was invoked. The message contains a FILE_OBJECTID_BUFFER (section 2.1.3) data element. Either a  
        /// Type 1 or a Type 2 buffer is valid. 
        /// </summary>
        FSCTL_SET_OBJECT_ID = 0x90098,

        /// <summary>
        /// The FSCTL_SET_OBJECT_ID_EXTENDED request message requests that the server set the extended information for 
        ///   the file or directory associated with the handle on which this FSCTL was invoked. The message contains 
        /// an  EXTENDED_INFO data element. 
        /// </summary>
        FSCTL_SET_OBJECT_ID_EXTENDED = 0x900bc,

        /// <summary>
        /// This message requests that the server set a reparse point on the file or directory associated with the  
        /// handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_SET_REPARSE_POINT = 0x900a4,

        /// <summary>
        /// This message requests that the server mark the file that is associated with the handle on which this FSCTL 
        ///   was invoked as sparse. In a sparse file, large ranges of zeros (0) may not require disk allocation. 
        /// Space   for nonzero data is allocated as the file is written. The message contains a 
        /// FILE_SET_SPARSE_BUFFER   element. 
        /// </summary>
        FSCTL_SET_SPARSE = 0x900c4,

        /// <summary>
        /// The FSCTL_SET_ZERO_DATA request message requests that the server fill the specified range of the file   
        /// (associated with the handle on which this FSCTL was invoked) with zeros. The message contains a   
        /// FILE_ZERO_DATA_INFORMATION element. 
        /// </summary>
        FSCTL_SET_ZERO_DATA = 0x980c8,

        /// <summary>
        /// This message requests that the server fill the clusters of the target file with zeros when they are   
        /// deallocated. This is used to set a file to secure delete mode, which ensures that data will be zeroed upon 
        ///  file truncation or deletion. 
        /// </summary>
        FSCTL_SET_ZERO_ON_DEALLOCATION = 0x90194,

        /// <summary>
        /// The FSCTL_SIS_COPYFILE request message requests that the server use the single-instance storage (SIS)   
        /// filter to copy a file. The message contains an SI_COPYFILE data element. For more information about   
        /// single-instance storage, see [SIS]. 
        /// </summary>
        FSCTL_SIS_COPYFILE = 0x90100,

        /// <summary>
        /// This message requests that the server generate a record in the server's file system change journal stream  
        ///  for the file or directory associated with the handle on which this FSCTL was invoked, indicating that the 
        ///   file or directory was closed. This FSCTL can be called independently of the actual file close operation 
        /// to  write a USN record and cause a post of any pending USN updates for the indicated file. 
        /// </summary>
        FSCTL_WRITE_USN_CLOSE_RECORD = 0x900ef,

        /// <summary>
        /// The FSCTL_QUERY_FILE_REGIONS request message requests that the server return a list of file regions, based 
        /// on a specified usage parameter, for the file associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_QUERY_FILE_REGIONS = 0x90284,
    }

    #endregion

    #region FSCTL Structures

    /// <summary>
    /// There is a DesiredUsage field in both FILE_REGION_INPUT data element and FILE_REGION_INFO data element.
    /// The following table provides the currently defined usage parameters. 
    /// </summary>
    public enum FILE_REGION_USAGE : uint
    {
        None = 0,

        /// <summary>
        /// Information about the valid data length for the specified file and file range in the cache will be returned.
        /// </summary>
        FILE_REGION_USAGE_VALID_CACHED_DATA = 0x00000001,

        /// <summary>
        /// Information about the valid data length for the specified file and file range on disk will be returned.
        /// </summary>
        FILE_REGION_USAGE_VALID_NONCACHED_DATA = 0x00000002,
    }
    /// <summary>
    /// The FSCTL_QUERY_FILE_REGIONS request message requests that the server return a list of file regions, 
    /// based on a specified usage parameter, for the file associated with the handle on which this FSCTL was invoked. 
    /// This message contains an optional FILE_REGION_INPUT data element. 
    /// A FILE_REGION_INPUT data element is as follows. 
    /// </summary>
    public partial struct FILE_REGION_INPUT
    {
        /// <summary>
        /// A 64-bit signed integer that contains the file offset, in bytes, of the start of a range of bytes in a file.
        /// </summary>
        public long FileOffset;

        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes, of the range.
        /// </summary>
        public long Length;

        /// <summary>
        /// A 32-bit unsigned integer that indicates usage parameters for this operation. 
        /// </summary>
        public FILE_REGION_USAGE DesiredUsage;

        /// <summary>
        /// Reserved.
        /// </summary>
        public uint Reserved;
    }

    /// <summary>
    /// The FSCTL_QUERY_FILE_REGIONS reply message returns the results of the FSCTL_QUERY_FILE_REGION Request as a 
    /// variably sized data element, FILE_REGION_OUTPUT, which contains one or more FILE_REGION_INFO elements that contain 
    /// the ranges computed as a result of the desired usage.
    /// </summary>
    public partial struct FILE_REGION_OUTPUT
    {
        /// <summary>
        /// A 32-bit unsigned integer that indicates the flags for this operation. No flags are currently defined, 
        /// thus this field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the total number of regions that could be returned.
        /// </summary>
        public uint TotalRegionEntryCount;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the number of regions that were actually returned and which are contained in this structure.
        /// </summary>
        public uint RegionEntryCount;

        /// <summary>
        /// A 32-bit unsigned integer that is reserved. This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// One or more FILE_REGION_INFO structures, as specified in section 2.3.42.1, that contain information on the desired ranges based on the desired usage indicated by the DesiredUsage field.
        /// </summary>
        [Size("RegionEntryCount")]
        public FILE_REGION_INFO[] Region;
    }

    /// <summary>
    /// The FILE_REGION_INFO structure contains a computed region of a file based on a desired usage. 
    /// This structure is used to store region information for the FSCTL_QUERY_FILE_REGIONS reply message, 
    /// with the FILE_REGION_OUTPUT structure containing one or more FILE_REGION_INFO structures.
    /// A FILE_REGION_INFO data element is as follows.
    /// </summary>
    public partial struct FILE_REGION_INFO
    {
        /// <summary>
        /// A 64-bit signed integer that contains the file offset, in bytes, of the region.
        /// </summary>
        public long FileOffset;

        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes, of the region.
        /// </summary>
        public long Length;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the usage for the given region of the file. 
        /// </summary>
        public FILE_REGION_USAGE DesiredUsage;

        /// <summary>
        /// A 32-bit unsigned integer field that is reserved. This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public uint Reserved;
    }

    /// <summary>
    /// This message requests that the server set the short  name behavior for the volume associated with the file   
    /// handle on which this FSCTL was invoked.  This FSCTL  is only supported on WinPE and is useful for WinPE  image 
    ///  tools to generate short names. It allows DBCS  characters to be used in short names. This operation  is only  
    /// implemented on NTFS. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\1e080960-a20d-4c59-8648-0358d67bbb56.xml
    //  </remarks>
    public partial struct FSCTL_SET_SHORT_NAME_BEHAVIOR_Request
    {

        /// <summary>
        /// A 32-bit unsigned integer that specifies support for  DBCS characters in short names. 
        /// </summary>
        public FSCTL_SET_SHORT_NAME_BEHAVIOR_Request_Flags_Values Flags;
    }

    /// <summary>
    /// A 32-bit unsigned integer that specifies support for  DBCS characters in short names. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FSCTL_SET_SHORT_NAME_BEHAVIOR_Request_Flags_Values : uint
    {

        /// <summary>
        /// DBCS characters are not permitted in short names. 
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        /// DBCS characters are permitted in short names. 
        /// </summary>
        V2 = 0x00000001,
    }

    /// <summary>
    /// The FSCTL_SIS_COPYFILE request message requests that  the server use the Single-Instance Storage (SIS)filter   
    /// to copy the file that is associated with the handle  on which this FSCTL was invoked. The message contains  an 
    ///  SI_COPYFILE data element. For more information about  Single-Instance Storage, see [SIS].  If the SISfilter   
    /// is installed on the server, it will attempt to copy  the source file to the destination file by creating  an  
    /// SIS link instead of actually copying the file data.  If necessary and allowed, the source file is placed   
    /// under SIS control before the destination file is created.  The SI_COPYFILE data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\2ceb5108-f6e4-484e-be43-863a16a5b69a.xml
    //  </remarks>
    public partial struct FSCTL_SIS_COPYFILE_Request
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the size, in  bytes, of the SourceFileName element, including a  
        /// terminating-Unicode  NULL character. 
        /// </summary>
        public uint SourceFileNameLength;

        /// <summary>
        /// A 32-bit unsigned integer that contains the size in  bytes of the DestinationFileName element, including   
        /// a terminating-Unicode NULL character. 
        /// </summary>
        public uint DestinationFileNameLength;

        /// <summary>
        /// A 32-bit unsigned integer that contains zero or more  of the following flag values. Flag values not  
        /// specified  in the following table SHOULD be set to 0, and MUST  be ignored. 
        /// </summary>
        public FSCTL_SIS_COPYFILE_Request_Flags_Values Flags;

        /// <summary>
        /// A NULL-terminated Unicode string containing the source  file name. 
        /// </summary>
        [Size("SourceFileNameLength")]
        public byte[] SourceFileName;

        /// <summary>
        /// A NULL-terminated Unicode string containing the destination  file name. Both the source and destination  
        /// file names  must represent paths on the same volume, and the file  names are the full paths to the files,  
        /// including the  share or drive letter at which each file is located. 
        /// </summary>
        [Size("DestinationFileNameLength")]
        public byte[] DestinationFileName;
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains zero or more  of the following flag values. Flag values not specified  
    ///  in the following table SHOULD be set to 0, and MUST  be ignored. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FSCTL_SIS_COPYFILE_Request_Flags_Values : uint
    {

        /// <summary>
        /// If this flag is set, only create the destination file  if the source file is already under SIS control. If 
        ///   the source file is not under SIS control, the FSCTL  returns STATUS_OBJECT_TYPE_MISMATCH. If this flag 
        /// is   not specified, place the source file under SIS control  (if it is not already under SIS control), and 
        ///  create  the destination file. 
        /// </summary>
        COPYFILE_SIS_LINK = 0x00000001,

        /// <summary>
        /// If this flag is set, create the destination file if  it does not exist; if it does exist, overwrite it.    
        /// If this flag is not specified, create the destination  file if it does not exist; if it does exist, the  
        /// FSCTL  returns STATUS_OBJECT_NAME_COLLISION. 
        /// </summary>
        COPYFILE_SIS_REPLACE = 0x00000002,
    }

    /// <summary>
    /// The NTFS_STATISTICS data element is returned with a  FSCTL_FILESYSTEM_GET_STATISTICS reply message when  NTFS  
    /// file system statistics are requested. The NTFS_STATISTICS  data element is as follows: 
    /// </summary>
    //  <remarks>
    //   MS-fscc\2db81d0c-4bb8-40b7-b226-ac2a937a9845.xml
    //  </remarks>
    public partial struct NTFS_STATISTICS
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of exceptions generated due to the log file being  
        /// full. 
        /// </summary>
        public uint LogFileFullExceptions;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of other exceptions generated. 
        /// </summary>
        public uint OtherExceptions;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on the Master File Table (MFT). 
        /// </summary>
        public uint MftReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from the MFT. 
        /// </summary>
        public uint MftReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on the MFT. 
        /// </summary>
        public uint MftWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to the MFT. 
        /// </summary>
        public uint MftWriteBytes;

        /// <summary>
        /// An MftWritesUserLevel structure containing statistics  about writes resulting from certain user-level  
        /// operations. 
        /// </summary>
        public MftWritesUserLevel MftWritesUserLevel;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of flushes  of the MFT performed because the log file was  
        /// full. 
        /// </summary>
        public ushort MftWritesFlushForLogFileFull;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  write operations performed by the lazy writer  
        /// thread. 
        /// </summary>
        public ushort MftWritesLazyWriter;

        /// <summary>
        /// A 16-bit unsigned integer which is reserved. This contains  the sum of the four fields in the  
        /// MftWritesUserLevel  structure. 
        /// </summary>
        public ushort MftWritesUserRequest;

        /// <summary>
        /// A 2-byte field that is unused and MUST be ignored. 
        /// </summary>
        public ushort Padding;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on the master file table mirror 
        ///   (MFT2). 
        /// </summary>
        public uint Mft2Writes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to the MFT2. 
        /// </summary>
        public uint Mft2WriteBytes;

        /// <summary>
        /// An MftWritesUserLevel structure containing statistics  about writes resulting from certain user-level  
        /// operations. 
        /// </summary>
        public MftWritesUserLevel Mft2WritesUserLevel;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of flushes  of the MFT2 performed because the log file was 
        ///  full. 
        /// </summary>
        public ushort Mft2WritesFlushForLogFileFull;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT2  write operations performed by the lazy writer  
        /// thread. 
        /// </summary>
        public ushort Mft2WritesLazyWriter;

        /// <summary>
        /// A 16-bit unsigned integer that contains the sum of the  four fields in the Mft2WritesUserLevel structure. 
        /// </summary>
        public ushort Mft2WritesUserRequest;

        /// <summary>
        /// A 2-byte field that is unused and MUST be ignored. 
        /// </summary>
        public ushort Padding1;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on the root index. 
        /// </summary>
        public uint RootIndexReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from the root index. 
        /// </summary>
        public uint RootIndexReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on the root index. 
        /// </summary>
        public uint RootIndexWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to the root index. 
        /// </summary>
        public uint RootIndexWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on the cluster allocation  
        /// bitmap. 
        /// </summary>
        public uint BitmapReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from the cluster allocation bitmap. 
        /// </summary>
        public uint BitmapReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on the cluster allocation  
        /// bitmap.  This is the sum of the BitmapWritesFlushForLogFileFull,  BitmapWritesLazyWriter and  
        /// BitmapWritesUserRequest  fields. 
        /// </summary>
        public uint BitmapWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to the cluster allocation bitmap. 
        /// </summary>
        public uint BitmapWriteBytes;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of flushes  of the bitmap performed because the log file  
        /// was full. 
        /// </summary>
        public ushort BitmapWritesFlushForLogFileFull;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of bitmap  write operations performed by the lazy writer  
        /// thread. 
        /// </summary>
        public ushort BitmapWritesLazyWriter;

        /// <summary>
        /// A 16-bit unsigned integer which is reserved. This is  the sum of the fields in the BitmapWritesUserLevel   
        /// structure. 
        /// </summary>
        public ushort BitmapWritesUserRequest;

        /// <summary>
        /// A BitmapWritesUserLevel structure containing statistics  about bitmap writes resulting from certain  
        /// user-level  operations. 
        /// </summary>
        public BitmapWritesUserLevel BitmapWritesUserLevel;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on the MFT bitmap. 
        /// </summary>
        public uint MftBitmapReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from the MFT bitmap. 
        /// </summary>
        public uint MftBitmapReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on the MFT bitmap. This value  
        /// is  the sum of the MftBitmapWritesFlushForLogFileFull,  MftBitmapWritesLazyWriter and  
        /// MftBitmapWritesUserRequest  fields. 
        /// </summary>
        public uint MftBitmapWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to the MFT bitmap. 
        /// </summary>
        public uint MftBitmapWriteBytes;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of flushes  of the MFT bitmap performed because the log  
        /// file was  full. 
        /// </summary>
        public ushort MftBitmapWritesFlushForLogFileFull;

        /// <summary>
        /// A 16-bit unsigned integer value containing the number  of MFT bitmap write operations performed by the  
        /// lazy  writer thread. 
        /// </summary>
        public ushort MftBitmapWritesLazyWriter;

        /// <summary>
        /// A 16-bit unsigned integer which is reserved. This value  is the sum of all the fields in the  
        /// MftBitmapWritesUserLevel  structure. 
        /// </summary>
        public ushort MftBitmapWritesUserRequest;

        /// <summary>
        /// An MftBitmapWritesUserLevel structure containing statistics  about MFT bitmap writes resulting from  
        /// certain user-level  operations. 
        /// </summary>
        public MftBitmapWritesUserLevel MftBitmapWritesUserLevel;

        /// <summary>
        /// A 2-byte field that is unused and MUST be ignored. 
        /// </summary>
        public ushort Padding2;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on the user index. 
        /// </summary>
        public uint UserIndexReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from user indices. 
        /// </summary>
        public uint UserIndexReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on user indices. 
        /// </summary>
        public uint UserIndexWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to user indices. 
        /// </summary>
        public uint UserIndexWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on the log file. 
        /// </summary>
        public uint LogFileReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from the log file. 
        /// </summary>
        public uint LogFileReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on the log file. 
        /// </summary>
        public uint LogFileWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to the log file. 
        /// </summary>
        public uint LogFileWriteBytes;

        /// <summary>
        /// An Allocate structure describes cluster allocation patterns  in NTFS. 
        /// </summary>
        public Allocate Allocate;
    }

    /// <summary>
    /// The first possible structure for the FILE_OBJECTID_BUFFER  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\34a727a2-960a-4825-9cd2-6100c84e3a81.xml
    //  </remarks>
    public partial struct FILE_OBJECTID_BUFFER_Type_1
    {

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the file or  directory within the volume on which it resides.  
        /// Specifically,  the same object ID can be assigned to another file  or directory on a different volume, but 
        ///  it MUST NOT  be assigned to another file or directory on the same  volume. 
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the volume on  which the object resided when the object identifier 
        ///   was created, or zero if the volume had no object identifier  at that time. After copy operations, move  
        /// operations,  or other file operations, this may not be the same  as the object identifier of the volume on 
        ///  which the  object presently resides. 
        /// </summary>
        public System.Guid BirthVolumeId;

        /// <summary>
        /// A 16-byte GUID value containing the object identifier  of the object at the time it was created. Copy  
        /// operations,  move operations, or other file operations MAY change  the value of the ObjectId member.  
        /// Therefore, the BirthObjectId  might not be the same as the ObjectId member at present.  Specifically, the  
        /// same object ID MAY be assigned to  another file or directory on a different volume, but  it MUST NOT be  
        /// assigned to another file or directory  on the same volume. The object ID is assigned at file  creation  
        /// time. When a file is moved or copied from  one volume to another, the ObjectId member value changes  to a  
        /// random unique value to avoid the potential for  ObjectId collisions because the object ID is not  
        /// guaranteed  to be unique across volumes. 
        /// </summary>
        public System.Guid BirthObjectId;

        /// <summary>
        /// A 16-byte GUID value containing the domain identifier.  This value is unused; it SHOULD be zero, and MUST  
        /// be  ignored. The NTFS file system places no constraints  on the format of the 48 bytes of information  
        /// following  the ObjectId in this structure. This format of the  FILE_OBJECTID_BUFFER is used on windows by  
        /// the Microsoft  Distributed Link Tracking Service (see [MS-DLTW] section  ). 
        /// </summary>
        public System.Guid DomainId;
    }


    /// <summary>
    /// The FSCTL_IS_PATHNAME_VALID request message requests  that the server indicate whether the specified pathname  
    ///  is well-formed (of acceptable length, with no invalid  characters, and so on - see section ) with respect  to 
    ///  the volume associated with the handle on which this  FSCTL was invoked. The data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\38b85b3b-ed6d-4a9c-a83b-b29f1684ba0b.xml
    //  </remarks>
    public partial struct FSCTL_IS_PATHNAME_VALID_Request
    {

        /// <summary>
        /// An unsigned 32-bit integer that specifies the length,  in bytes, of the PathName data element. 
        /// </summary>
        public uint PathNameLength;

        /// <summary>
        /// A variable-length Unicode string that specifies the  path name. 
        /// </summary>
        [Size("PathNameLength")]
        public byte[] PathName;
    }

    /// <summary>
    /// The MftWritesUserLevel structure contains statistics  about writes resulting from certain user-level  
    /// operations.  The MftWritesUserLevel structure is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\3a7b4ffd-4031-4542-8adb-cbe171062f1e.xml
    //  </remarks>
    public partial struct MftWritesUserLevel
    {

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  writes due to a write operation. 
        /// </summary>
        public ushort Write;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  writes due to a create operation. 
        /// </summary>
        public ushort Create;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  writes due to a set file information operation. 
        /// </summary>
        public ushort SetInfo;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  writes due to a flush operation. 
        /// </summary>
        public ushort Flush;
    }

    /// <summary>
    /// This message returns the result of the FSCTL_FILESYSTEM_GET_STATISTICS  request message as a  
    /// FILESYSTEM_STATISTICS structure  which contains information about both user and metadata  files. User files  
    /// are available for the user. Metadata  files are system files that contain information that  the file system  
    /// uses for its internal organization.  The output buffer contains a FILESYSTEM_STATISTICS  structure for each  
    /// processor. This FSCTL is implemented  on NTFS, FAT, and exFAT file systems. Other file systems  return  
    /// STATUS_INVALID_DEVICE_REQUEST. The statistics  structures contain fields that may overflow during  the  
    /// server's lifetime. This is by design. When an overflow  occurs, the value just wraps. For example 0xfffff000   
    /// + 0x2000 will result in 0x1000. The FILESYSTEM_STATISTICS  data element is as follows: 
    /// </summary>
    //  <remarks>
    //   MS-fscc\3b496011-eca3-4ffb-809a-17c6bab9fca4.xml
    //  </remarks>
    public partial struct FSCTL_FILESYSTEM_GET_STATISTICS_Reply
    {

        /// <summary>
        /// A 16-bit unsigned integer value containing the type  of file system. This field MUST contain one of the   
        /// following values. 
        /// </summary>
        public FileSystemType_Values FileSystemType;

        /// <summary>
        /// 16-bit unsigned integer value containing the version.  This field MUST contain 0x00000001. 
        /// </summary>
        public Version_Values Version;

        /// <summary>
        /// A 32-bit unsigned integer value that indicates the size,  in bytes, of this structure plus the size of the 
        ///  file  system-specific structure that follows this structure,  rounded up to a multiple of 64, and then  
        /// multiplied  by the number of processors. For example, if the size  of FILESYSTEM_STATISTICS is 0x38, the  
        /// size of NTFS_STATISTICS  is 0xd4, and if there are 2 processors, the buffer  allocated must be 0x280. This 
        ///  is the sum of the sizes  of the NTFS_STATISTICS structure and the FILESYSTEM_STATISTICS  structure,  
        /// rounded up to a multiple of 64, and multiplied  by the number of processors. 
        /// </summary>
        public uint SizeOfCompleteStructure;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on user files. 
        /// </summary>
        public uint UserFileReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from user files. 
        /// </summary>
        public uint UserFileReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on user files that went to the  
        /// disk  rather than the cache. This value includes sub-read  operations. 
        /// </summary>
        public uint UserDiskReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on user files. 
        /// </summary>
        public uint UserFileWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to user files. 
        /// </summary>
        public uint UserFileWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on user files that went to disk 
        ///   rather than the cache. This value includes sub-write  operations. 
        /// </summary>
        public uint UserDiskWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on metadata files. 
        /// </summary>
        public uint MetaDataReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from metadata files. 
        /// </summary>
        public uint MetaDataReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations on metadata files. This value  
        /// includes  sub-read operations. 
        /// </summary>
        public uint MetaDataDiskReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on metadata files. 
        /// </summary>
        public uint MetaDataWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to metadata files. 
        /// </summary>
        public uint MetaDataWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations on metadata files. This value  
        /// includes  sub-write operations. 
        /// </summary>
        public uint MetaDataDiskWrites;
    }

    /// <summary>
    /// A 16-bit unsigned integer value containing the type  of file system. This field MUST contain one of the   
    /// following values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileSystemType_Values : ushort
    {

        /// <summary>
        /// The file system is an NTFS file system. If this value  is set, this structure is followed by an  
        /// NTFS_STATISTICS  structure. 
        /// </summary>
        FILESYSTEM_STATISTICS_TYPE_NTFS = 0x0001,

        /// <summary>
        /// The file system is a FAT file system. If this value  is set, this structure is followed by a  
        /// FAT_STATISTICS  structure. 
        /// </summary>
        FILESYSTEM_STATISTICS_TYPE_FAT = 0x0002,

        /// <summary>
        /// The file system is an exFAT file system. If this value  is set, this structure is followed by an  
        /// EXFAT_STATISTICS  structure. 
        /// </summary>
        FILESYSTEM_STATISTICS_TYPE_EXFAT = 0x0003,
    }

    /// <summary>
    /// 16-bit unsigned integer value containing the version.  This field MUST contain 0x00000001. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Version_Values : ushort
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        V1 = 0x00000001,
    }

    /// <summary>
    /// Sets the software defect management state for the specified  file associated with the handle on which this  
    /// FSCTL  was invoked. Used for UDF file systems. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\3ce96098-fb24-472e-af53-1fa211bda38b.xml
    //  </remarks>
    public partial struct FSCTL_SET_DEFECT_MANAGEMENT_Request
    {

        /// <summary>
        /// If TRUE, indicates that defect management will be disabled. 
        /// </summary>
        public byte Disable;
    }

    /// <summary>
    /// This information class is used to query NTFS hard links  to an existing file. At least one name MUST be  
    /// returned.  The FILE_LINKS_INFORMATION data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\46021e52-29b1-475c-b6d3-fe5497d23277.xml
    //  </remarks>
    public partial struct FileHardLinkInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that MUST contain the number  of bytes needed to hold all available names. This  
        /// field  MUST NOT be 0. 
        /// </summary>
        public uint BytesNeeded;

        /// <summary>
        /// A 32-bit unsigned integer that MUST contain the number  of FILE_LINK_ENTRY_INFORMATION structures that  
        /// have  been returned in the Entries field. This field MUST  return as many entries as will fit in available 
        ///  memory.  A value of 0 indicates that there is not enough available  memory to return any entry. The error 
        ///  STATUS_BUFFER_OVERFLOW  (0x80000005) indicates that not all available entries  were returned. 
        /// </summary>
        public uint EntriesReturned;

        /// <summary>
        /// A buffer that MUST contain the returned FILE_LINK_ENTRY_INFORMATION  structures. It MUST be BytesNeeded  
        /// bytes in size to  return all of the available entries. 
        /// </summary>
        public FILE_LINK_ENTRY_INFORMATION Entries;
    }

    /// <summary>
    /// The Mft2WritesUserLevel structure contains statistics  about writes resulting from certain user-level  
    /// operations.  The Mft2WritesUserLevel structure is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\4912f122-a246-46a0-9993-de0ba251b7b4.xml
    //  </remarks>
    public partial struct Mft2WritesUserLevel
    {

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT2  writes due to a write operation. 
        /// </summary>
        public ushort Write;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT2  writes due to a create operation. 
        /// </summary>
        public ushort Create;

        /// <summary>
        /// A16-bit unsigned integer containing the number of MFT2  writes due to a set file information operation. 
        /// </summary>
        public ushort SetInfo;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT2  writes due to a flush operation. 
        /// </summary>
        public ushort Flush;
    }


    /// <summary>
    /// A ULONG that MUST specify on file creation or file open  how the file will subsequently be accessed. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Mode_Values : uint
    {

        /// <summary>
        /// When set, any system services, file system drivers (FSDs),  and drivers that write data to the file must  
        /// actually  transfer the data into the file before any requested  write operation is considered complete. 
        /// </summary>
        FILE_WRITE_THROUGH = 0x00000002,

        /// <summary>
        /// When set, all accesses to the file will be sequential. 
        /// </summary>
        FILE_SEQUENTIAL_ONLY = 0x00000004,

        /// <summary>
        /// When set, the file cannot be cached or buffered in a  driver's internal buffers. 
        /// </summary>
        FILE_NO_INTERMEDIATE_BUFFERING = 0x00000008,

        /// <summary>
        /// When set, all operations on the file are performed synchronously.  Any wait on behalf of the caller is  
        /// subject to premature  termination from alerts. This flag also causes the  I/O system to maintain the file  
        /// position context. 
        /// </summary>
        FILE_SYNCHRONOUS_IO_ALERT = 0x00000010,

        /// <summary>
        /// When set, all operations on the file are performed synchronously.  Wait requests in the system to  
        /// synchronize I/O queuing  and completion are not subject to alerts. This flag  also causes the I/O system  
        /// to maintain the file position  context. 
        /// </summary>
        FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020,

        /// <summary>
        /// When set, delete the file when the last handle to the  file is closed. 
        /// </summary>
        FILE_DELETE_ON_CLOSE = 0x00001000,
    }

    /// <summary>
    /// This identifies the type of given volume. It MUST be  one of the following. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DeviceType_Values : uint
    {
        /// <summary>
        /// Volume resides on a CD ROM. 
        /// </summary>
        FILE_DEVICE_CD_ROM = 0x00000002,

        /// <summary>
        /// Volume resides on a disk. 
        /// </summary>
        FILE_DEVICE_DISK = 0x00000007,
    }

    /// <summary>
    /// A bit field which identifies various characteristics  about a given volume. The following are valid bit  
    /// values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Characteristics_Values : uint
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_REMOVABLE_MEDIA = 0x00000001,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_READ_ONLY_DEVICE = 0x00000002,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_FLOPPY_DISKETTE = 0x00000004,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_WRITE_ONCE_MEDIA = 0x00000008,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_REMOTE_DEVICE = 0x00000010,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_DEVICE_IS_MOUNTED = 0x00000020,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_VIRTUAL_VOLUME = 0x00000040,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_AUTOGENERATED_DEVICE_NAME = 0x00000080,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_DEVICE_SECURE_OPEN = 0x00000100,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_CHARACTERISTIC_PNP_DEVICE = 0x00000800,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_CHARACTERISTIC_TS_DEVICE = 0x00001000,

        /// <summary>
        /// Possible value. 
        /// </summary>
        FILE_CHARACTERISTIC_WEBDAV_DEVICE = 0x00002000,
    }

    /// <summary>
    /// The FSCTL_GET_COMPRESSION reply message returns the  results of the FSCTL_GET_COMPRESSION request as a 16-bit  
    ///  unsigned integer value that indicates the current compression  state of the file or directory. The  
    /// CompressionState  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\62de177d-ed90-4884-ae04-9de52f1180f1.xml
    //  </remarks>
    public partial struct FSCTL_GET_COMPRESSION_Reply
    {

        /// <summary>
        /// One of the following standard values MUST be returned. 
        /// </summary>
        public CompressionState_Values CompressionState;
    }

    /// <summary>
    /// One of the following standard values MUST be returned. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CompressionState_Values : ushort
    {

        /// <summary>
        /// The file or directory is not compressed. 
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        /// The file or directory is compressed by using the default  compression algorithm. Equivalent to  
        /// COMPRESSION_FORMAT_LZNT1. 
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        /// The file or directory is compressed by using the LZNT1  compression algorithm. For more information, see  
        /// [UASDC]. 
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,
    }

    /// <summary>
    /// The FSCTL_LMR_GET_LINK_TRACKING_INFORMATION reply message  returns the results of the  
    /// FSCTL_LMR_GET_LINK_TRACKING_INFORMATION  request. The LINK_TRACKING_INFORMATION data element  is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\635eeb16-a108-40d8-a675-d39939fc7780.xml
    //  </remarks>
    public partial struct FSCTL_LMR_GET_LINK_TRACKING_INFORMATION_Reply
    {

        /// <summary>
        /// An unsigned 32-bit integer that indicates the type of  file system on which the file is hosted on the  
        /// destination  computer. This value MUST be one of the following. 
        /// </summary>
        public Type_Values Type;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the volume for  the object. 
        /// </summary>
        public System.Guid VolumeId;
    }

    /// <summary>
    /// An unsigned 32-bit integer that indicates the type of  file system on which the file is hosted on the  
    /// destination  computer. This value MUST be one of the following. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Type_Values : uint
    {

        /// <summary>
        /// The destination file system is NTFS. 
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        /// The destination file system is DFS. For more information,  see [MSDFS]. 
        /// </summary>
        V2 = 0x00000001,
    }

    /// <summary>
    /// This information class is used to create an NTFS hard  link to an existing file.  The FILE_LINK_INFORMATION   
    /// data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\69643dd3-b518-465d-bb0e-e2e9c5b7875e.xml
    //  </remarks>
    public partial struct FileLinkInformation
    {

        /// <summary>
        /// An 8-bit field that is set to 1 to indicate that if  the link already exists, it should be replaced with   
        /// the new link. MUST be set to 0 if the caller wants  the link creation operation to fail if the link  
        /// already  exists. 
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        /// Reserved for alignment. This field can contain any value  and MUST be ignored. 
        /// </summary>
        [StaticSize(7, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        /// A 64-bit unsigned integer that contains the file handle  for the directory where the link is to be  
        /// created.  For network operations, this value MUST be zero. 
        /// </summary>
        public FileLinkInformation_RootDirectory_Values RootDirectory;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the name  to be assigned to the newly created link. This field 
        ///   might not be NULL-terminated, and MUST be handled as  a sequence of FileNameLength bytes. If the  
        /// RootDirectory  member is NULL, and the link is to be created in a  different directory from the file that  
        /// is being linked  to, this member specifies the full path name for the  link to be created. Otherwise, it  
        /// specifies only the  file name. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// A 64-bit unsigned integer that contains the file handle  for the directory where the link is to be created.   
    /// For network operations, this value MUST be zero. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileLinkInformation_RootDirectory_Values : ulong
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// This message returns the results of the FSCTL_QUERY_ON_DISK_VOLUME_INFO  request (section ). 
    /// </summary>
    //  <remarks>
    //   MS-fscc\69930ad3-1ad3-41d0-810e-12b19ecbf5e7.xml
    //  </remarks>
    public partial struct FSCTL_QUERY_ON_DISK_VOLUME_INFO_Reply
    {

        /// <summary>
        /// A 64-bit signed integer. The number of directories on  the specified disk. This member is -1 if the number 
        ///   is unknown. For UDF file systems with a virtual allocation  table, this information is available only if 
        ///  the UDF  revision is greater than 1.50. 
        /// </summary>
        public long DirectoryCount;

        /// <summary>
        /// A 64-bit signed integer. The number of files on the  specified disk. Returns -1 if the number is unknown.  
        ///  For UDF file systems with a virtual allocation table,  this information is available only if the UDF  
        /// revision  is greater than 1.50. 
        /// </summary>
        public long FileCount;

        /// <summary>
        /// The major version number of the file system. Returns  -1 if the number is unknown or not applicable. For   
        /// example on UDF 1.02 file systems, 1 is returned. 
        /// </summary>
        public short FsFormatMajVersion;

        /// <summary>
        /// The minor version number of the file system. Returns  -1 if the number is unknown or not applicable. For   
        /// example: on UDF 1.02 file systems, 02 is returned. 
        /// </summary>
        public short FsFormatMinVersion;

        /// <summary>
        /// Always returns "UDF" in Unicode characters. 
        /// </summary>
        [StaticSize(24, StaticSizeMode.Elements)]
        public byte[] FsFormatName;

        /// <summary>
        /// The time the media was formatted. Expressed as a FILETIME  structure, specified in section . 
        /// </summary>
        public FILETIME FormatTime;

        /// <summary>
        /// The time the media was last updated. Expressed as a  FILETIME structure, specified in section . 
        /// </summary>
        public FILETIME LastUpdateTime;

        /// <summary>
        /// Any copyright notifications associated with the volume. 
        /// </summary>
        [StaticSize(68, StaticSizeMode.Elements)]
        public byte[] CopyrightInfo;

        /// <summary>
        /// Any abstract information written on the media. 
        /// </summary>
        [StaticSize(68, StaticSizeMode.Elements)]
        public byte[] AbstractInfo;

        /// <summary>
        /// Implementation-specific information; in some cases,  it is the operating system version that the media was 
        ///   formatted by. This value is set to *Microsoft Windows  when the media is formatted on windows. 
        /// </summary>
        [StaticSize(68, StaticSizeMode.Elements)]
        public byte[] FormattingImplementationInfo;

        /// <summary>
        /// The last implementation that modified the disk. This  information is implementation-specific; in some  
        /// cases,  it is the operating system version that the media was  last modified by. This value is set to  
        /// *Microsoft Windows  when the media is written to on a windows system. 
        /// </summary>
        [StaticSize(68, StaticSizeMode.Elements)]
        public byte[] LastModifyingImplementationInfo;
    }

    #region STORAGE_OFFLOAD_TOKEN
    public enum STORAGE_OFFLOAD_TOKEN_TYPE : uint
    {
        /// <summary>
        /// A well-known Token that indicates that the data logically represented by the Token is logically equivalent to zero.
        /// </summary>
        STORAGE_OFFLOAD_TOKEN_TYPE_ZERO_DATA = 0xFFFF0001,
    }

    /// <summary>
    /// The STORAGE_OFFLOAD_TOKEN structure contains the Token to be used as a representation of the data 
    /// contained within the portion of the file specified in the FSCTL_OFFLOAD_READ_INPUT data element 
    /// at the time of the FSCTL_OFFLOAD_READ operation. 
    /// This Token is used in FSCTL_OFFLOAD_READ and FSCTL_OFFLOAD_WRITE operations. 
    /// The format of the data within this field is either vendor-specific or of a well-known type. 
    /// The contents of this field MUST NOT be modified during subsequent operations.
    /// </summary>
    public partial struct STORAGE_OFFLOAD_TOKEN
    {
        /// <summary>
        /// A 32-bit unsigned integer that defines the type of Token that is contained within the STORAGE_OFFLOAD_TOKEN structure. 
        /// </summary>
        public STORAGE_OFFLOAD_TOKEN_TYPE TokenType;

        /// <summary>
        /// A 16-bit unsigned integer that is reserved. This field SHOULD be set to 0x0000 and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A 16-bit unsigned integer that defines the length of the TokenId field in bytes.
        /// </summary>
        public ushort TokenIdLength;

        /// <summary>
        /// A 504-byte unsigned integer that contains opaque vendor-specific data.
        /// </summary>
        [StaticSize(504)]
        public byte[] TokenId;
    }
    #endregion

    #region FSCTL_OFFLOAD_READ_INPUT

    [Flags]
    public enum FSCTL_OFFLOAD_READ_INPUT_FLAGS : uint
    {
        /// <summary>
        /// Currently, no flags are defined. This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        NONE = 0x00000000,
    }

    /// <summary>
    /// The FSCTL_OFFLOAD_READ Request message requests that the server perform an Offload Read operation 
    /// to a specified portion of a file on a target volume. On the client side, this request is received, 
    /// processed, and sent down to an intelligent storage subsystem that generates and returns a Token 
    /// in an FSCTL_OFFLOAD_READ Reply (section 2.3.72) message. This Token logically represents the data 
    /// to be read and can be used with an FSCTL_OFFLOAD_WRITE Request (section 2.3.74) 
    /// and an FSCTL_OFFLOAD_WRITE Reply (section 2.3.75) pair to complete the data movement.
    /// The request message contains an FSCTL_OFFLOAD_READ_INPUT data element, as follows.
    /// </summary>
    public partial struct FSCTL_OFFLOAD_READ_INPUT
    {
        /// <summary>
        /// A 32-bit unsigned integer that indicates the size, in bytes, of this data element.
        /// </summary>
        public uint Size;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the flags to be set for this operation. 
        /// Currently, no flags are defined. This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public FSCTL_OFFLOAD_READ_INPUT_FLAGS Flags;

        /// <summary>
        /// A 32-bit unsigned integer that contains the requested Time to Live (TTL) value in milliseconds for the generated Token.
        /// This value MUST be greater than or equal to 0x00000000. A value of 0x00000000 represents a default TTL interval.
        /// </summary>
        public uint TokenTimeToLive;

        /// <summary>
        /// A 32-bit unsigned integer field that is reserved. 
        /// This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A 64-bit unsigned integer that contains the file offset, in bytes, of the start of a range of bytes in a file from which to generate the Token. 
        /// The value of this field MUST be greater than or equal to 0x0000000000000000 and MUST be aligned to a logical sector boundary on the volume.
        /// </summary>
        public ulong FileOffset;

        /// <summary>
        /// A 64-bit unsigned integer that contains the size, in bytes, of the requested range of the file from which to generate the Token. 
        /// The value of this field MUST be greater than or equal to 0x0000000000000000 and MUST be aligned to a logical sector boundary on the volume.
        /// </summary>
        public ulong CopyLength;
    }

    #endregion

    #region FSCTL_OFFLOAD_READ_OUTPUT

    /// <summary>
    /// The Flags for FSCTL_OFFLOAD_READ_OUTPUT
    /// </summary>
    [Flags]
    public enum FSCTL_OFFLOAD_READ_OUTPUT_FLAGS : uint
    {
        NONE = 0,

        /// <summary>
        /// The data beyond the current range is logically equivalent to zero.
        /// </summary>
        OFFLOAD_READ_FLAG_ALL_ZERO_BEYOND_CURRENT_RANGE = 0x00000001,
    }

    /// <summary>
    /// The FSCTL_OFFLOAD_READ Reply message returns the results of the FSCTL_OFFLOAD_READ Request (section 2.3.71).
    /// The FSCTL_OFFLOAD_READ_OUTPUT data element is as follows.
    /// </summary>
    public partial struct FSCTL_OFFLOAD_READ_OUTPUT
    {
        /// <summary>
        /// A 32-bit unsigned integer that indicates the size, in bytes, of the returned data element.
        /// </summary>
        public uint Size;

        /// <summary>
        /// A 32-bit unsigned integer that indicates which flags were returned for this operation. Possible values for the flags follow. 
        /// All unused bits are reserved for future use, SHOULD be set to 0, and MUST be ignored.
        /// </summary>
        public FSCTL_OFFLOAD_READ_OUTPUT_FLAGS Flags;

        /// <summary>
        /// A 64-bit unsigned integer that contains the amount, in bytes, of data that the Token logically represents. 
        /// This value indicates a contiguous region of the file from the beginning of the requested offset 
        /// in the FileOffset field in the FSCTL_OFFLOAD_READ_INPUT data element (section 2.3.71).
        /// This value can be smaller than the CopyLength field specified in the FSCTL_OFFLOAD_READ_INPUT data element, 
        /// which indicates that less data was logically represented (logically read) with the Token than was requested. 
        /// The value of this field MUST be greater than 0x0000000000000000 and MUST be aligned to a logical sector boundary on the volume.
        /// </summary>
        public ulong TransferLength;

        /// <summary>
        /// A STORAGE_OFFLOAD_TOKEN (section 2.3.73) structure that contains the generated Token to be used as a representation of the data contained 
        /// within the portion of the file specified in the FSCTL_OFFLOAD_READ_INPUT data element at the time of the FSCTL_OFFLOAD_READ operation. 
        /// The contents of this field MUST NOT be modified during subsequent operations.
        /// </summary>
        public STORAGE_OFFLOAD_TOKEN Token;
    }
    #endregion

    #region FSCTL_OFFLOAD_WRITE_INPUT
    [Flags]
    public enum FSCTL_OFFLOAD_WRITE_INPUT_FLAGS : uint
    {
        /// <summary>
        /// Currently, no flags are defined. This field SHOULD be set to 0x00000000 and MUST be ignored
        /// </summary>
        NONE = 0x00000000,
    }

    /// <summary>
    /// The FSCTL_OFFLOAD_WRITE Request message requests that the server perform an Offload Write operation 
    /// to a specified portion of a file on a target volume, providing a Token to the server that indicates 
    /// what data is to be logically written. On the server side, this request is received, processed, 
    /// and sent to an intelligent storage subsystem that processes the Token and determines whether it can 
    /// perform the data movement to the requested portion of the file. The Token is generated by 
    /// an intelligent storage subsystem through an FSCTL_OFFLOAD_READ Request (section 2.3.71) 
    /// or is constructed as a well-known Token type (section 2.3.73).
    ///The request message contains an FSCTL_OFFLOAD_WRITE_INPUT data element, as follows.
    /// </summary>
    public partial struct FSCTL_OFFLOAD_WRITE_INPUT
    {
        /// <summary>
        /// A 32-bit unsigned integer that indicates the size, in bytes, of this data element.
        /// </summary>
        public uint Size;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the flags to be set for this operation. 
        /// Currently, no flags are defined. This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public FSCTL_OFFLOAD_WRITE_INPUT_FLAGS Flags;

        /// <summary>
        /// A 64-bit unsigned integer that contains the file offset, in bytes, of the start of a range of bytes in a file 
        /// at which to begin writing the data logically represented by the Token. The value of this field MUST be 
        /// greater than or equal to 0x0000000000000000 and MUST be aligned to a logical sector boundary on the volume.
        /// </summary>
        public ulong FileOffset;

        /// <summary>
        /// A 64-bit unsigned integer that contains the size, in bytes, of the requested range of the file to write the data 
        /// logically represented by the Token. The value of this field MUST be greater than or equal to 0x0000000000000000 
        /// and MUST be aligned to a logical sector boundary on the volume. 
        /// This value can be smaller than the size of the data logically represented by the Token.
        /// </summary>
        public ulong CopyLength;

        /// <summary>
        /// A 64-bit unsigned integer that contains the file offset, in bytes, relative to the front of a region of data logically 
        /// represented by the Token at which to start writing. The value of this field MUST be greater than or equal to 0x0000000000000000 
        /// and MUST be aligned to a logical sector boundary on the volume.
        /// </summary>
        public ulong TransferOffset;

        /// <summary>
        /// A STORAGE_OFFLOAD_TOKEN (section 2.3.73) structure that contains the generated (or constructed) Token 
        /// to be used as a representation of the data to be logically written. 
        /// The contents of this field MUST NOT be modified during subsequent operations.
        /// </summary>
        public STORAGE_OFFLOAD_TOKEN Token;
    }
    #endregion

    #region FSCTL_OFFLOAD_WRITE_OUTPUT
    [Flags]
    public enum FSCTL_OFFLOAD_WRITE_OUTPUT_FLAGS : uint
    {
        /// <summary>
        /// Currently, no flags are defined. This field SHOULD be set to 0x00000000 and MUST be ignored
        /// </summary>
        NONE = 0x00000000,
    }
    /// <summary>
    /// The FSCTL_OFFLOAD_WRITE Reply message returns the results of the FSCTL_OFFLOAD_WRITE Request (section 2.3.74).
    /// The FSCTL_OFFLOAD_WRITE_OUTPUT data element is as follows.
    /// </summary>
    public partial struct FSCTL_OFFLOAD_WRITE_OUTPUT
    {
        /// <summary>
        /// A 32-bit unsigned integer that indicates the size, in bytes, of the returned data element.
        /// </summary>
        public uint Size;

        /// <summary>
        /// A 32-bit unsigned integer that indicates which flags were returned for this operation. 
        /// Currently, no flags are defined. This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public FSCTL_OFFLOAD_WRITE_OUTPUT_FLAGS Flags;

        /// <summary>
        /// A 64-bit unsigned integer that contains the amount, in bytes, of data that was written. 
        /// The value of this field MUST be greater than or equal to zero and MUST be aligned to a logical sector boundary on the volume. 
        /// This value can be smaller than the CopyLength field specified in the FSCTL_OFFLOAD_WRITE_INPUT data element. 
        /// A smaller value indicates that less data was logically written with the specified Token than was requested.
        /// </summary>
        public ulong LengthWritten;
    }
    #endregion

    /// <summary>
    /// The FSCTL_PIPE_PEEK response returns data from the pipe  server's output buffer in the FSCTL output buffer.   
    /// The structure of that data is detailed below. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\6b6c8b8b-c5ac-4fa5-9182-619459fce7c7.xml
    //  </remarks>
    public partial struct FSCTL_PIPE_PEEK
    {
        /// <summary>
        /// A 32-bit unsigned integer referring to the current state  of the pipe. Allowed values are defined below: 
        /// </summary>
        public NamedPipeState_Values NamedPipeState;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the size, in  bytes, of the data available to read from the pipe. 
        /// </summary>
        public uint ReadDataAvailable;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the number  of messages available in the pipe if the pipe has  
        /// been  created as a message-type pipe. Otherwise, this field  is 0. 
        /// </summary>
        public uint NumberOfMessages;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the length  of the first message available in the pipe if the  
        /// pipe  has been created as a message-type pipe. Otherwise,  this field is 0. 
        /// </summary>
        public uint MessageLength;

        /// <summary>
        /// A byte buffer of preview data from the pipe. The length  of the buffer is indicated by the value of the  
        /// ReadDataAvailable  field. 
        /// </summary>
        [Size("ReadDataAvailable")]
        public byte[] Data;
    }


    /// <summary>
    /// The FSCTL_PIPE_TRANSCEIVE request is used to send and receive data from an open pipe.
    /// </summary>
    public struct FSCTL_PIPE_TRANSCEIVE
    {
        /// <summary>
        /// A BLOB of bytes that are written into the associated pipe
        /// </summary>
        public byte[] Data;
    }


    /// <summary>
    /// A 32-bit unsigned integer referring to the current state  of the pipe. Allowed values are defined below: 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NamedPipeState_Values : uint
    {

        /// <summary>
        /// The specified named pipe is in the disconnected state. 
        /// </summary>
        FILE_PIPE_DISCONNECTED_STATE = 0x00000001,

        /// <summary>
        /// The specified named pipe is in the listening state 
        /// </summary>
        FILE_PIPE_LISTENING_STATE = 0x00000002,

        /// <summary>
        /// The specified named pipe is in the connected state. 
        /// </summary>
        FILE_PIPE_CONNECTED_STATE = 0x00000003,

        /// <summary>
        /// The specified named pipe is in the closing state. 
        /// </summary>
        FILE_PIPE_CLOSING_STATE = 0x00000004,
    }

    /// <summary>
    /// The FSCTL_SET_ZERO_DATA request message requests that  the server fill the specified range of the file  
    /// (associated  with the handle on which this FSCTL was invoked) with  zeros. The message contains a  
    /// FILE_ZERO_DATA_INFORMATION  element.  The FILE_ZERO_DATA_INFORMATION element is  as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\6be5164f-3eb6-4149-b997-c37df1444779.xml
    //  </remarks>
    public partial struct FSCTL_SET_ZERO_DATA_Request
    {

        /// <summary>
        /// A 64-bit signed integer that contains the file offset  of the start of the range to set to zeros in bytes. 
        ///   The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        public long FileOffset;

        /// <summary>
        /// A 64-bit signed integer that contains the byte offset  of the first byte beyond the last zeroed byte. The  
        ///  value of this field MUST be greater than or equal to  0. 
        /// </summary>
        public long BeyondFinalZero;
    }


    /// <summary>
    /// This message returns the results of the FSCTL_QUERY_SPARING_INFO  request (section ). 
    /// </summary>
    //  <remarks>
    //   MS-fscc\6ca2a1f5-0b7c-45f5-891b-3ea5e9e757ae.xml
    //  </remarks>
    public partial struct FSCTL_QUERY_SPARING_INFO_Reply
    {

        /// <summary>
        /// The size of a sparing packet and the underlying error  check and correction (ECC) block size of the  
        /// volume. 
        /// </summary>
        public uint SparingUnitBytes;

        /// <summary>
        /// If TRUE, indicates that sparing behavior is software-based;  if FALSE, it is hardware-based. 
        /// </summary>
        public byte SoftwareSparing;

        /// <summary>
        /// The total number of blocks allocated for sparing. 
        /// </summary>
        public uint TotalSpareBlocks;

        /// <summary>
        /// The number of blocks available for sparing. 
        /// </summary>
        public uint FreeSpareBlocks;
    }

    /// <summary>
    /// If the TargetLinkTrackingInformationLength value is  less than 36, the TARGET_LINK_TRACKING_INFORMATION_Buffer 
    ///   data element MUST be as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\6e267f44-8412-498a-bbda-be12301857ac.xml
    //  </remarks>
    public partial struct TARGET_LINK_TRACKING_INFORMATION_Buffer_1
    {
        /// <summary>
        /// A NULL-terminated ASCII string containing the NetBIOS  name of the destination computer, if known. For  
        /// more  information, see [MS-DLTW] section . If not known,  this field is zero length and contains nothing. 
        /// </summary>
        public string NetBIOSName;
    }

    /// <summary>
    /// The EXTENTS data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\707cdf5a-9d5f-4a40-ae88-fb7331d1aa33.xml
    //  </remarks>
    public partial struct EXTENTS
    {

        /// <summary>
        /// A 64-bit unsigned integer that contains the VCN at which  the next extent begins. This value minus either  
        /// StartingVcn  (for the first Extents array element) or the NextVcn  of the previous element of the array  
        /// (for all other  Extents array elements) is the length in clusters of  the current extent. The length is an 
        ///  input to the FSCTL_MOVE_FILE  request. 
        /// </summary>
        public ulong NextVcn;

        /// <summary>
        /// A 64-bit unsigned integer that contains the logical  cluster number (LCN) at which the current extent  
        /// begins  on the volume. On the NTFS file system, a 64-bit value  of 0xFFFFFFFFFFFFFFFF indicates either a  
        /// compression  unit that is partially allocated or an unallocated  region of a sparse file. For more  
        /// information about  sparse files, see [SPARSE]. NTFS performs compression  in 16-cluster units. If a given  
        /// 16-cluster unit compresses  to fit in, for example, 9 clusters, there will be a  7-cluster extent of the  
        /// file with an LCN of -1. 
        /// </summary>
        public ulong Lcn;
    }

    /// <summary>
    /// The FSCTL_FIND_FILES_BY_SID reply message returns the  results of the FSCTL_FIND_FILES_BY_SID request as an   
    /// array of FIND_BY_SID_OUTPUT data elements, one for  each matching file that is found.  The FIND_BY_SID_OUTPUT  
    ///  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\74d1fc51-dda0-4a7d-9ef6-214897eb94c6.xml
    //  </remarks>
    public partial struct FSCTL_FIND_FILES_BY_SID_Reply
    {

        /// <summary>
        /// A 16-bit unsigned integer value containing the size  of the file name in bytes. This size MUST NOT include 
        ///   the NULL character. 
        /// </summary>
        public ushort FileNameLength;

        /// <summary>
        /// A NULL-terminated Unicode string that specifies the  fully qualified path name for the file. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// The FSCTL_SET_OBJECT_ID_EXTENDED request message requests  that the server set the extended information for  
    /// the  file or directory associated with the handle on which  this FSCTL was invoked. The message contains an  
    /// EXTENDED_INFO  data element. The EXTENDED_INFO data element is defined  as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\757bc8fa-f24f-44cf-86dc-adb2c113589d.xml
    //  </remarks>
    public partial struct FSCTL_SET_OBJECT_ID_EXTENDED_Request
    {

        /// <summary>
        /// A 48-byte binary large object(BLOB) containing user-defined  extended data that was passed to this FSCTL  
        /// by an application.  In this situation, the user refers to the implementer  who is calling this FSCTL,  
        /// meaning the extended info  is opaque to NTFS; there are no rules enforced by NTFS  as to what these last  
        /// 48 bytes contain. Contrast this  with the first 16 bytes of an object ID, which can  be used to open the  
        /// file, so NTFS requires that they  be unique within a volume. The Microsoft Distributed  Link Tracking  
        /// Service uses the last 48 bytes of the  ExtendedInfo BLOB to store information that helps it  locate files  
        /// that are moved to different volumes or  computers within a domain. For more information, see  [MS-DLTW]  
        /// section . 
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    /// The FSCTL_SET_COMPRESSION request message requests that  the server set the compression state of the file or   
    /// directory (associated with the handle on which this  FSCTL was invoked) on a volume whose file system supports 
    ///   per-stream compression. The message contains a 16-bit  unsigned integer. The CompressionState element is as  
    ///  follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\77f650a3-e3a2-4a25-baac-4bf9b36bcc46.xml
    //  </remarks>
    public partial struct FSCTL_SET_COMPRESSION_Request
    {

        /// <summary>
        /// MUST be one of the following standard values. 
        /// </summary>
        public FSCTL_SET_COMPRESSION_Request_CompressionState_Values CompressionState;
    }

    /// <summary>
    /// MUST be one of the following standard values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FSCTL_SET_COMPRESSION_Request_CompressionState_Values : ushort
    {

        /// <summary>
        /// The file or directory is not compressed. 
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        /// The file or directory is compressed by using the default  compression algorithm. Equivalent to  
        /// COMPRESSION_FORMAT_LZNT1. 
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        /// The file or directory is compressed by using the LZNT1  compression algorithm. For more information, see  
        /// [UASDC]. 
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,
    }

    /// <summary>
    /// This data structure can be used to specify a list of  attributes to query or set via the  
    /// FileFullEaInformation,  information class. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\79dc1ea1-158c-4b24-b0e1-8c16c7e2af6b.xml
    //  </remarks>
    public partial struct FILE_GET_EA_INFORMATION
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  from the beginning of this entry, at which the  
        /// next  FILE_GET_EA_INFORMATION entry is located, if multiple  entries are present in a buffer. This member  
        /// MUST be  zero if no other entries follow this one. An implementation  MUST use this value to determine the 
        ///  location of the  next entry (if multiple entries are present in a buffer),  and MUST NOT assume that the  
        /// value of NextEntryOffset  is the same as the size of the current entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 8-bit unsigned integer that contains the length, in  bytes, of the EaName field.  This value does not  
        /// include  the null-terminator to EaName. 
        /// </summary>
        public byte EaNameLength;

        /// <summary>
        /// An array of 8-bit ASCII characters that contains the  extended attribute name followed by a single  
        /// null-termination  character byte. 
        /// </summary>
        [Size("EaNameLength + 1")]
        public byte[] EaName;
    }


    /// <summary>
    /// The EXFAT_STATISTICS data element is returned with a  FSCTL_FILESYSTEM_GET_STATISTICS reply message when   
    /// exFAT file system statistics are requested. The EXFAT_STATISTICS  data element is as follows: 
    /// </summary>
    //  <remarks>
    //   MS-fscc\7e0a16ae-0b04-4dbc-a707-d8c11d857599.xml
    //  </remarks>
    public partial struct EXFAT_STATISTICS
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of create operations. 
        /// </summary>
        public uint CreateHits;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of successful create operations. 
        /// </summary>
        public uint SuccessfulCreates;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of failed create operations. 
        /// </summary>
        public uint FailedCreates;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations that were not cached. 
        /// </summary>
        public uint NonCachedReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from a file that were not cached. 
        /// </summary>
        public uint NonCachedReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations that were not cached. 
        /// </summary>
        public uint NonCachedWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to a file that were not cached. 
        /// </summary>
        public uint NonCachedWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations that were not cached. This value 
        ///   includes sub-read operations. 
        /// </summary>
        public uint NonCachedDiskReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations that were not cached. This  
        /// value  includes sub-write operations. 
        /// </summary>
        public uint NonCachedDiskWrites;
    }

    /// <summary>
    /// The FSCTL_LMR_SET_LINK_TRACKING_INFORMATION request  message sets Distributed Link Tracking information  such  
    /// as file system type, volume ID, object ID, and  destination computer's NetBIOS name for the file associated   
    /// with the handle on which this FSCTL was invoked. The  message contains a REMOTE_LINK_TRACKING_INFORMATION   
    /// data element. For more information about Distributed  Link Tracking, see [MS-DLTW] section . The  
    /// REMOTE_LINK_TRACKING_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\810bff38-05fb-4386-9fae-e103259757f3.xml
    //  </remarks>
    public partial struct FSCTL_LMR_SET_LINK_TRACKING_INFORMATION_Request
    {

        /// <summary>
        /// Fid of the file from which to obtain link tracking information.  For Fid type, see [MS-SMB] section . 
        /// </summary>
        public uint TargetFileObject;

        /// <summary>
        /// Length of the TargetLinkTrackingInformationBuffer. 
        /// </summary>
        public uint TargetLinkTrackingInformationLength;

        /// <summary>
        /// This field is as specified in TARGET_LINK_TRACKING_INFORMATION_Buffer. 
        /// </summary>
        [Size("TargetLinkTrackingInformationLength")]
        public byte[] TargetLinkTrackingInformationBuffer;
    }



    /// <summary>
    /// A 32-bit unsigned integer that contains a bitmask of  flags that indicate the transactional visibility of  the 
    ///  file. The value of this field MUST be a bitwise  OR of zero or more of the following values. Any flag  values 
    ///  not explicitly mentioned here can be set to  any value and MUST be ignored. If the  
    /// FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED  flag is not set, the other flags MUST NOT be set. If  flags other 
    ///  than FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED  are set, FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED  MUST  
    /// be set. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum TxInfoFlags_Values : uint
    {

        /// <summary>
        /// The file is locked for modification by a transaction.  The transaction's ID MUST be contained in the  
        /// LockingTransactionId  field if this flag is set. 
        /// </summary>
        FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED = 0x00000001,

        /// <summary>
        /// The file is visible to transacted enumerators of the  directory whose transaction ID is in the  
        /// LockingTransactionId  field. 
        /// </summary>
        FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_TO_TX = 0x00000002,

        /// <summary>
        /// The file is visible to transacted enumerators of the  directory other than the one whose transaction ID is 
        ///   in the LockingTransactionId field, and it is visible  to non-transacted enumerators of the directory. 
        /// </summary>
        FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_OUTSIDE_TX = 0x00000004,
    }


    /// <summary>
    /// The FAT_STATISTICS data element is returned with a FSCTL_FILESYSTEM_GET_STATISTICS  reply message when FAT  
    /// file system statistics are requested.  The FAT_STATISTICS data element is as follows: 
    /// </summary>
    //  <remarks>
    //   MS-fscc\8543e12b-937f-42ff-87de-2ced65f120c9.xml
    //  </remarks>
    public partial struct FAT_STATISTICS
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of create operations. 
        /// </summary>
        public uint CreateHits;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of successful create operations. 
        /// </summary>
        public uint SuccessfulCreates;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of failed create operations. 
        /// </summary>
        public uint FailedCreates;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations that were not cached. 
        /// </summary>
        public uint NonCachedReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes read from a file that were not cached. 
        /// </summary>
        public uint NonCachedReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations that were not cached. 
        /// </summary>
        public uint NonCachedWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of bytes written to a file that were not cached. 
        /// </summary>
        public uint NonCachedWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of read operations that were not cached. This value 
        ///   includes sub-read operations. 
        /// </summary>
        public uint NonCachedDiskReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of write operations that were not cached. This  
        /// value  includes sub-write operations. 
        /// </summary>
        public uint NonCachedDiskWrites;
    }

    /// <summary>
    /// If the TargetLinkTrackingInformationLength value is  greater than or equal to 36, the  
    /// TARGET_LINK_TRACKING_INFORMATION_Buffer  data element MUST be as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\8581fb31-a52c-453f-8014-082006f0f5b6.xml
    //  </remarks>
    public partial struct TARGET_LINK_TRACKING_INFORMATION_Buffer_2
    {

        /// <summary>
        /// An unsigned 32-bit integer that indicates the type of  file system on which the file is hosted on the  
        /// destination  computer. MUST be one of the following. 
        /// </summary>
        public TARGET_LINK_TRACKING_INFORMATION_Buffer_2_Type_Values Type;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the volume for  the object, as obtained from the reply to an  
        /// FSCTL_LMR_GET_LINK_TRACKING_INFORMATION  request, called using the file handle of the destination  file. 
        /// </summary>
        public System.Guid VolumeId;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the destination  file or directory within the volume on which it  
        /// resides,  as indicated by VolumeId. 
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        /// A NULL-terminated ASCII string containing the NetBIOS  name of the destination computer, if known. For  
        /// more  information, see [MS-DLTW] section . If not known,  this field is zero length and contains nothing. 
        /// </summary>
        public string NetBIOSName;
    }

    /// <summary>
    /// An unsigned 32-bit integer that indicates the type of  file system on which the file is hosted on the  
    /// destination  computer. MUST be one of the following. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum TARGET_LINK_TRACKING_INFORMATION_Buffer_2_Type_Values : uint
    {

        /// <summary>
        /// The destination file system is NTFS. 
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        /// The destination file system is DFS. For more information,  see [MSDFS]. 
        /// </summary>
        V2 = 0x00000001,
    }

    #region FSCTL_READ_FILE_USN_DATA
    /// <summary>
    /// The FSCTL_READ_FILE_USN_DATA reply message returns the  results of the FSCTL_READ_FILE_USN_DATA request as  a  
    /// USN_RECORD.  The USN_RECORD element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\8a86ae68-6d15-487c-b2b7-da83a5ad5329.xml
    //  </remarks>
    public partial struct FSCTL_READ_FILE_USN_DATA_Reply
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the total length  of the update sequence number (USN) record in  
        /// bytes. 
        /// </summary>
        public uint RecordLength;

        /// <summary>
        /// A 16-bit unsigned integer that contains the major version  of the change journal software for this record. 
        ///  For  example, if the change journal software is version  2.0, the major version number is 2. The major  
        /// version  number is 2 for file systems created on windows_2000,  windows_xp, windows_server_2003,  
        /// windows_vista, and  windows_server_2008. 
        /// </summary>
        public ushort MajorVersion;

        /// <summary>
        /// A 16-bit unsigned integer that contains the minor version  of the change journal software for this record. 
        ///  For  example, if the change journal software is version  2.0, the minor version number is 0 (zero). The  
        /// minor  version number is 0 for file systems created on windows_2000,  windows_xp, windows_server_2003,  
        /// windows_vista, and  windows_server_2008. 
        /// </summary>
        public ushort MinorVersion;

        /// <summary>
        /// A 64-bit unsigned integer, opaque to the client, containing  the number (assigned by the file system when  
        /// the file  is created) of the file or directory for which this  record notes changes. The  
        /// FileReferenceNumber is an  arbitrarily assigned value (unique within the volume  on which the file is  
        /// stored) that associates a journal  record with a file. This value SHOULD always be unique  within the  
        /// volume on which the file is stored over  the life of the volume.windows computes the file reference   
        /// number as follows: 48 bits are the index of the file's  primary record in the master file table (MFT), and 
        ///   the other 16 bits are a sequence number. Therefore,  it is possible that a different file can have the  
        /// same  FileReferenceNumber as a file on that volume had in  the past; however, this is an unlikely  
        /// scenario. 
        /// </summary>
        public ulong FileReferenceNumber;

        /// <summary>
        /// A 64-bit unsigned integer, opaque to the client, containing  the ordinal number of the directory on which  
        /// the file  or directory that is associated with this record is  located. This is an arbitrarily assigned  
        /// value (unique  within the volume on which the file is stored) that  associates a journal record with a  
        /// parent directory. 
        /// </summary>
        public ulong ParentFileReferenceNumber;

        /// <summary>
        /// A 64-bit signed integer, opaque to the client, containing  the USN of the record. This value is unique  
        /// within  the volume on which the file is stored. This value  MUST be greater than or equal to 0. This value 
        ///  MUST  be 0 if no USN change journal records have been logged  for the file or directory associated with  
        /// this record.  For more information, see [MSDN-CJ]. 
        /// </summary>
        public long Usn;

        /// <summary>
        /// A structure containing the absolute system time this  change journal event was logged, expressed as the  
        /// number  of 100-nanosecond intervals since January 1, 1601 (UTC),  in the format of a FILETIME structure. 
        /// </summary>
        public FILETIME TimeStamp;

        /// <summary>
        /// A 32-bit unsigned integer that contains flags that indicate  reasons for changes that have accumulated in  
        /// this file  or directory journal record since the file or directory  was opened. When a file or directory  
        /// is closed, a final  USN record is generated with the USN_REASON_CLOSE flag  set in this field. The next  
        /// change, occurring after  the next open operation or deletion, starts a new record  with a new set of  
        /// reason flags. A rename or move operation  generates two USN records: one that records the old  parent  
        /// directory for the item and one that records  the new parent in the ParentFileReferenceNumber member.   
        /// Possible values for the reason code are as follows  (all unused bits are reserved for future use and MUST  
        ///  NOT be used). 
        /// </summary>
        public Reason_Values Reason;

        /// <summary>
        /// A 32-bit unsigned integer that provides additional information  about the source of the change. When a  
        /// thread writes  a new USN record, the source information flags in the  prior record continue to be present  
        /// only if the thread  also sets those flags. Therefore, the source information  structure allows  
        /// applications to filter out USN records  that are set only by a known source, for example, an  antivirus  
        /// filter. This flag MUST contain one of the  following values. 
        /// </summary>
        public SourceInfo_Values SourceInfo;

        /// <summary>
        /// A 32-bit unsigned integer that contains an index of  a unique security identifier assigned to the file or  
        ///  directory associated with this record. This index is  internal to the underlying object store and MUST be 
        ///   ignored. 
        /// </summary>
        public uint SecurityId;

        /// <summary>
        /// A 32-bit unsigned integer that contains attributes for  the file or directory associated with this record. 
        ///   Attributes of streams associated with the file or directory  are excluded. Valid file attributes are  
        /// specified in  section . 
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        /// A 16-bit unsigned integer that contains the length of  the file or directory name associated with this  
        /// record  in bytes. The FileName member contains this name. Use  this member to determine file name length  
        /// rather than  depending on a trailing NULL to delimit the file name  in FileName. 
        /// </summary>
        public ushort FileNameLength;

        /// <summary>
        /// A 16-bit unsigned integer that contains the offset in  bytes of the FileName member from the beginning of  
        ///  the structure. 
        /// </summary>
        public ushort FileNameOffset;

        /// <summary>
        /// A variable-length field of UNICODE characters containing  the name of the file or directory associated  
        /// with this  record in Unicode format. When working with this field,  do not assume that the file name will  
        /// contain a trailing  Unicode NULL character. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// FSCTL_READ_FILE_USN_DATA requests that the server return the most recent change journal USN for the file or directory 
    /// associated with the handle on which this FSCTL was invoked. This message contains an optional READ_FILE_USN_DATA data element.
    /// The READ_FILE_USN_DATA data element is as follows.
    /// </summary>
    public partial struct READ_FILE_USN_DATA
    {
        /// <summary>
        /// A 16-bit unsigned integer that contains the minimum major version of records returned in the results of this request.
        /// </summary>
        public ushort MinMajorVersion;

        /// <summary>
        /// A 16-bit unsigned integer that contains the maximum major version of records returned in the results of this request.
        /// </summary>
        public ushort MaxMajorVersion;
    }

    /// <summary>
    ///  The FSCTL_READ_FILE_USN_DATA reply message returns the results of the FSCTL_READ_FILE_USN_DATA request as 
    ///  a USN_RECORD_V2 or a USN_RECORD_V3. Both forms of reply message begin with a USN_RECORD_COMMON_HEADER, 
    ///  which can be used to determine the form of the full reply message.
    ///  The USN_RECORD_COMMON_HEADER element is as follows.
    /// </summary>
    public partial struct USN_RECORD_COMMON_HEADER
    {
        /// <summary>
        ///  A 32-bit unsigned integer that contains the total length of the update sequence number (USN) record, in bytes.
        /// </summary>
        [StaticSize(4)]
        public uint RecordLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the major version of the change journal software for this record. For
        ///  example, if the change journal software is version 2.0, the major version number is 2.
        /// </summary>
        [StaticSize(2)]
        public ushort MajorVersion;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the minor version of the change journal software for this record. For
        ///  example, if the change journal software is version 2.0, the minor version number is 0 (zero).
        /// </summary>
        [StaticSize(2)]
        public ushort MinorVersion;
    }

    /// <summary>
    ///  The FSCTL_READ_FILE_USN_DATA reply message returns the results of the FSCTL_READ_FILE_USN_DATA request as 
    ///  a USN_RECORD_V2 or a USN_RECORD_V3. 
    ///  The USN_RECORD_V2 element is as follows.
    /// </summary>
    public partial struct USN_RECORD_V2
    {
        /// <summary>
        ///  A 32-bit unsigned integer that contains the total length of the update sequence number (USN) record, in bytes.
        /// </summary>
        [StaticSize(4)]
        public uint RecordLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the major version of the change journal software for this record. For
        ///  example, if the change journal software is version 2.0, the major version number is 2.
        /// </summary>
        [StaticSize(2)]
        public ushort MajorVersion;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the minor version of the change journal software for this record. For
        ///  example, if the change journal software is version 2.0, the minor version number is 0 (zero).
        /// </summary>
        [StaticSize(2)]
        public ushort MinorVersion;

        /// <summary>
        ///  A 64-bit unsigned integer, opaque to the client, containing the number (assigned by the file system when the file
        ///  is created) of the file or directory for which this record notes changes. The FileReferenceNumber is an arbitrarily 
        ///  assigned value (unique within the volume on which the file is stored) that associates a journal record with a file. 
        ///  If the value is -1, its meaning is undefined; otherwise this value SHOULD always be unique within the volume on 
        ///  which the file is stored over the life of the volume.
        /// </summary>
        [StaticSize(8)]
        public ulong FileReferenceNumber;

        /// <summary>
        ///  A 64-bit signed integer, opaque to the client, containing the ordinal number of the directory on which the file or 
        ///  directory that is associated with this record is located. This is an arbitrarily assigned value that associates a 
        ///  journal record with a parent directory. If the value is -1, its meaning is undefined; otherwise this value SHOULD 
        ///  always be unique within the volume on which the file is stored over the life of the volume.
        /// </summary>
        [StaticSize(8)]
        public ulong ParentFileReferenceNumber;

        /// <summary>
        ///  A 64-bit signed integer, opaque to the client, containing the USN of the record. This value is unique within the 
        ///  volume on which the file is stored. This value MUST be greater than or equal to 0. This value MUST be 0 if no 
        ///  USN change journal records have been logged for the file or directory associated with this record. 
        ///  For more information, see [MSDN-CJ].
        /// </summary>
        [StaticSize(8)]
        public long Usn;

        /// <summary>
        ///  A structure containing the absolute system time in UTC expressed as the number of 100-nanosecond intervals
        ///  since January 1, 1601 (UTC), in the format of a FILETIME structure.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME TimeStamp;

        /// <summary>
        ///  A 32-bit unsigned integer that contains flags that indicate reasons for changes that have accumulated in this file 
        ///  or directory journal record since the file or directory was opened. When a file or directory is closed, a final 
        ///  USN record is generated with the USN_REASON_CLOSE flag set in this field. The next change, occurring after the 
        ///  next open operation or deletion, starts a new record with a new set of reason flags. A rename or move operation 
        ///  generates two USN records: one that records the old parent directory for the item and one that records the new
        ///  parent in the ParentFileReferenceNumber member. Possible values for the reason code are as follows (all unused 
        ///  bits are reserved for future use and MUST NOT be used).
        /// </summary>
        [StaticSize(4)]
        public Reason_Values Reason;

        /// <summary>
        ///  A 32-bit unsigned integer that provides additional information about the source of the change. When a thread 
        ///  writes a new USN record, the source information flags in the prior record continue to be present only if the 
        ///  thread also sets those flags. Therefore, the source information structure allows applications to filter out 
        ///  USN records that are set only by a known source, for example, an antivirus filter. 
        /// </summary>
        [StaticSize(4)]
        public SourceInfo_Values SourceInfo;

        /// <summary>
        ///  A 32-bit unsigned integer that contains an index of a unique security identifier assigned to the file or 
        ///  directory associated with this record. This index is internal to the underlying object store and MUST be ignored.
        /// </summary>
        [StaticSize(4)]
        public uint SecurityId;

        /// <summary>
        ///  A 32-bit unsigned integer that contains attributes for the file or directory associated with this record. 
        ///  Attributes of streams associated with the file or directory are excluded. Valid file attributes are specified 
        ///  in section 2.6.
        /// </summary>
        [StaticSize(4)]
        public uint FileAttributes;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length of the file or directory name associated with this record, 
        ///  in bytes. The FileName member contains this name. Use this member to determine file name length rather than 
        ///  depending on a trailing null to delimit the file name in FileName.
        /// </summary>
        [StaticSize(2)]
        public ushort FileNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the offset, in bytes, of the FileName member from the beginning of the structure.
        /// </summary>
        [StaticSize(2)]
        public ushort FileNameOffset;

        /// <summary>
        ///  A variable-length field of Unicode characters containing the name of the file or directory associated with this record 
        ///  in Unicode format. When working with this field, do not assume that the file name will contain a trailing Unicode null character.
        /// </summary>
        [Size("(FileNameOffset == 0) ? FileNameLength : (FileNameOffset - 60 + FileNameLength)")] //60 is the length is fields before this one
        public byte[] FileName;
    }

    /// <summary>
    ///  The FSCTL_READ_FILE_USN_DATA reply message returns the results of the FSCTL_READ_FILE_USN_DATA request as 
    ///  a USN_RECORD_V2 or a USN_RECORD_V3. 
    ///  The USN_RECORD_V3 element is as follows.
    /// </summary>
    public partial struct USN_RECORD_V3
    {
        /// <summary>
        ///  A 32-bit unsigned integer that contains the total length of the update sequence number (USN) record, in bytes.
        /// </summary>
        [StaticSize(4)]
        public uint RecordLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the major version of the change journal software for this record. For
        ///  example, if the change journal software is version 2.0, the major version number is 2.
        /// </summary>
        [StaticSize(2)]
        public ushort MajorVersion;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the minor version of the change journal software for this record. For
        ///  example, if the change journal software is version 2.0, the minor version number is 0 (zero).
        /// </summary>
        [StaticSize(2)]
        public ushort MinorVersion;

        /// <summary>
        ///  A 128-bit signed integer, opaque to the client, containing the number (assigned by the file system when the file is created) 
        ///  of the file or directory for which this record notes changes. The FileReferenceNumber is an arbitrarily assigned value 
        ///  (unique within the volume on which the file is stored) that associates a journal record with a file. This value SHOULD always 
        ///  be unique within the volume on which the file is stored over the life of the volume. 
        /// </summary>
        [StaticSize(16)]
        public Guid FileReferenceNumber;

        /// <summary>
        ///  A 128-bit signed integer, opaque to the client, containing the ordinal number of the directory on which the file or directory 
        ///  that is associated with this record is located. This is an arbitrarily assigned value (unique within the volume on which the 
        ///  file is stored) that associates a journal record with a parent directory.
        /// </summary>
        [StaticSize(16)]
        public Guid ParentFileReferenceNumber;

        /// <summary>
        ///  A 64-bit signed integer, opaque to the client, containing the USN of the record. This value is unique within the 
        ///  volume on which the file is stored. This value MUST be greater than or equal to 0. This value MUST be 0 if no 
        ///  USN change journal records have been logged for the file or directory associated with this record. 
        ///  For more information, see [MSDN-CJ].
        /// </summary>
        [StaticSize(8)]
        public long Usn;

        /// <summary>
        ///  A structure containing the absolute system time in UTC expressed as the number of 100-nanosecond intervals
        ///  since January 1, 1601 (UTC), in the format of a FILETIME structure.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME TimeStamp;

        /// <summary>
        ///  A 32-bit unsigned integer that contains flags that indicate reasons for changes that have accumulated in this file 
        ///  or directory journal record since the file or directory was opened. When a file or directory is closed, a final 
        ///  USN record is generated with the USN_REASON_CLOSE flag set in this field. The next change, occurring after the 
        ///  next open operation or deletion, starts a new record with a new set of reason flags. A rename or move operation 
        ///  generates two USN records: one that records the old parent directory for the item and one that records the new
        ///  parent in the ParentFileReferenceNumber member. Possible values for the reason code are as follows (all unused 
        ///  bits are reserved for future use and MUST NOT be used).
        /// </summary>
        [StaticSize(4)]
        public Reason_Values Reason;

        /// <summary>
        ///  A 32-bit unsigned integer that provides additional information about the source of the change. When a thread 
        ///  writes a new USN record, the source information flags in the prior record continue to be present only if the 
        ///  thread also sets those flags. Therefore, the source information structure allows applications to filter out 
        ///  USN records that are set only by a known source, for example, an antivirus filter. 
        /// </summary>
        [StaticSize(4)]
        public SourceInfo_Values SourceInfo;

        /// <summary>
        ///  A 32-bit unsigned integer that contains an index of a unique security identifier assigned to the file or 
        ///  directory associated with this record. This index is internal to the underlying object store and MUST be ignored.
        /// </summary>
        [StaticSize(4)]
        public uint SecurityId;

        /// <summary>
        ///  A 32-bit unsigned integer that contains attributes for the file or directory associated with this record. 
        ///  Attributes of streams associated with the file or directory are excluded. Valid file attributes are specified 
        ///  in section 2.6.
        /// </summary>
        [StaticSize(4)]
        public uint FileAttributes;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length of the file or directory name associated with this record, 
        ///  in bytes. The FileName member contains this name. Use this member to determine file name length rather than 
        ///  depending on a trailing null to delimit the file name in FileName.
        /// </summary>
        [StaticSize(2)]
        public ushort FileNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the offset, in bytes, of the FileName member from the beginning of the structure.
        /// </summary>
        [StaticSize(2)]
        public ushort FileNameOffset;

        /// <summary>
        ///  A variable-length field of Unicode characters containing the name of the file or directory associated with this record 
        ///  in Unicode format. When working with this field, do not assume that the file name will contain a trailing Unicode null character.
        /// </summary>
        [Size("(FileNameOffset == 0) ? FileNameLength : (FileNameOffset - 76 + FileNameLength)")] //76 is the length is fields before this one
        public byte[] FileName;
    }
    #endregion

    /// <summary>
    /// A 32-bit unsigned integer that contains flags that indicate  reasons for changes that have accumulated in this 
    ///  file  or directory journal record since the file or directory  was opened. When a file or directory is 
    /// closed,  a final  USN record is generated with the USN_REASON_CLOSE flag  set in this field. The next change, 
    /// occurring  after  the next open operation or deletion, starts a new record  with a new set of reason flags. A 
    /// rename or  move operation  generates two USN records: one that records the old  parent directory for the item 
    /// and one  that records  the new parent in the ParentFileReferenceNumber member.  Possible values for the reason 
    /// code are  as follows  (all unused bits are reserved for future use and MUST  NOT be used). 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Reason_Values : uint
    {

        /// <summary>
        /// A user has either changed one or more files or directory  attributes (such as read-only, hidden, archive,  
        /// or  sparse) or one or more time stamps. 
        /// </summary>
        USN_REASON_BASIC_INFO_CHANGE = 0x00008000,

        /// <summary>
        /// The file or directory is closed. 
        /// </summary>
        USN_REASON_CLOSE = 0x80000000,

        /// <summary>
        /// The compression state of the file or directory is changed  from (or to) compressed. 
        /// </summary>
        USN_REASON_COMPRESSION_CHANGE = 0x00020000,

        /// <summary>
        /// The file or directory is extended (added to). 
        /// </summary>
        USN_REASON_DATA_EXTEND = 0x00000002,

        /// <summary>
        /// The data in the file or directory is overwritten. 
        /// </summary>
        USN_REASON_DATA_OVERWRITE = 0x00000001,

        /// <summary>
        /// The file or directory is truncated. 
        /// </summary>
        USN_REASON_DATA_TRUNCATION = 0x00000004,

        /// <summary>
        /// The user made a change to the extended attributes of  a file or directory. These NTFS file system  
        /// attributes  are not accessible to nonnative applications. This  USN reason does not appear under normal  
        /// system usage,  but can appear if an application or utility bypasses  the Win32 API and uses the native API 
        ///  to create or  modify extended attributes of a file or directory. 
        /// </summary>
        USN_REASON_EA_CHANGE = 0x00000400,

        /// <summary>
        /// The file or directory is encrypted or decrypted. 
        /// </summary>
        USN_REASON_ENCRYPTION_CHANGE = 0x00040000,

        /// <summary>
        /// The file or directory is created for the first time. 
        /// </summary>
        USN_REASON_FILE_CREATE = 0x00000100,

        /// <summary>
        /// The file or directory is deleted. 
        /// </summary>
        USN_REASON_FILE_DELETE = 0x00000200,

        /// <summary>
        /// An NTFS file system hard link is added to (or removed  from) the file or directory. An NTFS file system  
        /// hard  link, similar to a POSIX hard link, is one of several  directory entries that see the same file or  
        /// directory. 
        /// </summary>
        USN_REASON_HARD_LINK_CHANGE = 0x00010000,

        /// <summary>
        /// A user changes the FILE_ATTRIBUTE_NOT_CONTEXT_INDEXED  attribute. That is, the user changes the file or  
        /// directory  from one in which content can be indexed to one in  which content cannot be indexed, or vice  
        /// versa. 
        /// </summary>
        USN_REASON_INDEXABLE_CHANGE = 0x00004000,

        /// <summary>
        /// The one (or more) named data stream for a file is extended  (added to). 
        /// </summary>
        USN_REASON_NAMED_DATA_EXTEND = 0x00000020,

        /// <summary>
        /// The data in one (or more) named data stream for a file  is overwritten. 
        /// </summary>
        USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010,

        /// <summary>
        /// One (or more) named data stream for a file is truncated. 
        /// </summary>
        USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040,

        /// <summary>
        /// The object identifier of a file or directory is changed. 
        /// </summary>
        USN_REASON_OBJECT_ID_CHANGE = 0x00080000,

        /// <summary>
        /// A file or directory is renamed, and the file name in  the USN_RECORD structure is the new name. 
        /// </summary>
        USN_REASON_RENAME_NEW_NAME = 0x00002000,

        /// <summary>
        /// The file or directory is renamed, and the file name  in the USN_RECORD structure is the previous name. 
        /// </summary>
        USN_REASON_RENAME_OLD_NAME = 0x00001000,

        /// <summary>
        /// The reparse point that is contained in a file or directory  is changed, or a reparse point is added to (or 
        ///  deleted  from) a file or directory. 
        /// </summary>
        USN_REASON_REPARSE_POINT_CHANGE = 0x00100000,

        /// <summary>
        /// A change is made in the access rights to a file or directory. 
        /// </summary>
        USN_REASON_SECURITY_CHANGE = 0x00000800,

        /// <summary>
        /// A named stream is added to (or removed from) a file,  or a named stream is renamed. 
        /// </summary>
        USN_REASON_STREAM_CHANGE = 0x00200000,
    }

    /// <summary>
    /// A 32-bit unsigned integer that provides additional information  about the source of the change. When a thread  
    /// writes  a new USN record, the source information flags in the  prior record continue to be present only if the 
    ///  thread  also sets those flags. Therefore, the source information  structure allows applications to filter out 
    ///  USN records  that are set only by a known source, for example, an  antivirus filter. This flag MUST contain  
    /// one of the  following values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SourceInfo_Values : uint
    {

        /// <summary>
        /// The operation provides information about a change to  the file or directory that was made by the operating 
        ///   system. For example, a change journal record with this  SourceInfo value is generated when the Remote  
        /// Storage  system moves data from external to local storage. This  SourceInfo value indicates that the  
        /// modifications did  not change the application data in the file. 
        /// </summary>
        USN_SOURCE_DATA_MANAGEMENT = 0x00000001,

        /// <summary>
        /// The operation adds a private data stream to a file or  directory. For example, a virus detector might add  
        ///  checksum information. As the virus detector modifies  the item, the system generates USN records. This  
        /// SourceInfo  value indicates that the modifications did not change  the application data in the file. 
        /// </summary>
        USN_SOURCE_AUXILIARY_DATA = 0x00000002,

        /// <summary>
        /// The operation modified the file to match the content  of the same file that exists in another member of  
        /// the  replica set for the File Replication Service (FRS). 
        /// </summary>
        USN_SOURCE_REPLICATION_MANAGEMENT = 0x00000004,
    }

    /// <summary>
    /// The FSCTL_GET_RETRIEVAL_POINTERS request message requests  that the server return a variably sized data  
    /// element,  StartingVcn, that describes the location on disk of  the file or directory associated with the  
    /// handle on  which this FSCTL was invoked. It is most commonly used  by defragmentation utilities. This data  
    /// element describes  the mapping between virtual cluster numbers and logical  cluster numbers. The  
    /// STARTING_VCN_INPUT_BUFFER data  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\8e4688cb-0bbe-4a28-95ed-b1c763d86193.xml
    //  </remarks>
    public partial struct FSCTL_GET_RETRIEVAL_POINTERS_Request
    {

        /// <summary>
        /// A 64-bit signed integer that contains the virtual cluster  number (VCN) at which to begin retrieving  
        /// extents in  the file. This value MAY be rounded down to the first  VCN of the extent in which the given  
        /// extent is found.  This value MUST be greater than or equal to 0. 
        /// </summary>
        public long StartingVcn;
    }


    /// <summary>
    /// The Allocate structure describes cluster allocation  patterns in NTFS. The cache refers to in-memory  
    /// structures  that allow quick lookups of free cluster runs either  by LCN (logical cluster number) or by run  
    /// length. The  Allocate structure is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\9ca092ba-1a54-46c0-a384-50718bfd83c5.xml
    //  </remarks>
    public partial struct Allocate
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of individual calls to allocate clusters. 
        /// </summary>
        public uint Calls;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of clusters allocated. 
        /// </summary>
        public uint Clusters;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of times a hint was specified when trying to  
        /// determine  which clusters to allocate. 
        /// </summary>
        public uint Hints;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of runs used to satisfy all the requests. 
        /// </summary>
        public uint RunsReturned;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of times the starting LCN hint was used to  
        /// determine  which clusters to allocate. 
        /// </summary>
        public uint HintsHonored;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of clusters allocated via the starting LCN hint. 
        /// </summary>
        public uint HintsClusters;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of times the run length cache was useful. 
        /// </summary>
        public uint Cache;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of clusters allocated via the run length cache. 
        /// </summary>
        public uint CacheClusters;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of times the cache was not useful and the bitmapped 
        ///   had to be scanned for free clusters. 
        /// </summary>
        public uint CacheMiss;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number  of clusters allocated by scanning the bitmap. 
        /// </summary>
        public uint CacheMissClusters;
    }

    /// <summary>
    /// The REPARSE_GUID_DATA_BUFFER data element stores data  for a reparse point and associates a GUID with the   
    /// reparse tag. This reparse data buffer MUST be used  only with reparse tag values whose high bit is set  to  
    /// 0.Reparse pointGUIDs are assigned by the ISV. An  ISV MUST associate one GUID to each assigned reparse  point  
    /// tag, and MUST always use that GUID with that  tag. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\a4d08374-0e92-43e2-8f88-88b94112f070.xml
    //  </remarks>
    public partial struct REPARSE_GUID_DATA_BUFFER
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the reparse  point tag that uniquely identifies the owner of  
        /// the  reparse point. 
        /// </summary>
        public uint ReparseTag;

        /// <summary>
        /// A 16-bit unsigned integer value containing the size,  in bytes, of the reparse data in the DataBuffer  
        /// member. 
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        /// A 16-bit field. This field SHOULD be set to 0 by the  client, and MUST be ignored by the server. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the owner of  the reparse point. Reparse pointGUIDs are not  
        /// assigned  by Microsoft. A reparse point implementer MUST select  one GUID to be used with their assigned  
        /// reparse point  tag to uniquely identify that reparse point. For more  information, see [REPARSE]. 
        /// </summary>
        public System.Guid ReparseGuid;

        /// <summary>
        /// The content of this buffer is opaque to the file system.  On receipt, its content MUST be preserved and  
        /// properly  returned to the caller. 
        /// </summary>
        [Size("ReparseDataLength")]
        public byte[] DataBuffer;
    }

    /// <summary>
    /// The FSCTL_GET_NTFS_VOLUME_DATA reply message returns  the results of the FSCTL_GET_NTFS_VOLUME_DATA request   
    /// as an NTFS_VOLUME_DATA_BUFFER_REPLY element. The NTFS_VOLUME_DATA_BUFFER_REPLY  contains information on a  
    /// volume. For more information  about the NTFS file system, see [MSFT-NTFS]. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\a5bae3a3-9025-4f07-b70d-e2247b01faa6.xml
    //  </remarks>
    public partial struct NTFS_VOLUME_DATA_BUFFER_Reply
    {

        /// <summary>
        /// A 64-bit signed integer that contains the serial number  of the volume. This is a unique number assigned  
        /// to  the volume media by the operating system when the volume  is formatted. 
        /// </summary>
        public _LARGE_INTEGER VolumeSerialNumber;

        /// <summary>
        /// A 64-bit signed integer that contains the number of  sectors in the specified volume. 
        /// </summary>
        public _LARGE_INTEGER NumberSectors;

        /// <summary>
        /// A 64-bit signed integer that contains the total number  of clusters in the specified volume. 
        /// </summary>
        public _LARGE_INTEGER TotalClusters;

        /// <summary>
        /// A 64-bit signed integer that contains the number of  free clusters in the specified volume. 
        /// </summary>
        public _LARGE_INTEGER FreeClusters;

        /// <summary>
        /// A 64-bit signed integer that contains the number of  reserved clusters in the specified volume. Reserved   
        /// clusters are free clusters reserved for when the volume  becomes full. Reserved clusters are released when 
        ///  either  the master file table grows beyond its allocated space  (the volume has a large number of small  
        /// files) or the  volume becomes full (the volume has a small number  of large files). 
        /// </summary>
        public _LARGE_INTEGER TotalReserved;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in a sector on the specified volume. 
        /// </summary>
        public uint BytesPerSector;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in a cluster on the specified volume. This  
        /// value  is also known as the cluster factor. 
        /// </summary>
        public uint BytesPerCluster;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in a file record segment. 
        /// </summary>
        public uint BytesPerFileRecordSegment;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  clusters in a file record segment. 
        /// </summary>
        public uint ClustersPerFileRecordSegment;

        /// <summary>
        /// A 64-bit signed integer that contains the size of the  master file table in bytes. 
        /// </summary>
        public _LARGE_INTEGER MftValidDataLength;

        /// <summary>
        /// A 64-bit signed integer that contains the starting logical  cluster number of the master file table. 
        /// </summary>
        public _LARGE_INTEGER MftStartLcn;

        /// <summary>
        /// A 64-bit signed integer that contains the starting logical  cluster number of the master file table  
        /// mirror. 
        /// </summary>
        public _LARGE_INTEGER Mft2StartLcn;

        /// <summary>
        /// A 64-bit signed integer that contains the starting logical  cluster number of the master file table zone. 
        /// </summary>
        public _LARGE_INTEGER MftZoneStart;

        /// <summary>
        /// A 64-bit signed integer that contains the ending logical  cluster number of the master file table zone. 
        /// </summary>
        public _LARGE_INTEGER MftZoneEnd;
    }

    /// <summary>
    /// The FSCTL_GET_NTFS_VOLUME_DATA reply message returns  the results of the FSCTL_GET_NTFS_VOLUME_DATA request   
    /// as an NTFS_VOLUME_DATA_BUFFER_REPLY element. The NTFS_VOLUME_DATA_BUFFER_REPLY  contains information on a  
    /// volume. For more information  about the NTFS file system, see [MSFT-NTFS]. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\a5bae3a3-9025-4f07-b70d-e2247b01faa6.xml
    //  </remarks>
    public partial struct NTFS_VOLUME_DATA_BUFFER
    {

        /// <summary>
        /// A 64-bit signed integer that contains the serial number  of the volume. This is a unique number assigned  
        /// to  the volume media by the operating system when the volume  is formatted. 
        /// </summary>
        public long VolumeSerialNumber;

        /// <summary>
        /// A 64-bit signed integer that contains the number of  sectors in the specified volume. 
        /// </summary>
        public long NumberSectors;

        /// <summary>
        /// A 64-bit signed integer that contains the total number  of clusters in the specified volume. 
        /// </summary>
        public long TotalClusters;

        /// <summary>
        /// A 64-bit signed integer that contains the number of  free clusters in the specified volume. 
        /// </summary>
        public long FreeClusters;

        /// <summary>
        /// A 64-bit signed integer that contains the number of  reserved clusters in the specified volume. Reserved   
        /// clusters are free clusters reserved for when the volume  becomes full. Reserved clusters are released when 
        ///  either  the master file table grows beyond its allocated space  (the volume has a large number of small  
        /// files) or the  volume becomes full (the volume has a small number  of large files). 
        /// </summary>
        public long TotalReserved;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in a sector on the specified volume. 
        /// </summary>
        public uint BytesPerSector;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in a cluster on the specified volume. This  
        /// value  is also known as the cluster factor. 
        /// </summary>
        public uint BytesPerCluster;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in a file record segment. 
        /// </summary>
        public uint BytesPerFileRecordSegment;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  clusters in a file record segment. 
        /// </summary>
        public uint ClustersPerFileRecordSegment;

        /// <summary>
        /// A 64-bit signed integer that contains the size of the  master file table in bytes. 
        /// </summary>
        public ulong MftValidDataLength;

        /// <summary>
        /// A 64-bit signed integer that contains the starting logical  cluster number of the master file table. 
        /// </summary>
        public ulong MftStartLcn;

        /// <summary>
        /// A 64-bit signed integer that contains the starting logical  cluster number of the master file table  
        /// mirror. 
        /// </summary>
        public ulong Mft2StartLcn;

        /// <summary>
        /// A 64-bit signed integer that contains the starting logical  cluster number of the master file table zone. 
        /// </summary>
        public ulong MftZoneStart;

        /// <summary>
        /// A 64-bit signed integer that contains the ending logical  cluster number of the master file table zone. 
        /// </summary>
        public ulong MftZoneEnd;
    }

    /// <summary>
    /// The FSCTL_GET_REFS_VOLUME_DATA reply message returns the results of the FSCTL_GET_REFS_VOLUME_DATA request as an REFS_VOLUME_DATA_BUFFER element.
    /// The REFS_VOLUME_DATA_BUFFER contains information on a volume.
    /// </summary>
    public partial struct REFS_VOLUME_DATA_BUFFER
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the valid data length for this structure. 
        /// ByteCount can be less than the size of this structure. Only the fields that entirely fit within the valid data 
        /// length for this structure, as defined by ByteCount, are valid.
        /// </summary>
        public uint ByteCount;

        /// <summary>
        /// A 32-bit unsigned integer that contains the major version of the ReFS volume.
        /// </summary>
        public uint MajorVersion;

        /// <summary>
        /// A 32-bit unsigned integer that contains the minor version of the ReFS volume.
        /// </summary>
        public uint MinorVersion;

        /// <summary>
        /// A 32-bit unsigned integer that defines the number of bytes in a physical sector on the specified volume.
        /// </summary>
        public uint BytesPerPhysicalSector;

        /// <summary>
        /// A 64-bit signed integer that contains the serial number of the volume. This is a unique number assigned to 
        /// the volume media by the operating system when the volume is formatted.
        /// </summary>
        public long VolumeSerialNumber;

        /// <summary>
        /// A 64-bit signed integer that contains the number of sectors in the specified volume.
        /// </summary>
        public long NumberSectors;

        /// <summary>
        /// A 64-bit signed integer that contains the total number of clusters in the specified volume.
        /// </summary>
        public long TotalClusters;

        /// <summary>
        /// A 64-bit signed integer that contains the number of free clusters in the specified volume.
        /// </summary>
        public long FreeClusters;

        /// <summary>
        /// A 64-bit signed integer that contains the number of reserved clusters in the specified volume. Reserved clusters 
        /// are used to guarantee clusters are available at points when the file system can't properly report allocation failures.
        /// </summary>
        public long TotalReserved;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of bytes in a sector on the specified volume.
        /// </summary>
        public uint BytesPerSector;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of bytes in a cluster on the specified volume. This value is also known as the cluster factor.
        /// </summary>
        public uint BytesPerCluster;

        /// <summary>
        /// A 64-bit unsigned integer that defines the maximum number of bytes a file can contain and be co-located with 
        /// the file system metadata that describes the file (commonly known as resident files).
        /// </summary>
        public long MaximumSizeOfResidentFile;

        /// <summary>
        /// 80 bytes which, if included, as per the ByteCount field, are reserved, have an undefined value, and are not interpreted.
        /// </summary>
        [StaticSize(80)]
        public byte[] Reserved;
    }

    #region FSCTL_GET_INTEGRITY_INFORMATION_BUFFER

    /// <summary>
    /// ChecksumAlgorithm for FSCTL_GET_INTEGRITY_INFORMATION_BUFFER
    /// </summary>
    public enum FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM : ushort
    {
        /// <summary>
        /// The file or directory is not configured to use integrity.
        /// </summary>
        CHECKSUM_TYPE_NONE = 0x0000,

        /// <summary>
        /// The file or directory is configured to use a CRC64 checksum to provide integrity.
        /// </summary>
        CHECKSUM_TYPE_CRC64 = 0x0002,
    }

    /// <summary>
    /// Flags for FSCTL_GET_INTEGRITY_INFORMATION_Reply
    /// </summary>
    [Flags]
    public enum FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_FLAGS : uint
    {
        NONE = 0,

        /// <summary>
        /// Indicates that checksum enforcement is not currently enabled on the target file.
        /// </summary>
        FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF = 0x00000001,
    }

    /// <summary>
    /// The FSCTL_GET_INTEGRITY_INFORMATION Reply message returns the results of the FSCTL_GET_INTEGRITY_INFORMATION Request (section 2.3.45) 
    /// and indicates the current integrity state of the file or directory.
    ///The FSCTL_GET_INTEGRITY_INFORMATION_BUFFER data element is as follows.
    /// </summary>
    public partial struct FSCTL_GET_INTEGRITY_INFORMATION_BUFFER
    {
        /// <summary>
        /// MUST be one of the following standard values.
        /// </summary>
        public FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM ChecksumAlgorithm;

        /// <summary>
        /// A 16-bit reserved value. This field MUST be set to 0x0000 and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A 32-bit unsigned integer that contains zero or more of the following flag values. 
        /// Flag values not specified in the following table SHOULD be set to 0 and MUST be ignored.
        /// </summary>
        public FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_FLAGS Flags;

        /// <summary>
        /// A 32-bit unsigned integer specifying the size in bytes of each chunk in a stream that is configured with integrity.
        /// </summary>
        public uint ChecksumChunkSizeInBytes;

        /// <summary>
        /// A 32-bit unsigned integer specifying the size of a cluster for this volume in bytes. 
        /// The ClusterSizeInBytes field MUST be a power of 2 and MUST be a power of 2 multiple of SectorSize, 
        /// where SectorSize is a 32-bit unsigned integer specifying the size of a sector for this volume in bytes. 
        /// SectorSize MUST be a power of 2 and MUST be greater than or equal to 512 and less than or equal to 4,096.
        /// </summary>
        public uint ClusterSizeInBytes;
    }
    #endregion

    #region FSCTL_SET_INTEGRITY_INFORMATION_BUFFER

    /// <summary>
    /// ChecksumAlgorithm for FSCTL_SET_INTEGRITY_INFORMATION_BUFFER
    /// </summary>
    public enum FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM : ushort
    {
        /// <summary>
        /// The file or directory should be set to not use integrity.
        /// </summary>
        CHECKSUM_TYPE_NONE = 0x0000,

        /// <summary>
        /// The file or directory should be set to provide integrity using a CRC64 checksum.
        /// </summary>
        CHECKSUM_TYPE_CRC64 = 0x0002,

        /// <summary>
        /// The integrity status of the file or directory should be unchanged.
        /// </summary>
        CHECKSUM_TYPE_UNCHANGED = 0xFFFF,
    }

    /// <summary>
    /// Flags for FSCTL_SET_INTEGRITY_INFORMATION_BUFFER
    /// </summary>
    public enum FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS : uint
    {
        NONE = 0,

        /// <summary>
        /// When set, if a checksum does not match, the associated I/O operation will not be failed.
        /// </summary>
        FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF = 0x00000001
    }
    /// <summary>
    /// The FSCTL_SET_INTEGRITY_INFORMATION Request message requests that the server set the integrity state 
    /// of the file or directory associated with the handle on which this FSCTL was invoked.
    /// </summary>
    public partial struct FSCTL_SET_INTEGRITY_INFORMATION_BUFFER
    {
        /// <summary>
        /// MUST be one of the following standard values.
        /// </summary>
        public FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM ChecksumAlgorithm;

        /// <summary>
        /// A 16-bit reserved value. This field MUST be set to 0x0000 and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        ///  A 32-bit unsigned integer that contains zero or more of the following flag values. 
        ///  Flag values not specified in the following table SHOULD be set to 0 and MUST be ignored.
        /// </summary>
        public FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS Flags;
    }
    #endregion


    /// <summary>
    /// [MS-SMB2] 2.2.37.1   SMB2_QUERY_QUOTA_INFO
    /// The SMB2_QUERY_QUOTA_INFO packet specifies the quota information to return.
    /// </summary>
    public partial struct SMB2_QUERY_QUOTA_INFO
    {
        /// <summary>
        /// A Boolean value, where zero represents FALSE and nonzero represents TRUE.
        /// If the ReturnSingle field is TRUE, the server MUST return a single value. 
        /// Otherwise, the server SHOULD return the maximum number of entries 
        /// that will fit in the maximum output size that is indicated in the request.
        /// </summary>
        public byte ReturnSingle;

        /// <summary>
        /// A Boolean value, where zero represents FALSE and nonzero represents TRUE.
        /// If RestartScan is TRUE, the quota information MUST be read from the beginning. 
        /// Otherwise, the quota information MUST be continued from the previous enumeration that was executed on this open.
        /// </summary>
        public byte RestartScan;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. 
        /// The client MUST set this field to 0, and the server MUST ignore it on receipt.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The length, in bytes, of the SidBuffer when sent in format 1 as defined in the SidBuffer field or zero in all other cases.
        /// </summary>
        public uint SidListLength;

        /// <summary>
        /// The length, in bytes, of the SID, as specified in [MS-DTYP] section 2.4.2.2, 
        /// when sent in format 2 as defined in the SidBuffer field, or zero in all other cases.
        /// </summary>
        public uint StartSidLength;

        /// <summary>
        /// The offset, in bytes, from the start of the SidBuffer field to the SID 
        /// when sent in format 2 as defined in the SidBuffer field, or zero in all other cases.
        /// </summary>
        public uint StartSidOffset;
    }

    /// <summary>
    /// [MS-SMB] 2.2.7.5.1, structure for NT_TRANSACT_QUERY_QUOTA. 
    /// </summary>
    public partial struct SMB_QUERY_QUOTA_INFO
    {
        /// <summary>
        /// An Fid to a file or directory. The quota information of the object store underlying the file or directory MUST be queried.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// If TRUE (any non-zero value), then this field indicates that the server behavior is to be restricted to 
        /// only return a single SID's quota information instead of filling the entire buffer.
        /// </summary>
        public byte ReturnSingleEntry;

        /// <summary>
        /// If TRUE (any non-zero value), then this field indicates that the scan of quota information is to be restarted.
        /// </summary>
        public byte RestartScan;

        /// <summary>
        /// If non-zero, then this field indicates that the client is requesting quota information of a particular set of SIDs 
        /// and MUST represent the length of the NT_Trans_Data.SidList field.
        /// </summary>
        public uint SidListLength;

        /// <summary>
        /// If non-zero, then this field indicates that the SidList field contains a start SID 
        /// (that is, a single SID entry indicating to the server where to start user quota information enumeration) 
        /// and MUST represent the length, in bytes, of that SidList entry.
        /// </summary>
        public uint StartSidLength;

        /// <summary>
        /// If StartSidLength is non-zero, then this field MUST represent the offset from the start of the NT_Trans_Data 
        /// to the specific SidList entry at which to begin user quota information enumeration. 
        /// Otherwise, this field SHOULD be set to zero and MUST be ignored by the server.
        /// </summary>
        public uint StartSidOffset;
    }


    /// <summary>
    /// The Symbolic Link Reparse Data Buffer data element is  a subtype of REPARSE_DATA_BUFFER, which contains  
    /// information  on symbolic link reparse points. This reparse data  buffer MUST be used only with reparse tag  
    /// values whose  high bit is set to 1. A symbolic link has a substitute  name and a print name associated with  
    /// it. The substitute  name is a path name identifying the target of the symbolic  link; the print name is a path 
    ///  name, suitable for display  to a user, that also identifies the target of the symbolic  link. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\b41f1cbf-10df-4a47-98d4-1c52a833d913.xml
    //  </remarks>
    public partial struct Symbolic_Link_Reparse_Data_Buffer
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the reparse  point tag that uniquely identifies the owner (that 
        ///   is, the implementer of the filter driver associated  with this ReparseTag) of the reparse point. This  
        /// value  MUST be 0xA000000C, a reparse point tag assigned to  Microsoft. 
        /// </summary>
        public ReparseTag_Values ReparseTag;

        /// <summary>
        /// A 16-bit unsigned integer value containing the size,  in bytes, of the reparse data that follows the  
        /// common  portion of the REPARSE_DATA_BUFFER element. This value  is the length of the data starting at the  
        /// SubstituteNameOffset  field (or the size of the PathBuffer field, in bytes,  plus 12). 
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        /// A 16-bit field. This field is not used. It SHOULD be  set to 0 and MUST be ignored. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A 16-bit unsigned integer that contains the offset,  in bytes, of the substitute name string in the  
        /// PathBuffer  array, computed as an offset from byte 0 of PathBuffer.  Note that this offset must be divided 
        ///  by 2 to get the  array index. 
        /// </summary>
        public ushort SubstituteNameOffset;

        /// <summary>
        /// A 16-bit unsigned integer that contains the length,  in bytes, of the substitute name string. If this  
        /// string  is NULL-terminated, SubstituteNameLength does not include  the Unicode NULL character. 
        /// </summary>
        public ushort SubstituteNameLength;

        /// <summary>
        /// A 16-bit unsigned integer that contains the offset,  in bytes, of the print name string in the PathBuffer  
        ///  array, computed as an offset from byte 0 of PathBuffer.  Note that this offset must be divided by 2 to 
        /// get  the  array index. 
        /// </summary>
        public ushort PrintNameOffset;

        /// <summary>
        /// A 16-bit unsigned integer that contains the length,  in bytes, of the print name string. If this string   
        /// is NULL-terminated, PrintNameLength does not include  the Unicode NULL character. 
        /// </summary>
        public ushort PrintNameLength;

        /// <summary>
        /// A 32-bit bit field that specifies whether the substitute  name is a full windows_nt path name or a path  
        /// name  relative to the directory containing the symbolic link.   This field contains one of the values in  
        /// the following  table. 
        /// </summary>
        public Symbolic_Link_Reparse_Data_Buffer_Flags_Values Flags;

        /// <summary>
        /// Unicode character array that contains the substitute  name string and print name string. The substitute  
        /// name  and print name strings can appear in any order in the  PathBuffer. To locate the substitute name and 
        ///  print  name strings in the PathBuffer, use the SubstituteNameOffset,  SubstituteNameLength,  
        /// PrintNameOffset, and PrintNameLength  members. A substitute name may contain a relative pathname  or may  
        /// contain relative directory names. For more details,  see section . 
        /// </summary>
        [Size("ReparseDataLength")]
        public byte[] PathBuffer;
    }

    /// <summary>
    /// A 32-bit unsigned integer value containing the reparse  point tag that uniquely identifies the owner (that   
    /// is, the implementer of the filter driver associated  with this ReparseTag) of the reparse point. This value   
    /// MUST be 0xA000000C, a reparse point tag assigned to  Microsoft. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ReparseTag_Values : uint
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        IO_REPARSE_TAG_SYMLINK = 0xA000000C,
    }

    /// <summary>
    /// A 32-bit bit field that specifies whether the substitute  name is a full windows_nt path name or a path name   
    /// relative to the directory containing the symbolic link.   This field contains one of the values in the  
    /// following  table. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Symbolic_Link_Reparse_Data_Buffer_Flags_Values : uint
    {

        /// <summary>
        /// The substitute name is a full windows_nt path name. 
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        /// The substitute name is a path name relative to the directory  containing the symbolic link. 
        /// </summary>
        SYMLINK_FLAG_RELATIVE = 0x00000001,
    }

    /// <summary>
    /// The FSCTL_GET_RETRIEVAL_POINTERS reply message returns  the results of the FSCTL_GET_RETRIEVAL_POINTERS  
    /// request  as a variably–sized data element, RETRIEVAL_POINTERS_BUFFER,  that specifies the allocation and  
    /// location on disk  of a specific file.  For the NTFS file system, the  FSCTL_GET_RETRIEVAL_POINTERS  
    /// replyreturns the extent  locations (that is, locations of allocated regions  of disk space) of nonresident  
    /// data. A file system MAY  allow resident data, which is data that can be written  to disk within the file's  
    /// directory record. Because  resident data requires no additional disk space allocation,  no extent locations  
    /// are associated with resident data.  On an NTFSvolume, very short data streams (typically  several hundred  
    /// bytes) can be written to disk without  having any clusters allocated. These short streams  are sometimes  
    /// called resident because the data resides  in the file's master file table (MFT) record. A resident  data  
    /// stream has no retrieval pointers to return. The  RETRIEVAL_POINTERS_BUFFER data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\b89a7eb0-2455-4c19-a7c3-8c0b54b81960.xml
    //  </remarks>
    public partial struct FSCTL_GET_RETRIEVAL_POINTERS_Reply
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  EXTENTS data elements in the Extents array. This  
        /// number  can be zero if there are no clusters allocated at (or  beyond) the specified StartingVcn. 
        /// </summary>
        public uint ExtentCount;

        /// <summary>
        /// Reserved for alignment. This field can contain any value  and MUST be ignored. 
        /// </summary>
        public uint Unused;

        /// <summary>
        /// A 64-bit signed integer that contains the starting virtual  cluster number (VCN) returned by the  
        /// FSCTL_GET_RETRIEVAL_POINTERS  reply. This is not necessarily the VCN requested by  the  
        /// FSCTL_GET_RETRIEVAL_POINTERS request, as the file  system driver might round down to the first VCN of  the 
        ///  extent in which the requested starting VCN is found.  This value MUST be greater than or equal to 0. 
        /// </summary>
        public long StartingVcn;

        /// <summary>
        /// An array of zero or more EXTENTS data elements. For  the number of EXTENTS data elements in the array, see 
        ///   ExtentCount. 
        /// </summary>
        [Size("ExtentCount")]
        public byte[] Extents;
    }



    /// <summary>
    /// A 1-byte Boolean (unsigned char) that is TRUE (0x01)  if the file system supports object-oriented file system  
    ///  objects; otherwise, FALSE (0x00). This value is TRUE  for NTFS and FALSE for other file systems implemented   
    /// by windows. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SupportsObjects_Values : byte
    {

        /// <summary>
        /// TRUE 
        /// </summary>
        V1 = 0x01,

        /// <summary>
        /// FALSE 
        /// </summary>
        V2 = 0x00,
    }

    /// <summary>
    /// The FSCTL_SET_ENCRYPTION request sets the encryption for the file or directory associated with the given handle.    
    /// This message is implemented only on NTFS, and it is only for private use by the Encrypted File System (EFS).  
    /// The message contains an ENCRYPTION_BUFFER structure that indicates whether to encrypt/decrypt a file or an individual stream. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\bf78ff7e-b0a4-4ba9-8825-4af43682eb0d.xml
    //  </remarks>
    public partial struct ENCRYPTION_BUFFER
    {

        /// <summary>
        /// A 32-bit unsigned integer value that indicates the operation  to be performed. The valid values are as  
        /// follows. 
        /// </summary>
        public EncryptionOperation_Values EncryptionOperation;

        /// <summary>
        /// An 8-bit unsigned char value. 
        /// </summary>
        public byte Private;

        /// <summary>
        /// These bytes MUST be ignored.
        /// </summary>
        [StaticSize(3)]
        public byte[] Padding;
    }

    /// <summary>
    /// A 32-bit unsigned integer value that indicates the operation  to be performed. The valid values are as  
    /// follows. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum EncryptionOperation_Values : uint
    {

        /// <summary>
        /// This operation requests encryption of the specified  file or directory. windows sets the  
        /// FILE_ATTRIBUTE_ENCRYPTED  flag in the duplicate information file attributes field  and invokes the EFS  
        /// callback which then creates the  $EFS attribute. 
        /// </summary>
        FILE_SET_ENCRYPTION = 0x00000001,

        /// <summary>
        /// This operation requests removal of encryption from the  specified file or directory. It MUST fail if any  
        /// streams  for the file are marked encrypted. windows takes the  following actions to clear encryption:  
        /// Clears the FILE_ATTRIBUTE_ENCRYPTED  flag in the duplicate information file attributes field.  Invokes the 
        ///  EFS callback which removes the $EFS attribute. 
        /// </summary>
        FILE_CLEAR_ENCRYPTION = 0x00000002,

        /// <summary>
        /// This operation requests encryption of the specified  stream. windows takes the following actions to set   
        /// encryption on a stream: If the stream is a resident  user data stream, converts it to non-resident. Sets   
        /// ATTRIBUTE_FLAG_ENCRYPTED in the attribute header. Invokes  the EFS callback to generate a encryption  
        /// context for  this stream. Note that if this is called during the  creation of a named data attribute on a  
        /// file with an  empty unnamed data attribute, then the unnamed data  attribute will be converted to  
        /// non-resident and its  attribute header flag will be set to encrypted. Also  note that this will set the  
        /// FILE_ATTRIBUTE_ENCRYPTED  flag if it is the first stream on the file that is  encrypted. 
        /// </summary>
        STREAM_SET_ENCRYPTION = 0x00000003,

        /// <summary>
        /// This operation requests encryption of the specified  file or directory. windows clears the  
        /// ATTRIBUTE_FLAG_ENCRYPTED  flag from the attribute header and invokes the EFS  callback to free the  
        /// encryption context for the stream. 
        /// </summary>
        STREAM_CLEAR_ENCRYPTION = 0x00000004,
    }

    /// <summary>
    /// The REPARSE_DATA_BUFFER data element stores data for  a reparse point. This reparse data buffer MUST be used   
    /// only with reparse tag values whose high bit is set  to 1.  This data element has two subtypes: Symbolic  Link  
    /// Reparse Data Buffer  and Mount Point Reparse Data  Buffer . 
    /// </summary>
    //  <remarks>
    //   MS-fscc\c3a420cb-8a72-4adf-87e8-eee95379d78f.xml
    //  </remarks>
    public partial struct REPARSE_DATA_BUFFER
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the reparse  point tag that uniquely identifies the owner of  
        /// the  reparse point. 
        /// </summary>
        public uint ReparseTag;

        /// <summary>
        /// A 16-bit unsigned integer value containing the size,  in bytes, of the reparse data in the DataBuffer  
        /// member. 
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        /// A 16-bit field. This field is reserved. This field SHOULD  be set to 0, and MUST be ignored. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A variable-length array of 8-bit unsigned integer values  containing reparse-specific data for the reparse 
        ///  point.  The format of this data is defined by the owner (that  is, the implementer of the filter driver  
        /// associated  with the specified ReparseTag) of the reparse point. 
        /// </summary>
        [Size("ReparseDataLength")]
        public byte[] DataBuffer;
    }

    /// <summary>
    /// The Mount Point Reparse Data Buffer data element is  a subtype of REPARSE_DATA_BUFFER, which contains  
    /// information  about mount point reparse points. This reparse data  buffer MUST be used only with reparse tag  
    /// values whose  high bit is set to 1. A mount point has a substitute  name and a print name associated with it.  
    /// The substitute  name is a path name identifying the target of the mount  point; the print name is a path name, 
    ///  suitable for  display to a user, that also identifies the target  of the mount point. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\ca069dad-ed16-42aa-b057-b6b207f447cc.xml
    //  </remarks>
    public partial struct Mount_Point_Reparse_Data_Buffer
    {

        /// <summary>
        /// A 32-bit unsigned integer value containing the reparse  point tag that uniquely identifies the owner (that 
        ///   is, the implementer of the filter driver associated  with this ReparseTag) of the reparse point. This  
        /// value  MUST be 0xA0000003, a reparse point tag assigned to  Microsoft. 
        /// </summary>
        public Mount_Point_Reparse_Data_Buffer_ReparseTag_Values ReparseTag;

        /// <summary>
        /// A 16-bit unsigned integer value containing the size,  in bytes, of the reparse data that follows the  
        /// common  portion of the REPARSE_DATA_BUFFER element. This value  is the length of the data starting at the  
        /// SubstituteNameOffset  field (or the size of the PathBuffer field, in bytes,  plus 8). 
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        /// A 16-bit field. This field is not used. It SHOULD be  set to 0, and MUST be ignored. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A 16-bit unsigned integer that contains the offset,  in bytes, of the substitute name string in the  
        /// PathBuffer  array, computed as an offset from byte 0 of PathBuffer.  Note that this offset must be divided 
        ///  by 2 to get the  array index. 
        /// </summary>
        public ushort SubstituteNameOffset;

        /// <summary>
        /// A 16-bit unsigned integer that contains the length,  in bytes, of the substitute name string. If this  
        /// string  is NULL-terminated, SubstituteNameLength does not include  the Unicode NULL character. 
        /// </summary>
        public ushort SubstituteNameLength;

        /// <summary>
        /// A 16-bit unsigned integer that contains the offset,  in bytes, of the print name string in the PathBuffer  
        ///  array, computed as an offset from byte 0 of PathBuffer.  Note that this offset must be divided by 2 to 
        /// get  the  array index. 
        /// </summary>
        public ushort PrintNameOffset;

        /// <summary>
        /// A 16-bit unsigned integer that contains the length,  in bytes, of the print name string. If this string   
        /// is NULL-terminated, PrintNameLength does not include  the Unicode NULL character. 
        /// </summary>
        public ushort PrintNameLength;

        /// <summary>
        /// Unicode character array that contains the substitute  name string and print name string. The substitute  
        /// name  and print name strings can appear in any order in PathBuffer.  To locate the substitute name and  
        /// print name strings  in the PathBuffer field, use the SubstituteNameOffset,  SubstituteNameLength,  
        /// PrintNameOffset, and PrintNameLength  members. A substitute name may contain a relative pathname  or may  
        /// contain relative directory names. For more details,  see section . 
        /// </summary>
        [Size("ReparseDataLength")]
        public byte[] PathBuffer;
    }

    /// <summary>
    /// A 32-bit unsigned integer value containing the reparse  point tag that uniquely identifies the owner (that   
    /// is, the implementer of the filter driver associated  with this ReparseTag) of the reparse point. This value   
    /// MUST be 0xA0000003, a reparse point tag assigned to  Microsoft. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Mount_Point_Reparse_Data_Buffer_ReparseTag_Values : uint
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003,
    }


    /// <summary>
    /// A 32-bit unsigned integer that MUST contain one of the  following values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ReadMode_Values : uint
    {

        /// <summary>
        /// If this value is specified, data MUST be read from the  pipe as a stream of bytes. 
        /// </summary>
        FILE_PIPE_BYTE_STREAM_MODE = 0x00000000,

        /// <summary>
        /// If this value is specified, data MUST be read from the  pipe as a stream of messages. 
        /// </summary>
        FILE_PIPE_MESSAGE_MODE = 0x00000001,
    }

    /// <summary>
    /// A 32-bit unsigned integer that MUST contain one of the  following values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CompletionMode_Values : uint
    {

        /// <summary>
        /// If this value is specified, blocking mode MUST be enabled.  When the pipe is being connected, read to, or  
        /// written  from, the operation is not completed until there is  data to read, all data is written, or a  
        /// client is connected.  Use of this mode can mean waiting indefinitely in some  situations for a client  
        /// process to perform an action. 
        /// </summary>
        FILE_PIPE_QUEUE_OPERATION = 0x00000000,

        /// <summary>
        /// If this value is specified, non-blocking mode MUST be  enabled. When the pipe is being connected, read to, 
        ///   or written from, the operation completes immediately. 
        /// </summary>
        FILE_PIPE_COMPLETE_OPERATION = 0x00000001,
    }


    /// <summary>
    /// The FSCTL_QUERY_ALLOCATED_RANGES request message requests  that the server scan a file or alternate stream  
    /// looking  for byte ranges that may contain nonzero data, and  then return information on those ranges. Only  
    /// sparse  files can have zeroed ranges known to the operating  system. For other files, the server will return  
    /// only  a single range that contains the starting point and  the length requested. The message contains a  
    /// FILE_ALLOCATED_RANGE_BUFFER  data element. The FILE_ALLOCATED_RANGE_BUFFER data  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\d2cde38a-d0b9-4412-b966-52011f8cf6cb.xml
    //  </remarks>
    public partial struct FSCTL_QUERY_ALLOCATED_RANGES_Request
    {

        /// <summary>
        /// A 64-bit signed integer that contains the file offset,  in bytes, of the start of a range of bytes in a  
        /// file.  The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        public long FileOffset;

        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes,  of the range. The value of this field MUST be  
        /// greater  than or equal to 0. 
        /// </summary>
        public long Length;
    }


    /// <summary>
    /// The MftBitmapWritesUserLevel structure contains statistics  about MFT bitmap write operations resulting from  
    /// certain  user-level operations. The MftBitmapWritesUserLevel  structure is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\d3bfcebb-e34a-4b1c-bbfd-ae0e2e7324e6.xml
    //  </remarks>
    public partial struct MftBitmapWritesUserLevel
    {

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  bitmap write operations due to a write operation. 
        /// </summary>
        public ushort Write;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  bitmap write operations due to a create operation. 
        /// </summary>
        public ushort Create;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  bitmap write operations due to a set file  
        /// information  operation. 
        /// </summary>
        public ushort SetInfo;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of MFT  bitmap write operations due to a flush operation. 
        /// </summary>
        public ushort Flush;
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains the named pipe  type. MUST be one of the following. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NamedPipeType_Values : uint
    {

        /// <summary>
        /// If this value is specified, data MUST be read from the  pipe as a stream of bytes. 
        /// </summary>
        FILE_PIPE_BYTE_STREAM_TYPE = 0x00000000,

        /// <summary>
        /// If this flag is specified, data MUST be read from the  pipe as a stream of messages. 
        /// </summary>
        FILE_PIPE_MESSAGE_TYPE = 0x00000001,
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains the named pipe  configuration. MUST be one of the following. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NamedPipeConfiguration_Values : uint
    {

        /// <summary>
        /// If this value is specified, the flow of data in the  pipe goes from client to server only. 
        /// </summary>
        FILE_PIPE_INBOUND = 0x00000000,

        /// <summary>
        /// If this value is specified, the flow of data in the  pipe goes from server to client only. 
        /// </summary>
        FILE_PIPE_OUTBOUND = 0x00000001,

        /// <summary>
        /// If this value is specified, the pipe is bi-directional;  both server and client processes can read from  
        /// and  write to the pipe. 
        /// </summary>
        FILE_PIPE_FULL_DUPLEX = 0x00000002,
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains the named pipe  state that specifies the connection status for the   
    /// named pipe. MUST be one of the following. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FilePipeLocalInformation_NamedPipeState_Values : uint
    {

        /// <summary>
        /// Named pipe is disconnected. 
        /// </summary>
        FILE_PIPE_DISCONNECTED_STATE = 0x00000001,

        /// <summary>
        /// Named pipe is waiting to establish a connection. 
        /// </summary>
        FILE_PIPE_LISTENING_STATE = 0x00000002,

        /// <summary>
        /// Named pipe is connected. 
        /// </summary>
        FILE_PIPE_CONNECTED_STATE = 0x00000003,

        /// <summary>
        /// Named pipe is in the process of being closed. 
        /// </summary>
        FILE_PIPE_CLOSING_STATE = 0x00000004,
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains the type of  the named pipe end, which specifies whether this is  the  
    /// client or the server side of a named pipe. MUST  be one of the following. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NamedPipeEnd_Values : uint
    {

        /// <summary>
        /// This is the client end of a named pipe. 
        /// </summary>
        FILE_PIPE_CLIENT_END = 0x00000000,

        /// <summary>
        /// This is the server end of a named pipe. 
        /// </summary>
        FILE_PIPE_SERVER_END = 0x00000001,
    }



    /// <summary>
    /// A 32-bit unsigned integer that contains a bitmask of  flags that control quota enforcement and logging of   
    /// user-related quota events on the volume. The following  bit flags are valid in any combination. Bits not  
    /// defined  in the following table SHOULD be set to 0, and MUST  be ignored.windows sets flags not defined below  
    /// to  zero. Logging makes an entry in the windows application  event log. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileSystemControlFlags_Values : uint
    {

        /// <summary>
        /// Content indexing is disabled. 
        /// </summary>
        FILE_VC_CONTENT_INDEX_DISABLED = 0x00000008,

        /// <summary>
        /// An event log entry will be created when the user exceeds  his or her assigned disk quota limit. 
        /// </summary>
        FILE_VC_LOG_QUOTA_LIMIT = 0x00000020,

        /// <summary>
        /// An event log entry will be created when the user exceeds  his or her assigned quota warning threshold. 
        /// </summary>
        FILE_VC_LOG_QUOTA_THRESHOLD = 0x00000010,

        /// <summary>
        /// An event log entry will be created when the volume's  free space limit is exceeded. 
        /// </summary>
        FILE_VC_LOG_VOLUME_LIMIT = 0x00000080,

        /// <summary>
        /// An event log entry will be created when the volume's  free space threshold is exceeded. 
        /// </summary>
        FILE_VC_LOG_VOLUME_THRESHOLD = 0x00000040,

        /// <summary>
        /// Quotas are tracked and enforced on the volume. 
        /// </summary>
        FILE_VC_QUOTA_ENFORCE = 0x00000002,

        /// <summary>
        /// Quotas are tracked on the volume, but they are not enforced.  Tracked quotas enable reporting on the file  
        /// system  space used by system users. If both this field and  FILE_VC_QUOTA_ENFORCE are specified,  
        /// FILE_VC_QUOTA_ENFORCE  is ignored. 
        /// </summary>
        FILE_VC_QUOTA_TRACK = 0x00000001,

        /// <summary>
        /// The quota information for the volume is incomplete because  it is corrupt, or the system is in the process 
        ///  of rebuilding  the quota information. 
        /// </summary>
        FILE_VC_QUOTAS_INCOMPLETE = 0x00000100,

        /// <summary>
        /// The file system is rebuilding the quota information  for the volume. 
        /// </summary>
        FILE_VC_QUOTAS_REBUILDING = 0x00000200,
    }

    /// <summary>
    /// The FSCTL_FIND_FILES_BY_SID request message requests  that the server return a list of the files (in the   
    /// directory associated with the handle on which this  FSCTL was invoked) whose owner matches the specified   
    /// security identifier (SID). This message contains a  FIND_BY_SID_DATA data element. The FIND_BY_SID_DATA  data  
    /// element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\e904e83a-98da-49db-911d-e1c5ea0783b7.xml
    //  </remarks>
    public partial struct FSCTL_FIND_FILES_BY_SID_Request
    {

        /// <summary>
        /// A 32-bit unsigned integer value that indicates to restart  the search. This value MUST be 1 on first call  
        /// so that  the search starts from the root. For subsequent calls,  this member SHOULD be zero so that the  
        /// search resumes  at the point where it stopped. 
        /// </summary>
        public uint Restart;

        /// <summary>
        /// A SID (see [MS-SECO]) data element that specifies the  owner. 
        /// </summary>
        public SID SID;
    }

    /// <summary>
    /// The second possible structure for the FILE_OBJECTID_BUFFER  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\e9f94ce6-c61c-4f05-b772-af90f9d59d8f.xml
    //  </remarks>
    public partial struct FILE_OBJECTID_BUFFER_Type_2
    {

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the file or  directory within the volume on which it resides.  
        /// Specifically,  the same object ID can be assigned to another file  or directory on a different volume, but 
        ///  it MUST NOT  be assigned to another file or directory on the same  volume. 
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        /// A 48-byte value containing extended data that was set  with the FSCTL_SET_OBJECT_ID_EXTENDED request. This 
        ///   field contains application-specific data.windows places  Distributed Link Tracking information into the  
        /// ExtendedInfo  field for use by the Distributed Link Tracking protocols  (see [MS-DLTW] section ). 
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains a bitmask of  flags that specify attributes of the specified file   
    /// system as a combination of the following flags. The  value of this field MUST be a bitwise OR of zero or  more 
    ///  of the following with the exception that FS_FILE_COMPRESSION  and FS_VOL_IS_COMPRESSED cannot both be set. 
    /// Any  flag  values not explicitly mentioned here can be set to  any value, and MUST be ignored. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileSystemAttributes_Values : uint
    {
        /// <summary>
        /// The file system implements a USN change journal.
        /// </summary>
        FILE_SUPPORTS_USN_JOURNAL = 0x02000000,

        /// <summary>
        /// The file system supports opening a file by FileID or ObjectID.
        /// </summary>
        FILE_SUPPORTS_OPEN_BY_FILE_ID = 0x01000000,

        /// <summary>
        /// The file system persistently stores Extended Attribute information per file.
        /// </summary>
        FILE_SUPPORTS_EXTENDED_ATTRIBUTES = 0x00800000,

        /// <summary>
        /// The file system supports hard linking files.
        /// </summary>
        FILE_SUPPORTS_HARD_LINKS = 0x00400000,

        /// <summary>
        /// The volume supports transactions. windows_vista and  windows_server_2008 set this flag if the volume is   
        /// formatted for NTFS 3.0 or higher. 
        /// </summary>
        FILE_SUPPORTS_TRANSACTIONS = 0x00200000,

        /// <summary>
        /// The volume supports a single sequential write, such  as for a tape-based file system. 
        /// </summary>
        FILE_SEQUENTIAL_WRITE_ONCE = 0x00100000,

        /// <summary>
        /// The specified volume is read-only. This attribute is  only available on windows_xp, windows_server_2003,   
        /// windows_vista, and windows_server_2008. 
        /// </summary>
        FILE_READ_ONLY_VOLUME = 0x00080000,

        /// <summary>
        /// The file system supports named streams. 
        /// </summary>
        FILE_NAMED_STREAMS = 0x00040000,

        /// <summary>
        /// The file system supports the Encrypted File System (EFS).  NTFS version 2 supports EFS. To use EFS, the  
        /// operating  system must support NTFS version 2, and the file system  on disk must be formatted using NTFS  
        /// version 2.NTFS  version 2 is supported on windows_2000, windows_xp,  windows_server_2003, windows_vista,  
        /// and windows_server_2008. 
        /// </summary>
        FILE_SUPPORTS_ENCRYPTION = 0x00020000,

        /// <summary>
        /// The file system supports object identifiers. 
        /// </summary>
        FILE_SUPPORTS_OBJECT_IDS = 0x00010000,

        /// <summary>
        /// The specified volume is a compressed volume. This flag  is incompatible with the FILE_FILE_COMPRESSION  
        /// flag. 
        /// </summary>
        FILE_VOLUME_IS_COMPRESSED = 0x00008000,

        /// <summary>
        /// The file system supports remote storage.  Remote storage  is provided by the Remote Storage service to  
        /// create  virtual disk storage from a tape or other storage media. 
        /// </summary>
        FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100,

        /// <summary>
        /// The file system supports reparse points. 
        /// </summary>
        FILE_SUPPORTS_REPARSE_POINTS = 0x00000080,

        /// <summary>
        /// The file system supports sparse files. 
        /// </summary>
        FILE_SUPPORTS_SPARSE_FILES = 0x00000040,

        /// <summary>
        /// The file system supports per-user quotas. 
        /// </summary>
        FILE_VOLUME_QUOTAS = 0x00000020,

        /// <summary>
        /// The file volume supports file-based compression. This  flag is incompatible with the  
        /// FILE_VOLUME_IS_COMPRESSED  flag. 
        /// </summary>
        FILE_FILE_COMPRESSION = 0x00000010,

        /// <summary>
        /// The file system preserves and enforces access control  lists (ACLs). 
        /// </summary>
        FILE_PERSISTENT_ACLS = 0x00000008,

        /// <summary>
        /// The file system supports Unicode in file and directory  names. This flag applies only to file and  
        /// directory  names; the file system neither restricts nor interprets  the bytes of data within a file. 
        /// </summary>
        FILE_UNICODE_ON_DISK = 0x00000004,

        /// <summary>
        /// The file system preserves the case of file names when  it places a name on disk. 
        /// </summary>
        FILE_CASE_PRESERVED_NAMES = 0x00000002,

        /// <summary>
        /// The file system supports case-sensitive file names when  looking up (searching for) file names in a  
        /// directory. 
        /// </summary>
        FILE_CASE_SENSITIVE_SEARCH = 0x00000001,

        /// <summary>
        /// The file system supports integrity streams.
        /// </summary>
        FILE_SUPPORT_INTEGRITY_STREAMS = 0x04000000,
    }

    /// <summary>
    /// The FSCTL_PIPE_WAIT request requests that the server  wait until either a time-out interval elapses or an   
    /// instance of the specified named pipe is available for  connection. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\f030a3b9-539c-4c7b-a893-86b795b9b711.xml
    //  </remarks>
    public partial struct FSCTL_PIPE_WAIT_Request
    {

        /// <summary>
        /// A 64-bit signed integer that specifies the maximum amount  of time in units of 100 milliseconds that the  
        /// function  can wait for an instance of the named pipe to be available.  A positive value expresses an  
        /// absolute time at which  the base time is the beginning of the year 1601 A.D.  in the Gregorian calendar. A 
        ///  negative value expresses  a time interval relative to some base time, typically  the current time. 
        /// </summary>
        public long Timeout;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the size, in  bytes, of the named pipe Name field. 
        /// </summary>
        public uint NameLength;

        /// <summary>
        /// An 8-bit unsigned value that specifies whether or not  the Timeout parameter will be ignored. 
        /// </summary>
        public TimeoutSpecified_Values TimeoutSpecified;

        /// <summary>
        /// The server MUST ignore this 1-byte padding. 
        /// </summary>
        public byte Padding;

        /// <summary>
        /// A Unicode string that contains the name of the named  pipe. Name MUST not include the "\pipe\", so if the  
        ///  operation was on \\server\pipe\pipename, the name would  be "pipename". 
        /// </summary>
        [Size("NameLength")]
        public byte[] Name;
    }

    /// <summary>
    /// An 8-bit unsigned value that specifies whether or not  the Timeout parameter will be ignored. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum TimeoutSpecified_Values : byte
    {

        /// <summary>
        /// Indicates that the server MUST wait forever (no timeout)  for the named pipe. Any value in Timeout MUST be 
        ///  ignored. 
        /// </summary>
        FALSE = 0,

        /// <summary>
        /// Indicates that the server MUST use the value in the  Timeout parameter. 
        /// </summary>
        TRUE = 1,
    }

    /// <summary>
    /// The FSCTL_QUERY_ALLOCATED_RANGES reply message returns  the results of the FSCTL_QUERY_ALLOCATED_RANGES  
    /// request.  This message MUST return an array of zero or more FILE_ALLOCATED_RANGE_BUFFER  data elements. The  
    /// number of FILE_ALLOCATED_RANGE_BUFFER  elements returned is computed by dividing the size  of the returned  
    /// output buffer (from SMB, the lower-level  protocol that carries the FSCTL) by the size of the   
    /// FILE_ALLOCATED_RANGE_BUFFER element. Ranges returned  are always at least partially within the range specified 
    ///   in the FSCTL_QUERY_ALLOCATED_RANGES request. Zero FILE_ALLOCATED_RANGE_BUFFER  data elements MUST be 
    /// returned  when the file has no  allocated ranges. Each entry in the output array contains  an offset and a 
    /// length that  indicates a range in the  file that may contain nonzero data. The actual nonzero  data, if any, 
    /// is somewhere  within this range, and the  calling application must scan further within the range  to locate it 
    /// and determine  if it really is valid data.  Multiple instances of valid data may exist within the  range. The  
    /// FILE_ALLOCATED_RANGE_BUFFER data element  is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\f413dfc4-31d3-4d68-8d87-bd86ba896afd.xml
    //  </remarks>
    public partial struct FSCTL_QUERY_ALLOCATED_RANGES_Reply
    {

        /// <summary>
        /// A 64-bit signed integer that contains the file offset  in bytes from the start of the file; the start of a 
        ///   range of bytes to which storage is allocated. If the  file is a sparse file, it can contain ranges of  
        /// bytes  for which storage is not allocated; these ranges will  be excluded from the list of allocated  
        /// ranges returned  by this FSCTL.Sparse files are supported by windows_2000,  windows_xp,  
        /// windows_server_2003, windows_vista, and  windows_server_2008. The NTFS file system rounds down  the input  
        /// file offset to a 65,536-byte (64-kilobyte)  boundary, rounds up the length to a convenient boundary,  and  
        /// then begins to walk through the file.  Because  an application using a sparse file can choose whether  or  
        /// not to allocate disk space for each sequence of  0x00-valued bytes, the allocated ranges can contain   
        /// 0x00-valued bytes. This value MUST be greater than  or equal to 0.windows does not track every piece of   
        /// zero (0) or nonzero data. Because zero (0) is often  perfectly legal data, it would be misleading.  
        /// Instead,  the system tracks ranges in which disk space is allocated.  Where no disk space is allocated,  
        /// all data bytes within  that range for Length bytes from FileOffset are assumed  to be zero (0) (when data  
        /// is read, NTFS returns a zero  for every byte in a sparse region). Allocated storage  can contain zero (0)  
        /// or nonzero data. So all that this  operation does is return information on parts of the  file where  
        /// nonzero data might be located. It is up  to the application to scan these parts of the file  in accordance 
        ///  with the application's data conventions. 
        /// </summary>
        public long FileOffset;

        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes,  of the range. This value MUST be greater than  
        /// or equal  to 0. 
        /// </summary>
        public long Length;
    }

    #endregion

    #endregion

    #region 2.4   File Information Classes

    #region FileInformationCommand (File information class, Level)

    /// <summary>
    /// File Information command. File information classes are numerical values (specified by the Level column in the  
    /// following table) that specify what information for a file is to be queried or set. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileInformationCommand : ushort
    {
        /// <summary>
        /// Query. This information class is used to query the access rights of a file. 
        /// </summary>
        FileAccessInformation = 8,

        /// <summary>
        /// Query. The buffer alignment required by the underlying device. 
        /// </summary>
        FileAlignmentInformation = 17,

        /// <summary>
        /// Query This information class is used to query a collection of file information structures. 
        /// </summary>
        FileAllInformation = 18,

        /// <summary>
        /// Set This information class is used to set but not to query the allocation size for a file. The file system 
        ///  is passed a 64-bit signed integer containing the file allocation size in bytes. This value MUST be an  
        /// integer  multiple of the cluster size. 
        /// </summary>
        FileAllocationInformation = 19,

        /// <summary>
        /// Query This information class is used to query alternate name information for a file. The alternate name  
        /// for a  file is its 8.3 format name (eight characters that appear before the "." and three characters that  
        /// appear after). 
        /// </summary>
        FileAlternateNameInformation = 21,

        /// <summary>
        /// Query This information class is used to query for attribute and reparse tag information for a file. 
        /// </summary>
        FileAttributeTagInformation = 35,

        /// <summary>
        /// Query, Set This information class is used to query or set file information. 
        /// </summary>
        FileBasicInformation = 4,

        /// <summary>
        /// Query This information class is used in directory enumeration to return detailed information about the  
        /// contents  of a directory. 
        /// </summary>
        FileBothDirectoryInformation = 3,

        /// <summary>
        /// Query This information class is used to query compression information for a file. 
        /// </summary>
        FileCompressionInformation = 28,

        /// <summary>
        /// Query This information class is used in directory enumeration to return detailed information about the  
        /// contents  of a directory. 
        /// </summary>
        FileDirectoryInformation = 1,

        /// <summary>
        /// Set This information class is used to mark a file for deletion. 
        /// </summary>
        FileDispositionInformation = 13,

        /// <summary>
        /// Query This information class is used to query for the size of the extended attributes (EA) for a file. An  
        ///  extended attribute is a piece of application-specific metadata that an application can associate with a   
        /// file that is not part of the file's data. For more information about extended attributes, see [CIFS]. 
        /// </summary>
        FileEaInformation = 7,

        /// <summary>
        /// Set This information class is used to set end-of-file information for a file. 
        /// </summary>
        FileEndOfFileInformation = 20,

        /// <summary>
        /// Query This information class is used in directory enumeration to return detailed information about the  
        /// contents of a directory. 
        /// </summary>
        FileFullDirectoryInformation = 2,

        /// <summary>
        /// Query, Set This information class is used to query or set extended attribute (EA) information for a file. 
        /// </summary>
        FileFullEaInformation = 15,

        /// <summary>
        /// LOCAL This information class is used to query NTFS hard links to an existing file. At least one name MUST  
        /// be  returned. 
        /// </summary>
        FileHardLinkInformation = 46,

        /// <summary>
        /// Query This information class is used in directory enumeration to return detailed information about the  
        /// contents of a directory. 
        /// </summary>
        FileIdBothDirectoryInformation = 37,

        /// <summary>
        /// Query This information class is used in directory enumeration to return detailed information about the  
        /// contents  of a directory. 
        /// </summary>
        FileIdFullDirectoryInformation = 38,

        /// <summary>
        /// Query This information class is used to query transactional visibility information for the files in a  
        /// directory. This information class MAY be implemented for file systems that return the  
        /// FILE_SUPPORTS_TRANSACTIONS flag in response to specified in section 2.5.1. This information class MUST NOT 
        ///  be implemented for file systems that do not return that flag. 
        /// </summary>
        FileIdGlobalTxDirectoryInformation = 50,

        /// <summary>
        /// Query This information class is used to query for the file system's 8-byte file reference number for a  
        /// file. 
        /// </summary>
        FileInternalInformation = 6,

        /// <summary>
        /// Set This information class is used to create an NTFS hard link to an existing file. 
        /// </summary>
        FileLinkInformation = 11,

        /// <summary>
        /// LOCAL 
        /// </summary>
        FileMailslotQueryInformation = 26,

        /// <summary>
        /// LOCAL This information class is used to query information on a mailslot. 
        /// </summary>
        FileMailslotSetInformation = 27,

        /// <summary>
        /// Query, Set This information class is used to query or set the mode of the file. 
        /// </summary>
        FileModeInformation = 16,


        /// <summary>
        /// Windows file systems do not implement this file information class; the server will fail it with  
        /// STATUS_NOT_SUPPORTED. 
        /// </summary>
        FileMoveClusterInformation = 31,

        /// <summary>
        /// LOCAL This information class is used to query the name of a file. This information class returns a   
        /// FILE_NAME_INFORMATION data element as described in section 2.1.7. 
        /// </summary>
        FileNameInformation = 9,

        /// <summary>
        /// Query This information class is used in directory enumeration to return detailed information about the   
        /// contents of a directory. 
        /// </summary>
        FileNamesInformation = 12,

        /// <summary>
        /// Query This information class is used to query for information on a network file open. A network file open  
        /// differs from a file open in that the handle obtained from a network file open can be used to look up  
        /// attributes  using FileNetworkOpenInformation, but it cannot be used for reads and writes to the file. The  
        /// network file open is an optimization of file open that returns a file handle to the caller more quickly,  
        /// but the file  handle it returns cannot be used in all of the ways that a normal file handle can be used. 
        /// </summary>
        FileNetworkOpenInformation = 34,

        /// <summary>
        /// Windows file systems do not implement this file information class; the server will fail it with  
        /// STATUS_NOT_SUPPORTED. 
        /// </summary>
        FileNormalizedNameInformation = 48,

        /// <summary>
        /// LOCAL This information class is used to query object ID information for the files in a directory on a  
        /// volume. The query MUST fail if the file system does not support object IDs. 
        /// </summary>
        FileObjectIdInformation = 29,

        /// <summary>
        /// Query, Set This information class is used to query or set information on a named pipe that is not specific 
        ///  to one end  of the pipe or another. 
        /// </summary>
        FilePipeInformation = 23,

        /// <summary>
        /// Query This information class is used to query information on a named pipe that is associated with the end  
        /// of the  pipe that is being queried. 
        /// </summary>
        FilePipeLocalInformation = 24,

        /// <summary>
        /// Query, Set This information class is used to query or set information on a named pipe that is associated  
        /// with the  client end of the pipe that is being queried. Remote information is not available for local  
        /// pipes or for  the server end of a remote pipe. 
        /// </summary>
        FilePipeRemoteInformation = 25,

        /// <summary>
        /// Query, Set This information class is used to query or set the position of the file pointer within a file. 
        /// </summary>
        FilePositionInformation = 14,

        /// <summary>
        /// Query The information class is used to query quota information. This information class normally uses  
        /// volume handles; however, for an NTFS file system, a handle to NTFS  metadata file "\$Extend\$Quota" is  
        /// also valid. 
        /// </summary>
        FileQuotaInformation = 32,

        /// <summary>
        /// Set This information class is used to rename a file. There are two variations of this structure, depending 
        ///  on  whether it is embedded within [MS-SMB] or [MS-SMB2]. 
        /// </summary>
        FileRenameInformation = 10,

        /// <summary>
        /// LOCAL This information class is used to query for information on a reparse point. 
        /// </summary>
        FileReparsePointInformation = 33,

        /// <summary>
        /// LOCAL This information class is used to query or set reserved bandwidth for a file handle. Conceptually  
        /// reserving bandwidth is effectively specifying the bytes per second to allocate to file IO. 
        /// </summary>
        FileSfioReserveInformation = 44,

        /// <summary>
        /// Windows file systems do not implement this file information class; the server will fail it with  
        /// STATUS_NOT_SUPPORTED. 
        /// </summary>
        FileSfioVolumeInformation = 45,

        /// <summary>
        /// Set This information class is used to change a file's short name. A caller changing the file's short name  
        /// MUST have SeBackupPrivilege. SeBackupPrivilege is specified in the product behavior note of [MS-LSAD]  
        /// section  3.1.1.2.1. 
        /// </summary>
        FileShortNameInformation = 40,

        /// <summary>
        /// Query This information class is used to query file information. 
        /// </summary>
        FileStandardInformation = 5,

        /// <summary>
        /// LOCAL This information class is used to query file link information. 
        /// </summary>
        FileStandardLinkInformation = 54,

        /// <summary>
        /// Query This information class is used to enumerate the data streams for a file. 
        /// </summary>
        FileStreamInformation = 22,

        /// <summary>
        /// LOCAL 
        /// </summary>
        FileTrackingInformation = 36,

        /// <summary>
        /// Set This information class is used to set the valid data length information for a file. 
        /// </summary>
        FileValidDataLengthInformation = 39
    }

    #endregion

    #region File Information Structures

    #region 2.4.1   FileAccessInformation
    /// <summary>
    /// This information class is used to query or set the access  rights of the file. The FILE_ACCESS_INFORMATION  
    /// data  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\01cf43d2-deb3-40d3-a39b-9e68693d7c90.xml
    //  </remarks>
    public partial struct FILE_ACCESS_INFORMATION
    {

        /// <summary>
        /// A DWORD that MUST contain values specified in ACCESS_MASK  of [MS-DTYP]. 
        /// </summary>
        public uint AccessFlags;
    }
    #endregion

    #region 2.4.2   FileAllInformation

    /// <summary>
    /// This information class is used to query a collection  of file information structures. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\95f3056a-ebc1-4f5d-b938-3f68a44677a6.xml
    //  </remarks>
    public partial struct FileAllInformation
    {

        /// <summary>
        /// A FILE_BASIC_INFORMATION structure specified in section  . 
        /// </summary>
        public FileBasicInformation BasicInformation;

        /// <summary>
        /// A FILE_STANDARD_INFORMATION structure specified in section  . 
        /// </summary>
        public FileStandardInformation StandardInformation;

        /// <summary>
        /// A FILE_INTERNAL_INFORMATION structure specified in section  . 
        /// </summary>
        public FileInternalInformation InternalInformation;

        /// <summary>
        /// A FILE_EA_INFORMATION structure specified in section  . 
        /// </summary>
        public FileEaInformation EaInformation;

        /// <summary>
        /// A FILE_ACCESS_INFORMATION structure specified in section  . 
        /// </summary>
        public FILE_ACCESS_INFORMATION AccessInformation;

        /// <summary>
        /// A FILE_POSITION_INFORMATION structure specified in section  . 
        /// </summary>
        public FILE_POSITION_INFORMATION PositionInformation;

        /// <summary>
        /// A FILE_MODE_INFORMATION structure specified in section  . 
        /// </summary>
        public FILE_MODE_INFORMATION ModeInformation;

        /// <summary>
        /// A FILE_ALIGNMENT_INFORMATION structure specified in  section . 
        /// </summary>
        public FILE_ALIGNMENT_INFORMATION AlignmentInformation;

        /// <summary>
        /// A FILE_NAME_INFORMATION structure specified in section  . 
        /// </summary>
        public FileNameInformation NameInformation;
    }

    #endregion

    #region 2.4.3   FileAlignmentInformation

    /// <summary>
    /// The buffer alignment required by the underlying device.  The FILE_ALIGNMENT_INFORMATION data element is as  
    /// follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\9b0b9971-85aa-4651-8438-f1c4298bcb0d.xml
    //  </remarks>
    public partial struct FILE_ALIGNMENT_INFORMATION
    {

        /// <summary>
        /// A 32-bit unsigned integer that MUST contain one of the  following values. 
        /// </summary>
        public AlignmentRequirement_Values AlignmentRequirement;
    }

    /// <summary>
    /// A 32-bit unsigned integer that MUST contain one of the  following values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum AlignmentRequirement_Values : uint
    {
        /// <summary>
        /// If this value is specified, there are no alignment requirements  for the device. 
        /// </summary>
        FILE_BYTE_ALIGNMENT = 0x00000000,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 2-byte boundary. 
        /// </summary>
        FILE_WORD_ALIGNMENT = 0x00000001,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 4-byte boundary. 
        /// </summary>
        FILE_LONG_ALIGNMENT = 0x00000003,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  an 8-byte boundary. 
        /// </summary>
        FILE_QUAD_ALIGNMENT = 0x00000007,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 16-byte boundary. 
        /// </summary>
        FILE_OCTA_ALIGNMENT = 0x0000000f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 32-byte boundary. 
        /// </summary>
        FILE_32_BYTE_ALIGNMENT = 0x0000001f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 64-byte boundary. 
        /// </summary>
        FILE_64_BYTE_ALIGNMENT = 0x0000003f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 128-byte boundary. 
        /// </summary>
        FILE_128_BYTE_ALIGNMENT = 0x0000007f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 256-byte boundary. 
        /// </summary>
        FILE_256_BYTE_ALIGNMENT = 0x000000ff,

        /// <summary>
        /// If this value is specified, data MUST be aligned on  a 512-byte boundary. 
        /// </summary>
        FILE_512_BYTE_ALIGNMENT = 0x000001ff,
    }

    #endregion

    #region 2.4.4   FileAllocationInformation

    /// <summary> 
    /// This information class is used to set but not to query the allocation size for a file. 
    /// The file system is passed a 64-bit signed integer containing the file allocation size, in bytes. 
    /// The file system rounds the requested allocation size up to an integer multiple of the cluster size for nonresident files, 
    /// or an implementation-defined multiple for resident files.All unused allocation (beyond EOF) is freed on the last handle close. 
    /// </summary>
    public struct FILE_ALLOCATION_INFORMATION
    {
        /// <summary>
        /// A 64-bit signed integer that contains the desired allocation to be used by the given file.
        /// </summary>
        public ulong AllocationSize;
    }

    #endregion

    #region 2.4.5   FileAlternateNameInformation

    /// <summary>
    /// This information class is used to query alternate name  information for a file. The alternate name for a file  
    ///  is its 8.3 format name (eight characters that appear  before the "." and three characters that appear after). 
    ///   A file MAY have an alternate name to achieve compatibility  with the 8.3 naming requirements of legacy  
    /// applications.  NTFS assigns an alternate name to a file whose full  name is not compliant with restrictions  
    /// for file names  under MS-DOS and 16-bit windows unless the system has  been configured through a registry  
    /// entry to not generate  these names to improve performance. The FILE_NAME_INFORMATION  data element is as  
    /// follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\cb90d9e0-695d-4418-8d89-a29e2ba9faf8.xml
    //  </remarks>
    public partial struct FileAlternateNameInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the length in  bytes of the FileName member. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and   
        /// MUST be handled as a sequence of FileNameLength bytes. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.6   FileAttributeTagInformation

    /// <summary>
    /// This information class is used to query for attribute  and reparse tag information for a file. The  
    /// FILE_ATTRIBUTE_TAG_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\d295752f-ce89-4b98-8553-266d37c84f0e.xml
    //  </remarks>
    public partial struct FileAttributeTagInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the file attributes.  Valid file attributes are as specified in  
        /// section . 
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the reparse  point tag. If the FileAttributes member includes the 
        ///   FILE_ATTRIBUTE_REPARSE_POINT attribute flag, this member  specifies the reparse tag. Otherwise, this  
        /// member SHOULD  be set to 0, and MUST be ignored. Section  contains  more details on reparse tags. 
        /// </summary>
        public uint ReparseTag;
    }

    #endregion

    #region 2.4.7   FileBasicInformation

    /// <summary>
    /// This information class is used to query or set file  information. The FILE_BASIC_INFORMATION data element  is  
    /// as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\16023025-8a78-492f-8b96-c873b042ac50.xml
    //  </remarks>
    public partial struct FileBasicInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the time when  the file was created. All dates and times in this  
        /// message  are in absolute system-time format, which is represented  as a FILETIME structure. A valid time  
        /// for this field  is an integer greater than 0. When setting file attributes,  a value of 0 indicates to the 
        ///  server that it MUST NOT  change this attribute. When setting file attributes,  a value of -1 indicates to 
        ///  the server that it MUST  NOT change this attribute for all subsequent operations  on the same file 
        /// handle.  This field MUST NOT be set  to a value less than -1. 
        /// </summary>
        public FILETIME CreationTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  the file was accessed in the format of a FILETIME  
        /// structure.  A valid time for this field is an integer greater than  0. When setting file attributes, a  
        /// value of 0 indicates  to the server that it MUST NOT change this attribute.  When setting file attributes, 
        ///  a value of -1 indicates  to the server that it MUST NOT change this attribute  for all subsequent  
        /// operations on the same file handle.  This field MUST NOT be set to a value less than -1.   The file system 
        ///  updates the values of the LastAccessTime,  LastWriteTime, and ChangeTime members as appropriate  after an 
        ///  I/O operation is performed on a file. However,  a driver or application can request that the file system  
        ///  not update one or more of these members for I/O operations  that are performed on the caller's file 
        /// handle  by setting  the appropriate members to -1. The caller can set one,  all, or any other combination 
        /// of these  three members  to -1. Only the members that are set to -1 will be  unaffected by I/O operations 
        /// on the  file handle; the  other members will be updated as appropriate. This  behavior is consistent 
        /// across all  file system types.  Note that even though -1 can be used with the CreationTime  field, it has 
        /// no effect  since file creation time is  never updated in response to file system calls such  as read and 
        /// write. 
        /// </summary>
        public FILETIME LastAccessTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  information was written to the file in the format of  
        ///  a FILETIME structure. A valid time for this field is  an integer greater than 0. When setting file  
        /// attributes,  a value of 0 indicates to the server that it MUST NOT  change this attribute. When setting  
        /// file attributes,  a value of -1 indicates to the server that it MUST  NOT change this attribute for all  
        /// subsequent operations  on the same file handle. This field MUST NOT be set  to a value less than -1.  The  
        /// file system updates the  values of the LastAccessTime, LastWriteTime, and ChangeTime  members as  
        /// appropriate after an I/O operation is performed  on a file. However, a driver or application can request   
        /// that the file system not update one or more of these  members for I/O operations that are performed on the 
        ///   caller's file handle by setting the appropriate members  to -1. The caller can set one, all, or any 
        /// other  combination  of these three members to -1. Only the members that  are set to -1 will be unaffected 
        /// by I/O  operations  on the file handle; the other members will be updated  as appropriate. This behavior 
        /// is  consistent across  all file system types. Note that even though -1 can  be used with the CreationTime  
        /// field, it has no effect  since file creation time is never updated in response  to file system calls such  
        /// as read and write. 
        /// </summary>
        public FILETIME LastWriteTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  the file was changed in the format of a FILETIME  
        /// structure.  A valid time for this field is an integer greater than  0. When setting file attributes, a  
        /// value of 0 indicates  to the server that it MUST NOT change this attribute.  When setting file attributes, 
        ///  a value of -1 indicates  to the server that it MUST NOT change this attribute  for all subsequent  
        /// operations on the same file handle.  This field MUST NOT be set to a value less than -1.   The file system 
        ///  updates the values of the LastAccessTime,  LastWriteTime, and ChangeTime members as appropriate  after an 
        ///  I/O operation is performed on a file. However,  a driver or application can request that the file system  
        ///  not update one or more of these members for I/O operations  that are performed on the caller's file 
        /// handle  by setting  the appropriate members to -1. The caller can set one,  all, or any other combination 
        /// of these  three members  to -1. Only the members that are set to -1 will be  unaffected by I/O operations 
        /// on the  file handle; the  other members will be updated as appropriate. This  behavior is consistent 
        /// across all  file system types.  Note that even though -1 can be used with the CreationTime  field, it has 
        /// no effect  since file creation time is  never updated in response to file system calls such  as read and 
        /// write. 
        /// </summary>
        public FILETIME ChangeTime;

        /// <summary>
        /// A 32-bit unsigned integer that contains the file attributes.  Valid file attributes are specified in  
        /// section . 
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        /// A 32-bit field. This field is reserved. This field can  be set to any value, and MUST be ignored. 
        /// </summary>
        public uint Reserved;
    }

    #endregion

    #region 2.4.8   FileBothDirectoryInformation

    /// <summary>
    /// This information class is used to query detailed information  for the files in a directory.  The  
    /// FILE_BOTH_DIR_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\270df317-9ba5-4ccb-ba00-8d22be139bc5.xml
    //  </remarks>
    public partial struct FileBothDirectoryInformation
    {
        /// <summary>
        /// The common structure shared by FileDirectoryInformation, FileBothDirectoryInformation, FileFullDirectoryInformation, 
        /// FileIdBothDirectoryInformation, FileIdFullDirectoryInformation, FileIdGlobalTxDirectoryInformation
        /// </summary>
        public FileCommonDirectoryInformation FileCommonDirectoryInformation;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A 32-bit unsigned integer that contains the combined  length, in bytes, of the extended attributes (EA)  
        /// for  the file. 
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// MUST be an 8-bit signed value containing the length,  in bytes, of the ShortName field. If there is a NULL 
        ///   termination at the end of the string, it is not included  in the FileNameLength count. This value MUST 
        /// be  greater  than or equal to 0. 
        /// </summary>
        public byte ShortNameLength;

        /// <summary>
        /// Reserved for alignment. This field can contain any value  and MUST be ignored. 
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// A 24-byte Unicode char field containing the short (8.3)  file name. The file name might be  
        /// NULL-terminated. 
        /// </summary>
        [StaticSize(24, StaticSizeMode.Elements)]
        public byte[] ShortName;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and   
        /// MUST be handled as a sequence of FileNameLength bytes.  For every directory that is the target of the  
        /// FileBothDirectoryInformation  element, there MUST be a FILE_BOTH_DIR_INFORMATION  data element with the  
        /// FileName field containing the  relative directory names "." and "..". For more details,  see section . 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.9   FileCompressionInformation

    /// <summary>
    /// A 16-bit unsigned integer that contains the compression  format. The actual compression operation associated   
    /// with each of these compression format values is implementation-dependent.  An implementation can associate any 
    ///  local compression  algorithm with the values described in the following  table because the compressed data  
    /// does not travel across  the wire in the context of FSCTL, FileInformation class,  or FileSystemInformation  
    /// class requests or replies.windows_2000,  windows_xp, windows_server_2003, windows_vista, and   
    /// windows_server_2008 implement only one compression  algorithm, LZNT1. For more information, see [UASDC].   
    /// COMPRESSION_FORMAT_DEFAULT is therefore equivalent  to COMPRESSION_FORMAT_LZNT1. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CompressionFormat_Values : ushort
    {

        /// <summary>
        /// The file or directory is not compressed. 
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        /// The file or directory is compressed by using the default  compression algorithm. 
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        /// The file or directory is compressed by using the LZNT1  compression algorithm. 
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,
    }

    /// <summary>
    /// This information class is used to query compression  information for a file.  The FILE_COMPRESSION_INFORMATION 
    ///   data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\0a7e50c4-2839-438e-aa6c-0da7d681a5a7.xml
    //  </remarks>
    public partial struct FileCompressionInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes,  of the compressed file. This value MUST be  
        /// greater  than or equal to 0. 
        /// </summary>
        public long CompressedFileSize;

        /// <summary>
        /// A 16-bit unsigned integer that contains the compression  format. The actual compression operation  
        /// associated  with each of these compression format values is implementation-dependent.  An implementation  
        /// can associate any local compression  algorithm with the values described in the following  table because  
        /// the compressed data does not travel across  the wire in the context of FSCTL, FileInformation class,  or  
        /// FileSystemInformation class requests or replies.windows_2000,  windows_xp, windows_server_2003,  
        /// windows_vista, and  windows_server_2008 implement only one compression  algorithm, LZNT1. For more  
        /// information, see [UASDC].  COMPRESSION_FORMAT_DEFAULT is therefore equivalent  to  
        /// COMPRESSION_FORMAT_LZNT1. 
        /// </summary>
        public CompressionFormat_Values CompressionFormat;

        /// <summary>
        /// An 8-bit unsigned integer that contains the compression  unit shift, which is the number of bits by which  
        /// to  left-shift a 1 bit to arrive at the compression unit  size. The compression unit size is the number of 
        ///  bytes  in a compression unit, that is, the number of bytes  to be compressed. This value is  
        /// implementation-defined.NTFS  uses a value of 16 calculated as (4 + ClusterShift)  for the  
        /// CompressionUnitShift by default. The ultimate  size of data to be compressed depends on the cluster  size  
        /// set for the file system at initialization. NTFS  defaults to a 4-kilobyte cluster size, resulting in  a  
        /// ClusterShift value of 12, but NTFS file systems can  be initialized with a different cluster size, so the  
        ///  value may vary. The default compression unit size based  on this calculation is 64 kilobytes. 
        /// </summary>
        public byte CompressionUnitShift;

        /// <summary>
        /// An 8-bit unsigned integer that contains the compression  chunk size in bytes in log 2 format. The chunk  
        /// size  is the number of bytes that the operating system's  implementation of the Lempel-Ziv compression  
        /// algorithm  tries to compress at one time. This value is implementation-defined.NTFS  uses a value of 12  
        /// for the ChunkShift so that compression  chunks are 4 kilobytes in size. 
        /// </summary>
        public byte ChunkShift;

        /// <summary>
        /// An 8-bit unsigned integer that specifies, in log 2 format,  the amount of space that must be saved by  
        /// compression  to successfully compress a compression unit. If that  amount of space is not saved by  
        /// compression, the data  in that compression unit is stored uncompressed. Each  successfully compressed  
        /// compression unit MUST occupy  at least one cluster that is less in bytes than an  uncompressed compression 
        ///  unit. Therefore, the cluster  shift is the number of bits by which to left shift  a 1 bit to arrive at 
        /// the  size of a cluster. This value  is implementation defined. The value of this field  depends on the 
        /// cluster  size set for the file system  at initialization. NTFS uses a value of 12 by default  because the 
        /// default  NTFScluster size is 4-kilobyte  bytes. If an NTFS file system is initialized with a  different 
        /// cluster  size, the value of ClusterShift would  be log 2 of the cluster size for that file system. 
        /// </summary>
        public byte ClusterShift;

        /// <summary>
        /// A 24-bit reserved value. This field SHOULD be set to  0, and MUST be ignored. 
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;
    }
    #endregion

    #region 2.4.10   FileDirectoryInformation

    /// <summary>
    /// The common part of the structure FileDirectoryInformation, FileBothDirectoryInformation, FileFullDirectoryInformation, 
    /// FileIdBothDirectoryInformation, FileIdFullDirectoryInformation, FileIdGlobalTxDirectoryInformation
    /// </summary>
    public struct FileCommonDirectoryInformation
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  from the beginning of this entry, at which the  
        /// next  FILE_DIRECTORY_INFORMATION entry is located, if multiple  entries are present in a buffer. This  
        /// member MUST be  zero if no other entries follow this one. An implementation  MUST use this value to  
        /// determine the location of the  next entry (if multiple entries are present in a buffer),  and MUST NOT  
        /// assume that the value of NextEntryOffset  is the same as the size of the current entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  of the file within the parent directory. For file 
        ///  systems  in which the position of a file within the parent directory  is not fixed and can be changed at  
        /// any time to maintain  sort order, this field SHOULD be set to 0 and MUST  be ignored. When using NTFS, the 
        ///  position of a file  within the parent directory is not fixed and can be  changed at any time.  
        /// windows_2000, windows_xp, windows_server_2003,  windows_vista, and windows_server_2008 set this value  to  
        /// 0 for files on NTFS file systems. 
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// A 64-bit signed integer that contains the time when  the file was created. All dates and times are in  
        /// absolute  system-time format, which is represented as a FILETIME  structure. This value MUST be greater  
        /// than or equal to 0. 
        /// </summary>
        public FILETIME CreationTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  the file was accessed in the format of a FILETIME  
        /// structure.  This value MUST be greater than or equal to 0. 
        /// </summary>
        public FILETIME LastAccessTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  information was written to the file in the format of  
        ///  a FILETIME structure. This value MUST be greater than  or equal to 0. 
        /// </summary>
        public FILETIME LastWriteTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  the file was changed in the format of a FILETIME  
        /// structure.  This value MUST be greater than or equal to 0. 
        /// </summary>
        public FILETIME ChangeTime;

        /// <summary>
        /// A 64-bit signed integer that contains the absolute new  end-of-file position as a byte offset from the  
        /// start  of the file. EndOfFile specifies the offset to the  byte immediately following the last valid byte  
        /// in the  file. Because this value is zero-based, it actually  refers to the first free byte in the file.  
        /// That is,  it is the offset from the beginning of the file at  which new bytes appended to the file will be 
        ///  written.  The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        public long EndOfFile;

        /// <summary>
        /// A 64-bit signed integer that contains the file allocation  size, in bytes. Usually, this value is a  
        /// multiple of  the sector or cluster size of the underlying physical  device. The value of this field MUST  
        /// be greater than  or equal to 0. 
        /// </summary>
        public long AllocationSize;

        /// <summary>
        /// A 32-bit unsigned integer that contains the file attributes.  Valid attributes are as specified in section 
        ///  . 
        /// </summary>
        public uint FileAttributes;
    }

    /// <summary>
    /// This information class is used to query detailed information  for the files in a directory.  The  
    /// FILE_DIRECTORY_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\b38bf518-9057-4c88-9ddd-5e2d3976a64b.xml
    //  </remarks>
    public partial struct FileDirectoryInformation
    {
        public FileCommonDirectoryInformation FileCommonDirectoryInformation;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and   
        /// MUST be handled as a sequence of FileNameLength bytes.  For every directory that is the target of the  
        /// FileDirectoryInformation  element, there MUST be a FILE_DIRECTORY_INFORMATION  data element with the  
        /// FileName field containing the  relative directory names "." and "..". For more details,  see section . 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.11   FileDispositionInformation

    /// <summary>
    /// This information class is used to mark a file for deletion.   The FILE_DISPOSITION_INFORMATION data element is 
    ///  as  follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\12c3dd1c-14f6-4229-9d29-75fb2cb392f6.xml
    //  </remarks>
    public partial struct FileDispositionInformation
    {

        /// <summary>
        /// An 8-bit field that is set to 1 to indicate that a file  SHOULD be deleted when it is closed; otherwise,  
        /// 0.  A file marked for deletion is not actually deleted  until all open handles for the file object have  
        /// been  closed, and the link count for the file is zero. 
        /// </summary>
        public byte DeletePending;
    }

    #endregion

    #region 2.4.12   FileEaInformation

    /// <summary>
    /// This information class is used to query for the size  of the extended attributes (EA) for a file. An extended  
    ///  attribute is a piece of application-specific metadata  that an application can associate with a file that  is 
    ///  not part of the file's data. For more information  about extended attributes, see [CIFS].  The  
    /// FILE_EA_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\db6cf109-ead8-441a-b29e-cb2032778b0f.xml
    //  </remarks>
    public partial struct FileEaInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the combined  length, in bytes, of the extended attributes (EA)  
        /// for  the file. 
        /// </summary>
        public uint EaSize;
    }

    #endregion

    #region 2.4.13   FileEndOfFileInformation

    /// <summary>
    /// This information class is used to set end-of-file information  for a file.  The FILE_END_OF_FILE_INFORMATION  
    /// data  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\75241cca-3167-472f-8058-a52d77c6bb17.xml
    //  </remarks>
    public partial struct FileEndOfFileInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the absolute new  end of file position as a byte offset from the  
        /// start  of the file. EndOfFile specifies the offset from the  beginning of the file of the byte following  
        /// the last  byte in the file. That is, it is the offset from the  beginning of the file at which new bytes  
        /// appended to  the file will be written. The value of this field MUST  be greater than or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long EndOfFile;
    }

    #endregion

    #region 2.4.14   FileFullDirectoryInformation

    /// <summary>
    /// This information class is used to query detailed information  for the files in a directory.  The  
    /// FILE_FULL_DIR_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\e8d926d1-3a22-4654-be9c-58317a85540b.xml
    //  </remarks>
    public partial struct FileFullDirectoryInformation
    {
        /// <summary>
        /// The common structure shared by FileDirectoryInformation, FileBothDirectoryInformation, FileFullDirectoryInformation, 
        /// FileIdBothDirectoryInformation, FileIdFullDirectoryInformation, FileIdGlobalTxDirectoryInformation
        /// </summary>
        public FileCommonDirectoryInformation FileCommonDirectoryInformation;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A 32-bit unsigned integer that contains the combined  length, in bytes, of the extended attributes (EA)  
        /// for  the file. 
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and   
        /// MUST be handled as a sequence of FileNameLength bytes.  For every directory that is the target of the  
        /// FileFullDirectoryInformation  element, there MUST be a FILE_FULL_DIR_INFORMATION  data element with the  
        /// FileName field containing the  relative directory names "." and "..". For more details,  see section . 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.15   FileFullEaInformation
    /// <summary>
    /// This information class is used to query or set extended  attribute (EA) information for a file.  The  
    /// FILE_FULL_EA_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\0eb94f48-6aac-41df-a878-79f4dcfd8989.xml
    //  </remarks>
    public partial struct FileFullEaInformation
    {

        /// <summary>
        /// A 32-bit unsigned 4-byte aligned integer that contains  the byte offset from the beginning of this entry,  
        /// at  which the next FILE_ FULL_EA _INFORMATION entry is  located, if multiple entries are present in the  
        /// buffer.  This member MUST be zero if no other entries follow  this one. An implementation MUST use this  
        /// value to  determine the location of the next entry (if multiple  entries are present in a buffer), and  
        /// MUST NOT assume  that the value of NextEntryOffset field is the same  as the size of the current entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// An 8-bit unsigned integer that contains one of the following  flag values. 
        /// </summary>
        public FILE_FULL_EA_INFORMATION_FLAGS Flags;

        /// <summary>
        /// An 8-bit unsigned integer that contains the length,  in bytes, of the extended attribute name in the  
        /// EaName  field. This value does not include the null-terminator  to EaName. 
        /// </summary>
        public byte EaNameLength;

        /// <summary>
        /// A 16-bit unsigned integer that contains the length,  in bytes, of the extended attribute value in the  
        /// EaValue  field. 
        /// </summary>
        public ushort EaValueLength;

        /// <summary>
        /// An array of 8-bit ASCII characters that contains the  extended attribute name followed by a single  
        /// null-termination  character byte. 
        /// </summary>
        [Size("EaNameLength + 1")]
        public byte[] EaName;

        /// <summary>
        /// An array of bytes that contains the extended attribute  value. The length of this array is specified by  
        /// the  EaValueLength field. 
        /// </summary>
        [Size("EaValueLength")]
        public byte[] EaValue;
    }

    /// <summary>
    /// An 8-bit unsigned integer that contains one of the following  flag values. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FILE_FULL_EA_INFORMATION_FLAGS : byte
    {

        /// <summary>
        /// If no flags are set, this EA does not prevent the file to which the EA belongs from being interpreted by applications that do not understand EAs. 
        /// </summary>
        NONE = 0x00000000,

        /// <summary>
        /// If this flag is set, the file to which the EA belongs cannot be interpreted without understanding the  
        /// associated  extended attributes. 
        /// </summary>
        FILE_NEED_EA = 0x00000080,
    }
    #endregion

    #region 2.4.16   FileHardLinkInformation
    /// <summary>
    /// The FILE_LINK_ENTRY_INFORMATION packet is used to describe  a single NTFS hard link to an existing file. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\070580ed-46de-450a-a428-9f98a0d71d6d.xml
    //  </remarks>
    public partial struct FILE_LINK_ENTRY_INFORMATION
    {

        /// <summary>
        /// A 32-bit unsigned integer that MUST specify the offset,  in bytes, from the current  
        /// FILE_LINK_ENTRY_INFORMATION  structure to the next FILE_LINK_ENTRY_INFORMATION structure.  A value of 0  
        /// indicates this is the last entry structure. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 64-bit signed integer that MUST contain the FileID  of the parent directory of the given link. 
        /// </summary>
        public long ParentFileId;

        /// <summary>
        /// A 32-bit unsigned integer that MUST specify the length,  in characters, of the FileName for the given  
        /// link. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A WCHAR array whose size is given by FileNameLength  that MUST contain the Unicode string name of the  
        /// given  link. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }
    #endregion

    #region 2.4.17   FileIdBothDirectoryInformation

    /// <summary>
    /// This information class is used to query file reference  number information for the files in a directory.  The  
    ///  FILE_ID_BOTH_DIR_INFORMATION data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\1e144bff-c056-45aa-bd29-c13d214ee2ba.xml
    //  </remarks>
    public partial struct FileIdBothDirectoryInformation
    {
        /// <summary>
        /// The common structure shared by FileDirectoryInformation, FileBothDirectoryInformation, FileFullDirectoryInformation, 
        /// FileIdBothDirectoryInformation, FileIdFullDirectoryInformation, FileIdGlobalTxDirectoryInformation
        /// </summary>
        public FileCommonDirectoryInformation FileCommonDirectoryInformation;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A 32-bit unsigned integer that contains the combined  length, in bytes, of the extended attributes (EA)  
        /// for  the file. 
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// An 8-bit value containing the length, in bytes, of the  ShortName string. 
        /// </summary>
        public byte ShortNameLength;

        /// <summary>
        /// MUST be ignored by the receiver. 
        /// </summary>
        public byte Reserved1;

        /// <summary>
        /// A NULL-terminated 12-character Unicode string containing  the short file name (8.3 name). 
        /// </summary>
        [StaticSize(24, StaticSizeMode.Elements)]
        public byte[] ShortName;

        /// <summary>
        /// MUST be ignored by the receiver. 
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// An 8-byte file reference number for the file. This number  SHOULD be generated and assigned to the file by 
        ///  the  file system. For file systems which do not support  FileId, this field MUST be set to 0, and MUST be 
        ///  ignored. 
        /// </summary>
        public long FileId;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and   
        /// MUST be handled as a sequence of FileNameLength bytes.  For every directory that is the target of the  
        /// FileIdBothDirectoryInformation  element, there MUST be a FILE_ID_BOTH_DIR_INFORMATION  data element with  
        /// the FileName field containing the  relative directory names "." and "..". For more details,  see section . 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.18   FileIdFullDirectoryInformation

    /// <summary>
    /// This information class is used to query detailed information  for the files in a directory.  The  
    /// FILE_ID_FULL_DIR_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\ab8e7558-899c-4be1-a7c5-3a9ae8ab76a0.xml
    //  </remarks>
    public partial struct FileIdFullDirectoryInformation
    {
        /// <summary>
        /// The common structure shared by FileDirectoryInformation, FileBothDirectoryInformation, FileFullDirectoryInformation, 
        /// FileIdBothDirectoryInformation, FileIdFullDirectoryInformation, FileIdGlobalTxDirectoryInformation
        /// </summary>
        public FileCommonDirectoryInformation FileCommonDirectoryInformation;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A 32-bit unsigned integer that contains the combined  length, in bytes, of the extended attributes (EA)  
        /// for  the file. 
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// Reserved for alignment. This field can contain any value  and MUST be ignored. 
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// An 8-byte file reference number for the file. This number  is generated and assigned to the file by the  
        /// file system.  For file systems which do not support FileId, this  field MUST be set to 0, and MUST be  
        /// ignored. 
        /// </summary>
        public long FileId;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and   
        /// MUST be handled as a sequence of FileNameLength bytes.  For every directory that is the target of the  
        /// FileIdFullDirectoryInformation  element, there MUST be a FILE_ID_FULL_DIR_INFORMATION  data element with  
        /// the FileName field containing the  relative directory names "." and "..". For more details,  see section . 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.19   FileIdGlobalTxDirectoryInformation

    /// <summary>
    /// This information class is used to query transactional  visibility information for the files in a directory.   
    /// This information class MAY be implemented for file  systems that return the FILE_SUPPORTS_TRANSACTIONS  flag  
    /// in response to FileFsAttributeInformation specified  in section . This information class MUST NOT be  
    /// implemented  for file systems that do not return that flag. The  FILE_ID_GLOBAL_TX_DIR_INFORMATION data  
    /// element is as  follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\84121f32-27e1-40d1-a256-09f8ac3964c6.xml
    //  </remarks>
    public partial struct FileIdGlobalTxDirectoryInformation
    {
        /// <summary>
        /// The common structure shared by FileDirectoryInformation, FileBothDirectoryInformation, FileFullDirectoryInformation, 
        /// FileIdBothDirectoryInformation, FileIdFullDirectoryInformation, FileIdGlobalTxDirectoryInformation
        /// </summary>
        public FileCommonDirectoryInformation FileCommonDirectoryInformation;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// An 8-byte file reference number for the file. This number  is generated and assigned to the file by the  
        /// file system.  For file systems that do not support FileId, this field  MUST be set to 0, and MUST be  
        /// ignored. 
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] FileId;

        /// <summary>
        /// A GUID value that is the ID of the transaction that  has this file locked for modification. This number   
        /// is generated and assigned by the file system. If the  FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED flag is  
        ///  not set in the TxInfoFlags field, this field MUST be  ignored. 
        /// </summary>
        public System.Guid LockingTransactionId;

        /// <summary>
        /// A 32-bit unsigned integer that contains a bitmask of  flags that indicate the transactional visibility of  
        ///  the file. The value of this field MUST be a bitwise  OR of zero or more of the following values. Any flag 
        ///   values not explicitly mentioned here can be set to  any value and MUST be ignored. If the  
        /// FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED  flag is not set, the other flags MUST NOT be set. If  flags  
        /// other than FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED  are set,  
        /// FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED  MUST be set. 
        /// </summary>
        public TxInfoFlags_Values TxInfoFlags;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field MAY NOT be NULL-terminated and MUST 
        ///   be handled as a sequence of FileNameLength bytes. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.20   FileInternalInformation

    /// <summary>
    /// This information class is used to query for the file  system's 8-byte file reference number for a file.   The  
    /// FILE_INTERNAL_INFORMATION data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\7d796611-2fa5-41ac-8178-b6fea3a017b3.xml
    //  </remarks>
    public partial struct FileInternalInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the 8-byte file  reference number for the file. This number MUST be  
        ///  assigned by the file system and is unique to the volume  on which the file or directory is located. This  
        /// file  reference number is the same as the file reference  number that is stored in the FileId field of the 
        ///  FILE_ID_BOTH_DIR_INFORMATION  and FILE_ID_FULL_DIR_INFORMATION data elements. The  value of this field  
        /// MUST be greater than or equal to  0. For file systems which do not support a file reference  number, this  
        /// field MUST be set to 0, and MUST be ignored. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long IndexNumber;
    }

    #endregion

    #region 2.4.21   FileLinkInformation

    /// <summary>
    /// This information class is used to create a hard link to an existing file via the SMB Protocol 
    /// </summary>
    public partial struct FILE_LINK_INFORMATION_TYPE_SMB
    {
        /// <summary>
        /// Set to 1 to indicate that if the link already exists, it SHOULD be replaced with the new link. 
        /// Set to 0 to indicate that the link creation operation MUST fail if the link already exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        /// This field SHOULD be set to zero by the client and MUST be ignored by the server.
        /// </summary>
        [StaticSize(3)]
        public byte[] Reserved;

        /// <summary>
        /// A 32-bit unsigned integer that contains the file handle for the directory where the link is to be created. 
        /// For network operations, this value MUST always be zero.
        /// </summary>
        public uint RootDirectory;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length in bytes of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters that contains the name to be assigned to the newly created link. 
        /// When working with the FileName field, the FileNameLength field is used to determine the length of the file name 
        /// rather than assuming the presence of a trailing null delimiter. 
        /// If the RootDirectory field is zero, this field MUST specify a full pathname to the link to be created. 
        /// For network operations, this pathname is relative to the root of the share. 
        /// If the RootDirectory field is not zero, this field MUST specify a pathname, relative to RootDirectory, for the link name. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// This information class is used to create a hard link to an existing file via the SMB Version 2 Protocol.
    /// </summary>
    public partial struct FILE_LINK_INFORMATION_TYPE_SMB2
    {
        /// <summary>
        /// Set to 1 to indicate that if the link already exists, it SHOULD be replaced with the new link. 
        /// Set to 0 to indicate that the link creation operation MUST fail if the link already exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        /// Reserved for alignment. This field can contain any value and MUST be ignored.
        /// </summary>
        [StaticSize(7)]
        public byte[] Reserved;

        /// <summary>
        /// A 64-bit unsigned integer that contains the file handle for the directory where the link is to be created. 
        /// For network operations, this value MUST be zero.
        /// </summary>
        public ulong RootDirectory;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the length in bytes of the file name contained within the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the name to be assigned to the newly created link. 
        /// When working with this field, the FileNameLength field is used to determine the length of the file name 
        /// rather than assuming the presence of a trailing null delimiter. 
        /// If the RootDirectory field is zero, this field MUST specify a full pathname to the link to be created. 
        /// For network operations, this pathname is relative to the root of the share. 
        /// If the RootDirectory field is not zero, this field MUST specify a pathname, relative to RootDirectory, for the link name.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.22   FileMailslotQueryInformation

    /// <summary>
    /// This information class is used to query information  on a mailslot.  The FILE_MAILSLOT_QUERY_INFORMATION  data 
    ///  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\701a8cf4-99f8-4613-b2f8-3cdab1d568f2.xml
    //  </remarks>
    public partial struct FileMailslotQueryInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the maximum  size of a single message that can be written to the   
        /// mailslot in bytes. To specify that the message can  be of any size, set this value to zero. 
        /// </summary>
        public uint MaximumMessageSize;

        /// <summary>
        /// A 32-bit unsigned integer that contains the quota in  bytes for the mailslot. The mailslot quota specifies 
        ///   the in-memory pool quota that is reserved for writes  to this mailslot. 
        /// </summary>
        public uint MailslotQuota;

        /// <summary>
        /// A 32-bit unsigned integer that contains the next message  size in bytes. 
        /// </summary>
        public uint NextMessageSize;

        /// <summary>
        /// A 32-bit unsigned integer that contains the total number  of messages waiting to be read from the  
        /// mailslot. 
        /// </summary>
        public uint MessagesAvailable;

        /// <summary>
        /// A 64-bit signed integer that contains the time a read  operation can wait for a message to be written to  
        /// the  mailslot before a time-out occurs in milliseconds.  The value of this field MUST be (-1) or greater  
        /// than  or equal to 0. A value of (-1) requests that the read  wait forever for a message, without timing  
        /// out. A value  of 0 requests that the read not wait and return immediately  whether a pending message is  
        /// available to be read or  not. 
        /// </summary>
        public long ReadTimeout;
    }

    #endregion

    #region 2.4.23   FileMailslotSetInformation

    /// <summary>
    /// This information class is used to set information on  a mailslot.  The FILE_MAILSLOT_SET_INFORMATION data   
    /// element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\d3259c2b-b841-40af-9deb-b51edb0f1ef5.xml
    //  </remarks>
    public partial struct FileMailslotSetInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the time a read  operation can wait for a message to be written to  
        /// the  mailslot before a time-out occurs in milliseconds.  The value of this field MUST be (-1) or greater  
        /// than  or equal to 0. A value of (-1) requests that the read  wait forever for a message without timing  
        /// out. A value  of 0 requests that the read not wait and return immediately  whether a pending message is  
        /// available to be read or  not. 
        /// </summary>
        public long ReadTimeout;
    }

    #endregion

    #region 2.4.24   FileModeInformation

    /// <summary>
    /// This information class is used to query or set the mode  of the file. The FILE_MODE_INFORMATION data element   
    /// is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\52df7798-8330-474b-ac31-9afe8075640c.xml
    //  </remarks>
    public partial struct FILE_MODE_INFORMATION
    {

        /// <summary>
        /// A ULONG that MUST specify on file creation or file open  how the file will subsequently be accessed. 
        /// </summary>
        public Mode_Values Mode;
    }

    #endregion

    #region 2.4.25   FileNameInformation

    /// <summary>
    /// This information class is used to query detailed information  of a file.  The FILE_NAME_INFORMATION data  
    /// element  is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\cb30e415-54c5-4483-a346-822ea90e1e89.xml
    //  </remarks>
    public partial struct FileNameInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and
        /// MUST be handled as a sequence of FileNameLength bytes. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.26   FileNamesInformation

    /// <summary>
    /// This information class is used to query detailed information  on the names of files in a directory.  The  
    /// FILE_NAMES_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\a289f7a8-83d2-4927-8c88-b2d328dde5a5.xml
    //  </remarks>
    public partial struct FileNamesInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  from the beginning of this entry, at which the  
        /// next  FILE_ NAMES _INFORMATION entry is located, if multiple  entries are present in a buffer. This member 
        ///  MUST be  zero if no other entries follow this one. An implementation  MUST use this value to determine 
        /// the  location of the  next entry (if multiple entries are present in a buffer),  and MUST NOT assume that 
        /// the  value of NextEntryOffset  is the same as the size of the current entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  of the file within the parent directory. For file 
        ///  systems  in which the position of a file within the parent directory  is not fixed and can be changed at  
        /// any time to maintain  sort order, this field SHOULD be set to 0, and MUST  be ignored.  When using NTFS,  
        /// the position of a file  within the parent directory is not fixed and can be  changed at any time.  
        /// windows_2000, windows_xp, windows_server_2003,  windows_vista, and windows_server_2008 set this value  to  
        /// 0 for files on NTFS file systems. 
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field might not be NULL-terminated, and   
        /// MUST be handled as a sequence of FileNameLength bytes. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.27   FileNetworkOpenInformation

    /// <summary>
    /// This information class is used to query for information  on a network file open. A network file open differs   
    /// from a file open in that the handle obtained from a  network file open can be used to look up attributes   
    /// using FileNetworkOpenInformation, but it cannot be  used for reads and writes to the file. The network  file  
    /// open is an optimization of file open that returns  a file handle to the caller more quickly, but the file   
    /// handle it returns cannot be used in all of the ways  that a normal file handle can be used. The  
    /// FILE_NETWORK_OPEN_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\26d261db-58d1-4513-a548-074448cbb146.xml
    //  </remarks>
    public partial struct FileNetworkOpenInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the time when  the file was created in the format of a FILETIME  
        /// structure.  The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME CreationTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  the file was accessed in the format of a FILETIME  
        /// structure.  The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME LastAccessTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  information was written to the file in the format of  
        ///  a FILETIME structure. The value of this field MUST  be greater than or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME LastWriteTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  the file was changed in the format of a FILETIME  
        /// structure.  The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME ChangeTime;

        /// <summary>
        /// A 64-bit signed integer that contains the file allocation  size in bytes. Usually, this value is a  
        /// multiple of  the sector or cluster size of the underlying physical  device. The value of this field MUST  
        /// be greater than  or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long AllocationSize;

        /// <summary>
        /// A 64-bit signed integer that contains the absolute new  end-of-file position as a byte offset from the  
        /// start  of the file. EndOfFile specifies the offset to the  byte immediately following the last valid byte  
        /// in the  file. Because this value is zero-based, it actually  refers to the first free byte in the file.  
        /// That is,  it is the offset from the beginning of the file at  which new bytes appended to the file will be 
        ///  written.  The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long EndOfFile;

        /// <summary>
        /// A 32-bit unsigned integer that contains the file attributes.  Valid attributes are as specified in section 
        ///  . 
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        /// A 32-bit field. This field is reserved. This field can  be set to any value, and MUST be ignored. 
        /// </summary>
        public uint Reserved;
    }

    #endregion

    #region 2.4.28   FileObjectIdInformation

    /// <summary>
    /// The first type of data that may be returned. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\63cdde16-85ac-480c-95bf-0bb8f5f09de8.xml
    //  </remarks>
    public partial struct FileObjectIdInformation_Type_1
    {

        /// <summary>
        /// A 64-bit unsigned integer that contains the file reference  number for the file. NTFS generates this  
        /// number and  assigns it to the file automatically when the file  is created. The file reference number is  
        /// unique within  the volume on which the file exists. 
        /// </summary>
        public ulong FileReferenceNumber;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the file or  directory within the volume on which it resides.  
        /// Specifically,  the same object ID can be assigned to another file  or directory on a different volume, but 
        ///  it MUST NOT  be assigned to another file or directory on the same  volume. 
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the volume on  which the object resided when the object identifier 
        ///   was created, or zero if the volume had no object identifier  at that time. After copy operations, move  
        /// operations,  or other file operations, this may not be the same  as the object identifier of the volume on 
        ///  which the  object presently resides. 
        /// </summary>
        public System.Guid BirthVolumeId;

        /// <summary>
        /// A 16-byte GUID value containing the object identifier  of the object at the time it was created. After  
        /// copy  operations, move operations, or other file operations,  this value may not be the same as the  
        /// ObjectId member  at present. When a file is moved or copied from one  volume to another, the ObjectId  
        /// member's value changes  to a random unique value to avoid the potential for  ObjectId collisions because  
        /// the object ID is not guaranteed  to be unique across volumes. 
        /// </summary>
        public System.Guid BirthObjectId;

        /// <summary>
        /// A 16-byte GUID value containing the domain identifier.  This value is unused; it SHOULD be zero, and MUST  
        /// be  ignored. 
        /// </summary>
        public System.Guid DomainId;
    }

    /// <summary>
    /// The second type of data that may be returned. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\42fa1651-35a5-4cdd-94fc-881917b714b2.xml
    //  </remarks>
    public partial struct FileObjectIdInformation_Type_2
    {

        /// <summary>
        /// A 64-bit unsigned integer that contains the file reference  number for the file. NTFS generates this  
        /// number and  assigns it to the file automatically when the file  is created. The file reference number is  
        /// unique within  the volume on which the file exists. 
        /// </summary>
        public ulong FileReferenceNumber;

        /// <summary>
        /// A 16-byte GUID that uniquely identifies the file or  directory within the volume on which it resides.  
        /// Specifically,  the same object ID can be assigned to another file  or directory on a different volume, but 
        ///  it MUST NOT  be assigned to another file or directory on the same  volume. 
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        /// A 48-byte BLOB that contains application-specific extended  information on the file object. If no extended 
        ///  information  has been written for this file, the server MUST return  48 bytes of 0x00 in this field. 
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    #endregion

    #region 2.4.29   FilePipeInformation

    /// <summary>
    /// This information class is used to query or set information  on a named pipe that is not specific to one end of 
    ///   the pipe or another.  The FILE_PIPE_INFORMATION data  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\cd805dd2-9248-4024-ac0f-b87a702dd366.xml
    //  </remarks>
    public partial struct FilePipeInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that MUST contain one of the  following values. 
        /// </summary>
        public ReadMode_Values ReadMode;

        /// <summary>
        /// A 32-bit unsigned integer that MUST contain one of the  following values. 
        /// </summary>
        public CompletionMode_Values CompletionMode;
    }

    #endregion

    #region 2.4.30   FilePipeLocalInformation

    /// <summary>
    /// This information class is used to query information  on a named pipe that is associated with the end of  the  
    /// pipe that is being queried.  The FILE_PIPE_LOCAL_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\de9abdc7-b974-4ec3-a4dc-42853777f412.xml
    //  </remarks>
    public partial struct FilePipeLocalInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the named pipe  type. MUST be one of the following. 
        /// </summary>
        public NamedPipeType_Values NamedPipeType;

        /// <summary>
        /// A 32-bit unsigned integer that contains the named pipe  configuration. MUST be one of the following. 
        /// </summary>
        public NamedPipeConfiguration_Values NamedPipeConfiguration;

        /// <summary>
        /// A 32-bit unsigned integer that contains the maximum  number of instances that can be created for this  
        /// pipe.  The first instance of the pipe MUST specify this value. 
        /// </summary>
        public uint MaximumInstances;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  current named pipe instances. 
        /// </summary>
        public uint CurrentInstances;

        /// <summary>
        /// A 32-bit unsigned integer that contains the inbound  quota in bytes for the named pipe. 
        /// </summary>
        public uint InboundQuota;

        /// <summary>
        /// A 32-bit unsigned integer that contains the bytes of  data available to be read from the named pipe. 
        /// </summary>
        public uint ReadDataAvailable;

        /// <summary>
        /// A 32-bit unsigned integer that contains outbound quota  in bytes for the named pipe. 
        /// </summary>
        public uint OutboundQuota;

        /// <summary>
        /// A 32-bit unsigned integer that contains the write quota  in bytes for the named pipe. 
        /// </summary>
        public uint WriteQuotaAvailable;

        /// <summary>
        /// A 32-bit unsigned integer that contains the named pipe  state that specifies the connection status for the 
        ///   named pipe. MUST be one of the following. 
        /// </summary>
        public FilePipeLocalInformation_NamedPipeState_Values NamedPipeState;

        /// <summary>
        /// A 32-bit unsigned integer that contains the type of  the named pipe end, which specifies whether this is   
        /// the client or the server side of a named pipe. MUST  be one of the following. 
        /// </summary>
        public NamedPipeEnd_Values NamedPipeEnd;
    }

    #endregion

    #region 2.4.31   FilePipeRemoteInformation

    /// <summary>
    /// This information class is used to query or set information  on a named pipe that is associated with the client 
    ///   end of the pipe that is being queried. Remote information  is not available for local pipes or for the 
    /// server   end of a remote pipe. The FILE_PIPE_REMOTE_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\4319b135-4472-482f-a0a3-6cc3a856c6b6.xml
    //  </remarks>
    public partial struct FilePipeRemoteInformation
    {

        /// <summary>
        /// A LARGE_INTEGER that MUST contain the maximum amount  of time counted in 100-nanosecond intervals that  
        /// will  elapse before transmission of data from the client  machine to the server. 
        /// </summary>
        public _LARGE_INTEGER CollectDataTime;

        /// <summary>
        /// A ULONG that MUST contain the maximum size in bytes  of data that will be collected on the client machine  
        ///  before transmission to the server. 
        /// </summary>
        public uint MaximumCollectionCount;
    }

    #endregion

    #region 2.4.32   FilePositionInformation

    /// <summary>
    /// This information class is used to query the position  of the file pointer within a file. The  
    /// FILE_POSITION_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\e3ce4a39-327e-495c-99b6-6b61606b6f16.xml
    //  </remarks>
    public partial struct FILE_POSITION_INFORMATION
    {

        /// <summary>
        /// A LARGE_INTEGER that MUST contain the offset, in bytes,  of the file pointer from the beginning of the  
        /// file. 
        /// </summary>
        public _LARGE_INTEGER CurrentByteOffset;
    }

    #endregion

    #region 2.4.33   FileQuotaInformation

    /// <summary>
    /// The information class is used to query or set quota  information. This information class normally uses volume  
    ///  handles; however, for an NTFS file system, a handle  to NTFS metadata file \$Extend\$Quota is also valid.  
    /// The  FILE_QUOTA_INFORMATION data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\acdc0738-ba3c-47a1-b11a-72e22d831c57.xml
    //  </remarks>
    public partial struct FILE_QUOTA_INFORMATION
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  from the beginning of this entry, at which the  
        /// next  FILE_QUOTA_INFORMATION entry is located, if multiple  entries are present in a buffer. This member  
        /// MUST be  zero if no other entries follow this one. An implementation  MUST use this value to determine the 
        ///  location of the  next entry (if multiple entries are present in a buffer),  and MUST NOT assume that the  
        /// value of NextEntryOffset  is the same as the size of the current entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the SID data element. 
        /// </summary>
        public uint SidLength;

        /// <summary>
        /// A 64-bit signed integer that contains the last time  the quota was changed in the format of a FILETIME  
        /// structure.  This value MUST be greater than or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "")]
        public _FILETIME ChangeTime;

        /// <summary>
        /// A 64-bit signed integer that contains the amount of  quota used by this user in bytes. This value MUST be  
        ///  greater than or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long QuotaUsed;

        /// <summary>
        /// A 64-bit signed integer that contains the disk quota  warning threshold in bytes on this volume for this   
        /// user. This field MUST be set to a 64-bit integer value  greater than or equal to 0 to set a quota warning  
        /// threshold  for this user on this volume, or to (-1) to specify  that no quota warning threshold is set for 
        ///  this user. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long QuotaThreshold;

        /// <summary>
        /// A 64-bit signed integer that contains the disk quota  limit in bytes on this volume for this user. This  
        /// field  MUST be set to a 64-bit integer value greater than  or equal to 0 to set a disk quota limit for  
        /// this user  on this volume, or to (-1) to specify that no quota  limit is set for this user. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long QuotaLimit;

        /// <summary>
        /// Security identifier (SID) for this user. 
        /// </summary>
        [Size("SidLength")]
        public byte[] Sid;
    }

    #endregion

    #region 2.4.33.1   FILE_GET_QUOTA_INFORMATION

    /// <summary>
    /// The information class is used to provide the list of  SIDs for which query quota information is requested. 
    /// When multiple FILE_GET_QUOTA_INFORMATION data elements are present in the buffer, each MUST be aligned on a 4-byte boundary. 
    /// Any bytes inserted for alignment SHOULD be set to zero, and the receiver MUST ignore them. No padding is required following the last data element.
    /// </summary>
    //  <remarks>
    //   MS-fscc\56adae21-add4-4434-97ec-e40e87739d52.xml
    //  </remarks>
    public partial struct FILE_GET_QUOTA_INFORMATION
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  from the beginning of this entry, at which the  
        /// next  FILE_GET_QUOTA_INFORMATION entry is located, if multiple  entries are present in a buffer. This  
        /// member MUST be  zero if no other entries follow this one. An implementation  MUST use this value to  
        /// determine the location of the  next entry (if multiple entries are present in a buffer),  and MUST NOT  
        /// assume that the value of NextEntryOffset  is the same as the size of the current entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the Sid data element. 
        /// </summary>
        public uint SidLength;

        /// <summary>
        /// SID for this user. SIDs are sent in little-endian format  and require no packing. The format of a SID is  
        /// as specified  in [MS-DTYP] section . 
        /// </summary>
        [Size("SidLength")]
        public byte[] Sid;
    }

    #endregion

    #region 2.4.34   FileRenameInformation

    /// <summary>
    /// This information class is used to rename a file from  within the SMB Protocol, as specified in [MS-SMB].   The 
    ///  FILE_RENAME_INFORMATION data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\3668ae46-1df5-4656-b481-763877428bcb.xml
    //  </remarks>
    public partial struct FileRenameInformation_SMB
    {

        /// <summary>
        /// MUST be an 8-bit field that is set to 1 to indicate  that if a file with the given name already exists,   
        /// it SHOULD be replaced with the given file. If set to  0, the rename operation MUST fail if a file with the 
        ///   given name already exists. 
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        /// Reserved area for alignment. This field can contain  any value and MUST be ignored. 
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        /// A 32-bit unsigned integer that contains the file handle  for the root directory. For network operations,  
        /// this  value MUST always be zero. 
        /// </summary>
        public RootDirectory_Values RootDirectory;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length in  bytes of the new name for the file, including the  
        /// trailing  NULL if present. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. It is not a NULL-terminated string. This field 
        ///   MUST be handled as a sequence of FileNameLength bytes,  not as a NULL-terminated string. If the  
        /// RootDirectory  member is 0, this member MUST specify an absolute path  name to be assigned to the file. If 
        ///  the RootDirectory  member is not 0, this member MUST specify a relative  path name, relative to  
        /// RootDirectory, for the new name  of the file. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains the file handle  for the root directory. For network operations, this  
    ///  value MUST always be zero. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RootDirectory_Values : uint
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// This information class is used to rename a file from  within the SMB2 Protocol [MS-SMB2].  The  
    /// FILE_RENAME_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\52aa0b70-8094-4971-862d-79793f41e6a8.xml
    //  </remarks>
    public partial struct FileRenameInformation_SMB2
    {

        /// <summary>
        /// MUST be an 8-bit field that is set to 1 to indicate  that if a file with the given name already exists,   
        /// it SHOULD be replaced with the given file. If set to  0, the rename operation MUST fail if a file with the 
        ///   given name already exists. 
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        /// Reserved area for alignment. This field can contain  any value and MUST be ignored. 
        /// </summary>
        [StaticSize(7, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        /// A 64-bit unsigned integer that contains the file handle  for the root directory. For network operations,  
        /// this  value MUST always be zero. 
        /// </summary>
        public FileRenameInformation_SMB2_RootDirectory_Values RootDirectory;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length in  bytes of the new name for the file, including the  
        /// trailing  NULL if present. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. It is not a NULL-terminated string. This field 
        ///   MUST be handled as a sequence of FileNameLength bytes,  not as a NULL-terminated string. If the  
        /// RootDirectory  member is 0, this member MUST specify an absolute path  name to be assigned to the file. If 
        ///  the RootDirectory  member is not 0, this member MUST specify a relative  path name, relative to  
        /// RootDirectory, for the new name  of the file. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// A 64-bit unsigned integer that contains the file handle  for the root directory. For network operations, this  
    ///  value MUST always be zero. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileRenameInformation_SMB2_RootDirectory_Values : ulong
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        V1 = 0,
    }

    #endregion

    #region 2.4.35   FileReparsePointInformation

    /// <summary>
    /// This information class is used to query for information  on a reparse point.  The  
    /// FILE_REPARSE_POINT_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\852688a6-925d-46e8-ab0a-79286175c8c0.xml
    //  </remarks>
    public partial struct FileReparsePointInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the file reference  number for the file. NTFS generates this number  
        /// and  assigns it to the file automatically when the file  is created. The value of this field MUST be  
        /// greater  than or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long FileReferenceNumber;

        /// <summary>
        /// A 32-bit unsigned integer value containing the reparse  point tag that uniquely identifies the owner of  
        /// the  reparse point. Section  contains more details on reparse  tags. 
        /// </summary>
        public uint Tag;
    }

    #endregion

    #region 2.4.36   FileSfioReserveInformation

    /// <summary>
    /// This information class is used to query or set reserved  bandwidth for a file handle. Conceptually reserving   
    /// bandwidth is effectively specifying the bytes per second  to allocate to file IO.  The  
    /// FILE_SFIO_RESERVE_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\57af15b2-470f-44aa-83d9-7f3765368a5d.xml
    //  </remarks>
    public partial struct FileSfioReserveInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of I/O  requests that must complete per period of time, as 
        ///   specified in the Period field. When setting bandwidth  reservation, a value of 0 indicates to the file  
        /// system  that it MUST free any existing reserved bandwidth. 
        /// </summary>
        public uint RequestsPerPeriod;

        /// <summary>
        /// A 32-bit unsigned integer that contains the period for  reservation, which is the time from which I/O is  
        /// issued  to the kernel until the time the I/O should be completed,  specified in milliseconds. 
        /// </summary>
        public uint Period;

        /// <summary>
        /// An unsigned character (Boolean) value. 
        /// </summary>
        public byte RetryFailures;

        /// <summary>
        /// An unsigned character (Boolean) value. 
        /// </summary>
        public byte Discardable;

        /// <summary>
        /// Reserved for alignment. This field can contain any value  and MUST be ignored. 
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the minimum  size of any individual I/O request that may be  
        /// issued  by an application using bandwidth reservation. When  setting reservations, this field MUST be  
        /// ignored by  servers and SHOULD be set to 0 by clients. 
        /// </summary>
        public uint RequestSize;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the number  of RequestSize I/O requests allowed to be outstanding 
        ///   at any time. When setting reservations, this field  MUST be ignored by servers and SHOULD be set to 0 by 
        ///   clients. 
        /// </summary>
        public uint NumOutstandingRequests;
    }

    #endregion

    #region 2.4.37   FileShortNameInformation

    /// <summary>
    /// This information class is used to query or change the  file's short name. A caller changing the file's short   
    /// name MUST have SeBackupPrivilege. SeBackupPrivilege  is specified in the Windows behavior note of [MS-LSAD]   
    /// section . The FILE_NAME_INFORMATION data element is  as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\80cecad8-9172-4c42-af90-f890a84f2abc.xml
    //  </remarks>
    public partial struct FileShortNameInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the FileName field. 
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file  name. This field MUST NOT begin with a path  
        /// separator  character (backslash). This field might not be NULL-terminated,  and MUST be handled as a  
        /// sequence of FileNameLength  bytes. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    #endregion

    #region 2.4.38   FileStandardInformation

    /// <summary>
    /// This information class is used to query or set file  information.  The FILE_STANDARD_INFORMATION data element  
    ///  is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\5afa7f66-619c-48f3-955f-68c4ece704ae.xml
    //  </remarks>
    public partial struct FileStandardInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the file allocation  size in bytes. Usually, this value is a  
        /// multiple of  the sector or cluster size of the underlying physical  device. The value of this field MUST  
        /// be greater than  or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long AllocationSize;

        /// <summary>
        /// A 64-bit signed integer that contains the absolute new  end-of-file position as a byte offset from the  
        /// start  of the file. EndOfFile specifies the offset to the  byte immediately following the last valid byte  
        /// in the  file. Because this value is zero-based, it actually  refers to the first free byte in the file.  
        /// That is,  it is the offset from the beginning of the file at  which new bytes appended to the file will be 
        ///  written.  The value of this field MUST be greater than or equal  to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long EndOfFile;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  non-deleted links to this file. 
        /// </summary>
        public uint NumberOfLinks;

        /// <summary>
        /// An 8-bit field that MUST be set to 1 to indicate that  a file deletion has been requested; otherwise, 0. 
        /// </summary>
        public byte DeletePending;

        /// <summary>
        /// An 8-bit field that MUST be set to 1 to indicate that  the file is a directory; otherwise, 0. 
        /// </summary>
        public byte Directory;

        /// <summary>
        /// A 16-bit field. This field is reserved. This field can  be set to any value, and MUST be ignored. 
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] Reserved;
    }

    #endregion

    #region 2.4.39   FileStandardLinkInformation

    /// <summary>
    /// This information class is used to query file link information.  This information class is supported on  
    /// windows_7 and  windows_server_7. The FILE_STANDARD_LINK_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\a459c580-db65-4e49-bbb5-562268ae271a.xml
    //  </remarks>
    public partial struct FileStandardLinkInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  non-deleted links to this file. 
        /// </summary>
        public uint NumberOfAccessibleLinks;

        /// <summary>
        /// A 32-bit unsigned integer that contains the total number  of links to this file, including links marked  
        /// for delete. 
        /// </summary>
        public uint TotalNumberOfLinks;

        /// <summary>
        /// An 8-bit field that MUST be set to 1 to indicate that  a file deletion has been requested; otherwise, 0. 
        /// </summary>
        public byte DeletePending;

        /// <summary>
        /// An 8-bit field that MUST be set to 1 to indicate that  the file is a directory; otherwise, 0. 
        /// </summary>
        public byte Directory;

        /// <summary>
        /// A 16-bit field. This field is reserved. This field can  be set to any value and MUST be ignored. 
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] Reserved;
    }

    #endregion

    #region 2.4.40   FileStreamInformation

    /// <summary>
    /// This information class is used to enumerate the data  streams for a file.  A buffer can contain multiple   
    /// FILE_STREAM_INFORMATION  data elements. A FILE_STREAM_INFORMATION  data element MUST start on a 64-bit aligned 
    ///  offset  within the buffer. Any portions of a buffer that are  not within a FILE_STREAM_INFORMATION data  
    /// element are  reserved for alignment, can contain any value, and  MUST be ignored. The FILE_STREAM_INFORMATION  
    /// data element  is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\f8762be6-3ab9-411e-a7d6-5cc68f70c78d.xml
    //  </remarks>
    public partial struct FileStreamInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset  from the beginning of this entry, at which the  
        /// next  FILE_ STREAM _INFORMATION entry is located, if multiple  entries are present in a buffer. This  
        /// member is zero  if no other entries follow this one. An implementation  MUST use this value to determine  
        /// the location of the  next entry (if multiple entries are present in a buffer),  and MUST NOT assume that  
        /// the value of NextEntryOffset  is the same as the size of the current entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the stream name string. 
        /// </summary>
        public uint StreamNameLength;

        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes,  of the stream. The value of this field MUST be  
        /// greater  than or equal to 0. 
        /// </summary>
        public ulong StreamSize;

        /// <summary>
        /// A 64-bit signed integer that contains the file stream  allocation size in bytes. Usually, this value is a  
        ///  multiple of the sector or cluster size of the underlying  physical device. The value of this field MUST 
        /// be  greater  than or equal to 0. 
        /// </summary>
        public long StreamAllocationSize;

        /// <summary>
        /// A sequence of Unicode characters containing the name  of the stream using the form :streamname:$DATA, or   
        /// ::$DATA for the default stream, as specified in section  . The :$DATA string that follows streamname is an 
        ///  internal  data type tag that is unintentionally exposed via this  info class.   The leading ‘:’ and  
        /// trailing ‘:$DATA’  characters are not part of the stream name and MUST  be stripped from this field to  
        /// derive the actual stream  name. A resulting empty string for the stream name  denotes the default stream.  
        /// This field might not be  NULL-terminated, and MUST be handled as a sequence  of StreamNameLength bytes. 
        /// </summary>
        [Size("StreamNameLength")]
        public byte[] StreamName;
    }

    #endregion

    #region 2.4.41   FileValidDataLengthInformation

    /// <summary>
    /// This information class is used to set the valid data  length information for a file.  The  
    /// FILE_VALID_DATA_LENGTH_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\5c9f9d50-f0e0-40b1-9b84-0b78f59158b1.xml
    //  </remarks>
    public partial struct FileValidDataLengthInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the new valid  data length for the file. This parameter MUST be a   
        /// positive value that is greater than the current valid  data length, but less than or equal to the current  
        ///  file size. The FILE_VALID_DATA_LENGTH_INFORMATION structure  is used to set a new valid data length for a 
        ///  file on  an NTFSvolume. A file's valid data length is the length  in bytes of the data that has been  
        /// written to the file.  This valid data extends from the beginning of the file  to the last byte in the file 
        ///  that has not been zeroed  or left uninitialized. 
        /// </summary>
        public long ValidDataLength;
    }

    #endregion

    #region 2.4.41   FileIdInformation
    /// <summary>
    /// This information class is used to query the volume serial number and fileid information for a file.
    /// </summary>
    public partial struct FileIdInformation
    {
        /// <summary>
        /// A 64-bit unsigned integer that contains the serial number of the volume where the file is located.
        /// </summary>
        public long VolumeSerialNumber;

        /// <summary>
        /// An opaque 128-bit signed integer that is an identifier of the file.
        /// </summary>
        public Guid FileId;
    }

    #endregion

    #endregion

    #endregion

    #region 2.5   File System Information Classes

    #region FsInformationCommand (File system information class, Level)

    /// <summary>
    /// File system information classes are numerical values (specified by the Level column in the following table)
    /// that specify what information on a particular instance of a file system on a volume is to be queried. File
    /// system information classes can retrieve information such as the file system type, volume label, size of the
    /// file system, and name of the driver used to access the file system. The table indicates which file system
    /// information classes are supported for query and set operations.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsInformationCommand : ushort
    {
        /// <summary>
        /// This information class is used to query information on a volume on which a file system is mounted. The
        /// message contains a FILE_FS_VOLUME_INFORMATION data element.
        /// </summary>
        FileFsVolumeInformation = 1,

        /// <summary>
        /// This information class is used to set the label for a file system volume. The message contains a
        /// FILE_FS_LABEL_INFORMATION data element.
        /// </summary>
        FileFsLabelInformation = 2,

        /// <summary>
        /// This information class is used to query sector size information for a file system volume. The message 
        /// contains a FILE_FS_SIZE_INFORMATION data element.
        /// </summary>
        FileFsSizeInformation = 3,

        /// <summary>
        /// This information class is used to query device information associated with a file system volume. The
        /// message contains a FILE_FS_DEVICE_INFORMATION data element.
        /// </summary>
        FileFsDeviceInformation = 4,

        /// <summary>
        /// This information class is used to query attribute information for a file system. The message contains a
        /// FILE_FS_ATTRIBUTE_INFORMATION data element.
        /// </summary>
        FileFsAttributeInformation = 5,

        /// <summary>
        /// This information class is used to query or set quota and content indexing control information for a file
        /// system volume. The message contains a FILE_FS_CONTROL_INFORMATION data element.
        /// </summary>
        FileFsControlInformation = 6,

        /// <summary>
        /// This information class is used to query sector size information for a file system volume. The message
        /// contains a FILE_FS_FULL_SIZE_INFORMATION data element.
        /// </summary>
        FileFsFullSizeInformation = 7,

        /// <summary>
        /// This information class is used to query or set the object ID for a file system data element. The operation
        /// MUST fail if the file system does not support object IDs. The message contains a 
        /// FILE_FS_OBJECTID_INFORMATION data element.
        /// </summary>
        FileFsObjectIdInformation = 8,

        /// <summary>
        /// This information class is used to query if a given driver is in the I/O path for a file system volume. The
        /// message contains a FILE_FS_DRIVER_PATH_INFORMATION data element.
        /// </summary>
        FileFsDriverPathInformation = 9,

        /// <summary>
        /// This file system information class is not implemented by any Windows file systems; the server will fail it
        /// with status STATUS_NOT_SUPPORTED.
        /// </summary>
        FileFsVolumeFlagsInformation = 10,

        /// <summary>
        /// This information class is used to query for the extended sector size and alignment information for a volume. 
        /// The message contains a FILE_FS_SECTOR_SIZE_INFORMATION data element.
        /// </summary>
        FileFsSectorSizeInformation = 11
    }

    #endregion

    #region File System Information Structures

    #region 2.5.1   FileFsAttributeInformation

    /// <summary>
    /// This information class is used to query attribute information  for a file system. The message contains a  
    /// FILE_FS_ATTRIBUTE_INFORMATION  data element.  The FILE_FS_ATTRIBUTE_INFORMATION data  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\ebc7e6e5-4650-4e54-b17c-cf60f6fbeeaa.xml
    //  </remarks>
    public partial struct FileFsAttributeInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains a bitmask of  flags that specify attributes of the specified file  
        ///  system as a combination of the following flags. The  value of this field MUST be a bitwise OR of zero or  
        ///  more of the following with the exception that FS_FILE_COMPRESSION  and FS_VOL_IS_COMPRESSED cannot both 
        /// be  set. Any flag  values not explicitly mentioned here can be set to  any value, and MUST be ignored. 
        /// </summary>
        public FileSystemAttributes_Values FileSystemAttributes;

        /// <summary>
        /// A 32-bit signed integer that contains the maximum file  name component length, in bytes, supported by the  
        /// specified  file system. The value of this field MUST be greater  than 0. 
        /// </summary>
        [PossibleValueRange("0", "2147483647")]
        public int MaximumComponentNameLength;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, of the file system name in the  
        /// FileSystemName  field. The value of this field MUST be greater than  0. 
        /// </summary>
        [PossibleValueRange("0", "4294967295")]
        public uint FileSystemNameLength;

        /// <summary>
        /// A variable-length Unicode field containing the name  of the file system. This field might not be  
        /// NULL-terminated,  and MUST be handled as a sequence of FileSystemNameLength  bytes. This field is intended 
        ///  to be informative only.  A client SHOULD NOT infer file system type specific  behavior from this field.   
        /// Valid values for this field  depend on the version of windows that the server is  running. For Windows 7,  
        /// valid values are: FAT,FAT16,FAT32,exFAT,NTFS,CDFS,UDF.   For windows_vista_sp1 and windows_server_2008,  
        /// valid  values are: FAT,FAT16,FAT32,exFAT,NTFS,CDFS,and UDF.  For windows_vista RTM, valid values are:  
        /// FAT,FAT16,FAT32,NTFS,CDFS,and  UDF. For windows_xp, valid values are: FAT,FAT16,FAT32,NTFS,and  CDFS. 
        /// </summary>
        [Size("FileSystemNameLength")]
        public byte[] FileSystemName;
    }

    #endregion

    #region 2.5.2   FileFsControlInformation

    /// <summary>
    /// This information class is used to query or set quota  and content indexing control information for a file   
    /// system volume. The message contains a FILE_FS_CONTROL_INFORMATION  data element.  Setting quota information  
    /// requires the  caller to have permission to open a volume handle or  a handle to the quota index file for write 
    ///  access.  The FILE_FS_CONTROL_INFORMATION data element is as  follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\e5a70738-7ee4-46d9-a5f7-6644daa49a51.xml
    //  </remarks>
    public partial struct FileFsControlInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the minimum amount  of free disk space in bytes that is required for 
        ///  the  operating system's content indexing service to begin  document filtering. This value SHOULD be set 
        /// to  0,  and MUST be ignored.windows sets this value to 0. 
        /// </summary>
        public long FreeSpaceStartFiltering;

        /// <summary>
        /// A 64-bit signed integer that contains the minimum amount  of free disk space in bytes that is required for 
        ///  the  indexing service to continue to filter documents and  merge word lists. This value SHOULD be set to  
        /// 0, and  MUST be ignored.windows sets this value to 0. 
        /// </summary>
        public long FreeSpaceThreshold;

        /// <summary>
        /// A 64-bit signed integer that contains the minimum amount  of free disk space in bytes that is required for 
        ///  the  content indexing service to continue filtering. This  value SHOULD be set to 0, and MUST be  
        /// ignored.windows  sets this value to 0. 
        /// </summary>
        public long FreeSpaceStopFiltering;

        /// <summary>
        /// A 64-bit signed integer that contains the default per-user  disk quota warning threshold in bytes for the  
        /// volume.  This field MUST be set to a 64-bit integer value greater  than or equal to 0 to set a default  
        /// quota warning threshold  per user for this volume, or to (-1) to specify that  no default quota warning  
        /// threshold per user is set. 
        /// </summary>
        public ulong DefaultQuotaThreshold;

        /// <summary>
        /// A 64-bit signed integer that contains the default per-user  disk quota limit in bytes for the volume. This 
        ///  field  MUST be set to a 64-bit integer value greater than  or equal to 0 to set a default disk quota 
        /// limit  per  user for this volume, or to (-1) to specify that no  default quota limit per user is set. 
        /// </summary>
        public ulong DefaultQuotaLimit;

        /// <summary>
        /// A 32-bit unsigned integer that contains a bitmask of  flags that control quota enforcement and logging of  
        ///  user-related quota events on the volume. The following  bit flags are valid in any combination. Bits not  
        /// defined  in the following table SHOULD be set to 0, and MUST  be ignored.windows sets flags not defined  
        /// below to  zero. Logging makes an entry in the windows application  event log. 
        /// </summary>
        public FileSystemControlFlags_Values FileSystemControlFlags;

        /// <summary>
        /// This field SHOULD be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public uint Padding;
    }

    #endregion

    #region 2.5.3   FileFsDriverPathInformation

    /// <summary>
    /// This information class is used to query if a given driver  is in the I/O path for a file system volume. The  
    /// message  contains a FILE_FS_DRIVER_PATH_INFORMATION data element.   The FILE_FS_DRIVER_PATH_INFORMATION data  
    /// element is  as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\c530e769-4b87-44f4-a188-b9af8305eee0.xml
    //  </remarks>
    public partial struct FileFsDriverPathInformation
    {

        /// <summary>
        /// An unsigned character (Boolean) value that is TRUE if  the driver is in the I/O path for the file system  
        /// volume;  and otherwise, FALSE. 
        /// </summary>
        public byte DriverInPath;

        /// <summary>
        /// Reserved for alignment. This field can contain any value  and MUST be ignored. 
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length of  the DriverName string. 
        /// </summary>
        public uint DriverNameLength;

        /// <summary>
        /// A variable-length Unicode field containing the name  of the driver for which to query. This sequence of   
        /// Unicode characters MUST NOT be NULL-terminated. 
        /// </summary>
        [Size("DriverNameLength")]
        public byte[] DriverName;
    }

    #endregion

    #region 2.5.4   FileFsFullSizeInformation

    /// <summary>
    /// This information class is used to query sector size  information for a file system volume. The message  
    /// contains  a FILE_FS_FULL_SIZE_INFORMATION data element.  The  FILE_FS_FULL_SIZE_INFORMATION data element is as 
    ///  follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\63768db7-9012-4209-8cca-00781e7322f5.xml
    //  </remarks>
    public partial struct FileFsFullSizeInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the total number  of allocation units on the volume that are  
        /// available  to the user associated with the calling thread. The  value of this field MUST be greater than  
        /// or equal to  0. In windows_2000, windows_xp, windows_server_2003,  windows_vista, and windows_server_2008, 
        ///  if per-user  quotas are in use, this value may be less than the  total number of allocation units on the  
        /// disk. Non-Microsoft  quota management software might display the same behavior  as these versions of  
        /// windows if that software was implemented  as a file system filter driver, and the driver implementer   
        /// opted to set the FileFsFullSizeInformation in the same  manner as windows_2000. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long TotalAllocationUnits;

        /// <summary>
        /// A 64-bit signed integer that contains the total number  of free allocation units on the volume that are  
        /// available  to the user associated with the calling thread. The  value of this field MUST be greater than  
        /// or equal to  0. In windows_2000, windows_xp, windows_server_2003,  windows_vista, and windows_server_2008, 
        ///  if per-user  quotas are in use, this value may be less than the  total number of free allocation units on 
        ///  the disk. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long CallerAvailableAllocationUnits;

        /// <summary>
        /// A 64-bit signed integer that contains the total number  of free allocation units on the volume. The value  
        /// of  this field MUST be greater than or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long ActualAvailableAllocationUnits;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  sectors in each allocation unit. 
        /// </summary>
        public uint SectorsPerAllocationUnit;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in each sector. 
        /// </summary>
        public uint BytesPerSector;
    }

    #endregion

    #region 2.5.5   FileFsLabelInformation

    /// <summary>
    /// This information class is used to set the label for  a file system volume. The message contains a  
    /// FILE_FS_LABEL_INFORMATION  data element.  The FILE_FS_LABEL_INFORMATION data element  is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\d5ab2652-a7fd-4655-9b35-ea02bd05c0d8.xml
    //  </remarks>
    public partial struct FileFsLabelInformation
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, including the trailing NULL, if present,   
        /// of the name for the volume. 
        /// </summary>
        public uint VolumeLabelLength;

        /// <summary>
        /// A variable-length Unicode field containing the name  of the volume. The content of this field can be a  
        /// NULL-terminated  string, or it can be a string padded with the space  character to be VolumeLabelLength  
        /// bytes long. 
        /// </summary>
        [Size("VolumeLabelLength")]
        public byte[] VolumeLabel;
    }

    #endregion

    #region 2.5.6   FileFsObjectIdInformation

    /// <summary>
    /// This information class is used to query or set the object  ID for a file system data element. The operation  
    /// MUST  fail if the file system does not support object IDs.   The Microsoft FAT file system does not support  
    /// the  use of object IDs, and returns a status code of STATUS_INVALID_DEVICE_REQUEST.   The message contains a  
    /// FILE_FS_OBJECTID_INFORMATION  data element.  The FILE_FS_OBJECTID_INFORMATION data  element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\dbf535ae-315a-4508-8bc5-84276ea106d4.xml
    //  </remarks>
    public partial struct FileFsObjectIdInformation
    {

        /// <summary>
        /// A 16-byte GUID that identifies the file system volume  on the disk. This value is not required to be  
        /// unique  on the system. 
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        /// A 48-byte value containing extended information on the  file system volume. If no extended information has 
        ///   been written for this file system volume, the server  MUST return 48 bytes of 0x00 in this field. Windows 
        ///   does not write information into the ExtendedInfo field  for file systems. 
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    #endregion

    #region 2.5.7   FileFsSectorSizeInformation

    /// <summary>
    /// The Flags for FileFsSectorSizeInformation
    /// </summary>
    public enum FILE_FS_SECTOR_SIZE_INFORMATION_FLAGS : uint
    {
        /// <summary>
        /// When set, this flag indicates that the first physical sector of the device is misaligned with the first logical sector.
        /// </summary>
        SSINFO_FLAGS_ALIGNED_DEVICE = 0x00000001,

        /// <summary>
        /// When set, this flag indicates that the partition is aligned to the first physical sector of the device.
        /// </summary>
        SSINFO_FLAGS_PARTITION_ALIGNED_ON_DEVICE = 0x00000002,

        /// <summary>
        /// When set, the device reports that it does not incur a seek penalty 
        /// (this typically indicates that the device does not have rotating media, such as with flash-based disks). 
        /// </summary>
        SSINFO_FLAGS_NO_SEEK_PENALTY = 0x00000004,

        /// <summary>
        /// When set, the device supports TRIM operations, either T13 (ATA) TRIM or T10 (SCSI/SAS) UNMAP.
        /// </summary>
        SSINFO_FLAGS_TRIM_ENABLED = 0x00000008,
    }

    public partial struct FILE_FS_SECTOR_SIZE_INFORMATION
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the number of bytes in a logical sector for the device backing the volume. 
        /// This field is the unit of logical addressing for the device and is not the unit of atomic write. 
        /// Applications SHOULD NOT utilize this value for operations requiring physical sector alignment.
        /// </summary>
        public uint LogicalBytesPerSector;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of bytes in a physical sector for the device backing the volume. 
        /// Note that this is the reported physical sector size of the device and is the unit of atomic write. 
        /// Applications SHOULD utilize this value for operations requiring sector alignment.
        /// </summary>
        public uint PhysicalBytesPerSectorForAtomicity;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of bytes in a physical sector for the device backing the volume. 
        /// This is the reported physical sector size of the device and is the unit of performance. 
        /// Applications SHOULD utilize this value for operations requiring sector alignment.
        /// </summary>
        public uint PhysicalBytesPerSectorForPerformance;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the unit for which the file system on the volume 
        ///  will use for internal operations that require alignment and atomicity, in bytes.
        /// </summary>
        public uint FileSystemEffectivePhysicalBytesPerSectorForAtomicity;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the flags for this operation. 
        /// </summary>
        public FILE_FS_SECTOR_SIZE_INFORMATION_FLAGS Flags;

        /// <summary>
        /// A 32-bit unsigned integer that contains the logical sector offset within the first physical sector where the first logical sector is placed, in bytes. 
        /// If this value is set to SSINFO_OFFSET_UNKNOWN (0xffffffff), there was insufficient information to compute this field.
        /// </summary>
        public uint BytesOffsetForSectorAlignment;

        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset from the first physical sector where the first partition is placed. 
        /// If this value is set to SSINFO_OFFSET_UNKNOWN (0xffffffff), there was either insufficient information or an error was encountered in computing this field.
        /// </summary>
        public uint BytesOffsetForPartitionAlignment;
    }

    #endregion

    #region 2.5.8   FileFsSizeInformation

    /// <summary>
    /// This information class is used to query sector size  information for a file system volume. The message  
    /// contains  a FILE_FS_SIZE_INFORMATION data element.  The FILE_FS_SIZE_INFORMATION  data element is as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\e13e068c-e3a7-4dd4-94fd-3892b492e6e7.xml
    //  </remarks>
    public partial struct FileFsSizeInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the total number  of allocation units on the volume that are  
        /// available  to the user associated with the calling thread. This  value MUST be greater than or equal to 0. 
        ///  In windows_2000,  windows_xp, windows_server_2003, windows_vista, and  windows_server_2008, if per-user  
        /// quotas are in use,  this value may be less than the total number of allocation  units on the disk.  
        /// Non-Microsoft quota management software  might display the same behavior as windows_2000 if  that software 
        ///  was implemented as a file system filter  driver, and the driver implementer opted to set the   
        /// FileFsSizeInformation in the same manner as windows_2000. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long TotalAllocationUnits;

        /// <summary>
        /// A 64-bit signed integer that contains the total number  of free allocation units on the volume that are  
        /// available  to the user associated with the calling thread. This  value MUST be greater than or equal to 0. 
        ///  In windows_2000,  windows_xp, windows_server_2003, windows_vista, and  windows_server_2008, if per-user  
        /// quotas are in use,  this value may be less than the total number of free  allocation units on the disk. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long ActualAvailableAllocationUnits;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  sectors in each allocation unit. 
        /// </summary>
        public uint SectorsPerAllocationUnit;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of  bytes in each sector. 
        /// </summary>
        public uint BytesPerSector;
    }

    #endregion

    #region 2.5.9   FileFsVolumeInformation

    /// <summary>
    /// This information class is used to query information  on a volume on which a file system is mounted. The   
    /// message contains a FILE_FS_VOLUME_INFORMATION data  element.  The FILE_FS_VOLUME_INFORMATION data element  is  
    /// as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\bf691378-c34e-4a13-976e-404ea1a87738.xml
    //  </remarks>
    public partial struct FileFsVolumeInformation
    {

        /// <summary>
        /// A 64-bit signed integer that contains the time when  the volume was created in the format of a FILETIME   
        /// structure. The value of this field MUST be greater  than or equal to 0. 
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long VolumeCreationTime;

        /// <summary>
        /// A 32-bit unsigned integer that contains the serial number  of the volume. The serial number is an opaque  
        /// value  generated by the file system at format time, and is  not necessarily related to any hardware serial 
        ///  number  for the device on which the file system is located.  No specific format or content of this field  
        /// is required  for protocol interoperation. This value is not required  to be unique. 
        /// </summary>
        public uint VolumeSerialNumber;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length,  in bytes, including the trailing NULL, if present,   
        /// of the name of the volume. 
        /// </summary>
        public uint VolumeLabelLength;

        /// <summary>
        /// A 1-byte Boolean (unsigned char) that is TRUE (0x01)  if the file system supports object-oriented file  
        /// system  objects; otherwise, FALSE (0x00). This value is TRUE  for NTFS and FALSE for other file systems  
        /// implemented  by windows. 
        /// </summary>
        public SupportsObjects_Values SupportsObjects;

        /// <summary>
        /// MUST be ignored by the receiver. 
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// A variable-length Unicode field containing the name  of the volume. The content of this field can be a  
        /// NULL-terminated  string or can be a string padded with the space character  to be VolumeLabelLength bytes  
        /// long. 
        /// </summary>
        [Size("VolumeLabelLength")]
        public byte[] VolumeLabel;
    }

    #endregion

    #region 2.5.10   FileFsDeviceInformation

    /// <summary>
    /// This information class is used to query device information  associated with a file system volume. The message  
    /// contains  a FILE_FS_DEVICE_INFORMATION data element.  The FILE_FS_DEVICE_INFORMATION  data element is as  
    /// follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\616b66d5-b335-4e1c-8f87-b4a55e8d3e4a.xml
    //  </remarks>
    public partial struct FileFsDeviceInformation
    {

        /// <summary>
        /// This identifies the type of given volume. It MUST be  one of the following. 
        /// </summary>
        public DeviceType_Values DeviceType;

        /// <summary>
        /// A bit field which identifies various characteristics  about a given volume. The following are valid bit  
        /// values. 
        /// </summary>
        public Characteristics_Values Characteristics;
    }

    #endregion

    #endregion

    #endregion

    #region FSCC NTStatus Codes

    /// <summary>
    /// Status of FsctlCreateOrGetObjectIdReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlCreateOrGetObjectIdReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The file has no object ID yet, and the file system is unable to generate a unique (to this volume) ID. 
        /// </summary>
        STATUS_DUPLICATE_NAME = 0xC00000BD,

        /// <summary>
        /// The handle is not to a file or directory, or the output buffer is not large enough to contain a 
        /// FILE_OBJECTID_BUFFER structure. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D
    }

    /// <summary>
    /// Status of FsctlDeleteObjectIdReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlDeleteObjectIdReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle was not opened with restore access or write access. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// The file or directory has no object ID. This status is not returned on a healthy volume but can be 
        /// returned if the volume is corrupt. 
        /// </summary>
        STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034,

        /// <summary>
        /// The volume is write-protected and changes to it cannot be made. 
        /// </summary>
        STATUS_MEDIA_WRITE_PROTECTED = 0xC00000A2
    }

    /// <summary>
    /// Status of FsctlDeleteReparsePointReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlDeleteReparsePointReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// A nonzero value was passed for the output buffer's length, or the handle is not to a file or directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The handle was not opened to write file data or file attributes. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// The input buffer's length is neither the size of a REPARSE_DATA_BUFFER nor a REPARSE_GUID_DATA_BUFFER; or 
        /// the reparse data length is nonzero; or the reparse tag is a non-Microsoft reparse tag, and the length is 
        /// other than the size of REPARSE_GUID_DATA_BUFFER. 
        /// </summary>
        STATUS_IO_REPARSE_DATA_INVALID = 0xC0000278,

        /// <summary>
        /// The specified reparse tag with a value of 0 or 1 is reserved for use by the system and cannot be deleted. 
        /// </summary>
        STATUS_IO_REPARSE_TAG_INVALID = 0xC0000276,

        /// <summary>
        /// The file or directory does not have a reparse point. 
        /// </summary>
        STATUS_NOT_A_REPARSE_POINT = 0xC0000275,

        /// <summary>
        /// The file or directory has a reparse point but not one with the reparse tag that was specified in this 
        /// call. 
        /// </summary>
        STATUS_IO_REPARSE_TAG_MISMATCH = 0xC0000277,

        /// <summary>
        /// The file or directory has a non-Microsoft tag, and the Reparse GUID provided does not match the one in the 
        /// reparse point for this file or directory. 
        /// </summary>
        STATUS_REPARSE_ATTRIBUTE_CONFLICT = 0xC00002B2
    }

    /// <summary>
    /// Status of FsctlFilesystemGetStatisticsReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlFilesystemGetStatisticsReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The output buffer is too small to contain a FILESYSTEM_STATISTICS structure. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// The output buffer was filled before all the statistics data could be returned. 
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005
    }

    /// <summary>
    /// Status of FsctlFindFilesBySidReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlFindFilesBySidReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle specified is not the handle to a directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The input buffer is less than the size of a long integer (4 bytes) plus the length of the SID provided, or 
        /// the input or output buffer is not aligned to a 4-byte boundary, or the output buffer is not large enough 
        /// to hold the file name length and file name that is returned by this message, or the restart value is 
        /// greater than 1. 
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8
    }

    /// <summary>
    /// Status of FsctlGetCompressionReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlGetCompressionReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The output buffer length is less than 2, or the handle is not to a file or directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The volume does not support compression. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlGetNtfsVolumeDataReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlGetNtfsVolumeDataReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle specified is not open. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The specified volume is no longer mounted. 
        /// </summary>
        STATUS_VOLUME_DISMOUNTED = 0xC000026E
    }

    /// <summary>
    /// Status of FsctlGetObjectIdReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlGetObjectIdReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The output buffer length is less than the size of a FILE_OBJECTID_BUFFER or the handle is not to a file or 
        /// directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The file or directory has no object ID. 
        /// </summary>
        STATUS_OBJECTID_NOT_FOUND = 0xC00002F0,

        /// <summary>
        /// The file system does not support the use of object IDs. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlGetReparsePointReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlGetReparsePointReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The output buffer is too small to contain a REPARSE_GUID_DATA_BUFFER. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// The handle is not to a file or directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The output buffer filled before all the reparse point data was returned. 
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005,

        /// <summary>
        /// The file or directory is not a reparse point. 
        /// </summary>
        STATUS_NOT_A_REPARSE_POINT = 0xC0000275,

        /// <summary>
        /// The file system does not support the use of reparse points. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of 2.3.20FsctlGetRetrievalPointersReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlGetRetrievalPointersReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The output buffer is too small to contain a RETRIEVAL_POINTERS_BUFFER structure. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// The input buffer is too small to contain a STARTING_VCN_INPUT_BUFFER, or the StartingVcn given is 
        /// negative, or the handle is not to a file or directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The stream is resident in the MFT and has no clusters allocated, or the starting VCN is beyond the end of 
        /// the file. 
        /// </summary>
        STATUS_END_OF_FILE = 0xC0000011,

        /// <summary>
        /// The output buffer filled before all the extents for this file were returned. 
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005
    }

    /// <summary>
    /// Status of FsctlLmrSetLinkTrackingInformationReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlLmrSetLinkTrackingInformationReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The input buffer length is 0. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D
    }

    /// <summary>
    /// Status of FsctlPipePeekReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlPipePeekReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The specified named pipe is in the disconnected state. 
        /// </summary>
        STATUS_PIPE_DISCONNECTED = 0xC00000B0,

        /// <summary>
        /// The named pipe is not in the connected state or not in the full-duplex message mode. 
        /// </summary>
        STATUS_INVALID_PIPE_STATE = 0xC00000AD,

        /// <summary>
        /// The named pipe contains unread data. 
        /// </summary>
        STATUS_PIPE_BUSY = 0xC00000AE,

        /// <summary>
        /// An exception was raised while accessing a user buffer. 
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8,

        /// <summary>
        /// There were insufficient resources to complete the operation. 
        /// </summary>
        STATUS_INSUFFICIENT_RESOURCES = 0xC000009A,

        /// <summary>
        /// The type of the handle is not a pipe. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// The data was too large for the specified buffer. This is the warning code. Response contains information 
        /// including available data length and data that fits into the buffer. 
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005
    }

    /// <summary>
    /// Status of FsctlPipeWaitReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlPipeWaitReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The specified named pipe does not exist. This error code is also returned when the pipe is closed during 
        /// wait. 
        /// </summary>
        STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034,

        /// <summary>
        /// Timeout specified in the FSCTL_PIPE_WAIT request expired. 
        /// </summary>
        STATUS_IO_TIMEOUT = 0xC00000B5,

        /// <summary>
        /// There were insufficient resources to complete the operation. 
        /// </summary>
        STATUS_INSUFFICIENT_RESOURCES = 0xC000009A,

        /// <summary>
        /// The type of the handle is not a pipe. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlPipeTransceiveReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlPipeTransceiveReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The specified named pipe is in the disconnected state. 
        /// </summary>
        STATUS_PIPE_DISCONNECTED = 0xC00000B0,

        /// <summary>
        /// The named pipe is not in the connected state or not in the full-duplex message mode. 
        /// </summary>
        STATUS_INVALID_PIPE_STATE = 0xC00000AD,

        /// <summary>
        /// The named pipe contains unread data. 
        /// </summary>
        STATUS_PIPE_BUSY = 0xC00000AE,

        /// <summary>
        /// An exception was raised while accessing a user buffer. 
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8,

        /// <summary>
        /// There were insufficient resources to complete the operation. 
        /// </summary>
        STATUS_INSUFFICIENT_RESOURCES = 0xC000009A,

        /// <summary>
        /// The type of the handle is not a pipe. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// The data was too large to fit into the specified buffer. 
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005
    }

    /// <summary>
    /// Status of FsctlQueryAllocatedRangesReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlQueryAllocatedRangesReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file, or the size of the input buffer is less than the size of a 
        /// FILE_ALLOCATED_RANGE_BUFFER structure, or the given FileOffset is less than 0, or the given Length is less 
        /// than 0, or the given FileOffset plus the given Length is larger than 0x7FFFFFFFFFFFFFFF. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The input buffer or output buffer is not aligned to a 4-byte boundary. 
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8,

        /// <summary>
        /// The output buffer is too small to contain a FILE_ALLOCATED_RANGE_BUFFER structure. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// The output buffer is too small to contain the required number of FILE_ALLOCATED_RANGE_BUFFER structures. 
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005
    }

    /// <summary>
    /// Status of FsctlQueryFatBpbReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlQueryFatBpbReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The specified request is not a valid operation for the target device. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// The buffer is too small to contain the entry. No information has been written to the buffer. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023
    }

    /// <summary>
    /// Status of FsctlQueryOnDiskVolumeInfoReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlQueryOnDiskVolumeInfoReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// An access to a user buffer failed. 
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8,

        /// <summary>
        /// The buffer is too small to contain the entry. No information has been written to the buffer. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// An invalid parameter was passed to a service or function. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D
    }

    /// <summary>
    /// Status of FsctlQuerySparingInfoReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlQuerySparingInfoReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// An invalid parameter was passed to a service or function. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D
    }

    /// <summary>
    /// Status of FsctlReadFileUsnDataReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlReadFileUsnDataReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file or directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The output buffer is not aligned to a 4-byte boundary. 
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8,

        /// <summary>
        /// The output buffer is too small to contain a USN_RECORD structure. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// The file system does not support the use of a USN change journal. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlRecallFileReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlRecallFileReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The file is set to not allow recall. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// The Remote Storage option is not installed. 
        /// </summary>
        ERROR_INVALID_FUNCTION = 0x00000001,

        /// <summary>
        /// The request is not supported. 
        /// </summary>
        STATUS_NOT_SUPPORTED = 0xC00000BB,

        /// <summary>
        /// The supplied handle is not that of a file. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlSetCompressionReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetCompressionReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The input buffer length is less than 2, or the handle is not to a file or directory, or the requested 
        /// CompressionState is not 1 or 2, or the file or directory is encrypted. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The volume does not allow compression. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// The disk is full. 
        /// </summary>
        STATUS_DISK_FULL = 0xC000007
    }

    /// <summary>
    /// Status of FsctlSetDefectManagementReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetDefectManagementReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// An invalid parameter was passed to a service or function or the handle on which this FSCTL was invoked is 
        /// that of a directory. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The specified request is not a valid operation for the target device. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// A file cannot be opened because the share access flags are incompatible. 
        /// </summary>
        STATUS_SHARING_VIOLATION = 0xC0000043,

        /// <summary>
        /// An operation was attempted to a volume after it was dismounted. 
        /// </summary>
        STATUS_VOLUME_DISMOUNTED = 0xC000026e,

        /// <summary>
        /// The volume for a file has been externally altered such that the opened file is no longer valid. 
        /// </summary>
        STATUS_FILE_INVALID = 0xC0000098,

        /// <summary>
        /// The wrong volume is in the drive. 
        /// </summary>
        STATUS_WRONG_VOLUME = 0xC0000012,

        /// <summary>
        /// The media has changed and a verify operation is in progress so no reads or writes may be performed to the 
        /// device, except those used in the verify operation. 
        /// </summary>
        STATUS_VERIFY_REQUIRED = 0x80000016
    }

    /// <summary>
    /// Status of FsctlSetEncryptionReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetEncryptionReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The disk cannot be written to because it is write-protected. 
        /// </summary>
        STATUS_MEDIA_WRITE_PROTECTED = 0xC00000A2,

        /// <summary>
        /// The EncryptionOperation field value is invalid, the open request is not for a file or directory or stream 
        /// encryption has been requested on a stream that is compressed. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// Either the input buffer is smaller than the size of the encryption buffer or the output buffer is smaller 
        /// than the size of the structure DECRYPTION_STATUS_BUFFER. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// The version of the file system on the volume does not support encryption. 
        /// </summary>
        STATUS_VOLUME_NOT_UPGRADED = 0xC000029C,

        /// <summary>
        /// The request was invalid for a system-specific reason. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// A required attribute is missing from a directory for which encryption was requested. 
        /// </summary>
        STATUS_FILE_CORRUPT_ERROR = 0xC0000102,

        /// <summary>
        /// The volume is not mounted. 
        /// </summary>
        STATUS_VOLUME_DISMOUNTED = 0xC000026E,

        /// <summary>
        /// An exception was raised while accessing a user buffer. 
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8
    }

    /// <summary>
    /// Status of FsctlSetObjectIdReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetObjectIdReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file or directory, or the input buffer's length is not equal to the size of a 
        /// FILE_OBJECTID_BUFFER structure. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The handle was not opened with restore access. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// The file or directory already has an object ID. 
        /// </summary>
        STATUS_OBJECT_NAME_COLLISION = 0xC0000035,

        /// <summary>
        /// The file system does not support the use of object IDs. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlSetObjectIdExtendedReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetObjectIdExtendedReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file or directory, or the input buffer's length is not equal to the size of an 
        /// EXTENDED_INFO structure. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The handle was not opened with write data or write attribute access. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// The file or directory has no object ID. 
        /// </summary>
        STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034,

        /// <summary>
        /// The file system does not support the use of object IDs. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlSetReparsePointReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetReparsePointReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file or directory, or the output buffer's length is greater than 0. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The input buffer length is less than the size of a REPARSE_DATA_BUFFER structure, or the input buffer 
        /// length is greater than 16,384, or a REPARSE_DATA_BUFFER structure has been specified for a non-Microsoft 
        /// reparse tag, or the GUID specified for a non-Microsoft reparse tag does not match the GUID known by the 
        /// operating system for this reparse point, or the reparse tag is 0 or 1. 
        /// </summary>
        STATUS_IO_REPARSE_DATA_INVALID = 0xC0000278,

        /// <summary>
        /// The file system does not support reparse points. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// Status of FsctlSetSparseReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetSparseReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file, or the input buffer length is nonzero and is less than the size of a 
        /// FILE_SET_SPARSE_BUFFER structure. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The handle is not open with write data or write attribute access. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022
    }

    /// <summary>
    /// Status of FsctlSetZeroDataReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetZeroDataReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file, or input buffer length is not equal to the size of a 
        /// FILE_ZERO_DATA_INFORMATION structure, or the given FileOffset is less than zero, or the given 
        /// BeyondFinalZero is less than zero, or the given FileOffset is greater than the given BeyondFinalZero. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The handle is not open with write data or write attribute access. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022
    }

    /// <summary>
    /// Status of FsctlSetZeroOnDeallocationReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSetZeroOnDeallocationReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// Zero on deallocation can only be set on a user file opened for write access and cannot be set on a 
        /// directory. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022
    }

    /// <summary>
    /// Status of FsctlSisCopyfileReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlSisCopyfileReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The input buffer is NULL, or the input buffer length is less than the size of the SI_COPYFILE structure, 
        /// or the given SourceFileNameLength or DestinationFileNameLength is less than 2 or greater than the buffer 
        /// length, or the given SourceFileNameLength plus DestinationFileNameLength is greater than the length of the 
        /// given SourceFileName plus DestinationFileName in the input buffer, or the given SourceFileName or 
        /// DestinationFileName is NULL, or the given SourceFileName or DestinationFileName is not null-terminated. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The COPYFILE_SIS_REPLACE flag was not specified, and the destination file exists. 
        /// </summary>
        STATUS_OBJECT_NAME_COLLISION = 0xC0000035,

        /// <summary>
        /// The COPYFILE_SIS_LINK flag was specified, and the source file is not under SIS control. 
        /// </summary>
        STATUS_OBJECT_TYPE_MISMATCH = 0xC0000024,

        /// <summary>
        /// The source and destination file names are not located on the same volume. 
        /// </summary>
        STATUS_NOT_SAME_DEVICE = 0xC00000D4,

        /// <summary>
        /// The single-instance storage (SIS) filter is not installed on the server. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// The source or destination file is a directory. 
        /// </summary>
        STATUS_FILE_IS_A_DIRECTORY = 0xC00000BA,

        /// <summary>
        /// The caller is not an administrator. 
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022
    }

    /// <summary>
    /// Status of FsctlWriteUsnCloseRecordReply 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlWriteUsnCloseRecordReplyStatus : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The handle is not to a file or directory, or the length of the output buffer is less than the size of a 
        /// 64-bit integer, or the output buffer does not begin on a 4-byte boundary. 
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The file system does not support the use of a USN change journal. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010
    }

    /// <summary>
    /// The reply buffer contains the first 0x24 bytes of sector 0 for the volume associated with the handle on which
    /// this FSCTL was invoked.
    /// </summary>
    public struct FSCTL_QUERY_FAT_BPB_Reply
    {
        /// <summary>
        /// The reply buffer contains the first 0x24 bytes of sector 0 for the volume associated with the handle on 
        /// which this FSCTL was invoked.
        /// </summary>
        [StaticSize(24, StaticSizeMode.Elements)]
        public byte[] Data;
    }

    #endregion

    #region Other Structures

    #region SID Structure

    /// <summary>
    /// The SID structure defines a security identifier (SID),  which is a variable-length byte array that uniquely   
    /// identifies a security principal. Each security principal  has a unique SID that is issued by a security agent. 
    ///   The agent can be a windows local system or domain.  The agent generates the SID when the security principal  
    ///  is created. The RPC marshaled version of the SID structure  is defined in section . 
    /// </summary>
    //  <remarks>
    //   ms-dtyp\78eb9013-1c3a-4970-ad1f-2b1dad588a25.xml
    //  </remarks>
    public partial struct SID
    {

        /// <summary>
        /// An 8-bit unsigned integer that specifies the revision  level of the SID structure. This value MUST be set  
        ///  to 0x01. 
        /// </summary>
        public Revision_Values Revision;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the number  of elements in the SubAuthority array. The maximum   
        /// number of elements allowed is 15. 
        /// </summary>
        public byte SubAuthorityCount;

        /// <summary>
        /// A SID_IDENTIFIER_AUTHORITY structure that contains information,  which indicates the authority under which 
        ///  the SID was  created. It describes the entity that created the SID  and manages the account. 
        /// </summary>
        public SID_IDENTIFIER_AUTHORITY IdentifierAuthority;

        /// <summary>
        /// A variable length array of unsigned 32-bit integers  that uniquely identifies a principal relative to the  
        ///  IdentifierAuthority. Its length is determined by SubAuthorityCount. 
        /// </summary>
        [Size("SubAuthorityCount")]
        public uint[] SubAuthority;
    }

    /// <summary>
    /// An 8-bit unsigned integer that specifies the revision  level of the SID structure. This value MUST be set  to  
    /// 0x01. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Revision_Values : byte
    {

        /// <summary>
        /// Possible value. 
        /// </summary>
        V1 = 0x01,
    }

    /// <summary>
    /// The SID_IDENTIFIER_AUTHORITY structure represents the  top-level authority of a security identifier (SID). 
    /// </summary>
    //  <remarks>
    //   ms-dtyp\c6ce4275-3d90-4890-ab3a-514745e4637e.xml
    //  </remarks>
    public partial struct SID_IDENTIFIER_AUTHORITY
    {

        /// <summary>
        /// Six element arrays of 8-bit unsigned integers that specify  the top-level authority of a SID, RPC_SID, and 
        ///  LSAPR_SID_INFORMATION.  The identifier authority value identifies the domain  security authority that  
        /// issued the SID. The following  identifier authorities are predefined. 
        /// </summary>
        [StaticSize(6, StaticSizeMode.Elements)]
        public byte[] Value;
    }

    #endregion

    /// <summary>
    /// The BitmapWritesUserLevel structure contains statistics  about bitmap writes resulting from certain user-level 
    ///   operations. The BitmapWritesUserLevel structure is  as follows. 
    /// </summary>
    //  <remarks>
    //   MS-fscc\0d2a2afd-fe65-43fd-9f23-bd42a362f278.xml
    //  </remarks>
    public partial struct BitmapWritesUserLevel
    {

        /// <summary>
        /// A 16-bit unsigned integer containing the number of bitmap  writes due to a write operation. 
        /// </summary>
        public ushort Write;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of bitmap  writes due to a create operation. 
        /// </summary>
        public ushort Create;

        /// <summary>
        /// A 16-bit unsigned integer containing the number of bitmap  writes due to a set file information operation. 
        /// </summary>
        public ushort SetInfo;
    }

    /// <summary>
    /// The FILETIME structure is a 64-bit value that represents  the number of 100-nanosecond intervals that have  
    /// elapsed  since January 1, 1601, in Coordinated Universal Time  (UTC) format. 
    /// </summary>
    //  <remarks>
    //   ms-dtyp\2c57429b-fdd4-488f-b5fc-9e4cf020fcdf.xml
    //  </remarks>
    public partial struct FILETIME
    {

        /// <summary>
        /// A 32-bit unsigned integer that contains the low-order  bits of the file time. 
        /// </summary>
        public uint dwLowDateTime;

        /// <summary>
        /// A 32-bit unsigned integer that contains the high-order  bits of the file time. 
        /// </summary>
        public uint dwHighDateTime;
    }

    /// <summary>
    /// The LARGE_INTEGER structure is used to represent a 64-bit  signed integer value. 
    /// </summary>
    //  <remarks>
    //   ms-dtyp\e904b1ba-f774-4203-ba1b-66485165ab1a.xml
    //  </remarks>
    public partial struct _LARGE_INTEGER
    {

        /// <summary>
        /// QuadPart member. 
        /// </summary>
        public long QuadPart;
    }

    #endregion

    #region Static Classes
    /// <summary>
    /// Six element arrays of 8-bit unsigned integers that specify  the top-level authority of a SID, RPC_SID, and  
    /// LSAPR_SID_INFORMATION.  The identifier authority value identifies the domain  security authority that issued  
    /// the SID. The following  identifier authorities are predefined. 
    /// </summary>
    public static class Value_Byte
    {
        /// <summary>
        /// Specifies the NULL SID authority. It defines only the  NULL well-known-SID: S-1-0-0. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly byte[] NULL_SID_AUTHORITY = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Specifies the World SID authority. It only defines the  Everyone well-known-SID: S-1-1-0. 
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly byte[] WORLD_SID_AUTHORITY = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

        /// <summary>
        /// Specifies the Local SID authority. It defines only the  Local well-known-SID: S-1-2-0. 
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly byte[] LOCAL_SID_AUTHORITY = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x02 };

        /// <summary>
        /// Specifies the Creator SID authority. It defines the  Creator Owner, Creator Group, and Creator Owner  
        /// Server  well-known-SIDs: S-1-3-0, S-1-3-1, and S-1-3-2. These  SIDs are used as placeholders in an access  
        /// control  list (ACL) and are replaced by the user, group, and  machine SIDs of the security principal. 
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly byte[] CREATOR_SID_AUTHORITY = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 };

        /// <summary>
        /// Not used. 
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly byte[] NON_UNIQUE_AUTHORITY = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x04 };

        /// <summary>
        /// Specifies the windows_nt security subsystem SID authority.  It defines all other SIDs in the forest. 
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly byte[] NT_AUTHORITY = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 };

        /// <summary>
        /// Specifies the Mandatory label authority. It defines  the integrity level SIDs. 
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly byte[] SECURITY_MANDATORY_LABEL_AUTHORITY = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x10 };
    }
    #endregion
}
