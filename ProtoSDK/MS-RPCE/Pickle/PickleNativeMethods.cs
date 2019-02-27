// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    internal static class PickleNativeMethods
    {
        /// <summary>
        /// The MesDecodeBufferHandleCreate function creates a decoding handle 
        /// and initializes it for a (fixed) buffer style of serialization.
        /// </summary>
        /// <param name="pBuffer">Pointer to the buffer containing the data to decode.</param>
        /// <param name="BufferSize">Bytes of data to decode in the buffer.</param>
        /// <param name="pHandle">Pointer to the address to which the handle will be written.</param>
        /// <returns>RPC_S_OK (0) if the call succeeded. Otherwise failed.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int MesDecodeBufferHandleCreate(
                    IntPtr pBuffer,
                    uint BufferSize,
                    out IntPtr pHandle);


        /// <summary>
        /// The MesEncodeDynBufferHandleCreate function creates an encoding handle 
        /// and then initializes it for a dynamic buffer style of serialization.
        /// </summary>
        /// <param name="ppBuffer">Pointer to a pointer to the stub-supplied buffer 
        /// containing the encoding after serialization is complete.</param>
        /// <param name="pEncodedSize">Pointer to the size of the completed encoding. 
        /// The size will be written to the memory location pointed to by pEncodedSize 
        /// by subsequent encoding operations.</param>
        /// <param name="pHandle">Pointer to the address to which the handle will be written.</param>
        /// <returns>RPC_S_OK (0) if the call succeeded. Otherwise failed.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int MesEncodeDynBufferHandleCreate(
                    out IntPtr ppBuffer,
                    out uint pEncodedSize,
                    out IntPtr pHandle);


        /// <summary>
        /// The NdrMesTypeDecode2 function decodes NDR-encoded serialization buffer into 
        /// specified data type's unmanaged memory.
        /// </summary>
        /// <param name="Handle">Pointer to the address to which the handle will be written.
        /// The handle is relative to serialization buffer.</param>
        /// <param name="pPicklingInfo">Generated from IDL by midl.exe.</param>
        /// <param name="pStubDesc">Generated from IDL by midl.exe.</param>
        /// <param name="pFormatString">Generated from IDL by midl.exe.</param>
        /// <param name="pObject">Pointer to specified data type's unmanaged memory.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrMesTypeDecode2(
            IntPtr Handle,
            ref MIDL_TYPE_PICKLING_INFO pPicklingInfo,
            ref MIDL_STUB_DESC pStubDesc,
            IntPtr pFormatString,
            ref IntPtr pObject);


        /// <summary>
        /// The NdrMesTypeEncode2 function encodes specified data type's unmanaged memory into 
        /// NDR-encoded serialization buffer.
        /// </summary>
        /// <param name="Handle">Pointer to the address to which the handle will be written.
        /// The handle is relative to serialization buffer.</param>
        /// <param name="pPicklingInfo">Generated from IDL by midl.exe.</param>
        /// <param name="pStubDesc">Generated from IDL by midl.exe.</param>
        /// <param name="pFormatString">Generated from IDL by midl.exe.</param>
        /// <param name="pObject">Pointer to specified data type's unmanaged memory.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrMesTypeEncode2(
            IntPtr Handle,
            ref MIDL_TYPE_PICKLING_INFO pPicklingInfo,
            ref MIDL_STUB_DESC pStubDesc,
            [MarshalAs(UnmanagedType.LPArray)]
            byte[] pFormatString,
            ref IntPtr pObject);


        /// <summary>
        /// The MesHandleFree function frees the memory allocated by the serialization handle.
        /// </summary>
        /// <param name="Handle">Handle to be freed.</param>
        /// <returns>RPC_S_OK (0) if the call succeeded. Otherwise failed.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int MesHandleFree(IntPtr Handle);
    }
}
