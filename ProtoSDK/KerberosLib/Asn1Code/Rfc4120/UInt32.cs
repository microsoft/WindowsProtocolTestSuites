// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    UInt32          ::= INTEGER (0..4294967295)
    */
    public class KerbUInt32 : Asn1Integer
    {
        protected override bool VerifyConstraints()
        {
            return this.Value >= 0 && this.Value <= 4294967295;
        }
        
        public KerbUInt32()
            : base()
        {
        }
        
        public KerbUInt32(long val)
            : base(val)
        {
        }
    }
}

