// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// an enumeration to specify domains
    /// </summary>
    public enum DomainEnum
    {
        /// <summary>
        /// default
        /// </summary>
        None,

        /// <summary>
        /// invalid domain
        /// </summary>
        InvalidDomain,

        /// <summary>
        /// the primary test parent domain
        /// </summary>
        PrimaryDomain,

        /// <summary>
        /// child domain of the primary test domain
        /// </summary>
        ChildDomain,

        /// <summary>
        /// an external and trusted forest
        /// </summary>
        ExternalDomain,

        /// <summary>
        /// the domain for LDS instances
        /// </summary>
        LDSDomain
    }
}
