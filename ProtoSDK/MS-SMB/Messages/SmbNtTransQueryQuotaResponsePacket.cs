// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbNtTransQueryQuota Response 
    /// </summary>
    public class SmbNtTransQueryQuotaResponsePacket : SmbNtTransactSuccessResponsePacket
    {
        #region Fields

        private NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Parameters ntTransParameters;
        private Collection<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data> ntTransDataList;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the ntTransParameters:NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Parameters 
        /// </summary>
        public NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Parameters NtTransParameters
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
        /// get or set the NT_Trans_Data:NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data last 
        /// </summary>
        public NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data FirstNtTransData
        {
            get
            {
                if (this.NtTransDataList.Count > 0)
                {
                    return this.NtTransDataList[0];
                }

                return new NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data();
            }
            set
            {
                this.NtTransDataList[0] = value;
            }
        }


        /// <summary>
        /// get or set the NT_Trans_Data:NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data last 
        /// </summary>
        public NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data LastNtTransData
        {
            get
            {
                if (this.NtTransDataList.Count > 0)
                {
                    return this.NtTransDataList[this.NtTransDataList.Count - 1];
                }

                return new NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data();
            }
            set
            {
                this.NtTransDataList[0] = value;
            }
        }


        /// <summary>
        /// get or set the NT_Trans_Data:NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data list 
        /// </summary>
        public Collection<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data> NtTransDataList
        {
            get
            {
                if (this.ntTransDataList == null)
                {
                    this.ntTransDataList = new Collection<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data>();
                }

                return this.ntTransDataList;
            }
            set
            {
                this.ntTransDataList = value;
            }
        }


        #endregion


        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtTransQueryQuotaResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtTransQueryQuotaResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtTransQueryQuotaResponsePacket(SmbNtTransQueryQuotaResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
        }


        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this. </returns>
        public override StackPacket Clone()
        {
            return new SmbNtTransQueryQuotaResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters 
        /// </summary>
        protected override void EncodeNtTransParameters()
        {
            this.smbData.Parameters =
          CifsMessageUtils.ToBytes<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Parameters>(
          this.ntTransParameters);
        }


        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data 
        /// </summary>
        protected override void EncodeNtTransData()
        {
            List<byte> bytes = new List<byte>();

            foreach (NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data data in this.NtTransDataList)
            {
                bytes.AddRange(
                    CifsMessageUtils.ToBytes<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data>(data));
            }

            this.smbData.Data = bytes.ToArray();
        }


        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters. 
        /// </summary>
        protected override void DecodeNtTransParameters()
        {
            this.ntTransParameters =
            CifsMessageUtils.ToStuct<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Parameters>(
            this.smbData.Parameters);
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data. 
        /// </summary>
        protected override void DecodeNtTransData()
        {
            NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data data = new NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data();
            int start = 0;
            byte[] bytes = this.smbData.Data;

            do
            {
                data = CifsMessageUtils.ToStuct<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data>(bytes);
                this.NtTransDataList.Add(data);

                start += (int)data.NextEntryOffset;
                bytes = new byte[this.smbData.Data.Length - start];
                Array.Copy(this.smbData.Data, start, bytes, 0, bytes.Length);
            }
            while(data.NextEntryOffset != 0);
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
