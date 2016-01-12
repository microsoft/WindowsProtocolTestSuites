// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    ModifyRDNRequest ::=
    [APPLICATION 12] SEQUENCE {
         entry          LDAPDN,
         newrdn         RelativeLDAPDN -- old RDN always deleted
    }
    */
    [Asn1Tag(Asn1TagType.Application, 12)]
    public class ModifyRDNRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN entry { get; set; }
        
        [Asn1Field(1)]
        public RelativeLDAPDN newrdn { get; set; }
        
        public ModifyRDNRequest()
        {
            this.entry = null;
            this.newrdn = null;
        }
        
        public ModifyRDNRequest(
         LDAPDN entry,
         RelativeLDAPDN newrdn)
        {
            this.entry = entry;
            this.newrdn = newrdn;
        }
    }
}

