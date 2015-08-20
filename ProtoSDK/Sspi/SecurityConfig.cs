// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Base class of all kinds of security configuration.
    /// </summary>
    public class SecurityConfig
    {
        /// <summary>
        /// Security package type.
        /// </summary>
        private SecurityPackageType securityType;

        /// <summary>
        /// Security package type.
        /// </summary>
        public SecurityPackageType SecurityType
        {
            get
            {
                return this.securityType;
            }
            set
            {
                this.securityType = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="securityPackageType">Security package type.</param>
        protected SecurityConfig(SecurityPackageType securityPackageType)
        {
            this.securityType = securityPackageType;
        }
    }
}
