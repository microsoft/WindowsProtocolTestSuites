// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// The naming context enumerations.
    /// </summary>
    public enum NamingContext
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Schema NC
        /// </summary>
        SchemaNC,

        /// <summary>
        /// Config NC
        /// </summary>
        ConfigNC,

        /// <summary>
        /// Domain NC, only apply to AD DS
        /// </summary>
        DomainNC,

        /// <summary>
        /// Application NC, only apply to AD LDS
        /// </summary>
        AppNC
    }
}
