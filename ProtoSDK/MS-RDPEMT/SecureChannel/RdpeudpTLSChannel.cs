// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Microsoft.Protocols.TestTools.ExtendedLogging;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    /// <summary>
    /// A simulated inner stream which is used to create SSLStream.
    /// Using this simulated stream can communicate with .NET SSLStream interface:
    /// Get encrypted data for sent; or get un-decrypted data received 
    /// </summary>
    internal class SSLInnerStream : Stream
    {
        
        /// <summary>
        /// milliseconds for Read method sleep time when wait for data
        /// </summary>
        const int waitInterval = 50;

        /// <summary>
        /// buffer used to save data not be used in last Read
        /// If the count parameter is smaller than the length of this packet
        /// </summary>
        byte[] remainData; 

        /// <summary>
        /// buffer used for received data, which is received from RDPEUDP transport.
        /// Wait to be Read by SSL stream.
        /// </summary> 
        private List<byte[]> receivedBuffer;

        /// <summary>
        /// buffer used for data to be sent, which will be sent to RDPEUDP transport
        /// the data is provided by Write method called by SSL stream
        /// </summary>
        private List<byte[]> toSendBuffer;
        private long length;
        private long position;

        public SSLInnerStream()
        {
            receivedBuffer = new List<byte[]>();
            toSendBuffer = new List<byte[]>();
            length = 0;
            position = 0;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            return;
        }

        public override long Length
        {
            get { return length; }
        }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Read bytes from stream
        /// </summary>
        /// <param name="buffer">Buffer used to copy read bytes</param>
        /// <param name="offset">offset of the Buffer</param>
        /// <param name="count">bytes count</param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {

            int readLen = 0;
            
            if (remainData != null)
            {
                // If had remain data from last received packet, transfer these data directly
                readLen = Math.Min(count, remainData.Length);
                Array.Copy(remainData, 0, buffer, offset, readLen);
                // Update remainData
                if (readLen < remainData.Length)
                {
                    byte[] remain = new byte[remainData.Length - readLen];
                    Array.Copy(remainData, readLen, remain, 0, remainData.Length - readLen);
                    remainData = remain;
                }
                else
                {
                    remainData = null;
                }
                return readLen;
            }

            byte[] receivedData = null;

            
            while (receivedData == null)
            {
                if (receivedBuffer.Count > 0)
                {
                    lock (receivedBuffer)
                    {
                        if (receivedBuffer.Count > 0)
                        {
                            receivedData = receivedBuffer[0];
                            receivedBuffer.RemoveAt(0);
                        }
                    }

                }
                if (receivedData == null)
                {
                    Thread.Sleep(waitInterval);
                }
            }


            readLen = Math.Min(count, receivedData.Length);
            Array.Copy(receivedData, 0, buffer, offset, readLen);
            // If still have data not read, save it to remainData
            if (readLen < receivedData.Length)
            {
                remainData = new byte[receivedData.Length - readLen];
                Array.Copy(receivedData, readLen, remainData, 0, receivedData.Length - readLen);
            }            

            return (int)readLen;

        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                position = offset;
            }
            else
            {
                position += offset;
            }

            if (position < 0 || position >= length)
            {
                throw new IOException("The cursor index is out of boundary");
            }
            return position;
        }

        public override void SetLength(long value)
        {
            length = value;
        }

        /// <summary>
        /// Write bytes into stream
        /// </summary>
        /// <param name="buffer">buffer of bytes to write</param>
        /// <param name="offset">offset of the buffer</param>
        /// <param name="count">count of bytes</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] dgram = new byte[count];

            Array.Copy(buffer, offset, dgram, 0, count);

            lock (toSendBuffer)
            {
                toSendBuffer.Add(dgram);
            }

        }

        #region Methods for transfer data with this stream
        
        /// <summary>
        /// Add received data to this stream
        /// SSL Stream will read this data and decrypt it
        /// </summary>
        /// <param name="data"></param>
        public void AddReceivedData(byte[] data)
        {
            if(data != null && data.Length >0)
            {
                byte[] receivedData = (byte[])data.Clone();
           
                lock (receivedBuffer)
                {
                    receivedBuffer.Add(receivedData);
                }
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
        #endregion
    }

    /// <summary>
    /// Security channel for TLS transport, which is used for RDPEUDP reliable transport
    /// </summary>
    public class RdpeudpTLSChannel : ISecureChannel
    {
        #region Private Variables

        /// <summary>
        /// Max size of read buffer
        /// </summary>
        private const int readBufferMaxLen = 1300;

        private TimeSpan timeout = new TimeSpan(0, 0, 20);

        /// <summary>
        /// Short wait time
        /// </summary>
        private TimeSpan shortWaitTime = new TimeSpan(0, 0, 0, 0, 100);

        /// <summary>
        /// A RdpeudpSocket which is the transport of this security channel
        /// </summary>
        private RdpeudpSocket rdpeudpSocket;

        /// <summary>
        /// Simulated stream used as inner stream of the SSL Stream
        /// </summary>
        private SSLInnerStream innerStream;

        /// <summary>
        /// SSL Stream created from innerStream
        /// </summary>
        private SslStream sslStream;

        private bool isAuthenticated;

        private byte[] readBuffer = new byte[readBufferMaxLen];

        private RemoteCertificateValidationCallback remoteCertValCallback;

        #endregion Private Variables

        #region Properties

        /// <summary>
        /// Whether the SSL stream completed authentication (TLS handshake)
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
        /// <param name="eudpSocket">A RdpeudpSocket used for transport</param>
        public RdpeudpTLSChannel(RdpeudpSocket eudpSocket)
        {
            if (eudpSocket == null || eudpSocket.TransMode == TransportMode.Lossy)
            {
                throw new NotSupportedException("RdpeudpSocket is null or it is a socket for Lossy RDPEUDP connection.");
            }
            this.rdpeudpSocket = eudpSocket;
            this.rdpeudpSocket.Received += ReceiveBytes;
            innerStream = new SSLInnerStream();
            isAuthenticated = false;

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

        #region Implementation of ISecurityChannel Interface

        /// <summary>
        /// Send bytes through this security channel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        public void Send(byte[] data)
        {
            if (!isAuthenticated)
            {
                throw new NotSupportedException("Cannot send bytes from SSL Stream before it is authenticated.");
            }

            if (data != null && data.Length > 0)
            {
                byte[] dataToSent = Encrypt(data);
                this.rdpeudpSocket.Send(dataToSent);

                // ETW Provider Dump Message
                string messageName = "RDPEMT:SentPDU";
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer2, "RDPEMT Sent PDU", data);
            }            
        }

        /// <summary>
        /// Event be called when received data
        /// </summary>
        public event ReceiveData Received;

        #endregion Implementation of ISecurityChannel Interface

        #region Authenticate methods
        /// <summary>
        /// Called by servers to authenticate the server and optionally the client in
        /// a client-server connection using the specified certificate.
        /// </summary>
        /// <param name="cert">The certificate used to authenticate the server.</param>
        public void AuthenticateAsServer(X509Certificate cert)
        {
            // Using thread in threadpool to manage the authentication process
            ThreadPool.QueueUserWorkItem(AuthenticateAsServer, cert);

            DateTime endtime = DateTime.Now + timeout;
            if (rdpeudpSocket.AutoHandle)
            {
                // If the transport is Autohandle, send packets during authentication automatically
                TimeSpan waittime = new TimeSpan(0, 0, 1);
                while (!isAuthenticated && DateTime.Now < endtime)
                {
                    byte[] bytesToSend = innerStream.GetDataToSent(waittime);
                    if (bytesToSend != null && bytesToSend.Length > 0)
                    {
                        rdpeudpSocket.Send(bytesToSend);
                    }
                }

                if (!isAuthenticated)
                {
                    throw new TimeoutException("Time out when Authenticate as Server!");
                }
            }
            else
            {
                // If the transport is not Autohandle, return, not send packet automatically
            }
        }

        /// <summary>
        /// Called by clients to authenticate the server and optionally the client in
        /// a client-server connection.
        /// </summary>
        /// <param name="targetHost">The name of the server that shares this System.Net.Security.SslStream.</param>
        /// <param name="certValCallback">A System.Net.Security.RemoteCertificateValidationCallback delegate responsible for validating the certificate supplied by the remote party.</param>
        public void AuthenticateAsClient(string targetHost, RemoteCertificateValidationCallback certValCallback = null)
        {
            if (certValCallback == null)
            {
                certValCallback = new RemoteCertificateValidationCallback(CertificateValidationCallback);
            }
            remoteCertValCallback = certValCallback;
            // Using thread in threadpool to manage the authentication process
            ThreadPool.QueueUserWorkItem(AuthenticateAsClient, targetHost);
            DateTime endtime = DateTime.Now + timeout;
            if (rdpeudpSocket.AutoHandle)
            {
                // If the transport is Autohandle, send packets during authentication automatically
                TimeSpan waittime = new TimeSpan(0, 0, 1);
                while (!isAuthenticated && DateTime.Now < endtime)
                {
                    byte[] bytesToSend = innerStream.GetDataToSent(waittime);
                    if (bytesToSend != null && bytesToSend.Length > 0)
                    {
                        rdpeudpSocket.Send(bytesToSend);
                    }
                }

                if (!isAuthenticated)
                {
                    throw new TimeoutException("Time out when Authenticate as Client!");
                }
            }
            else
            {
                // If the transport is not Autohandle, return, not send packet automatically
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
                innerStream.AddReceivedData(data);
                if (isAuthenticated)
                {
                    int actualLen = this.sslStream.Read(readBuffer, 0, readBuffer.Length);
                    byte[] decryptedData = new byte[actualLen];
                    Array.Copy(readBuffer, 0, decryptedData, 0, actualLen);

                    // ETW Provider Dump Message
                    string messageName = "RDPEMT:ReceivedPDU";
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer2, "RDPEMT Received PDU", decryptedData);

                    if (Received != null)
                    {
                        Received(decryptedData);
                    }
                }
                
            }
        }

        /// <summary>
        /// Encrypt data using SSL Stream
        /// </summary>
        /// <param name="data">Data need to encrypted</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            if (!isAuthenticated)
            {
                return null;
            }

            sslStream.Write(data);
            byte[] encryptedData = innerStream.GetDataToSent(timeout);
            return encryptedData;
        }

        /// <summary>
        /// Get data to sent, this method is used to get to-sent data when doing authentication
        /// </summary>
        /// <returns></returns>
        public byte[] GetDataToSent(TimeSpan timeout)
        {
            if (isAuthenticated)
            {
                // If the SSL stream have been authenticated, don't used this method to get to-sent data
                // Instand, using Encrypt method to encrypt original data, the encrypted data should be used to sent
                return null;
            }

            byte[] data = innerStream.GetDataToSent(timeout);
            return data;
        }
 
        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose()
        {
            if (rdpeudpSocket != null)
            {
                rdpeudpSocket.Received -= ReceiveBytes;
            }
            if (sslStream != null)
            {
                sslStream.Dispose();
            }
        }

        // Callback for certificate validation.
        static bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Call by the server to finish TLS handshake
        /// </summary>
        /// <param name="cert">The certificate used to authenticate the server.</param>
        private void AuthenticateAsServer(object cert)
        {
            if (cert is X509Certificate)
            {
                try
                {
                    sslStream = new SslStream(innerStream);
                    sslStream.AuthenticateAsServer(cert as X509Certificate);
                    isAuthenticated = sslStream.IsAuthenticated;
                }
                catch
                {
                    // Do not throw exception in not-main thread
                }
            }
        }

        /// <summary>
        /// Call by Client to finish TLS handshake
        /// </summary>
        /// <param name="targetHost">The name of the server that shares this System.Net.Security.SslStream.</param>
        private void AuthenticateAsClient(object targetHost)
        {
            if (targetHost is string)
            {
                try
                {
                    sslStream = new SslStream(innerStream, false, remoteCertValCallback);
                    sslStream.AuthenticateAsClient(targetHost as string, null, System.Security.Authentication.SslProtocols.Tls, false);
                    isAuthenticated = true;
                }
                catch
                {
                    // Do not throw exception in not-main thread
                }
            }
        }
        #endregion Private Methods
    }
}
