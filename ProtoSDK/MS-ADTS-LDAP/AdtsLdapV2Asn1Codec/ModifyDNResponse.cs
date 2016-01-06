// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    ModifyDNResponse ::= [APPLICATION 13] LDAPResult
    */
    [Asn1Tag(Asn1TagType.Application, 13)]
    public class ModifyDNResponse : LDAPResult
    {
        public ModifyDNResponse()
            : base()
        {
        }

        public ModifyDNResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage) : base(resultCode, matchedDN, errorMessage)
        {
        }
    }
}

