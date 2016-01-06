// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    BindRequest ::=
    [APPLICATION 0] SEQUENCE {
         version        INTEGER (1 .. 127),
                        -- current version is 2
         name           LDAPDN,
                        -- null name implies an anonymous bind
         authentication BindRequest_authentication
}
    */
    [Asn1Tag(Asn1TagType.Application, 0)]
    public class BindRequest : Asn1Sequence
    {
        [Asn1Field(0), Asn1IntegerBound(Min = 1, Max = 127)]
        public Asn1Integer version { get; set; }
        
        [Asn1Field(1)]
        public LDAPDN name { get; set; }
        
        [Asn1Field(2)]
        public BindRequest_authentication authentication { get; set; }
        
        public BindRequest()
        {
            this.version = null;
            this.name = null;
            this.authentication = null;
        }
        
        public BindRequest(
         Asn1Integer version,
         LDAPDN name,
         BindRequest_authentication authentication)
        {
            this.version = version;
            this.name = name;
            this.authentication = authentication;
        }
    }
}

