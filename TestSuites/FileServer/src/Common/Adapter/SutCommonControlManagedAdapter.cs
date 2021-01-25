// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// This class is to query common info from domain or local computer.
    /// </summary>
    public class SutCommonControlManagedAdapter : ManagedAdapterBase, ISutCommonControlManagedAdapter
    {
        private ISutCommonControlAdapter sutCommonControlAdapter;

        private JsonSerializerOptions serializerOptions;

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            sutCommonControlAdapter = Site.GetAdapter<ISutCommonControlAdapter>();

            serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new _SIDConverter());
        }

        /// <summary>
        /// Get group members from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>All group members returned.</returns>
        public List<GroupMember> GetGroupMembers(string target, string adminUserName, string groupName)
        {
            var membersStr = sutCommonControlAdapter.GetGroupMembers(target, adminUserName, groupName);

            var members = JsonSerializer.Deserialize<List<GroupMember>>(membersStr, serializerOptions);

            return members;
        }

        /// <summary>
        /// Get groups from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <returns>All groups returned.</returns>
        public List<Group> GetGroups(string target, string adminUserName)
        {
            var groupsStr = sutCommonControlAdapter.GetGroups(target, adminUserName);

            var groups = JsonSerializer.Deserialize<List<Group>>(groupsStr, serializerOptions);

            return groups;
        }

        /// <summary>
        /// Get users from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <returns>All users returned.</returns>
        public List<User> GetUsers(string target, string adminUserName)
        {
            var usersStr = sutCommonControlAdapter.GetGroups(target, adminUserName);

            var users = JsonSerializer.Deserialize<List<User>>(usersStr, serializerOptions);

            return users;
        }

        /// <summary> 
        /// Get the SID of a user from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>SID of the user returned.</returns>
        public _SID GetUserSid(string target, string adminUserName, string userName)
        {
            var sidStr = sutCommonControlAdapter.GetUserSid(target, adminUserName, userName);

            return new _SID(sidStr);
        }

        /// <summary> 
        /// Get the Memberships of a user from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local computer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>Memberships of the user.</returns>
        public List<Group> GetUserMemberships(string target, string adminUserName, string userName)
        {
            var membershipsStr = sutCommonControlAdapter.GetUserMemberships(target, adminUserName, userName);

            return JsonSerializer.Deserialize<List<Group>>(membershipsStr, serializerOptions);
        }

        /// <summary>
        /// Get a _WindowsIdentity instance from domain or local computer by the user name.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>A _WindowsIdentity instance represents the user.</returns>
        public _WindowsIdentity GetWindowsIdentity(string target, string adminUserName, string userName)
        {
            var identity = new _WindowsIdentity();

            // Get identity name.
            var targetNetbios = target.Split('.')[0];
            identity.Name = $"{targetNetbios}\\{userName}";

            // Get User _SID and Owner _SID.
            var userSid = GetUserSid(target, adminUserName, userName);
            identity.User = userSid;
            identity.Owner = userSid;

            // Get _SIDs of Groups.
            var userMemberships = GetUserMemberships(target, adminUserName, userName);
            identity.Groups = userMemberships.Select(g => g.Sid).ToList();

            return identity;
        }
    }
}
