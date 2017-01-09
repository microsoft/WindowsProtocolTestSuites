// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Store user information per account
    /// </summary>
    public class DsUser
    {
        /// <summary>
        /// domain name
        /// </summary>
        DsDomain domain;

        /// <summary>
        /// user name
        /// </summary>
        string user;

        /// <summary>
        /// password
        /// </summary>
        string pwd;


        /// <summary>
        /// user name
        /// </summary>
        public string Username
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }

        /// <summary>
        /// valid password or invalid one for invalid account
        /// </summary>
        public string Password
        {
            get
            {
                return pwd;
            }
            set
            {
                pwd = value;
            }
        }

        /// <summary>
        /// domain where this user is binded
        /// </summary>
        public DsDomain Domain
        {
            get
            {
                return domain;
            }
            set
            {
                domain = value;
            }
        }

        /// <summary>
        /// return credential for this user
        /// </summary>
        /// <returns>an AccountCredential object</returns>
        public AccountCredential GetAccountCredential()
        {
            return new AccountCredential(domain.DNSName, user, pwd);
        }
    }
}
