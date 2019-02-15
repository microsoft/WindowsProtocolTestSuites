// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestManager.Detector;

namespace Microsoft.Protocols.TestManager.RDPClientPlugin
{
    public enum TriggerMethod
    {
        Powershell,
        Manual,
        Managed,
    }
    /// <summary>
    /// The detection information
    /// </summary>
    public class DetectionInfo
    {
        // Parameters for Detecting
        public string SUTName;
        public string UserNameInTC;
        public string UserPwdInTC;
        public string IsWindowsImplementation;
        public string DropConnectionForInvalidRequest;
        public int AgentListenPort;
        public TriggerMethod TriggerMethod;
                
        // Detect Result
        public bool? IsSupportTransportTypeUdpFECR;
        public bool? IsSupportTransportTypeUdpFECL;
        public bool? IsSupportAutoReconnect;
        public bool? IsSupportServerRedirection;
        public bool? IsSupportNetcharAutoDetect;
        public bool? IsSupportHeartbeatPdu;
        public bool? IsSupportStaticVirtualChannel;
        public bool? IsSupportRDPEVOR;
        public bool? IsSupportRDPEGFX;
        public bool? IsSupportRDPEDISP;
        public bool? IsSupportRDPEI;
        public bool? IsSupportRDPEUSB;
        public bool? IsSupportRDPEFS;
        public bool? IsSupportRDPRFX;
        public bool? IsSupportRDPEUDP;
        public bool? IsSupportRDPEMT;

        public string RdpVersion;
    }

    public class ResultItemMap
    {
        private List<ResultItem> resultItemList;
        public List<ResultItem> ResultItemList
        {
            get { return resultItemList; }
            set { resultItemList = value; }
        }

        public string Header { get; set; }
        public string Description { get; set; }

        public ResultItemMap()
        {
            resultItemList = new List<ResultItem>();
        }
    }

    public class ResultItem
    {
        public DetectResult DetectedResult { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }

    public enum DetectResult
    {
        Supported,
        UnSupported,
        DetectFail,
    }
}
