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
    public class DrsGetReplInfoFailures : DrsrFailureTestClassBase
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
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSGetReplInfo_No_More_Items()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if dwInVersion = 1 then 
  baseIndex := 0
else 
  if msgIn.dwEnumerationContext = 0xffffffff then
    return ERROR_NO_MORE_ITEMS
  endif
  baseIndex := msgIn.dwEnumerationContext
endif

            */


            /* Create request message */
            DRS_MSG_GETREPLINFO_REQ msgIn = drsTestClient.CreateDrsGetReplInfoV2Request();

            uint dwInVersion = 2;
            uint? dwOutVersion = 0;
            DRS_MSG_GETREPLINFO_REPLY? reply;
            /* Setting param #1 */
            /*dwInVersion = 2*/
            dwInVersion = 2;
            /* Setting param #2 */
            /*msgIn.V2.dwEnumerationContext = 0xffffffff*/
            msgIn.V2.dwEnumerationContext = 0xffffffff;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetReplInfo(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_NO_MORE_ITEMS,
                ret, 
                "DrsGetReplInfo: return code mismatch."
             ); 
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSGetReplInfo_No_Object()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if InfoType in {DS_REPL_INFO_METADATA_FOR_OBJ,
                DS_REPL_INFO_METADATA_2_FOR_OBJ,
                DS_REPL_INFO_METADATA_FOR_ATTR_VALUE,
                DS_REPL_INFO_METATDATA_2_FOR_ATTR_VALUE} then
  if object = null then
    return ERROR_INVALID_PARAMETER
  endif

            */


            /* Create request message */
            DRS_MSG_GETREPLINFO_REQ msgIn = drsTestClient.CreateDrsGetReplInfoV1Request();

            uint dwInVersion = 1;
            uint? dwOutVersion = 0;
            DRS_MSG_GETREPLINFO_REPLY? reply;
            /* Setting param #1 */
            /*dwInVersion = 1*/
            dwInVersion = 1;
            /* Setting param #2 */
            /*msgIn.V1.InfoType = (uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_OBJ*/
            msgIn.V1.InfoType = (uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_OBJ;
            /* Setting param #3 */
            /*msgIn.V1.pszObjectDN = null*/
            msgIn.V1.pszObjectDN = null;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetReplInfo(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_INVALID_PARAMETER,
                ret, 
                "DrsGetReplInfo: return code mismatch."
             ); 
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSGetReplInfo_Invalid_Object()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if InfoType in {DS_REPL_INFO_METADATA_FOR_OBJ,
                DS_REPL_INFO_METADATA_2_FOR_OBJ,
                DS_REPL_INFO_METADATA_FOR_ATTR_VALUE,
                DS_REPL_INFO_METATDATA_2_FOR_ATTR_VALUE} then
  if object = null then
    return ERROR_INVALID_PARAMETER
  endif
  if  not ObjExists(object) then
    if object.dn = null then
    return ERROR_DS_DRA_BAD_DN
    else 
      return ERROR_DS_OBJ_NOT_FOUND
    endif
  endif

            */


            /* Create request message */
            DRS_MSG_GETREPLINFO_REQ msgIn = drsTestClient.CreateDrsGetReplInfoV1Request();

            uint dwInVersion = 1;
            uint? dwOutVersion = 0;
            DRS_MSG_GETREPLINFO_REPLY? reply;
            /* Setting param #1 */
            /*dwInVersion = 1*/
            dwInVersion = 1;
            /* Setting param #2 */
            /*msgIn.V1.InfoType = (uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_OBJ*/
            msgIn.V1.InfoType = (uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_OBJ;
            /* Setting param #3 */
            /*msgIn.V1.pszObjectDN = "InvalidObj"*/
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);

            msgIn.V1.pszObjectDN = "CN=InvalidObj," + rootDse.configurationNamingContext;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetReplInfo(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_OBJ_NOT_FOUND,
                ret, 
                "DrsGetReplInfo: return code mismatch."
             ); 
        }

    }
}
