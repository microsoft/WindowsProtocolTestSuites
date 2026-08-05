// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    /// <summary>
    /// Represents an incremental log payload from a byte offset.
    /// </summary>
    public class DetectionLogChunk
    {
        /// <summary>
        /// Identifies the detection run that owns this log.
        /// </summary>
        public string RunId { get; set; } = string.Empty;

        /// <summary>
        /// The requested start offset.
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// The next offset to request.
        /// </summary>
        public long NextOffset { get; set; }

        /// <summary>
        /// The appended content from the requested offset.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Indicates that detection has stopped and all log content has been flushed.
        /// </summary>
        public bool IsComplete { get; set; }
    }
}
