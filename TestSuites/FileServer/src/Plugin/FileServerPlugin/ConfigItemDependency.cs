// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
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
        [Dependency("SameShareWithSMBBasic")]
        [Dependency("DifferentShareFromSMBBasic")]
        public void AppInstanceId()
        {
        }

        [Dependency("KeytabFile")]
        [Dependency("ServicePassword")]
        [Dependency("ServiceSaltString")]
        public void KerberosAuthentication()
        {
        }

        [Dependency("Symboliclink")]
        [Dependency("SymboliclinkInSubFolder")]
        [Dependency("PathSeparator")]
        public void CreateClose()
        {
        }

        [Dependency("EncryptedFileShare")]
        [Dependency("IsGlobalEncryptDataEnabled")]
        [Dependency("IsGlobalRejectUnencryptedAccessEnabled")]
        [Dependency("IsEncryptionSupported")]
        public void Encryption()
        {
        }

        [Dependency("CAShareName")]
        [Dependency("CAShareServerName")]
        public void PersistentHandle()
        {
        }

        [Dependency("FileShareSupportingIntegrityInfo")]
        public void FsctlSetGetIntegrityInformation()
        {
        }

        [Dependency("ShareTypeInclude_STYPE_CLUSTER_SOFS")]
        public void Replay()
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

        [Dependency("ScaleOutFileServerIP1")]
        [Dependency("ScaleOutFileServerIP2")]
        [Dependency("ScaleOutFileServerName")]
        [Dependency("CAShareName")]
        public void OperateOneFileFromTwoNodes()
        {
        }
    }
}
