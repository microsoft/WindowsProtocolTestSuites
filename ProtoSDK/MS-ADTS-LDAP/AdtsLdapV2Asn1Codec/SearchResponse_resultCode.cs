// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    SearchResponse_resultCode ::= [APPLICATION 5] LDAPResult
    */
    [Asn1Tag(Asn1TagType.Application, 5)]
    public class SearchResponse_resultCode : LDAPResult
    {
        public SearchResponse_resultCode()
            : base()
        {
        }
        
    }
}

