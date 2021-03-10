// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class Adapter
    {
        public string Name { get; set; }

        // Used by managed adapter only, the type name of adapter implementation.
        public string AdapterType { get; set; }

        public AdapterKind Kind { get; set; }

        public string ScriptDirectory { get; set; }
    }
}
