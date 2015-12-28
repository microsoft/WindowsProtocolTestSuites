// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    FastOptions ::= KerberosFlags
          -- reserved(0),
          -- hide-client-names(1),
          -- kdc-follow-referrals(16)
    */
    public class FastOptions : KerberosFlags
    {
        
        public FastOptions()
            : base()
        {
        }

        public FastOptions(string val)
            : base(val)
        {
        }
    }
}

