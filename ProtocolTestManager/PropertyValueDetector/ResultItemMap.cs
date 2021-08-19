// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.Detector
{
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

    /// <summary>
    /// DetectResult enumeration.
    /// </summary>
    public enum DetectResult
    {
        /// <summary>
        /// Detected failed
        /// </summary>
        DetectFail,

        /// <summary>
        /// Detected result is supported
        /// </summary>
        Supported,

        /// <summary>
        /// Detected result is unsupported
        /// </summary>
        UnSupported,
    }
}
