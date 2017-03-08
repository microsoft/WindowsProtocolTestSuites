// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestManager.Detector;

namespace Microsoft.Protocols.TestManager.ADODPlugin
{
    class Configs
    {
        public string FullDomainName { get; set; }
        public string DomainNC { get; set; }
        public string PartitionsNC { get; set; }
        public string DomainAdminUsername { get; set; }
        public string DomainAdminPwd { get; set; }

        public string PDCOperatingSystem { get; set; }
        public string PDCComputerName { get; set; }
        public string PDCIP { get; set; }

        public string ClientOperatingSystem { get; set; }
        public string ClientOSVersion { get; set; }
        public string ClientComputerName { get; set; }
        public string ClientIP { get; set; }
        public string ClientAdminUsername { get; set; }
        public string ClientAdminPwd { get; set; }

        public string LocalCapFilePath { get; set; }
        public string DriverLogPath { get; set; }
        public string ClientScriptPath { get; set; }
        public string ClientLogPath { get; set; }
        public string TriggerDisabled { get; set; }
        public string TelnetPort { get; set; }
        public string LocateDomainControllerScript { get; set; }
        public string JoinDomainCreateAcctLDAPScript { get; set; }
        public string JoinDomainCreateAcctSAMRScript { get; set; }
        public string JoinDomainPredefAcctScript { get; set; }
        public string UnjoinDomainScript { get; set; }
        public string IsJoinDomainSuccessScript { get; set; }
        public string IsUnjoinDomainSuccessScript { get; set; }
        public string Timeout { get; set; }
        public string RetryInterval { get; set; }
        public string DomainNewUserUsername { get; set; }
        public string DomainNewUserPwd { get; set; }
        public string DomainNewUserSamAccountName { get; set; }
        public string DomainNewUserNewPwd { get; set; }
        public string DomainNewGroup { get; set; }
        public string JoinDomainCreateAcctLDAPSleepTime { get; set; }
        public string JoinDomainCreateAcctSAMRSleepTime { get; set; }
        public string JoinDomainPredefAcctSleepTime { get; set; }
        public string UnjoinDomainSleepTime { get; set; }

        public void LoadDefaultValues()
        {
            Type cfg = typeof(Configs);
            foreach (var p in cfg.GetProperties())
            {
                string name = p.Name.Replace("__", ".");
                var val = DetectorUtil.GetPropertyValue(name);
                if (val != null)
                {
                    p.SetValue(this, val, null);
                }
            }
        }

        public Dictionary<string, List<string>> ToDictionary()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            Type cfg = typeof(Configs);
            foreach (var p in cfg.GetProperties())
            {
                string name = p.Name.Replace("__", ".");
                string value = p.GetValue(this, null).ToString();
                dict.Add(name, new List<string>() { value });
            }
            return dict;
        }
    }
}
