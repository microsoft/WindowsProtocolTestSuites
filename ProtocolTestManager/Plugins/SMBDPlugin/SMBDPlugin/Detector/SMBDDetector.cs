// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    public partial class SMBDDetector
    {
        public DetectionInfo DetectionInfo { get; private set; }

        public SMBDDetector(DetectionInfo detectionInfo)
        {
            DetectionInfo = detectionInfo;
        }


        public bool Detect()
        {
            bool result;

            result = PingSUT();
            if (!result)
            {
                // target unreachable and stop
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


            result = GetLocalAdapters();
            if (!result)
            {
                // stop if local card not detected
                return false;
            }

            result = CheckSmbDialect();
            if (!result)
            {
                // stop if none support dialect
                return false;
            }

            result = GetRemoteAdapters();
            if (!result)
            {
                // stop if remote card not detected
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

            bool rdmaChannelV1Supported;
            bool rdmaChannelV1InvalidateSupported;
            result = CheckSMBDCapability(out rdmaChannelV1Supported, out rdmaChannelV1InvalidateSupported);
            DetectionInfo.RDMAChannelV1Supported = rdmaChannelV1Supported;
            DetectionInfo.RDMAChannelV1InvalidateSupported = rdmaChannelV1InvalidateSupported;

            return result;
        }

    }
}
