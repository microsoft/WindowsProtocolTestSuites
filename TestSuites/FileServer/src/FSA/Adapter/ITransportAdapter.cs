// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Smb2 = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// Communicate with NTFS implemented machine in network protocols
    /// </summary>
    public interface ITransportAdapter : IAdapter
    {
        #region Define properties

        /// <summary>
        /// Domain name
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        string Domain { set; }

        /// <summary>
        /// Server name
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        string ServerName { set; }

        /// <summary>
        /// Name of share folder
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        string ShareName { set; }

        /// <summary>
        /// User name
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        string UserName { set; }

        /// <summary>
        /// Password of the user
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        string Password { set; }

        /// <summary>
        /// Port number of the protocols
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        ushort Port { set; }

        /// <summary>
        /// IpVersion of the protocols
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        IpVersion IPVersion { set; }

        /// <summary>
        /// Maximum number of connection 
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        UInt32 MaxConnection { set; }

        /// <summary>
        /// The timeout for the client waiting for the server response.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        TimeSpan Timeout { set; }

        /// <summary>
        /// Number in bytes of the net protocols' client buffer.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        UInt32 BufferSize { set; }

        /// <summary>
        /// To specify if use pass-through information level code
        /// </summary>
        bool IsUsePassThroughInfoLevelCode { get; set; }

        /// <summary>
        /// To specify if the transport sends signed request to server.
        /// </summary>
        bool IsSendSignedRequest { get; set; }
        #endregion

        #region Initialization
        /// <summary>
        /// Initialize a tcp connection with server
        /// </summary>
        /// <param name="isWindows">The flag is true if the OS is windows</param>
        void Initialize(bool isWindows);
        #endregion

        #region 3.1.5   Higher-Layer Triggered Events

        #region 3.1.5.1   Server Requests an Open of a File
        /// <summary>
        /// Create a new file or open an existing file.
        /// </summary>
        /// <param name="fileName">The name of the file to be created or opened</param>
        /// <param name="fileAttribute">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="desiredAccess">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13.1</param>
        /// <param name="shareAccess">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createOptions">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createDisposition">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createAction">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus CreateFile(
            string fileName,
            UInt32 fileAttribute,
            UInt32 desiredAccess,
            UInt32 shareAccess,
            UInt32 createOptions,
            UInt32 createDisposition,
            out UInt32 createAction
         );
        /// <summary>
        /// Basic CreateFile method
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="fileAttribute">Desired File Attribute</param>
        /// <param name="desiredAccess">Desired Access to the file.</param>
        /// <param name="shareAccess">Share Access to the file.</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file.</param>
        /// <param name="createDisposition">The desired disposition for the open.</param>
        /// <param name="createAction">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="fileId">The fileId for the open.</param>
        /// <param name="treeId">The treeId for the open.</param>
        /// <param name="sessionId">The sessionId for the open.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus CreateFile(           
            string fileName,
            UInt32 fileAttribute,
            UInt32 desiredAccess,
            UInt32 shareAccess,
            UInt32 createOptions,
            UInt32 createDisposition,
            out UInt32 createAction,
            out Smb2.FILEID fileId,
            out uint treeId,
            out ulong sessionId
         );
        #endregion

        #region 3.1.5.2   Server Requests a Read
        /// <summary>
        /// Read the file which is opened most recently
        /// </summary>
        /// <param name="offset">The position in byte to read start</param>
        /// <param name="valueCount">The maximum value of byte number to read</param>
        /// <param name="isNonCached">If true, the write should be sent directly to the disk instead of the cache.</param>
        /// <param name="outBuffer">The output data of this control operation</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus Read(UInt64 offset, UInt32 valueCount, bool isNonCached, out byte[] outBuffer);
        #endregion

        #region 3.1.5.3   Server Requests a Write
        /// <summary>
        /// Write the file which is opened most recently
        /// </summary>
        /// <param name="buffer">Bytes to be written into the file</param>
        /// <param name="offset">The offset of the file from where client wants to start writing</param>
        /// <param name="isWriteThrough">If true, the write should be treated in a write-through fashion</param>
        /// <param name="isNonCached">If true, the write should be sent directly to the disk instead of the cache</param>
        /// <param name="bytesWritten">The number of the bytes written</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus Write(byte[] buffer, UInt64 offset, bool isWriteThrough, bool isNonCached, out UInt64 bytesWritten);
        #endregion

        #region 3.1.5.4   Server Requests Closing an Open
        /// <summary>
        /// Close the open object that was lately created
        /// </summary>
        /// <returns>NTSTATUS code</returns>
        MessageStatus CloseFile();
        #endregion

        #region 3.1.5.5   Server Requests Querying a Directory
        /// <summary>
        /// Query an existing directory with specific file name pattern.
        /// </summary>
        /// <param name="fileInformationClass">The type of information to be queried, as specified in [MS-FSCC] section 2.4</param>
        /// <param name="maxOutPutSize">The maximum number of bytes to return</param>
        /// <param name="restartScan">If true, indicate the enumeration of the directory should be restarted</param>
        /// <param name="returnSingleEntry">Indicate whether return a single entry or not</param>
        /// <param name="fileIndex">An index number from which to resume the enumeration</param>
        /// <param name="fileNamePattern">A Unicode string containing the file name pattern to match. "* ?" must be treated as wildcards</param>
        /// <param name="outBuffer">The output data of this control operation</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus QueryDirectory(
            byte fileInformationClass,
            UInt32 maxOutPutSize,
            bool restartScan,
            bool returnSingleEntry,
            UInt32 fileIndex,
            string fileNamePattern,
            out byte[] outBuffer
            );

        /// <summary>
        /// Query an existing directory with specific file name pattern.
        /// </summary>
        /// <param name="fileId">The fileId for the open.</param>
        /// <param name="treeId">The treeId for the open.</param>
        /// <param name="sessionId">The sessionId for the open.</param>
        /// <param name="fileInformationClass">The type of information to be queried, as specified in [MS-FSCC] section 2.4</param>
        /// <param name="maxOutPutSize">The maximum number of bytes to return</param>
        /// <param name="restartScan">If true, indicating the enumeration of the directory should be restarted</param>
        /// <param name="returnSingleEntry">If true, indicate return an single entry of the query</param>
        /// <param name="fileIndex">An index number from which to resume the enumeration</param>
        /// <param name="fileNamePattern">A Unicode string containing the file name pattern to match. "* ?" must be treated as wildcards</param>
        /// <param name="outBuffer">The query result</param>
        /// <returns>NTStatus code</returns>
        MessageStatus QueryDirectory(
            Smb2.FILEID fileId,
            uint treeId,
            ulong sessionId,
            byte fileInformationClass,
            UInt32 maxOutPutSize,
            bool restartScan,
            bool returnSingleEntry,
            UInt32 fileIndex,
            string fileNamePattern,
            out byte[] outBuffer
            );
        #endregion

        #region 3.1.5.6   Server Requests Flushing Cached Data
        /// <summary>
        /// Flush all persistent attributes to stable storage
        /// </summary>
        /// <returns>NTSTATUS code</returns>
        MessageStatus FlushCachedData();
        #endregion

        #region 3.1.5.7   Server Requests a Byte-Range Lock
        /// <summary>
        /// Lock range of a file
        /// </summary>
        /// <param name="offset">The start position of bytes to be locked</param>
        /// <param name="length">The bytes size to be locked</param>
        /// <param name="exclusiveLock">
        /// NONE = 0,
        /// LOCKFLAG_SHARED_LOCK = 1,
        /// LOCKFLAG_EXCLUSIVE_LOCK = 2,
        /// LOCKFLAG_UNLOCK = 4,
        /// LOCKFLAG_FAIL_IMMEDIATELY = 16,
        /// </param>
        /// <param name="failImmediately">If the range is locked, indicate whether the operation failed at once 
        /// or wait for the range to be unlocked.</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus LockByteRange(
            UInt64 offset,
            UInt64 length,
            bool exclusiveLock,
            bool failImmediately);
        #endregion

        #region 3.1.5.8   Server Requests an Unlock of a Byte-Range
        /// <summary>
        /// Unlock a range of a file
        /// </summary>
        /// <param name="offset">The start position of bytes to be unlocked</param>
        /// <param name="length">The bytes size to be unlocked</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus UnlockByteRange(UInt64 offset, UInt64 length);
        #endregion

        #region 3.1.5.9   Server Requests an FsControl Request
        /// <summary>
        /// Send control code to latest opened file 
        /// </summary>
        /// <param name="controlCode">The control code to be sent, as specified in [FSCC] section 2.3</param>
        /// <param name="maxOutBufferSize">The maximum number of byte to output</param>
        /// <param name="inBuffer">The input data of this control operation</param>
        /// <param name="outBuffer">The output data of this control operation</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus IOControl(UInt32 controlCode, UInt32 maxOutBufferSize, byte[] inBuffer, out byte[] outBuffer);
        #endregion

        #region 3.1.5.10   Server Requests Change Notifications for a Directory
        /// <summary>
        /// Send or receive file change notification to latest opened file
        /// </summary>
        /// <param name="maxOutPutSize">Specify the maximum number of byte to be returned </param>
        /// <param name="watchTree">Indicates whether the directory should be monitored recursively</param>
        /// <param name="completionFilter">Indicates the filter of the notification</param>
        /// <param name="outBuffer">Array of bytes containing the notification data</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus ChangeNotification(UInt32 maxOutPutSize, bool watchTree, UInt32 completionFilter, out byte[] outBuffer);
        #endregion

        #region 3.1.5.11   Server Requests a Query of File Information
        /// <summary>
        /// Query information of the latest opened file
        /// </summary>
        /// <param name="maxOutPutSize">Specify the maximum number of byte to be returned</param>
        /// <param name="fileInformationClass">The type of the information, as specified in [MS-FSCC] section 2.4</param>
        /// <param name="buffer">The input buffer in bytes, including the file information query parameters</param>
        /// <param name="outBuffer">Array of bytes containing the file information. The structure of these bytes is 
        /// dependent on FileInformationClass, as specified in [MS-FSCC] section 2.4
        /// <returns>NTSTATUS code</returns>
        MessageStatus QueryFileInformation(
             UInt32 maxOutPutSize,
             UInt32 fileInformationClass,
             byte[] buffer,
             out byte[] outBuffer);
        #endregion

        #region 3.1.5.12   Server Requests a Query of File System Information
        /// <summary>
        /// Query system information of the latest opened file
        /// </summary>
        /// <param name="maxOutputSize">Specify the maximum byte number to be returned</param>
        /// <param name="fsInformationClass">The type of the system information, as specified in [MS-FSCC] section 2.5</param>
        /// <param name="outBuffer">The retrieved file information in bytes.
        /// Array of bytes containing the file system information. The structure of these bytes is 
        /// base on FileSystemInformationClass, as specified in [MS-FSCC] section 2.5</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus QueryFileSystemInformation(
            UInt32 maxOutPutSize,
            UInt32 fsInformationClass,
            out byte[] outBuffer);
        #endregion

        #region 3.1.5.13   Server Requests a Query of Security Information
        /// <summary>
        /// Query security information of the latest opened file
        /// </summary>
        /// <param name="maxOutPutSize">Specify the maximum  bytes number to be returned</param>
        /// <param name="securityInformation">The type of the system information, as specified in [MS-DTYP] section 2.4.7</param>
        /// <param name="outBuffer">Array of bytes containing the file system information, as defined in [MS-DTYP] section 2.4.6</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus QuerySecurityInformation(
            UInt32 maxOutPutSize,
            UInt32 securityInformation,
            out byte[] outBuffer);
        #endregion

        #region 3.1.5.14   Server Requests Setting of File Information
        /// <summary>
        /// Apply specific information of the latest opened file
        /// </summary>
        /// <param name="fileInformationClass">Array of bytes containing the file information. The structure of these bytes is 
        /// dependent on FileInformationClass, as specified in [MS-FSCC] section 2.4
        /// </param>
        /// <param name="buffer">
        /// Array of bytes containing the file information. The structure of these bytes is 
        /// dependent on FileInformationClass, as specified in [MS-FSCC] section 2.4
        /// </param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus SetFileInformation(
            UInt32 fileInformationClass,
            byte[] buffer);

        #endregion

        #region 3.1.5.15   Server Requests Setting of File System Information
        /// <summary>
        ///  Apply specific system information of the latest opened file
        /// </summary>
        /// <param name="fsInformationClass">The type of the system information, as specified in [MS-FSCC] section 2.5</param>
        /// <param name="buffer">
        /// Array of bytes containing the file system information. The structure of these bytes is 
        /// base on FileInformationClass, as specified in [MS-FSCC] section 2.4</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus SetFileSystemInformation(
            UInt32 fsInformationClass,
            byte[] buffer); 
        #endregion

        #region 3.1.5.16   Server Requests Setting of Security Information
        /// <summary>
        /// Apply security information of the latest opened file
        /// </summary>
        /// <param name="securityInformation">A SECURITY_INFORMATION data type, as specified in [MS-DTYP] section 2.4.7</param>
        /// <param name="buffer">
        /// Array of bytes containing the file system information, as defined in [MS-DTYP] section 2.4.6</param>
        /// <returns>NTSTATUS code</returns>
        MessageStatus SetSecurityInformation(
            UInt32 securityInformation,
            byte[] buffer); 
        #endregion

        #region 3.1.5.20   Server Requests Querying Quota Information
        /// <summary>
        /// Query Quota Information
        /// </summary>
        /// <param name="maxOutputBufferSize">Specify the maximum byte number to be returned.</param>
        /// <param name="quotaInformationClass">FileInfoClass for Quota Information.</param>
        /// <param name="inputBuffer">The input buffer in bytes, includes the quota information query parameters.</param>
        /// <param name="outputBuffer">An array of one or more FILE_QUOTA_INFORMATION structures as specified in [MS-FSCC] section 2.4.33.</param>        
        /// <returns>NTSTATUS code.</returns>
        MessageStatus QueryQuotaInformation(
            UInt32 maxOutputBufferSize,
            UInt32 quotaInformationClass,
            byte[] inputBuffer,
            out byte[] outputBuffer); 
        #endregion

        #region 3.1.5.21   Server Requests Setting Quota Information
        /// <summary>
        /// Set Quota Information
        /// </summary>        
        /// <param name="quotaInformationClass">FileInfoClass for Quota Information.</param>
        /// <param name="inputBuffer">The input buffer in bytes, includes the quota information query parameters.</param>              
        /// <returns>NTSTATUS code.</returns>
        MessageStatus SetQuotaInformation(
            UInt32 quotaInformationClass,
            byte[] inputBuffer); 
        #endregion

        #endregion
    }

}