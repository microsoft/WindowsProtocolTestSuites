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
    ///  Packets for SmbNtTransactNotifyChange Response
    /// </summary>
    public class SmbNtTransactNotifyChangeResponsePacket : SmbNtTransactSuccessResponsePacket
    {
        #region Fields

        private NT_TRANSACT_NOTIFY_CHANGE_Response_NT_Trans_Parameters ntTransParameters;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Parameters:NT_TRANSACT_NOTIFY_CHANGE_Response_NT_Trans_Parameters
        /// </summary>
        public NT_TRANSACT_NOTIFY_CHANGE_Response_NT_Trans_Parameters NtTransParameters
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

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbNtTransactNotifyChangeResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtTransactNotifyChangeResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtTransactNotifyChangeResponsePacket(SmbNtTransactNotifyChangeResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.ntTransParameters.FileNotifyInformation = packet.ntTransParameters.FileNotifyInformation;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbNtTransactNotifyChangeResponsePacket(this);
        }


        /// <summary>
        /// to update SmbParameters and SmbData automatically.
        /// </summary>
        public override void UpdateCountAndOffset()
        {
            if (this.ntTransParameters.FileNotifyInformation != null)
            {
                for (int i = 0; i < this.ntTransParameters.FileNotifyInformation.Length; i++)
                {
                    if (i == this.ntTransParameters.FileNotifyInformation.Length - 1)
                    {
                        this.ntTransParameters.FileNotifyInformation[i].NextEntryOffset = 0x00000000;
                    }
                    else
                    {
                        this.ntTransParameters.FileNotifyInformation[i].NextEntryOffset = (uint)((
                            TypeMarshal.GetBlockMemorySize(this.ntTransParameters.FileNotifyInformation[i]) + 3) & ~3);
                    }
                }
            }
            
            base.UpdateCountAndOffset();
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters
        /// </summary>
        protected override void EncodeNtTransParameters()
        {
            if (this.ntTransParameters.FileNotifyInformation != null &&
                this.ntTransParameters.FileNotifyInformation.Length > 0)
            {
                uint size = 0;
                ushort notifyInfoFixedLength = 12;

                foreach (FILE_NOTIFY_INFORMATION fileNotifyInformation in this.ntTransParameters.FileNotifyInformation)
                {
                    if (fileNotifyInformation.NextEntryOffset != 0)
                    {
                        size += fileNotifyInformation.NextEntryOffset;
                    }
                    else
                    {
                        size += notifyInfoFixedLength + fileNotifyInformation.FileNameLength;
                    }
                }
                this.smbData.Parameters = new byte[size];
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Parameters))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        foreach (FILE_NOTIFY_INFORMATION fileNotifyInformation in this.ntTransParameters.FileNotifyInformation)
                        {
                            channel.Write<FILE_NOTIFY_INFORMATION>(fileNotifyInformation);
                            int padLength = (int)(fileNotifyInformation.NextEntryOffset - notifyInfoFixedLength
                                - fileNotifyInformation.FileNameLength);

                            if (padLength > 0)
                            {
                                channel.WriteBytes(new byte[padLength]);
                            }
                        }
                        channel.EndWriteGroup();
                    }
                }
            }
        }


        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data
        /// </summary>
        protected override void EncodeNtTransData()
        {
        }


        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters.
        /// </summary>
        protected override void DecodeNtTransParameters()
        {
            if (this.smbParameters.ParameterCount > 0)
            {
                List<FILE_NOTIFY_INFORMATION> list = new List<FILE_NOTIFY_INFORMATION>();
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Parameters))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        FILE_NOTIFY_INFORMATION firstNotifyInformation = channel.Read<FILE_NOTIFY_INFORMATION>();
                        list.Add(firstNotifyInformation);
                        ushort notifyInfoFixedLength = 12;
                        uint nextEntryOffset = firstNotifyInformation.NextEntryOffset;
                        uint nameLength = firstNotifyInformation.FileNameLength;

                        while (nextEntryOffset != 0)
                        {
                            channel.ReadBytes((int)(nextEntryOffset - notifyInfoFixedLength - nameLength));
                            FILE_NOTIFY_INFORMATION nextNotifyInformation = channel.Read<FILE_NOTIFY_INFORMATION>();
                            nextEntryOffset = nextNotifyInformation.NextEntryOffset;
                            nameLength = nextNotifyInformation.FileNameLength;
                            list.Add(nextNotifyInformation);
                        }
                        this.ntTransParameters.FileNotifyInformation = list.ToArray();
                    }
                }
            }
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data.
        /// </summary>
        protected override void DecodeNtTransData()
        {
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