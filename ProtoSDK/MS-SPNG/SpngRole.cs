// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public abstract class SpngRole
    {
        /// <summary>
        /// Spng configuration
        /// </summary>
        private SpngConfig config;

        /// <summary>
        /// SpngClientContext contains the stateful variables, e.g. negotiated MechType 
        /// </summary>
        private SpngContext context;

        /// <summary>
        /// SpngClientConfig is the constructor parameter of SpngClient,
        /// contains the configuration parameters.
        /// </summary>
        public SpngConfig Config
        {
            get
            {
                return this.config;
            }
            set
            {
                this.config = value;
            }
        }

        /// <summary>
        /// SpngClientContext contains the stateful variables, e.g. negotiated MechType
        /// </summary>
        public SpngContext Context
        {
            get
            {
                return this.context;
            }
            set
            {
                this.context = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">configuration parameter</param>
        protected SpngRole(SpngConfig config)
        {
            this.Config = config;
        }

        /// <summary>
        /// SPNG Client Packet API, which is to generate SpngNegotiationToken.
        /// </summary>
        /// <param name="payloadType">An enum parameter, indicates the inner payload type, 
        /// e.g. PayloadType.NegInit is for NegTokenInit</param>
        /// <param name="negotiationState">Indicate the negotation status, e.g. incomplete, completed.</param>
        /// <param name="responseToken">Byte array of responseToken. 
        /// This field could be null when the first time invoked.</param>
        /// <param name="mechListMIC">This field, if present, contains an MIC token for 
        /// the mechanism list in the initial negotiation message.
        /// This field could be null.</param>
        /// <returns>SpngNegotiationToken packet</returns>
        /// <exception cref="ArgumentOutOfRangeException">PayloadType is not in scope. </exception>
        public SpngNegotiationToken CreateNegotiationToken(
            SpngPayloadType payloadType, NegState negotiationState, byte[] responseToken, byte[] mechListMIC)
        {
            NegotiationToken token;

            switch (payloadType)
            {
                case SpngPayloadType.NegResp:
                    SpngNegTokenResp negResp = CreateNegTokenResp(negotiationState, responseToken, mechListMIC);
                    token = new NegotiationToken(NegotiationToken.negTokenResp, negResp.Asn1Token);
                    break;

                case SpngPayloadType.NegInit:
                    NegTokenInit negInit = new NegTokenInit(
                        this.config.MechList,
                        SpngUtility.ConvertUintToContextFlags(this.config.ContextAttributes),
                        new Asn1OctetString(responseToken),
                        new Asn1OctetString(mechListMIC)
                        );
                    token = new NegotiationToken(NegotiationToken.negTokenInit, negInit);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("payloadType");
            }

            return new SpngNegotiationToken(token);
        }

        /// <summary>
        /// SPNG Client Packet API, which is to generate SpngNegTokenResp.
        /// </summary>
        /// <param name="negotiationState">Indicate the negotation status, e.g. incomplete, completed.</param>
        /// <param name="responseToken">Byte array of responseToken. 
        /// This field could be null when the first time invoked.</param>
        /// <param name="mechListMIC">This field, if present, contains an MIC token for 
        /// the mechanism list in the initial negotiation message.
        /// This field could be null.</param>
        /// <returns>SpngNegTokenResp packet</returns>
        public SpngNegTokenResp CreateNegTokenResp(NegState negotiationState, byte[] responseToken, byte[] mechListMIC)
        {
            Asn1OctetString asn1RespToken = null;
            Asn1OctetString asn1MechListMIC = null;

            if (responseToken != null)
            {
                asn1RespToken = new Asn1OctetString(responseToken);
            }
            if (mechListMIC != null)
            {
                asn1MechListMIC = new Asn1OctetString(mechListMIC);
            }

            NegTokenResp negToken = new NegTokenResp(negotiationState,
                this.Context.NegotiatedMechType, asn1RespToken, asn1MechListMIC);

            SpngNegTokenResp spngToken = new SpngNegTokenResp(negToken);

            return spngToken;
        }

        /// <summary>
        /// Determine the local negotiated MechType, based on the MechTypeList from Server.
        /// This function is used when server proposes a "MechTypeList" field
        /// in NegTokenInit2.
        /// </summary>
        /// <param name="clientMechList">Client supported MechTypeList</param>
        /// <param name="serverMechList">Server supported MechTypeList</param>
        public void NegotiateMechType(MechTypeList clientMechList, MechTypeList serverMechList)
        {
            this.Context.InitMechTypeList = clientMechList;
            Negotiate(clientMechList, serverMechList);
        }

        /// <summary>
        /// Generate the MechListMIC based on MechType list of the current packet
        /// </summary>
        /// <param name="securityContext">Specific security context</param>
        /// <returns>MechListMIC</returns>
        internal byte[] GenerateMechListMIC(SecurityContext securityContext)
        {
            byte[] unsignedMechList = SpngUtility.EncodeAsn1(context.InitMechTypeList);

            return securityContext.Sign(unsignedMechList);
        }

        /// <summary>
        /// Verify the MechListMIC based on initial MechType list of the current packet
        /// </summary>
        /// <param name="securityContext">Specific security context</param>
        /// <param name="signature">Signature of the initial MechListMIC</param>
        /// <returns>True if the signature matches the signed message, otherwise false</returns>
        internal bool VerifyMechListMIC(SecurityContext securityContext, byte[] signature)
        {
            byte[] unsignedMechList = SpngUtility.EncodeAsn1(context.InitMechTypeList);

            return securityContext.Verify(unsignedMechList, signature);
        }

        /// <summary>
        /// Determine the negotiated MechType that supported by both sides
        /// </summary>
        /// <param name="localMechList">The local MechType list</param>
        /// <param name="remoteMechList">The remote MechType list</param>
        /// <exception cref="ApplicationException">Throw exception if negotiation failed</exception>
        private void Negotiate(MechTypeList localMechList, MechTypeList remoteMechList)
        {
            // remoteList is the first priority to compare
            for (int i = 0; i < remoteMechList.Elements.Length; i++)
            {
                // find in localList 
                for (int j = 0; j < localMechList.Elements.Length; j++)
                {
                    // if support current MechType
                    if (ArrayUtility.CompareArrays<int>(remoteMechList.Elements[i].Value,
                            localMechList.Elements[j].Value))
                    {
                        this.Context.NegotiatedMechType = localMechList.Elements[j];

                        if (i == 0)
                        {
                            // "i == 0" means it is the perferred MechType of remote endpoint
                            this.Context.NegotiationState = SpngNegotiationState.AcceptIncomplete;
                        }
                        else
                        {
                            this.Context.NegotiationState = SpngNegotiationState.RequestMic;
                        }
                        return;
                    }
                }
            }

            if (this.Context.NegotiatedMechType == null)
            {
                this.Context.NegotiationState = SpngNegotiationState.Reject;
            }
        }
    }
}
