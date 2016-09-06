// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System.Diagnostics.CodeAnalysis;
using LdapV2 = Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2;
using LdapV3 = Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// The base class of all requests/response packets.
    /// </summary>
    public abstract class AdtsLdapPacket : StackPacket
    {
        /// <summary>
        /// Message ID. See MessageId property.
        /// </summary>
        internal long messageId;

        /// <summary>
        /// The LDAP v2 message.
        /// </summary>
        internal LdapV2.LDAPMessage ldapMessagev2;

        /// <summary>
        /// The LDAP v3 message.
        /// </summary>
        internal LdapV3.LDAPMessage ldapMessagev3;

        /// <summary>
        /// Message ID of the packet. Added to simplify the user to get this attribute.
        /// </summary>
        public long MessageId
        {
            get
            {
                return this.messageId;
            }
            set
            {
                this.messageId = value;
                if (this.ldapMessagev2 != null)
                {
                    ldapMessagev2.messageID = new LdapV2.MessageID(this.messageId);
                }
                else
                {
                    ldapMessagev3.messageID = new LdapV3.MessageID(this.messageId);
                }
            }
        }


        /// <summary>
        /// Gets the ldap message. Could be v2 or v3. A type case is possible after user retrieves the message.
        /// </summary>
        public Asn1Object LdapMessage
        {
            get
            {
                if (this.ldapMessagev2 != null)
                {
                    return this.ldapMessagev2;
                }
                return this.ldapMessagev3;
            }
        }


        /// <summary>
        /// Gets inner request or response that's contained in the ldap message.
        /// </summary>
        /// <returns>The request or response. This could be LDAP v2 or v3, so type cast may be possible.</returns>
        /// Disabled fxcop rule to use a method instead of property.
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public abstract Asn1Object GetInnerRequestOrResponse();


        /// <summary>
        /// Clones the packet.
        /// </summary>
        /// <returns>The cloned packet.</returns>
        public override StackPacket Clone()
        {
            return (StackPacket)ObjectUtility.DeepClone(this);
        }


        /// <summary>
        /// Converts the packet to a byte array that transmits on the wire.
        /// </summary>
        /// <returns>The byte array.</returns>
        public override byte[] ToBytes()
        {
            Asn1BerEncodingBuffer berEncoder = new Asn1BerEncodingBuffer();
            LdapV2.LDAPMessage ldapv2Msg = ldapMessagev2 as LdapV2.LDAPMessage;
            if (ldapv2Msg != null)
            {
                ldapv2Msg.BerEncode(berEncoder);
                return berEncoder.Data;
            }

            LdapV3.LDAPMessage ldapv3Msg = ldapMessagev3 as LdapV3.LDAPMessage;
            if (ldapv3Msg != null)
            {
                ldapv3Msg.BerEncode(berEncoder);
                return berEncoder.Data;
            }

            throw new StackException("The LDAP v2 and v3 messages cannot be both null");
        }
    }
}