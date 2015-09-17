// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Used as transport layer of MS-DFSC protocol
    /// </summary>
    public class CifsClientTransport : FileServiceClientTransport
    {
        #region Fields

        private CifsClient cifsClient;
        private CifsClientConfig config;
        private bool disposed;
        private ushort uid;
        private ushort treeId;

        private const byte dialectBufferFormat = 0x02;
        private const string dialectLanMan21 = "LANMAN2.1";
        private const string dialectNtLanMan = "NT LM 0.12";
        private const string nativeOS = "Windows Vista (TM) Enterprise 6001 Service Pack 1";
        private const string nativeLanMan = "Windows Vista (TM) Enterprise 6.0";
        private const string ipcConnectString = "IPC$";
        private const string treeConnectService = "?????";
        private const byte sizeofMaxReferralLevel = 2;

        #endregion


        #region Properties

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
                if (this.cifsClient == null)
                {
                    throw new InvalidOperationException("The transport is not connected.");
                }

                return this.cifsClient.IsDataAvailable;
            }
        }


        /// <summary>
        /// the context of client transport, that contains the runtime states and variables.
        /// </summary>
        public override FileServiceClientContext Context
        {
            get
            {
                return this.cifsClient.Context;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CifsClientTransport()
            : base()
        {
            this.config = new CifsClientConfig();
            this.cifsClient = new CifsClient(this.config);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public CifsClientTransport(CifsClientConfig config)
        {
            this.config = config;
            this.cifsClient = new CifsClient(this.config);
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
                if (this.cifsClient != null)
                {
                    this.cifsClient.Dispose();
                    this.cifsClient = null;
                }
                base.Dispose(disposing);
                this.disposed = true;
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Set up connection with server.
        /// Including 4 steps: 1. Tcp connection 2. Negotiation 3. SessionSetup 4. TreeConnect in order
        /// </summary>
        /// <param name="server">server name of ip address</param>
        /// <param name="client">client name of ip address</param>
        /// <param name="domain">user's domain</param>
        /// <param name="userName">user's name</param>
        /// <param name="password">user's password</param>
        /// <param name="timeout">The pending time to get server's response in step 2, 3 or 4</param>
        /// <exception cref="System.Net.ProtocolViolationException">Fail to set up connection with server</exception>
        public override void Connect(
            string server,
            string client,
            string domain,
            string userName,
            string password,
            TimeSpan timeout,
            SecurityPackageType securityPackage = SecurityPackageType.Ntlm,
            bool useServerToken = false)
        {
            this.cifsClient.Connect(server, client);

            SmbPacket request, response;
            uint status;

            // Negotiate:
            SMB_Dialect dialectLM21 = new SMB_Dialect();
            dialectLM21.BufferFormat = dialectBufferFormat;
            dialectLM21.DialectString = dialectLanMan21;
            SMB_Dialect dialectNTLM = new SMB_Dialect();
            dialectNTLM.BufferFormat = dialectBufferFormat;
            dialectNTLM.DialectString = dialectNtLanMan;
            request = this.cifsClient.CreateNegotiateRequest(new SMB_Dialect[] { dialectLM21, dialectNTLM });
            this.cifsClient.SendPacket(request);
            response = this.cifsClient.ExpectPacket(timeout);
            status = (response as SmbNegotiateResponsePacket).SmbHeader.Status;

            if (status != 0)
            {
                throw new ProtocolViolationException("Negotiate Failed. ErrorCode: " + status);
            }

            // Session setup:
            CifsUserAccount userAccount = new CifsUserAccount(domain, userName, password);
            request = this.cifsClient.CreateSessionSetupRequest(userAccount, nativeOS, nativeLanMan);
            this.cifsClient.SendPacket(request);
            response = this.cifsClient.ExpectPacket(timeout);
            SmbSessionSetupAndxResponsePacket sessionSetupResponse = response as SmbSessionSetupAndxResponsePacket;
            status = sessionSetupResponse.SmbHeader.Status;

            if (status != 0)
            {
                throw new ProtocolViolationException("Session Setup Failed. ErrorCode: " + status);
            }
            this.uid = sessionSetupResponse.SmbHeader.Uid;

            // Tree connect:
            string ipcAddress = "\\\\" + server + '\\' + ipcConnectString;
            request = this.cifsClient.CreateTreeConnectAndxRequest(this.uid, TreeConnectAndxFlags.NONE,
                ipcAddress, treeConnectService, null, null);
            this.cifsClient.SendPacket(request);
            response = this.cifsClient.ExpectPacket(timeout);
            SmbTreeConnectAndxResponsePacket treeConnectResponse = response as SmbTreeConnectAndxResponsePacket;
            status = treeConnectResponse.SmbHeader.Status;

            if (status != 0)
            {
                throw new ProtocolViolationException("Tree Connect Failed. ErrorCode: " + status);
            }
            this.treeId = treeConnectResponse.SmbHeader.Tid;
        }


        /// <summary>
        /// Send a Dfs request to server
        /// 1. Decode payload from byte array to Dfsc REQ_GET_DFS_REFERRAL payload defined in Cifs
        /// 2. Create cifs packet with payload and send out.
        /// </summary>
        /// <param name="payload">REQ_GET_DFS_REFERRAL structure in byte array</param>
        /// <param name="isEX"> 
        /// Boolean indicating whether payload contains REQ_GET_DFS_REFFERAL_EX message.
        /// MUST be false for CIFS transport
        /// </param>
        /// <exception cref="System.ArgumentNullException">the payload to be sent is null.</exception>
        /// <exception cref="ArgumentException">extended request packet cannot be in payload</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public override void SendDfscPayload(byte[] payload, bool isEX)
        {
            if (isEX)
            {
                throw new ArgumentException("REQ_DFS_GET_REFERRAL_EX message not supported over CIFS transport");
            }

            if (this.cifsClient == null)
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

            Array.Copy(payload, sizeofMaxReferralLevel, referral.RequestFileName, 0, referral.RequestFileName.Length
                - sizeofMaxReferralLevel);

            SmbPacket request = this.cifsClient.CreateTrans2GetDfsReferalRequest(
                this.uid,
                this.treeId,
                "",
                referral);
            this.cifsClient.SendPacket(request);
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
            if (this.cifsClient == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }

            SmbTrans2GetDfsReferalFinalResponsePacket response = (SmbTrans2GetDfsReferalFinalResponsePacket)
                this.cifsClient.ExpectPacket(timeout);
            status = response.SmbHeader.Status;
            payload = response.SmbData.Trans2_Data;
        }


        /// <summary>
        /// Disconnect from server.
        /// Including 3 steps: 1. TreeDisconnect 2. Logoff 3. Tcp disconnection in order.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response in step 1 or 2</param>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        /// <exception cref="System.Net.ProtocolViolationException">Fail to disconnect from server</exception>
        public override void Disconnect(TimeSpan timeout)
        {
            if (this.cifsClient == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }
            SmbPacket request, response;
            uint status;

            // Tree disconnect:
            request = this.cifsClient.CreateTreeDisconnectRequest(this.uid, this.treeId);
            this.cifsClient.SendPacket(request);
            response = this.cifsClient.ExpectPacket(timeout);
            status = (response as SmbTreeDisconnectResponsePacket).SmbHeader.Status;

            if (status != 0)
            {
                throw new ProtocolViolationException("Tree Disconnect Failed. ErrorCode: " + status);
            }

            // Log off:
            request = this.cifsClient.CreateLogoffRequest(this.uid);
            this.cifsClient.SendPacket(request);
            response = this.cifsClient.ExpectPacket(timeout);
            status = (response as SmbLogoffAndxResponsePacket).SmbHeader.Status;

            if (status != 0)
            {
                throw new ProtocolViolationException("Log off Failed. ErrorCode: " + status);
            }

            this.cifsClient.Disconnect();
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
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void ConnectShare(string serverName, int port, IpVersion ipVersion, string domain,
            string userName, string password, string shareName)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void Create(string fileName, FsFileDesiredAccess desiredAccess, FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute, FsCreateDisposition createDisposition, FsCreateOption createOption)
        {
            throw new NotImplementedException();
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
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void Create(string directoryName, FsDirectoryDesiredAccess desiredAccess, FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute, FsCreateDisposition createDisposition, FsCreateOption createOption)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Write data to server. cifs/smb implementation of this interface should pay attention to offset.
        /// They may not accept ulong as offset
        /// </summary>
        /// <param name="timeout">The waiting time of this operation</param>
        /// <param name="offset">The offset of the file from where client wants to start writing</param>
        /// <param name="data">The data which will be written to server</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint Write(TimeSpan timeout, ulong offset, byte[] data)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Read data from server, start at the positon indicated by offset
        /// </summary>
        /// <param name="timeout">The waiting time of this operation</param>
        /// <param name="offset">From where it will read</param>
        /// <param name="length">The length of the data client wants to read</param>
        /// <param name="data">The read data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint Read(TimeSpan timeout, ulong offset, uint length, out byte[] data)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// for that use, use FileSystemControl() function instead
        /// </summary>
        /// <param name="timeout">The waiting time of this operation</param>
        /// <param name="controlCode">The IO control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, uint controlCode, byte[] input, out byte[] output)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// for that use, use FileSystemControl() function instead
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The IO control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="inputResponse">The input data in the response of this control operation</param>
        /// <param name="outputResponse">The output data in the response of this control operation</param>
        /// <param name="maxInputResponse">The maximum number of bytes that the server can return for the input data</param>
        /// <param name="maxOutputResponse">The maximum number of bytes that the server can return for the output data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, uint controlCode, byte[] input, out byte[] inputResponse,
            out byte[] outputResponse, uint maxInputResponse, uint maxOutputResponse)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Do File system control on server
        /// </summary>
        /// <param name="timeout">The waiting time of this operation</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, FsCtlCode controlCode, byte[] input, out byte[] output)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Do File system control on server
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="inputResponse">The input data in the response of this control operation</param>
        /// <param name="outputResponse">The output data in the response of this control operation</param>
        /// <param name="maxInputResponse">The maximum number of bytes that the server can return for the input data</param>
        /// <param name="maxOutputResponse">The maximum number of bytes that the server can return for the output data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, FsCtlCode controlCode, byte[] input, out byte[] inputResponse,
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
            throw new NotImplementedException();
        }


        /// <summary>
        /// Disconnect share, including treedisconnect, logoff and tcp disconnect
        /// </summary>
        public override void DisconnetShare()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
