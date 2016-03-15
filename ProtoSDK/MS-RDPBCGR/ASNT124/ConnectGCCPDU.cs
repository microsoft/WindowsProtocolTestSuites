// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    ConnectGCCPDU ::= CHOICE
    {
        conferenceCreateRequest ConferenceCreateRequest,
        conferenceCreateResponse ConferenceCreateResponse,
        conferenceQueryRequest ConferenceQueryRequest,
        conferenceQueryResponse ConferenceQueryResponse,
        conferenceJoinRequest ConferenceJoinRequest,
        conferenceJoinResponse ConferenceJoinResponse,
        conferenceInviteRequest ConferenceInviteRequest,
        conferenceInviteResponse ConferenceInviteResponse,
        ...
    }
    */
    public class ConnectGCCPDU : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long conferenceCreateRequest = 0;
        [Asn1ChoiceElement(conferenceCreateRequest)]
        protected ConferenceCreateRequest field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long conferenceCreateResponse = 1;
        [Asn1ChoiceElement(conferenceCreateResponse)]
        protected ConferenceCreateResponse field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long conferenceQueryRequest = 2;
        [Asn1ChoiceElement(conferenceQueryRequest)]
        protected ConferenceQueryRequest field2 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long conferenceQueryResponse = 3;
        [Asn1ChoiceElement(conferenceQueryResponse)]
        protected ConferenceQueryResponse field3 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long conferenceJoinRequest = 4;
        [Asn1ChoiceElement(conferenceJoinRequest)]
        protected ConferenceJoinRequest field4 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long conferenceJoinResponse = 5;
        [Asn1ChoiceElement(conferenceJoinResponse)]
        protected ConferenceJoinResponse field5 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long conferenceInviteRequest = 6;
        [Asn1ChoiceElement(conferenceInviteRequest)]
        protected ConferenceInviteRequest field6 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long conferenceInviteResponse = 7;
        [Asn1ChoiceElement(conferenceInviteResponse)]
        protected ConferenceInviteResponse field7 { get; set; }

        [Asn1Extension]
        public long? ext = null;

        public ConnectGCCPDU()
            : base()
        {
        }
        
        public ConnectGCCPDU(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

