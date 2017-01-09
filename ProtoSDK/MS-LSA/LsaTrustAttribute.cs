// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// The bit mapped values that define the attributes of the trust.
    /// </summary>
    [Flags]
    public enum LsaTrustAttribute : int
    {
        /// <summary>
        /// TANT (TRUST_ATTRIBUTE_NON_TRANSITIVE) 0x00000001
        /// Trust Attributes: Non-transitive
        /// </summary>
        TRUST_ATTRIBUTE_NON_TRANSITIVE = 0x00000001,

        /// <summary>
        /// TAUO (TRUST_ATTRIBUTE_UPLEVEL_ONLY) 0x00000002
        /// Trust Attributes: Up level only
        /// </summary>
        TRUST_ATTRIBUTE_UPLEVEL_ONLY = 0x00000002,

        /// <summary>
        /// TAQD (TRUST_ATTRIBUTE_QUARANTINED_DOMAIN) 0x00000004
        /// Trust Attributes: Quarantined
        /// </summary>
        TRUST_ATTRIBUTE_QUARANTINED_DOMAIN = 0x00000004,

        /// <summary>
        /// TAFT (TRUST_ATTRIBUTE_FOREST_TRANSITIVE) 0x00000008
        /// Trust Attributes: Forest trust
        /// </summary>
        TRUST_ATTRIBUTE_FOREST_TRANSITIVE = 0x00000008,

        /// <summary>
        /// TACO (TRUST_ATTRIBUTE_CROSS_ORGANIZATION) 0x00000010
        /// Trust Attributes: Cross organization
        /// </summary>
        TRUST_ATTRIBUTE_CROSS_ORGANIZATION = 0x00000010,

        /// <summary>
        /// TAWF (TRUST_ATTRIBUTE_WITHIN_FOREST) 0x00000020
        /// Trust Attributes: Within forest
        /// </summary>
        TRUST_ATTRIBUTE_WITHIN_FOREST = 0x00000020,

        /// <summary>
        /// TATE (TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL) 0x00000040
        /// Trust Attributes: Treat as external
        /// </summary>
        TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL = 0x00000040,

        /// <summary>
        /// TARC (TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION) 0x00000080
        /// Trust Attributes: Use RC4 Encryption
        /// </summary>
        TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION = 0x00000080,

        /// <summary>
        /// Obsolete. SHOULD be set to 0.
        /// </summary>
        Obsolete8 = 0x00800000,

        /// <summary>
        /// Obsolete. SHOULD be set to 0.
        /// </summary>
        Obsolete9 = 0x00400000,
    }
}
