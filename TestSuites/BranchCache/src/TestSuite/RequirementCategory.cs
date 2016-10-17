// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    public static class RequirementCategory
    {
        public const int InvalidParameter = 1;
        public const string InvalidParameterMessage = "Error code should be STATUS_INVALID_PARAMETER";

        public const int EndOfFile = 2;
        public const string EndOfFileMessage = "Error code should be STATUS_END_OF_FILE";

        public const int HashNotPresent = 3;
        public const string HashNotPresentMessage = "Error code should be STATUS_HASH_NOT_PRESENT";
    }
}
