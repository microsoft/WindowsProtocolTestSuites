// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.RDPServerPlugin
{
    class Configs
    {
        public string ServerDomain { get; set; }

        public string ServerName { get; set; }

        public string ServerPort { get; set; }

        public string ServerUserName { get; set; }

        public string ServerUserPassword { get; set; }

        public string ClientName { get; set; }

        public string Version { get; set; }

        public void LoadDefaultValues()
        {
            Type cfg = typeof(Configs);
            foreach (var p in cfg.GetProperties())
            {
                string name = "RDP." + p.Name.Replace("__", ".");
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
                string name = "RDP." + p.Name.Replace("__", ".");
                string value = p.GetValue(this, null).ToString();
                dict.Add(name, new List<string>() { value });
            }
            return dict;
        }
    }
}
