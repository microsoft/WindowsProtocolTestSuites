// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CFeedbackRestriction structure contains the number of relevant documents and a property specification for a relevance feedback query.
    /// </summary>
    public struct CFeedbackRestriction : IWspRestriction
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the count of relevant documents.
        /// </summary>
        public uint _cFeedbackDoc;

        /// <summary>
        /// A CFullPropSpec structure, specifying a property.
        /// </summary>
        public CFullPropSpec _Property;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_cFeedbackDoc);

            _Property.ToBytes(buffer);
        }
    }
}
