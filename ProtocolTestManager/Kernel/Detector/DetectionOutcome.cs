// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This enum defines the status of Detection.
    /// </summary>
    public enum DetectionStatus { Inprogress, Finished, Skipped, NotFound, Error }

    /// <summary>
    /// This class defines the outcome structure of detection.
    /// </summary>
    public class DetectionOutcome
    {
        public DetectionOutcome(DetectionStatus status, Exception e)
        {
            Status = status;
            Exception = e;
        }

        /// <summary>
        /// Status of detection.
        /// </summary>
        public DetectionStatus Status;

        /// <summary>
        /// Exception of detection.
        /// </summary>
        public Exception Exception;
    }
}
