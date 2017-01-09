// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// base class for updates using DRSR messages
    /// </summary>
    public abstract class DrsrBaseUpdate
    {
        /// <summary>
        /// unique test client
        /// </summary>
        protected static IDRSRTestClient client;

        /// <summary>
        /// set test client to be used by child classes
        /// </summary>
        /// <param name="testClient">drsr test client</param>
        public static void SetDrsTestClient(IDRSRTestClient testClient)
        {
            client = testClient;
        }
    }
}
