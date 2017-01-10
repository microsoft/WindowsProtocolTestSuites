// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.AZODPlugin
{
    /// <summary>
    /// Get Summary of the detection information.
    /// </summary>
    public class SummaryBuilder
    {
        PtfConfig ptfcfg;
        public SummaryBuilder(PtfConfig ptfconfig)
        {
            ptfcfg = ptfconfig;
        }

        private static void AddMachine(StringBuilder sb, string role, string machineName, string ipAddress, string password)
        {
            if (password != null)
            {
                if (!string.IsNullOrWhiteSpace(machineName))
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", role, machineName, ipAddress, password);
                else
                    sb.AppendFormat("<tr><td>{0}</td><td colspan=\"3\">Not Exist</td></tr>", role);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(machineName))
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", role, machineName, ipAddress);
                else
                    sb.AppendFormat("<tr><td>{0}</td><td colspan=\"2\">Not Exist</td></tr>", role);
            }
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
            sb.AppendFormat("Domain Name: {0}<br />", ptfcfg.KdcDomainName);
            sb.AppendFormat("Domain Administrator<br /><div class=\"Context\" style=\"margin-left:21px\">Username: {0}<br />Password: {1}</div>", ptfcfg.KdcAdminUser, System.Security.SecurityElement.Escape(ptfcfg.KdcAdminPwd));
            sb.Append("</div>");
            sb.Append("<table>");
            sb.Append("<th>Role</th><th>Machine Name</th><th>IP Address</th><th>Machine Account Password</th>");

            AddMachine(sb, "DC01", ptfcfg.KdcName, ptfcfg.KDCIP, ptfcfg.KdcAdminPwd);
            AddMachine(sb, "DC02", ptfcfg.CrossForestDCName, ptfcfg.CrossForestDCIP, ptfcfg.KdcAdminPwd);
            AddMachine(sb, "AP01", ptfcfg.ApplicationServerName, ptfcfg.ApplicationServerIP, ptfcfg.KdcAdminPwd);
            AddMachine(sb, "AP02", ptfcfg.CrossForestApplicationServerName, ptfcfg.CrossForestApplicationServerIP, ptfcfg.KdcAdminPwd);
            AddMachine(sb, "Client", ptfcfg.ClientComputerName, ptfcfg.ClientComputerIp, ptfcfg.ClientAdminPwd);

            sb.Append("</table>");
            sb.Append("<br/>");
            sb.Append("</body></html>");
            return sb.ToString();
        }
    }

}
