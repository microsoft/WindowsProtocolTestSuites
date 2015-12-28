// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    public class AuthTestConfig : TestConfigBase
    {
        private const string AuthenticationGroupName = "Auth.Authentication";
        private const string AuthorizationGroupName = "Auth.Authorization";

        // The keytab file of the file server. Used in Kerberos Authentication cases.
        public string KeytabFile
        {
            get
            {
                return GetProperty(AuthenticationGroupName, "KeytabFile", false);
            }
        }

        // Password of the SMB2 service principal. Applicable when keytab file is not provided. Used in Kerberos Authentication cases.
        public string ServicePassword
        {
            get
            {
                return GetProperty(AuthenticationGroupName, "ServicePassword", false);
            }
        }

        // Password salt of the SMB2 service principal. Applicable when keytab file is not provided. Used in Kerberos Authentication cases.
        public string ServiceSaltString
        {
            get
            {
                return GetProperty(AuthenticationGroupName, "ServiceSaltString", false);
            }
        }

        // The share used to test CBAC
        public string CBACShare
        {
            get
            {
                return GetProperty(AuthorizationGroupName, "CBACShare");
            }
        }

        // The share used to test folder permission
        public string FolderPermissionTestShare
        {
            get
            {
                return GetProperty(AuthorizationGroupName, "FolderPermissionTestShare");
            }
        }

        // The share used to test share permission
        public string SharePermissionTestShare
        {
            get
            {
                return GetProperty(AuthorizationGroupName, "SharePermissionTestShare");
            }
        }

        // The share used to test file permission
        public string FilePermissionTestShare
        {
            get
            {
                return GetProperty(AuthorizationGroupName, "FilePermissionTestShare");
            }
        }

        public AuthTestConfig(ITestSite site)
            : base(site)
        {

        }
    }
}
