// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    ConnectData ::= SEQUENCE
    {
        t124Identifier Key,
        -- This shall be set to the value {itu-t recommendation t 124 version(0) 1}
        connectPDU OCTET STRING
    }
    */
    public class ConnectData : Asn1Sequence
    {
        [Asn1Field(0)]
        public Key t124Identifier { get; set; }
        
        [Asn1Field(1)]
        public Asn1OctetString connectPDU { get; set; }
        
        public ConnectData()
        {
            this.t124Identifier = null;
            this.connectPDU = null;
        }
        
        public ConnectData(
         Key t124Identifier,
         Asn1OctetString connectPDU)
        {
            this.t124Identifier = t124Identifier;
            this.connectPDU = connectPDU;
        }

        public override void PerEncode(IAsn1PerEncodingBuffer buffer)
        {
            if (t124Identifier != null && ((Asn1ObjectIdentifier)t124Identifier.GetData()).Value.SequenceEqual(new int[] { 0, 20, 124, 0, 1 }))
            {
                buffer.WriteBytes(new byte[] { 0, 5, 0, 20, 124, 0, 1 });
                connectPDU.PerEncode(buffer);
            }
            else
            {
                base.PerEncode(buffer);
            }
        }
    }
}

