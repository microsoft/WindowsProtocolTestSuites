// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    ETYPE-INFO              ::= SEQUENCE OF ETYPE-INFO-ENTRY
    */
    public class ETYPE_INFO : Asn1SequenceOf<ETYPE_INFO_ENTRY>
    {
        
        public ETYPE_INFO()
            : base()
        {
            this.Elements = null;
        }
        
        public ETYPE_INFO(ETYPE_INFO_ENTRY[] val)
            : base(val)
        {
        }
    }
}

