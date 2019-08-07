// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    public struct VT_LPWSTR : IWSPObject
    {
        #region Fields
        /// <summary>
        /// A 32-bit unsigned integer, indicating the size of the string field including the terminating null. 
        /// </summary>
        public UInt32 cLen;

        /// <summary>
        /// Null-terminated string.
        /// </summary>
        public string _string;
        #endregion

        #region Constructors
        public VT_LPWSTR(string value)
        {
            cLen = 0;
            _string = value;
        }
        #endregion

        public void ToBytes(WSPBuffer buffer)
        {
            if (String.IsNullOrEmpty(_string))
            {
                cLen = 0;
                buffer.Add(cLen);
            }
            else
            {
                var tempBuffer = new WSPBuffer();

                tempBuffer.Add(_string);

                cLen = (UInt32)tempBuffer.Offset;

                buffer.Add(cLen);

                buffer.AddRange(tempBuffer.GetBytes());
            }
        }
    }
}
