// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngNegTokenInit2: SpngPacket
    {
        /// <summary>
        /// Constructor
        /// Generally used when encoding
        /// </summary>
        /// <param name="token">The Asn.1 formatted token contains in the class</param>
        public SpngNegTokenInit2(NegTokenInit2 token)
            : base(token)
        {
        }


        /// <summary>
        /// Constructor
        /// Generally used when decoding
        /// </summary>
        /// <param name="data">The raw data is corresponding to the Asn.1 formatted token
        /// Generally used as the constructor parameter when decoding the Asn.1 type</param>
        public SpngNegTokenInit2()
            : base()
        {
        }


        /// <summary>
        /// Decode the SPNG payload from the message bytes.
        /// Should be overridden in the inheriting classes
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        public override void FromBytes(byte[] buffer)
        {
            this.Asn1Token = SpngUtility.DecodeAsn1<NegTokenInit2>(buffer);
        }
    }
}
