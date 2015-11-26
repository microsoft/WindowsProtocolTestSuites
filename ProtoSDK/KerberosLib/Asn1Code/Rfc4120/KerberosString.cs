// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KerberosString  ::= GeneralString (IA5String)
    */
    public class KerberosString : Asn1GeneralString
    {
        
        public KerberosString()
            : base()
        {
        }

        public KerberosString(string s)
            : base(s)
        {

        }
    }
}

