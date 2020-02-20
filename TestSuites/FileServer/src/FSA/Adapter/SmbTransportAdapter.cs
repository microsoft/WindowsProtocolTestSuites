// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using NamespaceSmb = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Smb2 = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// Communicate with NTFS implemented machine in SMB protocol
    /// </summary>
    public class SmbTransportAdapter : ManagedAdapterBase, ITransportAdapter
    {
        #region Define fields

        private string domainName;
        private string serverName;
        private string userName;
        private string password;
        private string shareName;
        private ushort fileId;
        private ushort portNumber;
        private ushort sessionId;
        private ushort treeId;

        private IpVersion ipVersion;
        private SmbClient smbClient;
        private TimeSpan timeout;
        private UInt32 bufferSize;

        private FSATestConfig testConfig;

        // The following suppression is adopted because this field will be used by reflection.
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private ITestSite site;

        // The following suppression is adopted because this field will be used by reflection.
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private UInt32 maxConnection;

        private bool isSendSignedRequest;

        /// <summary>
        /// The identifier of the connection to get.
        /// </summary>
        private const int ConnectionId = 0;
        #endregion

        #region Define properties

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
        /// Number in bytes of the net protocols' client buffer.
        /// </summary>
        public UInt32 BufferSize
        {
            set 
            { 
                this.bufferSize = value; 
            }
        }

        /// <summary>
        /// IpVersion of the protocols
        /// </summary>
        public IpVersion IPVersion
        {
            set 
            { 
                this.ipVersion = value; 
            }
        }

        /// <summary>
        /// Port number of the protocols
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
        /// User name being used
        /// </summary>
        public string UserName
        {
            set 
            { 
                this.userName = value; 
            }
        }

        /// <summary>
        /// Password of the user being used
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
                return this.smbClient.Capability.IsUsePassThrough;
            }
            set
            {
                this.smbClient.Capability.IsUsePassThrough = value;
            }
        }

        /// <summary>
        /// To specify if the transport sends signed request to server.
        /// </summary>
        public bool IsSendSignedRequest
        {
            get { return this.isSendSignedRequest; }
            set { this.isSendSignedRequest = value; }
        }
        #endregion

        #region Adapter initialization and cleanup

        public SmbTransportAdapter(FSATestConfig fsaTestConfig)
        {
            this.testConfig = fsaTestConfig;
        }

        /// <summary>
        /// Initialize this adapter, will be called by PTF automatically
        /// </summary>
        /// <param name="testSite">ITestSite type object, will be set by PTF automatically</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            this.site = testSite;
            this.smbClient = null;
        }

        /// <summary>
        /// Disconnect from share and release object
        /// </summary>
        /// <param name="disposing">bool, indicates it is disposing status</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.smbClient != null)
            {
                this.smbClient.Dispose();
                this.smbClient = null;
            }
        }

        /// <summary>
        /// Initialize a tcp connection with server
        /// </summary>
        /// <param name="isWindows">the flag is true if the OS is windows</param>
        public void Initialize(bool isWindows)
        {
            //Initialize SMBClientSDK and connect to share
            this.smbClient = new SmbClient();
            this.site.Log.Add(LogEntryKind.Debug, "Connect to server {0}.", this.serverName);
            this.smbClient.Connect(this.serverName, (int)this.portNumber, this.ipVersion, (int)this.bufferSize);

            MessageStatus status = MessageStatus.SUCCESS;
            status = this.Negotiate();
            if (status != MessageStatus.SUCCESS)
            {
                throw new InvalidOperationException("Negotiate failed:" + status.ToString());
            }

            status = this.SessionSetup(isWindows);
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
        /// Calling this method will disconnect current connection and
        /// create a new connect to a share as specified as in the properties.
        /// </summary>
        public override void Reset()
        {
           base.Reset();

           if (this.smbClient != null)
           {
               this.TreeDisconnect();
               this.LogOff();
               this.smbClient.Disconnect();
               this.smbClient.Dispose();
               this.smbClient = null;
           }        
        }

        # endregion

        #region Protected methods
        /// <summary>
        /// Negotiate method, will be called automatically by initialize and reset method
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus Negotiate()
        {
            string[] dialetNameString = { DialectNameString.PCNET1, 
                DialectNameString.LANMAN10, 
                DialectNameString.WFW10,
                DialectNameString.LANMAN12,
                DialectNameString.LANMAN21,
                DialectNameString.NTLANMAN
                };
            SmbPacket packet = this.smbClient.CreateNegotiateRequest(SignState.NONE, dialetNameString);
            this.smbClient.SendPacket(packet);

            SmbPacket response = this.smbClient.ExpectPacket(this.timeout);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_NEGOTIATE)
            {
                throw new InvalidOperationException("No negotiate response.");
            }
            return (MessageStatus)response.SmbHeader.Status;
        }

        /// <summary>
        /// SessionSetup method, will be called automatically by initialize and reset method
        /// </summary>
        /// <returns>NTStatus code</returns>
        /// <param name="isWindows">the flag is true if the OS is windows</param>
        protected MessageStatus SessionSetup(bool isWindows)
        {
            SmbPacket response = null;

            SmbPacket packet = this.smbClient.CreateFirstSessionSetupRequest(
                SmbSecurityPackage.NTLM, 
                this.serverName, 
                this.domainName, 
                this.userName, 
                this.password);
            this.smbClient.SendPacket(packet);

            response = this.smbClient.ExpectPacket(this.timeout);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_SESSION_SETUP_ANDX)
            {
                throw new InvalidOperationException("No session setup 1 response.");
            }

            this.sessionId = response.SmbHeader.Uid;
            if (!isWindows)
            {
                SmbClientConnection connection = this.smbClient.Context.Connection;
                connection.GssApi = new Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.SspiClientSecurityContext(
                    Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.SecurityPackageType.Ntlm,
                    new Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.AccountCredential(
                        this.domainName,
                        this.userName,
                        this.password),
                    "cifs/" + this.serverName,
                    Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.ClientSecurityContextAttribute.Connection,
                    Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.SecurityTargetDataRepresentation.SecurityNetworkDrep);
                this.smbClient.Context.AddOrUpdateConnection(connection);
            }
            packet = this.smbClient.CreateSecondSessionSetupRequest((ushort)this.sessionId, SmbSecurityPackage.NTLM);
            this.smbClient.SendPacket(packet);

            response = this.smbClient.ExpectPacket(this.timeout);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_SESSION_SETUP_ANDX)
            {
                throw new InvalidOperationException("No session setup 2 response.");
            }
            return (MessageStatus)response.SmbHeader.Status;
        }

        /// <summary>
        /// TreeConnect method, will be called automatically by initialize and reset method
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus TreeConnect()
        {
            string sharePath = @"\\" + this.serverName + @"\" + this.shareName;

            SmbPacket packet = this.smbClient.CreateTreeConnectRequest((ushort)this.sessionId, sharePath);           

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbTreeConnectAndxResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_TREE_CONNECT_ANDX)
            {
                throw new InvalidOperationException("No tree_connect_andx response.");
            }

            this.treeId = response.SmbHeader.Tid;
            return (MessageStatus)response.SmbHeader.Status;
        }

        /// <summary>
        /// TreeDisconnect method, will be called automatically by initialize and reset method
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus TreeDisconnect()
        {
            SmbPacket packet = this.smbClient.CreateTreeDisconnectRequest(this.treeId);            

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbTreeDisconnectResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_TREE_DISCONNECT)
            {
                throw new InvalidOperationException("No tree_disconnect_andx response.");
            }
            return (MessageStatus)response.SmbHeader.Status;
        }

        /// <summary>
        /// Logoff method, will be called automatically by initialize and reset method
        /// </summary>
        /// <returns>NTStatus code</returns>
        protected MessageStatus LogOff()
        {
            SmbPacket packet = this.smbClient.CreateLogoffRequest(this.sessionId);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbLogoffAndxResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_LOGOFF_ANDX)
            {
                throw new InvalidOperationException("No logoff response.");
            }
            return (MessageStatus)response.SmbHeader.Status;
        }

        #endregion

        #region Implementing ITransportAdapter methods

        #region 3.1.5.1   Server Requests an Open of a File

        /// <summary>
        /// Create a new file or open an existing file.
        /// </summary>
        /// <param name="fileName">The name of the data file or directory to be created or opened</param>
        /// <param name="fileAttribute">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="desiredAccess">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13.1</param>
        /// <param name="shareAccess">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createOptions">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createDisposition">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createAction">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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
            SmbPacket packet = this.smbClient.CreateCreateRequest(
                (ushort)this.treeId,
                fileName,
                (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.NtTransactDesiredAccess)desiredAccess,
                (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.SMB_EXT_FILE_ATTR)fileAttribute,
                (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.NtTransactShareAccess)shareAccess,
                (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.NtTransactCreateDisposition)createDisposition,
                (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.NtTransactCreateOptions)createOptions,
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPBATCH);            

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbNtCreateAndxResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_NT_CREATE_ANDX)
            {
                throw new InvalidOperationException("No create response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtCreateAndxResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtCreateAndxResponsePacket createResponse =
                    (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtCreateAndxResponsePacket)response;
                createAction = (uint)createResponse.SmbParameters.CreationAction;
                this.fileId = createResponse.SmbParameters.FID;
                return MessageStatus.SUCCESS;
            }
            else
            {
                createAction = (uint)CreateAction.CREATED;
                return (MessageStatus)response.SmbHeader.Status;
            }
        }

        /// <summary>
        /// Create a new file or open an existing file.
        /// </summary>
        /// <param name="fileName">The name of the data file or directory to be created or opened</param>
        /// <param name="fileAttribute">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="desiredAccess">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13.1</param>
        /// <param name="shareAccess">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createOptions">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createDisposition">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="createAction">A bitmask for the open operation, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="fileId">The fileId for the open.</param>
        /// <param name="treeId">The treeId for the open.</param>
        /// <param name="sessionId">The sessionId for the open.</param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus CreateFile(
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
         )
        {
            fileId = Smb2.FILEID.Zero;
            treeId = 0;
            sessionId = 0;    
            createAction = (uint)CreateAction.NULL;
            return MessageStatus.SUCCESS;
           
        }

        #endregion

        #region 3.1.5.2   Server Requests a Read

        /// <summary>
        /// Read the file which is opened most recently
        /// </summary>
        /// <param name="offset">The position in byte to read start</param>
        /// <param name="byteCount">The maximum value of byte number to read</param>
        /// <param name="isNonCached">Indicate whether make a cached for the operation in the server. </param>
        /// <param name="outBuffer">The output data of this control operation</param>
        /// <returns>NTstatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus Read(UInt64 offset, UInt32 byteCount, bool isNonCached, out byte[] outBuffer)
        {
            SmbPacket packet = this.smbClient.CreateReadRequest(this.fileId, (ushort)byteCount, (uint)offset);            

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbReadAndxResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_READ_ANDX)
            {
                throw new InvalidOperationException("No read response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbReadAndxResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbReadAndxResponsePacket readResponse =
                    (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbReadAndxResponsePacket)response;
                outBuffer = readResponse.SmbData.Data;
                return MessageStatus.SUCCESS;
            }
            else
            {
                outBuffer = new byte[0];
                return (MessageStatus)response.SmbHeader.Status;
            }
        }

        #endregion

        #region 3.1.5.3   Server Requests a Write

        /// <summary>
        /// Write the file which is opened most recently
        /// </summary>
        /// <param name="buffer">Bytes to be written in the file</param>
        /// <param name="offset">The offset of the file from where client wants to start writing</param>
        /// <param name="isWriteThrough">If true, the write should be treated in a write-through fashion</param>
        /// <param name="isNonCached">If true, the write should be sent directly to the disk instead of the cache</param>
        /// <param name="bytesWritten">The number of the bytes written</param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus Write(byte[] buffer, UInt64 offset, bool isWriteThrough, bool isNonCached, out UInt64 bytesWritten)
        {
            SmbPacket packet = this.smbClient.CreateWriteRequest(this.fileId, (uint)offset, buffer);            

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbWriteAndxResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_WRITE_ANDX)
            {
                throw new InvalidOperationException("No write response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbWriteAndxResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbWriteAndxResponsePacket writeResponse =
                    (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbWriteAndxResponsePacket)response;
                bytesWritten = writeResponse.SmbParameters.Count;
                return MessageStatus.SUCCESS;
            }
            else
            {
                bytesWritten = 0; //If fail, the number of byte written will be 0
                return (MessageStatus)response.SmbHeader.Status;
            }
        }

        #endregion

        #region 3.1.5.4   Server Requests Closing an Open

        /// <summary>
        /// Close the open object that was lately created
        /// </summary>
        /// <returns>NTStatus code</returns>
        public MessageStatus CloseFile()
        {
            Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbCloseRequestPacket packet =
                this.smbClient.CreateCloseRequest(this.fileId);            

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbCloseResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_CLOSE)
            {
                throw new InvalidOperationException("No close response.");
            }
            return (MessageStatus)response.SmbHeader.Status;
        }

        #endregion

        #region 3.1.5.5   Server Requests Querying a Directory

        /// <summary>
        /// Query an existing directory with specific file name pattern. Supported by SMB but still need to be completed.
        /// </summary>
        /// <param name="fileInformationClass">The type of information to be queried, as specified in [MS-FSCC] section 2.4</param>
        /// <param name="maxOutPutSize">The maximum number of bytes to return</param>
        /// <param name="restartScan">If true, indicating the enumeration of the directory should be restarted</param>
        /// <param name="returnSingleEntry">Indicate whether return a single entry or not</param>       
        /// <param name="fileIndex">An index number from which to resume the enumeration</param>
        /// <param name="fileNamePattern">A Unicode string containing the file name pattern to match. "* ?" must be treated as wildcards</param>
        /// <param name="outBuffer">The query result</param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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
            SmbPacket packet = this.smbClient.CreateTrans2QueryPathInformationRequest(
                this.treeId,
                fileNamePattern, 
                Trans2SmbParametersFlags.NONE,
                (QueryInformationLevel)(fileInformationClass),
                (ushort)maxOutPutSize,
                true);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbTrans2QueryPathInformationResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION2)
            {
                throw new InvalidOperationException("No query directory response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryPathInformationResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryPathInformationResponsePacket queryDirectoryResponse =
                    (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryPathInformationResponsePacket)response;
                outBuffer = queryDirectoryResponse.SmbData.Trans2_Data;
                return MessageStatus.SUCCESS;
            }
            else
            {
                outBuffer = null;
                return (MessageStatus)response.SmbHeader.Status;
            }
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
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus QueryDirectory(
            Smb2.FILEID fileId,
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
            //No implementation for SMBTransportadapter
            outBuffer = null;
            return MessageStatus.SUCCESS;            
        }
        #endregion

        #region 3.1.5.6   Server Requests Flushing Cached Data

        /// <summary>
        /// Flush all persistent attributes to stable storage. Not supported by SMB.
        /// </summary>
        /// <returns>NTStatus code</returns>
        public MessageStatus FlushCachedData()
        {            
            return MessageStatus.NOT_SUPPORTED;
        }

        #endregion

        #region 3.1.5.7   Server Requests a Byte-Range Lock

        /// <summary>
        /// Lock range of a file. Not supported by SMB.
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
            return MessageStatus.NOT_SUPPORTED;
        }

        #endregion

        #region 3.1.5.8   Server Requests an Unlock of a Byte-Range

        /// <summary>
        /// Unlock a range of a file. Not supported by SMB.
        /// </summary>
        /// <param name="offset">The start position of bytes to be unlocked</param>
        /// <param name="length">The bytes size to be unlocked</param>
        /// <returns>NTStatus code</returns>
        public MessageStatus UnlockByteRange(UInt64 offset, UInt64 length)
        {
            return MessageStatus.NOT_SUPPORTED;
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
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus IOControl(UInt32 ctlCode, UInt32 maxOutBufferSize, byte[] inBuffer, out byte[] outBuffer)
        {
            testConfig.CheckFSCTL(ctlCode);

            outBuffer = null;
            //Set isFlag to 0 to apply a share root handle as SMB described.
            SmbNtTransactIoctlRequestPacket packet = this.smbClient.CreateNTTransIOCtlRequest(this.fileId, true, 0, ctlCode, inBuffer);
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameter = new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameter.DataCount = packet.SmbParameters.DataCount;
            smbParameter.DataOffset = packet.SmbParameters.DataOffset;
            smbParameter.Function = packet.SmbParameters.Function;
            smbParameter.MaxDataCount = maxOutBufferSize;
            smbParameter.MaxParameterCount = packet.SmbParameters.MaxParameterCount;
            smbParameter.MaxSetupCount = packet.SmbParameters.MaxSetupCount;
            smbParameter.ParameterCount = packet.SmbParameters.ParameterCount;
            smbParameter.ParameterOffset = packet.SmbParameters.ParameterOffset;
            smbParameter.Reserved1 = packet.SmbParameters.Reserved1;
            smbParameter.Setup = packet.SmbParameters.Setup;
            smbParameter.SetupCount = packet.SmbParameters.SetupCount;
            smbParameter.TotalDataCount = packet.SmbParameters.TotalDataCount;
            smbParameter.TotalParameterCount = packet.SmbParameters.TotalParameterCount;
            smbParameter.WordCount = packet.SmbParameters.WordCount;

            Type smbType = typeof(SmbNtTransactIoctlRequestPacket);
            smbType.GetProperty("SmbParameters").SetValue(packet, smbParameter, null);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbNtTransactIoctlResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_NT_TRANSACT)
            {
                throw new InvalidOperationException("No read response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtTransactIoctlResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtTransactIoctlResponsePacket ioctlResponse
                    = (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtTransactIoctlResponsePacket)response;
                outBuffer = ioctlResponse.SmbData.Data;
            }
            else if (response is SmbErrorResponsePacket)
            {
                SmbErrorResponsePacket errorResponse = (SmbErrorResponsePacket)response;
                outBuffer = null;
                return (MessageStatus)errorResponse.SmbHeader.Status;
            }            
            return (MessageStatus)response.SmbHeader.Status;
        }

        #endregion

        #region 3.1.5.10   Server Requests Change Notifications for a Directory

        /// <summary>
        /// Send or receive file change notification to latest opened file
        /// </summary>
        /// <param name="maxOutPutSize">Specify the maximum number of byte to be returned </param>
        /// <param name="watchTree">Indicates whether the directory should be monitored recursively</param>
        /// <param name="completionFilter">Indicates the filter of the notification</param>
        /// <param name="outBuffer">Array of bytes containing the notification data</param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus ChangeNotification(UInt32 maxOutPutSize, bool watchTree, UInt32 completionFilter, out byte[] outBuffer)
        {
            outBuffer = null;
            CifsClientConfig cifsClientConfig = new CifsClientConfig();
            CifsClient cifsClient = new CifsClient(cifsClientConfig);

            // Set the messageId to 1.
            SmbPacket packet =
            cifsClient.CreateNtTransactNotifyChangeRequest(
                1, 
                this.sessionId,
                this.treeId,
                (SmbFlags)smbClient.Capability.Flag,
                (SmbFlags2)smbClient.Capability.Flags2,
                smbClient.Capability.MaxSetupCount,
                smbClient.Capability.MaxParameterCount,
                smbClient.Capability.MaxDataCount,
                this.fileId,
                CompletionFilter.FILE_NOTIFY_CHANGE_CREATION
                | CompletionFilter.FILE_NOTIFY_CHANGE_NAME
                | CompletionFilter.FILE_NOTIFY_CHANGE_FILE_NAME,
                true);

            SendSmbPacket(packet);

            packet = smbClient.CreateCreateRequest(
                this.treeId,
                Site.Properties.Get("FSA.ExistingFolder"),
                (NtTransactDesiredAccess)0x1,
                (SMB_EXT_FILE_ATTR)0x80,
                (NtTransactShareAccess)0x1,
                (NtTransactCreateDisposition)0x3,
                (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.NtTransactCreateOptions)0x1,
                NtTransactImpersonationLevel.SEC_IMPERSONATE,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            SendSmbPacket(packet);
            SmbPacket response = smbClient.ExpectPacket(this.timeout);

            if (response is SmbNtTransactNotifyChangeResponsePacket)
            {
                SmbNtTransactNotifyChangeResponsePacket changeNotifyResponse = (SmbNtTransactNotifyChangeResponsePacket)response;
                return (MessageStatus)changeNotifyResponse.SmbHeader.Status;
            }
            else if (response is SmbErrorResponsePacket)
            {
                SmbErrorResponsePacket errorResponse = (SmbErrorResponsePacket)response;
                outBuffer = null;
                return (MessageStatus)errorResponse.SmbHeader.Status;
            }
            else
            {
                outBuffer = null;
                return MessageStatus.INVALID_PARAMETER;
            }
        }

        #endregion

        #region 3.1.5.11   Server Requests a Query of File Information

        /// <summary>
        /// Query  information of the latest opened file
        /// </summary>
        /// <param name="maxOutPutSize">Specify the maximum number of byte to be returned</param>
        /// <param name="fileInfomationClass">The type of the information, as specified in [MS-FSCC] section 2.4</param>
        /// <param name="buffer">Data in bytes, includes the file information query parameters.</param>
        /// <param name="outBuffer">Array of bytes containing the file information. The structure of these bytes is 
        /// base on FileInformationClass, as specified in [MS-FSCC] section 2.4</param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus QueryFileInformation(
             UInt32 maxOutPutSize,
             UInt32 fileInfomationClass,
             byte[] buffer,
             out byte[] outBuffer)
        {
            SmbPacket packet = this.smbClient.CreateTrans2QueryFileInformationRequest(
                this.fileId,
                Trans2SmbParametersFlags.NONE,
                (ushort)maxOutPutSize,
                (QueryInformationLevel)fileInfomationClass,
                null);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbTrans2QueryFileInformationResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION2)
            {
                throw new InvalidOperationException("No query file information response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryFileInformationResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryFileInformationResponsePacket queryFileResponse =
                    (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryFileInformationResponsePacket)response;
                outBuffer = queryFileResponse.SmbData.Trans2_Data;
                return MessageStatus.SUCCESS;
            }
            else
            {
                outBuffer = null;
                return (MessageStatus)response.SmbHeader.Status;
            }
        }

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
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus QueryFileSystemInformation(
            UInt32 maxOutputSize,
            UInt32 fsInformationClass,
            out byte[] outBuffer)
        {
            SmbPacket packet = this.smbClient.CreateTrans2QueryFileSystemInformationRequest(
                this.treeId,
                (ushort)maxOutputSize,
                Trans2SmbParametersFlags.NONE,
                (QueryFSInformationLevel)fsInformationClass);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbTrans2QueryFsInformationResponsePacket>(packet);
            if (response.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION2)
            {
                throw new InvalidOperationException("No query file system information response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryFsInformationResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryFsInformationResponsePacket queryFsResponse =
                    (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbTrans2QueryFsInformationResponsePacket)response;
                outBuffer = queryFsResponse.SmbData.Trans2_Data;
                return MessageStatus.SUCCESS;
            }
            else
            {
                outBuffer = null;
                return (MessageStatus)response.SmbHeader.Status;
            }
        }

        #endregion

        #region 3.1.5.13   Server Requests a Query of Security Information

        /// <summary>
        /// Query security information of the latest opened file
        /// </summary>
        /// <param name="maxOutPutSize">Specify the maximum  bytes number to be returned</param>
        /// <param name="securityInformation">The type of the system information, 
        /// as specified in [MS-DTYP] section 2.4.7</param>
        /// <param name="outBuffer">
        /// Array of bytes containing the file system information, as defined in [MS-DTYP] section 2.4.6
        /// </param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus QuerySecurityInformation(
            UInt32 maxOutPutSize,
            UInt32 securityInformation,
            out byte[] outBuffer)
        {
            outBuffer = null;

            CifsClientConfig cifsClientConfig = new CifsClientConfig();
            CifsClient cifsClient = new CifsClient(cifsClientConfig);

            // Set the messageId to 1.
            SmbNtTransactQuerySecurityDescRequestPacket packet =
                cifsClient.CreateNtTransactQuerySecurityDescRequest(
                    1, 
                    this.sessionId,
                    this.treeId,
                    (SmbFlags)smbClient.Capability.Flag,
                    (SmbFlags2)smbClient.Capability.Flags2,
                    smbClient.Capability.MaxSetupCount,
                    smbClient.Capability.MaxParameterCount,
                    smbClient.Capability.MaxDataCount,
                    this.fileId,
                    (NtTransactSecurityInformation)securityInformation);

            SmbPacket response = this.SendPacketAndExpectResponse<SmbNtTransactQuerySecurityDescResponsePacket>(packet);

            if (response is SmbNtTransactQuerySecurityDescResponsePacket)
            {
                SmbNtTransactQuerySecurityDescResponsePacket querySecurityResponse = (SmbNtTransactQuerySecurityDescResponsePacket)response;
                RawSecurityDescriptor ReceivedSD = new RawSecurityDescriptor(" ");
                outBuffer = new byte[ReceivedSD.BinaryLength];

                //Read the data from index 0.
                ReceivedSD.GetBinaryForm(outBuffer, 0);

                return (MessageStatus)querySecurityResponse.SmbHeader.Status;
            }
            else if (response is SmbErrorResponsePacket)
            {
                SmbErrorResponsePacket errorResponse = (SmbErrorResponsePacket)response;
                outBuffer = null;
                return (MessageStatus)errorResponse.SmbHeader.Status;
            }
            else
            {
                outBuffer = null;
                return MessageStatus.INVALID_PARAMETER;
            }
        }

        #endregion

        #region 3.1.5.14   Server Requests Setting of File Information

        /// <summary>
        /// Apply specific information of the latest opened file
        /// </summary>
        /// <param name="fileInfomationClass">Array of bytes containing the file information. The structure of these bytes is 
        /// base on FileInformationClass, as specified in [MS-FSCC] section 2.4
        /// </param>
        /// <param name="buffer">
        /// Array of bytes containing the file information. The structure of these bytes is 
        /// base on FileInformationClass, as specified in [MS-FSCC] section 2.4
        /// </param>
        /// <returns>NTStatus code</returns>
        public MessageStatus SetFileInformation(
            UInt32 fileInfomationClass,
            byte[] buffer)
        {
            SmbPacket packet = this.smbClient.CreateTrans2SetFileInformationRequest(
                this.fileId,
                Trans2SmbParametersFlags.NONE,
                (SetInformationLevel)fileInfomationClass,
                buffer);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbTrans2SetFileInformationResponsePacket>(packet);
            if (response.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION2)
            {
                throw new InvalidOperationException("No set file information response.");
            }
            else if (response is SmbErrorResponsePacket)
            {
                SmbErrorResponsePacket errorResponse = (SmbErrorResponsePacket)response;
                return (MessageStatus)errorResponse.SmbHeader.Status;
            }
            return (MessageStatus)response.SmbHeader.Status;            
        }
        
        #endregion

        #region 3.1.5.15   Server Requests Setting of File System Information

        /// <summary>
        ///  Apply specific system information of the latest opened file
        /// </summary>
        /// <param name="fsInformationClass">The type of the system information, as specified in [MS-FSCC] section 2.5</param>
        /// <param name="buffer">
        /// Array of bytes containing the file system information. The structure of these bytes is 
        /// base on FileInformationClass, as specified in [MS-FSCC] section 2.4
        /// </param>
        /// <returns>NTStatus code</returns>
        public MessageStatus SetFileSystemInformation(
            UInt32 fsInformationClass,
            byte[] buffer)
        {
            SmbPacket packet = this.smbClient.CreateTrans2SetFileSystemInformationRequest(
                this.fileId,
                Trans2SmbParametersFlags.NONE,
                (QueryFSInformationLevel)fsInformationClass,
                buffer);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbTrans2SetFsInformationResponsePacket>(packet);
            if (response.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION2)
            {
                throw new InvalidOperationException("No set file system information response.");
            }
            else if (response is SmbErrorResponsePacket)
            {
                SmbErrorResponsePacket errorResponse = (SmbErrorResponsePacket)response;                
                return (MessageStatus)errorResponse.SmbHeader.Status;
            }
            return (MessageStatus)response.SmbHeader.Status;
        }

        #endregion

        #region 3.1.5.16   Server Requests Setting of Security Information

        /// <summary>
        /// Apply security information of the latest opened file
        /// </summary>
        /// <param name="securityInformation">The type of the system information, 
        /// as specified in [MS-DTYP] section 2.4.7</param>
        /// <param name="buffer">
        /// Array of bytes containing the file system information, as defined in [MS-DTYP] section 2.4.6
        /// </param>
        /// <returns>NTStatus code</returns>
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public MessageStatus SetSecurityInformation(
            UInt32 securityInformation,
            byte[] buffer)
        {
            // ReceivedSD from buffer
            RawSecurityDescriptor ReceivedSD = new RawSecurityDescriptor(" ");
            ReceivedSD.GetBinaryForm(buffer, 0);

            CifsClientConfig cifsClientConfig = new CifsClientConfig();
            CifsClient cifsClient = new CifsClient(cifsClientConfig);

            // Set the messageId to 1.
            SmbNtTransactSetSecurityDescRequestPacket packet =
                cifsClient.CreateNtTransactSetSecurityDescRequest(
                1, 
                this.sessionId,
                this.treeId,
                (SmbFlags)smbClient.Capability.Flag,
                (SmbFlags2)smbClient.Capability.Flags2,
                smbClient.Capability.MaxSetupCount,
                smbClient.Capability.MaxParameterCount,
                smbClient.Capability.MaxDataCount,
                this.fileId,
                (NtTransactSecurityInformation)securityInformation,
                ReceivedSD);

            SmbPacket response = this.SendPacketAndExpectResponse<SmbNtTransactSetSecurityDescResponsePacket>(packet);

            if (response is SmbNtTransactSetSecurityDescResponsePacket)
            {
                SmbNtTransactSetSecurityDescResponsePacket querySecurityResponse = 
                    (SmbNtTransactSetSecurityDescResponsePacket)response;
                return (MessageStatus)querySecurityResponse.SmbHeader.Status;
            }
            else if (response is SmbErrorResponsePacket)
            {
                SmbErrorResponsePacket errorResponse = (SmbErrorResponsePacket)response;
                buffer = null;
                return (MessageStatus)errorResponse.SmbHeader.Status;
            }
            else
            {
                buffer = null;
                return MessageStatus.INVALID_PARAMETER;
            }
           
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
            SMB_QUERY_QUOTA_INFO smbQueryQuotaInfo = TypeMarshal.ToStruct<SMB_QUERY_QUOTA_INFO>(inputBuffer);
            SmbPacket packet = this.smbClient.CreateNTTransQueryQuotaRequest(
                this.fileId,
                smbQueryQuotaInfo.ReturnSingleEntry > 0,
                smbQueryQuotaInfo.RestartScan > 0,
                (int)smbQueryQuotaInfo.SidListLength,
                (int)smbQueryQuotaInfo.StartSidLength,
                (int)smbQueryQuotaInfo.StartSidOffset);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbNtTransQueryQuotaResponsePacket>(packet);

            if (response.SmbHeader.Command != SmbCommand.SMB_COM_NT_TRANSACT)
            {
                throw new InvalidOperationException("No query quota information response.");
            }

            if (response is Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtTransQueryQuotaResponsePacket)
            {
                Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtTransQueryQuotaResponsePacket queryFsResponse =
                    (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.SmbNtTransQueryQuotaResponsePacket)response;
                outputBuffer = queryFsResponse.SmbData.Data;
                return MessageStatus.SUCCESS;
            }
            else
            {
                outputBuffer = null;
                return (MessageStatus)response.SmbHeader.Status;
            }
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
            FILE_QUOTA_INFORMATION fileQuotaInfo = TypeMarshal.ToStruct<FILE_QUOTA_INFORMATION>(inputBuffer);
            DateTime dateTime = DtypUtility.ToDateTime(fileQuotaInfo.ChangeTime);
            SmbPacket packet = this.smbClient.CreateNTTransSetQuotaRequest(
                this.fileId,
                fileQuotaInfo.NextEntryOffset,
                (ulong)dateTime.ToFileTimeUtc(),
                (ulong)fileQuotaInfo.QuotaUsed,
                (ulong)fileQuotaInfo.QuotaThreshold,
                (ulong)fileQuotaInfo.QuotaLimit,
                fileQuotaInfo.Sid);

            SmbPacket response = this.SendPacketAndExpectResponse<NamespaceSmb.SmbNtTransSetQuotaResponsePacket>(packet);
            if (response.SmbHeader.Command != SmbCommand.SMB_COM_NT_TRANSACT)
            {
                throw new InvalidOperationException("No set quota information response.");
            }
            else if (response is SmbErrorResponsePacket)
            {
                SmbErrorResponsePacket errorResponse = (SmbErrorResponsePacket)response;
                return (MessageStatus)errorResponse.SmbHeader.Status;
            }
            return (MessageStatus)response.SmbHeader.Status;

        }

        #endregion

        #endregion

        #region SMB Transport Utility

        private void SendSmbPacket(SmbPacket packet)
        {
            if (isSendSignedRequest)
            {
                CifsClientPerConnection connection =
                    this.smbClient.Context.GetConnection(ConnectionId);

                CifsClientPerSession session = this.smbClient.Context.GetSession(ConnectionId, (ushort)this.sessionId);

                packet.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            this.smbClient.SendPacket(packet);
        }

        private SmbPacket SendPacketAndExpectResponse<T>(SmbPacket packet) where T : SmbPacket, new()
        {
            SendSmbPacket(packet);

            DateTime endTime = DateTime.Now.Add(this.timeout);
            do
            {
                SmbPacket response = this.smbClient.ExpectPacket(this.timeout);
                if (response is T || response is SmbErrorResponsePacket)
                {
                    return response;
                }

            } while (DateTime.Now < endTime);

            throw new TimeoutException("This is timeout when waiting for the response of " + packet.ToString());
        }

        #endregion
    }
}
