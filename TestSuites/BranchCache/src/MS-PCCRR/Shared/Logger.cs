// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using System;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// This class implements all methods of ILogPrinter.
    /// </summary>
    public class Logger : ILogPrinter
    {
        /// <summary>
        /// An instance of ITestSite.
        /// </summary>
        private ITestSite site;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class
        /// The constructor of Logger.
        /// </summary>
        /// <param name="site">An instance of ITestSite.</param>
        public Logger(ITestSite site)
        {
            if (null == site)
            {
                throw new ArgumentNullException("site", "The input parameter (site) is null.");
            }

            this.site = site;
        }

        /// <summary>
        /// The method implements adding the log of warning information.
        /// </summary>
        /// <param name="message">Message reprents the log information.</param>
        public void AddWarning(string message)
        {
            this.site.Log.Add(LogEntryKind.Warning, message);
        }

        /// <summary>
        /// The method implements adding the log information.
        /// </summary>
        /// <param name="message">Message reprents the log information.</param>
        public void AddInfo(string message)
        {
            this.site.Log.Add(LogEntryKind.Comment, message);
        }

        /// <summary>
        /// The method implements adding the log of debug information.
        /// </summary>
        /// <param name="message">The log information for debugging</param>
        public void AddDebug(string message)
        {
            this.site.Log.Add(LogEntryKind.Debug, message);
        }
    }
}
