// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the server decode packet, export for smb using.
    /// </summary>
    public class CifsServerDecodePacket
    {
        #region Fields

        /// <summary>
        /// the context of smb server.
        /// </summary>
        protected CifsServerContext serverContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected CifsServerDecodePacket()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the context must not be null.</exception>
        public CifsServerDecodePacket(CifsServerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.serverContext = context;
        }


        #endregion

        #region Decode callback methods

        /// <summary>
        /// to decode stack packet from the received message bytes. 
        /// </summary>
        /// <param name = "endPoint">the endpoint from which the message bytes are received. </param>
        /// <param name = "messageBytes">the received message bytes to be decoded. </param>
        /// <param name = "consumedLength">the length of message bytes consumed by decoder. </param>
        /// <param name = "expectedLength">the length of message bytes the decoder expects to receive. </param>
        /// <returns>the stack packets decoded from the received message bytes. </returns>
        public StackPacket[] DecodePacketCallback(
            object endPoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            // initialize the default values
            consumedLength = 0;
            expectedLength = 0;
            SmbPacket[] packets = new SmbPacket[0];

            SmbPacket request = this.DecodeSmbRequestFromBytes(messageBytes, out consumedLength);
            // Use the decoded packet to UpdateRoleContext if it is not null and ContextUpdate is enabled:
            if (request != null)
            {
                if (this.serverContext.IsContextUpdateEnabled)
                {
                    this.serverContext.UpdateRoleContext(this.serverContext.ConnectionTable[endPoint], request);
                }
                packets = new SmbPacket[] { request };
            }

            expectedLength = 0;
            return packets;
        }


        /// <summary>
        /// decode packet from bytes
        /// </summary>
        /// <param name="messageBytes">bytes contains packet</param>
        /// <param name="consumedLength">the bytes length which are consumed when decode.</param>
        /// <returns>the decoded packet from the bytes array. if failed, return null.</returns>
        protected SmbPacket DecodeSmbRequestFromBytes(
            byte[] messageBytes,
            out int consumedLength)
        {
            consumedLength = 0;
            SmbPacket smbRequest = null;

            using (MemoryStream memoryStream = new MemoryStream(messageBytes, false))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    smbRequest = CreateSmbRequestPacket(messageBytes);
                    if (smbRequest == null)
                    {
                        return null;
                    }

                    // decode smb header
                    SmbHeader smbHeader = channel.Read<SmbHeader>();
                    consumedLength += smbRequest.HeaderSize;

                    // set packet header
                    smbRequest.SmbHeader = smbHeader;

                    // read SmbParameters:
                    consumedLength += smbRequest.ReadParametersFromChannel(channel);

                    // read SmbData:
                    consumedLength += smbRequest.ReadDataFromChannel(channel);

                    // read andx:
                    SmbBatchedRequestPacket smbBatchedRequest = smbRequest as SmbBatchedRequestPacket;
                    if (smbBatchedRequest != null)
                    {
                        consumedLength += DecodeBatchedRequest(channel, smbBatchedRequest);
                    }
                }
            }
            return smbRequest;
        }


        /// <summary>
        /// decode the batched request packet
        /// </summary>
        /// <param name="channel">the channel of bytes to read</param>
        /// <param name="smbBatchedRequest">the batched request</param>
        /// <returns>the consumed length of batched response packet</returns>
        protected virtual int DecodeBatchedRequest(
            Channel channel, SmbBatchedRequestPacket smbBatchedRequest)
        {
            int batchedConsumedLength = 0;

            batchedConsumedLength += smbBatchedRequest.ReadAndxFromChannel(channel);

            return batchedConsumedLength;
        }


        /// <summary>
        /// to new a Smb request packet in type of the Command in SmbHeader.
        /// </summary>
        /// <param name="messageBytes">bytes contains packet</param>
        /// <returns>
        /// the new request packet. 
        /// the null means that the utility don't know how to create the request.
        /// </returns>
        protected virtual SmbPacket CreateSmbRequestPacket(byte[] messageBytes)
        {
            SmbPacket smbRequest = null;

            using (MemoryStream stream = new MemoryStream(messageBytes, true))
            {
                using (Channel channel = new Channel(null, stream))
                {
                    //read smb header and new SmbPacket
                    if (messageBytes.Length >= CifsMessageUtils.PACKET_MINIMUM_LENGTH)
                    {
                        SmbHeader smbHeader = channel.Read<SmbHeader>();
                        smbRequest = CifsMessageUtils.CreateSmbRequestPacket(smbHeader, channel);
                    }
                }
            }

            return smbRequest;
        }


        
        #endregion
    }
}