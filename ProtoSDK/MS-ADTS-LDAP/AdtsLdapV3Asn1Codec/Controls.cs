// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     Controls ::= SEQUENCE OF Control
    */
    public class Controls : Asn1SequenceOf<Control>
    {
        public Controls()
            : base()
        {
            this.Elements = null;
        }
        
        public Controls(Control[] val)
            : base(val)
        {
        }
    }
}

