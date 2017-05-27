// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    ModifyResponse ::= [APPLICATION 7] LDAPResult
    */
    [Asn1Tag(Asn1TagType.Application, 7)]
    public class ModifyResponse : LDAPResult
    {
        public ModifyResponse()
            : base()
        {
        }

        public ModifyResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage,
         Referral referral) : base (resultCode, matchedDN, errorMessage, referral)
        {
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
            allLength += TagBerEncode(buffer,this.TopTag);

            return allLength;
        }
    }
}

