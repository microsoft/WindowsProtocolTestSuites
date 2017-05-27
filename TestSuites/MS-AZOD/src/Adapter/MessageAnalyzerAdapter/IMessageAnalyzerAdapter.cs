// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.Azod.Adapter.Util;

namespace Microsoft.Protocol.TestSuites.Azod.Adapter
{
    public interface IMessageAnalyzerAdapter : IAdapter
    {
        #region Properties

        string ExpectedSequenceFilePath { get; set; }
        string CapturedMessagesSavePath { get; set; }
        string SelectedMessagesSavePath { get; set; }
        string Filter { get; set; }

        #endregion Properties

        #region Functions

        /// <summary>
        /// Configure the adapter
        /// </summary>
        /// <param name="endpointRoles">Role Dictionary</param>
        /// <param name="capturedMessagesSavePath">File path to save captured messages</param>
        /// <param name="selectedMessagesSavePath">File path to save selected messages</param>
        /// <param name="expectedSequenceFilePath">File path for expected messages sequence</param>
        void ConfigureAdapter(Dictionary<string, EndpointRole> endpointRoles, string capturedMessagesSavePath,
            string selectedMessagesSavePath, string expectedSequenceFilePath);

        /// <summary>
        /// Start a live capture to capture messages
        /// </summary>
        /// <param name="isVerify">If true, verify the messages during capturing; If false, don't verify the messages</param>
        void StartCapture(bool isVerify = true);

        /// <summary>
        /// Stop the live capture
        /// </summary>
        void StopCapture();

        /// <summary>
        /// Parse and verify a capture file
        /// </summary>
        /// <param name="captureFilePath">Path of the capture file</param>
        /// <param name="saveResult">If true, save the captured messages and selected messages. If false, don't save</param>
        bool ParseAndVerify(string captureFilePath, bool saveResult = false);

        /// <summary>
        /// Load the expected message sequence from an expected sequence file
        /// </summary>
        /// <param name="expectedSequenceFilePath">File path for expected messages sequence</param>
        void LoadExpectedSequence(string expectedSequenceFilePath);

        #endregion Functions
    }
}
