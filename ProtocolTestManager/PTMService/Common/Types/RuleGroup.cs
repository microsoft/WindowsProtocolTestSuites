// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class RuleGroup
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public IList<Rule> Rules { get; set; }
    }
}
