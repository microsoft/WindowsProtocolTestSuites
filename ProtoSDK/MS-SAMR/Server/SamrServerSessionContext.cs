// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    /// <summary>
    /// A class contains context information of a SAMR session
    /// </summary>
    public class SamrServerSessionContext 
    {
        //The last request received.
        private SamrRequestStub requestReceived;

        private RpceServerSessionContext underlyingSessionContext;

        /// <summary>
        /// The last RPC request received.
        /// </summary>
        public SamrRequestStub LastRequestReceived
        {
            get
            {
                return requestReceived;
            }
            set
            {
                requestReceived = value;
            }
        }


        /// <summary>
        /// The corresponding RPCE layer session context
        /// </summary>
        public RpceServerSessionContext RpceLayerSessionContext
        {
            get
            {
                return underlyingSessionContext;
            }
            internal set
            {
                underlyingSessionContext = value;
            }
        }


        /// <summary>
        /// sessionKey of SMB session, used by crypto methods
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                if (underlyingSessionContext != null
                    && underlyingSessionContext.FileServiceTransportOpen != null
                    && underlyingSessionContext.FileServiceTransportOpen.TreeConnect != null
                    && underlyingSessionContext.FileServiceTransportOpen.TreeConnect.Session != null)
                {
                    return underlyingSessionContext.FileServiceTransportOpen.TreeConnect.Session.SessionKey4Smb;
                }
                return null;
            }
        }


        /// <summary>
        /// Initialize a Samr server session context class.
        /// </summary>
        internal SamrServerSessionContext()
        {

        }


        /// <summary>
        ///  Update the session context after receiving a request from the client.
        /// </summary>
        /// <param name="messageReceived">The Samr request received</param>
        internal void UpdateSessionContextWithMessageReceived(SamrRequestStub messageReceived)
        {
            requestReceived = messageReceived;
        }
    }
}

