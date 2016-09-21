// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    /// <summary>
    /// BATCHED_OFFER_MESSAGE structure.
    /// </summary>
    public struct BATCHED_OFFER_MESSAGE
    {
        /// <summary>
        /// MessageHeader (8 bytes): A MESSAGE_HEADER structure (section 2.2.1.1), 
        /// with the Type field set to 0x0003.
        /// </summary>
        public MESSAGE_HEADER MessageHeader;

        /// <summary>
        /// ConnectionInformation (8 bytes): A CONNECTION_INFORMATION structure (section 2.2.1.2).
        /// </summary>
        public CONNECTION_INFORMATION ConnectionInfo;

        /// <summary>
        /// The BATCHED_OFFER_MESSAGE contains a sequence of these segment descriptors.
        /// </summary>
        public SegmentDescriptor[] SegmentDescriptors;
    }
}
