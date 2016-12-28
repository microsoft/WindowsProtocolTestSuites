// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the DomainUpdatesContainer.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        /// <summary>
        /// A structure which is used to store domain revision
        /// </summary>
        public struct DomainRevision
        {
            //Domain minor revision
            public int minor;
            //Domain major revision
            public int major;
        }

        #region Validation of DomainUpdatesContainer.
        /// <summary>
        /// This method validates the requirements under 
        /// DomainUpdatesContainer Scenario.
        /// </summary>
        public void ValidateDomainUpdatesContainer()
        {
            DirectoryEntry domainUpdates = new DirectoryEntry();
            DomainRevision domain = new DomainRevision();
            bool isStoredOnActive = false;
            bool isStoredOnWin2003Up = false;

            //Get domain major revision
            adAdapter.GetObjectByDN("CN=ActiveDirectoryUpdate,CN=DomainUpdates,CN=System," + adAdapter.rootDomainDN, out domainUpdates);
            if (domainUpdates != null)
            {
                if (domainUpdates.Properties["Revision"].Value != null)
                {
                    domain.major = (int)domainUpdates.Properties["Revision"].Value;
                    isStoredOnActive = true;
                }

                DataSchemaSite.Assert.IsTrue(isStoredOnActive, @"The major version is stored on the revision attribute of the ActiveDirectoryUpdate container.");
            }

            //Get doamin minor revision
            adAdapter.GetObjectByDN("CN=Windows2003Update,CN=DomainUpdates,CN=System," + adAdapter.rootDomainDN, out domainUpdates);
            if (domainUpdates != null)
            {
                if (domainUpdates.Properties["Revision"].Value != null)
                {
                    domain.minor = (int)domainUpdates.Properties["Revision"].Value;
                    isStoredOnWin2003Up = true;
                }

                DataSchemaSite.Assert.IsTrue(isStoredOnWin2003Up, @"The minor version is stored on the revision attribute of the Windows2003Update container.");
            }

            DomainRevision DsBehavior = new DomainRevision();
            bool isDomainForCorrectDsBehavior = false;

            // The domain revision for WinSvr2008 is 3.8
            if (serverOS == OSVersion.WinSvr2008)
            {
                DsBehavior.major = 3;
                DsBehavior.minor = 8;
            }
            // The domain revision for WinSvr2008R2 is 5.8
            else if (serverOS == OSVersion.WinSvr2008R2)
            {
                DsBehavior.major = 5;
                DsBehavior.minor = 8;
            }
            // The domain revision for Win2012 is 8.8
            else if (serverOS == OSVersion.WinSvr2012)
            {
                DsBehavior.major = 8;
                DsBehavior.minor = 8;
            }
            // The domain revision for Win2012R2 is 10.9
            else if (serverOS == OSVersion.WinSvr2012R2)
            {
                DsBehavior.major = 10;
                DsBehavior.minor = 9;
            }

            // Check domain revision
            if ((domain.major > DsBehavior.major) || (domain.major == DsBehavior.major && domain.minor >= DsBehavior.minor))
            {
                isDomainForCorrectDsBehavior = true;
            }

            DataSchemaSite.Assert.IsTrue(isDomainForCorrectDsBehavior, @"The version of the domain revision is higher than or equal to 
             the minimum version of domain revision that is required for that DC functional level");
        }

        #endregion
    }
}