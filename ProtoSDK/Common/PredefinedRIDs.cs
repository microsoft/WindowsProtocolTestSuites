// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// These are predefined RIDs of users and groups.
    /// </summary>
    public static class PredefinedRIDs
    {
        /// <summary>
        /// Name: Administrator
        /// User for administering the computer or domain.
        /// </summary>
        public const int DOMAIN_USER_RID_ADMIN = 0x000001F4;
        
        /// <summary>
        /// Name: Guest
        /// User for guest access to the computer or domain.
        /// </summary>
        public const int DOMAIN_USER_RID_GUEST = 0x000001F5;
        
        /// <summary>
        /// Name: krbtgt
        /// User for Key Distribution Center Service.
        /// </summary>
        public const int DOMAIN_USER_RID_KRBTGT = 0x000001F6;

        /// <summary>
        /// Name: Domain Users
        /// A group that represents all domain users.
        /// </summary>
        public const int DOMAIN_GROUP_RID_USERS = 0x00000201;

        /// <summary>
        /// Name: Domain Computers
        /// A group that represents all workstations and servers joined to the domain.
        /// </summary>
        public const int DOMAIN_GROUP_RID_COMPUTERS = 0x00000203;

        /// <summary>
        /// Name: Domain Controllers
        /// A group that represents all DCs in the domain.
        /// </summary>
        public const int DOMAIN_GROUP_RID_CONTROLLERS = 0x00000204;

        /// <summary>
        /// Name: Administrators
        /// A group that has complete and unrestricted access to the computer or domain.
        /// </summary>
        public const int DOMAIN_ALIAS_RID_ADMINS = 0x00000220;

        /// <summary>
        /// Name: Read-only Domain Controllers
        /// A group that represents all RODCs in the domain.
        /// </summary>
        public const int DOMAIN_GROUP_RID_READONLY_CONTROLLERS = 0x00000209;

    }
}
