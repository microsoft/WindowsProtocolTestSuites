// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    Referral ::= SEQUENCE OF LDAPURL
    */
    public class Referral : Asn1SequenceOf<LDAPURL>
    {
        public Referral()
            : base()
        {
            this.Elements = null;
        }
        
        public Referral(LDAPURL[] val)
            : base(val)
        {
        }
    }
}

