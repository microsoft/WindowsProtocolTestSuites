// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// Helper class provides utility methods which are used through out Adapter implementation and response validation
    /// </summary>
    public class Helper
    {
        #region  Utility Methods

        /// <summary>
        /// Read unsigned Int from a byte array
        /// </summary>
        /// <param name="bytes">Byte array</param>
        /// <param name="startingIndex">Starting Index to read</param>
        /// <returns>unsigned int</returns>
        public static uint GetUInt(byte[] bytes, ref int startingIndex)
        {
            byte[] tempArray = new byte[Constants.SIZE_OF_UINT];
            for (int i = 0; i < Constants.SIZE_OF_UINT; i++)
            {
                tempArray[i] = bytes[i + startingIndex];
            }
            startingIndex += Constants.SIZE_OF_UINT;
            // Values are retrieved in little indian format
            return BitConverter.ToUInt32(tempArray, 0);

        }

        /// <summary>
        /// Appends bytes to the already existing Array
        /// </summary>
        /// <param name="mainBlob">destination array where 
        /// new bytes are to be appended</param>
        /// <param name="index">starting index (from where to append)</param>
        /// <param name="bytesToBeAppended">source bytes</param>
        public static void CopyBytes(byte[] mainBlob, ref int index, byte[] bytesToBeAppended)
        {
            if (mainBlob.Length - index >= bytesToBeAppended.Length)
            {
                for (int i = 0; i < bytesToBeAppended.Length; i++)
                {
                    mainBlob[i + index] = bytesToBeAppended[i];
                }
                index += bytesToBeAppended.Length;
            }
            else
            {
                throw new IndexOutOfRangeException(
                    "Source buffer is not suffcient for the copy Operation");
            }
        }

        /// <summary>
        /// Retrieves a data as byte array from a containing BLOB
        /// </summary>
        /// <param name="bytes">Parent BLOB object</param>
        /// <param name="index">Index from where the data should be retrieved</param>
        /// <param name="dataWidth">Length of the data field</param>
        /// <returns></returns>
        public static byte[] GetData(byte[] bytes, ref int index, int dataWidth)
        {
            byte[] tempArray = new byte[dataWidth];
            for (int i = 0; i < dataWidth; i++)
            {
                tempArray[i] = bytes[i + index];
            }
            index += dataWidth;
            return tempArray;
        }

        #endregion

        # region Requirements Validation Methods

        /// <summary>
        /// A short cut for Contracts.Requires using a condition,
        /// requirement id and description. 
        /// Every requires which captures a requirement should use this.
        /// </summary>
        /// <param name="condition">Condition to be verified</param>
        /// <param name="requirementId">Requirement Id</param>
        /// <param name="description">Requirement Description or text</param>
        public static void Requires(bool condition, int requirementId, string description)
        {
            //Contracts.Requires(condition, 
            //    MakeReqId(requirementId, description));
        }

        /// <summary>
        /// A short cut for Contracts.Requires using a requirement id and description. 
        /// Every requires which captures a requirement should use this.
        /// </summary>
        public static void RequiresCapture(int requirementId, string description)
        {
            //Requirements.Capture(MakeReqId(requirementId, description));
        }

        # endregion

        /// <summary>
        /// Fetches 4 byte unsigned integer from a BLOB without traversing
        /// </summary>
        /// <param name="bytes">BLOB to read 4 bytes from</param>
        /// <param name="startingIndex">offset in the BLOB</param>
        /// <returns>4 byte unsigned integer</returns>
        public static uint GetUIntWithoutAdvancing(byte[] bytes, int startingIndex)
        {
            byte[] tempArray = new byte[Constants.SIZE_OF_UINT];
            for (int i = 0; i < Constants.SIZE_OF_UINT; i++)
            {
                tempArray[i] = bytes[i + startingIndex];
            }
            return BitConverter.ToUInt32(tempArray, 0);
        }

        public static byte[] ToBytes(IWspObject message)
        {
            var buffer = new WspBuffer();

            message.ToBytes(buffer);

            return buffer.GetBytes();
        }

        public static void FromBytes<T>(ref T t, byte[] bytes) where T : IWspObject, new()
        {
            var buffer = new WspBuffer(bytes);

            t.FromBytes(buffer);
        }

        public static uint CalculateChecksum(WspMessageHeader_msg_Values msg, byte[] messageBlob)
        {
            uint checksum = 0;

            if (messageBlob.Length % 4 != 0)
            {
                Array.Resize
                    (ref messageBlob,
                    messageBlob.Length
                    + (4 - messageBlob.Length % 4));
            }

            int index = 0;

            while (index != messageBlob.Length)
            {
                checksum += Helper.GetUInt(messageBlob, ref index);
            }

            checksum = checksum ^ WspConsts.ChecksumMagicNumber;
            checksum -= (uint)msg;

            return checksum;
        }

        /// <summary>
        /// Update the offset related fields of each CTableColumn.
        /// </summary>
        /// <param name="columns">Array of CTableColumn.</param>
        public static void UpdateTableColumns(CTableColumn[] columns)
        {
            ushort offset = 0;

            for (int i = 0; i < columns.Length; i++)
            {
                // Status occupies 1 byte.
                columns[i].StatusOffset = offset;

                offset += 1;

                // Length occupies 4 bytes.
                columns[i].LengthOffset = offset;

                offset += 4;

                // Value occupies ValueSize bytes.
                columns[i].ValueOffset = offset;

                offset += columns[i].ValueSize.Value;
            }
        }

        /// <summary>
        /// Returns the Storage SIZE of a given BaseStorageVariant type.
        /// </summary>
        /// <param name="type">vType_Values.</param>
        /// <param name="is64Bit">Is 64-bit supported by both client and server.</param>
        /// <returns>size in bytes</returns>
        public static ushort GetSize(CBaseStorageVariant_vType_Values type, bool is64Bit)
        {
            ushort size = 0;
            switch (type)
            {
                case CBaseStorageVariant_vType_Values.VT_EMPTY:
                    break;

                case CBaseStorageVariant_vType_Values.VT_NULL:
                    break;

                case CBaseStorageVariant_vType_Values.VT_I1:
                case CBaseStorageVariant_vType_Values.VT_UI1:
                    size = 1; // Take 1 Byte
                    break;

                case CBaseStorageVariant_vType_Values.VT_I2:
                case CBaseStorageVariant_vType_Values.VT_UI2:
                case CBaseStorageVariant_vType_Values.VT_BOOL:
                    size = 2; // Take 2 Bytes
                    break;

                case CBaseStorageVariant_vType_Values.VT_I4:
                case CBaseStorageVariant_vType_Values.VT_UI4:
                case CBaseStorageVariant_vType_Values.VT_INT:
                case CBaseStorageVariant_vType_Values.VT_UINT:
                case CBaseStorageVariant_vType_Values.VT_ERROR:
                case CBaseStorageVariant_vType_Values.VT_R4:
                    size = 4; // Take 4 byte
                    break;

                case CBaseStorageVariant_vType_Values.VT_I8:
                case CBaseStorageVariant_vType_Values.VT_UI8:
                case CBaseStorageVariant_vType_Values.VT_CY:
                case CBaseStorageVariant_vType_Values.VT_R8:
                case CBaseStorageVariant_vType_Values.VT_DATE:
                case CBaseStorageVariant_vType_Values.VT_CLSID:
                case CBaseStorageVariant_vType_Values.VT_FILETIME:
                    size = 8;
                    break;

                case CBaseStorageVariant_vType_Values.VT_DECIMAL:
                    size = 12;
                    break;

                case CBaseStorageVariant_vType_Values.VT_VARIANT:
                    if (is64Bit)
                    {
                        size = 24;
                    }
                    else
                    {
                        size = 16;
                    }
                    break;

                default:
                    break;
            }

            return size;
        }
    }
}

