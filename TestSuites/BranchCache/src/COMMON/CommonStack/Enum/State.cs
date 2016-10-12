// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    /// <summary>
    /// State for the http server transport as web service.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The web service is starting.
        /// </summary>
        Starting,

        /// <summary>
        /// The web service is started.
        /// </summary>
        Started,

        /// <summary>
        /// The web service is stopping.
        /// </summary>
        Stopping,

        /// <summary>
        /// The web service is stopped.
        /// </summary>
        Stopped
    }
}
