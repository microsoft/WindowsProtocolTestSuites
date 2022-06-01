// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMFreeCursorOut message replies to a CPMFreeCursorIn message with the results of freeing a cursor.
    /// </summary>
    public class CPMFreeCursorOut : IWspOutMessage
    {
        #region Fields
        /// <summary>
        /// A 32-bit unsigned integer indicating the number of cursors still in use for the query.
        /// </summary>
        public uint _cCursorsRemaining;
        #endregion

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            Header = buffer.ToStruct<WspMessageHeader>();

            _cCursorsRemaining = buffer.ToStruct<uint>();
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
