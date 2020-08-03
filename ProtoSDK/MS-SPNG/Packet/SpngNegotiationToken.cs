// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngNegotiationToken: SpngPacket
    {
        /// <summary>
        /// Retrieve the byte-array formatted mechToken that contains in the inner payload
        /// </summary>
        public byte[] MechToken
        {
            get
            {
                Asn1OctetString asn1TokenString;

                NegotiationToken negToken = this.Asn1Token as NegotiationToken;

                switch (negToken.SelectedChoice)
                {
                    case NegotiationToken.negTokenInit:
                        NegTokenInit negInit = (NegTokenInit)negToken.GetData();
                        asn1TokenString = negInit.mechToken;
                        break;

                    case NegotiationToken.negTokenResp:
                        NegTokenResp negResp = (NegTokenResp)negToken.GetData();
                        asn1TokenString = negResp.responseToken;
                        break;

                    default:
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
        /// Retrieve the byte-array formatted MechListMIC that contains in the inner payload
        /// </summary>
        public byte[] MechListMIC
        {
            get
            {
                Asn1OctetString asn1TokenString;

                NegotiationToken negToken = this.Asn1Token as NegotiationToken;

                switch (negToken.SelectedChoice)
                {
                    case NegotiationToken.negTokenInit:
                        NegTokenInit negInit = (NegTokenInit)negToken.GetData();
                        asn1TokenString = negInit.mechListMIC;
                        break;

                    case NegotiationToken.negTokenResp:
                        NegTokenResp negResp = (NegTokenResp)negToken.GetData();
                        asn1TokenString = negResp.mechListMIC;
                        break;

                    default:
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
        /// Retrieve the Supported MechType that contains in the inner NegTokenResp only
        /// </summary>
        public MechType SupportedMechType
        {
            get
            {
                MechType mechType = null;

                NegotiationToken negToken = this.Asn1Token as NegotiationToken;

                switch (negToken.SelectedChoice)
                {
                    case NegotiationToken.negTokenResp:
                        NegTokenResp negResp = (NegTokenResp)negToken.GetData();
                        mechType = negResp.supportedMech;
                        break;

                    default:
                        // null
                        break;
                }

                return mechType;
            }
            set
            {
                NegotiationToken negToken = this.Asn1Token as NegotiationToken;

                switch (negToken.SelectedChoice)
                {
                    case NegotiationToken.negTokenResp:
                        NegTokenResp negResp = (NegTokenResp)negToken.GetData();
                        negResp.supportedMech = value;
                        break;

                    default:
                        // nothing to do
                        break;
                }
            }
        }


        /// <summary>
        /// Constructor
        /// Generally used when encoding
        /// </summary>
        /// <param name="token">The Asn.1 formatted token contains in the class</param>
        public SpngNegotiationToken(NegotiationToken token)
            : base(token)
        {
        }


        /// <summary>
        /// Constructor
        /// Generally used when decoding
        /// </summary>
        /// <param name="data">The raw data is corresponding to the Asn.1 formatted token
        /// Generally used as the constructor parameter when decoding the Asn.1 type</param>
        public SpngNegotiationToken()
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
            this.Asn1Token = SpngUtility.DecodeAsn1<NegotiationToken>(buffer);
        }
    }
}
