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
    /// Packets for SmbTrans2CreateDirectoryFinal Request
    /// </summary>
    public class SmbTrans2CreateDirectoryRequestPacket : SmbTransaction2RequestPacket
    {
        #region Fields

        private TRANS2_CREATE_DIRECTORY_Request_Trans2_Parameters trans2Parameters;
        private TRANS2_CREATE_DIRECTORY_Request_Trans2_Data trans2Data;
        /// <summary>
        /// The size of Reserved field in trans2Parameters
        /// </summary>
        private const ushort reservedLength = 4;
        /// <summary>
        /// The size of SizeOfListInBytes field in trans2Data
        /// </summary>
        private const ushort sizeOfListInBytesLength = 4;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_CREATE_DIRECTORY_Request_Trans2_Parameters
        /// </summary>
        public TRANS2_CREATE_DIRECTORY_Request_Trans2_Parameters Trans2Parameters
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
        /// get or set the Trans2_Data:TRANS2_CREATE_DIRECTORY_Request_Trans2_Data
        /// </summary>
        public TRANS2_CREATE_DIRECTORY_Request_Trans2_Data Trans2Data
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
        public SmbTrans2CreateDirectoryRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }

        
        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2CreateDirectoryRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2CreateDirectoryRequestPacket(SmbTrans2CreateDirectoryRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.trans2Parameters.Reserved = packet.trans2Parameters.Reserved;
            this.trans2Parameters.DirectoryName = new byte[packet.trans2Parameters.DirectoryName.Length];
            Array.Copy(packet.trans2Parameters.DirectoryName,
                this.trans2Parameters.DirectoryName, packet.trans2Parameters.DirectoryName.Length);

            this.trans2Data.ExtendedAttributeList.SizeOfListInBytes =
                packet.trans2Data.ExtendedAttributeList.SizeOfListInBytes;

            if (packet.trans2Data.ExtendedAttributeList.FEAList != null)
            {
                this.trans2Data.ExtendedAttributeList.FEAList =
                    new SMB_FEA[packet.trans2Data.ExtendedAttributeList.FEAList.Length];
                Array.Copy(packet.trans2Data.ExtendedAttributeList.FEAList,
                    this.trans2Data.ExtendedAttributeList.FEAList,
                    packet.trans2Data.ExtendedAttributeList.FEAList.Length);
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
            return new SmbTrans2CreateDirectoryRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            int trans2ParametersCount = reservedLength;

            if (this.trans2Parameters.DirectoryName != null)
            {
                trans2ParametersCount += this.trans2Parameters.DirectoryName.Length;
            }
            this.smbData.Trans2_Parameters = new byte[trans2ParametersCount];
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<uint>(this.trans2Parameters.Reserved);

                    if (this.trans2Parameters.DirectoryName != null)
                    {
                        channel.WriteBytes(this.trans2Parameters.DirectoryName);
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
            this.smbData.Trans2_Data = new byte[sizeOfListInBytesLength + CifsMessageUtils.GetSmbEAListSize(
                this.trans2Data.ExtendedAttributeList.FEAList)];
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
                        this.trans2Parameters.Reserved = channel.Read<uint>();
                        this.trans2Parameters.DirectoryName = channel.ReadBytes(this.smbParameters.ParameterCount
                            - reservedLength);
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
                        sizeOfListInBytes -= (uint)(EA.SMB_EA_FIXED_SIZE + smbEa.AttributeName.Length +
                            smbEa.ValueName.Length);
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