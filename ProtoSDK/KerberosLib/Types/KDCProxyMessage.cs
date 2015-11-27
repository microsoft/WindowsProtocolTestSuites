// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.Types
{
    /// <summary>
    /// A wrapper for KDC_PROXY_MESSAGE defined in MS-KKDCP
    /// </summary>
    public class KDCProxyMessage
    {
        /// <summary>
        /// kerb-message: A Kerberos message, including the 4 octet length value specified in [RFC4120] section 7.2.2 in network byte order.
        /// Section 7.2.2 defines the message syntax in TCP/IP transport.
        /// </summary>
        public KDC_PROXY_MESSAGE Message;

        /// <summary>
        /// target-domain: An optional KerberosString ([RFC4120] section 5.2.1) that represents the realm 
        /// to which the Kerberos message is sent, which is required for client messages and is not used in server messages. 
        /// This value is not case-sensitive.
        /// </summary>
        private string targetDomain;
        public string TargetDomain
        {
            set
            {
                targetDomain = value;
                Message.target_domain = new KerberosString(value);
            }
            get
            {
                return targetDomain;
            }
        }

        /// <summary>
        /// dclocator-hint: An optional Flags ([MS-NRPC] section 3.5.4.3.1) 
        /// which contains additional data to be used to find a domain controller for the Kerberos message.
        /// </summary>
        private uint dcLocatorHint;
        public uint DCLocatorHint
        {
            set
            {
                dcLocatorHint = value;
                Message.dclocator_hint = new Asn1Integer(value);
            }
            get
            {
                return dcLocatorHint;
            }
        }

        /// <summary>
        /// Initializes a new instance of the KDCProxyMessage class.
        /// </summary>
        public KDCProxyMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the KDCProxyMessage class using the specified kerb-message.
        /// </summary>
        /// <param name="pdu">PDU of the inner kerb-message </param>
        public KDCProxyMessage(KerberosPdu pdu)
        {
            Message = new KDC_PROXY_MESSAGE(new Asn1OctetString(pdu.ToBytes()), null, null);
        }

        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public byte[] ToBytes()
        {
            Asn1BerEncodingBuffer asBerBuffer = new Asn1BerEncodingBuffer();
            this.Message.BerEncode(asBerBuffer, true);
            return asBerBuffer.Data;
        }

        /// <summary>
        /// Decode KDCProxyMessage from bytes
        /// </summary>
        /// <param name="buffer">byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public void FromBytes(byte[] buffer)
        {
            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }
            this.Message = new KDC_PROXY_MESSAGE();
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(buffer);
            this.Message.BerDecode(decodeBuffer);
        }

    }
}
