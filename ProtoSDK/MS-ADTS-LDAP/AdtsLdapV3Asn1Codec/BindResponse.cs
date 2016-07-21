// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    BindResponse ::= [APPLICATION 1] SEQUENCE {
		COMPONENTS OF LDAPResult,
        serverSaslCreds    [7] OCTET STRING OPTIONAL }
    */
    [Asn1Tag(Asn1TagType.Application, 1)]
    public class BindResponse : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPResult_resultCode resultCode { get; set; }
        
        [Asn1Field(1)]
        public LDAPDN matchedDN { get; set; }
        
        [Asn1Field(2)]
        public LDAPString errorMessage { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Referral referral { get; set; }
        
        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 7)]
        public Asn1OctetString serverSaslCreds { get; set; }
        
        public BindResponse()
        {
            this.resultCode = null;
            this.matchedDN = null;
            this.errorMessage = null;
            this.referral = null;
            this.serverSaslCreds = null;
        }
        
        public BindResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage,
         Referral referral,
         Asn1OctetString serverSaslCreds)
        {
            this.resultCode = resultCode;
            this.matchedDN = matchedDN;
            this.errorMessage = errorMessage;
            this.referral = referral;
            this.serverSaslCreds = serverSaslCreds;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            int allLength = 0;

            if(serverSaslCreds != null)
            {
                allLength += serverSaslCreds.BerEncodeWithoutUnisersalTag(buffer);
                allLength += TagBerEncode(buffer,
                    new Asn1Tag(Asn1TagType.Context, 7) { EncodingWay = EncodingWay.Primitive });
            }

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
                new Asn1Tag(Asn1TagType.Application, 1) { EncodingWay = EncodingWay.Constructed });

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
            if (valueLenDecode == valueLen)
            {
                referral = null;
                serverSaslCreds = null;
            }
            else
            {
                Asn1Tag contextTag;
                valueLenDecode += TagBerDecode(buffer, out contextTag);
                if (contextTag.TagValue == 3)
                {
                    //referral
                    referral = new Referral();
                    valueLenDecode += referral.BerDecodeWithoutUnisersalTag(buffer);
                    if (valueLenDecode < valueLen)
                    {
                        valueLenDecode += TagBerDecode(buffer, out contextTag);
                    }
                }
                if (contextTag.TagValue == 7)
                {
                    //serverSaslCreds
                    serverSaslCreds = new Asn1OctetString();
                    valueLenDecode += serverSaslCreds.BerDecodeWithoutUnisersalTag(buffer);
                }
           }
            if (valueLen != valueLenDecode)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " BindResponse.");
            }
            return headLen + valueLen;
        }

    }
}

