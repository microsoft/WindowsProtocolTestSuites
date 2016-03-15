// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    Privilege ::= ENUMERATED
    {
        terminate (0),
        ejectUser (1),
        add (2),
        lockUnlock (3),
        transfer (4),
        ...
    }
    */
    public class Privilege : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long terminate = 0;
        
        [Asn1EnumeratedElement]
        public const long ejectUser = 1;
        
        [Asn1EnumeratedElement]
        public const long add = 2;
        
        [Asn1EnumeratedElement]
        public const long lockUnlock = 3;
        
        [Asn1EnumeratedElement]
        public const long transfer = 4;

        [Asn1Extension]
        public long? ext = null;
        
        public Privilege()
            : base()
        {
        }

        public Privilege(long val)
            : base(val)
        {
        }
    }
}

