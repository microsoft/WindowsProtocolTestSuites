// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// Base class of KileClient and KileServer.
    /// Maintain the common functions.
    /// </summary>
    public abstract class KileRole : IDisposable
    {
        #region Private Members
        /// <summary>
        /// Represents whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        internal abstract KileContext Context
        {
            get;
        }

        #endregion

        #region Packet api

        /// <summary>
        /// Create AP response. This method is used for mutual authentication.
        /// This method is used on the client side.
        /// </summary>
        /// <param name="subkey">Specify the new subkey used in the following exchange. This field is optional.
        /// This argument can be got with method GenerateKey(ApSessionKey).</param>
        /// <returns>The created AP response.</returns>
        [CLSCompliant(false)]
        public KileApResponse CreateApResponse(EncryptionKey subkey)
        {
            var response = new KileApResponse(Context);

            // Set AP_REP
            response.Response.msg_type = new Asn1Integer((int)MsgType.KRB_AP_RESP);
            response.Response.pvno = new Asn1Integer(ConstValue.KERBEROSV5);

            // Set EncAPRepPart
            var apEncPart = new EncAPRepPart();
            apEncPart.ctime = Context.Time;
            apEncPart.cusec = Context.Cusec;
            apEncPart.subkey = subkey;
            apEncPart.seq_number = new KerbUInt32((long)Context.currentRemoteSequenceNumber);
            response.ApEncPart = apEncPart;

            return response;
        }


        /// <summary>
        /// Generate client account's salt.
        /// </summary>
        /// <param name="domain">The realm part of the client's principal identifier.
        /// This argument cannot be null.</param>
        /// <param name="cName">The account to logon the remote machine. Either user account or computer account
        /// This argument cannot be null.</param>
        /// <param name="accountType">The type of the logon account. User or Computer</param>
        /// <returns>Client account's salt</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when the account type is neither user nor computer.
        /// </exception>
        public static string GenerateSalt(string domain, string cName, KileAccountType accountType)
        {
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }
            if (cName == null)
            {
                throw new ArgumentNullException(nameof(cName));
            }
            string salt;

            if (accountType == KileAccountType.User)
            {
                salt = domain.ToUpper() + cName;
            }
            else if (accountType == KileAccountType.Computer)
            {
                string computerName = cName;

                if (cName.EndsWith("$"))
                {
                    computerName = cName.Substring(0, cName.Length - 1);
                }
                salt = domain.ToUpper() + "host" + computerName.ToLower() + "." + domain.ToLower();
            }
            else
            {
                throw new NotSupportedException("Kile only support user or computer account.");
            }

            return salt;
        }

        #endregion


        #region Auth Data

        /// <summary>
        /// Construct AuthorizationData. User can add the AuthData they are interested in.
        /// All the types of AuthData can be created by the user themselves. 
        /// For example, to create a PAC type AuthData, user could use PAC data to new a PacAuthData type.
        /// </summary>
        /// <param name="authData">The AuthData user wants to construct. This argument can be null.</param>
        /// <returns>The constructed AuthData.</returns>
        [CLSCompliant(false)]
        public AuthorizationData ConstructAuthorizationData(params AuthData[] authData)
        {
            if (authData == null || authData.Length == 0)
            {
                return null;
            }

            var authList = new List<AuthorizationDataElement>();
            for (int i = 0; i < authData.Length; ++i)
            {
                if (authData[i] == null)
                {
                    continue;
                }

                switch (authData[i].AdType)
                {
                    case AuthorizationData_elementType.AD_AUTH_DATA_AP_OPTIONS:
                        AdAuthDataApOptions auth = (AdAuthDataApOptions)authData[i];
                        var element = new AuthorizationDataElement[1];
                        element[0] = new AuthorizationDataElement(new KerbInt32((int)auth.AdType),
                            new Asn1OctetString("0x" + auth.Options.ToString("x")));

                        AD_IF_RELEVANT adIf = new AD_IF_RELEVANT();
                        adIf.Elements = element;
                        Asn1BerEncodingBuffer adIFbuf = new Asn1BerEncodingBuffer();
                        adIf.BerEncode(adIFbuf, true);

                        authList.Add(new AuthorizationDataElement(new KerbInt32((int)AuthorizationData_elementType.AD_IF_RELEVANT),
                                                                 new Asn1OctetString(adIFbuf.Data)));
                        break;

                    case AuthorizationData_elementType.KERB_AUTH_DATA_TOKEN_RESTRICTIONS:
                        KerbAuthDataTokenRestrictions kerbAuth = (KerbAuthDataTokenRestrictions)authData[i];
                        KERB_AD_RESTRICTION_ENTRY entry = new KERB_AD_RESTRICTION_ENTRY();
                        entry.restriction_type = new KerbInt32(kerbAuth.RestrictionType);
                        Asn1OctetString machineId = new Asn1OctetString(kerbAuth.MachineId);
                        entry.restriction = machineId;
                            //new LSAP_TOKEN_INFO_INTEGRITY(new KerbUInt32(kerbAuth.Flags),
                            //                              new KerbUInt32(kerbAuth.TokenIL), machineId);
                        var entryBuffer = new Asn1BerEncodingBuffer();
                        entry.BerEncode(entryBuffer, true);
                        authList.Add(new AuthorizationDataElement(new KerbInt32((int)kerbAuth.AdType), new Asn1OctetString(entryBuffer.Data)));
                        break;

                    case AuthorizationData_elementType.AD_IF_RELEVANT:
                        PacAuthData pacAuth = (PacAuthData)authData[i];
                        authList.Add(new AuthorizationDataElement(new KerbInt32((int)pacAuth.AdType), new Asn1OctetString(pacAuth.Data)));
                        break;

                    default:
                        authList.Add(new AuthorizationDataElement(new KerbInt32((int)authData[i].AdType), new Asn1OctetString(new byte[0])));
                        break;

                }
            }

            var authorizationData = new AuthorizationData();
            if (authList.Count > 0)
            {
                authorizationData.Elements = authList.ToArray();
            }
            return authorizationData;
        }


        /// <summary>
        /// Construct AuthData: KerbAuthDataTokenRestrictions
        /// </summary>
        /// <param name="restrictionType">restriction type</param>
        /// <param name="flags">flags in LSAP_TOKEN_INFO_INTEGRITY</param>
        /// <param name="tokenIL">tokenIL field in LSAP_TOKEN_INFO_INTEGRITY</param>
        /// <param name="machineId">machineID field in LSAP_TOKEN_INFO_INTEGRITY. 
        /// This argument cannot be null.</param>
        /// <returns>KerbAuthDataTokenRestrictions</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when input parameter is null</exception>
        [CLSCompliant(false)]
        public KerbAuthDataTokenRestrictions ConstructKerbAuthDataTokenRestrictions(
            int restrictionType,
            uint flags,
            uint tokenIL,
            string machineId)
        {
            if (machineId == null)
            {
                throw new ArgumentNullException(nameof(machineId));
            }

            var authData = new KerbAuthDataTokenRestrictions();
            authData.AdType = AuthorizationData_elementType.KERB_AUTH_DATA_TOKEN_RESTRICTIONS;
            authData.RestrictionType = restrictionType;
            authData.Flags = flags;
            authData.TokenIL = tokenIL;
            authData.MachineId = machineId;
            return authData;
        }


        /// <summary>
        /// Construct AuthData: AdAuthDataApOptions
        /// </summary>
        /// <param name="options">ad_data in KERB_AP_OPTIONS_CBT</param>
        /// <returns>AdAuthDataApOptions</returns>
        [CLSCompliant(false)]
        public AdAuthDataApOptions ConstructAdAuthDataApOptions(uint options)
        {
            var authData = new AdAuthDataApOptions();
            authData.AdType = AuthorizationData_elementType.AD_AUTH_DATA_AP_OPTIONS;
            authData.Options = options;
            return authData;
        }


        /// <summary>
        /// Construct AuthData: PacAuthData
        /// </summary>
        /// <param name="pacData">PAC data. This argument cannot be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <returns>PacAuthData</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when input parameter is null</exception>
        public PacAuthData ConstructPacAuthData(byte[] pacData)
        {
            if (pacData == null)
            {
                throw new ArgumentNullException(nameof(pacData));
            }

            var authData = new PacAuthData();
            authData.AdType = AuthorizationData_elementType.AD_IF_RELEVANT;
            authData.Data = pacData;
            return authData;
        }
        #endregion


        #region PA Data

        /// <summary>
        /// Construct Pre-authentication Data. User can add the PaData they are interested in.
        /// For example, to create a PKCA type PaData, user could use PKCA data to new a PaPkcaData type.
        /// This method is commonly used by server side.
        /// </summary>
        /// <param name="password">The password of the currently logon account.</param>
        /// <param name="salt">The salt of the currently logon account. This could be generated using GenerateSalt method.</param>
        /// <param name="paData">The PaData user wants to construct. This argument can be null.</param>
        /// <returns>The constructed PaData.</returns>
        [CLSCompliant(false)]
        public Asn1SequenceOf<PA_DATA> ConstructPaData(string password, string salt, params PaData[] paData)
        {
            if (paData == null || paData.Length == 0)
            {
                return null;
            }

            var paList = new List<PA_DATA>();

            for (int i = 0; i < paData.Length; ++i)
            {
                if (paData[i] == null)
                {
                    continue;
                }

                switch (paData[i].PaType)
                {
                    case PaDataType.PA_ENC_TIMESTAMP:
                        PaEncTimeStamp paTimeStamp = (PaEncTimeStamp)paData[i];

                        if (paTimeStamp.TimeStamp != null)
                        {
                            // create a timestamp
                            var paEncTsEnc = new PA_ENC_TS_ENC(new KerberosTime(paTimeStamp.TimeStamp), new Microseconds(paTimeStamp.Usec));
                            var currTimeStampBuffer = new Asn1BerEncodingBuffer();
                            paEncTsEnc.BerEncode(currTimeStampBuffer);

                            // encrypt the timestamp
                            byte[] key = KeyGenerator.MakeKey(paTimeStamp.Type, password, salt);
                            byte[] encTimeStamp = KileUtility.Encrypt(paTimeStamp.Type,
                                                                      key,
                                                                      currTimeStampBuffer.Data,
                                                                      (int)KeyUsageNumber.PA_ENC_TIMESTAMP);

                            // create an encrypted timestamp
                            PA_ENC_TIMESTAMP paEncTimeStamp =
                                new PA_ENC_TIMESTAMP(new KerbInt32((int)paTimeStamp.Type), null, new Asn1OctetString(encTimeStamp));
                            var paEncTimestampBuffer = new Asn1BerEncodingBuffer();
                            paEncTimeStamp.BerEncode(paEncTimestampBuffer, true);

                            paList.Add(new PA_DATA(new KerbInt32((int)paTimeStamp.PaType), new Asn1OctetString(paEncTimestampBuffer.Data)));
                        }
                        else
                        {
                            paList.Add(new PA_DATA(new KerbInt32((int)paTimeStamp.PaType), null));
                        }
                        break;

                    case PaDataType.PA_ETYPE_INFO:
                        PaEtypeInfo paInfo = (PaEtypeInfo)paData[i];
                        if (paInfo.TypeList != null && paInfo.TypeList.Count > 0)
                        {
                            var etypeInfo = new ETYPE_INFO_ENTRY[paInfo.TypeList.Count];

                            for (int j = 0; j < paInfo.TypeList.Count; ++j)
                            {
                                etypeInfo[j] = new ETYPE_INFO_ENTRY(new KerbInt32((int)paInfo.TypeList[j]),
                                                                    new Asn1OctetString(salt));
                            }

                            var etype = new ETYPE_INFO(etypeInfo);
                            
                            //var etypeBuffer = new Asn1DerEncodeBuffer();
                            //etype.Encode(etypeBuffer);
                            var etypeBuffer = new Asn1BerEncodingBuffer();
                            etype.BerEncode(etypeBuffer);

                            paList.Add(new PA_DATA(new KerbInt32((int)paInfo.PaType), new Asn1OctetString(etypeBuffer.Data)));
                        }
                        break;

                    case PaDataType.PA_ETYPE_INFO2:
                        PaEtypeInfo2 paInfo2 = (PaEtypeInfo2)paData[i];

                        if (paInfo2.TypeList != null && paInfo2.TypeList.Count > 0)
                        {
                            var etypeInfo = new ETYPE_INFO2_ENTRY[paInfo2.TypeList.Count];

                            for (int j = 0; j < paInfo2.TypeList.Count; ++j)
                            {
                                etypeInfo[j] = new ETYPE_INFO2_ENTRY(new KerbInt32((int)paInfo2.TypeList[j]),
                                    new KerberosString((paInfo2.Salt == null) ? salt : paInfo2.Salt), null);
                            }

                            var etype = new ETYPE_INFO2(etypeInfo);
                            //var etypeBuffer = new Asn1DerEncodeBuffer();
                            //etype.Encode(etypeBuffer);
                            var etypeBuffer = new Asn1BerEncodingBuffer();
                            etype.BerEncode(etypeBuffer);

                            paList.Add(new PA_DATA(new KerbInt32((int)paInfo2.PaType), new Asn1OctetString(etypeBuffer.Data)));
                        }
                        break;

                    case PaDataType.PA_PAC_REQUEST:
                        PaPacRequest paPac = (PaPacRequest)paData[i];
                        var paPacRequest = new KERB_PA_PAC_REQUEST(new Asn1Boolean(paPac.IncludePac));
                        var paPacBuffer = new Asn1BerEncodingBuffer();
                        paPacRequest.BerEncode(paPacBuffer);
                        paList.Add(new PA_DATA(new KerbInt32((int)paPac.PaType), new Asn1OctetString(paPacBuffer.Data)));
                        break;

                    case PaDataType.PA_SVR_REFERRAL_INFO:
                        PaSvrReferralInfo paReferral = (PaSvrReferralInfo)paData[i];
                        if (paReferral.Realm != null)
                        {
                            PrincipalName principalName = null;
                            if (paReferral.PrincipalName != null)
                            {
                                principalName = new PrincipalName(new KerbInt32((int)paReferral.PrincipalType),
                                    KileUtility.String2SeqKerbString(paReferral.PrincipalName));
                            }

                            var svrData = new PA_SVR_REFERRAL_DATA(principalName, new Realm(paReferral.Realm));
                            var svrBuffer = new Asn1BerEncodingBuffer();
                            svrData.BerEncode(svrBuffer);
                            paList.Add(new PA_DATA(new KerbInt32((int)paReferral.PaType), new Asn1OctetString(svrBuffer.Data)));
                        }
                        break;

                    case PaDataType.PA_PK_AS_REP:
                    case PaDataType.PA_PK_AS_REQ:
                    case PaDataType.PA_PK_AS_REP_OLD:
                    case PaDataType.PA_PK_AS_REQ_OLD:
                        PaPkcaData pkcaData = (PaPkcaData)paData[i];
                        if (pkcaData.Data != null)
                        {
                            paList.Add(new PA_DATA(new KerbInt32((int)pkcaData.PaType), new Asn1OctetString(pkcaData.Data)));
                        }
                        break;

                    default:
                        paList.Add(new PA_DATA(new KerbInt32((int)paData[i].PaType), new Asn1OctetString(new byte[0])));
                        break;
                }
            }

            var seqOfPaData = new Asn1SequenceOf<PA_DATA>();
            if (paList.Count > 0)
            {
                seqOfPaData.Elements = paList.ToArray();
            }
            return seqOfPaData;
        }


        /// <summary>
        /// Parse seqOfPaData to PaData array
        /// </summary>
        /// <param name="password">The password of the currently logon account.</param>
        /// <param name="salt">The salt of the currently logon account.
        /// This could be generated using GenerateSalt method.</param>
        /// <param name="seqOfPaData">The sequence of PaData user wants to parse.</param>
        /// <returns>The parsed PaData array.</returns>
        [CLSCompliant(false)]
        public PaData[] ParseSeqOfPaData(Asn1SequenceOf<PA_DATA> seqOfPaData, string password, string salt)
        {
            if (seqOfPaData == null || seqOfPaData.Elements == null)
            {
                return null;
            }

            var paList = new List<PaData>();

            for (int i = 0; i < seqOfPaData.Elements.Length; i++)
            {

                PA_DATA paData = seqOfPaData.Elements[i];

                if (paData == null || paData.padata_type == null || paData.padata_value == null
                    || paData.padata_value.Value == null)
                {
                    continue;
                }
                Asn1DecodingBuffer decodeBuffer = null;

                switch ((PaDataType)paData.padata_type.Value)
                {
                    case PaDataType.PA_ENC_TIMESTAMP:

                        // Decode PA_ENC_TIMESTAMP
                        decodeBuffer = new Asn1DecodingBuffer(paData.padata_value.ByteArrayValue);
                        var paEncTimeStamp = new PA_ENC_TIMESTAMP();
                        paEncTimeStamp.BerDecode(decodeBuffer);

                        // Decrypt PA_ENC_TIMESTAMP
                        byte[] key = KeyGenerator.MakeKey((EncryptionType)paEncTimeStamp.etype.Value, password, salt);

                        // Decrypt the PA_ENC_TS_ENC
                        byte[] encTimeStamp = KileUtility.Decrypt(
                            (EncryptionType)paEncTimeStamp.etype.Value,
                            key,
                            paEncTimeStamp.cipher.ByteArrayValue,
                            (int)KeyUsageNumber.PA_ENC_TIMESTAMP);

                        // Decode PA_ENC_TS_ENC
                        decodeBuffer = new Asn1DecodingBuffer(encTimeStamp);
                        var paEncTsEnc = new PA_ENC_TS_ENC();
                        paEncTsEnc.BerDecode(decodeBuffer);

                        // Generate PaEncTimeStamp
                        var paTimeStamp = new PaEncTimeStamp();
                        paTimeStamp.PaType = PaDataType.PA_ENC_TIMESTAMP;
                        paTimeStamp.Type = (EncryptionType)paEncTimeStamp.etype.Value;
                        paTimeStamp.TimeStamp = paEncTsEnc.patimestamp.Value;

                        // Optional field
                        if (paEncTsEnc.pausec != null)
                        {
                            paTimeStamp.Usec = (int)paEncTsEnc.pausec.Value;
                        }

                        paList.Add(paTimeStamp);
                        break;

                    case PaDataType.PA_ETYPE_INFO:

                        // Decode ETYPE_INFO
                        decodeBuffer = new Asn1DecodingBuffer(paData.padata_value.ByteArrayValue);
                        var etypeInfo = new ETYPE_INFO();
                        etypeInfo.BerDecode(decodeBuffer);

                        // Generate PaEtypeInfo
                        var paEtypeInfo = new PaEtypeInfo();
                        paEtypeInfo.PaType = PaDataType.PA_ETYPE_INFO;
                        paEtypeInfo.TypeList = new Collection<EncryptionType>();

                        if (etypeInfo.Elements != null)
                        {
                            bool isSaltSet = false;

                            foreach (ETYPE_INFO_ENTRY etypeInfoEntry in etypeInfo.Elements)
                            {
                                paEtypeInfo.TypeList.Add((EncryptionType)etypeInfoEntry.etype.Value);

                                if (!isSaltSet && etypeInfoEntry.salt != null && etypeInfoEntry.salt.Value != null)
                                {
                                    paEtypeInfo.Salt = Encoding.ASCII.GetString(etypeInfoEntry.salt.ByteArrayValue);
                                    isSaltSet = true;
                                }
                            }
                        }

                        paList.Add(paEtypeInfo);
                        break;

                    case PaDataType.PA_ETYPE_INFO2:

                        // Decode ETYPE_INFO2
                        decodeBuffer = new Asn1DecodingBuffer(paData.padata_value.ByteArrayValue);
                        var etypeInfo2 = new ETYPE_INFO2();
                        etypeInfo2.BerDecode(decodeBuffer);

                        // Generate PaEtypeInfo2
                        var paEtypeInfo2 = new PaEtypeInfo2();
                        paEtypeInfo2.PaType = PaDataType.PA_ETYPE_INFO2;
                        paEtypeInfo2.TypeList = new Collection<EncryptionType>();

                        if (etypeInfo2.Elements != null)
                        {
                            bool isSaltSet = false;

                            foreach (ETYPE_INFO2_ENTRY etypeInfo2Entry in etypeInfo2.Elements)
                            {
                                paEtypeInfo2.TypeList.Add((EncryptionType)etypeInfo2Entry.etype.Value);

                                if (!isSaltSet && etypeInfo2Entry.salt != null && etypeInfo2Entry.salt.Value != null)
                                {
                                    paEtypeInfo2.Salt = etypeInfo2Entry.salt.Value;
                                    isSaltSet = true;
                                }
                            }
                        }

                        paList.Add(paEtypeInfo2);
                        break;

                    case PaDataType.PA_PAC_REQUEST:

                        // Decode KERB_PA_PAC_REQUEST
                        decodeBuffer = new Asn1DecodingBuffer(paData.padata_value.ByteArrayValue);
                        var paPacRequest = new KERB_PA_PAC_REQUEST();
                        paPacRequest.BerDecode(decodeBuffer);

                        // Generate PaPacRequest
                        var paPac = new PaPacRequest();
                        paPac.PaType = PaDataType.PA_PAC_REQUEST;

                        if (paPacRequest.include_pac != null)
                        {
                            paPac.IncludePac = paPacRequest.include_pac.Value;
                        }

                        paList.Add(paPac);
                        break;

                    case PaDataType.PA_SVR_REFERRAL_INFO:

                        // Decode PA_SVR_REFERRAL_DATA
                        decodeBuffer = new Asn1DecodingBuffer(paData.padata_value.ByteArrayValue);
                        var svrData = new PA_SVR_REFERRAL_DATA();
                        svrData.BerDecode(decodeBuffer);

                        // Generate PaSvrReferralInfo
                        var paReferral = new PaSvrReferralInfo();
                        paReferral.PaType = PaDataType.PA_SVR_REFERRAL_INFO;
                        paReferral.Realm = svrData.referred_realm.Value;

                        // Optional field
                        if (svrData.referred_name != null)
                        {
                            if (svrData.referred_name.name_string != null
                                && svrData.referred_name.name_string.Elements != null
                                && svrData.referred_name.name_string.Elements.Length > 0)
                            {
                                paReferral.PrincipalName = svrData.referred_name.name_string.Elements[0].Value;
                            }
                            if (svrData.referred_name.name_type != null)
                            {
                                paReferral.PrincipalType = (PrincipalType)svrData.referred_name.name_type.Value;
                            }
                        }

                        paList.Add(paReferral);
                        break;

                    case PaDataType.PA_PK_AS_REP:
                    case PaDataType.PA_PK_AS_REQ:
                    case PaDataType.PA_PK_AS_REP_OLD:
                    case PaDataType.PA_PK_AS_REQ_OLD:

                        // Generate PaPkcaData
                        var pkcaData = new PaPkcaData();
                        pkcaData.PaType = (PaDataType)paData.padata_type.Value;
                        pkcaData.Data = paData.padata_value.ByteArrayValue; //paData.padata_valueValue;

                        paList.Add(pkcaData);
                        break;

                    default:
                        paList.Add(new PaPkcaData());
                        break;
                }
            }

            return paList.ToArray();
        }


        /// <summary>
        /// Construct PaData: PaEncTimeStamp. usec and timeStamp will be set with the current time as default.
        /// </summary>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>PaEncTimeStamp</returns>
        public PaEncTimeStamp ConstructPaEncTimeStamp(EncryptionType encryptionType)
        {
            var paData = new PaEncTimeStamp();
            paData.PaType = PaDataType.PA_ENC_TIMESTAMP;
            paData.Type = encryptionType;
            paData.TimeStamp = KileUtility.CurrentKerberosTime.Value;
            paData.Usec = 0;
            return paData;
        }


        /// <summary>
        /// Construct PaData: PaEtypeInfo 
        /// </summary>
        /// <param name="encryptionTypeList">list of supported encryption types. This argument can be null. </param>
        /// <returns>PaEtypeInfo</returns>
        public PaEtypeInfo ConstructPaEtypeInfo(params EncryptionType[] encryptionTypeList)
        {
            PaEtypeInfo paData = null;
            if (encryptionTypeList != null)
            {
                paData = new PaEtypeInfo();
                paData.PaType = PaDataType.PA_ETYPE_INFO;
                paData.TypeList = new Collection<EncryptionType>();
                foreach (EncryptionType type in encryptionTypeList)
                {
                    paData.TypeList.Add(type);
                }
            }

            return paData;
        }


        /// <summary>
        /// Construct PaData: PaEtypeInfo2
        /// </summary>
        /// <param name="salt">The salt used to do encryption. This argument can be null.</param>
        /// <param name="typeList">list of supported encryption types. This argument can be null.</param>
        /// <returns>PaEtypeInfo2</returns>
        public PaEtypeInfo2 ConstructPaEtypeInfo2(string salt, params EncryptionType[] encryptionTypeList)
        {
            PaEtypeInfo2 paData = null;
            if (encryptionTypeList != null)
            {
                paData = new PaEtypeInfo2();
                paData.Salt = salt;
                paData.PaType = PaDataType.PA_ETYPE_INFO2;
                paData.TypeList = new Collection<EncryptionType>();
                foreach (EncryptionType type in encryptionTypeList)
                {
                    paData.TypeList.Add(type);
                }
            }

            return paData;
        }


        /// <summary>
        /// Construct PaData: PaPkcaData
        /// </summary>
        /// <param name="paType">The type of PaData.</param>
        /// <param name="pkcaData">PKCA data bytes. This argument cannot be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <returns>PaPkcaData</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when input parameter is null</exception>
        public PaPkcaData ConstructPaPkcaData(PaDataType paType, byte[] pkcaData)
        {
            if (pkcaData == null)
            {
                throw new ArgumentNullException(nameof(pkcaData));
            }

            var paData = new PaPkcaData();
            paData.PaType = paType;
            paData.Data = pkcaData;
            return paData;
        }


        /// <summary>
        /// Construct PaData: PaPacRequest
        /// </summary>
        /// <param name="includePac">if include pac data</param>
        /// <returns>PaPacRequest</returns>
        public PaPacRequest ConstructPaPacRequest(bool includePac)
        {
            var paData = new PaPacRequest();
            paData.PaType = PaDataType.PA_PAC_REQUEST;
            paData.IncludePac = includePac;
            return paData;
        }


        /// <summary>
        /// Construct PaData: PaSvrReferralInfo
        /// </summary>
        /// <param name="principalType">principal type</param>
        /// <param name="principalName">principal name</param>
        /// <param name="realm">realm</param>
        /// <returns>PaSvrReferralInfo</returns>
        public PaSvrReferralInfo ConstructPaSvrReferralInfo(
            PrincipalType principalType,
            string principalName,
            string realm)
        {
            var paData = new PaSvrReferralInfo();
            paData.PaType = PaDataType.PA_SVR_REFERRAL_INFO;
            paData.PrincipalName = principalName;
            paData.PrincipalType = principalType;
            paData.Realm = realm;
            return paData;
        }

        #endregion


        #region Wrap/Unwrap, GetMic/VerifyMic

        /// <summary>
        /// Create a Gss_Wrap token. Then use KilePdu.ToBytes() to get the byte array.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="signAlgorithm">Specify the checksum type.
        /// This is only used for encryption types DES and RC4.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the encryption type is not supported.</exception>
        [CLSCompliant(false)]
        public KilePdu GssWrap(bool isEncrypted, SGN_ALG signAlgorithm, byte[] message)
        {
            KilePdu pdu = null;
            EncryptionKey key = Context.ContextKey;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    pdu = GssWrap4121(isEncrypted, message, Context.isInitiator);
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    pdu = GssWrap1964(isEncrypted, signAlgorithm, message);
                    break;

                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    pdu = GssWrap4757(isEncrypted, signAlgorithm, message);
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return pdu;
        }


        /// <summary>
        /// Decode a Gss_Wrap token.
        /// </summary>
        /// <param name="token">The token got from an application message. 
        /// If this argument is null, null will be returned.</param>
        /// <returns>The decoded Gss_Wrap token.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the encryption is not supported.</exception>
        public KilePdu GssUnWrap(byte[] token)
        {
            return GssUnWrap(Context, token);
        }


        /// <summary>
        /// Decode a Gss_Wrap token from security buffers
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>The decoded Gss_Wrap token.</returns>
        public KilePdu GssUnWrapEx(params SecurityBuffer[] securityBuffers)
        {
            return GssUnWrapEx(Context, securityBuffers);
        }


        /// <summary>
        /// Decode a Gss_Wrap token.
        /// </summary>
        /// <param name="context">The context of decoding</param>
        /// <param name="token">The token got from an application message. </param>
        /// <returns>The decoded Gss_Wrap token.</returns>
        internal static KilePdu GssUnWrap(KileContext context, byte[] token)
        {
            KilePdu pdu = null;
            EncryptionKey key = context.ContextKey;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    pdu = new Token4121(context);
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    pdu = new Token1964_4757(context);
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            pdu.FromBytes(token);
            return pdu;
        }


        /// <summary>
        /// Decode a Gss_Wrap token from security buffers
        /// </summary>
        /// <param name="context">The context of decoding</param>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>The decoded Gss_Wrap token.</returns>
        internal static KilePdu GssUnWrapEx(KileContext context, SecurityBuffer[] securityBuffers)
        {
            KilePdu pdu = null;
            EncryptionKey key = context.ContextKey;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    var token4121Pdu = new Token4121(context);
                    token4121Pdu.FromSecurityBuffers(securityBuffers);
                    pdu = token4121Pdu;
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    var token1964or4757Pdu = new Token1964_4757(context);
                    token1964or4757Pdu.FromSecurityBuffers(securityBuffers);
                    pdu = token1964or4757Pdu;
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return pdu;
        }


        /// <summary>
        /// Create a Gss_GetMic token. Then use KilePdu.ToBytes() to get the byte array.
        /// </summary>
        /// <param name="signAlgorithm">Specify the checksum type.
        /// This is only used for encryption types DES and RC4.</param>
        /// <param name="message">The message to be computed signature. This argument can be null.</param>
        /// <returns>The created Gss_GetMic token, NotSupportedException.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the encryption is not supported.</exception>
        [CLSCompliant(false)]
        public KilePdu GssGetMic(SGN_ALG signAlgorithm, byte[] message)
        {
            KilePdu pdu = null;
            EncryptionKey key = Context.ContextKey;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    pdu = GssGetMic4121(message, Context.isInitiator);
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    pdu = GssGetMic1964_4757(signAlgorithm, message);
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return pdu;
        }


        /// <summary>
        /// Decode and verify a Gss_GetMic token.
        /// </summary>
        /// <param name="token">The token got from an application message. 
        /// If this argument is null, null will be returned.</param>
        /// <param name="message">The message to be computed signature. 
        /// If this argument is null, null will be returned.</param>
        /// <param name="pdu">The decoded Gss_GetMic token.</param>
        /// <returns>If verifying mic token is successful.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the encryption is not supported.</exception>
        public bool GssVerifyMic(byte[] token, byte[] message, out KilePdu pdu)
        {
            return GssVerifyMic(Context, token, message, out pdu);
        }

        /// <summary>
        /// Decode and verify a Gss_GetMic token.
        /// </summary>
        /// <param name="context">The context to verify a token.</param>
        /// <param name="token">The token got from an application message. 
        /// If this argument is null, null will be returned.</param>
        /// <param name="message">The message to be computed signature. 
        /// If this argument is null, null will be returned.</param>
        /// <param name="pdu">The decoded Gss_GetMic token.</param>
        /// <returns>If verifying mic token is successful.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the encryption is not supported.</exception>
        internal static bool GssVerifyMic(KileContext context, byte[] token, byte[] message, out KilePdu pdu)
        {
            pdu = null;
            bool isVerified = true;
            EncryptionKey key = context.ContextKey;

            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    var micPdu4121 = new Token4121(context);
                    micPdu4121.Data = message;
                    try
                    {
                        micPdu4121.FromBytes(token);
                    }
                    catch (FormatException)
                    {
                        isVerified = false;
                    }
                    pdu = micPdu4121;
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    var micPdu1964_4757 = new Token1964_4757(context);
                    micPdu1964_4757.Data = message;
                    try
                    {
                        micPdu1964_4757.FromBytes(token);
                    }
                    catch (FormatException)
                    {
                        isVerified = false;
                    }
                    pdu = micPdu1964_4757;
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return isVerified;
        }


        public bool GssVerifyMicEx(SecurityBuffer[] securityBuffers, out KilePdu pdu)
        {
            return GssVerifyMicEx(Context, securityBuffers, out pdu);
        }


        internal static bool GssVerifyMicEx(KileContext context, SecurityBuffer[] securityBuffers, out KilePdu pdu)
        {
            pdu = null;
            bool isVerified = true;
            EncryptionKey key = context.ContextKey;

            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    var micPdu4121 = new Token4121(context);
                    try
                    {
                        micPdu4121.FromSecurityBuffers(securityBuffers);
                    }
                    catch (FormatException)
                    {
                        isVerified = false;
                    }
                    pdu = micPdu4121;
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    var micPdu1964_4757 = new Token1964_4757(context);
                    try
                    {
                        micPdu1964_4757.FromSecurityBuffers(securityBuffers);
                    }
                    catch (FormatException)
                    {
                        isVerified = false;
                    }
                    pdu = micPdu1964_4757;
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return isVerified;
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Create a Gss_Wrap [RFC4121] token.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <param name="isInitiator">If the sender is initiator.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        private Token4121 GssWrap4121(bool isEncrypted, byte[] message, bool isInitiator)
        {
            var token = new Token4121(Context);
            var tokenHeader = new TokenHeader4121();
            tokenHeader.tok_id = TOK_ID.Wrap4121;
            tokenHeader.flags = isEncrypted ? WrapFlag.Sealed : WrapFlag.None;
            if (!isInitiator)
            {
                tokenHeader.flags |= WrapFlag.SentByAcceptor;
            }

            if (Context.AcceptorSubKey != null)
            {
                tokenHeader.flags |= WrapFlag.AcceptorSubkey;
            }

            tokenHeader.filler = ConstValue.TOKEN_FILLER_1_BYTE;
            tokenHeader.ec = 0;
            // The RRC field described in section 4.2.5 of [RFC4121] is 12 if no encryption is requested 
            // or 16 if encryption is requested.
            tokenHeader.rrc = isEncrypted ? (ushort)16 : (ushort)12;
            tokenHeader.snd_seq = Context.CurrentLocalSequenceNumber;

            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }


        /// <summary>
        /// Create a Gss_Wrap [RFC1964] token.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="signAlgorithm">Specify the checksum type.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        private Token1964_4757 GssWrap1964(bool isEncrypted, SGN_ALG signAlgorithm, byte[] message)
        {
            var token = new Token1964_4757(Context);
            var tokenHeader = new TokenHeader1964_4757();
            tokenHeader.tok_id = TOK_ID.Wrap1964_4757;
            tokenHeader.sng_alg = signAlgorithm;
            tokenHeader.seal_alg = isEncrypted ? SEAL_ALG.DES : SEAL_ALG.NONE;
            tokenHeader.filler = ConstValue.TOKEN_FILLER_2_BYTE;

            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }


        /// <summary>
        /// Create a Gss_Wrap [RFC4757] token.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="signAlgorithm">Specify the checksum type.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        private Token1964_4757 GssWrap4757(bool isEncrypted, SGN_ALG signAlgorithm, byte[] message)
        {
            var token = new Token1964_4757(Context);
            var tokenHeader = new TokenHeader1964_4757();
            tokenHeader.tok_id = TOK_ID.Wrap1964_4757;
            tokenHeader.sng_alg = signAlgorithm;
            tokenHeader.seal_alg = isEncrypted ? SEAL_ALG.RC4 : SEAL_ALG.NONE;
            tokenHeader.filler = ConstValue.TOKEN_FILLER_2_BYTE;

            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }


        /// <summary>
        /// Create a Gss_GetMic [RFC4121] token.
        /// </summary>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <param name="isInitiator">If the sender is initiator.</param>
        /// <returns>The created Gss_GetMic token.</returns>
        private Token4121 GssGetMic4121(byte[] message, bool isInitiator)
        {
            var token = new Token4121(Context);
            var tokenHeader = new TokenHeader4121();
            tokenHeader.tok_id = TOK_ID.Mic4121;
            tokenHeader.flags = WrapFlag.None;
            if (!isInitiator)
            {
                tokenHeader.flags |= WrapFlag.SentByAcceptor;
            }

            if (Context.AcceptorSubKey != null)
            {
                tokenHeader.flags |= WrapFlag.AcceptorSubkey;
            }

            tokenHeader.filler = ConstValue.TOKEN_FILLER_1_BYTE;
            tokenHeader.ec = ConstValue.TOKEN_FILLER_2_BYTE;
            tokenHeader.rrc = ConstValue.TOKEN_FILLER_2_BYTE;
            tokenHeader.snd_seq = Context.CurrentLocalSequenceNumber;

            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }


        /// <summary>
        /// Create a Gss_GetMic [RFC1964] token.
        /// </summary>
        /// <param name="signAlgorithm">Specify the checksum type.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_GetMic token.</returns>
        private Token1964_4757 GssGetMic1964_4757(SGN_ALG signAlgorithm, byte[] message)
        {
            var token = new Token1964_4757(Context);
            var tokenHeader = new TokenHeader1964_4757();
            tokenHeader.tok_id = TOK_ID.Mic1964_4757;
            tokenHeader.sng_alg = signAlgorithm;
            tokenHeader.seal_alg = SEAL_ALG.NONE;
            tokenHeader.filler = ConstValue.TOKEN_FILLER_2_BYTE;

            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }
        #endregion


        #region IDisposable
        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                }

                //Note disposing has been done.
                disposed = true;
            }
        }


        /// <summary>
        /// Destruct this instance.
        /// </summary>
        ~KileRole()
        {
            Dispose(false);
        }
        #endregion
    }
}
