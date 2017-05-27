// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Enumeration specifies the Group Type Flags
    /// </summary>
    [Flags]
    public enum GroupTypeFlags : uint
    {
        /// <summary>
        /// GROUP_TYPE_BUILTIN_LOCAL_GROUP: Specifices a group that is created by the system
        /// </summary>
        GROUP_TYPE_BUILTIN_LOCAL_GROUP = 0x00000001,

        /// <summary>
        /// GROUP_TYPE_ACCOUNT_GROUP: Specifices a global group
        /// </summary>
        GROUP_TYPE_ACCOUNT_GROUP = 0x00000002,

        /// <summary>
        /// GROUP_TYPE_RESOURCE_GROUP: Specifiies a domain local group
        /// </summary>
        GROUP_TYPE_RESOURCE_GROUP = 0x00000004,

        /// <summary>
        /// GROUP_TYPE_UNIVERSAL_GROUP: Specifies a universal group
        /// </summary>
        GROUP_TYPE_UNIVERSAL_GROUP = 0x00000008,

        /// <summary>
        /// GROUP_TYPE_APP_BASIC_GROUP: Groups of this type are not used by Active Directory.
        /// This constant is included in this document because the value of this constant is 
        /// used by Active Directory in processing the groupType attribute, as specified in [MS-ADA1] section 2.288.
        /// </summary>
        GROUP_TYPE_APP_BASIC_GROUP = 0x00000010,

        /// <summary>
        /// GROUP_TYPE_APP_QUERY_GROUP: Groups of this type are not used by Active Directory.
        /// This constant is included in this document because the value of this constant is 
        /// used by Active Directory in processing the groupType attribute.
        /// </summary>
        GROUP_TYPE_APP_QUERY_GROUP = 0x00000020,

        /// <summary>
        /// GROUP_TYPE_SECURITY_ENABLED: Specifies a security-enabled group.
        /// </summary>
        GROUP_TYPE_SECURITY_ENABLED = 0x80000000
    }

    public class GroupType
    {
        public static bool ValidateFlags(GroupTypeFlags groupTypeFlags)
        {
            GroupTypeFlags tmpFlags = groupTypeFlags;

            if (tmpFlags.HasFlag(GroupTypeFlags.GROUP_TYPE_BUILTIN_LOCAL_GROUP))
                tmpFlags = ~GroupTypeFlags.GROUP_TYPE_BUILTIN_LOCAL_GROUP & tmpFlags;

            if (tmpFlags.HasFlag(GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED))
                tmpFlags = ~GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED & tmpFlags;

            return tmpFlags == GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP
                || tmpFlags == GroupTypeFlags.GROUP_TYPE_APP_BASIC_GROUP
                || tmpFlags == GroupTypeFlags.GROUP_TYPE_APP_QUERY_GROUP
                || tmpFlags == GroupTypeFlags.GROUP_TYPE_RESOURCE_GROUP
                || tmpFlags == GroupTypeFlags.GROUP_TYPE_UNIVERSAL_GROUP;
        }
    }
}
