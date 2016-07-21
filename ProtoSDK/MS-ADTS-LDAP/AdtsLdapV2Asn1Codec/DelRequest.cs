// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    DelRequest ::= [APPLICATION 10] LDAPDN
    */
    [Asn1Tag(Asn1TagType.Application, 10)]
    public class DelRequest : LDAPDN
    {
        public DelRequest()
            : base()
        {
        }

        public DelRequest(string val)
            : base(val)
        {
        }
    }
}

