// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    METHOD-DATA     ::= SEQUENCE OF PA-DATA

    */
    public class METHOD_DATA : Asn1SequenceOf<PA_DATA>
    {
        
        public METHOD_DATA()
            : base()
        {
            this.Elements = null;
        }
        
        public METHOD_DATA(PA_DATA[] val)
            : base(val)
        {
        }
    }
}

