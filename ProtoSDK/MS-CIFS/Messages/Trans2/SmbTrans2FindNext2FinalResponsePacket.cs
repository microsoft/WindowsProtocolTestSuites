// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTrans2FindNext2Final Response
    /// </summary>
    public class SmbTrans2FindNext2FinalResponsePacket : SmbTransaction2FinalResponsePacket
    {
        #region Fields

        private TRANS2_FIND_NEXT2_Response_Trans2_Parameters trans2Parameters;
        private TRANS2_FIND_NEXT2_Response_Trans2_Data trans2Data;

        /// <summary>
        /// the information level
        /// </summary>
        protected FindInformationLevel informationLevel;

        private bool isResumeKeyExisted;
        /// <summary>
        /// The size of EaSize field in SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2 sub-structure
        /// </summary>
        private const ushort eaSizeLength = 4;

        #endregion


        #region Properties

        /// <summary>
        /// Whether Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS is set 1 in Request.Trans2Parameters.Flags
        /// </summary>
        public bool IsResumeKeyExisted
        {
            get
            {
                return this.isResumeKeyExisted;
            }
            set
            {
                this.isResumeKeyExisted = value;
            }
        }


        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_FIND_FIRST2_Response_Trans2_Parameters
        /// </summary>
        public TRANS2_FIND_NEXT2_Response_Trans2_Parameters Trans2Parameters
        {
            get
            {
                return this.trans2Parameters;
            }
            set
            {
                this.trans2Parameters = value;
            }
        }


        /// <summary>
        /// get or set the Trans2_Data:TRANS2_FIND_FIRST2_Response_Trans2_Data
        /// </summary>
        public TRANS2_FIND_NEXT2_Response_Trans2_Data Trans2Data
        {
            get
            {
                return this.trans2Data;
            }
            set
            {
                this.trans2Data = value;
            }
        }
        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTrans2FindNext2FinalResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }

        
        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2FindNext2FinalResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2FindNext2FinalResponsePacket(FindInformationLevel informationLevel,
            bool isResumeKeyExisted)
        {
            this.InitDefaultValue();
            this.informationLevel = informationLevel;
            this.isResumeKeyExisted = isResumeKeyExisted;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2FindNext2FinalResponsePacket(SmbTrans2FindNext2FinalResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.trans2Parameters.SearchCount = packet.trans2Parameters.SearchCount;
            this.trans2Parameters.EndOfSearch = packet.trans2Parameters.EndOfSearch;
            this.trans2Parameters.EaErrorOffset = packet.trans2Parameters.EaErrorOffset;
            this.trans2Parameters.LastNameOffset = packet.trans2Parameters.LastNameOffset;

            this.trans2Data.Data = packet.trans2Data.Data;
            this.informationLevel = packet.informationLevel;
            this.isResumeKeyExisted = packet.isResumeKeyExisted;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2FindNext2FinalResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            this.smbData.Trans2_Parameters = CifsMessageUtils.ToBytes<TRANS2_FIND_NEXT2_Response_Trans2_Parameters>(
                this.trans2Parameters);
        }


        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        protected override void EncodeTrans2Data()
        {
            int totalSize = 0;
            int fixedSize = 0; // The fixed size of each structure

            if (this.trans2Data.Data != null)
            {
                Type type = this.trans2Data.Data.GetType();

                if (type == typeof(SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2[]))
                {
                    SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2[] standardArray = (
                        SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2[])this.trans2Data.Data;
                    fixedSize = (this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                        SmbFlags2.SMB_FLAGS2_UNICODE ? 26 : 24;
                    ushort resumeKeyLength = 4;

                    if (isResumeKeyExisted)
                    {
                        totalSize = (resumeKeyLength + fixedSize) * standardArray.Length;
                    }
                    else
                    {
                        totalSize = fixedSize * standardArray.Length;
                    }

                    for (int i = 0; i < standardArray.Length; i++)
                    {
                        totalSize += standardArray[i].FileNameLength;
                    }
                    this.smbData.Trans2_Data = new byte[totalSize];
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            channel.BeginWriteGroup();
                            for (int i = 0; i < standardArray.Length; i++)
                            {
                                if (isResumeKeyExisted)
                                {
                                    channel.Write<uint>(standardArray[i].ResumeKey);
                                }
                                channel.Write<SmbDate>(standardArray[i].CreationDate);
                                channel.Write<SmbTime>(standardArray[i].CreationTime);
                                channel.Write<SmbDate>(standardArray[i].LastAccessDate);
                                channel.Write<SmbTime>(standardArray[i].LastAccessTime);
                                channel.Write<SmbDate>(standardArray[i].LastWriteDate);
                                channel.Write<SmbTime>(standardArray[i].LastWriteTime);
                                channel.Write<uint>(standardArray[i].DataSize);
                                channel.Write<uint>(standardArray[i].AllocationSize);
                                channel.Write<SmbFileAttributes>(standardArray[i].Attributes);
                                channel.Write<byte>(standardArray[i].FileNameLength);

                                if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                                {
                                    channel.Write<byte>(new byte());
                                }
                                channel.WriteBytes(standardArray[i].FileName);

                                if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                                {
                                    channel.Write<ushort>(new ushort());
                                }
                                else
                                {
                                    channel.Write<byte>(new byte());
                                }
                            }
                            channel.EndWriteGroup();
                        }
                    }
                }

                else if (type == typeof(SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2[]))
                {
                    SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2[] queryEaArray = (
                        SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2[])this.trans2Data.Data;
                    fixedSize = 28;
                    ushort resumeKeyLength = 4;

                    if (isResumeKeyExisted)
                    {
                        totalSize = (resumeKeyLength + fixedSize) * queryEaArray.Length;
                    }
                    else
                    {
                        totalSize = fixedSize * queryEaArray.Length;
                    }

                    for (int i = 0; i < queryEaArray.Length; i++)
                    {
                        totalSize += queryEaArray[i].FileNameLength;
                    }
                    this.smbData.Trans2_Data = new byte[totalSize];
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            channel.BeginWriteGroup();
                            for (int i = 0; i < queryEaArray.Length; i++)
                            {
                                if (isResumeKeyExisted)
                                {
                                    channel.Write<uint>(queryEaArray[i].ResumeKey);
                                }
                                channel.Write<SmbDate>(queryEaArray[i].CreationDate);
                                channel.Write<SmbTime>(queryEaArray[i].CreationTime);
                                channel.Write<SmbDate>(queryEaArray[i].LastAccessDate);
                                channel.Write<SmbTime>(queryEaArray[i].LastAccessTime);
                                channel.Write<SmbDate>(queryEaArray[i].LastWriteDate);
                                channel.Write<SmbTime>(queryEaArray[i].LastWriteTime);
                                channel.Write<uint>(queryEaArray[i].DataSize);
                                channel.Write<uint>(queryEaArray[i].AllocationSize);
                                channel.Write<SmbFileAttributes>(queryEaArray[i].Attributes);
                                channel.Write<uint>(queryEaArray[i].EaSize);
                                channel.Write<byte>(queryEaArray[i].FileNameLength);
                                channel.WriteBytes(queryEaArray[i].FileName);
                                channel.Write<byte>(new byte());
                            }
                            channel.EndWriteGroup();
                        }
                    }
                }

                else if (type == typeof(SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2[]))
                {
                    SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2[] queryEaFromListArray = (
                        SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2[])this.trans2Data.Data;
                    fixedSize = 24;
                    ushort resumeKeyLength = 4;

                    if (isResumeKeyExisted)
                    {
                        totalSize = (resumeKeyLength + fixedSize) * queryEaFromListArray.Length;
                    }
                    else
                    {
                        totalSize = fixedSize * queryEaFromListArray.Length;
                    }

                    for (int i = 0; i < queryEaFromListArray.Length; i++)
                    {
                        totalSize += (int)(queryEaFromListArray[i].FileNameLength +
                            queryEaFromListArray[i].EaSize);
                    }
                    this.smbData.Trans2_Data = new byte[totalSize];
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            channel.BeginWriteGroup();
                            for (int i = 0; i < queryEaFromListArray.Length; i++)
                            {
                                if (isResumeKeyExisted)
                                {
                                    channel.Write<uint>(queryEaFromListArray[i].ResumeKey);
                                }
                                channel.Write<SmbDate>(queryEaFromListArray[i].CreationDate);
                                channel.Write<SmbTime>(queryEaFromListArray[i].CreationTime);
                                channel.Write<SmbDate>(queryEaFromListArray[i].LastAccessDate);
                                channel.Write<SmbTime>(queryEaFromListArray[i].LastAccessTime);
                                channel.Write<SmbDate>(queryEaFromListArray[i].LastWriteDate);
                                channel.Write<SmbTime>(queryEaFromListArray[i].LastWriteTime);
                                channel.Write<uint>(queryEaFromListArray[i].DataSize);
                                channel.Write<uint>(queryEaFromListArray[i].AllocationSize);
                                channel.Write<SmbFileAttributes>(queryEaFromListArray[i].Attributes);
                                channel.Write<uint>(queryEaFromListArray[i].EaSize);

                                if (queryEaFromListArray[i].ExtendedAttributeList != null)
                                {
                                    foreach (SMB_FEA smbEa in queryEaFromListArray[i].ExtendedAttributeList)
                                    {
                                        channel.Write<SMB_FEA>(smbEa);
                                    }
                                }
                                channel.Write<byte>(queryEaFromListArray[i].FileNameLength);
                                channel.WriteBytes(queryEaFromListArray[i].FileName);
                                channel.Write<byte>(new byte());
                            }
                            channel.EndWriteGroup();
                        }
                    }
                }

                else if (type == typeof(SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[]))
                {
                    SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[] fileInfoArray = (
                        SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[])this.trans2Data.Data;
                    fixedSize = 64; // Including the fixed length of this structure.
                    totalSize = fixedSize * fileInfoArray.Length;
                    int pad = 0;

                    for (int i = 0; i < fileInfoArray.Length; i++)
                    {
                        totalSize += (int)(fileInfoArray[i].FileNameLength);
                        pad = (int)(fileInfoArray[i].NextEntryOffset - fixedSize - fileInfoArray[i].FileNameLength);

                        if (pad > 0)
                        {
                            totalSize += pad;
                        }
                    }
                    this.smbData.Trans2_Data = new byte[totalSize];
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            channel.BeginWriteGroup();
                            for (int i = 0; i < fileInfoArray.Length; i++)
                            {
                                fixedSize = 64; // Including the fixed length of this structure.
                                channel.Write<SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2>(fileInfoArray[i]);
                                fixedSize += (int)fileInfoArray[i].FileNameLength; // Add the length of file name and pad.
                                pad = (int)(fileInfoArray[i].NextEntryOffset - fixedSize);

                                if (pad > 0)
                                {
                                    channel.WriteBytes(new byte[pad]);
                                }
                            }
                            channel.EndWriteGroup();
                        }
                    }
                }

                else if (type == typeof(SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[]))
                {
                    SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[] fullInfoArray =
                        (SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[])this.trans2Data.Data;
                    fixedSize = 68; // Including the fixed length of this structure.
                    totalSize = fixedSize * fullInfoArray.Length;
                    int pad = 0;

                    for (int i = 0; i < fullInfoArray.Length; i++)
                    {
                        totalSize += (int)(fullInfoArray[i].FileNameLength + fullInfoArray[i].EaSize);
                        pad = (int)(fullInfoArray[i].NextEntryOffset - fixedSize - fullInfoArray[i].FileNameLength);

                        if (pad > 0)
                        {
                            totalSize += pad;
                        }
                    }
                    this.smbData.Trans2_Data = new byte[totalSize];
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            channel.BeginWriteGroup();
                            for (int i = 0; i < fullInfoArray.Length; i++)
                            {
                                fixedSize = 68; // Including the fixed length of this structure.
                                channel.Write<SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2>(fullInfoArray[i]);
                                fixedSize += (int)fullInfoArray[i].FileNameLength; // Add the length of file name and pad.
                                pad = (int)(fullInfoArray[i].NextEntryOffset - fixedSize);

                                if (pad > 0)
                                {
                                    channel.WriteBytes(new byte[pad]);
                                }
                            }
                            channel.EndWriteGroup();
                        }
                    }
                }

                else if (type == typeof(SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[]))
                {
                    SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[] bothInfoArray = (
                        SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[])this.trans2Data.Data;

                    List<byte> buffer = new List<byte>();

                    for (int i = 0; i < bothInfoArray.Length; i++)
                    {
                        SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2 item = bothInfoArray[i];

                        // update the next entry offset.
                        int len = CifsMessageUtils.GetSize<SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2>(item);
                        item.NextEntryOffset = (uint)len;
                        if (i + 1 < bothInfoArray.Length)
                        {
                            item.NextEntryOffset += (uint)CifsMessageUtils.CalculatePadLength(len, 8);
                        }

                        byte[] data = CifsMessageUtils.ToBytes<SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2>(item);
                        buffer.AddRange(data);

                        if (i + 1 < bothInfoArray.Length)
                        {
                            // 8 bytes align
                            buffer.AddRange(new byte[CifsMessageUtils.CalculatePadLength(data.Length, 8)]);
                        }
                    }

                    this.smbData.Trans2_Data = buffer.ToArray();
                }

                else if (type == typeof(SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2[]))
                {
                    SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2[] namesInfoArray = (
                        SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2[])this.trans2Data.Data;
                    fixedSize = 12; // Including the fixed length of this structure.
                    totalSize = fixedSize * namesInfoArray.Length;
                    int pad = 0;

                    for (int i = 0; i < namesInfoArray.Length; i++)
                    {
                        totalSize += (int)(namesInfoArray[i].FileNameLength);
                        pad = (int)(namesInfoArray[i].NextEntryOffset - fixedSize - namesInfoArray[i].FileNameLength);

                        if (pad > 0)
                        {
                            totalSize += pad;
                        }
                    }
                    this.smbData.Trans2_Data = new byte[totalSize];
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            channel.BeginWriteGroup();
                            for (int i = 0; i < namesInfoArray.Length; i++)
                            {
                                fixedSize = 12; // Including the fixed length of this structure.
                                channel.Write<SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2>(namesInfoArray[i]);
                                fixedSize += (int)namesInfoArray[i].FileNameLength; // Add the length of file name.
                                pad = (int)(namesInfoArray[i].NextEntryOffset - fixedSize);

                                if (pad > 0)
                                {
                                    channel.WriteBytes(new byte[pad]);
                                }
                            }
                            channel.EndWriteGroup();
                        }
                    }
                }

                else
                {
                    // Branch for negative testing.
                }
            }
            else
            {
                this.smbData.Trans2_Data = new byte[0];
            }
        }


        /// <summary>
        /// to decode the Trans2 parameters: from the general Trans2Parameters to the concrete Trans2 Parameters.
        /// </summary>
        protected override void DecodeTrans2Parameters()
        {
            if (this.smbData.Trans2_Parameters != null && this.smbData.Trans2_Parameters.Length > 0)
            {
                this.trans2Parameters = TypeMarshal.ToStruct<TRANS2_FIND_NEXT2_Response_Trans2_Parameters>(
                    this.smbData.Trans2_Parameters);
            }
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        protected override void DecodeTrans2Data()
        {
            if (this.smbData.Trans2_Data != null && this.smbData.Trans2_Data.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        switch (this.informationLevel)
                        {
                            case FindInformationLevel.SMB_INFO_STANDARD:
                                SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2[] standardArray =
                                    new SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2[this.trans2Parameters.SearchCount];

                                for (int i = 0; i < standardArray.Length; i++)
                                {
                                    if (isResumeKeyExisted)
                                    {
                                        standardArray[i].ResumeKey = channel.Read<uint>();
                                    }
                                    standardArray[i].CreationDate = channel.Read<SmbDate>();
                                    standardArray[i].CreationTime = channel.Read<SmbTime>();
                                    standardArray[i].LastAccessDate = channel.Read<SmbDate>();
                                    standardArray[i].LastAccessTime = channel.Read<SmbTime>();
                                    standardArray[i].LastWriteDate = channel.Read<SmbDate>();
                                    standardArray[i].LastWriteTime = channel.Read<SmbTime>();
                                    standardArray[i].DataSize = channel.Read<uint>();
                                    standardArray[i].AllocationSize = channel.Read<uint>();
                                    standardArray[i].Attributes = channel.Read<SmbFileAttributes>();
                                    standardArray[i].FileNameLength = channel.Read<byte>();

                                    if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                                    {
                                        channel.Read<byte>();
                                    }
                                    standardArray[i].FileName = channel.ReadBytes(standardArray[i].FileNameLength);

                                    if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                                    {
                                        channel.Read<ushort>();
                                    }
                                    else
                                    {
                                        channel.Read<byte>();
                                    }
                                }
                                this.trans2Data.Data = standardArray;
                                break;

                            case FindInformationLevel.SMB_INFO_QUERY_EA_SIZE:
                                SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2[] queryEaArray =
                                    new SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2[this.trans2Parameters.SearchCount];

                                for (int i = 0; i < queryEaArray.Length; i++)
                                {
                                    if (isResumeKeyExisted)
                                    {
                                        queryEaArray[i].ResumeKey = channel.Read<uint>();
                                    }
                                    queryEaArray[i].CreationDate = channel.Read<SmbDate>();
                                    queryEaArray[i].CreationTime = channel.Read<SmbTime>();
                                    queryEaArray[i].LastAccessDate = channel.Read<SmbDate>();
                                    queryEaArray[i].LastAccessTime = channel.Read<SmbTime>();
                                    queryEaArray[i].LastWriteDate = channel.Read<SmbDate>();
                                    queryEaArray[i].LastWriteTime = channel.Read<SmbTime>();
                                    queryEaArray[i].DataSize = channel.Read<uint>();
                                    queryEaArray[i].AllocationSize = channel.Read<uint>();
                                    queryEaArray[i].Attributes = channel.Read<SmbFileAttributes>();
                                    queryEaArray[i].EaSize = channel.Read<uint>();
                                    queryEaArray[i].FileNameLength = channel.Read<byte>();
                                    queryEaArray[i].FileName = channel.ReadBytes(queryEaArray[i].FileNameLength);
                                    channel.Read<byte>();
                                }
                                this.trans2Data.Data = queryEaArray;
                                break;

                            case FindInformationLevel.SMB_INFO_QUERY_EAS_FROM_LIST:
                                SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2[] queryEaFromListArray =
                                    new SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2[this.trans2Parameters.SearchCount];

                                for (int i = 0; i < queryEaFromListArray.Length; i++)
                                {
                                    if (isResumeKeyExisted)
                                    {
                                        queryEaFromListArray[i].ResumeKey = channel.Read<uint>();
                                    }
                                    queryEaFromListArray[i].CreationDate = channel.Read<SmbDate>();
                                    queryEaFromListArray[i].CreationTime = channel.Read<SmbTime>();
                                    queryEaFromListArray[i].LastAccessDate = channel.Read<SmbDate>();
                                    queryEaFromListArray[i].LastAccessTime = channel.Read<SmbTime>();
                                    queryEaFromListArray[i].LastWriteDate = channel.Read<SmbDate>();
                                    queryEaFromListArray[i].LastWriteTime = channel.Read<SmbTime>();
                                    queryEaFromListArray[i].DataSize = channel.Read<uint>();
                                    queryEaFromListArray[i].AllocationSize = channel.Read<uint>();
                                    queryEaFromListArray[i].Attributes = channel.Read<SmbFileAttributes>();
                                    queryEaFromListArray[i].EaSize = channel.Read<uint>();
                                    uint eaSize = queryEaFromListArray[i].EaSize - eaSizeLength;
                                    List<SMB_FEA> attributeList = new List<SMB_FEA>();

                                    while (eaSize > 0)
                                    {
                                        SMB_FEA smbEa = channel.Read<SMB_FEA>();
                                        attributeList.Add(smbEa);
                                        eaSize -= (uint)(EA.SMB_EA_FIXED_SIZE + smbEa.AttributeName.Length +
                                            smbEa.ValueName.Length);
                                    }
                                    queryEaFromListArray[i].ExtendedAttributeList = attributeList.ToArray();
                                    queryEaFromListArray[i].FileNameLength = channel.Read<byte>();
                                    queryEaFromListArray[i].FileName = channel.ReadBytes(queryEaFromListArray[i].FileNameLength);
                                    channel.Read<byte>();
                                }
                                this.trans2Data.Data = queryEaFromListArray;
                                break;

                            case FindInformationLevel.SMB_FIND_FILE_DIRECTORY_INFO:
                                SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[] fileInfoArray =
                                    new SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[this.trans2Parameters.SearchCount];

                                for (int i = 0; i < fileInfoArray.Length; i++)
                                {
                                    uint fixedSize = 64; // Including the fixed length of this structure.
                                    fileInfoArray[i] = channel.Read<SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2>();
                                    fixedSize += fileInfoArray[i].FileNameLength; // Add the length of file name and pad.
                                    int pad = (int)(fileInfoArray[i].NextEntryOffset - fixedSize);

                                    if (pad > 0)
                                    {
                                        channel.ReadBytes(pad);
                                    }
                                }
                                this.trans2Data.Data = fileInfoArray;
                                break;

                            case FindInformationLevel.SMB_FIND_FILE_FULL_DIRECTORY_INFO:
                                SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[] fullInfoArray =
                                    new SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[this.trans2Parameters.SearchCount];

                                for (int i = 0; i < fullInfoArray.Length; i++)
                                {
                                    uint fixedSize = 68; // Including the fixed length of this structure.
                                    fullInfoArray[i] = channel.Read<SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2>();
                                    fixedSize += fullInfoArray[i].FileNameLength; // Add the length of file name and pad.
                                    int pad = (int)(fullInfoArray[i].NextEntryOffset - fixedSize);

                                    if (pad > 0)
                                    {
                                        channel.ReadBytes(pad);
                                    }
                                }
                                this.trans2Data.Data = fullInfoArray;
                                break;

                            case FindInformationLevel.SMB_FIND_FILE_BOTH_DIRECTORY_INFO:
                                SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[] bothInfoArray =
                                    new SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[this.trans2Parameters.SearchCount];

                                for (int i = 0; i < bothInfoArray.Length; i++)
                                {
                                    uint fixedSize = 94; // Including the fixed length of this structure.

                                    bothInfoArray[i].NextEntryOffset = channel.Read<uint>();
                                    bothInfoArray[i].FileIndex = channel.Read<uint>();
                                    bothInfoArray[i].CreationTime = channel.Read<FileTime>();
                                    bothInfoArray[i].LastAccessTime = channel.Read<FileTime>();
                                    bothInfoArray[i].LastWriteTime = channel.Read<FileTime>();
                                    bothInfoArray[i].LastChangeTime = channel.Read<FileTime>();
                                    bothInfoArray[i].EndOfFile = channel.Read<ulong>();
                                    bothInfoArray[i].AllocationSize = channel.Read<ulong>();
                                    bothInfoArray[i].FileAttributes = channel.Read<SmbFileAttributes32>();
                                    bothInfoArray[i].FileNameLength = channel.Read<uint>();
                                    bothInfoArray[i].EaSize = channel.Read<uint>();
                                    bothInfoArray[i].ShortNameLength = channel.Read<byte>();
                                    bothInfoArray[i].Reserved = channel.Read<byte>();
                                    bothInfoArray[i].ShortName = channel.ReadBytes(24);
                                    bothInfoArray[i].FileName = channel.ReadBytes((int)bothInfoArray[i].FileNameLength);
                                    fixedSize += bothInfoArray[i].FileNameLength;// Add the length of file name.
                                    int pad = (int)(bothInfoArray[i].NextEntryOffset - fixedSize);

                                    if (pad > 0)
                                    {
                                        channel.ReadBytes(pad);
                                    }
                                }
                                this.trans2Data.Data = bothInfoArray;
                                break;

                            case FindInformationLevel.SMB_FIND_FILE_NAMES_INFO:
                                SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2[] namesInfoArray =
                                    new SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2[this.trans2Parameters.SearchCount];

                                for (int i = 0; i < namesInfoArray.Length; i++)
                                {
                                    uint fixedSize = 12; // Including the fixed length of this structure.
                                    namesInfoArray[i] = channel.Read<SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2>();
                                    fixedSize += namesInfoArray[i].FileNameLength; // Add the length of file name.
                                    int pad = (int)(namesInfoArray[i].NextEntryOffset - fixedSize);

                                    if (pad > 0)
                                    {
                                        channel.ReadBytes(pad);
                                    }
                                }
                                this.trans2Data.Data = namesInfoArray;
                                break;
                        }
                    }
                }
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
        }

        #endregion
    }
}