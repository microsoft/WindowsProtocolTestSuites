// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// LSA Server SDK
    /// </summary>
    public class LsaServer : IDisposable
    {
        private LsaContextManager contextManager;

        private RpceServerTransport rpceLayerServer;

        #region Constructor

        /// <summary>
        /// Constructor, initialize a LSA server.
        /// Create the instance will not listen for RPC calls, 
        /// you should call StartTcp or StartNamedPipe 
        /// to actually listen for RPC calls.
        /// Delegate RpceSecurityCreator will be called to create server security
        /// context for Netlogon, etc.
        /// </summary>
        /// <param name="rpceServerSecurityContextCreator">Server security creator</param>
        [CLSCompliant(false)]
        public LsaServer(RpceSecurityContextCreatingEventHandler rpceServerSecurityContextCreator)
        {
            contextManager = new LsaContextManager();
            rpceLayerServer = new RpceServerTransport();
            rpceLayerServer.RegisterInterface(
                LsaUtility.LSA_RPC_INTERFACE_UUID,
                LsaUtility.LSA_RPC_INTERFACE_MAJOR_VERSION,
                LsaUtility.LSA_RPC_INTERFACE_MINOR_VERSION);
            rpceLayerServer.SetSecurityContextCreator(rpceServerSecurityContextCreator);
        }

        /// <summary>
        ///Constructor, initialize a Lsa server.<para/>
        /// Create the instance will not listen for RPC calls, 
        /// you should call StartTcp or StartNamedPipe 
        /// to actually listen for RPC calls.
        /// </summary>
        /// <param name="rpceServer">the rpce transport that already set securityContextCreator and 
        /// you intend to use as transport of your Lsa server</param>
        [CLSCompliant(false)]
        public LsaServer(RpceServerTransport rpceServer)
        {
            contextManager = new LsaContextManager();
            rpceLayerServer = rpceServer;
            rpceLayerServer.RegisterInterface(
                LsaUtility.LSA_RPC_INTERFACE_UUID,
                LsaUtility.LSA_RPC_INTERFACE_MAJOR_VERSION,
                LsaUtility.LSA_RPC_INTERFACE_MINOR_VERSION);
        }

        #endregion

        /// <summary>
        /// Throws an exception when session context is null
        /// </summary>
        ///<param name="sessionContext">Context of the session</param>
        ///<exception cref="ArgumentNullException">Thrown when session context is null</exception>
        private void CheckIfSessionContextIsNull(LsaServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }
        }


        #region User interface for LSA protocol

        /// <summary>
        /// Start to listen a TCP port.
        /// </summary>
        /// <param name="port">The TCP port to listen</param>
        [CLSCompliant(false)]
        public void StartTcp(ushort port)
        {
            rpceLayerServer.StartTcp(port);
        }


        /// <summary>
        /// Stop Tcp server.
        /// </summary>
        /// <param name="port">The TCP port listened</param>
        [CLSCompliant(false)]
        public void StopTcp(ushort port)
        {
            rpceLayerServer.StopTcp(port);
        }


        /// <summary>
        /// Start to listen a named pipe.
        /// </summary>
        /// <param name="namedPipe">Name of the named pipe</param>
        /// <param name="credential">Credential to be used by underlayer SMB/SMB2 transport.</param>
		/// <param name="ipAddress">server's ipAddress</param>
        public void StartNamedPipe(string namedPipe, AccountCredential credential, IPAddress ipAddress)
        {
            rpceLayerServer.StartNamedPipe(namedPipe, credential, ipAddress);
        }


        /// <summary>
        /// Stop named pipe server.
        /// </summary>
        /// <param name="namedPipe">Name of the named pipe</param>
        public void StopNamedPipe(string namedPipe)
        {
            rpceLayerServer.StopNamedPipe(namedPipe);
        }


        /// <summary>
        /// Receives RPC calls from clients
        /// </summary>
        /// <param name="timeout">The maximum time waiting for RPC calls</param>
        /// <param name="sessionContext">The session context of the RPC call received</param>
        /// <returns>The input parameters of the RPC call received</returns>
        /// <exception cref="InvalidOperationException">Thrown when an invalid method is called, or
        /// an unexpected method is called</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public T ExpectRpcCall<T>(TimeSpan timeout, out LsaServerSessionContext sessionContext)
            where T : LsaRequestStub
        {
            RpceServerSessionContext rpceSessionContext;
            ushort opnum;

            byte[] requestStub = rpceLayerServer.ExpectCall(timeout, out rpceSessionContext, out opnum);

            if (!Enum.IsDefined(typeof(LsaMethodOpnums), (int)opnum))
            {
                throw new InvalidOperationException("An invalid method is invoked");
            }

            //If there isn't a corresponding lsa session context, it's a new session
            if (contextManager.LookupSessionContext(rpceSessionContext, out sessionContext))
            {
                sessionContext.RpceLayerSessionContext = rpceSessionContext;
            }

            T t;
            if (typeof(T) == typeof(LsaRequestStub))
            {
                t = (T)LsaUtility.CreateLsaRequestStub((LsaMethodOpnums)opnum);
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
        /// Sends RPC response to the client
        /// </summary>
        /// <param name="sessionContext">The session context of the RPC response to send</param>
        /// <param name="messageToSend">The RPC response to send</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionContext or messageToSend is null.
        /// </exception>
        public void SendRpcCallResponse(LsaServerSessionContext sessionContext, LsaResponseStub messageToSend)
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
        /// Remove a session context from the context manager
        /// </summary>
        /// <param name="sessionContext">The session context to remove</param>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        public void RemoveSessionContext(LsaServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);
            contextManager.RemoveSessionContext(sessionContext.RpceLayerSessionContext);
        }


        /// <summary>
        /// Send a Fault to client.
        /// </summary>
        /// <param name="sessionContext">Context of the LSA session.</param>
        /// <param name="statusCode">Status code.</param>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [CLSCompliant(false)]
        public virtual void SendFault(LsaServerSessionContext sessionContext, uint statusCode)
        {
            CheckIfSessionContextIsNull(sessionContext);
            rpceLayerServer.SendFault(sessionContext.RpceLayerSessionContext, statusCode);
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
                rpceLayerServer.Dispose();
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~LsaServer()
        {
            Dispose(false);
        }

        #endregion
    }
}
