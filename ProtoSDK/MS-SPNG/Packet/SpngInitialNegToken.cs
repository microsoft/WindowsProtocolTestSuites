// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngInitialNegToken : SpngPacket
    {
        /// <summary>
        /// Property of MechToken that contains in the inner Asn.1 NegTokenInit payload
        /// </summary>
        public byte[] MechToken
        {
            get
            {
                byte[] retToken;
                InitialNegToken initToken = this.Asn1Token as InitialNegToken;
                NegotiationToken negToken = initToken.negToken;
                
                switch (negToken.SelectedChoice)
                {
                    case NegotiationToken.negTokenInit:
                        NegTokenInit negInit = (NegTokenInit)negToken.GetData();
                        retToken = negInit.mechToken.ByteArrayValue;
                        break;

                    default:
                        retToken = null;
                        break;
                }

                return retToken;
            }
        }

        /// <summary>
        /// Property of MechType list that contains in the inner Asn.1 NegTokenInit payload
        /// </summary>
        public MechTypeList MechList
        {
            get
            {
                MechTypeList retList;
                InitialNegToken initToken = this.Asn1Token as InitialNegToken;
                NegotiationToken negToken = initToken.negToken;

                switch (negToken.SelectedChoice)
                {
                    case NegotiationToken.negTokenInit:
                        NegTokenInit negInit = (NegTokenInit)negToken.GetData();
                        retList = negInit.mechTypes;
                        break;

                    default:
                        // or throw exception here
                        retList = null;
                        break;
                }

                return retList;
            }
        }

        /// <summary>
        /// Property of MechListMIC that contains in the inner Asn.1 NegTokenInit payload
        /// </summary>
        public byte[] MechListMIC
        {
            get
            {
                byte[] retMechListMIC;
                InitialNegToken initToken = this.Asn1Token as InitialNegToken;
                NegotiationToken negToken = initToken.negToken;

                switch (negToken.SelectedChoice)
                {
                    case NegotiationToken.negTokenInit:
                        NegTokenInit negInit = (NegTokenInit)negToken.GetData();
                        retMechListMIC = negInit.mechListMIC.ByteArrayValue;
                        break;

                    default:
                        retMechListMIC = null;
                        break;
                }

                return retMechListMIC;
            }
        }


        /// <summary>
        /// Constructor
        /// Generally used when encoding
        /// </summary>
        /// <param name="token">The Asn.1 formatted token contains in the class</param>
        public SpngInitialNegToken(InitialNegToken token)
            : base(token)
        {
        }


        /// <summary>
        /// Constructor
        /// Generally used when decoding
        /// </summary>
        /// <param name="data">The raw data is corresponding to the Asn.1 formatted token
        /// Generally used as the constructor parameter when decoding the Asn.1 type</param>
        public SpngInitialNegToken()
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
            this.Asn1Token = SpngUtility.DecodeAsn1<InitialNegToken>(buffer);
        }
    }
}
