// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    AttachUserRequest ::= [APPLICATION 10] IMPLICIT SEQUENCE
    {
    }
    */
    [Asn1Tag(Asn1TagType.Application, 10)]
    public class AttachUserRequest : Asn1Sequence
    {
        public AttachUserRequest()
        {
        }
    }
}

