// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.RDPServerPlugin
{
    public class Configs
    {
        public string ServerDomain { get; set; }

        public string ServerName { get; set; }

        public string ServerPort { get; set; }

        public string ServerUserName { get; set; }

        public string ServerUserPassword { get; set; }

        public string ClientName { get; set; }

        public string Version { get; set; }

        public string SecurityProtocol { get; set; }
       
        public string RDPELESupported { get; set; }
   
        public string RDPEDYCSupported { get; set; }
      
        public void LoadDefaultValues()
        {
            Type cfg = typeof(Configs);
            foreach (var p in cfg.GetProperties())
            {
                string name = "";
                if (String.Compare(p.Name.ToLower(), "tlsversion") == 0)
                {
                    name = "RDP.Security.TLS.Version";
                }
                else if (String.Compare(p.Name.ToLower(), "securityprotocol") == 0)
                {
                    name = "RDP.Security.Protocol";
                }
                else if (String.Compare(p.Name.ToLower(), "negotiation") == 0)
                {
                    name = "RDP.Security.Negotiation";
                }
                else if (String.Compare(p.Name.ToLower(), "rdpelesupported") == 0)
                {
                    name = "RDPELESupported";
                }
                else if (String.Compare(p.Name.ToLower(), "rdpedycsupported") == 0)
                {
                    name = "RDPEDYCSupported";
                }
                else
                {
                    name = "RDP." + p.Name.Replace("__", ".");
                }

             
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
                string name = "";
                if ((String.Compare(p.Name.ToLower(), "tlsversion") == 0))
                {
                    name = "RDP.Security.TLS.Version";
                }
                else if ((String.Compare(p.Name.ToLower(), "securityprotocol") == 0))
                {
                    name = "RDP.Security.Protocol";
                }
                else if ((String.Compare(p.Name.ToLower(), "negotiation") == 0))
                {
                    name = "RDP.Security.Negotiation";
                }
                else if (String.Compare(p.Name.ToLower(), "rdpelesupported") == 0)
                {
                    name = "RDPELESupported";
                }
                else if (String.Compare(p.Name.ToLower(), "rdpedycsupported") == 0)
                {
                    name = "RDPEDYCSupported";
                }
                else
                {
                    name = "RDP." + p.Name.Replace("__", ".");
                }
                string value = p.GetValue(this, null).ToString();
                dict.Add(name, new List<string>() { value });
            }
            return dict;
        }
    }
}
