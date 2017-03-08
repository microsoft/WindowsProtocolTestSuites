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
    /// IClientControlAdapter is an interface which is inherited from the IAdapter interface.
    /// This is a client SUT verification adapter interface.
    /// </summary>
    public interface IClientControlAdapter : IAdapter
    {
        #region Client Status Check

        /// <summary>
        /// Verification method from the client side.
        /// Check whether the join domain is succeeded or failed.
        /// </summary>
        /// <returns>If the client has successfully joined the domain return true, else return false.</returns>
        [MethodHelp("Please check if the Client Computer has successfully joined the domain. If succeeded, return TRUE, else, return FALSE.")]
        bool IsJoinDomainSuccess();

        /// <summary>
        /// Verification method from the client side.
        /// Check whether the unjoin domain is succeeded or failed.
        /// </summary>
        /// <returns>If the client has successfully unjoined the domain return true, else return false.</returns>
        [MethodHelp("Please check if the Client Computer has successfully unjoined the domain. If succeeded, return TRUE, else return FALSE.")]
        bool IsUnjoinDomainSuccess();

        #endregion

        #region Client Behavior Trigger

        /// <summary>
        /// Trigger the client to locate domain controller.
        /// </summary>
        /// <returns>Returns the name of the DC.</returns>
        [MethodHelp("Please locate the Primary Domain Controller, and return the PDC FullComputerName or the NetBIOSComputerName.")]
        string LocateDomainController();

        /// <summary>
        /// Trigger the client to Join Domain by creating an account using LDAP.
        /// </summary>
        /// <returns>If the client has successfully joined the domain using LDAP return true, else return false.</returns>
        [MethodHelp("Please trigger the Client Computer to Join Domain using LDAP, make sure no computer account is created in AD beforehand. If succeeded, return TRUE, else return FALSE.")]
        bool JoinDomainCreateAcctLDAP();

        /// <summary>
        /// Trigger the client to Join Domain by creating an account using SAMR.
        /// </summary>
        /// <returns>If the client has successfully joined the domain using SAMR return true, else return false.</returns>
        [MethodHelp("Please trigger the Client Computer to Join Domain using SAMR, make sure no computer account is created in AD beforehand. If succeeded, return TRUE, else return FALSE.")]
        bool JoinDomainCreateAcctSAMR();

        /// <summary>
        /// Trigger the client to Join Domain by a predefined account.
        /// </summary>
        /// <returns>If the client has successfully joined the domain using the predefined account return true, else return false.</returns>
        [MethodHelp("Please trigger the Client Computer to Join Domain, make sure there is already a computer account created in AD beforehand.  If succeeded, return TRUE, else return FALSE.")]
        bool JoinDomainPredefAcct();

        /// <summary>
        /// Trigger the client to Unjoin Domain.
        /// </summary>
        /// <returns>If the client has successfully unjoined the domain return true, else return false.</returns>
        [MethodHelp("Please trigger the Client Computer to Unjoin Domain.  If succeeded, return TRUE, else return FALSE.")]
        bool UnjoinDomain();

        /// <summary>
        /// Trigger the client to provision a user account using LDAP.
        /// </summary>
        /// <returns>If the client has successfully provisioned a user account using LDAP return true, else return false.</returns>
        [MethodHelp("Please trigger the Client Computer to provision a user account in AD using LDAP - create a user account and change its password according to the PTFCONFIG file.  If succeeded, return TRUE, else return FALSE.")]
        bool ProvisionUserAcctLDAP();

        /// <summary>
        /// Trigger the client to provision a user account using SAMR.
        /// </summary>
        /// <returns>If the client has successfully provisioned a user account using SAMR return true, else return false.</.</returns>
        [MethodHelp("Please trigger the Client Computer to provision a user account in AD using SAMR - create a user account and change its password according to the PTFCONFIG file.  If succeeded, return TRUE, else return FALSE.")]
        bool ProvisionUserAcctSAMR();

        /// <summary>
        /// Trigger the client to change a user account's password.
        /// </summary>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        [MethodHelp("Please trigger the Client Computer to change the user account's password according to the PTF configuration file using LDAP.  If succeeded, return TRUE, else return FALSE.")]
        bool ChangeUserAcctPasswordLDAP();

        /// <summary>
        /// Trigger the client to change a user account's password.
        /// </summary>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        [MethodHelp("Please trigger the Client Computer to change the user account's password according to the PTF configuration file using SAMR.  If succeeded, return TRUE, else return FALSE.")]
        bool ChangeUserAcctPasswordSAMR();

        /// <summary>
        /// Trigger the client to determine a user account's groupmembership and list all the groups it has joined using LDAP.
        /// </summary>
        /// <returns>If succeeded, return the GROUP DN LIST, else return NULL.</returns>
        [MethodHelp("Please trigger the Client Computer to Determine the new user account's group membership using LDAP.  If succeeded, return the GROUP DN LIST, else return NULL.")]
        string DetermineUserAcctMembershipLDAP();

        /// <summary>
        /// Trigger the client to determine a user account's groupmembership and list all the groups it has joined using SAMR.
        /// </summary>
        /// <returns>If succeeded, return the GROUP DN LIST, else return NULL.</returns>
        [MethodHelp("Please trigger the Client Computer to determine the new user account's group membership using SAMR.  If succeeded, return the GROUP DN LIST, else return NULL.")]
        string DetermineUserAcctMembershipSAMR();
        
        /// <summary>
        /// Trigger the client to delete a user account.
        /// </summary>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        [MethodHelp("Please trigger the Client Computer to delete the new user account.  If succeeded, return TRUE, else return FALSE.")]
        bool DeleteUserAcct();

        /// <summary>
        /// Trigger the client to obtain a user account list using LDAP.
        /// </summary>
        /// <returns>If succeeded, return the USER DN LIST, else return NULL.</returns>
        [MethodHelp("Please trigger the Client Computer to obtain the user account list in AD.  If succeeded, return the USER DN LIST, else return NULL.")]
        string ObtainUserAcctListLDAP();

        /// <summary>
        /// Trigger the client to manage a group and its group memberships.
        /// </summary>
        /// <returns>If succeeded, return GROUP DN LIST, else return NULL.</returns>
        [MethodHelp("Please trigger the Client Computer to add a new group to AD and add a new user to the group.  If succeeded, return GROUP DN LIST, else return NULL.")]
        string ManageGroupsandTheirMemberships();

        /// <summary>
        /// Trigger the client to delete a group.
        /// </summary>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        [MethodHelp("Please trigger the Client Computer to delete the new group in AD.  If succeeded, return TRUE, else return FALSE.")]
        bool DeleteGroup();

        #endregion
    }
}
