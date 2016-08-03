// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    public class SmbdClient
    {
        #region Constant variable is the parameter for RDMA which is not related to SMBD test cases
        /// <summary>
        /// The maximum number of scatter/gather entries supported for Receive requests.
        /// </summary>
        private const uint INBOUND_SEGMENTS = 1;
        /// <summary>
        /// The maximum number of scatter/gather entries supported for Send, Read, and Write requests.
        /// </summary>
        private const uint OUTBOUND_SEGMENTS = 1;
        /// <summary>
        /// The maximum outbound read limit for the local Network Direct adapter. 
        /// Set to zero because SMBD client do not issue Read requests to the peer.
        /// </summary>
        private const uint OUTBOUND_READLIMIT = 0;
        #endregion

        private SmbdLogEvent logEvent;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logSmbdEvent">Delegate to log SMBD event</param>
        public SmbdClient(SmbdLogEvent logSmbdEvent = null)
        {
            this.logEvent = logSmbdEvent;
        }

        #region Properties
        
        public SmbdConnection Connection { get; protected set; }
        public int ReceiveEntryInQueue { get { return Connection.Endpoint.ReceiveRequestsCount; } }
        public int ReceivePostedCount { get { return Connection.Endpoint.ReceivePostedCount; } }
        #endregion

        /// <summary>
        /// Connection to server over RDMA, use the default parameters
        /// </summary>
        /// <param name="clientIp">Clinet IP address</param>
        /// <param name="serverIp">Server IP address</param>
        /// <param name="port">Port</param>
        /// <param name="ipFamily">IP Family</param>
        /// <param name="nInboundEntries">RDMA inbound entries</param>
        /// <param name="nOutboundEntries">RDMA outbound entries</param>
        /// <param name="inboundReadLimit">RDMA inbound read size</param>
        /// <param name="inboundDataSize">Max Size of RDMA inbound data</param>
        /// <returns>Status of RMDA connection</returns>
        public NtStatus ConnectToServerOverRdma(
            string clientIp,
            string serverIp,
            int port,
            AddressFamily ipFamily,
            uint nInboundEntries,
            uint nOutboundEntries,
            uint inboundReadLimit,
            uint inboundDataSize
            )
        {
            Connection = new SmbdConnection();

            // initialize parameters
            Connection.Role = SmbdRole.ACTIVE;

            // reset value
            Connection.Protocol = SmbdVersion.NONE;
            Connection.SendCredits = 0;
            Connection.ReceiveCredits = 0;
            Connection.FragmentReassemblyRemaining = 0;

            Connection.SendQueue = new Queue<SmbdDataTransferMessage>();
            Connection.FragmentReassemblyBuffer = null;

            // do connection
            Connection.Endpoint = new SmbdConnectionEndpoint(
                nInboundEntries, 
                nOutboundEntries, 
                INBOUND_SEGMENTS, 
                OUTBOUND_SEGMENTS, 
                inboundReadLimit,
                OUTBOUND_READLIMIT,
                inboundDataSize,
                logEvent
                );
            NtStatus ret = (NtStatus)Connection.Endpoint.ConnectToServerOverRdma(
                clientIp, 
                serverIp, 
                (ushort)port,
                ipFamily
                );
            return ret;
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {
            if (Connection != null && Connection.Endpoint != null)
            {
                Connection.Endpoint.Disconnect();
                Connection.Endpoint = null;
            }
            this.logEvent = null;
        }

        /// <summary>
        /// SMBDirect Negotiate
        /// </summary>
        /// <param name="minVersion">The minimum SMBDirect Protocol version supported by the sender</param>
        /// <param name="maxVersion">The maximum SMBDirect Protocol version supported by the sender</param>
        /// <param name="creditsRequested">The number of Send Credits requested of the receiver</param>
        /// <param name="receiveCreditMax">Maximum of receive credits</param>
        /// <param name="preferredSendSize">The maximum number of bytes that the sender requests to transmit in a single message</param>
        /// <param name="maxReceiveSize">The maximum number of bytes that the sender can receive in a single message</param>
        /// <param name="maxFragmentedSize">The maximum number of upper-layer bytes that the sender can receive as the result of a sequence of fragmented Send operations</param>
        /// <param name="smbdNegotiateResponse">SMBDirect Negotiate response</param>
        /// <param name="reserved">The sender SHOULD set this field to 0 and the receiver MUST ignore it on receipt</param>
        /// <returns></returns>
        public NtStatus Negotiate(
            SmbdVersion minVersion,
            SmbdVersion maxVersion,
            ushort creditsRequested,
            ushort receiveCreditMax,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentedSize,
            out SmbdNegotiateResponse smbdNegotiateResponse,
            ushort reserved = 0
            )
        {
            #region set SMBD connection

            Connection.ReceiveCreditMax = receiveCreditMax;
            Connection.SendCreditTarget = creditsRequested;
            Connection.MaxSendSize = preferredSendSize;
            Connection.MaxFragmentedSize = maxFragmentedSize;
            Connection.MaxReceiveSize = maxReceiveSize;
            #endregion

            smbdNegotiateResponse = new SmbdNegotiateResponse();

            // send negotiate message
            SmbdNegotiateRequest smbdRequest = new SmbdNegotiateRequest();
            smbdRequest.MinVersion = minVersion;
            smbdRequest.MaxVersion = maxVersion;
            smbdRequest.Reserved = reserved;
            smbdRequest.CreditsRequested = creditsRequested;
            smbdRequest.PreferredSendSize = preferredSendSize;
            smbdRequest.MaxReceiveSize = maxReceiveSize;
            smbdRequest.MaxFragmentedSize = maxFragmentedSize;
            

            byte[] requestBytes = TypeMarshal.ToBytes<SmbdNegotiateRequest>(smbdRequest);

            // post receive
            NtStatus ret = Connection.Endpoint.PostReceive((uint)SmbdNegotiateResponse.SIZE);
            if (ret != NtStatus.STATUS_SUCCESS)
            {
                LogEvent(string.Format("Connection.Endpoint.PostReceive with error {0}", ret));
                return ret;
            }

            // send message
            ret = (NtStatus)Connection.Endpoint.SendData(requestBytes);
            if (ret != NtStatus.STATUS_SUCCESS)
            {
                LogEvent(string.Format("Connection.Endpoint.SendData with error {0}", ret));
                return ret;
            }

            byte[] responseBytes;
            try
            {
                ret = Connection.Endpoint.ReceiveData(
                    TimeSpan.FromSeconds(SmbdConnection.ACTIVE_NEGOTIATION_TIMEOUT),
                    out responseBytes);
                if (ret != NtStatus.STATUS_SUCCESS)
                {
                    LogEvent(string.Format("Connection.Endpoint.ReceiveData with error {0}", ret));
                    return ret;
                }
            }
            catch (TimeoutException e)
            {
                LogEvent(string.Format("Do not get the SMBD negotiate response within {0} seconds", SmbdConnection.ACTIVE_NEGOTIATION_TIMEOUT));
                throw new TimeoutException(e.Message);
            }

            smbdNegotiateResponse = TypeMarshal.ToStruct<SmbdNegotiateResponse>(responseBytes);
            if ((NtStatus)smbdNegotiateResponse.Status != NtStatus.STATUS_SUCCESS)
            {
                LogEvent(string.Format("SMBDirect Negotiate response with status {0}", (NtStatus)smbdNegotiateResponse.Status));
                return (NtStatus)smbdNegotiateResponse.Status;
            }

            #region set connection parameters
            Connection.Protocol = SmbdVersion.V1;
            Connection.ReceiveCreditTarget = smbdNegotiateResponse.CreditsRequested;
            Connection.MaxReceiveSize = Smaller(Connection.MaxReceiveSize, smbdNegotiateResponse.PreferredSendSize);
            Connection.MaxSendSize = Smaller(Connection.MaxSendSize, smbdNegotiateResponse.MaxReceiveSize);
            Connection.MaxReadWriteSize = Smaller(smbdNegotiateResponse.MaxReadWriteSize, SmbdConnection.FLOOR_MAX_READ_WRITE_SIZE);
            Connection.SendCredits = smbdNegotiateResponse.CreditsGranted;
            #endregion

            Connection.Role = SmbdRole.ESTABLISHED;
            try
            {
                Connection.FragmentReassemblyBuffer = new byte[Connection.MaxFragmentedSize];
            }
            catch (OverflowException)
            {
                Connection.MaxFragmentedSize = smbdNegotiateResponse.MaxFragmentedSize;
                Connection.FragmentReassemblyBuffer = new byte[Connection.MaxFragmentedSize];
            }
            return (NtStatus)smbdNegotiateResponse.Status;
        }


        /// <summary>
        /// send SMBD data transfer message, for test case to control the transfer
        /// mannually
        /// </summary>
        /// <param name="creditsRequested">The total number of Send Credits requested 
        /// of the receiver, including any Send Credits already granted.
        /// </param>
        /// <param name="creditsGranted">The incremental number of Send Credits 
        /// granted by the sender.
        /// </param>
        /// <param name="flags">The flags indicating how the operation is to be processed.
        /// This field MUST be constructed by using any or none of the following values: 
        /// SMB_DIRECT_RESPONSE_REQUESTED. The Flags field MUST be set to zero if no flag 
        /// values are specified. 
        /// </param>
        /// <param name="reserved">The sender SHOULD set this field to 0 and the receiver 
        /// MUST ignore it on receipt.
        /// </param>
        /// <param name="remainingDataLength">The amount of data, in bytes, remaining in a 
        /// sequence of fragmented messages. If this value is 0x00000000, this message is 
        /// the final message in the sequence.
        /// </param>
        /// <param name="dataOffset">The offset, in bytes, from the beginning of the SMBDirect 
        /// header to the first byte of the message’s data payload. If no data payload 
        /// is associated with this message, this value MUST be 0. This offset MUST 
        /// be 8-byte aligned from the beginning of the message.
        /// </param>
        /// <param name="dataLength">The length, in bytes, of the message’s data payload. 
        /// If no data payload is associated with this message, this value MUST be 0.
        /// </param>
        /// <param name="padding">Additional bytes for alignment</param>
        /// <param name="buffer"></param>
        /// <returns>A buffer that contains the data payload as defined by 
        /// the DataOffset and DataLength fields.</returns>
        public NtStatus SendDataTransferMessage(
            ushort creditsRequested, 
            ushort creditsGranted,
            SmbdDataTransfer_Flags flags, 
            ushort reserved, 
            uint remainingDataLength, 
            uint dataOffset,
            uint dataLength, 
            byte[] padding, 
            byte[] buffer)
        {
            SmbdDataTransferMessage smbdDataTransfer = new SmbdDataTransferMessage();
            smbdDataTransfer.CreditsRequested = creditsRequested;
            smbdDataTransfer.CreditsGranted = creditsGranted;
            smbdDataTransfer.Flags = flags;
            smbdDataTransfer.Reserved = reserved;
            smbdDataTransfer.RemainingDataLength = remainingDataLength;
            smbdDataTransfer.DataOffset = dataOffset;
            smbdDataTransfer.DataLength = dataLength;
            smbdDataTransfer.Padding = padding;
            smbdDataTransfer.Buffer = buffer;

            return SendDataTransferMessage(smbdDataTransfer);
        }

        /// <summary>
        /// send SMBDirect data transfer message
        /// </summary>
        /// <param name="dataTransferMessage">SMBDirect data transfer message</param>
        /// <returns></returns>
        public NtStatus SendDataTransferMessage(SmbdDataTransferMessage dataTransferMessage)
        {
            byte[] data = TypeMarshal.ToBytes<SmbdDataTransferMessage>(dataTransferMessage);
            
            // register a buffer
            NtStatus status = Connection.Endpoint.SendData(data);

            return status;
        }


        /// <summary>
        /// receive SMBDirect data transfer message
        /// </summary>
        /// <param name="dataTransferMessage">Received SMBDirect data transfer message</param>
        /// <returns></returns>
        public NtStatus ReceiveDataTransferMessage(TimeSpan timeout, out SmbdDataTransferMessage dataTransferMessage)
        {
            dataTransferMessage = new SmbdDataTransferMessage();

            // get the memory
            byte[] data;
            NtStatus status = Connection.Endpoint.ReceiveData(timeout, out data);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }
            
            // decode
            dataTransferMessage = TypeMarshal.ToStruct<SmbdDataTransferMessage>(data);
            // log transfer package
            LogEvent(string.Format(
                    @"Receive SMBD Data Transfer
                    Credit Request: {0}
                    Credit Granted: {1}
                    Flags: {2}
                    Reserved: {3}
                    RemainingDataLength: {4}
                    DataOffset: {5}
                    Buffer Length: {6}",
                    dataTransferMessage.CreditsRequested,
                    dataTransferMessage.CreditsGranted,
                    dataTransferMessage.Flags,
                    dataTransferMessage.Reserved,
                    dataTransferMessage.RemainingDataLength,
                    dataTransferMessage.DataOffset,
                    dataTransferMessage.Buffer.Length));

            // The value of Connection.ReceiveCredits MUST be decremented by one.
            Connection.ReceiveCredits--;
            Connection.ReceiveCreditTarget = dataTransferMessage.CreditsRequested;

            if (dataTransferMessage.CreditsGranted > 0)
            {
                Connection.SendCredits += dataTransferMessage.CreditsGranted;
            }

            return NtStatus.STATUS_SUCCESS;
        }

        /// <summary>
        /// Upper layer requests that the SMBDirect Protocol sends a message.
        /// 
        /// send SMBDirect message. In this function, the data will disassemble to multiple segments
        /// if the data size is greater than Connection.MaxSendSize.
        /// </summary>
        /// <param name="message">data to send</param>
        public NtStatus SendMessage(byte[] message)
        {
            uint offset = 0; // current block offset
            // calculate the size of data each segment can send
            uint bufferSize = Connection.MaxSendSize - (uint)SmbdDataTransferMessage.DEFAULT_DATA_OFFSET;

            while (offset < message.Length)
            {
                uint blockLength;
                if (message.Length - offset > bufferSize)
                {
                    blockLength = bufferSize;
                }
                else
                {
                    blockLength = (uint)message.Length - offset;
                }
                byte[] tmp = new byte[blockLength];
                Array.Copy(message, offset, tmp, 0, blockLength);
                offset += blockLength;

                // enqueue
                SmbdDataTransferMessage transferMsg = new SmbdDataTransferMessage();
                transferMsg.CreditsGranted = 0;
                transferMsg.CreditsRequested = 0;
                transferMsg.Flags = 0;
                transferMsg.Reserved = 0;
                transferMsg.RemainingDataLength = (uint)message.Length - offset;
                transferMsg.DataOffset = (uint)SmbdDataTransferMessage.DEFAULT_DATA_OFFSET;
                transferMsg.DataLength = blockLength;
                transferMsg.Buffer = tmp;
                transferMsg.Padding = new byte[SmbdDataTransferMessage.DEFAULT_DATA_OFFSET - SmbdDataTransferMessage.MINIMUM_SIZE];
                Connection.SendQueue.Enqueue(transferMsg);
            }

            #region dequeue
            // grant the first message if exist

            bool isFirst = true;
            while (Connection.SendQueue.Count > 0)
            {
                isFirst = true;
                while (Connection.SendQueue.Count > 0)
                {
                    // If Connection.SendCredits is 0, stop processing messages, and break the loop.
                    if (Connection.SendCredits == 0)
                    {
                        break;
                    }
                    SmbdDataTransferMessage transferMsg = Connection.SendQueue.Peek();
                    if (isFirst)
                    {
                        isFirst = false;

                        int creditIncrement = ManageCredits(false);
                        if (Connection.SendQueue.Count != 0)
                        {
                            transferMsg.CreditsGranted = (ushort)creditIncrement;
                        }
                    }
                    /// If Connection.SendCredits is 1 and the CreditsGranted field of the message is 0, 
                    /// then at least one credit MUST be granted to the peer to prevent deadlock. 
                    if (Connection.SendCredits == 1 && transferMsg.CreditsGranted == 0)
                    {
                        int creditIncrement = ManageCredits(false);
                        if (creditIncrement == 0)
                        {// If the processing specified in section 3.1.5.9 returns zero, stop processing Sends
                            break;
                        }
                        // increment the CreditsGranted
                        transferMsg.CreditsGranted += (ushort)creditIncrement;
                    }

                    Connection.SendQueue.Dequeue();
                    Connection.SendCredits--;
                    transferMsg.CreditsRequested = Connection.SendCreditTarget;

                    
                    /// If Connection.KeepaliveRequested is "PENDING", the Flags field of the message MUST be 
                    /// set to SMB_DIRECT_RESPONSE_REQUESTED, Connection.KeepaliveRequested MUST be set to "SENT", 
                    /// and the Idle Connection Timer MUST be reset to 5 seconds. Otherwise, the Flags field of the message MUST be set to 0x0000.
                    /// 
                    /// No need follow the description in SDK. Test case checks the statement.
                    

                    // send message
                    NtStatus status = SendDataTransferMessage(transferMsg);
                    if (status != NtStatus.STATUS_SUCCESS)
                    {
                        return status;
                    }
                }

                // receive the credit granted message, when all send credits has been comsumed
                if (Connection.SendQueue.Count > 0 && Connection.SendCredits == 0)
                {
                    // receive the empty message for granting send credits
                    SmbdDataTransferMessage transferPackage;
                    this.ReceiveDataTransferMessage(
                        TimeSpan.FromSeconds(SmbdConnection.KEEP_ALIVE_INTERVAL),
                        out transferPackage
                        );
                }
                else
                {
                    break;
                }
            }

            #endregion

            return NtStatus.STATUS_SUCCESS;
        }

        /// <summary>
        /// receive SMBDirect message. If the fragmented message is sent as multiple segments from server,
        /// the method will reassemble the data before return.
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public NtStatus ReceiveMessage(TimeSpan timeout, out byte[] message)
        {
            uint offset = 0;
            bool isFirst = true;
            while (true)
            {
                SmbdDataTransferMessage transferPackage;
                NtStatus ret = ReceiveDataTransferMessage(timeout, out transferPackage);
                if (ret != NtStatus.STATUS_SUCCESS)
                {
                    message = null;
                    return ret;
                }

                // send message waiting in the QUEUE
                if (transferPackage.CreditsGranted > 0)
                {
                    this.SendMessage(new byte[0]);
                }
                #region Send Credit Grant message if Connection.ReceiveCredits == 0
                if (Connection.ReceiveCredits == 0)
                {
                    ManageCredits(false);
                    SendDataTransferMessage(
                        Connection.SendCreditTarget,
                        (ushort)Connection.ReceiveCredits,
                        SmbdDataTransfer_Flags.NONE,
                        0,
                        0,
                        0,
                        0,
                        new byte[0],
                        new byte[0]);
                    Connection.SendCredits--;
                }
                #endregion


                if (isFirst)
                {
                    Connection.FragmentReassemblyRemaining 
                        = transferPackage.DataLength + transferPackage.RemainingDataLength;
                }

                // append
                Array.Copy(transferPackage.Buffer, 0, Connection.FragmentReassemblyBuffer
                    , offset, transferPackage.DataLength);
                offset += transferPackage.DataLength;
                Connection.FragmentReassemblyRemaining -= transferPackage.DataLength;

                if (transferPackage.RemainingDataLength == 0)
                {
                    if(offset != 0)
                    {
                        message = new byte[offset];
                        Array.Copy(Connection.FragmentReassemblyBuffer, message, offset);
                        return NtStatus.STATUS_SUCCESS;
                    }

                    message = new byte[0]; // no data
                    return NtStatus.STATUS_SUCCESS;
                }
            }
        }

        /// <summary>
        /// register the memory locations indicated by the buffer with the underlying RDMA provider
        /// </summary>
        /// <param name="length"></param>
        /// <param name="flag">Read or Write flag</param>
        /// <param name="descriptor">Buffer Descriptor point to registered buffer</param>
        /// <param name="reversed">if it is true, little-endian and big-endian will be reversed in bufferDescriptor</param>
        /// <returns></returns>
        public NtStatus RegisterBuffer(
            uint length,
            SmbdBufferReadWrite flag, 
            bool reversed,
            out SmbdBufferDescriptorV1 descriptor
            )
        {
            RdmaBufferDescriptorV1 rdmaDescriptor;
            RdmaOperationReadWriteFlag readWriteFlag;
            if (flag == SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE)
            {
                readWriteFlag = RdmaOperationReadWriteFlag.Read;
            }
            else if (flag == SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE)
            {
                readWriteFlag = RdmaOperationReadWriteFlag.Write;
            }
            else
            {
                readWriteFlag = RdmaOperationReadWriteFlag.ReadAndWrite;
            }
            NtStatus status = (NtStatus)Connection.Endpoint.RegisterMemoryWindow(
                length, 
                readWriteFlag,
                reversed,
                out rdmaDescriptor
                );
            descriptor = new SmbdBufferDescriptorV1();
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            descriptor.Length = rdmaDescriptor.Length;
            descriptor.Offset = rdmaDescriptor.Offset;
            descriptor.Token = rdmaDescriptor.Token;

            return NtStatus.STATUS_SUCCESS;
        }

        /// <summary>
        /// deregister each memory region indicated by each SMBDirect Buffer Descriptor V1 
        /// structure with the underlying RDMA provider
        /// </summary>
        /// <param name="bufferDescriptor">Buffer Descriptor point to registered buffer</param>
        public void DeregisterBuffer(SmbdBufferDescriptorV1 bufferDescriptor)
        {
            RdmaBufferDescriptorV1 rdmaBufferDescriptor = new RdmaBufferDescriptorV1();
            rdmaBufferDescriptor.Length = bufferDescriptor.Length;
            rdmaBufferDescriptor.Token = bufferDescriptor.Token;
            rdmaBufferDescriptor.Offset = bufferDescriptor.Offset;
            Connection.Endpoint.DeregisterMemoryWindow(rdmaBufferDescriptor);
        }

        /// <summary>
        /// Managing Credits Prior to Sending
        /// </summary>
        /// <param name="onlyCheck">Only check the credit but not modify the parameters</param>
        /// <returns>the number of receive credits successfully added to the connection.</returns>
        public int ManageCredits(bool onlyCheck)
        {
            LogEvent(string.Format(
                    @"Manage Credits:
                    SendCredits: {0}
                    SendCreditsTarget: {1}
                    ReceiveCredits: {2}
                    ReceiveCreditsMax: {3}
                    ReceiveCreditsTarget: {4}",
                    Connection.SendCredits,
                    Connection.SendCreditTarget,
                    Connection.ReceiveCredits,
                    Connection.ReceiveCreditMax,
                    Connection.ReceiveCreditTarget));

            /// If Connection.ReceiveCredits is nonzero and greater than or equal to Connection.ReceiveCreditTarget
            /// , then sufficient credits are already present and a value of zero SHOULD be returned.
            if (Connection.ReceiveCredits != 0 
                && Connection.ReceiveCredits >= Connection.ReceiveCreditTarget)
            {
                return 0;
            }

            /// If Connection.ReceiveCreditTarget is greater than Connection.ReceiveCredits 
            /// and Connection.ReceiveCredits is less than Connection.ReceiveCreditMax
            if (Connection.ReceiveCreditTarget > Connection.ReceiveCredits
                && Connection.ReceiveCredits < Connection.ReceiveCreditMax)
            {
                int receiveCreditIncrement = (int)(Connection.ReceiveCreditMax - Connection.ReceiveCredits);
                if (onlyCheck)
                {
                    return receiveCreditIncrement;
                }
                /// the sender SHOULD attempt to increase the credits available to the peer on the connection. 
                /// 
                /// post a number of receive operations to the Connection with buffer of size MaxReceiveSize
                /// count ReceiveCreditTarget or ReceiveCreditMax

                uint orgReceiveCredits = Connection.ReceiveCredits;
                for (int i = 0; i < receiveCreditIncrement; ++i)
                {
                    if (PostReceive() != NtStatus.STATUS_SUCCESS)
                    {
                        break;
                    }
                }
                return (int)(Connection.ReceiveCredits - orgReceiveCredits);
            }

            return 0;
        }

        /// <summary>
        /// Post receive request
        /// </summary>
        /// <returns></returns>
        public NtStatus PostReceive()
        {
            return PostReceive(Connection.MaxReceiveSize);
        }

        /// <summary>
        /// Post receive request
        /// </summary>
        /// <param name="size">Size</param>
        /// <returns></returns>
        public NtStatus PostReceive(uint size)
        {
            NtStatus status = Connection.Endpoint.PostReceive(size);
            if (status == NtStatus.STATUS_SUCCESS)
            {
                Connection.ReceiveCredits++;
            }
            return status;
        }

        /// <summary>
        /// Write data to registered buffer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bufferDescriptor">Buffer Descriptor point to registered buffer</param>
        public NtStatus WriteRegisteredBuffer(byte[] data, SmbdBufferDescriptorV1 bufferDescriptor)
        {
            RdmaBufferDescriptorV1 rdmaBufferDescriptor = new RdmaBufferDescriptorV1();
            rdmaBufferDescriptor.Length = bufferDescriptor.Length;
            rdmaBufferDescriptor.Token = bufferDescriptor.Token;
            rdmaBufferDescriptor.Offset = bufferDescriptor.Offset;
            return Connection.Endpoint.WriteMemoryWindow(data, rdmaBufferDescriptor);
        }

        /// <summary>
        /// Read data from registered buffer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bufferDescriptor">Buffer Descriptor point to registered buffer</param>
        public NtStatus ReadRegisteredBuffer(byte[] data, SmbdBufferDescriptorV1 bufferDescriptor)
        {
            RdmaBufferDescriptorV1 rdmaBufferDescriptor = new RdmaBufferDescriptorV1();
            rdmaBufferDescriptor.Length = bufferDescriptor.Length;
            rdmaBufferDescriptor.Token = bufferDescriptor.Token;
            rdmaBufferDescriptor.Offset = bufferDescriptor.Offset;
            return Connection.Endpoint.ReadMemoryWindow(data, rdmaBufferDescriptor);
        }

        /// <summary>
        /// split fragmented message to multiple SMBDirect segments
        /// </summary>
        /// <param name="message">Input data</param>
        /// <param name="segmentLength">length of each segment</param>
        /// <returns></returns>
        public static List<SmbdDataTransferMessage> SplitData2Segments(
            byte[] message,
            uint segmentLength)
        {
            uint offset = 0; // current block offset
            // calculate the size of data each segment can send
            List<SmbdDataTransferMessage> ret = new List<SmbdDataTransferMessage>();

            while (offset < message.Length)
            {
                uint blockLength;
                if (message.Length - offset > segmentLength)
                {
                    blockLength = segmentLength;
                }
                else
                {
                    blockLength = (uint)message.Length - offset;
                }
                byte[] tmp = new byte[blockLength];
                Array.Copy(message, offset, tmp, 0, blockLength);
                offset += blockLength;

                // enqueue
                SmbdDataTransferMessage transferMsg = new SmbdDataTransferMessage();
                transferMsg.CreditsGranted = 0;
                transferMsg.CreditsRequested = 0;
                transferMsg.Flags = 0;
                transferMsg.Reserved = 0;
                transferMsg.RemainingDataLength = (uint)message.Length - offset;
                transferMsg.DataOffset = (uint)SmbdDataTransferMessage.DEFAULT_DATA_OFFSET;
                transferMsg.DataLength = blockLength;
                transferMsg.Buffer = tmp;
                transferMsg.Padding = new byte[SmbdDataTransferMessage.DEFAULT_DATA_OFFSET - SmbdDataTransferMessage.MINIMUM_SIZE];
                ret.Add(transferMsg);
            }

            return ret;
        }

        /// <summary>
        /// Return smaller one of value1 and value2
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static uint Smaller(uint value1, uint value2)
        {
            if (value1 < value2)
            {
                return value1;
            }
            return value2;
        }

        private void LogEvent(string log)
        {
            if (this.logEvent != null)
            {
                logEvent(log);
            }
        }
    }
}