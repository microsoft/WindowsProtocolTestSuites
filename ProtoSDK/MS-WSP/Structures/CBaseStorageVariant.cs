// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    [Flags]
    public enum CBaseStorageVariant_vType_Values : ushort
    {
        /// <summary>
        /// vValue is not present.
        /// </summary>
        VT_EMPTY = 0x0000,

        /// <summary>
        /// vValue is not present.
        /// </summary>
        VT_NULL = 0x0001,

        /// <summary>
        /// A 1-byte signed integer.
        /// </summary>
        VT_I1 = 0x0010,

        /// <summary>
        /// A 1-byte unsigned integer.
        /// </summary>
        VT_UI1 = 0x0011,

        /// <summary>
        /// A 2-byte signed integer.
        /// </summary>
        VT_I2 = 0x0002,

        /// <summary>
        /// A 2-byte unsigned integer.
        /// </summary>
        VT_UI2 = 0x0012,

        /// <summary>
        /// A Boolean value; a 2-byte integer.
        /// Contains 0x0000 (FALSE) or 0xFFFF (TRUE).
        /// </summary>
        VT_BOOL = 0x000B,

        /// <summary>
        /// A 4-byte signed integer.
        /// </summary>
        VT_I4 = 0x0003,

        /// <summary>
        /// A 4-byte unsigned integer.
        /// </summary>
        VT_UI4 = 0x0013,

        /// <summary>
        /// An IEEE 32-bit floating point number, as defined in [IEEE754].
        /// </summary>
        VT_R4 = 0x0004,

        /// <summary>
        /// A 4-byte signed integer.
        /// </summary>
        VT_INT = 0x0016,

        /// <summary>
        /// A 4-byte unsigned integer. Note that this is identical to VT_UI4, except that VT_UINT cannot be used with VT_VECTOR (defined below);
        /// the value chosen is up to the higher layer that provides it to the Windows Search Protocol Specification, but the Windows Search Protocol Specification treats VT_UINT and VT_UI4 as identical with the exception noted above.
        /// </summary>
        VT_UINT = 0x0017,

        /// <summary>
        /// A 4-byte unsigned integer containing an HRESULT, as specified in [MS-ERREF] section 2.1.
        /// </summary>
        VT_ERROR = 0x000A,

        /// <summary>
        /// An 8-byte signed integer.
        /// </summary>
        VT_I8 = 0x0014,

        /// <summary>
        /// An 8-byte unsigned integer.
        /// </summary>
        VT_UI8 = 0x0015,

        /// <summary>
        /// An IEEE 64-bit floating point number as defined in [IEEE754].
        /// </summary>
        VT_R8 = 0x0005,

        /// <summary>
        /// An 8-byte two's complement integer (vValue divided by 10,000).
        /// </summary>
        VT_CY = 0x0006,

        /// <summary>
        /// A 64-bit floating point number representing the number of days since 00:00:00 on December 31, 1899 (Coordinated Universal Time).
        /// </summary>
        VT_DATE = 0x0007,

        /// <summary>
        /// A 64-bit integer representing the number of 100-nanosecond intervals since 00:00:00 on January 1, 1601 (Coordinated Universal Time).
        /// </summary>
        VT_FILETIME = 0x0040,

        /// <summary>
        /// A DECIMAL structure as specified in section 2.2.1.1.1.1.
        /// </summary>
        VT_DECIMAL = 0x000E,

        /// <summary>
        /// A 16-byte binary value containing a GUID.
        /// </summary>
        VT_CLSID = 0x0048,

        /// <summary>
        /// A 4-byte unsigned integer count of bytes in the blob, followed by that many bytes of data.
        /// </summary>
        VT_BLOB = 0x0041,

        /// <summary>
        /// A 4-byte unsigned integer count of bytes in the blob, followed by that many bytes of data.
        /// </summary>
        VT_BLOB_OBJECT = 0x0046,

        /// <summary>
        /// A 4-byte unsigned integer count of bytes in the string, followed by a string, as specified below under vValue.
        /// </summary>
        VT_BSTR = 0x0008,

        /// <summary>
        /// A null-terminated string using the system code page.
        /// </summary>
        VT_LPSTR = 0x001E,

        /// <summary>
        /// A null-terminated, 16-bit Unicode string. See[UNICODE].
        /// Note The protocol uses UTF-16 LE encoding.
        /// </summary>
        VT_LPWSTR = 0x001F,

        /// <summary>
        /// A compressed version of a null-terminated, 16-bit Unicode string as specified in section 2.2.1.1.1.6.
        /// Note The protocol uses UTF-16 LE encoding.
        /// </summary>
        VT_COMPRESSED_LPWSTR = 0x0023,

        /// <summary>
        /// CBaseStorageVariant.
        /// </summary>
        VT_VARIANT = 0x000C,

        /// <summary>
        /// If the type indicator is combined with VT_VECTOR by using an OR operator, vValue is a counted array of values of the indicated type. See section 2.2.1.1.1.2.
        /// This type modifier MUST NOT be combined with the following types: VT_INT, VT_UINT, VT_DECIMAL, VT_BLOB, and VT_BLOB_OBJECT.
        /// </summary>
        VT_VECTOR = 0x1000,

        /// <summary>
        /// If the type indicator is combined with VT_ARRAY by an OR operator, the value is a SAFEARRAY containing values of the indicated type.
        /// This type modifier MUST NOT be combined with the following types: VT_I8, VT_UI8, VT_FILETIME, VT_CLSID, VT_BLOB, VT_BLOB_OBJECT, VT_LPSTR, and VT_LPWSTR.
        /// </summary>
        VT_ARRAY = 0x2000,
    }

    /// <summary>
    /// The CBaseStorageVariant structure contains the value on which to perform a match operation for a property specified in the CPropertyRestriction structure.
    /// </summary>
    public struct CBaseStorageVariant : IWspStructure
    {
        #region Fields
        /// <summary>
        /// A type indicator that indicates the type of vValue.
        /// </summary>
        public CBaseStorageVariant_vType_Values vType;

        /// <summary>
        /// When vType is VT_DECIMAL, the value of this field is specified as the Scale field in section 2.2.1.1.1.1. For all other vTypes, the value MUST be set to 0x00.
        /// </summary>
        public byte vData1;

        /// <summary>
        /// When vType is VT_DECIMAL, the value of this field is specified as the Sign field in section 2.2.1.1.1.1. For all other vTypes, the value MUST be set to 0x00.
        /// </summary>
        public byte vData2;

        /// <summary>
        /// The value for the match operation.
        /// </summary>
        public object vValue;
        #endregion

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(vType);

            buffer.Add(vData1);

            buffer.Add(vData2);

            if (!vType.HasFlag(CBaseStorageVariant_vType_Values.VT_VECTOR) && !vType.HasFlag(CBaseStorageVariant_vType_Values.VT_ARRAY))
            {
                switch (vType)
                {
                    case CBaseStorageVariant_vType_Values.VT_EMPTY:
                    case CBaseStorageVariant_vType_Values.VT_NULL:
                        {

                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_I1:
                        {
                            buffer.Add((sbyte)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_UI1:
                        {
                            buffer.Add((byte)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_I2:
                        {
                            buffer.Add((short)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_UI2:
                        {
                            buffer.Add((ushort)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_BOOL:
                        {
                            buffer.Add((ushort)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_I4:
                        {
                            buffer.Add((int)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_UI4:
                        {
                            buffer.Add((uint)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_R4:
                        {
                            buffer.Add((float)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_INT:
                        {
                            buffer.Add((int)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_UINT:
                        {
                            buffer.Add((uint)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_ERROR:
                        {
                            buffer.Add((uint)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_I8:
                        {
                            buffer.Add((long)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_UI8:
                        {
                            buffer.Add((ulong)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_R8:
                        {
                            buffer.Add((double)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_CY:
                        {
                            buffer.Add((long)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_DATE:
                        {
                            buffer.Add((double)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_FILETIME:
                        {
                            buffer.Add((long)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_DECIMAL:
                        {
                            ((DECIMAL)vValue).ToBytes(buffer);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_CLSID:
                        {
                            buffer.Add((Guid)vValue);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_BLOB:
                        {
                            ((VT_BLOB)vValue).ToBytes(buffer);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_BLOB_OBJECT:
                        {
                            ((VT_BLOB_OBJECT)vValue).ToBytes(buffer);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_BSTR:
                        {
                            ((VT_BSTR)vValue).ToBytes(buffer);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_LPSTR:
                        {
                            ((VT_LPSTR)vValue).ToBytes(buffer);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                        {
                            ((VT_LPWSTR)vValue).ToBytes(buffer);
                        }
                        break;

                    case CBaseStorageVariant_vType_Values.VT_COMPRESSED_LPWSTR:
                        {
                            ((VT_COMPRESSED_LPWSTR)vValue).ToBytes(buffer);
                        }
                        break;
                }
            }
            else
            {
                (vValue as IWspStructure).ToBytes(buffer);
            }
        }
    }
}
