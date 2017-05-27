// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    LDAPResult ::= SEQUENCE {
                resultCode      LDAPResult_resultCode,
                             -- 81-90 reserved for APIs --
                matchedDN       LDAPDN,
                errorMessage    LDAPString,
                referral        [3] Referral OPTIONAL }
    */
    public class LDAPResult : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPResult_resultCode resultCode { get; set; }
        
        [Asn1Field(1)]
        public LDAPDN matchedDN { get; set; }
        
        [Asn1Field(2)]
        public LDAPString errorMessage { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Referral referral { get; set; }
        
        public LDAPResult()
        {
            this.resultCode = null;
            this.matchedDN = null;
            this.errorMessage = null;
            this.referral = null;
        }
        
        public LDAPResult(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage,
         Referral referral)
        {
            this.resultCode = resultCode;
            this.matchedDN = matchedDN;
            this.errorMessage = errorMessage;
            this.referral = referral;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            int allLength = 0;

            if (referral != null)
            {
                allLength += referral.BerEncodeWithoutUnisersalTag(buffer);
                allLength += TagBerEncode(buffer, 
                    new Asn1Tag(Asn1TagType.Context, 3) { EncodingWay = EncodingWay.Constructed });
            }

            allLength += errorMessage.BerEncode(buffer);
            allLength += matchedDN.BerEncode(buffer);
            allLength += resultCode.BerEncode(buffer);
            allLength += LengthBerEncode(buffer, allLength);
            allLength += TagBerEncode(buffer,
                new Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Sequence) { EncodingWay = EncodingWay.Constructed });

            return allLength;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int headLen = 0;
            Asn1Tag seqTag;
            headLen += TagBerDecode(buffer, out seqTag);
            int valueLen;
            headLen += LengthBerDecode(buffer, out valueLen);

            int valueLenDecode = 0;
            resultCode = new LDAPResult_resultCode();
            valueLenDecode += resultCode.BerDecode(buffer);

            matchedDN = new LDAPDN();
            valueLenDecode += matchedDN.BerDecode(buffer);

            errorMessage = new LDAPString();
            valueLenDecode += errorMessage.BerDecode(buffer);
            if(valueLenDecode == valueLen)
            {
                referral = null;
            }
            else
            {
                Asn1Tag contextTag;
                valueLenDecode += TagBerDecode(buffer, out contextTag);
                referral = new Referral();
                valueLenDecode += referral.BerDecodeWithoutUnisersalTag(buffer);
            }
            if(valueLen != valueLenDecode)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " LDAPResult.");
            }
            return headLen + valueLen;
        }
    }
}

