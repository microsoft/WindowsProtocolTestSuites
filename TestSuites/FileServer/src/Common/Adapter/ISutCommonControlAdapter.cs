// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    public interface ISutCommonControlAdapter : IAdapter
    {
        /// <summary>
        /// Get users from domain or local computer.
        /// </summary>
        /// <returns>JSON string of all users returned.</returns>
        [MethodHelp("Get users from domain or local computer.")]
        string GetUsers();

        /// <summary>
        /// Get groups from domain or local computer.
        /// </summary>
        /// <returns>JSON string of all groups returned.</returns>
        [MethodHelp("Get groups from domain or local computer.")]
        string GetGroups();

        /// <summary>
        /// Get group members from domain or local computer.
        /// </summary>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>JSON string of all group members returned.</returns>
        [MethodHelp("Get group members from domain or local computer.")]
        string GetGroupMembers(string groupName);

        /// <summary> 
        /// Get the SID of a user from domain or local computer.
        /// </summary>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>The SDDL form of the user SID returned.</returns>
        [MethodHelp("Get SID of a user from domain or local computer.")]
        string GetUserSid(string userName);

        /// <summary> 
        /// Get the SID of a group from domain or local computer.
        /// </summary>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>The SDDL form of the group SID returned.</returns>
        [MethodHelp("Get SID of a group from domain or local computer.")]
        string GetGroupSid(string groupName);

        /// <summary> 
        /// Get the memberships of a user from domain or local computer.
        /// </summary>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>JSON string of all user memberships returned.</returns>
        [MethodHelp("Get the memberships of a user from domain or local computer.")]
        string GetUserMemberships(string userName);
    }
}

