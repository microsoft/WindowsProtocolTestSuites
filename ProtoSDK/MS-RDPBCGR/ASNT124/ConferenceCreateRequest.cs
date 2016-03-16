// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    ConferenceCreateRequest ::= SEQUENCE
    { -- MCS-Connect-Provider request user data
        conferenceName ConferenceName,
        convenerPassword Password OPTIONAL,
        password Password OPTIONAL,
        lockedConference BOOLEAN,
        listedConference BOOLEAN,
        conductibleConference BOOLEAN,
        terminationMethod TerminationMethod,
        conductorPrivileges SET OF Privilege OPTIONAL,
        conductedPrivileges SET OF Privilege OPTIONAL,
        nonConductedPrivileges SET OF Privilege OPTIONAL,
        conferenceDescription TextString OPTIONAL,
        callerIdentifier TextString OPTIONAL,
        userData UserData OPTIONAL,
        ...,
        conferencePriority ConferencePriority OPTIONAL,
        conferenceMode ConferenceMode OPTIONAL
    }
    */
    public class ConferenceCreateRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public ConferenceName conferenceName { get; set; }

        [Asn1Field(1, Optional = true)]
        public Password convenerPassword { get; set; }

        [Asn1Field(2, Optional = true)]
        public Password password { get; set; }

        [Asn1Field(3)]
        public Asn1Boolean lockedConference { get; set; }

        [Asn1Field(4)]
        public Asn1Boolean listedConference { get; set; }

        [Asn1Field(5)]
        public Asn1Boolean conductibleConference { get; set; }

        [Asn1Field(6)]
        public TerminationMethod terminationMethod { get; set; }

        [Asn1Field(7, Optional = true)]
        public Asn1SetOf<Privilege> conductorPrivileges { get; set; }

        [Asn1Field(8, Optional = true)]
        public Asn1SetOf<Privilege> conductedPrivileges { get; set; }

        [Asn1Field(9, Optional = true)]
        public Asn1SetOf<Privilege> nonConductedPrivileges { get; set; }

        [Asn1Field(10, Optional = true)]
        public TextString conferenceDescription { get; set; }

        [Asn1Field(11, Optional = true)]
        public TextString callerIdentifier { get; set; }

        [Asn1Field(12, Optional = true)]
        public UserData userData { get; set; }

        [Asn1Extension]
        public long? ext = null;

        public ConferenceCreateRequest()
        {
            this.conferenceName = null;
            this.convenerPassword = null;
            this.password = null;
            this.lockedConference = null;
            this.listedConference = null;
            this.conductibleConference = null;
            this.terminationMethod = null;
            this.conductorPrivileges = null;
            this.conductedPrivileges = null;
            this.nonConductedPrivileges = null;
            this.conferenceDescription = null;
            this.callerIdentifier = null;
            this.userData = null;
        }

        public ConferenceCreateRequest(
         ConferenceName conferenceName,
         Password convenerPassword,
         Password password,
         Asn1Boolean lockedConference,
         Asn1Boolean listedConference,
         Asn1Boolean conductibleConference,
         TerminationMethod terminationMethod,
         Asn1SetOf<Privilege> conductorPrivileges,
         Asn1SetOf<Privilege> conductedPrivileges,
         Asn1SetOf<Privilege> nonConductedPrivileges,
         TextString conferenceDescription,
         TextString callerIdentifier,
         UserData userData)
        {
            this.conferenceName = conferenceName;
            this.convenerPassword = convenerPassword;
            this.password = password;
            this.lockedConference = lockedConference;
            this.listedConference = listedConference;
            this.conductibleConference = conductibleConference;
            this.terminationMethod = terminationMethod;
            this.conductorPrivileges = conductorPrivileges;
            this.conductedPrivileges = conductedPrivileges;
            this.nonConductedPrivileges = nonConductedPrivileges;
            this.conferenceDescription = conferenceDescription;
            this.callerIdentifier = callerIdentifier;
            this.userData = userData;
        }

        public override void PerEncode(IAsn1PerEncodingBuffer buffer)
        {
            base.PerEncode(buffer);
        }

        public override void PerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            base.PerDecode(buffer, aligned);
        }
    }
}

