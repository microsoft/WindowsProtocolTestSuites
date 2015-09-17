// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// this class provide static utilities 
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public static class CifsMessageUtils
    {
        #region constants
        //default constants
        internal const string DIALECT_PCLAN = "PCLAN1.0";
        internal const string DIALECT_PCNETWORK_PROGRAM = "PC NETWORK PROGRAM 1.0";
        internal const string DIALECT_NTLANMAN = "NT LM 0.12";
        internal const string NATIVE_OS = "Windows Vista (TM) Enterprise 6001 Service Pack 1";
        internal const string NATIVE_LANMAN = "Windows Vista (TM) Enterprise 6.0";
        internal const string NATIVE_FS = "NTFS";
        internal const string IPC_CONNECT_STRING = "IPC$";
        internal const string TREE_CONNECT_SERVICE = "?????";
        internal const uint SMB_PROTOCOL_IDENTIFIER = 0x424D53FF; // 0xFF, 'S', 'M', 'B'
        internal const uint SMB_PROTOCOL_IDENTIFIER_ASYNC = 0x424D5300; // 0x00, 'S', 'M', 'B'
        internal const ushort INVALID_MID = 0xFFFF;
        //SDK use 0FF to indicate current packet is a andx packet. very useful when updating context.
        //since andx packet does not has a header,they share the first leading packet. 
        internal const uint SMB_PROTOCOL_ANDXPACKET = 0xFF;

        /// <summary>
        /// Minimal smb packet length
        /// </summary>
        public const int PACKET_MINIMUM_LENGTH = 35;
        #endregion

        #region encode & decode

        /// <summary>
        /// To convert a struct to byte array.
        /// </summary>
        /// <typeparam name="T">The struct type.</typeparam>
        /// <param name="t">The struct to be converted.</param>
        /// <returns>
        /// Return the byte array converted from a struct.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The sum of offset and count is greater than the buffer length.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// offset or count is negative.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// offset or count is negative.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// fail in GetSize of MessageUtils.</exception>
        public static byte[] ToBytes<T>(T t)
        {
            using (MessageUtils utils = new MessageUtils(null))
            {
                int size = utils.GetSize(t);
                byte[] rawBytes = new byte[size];
                using (MemoryStream memoryStream = new MemoryStream(rawBytes))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        channel.Write<T>(t);
                        channel.EndWriteGroup();
                        int actualSize = (int)channel.Stream.Position;
                        byte[] actualBytes = new byte[actualSize];
                        Array.Copy(rawBytes, actualBytes, actualSize);
                        return actualBytes;
                    }
                }
            }
        }


        /// <summary>
        /// To convert a byte array to a struct.
        /// </summary>
        /// <typeparam name="T">The struct type.</typeparam>
        /// <param name="data">The byte array to be converted.</param>
        /// <returns>
        /// The struct converted from byte array.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The sum of offset and count is greater than the buffer length.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// offset or count is negative.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// offset or count is negative.</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T ToStuct<T>(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    return channel.Read<T>();
                }
            }
        }


        /// <summary>
        /// To get the size in bytes of a structure.
        /// </summary>
        /// <typeparam name="T">The struct type.</typeparam>
        /// <param name="t">The structure to be sized.</param>
        /// <returns>
        /// Return the size in bytes of the structure.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// fail in GetSize of MessageUtils.</exception>
        public static int GetSize<T>(T t) where T : struct
        {
            return TypeMarshal.GetBlockMemorySize(t);
        }

        #endregion

        #region convert array

        /// <summary>
        /// convert a primitive type array to a byte array.
        /// </summary>
        /// <param name="source">a primitive type array</param>
        /// <returns>a byte array</returns>
        public static byte[] ToBytesArray<T>(T[] source)
        {
            byte[] dest = null;

            if (source != null)
            {
                if (source.Length == 0)
                {
                    dest = new byte[0];
                }
                else
                {
                    dest = new byte[Marshal.SizeOf(typeof(T)) * source.Length];
                    Buffer.BlockCopy(source, 0, dest, 0, dest.Length);
                }
            }
            return dest;
        }


        /// <summary>
        /// convert a byte array to a primitive type array.
        /// </summary>
        /// <param name="source">a byte array</param>
        /// <returns>a primitive type array</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T[] ToTypeArray<T>(byte[] source)
        {
            T[] dest = null;

            if (source == null || source.Length == 0)
            {
                dest = new T[0];
            }
            else
            {
                int size = Marshal.SizeOf(typeof(T));
                dest = new T[(int)Math.Ceiling((double)source.Length / size)];
                Buffer.BlockCopy(source, 0, dest, 0, source.Length);
            }

            return dest;
        }


        /// <summary>
        /// To convert a string to the bytes of SMB_String.
        /// </summary>
        /// <param name="smbString">the string without null-terminator to be converted.</param>
        /// <param name="isUnicode">the encoding type. Unicode if true, otherwise ASCII.</param>
        /// <returns>the bytes in Unicode/ASCII formate with a NULL-terminated.</returns>
        public static byte[] ToSmbStringBytes(string smbString, bool isUnicode)
        {
            if (smbString == null)
            {
                throw new ArgumentNullException("smbString");
            }

            if (isUnicode)
            {
                return Encoding.Unicode.GetBytes(smbString + "\0");
            }
            else
            {
                return Encoding.ASCII.GetBytes(smbString + "\0");
            }
        }



        /// <summary>
        /// Get String from a buffer.
        /// </summary>
        /// <param name="bytes">a byte array</param>
        /// <param name="smbFlags2">The smbFlags2 of the SmbHeader to which this bytes belong to</param>
        /// <returns>the string</returns>
        public static string ToString(byte[] bytes, SmbFlags2 smbFlags2)
        {
            if (bytes == null)
            {
                return string.Empty;
            }

            if ((smbFlags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                return Encoding.Unicode.GetString(bytes, 0, bytes.Length - 2);
            }
            else
            {
                return Encoding.ASCII.GetString(bytes, 0, bytes.Length - 1);
            }
        }


        /// <summary>
        /// To convert the bytes in ASCII formate with a NUL-terminated to SMB_String.
        /// </summary>
        /// <param name="bytes">the bytes in ASCII formate with a NUL-terminated.</param>
        /// <param name="startIndex">the start index of the .</param>
        /// <param name="withFormat">there is 1 byte BufferFormat in the bytes or not.</param>
        /// <returns>the string to be converted.</returns>
        public static string ToSmbString(byte[] bytes, int startIndex, bool withFormat)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            int formatLen = 0;
            if (withFormat)
            {
                formatLen = 1;
            }

            int endIndex = Array.IndexOf<byte>(bytes, 0, startIndex);

            if (endIndex >= 0)
            {
                if (bytes.Length >= (startIndex + formatLen + 1))
                {
                    return Encoding.ASCII.GetString(bytes, startIndex + formatLen,
                        endIndex - (startIndex + formatLen));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("startIndex", "The startIndex should be inside the length of bytes.");
                }
            }
            else
            {
                throw new ArgumentException("The bytes of SmbString should include a null-terminator.", "bytes");
            }
        }

        #endregion

        #region get size

        /// <summary>
        /// Get the total size of a SmbEa array
        /// </summary>
        /// <param name="extendedAttributeList">a SmbEa array</param>
        /// <returns>the size</returns>
        internal static uint GetSmbEAListSize(SMB_FEA[] extendedAttributeList)
        {
            uint sizeOfListInBytes = 0;

            if (extendedAttributeList != null)
            {
                foreach (SMB_FEA smbEa in extendedAttributeList)
                {
                    sizeOfListInBytes += (uint)EA.SMB_EA_FIXED_SIZE;

                    if (smbEa.AttributeName != null)
                    {
                        sizeOfListInBytes += (uint)smbEa.AttributeName.Length;
                    }
                    if (smbEa.ValueName != null)
                    {
                        sizeOfListInBytes += (uint)smbEa.ValueName.Length;
                    }
                }
            }

            return sizeOfListInBytes;
        }


        /// <summary>
        /// Get the total size of a SmbQueryEa array
        /// </summary>
        /// <param name="extendedAttributeList">a SmbQueryEa array</param>
        /// <returns>the size</returns>
        internal static uint GetSmbQueryEAListSize(SMB_GEA[] extendedAttributeList)
        {
            uint sizeOfListInBytes = 0;

            if (extendedAttributeList != null)
            {
                foreach (SMB_GEA smbQueryEa in extendedAttributeList)
                {
                    sizeOfListInBytes += (uint)EA.SMB_QUERY_EA_FIXED_SIZE;

                    if (smbQueryEa.AttributeName != null)
                    {
                        sizeOfListInBytes += (uint)smbQueryEa.AttributeName.Length;
                    }
                }
            }

            return sizeOfListInBytes;
        }

        #endregion

        #region Read a null terminated string from channel

        /// <summary>
        /// Read a null terminated UNICODE or ASCII string from channel.
        /// </summary>
        /// <param name="channel">the channel from which the string will be read.</param>
        /// <param name="isUnicode">if true, the string formate is UNICODE, otherwise ASCII.</param>
        /// <returns>the binary string with null terminator.</returns>
        internal static byte[] ReadNullTerminatedString(Channel channel, bool isUnicode)
        {
            int byteCount = 0;
            while (true)
            {
                if (isUnicode)
                {
                    byte data0 = channel.Peek<byte>(byteCount++);
                    byte data1 = channel.Peek<byte>(byteCount++);

                    if (data0 == 0 && data1 == 0)
                    {
                        break;
                    }
                }
                else
                {
                    byte data = channel.Peek<byte>(byteCount++);

                    if (data == 0)
                    {
                        break;
                    }
                }
            }
            return channel.ReadBytes(byteCount);
        }

        #endregion

        #region create packet header

        /// <summary>
        /// a full smb header create function
        /// </summary>
        /// <param name="command">The operation code that this SMB is requesting or responding to.</param>
        /// <param name="pid">Caller's process ID</param>
        /// <param name="mid">This field SHOULD be the multiplex ID that is used to associate a 
        /// response with a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="tid">This field identifies the subdirectory (or tree) on the server that the
        /// client is accessing.</param>
        /// <param name="flags">The Flags field contains individual flags.</param>
        /// <param name="flags2">
        ///  The Flags2 field contains individual bit flags
        ///  that, depending on the negotiated SMB dialect, indicate
        ///  various client and server capabilities.
        /// </param>
        /// <returns>smb header</returns>
        public static SmbHeader CreateSmbHeader(
            SmbCommand command,
            uint pid,
            ushort mid,
            ushort uid,
            ushort tid,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbHeader header = new SmbHeader();

            // (0xFF, 'S', 'M', 'B')
            header.Protocol = SMB_PROTOCOL_IDENTIFIER;
            header.Status = 0;
            header.PidHigh = (ushort)((pid >> 16) & 0xffff);
            header.Reserved = 0x0;

            header.Command = command;
            header.Flags = flags;
            header.Flags2 = flags2;
            header.SecurityFeatures = 0;
            header.Tid = tid;
            header.PidLow = (ushort)(pid & 0xffff);
            header.Uid = uid;
            header.Mid = mid;

            return header;
        }


        /// <summary>
        /// a full smb header create function
        /// </summary>
        /// <param name="command">The operation code that this SMB is requesting or responding to.</param>
        /// <param name="mid">This field SHOULD be the multiplex ID that is used to associate a 
        /// response with a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="tid">This field identifies the subdirectory (or tree) on the server that the 
        /// client is accessing.</param>
        /// <param name="flags">it describes various features in effect for the message.</param>
        /// <param name="flags2">it represents various features in effect for the message.</param>
        /// <returns>smb header</returns>
        public static SmbHeader CreateSmbHeader(
            SmbCommand command,
            ushort mid,
            ushort uid,
            ushort tid,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            return CreateSmbHeader(command, 0, mid, uid, tid, flags, flags2);
        }


        /// <summary>
        /// a full smb header create function
        /// </summary>
        /// <param name="connection">the connection to which the packet belongs</param>
        /// <param name="request">the request packet</param>
        /// <returns>smb header</returns>
        /// <exception cref="KeyNotFoundException">The request is not pending in this connection.</exception>
        public static SmbHeader CreateSmbHeader(
            CifsServerPerConnection connection,
            SmbPacket request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.SmbHeader.Protocol == SMB_PROTOCOL_IDENTIFIER // 0xFF, 'S', 'M', 'B'
                && !connection.PendingRequestTable.Contains(request))
            {
                throw new KeyNotFoundException("The request is not pending in this connection.");
            }

            SmbHeader smbHeader = smbHeader = request.SmbHeader;
            smbHeader.Status = 0;
            smbHeader.SecurityFeatures = 0;
            smbHeader.Flags |= SmbFlags.SMB_FLAGS_REPLY;
            return smbHeader;
        }

        #endregion

        #region create response packet

        /// <summary>
        /// to new a Smb response packet in type of the Command in SmbHeader.
        /// </summary>
        /// <param name="request">the request of the response.</param>
        /// <param name="smbHeader">the SMB header of the packet.</param>
        /// <param name="channel">the channel started with SmbParameters.</param>
        /// <returns>
        /// the new response packet. 
        /// the null means that the utility don't know how to create the response.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        internal static SmbPacket CreateSmbResponsePacket(
            SmbPacket request,
            SmbHeader smbHeader,
            Channel channel)
        {
            SmbPacket smbPacket = null;

            switch (smbHeader.Command)
            {
                case SmbCommand.SMB_COM_CREATE_DIRECTORY: // 0x00,
                    smbPacket = new SmbCreateDirectoryResponsePacket();
                    break;

                case SmbCommand.SMB_COM_DELETE_DIRECTORY: // 0x01,
                    smbPacket = new SmbDeleteDirectoryResponsePacket();
                    break;

                case SmbCommand.SMB_COM_OPEN: // 0x02,
                    smbPacket = new SmbOpenResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CREATE: // 0x03,
                    smbPacket = new SmbCreateResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE: // 0x04,
                    smbPacket = new SmbCloseResponsePacket();
                    break;

                case SmbCommand.SMB_COM_FLUSH: // 0x05,
                    smbPacket = new SmbFlushResponsePacket();
                    break;

                case SmbCommand.SMB_COM_DELETE: // 0x06,
                    smbPacket = new SmbDeleteResponsePacket();
                    break;

                case SmbCommand.SMB_COM_RENAME: // 0x07,
                    smbPacket = new SmbRenameResponsePacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_INFORMATION: // 0x08,
                    smbPacket = new SmbQueryInformationResponsePacket();
                    break;

                case SmbCommand.SMB_COM_SET_INFORMATION: // 0x09,
                    smbPacket = new SmbSetInformationResponsePacket();
                    break;

                case SmbCommand.SMB_COM_READ: // 0x0A,
                    smbPacket = new SmbReadResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE: // 0x0B,
                    smbPacket = new SmbWriteResponsePacket();
                    break;

                case SmbCommand.SMB_COM_LOCK_byte_RANGE: // 0x0C,
                    smbPacket = new SmbLockByteRangeResponsePacket();
                    break;

                case SmbCommand.SMB_COM_UNLOCK_byte_RANGE: // 0x0D,
                    smbPacket = new SmbUnlockByteRangeResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CREATE_TEMPORARY: // 0x0E,
                    smbPacket = new SmbCreateTemporaryResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CREATE_NEW: // 0x0F,
                    smbPacket = new SmbCreateNewResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CHECK_DIRECTORY: // 0x10,
                    smbPacket = new SmbCheckDirectoryResponsePacket();
                    break;

                case SmbCommand.SMB_COM_PROCESS_EXIT: // 0x11,
                    smbPacket = new SmbProcessExitResponsePacket();
                    break;

                case SmbCommand.SMB_COM_SEEK: // 0x12,
                    smbPacket = new SmbSeekResponsePacket();
                    break;

                case SmbCommand.SMB_COM_LOCK_AND_READ: // 0x13,
                    smbPacket = new SmbLockAndReadResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_AND_UNLOCK: // 0x14,
                    smbPacket = new SmbWriteAndUnlockResponsePacket();
                    break;

                case SmbCommand.SMB_COM_READ_RAW: // 0x1A,
                    smbPacket = new SmbReadRawResponsePacket();
                    break;

                case SmbCommand.SMB_COM_READ_MPX: // 0x1B,
                    smbPacket = new SmbReadMpxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_READ_MPX_SECONDARY: // 0x1C,
                    smbPacket = new SmbReadMpxSecondaryResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_RAW: // 0x1D,
                    smbPacket = new SmbWriteRawInterimResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_MPX: // 0x1E,
                    smbPacket = new SmbWriteMpxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_MPX_SECONDARY: // 0x1F,
                    smbPacket = new SmbWriteMpxSecondaryResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_COMPLETE: // 0x20,
                    // This command was introduced in LAN Manager 1.0 dialect. This command is deprecated. 
                    // This commandis sent by the server in the final response of an SMB_COM_WRITE_RAW command. 
                    // See SMB_COM_WRITE_RAW for details.
                    smbPacket = new SmbWriteRawFinalResponsePacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_SERVER: // 0x21,
                    smbPacket = new SmbQueryServerResponsePacket();
                    break;

                case SmbCommand.SMB_COM_SET_INFORMATION2: // 0x22,
                    smbPacket = new SmbSetInformation2ResponsePacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_INFORMATION2: // 0x23,
                    smbPacket = new SmbQueryInformation2ResponsePacket();
                    break;

                case SmbCommand.SMB_COM_LOCKING_ANDX: // 0x24,
                    if (request != null)
                    {
                        smbPacket = new SmbLockingAndxResponsePacket();
                    }
                    else
                    {
                        //OpLock Break Notification sent by server
                        smbPacket = new SmbLockingAndxRequestPacket();
                    }
                    break;

                case SmbCommand.SMB_COM_TRANSACTION: // 0x25,
                    // The format of the SMB_COM_TRANSACTION Interim Server Response message MUST be an SMB header
                    // with an empty Parameter and Data section and the WordCount and ByteCount fields MUST be zero. 
                    if (smbHeader.Status == 0 && channel.Peek<byte>(0) == 0 && channel.Peek<ushort>(1) == 0)
                    {
                        smbPacket = new SmbTransactionInterimResponsePacket();
                    }
                    else
                    {
                        SmbTransactionRequestPacket transactionRequest = request as SmbTransactionRequestPacket;

                        if (transactionRequest != null)
                        {
                            smbPacket = CreateSmbTransResponsePacket(
                                (TransSubCommand)transactionRequest.SmbParameters.Setup[0]);
                        }
                    }
                    break;

                case SmbCommand.SMB_COM_IOCTL: // 0x27,
                    smbPacket = new SmbIoctlResponsePacket();
                    break;

                case SmbCommand.SMB_COM_IOCTL_SECONDARY: // 0x28,
                    smbPacket = new SmbIoctlSecondaryResponsePacket();
                    break;

                case SmbCommand.SMB_COM_COPY: // 0x29,
                    smbPacket = new SmbCopyResponsePacket();
                    break;

                case SmbCommand.SMB_COM_MOVE: // 0x2A,
                    smbPacket = new SmbMoveResponsePacket();
                    break;

                case SmbCommand.SMB_COM_ECHO: // 0x2B,
                    smbPacket = new SmbEchoResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_AND_CLOSE: // 0x2C,
                    smbPacket = new SmbWriteAndCloseResponsePacket();
                    break;

                case SmbCommand.SMB_COM_OPEN_ANDX: // 0x2D,
                    smbPacket = new SmbOpenAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_READ_ANDX: // 0x2E,
                    smbPacket = new SmbReadAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_ANDX: // 0x2F,
                    smbPacket = new SmbWriteAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_NEW_FILE_SIZE: // 0x30,
                    smbPacket = new SmbNewFileSizeResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE_AND_TREE_DISC: // 0x31,
                    smbPacket = new SmbCloseAndTreeDiscResponsePacket();
                    break;

                case SmbCommand.SMB_COM_TRANSACTION2: // 0x32,
                    // The format of the SMB_COM_TRANSACTION2 Interim Server Response message MUST be an SMB header
                    // with an empty Parameter and Data section and the WordCount and ByteCount fields MUST be zero. 
                    if (smbHeader.Status == 0 && channel.Peek<byte>(0) == 0 && channel.Peek<ushort>(1) == 0)
                    {
                        smbPacket = new SmbTransaction2InterimResponsePacket();
                    }
                    else
                    {
                        SmbTransaction2RequestPacket transaction2Request = request as SmbTransaction2RequestPacket;

                        if (transaction2Request != null)
                        {
                            smbPacket = CreateSmbTrans2ResponsePacket(transaction2Request);
                        }
                    }
                    break;

                case SmbCommand.SMB_COM_FIND_CLOSE2: // 0x34,
                    smbPacket = new SmbFindClose2ResponsePacket();
                    break;

                case SmbCommand.SMB_COM_FIND_NOTIFY_CLOSE: // 0x35,
                    smbPacket = new SmbFindNotifyCloseResponsePacket();
                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT: // 0x70,
                    smbPacket = new SmbTreeConnectResponsePacket();
                    break;

                case SmbCommand.SMB_COM_TREE_DISCONNECT: // 0x71,
                    smbPacket = new SmbTreeDisconnectResponsePacket();
                    break;

                case SmbCommand.SMB_COM_NEGOTIATE: // 0x72,
                    smbPacket = new SmbNegotiateResponsePacket();
                    break;

                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX: // 0x73,
                    smbPacket = new SmbSessionSetupAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_LOGOFF_ANDX: // 0x74,
                    smbPacket = new SmbLogoffAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX: // 0x75,
                    smbPacket = new SmbTreeConnectAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_SECURITY_PACKAGE_ANDX: // 0x7E,
                    smbPacket = new SmbSecurityPackageAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_INFORMATION_DISK: // 0x80,
                    smbPacket = new SmbQueryInformationDiskResponsePacket();
                    break;

                case SmbCommand.SMB_COM_SEARCH: // 0x81,
                    smbPacket = new SmbSearchResponsePacket();
                    break;

                case SmbCommand.SMB_COM_FIND: // 0x82,
                    smbPacket = new SmbFindResponsePacket();
                    break;

                case SmbCommand.SMB_COM_FIND_UNIQUE: // 0x83,
                    smbPacket = new SmbFindUniqueResponsePacket();
                    break;

                case SmbCommand.SMB_COM_FIND_CLOSE: // 0x84,
                    smbPacket = new SmbFindCloseResponsePacket();
                    break;

                case SmbCommand.SMB_COM_NT_TRANSACT: // 0xA0,
                    // The format of the SMB_COM_NT_TRANSACTION Interim Server Response message MUST be an SMB header
                    // with an empty Parameter and Data section and the WordCount and ByteCount fields MUST be zero. 
                    if (smbHeader.Status == 0 && channel.Peek<byte>(0) == 0 && channel.Peek<ushort>(1) == 0)
                    {
                        smbPacket = new SmbNtTransactInterimResponsePacket();
                    }
                    else
                    {
                        SmbNtTransactRequestPacket ntTransactRequest = request as SmbNtTransactRequestPacket;

                        if (ntTransactRequest != null)
                        {
                            smbPacket = CreateSmbNtTransResponsePacket(
                                (NtTransSubCommand)ntTransactRequest.SmbParameters.Function);
                        }
                    }
                    break;

                case SmbCommand.SMB_COM_NT_CREATE_ANDX: // 0xA2,
                    smbPacket = new SmbNtCreateAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_NT_RENAME: // 0xA5,
                    smbPacket = new SmbNtRenameResponsePacket();
                    break;

                case SmbCommand.SMB_COM_OPEN_PRINT_FILE: // 0xC0,
                    smbPacket = new SmbOpenPrintFileResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_PRINT_FILE: // 0xC1,
                    smbPacket = new SmbWritePrintFileResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE_PRINT_FILE: // 0xC2,
                    smbPacket = new SmbClosePrintFileResponsePacket();
                    break;

                case SmbCommand.SMB_COM_GET_PRINT_QUEUE: // 0xC3,
                    smbPacket = new SmbGetPrintQueueResponsePacket();
                    break;

                case SmbCommand.SMB_COM_READ_BULK: // 0xD8,
                    smbPacket = new SmbReadBulkResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_BULK: // 0xD9,
                    smbPacket = new SmbWriteBulkResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_BULK_DATA: // 0xDA,
                    smbPacket = new SmbWriteBulkDataResponsePacket();
                    break;

                case SmbCommand.SMB_COM_INVALID: // 0xFE,
                    smbPacket = new SmbInvalidResponsePacket();
                    break;

                case SmbCommand.SMB_COM_NO_ANDX_COMMAND: // 0xFF,
                    smbPacket = new SmbNoAndxCommandResponsePacket();
                    break;
            }
            if (smbPacket != null)
            {
                smbPacket.SmbHeader = smbHeader;
            }
            return smbPacket;
        }

        #endregion

        #region create request packet

        /// <summary>
        /// create request packet in type of the command of smbheader.
        /// </summary>
        /// <param name="smbHeader">the header of smb packet</param>
        /// <param name="channel">the channel to access bytes</param>
        /// <returns>the target packet</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        internal static SmbPacket CreateSmbRequestPacket(SmbHeader smbHeader, Channel channel)
        {
            SmbPacket smbPacket = null;

            switch (smbHeader.Command)
            {
                #region Smb Com command
                case SmbCommand.SMB_COM_CREATE_DIRECTORY: // 0x00,
                    smbPacket = new SmbCreateDirectoryRequestPacket();
                    break;

                case SmbCommand.SMB_COM_DELETE_DIRECTORY: // 0x01,
                    smbPacket = new SmbDeleteDirectoryRequestPacket();
                    break;

                case SmbCommand.SMB_COM_OPEN: // 0x02,
                    smbPacket = new SmbOpenRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CREATE: // 0x03,
                    smbPacket = new SmbCreateRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE: // 0x04,
                    smbPacket = new SmbCloseRequestPacket();
                    break;

                case SmbCommand.SMB_COM_FLUSH: // 0x05,
                    smbPacket = new SmbFlushRequestPacket();
                    break;

                case SmbCommand.SMB_COM_DELETE: // 0x06,
                    smbPacket = new SmbDeleteRequestPacket();
                    break;

                case SmbCommand.SMB_COM_RENAME: // 0x07,
                    smbPacket = new SmbRenameRequestPacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_INFORMATION: // 0x08,
                    smbPacket = new SmbQueryInformationRequestPacket();
                    break;

                case SmbCommand.SMB_COM_SET_INFORMATION: // 0x09,
                    smbPacket = new SmbSetInformationRequestPacket();
                    break;

                case SmbCommand.SMB_COM_READ: // 0x0A,
                    smbPacket = new SmbReadRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE: // 0x0B,
                    smbPacket = new SmbWriteRequestPacket();
                    break;

                case SmbCommand.SMB_COM_LOCK_byte_RANGE: // 0x0C,
                    smbPacket = new SmbLockByteRangeRequestPacket();
                    break;

                case SmbCommand.SMB_COM_UNLOCK_byte_RANGE: // 0x0D,
                    smbPacket = new SmbUnlockByteRangeRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CREATE_TEMPORARY: // 0x0E,
                    smbPacket = new SmbCreateTemporaryRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CREATE_NEW: // 0x0F,
                    smbPacket = new SmbCreateNewRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CHECK_DIRECTORY: // 0x10,
                    smbPacket = new SmbCheckDirectoryRequestPacket();
                    break;

                case SmbCommand.SMB_COM_PROCESS_EXIT: // 0x11,
                    smbPacket = new SmbProcessExitRequestPacket();
                    break;

                case SmbCommand.SMB_COM_SEEK: // 0x12,
                    smbPacket = new SmbSeekRequestPacket();
                    break;

                case SmbCommand.SMB_COM_LOCK_AND_READ: // 0x13,
                    smbPacket = new SmbLockAndReadRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_AND_UNLOCK: // 0x14,
                    smbPacket = new SmbWriteAndUnlockRequestPacket();
                    break;

                case SmbCommand.SMB_COM_READ_RAW: // 0x1A,
                    smbPacket = new SmbReadRawRequestPacket();
                    break;

                case SmbCommand.SMB_COM_READ_MPX: // 0x1B,
                    smbPacket = new SmbReadMpxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_READ_MPX_SECONDARY: // 0x1C,
                    smbPacket = new SmbReadMpxSecondaryRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_RAW: // 0x1D,
                    smbPacket = new SmbWriteRawRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_MPX: // 0x1E,
                    smbPacket = new SmbWriteMpxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_MPX_SECONDARY: // 0x1F,
                    smbPacket = new SmbWriteMpxSecondaryRequestPacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_SERVER: // 0x21,
                    smbPacket = new SmbQueryServerRequestPacket();
                    break;

                case SmbCommand.SMB_COM_SET_INFORMATION2: // 0x22,
                    smbPacket = new SmbSetInformation2RequestPacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_INFORMATION2: // 0x23,
                    smbPacket = new SmbQueryInformation2RequestPacket();
                    break;

                case SmbCommand.SMB_COM_LOCKING_ANDX: // 0x24,
                    smbPacket = new SmbLockingAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_IOCTL: // 0x27,
                    smbPacket = new SmbIoctlRequestPacket();
                    break;

                case SmbCommand.SMB_COM_IOCTL_SECONDARY: // 0x28,
                    smbPacket = new SmbIoctlSecondaryRequestPacket();
                    break;

                case SmbCommand.SMB_COM_COPY: // 0x29,
                    smbPacket = new SmbCopyRequestPacket();
                    break;

                case SmbCommand.SMB_COM_MOVE: // 0x2A,
                    smbPacket = new SmbMoveRequestPacket();
                    break;

                case SmbCommand.SMB_COM_ECHO: // 0x2B,
                    smbPacket = new SmbEchoRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_AND_CLOSE: // 0x2C,
                    smbPacket = new SmbWriteAndCloseRequestPacket();
                    break;

                case SmbCommand.SMB_COM_OPEN_ANDX: // 0x2D,
                    smbPacket = new SmbOpenAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_READ_ANDX: // 0x2E,
                    smbPacket = new SmbReadAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_ANDX: // 0x2F,
                    smbPacket = new SmbWriteAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_NEW_FILE_SIZE: // 0x30,
                    smbPacket = new SmbNewFileSizeRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE_AND_TREE_DISC: // 0x31,
                    smbPacket = new SmbCloseAndTreeDiscRequestPacket();
                    break;

                case SmbCommand.SMB_COM_FIND_CLOSE2: // 0x34,
                    smbPacket = new SmbFindClose2RequestPacket();
                    break;

                case SmbCommand.SMB_COM_FIND_NOTIFY_CLOSE: // 0x35,
                    smbPacket = new SmbFindNotifyCloseRequestPacket();
                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT: // 0x70,
                    smbPacket = new SmbTreeConnectRequestPacket();
                    break;

                case SmbCommand.SMB_COM_TREE_DISCONNECT: // 0x71,
                    smbPacket = new SmbTreeDisconnectRequestPacket();
                    break;

                case SmbCommand.SMB_COM_NEGOTIATE: // 0x72,
                    smbPacket = new SmbNegotiateRequestPacket();
                    break;

                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX: // 0x73,
                    smbPacket = new SmbSessionSetupAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_LOGOFF_ANDX: // 0x74,
                    smbPacket = new SmbLogoffAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX: // 0x75,
                    smbPacket = new SmbTreeConnectAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_SECURITY_PACKAGE_ANDX: // 0x7E,
                    smbPacket = new SmbSecurityPackageAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_QUERY_INFORMATION_DISK: // 0x80,
                    smbPacket = new SmbQueryInformationDiskRequestPacket();
                    break;

                case SmbCommand.SMB_COM_SEARCH: // 0x81,
                    smbPacket = new SmbSearchRequestPacket();
                    break;

                case SmbCommand.SMB_COM_FIND: // 0x82,
                    smbPacket = new SmbFindRequestPacket();
                    break;

                case SmbCommand.SMB_COM_FIND_UNIQUE: // 0x83,
                    smbPacket = new SmbFindUniqueRequestPacket();
                    break;

                case SmbCommand.SMB_COM_FIND_CLOSE: // 0x84,
                    smbPacket = new SmbFindCloseRequestPacket();
                    break;

                case SmbCommand.SMB_COM_NT_CREATE_ANDX: // 0xA2,
                    smbPacket = new SmbNtCreateAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_NT_RENAME: // 0xA5,
                    smbPacket = new SmbNtRenameRequestPacket();
                    break;

                case SmbCommand.SMB_COM_OPEN_PRINT_FILE: // 0xC0,
                    smbPacket = new SmbOpenPrintFileRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_PRINT_FILE: // 0xC1,
                    smbPacket = new SmbWritePrintFileRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE_PRINT_FILE: // 0xC2,
                    smbPacket = new SmbClosePrintFileRequestPacket();
                    break;

                case SmbCommand.SMB_COM_GET_PRINT_QUEUE: // 0xC3,
                    smbPacket = new SmbGetPrintQueueRequestPacket();
                    break;

                case SmbCommand.SMB_COM_READ_BULK: // 0xD8,
                    smbPacket = new SmbReadBulkRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_BULK: // 0xD9,
                    smbPacket = new SmbWriteBulkRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_BULK_DATA: // 0xDA,
                    smbPacket = new SmbWriteBulkDataRequestPacket();
                    break;

                case SmbCommand.SMB_COM_INVALID: // 0xFE,
                    smbPacket = new SmbInvalidRequestPacket();
                    break;

                case SmbCommand.SMB_COM_NO_ANDX_COMMAND: // 0xFF,
                    smbPacket = new SmbNoAndxCommandRequestPacket();
                    break;

                #endregion

                #region Trans NtTrans Tran2
                case SmbCommand.SMB_COM_TRANSACTION:
                    SMB_COM_TRANSACTION_Request_SMB_Parameters transaction =
                        channel.Peek<SMB_COM_TRANSACTION_Request_SMB_Parameters>(0);
                    if (transaction.SetupCount == 0)
                    {
                        smbPacket = new SmbTransRapRequestPacket();
                    }
                    else
                    {
                        smbPacket = CreateTransactionPacket(
                            transaction.SetupCount, (TransSubCommand)transaction.Setup[0]);
                    }
                    break;

                case SmbCommand.SMB_COM_TRANSACTION2:
                    SMB_COM_TRANSACTION2_Request_SMB_Parameters transaction2 =
                        channel.Peek<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(0);
                    smbPacket = CreateTrans2Packet((Trans2SubCommand)transaction2.Setup[0]);
                    break;

                case SmbCommand.SMB_COM_NT_TRANSACT:
                    SMB_COM_NT_TRANSACT_Request_SMB_Parameters ntTransactoin =
                        channel.Peek<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(0);
                    smbPacket = CreateNtTransPacket(ntTransactoin.Function);
                    break;

                #endregion
            }

            return smbPacket;
        }


        /// <summary>
        /// create the nt transaction packets
        /// </summary>
        /// <param name="command">the command of nt transaction</param>
        /// <returns>the target nt transaction packet</returns>
        internal static SmbPacket CreateNtTransPacket(NtTransSubCommand command)
        {
            SmbPacket smbPacket = null;
            switch (command)
            {
                case NtTransSubCommand.NT_TRANSACT_CREATE:
                    smbPacket = new SmbNtTransactCreateRequestPacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_RENAME:
                    smbPacket = new SmbNtTransactRenameRequestPacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_IOCTL:
                    smbPacket = new SmbNtTransactIoctlRequestPacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_NOTIFY_CHANGE:
                    smbPacket = new SmbNtTransactNotifyChangeRequestPacket();
                    break;
                case NtTransSubCommand.NT_TRANSACT_QUERY_SECURITY_DESC:
                    smbPacket = new SmbNtTransactQuerySecurityDescRequestPacket();
                    break;
                case NtTransSubCommand.NT_TRANSACT_SET_SECURITY_DESC:
                    smbPacket = new SmbNtTransactSetSecurityDescRequestPacket();
                    break;
            }

            return smbPacket;
        }


        /// <summary>
        /// create the transaction2 packet.
        /// </summary>
        /// <param name="command">the command of transaction2 packet.</param>
        /// <returns>the target transaction2 packet</returns>
        internal static SmbPacket CreateTrans2Packet(Trans2SubCommand command)
        {
            SmbPacket smbPacket = null;
            switch ((Trans2SubCommand)command)
            {
                case Trans2SubCommand.TRANS2_OPEN2:
                    smbPacket = new SmbTrans2Open2RequestPacket();
                    break;
                case Trans2SubCommand.TRANS2_FIND_FIRST2:
                    smbPacket = new SmbTrans2FindFirst2RequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_NEXT2:
                    smbPacket = new SmbTrans2FindNext2RequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_FS_INFORMATION:
                    smbPacket = new SmbTrans2QueryFsInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_SET_FS_INFORMATION:
                    smbPacket = new SmbTrans2SetFsInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_PATH_INFORMATION:
                    smbPacket = new SmbTrans2QueryPathInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_SET_PATH_INFORMATION:
                    smbPacket = new SmbTrans2SetPathInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_FILE_INFORMATION:
                    smbPacket = new SmbTrans2QueryFileInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_SET_FILE_INFORMATION:
                    smbPacket = new SmbTrans2SetFileInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_FSCTL:
                    smbPacket = new SmbTrans2FsctlRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_IOCTL2:
                    smbPacket = new SmbTrans2Ioctl2RequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_NOTIFY_FIRST:
                    smbPacket = new SmbTrans2FindNotifyFirstRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_NOTIFY_NEXT:
                    smbPacket = new SmbTrans2FindNotifyNextRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_CREATE_DIRECTORY:
                    smbPacket = new SmbTrans2CreateDirectoryRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_SESSION_SETUP:
                    smbPacket = new SmbTrans2SessionSetupRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_GET_DFS_REFERRAL:
                    smbPacket = new SmbTrans2GetDfsReferalRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_REPORT_DFS_INCONSISTENCY:
                    smbPacket = new SmbTrans2ReportDfsInconsistencyRequestPacket();
                    break;
            }
            return smbPacket;
        }


        /// <summary>
        /// create the transaction packet.
        /// </summary>
        /// <param name="setupCount">the count of setup</param>
        /// <param name="command">the command of transaction packet</param>
        /// <returns>the target transaction packet</returns>
        internal static SmbPacket CreateTransactionPacket(byte setupCount, TransSubCommand command)
        {
            if (setupCount == 0)
            {
                return new SmbTransRapRequestPacket();
            }

            SmbPacket smbPacket = null;
            switch ((TransSubCommand)command)
            {
                case TransSubCommand.TRANS_SET_NMPIPE_STATE:
                    smbPacket = new SmbTransSetNmpipeStateRequestPacket();
                    break;

                case TransSubCommand.TRANS_RAW_READ_NMPIPE:
                    smbPacket = new SmbTransRawReadNmpipeRequestPacket();
                    break;

                case TransSubCommand.TRANS_QUERY_NMPIPE_STATE:
                    smbPacket = new SmbTransQueryNmpipeStateRequestPacket();
                    break;

                case TransSubCommand.TRANS_QUERY_NMPIPE_INFO:
                    smbPacket = new SmbTransQueryNmpipeInfoRequestPacket();
                    break;

                case TransSubCommand.TRANS_PEEK_NMPIPE:
                    smbPacket = new SmbTransPeekNmpipeRequestPacket();
                    break;

                case TransSubCommand.TRANS_TRANSACT_NMPIPE:
                    smbPacket = new SmbTransTransactNmpipeRequestPacket();
                    break;

                case TransSubCommand.TRANS_RAW_WRITE_NMPIPE:
                    smbPacket = new SmbTransRawWriteNmpipeRequestPacket();
                    break;

                case TransSubCommand.TRANS_READ_NMPIPE:
                    smbPacket = new SmbTransReadNmpipeRequestPacket();
                    break;

                case TransSubCommand.TRANS_WRITE_NMPIPE:
                    smbPacket = new SmbTransWriteNmpipeRequestPacket();
                    break;

                case TransSubCommand.TRANS_WAIT_NMPIPE:
                    smbPacket = new SmbTransWaitNmpipeRequestPacket();
                    break;

                case TransSubCommand.TRANS_CALL_NMPIPE:
                    smbPacket = new SmbTransCallNmpipeRequestPacket();
                    break;
            }
            return smbPacket;
        }


        /// <summary>
        /// to new a Smb sub trans response packet in type of the SubCommand in SmbParameters.
        /// </summary>
        /// <param name="subCommand">the TransSubCommand of the Trans response packet.</param>
        /// <returns>
        /// the new response packet. 
        /// the null means that the utility don't know how to create the response.
        /// </returns>
        private static SmbTransactionSuccessResponsePacket CreateSmbTransResponsePacket(
            TransSubCommand subCommand)
        {
            SmbTransactionSuccessResponsePacket response = null;
            switch (subCommand)
            {
                case TransSubCommand.TRANS_SET_NMPIPE_STATE: // 0x0001,
                    response = new SmbTransSetNmpipeStateSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_RAW_READ_NMPIPE: // 0x0011,
                    response = new SmbTransRawReadNmpipeSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_QUERY_NMPIPE_STATE: // 0x0021,
                    response = new SmbTransQueryNmpipeStateSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_QUERY_NMPIPE_INFO: // 0x0022,
                    response = new SmbTransQueryNmpipeInfoSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_PEEK_NMPIPE: // 0x0023,
                    response = new SmbTransPeekNmpipeSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_TRANSACT_NMPIPE: // 0x0026,
                    response = new SmbTransTransactNmpipeSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_RAW_WRITE_NMPIPE: // 0x0031,
                    response = new SmbTransRawWriteNmpipeSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_READ_NMPIPE: // 0x0036,
                    response = new SmbTransReadNmpipeSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_WRITE_NMPIPE: // 0x0037,
                    response = new SmbTransWriteNmpipeSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_WAIT_NMPIPE: // 0x0053,
                    response = new SmbTransWaitNmpipeSuccessResponsePacket();
                    break;

                case TransSubCommand.TRANS_CALL_NMPIPE: // 0x0054,
                    response = new SmbTransCallNmpipeSuccessResponsePacket();
                    break;

                default:
                    break;
            }
            return response;
        }


        /// <summary>
        /// to new a Smb sub transs response packet in type of the SubCommand in SmbParameters.
        /// </summary>
        /// <param name="smbTransaction2Request">the request of the Trans2 response packet.</param>
        /// <returns>
        /// the new response packet. 
        /// the null means that the utility don't know how to create the response.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        private static SmbTransaction2FinalResponsePacket CreateSmbTrans2ResponsePacket(
            SmbTransaction2RequestPacket smbTransaction2Request)
        {
            SmbTransaction2FinalResponsePacket response = null;
            switch ((Trans2SubCommand)smbTransaction2Request.SmbParameters.Setup[0])
            {
                case Trans2SubCommand.TRANS2_OPEN2: // 0x00,
                    response = new SmbTrans2Open2FinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_FIRST2: // 0x0001,
                    SmbTrans2FindFirst2RequestPacket first2Request =
                        smbTransaction2Request as SmbTrans2FindFirst2RequestPacket;
                    if (first2Request != null)
                    {
                        response = new SmbTrans2FindFirst2FinalResponsePacket(first2Request.Trans2Parameters.InformationLevel,
                            (first2Request.Trans2Parameters.Flags & Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS)
                            == Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS);
                    }
                    break;

                case Trans2SubCommand.TRANS2_FIND_NEXT2: // 0x0002,
                    SmbTrans2FindNext2RequestPacket next2Request =
                        smbTransaction2Request as SmbTrans2FindNext2RequestPacket;
                    if (next2Request != null)
                    {
                        response = new SmbTrans2FindNext2FinalResponsePacket(next2Request.Trans2Parameters.InformationLevel,
                            (next2Request.Trans2Parameters.Flags & Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS)
                            == Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS);
                    }
                    break;

                case Trans2SubCommand.TRANS2_QUERY_FS_INFORMATION: // 0x0003,
                    SmbTrans2QueryFsInformationRequestPacket queryFsRequest =
                        smbTransaction2Request as SmbTrans2QueryFsInformationRequestPacket;
                    if (queryFsRequest != null)
                    {
                        response = new SmbTrans2QueryFsInformationFinalResponsePacket(
                            queryFsRequest.Trans2Parameters.InformationLevel);
                    }
                    break;

                case Trans2SubCommand.TRANS2_SET_FS_INFORMATION: // 0x0004,
                    response = new SmbTrans2SetFsInformationFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_PATH_INFORMATION: // 0x0005,
                    SmbTrans2QueryPathInformationRequestPacket queryPathRequest =
                        smbTransaction2Request as SmbTrans2QueryPathInformationRequestPacket;
                    if (queryPathRequest != null)
                    {
                        response = new SmbTrans2QueryPathInformationFinalResponsePacket(
                            queryPathRequest.Trans2Parameters.InformationLevel);
                    }
                    break;

                case Trans2SubCommand.TRANS2_SET_PATH_INFORMATION: // 0x0006,
                    response = new SmbTrans2SetPathInformationFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_FILE_INFORMATION: // 0x0007,
                    SmbTrans2QueryFileInformationRequestPacket queryFileRequest =
                        smbTransaction2Request as SmbTrans2QueryFileInformationRequestPacket;
                    if (queryFileRequest != null)
                    {
                        response = new SmbTrans2QueryFileInformationFinalResponsePacket(
                            queryFileRequest.Trans2Parameters.InformationLevel);
                    }
                    break;

                case Trans2SubCommand.TRANS2_SET_FILE_INFORMATION: // 0x0008,
                    response = new SmbTrans2SetFileInformationFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_FSCTL: // 0x0009,
                    response = new SmbTrans2FsctlFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_IOCTL2: // 0x000a,
                    response = new SmbTrans2Ioctl2FinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_NOTIFY_FIRST: // 0x000b,
                    response = new SmbTrans2FindNotifyFirstFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_NOTIFY_NEXT: // 0x000c,
                    response = new SmbTrans2FindNotifyNextFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_CREATE_DIRECTORY: // 0x000d,
                    response = new SmbTrans2CreateDirectoryFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_SESSION_SETUP: // 0x000e,
                    response = new SmbTrans2SessionSetupFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_GET_DFS_REFERRAL: // 0x0010,
                    response = new SmbTrans2GetDfsReferalFinalResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_REPORT_DFS_INCONSISTENCY: // 0x0011,
                    response = new SmbTrans2ReportDfsInconsistencyFinalResponsePacket();
                    break;

                default:
                    break;
            }
            return response;
        }


        /// <summary>
        /// to new a Smb sub transs response packet in type of the SubCommand in SmbParameters.
        /// </summary>
        /// <param name="subCommand">the NtTransSubCommand of the NtTrans response packet.</param>
        /// <returns>
        /// the new response packet. 
        /// the null means that the utility don't know how to create the response.
        /// </returns>
        private static SmbNtTransactSuccessResponsePacket CreateSmbNtTransResponsePacket(
            NtTransSubCommand subCommand)
        {
            SmbNtTransactSuccessResponsePacket response = null;
            switch (subCommand)
            {
                case NtTransSubCommand.NT_TRANSACT_CREATE: // 0x0001,
                    response = new SmbNtTransactCreateResponsePacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_IOCTL: // 0x0002,
                    response = new SmbNtTransactIoctlResponsePacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_SET_SECURITY_DESC: // 0x0003,
                    response = new SmbNtTransactSetSecurityDescResponsePacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_NOTIFY_CHANGE: // 0x0004,
                    response = new SmbNtTransactNotifyChangeResponsePacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_RENAME: // 0x0005,
                    response = new SmbNtTransactRenameResponsePacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_QUERY_SECURITY_DESC: // 0x0006,
                    response = new SmbNtTransactQuerySecurityDescResponsePacket();
                    break;

                default:
                    break;
            }
            return response;
        }

        #endregion

        #region utilities

        /// <summary>
        /// it is used to compute a signature for a message block against a key. 
        /// </summary>
        /// <param name="message">the raw packet bytes.</param>
        /// <param name="sessionKey">the session key.</param>
        /// <returns>the signature.</returns>
        /// <exception cref="System.ArgumentNullException">the message or sessionKey is null.</exception>
        public static byte[] CreateSignature(byte[] message, byte[] sessionKey)
        {
            return CreateSignature(message, sessionKey, new byte[0]);
        }


        /// <summary>
        /// it is used to compute a signature for a message block against a key. 
        /// </summary>
        /// <param name="message">the raw packet bytes.</param>
        /// <param name="sessionKey">the session key.</param>
        /// <param name="challengeResponse">the challenge response.</param>
        /// <returns>the signature.</returns>
        /// <exception cref="System.ArgumentNullException">the message or sessionKey or challengeResponse is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]        
        public static byte[] CreateSignature(byte[] message, byte[] sessionKey, byte[] challengeResponse)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (challengeResponse == null)
            {
                throw new ArgumentNullException("sessionKey");
            }

            byte[] data = new byte[message.Length + sessionKey.Length + challengeResponse.Length];
            Array.Copy(sessionKey, data, sessionKey.Length);
            Array.Copy(challengeResponse, 0, data, sessionKey.Length, challengeResponse.Length);
            Array.Copy(message, 0, data, sessionKey.Length + challengeResponse.Length, message.Length);

            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(data);

            return result;
        }


        /// <summary>
        /// PlainTextAuthenticate Method
        /// </summary>
        /// <param name="request">the SessionSetup request.</param>
        /// <param name="accountCredentials">All the available users</param>
        /// <returns>the account name</returns>
        public static string PlainTextAuthenticate(
            SmbSessionSetupAndxRequestPacket request,
             Collection<AccountCredential> accountCredentials)
        {
            string account = string.Empty;
            if (request != null && accountCredentials != null)
            {
                foreach (AccountCredential accountCredential in accountCredentials)
                {
                    string actualAccount = ToString(request.SmbData.AccountName, request.SmbHeader.Flags2);
                    if (actualAccount != accountCredential.AccountName)
                    {
                       continue;
                    }

                    string actualPassword = string.Empty;
                    if (request.SmbParameters.UnicodePasswordLen != 0)
                    {
                        actualPassword = Encoding.Unicode.GetString(request.SmbData.UnicodePassword);
                    }
                    if (request.SmbParameters.OEMPasswordLen != 0)
                    {
                        actualPassword = Encoding.ASCII.GetString(request.SmbData.UnicodePassword);
                    }

                    if (actualPassword == accountCredential.Password)
                    {
                        account = accountCredential.AccountName;
                        break;
                    }
                }
            }
            return account;
        }


        /// <summary>
        /// NTLMAuthenticate Method
        /// </summary>
        /// <param name="request">the SessionSetup request.</param>
        /// <param name="nlmpServerSecurityContexts">all the users' security contexts.</param>
        /// <param name="systemTime">The server system time.</param>
        /// <returns>return the user's security context if passed, return null if failed.</returns>
        public static NlmpServerSecurityContext NTLMAuthenticate(
            SmbSessionSetupAndxRequestPacket request,
            Collection<NlmpServerSecurityContext> nlmpServerSecurityContexts,
             ulong systemTime)
        {
            NlmpServerSecurityContext nlmpServerSecurityContext = null;

            if (request != null
                && request.SmbParameters.OEMPasswordLen != 0
                && request.SmbParameters.UnicodePasswordLen != 0)
            {
                foreach (NlmpServerSecurityContext securityContext in nlmpServerSecurityContexts)
                {
                    string accountOfRequest = CifsMessageUtils.ToString(request.SmbData.AccountName,
                               request.SmbHeader.Flags2);
                    string domainOfRequest = CifsMessageUtils.ToString(request.SmbData.PrimaryDomain,
                        request.SmbHeader.Flags2);
                    if (securityContext.VerifySecurityToken(
                        accountOfRequest,
                        domainOfRequest,
                        systemTime,
                        request.SmbData.OEMPassword,
                        request.SmbData.UnicodePassword))
                    {
                        nlmpServerSecurityContext = securityContext;
                        break;
                    }
                }
            }
            return nlmpServerSecurityContext;
        }


        /// <summary>
        /// Verify the receiving request/response if message sign is active.
        /// </summary>
        /// <param name="packet">the request or response received.</param>
        /// <param name="sessionKey">the session key</param>
        /// <param name="sequenceNumber">the sequence number</param>
        /// <returns>return true if passed, return false if failed.</returns>
        public static bool VerifySignature(SmbPacket packet, byte[] sessionKey, uint sequenceNumber)
        {
            return VerifySignature(packet, sessionKey, sequenceNumber, new byte[0]);
        }


        /// <summary>
        /// Verify the receiving request/response if message sign is active.
        /// </summary>
        /// <param name="packet">the request or response received.</param>
        /// <param name="sessionKey">the session key</param>
        /// <param name="sequenceNumber">the sequence number</param>
        /// <param name="challengeResponse">the challenge response.</param>
        /// <returns>return true if passed, return false if failed.</returns>
        public static bool VerifySignature(SmbPacket packet, byte[] sessionKey, uint sequenceNumber, 
            byte[] challengeResponse)
        {
            SmbPacket clonePacket = packet.Clone() as SmbPacket;

            clonePacket.Sign(sequenceNumber, sessionKey, challengeResponse);
            return packet.SmbHeader.SecurityFeatures == clonePacket.SmbHeader.SecurityFeatures;
        }


        /// <summary>
        /// get the pad length.
        /// </summary>
        /// <param name="packetLength">the length to calculate pad for</param>
        /// <param name="lengthPadingTo">the length to padding, e.g. 4 bytes align, 8 bytes align.</param>
        /// <returns>the pad length</returns>
        public static int CalculatePadLength(int packetLength, int lengthPadingTo)
        {
            if(packetLength < 0)
            {
                throw new InvalidOperationException("the packet length mustnot be negative.");
            }

            if (packetLength == 0)
            {
                return 0;
            }

            int len = lengthPadingTo - packetLength % lengthPadingTo;

            return len;
        }

        #endregion
    }
}
