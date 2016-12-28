// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    internal static class SecureChannelThread
    {
        //time out in milliseconds
        private const int DefaultTimeout = 500;

        private static bool isStopped;

        internal static NrpcServer serverForSecureChannel;

        private static ushort tcpPort;

        internal static bool IsStopped
        {
            get
            {
                return isStopped;
            }

            set
            {
                isStopped = value;
            }
        }

        internal static ushort TcpPort
        {
            get
            {
                return tcpPort;
            }

            set
            {
                tcpPort = value;
            }
        }


        /// <summary>
        ///  Receives and processes remote requests to establish secure channels
        /// </summary>
        /// <param name="threadParameter">A dictionary whose key is the machine name and the 
        /// value is the machine password</param>
        internal static void EstablishSecureChannel(object threadParameter)
        {
            Dictionary<string, string> machineNameToPasswordDictionary = 
                threadParameter as Dictionary<string, string>;

            if (serverForSecureChannel == null)
            {
                serverForSecureChannel = new NrpcServer(null);
                serverForSecureChannel.StartTcp(tcpPort);
                serverForSecureChannel.StartNamedPipe(NrpcUtility.NETLOGON_RPC_OVER_NP_WELLKNOWN_ENDPOINT, null, IPAddress.Any);
            }

            TimeSpan defaultTimeSpan = new TimeSpan(0, 0, 0, 0, DefaultTimeout);
            NrpcServerSessionContext sessionContext;

            while (!isStopped)
            {
                NrpcRequestStub request = serverForSecureChannel.ExpectRpcCall<NrpcRequestStub>(defaultTimeSpan, out sessionContext);
                switch (request.Opnum)
                {
                    case NrpcMethodOpnums.NetrServerReqChallenge:
                        NrpcNetrServerReqChallengeRequest nrpcNetrServerReqChallengeRequest = 
                            request as NrpcNetrServerReqChallengeRequest;
                        NrpcNetrServerReqChallengeResponse nrpcNetrServerReqChallengeResponse;

                        if (!machineNameToPasswordDictionary.ContainsKey(sessionContext.ClientComputerName))
                        {
                            nrpcNetrServerReqChallengeResponse =
                                serverForSecureChannel.CreateNetrServerReqChallengeResponse(sessionContext,
                                NrpcUtility.GenerateNonce(8), "");
                            nrpcNetrServerReqChallengeResponse.Status = NtStatus.STATUS_ACCESS_DENIED;
                        }
                        else
                        {
                            nrpcNetrServerReqChallengeResponse =
                                serverForSecureChannel.CreateNetrServerReqChallengeResponse(sessionContext,
                                NrpcUtility.GenerateNonce(8), 
                                machineNameToPasswordDictionary[sessionContext.ClientComputerName]);
                        }
                        serverForSecureChannel.SendRpcCallResponse(
                            sessionContext, nrpcNetrServerReqChallengeResponse);
                        break;

                    case NrpcMethodOpnums.NetrServerAuthenticate:
                        NrpcNetrServerAuthenticateRequest nrpcNetrServerAuthenticateRequest =
                            request as NrpcNetrServerAuthenticateRequest;
                        NrpcNetrServerAuthenticateResponse nrpcNetrServerAuthenticateResponse = 
                            serverForSecureChannel.CreateNetrServerAuthenticateResponse(sessionContext);

                        serverForSecureChannel.SendRpcCallResponse(
                            sessionContext, nrpcNetrServerAuthenticateResponse);
                        break;

                    case NrpcMethodOpnums.NetrServerAuthenticate2:
                        NrpcNetrServerAuthenticate2Request nrpcNetrServerAuthenticate2Request =
                            request as NrpcNetrServerAuthenticate2Request;
                        NrpcNetrServerAuthenticate2Response nrpcNetrServerAuthenticate2Response =
                            serverForSecureChannel.CreateNetrServerAuthenticate2Response(sessionContext,
                            (uint)sessionContext.NegotiateFlags);

                        serverForSecureChannel.SendRpcCallResponse(
                            sessionContext, nrpcNetrServerAuthenticate2Response);
                        break;

                    case NrpcMethodOpnums.NetrServerAuthenticate3:
                        NrpcNetrServerAuthenticate3Request nrpcNetrServerAuthenticate3Request =
                            request as NrpcNetrServerAuthenticate3Request;
                        NrpcNetrServerAuthenticate3Response nrpcNetrServerAuthenticate3Response =
                            serverForSecureChannel.CreateNetrServerAuthenticate3Response(sessionContext,
                            (uint)sessionContext.NegotiateFlags, 100);

                        serverForSecureChannel.SendRpcCallResponse(
                            sessionContext, nrpcNetrServerAuthenticate3Response);
                        break;

                    default:
                        //bypass other requests
                        break;
                }
            }

            serverForSecureChannel.StopTcp(tcpPort);
            serverForSecureChannel.StopNamedPipe(NrpcUtility.NETLOGON_RPC_OVER_NP_WELLKNOWN_ENDPOINT);
            serverForSecureChannel.Dispose();
            serverForSecureChannel = null;
        }
    }
}
