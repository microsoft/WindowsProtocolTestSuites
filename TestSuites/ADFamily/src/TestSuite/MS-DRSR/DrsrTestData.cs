// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{    
    public static class DRSTestData
    {
        public const string DRSAddEntry_V2_Create_crossRef_newObjectPrefixName = "test";
        public static string DRSWriteSPN_V1_Success_AddSPNs_sPN1 = "host/dc";
        public static string DRSWriteSPN_V1_Success_AddSPNs_sPN2 = "host/ds3";
        public static string DRSGetReplInfo_ExistUser = "CN={0},CN=Users";
        public static string DRSGetReplInfo_ExistGroup = "CN=Domain Admins,CN=Users";
        public static string DRSGetNCChanges_NewPassword = "TestPassword!";
        public static string DRSGetNCChanges_OldPassword = "1*admin";
        public static string DRSGetNCChange_ExistGroup = "CN=Domain Admins,CN=Users";
        public static string DRSGetNCChange_ExistUser = "CN=Tester,CN=Users";
        public static string DrsCrackNames_DisplayNameRDN = "CN=DS-Replication-Get-Changes,CN=Extended-Rights";

    }
}
