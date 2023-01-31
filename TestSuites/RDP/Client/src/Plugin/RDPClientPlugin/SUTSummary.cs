// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Detector;

namespace Microsoft.Protocols.TestManager.RDPClientPlugin
{
    public class SUTSummary
    {
        private DetectionInfo detectionInfo = null;
        private const string protocolDescription = "\"Protocols\" use the Dynamic Virutal Channel. Execution Console tries to establish a Dynamic Virtual Channel to detect which protocol is supported in AutoDetection Phase";
        private const string featureDescription = "\"Features\" are found supported or not supported by analyzing the flags set in Client MCS Connect Initial PDU and Client Confirm Active PDU.";
        private ResultItemMap protocolItems = new ResultItemMap() { Header = "Protocols Support Info", Description = protocolDescription };
        private ResultItemMap featureItems = new ResultItemMap() { Header = "Features Support Info", Description = featureDescription };

        public List<ResultItemMap> LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            this.detectionInfo = detectionInfo;

            // Add protocolItems
            AddResultItem(ref protocolItems, "MS-RDPBCGR", true);
            AddResultItem(ref protocolItems, "MS-RDPEDISP", detectionInfo.IsSupportRDPEDISP);
            AddResultItem(ref protocolItems, "MS-RDPEGFX", detectionInfo.IsSupportRDPEGFX);
            AddResultItem(ref protocolItems, "MS-RDPEI", detectionInfo.IsSupportRDPEI);
            AddResultItem(ref protocolItems, "MS-RDPEMT", detectionInfo.IsSupportRDPEMT);
            AddResultItem(ref protocolItems, "MS-RDPEUDP", detectionInfo.IsSupportRDPEUDP);
            AddResultItem(ref protocolItems, "MS-RDPEUDP2", detectionInfo.IsSupportRDPEUDP2);
            AddResultItem(ref protocolItems, "MS-RDPEUSB", detectionInfo.IsSupportRDPEUSB);
            AddResultItem(ref protocolItems, "MS-RDPEVOR", detectionInfo.IsSupportRDPEVOR);
            AddResultItem(ref protocolItems, "MS-RDPRFX", detectionInfo.IsSupportRDPRFX);
            AddResultItem(ref protocolItems, "MS-RDPEFS (Used to test static virtual channel)", detectionInfo.IsSupportRDPEFS);

            // Add featuresItems

            AddResultItem(ref featureItems, "Auto Reconnect", detectionInfo.IsSupportAutoReconnect);
            AddResultItem(ref featureItems, "Server Redirection", detectionInfo.IsSupportServerRedirection);
            AddResultItem(ref featureItems, "Network Characteristics Detection", detectionInfo.IsSupportNetcharAutoDetect);
            
            AddResultItem(ref featureItems, "Connection Health Monitoring", detectionInfo.IsSupportHeartbeatPdu);
            AddResultItem(ref featureItems, "Static Virtual Channels", detectionInfo.IsSupportStaticVirtualChannel);
            AddResultItem(ref featureItems, "RDP-UDP Forward Error Correction reliable transport", detectionInfo.IsSupportTransportTypeUdpFECR);
            AddResultItem(ref featureItems, "RDP-UDP Forward Error Correction lossy transport", detectionInfo.IsSupportTransportTypeUdpFECL);

            List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();
            resultItemMapList.Add(protocolItems);
            resultItemMapList.Add(featureItems);

            return resultItemMapList;
        }

        private void AddResultItem(ref ResultItemMap resultItemMap, string value, bool? result)
        {
            DetectResult detectResult = new DetectResult();
            switch (result)
            {
                case true:
                    detectResult = DetectResult.Supported;
                    break;
                case false:
                    detectResult = DetectResult.UnSupported;
                    break;
                case null:
                    detectResult = DetectResult.DetectFail;
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = detectResult, Name = value };
            resultItemMap.ResultItemList.Add(item);
        }
    }
}