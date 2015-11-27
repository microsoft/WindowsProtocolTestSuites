// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RPCE client context.
    /// </summary>
    public class RpceClientContext : RpceContext
    {
        // RPC network address of remote host
        private string networkAddress;

        // RPC endpoint of remote host
        private string endpoint;

        // List of outstanding call id
        private Collection<uint> outstandingCalls;

        // Pdus received from server.
        private Dictionary<uint, RpcePdu> receivedPdus;
        
        // Events added in the background thread.
        private Collection<Object> events;

        // File service transport (SMB/SMB2)
        internal FileServiceClientTransport fileServiceTransport;

        // TCP transport
        internal TransportStack tcpTransport;

        //The max input buffer length in ioctl response
        private uint maxInputResponse;

        //The max output buffer length in ioctl response
        private uint maxOutputResponse = 1024 * 30;

        // Indicates whether the transport uses another thread to receive the response from server asynchronously.
        private bool isAsynchronous;

        // Indicates whether the SMB transport execute a transaction over the named pipe encompassing the write of the last PDU and the read of the first PDU on the client for synchronous RPCs
        private bool useTransactionForNamedPipe = true;

        /// <summary>
        /// Initialize a RPCE client context.
        /// </summary>
        internal RpceClientContext()
        {
            outstandingCalls = new Collection<uint>();
            receivedPdus = new Dictionary<uint, RpcePdu>();
            events = new Collection<Object>();
        }


        /// <summary>
        /// Network address of RPCE remote host.
        /// </summary>
        public string NetworkAddress
        {
            get
            {
                return networkAddress;
            }
            internal set
            {
                networkAddress = value;
            }
        }


        /// <summary>
        /// Endpoint of RPCE remote host.
        /// </summary>
        public string Endpoint
        {
            get
            {
                return endpoint;
            }
            internal set
            {
                endpoint = value;
            }
        }


        /// <summary>
        /// Security provider (SSPI).
        /// </summary>
        public new SecurityContext SecurityContext
        {
            get
            {
                return base.SecurityContext;
            }
            set
            {
                base.SecurityContext = value;
            }
        }


        /// <summary>
        /// Call Id of all outstanding calls.
        /// </summary>
        public Collection<uint> OutstandingCalls
        {
            get
            {
                return outstandingCalls;
            }
        }


        /// <summary>
        /// Pdus received from server.
        /// </summary>
        public Dictionary<uint, RpcePdu> ReceivedPdus
        {
            get
            {
                return receivedPdus;
            }
        }

        /// <summary>
        /// Events added in the receiving thread.
        /// </summary>
        public Collection<Object> Events
        {
            get
            {
                return events;
            }
        }


        /// <summary>
        /// Context of underlayer named pipe transport (SMB/SMB2).
        /// User should cast return value to SmbClientContext or Smb2ClientContext.
        /// If RPCE is using TCP as its transport, this property returns null.
        /// If underlayer named pipe transport is closed, this property returns null.
        /// </summary>
        public FileServiceClientContext FileServiceTransportContext
        {
            get
            {
                if (fileServiceTransport == null)
                {
                    return null;
                }
                else
                {
                    return fileServiceTransport.Context;
                }
            }
        }

        /// <summary>
        /// The max input buffer length in ioctl response
        /// </summary>
        public uint MaxInputResponse
        {
            get
            {
                return maxInputResponse;
            }
            set
            {
                maxInputResponse = value;
            }
        }
        
        /// <summary>
        /// The max output buffer length in ioctl response
        /// </summary>
        public uint MaxOutputResponse
        {
            get
            {
                return maxOutputResponse;
            }
            set
            {
                maxOutputResponse = value;
            }
        }

        /// <summary>
        /// Indicates whether the transport uses another thread to receive the response from server asynchronously.
        /// </summary>
        public bool IsAsynchronous
        {
            get
            {
                return isAsynchronous;
            }
            set
            {
                isAsynchronous = value;
            }
        }

        /// <summary>
        /// Indicates whether the SMB transport execute a transaction over the named pipe encompassing the write of the last PDU and the read of the first PDU on the client for synchronous RPCs
        /// </summary>
        public bool UseTransactionForNamedPipe
        {
            get
            {
                return useTransactionForNamedPipe;
            }
            set
            {
                useTransactionForNamedPipe = value;
            }
        }
    }
}
