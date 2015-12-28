// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    -- NOTE: AuthorizationData is always used as an OPTIONAL field and
    -- should not be empty.
    AuthorizationData       ::= SEQUENCE OF SEQUENCE {
            ad-type         [0] Int32,
            ad-data         [1] OCTET STRING
    }
    */
    public class AuthorizationData : Asn1SequenceOf<AuthorizationDataElement>
    {
        
        public AuthorizationData()
            : base()
        {
            this.Elements = null;
        }
        
        public AuthorizationData(AuthorizationDataElement[] val)
            : base(val)
        {
        }
    }
}

