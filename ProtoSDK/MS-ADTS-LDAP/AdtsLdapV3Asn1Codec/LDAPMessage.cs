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
                allLength += controls.BerEncode(buffer, false);
                allLength += LengthBerEncode(buffer, allLength);
                allLength += TagBerEncode(buffer,
                    new Asn1Tag(Asn1TagType.Context, 0) { EncodingWay = EncodingWay.Constructed });
            }

            allLength += protocolOp.BerEncode(buffer, true);
            allLength += messageID.BerEncode(buffer, true);

            if (explicitTag)
            {
                allLength += LengthBerEncode(buffer, allLength);
                allLength += TagBerEncode(buffer,
                    new Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Sequence) { EncodingWay = EncodingWay.Constructed });
            }

            return allLength;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            Asn1Tag asn1Tag;
            int headLen = 0, valueLen = 0, tagLength = 0;

            if (explicitTag)
            {
                headLen += TagBerDecode(buffer, out asn1Tag);
                headLen += LengthBerDecode(buffer, out valueLen);
            }

            int valueLenDecode = 0;
            messageID = new MessageID();
            valueLenDecode += messageID.BerDecode(buffer, true);
            protocolOp = new LDAPMessage_protocolOp();
            valueLenDecode += protocolOp.BerDecode(buffer, true);

            asn1Tag = new Asn1Tag(Asn1TagType.Context, 0) { EncodingWay = EncodingWay.Constructed };
            if (IsTagMatch(buffer, asn1Tag, out tagLength, true))
            {
                controls = new Controls();
                valueLenDecode += controls.BerDecode(buffer, false);
            }
            valueLenDecode += tagLength;

            return headLen + valueLenDecode;
        }
    }
}

