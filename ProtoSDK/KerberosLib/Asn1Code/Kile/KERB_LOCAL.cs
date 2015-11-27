// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KERB-LOCAL ::= OCTET STRING -- Implementation-specific data which MUST be
    -- ignored if Kerberos client is not local.

    */
    public class KERB_LOCAL : Asn1OctetString
    {
        
        public KERB_LOCAL()
            : base()
        {
        }
        
        public KERB_LOCAL(string val)
            : base(val)
        {
        }
    }
}

