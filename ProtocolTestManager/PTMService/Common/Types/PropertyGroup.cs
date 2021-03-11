// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class PropertyGroup
    {
        public string Name { get; set; }

        public IEnumerable<Property> Items { get; set; }
    }
}
