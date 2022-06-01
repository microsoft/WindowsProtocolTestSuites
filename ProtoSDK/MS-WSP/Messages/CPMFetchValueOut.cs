// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMFetchValueOut message replies to a CPMFetchValueIn message with a property value from a previous query. 
    /// </summary>
    public class CPMFetchValueOut : IWspOutMessage
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the total size, in bytes, of vValue.
        /// </summary>
        public uint _cbValue;

        /// <summary>
        /// A Boolean value indicating whether additional CPMFetchValueOut messages are available.
        /// </summary>
        public uint _fMoreExists;

        /// <summary>
        /// A Boolean value indicating whether there is a value for the property.
        /// </summary>
        public uint _fValueExists;

        /// <summary>
        /// A portion of a byte array containing a SERIALIZEDPROPERTYVALUE.
        /// </summary>
        public byte[] vValue;

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            Header = buffer.ToStruct<WspMessageHeader>();

            _cbValue = buffer.ToStruct<uint>();

            _fMoreExists = buffer.ToStruct<uint>();

            _fValueExists = buffer.ToStruct<uint>();

            if (_fValueExists == 1)
            {
                vValue = buffer.ReadBytes((int)_cbValue);
            }
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}

