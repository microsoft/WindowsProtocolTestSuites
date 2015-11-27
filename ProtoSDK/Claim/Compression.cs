// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{
    /// <summary>
    /// compression for claims
    /// </summary>
    public class ClaimsCompression
    {
        /// <summary>
        /// compress a buffer
        /// </summary>
        /// <param name="CompressionFormat"></param>
        /// <param name="pComressBufferSize"></param>
        /// <param name="pDecompressBufferSize"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), DllImport("ntdll.dll")]
        public static extern uint RtlGetCompressionWorkSpaceSize(uint CompressionFormat, ref int pComressBufferSize, ref int pDecompressBufferSize);

        /// <summary>
        /// decompress a buffer
        /// </summary>
        /// <param name="CompressionFormat"></param>
        /// <param name="pDecompressedBuffer"></param>
        /// <param name="DecompressedBufferSize"></param>
        /// <param name="pCompressedBuffer"></param>
        /// <param name="pCompressedBufferSize"></param>
        /// <param name="FinalDecompressedSize"></param>
        /// <param name="Workspace"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), DllImport("ntdll.dll")]
        public static extern uint RtlDecompressBufferEx(uint CompressionFormat, IntPtr pDecompressedBuffer, int DecompressedBufferSize, IntPtr pCompressedBuffer, int pCompressedBufferSize, ref int FinalDecompressedSize, IntPtr Workspace);

        /// <summary>
        /// decompress CLAIMS_SET
        /// </summary>
        /// <param name="option"></param>
        /// <param name="compressedData"></param>
        /// <param name="decompressedSize"></param>
        /// <param name="finalDecompressed"></param>
        /// <returns></returns>
        public static uint Decompress(CLAIMS_COMPRESSION_FORMAT option, byte[] compressedData, int decompressedSize, out byte[] finalDecompressed)
        {
            finalDecompressed = null;
            uint ret = 0;
            int compressWorkSpaceSize = 0, decompressWorkSpaceSize = 0;
            ret = RtlGetCompressionWorkSpaceSize((uint)option, ref compressWorkSpaceSize, ref decompressWorkSpaceSize);
            if (ret != 0)
                return ret;

            IntPtr workspace = Marshal.AllocHGlobal(decompressWorkSpaceSize);

            IntPtr compressedPtr = Marshal.AllocHGlobal(compressedData.Length);
            for (int i = 0; i < compressedData.Length; i++)
            {
                Marshal.WriteByte(compressedPtr, i, compressedData[i]);
            }
            IntPtr decompressedBuf = Marshal.AllocHGlobal(decompressedSize);
            int finalSize = 0;
            ret = RtlDecompressBufferEx((uint)option, decompressedBuf, decompressedSize, compressedPtr, compressedData.Length, ref finalSize, workspace);
            if (ret != 0)
                return ret;

            finalDecompressed = new byte[finalSize];
            for (int i = 0; i < finalSize; i++)
            {
                finalDecompressed[i] = Marshal.ReadByte(decompressedBuf, i);
            }

            return ret;
        }
    }
}
