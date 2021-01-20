using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// Represent a local or domain user
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user _SID.
        /// </summary>
        public _SID Sid { get; set; }
    }

    /// <summary>
    /// Represent a local or domain group
    /// </summary>
    public class Group
    {
        /// <summary>
        /// The group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The group _SID.
        /// </summary>
        public _SID Sid { get; set; }
    }

    /// <summary>
    /// _WindowsIdentity class contains the necessary information to form an SMB2 TREE_CONNECT Request Extension.
    /// </summary>
    public class _WindowsIdentity
    {
        /// <summary>
        /// The down-level logon name of the domain user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user _SID.
        /// </summary>
        public _SID User { get; set; }

        /// <summary>
        /// An _SID collection of groups.
        /// The user is a member of the groups.
        /// </summary>
        public List<_SID> Groups { get; set; }

        /// <summary>
        /// The owner _SID.
        /// </summary>
        public _SID Owner { get; set; }
    }
}
