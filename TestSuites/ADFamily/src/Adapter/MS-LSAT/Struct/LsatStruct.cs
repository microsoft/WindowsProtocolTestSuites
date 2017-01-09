// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat
{
    /// <summary>
    /// The LSA_UNICODE_STRING structure specifies the unicode string of LSA names.
    /// </summary>
    public partial struct _LSA_UNICODE_STRING
    {
        /// <summary>
        /// The length of LSA unicode string.
        /// </summary>
        public ushort Length;

        /// <summary>
        /// The max length of LSA unicode string.
        /// </summary>
        public ushort MaximumLength;

        /// <summary>
        /// The value of LSA unicode string.
        /// </summary>
        public ushort[] Buffer;
    }
}
