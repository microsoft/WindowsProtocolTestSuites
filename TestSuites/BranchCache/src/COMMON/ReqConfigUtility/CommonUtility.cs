// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocol.TestSuites
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// Common helper class
    /// </summary>
    public static class CommonUtility
    {
        /// <summary>
        /// A mapping between protocols and relevant disabled requirements
        /// </summary>
        private static Dictionary<string, List<int>> disabledReqsTable;

        /// <summary>
        /// This method is used to retrieve the value from the PTFConfig
        /// </summary>
        /// <param name="site">The related ITestSite instance.</param>
        /// <param name="propName">The need property name</param>
        /// <returns>Return the need property if success</returns>
        public static string GetProperty(ITestSite site, string propName)
        {
            const string ErrorMsg = "The property {0} cannot be found in PTFConfg. "
                                + "Please verify it was set properly.";
            string propVal = site.Properties[propName];
            if (null == propVal)
            {
                site.Assume.Fail(ErrorMsg, propName);
            }

            return propVal;
        }

        /// <summary>
        /// This method checks if a requirement verification needs to be disabled.
        /// </summary>
        /// <param name="site">The related ITestSite instance.</param>
        /// <param name="protocolName">The protocol short name</param>
        /// <param name="requirementId">The requirement ID to be checked.</param>
        /// <returns>True indicates the requirement is disabled.</returns>
        public static bool IsRequirementVerificationDisabled(ITestSite site, string protocolName, int requirementId)
        {
            if (disabledReqsTable == null)
            {
                disabledReqsTable = new Dictionary<string, List<int>>();
            }

            if (!disabledReqsTable.ContainsKey(protocolName.ToLower()))
            {
                List<int> disabledRequirements = new List<int>();

                // Check if there is a ReqSwitch in the config.
                string strReqIds = site.Properties[ConfigPropNames.ReqSwitch + "." + protocolName];
                if (string.IsNullOrEmpty(strReqIds.Trim()))
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
                        site.Assume.Fail(
                            "The value's format of disabled requirements property '{0}' is incorrect. For example, '1000, 1002'",
                            ConfigPropNames.ReqSwitch);
                    }

                    disabledRequirements.Add(nid);
                }

              disabledReqsTable.Add(protocolName.ToLower(), disabledRequirements);
            }

            return disabledReqsTable[protocolName.ToLower()].Contains(requirementId);
        }
    }
}

