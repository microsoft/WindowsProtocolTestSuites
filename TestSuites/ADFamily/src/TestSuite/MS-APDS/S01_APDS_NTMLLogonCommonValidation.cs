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
        /// when valid credentials are provided and validation level is NetlogonValidationSamInfo4.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC01_NTLM_INTERACTIVE_VALID_INFO4()
        {
           
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4);

            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);
        
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when valid credentials are provided and validation level is NetlogonValidationSamInfo4.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC02_NTLM_NETWORK_VALID_INFO4()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4);

            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);

            //
            //In this case, both of ResourceDCBlocked and AccountDCBlock are not changed, thus there are the default value.
            //If the returned status is not NTLMBlocked, we can ensure that both default value of them are false.
            //
            if (currentOS >= OSVersion.WINSVR2008R2)
            {
                // test cases validation
                Site.CaptureRequirementIfAreNotEqual<Status>(
                    Status.NTLMBlocked,
                    responseStatus,
                    297,
                    @"<4> Section 3.1.1: Default is FALSE [in AccountDCBlock parameter]. Supported in Windows Server 2008 R2.");

                // test cases validation
                Site.CaptureRequirementIfAreNotEqual<Status>(
                    Status.NTLMBlocked,
                    responseStatus,
                    298,
                    @"<5> Section 3.1.1: Default is FALSE[in ResourceDCBlock parameter]. Supported in Windows Server 2008 R2.");
            }

        }


        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when valid credentials are provided and validation level is NetlogonValidationSamInfo2.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC03_NTLM_INTERACTIVE_VALID_INFO2()
        {
           
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when valid credentials are provided and validation level is NetlogonValidationSamInfo2.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC04_NTLM_NETWORK_VALID_INFO2()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);


            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);

            //
            //In this case, both of ResourceDCBlocked and AccountDCBlock are not changed, thus there are the default value.
            //If the returned status is not NTLMBlocked, we can ensure that both default value of them are false.
            //
            if (currentOS >= OSVersion.WINSVR2008R2)
            {
                // test cases validation
                Site.CaptureRequirementIfAreNotEqual<Status>(
                    Status.NTLMBlocked,
                    responseStatus,
                    297,
                     @"<4> Section 3.1.1: Default is FALSE [in AccountDCBlock parameter]. Supported in Windows Server 2008 R2.");

                // test cases validation
                Site.CaptureRequirementIfAreNotEqual<Status>(
                    Status.NTLMBlocked,
                    responseStatus,
                    298,
                    @"<5> Section 3.1.1: Default is FALSE[in ResourceDCBlock parameter]. Supported in Windows Server 2008 R2.");
            }          
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when ResourceDC set Blocked and NTLM server's name is not included in DCBlockExceptions.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC05_NTLM_INTERACTIVE_VALIDATE_DCBLOCK_SET_RESOURCE_NOEXCEPTION()
        {
            Site.Assume.IsTrue( currentOS >= OSVersion.WINSVR2008R2,
            "This test case is only supported in Windows 2008 R2 or higher.");

            // Change the registry key which contol the DC blocker, modify the configuration in environment.
            serverControlAdapter.SetDCBlockValue(blockDCkey, resourceDCName);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            // Revert the registry key which contol the DC blocker as default value.
            serverControlAdapter.SetDCBlockValue(defaultResourceDCKey, resourceDCName);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                272,
                @"If the DC is of the resource domain, ResourceDCBlocked == TRUE, and the NTLM server's name is not 
                equal to any of the DCBlockExceptions server names, then the DC MUST return STATUS_NTLM_BLOCKED.<10>");

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                302,
                @"<10> Section 3.1.5: [If the DC is of the resource domain, ResourceDCBlocked == TRUE, and the NTLM 
                server's name is not equal to any of the DCBlockExceptions server names, then the DC MUST return 
                STATUS_NTLM_BLOCKED]Supported in Windows Server 2008 R2.");          
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when ResourceDC set Blocked and NTLM server's name is not included in DCBlockExceptions.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC06_NTLM_NETWORK_VALIDATE_DCBLOCK_SET_RESOURCE_NOEXCEPTION()
        {
            Site.Assume.IsTrue( currentOS >= OSVersion.WINSVR2008R2,
            "This test case is only supported in Windows 2008 R2 or higher.");

            // Change the registry key which contol the DC blocker, modify the configuration in environment.
            serverControlAdapter.SetDCBlockValue(blockDCkey, resourceDCName);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            // Revert the registry key which contol the DC blocker as default value.
            serverControlAdapter.SetDCBlockValue(defaultResourceDCKey, resourceDCName);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                272,
                @"If the DC is of the resource domain, ResourceDCBlocked == TRUE, and the NTLM server's name is not 
                equal to any of the DCBlockExceptions server names, then the DC MUST return STATUS_NTLM_BLOCKED.<10>");


            //
            //In this case, SUTControlAdapter set ResourceDCBlocked to true, but does not change the DCBlockExceptions, thus it is the default value.
            //If the returned status is NTLMBlocked, we can ensure that the NTLM server is not in DC's DCBlockExceptions list. It partially verifies r299.
            //                
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                299,
                @"<6> Section 3.1.1: Default is NULL[in DCBlockExceptions parameter]. Supported in Windows Server 2008 R2.");

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                302,
                @"<10> Section 3.1.5: [If the DC is of the resource domain, ResourceDCBlocked == TRUE, and the NTLM 
                server's name is not equal to any of the DCBlockExceptions server names, then the DC MUST return 
                STATUS_NTLM_BLOCKED]Supported in Windows Server 2008 R2.");         
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when AccountDC set Blocked.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC07_NTLM_INTERACTIVE_VALIDATE_DCBLOCK_SET_ACCOUNT()
        {
            Site.Assume.IsTrue( currentOS >= OSVersion.WINSVR2008R2,
            "This test case is only supported in Windows 2008 R2 or higher.");

            // Change the registry key which contol the DC blocker, modify the configuration in environment.
            serverControlAdapter.SetDCBlockValue(blockDCkey, accountDCName);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            // Revert the registry key which contol the DC blocker as default value.
            serverControlAdapter.SetDCBlockValue(defaultResourceDCKey, accountDCName);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                273,
                @"If the DC is of the account domain and AccountDCBlocked == TRUE, then the APDS server MUST 
                return STATUS_NTLM_BLOCKED.<11>");

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                263,
                @"[The DC uses the following configuration values:AccountDCBlock:]When set to TRUE, this setting disables 
                the account domain DC from responding to NTLM pass-through authentication messages. (section 3.1.5).<4>");

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                303,
                @"<11> Section 3.1.5: [If the DC is of the account domain and AccountDCBlocked == TRUE, then the APDS 
                server MUST return STATUS_NTLM_BLOCKED]Supported in Windows Server 2008 R2.");        
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when AccountDC set Blocked.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC08_NTLM_NETWORK_VALIDATE_DCBLOCK_SET_ACCOUNT()
        {
            Site.Assume.IsTrue( currentOS >= OSVersion.WINSVR2008R2,
            "This test case is only supported in Windows 2008 R2 or higher.");

            // Change the registry key which contol the DC blocker, modify the configuration in environment.
            serverControlAdapter.SetDCBlockValue(blockDCkey, accountDCName);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            // Revert the registry key which contol the DC blocker as default value.
            serverControlAdapter.SetDCBlockValue(defaultResourceDCKey, accountDCName);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                273,
                @"If the DC is of the account domain and AccountDCBlocked == TRUE, then the APDS server MUST 
                return STATUS_NTLM_BLOCKED.<11>");

            // test cases validation
            //Because of AccountDCBlocked set true,ResourceDCBlocked set false,Server get STATUS_NTLM_BLOCKED from AccountDC,
            //So AccountDC could not respond  NETLOGON_NETWORK_INFO messages to ResourceDC.
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                263,
                @"[The DC uses the following configuration values:AccountDCBlock:]When set to TRUE, this setting disables 
                the account domain DC from responding to NTLM pass-through authentication messages. (section 3.1.5).<4>");

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NTLMBlocked,
                responseStatus,
                303,
                @"<11> Section 3.1.5: [If the DC is of the account domain and AccountDCBlocked == TRUE, then the APDS 
                server MUST return STATUS_NTLM_BLOCKED]Supported in Windows Server 2008 R2.");
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when ResourceDC set Blocked and NTLM server's name is included in DCBlockExceptions.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC09_NTLM_INTERACTIVE_VALIDATE_DCBLOCK_SET_RESOURCE_EXCEPTION()
        {
            Site.Assume.IsTrue( currentOS >= OSVersion.WINSVR2008R2,
            "This test case is only supported in Windows 2008 R2 or higher.");

            // Change the registry key which contol the DC blocker, modify the configuration in environment.
            serverControlAdapter.SetDCBlockValue(blockDCkey, resourceDCName);
            serverControlAdapter.SetDCException(blockCDException, resourceDCName);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            // Revert the registry key which contol the DC blocker as default value.
            serverControlAdapter.SetDCBlockValue(defaultResourceDCKey, resourceDCName);
            serverControlAdapter.SetDCException(defaultResourceDCException, resourceDCName);

            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when ResourceDC set Blocked and NTLM server's name is included in DCBlockExceptions.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC10_NTLM_NETWORK_VALIDATE_DCBLOCK_SET_RESOURCE_EXCEPTION()
        {
            Site.Assume.IsTrue( currentOS >= OSVersion.WINSVR2008R2,
            "This test case is only supported in Windows 2008 R2 or higher.");

            // Change the registry key which contol the DC blocker, modify the configuration in environment.
            serverControlAdapter.SetDCBlockValue(blockDCkey, resourceDCName);
            serverControlAdapter.SetDCException(blockCDException, resourceDCName);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4);

            // Revert the registry key which contol the DC blocker as default value.
            serverControlAdapter.SetDCBlockValue(defaultResourceDCKey, resourceDCName);
            serverControlAdapter.SetDCException(defaultResourceDCException, resourceDCName);

            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when invalid credentials (invalid user name) are provided.
        /// and valid credentials are provided and validation level is NetlogonValidationSamInfo4.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC11_NTLM_INTERACTIVE_INVALID_USERNAME_INFO4()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.AccountNotExist,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NoSuchUser,
                responseStatus,
                123,
                @"If no matching account is found, an error STATUS_NO_SUCH_USER  MUST be returned to the 
                NTLM server, resulting in a logon failure");

            // test cases validation
            Site.CaptureRequirementIfAreNotEqual<Status>(
                Status.Success,
                responseStatus,
                276,
                @"If validation[validate the request, increment LogonAttempts] is unsuccessful, the DC MUST return an error.");

        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when invalid credentials (invalid user name) are provided
        /// and valid credentials are provided and validation level is NetlogonValidationSamInfo4.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC12_NTLM_NETWORK_INVALID_USERNAME_INFO4()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.AccountNotExist,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NoSuchUser,
                responseStatus,
                123,
                @"If no matching account is found, an error STATUS_NO_SUCH_USER  MUST be returned to the 
                NTLM server, resulting in a logon failure");

            // test cases validation
            Site.CaptureRequirementIfAreNotEqual<Status>(
                Status.Success,
                responseStatus,
                276,
                @"If validation[validate the request, increment LogonAttempts] is unsuccessful, the DC MUST return an error.");
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when invalid credentials (invalid user name) are provided.
        /// and valid credentials are provided and validation level is NetlogonValidationSamInfo2.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC13_NTLM_INTERACTIVE_INVALID_USERNAME_INFO2()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.AccountNotExist,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NoSuchUser,
                responseStatus,
                123,
                @"If no matching account is found, an error STATUS_NO_SUCH_USER  MUST be returned to the 
                NTLM server, resulting in a logon failure");

            // test cases validation
            Site.CaptureRequirementIfAreNotEqual<Status>(
                Status.Success,
                responseStatus,
                276,
                @"If validation[validate the request, increment LogonAttempts] is unsuccessful, the DC MUST return an error.");
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when invalid credentials (invalid user name) are provided
        /// and valid credentials are provided and validation level is NetlogonValidationSamInfo2.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC14_NTLM_NETWORK_INVALID_USERNAME_INFO2()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.AccountNotExist,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2);

            // test cases validation
            Site.CaptureRequirementIfAreEqual<Status>(
                Status.NoSuchUser,
                responseStatus,
                123,
                @"If no matching account is found, an error STATUS_NO_SUCH_USER  MUST be returned to the 
                NTLM server, resulting in a logon failure");

            // test cases validation
            Site.CaptureRequirementIfAreNotEqual<Status>(
                Status.Success,
                responseStatus,
                276,
                @"If validation[validate the request, increment LogonAttempts] is unsuccessful, the DC MUST return an error.");
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Interactive Logon 
        /// when valid credentials are provided and validation level is NetlogonValidationSamInfo.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC15_NTLM_INTERACTIVE_VALID_INFO()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);

            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);
        }

        /// <summary>
        /// Test method for validating APDS server end-point requirements for NTLM Network Logon 
        /// when valid credentials are provided and validation level is NetlogonValidationSamInfo.
        /// </summary>
        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC16_NTLM_NETWORK_VALID_INFO()
        {
            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);

            Site.Assert.AreEqual<Status>(Status.Success, responseStatus, "The return status must be STATUS_SUCCESS", null);

        }

        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2012R2")]
        [TestCategory("ForestWin2012R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC17_NTLM_INTERACTIVE_VALIDATE_A2AF_RESTRICTION()
        {
            Site.Assume.IsTrue( currentOS >= OSVersion.WINSVR2012R2,
            "This test case is only supported in Windows 2012 R2 or higher.");

            // sets A2AF policy to restrict the test user logon
            serverControlAdapter.SetA2AF(testUser);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            // reverts the policy setting
            serverControlAdapter.SetA2AF(null);

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"If the account is a user account object, and the corresponding msDS-UserAllowedToAuthenticateFrom 
                ([MS-ADA2] section 2.457) is populated, then the APDS MUST return STATUS_ACCOUNT_RESTRICTION.<13>"
                );

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"<13> Section 3.1.5: msDS-UserAllowedToAuthenticateFrom is not supported by Windows 2000, 
                Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012."
                );           
        }

        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2012R2")]
        [TestCategory("ForestWin2012R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC18_NTLM_NETWORK_VALIDATE_A2AF_RESTRICTION()
        {
            Site.Assume.IsTrue(currentOS >= OSVersion.WINSVR2012R2,
            "This test case is only supported in Windows 2012 R2 or higher.");

            // sets A2AF policy to restrict the test user logon
            serverControlAdapter.SetA2AF(testUser);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            // reverts the policy setting
            serverControlAdapter.SetA2AF(null);

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"If the account is a user account object, and the corresponding msDS-UserAllowedToAuthenticateFrom 
                ([MS-ADA2] section 2.457) is populated, then the APDS MUST return STATUS_ACCOUNT_RESTRICTION.<13>"
                );

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"<15> Section 3.1.5.1: msDS-UserAllowedToAuthenticateFrom is not supported by Windows 2000, 
                Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012."
                );
        }

        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2012R2")]
        [TestCategory("ForestWin2012R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC19_NTLM_INTERACTIVE_VALIDATE_A2A2_RESTRICTION()
        {
            Site.Assume.IsTrue(currentOS >= OSVersion.WINSVR2012R2,
            "This test case is only supported in Windows 2012 R2 or higher.");

            // sets A2A2 policy to restrict the test user logon
            serverControlAdapter.SetA2A2(testUser);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            // reverts the policy setting
            serverControlAdapter.SetA2A2(null);

            Site.Assert.AreEqual<Status>(
                Status.AuthenticationFirewallFailed,
                responseStatus,
                @"If AllowedToAuthenticateTo is not NULL, an access check is performed to determine whether 
                the user has the ACTRL_DS_CONTROL_ACCESS right against the AllowedToAuthenticateTo. If the 
                access check fails the APDS MUST return STATUS_AUTHENTICATION_FIREWALL_FAILED.<15>"
                );

            Site.Assert.AreEqual<Status>(
                Status.AuthenticationFirewallFailed,
                responseStatus,
                @"<15> Section 3.1.5: AllowedToAuthenticateTo is not supported by Windows 2000, Windows Server 2003, 
                Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 DCs."
                );
        }

        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2012R2")]
        [TestCategory("ForestWin2012R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC20_NTLM_NETWORK_VALIDATE_A2A2_RESTRICTION()
        {
            Site.Assume.IsTrue(currentOS >= OSVersion.WINSVR2012R2,
            "This test case is only supported in Windows 2012 R2 or higher.");

            // sets A2A2 policy to restrict the test user logon
            serverControlAdapter.SetA2A2(testUser);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            // reverts the policy setting
            serverControlAdapter.SetA2A2(null);

            Site.Assert.AreEqual<Status>(
                Status.AuthenticationFirewallFailed,
                responseStatus,
                @"If AllowedToAuthenticateTo is not NULL, an access check is performed to determine whether 
                the user has the ACTRL_DS_CONTROL_ACCESS right against the AllowedToAuthenticateTo. If the 
                access check fails the APDS MUST return STATUS_AUTHENTICATION_FIREWALL_FAILED.<15>"
                );

            Site.Assert.AreEqual<Status>(
                Status.AuthenticationFirewallFailed,
                responseStatus,
                @"<15> Section 3.1.5: AllowedToAuthenticateTo is not supported by Windows 2000, Windows Server 2003, 
                Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 DCs."
                );
        }

        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2012R2")]
        [TestCategory("ForestWin2012R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC21_NTLM_INTERACTIVE_VALIDATE_PROTECTED_USER()
        {
            Site.Assume.IsTrue(currentOS >= OSVersion.WINSVR2012R2,
            "This test case is only supported in Windows 2012 R2 or higher.");

            serverControlAdapter.SetProtectedUser(testUser);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            serverControlAdapter.SetProtectedUser(null);

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"If the user is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4), then the APDS MUST return 
                STATUS_ACCOUNT_RESTRICTION.<16>"
                );

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"<16> Section 3.1.5: PROTECTED_USERS is not supported by Windows 2000, Windows Server 2003,
                Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012."
                );
        }

        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2012R2")]
        [TestCategory("ForestWin2012R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        public void APDS_S1_TC22_NTLM_NETWORK_VALIDATE_PROTECTED_USER()
        {
            Site.Assume.IsTrue(currentOS >= OSVersion.WINSVR2012R2,
            "This test case is only supported in Windows 2012 R2 or higher.");

            serverControlAdapter.SetProtectedUser(testUser);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.Valid,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            serverControlAdapter.SetProtectedUser(null);

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"If the user is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4), then the APDS MUST return 
                STATUS_ACCOUNT_RESTRICTION.<16>"
                );

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"<16> Section 3.1.5: PROTECTED_USERS is not supported by Windows 2000, Windows Server 2003,
                Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012."
                );
        }

        [TestMethod]
        [TestCategory("MS-APDS")]
        [TestCategory("DomainWin2012R2")]
        [TestCategory("ForestWin2012R2")]
        [TestCategory("PDC")]
        public void APDS_S1_TC23_NTLM_NETWORK_VALIDATE_A2AF_RESTRICTION()
        {
            Site.Assume.IsTrue(currentOS >= OSVersion.WINSVR2012R2,
            "This test case is only supported in Windows 2012 R2 or higher.");

            // sets A2AF policy to restrict the test managed service account logon
            serverControlAdapter.SetA2AFServiceAccount(testManagedServiceAccount);

            Status responseStatus = apdsServerAdapter.NTLMLogon(
                _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                AccountInformation.ManagedServiceAccount,
                false,
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2
                );

            // reverts the policy setting
            serverControlAdapter.SetA2AFServiceAccount(null);

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"If a managed Service account object, and if the corresponding msDS-ServiceAllowedToAuthenticateFrom
                ([MS-ADA2] section 2.450) is populated and msDS-ServiceAllowedNTLMNetworkAuthentication is set to FALSE,
                APDS MUST return STATUS_ACCOUNT_RESTRICTION."
                );

            Site.Assert.AreEqual<Status>(
                Status.AccountRestriction,
                responseStatus,
                @"<16> Section 3.1.5.1: msDS-ServiceAllowedToAuthenticateFrom is not supported by Windows 2000, 
                Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012."
                );
        }
    }
}
