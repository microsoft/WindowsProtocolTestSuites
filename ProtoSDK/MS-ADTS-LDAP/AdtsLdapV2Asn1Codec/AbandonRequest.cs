// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    AbandonRequest ::= [APPLICATION 16] MessageID
    */
    [Asn1Tag(Asn1TagType.Application, 16)]
    public class AbandonRequest : MessageID
    {
        public AbandonRequest()
            : base()
        {
        }

        public AbandonRequest(long? val)
            : base(val)
        {
        }
    }
}

