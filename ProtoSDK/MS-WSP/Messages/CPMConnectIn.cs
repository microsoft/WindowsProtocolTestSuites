// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMConnectIn message begins a session between the client and server.
    /// </summary>
    public class CPMConnectIn : IWspInMessage
    {
        /// <summary>
        /// A 32-bit integer indicating whether the server is to validate the checksum value specified in the _ulChecksum field of the message headers for messages sent by the client.
        /// </summary>
        public uint _iClientVersion;

        /// <summary>
        /// A Boolean value indicating if the client is running on a different machine than the server.
        /// This field is set to 0x00000001 if the client is running on a different machine and 0x00000000 if it is not.
        /// </summary>
        public uint _fClientIsRemote;

        /// <summary>
        /// A 32-bit unsigned integer indicating the size, in bytes, of the cPropSets, PropertySet1, and PropertySet2 fields combined.
        /// </summary>
        public uint _cbBlob1;

        /// <summary>
        /// A 32-bit unsigned integer indicating the size in bytes of the cExtPropSet and aPropertySet fields, combined.
        /// </summary>
        public uint _cbBlob2;

        /// <summary>
        /// The machine name of the client. The name string MUST be a null-terminated array of less than 512 Unicode characters, including the null terminator.
        /// The server MUST ignore this field upon receipt.
        /// </summary>
        public string MachineName;

        /// <summary>
        /// A string that represents the user name of the person who is running the application that invoked this protocol.
        /// The name string MUST be a null-terminated array of less than 512 Unicode characters when concatenated with MachineName.
        /// The server MUST ignore this field upon receipt.
        /// </summary>
        public string UserName;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of CDbPropSet structures following this field.
        /// </summary>
        public uint cPropSets;

        /// <summary>
        /// A CDbPropSet structure with guidPropertySet containing DBPROPSET_FSCIFRMWRK_EXT.
        /// </summary>
        public CDbPropSet PropertySet1;

        /// <summary>
        /// A CDbPropSet structure with guidPropertySet containing DBPROPSET_CIFRMWRKCORE_EXT.
        /// </summary>
        public CDbPropSet PropertySet2;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of CDbPropSet structures following this field.
        /// This field must be greater than or equal to 1.
        /// </summary>
        public uint cExtPropSet;

        /// <summary>
        /// An array of CDbPropSet structures specifying other properties. The number of elements in this array MUST be equal to cExtPropSet.
        /// </summary>
        public CDbPropSet[] aPropertySets;

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            var bodyBytes = GetBodyBytes();

            var checksum = Helper.CalculateChecksum(WspMessageHeader_msg_Values.CPMConnectIn, bodyBytes);

            var header = Header;

            header._ulChecksum = checksum;

            Header = header;

            Header.ToBytes(buffer);

            buffer.AddRange(bodyBytes);
        }

        private byte[] GetBodyBytes()
        {
            var tempBuffer = new WspBuffer();

            tempBuffer.Add(_iClientVersion);

            tempBuffer.Add(_fClientIsRemote);

            var bufferForBlob1 = new WspBuffer();

            bufferForBlob1.Add(cPropSets);

            PropertySet1.ToBytes(bufferForBlob1);

            PropertySet2.ToBytes(bufferForBlob1);

            _cbBlob1 = (uint)bufferForBlob1.WriteOffset;

            tempBuffer.Add(_cbBlob1);

            var bufferForBlob2 = new WspBuffer();

            bufferForBlob2.Add(cExtPropSet, 8);

            foreach (var propertySet in aPropertySets)
            {
                propertySet.ToBytes(bufferForBlob2);
            }

            _cbBlob2 = (uint)bufferForBlob2.WriteOffset;

            tempBuffer.Add(_cbBlob2, 8);

            tempBuffer.AddRange(new byte[12]);

            tempBuffer.AddUnicodeString(MachineName);

            tempBuffer.AddUnicodeString(UserName);

            tempBuffer.AlignWrite(8);

            tempBuffer.AddRange(bufferForBlob1.GetBytes());

            tempBuffer.AlignWrite(8);

            tempBuffer.AddRange(bufferForBlob2.GetBytes());

            return tempBuffer.GetBytes();
        }
    }
}
