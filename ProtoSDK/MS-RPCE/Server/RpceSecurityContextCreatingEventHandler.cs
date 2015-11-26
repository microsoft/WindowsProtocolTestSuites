// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// A callback routine to create ServerSecurityContext.
    /// </summary>
    /// <param name="sessionContext">Context of the RPCE session.</param>
    /// <returns>Created SecurityContext.</returns>
    public delegate ServerSecurityContext RpceSecurityContextCreatingEventHandler(RpceServerSessionContext sessionContext);
}
