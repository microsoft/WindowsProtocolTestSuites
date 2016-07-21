// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     SearchRequest ::= [APPLICATION 3] SEQUENCE {
                baseObject      LDAPDN,
                scope           SearchRequest_scope,
                derefAliases    SearchRequest_derefAliases,
                sizeLimit       INTEGER (0 .. maxInt),
                timeLimit       INTEGER (0 .. maxInt),
                typesOnly       BOOLEAN,
                filter          Filter,
                attributes      AttributeDescriptionList }
    */
    [Asn1Tag(Asn1TagType.Application, 3)]
    public class SearchRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN baseObject { get; set; }
        
        [Asn1Field(1)]
        public SearchRequest_scope scope { get; set; }
        
        [Asn1Field(2)]
        public SearchRequest_derefAliases derefAliases { get; set; }
        
		[Asn1IntegerBound(Max = 2147483647L, Min = 0)]
        [Asn1Field(3)]
        public Asn1Integer sizeLimit { get; set; }

        [Asn1IntegerBound(Max = 2147483647L, Min = 0)]
        [Asn1Field(4)]
        public Asn1Integer timeLimit { get; set; }
        
        [Asn1Field(5)]
        public Asn1Boolean typesOnly { get; set; }
        
        [Asn1Field(6)]
        public Filter filter { get; set; }
        
        [Asn1Field(7)]
        public AttributeDescriptionList attributes { get; set; }
        
        public SearchRequest()
        {
            this.baseObject = null;
            this.scope = null;
            this.derefAliases = null;
            this.sizeLimit = null;
            this.timeLimit = null;
            this.typesOnly = null;
            this.filter = null;
            this.attributes = null;
        }
        
        public SearchRequest(
         LDAPDN baseObject,
         SearchRequest_scope scope,
         SearchRequest_derefAliases derefAliases,
         Asn1Integer sizeLimit,
         Asn1Integer timeLimit,
         Asn1Boolean typesOnly,
         Filter filter,
         AttributeDescriptionList attributes)
        {
            this.baseObject = baseObject;
            this.scope = scope;
            this.derefAliases = derefAliases;
            this.sizeLimit = sizeLimit;
            this.timeLimit = timeLimit;
            this.typesOnly = typesOnly;
            this.filter = filter;
            this.attributes = attributes;
        }
    }
}

