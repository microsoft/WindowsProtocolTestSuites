// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    public partial class SmbdDetector
    {
        public DetectionInfo DetectionInfo { get; private set; }
        private DetectLogger logWriter = new DetectLogger();

        public SmbdDetector(DetectionInfo detectionInfo)
        {
            DetectionInfo = detectionInfo;
        }

        public bool Detect(DetectContext context)
        {
            logWriter.ApplyDetectContext(context);
            logWriter.AddLog(DetectLogLevel.Information, "===== Start detecting =====");

            List<Func<bool>> detectActions = new List<Func<bool>>();
            detectActions.Add(() => PingSut());
            detectActions.Add(() => { if (GetOSVersion()) { DetectionInfo.OSDetected = true; } else { DetectionInfo.OSDetected = false; } return true; });
            detectActions.Add(() => GetLocalAdapters());
            detectActions.Add(() => CheckSmbDialects());
            detectActions.Add(() => GetRemoteAdapters());
            detectActions.Add(() => { if (ConnectToShareNonRDMA()) { DetectionInfo.NonRDMATransportSupported = true; } else { DetectionInfo.NonRDMATransportSupported = false; } return true; });
            detectActions.Add(() => { if (ConnectToShareRDMA()) { DetectionInfo.RDMATransportSupported = true; } else { DetectionInfo.RDMATransportSupported = false; } return true; });
            detectActions.Add(() =>
            {
                bool result = CheckSMBDCapability(out RdmaAdapterInfo rdmaAdapterInfo, out bool rdmaChannelV1Supported, out bool rdmaChannelV1InvalidateSupported);
                DetectionInfo.InboundEntries = rdmaAdapterInfo.MaxInboundRequests;
                DetectionInfo.OutboundEntries = rdmaAdapterInfo.MaxOutboundRequests;
                DetectionInfo.InboundReadLimit = rdmaAdapterInfo.MaxInboundReadLimit;
                DetectionInfo.RDMAChannelV1Supported = rdmaChannelV1Supported;
                DetectionInfo.RDMAChannelV1InvalidateSupported = rdmaChannelV1InvalidateSupported;
                logWriter.AddLog(DetectLogLevel.Information, "===== End detecting =====");
                return result;
            });

            foreach (Func<bool> func in detectActions)
            {
                if (context.Token.IsCancellationRequested)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                    return false;
                }

                if (!func())
                {
                    return false;
                }
            }
            detectActions.Clear();

            return true;
        }
    }
}
