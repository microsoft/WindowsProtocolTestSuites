// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.DFSC.TestSuite
{
    /// <summary>
    /// Indicates type of the root target
    /// </summary>
    public enum RootTargetType
    {
        /// <summary>
        /// Indicates the root target is a Netbios name
        /// </summary>
        NetBios,

        /// <summary>
        /// Indicates the root target is a fully qualified domain name 
        /// </summary>
        FQDN
    }

    public class DFSCTestConfig : TestConfigBase
    {
        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("DFSC", propertyName, checkNullOrEmpty);
        }

        public string ClientComputerName;

        public string DFSServerName
        {
            get
            {
                return this.SutComputerName;
            }
        }

        #region Root or link referral target
        public RootTargetType RootTargetType
        {
            get
            {
                return (RootTargetType)Enum.Parse(typeof(RootTargetType), GetProperty("RootTargetType"));
            }
        }

        public string LinkTarget
        {
            get
            {
                return string.Format(@"\{0}\{1}", this.StorageServerName, this.StorageServerShare);
            }
        }

        public string RootTargetDomain
        {
            get
            {
                if (RootTargetType == RootTargetType.FQDN
                    && !String.IsNullOrEmpty(DomainFQDNName))
                {
                    return string.Format(@"\{0}.{1}\{2}", this.DFSServerName, this.DomainFQDNName, this.DomainNamespace);
                }
                else
                {
                    return string.Format(@"\{0}\{1}", this.DFSServerName, this.DomainNamespace);
                }
            }
        }

        public string RootTargetStandalone
        {
            get
            {
                if (RootTargetType == RootTargetType.FQDN
                    && !String.IsNullOrEmpty(DomainFQDNName))
                {
                    return string.Format(@"\{0}.{1}\{2}", this.DFSServerName, this.DomainFQDNName, this.StandaloneNamespace);
                }
                else
                {
                    return string.Format(@"\{0}\{1}", this.DFSServerName, this.StandaloneNamespace);
                }
            }
        }

        public string InterlinkTarget
        {
            get
            {
                return string.Format(@"\{0}\{1}", this.DFSServerName, this.SMBDfsLink);
            }
        }
        #endregion

        #region Link path
        public string ValidLinkPathDomain
        {
            get
            {
                return string.Format(@"\{0}\{1}\{2}", this.DomainFQDNName, this.DomainNamespace, this.DFSLink);
            }
        }

        public string ValidLinkPathStandalone
        {
            get
            {
                return string.Format(@"\{0}\{1}\{2}", this.DFSServerName, this.StandaloneNamespace, this.DFSLink);
            }
        }

        public string ValidInterlinkPathDomain
        {
            get
            {
                return string.Format(@"\{0}\{1}\{2}", this.DomainFQDNName, this.DomainNamespace, this.Interlink);
            }
        }

        public string ValidInterlinkPathStandalone
        {
            get
            {
                return string.Format(@"\{0}\{1}\{2}", this.DFSServerName, this.StandaloneNamespace, this.Interlink);
            }
        }
        #endregion

        #region DFS root paths for testing
        public string ValidRootPathDomain
        {
            get
            {
                return string.Format("\\{0}\\{1}", this.DomainFQDNName, this.DomainNamespace);
            }
        }

        public string ValidRootPathStandalone
        {
            get
            {
                return string.Format("\\{0}\\{1}", this.DFSServerName, this.StandaloneNamespace);
            }
        }
        #endregion

        public string ValidNETBIOSPath
        {
            get
            {
                return string.Format("\\{0}", this.DomainNetBIOSName);
            }
        }

        public string ValidFQDNPath
        {
            get
            {
                return string.Format("\\{0}", this.DomainFQDNName);
            }
        }

        public string SMBDfsLink
        {
            get
            {
                return GetProperty("SMBDfsLink");
            }
        }

        //SiteName is a Unicode string specifying the name of the site to which the DFS client computer belongs
        //Use the default value "External". This field is set in a implementation specific function by client side.
        public string SiteName
        {
            get
            {
                return "External";
            }
        }

        public string DomainNetBIOSName
        {
            get
            {
                return GetProperty("DomainNetBIOSName");
            }
        }

        public string DomainFQDNName
        {
            get
            {
                return GetProperty("DomainFQDNName");
            }
        }

        public string DCServerName
        {
            get
            {
                return GetProperty("DCServerComputerName");
            }
        }

        public string DomainNamespace
        {
            get
            {
                return GetProperty("DomainNamespace");
            }
        }

        public string StandaloneNamespace
        {
            get
            {
                return GetProperty("StandaloneNamespace");
            }
        }

        public bool TransportPreferredSMB
        {
            get
            {
                return bool.Parse(GetProperty("TransportPreferredSMB"));
            }
        }

        public string StorageServerName
        {
            get
            {
                return GetProperty("StorageServerName");
            }
        }

        public string StorageServerShare
        {
            get
            {
                return GetProperty("StorageServerShare");
            }
        }

        public string DFSLink
        {
            get
            {
                return GetProperty("DFSLink");
            }
        }

        public string Interlink
        {
            get
            {
                return GetProperty("Interlink");
            }
        }

        public DFSCTestConfig(ITestSite site):base(site)
        {
            //Client Information
            ClientComputerName = System.Environment.MachineName;
        }
    }
}
