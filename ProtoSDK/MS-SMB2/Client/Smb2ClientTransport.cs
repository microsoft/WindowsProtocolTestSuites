// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System.Text.RegularExpressions;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiService;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// SMB2 client transport layer.
    /// </summary>
    public class Smb2ClientTransport : FileServiceClientTransport
    {
        #region Fields

        protected Smb2Client client;

        private ulong messageId;
        private ulong sessionId;
        private Queue<ulong> ioctlRequestMessageIds = new Queue<ulong>();
        private uint treeId;
        private FILEID fileId = FILEID.Zero;
        private DialectRevision negotiatedDialect;
        private Capabilities_Values serverCapabilities;
        private bool disposed = false;
        private readonly Guid clientGuid = Guid.NewGuid();

        private string serverPrincipleName;
        private Smb2ClientGlobalContext context = new Smb2ClientGlobalContext();

        //If user does not specify timeout value, this will be used
        private TimeSpan internalTimeout;

        //The share name of ipc
        private const string IPC_CONNECT_STRING = "IPC$";

        // [MS-SMB2] section 3.2.4.20.1 Application Requests Enumeration of Previous Versions   
        // The MaxInputResponse field is set to 0.  
        private const uint IOCTL_DEFAULT_MAX_INPUT_RESPONSE = 0;

        // [MS-SMB2] section 3.2.4.20.1 Application Requests Enumeration of Previous Versions
        // The MaxOutputResponse field is set to the maximum output buffer size that the application will accept.
        private const uint IOCTL_DEFAULT_MAX_OUTPUT_RESPONSE = 4096;

        // [MS-SMB2] section 3.2.4.20 Application Requests an IO Control Code Operation
        // If Connection.SupportsMultiCredit is TRUE, the CreditCharge field in the SMB2 header SHOULD<123> be set to (max(InputCount, MaxOutputResponse) – 1) / 65536 + 1.
        private const ushort IOCTL_DEFAULT_CREDIT_CHARGE = 1;

        //The internal timeout seconds
        private const int INTERNAL_TIMEOUT_SECS = 20;

        // Flag of packet header.
        private Packet_Header_Flags_Values headerFlags = Packet_Header_Flags_Values.NONE;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2ClientTransport()
            : base()
        {
            internalTimeout = new TimeSpan(0, 0, INTERNAL_TIMEOUT_SECS);
            client = new Smb2Client(internalTimeout);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2ClientTransport(TimeSpan timeout)
            : base()
        {
            internalTimeout = timeout;
            client = new Smb2Client(internalTimeout);
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    CloseFile();

                    // Call the appropriate methods to clean up unmanaged resources.
                    if (client != null)
                    {
                        client.Dispose();
                        client = null;
                    }
                }
                this.disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Methods

        protected virtual uint TreeConnect(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            string server,
            string share,
            out Packet_Header header,
            out TREE_CONNECT_Response response)
        {
            return client.TreeConnect(
                        creditCharge,
                        creditRequest,
                        flags,
                        messageId,
                        sessionId,
                        string.Format(@"\\{0}\{1}", server, share),
                        out treeId,
                        out header,
                        out response);
        }

        protected virtual uint Negotiate(
            ushort creditCharge,
            ushort creditRequest,
            ulong messageId,
            Guid clientGuid,
            out DialectRevision selectedDialect,
            out byte[] gssToken,
            out Packet_Header responseHeader,
            out NEGOTIATE_Response responsePayload)
        {
            uint status = client.Negotiate(
                            creditCharge,
                            creditRequest,
                            Packet_Header_Flags_Values.NONE,
                            messageId,
                            new DialectRevision[] { DialectRevision.Smb2002 },
                            SecurityMode_Values.NONE,
                            Capabilities_Values.NONE,
                            clientGuid,
                            out selectedDialect,
                            out gssToken,
                            out responseHeader,
                            out responsePayload);

            negotiatedDialect = selectedDialect;

            serverCapabilities = (Capabilities_Values)responsePayload.Capabilities;

            return status;
        }

        public void OpenFile(
            string server,
            string share,
            string file,
            SecurityPackageType securityPackageType,
            string domainName,
            string userName,
            string password,
            AccessMask accessMask)
        {
            IPAddress address = null;
            if(IPAddress.TryParse(server, out address)) // server is a ip address
            {
                client.ConnectOverTCP(address);
                serverPrincipleName = server;
            }
            else
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(server);
                client.ConnectOverTCP(hostEntry.AddressList[0]);
                serverPrincipleName = Smb2Utility.GetPrincipleName(hostEntry.HostName);
            }

            Packet_Header header;
            NEGOTIATE_Response negotiateResponse;
            DialectRevision selectedDialect;
            byte[] serverGssToken;

            CheckStatusCode(
                Negotiate(
                1,
                1,
                messageId++,
                clientGuid,
                out selectedDialect,
                out serverGssToken,
                out header,
                out negotiateResponse));

            // 3.2.5.2: If the SecurityMode field in the SMB2 header of the response has the SMB2_NEGOTIATE_SIGNING_REQUIRED bit set, 
            // the client MUST set Connection.RequireSigning to TRUE.
            // 3.2.5.3.1: If the global setting RequireMessageSigning is set to TRUE or 
            // Connection.RequireSigning is set to TRUE then Session.SigningRequired MUST be set to TRUE
            bool session_SigningRequired = negotiateResponse.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);
            if (session_SigningRequired)
            {
                // 3.2.4.1.1: If the client signs the request, it MUST set the SMB2_FLAGS_SIGNED bit in the Flags field of the SMB2 header.
                headerFlags |= Packet_Header_Flags_Values.FLAGS_SIGNED;
            }

            SESSION_SETUP_Response sessionSetupResponse;

            SspiClientSecurityContext sspiClientGss =
                new SspiClientSecurityContext(
                    securityPackageType,
                    new AccountCredential(domainName, userName, password),
                    Smb2Utility.GetCifsServicePrincipalName(server),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            if (securityPackageType == SecurityPackageType.Negotiate)
                sspiClientGss.Initialize(serverGssToken);
            else
                sspiClientGss.Initialize(null);

            uint status;
            do
            {
                status = client.SessionSetup(
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NONE,
                    SESSION_SETUP_Request_Capabilities_Values.NONE,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out header,
                    out sessionSetupResponse);

                CheckStatusCode(status);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            client.GenerateCryptoKeys(
                sessionId,
                sspiClientGss.SessionKey,
                session_SigningRequired,
                sessionSetupResponse.SessionFlags.HasFlag(SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA)); // Encrypt the session if server required.

            TREE_CONNECT_Response treeConnectResponse;

            CheckStatusCode(
                TreeConnect(
                1,
                1,
                headerFlags,
                messageId++,
                sessionId,
                server,
                share,
                out header,
                out treeConnectResponse));

            CREATE_Response createResponse;
            Smb2CreateContextResponse[] serverCreateContexts;

            CheckStatusCode(
                client.Create(
                    1,
                    1,
                    headerFlags,
                    messageId++,
                    sessionId,
                    treeId,
                    file,
                    accessMask,
                    ShareAccess_Values.FILE_SHARE_READ,
                    CreateOptions_Values.NONE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse));
        }

        public void CloseFile()
        {
            if (fileId.Equals(FILEID.Zero))
            {
                return;
            }

            Packet_Header header;
            TREE_DISCONNECT_Response treeDisconnectResponse;

            CheckStatusCode(
                client.TreeDisconnect(
                    1,
                    1,
                    headerFlags,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse));

            LOGOFF_Response logoffResponse;

            CheckStatusCode(
                client.LogOff(
                    1,
                    1,
                    headerFlags,
                    messageId++,
                    sessionId,
                    out header,
                    out logoffResponse));

            client.Disconnect();

            fileId = FILEID.Zero;
        }

        public byte[] ReadAllBytes()
        {
            uint status;
            ulong offset = 0;
            List<byte> content = new List<byte>();

            do
            {
                Packet_Header header;
                READ_Response readResponse;
                byte[] block;

                status = client.Read(
                    1,
                    1,
                    headerFlags,
                    messageId++,
                    sessionId,
                    treeId,
                    32 * 1024,
                    offset,
                    fileId,
                    0,
                    Channel_Values.CHANNEL_NONE,
                    0,
                    null,
                    out block,
                    out header,
                    out readResponse
                    );

                CheckStatusCode(status);

                offset += readResponse.DataLength;

                if (block != null && block.Length > 0)
                    content.AddRange(block);
            }
            while (status != Smb2Status.STATUS_END_OF_FILE);

            return content.ToArray();
        }

        public uint ReadHash(
            SRV_READ_HASH_Request_HashType_Values hashType,
            SRV_READ_HASH_Request_HashVersion_Values hashVersion,
            SRV_READ_HASH_Request_HashRetrievalType_Values hashRetrievalType,
            ulong offset,
            uint length,
            out HASH_HEADER hashHeader,
            out byte[] hashData)
        {
            hashHeader = new HASH_HEADER();
            hashData = null;

            SRV_READ_HASH_Request readHashRequest = new SRV_READ_HASH_Request();
            readHashRequest.HashType = hashType;
            readHashRequest.HashVersion = hashVersion;
            readHashRequest.HashRetrievalType = hashRetrievalType;
            readHashRequest.Offset = offset;
            readHashRequest.Length = length;

            byte[] requestInput = TypeMarshal.ToBytes(readHashRequest);

            byte[] responseOutput;

            uint status;
            SendIoctlPayload(CtlCode_Values.FSCTL_SRV_READ_HASH, requestInput);
            ExpectIoctlPayload(out status, out responseOutput);

            if (status != Smb2Status.STATUS_SUCCESS)
                return status;

            byte[] hashHeaderDataBuffer = null;
            switch (hashRetrievalType)
            {
                case SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED:
                    hashHeaderDataBuffer = TypeMarshal.ToStruct<SRV_HASH_RETRIEVE_HASH_BASED>(responseOutput).Buffer;
                    break;

                case SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED:
                    hashHeaderDataBuffer = TypeMarshal.ToStruct<SRV_HASH_RETRIEVE_FILE_BASED>(responseOutput).Buffer;
                    break;

                default:
                    throw new NotImplementedException();
            }

            int hashHeaderLength = 0;
            hashHeader = TypeMarshal.ToStruct<HASH_HEADER>(hashHeaderDataBuffer, ref hashHeaderLength);
            hashData = hashHeaderDataBuffer.Skip(hashHeaderLength).ToArray();

            return status;
        }

        public void SendIoctlPayload(CtlCode_Values code, byte[] payload)
        {
            if (this.client == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }

            if (payload == null)
            {
                throw new ArgumentNullException("payload");
            }

            var request = new Smb2IOCtlRequestPacket();

            request.Header.CreditCharge = IOCTL_DEFAULT_CREDIT_CHARGE;
            request.Header.Command = Smb2Command.IOCTL;
            request.Header.CreditRequestResponse = 1;
            request.Header.Flags = headerFlags;
            request.Header.MessageId = messageId++;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;

            request.PayLoad.CtlCode = code;

            if (code == CtlCode_Values.FSCTL_DFS_GET_REFERRALS || code == CtlCode_Values.FSCTL_DFS_GET_REFERRALS_EX)
            {
                request.PayLoad.FileId = FILEID.Invalid;
            }
            else
            {
                request.PayLoad.FileId = this.fileId;
            }

            if (payload.Length > 0)
            {
                request.PayLoad.InputOffset = request.BufferOffset;
                request.PayLoad.InputCount = (ushort)payload.Length;
                request.Buffer = payload;
            }

            request.PayLoad.MaxInputResponse = IOCTL_DEFAULT_MAX_INPUT_RESPONSE;
            request.PayLoad.MaxOutputResponse = IOCTL_DEFAULT_MAX_OUTPUT_RESPONSE;
            request.PayLoad.Flags = IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL;

            ioctlRequestMessageIds.Enqueue(request.Header.MessageId);
            client.SendPacket(request);
        }

        public void ExpectIoctlPayload(out uint status, out byte[] payload)
        {
            if (this.client == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }
            Smb2IOCtlResponsePacket response = client.ExpectPacket<Smb2IOCtlResponsePacket>(ioctlRequestMessageIds.Dequeue());

            payload = null;
            if (response.PayLoad.OutputCount > 0)
            {
                payload = response.Buffer.Skip((int)(response.PayLoad.OutputOffset - response.BufferOffset)).Take((int)response.PayLoad.OutputCount).ToArray();
            }

            status = response.Header.Status;
        }

        #endregion

        private void CheckStatusCode(uint status)
        {
            if (status != Smb2Status.STATUS_SUCCESS &&
                status != Smb2Status.STATUS_PENDING &&
                status != Smb2Status.STATUS_MORE_PROCESSING_REQUIRED &&
                status != Smb2Status.STATUS_END_OF_FILE)
                throw new InvalidOperationException("Operation failed with error code " + Smb2Status.GetStatusCode(status));
        }


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
                return this.client.IsDataAvailable;
            }
        }


        /// <summary>
        /// the context of client transport, that contains the runtime states and variables.
        /// </summary>
        public override FileServiceClientContext Context
        {
            get
            {
                return context;
            }
        }

        public Packet_Header_Flags_Values HeaderFlags
        {
            get
            {
                return headerFlags;
            }
            set
            {
                headerFlags = value;
            }
        }

        #endregion

        #region Methods For Dfsc

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
            SecurityPackageType securityPackage,
            bool useServerToken)
        {
            serverPrincipleName = server;
            this.client.ConnectOverTCP(Dns.GetHostAddresses(server).First());
            InternalConnectShare(domain, userName, password, IPC_CONNECT_STRING, timeout, securityPackage, useServerToken);
        }


        /// <summary>
        /// Disconnect from server.
        /// Including 3 steps: 1. TreeDisconnect 2. Logoff 3. Tcp disconnection in order.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response in step 1 or 2</param>
        /// <exception cref="System.Net.ProtocolViolationException">Fail to disconnect from server</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public override void Disconnect(TimeSpan timeout)
        {
            if (this.client == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }
            uint status;

            // Tree disconnect:
            Packet_Header header;
            TREE_DISCONNECT_Response treeDisconnectResponse;

            status = client.TreeDisconnect(
                1,
                1,
                headerFlags,
                messageId++,
                sessionId,
                treeId,
                out header,
                out treeDisconnectResponse);

            if (status != 0)
            {
                throw new ProtocolViolationException("Tree Disconnect Failed. ErrorCode: " + status);
            }

            // Log off:
            LOGOFF_Response logoffResponse;

            status = client.LogOff(
                1,
                1,
                headerFlags,
                messageId++,
                sessionId,
                out header,
                out logoffResponse);

            if (status != 0)
            {
                throw new ProtocolViolationException("Log off Failed. ErrorCode: " + status);
            }

            this.client.Disconnect();

            fileId = FILEID.Zero;
        }


        /// <summary>
        /// Send a Dfs request to server
        /// </summary>
        /// <param name="payload">REQ_GET_DFS_REFERRAL or REQ_GET_DFS_REFERRAL_EX structure in byte array</param>
        /// <param name="isEX">
        /// Boolean which indicates whether payload contains DFS_GET_REFERRAL_EX message
        /// </param>
        /// <exception cref="System.ArgumentNullException">the payload to be sent is null.</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public override void SendDfscPayload(byte[] payload, bool isEX)
        {
            SendIoctlPayload((isEX) ? CtlCode_Values.FSCTL_DFS_GET_REFERRALS_EX : CtlCode_Values.FSCTL_DFS_GET_REFERRALS, payload);
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
            ExpectIoctlPayload(out status, out payload);
        }

        #endregion

        #region Used for RSVD
        /// <summary>
        /// Connect to a share indicated by shareName in server
        /// This will use smb over tcp as transport. Only one server
        /// can be connected at one time
        /// </summary>
        /// <param name="serverName">The server Name</param>
        /// <param name="serverIP">The ip of server</param>
        /// <param name="domain">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <param name="securityPackage">The security package</param>
        /// <param name="useServerToken">Whether to use token from server</param> 
        public void ConnectShare(string serverName, IPAddress serverIP, string domain,
            string userName, string password, string shareName, SecurityPackageType securityPackage, bool useServerToken)
        {
            serverPrincipleName = serverName;
            client.ConnectOverTCP(serverIP);
            InternalConnectShare(domain, userName, password, shareName, internalTimeout, securityPackage, useServerToken);
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
            List<IPAddress> serverAddresses = new List<IPAddress>();
            IPAddress ipAddress;
            if (IPAddress.TryParse(serverName, out ipAddress)) // server is a ip address
            {
                serverPrincipleName = serverName;
                serverAddresses.Add(ipAddress);
            }
            else
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(serverName);
                serverPrincipleName = Smb2Utility.GetPrincipleName(hostEntry.HostName);
                serverAddresses.AddRange(hostEntry.AddressList);
            }

            if (ipVersion == IpVersion.Ipv4)
            {
                foreach (var ip in serverAddresses)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        client.ConnectOverTCP(ip);
                        break;
                    }
                }
            }
            else if (ipVersion == IpVersion.Ipv6)
            {
                foreach (var ip in serverAddresses)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        client.ConnectOverTCP(ip);
                        break;
                    }
                }
            }
            else
            {
                // if specified the IpVersion.Any, try ipv4 first, if failed, try ipv6
                try
                {
                    foreach (var ip in serverAddresses)
                    {
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            client.ConnectOverTCP(ip);
                            break;
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    foreach (var ip in serverAddresses)
                    {
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            client.ConnectOverTCP(ip);
                            break;
                        }
                    }
                }
            }

            if (!client.IsConnected)
            {
                throw new InvalidOperationException("No valid IP address can be used to connect to the server.");
            }

            InternalConnectShare(domain, userName, password, shareName, internalTimeout,
                securityPackage, useServerToken);
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
            serverPrincipleName = Smb2Utility.GetPrincipleName(serverNetBiosName);
            client.ConnectOverNetbios(serverNetBiosName);
            InternalConnectShare(domain, userName, password, shareName, internalTimeout,
                SecurityPackageType.Ntlm, false);
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
            if (createOption.HasFlag(FsCreateOption.FILE_DIRECTORY_FILE))
            {
                throw new ArgumentException("createOption cannot contain FILE_DIRECTORY_FILE when creating file.");
            }

            InternalCreate(fileName, (uint)desiredAccess, impersonationLevel, fileAttribute, createDisposition, createOption);
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
            if (createOption.HasFlag(FsCreateOption.FILE_NON_DIRECTORY_FILE))
            {
                throw new ArgumentException("createOption cannot contain FILE_NON_DIRECTORY_FILE when creating Directory.");
            }

            InternalCreate(directoryName, (uint)desiredAccess, impersonationLevel, fileAttribute, createDisposition, createOption);
        }

        /// <summary>
        /// Create a file with create contexts
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="desiredAccess">The desired access</param>
        /// <param name="shareAccess">Sharing mode for the open</param>
        /// <param name="impersonationLevel">The impersonation level</param>
        /// <param name="fileAttribute">The file attribute, this field is only valid when create file.</param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is specified in the name field already exists</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <param name="contextRequest">Create contexts to be sent in the create request</param>
        /// <param name="status">Status of the response packet</param>
        /// <param name="contextResponse">Create contexts to be received in the create response</param>
        public void Create(
            string fileName,
            FsFileDesiredAccess desiredAccess,
            ShareAccess_Values shareAccess,
            FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute,
            FsCreateDisposition createDisposition,
            FsCreateOption createOption,
            Smb2CreateContextRequest[] contextRequest,
            out uint status,
            out CREATE_Response contextResponse)
        {
            if (createOption.HasFlag(FsCreateOption.FILE_DIRECTORY_FILE))
            {
                throw new ArgumentException("createOption can not contain FILE_DIRECTORY_FILE when creating file.");
            }

            Packet_Header header;
            Smb2CreateContextResponse[] serverCreateContexts;

            status = client.Create(
                        1,
                        1,
                        headerFlags,
                        messageId++,
                        sessionId,
                        treeId,
                        fileName,
                        (AccessMask)desiredAccess,
                        shareAccess,
                        (CreateOptions_Values)createOption,
                        (CreateDisposition_Values)createDisposition,
                        (File_Attributes)fileAttribute,
                        (ImpersonationLevel_Values)impersonationLevel,
                        SecurityFlags_Values.NONE,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                        contextRequest,
                        out fileId,
                        out serverCreateContexts,
                        out header,
                        out contextResponse);
        }

        /// <summary>
        /// Create a file with create context
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="desiredAccess">The desired access</param>
        /// <param name="shareAccess">Sharing mode for the open</param>
        /// <param name="impersonationLevel">The impersonation level</param>
        /// <param name="fileAttribute">The file attribute, this field is only valid when create file.</param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is specified in the name field already exists</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <param name="contextRequest">Create contexts to be sent in the create request</param>
        /// <param name="status">Status of the response packet</param>
        /// <param name="serverCreateContexts">Create contexts to be received in the create response</param>
        /// <param name="contextResponse">Create response payload to be received in the create response</param>
        public void Create(
            string fileName,
            FsFileDesiredAccess desiredAccess,
            ShareAccess_Values shareAccess,
            FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute,
            FsCreateDisposition createDisposition,
            FsCreateOption createOption,
            Smb2CreateContextRequest[] contextRequest,
            out uint status,
            out Smb2CreateContextResponse[] serverCreateContexts,
            out CREATE_Response createResponse)
        {
            if (createOption.HasFlag(FsCreateOption.FILE_DIRECTORY_FILE))
            {
                throw new ArgumentException("createOption can not contain FILE_DIRECTORY_FILE when creating file.");
            }

            Packet_Header header;

            status = client.Create(
                        1,
                        1,
                        headerFlags,
                        messageId++,
                        sessionId,
                        treeId,
                        fileName,
                        (AccessMask)desiredAccess,
                        shareAccess,
                        (CreateOptions_Values)createOption,
                        (CreateDisposition_Values)createDisposition,
                        (File_Attributes)fileAttribute,
                        (ImpersonationLevel_Values)impersonationLevel,
                        SecurityFlags_Values.NONE,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                        contextRequest,
                        out fileId,
                        out serverCreateContexts,
                        out header,
                        out createResponse);
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
            Packet_Header header;
            WRITE_Response writeResponse;

            ushort creditCharge = Smb2Utility.CalculateCreditCharge((uint)data.Length, negotiatedDialect, serverCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_LARGE_MTU));

            uint status = client.Write(
                creditCharge,
                64,
                headerFlags,
                messageId,
                sessionId,
                treeId,
                offset,
                fileId,
                Channel_Values.CHANNEL_NONE,
                WRITE_Request_Flags_Values.None,
                new byte[0],
                data,
                out header,
                out writeResponse);

            messageId = (creditCharge == 0 ? messageId+1 : (messageId += (ulong)creditCharge));

            return status;
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
            Packet_Header header;
            READ_Response readResponse;

            ushort creditCharge = Smb2Utility.CalculateCreditCharge(length, negotiatedDialect, serverCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_LARGE_MTU));

            uint status = client.Read(
                creditCharge,
                64,
                headerFlags,
                messageId,
                sessionId,
                treeId,
                length,
                offset,
                fileId,
                0,
                Channel_Values.CHANNEL_NONE,
                0,
                new byte[0],
                out data,
                out header,
                out readResponse);

            messageId = (creditCharge == 0 ? messageId + 1 : (messageId += (ulong)creditCharge));

            return status;
        }


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// for that use, use FileSystemControl() function instead
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The IO control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, uint controlCode, byte[] input, out byte[] output)
        {
            return InternalIoControl(timeout, controlCode, false, input, out output);
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
            return InternalIoControl(timeout, controlCode, false, input, out inputResponse,
                out outputResponse, maxInputResponse, maxOutputResponse);
        }


        /// <summary>
        /// Do File system control on server
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override uint IoControl(TimeSpan timeout, FsCtlCode controlCode, byte[] input, out byte[] output)
        {
            return InternalIoControl(timeout, (uint)controlCode, true, input, out output);
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
            return InternalIoControl(timeout, (uint)controlCode, true, input, out inputResponse,
                out outputResponse, maxInputResponse, maxOutputResponse);
        }


        /// <summary>
        /// Close file, named pipe, directory
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void Close()
        {
            Packet_Header header;
            CLOSE_Response closeResponse;

            uint status = client.Close(
                1,
                1,
                headerFlags,
                messageId++,
                sessionId,
                treeId,
                fileId,
                Flags_Values.NONE,
                out header,
                out closeResponse);

            if (status != 0)
            {
                throw new InvalidOperationException(
                    string.Format("Fails with error code: 0x{0:x}", status));
            }
        }


        /// <summary>
        /// Disconnect share
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public override void DisconnetShare()
        {
            Disconnect(internalTimeout);
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// Connect to the share, not including tcp or netbios connect process
        /// </summary>
        /// <param name="domain">The domain</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <param name="timeout">The waiting time for response</param>
        /// <param name="securityPackage">The security package</param>
        /// <param name="useServerToken">Whether to use token from server</param>
        /// <exception cref="System.Net.ProtocolViolationException">Thrown when meets a connection error</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void InternalConnectShare(string domain, string userName, string password, string shareName, TimeSpan timeout,
            SecurityPackageType securityPackage, bool useServerToken)
        {
            uint status;
            DialectRevision selectedDialect;
            Packet_Header header;
            byte[] serverGssToken;

            Array allDialects = Enum.GetValues(typeof(DialectRevision));
            DialectRevision[] validDialects = new DialectRevision[allDialects.Length - 2];
            int index = 0;
            foreach (var dialect in allDialects)
            {
                if ((DialectRevision)dialect != DialectRevision.Smb2Unknown && (DialectRevision)dialect != DialectRevision.Smb2Wildcard)
                {
                    validDialects[index++] = (DialectRevision)dialect;
                }
            }

            PreauthIntegrityHashID[] preauthIntegrityHashIDArray = null;
            EncryptionAlgorithm[] encryptionAlgorithmArray = null;
            if (validDialects.Contains(DialectRevision.Smb311))
            {
                preauthIntegrityHashIDArray = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgorithmArray = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_GCM, EncryptionAlgorithm.ENCRYPTION_AES128_CCM };
            }

            // Negotiate:
            NEGOTIATE_Response negotiateResponse;
            CheckStatusCode(
                client.Negotiate(
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    messageId++,
                    // Will negotiate highest dialect server supports  
                    validDialects,
                    SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_ENCRYPTION | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                    clientGuid,
                    out selectedDialect,
                    out serverGssToken,
                    out header,
                    out negotiateResponse,
                    preauthHashAlgs: preauthIntegrityHashIDArray,
                    encryptionAlgs: encryptionAlgorithmArray));

            negotiatedDialect = selectedDialect;

            serverCapabilities = (Capabilities_Values)negotiateResponse.Capabilities;

            // 3.2.5.2: If the SecurityMode field in the SMB2 header of the response has the SMB2_NEGOTIATE_SIGNING_REQUIRED bit set, 
            // the client MUST set Connection.RequireSigning to TRUE.
            // 3.2.5.3.1: If the global setting RequireMessageSigning is set to TRUE or 
            // Connection.RequireSigning is set to TRUE then Session.SigningRequired MUST be set to TRUE
            bool session_SigningRequired = negotiateResponse.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);
            if (session_SigningRequired)
            {
                // 3.2.4.1.1: If the client signs the request, it MUST set the SMB2_FLAGS_SIGNED bit in the Flags field of the SMB2 header.
                headerFlags |= Packet_Header_Flags_Values.FLAGS_SIGNED;
            }

            // Session setup:
            SESSION_SETUP_Response sessionSetupResponse;

            SspiClientSecurityContext sspiClientGss =
                new SspiClientSecurityContext(
                    securityPackage,
                    new AccountCredential(domain, userName, password),
                    Smb2Utility.GetCifsServicePrincipalName(serverPrincipleName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            if (securityPackage == SecurityPackageType.Negotiate)
                sspiClientGss.Initialize(serverGssToken);
            else
                sspiClientGss.Initialize(null);

            do
            {
                status = client.SessionSetup(
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out header,
                    out sessionSetupResponse);

                CheckStatusCode(status);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            // 3.2.4.1.1 If Connection.Dialect is "3.1.1" and the message being sent is a TREE_CONNECT Request and the session identified by SessionId has Session.EncryptData equal to FALSE
            bool treeconnect_SigningRequired = session_SigningRequired || (selectedDialect >= DialectRevision.Smb311);
            client.GenerateCryptoKeys(sessionId, sspiClientGss.SessionKey, treeconnect_SigningRequired, sessionSetupResponse.SessionFlags.HasFlag(SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA));

            this.sessionId = header.SessionId;

            // Session Key will be used in the MS-LSA SDK, see LsaClient.cs Line 179 SessionKey
            // Insert the session key to the global context
            Smb2ClientSession smb2CliSession = new Smb2ClientSession();
            smb2CliSession.SessionKey = client.GetSessionKeyForAuthenticatedContext(sessionId);

            Smb2ClientConnection smb2CliConn = new Smb2ClientConnection();
            smb2CliConn.SessionTable = new Dictionary<ulong, Smb2ClientSession>();
            smb2CliConn.SessionTable.Add(sessionId, smb2CliSession);

            context.ConnectionTable = new Dictionary<string, Smb2ClientConnection>();
            context.ConnectionTable.Add("Smb2ClientConnection", smb2CliConn);

            // Tree connect:
            TREE_CONNECT_Response treeConnectResponse;

            status = client.TreeConnect(
                    1,
                    1,
                    treeconnect_SigningRequired ? headerFlags | Packet_Header_Flags_Values.FLAGS_SIGNED : headerFlags,
                    messageId++,
                    sessionId,
                    "\\\\" + serverPrincipleName + "\\" + shareName,
                    out treeId,
                    out header,
                    out treeConnectResponse);

            this.treeId = header.TreeId;

            // For the messages followed by TREE_CONNECT, set them as signed/not signed following the normal proces
            client.EnableSessionSigningAndEncryption(sessionId, session_SigningRequired, sessionSetupResponse.SessionFlags.HasFlag(SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA));
        }


        /// <summary>
        /// Do IO control on remote server
        /// </summary>
        /// <param name="timeout">The waiting time for response</param>
        /// <param name="ctlCode">The control code</param>
        /// <param name="isFsCtl">Indicate if the control code is a file system control code</param>
        /// <param name="input">The input data of io control</param>
        /// <param name="output">The output data of io control</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Throw when meet an transport error</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private uint InternalIoControl(TimeSpan timeout, uint ctlCode, bool isFsCtl, byte[] input, out byte[] output)
        {
            Packet_Header header;
            IOCTL_Response ioCtlResponse;
            byte[] inputResponse;

            IOCTL_Request_Flags_Values ioCtlRequestFlag = IOCTL_Request_Flags_Values.NONE;

            if (isFsCtl)
            {
                ioCtlRequestFlag = IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL;
            }

            return client.IoCtl(
                1,
                1,
                headerFlags,
                messageId++,
                sessionId,
                treeId,
                (CtlCode_Values)ctlCode,
                fileId,
                IOCTL_DEFAULT_MAX_INPUT_RESPONSE,
                input,
                IOCTL_DEFAULT_MAX_OUTPUT_RESPONSE,
                ioCtlRequestFlag,
                out inputResponse,
                out output,
                out header,
                out ioCtlResponse);
        }


        /// <summary>
        /// Do IO control on remote server
        /// </summary>
        /// <param name="timeout">The waiting time for response</param>
        /// <param name="ctlCode">The control code</param>
        /// <param name="isFsCtl">Indicate if the control code is a file system control code</param>
        /// <param name="input">The input data of io control</param>
        /// <param name="inputResponse">The input data of io control response</param>
        /// <param name="outputResponse">The output data of io control response</param>
        /// <param name="maxInputResponse">The maximum number of bytes that the server can return
        /// for the input data in the SMB2 IOCTL Response</param>
        /// <param name="maxOutputResponse">The maximum number of bytes that the server can return
        /// for the output data in the SMB2 IOCTL Response</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Throw when meet an transport error</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private uint InternalIoControl(TimeSpan timeout, uint ctlCode, bool isFsCtl, byte[] input, out byte[] inputResponse,
            out byte[] outputResponse, uint maxInputResponse, uint maxOutputResponse)
        {
            Packet_Header header;
            IOCTL_Response ioCtlResponse;

            IOCTL_Request_Flags_Values ioCtlRequestFlag = IOCTL_Request_Flags_Values.NONE;

            if (isFsCtl)
            {
                ioCtlRequestFlag = IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL;
            }
            return client.IoCtl(
                1,
                1,
                headerFlags,
                messageId++,
                sessionId,
                treeId,
                (CtlCode_Values)ctlCode,
                fileId,
                maxInputResponse,
                input,
                maxOutputResponse,
                ioCtlRequestFlag,
                out inputResponse,
                out outputResponse,
                out header,
                out ioCtlResponse);
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
        private void InternalCreate(string fileName, uint desiredAccess, FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute, FsCreateDisposition createDisposition, FsCreateOption createOption)
        {
            Packet_Header header;
            CREATE_Response createResponse;
            Smb2CreateContextResponse[] serverCreateContexts;

            CheckStatusCode(
                client.Create(
                    1,
                    1,
                    headerFlags,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    (AccessMask)desiredAccess,
                    ShareAccess_Values.NONE,
                    (CreateOptions_Values)createOption,
                    (CreateDisposition_Values)createDisposition,
                    (File_Attributes)fileAttribute,
                    (ImpersonationLevel_Values)impersonationLevel,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse));
        }


        /// <summary>
        /// Test if the response packet is an interim response packet
        /// </summary>
        /// <param name="singlePacket">The single response packet</param>
        /// <returns>True if it is an interim packet, otherwise false.</returns>
        private bool IsInterimResponsePacket(Smb2SinglePacket singlePacket)
        {
            return Smb2Utility.IsInterimResponsePacket(singlePacket);
        }

        #endregion
    }
}
