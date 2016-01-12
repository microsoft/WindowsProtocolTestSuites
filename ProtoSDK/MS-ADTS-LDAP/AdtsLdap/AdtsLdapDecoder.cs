// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// Decoder class which decodes LDAP requests/responses.
    /// </summary>
    internal abstract class AdtsLdapDecoder
    {
        /// <summary>
        /// Authenticates a message. Auth data such as token/checksum etc. in the message will be stripped.
        /// </summary>
        /// <param name="packetData">The packet data that may contain auth data.</param>
        /// <returns>The pure packet data without auth data that decoders can successfully decode.</returns>
        internal abstract byte[] AuthenticateMessage(byte[] packetData);


        /// <summary>
        /// Decodes LDAP packets.
        /// </summary>
        /// <param name="messageBytes">The message bytes that contains the packet data.</param>
        /// <param name="context">The context that contains decode-related information.</param>
        /// <returns>Decoded LDAP packet.</returns>
        internal abstract AdtsLdapPacket ParseAdtsLdapPacket(byte[] messageBytes, AdtsLdapContext context);
    }
}
