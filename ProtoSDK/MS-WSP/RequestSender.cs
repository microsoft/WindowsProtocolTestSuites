// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiService;
using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// RequeseSender class sends MS-WSP requests to the protocol server and receives corresponding subsequent responses.
    /// </summary>
    public class RequestSender
    {
        /// <summary>
        /// Name of the connected server.
        /// </summary>
        private string serverName = null;

        /// <summary>
        /// Name of the connected user.
        /// </summary>
        private string userName = null;

        /// <summary>
        /// Domain of the connected user.
        /// </summary>
        private string domainName = null;

        /// <summary>
        /// Password of the connected user.
        /// </summary>
        private string password = null;

        /// <summary>
        /// Security package used for authentication.
        /// </summary>
        private string securityPackage = null;

        /// <summary>
        /// Whether the client will use server-initiated SPNEGO authentication.
        /// </summary>
        private bool useServerGssToken = false;

        /// <summary>
        /// Name of the named pipe for WSP transport.
        /// </summary>
        private const string pipeName = "MsFteWds";

        /// <summary>
        /// Timeout of the SMB2Client.
        /// </summary>
        private TimeSpan smb2ClientTimeout = default(TimeSpan);

        /// <summary>
        /// Smb2Client used for the named pipe transport.
        /// </summary>
        private Smb2Client smb2Client = null;

        /// <summary>
        /// The default flags value in an SMB2 message.
        /// </summary>
        private Packet_Header_Flags_Values defaultFlags = Packet_Header_Flags_Values.NONE;

        /// <summary>
        /// The message ID for the SMB2 message sequencing.
        /// </summary>
        private ulong messageId = 0;

        /// <summary>
        /// The session ID for the SMB2 session.
        /// </summary>
        private ulong sessionId = 0;

        /// <summary>
        /// The tree ID of the server $IPC virtual directory.
        /// </summary>
        private uint treeId = 0;

        /// <summary>
        /// The file ID of the named pipe.
        /// </summary>
        private FILEID fileId = default(FILEID);

        /// <summary>
        /// The max buffer size of named pipe transport.
        /// </summary>
        private const uint maxBufferSize = 0x4000;

        /// <summary>
        /// Initialize a new RequestSender instance.
        /// </summary>
        /// <param name="serverName">Name of the connected server.</param>
        /// <param name="userName">Name of the connected user.</param>
        /// <param name="domainName">Domain of the connected user.</param>
        /// <param name="password">Password of the connected user.</param>
        /// <param name="securityPackage">Security package used for authentication.</param>
        /// <param name="useServerGssToken">Whether the client will use server-initiated SPNEGO authentication.</param>
        /// <param name="smb2ClientTimeout">Timeout of the SMB2Client for the named pipe transport.</param>
        public RequestSender(
            string serverName,
            string userName,
            string domainName,
            string password,
            string securityPackage,
            bool useServerGssToken,
            TimeSpan smb2ClientTimeout)
        {
            this.serverName = serverName;
            this.userName = userName;
            this.domainName = domainName;
            this.password = password;
            this.securityPackage = securityPackage;
            this.useServerGssToken = useServerGssToken;
            this.smb2ClientTimeout = smb2ClientTimeout;
        }

        /// <summary>
        /// Sends the message blob to the named pipe and obtains the response on a buffer.
        /// </summary>
        /// <param name="messageBlob">Message sent across the pipe.</param>
        /// <param name="readBuffer">Buffer read from the response.</param>
        /// <returns>Number of bytes read from the buffer.</returns>
        public int SendMessage(byte[] messageBlob, out byte[] readBuffer)
        {
            if (smb2Client == null || !smb2Client.IsConnected)
            {
                ConnectToServer();
            }

            var bufferRead = TransactNamedPipe(messageBlob, out var tempBuffer);
            if (tempBuffer == null)
            {
                // If the response contains no data, set the following varaibles to specific values,
                // these specific values are compatible with the previous implementation.
                readBuffer = new byte[0];
                bufferRead = -1;
            }
            else
            {
                readBuffer = new byte[bufferRead];
                Buffer.BlockCopy(tempBuffer, 0, readBuffer, 0, bufferRead);
            }

            return bufferRead;
        }

        /// <summary>
        /// Terminate the named pipe transport.
        /// </summary>
        public void Disconnect()
        {
            if (smb2Client == null || !smb2Client.IsConnected)
            {
                return;
            }

            if (fileId.Persistent != 0 || fileId.Volatile != 0)
            {
                var status = smb2Client.Close(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    sessionId: sessionId,
                    treeId: treeId,
                    fileId: fileId,
                    closeFlags: Flags_Values.NONE,
                    out _,
                    out _);
                CheckStatusCode(status, nameof(Smb2Client.Close));

                fileId = default(FILEID);
            }

            if (treeId != 0)
            {
                var status = smb2Client.TreeDisconnect(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    sessionId: sessionId,
                    treeId: treeId,
                    out _,
                    out _);
                CheckStatusCode(status, nameof(Smb2Client.TreeDisconnect));

                treeId = 0;
            }

            if (sessionId != 0)
            {
                var status = smb2Client.LogOff(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    sessionId: sessionId,
                    out _,
                    out _);
                CheckStatusCode(status, nameof(Smb2Client.LogOff));

                sessionId = 0;
            }

            defaultFlags = Packet_Header_Flags_Values.NONE;
            messageId = 0;
            smb2Client.Disconnect();
            smb2Client = null;
        }

        /// <summary>
        /// Connect to the Server and establish the named pipe transport.
        /// </summary>
        private void ConnectToServer()
        {
            smb2Client = new Smb2Client(smb2ClientTimeout);

            if (IPAddress.TryParse(serverName, out var serverIp))
            {
                smb2Client.ConnectOverTCP(serverIp);
            }
            else
            {
                var serverHostEntry = Dns.GetHostEntry(serverName);
                smb2Client.ConnectOverTCP(serverHostEntry.AddressList[0]);
            }

            var validDialects = new DialectRevision[]
            {
                DialectRevision.Smb2002,
                DialectRevision.Smb21,
                DialectRevision.Smb30,
                DialectRevision.Smb302,
                DialectRevision.Smb311
            };

            var preauthIntegrityHashIDs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            var encryptionAlgorithms = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_GCM, EncryptionAlgorithm.ENCRYPTION_AES128_CCM };
            var status = smb2Client.Negotiate(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    // Will negotiate highest dialect server supports  
                    dialects: validDialects,
                    securityMode: SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    capabilities: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_ENCRYPTION | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                    clientGuid: Guid.NewGuid(),
                    out var selectedDialect,
                    out var serverGssToken,
                    out Packet_Header _,
                    out var negotiateResponse,
                    preauthHashAlgs: preauthIntegrityHashIDs,
                    encryptionAlgs: encryptionAlgorithms);
            CheckStatusCode(status, nameof(Smb2Client.Negotiate));

            var sessionSiginingRequired = negotiateResponse.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);
            if (sessionSiginingRequired)
            {
                defaultFlags |= Packet_Header_Flags_Values.FLAGS_SIGNED;
            }

            var usedSecurityPackageType = (SecurityPackageType)Enum.Parse(typeof(SecurityPackageType), securityPackage);
            var sspiClientGss = new SspiClientSecurityContext(
                usedSecurityPackageType,
                new AccountCredential(domainName, userName, password),
                Smb2Utility.GetCifsServicePrincipalName(serverName),
                ClientSecurityContextAttribute.None,
                SecurityTargetDataRepresentation.SecurityNativeDrep);

            if (usedSecurityPackageType == SecurityPackageType.Negotiate && useServerGssToken)
            {
                sspiClientGss.Initialize(serverGssToken);
            }
            else
            {
                sspiClientGss.Initialize(null);
            }

            do
            {
                status = smb2Client.SessionSetup(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: Packet_Header_Flags_Values.NONE,
                    messageId: messageId++,
                    sessionId: sessionId,
                    sessionSetupFlags: SESSION_SETUP_Request_Flags.NONE,
                    securityMode: SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    capabilities: SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                    previousSessionId: 0,
                    clientGssToken: sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out _,
                    out _);
                CheckStatusCode(status, nameof(Smb2Client.SessionSetup));

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            var treeConnectSigningRequired = sessionSiginingRequired || (selectedDialect >= DialectRevision.Smb311);
            smb2Client.GenerateCryptoKeys(
                sessionId,
                sspiClientGss.SessionKey,
                treeConnectSigningRequired,
                false);

            status = smb2Client.TreeConnect(
                creditCharge: 1,
                creditRequest: 1,
                flags: treeConnectSigningRequired ? defaultFlags | Packet_Header_Flags_Values.FLAGS_SIGNED : defaultFlags,
                messageId: messageId++,
                sessionId: sessionId,
                $"\\\\{serverName}\\IPC$",
                out treeId,
                out _,
                out _);
            CheckStatusCode(status, nameof(Smb2Client.TreeConnect));

            smb2Client.EnableSessionSigningAndEncryption(sessionId, sessionSiginingRequired, false);

            status = smb2Client.Create(
                 creditCharge: 1,
                 creditRequest: 1,
                 flags: defaultFlags,
                 messageId: messageId++,
                 sessionId: sessionId,
                 treeId: treeId,
                 path: pipeName,
                 desiredAccess: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
                 shareAccess: ShareAccess_Values.FILE_SHARE_READ,
                 createOptions: CreateOptions_Values.NONE,
                 createDispositions: CreateDisposition_Values.FILE_OPEN_IF,
                 fileAttributes: File_Attributes.NONE,
                 impersonationLevel: ImpersonationLevel_Values.Impersonation,
                 securityFlag: SecurityFlags_Values.NONE,
                 requestedOplockLevel: RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                 createContexts: null,
                 out fileId,
                 out _,
                 out _,
                 out _);
            CheckStatusCode(status, nameof(Smb2Client.Create));
        }

        /// <summary>
        /// Check the SMB2 status code and throw specific exceptions if the status code is not a successful status code.
        /// </summary>
        /// <param name="status">The SMB2 status code to be checked.</param>
        /// <param name="opName">The name of the SMB2 operation to be checked.</param>
        private void CheckStatusCode(uint status, string opName)
        {
            if (status != Smb2Status.STATUS_SUCCESS &&
                status != Smb2Status.STATUS_PENDING &&
                status != Smb2Status.STATUS_MORE_PROCESSING_REQUIRED &&
                status != Smb2Status.STATUS_END_OF_FILE)
            {
                throw new RequestSenderException(status, $"Operation \"{opName}\" failed with error code {Smb2Status.GetStatusCode(status)}.");
            }
        }

        private bool IsCPMDisconnectMessage(byte[] messageBlob)
        {
            var buffer = new WspBuffer(messageBlob);
            var header = new WspMessageHeader();
            header.FromBytes(buffer);

            return header._msg == WspMessageHeader_msg_Values.CPMDisconnect;
        }

        /// <summary>
        /// Transact messages over the named pipe.
        /// </summary>
        /// <param name="inputBuffer">The input message bytes.</param>
        /// <param name="outputBuffer">The output buffer bytes.</param>
        /// <returns>The bytes should be read from the output buffer.</returns>
        private int TransactNamedPipe(byte[] inputBuffer, out byte[] outputBuffer)
        {
            var status = smb2Client.IoCtl(
                creditCharge: 1,
                creditRequest: 1,
                flags: defaultFlags,
                messageId: messageId++,
                sessionId: sessionId,
                treeId: treeId,
                ctlCode: CtlCode_Values.FSCTL_PIPE_TRANSCEIVE,
                fileId: fileId,
                maxInputResponse: 0,
                requestInput: inputBuffer,
                maxOutputResponse: maxBufferSize,
                ioCtlFlags: IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out _,
                out outputBuffer,
                out _,
                out var outputPayload);

            // Do not check the SMB2 status code while sending CPMDisconnect messages.
            if (!IsCPMDisconnectMessage(inputBuffer))
            {
                CheckStatusCode(status, nameof(Smb2Client.IoCtl));
            }

            return (int)outputPayload.OutputCount;
        }

        /// <summary>
        /// The exception is thrown when there are underlying errors occurred during sending or receiving WSP messages.
        /// </summary>
        public class RequestSenderException : Exception
        {
            /// <summary>
            /// The SMB2 status code related to this exception.
            /// </summary>
            public uint Smb2Status { get; set; }

            /// <summary>
            /// Initialize a new RequestSenderException instance.
            /// </summary>
            /// <param name="smb2Status">The SMB2 status code related to this exception.</param>
            /// <param name="message">The messsage to describe the exception.</param>
            public RequestSenderException(uint smb2Status, string message) : base(message)
            {
                Smb2Status = smb2Status;
            }
        }
    }
}