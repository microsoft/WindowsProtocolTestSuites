// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public enum GroupTypeFlags : uint
    {
        BUILTIN_LOCAL_GROUP = 0x00000001,
        ACCOUNT_GROUP = 0x00000002,
        RESOURCE_GROUP = 0x00000004,
        UNIVERSAL_GROUP = 0x00000008,
        APP_BASIC_GROUP = 0x00000010,
        APP_QUERY_GROUP = 0x00000020,
        SECURITY_ENABLED = 0x80000000
    }
}
