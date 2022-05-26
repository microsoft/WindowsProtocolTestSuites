// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMGetQueryStatusIn message requests the status of a query. 
    /// </summary>
    public class CPMGetQueryStatusIn : IWspInMessage
    {
        #region Fields
        /// <summary>
        /// A 32-bit unsigned integer representing the handle from the CPMCreateQueryOut message identifying the query for which to retrieve status information.
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
