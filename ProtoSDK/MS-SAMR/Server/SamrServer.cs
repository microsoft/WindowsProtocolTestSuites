// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    /// <summary>
    /// SAMR server SDK
    /// </summary>
    public class SamrServer : IDisposable
    {
        private SamrContextManager contextManager;

        private RpceServerTransport rpceLayerServer;

        #region Constructor
        /// <summary>
        /// Constructor, initialize a Samr server.<para/>
        /// Create the instance will not listen for RPC calls, 
        /// you should call StartTcp or StartNamedPipe 
        /// to actually listen for RPC calls.
        /// </summary>     
        /// <param name="securityContextCreator">Server security creator</param>
        public SamrServer(RpceSecurityContextCreatingEventHandler securityContextCreator)
        {            
            contextManager = new SamrContextManager();
            rpceLayerServer = new RpceServerTransport();
            rpceLayerServer.RegisterInterface(
                SamrUtility.SAMR_RPC_INTERFACE_UUID,
                SamrUtility.SAMR_RPC_INTERFACE_MAJOR_VERSION,
                SamrUtility.SAMR_RPC_INTERFACE_MINOR_VERSION);
            this.rpceLayerServer.SetSecurityContextCreator(securityContextCreator);
        }

        /// <summary>
        ///Constructor, initialize a Samr server.<para/>
        /// Create the instance will not listen for RPC calls, 
        /// you should call StartTcp or StartNamedPipe 
        /// to actually listen for RPC calls.
        /// </summary>
        /// <param name="rpceServer">the rpce transport that already set securityContextCreator and 
        /// you intend to use as transport of your samr server</param>
        public SamrServer(RpceServerTransport rpceServer)
        {
            contextManager = new SamrContextManager();
            rpceLayerServer = rpceServer;
            rpceLayerServer.RegisterInterface(
                SamrUtility.SAMR_RPC_INTERFACE_UUID,
                SamrUtility.SAMR_RPC_INTERFACE_MAJOR_VERSION,
                SamrUtility.SAMR_RPC_INTERFACE_MINOR_VERSION);
        }
        #endregion


        #region helper
        /// <summary>
        /// Throws an exception when session context is null
        /// </summary>
        ///<param name="sessionContext">Context of the session</param>
        ///<exception cref="ArgumentNullException">Thrown when session context is null</exception>
        private void CheckIfSessionContextIsNull(SamrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }
        }
        #endregion


        #region User interface for Samr protocol
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
        public T ExpectRpcCall<T>(TimeSpan timeout, out SamrServerSessionContext sessionContext)
            where T: SamrRequestStub
        {
            RpceServerSessionContext rpceSessionContext;
            ushort opnum;

            byte[] requestStub = rpceLayerServer.ExpectCall(timeout, out rpceSessionContext, out opnum);

            if (!Enum.IsDefined(typeof(SamrMethodOpnums), (int)opnum))
            {
                throw new InvalidOperationException("An invalid method is invoked");
            }

            //If there isn't a corresponding Samr session context, it's a new session
            if (contextManager.LookupSessionContext(rpceSessionContext, out sessionContext))
            {
                sessionContext.RpceLayerSessionContext = rpceSessionContext;
            }

            T t;
            if (typeof(T) == typeof(SamrRequestStub))
            {
                t = (T)SamrUtility.CreateSamrRequestStub((SamrMethodOpnums)opnum);
            }
            else
            {
                t = Activator.CreateInstance<T>();
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
        public void SendRpcCallResponse(SamrServerSessionContext sessionContext, SamrResponseStub messageToSend)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (messageToSend == null)
            {
                throw new ArgumentNullException("messageToSend");
            }
            rpceLayerServer.SendResponse(sessionContext.RpceLayerSessionContext, messageToSend.Encode(sessionContext));
        }


        /// <summary>
        ///  Remove a session context from the context manager
        /// </summary>
        /// <param name="sessionContext">The session context to remove</param>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        public void RemoveSessionContext(SamrServerSessionContext sessionContext)
        {
            CheckIfSessionContextIsNull(sessionContext);
            contextManager.RemoveSessionContext(sessionContext.RpceLayerSessionContext);
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
            }

            // Release unmanaged resources.
            rpceLayerServer.Dispose();
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~SamrServer()
        {
            Dispose(false);
        }
        #endregion
    }

}
