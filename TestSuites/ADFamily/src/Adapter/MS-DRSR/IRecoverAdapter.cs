// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public interface IRecoverAdapter: IAdapter
    {       

        /// <summary>
        /// Recover environment via operating VM host
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        [MethodHelp("Recover VM environment into initial")]
        void RecoverEnvironment();
    }
}
