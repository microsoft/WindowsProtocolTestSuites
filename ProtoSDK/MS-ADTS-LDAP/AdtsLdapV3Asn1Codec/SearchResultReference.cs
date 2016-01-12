// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    SearchResultReference ::= [APPLICATION 19] SEQUENCE OF LDAPURL
    */
    [Asn1Tag(Asn1TagType.Application, 19)]
    public class SearchResultReference : Asn1SequenceOf<LDAPURL>
    {
        public SearchResultReference()
            : base()
        {
            this.Elements = null;
        }
        
        public SearchResultReference(LDAPURL[] val)
            : base(val)
        {
        }
    }
}

