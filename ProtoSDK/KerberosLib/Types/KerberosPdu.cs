// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    #region PDU definition
    /// <summary>
    /// A type of PDU to indicate the PDU is a KILE PDU. It's a base class for all input/output PDUs.
    /// </summary> 
    public class KerberosPdu : StackPacket
    {
        public KerberosPdu()
        { 
        }

        /// <summary>
        /// This constructor is used to set field packetBytes in StackPacket.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        public KerberosPdu(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            return null;
        }


        /// <summary>
        /// Decode the KILE PDU from the message bytes.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        public virtual void FromBytes(byte[] buffer)
        {
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU.
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            return null;
        }

        
    }

    #endregion
}
