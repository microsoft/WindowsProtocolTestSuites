// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

            PingSUT();

            GetOSVersion();


            GetLocalAdapters();

            CheckSmbDialect();

            GetRemoteAdapters();

            ConnectToShareNonRDMA();

            ConnectToShareRDMA();

            CheckSMBDCapability();

            return true;
        }


    }
}
