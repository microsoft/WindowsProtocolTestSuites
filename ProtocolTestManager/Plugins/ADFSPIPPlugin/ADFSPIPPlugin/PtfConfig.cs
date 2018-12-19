// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.ADFSPIPPlugin
{
    public class PtfConfig
    {
        #region Domain
        [PtfConfig("Domain.DomainName")]
        public string DomainName { get; set; }

        [PtfConfig("Domain.Username")]
        public string DomainUsername { get; set; }

        [PtfConfig("Domain.Password")]
        public string DomainPassword { get; set; }
        #endregion

        #region SUT
        [PtfConfig("SUT.SutIPAddress")]
        public string SutIPAddress { get; set; }

        [PtfConfig("SUT.SutDns")]
        public string SutDns { get; set; }

        [PtfConfig("SUT.MaxDelaySeconds")]
        public string SutMaxDelaySeconds { get; set; }

        [PtfConfig("SUT.IsWindows")]
        public string SutIsWindows { get; set; }

        [PtfConfig("SUT.Username")]
        public string SutUsername { get; set; }

        [PtfConfig("SUT.Password")]
        public string SutPassword { get; set; }
        #endregion

        #region ADFS
        [PtfConfig("ADFS.AdfsName")]
        public string AdfsName { get; set; }

        [PtfConfig("ADFS.AdfsDns")]
        public string AdfsDns { get; set; }

        [PtfConfig("ADFS.AdfsUrl")]
        public string AdfsUrl { get; set; }

        [PtfConfig("ADFS.AdfsCert")]
        public string AdfsCert { get; set; }

        [PtfConfig("ADFS.SignCert")]
        public string AdfsSignCert { get; set; }

        [PtfConfig("ADFS.EncryptCert")]
        public string AdfsEncryptCert { get; set; }

        [PtfConfig("ADFS.IsWin2016")]
        public string AdfsIsWin2016 { get; set; }

        [PtfConfig("ADFS.StoreEntryKey_2012R2")]
        public string AdfsStoreEntryKey2012R2 { get; set; }

        [PtfConfig("ADFS.StoreEntryKey_2016")]
        public string AdfsStoreEntryKey2016 { get; set; }

        [PtfConfig("ADFS.MetadataTemplate_2012R2")]
        public string AdfsMetadataTemplate2012R2 { get; set; }

        [PtfConfig("ADFS.MetadataTemplate_2016")]
        public string AdfsMetadataTemplate2016 { get; set; }
        #endregion

        #region WebApp
        [PtfConfig("WebApp.ProxyHostName")]
        public string WebAppProxyHostName { get; set; }

        [PtfConfig("WebApp.App1Name")]
        public string WebApp1Name { get; set; }

        [PtfConfig("WebApp.App2Name")]
        public string WebApp2Name { get; set; }

        [PtfConfig("WebApp.App1Url")]
        public string WebApp1Url { get; set; }

        [PtfConfig("WebApp.App2Url")]
        public string WebApp2Url { get; set; }

        [PtfConfig("WebApp.AppUserUpn")]
        public string WebAppUserUpn { get; set; }

        [PtfConfig("WebApp.AppUserName")]
        public string WebAppUserName { get; set; }

        [PtfConfig("WebApp.AppUserPassword")]
        public string WebAppUserPassword { get; set; }

        [PtfConfig("WebApp.WebAppCert")]
        public string WebAppCert { get; set; }

        [PtfConfig("WebApp.ClientCert")]
        public string WebAppClientCert { get; set; }
        #endregion

        #region Common
        [PtfConfig("Common.PfxPassword")]
        public string CommonPfxPassword { get; set; }

        [PtfConfig("Common.DataStorePath")]
        public string CommonDataStorePath { get; set; }

        [PtfConfig("Common.DriverShareFolder")]
        public string CommonDriverShareFolder { get; set; }

        [PtfConfig("Common.SutShareFolder")]
        public string CommonSutShareFolder { get; set; }

        [PtfConfig("Common.ProxyCert")]
        public string CommonProxyCert { get; set; }

        [PtfConfig("Common.RealADFSIP")]
        public string CommonRealADFSIP { get; set; }
        #endregion


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
