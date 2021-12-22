// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    public partial class SMBDDetector
    {
        public DetectionInfo DetectionInfo { get; private set; }
        private DetectLogger logWriter = new DetectLogger();

        public SMBDDetector(DetectionInfo detectionInfo)
        {
            DetectionInfo = detectionInfo;
        }


        public bool Detect(DetectContext context)
        {
            bool result;

            logWriter.ApplyDetectContext(context);

            logWriter.AddLog(DetectLogLevel.Information, "===== Start detecting =====");

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            result = PingSUT();
            if (!result)
            {
                // target unreachable and stop
                return false;
            }

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            result = GetOSVersion();
            if (!result)
            {
                DetectionInfo.OSDetected = false;
            }
            else
            {
                DetectionInfo.OSDetected = true;
            }

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            result = GetLocalAdapters();
            if (!result)
            {
                // stop if local card not detected
                return false;
            }

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            result = CheckSmbDialect();
            if (!result)
            {
                // stop if none support dialect
                return false;
            }

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            result = GetRemoteAdapters();
            if (!result)
            {
                // stop if remote card not detected
                return false;
            }

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            result = ConnectToShareNonRDMA();
            if (!result)
            {
                DetectionInfo.NonRDMATransportSupported = false;
            }
            else
            {
                DetectionInfo.NonRDMATransportSupported = true;
            }

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            result = ConnectToShareRDMA();
            if (!result)
            {
                // stop if RDMA transport failed
                DetectionInfo.RDMATransportSupported = false;
            }
            else
            {
                DetectionInfo.RDMATransportSupported = true;
            }

            if (context.Token.IsCancellationRequested)
            {
                logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                return false;
            }
            RdmaAdapterInfo rdmaAdapterInfo;

            bool rdmaChannelV1Supported;
            bool rdmaChannelV1InvalidateSupported;

            result = CheckSMBDCapability(out rdmaAdapterInfo, out rdmaChannelV1Supported, out rdmaChannelV1InvalidateSupported);

            DetectionInfo.InboundEntries = rdmaAdapterInfo.MaxInboundRequests;

            DetectionInfo.OutboundEntries = rdmaAdapterInfo.MaxOutboundRequests;

            DetectionInfo.InboundReadLimit = rdmaAdapterInfo.MaxInboundReadLimit;

            DetectionInfo.RDMAChannelV1Supported = rdmaChannelV1Supported;
            DetectionInfo.RDMAChannelV1InvalidateSupported = rdmaChannelV1InvalidateSupported;

            logWriter.AddLog(DetectLogLevel.Information, "===== End detecting =====");
            return result;
        }

    }
}
