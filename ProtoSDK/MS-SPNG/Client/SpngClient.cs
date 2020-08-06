// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngClient : SpngRole
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">configuration parameter</param>
        public SpngClient(SpngClientSecurityConfig config)
            : base(config)
        {
            this.Context = new SpngClientContext();
            this.Context.NegotiationState = SpngNegotiationState.Initial;
        }


        #region PacketAPI
        /// <summary>
        /// SPNG Client Packet API, which is to generate SpngInitialNegToken
        /// </summary>
        /// <param name="payloadType">An enum parameter, indicates the inner payload type, 
        /// e.g. PayloadType.NegInit is for NegTokenInit</param>
        /// <param name="negotiationState">Indicate the negotation status, e.g. incomplete, completed.</param>
        /// <param name="mechToken">Binary mechToken byte array, or responseToken. 
        /// This field could be null when the first time invoked.</param>
        /// <param name="mechListMIC">This field, if present, contains an MIC token for 
        /// the mechanism list in the initial negotiation message.
        /// This field could be null.</param>
        /// <returns>SpngInitialNegToken2 packet</returns>
        /// <exception cref="ArgumentOutOfRangeException">PayloadType is not in scope. </exception>
        public SpngInitialNegToken CreateInitialNegToken(SpngPayloadType payloadType,
            NegState negotiationState, byte[] mechToken, byte[] mechListMIC)
        {
            Asn1Object asn1Element;
            NegotiationToken negToken;
            Asn1OctetString asn10MechListMic = null;
            if (mechListMIC != null)
            {
                asn10MechListMic = new Asn1OctetString(mechListMIC);
            }

            switch (payloadType)
            {
                case SpngPayloadType.NegInit:
                    asn1Element = new NegTokenInit(this.Config.MechList,
                        this.Config.Asn1ContextAttributes, 
                        new Asn1.Asn1OctetString(mechToken),
                        asn10MechListMic
                    );
                    // save the initial MechType list. Will be used when computing MechListMIC
                    this.Context.InitMechTypeList = (asn1Element as NegTokenInit).mechTypes;
                    negToken = new NegotiationToken(NegotiationToken.negTokenInit, asn1Element);
                    break;

                default:
                    //throw exception here
                    throw new ArgumentOutOfRangeException("payloadType");
            }

            InitialNegToken intialToken = new InitialNegToken(new MechType(this.Config.SpngOidIntArray), negToken);

            return new SpngInitialNegToken(intialToken);
        }


        /// <summary>
        /// SPNG Client Packet API, which is to generate SpngNegTokenInit.
        /// </summary>
        /// <param name="mechToken">Byte array of mechToken. 
        /// This field could be null when the first time invoked.</param>
        /// <param name="mechListMIC">This field, if present, contains an MIC token for 
        /// the mechanism list in the initial negotiation message.
        /// This field could be null.</param>
        /// <returns>SpngNegTokenInit packet</returns>
        public SpngNegTokenInit CreateNegTokenInit(byte[] mechToken, byte[] mechListMIC)
        {
            NegTokenInit negToken = new NegTokenInit(this.Config.MechList,
                this.Config.Asn1ContextAttributes,
                new Asn1.Asn1OctetString(mechToken),
                new Asn1.Asn1OctetString(mechListMIC)
            );

            SpngNegTokenInit spngToken = new SpngNegTokenInit(negToken);

            return spngToken;
        }
        #endregion 


        /// <summary>
        /// Determine the local negotiated MechType, based on the MechTypeList from Server.
        /// This function is used when server proposes a "MechTypeList" field
        /// in NegTokenInit2.
        /// </summary>
        /// <param name="serverMechList">Server supported MechTypeList</param>
        public void NegotiateMechType(MechTypeList serverMechList)
        {
            NegotiateMechType(this.Config.MechList, serverMechList);
        }
    }
}
