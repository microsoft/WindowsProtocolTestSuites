// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{
    /// <summary>
    /// class type of principal
    /// </summary>
    public enum ClaimsPrincipalClass
    {
        /// <summary>
        /// For user
        /// </summary>
        User,

        /// <summary>
        /// For device
        /// </summary>
        Device
    }

    /// <summary>
    /// Declare which type of source will be checked for claims
    /// </summary>
    [Flags]
    public enum ClaimsSource
    {
        /// <summary>
        /// from AD
        /// </summary>
        AD,

        /// <summary>
        /// from certificate
        /// </summary>
        Certificate
    }

    /// <summary>
    /// Declare the value of msDS-ClaimSourceType
    /// </summary>
    public enum ClaimsSourceType
    {
        /// <summary>
        /// the value of msDS-ClaimSourceType is AD
        /// </summary>
        AD,

        /// <summary>
        /// the value of msDS-ClaimSourceType is Certificate
        /// </summary>
        Certificate,

        /// <summary>
        /// the value of msDS-ClaimSourceType is Transform Policy
        /// </summary>
        TransformPolicy,

        /// <summary>
        /// the value of msDS-ClaimSourceType is Constructed
        /// </summary>
        Constructed
    }
}
