// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class Rule : List<Rule>
    {
        /// <summary>
        /// The select status of this rule.
        /// </summary>
        public RuleSelectStatus SelectStatus { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string[] Categories { get; set; }
    }
}
