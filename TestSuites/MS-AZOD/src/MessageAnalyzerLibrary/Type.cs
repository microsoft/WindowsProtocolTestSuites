// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.MessageAnalyzer
{   
    /// <summary>
    /// Address type
    /// </summary>
    public enum AddressType
    {
        None,
        IPv4,
        IPv6,
        HTTP,
        MAC,
        WFP
    }
       
    /// <summary>
    /// ETWProvider contains a provider list for MA
    /// </summary>
    public class ETWProvider
    {
        /// <summary>
        /// Name for Windows NDIS provider
        /// </summary>
        public static string Windows_NDIS = "Microsoft-Windows-NDIS-PacketCapture";

        /// <summary>
        /// Name for PEF NDIS provider
        /// </summary>
        public static string PEF_NDIS = "Microsoft-PEF-NDIS-PacketCapture";
    
        /// <summary>
        /// Name for PEF WFP
        /// </summary>
        public static string PEF_WFP = "Microsoft-Pef-WFP-MessageProvider";

        
    }
}
