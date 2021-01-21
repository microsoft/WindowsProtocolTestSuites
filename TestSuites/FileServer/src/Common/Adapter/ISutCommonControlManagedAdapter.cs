using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    public interface ISutCommonControlManagedAdapter : IAdapter
    {
        /// <summary>
        /// Get users from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local computer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <returns>All users returned.</returns>
        List<User> GetUsers(string target, string adminUserName, string adminPassword);

        /// <summary>
        /// Get groups from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local computer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <returns>All groups returned.</returns>
        List<Group> GetGroups(string target, string adminUserName, string adminPassword);

        /// <summary>
        /// Get group members from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local computer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>All group members returned.</returns>
        List<GroupMember> GetGroupMembers(string target, string adminUserName, string adminPassword, string groupName);

        /// <summary> 
        /// Get the SID of a user from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local computer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>SID of the user returned.</returns>
        _SID GetUserSid(string target, string adminUserName, string adminPassword, string userName);

        /// <summary>
        /// Get a _WindowsIdentity instance from domain or local computer by the user name.
        /// </summary>
        /// <param name="target">Name of the domain or local computer. Use FQDN for domain.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="adminPassword">Password of the user who has administrative privileges. This value can be omitted for local computer.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>A _WindowsIdentity instance that represents the user.</returns>
        _WindowsIdentity GetWindowsIdentity(string target, string adminUserName, string adminPassword, string userName);
    }
}
