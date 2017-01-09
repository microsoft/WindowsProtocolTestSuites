// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class UpdateStats_element : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPOID statID { get; set; }

        [Asn1Field(1)]
        public Asn1OctetString statValue { get; set; }

        public UpdateStats_element()
        {
            statID = null;
            statValue = null;
        }

        /// <summary>
        /// This constructor sets all elements to references to the 
        /// given objects
        /// </summary>
        public UpdateStats_element(LDAPOID statID_, Asn1OctetString statValue_)
        {
            statID = statID_;
            statValue = statValue_;
        }

        /// <summary>
        /// This constructor allows primitive data to be passed for all 
        /// primitive elements.  It will create new object wrappers for 
        /// the primitive data and set other elements to references to 
        /// the given objects 
        /// </summary>
        public UpdateStats_element(LDAPOID statID_, byte[] statValue_)
        {
            statID = statID_;
            statValue = new Asn1OctetString(statValue_);
        }
    }
}
