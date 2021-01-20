// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// This class is to query commn info from domain or local computer.
    /// </summary>
    public class SutCommonControlManagedAdapter : ManagedAdapterBase, ISutCommonControlManagedAdapter
    {
        private ISutCommonControlAdapter sutCommonControlAdapter;

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            sutCommonControlAdapter = Site.GetAdapter<ISutCommonControlAdapter>();
        }

        /// <summary>
        /// Get group members from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>All group members returned.</returns>
        public List<User> GetGroupMembers(string target, string adminUserName, string adminPassord, string groupName)
        {
            var usersStr = sutCommonControlAdapter.GetGroupMembers(target, adminUserName, adminPassord, groupName);

            return null;
        }

        /// <summary>
        /// Get groups from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <returns>All groups returned.</returns>
        public List<Group> GetGroups(string target, string adminUserName, string adminPassord)
        {
            var groupsStr = sutCommonControlAdapter.GetGroups(target, adminUserName, adminPassord);

            return null;
        }

        /// <summary>
        /// Get users from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <returns>All users returned.</returns>
        public List<User> GetUsers(string target, string adminUserName, string adminPassord)
        {
            var usersStr = sutCommonControlAdapter.GetGroups(target, adminUserName, adminPassord);

            return null;
        }

        /// <summary> 
        /// Get the SID of a user from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>SID of the user returned.</returns>
        public _SID GetUserSid(string target, string adminUserName, string adminPassord, string userName)
        {
            var sidStr = sutCommonControlAdapter.GetUserSid(target, adminUserName, adminPassord, userName);

            return new _SID(sidStr);
        }

        /// <summary>
        /// Get a _WindowsIdentity instance from domain or local computer by the user name.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>A _WindowsIdentity instance represents the user.</returns>
        public _WindowsIdentity GetWindowsIdentity(string target, string adminUserName, string adminPassord, string userName)
        {
            var identity = new _WindowsIdentity();

            // Get identity name.
            var targetNetbios = target.Split('.')[0];
            identity.Name = $"{targetNetbios}\\{userName}";

            // Get User _SID and Owner _SID.
            var userSid = GetUserSid(target, adminUserName, adminPassord, userName);
            identity.User = userSid;
            identity.Owner = userSid;

            // Get _SIDs of Groups.
            var groups = GetGroups(target, adminUserName, adminPassord);
            var groupsJoined = groups.Where(g =>
            {
                var groupMembers = GetGroupMembers(target, adminUserName, adminUserName, g.Name);
                return groupMembers.Any(u => u.Sid.GetSddlForm() == userSid.GetSddlForm());
            });
            identity.Groups = groupsJoined.Select(g => g.Sid).ToList();

            return identity;
        }
    }
}
