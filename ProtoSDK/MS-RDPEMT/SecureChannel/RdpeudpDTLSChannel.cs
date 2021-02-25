// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
// using Microsoft.Protocols.TestTools.ExtendedLogging;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiService;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    [SupportedOSPlatform("Windows")]
    public class RdpeudpDTLSChannel : ISecureChannel
    {
        #region Private variables

        /// <summary>
        /// milliseconds for method sleep time when wait for data
        /// </summary>
        const int waitInterval = 50;

        /// <summary>
        /// The time in seconds to wait for next packets from low layer transport.
        /// </summary>
        public TimeSpan timeout = new TimeSpan(0, 0, 20);

        /// <summary>
        /// Short wait time
        /// </summary>
        private TimeSpan shortWaitTime = new TimeSpan(0, 0, 0, 0, 100);

        /// <summary>
        /// A RdpeudpSocket which is the transport of this security channel
        /// </summary>
        private RdpeudpSocket rdpeudpSocket;

        /// <summary>
        /// buffer used for received data, which is received from RDPEUDP transport.
        /// </summary> 
        private List<byte[]> receivedBuffer;

        /// <summary>
        /// buffer used for data to be sent, which will be sent to RDPEUDP transport
        /// </summary>
        private List<byte[]> toSendBuffer;

        /// <summary>
        /// Security context, which used by DTLS server.
        /// Accept client token to get server token.
        /// </summary>
        private DtlsServerSecurityContext dtlsServerContext;

        /// <summary>
        /// SecurityContext used by DTLS client.
        /// Supports DTLS 1.0
        /// Invokes InitializeSecurityContext function of SSPI
        /// </summary>
        private DtlsClientSecurityContext dtlsClientContext;

        private SecurityPackageContextStreamSizes dtlsStreamSizes;

        private bool isAuthenticated;

        #endregion Private variables

        #region Properties

        /// <summary>
        /// Whether this channel has been authenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return isAuthenticated;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eudpSocket">RDPEUDP Socket, which is transport</param>
        public RdpeudpDTLSChannel(RdpeudpSocket eudpSocket)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("The implementation of RdpeudpDTLSChannel is only supported on Windows.");
            }

            if (eudpSocket == null || eudpSocket.TransMode == TransportMode.Reliable)
            {
                throw new NotSupportedException("RdpeudpSocket is null or it is a socket for reliable RDPEUDP connection.");
            }

            this.rdpeudpSocket = eudpSocket;
            this.rdpeudpSocket.Received += ReceiveBytes;
            isAuthenticated = false;

            rdpeudpSocket.Disconnected += RdpeudpSocket_Disconnected;

            receivedBuffer = new List<byte[]>();
            toSendBuffer = new List<byte[]>();

            if (eudpSocket.AutoHandle)
            {
                // Check whether there is packets in unprocessed packet buffer

                RdpeudpPacket packet = eudpSocket.ExpectPacket(shortWaitTime);
                if (packet != null)
                {
                    eudpSocket.ReceivePacket(packet);
                }
            }
        }

        #endregion Constructor

        #region Public Methods

        #region Implemented ISecurityChannel Interfaces

        /// <summary>
        /// Send bytes through this security channel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        public void Send(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                List<byte[]> toSentList = Encrypt(data);
                if (toSentList != null)
                {
                    foreach (byte[] toSentData in toSentList)
                    {
                        rdpeudpSocket.Send(toSentData);
                    }
                }

                // ETW Provider Dump Message
                string messageName = "RDPEMT:SentPDU";
                // ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer2, "RDPEMT Sent PDU", data);
            }
        }

        /// <summary>
        /// Event be called when received data
        /// </summary>
        public event ReceiveData Received;

        /// <summary>
        /// Event triggered when connection is closed.
        /// </summary>
        public event DisconnectedHandler Disconnected;

        #endregion Implemented ISecurityChannel Interfaces

        #region Authenticate methods

        /// <summary>
        /// Called by servers to authenticate the server and optionally the client in
        /// a client-server connection using the specified certificate.
        /// </summary>
        /// <param name="cert">The certificate used to authenticate the server.</param>
        public void AuthenticateAsServer(X509Certificate cert)
        {
            var data = new AuthenticateAsServerData
            {
                Certificate = cert,
            };

            // Using thread in threadpool to manage the authentication process
            ThreadPool.QueueUserWorkItem(AuthenticateAsServerTask, data, false);

            DateTime endTime = DateTime.Now + timeout;

            if (rdpeudpSocket.AutoHandle)
            {
                while (!IsAuthenticated && DateTime.Now < endTime)
                {
                    Thread.Sleep(waitInterval);
                }
                if (!IsAuthenticated)
                {
                    if (data.Exception != null)
                    {
                        throw data.Exception;
                    }

                    throw new TimeoutException("Time out when Authenticate as Server!");
                }
            }
        }

        /// <summary>
        /// Called by clients to authenticate the server and optionally the client in
        /// a client-server connection.
        /// </summary>
        /// <param name="targetHost">The name of the server that shares this System.Net.Security.SslStream.</param>
        /// <param name="certValCallback">A System.Net.Security.RemoteCertificateValidationCallback delegate responsible for validating the certificate supplied by the remote party.</param>
        public void AuthenticateAsClient(string targetHost)
        {
            // Using thread in threadpool to manage the authentication process
            ThreadPool.QueueUserWorkItem(AuthenticateAsClient, targetHost);

            DateTime endTime = DateTime.Now + timeout;

            if (rdpeudpSocket.AutoHandle)
            {
                while (!IsAuthenticated && DateTime.Now < endTime)
                {
                    Thread.Sleep(waitInterval);
                }
                if (!IsAuthenticated)
                {
                    throw new TimeoutException("Time out when Authenticate as Client!");
                }
            }
        }

        #endregion Authenticate methods

        /// <summary>
        /// Process bytes received from 
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveBytes(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                if (isAuthenticated)
                {
                    byte[] decryptedData = Decrypt(data);

                    // ETW Provider Dump Message
                    string messageName = "RDPEMT:ReceivedPDU";
                    // ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer2, "RDPEMT Received PDU", decryptedData);

                    if (Received != null)
                    {
                        Received(decryptedData);
                    }
                }
                else
                {
                    this.AddReceivedData(data);
                }
            }
        }


        /// <summary>
        /// Send source Data to remote endpoint through DTLS transport.
        /// </summary>
        /// <param name="data">The sending data.</param>
        public List<byte[]> Encrypt(byte[] data)
        {
            if (data == null) return null;
            List<byte[]> encryptedDataList = new List<byte[]>();
            int consumedLen = 0;
            while (data.Length - consumedLen > 0)
            {
                int toSendLen = (int)Math.Min(data.Length - consumedLen, dtlsStreamSizes.MaximumMessage);
                byte[] dataToSend = new byte[toSendLen];
                Array.Copy(data, consumedLen, dataToSend, 0, toSendLen);

                SecurityBuffer streamHd = new SecurityBuffer(SecurityBufferType.StreamHeader, new byte[dtlsStreamSizes.Header]);
                SecurityBuffer dataBuffer = new SecurityBuffer(SecurityBufferType.Data, dataToSend);
                SecurityBuffer streamTl = new SecurityBuffer(SecurityBufferType.StreamTrailer, new byte[dtlsStreamSizes.Trailer]);
                SecurityBuffer emptyBuffer = new SecurityBuffer(SecurityBufferType.Empty, null);
                if (dtlsServerContext != null)
                {
                    dtlsServerContext.Encrypt(streamHd, dataBuffer, streamTl, emptyBuffer);
                }
                else
                {
                    dtlsClientContext.Encrypt(streamHd, dataBuffer, streamTl, emptyBuffer);
                }

                byte[] dtlsEncrptedData = new byte[streamHd.Buffer.Length + dataBuffer.Buffer.Length + streamTl.Buffer.Length];
                Array.Copy(streamHd.Buffer, dtlsEncrptedData, streamHd.Buffer.Length);
                Array.Copy(dataBuffer.Buffer, 0, dtlsEncrptedData, streamHd.Buffer.Length, dataBuffer.Buffer.Length);
                Array.Copy(streamTl.Buffer, 0, dtlsEncrptedData, streamHd.Buffer.Length + dataBuffer.Buffer.Length, streamTl.Buffer.Length);

                encryptedDataList.Add(dtlsEncrptedData);
                consumedLen += toSendLen;
            }

            return encryptedDataList;
        }
        /// <summary>
        /// Decrypt data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            byte[] dtlsDataBuffer = new byte[data.Length];
            Array.Copy(data, dtlsDataBuffer, data.Length);
            SecurityBuffer dataBuffer = new SecurityBuffer(SecurityBufferType.Data, dtlsDataBuffer);
            SecurityBuffer emptyBuffer1 = new SecurityBuffer(SecurityBufferType.Empty, null);
            SecurityBuffer emptyBuffer2 = new SecurityBuffer(SecurityBufferType.Empty, null);
            SecurityBuffer emptyBuffer3 = new SecurityBuffer(SecurityBufferType.Empty, null);
            if (dtlsServerContext != null)
            {
                dtlsServerContext.Decrypt(dataBuffer, emptyBuffer1, emptyBuffer2, emptyBuffer3);
            }
            else
            {
                dtlsClientContext.Decrypt(dataBuffer, emptyBuffer1, emptyBuffer2, emptyBuffer3);
            }
            SecurityBuffer decryptedDataBuffer =
                (dataBuffer.BufferType == SecurityBufferType.Data) ? dataBuffer :
                (emptyBuffer1.BufferType == SecurityBufferType.Data) ? emptyBuffer1 :
                (emptyBuffer2.BufferType == SecurityBufferType.Data) ? emptyBuffer2 :
                (emptyBuffer3.BufferType == SecurityBufferType.Data) ? emptyBuffer3 : null;
            if (decryptedDataBuffer != null)
            {
                return decryptedDataBuffer.Buffer;
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// Get data to sent, the data is Write by SSL Stream
        /// The data is encrypted by SSL Stream
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public byte[] GetDataToSent(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;

            while (DateTime.Now < endtime)
            {
                if (toSendBuffer.Count > 0)
                {
                    lock (toSendBuffer)
                    {
                        if (toSendBuffer.Count > 0)
                        {
                            byte[] returnData = toSendBuffer[0];
                            toSendBuffer.RemoveAt(0);
                            return returnData;
                        }
                    }
                }
                Thread.Sleep(waitInterval);
            }

            return null;
        }

        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose()
        {
            if (rdpeudpSocket != null)
            {
                rdpeudpSocket.Received -= ReceiveBytes;

                rdpeudpSocket.Disconnected -= RdpeudpSocket_Disconnected;
            }
            if (dtlsServerContext != null)
            {
                dtlsServerContext.Dispose();
            }
            if (dtlsClientContext != null)
            {
                dtlsClientContext.Dispose();
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Add received data to this stream
        /// SSL Stream will read this data and decrypt it
        /// </summary>
        /// <param name="data"></param>
        private void AddReceivedData(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                byte[] receivedData = (byte[])data.Clone();

                lock (receivedBuffer)
                {
                    receivedBuffer.Add(receivedData);
                }
            }
        }

        /// <summary>
        /// Get Received Data from receiveBuffer
        /// This private method is used during authentication
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns></returns>
        private byte[] GetReceivedData(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;

            while (DateTime.Now < endtime)
            {
                if (receivedBuffer.Count > 0)
                {
                    lock (receivedBuffer)
                    {
                        if (receivedBuffer.Count > 0)
                        {
                            byte[] returnData = receivedBuffer[0];
                            receivedBuffer.RemoveAt(0);
                            return returnData;
                        }
                    }
                }
                Thread.Sleep(waitInterval);
            }

            return null;
        }

        /// <summary>
        /// Add data to toSendBuffer
        /// </summary>
        /// <param name="data"></param>
        private void AddDataToSent(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                byte[] toSendData = (byte[])data.Clone();

                lock (toSendBuffer)
                {
                    toSendBuffer.Add(toSendData);
                }
            }
        }

        /// <summary>
        /// Authenticate as server data.
        /// </summary>
        private class AuthenticateAsServerData
        {
            /// <summary>
            /// The certificate used by server.
            /// </summary>
            public X509Certificate Certificate { get; set; }

            /// <summary>
            /// The exception happened during authentication.
            /// </summary>
            public Exception Exception { get; set; }
        }

        /// <summary>
        /// Called by servers to authenticate the server and optionally the client in
        ///     a client-server connection using the specified certificate.
        /// </summary>
        /// <param name="data">The authenticate as server data.</param>
        private void AuthenticateAsServerTask(AuthenticateAsServerData data)
        {
            try
            {
                var cert = data.Certificate;

                dtlsServerContext = new DtlsServerSecurityContext(
                    SecurityPackageType.Schannel,
                    new CertificateCredential((X509Certificate)cert),
                    null,
                    ServerSecurityContextAttribute.ReplayDetect | ServerSecurityContextAttribute.SequenceDetect |
                     ServerSecurityContextAttribute.Confidentiality | ServerSecurityContextAttribute.ExtendedError |
                     ServerSecurityContextAttribute.Datagram,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

                // First accept.
                byte[] clientToken = this.GetReceivedData(this.timeout);
                dtlsServerContext.Accept(clientToken);
                this.SendData(dtlsServerContext.Token);

                while (dtlsServerContext.NeedContinueProcessing)
                {
                    if (dtlsServerContext.HasMoreFragments)
                    {
                        dtlsServerContext.Accept(null);
                    }
                    else
                    {
                        clientToken = this.GetReceivedData(this.timeout);
                        dtlsServerContext.Accept(clientToken);
                    }
                    if (dtlsServerContext.Token != null)
                    {
                        this.SendData(dtlsServerContext.Token);
                    }
                }


                isAuthenticated = true;

                dtlsStreamSizes = dtlsServerContext.StreamSizes;
            }
            catch (Exception ex)
            {
                data.Exception = ex;
            }
        }

        /// <summary>
        /// Called by clients to authenticate the server and optionally the client in
        /// a client-server connection.
        /// </summary>
        /// <param name="targetHost">The name of the server that share this connection.</param>
        public void AuthenticateAsClient(object targetHost)
        {
            if (targetHost is string)
            {
                dtlsClientContext = new DtlsClientSecurityContext(
                    SecurityPackageType.Schannel,
                    null,
                    (string)targetHost,
                    ClientSecurityContextAttribute.ReplayDetect | ClientSecurityContextAttribute.SequenceDetect |
                     ClientSecurityContextAttribute.Confidentiality | ClientSecurityContextAttribute.ExtendedError |
                     ClientSecurityContextAttribute.Datagram |
                     ClientSecurityContextAttribute.UseSuppliedCreds,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

                try
                {
                    // First Initialize.
                    byte[] serverToken = null;
                    dtlsClientContext.Initialize(serverToken);
                    this.SendData(dtlsClientContext.Token);

                    while (dtlsClientContext.NeedContinueProcessing)
                    {
                        if (dtlsClientContext.HasMoreFragments)
                        {
                            dtlsClientContext.Initialize(null);
                        }
                        else
                        {
                            serverToken = this.GetReceivedData(this.timeout);
                            dtlsClientContext.Initialize(serverToken);
                        }
                        if (dtlsClientContext.Token != null)
                        {
                            this.SendData(dtlsClientContext.Token);
                        }
                    }

                    isAuthenticated = true;

                    dtlsStreamSizes = dtlsClientContext.StreamSizes;
                }
                catch
                {
                    // Don't throw exception in ThreadPool thread
                }

            }
        }

        /// <summary>
        /// Method used during authentication.
        /// If RDPEUDP Socket is autohandle, send data throw RDPEUDP Socket
        /// If RDPEUDP Socket is not autohandle, save data to dataToSent buffer
        /// </summary>
        /// <param name="data"></param>
        private void SendData(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                if (rdpeudpSocket.AutoHandle)
                {
                    rdpeudpSocket.Send(data);
                }
                else
                {
                    this.AddDataToSent(data);
                }
            }
        }

        private void RdpeudpSocket_Disconnected()
        {
            Disconnected?.Invoke();
        }
        #endregion Private Methods
    }
}
