// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public interface IPaData
    {
        /// <summary>
        /// Raw PA_DATA
        /// </summary>
        PA_DATA Data
        {
            get;
        }
    }

    public class PaPacOptions : IPaData
    {
        public PacOptions Options
        {
            get;
            set;
        }

        public PaPacOptions(PacOptions pacOptions)
        {
            this.Options = pacOptions;
        }

        public PA_DATA Data
        {
            get 
            {
                KERB_PA_PAC_OPTIONS paPacOptions = new KERB_PA_PAC_OPTIONS(new PA_PAC_OPTIONS(KerberosUtility.ConvertInt2Flags((int)this.Options)));
                Asn1BerEncodingBuffer paPacOptionBuffer = new Asn1BerEncodingBuffer();
                paPacOptions.BerEncode(paPacOptionBuffer, true);
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_PAC_OPTION), new Asn1OctetString(paPacOptionBuffer.Data));
            }
        }
        public static PaPacOptions Parse(PA_DATA data)
        {
            if (data.padata_type.Value != (long)PaDataType.PA_PAC_OPTION)
                throw new Exception();
            KERB_PA_PAC_OPTIONS paPacOptions = new KERB_PA_PAC_OPTIONS();
            paPacOptions.BerDecode(new Asn1DecodingBuffer(data.padata_value.ByteArrayValue));
            throw new NotImplementedException();//need to convert from bytd[] to integer
        }

    }

    public class PaPacRequest : IPaData
    {
        public bool IncludePac
        {
            get;
            set;
        }

        public PaPacRequest(bool includePac)
        {
            this.IncludePac = includePac;
        }

        public PA_DATA Data
        {
            get
            {
                KERB_PA_PAC_REQUEST paPacRequest = new KERB_PA_PAC_REQUEST(new Asn1Boolean(this.IncludePac));
                Asn1BerEncodingBuffer paPacBuffer = new Asn1BerEncodingBuffer();
                paPacRequest.BerEncode(paPacBuffer);
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_PAC_REQUEST), new Asn1OctetString(paPacBuffer.Data));
            }
        }

        /// <summary>
        /// Parse raw PA_DATA type to PaPacRequest object.
        /// </summary>
        /// <param name="data">Raw PA_DATA</param>
        /// <returns>Reference to PaPacRequest object</returns>
        public static PaPacRequest Parse(PA_DATA data)
        {
            if (data.padata_type.Value != (long)PaDataType.PA_PAC_REQUEST)
                throw new Exception();
            KERB_PA_PAC_REQUEST request = new KERB_PA_PAC_REQUEST();
            request.BerDecode(new Asn1DecodingBuffer(data.padata_value.ByteArrayValue));
            return new PaPacRequest(request.include_pac.Value);
        }
    }


    public class PaEncryptedChallenge : IPaData
    {
        /// <summary>
        /// The client's time.
        /// </summary>
        public string TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// The microseconds of the client's time.
        /// </summary>
        public int Usec
        {
            get;
            set;
        }

        public EncryptionKey Key { get; set; }


        public PaEncryptedChallenge(EncryptionType type, string timeStamp, int usec, EncryptionKey armorKey, EncryptionKey userLongTermKey)
        {
            this.TimeStamp = timeStamp;
            this.Usec = usec;

            var keyvalue = KeyGenerator.KrbFxCf2(
                (EncryptionType)armorKey.keytype.Value,
                armorKey.keyvalue.ByteArrayValue,
                userLongTermKey.keyvalue.ByteArrayValue,
                "clientchallengearmor",
                "challengelongterm");
            switch (type)
            {
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    {
                        var key = new EncryptionKey(new KerbInt32((long)EncryptionType.AES256_CTS_HMAC_SHA1_96), new Asn1OctetString(keyvalue));
                        this.Key = key;
                        break;
                    }
                case EncryptionType.RC4_HMAC:
                    {
                        var key = new EncryptionKey(new KerbInt32((long)EncryptionType.RC4_HMAC), new Asn1OctetString(keyvalue));
                        this.Key = key;
                        break;
                    }
                default:
                    throw new ArgumentException("Unsupported encryption type.");
            }

            // create a timestamp            
            PA_ENC_TS_ENC paEncTsEnc = new PA_ENC_TS_ENC(new KerberosTime(this.TimeStamp), new Microseconds(this.Usec));
            Asn1BerEncodingBuffer currTimeStampBuffer = new Asn1BerEncodingBuffer();
            paEncTsEnc.BerEncode(currTimeStampBuffer);

            var rawData = currTimeStampBuffer.Data;
            KerberosUtility.OnDumpMessage("KRB5:PA-ENC-TS-ENC",
                "Encrypted Timestamp Pre-authentication",
                KerberosUtility.DumpLevel.PartialMessage,
                rawData);

            // encrypt the timestamp
            byte[] encTimeStamp = KerberosUtility.Encrypt((EncryptionType)this.Key.keytype.Value,
                                                      this.Key.keyvalue.ByteArrayValue,
                                                      rawData,
                                                      (int)KeyUsageNumber.ENC_CHALLENGE_CLIENT);

            EncryptedChallenge encryptedChallenge = new EncryptedChallenge(new KerbInt32((long)this.Key.keytype.Value),
                null,
                new Asn1OctetString(encTimeStamp));

            Asn1BerEncodingBuffer paEncTimestampBuffer = new Asn1BerEncodingBuffer();
            encryptedChallenge.BerEncode(paEncTimestampBuffer, true);

            Data = new PA_DATA(new KerbInt32((long)PaDataType.PA_ENCRYPTED_CHALLENGE), new Asn1OctetString(paEncTimestampBuffer.Data));

        }

        public PA_DATA Data
        {
            private set;
            get;

        }
        public static PaEncryptedChallenge Parse(PA_DATA data)
        {
            throw new NotImplementedException();
        }
    }

    public class PaEncTimeStamp : IPaData
    {
        /// <summary>
        /// The client's time.
        /// </summary>
        public string TimeStamp
        {
            get;
            private set;
        }

        /// <summary>
        /// The microseconds of the client's time.
        /// </summary>
        public int Usec
        {
            get;
            private set;
        }

        public EncryptionKey Key { get; set; }


        public PaEncTimeStamp(string timeStamp, int usec, EncryptionType eType, string password, string salt)
        {
            this.TimeStamp = timeStamp;
            this.Usec = usec;
            byte[] key = KeyGenerator.MakeKey(eType, password, salt);
            this.Key = new EncryptionKey(new KerbInt32((long)eType), new Asn1OctetString(key));

            // create a timestamp
            PA_ENC_TS_ENC paEncTsEnc = new PA_ENC_TS_ENC(new KerberosTime(this.TimeStamp), new Microseconds(this.Usec));
            Asn1BerEncodingBuffer currTimeStampBuffer = new Asn1BerEncodingBuffer();
            paEncTsEnc.BerEncode(currTimeStampBuffer);
            var rawData = currTimeStampBuffer.Data;

            KerberosUtility.OnDumpMessage("KRB5:PA-ENC-TS-ENC",
                "Encrypted Timestamp Pre-authentication",
                KerberosUtility.DumpLevel.PartialMessage, 
                rawData);
            // encrypt the timestamp
            byte[] encTimeStamp = KerberosUtility.Encrypt((EncryptionType)this.Key.keytype.Value,
                                                      this.Key.keyvalue.ByteArrayValue,
                                                      rawData,
                                                      (int)KeyUsageNumber.PA_ENC_TIMESTAMP);

            // create an encrypted timestamp
            PA_ENC_TIMESTAMP paEncTimeStamp =
                new PA_ENC_TIMESTAMP(new KerbInt32(this.Key.keytype.Value), null, new Asn1OctetString(encTimeStamp));
            Asn1BerEncodingBuffer paEncTimestampBuffer = new Asn1BerEncodingBuffer();
            paEncTimeStamp.BerEncode(paEncTimestampBuffer, true);

            Data = new PA_DATA(new KerbInt32((long)PaDataType.PA_ENC_TIMESTAMP), new Asn1OctetString(paEncTimestampBuffer.Data));
        }

        public PA_DATA Data
        {
            get;
            private set;
        }

        public static PaEncTimeStamp Parse(PA_DATA data)
        {
            throw new NotImplementedException();
        }
    }
    public class PaTgsReq : IPaData
    {
        public AP_REQ ApReq;
        public PaTgsReq(AP_REQ apReq)
        {
            ApReq = apReq;

        }

        public PA_DATA Data
        {
            get
            {
                Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
                ApReq.BerEncode(buffer);
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_TGS_REQ), new Asn1OctetString(buffer.Data));
            }
        }
        /// <summary>
        /// Parse raw PA_DATA type to PaTgsReq object.
        /// </summary>
        /// <param name="data">Raw PA_DATA</param>
        /// <returns>Reference to PaTgsReq object</returns>
        public static PaTgsReq Parse(PA_DATA data)
        {
            if (data.padata_type.Value != (long)PaDataType.PA_TGS_REQ)
                throw new Exception();
            AP_REQ apReq = new AP_REQ();
            apReq.BerDecode(new Asn1DecodingBuffer(data.padata_value.ByteArrayValue));
            return new PaTgsReq(apReq);
        }
    }

    public class PaSvrReferralInfo : IPaData
    {
        public PA_SVR_REFERRAL_DATA PaSvrReferralData;
        public PaSvrReferralInfo(PA_SVR_REFERRAL_DATA paSvrReferralData)
        {
            this.PaSvrReferralData = paSvrReferralData;
        }

        public PA_DATA Data
        {
            get
            {
                Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
                PaSvrReferralData.BerEncode(buffer);
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_SVR_REFERRAL_INFO), new Asn1OctetString(buffer.Data));
            }
        }

        public static PaSvrReferralInfo Parse(PA_DATA data)
        {
            if (data.padata_type.Value != (long)PaDataType.PA_SVR_REFERRAL_INFO)
                throw new Exception();
            PA_SVR_REFERRAL_DATA paSvrReferralData = new PA_SVR_REFERRAL_DATA();
            paSvrReferralData.BerDecode(new Asn1DecodingBuffer(data.padata_value.ByteArrayValue));
            return new PaSvrReferralInfo(paSvrReferralData);
        }
    }

    public class PaFxFastReq : IPaData
    {
        public PA_FX_FAST_REQUEST FastRequest { get; private set; }
        public PaFxFastReq(PA_FX_FAST_REQUEST fastRequest)
        {
            FastRequest = fastRequest;
        }
        public PA_DATA Data
        {
            get 
            {
                if (FastRequest != null)
                {
                    Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
                    FastRequest.BerEncode(buffer);
                    return new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), new Asn1OctetString(buffer.Data));
                }
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null);
            }
        }
        /// <summary>
        /// Parse raw PA_DATA type to PaFxFast object.
        /// </summary>
        /// <param name="data">Raw PA_DATA</param>
        /// <returns>Reference to PaFxFast object</returns>
        public static PaFxFastReq Parse(PA_DATA data)
        {
            if (data.padata_type.Value != (long)PaDataType.PA_FX_FAST)
                throw new Exception();
            PA_FX_FAST_REQUEST request = new PA_FX_FAST_REQUEST();
            request.BerDecode(new Asn1DecodingBuffer(data.padata_value.ByteArrayValue));
            return new PaFxFastReq(request);
        }
    }


    public class PaFxFastRep : IPaData
    {
        public PA_FX_FAST_REPLY FastReply { get; private set; }

        public KrbFastArmoredRep GetArmoredRep()
        {
            var rep = FastReply.GetData();
            if (rep is KrbFastArmoredRep) return (KrbFastArmoredRep)rep;
            throw new ArrayTypeMismatchException();
        }

        public KerberosFastResponse GetKerberosFastRep(EncryptionKey key)
        {
            var armoredRep = GetArmoredRep();
            var decrypted = KerberosUtility.Decrypt((EncryptionType)key.keytype.Value ,
                key.keyvalue.ByteArrayValue,
                armoredRep.enc_fast_rep.cipher.ByteArrayValue,
                (int)KeyUsageNumber.FAST_REP
            );

            KerberosUtility.OnDumpMessage("KRB5:KrbFastArmoredRep(enc-fast-req)",
                "An encrypted KrbFastRep in PA_FX_FAST_REPLY",
                KerberosUtility.DumpLevel.PartialMessage,
                decrypted);

            KrbFastResponse fastrep = new KrbFastResponse();
            fastrep.BerDecode(new Asn1DecodingBuffer(decrypted));
            return new KerberosFastResponse(fastrep);
        }
        
        public PaFxFastRep(PA_FX_FAST_REPLY fastReply)
        {
            FastReply = fastReply;
        }

        public PA_DATA Data
        {
            get
            {
                if (FastReply == null)
                    return new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null);
                Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
                FastReply.BerEncode(buffer);
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), new Asn1OctetString(buffer.Data));
            }
        }
        /// <summary>
        /// Parse raw PA_DATA type to PaFxFastReq object.
        /// </summary>
        /// <param name="data">Raw PA_DATA</param>
        /// <returns>Reference to PaFxFast object</returns>
        public static PaFxFastRep Parse(PA_DATA data)
        {
            if (data.padata_type.Value != (long)PaDataType.PA_FX_FAST)
                throw new Exception();
            if (data.padata_value != null && data.padata_value.Value.Length > 0)
            {
            PA_FX_FAST_REPLY reply = new PA_FX_FAST_REPLY();
            reply.BerDecode(new Asn1DecodingBuffer(data.padata_value.ByteArrayValue));
            return new PaFxFastRep(reply);
            }
            return new PaFxFastRep(null);
        }
    }



    public class PaETypeInfo2 : IPaData
    {
        public ETYPE_INFO2 ETypeInfo2 { get; private set; }

        public PaETypeInfo2(ETYPE_INFO2 eTypeInfo2)
        {
            ETypeInfo2 = eTypeInfo2;
        }

        public PA_DATA Data
        {
            get 
            {
                var buffer = new Asn1BerEncodingBuffer();
                ETypeInfo2.BerEncode(buffer);
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_ETYPE_INFO2), new Asn1OctetString(buffer.Data));
            }
        }

        /// <summary>
        /// Parse raw PA_DATA type to PaFxFast object.
        /// </summary>
        /// <param name="data">Raw PA_DATA</param>
        /// <returns>Reference to PaFxFast object</returns>
        public static PaETypeInfo2 Parse(PA_DATA data)
        {
            if (data.padata_type.Value != (long)PaDataType.PA_ETYPE_INFO2)
                throw new Exception();
            ETYPE_INFO2 eTypeInfo = new ETYPE_INFO2();
            eTypeInfo.BerDecode(new Asn1DecodingBuffer(data.padata_value.ByteArrayValue));
            return new PaETypeInfo2(eTypeInfo);
        }
    }

    public class PaFxError : IPaData
    {
        public KerberosKrbError KrbError { get; private set; }
        private PA_DATA paData;
        public PA_DATA Data
        {
            get {
                return paData; 
            }
        }
        public PaFxError(PA_DATA PaData)
        {
            paData = PaData;
            KrbError = new KerberosKrbError();
            KrbError.FromBytes(PaData.padata_value.ByteArrayValue);
        }

        public static PaFxError Parse(PA_DATA data)
        {
            var paFxError = new PaFxError(data);
            return paFxError;
        }
    }

    public class PaFxCookie : IPaData
    {
        private PA_DATA data;
        public PA_DATA Data
        {
            get { return data; }
        }
        public PaFxCookie(PA_DATA data)
        {
            this.data = data;
        }
        public static PaFxCookie Parse(PA_DATA data)
        {
            return new PaFxCookie(data);
        }
    }

    public class PaSupportedEncTypes : IPaData
    {
        public PA_DATA Data
        {
            get
            {
                var value = (int)SupportedEncTypes;
                return new PA_DATA(new KerbInt32((long)PaDataType.PA_SUPPORTED_ENCTYPES), new Asn1OctetString(BitConverter.GetBytes(value)));
            }
        }
        public SupportedEncryptionTypes SupportedEncTypes { get; set; }

        public PaSupportedEncTypes(PA_DATA data)
        {
            uint value = BitConverter.ToUInt32(data.padata_value.ByteArrayValue, 0);
            SupportedEncTypes = (SupportedEncryptionTypes)value;
        }
        public static PaSupportedEncTypes Parse(PA_DATA data)
        {
            return new PaSupportedEncTypes(data);
        }
    }

    public class PaRawData : IPaData
    {
        private PA_DATA data;
        public PA_DATA Data
        {
            get { return data; }
        }
        public PaRawData(PA_DATA data)
        {
            this.data = data;
        }
        public static PaRawData Parse(PA_DATA data)
        {
            return new PaRawData(data);
        }
    }

    /// <summary>
    /// Pa_Data parser
    /// </summary>
    public static class PaDataParser
    {
        /// <summary>
        /// Parse raw PA_DATA type to IPaData object.
        /// </summary>
        /// <param name="data">Raw PA_DATA</param>
        /// <returns>Reference to IPaData object</returns>
        public static IPaData ParseReqPaData(PA_DATA data)
        {
            switch (data.padata_type.Value)
            {
                case (long)PaDataType.PA_FX_FAST:
                    return PaFxFastReq.Parse(data);
                case (long)PaDataType.PA_PAC_REQUEST:
                    return PaPacRequest.Parse(data);
                case (long)PaDataType.PA_TGS_REQ:
                    return PaTgsReq.Parse(data);
                case (long)PaDataType.PA_ETYPE_INFO2:
                    return PaETypeInfo2.Parse(data);
            }
            return PaRawData.Parse(data);
        }

        /// <summary>
        /// Parse raw PA_DATA type to IPaData object.
        /// </summary>
        /// <param name="data">Raw PA_DATA</param>
        /// <returns>Reference to IPaData object</returns>
        public static IPaData ParseRepPaData(PA_DATA data)
        {
            switch (data.padata_type.Value)
            {
                case (long)PaDataType.PA_FX_FAST:
                    return PaFxFastRep.Parse(data);
                case (long)PaDataType.PA_ETYPE_INFO2:
                    return PaETypeInfo2.Parse(data);
                case (long)PaDataType.PA_FX_ERROR:
                    return PaFxError.Parse(data);
                case (long)PaDataType.PA_FX_COOKIE:
                    return PaFxCookie.Parse(data);
                case (long)PaDataType.PA_SUPPORTED_ENCTYPES:
                    return PaSupportedEncTypes.Parse(data);
                case (long)PaDataType.PA_SVR_REFERRAL_INFO:
                    return PaSvrReferralInfo.Parse(data);
            }
            return PaRawData.Parse(data);
        }
    }
}
