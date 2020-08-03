// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngNegotiationToken2: SpngPacket
    {
        /// <summary>
        /// Retrieve the byte-array formatted mechToken that contains in the inner payload
        /// </summary>
        public byte[] MechToken
        {
            get
            {
                Asn1OctetString asn1TokenString;

                NegotiationToken2 negToken2 = this.Asn1Token as NegotiationToken2;

                switch (negToken2.SelectedChoice)
                {
                    case NegotiationToken2.negTokenInit2:
                        NegTokenInit2 negInit2 = (NegTokenInit2)negToken2.GetData();
                        asn1TokenString = negInit2.mechToken;
                        break;

                    default:
                        // throw exception here
                        asn1TokenString = null;
                        break;
                }

                if (asn1TokenString != null)
                {
                    return asn1TokenString.ByteArrayValue;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Constructor
        /// Generally used when encoding
        /// </summary>
        /// <param name="token">The Asn.1 formatted token contains in the class</param>
        public SpngNegotiationToken2(NegotiationToken2 token)
            : base(token)
        {
        }


        /// <summary>
        /// Constructor
        /// Generally used when decoding
        /// </summary>
        /// <param name="data">The raw data is corresponding to the Asn.1 formatted token
        /// Generally used as the constructor parameter when decoding the Asn.1 type</param>
        public SpngNegotiationToken2()
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
            this.Asn1Token = SpngUtility.DecodeAsn1<NegotiationToken2>(buffer);
        }
    }
}
