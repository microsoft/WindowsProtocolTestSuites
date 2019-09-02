// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    public enum TriggerMethod
    {
        Powershell,
        Manual,
        Managed,
    }
    /// <summary>
    /// Platform type of the SUT
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Non Windows implementation
        /// </summary>
        NonWindows = 0x00000000,

        /// <summary>
        /// Windows Server 2008
        /// </summary>
        WindowsServer2008 = 0x10000002,

        /// <summary>
        /// Windows Server 2008 R2
        /// </summary>
        WindowsServer2008R2 = 0x10000004,

        /// <summary>
        /// Windows Server 2012
        /// </summary>
        WindowsServer2012 = 0x10000006,

        /// <summary>
        /// Windows Server 2012 R2
        /// </summary>
        WindowsServer2012R2 = 0x10000007,

        /// <summary>
        /// Windows Server 2016
        /// </summary>
        WindowsServer2016 = 0x10000008,

        /// <summary>
        /// Windows Server v1709
        /// </summary>
        WindowsServerV1709 = 0x10000009,

        /// <summary>
        /// Windows Server v1803
        /// </summary>
        WindowsServerV1803 = 0x1000000A,

        /// <summary>
        /// Windows Server 2019 
        /// </summary>
        WindowsServer2019 = 0x1000000B,

        /// <summary>
        /// Windows Server v1903
        /// </summary>
        WindowsServerV1903 = 0x1000000C,
    }

    /// <summary>
    /// The detection information
    /// </summary>
    public class DetectionInfo
    {
        // Parameters for Detecting      
        public string DomainName { get; set; }

        public string ServerComputerName { get; set; }

        public string ServerVersion { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public string SharedPath { get; set; }

        public string CatalogName { get; set; }

        public string ServerOffset { get; set; }

        public string ClientName { get; set; }

        public string ClientOffset { get; set; }
        public string ClientVersion { get; set; }

        public bool IsWDSInstalled { get; set; }
        public bool IsServerWindows { get; set; }
        public string LanguageLocale { get; set; }

        public string LCID_VALUE { get; set; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public DetectionInfo()
        {
            ResetDetectResult();           
        }

        /// <summary>
        /// To reset the detection information.
        /// </summary>
        public void ResetDetectResult()
        {
            ServerComputerName = "sut";            
        }
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
