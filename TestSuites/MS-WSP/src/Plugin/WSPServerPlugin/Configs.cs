// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    public class Configs
    {
        private static PropertyInfo[] PropInfos { get; }

        public string ServerComputerName { get; private set; }

        public string ServerVersion { get; private set; }

        public string ServerOffset { get; private set; }

        public string IsServerWindows { get; private set; }

        public string IsWDSInstalled { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string SupportedSecurityPackage { get; private set; }

        public string DomainName { get; private set; }

        public string UseServerGssToken { get; private set; }

        public string SMB2ClientTimeout { get; private set; }

        public string SharedPath { get; private set; }

        public string ShareName { get; private set; }

        public string CatalogName { get; private set; }

        public string ClientName { get; private set; }

        public string ClientVersion { get; private set; }

        public string ClientOffset { get; private set; }

        public string LanguageLocale { get; private set; }

        public string LcidValue { get; private set; }

        public string QueryPath { get; private set; }

        public string QueryText { get; private set; }

        public string CbChunk { get; private set; }

        public string PropertySet_One_DBProperties { get; private set; }

        public string PropertySet_Two_DBProperties { get; private set; }

        public string Array_PropertySet_One_Guid { get; private set; }

        public string Array_PropertySet_One_DBProperties { get; private set; }

        public string Array_PropertySet_Two_Guid { get; private set; }

        public string Array_PropertySet_Two_DBProperties { get; private set; }

        public string Array_PropertySet_Three_Guid { get; private set; }

        public string Array_PropertySet_Three_DBProperties { get; private set; }

        public string Array_PropertySet_Four_Guid { get; private set; }

        public string Array_PropertySet_Four_DBProperties { get; private set; }

        public string WorkIdPropertyGuid { get; private set; }

        public string WorkId { get; private set; }

        public string ClientBase { get; private set; }

        public string BufferSize { get; private set; }

        public string RowsToTransfer { get; private set; }

        public string EType { get; private set; }

        static Configs()
        {
            var cfg = typeof(Configs);
            PropInfos = cfg.GetProperties();
        }

        public Configs()
        {
            foreach (var p in PropInfos)
            {
                var val = DetectorUtil.GetPropertyValue(p.Name);
                if (val != null)
                {
                    p.SetValue(this, val);
                }
            }
        }
    }
}
