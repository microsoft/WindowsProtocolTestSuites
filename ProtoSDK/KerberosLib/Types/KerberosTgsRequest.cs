// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosTgsRequest : KerberosPdu
    {
        private TransportType transportType;

        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        public TGS_REQ Request
        {
            get;
            private set;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KerberosTgsRequest()
            : base()
        {
            this.Request = new TGS_REQ();
        }

        public KerberosTgsRequest(long pvno, KDC_REQ_BODY kdcReqBody, Asn1SequenceOf<PA_DATA> paDatas, TransportType transportType)
        {
            this.Request = new TGS_REQ(new Asn1Integer(pvno), new Asn1Integer((long)MsgType.KRB_TGS_REQ), paDatas, kdcReqBody);
            this.transportType = transportType;
        }

        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            Asn1BerEncodingBuffer tgsBerBuffer = new Asn1BerEncodingBuffer();
            this.Request.BerEncode(tgsBerBuffer, true);
            KerberosUtility.OnDumpMessage("KRB5:KrbMessage",
                "Kerberos Message",
                KerberosUtility.DumpLevel.WholeMessage,
                tgsBerBuffer.Data);
            if (transportType == TransportType.TCP)
            {
                return KerberosUtility.WrapLength(tgsBerBuffer.Data, true);
            }
            else
            {
                return tgsBerBuffer.Data;
            }
        }


        /// <summary>
        /// Decode TGS Request from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public void Encrypt(KerberosContext context)
        {
        }
    }
}
