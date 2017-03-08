// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter
{
    /// <summary>
    /// IDCControlAdapter is an interface, which implements IAdapter interface.
    /// This is a server SUT verification adapter interface.
    /// </summary>
    public interface IDCControlAdapter : IAdapter
    {
        #region DC Status Check

        /// <summary>
        /// Verification method from the server side.
        /// Verify if the client has successfully joined domain or not 
        /// by checking the existence of the client computer name in DC's active directory.
        /// </summary>
        /// <param name="clientName">client computer name</param>
        /// <returns>If the client computer account exists in AD return true, else return false.</returns>
        bool IsJoinDomainSuccess(string clientName);

        /// <summary>
        /// Verification method from the server side.
        /// Verify if the client has successfully provisioned a user's account or not
        /// by checking the existence of the user account in DC's active directory and its attributes.
        /// </summary>
        /// <param name="domainName">full domain name</param>
        /// <param name="userName">Specifies the new provisioned account user name.</param>
        /// <param name="password">Specifies the new provisioned account password.</param>
        /// <returns>true: provision user account successful, false: provision user account unsuccessfully.</returns>
        bool IsProvisionUserAcctSuccess(string domainName, string userName, string password);

        /// <summary>
        /// Verification method from the server side
        /// Verify if the client has successfully changed the user account's password
        /// by authenticating the user by its new password
        /// </summary>
        /// <param name="domainName">full domain name</param>
        /// <param name="userName">Specifies the user name for account change password.</param>
        /// <param name="newPassword">new password</param>
        /// <returns>true: change password successful, false: change password unsuccessfully.</returns>
        bool IsChangeUserAcctPasswordSuccess(string domainName, string userName, string newPassword);

        /// <summary>
        /// Verification method from the server side
        /// Verify if the client has successfully deleted the user account
        /// by checking the existence of the user account in DC's active directory
        /// </summary>
        /// <param name="userName">Specifies the user name for the account to be deleted.</param>
        /// <returns>If deleted user account successfully, return TRUE; else, return FALSE.</returns>
        bool IsDeleteUserAcctSuccess(string userName);

        /// <summary>
        /// Verification method from the server side
        /// Verify if the client has successfully managed a group and its memberships
        /// by checking the existence of the group and list its group memberships
        /// </summary>
        /// <param name="groupName">new group name</param>
        /// <returns>Return True if manage group and its memberships successfully, otherwise return False.</returns>
        bool IsManageGroupsandTheirMembershipsSuccess(string groupName);

        /// <summary>
        /// Verification method from the server side
        /// Verify if the client has successfully deleted the group indicated
        /// by checking the existence of the group
        /// </summary>
        /// <param name="groupName">Specifies the group name that it to be deleted.</param>
        /// <returns>If deleted group successfully, return TRUE; else, return FALSE.</returns>
        bool IsDeleteGroupSuccess(string groupName);

        #endregion
    }
}
