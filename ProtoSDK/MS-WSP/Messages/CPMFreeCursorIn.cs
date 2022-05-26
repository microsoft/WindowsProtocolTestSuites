// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMFreeCursorIn message requests the release of a cursor.
    /// </summary>
    public class CPMFreeCursorIn : IWspInMessage
    {
        #region Fields
        /// <summary>
        /// A 32-bit value representing the handle of the cursor from the CPMCreateQueryOut message to release.
        /// </summary>
        public uint _hCursor;
        #endregion

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            Header.ToBytes(buffer);

            buffer.Add(_hCursor);
        }
    }
}