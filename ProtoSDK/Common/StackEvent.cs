// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Contains stack event information.
    /// </summary>
    public class StackEvent
    {
        private IPEndPoint endPoint;
        private object eventObject;

        /// <summary>
        /// The endpoint of remote machine
        /// </summary>
        public IPEndPoint EndPoint
        {
            get
            {
                return this.endPoint;
            }
            set
            {
                this.endPoint = value;
            }
        }

        /// <summary>
        /// The event object. contains real event information
        /// </summary>
        public object EventObject
        {
            get
            {
                return this.eventObject;
            }
            set
            {
                this.eventObject = value;
            }
        }


        /// <summary>
        /// Constructor with specified endPoint and evtObj
        /// </summary>
        /// <param name="endPoint">The endPoint</param>
        /// <param name="evtObj">The event object</param>
        public StackEvent(IPEndPoint endPoint, object evtObj)
        {
            this.endPoint = endPoint;
            this.eventObject = evtObj;
        }
    }
}
