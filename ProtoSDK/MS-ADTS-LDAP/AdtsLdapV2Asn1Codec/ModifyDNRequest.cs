// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    ModifyDNRequest ::= [APPLICATION 12] SEQUENCE {
                entry           LDAPDN,
                newrdn          RelativeLDAPDN,
                deleteoldrdn    BOOLEAN,
                newSuperior     [0] LDAPDN OPTIONAL }
    */
    [Asn1Tag(Asn1TagType.Application, 12)]
    public class ModifyDNRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN entry { get; set; }
        
        [Asn1Field(1)]
        public RelativeLDAPDN newrdn { get; set; }
        
        [Asn1Field(2)]
        public Asn1Boolean deleteoldrdn { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 0)]
        public LDAPDN newSuperior { get; set; }
        
        public ModifyDNRequest()
        {
            this.entry = null;
            this.newrdn = null;
            this.deleteoldrdn = null;
            this.newSuperior = null;
        }
        
        public ModifyDNRequest(
         LDAPDN entry,
         RelativeLDAPDN newrdn,
         Asn1Boolean deleteoldrdn,
         LDAPDN newSuperior)
        {
            this.entry = entry;
            this.newrdn = newrdn;
            this.deleteoldrdn = deleteoldrdn;
            this.newSuperior = newSuperior;
        }
    }
}

