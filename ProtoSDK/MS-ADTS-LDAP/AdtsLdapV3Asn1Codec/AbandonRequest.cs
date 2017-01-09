// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    AbandonRequest ::= [APPLICATION 16] MessageID
    */
    [Asn1Tag(Asn1TagType.Application, 16, EncodingWay = EncodingWay.Primitive)]
    public class AbandonRequest : MessageID
    {
        public AbandonRequest()
            : base()
        {
        }

        public AbandonRequest(long? val)
            : base(val)
        {
        }


        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Encode " + this.GetType().Name + ".");
            }

            //Add the encoding result of Value to the front of buffer.
            int resultLen = ValueBerEncode(buffer);

            //Add the encoding result of Length to the front of buffer.
            resultLen += LengthBerEncode(buffer, resultLen);

            //Add the encoding result of the top most tag (in most cases it's Application Class Tag) to the front of buffer if it is defined.
            Asn1Tag topTag = this.TopTag;
            if (topTag.TagType != Asn1TagType.Universal)
            {
                //resultLen += LengthBerEncode(buffer, resultLen);
                resultLen += TagBerEncode(buffer, topTag);
            }
            return resultLen;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int returnVal = 0;
            //Decode the top most tag and universal class tag
            Asn1Tag topTag;
            returnVal += TagBerDecode(buffer, out topTag);

            Asn1Tag topTagInDefinition = this.TopTag;
            if (!topTag.Equals(topTagInDefinition))
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " Top Most Tag decoding fail.");
            }

            //Decode length
            int lengthAfterUniTag;
            returnVal += LengthBerDecode(buffer, out lengthAfterUniTag);
            //Decode data
            returnVal += ValueBerDecode(buffer, lengthAfterUniTag);

            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Decode " + this.GetType().Name + ".");
            }

            return returnVal;
        }
    }
}

