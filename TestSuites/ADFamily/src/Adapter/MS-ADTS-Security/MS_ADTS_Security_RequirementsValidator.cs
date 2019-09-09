// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ActiveDs;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.MAAdapter;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.MessageAnalyzer;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using ProtocolMessageStructures;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using Common = Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    /// <summary>
    /// This class validates the requirements of the MS-ADTS-Security protocol.
    /// the requirements validated are Authentication,Authorization and passwordChange operations.
    /// </summary>
    public partial class MS_ADTS_SecurityRequirementsValidator : ADCommonServerAdapter
    {
        /// <summary>
        /// Initialize information from PTF config
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            adDomain = PrimaryDomainNetBiosName;
            //get the parse string  for the domain name to use as distinguish name.
            parsedDN = ADTSHelper.ParseDomainName(PrimaryDomainDnsName);
            sleepTime = int.Parse(TestClassBase.BaseTestSite.Properties.Get("MS_ADTS_SECURITY.SleepTime"));

            if (!string.IsNullOrEmpty(ADLDSPortNum))
            {
                LDSRootObjectName = Common.Utilities.GetLdsDomainDN(string.Format("{0}.{1}:{2}", PDCNetbiosName, PrimaryDomainDnsName, ADLDSPortNum));
            }
            PdcFqdn = PDCNetbiosName + "." + PrimaryDomainDnsName;
            EndpointFqdn = ENDPOINTNetbiosName + "." + PrimaryDomainDnsName;
            EndpointSPN = "HOST/" + EndpointFqdn;

            CheckAndCreateUserInPrimaryDomain(AdminChgPwdUser, AdminChgPwdUserPassword);
            CheckAndCreateUserInPrimaryDomain(PwdChangedUser, PwdChangedUserOldPassword);
            CheckAndCreateUserInPrimaryDomain(PwdChangedInetUsername, PwdChangedInetPassword);
            CheckAndCreateUserInPrimaryDomain(NullDaclUser, NullDaclUserPassword);
            CheckAndCreateUserInPrimaryDomain(NoDaclUser, NoDaclUserPassword);
            CheckAndCreateUserInPrimaryDomain(AdminUser, AdminUserPassword);
            CheckAndCreateUserInPrimaryDomain(TempUser0, TempUser0Password);
            CheckAndCreateUserInLDS(ADLDSUser,ADLDSUserPassword);

            if (!System.IO.File.Exists(GroupCreatedSignalFile))
            {
                string entryPath = string.Format("LDAP://{0}:{1}/CN=Roles,CN=Configuration,{2}", PdcFqdn, ADLDSPortNum, LDSRootObjectName);
                DirectoryEntry de = new DirectoryEntry(entryPath, ClientUserName, ClientUserPassword);
                DirectoryEntry newGroup = de.Children.Add(string.Format("CN={0}", AdldsTestGroupName), "group");
                newGroup.CommitChanges();
                newGroup.Close();

                DirectoryEntry de1 = new DirectoryEntry(entryPath, ClientUserName, ClientUserPassword);
                DirectoryEntry removeGroup = de1.Children.Find(string.Format("CN={0}", AdldsTestGroupName), "group");
                removeGroup.DeleteTree();
                removeGroup.CommitChanges();
                removeGroup.Close();
                de1.Close();

                System.IO.StreamWriter sw = new System.IO.StreamWriter(GroupCreatedSignalFile);
                sw.WriteLine("Done");
                sw.Close();
            }

            Site.Log.Add(LogEntryKind.Debug, "Initialize Message Analyzer adapter.");
            MaAdapter = Site.GetAdapter<IMessageAnalyzerAdapter>();
        }

        /// <summary>
        /// Internal use only for creating user in LDS
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private void CheckAndCreateUserInLDS(string userName, string password)
        {
            // Check whether the user to create already exists.
            LdapConnection conn = new LdapConnection(
                new LdapDirectoryIdentifier(PdcFqdn, int.Parse(ADLDSPortNum)),
                new NetworkCredential(ClientUserName, ClientUserPassword, PrimaryDomainDnsName));
            conn.Bind();
            SearchRequest sr = new SearchRequest(
                "CN=Roles,CN=Configuration," + LDSRootObjectName,
                "(objectclass=*)",
                System.DirectoryServices.Protocols.SearchScope.OneLevel,
                "name");

            SearchResponse srep = conn.SendRequest(sr) as SearchResponse;
            foreach (SearchResultEntry entry in srep.Entries)
            {
                if(entry.DistinguishedName.Contains(ADLDSUser))
                    return; //Return and do nothing if the user exists.
            }

            Common.Utilities.NewLDSUser(
                    PdcFqdn,
                    ADLDSPortNum,
                    "CN=Roles,CN=Configuration," + LDSRootObjectName,
                    userName,
                    password,
                    PrimaryDomainDnsName + "\\" + ClientUserName,
                    ClientUserPassword);

            Common.Utilities.AddLDSGroupMember(
                PdcFqdn,
                ADLDSPortNum,
                PrimaryDomainDnsName + "\\" + ClientUserName,
                ClientUserPassword,
                string.Format("CN={0},CN=Roles,CN=Configuration,{1}", "Administrators", LDSRootObjectName),
                string.Format("CN={0},CN=Roles,CN=Configuration,{1}", userName, LDSRootObjectName));
        }

        /// <summary>
        /// Internal Use Only.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private void CheckAndCreateUserInPrimaryDomain(string userName, string password, bool addGroups = true)
        {
            string[] groups = new string[] {
                      GroupNames.DomainAdmins,
                      GroupNames.EnterpriseAdmins,
                      GroupNames.SchemaAdmins,
                      GroupNames.GroupPolicyCreatorOwners };
            // Check whether the user to create already exists.
            LdapConnection conn = new LdapConnection(
                new LdapDirectoryIdentifier(PdcFqdn, int.Parse(ADDSPortNum)), 
                new NetworkCredential(DomainAdministratorName, AdminUserPassword, PrimaryDomainDnsName));
            conn.Bind();
            SearchRequest sr = new SearchRequest(
                "CN=Users," + parsedDN,
                "(objectclass=*)",
                System.DirectoryServices.Protocols.SearchScope.OneLevel,
                "name");

            SearchResponse srep = conn.SendRequest(sr) as SearchResponse;
            foreach (SearchResultEntry entry in srep.Entries)
            {
                if (string.Equals(entry.DistinguishedName, string.Format("CN={0},CN=Users,{1}", userName, parsedDN), StringComparison.InvariantCultureIgnoreCase))
                    return; //Return and do nothing if the user exists.
            }
        
            Common.Utilities.NewUser(
                    PdcFqdn,
                    ADDSPortNum,
                    "CN=Users," + parsedDN,
                    userName,
                    password,
                    DomainAdministratorName,
                    AdminUserPassword);
            if (!addGroups) return;

            foreach (var g in groups)
            {
                Common.Utilities.AddGroupMember(
                    PdcFqdn,
                    DomainAdministratorName,
                    AdminUserPassword,
                    string.Format("CN={0},CN=Users,{1}", g, parsedDN),
                    string.Format("CN={0},CN=Users,{1}", userName, parsedDN));
            }
        }
        #region private members

        /// <summary>
        /// This variable gets the short name of Active Directory Domain used form the config file.
        /// </summary>
        private string adDomain;

        /// <summary>
        /// This variable gets the distinguished name of LDS
        /// </summary>
        private string LDSRootObjectName;

        /// <summary>
        /// this variable stores the parsed Distinguished name
        /// </summary>
        private string parsedDN = string.Empty;

        /// <summary>
        /// The FQDN of PDC
        /// </summary>
        private string PdcFqdn;

        /// <summary>
        /// The FQDN of Endpoint
        /// </summary>
        private string EndpointFqdn;

        /// <summary>
        /// This variable gets the new value for name attributes. 
        /// </summary>
        private string namevalue = "name";

        /// <summary>
        /// This variable gets the new value for servicePrincipalName attributes. 
        /// </summary>
        private string EndpointSPN;

        /// <summary>
        /// This variable specifies the Ldap connection.
        /// it is used by all the methods throughout the class
        /// </summary>
        private LdapConnection connection;

        /// <summary>
        /// This variable specifies the port number used for lds operations
        /// </summary>
        private int ldsPort = 0;

        /// <summary>
        /// denotes the constant, when a method is successfully changed.
        /// </summary>
        private const string strSuccess = "NOERROR";

        /// <summary>
        /// denotes the constant, when a method is successfully changed.
        /// </summary>
        private const string strInsufficientRights = "insufficientAccessRights";

        /// <summary>
        /// Variable defines the port number
        /// </summary>
        private Port enumPortNum;

        /// <summary>
        /// specifies tls connection is enabled.
        /// </summary>
        private bool isTlsConnection = false;

        /// <summary>
        /// Set to true if the If ctrlAccessRights bit is set to User_Change_Password
        /// </summary>
        private bool userChangePwd = false;

        /// <summary>
        /// If ctrlAccessRights bit is set to User_Force_Change_Password,
        /// then userForcePwd is set to true
        /// </summary>
        private bool userForcePwd = false;

        /// <summary>
        /// If SASL Bind is Successful
        /// </summary>
        private bool isSASLBindSuccessful = false;

        /// <summary>
        /// Location of the Client certificate that is used for External Bind
        /// </summary>
        private string certFilewithPathSpec = TestClassBase.BaseTestSite.Properties["MS_ADTS_SECURITY.certFilewithPathSpec"];

        /// <summary>
        /// password of the certificate that is used for External Bind.
        /// </summary>
        private string certPassword = TestClassBase.BaseTestSite.Properties["MS_ADTS_SECURITY.certPassword"];

        /// <summary>
        /// Instance of MessageAnalyzerAdapter
        /// </summary>
        public IMessageAnalyzerAdapter MaAdapter;

        private string AdldsTestGroupName = "TestGroupSecurity";

        private string GroupCreatedSignalFile = @"C:\TestGroupCreated";

        private int sleepTime;

        /// <summary>
        /// used as counter for Sicily Netmon requirements.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static int netmonReqValidated = 1;

        #endregion


        #region static configuration members
        /// <summary>
        /// This variable specifies the user name for AD LDS Principle. The value is "ADLDSUser".
        /// </summary>
        public const string ADLDSUser = "ADLDSTestUser";

        /// <summary>
        /// The password to the ADLDSUser. The value is "Password01!".
        /// </summary>
        public const string ADLDSUserPassword = "Password01!";

        /// <summary>
        /// This variable specifies the user name for No ACE operations. The value is "AdtsSecurityNoAce".
        /// </summary>
        public const string NoAceUser = "AdtsSecurityNoAce";

        /// <summary>
        /// The password to the NoAceUser. The value is "Password01!".
        /// </summary>
        public const string NoAceUserPassword = "Password01!";

        /// <summary>
        /// An existing User with Administrator Rights to be used in ForceChangePassword operations
        /// This user is created when the test suite is run for the first time. The value is "AdtsSecurityTest1".
        /// </summary>
        public const string AdminChgPwdUser = "AdtsSecurityTest1";

        /// <summary>
        /// The password to the user AdminChgPwdUser. The value is "Password01!".
        /// </summary>
        public const string AdminChgPwdUserPassword = "Password01!";

        /// <summary>
        /// An existing user in the Primary Domain with administrator rights and is used for Password Changing Operations.
        /// This user is created when the test suite is run for the first time. The value is "AdtsSecurityTest2".
        /// </summary>
        public const string PwdChangedUser = "AdtsSecurityTest2";

        /// <summary>
        /// The password to the user PwdChangedUser.
        /// </summary>
        public const string PwdChangedUserOldPassword = "Password01!";

        /// <summary>
        /// The password to the user PwdChangedUser.
        /// </summary>
        public const string PwdChangedUserNewPassword = "Password01!";

        public const string PwdChangedInetUsername = "AdtsSecurityTest3";

        public const string PwdChangedInetPassword = "Password01!";

        public const string NullDaclUser = "AdtsSecurityNullDacl";

        public const string NullDaclUserPassword = "Password01!";

        /// <summary>
        /// Not used
        /// </summary>
        public const string EmptyDaclUser = "AdtsSecurityEmptyDacl";

        /// <summary>
        /// Not used
        /// </summary>
        public const string EmptyDaclUserPassword = "Password01!";

        public const string NoDaclUser = "AdtsSecurityNoDacl";

        public const string NoDaclUserPassword = "Password01!";

        public const string AdminUser = "AdtsSecurityAdmin";

        public const string AdminUserPassword = "Password01!";

        public const string TempUser0 = "AdtsSecurityTemp0";

        public const string TempUser0Password = "Password01!";

        public const string TempUser1 = "AdtsSecurityTemp1";

        public const string TempUser1Password = "Password01!";

        public const string NonExistUserName = "AdtsNotExist";

        public const string InvalidPassword = "invalidADUserspassword";

        public const string ExternalBindUser = "AdtsExternalBindUser";

        public const string NameMapsMorethanOneObject = "NameMapsMorethanOneObject";

        public const UInt32 TrustAttributes = (UInt32)TrustAttributesEnum.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN;
        #endregion
        #region GUIDS

        //The following values are Constants and are defined in ADTS -TD.
        //Guid of DS-Replication-Manage-Topology Control Access Right.
        Guid guidDS_Repl_ManageE_Topology = new Guid("1131f6ac-9c07-11d1-f79f-00c04fc2dcd2");

        //Guid of RIGHT_DS_REPL_MONITOR_TOPOLOGY Control Access Right.
        Guid guidDS_Replication_Monitor_Topology = new Guid("f98340fb-7c5b-4cdb-a00b-2ebdfa115a96");

        //Guid of User_Change_Password Control Access Right.
        Guid guidUser_Change_Password = new Guid("ab721a53-1e2f-11d0-9819-00aa0040529b");

        //Guid of User_Force_Change_Password Control Access Right.
        Guid guidUser_Force_Change_Password = new Guid("00299570-246d-11d0-a768-00aa006e0529");

        //Guid of User_Force_Change_Password Control Access Right.
        Guid guidDS_Query_Self_Quota = new Guid("4ecc03fe-ffc0-4947-b630-eb672a8a9dbc");

        #endregion

        #region InitializeSession

        /// <summary>
        /// Initialize settings for validations 
        /// </summary>
        public void InitializeSession()
        {
            //Default short of Doc.
            TestClassBase.BaseTestSite.DefaultProtocolDocShortName = "MS-ADTS-Security";
        }

        #endregion

        #region Authentication

        #region SimpleBinding

        /// <summary>
        /// This method is used to authenticate the domain user and anonymous user
        /// on both regular and protected LDAP ports
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="portNo">Variable contains LDAP port number</param>
        /// <param name="userName">Variable contains name of the user of DC</param>
        /// <param name="password">Variable contains password of the user of DC</param>
        /// <param name="tls">Variable is used to determine enabling or disabling
        /// of transport layer security</param>
        /// <param name="adtestType">Indicate if the test cases is running against AD/DS or AD/LDS.</param>
        /// <returns>Returns Success if the method is successful
        /// 
        ///          Returns InvalidCredentials if the passed in credentials are invalid
        ///          
        ///          Returns unwillingToPerform if TLS is enabled over protected LDAP ports</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        public errorstatus SimpleBind(string hostName,
                                        uint portNo,
                                        string userName,
                                        string password,
                                        bool tls,
                                        ADTypes adtestType)
        {

            #region Local Variables

            //Specifies concatenated string of host name and port used for ldap connection.
            string hostNameAndLDAPPort = string.Empty;

            //Specifies credentials
            NetworkCredential credential;

            //Specifies Bind response of ldap connection bind
            //this variable is used inBind and Tls operations to assign the response
            uint bindResponse = 0;

            //Assigning the port number of the current bind
            //Assign the port number to global variable to use in Authorization and PasswordChange operations.
            enumPortNum = (Port)portNo;

            //Assign tls state , global variable to use in Authorization and PasswordChange operations.
            isTlsConnection = tls;

            #endregion

            //Concatening the port number to pass for LdapConnection
            //Connection with directory.
            hostNameAndLDAPPort = hostName + ":" + portNo;

            // LDAP connection to the server with port
            connection = new LdapConnection(hostNameAndLDAPPort);

            //Checking for valid user name and valid password
            //when Both user name and password are valid
            if ((userName == ClientUserName) &&
               (password == ClientUserPassword))
            {

                #region AD-LDS
                if (ADTypes.AD_LDS == adtestType)
                {
                    //Validate AD-LDS  Requirements
                    ValidateAdLdsRequirements(hostName, portNo, userName, password);
                }
                #endregion

                #region Regular ports

                //checking for Regular port
                //if the ports are Regular ports
                if ((portNo == (uint)Port.LDAP_PORT) || (portNo == (uint)Port.LDAP_GC_PORT))
                {

                    #region Tls

                    //Checking for the presence of tls.
                    if (tls)
                    {
                        //Bind the LdapConnection.
                        //Get the BindResponse
                        //AuthType is Basic for SimpleBind
                        bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>
                        ((uint)errorstatus.success,
                        bindResponse,
                        "Bind success");

                        //represents the tlsRepsonse
                        uint tlsResponse;

                        //if the port is Ldap Port 389
                        #region LDAP_PORT

                        if (portNo == (uint)Port.LDAP_PORT)
                        {
                            //Start tls on Regular port  389
                            //Tls can be enabled on a Basic connection with Regular port
                            tlsResponse = ADTSHelper.TLSProtection(null, connection);

                            //Validate MS-ADTS-Security_R203
                            //Check the tlsResponse is 0 or not.
                            //Tls can be enabled on a Basic connection with Regular port
                            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>((uint)errorstatus.success, tlsResponse, 203,
                                @"Active Directory permits  establishing an  SSL/TLS-protected connection 
                                to a DC on a regular LDAP port.(port numbers 389");
                        }

                        #endregion

                        #region LDAP_GC_PORT

                        if (portNo == (uint)Port.LDAP_GC_PORT)
                        {
                            //Start tls on Regular port   
                            //Tls can be enabled on a Basic connection with Regular port
                            tlsResponse = ADTSHelper.TLSProtection(null, connection);

                            //Validate MS-ADTS-Security_R205
                            //Check for Tls Response.
                            //Tls can be enabled on a Basic connection with Regular port
                            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>((uint)errorstatus.success, tlsResponse, 205,
                                @"For an AD/DS Active Directory permits  establishing an SSL/TLS-protected connection to
                                a DC on TCP ports 389 or 3268 ");
                        }

                        #endregion

                        #region validating the security prinicipal object reqs

                        //Constant value and is property
                        string samAccName = "SamAcccoutName";

                        //Validating the security principal object reqs.
                        try
                        {
                            //Value of samAccountName. 
                            //Creating  dir entry 
                            //in ConfigurationNC
                            DirectoryEntry reqConfigurationEntry = new DirectoryEntry(string.Format("LDAP://{0}/CN=Services,CN=Configuration,{1}", PdcFqdn, parsedDN));

                            //Creating new user
                            DirectoryEntry newUser = reqConfigurationEntry.Children.Add("CN=" + TempUser0, "user");

                            //Set value
                            //Set some non configure value.
                            newUser.Properties["samAccountName"].Value = samAccName;

                            newUser.CommitChanges();

                            //invoke SetPassword
                            newUser.Invoke("SetPassword", new Object[] { samAccName });

                            newUser.CommitChanges();
                            newUser.Close();
                        }

                        catch (DirectoryServicesCOMException excpObj)
                        {
                            try
                            {

                                DirectoryEntry reqConfigurationEntry = new DirectoryEntry(string.Format("LDAP://{0}/CN=Schema,CN=Configuration,{1}", PdcFqdn, parsedDN));
                                DirectoryEntry newUser = reqConfigurationEntry.Children.Add("CN=" + TempUser0, "user");
                                newUser.Properties["samAccountName"].Value = samAccName;
                                newUser.CommitChanges();
                                newUser.Invoke("SetPassword", new Object[] { samAccName });
                                newUser.CommitChanges();
                                newUser.Close();
                            }
                            //To catch Specific Exception only
                            catch (DirectoryServicesCOMException excp)
                            {
                                //Validating MS_AD-Security_R238
                                TestClassBase.BaseTestSite.CaptureRequirement(238, @"AD/DS restricts security 
                                    principals to the Domain NC.");

                                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, excp.Message);
                            }
                            TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, excpObj.Message);
                        }

                        #endregion

                        //Return Success
                        return errorstatus.success;
                    }

                    #endregion

                    #region tls not present

                    //if tls not present enable tls
                    else
                    {
                        bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>
                        ((uint)errorstatus.success,
                        bindResponse,
                        "Bind success");

                        return errorstatus.success;
                    }

                    #endregion

                }

                #endregion

                #region Protected ports and No tls

                //Checking for Protected Ports

                else if ((portNo == (uint)Port.LDAP_SSL_PORT) ||
                         (portNo == (uint)Port.LDAP_SSL_GC_PORT) && (!tls))
                {

                    //perform Simple bind connection.
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                    //Basic search operation to prove the binding.
                    SearchResponse searchResponse = SearchOperation(portNo, tls);

                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>
                        ((uint)errorstatus.success,
                        bindResponse,
                        "SearchResponse success");

                    //port no 636
                    if (portNo == (uint)Port.LDAP_SSL_PORT)
                    {
                        //perform Simple bind connection.
                        bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>
                        ((uint)errorstatus.success,
                        bindResponse,
                        "Bind success");
                    }

                    #region trying to enable tls for Security ports

                    //if tls is enabled for this connection
                    //Unwilling to perform exception should be returned when trying to enable Tls
                    if (tls)
                    {
                        //Start tls on the connection,The operation Should fail.
                        uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        //The Tls operation Should Fail on Secured ports.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                            (uint)errorstatus.unwillingToPerform,
                            tlsResponse,
                            "Unwilling to perform");

                        return errorstatus.unwillingToPerform;
                    }

                    #endregion

                    return errorstatus.success;
                }

                #endregion

                #region Protected ports and tls

                //if protected ports and tls is enable
                //tls is not possible in protected ports
                else if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT) && (tls))
                {
                    //start tls 
                    uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                    //Expecting unwillingToPerform
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                        (uint)errorstatus.unwillingToPerform,
                        tlsResponse,
                        "Unwilling to perform");

                    //for protected port if tls is enabled return unwillingToPerform 
                    return errorstatus.unwillingToPerform;
                }

                #endregion

                return errorstatus.success;
            }

        #endregion

            #region InvalidUser and Validpassword

            //Check Simple Bind for InvalidUserName and Password
            //Invalid username also represent name field is mapped to No object. 
            else if ((userName == NonExistUserName) && (password == ClientUserPassword))
            {
                //Bind LdapConnection
                //Binding with invalid credentials
                // The response must be 49.
                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                TestClassBase.BaseTestSite.Assert.AreEqual<uint>
                        ((uint)errorstatus.invalidCredentials,
                        bindResponse,
                        "Bind failed by invalid credential");

                #region trying to enable tls for Security ports

                //if tls is enabled for this connection
                //Unwilling to perform exception should be returned when trying to enable Tls
                if (tls)
                {
                    //port numbers
                    if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                    {
                        //Start tls for the ldap connection,
                        uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                            (uint)errorstatus.unwillingToPerform,
                            tlsResponse,
                            "Unwilling to perform");

                        //Returns unwillingToPerform exception
                        return errorstatus.unwillingToPerform;
                    }
                }

                #endregion

                //Returns invalidCredentials
                return errorstatus.invalidCredentials;
            }

            #endregion

            #region InvalidPassword and valid user

            // Check simple bind for InvalidPassword and valid user.
            // The result should be 0.
            else if ((password == InvalidPassword) && (userName == ClientUserName))
            {
                //Binding withInvalidPassword and valid user
                //Expected response 49
                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                    (uint)errorstatus.invalidCredentials,
                    bindResponse,
                    "Bind failed by invalid credential");

                #region trying to enable tls for Security ports

                //if tls is already enabled enabled for this connection
                //Unwilling to perform exception should be returned when trying to enable Tls
                if (tls)
                {
                    //port numbers
                    if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                    {
                        //Start tls for the ldap connection,
                        uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                            (uint)errorstatus.unwillingToPerform,
                            tlsResponse,
                            "Unwilling to perform");

                        //Returns unwillingToPerform Exception
                        return errorstatus.unwillingToPerform;
                    }
                }

                #endregion

                //Returns invalidCredentials Exception
                return errorstatus.invalidCredentials;
            }

            #endregion

            #region invalid credentials

            //Bind  using Both username and password credentials
            //Invalid credentials is expected result
            if ((userName == NonExistUserName) && (password == InvalidPassword))
            {
                //Bind with invalid credentials.
                //Expected response 49
                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                    (uint)errorstatus.invalidCredentials,
                                    bindResponse,
                                    "Bind failed by invalid credential");

                #region trying to enable tls for Security ports

                //if tls is enabled for this connection
                //Unwilling to perform exception should be returned when trying to enable Tls
                if (tls)
                {
                    //port numbers
                    if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                    {
                        //Start tls for the ldap connection,
                        uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                            (uint)errorstatus.unwillingToPerform,
                            tlsResponse,
                            "Unwilling to perform");

                        return errorstatus.unwillingToPerform;
                    }
                }

                #endregion

                //Throw invalid Credentials 
                return errorstatus.invalidCredentials;
            }

            #endregion

            #region Morethanoneusername map and valid  password

            else if ((userName == NameMapsMorethanOneObject) && (password == ClientUserPassword))
            {
                if (DirectoryEntry.Exists(string.Format("LDAP://{0}/CN={1},CN=Users,{2}", PdcFqdn, TempUser0, parsedDN)))
                {
                    #region trying to enable tls for Security ports

                    //if tls is enabled for this connection
                    //Throws UnwillingToperform Exception as already tls is enabled in the connection.
                    if (tls)
                    {
                        //port numbers
                        if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                        {
                            //Start tls for the ldap connection,
                            uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                            //expecting  unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                (uint)errorstatus.unwillingToPerform,
                                tlsResponse,
                                "Unwilling to perform");

                            return errorstatus.unwillingToPerform;
                        }
                    }

                    #endregion

                    #region Deleting TestUser

                    //Delete the test user created
                    //get the value from config file
                    connection.AuthType = AuthType.Basic;

                    credential = new NetworkCredential(
                        ClientUserName,
                        ClientUserPassword, PrimaryDomainDnsName);
                    connection.Credential = credential;
                    connection.Bind();

                    try
                    {
                        DeleteRequest delRequest = new DeleteRequest("CN=" + TempUser0 + ",CN=Users," + parsedDN);
                        DeleteResponse DelResponse = (DeleteResponse)connection.SendRequest(delRequest);
                    }
                    catch (DirectoryOperationException err)
                    {
                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, err.GetType().Name, err.Message);
                        return errorstatus.invalidCredentials;
                    }

                    #endregion Deleting TestUser

                    return errorstatus.invalidCredentials;
                }
            }

            #endregion

            #region NameMapsMorethanOne and invalidpassword

            //if namemapsmorethanone and invalidpassword 
            else if ((userName == NameMapsMorethanOneObject) && (password == InvalidPassword) || (password == ClientUserPassword))
            {
                //Bind with invalid credentials
                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);
                TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                    (uint)errorstatus.invalidCredentials,
                    bindResponse,
                    "Bind failed by invalid credential");
                #region trying to enable tls for Security ports

                //if tls is enabled for this Credentials
                if (tls)
                {
                    //port numbers
                    if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                    {
                        //start tls for protected port.
                        uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>((uint)errorstatus.unwillingToPerform, tlsResponse, "Unwilling to perform");

                        return errorstatus.unwillingToPerform;
                    }
                }

                #endregion

                //Return invalid credentials.
                return errorstatus.invalidCredentials;
            }

            #endregion

            #region AnonymousUser
            //Anonymous User and Valid password.
            //the credential for Anonymous yuser are Null values
            if ((userName == null) && ((password == null)))
            {
                //To Modify the dsHeuristic property fLdapBlockAnonOps to false
                //This enables to perform all ldap operations for anonymous user
                bool isModifySuccess = ModifyOperation(PdcFqdn, name.anonymousUser);

                if (true == isModifySuccess)
                {
                    //After fLdapBlockAnonOps to false
                    //Read operation can be performed
                    //Validating the requirements
                    ValidateAnonymousUsersRequirements();

                    //Bind LdapConnection
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                        (uint)errorstatus.success,
                        bindResponse,
                        "Bind succeed");
                    #region trying to enable tls for Security ports

                    //if tls is enabled for this connection
                    //As Tls is already enables ,it throws unwillingToPerform exception
                    if (tls)
                    {
                        //port numbers
                        if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                        {
                            //Start tls for the ldap connection,
                            uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                            //expecting  unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                (uint)errorstatus.unwillingToPerform,
                                tlsResponse,
                                "Unwilling to perform");

                            return errorstatus.unwillingToPerform;
                        }
                    }
                }
                    #endregion

                return errorstatus.success;
            }

            #endregion

            //Return Success
            return errorstatus.success;
        }

        #region FastBind
        /// <summary>
        /// This method validates the requirements of Fast Bind operation
        /// </summary>
        /// <param name="hostNameAndLDAPPort">hostname and Ldap port</param>
        /// <param name="bindResponse">response</param>
        /// <returns>returns true if all the requirements are successfully validated success</returns>
        private bool ValidateFastBindRequirements(string hostNameAndLDAPPort, uint bindResponse)
        {
            #region Fast Bind Requirements Validation.

            //Client starts the Fast bind connection
            //Enable fast bind 
            connection = LdapFastBindConnection(hostNameAndLDAPPort);

            //Value from MS-ADTS TD.
            //Non Configurable value and content.
            //3.1.1.3.4.2 LDAP Extended Operations in MS-ADTS Document
            //LDAP_SERVER_FAST_BIND_OID
            string requestName = "1.2.840.113556.1.4.1781";

            string str = "\0";

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            //Converting the request value into bytes
            byte[] requestValue = encoding.GetBytes(str);

            //FastBind can be achieved with Extended operations.
            ExtendedResponse extendedoperationResponse = ADTSHelper.ExtendedOperations(requestName,
                                                                                       requestValue,
                                                                                       connection);

            //Validate MS-ADTS-Security_R217
            TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(
                extendedoperationResponse,
                217,
                @"Fast bind mode is enabled on a LDAP connection by sending the 
                LDAP_SERVER_FAST_BIND_OID LDAP extended operation on the connection.");

            connection.AutoBind = true;

            //Basic Authentication Simple authentication can be performed 
            //in Fastbind connection.
            bindResponse = LdapBindConnection(hostNameAndLDAPPort, ClientUserName, ClientUserPassword, AuthType.Basic);

            //Validate MS-ADTS-Security_R223.
            // perform Fast Bind with AuthType other than Simple bind
            //Bind Should Fail
            //Try to Bind with other AuthType
            //create an instance of LdapConnection to specify Bind Type other than SimpleBind.
            LdapConnection nonSimpleBindConnection = new LdapConnection(new LdapDirectoryIdentifier(PdcFqdn));

            nonSimpleBindConnection.Credential = new NetworkCredential(ClientUserName, ClientUserPassword);

            //Enable Fast Bind for the connection
            int nonBasicFastBindResult = FastBindonNonBasicConnection(nonSimpleBindConnection);

            //Check whether the FastBind operation for Non Basic Ldap Connection is Succeed or not.
            TestClassBase.BaseTestSite.Assert.AreNotEqual<int>(
                (int)errorstatus.success,
                nonBasicFastBindResult,
                "Fast Bind can be enabled only on Basic Connection");

            //Enable Fast Bind for SimpleBindConnection.
            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                (uint)errorstatus.success,
                bindResponse,
                223,
                @"Only simple binds are accepted on a connection in fast bind mode.");

            //perform Simple bind ldap connection
            //Perform Bind with different credentials.
            bindResponse = LdapBindConnection(hostNameAndLDAPPort, AdminUser, AdminUserPassword, AuthType.Basic);

            //Validate MS-ADTS-Security_R222
            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                (uint)errorstatus.success,
                bindResponse,
                222,
                @"when the connection is in fast bind mode, multiple independent binds 
                (using different credentials) can simultaneously be in progress on the same 
                connection without any of them being abandoned.");

            //perform Simple bind ldap connection
            bindResponse = LdapBindConnection(hostNameAndLDAPPort, ClientUserName, ClientUserPassword, AuthType.Basic);

            //Enable FastBind on Already Established connection
            //It returns 49.
            uint fastBindResponse = EnableFastBindonLdapConnection(connection);

            //Validate MS-ADTS-Security_R219
            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                (uint)errorstatus.unwillingToPerform,
                fastBindResponse,
                219,
                @"Server returns unwillingToPerform, while trying to enable Fast 
                Bind mode on a LDAP connection,on which a successful bind was previosly performed.");

            #endregion

            return true;
        }

        #endregion

        #endregion

        #region FastBindAuthentication
        /// <summary>
        /// This method used to authenticate the domain user on regular LDAP ports.
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="portNo">Variable contains LDAP port number</param>
        /// <param name="userName">Variable contains name of the user of DC</param>
        /// <param name="passWord">password for the user</param>
        /// <param name="tls">used to indicate if tls is enabled</param>
        /// <returns>Returns Success if the method is successful
        /// 
        ///          Returns InvalidCredentials if the passed in credentials are invalid
        ///          
        ///          Returns unwillingToPerform if TLS is enabled over protected LDAP ports</returns>
        public errorstatus FastBind(string hostName,
                                    uint portNo,
                                    string userName,
                                    string passWord,
                                    bool tls)
        {
            #region local variables

            //Specifies concatenated string of host name and port used for ldap connection.
            string hostNameAndLDAPPort = string.Empty;

            //Specifies Bind response of ldap connection bind
            //this variable is used inBind and Tls operations to assign the response
            uint bindResponse = 0;

            //starttsl local variable
            uint startTLS = 0;

            //Assigning the port number of the current bind
            //Assign the port number to global variable to use in Authorization and PasswordChange operations.
            enumPortNum = (Port)portNo;

            //Assign tls state , global variable to use in Authorization and PasswordChange operations.
            isTlsConnection = tls;

            //specifies state before tls started
            uint authStateBeforeTLS = 0;

            //specifies state after tls started
            uint authStateAfterTLS = 0;

            #endregion

            //Concatening the port number to pass for LdapConnection
            //Connection with directory.
            hostNameAndLDAPPort = hostName + ":" + portNo;

            // LDAP connection to the server with port
            connection = new LdapConnection(hostNameAndLDAPPort);

            #region validusername and validpassword
            //Checking for valid user name and valid password
            //when Both user name and password are valid
            if ((userName == ClientUserName) &&
                (passWord == ClientUserPassword))
            {
                //if tls not present enable tls
                if (!tls)
                {
                    //fastBindPerformed = true if all the requirements of Fast bind in ValidateFastBindRequirements()
                    //are validated successfully.
                    bool fastBindPerformed = ValidateFastBindRequirements(hostNameAndLDAPPort, bindResponse);
                    TestClassBase.BaseTestSite.Log.Add(
                        LogEntryKind.Comment,
                        @"FastBind is completed and the status of Requirements validation is",
                        fastBindPerformed);

                    uint tlsResponse = ADTSHelper.TLSProtection(null, connection);
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>
                        ((uint)errorstatus.success, tlsResponse, "StartTLS Success");

                    return errorstatus.success;
                }
                else
                {
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, passWord, AuthType.Basic);

                    //Validate MS-ADTS-Security_R152
                    //if the Response is Success 
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                        (uint)errorstatus.success,
                        bindResponse,
                        152,
                        @"For simple Bind Active Directory does not require the use of an SSL/TLS-encrypted connection.");

                    SearchResponse tlsSearchResponse = SearchOperation(portNo, tls);
                    authStateBeforeTLS = (uint)tlsSearchResponse.ResultCode;
                    startTLS = ADTSHelper.TLSProtection(null, connection);
                    if (startTLS != 0)
                    {
                        tlsSearchResponse = SearchOperation(portNo, tls);
                        authStateAfterTLS = (uint)tlsSearchResponse.ResultCode;
                    }
                    //Validates if the status of tls is equal before and after enabling of tls.
                    //Validate MS-ADTS-Security_R213
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                        authStateAfterTLS,
                        authStateBeforeTLS,
                        213,
                        @"If the client establishes the SSL/TLS-protected 
                            connection by means of an LDAP_SERVER_START_TLS_OID operation, the authentication 
                            state of the connection remains the same after the operation as it was
                            before the operation.");

                    return errorstatus.success;
                }
            }
            #endregion

            #region Invalidusername and validpassword
            else if ((userName == NonExistUserName) && (passWord == ClientUserPassword))
            {
                LdapConnection nonbindconnection = new LdapConnection(new LdapDirectoryIdentifier(PdcFqdn));
                nonbindconnection.Credential = new NetworkCredential(ClientUserName, ClientUserPassword);
                int nobindfastbindresult = FastBindConnection(nonbindconnection);

                TestClassBase.BaseTestSite.Assert.AreEqual<int>(
                    (int)errorstatus.invalidCredentials,
                    nobindfastbindresult,
                    "Invalid Credentials");
                return errorstatus.invalidCredentials;
            }
            #endregion

            #region validusername and Invalidpassword
            else if ((userName == ClientUserName) && (passWord == InvalidPassword))
            {
                LdapConnection nonbindconnection = new LdapConnection(new LdapDirectoryIdentifier(PdcFqdn));
                nonbindconnection.Credential = new NetworkCredential(ClientUserName, ClientUserPassword);
                int nobindfastbindresult = FastBindConnection(nonbindconnection);

                TestClassBase.BaseTestSite.Assert.AreEqual<int>(
                    (int)errorstatus.invalidCredentials,
                    nobindfastbindresult,
                    "Invalid Credentials");
                return errorstatus.invalidCredentials;
            }
            #endregion

            #region Invalidusername and Invalidpassword
            if ((userName == NonExistUserName) && (passWord == InvalidPassword))
            {
                LdapConnection nonbindconnection = new LdapConnection(new LdapDirectoryIdentifier(PdcFqdn));
                nonbindconnection.Credential = new NetworkCredential(ClientUserName, ClientUserPassword);
                int nobindfastbindresult = FastBindConnection(nonbindconnection);

                TestClassBase.BaseTestSite.Assert.AreEqual<int>(
                    (int)errorstatus.invalidCredentials,
                    nobindfastbindresult,
                    "Invalid Credentials");
                return errorstatus.invalidCredentials;
            }
            #endregion

            return errorstatus.success;
        }
        #endregion

        #region SaslBind

        /// <summary>
        /// This method is used to authenticate the domain user and anonymous user
        /// on both regular and protected LDAP ports
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="portNo">Variable contains LDAP port number</param>
        /// <param name="userName">Variable contains name of the user of DC</param>
        /// <param name="password">Variable contains password of the user of DC</param>
        /// <param name="tls">Variable is used to determine enabling or disabling
        /// of transport layer security</param>
        /// <param name="authMech">Variable contains authentication mechanism</param>
        /// <param name="invalidSPN">Variable is used to determine if passed-in SPN is valid or invalid</param>
        /// <returns>Returns Success if the method is successful
        /// 
        ///          Returns InvalidCredentials if the passed in credentials are invalid
        ///          
        ///          Returns unwillingToPerform if TLS is enabled over protected LDAP ports</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public errorstatus SASLBind(string hostName,
                                        uint portNo,
                                        string userName,
                                        string password,
                                        bool tls,
                                        SASLChoice authMech,
                                        bool invalidSPN)
        {
            #region Local Variables

            //To connect host name and port 
            string hostNameAndLDAPPort = string.Empty;

            //Specifies network credential
            NetworkCredential credential;

            //Specifies the response of Ldap Bind.
            uint bindResponse = 0;

            //Status of start TSL
            uint startTSL = 0;
            
            #endregion

            //Connection with directory.
            hostNameAndLDAPPort = hostName + ":" + portNo;

            // LDAP connection to the server
            connection = (invalidSPN == true) ? new LdapConnection(PrimaryDomainDnsName) : new LdapConnection(hostNameAndLDAPPort);

            //Setting Authtpyye for the selected mechanism.
            AuthType saslAuthType;

            //Assigning the port number of the current bind
            enumPortNum = (Port)portNo;

            //Assigning the  tls state for global variable ,which can be use in other methods.
            isTlsConnection = tls;

            //SASL Bind is successful
            isSASLBindSuccessful = true;

            //Checking the authentication mechanisms
            #region Checking the authentication mechanisms

            //Bind current Credentials
            credential = new NetworkCredential(userName, password, hostName);

            //Assign the credential to ldap connection instance.
            connection.Credential = credential;

            //Bind and get the response
            bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Ntlm);

            #endregion

            if (bindResponse == (uint)errorstatus.success)
            {
                #region Valid Credentials

                //Perform SASL Bind for Valid User name and Valid password.
                if ((userName == ClientUserName) &&
                   (password == ClientUserPassword))
                {

                    #region saslgssapi

                    //if authMech is SASLChoice.saslgssapi
                    //Bind with Kerberos AuthMech
                    if (authMech == SASLChoice.saslgssapi)
                    {
                        //Auth Type == Kerberos for GSSAPI
                        saslAuthType = AuthType.Kerberos;

                        //in Windows Server 2008 Kerberos Supports only for Regular ports
                        //Kerberos will not work for protected ports in in Win2k8
                        //perform Bind only with Regular ports only
                        if ((PDCOSVersion == ServerVersion.Win2008 || PDCOSVersion == ServerVersion.Win2008R2)
                            && ((enumPortNum == Port.LDAP_SSL_GC_PORT) || (enumPortNum == Port.LDAP_SSL_PORT)))
                        {
                            //Convert to Regular ports if model sends protected port
                            hostNameAndLDAPPort = hostName + ":" + (uint)Port.LDAP_PORT;
                        }

                        //Bind 
                        //Response should be 0
                        bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, saslAuthType);
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                    (uint)errorstatus.success,
                                    bindResponse,
                                    "Bind Success");
                    }

                    #endregion

                    #region saslspnego

                    //if authMech is SASLChoice.saslspnego( GSS-SPNEGO)
                    //Bind with Ntlm AuthMech
                    if (authMech == SASLChoice.saslspnego)
                    {

                        //For GSS-SPNEGO the Auth Type can be NTLM or Kerberos.
                        //Set Auth mech is Ntlm
                        saslAuthType = AuthType.Ntlm;

                        //Bind Operation
                        //BindResponse should be 0.
                        bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, saslAuthType);

                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                                                        (uint)errorstatus.success,
                                                                        bindResponse,
                                                                        "Bind Success");

                        //invoked GSSSPNEGOBind() method to validate MS-ADTS-Security_R473 and MS-ADTS-Security_R757 specifically.
                        #region GSS-SPNEGO Specific 473 and 757

                        //GSSSPNEGOBind() method perform Bind based on the AuthType 
                        //Set AuthType to NTLM to validate MS-ADTS-Security_R468 the response should be success.
                        uint gssNtlm = GSSSPNEGOBind(userName, password, AuthType.Ntlm);

                        //Validate MS-ADTS-Security_R473
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)errorstatus.success,
                            gssNtlm,
                            473,
                            @"Active Directory supports NTLM when using GSS-SPNEGO");

                        //Set AuthType to Kerberos to validate MS-ADTS-Security_757 the response should be success.
                        uint gssKerberosBind = GSSSPNEGOBind(userName, password, AuthType.Kerberos);

                        //Validate MS-ADTS-Security_R757
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)errorstatus.success,
                            gssKerberosBind,
                            757,
                            @"Active Directory supports Kerberos when using GSS-SPNEGO");

                        #endregion
                    }

                    #endregion

                    #region External

                    //if authMech is SASLChoice.saslexternal
                    //Bind with External AuthMech
                    if (authMech == SASLChoice.saslexternal)
                    {
                        //AuthType is External
                        saslAuthType = AuthType.External;

                        //Bind Operation
                        //perform External Bind using Client Certificate(.pfx Certificate)
                        bool isExternalBind = ExternalBindUsingCertificate();

                        //Validate MS-ADTS-Security_R469
                        //if isExternalBind is true which means External Bind is performed 
                        //successfully with client certificate
                        //if the Server version is Windows version 2003 and higher
                        if (PDCOSVersion >= Common.ServerVersion.Win2003)
                        {
                            TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                               isExternalBind,
                               469,
                               @"EXTERNAL mechanism of SASL is supported in Windows Server 2003 operating system RTM and later.");
                        }

                        //Validate MS-ADTS-Security_R167
                        //if isExternalBind is true which means External Bind is performed 
                        //successfully with client certificate
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            isExternalBind,
                            167,
                            @"In SASL binds, when using the EXTERNAL SASL mechanism,Active 
                        Directory only supports the dnAuthzId form");

                        //Start Tls by using Extended Operation on Regular port
                        //perform Subsequent External Bind using client certificate
                        bool isLDAPSERVERSTARTTLSOID = ExtendedOperationToStartTLS();

                        //Validate MS-ADTS-Security_R214
                        //if isLDAPSERVERSTARTTLSOID is true means tls is started using Extended operation LDAP_SERVER_START_TLS_OID operation
                        //After that an External bind is performed successfully using client certificate.
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            isExternalBind,
                            214,
                            @"If the client
                        establishes the SSL/TLS-protected connection by means 
                        of an LDAP_SERVER_START_TLS_OID operation,The DC authenticates the connection as the 
                        credentials represented by the client's certificate ,only if an EXTERNAL SASL bind is
                        subsequently performed.");
                    }

                    #endregion

                    #region SASLDigestMD5

                    // if authMech == SASLChoice.sasldigestMD5
                    //Bind the connection Digest as AuthType.
                    if (authMech == SASLChoice.sasldigestMD5)
                    {
                        //AuthgType as Digest.
                        saslAuthType = AuthType.Digest;

                        //Bind Operation
                        //Digest Binding
                        //BindResponse should be 0.
                        bindResponse = DigestBind(userName, password);
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                                                        (uint)errorstatus.success,
                                                                        bindResponse,
                                                                        "Bind Success");
                        #region SubSequent Binding

                        //This method creates new connection with Authtype as Digest
                        //and Binds subsequently
                        //an exception will be thrown.
                        int subSequentBindResult = SubSequentDigestBind(userName, password);

                        //Validate MS-ADTS-Security_R170
                        //This requirement is validated when Sub-Sequent bind is failed after Digest Bind.
                        //The result should not be success.
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreNotEqual<int>(
                            (int)errorstatus.success,
                            subSequentBindResult,
                            170,
                            @"In SASL binds,when using the DIGEST-MD5 mechanism, Active Directory 
                        does not support subsequent authentication.");

                        #endregion
                    }

                    #endregion

                    #region Regular Ports

                    //Regular ports
                    if ((portNo == (uint)Port.LDAP_PORT) || (portNo == (uint)Port.LDAP_GC_PORT))
                    {

                        #region Validate MS-ADTS-Security_R257

                        //Create LDAP connection to act as SASL Negotiated Security layer
                        string hstNameandPrt = PdcFqdn + ":" + portNo;

                        //Create an instance of LdapConnection
                        LdapConnection saslConnection = new LdapConnection(new LdapDirectoryIdentifier(hstNameandPrt));

                        //Set Auth Type as external
                        //Now the connection acts as SASL Negotiated Security layer
                        saslConnection.AuthType = AuthType.External;

                        //Currently SASL Layer is in effect
                        //Start TLS on SASL Negotiated Layer 
                        //if TLS Started successfully then SASL negotiated security layer will not be in effect.
                        uint startTLSOnSASLLayer = ADTSHelper.TLSProtection(null, saslConnection);

                        //Expected startTLSOnSASLLayer value  = 0; since tls started successfully
                        //if tls is installed then SASL negotiated security layer is not in effect
                        //Validate MS-ADTS-Security_R257   check for success of the operation.     
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)errorstatus.success,
                            startTLSOnSASLLayer,
                            257,
                            @"If SASL negotiated security layer is in 
                        effect in the LDAP protocol data stream, it remains in effect until either a 
                        subsequently negotiated security layer (like TLS) is installed(started).");

                        #endregion

                    }

                    #endregion

                    #region Secured Ports

                    //Connection with Protected LDAPS port(636 and 3269), 
                    //then the connection is considered to be immediately authenticated (bound)
                    if (((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT)))
                    {
                        //perform ExternalBind for protected ports using client certificate
                        bool isProtectedBind = ProtectedExternalBindUsingCertificate();

                        //Validate MS-ADTS-Security_R209
                        //Validate this requirement if isProtectedBind which indicates External bind is performed 
                        //successfully on protected port.
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            isProtectedBind,
                            209,
                            @"While establishing SSL/TLS protected connection to a DC, the DC will request 
                        (but not require) the client certificate as part of SSL/TLS handshake.");

                        #region trying to enable tls for Security ports

                        //if tls is enabled for this credentials
                        //Tls cannot be enables on Secured ports
                        //UnwillingToperform will be returned. 
                        if (tls)
                        {
                            //port numbers
                            if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                            {

                                #region MS-ADTS-Security_R164

                                //Validating MS-ADTS-Security_R164 part-1
                                //While Active Directory permits SASL binds to be performed on an SSL/TLS-protected connection,
                                //So perform SASL Bind the Bind response should be Success,Bind with Kerberos which is SASL type.
                                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Kerberos);

                                //SASL Bind Success
                                TestClassBase.BaseTestSite.Log.Add(
                                    LogEntryKind.Comment,
                                    @"While Active Directory permits SASL binds to be performed on an SSL/TLS-protected 
                                connection,Validating MS-ADTS-Security_R164 The bind Response is ",
                                    bindResponse);

                                //After Successful SASL Bind  start tls for integrity mechanism or SASL-layer encryption
                                //Start tls for the Ldap connection,
                                startTSL = ADTSHelper.TLSProtection(null, connection);

                                //Validate MS-ADTS-Security_R164 part-2
                                //it does not permit the use of SASL-layer encryption integrity verification mechanisms on such a connection.
                                //Start tls for integrity mechanism or SASL-layer encryption
                                //The Result should not be 0 the the expected result is 53.
                                TestClassBase.BaseTestSite.Log.Add(
                                    LogEntryKind.Comment,
                                    @"it does not permit the use of SASL-layer encryption/integrity verification mechanisms on 
                                such a connection,Validating MS-ADTS-Security_R164 The Start tls  Response is ",
                                    startTSL);

                                //SASL Bind performed Successfully 
                                //Tls operation has failed
                                //Validate MS-ADTS-Security_R164
                                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                                    (uint)errorstatus.unwillingToPerform,
                                    startTSL,
                                    164,
                                    @"While Active Directory permits SASL binds to be performed on an SSL/TLS-
                              protected connection, it does not permit the use of SASL-layer encryption/
                              integrity verification mechanisms on such a connection.");

                                #endregion

                                //expecting  unwillingtoperform.
                                TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                    (uint)errorstatus.unwillingToPerform,
                                    startTSL,
                                    "Unwilling to perform");

                                return errorstatus.unwillingToPerform;
                            }

                        }

                        #endregion

                    }

                    #endregion

                    return errorstatus.success;
                }

                #endregion

                //Invalid credentials. for protected port
                //Bind with invalid credentials
                if (((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT)) &&
                    (userName == NonExistUserName) && (password == InvalidPassword))
                {
                    #region Trying to enable tls for Security ports

                    //if tls is enabled for this credentials
                    if (tls)
                    {
                        //port numbers
                        if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                        {
                            #region MS-ADTS-Security_R164

                            //Validating MS-ADTS-Security_R164 part-1
                            //While Active Directory permits SASL binds to be performed on an SSL/TLS-protected connection,
                            //So perform SASL Bind the Bind response should be Success ,Bind with Kerberos which is SASL type.
                            bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Kerberos);

                            //SASL Bind Success
                            TestClassBase.BaseTestSite.Log.Add(
                                LogEntryKind.Comment,
                                @"While Active Directory permits
                            SASL binds to be performed on an SSL/TLS-protected connection,
                            Validating MS-ADTS-Security_R164 The bind Response is ",
                                bindResponse);

                            //After Successful SASL Bind  start tls for integrity mechanism or SASL-layer encryption
                            //Start tls for the Ldap connection,
                            startTSL = ADTSHelper.TLSProtection(null, connection);

                            //Validate MS-ADTS-Security_R164 part-2
                            //it does not permit the use of SASL-layer encryption integrity verification mechanisms on such a connection.
                            //Start tls for integrity mechanism or SASL-layer encryption
                            //The Result should not be 0 the the expected result is 53.
                            TestClassBase.BaseTestSite.Log.Add(
                                LogEntryKind.Comment,
                                @"it does not permit the use of SASL-layer encryption/
                            integrity verification mechanisms on such a connection,
                            Validating MS-ADTS-Security_R164 The Start tls  Response is ",
                                startTSL);

                            //SASL Bind performed Successfully 
                            //Tls operation has failed
                            //Validate MS-ADTS-Security_R164
                            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                                    (uint)errorstatus.unwillingToPerform,
                                    startTSL,
                                    164,
                                    @"While Active Directory permits SASL binds to be performed on an SSL/TLS-
                              protected connection, it does not permit the use of SASL-layer encryption/
                              integrity verification mechanisms on such a connection.");

                            #endregion

                            //expecting  unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                (uint)errorstatus.unwillingToPerform,
                                startTSL,
                                "Unwilling to perform");

                            return errorstatus.unwillingToPerform;
                        }

                    }

                    #endregion

                    //invalid credentials.
                    return errorstatus.invalidCredentials;
                }
            }
            else
            {
                #region invalid credentials

                //Both username and password are invalid
                //perform Bind with invalid credentials
                if ((userName == NonExistUserName) && (password == InvalidPassword))
                {
                    #region External and Anonymous connection

                    //if the Authtype is External
                    if (authMech == SASLChoice.saslexternal)
                    {
                        //Perform External binding in ssl
                        //invalidcredentials error will be returned 
                        //pass null credentials as to validate Anonymous connection.
                        bindResponse = ExternalBindConnection(adDomain, userName, password, AuthType.External);

                        //if the response is invalid credentials then
                        //Validate MS-ADTS-Security_R216
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)errorstatus.invalidCredentials,
                            bindResponse,
                            216,
                            @"If the connection is anonymous during SSL/TLS handshake,the DC rejects any attempt
                        to perform an EXTERNAL bind with the error invalidCredentials");

                        //validate MD_ADTS_Security_R168
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)errorstatus.invalidCredentials,
                            bindResponse,
                            168,
                            @"In SASL binds, when using the EXTERNAL SASL mechanism, if the 
                        Active Directory supports the  the uAuthzId form of authzId field.
                        then DC returns invalidCredentials error.");
                    }

                    #endregion

                    //Bind LdapConnection with the credentials
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                    //expecting invalidCredentials
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                                                    (uint)errorstatus.invalidCredentials,
                                                                    bindResponse,
                                                                    "Bind failed with invalidCredentials");

                    #region Trying to enable tls for Security ports

                    //if Tls is enabled for this credentials
                    if (tls)
                    {
                        //port numbers
                        if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                        {
                            //Start tls for the ldap connection,
                            bindResponse = ADTSHelper.TLSProtection(null, connection);

                            //expecting unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                (uint)errorstatus.unwillingToPerform,
                                bindResponse,
                                "Unwilling to perform");

                            //return unwillingToPerform
                            return errorstatus.unwillingToPerform;
                        }
                    }

                    #endregion

                    //return invalidCredentials
                    return errorstatus.invalidCredentials;
                }

                #endregion

                #region InvalidUserName  and valid Password

                //InvalidUserName  and valid Password 
                //Bind with invalid credentials.
                if ((userName == NonExistUserName) &&
                    (password == ClientUserPassword))
                {
                    //Bind LdapConnection with the credentials 
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                    //expecting invalidCredentials
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                                (uint)errorstatus.invalidCredentials,
                                                bindResponse,
                                                "Bind failed with invalidCredentials");

                    #region trying to enable tls for Security ports

                    //if tls is enabled for this credentials
                    if (tls)
                    {
                        //port numbers
                        //Secured ports
                        if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                        {
                            //Start tls for the ldap connection,
                            bindResponse = ADTSHelper.TLSProtection(null, connection);

                            //expecting  unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>((uint)errorstatus.unwillingToPerform, bindResponse, "Unwilling to perform");

                            // return unwillingtoperform
                            return errorstatus.unwillingToPerform;
                        }
                    }

                    #endregion

                    //return invalidCredentials 
                    return errorstatus.invalidCredentials;
                }

                #endregion

                #region Valid User and Invalid Password

                //Valid User and invalid password
                if ((userName == ClientUserName) && (password == InvalidPassword))
                {

                    //Bind LdapConnection with the credentials
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Basic);

                    //excepting invalidCredentials
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                                (uint)errorstatus.invalidCredentials,
                                                bindResponse,
                                                "Bind failed with invalidCredentials");
                    #region trying to enable tls for Security ports

                    //if tls is enabled for this credentials
                    if (tls)
                    {
                        //Start tls for the ldap connection,
                        bindResponse = ADTSHelper.TLSProtection(null, connection);

                        //port numbers
                        if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                        {
                            //expecting  unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                (uint)errorstatus.unwillingToPerform,
                                bindResponse,
                                "Unwilling to perform");

                            //return unwillingToPerform
                            return errorstatus.unwillingToPerform;
                        }
                    }

                    #endregion

                    //return invalidCredentials
                    return errorstatus.invalidCredentials;
                }

                #endregion
            }
            // return invalidCredentials

            return errorstatus.success;
        }

        #endregion

        #region MutualBinding

        /// <summary>
        /// This method validates the requirements of Mutual binding
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <param name="validSPN">specifies whether spn is true or false</param>
        /// <returns>returns status of the mutual binding</returns>
        public errorstatus MutualBind(string userName, string password, bool validSPN)
        {

            //Specifies the bindResponse
            uint bindResponse;

            //Valid User
            if ((userName == ClientUserName) &&
                (password == ClientUserPassword))
            {
                //if validSPN is false
                if (!validSPN)
                {
                    //Mutual Authentication
                    bindResponse = MutualBind(ClientUserName, ClientUserPassword);

                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                        (uint)errorstatus.spnUnknown,
                        bindResponse,
                        "Mutual Bind Failed by unknown SPN");

                    //return invalid credentials.
                    return errorstatus.failure;

                }
                //valid spn 
                else
                {
                    //Bind connection
                    bindResponse = LdapBindConnection(PrimaryDomainDnsName, userName, password, AuthType.Kerberos);

                    //Check result.
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>((uint)errorstatus.success, bindResponse, "Mutual Bind");

                    //return success
                    return errorstatus.success;
                }

            }

            else
            {
                //Mutual Bind 
                //Mutual Bind is Kerberos
                bindResponse = LdapBindConnection(PrimaryDomainDnsName, userName, password, AuthType.Kerberos);

                //check whether the result is invalid credentials or not
                TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                    (uint)errorstatus.invalidCredentials,
                    bindResponse,
                    "Mutual Bind Failed");

                //mask the spunknown
                return errorstatus.failure;
            }

        }

        #endregion

        #region SicilyBind

        /// <summary>
        /// This method is used to authenticate the domain user and anonymous user
        /// on both regular and protected LDAP ports
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="portNo">Variable contains LDAP port number</param>
        /// <param name="userName">Variable contains name of the user of DC</param>
        /// <param name="password">Variable contains password of the user of DC</param>
        /// <param name="tls">Variable is used to determine enabling or disabling
        /// of transport layer security</param>
        /// <returns>Returns Success if the method is successful
        /// 
        ///          Returns InvalidCredentials if the passed in credentials are invalid
        ///          
        ///          Returns unwillingToPerform if TLS is enabled over protected LDAP ports</returns>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public errorstatus SicilyBind(string hostName,
                                        uint portNo,
                                        string userName,
                                        string password,
                                        bool tls)
        {
            #region Local Variables

            //Specifies concatenated string of host name and port used for ldap connection
            string hostNameAndLDAPPort = string.Empty;

            //Specifies Bind response of ldap connection bind
            uint bindResponse = 0;

            //startsl local variable
            uint startTSL = 0;

            //Assigning the port number of the current bind
            enumPortNum = (Port)portNo;

            //Assigning tls state
            isTlsConnection = tls;

            #endregion

            //Concatening the port number to pass for LdapConnection
            //Connection with directory.
            hostNameAndLDAPPort = hostName + ":" + portNo;

            //LDAP connection to the server with port
            connection = new LdapConnection(hostNameAndLDAPPort);

            #region Valid Credentials

            //Checking for valid user name and valid password
            if ((userName == ClientUserName) &&
               (password == ClientUserPassword))
            {

                #region Regular ports

                //checking for Regular port
                if ((portNo == (uint)Port.LDAP_PORT) || (portNo == (uint)Port.LDAP_GC_PORT))
                {
                    //Bind the LdapConnection.
                    //perform Sicily Bind
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Sicily);

                    //perform RootDSE Search operation
                    SearchResponse searchResponse = ADTSHelper.RootDSESearchOperation(portNo, false);

                    //Array of capability oids 
                    string[] capabilityOID = new string[3];

                    //Constant taken fromADTS Technical Document
                    string LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID = "1.2.840.113556.1.4.1791";

                    //oidcount
                    int oidCount = 0;

                    //Enumerate the search Response
                    for (int count = 0; count < 3; count++)
                    {
                        capabilityOID[oidCount] = (string)(searchResponse.Entries[0].Attributes["supportedCapabilities"][count]);
                        oidCount++;
                    }

                    //perform Binary Search.
                    if (Array.BinarySearch(capabilityOID, LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID) >= 0)
                    {

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)errorstatus.success,
                            bindResponse,
                            199,
                            @"Once such a mechanism(Sicily integrity verification or encryption) is in use, the 
                            connection cannot be rebound unless the LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID capability 
                            is present in the supportedCapabilities attribute of the rootDSE of the DC.");
                    }

                    //Validate the following requirements using Message Analyzer
                    //System.DirectoryServices.Protocols cannot make specific calls to SicilyPakageDiscovery and SicilyNegotiate
                    //AuthType.Bind = Sicily will be used for SicilyPakageDiscovery and SicilyNegotiate in Bind operation
                    if (netmonReqValidated == 1)
                    {
                        System.Threading.Thread.Sleep(sleepTime);

                        //increasing the counter
                        netmonReqValidated++;

                        #region   Checking with Message Analyzer                     

                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "Start capturing using Message Analyzer.");
                        MaAdapter.Reset();
                        MaAdapter.StartCapture(capturePath_SecuritySicily, "LDAP");

                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "Perform Sicily Bind.");
                        SicilyBindConnection(hostNameAndLDAPPort, userName, password);

                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "Stop capturing.");
                        MaAdapter.StopCapture();

                        string filter = "(LDAP.ProtocolOp.ServerCreds == \"NTLM\") and (LDAP.ProtocolOp.ResultCode == LDAP.ResultCode.Success)";
                        List<Message> messages = MaAdapter.GetMessages(capturePath_SecuritySicily, filter);
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            messages.Count > 0,
                            174,
                            @"In sicily authentication, If the sicilyPackageDiscovery request is successful,
                                                    the DC sets the resultCode to success in its SicilyBindResponse.");
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            messages.Count > 0,
                            184,
                            @"In sicily authentication mechanism, when the client sends the sicilyNegotiate request to the DC, 
                                                    If successful, the DC responds with a SicilyBindResponse in which the resultCode is set to success.");
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            messages.Count > 0,
                            171,
                            @"In sicily authentication, All Versions of Active Directory expose and 
                                                    support only the NTLM authentication protocol, via Sicily");
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            messages.Count > 0,
                            175,
                            @"In sicily authentication,If the sicilyPackageDiscovery request is successful, the DC returns 
                            in serverCreds an ANSI string consisting of the semicolon-separated names of the authentication
                            protocols it supports via the Sicily authentication mechanism.");
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            messages.Count > 0,
                            176,
                            @"Active Directory supports NTLM, and returns the string NTLM in the package 
                            discovery response when the sicilyPackageDiscovery request is successful.");
                        
                        #endregion
                    }

                    #region Tls

                    //Checking for the presence of tls.
                    if (tls)
                    {
                        //represents the tlsRepsonse
                        uint tlsResponse;
                        if (portNo == (uint)Port.LDAP_PORT)
                        {
                            //Start tls on Regular port  
                            tlsResponse = ADTSHelper.TLSProtection(null, connection);
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                                                      (uint)errorstatus.success,
                                                                       tlsResponse,
                                                                       "startTLS success");
                        }

                        if (portNo == (uint)Port.LDAP_GC_PORT)
                        {
                            //Start tls on Regular port   
                            tlsResponse = ADTSHelper.TLSProtection(null, connection);
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                          (uint)errorstatus.success,
                                           tlsResponse,
                                           "startTLS success");
                        }

                        if (startTSL != 0)
                        {
                            //enabled tls again to get unwillingToPerform exception
                            startTSL = ADTSHelper.TLSProtection(null, connection);

                            //expecting  unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                (uint)errorstatus.unwillingToPerform,
                                startTSL,
                                "Unwilling to perform");

                            //Unwilling to perform
                            return errorstatus.unwillingToPerform;
                        }

                        return errorstatus.success;

                    }

                    #endregion

                }

                #endregion

                #region Protected ports and No tls

                //Checking for Protected
                else if ((portNo == (uint)Port.LDAP_SSL_PORT) ||
                         (portNo == (uint)Port.LDAP_SSL_GC_PORT) && (!tls))
                {

                    //perform Simple bind connection.
                    bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Sicily);

                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>
                                            ((uint)errorstatus.success,
                                            bindResponse,
                                            "Bind success");

                    //Basic search operation to prove the binding.
                    SearchResponse searchResponse = SearchOperation(portNo, tls);

                    #region Trying to enable tls for Secured ports

                    //if tls is enabled for this credentials 
                    if (tls)
                    {
                        //Start tls for the ldap connection,
                        bindResponse = ADTSHelper.TLSProtection(null, connection);

                        //Secured port numbers
                        //the connection is binded with secured ports.
                        if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                        {
                            //expecting  unwillingtoperform.
                            TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                                (uint)errorstatus.unwillingToPerform,
                                bindResponse,
                                "Unwilling to perform");

                            return errorstatus.unwillingToPerform;
                        }

                    }

                    #endregion

                    return errorstatus.success;
                }

                #endregion

                #region Protected ports and tls

                //if protected ports and tls is enable
                //tls is not possible in protected ports
                else if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT) && (tls))
                {
                    //start tls 
                    uint tlsResponse = ADTSHelper.TLSProtection(null, connection);

                    //Expecting unwillingToPerform
                    TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                        (uint)errorstatus.unwillingToPerform,
                        tlsResponse,
                        "Unwilling to perform");

                    //for protected port if tls is enabled return unwillingToPerform 
                    return errorstatus.unwillingToPerform;
                }

                #endregion

                return errorstatus.success;
            }

            #endregion

            #region InvalidUser and Validpassword

            // InvalidUserName and Password
            // Perform Sicily bind
            // Result should be 49.
            else if ((userName == NonExistUserName) && (password == ClientUserPassword))
            {
                //Bind LdapConnection
                //Binding with invalid credentials
                // The response must be 49.
                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Sicily);

                //Validate  MS-ADTS-Security_R187
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)errorstatus.invalidCredentials,
                    bindResponse,
                    187,
                    @"When the client sends the sicilyNegotiate request to the DC, If the credentials supplied by
                    the client are invalid, the DC returns the invalidCredentials error");

                #region trying to enable tls for Security ports

                //if tls is enabled for this credentials
                //tls cannot be possible
                if (tls)
                {
                    //port numbers
                    if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                    {
                        //Start tls for the ldap connection,
                        bindResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                            (uint)errorstatus.unwillingToPerform,
                            bindResponse,
                            "Unwilling to perform");

                        return errorstatus.unwillingToPerform;
                    }
                }

                #endregion

                return errorstatus.invalidCredentials;
            }

            #endregion

            #region InvalidPassword and valid user

            // InvalidPassword and valid user
            // Perform Sicily Bind
            // Result should be 49
            // Getting the values from Config file.
            else if ((password == InvalidPassword) && (userName == ClientUserName))
            {
                //Binding withInvalidPassword and valid user
                //Expected response 49
                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Sicily);

                //Validate  MS-ADTS-Security_R187
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)errorstatus.invalidCredentials,
                    bindResponse,
                    187,
                    @"When the client sends the sicilyNegotiate request to the DC, If the credentials supplied by
                    the client are invalid, the DC returns the invalidCredentials error");

                #region Trying to enable tls for Secured ports

                //if tls is enabled for this credentials
                //unwillingToPerform should be returned.
                if (tls)
                {
                    //port numbers
                    if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                    {
                        //Start tls for the ldap connection,
                        bindResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                            (uint)errorstatus.unwillingToPerform,
                            bindResponse,
                            "Unwilling to perform");

                        return errorstatus.unwillingToPerform;
                    }
                }

                #endregion

                return errorstatus.invalidCredentials;
            }

            #endregion

            #region invalid credentials

            //Both username and password are invalid
            //Perform SicilyBind and the response should be 49
            if ((userName == NonExistUserName) && (password == InvalidPassword))
            {
                //Bind with invalid credentials.
                //Expected resultcode 49
                bindResponse = LdapBindConnection(hostNameAndLDAPPort, userName, password, AuthType.Sicily);

                //Validate  MS-ADTS-Security_R194
                //Set 49 as resultcode for invalid credentials
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)errorstatus.invalidCredentials,
                    bindResponse,
                    194,
                    @"If SicilyBindResponse message fails, the DC sets resultCode
                    to an error and the connection is not authenticated. ");

                //Validate  MS-ADTS-Security_R195
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)errorstatus.invalidCredentials,
                    bindResponse,
                    195,
                    @"If SicilyBindResponse message fails, the DC sets resultCode field to an error  
                    invalidCredentials that indicates the client presented incorrect credentials.");

                #region trying to enable tls for Security ports

                //if tls is enabled for this credentials
                if (tls)
                {
                    //port numbers
                    //Tls cann ot be enabled on Secured ports
                    if ((portNo == (uint)Port.LDAP_SSL_PORT) || (portNo == (uint)Port.LDAP_SSL_GC_PORT))
                    {
                        //Start tls for the ldap connection,
                        bindResponse = ADTSHelper.TLSProtection(null, connection);

                        //expecting  unwillingtoperform.
                        TestClassBase.BaseTestSite.Assert.AreEqual<uint>(
                            (uint)errorstatus.unwillingToPerform,
                            bindResponse,
                            "Unwilling to perform");

                        return errorstatus.unwillingToPerform;
                    }
                }

                #endregion

                //return invalid credentials 
                return errorstatus.invalidCredentials;
            }

            #endregion

            return errorstatus.success;
        }

        #endregion

        #region AD LDS bind
        /// <summary>
        /// This method validates the requirements of LDS.
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="portNo">Variable contains LDAP port number</param>
        /// <param name="userName">Variable contains name of the user of DC</param>
        /// <param name="password">Variable contains password of the user of DC</param>
        private void ValidateAdLdsRequirements(string hostName, uint portNo, string userName, string password)
        {
            #region AD-LDS

            //variable to represent success state of Lds operations.
            bool isSuccess = false;
            bool isSSLTLSEnabled = false;
            DirectoryEntry userEntry = null;

            //if the ports are Regular port
            if ((portNo == (uint)Port.LDAP_GC_PORT) || (portNo == (uint)Port.LDAP_PORT))
            {
                //get the protected Regular port number for LDS operations  from config file.
                ldsPort = Convert.ToInt32(ADLDSPortNum);
                //Create A Directory Entry for Lds operations
                userEntry = new DirectoryEntry("LDAP://" + hostName + ":" + ldsPort + "/" + "CN=User,CN=Schema,CN=Configuration," + LDSRootObjectName);
                userEntry.RefreshCache();
            }
            //Protected port
            else
            {
                isSSLTLSEnabled = true;
                //get the protected port number for Lds Operations from config file
                ldsPort = Convert.ToInt32(ADLDSSSLPortNum);  
                userName = string.Format("CN={0},CN=Roles,CN=Configuration,{1}",ADLDSUser,LDSRootObjectName);
                //Create A Directory Entry for Lds operations
                userEntry = new DirectoryEntry("LDAP://" + hostName + ":" + ldsPort + "/" + "CN=User,CN=Schema,CN=Configuration," + LDSRootObjectName,userName,password,AuthenticationTypes.SecureSocketsLayer);
                userEntry.RefreshCache();

                //Validate MS-ADTS-Security_R202
                TestClassBase.BaseTestSite.CaptureRequirement(
                    202,
                    @"For an AD/LDS 
                        Active Directory permits establishing an SSL/TLS-protected connection to a DC 
                        on a protected LDAPS port whose port number is configuration specific.");
            }

            #region Existence of  msDS-BindableObject

            //if the entry contains msDS-BindableObject
            //Validate all the requirements that check for the existence of msDS-BindableObject 
            if (userEntry.Properties["systemAuxiliaryClass"].Contains("msDS-BindableObject"))
            {
                //Validate the requirement as it contains msDS-BindableObject class in the DirectoryEntry
                //Validate MS-ADTS-Security_R246
                TestClassBase.BaseTestSite.CaptureRequirement(
                    246,
                    @"In AD/LDS, If an object 
                        class to be usable in an LDAP bind request, that object class must either 
                        contain the msDS-BindableObject class or the msDS-BindProxy class.");

                //Validate MS-ADTS-Security_R585
                TestClassBase.BaseTestSite.CaptureRequirement(
                    585,
                    @"In AD LDS, the config NC contains a well-known
                        Foreign Security Principals container. It stores foreign 
                        security principals from outside of the AD LDS forest.");

                //Validate the requirement as it contains msDS-BindableObject class in the DirectoryEntry
                //Validate MS-ADTS-Security_R10143
                TestClassBase.BaseTestSite.CaptureRequirement(
                    10143,
                    @"Each instance(user) of an 
                        object class within an AD/LDS schema can perform a bind operation if The 
                        object class contains msDS-BindableObject as a static auxiliary class");

                //Validate the requirement as it contains msDS-BindableObject class in the DirectoryEntry
                //Validate MS-ADTS-Security_R10144
                TestClassBase.BaseTestSite.CaptureRequirement(
                    10144,
                    @"Each instance(user) of an 
                        object class within an AD/LDS schema can perform a bind operation if The 
                        object class contains  a static auxiliary class that is a subclass of 
                        msDS-BindableObject");

                //Validate the requirement as it contains msDS-BindableObject class in the DirectoryEntry
                //Validate MS-ADTS-Security_R10145
                TestClassBase.BaseTestSite.CaptureRequirement(
                    10145,
                    @"Each instance(user) of an 
                        object class within an AD/LDS schema can perform a bind operation if The 
                        object class contains  The object class is a subclass of another object class 
                        that satisfies the above conditions that   he object class contains 
                        msDS-BindableObject as a static auxiliary class   or  The object class 
                        contains a static auxiliary class that is a subclass of msDS-BindableObject");
            }

            #endregion

            //set to false
            isSuccess = false;

            LdapConnection con = null;
            if (isSSLTLSEnabled)
            {
                con = new LdapConnection(new LdapDirectoryIdentifier(hostName, ldsPort), new NetworkCredential(userName, password), AuthType.Basic);
                con.SessionOptions.SecureSocketLayer = true;
            }
            else
            {
                con = new LdapConnection(new LdapDirectoryIdentifier(hostName, ldsPort), new NetworkCredential(userName, password, PrimaryDomainDnsName));
            }
            DeleteRequest del = new DeleteRequest("cn=" + TempUser0 + ",CN=Roles," + LDSApplicationNC);
            del.Controls.Add(new TreeDeleteControl());
            try
            {
                con.SendRequest(del);
            }
            catch
            {
            }

            //Create Security Principle object on ApplicationNC
            isSuccess = ADTSHelper.CreateSecurityPrincipleObject(hostName,
                                                                ldsPort,
                                                                "CN=Roles," + LDSApplicationNC,
                                                                userName,
                                                                password,
                                                                isSSLTLSEnabled);

            //if SecurityPrincipal is created on ApplicationNC Successfully
            //Validate this requirement
            //Validate MS-ADTS-Security_R239
            TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                isSuccess,
                239,
                @" AD/LDS (which has no domain NCs) permits security principals to be stored in an application NC.");

            #region Windows Server 2008 or 2008R2 Specific Requirements

            //if the Server version is Win 2k8 or Win 2k8r2
            if (PDCOSVersion == ServerVersion.Win2008 || PDCOSVersion == ServerVersion.Win2008R2)
            {
                //Represent the success of operation
                isSuccess = false;

                del = new DeleteRequest("cn=" + TempUser0 + ",CN=Roles,CN=Configuration," + LDSRootObjectName);
                del.Controls.Add(new TreeDeleteControl());
                try
                {
                    con.SendRequest(del);
                }
                catch
                {
                }

                //Create Security principal object 
                isSuccess = ADTSHelper.CreateSecurityPrincipleObject(hostName, ldsPort, "CN=Roles,CN=Configuration," + LDSRootObjectName, userName, password,isSSLTLSEnabled);

                //Validate MS-ADTS-Security_R240
                //if AD permits to create Security Principal object.
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    isSuccess,
                    240,
                    @"If the ADAMAllowADAMSecurityPrincipalsInConfigPartition
                        configuration setting equals 1, AD/LDS permits security
                        principals to be created in the config NC.");
            }

            #endregion

            #endregion
        }

        #endregion

        #region private methods

        //The following methods are used for Authentication mechanism 

        #region LDAPFASTBINDCONNECTION
        /// <summary>
        /// ldap connection for fast bind
        /// </summary>
        /// <param name="conn">instance of the ldapconnection</param>
        /// <returns>the status of the fast bind</returns>
        private int FastBindConnection(LdapConnection conn)
        {
            int result = 0;
            try
            {
                conn.AuthType = AuthType.External;

                conn.SessionOptions.FastConcurrentBind();
                conn.Bind();
            }
            catch (DirectoryOperationException doEx)
            {

                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, doEx.Message);
                result = (int)errorstatus.invalidCredentials;

            }
            return result;
        }
        #endregion

        #region Search Operation
        /// <summary>
        /// SearchOperation
        /// This action is used to perform LDAP Search operation
        /// </summary>
        /// <param name="portNum">Variable contains LDAP port numbers</param>
        /// <param name="tls">Variable is used to determine enabling or disabling
        /// of transport layer security</param>
        /// <returns>Returns Success if the method is successful
        /// 
        ///          Returns InvalidCredentials if the passed in credentials are invalid
        ///          
        ///          Returns unwillingToPerform if TLS is enabled over protected LDAP ports</returns>

        public SearchResponse SearchOperation(uint portNum, bool tls)
        {
            //get values from config file
            string hostName = PdcFqdn;

            //Search filter of Search operation
            string ldapSearchFilter = "objectclass=*";

            //array to get the attributes
            string[] attributesToReturn;

            if (((portNum == (uint)Port.LDAP_PORT) || (portNum == (uint)Port.LDAP_GC_PORT)) && tls)
            {
                attributesToReturn = new string[] { "supportedCapabilities" };
            }
            else
            {
                attributesToReturn = new string[] { "supportedSASLMechanisms" };
            }

            //perform Search operation
            SearchRequest searchRequest = new SearchRequest(null,
                                                            ldapSearchFilter,
                                                            System.DirectoryServices.Protocols.SearchScope.Base,
                                                            attributesToReturn);

            //Search Response.
            SearchResponse searchResponse = (SearchResponse)new LdapConnection(hostName).SendRequest(searchRequest);

            return searchResponse;
        }

        #endregion Search Operation

        #region Add Operation
        /// <summary>
        /// CreateActiveDirUser
        /// This action is used to create a new user in DC
        /// </summary>
        /// <param name="userName">Variable contains name of the user in DC</param>
        /// <param name="password">Variable contains password of the username</param>
        /// <returns>Returns GUID of the user</returns>
        public Guid CreateActiveDirUser(name userName, string password)
        {
            Guid oGuid = Guid.Empty;
            string strPath = string.Empty;

            try
            {
                if (userName == name.nameMapsMoreThanOneObject)
                {
                    // creating User in domain NC
                    strPath = string.Format("LDAP://{0}/CN=Users,{1}", PdcFqdn, parsedDN);
                }
                else
                    // creating security principle in Configuration NC
                    strPath = string.Format("LDAP://{0}/CN=Configuration,{1}", PdcFqdn, parsedDN);

                DirectoryEntry entry = new DirectoryEntry(strPath);
                DirectoryEntry newUser = entry.Children.Add("CN=" + TempUser0, "user");

                // Checking whether the temporary User (from config file) is already exists or not.
                if (!DirectoryEntry.Exists(string.Format("LDAP://{0}/CN={1},CN=Users,{2}", PdcFqdn, TempUser0, parsedDN)))
                {
                    newUser.Properties["samAccountName"].Value = userName;
                    newUser.CommitChanges();
                    oGuid = newUser.Guid;

                    newUser.Invoke("SetPassword", new Object[] { password });
                    newUser.CommitChanges();
                    newUser.Close();
                }
            }

            catch (DirectoryServicesCOMException ex)
            {
                // creating security principle in Schema NC
                strPath = string.Format("LDAP://{0}/CN=Schema,CN=Configuration,{1}", PdcFqdn, parsedDN);

                try
                {
                    DirectoryEntry entry = new DirectoryEntry(strPath);
                    DirectoryEntry newUser = entry.Children.Add("CN=" + TempUser0, "user");

                    // Checking whether User is already exists or not.
                    if (!DirectoryEntry.Exists(string.Format("LDAP://{0}/CN={1},CN=Users,{2}", PdcFqdn, TempUser0, parsedDN)))
                    {
                        newUser.Properties["samAccountName"].Value = TempUser0;
                        newUser.CommitChanges();
                        oGuid = newUser.Guid;

                        newUser.Invoke("SetPassword", new Object[] { password });
                        newUser.CommitChanges();
                        newUser.Close();
                    }
                }

                catch (DirectoryServicesCOMException excpObj)
                {
                    // MS-ADTS-Security_R238
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreNotEqual<int>(
                        excpObj.ErrorCode,
                        0,
                        238,
                        @"AD/DS restricts security principals to the domain NC.");

                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, excpObj.Message);

                }
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);

            }

            return oGuid;

        }

        #endregion Add Operation

        #region Modify Operation

        /// <summary>
        /// ModifyOperation
        /// This action is used to perform LDAP modify operation 
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="userName">Variable contains name of the user in DC</param>
        public bool ModifyOperation(string hostName, name userName)
        {
            //initialize
            string distinguishedName = string.Empty;
            string attributeName = string.Empty;
            string newAttributeValue = string.Empty;

            try
            {
                #region Establishing connection for ModifyOperation

                //connection 
                connection = new LdapConnection(hostName + ":" + (uint)Port.LDAP_PORT);
                connection.AuthType = AuthType.Basic;

                //Get the credential from config file for Bind operation.
                NetworkCredential credential = new NetworkCredential(
                    ClientUserName,
                    ClientUserPassword,
                    adDomain);

                connection.Credential = credential;
                connection.Bind();

                #endregion Establishing connection for ModifyOperation
            }
            //Catch specific exception
            catch (LdapException errMsg)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "An exception has occurred" + errMsg);
            }

            if (userName == name.anonymousUser)
            {
                distinguishedName = "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," + parsedDN;

                //The following property and value are defined in 3.1.1.4.4 Extended Access Checks
                //This property is mentioned in 3.1.1.4.4 Extended Access Checks of ADTS Document
                //Constant
                attributeName = "dSHeuristics";

                newAttributeValue = "0000002";
            }

            else
            {
                distinguishedName = "CN=" + TempUser0 + ",CN=Users," + parsedDN;

                //The following property and value are defined in TD
                attributeName = "userPrincipalName";

                //Can be any value ,concatenate with .com
                newAttributeValue = ClientUserName + "@" + adDomain + ".com";
            }

            try
            {
                ModifyRequest modReq = new ModifyRequest(distinguishedName,
                                             DirectoryAttributeOperation.Replace,
                                             attributeName,
                                             newAttributeValue);
                ModifyResponse modRes = (ModifyResponse)connection.SendRequest(modReq);
                return true;
            }

            catch (DirectoryOperationException e)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "An exception has occurred" + e.Message);
                return false;
            }

        }

        #endregion Modify Operation

        #region  ModifyOperationWithReturn

        /// <summary>
        /// ModifyOperation
        /// This action is used to perform LDAP modify operation 
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="attrName">Attribute name</param>
        /// <param name="attrValue">Attribute Value</param>
        /// <returns>true if success</returns>

        public ResultCode ModifyOperationWithReturn(string hostName, string attrName, string attrValue)
        {
            string distinguishedName = string.Empty;
            string attributeName = attrName;
            string newAttributeValue = attrValue;

            try
            {
                #region Establishing connection for ModifyOperation

                //used in Establishing connection for ModifyOperation
                connection = new LdapConnection(hostName + ":" + (uint)Port.LDAP_PORT);

                connection.AuthType = AuthType.Basic;

                NetworkCredential credential = new NetworkCredential(
                    ClientUserName,
                    ClientUserPassword,
                    adDomain);

                connection.Credential = credential;
                connection.Bind();

                #endregion Establishing connection for ModifyOperation

                //get the dn of Computers NC
                distinguishedName = "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN;

                //Modify Request
                ModifyRequest modReq = new ModifyRequest(distinguishedName,
                                             DirectoryAttributeOperation.Replace,
                                             attributeName,
                                             newAttributeValue);

                //Response of the Modify Request.
                ModifyResponse modRes = (ModifyResponse)connection.SendRequest(modReq);

                //if success return true.
                return modRes.ResultCode;
            }
            catch (DirectoryOperationException e)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                return e.Response.ResultCode;
            }

        }

        #endregion Modify Operation

        #region  ModifyOperationWithReturn

        /// <summary>
        /// ModifyOperation
        /// This action is used to perform LDAP modify operation 
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="attrName">Attribute name</param>
        /// <param name="attrValue">Attribute Value</param>
        /// <param name="strResult">Specifies the value of exception if any occurs</param>
        /// <returns>true if success</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        public bool ModifyOperationWithReturn(string hostName, string attrName, string attrValue, out string strResult)
        {
            string distinguishedName = string.Empty;
            string attributeName = attrName;
            string newAttributeValue = attrValue;
            strResult = string.Empty;

            try
            {
                #region Establishing connection for ModifyOperation

                //used in Establishing connection for ModifyOperation
                connection = new LdapConnection(hostName + ":" + (uint)Port.LDAP_PORT);

                connection.AuthType = AuthType.Basic;
                NetworkCredential credential = new NetworkCredential(ClientUserName, ClientUserPassword, adDomain);
                connection.Credential = credential;
                connection.Bind();

                #endregion Establishing connection for ModifyOperation

                //get the dn of Computers NC
                distinguishedName = "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN;

                //Modify Request
                ModifyRequest modReq = new ModifyRequest(distinguishedName,
                                             DirectoryAttributeOperation.Replace,
                                             attributeName,
                                             newAttributeValue);

                //Response of the Modify Request.
                /**************edit by Rina **************/
                DirectoryResponse resTmp = connection.SendRequest(modReq);
                ModifyResponse modRes = (ModifyResponse)resTmp;
                //ModifyResponse modRes = (ModifyResponse)connection.SendRequest(modReq);

                strResult = strSuccess;

                //if success return true.
                return true;
            }
            catch (DirectoryOperationException e)
            {
                if (e.Message.Contains("The user has insufficient access rights"))
                {
                    strResult = strInsufficientRights;
                }
                // add by Rina
                else
                {
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "An exception has occurred:" + e.Message);
                }

                return false;
            }
        }

        #endregion Modify Operation

        #region LdapBindConnection
        /// <summary>
        /// This methods binds a ldap connection for the specific credentials, domain and port. 
        /// </summary>
        /// <param name="fullDomainName">Specifies Full domain name</param>
        /// <param name="userName">Specifies user name</param>
        /// <param name="password">specifies password</param>
        /// <param name="authType">specifies the AuthType mechanism used for Bind Ldap Connection</param>
        /// <returns>returns 0 if successful else error code.</returns>
        private uint LdapBindConnection(string fullDomainName,
                                        string userName,
                                        string password,
                                        AuthType authType)
        {
            uint ldapResultCode = 0;

            try
            {
                //Ldap identifier
                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(fullDomainName);

                //Asign the identifies to connection.
                connection = new LdapConnection(identifier);

                //Credentials.
                NetworkCredential networkCredential = new NetworkCredential(userName, password, adDomain);

                //Authtype for Binding the connection
                //Assign the specified Authtype.
                connection.AuthType = authType;

                // If unspecified the protocolVersion number, by default it will take 2.
                //We have to validate LDAP V3. So the version has to set explicitly to 3
                connection.SessionOptions.ProtocolVersion = 3;

                //Tome out 
                connection.Timeout = new TimeSpan(0, 0, 120);

                //perform Bind
                //if failure control will return to Catch block
                connection.Bind(networkCredential);

            }

            //To catch the protocol specific error code.
            catch (LdapException ldapEx)
            {
                ldapResultCode = (uint)ldapEx.ErrorCode;

            }

            return ldapResultCode;

        }

        #endregion

        #region ExternalBindConnection
        /// <summary>
        /// This methods binds a ldap connection for the specific credentials, domain and port. 
        /// </summary>
        /// <param name="fullDomainName">Specifies Full domain name</param>
        /// <param name="userName">Specifies user name</param>
        /// <param name="password">specifies password</param>
        /// <param name="authType">specifies the AuthType mechanism used for Bind Ldap Connection</param>
        /// <returns>returns 0 if successful else error code.</returns>
        private uint ExternalBindConnection(string fullDomainName,
                                        string userName,
                                        string password,
                                        AuthType authType)
        {
            uint ldapResultCode = 0;

            try
            {
                //Ldap identifier
                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(fullDomainName);

                //Assign the identifies to connection.
                connection = new LdapConnection(identifier);

                //Credentials.
                NetworkCredential networkCredential = new NetworkCredential(userName, password, adDomain);

                //Authtype for Binding the connection
                //Assign the specified Authtype.
                connection.AuthType = authType;

                // If unspecified the protocolVersion number, by default it will take 2. 
                //We have to validate LDAP V3. So the version has to set explicitly to 3
                connection.SessionOptions.ProtocolVersion = 3;

                //Tome out 
                connection.Timeout = new TimeSpan(0, 0, 120);

                //perform Bind
                //if failure control will return to Catch block
                connection.Bind(networkCredential);
            }

            //To catch the protocol specific error code.
            catch (LdapException ldapEx)
            {
                //Exception occured as Invalid Credentials are passed.
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ldapEx.Message);

                ldapResultCode = (uint)errorstatus.invalidCredentials;
            }

            return ldapResultCode;

        }

        #endregion

        #region SicilyBindConnection
        /// <summary>
        /// This methods binds a ldap connection for the specific credentials, domain and port. 
        /// </summary>
        /// <param name="fullDomainName">Specifies Full domainname</param>
        /// <param name="userName">Specifies user name</param>
        /// <param name="password">specifies password</param>
        /// <returns>returns 0 if successful else error code.</returns>
        private uint SicilyBindConnection(string fullDomainName,
                                          string userName,
                                          string password)
        {
            uint ldapResultCode = 0;

            try
            {
                //Ldap identifier
                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(fullDomainName);

                //Assign the identifies to connection.
                connection = new LdapConnection(identifier);

                //Credentials.
                NetworkCredential networkCredential = new NetworkCredential(userName, password, adDomain);

                //Authtype for Binding the connection
                //Assign the specified Authtype.
                connection.AuthType = AuthType.Sicily;

                // If unspecified the protocolVersion number, by default it will take 2. 
                //We have to validate LDAP V3. So the version has to set explicitly to 3
                connection.SessionOptions.ProtocolVersion = 3;

                //Tome out 
                connection.Timeout = new TimeSpan(0, 0, 120);

                //perform Bind
                //if failure control will return to Catch block
                connection.Bind(networkCredential);
            }

            //To catch the protocol specific error code.
            catch (LdapException ldapEx)
            {
                ldapResultCode = (uint)ldapEx.ErrorCode;
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ldapEx.Message);
            }

            return ldapResultCode;

        }

        #endregion

        #region MutualBind LdapConnection

        /// <summary>
        /// This method is used for MutualBind
        /// </summary>
        /// <param name="userName">specifies username</param>
        /// <param name="password">specifies password</param>
        private uint MutualBind(string userName, string password)
        {
            uint ldapResultCode = 0;
            try
            {
                //mutualBindConnection
                LdapConnection mutualBindConnection;

                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(PrimaryDomainDnsName);
                //connect with identifier
                mutualBindConnection = new LdapConnection(identifier);

                //Credential used for Mutual bind connection.
                NetworkCredential networkCredential = new NetworkCredential(userName, password, PdcFqdn);

                //For Mutual Bind the AuthType should be Kerberos.
                mutualBindConnection.AuthType = AuthType.Kerberos;

                // If unspecified the protocolVersion number, by default it will take 2.
                //We have to validate LDAP V3. So the version has to set explicitly to 3
                mutualBindConnection.SessionOptions.ProtocolVersion = 3;

                //Tome out limit of the connection.
                mutualBindConnection.Timeout = new TimeSpan(0, 0, 120);

                //Bind the Mutual authenticated connection.
                mutualBindConnection.Bind(networkCredential);
            }

            //To catch the protocol specific error code.
            catch (LdapException ldapEx)
            {
                ldapResultCode = (uint)ldapEx.ErrorCode;
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ldapEx.Message);
            }

            return ldapResultCode;

        }

        #endregion

        #region GSSSPNEGOBind  Connection

        /// <summary>
        /// This method performs Bind using either NTLM\Kerberos
        /// </summary>
        /// <param name="userName">specifies username</param>
        /// <param name="password">specifies password</param>
        /// <param name="authtype"> auth type used</param>
        private uint GSSSPNEGOBind(string userName, string password, AuthType authtype)
        {
            uint ldapResultCode = 0;
            try
            {

                //gssSPNegoConnection
                LdapConnection gssSPNegoConnection;

                string fullDomainName = PdcFqdn + ":" + (uint)Port.LDAP_PORT;
                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(fullDomainName);
                //connect with identifier
                gssSPNegoConnection = new LdapConnection(identifier);

                //Credential used for gssSPNegoConnection bind connection.
                NetworkCredential networkCredential = new NetworkCredential(userName, password, adDomain);

                //For GSSSPNEGO the AuthType can be NTLM or Kerberos
                //set the AuthType based on the argument
                gssSPNegoConnection.AuthType = authtype;

                // If unspecified the protocolVersion number, by default it will take 2. 
                //We have to validate LDAP V3. So the version has to set explicitly to 3
                gssSPNegoConnection.SessionOptions.ProtocolVersion = 3;

                //Tome out limit of the connection.
                gssSPNegoConnection.Timeout = new TimeSpan(0, 0, 120);

                //Bind the gssSPNegoConnection authenticated connection.
                gssSPNegoConnection.Bind(networkCredential);
            }

            //To catch the protocol specific error code.
            catch (LdapException ldapEx)
            {
                ldapResultCode = (uint)ldapEx.ErrorCode;
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ldapEx.Message);
            }

            return ldapResultCode;

        }
        #endregion

        #region DigestBind LdapConnection

        /// <summary>
        /// This method is used for MutualBind
        /// </summary>
        /// <param name="userName">specifies username</param>
        /// <param name="password">specifies password</param>
        private uint DigestBind(string userName, string password)
        {
            uint ldapResultCode = 0;
            try
            {
                //An instance of Digest Connection
                LdapConnection digestBindConnection;

                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(PdcFqdn + ":" + (int)enumPortNum);

                digestBindConnection = new LdapConnection(identifier);

                NetworkCredential networkCredential = new NetworkCredential(userName, password, PrimaryDomainDnsName);

                //Specifies Digest Bind
                digestBindConnection.AuthType = AuthType.Digest;

                // If unspecified the protocolVersion number, by default it will take 2.
                //We have to validate LDAP V3. So the version has to set explicitly to 3
                digestBindConnection.SessionOptions.ProtocolVersion = 3;

                //Time out limit
                digestBindConnection.Timeout = new TimeSpan(0, 0, 120);

                //Bind Ldapconnection.
                digestBindConnection.Bind(networkCredential);

            }

            //To catch the protocol specific error code.
            catch (LdapException ldapEx)
            {
                ldapResultCode = (uint)ldapEx.ErrorCode;
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ldapEx.Message);
            }

            return ldapResultCode;

        }

        #endregion

        #region SubSequentDigestBind
        /// <summary>
        /// This method proves that the subsequent digest bind is not applicable
        /// </summary>
        /// <param name="userName">username </param>
        /// <param name="password">password</param>
        /// <returns>returns error</returns>
        private int SubSequentDigestBind(string userName, string password)
        {
            //this variable is used to get the result of subsequent binding
            int subSequentBindResult = 0;

            //This instance of Ldap connection is used to prove subsequent binding.
            LdapConnection subDigestConnection = new LdapConnection(new LdapDirectoryIdentifier(PdcFqdn));

            //Credentials to be passed
            NetworkCredential subdigestCredential = new NetworkCredential(userName, password, PrimaryDomainDnsName);

            subDigestConnection.Credential = subdigestCredential;

            //Specifies Digest Bind
            subDigestConnection.AuthType = AuthType.Digest;

            //Bind Ldapconnection.
            //subDigestConnection.Bind(subdigestCredential);  //by Rina

            try
            {
                subDigestConnection.Bind(subdigestCredential);  //by Rina
                //Sub-sequent bind after Digest Bind 
                //Bind Should not happen.
                //Bind Again 
                subDigestConnection.AuthType = AuthType.Basic;

                subDigestConnection.Bind();
            }

            //To catch the protocol specific error code.
            catch (LdapException ex)
            {
                //Get the result if bind 
                subSequentBindResult = ex.ErrorCode;
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);
            }
            return subSequentBindResult;
        }

        #endregion

        #region LDAP Fast Bind connection

        /// <summary>
        /// This method is used for fast bind ldap connection.
        /// </summary>
        /// <param name="fullDomainName"></param>
        /// <returns></returns>
        private LdapConnection LdapFastBindConnection(string fullDomainName)
        {
            try
            {
                //For LdapFastBind Connection
                //get the full domainname
                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(fullDomainName);

                connection = new LdapConnection(identifier);

                //set Auto bind to false.
                connection.AutoBind = false;

                connection.AuthType = AuthType.Basic;

                return connection;
            }
            //To catch the protocol specific error code.
            catch (LdapException e)
            {
                string str = e.Message;
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                return connection;
            }

        }
        #endregion

        #region EnableFastBindonLdapConnection
        /// <summary>
        /// This method enables fast bind on already established Ldap connection
        /// </summary>
        /// <param name="conn">specifies ldap connection</param>
        /// <returns>retunes 0 if success else returns unwillingtoperform</returns>
        private uint EnableFastBindonLdapConnection(LdapConnection conn)
        {

            uint result = 0;

            try
            {
                //Enable Fast Bind on LdapConnection.
                conn.SessionOptions.FastConcurrentBind();
            }
            //To catch the protocol specific error code.
            catch (DirectoryOperationException e)
            {
                //return unwillingToPerform as fast bind cannot be enabled and exception will be thrown
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                result = (uint)errorstatus.unwillingToPerform;
            }

            return result;
        }

        #endregion

        #region FastBindonNonBasicConnection
        /// <summary>
        /// This method enables fast bind on already established Ldap connection
        /// </summary>
        /// <param name="conn">specifies ldap connection</param>
        /// <returns>retunes 0 if success else returns unwillingtoperform</returns>
        private int FastBindonNonBasicConnection(LdapConnection conn)
        {

            int result = 0;

            try
            {
                //other Bind Type
                conn.AuthType = AuthType.External;

                //Enable Fast Bind on LdapConnection.
                conn.SessionOptions.FastConcurrentBind();

                conn.Bind();
            }
            //To catch the protocol specific error code.
            catch (DirectoryOperationException ex)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);

                //Other than Success
                //The operation will fail.
                result = (int)errorstatus.unwillingToPerform;
            }

            return result;
        }

        #endregion

        #region External Binding

        #region // pre-configuration instructions for running the Certificate method
        /* 1. Create or obtain a client certificate file that contains the private key (with extension .pfx)
              you can generate a client authentication certificate using Microsoft Certificate Services.
              This certificate name and file location is what you pass to the certFileandPath parameter in the 
              Certificate method. Specify the password you assigned to the certificate for the certPword parameter.
           2. Install the personal certificate store on the client machine where you intend on running this sample.    
           3. From the Certificates mmc, export the certificate to a file without the private key and use the DER encoding format
           4. Copy the DER encoded certificate to the domaincontroller that will perform the certificate authentication
           5. Assign the client certificate to a user account with administrative permissions
              - Open the Active Directory Users and Computers MMC
              - Click the View menu and verify that Advanced Features is enabled
              - From the context menu of a user account, click All Tasks and then click Name Mapping
              - Click Add and assign the certificate copied in step 4. 
              - Verify that "Use subject for alternate security identity" is enabled and then click OK.
           6. The domaincontroller must have a server authentication certificate installed. 
              You can verify this from LDP by attempting to connect to the server using ldp over the AD ssl connection port 636
         */
        #endregion

        /// <summary>
        /// performs External Bind using Client Certificate,
        /// </summary>
        /// <returns></returns>
        private bool ExternalBindUsingCertificate()
        {
            string hostNameandPort = PdcFqdn + ":" + (int)(Port.LDAP_PORT);

            //Create an instance of LdapConnection
            LdapConnection externalConnection = new LdapConnection(new LdapDirectoryIdentifier(hostNameandPort));

            //Assign the connection options to options
            LdapSessionOptions options = externalConnection.SessionOptions;

            options.SecureSocketLayer = false;
            options.ProtocolVersion = 3;

            //Start Tls 
            options.StartTransportLayerSecurity(null);

            // QueryClientCertificate
            X509Certificate cert = new X509Certificate();

            //External AuthType 
            externalConnection.AuthType = AuthType.External;

            // select a certificate to import
            cert.Import(certFilewithPathSpec, certPassword, X509KeyStorageFlags.DefaultKeySet);

            // add the certificate to the connection
            externalConnection.ClientCertificates.Add(cert);

            //Set AutoBind to false
            externalConnection.AutoBind = false;

            //perform Implicit Bind 
            ExternalBindAuthentication(externalConnection);

            return true;

        }

        #endregion

        #region ExtendedOperationToStartTLS

        /// <summary>
        /// This method starts tls by means of Extended operation and subsequently perform external bind
        /// with Client Certificate
        /// </summary>
        /// <returns>retunes true if success</returns>
        private bool ExtendedOperationToStartTLS()
        {
            //Regular port
            string hostNameandPort = PdcFqdn + ":" + (int)(Port.LDAP_PORT);

            //Create an instance of LdapConnection
            LdapConnection externalConnection = new LdapConnection(new LdapDirectoryIdentifier(hostNameandPort));

            //Constant value taken from ADTS TD Section - 3.1.1.3.4.2 LDAP Extended Operations
            string requestName = "1.3.6.1.4.1.1466.20037";

            //null terminated 
            string strValue = "\0";

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            //Converting the request value into  bytes
            byte[] requestValue = encoding.GetBytes(strValue);

            //Start tls using Extended operation
            ExtendedResponse extendedoperationResponse = ADTSHelper.ExtendedOperations(requestName,
                                                                                       requestValue,
                                                                                       externalConnection);
            TestClassBase.BaseTestSite.Assert.IsNull(extendedoperationResponse, "extended operation is success");
            // QueryClientCertificate
            X509Certificate cert = new X509Certificate();

            //External AuthType 
            externalConnection.AuthType = AuthType.External;

            //select a certificate to import
            cert.Import(certFilewithPathSpec, certPassword, X509KeyStorageFlags.DefaultKeySet);

            //add the certificate to the connection
            externalConnection.ClientCertificates.Add(cert);

            //Set AutoBind to false
            externalConnection.AutoBind = false;

            //perform Implicit Bind 
            ExternalBindAuthentication(externalConnection);

            return true;
        }

        #endregion

        #region ProtectedExternalBindUsingCertificate

        /// <summary>
        /// performs External Bind using Client Certificate on protected port,
        /// </summary>
        /// <returns></returns>
        private bool ProtectedExternalBindUsingCertificate()
        {
            string hostNameandPort = PdcFqdn + ":" + (int)(Port.LDAP_SSL_PORT);

            //Create an instance of LdapConnection
            LdapConnection externalConnection = new LdapConnection(new LdapDirectoryIdentifier(hostNameandPort));

            //QueryClientCertificate
            X509Certificate cert = new X509Certificate();

            //External AuthType 
            externalConnection.AuthType = AuthType.External;

            //select a certificate to import
            cert.Import(certFilewithPathSpec, certPassword, X509KeyStorageFlags.DefaultKeySet);

            //add the certificate to the connection
            externalConnection.ClientCertificates.Add(cert);

            externalConnection.AutoBind = false;

            //Perform Implicit bind operation.
            ExternalBindAuthentication(externalConnection);
            return true;

        }

        #endregion

        #region ExternalBindAuthentication

        /// <summary>
        /// This method verifies whether external Bind is Bounded or not by Adding and deleting a user
        /// </summary>
        /// <param name="con">specifies Ldap connection</param>
        /// <returns>true on success</returns>
        public bool ExternalBindAuthentication(LdapConnection con)
        {
            AuthType authType = con.AuthType;
            AddResponse addResponse = null;
            DeleteResponse delResponse = null;

            string dn = "CN=" + ExternalBindUser + ",CN=Users," + parsedDN;

            // send a request that should fail without properly authenticating to the directory server        
            try
            {
                //Create new object in User 
                addResponse = (AddResponse)con.SendRequest(new AddRequest(dn, "user"));

            }
            catch (DirectoryOperationException e)
            {
                // if an exception occurs return false
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                return false;
            }
            catch (LdapException e)
            {
                // if an exception occurs return false
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                return false;
            }

            if (addResponse.ResultCode != ResultCode.Success)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "operation performed Successfully  ", addResponse.ResultCode);
                return false;
            }

            // delete the created user
            delResponse = (DeleteResponse)con.SendRequest(new DeleteRequest(dn));
            TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "User account deleted (result {0})", addResponse.ResultCode);

            if (delResponse.ResultCode != ResultCode.Success)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "An Exception has occured ", addResponse.ResultCode);
                return false;
            }

            return true;

        }

        #endregion

        #endregion

        #region Authorization

        #region ValidatemsDS_QuotaEffectiveAttribute

        /// <summary>
        /// Validates the Requirements of msDS_QuotaEffectiveAttribute
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="ctrlAccessRight">Specifies the control access Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidatemsDS_QuotaEffectiveAttribute(AccessRights accessRight,
                                                                ControlAccessRights ctrlAccessRight,
                                                                AttribsToCheck attr)
        {

            #region Allow Access Requirementts Validation

            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;
            userForcePwd = false;

            //Specifies the ReadAccess.
            bool isReadAccessGranted;

            //Specifies the ControlAccess.
            bool isControlAccessSet;

            //Specifies whether the ControlAccessGranted.
            bool isControlAccessGranted;

            //Specifies the AccessRight is removed.
            bool isAccessRightRemoved;

            //validate if msDS_QuotaEffective attribute and RIGHT_DS_READ_PROPERTY and DS_Query_Self_Quota
            //Allow the specifies Access to validate the Requirements.DS_Query_Self_Quota
            if ((attr == AttribsToCheck.msDS_QuotaEffective) &&
                (accessRight == AccessRights.RIGHT_DS_READ_PROPERTY) &&
                (ctrlAccessRight == ControlAccessRights.DS_Query_Self_Quota))
            {

                try
                {

                    #region Set Access Rights

                    //Set READ AccessRight to the user to access msDS-QuotaEffective attribute.
                    //Grant Access Right
                    //NTDS Quotas will be in configuration NC.
                    isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                    "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    ActiveDirectoryRights.ReadProperty,
                                                    AccessControlType.Allow);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Set READ AccessRight to the user to access msDS-QuotaEffective attribute.");

                    //Grant Specific  Control Access right
                    isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Query_Self_Quota,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "grant specific control access right is success");
                    #endregion

                    return errorstatus.success;
                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {

                    //Failure has happened while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;
                }
            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else if ((attr == AttribsToCheck.msDS_QuotaEffective) &&
                ((accessRight != AccessRights.NotSet) ||
                (ctrlAccessRight != ControlAccessRights.NotSet)))
            {
                try
                {
                    #region Do not Set the Access

                    //deny Repl_ManageE_Topology Control Access  Right As the rights are notsett
                    bool isReadControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                              ClientUserName,
                                                              ClientUserPassword,
                                                              adDomain,
                                                              guidDS_Query_Self_Quota,
                                                              ActiveDirectoryRights.ExtendedRight,
                                                              AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadControlAccess, "deny Repl_ManageE_Topology Control Access");
                    //Deny READ Access right as the rights are not set
                    //Do not set the Access Right
                    bool isReadAccessRemoved = ADTSHelper.SetAccessRights(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       ActiveDirectoryRights.ReadProperty,
                                       AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemoved, "Deny READ Access right as the rights are not set");
                    #endregion

                    #region Validation

                    //Trying to access the msDS-QuotaEffective attribute
                    SearchResponse searchResponseQuotaEffective = ADTSHelper.SearchObject(
                                                    "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                    "objectclass=msDS-QuotaContainer",
                                                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                    new string[] { "msDS-QuotaEffective" },
                                                    connection);

                    int actualCount = searchResponseQuotaEffective.Entries.Count;

                    #region RollBack the Removed Operations



                    //Rollback to previous Access Rights.
                    //Rollback to ReadAccess.
                    isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=NTDS Quotas," + parsedDN,
                                                                             AdminUser,
                                                                             AdminUserPassword,
                                                                             adDomain,
                                                                             ActiveDirectoryRights.ReadProperty,
                                                                             AccessControlType.Deny);

                    if (true == isAccessRightRemoved)
                    {
                        isReadAccessGranted = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                        AdminUser,
                                                        AdminUserPassword,
                                                        ClientUserName,
                                                        adDomain,
                                                        ActiveDirectoryRights.ReadProperty,
                                                        AccessControlType.Allow);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "READ Access right is granted");

                        //Rollback To Control Access Right
                        isControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       AdminUser,
                                       AdminUserPassword,
                                       adDomain,
                                       guidDS_Query_Self_Quota,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Control Access right is set");

                        isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                      "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                      ClientUserName,
                                                                      ClientUserPassword,
                                                                      adDomain,
                                                                      guidDS_Query_Self_Quota,
                                                                      ActiveDirectoryRights.ExtendedRight,
                                                                      AccessControlType.Allow);

                        TestClassBase.BaseTestSite.Assert.IsFalse(isControlAccessGranted, "Control Access right is not granted");
                    }
                    #endregion

                    //Validate MS-ADTS-Security_R44
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, actualCount, 44,
                        @"While accessing  msDS-QuotaEffective attribute,if the security context of the requestor
                        are not granted these rights RIGHT_DS_READ_PROPERTY on the Quotas container, or and
                        DS-Query-Self-Quota control access right on the Quotas container,then the value is treated as 
                        does not exist in the returned attributes and the LDAP filter.");

                    #endregion

                    #region RollBack the Removed Operations

                    //To Set the Access Rights to allow
                    //To Rollback to previous Rights.
                    //Delete the current right
                    isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                        AdminUser,
                                                                        AdminUserPassword,
                                                                        adDomain,
                                                                        ActiveDirectoryRights.ReadProperty,
                                                                        AccessControlType.Deny);
                    if (true == isAccessRightRemoved)
                    {
                        //Set Allow Access Right
                        isReadAccessGranted = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                        ClientUserName,
                                                        ClientUserPassword,
                                                        AdminUser,
                                                        adDomain,
                                                        ActiveDirectoryRights.ReadProperty,
                                                        AccessControlType.Allow);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "READ Access right is granted");

                        //Rollback To Control Access Right
                        //Remove Control Access Right for Roll back
                        isControlAccessSet = ADTSHelper.RemoveControlAcessRights(
                                       PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Repl_ManageE_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Deny the Control Access right");

                        //Allow Control  Access Right
                        isControlAccessGranted = ADTSHelper.SetControlAcessRights(
                                                                      PdcFqdn, 
                                                                      "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                      ClientUserName,
                                                                      ClientUserPassword,
                                                                      adDomain,
                                                                      guidDS_Query_Self_Quota,
                                                                      ActiveDirectoryRights.ExtendedRight,
                                                                      AccessControlType.Allow);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Control Access right is granted.");
                    }
                    #endregion

                    return errorstatus.failure;

                }
                //Protocol Specific Exception
                catch (DirectoryException)
                {
                    //Failure has happended while performing Ldap operation.
                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Rollback to ReadAccess
                    isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                        AdminUser,
                                                                        AdminUserPassword,
                                                                        adDomain,
                                                                        ActiveDirectoryRights.ReadProperty,
                                                                        AccessControlType.Deny);
                    if (true == isAccessRightRemoved)
                    {
                        isReadAccessGranted = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                        ClientUserName,
                                                        ClientUserPassword,
                                                        AdminUser,
                                                        adDomain,
                                                        ActiveDirectoryRights.ReadProperty,
                                                        AccessControlType.Allow);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Read Access right is granted.");

                        //Rollback To Control Access Right
                        isControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       AdminUser,
                                       AdminUserPassword,
                                       adDomain,
                                       guidDS_Repl_ManageE_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Control Access right is removed.");

                        //Set ExtendedRight 
                        isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                      "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                      ClientUserName,
                                                                      ClientUserPassword,
                                                                      adDomain,
                                                                      guidDS_Query_Self_Quota,
                                                                      ActiveDirectoryRights.ExtendedRight,
                                                                      AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allow ExtendedRight Control Access.");
                    }
                    #endregion

                    return errorstatus.failure;

                }

            }

            #endregion

            #region Notset

            else
            {
                //if the Access right other than the specified rights for the attribute 
                //Simple Return failure
                //As the Actual Rights are NotSet
                return errorstatus.failure;
            }

            #endregion
        }

        #endregion

        #region ValidatemsDS_QuotaUsedAttribute

        /// <summary>
        /// Validates the Requirements of msDS_QuotaUsedAttribute
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="ctrlAccessRight">Specifies the control Access Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidatemsDS_QuotaUsedAttribute(AccessRights accessRight,
                                                                ControlAccessRights ctrlAccessRight,
                                                                AttribsToCheck attr)
        {

            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;

            userForcePwd = false;

            #region Allow Access Requirements Validation

            //validate if msDS_QuotaUsed attribute and RIGHT_DS_READ_PROPERTY and DS_Query_Self_Quota
            //Allow the specifies Access to validate the Requirements.
            if ((attr == AttribsToCheck.msDS_QuotaUsed) &&
                (accessRight == AccessRights.RIGHT_DS_READ_PROPERTY) &&
                (ctrlAccessRight == ControlAccessRights.DS_Query_Self_Quota))
            {
                try
                {
                    #region Set Access Rights

                    //Validate the requirements which required RIGHT_DS_READ_PROPERTY Access right only.
                    ValidateReadAccessControl(accessRight);

                    //Set READ AccessRight to the user to access msDS-QuotaEffective attribute.
                    //Grant Access Rights
                    bool isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    ActiveDirectoryRights.ReadProperty,
                                                    AccessControlType.Allow);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Read Access right is Granted.");

                    //Grant Specified  Control Access right
                    bool isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                  ClientUserName,
                                                                  ClientUserPassword,
                                                                  adDomain,
                                                                  guidDS_Query_Self_Quota,
                                                                  ActiveDirectoryRights.ExtendedRight,
                                                                  AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allow ExtendedRight Control Access.");
                    #endregion

                    return errorstatus.success;

                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {
                    //Failure has happened while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;

                }

            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else if ((attr == AttribsToCheck.msDS_QuotaUsed) &&
                ((accessRight != AccessRights.RIGHT_DS_READ_PROPERTY) ||
                (ctrlAccessRight != ControlAccessRights.DS_Query_Self_Quota)))
            {
                //Specifies the AccessRights is removed;
                bool isAccessRightsRemoved;

                //Specifies the AccessRight;
                bool isaccessset;

                //Specifies the Control Access Right is rollback.
                bool isControlAccessremoved;

                //Specifies the ControlAccess is set.
                bool isControlAccessSet;

                try
                {
                    #region Remove Accesss

                    //deny Repl_ManageE_Topology Control Access  Right
                    bool isReadControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                              "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                              ClientUserName,
                                                              ClientUserPassword,
                                                              adDomain,
                                                              guidDS_Repl_ManageE_Topology,
                                                              ActiveDirectoryRights.ExtendedRight,
                                                              AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadControlAccess, "Deny ExtendedRight Control Access.");
                    //Deny READ Access right
                    //Remove Access to deny access of the specifies attribute
                    bool isReadAccessRemoved = ADTSHelper.SetAccessRights(PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       ActiveDirectoryRights.ReadProperty,
                                       AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemoved, "Read Access right is removed.");

                    #endregion

                    #region Validation

                    //Trying to access the msDS-QuotaEffective attribute
                    SearchResponse searchResponseQuotaUsed = ADTSHelper.SearchObject(
                                                    "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                    "objectclass=msDS-QuotaContainer",
                                                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                    new string[] { "msDS-QuotaUsed" },
                                                    connection);

                    int actualCount = searchResponseQuotaUsed.Entries.Count;

                    #region RollBack the Removed Operations


                    //Rollback to previous Access Rights.
                    //Rollback to ReadAccess
                    isAccessRightsRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                         "CN=NTDS Quotas ," + parsedDN,
                                                                         AdminUser,
                                                                         AdminUserPassword,
                                                                         adDomain,
                                                                         ActiveDirectoryRights.ReadProperty,
                                                                         AccessControlType.Deny);
                    if (true == isAccessRightsRemoved)
                    {
                        //setting back access rights
                        isaccessset = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                       "CN=NTDS Quotas ," + parsedDN,
                                       AdminUser,
                                       AdminUserPassword,
                                       ClientUserName,
                                       adDomain,
                                       ActiveDirectoryRights.ReadProperty,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isaccessset, "Access right is allow.");

                        //Rollback To Control Access Right
                        isControlAccessremoved = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=NTDS Quotas ," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Query_Self_Quota,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessremoved, "ControlAccess right is removed.");

                        //setting back control access rights
                        isControlAccessSet = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                       "CN=NTDS Quotas ," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Query_Self_Quota,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Allow ExtendedRight Control Access.");
                    }
                    #endregion

                    //Validate MS-ADTS-Security_R48
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, actualCount, 48,
                        @"While accessing  msDS-QuotaUsed attribute,if the security context of the requestor
                        are not granted these rights RIGHT_DS_READ_PROPERTY on the Quotas container, or and
                        DS-Query-Self-Quota control access right on the Quotas container,then the value is treated as 
                        does not exist in the returned attributes and the LDAP filter.");

                    #endregion

                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Rollback to ReadAccess
                    isAccessRightsRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                          AdminUser,
                                                                          AdminUserPassword,
                                                                          adDomain,
                                                                          ActiveDirectoryRights.ReadProperty,
                                                                          AccessControlType.Deny);
                    if (true == isAccessRightsRemoved)
                    {
                        //setting back access rights
                        isaccessset = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       AdminUser,
                                       adDomain,
                                       ActiveDirectoryRights.ReadProperty,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isaccessset, "Access right is allow.");

                        //Rollback To Control Access Right
                        isControlAccessremoved = ADTSHelper.RemoveControlAcessRights(
                                       PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Query_Self_Quota,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessremoved, "ControlAccess right is removed.");

                        //setting back control access rights
                        isControlAccessSet = ADTSHelper.SetControlAcessRights(
                                       PdcFqdn, 
                                       "CN=NTDS Quotas ," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Query_Self_Quota,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Allow the ExtendedRight to the user.");
                    }
                    #endregion

                    return errorstatus.failure;
                }
                catch (DirectoryException)
                {
                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Rollback to ReadAccess
                    isAccessRightsRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                           "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                                                           AdminUser,
                                                                           AdminUserPassword,
                                                                           adDomain,
                                                                           ActiveDirectoryRights.ReadProperty,
                                                                           AccessControlType.Deny);
                    if (true == isAccessRightsRemoved)
                    {
                        //setting back access rights
                        isaccessset = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       AdminUser,
                                       adDomain,
                                       ActiveDirectoryRights.ReadProperty,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isaccessset, "Access right is allow.");

                        //Rollback To Control Access Right
                        isControlAccessremoved = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=NTDS Quotas,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Query_Self_Quota,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessremoved, "ControlAccess right is removed.");

                        //setting back control access rights
                        isControlAccessSet = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                       "CN=NTDS Quotas ," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Query_Self_Quota,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Allow ExtendedRight Control Access.");
                    }
                    #endregion

                    //Failure has happened while performing Ldap operation.
                    return errorstatus.failure;

                }

            }

            #endregion

            #region NotSet

            else
            {
                //if the Access right other than the specified rights for the attribute 
                //Simple Return failure
                //As the Actual Rights are NotSet
                return errorstatus.failure;
            }

            #endregion

        }

        #endregion

        #region ValidatenTSecurityDescriptorAttribute

        /// <summary>
        /// Validates the Requirements of SecurityDescriptor attribute
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidatenTSecurityDescriptor(AccessRights accessRight,
                                                              AttribsToCheck attr)
        {

            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;

            userForcePwd = false;

            #region Allow Access Requirements Validation

            //validate if msDS_SecurityDescriptor attribute
            //Set the specified Access Rights to validate the Requirements.
            if ((attr == AttribsToCheck.nTSecurityDescriptor) &&
                (accessRight == AccessRights.ACC_SYS_SEC_READ_CONTROL))
            {
                try
                {
                    #region Set Access Rights

                    //To access ntSecurityDescriptor attribute,AccessSystemSecurity and ReadProperty
                    // access rights are required for the user.

                    // set AccessSystemSecurity access rights to access ntSecurity Descriptor
                    bool isSystemSecurityAccess = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                   ClientUserName,
                                                   ClientUserPassword,
                                                   adDomain,
                                                   ActiveDirectoryRights.AccessSystemSecurity,
                                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isSystemSecurityAccess, "AccessRight is allowed.");

                    //Set ReadProperty access right to allow
                    bool isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                  "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                   ClientUserName,
                                                   ClientUserPassword,
                                                   adDomain,
                                                   ActiveDirectoryRights.ReadProperty,
                                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "ReadAccess right is granted");

                    #endregion

                    #region Validation

                    //if the access rights are set successfully ,the below search operation returns
                    //ntSecurityDescriptor 

                    //perform Ldsp Searchproperty
                    SearchResponse searchResult = ADTSHelper.SearchObject(
                                                 "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                 "objectclass=user",
                                                 System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                 new string[] { "nTSecurityDescriptor" }, connection);

                    bool isNtSecDescReturned = searchResult.Entries[0].Attributes.Contains("nTSecurityDescriptor");

                    TestClassBase.BaseTestSite.Assert.IsTrue(isNtSecDescReturned, "the ntSecurityDescriptor is returned");

                    byte[] buffer = (byte[])(searchResult.Entries[0].Attributes["nTSecurityDescriptor"])[0];
                    var securityDescriptor = DtypUtility.DecodeSecurityDescriptor(buffer);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(securityDescriptor, "Security descriptor values stored in Active Directory are in SECURITY_DESCRIPTOR format (see [MS-DTYP] section 2.4.6).");
                    #endregion

                    //Returns success as the attribute is returned 
                    return errorstatus.success;

                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {
                    //Failure has happened while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;
                }

            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else if (((attr == AttribsToCheck.nTSecurityDescriptor) &&
                     ((accessRight != AccessRights.ACC_SYS_SEC_READ_CONTROL))))
            {
                try
                {
                    //if the Access Rights AccessSystemSecurity and ReadProperty are set to deny
                    //ntSecurity descriptor attribute will be never returned.

                    //Deny or remove the AccessSystemSecurity Access for the user
                    bool isReadAccessRemovd = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                   "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                   ClientUserName,
                                                   ClientUserPassword,
                                                   adDomain,
                                                   ActiveDirectoryRights.AccessSystemSecurity,
                                                   AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemovd, "AccessSystemSecurity Right is removed.");

                    //Deny or remove the Read Access permission to the user
                    isReadAccessRemovd = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                  "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                  ClientUserName,
                                                  ClientUserPassword,
                                                  adDomain,
                                                  ActiveDirectoryRights.ReadProperty,
                                                  AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemovd, "ReadAccess Right is removed.");

                    //Search for the ntSecurityDescriptor attribute 
                    //As there are no access rights
                    //The result should be false.
                    //get the response in secDescSearchResponse to validate
                    SearchResponse secDescSearchResponse = ADTSHelper.SearchObject(
                                          "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                          "objectClass=user",
                                          System.DirectoryServices.Protocols.SearchScope.Subtree,
                                          new string[] { "nTSecurityDescriptor" },
                                          connection);

                    TestClassBase.BaseTestSite.Assert.IsNotNull(secDescSearchResponse, "Search operation success");

                    int actualCount = secDescSearchResponse.Entries.Count;


                    #region Rollback

                    //Rollback
                    //perform Roll back immediately.
                    //Set the Access Right that were denied 


                    //Remove the Deny AccessSystemSecurity right 
                    bool isSecurityAccessRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                   AdminUser,
                                                   AdminUserPassword,
                                                   adDomain,
                                                   ActiveDirectoryRights.ReadProperty,
                                                   AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isSecurityAccessRemoved, "AccessSystemSecurity right is denied.");

                    //Set the ReadProperty to allow as normal
                    isReadAccessRemovd = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                                   "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                   AdminUser,
                                                   AdminUserPassword,
                                                   ClientUserName,
                                                   adDomain,
                                                   ActiveDirectoryRights.ReadProperty,
                                                   AccessControlType.Allow);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemovd, "ReadAccess right is allowed.");

                    //Remove the Deny AccessSystemSecurity right
                    isSecurityAccessRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                   ClientUserName,
                                                   ClientUserPassword,
                                                   adDomain,
                                                   ActiveDirectoryRights.AccessSystemSecurity,
                                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemovd, "Deny the AccessSystemSecurity right is removed.");

                    //Set the AccessSystemSecurity to allow for the user
                    isReadAccessRemovd = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                   "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                   ClientUserName,
                                                   ClientUserPassword,
                                                   adDomain,
                                                   ActiveDirectoryRights.AccessSystemSecurity,
                                                   AccessControlType.Allow);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemovd, "AccessSystemSecurity right is allowed.");

                    #endregion


                    //Validate the Requirement
                    //Req:ntSecurity Descriptor should never returned if access rights are set to deny
                    //read the ntSecurityDescriptorattribute from secDescSearchResponse
                    //Validate MS-ADTS-Security_R40
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, actualCount, 40,
                        @"While accessing nTSecurityDescriptor attribute,if the security context of the requestor
                            are not granted these rights (ACCESS_SYSTEM_SECURITY) and (READ_CONTROL),then the value is 
                            treated as does not exist in the returned attributes and the LDAP filter.");

                    return errorstatus.failure;
                }
                //catch protocol specific error
                catch (DirectoryException)
                {

                    //if an exception occurs
                    //after removing the rights
                    //then Rollback
                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    //Rollback the ReadAccess
                    bool isAccessRightsRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadProperty,
                                                                                AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightsRemoved, "Access right is removed.");

                    //Setting the Access Right for the user.
                    bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                                                                "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                ClientUserName,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadProperty,
                                                                                AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Access right is allowed for the user.");


                    //Rollback To Control Access Right

                    //Remove the deny access
                    bool isControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Removed the Deny Access right");

                    //Set the Control access right 
                    isControlAccessSet = ADTSHelper.SetControlAcessRights(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                           ClientUserName,
                                                                           ClientUserPassword,
                                                                           adDomain,
                                                                           guidDS_Repl_ManageE_Topology,
                                                                           ActiveDirectoryRights.ExtendedRight,
                                                                           AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Allow ExtendedRight Control Access.");
                    #endregion

                    //Failure has happened while performing Ldap operation.
                    return errorstatus.failure;
                }

            }

            else
            {
                //if the Access right other than the specified rights for the attribute 
                //Simple Return failure
                //As the Actual Rights are NotSet
                return errorstatus.failure;
            }
            #endregion
        }

        #endregion

        #region ValidateuserPasswordAttribute

        /// <summary>
        /// This method validates the requirements userPasswordAttribute
        /// </summary>
        /// <param name="attr">specifies the attribute name</param>
        /// <param name="accRight">specifies the access right </param>
        /// <param name="ctrlAccRight">specifies the control access right</param>
        /// <param name="fUserPwdSupport">specifies to change the password attribute or not</param>
        /// <returns>returns success if </returns>
        public errorstatus ValidateUserPasswordAttribute(AttribsToCheck attr,
                                                         AccessRights accRight,
                                                         ControlAccessRights ctrlAccRight,
                                                         bool fUserPwdSupport)
        {
            #region userpassword attribute returns

            //Check if Access Rights and control rights are correct for userPassword
            if ((attr == AttribsToCheck.userPassword) &&
                (accRight == AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ) &&
                (fUserPwdSupport == false))
            {

                userForcePwd = false;

                //set to true
                //if attr == AttribsToCheck.userPassword
                //Set userChangePwd to true as user can change password
                if (ctrlAccRight == ControlAccessRights.User_Change_Password)
                {
                    userChangePwd = true;
                    userForcePwd = false;
                }

                //if the ControlAccessRights is User_Force_Change_Password
                //Set userForcePwd = true  as user can  for change password
                if (ctrlAccRight == ControlAccessRights.User_Force_Change_Password)
                {
                    userForcePwd = true;
                    userChangePwd = false;
                }

                //Attribute collection
                List<DirectoryAttribute> dsHeurAttributes = new List<DirectoryAttribute>();

                DirectoryAttributeModification dsHeurAttr = new DirectoryAttributeModification();

                //Constant value Extracted from TD
                dsHeurAttr.Name = "dsheuristics";

                dsHeurAttr.Operation = DirectoryAttributeOperation.Replace;

                //Change the value to false
                dsHeurAttr.Add("00000002");

                //add the value to collection list
                dsHeurAttributes.Add(dsHeurAttr);

                //Modify Result 
                string strModifyResult = ADTSHelper.ModifyObject(
                                             "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," + parsedDN,
                                             dsHeurAttributes, connection);

                TestClassBase.BaseTestSite.Assert.IsNotNull(strModifyResult, "modify opeartion success");
                //Set the ActiveDirectoryRights to ReadProperty
                bool isDsHeuristicsAccess = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                 "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                 ClientUserName,
                                                 ClientUserPassword,
                                                 adDomain,
                                                 ActiveDirectoryRights.ReadProperty,
                                                 AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isDsHeuristicsAccess, "The ActiveDirectoryRights is Readable");

                //Search for user password
                SearchResponse searchPwdAttributeResponse = ADTSHelper.SearchObject(
                                            "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                            "objectClass=*",
                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                            new string[] { "userPassword" },
                                            connection);

                //Check whether the userPassword attribute is existed in the returned search response.
                bool isPwdAttrExist = searchPwdAttributeResponse.Entries.Count > 0;

                //Validate MS-ADTS-Security_R52
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isPwdAttrExist, 52, @"While accessing  userPassword attribute,if 
                                the security context of the requestor is granted access when dSHeuristics.fUserPwdSupport is 
                                false.");

                return (errorstatus)searchPwdAttributeResponse.ResultCode;
            }

            #endregion

            #region userpassword never returns

            //if Rights and Control Access Rights are both set
            else if ((attr == AttribsToCheck.userPassword) &&
                   (accRight != AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ)
                    && (fUserPwdSupport == true))
            {

                try
                {
                    //Set userChangePwd to false as Control Access Rights and Access Right are not set
                    //User cannot change password
                    userChangePwd = false;
                    userForcePwd = false;

                    //userpwdAttribute list 
                    List<DirectoryAttribute> userpwdAttribute = new List<DirectoryAttribute>();

                    //userpwdAttr value
                    DirectoryAttributeModification userpwdAttr = new DirectoryAttributeModification();

                    //Set the property of name to dsheuristics
                    //Value Extracted from ADTS TD.
                    userpwdAttr.Name = "dsheuristics";

                    //Replace the previous property with current value
                    userpwdAttr.Operation = DirectoryAttributeOperation.Replace;

                    //Change the value [Values Extracted from ADTS TD]
                    userpwdAttr.Add("00000002");

                    //Add to the attribute collection
                    userpwdAttribute.Add(userpwdAttr);

                    //Modify Result.
                    string strModifyResult = ADTSHelper.ModifyObject("CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," + parsedDN, userpwdAttribute, connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(strModifyResult, "modify operation success");

                    SearchResponse userPwdSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                "objectClass=user",
                                                System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                new string[] { "userPassword" },
                                                connection);

                    //Check whether the userPassword attribute is existed in the returned search response.
                    bool isPwdAttrExist = userPwdSearchResponse.Entries[0].Attributes.Contains("userPassword");

                    //Validate MS-ADTS-Security_R51
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(isPwdAttrExist, 51, @"While accessing  userPassword attribute,
                                the security context of the requestor is never granted access,When dSHeuristics.fUserPwdSupport 
                                is true. ");

                    //Access the Attribute
                    bool isdsHeursiticsReadAccess = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                    "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    ActiveDirectoryRights.ReadProperty,
                                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isdsHeursiticsReadAccess, "Set the AccessRight to readable");
                    //Search for userPassword attribute
                    userPwdSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                    "objectClass=user",
                                                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                    new string[] { "userPassword" },
                                                    connection);

                    //Reset the value    
                    isPwdAttrExist = true;

                    //Try to access the userPassword from the SearchResponse
                    //the return value should be false.
                    isPwdAttrExist = userPwdSearchResponse.Entries[0].Attributes.Contains("userPassword");

                    //Validate MS-ADTS-Security_R50
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(isPwdAttrExist, 50, @"While accessing  userPassword attribute,
                                When dSHeuristics.fUserPwdSupport is true the requestor is never granted access even if the 
                                security context of the requestor are granted granted these rights RIGHT_DS_READ_PROPERTY.");

                    return errorstatus.failure;

                }

                catch (LdapException ex)
                {
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "An Exception", ex.Message);
                    return errorstatus.failure;
                }

            }

            //Condition to change the AdminPAsswordReset.
            else if ((attr == AttribsToCheck.userPassword) &&//the Attribute should be userPassword
                     (accRight == AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ) &&//it should have ReadProperty Right
                     fUserPwdSupport == false)
            {
                if (ctrlAccRight == ControlAccessRights.User_Force_Change_Password)
                {
                    userForcePwd = true;
                }
                return errorstatus.success;
            }

            //Condition to change the AdminPAsswordReset.
            //ProtocolMessageStructures.AccessRights.NotSet, 
            //ProtocolMessageStructures.ControlAccessRights.User_Force_Change_Password, 
            //ProtocolMessageStructures.AttribsToCheck.userPassword, false
            else if ((attr == AttribsToCheck.userPassword) &&//the Attribute should be userPassword
                     (accRight == AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ) &&//it should have ReadProperty Right
                     fUserPwdSupport == true)
            {
                //set to false 
                userForcePwd = false;

                //end in failure state
                return errorstatus.failure;

            }

            #endregion

            return errorstatus.failure;

        }

        #endregion

        #region ValidatemsDS_ReplAttributeMetaDataAttribute

        /// <summary>
        /// Validates the Requirements of msDS_ReplAttributeMetaData
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="ctrlAccessRight">Specifies the control access Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidateMsDS_ReplAttributeMetaData(AccessRights accessRight,
                                                              ControlAccessRights ctrlAccessRight,
                                                              AttribsToCheck attr)
        {
            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;

            userForcePwd = false;

            #region Allow Access Requirementts Validation

            //validate if msDS_QuotaEffective attribute and RIGHT_DS_READ_PROPERTY and DS_Query_Self_Quota
            //Allow the specifies Access to validate the Requirements.
            if ((attr == AttribsToCheck.msDS_ReplAttributeMetaData) &&
                ((accessRight == AccessRights.RIGHT_DS_READ_PROPERTY) ||
                accessRight == AccessRights.DS_READ_METADATA ||
                (ctrlAccessRight == ControlAccessRights.DS_Replication_Manage_Topology)))
            {
                try
                {
                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    #region Set Access Rights

                    //Set READ AccessRight to the user to access msDS-QuotaEffective attribute.

                    //Allow Access Right to the current user
                    bool isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                    "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    ActiveDirectoryRights.ReadProperty,
                                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Set the AccessRight is readable to current user.");

                    //Search for the attribute
                    SearchResponse replAttributeResponse = ADTSHelper.SearchObject(
                                                     "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "msDS-ReplAttributeMetaData" },
                                                     connection);

                    TestClassBase.BaseTestSite.Assert.IsNotNull(replAttributeResponse, "the replAttributeResponse is not null.");

                    //Set ReadControl AccessRight to Allow.
                    bool isAccessSet = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                "CN=" + ClientUserName + ",cn=users," + parsedDN,
                                                ClientUserName,
                                                ClientUserPassword,
                                                adDomain,
                                                ActiveDirectoryRights.ReadControl,
                                                AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessSet, "Set the ReadControl AccessRight is allowed.");
                    //Search for the attribute
                    replAttributeResponse = ADTSHelper.SearchObject(
                                                    "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                    "objectClass=attributeSchema",
                                                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                    new string[] { "msDS-ReplAttributeMetaData" },
                                                    connection);

                    //Validate MS-ADTS-Security_R408     
                    //After Granting all the specifies Access
                    //Search for the Attribute
                    //The SearchResponse for the Attribute should not be null.
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(replAttributeResponse, 408, @"the authorization
                        for the requester's security context for Checking Control Access Right-Based Access is  If
                        the ACE type is Object Access Allowed the access right RIGHT_DS_CONTROL_ACCESS (CR) is 
                        present in M, and the ObjectType field in the ACE contains a GUID value equal to G, then 
                        grant the requestor the requested control access right (access to  the 
                        msDS-ReplAttributeMetaData attribute.)");

                    //Allow Specifies Control Access right to the user.
                    bool isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "The ControlAccess Right is granted.");
                    #endregion

                    #region Validation

                    //Access msDS-QuotaEffective attribute
                    //Search for the msDS-QuotaEffective attribute 
                    //the result count should be not be 0 as both the access rights and control access rights are granted
                    replAttributeResponse = ADTSHelper.SearchObject(
                                                     "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "msDS-ReplAttributeMetaData" },
                                                     connection);

                    TestClassBase.BaseTestSite.Assert.IsNotNull(replAttributeResponse, "The replAttributeResponse is not null");
                    #endregion

                    return errorstatus.success;

                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {
                    //Failure has happened while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;

                }

            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else
                if ((attr == AttribsToCheck.msDS_ReplAttributeMetaData) &&
                     ((accessRight != AccessRights.RIGHT_DS_READ_PROPERTY) &&
                     (ctrlAccessRight != ControlAccessRights.DS_Replication_Manage_Topology)))
                {
                    try
                    {
                        //Rollback the ReadAccess
                        bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                   AdminUser,
                                                                                   AdminUserPassword,
                                                                                   adDomain,
                                                                                   ActiveDirectoryRights.ReadProperty,
                                                                                   AccessControlType.Deny);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Rollback the ReadAccess Right.");

                        //Setting the Access Right for the user.
                        bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                                                                   "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                   AdminUser,
                                                                                   AdminUserPassword,
                                                                                   ClientUserName,
                                                                                   adDomain,
                                                                                   ActiveDirectoryRights.ReadProperty,
                                                                                   AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Set the ReadProperty Access Right to current user.");
                        #region Remove Accesss

                        //deny Repl_ManageE_Topology Control Access  Right
                        bool isReadControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                        "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                        ClientUserName,
                                                        ClientUserPassword,
                                                        adDomain,
                                                        guidDS_Repl_ManageE_Topology,
                                                        ActiveDirectoryRights.ExtendedRight,
                                                        AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isReadControlAccess, "The Repl_ManageE_Topology Control Access Right is denied.");
                        //Deny READ Access right
                        //Remove Access
                        bool isReadAccessRemoved = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                            "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                            ClientUserName,
                                                            ClientUserPassword,
                                                            adDomain,
                                                            ActiveDirectoryRights.ReadProperty,
                                                            AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemoved, "Removed the ReadProperty Access Right.");

                        bool isAccessSetRemoved = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ClientUserName + ",cn=users," + parsedDN,
                                                                            ClientUserName,
                                                                            ClientUserPassword,
                                                                            adDomain,
                                                                            ActiveDirectoryRights.ReadControl,
                                                                            AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessSetRemoved, "Removed the ReadControl Access Right.");

                        #endregion

                        #region Validation
                        int isReplAttributeMetaDataExist;
                        int retryCount = 0;
                        do
                        {
                            Thread.Sleep(sleepTime);
                            //Search for the specifies attribute after removing both the access right and control access
                            //the count should be 0
                            SearchResponse searchResponseMsDSReplAttributeMetaData = ADTSHelper.SearchObject(
                                                  "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                  "objectClass=attributeSchema",
                                                  System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                  new string[] { "msDS-ReplAttributeMetaData" },
                                                  connection);

                            //check whet ether the specifies attribute is present in the search response returned.
                            //The result should be false as the access right are removed,the attribute should not return. 
                            //after getting the result store it in a variable 
                            isReplAttributeMetaDataExist = searchResponseMsDSReplAttributeMetaData.Entries.Count;
                        } while (isReplAttributeMetaDataExist != 0 && retryCount++ < 10);
                        //Rollback to previous right because if an error or an exception occurs the user will not have access right
                        //as the rights were denied and subsequent test cases will fail. so roll back to previous rights
                        //and then validate

                        #region RollBack the Removed Operations

                        //Rollback to previous Access Rights.
                        //Delete the Denied AccessRight
                        //Set the AccessRight and Control Access Right

                        //Rollback the ReadAccess
                        bool isAccessRightsRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                                   "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                   AdminUser,
                                                                                   AdminUserPassword,
                                                                                   adDomain,
                                                                                   ActiveDirectoryRights.ReadProperty,
                                                                                   AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightsRemoved, "Removed the ReadProperty Access Right.");
                        //Setting the Access Right for the user.
                        bool isAccessRightsAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                                                                   "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                   AdminUser,
                                                                                   AdminUserPassword,
                                                                                   ClientUserName,
                                                                                   adDomain,
                                                                                   ActiveDirectoryRights.ReadProperty,
                                                                                   AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightsAllowed, "Allow the ReadProperty Access Right for current user.");
                        //Rollback To Control Access Right
                        //Remove the deny access
                        bool isControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Repl_ManageE_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Deny the ExtendedRight Access Right.");

                        //Set the Control access right 
                        bool isControlAccessRightsSet = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                                       "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                       ClientUserName,
                                                                                       ClientUserPassword,
                                                                                       adDomain,
                                                                                       guidDS_Repl_ManageE_Topology,
                                                                                       ActiveDirectoryRights.ExtendedRight,
                                                                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightsSet, "The ExtendedRight is allowed.");

                        bool isRemovedAccessRights = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ClientUserName + ",cn=users," + parsedDN,
                                                                                   AdminUser,
                                                                                   AdminUserPassword,
                                                                                   adDomain,
                                                                                   ActiveDirectoryRights.ReadControl,
                                                                                   AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isRemovedAccessRights, "Removed the ReadControl Access Right.");

                        bool isSetAccessRightAllow = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + ClientUserName + ",cn=users," + parsedDN,
                                                                                    AdminUser,
                                                                                    AdminUserPassword,
                                                                                    ClientUserName,
                                                                                    adDomain,
                                                                                    ActiveDirectoryRights.ReadControl,
                                                                                    AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isSetAccessRightAllow, "Allowed the ReadControl Access Right.");
                        #endregion

                        //Rollback is done and validate now.

                        //Validate MS-ADTS-Security_R56
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, isReplAttributeMetaDataExist, 56,
                              @"While accessing  msDS-ReplAttributeMetaData attribute if The security context of the requestor 
                          is not  granted the following rights on the replPropertyMetaData attribute:
                          (RIGHT_DS_READ_PROPERTY) and (RIGHT_DS_REPL_MANAGE_TOPOLOGY by ON!nTSecurityDescriptor),
                          then the value is treated as ""does not exist"" in the returned attributes and the LDAP filter.");


                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, isReplAttributeMetaDataExist, 412, @"the authorization for the requester's security 
                        context for Checking Control Access Right-Based Access is If the ACE type is Object Access 
                        Denied the access right RIGHT_DS_CONTROL_ACCESS (CR) is present in M, and the ObjectType 
                        field in the ACE contains a GUID value equal to G,  then deny the requestor the requested 
                        control access right (access to  the msDS-ReplAttributeMetaData attribute.)");

                        return errorstatus.failure;

                        #endregion

                    }

                    catch (DirectoryException)
                    {

                        #region RollBack the Removed Operations

                        //Rollback to previous Access Rights.
                        //Delete the Denied AccessRight
                        //Set the AccessRight and Control Access Right

                        //Rollback the ReadAccess
                        bool isRemoveAccessRights = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                       "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                       AdminUser,
                                       AdminUserPassword,
                                       adDomain,
                                       ActiveDirectoryRights.ReadProperty,
                                       AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isRemoveAccessRights, "Removed the ReadProperty Access Right.");

                        //Setting the Access Right for the user.
                        bool isAccessRightAllowSet = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                       "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                       AdminUser,
                                       AdminUserPassword,
                                       ClientUserName,
                                       adDomain,
                                       ActiveDirectoryRights.ReadProperty,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowSet, "Allowed the ReadProperty Access Right for current user.");
                        //Rollback To Control Access Right

                        //Remove the deny access
                        bool isControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Repl_ManageE_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Deny the ExtendedRight Access Right.");

                        //Set the Control access right 
                        bool isControlAccessRightSet = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                                       "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                                       ClientUserName,
                                                                                       ClientUserPassword,
                                                                                       adDomain,
                                                                                       guidDS_Repl_ManageE_Topology,
                                                                                       ActiveDirectoryRights.ExtendedRight,
                                                                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightSet, "Allow ExtendedRight Control Access.");

                        isRemoveAccessRights = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ClientUserName + ",cn=users," + parsedDN,
                                                                             AdminUser,
                                                                             AdminUserPassword,
                                                                             adDomain,
                                                                             ActiveDirectoryRights.ReadControl,
                                                                             AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isRemoveAccessRights, "Removed the ReadControl Access Right.");

                        isAccessRightAllowSet = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + ClientUserName + ",cn=users," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                ClientUserName,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadControl,
                                                                                AccessControlType.Allow);

                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowSet, "Allowed the ReadControl Access Right.");
                        #endregion

                        //Failure has happened while performing Ldap operation.
                        return errorstatus.failure;

                    }

                }

            #endregion

            return errorstatus.success;
        }

        #endregion

        #region ValidatemsDS_ReplValueMetaDataAttribute
        /// <summary>
        /// Validates the Requirements of MsDS_ReplValueMetaDat
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="ctrlAccessRight">Specifies the control Access Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidateMsDS_ReplValueMetaData(AccessRights accessRight,
                                                              ControlAccessRights ctrlAccessRight,
                                                              AttribsToCheck attr)
        {
            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;
            userForcePwd = false;

            #region Allow Access Requirementts Validation

            //validate if msDS_QuotaEffective attribute and RIGHT_DS_READ_PROPERTY and DS_Query_Self_Quota
            //Allow the specifies Access to validate the Requirements.
            if ((attr == AttribsToCheck.msDS_ReplValueMetaData) &&
                ((accessRight == AccessRights.RIGHT_DS_READ_PROPERTY) ||
                accessRight == AccessRights.DS_READ_METADATA ||
                (ctrlAccessRight == ControlAccessRights.DS_Replication_Manage_Topology)))
            {
                try
                {
                    #region Set Access Rights

                    //Set READ AccessRight to the user to access msDS_ReplValueMetaData attribute.
                    //Allow Access Right to the current user
                    bool isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                    "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    ActiveDirectoryRights.ReadProperty,
                                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Granted the ReadProperty Access Right to the user.");
                    //Access msDS_ReplValueMetaData attribute
                    //Search for the msDS-QuotaEffective attribute 
                    //the result count should be not be 0 as both the access rights and control access rights are granted
                    SearchResponse replValueReponse = ADTSHelper.SearchObject(
                                                     "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "msDS_ReplValueMetaData" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(replValueReponse, "the replValueReponse is not null");
                    #endregion

                    #region Control Access Right

                    //Set DS_Repl_ManageE_Topology control access right to access msDS-ReplValueMetaData attribute
                    //Allow Specified Control Access right to the user.
                    bool isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allow the ExtendedRight to the user.");
                    replValueReponse = ADTSHelper.SearchObject(
                                                     "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "msDS_ReplValueMetaData" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(replValueReponse, "the replValueReponse is not null");
                    #endregion

                    return errorstatus.success;

                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {
                    //Failure has happened while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;

                }

            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else if ((attr == AttribsToCheck.msDS_ReplValueMetaData) &&
                     (accessRight != AccessRights.RIGHT_DS_READ_PROPERTY) &&
                     (ctrlAccessRight != ControlAccessRights.DS_Replication_Manage_Topology))
            {
                try
                {

                    #region Remove Accesss

                    //deny Repl_ManageE_Topology Control Access Right
                    bool isReadControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                    "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    guidDS_Repl_ManageE_Topology,
                                                    ActiveDirectoryRights.ExtendedRight,
                                                    AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadControlAccess, "Deny the ExtendedRight to the user.");
                    //Deny READ Access right
                    //Remove Acccess
                    bool isReadAccessRemoved = ADTSHelper.SetAccessRights(PdcFqdn, "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                          ClientUserName,
                                                                          ClientUserPassword,
                                                                          adDomain,
                                                                          ActiveDirectoryRights.ReadProperty,
                                                                          AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemoved, "Removed the ReadProperty Access Right to the user.");
                    #endregion

                    #region Validation
                    int replyattributeMetaDataCount ;
                    int retryCount = 0;
                    do
                    {
                        Thread.Sleep(2000);
                        //Search for the specifies attribute after removing both the access right and control access
                        //the count should be 0
                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Debug, "Try to query the msDS_ReplValueMetaData attribute of CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration...");
                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Debug, string.Format("The server should return 0 entries. Try {0}/10.", retryCount));
                        SearchResponse searchResponseMsDSReplAttributeMetaData = ADTSHelper.SearchObject("CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                                        "objectClass=attributeSchema",
                                                                                                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                                                        new string[] { "msDS_ReplValueMetaData" },
                                                                                                        connection);

                        //check whet ether the specifies attribute is present in the search response returned.
                        //The result should be false as the access rights are removed,the attribute should not return. 

                        //after getting the result store it in a variable 
                        replyattributeMetaDataCount = searchResponseMsDSReplAttributeMetaData.Entries.Count;
                    } while (replyattributeMetaDataCount != 0 && retryCount++ < 10);
                    //Rollback to previous right because if an error or an exception occurs the user will not have access right
                    //as the rights were denied and subsequent test cases will fail. so roll back to previous rights
                    //and then validate

                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    //Rollback the ReadAccess
                    bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                               "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                               AdminUser,
                                                                               AdminUserPassword,
                                                                               adDomain,
                                                                               ActiveDirectoryRights.ReadProperty,
                                                                               AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Removed the ReadProperty Access Right to the user.");
                    //Setting the Access Right for the user.
                    bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                ClientUserName,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadProperty,
                                                                                AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Set the ReadProperty Access Right to the user.");
                    //Rollback To Control Access Right

                    //Remove the deny access
                    bool isControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                   ClientUserName,
                                                                                   ClientUserPassword,
                                                                                   adDomain,
                                                                                   guidDS_Repl_ManageE_Topology,
                                                                                   ActiveDirectoryRights.ExtendedRight,
                                                                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Deny the ExtendedRight Access Right to the user.");
                    //Set the Control access right 
                    bool isControlAccessRightSet = ADTSHelper.SetControlAcessRights(PdcFqdn, "CN=Repl-Property-Meta-Data,CN=Schema,CN=Configuration," + parsedDN,
                                                                                    ClientUserName,
                                                                                    ClientUserPassword,
                                                                                    adDomain,
                                                                                    guidDS_Repl_ManageE_Topology,
                                                                                    ActiveDirectoryRights.ExtendedRight,
                                                                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightSet, "Allow the ExtendedRight to the user.");
                    #endregion

                    //Rollback is done and validate now.


                    //Validate MS-ADTS-Security_R62
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, replyattributeMetaDataCount, 62,
                          @"While accessing  msDS-ReplValueMetaData attribute  if The security context of the requestor is 
                          not granted the following rights on the replPropertyMetaData attribute: (RIGHT_DS_READ_PROPERTY)
                          and (RIGHT_DS_REPL_MANAGE_TOPOLOGY by ON!nTSecurityDescriptor) ,then the value is treated as does
                          not exist in the returned attributes and the LDAP filter.");

                    return errorstatus.failure;

                    #endregion

                }

                catch (DirectoryException)
                {
                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    //Rollback the ReadAccess
                    bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                                "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadProperty,
                                                                                AccessControlType.Deny);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Deny the ReadProperty Access Right.");

                    //Setting the Access Right for the user.
                    bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                               AdminUser,
                                                                               AdminUserPassword,
                                                                               ClientUserName,
                                                                               adDomain,
                                                                               ActiveDirectoryRights.ReadProperty,
                                                                               AccessControlType.Allow);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allow the ReadProperty Access Right to the user.");

                    //Rollback To Control Access Right

                    //Remove the deny access
                    bool isControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                                  ClientUserName,
                                                                                  ClientUserPassword,
                                                                                  adDomain,
                                                                                  guidDS_Repl_ManageE_Topology,
                                                                                  ActiveDirectoryRights.ExtendedRight,
                                                                                  AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessSet, "Remove the ExtendedRight Access Right to the user.");
                    //Set the Control access right 
                    bool isControlAccessRightSet = ADTSHelper.SetControlAcessRights(PdcFqdn, "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                                    ClientUserName,
                                                                                    ClientUserPassword,
                                                                                    adDomain,
                                                                                    guidDS_Repl_ManageE_Topology,
                                                                                    ActiveDirectoryRights.ExtendedRight,
                                                                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightSet, "Allow the ExtendedRight to the user.");
                    #endregion

                    //Failure has happened while performing Ldap operation.
                    return errorstatus.failure;

                }

            }

            #endregion

            return errorstatus.success;
        }

        #endregion

        #region Validate RIGHT_DS_READ_PROPERTY Access Right Requiremnts

        /// <summary>
        /// This method validates all the requirements where only Read Access Right is required
        /// </summary>
        /// <param name="accessRights">AccessRights</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        public void ValidateReadAccessControl(AccessRights accessRights)
        {

            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;
            userForcePwd = false;

            //if the AccessRights is RIGHT_DS_READ_PROPERTY
            if (accessRights == AccessRights.RIGHT_DS_READ_PROPERTY)
            {

                #region SearchAttributes

                //The requirements in this block will be validated  based on the response of the following
                //Search operation
                //the procedure of validation is similar for all the requirements
                //ProcessSearchRespone returns the actual value as out parameter.
                //based on the result of ProcessSearchRespone the requirements are validated.

                //Search for the properties of attribute for the given distinguishname of LdapConnection.
                SearchResponse searchResponseUser = ADTSHelper.SearchObject("CN=Users," + parsedDN,
                                                                             "(objectClass=user)",
                                                                             System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                             new string[] { "name", "objectclass", "primaryGroupID" },
                                                                             connection);

                //local variable
                object expectedResult = string.Empty;

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseUser, DomainAdministratorName, "name", out expectedResult);

                //Validate MS-ADTS-Security_R518
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(DomainAdministratorName, expectedResult.ToString(), 518,
                    @"An Administrator has name as Administrator");

                bool checkResult;
                string msg;

                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Administrator");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseUser, DomainAdministratorName, "objectClass", "user", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R519
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    519,
                    @"An Administrator has objectClass as user");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseUser, DomainAdministratorName, "primaryGroupID", out expectedResult);

                //Validate MS-ADTS-Security_R521
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(513, Convert.ToInt32(expectedResult), 521,
                    @"An Administrator has primaryGroupID as 513 (Domain Users)");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseUser, "guest", "name", out expectedResult);

                //Validate MS-ADTS-Security_R522
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Guest", expectedResult.ToString(), 522,
                    @"A guest must have name as Guest");

                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Guest");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseUser, "guest", "objectClass", "user", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R523
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    523,
                    @"An guest MUST have objectClass as user");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseUser, "guest", "primaryGroupID", out expectedResult);

                //Validate MS-ADTS-Security_R525
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(514, Convert.ToInt32(expectedResult), 525,
                    @"An Administrator has primaryGroupID as 513 (Domain Users)");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseUser, "krbtgt", "name", out expectedResult);

                //Validate MS-ADTS-Security_R526
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("krbtgt", expectedResult.ToString(), 526,
                    @" The Key Distribution Center Service Account has name as krbtgt");

                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of krbtgt");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseUser, "krbtgt", "objectClass", "user", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R527
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    527,
                    @"The Key Distribution Center Service Account has objectClass as user");
                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseUser, "krbtgt", "primaryGroupID", out expectedResult);

                //Validate MS-ADTS-Security_R529
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(513, Convert.ToInt32(expectedResult), 529,
                    @"The Key Distribution Center Service Account has primaryGroupID as 513 (Domain Users)");

                //The requirements in this block will be validated  based on the response of the following
                //Search operation
                //the procedure of validation is similar for all the requirements
                //ProcessSearchRespone returns the actual value as out parameter.
                //based on the result of ProcessSearchRespone the requirements are validated.

                //Search for the name", "objectclass", "primaryGroupID properties of attributes
                SearchResponse searchResponseGroup = ADTSHelper.SearchObject("CN=Users," + parsedDN,
                                                                            "objectclass=group",
                                                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                            new string[] { "name", "objectclass", "primaryGroupID" },
                                                                            connection);

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Cert Publishers", "name", out expectedResult);

                //Validate MS-ADTS-Security_R530
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Cert Publishers", expectedResult.ToString(), 530,
                    @"The Cert Publishers has name as Cert Publishers  ");

                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Cert Publishers.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Cert Publishers", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R531
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    531,
                    @"The Cert Publishers has objectClass as group");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Cert Publishers", "primaryGroupID", out expectedResult);

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Domain Admins", "name", out expectedResult);

                //Validate MS-ADTS-Security_R534
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Domain Admins", expectedResult.ToString(), 534,
                    @"The Domain Administrators have name as Domain Admins");

                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Domain Admins.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Domain Admins", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R535
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    535,
                    @"The Domain Admins has objectClass as group");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Domain Computers", "name", out expectedResult);

                //Validate MS-ADTS-Security_R538
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Domain Computers", expectedResult.ToString(), 538,
                    @"The Domain Computers has name as Domain Computers");
                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Domain Computers.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Domain Computers", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R539
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    539,
                    @"The Domain Computers has objectClass as group");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Domain Controllers", "name", out expectedResult);

                //Validate MS-ADTS-Security_R542
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Domain Controllers", expectedResult.ToString(), 542,
                    @"The Domain Controllers have name as Domain Controllers ");
                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Domain Controllers.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Domain Controllers", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R543
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    543,
                    @"The Domain Controllers has objectClass as group");
                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Domain Guests", "name", out expectedResult);

                //Validate MS-ADTS-Security_R546
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Domain Guests", expectedResult.ToString(), 546,
                    @"The Domain Guests have name as Domain Guests");
                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Domain Guests.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Domain Guests", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R547
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    547,
                    @"The Domain Guests has objectClass as group");
                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Domain Users", "name", out expectedResult);

                //Validate MS-ADTS-Security_R550
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Domain Users", expectedResult.ToString(), 550,
                    @"The Domain Users have name as Domain Users");

                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Domain Users.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Domain Users", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R551
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    551,
                    @"The Domain Users has objectClass as group");
                expectedResult = new object();

                //554 part-1
                //process the Search result  for Enterprise Admins in other NCRoot other than Forest Root Domain, 
                //it should fail
                ADTSHelper.ProcessSearchRespone(searchResponseUser, "Enterprise Admins", "name", out expectedResult);

                //the result should be null
                TestClassBase.BaseTestSite.Assert.IsNull(expectedResult,
                    @"Enterprise Admin does not exist only other than Forest Root Domain.");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Enterprise Admins", "name", out expectedResult);

                //554 part-2
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Enterprise Admins", expectedResult.ToString(), 554,
                    @"The Enterprise Administrators exist only in Forest Root Domain.");

                //Validate MS-ADTS-Security_R555
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Enterprise Admins", expectedResult.ToString(), 555,
                    @"The Enterprise Administrators have name as Enterprise Admins");
                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Enterprise Admins.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Enterprise Admins", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R556
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    556,
                    @"The Enterprise Admins has objectClass as group");
                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Group Policy Creator Owners", "name", out expectedResult);

                //Validate MS-ADTS-Security_R560
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Group Policy Creator Owners", expectedResult.ToString(), 560,
                    @"The Group Policy Creator Owners have name as Group Policy Creator Owners");

                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Group Policy Creator Owners.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Group Policy Creator Owners", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R561
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    561,
                    @"The Group Policy Creator Owners has objectClass as group");

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "RAS and IAS Servers", "name", out expectedResult);

                //Validate MS-ADTS-Security_R564
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("RAS and IAS Servers", expectedResult.ToString(), 564,
                    @"The RAS and IAS servers have name as RAS and IAS Servers");

                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of RAS and IAS Servers.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "RAS and IAS Servers", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R565
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    565,
                    @"The RAS and IAS Servers has objectClass as group");
                
                //Requirements specific to WindowsServer2008 only

                #region WindowsServer2008 Server

                //The following attributes will be present from windows server 2008 onwards.
                //Windows 2008 specific requirements
                if (PDCOSVersion > Common.ServerVersion.Win2003)
                {
                    ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Read-only Domain Controllers", "name", out expectedResult);

                    //Validate MS-ADTS-Security_R568
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Read-only Domain Controllers", expectedResult.ToString(), 568,
                        @"The Read-Only Domain Controllers have name as Read-Only Domain Controllers");
                    //process the search Result for the expected property. 
                    TestClassBase.BaseTestSite.Log.Add(
                        LogEntryKind.Comment,
                        "Verify the objectClass of Read-only Domain Controllers.");
                    checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Read-only Domain Controllers", "objectClass", "group", out msg);
                    TestClassBase.BaseTestSite.Log.Add(
                        LogEntryKind.Comment,
                        msg);
                    //Validate MS-ADTS-Security_R569
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                        checkResult,
                        569,
                        @"The Read-only Domain Controllers has objectClass as group");
                    ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Enterprise Read-only Domain Controllers", "name", out expectedResult);

                    //Validate MS-ADTS-Security_R572
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Enterprise Read-only Domain Controllers", expectedResult.ToString(), 572,
                        @"The Enterprise Read-Only Domain Controllers have name as Enterprise Read-Only Domain Controllers");
                    
                    //process the search Result for the expected property. 
                    TestClassBase.BaseTestSite.Log.Add(
                        LogEntryKind.Comment,
                        "Verify the objectClass of Enterprise Read-only Domain Controllers.");
                    checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Enterprise Read-only Domain Controllers", "objectClass", "group", out msg);
                    TestClassBase.BaseTestSite.Log.Add(
                        LogEntryKind.Comment,
                        msg);
                    //Validate MS-ADTS-Security_R573
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                        checkResult,
                        573,
                        @"The Enterprise Read-only Domain Controllers has objectClass as group");
                }

                #endregion

                //process the search Result for the expected property. 
                ADTSHelper.ProcessSearchRespone(searchResponseGroup, "Schema Admins", "name", out expectedResult);

                //Validate MS-ADTS-Security_R576 and MS-ADTS-Security_R577
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Schema Admins", expectedResult.ToString(), 576,
                    @"The Schema Admins group exists only in forest domain");

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("Schema Admins", expectedResult.ToString(), 577,
                    @"The Schema Admins have name as schema admins");
                //process the search Result for the expected property. 
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Verify the objectClass of Schema Admins.");
                checkResult = ADTSHelper.SearchResponseHasValue(searchResponseGroup, "Schema Admins", "objectClass", "group", out msg);
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    msg);
                //Validate MS-ADTS-Security_R578
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                    checkResult,
                    578,
                    @"The Schema Admins has objectClass as group");
                //Search for name attribute for given dn of ldap connection
                SearchResponse searchResponse1 = ADTSHelper.SearchObject(parsedDN, "name=ForeignSecurityPrincipals",
                                                                           System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                           new string[] { "name" }, connection);

                //Validate MS-ADTS-Security_R582
                TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(searchResponse1, 582, @"In AD DS, each Domain NC contains a 
                                well-known Foreign Security Principals container");

                #region AdminSDHolder

                //Search for AdminSDHolder.
                SearchResponse isAdminSDHolder = ADTSHelper.SearchObject("CN=AdminSDHolder,CN=System," + parsedDN,
                                                                         "objectClass=container",
                                                                         System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                         new string[] { "name" },
                                                                         connection);

                //process for AdminSDHolder
                ADTSHelper.ProcessSearchRespone(isAdminSDHolder, "AdminSDHolder", "name", out expectedResult);

                //Validate MS-ADTS-Security_R138
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>("AdminSDHolder", expectedResult.ToString(),
                     138, @"The AdminSDHolder object is of class container and has a DN of CN=AdminSDHolder,
                      CN=System,<Domain NC DN>.");

                #endregion

                #endregion

                #region Search Secret Attributes

                //The requirements in this block will be validated  based on the response of the following
                //Search operation
                //the procedure of validation is similar for all the requirements
                //ProcessSearchRespone returns the actual value as out parameter.
                //based on the result of ProcessSearchRespone the requirements are validated.

                SearchResponse resultSearchResponse;

                //Search for the peklist properties.
                resultSearchResponse = ADTSHelper.SearchObject(parsedDN,
                                                "objectclass=domain",
                                                System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                   new string[] { "peklist" }, connection);

                //peklist attributes should not accessible
                //Validating peklist attribute access
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("peklist"), 10085, @"While accessing  
                                pekList  attribute,Access is never  granted to the security context of the requestor. ");

                //currentValue attributes should not accessible
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                            "objectclass=user",
                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                             new string[] { " currentValue" }, connection);

                //Validating currentValue attribute 
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("currentValue"), 10086, @"While 
                                  accessing  currentValue attribute,Access is never granted to the security context of the 
                                  requestor. ");

                //Search for dBCSPwd properties
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                             "objectclass=user",
                                             System.DirectoryServices.Protocols.SearchScope.Subtree,
                                              new string[] { "dBCSPwd" }, connection);

                //dBCSPwd Should not accessible.
                //Validating dBCSPwd attribute access
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("dBCSPwd"), 10087, @"While 
                                accessing  dBCSPwd  attribute,Access is never granted to the security context of the requestor. ");

                //Searching for unicodePwd
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                           "objectclass=user",
                                           System.DirectoryServices.Protocols.SearchScope.Subtree,
                                            new string[] { " unicodePwd" }, connection);

                //Validate unicodePwd attribute access.
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("unicodePwd"), 10088, @"While 
                                accessing  unicodePwd  attribute,Access is never granted to the security context of the requestor. ");

                //Searching for ntPwdHistory Attribute.
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                             "objectclass=user",
                                             System.DirectoryServices.Protocols.SearchScope.Subtree,
                                              new string[] { " ntPwdHistory" }, connection);

                //Validating ntPwdHistory attribute access
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("ntPwdHistory"), 10089, @"While 
                                accessing  ntPwdHistory  attribute,Access is never granted to the security context of the 
                                requestor. ");

                //Searching for priorValue Attribute.
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                             "objectclass=user",
                                             System.DirectoryServices.Protocols.SearchScope.Subtree,
                                              new string[] { " priorValue" }, connection);

                //Validating priorValue attribute access
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("priorValue"), 10090, @"While 
                                accessing  priorValue  attribute,Access is never granted to the security context of the 
                                requestor. ");

                //Searching for supplementalCredentials attibute
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                 "objectclass=user",
                                                 System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                  new string[] { " supplementalCredentials" }, connection);

                //Validating supplementalCredentials attribute access
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("supplementalCredentials"), 10091, @"While 
                                accessing  supplementalCredentials  attribute,Access is never granted to the security context 
                                of the requestor. ");

                //Searching for lmPwdHistory attribute 
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                            "objectclass=user",
                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                             new string[] { " lmPwdHistory" }, connection);

                //Validating lmPwdHistory attribute 
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("lmPwdHistory"), 10094, @"While 
                                  accessing  lmPwdHistory attribute,Access is never granted to the security context of the 
                                  requestor. ");

                //Searching for msDS-ExecuteScriptPassword attribute 
                resultSearchResponse = ADTSHelper.SearchObject("CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                            "objectclass=user",
                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                             new string[] { " msDS-ExecuteScriptPassword" }, connection);

                //Validating msDS-ExecuteScriptPassword attribute 
                TestClassBase.BaseTestSite.CaptureRequirementIfIsFalse(resultSearchResponse.Entries[0].Attributes.Contains("msDS-ExecuteScriptPassword"), 10098, @"While 
                                  accessing  msDS-ExecuteScriptPassword attribute,Access is never granted to the security context of the 
                                  requestor. ");

                #endregion

                #region ControlAccessRightsGuid's

                //The requirements in this block will be validated  based on the response of the following
                //Search operation
                //the procedure of validation is similar for all the requirements
                //ProcessSearchRespone returns the actual value as out parameter.
                //based on the result of ProcessSearchRespone the requirements are validated.

                //Temporary Ldapconnection instance used to search ControlAccess Guids           
                LdapConnection tempLdapConnection = new LdapConnection(new LdapDirectoryIdentifier(PdcFqdn), new NetworkCredential(ClientUserName, ClientUserPassword, adDomain), AuthType.Basic);

                //Searching for Control access rights.
                SearchResponse searchControlAccessGuids = ADTSHelper.SearchObject("CN=Extended-Rights,CN=Configuration," + parsedDN,
                                                                               "objectclass=ControlAccessRight",
                                                                               System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                               new string[] { "rightsGuid" },
                                                                               tempLdapConnection);

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Self-Membership", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "bf9679c0-0de6-11d0-a285-00aa003049e2",
                    expectedResult.ToString(),
                    325,
                    @"The Validated write right symbol Self-Membership has an identifying GUID used in 
                    ACE as bf9679c0-0de6-11d0-a285-00aa003049e2 (member attribute)");

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "bf9679c0-0de6-11d0-a285-00aa003049e2",
                    expectedResult.ToString(),
                    587,
                    @"The Self-Membership Validated write right symbol has an Identifying GUID used in ACE as 
                    bf9679c0-0de6-11d0-a285-00aa003049e2 (member attribute))");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Add-GUID", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "440820ad-65b4-11d1-a3da-0000f875ae0d",
                    expectedResult.ToString(),
                    346,
                    @"TThe control Access Right Symbol Add-GUID has an identifying GUID in ACE as 
                    440820ad-65b4-11d1-a3da-0000f875ae0d");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Allocate-Rids", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "1abd7cf8-0a99-11d1-adbb-00c04fd8d5cd",
                    expectedResult.ToString(),
                    347,
                    @"The control Access Right Symbol  Allocate-Rids has an identifying GUID in ACE as 
                    1abd7cf8-0a99-11d1-adbb-00c04fd8d5cd");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Allowed-To-Authenticate", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "68B1D179-0D15-4d4f-AB71-46152E79A7BC",
                    expectedResult.ToString(),
                    348,
                    @"The control Access Right Symbol Allowed-To-Authenticate has an identifying GUID in ACE as 
                    68b1d179-0d15-4d4f-ab71-46152e79a7bc");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Apply-Group-Policy", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "edacfd8f-ffb3-11d1-b41d-00a0c968f939",
                    expectedResult.ToString(),
                    349,
                    @"The control Access Right Symbol Apply-Group-Policy has an identifying GUID in ACE as 
                    edacfd8f-ffb3-11d1-b41d-00a0c968f939");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Certificate-Enrollment", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "0e10c968-78fb-11d2-90d4-00c04f79dc55",
                    expectedResult.ToString(),
                    350,
                    @"The control Access Right Symbol Certificate-Enrollment has an identifying GUID in ACE as 
                    0e10c968-78fb-11d2-90d4-00c04f79dc55");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Change-Domain-Master", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "014bf69c-7b3b-11d1-85f6-08002be74fab",
                    expectedResult.ToString(),
                    351,
                    @"The control Access Right Symbol Change-Domain-Master has an identifying GUID in ACE as 
                    014bf69c-7b3b-11d1-85f6-08002be74fab");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Change-Infrastructure-Master", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "cc17b1fb-33d9-11d2-97d4-00c04fd8d5cd",
                    expectedResult.ToString(),
                    352,
                    @"The control Access Right Symbol Change-Infrastructure-Master has an identifying GUID in ACE 
                    as cc17b1fb-33d9-11d2-97d4-00c04fd8d5cd");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Change-PDC", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "bae50096-4752-11d1-9052-00c04fc2d4cf",
                    expectedResult.ToString(),
                    353,
                    @"The control Access Right Symbol Change-PDC has an identifying GUID in ACE as 
                    bae50096-4752-11d1-9052-00c04fc2d4cf");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Change-Rid-Master", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "d58d5f36-0a98-11d1-adbb-00c04fd8d5cd",
                    expectedResult.ToString(),
                    354,
                    @"The control Access Right Symbol  Change-Rid-Master has an identifying GUID in ACE as 
                    d58d5f36-0a98-11d1-adbb-00c04fd8d5cd");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Change-Schema-Master", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "e12b56b6-0a95-11d1-adbb-00c04fd8d5cd",
                    expectedResult.ToString(),
                    355,
                    @"The control Access Right Symbol Change-Schema-Master  has an identifying GUID in ACE as 
                    e12b56b6-0a95-11d1-adbb-00c04fd8d5cd");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Create-Inbound-Forest-Trust", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "e2a36dc9-ae17-47c3-b58b-be34c55ba633",
                    expectedResult.ToString(),
                    356,
                    @"The control Access Right Symbol  Create-Inbound-Forest-Trust has an identifying GUID in ACE as 
                    e2a36dc9-ae17-47c3-b58b-be34c55ba633");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Do-Garbage-Collection", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "fec364e0-0a98-11d1-adbb-00c04fd8d5cd",
                    expectedResult.ToString(),
                    357,
                    @"The control Access Right Symbol  Do-Garbage-Collection has an identifying GUID in ACE as 
                    fec364e0-0a98-11d1-adbb-00c04fd8d5cd");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Domain-Administer-Server", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "ab721a52-1e2f-11d0-9819-00aa0040529b",
                    expectedResult.ToString(),
                    358,
                    @"The control Access Right Symbol Domain-Administer-Server has an identifying GUID in ACE as 
                    ab721a52-1e2f-11d0-9819-00aa0040529b");

                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Check-Stale-Phantoms", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "69ae6200-7f46-11d2-b9ad-00c04f79f805",
                    expectedResult.ToString(),
                    359,
                    @"The control Access Right Symbol  DS-Check-Stale-Phantoms has an identifying GUID in ACE as 
                    69ae6200-7f46-11d2-b9ad-00c04f79f805");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Execute-Intentions-Script", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "2f16c4a5-b98e-432c-952a-cb388ba33f2e",
                    expectedResult.ToString(),
                    360,
                    @"The control Access Right Symbol DS-Execute-Intentions-Script  has an identifying GUID in ACE as 
                    2f16c4a5-b98e-432c-952a-cb388ba33f2e");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Install-Replica", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "9923a32a-3607-11d2-b9be-0000f87a36b2",
                    expectedResult.ToString(),
                    361,
                    @"The control Access Right Symbol  DS-Install-Replica has an identifying GUID in ACE as 
                    9923a32a-3607-11d2-b9be-0000f87a36b2");

                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Query-Self-Quota", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "4ecc03fe-ffc0-4947-b630-eb672a8a9dbc",
                    expectedResult.ToString(),
                    362,
                    @"The control Access Right Symbol  DS-Query-Self-Quota has an identifying GUID in ACE as 
                    4ecc03fe-ffc0-4947-b630-eb672a8a9dbc");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Replication-Get-Changes", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "1131f6aa-9c07-11d1-f79f-00c04fc2dcd2",
                    expectedResult.ToString(),
                    363,
                    @"The control Access Right Symbol  DS-Replication-Get-Changes has an identifying GUID in ACE as 
                    1131f6aa-9c07-11d1-f79f-00c04fc2dcd2");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Replication-Get-Changes-All", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "1131f6ad-9c07-11d1-f79f-00c04fc2dcd2",
                    expectedResult.ToString(),
                    364,
                    @"The control Access Right Symbol  DS-Replication-Get-Changes-All has an identifying GUID in ACE as 
                    1131f6ad-9c07-11d1-f79f-00c04fc2dcd2");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Replication-Manage-Topology", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "1131f6ac-9c07-11d1-f79f-00c04fc2dcd2",
                    expectedResult.ToString(),
                    366,
                    @"The control Access Right Symbol  DS-Replication-Manage-Topology has an identifying GUID in ACE as 
                    1131f6ac-9c07-11d1-f79f-00c04fc2dcd2");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Replication-Monitor-Topology", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "f98340fb-7c5b-4cdb-a00b-2ebdfa115a96",
                    expectedResult.ToString(),
                    367,
                    @"The control Access Right Symbol DS-Replication-Monitor-Topology  has an identifying GUID in ACE as 
                    f98340fb-7c5b-4cdb-a00b-2ebdfa115a96");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Replication-Synchronize", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "1131f6ab-9c07-11d1-f79f-00c04fc2dcd2",
                    expectedResult.ToString(),
                    368,
                    @"The control Access Right Symbol  DS-Replication-Synchronize has an identifying GUID in ACE as 1
                    131f6ab-9c07-11d1-f79f-00c04fc2dcd2");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Enable-Per-User-Reversibly-Encrypted-Password", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "05c74c5e-4deb-43b4-bd9f-86664c2a7fd5",
                    expectedResult.ToString(),
                    370,
                    @"The control Access Right Symbol Enable-Per-User-Reversibly-Encrypted-Password  has an identifying GUID in ACE as 
                    05c74c5e-4deb-43b4-bd9f-86664c2a7fd5");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Generate-RSoP-Logging", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "b7b1b3de-ab09-4242-9e30-9980e5d322f7",
                    expectedResult.ToString(),
                    371,
                    @"The control Access Right Symbol Generate-RSoP-Logging  has an identifying GUID in ACE as 
                    b7b1b3de-ab09-4242-9e30-9980e5d322f7");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Generate-RSoP-Planning", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "b7b1b3dd-ab09-4242-9e30-9980e5d322f7",
                    expectedResult.ToString(),
                    372,
                    @"The control Access Right Symbol Generate-RSoP-Planning has an identifying GUID in ACE as 
                    b7b1b3dd-ab09-4242-9e30-9980e5d322f7");

                if (PDCOSVersion == ServerVersion.Win2008R2)
                {
                    //process the search Result for the expected guid. 
                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Manage-Optional-Features", "rightsGuid", out expectedResult);

                    //Verify MS-ADTS-Security_R10664             
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                        "7c0e2a7c-a419-48e4-a995-10180aad54dd",
                        expectedResult.ToString(),
                        10664,
                        @"The control Access Right Symbol Manage-Optional-Features has an identifying GUID in ACE as 
                        7c0e2a7c-a419-48e4-a995-10180aad54dd");
                }
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Migrate-SID-History", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "BA33815A-4F93-4c76-87F3-57574BFF8109",
                    expectedResult.ToString(),
                    373,
                    @"The control Access Right Symbol Migrate-SID-History has an identifying GUID in ACE as 
                    ba33815a-4f93-4c76-87f3-57574bff8109");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Open-Connector", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "b4e60130-df3f-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    374,
                    @"The control Access Right Symbol msmq-Open-Connector has an identifying GUID in ACE as 
                    b4e60130-df3f-11d1-9c86-006008764d0e");

                //process the search Result for the expected guid. 
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Peek", "rightsGuid", out expectedResult);

                //Search in Configuration NC.
                //non-configurable values
                DirectoryEntry tempEntry = new DirectoryEntry(string.Format("LDAP://{0}/CN=msmq-Peek,CN=Extended-Rights,CN=Configuration,{1}", PdcFqdn, parsedDN));

                //Check for the presence of rightsGuid
                if (tempEntry.Properties.Contains("rightsGuid"))
                {
                    expectedResult = tempEntry.Properties["rightsGuid"].Value;
                }
                //reset to null
                tempEntry = null;

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "06bd3201-df3e-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    375,
                    @"The control Access Right Symbol msmq-Peek has an identifying GUID in ACE as 
                    06bd3201-df3e-11d1-9c86-006008764d0e");

                //process the easrch response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Peek-computer-Journal", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "4b6e08c3-df3c-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    376,
                    @"The control Access Right Symbol msmq-Peek-computer-Journal has an identifying GUID in ACE as 
                    4b6e08c3-df3c-11d1-9c86-006008764d0e");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Peek-Dead-Letter", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "4b6e08c1-df3c-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    377,
                    @"The control Access Right Symbol msmq-Peek-Dead-Letter has an identifying GUID in ACE as 
                    4b6e08c1-df3c-11d1-9c86-006008764d0e");

                //process the easrch response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Receive", "rightsGuid", out expectedResult);

                //Seacrh for extended right
                tempEntry = new DirectoryEntry(string.Format("LDAP://{0}/CN=msmq-Receive,CN=Extended-Rights,CN=Configuration,{1}", PdcFqdn, parsedDN));

                //Check for the presence of specifies Guid.
                if (tempEntry.Properties.Contains("rightsGuid"))
                {
                    expectedResult = tempEntry.Properties["rightsGuid"].Value;
                }

                tempEntry = null;

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "06bd3200-df3e-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    378,
                    @"The control Access Right Symbol msmq-Receive has an identifying GUID in ACE as 
                    06bd3200-df3e-11d1-9c86-006008764d0e");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Receive-computer-Journal", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "4b6e08c2-df3c-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    379,
                    @"The control Access Right Symbol msmq-Receive-computer-Journal has an identifying GUID in ACE as 
                    4b6e08c2-df3c-11d1-9c86-006008764d0e");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Receive-Dead-Letter", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "4b6e08c0-df3c-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    380,
                    @"The control Access Right Symbol msmq-Receive-Dead-Letter has an identifying GUID in ACE as 
                    4b6e08c0-df3c-11d1-9c86-006008764d0e");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Receive-journal", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "06bd3203-df3e-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    381,
                    @"The control Access Right Symbol msmq-Receive-journal has an identifying GUID in ACE as 
                    06bd3203-df3e-11d1-9c86-006008764d0e");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "msmq-Send", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "06bd3202-df3e-11d1-9c86-006008764d0e",
                    expectedResult.ToString(),
                    382,
                    @"The control Access Right Symbol msmq-Send has an identifying GUID in ACE as 
                    06bd3202-df3e-11d1-9c86-006008764d0e");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Open-Address-Book", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "a1990816-4298-11d1-ade2-00c04fd8d5cd",
                    expectedResult.ToString(),
                    383,
                    @"The control Access Right Symbol Open-Address-Book has an identifying GUID in ACE as 
                    a1990816-4298-11d1-ade2-00c04fd8d5cd");

                //The following requirements are specific to Win 2k8
                #region ControlAccessRightGuid's 2008
                if (PDCOSVersion > Common.ServerVersion.Win2003)
                {

                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Read-Only-Replication-Secret-Synchronization", "rightsGuid", out expectedResult);

                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                        "1131f6ae-9c07-11d1-f79f-00c04fc2dcd2",
                        expectedResult.ToString(),
                        384,
                        @"The control Access Right Symbol Read-Only-Replication-Secret-Synchronization has an identifying
                        GUID in ACE as 1131f6ae-9c07-11d1-f79f-00c04fc2dcd2");

                    //process the search response for expected Guid
                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Reload-SSL-Certificate", "rightsGuid", out expectedResult);

                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                        "1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8",
                        expectedResult.ToString(),
                        391,
                        @"The control Access Right Symbol Reload-SSL-Certificate has an identifying GUID in ACE as 
                        1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8");
                }

                #endregion

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Reanimate-Tombstones", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "45EC5156-DB7E-47bb-B53F-DBEB2D03C40F",
                    expectedResult.ToString(),
                    385,
                    @"The control Access Right Symbol Reanimate-Tombstones has an identifying GUID in ACE as 
                    45ec5156-db7e-47bb-b53f-dbeb2d03c40f");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Recalculate-Hierarchy", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "0bc1554e-0a99-11d1-adbb-00c04fd8d5cd",
                    expectedResult.ToString(),
                    386,
                    @"The control Access Right Symbol Recalculate-Hierarchy has an identifying GUID in ACE as 
                    0bc1554e-0a99-11d1-adbb-00c04fd8d5cd");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Recalculate-Security-Inheritance", "rightsGuid", out expectedResult);

                //process the search response for expected Guid
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "62dd28a8-7f46-11d2-b9ad-00c04f79f805",
                    expectedResult.ToString(),
                    387,
                    @"The control Access Right Symbol Recalculate-Security-Inheritance has an identifying GUID in ACE as 
                    62dd28a8-7f46-11d2-b9ad-00c04f79f805");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Receive-As", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "ab721a56-1e2f-11d0-9819-00aa0040529b",
                    expectedResult.ToString(),
                    388,
                    @"The control Access Right Symbol Receive-As has an identifying GUID in ACE as 
                    ab721a56-1e2f-11d0-9819-00aa0040529b");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Refresh-Group-Cache", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "9432c620-033c-4db7-8b58-14ef6d0bf477",
                    expectedResult.ToString(),
                    389,
                    @"The control Access Right Symbol Refresh-Group-Cache has an identifying GUID in ACE as 
                    9432c620-033c-4db7-8b58-14ef6d0bf477");


                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "SAM-Enumerate-Entire-Domain", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "91d67418-0135-4acc-8d79-c08e857cfbec",
                    expectedResult.ToString(),
                    392,
                    @"The control Access Right Symbol SAM-Enumerate-Entire-Domain has an identifying GUID in ACE as 
                    91d67418-0135-4acc-8d79-c08e857cfbec");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Send-As", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "ab721a54-1e2f-11d0-9819-00aa0040529b",
                    expectedResult.ToString(),
                    393,
                    @"The control Access Right Symbol Send-As has an identifying GUID in ACE as 
                    ab721a54-1e2f-11d0-9819-00aa0040529b");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Send-To", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "ab721a55-1e2f-11d0-9819-00aa0040529b",
                    expectedResult.ToString(),
                    394,
                    @"The control Access Right Symbol Send-To has an identifying GUID in ACE as 
                    ab721a55-1e2f-11d0-9819-00aa0040529b");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Unexpire-Password", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501",
                    expectedResult.ToString(),
                    395,
                    @"The control Access Right Symbol Unexpire-Password has an identifying GUID in ACE as 
                    ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Update-Password-Not-Required-Bit", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "280f369c-67c7-438e-ae98-1d46f3c6f541",
                    expectedResult.ToString(),
                    396,
                    @"The control Access Right Symbol Update-Password-Not-Required-Bit has an identifying GUID in ACE as 
                    280f369c-67c7-438e-ae98-1d46f3c6f541");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Update-Schema-Cache", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "be2bb760-7f46-11d2-b9ad-00c04f79f805",
                    expectedResult.ToString(),
                    397,
                    @"The control Access Right Symbol Update-Schema-Cache has an identifying GUID in ACE as 
                    be2bb760-7f46-11d2-b9ad-00c04f79f805");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "User-Change-Password", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "ab721a53-1e2f-11d0-9819-00aa0040529b",
                    expectedResult.ToString(),
                    398,
                    @"The control Access Right Symbol User-Change-Password has an identifying GUID in ACE as 
                    ab721a53-1e2f-11d0-9819-00aa0040529b");

                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "User-Force-Change-Password", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "00299570-246d-11d0-a768-00aa006e0529",
                    expectedResult.ToString(),
                    399,
                    @"The control Access Right Symbol User-Force-Change-Password has an identifying GUID in ACE as 
                    00299570-246d-11d0-a768-00aa006e0529");
                if (PDCOSVersion >= Common.ServerVersion.Win2012R2)
                {
                    //process the search response for expected Guid
                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Clone-Domain-Controller", "rightsGuid", out expectedResult);

                    TestClassBase.BaseTestSite.Assert.AreEqual(
                        "3e0f7e18-2c7a-4c10-ba82-4d926db99a3e",
                        expectedResult.ToString().ToLower(),
                        @"The control Access Right Symbol DS-Clone-Domain-Controller has an identifying GUID in ACE as 
                    3e0f7e18-2c7a-4c10-ba82-4d926db99a3e");


                    //process the search response for expected Guid
                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Read-Partition-Secrets", "rightsGuid", out expectedResult);

                    TestClassBase.BaseTestSite.Assert.AreEqual(
                        "084c93a2-620d-4879-a836-f0ae47de0e89",
                        expectedResult.ToString().ToLower(),
                        @"The control Access Right Symbol DS-Read-Partition-Secrets has an identifying GUID in ACE as 
                    084c93a2-620d-4879-a836-f0ae47de0e89");


                    //process the search response for expected Guid
                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Write-Partition-Secrets", "rightsGuid", out expectedResult);

                    TestClassBase.BaseTestSite.Assert.AreEqual(
                        "94825a8d-b171-4116-8146-1e34d8f54401",
                        expectedResult.ToString().ToLower(),
                        @"The control Access Right Symbol DS-Write-Partition-Secrets has an identifying GUID in ACE as 
                    94825a8d-b171-4116-8146-1e34d8f54401");


                    //process the search response for expected Guid
                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Set-Owner", "rightsGuid", out expectedResult);

                    TestClassBase.BaseTestSite.Assert.AreEqual(
                        "4125c71f-7fac-4ff0-bcb7-f09a41325286",
                        expectedResult.ToString().ToLower(),
                        @"The control Access Right Symbol DS-Set-Owner has an identifying GUID in ACE as 
                    4125c71f-7fac-4ff0-bcb7-f09a41325286");

                    //process the search response for expected Guid
                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "DS-Bypass-Quota", "rightsGuid", out expectedResult);

                    //TDI 72228. The GUID in TD is 4125c71f-7fac-4ff0-bcb7-f09a41325286. It is incorrect.
                    TestClassBase.BaseTestSite.Assert.AreEqual(
                        "88a9933e-e5c8-4f2a-9dd7-2527416b8092",
                        expectedResult.ToString().ToLower(),
                        @"The control Access Right Symbol DS-Bypass-Quota has an identifying GUID in ACE as 
                    88a9933e-e5c8-4f2a-9dd7-2527416b8092");

                    //process the Search response for expected Guid

                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Validated-MS-DS-Additional-DNS-Host-Name", "rightsGuid", out expectedResult);

                    TestClassBase.BaseTestSite.Assert.AreEqual(
                        "80863791-dbe9-4eb8-837e-7f0ab55d9ac7",
                        expectedResult.ToString(),
                        @"The Validated-MS-DS-Additional-DNS-Host-Name Validated write right symbol has an Identifying
                     Guid used in ACE as 80863791-dbe9-4eb8-837e-7f0ab55d9ac7(msDS-AdditionalDnsHostName attribute)"
                        );

                    ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Validated-MS-DS-Behavior-Version", "rightsGuid", out expectedResult);
                    TestClassBase.BaseTestSite.Assert.AreEqual(
                        "d31a8757-2447-4545-8081-3bb610cacbf2",
                        expectedResult.ToString(),
                        @"TheValidated-MS-DS-Behavior-Version Validated write right symbol has an Identifying
                      Guid used in ACE as d31a8757-2447-4545-8081-3bb610cacbf2(msDS-Behavior-Version attribute)"
                        );

                }
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Validated-DNS-Host-Name", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "72e39547-7b18-11d1-adef-00c04fd8d5cd",
                    expectedResult.ToString(),
                    326,
                    @"The Validated write right symbol Validated-DNS-Host-Name has an identifying GUID used in ACE as 
                    72e39547-7b18-11d1-adef-00c04fd8d5cd (dNSHostName attribute)");

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "72e39547-7b18-11d1-adef-00c04fd8d5cd",
                    expectedResult.ToString(),
                    588,
                    @"The Validated-DNS-Host-Name Validated write right symbol has an Identifying 
                    GUID used in ACE as 72e39547-7b18-11d1-adef-00c04fd8d5cd (dNSHostName attribute)");
                //process the search response for expected Guid
                ADTSHelper.ProcessSearchRespone(searchControlAccessGuids, "Validated-SPN", "rightsGuid", out expectedResult);

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "f3a64788-5306-11d1-a9c5-0000f80367c1",
                    expectedResult.ToString(),
                    327,
                    @"The Validated write right symbol Validated-SPN has an identifying GUID used in ACE as 
                    f3a64788-5306-11d1-a9c5-0000f80367c1 (servicePrincipalName attribute)");

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(
                    "f3a64788-5306-11d1-a9c5-0000f80367c1",
                    expectedResult.ToString(),
                    589,
                    @"The Validated-SPN Validated write right symbol has an Identifying 
                    GUID used in ACE as f3a64788-5306-11d1-a9c5-0000f80367c1 (servicePrincipalName attribute)");

                #endregion

                #region GroupType Requirements
                //This region validates the group type related requirements 

                //Specifies expected value
                object expectedValue;

                //Specifies actual value
                int actualValue;

                //if the search contains the expected ,then set this variable to true.
                bool isExisted = false;

                #region Constants

                //from ADTS Technical Specification
                const uint GROUP_TYPE_ACCOUNT_GROUP = 2;

                //from ADTS Technical Specification
                const uint GROUP_TYPE_RESOURCE_GROUP = 4;

                //from ADTS Technical Specification
                const uint GROUP_TYPE_UNIVERSAL_GROUP = 8;

                //from ADTS Technical Specification
                const uint GROUP_TYPE_SECURITY_ENABLED = 2147483648;

                #endregion

                //Searching Group objects in Users container in Domain NC.
                resultSearchResponse = ADTSHelper.SearchObject("CN=Users," + parsedDN,
                                               "objectclass=group",
                                               System.DirectoryServices.Protocols.SearchScope.Subtree,
                                               new string[] { "groupType" }, tempLdapConnection);

                #region Validate MS-ADTS-Security_R533

                isExisted = false;

                //get groupType value
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Cert Publishers", "groupType", out expectedValue);

                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_RESOURCE_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_RESOURCE_GROUP) == GROUP_TYPE_RESOURCE_GROUP &&
                     (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 533, @"The Cert Publishers has 
                        groupType as {GROUP_TYPE_RESOURCE_GROUP | GROUP_TYPE_SECURITY_ENABLED} _ENABLED}");

                #endregion

                #region Validate MS-ADTS-Security_R537

                //reset to default values
                isExisted = false;

                expectedValue = null;
                actualValue = -1;

                //get groupType value
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Domain Admins", "groupType", out expectedValue);

                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                    (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 537, @"The Domain Administrators have 
                        groupType as { GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED}");

                #endregion

                #region Validate MS-ADTS-Security_R541

                //reset to default values
                isExisted = false;

                expectedValue = null;
                actualValue = -1;

                //get groupType value
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Domain Computers", "groupType", out expectedValue);
                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                    (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 541, @"The Domain Computers has 
                        groupType as { GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED }");

                #endregion

                #region Validate MS-ADTS-Security_R545

                //reset to default values
                isExisted = false;

                expectedValue = null;
                actualValue = -1;

                //get groupType value
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Domain Controllers", "groupType", out expectedValue);
                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                    (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 545, @"The Domain Controllers have 
                        groupType as { GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED }");

                #endregion

                #region Validate MS-ADTS-Security_R549

                //reset to default values
                isExisted = false;
                expectedValue = null;
                actualValue = -1;

                //Process the Search response for the expected group type value.
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Domain Guests", "groupType", out expectedValue);

                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                    (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 549, @"The Domain Guests have 
                                    groupType as { GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED }");

                #endregion

                #region Validate MS-ADTS-Security_R553

                isExisted = false;
                expectedValue = null;
                actualValue = -1;

                //get groupType value
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Domain Users", "groupType", out expectedValue);
                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                    (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 553, @"The Domain Users have 
                                    groupType as { GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED }");

                #endregion

                #region Validate MS-ADTS-Security_R558, MS-ADTS-Security_R559, MS-ADTS-Security_R580, MS-ADTS-Security_R581

                //reset to default values
                isExisted = false;
                expectedValue = null;
                actualValue = -1;

                int mixedValue = -1;

                // Disable referral.
                tempLdapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;
                //search for DomainDNS
                SearchResponse mixedSearch = ADTSHelper.SearchObject(parsedDN, "objectClass=domainDNS",
                    System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "nTMixedDomain" }, tempLdapConnection);

                //get nTMixedDomain to know whether the domain is in native or mixed mode
                ADTSHelper.ProcessSearchRespone(mixedSearch, adDomain, "nTMixedDomain", out expectedValue);

                mixedValue = int.Parse(expectedValue.ToString());

                //if domain is in mixed mode,
                if (mixedValue == 1)
                {
                    //get groupType value
                    ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Enterprise Admins", "groupType", out expectedValue);
                    actualValue = int.Parse(expectedValue.ToString());

                    //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                    if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                        (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                    {
                        isExisted = true;
                    }

                    //capturing requirement
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 558, @"The Enterprise 
                            Administrators have groupType as { GROUP_TYPE_ACCOUNT_GROUP | 
                            GROUP_TYPE_SECURITY_ENABLED } If the forest root domain is mixed");

                    //get groupType value
                    ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Schema Admins", "groupType", out expectedValue);
                    actualValue = int.Parse(expectedValue.ToString());

                    //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                    if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                        (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                    {
                        isExisted = true;
                    }

                    //capturing requirement
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 580, @"The Schema Admins have 
                       groupType as {GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED } if the forest root 
                       domain is mixed ");

                    //ModifyRequest to modify nTMixedDomain
                    ModifyRequest modReq = new ModifyRequest(parsedDN,
                                             DirectoryAttributeOperation.Replace,
                                             "nTMixedDomain",
                                             "0");
                    //Response 
                    ModifyResponse modRes = (ModifyResponse)tempLdapConnection.SendRequest(modReq);
                }

                //search for DomainDNS
                mixedSearch = ADTSHelper.SearchObject(parsedDN, "objectClass=domainDNS",
                    System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "nTMixedDomain" }, tempLdapConnection);

                //get nTMixedDomain to know whether the domain is in native or mixed mode
                ADTSHelper.ProcessSearchRespone(mixedSearch, adDomain, "nTMixedDomain", out expectedValue);
                mixedValue = int.Parse(expectedValue.ToString());


                //if domain in in native mode
                if (mixedValue == 0)
                {
                    //get groupType value
                    ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Enterprise Admins", "groupType", out expectedValue);
                    actualValue = int.Parse(expectedValue.ToString());

                    //check GROUP_TYPE_UNIVERSAL_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                    if ((actualValue & GROUP_TYPE_UNIVERSAL_GROUP) == GROUP_TYPE_UNIVERSAL_GROUP &&
                        (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                    {
                        isExisted = true;
                    }

                    //capturing requirement
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 559, @"The Enterprise 
                            Administrators have  groupType as {GROUP_TYPE_UNIVERSAL_GROUP | 
                            GROUP_TYPE_SECURITY_ENABLED } If the forest root domain is not mixed");

                    //get groupType value
                    ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Schema Admins", "groupType", out expectedValue);
                    actualValue = int.Parse(expectedValue.ToString());

                    //check GROUP_TYPE_UNIVERSAL_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                    if ((actualValue & GROUP_TYPE_UNIVERSAL_GROUP) == GROUP_TYPE_UNIVERSAL_GROUP &&
                        (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                    {
                        isExisted = true;
                    }

                    //capturing requirement
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 581, @"The Schema Admins have 
                          group type as { GROUP_TYPE_UNIVERSAL_GROUP | GROUP_TYPE_SECURITY_ENABLED } 
                          If the forest root domain is not mixed");
                }

                #endregion

                #region Validate MS-ADTS-Security_R563

                //Reset to default values
                isExisted = false;
                expectedValue = null;
                actualValue = -1;

                //get groupType value
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Group Policy Creator Owners", "groupType", out expectedValue);

                //parse for actual value
                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                    (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 563, @"The Group Policy Creator Owners  
                                    have groupType as { GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED}");

                #endregion

                #region Validate MS-ADTS-Security_R567

                //Reset to default values
                isExisted = false;
                expectedValue = null;
                actualValue = -1;

                //get groupType value
                ADTSHelper.ProcessSearchRespone(resultSearchResponse, "RAS and IAS Servers", "groupType", out expectedValue);

                actualValue = int.Parse(expectedValue.ToString());

                //check GROUP_TYPE_ACCOUNT_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                if ((actualValue & GROUP_TYPE_RESOURCE_GROUP) == GROUP_TYPE_RESOURCE_GROUP &&
                    (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                {
                    isExisted = true;
                }

                //capturing requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 567, @"The RAS and IAS servers have 
                                    groupType as {GROUP_TYPE_RESOURCE_GROUP | GROUP_TYPE_SECURITY_ENABLED}");

                #endregion

                //Specific to win 2k8 .
                if (PDCOSVersion == ServerVersion.Win2008 || PDCOSVersion == ServerVersion.Win2008R2)
                {
                    #region Covering MS-ADTS-Security_R571

                    isExisted = false;

                    //get groupType value
                    ADTSHelper.ProcessSearchRespone(resultSearchResponse, "Read-only Domain Controllers", "groupType", out expectedValue);

                    actualValue = int.Parse(expectedValue.ToString());

                    //check GROUP_TYPE_RESOURCE_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                    if ((actualValue & GROUP_TYPE_ACCOUNT_GROUP) == GROUP_TYPE_ACCOUNT_GROUP &&
                         (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                    {
                        isExisted = true;
                    }

                    //capturing requirement
                    //Add the log information
                    TestClassBase.BaseTestSite.Log.Add(
                         LogEntryKind.Debug,
                         @"[MS-ADTS-Security_R571] actualValue={0},GROUP_TYPE_ACCOUNT_GROUP={1}",
                         actualValue,
                         GROUP_TYPE_ACCOUNT_GROUP);

                    //Verify MS-ADTS-Security_R571
                    //This action assume taking place in deployment
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                        isExisted,
                        571,
                        @"The Read-Only Domain Controllers have groupType as { GROUP_TYPE_ACCOUNT_GROUP | 
                         GROUP_TYPE_SECURITY_ENABLED } and the  group is created in a domain by the PDC, 
                         the first time(This may occur upon the new installation of or upgrade to a W2K8/Win7
                         DC if that DC also holds the PDC role. However, it may also occur when the first 
                         DC that is running WS08 or Win7 assumes the PDC role.) a Windows Server 2008 
                         operating system or Windows Server 2008 R2 operating system DC holds the PDC FSMO.");

                    #endregion

                    #region Covering MS-ADTS-Security_R575

                    isExisted = false;

                    //get groupType value
                    ADTSHelper.ProcessSearchRespone(
                        resultSearchResponse,
                        "Enterprise Read-only Domain Controllers",
                        "groupType",
                        out expectedValue);

                    actualValue = int.Parse(expectedValue.ToString());

                    //check GROUP_TYPE_RESOURCE_GROUP and GROUP_TYPE_SECURITY_ENABLED bits are set
                    if ((actualValue & GROUP_TYPE_UNIVERSAL_GROUP) == GROUP_TYPE_UNIVERSAL_GROUP &&
                         (actualValue & GROUP_TYPE_SECURITY_ENABLED) == GROUP_TYPE_SECURITY_ENABLED)
                    {
                        isExisted = true;
                    }

                    //capturing requirement
                    //Add the log information
                    TestClassBase.BaseTestSite.Log.Add(
                         LogEntryKind.Debug,
                         @"[MS-ADTS-Security_R575] actualValue={0},GROUP_TYPE_ACCOUNT_GROUP={1}",
                         actualValue,
                         GROUP_TYPE_ACCOUNT_GROUP);

                    //Verify MS-ADTS-Security_R575
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                        isExisted,
                        575,
                        @"The Enterprise Read-Only Domain Controllers have groupType as { GROUP_TYPE_UNIVERSAL_GROUP | 
                         GROUP_TYPE_SECURITY_ENABLED } and the group is created in the root domain by 
                         the root domain PDC, the first time a Windows Server 2008 operating system or Windows Server
                         2008 R2 operating system DC holds the root domain PDC FSMO.");

                    #endregion
                }

                #endregion

                #region MS-ADTS-Security_R38

                //Access Rights are set indicates accessing

                //Set Right Access.
                bool isResult = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                "CN =" + ClientUserName + ",CN=Users," + parsedDN,
                                                ClientUserName,
                                                ClientUserPassword,
                                                adDomain,
                                                ActiveDirectoryRights.ReadControl,
                                                AccessControlType.Allow);

                //perform SearchOpeartion
                SearchResponse response = ADTSHelper.SearchObject("CN =" + ClientUserName + ",CN=Users," + parsedDN,
                                                        "objectclass=user", System.DirectoryServices.Protocols.SearchScope.Base,
                                                        new string[] { "cn" }, connection);

                //if the SearchResponse Entries is greater than 0
                isResult = response.Entries.Count > 0;

                //Validate MS-ADTS-Security_R38
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isResult, 38, @"Generally, the security 
                                    context of the requester must be granted rights RIGHT_DS_READ_PROPERTY on OA by
                                    O!nTSecurityDescriptor. Otherwise, the value is treated as does not exist in the  
                                    returned attributes and the LDAP filter");

                #endregion

                #region ValidatingParentChildSidMatchingRequirements.
                if (ADTSHelper.CreateActiveDirectoryUser(PdcFqdn, TempUser1, TempUser1Password, parsedDN))
                {
                    try
                    {
                        string requiredObjectDN = "CN=" + TempUser1 + ",CN=Users," + parsedDN;

                        //getting the newly created user object.
                        DirectoryEntry obj = new DirectoryEntry(string.Format("LDAP://{0}/{1}", PdcFqdn, requiredObjectDN));

                        //get its owner sid
                        string expectedSid = obj.ObjectSecurity.GetOwner(typeof(SecurityIdentifier)).Value;

                        //get its parent name
                        NTAccount ownerAcctName = (NTAccount)obj.ObjectSecurity.GetOwner(typeof(SecurityIdentifier)).Translate(typeof(NTAccount));
                        string parentName = ownerAcctName.Value.Split(new string[] { "\\" }, StringSplitOptions.None)[1];

                        //get the parent object
                        DirectoryEntry parentObj = new DirectoryEntry(string.Format("LDAP://{0}/CN={1},CN=Users,{2}", PdcFqdn, parentName, parsedDN));

                        //get its object sid
                        SecurityIdentifier parentSid = new SecurityIdentifier((byte[])parentObj.Properties["objectsid"].Value, 0);
                        string actualSid = parentSid.Value;

                        bool readRight = false, writeDaclRight = false, writeOwnerRight = false;

                        //if both sids are equal
                        if (expectedSid.ToLower().Equals(actualSid.ToLower()))
                        {
                            readRight = ADTSHelper.SetAccessRights(PdcFqdn, requiredObjectDN, AdminUser, AdminUserPassword, adDomain, ActiveDirectoryRights.ReadControl, AccessControlType.Allow);

                            writeDaclRight = ADTSHelper.SetAccessRights(PdcFqdn, requiredObjectDN, AdminUser, AdminUserPassword, adDomain, ActiveDirectoryRights.WriteDacl, AccessControlType.Allow);

                            writeOwnerRight = ADTSHelper.SetAccessRights(PdcFqdn, requiredObjectDN, AdminUser, AdminUserPassword, adDomain, ActiveDirectoryRights.WriteOwner, AccessControlType.Allow);
                        }

                        bool isSuccess = readRight && writeDaclRight && writeOwnerRight;

                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isSuccess, 343, @"G and D denote the 
                            access rights that are granted and denied, respectively, on the object. If the SID in 
                            the Owner field of the object's security descriptor matches any SID in the requester's 
                            security context, then add the bits Read Control (RC), Write DACL (WD) and Write Owner 
                            (WO) to G.");

                    }
                    catch (DirectoryServicesCOMException ex)
                    {
                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "An Exception has occured", ex.Message);
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "An Exception has occured", ex.Message);
                    }
                }

                #endregion
            }

        }

        #endregion

        #region ValidateMsDS_NCReplInboundNeighborsAttributeRequirements

        /// <summary>
        /// Validates the Requirements of ms-DS_NCReplInboundNeighbors
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="ctrlAccessRight">Specifies the control access Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidateMsDS_NCReplInboundNeighborsAttribute(AccessRights accessRight,
                                                                        ControlAccessRights ctrlAccessRight,
                                                                        AttribsToCheck attr)
        {

            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;
            userForcePwd = false;

            #region Allow Access Requirementts Validation

            //validate if msDS_NCReplInboundNeighbors attribute and RIGHT_DS_READ_PROPERTY and DS_Query_Self_Quota
            //Allow the specifies Access to validate the Requirements.
            if ((attr == AttribsToCheck.msDS_NCReplInboundNeighbors) &&
                ((accessRight == AccessRights.DS_READ_REPSFORM) ||
                 (ctrlAccessRight == ControlAccessRights.Repl_Man_Topo_REPSFROM) ||
                 (ctrlAccessRight == ControlAccessRights.Repl_Mon_Topo_REPSFROM)))
            {
                try
                {
                    #region Set RIGHT_DS_READ_PROPERTY Access Rights

                    //Allow Access Right to the current user
                    bool isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                          ClientUserName,
                                                                          ClientUserPassword,
                                                                          adDomain,
                                                                          ActiveDirectoryRights.ReadProperty,
                                                                          AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Granted the ReadProperty to the current user");

                    //Search for the msDS-NCReplInboundNeighbors attribute after Setting the READProperty Access Right.
                    SearchResponse searchResponseNCReplOutboundNeighbors = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Inbound-Neighbors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Inbound-Neighbors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplOutboundNeighbors, "the searchResponseNCReplOutboundNeighbors is not null");
                    #endregion

                    #region Set Control Access right RIGHT_DS_REPL_MANAGE_TOPOLOGY

                    //Allow Specifies Control Access right to the user.
                    //Set the RIGHT_DS_REPL_MANAGE_TOPOLOGY
                    bool isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allowed the ExtendedRight control access right.");
                    //the result count should  not be 0 as both the access rights and control access rights are granted
                    SearchResponse searchResponseNCReplInboundNeighborManageTopology;

                    //Search for the attribute msds-NCReplInboundNeighbors.
                    searchResponseNCReplInboundNeighborManageTopology = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Inbound-Neighbors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Inbound-Neighbors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplInboundNeighborManageTopology, "the searchResponseNCReplInboundNeighborManageTopology is not null");
                    #endregion

                    #region Set Control Access right RIGHT_DS_REPL_MONITOR_TOPOLOGY

                    //Allow Specifies Control Access right to the user.
                    //Set the RIGHT_DS_REPL_MANAGE_TOPOLOGY
                    isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allowed the ExtendedRight control access right.");
                    //the result count should  not be 0 as both the access rights and control access rights are granted
                    SearchResponse searchResponseNCReplInboundNeighborMonitorTopology;

                    //Search for the attribute msds-NCReplInboundNeighbors.
                    searchResponseNCReplInboundNeighborMonitorTopology = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Inbound-Neighbors,CN=Schema,CN=Configuration,,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Inbound-Neighbors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplInboundNeighborMonitorTopology, "the searchResponseNCReplInboundNeighborMonitorTopology is not null");
                    #endregion

                    return errorstatus.success;

                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {
                    //Failure has happended while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;
                }
            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else
                if ((attr == AttribsToCheck.msDS_NCReplInboundNeighbors) &&
                     ((accessRight != AccessRights.DS_READ_REPSFORM) &&
                     (ctrlAccessRight != ControlAccessRights.Repl_Mon_Topo_REPSFROM) &&
                     (ctrlAccessRight != ControlAccessRights.Repl_Man_Topo_REPSFROM)))
                {
                    try
                    {
                        #region Remove ManageTopology .MonitorTopology Control Access rights and Read property Access Rights

                        //Set the control access right to deny 
                        //deny Repl_Manage_Topology Control Access Right
                        bool isManageControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                        "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                        ClientUserName,
                                                        ClientUserPassword,
                                                        adDomain,
                                                        guidDS_Repl_ManageE_Topology,
                                                        ActiveDirectoryRights.ExtendedRight,
                                                        AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccess, "Deny the ExtendedRight control access right.");
                        //Set the control access right to deny 
                        //deny Repl_Monitor_Topology Control Access Right
                        bool isMonitorControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                        "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                        ClientUserName,
                                                        ClientUserPassword,
                                                        adDomain,
                                                        guidDS_Replication_Monitor_Topology,
                                                        ActiveDirectoryRights.ExtendedRight,
                                                        AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccess, "Deny the ExtendedRight control access right.");
                        //Set the Read access right to deny 
                        //Deny READ Access right
                        bool isReadAccessRemoved = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                            "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                            ClientUserName,
                                                            ClientUserPassword,
                                                            adDomain,
                                                            ActiveDirectoryRights.ReadProperty,
                                                            AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemoved, "Removed the ReadProperty from previous action");
                        #endregion

                        #region Validation

                        //Search for the specifies attribute after removing both the access right and control access
                        //the count should be 0
                        SearchResponse searchResponseNCReplInboundNeighbors = ADTSHelper.SearchObject(
                                              "CN=ms-DS-NC-Repl-Inbound-Neighbors,CN=Schema,CN=Configuration," + parsedDN,
                                              "objectClass=attributeSchema",
                                              System.DirectoryServices.Protocols.SearchScope.Subtree,
                                              new string[] { "ms-DS-NC-Repl-Inbound-Neighbors" },
                                              connection);

                        //check whether the specifies attribute is present in the search response returned.
                        //The result should be false as the access rights are removed ,the attribute should not return. 

                        //after getting the result store it in a variable 
                        int isNCReplInboundNeighborsExist = searchResponseNCReplInboundNeighbors.Entries[0].Attributes.Count;
                        //Rollback to previous right because if an error or an exception occurs the user will not have access right
                        //as the rights were denied and subsequent test cases will fail. so roll back to previous rights
                        //and then validate

                        #region RollBack the Removed Operations

                        //Rollback to previous Access Rights.
                        //Delete the Denied AccessRight
                        //Set the AccessRight and Control Access Right

                        //Rollback the ReadAccess
                        //Remove the Deny Access Right
                        bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                                 "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                                 AdminUser,
                                                                                 AdminUserPassword,
                                                                                 adDomain,
                                                                                 ActiveDirectoryRights.ReadProperty,
                                                                                 AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Removed the ReadProperty Access Right.");
                        //Setting the Access Right for the user.
                        //Set Read Access Right.
                        bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                                    AdminUser,
                                                                                    AdminUserPassword,
                                                                                    ClientUserName,
                                                                                    adDomain,
                                                                                    ActiveDirectoryRights.ReadProperty,
                                                                                    AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Set the ReadProperty Access Right for the current user.");

                        //Rollback To Control Access Right
                        //Remove the deny access
                        bool isManageControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                                            ClientUserName,
                                                                                            ClientUserPassword,
                                                                                            adDomain,
                                                                                            guidDS_Repl_ManageE_Topology,
                                                                                            ActiveDirectoryRights.ExtendedRight,
                                                                                            AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccessSet, "Removed the ExtendedRight Access Right.");
                        //Set the Control access right 
                        bool isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                       "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Repl_ManageE_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right.");
                        //Remove the deny access
                        bool isMonitorControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                                             ClientUserName,
                                                                                             ClientUserPassword,
                                                                                             adDomain,
                                                                                             guidDS_Replication_Monitor_Topology,
                                                                                             ActiveDirectoryRights.ExtendedRight,
                                                                                             AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccessSet, "Removed the ExtendedRight Access Right.");
                        //Set the Control access right 
                        isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                        "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                        ClientUserName,
                                        ClientUserPassword,
                                        adDomain,
                                        guidDS_Replication_Monitor_Topology,
                                        ActiveDirectoryRights.ExtendedRight,
                                        AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right.");
                        #endregion

                        //Rollback is done and validate now.
                        //Check for the attribute in entries of returned search response.

                        //Validate MS-ADTS-Security_66
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, isNCReplInboundNeighborsExist, 66,
                            @"While accessing  msDS-NCReplInboundNeighbors attribute if The security context of the 
                            requestor is not  granted the following rights on repsFrom: (RIGHT_DS_READ_PROPERTY) and 
                            (RIGHT_DS_REPL_MANAGE_TOPOLOGY) snd (RIGHT_DS_REPL_MONITOR_TOPOLOGY),then the value 
                            is treated as does not exist in the returned attributes and the LDAP filter.");

                        return errorstatus.failure;

                        #endregion

                    }

                    catch (DirectoryException)
                    {
                        #region RollBack the Removed Operations

                        //Rollback to previous Access Rights.
                        //Delete the Denied AccessRight
                        //Set the AccessRight and Control Access Right

                        //Rollback the ReadAccess
                        bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                                   "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                                   AdminUser,
                                                                                   AdminUserPassword,
                                                                                   adDomain,
                                                                                   ActiveDirectoryRights.ReadProperty,
                                                                                   AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Removed the ReadProperty Access Right.");

                        //Setting the Access Right for the user.
                        bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                                                                    AdminUser,
                                                                                    AdminUserPassword,
                                                                                    ClientUserName,
                                                                                    adDomain,
                                                                                    ActiveDirectoryRights.ReadProperty,
                                                                                    AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allowed the ReadProperty Access Right for the current user.");
                        //Rollback To Control Access Right

                        //Remove the deny access
                        bool isManageControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Repl_ManageE_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccessSet, "Removed the ExtendedRight Access Right.");
                        //Set the Control access right 
                        bool isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                       "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Repl_ManageE_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right.");
                        //Remove the deny access
                        bool isMonitorControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                       "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                       ClientUserName,
                                       ClientUserPassword,
                                       adDomain,
                                       guidDS_Replication_Monitor_Topology,
                                       ActiveDirectoryRights.ExtendedRight,
                                       AccessControlType.Deny);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccessSet, "Removed the ExtendedRight Access Right.");

                        //Set the Control access right 
                        isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                      "CN=Reps-From,CN=Schema,CN=Configuration," + parsedDN,
                                      ClientUserName,
                                      ClientUserPassword,
                                      adDomain,
                                      guidDS_Replication_Monitor_Topology,
                                      ActiveDirectoryRights.ExtendedRight,
                                      AccessControlType.Allow);
                        TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right.");
                        #endregion

                        //Failure has happended while performing Ldap operation.
                        return errorstatus.failure;

                    }

                }

            #endregion

            return errorstatus.success;
        }

        #endregion

        #region ValidateMsDS_NCReplOutboundNeighborsAttributeRequirements

        /// <summary>
        /// Validates the Requirements of ms-DS_NCReplInboundNeighbors
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="ctrlAccessRight">Specifies the control accese Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidateMsDS_NCReplOutboundNeighborsAttribute(AccessRights accessRight,
                                                                        ControlAccessRights ctrlAccessRight,
                                                                        AttribsToCheck attr)
        {

            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;
            userForcePwd = false;

            #region Allow Access Requirementts Validation

            //validate if msDS_QuotaEffective attribute and RIGHT_DS_READ_PROPERTY and DS_Query_Self_Quota
            //Allow the specifies Access to validate the Requirements.
            if ((attr == AttribsToCheck.msDS_NCReplOutboundNeighbors) &&
                ((accessRight == AccessRights.DS_READ_REPSTO) ||
                 (ctrlAccessRight == ControlAccessRights.Repl_Man_Topo_REPSTO) ||
                 (ctrlAccessRight == ControlAccessRights.Repl_Mon_Topo_REPSTO)))
            {
                try
                {
                    #region Set RIGHT_DS_READ_PROPERTY Access Rights

                    //Allow Access Right to the current user
                    bool isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                    "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    ActiveDirectoryRights.ReadProperty,
                                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Allowed the ReadProperty Access Right for the current user.");

                    //Search for the msDS-NCReploutboundNeighbors attribute after Setting the READProperty Access Right.
                    SearchResponse searchResponseNCReplOutboundNeighbors = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Outbound-Neighbors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Outbound-Neighbors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplOutboundNeighbors, "the searchResponseNCReplOutboundNeighbors is not null");
                    #endregion

                    #region Set Control Access right RIGHT_DS_REPL_MANAGE_TOPOLOGY

                    //Allow Specifies Control Access right to the user.
                    //Set the RIGHT_DS_REPL_MANAGE_TOPOLOGY
                    bool isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allow the ExtendedRight control access right.");
                    //the result count should  not be 0 as both the access rights and control access rights are granted
                    SearchResponse searchResponseNCReplOutBoundNeighborManageTopology;

                    //Search for the attribute msds-NCReplInboundNeighbors.
                    searchResponseNCReplOutBoundNeighborManageTopology = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Outbound-Neighbors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Outbound-Neighbors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplOutBoundNeighborManageTopology, "the searchResponseNCReplOutBoundNeighborManageTopology is not null");
                    #endregion

                    #region Set Control Access right RIGHT_DS_REPL_MONITOR_TOPOLOGY

                    //Allow Specifies Control Access right to the user.
                    //Set the RIGHT_DS_REPL_MANAGE_TOPOLOGY
                    isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allow the ExtendedRight control access right.");
                    //the result count should  not be 0 as both the access rights and control access rights are granted
                    SearchResponse searchResponseNCReplOutboundNeighborMonitorTopology;

                    //Search for the attribute msds-NCReplOutboundNeighbors.
                    searchResponseNCReplOutboundNeighborMonitorTopology = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Outbound-Neighbors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Outbound-Neighbors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplOutboundNeighborMonitorTopology, "the searchResponseNCReplOutboundNeighborMonitorTopology is not null");
                    #endregion

                    return errorstatus.success;

                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {

                    //Failure has happened while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;

                }

            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else if ((attr == AttribsToCheck.msDS_NCReplOutboundNeighbors) &&
                     ((accessRight != AccessRights.DS_READ_REPSTO) &&
                     (ctrlAccessRight != ControlAccessRights.Repl_Man_Topo_REPSTO) &&
                     (ctrlAccessRight != ControlAccessRights.Repl_Mon_Topo_REPSTO)))
            {
                try
                {
                    #region Remove ManageTopology.MonitorTopology Control Access rights and Read property Access Rights

                    //Set the control access right to deny 
                    //deny Repl_Manage_Topology Control Access  Right
                    bool isManageControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                    "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    guidDS_Repl_ManageE_Topology,
                                                    ActiveDirectoryRights.ExtendedRight,
                                                    AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccess, "Deny the ExtendedRight control access right.");
                    //Set the control access right to deny 
                    //deny Repl_Monitor_Topology Control Access  Right
                    bool isMonitorControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                    "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    guidDS_Replication_Monitor_Topology,
                                                    ActiveDirectoryRights.ExtendedRight,
                                                    AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccess, "Deny the ExtendedRight control access right.");
                    //Set the Read access right to deny 
                    //Deny READ Access right
                    bool isReadAccessRemoved = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                        "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                        ClientUserName,
                                                        ClientUserPassword,
                                                        adDomain,
                                                        ActiveDirectoryRights.ReadProperty,
                                                        AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemoved, "Removed the ReadProperty Access Right.");
                    #endregion

                    #region Validation

                    //Search for the specifies attribute after removing both the access right and control access
                    //the count should be 0
                    SearchResponse searchResponseNCReplOutboundNeighbors = ADTSHelper.SearchObject(
                                          "CN=ms-DS-NC-Repl-Outbound-Neighborso,CN=Schema,CN=Configuration," + parsedDN,
                                          "objectClass=attributeSchema",
                                          System.DirectoryServices.Protocols.SearchScope.Subtree,
                                          new string[] { "ms-DS-NC-Repl-Outbound-Neighbors" },
                                          connection);

                    //check whethether the specifies attribute is present in the search response returned.
                    //The result should be false as the access rights are removed,the attribute should not return. 

                    //after getting the result store it in a variable 
                    int nCReplOutboundNeighborsCount = 0;                   // add by Rina
                    bool verify_nCReplOutboundNeighborsCount = false;
                    if (searchResponseNCReplOutboundNeighbors != null && searchResponseNCReplOutboundNeighbors.Entries.Count != 0)
                    {
                        verify_nCReplOutboundNeighborsCount = true;
                        nCReplOutboundNeighborsCount = searchResponseNCReplOutboundNeighbors.Entries[0].Attributes.Count;
                    }
                    //Rollback to previous right because if an error or an exception occurs the user will not have access right
                    //as the rights were denied and subsequent test cases will fail. so roll back to previous rights
                    //and then validate

                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    //Rollback the ReadAccess
                    //Remove the Deny Access Right
                    bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                               "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                                               AdminUser,
                                                                               AdminUserPassword,
                                                                               adDomain,
                                                                               ActiveDirectoryRights.ReadProperty,
                                                                               AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Removed the ReadProperty Access Right.");
                    //Setting the Access Right for the user.
                    //Set Read Access Right.
                    bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                                                                "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                ClientUserName,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadProperty,
                                                                                AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allowed the ReadProperty Access Right.");
                    //Rollback To Control Access Right
                    //Remove the deny access
                    bool isManageControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccessSet, "Denied the ExtendedRight Access Right.");
                    //Set the Control access right 
                    bool isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Deny the ExtendedRight control access right.");
                    //Remove the deny access
                    bool isMonitorControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccessSet, "Denied the ExtendedRight Access Right.");
                    //Set the Control access right 
                    isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Deny the ExtendedRight control access right.");
                    #endregion

                    //Rollback is done and validate now.
                    //Check for the attribure in entries of returned search response.
                    if (verify_nCReplOutboundNeighborsCount)
                    {
                        //Validate MS-ADTS-Security_71
                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, nCReplOutboundNeighborsCount, 71,
                            @"While accessing msDS-NCReplOutboundNeighbors attribute, if The security context of the 
                        requestor is not granted the following rights on repsTo: (RIGHT_DS_READ_PROPERTY) and 
                        (RIGHT_DS_REPL_MANAGE_TOPOLOGY) and (RIGHT_DS_REPL_MONITOR_TOPOLOGY),then the value is treated 
                        as does not exist in the returned attributes and the LDAP search filter.");
                    }

                    return errorstatus.failure;

                    #endregion

                }

                catch (DirectoryException)
                {
                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    //Rollback the ReadAccess
                    bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                               "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                                               AdminUser,
                                                                               AdminUserPassword,
                                                                               adDomain,
                                                                               ActiveDirectoryRights.ReadProperty,
                                                                               AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Denied the ReadProperty Access Right.");
                    //Setting the Access Right for the user.
                    bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                                                                "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                ClientUserName,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadProperty,
                                                                                AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allowed the ReadProperty Access Right.");
                    //Rollback To Control Access Right

                    //Remove the deny access
                    bool isManageControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccessSet, "Deny the ExtendedRight Access Right.");
                    //Set the Control access right 
                    bool isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Deny the ExtendedRight control access right.");
                    //Remove the deny access
                    bool isMonitorControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccessSet, "Deny the ExtendedRight Access Right.");
                    //Set the Control access right 
                    isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                   "CN=Reps-To,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Deny the ExtendedRight control access right.");
                    #endregion

                    //Failure has happended while performing Ldap operation.
                    return errorstatus.failure;
                }
            }
            #endregion

            return errorstatus.success;
        }

        #endregion

        #region ValidateMsDS_NCReplCursorAttributeRequirements

        /// <summary>
        /// Validates the Requirements of msDS-NCReplCursors
        /// </summary>
        /// <param name="accessRight">Specifies the access Right</param>
        /// <param name="ctrlAccessRight">Specifies the control access Right</param>
        /// <param name="attr">attribute to check</param>
        /// <returns>returns the status of Ldap Operation and validation</returns>
        public errorstatus ValidateMsDS_NCReplCursor(AccessRights accessRight,
                                                                        ControlAccessRights ctrlAccessRight,
                                                                        AttribsToCheck attr)
        {

            //Set to false as the attribute is not UserChangepassword
            //set userChangePwd and userForcePwd to false.
            userChangePwd = false;
            userForcePwd = false;

            #region Allow Access Requirementts Validation

            //validate if msDS_NCReplCursors attribute and RIGHT_DS_READ_PROPERTY and DS_Query_Self_Quota
            //Allow the specifies Access to validate the Requirements.
            if ((attr == AttribsToCheck.msDS_NCReplCursors) &&
                ((accessRight == AccessRights.DS_READ_REPLVECTOR) ||
                 (ctrlAccessRight == ControlAccessRights.DS_Replication_Monitor_Topology) ||
                 (ctrlAccessRight == ControlAccessRights.Repl_Man_Topo_REPLVECTOR)))
            {
                try
                {
                    #region Set RIGHT_DS_READ_PROPERTY Access Rights

                    //Allow Access Right to the current user
                    bool isReadAccessGranted = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                    "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    ActiveDirectoryRights.ReadProperty,
                                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessGranted, "Allowed the ReadProperty control access right.");
                    //Search for the msDS-NCReplCursors attribute after Setting the READProperty Access Right.
                    SearchResponse searchResponseNCReplCursors = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Cursors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Cursors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplCursors, "the searchResponseNCReplCursors is not null");
                    #endregion

                    #region Set Control Access right RIGHT_DS_REPL_MANAGE_TOPOLOGY

                    //Allow Specifies Control Access right to the user.
                    //Set the RIGHT_DS_REPL_MANAGE_TOPOLOGY
                    bool isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allowed the ExtendedRight control access right.");
                    //the result count should  not be 0 as both the access rights and control access rights are granted
                    SearchResponse searchResponseNCReplOutBoundNeighborManageTopology;

                    //Search for the attribute msds-NCReplInboundNeighbors.
                    searchResponseNCReplOutBoundNeighborManageTopology = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Cursors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Cursors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplOutBoundNeighborManageTopology, "the searchResponseNCReplOutBoundNeighborManageTopology is not null");
                    #endregion

                    #region Set Control Access right RIGHT_DS_REPL_MONITOR_TOPOLOGY

                    //Allow Specifies Control Access right to the user.
                    //Set the RIGHT_DS_REPL_MONITOR_TOPOLOGY
                    isControlAccessGranted = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                 "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                                 ClientUserName,
                                                                 ClientUserPassword,
                                                                 adDomain,
                                                                 guidDS_Repl_ManageE_Topology,
                                                                 ActiveDirectoryRights.ExtendedRight,
                                                                 AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessGranted, "Allowed the ExtendedRight control access right.");
                    //the result count should  not be 0 as both the access rights and control access rights are granted
                    SearchResponse searchResponseNCReplOutboundNeighborMonitorTopology;

                    //Search for the attribute msds-NCReplOutboundNeighbors.
                    searchResponseNCReplOutboundNeighborMonitorTopology = ADTSHelper.SearchObject(
                                                     "CN=ms-DS-NC-Repl-Cursors,CN=Schema,CN=Configuration," + parsedDN,
                                                     "objectClass=attributeSchema",
                                                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                     new string[] { "ms-DS-NC-Repl-Cursors" },
                                                     connection);
                    TestClassBase.BaseTestSite.Assert.IsNotNull(searchResponseNCReplOutboundNeighborMonitorTopology, "the searchResponseNCReplOutboundNeighborMonitorTopology is not null");
                    #endregion

                    return errorstatus.success;

                }

                //if any AD Specific exception. 
                catch (DirectoryException e)
                {
                    //Failure has happened while performing Ldap operation.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;
                }
            }

            #endregion

            #region Deny Access Requirements

            //if the Access is not set ,READ Access and specified Control Access
            //Deny the Ldap operation
            else if ((attr == AttribsToCheck.msDS_NCReplCursors) &&
                     ((accessRight != AccessRights.DS_READ_REPLVECTOR) &&
                     (ctrlAccessRight != ControlAccessRights.Repl_Mon_Topo_REPLVECTOR) &&
                     (ctrlAccessRight != ControlAccessRights.Repl_Man_Topo_REPLVECTOR)))
            {
                try
                {
                    #region Remove ManageTopology .MonitorTopology Control Access rights and Read property Access Rights

                    //Set the control access right to deny 
                    //deny Repl_Manage_Topology Control Access Right
                    bool isManageControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                    "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    guidDS_Repl_ManageE_Topology,
                                                    ActiveDirectoryRights.ExtendedRight,
                                                    AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccess, "Deny the ExtendedRight control access right.");
                    //Set the control access right to deny 
                    //deny Repl_Monitor_Topology Control Access Right
                    bool isMonitorControlAccess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                    "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                    ClientUserName,
                                                    ClientUserPassword,
                                                    adDomain,
                                                    guidDS_Replication_Monitor_Topology,
                                                    ActiveDirectoryRights.ExtendedRight,
                                                    AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccess, "Deny the ExtendedRight control access right.");
                    //Set the Read access right to deny 
                    //Deny READ Access right
                    bool isReadAccessRemoved = ADTSHelper.SetAccessRights(PdcFqdn, 
                                                        "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                        ClientUserName,
                                                        ClientUserPassword,
                                                        adDomain,
                                                        ActiveDirectoryRights.ReadProperty,
                                                        AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isReadAccessRemoved, "Deny the ReadProperty control access right.");
                    #endregion

                    #region Validation

                    //Search for the specifies attribute after removing both the access right and control access
                    //the count should be 0
                    SearchResponse searchResponseNCReplOutboundNeighbors = ADTSHelper.SearchObject(
                                          "CN=ms-DS-NC-Repl-Cursors,CN=Schema,CN=Configuration," + parsedDN,
                                          "objectClass=attributeSchema",
                                          System.DirectoryServices.Protocols.SearchScope.Subtree,
                                          new string[] { "ms-DS-NC-Repl-Cursors" },
                                          connection);

                    //check whethether the specifies attribute is present in the search response returned.
                    //The result should be false as the access rights are removed ,the attribute should not return. 

                    //after getting the result store it in a variable 
                    int isNCReplOutboundNeighborsExist = searchResponseNCReplOutboundNeighbors.Entries[0].Attributes.Count;
                    //Rollback to previous right because if an error or an exception occurs the user will not have access right
                    //as the rights were denied and subsequent test cases will fail. so roll back to previous rights
                    //and then validate

                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    //Rollback the ReadAccess
                    //Remove the Deny Access Right
                    bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                                                               "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                                               AdminUser,
                                                                               AdminUserPassword,
                                                                               adDomain,
                                                                               ActiveDirectoryRights.ReadProperty,
                                                                               AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Deny the ReadProperty control access right.");
                    //Setting the Access Right for the user.
                    //Set Read Access Right.
                    bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                                                                "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                                                                AdminUser,
                                                                                AdminUserPassword,
                                                                                ClientUserName,
                                                                                adDomain,
                                                                                ActiveDirectoryRights.ReadProperty,
                                                                                AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allow the ReadProperty control access right for current user.");
                    //Rollback To Control Access Right
                    //Remove the deny access
                    bool isManageControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccessSet, "Deny the ExtendedRight control access right.");
                    //Set the Control access right 
                    bool isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                    "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                    ClientUserName,
                                    ClientUserPassword,
                                    adDomain,
                                    guidDS_Repl_ManageE_Topology,
                                    ActiveDirectoryRights.ExtendedRight,
                                    AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right for current user.");
                    //Remove the deny access
                    bool isMonitorControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccessSet, "Deny the ExtendedRight control access.");
                    //Set the Control access right 
                    isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right for current user.");
                    #endregion

                    //Rollback is done and validate now.
                    //Check for the attribute in entries of returned search response.

                    //Validate MS-ADTS-Security_76
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<int>(0, isNCReplOutboundNeighborsExist, 76,
                        @"While accessing msDS-NCReplCursors attribute,if The security context of the requestor is not  
                        granted the following rights on replUpToDateVector: (RIGHT_DS_READ_PROPERTY) and (RIGHT_DS_REPL_MANAGE_TOPOLOGY)
                        and (RIGHT_DS_REPL_MONITOR_TOPOLOGY),then the value is treated as does not exist in the returned attributes and 
                        the LDAP filter.");

                    return errorstatus.failure;

                    #endregion

                }

                catch (DirectoryException)
                {
                    #region RollBack the Removed Operations

                    //Rollback to previous Access Rights.
                    //Delete the Denied AccessRight
                    //Set the AccessRight and Control Access Right

                    //Rollback the ReadAccess
                    bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, 
                                    "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                    AdminUser,
                                    AdminUserPassword,
                                    adDomain,
                                    ActiveDirectoryRights.ReadProperty,
                                    AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Deny the ReadProperty control access right for admin user.");
                    //Setting the Access Right for the user.
                    bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   AdminUser,
                                   AdminUserPassword,
                                   ClientUserName,
                                   adDomain,
                                   ActiveDirectoryRights.ReadProperty,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allow the ReadProperty control access right for current user.");
                    //Rollback To Control Access Right

                    //Remove the deny access
                    bool isManageControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isManageControlAccessSet, "Deny the ExtendedRight control access right for current user.");
                    //Set the Control access right 
                    bool isControlAccessRightSet = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Repl_ManageE_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightSet, "Allow the ExtendedRight control access right for current user.");
                    //Remove the deny access
                    bool isMonitorControlAccessSet = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isMonitorControlAccessSet, "Deny the ExtendedRight control access right for current user.");
                    //Set the Control access right 
                    isControlAccessRightSet = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                   "CN=Repl-UpToDate-Vector,CN=Schema,CN=Configuration," + parsedDN,
                                   ClientUserName,
                                   ClientUserPassword,
                                   adDomain,
                                   guidDS_Replication_Monitor_Topology,
                                   ActiveDirectoryRights.ExtendedRight,
                                   AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightSet, "aAlow the ExtendedRight control access right for current user.");
                    #endregion

                    //Failure has happended while performing Ldap operation.
                    return errorstatus.failure;

                }

            }

            #endregion

            return errorstatus.success;
        }

        #endregion

        #region ValidateAnonymousUsersRequirements

        /// <summary>
        /// This method validate the requirements pertaining to Anonymouse user  (null credentials)
        /// </summary>
        /// <returns>returns status</returns>
        public errorstatus ValidateAnonymousUsersRequirements()
        {

            //set fLDAPBlockAnonOps dsHeuristic to true,Already fLDAPBlockAnonOps is set to true while binding.
            //users cannot do Read operation

            //Search for the properties of attribute for the given distinguishname of LdapConnection.
            SearchResponse anonySearchResponse = ADTSHelper.SearchObject("CN=Users," + parsedDN,
                                                               "(objectClass=user)",
                                                               System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                               new string[] { "name", "objectclass", "primaryGroupID" },
                                                               connection);

            //if search result is not null
            //then Anonymous use able to perform search
            //Validate
            TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(anonySearchResponse, 264,
                @"If the fLDAPBlockAnonOps dsHeuristic is false, anonymous (unauthenticated) 
                users can do a read operation");

            //set fLDAPBlockAnonOps dsHeuristic to false
            //users cannot do Read operation
            //Modify the dSHeuristics attribute.

            //Create Modify Request with dn and attribute values to modify.
            ModifyRequest modReq = new ModifyRequest("CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," + parsedDN,
                                            DirectoryAttributeOperation.Replace,
                                            "dSHeuristics",
                                            "0000000");

            //get the response.
            ModifyResponse modRes = (ModifyResponse)connection.SendRequest(modReq);

            //perform Read or search operation
            //the result should be null values as the user is anonymous and the value fLDAPBlockAnonOps is true;
            anonySearchResponse = ADTSHelper.SearchObject("CN=Users," + parsedDN,
                                                           "(objectClass=users)",
                                                           System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                           new string[] { "name", "objectclass", "primaryGroupID" },
                                                           connection);

            if (anonySearchResponse == null || anonySearchResponse.Entries.Count == 0)
            {
                //Validate MS-ADTS-Security_R262
                //The Response should be Null.
                TestClassBase.BaseTestSite.CaptureRequirement(262,
                   @"If the fLDAPBlockAnonOps dsHeuristic is true, anonymous (unauthenticated) users cannot do a read 
                   operation");

                //success
                return errorstatus.success;
            }
            else
            {
                return errorstatus.failure;
            }

        }

        #endregion

        #region ValidateDNSHostnameAndName

        /// <summary>
        /// This method validate the requirements of Validated Writes.
        /// </summary>
        /// <param name="accessRight">specifies access right</param>
        /// <param name="ctrlAccessRight">specifies control access right</param>
        /// <param name="attr">specifies attribute</param>
        /// <returns>returns result of the operation</returns>
        public errorstatus ValidateDNSHostname(AccessRights accessRight,
                                               ControlAccessRights ctrlAccessRight,
                                               AttribsToCheck attr)
        {

            #region AllowAccessRights

            //Validate dnsHostName
            //RIGHT_DS_WRITE_PROPERTY should be granted
            //ControlAccessRight is not Required
            if ((attr == AttribsToCheck.dnsHostName) &&
                ((accessRight == AccessRights.RIGHT_DS_WRITE_PROPERTY) || (accessRight == AccessRights.RIGHT_DS_WRITE_EXTENDED)) &&
                (ctrlAccessRight == ControlAccessRights.NotSet))
            {

                //Deny WriteProperty AccessRight to the specified user.
                bool isAccessRightSet = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                            ClientUserName,
                                            ClientUserPassword,
                                            adDomain,
                                            ActiveDirectoryRights.WriteProperty,
                                            AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightSet, "aAlow the WriteProperty control access right for current user.");
                //Allow the user GenericWrite to modify the dnsHostname
                //Grant GenericWrite or ValidatedWrite  AccessRight to the specified user.
                isAccessRightSet = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                            ClientUserName,
                                            ClientUserPassword,
                                            adDomain,
                                            ActiveDirectoryRights.GenericWrite,
                                            AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightSet, "aAlow the GenericWrite control access right for current user.");
                //Modify the dnsHostName after setting VW Access Right

                ResultCode modifyResult = ModifyOperationWithReturn(PdcFqdn, "dnshostname", EndpointFqdn);

                #region Validate MS-ADTS-Security_R415,499,500,501,505

                //Validate MS-ADTS-Security_R415
                //if the dnsHostName attribute is modified Successfully. 
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(ResultCode.Success, modifyResult, 415,
                        @"the authorization for the requester's security context for Checking Validated 
                        Write-Based Access is If the ACE type is Object Access Allowed the access right 
                        RIGHT_DS_WRITE_PROPERTY_EXTENDED (VW) is present in M, and the ObjectType field in the ACE 
                        contains a GUID value equal to G, then grant the requestor the requested Validated - write 
                        permission to change the validated DNS host name.");

                //Validate MS-ADTS-Security_R499
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(ResultCode.Success, modifyResult, 499,
                        @"The requester does not 
                        have RIGHT_DS_WRITE_PROPERTY access on an attribute, but has RIGHT_DS_WRITE_PROPERTY_EXTENDED 
                        access (also called validated write), then the write is allowed if The operation is either 
                        add value or remove value, and the value is the dNSHostname of the user object representing 
                        the requester");

                //Validate MS-ADTS-Security_R501
                //if the dnsHostName attribute is modified Successfully. 
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(ResultCode.Success, modifyResult, 501,
                        @"In AD DS, the value of the dNSHostName attribute being written is in 
                        computerName.fullDomainDnsName format, where computerName is the current sAMAccountName of 
                        the object (without the final $ character), and the fullDomainDnsName is the DNS name of the 
                        domain NC or one of the values of msDS-AllowedDNSSuffixes on the domain NC (if any) where the 
                        object that is being modified is located.");

                //Validate MS-ADTS-Security_R500
                //if the dnsHostName attribute is modified Successfully. 
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(ResultCode.Success, modifyResult, 500, 
                        @"The requester does not have 
                        RIGHT_DS_WRITE_PROPERTY access on an attribute, but has RIGHT_DS_WRITE_PROPERTY_EXTENDED 
                        access also called validated write)(modifying the dnshostnbame attribute)), then the write 
                        is allowed if The object has class computer or server.");

                //Validate MS-ADTS-Security_R505
                //if dnsHostName attribute is modified successfully.
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(ResultCode.Success, modifyResult, 505,
                      @"the requester does not have RIGHT_DS_WRITE_PROPERTY access on an attribute, but has 
                      RIGHT_DS_WRITE_PROPERTY_EXTENDED access ((also called validated write)(modifying the 
                      dnsHostName attribute)), then the write is allowed if The instance name matches one 
                      of the following: the full DNS name of the machine, the sAMAccountName of the machine 
                      (without the terminating ""$""), one of the msDS-AdditionalDnsHostName, or one of the 
                      msDS-AdditionalSamAccountName (without the terminating ""$"").");

                #endregion

                #region Rollback WriteProperty

                //Admin user will remove the current Deny WriteProperty Access Right of current user
                bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                              AdminUser,
                                              AdminUserPassword,
                                              adDomain,
                                              ActiveDirectoryRights.WriteProperty,
                                              AccessControlType.Deny);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Deny the WriteProperty control access right.");
                //Admin user will set the WriteProperty to Allow of current user
                bool isAccessRightAllowSet = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                                AdminUser,
                                                AdminUserPassword,
                                                ClientUserName,
                                                adDomain,
                                                ActiveDirectoryRights.WriteProperty,
                                                AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowSet, "Allow the WriteProperty control access right for current user.");
                #endregion

                // if the dnsHostname is modified successfully
                //return the status to success
                return errorstatus.success;
            }

            #endregion

            #region Deny Write permission

            //Deny the ValidatedWrite property
            //if the ValidatedWrite(GenericWrite) is denied ,it is not possible to modify dnsHostName
            else if ((attr == AttribsToCheck.dnsHostName) &&
                     ((accessRight != AccessRights.RIGHT_DS_WRITE_PROPERTY) || (accessRight != AccessRights.RIGHT_DS_WRITE_EXTENDED)) &&
                     (ctrlAccessRight == ControlAccessRights.NotSet))
            {
                bool isAccessRightSet = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                           ClientUserName,
                                           ClientUserPassword,
                                           adDomain,
                                           ActiveDirectoryRights.WriteProperty,
                                           AccessControlType.Deny);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightSet, "Deny the WriteProperty control access right for current user.");
                //To get the Exception if an Exception is occurred
                //Expected Exception :InSufficientAccessRights.
                string strException = string.Empty;

                //Specifies Attribute name to modify,not s configurable value,it is a constant.
                string attributeName = "name";

                //Invoke the method to get insufficientAccessRights
                //The result validates MS-ADTS-Security_R510 requirement if the result is equal to insufficientAccessRights
                bool isFailedname = ModifyOperationWithReturn(PdcFqdn, attributeName, namevalue, out strException);

                //Deny the GenericWrite Access Right to the user
                isAccessRightSet = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                           ClientUserName,
                                           ClientUserPassword,
                                           adDomain,
                                           ActiveDirectoryRights.GenericWrite,
                                           AccessControlType.Deny);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightSet, "Deny the GenericWrite control access right for current user.");
                //Try to Modify the dnsHostName
                ResultCode modifyResult = ModifyOperationWithReturn(PdcFqdn, "dnshostname", EndpointFqdn);

                //Rollback to previous Rights
                //RollBack before doing validation to avoid skip of Rollback in case of failure in the 
                //Validation of Requirement.

                #region Rollback

                //Admin User will remove the current Deny write property Access Right for current user.
                bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                           AdminUser,
                                           AdminUserPassword,
                                           adDomain,
                                           ActiveDirectoryRights.WriteProperty,
                                           AccessControlType.Deny);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Deny the WriteProperty control access right.");
                //Admin User will set the Write property Access Right to Allow for current user.
                bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                           AdminUser,
                                           AdminUserPassword,
                                           ClientUserName,
                                           adDomain,
                                           ActiveDirectoryRights.WriteProperty,
                                           AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allow the WriteProperty control access right for current user.");
                //Admin User will remove the current Deny GenericWrite Access Right for current user.
                isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                           AdminUser,
                                           AdminUserPassword,
                                           adDomain,
                                           ActiveDirectoryRights.GenericWrite,
                                           AccessControlType.Deny);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Deny the GenericWrite control access right.");
                //Admin User will set the GenericWrite Access Right to Allow for current user.
                isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                           AdminUser,
                                           AdminUserPassword,
                                           ClientUserName,
                                           adDomain,
                                           ActiveDirectoryRights.GenericWrite,
                                           AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allow the GenericWrite control access right for current user.");
                #endregion

                //Validate MS-ADTS-Security_R508
                //if the result is insufficientAccessRights then validate the requirement
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(modifyResult, ResultCode.InsufficientAccessRights, 508,
                    @"For originating updates if The security context of the requester is not 
                    granted rights RIGHT_DS_WRITE_PROPERTY permission on 
                    O!name to perform move operation, then server returns LDAP error insufficientAccessRights.");

                //Validate MS-ADTS-Security_R419
                //Cannot modify dnshostname.
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(modifyResult, ResultCode.InsufficientAccessRights, 419,
                    @"the authorization for the requester's security context for Checking Validated 
                    Write-Based Access is If the ACE type is Object Access Denied the access right 
                    RIGHT_DS_WRITE_PROPERTY_EXTENDED (VW) is present in M, and the ObjectType field in the ACE 
                    contains a GUID value equal to G, then deny the requestor the requested Validated - write 
                    permission to change the validated DNS host name.");

                //Validate MS-ADTS-Security_R493
                //If accessRight is not RIGHT_DS_WRITE_PROPERTY
                //then dnsHostName attribute is modified unsuccessfully.
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(
                    modifyResult, 
                    ResultCode.InsufficientAccessRights,
                    493,
                    @"For an originating update the requester needs to have 
                     RIGHT_DS_WRITE_PROPERTY access to all attributes being directly affected by the modify operation");

                //Validate MS-ADTS-Security_R510
                //if the result is insufficientAccessRights then validate the req
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<string>(strInsufficientRights, strException, 510,
                    @"For originating updates if The security context of the requester is not 
                    granted rights RIGHT_DS_WRITE_PROPERTY permission on 
                    O!name to perform rename  operation, then server returns LDAP error insufficientAccessRights.");

                //end in failure as the operation failed to modify dnsHostname attribute.
                return errorstatus.failure;

            }

            #endregion

            return errorstatus.failure;
        }

        #endregion

        #region ValidateServicePrincipalName

        /// <summary>
        /// This method validate the requirements of servicePrincipleName attribute.
        /// </summary>
        /// <param name="accessRight">specifies access right</param>
        /// <param name="ctrlAccessRight">specifies control access right</param>
        /// <param name="attr">specifies attribute</param>
        /// <returns>returns reuslt of the operation</returns>
        public errorstatus ValidateServicePrincipalName(AccessRights accessRight,
                                                        ControlAccessRights ctrlAccessRight,
                                                        AttribsToCheck attr)
        {

            #region AllowAccessRights

            //Validate servicePrincipleName Attribute
            //RIGHT_DS_WRITE_EXTENDED should be granted
            //ControlAccessRight is not Required
            if ((attr == AttribsToCheck.servicePrincipleName) &&
                ((accessRight == AccessRights.RIGHT_DS_WRITE_EXTENDED) || (accessRight == AccessRights.RIGHT_DS_WRITE_PROPERTY)) &&
                (ctrlAccessRight == ControlAccessRights.NotSet))
            {

                //Deny the WriteProperty to the specified user
                bool isAccessRightSet = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                            ClientUserName,
                                            ClientUserPassword,
                                            adDomain,
                                            ActiveDirectoryRights.WriteProperty,
                                            AccessControlType.Deny);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightSet, "Deny the WriteProperty control access right for current user.");
                //Set Allow the GenericWrite Propery
                isAccessRightSet = ADTSHelper.SetAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                            ClientUserName,
                                            ClientUserPassword,
                                            adDomain,
                                            ActiveDirectoryRights.GenericWrite,
                                            AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightSet, "Allow the GenericWrite control access right for current user.");
                //Modify the servicePrincipalName attribute after GenericWrite is set to Allow
                ResultCode modifyResult = ModifyOperationWithReturn(PdcFqdn, "servicePrincipalName", EndpointSPN);

                //Search
                SearchResponse srchResponse = ADTSHelper.SearchObject(
                                                        "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                                        "objectClass=computer",
                                                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                        null,
                                                        connection);

                bool isExist = false;

                //if the count of entries is greater than 0.
                if (srchResponse.Entries.Count > 0)
                {
                    isExist = true;
                }

                //Validate MS-ADTS-Security_R504
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExist, 504, @"the requester does not have 
                     RIGHT_DS_WRITE_PROPERTY access on an attribute, but has RIGHT_DS_WRITE_PROPERTY_EXTENDED 
                     access (also called validated write),then the write is allowed for servicePrincipalName ,
                     if The object has class computer (or a subclass of computer).");

                #region Rollback WriteProperty

                //Admin user will remove the current Deny WriteProperty Access Right of current user
                bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                              AdminUser,
                                              AdminUserPassword,
                                              adDomain,
                                              ActiveDirectoryRights.WriteProperty,
                                              AccessControlType.Deny);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Deny the WriteProperty control access right.");
                //Admin user will set the WriteProperty to Allow of current user
                bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + ENDPOINTNetbiosName + ",CN=Computers," + parsedDN,
                                                AdminUser,
                                                AdminUserPassword,
                                                ClientUserName,
                                                adDomain,
                                                ActiveDirectoryRights.WriteProperty,
                                                AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allow the WriteProperty control access right for current user.");
                #endregion

                //if the servicePrincipalName attribute is modified successfully
                return errorstatus.success;
            }

            #endregion

            #region RIGHT_DS_WRITE_EXTENDED NotSet

            //if RIGHT_DS_WRITE_EXTENDED is not set
            else if ((attr == AttribsToCheck.servicePrincipleName) &&
               ((accessRight != AccessRights.RIGHT_DS_WRITE_EXTENDED) || (accessRight != AccessRights.RIGHT_DS_WRITE_PROPERTY)) &&
               (ctrlAccessRight == ControlAccessRights.NotSet))
            {
                return errorstatus.failure;
            }

            #endregion

            return errorstatus.success;
        }

        #endregion

        #region ValidatewriteDACLOperations

        /// <summary>
        /// This method validates the requirements related to WriteDACL in nTSecurityDescriptor.
        /// </summary>
        /// <param name="accessRight"></param>
        /// <param name="ctrlAccessRight"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public errorstatus ValidatewriteDACLOperation(AccessRights accessRight,
                                                      ControlAccessRights ctrlAccessRight,
                                                      AttribsToCheck attr)
        {

            //if AccessRight is WriteDACL and the Attribute is DaclOpearyion
            //ControlAccssRight is not required.
            if ((accessRight == AccessRights.RIGHT_WRITE_DAC) &&
                (ctrlAccessRight == ControlAccessRights.NotSet) &&
                (attr == AttribsToCheck.writeDACLOperation))
            {

                #region NullDACL in nTSecurityDescriptor attribute

                //Remove Access
                bool isAccessRightRemoved = ADTSHelper.RemoveAccessRights(PdcFqdn, "CN=" + NullDaclUser + ",cn=users," + parsedDN,
                                                AdminUser,
                                                AdminUserPassword,
                                                adDomain,
                                                ActiveDirectoryRights.GenericRead,
                                                AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightRemoved, "Allow the GenericRead control access right.");

                bool isAccessRightAllowed = ADTSHelper.SetAccessRightsAllow(PdcFqdn, "CN=" + NullDaclUser + ",cn=users," + parsedDN,
                                                AdminUser,
                                                AdminUserPassword,
                                                NullDaclUser,
                                                adDomain,
                                                ActiveDirectoryRights.GenericRead,
                                                AccessControlType.Allow);
                TestClassBase.BaseTestSite.Assert.IsTrue(isAccessRightAllowed, "Allow the GenericRead control access right for nullDaclUser.");

                //Check for he presence of a NULL DACL in the nTSecurityDescriptor attribute of an object
                bool isNullDacl = ADTSHelper.NullSecurityDescriptor(PdcFqdn, "CN=" + NullDaclUser + ",cn=users," + parsedDN,
                                                                     NullDaclUser,
                                                                     NullDaclUserPassword);
                //if  NULL DACL 
                if (isNullDacl)
                {
                    //Read the content of the object to check the existence of entries.
                    SearchResponse nullDaclResponse = ADTSHelper.SearchObject("CN=" + NullDaclUser + ",cn=users," +
                                            parsedDN, "objectclass=user",
                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                            new string[] { "name" }, connection);

                    //if the entries count is greater than 0 in the response. 
                    bool isExisted = nullDaclResponse.Entries.Count > 0;

                    //ValidateMS-ADTS-Security_R334
                    //if able to read 
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 334, @"The presence of a NULL 
                        DACL in the nTSecurityDescriptor attribute of an object grants rights to read the contents of the object.");

                    //Again invoke NullSecurityDescriptor method, to check even after removing the rights, 
                    //the user can modify the DACL value.

                    //Reset to false
                    isExisted = false;
                    if (ADTSHelper.NullSecurityDescriptor(PdcFqdn,
                                                           "CN=" + NullDaclUser + ",cn=users," + parsedDN,
                                                           NullDaclUser,
                                                           NullDaclUserPassword))
                    {
                        //set to true if Null DACL is present
                        isExisted = true;
                    }

                    //ValidateMS-ADTS-Security_R335
                    //if able to read 
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 335, @"The presence of a NULL 
                        DACL in the nTSecurityDescriptor attribute of an object grants right to modify the DACL in 
                        the object security descriptor  to the object to any principal that requests it.");

                }

                #endregion

                #region NoDACL in ntSecurityDescriptor attribute

                //if There is No DACL in ntSecurityDescriptor Attribute
                bool isNoDACL = ADTSHelper.NoDacl(PdcFqdn,
                                                    "CN=" + NoDaclUser + ",cn=users," + parsedDN,
                                                    NoDaclUser,
                                                    NoDaclUserPassword);
                if (isNoDACL)
                {
                    //Read the object 
                    SearchResponse nullDaclResponse = ADTSHelper.SearchObject("CN=" + NoDaclUser + ",cn=users," + parsedDN,
                                                            "objectclass=user",
                                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                            new string[] { "name" }, connection);

                    //the entries count should be greater than 0 as the user will have all rights to read the object
                    bool isExisted = nullDaclResponse.Entries.Count > 0;

                    //Validate MS-ADTS-Security_R341
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 341, @"G and D denote the access rights 
                        that are granted and denied, respectively, on the object.If the security descriptor has no 
                        DACL or its DACL Present (DP) bit is not set, then grant the requester all possible 
                        access(grant permission to read name atribute) rights on the object.");

                    //Reset the variable to false
                    isExisted = false;

                    //Set the control Access Rights to the specified user
                    bool isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn, "CN=" + NoDaclUser + ",cn=users," + parsedDN,
                                                     NoDaclUser,
                                                     NoDaclUserPassword,
                                                     adDomain,
                                                     guidDS_Replication_Monitor_Topology,
                                                     ActiveDirectoryRights.ExtendedRight,
                                                     AccessControlType.Allow);

                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right for nullDaclUser.");

                    //Search the object after setting the control access right.
                    SearchResponse noDaclsearchResponse = ADTSHelper.SearchObject("CN=" + NoDaclUser + ",cn=users," + parsedDN,
                                                                            "objectclass=user",
                                                                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                            new string[] { "msDS-ReplAttributeMetaData" },
                                                                            connection);

                    //if the count of entries is greater than 0
                    isExisted = noDaclsearchResponse.Entries.Count > 0;

                    //Validate MS-ADTS-Security_R401
                    //if the Entries in the response are greater than 0 indicates 
                    //the Access rights are present 
                    //Since access is granted ,Entries count is greater than 0.
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 401, @"r be the root node of the object 
                        type tree T. Further, label each node v that is an element of V with two additional labels 
                        called Grant(v) and Deny(v) indicating the access rights that are granted and denied, 
                        respectively, at that node. Set both labels to a value 0 initially for every node  If the 
                        security descriptor of object O has no DACL or its DACL Present (DP) bit is not set, then 
                        grant the requester all access (access to msDS-ReplAttributeMetaData attribute) on the 
                        object");

                    //Validate MS-ADTS-Security_R405
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isExisted, 405, @"the authorization for the requester's security 
                        context for Checking Control Access Right-Based Access is  If the security descriptor has no 
                        DACL or its DACL Present (DP) bit is not set, then grant the requestor requested control 
                        access right (allow the access to msDS-ReplAttributeMetaData attribute).");

                }

                #endregion

                #region NoACE
                SearchResponse searchResp = ADTSHelper.SearchObject(
                                                        "CN=" + NoAceUser + ",cn=users," + parsedDN,
                                                        "objectclass=user",
                                                        System.DirectoryServices.Protocols.SearchScope.Base,
                                                        new string[] { "name" },
                                                        connection);
                if (searchResp.Entries.Count != 0)
                {
                    //Give the Control Access Right
                    bool isControlAccessRight = ADTSHelper.SetControlAcessRights(PdcFqdn,
                                                     "CN=" + NoAceUser + ",cn=users," + parsedDN,
                                                      AdminUser,
                                                      AdminUserPassword,
                                                      adDomain,
                                                      guidDS_Replication_Monitor_Topology,
                                                      ActiveDirectoryRights.ExtendedRight,
                                                      AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRight, "Allow the ExtendedRight control access right.");
                    //sleep 2 seconds to wait the change be available
                    Thread.Sleep(2000);

                    //Check whether all the the ACE Rights are removed for the specified user
                    bool isNoAce = ADTSHelper.RemoveAllACE(PdcFqdn,
                                                           "CN=" + NoAceUser + ",cn=users," + parsedDN,
                                                           AdminUser,
                                                           AdminUserPassword);
                    if (!isNoAce)
                    {
                        Site.Log.Add(LogEntryKind.Debug, "Remove all ACE failed.");
                    }
                }

                //Search the object after denying or removing all the permissions
                SearchResponse searchNameResp = ADTSHelper.SearchObject(
                                                    "CN=" + NoAceUser + ",cn=users," + parsedDN,
                                                    "objectclass=user",
                                                    System.DirectoryServices.Protocols.SearchScope.Base,
                                                    new string[] { "name" },
                                                    connection);

                //The count should be 0 since there are no permissions
                bool isNoEntries = searchNameResp.Entries.Count == 0;

                //Validate MS-ADTS-Security_R342
                //if there are no entries in the returned searchResponse
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isNoEntries, 342, @"G and D denote the access rights 
                        that are granted and denied, respectively, on the object. If the DACL does not have any ACE, 
                        then grant the requester no access rights (do not grant permission to read name attribute) 
                        on the object.");

                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isNoEntries, 338, @"An empty DACL in the 
                        nTSecurityDescriptor attribute of an object does not  grants right to right to read data 
                        from the security descriptor of the object.");

                //Reset to false
                isNoEntries = false;

                //Search the object after setting control access right
                SearchResponse searchReplAttrResp = ADTSHelper.SearchObject("CN=" + NoAceUser + ",cn=users," + parsedDN,
                                                "objectclass=user",
                                                System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                new string[] { "msDS-ReplAttributeMetaData" },
                                                connection);

                //The result should be 0
                isNoEntries = searchReplAttrResp.Entries.Count == 0;

                //Validate MS-ADTS-Security_R402
                //The response should not contain any entries.
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isNoEntries, 402, @"r be the root node of
                        the object type tree T. Further, label each node v that is an element of V with two additional
                        labels called Grant(v) and Deny(v) indicating the access rights that are granted and denied, 
                        respectively, at that node. Set both labels to a value 0 initially for every node. If the 
                        DACL does not have any ACE, then grant the requester no access rights (deny the access to 
                        msDS-ReplAttributeMetaData attribute) on the object.");

                //Validate MS-ADTS-Security_R406
                //The response should not contain any entries.
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isNoEntries, 406, @"the authorization for 
                        the requester's security context for Checking Control Access Right-Based Access is If the
                        DACL does not have any ACE, then deny the requestor requested control access right 
                        (access to  the msDS-ReplAttributeMetaData attribute).");

                //Validate MS_AD_Security_R410
                //The response should not contain any entries.
                TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(isNoEntries, 410, @"the authorization for 
                        the requester's security context for Checking Control Access Right-Based Access is If the 
                        ACE type is Object Access Denied, the access right RIGHT_DS_CONTROL_ACCESS (CR) is 
                        present in M, and the ObjectType field in the ACE is not present, then deny the 
                        requestor  the requested control access right (access to  the msDS-ReplAttributeMetaData 
                        attribute.)");

                string entry = string.Format("CN={0},CN=Computers,{1}", ENDPOINTNetbiosName, parsedDN);

                Common.Utilities.BackupOrRestoreNtSecurityDescriptor(
                    PdcFqdn,
                    int.Parse(ADDSPortNum),
                    entry,
                    Common.Utilities.SecurityDescriptorBackupFilename,
                    new NetworkCredential(DomainAdministratorName, DomainUserPassword, PrimaryDomainDnsName));

                DirectoryEntry dirEntry = new DirectoryEntry(
                 string.Format("LDAP://{0}/{1}", PdcFqdn, entry),
                DomainAdministratorName, DomainUserPassword,
                AuthenticationTypes.Secure);
                IADsSecurityDescriptor sd;

                sd = (IADsSecurityDescriptor)dirEntry.Properties["nTSecurityDescriptor"].Value;
                IADsAccessControlList dacl = (IADsAccessControlList)sd.DiscretionaryAcl;
                // Set PD bit, disable inheritance.
                sd.Control |= 0x1000; //[MS-DTYP] 2.4.6 SECURITY_DESCRIPTOR
                // Remove Other ACEs.
                foreach (AccessControlEntry ace in dacl)
                {
                    dacl.RemoveAce(ace);
                }

                // Add ace Deny the RIGHT_DS_WRITE_PROPERTY_EXTENDED of the client user.
                AccessControlEntry myace = new AccessControlEntry();
                myace.Trustee = PrimaryDomainNetBiosName + "\\" + ClientUserName;
                myace.AceType = (int)AccessControlType.Deny;
                myace.AccessMask = 0x08; // [MS-ADTS] Section 5.1.3.2 Access Rights
                dacl.AddAce(myace);
                sd.DiscretionaryAcl = dacl;
                dirEntry.Properties["nTSecurityDescriptor"].Value = sd;
                dirEntry.CommitChanges();

                ResultCode modifyResult = ModifyOperationWithReturn(PdcFqdn, "serviceprincipalname", EndpointSPN);

                Common.Utilities.BackupOrRestoreNtSecurityDescriptor(
                    PdcFqdn,
                    int.Parse(ADDSPortNum),
                    string.Format("CN={0},CN=Computers,{1}", ENDPOINTNetbiosName, parsedDN),
                    Common.Utilities.SecurityDescriptorBackupFilename,
                    new NetworkCredential(DomainAdministratorName, DomainUserPassword, PrimaryDomainDnsName));

                //Validate MS_AD_Security_R417
                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual(modifyResult, ResultCode.InsufficientAccessRights, 417, @"the authorization for the requester's 
                        security context for Checking Validated Write-Based Access is If the ACE type is Object 
                        Access Denied, the access right RIGHT_DS_WRITE_PROPERTY_EXTENDED (VW) is present in M, 
                        and the ObjectType field in the ACE is not  present, then deny the requestor the requested 
                        validated - write (permission to change the validated DNS host name).");

                #endregion

                return errorstatus.success;
            }

            //if AccessRight is WriteDACL and the Attribute is DaclOperation
            else if ((accessRight != AccessRights.RIGHT_WRITE_DAC) &&
                (ctrlAccessRight == ControlAccessRights.NotSet) &&
                (attr == AttribsToCheck.writeDACLOperation))
            {
                return errorstatus.failure;
            }

            return errorstatus.failure;

        }

        #endregion

        #region ValidateMoveOperation

        /// <summary>
        /// This method validates the requirements related to WriteDACL in nTSecurityDescriptor.
        /// </summary>
        /// <param name="accessRight"></param>
        /// <param name="ctrlAccessRight"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public errorstatus ValidateMoveOperation(AccessRights accessRight,
                                                      ControlAccessRights ctrlAccessRight,
                                                      AttribsToCheck attr)
        {

            //if AccessRight is WriteDACL and the Attribute is DaclOpearyion
            //ControlAccssRight is not required.
            if (((accessRight == AccessRights.RIGHT_DS_WRITE_PROPERTY || accessRight == AccessRights.RIGHT_DS_CREATE_CHILD)) &&
                (ctrlAccessRight == ControlAccessRights.NotSet) &&
                (attr == AttribsToCheck.moveOperation))
            {
                return errorstatus.success;
            }
            //if AccessRight is WriteDACL and the Attribute is DaclOperation
            else if ((accessRight != AccessRights.RIGHT_WRITE_DAC) &&
                (ctrlAccessRight == ControlAccessRights.NotSet) &&
                (attr == AttribsToCheck.moveOperation))
            {
                return errorstatus.failure;
            }
            return errorstatus.failure;
        }

        #endregion

        #region PasswordChange

        #region ValidatePasswordChangeOperation

        /// <summary>
        /// This method validate the requirements of Change password operation
        /// </summary>
        /// <param name="vdel">specifies old password</param>
        /// <param name="fAllowOverNonSecure">specifies secure connection is allowed or not</param>
        /// <param name="adType">Indicate if the test case is running against DS or LDS</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public errorstatus ValidatePasswordChangeOperation(Password vdel,
                                                           bool fAllowOverNonSecure,
                                                           ADTypes adType)
        {

            #region PasswordChangeSuccessful

            // SASL Bind on Regular or Protected ports
            // Simple Bind on Protected Ports
            // Simple Bind on Regular Ports with TLS Enabled
            // Simple Bind on Regular Ports
            // In Windows Server 2008 and later, if the 
            // fAllowPasswordOperationsOverNonSecureConnection flag of the 
            // dSHeuristics attribute (section 7.1.1.2.4.1.2) equals true and 
            // Active Directory is operating as AD LDS, then the DC permits 
            // modification of the unicodePwd attribute over a connection that is 
            // neither SSL/TLS-encrypted nor SASL-encrypted 
            // If ctrlAccessRights bit is set to User_Change_Password,
            // then userChangePwd is set to true
            // For user change password, the old password is necessary

            //passwordChange Succssful conditions
            if ((userChangePwd == true) &&
                (vdel == Password.validPassword) &&
                (((connection.AuthType == AuthType.Basic) &&
                ((enumPortNum == Port.LDAP_SSL_PORT) || (enumPortNum == Port.LDAP_SSL_GC_PORT))) ||//if Basic binding and protected ports

                ((connection.AuthType == AuthType.Basic) &&
               ((enumPortNum == Port.LDAP_GC_PORT) || (enumPortNum == Port.LDAP_PORT)) && isTlsConnection == true) ||

               ((connection.AuthType == AuthType.Basic) &&
               ((enumPortNum == Port.LDAP_GC_PORT) || (enumPortNum == Port.LDAP_PORT)) &&
                (PDCOSVersion >= ServerVersion.Win2008) && (ADTypes.AD_LDS == adType) &&
                (fAllowOverNonSecure)) ||//2008Server,ADLDS and fAllowOverNonSecure

                ((connection.AuthType != AuthType.Basic) && (isSASLBindSuccessful))))//SASL Bind Successful
            {

                if (!fAllowOverNonSecure)
                {
                    //Secured Connection
                    connection.SessionOptions.SecureSocketLayer = true;
                }
                //Setting protocl version to 3.
                connection.SessionOptions.ProtocolVersion = 3;

                try
                {
                    #region Search operation

                    //Validate MS-ADTS-Security_R34
                    //Search for unicodePwd attribute.
                    SearchResponse unicodePwdSearchResponse = ADTSHelper.SearchObject(
                                       "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                       "objectclass=user",
                                       System.DirectoryServices.Protocols.SearchScope.Subtree,
                                       new string[] { "unicodePwd" },
                                       connection);

                    #endregion

                    #region Validations

                    //Setting Access Control Rights to change password
                    //Set the control access right  to perform changePassword Ldap operation.
                    bool isResult = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                                     "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                                     ClientUserName,
                                                                     ClientUserPassword,
                                                                     adDomain,
                                                                     guidUser_Change_Password,
                                                                     ActiveDirectoryRights.ExtendedRight,
                                                                     AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isResult, "Allow the ExtendedRight control access right for current user.");
                    //Try to change the password after setting the control access.           
                    //To Change the password
                    //Replace the oldpasword with new password
                    bool ispwdChanged = ADTSHelper.ChangePassword(connection,
                                                             "CN=" + PwdChangedUser + ",CN=Users," + parsedDN,
                                                              PwdChangedUserOldPassword,
                                                              PwdChangedUserNewPassword);

                    bool isInetPwdChanged = ADTSHelper.ChangePassword(connection,
                                                             "CN=" + PwdChangedUser + ",CN=Users," + parsedDN,
                                                              PwdChangedUserOldPassword,
                                                              PwdChangedUserNewPassword);

                    //The ChangePassword method changes the unicode password on a user or InetOrg object
                    //if it is changed successfully then validate 
                    //Validate MS-ADTS-Security_R31
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                        ispwdChanged && isInetPwdChanged,
                        31,
                        @"Active Directory stores the password on a user object 
                        or inetOrgPerson object in the unicodePwd attribute");

                    //Validate MS-ADTS-Security_R484
                    DirectoryEntry ent = new DirectoryEntry(
                        string.Format(
                        "LDAP://{0}/CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,{1}",
                        PdcFqdn, 
                        ADTSHelper.ParseDomainName(PrimaryDomainDnsName)));

                    string dSHeuristics = ent.Properties["dSHeuristics"][0].ToString();

                    if (dSHeuristics.Length < 9 ? true : (dSHeuristics[8] != '0' || dSHeuristics[8] != '2'))
                    {
                        //if the password is modified successfully
                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(ispwdChanged, 484, @"Active Directory 
                        supports modifying passwords on objects via the userPassword attribute  the DC is running as AD DS and the 
                        domain functional level is DS_BEHAVIOR_WIN2003 or greater, and (2) fUserPwdSupport is true in the dSHeuristics
                        attribute.");
                    }

                    //Validate MS-ADTS-Security_R495
                    //ctrlAccessRights bit has been set to User_Change_Password
                    //If the password is modified successfully, the req is validated.
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(ispwdChanged, 495, @"For an originating update In AD LDS, 
                        if a password value is being modified as a password change operation, then the requester needs to have the
                        User-Change-Password control access right on the object being modified");

                    //Condition is checked initial
                    //Validate if password is changed successfully
                    //Validate MS-ADTS-Security_R758
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(ispwdChanged, 758, @"On Windows Server 2003 and later, the DC 
                        also permits modification of the unicodePwd attribute on a connection protected by SSL/TLS connection");

                    //Rolling back the changed user password.
                    //Reset to old password again to support multiple runs.
                    bool isChangepassword = ADTSHelper.ChangePassword(connection,
                                               "CN=" + PwdChangedUser + ",CN=Users," + parsedDN,
                                                PwdChangedUserNewPassword,
                                                PwdChangedUserOldPassword);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isChangepassword, "Change Password success.");
                    #endregion
                    return errorstatus.success;
                }
                //if Exception
                catch (LdapException)
                {
                    return errorstatus.failure;
                }
            }

            #endregion

            #region PasswordChange Failure

            //if Password.invalidPassword
            //Not a Secured Connection
            else if (
                (vdel == Password.invalidPassword) || (userChangePwd == false) ||
                ((connection.AuthType == AuthType.Basic) &&
                ((enumPortNum == Port.LDAP_PORT) || (enumPortNum == Port.LDAP_GC_PORT)) && (isTlsConnection == false)) ||
                ((fAllowOverNonSecure == false) && (PDCOSVersion >= ServerVersion.Win2008) && (ADTypes.AD_LDS == adType)))
            {
                try
                {
                    //Invoke password change with invalid password
                    bool ispwdChanged = ADTSHelper.PasswordChange(connection,
                                               "CN=" + PwdChangedUser + ",CN=Users" + parsedDN,
                                                InvalidPassword,
                                                PwdChangedUserNewPassword);

                    //Check operation is failed or not
                    TestClassBase.BaseTestSite.Assert.AreEqual<bool>(false, ispwdChanged, "Password Operations fails");

                    //return failure
                    return errorstatus.failure;
                }

                catch (LdapException e)
                {
                    //return failure
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;
                }
            }

            #endregion

            else
            {
                return errorstatus.failure;
            }
        }

        #endregion

        #region AdminPasswordReset
        /// <summary>
        /// AdminPasswordReset deals with administrative change of a password.
        /// The old password is not required.
        /// </summary>
        /// <param name="fAllowOverNonSecure">
        /// fAllowPasswordOperationsOverNonSecureConnection flag of dsHeuristics attribute bit
        /// </param>
        /// <param name="adType">Indicate if DS or LDS is used</param>
        /// <returns>errorstatus</returns>
        public errorstatus ValidateAdminPasswordReset(bool fAllowOverNonSecure, ADTypes adType)
        {
            //if SASL bind Successful
            //or protected ports
            //or tls on regular ports
            //or
            // In Windows Server 2008 and later, if the 
            // fAllowPasswordOperationsOverNonSecureConnection flag of the 
            // dSHeuristics attribute (section 7.1.1.2.4.1.2) equals true and 
            // Active Directory is operating as AD LDS, then the DC permits 
            // modification of the unicodePwd attribute over a connection that is 
            // neither SSL/TLS-encrypted nor SASL-encrypted   
            // and
            //the attriubuyte ForceChangepassword
            if (((isSASLBindSuccessful) ||
                  ((enumPortNum == Port.LDAP_SSL_GC_PORT) || (enumPortNum == Port.LDAP_SSL_PORT)) ||
                  (((enumPortNum == Port.LDAP_GC_PORT) || (enumPortNum == Port.LDAP_PORT)) && isTlsConnection == true) ||
                   ((enumPortNum == Port.LDAP_GC_PORT) || (enumPortNum == Port.LDAP_PORT)) && fAllowOverNonSecure && (PDCOSVersion == ServerVersion.Win2008 || PDCOSVersion == ServerVersion.Win2008R2) && (ADTypes.AD_LDS == adType)) &&
                    userForcePwd)
            {
                try
                {

                    bool isPwdSet = false;

                    //Set the Control Access Right to /force Change the password
                    bool isControlAccessRightSet = ADTSHelper.SetControlAcessRights(PdcFqdn, "CN=" + AdminChgPwdUser + ",CN=Users," + parsedDN,
                                                     AdminChgPwdUser,
                                                     AdminChgPwdUserPassword,
                                                     adDomain,
                                                     guidUser_Force_Change_Password,
                                                     ActiveDirectoryRights.ExtendedRight,
                                                     AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightSet, "Allow the extendedRight access right.");
                    //Force Change the password with the given password
                    //after successfully getting the required Control Access right
                    //Expected Result True
                    isPwdSet = ADTSHelper.SetPassword(connection,
                                                      "CN=" + AdminChgPwdUser + ",CN=Users," + parsedDN,
                                                      AdminChgPwdUserPassword);

                    bool isInetPwdSet = ADTSHelper.SetPassword(connection,
                                                      "CN=" + PwdChangedInetUsername + ",CN=Users," + parsedDN,
                                                      AdminChgPwdUserPassword);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isInetPwdSet, "set password is success");

                    //Validate MS-ADTS-Security_R496
                    //If ctrlAccessRights bit is set to User_Force_Change_Password, then userForcePwd is set to true.
                    //If Setpassword invoked successfully isPwdSet = true, the req is validated.
                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                        isPwdSet,
                        496,
                        @"For an originating update In AD LDS, if a password value is being modified as a password reset operation, 
                        then the requester needs to have the User-Force-Change-Password  control access right on the object being modified");

                    //Return Success ,since password is Set Successfully
                    return errorstatus.success;
                }
                catch (LdapException ldapError)
                {
                    //return failure, as an exception is occured
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ldapError.Message);
                    return errorstatus.failure;
                }

            }
            else
            {
                try
                {
                    //To Force Change the user passowrd,ForceChangePassword Control Access Right is Required 
                    //Change the Settings to deny the control access to change the password.
                    bool isResult = ADTSHelper.SetControlAcessRights(PdcFqdn,
                                                      "CN=" + AdminChgPwdUser + ",CN=Users," + parsedDN,
                                                      ClientUserName,
                                                      ClientUserPassword,
                                                      adDomain,
                                                      guidUser_Force_Change_Password,
                                                      ActiveDirectoryRights.WriteProperty,
                                                      AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isResult, "Deny the WriteProperty control access right for current user.");
                    //to force change the specified users password.
                    //SetPassword  should fail as there are no ControlAccessRight
                    bool isPwdNotSet = ADTSHelper.ForchangePassword(connection,
                                                             "CN=" + AdminChgPwdUser + ",CN=Users," + parsedDN,
                                                              InvalidPassword);

                    bool isInetPwdNotSet = ADTSHelper.ForchangePassword(connection,
                                                             "CN=" + PwdChangedInetUsername + ",CN=Users," + parsedDN,
                                                              InvalidPassword);
                    TestClassBase.BaseTestSite.Assert.AreEqual<bool>(isPwdNotSet, isInetPwdNotSet, "the setPassword operation is failed by no access right");
                    #region Rollback

                    //Rollback to previous state
                    //Remove the Deny Force change password Right
                    bool isControlAccessRightRemoved = ADTSHelper.RemoveControlAcessRights(PdcFqdn, 
                                                      "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                      ClientUserName,
                                                      ClientUserPassword,
                                                      adDomain,
                                                      guidUser_Force_Change_Password,
                                                      ActiveDirectoryRights.ExtendedRight,
                                                      AccessControlType.Deny);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isControlAccessRightRemoved, "Deny the ExtendedRight control access right for current user.");
                    //set the control access right
                    bool isResultSucess = ADTSHelper.SetControlAcessRights(PdcFqdn, 
                                                      "CN=" + ClientUserName + ",CN=Users," + parsedDN,
                                                      ClientUserName,
                                                      ClientUserPassword,
                                                      adDomain,
                                                      guidUser_Force_Change_Password,
                                                      ActiveDirectoryRights.ExtendedRight,
                                                      AccessControlType.Allow);
                    TestClassBase.BaseTestSite.Assert.IsTrue(isResultSucess, "Allow the ExtendedRight access right.");
                    #endregion

                    return errorstatus.failure;
                }
                catch (LdapException e)
                {
                    //return failure 
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                    return errorstatus.failure;
                }
            }
        }

        #endregion

        #endregion
        #endregion
    }
}
