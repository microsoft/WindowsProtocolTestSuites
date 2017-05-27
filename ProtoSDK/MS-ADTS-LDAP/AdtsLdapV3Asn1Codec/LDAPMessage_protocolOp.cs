// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    LDAPMessage_protocolOp ::=     CHOICE {
                        bindRequest     BindRequest,
                        bindResponse    BindResponse,
                        sicilyResponse  SicilyBindResponse,
                        unbindRequest   UnbindRequest,
                        searchRequest   SearchRequest,
                        searchResEntry  SearchResultEntry,
                        searchResDone   SearchResultDone,
                        searchResRef    SearchResultReference,
                        modifyRequest   ModifyRequest,
                        modifyResponse  ModifyResponse,
                        addRequest      AddRequest,
                        addResponse     AddResponse,
                        delRequest      DelRequest,
                        delResponse     DelResponse,
                        modDNRequest    ModifyDNRequest,
                        modDNResponse   ModifyDNResponse,
                        compareRequest  CompareRequest,
                        compareResponse CompareResponse,
                        abandonRequest  AbandonRequest,
                        extendedReq     ExtendedRequest,
                        extendedResp    ExtendedResponse }
    */
    public class LDAPMessage_protocolOp : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long bindRequest = 1;
        [Asn1ChoiceElement(bindRequest)]
        protected BindRequest field0 { get; set; }

        [Asn1ChoiceIndex]
        public const long bindResponse = 2;
        [Asn1ChoiceElement(bindResponse)]
        protected BindResponse field1 { get; set; }

        [Asn1ChoiceIndex]
        public const long sicilybindResponse = 3;
        [Asn1ChoiceElement(sicilybindResponse)]
        protected SicilyBindResponse field2 { get; set; }

        [Asn1ChoiceIndex]
        public const long unbindRequest = 4;
        [Asn1ChoiceElement(unbindRequest)]
        protected UnbindRequest field3 { get; set; }

        [Asn1ChoiceIndex]
        public const long searchRequest = 5;
        [Asn1ChoiceElement(searchRequest)]
        protected SearchRequest field4 { get; set; }

        [Asn1ChoiceIndex]
        public const long searchResEntry = 6;
        [Asn1ChoiceElement(searchResEntry)]
        protected SearchResultEntry field5 { get; set; }

        [Asn1ChoiceIndex]
        public const long searchResDone = 7;
        [Asn1ChoiceElement(searchResDone)]
        protected SearchResultDone field6 { get; set; }

        [Asn1ChoiceIndex]
        public const long modifyRequest = 8;
        [Asn1ChoiceElement(modifyRequest)]
        protected ModifyRequest field7 { get; set; }

        [Asn1ChoiceIndex]
        public const long modifyResponse = 9;
        [Asn1ChoiceElement(modifyResponse)]
        protected ModifyResponse field8 { get; set; }

        [Asn1ChoiceIndex]
        public const long addRequest = 10;
        [Asn1ChoiceElement(addRequest)]
        protected AddRequest field9 { get; set; }

        [Asn1ChoiceIndex]
        public const long addResponse = 11;
        [Asn1ChoiceElement(addResponse)]
        protected AddResponse field10 { get; set; }

        [Asn1ChoiceIndex]
        public const long delRequest = 12;
        [Asn1ChoiceElement(delRequest)]
        protected DelRequest field11 { get; set; }

        [Asn1ChoiceIndex]
        public const long delResponse = 13;
        [Asn1ChoiceElement(delResponse)]
        protected DelResponse field12 { get; set; }

        [Asn1ChoiceIndex]
        public const long modDNRequest = 14;
        [Asn1ChoiceElement(modDNRequest)]
        protected ModifyDNRequest field13 { get; set; }

        [Asn1ChoiceIndex]
        public const long modDNResponse = 15;
        [Asn1ChoiceElement(modDNResponse)]
        protected ModifyDNResponse field14 { get; set; }

        [Asn1ChoiceIndex]
        public const long compareRequest = 16;
        [Asn1ChoiceElement(compareRequest)]
        protected CompareRequest field15 { get; set; }

        [Asn1ChoiceIndex]
        public const long compareResponse = 17;
        [Asn1ChoiceElement(compareResponse)]
        protected CompareResponse field16 { get; set; }

        [Asn1ChoiceIndex]
        public const long abandonRequest = 18;
        [Asn1ChoiceElement(abandonRequest)]
        protected AbandonRequest field17 { get; set; }

        [Asn1ChoiceIndex]
        public const long searchResRef = 19;
        [Asn1ChoiceElement(searchResRef)]
        protected SearchResultReference field18 { get; set; }

        [Asn1ChoiceIndex]
        public const long extendedReq = 20;
        [Asn1ChoiceElement(extendedReq)]
        protected ExtendedRequest field19 { get; set; }

        [Asn1ChoiceIndex]
        public const long extendedResp = 21;
        [Asn1ChoiceElement(extendedResp)]
        protected ExtendedResponse field20 { get; set; }

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

