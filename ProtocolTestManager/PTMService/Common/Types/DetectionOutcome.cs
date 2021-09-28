// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
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
        public DetectionStatus Status { get; set; }

        /// <summary>
        /// Exception of detection.
        /// </summary>
        public Exception Exception { get; set; }
    }
}