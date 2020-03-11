// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Principal;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// Delegate to check response header and payload
    /// </summary>
    /// <typeparam name="T">Type of response payload</typeparam>
    /// <param name="responseHeader">Response header to be checked</param>
    /// <param name="response">Response payload to be checked</param>
    public delegate void ResponseChecker<T>(Packet_Header responseHeader, T response);

    /// <summary>
    /// Delegate to generate message id
    /// </summary>
    /// <returns></returns>
    public delegate ulong MessageIdGenerator(SortedSet<ulong> sequenceWindow);

    /// <summary>
    /// Delegate to generate credit charge
    /// </summary>
    /// <param name="payloadSize"></param>
    /// <returns></returns>
    public delegate ushort CreditChargeGenerator(uint payloadSize);

    /// <summary>
    /// Delegate to generate credit request
    /// </summary>
    /// <returns></returns>
    public delegate ushort CreditRequestGenerator(SortedSet<ulong> sequenceWindow, ushort creditGoal, ushort creditCharge);

    /// <summary>
    /// Test client that wraps functionalities of Smb2Client SDK
    /// Temp named as "Smb2FunctionalClient" for now
    /// Will consider a better name once all models leverage this
    /// </summary>
    public class Smb2FunctionalClient
    {
        #region const
        /// <summary>
        /// Windows client sets this value to 64 kb.
        /// </summary>
        public const uint DefaultMaxOutputResponse = 64 * 1024;
        #endregion
        #region Field

        protected Smb2Client client;
        protected ulong sessionId;
        protected byte[] sessionKey;
        protected byte[] serverGssToken;
        protected DialectRevision selectedDialect;
        protected SortedSet<ulong> sequenceWindow;
        protected ITestSite baseTestSite;
        protected TestConfigBase testConfig;

        protected ushort sessionChannelSequence;

        /// <summary>
        /// Credit number that we expect to maintain during test
        /// </summary>
        protected ushort creditGoal;

        /// <summary>
        /// Generator to get message id for a request
        /// </summary>
        protected MessageIdGenerator generateMessageId;

        /// <summary>
        /// Generator to get credit charge for a request 
        /// </summary>
        protected CreditChargeGenerator generateCreditCharge;

        /// <summary>
        /// Generator to get credit request for a request
        /// </summary>
        protected CreditRequestGenerator generateCreditRequest;

        protected uint maxBufferSize;

        /// <summary>
        /// Max message Id ever produced based on granted credits regardless it's used or not.
        /// </summary>
        protected ulong maxMidEverProduced;

        /// <summary>
        /// The maximum size, in bytes, of the buffer that can be used for QUERY_INFO, QUERY_DIRECTORY, SET_INFO and CHANGE_NOTIFY operations.
        /// </summary>
        protected uint maxTransactSize;

        #endregion

        public event Action<Packet_Header> RequestSent;

        #region Constructor

        public Smb2FunctionalClient(TimeSpan timeout, TestConfigBase testConfig, ITestSite baseTestSite, bool checkEncrypt = true)
        {
            client = new Smb2Client(timeout);
            this.testConfig = testConfig;
            client.DisableVerifySignature = this.testConfig.DisableVerifySignature;
            client.CheckEncrypt = checkEncrypt; // Whether to check the response from the server is actually encrypted.

            sequenceWindow = new SortedSet<ulong>();
            sequenceWindow.Add(0);

            // Set to default generators
            generateMessageId = GetDefaultMId;
            generateCreditCharge = GetDefaultCreditCharge;
            generateCreditRequest = GetDefaultCreditRequest;

            this.baseTestSite = baseTestSite;

            client.PacketReceived += new Action<Smb2Packet>(client_PacketReceived);
            client.PacketSending += new Action<Smb2Packet>(client_PacketSent);
            client.PendingResponseReceived += new Action<Smb2SinglePacket>(client_PendingResponseReceived);

            client.NotificationThreadExceptionHappened += client_NotificationThreadExceptionHappened;

            sessionChannelSequence = 0;

            creditGoal = 1;
        }

        #endregion

        #region Properties

        public Smb2Client Smb2Client
        {
            get
            {
                return client;
            }
        }

        /// <summary>
        /// Current session id
        /// </summary>
        public ulong SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                sessionId = value;
            }
        }

        public byte[] SessionKey
        {
            get
            {
                return sessionKey;
            }
            set
            {
                sessionKey = value;
            }
        }

        public DialectRevision Dialect
        {
            get
            {
                return selectedDialect;
            }
            set
            {
                selectedDialect = value;
            }
        }

        public SortedSet<ulong> SequenceWindow
        {
            get
            {
                return sequenceWindow;
            }
        }

        /// <summary>
        /// Credit number that we expect to maintain during test
        /// </summary>
        public ushort CreditGoal
        {
            get
            {
                return creditGoal;
            }
            set
            {
                creditGoal = value;
            }
        }

        /// <summary>
        /// Credit number that client currently holds
        /// </summary>
        public ushort Credits
        {
            get
            {
                return (ushort)sequenceWindow.Count;
            }
        }

        /// <summary>
        /// Generator to get message id for next request
        /// </summary>
        public MessageIdGenerator GenerateMessageId
        {
            get
            {
                return generateMessageId;
            }
            set
            {
                generateMessageId = value;
            }
        }

        /// <summary>
        /// Generator to get credit charge for next request
        /// </summary>
        public CreditChargeGenerator GenerateCreditCharge
        {
            get
            {
                return generateCreditCharge;
            }
            set
            {
                generateCreditCharge = value;
            }
        }

        /// <summary>
        /// Generator to get credit request for next request
        /// </summary>
        public CreditRequestGenerator GenerateCreditRequest
        {
            get
            {
                return generateCreditRequest;
            }
            set
            {
                generateCreditRequest = value;
            }
        }

        /// <summary>
        /// Get or set Session.ChannelSequence
        /// </summary>
        public ushort SessionChannelSequence
        {
            get
            {
                return sessionChannelSequence;
            }
            set
            {
                sessionChannelSequence = value;
            }
        }

        /// <summary>
        /// The maximum size, in bytes, of Length in READ/WRITE that server will accept on the connection
        /// </summary>
        public uint MaxBufferSize
        {
            get
            {
                return maxBufferSize;
            }
        }

        /// <summary>
        /// The server selected cipher ID for encryption.
        /// </summary>
        public EncryptionAlgorithm SelectedCipherID
        {
            get
            {
                return client.SelectedCipherID;
            }
        }

        /// <summary>
        /// The server selected hash ID for Preauth Integrity.
        /// </summary>
        public PreauthIntegrityHashID SelectedPreauthIntegrityHashID
        {
            get
            {
                return client.SelectedPreauthIntegrityHashID;
            }
        }

        /// <summary>
        /// The maximum size, in bytes, of the buffer that can be used for QUERY_INFO, QUERY_DIRECTORY, SET_INFO and CHANGE_NOTIFY operations.
        /// </summary>
        public uint MaxTransactSize
        {
            get
            {
                return maxTransactSize;
            }
        }

        #endregion

        #region Connect and Disconnect

        public void ConnectToServerOverTCP(IPAddress serverIp)
        {
            client.ConnectOverTCP(serverIp);
        }

        public void ConnectToServerOverTCP(IPAddress serverIp, IPAddress clientIp)
        {
            client.ConnectOverTCP(serverIp, clientIp);
        }

        public void ConnectToServerOverNetbios(string serverName)
        {
            client.ConnectOverNetbios(serverName);
        }

        public void ConnectToServer(Smb2TransportType transportType, string serverName, IPAddress serverIp, IPAddress clientIp = null)
        {
            switch (transportType)
            {
                case Smb2TransportType.Tcp:
                    this.baseTestSite.Assert.IsTrue(
                        serverIp != null && serverIp != IPAddress.None,
                        "serverIp should not be empty when transport type is TCP.");
                    if (clientIp != null && clientIp != IPAddress.None)
                    {
                        this.baseTestSite.Log.Add(LogEntryKind.Debug, "Connect to server over TCP from IP {0} to IP {1}", clientIp.ToString(), serverIp.ToString());
                    }
                    else
                    {
                        this.baseTestSite.Log.Add(LogEntryKind.Debug, "Connect to server {0} over TCP", serverIp.ToString());
                    }
                    ConnectToServerOverTCP(serverIp, clientIp);
                    break;
                case Smb2TransportType.NetBios:
                    this.baseTestSite.Assert.IsFalse(string.IsNullOrEmpty(serverName), "serverName should not be null when transport type is NetBIOS.");
                    this.baseTestSite.Log.Add(LogEntryKind.Debug, "Connect to server {0} over NetBios", serverName);
                    ConnectToServerOverNetbios(serverName);
                    break;
                default:
                    this.baseTestSite.Assert.Fail("The transport type is {0}, but currently only Tcp and NetBIOS are supported.", transportType);
                    break;
            }
        }

        public void Disconnect()
        {
            client.NotificationThreadExceptionHappened -= client_NotificationThreadExceptionHappened;
            client.Disconnect();
        }

        #endregion

        #region Encryption and Signing Settings

        public void GenerateCryptoKeys(
           bool enableSigning,
           bool enableEncryption,
           Smb2FunctionalClient previousClient = null,
           bool isBinding = false)
        {
            if (previousClient == null)
            {
                // If isBinding == true, will generate the new session.signingkey and encrypt/decrypt key with current session key (for new session)
                // If isBinding == false, will generate the new session.signingkey and encrypt/decrypt key with previous session key (for re-auth session)
                client.GenerateCryptoKeys(
                    sessionId,
                    sessionKey,
                    enableSigning,
                    enableEncryption,
                    isBinding: isBinding);
            }
            else
            {
                // The alternative channel will copy session.signingkey and encrypt/decrypt key from previous channel
                client.GenerateCryptoKeys(
                    previousClient.SessionId,
                    previousClient.SessionKey,
                    enableSigning,
                    enableEncryption,
                    previousClient.Smb2Client,
                    isBinding);
            }
        }

        /// <summary>
        /// Enable, disable session wide signing and encryption
        /// </summary>
        /// <param name="enableSigning">true if need sign, otherwise false</param>
        /// <param name="enableEncryption">true if need encryption, otherwise false</param>
        public void EnableSessionSigningAndEncryption(
          bool enableSigning,
          bool enableEncryption)
        {
            client.EnableSessionSigningAndEncryption(sessionId, enableSigning, enableEncryption);
        }

        public void SetTreeEncryption(uint treeId, bool enableEncryption)
        {
            client.SetTreeEncryption(sessionId, treeId, enableEncryption);
        }

        #endregion

        #region Message Requests

        #region Negotiate

        public uint MultiProtocolNegotiate(
            string[] dialects,
            ResponseChecker<NEGOTIATE_Response> checker = null)
        {
            Smb2NegotiateResponsePacket negotiateResponse;
            SmbNegotiateRequestPacket request;
            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.MultiProtocolNegotiate(
                dialects,
                out selectedDialect,
                out serverGssToken,
                out request,
                out negotiateResponse);
            Packet_Header header = negotiateResponse.Header;
            maxBufferSize = negotiateResponse.PayLoad.MaxReadSize < negotiateResponse.PayLoad.MaxWriteSize ?
                negotiateResponse.PayLoad.MaxReadSize : negotiateResponse.PayLoad.MaxWriteSize;

            SetCreditGoal();

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, negotiateResponse.PayLoad);
            testConfig.CheckNegotiateContext(request, negotiateResponse);  //request is not set any negotiatecontext here so set it null.
            return status;
        }

        public uint Negotiate(
            DialectRevision[] dialects,
            bool isSmb1NegotiateEnabled,
            SecurityMode_Values securityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            Capabilities_Values? capabilityValue = null,
            Guid? clientGuid = null,
            ResponseChecker<NEGOTIATE_Response> checker = null,
            bool ifHandleRejectUnencryptedAccessSeparately = false,
            bool? ifAddGLOBAL_CAP_ENCRYPTION = null,
            bool addDefaultEncryption = false,
            bool addNetNameConetxtID = false
            )
        {
            if (isSmb1NegotiateEnabled)
            {
                string[] comDialects;
                if (dialects != null
                    && dialects.Length == 1
                    && dialects[0] == DialectRevision.Smb2002)
                {
                    // If client only needs to negotiate Smb2002, then ComNegotiate only send SMB 2.002 dialect string
                    comDialects = new string[] { "SMB 2.002" };
                }
                else
                {
                    comDialects = new string[] { "SMB 2.002", "SMB 2.???" };
                }

                bool isSmb2002Selected = false;
                uint status = 0;
                Packet_Header? respHeader = null;
                NEGOTIATE_Response? respNegotiate = null;

                MultiProtocolNegotiate(
                    comDialects,
                    (header, response) =>
                    {
                        this.baseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "ComNegotiate should succeed.");

                        this.baseTestSite.Log.Add(
                            LogEntryKind.Debug,
                            "The selected dialect is " + response.DialectRevision);

                        // If server only implements Smb2.002, stop further SMB2 Negotiate
                        if (response.DialectRevision == DialectRevision.Smb2002)
                        {
                            status = header.Status;
                            isSmb2002Selected = true;

                            respHeader = header;
                            respNegotiate = response;
                            maxTransactSize = response.MaxTransactSize;

                            baseTestSite.Assert.IsTrue(
                                header.CreditRequestResponse >= 1,
                                "The server SHOULD<168> grant the client a non-zero value of credits in response to any non-zero value requested, within administratively configured limits. The server MUST grant the client at least 1 credit when responding to SMB2 NEGOTIATE, actually server returns {0}", header.CreditRequestResponse);
                        }
                    }
                );

                if (isSmb2002Selected)
                {
                    if (!ifHandleRejectUnencryptedAccessSeparately)
                    {
                        if (testConfig.IsGlobalEncryptDataEnabled && testConfig.IsGlobalRejectUnencryptedAccessEnabled)
                        {
                            baseTestSite.Assert.Inconclusive("Test case is not applicable when dialect is less than SMB 3.0, both IsGlobalEncryptDataEnabled and IsGlobalRejectUnencryptedAccessEnabled set to true.");
                        }
                    }
                    if (checker != null)
                    {
                        checker(respHeader.Value, respNegotiate.Value);
                    }
                    return status;
                }
            }

            if (ifAddGLOBAL_CAP_ENCRYPTION == null)
            {
                // Assign default value to ifAddGLOBAL_CAP_ENCRYPTION according to server's configuration.
                ifAddGLOBAL_CAP_ENCRYPTION = testConfig.IsGlobalEncryptDataEnabled;
            }

            return Negotiate(
                    Packet_Header_Flags_Values.NONE,
                    dialects,
                    securityMode,
                    capabilityValue,
                    clientGuid,
                    checker: checker,
                    ifHandleRejectUnencryptedAccessSeparately: ifHandleRejectUnencryptedAccessSeparately,
                    ifAddGLOBAL_CAP_ENCRYPTION: ifAddGLOBAL_CAP_ENCRYPTION.Value,
                    addDefaultEncryption: addDefaultEncryption,
                    addNetNameConetxtID: addNetNameConetxtID
                    );
        }

        public uint Negotiate(
            Packet_Header_Flags_Values headerFlag,
            DialectRevision[] dialects,
            SecurityMode_Values securityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            Capabilities_Values? capabilityValue = null,
            Guid? clientGuid = null,
            ResponseChecker<NEGOTIATE_Response> checker = null,
            bool ifHandleRejectUnencryptedAccessSeparately = false,
            bool ifAddGLOBAL_CAP_ENCRYPTION = true,
            bool addDefaultEncryption = false,
            bool addNetNameConetxtID = false
           )
        {
            PreauthIntegrityHashID[] preauthHashAlgs = null;
            EncryptionAlgorithm[] encryptionAlgs = null;
            SMB2_NETNAME_NEGOTIATE_CONTEXT_ID netNameContext = null;

            // For back compatibility, if dialects contains SMB 3.11, preauthentication integrity context should be present.
            if (Array.IndexOf(dialects, DialectRevision.Smb311) >= 0)
            {
                preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgs = ((capabilityValue != null && capabilityValue.Value.HasFlag(Capabilities_Values.GLOBAL_CAP_ENCRYPTION)) || ifAddGLOBAL_CAP_ENCRYPTION) ?
                    new EncryptionAlgorithm[]
                    {
                        EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                        EncryptionAlgorithm.ENCRYPTION_AES128_CCM
                    }
                    : null;
                if (addNetNameConetxtID)
                {
                    netNameContext.Header.ContextType = SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_NETNAME_NEGOTIATE_CONTEXT_ID;
                    netNameContext.Header.Reserved = 0;
                    netNameContext.NetName = testConfig.SutComputerName.ToArray();
                    netNameContext.Header.DataLength = netNameContext.GetDataLength();
                }               
            }

            return NegotiateWithContexts
            (
                headerFlag,
                dialects,
                securityMode,
                capabilityValue,
                clientGuid,
                preauthHashAlgs,
                encryptionAlgs,
                null,               
                checker,
                ifHandleRejectUnencryptedAccessSeparately,
                ifAddGLOBAL_CAP_ENCRYPTION,
                addDefaultEncryption,
                addNetNameConetxtID
            );
        }

        public uint NegotiateWithContexts(
            Packet_Header_Flags_Values headerFlag,
            DialectRevision[] dialects,
            SecurityMode_Values securityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            Capabilities_Values? capabilityValue = null,
            Guid? clientGuid = null,
            PreauthIntegrityHashID[] preauthHashAlgs = null,
            EncryptionAlgorithm[] encryptionAlgs = null,
            CompressionAlgorithm[] compressionAlgorithms = null,            
            ResponseChecker<NEGOTIATE_Response> checker = null,            
            bool ifHandleRejectUnencryptedAccessSeparately = false,
            bool ifAddGLOBAL_CAP_ENCRYPTION = true,
            bool addDefaultEncryption = false,
            bool addNetNameContextId = false
            )
        {
            SMB2_NETNAME_NEGOTIATE_CONTEXT_ID netNameContext = null;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            // If the client implements the SMB 3.0 dialect, the Capabilities field MUST be constructed using the flages defined in Capabilities_Values. 
            // Otherwise, this field MUST be set to 0.
            if (null == capabilityValue)
            {
                if (Array.IndexOf(dialects, DialectRevision.Smb30) >= 0 || Array.IndexOf(dialects, DialectRevision.Smb302) >= 0 || Array.IndexOf(dialects, DialectRevision.Smb311) >= 0)
                {
                    capabilityValue = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
                }
                else
                {
                    capabilityValue = Capabilities_Values.NONE;
                }
            }
            if (ifAddGLOBAL_CAP_ENCRYPTION && (Array.IndexOf(dialects, DialectRevision.Smb30) >= 0 || Array.IndexOf(dialects, DialectRevision.Smb302) >= 0 || Array.IndexOf(dialects, DialectRevision.Smb311) >= 0))
            {
                capabilityValue |= Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            }
            // Guid should be zero when dialect is 2.0 and should not be zero when dialect is not 2.0
            if (null == clientGuid)
                clientGuid = (dialects.Length == 1 && dialects[0] == DialectRevision.Smb2002) ? Guid.Empty : Guid.NewGuid();

            if (addNetNameContextId)
            {
                netNameContext = new SMB2_NETNAME_NEGOTIATE_CONTEXT_ID();
                netNameContext.Header.ContextType = SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_NETNAME_NEGOTIATE_CONTEXT_ID;
                netNameContext.Header.Reserved = 0;
                netNameContext.NetName = testConfig.SutComputerName.ToArray();
                netNameContext.Header.DataLength = netNameContext.GetDataLength();
            }
            Smb2NegotiateRequestPacket negotiateRequest;
            Smb2NegotiateResponsePacket negotiateResponse;

            uint status = client.Negotiate(              
               creditCharge,
               generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
               headerFlag,
               messageId,
               dialects,
               securityMode,
               capabilityValue.Value,
               clientGuid.Value,
               out selectedDialect,
               out serverGssToken,
               out negotiateRequest,
               out negotiateResponse,
               preauthHashAlgs: preauthHashAlgs,
               encryptionAlgs: encryptionAlgs,
               compressionAlgorithms: compressionAlgorithms,
               addDefaultEncryption: addDefaultEncryption,
               netNameContext: netNameContext
               );           

            if (!ifHandleRejectUnencryptedAccessSeparately)
            {
                if (testConfig.IsGlobalEncryptDataEnabled && selectedDialect < DialectRevision.Smb30 && testConfig.IsGlobalRejectUnencryptedAccessEnabled)
                {
                    baseTestSite.Assert.Inconclusive("Test case is not applicable when dialect is less than SMB 3.0, both IsGlobalEncryptDataEnabled and IsGlobalRejectUnencryptedAccessEnabled set to true.");
                }
            }

            maxBufferSize = negotiateResponse.PayLoad.MaxReadSize < negotiateResponse.PayLoad.MaxWriteSize ?
                negotiateResponse.PayLoad.MaxReadSize : negotiateResponse.PayLoad.MaxWriteSize;

            maxTransactSize = negotiateResponse.PayLoad.MaxTransactSize;

            baseTestSite.Assert.IsTrue(
                negotiateResponse.Header.CreditRequestResponse >= 1,
                "The server SHOULD<168> grant the client a non-zero value of credits in response to any non-zero value requested, within administratively configured limits. The server MUST grant the client at least 1 credit when responding to SMB2 NEGOTIATE, actually server returns {0}", negotiateResponse.Header.CreditRequestResponse);

            SetCreditGoal();

            ProduceCredit(messageId, negotiateResponse.Header);
            InnerResponseChecker(checker, negotiateResponse.Header, negotiateResponse.PayLoad);
            testConfig.CheckNegotiateContext(negotiateRequest, negotiateResponse);

            return status;
        }

        #endregion

        #region Session Setup and Logoff

        public uint SessionSetup(
            SecurityPackageType securityPackageType,
            string serverName,
            AccountCredential credential,
            bool useServerGssToken,
            SESSION_SETUP_Request_SecurityMode_Values securityMode = SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            SESSION_SETUP_Request_Capabilities_Values capabilities = SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
            ResponseChecker<SESSION_SETUP_Response> checker = null)
        {
            return SessionSetup(
                Packet_Header_Flags_Values.NONE,
                SESSION_SETUP_Request_Flags.NONE,
                securityMode,
                capabilities,
                0,
                securityPackageType,
                serverName,
                credential,
                useServerGssToken,
                checker: checker);
        }

        public uint SessionSetup(
            Packet_Header_Flags_Values headerFlags,
            SecurityPackageType securityPackageType,
            string serverName,
            AccountCredential credential,
            bool useServerGssToken,
            SESSION_SETUP_Request_SecurityMode_Values securityMode,
            SESSION_SETUP_Request_Capabilities_Values capabilities = SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
            ResponseChecker<SESSION_SETUP_Response> checker = null)
        {
            return SessionSetup(
                headerFlags,
                SESSION_SETUP_Request_Flags.NONE,
                securityMode,
                capabilities,
                0,
                securityPackageType,
                serverName,
                credential,
                useServerGssToken,
                checker: checker);
        }

        public uint AlternativeChannelSessionSetup(
            Smb2FunctionalClient mainChannelClient,
            SecurityPackageType securityPackageType,
            string serverName,
            AccountCredential credential,
            bool useServerGssToken,
            SESSION_SETUP_Request_SecurityMode_Values securityMode = SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            SESSION_SETUP_Request_Capabilities_Values capabilities = SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
            ResponseChecker<SESSION_SETUP_Response> checker = null,
            bool invalidToken = false)
        {
            #region Check Applicability
            // According to TD, server must support signing when it supports multichannel.
            // 3.3.5.5   Receiving an SMB2 SESSION_SETUP Request
            // 4. If Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE, and the SMB2_SESSION_FLAG_BINDING bit is
            //    set in the Flags field of the request, the server MUST perform the following:
            //    If the SMB2_FLAGS_SIGNED bit is not set in the Flags field in the header, the server MUST fail the request with error STATUS_INVALID_PARAMETER.
            testConfig.CheckSigning();
            #endregion

            sessionId = mainChannelClient.sessionId;
            sessionKey = mainChannelClient.sessionKey;

            // copy crypto info for Session.SigningKey from main channel for signing the session binding message
            GenerateCryptoKeys(testConfig.SendSignedRequest, false, mainChannelClient);

            return SessionSetup(
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                SESSION_SETUP_Request_Flags.SESSION_FLAG_BINDING,
                securityMode,
                capabilities,
                0,
                securityPackageType,
                serverName,
                credential,
                useServerGssToken,
                checker: checker,
                invalidToken: invalidToken);
        }

        public uint ReconnectSessionSetup(
            Smb2FunctionalClient previousClient,
            SecurityPackageType securityPackageType,
            string serverName,
            AccountCredential credential,
            bool useServerGssToken,
            SESSION_SETUP_Request_SecurityMode_Values securityMode = SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            SESSION_SETUP_Request_Capabilities_Values capabilities = SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
            ResponseChecker<SESSION_SETUP_Response> checker = null)
        {
            return SessionSetup(
                Packet_Header_Flags_Values.NONE,
                SESSION_SETUP_Request_Flags.NONE,
                securityMode,
                capabilities,
                previousClient.sessionId,
                securityPackageType,
                serverName,
                credential,
                useServerGssToken,
                checker: checker);
        }

        public uint ReconnectSessionSetup(
            ulong sessionId,
            SecurityPackageType securityPackageType,
            string serverName,
            AccountCredential credential,
            bool useServerGssToken,
            SESSION_SETUP_Request_SecurityMode_Values securityMode = SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            SESSION_SETUP_Request_Capabilities_Values capabilities = SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
            ResponseChecker<SESSION_SETUP_Response> checker = null)
        {
            return SessionSetup(
                Packet_Header_Flags_Values.NONE,
                SESSION_SETUP_Request_Flags.NONE,
                securityMode,
                capabilities,
                sessionId,
                securityPackageType,
                serverName,
                credential,
                useServerGssToken,
                checker: checker);
        }

        public uint SessionSetup(
            Packet_Header_Flags_Values headerFlags,
            SESSION_SETUP_Request_Flags sessionSetupFlags,
            ulong previousSessionId,
            SecurityPackageType securityPackageType,
            string serverName,
            AccountCredential credential,
            bool useServerGssToken,
            bool isMultipleChannelSupported = true,
            ResponseChecker<SESSION_SETUP_Response> checker = null)
        {
            return SessionSetup(
                headerFlags,
                sessionSetupFlags,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                previousSessionId,
                securityPackageType,
                serverName,
                credential,
                useServerGssToken,
                false,
                isMultipleChannelSupported,
                checker);
        }

        public uint LogOff(ResponseChecker<LOGOFF_Response> checker = null)
        {
            Packet_Header header;
            LOGOFF_Response logoffResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.LogOff(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                out header,
                out logoffResponse);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, logoffResponse);

            return status;
        }

        #endregion

        #region Tree Connect and Disconnect

        public uint TreeConnect(
            string uncSharePath,
            out uint treeId,
            ResponseChecker<TREE_CONNECT_Response> checker = null,
            TreeConnect_Flags flags = TreeConnect_Flags.SMB2_SHAREFLAG_NONE,
            WindowsIdentity identity = null)
        {
            return TreeConnect(
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                uncSharePath,
                out treeId,
                checker,
                flags,
                identity);
        }

        /// <summary>
        /// Client sends TreeConnect request to server and expects response
        /// </summary>
        /// <param name="headerFlags">Flags field in SMB2 header, should has flag Packet_Header_Flags_Values.FLAGS_SIGNED if server supports signing</param>
        /// <param name="uncSharePath">UNC path of the share to be connected</param>
        /// <param name="treeId">Tree id returned by server</param>
        /// <param name="checker">Delegate to check the response</param>
        /// <returns></returns>
        public uint TreeConnect(
            Packet_Header_Flags_Values headerFlags,
            string uncSharePath,
            out uint treeId,
            ResponseChecker<TREE_CONNECT_Response> checker = null,
            TreeConnect_Flags flags = TreeConnect_Flags.SMB2_SHAREFLAG_NONE,
            WindowsIdentity identity = null)
        {
            Packet_Header header;
            TREE_CONNECT_Response treeConnectResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status;
            /*
             *  According to [MS-SMB2] section 2.2.9
             *  1. If SMB2_TREE_CONNECT_FLAG_EXTENSION_PRESENT is not set in the Flags field of this structure,
             *     this field is a variable-length buffer that contains the full share path name.
             */
            if (!flags.HasFlag(TreeConnect_Flags.SMB2_SHAREFLAG_EXTENSION_PRESENT))
            {
                status = client.TreeConnect(
                    creditCharge,
                    generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                    headerFlags,
                    messageId,
                    sessionId,
                    uncSharePath,
                    out treeId,
                    out header,
                    out treeConnectResponse,
                    0,
                    flags);
            }
            /*
             *  2. If SMB2_TREE_CONNECT_FLAG_EXTENSION_PRESENT is set in the Flags field in this structure,
             *     this field is a variable-length buffer that contains the tree connect request extension,
             *     as specified in section 2.2.9.1.
             */
            else
            {
                byte[] buffer = CreateTreeConnectRequestExt(uncSharePath, identity);
                status = client.TreeConnect(
                    creditCharge,
                    generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                    headerFlags,
                    messageId,
                    sessionId,
                    Encoding.Unicode.GetBytes(uncSharePath).Length,
                    buffer,
                    out treeId,
                    out header,
                    out treeConnectResponse,
                    0,
                    flags);
            }

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, treeConnectResponse);

            return status;
        }

        public uint TreeDisconnect(
            uint treeId,
            ResponseChecker<TREE_DISCONNECT_Response> checker = null)
        {
            Packet_Header header;
            TREE_DISCONNECT_Response treeDisconnectResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.TreeDisconnect(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                out header,
                out treeDisconnectResponse);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, treeDisconnectResponse);

            return status;
        }

        #endregion

        #region Create and Close

        /// <summary>
        /// Client sends Create request to server and expects response
        /// </summary>
        /// <param name="treeId">Tree Id from Tree Connect response</param>
        /// <param name="fileName">File name to be created</param>
        /// <param name="createOptions">Options to be applied when creating or opening the file</param>
        /// <param name="headerFlag">Flags field in SMB2 header</param>
        /// <param name="fileId">File id returned by server</param>
        /// <param name="serverCreateContexts">Create Contexts returned by server</param>
        /// <param name="requestedOplockLevel_Values">The requested oplock level</param>
        /// <param name="createContexts">Create Contexts sent in Create request</param>
        /// <param name="accessMask">The level of access that is required</param>
        /// <param name="shareAccess">Specifies the sharing mode for the open</param>
        /// <param name="checker">Delegate to check the response</param>
        /// <param name="impersonationLevel">Impersonation level</param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is specified in the name field already exists</param>
        /// <param name="fileAttributes">Attributes of the file to be created</param>
        /// <returns>Status of response</returns>
        public uint Create(
            uint treeId,
            string fileName,
            CreateOptions_Values createOptions,
            Packet_Header_Flags_Values headerFlag,
            out FILEID fileId,
            out Smb2CreateContextResponse[] serverCreateContexts,
            RequestedOplockLevel_Values requestedOplockLevel_Values = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
            Smb2CreateContextRequest[] createContexts = null,
            AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
            ShareAccess_Values shareAccess = ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
            ResponseChecker<CREATE_Response> checker = null,
            ImpersonationLevel_Values impersonationLevel = ImpersonationLevel_Values.Impersonation,
            CreateDisposition_Values createDisposition = CreateDisposition_Values.FILE_OPEN_IF,
            File_Attributes fileAttributes = File_Attributes.NONE)
        {
            Packet_Header header;
            CREATE_Response createResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.Create(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                headerFlag,
                messageId,
                sessionId,
                treeId,
                fileName,
                accessMask,
                shareAccess,
                createOptions,
                createDisposition,
                fileAttributes,
                impersonationLevel,
                SecurityFlags_Values.NONE,
                requestedOplockLevel_Values,
                createContexts,
                out fileId,
                out serverCreateContexts,
                out header,
                out createResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, createResponse);

            return status;
        }

        /// <summary>
        /// Client sends Create request to server and expects response
        /// </summary>
        /// <param name="treeId">Tree Id from Tree Connect response</param>
        /// <param name="fileName">File name to be created</param>
        /// <param name="createOptions">Options to be applied when creating or opening the file</param>
        /// <param name="fileId">File id returned by server</param>
        /// <param name="serverCreateContexts">Create Contexts returned by server</param>
        /// <param name="requestedOplockLevel_Values">The requested oplock level</param>
        /// <param name="createContexts">Create Contexts sent in Create request</param>
        /// <param name="accessMask">The level of access that is required</param>
        /// <param name="shareAccess">Specifies the sharing mode for the open</param>
        /// <param name="checker">Delegate to check the response</param>
        /// <param name="impersonationLevel">Impersonation level</param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is specified in the name field already exists</param>
        /// <param name="fileAttributes">Attributes of the file to be created</param>
        /// <returns>Status of response</returns>
        public uint Create(
            uint treeId,
            string fileName,
            CreateOptions_Values createOptions,
            out FILEID fileId,
            out Smb2CreateContextResponse[] serverCreateContexts,
            RequestedOplockLevel_Values requestedOplockLevel_Values = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
            Smb2CreateContextRequest[] createContexts = null,
            AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
            ShareAccess_Values shareAccess = ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
            ResponseChecker<CREATE_Response> checker = null,
            ImpersonationLevel_Values impersonationLevel = ImpersonationLevel_Values.Impersonation,
            CreateDisposition_Values createDisposition = CreateDisposition_Values.FILE_OPEN_IF,
            File_Attributes fileAttributes = File_Attributes.NONE)
        {
            Packet_Header_Flags_Values headerFlag = testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

            return Create(
                treeId,
                fileName,
                createOptions,
                headerFlag,
                out fileId,
                out serverCreateContexts,
                requestedOplockLevel_Values,
                createContexts,
                accessMask,
                shareAccess,
                checker,
                impersonationLevel,
                createDisposition,
                fileAttributes);
        }

        /// <summary>
        /// Client sends Create request to server and expects response
        /// </summary>
        /// <param name="treeId">Tree Id from Tree Connect response</param>
        /// <param name="fileName">File name to be created</param>
        /// <param name="createOptions">Options to be applied when creating or opening the file</param>
        /// <param name="headerFlag">Flags field in SMB2 header</param>
        /// <param name="messageId">Message id returned by server</param>
        /// <param name="requestedOplockLevel_Values">The requested oplock level</param>
        /// <param name="createContexts">Create Contexts sent in Create request</param>
        /// <param name="accessMask">The level of access that is required</param>
        /// <param name="shareAccess">Specifies the sharing mode for the open</param>
        /// <param name="impersonationLevel">Impersonation level</param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is specified in the name field already exists</param>
        /// <param name="fileAttributes">Attributes of the file to be created</param>
        public void CreateRequest(
            uint treeId,
            string fileName,
            CreateOptions_Values createOptions,
            Packet_Header_Flags_Values headerFlag,
            out ulong messageId,
            RequestedOplockLevel_Values requestedOplockLevel_Values = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
            Smb2CreateContextRequest[] createContexts = null,
            AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
            ShareAccess_Values shareAccess = ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
            ImpersonationLevel_Values impersonationLevel = ImpersonationLevel_Values.Impersonation,
            CreateDisposition_Values createDisposition = CreateDisposition_Values.FILE_OPEN_IF,
            File_Attributes fileAttributes = File_Attributes.NONE)
        {
            messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            client.CreateRequest(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                headerFlag,
                messageId,
                sessionId,
                treeId,
                fileName,
                accessMask,
                shareAccess,
                createOptions,
                createDisposition,
                fileAttributes,
                impersonationLevel,
                SecurityFlags_Values.NONE,
                requestedOplockLevel_Values,
                createContexts,
                sessionChannelSequence);
        }

        /// <summary>
        /// Client sends Create request to server and expects response
        /// </summary>
        /// <param name="treeId">Tree Id from Tree Connect response</param>
        /// <param name="fileName">File name to be created</param>
        /// <param name="createOptions">Options to be applied when creating or opening the file</param>
        /// <param name="messageId">Message id returned by server</param>
        /// <param name="requestedOplockLevel_Values">The requested oplock level</param>
        /// <param name="createContexts">Create Contexts sent in Create request</param>
        /// <param name="headerFlag">Flags field in SMB2 header, flag Packet_Header_Flags_Values.FLAGS_SIGNED is set by default, but it will be cleared if server does not support signing</param>
        /// <param name="accessMask">The level of access that is required</param>
        /// <param name="shareAccess">Specifies the sharing mode for the open</param>
        /// <param name="impersonationLevel">Impersonation level</param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is specified in the name field already exists</param>
        /// <param name="fileAttributes">Attributes of the file to be created</param>
        public void CreateRequest(
            uint treeId,
            string fileName,
            CreateOptions_Values createOptions,
            out ulong messageId,
            RequestedOplockLevel_Values requestedOplockLevel_Values = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
            Smb2CreateContextRequest[] createContexts = null,
            AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
            ShareAccess_Values shareAccess = ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
            ImpersonationLevel_Values impersonationLevel = ImpersonationLevel_Values.Impersonation,
            CreateDisposition_Values createDisposition = CreateDisposition_Values.FILE_OPEN_IF,
            File_Attributes fileAttributes = File_Attributes.NONE)
        {
            Packet_Header_Flags_Values headerFlag = testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

            CreateRequest(
                treeId,
                fileName,
                createOptions,
                headerFlag,
                out messageId,
                requestedOplockLevel_Values,
                createContexts,
                accessMask,
                shareAccess,
                impersonationLevel,
                createDisposition,
                fileAttributes);
        }

        public uint CreateResponse(
            ulong messageId,
            out FILEID fileId,
            out Smb2CreateContextResponse[] serverCreateContexts,
            ResponseChecker<CREATE_Response> checker = null)
        {
            Packet_Header header;
            CREATE_Response createResponse;

            uint status = client.CreateResponse(
                messageId,
                out fileId,
                out serverCreateContexts,
                out header,
                out createResponse);

            ProduceCredit(header.MessageId, header);

            InnerResponseChecker(checker, header, createResponse);

            return status;
        }

        public uint Close(uint treeId, FILEID fileId, ResponseChecker<CLOSE_Response> checker = null, Flags_Values flags = Flags_Values.NONE)
        {
            Packet_Header header;
            CLOSE_Response closeResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.Close(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                fileId,
                flags,
                out header,
                out closeResponse);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, closeResponse);

            return status;
        }

        #endregion

        #region Read and Write

        public uint Read(
            uint treeId,
            FILEID fileId,
            ulong offset,
            uint lengthToRead,
            out string data,
            ResponseChecker<READ_Response> checker = null,
            bool isReplay = false)
        {
            byte[] content;

            uint status = Read(treeId, fileId, offset, lengthToRead, out content, checker, isReplay);

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                data = Encoding.ASCII.GetString(content);
            }
            else
            {
                data = null;
            }
            return status;
        }

        public uint Read(
            uint treeId,
            FILEID fileId,
            ulong offset,
            uint lengthToRead,
            out byte[] data,
            ResponseChecker<READ_Response> checker = null,
            bool isReplay = false,
            bool compressRead = false)
        {
            Packet_Header header;
            READ_Response readResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(lengthToRead);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.Read(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                (testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE) | (isReplay ? Packet_Header_Flags_Values.FLAGS_REPLAY_OPERATION : Packet_Header_Flags_Values.NONE),
                messageId,
                sessionId,
                treeId,
                lengthToRead,
                offset,
                fileId,
                0,
                Channel_Values.CHANNEL_NONE,
                0,
                new byte[0],
                out data,
                out header,
                out readResponse,
                compressRead: compressRead);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, readResponse);

            return status;
        }

        public uint Write(
            uint treeId,
            FILEID fileId,
            string data,
            ulong offset = 0,
            ResponseChecker<WRITE_Response> checker = null,
            bool isReplay = false)
        {
            return Write(treeId, fileId, Encoding.ASCII.GetBytes(data), offset, checker, isReplay);
        }

        public uint Write(
            uint treeId,
            FILEID fileId,
            byte[] data,
            ulong offset = 0,
            ResponseChecker<WRITE_Response> checker = null,
            bool isReplay = false,
            bool compressWrite = false)
        {
            Packet_Header header;
            WRITE_Response writeResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge((uint)data.Length);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.Write(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                (testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE) | (isReplay ? Packet_Header_Flags_Values.FLAGS_REPLAY_OPERATION : Packet_Header_Flags_Values.NONE),
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
                out writeResponse,
                sessionChannelSequence,
                compressWrite);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, writeResponse);

            return status;
        }

        public void WriteRequest(
            uint treeId,
            FILEID fileId,
            byte[] data,
            out ulong messageId,
            ulong offset = 0)
        {
            messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(64);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            client.WriteRequest(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                offset,
                fileId,
                Channel_Values.CHANNEL_NONE,
                WRITE_Request_Flags_Values.None,
                new byte[0],
                data
                );
        }

        public uint WriteResponse(
           ulong messageId,
           ResponseChecker<WRITE_Response> checker = null)
        {
            Packet_Header header;
            WRITE_Response writeResponse;

            uint status = client.WriteResponse(
                messageId,
                out header,
                out writeResponse
                );

            ProduceCredit(header.MessageId, header);

            InnerResponseChecker(checker, header, writeResponse);

            return status;
        }

        #endregion

        #region Flush

        public uint Flush(uint treeId, FILEID fileId, ResponseChecker<FLUSH_Response> checker = null)
        {
            uint status;
            Packet_Header header;
            FLUSH_Response flushResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.Flush(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                fileId,
                out header,
                out flushResponse);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, flushResponse);

            return status;
        }

        #endregion

        #region IOCTL

        public uint ResiliencyRequest(
            uint treeId,
            FILEID fileId,
            uint timeout,
            uint inputCount,
            out Packet_Header header,
            out IOCTL_Response ioCtlResponse,
            out byte[] inputInResponse,
            out byte[] outputInResponse,
            ResponseChecker<IOCTL_Response> checker = null,
            bool isReplay = false)
        {
            NETWORK_RESILIENCY_Request resilientRequest = new NETWORK_RESILIENCY_Request()
            {
                Timeout = timeout
            };

            byte[] resiliencyRequestBytes = TypeMarshal.ToBytes<NETWORK_RESILIENCY_Request>(resilientRequest);
            byte[] buffer = resiliencyRequestBytes;
            if (inputCount < resiliencyRequestBytes.Length)
            {
                buffer = new byte[inputCount];
                Array.Copy(resiliencyRequestBytes, buffer, buffer.Length);
            }
            else if (inputCount > resiliencyRequestBytes.Length)
            {
                buffer = new byte[inputCount];
                Array.Copy(resiliencyRequestBytes, buffer, resiliencyRequestBytes.Length);
            }


            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                (testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE) | (isReplay ? Packet_Header_Flags_Values.FLAGS_REPLAY_OPERATION : Packet_Header_Flags_Values.NONE),
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY,
                fileId,
                0,
                buffer,
                DefaultMaxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out inputInResponse,
                out outputInResponse,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }


        public uint QueryNetworkInterfaceInfo(
            uint treeId,
            out NETWORK_INTERFACE_INFO_Response[] networkInfoResponses,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            Packet_Header header;
            FILEID ioCtlFileId = new FILEID();
            ioCtlFileId.Persistent = 0xFFFFFFFFFFFFFFFF;
            ioCtlFileId.Volatile = 0xFFFFFFFFFFFFFFFF;
            IOCTL_Response ioCtlResponse;
            byte[] respInput;
            byte[] respOutput;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO,
                ioCtlFileId,
                0,
                null,
                DefaultMaxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out respInput,
                out respOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            networkInfoResponses = Smb2Utility.UnmarshalNetworkInterfaceInfoResponse(respOutput);
            return status;
        }

        public uint ValidateNegotiateInfo(
            uint treeId,
            byte[] buffer,
            out byte[] respOutput,
            uint maxOutputResponse = DefaultMaxOutputResponse,
            bool signRequest = true,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            uint status = 0;
            Packet_Header header;
            IOCTL_Response ioCtlResponse;
            byte[] respInput;
            FILEID ioCtlFileId = new FILEID();
            ioCtlFileId.Persistent = 0xFFFFFFFFFFFFFFFF;
            ioCtlFileId.Volatile = 0xFFFFFFFFFFFFFFFF;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                signRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_VALIDATE_NEGOTIATE_INFO,
                ioCtlFileId,
                0,
                buffer,
                maxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out respInput,
                out respOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint FileLevelTrim(
            uint treeId,
            FILEID fileId,
            byte[] buffer,
            out byte[] respOutput,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            uint status = 0;
            Packet_Header header;
            IOCTL_Response ioCtlResponse;
            byte[] respInput;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_FILE_LEVEL_TRIM,
                fileId,
                0,
                buffer,
                DefaultMaxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out respInput,
                out respOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint GetIntegrityInfo(
            uint treeId,
            FILEID fileId,
            out FSCTL_GET_INTEGRITY_INFO_OUTPUT getIntegrityInfo,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            uint status = 0;
            Packet_Header header;
            IOCTL_Response ioCtlResponse;
            byte[] respInput;
            byte[] respOutput;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION,
                fileId,
                0,
                null,
                DefaultMaxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out respInput,
                out respOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFO_OUTPUT>(respOutput);

            return status;
        }

        public uint EnumerateSnapShots(
            uint treeId,
            FILEID fileId,
            uint maxOutputResponse,
            out SRV_SNAPSHOT_ARRAY snapshotArray,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            uint status = 0;
            Packet_Header header;
            IOCTL_Response ioCtlResponse;
            byte[] respInput;
            byte[] respOutput;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_SRV_ENUMERATE_SNAPSHOTS,
                fileId,
                0,
                null,
                maxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out respInput,
                out respOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            snapshotArray = TypeMarshal.ToStruct<SRV_SNAPSHOT_ARRAY>(respOutput);

            return status;
        }

        public uint SetIntegrityInfo(
            uint treeId,
            FILEID fileId,
            byte[] buffer,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            uint status = 0;
            Packet_Header header;
            IOCTL_Response ioCtlResponse;
            byte[] respInput;
            byte[] respOutput;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION,
                fileId,
                0,
                buffer,
                DefaultMaxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out respInput,
                out respOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint QueryDFSReferralInfo(
            uint treeId,
            byte[] buffer,
            bool isExtendedReferral,
            out byte[] respOutput,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            uint status = 0;
            Packet_Header header;
            IOCTL_Response ioCtlResponse;
            byte[] respInput;
            FILEID ioCtlFileId = new FILEID();
            ioCtlFileId.Persistent = 0xFFFFFFFFFFFFFFFF;
            ioCtlFileId.Volatile = 0xFFFFFFFFFFFFFFFF;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                isExtendedReferral ? CtlCode_Values.FSCTL_DFS_GET_REFERRALS_EX : CtlCode_Values.FSCTL_DFS_GET_REFERRALS,
                ioCtlFileId,
                0,
                buffer,
                DefaultMaxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out respInput,
                out respOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint ReadHash(
            uint treeId,
            FILEID fileId,
            SRV_READ_HASH_Request_HashType_Values hashType,
            SRV_READ_HASH_Request_HashVersion_Values hashVersion,
            SRV_READ_HASH_Request_HashRetrievalType_Values hashRetrievalType,
            ulong hashOffset,
            uint hashLength,
            uint maxOutputResponse,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            SRV_READ_HASH_Request readHash = new SRV_READ_HASH_Request();
            readHash.HashType = hashType;
            readHash.HashVersion = hashVersion;
            readHash.HashRetrievalType = hashRetrievalType;
            readHash.Offset = hashOffset;
            readHash.Length = hashLength;

            byte[] requestInput = TypeMarshal.ToBytes(readHash);
            byte[] responseInput;
            byte[] responseOutput;

            Packet_Header header;
            IOCTL_Response ioCtlResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_SRV_READ_HASH,
                fileId,
                0,
                requestInput,
                maxOutputResponse,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out responseInput,
                out responseOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint OffloadRead(
            uint treeId,
            FILEID fileId,
            ulong fileOffset,
            ulong copyLength,
            out ulong transferLength,
            out STORAGE_OFFLOAD_TOKEN token,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            FSCTL_OFFLOAD_READ_INPUT offloadReadInput = new FSCTL_OFFLOAD_READ_INPUT();
            offloadReadInput.Size = 32;
            offloadReadInput.FileOffset = fileOffset;
            offloadReadInput.CopyLength = copyLength;

            byte[] requestInput = TypeMarshal.ToBytes(offloadReadInput);
            byte[] responseInput;
            byte[] responseOutput;

            Packet_Header header;
            IOCTL_Response ioCtlResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_OFFLOAD_READ,
                fileId,
                0,
                requestInput,
                32000,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out responseInput,
                out responseOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            if (responseOutput != null)
            {
                var offloadReadOutput = TypeMarshal.ToStruct<FSCTL_OFFLOAD_READ_OUTPUT>(responseOutput);
                transferLength = offloadReadOutput.TransferLength;
                token = offloadReadOutput.Token;
            }
            else
            {
                transferLength = 0;
                token = new STORAGE_OFFLOAD_TOKEN();
            }

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint OffloadWrite(
            uint treeId,
            FILEID fileId,
            ulong fileOffset,
            ulong copyLength,
            ulong transferOffset,
            STORAGE_OFFLOAD_TOKEN token,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            FSCTL_OFFLOAD_WRITE_INPUT offloadWriteInput = new FSCTL_OFFLOAD_WRITE_INPUT();
            offloadWriteInput.Size = 544;
            offloadWriteInput.FileOffset = fileOffset;
            offloadWriteInput.CopyLength = copyLength;
            offloadWriteInput.TransferOffset = transferOffset;
            offloadWriteInput.Token = token;

            byte[] requestInput = TypeMarshal.ToBytes(offloadWriteInput);
            byte[] responseInput;
            byte[] responseOutput;

            Packet_Header header;
            IOCTL_Response ioCtlResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_OFFLOAD_WRITE,
                fileId,
                0,
                requestInput,
                32000,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out responseInput,
                out responseOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint SetZeroData(
            uint treeId,
            FILEID fileId,
            ulong fileOffset,
            ulong beyondFileZero,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            FSCTL_SET_ZERO_DATA_Request setZeroDataRequest = new FSCTL_SET_ZERO_DATA_Request();
            setZeroDataRequest.FileOffset = fileOffset;
            setZeroDataRequest.BeyondFinalZero = beyondFileZero;
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_ZERO_DATA_Request>(setZeroDataRequest).Length;

            byte[] requestInput = TypeMarshal.ToBytes(setZeroDataRequest);

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            byte[] inputResponse;
            byte[] outputBuffer;
            Packet_Header header;
            IOCTL_Response ioCtlResponse;

            uint status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_SET_ZERO_DATA,
                fileId,
                inputBufferSize,
                requestInput,
                inputBufferSize,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out inputResponse,
                out outputBuffer,
                out header,
                out ioCtlResponse,
                sessionChannelSequence
                );


            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        public uint DuplicateExtentsToFile(
            uint treeId,
            FILEID fileId,
            long sourceFileOffset,
            long targetFileOffset,
            long byteCount,
            ResponseChecker<IOCTL_Response> checker = null)
        {
            FSCTL_DUPLICATE_EXTENTS_TO_FILE_Request request = new FSCTL_DUPLICATE_EXTENTS_TO_FILE_Request();
            request.SourceFileId = fileId;
            request.SourceFileOffset = sourceFileOffset;
            request.TargetFileOffset = targetFileOffset;
            request.ByteCount = byteCount;

            byte[] requestInput = TypeMarshal.ToBytes(request);
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_DUPLICATE_EXTENTS_TO_FILE_Request>(request).Length;
            byte[] responseInput;
            byte[] responseOutput;

            Packet_Header header;
            IOCTL_Response ioCtlResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            ConsumeCredit(messageId, creditCharge);

            uint status = client.IoCtl(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                CtlCode_Values.FSCTL_DUPLICATE_EXTENTS_TO_FILE,
                fileId,
                inputBufferSize,
                requestInput,
                inputBufferSize,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out responseInput,
                out responseOutput,
                out header,
                out ioCtlResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, ioCtlResponse);

            return status;
        }

        #endregion

        #region Query Directory
        public uint QueryDirectory(
            uint treeId,
            FileInformationClass_Values fileInfoClass,
            QUERY_DIRECTORY_Request_Flags_Values queryDirectoryFlags,
            uint fileIndex,
            FILEID fileId,
            out byte[] outputBuffer,
            ResponseChecker<QUERY_DIRECTORY_Response> checker = null)
        {
            uint status;
            uint maxOutputBufferLength = DefaultMaxOutputResponse;
            Packet_Header header;
            QUERY_DIRECTORY_Response queryDirResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.QueryDirectory(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                fileInfoClass,
                queryDirectoryFlags,
                fileIndex,
                fileId,
                "*",
                maxOutputBufferLength,
                out outputBuffer,
                out header,
                out queryDirResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, queryDirResponse);

            return status;
        }
        #endregion

        #region Query and Set Info
        /// <summary>
        /// Query File Information of the file/directory specified by fileId.
        /// </summary>
        /// <param name="treeId">Tree id used in QueryInfo request.</param>
        /// <param name="fileInfoClass">File information class</param>
        /// <param name="queryInfoFlags">Flags used in QueryInfo request</param>
        /// <param name="fileId">File id associate with the file to query.</param>
        /// <param name="inputBuffer">A buffer containing the input buffer for QueryInfo request.</param>
        /// <param name="outputBuffer">A buffer containing the information returned in the response.</param>
        /// <param name="checker">An optional checker to check the QueryInfo response.</param>
        /// <returns>The status code of querying file information.</returns>
        public uint QueryFileAttributes(
            uint treeId,
            byte fileInfoClass,
            QUERY_INFO_Request_Flags_Values queryInfoFlags,
            FILEID fileId,
            byte[] inputBuffer,
            out byte[] outputBuffer,
            ResponseChecker<QUERY_INFO_Response> checker = null)
        {
            uint maxOutputBufferLength = 1024;
            Packet_Header header;
            QUERY_INFO_Response queryInfoResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.QueryInfo(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                InfoType_Values.SMB2_0_INFO_FILE,
                fileInfoClass,
                maxOutputBufferLength,
                AdditionalInformation_Values.NONE,
                queryInfoFlags,
                fileId,
                inputBuffer,
                out outputBuffer,
                out header,
                out queryInfoResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, queryInfoResponse);

            return status;
        }

        /// <summary>
        /// Query the Security Descriptor of the file/directory specified by fileId.
        /// </summary>
        /// <param name="treeId">tree id used in QueryInfo request.</param>
        /// <param name="fileId">The file id associate with the file to query.</param>
        /// <param name="securityAttributesToQuery">The security info type to query.</param>
        /// <param name="sd">When this method returns, contains the Security Descriptor of the specified type.</param>
        /// <param name="checker">An optional checker to check the QueryInfo response.</param>
        /// <returns>The status code of querying security descriptor.</returns>
        public uint QuerySecurityDescriptor(
            uint treeId,
            FILEID fileId,
            AdditionalInformation_Values securityAttributesToQuery,
            out _SECURITY_DESCRIPTOR sd,
            ResponseChecker<QUERY_INFO_Response> checker = null)
        {
            uint maxOutputBufferLength = DefaultMaxOutputResponse;
            Packet_Header header;
            QUERY_INFO_Response queryInfoResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);
            byte[] outputBuffer;
            uint status = client.QueryInfo(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                InfoType_Values.SMB2_0_INFO_SECURITY,
                0,
                maxOutputBufferLength,
                securityAttributesToQuery,
                0,
                fileId,
                null,
                out outputBuffer,
                out header,
                out queryInfoResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, queryInfoResponse);

            sd = DtypUtility.DecodeSecurityDescriptor(outputBuffer);
            return status;
        }

        /// <summary>
        /// Query the Object Store Information of the file/directory specified by fileId.
        /// </summary>
        /// <param name="treeId">Tree id used in QueryInfo request.</param>
        /// <param name="fileInfoClass">File information class</param>
        /// <param name="fileId">File id associate with the file to query.</param>
        /// <param name="outputBuffer">A buffer containing the information returned in the response.</param>
        /// <param name="checker">An optional checker to check the QueryInfo response.</param>
        /// <returns>The status code of querying object store information.</returns>
        public uint QueryFSAttributes(
            uint treeId,
            byte fileInfoClass,
            FILEID fileId,
            out byte[] outputBuffer,
            ResponseChecker<QUERY_INFO_Response> checker = null)
        {
            uint maxOutputBufferLength = 1024;
            Packet_Header header;
            QUERY_INFO_Response queryInfoResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            QUERY_INFO_Request_Flags_Values queryInfoFlags = QUERY_INFO_Request_Flags_Values.V1;

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.QueryInfo(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                InfoType_Values.SMB2_0_INFO_FILESYSTEM,
                fileInfoClass,
                maxOutputBufferLength,
                AdditionalInformation_Values.NONE,
                queryInfoFlags,
                fileId,
                null,
                out outputBuffer,
                out header,
                out queryInfoResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, queryInfoResponse);

            return status;
        }

        /// <summary>
        /// Query File Quota Information of the file/directory specified by fileId.
        /// </summary>
        /// <param name="treeId">Tree id used in QueryInfo request.</param>
        /// <param name="queryInfoFlags">Flags used in QueryInfo request</param>
        /// <param name="fileId">File id associate with the file to query.</param>
        /// <param name="inputBuffer">A buffer containing the input buffer for QueryInfo request.</param>
        /// <param name="outputBuffer">A buffer containing the information returned in the response.</param>
        /// <param name="checker">An optional checker to check the QueryInfo response.</param>
        /// <returns>The status code of querying file quota information.</returns>
        public uint QueryFileQuotaInfo(
            uint treeId,
            QUERY_INFO_Request_Flags_Values queryInfoFlags,
            FILEID fileId,
            byte[] inputBuffer,
            out byte[] outputBuffer,
            ResponseChecker<QUERY_INFO_Response> checker = null)
        {
            uint maxOutputBufferLength = 1024;
            Packet_Header header;
            QUERY_INFO_Response queryInfoResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.QueryInfo(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                InfoType_Values.SMB2_0_INFO_QUOTA,
                (byte)FileInformationClasses.FileQuotaInformation,
                maxOutputBufferLength,
                AdditionalInformation_Values.NONE,
                queryInfoFlags,
                fileId,
                inputBuffer,
                out outputBuffer,
                out header,
                out queryInfoResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);
            InnerResponseChecker(checker, header, queryInfoResponse);
            return status;
        }

        /// <summary>
        /// Query the Security Descriptor of the file/directory specified by fileId.
        /// </summary>
        /// <param name="treeId">tree id used in SetInfo request.</param>
        /// <param name="fileId">The file id associate with the file to query.</param>
        /// <param name="securityAttributesToQuery">The security info type to set.</param>
        /// <param name="checker">An optional checker to check the SetInfo response.</param>
        /// <returns>The status code of SetInfo response.</returns>
        public uint SetSecurityDescriptor(
            uint treeId,
            FILEID fileId,
            SET_INFO_Request_AdditionalInformation_Values securityAttributesToApply,
            _SECURITY_DESCRIPTOR sd,
            ResponseChecker<SET_INFO_Response> checker = null)
        {
            Packet_Header header;
            SET_INFO_Response setInfoResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.SetInfo(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                (testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE),
                messageId,
                sessionId,
                treeId,
                SET_INFO_Request_InfoType_Values.SMB2_0_INFO_SECURITY,
                0,
                securityAttributesToApply,
                fileId,
                DtypUtility.EncodeSecurityDescriptor(sd),
                out header,
                out setInfoResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, setInfoResponse);

            return status;
        }

        public uint SetFileAttributes(
            uint treeId,
            byte fileInfoClass,
            FILEID fileId,
            byte[] inputBuffer,
            ResponseChecker<SET_INFO_Response> checker = null,
            bool isReplay = false)
        {
            Packet_Header header;
            SET_INFO_Response setInfoResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.SetInfo(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                (testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE) | (isReplay ? Packet_Header_Flags_Values.FLAGS_REPLAY_OPERATION : Packet_Header_Flags_Values.NONE),
                messageId,
                sessionId,
                treeId,
                SET_INFO_Request_InfoType_Values.SMB2_0_INFO_FILE,
                fileInfoClass,
                SET_INFO_Request_AdditionalInformation_Values.NONE,
                fileId,
                inputBuffer,
                out header,
                out setInfoResponse,
                sessionChannelSequence);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, setInfoResponse);

            return status;
        }

        public void SetFileAttributesRequest(
            uint treeId,
            byte fileInfoClass,
            FILEID fileId,
            byte[] inputBuffer,
            out ulong messageId)
        {
            messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            client.SetInfoRequest(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                SET_INFO_Request_InfoType_Values.SMB2_0_INFO_FILE,
                fileInfoClass,
                SET_INFO_Request_AdditionalInformation_Values.NONE,
                fileId,
                inputBuffer);
        }

        public uint SetInfoResponse(
            ulong messageId,
            ResponseChecker<SET_INFO_Response> checker = null)
        {
            Packet_Header header;
            SET_INFO_Response setupResponse;

            uint status = client.SetInfoResponse(
                messageId,
                out header,
                out setupResponse
                );

            ProduceCredit(header.MessageId, header);

            InnerResponseChecker(checker, header, setupResponse);

            return status;
        }

        #endregion

        #region Change Notify
        public void ChangeNotify(
            uint treeId,
            FILEID fileId,
            CompletionFilter_Values completionFilter,
            CHANGE_NOTIFY_Request_Flags_Values flags = CHANGE_NOTIFY_Request_Flags_Values.NONE,
            uint maxOutputBufferLength = DefaultMaxOutputResponse)
        {
            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            client.ChangeNotify(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                maxOutputBufferLength,
                fileId,
                flags,
                completionFilter);

            /// TODO: granted credit is in  interim response
            /// Assuming server grant at least 1 for now
            Packet_Header header = new Packet_Header();
            header.CreditRequestResponse = 1;
            ProduceCredit(messageId, header);
        }
        #endregion

        #region Lock and Unlock
        public uint Lock(
            uint treeId,
            uint lockSequence,
            FILEID fileId,
            LOCK_ELEMENT[] locks,
            ResponseChecker<LOCK_Response> checker = null)
        {
            uint status;

            Packet_Header header;
            LOCK_Response lockResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.Lock(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                lockSequence,
                fileId,
                locks,
                out header,
                out lockResponse);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, lockResponse);

            return status;
        }

        public void LockRequest(
            uint treeId,
            uint lockSequence,
            FILEID fileId,
            LOCK_ELEMENT[] locks,
            out ulong messageId)
        {
            messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            client.LockRequest(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                lockSequence,
                fileId,
                locks);
        }

        public uint LockResponse(
            ulong messageId,
            ResponseChecker<LOCK_Response> checker = null)
        {
            Packet_Header header;
            LOCK_Response lockResponse;

            uint status = client.LockResponse(
                messageId,
                out header,
                out lockResponse
                );

            ProduceCredit(header.MessageId, header);

            InnerResponseChecker(checker, header, lockResponse);

            return status;
        }

        #endregion

        #region Echo

        public uint Echo(uint treeId, ResponseChecker<ECHO_Response> checker = null)
        {
            uint status;
            Packet_Header header;
            ECHO_Response echoResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            status = client.Echo(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                out header,
                out echoResponse);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, echoResponse);

            return status;
        }

        #endregion

        #region Cancel
        public void Cancel()
        {
            client.Cancel(testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE, generateMessageId(sequenceWindow) - 1, sessionId);
        }
        #endregion

        #region Oplock Break

        public uint OplockAcknowledgement(uint treeId, FILEID fileId, OPLOCK_BREAK_Acknowledgment_OplockLevel_Values oplockLevel, ResponseChecker<OPLOCK_BREAK_Response> checker = null)
        {
            Packet_Header header;
            OPLOCK_BREAK_Response oplockBreakResp;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.OplockBreakAcknowledgment(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                fileId,
                oplockLevel,
                out header,
                out oplockBreakResp);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, oplockBreakResp);

            return status;
        }

        public uint LeaseBreakAcknowledgment(
            uint treeId,
            Guid leaseKey,
            LeaseStateValues leaseState,
            ResponseChecker<LEASE_BREAK_Response> checker = null)
        {
            Packet_Header header;
            LEASE_BREAK_Response leaseBreakResp;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = client.LeaseBreakAcknowledgment(
                creditCharge,
                generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId,
                sessionId,
                treeId,
                leaseKey,
                leaseState,
                out header,
                out leaseBreakResp);

            ProduceCredit(messageId, header);

            InnerResponseChecker(checker, header, leaseBreakResp);

            return status;
        }

        #endregion

        #endregion

        /// <summary>
        /// Send compounded request which contains a list of single packets.
        /// And expect to receive a compounded response which contains a list of packets or a list of separate packets
        /// </summary>
        /// <param name="related">Indicate whether the compounded request are related or unrelated.</param>
        /// <param name="requestPackets">The list of packets contained in the compounded request</param>
        /// <returns>Return the list of response packets</returns>
        public List<Smb2SinglePacket> SendAndReceiveCompoundPacket(bool related, List<Smb2SinglePacket> requestPackets)
        {
            Smb2CompoundPacket compoundRequest = new Smb2CompoundPacket();
            compoundRequest.Packets = requestPackets;

            List<ulong> messageIdList = new List<ulong>();
            for (int i = 0; i < requestPackets.Count; ++i)
            {
                Smb2SinglePacket packet = requestPackets[i];
                // Set MessageId, credit and Flags before sending the compound packet.
                ulong messageId = generateMessageId(sequenceWindow);
                ushort creditCharge = generateCreditCharge(1);
                ConsumeCredit(messageId, creditCharge);
                packet.Header.MessageId = messageId;
                packet.Header.CreditCharge = creditCharge;
                packet.Header.CreditRequestResponse = generateCreditRequest(sequenceWindow, creditGoal, creditCharge);
                if (testConfig.SendSignedRequest)
                {
                    packet.Header.Flags |= Packet_Header_Flags_Values.FLAGS_SIGNED;
                }

                // If the packet is not the last one in the chain, do 8-byte alignment and calculate value of NextCommand
                // The last packet in the chain doesn't need to do this.
                if (i != requestPackets.Count - 1)
                {
                    // The message should be padded to an 8-byte boundary.
                    uint packetLength = (uint)packet.ToBytes().Length;
                    uint alignedPacketLength = packetLength;
                    Smb2Utility.Align8(ref alignedPacketLength);
                    packet.Padding = new byte[alignedPacketLength - packetLength];

                    // This field MUST be set to the offset, in bytes, from the beginning of this SMB2 header to the start of the subsequent 8-byte aligned SMB2 header.
                    packet.Header.NextCommand = alignedPacketLength;
                }

                messageIdList.Add(messageId);
            }

            client.SendPacket(compoundRequest);

            // response can come either compounded or not
            var responsePackets = new List<Smb2SinglePacket>();

            var firstPacket = client.ExpectPacket<Smb2Packet>(messageIdList[0]);

            if (firstPacket is Smb2CompoundPacket)
            {
                var compoundedPacket = firstPacket as Smb2CompoundPacket;

                if (related)
                {
                    // If the responses are compounded, the server MUST set SMB2_FLAGS_RELATED_OPERATIONS in the Flags field of the SMB2 header of all responses except the first one. 
                    // This indicates that the response was part of a compounded chain.
                    bool checkRelatedOperations = compoundedPacket.Packets.Skip(1).All(innerPacket => innerPacket.Header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS));
                    if (!checkRelatedOperations)
                    {
                        throw new InvalidOperationException("FLAGS_RELATED_OPERATIONS is not set for all compounded request except the first.");
                    }
                }

                responsePackets.AddRange(compoundedPacket.Packets);
            }
            else if (firstPacket is Smb2SinglePacket)
            {
                // response is not compounded
                responsePackets.Add(firstPacket as Smb2SinglePacket);

                // expect the others by their MessageId
                foreach (ulong messageId in messageIdList.Skip(1))
                {
                    var packetAfterFirst = client.ExpectPacket<Smb2SinglePacket>(messageId);
                    responsePackets.Add(packetAfterFirst);
                }
            }
            else
            {
                throw new InvalidOperationException("Unexpected packet type!");
            }


            foreach (var response in responsePackets)
            {
                ProduceCredit(response.Header.MessageId, response.Header);
            }

            return responsePackets;
        }

        #region event handler
        public void BeforeSendingPacket(Action<Smb2Packet> client_PacketSending)
        {
            client.PacketSending += client_PacketSending;
        }

        #endregion

        #region Default Generator

        /// <summary>
        /// Default generator for message id
        /// </summary>
        /// <param name="sequenceWindow"></param>
        /// <returns></returns>
        public static ulong GetDefaultMId(SortedSet<ulong> sequenceWindow)
        {
            return sequenceWindow.Min;
        }

        /// <summary>
        /// Default generator for credit charge
        /// </summary>
        /// <param name="payloadSize">Payload size of the request/response</param>
        /// <returns></returns>
        public static ushort GetDefaultCreditCharge(uint payloadSize)
        {
            return (ushort)(1 + ((payloadSize - 1) / 65536));
        }

        /// <summary>
        /// Default generator for credit request
        /// </summary>
        /// <param name="sequenceWindow"></param>
        /// <param name="creditGoal"></param>
        /// <param name="creditCharge"></param>
        /// <returns></returns>
        public static ushort GetDefaultCreditRequest(SortedSet<ulong> sequenceWindow, ushort creditGoal, ushort creditCharge)
        {
            /// If client holds no less credit than our credit goal
            /// Then just maintain the credit
            if (sequenceWindow.Count - creditGoal >= 0)
            {
                return creditCharge;
            }
            else
            {
                /// If client holds less credit than our credit goal
                /// Then try to request to fill the gap
                return (ushort)(creditGoal - sequenceWindow.Count);
            }
        }

        #endregion


        #region Protected Methods
        protected void ConsumeCredit(ulong startMId, ushort creditCharge)
        {
            ulong mId = startMId;

            if (!sequenceWindow.Contains(startMId)
                || sequenceWindow.Count < creditCharge
                || sequenceWindow.Max < startMId + creditCharge - 1)
            {
                // Skip consuming invalid mid range
                return;
            }

            lock (sequenceWindow)
            {
                if (creditCharge == 0)
                {
                    sequenceWindow.Remove(mId);

                    maxMidEverProduced = sequenceWindow.Count == 0 ? mId : sequenceWindow.Max;
                }
                else
                {
                    for (; creditCharge > 0; creditCharge--)
                    {
                        sequenceWindow.Remove(mId++);
                    }

                    maxMidEverProduced = sequenceWindow.Count == 0 ? mId - 1 : sequenceWindow.Max;
                }
            }
        }

        protected void ProduceCredit(ulong mIdInRequest, Packet_Header header)
        {
            // Credits granted this time
            ushort creditGranted = header.CreditRequestResponse;

            lock (sequenceWindow)
            {
                for (; creditGranted > 0; creditGranted--)
                {
                    sequenceWindow.Add(++maxMidEverProduced);
                }
            }

            if (RequestSent != null)
            {
                RequestSent(header);
            }
        }
        #endregion

        #region Private Methods
        private SspiClientSecurityContext sspiClientGss = null;
        private bool needContinueAuthenticating = false;
        private uint SessionSetup(
            Packet_Header_Flags_Values headerFlags,
            SESSION_SETUP_Request_Flags sessionSetupFlags,
            SESSION_SETUP_Request_SecurityMode_Values securityMode,
            SESSION_SETUP_Request_Capabilities_Values capabilities,
            ulong previousSessionId,
            SecurityPackageType securityPackageType,
            string serverName,
            AccountCredential credential,
            bool useServerGssToken,
            bool allowPartialAuthentication = false,
            bool isMultipleChannelSupported = true,
            ResponseChecker<SESSION_SETUP_Response> checker = null,
            bool invalidToken = false)
        {
            Packet_Header header;
            SESSION_SETUP_Response sessionSetupResponse;

            if (sspiClientGss == null || !needContinueAuthenticating)
            {
                sspiClientGss =
                    new SspiClientSecurityContext(
                        securityPackageType,
                        credential,
                        Smb2Utility.GetCifsServicePrincipalName(serverName),
                        ClientSecurityContextAttribute.None,
                        SecurityTargetDataRepresentation.SecurityNativeDrep);

                // Server GSS token is used only for Negotiate authentication when enabled
                if (securityPackageType == SecurityPackageType.Negotiate && useServerGssToken)
                    sspiClientGss.Initialize(serverGssToken);
                else
                    sspiClientGss.Initialize(null);
            }
            uint status;
            ulong messageId;
            ushort creditCharge;

            do
            {
                messageId = generateMessageId(sequenceWindow);
                creditCharge = generateCreditCharge(1);

                // Need to consume credit from sequence window first according to TD
                ConsumeCredit(messageId, creditCharge);

                // Modify the first byte of token to make it invalid.
                if (invalidToken)
                    sspiClientGss.Token[0] = (byte)(sspiClientGss.Token[0] + 1);

                status = client.SessionSetup(
                    creditCharge,
                    generateCreditRequest(sequenceWindow, creditGoal, creditCharge),
                    headerFlags,
                    messageId,
                    sessionId,
                    sessionSetupFlags,
                    securityMode,
                    capabilities,
                    previousSessionId,
                    sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out header,
                    out sessionSetupResponse);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }
                if (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    needContinueAuthenticating = true;
                }

                ProduceCredit(messageId, header);

            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED && !allowPartialAuthentication);

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                sessionKey = sspiClientGss.SessionKey;

                // Enable/Disable signing according to test config.
                // If server supports signing (testConfig.IsSigningSupported is true), all test cases (except signning related cases) should 
                // set Packet_Header_Flags_Values.FLAGS_SIGNED in the header flags for requests other than Negotiate and SessionSetup.                             
                if (sessionSetupFlags == SESSION_SETUP_Request_Flags.SESSION_FLAG_BINDING && isMultipleChannelSupported)
                {
                    // For bind session
                    // Set isBinding = true to regenerate the channel.signingkey after session setup succeeds.
                    GenerateCryptoKeys(testConfig.SendSignedRequest, false, isBinding: true);
                }
                else
                {
                    // For new session
                    GenerateCryptoKeys(testConfig.SendSignedRequest, false);
                }

                needContinueAuthenticating = false;
                if (testConfig.IsGlobalEncryptDataEnabled && Dialect >= DialectRevision.Smb30 && Dialect != DialectRevision.Smb2Unknown)
                {
                    EnableSessionSigningAndEncryption(testConfig.SendSignedRequest, true);
                }
            }

            // The signature of Session Setup Response can only be verified after the crypto key is generated.
            client.TryVerifySessionSetupResponseSignature(sessionId);

            InnerResponseChecker(checker, header, sessionSetupResponse);

            return status;
        }

        private void InnerResponseChecker<T>(ResponseChecker<T> checker, Packet_Header header, T response)
        {
            if (checker != null)
            {
                checker(header, response);
            }
            else if (header.Status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(
                    string.Format("Unexpected status {0} for {1}",
                    Smb2Status.GetStatusCode(header.Status),
                    header.Command));
            }
        }

        private void client_PacketSent(Smb2Packet obj)
        {
            this.baseTestSite.Log.Add(LogEntryKind.Debug, obj.ToString());
        }

        private void client_PacketReceived(Smb2Packet obj)
        {
            this.baseTestSite.Log.Add(LogEntryKind.Debug, obj.ToString());
        }

        private void client_PendingResponseReceived(Smb2SinglePacket pendingResponse)
        {
            // Produce credit from interim response as
            // server will set the CreditResponse field to the number of credits the server chooses to grant for this request
            ProduceCredit(pendingResponse.Header.MessageId, pendingResponse.Header);
        }

        private void client_NotificationThreadExceptionHappened(Exception exception)
        {
            // Print the exception happened in notification thread.
            baseTestSite.Log.Add(LogEntryKind.Debug, "Exception happened in notification thread: {0}.", exception);
        }

        private void SetCreditGoal()
        {
            // Calculate how many credits the maxBufferSize will consume
            ushort maxBufferSizeInCredit = (ushort)(1 + (MaxBufferSize - 1) / 65536);

            // Set creditGoal to expect server grant credits that could at leaset accept request with max buffer size
            CreditGoal = maxBufferSizeInCredit;
        }

        #region SMB2 TREE_CONNECT Request Extension
        private byte[] CreateTreeConnectRequestExt(string sharePath, WindowsIdentity identity)
        {
            ushort treeConnectContextCount = 1;
            byte[] pathName = Encoding.Unicode.GetBytes(sharePath);
            uint treeConnectContextOffset = (uint)(64 + // SMB2 header
                                                   2 +  // StructureSize
                                                   2 +  // Flags
                                                   2 +  // PathOffset
                                                   2 +  // PathLength
                                                   4 +  // TreeConnectContextOffset
                                                   2 +  // TreeConnectContextCount
                                                   10 + // Reserved
                                                   pathName.Length);
            Smb2Utility.Align8(ref treeConnectContextOffset);

            byte[] buffer = TypeMarshal.ToBytes<uint>(treeConnectContextOffset);
            buffer = buffer.Concat(TypeMarshal.ToBytes<ushort>(treeConnectContextCount)).ToArray();
            buffer = buffer.Concat(new byte[10]).ToArray();
            buffer = buffer.Concat(pathName).ToArray();
            Smb2Utility.Align8(0, ref buffer);

            byte[] ctxBuf = CreateTreeConnectContext(identity);
            Smb2Utility.Align8(0, ref ctxBuf);
            buffer = buffer.Concat(ctxBuf).ToArray();
            return buffer;
        }

        private byte[] CreateTreeConnectContext(WindowsIdentity identity)
        {
            byte[] buffer;
            Tree_Connect_Context ctx = new Tree_Connect_Context();
            ctx.ContextType = Context_Type.RESERVED_TREE_CONNECT_CONTEXT_ID;
            byte[] remotedIdentityBuf = CreateRemotedIdentity(out ctx.DataLength, identity);
            buffer = TypeMarshal.ToBytes<Context_Type>(ctx.ContextType);
            buffer = buffer.Concat(TypeMarshal.ToBytes<ushort>(ctx.DataLength)).ToArray();
            buffer = buffer.Concat(TypeMarshal.ToBytes<uint>(ctx.Reserved)).ToArray();
            buffer = buffer.Concat(remotedIdentityBuf).ToArray();
            return buffer;
        }

        private byte[] CreateRemotedIdentity(out ushort dataLength, WindowsIdentity identity)
        {
            REMOTED_IDENTITY_TREE_CONNECT_Context remotedIdentity = new REMOTED_IDENTITY_TREE_CONNECT_Context();
            ushort currentOffset = 0;
            remotedIdentity.TicketType = 0x0001;
            currentOffset += 2 * 14;  // TicketType, TicketSize, User, UserName, Domain, Groups, RestrictedGroups, Privileges, PrimaryGroup, Owner, DefaultDacl, DeviceGroups, UserClaims, DeviceClaims

            // User: SID_ATTR_DATA
            remotedIdentity.User = currentOffset;
            byte[] userBinary = new byte[identity.User.BinaryLength];
            identity.User.GetBinaryForm(userBinary, 0);
            remotedIdentity.TicketInfo.User = new SID_ATTR_DATA();
            remotedIdentity.TicketInfo.User.SidData = new BLOB_DATA();
            remotedIdentity.TicketInfo.User.SidData.BlobData = userBinary;
            remotedIdentity.TicketInfo.User.SidData.BlobSize = (ushort)identity.User.BinaryLength;
            remotedIdentity.TicketInfo.User.Attr = (SID_ATTR)0;
            currentOffset += (ushort)(2 /* BlobSize */ + remotedIdentity.TicketInfo.User.SidData.BlobSize + 4 /* Attr */);

            // UserName: null-terminated Unicode string
            remotedIdentity.UserName = currentOffset;
            remotedIdentity.TicketInfo.UserName = Encoding.Unicode.GetBytes(identity.Name.Split('\\')[1] + '\x0');
            ushort userNameLen = (ushort)remotedIdentity.TicketInfo.UserName.Length;
            currentOffset += (ushort)(remotedIdentity.TicketInfo.UserName.Length + 2 /* '\x0' */);

            // Domain: null-terminated Unicode string
            remotedIdentity.Domain = currentOffset;
            remotedIdentity.TicketInfo.Domain = Encoding.Unicode.GetBytes(identity.Name.Split('\\')[0] + '\x0');
            ushort domainLen = (ushort)remotedIdentity.TicketInfo.Domain.Length;
            currentOffset += (ushort)(remotedIdentity.TicketInfo.Domain.Length + 2 /* '\x0' */);

            // Groups: SID_ARRAY_DATA
            remotedIdentity.Groups = currentOffset;
            remotedIdentity.TicketInfo.Groups = new SID_ARRAY_DATA();
            remotedIdentity.TicketInfo.Groups.SidAttrCount = (ushort)identity.Groups.Count;
            SID_ATTR_DATA[] groups = new SID_ATTR_DATA[identity.Groups.Count];
            for (int i = 0; i < identity.Groups.Count; i++)
            {
                SecurityIdentifier curGroupSid = (SecurityIdentifier)identity.Groups[i];
                byte[] curGroupBinary = new byte[curGroupSid.BinaryLength];
                curGroupSid.GetBinaryForm(curGroupBinary, 0);
                groups[i].SidData.BlobSize = (ushort)curGroupSid.BinaryLength;
                groups[i].SidData.BlobData = curGroupBinary;
                groups[i].Attr = SID_ATTR.SE_GROUP_ENABLED;
                currentOffset += (ushort)(2 /* BlobSize */ + groups[i].SidData.BlobSize + 4 /* Attr */);
            }
            currentOffset += 2; // SidAttrCount

            // RestrictedGroups: SID_ARRAY_DATA
            remotedIdentity.RestrictedGroups = 0;

            // PrimaryGroup: SID_ARRAY_DATA
            remotedIdentity.PrimaryGroup = 0;

            // Owner: BLOB_DATA
            remotedIdentity.Owner = currentOffset;
            remotedIdentity.TicketInfo.Owner = new BLOB_DATA();
            byte[] ownerBinary = new byte[identity.Owner.BinaryLength];
            identity.Owner.GetBinaryForm(ownerBinary, 0);
            remotedIdentity.TicketInfo.Owner.BlobData = ownerBinary;
            remotedIdentity.TicketInfo.Owner.BlobSize = (ushort)identity.Owner.BinaryLength;
            currentOffset += (ushort)(2 /* BlobSize */ + remotedIdentity.TicketInfo.Owner.BlobSize);

            // DefaultDacl: BLOB_DATA
            remotedIdentity.DefaultDacl = 0;

            // DeviceGroups: SID_ARRAY_DATA
            remotedIdentity.DeviceGroups = 0;

            // UserClaims: BLOB_DATA
            remotedIdentity.UserClaims = 0;

            // DeviceClaims: BLOB_DATA
            remotedIdentity.DeviceClaims = 0;

            remotedIdentity.TicketSize = currentOffset;
            dataLength = currentOffset;

            byte[] ctxBuf = TypeMarshal.ToBytes<ushort>(remotedIdentity.TicketType);
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.TicketSize)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.User)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.UserName)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.Domain)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.Groups)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.RestrictedGroups)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.Privileges)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.PrimaryGroup)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.Owner)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.DefaultDacl)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.DeviceGroups)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.UserClaims)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.DeviceClaims)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<SID_ATTR_DATA>(remotedIdentity.TicketInfo.User)).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(userNameLen)).ToArray();
            ctxBuf = ctxBuf.Concat(remotedIdentity.TicketInfo.UserName).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(domainLen)).ToArray();
            ctxBuf = ctxBuf.Concat(remotedIdentity.TicketInfo.Domain).ToArray();
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(remotedIdentity.TicketInfo.Groups.SidAttrCount)).ToArray();
            for (int i = 0; i < identity.Groups.Count; i++)
            {
                ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<ushort>(groups[i].SidData.BlobSize)).ToArray();
                ctxBuf = ctxBuf.Concat(groups[i].SidData.BlobData).ToArray();
                ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<SID_ATTR>(groups[i].Attr)).ToArray();
            }
            ctxBuf = ctxBuf.Concat(TypeMarshal.ToBytes<BLOB_DATA>(remotedIdentity.TicketInfo.Owner)).ToArray();
            return ctxBuf;
        }
        #endregion

        #endregion

    }
}
