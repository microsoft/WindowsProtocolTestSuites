// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosFastRequest
    {
        public KrbFastReq FastReq
        {
            get;
            set;
        }

        public KerberosFastRequest(FastOptions options, Asn1SequenceOf<PA_DATA> seqPaData, KDC_REQ_BODY kdcReqBody)
        {
            this.FastReq = new KrbFastReq(options, seqPaData, kdcReqBody);
        }

        public KerberosFastRequest(KrbFastReq fastReq)
        {
            FastReq = fastReq;
        }
        public byte[] ToBytes(TransportType transportType)
        {
            Asn1BerEncodingBuffer asBerBuffer = new Asn1BerEncodingBuffer();
            this.FastReq.BerEncode(asBerBuffer, true);
            if (transportType == TransportType.TCP)
            {
                return KerberosUtility.WrapLength(asBerBuffer.Data, true);
            }
            else
            {
                return asBerBuffer.Data;
            }
        }


    }
}
