// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    Microseconds    ::= INTEGER (0..999999)
    */
    public class Microseconds : Asn1Integer
    {
        protected override bool VerifyConstraints()
        {
            return this.Value >= 0 && this.Value <= 999999;
        }
        public Microseconds()
            : base()
        {
        }
        
        public Microseconds(long val)
            : base(val)
        {
        }
    }
}

