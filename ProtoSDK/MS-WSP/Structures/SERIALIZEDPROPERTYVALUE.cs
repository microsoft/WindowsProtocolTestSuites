// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CBaseStorageVariant structure contains the value on which to perform a match operation for a property specified in the CPropertyRestriction structure.
    /// </summary>
    public struct SERIALIZEDPROPERTYVALUE : IWspStructure
    {
        #region Fields
        /// <summary>
        /// One of the variant types.
        /// </summary>
        public uint dwType;

        /// <summary>
        /// Serialized value.
        /// </summary>
        public object rgb;
        #endregion

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public static SERIALIZEDPROPERTYVALUE GetSerializedPropertyValue(IEnumerable<byte[]> consequentByteArrays)
        {
            var valueBytes = consequentByteArrays.SelectMany(b => b).ToArray();
            var buffer = new WspBuffer(valueBytes);

            var serializedPropertyValue = new SERIALIZEDPROPERTYVALUE();
            serializedPropertyValue.dwType = buffer.ToStruct<uint>();

            var vType = (CBaseStorageVariant_vType_Values)serializedPropertyValue.dwType;
            object propValue = null;
            if (vType.HasFlag(CBaseStorageVariant_vType_Values.VT_VECTOR))
            {
                propValue = GetVectorValue((vType ^ CBaseStorageVariant_vType_Values.VT_VECTOR), buffer);
            }
            else
            {
                propValue = GetValue(vType, buffer);
            }

            serializedPropertyValue.rgb = propValue;
            return serializedPropertyValue;
        }

        private static object GetVectorValue(CBaseStorageVariant_vType_Values vType, WspBuffer buffer)
        {
            object value = null;
            switch (vType)
            {
                case CBaseStorageVariant_vType_Values.VT_EMPTY:
                case CBaseStorageVariant_vType_Values.VT_NULL:
                    break;

                case CBaseStorageVariant_vType_Values.VT_I1:
                    value = GetVectorValue<sbyte>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI1:
                    value = GetVectorValue<byte>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_I2:
                    value = GetVectorValue<short>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI2:
                case CBaseStorageVariant_vType_Values.VT_BOOL:
                    value = GetVectorValue<ushort>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_I4:
                case CBaseStorageVariant_vType_Values.VT_INT:
                    value = GetVectorValue<int>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_R4:
                    value = GetVectorValue<float>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI4:
                case CBaseStorageVariant_vType_Values.VT_UINT:
                case CBaseStorageVariant_vType_Values.VT_ERROR:
                    value = GetVectorValue<uint>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_I8:
                case CBaseStorageVariant_vType_Values.VT_CY:
                    value = GetVectorValue<long>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI8:
                    value = GetVectorValue<ulong>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_R8:
                    value = GetVectorValue<double>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_DATE:
                    value = GetVectorValue<DateTime>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_FILETIME:
                    value = GetVectorValue<DateTime>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_DECIMAL:
                    value = GetVectorValue<DECIMAL>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_CLSID:
                    value = GetVectorValue<Guid>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_BLOB:
                    value = GetVectorValue<VT_BLOB>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_BLOB_OBJECT:
                    value = GetVectorValue<VT_BLOB_OBJECT>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_BSTR:
                    value = GetVectorValue<VT_BSTR>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_LPSTR:
                    value = GetVectorValue<VT_LPSTR>(vType, buffer);
                    break;


                case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                    value = GetVectorValue<VT_LPWSTR>(vType, buffer);
                    break;

                case CBaseStorageVariant_vType_Values.VT_COMPRESSED_LPWSTR:
                    value = GetVectorValue<VT_COMPRESSED_LPWSTR>(vType, buffer);
                    break;
            }

            return value;
        }

        private static VT_VECTOR<T> GetVectorValue<T>(CBaseStorageVariant_vType_Values vType, WspBuffer buffer) where T : struct
        {
            var vector = new VT_VECTOR<T>();
            vector.vVectorElements = buffer.ToStruct<uint>();
            vector.vVectorData = new T[vector.vVectorElements];
            for (var idx = 0; idx < vector.vVectorElements; idx++)
            {
                vector.vVectorData[idx] = (T)GetValue(vType, buffer);
            }

            return vector;
        }

        private static object GetValue(CBaseStorageVariant_vType_Values vType, WspBuffer buffer)
        {
            object value = null;
            switch (vType)
            {
                case CBaseStorageVariant_vType_Values.VT_EMPTY:
                case CBaseStorageVariant_vType_Values.VT_NULL:
                    break;

                case CBaseStorageVariant_vType_Values.VT_I1:
                    value = buffer.ToStruct<sbyte>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI1:
                    value = buffer.ToStruct<byte>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_I2:
                    value = buffer.ToStruct<short>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI2:
                case CBaseStorageVariant_vType_Values.VT_BOOL:
                    value = buffer.ToStruct<ushort>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_I4:
                case CBaseStorageVariant_vType_Values.VT_INT:
                    value = buffer.ToStruct<int>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_R4:
                    value = buffer.ToStruct<float>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI4:
                case CBaseStorageVariant_vType_Values.VT_UINT:
                case CBaseStorageVariant_vType_Values.VT_ERROR:
                    value = buffer.ToStruct<uint>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_I8:
                case CBaseStorageVariant_vType_Values.VT_CY:
                    value = buffer.ToStruct<long>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI8:
                    value = buffer.ToStruct<ulong>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_R8:
                    value = buffer.ToStruct<double>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_DATE:
                    value = DateTime.FromOADate(buffer.ToStruct<double>()).ToUniversalTime();
                    break;

                case CBaseStorageVariant_vType_Values.VT_FILETIME:
                    value = DateTime.FromFileTime(buffer.ToStruct<long>()).ToUniversalTime();
                    break;

                case CBaseStorageVariant_vType_Values.VT_DECIMAL:
                    value = buffer.ToStruct<DECIMAL>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_CLSID:
                    value = buffer.ToStruct<Guid>();
                    break;

                case CBaseStorageVariant_vType_Values.VT_BLOB:
                    var blob = new VT_BLOB();
                    blob.cbSize = buffer.ToStruct<uint>();
                    blob.blobData = buffer.ReadBytes((int)blob.cbSize);
                    value = blob;
                    break;

                case CBaseStorageVariant_vType_Values.VT_BLOB_OBJECT:
                    var blobObj = new VT_BLOB_OBJECT();
                    blobObj.cbSize = buffer.ToStruct<uint>();
                    blobObj.blobData = buffer.ReadBytes((int)blobObj.cbSize);
                    value = blobObj;
                    break;

                case CBaseStorageVariant_vType_Values.VT_BSTR:
                    var bstr = new VT_BSTR();
                    bstr.cbSize = buffer.ToStruct<uint>();
                    bstr.blobData = buffer.ReadBytes((int)bstr.cbSize);
                    value = bstr;
                    break;

                case CBaseStorageVariant_vType_Values.VT_LPSTR:
                    var lpstr = new VT_LPSTR();
                    lpstr.cLen = buffer.ToStruct<uint>();
                    var strLength = GetNullTerminatedUnicodeStringLength(buffer, buffer.ReadOffset);
                    lpstr._string = Encoding.Unicode.GetString(buffer.ReadBytes((int)(lpstr.cLen * 2)), 0, strLength);
                    value = lpstr;
                    break;


                case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                    var lpwstr = new VT_LPWSTR();
                    lpwstr.cLen = buffer.ToStruct<uint>();
                    strLength = GetNullTerminatedUnicodeStringLength(buffer, buffer.ReadOffset);
                    lpwstr._string = Encoding.Unicode.GetString(buffer.ReadBytes((int)(lpwstr.cLen * 2)), 0, strLength);
                    value = lpwstr;
                    break;

                case CBaseStorageVariant_vType_Values.VT_COMPRESSED_LPWSTR:
                    var compressedLpwstr = new VT_COMPRESSED_LPWSTR();
                    compressedLpwstr.ccLen = buffer.ToStruct<uint>();
                    compressedLpwstr.bytes = buffer.ReadBytes((int)compressedLpwstr.ccLen);
                    value = compressedLpwstr;
                    break;
            }

            return value;
        }

        private static int GetNullTerminatedUnicodeStringLength(WspBuffer buffer, int startOffset)
        {
            int length = 0;
            while (buffer.Peek<ushort>(startOffset) != 0)
            {
                startOffset += 2;
                length += 2;
            }

            return length;
        }
    }
}
