// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public abstract class SpngPacket: StackPacket
    {
        private Asn1Object asn1Token;

        /// <summary>
        /// The Asn.1 formatted token contains in the class
        /// </summary>
        public Asn1Object Asn1Token
        {
            get
            {
                return this.asn1Token;
            }
            set
            {
                this.asn1Token = value;
            }
        }

        public SpngPacket()
        {
        }

        /// <summary>
        /// This constructor is used to set field packetBytes in StackPacket.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        public SpngPacket(byte[] data)
            : base(data)
        {
        }

        public SpngPacket(Asn1Object token)
        {
            this.asn1Token = token;
        }

        public override byte[] ToBytes()
        {
            return SpngUtility.EncodeAsn1(this.asn1Token);
        }

        public abstract void FromBytes(byte[] data);

        public override StackPacket Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
