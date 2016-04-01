// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    Connect-Initial ::= [APPLICATION 101] IMPLICIT SEQUENCE
    {
        callingDomainSelector OCTET STRING,
        calledDomainSelector OCTET STRING,
        upwardFlag BOOLEAN,
        -- TRUE if called provider is higher
        targetParameters DomainParameters,
        minimumParameters DomainParameters,
        maximumParameters DomainParameters,
        userData OCTET STRING
    }
    */
    [Asn1Tag(Asn1TagType.Application, 101)]
    public class Connect_Initial : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1OctetString callingDomainSelector { get; set; }
        
        [Asn1Field(1)]
        public Asn1OctetString calledDomainSelector { get; set; }
        
        [Asn1Field(2)]
        public Asn1Boolean upwardFlag { get; set; }
        
        [Asn1Field(3)]
        public DomainParameters targetParameters { get; set; }
        
        [Asn1Field(4)]
        public DomainParameters minimumParameters { get; set; }
        
        [Asn1Field(5)]
        public DomainParameters maximumParameters { get; set; }
        
        [Asn1Field(6)]
        public Asn1OctetString userData { get; set; }
        
        public Connect_Initial()
        {
            this.callingDomainSelector = null;
            this.calledDomainSelector = null;
            this.upwardFlag = null;
            this.targetParameters = null;
            this.minimumParameters = null;
            this.maximumParameters = null;
            this.userData = null;
        }
        
        public Connect_Initial(
         Asn1OctetString callingDomainSelector,
         Asn1OctetString calledDomainSelector,
         Asn1Boolean upwardFlag,
         DomainParameters targetParameters,
         DomainParameters minimumParameters,
         DomainParameters maximumParameters,
         Asn1OctetString userData)
        {
            this.callingDomainSelector = callingDomainSelector;
            this.calledDomainSelector = calledDomainSelector;
            this.upwardFlag = upwardFlag;
            this.targetParameters = targetParameters;
            this.minimumParameters = minimumParameters;
            this.maximumParameters = maximumParameters;
            this.userData = userData;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            int length = ValueBerEncode(buffer);
            length += LengthBerEncode(buffer, length);
            buffer.WriteByte(101);
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

