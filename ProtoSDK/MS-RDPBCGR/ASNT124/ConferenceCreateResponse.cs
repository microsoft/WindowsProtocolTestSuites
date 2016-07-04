// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    ConferenceCreateResponse ::= SEQUENCE
    { -- MCS-Connect-Provider response user data
        nodeID UserID, -- Node ID of the sending node
        tag INTEGER,
        result ENUMERATED
            {
                success (0),
                userRejected (1),
                resourcesNotAvailable (2),
                rejectedForSymmetryBreaking (3),
                lockedConferenceNotSupported (4),
                ...
            },
        userData UserData OPTIONAL,
        ...
    }
    */
    public class ConferenceCreateResponse : Asn1Sequence
    {
        [Asn1Field(0)]
        public UserID nodeID { get; set; }

        [Asn1Field(1)]
        public Asn1Integer tag { get; set; }

        [Asn1Field(2)]
        public ConferenceCreateResponse_result result { get; set; }

        [Asn1Field(3, Optional = true)]
        public UserData userData { get; set; }

        [Asn1Extension]
        public long? ext = null;

        public ConferenceCreateResponse()
        {
            this.nodeID = null;
            this.tag = null;
            this.result = null;
            this.userData = null;
        }

        public ConferenceCreateResponse(
         UserID nodeID,
         Asn1Integer tag,
         ConferenceCreateResponse_result result,
         UserData userData)
        {
            this.nodeID = nodeID;
            this.tag = tag;
            this.result = result;
            this.userData = userData;
        }
    }

    public class ConferenceCreateResponse_result : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long success = 0;
        
        [Asn1EnumeratedElement]
        public const long userRejected = 1;
        
        [Asn1EnumeratedElement]
        public const long resourcesNotAvailable = 2;
        
        [Asn1EnumeratedElement]
        public const long rejectedForSymmetryBreaking = 3;
        
        [Asn1EnumeratedElement]
        public const long lockedConferenceNotSupported = 4;

        [Asn1Extension]
        public long? ext = null;
        
        public ConferenceCreateResponse_result()
            : base()
        {
        }

        public ConferenceCreateResponse_result(long val)
            : base(val)
        {
        }
    }
}

