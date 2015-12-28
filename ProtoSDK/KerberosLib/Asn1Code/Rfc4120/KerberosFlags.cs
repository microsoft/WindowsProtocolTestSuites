// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KerberosFlags   ::= BIT STRING (SIZE (32..MAX))
    */
    public class KerberosFlags : Asn1BitString
    {
        protected override bool VerifyConstraints()
        {
            return this.Length >= 32;
        }
        
        public KerberosFlags()
            : base()
        {
        }
        
        public KerberosFlags(string val)
            : base(val)
        {
        }
    }
}

