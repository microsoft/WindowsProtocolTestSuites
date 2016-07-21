// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC Server SDK
    /// </summary>
    public class NrpcServer : IDisposable
    {
        private NrpcContextManager contextManager;

        private RpceServerTransport rpceLayerServer;

        private RpceSecurityContextCreatingEventHandler createSecurityContextImp;

        internal Dictionary<string, NrpcClientSessionInfo> clientSessionInfo;

        private static Thread secureChannelThread;

        // Computer name length is 16, static size as TD IDL in section 6
        // ComputerName of _UAS_INFO_0 is used only for NetrAccountDeltas and NetrAccountSync that are obsolete
        private const int COMPUTER_NAME_LENGTH = 16;

        #region Constructor

        /// <summary>
        /// Constructor, initialize a NRPC server.<para/>
        /// Create the instance will not listen for RPC calls, 
        /// you should call StartTcp or StartNamedPipe 
        /// to actually listen for RPC calls.
        /// Delegate RpceSecurityCreator will be called to create server security
        /// context for Kerberos, NLMP, etc.
        /// If the requested security package type is Netlogon, and the user doesn't return an it 
        /// in the delegate function, the SDK will create it for users.
        /// </summary>     
        ///  <param name="rpceServerSecurityContextCreator">Server security creator</param>
        public NrpcServer(RpceSecurityContextCreatingEventHandler rpceServerSecurityContextCreator)
        {
            contextManager = new NrpcContextManager();
            rpceLayerServer = new RpceServerTransport();
            rpceLayerServer.RegisterInterface(
                NrpcUtility.NETLOGON_RPC_INTERFACE_UUID,
                NrpcUtility.NETLOGON_RPC_INTERFACE_MAJOR_VERSION,
                NrpcUtility.NETLOGON_RPC_INTERFACE_MINOR_VERSION);
            rpceLayerServer.SetSecurityContextCreator(CreateSecurityContext);
            createSecurityContextImp = rpceServerSecurityContextCreator;
            clientSessionInfo = new Dictionary<string, NrpcClientSessionInfo>();
        }

        #endregion


        /// <summary>
        /// Called when the underlying RPCE SDK need a server security context. The method will call user delegate
        /// function to create the security context, and if user doesn't create a Netlogon security context,
        /// this method will create it for the user.
        /// </summary>
        ///<param name="sessionContext">Context of the session</param>
        /// <returns>A server secruity context</returns>
        private ServerSecurityContext CreateSecurityContext(RpceServerSessionContext sessionContext)
        {
            if (createSecurityContextImp != null)
            {
                ServerSecurityContext ret = createSecurityContextImp(sessionContext);
                if (ret != null)
                {
                    return ret;
                }
            }

            if (sessionContext.AuthenticationType != RpceAuthenticationType.RPC_C_AUTHN_NETLOGON)
            {
                throw new InvalidOperationException("non-Netlogon security context isn't created");
            }

            return new NrpcServerSecurityContext();
        }


        /// <summary>
        /// Throws an exception when session context is null
        /// </summary>
        ///<param name="sessionContext">Context of the session</param>
        ///<exception cref="ArgumentNullException">Thrown when session context is null</exception>
        private void CheckIfSessionContextIsNull(NrpcServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }
        }


        /// <summary>
        /// Computes ReturnAuthenticator for users
        /// </summary>
        ///<param name="sessionContext">Context of the session</param>
        ///<returns>ReturnAuthenticator computed</returns>
        private _NETLOGON_AUTHENTICATOR ComputeAuthenticator(NrpcServerSessionContext sessionContext)
        {
            if (sessionContext.SessionKey == null || sessionContext.StoredCredential == null)
            {
                throw new InvalidOperationException("Unable to compute server authenticator");
            }

            NrpcComputeNetlogonCredentialAlgorithm algorithm;
            if ((sessionContext.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0)
            {
                algorithm = NrpcComputeNetlogonCredentialAlgorithm.AES128;
            }
            else
            {
                algorithm = NrpcComputeNetlogonCredentialAlgorithm.DESECB;
            }

            byte[] serverStoredCredential = sessionContext.StoredCredential;
            _NETLOGON_AUTHENTICATOR returnAuthenticator = NrpcUtility.ComputeServerNetlogonAuthenticator(
                algorithm, ref serverStoredCredential, sessionContext.SessionKey);
            sessionContext.StoredCredential = serverStoredCredential;
            return returnAuthenticator;
        }


        #region User interface for NRPC protocol
        /// <summary>
        ///  Starts to listen a TCP port
        /// </summary>
        /// <param name="port">The TCP port to listen</param>
        public void StartTcp(ushort port)
        {
            rpceLayerServer.StartTcp(port);
        }


        /// <summary>
        ///  Stops to listen a TCP port
        /// </summary>
        /// <param name="port">The TCP port listened</param>
        public void StopTcp(ushort port)
        {
            rpceLayerServer.StopTcp(port);
        }


        /// <summary>
        ///  Starts to listen a named pipe
        /// </summary>
        /// <param name="namedPipe">Name of the named pipe</param>
        /// <param name="credential">Credential to be used by underlayer SMB/SMB2 transport.</param>
        /// <param name="ipAddress">server's ipAddress</param>
        public void StartNamedPipe(string namedPipe, AccountCredential credential, IPAddress ipAddress)
        {
            rpceLayerServer.StartNamedPipe(namedPipe, credential, ipAddress);
        }


        /// <summary>
        ///  Stops to listen a named pipe
        /// </summary>
        /// <param name="namedPipe">Name of the named pipe</param>
        public void StopNamedPipe(string namedPipe)
        {
            rpceLayerServer.StopNamedPipe(namedPipe);
        }


        /// <summary>
        ///  Receives RPC calls from clients
        /// </summary>
        /// <param name="timeout">The maximum time waiting for RPC calls</param>
        /// <param name="sessionContext">The session context of the RPC call received</param>
        /// <returns>The input parameters of the RPC call received</returns>
        /// <exception cref="InvalidOperationException">Thrown when an invalid method is called, or
        /// an unexpected method is called</exception>
        public T ExpectRpcCall<T>(TimeSpan timeout, out NrpcServerSessionContext sessionContext)
            where T : NrpcRequestStub
        {
            RpceServerSessionContext rpceSessionContext;
            ushort opnum;

            byte[] requestStub = rpceLayerServer.ExpectCall(timeout, out rpceSessionContext, out opnum);

            if (!Enum.IsDefined(typeof(NrpcMethodOpnums), (int)opnum))
            {
                throw new InvalidOperationException("An invalid method is invoked");
            }

            //If there isn't a corresponding nrpc session context, it's a new session
            if (contextManager.LookupSessionContext(rpceSessionContext, out sessionContext))
            {
                sessionContext.RpceLayerSessionContext = rpceSessionContext;
                sessionContext.ClientSessionInfoTable = clientSessionInfo;
            }

            T t;
            if (typeof(T) == typeof(NrpcRequestStub))
            {
                t = (T)NrpcUtility.CreateNrpcRequestStub((NrpcMethodOpnums)opnum);
            }
            else
            {
                t = (T)Activator.CreateInstance(typeof(T));
                if ((ushort)t.Opnum != opnum)
                {
                    throw new InvalidOperationException("An unexpected method call is received");
                }
            }

            //Decode the request stub
            t.Decode(sessionContext, requestStub);

            //Update the session context
            sessionContext.UpdateSessionContextWithMessageReceived(t);
            return t;
        }


        /// <summary>
        ///  Sends RPC response to the client
        /// </summary>
        /// <param name="sessionContext">The session context of the RPC response to send</param>
        /// <param name="messageToSend">The RPC response to send</param>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext or messageToSend is null.</exception>
        public void SendRpcCallResponse(NrpcServerSessionContext sessionContext, NrpcResponseStub messageToSend)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (messageToSend == null)
            {
                throw new ArgumentNullException("messageToSend");
            }

            sessionContext.UpdateSessionContextWithMessageSent(messageToSend);
            rpceLayerServer.SendResponse(sessionContext.RpceLayerSessionContext,
                messageToSend.Encode(sessionContext));
        }


        /// <summary>
        ///  Receives and processes the requests from a certain remote client to establish a secure channel
        /// </summary>
        /// <param name="timeout">The maximum time waiting for secure channel establishment</param>
        /// <param name="clientComputerName">The remote client with which to establish secure channel</param>
        /// <param name="clientMachinePassword">The remote client machine password</param>
        /// <returns>an instance of NrpcServerSecurityContextthe that indicates the secure channel is 
        /// established</returns>
        /// <exception cref="ArgumentNullException">Thrown when clientComputerName or clientMachinePassword
        /// is null.</exception>
        /// <exception cref="TimeoutException">Thrown when it is timeout</exception>
        /// <exception cref="InvalidOperationException">Thrown when the negotiation sequence doesn't 
        /// match that in TD</exception>
        public NrpcServerSecurityContext ExpectSecureChannel(TimeSpan timeout, 
            string clientComputerName, string clientMachinePassword)
        {
            if(clientComputerName == null)
            {
                throw new ArgumentNullException("clientComputerName");
            }

            if(clientMachinePassword == null)
            {
                throw new ArgumentNullException("clientMachinePassword");
            }

            NrpcServerSessionContext sessionContextLast = null;
            NrpcServerSessionContext sessionContextCurrent = null;
            DateTime startTime = DateTime.Now;

            while (DateTime.Now - startTime < timeout)
            {
                NrpcRequestStub request = ExpectRpcCall<NrpcRequestStub>(timeout, out sessionContextCurrent);

                switch (request.Opnum)
                {
                    case NrpcMethodOpnums.NetrServerReqChallenge:
                        NrpcNetrServerReqChallengeRequest nrpcNetrServerReqChallengeRequest =
                            request as NrpcNetrServerReqChallengeRequest;

                        if (nrpcNetrServerReqChallengeRequest == null)
                        {
                            throw new InvalidOperationException("request doesn't match its Opnum");
                        }

                        // receive two NetrServerReqChallenges in one session
                        if (sessionContextCurrent == sessionContextLast)
                        {
                            throw new InvalidOperationException(
                                "More than one NetrServerReqChallenge is received in one session that violates the TD");
                        }
                        else
                        {
                            // one new session
                            if (sessionContextLast == null)
                            {
                                sessionContextLast = sessionContextCurrent;
                            }
                            else
                            {
                                // receive two NetrServerReqChallenges in two different session, drop the first one
                                RemoveSessionContext(sessionContextLast);
                                sessionContextLast = sessionContextCurrent;
                            }
                        }

                        NrpcNetrServerReqChallengeResponse nrpcNetrServerReqChallengeResponse;

                        if (string.Compare(clientComputerName, sessionContextCurrent.ClientComputerName, StringComparison.Ordinal) == 0)
                        {
                            nrpcNetrServerReqChallengeResponse = CreateNetrServerReqChallengeResponse(
                                sessionContextCurrent, NrpcUtility.GenerateNonce(8), clientMachinePassword);
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerReqChallengeResponse);
                        }
                        else
                        {
                            nrpcNetrServerReqChallengeResponse = CreateNetrServerReqChallengeResponse(
                                sessionContextCurrent, NrpcUtility.GenerateNonce(8), "");
                            nrpcNetrServerReqChallengeResponse.Status = NtStatus.STATUS_ACCESS_DENIED;
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerReqChallengeResponse);
                            RemoveSessionContext(sessionContextCurrent);
                            sessionContextLast = null;
                            sessionContextCurrent = null;
                        }
                        break;

                    case NrpcMethodOpnums.NetrServerAuthenticate:
                        if (sessionContextCurrent != sessionContextLast)
                        {
                            throw new InvalidOperationException("NetrServerReqChallenge is not received.");
                        }

                        NrpcNetrServerAuthenticateRequest nrpcNetrServerAuthenticateRequest =
                            request as NrpcNetrServerAuthenticateRequest;

                        if (nrpcNetrServerAuthenticateRequest == null)
                        {
                            throw new InvalidOperationException("request doesn't match its Opnum");
                        }

                        if (string.Compare(clientComputerName, sessionContextCurrent.ClientComputerName, StringComparison.Ordinal) == 0)
                        {
                            NrpcNetrServerAuthenticateResponse nrpcNetrServerAuthenticateResponse =
                                CreateNetrServerAuthenticateResponse(sessionContextCurrent);
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerAuthenticateResponse);

                            return new NrpcServerSecurityContext(clientSessionInfo[clientComputerName]);
                        }
                        else
                        {
                            NrpcNetrServerAuthenticateResponse nrpcNetrServerAuthenticateResponse =
                                CreateNetrServerAuthenticateResponse(sessionContextCurrent);
                            nrpcNetrServerAuthenticateResponse.Status = NtStatus.STATUS_ACCESS_DENIED;
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerAuthenticateResponse);
                            RemoveSessionContext(sessionContextCurrent);
                            sessionContextLast = null;
                            sessionContextCurrent = null;
                        }
                        break;

                    case NrpcMethodOpnums.NetrServerAuthenticate2:
                        if (sessionContextCurrent != sessionContextLast)
                        {
                            throw new InvalidOperationException("NetrServerReqChallenge is not received.");
                        }

                        NrpcNetrServerAuthenticate2Request nrpcNetrServerAuthenticate2Request =
                            request as NrpcNetrServerAuthenticate2Request;

                        if (nrpcNetrServerAuthenticate2Request == null)
                        {
                            throw new InvalidOperationException("request doesn't match its Opnum");
                        }

                        if (string.Compare(clientComputerName, sessionContextCurrent.ClientComputerName, StringComparison.Ordinal) == 0)
                        {
                            NrpcNetrServerAuthenticate2Response nrpcNetrServerAuthenticate2Response =
                                CreateNetrServerAuthenticate2Response(sessionContextCurrent,
                                          (uint)sessionContextCurrent.NegotiateFlags);
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerAuthenticate2Response);

                            return new NrpcServerSecurityContext(clientSessionInfo[clientComputerName]);
                        }
                        else
                        {
                            NrpcNetrServerAuthenticate2Response nrpcNetrServerAuthenticate2Response =
                                CreateNetrServerAuthenticate2Response(sessionContextCurrent, 
                                (uint)sessionContextCurrent.NegotiateFlags);
                            nrpcNetrServerAuthenticate2Response.Status = NtStatus.STATUS_ACCESS_DENIED;
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerAuthenticate2Response);
                            RemoveSessionContext(sessionContextCurrent);
                            sessionContextLast = null;
                            sessionContextCurrent = null;
                        }
                        break;

                    case NrpcMethodOpnums.NetrServerAuthenticate3:
                        if (sessionContextCurrent != sessionContextLast)
                        {
                            throw new InvalidOperationException("NetrServerReqChallenge is not received.");
                        }

                        NrpcNetrServerAuthenticate3Request nrpcNetrServerAuthenticate3Request =
                            request as NrpcNetrServerAuthenticate3Request;

                        if (nrpcNetrServerAuthenticate3Request == null)
                        {
                            throw new InvalidOperationException("request doesn't match its Opnum");
                        }

                        if (string.Compare(clientComputerName, sessionContextCurrent.ClientComputerName, StringComparison.Ordinal) == 0)
                        {
                            NrpcNetrServerAuthenticate3Response nrpcNetrServerAuthenticate3Response =
                                CreateNetrServerAuthenticate3Response(sessionContextCurrent,
                                          (uint)sessionContextCurrent.NegotiateFlags, 100);
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerAuthenticate3Response);

                            return new NrpcServerSecurityContext(clientSessionInfo[clientComputerName]);
                        }
                        else
                        {
                            NrpcNetrServerAuthenticate3Response nrpcNetrServerAuthenticate3Response =
                                CreateNetrServerAuthenticate3Response(sessionContextCurrent,
                                (uint)sessionContextCurrent.NegotiateFlags, 100);
                            nrpcNetrServerAuthenticate3Response.Status = NtStatus.STATUS_ACCESS_DENIED;
                            SendRpcCallResponse(sessionContextCurrent, nrpcNetrServerAuthenticate3Response);
                            RemoveSessionContext(sessionContextCurrent);
                            sessionContextLast = null;
                            sessionContextCurrent = null;
                        }
                        break;

                    default:
                        break;
                }
            }

            throw new TimeoutException();
        }


        /// <summary>
        ///  Remove a session context from the context manager
        /// </summary>
        /// <param name="sessionContext">The session context to remove</param>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        public void RemoveSessionContext(NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);
            contextManager.RemoveSessionContext(sessionContext.RpceLayerSessionContext);
        }


        /// <summary>
        ///  Enumerates all client sessions established
        /// </summary>
        public NrpcClientSessionInfo[] EnumClientSessionInfo()
        {
            NrpcClientSessionInfo[] sessionInfo = new NrpcClientSessionInfo[clientSessionInfo.Count];
            clientSessionInfo.Values.CopyTo(sessionInfo, 0);
            return sessionInfo;
        }


        #region Static Methods to establish secure channels with clients
        /// <summary>
        /// Tells the NRPC server SDK to listen for the requests from clients to establish secure channel.
        /// </summary>
        /// <param name="machineNameToPasswordDictionary">A dictionary whose key is the machine name and the
        /// value is the password</param>
        /// <param name="tcpServerPort">The TCP Server Port of the Netlogon Server</param>
        static public void ListenForSecureChannel(Dictionary<string, string> machineNameToPasswordDictionary,
            ushort tcpServerPort)
        {
            if (secureChannelThread != null)
            {
                return;
            }

            secureChannelThread = new Thread(new ParameterizedThreadStart(
                SecureChannelThread.EstablishSecureChannel));
            SecureChannelThread.IsStopped = false;
            SecureChannelThread.TcpPort = tcpServerPort;

            secureChannelThread.Start(machineNameToPasswordDictionary);
        }


        /// <summary>
        ///  Retrieves the secure channels established with the client
        /// </summary>
        /// <returns>All secure channels established</returns>
        static public NrpcServerSecurityContext[] RetrieveSecureChannel()
        {
            if (secureChannelThread == null)
            {
                return null;
            }

            List<NrpcServerSecurityContext> contextList = new List<NrpcServerSecurityContext>();
            foreach (NrpcClientSessionInfo sessionInfo in
                SecureChannelThread.serverForSecureChannel.clientSessionInfo.Values)
            {
                contextList.Add(new NrpcServerSecurityContext(sessionInfo));
            }
            return contextList.ToArray();
        }


        /// <summary>
        ///  Retrieves the secure channel established with the client
        /// </summary>
        /// <param name="clientMachineName">The name of the client machine, with which the secure channel
        /// is established</param>
        /// <param name="timeout">Maximal time waiting for the establishment of secure channel</param>
        /// <returns>A NRPC server security context representing the secure channel established</returns>
        static public NrpcServerSecurityContext RetrieveSecureChannel(string clientMachineName, TimeSpan timeout)
        {
            const int waitingInterval = 200;
            if (secureChannelThread == null)
            {
                return null;
            }

            DateTime startTime = DateTime.Now;
            while (DateTime.Now - startTime < timeout)
            {
                if (SecureChannelThread.serverForSecureChannel.clientSessionInfo.ContainsKey(clientMachineName))
                {
                    return new NrpcServerSecurityContext(
                        SecureChannelThread.serverForSecureChannel.clientSessionInfo[clientMachineName]);
                }
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, waitingInterval));
            }
            throw new TimeoutException();
        }


        /// <summary>
        ///  Releases all the secure channels established with the client, and stops the NRPC server if started
        /// </summary>
        static public void StopAndReleaseAllSecureChannelEstablished()
        {
            if (secureChannelThread == null || SecureChannelThread.IsStopped)
            {
                return;
            }

            SecureChannelThread.IsStopped = true;
            secureChannelThread.Join();
            secureChannelThread = null;
        }
        #endregion


        #region Methods to create NRPC responses

        /// <summary>
        ///  Creates the response of NRPC method NrpcNetrLogonUasLogon.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="status">NTStatus returned</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonUasLogonResponse CreateNetrLogonUasLogonResponse(NrpcServerSessionContext sessionContext,
            NetApiStatus status)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonUasLogonResponse messageToSend = new NrpcNetrLogonUasLogonResponse();
            messageToSend.Status = status;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NrpcNetrLogonUasLogoff
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="status">NTStatus returned</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonUasLogoffResponse CreateNetrLogonUasLogoffResponse(NrpcServerSessionContext sessionContext, 
            NetApiStatus status)
        {
            CheckIfSessionContextIsNull(sessionContext);

            // it is beyond scope of TD
            _NETLOGON_LOGOFF_UAS_INFO logoffInformation = new _NETLOGON_LOGOFF_UAS_INFO();
            logoffInformation.Duration = 0;
            logoffInformation.LogonCount = 0;

            NrpcNetrLogonUasLogoffResponse messageToSend = new NrpcNetrLogonUasLogoffResponse();
            messageToSend.Status = status;
            messageToSend.LogoffInformation = logoffInformation;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonSamLogon. ReturnAuthenticator field will be generated
        ///  automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="validationInformation">Validation information, this method will encrypt UserSessionKey
        /// and ExpansionRoom fields for user if necessary. </param>
        /// <param name="authoritative">Authoritative parameter</param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonSamLogonResponse CreateNetrLogonSamLogonResponse(NrpcServerSessionContext sessionContext,
            _NETLOGON_VALIDATION validationInformation,byte? authoritative)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonSamLogonResponse messageToSend = new NrpcNetrLogonSamLogonResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ValidationInformation = validationInformation;
            messageToSend.Authoritative = authoritative;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonSamLogoff. ReturnAuthenticator field will be generated
        ///  automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonSamLogoffResponse CreateNetrLogonSamLogoffResponse(
            NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonSamLogoffResponse messageToSend = new NrpcNetrLogonSamLogoffResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerReqChallenge. ReturnAuthenticator field will be generated
        ///  automatically.
        ///  After users call this method, session key will be generated and put into the session context
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="serverNonce">nonce of server</param>
        /// <param name="sharedSecret">The secret shared between the client and server</param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext, serverNonce or
        /// shareedSecret is null</exception>
        public NrpcNetrServerReqChallengeResponse CreateNetrServerReqChallengeResponse(
            NrpcServerSessionContext sessionContext, byte[] serverNonce, string sharedSecret)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (serverNonce == null)
            {
                throw new ArgumentNullException("serverNonce");
            }

            if (sharedSecret == null)
            {
                throw new ArgumentNullException("sharedSecret");
            }

            NrpcNetrServerReqChallengeResponse messageToSend = new NrpcNetrServerReqChallengeResponse();
            _NETLOGON_CREDENTIAL serverChallenge = new _NETLOGON_CREDENTIAL();
            serverChallenge.data = serverNonce;

            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ServerChallenge = serverChallenge;

            //session key will be calculated when this response is sent.
            sessionContext.SharedSecret = sharedSecret;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerAuthenticate. ReturnAuthenticator field will be generated
        ///  automatically. Server credential will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrServerAuthenticateResponse CreateNetrServerAuthenticateResponse(
            NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrServerAuthenticateResponse messageToSend = new NrpcNetrServerAuthenticateResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            _NETLOGON_CREDENTIAL serverCredential = new _NETLOGON_CREDENTIAL();
            serverCredential.data = NrpcUtility.ComputeNetlogonCredential(sessionContext.SessionKey,
                (uint)sessionContext.NegotiateFlags, sessionContext.ServerChallenge);
            messageToSend.ServerCredential = serverCredential;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerPasswordSet. ReturnAuthenticator field will be generated
        ///  automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrServerPasswordSetResponse CreateNetrServerPasswordSetResponse(
            NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrServerPasswordSetResponse messageToSend = new NrpcNetrServerPasswordSetResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrDatabaseDeltas. ReturnAuthenticator field will be generated
        ///  automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domainModifiedCount">
        /// A pointer to an NLPR_MODIFIED_COUNT structure, as specified in section 
        /// 2.2.1.5.26, that contains the database serial number. On input, this is 
        /// the value of the database serial number on the client. On output, this is 
        /// the value of the database serial number corresponding to the last element 
        /// (delta) returned in the DeltaArray parameter.
        /// </param>
        /// <param name="deltaArray">
        /// A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure that contains an array 
        /// of enumerated changes (deltas) to the specified database with database 
        /// serial numbers larger than the database serial number value specified in 
        /// the input value of the DomainModifiedCount parameter.
        /// </param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrDatabaseDeltasResponse CreateNetrDatabaseDeltasResponse(NrpcServerSessionContext sessionContext, 
            _NLPR_MODIFIED_COUNT? domainModifiedCount, _NETLOGON_DELTA_ENUM_ARRAY? deltaArray)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrDatabaseDeltasResponse messageToSend = new NrpcNetrDatabaseDeltasResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.DomainModifiedCount = domainModifiedCount;
            messageToSend.DeltaArray = deltaArray;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrDatabaseSync. ReturnAuthenticator field will be generated
        ///  automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="syncContext">
        ///  Specifies context needed to continue the operation.
        ///  The value MUST be set to zero on the first call. The
        ///  caller MUST treat this as an opaque value, unless this
        ///  call is a restart of the series of synchronization
        ///  calls. The value returned is to be used on input for
        ///  the next call in the series of synchronization calls. If
        ///  this call is the restart of the series, the values
        ///  of the RestartState and the SyncContext parameters
        ///  are dependent on the DeltaType value received on the
        ///  last call before the restart and MUST be set as follows.
        ///  Find the last NETLOGON_DELTA_ENUM structure in the
        ///  DeltaArray parameter of the call. The DeltaType field
        ///  of this NETLOGON_DELTA_ENUM structure, as specified
        ///  in section , is the DeltaType needed for the restart.
        ///  The values of RestartState and SyncContext are then
        ///  determined from the following table. DeltaTypeRestartStateSyncContextAddOrChangeGroupGroupStateThe
        ///  value of the RID field of the last element AddOrChangeUserUserStateThe
        ///  value of the RID field of the last element ChangeGroupMembershipGroupMemberStateThe
        ///  value of the RID field of the last element 
        ///  AddOrChangeAliasAliasState0x00000000ChangeAliasMembershipAliasMemberState0x00000000Any
        ///  other value not previously listedNormalState0x00000000
        /// </param>
        /// <param name="deltaArray">
        /// A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrDatabaseSyncResponse CreateNetrDatabaseSyncResponse(NrpcServerSessionContext sessionContext,
            uint? syncContext, _NETLOGON_DELTA_ENUM_ARRAY? deltaArray)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrDatabaseSyncResponse messageToSend = new NrpcNetrDatabaseSyncResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.SyncContext = syncContext;
            messageToSend.DeltaArray = deltaArray;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrAccountDeltas
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrAccountDeltasResponse CreateNetrAccountDeltasResponse(NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrAccountDeltasRequest received = sessionContext.LastRequestReceived as NrpcNetrAccountDeltasRequest;

            if (received == null)
            {
                throw new InvalidOperationException("Last received request is not NrpcNetrAccountDeltasRequest");
            }

            NrpcNetrAccountDeltasResponse messageToSend = new NrpcNetrAccountDeltasResponse();
            messageToSend.Status = NtStatus.STATUS_NOT_IMPLEMENTED;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);
            messageToSend.Buffer = new byte[received.BufferSize];
            messageToSend.CountReturned = 0;
            messageToSend.TotalEntries = 0;
            _UAS_INFO_0 nextRecordId = new _UAS_INFO_0();
            nextRecordId.ComputerName = new byte[COMPUTER_NAME_LENGTH]; 
            messageToSend.NextRecordId = nextRecordId;

            return messageToSend;
        } 


        /// <summary>
        ///  Creates the response of NRPC method NetrAccountSync
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrAccountSyncResponse CreateNetrAccountSyncResponse(NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrAccountSyncRequest received = sessionContext.LastRequestReceived as NrpcNetrAccountSyncRequest;

            if (received == null)
            {
                throw new InvalidOperationException("Last received request is not NrpcNetrAccountSyncRequest");
            }

            NrpcNetrAccountSyncResponse messageToSend = new NrpcNetrAccountSyncResponse();
            messageToSend.Status = NtStatus.STATUS_NOT_IMPLEMENTED;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);
            messageToSend.Buffer = new byte[received.BufferSize];
            messageToSend.CountReturned = 0;
            messageToSend.TotalEntries = 0;
            _UAS_INFO_0 lastRecordId = new _UAS_INFO_0();
            lastRecordId.ComputerName = new byte[COMPUTER_NAME_LENGTH]; 
            messageToSend.LastRecordId = lastRecordId;
            messageToSend.NextReference = 0;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrGetDCName
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="pDCNetbiosName">Netbios name of PDC</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrGetDCNameResponse CreateNetrGetDCNameResponse(NrpcServerSessionContext sessionContext,
            string pDCNetbiosName)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrGetDCNameResponse messageToSend = new NrpcNetrGetDCNameResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Buffer = pDCNetbiosName;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonControl
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="buffer">A structure contains the specific query results,
        /// with a level of verbosity as specified in QueryLevel</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonControlResponse CreateNetrLogonControlResponse(NrpcServerSessionContext sessionContext,
            _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonControlResponse messageToSend = new NrpcNetrLogonControlResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Buffer = buffer;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrGetAnyDCName
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="netBiosNameofDC">The null-terminated Unicode string containing the NetBIOS name
        /// of a DC in the specified domain</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrGetAnyDCNameResponse CreateNetrGetAnyDCNameResponse(NrpcServerSessionContext sessionContext,
            string netBiosNameofDC)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrGetAnyDCNameResponse messageToSend = new NrpcNetrGetAnyDCNameResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Buffer = netBiosNameofDC;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonControl2
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="buffer">A structure contains the specific query results,
        /// with a level of verbosity as specified in QueryLevel</param>
        /// <returns>The created response to be sent to client</returns>
        public NrpcNetrLogonControl2Response CreateNetrLogonControl2Response(NrpcServerSessionContext sessionContext,
            _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonControl2Response messageToSend = new NrpcNetrLogonControl2Response();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Buffer = buffer;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerAuthenticate2. ServerCredential will be
        ///  generated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="negotiateFlags">A pointer to a 32-bit set of bit flags in little-endian
        /// format that indicate features supported. As input, the set of flags are those requested
        /// by the client and SHOULD be the same as ClientCapabilities. As output, they are the bit-wise
        /// AND of the client's requested capabilities and the server's ServerCapabilities</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrServerAuthenticate2Response CreateNetrServerAuthenticate2Response(
            NrpcServerSessionContext sessionContext, uint negotiateFlags)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrServerAuthenticate2Response messageToSend = new NrpcNetrServerAuthenticate2Response();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.NegotiateFlags = negotiateFlags;
            _NETLOGON_CREDENTIAL serverCredential = new _NETLOGON_CREDENTIAL();
            serverCredential.data = NrpcUtility.ComputeNetlogonCredential(
                sessionContext.SessionKey, negotiateFlags, sessionContext.ServerChallenge);
            messageToSend.ServerCredential = serverCredential; 

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrDatabaseSync2. ReturnAuthenticator will be
        ///  generated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        ///  <param name="syncContext">Specifies context needed to continue operation</param>
        /// <param name="deltaArray">
        /// A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrDatabaseSync2Response CreateNetrDatabaseSync2Response(NrpcServerSessionContext sessionContext,
            uint? syncContext, _NETLOGON_DELTA_ENUM_ARRAY? deltaArray)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrDatabaseSync2Response messageToSend = new NrpcNetrDatabaseSync2Response();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.DeltaArray = deltaArray;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);
            messageToSend.SyncContext = syncContext;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrDatabaseRedo. ReturnAuthenticator will be
        ///  generated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="deltaArray">
        /// A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrDatabaseRedoResponse CreateNetrDatabaseRedoResponse(NrpcServerSessionContext sessionContext,
            _NETLOGON_DELTA_ENUM_ARRAY? deltaArray)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrDatabaseRedoResponse messageToSend = new NrpcNetrDatabaseRedoResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.DeltaArray = deltaArray;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonControl2Ex
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="buffer">A structure contains the specific query results,
        /// with a level of verbosity as specified in QueryLevel</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonControl2ExResponse CreateNetrLogonControl2ExResponse(
            NrpcServerSessionContext sessionContext, _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonControl2ExResponse messageToSend = new NrpcNetrLogonControl2ExResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Buffer = buffer;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrEnumerateTrustedDomains
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="trustedDomainNames">A list of trusted domain names</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when trustedDomainNames is null</exception>
        public NrpcNetrEnumerateTrustedDomainsResponse CreateNetrEnumerateTrustedDomainsResponse(
            NrpcServerSessionContext sessionContext, string[] trustedDomainNames)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (trustedDomainNames == null)
            {
                throw new ArgumentNullException("trustedDomainNames");
            }

            NrpcNetrEnumerateTrustedDomainsResponse messageToSend = new NrpcNetrEnumerateTrustedDomainsResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            _DOMAIN_NAME_BUFFER domainNameBuffer = new _DOMAIN_NAME_BUFFER();

            int trustedDomainCount = trustedDomainNames.Length;
            byte[][] domainNamesArray = new byte[trustedDomainCount][];
            // null of utf-16 is 0x0000 as TD 2.2.1.5.1
            byte[] utf16Null = new byte[] { 0x00, 0x00 }; 

            for (int i = 0; i < trustedDomainCount;i++ )
            {
                domainNamesArray[i] = 
                    ArrayUtility.ConcatenateArrays<byte>(Encoding.Unicode.GetBytes(trustedDomainNames[i]), utf16Null);
            }

            domainNameBuffer.DomainNames = ArrayUtility.ConcatenateArrays<byte>(domainNamesArray);
            domainNameBuffer.DomainNames = 
                ArrayUtility.ConcatenateArrays<byte>(domainNameBuffer.DomainNames, utf16Null);
            domainNameBuffer.DomainNameByteCount = (uint)domainNameBuffer.DomainNames.Length;

            messageToSend.DomainNameBuffer = domainNameBuffer;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrGetDcName
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domainControllerInfo">Information of the domain controller</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrGetDcNameResponse CreateDsrGetDcNameResponse(NrpcServerSessionContext sessionContext,
            _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrGetDcNameResponse messageToSend = new NrpcDsrGetDcNameResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.DomainControllerInfo = domainControllerInfo;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonGetCapabilities. ReturnAuthenticator will be
        ///  generated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="serverCapabilities">
        /// A pointer to a 32-bit set of bit flags that identify the server's capabilities.
        /// </param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonGetCapabilitiesResponse CreateNetrLogonGetCapabilitiesResponse(
            NrpcServerSessionContext sessionContext, _NETLOGON_CAPABILITIES? serverCapabilities)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonGetCapabilitiesResponse messageToSend = new NrpcNetrLogonGetCapabilitiesResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ServerCapabilities = serverCapabilities;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonSetServiceBits. 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonSetServiceBitsResponse CreateNetrLogonSetServiceBitsResponse(
            NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonSetServiceBitsResponse messageToSend = new NrpcNetrLogonSetServiceBitsResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonGetTrustRid. 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="rid">
        /// A pointer to an unsigned long that receives the RID of the account.
        /// </param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonGetTrustRidResponse CreateNetrLogonGetTrustRidResponse(
            NrpcServerSessionContext sessionContext, uint? rid)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonGetTrustRidResponse messageToSend = new NrpcNetrLogonGetTrustRidResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Rid = rid;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonComputeServerDigest. This method will calculate
        ///  the digest for users 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="newPassword">The new password of the account</param>
        /// <param name="oldPassword">The old password of the account</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when newPassword is null</exception>
        public NrpcNetrLogonComputeServerDigestResponse CreateNetrLogonComputeServerDigestResponse(
            NrpcServerSessionContext sessionContext, string newPassword, string oldPassword)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (newPassword == null)
            {
                throw new ArgumentNullException("newPassword");
            }

            NrpcNetrLogonComputeServerDigestRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonComputeServerDigestRequest;

            if (request == null)
            {
                throw new InvalidOperationException("Last request is not NrpcNetrLogonComputeServerDigestRequest");
            }

            byte[] messageForDigest = request.Message;

            if (oldPassword == null)
            {
                oldPassword = newPassword;
            }

            NrpcNetrLogonComputeServerDigestResponse messageToSend = new NrpcNetrLogonComputeServerDigestResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.NewMessageDigest = ComputeMessageDigest(newPassword, messageForDigest);
            messageToSend.OldMessageDigest = ComputeMessageDigest(oldPassword, messageForDigest);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonComputeClientDigest. 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="newPassword">The new password of the account</param>
        /// <param name="oldPassword">The old password of the account</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when newPassword is null</exception>
        public NrpcNetrLogonComputeClientDigestResponse CreateNetrLogonComputeClientDigestResponse(
            NrpcServerSessionContext sessionContext, string newPassword, string oldPassword)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (newPassword == null)
            {
                throw new ArgumentNullException("newPassword");
            }

            NrpcNetrLogonComputeClientDigestRequest request = 
                sessionContext.LastRequestReceived as NrpcNetrLogonComputeClientDigestRequest;

            if (request == null)
            {
                throw new InvalidOperationException("Last request is not NrpcNetrLogonComputeServerDigestRequest");
            }

            byte[] messageForDigest = request.Message;

            if (oldPassword == null)
            {
                oldPassword = newPassword;
            }

            NrpcNetrLogonComputeClientDigestResponse messageToSend = new NrpcNetrLogonComputeClientDigestResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.NewMessageDigest = ComputeMessageDigest(newPassword, messageForDigest);
            messageToSend.OldMessageDigest = ComputeMessageDigest(oldPassword, messageForDigest);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerAuthenticate3. ServerCredential will be
        ///  generated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="negotiateFlags">A pointer to a 32-bit set of bit flags in little-endian
        /// format that indicate features supported. As input, the set of flags are those requested
        /// by the client and SHOULD be the same as ClientCapabilities. As output, they are the bit-wise
        /// AND of the client's requested capabilities and the server's ServerCapabilities</param>
        /// <param name="accountRid">A pointer that receives the RID of the account specified by 
        /// the AccountName parameter.</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrServerAuthenticate3Response CreateNetrServerAuthenticate3Response(
            NrpcServerSessionContext sessionContext, uint negotiateFlags, uint? accountRid)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrServerAuthenticate3Response messageToSend = new NrpcNetrServerAuthenticate3Response();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.NegotiateFlags = negotiateFlags;
            messageToSend.AccountRid = accountRid;
            _NETLOGON_CREDENTIAL serverCredential = new _NETLOGON_CREDENTIAL();
            serverCredential.data = NrpcUtility.ComputeNetlogonCredential(
                sessionContext.SessionKey, negotiateFlags, sessionContext.ServerChallenge);
            messageToSend.ServerCredential = serverCredential;

            return messageToSend;
        }

        /// <summary>
        ///  Creates the response of NRPC method DsrGetDcNameEx
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domainControllerInfo">Information of the domain controller</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrGetDcNameExResponse CreateDsrGetDcNameExResponse(NrpcServerSessionContext sessionContext,
            _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrGetDcNameExResponse messageToSend = new NrpcDsrGetDcNameExResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.DomainControllerInfo = domainControllerInfo;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrGetSiteName
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="siteName">A null-terminated Unicode string that contains the name of the
        /// site in which the computer that receives this call resides</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrGetSiteNameResponse CreateDsrGetSiteNameResponse(NrpcServerSessionContext sessionContext,
            string siteName)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrGetSiteNameResponse messageToSend = new NrpcDsrGetSiteNameResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.SiteName = siteName;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonGetDomainInfo. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domBuffer">A pointer to a NETLOGON_DOMAIN_INFORMATION structure,
        /// as specified in section 2.2.1.3.12,that contains information about the domain or
        /// policy information</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonGetDomainInfoResponse CreateNetrLogonGetDomainInfoResponse(
            NrpcServerSessionContext sessionContext, _NETLOGON_DOMAIN_INFORMATION? domBuffer)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonGetDomainInfoResponse messageToSend = new NrpcNetrLogonGetDomainInfoResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.DomBuffer = domBuffer;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerPasswordSet2. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrServerPasswordSet2Response CreateNetrServerPasswordSet2Response(
            NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrServerPasswordSet2Response messageToSend = new NrpcNetrServerPasswordSet2Response();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerPasswordGet. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="machineAccountPassword">a machine account password retrieved</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when machineAccountPassword is null</exception>
        public NrpcNetrServerPasswordGetResponse CreateNetrServerPasswordGetResponse(
            NrpcServerSessionContext sessionContext, string machineAccountPassword)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (machineAccountPassword == null)
            {
                throw new ArgumentNullException("machineAccountPassword");
            }

            NrpcNetrServerPasswordGetResponse messageToSend = new NrpcNetrServerPasswordGetResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);
            messageToSend.EncryptedNtOwfPassword = 
                NrpcUtility.ComputeEncryptedNtOwfv1Password(machineAccountPassword, sessionContext.SessionKey);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonSendToSam. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonSendToSamResponse CreateNetrLogonSendToSamResponse(
            NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonSendToSamResponse messageToSend = new NrpcNetrLogonSendToSamResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrAddressToSiteNamesW.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="siteNames">An array of site names</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when siteNames is null</exception>
        public NrpcDsrAddressToSiteNamesWResponse CreateDsrAddressToSiteNamesWResponse(
            NrpcServerSessionContext sessionContext, string[] siteNames)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (siteNames == null)
            {
                throw new ArgumentNullException("siteNames");
            }

            _NL_SITE_NAME_ARRAY nlSiteNameArray = new _NL_SITE_NAME_ARRAY();
            nlSiteNameArray.EntryCount = (uint)siteNames.Length;
            nlSiteNameArray.SiteNames = new _RPC_UNICODE_STRING[nlSiteNameArray.EntryCount];

            for (int i = 0; i < nlSiteNameArray.EntryCount; i++)
            {
                nlSiteNameArray.SiteNames[i] = DtypUtility.ToRpcUnicodeString(siteNames[i]);
            }

            NrpcDsrAddressToSiteNamesWResponse messageToSend = new NrpcDsrAddressToSiteNamesWResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.SiteNames = nlSiteNameArray;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrGetDcNameEx2
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domainControllerInfo">Information of the domain controller</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrGetDcNameEx2Response CreateDsrGetDcNameEx2Response(NrpcServerSessionContext sessionContext,
            _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrGetDcNameEx2Response messageToSend = new NrpcDsrGetDcNameEx2Response();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.DomainControllerInfo = domainControllerInfo;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonGetTimeServiceParentDomain
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domainName">A pointer to the buffer that receives the null-terminated
        /// Unicode string that contains the name of the parent domain</param>
        /// <param name="pdcSameSite">A pointer to the buffer that receives the value that indicates 
        /// whether the PDC for the domain DomainName is in the same site as the server specified by ServerName
        /// </param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonGetTimeServiceParentDomainResponse CreateNetrLogonGetTimeServiceParentDomainResponse(
            NrpcServerSessionContext sessionContext, string domainName, PdcSameSite_Values? pdcSameSite)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonGetTimeServiceParentDomainResponse messageToSend =
                new NrpcNetrLogonGetTimeServiceParentDomainResponse();

            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.DomainName = domainName;
            messageToSend.PdcSameSite = pdcSameSite;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrEnumerateTrustedDomainsEx
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domains">An _NETLOGON_TRUSTED_DOMAIN_ARRAY object contains trusted domains info</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrEnumerateTrustedDomainsExResponse CreateNetrEnumerateTrustedDomainsExResponse(
            NrpcServerSessionContext sessionContext, _NETLOGON_TRUSTED_DOMAIN_ARRAY? domains)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrEnumerateTrustedDomainsExResponse messageToSend = new NrpcNetrEnumerateTrustedDomainsExResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Domains = domains;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrAddressToSiteNamesExW
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="siteNames">An array of site names.</param>
        /// <param name="subSiteNames">An array of subsite names.</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when siteNames is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when subSiteNames is null</exception>
        /// <exception cref="ArgumentException">Thrown when siteNames length and subSiteNames length 
        /// are not equal</exception>
        public NrpcDsrAddressToSiteNamesExWResponse CreateDsrAddressToSiteNamesExWResponse(
            NrpcServerSessionContext sessionContext, string[] siteNames, string[] subSiteNames)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (siteNames == null)
            {
                throw new ArgumentNullException("siteNames");
            }

            if (subSiteNames == null)
            {
                throw new ArgumentNullException("subSiteNames");
            }

            if (siteNames.Length != subSiteNames.Length)
            {
                throw new ArgumentException("siteNames length and subSiteNames length are not equal");
            }

            _NL_SITE_NAME_EX_ARRAY nlSiteNameExArray = new _NL_SITE_NAME_EX_ARRAY();
            nlSiteNameExArray.EntryCount = (uint)siteNames.Length;
            nlSiteNameExArray.SiteNames = new _RPC_UNICODE_STRING[nlSiteNameExArray.EntryCount];
            nlSiteNameExArray.SubnetNames = new _RPC_UNICODE_STRING[nlSiteNameExArray.EntryCount];

            for (int i = 0; i < nlSiteNameExArray.EntryCount; i++)
            {
                nlSiteNameExArray.SiteNames[i] = DtypUtility.ToRpcUnicodeString(siteNames[i]);
                nlSiteNameExArray.SubnetNames[i] = DtypUtility.ToRpcUnicodeString(subSiteNames[i]);
            }

            NrpcDsrAddressToSiteNamesExWResponse messageToSend = new NrpcDsrAddressToSiteNamesExWResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.SiteNames = nlSiteNameExArray;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrGetDcSiteCoverageW
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="siteNames">An array of site names</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when siteNames is null</exception>
        public NrpcDsrGetDcSiteCoverageWResponse CreateDsrGetDcSiteCoverageWResponse(
            NrpcServerSessionContext sessionContext, string[] siteNames)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (siteNames == null)
            {
                throw new ArgumentNullException("siteNames");
            }

            _NL_SITE_NAME_ARRAY nlSiteNameArray = new _NL_SITE_NAME_ARRAY();
            nlSiteNameArray.EntryCount = (uint)siteNames.Length;
            nlSiteNameArray.SiteNames = new _RPC_UNICODE_STRING[nlSiteNameArray.EntryCount];

            for (int i = 0; i < nlSiteNameArray.EntryCount; i++)
            {
                nlSiteNameArray.SiteNames[i] = DtypUtility.ToRpcUnicodeString(siteNames[i]);
            }

            NrpcDsrGetDcSiteCoverageWResponse messageToSend = new NrpcDsrGetDcSiteCoverageWResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.SiteNames = nlSiteNameArray;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonSamLogon. 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="validationInformation">Validation information, this method will encrypt UserSessionKey
        /// and ExpansionRoom fields for user if necessary. </param>
        /// <param name="authoritative">Authoritative parameter</param>
        /// <param name="ExtraFlags">A pointer to a set of bit flags that specify delivery settings.</param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonSamLogonExResponse CreateNetrLogonSamLogonExResponse(
            NrpcServerSessionContext sessionContext, _NETLOGON_VALIDATION validationInformation,
            byte? authoritative, uint? ExtraFlags)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonSamLogonExResponse messageToSend = new NrpcNetrLogonSamLogonExResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ValidationInformation = validationInformation;
            messageToSend.Authoritative = authoritative;
            messageToSend.ExtraFlags = ExtraFlags;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrEnumerateDomainTrusts
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="domains">An array of domains</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrEnumerateDomainTrustsResponse CreateDsrEnumerateDomainTrustsResponse(
            NrpcServerSessionContext sessionContext, _NETLOGON_TRUSTED_DOMAIN_ARRAY? domains)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrEnumerateDomainTrustsResponse messageToSend = new NrpcDsrEnumerateDomainTrustsResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.Domains = domains;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrDeregisterDnsHostRecords
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrDeregisterDnsHostRecordsResponse CreateDsrDeregisterDnsHostRecordsResponse(
            NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrDeregisterDnsHostRecordsResponse messageToSend = new NrpcDsrDeregisterDnsHostRecordsResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerTrustPasswordsGet. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="newPassword">Current password</param>
        /// <param name="oldPassword">Old password</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when newPassword is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when oldPassword is null</exception>
        public NrpcNetrServerTrustPasswordsGetResponse CreateNetrServerTrustPasswordsGetResponse(
            NrpcServerSessionContext sessionContext, string newPassword, string oldPassword)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (newPassword == null)
            {
                throw new ArgumentNullException("newPassword");
            }

            if (oldPassword == null)
            {
                throw new ArgumentNullException("oldPassword");
            }

            NrpcNetrServerTrustPasswordsGetResponse messageToSend = new NrpcNetrServerTrustPasswordsGetResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);
            messageToSend.EncryptedNewOwfPassword = 
                NrpcUtility.ComputeEncryptedNtOwfv1Password(newPassword, sessionContext.SessionKey);
            messageToSend.EncryptedOldOwfPassword = 
                NrpcUtility.ComputeEncryptedNtOwfv1Password(oldPassword, sessionContext.SessionKey);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrGetForestTrustInformation. 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="forestTrustInfo">A pointer to an LSA_FOREST_TRUST_INFORMATION structure, as specified
        /// in [MS-LSAD] section 2.2.7.25, that contains data for each forest trust</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrGetForestTrustInformationResponse CreateDsrGetForestTrustInformationResponse(
            NrpcServerSessionContext sessionContext, _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrGetForestTrustInformationResponse messageToSend = new NrpcDsrGetForestTrustInformationResponse();
            messageToSend.Status = NetApiStatus.NERR_Success;
            messageToSend.ForestTrustInfo = forestTrustInfo;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrGetForestTrustInformation. 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="forestTrustInfo">A pointer to an LSA_FOREST_TRUST_INFORMATION structure, as specified
        /// in [MS-LSAD] section 2.2.7.25, that contains data for each forest trust</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrGetForestTrustInformationResponse CreateNetrGetForestTrustInformationResponse(
            NrpcServerSessionContext sessionContext, _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrGetForestTrustInformationResponse messageToSend = new NrpcNetrGetForestTrustInformationResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ForestTrustInfo = forestTrustInfo;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrLogonSamLogonWithFlags. 
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="validationInformation">Validation information, this method will encrypt UserSessionKey
        /// and ExpansionRoom fields for user if necessary. </param>
        /// <param name="authoritative">Authoritative parameter</param>
        /// <param name="ExtraFlags">A pointer to a set of bit flags that specify delivery settings.</param>
        /// <returns>The created response to be sent to the client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrLogonSamLogonWithFlagsResponse CreateNetrLogonSamLogonWithFlagsResponse(
            NrpcServerSessionContext sessionContext, _NETLOGON_VALIDATION validationInformation,
            byte? authoritative, uint? ExtraFlags)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrLogonSamLogonWithFlagsResponse messageToSend = new NrpcNetrLogonSamLogonWithFlagsResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ValidationInformation = validationInformation;
            messageToSend.Authoritative = authoritative;
            messageToSend.ExtraFlags = ExtraFlags;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrServerGetTrustInfo. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="newPassword">Current password</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="trustInfo">A pointer to an NL_GENERIC_RPC_DATA structure, as specified
        /// in section 2.2.1.6.4, that contains a block of generic RPC data with trust information
        /// for the specified server.</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when newPassword is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when oldPassword is null</exception>
        public NrpcNetrServerGetTrustInfoResponse CreateNetrServerGetTrustInfoResponse(
            NrpcServerSessionContext sessionContext, 
            string newPassword, 
            string oldPassword,
            _NL_GENERIC_RPC_DATA? trustInfo)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (newPassword == null)
            {
                throw new ArgumentNullException("newPassword");
            }

            if (oldPassword == null)
            {
                throw new ArgumentNullException("oldPassword");
            }

            NrpcNetrServerGetTrustInfoResponse messageToSend = new NrpcNetrServerGetTrustInfoResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);
            messageToSend.EncryptedNewOwfPassword = 
                NrpcUtility.ComputeEncryptedNtOwfv1Password(newPassword, sessionContext.SessionKey);
            messageToSend.EncryptedOldOwfPassword = 
                NrpcUtility.ComputeEncryptedNtOwfv1Password(oldPassword, sessionContext.SessionKey);
            messageToSend.TrustInfo = trustInfo;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method OpnumUnused47.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcOpnumUnused47Response CreateOpnumUnused47Response(NrpcServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcOpnumUnused47Response messageToSend = new NrpcOpnumUnused47Response();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method DsrUpdateReadOnlyServerDnsRecords. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="dnsNames">An array of DNS names</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcDsrUpdateReadOnlyServerDnsRecordsResponse CreateDsrUpdateReadOnlyServerDnsRecordsResponse(
            NrpcServerSessionContext sessionContext, _NL_DNS_NAME_INFO_ARRAY? dnsNames)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcDsrUpdateReadOnlyServerDnsRecordsResponse messageToSend =
                new NrpcDsrUpdateReadOnlyServerDnsRecordsResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);
            messageToSend.DnsNames = dnsNames;

            return messageToSend;
        }


        /// <summary>
        ///  Creates the response of NRPC method NetrChainSetClientAttributes. ReturnAuthenticator
        ///  will be calculated automatically.
        /// </summary>
        /// <param name="sessionContext">Context of the RPC session</param>
        /// <param name="pmsgOut">A pointer to an NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure
        /// that contains information on the client workstation and the writable domain controller</param>
        /// <returns>The created response to be sent to client</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null</exception>
        public NrpcNetrChainSetClientAttributesResponse CreateNetrChainSetClientAttributesResponse(
            NrpcServerSessionContext sessionContext, NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgOut)
        {
            CheckIfSessionContextIsNull(sessionContext);

            NrpcNetrChainSetClientAttributesResponse messageToSend = new NrpcNetrChainSetClientAttributesResponse();
            messageToSend.Status = NtStatus.STATUS_SUCCESS;
            messageToSend.PdwOutVersion = 1;
            messageToSend.PmsgOut = pmsgOut;
            messageToSend.ReturnAuthenticator = ComputeAuthenticator(sessionContext);

            return messageToSend;
        }
        #endregion


        #region Methods to validate client inputs and generate outputs for users
        /// <summary>
        /// Encrypt LM_OWF_PASSWORD and NT_OWF_PASSWORD by RC4 and session key.
        /// </summary>
        /// <param name="sessionContext">Session Context</param>
        /// <param name="lmOwfPassword">LM_OWF_PASSWORD</param>
        /// <param name="ntOwfPassword">NT_OWF_PASSWORD</param>
        private static void EncryptLmAndNtOwfPassword(NrpcServerSessionContext sessionContext,
            ref _CYPHER_BLOCK[] lmOwfPassword,
            ref _CYPHER_BLOCK[] ntOwfPassword)
        {
            byte[] lmOwf = ArrayUtility.ConcatenateArrays(
                lmOwfPassword[0].data,
                lmOwfPassword[1].data);
            byte[] ntOwf = ArrayUtility.ConcatenateArrays(
                ntOwfPassword[0].data,
                ntOwfPassword[1].data);

            bool isAesNegotiated = ((sessionContext.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0);

            lmOwf = NrpcUtility.EncryptBuffer(isAesNegotiated, sessionContext.SessionKey, lmOwf);
            ntOwf = NrpcUtility.EncryptBuffer(isAesNegotiated, sessionContext.SessionKey, ntOwf);

            lmOwfPassword = NrpcUtility.CreateCypherBlocks(lmOwf);
            ntOwfPassword = NrpcUtility.CreateCypherBlocks(ntOwf);
        }


        /// <summary>
        ///  Validates whether the LogonInformation.
        /// </summary>
        /// <param name="sessionContext">The session context of the RPC response to send</param>
        /// <param name="domainName">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password of the user</param>
        /// <param name="logonLevel">The logon level passed by the client</param>
        /// <param name="LogonInformation">The logon information passed by the client</param>
        /// <returns>True if LogonInformation is valid, otherwise, returns false</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if value of logonLevel is out of range</exception>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext, domainName, userName or
        /// password is NULL</exception>
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static bool ValidateLogonInformation(NrpcServerSessionContext sessionContext, string domainName,
            string userName, string password, _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? LogonInformation)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (domainName == null)
            {
                throw new ArgumentNullException("domainName");
            }

            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (LogonInformation == null)
            {
                return false;
            }

            switch (logonLevel)
            {
                case _NETLOGON_LOGON_INFO_CLASS.NetlogonGenericInformation:
                    //In this case, simply validate the syntax of LogonGeneric.
                    //Nrpc SDK doesn't know how to validate LogonData
                    return ValidateGenericLogonInformation(LogonInformation);

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation:
                    return ValidateInteractiveInformation(sessionContext, domainName, userName,
                        password, LogonInformation);

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation:
                    return ValidateInteractiveTransitiveInformation(sessionContext, domainName,
                        userName, password, LogonInformation);

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation:
                    return ValidateNetworkInformation(domainName, userName, password, LogonInformation);

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation:
                    return ValidateNetworkTransitiveInformation(domainName, userName, password, LogonInformation);

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation:
                    return ValidateServiceInformation(sessionContext, domainName, userName,
                        password, LogonInformation);

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation:
                    return ValidateServiceTransitiveInformation(sessionContext, domainName,
                        userName, password, LogonInformation);

                default:
                    throw new ArgumentOutOfRangeException("logonLevel");
            }
        }


        /// <summary>
        ///  Validates Service Transitive Information
        /// </summary>
        /// <param name="sessionContext">The session context of the RPC response to send</param>
        /// <param name="domainName">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password of the user</param>
        /// <param name="LogonInformation">Logon information received</param>
        /// <returns>Whether the LogonInformation is correct</returns>
        private static bool ValidateServiceTransitiveInformation(NrpcServerSessionContext sessionContext,
            string domainName, string userName, string password, _NETLOGON_LEVEL? LogonInformation)
        {
            _CYPHER_BLOCK[] lmOwfBlocks;
            _CYPHER_BLOCK[] ntOwfBlocks;

            if (LogonInformation.Value.LogonServiceTransitive == null ||
                LogonInformation.Value.LogonServiceTransitive.Length != 1)
            {
                return false;
            }

            if (DtypUtility.ToInt64(LogonInformation.Value.LogonServiceTransitive[0].Identity.Reserved) != 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(NrpcParameterControlFlags),
                LogonInformation.Value.LogonServiceTransitive[0].Identity.ParameterControl))
            {
                return false;
            }

            lmOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password));

            ntOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password));

            NrpcServer.EncryptLmAndNtOwfPassword(sessionContext, ref lmOwfBlocks, ref ntOwfBlocks);

            if (!CompareHashPassword(LogonInformation.Value.LogonServiceTransitive[0].LmOwfPassword.data,
                lmOwfBlocks))
            {
                return false;
            }

            if (!CompareHashPassword(LogonInformation.Value.LogonServiceTransitive[0].NtOwfPassword.data,
                ntOwfBlocks))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        ///  Validates Service Information
        /// </summary>
        /// <param name="sessionContext">The session context of the RPC response to send</param>
        /// <param name="domainName">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password of the user</param>
        /// <param name="LogonInformation">Logon information received</param>
        /// <returns>Whether the LogonInformation is correct</returns>
        private static bool ValidateServiceInformation(NrpcServerSessionContext sessionContext, string domainName,
            string userName, string password, _NETLOGON_LEVEL? LogonInformation)
        {
            _CYPHER_BLOCK[] lmOwfBlocks;
            _CYPHER_BLOCK[] ntOwfBlocks;

            if (LogonInformation.Value.LogonService == null ||
                LogonInformation.Value.LogonService.Length != 1)
            {
                return false;
            }

            if (DtypUtility.ToInt64(LogonInformation.Value.LogonService[0].Identity.Reserved) != 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(NrpcParameterControlFlags),
                LogonInformation.Value.LogonService[0].Identity.ParameterControl))
            {
                return false;
            }

            lmOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password));

            ntOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password));

            NrpcServer.EncryptLmAndNtOwfPassword(sessionContext, ref lmOwfBlocks, ref ntOwfBlocks);

            if (!CompareHashPassword(LogonInformation.Value.LogonService[0].LmOwfPassword.data, lmOwfBlocks))
            {
                return false;
            }

            if (!CompareHashPassword(LogonInformation.Value.LogonService[0].NtOwfPassword.data, ntOwfBlocks))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        ///  Validates Network Transitive Information
        /// </summary>
        /// <param name="domainName">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password of the user</param>
        /// <param name="LogonInformation">Logon information received</param>
        /// <returns>Whether the LogonInformation is correct</returns>
        private static bool ValidateNetworkTransitiveInformation(string domainName,
            string userName, string password, _NETLOGON_LEVEL? LogonInformation)
        {
            byte[] lmChallenge;
            byte[] ntChallengeResponse;
            byte[] lmChallengeResponse;

            if (LogonInformation.Value.LogonNetworkTransitive == null ||
                LogonInformation.Value.LogonNetworkTransitive.Length != 1)
            {
                return false;
            }

            if (LogonInformation.Value.LogonNetworkTransitive[0].LmChallenge.data == null ||
                LogonInformation.Value.LogonNetworkTransitive[0].LmChallenge.data.Length !=
                NrpcUtility.NL_CREDENTIAL_LENGTH)
            {
                return false;
            }

            if (LogonInformation.Value.LogonNetworkTransitive[0].LmChallengeResponse.Buffer == null ||
                LogonInformation.Value.LogonNetworkTransitive[0].LmChallengeResponse.Buffer.Length !=
                LogonInformation.Value.LogonNetworkTransitive[0].LmChallengeResponse.Length ||
                LogonInformation.Value.LogonNetworkTransitive[0].LmChallengeResponse.Length >
                LogonInformation.Value.LogonNetworkTransitive[0].LmChallengeResponse.MaximumLength)
            {
                return false;
            }

            if (LogonInformation.Value.LogonNetworkTransitive[0].NtChallengeResponse.Buffer == null ||
                LogonInformation.Value.LogonNetworkTransitive[0].NtChallengeResponse.Buffer.Length !=
                LogonInformation.Value.LogonNetworkTransitive[0].NtChallengeResponse.Length ||
                LogonInformation.Value.LogonNetworkTransitive[0].NtChallengeResponse.Length >
                LogonInformation.Value.LogonNetworkTransitive[0].NtChallengeResponse.MaximumLength)
            {
                return false;
            }

            if (DtypUtility.ToInt64(LogonInformation.Value.LogonNetworkTransitive[0].Identity.Reserved) != 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(NrpcParameterControlFlags),
                LogonInformation.Value.LogonNetworkTransitive[0].Identity.ParameterControl))
            {
                return false;
            }

            lmChallenge = LogonInformation.Value.LogonNetworkTransitive[0].LmChallenge.data;

            lmChallengeResponse = NlmpUtility.DESL(
                NlmpUtility.GetResponseKeyLm(NlmpVersion.v1, domainName, userName, password), lmChallenge);
            if (!ArrayUtility.CompareArrays<byte>(lmChallengeResponse,
                LogonInformation.Value.LogonNetworkTransitive[0].LmChallengeResponse.Buffer))
            {
                return false;
            }

            ntChallengeResponse = NlmpUtility.DESL(
                NlmpUtility.GetResponseKeyNt(NlmpVersion.v1, domainName, userName, password), lmChallenge);

            if (!ArrayUtility.CompareArrays<byte>(ntChallengeResponse,
                LogonInformation.Value.LogonNetworkTransitive[0].NtChallengeResponse.Buffer))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        ///  Validates Network Information
        /// </summary>
        /// <param name="domainName">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password of the user</param>
        /// <param name="LogonInformation">Logon information received</param>
        /// <returns>Whether the LogonInformation is correct</returns>
        private static bool ValidateNetworkInformation(string domainName, string userName,
            string password, _NETLOGON_LEVEL? LogonInformation)
        {
            byte[] lmChallenge;
            byte[] ntChallengeResponse;
            byte[] lmChallengeResponse;

            if (LogonInformation.Value.LogonNetwork == null ||
                LogonInformation.Value.LogonNetwork.Length != 1)
            {
                return false;
            }

            if (LogonInformation.Value.LogonNetwork[0].LmChallenge.data == null ||
                LogonInformation.Value.LogonNetwork[0].LmChallenge.data.Length !=
                NrpcUtility.NL_CREDENTIAL_LENGTH)
            {
                return false;
            }

            if (LogonInformation.Value.LogonNetwork[0].LmChallengeResponse.Buffer == null ||
                LogonInformation.Value.LogonNetwork[0].LmChallengeResponse.Buffer.Length !=
                LogonInformation.Value.LogonNetwork[0].LmChallengeResponse.Length ||
                LogonInformation.Value.LogonNetwork[0].LmChallengeResponse.Length >
                LogonInformation.Value.LogonNetwork[0].LmChallengeResponse.MaximumLength)
            {
                return false;
            }

            if (LogonInformation.Value.LogonNetwork[0].NtChallengeResponse.Buffer == null ||
                LogonInformation.Value.LogonNetwork[0].NtChallengeResponse.Buffer.Length !=
                LogonInformation.Value.LogonNetwork[0].NtChallengeResponse.Length ||
                LogonInformation.Value.LogonNetwork[0].NtChallengeResponse.Length >
                LogonInformation.Value.LogonNetwork[0].NtChallengeResponse.MaximumLength)
            {
                return false;
            }

            if (DtypUtility.ToInt64(LogonInformation.Value.LogonNetwork[0].Identity.Reserved) != 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(NrpcParameterControlFlags),
                LogonInformation.Value.LogonNetwork[0].Identity.ParameterControl))
            {
                return false;
            }

            lmChallenge = LogonInformation.Value.LogonNetwork[0].LmChallenge.data;

            lmChallengeResponse = NlmpUtility.DESL(
                NlmpUtility.GetResponseKeyLm(NlmpVersion.v1, domainName, userName, password), lmChallenge);
            if (!ArrayUtility.CompareArrays<byte>(lmChallengeResponse,
                LogonInformation.Value.LogonNetwork[0].LmChallengeResponse.Buffer))
            {
                return false;
            }

            ntChallengeResponse = NlmpUtility.DESL(
                NlmpUtility.GetResponseKeyNt(NlmpVersion.v1, domainName, userName, password), lmChallenge);

            if (!ArrayUtility.CompareArrays<byte>(ntChallengeResponse,
                LogonInformation.Value.LogonNetwork[0].NtChallengeResponse.Buffer))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        ///  Validates Interactive Transitive Information
        /// </summary>
        /// <param name="sessionContext">The session context of the RPC response to send</param>
        /// <param name="domainName">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password of the user</param>
        /// <param name="LogonInformation">Logon information received</param>
        /// <returns>Whether the LogonInformation is correct</returns>
        private static bool ValidateInteractiveTransitiveInformation(NrpcServerSessionContext sessionContext,
            string domainName, string userName, string password, _NETLOGON_LEVEL? LogonInformation)
        {
            _CYPHER_BLOCK[] lmOwfBlocks;
            _CYPHER_BLOCK[] ntOwfBlocks;
            if (LogonInformation.Value.LogonInteractiveTransitive == null ||
                LogonInformation.Value.LogonInteractiveTransitive.Length != 1)
            {
                return false;
            }

            lmOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password));

            ntOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password));

            NrpcServer.EncryptLmAndNtOwfPassword(sessionContext, ref lmOwfBlocks, ref ntOwfBlocks);

            if (!CompareHashPassword(LogonInformation.Value.LogonInteractiveTransitive[0].LmOwfPassword.data,
                lmOwfBlocks))
            {
                return false;
            }

            if (!CompareHashPassword(LogonInformation.Value.LogonInteractiveTransitive[0].NtOwfPassword.data,
                ntOwfBlocks))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        ///  Validates Netlogon Generic Information
        /// </summary>
        /// <param name="sessionContext">The session context of the RPC response to send</param>
        /// <param name="domainName">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password of the user</param>
        /// <param name="LogonInformation">Logon information received</param>
        /// <returns>Whether the LogonInformation is correct</returns>
        private static bool ValidateInteractiveInformation(NrpcServerSessionContext sessionContext,
            string domainName, string userName, string password, _NETLOGON_LEVEL? LogonInformation)
        {
            _CYPHER_BLOCK[] lmOwfBlocks;
            _CYPHER_BLOCK[] ntOwfBlocks;
            if (LogonInformation.Value.LogonInteractive == null ||
                LogonInformation.Value.LogonInteractive.Length != 1)
            {
                return false;
            }

            if (DtypUtility.ToInt64(LogonInformation.Value.LogonInteractive[0].Identity.Reserved) != 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(NrpcParameterControlFlags),
                LogonInformation.Value.LogonInteractive[0].Identity.ParameterControl))
            {
                return false;
            }

            lmOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password));

            ntOwfBlocks = NrpcUtility.CreateCypherBlocks(
                NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password));

            NrpcServer.EncryptLmAndNtOwfPassword(sessionContext, ref lmOwfBlocks, ref ntOwfBlocks);

            if (!CompareHashPassword(LogonInformation.Value.LogonInteractive[0].LmOwfPassword.data,
                lmOwfBlocks))
            {
                return false;
            }

            if (!CompareHashPassword(LogonInformation.Value.LogonInteractive[0].NtOwfPassword.data,
                ntOwfBlocks))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Validates Netlogon Generic Information
        /// </summary>
        /// <param name="LogonInformation">Logon information received</param>
        private static bool ValidateGenericLogonInformation(_NETLOGON_LEVEL? LogonInformation)
        {
            if (LogonInformation.Value.LogonGeneric == null ||
                LogonInformation.Value.LogonGeneric.Length != 1)
            {
                return false;
            }

            if (DtypUtility.ToInt64(LogonInformation.Value.LogonGeneric[0].Identity.Reserved) != 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(NrpcParameterControlFlags),
                LogonInformation.Value.LogonGeneric[0].Identity.ParameterControl))
            {
                return false;
            }

            if ((LogonInformation.Value.LogonGeneric[0].LogonData == null &&
                LogonInformation.Value.LogonGeneric[0].LogonData.Length != 0) ||
                LogonInformation.Value.LogonGeneric[0].LogonData.Length !=
                LogonInformation.Value.LogonGeneric[0].DataLength)
            {
                return false;
            }

            if ((LogonInformation.Value.LogonGeneric[0].PackageName.Buffer == null &&
                LogonInformation.Value.LogonGeneric[0].PackageName.Length != 0) ||
                LogonInformation.Value.LogonGeneric[0].PackageName.Length >
                LogonInformation.Value.LogonGeneric[0].PackageName.MaximumLength)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Check whether two cypher_block arrays are the same
        /// </summary>
        /// <returns>True if LogonInformation is valid, otherwise, returns false</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if value of logonLevel is out of range</exception>
        private static bool CompareHashPassword(_CYPHER_BLOCK[] cypherBlock1, _CYPHER_BLOCK[] cypherBlock2)
        {
            if (cypherBlock1 == null || cypherBlock1.Length != 2 || cypherBlock2 == null
                || cypherBlock2.Length != 2 || !ArrayUtility.CompareArrays<byte>(cypherBlock1[0].data,
                cypherBlock2[0].data) || !ArrayUtility.CompareArrays<byte>(cypherBlock1[1].data,
                cypherBlock2[1].data))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// compute message digest for NetrLogonComputeServerDigest and NetrLogonComputeClientDigest
        /// </summary>
        /// <param name="password">the machine account password</param>
        /// <param name="message">message used for compute digest</param>
        /// <returns>128-bit MD5 digest of current machine account password and message </returns>
        private static byte[] ComputeMessageDigest(string password, byte[] message)
        {
            // NTOWFv1 of password as TD 3.5.5.7.3
            byte[] ntowfv1NewPassword; 
            byte[] messageDigest;

            using (MD4 md4 = MD4.Create())
            {
                md4.Initialize();
                ntowfv1NewPassword = md4.ComputeHash(Encoding.Unicode.GetBytes(password));
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = ArrayUtility.ConcatenateArrays(ntowfv1NewPassword, message);
                messageDigest = md5.ComputeHash(buffer);
            }

            return messageDigest;
        }

        #endregion

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                rpceLayerServer.Dispose();
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~NrpcServer()
        {
            Dispose(false);
        }

        #endregion
    }
}
