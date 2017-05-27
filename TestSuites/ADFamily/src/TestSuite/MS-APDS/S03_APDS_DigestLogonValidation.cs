// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.Security.Apds;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    /// <summary>
    /// Test suite to test the implementation for MS-APDS protocol.
    /// </summary>
    public partial class TestSuite
    {
        /// <summary>
        /// Test method for validating APDS server end-point requirements for Digest Validation protocol
        /// over HTTP when valid credentials are provided.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S3_TC01_DIGEST_OVER_HTTP_VALID_INFO()
        {
            IgnoredFields ignorefFields = new IgnoredFields();
            Status responseStatus = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.Valid,
                DigestType_Values.Basic,
                AlgType_Values.MD5SessDigestAndChecksum, 
                ignorefFields);

            //test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.Success,
                responseStatus,
                99,
                @"On successful validation, the Status field[in the DIGEST_VALIDATION_RESP Message] MUST be set to STATUS_SUCCESS.");
            
            //test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.Success,
                responseStatus,
                185,
                @"If validation is successful, a DIGEST_VALIDATION_RESP message with Status[MS-ERREF] indicating successful 
                authentication (that is, STATUS_SUCCESS).");

        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for Digest Validation protocol
        /// over HTTP when invalid credentials are provided.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S3_TC02_DIGEST_OVER_HTTP_INVALID_INFO()
        {

            IgnoredFields ignorefFields = new IgnoredFields();
            Status responseStatus = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.AccountNotExist,
                DigestType_Values.Basic,
                AlgType_Values.MD5SessDigestAndChecksum, 
                ignorefFields);

            //test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.LogonFailure,
                responseStatus,
                100,
                @"On logon failure, it[status field in the DIGEST_VALIDATION_RESP Message] MUST be set to STATUS_LOGON_FAILURE.");

            //test cases validation
            Site.CaptureRequirementIfAreNotEqual<Status>(
                Status.Success,
                responseStatus,
                186,
                @"If unsuccessful, the digest validation server DC MUST return an error code as an error status in NRPC API.");

        }


        /// <summary>
        /// Test method for validating APDS server end-point requirements for Digest Validation protocol
        /// over HTTP, whatever the value Reserved3 is set, the reply is same.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S3_TC03_DIGEST_DIFF_RESERVED3_SAME_REPLY()
        {
            IgnoredFields ignorefFields = new IgnoredFields();
            ignorefFields.reserved3 = (int)Reserved3.V1;
            Status responseStatus = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.Valid,
                DigestType_Values.Basic,
                AlgType_Values.MD5SessDigestAndChecksum,
                ignorefFields);

            IgnoredFields ignorefFieldsEx = new IgnoredFields();
            ignorefFieldsEx.reserved3 = (int)Reserved3.V2;
            Status responseStatusEx = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.Valid,
                DigestType_Values.Basic,
                AlgType_Values.MD5SessDigestAndChecksum,
                ignorefFieldsEx);

            //test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                responseStatus,
                responseStatusEx,
                241,
                @"Reply is the same whatever the value of Reserved3 field in the DIGEST_VALIDATION_REQ Message is set.");

        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for Digest Validation protocol
        /// over HTTP, whatever the value Reserved4 is set, the reply is same.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S3_TC04_DIGEST_DIFF_RESERVED4_SAME_REPLY()
        {
            IgnoredFields ignorefFields = new IgnoredFields();
            ignorefFields.reserved4 = (int)Reserved4.V1;
            Status responseStatus = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.Valid,
                DigestType_Values.Basic,
                AlgType_Values.MD5SessDigestAndChecksum,
                ignorefFields);

            IgnoredFields ignorefFieldsEx = new IgnoredFields();
            ignorefFieldsEx.reserved4 = (int)Reserved4.V2;
            Status responseStatusEx = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.Valid,
                DigestType_Values.Basic,
                AlgType_Values.MD5SessDigestAndChecksum,
                ignorefFieldsEx);

            //test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                responseStatus,
                responseStatusEx,
                244,
                @"Reply is the same whatever the value of Reserved4 field in the DIGEST_VALIDATION_REQ Message is set.");

        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for Digest Validation protocol
        /// over HTTP, DC will reject authentication request with MD5(AlgType:0x01).
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S3_TC05_DIGEST_REJECT_ALGTYPE_MD5()
        {
            // authentication request with MD5(AlgType:0x01)
            IgnoredFields ignorefFields = new IgnoredFields();
            Status responseStatus = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.Valid,
                DigestType_Values.Basic,
                AlgType_Values.MD5DigestAndChecksum,
                ignorefFields);

            //
            //Configured in PTFConfig file,default SHOULD to true and MAY to false.
            //
            string isR300182Implemented = "true";
            //
            //Check OS version
            //
            if (isServerWindows)
            {
                if (null == isR300182Implemented)
                {
                    Site.Properties.Add("R300182Implemented", Boolean.TrueString);
                    isR300182Implemented = Boolean.TrueString;
                }
            }
            if (null != isR300182Implemented)
            {
                bool implSigns = Boolean.Parse(isR300182Implemented);
                bool isSatisfied = ((uint)Status.NotSupported == (uint)responseStatus);
                
                
                //
                //Verify MS-APDS requirment:MS-APDS_R300182
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implSigns,
                    isSatisfied,
                    300182,
                    @"If the AlgType field of the DIGEST_VALIDATION_REQ (section 2.2.3.1) is not set to 0x03, 
                    then the DC SHOULD return SEC_E_QOP_NOT_SUPPORTED.<22>");
            }
        }


        /// <summary>
        /// Test method for validating APDS server end-point requirements for Digest Validation protocol
        /// over HTTP, DC will reject authentication request with MD5(AlgType:0x02).
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S3_TC06_DIGEST_REJECT_ALGTYPE_MD5_ASSUMED()
        {
            //authentication request with  MD5(AlgType:0x02)
            IgnoredFields ignorefFields = new IgnoredFields();
            Status responseStatus = apdsServerAdapter.GenerateDigestRequest(
                true,
                AccountInformation.Valid,
                DigestType_Values.Basic,
                AlgType_Values.NotPresentAndMD5Assumed,
                ignorefFields);

            //
            //Configured in PTFConfig file,default SHOULD to true and MAY to false.
            //
            string isR300182Implemented = "true";
            //
            //Check OS version
            //
            if (isServerWindows)
            {
                if (null == isR300182Implemented)
                {
                    Site.Properties.Add("R300182Implemented", Boolean.TrueString);
                    isR300182Implemented = Boolean.TrueString;
                }
            }
            if (null != isR300182Implemented)
            {
                bool implSigns = Boolean.Parse(isR300182Implemented);
                bool isSatisfied = ((uint)Status.NotSupported == (uint)responseStatus);

                //
                //Verify MS-APDS requirment:MS-APDS_R300182
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implSigns,
                    isSatisfied,
                    300182,
                    @"If the AlgType field of the DIGEST_VALIDATION_REQ (section 2.2.3.1) is not set to 0x03, 
                    then the DC SHOULD return SEC_E_QOP_NOT_SUPPORTED.<22>");
            }
        }
    }
}
