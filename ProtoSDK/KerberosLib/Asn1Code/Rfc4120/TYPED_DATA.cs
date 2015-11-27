// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    TYPED-DATA      ::= SEQUENCE SIZE (1..MAX) OF SEQUENCE {
        data-type       [0] Int32,
        data-value      [1] OCTET STRING OPTIONAL
    }
    */
    public class TYPED_DATA : Asn1SequenceOf<TYPED_DATAElement>
    {
        
        public TYPED_DATA()
            : base()
        {
            this.Elements = null;
        }
        
        public TYPED_DATA(TYPED_DATAElement[] val)
            : base(val)
        {
        }

        protected override bool VerifyConstraints()
        {
            return this.Elements != null && this.Elements.Length >= 1;
        }
    }
}

