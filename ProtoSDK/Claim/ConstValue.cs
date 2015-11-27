// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{
    /// <summary>
    /// A class defining const names.
    /// </summary>
    internal static class ConstValue
    {
        /// <summary>
        /// The OSVersion of WinSvr2012R2
        /// </summary>
        internal const int WinSvr2012R2 = 6;

        /// <summary>
        /// The standard name of AuthenticationSilo Claim Name
        /// </summary>
        internal const string authSiloClaimName = "ad://ext/AuthenticationSilo";

        /// <summary>
        /// The standard name of distinguishedname
        /// </summary>
        internal const string distinguishedname = "distinguishedname";

        /// <summary>
        /// The standard name of name
        /// </summary>
        internal const string name = "name";

        /// <summary>
        /// The standard name of msDS-ClaimValueType
        /// </summary>
        internal const string msDSClaimValueType = "msDS-ClaimValueType";

        /// <summary>
        /// The standard name of msDS-ClaimSourceType
        /// </summary>
        internal const string msDSClaimSourceType = "msDS-ClaimSourceType";

        /// <summary>
        /// The standard name of msDS-ClaimAttributeSource
        /// </summary>
        internal const string msDSClaimAttributeSource = "msDS-ClaimAttributeSource";

        /// <summary>
        /// The standard name of msDs-ClaimSource
        /// </summary>
        internal const string msDsClaimSource = "msDs-ClaimSource";

        /// <summary>
        /// The standard name of msDS-ClaimTypeAppliesToClass
        /// </summary>
        internal const string msDSClaimTypeAppliesToClass = "msDS-ClaimTypeAppliesToClass";

        /// <summary>
        /// The standard name of msDS-AssignedAuthNPolicySilo
        /// </summary>
        internal const string msDSAssignedAuthNPolicySilo = "msDS-AssignedAuthNPolicySilo";

        /// <summary>
        /// The standard name of msDS-AuthNPolicySiloEnforced
        /// </summary>
        internal const string msDSAuthNPolicySiloEnforced = "msDS-AuthNPolicySiloEnforced";

        /// <summary>
        /// The standard name of msDS-AuthNPolicySiloMembers
        /// </summary>
        internal const string msDSAuthNPolicySiloMembers = "msDS-AuthNPolicySiloMembers";

        /// <summary>
        /// The standard name of msDS-Behavior-Version
        /// </summary>
        internal const string msDSBehaviorVersion = "msDS-Behavior-Version";

        /// <summary>
        /// The ldap path of Claim Types
        /// </summary>
        internal const string claimTypesPath = "LDAP://CN=Claim Types, CN=Claims Configuration, CN=Services,CN=Configuration,";

        /// <summary>
        /// The RDN of schema user
        /// </summary>
        internal const string userRDN = "cn=user,cn=schema,cn=configuration,";

        /// <summary>
        /// The RDN of schema computer
        /// </summary>
        internal const string computerRDN = "cn=computer,cn=schema,cn=configuration,";
    }

}
