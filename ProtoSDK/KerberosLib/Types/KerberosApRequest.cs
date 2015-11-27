// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosApRequest : KerberosPdu
    {
        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        public AP_REQ Request
        {
            get;
            private set;
        }

        /// <summary>
        /// The non-encrypt EncryptedData authenticator of AP_REQ.
        /// </summary>
        public Authenticator Authenticator
        {
            get;
            private set;
        }

        public KerberosApRequest()
            : base()
        {
            this.Request = new AP_REQ();
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        public KerberosApRequest(long pvno, APOptions ap_options, KerberosTicket ticket, Authenticator authenticator, KeyUsageNumber keyUsageNumber)
        {
            Asn1BerEncodingBuffer asnBuffPlainAuthenticator = new Asn1BerEncodingBuffer();
            authenticator.BerEncode(asnBuffPlainAuthenticator, true);
            KerberosUtility.OnDumpMessage("KRB5:Authenticator",
                "Authenticator in AP-REQ structure",
                 KerberosUtility.DumpLevel.PartialMessage,
                 asnBuffPlainAuthenticator.Data);
            byte[] encAsnEncodedAuth = KerberosUtility.Encrypt((EncryptionType)ticket.SessionKey.keytype.Value,
                                    ticket.SessionKey.keyvalue.ByteArrayValue,
                                    asnBuffPlainAuthenticator.Data,
                                    (int)keyUsageNumber);
            var encrypted = new EncryptedData();
            encrypted.etype = new KerbInt32(ticket.SessionKey.keytype.Value);
            encrypted.cipher = new Asn1OctetString(encAsnEncodedAuth);

            long msg_type = (long)MsgType.KRB_AP_REQ;
            Request = new AP_REQ(new Asn1Integer(pvno), new Asn1Integer(msg_type), ap_options, ticket.Ticket, encrypted);
            Authenticator = authenticator;
        }

        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            Asn1BerEncodingBuffer asBerBuffer = new Asn1BerEncodingBuffer();
            this.Request.BerEncode(asBerBuffer, true);
            return asBerBuffer.Data;
        }


        /// <summary>
        /// Decode AP Request from bytes
        /// </summary>
        /// <param name="buffer">byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
