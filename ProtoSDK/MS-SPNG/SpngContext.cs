// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public abstract class SpngContext
    {
        /// <summary>
        /// Negotiation State. e.g. accept_completed or accept_incomplete
        /// </summary>
        private SpngNegotiationState negotiationState;

        /// <summary>
        /// The MechType that is supported by both sides
        /// </summary>
        private MechType negotiatedMechType;

        /// <summary>
        /// The MechTypeList that is carried in NegTokenInit. 
        /// MechListMIC will be computed based on this list.
        /// </summary>
        private MechTypeList initMechTypeList;

        /// <summary>
        /// Negotiation State. e.g. accept_completed or accept_incomplete
        /// </summary>
        public SpngNegotiationState NegotiationState
        {
            get
            {
                return this.negotiationState;
            }
            set
            {
                this.negotiationState = value;
            }
        }

        /// <summary>
        /// The MechType that is supported by both sides
        /// </summary>
        public MechType NegotiatedMechType
        {
            get
            {
                return this.negotiatedMechType;
            }
            set
            {
                this.negotiatedMechType = value;
            }
        }

        /// <summary>
        /// The MechTypeList that is carried in NegTokenInit. 
        /// MechListMIC will be computed based on this list.
        /// </summary>
        public MechTypeList InitMechTypeList
        {
            get
            {
                return this.initMechTypeList;
            }
            set
            {
                this.initMechTypeList = value;
            }
        }
    }
}
