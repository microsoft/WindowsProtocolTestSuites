// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.RDPClientPlugin
{
    /// <summary>
    /// DetectResult 
    /// </summary>
    public enum DetectResult
    {
        /// <summary>
        /// Detected result is supported
        /// </summary>
        Supported,

        /// <summary>
        /// Detected result is not supported
        /// </summary>
        UnSupported,

        /// <summary>
        /// Detection failed
        /// </summary>
        DetectFail,
    }

    public enum TriggerMethod
    {
        Powershell,
        Manual,
        Managed,
        Shell
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
        public string ProxyIP;//The Proxy IP in SUT side if there's proxy setup.
        public string IsWindowsImplementation;
        public string DropConnectionForInvalidRequest;
        public int AgentListenPort;
        public int RDPServerPort;
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

    /// <summary>
    /// ResultItem
    /// </summary>
    public class ResultItem
    {
        /// <summary>
        /// DetectedResult status
        /// </summary>
        public DetectResult DetectedResult { get; set; }

        /// <summary>
        /// ResultItem name
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// ResultItemMap
    /// </summary>
    public class ResultItemMap
    {
        private List<ResultItem> resultItemList;

        /// <summary>
        /// ResultItemList
        /// </summary>
        public List<ResultItem> ResultItemList
        {
            get { return resultItemList; }
            set { resultItemList = value; }
        }

        /// <summary>
        /// ResultItemMap header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// ResultItemMap description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Create new instance of ResultItemMap
        /// </summary>
        public ResultItemMap()
        {
            resultItemList = new List<ResultItem>();
        }
    }
}
