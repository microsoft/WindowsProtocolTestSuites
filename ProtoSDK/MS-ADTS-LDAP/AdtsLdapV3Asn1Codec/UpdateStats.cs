// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class UpdateStats : Asn1SequenceOf<UpdateStats_element>
    {
        public UpdateStats()
        {
            this.Elements = null;
        }

        public UpdateStats(UpdateStats_element[] val)
            : base(val)
        {
        }
    }
}
