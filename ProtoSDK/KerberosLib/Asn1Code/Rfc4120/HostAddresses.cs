// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    HostAddresses   -- NOTE: subtly different from rfc1510,
    -- but has a value mapping and encodes the same
            ::= SEQUENCE OF HostAddress
    */
    public class HostAddresses : Asn1SequenceOf<HostAddress>
    {
        
        public HostAddresses()
            : base()
        {
            this.Elements = null;
        }
        
        public HostAddresses(HostAddress[] val)
            : base(val)
        {
        }
    }
}

