// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp
{
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// The PCCRTP request.
    /// </summary>
    public class PccrtpRequest
    {
        #region Fields

        /// <summary>
        /// Indicates the address of SUT (System Under Testing).
        /// </summary>
        private string serverAddress;

        /// <summary>
        /// Indicates the port of SUT.
        /// </summary>
        private int port;

        /// <summary>
        /// Indicates the file name on the SUT requested by the client.
        /// </summary>
        private string requestFileName;

        /// <summary>
        /// Indicates the pair of key and value of the HTTP request header.
        /// </summary>
        private Dictionary<string, string> httpHeader;

        /// <summary>
        /// Indicates the HTTP request palyload data in bytes.
        /// </summary>
        private byte[] payLoadData;

        /// <summary>
        /// Indicates the HTTP request.
        /// </summary>
        private HttpListenerRequest httpRequest;

        #endregion

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the PccrtpRequest class.
        /// </summary>
        public PccrtpRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PccrtpRequest class.
        /// </summary>
        /// <param name="serverAddress">Indicates the address of SUT (System Under Testing).</param>
        /// <param name="port">Indicates the port of SUT.</param>
        /// <param name="fileName">Indicates the requested file name on the SUT.</param>
        /// <param name="httpHeader">Indicates the pair of key and value of the HTTP request header.</param>
        public PccrtpRequest(
            string serverAddress, 
            int port, 
            string fileName, 
            Dictionary<string, string> httpHeader)
        {
            this.serverAddress = serverAddress;
            this.port = port;
            this.requestFileName = fileName;
            this.httpHeader = httpHeader;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the service address to use for the request.
        /// </summary>
        public string ServerAddress
        {
            get
            {
                return this.serverAddress;
            }

            set
            {
                this.serverAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the server endpoint's port number.
        /// </summary>
        public int Port
        {
            get
            {
                return this.port;
            }

            set
            {
                this.port = value;
            }
        }

        /// <summary>
        /// Gets or sets the requested file name.
        /// </summary>
        public string RequestFileName
        {
            get
            {
                return this.requestFileName;
            }

            set
            {
                this.requestFileName = value;
            }
        }

        /// <summary>
        /// Gets or sets httpHeader.
        /// </summary>
        public Dictionary<string, string> HttpHeader
        {
            get
            {
                return this.httpHeader;
            }

            set
            {
                this.httpHeader = value;
            }
        }

        /// <summary>
        /// Gets or sets the service address to use for the request.
        /// </summary>
        public byte[] PayLoadData
        {
            get
            {
                return this.payLoadData;
            }

            set
            {
                this.payLoadData = value;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP listener request.
        /// </summary>
        public HttpListenerRequest HttpRequest
        {
            get
            {
                return this.httpRequest;
            }

            set
            {
                this.httpRequest = value;
            }
        }

        #endregion

        #region Decode Methods

        /// <summary>
        /// Decode the struct of HTTP request header.
        /// </summary>
        /// <param name="request">Indicates the HTTP request.</param>
        public void DecodeHttpHeader(HttpListenerRequest request)
        {
            Dictionary<string, string> tempHeaders = new Dictionary<string, string>();
            for (int i = 0; i < request.Headers.Count; i++)
            {
                tempHeaders.Add(request.Headers.Keys[i], request.Headers[i]);
            }

            this.httpHeader = tempHeaders;
        }

        #endregion
    }
}

