// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// All constants defined by DRS protocol
    /// </summary>
    public static class DRSConstants
    {
        /// <summary>
        /// Client NTDS API Guid
        /// </summary>
        public static readonly Guid NTDSAPI_CLEINT_GUID = new Guid("{e24d201a-4fd6-11d1-a3da-0000f875ae0d}");

        public static readonly Guid DrsRpcInterfaceGuid = new Guid("E3514235-4B06-11D1-AB04-00C04FC2DCD2");
        /// <summary>
        /// extend rights GUID
        /// </summary>
        public static class ExtendRights
        {
            /// <summary>
            /// DS-Clone-Domain-Controller
            /// </summary>
            public static readonly Guid DSCloneDomainController = new Guid("3e0f7e18-2c7a-4c10-ba82-4d926db99a3e");
        }

        public static string RDN_OID = "1.2.840.113556.1.4.1";
    }
}
