// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// IDL format char types.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RpceFormatCharType : byte
    {
        /// <summary>
        /// This might catch some errors.
        /// </summary>
        FC_ZERO = 0,


        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_BYTE = 0x01,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_CHAR = 0x02,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_SMALL = 0x03,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_USMALL = 0x04,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_WCHAR = 0x05,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_SHORT = 0x06,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_USHORT = 0x07,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_LONG = 0x08,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_ULONG = 0x09,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_FLOAT = 0x0a,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_HYPER = 0x0b,

        /// <summary>
        /// Simple integer and floating point types. 
        /// </summary>
        FC_DOUBLE = 0x0c,


        /// <summary>
        /// Enums.
        /// </summary>
        FC_ENUM16 = 0x0d,

        /// <summary>
        /// Enums.
        /// </summary>
        FC_ENUM32 = 0x0e,


        /// <summary>
        /// Special.
        /// </summary>
        FC_IGNORE = 0x0f,

        /// <summary>
        /// Special.
        /// </summary>
        FC_ERROR_STATUS_T = 0x10,


        /// <summary>
        /// Pointer types: RP - reference pointer
        /// </summary>
        FC_RP = 0x11,

        /// <summary>
        /// Pointer types: UP - unique pointer
        /// </summary>     
        FC_UP = 0x12,

        /// <summary>
        /// Pointer types: OP - OLE unique pointer
        /// </summary>
        FC_OP = 0x13,

        /// <summary>
        /// Pointer types: FP - full pointer
        /// </summary>
        FC_FP = 0x14,


        /// <summary>
        /// Structure containing only simple types and fixed arrays.
        /// </summary>
        FC_STRUCT = 0x15,

        /// <summary>
        /// Structure containing only simple types, pointers and fixed arrays.
        /// </summary>
        FC_PSTRUCT = 0x16,

        /// <summary>
        /// Structure containing a conformant array plus all those types
        /// allowed by FC_STRUCT.
        /// </summary>
        FC_CSTRUCT = 0x17,

        /// <summary>
        /// Struct containing a conformant array plus all those types allowed by
        /// FC_PSTRUCT.
        /// </summary>
        FC_CPSTRUCT = 0x18,

        /// <summary>
        /// Struct containing either a conformant varying array or a conformant 
        /// string, plus all those types allowed by FC_PSTRUCT. 
        /// </summary>
        FC_CVSTRUCT = 0x19,

        /// <summary>
        /// Complex struct - totally bogus!
        /// </summary>
        FC_BOGUS_STRUCT = 0x1a,


        /// <summary>
        /// Conformant array.
        /// </summary>
        FC_CARRAY = 0x1b,

        /// <summary>
        /// Conformant varying array.
        /// </summary>
        FC_CVARRAY = 0x1c,

        /// <summary>
        /// Fixed array, small.
        /// </summary>
        FC_SMFARRAY = 0x1d,

        /// <summary>
        /// Fixed array, large.
        /// </summary>
        FC_LGFARRAY = 0x1e,

        /// <summary>
        /// Varying array, small.
        /// </summary>
        FC_SMVARRAY = 0x1f,

        /// <summary>
        /// Varying array, large.
        /// </summary>
        FC_LGVARRAY = 0x20,

        /// <summary>
        /// Complex arrays - totally bogus!
        /// </summary>
        FC_BOGUS_ARRAY = 0x21,


        /// <summary>
        /// Conformant strings. 
        /// CSTRING - character string
        /// </summary>
        FC_C_CSTRING = 0x22,

        /// <summary>
        /// Conformant strings. 
        /// BSTRING - byte string (Beta2 compatibility only)
        /// </summary>     
        FC_C_BSTRING = 0x23,

        /// <summary>
        /// Conformant strings. 
        /// SSTRING - structure string
        /// </summary>
        FC_C_SSTRING = 0x24,

        /// <summary>
        /// Conformant strings. 
        /// WSTRING - wide character string
        /// </summary>
        FC_C_WSTRING = 0x25,

        /// <summary>
        /// Non-conformant strings. 
        /// CSTRING - character string
        /// </summary>
        FC_CSTRING = 0x26,

        /// <summary>
        /// Non-conformant strings. 
        /// BSTRING - byte string (Beta2 compatibility only)
        /// </summary>
        FC_BSTRING = 0x27,

        /// <summary>
        /// Non-conformant strings. 
        /// SSTRING - structure string
        /// </summary>
        FC_SSTRING = 0x28,

        /// <summary>
        /// Non-conformant strings. 
        /// WSTRING - wide character string
        /// </summary>
        FC_WSTRING = 0x29,


        /// <summary>
        /// Unions
        /// </summary>
        FC_ENCAPSULATED_UNION = 0x2a,

        /// <summary>
        /// Unions
        /// </summary>
        FC_NON_ENCAPSULATED_UNION = 0x2b,


        /// <summary>
        /// Byte count pointer.
        /// </summary>
        FC_BYTE_COUNT_POINTER = 0x2c,


        /// <summary>
        /// transmit_as
        /// </summary>     
        FC_TRANSMIT_AS = 0x2d,

        /// <summary>
        /// represent_as
        /// </summary>     
        FC_REPRESENT_AS = 0x2e,


        /// <summary>
        /// Cairo Interface pointer.
        /// </summary>
        FC_IP = 0x2f,


        /// <summary>
        /// Binding handle types
        /// </summary>
        FC_BIND_CONTEXT = 0x30,

        /// <summary>
        /// Binding handle types
        /// </summary>
        FC_BIND_GENERIC = 0x31,

        /// <summary>
        /// Binding handle types
        /// </summary>
        FC_BIND_PRIMITIVE = 0x32,

        /// <summary>
        /// Binding handle types
        /// </summary>
        FC_AUTO_HANDLE = 0x33,

        /// <summary>
        /// Binding handle types
        /// </summary>
        FC_CALLBACK_HANDLE = 0x34,


        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED1 = 0x35,


        /// <summary>
        /// Embedded pointer - used in complex structure layouts only.
        /// </summary>
        FC_POINTER = 0x36,


        /// <summary>
        /// Alignment directives, used in structure layouts.
        /// No longer generated with post NT5.0 MIDL.
        /// </summary>
        FC_ALIGNM2 = 0x37,

        /// <summary>
        /// Alignment directives, used in structure layouts.
        /// No longer generated with post NT5.0 MIDL.
        /// </summary>
        FC_ALIGNM4 = 0x38,

        /// <summary>
        /// Alignment directives, used in structure layouts.
        /// No longer generated with post NT5.0 MIDL.
        /// </summary>
        FC_ALIGNM8 = 0x39,


        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED2 = 0x3a,

        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED3 = 0x3b,

        /// <summary>
        /// Used for [system_handle] IDL attribute.
        /// </summary>
        FC_SYSTEM_HANDLE = 0x3c,

        /// <summary>
        /// Structure padding directives, used in structure layouts only.
        /// </summary>
        FC_STRUCTPAD1 = 0x3d,

        /// <summary>
        /// Structure padding directives, used in structure layouts only.
        /// </summary>
        FC_STRUCTPAD2 = 0x3e,

        /// <summary>
        /// Structure padding directives, used in structure layouts only.
        /// </summary>
        FC_STRUCTPAD3 = 0x3f,

        /// <summary>
        /// Structure padding directives, used in structure layouts only.
        /// </summary>
        FC_STRUCTPAD4 = 0x40,

        /// <summary>
        /// Structure padding directives, used in structure layouts only.
        /// </summary>
        FC_STRUCTPAD5 = 0x41,

        /// <summary>
        /// Structure padding directives, used in structure layouts only.
        /// </summary>
        FC_STRUCTPAD6 = 0x42,

        /// <summary>
        /// Structure padding directives, used in structure layouts only.
        /// </summary>
        FC_STRUCTPAD7 = 0x43,


        /// <summary>
        /// Additional string attribute.
        /// </summary>
        FC_STRING_SIZED = 0x44,


        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED5 = 0x45,


        /// <summary>
        /// Pointer layout attributes.
        /// </summary>
        FC_NO_REPEAT = 0x46,

        /// <summary>
        /// Pointer layout attributes.
        /// </summary>
        FC_FIXED_REPEAT = 0x47,

        /// <summary>
        /// Pointer layout attributes.
        /// </summary>
        FC_VARIABLE_REPEAT = 0x48,

        /// <summary>
        /// Pointer layout attributes.
        /// </summary>
        FC_FIXED_OFFSET = 0x49,

        /// <summary>
        /// Pointer layout attributes.
        /// </summary>
        FC_VARIABLE_OFFSET = 0x4a,


        /// <summary>
        /// Pointer section delimiter.
        /// </summary>
        FC_PP = 0x4b,


        /// <summary>
        /// Embedded complex type.
        /// </summary>
        FC_EMBEDDED_COMPLEX = 0x4c,


        /// <summary>
        /// Parameter attributes.
        /// </summary>
        FC_IN_PARAM = 0x4d,

        /// <summary>
        /// Parameter attributes.
        /// </summary>
        FC_IN_PARAM_BASETYPE = 0x4e,

        /// <summary>
        /// Parameter attributes.
        /// </summary>
        FC_IN_PARAM_NO_FREE_INST = 0x4d,

        /// <summary>
        /// Parameter attributes.
        /// </summary>
        FC_IN_OUT_PARAM = 0x50,

        /// <summary>
        /// Parameter attributes.
        /// </summary>
        FC_OUT_PARAM = 0x51,

        /// <summary>
        /// Parameter attributes.
        /// </summary>
        FC_RETURN_PARAM = 0x52,

        /// <summary>
        /// Parameter attributes.
        /// </summary>
        FC_RETURN_PARAM_BASETYPE = 0x53,


        /// <summary>
        /// Conformance/variance attributes.
        /// </summary>
        FC_DEREFERENCE = 0x54,

        /// <summary>
        /// Conformance/variance attributes.
        /// </summary>
        FC_DIV_2 = 0x55,

        /// <summary>
        /// Conformance/variance attributes.
        /// </summary>
        FC_MULT_2 = 0x56,

        /// <summary>
        /// Conformance/variance attributes.
        /// </summary>
        FC_ADD_1 = 0x57,

        /// <summary>
        /// Conformance/variance attributes.
        /// </summary>
        FC_SUB_1 = 0x58,

        /// <summary>
        /// Conformance/variance attributes.
        /// </summary>
        FC_CALLBACK = 0x59,


        /// <summary>
        /// Iid flag.
        /// </summary>
        FC_CONSTANT_IID = 0x5a,


        /// <summary>
        /// FC_END
        /// </summary>
        FC_END = 0x5b,

        /// <summary>
        /// FC_PAD
        /// </summary>
        FC_PAD = 0x5c,

        /// <summary>
        /// FC_EXPR
        /// </summary>
        FC_EXPR = 0x5d,

        /// <summary>
        /// FC_PARTIAL_IGNORE_PARAM
        /// </summary>
        FC_PARTIAL_IGNORE_PARAM = 0x5e,


        /// <summary>
        /// split Conformance/variance attributes.
        /// </summary>
        FC_SPLIT_DEREFERENCE = 0x74,

        /// <summary>
        /// split Conformance/variance attributes.
        /// </summary>
        FC_SPLIT_DIV_2 = 0x75,

        /// <summary>
        /// split Conformance/variance attributes.
        /// </summary>
        FC_SPLIT_MULT_2 = 0x76,

        /// <summary>
        /// split Conformance/variance attributes.
        /// </summary>
        FC_SPLIT_ADD_1 = 0x77,

        /// <summary>
        /// split Conformance/variance attributes.
        /// </summary>
        FC_SPLIT_SUB_1 = 0x78,

        /// <summary>
        /// split Conformance/variance attributes.
        /// </summary>
        FC_SPLIT_CALLBACK = 0x79,


        /// <summary>
        /// FC_FORCED_BOGUS_STRUCT
        /// </summary>
        FC_FORCED_BOGUS_STRUCT = 0xb1,


        /// <summary>
        /// FC_TRANSMIT_AS_PTR
        /// </summary>
        FC_TRANSMIT_AS_PTR = 0xb2,


        /// <summary>
        /// FC_REPRESENT_AS_PTR
        /// </summary>
        FC_REPRESENT_AS_PTR = 0xb3,


        /// <summary>
        /// FC_USER_MARSHAL
        /// </summary>
        FC_USER_MARSHAL = 0xb4,


        /// <summary>
        /// FC_PIPE
        /// </summary>
        FC_PIPE = 0xb5,


        /// <summary>
        /// FC_SUPPLEMENT
        /// </summary>
        FC_SUPPLEMENT = 0xb6,


        /// <summary>
        /// FC_RANGE, supported from NT 5 beta2 MIDL 3.3.110
        /// </summary>
        FC_RANGE = 0xb7,


        /// <summary>
        /// FC_INT3264, supported from NT 5 beta2, MIDL64, 5.1.194+
        /// </summary>
        FC_INT3264 = 0xb8,

        /// <summary>
        /// FC_UINT3264, supported from NT 5 beta2, MIDL64, 5.1.194+
        /// </summary>
        FC_UINT3264 = 0xb9,


        /// <summary>
        /// Arrays of international characters
        /// </summary>
        FC_CSARRAY = 0xba,

        /// <summary>
        /// FC_CS_TAG
        /// </summary>
        FC_CS_TAG = 0xbb,


        /// <summary>
        /// Replacement for alignment in structure layout.
        /// </summary>
        FC_STRUCTPADN = 0xbc,


        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED7 = 0xbd,

        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED8 = 0xbe,

        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED9 = 0xbf,

        /// <summary>
        /// Unused.
        /// </summary>
        FC_UNUSED10 = 0xc0,


        /// <summary>
        /// FC_BUFFER_ALIGN
        /// </summary>
        FC_BUFFER_ALIGN = 0xc1,

        /// <summary>
        /// FC_INT128
        /// </summary>
        FC_INT128 = 0xc0,

        /// <summary>
        /// FC_UINT128
        /// </summary>
        FC_UINT128 = 0xc1,

        /// <summary>
        /// FC_FLOAT80
        /// </summary>
        FC_FLOAT80 = 0xc2,

        /// <summary>
        /// FC_FLOAT128
        /// </summary>
        FC_FLOAT128 = 0xc3,


        /// <summary>
        /// FC_END_OF_UNIVERSE
        /// </summary>
        FC_END_OF_UNIVERSE = 0xd8
    }
}
