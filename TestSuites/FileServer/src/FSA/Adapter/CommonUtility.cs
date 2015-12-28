// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using System.Net;
using System.Net.Sockets;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// Common helper class
    /// </summary>
    public static class CommonUtility
    {
        private static List<int> disabledRequirements;

        /// <summary>
        /// This method checks if a requirement verification needs to be disabled.
        /// </summary>
        /// <param name="site">The related ITestSite instance.</param>
        /// <param name="requirementId">The requirement ID to be checked.</param>
        /// <returns>True indicates the requirement is disabled.</returns>
        public static bool IsRequirementVerificationDisabled(ITestSite site, int requirementId)
        {
            if (disabledRequirements == null)
            {
                // Parse disable requirement IDs.
                disabledRequirements = new List<int>();

                // Check if there is a ReqSwitch in the config.
                string strReqIds = site.Properties[ConfigPropNames.ReqSwitch];
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
                        site.Assume.Fail("The value's format of disabled requirements property '{0}' is incorrect. For example, '1000, 1002'", ConfigPropNames.ReqSwitch);
                    }
                    disabledRequirements.Add(nid);
                }
            }

            return disabledRequirements.Contains(requirementId);
        }
    }
}
