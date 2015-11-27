// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Srvs
{
    public static class SrvsUtility
    {
        public static readonly Guid SRVS_INTERFACE_UUID = new Guid("4b324fc8-1670-01d3-1278-5a47bf6ee188");

        public const string SRVS_NAMED_PIPE = @"\PIPE\srvsvc";

        public const ushort SRVS_INTERFACE_MAJOR_VERSION = 3;

        public const ushort SRVS_INTERFACE_MINOR_VERSION = 0;

        public const Int32 MAX_PREFERRED_LENGTH = -1;
    }

}
