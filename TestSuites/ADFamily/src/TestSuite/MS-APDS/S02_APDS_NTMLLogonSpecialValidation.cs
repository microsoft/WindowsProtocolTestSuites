// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    /// <summary>
    /// Test suite to test the implementation for MS-APDS protocol.
    /// </summary>
    public partial class TestSuite
    {

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when invalid credentials (invalid password) are provided.
        /// and valid credentials are provided and validation level is NetlogonValidationSamInfo2.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S2_TC01_NTLM_INTERACTIVE_INVALID_PASSWORD_INFO2()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.WrongPassword,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.WrongPassword,
                responseStatus,
                137,
                @"If the local copy of password not matches with the one sent in the request, the DC MUST return the failure 
                error code STATUS_WRONG_PASSWORD with no response data<14>");

            // test cases validation
            if (isServerWindows == true)
            {
                Site.CaptureRequirementIfAreEqual<Status>(
                    Status.WrongPassword,
                    responseStatus,
                    215,
                    @"<14> Section 3.1.5.1: If the DC returns a failure code, Windows fails the logon attempt.");
            }
        }
    }
}
