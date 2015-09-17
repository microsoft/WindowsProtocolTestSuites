// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtTransactCreate Request
    /// </summary>
    public class SmbNtTransactCreateRequestPacket : SmbNtTransactRequestPacket
    {
        #region Fields

        private NT_TRANSACT_CREATE_Request_NT_Trans_Parameters ntTransParameters;
        private NT_TRANSACT_CREATE_Request_NT_Trans_Data ntTransData;
        // Size of padding data before Name field in NT_Trans_Parameters structure.
        private const byte PaddingSize = 1;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Parameters:NT_TRANSACT_CREATE_Request_NT_Trans_Parameters
        /// </summary>
        public NT_TRANSACT_CREATE_Request_NT_Trans_Parameters NtTransParameters
        {
            get
            {
                return this.ntTransParameters;
            }
            set
            {
                this.ntTransParameters = value;
            }
        }


        /// <summary>
        /// get or set the NT_Trans_Data:NT_TRANSACT_CREATE_Request_NT_Trans_Data
        /// </summary>
        public NT_TRANSACT_CREATE_Request_NT_Trans_Data NtTransData
        {
            get
            {
                return this.ntTransData;
            }
            set
            {
                this.ntTransData = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbNtTransactCreateRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtTransactCreateRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtTransactCreateRequestPacket(SmbNtTransactCreateRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.ntTransParameters.Flags = packet.ntTransParameters.Flags;
            this.ntTransParameters.RootDirectoryFID = packet.ntTransParameters.RootDirectoryFID;
            this.ntTransParameters.DesiredAccess = packet.ntTransParameters.DesiredAccess;
            this.ntTransParameters.AllocationSize = packet.ntTransParameters.AllocationSize;
            this.ntTransParameters.ExtFileAttributes = packet.ntTransParameters.ExtFileAttributes;
            this.ntTransParameters.ShareAccess = packet.ntTransParameters.ShareAccess;
            this.ntTransParameters.CreateDisposition = packet.ntTransParameters.CreateDisposition;
            this.ntTransParameters.CreateOptions = packet.ntTransParameters.CreateOptions;
            this.ntTransParameters.SecurityDescriptorLength = packet.ntTransParameters.SecurityDescriptorLength;
            this.ntTransParameters.EALength = packet.ntTransParameters.EALength;
            this.ntTransParameters.NameLength = packet.ntTransParameters.NameLength;
            this.ntTransParameters.ImpersonationLevel = packet.ntTransParameters.ImpersonationLevel;
            this.ntTransParameters.SecurityFlags = packet.ntTransParameters.SecurityFlags;

            if (packet.ntTransParameters.Name != null)
            {
                this.ntTransParameters.Name = new byte[packet.ntTransParameters.NameLength];
                Array.Copy(packet.ntTransParameters.Name,
                    this.ntTransParameters.Name, packet.ntTransParameters.NameLength);
            }
            else
            {
                this.ntTransParameters.Name = new byte[0];
            }
            this.ntTransData.SecurityDescriptor = packet.ntTransData.SecurityDescriptor;

            if (packet.ntTransData.ExtendedAttributes != null)
            {
                this.ntTransData.ExtendedAttributes = new FILE_FULL_EA_INFORMATION[packet.ntTransData.ExtendedAttributes.Length];
                Array.Copy(packet.ntTransData.ExtendedAttributes,
                    this.ntTransData.ExtendedAttributes, packet.ntTransData.ExtendedAttributes.Length);
            }
            else
            {
                this.ntTransData.ExtendedAttributes = new FILE_FULL_EA_INFORMATION[0];
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
            return new SmbNtTransactCreateRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters
        /// </summary>
        protected override void EncodeNtTransParameters()
        {
            if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                int ntTransParameterSize = CifsMessageUtils.GetSize<NT_TRANSACT_CREATE_Request_NT_Trans_Parameters>(
                    this.ntTransParameters) + PaddingSize;
                this.smbData.NT_Trans_Parameters = new byte[ntTransParameterSize];

                using (MemoryStream memoryStream = new MemoryStream(this.smbData.NT_Trans_Parameters))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        channel.Write<NtTransactFlags>(this.ntTransParameters.Flags);
                        channel.Write<uint>(this.ntTransParameters.RootDirectoryFID);
                        channel.Write<NtTransactDesiredAccess>(this.ntTransParameters.DesiredAccess);
                        channel.Write<ulong>(this.ntTransParameters.AllocationSize);
                        channel.Write<SMB_EXT_FILE_ATTR>(this.ntTransParameters.ExtFileAttributes);
                        channel.Write<NtTransactShareAccess>(this.ntTransParameters.ShareAccess);
                        channel.Write<NtTransactCreateDisposition>(this.ntTransParameters.CreateDisposition);
                        channel.Write<NtTransactCreateOptions>(this.ntTransParameters.CreateOptions);
                        channel.Write<uint>(this.ntTransParameters.SecurityDescriptorLength);
                        channel.Write<uint>(this.ntTransParameters.EALength);
                        channel.Write<uint>(this.ntTransParameters.NameLength);
                        channel.Write<NtTransactImpersonationLevel>(this.ntTransParameters.ImpersonationLevel);
                        channel.Write<NtTransactSecurityFlags>(this.ntTransParameters.SecurityFlags);
                        // Padding data
                        channel.WriteBytes(new byte[PaddingSize]);
                        channel.WriteBytes(this.ntTransParameters.Name);
                        channel.EndWriteGroup();
                    }
                }
            }
            else
            {
                this.smbData.NT_Trans_Parameters =
                    CifsMessageUtils.ToBytes<NT_TRANSACT_CREATE_Request_NT_Trans_Parameters>(this.ntTransParameters);
            }
        }


        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data
        /// </summary>
        protected override void EncodeNtTransData()
        {
            byte[] securicyInformation = null;

            if (this.ntTransData.SecurityDescriptor != null)
            {
                securicyInformation = new byte[this.ntTransData.SecurityDescriptor.BinaryLength];
                this.ntTransData.SecurityDescriptor.GetBinaryForm(securicyInformation, 0);
                this.smbData.NT_Trans_Data = securicyInformation;
            }
            else
            {
                securicyInformation = new byte[0];
            }
            this.smbData.NT_Trans_Data = new byte[this.ntTransParameters.SecurityDescriptorLength +
                this.ntTransParameters.EALength];
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.NT_Trans_Data))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.WriteBytes(securicyInformation);

                    if (this.ntTransData.ExtendedAttributes != null)
                    {
                        for (int i = 0; i < this.ntTransData.ExtendedAttributes.Length; i++)
                        {
                            channel.Write<uint>(this.ntTransData.ExtendedAttributes[i].NextEntryOffset);
                            channel.Write<FULL_EA_FLAGS>(this.ntTransData.ExtendedAttributes[i].Flags);
                            channel.Write<byte>(this.ntTransData.ExtendedAttributes[i].EaNameLength);
                            channel.Write<ushort>(this.ntTransData.ExtendedAttributes[i].EaValueLength);
                            int size = 0;

                            if (this.ntTransData.ExtendedAttributes[i].EaName != null)
                            {
                                channel.WriteBytes(this.ntTransData.ExtendedAttributes[i].EaName);
                                size += this.ntTransData.ExtendedAttributes[i].EaName.Length;
                            }
                            if (this.ntTransData.ExtendedAttributes[i].EaValue != null)
                            {
                                channel.WriteBytes(this.ntTransData.ExtendedAttributes[i].EaValue);
                                size += this.ntTransData.ExtendedAttributes[i].EaValue.Length;
                            }
                            int pad = (int)(this.ntTransData.ExtendedAttributes[i].NextEntryOffset - EA.FULL_EA_FIXED_SIZE - size);

                            if (pad > 0)
                            {
                                channel.WriteBytes(new byte[pad]);
                            }
                        }
                    }
                    channel.EndWriteGroup();
                }
            }
        }


        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters.
        /// </summary>
        protected override void DecodeNtTransParameters()
        {
            if (this.smbData.NT_Trans_Parameters != null)
            {
                if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                {
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.NT_Trans_Parameters))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            this.ntTransParameters.Flags = channel.Read<NtTransactFlags>();
                            this.ntTransParameters.RootDirectoryFID = channel.Read<uint>();
                            this.ntTransParameters.DesiredAccess = channel.Read<NtTransactDesiredAccess>();
                            this.ntTransParameters.AllocationSize = channel.Read<ulong>();
                            this.ntTransParameters.ExtFileAttributes = channel.Read<SMB_EXT_FILE_ATTR>();
                            this.ntTransParameters.ShareAccess = channel.Read<NtTransactShareAccess>();
                            this.ntTransParameters.CreateDisposition = channel.Read<NtTransactCreateDisposition>();
                            this.ntTransParameters.CreateOptions = channel.Read<NtTransactCreateOptions>();
                            this.ntTransParameters.SecurityDescriptorLength = channel.Read<uint>();
                            this.ntTransParameters.EALength = channel.Read<uint>();
                            this.ntTransParameters.NameLength = channel.Read<uint>();
                            this.ntTransParameters.ImpersonationLevel = channel.Read<NtTransactImpersonationLevel>();
                            this.ntTransParameters.SecurityFlags = channel.Read<NtTransactSecurityFlags>();
                            // Padding data
                            channel.ReadBytes(PaddingSize);
                            this.ntTransParameters.Name = channel.ReadBytes((int)this.ntTransParameters.NameLength);
                        }
                    }
                }
                else
                {
                    this.ntTransParameters = TypeMarshal.ToStruct<NT_TRANSACT_CREATE_Request_NT_Trans_Parameters>(
                        this.smbData.NT_Trans_Parameters);
                }
            }
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data.
        /// </summary>
        protected override void DecodeNtTransData()
        {
            if (this.smbData.NT_Trans_Data != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.NT_Trans_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        if (this.ntTransParameters.SecurityDescriptorLength > 0)
                        {
                            byte[] securicyInformation = channel.ReadBytes((int)this.ntTransParameters.SecurityDescriptorLength);
                            this.ntTransData.SecurityDescriptor = new RawSecurityDescriptor(securicyInformation, 0);
                        }
                        uint sizeOfListInBytes = this.ntTransParameters.EALength;
                        List<FILE_FULL_EA_INFORMATION> attributeList = new List<FILE_FULL_EA_INFORMATION>();

                        while (sizeOfListInBytes > 0)
                        {
                            FILE_FULL_EA_INFORMATION eaInformation = channel.Read<FILE_FULL_EA_INFORMATION>();
                            attributeList.Add(eaInformation);
                            sizeOfListInBytes -= (uint)(EA.FULL_EA_FIXED_SIZE + eaInformation.EaName.Length +
                                eaInformation.EaValue.Length);
                        }
                        this.ntTransData.ExtendedAttributes = attributeList.ToArray();
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