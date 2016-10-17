// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    
    /// <summary>
    /// The PCHC client class which is used to exchange the pchc message between the hosted cache server.
    /// </summary>
    public class PCHCClient : IDisposable
    {
        #region field

        /// <summary>
        /// The RESPONSE_MESSAGE response for the SEGMENT_INFO_MESSAG request or INITIAL_OFFER_MESSAGE request.
        /// </summary>
        private RESPONSE_MESSAGE responseMessage;

        /// <summary>
        /// The HttpClientTransport instance: transport is used to as the http client side.
        /// </summary>
        private HttpClientTransport httpClientTransport;

        /// <summary>
        /// The http method is used in the response from the hosted cache server.
        /// </summary>
        private string httpResponseMethod;

        /// <summary>
        /// The http uri is used in the response from the hosted cache server.
        /// </summary>
        private Uri httpResponseUri;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogPrinter logger;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Initializes a new instance of the PCHCClient class 
        /// With the specified http transport type, server name, linsten port number and resource.
        /// </summary>
        /// <param name="httpTransportType">Http transport type.</param>
        /// <param name="serverName">The server name.</param>
        /// <param name="port">The listening port.</param>
        /// <param name="resource">The resource.</param>
        public PCHCClient(TransferProtocol httpTransportType, string serverName, int port, string resource)
            : this(httpTransportType, serverName, port, resource, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PCHCClient class 
        /// With the specified http transport type, server name, linsten port number and resource.
        /// </summary>
        /// <param name="httpTransportType">Http transport type.</param>
        /// <param name="serverName">The server name.</param>
        /// <param name="port">The listening port.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="domainName">The domain name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="userPassword">The password.</param>
        public PCHCClient(TransferProtocol httpTransportType, string serverName, int port, string resource, string domainName, string userName, string userPassword)
        {
            if (httpTransportType != TransferProtocol.HTTP && httpTransportType != TransferProtocol.HTTPS)
            {
                throw new ArgumentException(
                    "httpTransportType contains invalid not supported transport type", "httpTransportType");
            }

            this.httpClientTransport = new HttpClientTransport(httpTransportType, serverName, port, resource, domainName, userName, userPassword);
        }

        /// <summary>
        /// Initializes a new instance of the PCHCClient class 
        /// With the specified http transport type, server name, linsten port number and resource.
        /// </summary>
        /// <param name="httpTransportType">Http transport type.</param>
        /// <param name="serverName">The server name.</param>
        /// <param name="port">The listening port.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="logger">The logger.</param>
        public PCHCClient(TransferProtocol httpTransportType, string serverName, int port, string resource, ILogPrinter logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger", "The input parameter \"logger\" is null.");
            }

            this.logger = logger;

            if (httpTransportType != TransferProtocol.HTTP && httpTransportType != TransferProtocol.HTTPS)
            {
                throw new ArgumentException(
                    "httpTransportType contains invalid not supported transport type", "httpTransportType");
            }

            this.httpClientTransport = new HttpClientTransport(httpTransportType, serverName, port, resource, logger);
        }

        /// <summary>
        /// Finalizes an instance of the PCHCClient class.
        /// This destructor will run only if the Dispose method 
        /// does not get called.
        /// It gives your base class the opportunity to finalize.
        /// Do not provide destructors in types derived from this class.
        /// </summary>
        ~PCHCClient()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region property

        /// <summary>
        /// Gets the http method which is used in the response from the hosted cache server.
        /// </summary>
        public string HTTPMethod
        {
            get { return this.httpResponseMethod; }
        }

        /// <summary>
        /// Gets the http uri which is used in the response from the hosted cache server.
        /// </summary>
        public Uri HttpResponseUri
        {
            get { return this.httpResponseUri; }
        }

        #endregion

        #region public method

        /// <summary>
        /// Create SEGMENT_INFO_MESSAGE package.
        /// </summary>
        /// <param name="connectionInfoPort">
        /// A 16-bit unsigned integer that MUST be set by the client to the port 
        /// on which it is listening as a server-role peer, for use with the retrieval protocol.
        /// </param>
        /// <param name="contentInformation">A Content Information data structure.</param>
        /// <param name="segmentIndex">Segment index.</param>
        /// <returns>Return the SEGMENT_INFO_MESSAGE.</returns>
        public SEGMENT_INFO_MESSAGE CreateSegmentInfoMessage(
            int connectionInfoPort,
            Content_Information_Data_Structure contentInformation,
            int segmentIndex)
        {
            MESSAGE_HEADER messageHeader;

            // MajorVersion (1 byte):  The major part of the version, which MUST be 0x01.
            // MinorVersion (1 byte):  The minor part of the version, which MUST be 0x00.
            // Padding (4 bytes):  The value of this field is indeterminate and MUST be ignored on processing
            messageHeader.MajorVersion = 1;
            messageHeader.MinorVersion = 0;
            messageHeader.MsgType = PCHC_MESSAGE_TYPE.SEGMENT_INFO_MESSAGE;
            messageHeader.Padding = new byte[4];

            Content_Information_Data_Structure segmentInfomation = new Content_Information_Data_Structure
                {
                    Version = contentInformation.Version,
                    dwHashAlgo = contentInformation.dwHashAlgo,
                    dwOffsetInFirstSegment = (uint)contentInformation.dwOffsetInFirstSegment,
                    dwReadBytesInLastSegment = contentInformation.segments[segmentIndex].cbSegment,
                    cSegments = 1,
                    segments = new SegmentDescription[] { contentInformation.segments[segmentIndex] },
                    blocks = new SegmentContentBlocks[] { contentInformation.blocks[segmentIndex] }
                };

            CONNECTION_INFORMATION connectionInfo;

            // Padding (6 bytes):  The value of this field is indeterminated and MUST be ignored on processing.
            // Port (2 bytes): A 16-bit unsigned integer that MUST be set by the client to the port on 
            // which it is listening as a server-role peer, for use with the retrieval protocol.
            connectionInfo.Padding = new byte[6];
            connectionInfo.Port = (ushort)connectionInfoPort;

            // ContentTag (16 bytes):  A structure consisting of 16 bytes of opaque data.
            byte[] contentTag = new byte[16];

            SEGMENT_INFO_MESSAGE segmentInfoMessage;
            segmentInfoMessage.ConnectionInfo = connectionInfo;
            segmentInfoMessage.ContentTag = contentTag;
            segmentInfoMessage.MsgHeader = messageHeader;
            segmentInfoMessage.SegmentInfo = segmentInfomation;

            return segmentInfoMessage;
        }

        /// <summary>
        /// Create SEGMENT_INFO_MESSAGE package.
        /// </summary>
        /// <param name="connectionInfoPort">
        /// A 16-bit unsigned integer that MUST be set by the client to the port 
        /// on which it is listening as a server-role peer, for use with the retrieval protocol.
        /// </param>
        /// <param name="segmentInformation">A Content Information data structure.</param>
        /// <returns>Return the SEGMENT_INFO_MESSAGE.</returns>
        public SEGMENT_INFO_MESSAGE CreateSegmentInfoMessage(
            int connectionInfoPort,
            Content_Information_Data_Structure segmentInformation)
        {
            MESSAGE_HEADER messageHeader;

            // MajorVersion (1 byte):  The major part of the version, which MUST be 0x01.
            // MinorVersion (1 byte):  The minor part of the version, which MUST be 0x00.
            // Padding (4 bytes):  The value of this field is indeterminate and MUST be ignored on processing
            messageHeader.MajorVersion = 1;
            messageHeader.MinorVersion = 0;
            messageHeader.MsgType = PCHC_MESSAGE_TYPE.SEGMENT_INFO_MESSAGE;
            messageHeader.Padding = new byte[4];
            Content_Information_Data_Structure segmentInfomation = segmentInformation;

            CONNECTION_INFORMATION connectionInfo;

            // Padding (6 bytes):  The value of this field is indeterminated and MUST be ignored on processing.
            // Port (2 bytes): A 16-bit unsigned integer that MUST be set by the client to the port on 
            // which it is listening as a server-role peer, for use with the retrieval protocol.
            connectionInfo.Padding = new byte[6];
            connectionInfo.Port = (ushort)connectionInfoPort;

            // ContentTag (16 bytes):  A structure consisting of 16 bytes of opaque data.
            byte[] contentTag = new byte[16];

            SEGMENT_INFO_MESSAGE segmentInfoMessage;
            segmentInfoMessage.ConnectionInfo = connectionInfo;
            segmentInfoMessage.ContentTag = contentTag;
            segmentInfoMessage.MsgHeader = messageHeader;
            segmentInfoMessage.SegmentInfo = segmentInfomation;

            return segmentInfoMessage;
        }

        /// <summary>
        /// Create BATCHED_OFFER_MESSAGE package.
        /// </summary>
        /// <param name="connectionInfoPort">
        /// A 16-bit unsigned integer that MUST be set by the client to the port 
        /// on which it is listening as a server-role peer, for use with the retrieval protocol.
        /// </param>
        /// <param name="contentInfo">A Content Information V2 data structure.</param>
        /// <returns>Return the BATCHED_OFFER_MESSAGE.</returns>
        public BATCHED_OFFER_MESSAGE CreateBatchedOfferMessage(
            int connectionInfoPort,
            Content_Information_Data_Structure_V2 contentInfo)
        {
            MESSAGE_HEADER messageHeader;

            messageHeader.MajorVersion = 2;
            messageHeader.MinorVersion = 0;
            messageHeader.MsgType = PCHC_MESSAGE_TYPE.BATCHED_OFFER_MESSAGE;
            messageHeader.Padding = new byte[4];

            CONNECTION_INFORMATION connectionInfo;

            connectionInfo.Padding = new byte[6];
            connectionInfo.Port = (ushort)connectionInfoPort;

            BATCHED_OFFER_MESSAGE batchedOfferMessage;
            batchedOfferMessage.MessageHeader = messageHeader;
            batchedOfferMessage.ConnectionInfo = connectionInfo;

            List<SegmentDescriptor> segmentDescriptors = new List<SegmentDescriptor>();
            for (int i = 0; i < contentInfo.chunks.Length; i++)
            {
                var chunk = contentInfo.chunks[i];
                for (int j = 0; j < chunk.chunkData.Length; j++)
                {
                    var segment = chunk.chunkData[j];
                    SegmentDescriptor segmentDescriptor = new SegmentDescriptor();
                    segmentDescriptor.BlockSize = segment.cbSegment;
                    segmentDescriptor.SegmentSize = segment.cbSegment;
                    segmentDescriptor.SizeOfContentTag = 16;
                    segmentDescriptor.ContentTag = new byte[16];
                    segmentDescriptor.HashAlgorithm = (byte)contentInfo.dwHashAlgo;
                    segmentDescriptor.SegmentHoHoDk = contentInfo.GetSegmentId(i, j);
                    segmentDescriptors.Add(segmentDescriptor);
                }
            }
            batchedOfferMessage.SegmentDescriptors = segmentDescriptors.ToArray();

            return batchedOfferMessage;
        }

        /// <summary>
        /// Create INITIAL_OFFER_MESSAGE package.
        /// </summary>
        /// <param name="connectionInfoPort">
        /// A 16-bit unsigned integer that MUST be set by the client to the port 
        /// on which it is listening as a server-role peer, for use with the retrieval protocol.
        /// </param>
        /// <param name="hash">The HoHoDk value of the segment.</param>
        /// <returns>Return the INITIAL_OFFER_MESSAGE.</returns>
        public INITIAL_OFFER_MESSAGE CreateInitialOfferMessage(
            int connectionInfoPort,
            byte[] hash)
        {
            if (hash == null)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning("Please specify the Segment ID under test to the param \"hash\".");
                }

                throw new ArgumentNullException("hash", "Please specify the Segment ID under test to the param \"hash\".");
            }

            MESSAGE_HEADER header;

            // MajorVersion (1 byte):  The major part of the version, which MUST be 0x01.
            // MinorVersion (1 byte):  The minor part of the version, which MUST be 0x00.
            // Padding (4 bytes):  The value of this field is indeterminate and MUST be ignored on processing
            header.MajorVersion = 1;
            header.MinorVersion = 0;
            header.Padding = new byte[4];
            header.MsgType = PCHC_MESSAGE_TYPE.INITIAL_OFFER_MESSAGE;

            CONNECTION_INFORMATION connectionInfo;

            // Padding (6 bytes):  The value of this field is indeterminated and MUST be ignored on processing.
            // Port (2 bytes): A 16-bit unsigned integer that MUST be set by the client to the port on 
            // which it is listening as a server-role peer, for use with the retrieval protocol.
            connectionInfo.Port = (ushort)connectionInfoPort;
            connectionInfo.Padding = new byte[6];

            INITIAL_OFFER_MESSAGE initialOfferMessage;

            initialOfferMessage.MsgHeader = header;
            initialOfferMessage.Hash = hash;
            initialOfferMessage.ConnectionInfo = connectionInfo;

            return initialOfferMessage;
        }

        /// <summary>
        /// Send the INITIAL_OFFER_MESSAGE request.
        /// </summary>
        /// <param name="initialOfferMessage">The INITIAL_OFFER_MESSAGE message.</param>
        /// <returns>The repsonse message RESPONSE_MESSAGE from the hosted cache.</returns>
        public RESPONSE_MESSAGE SendInitialOfferMessage(INITIAL_OFFER_MESSAGE initialOfferMessage)
        {
            return this.responseMessage = this.SendByte(EncodeMessage.EncodeInitialOfferMessage(initialOfferMessage));
        }

        /// <summary>
        /// Send the SEGMENT_INFO_MESSAGE request.
        /// </summary>
        /// <param name="segmentInfoMessage">The SEGMENT_INFO_MESSAGE message.</param>
        /// <returns>The repsonse message RESPONSE_MESSAGE from the hosted cache.</returns>
        public RESPONSE_MESSAGE SendSegmentInfoMessage(SEGMENT_INFO_MESSAGE segmentInfoMessage)
        {
            return this.responseMessage = this.SendByte(EncodeMessage.EncodeSegmentInfoMessage(segmentInfoMessage));
        }

        /// <summary>
        /// Send the BATCHED_OFFER_MESSAGE request.
        /// </summary>
        /// <param name="batchedOfferMessage">The BATCHED_OFFER_MESSAGE message.</param>
        /// <returns>The repsonse message RESPONSE_MESSAGE from the hosted cache.</returns>
        public RESPONSE_MESSAGE SendBatchedOfferMessage(BATCHED_OFFER_MESSAGE batchedOfferMessage)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(TypeMarshal.ToBytes(batchedOfferMessage.MessageHeader));
            buffer.AddRange(TypeMarshal.ToBytes(batchedOfferMessage.ConnectionInfo));
            for (int i = 0; i < batchedOfferMessage.SegmentDescriptors.Length; i++)
            {
                buffer.AddRange(TypeMarshal.ToBytes(batchedOfferMessage.SegmentDescriptors[i]));
            }
            return this.responseMessage = this.SendByte(buffer.ToArray());
        }

        /// <summary>
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion

        #region private method

        /// <summary>
        /// Send the byte array of PCHC message using https transport .
        /// </summary>
        /// <param name="httpRequestPayload">The http request payload.</param>
        /// <returns>The pchc response message.</returns>
        private RESPONSE_MESSAGE SendByte(byte[] httpRequestPayload)
        {
            // Set the timeout of http.
            int timeout = 20000;
            byte[] payloadBuffer = null;
            HttpWebResponse httpWebResponse;

            this.httpClientTransport.Send(HttpVersion.Version10, null, httpRequestPayload, HttpMethod.POST, timeout);

            try
            {
                httpWebResponse = this.httpClientTransport.Receive(ref payloadBuffer);
            }
            catch (WebException e)
            {
                if (e.Message.Contains(HttpStatusCode.Unauthorized.ToString()))
                {
                    throw new HttpStatusCode401Exception();
                }
                else if (e.Message.ToLower().Contains("timed out".ToLower()))
                {
                    throw new NoRESPONSEMESSAGEException();
                }
                else
                {
                    // Un expected exception is received.
                    throw;
                }
            }

            this.httpResponseMethod = httpWebResponse.Method;

            this.httpResponseUri = httpWebResponse.ResponseUri;

            if (payloadBuffer == null)
            {
                throw new NoRESPONSEMESSAGEException();
            }

            try
            {
                this.responseMessage = DecodeMessage.DecodeResponseMessage(payloadBuffer);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new NoRESPONSEMESSAGEException(e.Message);
            }

            return this.responseMessage;
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed
        /// </summary>
        /// <param name="disposing">Specify which scenario is used.</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    if (this.httpClientTransport != null)
                    {
                        this.httpClientTransport.Dispose();
                        this.httpClientTransport = null;
                    }
                }
            }

            // Note disposing has been done.
            this.disposed = true;
        }

        #endregion
    }
}
