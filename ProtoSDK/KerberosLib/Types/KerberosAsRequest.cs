// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosAsRequest : KerberosPdu
    {
        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        public AS_REQ Request
        {
            get;
            private set;
        }

        private TransportType transportType;

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KerberosAsRequest()
        {
            this.Request = new AS_REQ();
        }

        public KerberosAsRequest(long pvno, KDC_REQ_BODY kdcReqBody, Asn1SequenceOf<PA_DATA> paDatas, TransportType transportType)
        {
            this.Request = new AS_REQ(new Asn1Integer(pvno), new Asn1Integer((long)MsgType.KRB_AS_REQ), paDatas, kdcReqBody);
            this.transportType = transportType;
        }

        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            Asn1BerEncodingBuffer asBerBuffer = new Asn1BerEncodingBuffer();
            this.Request.BerEncode(asBerBuffer, true);
            KerberosUtility.OnDumpMessage("KRB5:KrbMessage",
                "Kerberos Message",
                KerberosUtility.DumpLevel.WholeMessage,
                asBerBuffer.Data);
            if (transportType == TransportType.TCP)
            {
                return KerberosUtility.WrapLength(asBerBuffer.Data, true);
            }
            else
            {
                return asBerBuffer.Data;
            }
        }


        /// <summary>
        /// Decode AS Request from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(buffer);
            Request.BerDecode(decodeBuffer);
        }
    }
}
