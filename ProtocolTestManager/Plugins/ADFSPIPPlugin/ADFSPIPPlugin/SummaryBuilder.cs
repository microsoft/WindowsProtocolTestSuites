// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.ADFSPIPPlugin
{
    public class SummaryBuilder
    {
        PtfConfig ptfcfg;
        public SummaryBuilder(PtfConfig ptfconfig)
        {
            ptfcfg = ptfconfig;
        }

        public string GetHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta charset=\"utf-8\" /><title>Summary</title>");
            sb.Append("<style>body{font-family:Arial;}");
            sb.Append("div.Title{font-size:larger;margin-top:10px;margin-bottom:10px;font-weight:bold;}");
            sb.Append("div.Context{font-size:medium;margin-top:auto}");
            sb.Append("div.Subtitle{font-size:medium;margin-top:auto;font-weight:bold}");
            sb.Append("</style></head><body>");
            sb.Append("<div class=\"Title\">Environment Summary</div>");

            sb.Append("<h3>Domain</h3>");
            sb.Append("<div class=\"Context\" style=\"margin-left:21px\">");
            sb.AppendFormat("Domain Name: {0}<br />", ptfcfg.DomainName);
            sb.AppendFormat("Administrator Username: {0}<br />", ptfcfg.DomainUsername);
            sb.AppendFormat("Password: {0}", System.Security.SecurityElement.Escape(ptfcfg.DomainPassword));
            sb.Append("</div>");

            sb.Append("<h3>SUT:</h3>");
            sb.Append("<div class=\"Context\" style=\"margin-left:21px\">");
            sb.AppendFormat("IP Address: {0}<br />DNS: {1}<br />", ptfcfg.SutIPAddress, ptfcfg.SutDns);
            sb.AppendFormat("Username: {0}<br />Password: {1}", ptfcfg.SutUsername, ptfcfg.SutPassword);
            sb.Append("</div>");

            sb.Append("<h3>ADFS:</h3>");
            sb.Append("<div class=\"Context\" style=\"margin-left:21px\">");
            sb.AppendFormat("DNS: {0}<br />", ptfcfg.AdfsDns);
            sb.AppendFormat("ADFS Cert: {0}<br />", Utility.FilePath(ptfcfg.AdfsCert));
            sb.AppendFormat("Sign Cert: {0}<br />", Utility.FilePath(ptfcfg.AdfsSignCert));
            sb.AppendFormat("Encrypt Cert: {0}<br />", Utility.FilePath(ptfcfg.AdfsEncryptCert));
            sb.AppendFormat("Windows version: {0}", ptfcfg.AdfsIsWin2016 == "true" ? "2016" : "2012R2");
            sb.Append("</div>");

            sb.Append("<h3>Web App:</h3>");
            sb.Append("<div class=\"Context\" style=\"margin-left:21px\">");
            sb.AppendFormat("Web App Cert: {0}<br />", Utility.FilePath(ptfcfg.WebAppCert));
            sb.AppendFormat("Client Cert: {0}", Utility.FilePath(ptfcfg.WebAppClientCert));
            sb.Append("</div>");

            return sb.ToString();
        }
    }
}
