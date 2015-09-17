// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// to decode the CIFS packets.
    /// </summary>
    public class CifsClientDecodePacket
    {
        #region fields

        /// <summary>
        /// the context of smb client.
        /// </summary>
        protected CifsClientContext clientContext;

        private CifsClientConfig clientConfig;
        private bool isContextUpdateEnabled;

        #endregion


        #region properties

        /// <summary>
        /// to update context by stack automatically or not.
        /// </summary>
        public bool IsContextUpdateEnabled
        {
            get
            {
                return this.isContextUpdateEnabled;
            }
            set
            {
                this.isContextUpdateEnabled = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the context must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">the config must not be null.</exception>
        public CifsClientDecodePacket(CifsClientContext context, CifsClientConfig config)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            this.clientContext = context;
            this.clientConfig = config;
            this.isContextUpdateEnabled = true;
        }

        #endregion


        #region decode

        /// <summary>
        /// to decode the CIFS packets.
        /// </summary>
        /// <param name="endPoint">the identification of the connection.</param>
        /// <param name="messageBytes">the message to be decoded.</param>
        /// <param name="consumedLength">the bytes length which are consumed when decode.</param>
        /// <param name="expectedLength">the expected length of data to be received.</param>
        /// <returns>the packets decoded from the input buffer.</returns>
        public StackPacket[] DecodePacket(
            object endPoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            StackPacket[] packets = new StackPacket[0];

            StackPacket response = this.DecodeSmbResponseFromBytes((int)endPoint, messageBytes, out consumedLength);
            // Use the decoded packet to UpdateRoleContext if it is not null and ContextUpdate is enabled:
            if (response != null)
            {
                if (this.isContextUpdateEnabled)
                {
                    this.clientContext.UpdateRoleContext((int)endPoint, response);
                }
                packets = new StackPacket[] { response };
            }

            expectedLength = 0;
            return packets;
        }


        /// <summary>
        /// decode packet from bytes
        /// </summary>
        /// <param name="connectId">the connection identity.</param>
        /// <param name="messageBytes">bytes contains packet</param>
        /// <param name="consumedLength">the bytes length which are consumed when decode.</param>
        /// <returns>the decoded packet from the bytes array. if failed, return null.</returns>
        protected SmbPacket DecodeSmbResponseFromBytes(
            int connectId,
            byte[] messageBytes,
            out int consumedLength)
        {
            consumedLength = 0;
            SmbPacket smbRequest = null;
            SmbPacket smbResponse = null;
            using (MemoryStream memoryStream = new MemoryStream(messageBytes, true))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    // read raw response:
                    Collection<SmbPacket> outstandingRequests = this.clientContext.GetOutstandingRequests(connectId);
                    if (outstandingRequests != null && outstandingRequests.Count > 0)
                    {
                        SmbReadRawRequestPacket readRawRequest = outstandingRequests[0] as SmbReadRawRequestPacket;
                        if (readRawRequest != null)
                        {
                            SmbReadRawResponsePacket readRawResponse = this.CreateSmbResponsePacket(
                                readRawRequest, readRawRequest.SmbHeader, channel) as SmbReadRawResponsePacket;
                            if (readRawResponse != null)
                            {
                                byte[] rawData = new byte[messageBytes.Length];
                                Array.Copy(messageBytes, rawData, rawData.Length);
                                readRawResponse.RawData = rawData;
                                consumedLength = rawData.Length;
                                return readRawResponse;
                            }
                            else
                            {
                                // discard the none-parsable data silently:
                                consumedLength = messageBytes.Length;
                                return null;
                            }
                        }
                        else
                        {
                            // No SmbReadRawResponsePacket sent, so the response should not be SmbReadRawResponsePacket.
                            // and do nothing here.
                        }
                    }

                    // read smb header and new SmbPacket: 
                    if (channel.Stream.Position < channel.Stream.Length
                        && messageBytes.Length >= CifsMessageUtils.GetSize<SmbHeader>(new SmbHeader()))
                    {
                        SmbHeader smbHeader = channel.Read<SmbHeader>();
                        smbRequest = this.clientContext.GetOutstandingRequest(connectId, smbHeader.Mid);
                        smbResponse = this.CreateSmbResponsePacket(smbRequest, smbHeader, channel);
                        consumedLength += smbResponse.HeaderSize;
                    }
                    else
                    {
                        // The data in the channel is less than the size of SmbHeader. consume nothing and return null:
                        consumedLength = 0;
                        return null;
                    }

                    // read SmbParameters:
                    consumedLength += smbResponse.ReadParametersFromChannel(channel);

                    // read SmbData:
                    consumedLength += smbResponse.ReadDataFromChannel(channel);

                    // read andx:
                    SmbBatchedResponsePacket smbBatchedResponse = smbResponse as SmbBatchedResponsePacket;
                    if (smbRequest != null && smbBatchedResponse != null)
                    {
                        consumedLength += DecodeBatchedRequest(channel, smbRequest, smbBatchedResponse);
                    }

                    // handle the difference of protocol implementation:
                    SmbWriteAndCloseResponsePacket writeAndCloseResponse = smbResponse as SmbWriteAndCloseResponsePacket;
                    if (writeAndCloseResponse != null)
                    {
                        if (this.clientConfig.IsWriteAndCloseResponseExtraPadding)
                        {
                            // Windows NT Server appends three NULL bytes to this message, following the ByteCount field.
                            // These three bytes are not message data and can safely be discarded.
                            const int PaddingLength = 3;
                            writeAndCloseResponse.PaddingBytes = channel.ReadBytes(PaddingLength);
                            consumedLength += PaddingLength;
                        }
                    }
                }
            }
            return smbResponse;
        }


        /// <summary>
        /// decode the batched request packet
        /// </summary>
        /// <param name="channel">the channel of bytes to read</param>
        /// <param name="request">the request of the response.</param>
        /// <param name="smbBatchedResponse">the batched response</param>
        /// <returns>the consumed length of batched response packet</returns>
        protected virtual int DecodeBatchedRequest(
            Channel channel,
            SmbPacket request, SmbBatchedResponsePacket smbBatchedResponse)
        {
            int batchedConsumedLength = 0;

            batchedConsumedLength += smbBatchedResponse.ReadAndxFromChannel(request, channel);

            return batchedConsumedLength;
        }


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
        protected virtual SmbPacket CreateSmbResponsePacket(
            SmbPacket request,
            SmbHeader smbHeader,
            Channel channel)
        {
            return CifsMessageUtils.CreateSmbResponsePacket(request, smbHeader, channel);
        }


        #endregion
    }
}