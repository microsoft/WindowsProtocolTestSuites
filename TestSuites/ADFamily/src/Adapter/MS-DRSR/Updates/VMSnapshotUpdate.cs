// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public class VMSnapshotUpdate : IUpdate
    {
        public bool Invoke()
        {
         
            return true;
        }

        public bool Revert()
        {

            return true;
        }
    }
}
