// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Microsoft.Protocols.TestManager.Detector
{
    public class DetectContext
    {
        public delegate void StepStatusChangedHandler(string detectorInstanceId, int stepId, LogStyle style);

        //
        // Summary:
        //     Gets whether cancellation has been requested.
        //
        // Returns:
        //     true if cancellation has been requested;
        //     otherwise, false.
        public CancellationToken Token
        {
            get;
        }

        //
        // Summary:
        //     Gets an ID of current detection instance.
        //
        // Returns:
        //     The identifier that is assigned by the system of current detection instance
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Path to the test suite installation directory.
        /// </summary>
        public string TestSuitePath { get; }

        /// <summary>
        /// Notify StepStatusChanged
        /// </summary>
        public StepStatusChangedHandler StepStatusChanged { get; private set; }

        public DetectContext(StepStatusChangedHandler stepHandler, CancellationToken token, string path)
        {
            Id = Guid.NewGuid().ToString();
            StepStatusChanged = stepHandler;
            Token = token;
            TestSuitePath = path;
        }
    }
}
