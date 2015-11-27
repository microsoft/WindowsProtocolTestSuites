// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    SavedMechTypeList ::= MechTypeList
    */
    public class SavedMechTypeList : MechTypeList
    {
        public SavedMechTypeList()
            : base()
        {
            this.Elements = null;
        }

        public SavedMechTypeList(MechType[] val)
            : base(val)
        {
        }
    }
}

