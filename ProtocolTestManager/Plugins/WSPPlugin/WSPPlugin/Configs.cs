// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    public class Configs
    {
        public string DomainName { get; set; }

        public string ServerComputerName { get; set; }

        public string ServerVersion { get; set; }
        public string ServerOffset { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string SharedPath { get; set; }

        public string CatalogName { get; set; }      

        public string ClientName { get; set; }
        public string ClientVersion { get; set; }        

        public string ClientOffset { get; set; }

        public string IsWDSInstalled { get; set; }

        public string IsServerWindows { get; set; }

        public string LanguageLocale { get; set; }


        public void LoadDefaultValues()
        {            
            this.DomainName = DetectorUtil.GetPropertyValue("DomainName");
            this.ServerComputerName = DetectorUtil.GetPropertyValue("ServerComputerName");
            this.ServerVersion = DetectorUtil.GetPropertyValue("ServerOSVersion");
            this.UserName = DetectorUtil.GetPropertyValue("UserName");
            this.Password = DetectorUtil.GetPropertyValue("Password");

            this.SharedPath = DetectorUtil.GetPropertyValue("SharedPath");
            this.CatalogName = DetectorUtil.GetPropertyValue("CatalogName");
            this.ServerOffset = DetectorUtil.GetPropertyValue("ServerOffset");

            this.ClientName = DetectorUtil.GetPropertyValue("ClientComputerName");
            this.ClientOffset = DetectorUtil.GetPropertyValue("ClientOffset");

            this.IsWDSInstalled = DetectorUtil.GetPropertyValue("IsWDSInstalled");
            this.IsServerWindows = DetectorUtil.GetPropertyValue("IsServerWindows");

            this.LanguageLocale = DetectorUtil.GetPropertyValue("LanguageLocale");
        }

        public Dictionary<string, List<string>> ToDictionary()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            Type cfg = typeof(Configs);
            foreach (var p in cfg.GetProperties())
            {                
                string value = p.GetValue(this, null).ToString();
                dict.Add(p.Name, new List<string>() { value });
            }
            return dict;
        }
    }
}
