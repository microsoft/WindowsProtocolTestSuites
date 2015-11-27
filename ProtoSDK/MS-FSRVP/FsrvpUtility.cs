// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp
{
    /// <summary>
    /// Fsrvp utility class.
    /// </summary>
    public static class FsrvpUtility
    {
        #region Fileds
        /// <summary>
        /// FSRVP interface UUID.
        /// </summary>
        public static readonly Guid FSRVP_INTERFACE_UUID = new Guid("A8E0653C-2744-4389-A61D-7373DF8B2292");

        /// <summary>
        /// FSRVP named pipe.
        /// </summary>
        public static readonly string FSRVP_NAMED_PIPE = @"\pipe\FssagentRpc";

        /// <summary>
        /// FSRVP interface major version.
        /// </summary>
        public const ushort FSRVP_INTERFACE_MAJOR_VERSION = 1;

        /// <summary>
        /// FSRVP interface minor version.
        /// </summary>
        public const ushort FSRVP_INTERFACE_MINOR_VERSION = 0;

        // The FSRVP client uses non-default behavior for the RPC Call Timeout timer defined in [MS-RPCE] section 3.3.2.2.2. 
        // The timer value that the client uses is 180,000 milliseconds; this value applies to all the method calls.
        public const uint FSRVPTimeoutInSeconds = 180;
        // The time in seconds that the client waits for the completion of the shadow copy preparation operation on the server.
        // <10> Section 3.2.3:  Windows clients set this timeout to 1,800,000 milliseconds.
        public const uint FSRVPPrepareTimeoutInSeconds = 1800;
        // The time in seconds that the client waits for the completion of the shadow copy commit operation on the server.
        // <11> Section 3.2.3:  Windows clients set this timeout to 60,000 milliseconds.
        public const uint FSRVPCommitTimeoutInSeconds = 60;
        // The time in seconds that the client waits for the completion of the shadow copy expose operation on the server.
        // <12> Section 3.2.3:  Windows clients set this timeout to 1,800,000 milliseconds.
        public const uint FSRVPExposeTimeoutInSeconds = 1800;

        #endregion
    }
}
