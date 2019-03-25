// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    /// <summary>
    /// Process received data
    /// </summary>
    /// <param name="data"></param>
    public delegate void ReceiveData(byte[] data, uint channelId);

    public class DynamicVirtualChannel
    {
        #region Variables


        /// <summary>
        /// Channel Id;
        /// </summary>
        private UInt32 channelId;

        /// <summary>
        /// channel Name
        /// </summary>
        private string channelName;

        /// <summary>
        /// Priority
        /// </summary>
        private ushort priority;

        /// <summary>
        /// Transport of this dynamic virtual channel
        /// </summary>
        private IDVCTransport transport;

        private PduBuilder pduBuilder;

        /// <summary>
        /// Fragment manager used to reassemble DVC data
        /// </summary>
        private DataFragmentManager dataFragmentManager;

        private static UInt32 CurrentChannelId = 0;
        private static readonly object curChannelIdLock = new object();

        #endregion Variables

        #region Properties

        /// <summary>
        /// Channel ID
        /// </summary>
        public UInt32 ChannelId
        {
            get
            {
                return channelId;
            }
        }

        /// <summary>
        /// Channel Name
        /// </summary>
        public string ChannelName
        {
            get
            {
                return channelName;
            }
        }

        /// <summary>
        /// Priority
        /// </summary>
        public ushort Priority
        {
            get
            {
                return priority;
            }
        }

        /// <summary>
        /// Transport type
        /// </summary>
        public DynamicVC_TransportType TransportType
        {
            get
            {
                return transport.TransportType;
            }
        }

        public bool IsActive { set; get; }

        #endregion Properties

        #region Constructor

        public DynamicVirtualChannel(UInt32 channelId, string channelName, ushort priority, IDVCTransport transport)
        {
            this.channelId = channelId;
            this.channelName = channelName;
            this.priority = priority;
            this.transport = transport;
            
            pduBuilder = new PduBuilder();
            IsActive = true;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Send data using this DVC
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data, bool isCompressed = false)
        {
            DataDvcBasePdu[] dataPdus = null;
            if (isCompressed)
            {
                dataPdus = pduBuilder.CreateCompressedDataPdu(
                    channelId, 
                    data);
            }
            else
            {
                dataPdus = pduBuilder.CreateDataPdu(channelId, data, ConstLength.MAX_CHUNK_LEN);
            }

            if (dataPdus != null)
            {
                foreach (DataDvcBasePdu pdu in dataPdus)
                {
                    transport.Send(pdu);
                }
            }
        }

        public void SendFirstCompressedData(byte[] data)
        {
            DataDvcBasePdu[] dataPdus = null;
            
            dataPdus = pduBuilder.CreateCompressedDataPdu(
                channelId,
                pduBuilder.CompressDataToRdp8BulkEncodedData(data, PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED));
           

            if (dataPdus != null)
            {
                foreach (DataDvcBasePdu pdu in dataPdus)
                {
                    transport.Send(pdu);
                }
            }
        }

        /// <summary>
        /// Event used to process received DVC data
        /// </summary>
        public event ReceiveData Received;

        /// <summary>
        /// Process DVC packet, only process Data on this channel
        /// </summary>
        /// <param name="pdu"></param>
        public void ProcessPacket(DataDvcBasePdu pdu)
        {
            if (pdu.ChannelId == this.channelId)
            {
                ProcessDataPdu(pdu as DataDvcBasePdu);
            }
        }

        /// <summary>
        /// Get a new unused channel id
        /// </summary>
        /// <returns></returns>
        public static UInt32 NewChannelId()
        {
            uint newChannelId = 0;
            lock (curChannelIdLock)
            {
                newChannelId = ++CurrentChannelId;
            }
            return newChannelId;
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Process DVC Data
        /// </summary>
        /// <param name="pdu"></param>
        private void ProcessDataPdu(DataDvcBasePdu pdu)
        {
            if (pdu is DataFirstDvcPdu)
            {
                dataFragmentManager = new DataFragmentManager();
                DataFirstDvcPdu first = (DataFirstDvcPdu)pdu;
                dataFragmentManager.AddFirstData(first.Length, first.Data);
            }
            else if (pdu is DataDvcPdu)
            {
                if (dataFragmentManager == null)
                {
                    // Single data PDU which is not fragmented.
                    if (this.Received != null)
                    {
                        this.Received(pdu.Data, this.ChannelId);
                    }
                    return;
                }
                else
                {
                    // Received a fragment.
                    dataFragmentManager.AppendData(pdu.Data);
                }
            }

            if (dataFragmentManager.Completed)
            {
                if (this.Received != null)
                {
                    this.Received(dataFragmentManager.Data, this.ChannelId);
                }
                dataFragmentManager = null;
            }
        }

        #endregion Private Methods
    }
}
