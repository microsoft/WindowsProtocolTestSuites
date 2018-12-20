// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.ADFamilyPlugin
{
    class SummaryBuilder
    {
        PtfConfig ptfcfg;
        public SummaryBuilder(PtfConfig ptfconfig)
        {
            ptfcfg = ptfconfig;
        }

        private static void AddMachine(StringBuilder sb, string role, string machineName, string ipAttress, string password)
        {
            if (password != null)
            {
                if (!string.IsNullOrWhiteSpace(machineName))
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", role, machineName, ipAttress, password);
                else
                    sb.AppendFormat("<tr><td>{0}</td><td colspan=\"3\">Not Exist</td></tr>", role);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(machineName))
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", role, machineName, ipAttress);
                else
                    sb.AppendFormat("<tr><td>{0}</td><td colspan=\"2\">Not Exist</td></tr>", role);
            }
        }

        private static string DomainFunctionLevel(string number)
        {
            switch(int.Parse(number))
            {
                case 0:
                    return "DS_BEHAVIOR_WIN2000 (0)";
                case 1:
                    return "DS_BEHAVIOR_WIN2000_WITH_MIXED_DOMAINS (1)";
                case 2:
                    return "DS_BEHAVIOR_WIN2003 (2)";
                case 3:
                    return "DS_BEHAVIOR_WIN2008 (3)";
                case 4:
                    return "DS_BEHAVIOR_WIN2008R2 (4)";
                case 5:
                    return "DS_BEHAVIOR_WIN2012 (5)";
                case 6:
                    return "DS_BEHAVIOR_WIN2012R2 (6)";
                case 7:
                    return "DS_BEHAVIOR_WINTHRESHOLD (7)";
                case 8:
                    return "DS_BEHAVIOR_WINv1803 (8)";
            }
            return string.Format("Unknown ({0})", number);
        }

        public string GetHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta charset=\"utf-8\" /><title>Summary</title>");
            sb.Append("<style>div.Title{font-family:Arial;font-size:larger;margin-bottom:10px;font-weight:bold;}");
            sb.Append("div.Context{font-family:'Segoe UI';font-size:medium;margin-top:auto}");
            sb.Append("div.Subtitle{font-family:'Segoe UI';font-size:medium;margin-top:auto;font-weight:bold}");
            sb.Append("table, th, td {font-family: 'Segoe UI';font-size: medium;border: 1px solid black;border-collapse: collapse;padding-left: 7px;padding-right: 7px;}");
            sb.Append("table {margin-left: 21px;margin-top: 21px;}");
            sb.Append("</style></head><body>");
            sb.Append("<div class=\"Title\">Environment Summary<br/></div>");
            sb.Append("<div class=\"Subtitle\">Primary Domain</div>");
            sb.Append("<div class=\"Context\">");
            sb.AppendFormat("Domain Name: {0}<br />", ptfcfg.PrimaryDomainDnsName);
            sb.AppendFormat("Domain Administrator<br /><div class=\"Context\" style=\"margin-left:21px\">Username: {0}<br />Password: {1}</div>", ptfcfg.DomainAdminName, System.Security.SecurityElement.Escape(ptfcfg.DomainUserPassword));
            sb.AppendFormat("Domain Function Level: {0}<br />", DomainFunctionLevel(ptfcfg.DomainFunctionLevel));
            sb.AppendFormat("Supported SASL Mechanisms: {0}<br />", ptfcfg.PdcSupportedSaslMechanisms);
            sb.Append("</div>");
            sb.Append("<table>");
            sb.Append("<th>Role</th><th>Machine Name</th><th>IP Address</th><th>Machine Account Password</th>");

            AddMachine(sb, "DC01", ptfcfg.Dc1NetBiosName, ptfcfg.Dc1IpAddress, ptfcfg.Dc1PasswordVerified ? ptfcfg.Dc1Password : "&lt;Not Detected&gt;");
            AddMachine(sb, "DC02", ptfcfg.Dc2NetbiosName, ptfcfg.Dc2IpAddress, ptfcfg.Dc2PasswordVerified ? ptfcfg.Dc2Password : "&lt;Not Detected&gt;");
            AddMachine(sb, "Readonly DC", ptfcfg.RodcNetbiosName, ptfcfg.RodcIpAddress, ptfcfg.RodcPasswordVerified ? ptfcfg.RodcPassword : "&lt;Not Detected&gt;");
            AddMachine(sb, "Domain Member", ptfcfg.DmNetbiosName, ptfcfg.DmIpAddress, ptfcfg.DmPasswordVerified ? ptfcfg.DmPassword : "&lt;Not Detected&gt;");
            AddMachine(sb, "Driver", ptfcfg.EndpointNetbiosName, ptfcfg.EndpointIPAddress, ptfcfg.EndpointPasswordVerified ? ptfcfg.EndpointPassword : "&lt;Not Detected&gt;");

            sb.Append("</table>");
            sb.Append("<br/>");
            if (string.IsNullOrWhiteSpace(ptfcfg.ChildDomainDnsName))
            {
                sb.Append("<div class=\"Subtitle\">Child Domain:</div><div class=\"Context\"> Not Exist<br/></div>");
            }
            else
            {
                sb.Append("<div class=\"Subtitle\">Child Domain</div>");
                sb.Append("<div class=\"Context\">");
                sb.AppendFormat("Domain name: {0}<br />", ptfcfg.ChildDomainDnsName);
                sb.AppendFormat("Domain Administrator<br /><div class=\"Context\" style=\"margin-left:21px\">Username: {0}<br />Password: {1}</div>", ptfcfg.DomainAdminName, System.Security.SecurityElement.Escape(ptfcfg.DomainUserPassword));
                sb.Append("</div>");
                sb.Append("<table><th>Role</th><th>Machine Name</th><th>IP Address</th>");
                AddMachine(sb, "Child Domain DC", ptfcfg.CdcNetbiosName, ptfcfg.CdcIpAddress, null);
                sb.Append("</table><br/>");
            }
            if (string.IsNullOrWhiteSpace(ptfcfg.TrustDomainDnsName))
            {
                sb.Append("<div class=\"Subtitle\">Trust Domain:</div><div class=\"Context\"> Not Exist<br/></div>");
            }
            else
            {
                sb.Append("<div class=\"Subtitle\">Trust Domain</div>");
                sb.Append("<div class=\"Context\">");
                sb.AppendFormat("<dt>Domain name: {0}</dt>", ptfcfg.TrustDomainDnsName);
                sb.AppendFormat("<dt>Domain Administrator<br/><div class=\"Context\" style=\"margin-left:21px\">Username: {0}<br/>Password: {1}</div>", ptfcfg.DomainAdminName, System.Security.SecurityElement.Escape(ptfcfg.DomainUserPassword));
                sb.Append("</div>");
                sb.Append("<table><th>Role</th><th>Machine Name</th><th>IP Address</th>");
                AddMachine(sb, "Trust Domain DC", ptfcfg.TdcNetbiosName, ptfcfg.TdcIpAddress, null);
                sb.Append("</table><br/>");
            }
            sb.Append("</body></html>");
            return sb.ToString();
        }
    }

}
