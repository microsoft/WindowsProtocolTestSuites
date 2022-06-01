// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CScopeRestriction structure restricts the files to be returned to those with a path that matches the restriction.
    /// </summary>
    public struct CScopeRestriction : IWspRestriction
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the number of Unicode characters in the _lowerPath field.
        /// </summary>
        public uint CcLowerPath;

        /// <summary>
        /// A non-null-terminated Unicode string representing the path to which the query is restricted. The CcLowerPath field contains the length of the string.
        /// </summary>
        public string _lowerPath;

        /// <summary>
        /// A 32-bit unsigned integer containing the length of _lowerPath in Unicode characters. This MUST be the same value as CcLowerPath.
        /// </summary>
        public uint _length;

        /// <summary>
        /// A 32-bit unsigned integer. MUST be set to one of the following values:
        /// 0x00000000	The server is not to examine any subdirectories.
        /// 0x00000001	The server is to recursively examine all subdirectories of the path.
        /// </summary>
        public uint _fRecursive;

        /// <summary>
        /// A 32-bit unsigned integer. MUST be set to one of the following values: 
        /// 0x00000000	_lowerPath is a file system path.
        /// 0x00000001	_lowerPath is a virtual path(the URL associated with a physical directory on the file system) for a website.
        /// 0x00000001	_lowerPath is a virtual path(the URL associated with a physical directory on the file system) for a website.
        /// </summary>
        public uint _fVirtual;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(CcLowerPath);

            buffer.AddUnicodeString(_lowerPath, false);

            buffer.Add(_length, 4);

            buffer.Add(_fRecursive);

            buffer.Add(_fVirtual);
        }
    }
}
