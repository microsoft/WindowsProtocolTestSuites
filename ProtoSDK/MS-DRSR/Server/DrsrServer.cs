// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// DRSR Server SDK
    /// </summary>
    public class DrsrServer : IDisposable
    {
        private DrsrContextManager contextManager;
        private RpceServerTransport rpceLayerServer;


        #region Constructor

        /// <summary>
        /// Constructor, initialize a DRSR server.
        /// Create the instance will not listen for RPC calls, 
        /// you should call StartTcp or StartNamedPipe 
        /// to actually listen for RPC calls.
        /// Delegate RpceSecurityCreator will be called to create server security
        /// context for Netlogon, etc.
        /// </summary>
        /// <param name="rpceServerSecurityContextCreator">Server security creator</param>
        [CLSCompliant(false)]
        public DrsrServer(RpceSecurityContextCreatingEventHandler rpceServerSecurityContextCreator)
        {
            contextManager = new DrsrContextManager();
            rpceLayerServer = new RpceServerTransport();
            rpceLayerServer.RegisterInterface(
                DrsrUtility.DRSUAPI_RPC_INTERFACE_UUID,
                DrsrUtility.DRSUAPI_RPC_INTERFACE_MAJOR_VERSION,
                DrsrUtility.DRSUAPI_RPC_INTERFACE_MINOR_VERSION);
            rpceLayerServer.RegisterInterface(
                DrsrUtility.DSAOP_RPC_INTERFACE_UUID,
                DrsrUtility.DSAOP_RPC_INTERFACE_MAJOR_VERSION,
                DrsrUtility.DSAOP_RPC_INTERFACE_MINOR_VERSION);
            rpceLayerServer.SetSecurityContextCreator(rpceServerSecurityContextCreator);
        }

        #endregion


        /// <summary>
        /// Throws an exception when session context is null
        /// </summary>
        ///<param name="sessionContext">Context of the session</param>
        ///<exception cref="ArgumentNullException">Thrown when session context is null</exception>
        internal protected void CheckIfSessionContextIsNull(DrsrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }
        }


        #region User interface for DRSR protocol

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
        /// Receives RPC calls from clients
        /// </summary>
        /// <param name="interfaceType">Drsr interface type, drsuapi or dsaop.</param>    
        /// <param name="timeout">The maximum time waiting for RPC calls</param>
        /// <param name="sessionContext">The session context of the RPC call received</param>
        /// <returns>The input parameters of the RPC call received</returns>
        /// <exception cref="InvalidOperationException">Thrown when an invalid method is called, or
        /// an unexpected method is called</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public T ExpectRpcCall<T>(
            DrsrRpcInterfaceType interfaceType,
            TimeSpan timeout,
            out DrsrServerSessionContext sessionContext)
            where T : DrsrRequestStub
        {
            RpceServerSessionContext rpceSessionContext;
            ushort opnum;

            byte[] requestStub = rpceLayerServer.ExpectCall(timeout, out rpceSessionContext, out opnum);

            if ((interfaceType == DrsrRpcInterfaceType.DSAOP 
                    && !Enum.IsDefined(typeof(DsaopMethodOpnums), (ushort)opnum))
                || (interfaceType == DrsrRpcInterfaceType.DRSUAPI 
                    && !Enum.IsDefined(typeof(DrsuapiMethodOpnums), (ushort)opnum)))
            {
                throw new InvalidOperationException("An invalid method is invoked");
            }

            //If there isn't a corresponding Drsr session context, it's a new session
            if (contextManager.LookupSessionContext(rpceSessionContext, out sessionContext))
            {
                sessionContext.RpceLayerSessionContext = rpceSessionContext;
            }

            T t;
            if (typeof(T) == typeof(DrsrRequestStub))
            {
                t = (T)DrsrUtility.CreateDrsrRequestStub(interfaceType, opnum);
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
            sessionContext.UpdateSessionContextWithMessageReceived(interfaceType, t);
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
        public void SendRpcCallResponse(DrsrServerSessionContext sessionContext, DrsrResponseStub messageToSend)
        {
            CheckIfSessionContextIsNull(sessionContext);

            if (messageToSend == null)
            {
                throw new ArgumentNullException("messageToSend");
            }

            sessionContext.UpdateSessionContextWithMessageSent(sessionContext.InterfaceType, messageToSend);
            rpceLayerServer.SendResponse(
                sessionContext.RpceLayerSessionContext,
                messageToSend.Encode(sessionContext));
        }


        /// <summary>
        /// Remove a session context from the context manager
        /// </summary>
        /// <param name="sessionContext">The session context to remove</param>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        public void RemoveSessionContext(DrsrServerSessionContext sessionContext)
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
                rpceLayerServer.Dispose();
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~DrsrServer()
        {
            Dispose(false);
        }

        #endregion
    }
}
