// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    internal class AdtsLdapClientDecoder : AdtsLdapDecoderCallbackBase
    {
        /// <summary>
        /// an AdtsLdapClient object that provides the transport/packets/security services.
        /// </summary>
        private AdtsLdapClient client;

        /// <summary>
        /// The decoder instance.
        /// </summary>
        private AdtsLdapDecoder decoder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adtsLdapClient">
        /// an AdtsLdapClient object that provides the transport/packets/security services.
        /// </param>
        internal AdtsLdapClientDecoder(AdtsLdapClient adtsLdapClient)
        {
            this.client = adtsLdapClient;
            if (this.client.Context.ClientVersion == AdtsLdapVersion.V2)
            {
                this.decoder = new AdtsLdapV2Decoder();
            }
            else
            {
                this.decoder = new AdtsLdapV3Decoder();
            }
        }


        /// <summary>
        /// Decodes LDAP packets.
        /// </summary>
        /// <param name="endPoint">The end point of the client.</param>
        /// <param name="messageBytes">The message bytes that contains the packet data.</param>
        /// <param name="consumedLength">Consumed length.</param>
        /// <param name="expectedLength">
        /// Indicates expected length if the message bytes doesn't contain all packet data.
        /// </param>
        /// <returns>Decoded packets.</returns>
        internal override StackPacket[] DecodeLdapPacketCallBack(
            object endPoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            consumedLength = 0;
            expectedLength = 0;
            byte[] data = messageBytes;

            if (this.client.Security != null && messageBytes != null)
            {
                data = this.client.Security.Decode(messageBytes);
            }

            List<StackPacket> packets = new List<StackPacket>();
            while (data != null && data.Length > 0)
            {
                int nextPacketIndex = 0;
                byte[] packetData = this.GetSinglePacketData(data, out nextPacketIndex, out expectedLength);
                consumedLength += nextPacketIndex;

                if (packetData == null)
                {
                    break;
                }

                data = ArrayUtility.SubArray<byte>(data, nextPacketIndex);

                StackPacket packet = null;
                try
                {
                    packet = this.decoder.ParseAdtsLdapPacket(packetData, this.client.Context);
                }
                // because sometimes there is some zero bytes after packet, and cannot be parsed and throw exception,
                // so just ignore the exception.
                catch (Asn1Exception ae)
                {
                    if (ae.Message.Contains("Decoding error. Unexpected data is read"))
                    {
                        throw ae;
                    }
                    else
                    {
                        break;
                    }
                }

                if (packet == null)
                {
                    break;
                }

                packets.Add(packet);
            }

            // add the comsumed length of security decorder
            if (this.client.Security != null && this.client.Security.ConsumedData)
            {
                consumedLength = this.client.Security.ConsumedLength;
            }

            if (packets.Count == 0)
            {
                return null;
            }

            return packets.ToArray();
        }
    }
}
