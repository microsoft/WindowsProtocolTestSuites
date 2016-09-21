// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    /// <summary>
    /// Define all interface methods for logging.
    /// </summary>
    public interface ILogPrinter
    {
        /// <summary>
        /// Add warning log information.
        /// </summary>
        /// <param name="message">Message reprents the log information.</param>
        void AddWarning(string message);

        /// <summary>
        /// Add log information.
        /// </summary>
        /// <param name="message">Message reprents the log information.</param>
        void AddInfo(string message);

        /// <summary>
        /// Add debug log information.
        /// </summary>
        /// <param name="message">Message reprents the log information.</param>
        void AddDebug(string message);
    }
}
