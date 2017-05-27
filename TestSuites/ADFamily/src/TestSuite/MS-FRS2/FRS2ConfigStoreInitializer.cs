// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestSuites.MS_FRS2;
using Microsoft.Protocols.TestTools;
using System.Reflection;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.MS_FRS2
{
    public class FRS2ConfigStoreInitializer : ManagedAdapterBase, IConfigStoreInitializerAdapter
    {
        public void UpdateConfigStore(TestTools.ITestSite s)
        {
            string[] keys = s.Properties.AllKeys;
            List<string> unusedKeys = new List<string>();
            foreach (string str in keys)
            {
                if (!str.StartsWith("MS_FRS2."))
                    continue;
                string ss = str.Replace("MS_FRS2.", "");
                string refactorString = ss.Replace("-", "_");
                refactorString = refactorString[0].ToString().ToUpper() + refactorString.Substring(1);
                FieldInfo fi = typeof(ConfigStore).GetField(refactorString);
                if (fi != null)
                {
                    if (fi.FieldType == typeof(string))
                        fi.SetValue(null, s.Properties[str]);
                    else if (fi.FieldType == typeof(bool))
                        fi.SetValue(null, bool.Parse(s.Properties[str]));
                    else if (fi.FieldType == typeof(int))
                        fi.SetValue(null, int.Parse(s.Properties[str]));
                    else if (fi.FieldType == typeof(Guid))
                        fi.SetValue(null, new Guid(s.Properties[str]));
                }
                else if (ss.ToLower().StartsWith("reqimplemented."))
                    ConfigStore.WillingToCheckReq.Add(int.Parse(ss.ToLower().Replace("reqimplemented.", "")), bool.Parse(s.Properties[str]));
                else if (ss.StartsWith("MAYRequirements."))
                    ConfigStore.WillingToCheckReq.Add(int.Parse(ss.Replace("MAYRequirements.", "")), bool.Parse(s.Properties[str]));
                else
                    unusedKeys.Add(ss);
            }
            IFRS2ServerControllerAdapter serverControllerAdapter = s.GetAdapter<IFRS2ServerControllerAdapter>();
            ConfigStore.IsTestSYSVOL = true;
            if (ADCommonServerAdapter.Instance(s).PDCOSVersion >= ServerVersion.Win2012R2)
                ConfigStore.OSVer = SUTOSVersion.Win2012R2;
            else if (ADCommonServerAdapter.Instance(s).PDCOSVersion == ServerVersion.Win2012)
                ConfigStore.OSVer = SUTOSVersion.Win2012;
            else if (ADCommonServerAdapter.Instance(s).PDCOSVersion == ServerVersion.Win2008R2)
                ConfigStore.OSVer = SUTOSVersion.Win2008R2;
            ConfigStore.IsWindows = ADCommonServerAdapter.Instance(s).PDCIsWindows;
            ConfigStore.Should = false;
            ConfigStore.Win2k8 = false;
            ConfigStore.Win2K8R2 = true;
            ConfigStore.DomainDnsName = ADCommonServerAdapter.Instance(s).PrimaryDomainDnsName;
            ConfigStore.DomainNetbiosName = ADCommonServerAdapter.Instance(s).PrimaryDomainNetBiosName;

            // Check if SDC is set in ptfcofig file
            if (string.IsNullOrWhiteSpace(ADCommonServerAdapter.Instance(s).SDCNetbiosName))
            {
                s.Assume.Fail("The test requires a Secondary writable DC in the environment. Please set the corresponding field in PTF config. ");
            }
            ConfigStore.RepPartnerNetbiosName = ADCommonServerAdapter.Instance(s).SDCNetbiosName;
            ConfigStore.RepPartnerPassword = ADCommonServerAdapter.Instance(s).SDCPassword;
            ConfigStore.SutIp = ADCommonServerAdapter.Instance(s).PDCIPAddress;
            ConfigStore.SutLdapAddress = ConfigStore.SutIp + ":389";
            ConfigStore.SutDnsName = ADCommonServerAdapter.Instance(s).PDCNetbiosName + "." + ConfigStore.DomainDnsName;

            ConfigStore.AdminName = ADCommonServerAdapter.Instance(s).DomainAdministratorName;
            ConfigStore.AdminPassword = ADCommonServerAdapter.Instance(s).DomainUserPassword;
            ConfigStore.RepPartnerNTDSConnectionGUID = serverControllerAdapter.GetNtdsConnectionGuid(FRS2Helpers.ComputeNTDSConnectionPath(ConfigStore.DomainDnsName, ConfigStore.RepPartnerNetbiosName), FRS2Helpers.ComputeNTDSConnectionPath(ConfigStore.DomainDnsName, ADCommonServerAdapter.Instance(s).PDCNetbiosName));
            s.Log.Add(LogEntryKind.Checkpoint, "Detected partner's NTDS Connection GUID is: " + ConfigStore.RepPartnerNTDSConnectionGUID);
            ConfigStore.TransportProtocol = "ncacn_ip_tcp";
            ConfigStore.FRS2RpcUuid = "5bc1ed07-f5f5-485f-9dfd-6fd0acf9a23c";
            ConfigStore.SPNUuid = "e3514235-4b06-11d1-ab04-00c04fc2dcd2";
            string domainDn = DrsrHelper.GetDNFromFQDN(ConfigStore.DomainDnsName);
            ConfigStore.SutDN = "CN=" + ADCommonServerAdapter.Instance(s).PDCNetbiosName + ",OU=Domain Controllers," + domainDn;
            ConfigStore.Ms_DFSRLocalSettings = "CN=DFSR-LocalSettings," + ConfigStore.SutDN;
            ConfigStore.Ms_DFSRSubscriber = "CN=Domain System Volume," + ConfigStore.Ms_DFSRLocalSettings;
            ConfigStore.Ms_DFSR_Subscription1 = (ConfigStore.Ms_DFSR_Subscription = "CN=SYSVOL Subscription," + ConfigStore.Ms_DFSRSubscriber);

            ConfigStore.Ms_DFSR_GlobalSettings = "CN=DFSR-GlobalSettings,CN=System," + domainDn;
            ConfigStore.Ms_DFSR_Content = (ConfigStore.MsDFSR_Content = "CN=Content,CN=Domain System Volume," + ConfigStore.Ms_DFSR_GlobalSettings);
            ConfigStore.Ms_DFSR_Topology = "CN=Topology,CN=Domain System Volume," + ConfigStore.Ms_DFSR_GlobalSettings;
            ConfigStore.Ms_DFSR_Member = "CN=" + ADCommonServerAdapter.Instance(s).PDCNetbiosName + "," + ConfigStore.Ms_DFSR_Topology;
            ConfigStore.ReplicaId5 = (ConfigStore.ReplicaId4 = (ConfigStore.ReplicaId3 = (ConfigStore.ReplicaId2 = (ConfigStore.ReplicaId1 = "CN=Domain System Volume," + ConfigStore.Ms_DFSR_GlobalSettings))));
            ConfigStore.ContentId6 = (ConfigStore.ContentId4 = (ConfigStore.ContentId3 = (ConfigStore.ContentId2 = (ConfigStore.ContentId1 = "CN=SYSVOL Share," + ConfigStore.Ms_DFSR_Content))));
            ConfigStore.ContentId5 = "84a453ae-7902-4063-b9d7-8921aa793b11";
            ConfigStore.ConnectionId1 = "f40bb5f0-aa59-4d01-b81c-735555955e3a";
            ConfigStore.ConnectionId2 = "e4cc5b30-20f4-4de1-bcc9-b2b66551d7da";
            ConfigStore.ConnectionId3 = "320a49a6-46b8-48b5-85e5-9ba7cbf47f57";
            ConfigStore.ConnectionId4 = "cf3d2bd9-f3cc-4310-89b9-ae5f5bedbf4c";
            ConfigStore.ConnectionId5 = "9f65fe10-bdb7-4d32-88ad-ec05fc86c016";
            ConfigStore.InvalidGUID = "cf3d2bd9-f3cc-4310-89b9-ae5f5bedbfff";
            ConfigStore.CreditsAvailable = "10";
            ConfigStore.HashRequested = "0";
            ConfigStore.Offset = "0";
            ConfigStore.Length = "200";
            ConfigStore.BufferSize = "262144";
            ConfigStore.Reqnoblocked536 = "true";
            ConfigStore.Reqnoblocked764 = "true";
            ConfigStore.AuthenticationSVC = "16";
        }
    }
}
