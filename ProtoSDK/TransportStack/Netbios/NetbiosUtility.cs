// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// This class provides command methods for netbios operations.
    /// </summary>
    internal static class NetbiosUtility
    {
        #region Const values defined in <nb30.h>

        /// <summary>
        /// the absolute length of a net name. it is defined in nb30.h
        /// </summary>
        public const int NCBNAMSZ = 16;  

        /// <summary>
        /// the size in bytes of reserved field used by BIOS in WIN64. it is defined in nb30.h
        /// </summary>
        public const int NCBRSVSZ64 = 18;

        /// <summary>
        /// the size in bytes of reserved field used by BIOS in not WIN32. it is defined in nb30.h
        /// </summary>
        public const int NCBRSVSZ32 = 10;

        #endregion

        #region Const values defined by sdk

        /// <summary>
        /// the size in bytes of padding for win64 using 8bytes alignment.
        /// </summary>
        private const int Win64NcbPaddingSize = 4;

        /// <summary>
        /// the size in bytes of the ncb struct for win32.
        /// </summary>
        private const int Win32NcbStructSize = 64;

        /// <summary>
        /// the size in bytes of the ncb struct for win64.
        /// </summary>
        private const int Win64NcbStructSize = 96;

        #endregion

        #region Methods

        /// <summary>
        /// Use the size of IntPtr to detect whether the os is win64 at runtime. 
        /// </summary>
        /// <returns>true if win64, otherwise false.</returns>
        private static bool IsWin64()
        {
            return IntPtr.Size == sizeof(UInt64);
        }


        /// <summary>
        /// Convert a string to Netbios Name which is always an ASCII bytes array in size of NCBNAMSZ. 
        /// if the length of input name is less than NCBNAMSZ, the remaining bytes will be padded with ' '.
        /// if the length of input name is more than NCBNAMSZ, only the front NCBNAMSZ bytes will be used.
        /// </summary>
        /// <param name="name">the input Netbios name.</param>
        /// <returns>the standard Netbios Name which is always an ASCII bytes array in size of NCBNAMSZ.</returns>
        public static byte[] ToNetbiosName(string name)
        {
            byte[] netbiosName = new byte[NetbiosUtility.NCBNAMSZ];

            // Only the front NCBNAMSZ bytes will be used.
            int nameLen = Math.Min(name.Length, netbiosName.Length);
            Array.Copy(Encoding.ASCII.GetBytes(name), netbiosName, nameLen);

            // Pad the remaining bytes with ' '.
            for (int i = nameLen; i < netbiosName.Length; i++)
            {
                netbiosName[i] = (byte)' ';
            }

            return netbiosName;
        }


        /// <summary>
        /// Marshal NCB structure to native buffer
        /// </summary>
        /// <param name="ncb">The NCB structure</param>
        /// <param name="ncbSize">The native buffer size</param>
        /// <returns>The native buffer point</returns>
        public static IntPtr MarshalNcb(ref NCB ncb, out int ncbSize)
        {
            // size:
            if (IsWin64())
            {
                ncbSize = NetbiosUtility.Win64NcbStructSize;
            }
            else
            {
                ncbSize = NetbiosUtility.Win32NcbStructSize;
            }

            // the IntPtr used to marshal/unmarshal the NCB struct:
            IntPtr pNcb = Marshal.AllocHGlobal(ncbSize);

            // to bytes:
            byte[] ncbBytes = new byte[ncbSize];
            using (MemoryStream stream = new MemoryStream(ncbBytes, true))
            {
                stream.WriteByte((byte)ncb.ncb_command);
                stream.WriteByte((byte)ncb.ncb_retcode);
                stream.WriteByte(ncb.ncb_lsn);
                stream.WriteByte(ncb.ncb_num);

                if (ncb.ncb_padding0 != null)
                {
                    stream.Write(ncb.ncb_padding0, 0, ncb.ncb_padding0.Length);
                }
                stream.Write(BitConverter.GetBytes(ncb.ncb_buffer.ToInt64()), 0, IntPtr.Size);

                stream.Write(BitConverter.GetBytes(ncb.ncb_length), 0, 2);
                stream.Write(ncb.ncb_callname, 0, ncb.ncb_callname.Length);
                stream.Write(ncb.ncb_name, 0, ncb.ncb_name.Length);
                stream.WriteByte(ncb.ncb_rto);
                stream.WriteByte(ncb.ncb_sto);

                if (ncb.ncb_padding1 != null)
                {
                    stream.Write(ncb.ncb_padding1, 0, ncb.ncb_padding1.Length);
                }
                stream.Write(BitConverter.GetBytes(ncb.ncb_post.ToInt64()), 0, IntPtr.Size);

                stream.WriteByte(ncb.ncb_lana_num);
                stream.WriteByte(ncb.ncb_cmd_cplt);
                stream.Write(ncb.ncb_reserve, 0, ncb.ncb_reserve.Length);

                if (ncb.ncb_padding2 != null)
                {
                    stream.Write(ncb.ncb_padding2, 0, ncb.ncb_padding2.Length);
                }
                stream.Write(BitConverter.GetBytes(ncb.ncb_event.ToInt64()), 0, IntPtr.Size);

                Marshal.Copy(ncbBytes, 0, pNcb, ncbSize);
            }

            return pNcb;
        }


        /// <summary>
        /// Un marshal NCB structure from a native buffer
        /// </summary>
        /// <param name="ncbPtr">The native buffer point</param>
        /// <param name="ncbSize">The native buffer size</param>
        /// <returns>The un-marshaled NCB structure</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Throw when there is no enough data can be unmarshaled.
        /// </exception>
        public static NCB UnMarshalNcb(IntPtr ncbPtr, int ncbSize)
        {
            NCB ncb = new NCB();
            NetbiosUtility.InitNcb(ref ncb);
            byte[] ncbBytes = new byte[ncbSize];

            // to struct:
            Marshal.Copy(ncbPtr, ncbBytes, 0, ncbBytes.Length);
            using (MemoryStream stream = new MemoryStream(ncbBytes, false))
            {
                int readedBufferCount = 0;

                ncb.ncb_command = (byte)stream.ReadByte();
                ncb.ncb_retcode = (byte)stream.ReadByte();
                ncb.ncb_lsn = (byte)stream.ReadByte();
                ncb.ncb_num = (byte)stream.ReadByte();

                if (ncb.ncb_padding0 != null)
                {
                    stream.Read(ncb.ncb_padding0, 0, ncb.ncb_padding0.Length);
                }

                byte[] ncbBuffer = new byte[IntPtr.Size];
                readedBufferCount = stream.Read(ncbBuffer, 0, ncbBuffer.Length);

                if (readedBufferCount != ncbBuffer.Length)
                {
                    throw new InvalidOperationException("No enough data can be read");
                }

                if (!IsWin64())
                {
                    ncb.ncb_buffer = new IntPtr(BitConverter.ToInt32(ncbBuffer, 0));
                }
                else
                {
                    ncb.ncb_buffer = new IntPtr(BitConverter.ToInt64(ncbBuffer, 0));
                }

                byte[] ncbLength = new byte[sizeof(ushort)];

                readedBufferCount = stream.Read(ncbLength, 0, ncbLength.Length);
                if (readedBufferCount != ncbLength.Length)
                {
                    throw new InvalidOperationException("No enough data can be read");
                }

                ncb.ncb_length = (ushort)BitConverter.ToInt16(ncbLength, 0);

                readedBufferCount = stream.Read(ncb.ncb_callname, 0, ncb.ncb_callname.Length);

                if (readedBufferCount != ncb.ncb_callname.Length)
                {
                    throw new InvalidOperationException("No enough data can be read");
                }

                readedBufferCount = stream.Read(ncb.ncb_name, 0, ncb.ncb_name.Length);

                if (readedBufferCount != ncb.ncb_name.Length)
                {
                    throw new InvalidOperationException("No enough data can be read");
                }

                ncb.ncb_rto = (byte)stream.ReadByte();
                ncb.ncb_sto = (byte)stream.ReadByte();

                if (ncb.ncb_padding1 != null)
                {
                    stream.Read(ncb.ncb_padding1, 0, ncb.ncb_padding1.Length);
                }

                byte[] ncbPost = new byte[IntPtr.Size];
                stream.Read(ncbPost, 0, ncbPost.Length);

                if (!IsWin64())
                {
                    ncb.ncb_post = new IntPtr(BitConverter.ToInt32(ncbPost, 0));
                }
                else
                {
                    ncb.ncb_post = new IntPtr(BitConverter.ToInt64(ncbPost, 0));
                }

                ncb.ncb_lana_num = (byte)stream.ReadByte();
                ncb.ncb_cmd_cplt = (byte)stream.ReadByte();
                stream.Read(ncb.ncb_reserve, 0, ncb.ncb_reserve.Length);

                if (ncb.ncb_padding2 != null)
                {
                    stream.Read(ncb.ncb_padding2, 0, ncb.ncb_padding2.Length);
                }

                byte[] ncbEvent = new byte[IntPtr.Size];
                readedBufferCount = stream.Read(ncbEvent, 0, ncbEvent.Length);

                if (readedBufferCount != ncbEvent.Length)
                {
                    throw new InvalidOperationException("No enough data can be read");
                }

                if (!IsWin64())
                {
                    ncb.ncb_event = new IntPtr(BitConverter.ToInt32(ncbEvent, 0));
                }
                else
                {
                    ncb.ncb_event = new IntPtr(BitConverter.ToInt64(ncbEvent, 0));
                }
            }

            return ncb;
        }


        /// <summary>
        /// Free native resource the ncb used
        /// </summary>
        /// <param name="ncb">The network control block</param>
        public static void FreeNcbNativeFields(ref NCB ncb)
        {
            if (ncb.ncb_buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ncb.ncb_buffer);
                ncb.ncb_buffer = IntPtr.Zero;
            }

            if (ncb.ncb_event != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ncb.ncb_event);
                ncb.ncb_event = IntPtr.Zero;
            }

            if (ncb.ncb_post != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ncb.ncb_post);
                ncb.ncb_post = IntPtr.Zero;
            }
        }


        /// <summary>
        /// Initiate NCB structure
        /// </summary>
        /// <param name="ncb">The NCB structure</param>
        public static void InitNcb(ref NCB ncb)
        {
            ncb.ncb_callname = new byte[NetbiosUtility.NCBNAMSZ];
            ncb.ncb_name = new byte[NetbiosUtility.NCBNAMSZ];

            if (IsWin64())
            {
                ncb.ncb_padding0 = new byte[NetbiosUtility.Win64NcbPaddingSize];
                ncb.ncb_padding1 = new byte[NetbiosUtility.Win64NcbPaddingSize];
                ncb.ncb_padding2 = new byte[NetbiosUtility.Win64NcbPaddingSize];
                ncb.ncb_reserve = new byte[NetbiosUtility.NCBRSVSZ64];
            }
            else
            {
                ncb.ncb_reserve = new byte[NetbiosUtility.NCBRSVSZ32];
            }
        }

        #endregion
    }
}