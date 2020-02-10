// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpedyc
{    
    public partial class RdpedycAdapter : ManagedAdapterBase
    {
        public const string SVCNameForEGT = "Microsoft::Windows::RDS::Geometry::v08.01";

        public RdpbcgrAdapter bcgrAdapter;

        public RdpedycClient rdpedycClientStack;          
       
        public override void Reset()
        {
            if (rdpedycClientStack != null)
            {
                rdpedycClientStack.Dispose();
            }

            this.bcgrAdapter.Reset();
            base.Reset();
        }

        public RdpedycAdapter(TestConfig testConfig)
        {
            this.bcgrAdapter = new RdpbcgrAdapter(testConfig);
        }

        public override void Initialize(ITestSite testSite )
        {
            base.Initialize(testSite);
            bcgrAdapter.Initialize(testSite);
        }

        /// <summary>
        /// Establish the RDPBCGR connection with RDP server 
        /// If the SVCNames is not null, then a RDPEDYC static channel is created.
        /// </summary>
        /// <param name="encryptProtocol">Encryption protocol, RDP, TLS, Credssp </param>
        /// <param name="requestedProtocols"></param>
        /// <param name="SVCNames">Static virtual virtual name, DRDYNVC for EDYC channel name</param>
        /// <param name="highestCompressionTypeSupported">whether compressed supported </param>
        /// <param name="isReconnect">whether reconnect supported </param>
        /// <param name="autoLogon">whether autologon supported </param>
        /// <param name="supportEGFX">whether EGFX supported </param>
        /// <param name="supportAutoDetect">whether auto detect supported </param>
        /// <param name="supportHeartbeatPDU">whether HeartbeatPDU supported </param>
        /// <param name="supportMultitransportReliable">whether MultitransportReliable supported </param>
        /// <param name="supportMultitransportLossy">whether MultitransportLossy supported </param>
        /// <param name="supportAutoReconnect">whether AutoReconnect supported </param>
        /// <param name="supportFastPathInput">whether FastPathInput supported </param>
        /// <param name="supportFastPathOutput">whether FastPathOutput supported </param>
        /// <param name="supportSurfaceCommands">whether SurfaceCommands supported </param>
        /// <param name="supportSVCCompression">whether SVCCompression supported </param>
        /// <param name="supportRemoteFXCodec">whether RemoteFXCodec supported </param>
        public void ConnectToServer(EncryptedProtocol encryptProtocol,
           requestedProtocols_Values requestedProtocols,
            string[] SVCNames,
            CompressionType highestCompressionTypeSupported = CompressionType.PACKET_COMPR_TYPE_RDP61,
            bool isReconnect = false,
            bool autoLogon = false,
            bool supportEGFX = false,
            bool supportAutoDetect = false,
            bool supportHeartbeatPDU = false,
            bool supportMultitransportReliable = false,
            bool supportMultitransportLossy = false,
            bool supportAutoReconnect = false,
            bool supportFastPathInput = false,
            bool supportFastPathOutput = false,
            bool supportSurfaceCommands = false,
            bool supportSVCCompression = false,
            bool supportRemoteFXCodec = false
           )
       {
            this.bcgrAdapter.ConnectToServer(encryptProtocol);
            this.bcgrAdapter.EstablishRDPConnection(requestedProtocols,
                SVCNames,
                highestCompressionTypeSupported,
                isReconnect,
                autoLogon,
                supportEGFX,
                supportAutoDetect,
                supportHeartbeatPDU,
                supportMultitransportReliable,
                supportMultitransportLossy,
                supportAutoReconnect,
                supportFastPathInput,
                supportFastPathOutput,
                supportSurfaceCommands,
                supportSVCCompression,
                supportRemoteFXCodec
                );

            rdpedycClientStack = new RdpedycClient(bcgrAdapter.rdpbcgrClientStack.Context, false);
            
        }

        /// <summary>
        /// Expect the capabilites requset pdu from SUT
        /// </summary>
        /// <param name="timeout">Time span for waiting</param>
        public DYNVC_CAPS_Version ExchangeCapabilities(TimeSpan timeout)
        {
            if(rdpedycClientStack == null)
            {
                throw new Exception("RDPEDYC Client is required to be created before exchange capabilities.");
            }

            DYNVC_CAPS_Version version = rdpedycClientStack.ExchangeCapabilities(timeout);

            return version;            
        }

        /// <summary>
        /// Expect create channel reqeust from SUT
        /// </summary>
        /// <param name="timeout">Time span for waiting</param>
        /// <param name="channelName">Channel name to be created </param>
        /// <param name="transportType">Transport type, Tcp by default</param>
        /// <returns></returns>
        public DynamicVirtualChannel ExpectChannel(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {           
            if (rdpedycClientStack == null)
            {
                throw new InvalidOperationException("RDPEDYC Client is required to be created before create channel!");
            }
            DynamicVirtualChannel channel = rdpedycClientStack.ExpectChannel(timeout, SVCNameForEGT, transportType);

            return channel;
        }

        /// <summary>
        /// Close channel by channel ID
        /// </summary>
        /// <param name="timeout">Time span for waiting</param>
        /// <param name="channelId"> channel id to specify the channel to be closed</param>
        public void CloseChannel(TimeSpan timeout, ushort channelId)
        {
                       
            if (rdpedycClientStack == null)
            {
                throw new InvalidOperationException("RDPEDYC Client is required to be created before close channel!");
            }          

            rdpedycClientStack.CloseChannel(channelId);
        }

        /// <summary>
        /// Send DYNVC_DATA PDU to SUT
        /// </summary>
        /// <param name="channelId"> The channel ID</param>
        /// <param name="transportType"> transport type</param>
        public void SendUncompressedPdu(uint channelId, DynamicVC_TransportType transportType)
        {
            if (rdpedycClientStack == null)
            {
                throw new InvalidOperationException("RDPEDYC Client is required to be created before send data!");
            }

            this.rdpedycClientStack.SendUncompressedPdu(channelId, transportType);

            return;
        }        

        /// <summary>
        /// Send compressed data sequence PDU to SUT, including the DYNVC_DATA_FIST_COMPRESSED and DYNVC_DATA_COMPRESSED
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="transportType"></param>
        public void SendCompressedSequencePdu(uint channelId, DynamicVC_TransportType transportType)
        {
            if (rdpedycClientStack == null)
            {
                throw new InvalidOperationException("RDPEDYC Client is required to be created before send data!");
            }

            this.rdpedycClientStack.SendCompressedData(channelId, transportType);

            return;
        }              

        /// <summary>
        /// Expect the Soft-sync request from SUT
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public SoftSyncReqDvcPDU ExpectSoftSyncReqPDU(TimeSpan timeout, DynamicVC_TransportType transportType)
        {
            if (rdpedycClientStack == null)
            {
                throw new InvalidOperationException("RDPEDYC Client is required to be created before expect soft sync request.");
            }
            SoftSyncReqDvcPDU pdu = this.rdpedycClientStack.ExpectSoftSyncReqPDU(timeout, transportType);

            return pdu;
        }

    }
}
