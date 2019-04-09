// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Performs enhanced RDP security transport. The External Security Protocol is CredSSP.
    /// </summary>
    internal class RdpbcgrServerCredSspStream : Stream
    {
        #region Member variables
        /// <summary>
        /// Indicates whether the instance is disposed
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Maximum buffer size
        /// </summary>
        private const int MaxBufferSize = 102400;

        /// <summary>
        /// A read block size 
        /// </summary>
        //private const int BlockSize = 1500;
        private const int BlockSize = 5500;

        /// <summary>
        /// Underlying network stream
        /// </summary>
        private Stream serverStream;

        /// <summary>
        /// The buffer that contains decrypted data 
        /// </summary>
        private byte[] decryptedBuffer;

        /// <summary>
        /// The buffer pooled for decryption. There may be extra data left when decrypting a 
        /// sequence of bytes, the extra data is then pooled in this buffer for next decryption
        /// </summary>
        private byte[] pooledBuffer;

        /// <summary>
        /// Start offset that contains decrypted data in buffer 
        /// </summary>
        private int startIndex;

        /// <summary>
        /// End offset that contains decrypted data in buffer
        /// </summary>
        private int endIndex;

        /// <summary>
        /// Indicates whether CredSSP has been established 
        /// </summary>
        private bool isAuthenticated;

        /// <summary>
        /// Client credential for authentication
        /// </summary>
        private CertificateCredential credential;

        /// <summary>
        /// Client context for authentication
        /// </summary>
        private SspiServerSecurityContext context;

        /// <summary>
        /// Context attribute used for CredSsp
        /// </summary>
        private const ServerSecurityContextAttribute attribute =
            ServerSecurityContextAttribute.Delegate
            | ServerSecurityContextAttribute.ReplayDetect
            | ServerSecurityContextAttribute.SequenceDetect
            | ServerSecurityContextAttribute.Confidentiality
            | ServerSecurityContextAttribute.AllocMemory
            | ServerSecurityContextAttribute.ExtendedError
            | ServerSecurityContextAttribute.Stream;

        /// <summary>
        /// Server principal to be logged on
        /// </summary>
        private string serverPrincipal;
        #endregion Member variables

        #region Helper methods
        /// <summary>
        /// Checks if the decrypted buffer is empty
        /// </summary>
        /// <returns></returns>
        private bool IsBufferEmpty()
        {
            return (this.startIndex == this.endIndex);
        }


        /// <summary>
        /// Checks if there's enough buffer for reading
        /// </summary>
        /// <param name="count">The size to read</param>
        /// <returns>True if there's enough buffer for reading, false otherwise</returns>
        private bool CheckAvailableCount(int count)
        {
            return (this.startIndex + count < this.endIndex);
        }


        /// <summary>
        /// Closes the underlying network stream
        /// </summary>
        private void CloseServerStream()
        {
            if (this.serverStream != null)
            {
                serverStream.Close();
                serverStream = null;
            }
        }
        #endregion Helper methods

        #region Overriden properties
        /// <summary>
        /// Indicates whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return serverStream.CanRead;
            }
        }


        /// <summary>
        /// Indicates whether the current stream supports seeking.
        /// CredSspStream does not support seeking.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return serverStream.CanSeek;
            }
        }


        /// <summary>
        /// Indicates whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return serverStream.CanWrite;
            }
        }


        /// <summary>
        /// Length of bytes in stream.
        /// </summary>
        public override long Length
        {
            get
            {
                return serverStream.Length;
            }
        }


        /// <summary>
        /// The current position within the stream.
        /// </summary>
        public override long Position
        {
            get
            {
                return serverStream.Position;
            }
            set
            {
                serverStream.Position = value;
            }
        }
        #endregion Overriden properties

        #region Public methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">The underlying network stream</param>
        /// <param name="principal">Server principal to be logged on, prefixed with "TERMSRV/"</param>
        public RdpbcgrServerCredSspStream(Stream server, string principal)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            this.serverStream = server;
            this.serverPrincipal = principal;

            this.decryptedBuffer = new byte[MaxBufferSize];
            this.pooledBuffer = null;
            this.startIndex = 0;
            this.endIndex = 0;
            this.isAuthenticated = false;
        }


        /// <summary>
        /// Performs CredSSP authentication.
        /// </summary>
        /// <param name="x509Cert">The certificate used by TLS.</param>
        /// <exception cref="IOException">Raised when attempting to read from/write to the remote connection which
        /// has been closed</exception>
        /// <exception cref="EndOfStreamException">Raised when the username or password doesn't match or authentication
        /// fails</exception>
        public void Authenticate(X509Certificate x509Cert)
        {
            // Authenticated already, do nothing
            if (isAuthenticated)
            {
                return;
            }

            credential = new CertificateCredential(x509Cert);

            byte[] receivedBuffer = new byte[MaxBufferSize];
            int bytesReceived = 0;

            // Dispose the context as it may be timed out
            if (context != null)
            {
                context.Dispose();
            }

            context = new SspiServerSecurityContext(
                SecurityPackageType.CredSsp,
                credential,
                serverPrincipal,
                attribute,
                SecurityTargetDataRepresentation.SecurityNativeDrep);

            // Get first token
            byte[] token = context.Token;
            // Credssp handshake
            while (context.NeedContinueProcessing)
            {
                // Get handshake resopnse
                bytesReceived = serverStream.Read(receivedBuffer, 0, receivedBuffer.Length);
                // The remote connection has been closed
                if (bytesReceived == 0)
                {
                    throw new EndOfStreamException("Authentication failed: remote connection has been closed.");
                }

                byte[] inToken = new byte[bytesReceived];
                Array.Copy(receivedBuffer, inToken, bytesReceived);
                // Get next token from response
                context.Accept(inToken);
                token = context.Token;

                if (token != null)
                {
                    // Send handshake request
                    serverStream.Write(token, 0, token.Length);
                } 
            }

            isAuthenticated = true;
        }


        /// <summary>
        /// Reads a sequence of bytes from the current stream.
        /// </summary>
        /// <param name="buffer">The buffer that contains decrypted data.</param>
        /// <param name="offset">The offset in buffer at which to begin storing the decrypted data.</param>
        /// <param name="count">The maximum number of bytes to get.</param>
        /// <exception cref="ArgumentOutOfRangeException">Raised when buffer or the internal decryptedBuffer doesn't
        /// contain enough space
        /// </exception>
        /// <exception cref="IOException">Raised when attempting to read from/write to a remote connection which 
        /// has been closed</exception>
        /// <returns>The actual number of bytes read into the buffer. Could be less than count</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset > buffer.Length - 1)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (!IsBufferEmpty())
            {
                if (CheckAvailableCount(count))
                {
                    Array.Copy(decryptedBuffer, this.startIndex, buffer, offset, count);
                    this.startIndex += count;

                    return count;
                }
                else
                {
                    int sizeRead = this.endIndex - this.startIndex;
                    Array.Copy(decryptedBuffer, this.startIndex, buffer, offset, sizeRead);
                    // All data is read, reset indices
                    this.startIndex = 0;
                    this.endIndex = 0;

                    return sizeRead;
                }
            }

            // The buffer is empty, read data from network stream
            byte[] recvBuffer = new byte[BlockSize];
            byte[] encryptedMsg = null;
            int bytesReceived = 0;
            byte[] decryptedMsg = null;
            bool triedPooledBuffer = false;

            while (decryptedMsg == null)
            {
                if (!triedPooledBuffer && this.pooledBuffer != null && this.pooledBuffer.Length > 0)
                {// try to decrypte the data in this.pooledBuffer firstly.
                    encryptedMsg = new byte[this.pooledBuffer.Length];
                    Array.Copy(this.pooledBuffer, encryptedMsg, encryptedMsg.Length);
                    this.pooledBuffer = null;
                    triedPooledBuffer = true;
                }
                else
                {
                    // decryptedMsg being null indicates incomplete data, so we continue reading and decrypting.
                    bytesReceived = serverStream.Read(recvBuffer, 0, recvBuffer.Length);

                    // The connection has been closed by remote server
                    if (bytesReceived == 0)
                    {
                        return 0;
                    }

                    // There's pooled data, concatenate the buffer together for decryption
                    if (this.pooledBuffer != null && this.pooledBuffer.Length > 0)
                    {
                        encryptedMsg = new byte[this.pooledBuffer.Length + bytesReceived];
                        Array.Copy(this.pooledBuffer, encryptedMsg, this.pooledBuffer.Length);
                        Array.Copy(recvBuffer, 0, encryptedMsg, this.pooledBuffer.Length, bytesReceived);

                        this.pooledBuffer = null;
                    }
                    else
                    {
                        encryptedMsg = new byte[bytesReceived];
                        Array.Copy(recvBuffer, encryptedMsg, bytesReceived);
                    }
                }

                byte[] extraData = null;
                // Do decryption
                SecurityBuffer[] securityBuffers = new SecurityBuffer[] 
                {
                    new SecurityBuffer(SecurityBufferType.Data, encryptedMsg),
                    new SecurityBuffer(SecurityBufferType.Empty, null),
                    new SecurityBuffer(SecurityBufferType.Empty, null),
                    new SecurityBuffer(SecurityBufferType.Empty, null)
                };

                context.Decrypt(securityBuffers);
                for (int i = 0; i < securityBuffers.Length; i++)
                {
                    if (securityBuffers[i].BufferType == SecurityBufferType.Data)
                    {
                        decryptedMsg = ArrayUtility.ConcatenateArrays(decryptedMsg, securityBuffers[i].Buffer);
                    }
                    else if (securityBuffers[i].BufferType == SecurityBufferType.Extra)
                    {
                        extraData = ArrayUtility.ConcatenateArrays(extraData, securityBuffers[i].Buffer);
                    }
                }

                if (extraData != null && extraData.Length > 0)
                {
                    this.pooledBuffer = extraData;
                }
            }

            Array.Copy(decryptedMsg, 0, this.decryptedBuffer, this.endIndex, decryptedMsg.Length);
            this.endIndex += decryptedMsg.Length;

            return Read(buffer, offset, count);
        }


        /// <summary>
        /// Writes a sequence of bytes to the current stream. The data is encrypted first before sending out.
        /// </summary>
        /// <param name="buffer">The buffer to be sent.</param>
        /// <param name="offset">The offset in buffer at which to begin writing to the stream.</param>
        /// <param name="count">The number of bytes to be written.</param>
        /// <exception cref="IOException">Raised when attempting to read from/write to a remote connection which 
        /// has been closed</exception>
        /// <exception cref="ArgumentOutOfRangeException">Raised when the offset incremented by count exceeds
        /// the length of buffer</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (offset > buffer.Length - 1)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            // Get stream attribute
            SecurityPackageContextStreamSizes streamSizes =
                (SecurityPackageContextStreamSizes)context.QueryContextAttributes("SECPKG_ATTR_STREAM_SIZES");

            int chunckSize = (int)streamSizes.MaximumMessage;
            List<byte> byteList = new List<byte>();

            while (count > 0)
            {
                int bufferSize = count;
                if(bufferSize > chunckSize)
                {
                    bufferSize = chunckSize;
                }

                byte[] outBuffer = new byte[bufferSize];
                Array.Copy(buffer, offset, outBuffer, 0, bufferSize);
                count -= bufferSize;
                offset += bufferSize;

                // Encrypt Chunck
                SecurityBuffer messageBuffer = new SecurityBuffer(SecurityBufferType.Data, outBuffer);
                SecurityBuffer headerBuffer = new SecurityBuffer(
                    SecurityBufferType.StreamHeader,
                    new byte[streamSizes.Header]);
                SecurityBuffer trailerBuffer = new SecurityBuffer(
                    SecurityBufferType.StreamTrailer,
                    new byte[streamSizes.Trailer]);
                SecurityBuffer emptyBuffer = new SecurityBuffer(SecurityBufferType.Empty, null);

                context.Encrypt(headerBuffer, messageBuffer, trailerBuffer, emptyBuffer);
                byte[] encryptedChunck = ArrayUtility.ConcatenateArrays(
                    headerBuffer.Buffer,
                    messageBuffer.Buffer,
                    trailerBuffer.Buffer);
                byteList.AddRange(encryptedChunck);
            }

            byte[] encryptedMsg = byteList.ToArray();
            serverStream.Write(encryptedMsg, 0, encryptedMsg.Length);
        }


        /// <summary>
        /// clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <returns></returns>
        public override void Flush()
        {
            serverStream.Flush();
        }


        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type System.IO.SeekOrigin indicating the reference point used
        /// to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception>NotSupportedException</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("This is not supported for CredSspStream.");
        }

         
        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception>NotSupportedException</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("This is not supported for CredSspStream.");
        }
        #endregion Public methods

        #region Implements dispose pattern
        /// <summary>
        /// Implements IDisposable interface(from NetworkStream)
        /// </summary>
        /// <param name="disposing">Indicates if there's managed resource to release</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Clean managed resource
                    CloseServerStream();

                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                }
                this.disposed = true;
                this.isAuthenticated = false;
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Overrides Close method of NetworkStream
        /// </summary>
        public override void Close()
        {
            CloseServerStream();
        }


        /// <summary>
        /// Finalizer, cleans up unmanaged resources
        /// </summary>
        ~RdpbcgrServerCredSspStream()
        {
            this.Dispose(false);
        }
        #endregion Implements dispose pattern
    }
}
