// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// Communicate with target machine using MS-SMB2 protocol
    /// </summary>
    public class Smb2TransportAdapter : ManagedAdapterBase, ITransportAdapter
    {
        #region fields

        private string domainName;
        private string serverName;
        private string shareName;
        private string userName;
        private string password;
        private uint treeId;
        private uint lockSequence;
        private ulong sessionId;
        private ulong messageId;
        private ushort portNumber;
        private byte[] gssToken;
        private byte[] sessionKey;
        private DialectRevision[] requestDialects;
        private Packet_Header packetHeader;
        private string fileName;
        private FileType fileType;
        private FILEID fileId;
        private IpVersion ipVersion;
        private Smb2Client smb2Client;
        private TimeSpan timeout;
        private FSATestConfig testConfig;
        private DialectRevision selectedDialect;

        // The following suppression is adopted because this field will be used by reflection.
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private ITestSite site;

        // The following suppression is adopted because this field will be used by reflection.
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private UInt32 bufferSize;

        // The following suppression is adopted because this field will be used by reflection.
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private UInt32 maxConnection;

        private bool isUsePassThroughInfoLevelCode;

        private bool isSendSignedRequest;

        private Packet_Header_Flags_Values packetHeaderFlag = Packet_Header_Flags_Values.FLAGS_SIGNED;
        #endregion

        #region Constructors

        public Smb2TransportAdapter(DialectRevision[] dialects, FSATestConfig testConfig)
        {
            this.requestDialects = dialects;
            this.testConfig = testConfig;
        }

        #endregion

        #region properties

        /// <summary>
        /// The timeout for the client waiting for the server response.
        /// </summary>
        public TimeSpan Timeout
        {
            set
            {
                this.timeout = value;
            }
        }

        /// <summary>
        /// Number in bytes of the client buffer.
        /// </summary>
        public UInt32 BufferSize
        {
            set
            {
                this.bufferSize = value;
            }
        }

        /// <summary>
        /// IpVersion of the connection
        /// </summary>
        public IpVersion IPVersion
        {
            set
            {
                this.ipVersion = value;
            }
        }

        /// <summary>
        /// Port number of the connection
        /// </summary>
        public ushort Port
        {
            set
            {
                this.portNumber = value;
            }
        }

        /// <summary>
        /// Domain name connected
        /// </summary>
        public string Domain
        {
            set
            {
                this.domainName = value;
            }
        }

        /// <summary>
        /// Server name connected
        /// </summary>
        public string ServerName
        {
            set
            {
                this.serverName = value;
            }
        }

        /// <summary>
        /// User name for connection
        /// </summary>
        public string UserName
        {
            set
            {
                this.userName = value;
            }
        }

        /// <summary>
        /// Password of the user to connect to the SUT
        /// </summary>
        public string Password
        {
            set
            {
                this.password = value;
            }
        }

        /// <summary>
        /// Number of maximum connection 
        /// </summary>
        public UInt32 MaxConnection
        {
            set
            {
                this.maxConnection = value;
            }
        }

        /// <summary>
        /// Share name being connecting
        /// </summary>
        public string ShareName
        {
            set
            {
                this.shareName = value;
            }
        }

        /// <summary>
        /// To specify if use pass-through information level code
        /// </summary>
        public bool IsUsePassThroughInfoLevelCode
        {
            get
            {
                return this.isUsePassThroughInfoLevelCode;
            }
            set
            {
                this.isUsePassThroughInfoLevelCode = value;
            }
        }

        /// <summary>
        /// To specify if the transport sends signed request to server.
        /// </summary>
        public bool IsSendSignedRequest
        {
            get { return this.isSendSignedRequest; }
            set
            {
                this.isSendSignedRequest = value;
                this.packetHeaderFlag = this.isSendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;
            }
        }

        internal FILEID FileId
        {
            get
            {
                return fileId;
            }
        }

        #endregion

        #region adapter Initializeation and CleanUp

        /// <summary>
        /// Initialize this adapter, will be called by PTF automatically
        /// </summary>
        /// <param name="testSite">ITestSite type object, will be set by PTF automatically</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            this.site = testSite;
            this.smb2Client = null;
        }

        /// <summary>
        /// Disconnect from share and release object
        /// </summary>
        /// <param name="disposing">bool, indicates is disposing status</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.smb2Client != null)
            {
                this.DeleteTestFile();
                this.smb2Client.Disconnect();
                this.smb2Client.Dispose();
                this.smb2Client = null;
            }
        }

        /// <summary>
        /// Initialize a tcp connection with server
        /// </summary>
        /// <param name="isWindows">the flag is true if the OS is windows</param>
        public void Initialize(bool isWindows)
        {
            //Reset variables
            this.messageId = 0;
            this.lockSequence = 0;
            this.sessionId = 0;

            //Initialize SMB2 SDK and connect to share
            this.smb2Client = new Smb2Client(timeout);

            bool isIpv4 = this.ipVersion == IpVersion.Ipv6 ? false : true;
            IPAddress serverIp = FsaUtility.GetIpAddress(this.serverName, isIpv4);
            this.site.Log.Add(LogEntryKind.Debug, "Connect to server {0} over TCP.", serverIp.ToString());
            this.smb2Client.ConnectOverTCP(serverIp);

            MessageStatus status = MessageStatus.SUCCESS;
            status = this.Negotiate();
            if (status != MessageStatus.SUCCESS)
            {
                throw new InvalidOperationException("Negotiate failed:" + status.ToString());
            }

            status = this.SessionSetup();
            if (status != MessageStatus.SUCCESS)
            {
                throw new InvalidOperationException("session setup failed:" + status.ToString());
            }

            status = this.TreeConnect();
            if (status != MessageStatus.SUCCESS)
            {
                throw new InvalidOperationException("treeconeect failed:" + status.ToString());
            }
        }

        /// <summary>
        /// Calling this method will disconnect current connection.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            if (this.smb2Client != null)
            {
                this.DeleteTestFile();
                this.smb2Client.Disconnect();
                this.smb2Client.Dispose();
                this.smb2Client = null;
            }
        }
        # endregion

        #region protected methods

        /// <summary>
        /// Negotiate method, will be called automatically when initialize
        /// </summary>
        /// <returns>NTStatus code</returns>
        public MessageStatus Negotiate()
        {
            uint status;
            Smb2NegotiateRequestPacket negotiateRequest;
            Smb2NegotiateResponsePacket negotiateResponse;
            Capabilities_Values capabilityValue = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING |
                Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING |
                Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            if (Array.IndexOf(this.requestDialects, DialectRevision.Smb30) >= 0)
            {
                capabilityValue |= Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            }

            PreauthIntegrityHashID[] preauthHashAlgs = null;
            EncryptionAlgorithm[] encryptionAlgs = null;
            if (this.requestDialects.Contains(DialectRevision.Smb311))
            {
                // initial negotiation context for SMB 3.1.1 dialect
                preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgs = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_CCM, EncryptionAlgorithm.ENCRYPTION_AES128_GCM };
            }

            status = this.smb2Client.Negotiate(
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                this.messageId++,
                this.requestDialects,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                capabilityValue,
                Guid.NewGuid(),
                out selectedDialect,
                out this.gssToken,
                out negotiateRequest,
                out negotiateResponse,
                0,
                preauthHashAlgs,
                encryptionAlgs
                );
            packetHeader = negotiateResponse.Header;
            if (testConfig.IsGlobalEncryptDataEnabled && testConfig.IsGlobalRejectUnencryptedAccessEnabled)
            {
                site.Assert.Inconclusive("Test case is not applicable when both IsGlobalEncryptDataEnabled and IsGlobalRejectUnencryptedAccessEnabled set to true.");
            }
            testConfig.CheckNegotiateContext(negotiateRequest, negotiateResponse);

            return (MessageStatus)status;
        }

        /// <summary>
        /// SessionSetup method, will be called automatically when initialize
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus SessionSetup()
        {
            uint status;
            SESSION_SETUP_Response sessionSetupResponse;

            SspiClientSecurityContext sspiClientGss =
                new SspiClientSecurityContext(
                    SecurityPackageType.Negotiate,
                    new AccountCredential(this.domainName, this.userName, this.password),
                    Smb2Utility.GetCifsServicePrincipalName(this.serverName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            sspiClientGss.Initialize(this.gssToken);
            this.sessionId = 0;

            do
            {
                status = this.smb2Client.SessionSetup(
                    1,
                    64,
                    Packet_Header_Flags_Values.NONE,
                    this.messageId++,
                    this.sessionId,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out this.gssToken,
                    out packetHeader,
                    out sessionSetupResponse);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    this.gssToken != null && this.gssToken.Length > 0)
                {
                    sspiClientGss.Initialize(this.gssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                sessionKey = sspiClientGss.SessionKey;
                this.smb2Client.GenerateCryptoKeys(sessionId, sessionKey, testConfig.SendSignedRequest, false);
                if (testConfig.IsGlobalEncryptDataEnabled && selectedDialect >= DialectRevision.Smb30 && selectedDialect != DialectRevision.Smb2Unknown)
                {
                    this.smb2Client.EnableSessionSigningAndEncryption(sessionId, testConfig.SendSignedRequest, true);
                }
            }

            return (MessageStatus)status;
        }

        /// <summary>
        /// TreeConnect, will be called automatically when initialize
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus TreeConnect()
        {

            TREE_CONNECT_Response treeConnectResponse;
            string uncSharepath = "\\\\" + this.serverName + "\\" + this.shareName;
            uint status = this.smb2Client.TreeConnect(
                    1,
                    1,
                    this.packetHeaderFlag,
                    this.messageId++,
                    this.sessionId,
                    uncSharepath,
                    out this.treeId,
                    out packetHeader,
                    out treeConnectResponse);

            return (MessageStatus)status;
        }

        /// <summary>
        /// TreeDisconnect method, will be called automatically when reset or dispose
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus TreeDisconnect()
        {
            TREE_DISCONNECT_Response treeDisconnectResponse;

            uint status = this.smb2Client.TreeDisconnect(
                1,
                64,
                this.packetHeaderFlag,
                messageId++,
                sessionId,
                treeId,
                out packetHeader,
                out treeDisconnectResponse);

            return (MessageStatus)status;
        }

        /// <summary>
        /// Logoff method, will be called automatically when reset or dispose
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus LogOff()
        {
            LOGOFF_Response logoffResponse;

            uint status = this.smb2Client.LogOff(
                1,
                64,
                this.packetHeaderFlag,
                messageId++,
                sessionId,
                out packetHeader,
                out logoffResponse);

            return (MessageStatus)status;
        }

        /// <summary>
        /// Delete test file
        /// </summary>
        protected void DeleteTestFile()
        {
            if (string.IsNullOrEmpty(this.fileName)
                || this.fileName.Contains("Existing")
                || this.fileName.Contains("Quota")
                || this.fileName.Contains("MountPoint")
                || this.fileName.Contains("link"))
            {
                //Do not remove these existing files.
            }
            else
            {
                try
                {
                    MessageStatus status;
                    status = CloseFile();

                    UInt32 oAction;
                    CreateOptions fileCreateOption = this.fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE;

                    status = CreateFile(fileName,
                    (UInt32)File_Attributes.NONE,
                    (UInt32)AccessMask.GENERIC_ALL,
                    (UInt32)(ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE),
                    (UInt32)fileCreateOption,
                    (UInt32)CreateDisposition.OPEN, out oAction);

                    FileDispositionInformation fileDispositionInfo = new FileDispositionInformation();
                    fileDispositionInfo.DeletePending = 1;
                    List<byte> byteList = new List<byte>();
                    byteList.AddRange(BitConverter.GetBytes(fileDispositionInfo.DeletePending));

                    status = SetFileInformation((uint)FileInfoClass.FILE_DISPOSITION_INFORMATION, byteList.ToArray());
                    status = CloseFile();
                }
                catch
                {
                    //Ignore failure for clean up
                }
            }
        }

        #endregion

        #region implementing ITransportAdapter methods

        #region 3.1.5.1   Server Requests an Open of a File

        /// <summary>
        /// Create a new file or open an existing file.
        /// </summary>
        /// <param name="fileName">The name of the data file or directory to be created or opened</param>
        /// <param name="fileAttribute">FileAttributes for the file.</param>
        /// <param name="desiredAccess">The level of access that is required.</param>
        /// <param name="shareAccess">Specifies the sharing mode for the open.</param>
        /// <param name="createOptions">Specifies the options to be applied when creating or opening the file. </param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is specified in the name field already exists.</param>
        /// <param name="createAction">The action taken in establishing the open. </param>
        /// <returns>NTStatus code</returns>
        public MessageStatus CreateFile(
            string fileName,
            UInt32 fileAttribute,
            UInt32 desiredAccess,
            UInt32 shareAccess,
            UInt32 createOptions,
            UInt32 createDisposition,
            out UInt32 createAction
         )
        {
            CREATE_Response createResponse;
            Smb2CreateContextResponse[] serverCreateContexts;
            this.fileName = fileName;
            this.fileType = (((CreateOptions_Values)createOptions) & CreateOptions_Values.FILE_NON_DIRECTORY_FILE) == CreateOptions_Values.FILE_NON_DIRECTORY_FILE ? FileType.DataFile : FileType.DirectoryFile;
            uint status = this.smb2Client.Create(
                1,
                64,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                fileName,
                (AccessMask)desiredAccess,
                (ShareAccess_Values)shareAccess,
                (CreateOptions_Values)createOptions,
                (CreateDisposition_Values)createDisposition,
                (File_Attributes)fileAttribute,
                ImpersonationLevel_Values.Impersonation,
                SecurityFlags_Values.NONE,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null,
                out this.fileId,
                out serverCreateContexts,
                out packetHeader,
                out createResponse);

            createAction = (UInt32)createResponse.CreateAction;
            return (MessageStatus)status;
        }

        /// <summary>
        /// Basic CreateFile method
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="fileAttribute">Desired File Attribute</param>
        /// <param name="desiredAccess">Desired Access to the file.</param>
        /// <param name="shareAccess">Share Access to the file.</param>
        /// <param name="createOptions">Specifies the options to be applied when creating or opening the file.</param>
        /// <param name="createDisposition">The desired disposition for the open.</param>
        /// <param name="createAction">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="fileId">The fileId for the open.</param>
        /// <param name="treeId">The treeId for the open.</param>
        /// <param name="sessionId">The sessionId for the open.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus CreateFile(
            string fileName,
            UInt32 fileAttribute,
            UInt32 desiredAccess,
            UInt32 shareAccess,
            UInt32 createOptions,
            UInt32 createDisposition,
            out UInt32 createAction,
            out FILEID fileId,
            out uint treeId,
            out ulong sessionId
         )
        {
            CREATE_Response createResponse;
            Smb2CreateContextResponse[] serverCreateContexts;
            this.fileName = fileName;
            this.fileType = (((CreateOptions_Values)createOptions) & CreateOptions_Values.FILE_NON_DIRECTORY_FILE) == CreateOptions_Values.FILE_NON_DIRECTORY_FILE ? FileType.DataFile : FileType.DirectoryFile;
            uint status = this.smb2Client.Create(
                1,
                64,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                fileName,
                (AccessMask)desiredAccess,
                (ShareAccess_Values)shareAccess,
                (CreateOptions_Values)createOptions,
                (CreateDisposition_Values)createDisposition,
                (File_Attributes)fileAttribute,
                ImpersonationLevel_Values.Impersonation,
                SecurityFlags_Values.NONE,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null,
                out this.fileId,
                out serverCreateContexts,
                out packetHeader,
                out createResponse);

            createAction = (UInt32)createResponse.CreateAction;

            fileId = this.fileId;
            treeId = this.treeId;
            sessionId = this.sessionId;
            return (MessageStatus)status;
        }
        #endregion

        #region 3.1.5.2   Server Requests a Read

        /// <summary>
        /// Read the file which is opened most recently
        /// </summary>
        /// <param name="offset">The position in byte to read start</param>
        /// <param name="byteCount">The maximum value of byte number to read</param>
        /// <param name="isNonCached">Indicate whether make a cached for the operation in the server.</param>
        /// <param name="outBuffer">The output data of this control operation</param>
        /// <returns>NTstatus code</returns>
        public MessageStatus Read(UInt64 offset, UInt32 byteCount, bool isNonCached, out byte[] outBuffer)
        {
            READ_Response readResponse;
            int creditCharge = 1 + (((int)byteCount - 1) / 65535);
            uint status = this.smb2Client.Read(
                (ushort)creditCharge,
                64,
                this.packetHeaderFlag,
                this.messageId,
                this.sessionId,
                this.treeId,
                byteCount,
                offset,
                this.fileId,
                0,
                Channel_Values.CHANNEL_NONE,
                0,
                new byte[0],
                out outBuffer,
                out packetHeader,
                out readResponse);

            messageId += (ulong)creditCharge;

            return (MessageStatus)status;
        }

        #endregion

        #region 3.1.5.3   Server Requests a Write

        /// <summary>
        /// Write the file which is opened most recently
        /// </summary>
        /// <param name="buffer">Bytes to be written in the file</param>
        /// <param name="offset">The offset of the file from where client wants to start writing</param>
        /// <param name="isWriteThrough">If true, the write should be treated in a write-through fashion.</param>
        /// <param name="isUnBuffered">If true, File buffering is not performed.</param>
        /// <param name="bytesWritten">The number of the bytes written</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus Write(byte[] buffer, UInt64 offset, bool isWriteThrough, bool isUnBuffered, out UInt64 bytesWritten)
        {
            WRITE_Response writeResponse;
            int creditCharge = 1 + ((buffer.Length - 1) / 65535);

            WRITE_Request_Flags_Values writeFlag = WRITE_Request_Flags_Values.None;
            if (isWriteThrough)
            {
                writeFlag |= WRITE_Request_Flags_Values.SMB2_WRITEFLAG_WRITE_THROUGH;
            }

            if (isUnBuffered)
            {
                writeFlag |= WRITE_Request_Flags_Values.SMB2_WRITEFLAG_WRITE_UNBUFFERED;
            }

            uint status = this.smb2Client.Write(
                (ushort)creditCharge,
                64,
                this.packetHeaderFlag,
                this.messageId,
                this.sessionId,
                this.treeId,
                offset,
                this.fileId,
                Channel_Values.CHANNEL_NONE,
                writeFlag,
                new byte[0],
                buffer,
                out packetHeader,
                out writeResponse);

            MessageStatus returnedStatus = (MessageStatus)status;
            messageId += (ulong)creditCharge;
            bytesWritten = (returnedStatus == MessageStatus.SUCCESS) ? writeResponse.Count : 0;

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.4   Server Requests Closing an Open

        /// <summary>
        /// Close the open object that was lately created
        /// </summary>
        /// <returns>NTStatus code</returns>
        public MessageStatus CloseFile()
        {
            CLOSE_Response closeResponse;

            uint status = this.smb2Client.Close(
                1,
                64,
                this.packetHeaderFlag,
                messageId++,
                sessionId,
                treeId,
                fileId,
                Flags_Values.NONE,
                out packetHeader,
                out closeResponse);

            return (MessageStatus)status;
        }

        #endregion

        #region 3.1.5.5   Server Requests Querying a Directory

        /// <summary>
        /// Query an existing directory with specific file name pattern.
        /// </summary>
        /// <param name="fileInformationClass">The type of information to be queried, as specified in [MS-FSCC] section 2.4</param>
        /// <param name="maxOutPutSize">The maximum number of bytes to return</param>
        /// <param name="restartScan">If true, indicating the enumeration of the directory should be restarted</param>
        /// <param name="returnSingleEntry">If true, indicate return an single entry of the query</param>
        /// <param name="fileIndex">An index number from which to resume the enumeration</param>
        /// <param name="fileNamePattern">A Unicode string containing the file name pattern to match. "* ?" must be treated as wildcards</param>
        /// <param name="outBuffer">The query result</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus QueryDirectory(
            byte fileInformationClass,
            UInt32 maxOutPutSize,
            bool restartScan,
            bool returnSingleEntry,
            uint fileIndex,
            string fileNamePattern,
            out byte[] outBuffer
            )
        {
            QUERY_DIRECTORY_Response responsePayload;
            QUERY_DIRECTORY_Request_Flags_Values restartScanFlag = 
                restartScan ? QUERY_DIRECTORY_Request_Flags_Values.RESTART_SCANS : QUERY_DIRECTORY_Request_Flags_Values.NONE;

            uint status = this.smb2Client.QueryDirectory(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                (FileInformationClass_Values)fileInformationClass,
                restartScanFlag,
                fileIndex,
                this.fileId,
                fileNamePattern,
                maxOutPutSize,
                out outBuffer,
                out packetHeader,
                out responsePayload);
            return (MessageStatus)status;
        }

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
        public MessageStatus QueryDirectory(
            FILEID fileId,
            uint treeId,
            ulong sessionId,
            byte fileInformationClass,
            UInt32 maxOutPutSize,
            bool restartScan,
            bool returnSingleEntry,
            uint fileIndex,
            string fileNamePattern,
            out byte[] outBuffer            
            )
        {
            QUERY_DIRECTORY_Response responsePayload;
            QUERY_DIRECTORY_Request_Flags_Values requestFlag =
                restartScan ? QUERY_DIRECTORY_Request_Flags_Values.RESTART_SCANS : QUERY_DIRECTORY_Request_Flags_Values.NONE;
            requestFlag |= returnSingleEntry ? QUERY_DIRECTORY_Request_Flags_Values.RETURN_SINGLE_ENTRY : QUERY_DIRECTORY_Request_Flags_Values.NONE;

            uint status = this.smb2Client.QueryDirectory(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                sessionId,
                treeId,
                (FileInformationClass_Values)fileInformationClass,
                requestFlag,
                fileIndex,
                fileId,
                fileNamePattern,
                maxOutPutSize,
                out outBuffer,
                out packetHeader,
                out responsePayload);
           
            return (MessageStatus)status;
        }

        #endregion

        #region 3.1.5.6   Server Requests Flushing Cached Data
        /// <summary>
        /// Flush all persistent attributes to stable storage
        /// </summary>
        /// <returns>NTStatus code</returns>
        public MessageStatus FlushCachedData()
        {
            FLUSH_Response responsePayload;
            uint status = this.smb2Client.Flush(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                this.fileId,
                out packetHeader,
                out responsePayload);

            return (MessageStatus)status;
        }
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
        /// <param name="failImmediately">If the range is locked. Indicate whether the operation failed at once or wait for the range been unlocked.</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus LockByteRange(
            UInt64 offset,
            UInt64 length,
            bool exclusiveLock,
            bool failImmediately)
        {
            // SMB2 can lock multi-ranges within a file. But this interface method only requires one range be locked.
            // So only need specify a one element lock_element array.  
            LOCK_ELEMENT[] elements = new LOCK_ELEMENT[1];
            elements[0].Offset = offset;
            elements[0].Length = length;
            elements[0].Flags = LOCK_ELEMENT_Flags_Values.NONE;
            if (failImmediately)
            {
                elements[0].Flags |= LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY;
            }
            if (exclusiveLock)
            {
                elements[0].Flags |= LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK;
            }
            else
            {
                elements[0].Flags |= LOCK_ELEMENT_Flags_Values.LOCKFLAG_SHARED_LOCK;
            }

            LOCK_Response responsePayload;
            uint status = this.smb2Client.Lock(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                this.lockSequence++,
                this.fileId,
                elements,
                out packetHeader,
                out responsePayload);

            return (MessageStatus)status;
        }

        #endregion

        #region 3.1.5.8   Server Requests an Unlock of a Byte-Range
        /// <summary>
        /// Unlock a range of a file
        /// </summary>
        /// <param name="offset">The start position of bytes to be unlocked</param>
        /// <param name="length">The bytes size to be unlocked</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus UnlockByteRange(UInt64 offset, UInt64 length)
        {
            // SMB2 can lock or unlock multi-ranges within a file. 
            // However this interface method requires one locked range, so only need to specify a one element lock_element array.
            LOCK_ELEMENT[] elements = new LOCK_ELEMENT[1];
            elements[0].Offset = offset;
            elements[0].Length = length;
            elements[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;

            LOCK_Response responsePayload;
            uint status = this.smb2Client.Lock(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                this.lockSequence++,
                this.fileId,
                elements,
                out packetHeader,
                out responsePayload);

            return (MessageStatus)status;
        }
        #endregion

        #region 3.1.5.9   Server Requests an FsControl Request
        /// <summary>
        /// Send control code to latest opened file 
        /// </summary>
        /// <param name="ctlCode">The control code to be sent, as specified in [FSCC] section 2.3</param>
        /// <param name="maxOutBufferSize">The maximum number of byte to output</param>
        /// <param name="inBuffer">The input data of this control operation</param>
        /// <param name="outBuffer">The output data of this control operation</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus IOControl(UInt32 ctlCode, UInt32 maxOutBufferSize, byte[] inBuffer, out byte[] outBuffer)
        {
            testConfig.CheckFSCTL(ctlCode);

            IOCTL_Response ioctlResponse;
            byte[] inputResponse;
            uint status = this.smb2Client.IoCtl(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                (CtlCode_Values)ctlCode,
                this.fileId,
                maxOutBufferSize,
                inBuffer,
                maxOutBufferSize,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out inputResponse,
                out outBuffer,
                out packetHeader,
                out ioctlResponse);

            return (MessageStatus)status;
        }
        #endregion

        #region 3.1.5.10   Server Requests Change Notifications for a Directory
        /// <summary>
        /// Send change notification to latest opened file
        /// </summary>
        /// <param name="maxOutputSize">Specify the max bytes number to be returned </param>
        /// <param name="watchTree">Indicates whether the directory should be monitored recursively</param>
        /// <param name="completionFilter">Indicates the filter of the notification</param>
        /// <param name="outBuffer">Array of bytes containing the notification data</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus ChangeNotification(UInt32 maxOutputSize, bool watchTree, UInt32 completionFilter, out byte[] outBuffer)
        {
            this.smb2Client.ChangeNotify(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                maxOutputSize,
                this.fileId,
                watchTree ? CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE : CHANGE_NOTIFY_Request_Flags_Values.NONE,
                (CompletionFilter_Values)completionFilter);

            outBuffer = null;
            return MessageStatus.SUCCESS; // Do not wait for response since it must be triggered by another action
        }
        #endregion

        #region 3.1.5.11   Server Requests a Query of File Information
        /// <summary>
        /// Query  information of the latest opened file
        /// </summary>
        /// <param name="maxOutPutSize">Specify the maximum number of byte to be returned</param>
        /// <param name="fileInfomationClass">The type of the information.</param>
        /// <param name="buffer">The input buffer in bytes, including the file information query parameters</param>
        /// <param name="outBuffer">Array of bytes containing the file information. 
        /// The structure of these bytes is based on FileInformationClass.
        /// </param>
        /// <returns>NTStatus code</returns>
        public MessageStatus QueryFileInformation(
             UInt32 maxOutputSize,
             UInt32 fileInfomationClass,
             byte[] buffer,
             out byte[] outBuffer)
        {
            QUERY_INFO_Response queryInfoResponse;
            uint status = this.smb2Client.QueryInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                InfoType_Values.SMB2_0_INFO_FILE,
                (byte)fileInfomationClass,
                maxOutputSize,
                AdditionalInformation_Values.NONE,
                QUERY_INFO_Request_Flags_Values.V1,
                this.fileId,
                buffer,
                out outBuffer,
                out packetHeader,
                out queryInfoResponse);

            return (MessageStatus)status;
        }
        #endregion

        #region 3.1.5.12   Server Requests a Query of File System Information

        /// <summary>
        /// Query system information of the latest opened file
        /// </summary>
        /// <param name="maxOutputSize">Specify the maximum byte number to be returned</param>
        /// <param name="fsInformationClass">The type of the system information.</param>
        /// <param name="outBuffer">
        /// Array of bytes containing the file system information. 
        /// The structure of these bytes is based on FileSystemInformationClass.</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus QueryFileSystemInformation(
            UInt32 maxOutputSize,
            UInt32 fsInformationClass,
            out byte[] outBuffer)
        {
            QUERY_INFO_Response queryInfoResponse;
            uint status = this.smb2Client.QueryInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                InfoType_Values.SMB2_0_INFO_FILESYSTEM,
                (byte)fsInformationClass,
                maxOutputSize,
                AdditionalInformation_Values.NONE,
                QUERY_INFO_Request_Flags_Values.V1,
                this.fileId,
                null,
                out outBuffer,
                out packetHeader,
                out queryInfoResponse);

            return (MessageStatus)status;
        }

        #endregion

        #region 3.1.5.13   Server Requests a Query of Security Information

        /// <summary>
        /// Query security information of the latest opened file
        /// </summary>
        /// <param name="maxOutputSize">Specify the maximum  bytes number to be returned</param>
        /// <param name="securityInformation">The type of the system information.</param>
        /// <param name="outBuffer">Array of bytes containing the file system information</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus QuerySecurityInformation(
            UInt32 maxOutputSize,
            UInt32 securityInformation,
            out byte[] outBuffer)
        {
            QUERY_INFO_Response queryInfoResponse;
            uint status = this.smb2Client.QueryInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                InfoType_Values.SMB2_0_INFO_SECURITY,
                0, // For security queries, this field MUST be set to 0
                maxOutputSize,
                (AdditionalInformation_Values)securityInformation,
                QUERY_INFO_Request_Flags_Values.SL_RESTART_SCAN,
                this.fileId,
                null,
                out outBuffer,
                out packetHeader,
                out queryInfoResponse);

            return (MessageStatus)status;
        }

        #endregion

        #region 3.1.5.14   Server Requests Setting of File Information
        /// <summary>
        /// Apply specific information of the latest opened file
        /// </summary>
        /// <param name="fileInfomationClass">Array of bytes containing the file information. 
        /// The structure of these bytes is based on FileInformationClass.
        /// </param>
        /// <param name="buffer">
        /// Array of bytes containing the file information. 
        /// The structure of these bytes is based on FileInformationClass.
        /// </param>
        /// <returns>NTStatus code</returns>
        public MessageStatus SetFileInformation(
            UInt32 fileInfomationClass,
            byte[] buffer)
        {
            SET_INFO_Response setInfoResponse;
            uint status = this.smb2Client.SetInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                SET_INFO_Request_InfoType_Values.SMB2_0_INFO_FILE,
                (byte)fileInfomationClass,
                SET_INFO_Request_AdditionalInformation_Values.NONE,
                this.fileId,
                buffer,
                out packetHeader,
                out setInfoResponse);

            return (MessageStatus)status;
        }

        #endregion

        #region 3.1.5.15   Server Requests Setting of File System Information
        /// <summary>
        ///  Apply specific system information of the latest opened file
        /// </summary>
        /// <param name="fsInformationClass">The type of the system information.</param>
        /// <param name="buffer">
        /// Array of bytes containing the file system information. 
        /// The structure of these bytes is based on FileInformationClass.
        /// </param>
        /// <returns>NTStatus code</returns>
        public MessageStatus SetFileSystemInformation(
            UInt32 fsInformationClass,
            byte[] buffer)
        {
            SET_INFO_Response setInfoResponse;
            uint status = this.smb2Client.SetInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                SET_INFO_Request_InfoType_Values.SMB2_0_INFO_FILESYSTEM,
                (byte)fsInformationClass,
                SET_INFO_Request_AdditionalInformation_Values.NONE,
                this.fileId,
                buffer,
                out packetHeader,
                out setInfoResponse);

            return (MessageStatus)status;
        }
        #endregion

        #region 3.1.5.16   Server Requests Setting of Security Information
        /// <summary>
        /// Apply security information of the latest opened file
        /// </summary>
        /// <param name="securityInformation">The type of the system information.</param>
        /// <param name="buffer">Array of bytes containing the file system information.</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus SetSecurityInformation(
            UInt32 securityInformation,
            byte[] buffer)
        {
            SET_INFO_Response setInfoResponse;
            uint status = this.smb2Client.SetInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                SET_INFO_Request_InfoType_Values.SMB2_0_INFO_SECURITY,
                (byte)securityInformation,
                SET_INFO_Request_AdditionalInformation_Values.NONE,
                this.fileId,
                buffer,
                out packetHeader,
                out setInfoResponse);

            return (MessageStatus)status;
        }
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
        public MessageStatus QueryQuotaInformation(
            UInt32 maxOutputBufferSize,
            UInt32 quotaInformationClass,
            byte[] inputBuffer,
            out byte[] outputBuffer)
        {
            QUERY_INFO_Response queryInfoResponse;
            uint status = this.smb2Client.QueryInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                InfoType_Values.SMB2_0_INFO_QUOTA,
                (byte)quotaInformationClass,
                maxOutputBufferSize,
                AdditionalInformation_Values.NONE,
                QUERY_INFO_Request_Flags_Values.V1,
                this.fileId,
                inputBuffer,
                out outputBuffer,
                out packetHeader,
                out queryInfoResponse);

            return (MessageStatus)status;
        }
        #endregion

        #region 3.1.5.21   Server Requests Setting Quota Information
        /// <summary>
        /// Set Quota Information
        /// </summary>        
        /// <param name="quotaInformationClass">FileInfoClass for Quota Information.</param>
        /// <param name="inputBuffer">The input buffer in bytes, includes the quota information query parameters.</param>              
        /// <returns>NTSTATUS code.</returns>
        public MessageStatus SetQuotaInformation(
            UInt32 quotaInformationClass,
            byte[] inputBuffer)
        {
            SET_INFO_Response setInfoResponse;
            uint status = this.smb2Client.SetInfo(
                1,
                1,
                this.packetHeaderFlag,
                this.messageId++,
                this.sessionId,
                this.treeId,
                SET_INFO_Request_InfoType_Values.SMB2_0_INFO_QUOTA,
                (byte)quotaInformationClass,
                SET_INFO_Request_AdditionalInformation_Values.NONE,
                this.fileId,
                inputBuffer,
                out packetHeader,
                out setInfoResponse);

            return (MessageStatus)status;
        }
        #endregion

        #endregion
    }
}