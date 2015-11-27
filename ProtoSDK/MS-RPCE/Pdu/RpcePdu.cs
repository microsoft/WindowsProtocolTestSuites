// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RpcePdu is a base class of RPCE PDU, both CO and CL.
    /// </summary>
    public abstract class RpcePdu : StackPacket
    {
        /// <summary>
        /// context
        /// </summary>
        protected RpceContext context;


        /// <summary>
        /// Initialize an instance of RpcePdu class.
        /// </summary>
        /// <param name="context">context</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when context is null.
        /// </exception>
        protected RpcePdu(RpceContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
        }


        /// <summary>
        /// Initialize an instance of RpcePdu class.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when context or pduBytes is null.
        /// </exception>
        protected RpcePdu(RpceContext context, byte[] pduBytes)
            : base(pduBytes)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (pduBytes == null)
            {
                throw new ArgumentNullException("pduBytes");
            }

            this.context = context;
        }


        /// <summary>
        /// Clone the object.
        /// </summary>
        /// <returns>A new instance with the same field value.</returns>
        /// <exception cref="NotSupportedException">Always throw NotSupportedException.</exception>
        public override StackPacket Clone()
        {
            throw new NotSupportedException("PDU clone is not supported.");
        }


        /// <summary>
        /// Get the size of structure. 
        /// Length of stub, auth_verifier and auth_value is not included.
        /// </summary>
        /// <returns>The size</returns>
        // Because this method will be overrided in derived classes, 
        // and there'll be some complex logic in it. We use method instead of property.
        internal abstract int GetSize();


        /// <summary>
        /// Marshal the PDU struct to a byte array.
        /// </summary>
        /// <param name="binaryWriter">BinaryWriter</param>
        internal abstract void ToBytes(BinaryWriter binaryWriter);


        /// <summary>
        /// Un-marshal a byte array to PDU struct.
        /// </summary>
        /// <param name="binaryReader">A binary reader.</param>
        internal abstract void FromBytes(BinaryReader binaryReader);


        /// <summary>
        /// Append auth_verifier to the end of PDU.
        /// </summary>
        public abstract void AppendAuthenticationVerifier();


        /// <summary>
        /// Set length field of PDU.
        /// </summary>
        public abstract void SetLength();


        /// <summary>
        /// Encrypt stub and initial auth_token.
        /// </summary>
        public abstract void InitializeAuthenticationToken();


        /// <summary>
        /// Decrypt stub and validate auth_token.
        /// </summary>
        public abstract bool ValidateAuthenticationToken();
    }
}
