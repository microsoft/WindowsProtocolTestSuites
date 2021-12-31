// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CRowSeekByBookmark structure identifies the bookmarks from which to begin retrieving rows for a CPMGetRowsIn message.
    /// </summary>
    public struct CRowSeekByBookmark : IWspSeekDescription
    {
        /// <summary>
        /// A 32-bit unsigned integer representing the number of elements in _aBookmarks array.
        /// </summary>
        public uint _cBookmarks;

        /// <summary>
        /// An array of bookmark handles (each represented by 4 bytes), as obtained from a previous CPMGetRowsOut message.
        /// </summary>
        public uint[] _aBookmarks;

        /// <summary>
        /// A 32-bit unsigned integer representing the number of elements in the _ascRet array.
        /// </summary>
        public uint _maxRet;

        /// <summary>
        /// An array of HRESULT values. When the CRowSeekByBookmark is sent as part of the CPMGetRowsIn request, the number of entries in the array MUST be equal to _maxRet.
        /// When sent by the client, MUST be set to zero when sent and MUST be ignored on receipt.
        /// When sent by the server (as part of the CPMGetRowsOut message), the values in the array indicate the result status for each row retrieval.
        /// </summary>
        public uint[] _ascRet;

        public void FromBytes(WspBuffer buffer)
        {
            _cBookmarks = buffer.ToStruct<uint>();
            _aBookmarks = new uint[_cBookmarks];
            for (int i = 0; i < _cBookmarks; i++)
            {
                _aBookmarks[i] = buffer.ToStruct<uint>();
            }

            _maxRet = buffer.ToStruct<uint>();
            _ascRet = new uint[_maxRet];
            for (int i = 0; i < _maxRet; i++)
            {
                _ascRet[i] = buffer.ToStruct<uint>();
            }
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_cBookmarks);

            foreach (var bookmark in _aBookmarks)
            {
                buffer.Add(bookmark);
            }

            buffer.Add(_maxRet);

            foreach (var ret in _ascRet)
            {
                buffer.Add(ret);
            }
        }
    }
}
