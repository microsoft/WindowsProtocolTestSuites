// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.KerberosPlugin
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class DependencyAttribute : Attribute
    {
        public DependencyAttribute(string testCategory)
        {
            dependencies = testCategory;
        }
        private string dependencies;
        public string TestCategory
        {
            get
            {
                return dependencies;
            }
        }
    }

    public class ConfigItemDependency
    {
        [Dependency("SameLocalDifferentShareWithBasic")]
        [Dependency("DifferentLocalDifferentShareWithBasic")]
        public void AppInstanceId()
        {
        }

        [Dependency("Symboliclink")]
        [Dependency("SymboliclinkInSubFolder")]
        public void CreateClose()
        {
        }

        [Dependency("ClientRequestedCredit")]
        public void Credit()
        {
        }

        [Dependency("EncryptedFileShare")]
        [Dependency("GlobalEncryptDataEnabled")]
        [Dependency("IsGlobalRejectUnencryptedAccessEnabled")]
        [Dependency("IsEncryptionSupported")]
        public void Encryption()
        {
        }

        [Dependency("CAShareName")]
        [Dependency("CAShareServerIpAddress")]
        [Dependency("CAShareServerName")]
        public void DurableHandle()
        {
        }

        [Dependency("FileShareSupportingIntegrityInfo")]
        public void FsctlSetGetIntegrityInformation()
        {
        }

        [Dependency("IsUnderlyingObjectStoreSupported")]
        [Dependency("ShareForceLevel2Oplock")]
        [Dependency("ShareTypeInclude_STYPE_CLUSTER_SOFS")]
        public void Leasing()
        {
        }

        [Dependency("ShareWithoutForceLevel2OrSOFS")]
        [Dependency("ShareWithoutForceLevel2WithSOFS")]
        [Dependency("ShareWithForceLevel2WithoutSOFS")]
        [Dependency("ShareWithForceLevel2AndSOFS")]
        [Dependency("ScaleOutFileServerName")]
        public void Oplock()
        {
        }

        [Dependency("SutAlternativeIPAddress")]
        public void Session()
        {
        }

        [Dependency("IsRequireMessageSigning")]
        public void Signing()
        {
        }

        [Dependency("SpecialShare")]
        public void Tree()
        {
        }
    }
}
