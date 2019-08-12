// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CFeedbackRestriction structure contains the number of relevant documents and a property specification for a relevance feedback query.
    /// </summary>
    public struct CFeedbackRestriction : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the count of relevant documents.
        /// </summary>
        public UInt32 _cFeedbackDoc;

        /// <summary>
        /// A CFullPropSpec structure, specifying a property.
        /// </summary>
        public CFullPropSpec _Property;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(_cFeedbackDoc);

            _Property.ToBytes(buffer);
        }
    }
}
