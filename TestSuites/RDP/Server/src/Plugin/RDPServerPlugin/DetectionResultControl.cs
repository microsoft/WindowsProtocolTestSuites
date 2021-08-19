// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.RDPServerPlugin
{
    public class DetectionResultControl
    {
        #region Properties

        private DetectionInfo info = null;

        private const string protocolDescription = "\"Protocols\" use the Dynamic Virutal Channel. Execution Console tries to establish a Dynamic Virtual Channel to detect which protocol is supported in AutoDetection Phase";
        private const string featureDescription = "\"Features\" are found supported or not supported by analyzing the flags set in Client MCS Connect Initial PDU and Client Confirm Active PDU.";

        private ResultItemMap protocolItems = new ResultItemMap() { Header = "Protocols Support Info", Description = protocolDescription };
        private ResultItemMap featureItems = new ResultItemMap() { Header = "Features Support Info", Description = featureDescription };

        private List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();

        #endregion

        #region Private functions

        private void AddResultItem(ref ResultItemMap resultItemMap, string value, bool? result)
        {
            string imagePath = string.Empty;
            DetectResult detectResult = new DetectResult();
            switch (result)
            {
                case true:
                    imagePath = "/RDPServerPlugin;component/Icons/supported.png"; ;
                    detectResult = DetectResult.Supported;
                    break;
                case false:
                    imagePath = "/RDPServerPlugin;component/Icons/unsupported.png";
                    detectResult = DetectResult.UnSupported;
                    break;
                case null:
                    imagePath = "/RDPServerPlugin;component/Icons/undetected.png";
                    detectResult = DetectResult.DetectFail;
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = detectResult, ImageUrl = imagePath, Name = value };
            resultItemMap.ResultItemList.Add(item);
        }

        #endregion

        public List<ResultItemMap> LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            this.info = detectionInfo;

            // Add protocolItems
            AddResultItem(ref protocolItems, "MS-RDPBCGR", true);
            AddResultItem(ref protocolItems, "MS-RDPEDYC", detectionInfo.IsSupportRDPEDYC);
            AddResultItem(ref protocolItems, "MS-RDPEMT", detectionInfo.IsSupportRDPEMT);
            AddResultItem(ref protocolItems, "MS-RDPELE", detectionInfo.IsSupportRDPELE);

            // Add featuresItems
            AddResultItem(ref featureItems, "Auto Reconnect", detectionInfo.IsSupportAutoReconnect);
            AddResultItem(ref featureItems, "FastPath Input", detectionInfo.IsSupportFastPathInput);

            resultItemMapList.Add(protocolItems);
            resultItemMapList.Add(featureItems);
            return resultItemMapList;
        }
    }
}
