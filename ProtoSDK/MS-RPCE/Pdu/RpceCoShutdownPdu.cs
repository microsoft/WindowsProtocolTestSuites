// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The shutdown PDU is sent by the server to request that 
    /// a client terminate the connection, freeing the related resources.<para/>
    /// The shutdown PDU never contains an authentication verifier 
    /// even if authentication services are in use.
    /// </summary>
    public class RpceCoShutdownPdu : RpceCoPdu
    {
        /// <summary>
        /// Initialize an instance of RpceCoShutdownPdu class.
        /// </summary>
        /// <param name="context">context</param>
        public RpceCoShutdownPdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoShutdownPdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        public RpceCoShutdownPdu(RpceContext context, byte[] pduBytes)
            : base(context, pduBytes)
        {
        }
        
    }
}
