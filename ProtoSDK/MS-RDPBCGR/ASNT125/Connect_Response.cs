// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    Connect-Response ::= [APPLICATION 102] IMPLICIT SEQUENCE
    {
        result Result,
        calledConnectId INTEGER (0..MAX),
        -- assigned by the called provider
        -- to identify additional TCs of
        -- the same MCS connection
        domainParameters DomainParameters,
        userData OCTET STRING
    }
    */
    [Asn1Tag(Asn1TagType.Application, 102)]
    public class Connect_Response : Asn1Sequence
    {
        [Asn1Field(0)]
        public Result result { get; set; }
        
        [Asn1Field(1), Asn1IntegerBound(Min = 0)]
        public Asn1Integer calledConnectId { get; set; }
        
        [Asn1Field(2)]
        public DomainParameters domainParameters { get; set; }
        
        [Asn1Field(3)]
        public Asn1OctetString userData { get; set; }
        
        public Connect_Response()
        {
            this.result = null;
            this.calledConnectId = null;
            this.domainParameters = null;
            this.userData = null;
        }
        
        public Connect_Response(
         Result result,
         Asn1Integer calledConnectId,
         DomainParameters domainParameters,
         Asn1OctetString userData)
        {
            this.result = result;
            this.calledConnectId = calledConnectId;
            this.domainParameters = domainParameters;
            this.userData = userData;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            int length = ValueBerEncode(buffer);
            length += LengthBerEncode(buffer, length);
            buffer.WriteByte(102);
            buffer.WriteByte(127);
            return length + 2;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            buffer.ReadBytes(2);
            int consumedLen = 2, len;
            consumedLen += LengthBerDecode(buffer, out len);
            return consumedLen + ValueBerDecode(buffer, len);
        }
    }
}

