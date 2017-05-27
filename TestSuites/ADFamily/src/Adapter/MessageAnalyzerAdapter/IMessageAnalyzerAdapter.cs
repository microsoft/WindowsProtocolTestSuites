// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.MessageAnalyzer;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.MAAdapter
{
    public interface IMessageAnalyzerAdapter : IAdapter
    {
        #region Functions

        /// <summary>
        /// Get Messages with filter applied to the capture
        /// </summary>
        /// <param name="capturePath">The capture file path</param>
        /// <param name="frameFilter">The filter applied to the capture</param>
        /// <returns>Return the filtered messages</returns>
        List<Message> GetMessages(string capturePath, string frameFilter);

        /// <summary>
        /// Start a live capture to capture messages
        /// </summary>
        /// <param name="capturePath">The capture file path</param>
        /// <param name="filter">The filter applied to the capture</param>
        void StartCapture(string capturePath, string filter = null);

        /// <summary>
        /// Stop the live capture
        /// </summary>
        void StopCapture();

        #endregion Functions
    }
}
