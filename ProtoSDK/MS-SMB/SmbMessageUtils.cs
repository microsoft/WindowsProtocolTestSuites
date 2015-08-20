// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the utility for smb stacksdk. 
    /// </summary>
    internal static class SmbMessageUtils
    {
        #region Com Packet Converter

        /// <summary>
        /// convert the payload of tree connect packet from base class format to sub class format, that is 
        /// Cifs to Smb format.
        /// </summary>
        /// <param name="baseClassFormatPayload">the base class format, Cifs format.</param>
        /// <returns>the sub class format, Smb format</returns>
        internal static Smb.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters ConvertSmbComTreeConnectPacketPayload(
            Cifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters
                subClassFormatPayload = new Smb.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.AndXCommand = baseClassFormatPayload.AndXCommand;
            subClassFormatPayload.AndXReserved = baseClassFormatPayload.AndXReserved;
            subClassFormatPayload.AndXOffset = baseClassFormatPayload.AndXOffset;
            subClassFormatPayload.OptionalSupport = baseClassFormatPayload.OptionalSupport;

            return subClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of tree connect packet from sub class format to base class format, that is Smb
        /// to Cifs format.
        /// </summary>
        /// <param name="subClassFormatPayload">the sub class format, Smb format.</param>
        /// <returns>the base class format, Cifs format</returns>
        internal static Cifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters ConvertSmbComTreeConnectPacketPayload(
            Smb.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters subClassFormatPayload)
        {
            Cifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.AndXCommand = subClassFormatPayload.AndXCommand;
            baseClassFormatPayload.AndXReserved = subClassFormatPayload.AndXReserved;
            baseClassFormatPayload.AndXOffset = subClassFormatPayload.AndXOffset;
            baseClassFormatPayload.OptionalSupport = subClassFormatPayload.OptionalSupport;

            return baseClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of write response packet from base class format to sub class format, that is 
        /// Cifs to Smb format.
        /// </summary>
        /// <param name="baseClassFormatPayload">the base class format, Cifs format.</param>
        /// <returns>the sub class format, Smb format</returns>
        internal static Smb.SMB_COM_WRITE_ANDX_Response_SMB_Parameters ConvertSmbComWriteResponsePacketPayload(
            Cifs.SMB_COM_WRITE_ANDX_Response_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_WRITE_ANDX_Response_SMB_Parameters
                subClassFormatPayload = new Smb.SMB_COM_WRITE_ANDX_Response_SMB_Parameters();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.AndXCommand = baseClassFormatPayload.AndXCommand;
            subClassFormatPayload.AndXReserved = baseClassFormatPayload.AndXReserved;
            subClassFormatPayload.AndXOffset = baseClassFormatPayload.AndXOffset;
            subClassFormatPayload.Count = baseClassFormatPayload.Count;
            subClassFormatPayload.Available = baseClassFormatPayload.Available;
            // get CountHigh from the low 2 bytes of Reserved
            subClassFormatPayload.CountHigh = (ushort)baseClassFormatPayload.Reserved;
            subClassFormatPayload.Reserved = (ushort)(baseClassFormatPayload.Reserved >> 16);

            return subClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of write response packet from sub class format to base class format, that is Smb
        /// to Cifs format.
        /// </summary>
        /// <param name="subClassFormatPayload">the sub class format, Smb format.</param>
        /// <returns>the base class format, Cifs format</returns>
        internal static Cifs.SMB_COM_WRITE_ANDX_Response_SMB_Parameters ConvertSmbComWriteResponsePacketPayload(
            Smb.SMB_COM_WRITE_ANDX_Response_SMB_Parameters subClassFormatPayload)
        {
            Cifs.SMB_COM_WRITE_ANDX_Response_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_WRITE_ANDX_Response_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.AndXCommand = subClassFormatPayload.AndXCommand;
            baseClassFormatPayload.AndXReserved = subClassFormatPayload.AndXReserved;
            baseClassFormatPayload.AndXOffset = subClassFormatPayload.AndXOffset;
            baseClassFormatPayload.Count = subClassFormatPayload.Count;
            baseClassFormatPayload.Available = subClassFormatPayload.Available;
            // save CountHigh to high 2 bytes in Reserved.
            baseClassFormatPayload.Reserved = (uint)(subClassFormatPayload.Reserved << 16);
            baseClassFormatPayload.Reserved |= subClassFormatPayload.CountHigh;

            return baseClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of write request packet from base class format to sub class format, that is 
        /// Cifs to Smb format.
        /// </summary>
        /// <param name="baseClassFormatPayload">the base class format, Cifs format.</param>
        /// <returns>the sub class format, Smb format</returns>
        internal static Smb.SMB_COM_WRITE_ANDX_Request_SMB_Parameters ConvertSmbComWriteRequestPacketPayload(
            Cifs.SMB_COM_WRITE_ANDX_Request_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_WRITE_ANDX_Request_SMB_Parameters
                subClassFormatPayload = new Smb.SMB_COM_WRITE_ANDX_Request_SMB_Parameters();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.AndXCommand = baseClassFormatPayload.AndXCommand;
            subClassFormatPayload.AndXReserved = baseClassFormatPayload.AndXReserved;
            subClassFormatPayload.AndXOffset = baseClassFormatPayload.AndXOffset;
            subClassFormatPayload.FID = baseClassFormatPayload.FID;
            subClassFormatPayload.Offset = baseClassFormatPayload.Offset;
            subClassFormatPayload.Timeout = baseClassFormatPayload.Timeout;
            subClassFormatPayload.WriteMode = baseClassFormatPayload.WriteMode;
            subClassFormatPayload.Remaining = baseClassFormatPayload.Remaining;
            subClassFormatPayload.DataLengthHigh = baseClassFormatPayload.Reserved;
            subClassFormatPayload.DataLength = baseClassFormatPayload.DataLength;
            subClassFormatPayload.DataOffset = baseClassFormatPayload.DataOffset;
            subClassFormatPayload.OffsetHigh = baseClassFormatPayload.OffsetHigh;

            return subClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of write request packet from sub class format to base class format, that is Smb
        /// to Cifs format.
        /// </summary>
        /// <param name="subClassFormatPayload">the sub class format, Smb format.</param>
        /// <returns>the base class format, Cifs format</returns>
        internal static Cifs.SMB_COM_WRITE_ANDX_Request_SMB_Parameters ConvertSmbComWriteRequestPacketPayload(
            Smb.SMB_COM_WRITE_ANDX_Request_SMB_Parameters subClassFormatPayload)
        {
            Cifs.SMB_COM_WRITE_ANDX_Request_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_WRITE_ANDX_Request_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.AndXCommand = subClassFormatPayload.AndXCommand;
            baseClassFormatPayload.AndXReserved = subClassFormatPayload.AndXReserved;
            baseClassFormatPayload.AndXOffset = subClassFormatPayload.AndXOffset;
            baseClassFormatPayload.FID = subClassFormatPayload.FID;
            baseClassFormatPayload.Offset = subClassFormatPayload.Offset;
            baseClassFormatPayload.Timeout = subClassFormatPayload.Timeout;
            baseClassFormatPayload.WriteMode = subClassFormatPayload.WriteMode;
            baseClassFormatPayload.Remaining = subClassFormatPayload.Remaining;
            baseClassFormatPayload.Reserved = subClassFormatPayload.DataLengthHigh;
            baseClassFormatPayload.DataLength = subClassFormatPayload.DataLength;
            baseClassFormatPayload.DataOffset = subClassFormatPayload.DataOffset;
            baseClassFormatPayload.OffsetHigh = subClassFormatPayload.OffsetHigh;

            return baseClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of tree connect packet from base class format to sub class format, that is 
        /// Cifs to Smb format.
        /// </summary>
        /// <param name="baseClassFormatPayload">the base class format, Cifs format.</param>
        /// <returns>the sub class format, Smb format</returns>
        internal static Smb.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters ConvertSmbComCreatePacketPayload(
            Cifs.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters
                subClassFormatPayload = new Smb.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.AndXCommand = baseClassFormatPayload.AndXCommand;
            subClassFormatPayload.AndXReserved = baseClassFormatPayload.AndXReserved;
            subClassFormatPayload.AndXOffset = baseClassFormatPayload.AndXOffset;
            subClassFormatPayload.OplockLevel = baseClassFormatPayload.OplockLevel;
            subClassFormatPayload.FID = baseClassFormatPayload.FID;
            subClassFormatPayload.CreationAction = (uint)baseClassFormatPayload.CreateDisposition;
            subClassFormatPayload.CreateTime = baseClassFormatPayload.CreateTime;
            subClassFormatPayload.LastAccessTime = baseClassFormatPayload.LastAccessTime;
            subClassFormatPayload.LastWriteTime = baseClassFormatPayload.LastWriteTime;
            subClassFormatPayload.LastChangeTime = baseClassFormatPayload.LastChangeTime;
            subClassFormatPayload.ExtFileAttributes = (uint)baseClassFormatPayload.ExtFileAttributes;
            subClassFormatPayload.AllocationSize = baseClassFormatPayload.AllocationSize;
            subClassFormatPayload.EndOfFile = baseClassFormatPayload.EndOfFile;
            subClassFormatPayload.ResourceType = baseClassFormatPayload.ResourceType;
            subClassFormatPayload.NMPipeStatus_or_FileStatusFlags = baseClassFormatPayload.NMPipeStatus;
            subClassFormatPayload.Directory = baseClassFormatPayload.Directory;

            return subClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of tree connect packet from sub class format to base class format, that is Smb
        /// to Cifs format.
        /// </summary>
        /// <param name="subClassFormatPayload">the sub class format, Smb format.</param>
        /// <returns>the base class format, Cifs format</returns>
        internal static Cifs.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters ConvertSmbComCreatePacketPayload(
            Smb.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters subClassFormatPayload)
        {
            Cifs.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.AndXCommand = subClassFormatPayload.AndXCommand;
            baseClassFormatPayload.AndXReserved = subClassFormatPayload.AndXReserved;
            baseClassFormatPayload.AndXOffset = subClassFormatPayload.AndXOffset;
            baseClassFormatPayload.OplockLevel = subClassFormatPayload.OplockLevel;
            baseClassFormatPayload.FID = subClassFormatPayload.FID;
            baseClassFormatPayload.CreateDisposition = (NtTransactCreateDisposition)subClassFormatPayload.CreationAction;
            baseClassFormatPayload.CreateTime = subClassFormatPayload.CreateTime;
            baseClassFormatPayload.LastAccessTime = subClassFormatPayload.LastAccessTime;
            baseClassFormatPayload.LastWriteTime = subClassFormatPayload.LastWriteTime;
            baseClassFormatPayload.LastChangeTime = subClassFormatPayload.LastChangeTime;
            baseClassFormatPayload.ExtFileAttributes = (SMB_EXT_FILE_ATTR)subClassFormatPayload.ExtFileAttributes;
            baseClassFormatPayload.AllocationSize = subClassFormatPayload.AllocationSize;
            baseClassFormatPayload.EndOfFile = subClassFormatPayload.EndOfFile;
            baseClassFormatPayload.ResourceType = subClassFormatPayload.ResourceType;
            baseClassFormatPayload.NMPipeStatus = subClassFormatPayload.NMPipeStatus_or_FileStatusFlags;
            baseClassFormatPayload.Directory = subClassFormatPayload.Directory;

            return baseClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of read response packet from base class format to sub class format, that is 
        /// Cifs to Smb format.
        /// </summary>
        /// <param name="baseClassFormatPayload">the base class format, Cifs format.</param>
        /// <returns>the sub class format, Smb format</returns>
        internal static Smb.SMB_COM_READ_ANDX_Response_SMB_Parameters ConvertSmbComReadResponsePacketPayload(
            Cifs.SMB_COM_READ_ANDX_Response_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_READ_ANDX_Response_SMB_Parameters
                subClassFormatPayload = new Smb.SMB_COM_READ_ANDX_Response_SMB_Parameters();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.AndXCommand = baseClassFormatPayload.AndXCommand;
            subClassFormatPayload.AndXReserved = baseClassFormatPayload.AndXReserved;
            subClassFormatPayload.AndXOffset = baseClassFormatPayload.AndXOffset;
            subClassFormatPayload.Available = baseClassFormatPayload.Available;
            subClassFormatPayload.DataCompactionMode = baseClassFormatPayload.DataCompactionMode;
            subClassFormatPayload.Reserved1 = baseClassFormatPayload.Reserved1;
            subClassFormatPayload.DataLength = baseClassFormatPayload.DataLength;
            subClassFormatPayload.DataOffset = baseClassFormatPayload.DataOffset;
            // DataLengthHigh stored in the first ushort of base.
            subClassFormatPayload.DataLengthHigh = baseClassFormatPayload.Reserved2[0];
            // reserved2 must be 4 words.
            subClassFormatPayload.Reserved2 = new ushort[4];
            subClassFormatPayload.Reserved2[0] = baseClassFormatPayload.Reserved2[1];
            subClassFormatPayload.Reserved2[1] = baseClassFormatPayload.Reserved2[2];
            subClassFormatPayload.Reserved2[2] = baseClassFormatPayload.Reserved2[3];
            subClassFormatPayload.Reserved2[3] = baseClassFormatPayload.Reserved2[4];

            return subClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of read response packet from sub class format to base class format, that is
        /// Smb to Cifs format.
        /// </summary>
        /// <param name="subClassFormatPayload">the sub class format, Smb format.</param>
        /// <returns>the base class format, Cifs format</returns>
        internal static Cifs.SMB_COM_READ_ANDX_Response_SMB_Parameters ConvertSmbComReadResponsePacketPayload(
            Smb.SMB_COM_READ_ANDX_Response_SMB_Parameters subClassFormatPayload)
        {
            Cifs.SMB_COM_READ_ANDX_Response_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_READ_ANDX_Response_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.AndXCommand = subClassFormatPayload.AndXCommand;
            baseClassFormatPayload.AndXReserved = subClassFormatPayload.AndXReserved;
            baseClassFormatPayload.AndXOffset = subClassFormatPayload.AndXOffset;
            baseClassFormatPayload.Available = subClassFormatPayload.Available;
            baseClassFormatPayload.DataCompactionMode = subClassFormatPayload.DataCompactionMode;
            baseClassFormatPayload.Reserved1 = subClassFormatPayload.Reserved1;
            baseClassFormatPayload.DataLength = subClassFormatPayload.DataLength;
            baseClassFormatPayload.DataOffset = subClassFormatPayload.DataOffset;
            // reserved2 must be 5 words.
            baseClassFormatPayload.Reserved2 = new ushort[5];
            // store DataLengthHigh to the first ushort of base.
            baseClassFormatPayload.Reserved2[0] = subClassFormatPayload.DataLengthHigh;
            baseClassFormatPayload.Reserved2[1] = subClassFormatPayload.Reserved2[0];
            baseClassFormatPayload.Reserved2[2] = subClassFormatPayload.Reserved2[1];
            baseClassFormatPayload.Reserved2[3] = subClassFormatPayload.Reserved2[2];
            baseClassFormatPayload.Reserved2[4] = subClassFormatPayload.Reserved2[3];

            return baseClassFormatPayload;
        }


        #endregion

        #region Transaction Packet Converter

        /// <summary>
        /// convert the payload of transaction packet from sub class format to base class format, that is Smb 
        /// to Cifs format. 
        /// </summary>
        /// <param name = "subClassFormatPayload">the sub class format, Smb format </param>
        /// <returns>the base class format, Cifs format </returns>
        internal static SMB_COM_TRANSACTION_Request_SMB_Parameters ConvertTransactionFilePacketPayload(
            SMB_COM_TRANSACTION_Request_SMB_Parameters_File subClassFormatPayload)
        {
            Cifs.SMB_COM_TRANSACTION_Request_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_TRANSACTION_Request_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.TotalParameterCount = subClassFormatPayload.TotalParameterCount;
            baseClassFormatPayload.TotalDataCount = subClassFormatPayload.TotalDataCount;
            baseClassFormatPayload.MaxParameterCount = subClassFormatPayload.MaxParameterCount;
            baseClassFormatPayload.MaxDataCount = subClassFormatPayload.MaxDataCount;
            baseClassFormatPayload.MaxSetupCount = subClassFormatPayload.MaxSetupCount;
            baseClassFormatPayload.Reserved1 = subClassFormatPayload.Reserved1;
            baseClassFormatPayload.Flags = subClassFormatPayload.Flags;
            baseClassFormatPayload.Timeout = subClassFormatPayload.Timeout;
            baseClassFormatPayload.Reserved2 = subClassFormatPayload.Reserved2;
            baseClassFormatPayload.ParameterCount = subClassFormatPayload.ParameterCount;
            baseClassFormatPayload.ParameterOffset = subClassFormatPayload.ParameterOffset;
            baseClassFormatPayload.DataCount = subClassFormatPayload.DataCount;
            baseClassFormatPayload.DataOffset = subClassFormatPayload.DataOffset;
            baseClassFormatPayload.SetupCount = subClassFormatPayload.SetupCount;
            baseClassFormatPayload.Reserved3 = subClassFormatPayload.Reserved3;
            baseClassFormatPayload.Setup = new ushort[2];
            baseClassFormatPayload.Setup[0] = subClassFormatPayload.Subcommand;
            baseClassFormatPayload.Setup[1] = subClassFormatPayload.Fid;

            return baseClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of transaction packet from base class format to sub class format, that is 
        /// Cifs to Smb format. 
        /// </summary>
        /// <param name = "baseClassFormatPayload">the base class format, Cifs format </param>
        /// <returns>the sub class format, Smb format </returns>
        internal static SMB_COM_TRANSACTION_Request_SMB_Parameters_File ConvertTransactionFilePacketPayload(
            SMB_COM_TRANSACTION_Request_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_TRANSACTION_Request_SMB_Parameters_File
                subClassFormatPayload = new Smb.SMB_COM_TRANSACTION_Request_SMB_Parameters_File();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.TotalParameterCount = baseClassFormatPayload.TotalParameterCount;
            subClassFormatPayload.TotalDataCount = baseClassFormatPayload.TotalDataCount;
            subClassFormatPayload.MaxParameterCount = baseClassFormatPayload.MaxParameterCount;
            subClassFormatPayload.MaxDataCount = baseClassFormatPayload.MaxDataCount;
            subClassFormatPayload.MaxSetupCount = baseClassFormatPayload.MaxSetupCount;
            subClassFormatPayload.Reserved1 = baseClassFormatPayload.Reserved1;
            subClassFormatPayload.Flags = baseClassFormatPayload.Flags;
            subClassFormatPayload.Timeout = baseClassFormatPayload.Timeout;
            subClassFormatPayload.Reserved2 = baseClassFormatPayload.Reserved2;
            subClassFormatPayload.ParameterCount = baseClassFormatPayload.ParameterCount;
            subClassFormatPayload.ParameterOffset = baseClassFormatPayload.ParameterOffset;
            subClassFormatPayload.DataCount = baseClassFormatPayload.DataCount;
            subClassFormatPayload.DataOffset = baseClassFormatPayload.DataOffset;
            subClassFormatPayload.SetupCount = baseClassFormatPayload.SetupCount;
            subClassFormatPayload.Reserved3 = baseClassFormatPayload.Reserved3;
            subClassFormatPayload.Subcommand = baseClassFormatPayload.Setup[0];
            subClassFormatPayload.Fid = baseClassFormatPayload.Setup[1];

            return subClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of transaction packet from sub class format to base class format, that is Smb 
        /// to Cifs format. 
        /// </summary>
        /// <param name = "subClassFormatPayload">the sub class format, Smb format </param>
        /// <returns>the base class format, Cifs format </returns>
        internal static SMB_COM_TRANSACTION_Request_SMB_Parameters ConvertTransactionMailslotPacketPayload(
            SMB_COM_TRANSACTION_Request_SMB_Parameters_Mailslot subClassFormatPayload)
        {
            Cifs.SMB_COM_TRANSACTION_Request_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_TRANSACTION_Request_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.TotalParameterCount = subClassFormatPayload.TotalParameterCount;
            baseClassFormatPayload.TotalDataCount = subClassFormatPayload.TotalDataCount;
            baseClassFormatPayload.MaxParameterCount = subClassFormatPayload.MaxParameterCount;
            baseClassFormatPayload.MaxDataCount = subClassFormatPayload.MaxDataCount;
            baseClassFormatPayload.MaxSetupCount = subClassFormatPayload.MaxSetupCount;
            baseClassFormatPayload.Reserved1 = subClassFormatPayload.Reserved1;
            baseClassFormatPayload.Flags = subClassFormatPayload.Flags;
            baseClassFormatPayload.Timeout = subClassFormatPayload.Timeout;
            baseClassFormatPayload.Reserved2 = subClassFormatPayload.Reserved2;
            baseClassFormatPayload.ParameterCount = subClassFormatPayload.ParameterCount;
            baseClassFormatPayload.ParameterOffset = subClassFormatPayload.ParameterOffset;
            baseClassFormatPayload.DataCount = subClassFormatPayload.DataCount;
            baseClassFormatPayload.DataOffset = subClassFormatPayload.DataOffset;
            baseClassFormatPayload.SetupCount = subClassFormatPayload.SetupCount;
            baseClassFormatPayload.Reserved3 = subClassFormatPayload.Reserved3;
            baseClassFormatPayload.Setup = new ushort[3];
            baseClassFormatPayload.Setup[0] = subClassFormatPayload.Subcommand;
            baseClassFormatPayload.Setup[1] = subClassFormatPayload.Priority;
            baseClassFormatPayload.Setup[2] = (ushort)subClassFormatPayload.Class;

            return baseClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of transaction packet from base class format to sub class format, that is 
        /// Cifs to Smb format. 
        /// </summary>
        /// <param name = "baseClassFormatPayload">the base class format, Cifs format </param>
        /// <returns>the sub class format, Smb format </returns>
        internal static SMB_COM_TRANSACTION_Request_SMB_Parameters_Mailslot ConvertTransactionMailslotPacketPayload(
            SMB_COM_TRANSACTION_Request_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_TRANSACTION_Request_SMB_Parameters_Mailslot
                subClassFormatPayload = new Smb.SMB_COM_TRANSACTION_Request_SMB_Parameters_Mailslot();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.TotalParameterCount = baseClassFormatPayload.TotalParameterCount;
            subClassFormatPayload.TotalDataCount = baseClassFormatPayload.TotalDataCount;
            subClassFormatPayload.MaxParameterCount = baseClassFormatPayload.MaxParameterCount;
            subClassFormatPayload.MaxDataCount = baseClassFormatPayload.MaxDataCount;
            subClassFormatPayload.MaxSetupCount = baseClassFormatPayload.MaxSetupCount;
            subClassFormatPayload.Reserved1 = baseClassFormatPayload.Reserved1;
            subClassFormatPayload.Flags = baseClassFormatPayload.Flags;
            subClassFormatPayload.Timeout = baseClassFormatPayload.Timeout;
            subClassFormatPayload.Reserved2 = baseClassFormatPayload.Reserved2;
            subClassFormatPayload.ParameterCount = baseClassFormatPayload.ParameterCount;
            subClassFormatPayload.ParameterOffset = baseClassFormatPayload.ParameterOffset;
            subClassFormatPayload.DataCount = baseClassFormatPayload.DataCount;
            subClassFormatPayload.DataOffset = baseClassFormatPayload.DataOffset;
            subClassFormatPayload.SetupCount = baseClassFormatPayload.SetupCount;
            subClassFormatPayload.Reserved3 = baseClassFormatPayload.Reserved3;
            subClassFormatPayload.Subcommand = baseClassFormatPayload.Setup[0];
            subClassFormatPayload.Priority = baseClassFormatPayload.Setup[1];
            subClassFormatPayload.Class = (SmbTransMailslotClass)baseClassFormatPayload.Setup[2];

            return subClassFormatPayload;
        }


        #endregion

        #region Transaction2 Packet Convert

        /// <summary>
        /// convert the payload of transaction2 packet from base class format to sub class format, that is 
        /// Cifs format to Smb format. 
        /// </summary>
        /// <param name = "baseClassFormatPayload">the base class format, Cifs format </param>
        /// <returns>the sub class format, Smb format </returns>
        internal static Smb.SMB_COM_TRANSACTION2_Request_SMB_Parameters ConvertTransaction2PacketPayload(
            Cifs.SMB_COM_TRANSACTION2_Request_SMB_Parameters baseClassFormatPayload)
        {
            Smb.SMB_COM_TRANSACTION2_Request_SMB_Parameters
                subClassFormatPayload = new Smb.SMB_COM_TRANSACTION2_Request_SMB_Parameters();

            subClassFormatPayload.WordCount = baseClassFormatPayload.WordCount;
            subClassFormatPayload.TotalParameterCount = baseClassFormatPayload.TotalParameterCount;
            subClassFormatPayload.TotalDataCount = baseClassFormatPayload.TotalDataCount;
            subClassFormatPayload.MaxParameterCount = baseClassFormatPayload.MaxParameterCount;
            subClassFormatPayload.MaxDataCount = baseClassFormatPayload.MaxDataCount;
            subClassFormatPayload.MaxSetupCount = baseClassFormatPayload.MaxSetupCount;
            subClassFormatPayload.Reserved1 = baseClassFormatPayload.Reserved1;
            subClassFormatPayload.Flags = baseClassFormatPayload.Flags;
            subClassFormatPayload.Timeout = baseClassFormatPayload.Timeout;
            subClassFormatPayload.Reserved2 = baseClassFormatPayload.Reserved2;
            subClassFormatPayload.ParameterCount = baseClassFormatPayload.ParameterCount;
            subClassFormatPayload.ParameterOffset = baseClassFormatPayload.ParameterOffset;
            subClassFormatPayload.DataCount = baseClassFormatPayload.DataCount;
            subClassFormatPayload.DataOffset = baseClassFormatPayload.DataOffset;
            subClassFormatPayload.SetupCount = baseClassFormatPayload.SetupCount;
            subClassFormatPayload.Reserved3 = baseClassFormatPayload.Reserved3;
            subClassFormatPayload.Subcommand = baseClassFormatPayload.Setup[0];

            return subClassFormatPayload;
        }


        /// <summary>
        /// convert the payload of transaction2 packet from sub class format to base class format, that is 
        /// Smb format to Cifs format. 
        /// </summary>
        /// <param name = "subClassFormatPayload">the sub class format, Smb format </param>
        /// <returns>the base class format, Cifs format </returns>
        internal static Cifs.SMB_COM_TRANSACTION2_Request_SMB_Parameters ConvertTransaction2PacketPayload(
            Smb.SMB_COM_TRANSACTION2_Request_SMB_Parameters subClassFormatPayload)
        {
            Cifs.SMB_COM_TRANSACTION2_Request_SMB_Parameters
                baseClassFormatPayload = new Cifs.SMB_COM_TRANSACTION2_Request_SMB_Parameters();

            baseClassFormatPayload.WordCount = subClassFormatPayload.WordCount;
            baseClassFormatPayload.TotalParameterCount = subClassFormatPayload.TotalParameterCount;
            baseClassFormatPayload.TotalDataCount = subClassFormatPayload.TotalDataCount;
            baseClassFormatPayload.MaxParameterCount = subClassFormatPayload.MaxParameterCount;
            baseClassFormatPayload.MaxDataCount = subClassFormatPayload.MaxDataCount;
            baseClassFormatPayload.MaxSetupCount = subClassFormatPayload.MaxSetupCount;
            baseClassFormatPayload.Reserved1 = subClassFormatPayload.Reserved1;
            baseClassFormatPayload.Flags = subClassFormatPayload.Flags;
            baseClassFormatPayload.Timeout = subClassFormatPayload.Timeout;
            baseClassFormatPayload.Reserved2 = subClassFormatPayload.Reserved2;
            baseClassFormatPayload.ParameterCount = subClassFormatPayload.ParameterCount;
            baseClassFormatPayload.ParameterOffset = subClassFormatPayload.ParameterOffset;
            baseClassFormatPayload.DataCount = subClassFormatPayload.DataCount;
            baseClassFormatPayload.DataOffset = subClassFormatPayload.DataOffset;
            baseClassFormatPayload.SetupCount = subClassFormatPayload.SetupCount;
            baseClassFormatPayload.Reserved3 = subClassFormatPayload.Reserved3;
            baseClassFormatPayload.Setup = new ushort[1];
            baseClassFormatPayload.Setup[0] = subClassFormatPayload.Subcommand;

            return baseClassFormatPayload;
        }


        #endregion

        #region Convert Smb Find Information Level Payloads

        /// <summary>
        /// convert data to SMB_FIND_FILE_BOTH_DIRECTORY_INFO array.
        /// </summary>
        /// <param name="informationLevel">the information level</param>
        /// <param name="arraySize">the array size</param>
        /// <param name="data">the data</param>
        /// <returns>the unmarshaled result</returns>
        public static object UnmarshalSmbFindInformationLevelPayloads(
            FindInformationLevel informationLevel,
            int arraySize, byte[] data)
        {
            switch ((FindInformationLevel)informationLevel)
            {
                case FindInformationLevel.SMB_FIND_FILE_BOTH_DIRECTORY_INFO:
                    {
                        SMB_FIND_FILE_BOTH_DIRECTORY_INFO[] result =
                            new SMB_FIND_FILE_BOTH_DIRECTORY_INFO[arraySize];

                        using (MemoryStream memoryStream = new MemoryStream(data))
                        {
                            using (Channel channel = new Channel(null, memoryStream))
                            {
                                for (int i = 0; i < result.Length; i++)
                                {
                                    result[i] = channel.Read<SMB_FIND_FILE_BOTH_DIRECTORY_INFO>();
                                    int pad = Convert.ToInt32(result[i].NextEntryOffset -
                                        CifsMessageUtils.GetSize<SMB_FIND_FILE_BOTH_DIRECTORY_INFO>(result[i]));
                                    if (pad > 0)
                                    {
                                        channel.ReadBytes(pad);
                                    }
                                }
                            }
                        }

                        return result;
                    }

                case FindInformationLevel.SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO:
                    {
                        SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO[] result =
                            new SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO[arraySize];

                        using (MemoryStream memoryStream = new MemoryStream(data))
                        {
                            using (Channel channel = new Channel(null, memoryStream))
                            {
                                for (int i = 0; i < result.Length; i++)
                                {
                                    result[i] = channel.Read<SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO>();
                                    int pad = Convert.ToInt32(result[i].NextEntryOffset -
                                        CifsMessageUtils.GetSize<SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO>(result[i]));
                                    if (pad > 0)
                                    {
                                        channel.ReadBytes(pad);
                                    }
                                }
                            }
                        }

                        return result;
                    }

                case FindInformationLevel.SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO:
                    {

                        SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO[] result =
                            new SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO[arraySize];

                        using (MemoryStream memoryStream = new MemoryStream(data))
                        {
                            using (Channel channel = new Channel(null, memoryStream))
                            {
                                for (int i = 0; i < result.Length; i++)
                                {
                                    result[i] = channel.Read<SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO>();
                                    int pad = Convert.ToInt32(result[i].NextEntryOffset -
                                        CifsMessageUtils.GetSize<SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO>(result[i]));
                                    if (pad > 0)
                                    {
                                        channel.ReadBytes(pad);
                                    }
                                }
                            }
                        }
                        return result;
                    }

                default:

                    return null;
            }
        }


        #endregion

        #region Common Converter

        /// <summary>
        /// get the string in Unicode or ascii format.
        /// </summary>
        /// <param name="bytes">the bytes of string</param>
        /// <param name="isUnicode">whether the format is Unicode</param>
        /// <returns>the decoded string</returns>
        internal static string GetString(byte[] bytes, bool isUnicode)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            if (isUnicode)
            {
                // skip the first zero padding byte.
                if (bytes[0] == 0x00)
                {
                    return Encoding.Unicode.GetString(bytes, 1, bytes.Length - 1);
                }
                else
                {
                    return Encoding.Unicode.GetString(bytes);
                }
            }
            else
            {
                return Encoding.ASCII.GetString(bytes);
            }
        }


        #endregion

        #region Utilities

        /// <summary>
        /// check the status whether is error status.<para/>
        /// if true, throw exception; otherwise, return its value.
        /// </summary>
        /// <param name="status">
        /// a uint value that specifies the value to check.
        /// </param>
        /// <returns>
        /// if status indicates that the packet is not error, return status.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when server response error.
        /// </exception>
        internal static uint CheckStatus(uint status)
        {
            SmbStatus smbStatus = (SmbStatus)status;
            NtStatus ntStatus = (NtStatus)status;

            if (smbStatus != SmbStatus.STATUS_SUCCESS
                && smbStatus != SmbStatus.STATUS_MORE_PROCESSING_REQUIRED
                && ntStatus != NtStatus.STATUS_PIPE_EMPTY
                && smbStatus != SmbStatus.STATUS_BUFFER_OVERFLOW)
            {
                throw new InvalidOperationException(
                   string.Format("Fails with error code: 0x{0:x}.", status));
            }

            return status;
        }

        #endregion
    }
}
