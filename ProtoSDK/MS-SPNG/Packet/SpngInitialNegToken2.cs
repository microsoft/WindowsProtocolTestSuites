// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngInitialNegToken2: SpngPacket
    {
        /// <summary>
        /// Property of MechToken that contains in the inner Asn.1 NegTokenInit2 payload
        /// </summary>
        public byte[] MechToken
        {
            get
            {
                byte[] retToken = null;
                InitialNegToken2 initToken2 = this.Asn1Token as InitialNegToken2;
                NegotiationToken2 negToken2 = initToken2.negToken;

                switch (negToken2.SelectedChoice)
                {
                    case NegotiationToken2.negTokenInit2:
                        NegTokenInit2 negInit2 = (NegTokenInit2)negToken2.GetData();
                        if (negInit2.mechToken != null)
                        {
                            retToken = negInit2.mechToken.ByteArrayValue;
                        }
                        break;

                    default:
                        retToken = null;
                        break;
                }

                return retToken;
            }
        }

        /// <summary>
        /// Property of MechType list that contains in the inner Asn.1 NegTokenInit2 payload
        /// </summary>
        public MechTypeList MechList
        {
            get
            {
                MechTypeList retList;
                InitialNegToken2 initToken2 = this.Asn1Token as InitialNegToken2;
                NegotiationToken2 negToken2 = initToken2.negToken;

                switch (negToken2.SelectedChoice)
                {
                    case NegotiationToken2.negTokenInit2:
                        NegTokenInit2 negInit2 = (NegTokenInit2)negToken2.GetData();
                        retList = negInit2.mechTypes;
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
        /// Property of MechListMIC that contains in the inner Asn.1 NegTokenInit2 payload
        /// </summary>
        public byte[] MechListMIC
        {
            get
            {
                byte[] retMechListMIC;
                InitialNegToken2 initToken2 = this.Asn1Token as InitialNegToken2;
                NegotiationToken2 negToken2 = initToken2.negToken;

                switch (negToken2.SelectedChoice)
                {
                    case NegotiationToken2.negTokenInit2:
                        NegTokenInit2 negInit2 = (NegTokenInit2)negToken2.GetData();
                        retMechListMIC = negInit2.mechListMIC.ByteArrayValue;
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
        [CLSCompliant(false)]
        public SpngInitialNegToken2(InitialNegToken2 token)
            : base(token)
        {
        }


        /// <summary>
        /// Constructor
        /// Generally used when decoding
        /// </summary>
        /// <param name="data">The raw data is corresponding to the Asn.1 formatted token
        /// Generally used as the constructor parameter when decoding the Asn.1 type</param>
        public SpngInitialNegToken2()
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
            this.Asn1Token = SpngUtility.DecodeAsn1<InitialNegToken2>(buffer);
        }
    }
}
