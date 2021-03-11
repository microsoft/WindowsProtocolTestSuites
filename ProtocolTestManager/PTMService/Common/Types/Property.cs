// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class Property
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public IEnumerable<string> Choices { get; set; }

        public string Description { get; set; }
    }
}
