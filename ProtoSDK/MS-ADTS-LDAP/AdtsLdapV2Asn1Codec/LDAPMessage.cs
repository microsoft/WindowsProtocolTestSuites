// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    LDAPMessage ::=
    SEQUENCE {
         messageID      MessageID,
                        -- unique id in request,
                        -- to be echoed in response(s)
         protocolOp     LDAPMessage_protocolOp
    }
    */
    public class LDAPMessage : Asn1Sequence
    {
        [Asn1Field(0)]
        public MessageID messageID { get; set; }
        
        [Asn1Field(1)]
        public LDAPMessage_protocolOp protocolOp { get; set; }
        
        public LDAPMessage()
        {
            this.messageID = null;
            this.protocolOp = null;
        }
        
        public LDAPMessage(
         MessageID messageID,
         LDAPMessage_protocolOp protocolOp)
        {
            this.messageID = messageID;
            this.protocolOp = protocolOp;
        }
    }
}

