// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// LspSession is used to identify the correlation of LspConsole and the LspManagementClient.
    /// </summary>
    public class LspSession
    {
        private int sessionId;
        private ProtocolEndPoint interceptedEndPoint;
        private IPEndPoint consoleEndPoint;
        private IPEndPoint managementClientEndPoint;

        /// <summary>
        /// Property of Session ID.
        /// </summary>
        public int SessionId
        {
            get
            {
                return this.sessionId;
            }
        }


        /// <summary>
        /// Property of Intercepted IPEndPoint.
        /// </summary>
        public ProtocolEndPoint InterceptedEndPoint
        {
            get
            {
                return this.interceptedEndPoint;
            }
            set
            {
                this.interceptedEndPoint = value;
            }
        }


        /// <summary>
        /// Property of LspConsole IPEndPoint.
        /// </summary>
        public IPEndPoint ConsoleEndPoint
        {
            get
            {
                return this.consoleEndPoint;
            }
            set
            {
                this.consoleEndPoint = value;
            }
        }


        /// <summary>
        /// Property of ManagementClient IPEndPoint.
        /// </summary>
        public IPEndPoint ManagementClientEndPoint
        {
            get
            {
                return this.managementClientEndPoint;
            }
            set
            {
                this.managementClientEndPoint = value;
            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sessionId">Session ID. </param>
        public LspSession(int sessionId)
        {
            this.sessionId = sessionId;
        }


    }
}
