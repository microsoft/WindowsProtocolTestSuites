using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpedyc
{
    
    public interface IRdpedycAdapter: IAdapter
    {
        void ConnectToServer(EncryptedProtocol encryptProtocol,
           requestedProtocols_Values requestedProtocols,         
            string[] SVCNames,
            CompressionType highestCompressionTypeSupported = CompressionType.PACKET_COMPR_TYPE_NONE,
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
           );
        
        void ExchangeCapabilities(TimeSpan timeout);
        void SendUncompressedPdu(uint channelId, DynamicVC_TransportType transportType);
        DynamicVirtualChannel ExpectChannel(TimeSpan timeout, string channelName, DynamicVC_TransportType transportType);
        void CloseChannel(TimeSpan timeout, ushort channelId);
        SoftSyncReqDvcPDU ExpectSoftSyncReqPDU(TimeSpan timeout, DynamicVC_TransportType transportType);
        void SendCompressedSequencePdu(uint channelId, DynamicVC_TransportType transportType);        
    }
}
