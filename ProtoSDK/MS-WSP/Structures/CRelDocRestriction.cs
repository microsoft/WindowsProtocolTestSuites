// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// A CRelDocRestriction structure contains a relevant document ID.
    /// </summary>
    public struct CRelDocRestriction : IWSPObject
    {
        /// <summary>
        /// A CBaseStorageVariant structure that specifies a relevant document for a relevance feedback query.
        /// The vType field of the _vDocument structure MUST be set to either VT_I4 or to VT_BSTR, specifying a URL string. 
        /// </summary>
        public CBaseStorageVariant _vDocument;

        public void ToBytes(WSPBuffer buffer)
        {
            _vDocument.ToBytes(buffer);
        }
    }
}
