// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// Common interface for WSP structures and messages.
    /// </summary>
    public interface IWSPObject
    {
        /// <summary>
        /// Marshall WSP object to bytes.
        /// </summary>
        /// <param name="buffer">The WSP buffer used to contain marshalled bytes.</param>
        void ToBytes(WSPBuffer buffer);
    }
}
