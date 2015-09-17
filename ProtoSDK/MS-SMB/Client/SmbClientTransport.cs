// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Used as transport layer of MS-DFSC protocol
    /// </summary>
    public class SmbClientTransport : FileServiceClientTransport
    {
        #region Fields

        /// <summary>
        /// underlying smb client 
        /// </summary>
        private SmbClient smbClient;

        /// <summary>
        /// The uid is used to identify the client
        /// </summary>
        private ushort uid;

        /// <summary>
        /// The treeId is used to identify a share
        /// </summary>
        private ushort treeId;

        /// <summary>
        /// The fid is used to identify an open
        /// </summary>
        private ushort fid;

        /// <summary>
        /// Indicate if the packet should be signed
        /// </summary>
        private bool isSignRequired;

        /// <summary>
        /// The sessionKey which is used to create signature.
        /// </summary>
        private byte[] sessionKey;

        /// <summary>
        /// Indicate if this object has been disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The waiting time for each ExceptPacket call in InternalConnectShare
        /// </summary>
        private TimeSpan internalTimeout;

        /// <summary>
        /// The default waiting time for each ExceptPacket call in InternalConnectShare
        /// </summary>
        private readonly TimeSpan defaultTimeout = new TimeSpan(0, 0, 20);

        /// <summary>
        /// default value of Netbios buffer size
        /// </summary>
        private const int defaultBufferSize = 4096;

        /// <summary>
        /// default number of Netbios max sessions
        /// </summary>
        private const int defaultMaxSessions = 16;

        /// <summary>
        /// default number of Netbios max names
        /// </summary>
        private const int defaultMaxNames = 16;

        /// <summary>
        /// The IPC$ share
        /// </summary>
        private const string IPC_SHARE = "IPC$";

        /// <summary>
        /// Max referral level size
        /// </summary>
        private const ushort sizeofMaxReferralLevel = 2;

        /// <summary>
        /// The size of Unicode null char
        /// </summary>
        private const ushort sizeofUnicodeNullChar = 2;

        /// <summary>
        /// The sign state used in smb negotiate packet
        /// </summary>
        private const SignState defaultSignState = SignState.NONE;

        #endregion


        #region Properties

        /// <summary>
        /// Used privately to get the ClientNextSendSequenceNumber for signing.
        /// </summary>
        private ulong NextSequenceNumber
        {
            get
            {
                if (this.smbClient != null)
                {
                    return this.smbClient.Context.GetConnection(
                        this.smbClient.Context.Connection.ConnectionId).ClientNextSendSequenceNumber;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// To detect whether there are packets cached in the queue of Transport.
        /// Usually, it should be called after Disconnect to assure all events occurred in transport
        /// have been handled.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The transport is not connected.</exception>
        public override bool IsDataAvailable
        {
            get
            {
                if (this.smbClient == null)
                {
                    throw new InvalidOperationException("The transport is not connected.");
                }

                return this.smbClient.IsDataAvailable;
            }
        }


        /// <summary>
        /// the context of client transport, that contains the runtime states and variables.
        /// </summary>
        public override FileServiceClientContext Context
        {
            get
            {
                return this.smbClient.Context;
            }
        }


        #endregion


        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SmbClientTransport()
            : base()
        {
            internalTimeout = defaultTimeout;
            smbClient = new SmbClient();
        }


        #endregion


        #region Methods For Dfsc

        /// <summary>
        /// Set up connection with server.
        /// Including 4 steps: 1. Tcp connection 2. Negotiation 3. SessionSetup 4. TreeConnect in order
        /// </summary>
        /// <param name="server">server name</param>
        /// <param name="client">client name</param>
        /// <param name="domain">user's domain</param>
        /// <param name="userName">user's name</param>
        /// <param name="password">user's password</param>
        /// <param name="timeout">The pending time to get server's response in step 2, 3 or 4</param>
        /// <exception cref="InvalidOperationException">Fail to set up connection with server</exception>
        public override void Connect(
            string server,
            string client,
            string domain,
            string userName,
            string password,
            TimeSpan timeout,
            SecurityPackageType securityPackage, 
            bool useServerToken)
        {
            this.internalTimeout = timeout;

            ConnectShare(server, 445, IpVersion.Any, domain, userName, password, IPC_SHARE, securityPackage, useServerToken);
        }


        /// <summary>
        /// Disconnect from server.
        /// Including 3 steps: 1. TreeDisconnect 2. Logoff 3. Tcp disconnection in order.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response in step 1 or 2</param>
        /// <exception cref="InvalidOperationException">Fail to disconnect from server</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public override void Disconnect(TimeSpan timeout)
        {
            this.internalTimeout = timeout;
            DisconnetShare();
        }


        #endregion

        #region Rap

        /// <summary>
        /// Send rap request payLoad to server
        /// </summary>
        /// <param name="request">The request packet</param>
        /// <returns>The messageId of the sent packet</returns>
        public virtual ushort SendRapPayload(FsRapRequest request)
        {
            SmbTransRapRequestPacket transRequest = smbClient.CreateTransNamedRapRequest(treeId, TransSmbParametersFlags.NONE,
                request.TransParameters.RapOPCode, request.TransParameters.ParamDesc, request.TransParameters.DataDesc,
                request.TransParameters.RAPParamsAndAuxDesc, request.transData.RAPInData);

            if (isSignRequired)
            {
                transRequest.Sign(NextSequenceNumber, sessionKey);
            }

            smbClient.SendPacket(transRequest);
            return transRequest.SmbHeader.Mid;
        }


        /// <summary>
        /// Expect rap response payLoadPacket from server
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The rap response packet</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public virtual FsRapResponse ExpectRapPayload(TimeSpan timeout)
        {
            this.internalTimeout = timeout;

            SmbPacket packet = smbClient.ExpectPacket(timeout);

            if (packet is SmbErrorResponsePacket)
            {
                throw new InvalidOperationException(string.Format(
                    "The server returns error, the error code is 0x{0:x}",
                    packet.SmbHeader.Status));
            }

            SmbTransRapResponsePacket transResponse = packet as SmbTransRapResponsePacket;

            FsRapResponse responsePacket = new FsRapResponse();
            responsePacket.messageId = transResponse.SmbHeader.Mid;
            responsePacket.TransParameters = new RapResponseParam();
            responsePacket.TransParameters.Converter = transResponse.TransParameters.Converter;
            responsePacket.TransParameters.RAPOutParams = transResponse.TransParameters.RAPOutParams;
            responsePacket.TransParameters.Win32ErrorCode = transResponse.TransParameters.Win32ErrorCode;
            responsePacket.TransData = new RapResponseData();
            responsePacket.TransData.RAPOutData = transResponse.TransData.RAPOutData;

            return responsePacket;
        }


        #endregion

        #region Dfsc

        /// <summary>
        /// Send a Dfs request to server
        /// </summary>
        /// <param name="payload">REQ_GET_DFS_REFERRAL structure in byte array</param>
        /// <param name="isEX">
        /// Boolean which indicates whether payload contains DFS_GET_REFERRAL_EX message
        /// MUST be false for SMB transport.
        /// </param>>
        /// <exception cref="System.ArgumentNullException">the payload to be sent is null.</exception>
        /// <exception cref="ArgumentException">extended request packet cannot be in payload</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public override void SendDfscPayload(byte[] payload, bool isEX) 
        {
            if (isEX)
            {
                throw new ArgumentException("REQ_DFS_GET_REFERRAL_EX message not supported over SMB transport");
            }
            if (this.smbClient == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }

            if (payload == null)
            {
                throw new ArgumentNullException("payload");
            }
            REQ_GET_DFS_REFERRAL referral = new REQ_GET_DFS_REFERRAL();

            // Put payload[0] into the lower byte of MaxReferralLevel
            // Put payload[1] into the upper byte of MaxReferralLevel
            // Sample: payload[0] = 0x0B; payload[1] = 0x0A
            // MaxReferralLevel = 0x0A0B;
            referral.MaxReferralLevel = (ushort)(payload[1] << 8 | payload[0]);
            referral.RequestFileName = new byte[payload.Length - sizeofMaxReferralLevel];

            Array.Copy(payload, sizeofMaxReferralLevel, referral.RequestFileName, 0, referral.RequestFileName.Length);
            SmbTrans2GetDfsReferralRequestPacket request = this.smbClient.CreateTrans2GetDFSReferralRequest(
                this.treeId, Trans2SmbParametersFlags.NONE, "", referral);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }
            this.smbClient.SendPacket(request);
        }


        /// <summary>
        /// Wait for the Dfs response packet from server.
        /// </summary>
        /// <param name="status">The status of response</param>
        /// <param name="payload">RESP_GET_DFS_REFERRAL structure in byte array</param>
        /// <param name="timeout">The pending time to get server's response</param> 
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public override void ExpectDfscPayload(TimeSpan timeout, out uint status, out byte[] payload)
        {
            if (this.smbClient == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }

            SmbPacket packet = this.smbClient.ExpectPacket(timeout);
            status = packet.SmbHeader.Status;
            if (packet is SmbErrorResponsePacket)
            {
                payload = null;
            }
            else
            {
                SmbTransaction2FinalResponsePacket response = (SmbTransaction2FinalResponsePacket)packet;
                payload = response.SmbData.Trans2_Data;
            }
        }


        #endregion


        #region Methods For Rpce

        /// <summary>
        /// Connect to a share indicated by shareName in server
        /// This will use smb over tcp as transport. Only one server
        /// can be connected at one time
        /// </summary>
        /// <param name="serverName">The server Name</param>
        /// <param name="port">The server port</param>
        /// <param name="ipVersion">The ip version</param>
        /// <param name="domain">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void ConnectShare(string serverName, int port, IpVersion ipVersion, string domain,
            string userName, string password, string shareName)
        {
            ConnectShare(serverName, port, ipVersion, domain, userName, password, shareName, SecurityPackageType.Ntlm, false);
        }

        /// <summary>
        /// Connect to a share indicated by shareName in server
        /// This will use smb over tcp as transport. Only one server
        /// can be connected at one time
        /// </summary>
        /// <param name="serverName">The server Name</param>
        /// <param name="port">The server port</param>
        /// <param name="ipVersion">The ip version</param>
        /// <param name="domain">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <param name="securityPackage">The security package</param>
        /// <param name="useServerToken">Whether to use token from server</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void ConnectShare(string serverName, int port, IpVersion ipVersion, string domain,
            string userName, string password, string shareName, SecurityPackageType securityPackage, bool useServerToken)
        {
            smbClient.Connect(serverName, port, ipVersion, defaultBufferSize);

            InternalConnectShare(serverName, domain, userName, password, shareName, securityPackage);
        }

        /// <summary>
        /// Connect to a share indicated by shareName in server.
        /// This will use smb over netbios as transport. Only one server
        /// can be connected at one time.
        /// </summary>
        /// <param name="serverNetBiosName">The server netbios name</param>
        /// <param name="clientNetBiosName">The client netbios name</param>
        /// <param name="domain">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void ConnectShare(string serverNetBiosName, string clientNetBiosName, string domain,
            string userName, string password, string shareName)
        {
            smbClient.Connect(serverNetBiosName, clientNetBiosName, defaultBufferSize, defaultMaxSessions,
                defaultMaxNames);

            InternalConnectShare(serverNetBiosName, domain, userName, password, shareName, SecurityPackageType.Ntlm);
        }


        /// <summary>
        /// Create File, named pipe, directory. One transport can only create one file.
        /// </summary>
        /// <param name="fileName">The file, namedpipe, directory name</param>
        /// <param name="desiredAccess">The desired access</param>
        /// <param name="impersonationLevel">The impersonation level</param>
        /// <param name="fileAttribute">The file attribute, this field is only valid when create file.
        /// </param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is
        /// specified in the name field already exists</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <exception cref="ArgumentException">
        /// createOption can not contain FILE_DIRECTORY_FILE when creating file.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void Create(string fileName,
            FsFileDesiredAccess desiredAccess, FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute, FsCreateDisposition createDisposition, FsCreateOption createOption)
        {
            if ((createOption & FsCreateOption.FILE_DIRECTORY_FILE) == FsCreateOption.FILE_DIRECTORY_FILE)
            {
                throw new ArgumentException(
                    "createOption can not contain FILE_DIRECTORY_FILE when creating file.", "createOption");
            }

            InternalCreate(
                fileName,
                (NtTransactDesiredAccess)desiredAccess,
                (NtTransactImpersonationLevel)impersonationLevel,
                (SMB_EXT_FILE_ATTR)fileAttribute,
                (NtTransactCreateDisposition)createDisposition,
                (NtTransactCreateOptions)createOption);
        }


        /// <summary>
        /// Create directory. One transport can only create one directory
        /// </summary>
        /// <param name="directoryName">The directory name</param>
        /// <param name="desiredAccess">The desired access</param>
        /// <param name="impersonationLevel">The impersonation level</param>
        /// <param name="fileAttribute">The file attribute, this field is only valid when create file.
        /// </param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is
        /// specified in the name field already exists</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <exception cref="ArgumentException">
        /// createOption can not contain FILE_NON_DIRECTORY_FILE when creating directory.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void Create(string directoryName,
            FsDirectoryDesiredAccess desiredAccess, FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute, FsCreateDisposition createDisposition, FsCreateOption createOption)
        {
            if ((createOption & FsCreateOption.FILE_NON_DIRECTORY_FILE) == FsCreateOption.FILE_NON_DIRECTORY_FILE)
            {
                throw new ArgumentException(
                    "createOption can not contain FILE_NON_DIRECTORY_FILE when creating directory.", "createOption");
            }

            InternalCreate(
                directoryName,
                (NtTransactDesiredAccess)desiredAccess,
                (NtTransactImpersonationLevel)impersonationLevel,
                (SMB_EXT_FILE_ATTR)fileAttribute,
                (NtTransactCreateDisposition)createDisposition,
                (NtTransactCreateOptions)createOption);
        }


        /// <summary>
        /// Write data to server. cifs/smb implementation of this interface should pay attention to offset.
        /// They may not accept ulong as offset
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="offset">The offset of the file from where client wants to start writing</param>
        /// <param name="data">The data which will be written to server</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint Write(TimeSpan timeout, ulong offset, byte[] data)
        {
            this.internalTimeout = timeout;

            SmbWriteAndxRequestPacket request = smbClient.CreateWriteRequest(this.fid, (uint)offset, data);

            SMB_COM_WRITE_ANDX_Request_SMB_Parameters param = request.SmbParameters;

            // get the high 4 bytes
            param.OffsetHigh = (uint)(offset >> 32);

            request.SmbParameters = param;

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }

            uint status = 0;
            this.SendAndExpectSmbPacket(request, internalTimeout, out status);

            return SmbMessageUtils.CheckStatus(status);
        }


        /// <summary>
        /// Read data from server, start at the positon indicated by offset
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="offset">From where it will read</param>
        /// <param name="length">The length of the data client wants to read</param>
        /// <param name="data">The read data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint Read(TimeSpan timeout, ulong offset, uint length, out byte[] data)
        {
            this.internalTimeout = timeout;

            SmbReadAndxRequestPacket request = smbClient.CreateReadRequest(this.fid, (ushort)length, (uint)offset);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }

            uint status = 0;
            SmbReadAndxResponsePacket response =
                this.SendAndExpectSmbPacket(request, internalTimeout, out status) as SmbReadAndxResponsePacket;

            data = ArrayUtility.SubArray<byte>(response.SmbData.Data, 0);

            return SmbMessageUtils.CheckStatus(status);
        }

        /// <summary>
        /// Execute a transacted exchange against a named pipe. 
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="writeData">the written data to the named pipe</param>
        /// <param name="readData">The read data from the named pipe</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public uint Transaction(TimeSpan timeout, byte[] writeData, out byte[] readData)
        {
            this.internalTimeout = timeout;

            SmbTransTransactNmpipeRequestPacket request = smbClient.CreateTransTransactNamedPipeRequest(this.fid, TransSmbParametersFlags.NONE, writeData, smbClient.Capability.MaxDataCount);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }

            uint status = 0;
            SmbTransTransactNmpipeResponsePacket response =
                this.SendAndExpectSmbPacket(request, internalTimeout, out status) as SmbTransTransactNmpipeResponsePacket;

            readData = ArrayUtility.SubArray<byte>(response.TransData.ReadData, 0);

            return SmbMessageUtils.CheckStatus(status);
        }


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="ioControlCode">The IO control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, uint ioControlCode, byte[] input, out byte[] output)
        {
            this.internalTimeout = timeout;

            return InternalIoControl(ioControlCode, false, input, out output);
        }


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="ioControlCode">The IO control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="inputResponse">The input data in the response of this control operation</param>
        /// <param name="outputResponse">The output data in the response of this control operation</param>
        /// <param name="maxInputResponse">The maximum number of bytes that the server can return for the input data</param>
        /// <param name="maxOutputResponse">The maximum number of bytes that the server can return for the output data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, uint ioControlCode, byte[] input, out byte[] inputResponse,
            out byte[] outputResponse, uint maxInputResponse, uint maxOutputResponse)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Do File system control on server
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="fsControlCode">The file system control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, FsCtlCode fsControlCode, byte[] input, out byte[] output)
        {
            this.internalTimeout = timeout;

            return InternalIoControl((uint)fsControlCode, true, input, out output);
        }


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="fsControlCode">The file system control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="inputResponse">The input data in the response of this control operation</param>
        /// <param name="outputResponse">The output data in the response of this control operation</param>
        /// <param name="maxInputResponse">The maximum number of bytes that the server can return for the input data</param>
        /// <param name="maxOutputResponse">The maximum number of bytes that the server can return for the output data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, FsCtlCode fsControlCode, byte[] input, out byte[] inputResponse,
            out byte[] outputResponse, uint maxInputResponse, uint maxOutputResponse)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Close file, named pipe, directory
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void Close()
        {
            SmbCloseRequestPacket request = smbClient.CreateCloseRequest(this.fid);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }

            uint status = 0;
            this.SendAndExpectSmbPacket(request, internalTimeout, out status);
        }


        /// <summary>
        /// Disconnect share
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void DisconnetShare()
        {
            if (this.smbClient == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }
            SmbPacket request;
            uint status;

            // Tree disconnect:
            request = this.smbClient.CreateTreeDisconnectRequest(this.treeId);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }
            this.SendAndExpectSmbPacket(request, internalTimeout, out status);

            if (status != 0)
            {
                throw new InvalidOperationException("Tree Disconnect Failed. ErrorCode: " + status);
            }

            // Log off:
            request = this.smbClient.CreateLogoffRequest(this.uid);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }
            this.SendAndExpectSmbPacket(request, internalTimeout, out status);

            if (status != 0)
            {
                throw new InvalidOperationException("Log off Failed. ErrorCode: " + status);
            }

            this.smbClient.Disconnect();
        }


        #endregion


        #region Private functions

        /// <summary>
        /// Send and get smb packet
        /// </summary>
        /// <param name="request">The request to server</param>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="status">The status of the response</param>
        /// <returns>The response</returns>
        private SmbPacket SendAndExpectSmbPacket(SmbPacket request, TimeSpan timeout, out uint status)
        {
            this.smbClient.SendPacket(request);
            SmbPacket response = this.smbClient.ExpectPacket(timeout);
            status = response.SmbHeader.Status;

            return response;
        }


         /// <summary>
        /// Do IO control on remote server
        /// </summary>
        /// <param name="ctlCode">The control code</param>
        /// <param name="isFsCtl">Indicate if the control code is a file system control code</param>
        /// <param name="input">The input data of io control</param>
        /// <param name="output">The output data of io control</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown when meet an transport error</exception>
        private uint InternalIoControl(uint ctlCode, bool isFsCtl, byte[] input, out byte[] output)
        {
            SmbNtTransactIoctlRequestPacket request =
                smbClient.CreateNTTransIOCtlRequest(this.fid, isFsCtl, 0x00, ctlCode, input);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }

            uint status = 0;
            SmbNtTransactIoctlResponsePacket response =
                this.SendAndExpectSmbPacket(request, internalTimeout, out status) as SmbNtTransactIoctlResponsePacket;

            output = null;
            if (response != null)
            {
                output = response.NtTransData.Data;
            }

            return SmbMessageUtils.CheckStatus(status);
        }


       /// <summary>
        /// Create File, named pipe, directory. One transport can only create one file.
        /// </summary>
        /// <param name="fileName">The file, namedpipe, directory name</param>
        /// <param name="desiredAccess">The desired access</param>
        /// <param name="impersonationLevel">The impersonation level</param>
        /// <param name="fileAttribute">The file attribute, this field is only valid when create file.
        /// </param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is
        /// specified in the name field already exists</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        private void InternalCreate(string fileName, NtTransactDesiredAccess desiredAccess,
            NtTransactImpersonationLevel impersonationLevel, SMB_EXT_FILE_ATTR fileAttribute,
            NtTransactCreateDisposition createDisposition, NtTransactCreateOptions createOption)
        {
            SmbNtCreateAndxRequestPacket request = smbClient.CreateCreateRequest(
                this.treeId,
                fileName,
                desiredAccess,
                fileAttribute,
                NtTransactShareAccess.NONE,
                createDisposition,
                createOption,
                impersonationLevel,
                CreateFlags.NT_CREATE_REQUEST_EXTENDED_RESPONSE);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }

            uint status = 0;
            SmbNtCreateAndxResponsePacket response =
                this.SendAndExpectSmbPacket(request, internalTimeout, out status) as SmbNtCreateAndxResponsePacket;

            if (status != 0)
            {
                throw new InvalidOperationException("Create Failed. ErrorCode: " + status);
            }

            this.fid = response.SmbParameters.FID;
        }


        /// <summary>
        /// Connect share specified by shareName, this function does not issue tcp or netbios
        /// connect, it does Negotiate -> SessionSetup -> TreeConnect
        /// </summary>
        /// <param name="serverName">The server name</param>
        /// <param name="domain">The domain</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <param name="securityPackage">The security package</param>
        private void InternalConnectShare(string serverName, string domain,
            string userName, string password, string shareName, SecurityPackageType securityPackage)
        {
            SmbPacket request;
            SmbPacket response;
            uint status;

            // Negotiate:
            request = this.smbClient.CreateNegotiateRequest(
                defaultSignState, new string[] { DialectNameString.NTLANMAN });
            response = this.SendAndExpectSmbPacket(request, internalTimeout, out status);

            if (status != 0)
            {
                throw new InvalidOperationException("Negotiate Failed. ErrorCode: " + status);
            }
            SecurityModes securityMode = (response as SmbNegotiateResponsePacket).SmbParameters.SecurityMode;
            this.isSignRequired = (securityMode & SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED)
                == SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED;

            SmbSecurityPackage secPkg;

            switch (securityPackage)
            {
                case SecurityPackageType.Ntlm:
                    secPkg = SmbSecurityPackage.NTLM;
                    break;

                case SecurityPackageType.Kerberos:
                    secPkg = SmbSecurityPackage.Kerberos;
                    break;

                case SecurityPackageType.Negotiate:
                    secPkg = SmbSecurityPackage.Negotiate;
                    break;

                default:
                    throw new ArgumentException("Unsupported securityPackage: " + securityPackage.ToString());
            }


            // Session setup:
            request = this.smbClient.CreateFirstSessionSetupRequest(secPkg, serverName, domain, userName,
                password);
            response = this.SendAndExpectSmbPacket(request, internalTimeout, out status);

            while (status != 0)
            {
                if ((int)status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    this.uid = (response as SmbSessionSetupAndxResponsePacket).SmbHeader.Uid;
                    request = this.smbClient.CreateSecondSessionSetupRequest(this.uid, secPkg);
                    response = this.SendAndExpectSmbPacket(request, internalTimeout, out status);
                }
                else
                {
                    throw new InvalidOperationException("Session Setup Failed. ErrorCode: " + status);
                }
            }
            this.uid = (response as SmbSessionSetupAndxResponsePacket).SmbHeader.Uid;

            if (isSignRequired)
            {
                CifsClientPerSession session = this.smbClient.Context.GetSession(
                    this.smbClient.Context.Connection.ConnectionId, this.uid);
                this.sessionKey = session.SessionKey;
            }

            // Tree connect:
            string sharePath = "\\\\" + serverName + '\\' + shareName;
            request = this.smbClient.CreateTreeConnectRequest(this.uid, sharePath);

            if (this.isSignRequired)
            {
                request.Sign(this.NextSequenceNumber, this.sessionKey);
            }
            response = this.SendAndExpectSmbPacket(request, internalTimeout, out status);

            if (status != 0)
            {
                throw new InvalidOperationException("Tree Connect Failed. ErrorCode: " + status);
            }
            this.treeId = (response as SmbTreeConnectAndxResponsePacket).SmbHeader.Tid;
        }

        #endregion


        #region Dispose

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                }

                // Call the appropriate methods to clean up unmanaged resources.
                if (this.smbClient != null)
                {
                    this.smbClient.Dispose();
                    this.smbClient = null;
                }
                base.Dispose(disposing);
                this.disposed = true;
            }
        }


        #endregion
    }
}
