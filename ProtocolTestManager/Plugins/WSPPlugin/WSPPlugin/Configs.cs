// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    public class Configs
    {
        public string DomainName { get; set; }
        public string ServerComputerName { get; set; }

        public string ServerVersion { get; set; }
        public string ServerOffset { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string SharedPath { get; set; }

        public string CatalogName { get; set; }      

        public string ClientName { get; set; }
        public string ClientVersion { get; set; }        

        public string ClientOffset { get; set; }

        public string IsWDSInstalled { get; set; }

        public string IsServerWindows { get; set; }

        public string LanguageLocale { get; set; }
        public string LCID_VALUE { get; set; }
        public string QueryPath { get; set; }
        public string QueryText { get; set; }
        public string CbChunk { get; set; }
        public string PropertySet_One_DBProperties { get; set; }
        public string PropertySet_Two_DBProperties { get; set; }

        public string Array_PropertySet_One_Guid { get; set; }
        public string Array_PropertySet_One_DBProperties { get; set; }

        public string Array_PropertySet_Two_Guid { get; set; }

        public string Array_PropertySet_Two_DBProperties { get; set; }

        public string Array_PropertySet_Three_Guid { get; set; }

        public string Array_PropertySet_Three_DBProperties { get; set; }

        public string Array_PropertySet_Four_Guid { get; set; }
        public string Array_PropertySet_Four_DBProperties { get; set; }

        public string NumberOfSetBindingsColumns { get; set; }
      
        public string WorkIdPropertyGuid { get; set; }
        public string WorkId { get; set; }
        public string ClientBase { get; set; }
        public string BufferSize { get; set; }
        public string RowsToTransfer { get; set; }
        public string EType { get; set; }


        public void LoadDefaultValues()
        {                    

            Type cfg = typeof(Configs);
            foreach (var p in cfg.GetProperties())
            {                
                var val = DetectorUtil.GetPropertyValue(p.Name);
                if (val != null)
                {
                    p.SetValue(this, val, null);
                }
            }
        }

        public Dictionary<string, List<string>> ToDictionary()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            Type cfg = typeof(Configs);
            foreach (var p in cfg.GetProperties())
            {                
                string value = p.GetValue(this, null).ToString();
                dict.Add(p.Name, new List<string>() { value });
            }
            return dict;
        }
    }
}
