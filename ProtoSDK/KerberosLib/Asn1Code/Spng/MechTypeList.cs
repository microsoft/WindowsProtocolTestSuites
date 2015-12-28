// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    MechTypeList ::= SEQUENCE OF MechType
    */
    public class MechTypeList : Asn1SequenceOf<MechType>
    {
        public MechTypeList()
            : base()
        {
            this.Elements = null;
        }
        
        public MechTypeList(MechType[] val)
            : base(val)
        {
        }
    }
}

