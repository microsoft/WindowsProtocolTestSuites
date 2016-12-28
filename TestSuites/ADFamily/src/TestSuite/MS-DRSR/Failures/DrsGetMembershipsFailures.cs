// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsGetMembershipsFailures : DrsrFailureTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrFailureTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrFailureTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSGetMemberships_Access_Denied()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainUser,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if not AccessCheckCAR(DefaultNC(), DS-Replication-Get-Changes) then
  return ERROR_DS_DRA_ACCESS_DENIED
endif

            */

            /* comments from likezh */
            /*
             not AccessCheckCAR(DefaultNC(), DS-Replication-Get-Changes)
            */
            //throw new NotImplementedException();

            /* Create request message */
            DRS_MSG_REVMEMB_REQ msgIn = drsTestClient.CreateDrsGetMembershipsV1Request();

            uint dwInVersion = 1;
            uint? dwOutVersion = 0;
            DRS_MSG_REVMEMB_REPLY? reply;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetMemberships(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED,
                ret, 
                "DrsGetMemberships: return code mismatch."
             ); 
        }


    }
}
