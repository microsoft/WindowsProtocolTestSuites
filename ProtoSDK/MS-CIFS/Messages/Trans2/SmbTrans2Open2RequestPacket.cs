// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTrans2Open2 Request
    /// </summary>
    public class SmbTrans2Open2RequestPacket : SmbTransaction2RequestPacket
    {
        #region Fields

        private TRANS2_OPEN2_Request_Trans2_Parameters trans2Parameters;
        private TRANS2_OPEN2_Request_Trans2_Data trans2Data;
        /// <summary>
        /// The size of SizeOfListInBytes field in trans2Data
        /// </summary>
        private const ushort sizeOfListInBytesLength = 4;

        /// <summary>
        /// Including the total size in bytes of fixed fields in Trans2_Parameters
        /// </summary>
        private const ushort trans2ParametersLength = 28;

        #endregion


        #region Properties


        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_OPEN2_Request_Trans2_Parameters
        /// </summary>
        public TRANS2_OPEN2_Request_Trans2_Parameters Trans2Parameters
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
        /// get or set the Trans2_Data:TRANS2_OPEN2_Request_Trans2_Data
        /// </summary>
        public TRANS2_OPEN2_Request_Trans2_Data Trans2Data
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


        /// <summary>
        /// get the FID of Trans2_Parameters
        /// </summary>
        internal override ushort FID
        {
            get
            {
                return INVALID_FID;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTrans2Open2RequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2Open2RequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2Open2RequestPacket(SmbTrans2Open2RequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.trans2Parameters.Flags = packet.trans2Parameters.Flags;
            this.trans2Parameters.AccessMode = packet.trans2Parameters.AccessMode;
            this.trans2Parameters.Reserved1 = packet.trans2Parameters.Reserved1;
            this.trans2Parameters.FileAttributes = packet.trans2Parameters.FileAttributes;
            this.trans2Parameters.CreationTime = packet.trans2Parameters.CreationTime;
            this.trans2Parameters.OpenMode = packet.trans2Parameters.OpenMode;
            this.trans2Parameters.AllocationSize = packet.trans2Parameters.AllocationSize;
            this.trans2Parameters.Reserved = new ushort[5];

            if (packet.trans2Parameters.Reserved != null)
            {
                Array.Copy(packet.trans2Parameters.Reserved, this.trans2Parameters.Reserved, 5);
            }
            if (packet.trans2Parameters.FileName != null)
            {
                trans2Parameters.FileName = new byte[packet.trans2Parameters.FileName.Length];
                Array.Copy(packet.trans2Parameters.FileName,
                    this.trans2Parameters.FileName, trans2Parameters.FileName.Length);
            }
            else
            {
                this.trans2Parameters.FileName = new byte[0];
            }
            this.trans2Data.ExtendedAttributeList.SizeOfListInBytes = 
                packet.trans2Data.ExtendedAttributeList.SizeOfListInBytes;

            if (packet.trans2Data.ExtendedAttributeList.FEAList != null)
            {
                this.trans2Data.ExtendedAttributeList.FEAList = 
                    new SMB_FEA[packet.trans2Data.ExtendedAttributeList.FEAList.Length];

                Array.Copy(packet.trans2Data.ExtendedAttributeList.FEAList,
                    this.trans2Data.ExtendedAttributeList.FEAList, packet.trans2Data.ExtendedAttributeList.FEAList.Length);
            }
            else
            {
                this.trans2Data.ExtendedAttributeList.FEAList = new SMB_FEA[0];
            }
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2Open2RequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            int trans2ParametersSize = trans2ParametersLength;

            if (this.trans2Parameters.FileName != null)
            {
                trans2ParametersSize += this.trans2Parameters.FileName.Length;
            }
            this.smbData.Trans2_Parameters = new byte[trans2ParametersSize];
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<ushort>(this.trans2Parameters.Flags);
                    channel.Write<ushort>(this.trans2Parameters.AccessMode);
                    channel.Write<ushort>(this.trans2Parameters.Reserved1);
                    channel.Write<SmbFileAttributes>(this.trans2Parameters.FileAttributes);
                    channel.Write<uint>(this.trans2Parameters.CreationTime);
                    channel.Write<ushort>(this.trans2Parameters.OpenMode);
                    channel.Write<uint>(this.trans2Parameters.AllocationSize);
                    byte[] reserved = CifsMessageUtils.ToBytesArray(this.trans2Parameters.Reserved);
                    channel.WriteBytes(reserved);

                    if (this.trans2Parameters.FileName != null)
                    {
                        channel.WriteBytes(this.trans2Parameters.FileName);
                    }
                    channel.EndWriteGroup();
                }
            }
        }


        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data
        /// </summary>
        protected override void EncodeTrans2Data()
        {
            this.smbData.Trans2_Data = new byte[sizeOfListInBytesLength + CifsMessageUtils.GetSmbEAListSize
                (this.trans2Data.ExtendedAttributeList.FEAList)];
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<uint>(this.trans2Data.ExtendedAttributeList.SizeOfListInBytes);

                    if (this.trans2Data.ExtendedAttributeList.FEAList != null)
                    {
                        foreach (SMB_FEA smbEa in this.trans2Data.ExtendedAttributeList.FEAList)
                        {
                            channel.Write<byte>(smbEa.ExtendedAttributeFlag);
                            channel.Write<byte>(smbEa.AttributeNameLengthInBytes);
                            channel.Write<ushort>(smbEa.ValueNameLengthInBytes);

                            if (smbEa.AttributeName != null)
                            {
                                channel.WriteBytes(smbEa.AttributeName);
                            }
                            if (smbEa.ValueName != null)
                            {
                                channel.WriteBytes(smbEa.ValueName);
                            }
                        }
                    }
                    channel.EndWriteGroup();
                }
            }
        }


        /// <summary>
        /// to decode the Trans2 parameters: from the general Trans2Parameters to the concrete Trans2 Parameters.
        /// </summary>
        protected override void DecodeTrans2Parameters()
        {
            if (this.smbData.Trans2_Parameters != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.trans2Parameters.Flags = channel.Read<ushort>();
                        this.trans2Parameters.AccessMode = channel.Read<ushort>();
                        this.trans2Parameters.Reserved1 = channel.Read<ushort>();
                        this.trans2Parameters.FileAttributes = channel.Read<SmbFileAttributes>();
                        this.trans2Parameters.CreationTime = channel.Read<uint>();
                        this.trans2Parameters.OpenMode = channel.Read<ushort>();
                        this.trans2Parameters.AllocationSize = channel.Read<uint>();
                        this.trans2Parameters.Reserved = new ushort[5];

                        for (int i = 0; i < 5; i++)
                        {
                            this.trans2Parameters.Reserved[i] = channel.Read<ushort>();
                        }
                        this.trans2Parameters.FileName = channel.ReadBytes(this.smbParameters.ParameterCount
                            - trans2ParametersLength);
                    }
                }
            }
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected override void DecodeTrans2Data()
        {
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.trans2Data.ExtendedAttributeList.SizeOfListInBytes = channel.Read<uint>();
                    uint sizeOfListInBytes = 
                        this.trans2Data.ExtendedAttributeList.SizeOfListInBytes - sizeOfListInBytesLength;
                    List<SMB_FEA> attributeList = new List<SMB_FEA>();

                    while (sizeOfListInBytes > 0)
                    {
                        SMB_FEA smbEa = channel.Read<SMB_FEA>();
                        attributeList.Add(smbEa);
                        sizeOfListInBytes -= (uint)(EA.SMB_EA_FIXED_SIZE + smbEa.AttributeName.Length + smbEa.ValueName.Length);
                    }
                    this.trans2Data.ExtendedAttributeList.FEAList = attributeList.ToArray();
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