using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    public interface ISutCommonControlManagedAdapter : IAdapter
    {
        /// <summary>
        /// Get users from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <returns>All users returned.</returns>
        List<User> GetUsers(string target, string adminUserName, string adminPassord);

        /// <summary>
        /// Get groups from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <returns>All groups returned.</returns>
        List<Group> GetGroups(string target, string adminUserName, string adminPassord);

        /// <summary>
        /// Get group members from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <param name="groupName">Name of the queried group.</param>
        /// <returns>All group members returned.</returns>
        List<User> GetGroupMembers(string target, string adminUserName, string adminPassord, string groupName);

        /// <summary> 
        /// Get the SID of a user from domain or local computer.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>SID of the user returned.</returns>
        _SID GetUserSid(string target, string adminUserName, string adminPassord, string userName);

        /// <summary>
        /// Get a _WindowsIdentity instance from domain or local computer by the user name.
        /// </summary>
        /// <param name="target">Name of the domain or local compuer.</param>
        /// <param name="adminUserName">Name of the user who has administrative privileges.</param>
        /// <param name="adminPassord">Password of the user who has administrative privileges.</param>
        /// <param name="userName">Name of the queried user.</param>
        /// <returns>A _WindowsIdentity instance that represents the user.</returns>
        _WindowsIdentity GetWindowsIdentity(string target, string adminUserName, string adminPassword, string userName);
    }
}
