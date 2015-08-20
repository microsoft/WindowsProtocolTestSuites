// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// IHasInterrelatedFields extends stackpacket, add an interface to update relatied field in stackpacket
    /// </summary>
    public interface IHasInterrelatedFields
    {
        /// <summary>
        /// Update Inter related field because some field of packet changed
        /// </summary>
        void UpdateInterrelatedFields();
    }
}
