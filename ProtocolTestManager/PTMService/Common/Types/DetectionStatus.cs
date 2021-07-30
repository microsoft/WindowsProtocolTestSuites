// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    /// <summary>
    /// This enum defines the status of Detection.
    /// </summary>
    public enum DetectionStatus 
    { 
        /// <summary>
        /// Detection not start.
        /// </summary>
        NotStart,

        /// <summary>
        /// Detection in progress.
        /// </summary>
        InProgress, 

        /// <summary>
        /// Detection finished.
        /// </summary>
        Finished, 

        /// <summary>
        /// Detection step skipped.
        /// </summary>
        Skipped, 

        /// <summary>
        /// Detection step not found.
        /// </summary>
        NotFound, 

        /// <summary>
        /// Error occured running detection step
        /// </summary>
        Error 
    }
}