// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Linq;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter
{
    public class SMB2ModelTestConfig : TestConfigBase
    {
        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("SMB2", propertyName, checkNullOrEmpty);
        }

        #region Info of Server got from Negotiate
        #endregion

        #region Oplock Model Config

        public string ShareWithoutForceLevel2OrSOFS
        {
            get
            {
                return GetProperty("ShareWithoutForceLevel2OrSOFS");
            }
        }

        public string ShareWithoutForceLevel2WithSOFS
        {
            get
            {
                return GetProperty("ShareWithoutForceLevel2WithSOFS");
            }
        }

        public string ShareWithForceLevel2WithoutSOFS
        {
            get
            {
                return GetProperty("ShareWithForceLevel2WithoutSOFS");
            }
        }

        public string ShareWithForceLevel2AndSOFS
        {
            get
            {
                return GetProperty("ShareWithForceLevel2AndSOFS");
            }
        }

        public string ScaleOutFileServerName
        {
            get
            {
                return GetProperty("ScaleOutFileServerName");
            }
        }

        #endregion

        #region TreeMgmt Model Config
        public string SpecialShare
        {
            get
            {
                return GetProperty("SpecialShare");
            }
        }
        #endregion

        #region AppInstanceId Model Config
        public string SameWithSMBBasic
        {
            get
            {
                return GetProperty("SameShareWithSMBBasic");
            }
        }
        public string DifferentFromSMBBasic
        {
            get
            {
                return GetProperty("DifferentShareFromSMBBasic");
            }
        }
        #endregion

        #region CreateClose Model Config

        public string PathSeparator
        {
            get
            {
                return GetProperty("PathSeparator");
            }
        }

        #endregion

        #region Conflict Model Config
        public IPAddress ScaleOutFileServerIP1
        {
            get
            {
                var result = GetProperty("ScaleOutFileServerIP1").ParseIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "ScaleOutFileServerIP1 should be a valid IP address or a resolvable host name!");
                return result;
            }
        }

        public IPAddress ScaleOutFileServerIP2
        {
            get
            {
                var result = GetProperty("ScaleOutFileServerIP2").ParseSecondaryIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "ScaleOutFileServerIP2 should be a valid IP address or a resolvable host name with at least two IP addresses!");
                return result;
            }
        }
        #endregion
        public SMB2ModelTestConfig(ITestSite site) : base(site)
        {
        }
    }
}
