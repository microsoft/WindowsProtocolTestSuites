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
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>All group members returned.</returns>
        public List<GroupMember> GetGroupMembers(string target, string adminUserName, string adminPassword, string groupName)
        {
            var membersStr = sutCommonControlAdapter.GetGroupMembers(target, adminUserName, adminPassword, groupName);

            var members = JsonSerializer.Deserialize<List<GroupMember>>(membersStr, serializerOptions);

            return members;
        }

        /// <summary>
        /// Get groups from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <returns>All groups returned.</returns>
        public List<Group> GetGroups(string target, string adminUserName, string adminPassword)
        {
            var groupsStr = sutCommonControlAdapter.GetGroups(target, adminUserName, adminPassword);

            var groups = JsonSerializer.Deserialize<List<Group>>(groupsStr, serializerOptions);

            return groups;
        }

        /// <summary>
        /// Get users from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <returns>All users returned.</returns>
        public List<User> GetUsers(string target, string adminUserName, string adminPassword)
        {
            var usersStr = sutCommonControlAdapter.GetGroups(target, adminUserName, adminPassword);

            var users = JsonSerializer.Deserialize<List<User>>(usersStr, serializerOptions);

            return users;
        }

        /// <summary> 
        /// Get the SID of a user from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>SID of the user returned.</returns>
        public _SID GetUserSid(string target, string adminUserName, string adminPassword, string userName)
        {
            var sidStr = sutCommonControlAdapter.GetUserSid(target, adminUserName, adminPassword, userName);

            return new _SID(sidStr);
        }

        /// <summary>
        /// Get a _WindowsIdentity instance from domain or local computer by the user name.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>A _WindowsIdentity instance represents the user.</returns>
        public _WindowsIdentity GetWindowsIdentity(string target, string adminUserName, string adminPassword, string userName)
        {
            var identity = new _WindowsIdentity();

            // Get identity name.
            var targetNetbios = target.Split('.')[0];
            identity.Name = $"{targetNetbios}\\{userName}";

            // Get User _SID and Owner _SID.
            var userSid = GetUserSid(target, adminUserName, adminPassword, userName);
            identity.User = userSid;
            identity.Owner = userSid;

            // Get _SIDs of Groups.
            var groups = GetGroups(target, adminUserName, adminPassword);
            var searchResults = new Dictionary<Group, List<GroupMember>>(new GroupEqualityComparer());
            foreach (var group in groups)
            {
                searchResults.Add(group, GetGroupMembers(target, adminUserName, adminPassword, group.Name));
            }

            var membershipSids = new List<_SID>();
            foreach (var group in groups)
            {
                // Validate direct membership.
                if (searchResults[group].Any(m => m.Sid.GetSddlForm() == userSid.GetSddlForm()))
                {
                    membershipSids.Add(group.Sid);
                    continue;
                }

                // Validate indirect membership.
                var lowerGroups = GetLowerGroups(searchResults[group]);
                if (lowerGroups.Any())
                {
                    var searchQueue = new Queue<Group>(lowerGroups);
                    while (searchQueue.Count > 0)
                    {
                        var lowerGroup = searchQueue.Dequeue();
                        if (searchResults[lowerGroup].Any(m => m.Sid.GetSddlForm() == userSid.GetSddlForm()))
                        {
                            membershipSids.Add(group.Sid);
                            break;
                        }

                        var nextLowerGroups = GetLowerGroups(searchResults[lowerGroup]);
                        if (nextLowerGroups.Any())
                        {
                            foreach (var nextLowerGroup in nextLowerGroups)
                            {
                                searchQueue.Enqueue(nextLowerGroup);
                            }
                        }
                    }
                }
            }
            identity.Groups = membershipSids;

            return identity;
        }

        private IEnumerable<Group> GetLowerGroups(IEnumerable<GroupMember> members)
        {
            return members.Where(m => m.ObjectClass.ToUpper() == "GROUP" && m.PrincipalSource.ToUpper() != "UNKNOWN").Select(m => m.ToGroup());
        }
    }
}
