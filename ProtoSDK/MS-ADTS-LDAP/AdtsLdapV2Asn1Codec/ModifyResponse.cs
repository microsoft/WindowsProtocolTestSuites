// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    ModifyResponse ::= [APPLICATION 7] LDAPResult
    */
    [Asn1Tag(Asn1TagType.Application, 7)]
    public class ModifyResponse : LDAPResult
    {
        public ModifyResponse()
            : base()
        {
        }

        public ModifyResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage) : base(resultCode, matchedDN, errorMessage)
        {
        }
    }
}

