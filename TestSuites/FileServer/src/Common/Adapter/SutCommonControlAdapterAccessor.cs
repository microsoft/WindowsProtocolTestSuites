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
    public class SutCommonControlAdapterAccessor
    {
        private ISutCommonControlAdapter sutCommonControlAdapter;

        private JsonSerializerOptions serializerOptions;

        private string domainName;

        private string sutComputerName;

        public SutCommonControlAdapterAccessor(ITestSite testSite)
        {
            sutCommonControlAdapter = testSite.GetAdapter<ISutCommonControlAdapter>();

            var testConfig = new TestConfigBase(testSite);
            domainName = testConfig.DomainName;
            sutComputerName = testConfig.SutComputerName;

            serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new _SIDConverter());
        }

        /// <summary>
        /// Get group members from domain or local computer.
        /// </summary>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>All group members returned.</returns>
        public List<GroupMember> GetGroupMembers(string groupName)
        {
            var membersStr = sutCommonControlAdapter.GetGroupMembers(groupName);

            var members = JsonSerializer.Deserialize<List<GroupMember>>(membersStr, serializerOptions);

            return members;
        }

        /// <summary>
        /// Get groups from domain or local computer.
        /// </summary>
        /// <returns>All groups returned.</returns>
        public List<Group> GetGroups()
        {
            var groupsStr = sutCommonControlAdapter.GetGroups();

            var groups = JsonSerializer.Deserialize<List<Group>>(groupsStr, serializerOptions);

            return groups;
        }

        /// <summary>
        /// Get users from domain or local computer.
        /// </summary>
        /// <returns>All users returned.</returns>
        public List<User> GetUsers()
        {
            var usersStr = sutCommonControlAdapter.GetUsers();

            var users = JsonSerializer.Deserialize<List<User>>(usersStr, serializerOptions);

            return users;
        }

        /// <summary> 
        /// Get the SID of a user from domain or local computer.
        /// </summary>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>SID of the user returned.</returns>
        public _SID GetUserSid(string userName)
        {
            var sidStr = sutCommonControlAdapter.GetUserSid(userName);

            return new _SID(sidStr);
        }

        /// <summary> 
        /// Get the SID of a group from domain or local computer.
        /// </summary>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>SID of the group returned.</returns>
        public _SID GetGroupSid(string groupName)
        {
            var groupSidStr = sutCommonControlAdapter.GetGroupSid(groupName);

            return new _SID(groupSidStr);
        }

        /// <summary> 
        /// Get the Memberships of a user from domain or local computer.
        /// </summary>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>Memberships of the user.</returns>
        public List<Group> GetUserMemberships(string userName)
        {
            var membershipsStr = sutCommonControlAdapter.GetUserMemberships(userName);

            return JsonSerializer.Deserialize<List<Group>>(membershipsStr, serializerOptions);
        }

        /// <summary>
        /// Get a _WindowsIdentity instance from domain or local computer by the user name.
        /// </summary>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>A _WindowsIdentity instance represents the user.</returns>
        public _WindowsIdentity GetWindowsIdentity(string userName)
        {
            var identity = new _WindowsIdentity();

            // Get identity name.
            var target = !string.IsNullOrEmpty(domainName) ? domainName : sutComputerName;
            var targetNetbios = target.Split('.')[0];
            identity.Name = $"{targetNetbios}\\{userName}";

            // Get User _SID and Owner _SID.
            var userSid = GetUserSid(userName);
            identity.User = userSid;
            identity.Owner = userSid;

            // Get _SIDs of Groups.
            var userMemberships = GetUserMemberships(userName);
            identity.Groups = userMemberships.Select(g => g.Sid).ToList();

            return identity;
        }
    }
}
