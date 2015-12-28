// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    Int32           ::= INTEGER (-2147483648..2147483647)
    */
    public class KerbInt32 : Asn1Integer
    {
        protected override bool VerifyConstraints()
        {
            return this.Value >=-2147483648 && this.Value <= 2147483647;
        }

        public KerbInt32()
            : base()
        {
        }
        
        public KerbInt32(long? val)
            : base(val)
        {
        }
    }
}

