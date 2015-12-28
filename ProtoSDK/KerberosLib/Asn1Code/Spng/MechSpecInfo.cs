// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    MechSpecInfo ::= OCTET STRING
    */
    public class MechSpecInfo : Asn1OctetString
    {
        public MechSpecInfo()
            : base()
        {
        }
        
        public MechSpecInfo(string val)
            : base(val)
        {
        }

        public MechSpecInfo(byte[] val)
            : base(val)
        {

        }
    }
}

