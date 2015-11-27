// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosKrbError : KerberosPdu
    {
        /// <summary>
        /// ASN.1 type of the PDU.
        /// </summary>
        public KRB_ERROR KrbError
        {
            get;
            set;
        }

        private TransportType transportType = TransportType.TCP;

        public KRB_ERROR_CODE ErrorCode
        {
            get;
            set;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        public KerberosKrbError()
        {
            this.KrbError = new KRB_ERROR();
        }


        /// <summary>
        /// Decode the Krb Error from bytes
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }
            KerberosUtility.OnDumpMessage("KRB5:KrbMessage",
                "Kerberos Message",
                KerberosUtility.DumpLevel.WholeMessage,
                buffer);
            // Decode Krb Error
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(buffer);
            this.KrbError.BerDecode(decodeBuffer);

            ErrorCode = (KRB_ERROR_CODE)KrbError.error_code.Value;
        }


        /// <summary>
        /// Encode the Krb Error to bytes
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        public override byte[] ToBytes()
        {
            Asn1BerEncodingBuffer errorBerBuffer = new Asn1BerEncodingBuffer();
            this.KrbError.BerEncode(errorBerBuffer, true);

            if (transportType == TransportType.TCP)
            {
                return KerberosUtility.WrapLength(errorBerBuffer.Data, true);
            }
            else
            {
                return errorBerBuffer.Data;
            }
        }

        public void Decrypt(KerberosContext context)
        { 
            throw new NotImplementedException();
        }
    }
}
