// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public partial class KerberosTestClient : KerberosClient
    {
        private ITestSite testSite;

        /// <summary>
        /// Construct a Kerberos test client
        /// </summary>
        /// <param name="domain">The realm part of the client's principal identifier.
        /// This argument cannot be null.</param>
        /// <param name="cName">The account to logon the remote machine. Either user account or computer account
        /// This argument cannot be null.</param>
        /// <param name="password">The password of the user. This argument cannot be null.</param>
        /// <param name="accountType">The type of the logged on account. User or Computer</param>
        public KerberosTestClient(string domain, string cName, string password, KerberosAccountType accountType, string kdcAddress, int kdcPort, TransportType transportType, KerberosConstValue.OidPkt oidPkt, string salt = null)
            : base(domain, cName, password, accountType, kdcAddress, kdcPort, transportType, oidPkt, salt)
        {
            testSite = TestClassBase.BaseTestSite;
            if (accountType == KerberosAccountType.Device)
            {
                testSite.Log.Add(LogEntryKind.Debug, "Construct Kerberos client using computer account: {0}@{1}.",
                                    cName, domain);
            }
            else
            {
                testSite.Log.Add(LogEntryKind.Debug, "Construct Kerberos client using user account: {0}@{1}.",
                                    cName, domain);
            }
            EncryptionType[] encryptionTypes = new EncryptionType[]
            { 
                EncryptionType.AES256_CTS_HMAC_SHA1_96,
                EncryptionType.AES128_CTS_HMAC_SHA1_96,
                EncryptionType.RC4_HMAC,
                EncryptionType.RC4_HMAC_EXP,
                EncryptionType.DES_CBC_CRC,
                EncryptionType.DES_CBC_MD5
            };

            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);

            Context.SupportedEType = etype;

            Context.Pvno = KerberosConstValue.KERBEROSV5;
        }

        /// <summary>
        /// Construct a Kerberos test client for FAST
        /// </summary>
        /// <param name="domain">The realm part of the client's principal identifier.
        /// This argument cannot be null.</param>
        /// <param name="cName">The account to logon the remote machine. Either user account or computer account
        /// This argument cannot be null.</param>
        /// <param name="password">The password of the user. This argument cannot be null.</param>
        /// <param name="accountType">The type of the logged on account. User or Computer</param>
        public KerberosTestClient(string domain, string cName, string password, KerberosAccountType accountType, KerberosTicket armorTicket, EncryptionKey armorSessionKey, string kdcAddress, int kdcPort, TransportType transportType, KerberosConstValue.OidPkt omiPkt)
            : base(domain, cName, password, accountType, armorTicket, armorSessionKey, kdcAddress, kdcPort, transportType, omiPkt)
        {
            testSite = TestClassBase.BaseTestSite;
            if (accountType == KerberosAccountType.Device)
            {
                testSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client using computer account: {0}@{1}.",
                                    cName, domain);
            }
            else
            {
                testSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client using user account: {0}@{1}.",
                                    cName, domain);
            }
            EncryptionType[] encryptionTypes = new EncryptionType[]
            { 
                EncryptionType.AES256_CTS_HMAC_SHA1_96,
                EncryptionType.AES128_CTS_HMAC_SHA1_96,
                EncryptionType.RC4_HMAC,
                EncryptionType.RC4_HMAC_EXP,
                EncryptionType.DES_CBC_CRC,
                EncryptionType.DES_CBC_MD5
            };

            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);

            Context.SupportedEType = etype;

            Context.Pvno = KerberosConstValue.KERBEROSV5;
        }

        /// <summary>
        /// Set Supported Encryption Types
        /// </summary>
        /// <param name="EncryptionType[]">An array of encryption types</param>
        public void SetSupportedEType(EncryptionType[] encryptionTypes)
        {
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);

            Context.SupportedEType = etype;
        }

        public override void Connect()
        {
            testSite.Log.Add(LogEntryKind.Debug, "Setup {0} connection with KDC({1}:{2}).", transportType, kdcAddress, kdcPort);
            base.Connect();
        }

        public override void DisConnect()
        {
            testSite.Log.Add(LogEntryKind.Debug, "Drop {0} connection with KDC({1}:{2}).", transportType, kdcAddress, kdcPort);
            base.DisConnect();
        }

        #region AS

        /// <summary>
        /// Create and send AS request
        /// </summary>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="seqPaData">A sequence of preauthentication data</param>
        public void SendAsRequest(KdcOptions kdcOptions, Asn1SequenceOf<PA_DATA> seqPaData)
        {
            string sName = KerberosConstValue.KERBEROS_SNAME;
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname);
            KerberosAsRequest asRequest = this.CreateAsRequest(kdcReqBody, seqPaData);
            this.SendPdu(asRequest);
            this.testSite.Log.Add(LogEntryKind.Debug, "Send AS Request.");
        }

        /// <summary>
        /// Create and send AS request for Password change
        /// </summary>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="seqPaData">A sequence of preauthentication data</param>
        public void SendAsRequestForPwdChange(KdcOptions kdcOptions, Asn1SequenceOf<PA_DATA> seqPaData)
        {
            PrincipalName sname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST),
                KerberosUtility.String2SeqKerbString(KerberosConstValue.KERBEROS_KADMIN_SNAME.Split('/')));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname);
            KerberosAsRequest asRequest = this.CreateAsRequest(kdcReqBody, seqPaData);
            this.SendPdu(asRequest);
            this.testSite.Log.Add(LogEntryKind.Debug, "Send AS Request for password change.");
        }

        /// <summary>
        /// Create and send AS request with specific kvno
        /// </summary>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="seqPaData">A sequence of preauthentication data</param>
        /// <param name="pvno">Protocol version number</param>
        /// <param name="msgType">Message Type</param>
        public void SendAsRequest(KdcOptions kdcOptions, Asn1SequenceOf<PA_DATA> seqPaData, long pvno, MsgType msgType = MsgType.KRB_AS_REQ)
        {
            string sName = KerberosConstValue.KERBEROS_SNAME;
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname);
            KerberosAsRequest asRequest = this.CreateAsRequest(kdcReqBody, seqPaData, pvno);
            asRequest.Request.msg_type.Value = (long)msgType;
            this.SendPdu(asRequest);
            this.testSite.Log.Add(LogEntryKind.Debug, "Send AS Request.");
        }

        /// <summary>
        /// Create and send AS request
        /// </summary>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="seqPaData">A sequence of preauthentication data</param>
        public void SendAsRequest(KdcOptions kdcOptions, Asn1SequenceOf<PA_DATA> seqPaData, KerberosTime from, KerberosTime till)
        {
            string sName = KerberosConstValue.KERBEROS_SNAME;
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname, from, till);
            KerberosAsRequest asRequest = this.CreateAsRequest(kdcReqBody, seqPaData);
            this.SendPdu(asRequest);
            this.testSite.Log.Add(LogEntryKind.Debug, "Send AS Request.");
        }

        /// <summary>
        /// Create and send FAST AS request
        /// </summary>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="innerSeqPaData">A sequence of preauthentication data in FAST request</param>
        /// <param name="outerSeqPaData">A sequence of preauthentication data</param>
        /// <param name="subKey">Sub-session key for authenticator in FAST armor field</param>
        /// <param name="fastOptions">FAST options</param>
        /// <param name="apOptions">AP options in FAST armor field</param>
        public void SendAsRequestWithFast(
            KdcOptions kdcOptions,
            Asn1SequenceOf<PA_DATA> innerSeqPaData,
            Asn1SequenceOf<PA_DATA> outerSeqPaData,
            EncryptionKey subKey,
            FastOptions fastOptions,
            ApOptions apOptions)
        {
            Context.Subkey = subKey;
            string sName = KerberosConstValue.KERBEROS_SNAME;
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));
            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname);
            var pafxfast = CreateAsPaFxFast(subKey, fastOptions, apOptions, innerSeqPaData, sName, kdcReqBody);
            PA_DATA[] elements;
            if (outerSeqPaData != null && outerSeqPaData.Elements.Length > 0)
            {
                elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, elements, outerSeqPaData.Elements.Length);
                elements[outerSeqPaData.Elements.Length] = pafxfast.Data;
            }
            else
            {
                elements = new PA_DATA[] { pafxfast.Data };
            }
            Asn1SequenceOf<PA_DATA> seqPaData = new Asn1SequenceOf<PA_DATA>();
            KerberosAsRequest asRequest = this.CreateAsRequest(kdcReqBody, new Asn1SequenceOf<PA_DATA>(elements));
            this.SendPdu(asRequest);
            this.testSite.Log.Add(LogEntryKind.Debug, "Send AS Request with FAST PA-DATA.");
        }

        /// <summary>
        /// Create and send FAST AS request with unknown armor type
        /// </summary>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="innerSeqPaData">A sequence of preauthentication data in FAST request</param>
        /// <param name="outerSeqPaData">A sequence of preauthentication data</param>
        /// <param name="subKey">Sub-session key for authenticator in FAST armor field</param>
        /// <param name="fastOptions">FAST options</param>
        /// <param name="apOptions">AP options in FAST armor field</param>
        /// <param name="armorType">armorType</param>
        public void SendAsRequestWithFast(
           KdcOptions kdcOptions,
           Asn1SequenceOf<PA_DATA> innerSeqPaData,
           Asn1SequenceOf<PA_DATA> outerSeqPaData,
           EncryptionKey subKey,
           FastOptions fastOptions,
           ApOptions apOptions,
           KrbFastArmorType armorType
           )
        {
            Context.Subkey = subKey;
            string sName = KerberosConstValue.KERBEROS_SNAME;
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));
            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname);
            var pafxfast = CreateAsPaFxFast(subKey, fastOptions, apOptions, innerSeqPaData, sName, kdcReqBody, armorType);
            PA_DATA[] elements;
            if (outerSeqPaData != null && outerSeqPaData.Elements.Length > 0)
            {
                elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, elements, outerSeqPaData.Elements.Length);
                elements[outerSeqPaData.Elements.Length] = pafxfast.Data;
            }
            else
            {
                elements = new PA_DATA[] { pafxfast.Data };
            }
            Asn1SequenceOf<PA_DATA> seqPaData = new Asn1SequenceOf<PA_DATA>();
            KerberosAsRequest asRequest = this.CreateAsRequest(kdcReqBody, new Asn1SequenceOf<PA_DATA>(elements));
            this.SendPdu(asRequest);
            this.testSite.Log.Add(LogEntryKind.Debug, "Send AS Request with FAST PA-DATA.");
        }

        /// <summary>
        /// Create and send FAST AS request
        /// </summary>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="innerSeqPaData">A sequence of preauthentication data in FAST request</param>
        /// <param name="outerSeqPaData">A sequence of preauthentication data</param>
        /// <param name="subKey">Sub-session key for authenticator in FAST armor field</param>
        /// <param name="fastOptions">FAST options</param>
        /// <param name="apOptions">AP options in FAST armor field</param>
        public void SendAsRequestWithFastHideCName(
            KdcOptions kdcOptions,
            string cName,
            Asn1SequenceOf<PA_DATA> innerSeqPaData,
            Asn1SequenceOf<PA_DATA> outerSeqPaData,
            EncryptionKey subKey,
            ApOptions apOptions)
        {
            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(
                KerberosUtility.ConvertInt2Flags((int)FastOptionFlags.Hide_Client_Names));
            Context.Subkey = subKey;
            string sName = KerberosConstValue.KERBEROS_SNAME;
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));
            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname);
            kdcReqBody.cname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_PRINCIPAL), KerberosUtility.String2SeqKerbString(cName));
            var pafxfast = CreateAsPaFxFast(subKey, fastOptions, apOptions, innerSeqPaData, sName, kdcReqBody);
            PA_DATA[] elements;
            if (outerSeqPaData != null && outerSeqPaData.Elements.Length > 0)
            {
                elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, elements, outerSeqPaData.Elements.Length);
                elements[outerSeqPaData.Elements.Length] = pafxfast.Data;
            }
            else
            {
                elements = new PA_DATA[] { pafxfast.Data };
            }
            Asn1SequenceOf<PA_DATA> seqPaData = new Asn1SequenceOf<PA_DATA>();
            KerberosAsRequest asRequest = this.CreateAsRequest(kdcReqBody, new Asn1SequenceOf<PA_DATA>(elements));
            this.SendPdu(asRequest);
            this.testSite.Log.Add(LogEntryKind.Debug, "Send AS Request with FAST PA-DATA.");
        }

        /// <summary>
        /// Recieve an AS request
        /// </summary>
        /// <returns></returns>
        public KerberosAsResponse ExpectAsResponse()
        {
            KerberosPdu responsePdu = this.ExpectPdu(KerberosConstValue.TIMEOUT_DEFAULT, typeof(KerberosAsResponse));
            this.testSite.Assert.IsNotNull(responsePdu, "Response Pdu should not be null.");
            if (responsePdu is KerberosKrbError)
            {
                KerberosKrbError error = responsePdu as KerberosKrbError;
                this.testSite.Log.Add(LogEntryKind.Comment, "ERROR CODE: {0}", error.ErrorCode.ToString());
            }
            this.testSite.Assert.IsInstanceOfType(responsePdu, typeof(KerberosAsResponse), "Response type mismatches");

            KerberosAsResponse response = responsePdu as KerberosAsResponse;
            this.testSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");

            this.testSite.Assume.IsNotNull(Context.ReplyKey, "Reply key should not be null.");
            response.Decrypt(Context.ReplyKey.keyvalue.ByteArrayValue);
            return response;
        }
        #endregion

        #region KrbError

        /// <summary>
        /// Expect an Error
        /// </summary>
        /// <returns></returns>
        public KerberosKrbError ExpectKrbError()
        {
            KerberosPdu responsePdu = this.ExpectPdu(KerberosConstValue.TIMEOUT_DEFAULT, typeof(KerberosKrbError));
            this.testSite.Assert.IsNotNull(responsePdu, "Response Pdu should not be null.");
            this.testSite.Assert.IsInstanceOfType(responsePdu, typeof(KerberosKrbError), "Response type should be KerberosKrbError");

            KerberosKrbError krbError = responsePdu as KerberosKrbError;
            testSite.Log.Add(LogEntryKind.Debug, "Recieve Kerberos Error.");
            return krbError;
        }

        /// <summary>
        /// Expected a KDC_ERR_PREAUTH_REQUIRED error
        /// </summary>
        /// <param name="eData">Error data of this error</param>
        /// <returns></returns>
        public KerberosKrbError ExpectPreauthRequiredError(out METHOD_DATA eData)
        {
            KerberosPdu responsePdu = this.ExpectPdu(KerberosConstValue.TIMEOUT_DEFAULT, typeof(KerberosKrbError));
            this.testSite.Assert.IsNotNull(responsePdu, "Response Pdu should not be null.");
            this.testSite.Assert.IsInstanceOfType(responsePdu, typeof(KerberosKrbError), "Response type should be KerberosKrbError");

            KerberosKrbError krbError = responsePdu as KerberosKrbError;
            this.testSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError.ErrorCode, "The Error code should be KDC_ERR_PREAUTH_REQUIRED");
            this.testSite.Log.Add(LogEntryKind.Debug, "Recieve KDC_ERR_PREAUTH_REQUIRED error.");

            eData = new METHOD_DATA();
            Asn1DecodingBuffer buffer = new Asn1DecodingBuffer(krbError.KrbError.e_data.ByteArrayValue);
            eData.BerDecode(buffer);
            return krbError;
        }
        #endregion

        #region TGS
        /// <summary>
        /// Create and send TGS request
        /// </summary>
        /// <param name="sName">Service principal name</param>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="seqPadata">A sequence of preauthentication data</param>
        /// <param name="checksumType">Checksum type of PA-TGS-REQ</param>
        /// <param name="data">Authorization data</param>
        /// <param name="msgType">Message Type</param>
        public void SendTgsRequest(string sName,
            KdcOptions kdcOptions,
            Asn1SequenceOf<PA_DATA> seqPadata = null,
            AuthorizationData dataInAuthenticator = null,
            AuthorizationData dataInEncAuthData = null,
            MsgType msgType = MsgType.KRB_TGS_REQ)
        {
            if (sName == null)
            {
                throw new ArgumentNullException("sName");
            }
            PrincipalName sname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST),
                KerberosUtility.String2SeqKerbString(sName.Split('/')));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname, dataInEncAuthData);
            Asn1BerEncodingBuffer bodyBuffer = new Asn1BerEncodingBuffer();
            kdcReqBody.BerEncode(bodyBuffer);

            ChecksumType checksumType = KerberosUtility.GetChecksumType(Context.SelectedEType);
            PA_DATA paTgsReq = CreatePaTgsReq(checksumType, bodyBuffer.Data, dataInAuthenticator);

            Asn1SequenceOf<PA_DATA> tempPaData = null;
            if (seqPadata == null || seqPadata.Elements == null || seqPadata.Elements.Length == 0)
            {
                tempPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paTgsReq });
            }
            else
            {
                PA_DATA[] paDatas = new PA_DATA[seqPadata.Elements.Length + 1];
                Array.Copy(seqPadata.Elements, paDatas, seqPadata.Elements.Length);
                paDatas[seqPadata.Elements.Length] = paTgsReq;
                tempPaData = new Asn1SequenceOf<PA_DATA>(paDatas);
            }

            KerberosTgsRequest tgsRequest = new KerberosTgsRequest(KerberosConstValue.KERBEROSV5, kdcReqBody, tempPaData, Context.TransportType);
            tgsRequest.Request.msg_type.Value = (long)msgType;
            this.SendPdu(tgsRequest);
            this.testSite.Log.Add(LogEntryKind.Comment, "Send TGS request.");
        }

        /// <summary>
        /// Create and send TGS request
        /// </summary>
        /// <param name="sName">Service principal name</param>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="msgType">Message Type</param>
        public void SendTgsRequest(string sName,
            KdcOptions kdcOptions,
            MsgType msgType)
        {
            SendTgsRequest(sName, kdcOptions, null, null, null, msgType);
        }

        /// <summary>
        /// Create and send FAST TGS request
        /// </summary>
        /// <param name="sName">Service principal name</param>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="innerSeqPaData">A sequence of preauthentication data in FAST request</param>
        /// <param name="outerSeqPaData">A sequence of preauthentication data</param>
        /// <param name="subKey">Sub-session key for authenticator in FAST armor field</param>
        /// <param name="fastOptions">FAST options</param>
        /// <param name="apOptions">AP options in FAST armor field</param>
        /// <param name="data">Authorizaion data</param>
        public void SendTgsRequestWithFast(
            string sName,
            KdcOptions kdcOptions,
            Asn1SequenceOf<PA_DATA> innerSeqPaData,
            Asn1SequenceOf<PA_DATA> outerSeqPaData,
            EncryptionKey subKey,
            FastOptions fastOptions,
            ApOptions apOptions,
            AuthorizationData data = null)
        {
            Context.Subkey = subKey;
            Context.ReplyKey = subKey;
            string domain = this.Context.Realm.Value;
            PrincipalName sname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST),
                KerberosUtility.String2SeqKerbString(sName.Split('/')));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname, data);
            Asn1BerEncodingBuffer bodyBuffer = new Asn1BerEncodingBuffer();
            kdcReqBody.BerEncode(bodyBuffer);

            APOptions option = new APOptions(KerberosUtility.ConvertInt2Flags((int)ApOptions.None));
            ChecksumType checksumType = KerberosUtility.GetChecksumType(Context.SelectedEType);
            KerberosApRequest apRequest = CreateApRequest(
                option,
                Context.Ticket,
                subKey,
                data,
                KeyUsageNumber.TG_REQ_PA_TGS_REQ_padataOR_AP_REQ_Authenticator,
                checksumType,
                bodyBuffer.Data);

            PaTgsReq paTgsReq = new PaTgsReq(apRequest.Request);

            Asn1SequenceOf<PA_DATA> tempPaData = null;
            if (outerSeqPaData == null || outerSeqPaData.Elements == null || outerSeqPaData.Elements.Length == 0)
            {
                tempPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paTgsReq.Data });
            }
            else
            {
                tempPaData.Elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, tempPaData.Elements, outerSeqPaData.Elements.Length);
                tempPaData.Elements[outerSeqPaData.Elements.Length] = paTgsReq.Data;
            }
            var armorKey = GetArmorKey(Context.SessionKey, subKey);
            var pafxfast = CreateTgsPaFxFast(armorKey, Context.Ticket, fastOptions, apOptions, tempPaData, sName, paTgsReq.Data.padata_value.ByteArrayValue);
            Context.FastArmorkey = armorKey;
            PA_DATA[] elements;
            if (outerSeqPaData != null && outerSeqPaData.Elements.Length > 0)
            {
                elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, elements, outerSeqPaData.Elements.Length);
                elements[outerSeqPaData.Elements.Length] = pafxfast.Data;
                elements[outerSeqPaData.Elements.Length + 1] = paTgsReq.Data;
            }
            else
            {
                elements = new PA_DATA[] { pafxfast.Data, paTgsReq.Data };
            }
            Asn1SequenceOf<PA_DATA> seqPaData = new Asn1SequenceOf<PA_DATA>();
            KerberosTgsRequest tgsRequest = new KerberosTgsRequest(KerberosConstValue.KERBEROSV5, kdcReqBody, new Asn1SequenceOf<PA_DATA>(elements), Context.TransportType);

            this.SendPdu(tgsRequest);
            this.testSite.Log.Add(LogEntryKind.Comment, "Send FAST TGS request.");
        }

        /// <summary>
        /// Create and send FAST TGS request
        /// </summary>
        /// <param name="sName">Service principal name</param>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="innerSeqPaData">A sequence of preauthentication data in FAST request</param>
        /// <param name="outerSeqPaData">A sequence of preauthentication data</param>
        /// <param name="subKey">Sub-session key for authenticator in FAST armor field</param>
        /// <param name="fastOptions">FAST options</param>
        /// <param name="apOptions">AP options in FAST armor field</param>
        /// <param name="data">Authorizaion data</param>
        public void SendTgsRequestWithFastHideCName(
            string sName,
            PrincipalName cName,
            KdcOptions kdcOptions,
            Asn1SequenceOf<PA_DATA> innerSeqPaData,
            Asn1SequenceOf<PA_DATA> outerSeqPaData,
            EncryptionKey subKey,
            ApOptions apOptions,
            AuthorizationData data = null)
        {
            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(
                KerberosUtility.ConvertInt2Flags((int)FastOptionFlags.Hide_Client_Names));
            Context.Subkey = subKey;
            Context.ReplyKey = subKey;
            string domain = this.Context.Realm.Value;
            PrincipalName sname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST),
                KerberosUtility.String2SeqKerbString(sName.Split('/')));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname, data);
            kdcReqBody.cname = cName;
            Asn1BerEncodingBuffer bodyBuffer = new Asn1BerEncodingBuffer();
            kdcReqBody.BerEncode(bodyBuffer);

            APOptions option = new APOptions(KerberosUtility.ConvertInt2Flags((int)ApOptions.None));
            ChecksumType checksumType = KerberosUtility.GetChecksumType(Context.SelectedEType);
            KerberosApRequest apRequest = CreateApRequest(
                option,
                Context.Ticket,
                subKey,
                data,
                KeyUsageNumber.TG_REQ_PA_TGS_REQ_padataOR_AP_REQ_Authenticator,
                checksumType,
                bodyBuffer.Data);

            PaTgsReq paTgsReq = new PaTgsReq(apRequest.Request);

            Asn1SequenceOf<PA_DATA> tempPaData = null;
            if (outerSeqPaData == null || outerSeqPaData.Elements == null || outerSeqPaData.Elements.Length == 0)
            {
                tempPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paTgsReq.Data });
            }
            else
            {
                tempPaData.Elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, tempPaData.Elements, outerSeqPaData.Elements.Length);
                tempPaData.Elements[outerSeqPaData.Elements.Length] = paTgsReq.Data;
            }
            var armorKey = GetArmorKey(Context.SessionKey, subKey);
            var pafxfast = CreateTgsPaFxFast(armorKey, Context.Ticket, fastOptions, apOptions, tempPaData, sName, paTgsReq.Data.padata_value.ByteArrayValue);
            Context.FastArmorkey = armorKey;
            PA_DATA[] elements;
            if (outerSeqPaData != null && outerSeqPaData.Elements.Length > 0)
            {
                elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, elements, outerSeqPaData.Elements.Length);
                elements[outerSeqPaData.Elements.Length] = pafxfast.Data;
                elements[outerSeqPaData.Elements.Length + 1] = paTgsReq.Data;
            }
            else
            {
                elements = new PA_DATA[] { pafxfast.Data, paTgsReq.Data };
            }
            Asn1SequenceOf<PA_DATA> seqPaData = new Asn1SequenceOf<PA_DATA>();
            KerberosTgsRequest tgsRequest = new KerberosTgsRequest(KerberosConstValue.KERBEROSV5, kdcReqBody, new Asn1SequenceOf<PA_DATA>(elements), Context.TransportType);

            this.SendPdu(tgsRequest);
            this.testSite.Log.Add(LogEntryKind.Comment, "Send FAST TGS request.");
        }

        /// <summary>
        /// Create and send FAST TGS request
        /// </summary>
        /// <param name="sName">Service principal name</param>
        /// <param name="kdcOptions">KDC options</param>
        /// <param name="innerSeqPaData">A sequence of preauthentication data in FAST request</param>
        /// <param name="outerSeqPaData">A sequence of preauthentication data</param>
        /// <param name="subKey">Sub-session key for authenticator in FAST armor field</param>
        /// <param name="fastOptions">FAST options</param>
        /// <param name="apOptions">AP options in FAST armor field</param>
        /// <param name="data">Authorizaion data</param>
        public void SendTgsRequestWithExplicitFast(
            string sName,
            KdcOptions kdcOptions,
            Asn1SequenceOf<PA_DATA> innerSeqPaData,
            Asn1SequenceOf<PA_DATA> outerSeqPaData,
            EncryptionKey subKey,
            FastOptions fastOptions,
            ApOptions apOptions,
            AuthorizationData data = null)
        {
            Context.Subkey = subKey;
            Context.ReplyKey = subKey;
            string domain = this.Context.Realm.Value;
            PrincipalName sname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST),
                KerberosUtility.String2SeqKerbString(sName.Split('/')));

            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sname, data);
            Asn1BerEncodingBuffer bodyBuffer = new Asn1BerEncodingBuffer();
            kdcReqBody.BerEncode(bodyBuffer);

            //Create PA-TGS-REQ
            APOptions option = new APOptions(KerberosUtility.ConvertInt2Flags((int)ApOptions.None));
            ChecksumType checksumType = KerberosUtility.GetChecksumType(Context.SelectedEType);
            KerberosApRequest apRequest = CreateApRequest(
                option,
                Context.Ticket,
                subKey,
                data,
                KeyUsageNumber.TG_REQ_PA_TGS_REQ_padataOR_AP_REQ_Authenticator,
                checksumType,
                bodyBuffer.Data);

            PaTgsReq paTgsReq = new PaTgsReq(apRequest.Request);

            Asn1SequenceOf<PA_DATA> tempPaData = null;
            if (outerSeqPaData == null || outerSeqPaData.Elements == null || outerSeqPaData.Elements.Length == 0)
            {
                tempPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paTgsReq.Data });
            }
            else
            {
                tempPaData.Elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, tempPaData.Elements, outerSeqPaData.Elements.Length);
                tempPaData.Elements[outerSeqPaData.Elements.Length] = paTgsReq.Data;
            }
            //Create explicit FAST armor
            EncryptionKey explicitSubkey = KerberosUtility.MakeKey(
                Context.SelectedEType,
                "Password04!",
                "This is a salt");
            Authenticator plaintextAuthenticator = CreateAuthenticator(Context.ArmorTicket, null, explicitSubkey);
            KerberosApRequest apReq = new KerberosApRequest(Context.Pvno,
                new APOptions(KerberosUtility.ConvertInt2Flags((int)apOptions)),
                Context.ArmorTicket,
                plaintextAuthenticator,
                KeyUsageNumber.AP_REQ_Authenticator);
            FastArmorApRequest explicitArmor = new FastArmorApRequest(apReq.Request);

            //Create armor key
            var armorKey = GetArmorKey(Context.ArmorSessionKey, subKey, explicitSubkey);
            Context.FastArmorkey = armorKey;

            //Create PA-FX-FAST
            var pafxfast = CreateTgsPaFxFast(armorKey, Context.ArmorTicket, fastOptions, apOptions, tempPaData, sName, paTgsReq.Data.padata_value.ByteArrayValue, explicitArmor);

            PA_DATA[] elements;
            if (outerSeqPaData != null && outerSeqPaData.Elements.Length > 0)
            {
                elements = new PA_DATA[outerSeqPaData.Elements.Length + 1];
                Array.Copy(outerSeqPaData.Elements, elements, outerSeqPaData.Elements.Length);
                elements[outerSeqPaData.Elements.Length] = pafxfast.Data;
                elements[outerSeqPaData.Elements.Length + 1] = paTgsReq.Data;
            }
            else
            {
                elements = new PA_DATA[] { pafxfast.Data, paTgsReq.Data };
            }
            Asn1SequenceOf<PA_DATA> seqPaData = new Asn1SequenceOf<PA_DATA>();
            KerberosTgsRequest tgsRequest = new KerberosTgsRequest(KerberosConstValue.KERBEROSV5, kdcReqBody, new Asn1SequenceOf<PA_DATA>(elements), Context.TransportType);

            this.SendPdu(tgsRequest);
            this.testSite.Log.Add(LogEntryKind.Comment, "Send FAST TGS request.");
        }

        /// <summary>
        /// Recieve a TGS response
        /// </summary>
        /// <param name="usage">Key usage number to decrypt TGS response</param>
        /// <returns></returns>
        public KerberosTgsResponse ExpectTgsResponse(KeyUsageNumber usage = KeyUsageNumber.TGS_REP_encrypted_part)
        {
            var response = this.ExpectPdu(KerberosConstValue.TIMEOUT_DEFAULT, typeof(KerberosTgsResponse));
            this.testSite.Assert.IsNotNull(response, "Response should not be null");
            this.testSite.Assert.IsInstanceOfType(response, typeof(KerberosTgsResponse), "Response type mismatches");
            KerberosTgsResponse tgsResponse = response as KerberosTgsResponse;
            this.testSite.Log.Add(LogEntryKind.Comment, "Recieve TGS response.");
            this.testSite.Assume.IsNotNull(Context.ReplyKey, "Reply key should not be null.");
            tgsResponse.DecryptTgsResponse(Context.ReplyKey.keyvalue.ByteArrayValue, usage);
            return tgsResponse;
        }

        #endregion

        #region AP
        /// <summary>
        /// Create AP request and encode to GSSAPI token
        /// </summary>
        /// <param name="apOptions">AP options</param>
        /// <param name="data">Authorization data</param>
        /// <param name="subkey">Sub-session key in authenticator</param>
        /// <param name="checksumFlags">Checksum flags</param>
        /// <returns></returns>
        public byte[] CreateGssApiToken(ApOptions apOptions, AuthorizationData data, EncryptionKey subkey, ChecksumFlags checksumFlags,
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerberosConstValue.GSSToken gssToken = KerberosConstValue.GSSToken.GSSSPNG)
        {
            APOptions options = new APOptions(KerberosUtility.ConvertInt2Flags((int)apOptions));

            Authenticator authenticator = CreateAuthenticator(Context.Ticket, data, subkey, checksumFlags);

            KerberosApRequest request = new KerberosApRequest(
                Context.Pvno,
                options,
                Context.Ticket,
                authenticator,
                KeyUsageNumber.AP_REQ_Authenticator
                );

            return KerberosUtility.AddGssApiTokenHeader(request, this.oidPkt, gssToken);
        }

        /// <summary>
        /// Decode GSSAPI token to AP-REP
        /// </summary>
        /// <param name="token">GSSAPI token</param>
        /// <returns></returns>
        public KerberosApResponse GetApResponseFromToken(byte[] token, KerberosConstValue.GSSToken gssToken = KerberosConstValue.GSSToken.GSSSPNG)
        {
            if (gssToken == KerberosConstValue.GSSToken.GSSSPNG)
                token = KerberosUtility.DecodeNegotiationToken(token);

            if (token[0] == KerberosConstValue.KERBEROS_TAG)
            {
                byte[] apData = KerberosUtility.VerifyGssApiTokenHeader(token, this.oidPkt);

                // Check if it has a two-byte tok_id
                if (null == apData || apData.Length <= sizeof(TOK_ID))
                {
                    throw new FormatException(
                        "Data length is shorter than a valid AP Response data length.");
                }

                // verify TOK_ID
                byte[] tokenID = ArrayUtility.SubArray<byte>(apData, 0, sizeof(TOK_ID));
                Array.Reverse(tokenID);
                TOK_ID id = (TOK_ID)BitConverter.ToUInt16(tokenID, 0);

                this.testSite.Assert.AreEqual(TOK_ID.KRB_AP_REP, id, "The Token ID should be KRB_AP_REP");

                // Get apBody
                token = ArrayUtility.SubArray(apData, sizeof(TOK_ID));
            }

            KerberosApResponse response = new KerberosApResponse();
            response.FromBytes(token);

            return response;
        }

        public KerberosKrbError GetKrbErrorFromToken(byte[] token)
        {
            token = KerberosUtility.DecodeNegotiationToken(token);

            if (token[0] == KerberosConstValue.KERBEROS_TAG)
            {
                byte[] apData = KerberosUtility.VerifyGssApiTokenHeader(token, this.oidPkt);

                // Check if it has a two-byte tok_id
                if (null == apData || apData.Length <= sizeof(TOK_ID))
                {
                    throw new FormatException(
                        "Data length is shorter than a valid AP Response data length.");
                }

                // verify TOK_ID
                byte[] tokenID = ArrayUtility.SubArray<byte>(apData, 0, sizeof(TOK_ID));
                Array.Reverse(tokenID);
                TOK_ID id = (TOK_ID)BitConverter.ToUInt16(tokenID, 0);

                this.testSite.Assert.AreEqual(TOK_ID.KRB_ERROR, id, "The Token ID should be KRB_ERROR");

                // Get apBody
                token = ArrayUtility.SubArray(apData, sizeof(TOK_ID));
            }

            KerberosKrbError error = new KerberosKrbError();
            error.FromBytes(token);

            return error;
        }

        #endregion

        #region FAST
        public PaFxFastReq CreateAsPaFxFast(
            EncryptionKey subKey,
            FastOptions fastOptions,
            ApOptions apOptions,
            Asn1SequenceOf<PA_DATA> seqPaData,
            string sName,
            KDC_REQ_BODY kdcReqBody
            )
        {
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));

            var armorKey = KerberosUtility.MakeArmorKey(
                Context.SelectedEType,
                subKey.keyvalue.ByteArrayValue,
                Context.ArmorSessionKey.keyvalue.ByteArrayValue);
            Context.FastArmorkey = new EncryptionKey(new KerbInt32((long)Context.SelectedEType), new Asn1OctetString(armorKey));


            Asn1BerEncodingBuffer encodebuf = new Asn1BerEncodingBuffer();
            kdcReqBody.BerEncode(encodebuf);
            var checksumType = KerberosUtility.GetChecksumType(Context.SelectedEType);
            var chksum = KerberosUtility.GetChecksum(
                armorKey,
                encodebuf.Data,
                (int)KeyUsageNumber.FAST_REQ_CHECKSUM,
                checksumType);
            Checksum checkSum = new Checksum(new KerbInt32((int)checksumType), new Asn1OctetString(chksum));

            Authenticator plaintextAuthenticator = CreateAuthenticator(Context.ArmorTicket, null, subKey);

            KerberosApRequest apReq = new KerberosApRequest(Context.Pvno,
                new APOptions(KerberosUtility.ConvertInt2Flags((int)apOptions)),
                Context.ArmorTicket,
                plaintextAuthenticator,
                KeyUsageNumber.AP_REQ_Authenticator);

            KDC_REQ_BODY innerKdcReqBody = CreateKdcRequestBody(KdcOptions.CANONICALIZE | KdcOptions.FORWARDABLE | KdcOptions.RENEWABLE, sname);
            KerberosFastRequest fastReq = new KerberosFastRequest(fastOptions, seqPaData, innerKdcReqBody);
            FastArmorApRequest fastArmor = new FastArmorApRequest(apReq.Request);
            KerberosArmoredRequest armoredReq
                = new KerberosArmoredRequest(fastArmor, checkSum, (long)Context.SelectedEType, armorKey, fastReq);
            PA_FX_FAST_REQUEST paFxFastReq = new PA_FX_FAST_REQUEST();
            paFxFastReq.SetData(PA_FX_FAST_REQUEST.armored_data, armoredReq.FastArmoredReq);
            PaFxFastReq paFxfast = new PaFxFastReq(paFxFastReq);
            return paFxfast;
        }

        public PaFxFastReq CreateAsPaFxFast(
        EncryptionKey subKey,
        FastOptions fastOptions,
        ApOptions apOptions,
        Asn1SequenceOf<PA_DATA> seqPaData,
        string sName,
        KDC_REQ_BODY kdcReqBody,
        KrbFastArmorType armorType
        )
        {
            string domain = this.Context.Realm.Value;
            PrincipalName sname =
                new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KerberosUtility.String2SeqKerbString(sName, domain));

            var armorKey = KerberosUtility.MakeArmorKey(
                Context.SelectedEType,
                subKey.keyvalue.ByteArrayValue,
                Context.ArmorSessionKey.keyvalue.ByteArrayValue);
            Context.FastArmorkey = new EncryptionKey(new KerbInt32((long)Context.SelectedEType), new Asn1OctetString(armorKey));


            Asn1BerEncodingBuffer encodebuf = new Asn1BerEncodingBuffer();
            kdcReqBody.BerEncode(encodebuf);
            var checksumType = KerberosUtility.GetChecksumType(Context.SelectedEType);
            var chksum = KerberosUtility.GetChecksum(
                armorKey,
                encodebuf.Data,
                (int)KeyUsageNumber.FAST_REQ_CHECKSUM,
                checksumType);
            Checksum checkSum = new Checksum(new KerbInt32((int)checksumType), new Asn1OctetString(chksum));

            Authenticator plaintextAuthenticator = CreateAuthenticator(Context.ArmorTicket, null, subKey);

            KerberosApRequest apReq = new KerberosApRequest(Context.Pvno,
                new APOptions(KerberosUtility.ConvertInt2Flags((int)apOptions)),
                Context.ArmorTicket,
                plaintextAuthenticator,
                KeyUsageNumber.AP_REQ_Authenticator);

            KDC_REQ_BODY innerKdcReqBody = CreateKdcRequestBody(KdcOptions.CANONICALIZE | KdcOptions.FORWARDABLE | KdcOptions.RENEWABLE, sname);
            KerberosFastRequest fastReq = new KerberosFastRequest(fastOptions, seqPaData, innerKdcReqBody);
            FastArmorApRequest fastArmor = new FastArmorApRequest(apReq.Request);
            fastArmor.armorType = armorType;
            KerberosArmoredRequest armoredReq
                = new KerberosArmoredRequest(fastArmor, checkSum, (long)Context.SelectedEType, armorKey, fastReq);
            PA_FX_FAST_REQUEST paFxFastReq = new PA_FX_FAST_REQUEST();
            paFxFastReq.SetData(PA_FX_FAST_REQUEST.armored_data, armoredReq.FastArmoredReq);
            PaFxFastReq paFxfast = new PaFxFastReq(paFxFastReq);
            return paFxfast;
        }

        public PaFxFastReq CreateTgsPaFxFast(
            EncryptionKey armorKey,
            KerberosTicket armorTicket,
            FastOptions fastOptions,
            ApOptions apOptions,
            Asn1SequenceOf<PA_DATA> seqPaData,
            string sName,
            byte[] apReq,
            IFastArmor armor = null
           )
        {
            string domain = this.Context.Realm.Value;
            PrincipalName sname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST),
                KerberosUtility.String2SeqKerbString(sName.Split('/')));

            KDC_REQ_BODY innerKdcReqBody = CreateKdcRequestBody(KdcOptions.CANONICALIZE | KdcOptions.FORWARDABLE | KdcOptions.RENEWABLE, sname);

            //Generate checksum
            var checksumType = KerberosUtility.GetChecksumType(Context.SelectedEType);
            var chksum = KerberosUtility.GetChecksum(
                armorKey.keyvalue.ByteArrayValue,
                apReq,
                (int)KeyUsageNumber.FAST_REQ_CHECKSUM,
                checksumType);
            Checksum checkSum = new Checksum(new KerbInt32((int)checksumType), new Asn1OctetString(chksum));

            KerberosFastRequest fastReq = new KerberosFastRequest(fastOptions, seqPaData, innerKdcReqBody);
            KerberosArmoredRequest armoredReq
                = new KerberosArmoredRequest(armor, checkSum, (long)Context.SelectedEType, armorKey.keyvalue.ByteArrayValue, fastReq);
            PA_FX_FAST_REQUEST paFxFastReq = new PA_FX_FAST_REQUEST();
            paFxFastReq.SetData(PA_FX_FAST_REQUEST.armored_data, armoredReq.FastArmoredReq);
            PaFxFastReq paFxfast = new PaFxFastReq(paFxFastReq);
            return paFxfast;
        }

        #endregion

        #region private method
        private KerberosAsRequest CreateAsRequest(KDC_REQ_BODY kdcReqBody, Asn1SequenceOf<PA_DATA> paDatas, long pvno = KerberosConstValue.KERBEROSV5)
        {
            KerberosAsRequest request = new KerberosAsRequest(
                pvno,
                kdcReqBody,
                paDatas,
                Context.TransportType);
            return request;
        }

        private KerberosApRequest CreateApRequest(APOptions option, KerberosTicket ticket, EncryptionKey subkey, AuthorizationData data, KeyUsageNumber keyUsageNumber, ChecksumFlags checksumFlag)
        {
            Authenticator authenticator = CreateAuthenticator(ticket, data, subkey, checksumFlag);
            KerberosApRequest apReq = new KerberosApRequest(Context.Pvno, option, ticket, authenticator, keyUsageNumber);
            return apReq;
        }

        private KerberosApRequest CreateApRequest(APOptions option, KerberosTicket ticket, EncryptionKey subkey, AuthorizationData data, KeyUsageNumber keyUsageNumber, ChecksumType checksumType, byte[] checksumBody)
        {
            Authenticator authenticator = CreateAuthenticator(ticket, data, subkey, checksumType, checksumBody);
            KerberosApRequest apReq = new KerberosApRequest(Context.Pvno, option, ticket, authenticator, keyUsageNumber);
            return apReq;
        }

        private PA_DATA CreatePaTgsReq(ChecksumType checksumType, byte[] checksumBody, AuthorizationData data)
        {
            APOptions option = new APOptions(KerberosUtility.ConvertInt2Flags((int)ApOptions.None));
            EncryptionKey key = Context.SessionKey;
            EncryptionKey subkey = null;
            Ticket ticket = Context.Ticket.Ticket;
            KerberosApRequest apRequest = CreateApRequest(option, Context.Ticket, subkey, data, KeyUsageNumber.TG_REQ_PA_TGS_REQ_padataOR_AP_REQ_Authenticator, checksumType, checksumBody);

            PaTgsReq paTgsReq = new PaTgsReq(apRequest.Request);
            return paTgsReq.Data;
        }

        private Authenticator CreateAuthenticator(
            KerberosTicket ticket,
            AuthorizationData data,
            EncryptionKey subkey
            )
        {
            Authenticator plaintextAuthenticator = new Authenticator();
            plaintextAuthenticator.authenticator_vno = new Asn1Integer(KerberosConstValue.KERBEROSV5);
            plaintextAuthenticator.crealm = Context.CName.Realm;
            plaintextAuthenticator.cusec = new Microseconds(0);
            plaintextAuthenticator.ctime = KerberosUtility.CurrentKerberosTime;
            plaintextAuthenticator.seq_number = new Protocols.TestTools.StackSdk.Security.KerberosLib.KerbUInt32(0);
            plaintextAuthenticator.cname = ticket.TicketOwner;
            plaintextAuthenticator.subkey = subkey;
            plaintextAuthenticator.authorization_data = data;
            return plaintextAuthenticator;
        }

        private Authenticator CreateAuthenticator(
            KerberosTicket ticket,
            AuthorizationData data,
            EncryptionKey subkey,
            ChecksumType checksumType,
            byte[] checksumBody
            )
        {
            Authenticator plaintextAuthenticator = CreateAuthenticator(ticket, data, subkey);

            byte[] checkData = KerberosUtility.GetChecksum(ticket.SessionKey.keyvalue.ByteArrayValue,
                checksumBody, (int)KeyUsageNumber.TGS_REQ_PA_TGS_REQ_adataOR_AP_REQ_Authenticator_cksum, checksumType);

            plaintextAuthenticator.cksum = new Checksum(new KerbInt32((int)checksumType), new Asn1OctetString(checkData));
            return plaintextAuthenticator;
        }

        /// <summary>
        /// Create authenticator for ChecksumType.ap_authenticator_8003
        /// </summary>
        private Authenticator CreateAuthenticator(
            KerberosTicket ticket,
            AuthorizationData data,
            EncryptionKey subkey,
            ChecksumFlags checksumFlag
            )
        {
            Authenticator plaintextAuthenticator = CreateAuthenticator(ticket, data, subkey);

            AuthCheckSum checksum = new AuthCheckSum();
            checksum.Lgth = KerberosConstValue.AUTHENTICATOR_CHECKSUM_LENGTH;
            checksum.Bnd = new byte[checksum.Lgth];
            checksum.Flags = (int)checksumFlag;
            byte[] checkData = ArrayUtility.ConcatenateArrays(BitConverter.GetBytes(checksum.Lgth),
                checksum.Bnd, BitConverter.GetBytes(checksum.Flags));

            plaintextAuthenticator.cksum = new Checksum(new KerbInt32((int)ChecksumType.ap_authenticator_8003), new Asn1OctetString(checkData));
            return plaintextAuthenticator;
        }

        private KDC_REQ_BODY CreateKdcRequestBody(
            KdcOptions kdcOptions,
            PrincipalName sName
            )
        {

            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbUInt32 nonce =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbUInt32((uint)Math.Abs((int)DateTime.Now.Ticks));
            KerberosTime till = new KerberosTime(KerberosConstValue.TGT_TILL_TIME);
            KerberosTime rtime = new KerberosTime(KerberosConstValue.TGT_RTIME);
            HostAddresses addresses =
                new HostAddresses(new HostAddress[1] { new HostAddress(new KerbInt32((int)AddressType.NetBios),
                    new Asn1OctetString(Encoding.ASCII.GetBytes(System.Net.Dns.GetHostName()))) });

            KDCOptions options = new KDCOptions(KerberosUtility.ConvertInt2Flags((int)kdcOptions));

            KDC_REQ_BODY kdcReqBody = new KDC_REQ_BODY(options, Context.CName.Name, Context.Realm, sName, null, till, rtime, nonce, Context.SupportedEType, addresses, null, null);
            return kdcReqBody;
        }

        private KDC_REQ_BODY CreateKdcRequestBody(
            KdcOptions kdcOptions,
            PrincipalName sName,
            KerberosTime from,
            KerberosTime till
            )
        {

            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbUInt32 nonce =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbUInt32((uint)Math.Abs((int)DateTime.Now.Ticks));
            KerberosTime rtime = new KerberosTime(KerberosConstValue.TGT_RTIME);
            HostAddresses addresses =
                new HostAddresses(new HostAddress[1] { new HostAddress(new KerbInt32((int)AddressType.NetBios),
                    new Asn1OctetString(Encoding.ASCII.GetBytes(System.Net.Dns.GetHostName()))) });

            KDCOptions options = new KDCOptions(KerberosUtility.ConvertInt2Flags((int)kdcOptions));

            KDC_REQ_BODY kdcReqBody = new KDC_REQ_BODY(options, Context.CName.Name, Context.Realm, sName, from, till, rtime, nonce, Context.SupportedEType, addresses, null, null);
            return kdcReqBody;
        }

        private KDC_REQ_BODY CreateKdcRequestBody(
            KdcOptions kdcOptions,
            PrincipalName sName,
            AuthorizationData data = null
            )
        {
            KDC_REQ_BODY kdcReqBody = CreateKdcRequestBody(kdcOptions, sName);

            if (data == null)
                return kdcReqBody;

            Asn1BerEncodingBuffer asnBuffer = new Asn1BerEncodingBuffer();
            // Fix me: NullReferenceException if data is null. 
            if (data != null) data.BerEncode(asnBuffer, true);

            EncryptedData eData = new EncryptedData();
            eData.etype = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32(0);
            byte[] encAsnEncoded = asnBuffer.Data;
            if (Context.SessionKey != null && Context.SessionKey.keytype != null
                && Context.SessionKey.keyvalue != null && Context.SessionKey.keyvalue.Value != null)
            {
                encAsnEncoded = KerberosUtility.Encrypt((EncryptionType)Context.SessionKey.keytype.Value,
                                                    Context.SessionKey.keyvalue.ByteArrayValue,
                                                    asnBuffer.Data,
                                                    (int)KeyUsageNumber.TGS_REQ_KDC_REQ_BODY_AuthorizationData);
                eData.etype =
                    new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32(Context.SessionKey.keytype.Value);
            }
            eData.cipher = new Asn1OctetString(encAsnEncoded);
            kdcReqBody.enc_authorization_data = eData;

            return kdcReqBody;
        }

        private static EncryptionKey GetArmorKey(EncryptionKey sessionKey, EncryptionKey subKey, EncryptionKey explicitSubkey = null)
        {
            EncryptionKey armorKey;
            if (explicitSubkey != null)
            {
                armorKey = KerberosUtility.KrbFxCf2(explicitSubkey, sessionKey, "subkeyarmor", "ticketarmor");
                armorKey = KerberosUtility.KrbFxCf2(armorKey, subKey, "explicitarmor", "tgsarmor");
            }
            else
            {
                armorKey = KerberosUtility.KrbFxCf2(subKey, sessionKey, "subkeyarmor", "ticketarmor");
            }

            return armorKey;
        }
        #endregion
    }
}
