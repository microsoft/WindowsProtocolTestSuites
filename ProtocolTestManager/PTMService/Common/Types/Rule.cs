// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class Rule
    {
        public RuleType Type { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string[] Categories { get; set; }
    }
}
