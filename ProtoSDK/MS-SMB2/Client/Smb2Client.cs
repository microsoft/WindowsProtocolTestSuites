// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Common;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class ReceivedPackets
    {
        private class WaitingPacket
        {
            /// <summary>
            /// The event to wait packet.
            /// </summary>
            public AutoResetEvent WaitEvent;

            /// <summary>
            /// Expected packet.
            /// </summary>
            public Smb2Packet Packet;

            public WaitingPacket()
            {
                WaitEvent = new AutoResetEvent(false);
                Packet = null;
            }
        }

        /// <summary>
        /// Key: Message Id
        /// Value: A queue which contains wait events for the same message id. It is used for negative test cases which send multiple messages with the same message id.
        /// </summary>
        private Dictionary<ulong, Queue<WaitingPacket>> packetReceived;

        /// <summary>
        /// The timeout to wait a packet.
        /// </summary>
        private TimeSpan timeout;

        public ReceivedPackets(TimeSpan timeout)
        {
            this.timeout = timeout;
            packetReceived = new Dictionary<ulong, Queue<WaitingPacket>>();
        }

        /// <summary>
        /// This method must be called before sending a request packet to the server.
        /// Prepare to wait response packet.
        /// </summary>
        /// <param name="packet">The packet which will be sent to the server.</param>
        public void PrepareWaitPacket(Smb2Packet packet)
        {
            ulong messageId = 0;
            if (packet is Smb2SinglePacket)
            {
                messageId = (packet as Smb2SinglePacket).Header.MessageId;
                EnqueueWaitPacket(messageId);
            }
            else if (packet is Smb2CompoundPacket)
            {
                // Add all MessageIds into the wait list, in case the coming responses are not in one Compound packet.
                List<Smb2SinglePacket> packets = (packet as Smb2CompoundPacket).Packets;
                foreach (var single in packets)
                {
                    EnqueueWaitPacket(single.Header.MessageId);
                }
            }
            else
            {
                // Message id of SmbNegotiateRequestPacket is 0.
                EnqueueWaitPacket(messageId);
            }
        }

        /// <summary>
        /// This method must be called when waiting a response packet from the server.
        /// </summary>
        /// <param name="messageId">The message id of the expected response packet.</param>
        /// <returns></returns>
        public Smb2Packet WaitPacket(ulong messageId)
        {
            bool ret = false;
            WaitingPacket waitingPacket;

            lock (packetReceived)
            {
                if (!packetReceived.ContainsKey(messageId))
                {
                    throw new ArgumentOutOfRangeException("Invalid message id " + messageId);
                }
            }

            ret = packetReceived[messageId].Peek().WaitEvent.WaitOne(timeout);
            lock (packetReceived)
            {
                waitingPacket = packetReceived[messageId].Dequeue();
                if (packetReceived[messageId].Count == 0)
                {
                    packetReceived.Remove(messageId);
                }
            }

            if (!ret)
            {
                throw new TimeoutException();
            }

            return waitingPacket.Packet;
        }

        /// <summary>
        /// This method must be called when an expected packet is received.
        /// </summary>
        /// <param name="packet"></param>
        public void SetReceivedPacket(Smb2Packet packet)
        {
            ulong messageId = 0;
            if (packet is Smb2SinglePacket)
            {
                messageId = (packet as Smb2SinglePacket).Header.MessageId;
            }
            else if (packet is Smb2CompoundPacket)
            {
                messageId = (packet as Smb2CompoundPacket).Packets[0].Header.MessageId;
            }
            else
            {
                // Message id of SmbNegotiateResponsePacket is 0.
            }

            lock (packetReceived)
            {
                if (!packetReceived.ContainsKey(messageId))
                {
                    throw new ArgumentOutOfRangeException("Invalid message id.");
                }
                packetReceived[messageId].ElementAt<WaitingPacket>(0).Packet = packet;
                packetReceived[messageId].ElementAt<WaitingPacket>(0).WaitEvent.Set();
            }
        }

        /// <summary>
        /// Signal all events to unblock the waiting.
        /// </summary>
        public void Release()
        {
            lock (packetReceived)
            {
                foreach (var waitingPacketQueue in packetReceived)
                {
                    for (int i = 0; i < waitingPacketQueue.Value.Count; i++)
                    {
                        waitingPacketQueue.Value.ElementAt(i).WaitEvent.Set();
                    }
                }
            }
        }

        private void EnqueueWaitPacket(ulong messageId)
        {
            lock (packetReceived)
            {
                if (!packetReceived.ContainsKey(messageId))
                {
                    packetReceived[messageId] = new Queue<WaitingPacket>();
                }
                packetReceived[messageId].Enqueue(new WaitingPacket());
            }
        }
    }

    /// <summary>
    /// Smb2Client mocks the client functionality of smb2, It is
    /// used to create all kinds of packet, send the packet to 
    /// server, and receive packets from server. 
    /// It establishes one connection and maintains info of multiple sessions
    /// within that connection and could hold one channel for each session in multichannel scenario
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Smb2Client : IDisposable
    {
        #region Fields

        private TimeSpan timeout;

        private TransportStack transport;
        private Smb2TransportType transportType;

        private PreauthIntegrityContext preauthContext;
        private EncryptionAlgorithm cipherId;
        private PreauthIntegrityHashID hashId;

        protected Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable = new Dictionary<ulong, Smb2CryptoInfo>();
        private Smb2CompressionInfo compressionInfo;
        private Smb2Decoder decoder;

        private bool disposed;
        private bool serverDisconnected;
        private bool connected;

        /// <summary>
        /// The exception happens when receiving the packet from server.
        /// </summary>
        private Exception exceptionWhenReceivingPacket;

        private Thread thread;
        private Thread notificationThread;

        private Queue<Smb2SinglePacket> receivedNotifications = new Queue<Smb2SinglePacket>();
        private AutoResetEvent notificationReceivedEvent = new AutoResetEvent(false);
        private ReceivedPackets receivedPackets;

        private DialectRevision dialect;

        // Disable signature verification by default.
        private bool disableVerifySignature = true;
        private Smb2SessionSetupResponsePacket sessionSetupResponse;

        private Smb2ErrorResponsePacket error;

        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether the client is connected to server.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return connected;
            }
        }

        /// <summary>
        /// Indicates whether the connection is terminated by server
        /// </summary>
        public bool IsServerDisconnected
        {
            get
            {
                return serverDisconnected;
            }
        }

        /// <summary>
        /// Is any data can be read
        /// </summary>
        public virtual bool IsDataAvailable
        {
            get
            {
                if (connected)
                {
                    return transport.IsDataAvailable;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The server selected cipher ID for encryption.
        /// </summary>
        public EncryptionAlgorithm SelectedCipherID
        {
            get
            {
                return cipherId;
            }
        }

        /// <summary>
        /// The server selected hash ID for Preauth Integrity.
        /// </summary>
        public PreauthIntegrityHashID SelectedPreauthIntegrityHashID
        {
            get
            {
                return hashId;
            }
        }

        /// <summary>
        /// Indicates if the signature verification of SMB2 responses from SUT will be disabled.
        /// </summary>
        public bool DisableVerifySignature
        {
            get
            {
                return disableVerifySignature;
            }
            set
            {
                disableVerifySignature = value;
                foreach (var cryptoInfo in cryptoInfoTable)
                {
                    cryptoInfo.Value.DisableVerifySignature = value;
                }
            }
        }

        /// <summary>
        /// Indicates whether to check the response from the server is actually encrypted.
        /// </summary>
        public bool CheckEncrypt
        {
            get
            {
                return decoder.CheckEncrypt;
            }
            set
            {
                decoder.CheckEncrypt = value;
            }
        }

        public Smb2ErrorResponsePacket Error
        {
            get
            {
                return error;
            }
        }

        public Smb2CompressionInfo CompressionInfo
        {
            get
            {
                return compressionInfo;
            }
        }
        #endregion

        #region Constructor

        public Smb2Client(TimeSpan timeout)
        {
            this.timeout = timeout;

            compressionInfo = new Smb2CompressionInfo();

            this.decoder = new Smb2Decoder(Smb2Role.Client, cryptoInfoTable, compressionInfo);

            receivedPackets = new ReceivedPackets(timeout);

            hashId = PreauthIntegrityHashID.HashAlgorithm_NONE;

            cipherId = EncryptionAlgorithm.ENCRYPTION_NONE;
        }

        #endregion

        #region Events

        public event Action<Smb2Packet> PacketSending;
        public event Action<Smb2Packet> ProcessedPacketModifier;
        public event Func<byte[], byte[]> OnWirePacketModifier;
        public event Action<Smb2Packet> PacketReceived;
        public event Action Disconnected;

        public event Action<Smb2SinglePacket> PendingResponseReceived;

        public event Action<FILE_NOTIFY_INFORMATION[], Packet_Header, CHANGE_NOTIFY_Response> ChangeNotifyResponseReceived;

        public event Action<Packet_Header, OPLOCK_BREAK_Notification_Packet> OplockBreakNotificationReceived;
        public event Action<Packet_Header, LEASE_BREAK_Notification_Packet> LeaseBreakNotificationReceived;

        public event Action<Exception> NotificationThreadExceptionHappened;

        #endregion

        #region Utility Functions

        /// <summary>
        /// Event loop to handle notifications.
        /// Notifications will be received in EventLoop but are handled in this method.
        /// NotificationEventLoop is called in a different thread from which EventLoop is called.
        /// </summary>
        private void NotificationEventLoop()
        {
            while (true)
            {
                try
                {
                    if (notificationReceivedEvent.WaitOne())
                    {
                        if (serverDisconnected)
                            break;

                        do
                        {
                            Smb2SinglePacket packet = null;

                            lock (receivedNotifications)
                            {
                                if (receivedNotifications.Count > 0)
                                {
                                    packet = receivedNotifications.Dequeue();
                                }
                            }

                            if (packet == null) break;

                            if (packet.Header.Status == Smb2Status.STATUS_PENDING)
                            {
                                if (PendingResponseReceived != null)
                                {
                                    PendingResponseReceived(packet);
                                }
                            }
                            else if (packet is Smb2ChangeNotifyResponsePacket)
                            {
                                var changeNotifyResponse = packet as Smb2ChangeNotifyResponsePacket;

                                if (ChangeNotifyResponseReceived != null)
                                {
                                    uint outputBufferLength = changeNotifyResponse.PayLoad.OutputBufferLength;
                                    byte[] outputBuffer = new byte[outputBufferLength];
                                    if (changeNotifyResponse.Error == null)
                                    {
                                        Array.Copy(changeNotifyResponse.Buffer, changeNotifyResponse.PayLoad.OutputBufferOffset - changeNotifyResponse.BufferOffset, outputBuffer, 0, outputBufferLength);
                                    }
                                    FILE_NOTIFY_INFORMATION[] arrFileNotifyInfo = Smb2Utility.UnmarshalFileNotifyInformation(outputBuffer);
                                    ChangeNotifyResponseReceived(
                                        arrFileNotifyInfo,
                                        changeNotifyResponse.Header,
                                        changeNotifyResponse.PayLoad);
                                }
                            }
                            else if (packet is Smb2OpLockBreakNotificationPacket)
                            {
                                var oplockBreakNotification = packet as Smb2OpLockBreakNotificationPacket;

                                if (OplockBreakNotificationReceived != null)
                                {
                                    OplockBreakNotificationReceived(
                                        oplockBreakNotification.Header,
                                        oplockBreakNotification.PayLoad);
                                }
                            }
                            else if (packet is Smb2LeaseBreakNotificationPacket)
                            {
                                var leaseBreakNotification = packet as Smb2LeaseBreakNotificationPacket;

                                if (LeaseBreakNotificationReceived != null)
                                {
                                    LeaseBreakNotificationReceived(
                                        leaseBreakNotification.Header,
                                        leaseBreakNotification.PayLoad);
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException("Unknown notification: " + packet);
                            }

                        } while (true);
                    }

                }
                catch (ThreadAbortException)
                {
                    // Notification thread is aborted.
                    // End the current thread.
                    return;
                }
                catch (Exception exception)
                {
                    // If throw the exception from this receive thread, QTAgent will crash.
                    // So pass the exception by NotificationThreadExceptionHappened event.
                    // End the current thread.
                    if (NotificationThreadExceptionHappened != null)
                    {
                        try
                        {
                            NotificationThreadExceptionHappened(exception);
                        }
                        catch
                        {
                            // Exception happens when handling event.
                            // Catch to avoid the exception crash QTAgent.
                            // End the current thread.
                            return;
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Event loop to handle all received packets.
        /// </summary>
        private void EventLoop()
        {
            while (true)
            {
                TransportEvent transEvent = null;
                try
                {
                    transEvent = transport.ExpectTransportEvent(timeout);

                    switch (transEvent.EventType)
                    {
                        case EventType.ReceivedPacket:
                            var packet = (Smb2Packet)transEvent.EventObject;

                            if (PacketReceived != null)
                                PacketReceived(packet);

                            if (packet is SmbNegotiateResponsePacket)
                            {
                                receivedPackets.SetReceivedPacket(packet);
                                break;
                            }

                            var single = packet as Smb2SinglePacket;
                            if (single != null)
                            {
                                if (single.Header.Status == Smb2Status.STATUS_PENDING
                                    || packet is Smb2ChangeNotifyResponsePacket
                                    || packet is Smb2OpLockBreakNotificationPacket
                                    || packet is Smb2LeaseBreakNotificationPacket)
                                {
                                    lock (receivedNotifications)
                                    {
                                        receivedNotifications.Enqueue(single);
                                    }
                                    notificationReceivedEvent.Set();
                                }
                                else
                                {
                                    receivedPackets.SetReceivedPacket(packet);
                                }
                            }
                            else // Compound
                            {
                                receivedPackets.SetReceivedPacket(packet);
                            }
                            break;
                        case EventType.Disconnected:
                            serverDisconnected = true;
                            if (Disconnected != null)
                                Disconnected();

                            receivedPackets.Release();

                            notificationReceivedEvent.Set();

                            return;
                    }
                }
                catch (Exception exception)
                {
                    // If throw the exception from this receive thread, QTAgent will crash.
                    // So save the exception to a member variable, and throw it when the case calls ExpectPacket.
                    // End the current thread.
                    exceptionWhenReceivingPacket = exception;
                    receivedPackets.Release();
                    notificationReceivedEvent.Set();
                    return;
                }
            }
        }

        private T SendPacketAndExpectResponse<T>(Smb2Packet packet) where T : Smb2Packet, new()
        {
            SendPacket(packet);

            ulong messageId = 0; // Message id of SmbNegotiateRequestPacket is 0.
            if (packet is Smb2SinglePacket)
            {
                messageId = (packet as Smb2SinglePacket).Header.MessageId;
            }
            return ExpectPacket<T>(messageId);
        }

        /// <summary>
        /// Send a packet to server
        /// </summary>
        /// <param name="packet">The packet</param>
        public void SendPacket(Smb2Packet packet)
        {
            receivedPackets.PrepareWaitPacket(packet);

            if (PacketSending != null)
                PacketSending(packet);

            if (packet is SmbNegotiateRequestPacket)
            {
                // Send SMB packet directly
                SendPacket(packet.ToBytes());
            }
            else
            {
                var processedPacket = Smb2Crypto.SignCompressAndEncrypt(packet, cryptoInfoTable, CompressionInfo, Smb2Role.Client);

                if (ProcessedPacketModifier != null)
                {
                    ProcessedPacketModifier(processedPacket);
                }

                SendPacket(processedPacket.ToBytes());
            }
        }

        public virtual void SendPacket(byte[] data)
        {
            if (OnWirePacketModifier != null)
            {
                data = OnWirePacketModifier(data);
            }

            if (transportType == Smb2TransportType.NetBios)
            {
                try
                {
                    transport.SendBytes(data);
                }
                catch (InvalidOperationException)
                {
                    // NetbiosTransport uses synchronized NCBSEND.
                    // So if connection is aborted when sending the packet, exception is thrown from NetbiosTransport.
                    transport.AddEvent(new TransportEvent(EventType.Disconnected, null, null));
                }
            }
            else
            {
                transport.SendBytes(Smb2Utility.GenerateTcpTransportPayLoad(data));
            }
        }

        public virtual T ExpectPacket<T>(ulong messageId) where T : Smb2Packet
        {
            Smb2Packet packet = receivedPackets.WaitPacket(messageId);

            // Sometimes the response packet is followed by the disconnect message from server.
            // "serverDisconnected" could be changed to true by the EventLoop() in another thread
            // if the disconnect message is very close to the response packet.
            // In this case, we should handle the response packet first. We should not throw the exception.
            // On the other hand, if the response is a disconnect message (which means we cannot get a packet from the receive list), 
            // then we should throw the exception.
            if (packet == null && serverDisconnected)
                throw new InvalidOperationException("Underlying connection has been closed.");

            if (exceptionWhenReceivingPacket != null)
                throw exceptionWhenReceivingPacket;

            if (packet is T)
            {
                return packet as T;
            }
            else if (packet is Smb2SinglePacket)
            {
                var singlePacket = packet as Smb2SinglePacket;
                // We will force the casting if it's an OPLOCK_BREAK error packet
                if (singlePacket.Error != null && singlePacket.Header.Command == Smb2Command.OPLOCK_BREAK)
                {
                    if (typeof(T) == typeof(Smb2LeaseBreakResponsePacket))
                    {
                        var ret = new Smb2LeaseBreakResponsePacket();
                        ret.Header = singlePacket.Header;
                        ret.Error = singlePacket.Error;

                        return ret as T;
                    }
                    else
                    {
                        var ret = new Smb2OpLockBreakResponsePacket();
                        ret.Header = singlePacket.Header;
                        ret.Error = singlePacket.Error;

                        return ret as T;
                    }
                }
            }
            throw new InvalidOperationException("Unexpected packet: " + packet.ToString());
        }

        /// <summary>
        /// Used when expecting the response to a compound request
        /// The response could be a compound message or could be separate messages.
        /// </summary>
        /// <param name="messageIdList">A list of the message id sent in the compound request</param>
        /// <returns>A list of response packets to a compound request</returns>
        public List<Smb2SinglePacket> ExpectPackets(List<ulong> messageIdList)
        {
            List<Smb2SinglePacket> packets = new List<Smb2SinglePacket>();
            for (int i = 0; i < messageIdList.Count;)
            {
                Smb2Packet packet = receivedPackets.WaitPacket(messageIdList[i]);

                // Sometimes the response packet is followed by the disconnect message from server.
                // "serverDisconnected" could be changed to true by the EventLoop() in another thread
                // if the disconnect message is very close to the response packet.
                // In this case, we should handle the response packet first. We should not throw the exception.
                // On the other hand, if the response is a disconnect message (which means we cannot get a packet from the receive list), 
                // then we should throw the exception.
                if (packet == null && serverDisconnected)
                    throw new InvalidOperationException("Underlying connection has been closed.");

                if (exceptionWhenReceivingPacket != null)
                    throw exceptionWhenReceivingPacket;

                if (packet is Smb2CompoundPacket)
                {
                    packets.AddRange((packet as Smb2CompoundPacket).Packets);
                    i += (packet as Smb2CompoundPacket).Packets.Count;
                }
                else if (packet is Smb2SinglePacket)
                {
                    packets.Add(packet as Smb2SinglePacket);
                    i++;
                }
                else
                {
                    throw new NotImplementedException("SmbNegotiateResponsePacket to compound request is not implemented yet");
                }
            }

            return packets;
        }

        #endregion

        #region Connecting and Disconnect

        public void ConnectOverNetbios(string serverName)
        {
            Connect(Smb2TransportType.NetBios, serverName, Environment.MachineName, null, null);
        }

        public void ConnectOverTCP(IPAddress serverIp)
        {
            Connect(Smb2TransportType.Tcp, null, null, serverIp, null);
        }

        public void ConnectOverTCP(IPAddress serverIp, IPAddress clientIp)
        {
            Connect(Smb2TransportType.Tcp, null, null, serverIp, clientIp);
        }

        private void Connect(Smb2TransportType transportType, string serverName, string clientName, IPAddress serverIp, IPAddress clientIp)
        {
            this.transportType = transportType;
            decoder.TransportType = transportType;

            TransportConfig transportConfig = null;

            switch (transportType)
            {
                case Smb2TransportType.Tcp:
                    transportConfig = new SocketTransportConfig
                    {
                        LocalIpAddress = clientIp,
                        RemoteIpAddress = serverIp,
                        RemoteIpPort = 445,
                        Type = StackTransportType.Tcp,
                        Timeout = this.timeout,
                        BufferSize = Smb2Consts.MaxTcpBufferSize,
                    };
                    break;
                case Smb2TransportType.NetBios:
                    transportConfig = new NetbiosTransportConfig
                    {
                        MaxNames = Smb2Consts.MaxNames,
                        MaxSessions = Smb2Consts.MaxSessions,
                        LocalNetbiosName = clientName + new Random().Next(),
                        RemoteNetbiosName = serverName,
                        Type = StackTransportType.Netbios,
                        BufferSize = Smb2Consts.MaxNetbiosBufferSize,
                    };
                    break;
            }

            transportConfig.Role = Role.Client;

            transport = new TransportStack(transportConfig, decoder.Smb2DecodePacketCallback);
            transport.Connect();

            thread = new Thread(new ThreadStart(EventLoop));
            thread.IsBackground = true;
            thread.Start();
            connected = true;

            notificationThread = new Thread(new ThreadStart(NotificationEventLoop));
            notificationThread.IsBackground = true;
            notificationThread.Start();
        }

        /// <summary>
        /// Disconnect with server
        /// </summary>
        public void Disconnect()
        {
            if (connected)
            {
                try
                {
                    // After that, Client will receive disconnect from server and Eventloop will finish, and thread will terminate itself.
                    transport.Disconnect();
                }
                catch
                {
                    // Sometimes transport will throw an exception.
                }
                if (!thread.Join(new TimeSpan(0, 0, 2)))
                {
                    thread.Abort();
                }
                thread = null;
                if (!notificationThread.Join(new TimeSpan(0, 0, 2)))
                {
                    notificationThread.Abort();
                }
                notificationThread = null;

                transport.Dispose();

                connected = false;
            }
        }

        #endregion

        #region Encryption and Signing Settings

        /// <summary>
        /// Generate Signing and encrypt keys, enable signing and encryption flags in session wide.
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <param name="cryptographicKey">Crypto key computed with SSPI.</param>
        /// <param name="enableSigning">True if signing is enabled, false otherwise. Only valid when encryption is disabled.</param>
        /// <param name="enableEncryption">True if encryption is enabled, false otherwise.</param>
        /// <param name="mainChannelClient">
        /// mainChannelClient MUST be set when alternative channel with encryption
        /// When Encryption, alternative channel and main channel share the same Session.EncryptionKey and Session.DecryptionKey.
        /// So cryptInfo in main channel MUST be assigned to alternative channel.
        /// </param>
        /// <param name="isBinding">SMB2_SESSION_FLAG_BINDING is set. </param>
        public void GenerateCryptoKeys(
            ulong sessionId,
            byte[] cryptographicKey,
            bool enableSigning,
            bool enableEncryption,
            Smb2Client mainChannelClient = null,
            bool isBinding = false)
        {
            Smb2CryptoInfo cryptoInfo = null;

            if (cryptoInfoTable.ContainsKey(sessionId))
            {
                if (!isBinding)
                {
                    // re-authentication will not use new crypto key
                    cryptographicKey = cryptoInfoTable[sessionId].SessionKey;
                }

                //reserve Session.Encryption/Decryption from main channel
                cryptoInfo = cryptoInfoTable[sessionId];

                //remove the existing cryptoInfo, will re-generate a new cryptoInfo later
                cryptoInfoTable.Remove(sessionId);
            }

            // Get the preauthenticationHashValue for SMB 311
            byte[] preauthHashValue = null;
            if (dialect >= DialectRevision.Smb311 && dialect != DialectRevision.Smb2Unknown)
            {
                if (preauthContext != null)
                {
                    preauthHashValue = preauthContext.GetSessionPreauthIntegrityHashValue(sessionId);
                }
            }

            if (mainChannelClient == null)
            {
                if (cryptoInfo == null)
                {
                    // For a new session
                    // Generate new Session.SigningKey, Session.Encryption/Decryption with current session's sessionkey
                    // and Session.PreauthIntegrityHashValue (for dialect 3.11) as crypto key
                    cryptoInfoTable.Add(
                        sessionId,
                        new Smb2CryptoInfo(
                            dialect,
                            cryptographicKey,
                            enableSigning,
                            enableEncryption,
                            DisableVerifySignature,
                            null,
                            preauthHashValue,
                            cipherId));
                }
                else
                {
                    // For new-authentication, re-authentication
                    // For multiple channel alternative channel after session binding success
                    // - generate Channel.SigningKey
                    // - copy Session.Encryption/Decryption from main channel
                    cryptoInfoTable.Add(
                        sessionId,
                        new Smb2CryptoInfo(
                            dialect,
                            cryptographicKey,
                            enableSigning,
                            enableEncryption,
                            DisableVerifySignature,
                            cryptoInfo,
                            preauthHashValue));
                }
            }
            else
            {
                // For alternative channel, before bind session to main channel
                // Copy Session.SigningKey, Session.Encryption/Decryption from main channel                
                cryptoInfoTable.Add(sessionId, mainChannelClient.cryptoInfoTable[sessionId]);
            }
        }

        /// <summary>
        /// Enable, disable session wide signing and encryption
        /// </summary>
        /// <param name="sessionId">the session Id</param>
        /// <param name="enableSigning">true if need sign, otherwise false</param>
        /// <param name="enableEncryption">true if need encryption, otherwise false</param>
        public void EnableSessionSigningAndEncryption(
          ulong sessionId,
          bool enableSigning,
          bool enableEncryption)
        {
            if (!cryptoInfoTable.ContainsKey(sessionId))
                throw new InvalidOperationException("No crypto info could be found for this session.");

            cryptoInfoTable[sessionId].EnableSessionSigning = enableSigning;
            cryptoInfoTable[sessionId].EnableSessionEncryption = enableEncryption;
        }

        /// <summary>
        /// Set encryption for a specific share.
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <param name="treeId">Tree ID.</param>
        /// <param name="enableEncryption">True if encryption is enabled, false otherwise.</param>
        public void SetTreeEncryption(ulong sessionId, uint treeId, bool enableEncryption)
        {
            if (!cryptoInfoTable.ContainsKey(sessionId))
                throw new InvalidOperationException("Session wide signing and encryption settings must be set first by calling GenerateCryptoKeys.");

            if (enableEncryption)
                cryptoInfoTable[sessionId].EnableTreeEncryption.Add(treeId);
            else
                cryptoInfoTable[sessionId].EnableTreeEncryption.Remove(treeId);
        }

        /// <summary>
        /// MS-SMB2 section 3.2.4.25 Application Requests the Session Key for an Authenticated Context
        /// Application Requests the Session Key for an Authenticated Context
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <param name="sessionKey">Session Key for Higher Layer.</param>
        public byte[] GetSessionKeyForAuthenticatedContext(ulong sessionId)
        {
            if (!cryptoInfoTable.ContainsKey(sessionId))
                throw new InvalidOperationException("Session wide signing and encryption settings must be set first before calling GetSessionKeyForAuthenticatedContext.");

            if (Smb2Utility.IsSmb2Family(cryptoInfoTable[sessionId].Dialect)) return cryptoInfoTable[sessionId].SessionKey;
            else return cryptoInfoTable[sessionId].ApplicationKey;
        }

        #endregion

        #region Multi-Protocol Negotiate

        public uint MultiProtocolNegotiate(
            string[] dialects,
            out DialectRevision selectedDialect,
            out byte[] gssToken,
            out Packet_Header responseHeader,
            out NEGOTIATE_Response responsePayload)
        {
            Smb2NegotiateResponsePacket response;
            SmbNegotiateRequestPacket request;
            MultiProtocolNegotiate(dialects, out selectedDialect, out gssToken, out request, out response);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        public uint MultiProtocolNegotiate(
            string[] dialects,
            out DialectRevision selectedDialect,
            out byte[] gssToken,
            out SmbNegotiateRequestPacket request,
            out Smb2NegotiateResponsePacket response)
        {
            request = new SmbNegotiateRequestPacket();

            // Use the same flags windows SMB2 client sends
            // Some flags such as Unicode support are required by some SMB2 server implementations
            request.Header.Flags = 0x18;
            request.Header.Flags2 = 0xC853;
            request.Header.Tid = 0xFFFF;

            using (MemoryStream ms = new MemoryStream())
            {
                foreach (string dialect in dialects)
                {
                    ms.Write(new byte[] { SmbNegotiateRequestPacket.DialectFormatCharactor }, 0, sizeof(byte));
                    byte[] dialectArray = Encoding.ASCII.GetBytes(dialect + '\0');
                    ms.Write(dialectArray, 0, dialectArray.Length);
                }

                request.PayLoad.DialectName = ms.ToArray();
                request.PayLoad.ByteCount = (ushort)request.PayLoad.DialectName.Length;
            }

            response = SendPacketAndExpectResponse<Smb2NegotiateResponsePacket>(request);

            selectedDialect = response.PayLoad.DialectRevision;
            gssToken = response.Buffer.Skip(response.PayLoad.SecurityBufferOffset - response.BufferOffset).Take(response.PayLoad.SecurityBufferLength).ToArray();

            // set dialect if ComNegotiate returns SMB 2.002 because we'll stop negotiation in this situation
            if (selectedDialect == DialectRevision.Smb2002)
            {
                dialect = selectedDialect;
            }

            return response.Header.Status;
        }

        #endregion

        #region Negotiate
        public uint Negotiate(
             ushort creditCharge,
             ushort creditRequest,
             Packet_Header_Flags_Values flags,
             ulong messageId,
             DialectRevision[] dialects,
             SecurityMode_Values securityMode,
             Capabilities_Values capabilities,
             Guid clientGuid,
             out DialectRevision selectedDialect,
             out byte[] gssToken,
             out Smb2NegotiateRequestPacket request,
             out Smb2NegotiateResponsePacket response,
             ushort channelSequence = 0,
             PreauthIntegrityHashID[] preauthHashAlgs = null,
             EncryptionAlgorithm[] encryptionAlgs = null,
             CompressionAlgorithm[] compressionAlgorithms = null,
             SMB2_NETNAME_NEGOTIATE_CONTEXT_ID netNameContext = null,
             bool addDefaultEncryption = false
         )
        {
            request = new Smb2NegotiateRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.NEGOTIATE;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.Status = channelSequence;

            request.Dialects = dialects;
            request.PayLoad.DialectCount = (ushort)dialects.Length;

            request.PayLoad.SecurityMode = securityMode;
            request.PayLoad.Capabilities = capabilities;
            request.PayLoad.ClientGuid = clientGuid;

            if (preauthHashAlgs != null)
            {
                SMB2_PREAUTH_INTEGRITY_CAPABILITIES preauthCaps = new SMB2_PREAUTH_INTEGRITY_CAPABILITIES();
                preauthCaps.Header.ContextType = SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_PREAUTH_INTEGRITY_CAPABILITIES;
                preauthCaps.HashAlgorithmCount = (ushort)preauthHashAlgs.Length;
                preauthCaps.HashAlgorithms = preauthHashAlgs;

                int saltLen = Smb2Consts.PreauthIntegrityHashSaltLength;
                preauthCaps.SaltLength = (ushort)saltLen;
                preauthCaps.Salt = new byte[saltLen];
                new Random().NextBytes(preauthCaps.Salt);
                preauthCaps.Header.DataLength = (ushort)(preauthCaps.GetDataLength());
                request.NegotiateContext_PREAUTH = preauthCaps;

                request.PayLoad.NegotiateContextCount++;
            }

            if (encryptionAlgs != null)
            {
                SMB2_ENCRYPTION_CAPABILITIES encryptionCap = new SMB2_ENCRYPTION_CAPABILITIES();
                encryptionCap.Header.ContextType = SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_ENCRYPTION_CAPABILITIES;
                encryptionCap.CipherCount = (ushort)encryptionAlgs.Length;
                encryptionCap.Ciphers = encryptionAlgs;
                encryptionCap.Header.DataLength = (ushort)(encryptionCap.GetDataLength());
                request.NegotiateContext_ENCRYPTION = encryptionCap;

                request.PayLoad.NegotiateContextCount++;
            }

            if (compressionAlgorithms != null)
            {
                var compresssionCapbilities = new SMB2_COMPRESSION_CAPABILITIES();
                compresssionCapbilities.Header.ContextType = SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_COMPRESSION_CAPABILITIES;
                compresssionCapbilities.CompressionAlgorithmCount = (ushort)compressionAlgorithms.Length;
                compresssionCapbilities.Padding = 0;
                compresssionCapbilities.Reserved = 0;
                compresssionCapbilities.CompressionAlgorithms = compressionAlgorithms;
                compresssionCapbilities.Header.DataLength = (ushort)(compresssionCapbilities.GetDataLength());
                request.NegotiateContext_COMPRESSION = compresssionCapbilities;

                request.PayLoad.NegotiateContextCount++;
            }

            if (netNameContext != null)
            {
                request.NegotiateContext_NETNAME = netNameContext;
                request.PayLoad.NegotiateContextCount++;
            }

            if (request.PayLoad.NegotiateContextCount > 0)
            {
                request.PayLoad.NegotiateContextOffset = (uint)(64 + // Header
                    36 + // fixed payload
                    2 * request.PayLoad.DialectCount); // dialect size

                // 8-byte align
                Smb2Utility.Align8(ref request.PayLoad.NegotiateContextOffset);
            }

            response = SendPacketAndExpectResponse<Smb2NegotiateResponsePacket>(request);

            selectedDialect = response.PayLoad.DialectRevision;
            gssToken = response.Buffer.Skip(response.PayLoad.SecurityBufferOffset - response.BufferOffset).Take(response.PayLoad.SecurityBufferLength).ToArray();

            dialect = response.PayLoad.DialectRevision;

            if (dialect >= DialectRevision.Smb311 && dialect != DialectRevision.Smb2Unknown)
            {
                if (response.NegotiateContext_PREAUTH != null)
                {
                    this.hashId = response.NegotiateContext_PREAUTH.Value.HashAlgorithms[0];
                }
                if (response.NegotiateContext_ENCRYPTION != null)
                {
                    this.cipherId = response.NegotiateContext_ENCRYPTION.Value.Ciphers[0];
                }
                if (response.NegotiateContext_COMPRESSION != null)
                {
                    UpdateNegotiateContext(compressionAlgorithms, response);
                }

                // In SMB 311, client use SMB2_ENCRYPTION_CAPABILITIES context to indicate whether it 
                // support Encryption rather than SMB2_GLOBAL_CAP_ENCRYPTION as SMB 30/302
                // For those client with dialect 311 but not support encryption cases (typically in encryption model cases), 
                // we shouldn't set the default encryption algorithm so that the SMB2_ENCRYPTION_CAPABILITIES won't be added.
                if (addDefaultEncryption && response.NegotiateContext_ENCRYPTION == null)
                {
                    this.cipherId = EncryptionAlgorithm.ENCRYPTION_AES128_CCM;
                }

                preauthContext = new PreauthIntegrityContext(hashId);
                preauthContext.UpdateConnectionState(request);
                preauthContext.UpdateConnectionState(response);
            }

            return response.Header.Status;
        }

        public uint Negotiate(
             ushort creditCharge,
             ushort creditRequest,
             Packet_Header_Flags_Values flags,
             ulong messageId,
             DialectRevision[] dialects,
             SecurityMode_Values securityMode,
             Capabilities_Values capabilities,
             Guid clientGuid,
             out DialectRevision selectedDialect,
             out byte[] gssToken,
             out Packet_Header responseHeader,
             out NEGOTIATE_Response responsePayload,
             ushort channelSequence = 0,
             PreauthIntegrityHashID[] preauthHashAlgs = null,
             EncryptionAlgorithm[] encryptionAlgs = null,
             CompressionAlgorithm[] compressionAlgorithms = null,
             SMB2_NETNAME_NEGOTIATE_CONTEXT_ID netNameContext = null,
             bool addDefaultEncryption = false
         )
        {
            Smb2NegotiateRequestPacket request;
            Smb2NegotiateResponsePacket response;
            Negotiate(creditCharge, creditRequest, flags, messageId, dialects, securityMode, capabilities, clientGuid, out selectedDialect, out gssToken, out request, out response,
                channelSequence, preauthHashAlgs, encryptionAlgs, compressionAlgorithms, netNameContext, addDefaultEncryption);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        private void UpdateNegotiateContext(CompressionAlgorithm[] compressionAlgorithms, Smb2NegotiateResponsePacket response)
        {
            // If CompressionAlgorithmCount is zero, the client MUST return an error to the calling application.
            if (response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithmCount == 0)
            {
                throw new InvalidOperationException("CompressionAlgorithmCount should not be zero!");
            }

            // If the length of the negotiate context is greater than DataLength of the negotiate context, the client MUST return an error to the calling application.
            if (response.NegotiateContext_COMPRESSION.Value.GetDataLength() > response.NegotiateContext_COMPRESSION.Value.Header.DataLength)
            {
                throw new InvalidOperationException("DataLength is inconsistent with context size!");
            }

            // For each algorithm in CompressionAlgorithms, if the value of algorithm is greater than 32, the client MUST return an error to the calling application.
            if (response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithms.Any(compressionAlgorithm => (ushort)compressionAlgorithm > 32))
            {
                throw new InvalidOperationException("Each item in CompressionAlgorithms should not be greater than 32!");
            }

            // If there is a duplicate value in CompressionAlgorithms, the client MUST return an error to the calling application.
            if (response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithms.GroupBy(compressionAlgorithm => compressionAlgorithm).Any(compressionAlgorithm => compressionAlgorithm.Count() > 1))
            {
                throw new InvalidOperationException("Duplicate item is found in CompressionAlgorithms!");
            }

            if (response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithmCount == 1 && response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithms[0] == CompressionAlgorithm.NONE)
            {
                // If CompressionAlgorithmCount is 1 and CompressionAlgorithms contains "NONE", the client MUST set Connection.CompressionIds to an empty list.
                // If Connection.CompressionIds is empty,
                // Set CompressionAlgorithmCount to 1.
                // Set CompressionAlgorithms to "NONE".
                this.CompressionInfo.CompressionIds = response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithms;
            }
            else if (response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithms.Any(compressionAlgorithm => !compressionAlgorithms.Contains(compressionAlgorithm)))
            {
                // Otherwise, for each algorithm in CompressionAlgorithms, if the value of algorithm does not match any of the algorithms sent in SMB2 NEGOTIATE request, the client MUST return an error to the calling application
                throw new InvalidOperationException("Each item in CompressionAlgorithms should be some one sent in request!");
            }
            else
            {
                // Otherwise, the client MUST set Connection.CompressionIds to all the algorithms received in CompressionAlgorithms.
                // Otherwise,
                // Set CompressionAlgorithmCount to the number of compression algorithms in Connection.CompressionIds.
                // Set CompressionAlgorithms to Connection.CompressionIds.
                this.CompressionInfo.CompressionIds = response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithms;
            }
        }

        #endregion

        #region SessionSetup
        /// <summary>
        /// Basic SessionSetup Call.
        /// </summary>
        /// <param name="creditCharge">The number of credits that this request consumes.</param>
        /// <param name="creditRequest">The number of credits the client is requesting</param>
        /// <param name="flags">A Flags field indicates how to process the operation.</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely across all messages.</param>
        /// <param name="sessionId">For new SessionSetup,set to 0; For use exist session, set to the correct messageId.</param>
        /// <param name="sessionSetupFlags">To bind an existing session to a new connection,set to SMB2_SESSION_FLAG_BINDING to bind; otherwise set it to NONE. </param>
        /// <param name="securityMode">The security mode field specifies whether SMB signing is enabled, required at the server, or both</param>
        /// <param name="capabilities">Specifies protocol capabilities for the client.</param>
        /// <param name="previousSessionId">For reconnect, set it to previous sessionId, otherwise set it to 0.</param>
        /// <param name="clientGssToken">Gss token return from Negotiate call.</param>
        /// <param name="serverSessionId">A valid sessionId return from server.</param>
        /// <param name="serverGssToken">A valid GssToken return from server.</param>
        /// <param name="responseHeader">SMB2 response header.</param>
        /// <param name="responsePayload">SMB2 SESSION_SETUP Response packet.</param>
        /// <returns>The status code for SESSION_SETUP Response.</returns>
        public uint SessionSetup(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            SESSION_SETUP_Request_Flags sessionSetupFlags,
            SESSION_SETUP_Request_SecurityMode_Values securityMode,
            SESSION_SETUP_Request_Capabilities_Values capabilities,
            ulong previousSessionId,
            byte[] clientGssToken,
            out ulong serverSessionId,
            out byte[] serverGssToken,
            out Packet_Header responseHeader,
            out SESSION_SETUP_Response responsePayload,
            ushort channelSequence = 0)
        {
            var request = new Smb2SessionSetupRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.SESSION_SETUP;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.Flags = sessionSetupFlags;
            request.PayLoad.SecurityMode = securityMode;
            request.PayLoad.Capabilities = capabilities;
            request.PayLoad.PreviousSessionId = previousSessionId;

            request.Buffer = clientGssToken;
            request.PayLoad.SecurityBufferOffset = request.BufferOffset;
            request.PayLoad.SecurityBufferLength = (ushort)request.Buffer.Length;

            var response = SendPacketAndExpectResponse<Smb2SessionSetupResponsePacket>(request);
            this.sessionSetupResponse = response;

            serverSessionId = response.Header.SessionId;
            serverGssToken = response.Buffer.Skip(response.PayLoad.SecurityBufferOffset - response.BufferOffset).Take(response.PayLoad.SecurityBufferLength).ToArray();

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            // Only update the context when session setup response returns STATUS_SUCCESS or STATUS_MORE_PROCESSING_REQUIRED
            if (preauthContext != null && (response.Header.Status == Smb2Status.STATUS_SUCCESS || response.Header.Status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED))
            {
                ulong responseSessionId = response.Header.SessionId;
                preauthContext.UpdateSessionState(responseSessionId, request);
                preauthContext.UpdateSessionState(responseSessionId, response);
            }

            return response.Header.Status;
        }

        #endregion

        #region Verify Signature of Session Setup Response
        public void TryVerifySessionSetupResponseSignature(ulong sessionId)
        {
            decoder.TryVerifySessionSetupResponseSignature(sessionSetupResponse, sessionId, sessionSetupResponse.MessageBytes);
        }
        #endregion

        #region LogOff

        public uint LogOff(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            out Packet_Header responseHeader,
            out LOGOFF_Response responsePayload,
            ushort channelSequence = 0)
        {
            var request = new Smb2LogOffRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.LOGOFF;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.MessageId = messageId;
            request.Header.Flags = flags;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            var response = SendPacketAndExpectResponse<Smb2LogOffResponsePacket>(request);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region TreeConnect

        public uint TreeConnect(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            int pathLength,
            byte[] buffer,
            out uint treeId,
            out Packet_Header responseHeader,
            out TREE_CONNECT_Response responsePayload,
            ushort channelSequence = 0,
            TreeConnect_Flags treeConnectFlags = TreeConnect_Flags.SMB2_SHAREFLAG_NONE)
        {
            var request = new Smb2TreeConnectRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.TREE_CONNECT;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.MessageId = messageId;
            request.Header.Flags = flags;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.Buffer = buffer;
            if (treeConnectFlags.HasFlag(TreeConnect_Flags.SMB2_SHAREFLAG_EXTENSION_PRESENT))
            {
                request.PayLoad.PathOffset = (ushort)(request.BufferOffset +
                                                      4 +   // TreeConnectContextOffset
                                                      2 +   // TreeConnectContextCount
                                                      10);  // Reserved
            }
            else
            {
                request.PayLoad.PathOffset = request.BufferOffset;
            }
            request.PayLoad.PathLength = (ushort)pathLength;
            request.PayLoad.Flags = treeConnectFlags;

            var response = SendPacketAndExpectResponse<Smb2TreeConnectResponsePacket>(request);

            treeId = response.Header.TreeId;
            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        public uint TreeConnect(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            string path,
            out uint treeId,
            out Packet_Header responseHeader,
            out TREE_CONNECT_Response responsePayload,
            ushort channelSequence = 0,
            TreeConnect_Flags treeConnectFlags = TreeConnect_Flags.SMB2_SHAREFLAG_NONE)
        {
            var request = new Smb2TreeConnectRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.TREE_CONNECT;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.MessageId = messageId;
            request.Header.Flags = flags;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.Flags = treeConnectFlags;

            request.Buffer = Encoding.Unicode.GetBytes(path);
            request.PayLoad.PathOffset = request.BufferOffset;
            request.PayLoad.PathLength = (ushort)request.Buffer.Length;

            var response = SendPacketAndExpectResponse<Smb2TreeConnectResponsePacket>(request);

            treeId = response.Header.TreeId;

            responseHeader = response.Header;
            responsePayload = response.PayLoad;
            error = response.Error;

            return response.Header.Status;
        }

        #endregion

        #region TreeDisconnect

        public uint TreeDisconnect(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            out Packet_Header responseHeader,
            out TREE_DISCONNECT_Response responsePayload,
            ushort channelSequence = 0)
        {
            var request = new Smb2TreeDisconnectRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.TREE_DISCONNECT;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.MessageId = messageId;
            request.Header.Flags = flags;
            request.Header.SessionId = sessionId;
            request.Header.TreeId = treeId;
            request.Header.Status = channelSequence;

            var response = SendPacketAndExpectResponse<Smb2TreeDisconnectResponsePacket>(request);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Create

        public uint Create(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            string path,
            AccessMask desiredAccess,
            ShareAccess_Values shareAccess,
            CreateOptions_Values createOptions,
            CreateDisposition_Values createDispositions,
            File_Attributes fileAttributes,
            ImpersonationLevel_Values impersonationLevel,
            SecurityFlags_Values securityFlag,
            RequestedOplockLevel_Values requestedOplockLevel,
            Smb2CreateContextRequest[] createContexts,
            out FILEID fileId,
            out Smb2CreateContextResponse[] serverCreateContexts,
            out Packet_Header responseHeader,
            out CREATE_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            CreateRequest(creditCharge, creditRequest, flags, messageId, sessionId, treeId, path,
                desiredAccess, shareAccess, createOptions, createDispositions, fileAttributes, impersonationLevel, securityFlag, requestedOplockLevel, createContexts, channelSequence);

            return CreateResponse(messageId, out fileId, out serverCreateContexts, out responseHeader, out responsePayload);
        }

        public void CreateRequest(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            string path,
            AccessMask desiredAccess,
            ShareAccess_Values shareAccess,
            CreateOptions_Values createOptions,
            CreateDisposition_Values createDispositions,
            File_Attributes fileAttributes,
            ImpersonationLevel_Values impersonationLevel,
            SecurityFlags_Values securityFlag,
            RequestedOplockLevel_Values requestedOplockLevel,
            Smb2CreateContextRequest[] createContexts,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2CreateRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.CREATE;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.MessageId = messageId;
            request.Header.Flags = flags;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.SecurityFlags = SecurityFlags_Values.NONE;
            request.PayLoad.RequestedOplockLevel = requestedOplockLevel;
            request.PayLoad.ImpersonationLevel = impersonationLevel;
            request.PayLoad.DesiredAccess = desiredAccess;
            request.PayLoad.FileAttributes = fileAttributes;
            request.PayLoad.ShareAccess = shareAccess;
            request.PayLoad.CreateDisposition = createDispositions;
            request.PayLoad.CreateOptions = createOptions;

            if (string.IsNullOrEmpty(path))
            {
                request.PayLoad.NameOffset = request.BufferOffset;
                request.PayLoad.NameLength = 0;
            }
            else
            {
                byte[] nameBuffer = Encoding.Unicode.GetBytes(path);
                request.PayLoad.NameOffset = request.BufferOffset;
                request.PayLoad.NameLength = (ushort)nameBuffer.Length;
                request.Buffer = nameBuffer;
            }

            if (createContexts != null && createContexts.Length > 0)
            {
                Smb2Utility.Align8(ref request.Buffer);
                byte[] createContextValuesBuffer = Smb2Utility.MarshalCreateContextRequests(createContexts);
                request.PayLoad.CreateContextsOffset = (uint)(request.BufferOffset + request.Buffer.Length);
                request.PayLoad.CreateContextsLength = (uint)createContextValuesBuffer.Length;
                if (request.Buffer.Length == 0)
                {
                    request.Buffer = createContextValuesBuffer;
                }
                else
                {
                    request.Buffer = request.Buffer.Concat(createContextValuesBuffer).ToArray();
                }
            }

            if (request.Buffer.Length == 0)
            {
                // In the request, the Buffer field MUST be at least one byte in length.
                request.Buffer = new byte[1];
            }

            SendPacket(request);
        }

        public uint CreateResponse(
            ulong messageId,
            out FILEID fileId,
            out Smb2CreateContextResponse[] serverCreateContexts,
            out Packet_Header responseHeader,
            out CREATE_Response responsePayload
            )
        {
            var response = ExpectPacket<Smb2CreateResponsePacket>(messageId);

            fileId = response.PayLoad.FileId;

            serverCreateContexts = null;
            if (response.PayLoad.CreateContextsLength > 0)
            {
                byte[] serverCreateContextValuesBuffer = response.Buffer.Skip((int)response.PayLoad.CreateContextsOffset - response.BufferOffset).Take((int)response.PayLoad.CreateContextsLength).ToArray();

                serverCreateContexts = Smb2Utility.UnmarshalCreateContextResponses(serverCreateContextValuesBuffer);
            }

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Close

        public uint Close(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            FILEID fileId,
            Flags_Values closeFlags,
            out Packet_Header responseHeader,
            out CLOSE_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2CloseRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.CLOSE;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.FileId = fileId;

            request.PayLoad.Flags = closeFlags;

            var response = SendPacketAndExpectResponse<Smb2CloseResponsePacket>(request);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Flush

        public uint Flush(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            FILEID fileId,
            out Packet_Header responseHeader,
            out FLUSH_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2FlushRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.FLUSH;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.FileId = fileId;

            var response = SendPacketAndExpectResponse<Smb2FlushResponsePacket>(request);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Read

        public uint Read(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            uint length,
            ulong offset,
            FILEID fileId,
            uint minimumCount,
            Channel_Values channel,
            uint remainingBytes,
            byte[] readChannelInfo,
            out byte[] content,
            out Packet_Header responseHeader,
            out READ_Response responsePayload,
            ushort channelSequence = 0,
            bool compressRead = false
            )
        {
            var request = new Smb2ReadRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.READ;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.Length = length;
            request.PayLoad.Offset = offset;
            request.PayLoad.FileId = fileId;
            request.PayLoad.MinimumCount = minimumCount;
            request.PayLoad.Channel = channel;
            request.PayLoad.RemainingBytes = remainingBytes;
            request.PayLoad.Flags = READ_Request_Flags_Values.ZERO;
            if (compressRead)
            {
                request.PayLoad.Flags |= READ_Request_Flags_Values.SMB2_READFLAG_REQUEST_COMPRESSED;
            }

            if (readChannelInfo != null && readChannelInfo.Length > 0)
            {
                request.PayLoad.ReadChannelInfoOffset = request.BufferOffset;
                request.PayLoad.ReadChannelInfoLength = (ushort)readChannelInfo.Length;
                request.Buffer = readChannelInfo.ToArray();
            }

            var response = SendPacketAndExpectResponse<Smb2ReadResponsePacket>(request);

            if (response.Error == null)
            {
                content = response.Buffer.Skip((int)(response.PayLoad.DataOffset - response.BufferOffset)).Take((int)response.PayLoad.DataLength).ToArray();
            }
            else
            {
                content = null;
            }

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Write

        public uint Write(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            ulong offset,
            FILEID fileId,
            Channel_Values channel,
            WRITE_Request_Flags_Values writeFlags,
            byte[] writeChannelInfo,
            byte[] content,
            out Packet_Header responseHeader,
            out WRITE_Response responsePayload,
            ushort channelSequence = 0,
            bool compressWrite = false
            )
        {
            WriteRequest(creditCharge, creditRequest, flags, messageId, sessionId, treeId, offset, fileId, channel, writeFlags, writeChannelInfo, content, channelSequence, compressWrite);

            return WriteResponse(messageId, out responseHeader, out responsePayload);
        }

        public void WriteRequest(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            ulong offset,
            FILEID fileId,
            Channel_Values channel,
            WRITE_Request_Flags_Values writeFlags,
            byte[] writeChannelInfo,
            byte[] content,
            ushort channelSequence = 0,
            bool compressWrite = false
            )
        {
            var request = new Smb2WriteRequestPacket();

            // SMB2 header
            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.WRITE;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            // SMB2 WRITE request
            request.PayLoad.Channel = channel;

            if (channel == Channel_Values.CHANNEL_NONE)
            {
                // Non-RDMA
                request.PayLoad.WriteChannelInfoOffset = 0;
                request.PayLoad.WriteChannelInfoLength = 0;
                request.PayLoad.RemainingBytes = 0;
                request.PayLoad.DataOffset = request.BufferOffset;
                request.PayLoad.Length = (uint)content.Length;
                request.Buffer = content.ToArray();
            }
            else
            {
                // RDMA
                request.PayLoad.WriteChannelInfoOffset = request.BufferOffset;
                request.PayLoad.WriteChannelInfoLength = (ushort)writeChannelInfo.Length;
                request.PayLoad.RemainingBytes = (uint)content.Length;
                request.PayLoad.DataOffset = 0;
                request.PayLoad.Length = 0;
                request.Buffer = writeChannelInfo.ToArray();
            }

            request.PayLoad.Offset = offset;

            request.PayLoad.FileId = fileId;

            request.PayLoad.Flags = writeFlags;

            // SMB2 compression
            request.EligibleForCompression = compressWrite;

            SendPacket(request);
        }

        public uint WriteResponse(
            ulong messageId,
            out Packet_Header responseHeader,
            out WRITE_Response responsePayload
            )
        {
            var response = ExpectPacket<Smb2WriteResponsePacket>(messageId);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region OplockBreakAcknowledgment

        public uint OplockBreakAcknowledgment(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            FILEID fileId,
            OPLOCK_BREAK_Acknowledgment_OplockLevel_Values oplockLevel,
            out Packet_Header responseHeader,
            out OPLOCK_BREAK_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2OpLockBreakAckPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.OPLOCK_BREAK;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.FileId = fileId;
            request.PayLoad.OplockLevel = oplockLevel;

            var response = SendPacketAndExpectResponse<Smb2OpLockBreakResponsePacket>(request);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region LeaseBreakAcknowledgment

        public uint LeaseBreakAcknowledgment(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            Guid leaseKey,
            LeaseStateValues leaseState,
            out Packet_Header responseHeader,
            out LEASE_BREAK_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2LeaseBreakAckPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.OPLOCK_BREAK;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.LeaseKey = leaseKey;
            request.PayLoad.LeaseState = leaseState;

            var response = SendPacketAndExpectResponse<Smb2LeaseBreakResponsePacket>(request);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Lock

        public uint Lock(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            uint lockSequence,
            FILEID fileId,
            LOCK_ELEMENT[] locks,
            out Packet_Header responseHeader,
            out LOCK_Response responsePayload
            )
        {
            this.LockRequest(creditCharge, creditRequest, flags, messageId, sessionId, treeId, lockSequence, fileId, locks);

            return this.LockResponse(messageId, out responseHeader, out responsePayload);
        }

        public void LockRequest(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            uint lockSequence,
            FILEID fileId,
            LOCK_ELEMENT[] locks,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2LockRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.LOCK;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.FileId = fileId;

            request.PayLoad.LockSequence = lockSequence;

            request.Locks = locks;
            request.PayLoad.LockCount = (ushort)locks.Length;

            SendPacket(request);
        }

        public uint LockResponse(
            ulong messageId,
            out Packet_Header responseHeader,
            out LOCK_Response responsePayload
            )
        {
            var response = ExpectPacket<Smb2LockResponsePacket>(messageId);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Echo

        public uint Echo(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            out Packet_Header responseHeader,
            out ECHO_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2EchoRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.ECHO;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            var response = SendPacketAndExpectResponse<Smb2EchoResponsePacket>(request);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region Cancel

        public void Cancel(
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2EchoRequestPacket();

            request.Header.Command = Smb2Command.CANCEL;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            SendPacket(request);
        }

        #endregion

        #region IOCTL

        public uint IoCtl(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            CtlCode_Values ctlCode,
            FILEID fileId,
            uint maxInputResponse,
            byte[] requestInput,
            uint maxOutputResponse,
            IOCTL_Request_Flags_Values ioCtlFlags,
            out byte[] responseInput,
            out byte[] responseOutput,
            out Packet_Header responseHeader,
            out IOCTL_Response responsePayload,
            ushort channelSequence = 0)
        {
            var request = new Smb2IOCtlRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.IOCTL;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.CtlCode = ctlCode;
            request.PayLoad.FileId = fileId;

            if (requestInput != null && requestInput.Length > 0)
            {
                request.PayLoad.InputOffset = request.BufferOffset;
                request.PayLoad.InputCount = (ushort)requestInput.Length;
                request.Buffer = requestInput;
            }

            request.PayLoad.MaxInputResponse = maxInputResponse;
            request.PayLoad.MaxOutputResponse = maxOutputResponse;
            request.PayLoad.Flags = ioCtlFlags;

            var response = SendPacketAndExpectResponse<Smb2IOCtlResponsePacket>(request);

            responseInput = null;
            if (response.PayLoad.InputCount > 0)
            {
                responseInput = response.Buffer.Skip((int)(response.PayLoad.InputOffset - response.BufferOffset)).Take((int)response.PayLoad.InputCount).ToArray();
            }

            responseOutput = null;
            if (response.PayLoad.OutputCount > 0)
            {
                responseOutput = response.Buffer.Skip((int)(response.PayLoad.OutputOffset - response.BufferOffset)).Take((int)response.PayLoad.OutputCount).ToArray();
            }

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region QueryDirectory

        public uint QueryDirectory(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            FileInformationClass_Values fileInfoClass,
            QUERY_DIRECTORY_Request_Flags_Values queryDirectoryFlags,
            uint fileIndex,
            FILEID fileId,
            string fileName,
            uint maxOutputBufferLength,
            out byte[] outputBuffer,
            out Packet_Header responseHeader,
            out QUERY_DIRECTORY_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2QueryDirectoryRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.QUERY_DIRECTORY;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.FileInformationClass = fileInfoClass;
            request.PayLoad.Flags = queryDirectoryFlags;
            request.PayLoad.FileIndex = fileIndex;
            request.PayLoad.FileId = fileId;
            request.Buffer = Encoding.Unicode.GetBytes(fileName); //TD: A variable-length buffer containing the Unicode search pattern for the request
            request.PayLoad.FileNameOffset = request.BufferOffset;
            request.PayLoad.FileNameLength = (ushort)request.Buffer.Length; //TD:The length of the search pattern
            request.PayLoad.OutputBufferLength = maxOutputBufferLength;

            var response = SendPacketAndExpectResponse<Smb2QueryDirectoryResponePacket>(request);

            if (response.PayLoad.OutputBufferLength > 0)
            {
                outputBuffer = response.Buffer.Skip((int)(response.PayLoad.OutputBufferOffset - response.BufferOffset)).Take((int)response.PayLoad.OutputBufferLength).ToArray();
            }
            else
            {
                outputBuffer = null;
            }

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region ChangeNotify

        public void ChangeNotify(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            uint maxOutputBufferLength,
            FILEID fileId,
            CHANGE_NOTIFY_Request_Flags_Values changeNotifyFlag,
            CompletionFilter_Values completionFilter,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2ChangeNotifyRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.CHANGE_NOTIFY;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.OutputBufferLength = maxOutputBufferLength;
            request.PayLoad.Flags = changeNotifyFlag;
            request.PayLoad.FileId = fileId;
            request.PayLoad.CompletionFilter = completionFilter;

            SendPacket(request);

            // Do not wait for response since it must be triggered by another action
        }
        #endregion

        #region QueryInfo

        public uint QueryInfo(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            InfoType_Values infoType,
            byte fileInfoClass,
            uint maxOutputBufferLength,
            AdditionalInformation_Values additionalInfo,
            QUERY_INFO_Request_Flags_Values queryInfoFlags,
            FILEID fileId,
            byte[] inputBuffer,
            out byte[] outputBuffer,
            out Packet_Header responseHeader,
            out QUERY_INFO_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2QueryInfoRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.QUERY_INFO;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.InfoType = infoType;
            request.PayLoad.FileInfoClass = fileInfoClass;
            request.PayLoad.OutputBufferLength = maxOutputBufferLength;

            if (inputBuffer != null && inputBuffer.Length > 0)
            {
                request.PayLoad.InputBufferOffset = request.BufferOffset;
                request.PayLoad.InputBufferLength = (ushort)inputBuffer.Length;
                request.Buffer = inputBuffer;
            }

            request.PayLoad.AdditionalInformation = additionalInfo;
            request.PayLoad.Flags = queryInfoFlags;
            request.PayLoad.FileId = fileId;

            var response = SendPacketAndExpectResponse<Smb2QueryInfoResponsePacket>(request);

            outputBuffer = null;
            if (response.PayLoad.OutputBufferLength > 0)
            {
                outputBuffer = response.Buffer.Skip((int)(response.PayLoad.OutputBufferOffset - response.BufferOffset)).Take((int)response.PayLoad.OutputBufferLength).ToArray();
            }

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region SetInfo

        public uint SetInfo(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            SET_INFO_Request_InfoType_Values infoType,
            byte fileInfoClass,
            SET_INFO_Request_AdditionalInformation_Values additionalInfo,
            FILEID fileId,
            byte[] inputBuffer,
            out Packet_Header responseHeader,
            out SET_INFO_Response responsePayload,
            ushort channelSequence = 0
            )
        {
            SetInfoRequest(creditCharge, creditRequest, flags, messageId, sessionId, treeId,
                infoType, fileInfoClass, additionalInfo, fileId, inputBuffer, channelSequence);

            return SetInfoResponse(messageId, out responseHeader, out responsePayload);
        }

        public void SetInfoRequest(
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ulong messageId,
            ulong sessionId,
            uint treeId,
            SET_INFO_Request_InfoType_Values infoType,
            byte fileInfoClass,
            SET_INFO_Request_AdditionalInformation_Values additionalInfo,
            FILEID fileId,
            byte[] inputBuffer,
            ushort channelSequence = 0
            )
        {
            var request = new Smb2SetInfoRequestPacket();

            request.Header.CreditCharge = creditCharge;
            request.Header.Command = Smb2Command.SET_INFO;
            request.Header.CreditRequestResponse = creditRequest;
            request.Header.Flags = flags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;
            request.Header.Status = channelSequence;

            request.PayLoad.InfoType = infoType;
            request.PayLoad.FileInfoClass = fileInfoClass;

            if (inputBuffer != null && inputBuffer.Length > 0)
            {
                request.PayLoad.BufferOffset = request.BufferOffset;
                request.PayLoad.BufferLength = (uint)inputBuffer.Length;
                request.Buffer = inputBuffer;
            }

            request.PayLoad.AdditionalInformation = additionalInfo;
            request.PayLoad.FileId = fileId;

            SendPacket(request);
        }

        public uint SetInfoResponse(
            ulong messageId,
            out Packet_Header responseHeader,
            out SET_INFO_Response responsePayload
            )
        {
            var response = ExpectPacket<Smb2SetInfoResponsePacket>(messageId);

            responseHeader = response.Header;
            responsePayload = response.PayLoad;

            return response.Header.Status;
        }

        #endregion

        #region IDispose

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicate user or GC calling this method</param>
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Disconnect();
                }

                disposed = true;
            }
        }


        /// <summary>
        /// Deconstructor
        /// </summary>
        ~Smb2Client()
        {
            Dispose(false);
        }

        #endregion
    }
}
