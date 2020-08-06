// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /// <summary>
    /// Helper functions of Spng StackSDK
    /// </summary>
    public static class SpngUtility
    {
        /// <summary>
        /// The size of the context flags
        /// </summary>
        private static int ContextFlagsSize = 32;

        /// <summary>
        /// Convert uint to Asn.1 formatted ContextFlags
        /// </summary>
        /// <param name="attributes">Security context attributes</param>
        /// <returns>the converted ContextFlags</returns>
        public static ContextFlags ConvertUintToContextFlags(uint attributes)
        {
            ContextFlags options = new ContextFlags(KerberosUtility.ConvertInt2Flags((int)attributes));
           
            return options;
        }


        /// <summary>
        /// Encode an Asn.1 formatted type into a byte array
        /// </summary>
        /// <param name="type">Asn.1 formatted type to be encoded</param>
        /// <returns>encoded byte array</returns>
        public static byte[] EncodeAsn1(Asn1Object type)
        {
            Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
            type.BerEncode(buffer);

            return buffer.Data;
        }


        /// <summary>
        /// Decode a byte array to a designated Asn.1 type
        /// </summary>
        /// <typeparam name="T">designated Asn.1 type</typeparam>
        /// <param name="rawData">byte array to be decoded</param>
        /// <returns>the decoded Asn.1 type</returns>
        public static T DecodeAsn1<T>(byte[] rawData) where T : Asn1Object, new()
        {
            T asn1Type = new T();
            Asn1DecodingBuffer buffer = new Asn1DecodingBuffer(rawData);
            
            asn1Type.BerDecode(buffer);

            return asn1Type;
        }


        /// <summary>
        /// Generate the default supported MechType list
        /// </summary>
        /// <returns>DefaultMechList</returns>
        public static MechTypeList GenerateDefaultMechList()
        {
            MechType unknownMechType = new MechType(SspiLib.Consts.UnknownOidInt);
            MechType msKerbMechType = new MechType(SspiLib.Consts.MsKerbOidInt);
            MechType kerbMechType = new MechType(SspiLib.Consts.KerbOidInt);
            MechType nlmpMechType = new MechType(SspiLib.Consts.NlmpOidInt);
            MechType negoExMechType = new MechType(SspiLib.Consts.NegoExOidInt);

            MechTypeList mechList = new MechTypeList(new MechType[] {
                nlmpMechType,
                msKerbMechType,
                negoExMechType,
                kerbMechType,
                unknownMechType
            });

            return mechList;
        }


        /// <summary>
        /// Adjust the sequence of a MechType list
        /// </summary>
        /// <param name="mechList">the MechType list</param>
        /// <param name="indexToMove">the MechType with this index will be moved to first</param>
        /// <returns>The updated MechType list</returns>
        /// <exception cref="ArgumentOutOfRangeException">index out of range</exception>
        public static MechTypeList MoveMechTypeToFirst(MechTypeList mechList, int indexToMove)
        {
            if (indexToMove >= mechList.Elements.Length)
            {
                throw new System.ArgumentOutOfRangeException("indexToMove");
            }

            MechType mechTypeToMove = mechList.Elements[indexToMove];

            for (int i = indexToMove; i > 0; i--)
            {
                //swap i with i-1
                MechType currentMechType = mechList.Elements[i - 1];
                mechList.Elements[i - 1] = mechList.Elements[i];
                mechList.Elements[i] = currentMechType;
            }

            return mechList;
        }


        /// <summary>
        /// Convert MechType to SecurityPackage enum
        /// </summary>
        /// <param name="mechType">The MechType value to be convert</param>
        /// <returns>The converted AuthMech enum value</returns>
        public static SecurityPackageType ConvertMechType(MechType mechType)
        {
            SecurityPackageType authType = SecurityPackageType.Unknown;

            if (ArrayUtility.CompareArrays<int>(mechType.Value, SspiLib.Consts.MsKerbOidInt))
            {
                authType = SecurityPackageType.Kerberos;
            }
            else if (ArrayUtility.CompareArrays<int>(mechType.Value, SspiLib.Consts.NlmpOidInt))
            {
                authType = SecurityPackageType.Ntlm;
            }
            else if (ArrayUtility.CompareArrays<int>(mechType.Value, SspiLib.Consts.KerbOidInt))
            {
                authType = SecurityPackageType.Kerberos;
            }
            else if (ArrayUtility.CompareArrays<int>(mechType.Value, SspiLib.Consts.NegoExOidInt))
            {
                authType = SecurityPackageType.Unknown;
            }

            return authType;
        }

        /// <summary>
        /// Get the specific configuration from the SecurityConfig list by the security type.
        /// </summary>
        /// <param name="configList">SecurityConfig list.</param>
        /// <param name="securityType">The security type.</param>
        /// <returns>The specific security configuration.</returns>
        internal static SecurityConfig GetSecurityConfig(SecurityConfig[] configList, SecurityPackageType securityType)
        {
            SecurityConfig sspiConfig = null;

            foreach (SecurityConfig configItem in configList)
            {
                if (configItem.SecurityType == securityType)
                {
                    return configItem;
                }
                else if (configItem.SecurityType == SecurityPackageType.Unknown)
                {
                    // SSPI configuration is initialized as Unknown type
                    sspiConfig = configItem;
                }
            }

            return sspiConfig;
        }

    }
}
