// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public interface IAuthDataElement
    {
        AuthorizationDataElement AuthDataElement{get;}
    }

    public class KerbAuthDataTokenRestrictions : IAuthDataElement 
    {
        /// <summary>
        /// restriction_type of KERB_AD_RESTRICTION_ENTRY.
        /// </summary>
        private int restriction_type;

        /// <summary>
        /// flags of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        private uint flags;

        /// <summary>
        /// tokenIL of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        private uint tokenIL;

        /// <summary>
        /// machineID of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        private string machineId;

        public KerbAuthDataTokenRestrictions(
            int restrictionType,
            uint flags,
            uint tokenIL,
            string machineId
            )
        {
            if (machineId == null)
            {
                throw new Exception();
            }

            this.restriction_type = restrictionType;
            this.flags = flags;
            this.tokenIL = tokenIL;
            this.machineId = machineId;
        }

        /// <summary>
        /// restriction_type of KERB_AD_RESTRICTION_ENTRY.
        /// </summary>
        public int RestrictionType
        {
            get
            {
                return restriction_type;
            }
            set
            {
                restriction_type = value;
            }
        }

        /// <summary>
        /// flags of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        public uint Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        /// <summary>
        /// tokenIL of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        public uint TokenIL
        {
            get
            {
                return tokenIL;
            }
            set
            {
                tokenIL = value;
            }
        }

        /// <summary>
        /// machineID of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        public string MachineId
        {
            get
            {
                return machineId;
            }
            set
            {
                machineId = value;
            }
        }
        
        public AuthorizationDataElement AuthDataElement
        {
            get {
                var machineID = new Asn1OctetString(machineId);
                KerbUInt32 _flags = new KerbUInt32(flags);
                KerbUInt32 _tokenIL = new KerbUInt32(tokenIL);
                var restriction = new LSAP_TOKEN_INFO_INTEGRITY(_flags, _tokenIL, machineID);
                var kerbAdRestrictionEntry = new KERB_AD_RESTRICTION_ENTRY(new KerbInt32(restriction_type), restriction);

                var dataBuffer = new Asn1BerEncodingBuffer();
                kerbAdRestrictionEntry.BerEncode(dataBuffer, true);

                return new AuthorizationDataElement(
                    new KerbInt32((int)AuthorizationData_elementType.KERB_AUTH_DATA_TOKEN_RESTRICTIONS),
                    new Asn1OctetString(dataBuffer.Data));
            }
        }

        public static KerbAuthDataTokenRestrictions Parse(AuthorizationDataElement element)
        {
            if (element.ad_type.Value != (int)AuthorizationData_elementType.KERB_AUTH_DATA_TOKEN_RESTRICTIONS)
                throw new Exception();
            var entry = new KERB_AD_RESTRICTION_ENTRY();
            entry.BerDecode(new Asn1DecodingBuffer(element.ad_data.ByteArrayValue));
            
            LSAP_TOKEN_INFO_INTEGRITY ltii = new LSAP_TOKEN_INFO_INTEGRITY();
            ltii.GetElements(entry.restriction);

            return new KerbAuthDataTokenRestrictions(
                (int)entry.restriction_type.Value,
                (uint)ltii.flags.Value,
                (uint)ltii.tokenIL.Value,
                Encoding.UTF8.GetString(ltii.machineID.ByteArrayValue));
        }
    }

    public class AdWin2KPac : IAuthDataElement
    {
        public AuthorizationDataElement AuthDataElement
        {
            get { return new AuthorizationDataElement(
                new KerbInt32((int)AuthorizationData_elementType.AD_WIN2K_PAC),
                new Asn1OctetString(Pac.ToBytes())); }
        }

        public PacType Pac { get; private set; }

        public static AdWin2KPac Parse(AuthorizationDataElement element)
        {
            if (element.ad_type.Value != (int)AuthorizationData_elementType.AD_WIN2K_PAC)
                throw new Exception();
            var adWin2KPac = new AdWin2KPac();
            adWin2KPac.Pac = PacUtility.DecodePacType(element.ad_data.ByteArrayValue);
            return adWin2KPac;
        }
    }

    public class AdIfRelevent : IAuthDataElement 
    {
        public AD_IF_RELEVANT adIfRelevant { get; set; }

        public IAuthDataElement[] Elements { get; private set; }

        public AdIfRelevent(AD_IF_RELEVANT adIfRelevant)
        {
            this.adIfRelevant = adIfRelevant;
            int len = adIfRelevant.Elements.Length;
            Elements = new IAuthDataElement[len];
            for(int i =0;i<len;i++)
            {
                Elements[i] = AuthDataElementParser.ParseAuthDataElement(adIfRelevant.Elements[i]);
            }
        }

        public AuthorizationDataElement AuthDataElement
        {
            get {
                var dataBuffer = new Asn1BerEncodingBuffer();
                adIfRelevant.BerEncode(dataBuffer);

                return new AuthorizationDataElement(
                    new KerbInt32((int)AuthorizationData_elementType.AD_IF_RELEVANT),
                    new Asn1OctetString(dataBuffer.Data));
            }
        }

        public static AdIfRelevent Parse(AuthorizationDataElement element)
        {
            if (element.ad_type.Value != (int)AuthorizationData_elementType.AD_IF_RELEVANT)
                throw new Exception();
            var adIfRelevant = new AD_IF_RELEVANT();
            adIfRelevant.BerDecode(new Asn1DecodingBuffer(element.ad_data.ByteArrayValue));
            return new AdIfRelevent(adIfRelevant);
        }
    }

    public class AdAuthDataApOptions : IAuthDataElement
    {
        /// <summary>
        /// ad_data of KERB_AP_OPTIONS_CBT.
        /// </summary>
        private uint options;

        /// <summary>
        /// ad_data of KERB_AP_OPTIONS_CBT.
        /// </summary>
        public uint Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
            }
        }

        public AdAuthDataApOptions(uint options)
        {
            this.options = options;
        }

        public AuthorizationDataElement AuthDataElement
        {
            get {
                var apOptions = new APOptions(KerberosUtility.ConvertInt2Flags((int)options));
                var dataBuffer = new Asn1BerEncodingBuffer();
                apOptions.BerEncode(dataBuffer);

                return new AuthorizationDataElement(
                    new KerbInt32((int)AuthorizationData_elementType.AD_AUTH_DATA_AP_OPTIONS),
                    new Asn1OctetString(dataBuffer.Data));
            }
        }
        public static AdAuthDataApOptions Parse(AuthorizationDataElement element)
        {
            if (element.ad_type.Value != (int)AuthorizationData_elementType.AD_AUTH_DATA_AP_OPTIONS)
                throw new Exception();
            var apOptions = new APOptions();
            apOptions.BerDecode(new Asn1DecodingBuffer(element.ad_data.ByteArrayValue));
            throw new NotImplementedException();
        }
    }

    public class AdFxFastUsed : IAuthDataElement
    {

        public AdFxFastUsed()
        {
        }

        public AuthorizationDataElement AuthDataElement
        {
            get
            {
                return new AuthorizationDataElement(
                    new KerbInt32((int)AuthorizationData_elementType.AD_FX_FAST_USED),
                    new Asn1OctetString((byte[])null));
            }
        }
        public static AdFxFastUsed Parse(AuthorizationDataElement element)
        {
            if (element.ad_type.Value != (int)AuthorizationData_elementType.AD_FX_FAST_USED)
                throw new Exception();
            return new AdFxFastUsed();
        }
    }


    public class AdFxFastArmor : IAuthDataElement
    {

        public AdFxFastArmor()
        {
        }

        public AuthorizationDataElement AuthDataElement
        {
            get
            {
                return new AuthorizationDataElement(
                    new KerbInt32((int) AuthorizationData_elementType.AD_FX_FAST_ARMOR),
                    new Asn1OctetString((byte[]) null));
            }
        }
        public static AdFxFastArmor Parse(AuthorizationDataElement element)
        {
            if (element.ad_type.Value != (int)AuthorizationData_elementType.AD_FX_FAST_ARMOR)
                throw new Exception();
            return new AdFxFastArmor();
        }
    }

    public static class AuthDataElementParser
    {
        public static IAuthDataElement ParseAuthDataElement(AuthorizationDataElement element)
        {
            IAuthDataElement authDataElement;
            switch ((AuthorizationData_elementType)element.ad_type.Value)
            {
                case AuthorizationData_elementType.AD_IF_RELEVANT:
                    authDataElement = AdIfRelevent.Parse(element);
                    break;
                case AuthorizationData_elementType.AD_WIN2K_PAC:
                    authDataElement = AdWin2KPac.Parse(element);
                    break;
                case AuthorizationData_elementType.AD_FX_FAST_USED:
                    authDataElement = AdFxFastUsed.Parse(element);
                    break;
                case AuthorizationData_elementType.KERB_AUTH_DATA_TOKEN_RESTRICTIONS:
                    authDataElement = KerbAuthDataTokenRestrictions.Parse(element);
                    break;
                default:
                    return null;
            }
            return authDataElement;
        }
    }
}
