// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    MechType ::= OBJECT IDENTIFIER
       -- OID represents each security mechanism as suggested by
       -- [RFC2743]
    */
    public class MechType : Asn1ObjectIdentifier
    {
        public MechType()
            : base()
        {
        }

        public MechType(int[] value) :
            base(value)
        {
        }
    }
}