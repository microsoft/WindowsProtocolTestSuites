// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KerberosTime    ::= GeneralizedTime -- with no fractional seconds
    */
    public class KerberosTime : Asn1GeneralizedTime
    {
        
        public KerberosTime()
            : base()
        {
        }
        public KerberosTime(string s)
            : base(s)
        {

        }

        public KerberosTime(string s, bool useDer)
            : base(s)
        {

        }

    }
}

