// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// Decoder class which decodes requests/responses for LDAP v3.
    /// </summary>
    internal class AdtsLdapV3Decoder : AdtsLdapDecoder
    {
        /// <summary>
        /// Authenticates a message. Auth data such as token/checksum etc. in the message will be stripped.
        /// For ADTS-LDAP, we simply don't do anything and pass the original packet data back.
        /// </summary>
        /// <param name="packetData">The packet data that may contain auth data.</param>
        /// <returns>The pure packet data without auth data that decoders can successfully decode.</returns>
        internal override byte[] AuthenticateMessage(byte[] packetData)
        {
            return packetData;
        }


        /// <summary>
        /// Decodes an LDAP v3 packet.
        /// </summary>
        /// <param name="messageBytes">The message bytes that contains the packet data.</param>
        /// <param name="context">The context that contains decode-related information.</param>
        /// <returns>Decoded LDAP v3 packet.</returns>
        internal override AdtsLdapPacket ParseAdtsLdapPacket(byte[] messageBytes, AdtsLdapContext context)
        {
            LDAPMessage message = new LDAPMessage();
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(messageBytes);
            
            message.BerDecode(decodeBuffer);

            Type innerMessageType = message.protocolOp.GetData().GetType();
            AdtsLdapPacket packet = CreatePacketFromType(innerMessageType);

            context.MessageId = (long)message.messageID.Value;
            packet.messageId = (long)message.messageID.Value;
            packet.ldapMessagev3 = message;

            return packet;
        }


        /// <summary>
        /// Creates an AdtsLdapPacket from request or response type.
        /// </summary>
        /// <param name="messageType">The type of request or response.</param>
        /// <returns>The packet created. Note no values in the packet are set.</returns>
        /// <remarks>
        /// Note that LDAP v2 and v3 don't share messages, so this method is not put to a common place.
        /// </remarks>
        /// Suppress AvoidExcessiveClassCoupling warning.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        internal AdtsLdapPacket CreatePacketFromType(Type messageType)
        {
            AdtsLdapPacket packet = null;

            if (messageType == typeof(AbandonRequest))
            {
                packet = new AdtsAbandonRequestPacket();
            }
            else if (messageType == typeof(AddRequest))
            {
                packet = new AdtsAddRequestPacket();
            }
            else if (messageType == typeof(AddResponse))
            {
                packet = new AdtsAddResponsePacket();
            }
            else if (messageType == typeof(BindRequest))
            {
                packet = new AdtsBindRequestPacket();
            }
            else if (messageType == typeof(BindResponse))
            {
                packet = new AdtsBindResponsePacket();
            }
            else if (messageType == typeof(CompareRequest))
            {
                packet = new AdtsCompareRequestPacket();
            }
            else if (messageType == typeof(CompareResponse))
            {
                packet = new AdtsCompareResponsePacket();
            }
            else if (messageType == typeof(DelRequest))
            {
                packet = new AdtsDelRequestPacket();
            }
            else if (messageType == typeof(DelResponse))
            {
                packet = new AdtsDelResponsePacket();
            }
            else if (messageType == typeof(ExtendedRequest))
            {
                packet = new AdtsExtendedRequestPacket();
            }
            else if (messageType == typeof(ExtendedResponse))
            {
                packet = new AdtsExtendedResponsePacket();
            }
            else if (messageType == typeof(ModifyDNRequest))
            {
                packet = new AdtsModifyDnRequestPacket();
            }
            else if (messageType == typeof(ModifyDNResponse))
            {
                packet = new AdtsModifyDnResponsePacket();
            }
            else if (messageType == typeof(ModifyRequest))
            {
                packet = new AdtsModifyRequestPacket();
            }
            else if (messageType == typeof(ModifyResponse))
            {
                packet = new AdtsModifyResponsePacket();
            }
            else if (messageType == typeof(SearchRequest))
            {
                packet = new AdtsSearchRequestPacket();
            }
            else if (messageType == typeof(SearchResultEntry))
            {
                packet = new AdtsSearchResultEntryPacket();
            }
            else if (messageType == typeof(SearchResultReference))
            {
                packet = new AdtsSearchResultReferencePacket();
            }
            else if (messageType == typeof(SearchResultDone))
            {
                packet = new AdtsSearchResultDonePacket();
            }
            else if (messageType == typeof(SicilyBindResponse))
            {
                packet = new AdtsSicilyBindResponsePacket();
            }
            else if (messageType == typeof(UnbindRequest))
            {
                packet = new AdtsUnbindRequestPacket();
            }
            else
            {
                throw new StackException("Unknown message type: " + messageType.ToString());
            }

            return packet;
        }
    }
}
