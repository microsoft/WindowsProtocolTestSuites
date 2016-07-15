// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    LDAPMessage_protocolOp ::= CHOICE {
                             searchRequest       SearchRequest,
                             searchResponse      SearchResponse,
                             modifyRequest       ModifyRequest,
                             modifyResponse      ModifyResponse,
                             addRequest          AddRequest,
                             addResponse         AddResponse,
                             delRequest          DelRequest,
                             delResponse         DelResponse,
                             modifyDNRequest     ModifyDNRequest,
                             modifyDNResponse    ModifyDNResponse,
                             compareDNRequest    CompareRequest,
                             compareDNResponse   CompareResponse,
                             bindRequest         BindRequest,
                             bindResponse        BindResponse,
                             abandonRequest      AbandonRequest,
                             unbindRequest       UnbindRequest,
                             sicilyResponse      SicilyBindResponse
                        }
    */
    public class LDAPMessage_protocolOp : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long searchRequest = 0;
        [Asn1ChoiceElement(searchRequest)]
        protected SearchRequest field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long searchResponse = 1;
        [Asn1ChoiceElement(searchResponse)]
        protected SearchResponse field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long modifyRequest = 2;
        [Asn1ChoiceElement(modifyRequest)]
        protected ModifyRequest field2 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long modifyResponse = 3;
        [Asn1ChoiceElement(modifyResponse)]
        protected ModifyResponse field3 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long addRequest = 4;
        [Asn1ChoiceElement(addRequest)]
        protected AddRequest field4 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long addResponse = 5;
        [Asn1ChoiceElement(addResponse)]
        protected AddResponse field5 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long delRequest = 6;
        [Asn1ChoiceElement(delRequest)]
        protected DelRequest field6 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long delResponse = 7;
        [Asn1ChoiceElement(delResponse)]
        protected DelResponse field7 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long modifyDNRequest = 8;
        [Asn1ChoiceElement(modifyDNRequest)]
        protected ModifyDNRequest field8 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long modifyDNResponse = 9;
        [Asn1ChoiceElement(modifyDNResponse)]
        protected ModifyDNResponse field9 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long compareDNRequest = 10;
        [Asn1ChoiceElement(compareDNRequest)]
        protected CompareRequest field10 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long compareDNResponse = 11;
        [Asn1ChoiceElement(compareDNResponse)]
        protected CompareResponse field11 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long bindRequest = 12;
        [Asn1ChoiceElement(bindRequest)]
        protected BindRequest field12 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long bindResponse = 13;
        [Asn1ChoiceElement(bindResponse)]
        protected BindResponse field13 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long abandonRequest = 14;
        [Asn1ChoiceElement(abandonRequest)]
        protected AbandonRequest field14 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long unbindRequest = 15;
        [Asn1ChoiceElement(unbindRequest)]
        protected UnbindRequest field15 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sicilyResponse = 16;
        [Asn1ChoiceElement(sicilyResponse)]
        protected SicilyBindResponse field16 { get; set; }
        
        public LDAPMessage_protocolOp()
            : base()
        {
        }
        
        public LDAPMessage_protocolOp(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

