// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.PublishDc
{
    public class PublishDCAdapter : ADCommonServerAdapter
    {
        #region Variables
        public string AdLdsAppNamingContext;
        public bool IsSutGc = true;
        public bool IsSutKdc = true;
        public bool IsSutPdc = true;
        public bool IsSutHostingWin32TimeService = true;
        public bool IsSutInSameSiteAsClient = true;
        public bool IsSutRodc = false;
        public bool IsSutWritableDc = true;
        public string PDCFullName;
        public string SDCFullName;
        public string RODCFullName;
        public string CDCFullName;
        public string DomainNC;
        public string ActualDcSiteName = "Default-First-Site-Name";
        public string AdLdsSite = "site:default-first-site-name";
        public string SamLogonResponseExFilter = "ADTS.NetlogonSamLogonResponseEx";
        public string SamLogonResponseFilter = "ADTS.NetlogonSamLogonResponse";
        public string SbzFilter = "Sbz";
        public string FlagsFilter = "Flags";
        public string NtVersionFilter = "NtVersion";
        public string LmNtTokenFilter = "LmNtToken";
        public string Lm20TokenFilter = "Lm20Token";
        public string OpcodeFilter = "Opcode";
        public string DomainGuidFilter = "DomainGuid";
        public string DnsForestNameFilter = "DnsForestName";
        public string DnsDomainNameFilter = "DnsDomainName";
        public string DnsHostNameFilter = "DnsHostName";
        public string NetbiosDomainNameFilter = "NetbiosDomainName";
        public string NetbiosComputerNameFilter = "NetbiosComputerName";
        public string DcSiteNameFilter = "DcSiteName";
        public string ClientSiteNameFilter = "ClientSiteName";
        public string DcSockAddrFilter = "DcSockAddr";
        public string DcSockAddrSizeFilter = "DcSockAddrSize";
        public string SinFamilyFilter = "sin_family";
        public string SinPortFilter = "sin_port";
        public string SinAddrFilter = "sin_addr";
        public string SinZeroFilter = "sin_zero";
        public string NullGuidFilter = "NullGuid";
        public string UnicodeLogonServerFilter = "UnicodeLogonServer";
        public string UnicodeDomainNameFilter = "UnicodeDomainName";
        public string UnicodeUserNameFilter = "UnicodeUserName";
        public uint DS_PDC_FLAG = 0x00000001;
        public uint DS_Reserved = 0x00000002;
        public uint DS_GC_FLAG = 0x00000004;
        public uint DS_LDAP_FLAG = 0x00000008;
        public uint DS_DS_FLAG = 0x00000010;
        public uint DS_KDC_FLAG = 0x00000020;
        public uint DS_TIMESERV_FLAG = 0x00000040;
        public uint DS_CLOSEST_FLAG = 0x00000080;
        public uint DS_WRITABLE_FLAG = 0x00000100;
        public uint DS_SELECT_SECRET_DOMAIN_6_FLAG = 0x00000800;
        public uint DS_FULL_SECRET_DOMAIN_6_FLAG = 0x00001000;
        public uint DS_DS_8_FLAG = 0x00004000;
        public uint DS_DNS_CONTROLLER_FLAG = 0x20000000;
        public uint DS_DNS_DOMAIN_FLAG = 0x40000000;
        public uint DS_DNS_FOREST_FLAG = 0x80000000;

        #endregion

        #region Initialize and Reset
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            AdLdsAppNamingContext = "partition:" + LDSApplicationNC;
            PDCFullName = string.Format("{0}.{1}",PDCNetbiosName,PrimaryDomainDnsName);
            SDCFullName = string.Format("{0}.{1}",SDCNetbiosName,PrimaryDomainDnsName);
            RODCFullName = string.Format("{0}.{1}",RODCNetbiosName,PrimaryDomainDnsName);
            CDCFullName = string.Format("{0}.{1}",CDCNetbiosName,ChildDomainDnsName);
            DomainNC = "DC=" + PrimaryDomainDnsName.Replace(".",",DC=");
        }

        public override void Reset()
        {
            base.Reset();
        }

        private string dsaGuid = null;

        public string DsaGuid
        {
            get
            {
                if (!String.IsNullOrEmpty(dsaGuid)) return dsaGuid;
                using(DirectoryEntry parent = new DirectoryEntry(string.Format("LDAP://CN={0},OU=Domain Controllers,{1}",PDCNetbiosName,DomainNC)))
                {
                    foreach(DirectoryEntry entry in parent.Children)
                    {
                        if (entry.Properties["distinguishedName"].Value.ToString().StartsWith("CN={"))
                        {
                            dsaGuid = entry.Properties["cn"].Value.ToString();
                            break;
                        }
                    }
                }
                return dsaGuid; 
            }
        }

        private string pdcDnsAlias = null;

        public string PdcDnsAlias
        {
            get
            {
                if (!String.IsNullOrEmpty(pdcDnsAlias)) return pdcDnsAlias;
                pdcDnsAlias = Utilities.GetAttributeFromEntry(
                  string.Format("CN=NTDS Settings,CN={0},CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,{1}", PDCNetbiosName, DomainNC),
                  "objectGUID",
                  PDCNetbiosName,
                  ADDSPortNum,
                  DomainAdministratorName,
                  DomainUserPassword).ToString();
                return pdcDnsAlias;
            }
        }
        #endregion
    }
}
