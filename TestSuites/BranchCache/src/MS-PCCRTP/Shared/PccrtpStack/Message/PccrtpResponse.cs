// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp
{
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;

    /// <summary>
    /// The PCCRTP resopnse.
    /// </summary>
    public class PccrtpResponse
    {
        #region Fields

        /// <summary>
        /// Indicates the HTTP header in HTTP response.
        /// </summary>
        private Dictionary<string, string> httpHeader;

        /// <summary>
        /// Indicates the payload data in HTTP response.
        /// </summary>
        private byte[] payloadData;

        /// <summary>
        /// Indicates the HTTP response.
        /// </summary>
        private HttpWebResponse httpWebResponse;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PccrtpResponse class.
        /// </summary>
        public PccrtpResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PccrtpResponse class.
        /// </summary>
        /// <param name="data">Indicates the payload data returned from server.</param>
        public PccrtpResponse(byte[] data)
        {
            this.payloadData = data;
        }

        #endregion

        #region Properties

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
        /// Gets or sets payloadData.
        /// </summary>
        public byte[] PayloadData
        {
            get
            {
                return this.payloadData;
            }

            set
            {
                this.payloadData = value;
            }
        }

        /// <summary>
        /// Gets or sets HttpResponse.
        /// </summary>
        public HttpWebResponse HttpResponse
        {
            get
            {
                return this.httpWebResponse;
            }

            set
            {
                this.httpWebResponse = value;
            }
        }
        #endregion

        #region Decode Methods

        /// <summary>
        /// Decode the struct of HTTP response header.
        /// </summary>
        /// <param name="response">Indicates the HTTP response.</param>
        public void DecodeHttpHeader(WebResponse response)
        {
            Dictionary<string, string> tempHeaders = new Dictionary<string, string>();
            for (int i = 0; i < response.Headers.Count; i++)
            {
                tempHeaders.Add(response.Headers.Keys[i], response.Headers[i]);
            }

            this.httpHeader = tempHeaders;
        }

        #endregion
    }
}
