// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    MessageID ::= INTEGER (0 .. maxInt)
    */
    [Asn1IntegerBound(Min = 1, Max = 65535L)]
    public class MessageID : Asn1Integer
    {
        public MessageID()
            : base()
        {
        }
        
        public MessageID(long? val)
            : base(val)
        {
        }
    }
}

