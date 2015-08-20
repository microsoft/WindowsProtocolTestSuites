// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// Stores static resources
    /// </summary>
    internal static class MessageRuntime
    {
        /// <summary>
        /// AssertCheckFail
        /// </summary>
        internal static string AssertCheckFail = "[CheckFailed] Assert check failed. {0}";

        /// <summary>
        /// AssertCheckSucceed
        /// </summary>
        internal static string AssertCheckSucceed = "[CheckSucceeded] Assert check succeeded. {0}";

        /// <summary>
        /// AssumeCheckFail
        /// </summary>
        internal static string AssumeCheckFail = "[CheckFailed] Assume check failed. {0}";

        /// <summary>
        /// AssumeCheckSucceed
        /// </summary>
        internal static string AssumeCheckSucceed = "[CheckSucceeded] Assume check succeeded. {0}";

        /// <summary>
        /// DebugCheckFail
        /// </summary>
        internal static string DebugCheckFail = "[CheckFailed] Debug check failed. {0}";

        /// <summary>
        /// DebugCheckSucceed
        /// </summary>
        internal static string DebugCheckSucceed = "[CheckSucceeded] Debug check succeeded. {0}";

        /// <summary>
        /// MessageLogFormat
        /// </summary>
        internal static string MessageLogFormat = "[{0}] {1}";

        /// <summary>
        /// ReqCaptureFormat
        /// </summary>
        internal static string ReqCaptureFormat = "[checkpoint] {0} : {1}";

        /// <summary>
        /// ReqId
        /// </summary>
        internal static string ReqId = "{0}_R{1}";
    }
}