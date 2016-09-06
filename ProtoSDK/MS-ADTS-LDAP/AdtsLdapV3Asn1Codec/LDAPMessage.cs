// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    LDAPMessage ::= SEQUENCE {
                messageID       MessageID,
                protocolOp      LDAPMessage_protocolOp,
                 controls       [0] Controls OPTIONAL }
    */
    public class LDAPMessage : Asn1Sequence
    {
        [Asn1Field(0)]
        public MessageID messageID { get; set; }
        
        [Asn1Field(1)]
        public LDAPMessage_protocolOp protocolOp { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 0)]
        public Controls controls { get; set; }
        
        public LDAPMessage()
        {
            this.messageID = null;
            this.protocolOp = null;
            this.controls = null;
        }
        
        public LDAPMessage(
         MessageID messageID,
         LDAPMessage_protocolOp protocolOp,
         Controls controls)
        {
            this.messageID = messageID;
            this.protocolOp = protocolOp;
            this.controls = controls;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            int allLength = 0;

            if (controls != null)
            {
                allLength += controls.BerEncodeWithoutUnisersalTag(buffer);
                allLength += TagBerEncode(buffer,
                    new Asn1Tag(Asn1TagType.Context, 0) { EncodingWay = EncodingWay.Constructed });
            }

            allLength += protocolOp.BerEncode(buffer);
            allLength += messageID.BerEncode(buffer);
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
            messageID = new MessageID();
            valueLenDecode += messageID.BerDecode(buffer);
            protocolOp = new LDAPMessage_protocolOp();
            valueLenDecode += protocolOp.BerDecode(buffer);
            if (valueLenDecode == valueLen)
            {
                controls = null;
            }
            else
            {
                Asn1Tag contextTag;
                valueLenDecode += TagBerDecode(buffer, out contextTag);
                controls = new Controls();
                valueLenDecode += controls.BerDecodeWithoutUnisersalTag(buffer);
            }
            if (valueLen != valueLenDecode)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " LDAPResult.");
            }
            return headLen + valueLen;
        }
    }
}

