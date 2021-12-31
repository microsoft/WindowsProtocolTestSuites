// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMFetchValueIn message requests a property value that was too large to return in a rowset. 
    /// </summary>
    public class CPMFetchValueIn : IWspInMessage
    {
        /// <summary>
        /// A 32-bit unsigned integer representing the document ID identifying the document for which a property is to be fetched.
        /// </summary>
        public uint _wid;

        /// <summary>
        /// A 32-bit unsigned integer containing the number of bytes previously transferred for this property.
        /// </summary>
        public uint _cbSoFar;

        /// <summary>
        /// A 32-bit unsigned integer containing the size, in bytes, of the PropSpec field.
        /// </summary>
        public uint _cbPropSpec;

        /// <summary>
        /// A 32-bit unsigned integer containing the maximum number of bytes that the sender can accept in a CPMFetchValueOut message.
        /// </summary>
        public uint _cbChunk;

        /// <summary>
        /// A CFullPropSpec structure specifying the property to retrieve. 
        /// </summary>
        public CFullPropSpec PropSpec;

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            var bodyBytes = GetBodyBytes();

            var checksum = Helper.CalculateChecksum(WspMessageHeader_msg_Values.CPMFetchValueIn, bodyBytes);

            var header = Header;

            header._ulChecksum = checksum;

            Header = header;

            Header.ToBytes(buffer);

            buffer.AddRange(bodyBytes);
        }

        public byte[] GetBodyBytes()
        {
            var buffer = new WspBuffer();

            buffer.Add(_wid);

            buffer.Add(_cbSoFar);

            var tempBuffer = new WspBuffer();

            PropSpec.ToBytes(tempBuffer);

            var propSpecBytes = tempBuffer.GetBytes();

            _cbPropSpec = (uint)propSpecBytes.Length;

            buffer.Add(_cbPropSpec);

            buffer.Add(_cbChunk);

            buffer.AddRange(propSpecBytes);

            buffer.AlignWrite(4);

            return buffer.GetBytes();
        }
    }
}
