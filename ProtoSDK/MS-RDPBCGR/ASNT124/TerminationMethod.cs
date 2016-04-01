// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    TerminationMethod ::= ENUMERATED
    {
        automatic (0),
        manual (1),
        ...
    }
    */
    public class TerminationMethod : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long automatic = 0;
        
        [Asn1EnumeratedElement]
        public const long manual = 1;

        [Asn1Extension]
        public long? ext = null;
        
        public TerminationMethod()
            : base()
        {
        }

        public TerminationMethod(long val)
            : base(val)
        {
        }
    }
}

