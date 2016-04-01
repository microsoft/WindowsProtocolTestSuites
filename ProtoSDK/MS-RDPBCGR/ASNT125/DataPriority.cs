// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    DataPriority ::= ENUMERATED
    {
        top (0),
        high (1),
        medium (2),
        low (3)
    }
    */
    public class DataPriority : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long top = 0;
        
        [Asn1EnumeratedElement]
        public const long high = 1;
        
        [Asn1EnumeratedElement]
        public const long medium = 2;
        
        [Asn1EnumeratedElement]
        public const long low = 3;
        
        public DataPriority()
            : base()
        {
        }
        
        public DataPriority(long val)
            : base(val)
        {
        }
    }
}

