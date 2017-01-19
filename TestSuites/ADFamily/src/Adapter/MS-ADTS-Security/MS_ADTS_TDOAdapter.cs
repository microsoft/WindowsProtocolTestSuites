// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using ProtocolMessageStructures;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    /// <summary>
    /// MS_ADTS_TDOAdapter contains implementation of the methods defined 
    /// in IMS_ADTS_TDOAdapter adapter. The class also inherits ManagedAdapterBase
    /// Methods used for TDO operations are implemented in this class
    /// </summary>
    public class MS_ADTS_TDOAdapter : ADCommonServerAdapter, IMS_ADTS_TDOAdapter
    {

        #region GlobalVariables
        /// <summary>
        /// TDO operation sleep time
        /// </summary>
        public int sleepTime = int.Parse(TestClassBase.BaseTestSite.Properties.Get("MS_ADTS_SECURITY.TDOSleepTime"));

        /// <summary>
        /// Lsad adapter object
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static LsaClient lsadAdapterObj;

        /// <summary>
        /// Default PolicyHandle value
        /// </summary>
        private IntPtr? policyHandle = IntPtr.Zero;

        /// <summary>
        /// Default trustedDomainHandle value
        /// </summary>
        public IntPtr? trustedDomainHandle = IntPtr.Zero;

        /// <summary>
        /// Return Status value for temporary storage purposes
        /// </summary>
        public NtStatus uintMethodStatus = 0;
        /// <summary>
        /// Constant value
        /// </summary>
        public const ACCESS_MASK MAXIMUM_ALLOWED = ACCESS_MASK.MAXIMUM_ALLOWED;
        /// <summary>
        /// testDomain name
        /// </summary>
        public string testDomain = "TrustObject2";
        /// <summary>
        /// uintMethod of type uint
        /// </summary>
        public NtStatus uintMethod = 0;
        /// <summary>
        /// Default trustedHandle value
        /// </summary>
        public IntPtr? trustedHandle = IntPtr.Zero;
        /// <summary>
        /// Default strtrustDirection value
        /// </summary>
        public string strtrustDirection = string.Empty;
        /// <summary>
        /// Default userAccountControl value
        /// </summary>
        public string userAccountControl = string.Empty;
        /// <summary>
        /// Default samAccountType value
        /// </summary>
        public string samAccountType = string.Empty;
        /// <summary>
        /// checkTrust is of type bool
        /// </summary>
        public bool checkTrust;
        /// <summary>
        /// duplicateValuePassed is of type bool
        /// </summary>
        public bool duplicateValuePassed;

        /// <summary>
        /// bool variable is Collision signifies whether duplicate SID or NetBios Name exists
        /// </summary>
        public bool isCollision;

        /// <summary>
        /// OS Version for the test-pass as per test-case information (from Cord)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static Common.ServerVersion serverVer;

        /// <summary>
        /// Default Trust Direction Value
        /// </summary>
        public int trustDirection = 0;

        //Get trusting and trusted domain names.
        /// <summary>
        /// Trusted domain's name
        /// </summary>
        public string trustedDomainName;

        /// <summary>
        /// Trusted domain's netbios name
        /// </summary>
        public string trustedDomainNetBiosName;
        
        /// <summary>
        /// Trusting Domain's name
        /// </summary>
        public string trustingDomainName;

        /// <summary>
        /// LDAP Connection object
        /// </summary>
        public LdapConnection connection;

        /// <summary>
        /// Forest Functional Level for the test-pass
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static ForestFunctionalLevel forestLevelTestPass = ForestFunctionalLevel.NOT_SET;

        /// <summary>
        /// testAttrvalue 
        /// </summary>
        private string testAttrValue;

        /// <summary>
        /// currentDomainName
        /// </summary>
        private string PrimaryDomainNetBIOSName;

        /// <summary>
        /// timeout
        /// </summary>
        private TimeSpan timeout;

        /// <summary>
        /// constLsadUUID
        /// </summary>
        private const string constLsadUUID = "12345778-1234-ABCD-EF00-0123456789AB";

        /// <summary>
        /// constLsadendPoint
        /// </summary>
        private const string constLsadendPoint = "\\PIPE\\lsarpc";

        private string PdcFqdn;

        public const string TDOTestUser = "AdtsSecurityTDO";

        public const string TDOTestUserPassword = "Password01!";

        private string primaryDomainDn;
        #endregion GlobalVariables
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            PdcFqdn = PDCNetbiosName + "." + PrimaryDomainDnsName;
            lsadAdapterObj = new LsaClient();
            trustedDomainName = TrustDomainDnsName.ToUpper();
            trustedDomainNetBiosName = TrustDomainNetBiosName.ToUpper();
            connection = new LdapConnection(PdcFqdn);
            trustingDomainName = PrimaryDomainDnsName.ToUpper();
            //Get the current domain name
            PrimaryDomainNetBIOSName = PrimaryDomainNetBiosName;
            primaryDomainDn = Utilities.ParseDomainName(PrimaryDomainDnsName);
            if (!Common.Utilities.IsObjectExist(
                string.Format("CN={0},CN=Users,{1}", TDOTestUser, primaryDomainDn),
                PdcFqdn,
                ADDSPortNum,
                DomainAdministratorName,
                DomainUserPassword))
            {
                Common.Utilities.NewUser(
                        PdcFqdn,
                        ADDSPortNum,
                        "CN=Users," + primaryDomainDn,
                        TDOTestUser,
                        TDOTestUserPassword,
                        DomainAdministratorName,
                        DomainUserPassword);
            }
        }

        /// <summary>
        /// Reset
        /// </summary>
        public override void Reset()
        {
            if (lsadAdapterObj != null)
            {
                if (policyHandle != IntPtr.Zero)
                {
                    lsadAdapterObj.LsarClose(ref policyHandle);
                    policyHandle = IntPtr.Zero;
                }
                lsadAdapterObj.Dispose();
                lsadAdapterObj = null;
            }

            if (lsadAdapterObj == null)
            {
                lsadAdapterObj = new LsaClient();
            }
            base.Reset();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "connection")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (lsadAdapterObj != null)
                {
                    lsadAdapterObj.Dispose();
                    lsadAdapterObj = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// InitializeOSVersion deals with setting the OS Version for the test-pass.
        /// InitializeOSVersion is called only once during any test-pass
        /// </summary>
        /// <param name="serverVersion">OS Version for the test pass</param>
        /// <param name="forestFuncLevel">Forest Functional Level for the test pass</param>
        public void InitializeOSVersion(Common.ServerVersion serverVersion,
                                        ForestFunctionalLevel forestFuncLevel)
        {
            // Probable values for the property ForestFuncLevel are listed in TDG
            // If value passed from ptfconfig is "DS_BEHAVIOR_2003" 
            // then forestLevelTestPass should be of type ForestFunctionalLevel.DS_BEHAVIOR_WIN2003
            if (DomainFunctionLevel == Common.DomainFunctionLevel.DS_BEHAVIOR_WIN2003)
            {
                forestLevelTestPass = ForestFunctionalLevel.DS_BEHAVIOR_WIN2003;
            }
            // If value passed from ptfconfig is "HIGHER_DS_BEHAVIOR_2003" 
            // then forestLevelTestPass should be of type ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003
            else if (DomainFunctionLevel > Common.DomainFunctionLevel.DS_BEHAVIOR_WIN2003)
            {
                forestLevelTestPass = ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003;
            }
            // If value passed from ptfconfig is "LESS_DS_BEHAVIOR_2003" 
            // then forestLevelTestPass should be of type ForestFunctionalLevel.LESS_DS_BEHAVIOR_WIN2003
            else if (DomainFunctionLevel < Common.DomainFunctionLevel.DS_BEHAVIOR_WIN2003)
            {
                forestLevelTestPass = ForestFunctionalLevel.LESS_DS_BEHAVIOR_WIN2003;
            }

            timeout = TimeSpan.FromMilliseconds(Convert.ToDouble(TestClassBase.BaseTestSite.Properties["MS_ADTS_SECURITY.TDOTimeout"]));

            AccountCredential transportCredential = new AccountCredential(PrimaryDomainNetBIOSName, ClientUserName, ClientUserPassword);

            lsadAdapterObj.BindOverNamedPipe(PDCNetbiosName,
                transportCredential,
                null,
                RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE,
                timeout);
        }


        /// <summary>
        /// SetTrustedDomainObject is a top level client action which maps to
        /// LDAP/LSAD calls in the adapter, which creates and sets the attributes
        /// for a TDO Object
        /// </summary>
        /// <param name="nameAttribute">name of the trusted object</param>
        /// <param name="tdoObject">attributes of TDO are stored</param>
        /// <returns>bool</returns>
        public bool SetInformationTDO(string nameAttribute,
                                      TRUSTED_DOMAIN_OBJECT tdoObject)
        {
            if (policyHandle != null)
            {
                policyHandle = Utilities.LsarOpenPolicy2(lsadAdapterObj, PDCNetbiosName, MAXIMUM_ALLOWED);
            }
            else
            {
                policyHandle = Utilities.LsarOpenPolicy2(lsadAdapterObj, PDCNetbiosName, MAXIMUM_ALLOWED);
            }
            #region LsarCreateTrustedDomainEx2

            #region Variables
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL[] AuthenticationInformation =
                new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL[1];

            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[] domainInformation =
                new _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[1];

            _LSAPR_TRUSTED_DOMAIN_INFO? domainInfo = new _LSAPR_TRUSTED_DOMAIN_INFO();

            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[] authInformation =
                new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[1];

            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];

            IntPtr authBuffer = IntPtr.Zero;

            trustDirection = tdoObject.TrustDir.GetHashCode();

            strtrustDirection = tdoObject.TrustDir.ToString();

            #endregion Variables

            Utilities.GetTheDomainName(nameAttribute, ref domainName);

            domainName[0].Length = (ushort)(domainName[0].Buffer.Length * 2);
            domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);

            domainInformation[0].Name = new _RPC_UNICODE_STRING();
            domainInformation[0].Name = domainName[0];
            domainInformation[0].FlatName = domainName[0];
            domainInformation[0].TrustDirection = (uint)trustDirection;
            domainInformation[0].TrustType = TrustType_Values.ActiveDirectory;


            if (tdoObject.TrustDomain_Sid == "SID1")
            {
                domainInformation[0].Sid = Utilities.GetSid("TrustObject1");
            }
            else
            {
                domainInformation[0].Sid = Utilities.GetSid("TrustObject2");
            }
            #region TrustAttribute Value
            //Setting TrustAttributes value
            if (tdoObject.TrustAttr == TrustAttributesEnum.TACOAndTAWF)
            {
                domainInformation[0].TrustAttributes = (uint)TrustAttributesEnum.TRUST_ATTRIBUTE_CROSS_ORGANIZATION | (uint)TrustAttributesEnum.TRUST_ATTRIBUTE_WITHIN_FOREST;
            }
            else if (tdoObject.TrustAttr == TrustAttributesEnum.TAFTAndTAWF)
            {
                domainInformation[0].TrustAttributes = (uint)TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE | (uint)TrustAttributesEnum.TRUST_ATTRIBUTE_WITHIN_FOREST;
            }
            else if (tdoObject.TrustAttr == TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE)
            {
                domainInformation[0].TrustAttributes = (uint)TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE;
            }
            else if (tdoObject.TrustAttr == TrustAttributesEnum.TRUST_ATTRIBUTE_CROSS_ORGANIZATION)
            {
                domainInformation[0].TrustAttributes = (uint)TrustAttributesEnum.TRUST_ATTRIBUTE_CROSS_ORGANIZATION;
            }
            #endregion TrustAttribute Value

            _LSAPR_AUTH_INFORMATION[] authInfos = new _LSAPR_AUTH_INFORMATION[1];
            authInfos[0].AuthInfo = new byte[3] { 1, 2, 3 };
            authInfos[0].AuthInfoLength = (uint)(authInfos[0].AuthInfo.Length);
            authInfos[0].AuthType = AuthType_Values.V1;
            authInfos[0].LastUpdateTime = new _LARGE_INTEGER();
            authInfos[0].LastUpdateTime.QuadPart = 0xcafebeef;

            AuthenticationInformation[0].AuthBlob = LsaUtility.CreateTrustedDomainAuthorizedBlob(
                    authInfos,
                    null,
                    authInfos,
                    null,
                    lsadAdapterObj.SessionKey);


            if ((((tdoObject.TrustAttr != TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE) &&
                 (tdoObject.TrustAttr != TrustAttributesEnum.TRUST_ATTRIBUTE_CROSS_ORGANIZATION) &&
                 (tdoObject.TrustAttr != TrustAttributesEnum.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL) &&
                 (tdoObject.TrustAttr != TrustAttributesEnum.TRUST_ATTRIBUTE_UPLEVEL_ONLY)))
                  ||
                  (PDCOSVersion >= serverVer))
            {
                uintMethodStatus = lsadAdapterObj.LsarCreateTrustedDomainEx2(policyHandle.Value,
                                                                                   domainInformation[0],
                                                                                   AuthenticationInformation[0],
                                                                                   ACCESS_MASK.MAXIMUM_ALLOWED,
                                                                                   out trustedDomainHandle);
            }
            else
            {
                return false;
            }

            if ((tdoObject.TrustAttr == TrustAttributesEnum.TACOAndTAWF) ||
                (tdoObject.TrustAttr == TrustAttributesEnum.TAFTAndTAWF))
            {
                return false;
            }

            if (forestLevelTestPass == ForestFunctionalLevel.LESS_DS_BEHAVIOR_WIN2003)
            {
                #region MS-ADTS-Security_R746
                TestClassBase.BaseTestSite.CaptureRequirementIfAreNotEqual<uint>(0,
                                                                                 (uint)uintMethodStatus,
                                                                                 "MS-ADTS-Security",
                                                                                 746,
                                                                                 @"The server rejects the request, and does not 
                create the TDO if an attempt by requestor to set the TRUST_ATTRIBUTE_FOREST_TRANSITIVE bit in trust attributes 
                of the trusted domain object and if the domain is not in a forest functional level of DS_BEHAVIOR_WIN2003 or greater");
                #endregion MS-ADTS-Security_R746

                #region MS-ADTS-Security_R747
                TestClassBase.BaseTestSite.CaptureRequirementIfAreNotEqual<uint>(0,
                                                                                 (uint)uintMethodStatus,
                                                                                 "MS-ADTS-Security",
                                                                                 747,
                                                                                 @"The server rejects the request, and does not 
                create the TDO if an attempt by requestor to set the TRUST_ATTRIBUTE_FOREST_TRANSITIVE bit in trust attributes of
                the trusted domain object and if the domain server is a domain controller in the root domain of the forest.");
                #endregion MS-ADTS-Security_R747

                #region MS-ADTS-Security_R749
                TestClassBase.BaseTestSite.CaptureRequirementIfAreNotEqual<uint>(0,
                                                                                 (uint)uintMethodStatus,
                                                                                 "MS-ADTS-Security",
                                                                                 749,
                                                                                 @"The server rejects the request, and does not 
                create the TDO if an attempt by the requestor to set the TRUST_ATTRIBUTE_CROSS_ORGANIZATION bit in the trust 
                attributes of the trusted domain object and if the domain is not in a forest functional level of 
                DS_BEHAVIOR_WIN2003 or greater.");
                #endregion MS-ADTS-Security_R749
                return false;
            }

            if ((duplicateValuePassed) && (tdoObject.TrustAttr == TrustAttributesEnum.NotSet))
            {
                return false;
            }

            else if (uintMethodStatus == 0)
                duplicateValuePassed = true;

            #endregion LsarCreateTrustedDomainEx2


            if (trustedDomainHandle != IntPtr.Zero)
            {
                #region LsarQueryInfoTrustedDomain
                _TRUSTED_INFORMATION_CLASS trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal;

                /*
                * Function: LsarQueryInfoTrustedDomain
                * 
                * Description:  This method is invoked retrieve information about the trusted domain object.
                * 
                * Input Parameters:
                * a) trustedDomainHandle = An open trusted domain object handle.
                * b) trustedInfoClass = One of the TRUSTED_INFORMATION_CLASS values indicating the type of information the caller is interested in.
                * 
                * Output Parameters
                * a) domainInformation = Used to return requested information about the trusted domain object.
                * 
                * Returns: STATUS_SUCCESS when this method is executed successfully.
                */
                uintMethodStatus = lsadAdapterObj.LsarQueryInfoTrustedDomain(trustedDomainHandle.Value,
                                                                                   trustedInfoClass,
                                                                                   out domainInfo);

                ushort[] ushortFlatName = domainInfo.Value.TrustedFullInfo2.Information.FlatName.Buffer;
                string strFlatName = Utilities.ConversionfromushortArraytoString(ushortFlatName);

                #region MS-ADTS-Security_R607
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(nameAttribute,
                                                                                strFlatName,
                                                                                "MS-ADTS-Security",
                                                                                607,
                                                                                @"The flatName attribute of the trustedDomain schema
                object contains the NetBIOS name of the trusted domain in String(Unicode) syntax.");
                #endregion MS-ADTS-Security_R607

                ////Checking If the TRUST_ATTRIBUTE_FOREST_TRANSITIVE bit is cleared from a 
                ////TDO's trustAttributes attribute, all of forest trust information in the msDS-Trust on 
                ////that TDO is removed from the TDO's msDS-TrustForestTrustInfo attribute.
                if (domainInfo.Value.TrustedFullInfo2.Information.ForestTrustInfo == null)
                {
                    #region MS-ADTS-Security_R753
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0,
                                                                                  (uint)uintMethodStatus,
                                                                                  "MS-ADTS-Security",
                                                                                  753,
                                                                                  @"If the TRUST_ATTRIBUTE_FOREST_TRANSITIVE bit is 
                    cleared from a TDO's trustAttributes attribute, all of forest trust information in the msDS-Trust on that 
                    TDO is removed from the TDO's msDS-TrustForestTrustInfo attribute.");
                    #endregion MS-ADTS-Security_R753
                }

                #endregion LsarQueryInfoTrustedDomain
            }
            return true;
        }

        /// <summary>
        /// CreateTrustedAccounts is used for storing inter domain trust information.
        /// Information is required when trustDirection equals to:
        /// TRUST_DIRECTION_INBOUND or TRUST_DIRECTION_BIDIRECTIONAL 
        /// </summary>
        /// <param name="nameAttribute">name of the trusted object</param>
        /// <param name="interDomainAccInfo">for storing the interdomain trust
        /// information</param>
        public void CreateTrustedAccounts(string nameAttribute,
                                          InterDomain_Trust_Info interDomainAccInfo)
        {
            checkTrust = true;
            string expectedSamAccName = string.Empty;

            userAccountControl = interDomainAccInfo.accControl.ToString();
            samAccountType = interDomainAccInfo.interDomainAccType.ToString();
            //Parse the domain names with LDAP domain distinguished name.
            string trustedDomainDN = Utilities.ParseDomainName(trustedDomainName);
            string trustingDomainDN = Utilities.ParseDomainName(trustingDomainName);

            //The expecteed value for CN attribute of TDA object will be the NetBIOS name of the domain
            //appended with $.
            string expectedCn = trustedDomainNetBiosName + "$";


            //Preparing the DN of the TDO object.
            string tdoDistinguishedName = "CN=" + trustedDomainName + ",CN=System," + trustingDomainDN;

            //Searching for the TDO object.
            SearchRequest searchRequest = new SearchRequest(tdoDistinguishedName,
                                                            "objectclass=trustedDomain",
                                                            System.DirectoryServices.Protocols.SearchScope.Base,
                                                            new string[] { "msDs-supportedEncryption" });

            SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);


            //Getting the TDO object as entry.
            SearchResultEntry entry = searchResponse.Entries[0];

            #region Validating MS-ADTS-Security_R662
            //Preparing the Trusted Domain Account object DN.
            tdoDistinguishedName = "CN=" + trustedDomainNetBiosName
                                    + "$,CN=Users," + trustingDomainDN;

            searchRequest = new SearchRequest(tdoDistinguishedName, "objectclass=user",
                                              System.DirectoryServices.Protocols.SearchScope.Base,
                                              new string[] { "cn", "sAMAccountName", "sAMAccountType", "userAccountControl" });
            //Getting the results.
            searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

            //The expected sAMAccountName should also be like the above attribute.

            expectedSamAccName = trustedDomainNetBiosName + "$";

            //Get the sAMAccountName attribute of TDA object as actual value.
            string strsAMAccountName = searchResponse.Entries[0].Attributes["sAMAccountName"][0].ToString();

            //Capturing the requirement MS-ADTS-Security_R662
            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(expectedSamAccName.ToUpper(),
                                                                    strsAMAccountName.ToUpper(),
                                                                    "MS-ADTS-Security",
                                                                    662,
                                                                    @"The sAMAccountName attribute contains the NetBIOS name of 
            the trusted domain account appended with the character '$', in String(Unicode) syntax.");
            #endregion MS-ADTS-Security_R662

            #region Validating MS-ADTS-Security_R660
            //Getting CN value of TDA object as actual value..
            string actualCn = searchResponse.Entries[0].Attributes["cn"][0].ToString();

            //Capturing the requirement MS-ADTS-Security_R660
            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(expectedCn.ToUpper(),
                                                                            actualCn.ToUpper(),
                                                                            "MS-ADTS-Security",
                                                                            660,
                                                                            @"The RDN of an interdomain trust account, the cn attribute, 
            contains the NetBIOS name of the trusted domain account appended with the character '$', in String(Unicode) syntax.");
            #endregion

        }

        /// <summary>
        /// TrustedForestInformation is used for storing Trusted Forest Info Records
        /// when the trust attribute of the TDO is TRUST_ATTRIBUTE_FOREST_TRANSITIVE
        /// </summary>
        /// <param name="nameAttribute">name of the TDO</param>
        /// <param name="trustedForestInfo">Trust Forest Information Records</param>
        /// <returns>bool to indicate the action result</returns>
        public bool TrustedForestInformation(string nameAttribute,
                                     Trusted_Forest_Information trustedForestInfo)
        {
            _RPC_UNICODE_STRING[] domainName = new TestTools.StackSdk.Dtyp._RPC_UNICODE_STRING[1];

            _LSA_FOREST_TRUST_RECORD_TYPE recordType = new _LSA_FOREST_TRUST_RECORD_TYPE();

            if (trustedForestInfo.recType == RecordType.ForestTrustDomainInfo)
            {
                recordType = _LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelName;
            }
            else if (trustedForestInfo.recType == RecordType.ForestTrustTopLevelName)
            {
                recordType = _LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelName;
            }

            const int WRITE_MODE = 0;
            byte checkOnly = WRITE_MODE;

            _LSA_FOREST_TRUST_COLLISION_INFORMATION? collisionInformation =
                new _LSA_FOREST_TRUST_COLLISION_INFORMATION();

            _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo = new _LSA_FOREST_TRUST_INFORMATION();
            _LSA_FOREST_TRUST_INFORMATION forestTrustInfoValue = new _LSA_FOREST_TRUST_INFORMATION();

            if ((isCollision == true) && (trustedForestInfo.ndcBit == NDCFlag.Set))
            {
                Utilities.GetTheDomainName("TrustObject1", ref domainName);
            }
            else
            {
                Utilities.GetTheDomainName(nameAttribute, ref domainName);
            }
            forestTrustInfoValue.Entries = new _LSA_FOREST_TRUST_RECORD[1][];
            forestTrustInfoValue.Entries[0] = new _LSA_FOREST_TRUST_RECORD[1];
            forestTrustInfoValue.Entries[0][0] = new _LSA_FOREST_TRUST_RECORD();

            if (trustedForestInfo.sdcBit == SDCFlag.Set)
            {
                forestTrustInfoValue.Entries[0][0].Flags = (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa.Flags_Values)
                    ProtocolMessageStructures.Flags_Values.LSA_SID_DISABLED_CONFLICT;
            }
            else if (trustedForestInfo.ndcBit == NDCFlag.Set)
            {
                forestTrustInfoValue.Entries[0][0].Flags = (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa.Flags_Values)
                    ProtocolMessageStructures.Flags_Values.LSA_NB_DISABLED_CONFLICT;
            }
            domainName[0].Length = (ushort)(2 * (domainName[0].Buffer.Length));
            domainName[0].MaximumLength = (ushort)(2 + (domainName[0].Length));

            forestTrustInfoValue.Entries[0][0].ForestTrustType = recordType;
            forestTrustInfoValue.Entries[0][0].ForestTrustData.DomainInfo.DnsName = domainName[0];
            forestTrustInfoValue.Entries[0][0].ForestTrustData.DomainInfo.NetbiosName = domainName[0];
            forestTrustInfoValue.Entries[0][0].ForestTrustData.TopLevelName = domainName[0];
            if ((trustedForestInfo.sdcBit == SDCFlag.Set) && (isCollision))
                forestTrustInfoValue.Entries[0][0].ForestTrustData.DomainInfo.Sid = Utilities.GetSid("CollisionObject");
            else
                forestTrustInfoValue.Entries[0][0].ForestTrustData.DomainInfo.Sid = Utilities.GetSid(nameAttribute);

            forestTrustInfoValue.RecordCount = 0x1;
            forestTrustInfo = forestTrustInfoValue;

            uintMethodStatus = lsadAdapterObj.LsarSetForestTrustInformation(policyHandle.Value,
                                                                       domainName[0],
                                                                       recordType,
                                                                       forestTrustInfo,
                                                                       checkOnly,
                                                                       out collisionInformation);

            if ((!isCollision))
            {
                #region MS-ADTS-Security_R716
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0,
                                                                              (uint)uintMethodStatus,
                                                                              "MS-ADTS-Security",
                                                                              716,
                                                                              @"while determining namespaces collision for 
                ForestTrustDomainInfo Records ,Each SID corresponding to a domain in a trusted forest is unique among all TDOs, 
                and among all of the SIDs listed within the ForestTrustData Records.");
                #endregion MS-ADTS-Security_R716
            }
            if ((trustedForestInfo.sdcBit == SDCFlag.Set) && (isCollision))
            {

                #region MS-ADTS-Security_R717
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0,
                                                                              (uint)uintMethodStatus,
                                                                              "MS-ADTS-Security",
                                                                              717,
                                                                              @"while determining namespaces collision  for 
                ForestTrustDomainInfo Records ,if the SIDs listed within the ForestTrustData Records is not unique, 
                the Record MUST have the SDC bit in the Record Flags.");
                #endregion MS-ADTS-Security_R717

                #region MS-ADTS-Security_R719
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0,
                                                                              (uint)uintMethodStatus,
                                                                              "MS-ADTS-Security",
                                                                              719,
                                                                              @"while determining namespaces collision  for
                ForestTrustDomainInfo Records,if Each SID for each domain in a trusted forest is equal to any SIDs within the domains
                of the local forest,the Record MUST have the SDC bit in the Record Flags.");
                #endregion MS-ADTS-Security_R719
            }

            if (!(isCollision))
            {
                #region MS-ADTS-Security_R721
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0,
                                                                              (uint)uintMethodStatus,
                                                                              "MS-ADTS-Security",
                                                                              721,
                                                                              @"while determining namespaces collision  for 
                ForestTrustDomainInfo Records,if Each FQDN corresponding to a domain in a trusted forest is not unique among
                all TDOs, and among all of the FQDNs and TLNs listed within the ForestTrustData Records,the Record MUST have
                the NDC bit in the Record Flags. ");
                #endregion MS-ADTS-Security_R721
            }
            if ((trustedForestInfo.ndcBit == NDCFlag.Set) && (isCollision))
            {

                #region MS-ADTS-Security_R723
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0,
                                                                              (uint)uintMethodStatus,
                                                                              "MS-ADTS-Security",
                                                                              723,
                                                                              @"while determining namespaces collision  for 
                ForestTrustDomainInfo Records,if Each FQDN for each domain in the trusted forest does correspond to any FQDNs 
                within the domains from the local forest the Record MUST have the NDC bit in the Record Flags.");
                #endregion MS-ADTS-Security_R723

                #region MS-ADTS-Security_R725
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0,
                                                                              (uint)uintMethodStatus,
                                                                              "MS-ADTS-Security",
                                                                              725,
                                                                              @"while determining namespaces collision  for
                ForestTrustDomainInfo Records,if Each NetBIOS domain name corresponding to a domain in a trusted forest is
                not unique among all TDOs, and among all of the NetBIOS domains listed within the Forest Trust Data records,
                the Record MUST have the NDC bit in the Record Flags.");
                #endregion MS-ADTS-Security_R725

            }
            isCollision = true;
            return true;
        }


        /// <summary>
        /// TDOOperation is mapped to LDAP call which will check the TDO operations after
        /// the TDO attributes are set. The method seeks to model all the behaviors related
        /// to TDO attributes and features. TDO attributes will be changed one by one and 
        /// its behaviors are modeled and validated as per MS-ADTS TD Section: 7.1.6.
        /// Attributes of a TDO object are already stored as state variables.
        /// Assumption Enforced: Trusted Domain Object must be created prior to calling this method.
        ///                      Then the behavior is noted and validated in the implementation.
        /// In parameter nameAttribute will identify the Trusted Domain Object.
        /// </summary>
        /// <param name="nameAttribute">name attribute of the TDO</param>
        /// <returns>bool</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public bool TDOOperation(string nameAttribute)
        {
            #region Variables
            // Retreiving the test user name
            // Retreiving the password of test user

            string attrisCriticalSystemObject = string.Empty;
            string attrtrustPartner = string.Empty;
            string attrdistinguishedName = string.Empty;
            string trustingDomainDN = Utilities.ParseDomainName(PrimaryDomainDnsName);

            int unauthorizedUserEntries = 0;
            string targetOu = "CN=" + nameAttribute + ",CN=System," + trustingDomainDN;
            string ldapSearchFilter = "(&(objectClass=trustedDomain))";
            #endregion Variables

            #region LsarCreateTrustedDomainEx2

            // The LsarCreateTrustedDomainEx2 method is invoked to
            // create a trusted domain object for checking securityIdentifier attribute of the 
            // trustedDomain schema object is unique on all TDOs within the domain
            // by supplying the name, SID and authentication information for the trusted domain.
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL[] AuthenticationInformation2 =
                new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL[1];
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[] domainInformation2 =
                new _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[1];

            _RPC_UNICODE_STRING[] domainName2 = new TestTools.StackSdk.Dtyp._RPC_UNICODE_STRING[1];

            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[] authInformation2 = new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[1];

            IntPtr authBuffer = IntPtr.Zero;

            Utilities.GetTheDomainName(testDomain, ref domainName2);

            domainName2[0].Length = (ushort)(domainName2[0].Buffer.Length * 2);
            domainName2[0].MaximumLength = (ushort)(2 + domainName2[0].Length);

            domainInformation2[0].Name = new _RPC_UNICODE_STRING();
            domainInformation2[0].Name = domainName2[0];
            domainInformation2[0].FlatName = domainName2[0];
            domainInformation2[0].Sid = Utilities.GetSid(testDomain);
            domainInformation2[0].TrustAttributes = MS_ADTS_SecurityRequirementsValidator.TrustAttributes;
            domainInformation2[0].TrustDirection = (uint)trustDirection;
            domainInformation2[0].TrustType = TrustType_Values.ActiveDirectory;

            _LSAPR_AUTH_INFORMATION[] authInfos2 = new _LSAPR_AUTH_INFORMATION[1];
            authInfos2[0].AuthInfo = new byte[3] { 1, 2, 3 };
            authInfos2[0].AuthInfoLength = (uint)(authInfos2[0].AuthInfo.Length);
            authInfos2[0].AuthType = AuthType_Values.V3;
            authInfos2[0].LastUpdateTime = new TestTools.StackSdk.Dtyp._LARGE_INTEGER();
            authInfos2[0].LastUpdateTime.QuadPart = 0xcafebeef;
            AuthenticationInformation2[0].AuthBlob = LsaUtility.CreateTrustedDomainAuthorizedBlob(
                authInfos2,
                null,
                authInfos2,
                null,
                lsadAdapterObj.SessionKey);
            uintMethod = lsadAdapterObj.LsarCreateTrustedDomainEx2(policyHandle.Value,
                                                                         domainInformation2[0],
                                                                         AuthenticationInformation2[0],
                                                                         ACCESS_MASK.MAXIMUM_ALLOWED,
                                                                         out trustedHandle);
            #endregion LsarCreateTrustedDomainEx2

            //userAccountControl attribute must have the flag ADS_UF_INTERDOMAIN_TRUST_ACCOUNT (0x00000800)
            if ((checkTrust) && (userAccountControl != "ADS_UF_INTERDOMAIN_TRUST_ACCOUNT"))
                return false;

            //samAccountType attribute must have the value SAM_TRUST_ACCOUNT (0x30000002),
            if ((checkTrust) && (samAccountType != "SAM_TRUST_ACCOUNT"))
                return false;

            //Searching for TDO attributes.
            string tdoDistinguishedName = "CN=System," + trustingDomainDN;

            SearchRequest searchRequest = new SearchRequest(tdoDistinguishedName,
                                                            "objectclass=trustedDomain",
                                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                            null);

            SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                string fqdn = "CN=" + entry.Attributes["cn"][0].ToString() + ",CN=System," + trustingDomainDN;
                //Retrieve isCriticalSystemObject attribute value on a trusted domain object.
                attrisCriticalSystemObject = entry.Attributes["isCriticalSystemObject"][0].ToString();
                // TDO ?System/AD will set the value of isCriticalSystemObject. For a TDO, 
                // isCriticalSystemObject attribute value is always true. We are just querying 
                // the value of the attribute. 
                #region MS-ADTS-Security_R608
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("TRUE",
                                                                                attrisCriticalSystemObject,
                                                                                "MS-ADTS-Security",
                                                                                608,
                                                                                @"The isCriticalSystemObject attribute of 
                the trustedDomain schema object is always set to true for TDOs");
                #endregion MS-ADTS-Security_R608

                //Retrieve trustPartner attribute value on a trusted domain object.
                attrtrustPartner = entry.Attributes["trustPartner"][0].ToString();
                #region MS-ADTS-Security_R651
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(entry.Attributes["cn"][0].ToString(),
                                                                                attrtrustPartner,
                                                                                "MS-ADTS-Security",
                                                                                651,
                                                                                @"The trustPartner attribute of the trustedDomain 
                schema object dictates, it is a String(Unicode) attribute and contains the FQDN (2) of the trusted domain.");
                #endregion MS-ADTS-Security_R651

                //Retrieve distinguishedName attribute value on a trusted domain object.
                attrdistinguishedName = entry.Attributes["distinguishedName"][0].ToString();
                #region MS-ADTS-Security_R604
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(fqdn.ToLower(),
                                                                                attrdistinguishedName.ToLower(),
                                                                                "MS-ADTS-Security",
                                                                                604,
                                                                                @"TDOs are stored in the System container, 
                with a CN representing the fully qualified domain name (FQDN) (2) of the trusted domain.");
                #endregion MS-ADTS-Security_R604

                //Retrieve trustType attribute value on a trusted domain object.
                uint trustType = Convert.ToUInt32(entry.Attributes["trustType"][0]);

                //TTU (TRUST_TYPE_UPLEVEL) (0x00000002)
                #region MS-ADTS-Security_R655
                if (PDCOSVersion != Common.ServerVersion.NonWin && PDCOSVersion != Common.ServerVersion.Invalid)
                {
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(0x00000002,
                                                                                  trustType,
                                                                                  "MS-ADTS-Security",
                                                                                  655,
                                                                                  @"If the flag TTU (TRUST_TYPE_UPLEVEL) is 
                set on the trustType attribute of the trustedDomain schema object, then it indicates the trusted domain is 
                a Windows domain running Active Directory.");
                }
                #endregion MS-ADTS-Security_R655




                //Comparing securityIdentifier attribute values of the different trustedDomains.
                if (entry.Attributes["securityIdentifier"][0].ToString() == "System.Byte[]")
                {
                    testAttrValue = Encoding.ASCII.GetString((byte[])(entry.Attributes["securityIdentifier"][0]));
                }
                else
                {
                    testAttrValue = (string)entry.Attributes["securityIdentifier"][0];
                }

                foreach (SearchResultEntry single in searchResponse.Entries)
                {
                    if (single.Attributes["cn"][0].ToString() != entry.Attributes["cn"][0].ToString())
                    {
                        string sidAttrValue;
                        if (single.Attributes["securityIdentifier"][0].ToString() != "System.Byte[]")
                        {
                            sidAttrValue = (string)(single.Attributes["securityIdentifier"][0]);
                        }
                        else
                        {
                            sidAttrValue = Encoding.ASCII.GetString((byte[])(single.Attributes["securityIdentifier"][0]));
                        }
                        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                        byte[] domainSid1 = encoding.GetBytes(testAttrValue);
                        byte[] domainSid2 = encoding.GetBytes(sidAttrValue);

                        SecurityIdentifier testSid1 = new SecurityIdentifier(domainSid1, 0);
                        SecurityIdentifier testSid2 = new SecurityIdentifier(domainSid2, 0);

                        //Checking SID value of TDO having trustDirection "TRUST_DIRECTION_OUTBOUND".
                        if (strtrustDirection == "TRUST_DIRECTION_OUTBOUND")
                        {
                            if (testSid1 != null)
                            {
                                #region MS-ADTS-Security_R751
                                TestClassBase.BaseTestSite.CaptureRequirement("MS-ADTS-Security",
                                                                              751,
                                                                              @"Uplevel or downlevel trusts that have 
                                TRUST_DIRECTION_OUTBOUND as one of the direction bits cannot have a SID of NULL. ");
                                #endregion MS-ADTS-Security_R751
                            }
                        }
                    }
                }
            }

            //Checking OS used for storing TDO.
            tdoDistinguishedName = "CN=" + PDCNetbiosName + ",OU=Domain Controllers," + trustingDomainDN;




            searchRequest = new SearchRequest(tdoDistinguishedName,
                                              "objectclass=*",
                                              System.DirectoryServices.Protocols.SearchScope.Subtree,
                                              null);

            searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

            System.Threading.Thread.Sleep(sleepTime);

            searchRequest = new SearchRequest(null,
                                              "objectclass=*",
                                              System.DirectoryServices.Protocols.SearchScope.Base,
                                              null);
            searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            int domainFunctionality = Convert.ToInt32(searchResponse.Entries[0].Attributes["domainFunctionality"][0]);
            int forestFunctionality = Convert.ToInt32(searchResponse.Entries[0].Attributes["forestFunctionality"][0]);

            //Values of Identifier:
            //DS_BEHAVIOR_WIN2000 = 0; 
            //DS_BEHAVIOR_WIN2003_WITH_MIXED_DOMAINS = 1;
            //DS_BEHAVIOR_WIN2003 = 2; 
            //DS_BEHAVIOR_WIN2008 = 3;
            if ((domainFunctionality >= 2)
                 ||
                (forestFunctionality >= 2))
            {
                #region MS-ADTS-Security_R601
                TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(searchRequest,
                                                                         "MS-ADTS-Security",
                                                                         601,
                                                                         @"Building TDOs that represent cross forest trusts 
                requires that both the domain and the forest are running in a domain and forest functional level of 
                DS_BEHAVIOR_WIN2003 or greater.");
                #endregion MS-ADTS-Security_R601
            }

            //TDO default security descriptor format checking.
            tdoDistinguishedName = "CN=" + nameAttribute + ",CN=System," + trustingDomainDN;
            DirectoryEntry dirEntry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", PDCIPAddress, tdoDistinguishedName));
            ActiveDirectorySecurity adSecurity = dirEntry.ObjectSecurity;
            string SecurityDescriptor = adSecurity.GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.Access);
            #region MS-ADTS-Security_R619
            TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue((SecurityDescriptor.Contains("(A;;") && SecurityDescriptor.Contains("(OA;;")),
                                                                   "MS-ADTS-Security",
                                                                   619,
                                                                   "TDOs are Stored as the type String(NT-Sec-Desc) in SDDL");
            #endregion MS-ADTS-Security_R619

            if (PDCOSVersion == ServerVersion.Win2008)
            {
                #region MS-ADTS-Security_R759
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue((SecurityDescriptor.Contains("(A;;") && SecurityDescriptor.Contains("(OA;;")),
                                                                       "MS-ADTS-Security",
                                                                       759,
                                                                       @"The default security descriptor for TDO's in  Windows 2008 in 
                SDDL format is D:(A;;RPWPCRCCDCLCLORCWOWDSDDTSW;;;DA)(A;;RPWPCRCCDCLCLOR CWOWDSDDTSW;;;SY)
                (A;;RPLCLORC;;;AU)(OA;;WP;736e4812-af31- 11d2-b7df-00805f48caeb;bf967ab8-0de6-11d0-a285-00aa003049 e2;CO)(A;;SD;;;CO)");
                #endregion MS-ADTS-Security_R759
            }
            else if (PDCOSVersion == ServerVersion.Win2008R2)
            {
                //
                //Add the log information
                //
                Site.Log.Add(
                     LogEntryKind.Debug,
                     @"[MS-ADTS-Security_R10671] Expectedvalue={0},SecurityDescriptor={1}",
                     "(A;;",
                     SecurityDescriptor);
                //
                //Verify MS-ADTS-Security_R10671
                //
                #region MS-ADTS-Security_R10671
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    SecurityDescriptor.Contains("(A;;") && SecurityDescriptor.Contains("(OA;;") && SecurityDescriptor.Contains("AU)") &&
                    SecurityDescriptor.Contains("WP") && SecurityDescriptor.Contains("D:"),
                    "MS-ADTS-Security",
                    10671,
                    @"The default security descriptor for TDO's in  Windows 2008R2 in SDDL format is D:(A;;RPWPCRCCDCLCLORCWOWDSDDTSW;;;DA)
                (A;;RPWPCRCCDCLCLOR CWOWDSDDTSW;;;SY)(A;;RPLCLORC;;;AU)(OA;;WP;736e4812-af31- 11d2-b7df-00805f48caeb;bf967ab8-0de6-11d0-a285-00aa003049 e2;CO)
                (A;;SD;;;CO)");
                #endregion MS-ADTS-Security_R10671

            }


            //Checking for authorization access on TDO.
            tdoDistinguishedName = "CN=System," + trustingDomainDN;
            searchRequest = new SearchRequest(tdoDistinguishedName,
                                              ldapSearchFilter,
                                              System.DirectoryServices.Protocols.SearchScope.Subtree,
                                              null);

            tdoDistinguishedName = "CN=System," + trustingDomainDN;
            Utilities.RemoveAccessRights(string.Format("LDAP://{0}/{1}", PdcFqdn, tdoDistinguishedName),
                                          TDOTestUser,
                                          PrimaryDomainNetBIOSName,
                                          ClientUserName,
                                          ClientUserPassword,
                                          ActiveDirectoryRights.GenericRead,
                                          AccessControlType.Deny);
            System.Threading.Thread.Sleep(sleepTime);
            LdapConnection con = new LdapConnection(PdcFqdn);
            con.SessionOptions.ProtocolVersion = 3;
            con.AuthType = AuthType.Basic;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                TDOTestUser,
                TDOTestUserPassword,
                PrimaryDomainNetBIOSName);
            con.Bind(credentials);
            searchRequest = new SearchRequest(tdoDistinguishedName,
                                              "objectclass=container",
                                              System.DirectoryServices.Protocols.SearchScope.Base,
                                              null);
            searchResponse = null;
            searchResponse = (SearchResponse)con.SendRequest(searchRequest);
            unauthorizedUserEntries = searchResponse.Entries.Count;

            #region MS-ADTS-Security_R598
            TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse((unauthorizedUserEntries > 0),
                                                                    "MS-ADTS-Security",
                                                                    598,
                                                                    @"While inspecting authorization with respect to TDO,
                If a SID or name within the authorization data does not correspond to those claimed within the TDO, the request is rejected. ");

            #endregion MS-ADTS-Security_R598

            //Trying to retrieve unicodePwd attribute.
            try
            {
                searchRequest = new SearchRequest("CN=" + ClientUserName + ",CN=Users," + trustingDomainDN,
                                                  "objectclass=user",
                                                  System.DirectoryServices.Protocols.SearchScope.Base,
                                                  null);
                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                testAttrValue = searchResponse.Entries[0].Attributes["unicodePwd"][0].ToString();
            }
            catch (NullReferenceException unicodePwd)
            {
                #region MS-ADTS-Security_R34
                TestClassBase.BaseTestSite.CaptureRequirement("MS-ADTS-Security",
                                                              34,
                                                              "The unicodePwd attribute is never returned by an LDAP search.");
                #endregion MS-ADTS-Security_R34
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, unicodePwd.Message);
            }

            //Trying to read the trustAuthIncoming/trustAuthOutgoing attribute.
            tdoDistinguishedName = "CN=" + nameAttribute + ",CN=System," + trustingDomainDN;
            searchRequest = new SearchRequest(tdoDistinguishedName,
                                  "objectclass=*",
                                  System.DirectoryServices.Protocols.SearchScope.Base,
                                  null);
            searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            try
            {
                testAttrValue = searchResponse.Entries[0].Attributes["trustAuthIncoming"][0].ToString();
            }
            catch (NullReferenceException trustAuthIncoming)
            {
                #region MS-ADTS-Security_R10092
                TestClassBase.BaseTestSite.CaptureRequirement("MS-ADTS-Security",
                                                              10092,
                                                              "While trying to read the trustAuthIncoming  attribute,value is never returned the requestor. ");
                #endregion MS-ADTS-Security_R10076
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, trustAuthIncoming.Message);
            }
            try
            {
                testAttrValue = searchResponse.Entries[0].Attributes["trustAuthOutgoing"][0].ToString();
            }
            catch (NullReferenceException trustAuthOutgoing)
            {
                #region MS-ADTS-Security_R10093
                TestClassBase.BaseTestSite.CaptureRequirement("MS-ADTS-Security",
                                                              10093,
                                                              "While trying to read the trustAuthOutgoing  attribute,value is never returned the requestor. ");
                #endregion MS-ADTS-Security_R10077
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, trustAuthOutgoing.Message);
            }
            try
            {
                testAttrValue = searchResponse.Entries[0].Attributes["initialAuthIncoming"][0].ToString();
            }
            catch (NullReferenceException initialAuthIncoming)
            {
                #region MS-ADTS-Security_R10095
                TestClassBase.BaseTestSite.CaptureRequirement("MS-ADTS-Security",
                                                              10095,
                                                              "While trying to read the initialAuthIncoming attribute,value is never returned the requestor. ");
                #endregion MS-ADTS-Security_R10079
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, initialAuthIncoming.Message);
            }
            try
            {
                testAttrValue = searchResponse.Entries[0].Attributes["initialAuthOutgoing"][0].ToString();
            }
            catch (NullReferenceException initialAuthOutgoing)
            {
                #region MS-ADTS-Security_R10096
                TestClassBase.BaseTestSite.CaptureRequirement("MS-ADTS-Security",
                                                              10096,
                                                              "While trying to read the initialAuthOutgoing attribute,value is never returned the requestor. ");
                #endregion MS-ADTS-Security_R10080
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, initialAuthOutgoing.Message);
            }
            return true;
        }
        /// <summary>
        /// DeleteTDO is a top-level client action which is mapped to MS-LSAD
        /// call LsarDeleteTrustedDomain in adapter. Used for deleting a TDO.
        /// </summary>
        public void DeleteTDO()
        {
            checkTrust = false;
            isCollision = false;
            duplicateValuePassed = false;
            _RPC_SID[] trustSid = new _RPC_SID[1];
            trustSid = Utilities.GetSid("TrustObject1");

            /*
             * Function: LsarDeleteTrustedDomain
             * 
             * Description: This method is invoked to delete a trusted domain object.
             * Input Parameters:
             * a) PolicyHandle =An RPC context handle obtained from either LsarOpenPolicy or LsarOpenPolicy2.
             * b) trustSid = A security descriptor of the trusted domain object to be deleted.
             * 
             * 
             * Return Value: STATUS_SUCCESS when this method is executed successfully.
             */

            uintMethodStatus = lsadAdapterObj.LsarDeleteTrustedDomain(policyHandle.Value,
                                                                            trustSid[0]);

            _RPC_SID[] testSid2 = new _RPC_SID[1];
            testSid2 = Utilities.GetSid("TrustObject2");
            uintMethodStatus = lsadAdapterObj.LsarDeleteTrustedDomain(policyHandle.Value,
                                                                            testSid2[0]);
        }
    }
}