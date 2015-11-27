// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{
    /// <summary>
    /// Enum for CLAIMS_SET compression
    /// </summary>
    public enum CLAIMS_COMPRESSION_FORMAT
    {
        COMPRESSION_FORMAT_NONE = 0x0,
        COMPRESSION_FORMAT_DEFAULT = 0x1,
        COMPRESSION_FORMAT_LZNT1 = 0x2,
        COMPRESSION_FORMAT_XPRESS = 0x3,
        COMPRESSION_FORMAT_XPRESS_HUFF = 0x4,
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIMS_SET_METADATA
    {

        /// ULONG->unsigned int
        public uint ulClaimsSetSize;

        /// BYTE*
        [Size("ulClaimsSetSize")]
        public byte[] ClaimsSet;

        /// CLAIMS_COMPRESSION_FORMAT->_CLAIMS_FORMAT
        public CLAIMS_COMPRESSION_FORMAT usCompressionFormat;

        /// ULONG->unsigned int
        public uint ulUncompressedClaimsSetSize;

        /// USHORT->unsigned short
        public ushort usReservedType;

        /// ULONG->unsigned int
        public uint ulReservedFieldSize;

        /// BYTE*
        [Size("ulReservedFieldSize")]
        public byte[] ReservedField;
    }

    public enum CLAIMS_SOURCE_TYPE
    {

        /// CLAIMS_SOURCE_TYPE_AD -> 1
        CLAIMS_SOURCE_TYPE_AD = 1,

        /// CLAIMS_SOURCE_TYPE_CERTIFICATE -> (CLAIMS_SOURCE_TYPE_AD+1)
        CLAIMS_SOURCE_TYPE_CERTIFICATE = (CLAIMS_SOURCE_TYPE.CLAIMS_SOURCE_TYPE_AD + 1),
    }

    public enum CLAIM_TYPE
    {

        /// CLAIM_TYPE_INT64 -> 1
        CLAIM_TYPE_INT64 = 1,

        /// CLAIM_TYPE_UINT64 -> 2
        CLAIM_TYPE_UINT64 = 2,

        /// CLAIM_TYPE_STRING -> 3
        CLAIM_TYPE_STRING = 3,

        /// CLAIM_TYPE_BOOLEAN -> 6
        CLAIM_TYPE_BOOLEAN = 6,
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIM_TYPE_VALUE_INT64
    {

        /// ULONG->unsigned int
        public uint ValueCount;

        /// LONG64*
        [Size("ValueCount")]
        [MarshalAs(UnmanagedType.ByValArray)]
        public Int64[] Int64Values;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIM_TYPE_VALUE_UINT64
    {

        /// ULONG->unsigned int
        public uint ValueCount;

        /// ULONG64*
        [Size("ValueCount")]
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt64[] Uint64Values;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIM_TYPE_VALUE_LPWSTR
    {

        /// ULONG->unsigned int
        public uint ValueCount;

        /// LPWSTR*
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public string[] StringValues;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIM_TYPE_VALUE_BOOL
    {

        /// ULONG->unsigned int
        public uint ValueCount;

        /// ULONG64*
        [MarshalAs(UnmanagedType.ByValArray)]
        public bool[] BooleanValues;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIM_TYPE_VALUE_BAD
    {
        public uint ValueCount;
    }

    [Union("Type")]
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIM_ENTRY_VALUE_UNION
    {
        /// Anonymous_b3e6089d_89ef_41e8_ab86_81e2f84c5ca8  CLAIM_TYPE_VALUE_BAD
        [Case("0")]
        //  [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public CLAIM_TYPE_VALUE_BAD Struct0;

        //    /// Anonymous_b3e6089d_89ef_41e8_ab86_81e2f84c5ca8 
        [Case("1")]
        //    [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public CLAIM_TYPE_VALUE_INT64 Struct1;

        //    /// Anonymous_d0149cae_14f4_4ea8_9d10_859e2367335b
        [Case("2")]
        //    [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public CLAIM_TYPE_VALUE_UINT64 Struct2;

        //    /// Anonymous_81c16a4e_9b96_4b57_a7ed_fd2d06e6699e
        [Case("3")]
        //    [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public CLAIM_TYPE_VALUE_LPWSTR Struct3;

        //    /// Anonymous_62b8cf30_3f61_49d3_b707_3808779f1319
        [Case("6")]
        //    [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public CLAIM_TYPE_VALUE_BOOL Struct4;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIM_ENTRY
    {

        /// CLAIM_ID->wchar_t*
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public string Id;


        public CLAIM_TYPE Type;
        /// CLAIM_ENTRY_VALUE_UNION
        [Switch("Type")]
        public CLAIM_ENTRY_VALUE_UNION Values;


    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIMS_ARRAY
    {

        /// CLAIMS_SOURCE_TYPE->_CLAIMS_SOURCE_TYPE
        public short usClaimsSourceType;

        /// ULONG->unsigned int
        public uint ulClaimsCount;

        /// PCLAIM_ENTRY->_CLAIM_ENTRY*
        [Size("ulClaimsCount")]
        public CLAIM_ENTRY[] ClaimEntries;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLAIMS_SET
    {

        /// ULONG->unsigned int
        public uint ulClaimsArrayCount;

        /// PCLAIMS_ARRAY->_CLAIMS_ARRAY*
        [Size("ulClaimsArrayCount")]
        public CLAIMS_ARRAY[] ClaimsArrays;

        /// USHORT->unsigned short
        public ushort usReservedType;

        /// ULONG->unsigned int
        public uint ulReservedFieldSize;

        /// BYTE*
        [Size("ulReservedFieldSize")]
        public byte[] ReservedField;
    }
}
