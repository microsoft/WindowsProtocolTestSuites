// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security
{
    /// <summary>
    /// Performs enhanced RDP security transport. The External Security Protocol is CredSSP. This class is NOT
    /// guaranteed to be thread safe.
    /// </summary>
    internal class CredSspStream : Stream
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
        private const int BlockSize = 102400;

        byte[] recvBuffer = new byte[BlockSize];

        /// <summary>
        /// Underlying network stream
        /// </summary>
        private Stream clientStream;

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
        private AccountCredential credential;

        /// <summary>
        /// Client context for authentication
        /// </summary>
        private SspiClientSecurityContext context;

        /// <summary>
        /// Context attribute used for CredSsp
        /// </summary>
        private const ClientSecurityContextAttribute attribute =
            ClientSecurityContextAttribute.Delegate
            | ClientSecurityContextAttribute.ReplayDetect
            | ClientSecurityContextAttribute.SequenceDetect
            | ClientSecurityContextAttribute.Confidentiality
            | ClientSecurityContextAttribute.AllocMemory
            | ClientSecurityContextAttribute.ExtendedError
            | ClientSecurityContextAttribute.Stream;

        /// <summary>
        ///  Domain name used for authentication, if the authentication doesn't contain a domain, use ""
        /// </summary>
        private string domain;

        /// <summary>
        /// User name used for authentication, if the authentication doesn't require it, use ""
        /// </summary>
        private string userName;

        /// <summary>
        /// Password used for authentication, if the authentication doesn't require it, use ""
        /// </summary>
        private string password;

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
        private void CloseClientStream()
        {
            if (this.clientStream != null)
            {
                clientStream.Close();
                clientStream = null;
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
                return clientStream.CanRead;
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
                return clientStream.CanSeek;
            }
        }

        /// <summary>
        /// Indicates whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return clientStream.CanWrite;
            }
        }

        /// <summary>
        /// Length of bytes in stream.
        /// </summary>
        public override long Length
        {
            get
            {
                return clientStream.Length;
            }
        }

        /// <summary>
        /// The current position within the stream.
        /// </summary>
        public override long Position
        {
            get
            {
                return clientStream.Position;
            }
            set
            {
                clientStream.Position = value;
            }
        }
        #endregion Overriden properties

        #region Public methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The underlying network stream</param>
        /// <param name="domain">Domain name</param>
        /// <param name="principal">Server principal to be logged on, prefixed with "TERMSRV/"</param>
        /// <param name="userName">Username used for authentication, doesn't contain the domain prefix</param>
        /// <param name="password">Password used for authentication</param>
        public CredSspStream(Stream client, string domain, string principal, string userName, string password)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            this.clientStream = client;
            this.domain = domain;
            this.serverPrincipal = principal;
            this.userName = userName;
            this.password = password;

            this.decryptedBuffer = new byte[MaxBufferSize];
            this.pooledBuffer = null;
            this.startIndex = 0;
            this.endIndex = 0;
            this.isAuthenticated = false;
        }


        /// <summary>
        /// Performs CredSSP authentication.
        /// </summary>
        /// <exception cref="IOException">Raised when attempting to read from/write to the remote connection which
        /// has been closed</exception>
        /// <exception cref="EndOfStreamException">Raised when the username or password doesn't match or authentication
        /// fails</exception>
        public void Authenticate()
        {
            // Authenticated already, do nothing
            if (isAuthenticated)
            {
                return;
            }

            credential = new AccountCredential(domain, userName, password);

            byte[] receivedBuffer = new byte[MaxBufferSize];
            int bytesReceived = 0;

            // Dispose the context as it may be timed out
            if (context != null)
            {
                context.Dispose();
            }
            context = new SspiClientSecurityContext(
                SecurityPackageType.CredSsp,
                credential,
                serverPrincipal,
                attribute,
                SecurityTargetDataRepresentation.SecurityNativeDrep);

            context.Initialize(null);
            // Get first token
            byte[] token = context.Token;
            // SSL handshake
            while (context.NeedContinueProcessing)
            {
                // Send handshake request
                clientStream.Write(token, 0, token.Length);
                // Get handshake resopnse
                bytesReceived = clientStream.Read(receivedBuffer, 0, receivedBuffer.Length);
                // The remote connection has been closed
                if (bytesReceived == 0)
                {
                    throw new EndOfStreamException("Authentication failed: remote connection has been closed.");
                }

                byte[] inToken = new byte[bytesReceived];
                Array.Copy(receivedBuffer, inToken, bytesReceived);
                // Get next token from response
                context.Initialize(inToken);
                token = context.Token;
            }
            // Send the last token, handshake over, CredSSP is established
            // Note if there're errors during authentication, an SSPIException will be raised
            // and isAuthentication will not be true.
            clientStream.Write(token, 0, token.Length);
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

            byte[] encryptedMsg = null;
            int bytesReceived = 0;
            byte[] decryptedMsg = null;

            while (decryptedMsg == null)
            {
                // decryptedMsg being null indicates incomplete data, so we continue reading and decrypting.
                bytesReceived = ReceivePacket();

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

        private struct TLSHeader
        {
            public byte type;
            public byte version_major;
            public byte version_minor;
            public byte length_hi;
            public byte length_lo;
        }

        private int ReceivePacket()
        {
            // Receive TLS header first.
            var header = new TLSHeader();

            var headerBuffer = TypeMarshal.ToBytes(header);

            int headerLength = TypeMarshal.ToBytes(header).Length;

            int reveivedLength = 0;

            while (reveivedLength < headerLength)
            {
                int lenth = clientStream.Read(headerBuffer, reveivedLength, headerLength - reveivedLength);

                if (lenth == 0)
                {
                    return 0;
                }

                reveivedLength += lenth;
            }

            Array.Copy(headerBuffer, 0, recvBuffer, 0, headerLength);

            // Calculate the body Length.
            header = TypeMarshal.ToStruct<TLSHeader>(headerBuffer);

            int bodyLength = (header.length_hi << 8) + header.length_lo;

            // Receive body.
            int bodyReceivedLength = 0;

            while (bodyReceivedLength < bodyLength)
            {
                int lenth = clientStream.Read(recvBuffer, headerLength + bodyReceivedLength, bodyLength - bodyReceivedLength);

                if (lenth == 0)
                {
                    return 0;
                }

                bodyReceivedLength += lenth;
            }

            int totalLength = headerLength + bodyLength;

            return totalLength;
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

            byte[] outBuffer = new byte[count];
            Array.Copy(buffer, offset, outBuffer, 0, count);

            // Encrypt message
            SecurityPackageContextStreamSizes streamSizes =
                (SecurityPackageContextStreamSizes)context.QueryContextAttributes("SECPKG_ATTR_STREAM_SIZES");
            SecurityBuffer messageBuffer = new SecurityBuffer(SecurityBufferType.Data, buffer);
            SecurityBuffer headerBuffer = new SecurityBuffer(
                SecurityBufferType.StreamHeader,
                new byte[streamSizes.Header]);
            SecurityBuffer trailerBuffer = new SecurityBuffer(
                SecurityBufferType.StreamTrailer,
                new byte[streamSizes.Trailer]);
            SecurityBuffer emptyBuffer = new SecurityBuffer(SecurityBufferType.Empty, null);

            context.Encrypt(headerBuffer, messageBuffer, trailerBuffer, emptyBuffer);
            byte[] encryptedMsg = ArrayUtility.ConcatenateArrays(
                headerBuffer.Buffer,
                messageBuffer.Buffer,
                trailerBuffer.Buffer);
            clientStream.Write(encryptedMsg, 0, encryptedMsg.Length);
        }


        /// <summary>
        /// clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <returns></returns>
        public override void Flush()
        {
            clientStream.Flush();
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
                    CloseClientStream();

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
            CloseClientStream();
        }


        /// <summary>
        /// Finalizer, cleans up unmanaged resources
        /// </summary>
        ~CredSspStream()
        {
            this.Dispose(false);
        }
        #endregion Implements dispose pattern
    }
}
