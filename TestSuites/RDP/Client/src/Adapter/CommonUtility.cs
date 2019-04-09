// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using System.Reflection;
using System.Diagnostics;
using System.Net;

namespace Microsoft.Protocols.TestSuites
{
    /// <summary>
    /// Common helper class
    /// </summary>
    public static class CommonUtility
    {
        /// <summary>
        /// This method is used to retrieve the value from the PTFConfig
        /// </summary>
        /// <param name="site"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static string GetProperty(ITestSite site, string propName)
        {
            const string errMsg = "The property '{0}' cannot be found from the PTFConfg. "
                                + "Please verify it was set properly.";
            string propVal = site.Properties[propName];
            if (null == propVal)
            {
                site.Assume.Fail(errMsg, propName);
            }
            return propVal;
        }

        /// <summary>
        /// This method is used to retrieve the Int32 value from the PTFConfig
        /// </summary>
        /// <param name="site"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static int GetIntProperty(ITestSite site, string propName)
        {
            const string errMsg = "The value of property '{0}' must be an Int32 type. "
                                + "Please verify it was set properly.";

            string propVal = GetProperty(site, propName);
            int res;
            if (!int.TryParse(propVal, out res))
            {
                site.Assume.Fail(errMsg, propName);
            }

            return res;
        }

        /// <summary>
        /// This method is used to retrieve the Boolean value from the PTFConfig
        /// </summary>
        /// <param name="site"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static bool GetBoolProperty(ITestSite site, string propName)
        {
            const string errMsg = "The value of property '{0}' must be a Boolean type. "
                                + "Please verify it was set properly.";

            string propVal = GetProperty(site, propName);
            bool res;
            if (!bool.TryParse(propVal, out res))
            {
                site.Assume.Fail(errMsg, propName);
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static TEnum GetEnumProperty<TEnum>(ITestSite site, string propName) where TEnum : struct
        {
            const string errMsg = "The value of property '{0}' must be a {1} type. "
                                + "Please verify it was set properly.";

            string propVal = GetProperty(site, propName);
            TEnum res;
            if (!Enum.TryParse<TEnum>(propVal, true, out res))
            {
                site.Assume.Fail(errMsg, propName, typeof(TEnum).ToString());
            }

            return res;
        }

        private static Dictionary<string, List<int>> disabledReqsTable = null;

        /// <summary>
        /// This method checks if a requirement verification needs to be disabled.
        /// </summary>
        /// <param name="site">The related ITestSite instance.</param>
        /// <param name="requirementId">The requirement ID to be checked.</param>
        /// <returns>True indicates the requirement is disabled.</returns>
        public static bool IsRequirementVerificationDisabled(ITestSite site,string protocolName, int requirementId)
        {
            if(disabledReqsTable==null)
            {
                disabledReqsTable = new Dictionary<string, List<int>>();
            }
            if(!disabledReqsTable.ContainsKey(protocolName.ToLower()))
            {
                
              List<int> disabledRequirements = new List<int>();

                // Check if there is a ReqSwitch in the config.
              string strReqIds = site.Properties[ConfigPropNames.ReqSwitch + "." + protocolName];
                if (String.IsNullOrEmpty(strReqIds.Trim()))
                {
                    return false;
                }
                string[] ids = strReqIds.Split(new char[] { ',' });
                foreach (string id in ids)
                {
                    string sid = id.Trim();
                    int nid;
                    if (!int.TryParse(sid, out nid))
                    {
                        site.Assume.Fail("The value's format of disabled requirements property '{0}' is incorrect. For example, '1000, 1002'.", ConfigPropNames.ReqSwitch);
                    }
                    disabledRequirements.Add(nid);
                }

              disabledReqsTable.Add(protocolName.ToLower(), disabledRequirements);
            }
            return disabledReqsTable[protocolName.ToLower()].Contains(requirementId);
        }

        /// <summary>
        /// Get Help message from MethodHelp defined in an interface.
        /// </summary>
        /// <param name="interfaceFullName">Fullname of interfaces implemented by current class</param>
        /// <returns>Help message of a SUT Control function</returns>
        public static string GetHelpMessage(string interfaceFullName)
        {
            string helpMessage = "";
            try
            {
                // Get method name
                StackTrace trace = new StackTrace();
                string methodName = trace.GetFrame(1).GetMethod().Name;

                // Get corresponding method defined in interface
                Assembly assembly = Assembly.GetCallingAssembly();
                Type adapterInterface = assembly.GetType(interfaceFullName);
                MethodBase method = adapterInterface.GetMethod(methodName);

                // Get and Return help message
                object[] attrArray = method.GetCustomAttributes(typeof(MethodHelpAttribute), true);

                foreach (object attr in attrArray)
                {
                    MethodHelpAttribute helpAttr = (MethodHelpAttribute)attr;
                    helpMessage += helpAttr.HelpMessage;
                }
            }
            catch { }

            return helpMessage;
        }
        /// <summary>
        /// Get all IP addresses by one host name or one IP
        /// </summary>
        /// <param name="hostnameOrIP"></param>
        /// <returns>An IP address list specified by hostname or IP</returns>
        public static List<IPAddress> GetHostIPs(ITestSite site, string hostnameOrIP)
        {
            List<IPAddress> ipList = new List<IPAddress>();
            IPAddress address;

            if (IPAddress.TryParse(hostnameOrIP, out address))
            {
                ipList.Add(address);
            }
            else
            {
                try
                {
                    IPHostEntry host = Dns.GetHostEntry(hostnameOrIP);
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipList.Add(ip);
                        }
                    }
                }
                catch (Exception e)
                {
                    site.Assume.Fail(String.Format("GetHostIPs failed with exception: {0}.", e.Message));
                }
            }

            return ipList;
        }
    }
}
