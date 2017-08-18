// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.ADFamilyPlugin
{
    public class PtfConfig
    {
        [PtfConfig("Common.DomainAdministratorName")]
        public string DomainAdminName { get; set; }

        [PtfConfig("Common.DomainUserPassword")]
        public string DomainUserPassword { get; set; }

        [PtfConfig("Common.DomainFunctionLevel")]
        public string DomainFunctionLevel { get; set; }

        [PtfConfig("Common.PrimaryDomain.DNSName")]
        public string PrimaryDomainDnsName { get; set; }

        [PtfConfig("Common.PrimaryDomain.NetBiosName")]
        public string PrimaryDomainNetBiosName { get; set; }

        [PtfConfig("Common.PrimaryDomain.ServerGUID")]
        public string PrimaryDomainServerGuid { get; set; }

        [PtfConfig("Common.PrimaryDomain.SID")]
        public string PrimaryDomainSid { get; set; }

        [PtfConfig("Common.ADLDSPortNum")]
        public string AdldsPort { get; set; }

        [PtfConfig("Common.LDSApplicationNC")]
        public string LdsAppNc { get; set; }

        [PtfConfig("Common.WritableDC1.NetbiosName")]
        public string Dc1NetBiosName { get; set; }

        [PtfConfig("Common.WritableDC1.Password")]
        public string Dc1Password { get; set; }

        public bool Dc1PasswordVerified = false;

        [PtfConfig("Common.WritableDC1.IPAddress")]
        public string Dc1IpAddress { get; set; }

        [PtfConfig("Common.WritableDC1.OSVersion")]
        public string Dc1OsVersion { get; set; }

        [PtfConfig("Common.WritableDC2.NetbiosName")]
        public string Dc2NetbiosName { get; set; }

        [PtfConfig("Common.WritableDC2.Password")]
        public string Dc2Password { get; set; }

        public bool Dc2PasswordVerified = false;

        [PtfConfig("Common.WritableDC2.IPAddress")]
        public string Dc2IpAddress { get; set; }

        [PtfConfig("Common.WritableDC2.OSVersion")]
        public string Dc2OsVersion { get; set; }

        [PtfConfig("Common.RODC.NetbiosName")]
        public string RodcNetbiosName { get; set; }

        [PtfConfig("Common.RODC.Password")]
        public string RodcPassword { get; set; }

        public bool RodcPasswordVerified = false;

        [PtfConfig("Common.RODC.IPAddress")]
        public string RodcIpAddress { get; set; }

        [PtfConfig("Common.RODC.OSVersion")]
        public string RodcOsVersion { get; set; }

        [PtfConfig("Common.ChildDomain.DNSName")]
        public string ChildDomainDnsName { get; set; }

        [PtfConfig("Common.ChildDomain.NetBiosName")]
        public string ChildDomainNetBiosName { get; set; }

        [PtfConfig("Common.CDC.NetbiosName")]
        public string CdcNetbiosName { get; set; }

        [PtfConfig("Common.CDC.IPAddress")]
        public string CdcIpAddress { get; set; }

        [PtfConfig("Common.CDC.OSVersion")]
        public string CdcOsVersion { get; set; }

        [PtfConfig("Common.TrustDomain.DNSName")]
        public string TrustDomainDnsName { get; set; }

        [PtfConfig("Common.TrustDomain.NetBiosName")]
        public string TrustDomainNetBiosName { get; set; }

        [PtfConfig("Common.TDC.NetbiosName")]
        public string TdcNetbiosName { get; set; }

        [PtfConfig("Common.TDC.IPAddress")]
        public string TdcIpAddress { get; set; }

        [PtfConfig("Common.TDC.OSVersion")]
        public string TdcOsVersion { get; set; }

        [PtfConfig("Common.DM.NetbiosName")]
        public string DmNetbiosName { get; set; }

        [PtfConfig("Common.DM.Password")]
        public string DmPassword { get; set; }

        public bool DmPasswordVerified = false;

        [PtfConfig("Common.DM.IPAddress")]
        public string DmIpAddress { get; set; }

        [PtfConfig("Common.ENDPOINT.NetbiosName")]
        public string EndpointNetbiosName { get; set; }

        [PtfConfig("Common.ENDPOINT.IPAddress")]
        public string EndpointIPAddress { get; set; }

        [PtfConfig("Common.ENDPOINT.Password")]
        public string EndpointPassword { get; set; }

        [PtfConfig("Common.ClientUserName")]
        public string ClientUserName { get; set; }

        [PtfConfig("Common.ClientUserPassword")]
        public string ClientUserPassword { get; set; }

        [PtfConfig("MS_ADTS_Schema.TDXmlPath")]
        public string SchemaTdPath { get; set; }

        [PtfConfig("MS_ADTS_Schema.LdsTDXmlPath")]
        public string SchemaLdsTdPath { get; set; }

        [PtfConfig("MS_ADTS_Schema.OpenXmlPath2016")]
        public string SchemaOpenXmlPath { get; set; }

        [PtfConfig("MS_ADTS_Schema.LdsOpenXmlPath2016")]
        public string SchemaLdsOpenXmlPath { get; set; }

        [PtfConfig("MS_LSAT.SUT.Server.Computer.Name")]
        public string LsatSutName { get; set; }

        [PtfConfig("MS_NRPC.SUT.SubnetNames.IP.V4")]
        public string NrpcSutSubnet { get; set; }

        [PtfConfig("MS_FRS2.ReplicationDirectoryName")]
        public string ReplicaDirName { get; set; }

        public bool EndpointPasswordVerified = false;

        public string PdcSupportedSaslMechanisms;

        public void LoadDefaultValues()
        {
            Type cfg = typeof(PtfConfig);
            foreach (var p in cfg.GetProperties())
            {
                PtfConfigAttribute configAttribute = p.GetCustomAttributes(typeof(PtfConfigAttribute), false).FirstOrDefault() as PtfConfigAttribute;
                if (configAttribute == null) continue;
                var val = DetectorUtil.GetPropertyValue(configAttribute.Name);
                if (val != null)
                {
                    p.SetValue(this, val, null);
                }
            }
        }

        public Dictionary<string, List<string>> ToDictionary()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            Type cfg = typeof(PtfConfig);
            foreach (var p in cfg.GetProperties())
            {
                PtfConfigAttribute configAttribute = p.GetCustomAttributes(typeof(PtfConfigAttribute), false).FirstOrDefault() as PtfConfigAttribute;
                if (configAttribute == null) continue;
                string value = p.GetValue(this, null).ToString();
                dict.Add(configAttribute.Name, new List<string>() { value });
            }
            return dict;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PtfConfigAttribute : Attribute
    {
        public string Name { get; private set; }
        public PtfConfigAttribute(string name)
        {
            Name = name;
        }
    }
}
