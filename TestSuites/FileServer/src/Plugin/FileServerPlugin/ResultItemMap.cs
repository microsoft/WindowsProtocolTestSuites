// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
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
}
