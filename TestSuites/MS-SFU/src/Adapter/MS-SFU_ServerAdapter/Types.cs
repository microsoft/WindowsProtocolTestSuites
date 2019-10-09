// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestSuites.SFUProtocol.Adapter
{
    /// <summary>
    /// The delegate type of response checker.
    /// </summary>
    /// <param name="pdu">The KerberosPdu received.</param>
    public delegate void ResponseChecker(KerberosPdu pdu);

    /// <summary>
    /// Service information.
    /// </summary>
    public class ServiceInfo
    {
        /// <summary>
        /// Realm name.
        /// </summary>
        public string Realm;

        /// <summary>
        /// Service principal name.
        /// </summary>
        public string ServicePrincipalName;

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName;

        /// <summary>
        /// User type.
        /// </summary>
        public KerberosAccountType UserType;

        /// <summary>
        /// Password.
        /// </summary>
        public string Password;

        /// <summary>
        /// Salt.
        /// </summary>
        public string Salt;

        /// <summary>
        /// TA.
        /// </summary>
        public bool TrustedToAuthenticationForDelegation;

        /// <summary>
        /// msDS-AllowedToDelegateTo.
        /// </summary>
        public string[] ServicesAllowedToSendForwardedTicketsTo;

        /// <summary>
        /// msDS-AllowedToActOnBehalfOfOtherIdentity.
        /// </summary>
        public string[] ServicesAllowedToReceiveForwardedTicketsFrom;
    }

    /// <summary>
    /// User information.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string UserName;

        /// <summary>
        /// User type.
        /// </summary>
        public PrincipalType UserType;

        /// <summary>
        /// ND.
        /// </summary>
        public bool DelegationNotAllowed;
    }
}
