// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;

using LdapV3 = Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// LDAP server decoder class.
    /// </summary>
    internal class AdtsLdapServerDecoder : AdtsLdapDecoderCallbackBase
    {
        /// <summary>
        /// an AdtsLdapServer object that indicates the server.
        /// </summary>
        private AdtsLdapServer server;

        /// <summary>
        /// The decoder that decodes LDAP v2 messages.
        /// </summary>
        private AdtsLdapV2Decoder decoderv2;

        /// <summary>
        /// The decoder that decodes LDAP v3 messages.
        /// </summary>
        private AdtsLdapV3Decoder decoderv3;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ldapServer">
        /// an AdtsLdapServer object that indicates the server.
        /// </param>
        internal AdtsLdapServerDecoder(AdtsLdapServer ldapServer)
        {
            this.server = ldapServer;
            this.decoderv2 = new AdtsLdapV2Decoder();
            this.decoderv3 = new AdtsLdapV3Decoder();
        }


        /// <summary>
        /// Gets context. 
        /// </summary>
        /// <param name="clientAddress">The address of LDAP client.</param>
        /// <param name="serverPort">The port of LDAP server.</param>
        /// <param name="isTcp">Specifies whether the connection is TCP.</param>
        /// <returns>The corresponding context. If the context doesn't exist, null is returned.</returns>
        internal AdtsLdapContext GetContext(
            IPEndPoint clientAddress,
            bool isTcpConnection)
        {
            return this.server.ContextManager.GetContext(clientAddress, isTcpConnection);
        }


        /// <summary>
        /// This method encapsulates LdapV2Decoder and LdapV3Decoder because the incoming packets may be 
        /// LDAP v2 or v3 packets, the decoding may require both decoders if the LDAP version that client uses
        /// is not clear.
        /// </summary>
        /// <param name="endPoint">The end point of the client.</param>
        /// <param name="messageBytes">The message bytes that contains the packet data.</param>
        /// <param name="consumedLength">Consumed length.</param>
        /// <param name="expectedLength">
        /// Indicates expected length if the message bytes doesn't all packet data.
        /// </param>
        /// <returns>Decoded packets.</returns>
        internal override StackPacket[] DecodeLdapPacketCallBack(
            object endPoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            // Get packet data, start decoding.
            IPEndPoint remote = (IPEndPoint)endPoint;
            AdtsLdapContext context = GetContext(remote, this.server.IsTcp);

            byte[] data = messageBytes;

            // decode messageBytes if needed
            if (context != null && messageBytes != null)
            {
                // if security is not init, and the package is SslHandshake, init security.
                if (context.Security == null)
                {
                    AdtsLdapSslTlsSecurityHeader header = new AdtsLdapSslTlsSecurityHeader();
                    header.FromBytes(messageBytes);

                    if (header.IsSslHandShake)
                    {
                        this.server.SslStartup(context);
                    }
                }

                // decode messageBytes with security.
                if (context.Security != null)
                {
                    data = this.server.Decrypt(context, messageBytes);
                }
            }

            byte[] packetData = this.GetSinglePacketData(data, out consumedLength, out expectedLength);

            // add the comsumed length of security decorder
            if (context != null && context.Security != null && context.Security.ConsumedData)
            {
                consumedLength = context.Security.ConsumedLength;
            }

            if (packetData == null)
            {
                return null;
            }

            // New connection. Try both LDAP v2 and v3 decoders. Note that for requests that don't have version
            // context(e.g., UDP search requests), LDAP v3 is used by default.
            AdtsLdapPacket packet = null;
            if (context == null)
            {
                context = new AdtsLdapContext(AdtsLdapVersion.V3, remote);
                packet = this.decoderv3.ParseAdtsLdapPacket(
                    this.decoderv3.AuthenticateMessage(packetData),
                    context);
                // synchronize the message ID with newly received packet.
                context.MessageId = packet.messageId;

                // Check BindRequestPacket, normally it's the first message from client in which
                // contains the LDAP version. And since LDAP v3 decoder can decode LDAP v2 messages
                // without any exception, we need to check into the 'version' variable in the request.
                AdtsBindRequestPacket bindRequestPacket = packet as AdtsBindRequestPacket;
                if (bindRequestPacket != null)
                {
                    LdapV3.BindRequest bindRequest = 
                        (LdapV3.BindRequest)bindRequestPacket.GetInnerRequestOrResponse();
                    // Version doesn't match. Decode again with LDAP v2 decoder and update context version.
                    if ((AdtsLdapVersion)bindRequest.version.Value == AdtsLdapVersion.V2)
                    {
                        packet = this.decoderv2.ParseAdtsLdapPacket(
                            this.decoderv2.AuthenticateMessage(packetData),
                            context);
                        context.ClientVersion = context.ServerVersion = AdtsLdapVersion.V2;
                    }
                }

                // Add context.
                this.server.ContextManager.AddContext(context, this.server.IsTcp);
            }
            else
            {
                if (context.ClientVersion == AdtsLdapVersion.V2)
                {
                    packet = this.decoderv2.ParseAdtsLdapPacket(
                        this.decoderv2.AuthenticateMessage(packetData),
                        context);
                }
                else
                {
                    packet = this.decoderv3.ParseAdtsLdapPacket(
                        this.decoderv3.AuthenticateMessage(packetData),
                        context);
                }
                // synchronize the message ID with newly received packet.
                context.MessageId = packet.messageId;
            }

            return new StackPacket[] { packet };
        }
    }
}
