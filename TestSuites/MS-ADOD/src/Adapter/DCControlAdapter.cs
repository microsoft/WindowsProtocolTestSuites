// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.ADOD.Adapter.Util;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter
{
    /// <summary>
    /// DCControlAdapter is a class which implements the IDCControlAdapter interface.
    /// This is a managed adapter providing methods for verifying state changes in windows environments.
    /// </summary>
    public class DCControlAdapter : ManagedAdapterBase, IDCControlAdapter
    {
        private ADODTestConfig config;
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            config = new ADODTestConfig(testSite);
        }

        /// <summary>
        /// Checks whether join domain is succeeded or failed.
        /// </summary>
        /// <param name="clientName">Specifies the name of the domain member to be verified.</param>
        /// <returns>If the client name exists in computer entry return true, else return false.</returns>
        public bool IsJoinDomainSuccess(string clientName)
        {
            return ActiveDirectoryHelper.IsComputerExist(clientName);
        }

        /// <summary>
        /// Check whether add user is succeeded or failed.
        /// </summary>
        /// <param name="userName">Specifies the name of the user to be verified.</param>
        /// <param name="password">Specifies the password of the user to be verified.</param>
        /// <returns>If the user exists and passes authentication return true, else return false.</returns>
        public bool IsProvisionUserAcctSuccess(string domainName, string userName, string password)
        {
            if (!ActiveDirectoryHelper.IsUserExist(userName))
                return false;

            return ActiveDirectoryHelper.AuthenticateUser(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", domainName, userName), password);
        }


        public bool IsChangeUserAcctPasswordSuccess(string domainName, string userName, string newPassword)
        {
            if (!ActiveDirectoryHelper.IsUserExist(userName))
                return false;

            return ActiveDirectoryHelper.AuthenticateUser(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", domainName, userName), newPassword);
        }


        public bool IsDeleteUserAcctSuccess(string userName)
        {
            return (!ActiveDirectoryHelper.IsUserExist(userName));
        }


        public bool IsManageGroupsandTheirMembershipsSuccess(string groupName)
        {
            return (ActiveDirectoryHelper.IsGroupExist(groupName));
        }

        public bool IsDeleteGroupSuccess(string groupName)
        {
            return (!ActiveDirectoryHelper.IsGroupExist(groupName));
        }
    }
}
