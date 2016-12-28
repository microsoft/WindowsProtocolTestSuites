// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// name type of AD objects
    /// </summary>
    public enum ObjectNameType
    {
        /// <summary>
        /// none
        /// </summary>
        NONE,

        /// <summary>
        /// SAM account name
        /// </summary>
        SAM,

        /// <summary>
        /// common name
        /// </summary>
        CN,

        /// <summary>
        /// distinguished name
        /// </summary>
        DN
    }
}
