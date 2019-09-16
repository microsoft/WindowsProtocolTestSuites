// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public class TrustInfo
    {
        /// <summary>
        /// Forest trust information record flags for record type 0 and 1.
        /// </summary>
        public enum FOREST_TRUST_RECORD_FLAGS_TOPLEVEL_NAME : uint
        {
            LSA_TLN_DISABLED_NEW = 0x00000001,
            LSA_TLN_DISABLED_ADMIN = 0x00000002,
            LSA_TLN_DISABLED_CONFLICT = 0x00000004,
        }

        /// <summary>
        /// Forest trust information record flags for record type 2.
        /// </summary>
        public enum FOREST_TRUST_RECORD_FLAGS_DOMAIN_INFO : uint {
            LSA_SID_DISABLED_ADMIN = 0x00000001,
            LSA_SID_DISABLED_CONFLICT = 0x00000002,
            LSA_NB_DISABLED_ADMIN = 0x00000004,
            LSA_NB_DISABLED_CONFLICT = 0x00000008,
            NETBIOS_DISABLED_MASK = 0x0000000f
        }

        /// <summary>
        /// For all record types, LSA_FTRECORD_DISABLED_REASONS is defined 
        /// as a mask on the lower 16 bits of the Flags field. 
        /// Unused bits covered by the mask are reserved for future use.
        /// </summary>
        public const uint LSA_FTRECORD_DISABLED_REASONS = 0x0000ffff;

        /// <summary>
        /// The sequence [index .. index + size] of bytes in buffer is returned.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] ExtractBinary(byte[] buffer, uint index, uint size)
        {
            byte[] d = new byte[size];
            Array.Copy(buffer, index, d, 0, size);
            return d;
        }

        /// <summary>
        /// The sequence [index .. index + size] of bytes in buffer is interpreted as a UTF-8 string, 
        /// and a corresponding unicodestring (section 3.4.3) is returned.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ExtractString(byte[] buffer, uint index, uint size)
        {
            return System.Text.Encoding.UTF8.GetString(
                ExtractBinary(buffer, index, size)
                );
        }

        /// <summary>
        /// The sequence [index .. index + size] of bytes in buffer is converted into a SID structure and returned.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static NT4SID ExtractSid(byte[] buffer, uint index, uint size)
        {
            NT4SID sid = new NT4SID();
            Array.Copy(buffer, index, sid.Data, 0, size);
            return sid;
        }

        /// <summary>
        /// The IsSubdomainOf procedure takes a pair of domain names and 
        /// returns true if subdomainName is a subdomain of superiordomainName 
        /// as described in [RFC1034] section 3.1, and false otherwise.
        /// </summary>
        /// <param name="subdomainName"></param>
        /// <param name="superiordomainName"></param>
        /// <returns></returns>
        public static bool IsSubdomainOf(string subdomainName, string superiordomainName)
        {
            string[] subArr = subdomainName.Split('.');
            string[] supArr = superiordomainName.Split('.');
            if (subArr.Length <= supArr.Length)
                return false;

            for (int i = supArr.Length - 1; i >= 0; --i)
            {
                if (supArr[i] != subArr[i + (subArr.Length - supArr.Length)])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// The UnmarshalForestTrustInfo procedure unmarshals the byte stream inputBuffer, 
        /// which holds the content of a msDS-TrustForestTrustInfo attribute that contains forest trust information, 
        /// as described in FOREST_TRUST_INFORMATION, into the forestTrustInfo structure.
        /// </summary>
        /// <param name="inputBuffer"></param>
        /// <param name="forestTrustInfo"></param>
        /// <returns></returns>
        public static bool UnmarshalForestTrustInfo(byte[] inputBuffer, out FOREST_TRUST_INFORMATION forestTrustInfo)
        {
            forestTrustInfo = new FOREST_TRUST_INFORMATION();
            uint index = 0;
            uint version = inputBuffer[index];
            if (version != 1)
                return false;

            index += 4;
            uint recordCount = inputBuffer[index];
            forestTrustInfo.RecordCount = recordCount;
            forestTrustInfo.Records = new Record[recordCount];

            index += 4;

            for (int i = 0; i < recordCount; ++i)
            {
                uint recordLength = inputBuffer[index];
                forestTrustInfo.Records[i].RecordLen = recordLength;
                index += 4;

                uint flags = inputBuffer[index];
                forestTrustInfo.Records[i].Flags = flags;
                index += 4;
                long ulTime = (long)(inputBuffer[index] << 32) + (long)inputBuffer[index + 4];
                forestTrustInfo.Records[i].Timestamp.int64Value = ulTime;
                index += 8;

                forestTrustInfo.Records[i].RecordType = inputBuffer[index];
                index += 1;

                if (forestTrustInfo.Records[i].RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelName
                    || forestTrustInfo.Records[i].RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelNameEx)
                {
                    RecordTopLevelName r = new RecordTopLevelName();
                    uint sz = inputBuffer[index];
                    index += 4;

                    r.TopLevelName = ExtractString(inputBuffer, index, sz);

                    index += sz;

                    forestTrustInfo.Records[i].ForestTrustData = r;
                }
                else if (forestTrustInfo.Records[i].RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo)
                {
                    uint sz = inputBuffer[index];
                    index += 4;
                    RecordDomainInfo r = new RecordDomainInfo();
                    r.Sid.Data = ExtractBinary(inputBuffer, index, sz);
                    index += sz;

                    sz = inputBuffer[index];
                    index += 4;
                    r.DnsName = ExtractString(inputBuffer, index, sz);
                    index += sz;

                    sz = inputBuffer[index];
                    index += 4;
                    r.NetbiosName = ExtractString(inputBuffer, index, sz);
                    index += sz;

                    forestTrustInfo.Records[i].ForestTrustData = r;
                }
                else
                {
                    uint sz = inputBuffer[index];
                    index += 4;

                    forestTrustInfo.Records[i].ForestTrustData = ExtractBinary(inputBuffer, index, sz);
                    index += sz;
                }
            }

            return true;
        }


        /// <summary>
        /// The IsDomainNameInTrustedForest procedure returns true 
        /// if the domain identified by name is in a forest trusted by the caller's forest, 
        /// as determined by the FOREST_TRUST_INFORMATION state of the caller's forest, and false otherwise.
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="name">Either an FQDN or a NetBIOS name of a domain</param>
        /// <param name="referredDomain">If returns true, 
        /// will be set to the FQDN of the root domain of the forest of the domain specified by name.</param>
        /// <returns></returns>
        public static bool IsDomainNameInTrustedForest(DsServer dc, string name, ref string referredDomain)
        {
            if (IsDomainDnsNameInTrustedForest(dc, name, ref referredDomain))
                return true;

            if (IsDomainNetbiosNameInTrustedForest(dc, name, ref referredDomain))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if the domain identified by sid is in a forest trusted by the caller's forest, 
        /// as determined by the FOREST_TRUST_INFORMATION state of the caller's forest, false otherwise. 
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="sid">The SID of a domain.</param>
        /// <returns></returns>
        static bool IsDomainSidInTrustedForest(DsServer dc, NT4SID sid)
        {
            FOREST_TRUST_INFORMATION f;
            bool b;

            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string[] tdos = LdapUtility.GetAttributeValuesString(
                dc,
                rootDse.rootDomainNamingContext,
                "distinguishedName",
                "(&(objectClass=trustedDomain)(msDS-TrustForestTrustInfo=*)(trustAttributes:1.2.840.113556.1.4.803:=0x8))",
                System.DirectoryServices.Protocols.SearchScope.Subtree);

            foreach (string o in tdos)
            {
                byte[] trustInfo = LdapUtility.GetAttributeValueInBytes(dc, o, "msDS-TrustForestTrustInfo");
                if (!TrustInfo.UnmarshalForestTrustInfo(trustInfo, out f))
                    return false;

                foreach (Record e in f.Records)
                {
                    if (e.RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo
                        && (DrsrHelper.IsByteArrayEqual(sid.Data, ((RecordDomainInfo)e.ForestTrustData).Sid.Data))
                        && ((e.Flags & TrustInfo.LSA_FTRECORD_DISABLED_REASONS) == 0))
                    {
                        b = true;
                        foreach (Record g in f.Records)
                        {
                            if (g.RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelNameEx
                                && (g.Flags & TrustInfo.LSA_FTRECORD_DISABLED_REASONS) == 0
                                && (
                                    ((RecordTopLevelName)g.ForestTrustData).TopLevelName 
                                    == ((RecordDomainInfo)e.ForestTrustData).DnsName
                                    || 
                                    TrustInfo.IsSubdomainOf(
                                        ((RecordDomainInfo)e.ForestTrustData).DnsName, 
                                        ((RecordTopLevelName)g.ForestTrustData).TopLevelName)
                                   )
                                )
                            {
                                b = false;
                                break;
                            }
                        }

                        if (b)
                            return true;
                    }
                }
            }
            return false;
        }

        static bool IsDomainNetbiosNameInTrustedForest(DsServer dc, string name, ref string referredDomain)
        {
            FOREST_TRUST_INFORMATION f;
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string[] tdos = LdapUtility.GetAttributeValuesString(
                dc,
                rootDse.rootDomainNamingContext,
                "distinguishedName",
                "(&(objectClass=trustedDomain)(msDS-TrustForestTrustInfo=*)(trustAttributes:1.2.840.113556.1.4.803:=0x8))",
                System.DirectoryServices.Protocols.SearchScope.Subtree);

            if (tdos == null)
                return false;

            foreach (string o in tdos)
            {
                byte[] trustInfo = LdapUtility.GetAttributeValueInBytes(dc, o, "msDS-TrustForestTrustInfo");
                if (!TrustInfo.UnmarshalForestTrustInfo(trustInfo, out f))
                    return false;

                foreach (Record e in f.Records)
                {
                    if (e.RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo)
                    {
                        RecordDomainInfo ee = (RecordDomainInfo)e.ForestTrustData;
                        if (ee.NetbiosName == name
                            && (e.Flags & (uint)TrustInfo.FOREST_TRUST_RECORD_FLAGS_DOMAIN_INFO.NETBIOS_DISABLED_MASK) == 0
                            && ForestTrustOwnsName(f, ee.NetbiosName))
                        {
                            referredDomain = LdapUtility.GetAttributeValueInString(dc, o, "trustPartner");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        static bool IsDomainDnsNameInTrustedForest(DsServer dc, string name, ref string referredDomain)
        {
            FOREST_TRUST_INFORMATION f;
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string[] tdos = LdapUtility.GetAttributeValuesString(
                dc,
                rootDse.rootDomainNamingContext,
                "distinguishedName",
                "(&(objectClass=trustedDomain)(msDS-TrustForestTrustInfo=*)(trustAttributes:1.2.840.113556.1.4.803:=0x8))",
                System.DirectoryServices.Protocols.SearchScope.Subtree);

            if (tdos == null)
                return false;

            foreach (string o in tdos)
            {
                byte[] trustInfo = LdapUtility.GetAttributeValueInBytes(dc, o, "msDS-TrustForestTrustInfo");
                if (!TrustInfo.UnmarshalForestTrustInfo(trustInfo, out f))
                    return false;

                foreach (Record e in f.Records)
                {
                    if (e.RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo)
                    {
                        RecordDomainInfo ee = (RecordDomainInfo)e.ForestTrustData;
                        if (ee.DnsName == name
                            && (e.Flags & (uint)TrustInfo.FOREST_TRUST_RECORD_FLAGS_DOMAIN_INFO.LSA_SID_DISABLED_ADMIN) == 0
                            && (e.Flags & (uint)TrustInfo.FOREST_TRUST_RECORD_FLAGS_DOMAIN_INFO.LSA_SID_DISABLED_CONFLICT) == 0
                            && ForestTrustOwnsName(f, ee.DnsName))
                        {
                            referredDomain = LdapUtility.GetAttributeValueInString(dc, o, "trustPartner");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        static bool ForestTrustOwnsName(FOREST_TRUST_INFORMATION f, string name)
        {
            foreach (Record e in f.Records)
            {
                if (e.RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelNameEx
                    && (((RecordTopLevelName)e.ForestTrustData).TopLevelName == name)
                        || IsSubdomainOf(name, ((RecordTopLevelName)e.ForestTrustData).TopLevelName)) 
                    return false;

                if (e.RecordType == (byte)FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelName
                    && (e.Flags & LSA_FTRECORD_DISABLED_REASONS) == 0
                    && (((RecordTopLevelName)e.ForestTrustData).TopLevelName == name)
                        || IsSubdomainOf(name, ((RecordTopLevelName)e.ForestTrustData).TopLevelName))
                    return true;
            }

            return false;
        }
    }
}
