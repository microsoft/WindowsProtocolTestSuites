// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    ETYPE-INFO2             ::= SEQUENCE SIZE (1..MAX) OF ETYPE-INFO2-ENTRY
    */
    public class ETYPE_INFO2 : Asn1SequenceOf<ETYPE_INFO2_ENTRY>
    {
        
        public ETYPE_INFO2()
            : base()
        {
            this.Elements = null;
        }
        
        public ETYPE_INFO2(ETYPE_INFO2_ENTRY[] val)
            : base(val)
        {
        }

        protected override bool VerifyConstraints()
        {
            return this.Elements != null && this.Elements.Length >= 1;
        }
    }
}

