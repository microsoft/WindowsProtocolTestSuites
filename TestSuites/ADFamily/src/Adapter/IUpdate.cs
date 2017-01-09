// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    /// <summary>
    /// Interface for updates that can be pushed into the storage
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// Update content in SUT
        /// </summary>
        /// <returns>true if succeed,otherwise false.</returns>
        bool Invoke();

        /// <summary>
        /// Revert content in SUT
        /// </summary>
        /// <returns>true if succeed,otherwise false.</returns>
        bool Revert();
    }
}
