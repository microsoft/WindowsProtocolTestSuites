// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.AZODPlugin
{
    public class PtfConfig
    {
        [PtfConfig("KdcDomainName")]
        public string KdcDomainName { get; set; }

        [PtfConfig("KdcName")]
        public string KdcName { get; set; }

        [PtfConfig("KDCIP")]
        public string KDCIP { get; set; }

        [PtfConfig("KdcAdminUser")]
        public string KdcAdminUser { get; set; }

        [PtfConfig("KdcAdminPwd")]
        public string KdcAdminPwd { get; set; }

        [PtfConfig("KdcClaimUser")]
        public string KdcClaimUser { get; set; }

        [PtfConfig("KdcClaimUserPwd")]
        public string KdcClaimUserPwd { get; set; }

        [PtfConfig("ApplicationServerName")]
        public string ApplicationServerName { get; set; }

        [PtfConfig("ApplicationServerIP")]
        public string ApplicationServerIP { get; set; }

        [PtfConfig("FQDNUncPath")]
        public string FQDNUncPath { get; set; }

        [PtfConfig("UncPath")]
        public string UncPath { get; set; }

        [PtfConfig("ClientComputerName")]
        public string ClientComputerName { get; set; }

        [PtfConfig("ClientComputerIp")]
        public string ClientComputerIp { get; set; }

        public bool Dc2PasswordVerified = false;

        [PtfConfig("ClientAdminUser")]
        public string ClientAdminUser { get; set; }

        [PtfConfig("ClientAdminPwd")]
        public string ClientAdminPwd { get; set; }

        [PtfConfig("CrossForestName")]
        public string CrossForestName { get; set; }

        [PtfConfig("CrossForestDCName")]
        public string CrossForestDCName { get; set; }

        public bool RodcPasswordVerified = false;

        [PtfConfig("CrossForestDCIP")]
        public string CrossForestDCIP { get; set; }

        [PtfConfig("CrossForestAdminUser")]
        public string CrossForestAdminUser { get; set; }

        [PtfConfig("CrossForestAdminPwd")]
        public string CrossForestAdminPwd { get; set; }

        [PtfConfig("CrossForestApplicationServerName")]
        public string CrossForestApplicationServerName { get; set; }

        [PtfConfig("CrossForestApplicationServerIP")]
        public string CrossForestApplicationServerIP { get; set; }

        [PtfConfig("CrossForestApplicationServerShareFolder")]
        public string CrossForestApplicationServerShareFolder { get; set; }

        [PtfConfig("ScriptPath")]
        public string ScriptPath { get; set; }

        [PtfConfig("LocalCapFilePath")]
        public string LocalCapFilePath { get; set; }

        [PtfConfig("ExpectedSequenceFilePath")]
        public string ExpectedSequenceFilePath { get; set; }

        [PtfConfig("DriverLogPath")]
        public string DriverLogPath { get; set; }

        [PtfConfig("MaxSMB2DialectSupported")]
        public string MaxSMB2DialectSupported { get; set; }

        [PtfConfig("SiteName")]
        public string SiteName { get; set; }

        public bool DmPasswordVerified = false;

        [PtfConfig("CentralAccessPolicyNames")]
        public string CentralAccessPolicyNames { get; set; }

        [PtfConfig("CentralAccessRuleNames")]
        public string CentralAccessRuleNames { get; set; }

        [PtfConfig("ResourcepropertyNames")]
        public string ResourcepropertyNames { get; set; }

        [PtfConfig("Timeout")]
        public string Timeout { get; set; }

        public bool EndpointPasswordVerified = false;

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
