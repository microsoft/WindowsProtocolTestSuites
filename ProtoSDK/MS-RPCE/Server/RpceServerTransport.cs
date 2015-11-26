// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RpceServerTransport provides transport layer 
    /// and support for upper layer protocol to comunicate.
    /// </summary>
    public class RpceServerTransport : IDisposable
    {
        // RPC INTERFACE UUID, VERS_MAJOR, VERS_MINOR
        private struct RpcIf
        {
            internal RpcIf(Guid ifId, ushort ifMajorVer, ushort ifMinorVer)
            {
                this.id = ifId;
                this.majorVer = ifMajorVer;
                this.minorVer = ifMinorVer;
            }

            internal Guid id;
            internal ushort majorVer;
            internal ushort minorVer;
        }

        // callback func to check interface is expected.
        private delegate bool RpcIfMatchFunc(RpcIf rpcIf);



        #region Field members

        //A instance of RPCE server
        private RpceServer rpceServer;

        //The list of interface to automatically accept bind
        private List<RpcIf> registeredInterfaceList;

        #endregion


        #region ctor

        /// <summary>
        /// Initialize RPCE server transport
        /// </summary>
        public RpceServerTransport()
        {
            this.rpceServer = new RpceServer();
            this.registeredInterfaceList = new List<RpcIf>();
        }

        #endregion


        /// <summary>
        /// A read-only collection for servercontexts in the RPCE server.
        /// </summary>
        public ReadOnlyCollection<RpceServerContext> ServerContexts
        {
            get
            {
                return this.rpceServer.ServerContexts;
            }
        }


        #region start and stop


        /// <summary>
        /// Register an interface and turn on automatic accept bind request.
        /// </summary>
        /// <param name="ifId">
        /// If_id to accept the bind. Null to accept all interface regardless the if_id.
        /// </param>
        /// <param name="ifMajorVer">
        /// If_major_ver to accept the bind. Null to accept all interface regardless the if_major_ver.
        /// </param>
        /// <param name="ifMinorVer">
        /// If_minor_ver to accept the bind. Null to accept all interface regardless the if_minor_ver.
        /// </param>
        public void RegisterInterface(
            Guid ifId,
            ushort ifMajorVer,
            ushort ifMinorVer)
        {
            RpcIf rpcIf = new RpcIf(ifId, ifMajorVer, ifMinorVer);
            this.registeredInterfaceList.Add(rpcIf);
        }


        /// <summary>
        /// Unregister an interface. If last interface is removed, turn off automatic accept bind request.
        /// </summary>
        /// <param name="ifId">
        /// If_id to accept the bind. Null to accept all interface regardless the if_id.
        /// </param>
        /// <param name="ifMajorVer">
        /// If_major_ver to accept the bind. Null to accept all interface regardless the if_major_ver.
        /// </param>
        /// <param name="ifMinorVer">
        /// If_minor_ver to accept the bind. Null to accept all interface regardless the if_minor_ver.
        /// </param>
        public void UnregisterInterface(
            Guid ifId,
            ushort ifMajorVer,
            ushort ifMinorVer)
        {
            for (int i = 0; i < this.registeredInterfaceList.Count; i++)
            {
                RpcIf rpcIf = this.registeredInterfaceList[i];
                if (rpcIf.id == ifId && rpcIf.majorVer == ifMajorVer && rpcIf.minorVer == ifMinorVer)
                {
                    this.registeredInterfaceList.RemoveAt(i);
                    break;
                }
            }
        }


        /// <summary>
        /// Set callback to create a ServerSecurityContext.
        /// </summary>
        /// <param name="securityContextCreator">A callback to create ServerSecurityContext.</param>
        public void SetSecurityContextCreator(RpceSecurityContextCreatingEventHandler securityContextCreator)
        {
            this.rpceServer.SetSecurityContextCreator(securityContextCreator);
        }


        /// <summary>
        /// Start to listen a TCP port.
        /// </summary>
        /// <param name="port">The TCP port to listen.</param>
        public virtual RpceServerContext StartTcp(ushort port)
        {
            return this.rpceServer.StartTcp(port);
        }


        /// <summary>
        ///  Stop Tcp server.
        /// </summary>
        /// <param name="port">The TCP port listened.</param>
        public virtual void StopTcp(ushort port)
        {
            this.rpceServer.StopTcp(port);
        }


        /// <summary>
        ///  Start to listen on a named pipe.
        /// </summary>
        /// <param name="namedPipe">The name of named pipe to listen.</param>       
        /// <param name="credential">Credential to be used by underlayer SMB/SMB2 transport.</param>
        /// <param name="ipAddress">server's ipAddress</param>
        public virtual RpceServerContext StartNamedPipe(string namedPipe, 
            AccountCredential credential, IPAddress ipAddress)
        {
            return this.rpceServer.StartNamedPipe(namedPipe, credential, ipAddress);
        }


        /// <summary>
        ///  Stop named pipe server.
        /// </summary>
        /// <param name="namedPipe">The name of the named pipe listened on</param>
        public virtual void StopNamedPipe(string namedPipe)
        {
            this.rpceServer.StopNamedPipe(namedPipe);
        }


        /// <summary>
        /// Stop all servers.
        /// </summary>
        public virtual void StopAll()
        {
            this.rpceServer.StopAll();
        }

        #endregion


        /// <summary>
        /// Expect to receive a call.
        /// </summary>
        /// <param name="timeout">Timeout of expecting a call.</param>
        /// <param name="sessionContext">Session Context of the received call.</param>
        /// <param name="opnum">Operation number of the method invoked.</param>
        /// <returns>Received a byte array of the request stub from a client.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server.
        /// </exception>
        public virtual byte[] ExpectCall(
            TimeSpan timeout,
            out RpceServerSessionContext sessionContext,
            out ushort opnum)
        {
            sessionContext = null;

            RpcePdu receivedPdu = ReceiveAndReassemblePdu(timeout, ref sessionContext);
            RpceCoRequestPdu requestPdu = receivedPdu as RpceCoRequestPdu;

            if (requestPdu == null)
            {
                throw new InvalidOperationException("Expect request_pdu, but received others.");
            }

            opnum = requestPdu.opnum;

            byte[] stub = requestPdu.stub;
            if (stub == null)
            {
                stub = new byte[0];
            }

            return stub;
        }


        /// <summary>
        /// Send a Response to client.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <param name="responseStub">A byte array of the response stub send to client.</param>
        /// <exception cref="ArgumentNullException">Thrown when responseStub is null.</exception>
        public virtual void SendResponse(
            RpceServerSessionContext sessionContext,
            byte[] responseStub)
        {
            if (responseStub == null)
            {
                throw new ArgumentNullException("responseStub");
            }

            RpceCoResponsePdu responsePdu = this.rpceServer.CreateCoResponsePdu(sessionContext, responseStub);
            FragmentAndSendPdu(sessionContext, responsePdu);
        }


        /// <summary>
        /// Send a Fault to client.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <param name="statusCode">Status code.</param>
        public virtual void SendFault(RpceServerSessionContext sessionContext, uint statusCode)
        {
            RpceCoFaultPdu faultPdu = this.rpceServer.CreateCoFaultPdu(sessionContext, null, statusCode);
            FragmentAndSendPdu(sessionContext, faultPdu);
        }


        #region Bind / Disconnect

        /// <summary>
        /// Calling this method to be notified when a new RPC connection is coming. 
        /// Users, who don't care about the coming of a new connection, just call ExpectCall directly.
        /// </summary>
        /// <param name="ifId">
        /// If_id to accept the bind. Null to accept all interface regardless the if_id.
        /// </param>
        /// <param name="ifMajorVer">
        /// If_major_ver to accept the bind. Null to accept all interface regardless the if_major_ver.
        /// </param>
        /// <param name="ifMinorVer">
        /// If_minor_ver to accept the bind. Null to accept all interface regardless the if_minor_ver.
        /// </param>
        /// <param name="timeout">Timeout of expecting a connection.</param>
        /// <param name="sessionContext">The sessionContext of binded connection.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when securityProvider is null and auth_level is not NONE 
        /// </exception>
        /// <exception cref="SspiException">
        /// Thrown when accept client token failed
        /// </exception>
        public virtual void ExpectBind(
            Guid? ifId,
            ushort? ifMajorVer,
            ushort? ifMinorVer,
            TimeSpan timeout,
            out RpceServerSessionContext sessionContext)
        {
            if (this.registeredInterfaceList.Count > 0)
            {
                throw new InvalidOperationException("Auto accept bind was turned on, ExpectBind is not allowed.");
            }

            DateTime t0 = DateTime.Now;

            sessionContext = rpceServer.ExpectConnect(timeout);

            RpcIfMatchFunc matchFunc = delegate(RpcIf rpcIf)
            {
                return !((ifId != null && rpcIf.id != ifId.Value)
                || (ifMajorVer != null && rpcIf.majorVer != ifMajorVer.Value)
                || (ifMinorVer != null && rpcIf.minorVer != ifMinorVer.Value));
            };

            while (!InternalExpectBind(matchFunc, timeout - (DateTime.Now - t0), ref sessionContext))
            {
                if ((DateTime.Now - t0) >= timeout)
                {
                    throw new TimeoutException();
                }
            }
        }


        /// <summary>
        /// Expect to disconnect from the specific client.
        /// </summary>
        /// <param name="timeout">Timeout of expecting to disconnect with client.</param>
        /// <param name="sessionContext">The sessionContext of expecting to disconnect.</param>
        public virtual void ExpectDisconnect(
            TimeSpan timeout,
            out RpceServerSessionContext sessionContext)
        {
            if (this.registeredInterfaceList.Count > 0)
            {
                throw new InvalidOperationException("Auto accept bind was turned on, ExpectBind is not allowed.");
            }

            sessionContext = null;
            this.rpceServer.ExpectDisconnect(timeout, ref sessionContext);
        }


        /// <summary>
        /// Disconnect with a client.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session</param>
        public virtual void Disconnect(RpceServerSessionContext sessionContext)
        {
            this.rpceServer.Disconnect(sessionContext);
        }

        #endregion


        #region private method


        /// <summary>
        /// Calling this method to be notified when a new RPC connection is coming. 
        /// </summary>
        /// <param name="ifMatchFunc">Matching function.</param>
        /// <param name="timeout">Timeout of expecting a connection.</param>
        /// <param name="sessionContext">The sessionContext of binded connection.</param>
        /// <returns>If bind succeeded, return true; otherwise, false.</returns>
        private bool InternalExpectBind(
            RpcIfMatchFunc ifMatchFunc,
            TimeSpan timeout,
            ref RpceServerSessionContext sessionContext)
        {
            RpcePdu receivedPdu = ReceiveAndReassemblePdu(timeout, ref sessionContext);
            RpceCoBindPdu bindPdu = receivedPdu as RpceCoBindPdu;

            if (bindPdu == null)
            {
                throw new InvalidOperationException("Expect bind_pdu, but received others.");
            }

            RpcIf rpcIf = new RpcIf(sessionContext.InterfaceId, sessionContext.InterfaceMajorVersion, sessionContext.InterfaceMinorVersion);
            if (!ifMatchFunc(rpcIf))
            {
                // Interface doesn't match, response BindNak
                RpceCoBindNakPdu bindNakPdu =
                    rpceServer.CreateCoBindNakPdu(sessionContext, p_reject_reason_t.REASON_NOT_SPECIFIED, null);
                FragmentAndSendPdu(sessionContext, bindNakPdu);
                return false;
            }

            RpceCoBindAckPdu bindAckPdu = rpceServer.CreateCoBindAckPdu(sessionContext);

            FragmentAndSendPdu(sessionContext, bindAckPdu);

            while (sessionContext.SecurityContext != null && sessionContext.SecurityContextNeedContinueProcessing)
            {
                receivedPdu = ReceiveAndReassemblePdu(timeout, ref sessionContext);
                RpceCoAlterContextPdu alterContextPdu = receivedPdu as RpceCoAlterContextPdu;
                RpceCoAuth3Pdu auth3Pdu = receivedPdu as RpceCoAuth3Pdu;

                if (alterContextPdu != null)
                {
                    RpceCoAlterContextRespPdu alterContextRespPdu =
                        rpceServer.CreateCoAlterContextRespPdu(sessionContext);

                    FragmentAndSendPdu(sessionContext, alterContextRespPdu);
                }
                else if (auth3Pdu != null)
                {
                    //Do nothing
                }
            }

            return true;
        }


        /// <summary>
        /// Receive and reassemble PDU.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU</param>
        /// <param name="sessionContext">Context of the RPCE session</param>
        /// <returns>Received PDU</returns>
        private RpcePdu ReceiveAndReassemblePdu(
            TimeSpan timeout,
            ref RpceServerSessionContext sessionContext)
        {
            RpcePdu receivedPdu;
            bool expectAny = sessionContext == null;

            WaitForEvent:

            if (expectAny)
            {
                sessionContext = null;
            }
            EventType eventType = rpceServer.ExpectEvent(timeout, ref sessionContext, out receivedPdu);
            if (this.registeredInterfaceList.Count > 0)
            {
                // auto accept connect/bind/disconnect
                if (eventType == EventType.Connected)
                {
                    RpcIfMatchFunc matchFunc = delegate(RpcIf rpcIf)
                    {
                        for (int i = 0; i < this.registeredInterfaceList.Count; i++)
                        {
                            if (this.registeredInterfaceList[i].Equals(rpcIf))
                            {
                                return true;
                            }
                        }
                        return false;
                    };
                    InternalExpectBind(matchFunc, timeout, ref sessionContext);
                    goto WaitForEvent;
                }
                else if (eventType == EventType.Disconnected)
                {
                    goto WaitForEvent;
                }
                else
                {
                    // it is a PDU.
                }
            }
            else if (eventType != EventType.ReceivedPacket)
            {
                throw new InvalidOperationException(
                    string.Format("Unexpected event ({0}) received.", eventType));
            }


            RpceCoPdu receivedCoPdu = receivedPdu as RpceCoPdu;
            if (receivedCoPdu == null)
            {
                return receivedPdu;
            }

            List<RpceCoPdu> pduList = new List<RpceCoPdu>();
            pduList.Add(receivedCoPdu);

            while ((receivedCoPdu.pfc_flags & RpceCoPfcFlags.PFC_LAST_FRAG) == 0)
            {
                receivedPdu = rpceServer.ExpectPdu(timeout, ref sessionContext);

                receivedCoPdu = receivedPdu as RpceCoPdu;
                if (receivedCoPdu == null)
                {
                    throw new InvalidOperationException("CL PDU received inside a connection.");
                }

                pduList.Add(receivedCoPdu);
            }

            return RpceUtility.ReassemblePdu(sessionContext, pduList.ToArray());
        }

        /// <summary>
        /// Fragment and send PDU.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session</param>
        /// <param name="pdu">PDU to Fragment and send.</param>
        private void FragmentAndSendPdu(
            RpceServerSessionContext sessionContext,
            RpceCoPdu pdu)
        {
            if (pdu.PTYPE == RpcePacketType.Bind
                || pdu.PTYPE == RpcePacketType.BindAck
                || pdu.PTYPE == RpcePacketType.AlterContext
                || pdu.PTYPE == RpcePacketType.AlterContextResp
                || pdu.PTYPE == RpcePacketType.Auth3)
            {
                pdu.InitializeAuthenticationToken();
                pdu.SetLength();
                foreach (RpceCoPdu fragPdu in RpceUtility.FragmentPdu(sessionContext, pdu))
                {
                    rpceServer.SendPdu(sessionContext, fragPdu);
                }
            }
            else
            {
                foreach (RpceCoPdu fragPdu in RpceUtility.FragmentPdu(sessionContext, pdu))
                {
                    fragPdu.InitializeAuthenticationToken();
                    rpceServer.SendPdu(sessionContext, fragPdu);
                }
            }
        }

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
                this.rpceServer.Dispose();
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~RpceServerTransport()
        {
            Dispose(false);
        }

        #endregion
    }
}
