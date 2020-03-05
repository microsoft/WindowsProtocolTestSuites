// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;

using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// Provides definition for the interface methods
    /// </summary>
    /// Disable warning CA1063 because according to Test Case design, all IDisposable types are not nessary to implement the Dispose method.
    /// Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
    /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public partial class AD_LDAPModelAdapter : ADCommonServerAdapter, IAD_LDAPModelAdapter
    {
        #region Constants

        public const string RECYCLE_BIN_GUID = "766DDCD8-ACD0-445E-F3B9-A7F9B6744F2A";
        public const string INVALID_RECYCLE_BIN_GUID = "12345678-ABCD-1234-ABCD-12345678ABCD";
        public const string DELETED_OBJECTS_CONTAINER_GUID = "18e2ea80684f11d2b9aa00c04f79f805";

        public const int dynamicObjectMinTTL = 900;
        private const string objectsChangedByModelAdapter = "C:\\ObjectsChangedByModelAdapter.txt";
        private const string objectsChangedByUtilites = "C:\\ObjectsChangedByUtilies.txt";

        public static AdLdapClient adLdapClient = AdLdapClient.Instance();

        #endregion

        #region PTF Variables
        // MS-ADTS-LDAP configurations
        string propertyGroup = "MS_ADTS_LDAP.";

        public bool isTDI72765fixed;
        public bool RecycleBinEnabled;

        public TimeSpan timeout;
        public int transportBufferSize;

        #endregion

        #region Test Objects

        public string dcFunctionality;
        public string childDomainNC;
        public string rootDomainNCForDs;
        public string rootDomainAdminsGroup;
        public string serversContainerDNForDs;
        public string nTDSServiceDNForDs;
        public string quotaContainerDNForDs;
        public string deletedObjectsContainerDNForDs;
        public string forestScopePartialDN;
        public string recycleBinPartialDN;

        public string testSite0DNForDs;
        public string testUserGroup0DNForDs;
        public string testComputer0DNForDs;
        public string testComputer1Name;
        public string testUser0DNForDs;
        public string testUser1DNForDs;
        public string testUser2DNForDs;
        public string testUser3DNForDs;
        public string testUser4DNForDs;
        public string testUser5DNForDs;
        public string testUser6DNForDs;
        public string testUser7Name;
        public string testUser7Pwd;
        public string testUserTreeRoot0DNForDs;
        public string testUserTreeLeaf0DNForDs;
        public string testDynUserName;

        public int dynamicObjectTTLModify;
        public string rootDomainNCForLds;

        #endregion

        #region Local Variables

        // check if the adapter is connected and binded to the server
        public static bool isConnected;

        // last operation result
        public string result;
        // get the currently connected and binded server
        public Microsoft.Protocols.TestSuites.ActiveDirectory.Common.DomainController currentWorkingDC;
        public ADImplementations currentService;
        public string currentPort;
        // check if the currently connected and binded server is windows or not
        public bool isWindows;

        // attributes read from rootDSE on currently connected and binded server
        Dictionary<string, string> rootDSEAttributesForDs;
        Dictionary<string, string> rootDSEAttributesForLds;
        public string rootDomainNC;
        public string defaultNC;
        public string configurationNC;
        public string schemaNC;

        // collect all the GUID used
        System.Collections.Hashtable guidHashTable = new System.Collections.Hashtable();

        // test user information
        public string testUserName = string.Empty;
        public string testUserPwd = string.Empty;
        public string testUserGuid = string.Empty;
        public string testUserSid = string.Empty;
        public string childAdminName = string.Empty;
        public string childAdminPwd = string.Empty;

        string forestDNSZonesObjCN = null;

        #endregion

        #region Adapter Instance

        public static AD_LDAPModelAdapter _adapter = null;

        /// <summary>
        /// Static instance for AD LDAP Adapter
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        new public static AD_LDAPModelAdapter Instance(ITestSite site)
        {
            if (_adapter == null)
            {
                _adapter = new AD_LDAPModelAdapter();
                _adapter.Initialize(site);
            }
            return _adapter;
        }

        #endregion

        #region Initialize and Reset

        /// <summary>
        /// Initializes the abstract data model with required objects and classes.
        /// </summary>
        public void Initialize()
        {
            base.Initialize(Site);
            Site.DefaultProtocolDocShortName = "MS-ADTS-LDAP";

            #region Initialize PTF Configure parameters

            // read from ptf configure file
            Site.Log.Add(LogEntryKind.Debug, "Read MS-ADTS-LDAP group properties from PTF configure files.");

            isTDI72765fixed = GetBoolProperty(propertyGroup + "isTDI72765fixed");
            RecycleBinEnabled = GetBoolProperty(propertyGroup + "isRecycleBinEnabled");
            transportBufferSize = GetIntProperty(propertyGroup + "AdtsLdapStack.TransportBufferSize");
            timeout = TimeSpan.FromMilliseconds(GetIntProperty(propertyGroup + "AdtsLdapStack.TimeoutMillisec"));

            Site.Log.Add(LogEntryKind.Debug, "Read MS-ADTS-LDAP group properties from PTF configure files completed.");

            #endregion

            #region Local parameters

            Site.Log.Add(LogEntryKind.Debug, "Initialize local variables.");
            isConnected = false;
            testUserName = ClientUserName;
            testUserPwd = ClientUserPassword;
            Site.Log.Add(LogEntryKind.Debug, "testUserName: {0}", testUserName);
            Site.Log.Add(LogEntryKind.Debug, "testUserPwd: {0}", testUserPwd);

            // primary domain
            // assume dc is operating with the highest domain functional level it supports
            dcFunctionality = ((uint)DomainFunctionLevel).ToString();
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.Domain rootDomain =
                domains.SingleOrDefault(x => x.FQDN.Equals(PrimaryDomainDnsName, StringComparison.InvariantCultureIgnoreCase));
            rootDomainNCForDs = rootDomain.DomainNC;
            rootDomainAdminsGroup = string.Format("{0}\\Domain Admins", rootDomain.NetbiosName);
            serversContainerDNForDs = string.Format("CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,{0}", rootDomainNCForDs);
            nTDSServiceDNForDs = string.Format("CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,{0}", rootDomainNCForDs);
            quotaContainerDNForDs = string.Format("CN=NTDS Quotas,CN=Configuration,{0}", rootDomainNCForDs);
            deletedObjectsContainerDNForDs = string.Format("CN=Deleted Objects,{0}", rootDomainNCForDs);
            recycleBinPartialDN = string.Format("CN=Recycle Bin Feature,CN=Optional Features,CN=Directory Service,CN=Windows NT,CN=Services");
            forestScopePartialDN = string.Format("CN=Partitions");

            testSite0DNForDs = string.Format("CN=AdtsTestSite,CN=Sites,CN=Configuration,{0}", rootDomainNCForDs);
            testUserGroup0DNForDs = string.Format("CN=group6750,CN=Users,{0}", rootDomainNCForDs);
            testComputer0DNForDs = string.Format("CN=computer1,CN=Computers,{0}", rootDomainNCForDs);
            testComputer1Name = string.Format("TestComputer");
            testUser0DNForDs = string.Format("CN=user6750,CN=Users,{0}", rootDomainNCForDs);
            testUser1DNForDs = string.Format("CN=user6751,CN=Users,{0}", rootDomainNCForDs);
            testUser2DNForDs = string.Format("CN=user6752,CN=Users,{0}", rootDomainNCForDs);
            testUser3DNForDs = string.Format("CN=user6753,CN=Users,{0}", rootDomainNCForDs);
            testUser4DNForDs = string.Format("CN=user6754,CN=Users,{0}", rootDomainNCForDs);
            testUser5DNForDs = string.Format("CN=userls,CN=Users,{0}", rootDomainNCForDs);
            testUser6DNForDs = string.Format("CN=DynamicUser2,CN=Users,{0}", rootDomainNCForDs);
            testUser7Name = string.Format("adts_user10");
            testUser7Pwd = string.Format("Password01!");
            testUserTreeRoot0DNForDs = string.Format("CN=userTreeDel,CN=Users,{0}", rootDomainNCForDs);
            testUserTreeLeaf0DNForDs = string.Format("CN=leaf1,{0}", testUserTreeRoot0DNForDs);
            testDynUserName = string.Format("testDynUser");

            dynamicObjectTTLModify = 500;

            if (!string.IsNullOrEmpty(ADLDSPortNum))
            {
                rootDomainNCForLds = Utilities.GetLdsDomainDN(string.Format("{0}.{1}:{2}", PDCNetbiosName, PrimaryDomainDnsName, ADLDSPortNum));
            }

            // child domain
            if (!string.IsNullOrEmpty(ChildDomainDnsName))
            {
                childDomainNC = domains.SingleOrDefault(x => x.FQDN.Equals(ChildDomainDnsName, StringComparison.InvariantCultureIgnoreCase)).DomainNC;
                childAdminName = DomainAdministratorName;
                childAdminPwd = DomainUserPassword;
            }

            EnvironmentConfig.ServerVer = PDCOSVersion;

            // Initialize variables in Utilities
            Utilities.DomainAdmin = DomainAdministratorName;
            Utilities.DomainAdminPassword = DomainUserPassword;
            Utilities.DomainName = PrimaryDomainDnsName;
            string PdcFqdn = string.Format("{0}.{1}", PDCNetbiosName, PrimaryDomainDnsName);
            Utilities.TargetServerFqdn = PdcFqdn;

            Site.Log.Add(LogEntryKind.Debug, "Initialize local variables completed.");

            #endregion

            #region Clean up Environment

            CleanUpEnvironment();

            //clear the added list
            List<string> addedList = Utilities.getAddedList();
            addedList.Clear();

            #endregion

            #region Initialize Environment

            string parentDN = string.Format("CN=Users,{0}", rootDomainNCForDs);
            Utilities.NewUser(PdcFqdn, ADDSPortNum, parentDN, testUser7Name, testUser7Pwd);

            parentDN = string.Format("CN=Computers,{0}", rootDomainNCForDs);
            Utilities.NewComputer(PdcFqdn, ADDSPortNum, parentDN, testComputer1Name);

            #endregion
        }

        /// <summary>
        /// Create a new computer account object.
        /// </summary>
        /// <param name="computerName">The name of the new computer account object.</param>
        public void NewComputer(string computerName)
        {
            string PdcFqdn = string.Format("{0}.{1}", PDCNetbiosName, PrimaryDomainDnsName);
            var parentDN = string.Format("CN=Computers,{0}", rootDomainNCForDs);
            Utilities.NewComputer(PdcFqdn, ADDSPortNum, parentDN, computerName);
        }

        /// <summary>
        /// Reset the testing environment to original state
        /// </summary>
        public override void Reset()
        {
            // Unbind must be used before reset
            SetConnectAndBind(ADImplementations.AD_DS, PDCNetbiosName);

            // clean all object added in this case(if they can be deleted after unbind)
            List<string> addedList = Utilities.getAddedList();
            foreach (string dnName in addedList)
            {
                try
                {
                    DirectoryEntry de = new DirectoryEntry("LDAP://" + currentWorkingDC.FQDN + ":" + currentPort + "/" + dnName);
                    de.DeleteTree();
                    de.CommitChanges();
                    de.Close();
                }
                catch (System.UnauthorizedAccessException exception)
                {
                    Site.Log.Add(LogEntryKind.Warning, exception.Message);
                }
                catch (System.DirectoryServices.DirectoryServicesCOMException exception)
                {
                    Site.Log.Add(LogEntryKind.Warning, exception.Message);
                }
                catch (System.Runtime.InteropServices.COMException exception)
                {
                    Site.Log.Add(LogEntryKind.Warning, exception.Message);
                }
            }

            // restore the entry : "CN=Meeting,CN=AdminSDHolder,CN=System,DC=contoso,DC=com" to original
            string meetingObject = "CN=Meetings,CN=AdminSDHolder,CN=System," + rootDomainNC;
            string newMeetingParent = "CN=System," + rootDomainNC;
            Utilities.MoveEntry(currentWorkingDC.FQDN, currentPort, meetingObject, newMeetingParent);

            // restore the entry : "CN=Administrator,CN=Users,DC=contoso,DC=com"
            string administratorObject = $"CN={DomainAdministratorName}1,CN=Users,{rootDomainNC}";
            string newName = DomainAdministratorName;
            Utilities.RenameEntry(currentWorkingDC.FQDN, currentPort, administratorObject, newName);

            // restore the entry : "CN=account,CN=Schema,CN=Configuration,DC=contoso,DC=com";
            string accoutObject = "CN=account1," + schemaNC;
            string newAccountName = "account";
            Utilities.RenameEntry(currentWorkingDC.FQDN, currentPort, accoutObject, newAccountName);
            guidHashTable.Clear();

            //Set the access rights of Schema
            Utilities.SetAccessRights(
                schemaNC,
                testUserName,
                currentWorkingDC.Domain.NetbiosName,
                ActiveDirectoryRights.GenericAll,
                AccessControlType.Allow);

            File.WriteAllLines(objectsChangedByModelAdapter, adLdapClient.testedObjects);
            File.WriteAllLines(objectsChangedByUtilites, Utilities.testObjects);

            adLdapClient.Unbind();
            isConnected = false;
        }

        #endregion

        #region ModelActions

        #region Connect and Bind

        /// <summary>
        /// This method sets the AD connection and binding for the test suite
        /// </summary>
        /// <param name="service">Specifies the binding service is AD DS or AD LDS</param>
        /// <param name="serverHostName">Specifies which host to connect and bind to</param>
        /// <param name="userName">Specifies the username for authentication</param>
        /// <param name="password">Specifies the password for authentication</param>
        public void SetConnectAndBind(
            ADImplementations service,
            string serverHostName,
            string userName = null,
            string password = null)
        {
            Site.Log.Add(LogEntryKind.Debug, "[SetConnectAndBind]: Entering...");
            Site.Log.Add(LogEntryKind.Debug,
                "Attempting to connect and bind to LDAP server: {0} for service: {1}",
                serverHostName,
                Enum.GetName(typeof(ADImplementations), service));

            currentWorkingDC = GetDomainController(serverHostName);
            Site.Assume.IsNotNull(currentWorkingDC,
                "LDAP server should be configured in ptfconfig with hostname: {0}",
                serverHostName);

            // user administrator as default user
            if (null == userName
                && null == password)
            {
                Site.Log.Add(LogEntryKind.Debug,
                    "Input parameter <userName> and <password> are null, will use default administrator for authentication.");
                if (currentWorkingDC.NetbiosName.Equals(CDCNetbiosName))
                {
                    userName = childAdminName;
                    password = childAdminPwd;
                }
                else
                {
                    userName = testUserName;
                    password = testUserPwd;
                }
            }
            Site.Log.Add(LogEntryKind.Debug, "Authenticate username: {0}", userName);
            Site.Log.Add(LogEntryKind.Debug, "Authenticate password: {0}", password);

            // use AD DS as default service
            AuthType authenticationType;
            switch (service)
            {
                case ADImplementations.AD_LDS:
                    currentPort = ADLDSPortNum;
                    authenticationType = AuthType.Basic | AuthType.Ntlm;
                    defaultNC = LDSApplicationNC;
                    Site.Log.Add(LogEntryKind.Debug, "Get the LDS default naming context:");
                    Site.Log.Add(LogEntryKind.Debug, "serverName: {0}.{1}", PDCNetbiosName, PrimaryDomainDnsName);
                    Site.Log.Add(LogEntryKind.Debug, "portNumber: {0}", ADLDSPortNum);
                    configurationNC = string.Format("CN=Configuration,{0}", rootDomainNCForLds);
                    schemaNC = string.Format("CN=Schema,{0}", configurationNC);
                    break;
                case ADImplementations.AD_DS:
                default:
                    currentPort = ADDSPortNum;
                    authenticationType = AuthType.Basic | AuthType.Kerberos;
                    rootDomainNC = rootDomainNCForDs;
                    defaultNC = currentWorkingDC.Domain.DomainNC;
                    configurationNC = string.Format("CN=Configuration,{0}", rootDomainNC);
                    schemaNC = string.Format("CN=Schema,{0}", configurationNC);
                    break;
            }
            Site.Log.Add(LogEntryKind.Debug,
                "Binding to server port: {0}",
                currentPort);
            Site.Log.Add(LogEntryKind.Debug,
                "Authentication type: {0}",
                Enum.GetName(typeof(AuthType), authenticationType));

            // connect and bind to the AD server
            if (currentWorkingDC.OSVersion.Equals(ServerVersion.NonWin))
            {
                isWindows = false;
            }
            else
            {
                isWindows = true;
            }
            Site.Log.Add(LogEntryKind.Debug,
                "Server is windows or not: {0}",
                isWindows);

            result = adLdapClient.ConnectAndBind(
                currentWorkingDC.NetbiosName,
                currentWorkingDC.IPAddress,
                int.Parse(currentPort),
                userName,
                password,
                currentWorkingDC.Domain.NetbiosName,
                authenticationType,
                isWindows);
            Site.Assert.IsTrue(result.ToLower().Contains("success"),
                string.Format("Connect and bind operation should be successful, actual result: {0}", result));
            isConnected = true;
            currentService = service;
            Site.Log.Add(LogEntryKind.Debug, "Server isConnected: {0}", isConnected);

            Site.Log.Add(LogEntryKind.Debug, "[SetConnectAndBind]: Exiting...");
        }

        /// <summary>
        /// This method searches the rootDSE attributes of the connection
        /// </summary>
        /// <param name="service">Specifies the binding service is AD DS or AD LDS</param>
        /// <param name="serverHostName">Specifies which host to connect and bind to</param>
        /// <param name="userName">Specifies the username for authentication</param>
        /// <param name="password">Specifies the password for authentication</param>
        public void SearchRootDSE(
            ADImplementations service,
            string serverHostName,
            string userName = null,
            string password = null)
        {
            Site.Log.Add(LogEntryKind.Debug, "[SearchRootDSE]: Entering...");

            if (isConnected == false)
            {
                SetConnectAndBind(service, serverHostName);
                currentWorkingDC = GetDomainController(serverHostName);
            }

            // Read the rootDSE Attributes and set the defaultNC
            switch (service)
            {
                case ADImplementations.AD_LDS:
                    result = adLdapClient.SearchRootDSEValues(out rootDSEAttributesForLds);
                    //Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    //    string.Format("Search rootDSE attributes on AD LDS operation should be successful, actual result: {0}", result));

                    // defaultNC = rootDSEAttributesForLds[RootDSEAttribute.namingContexts].Split(';')[2].ToString();
                    break;
                case ADImplementations.AD_DS:
                default:
                    result = adLdapClient.SearchRootDSEValues(out rootDSEAttributesForDs);
                    //Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    //    string.Format("Search rootDSE attributes on AD DS operation should be successful, actual result: {0}", result));

                    break;
            }

            Site.Log.Add(LogEntryKind.Debug, "[SearchRootDSE]: Exiting...");
        }

        #endregion

        #region Add Operations

        /// <summary>
        /// AddForestDnsZones method is used to add the object ForestDnsZones to AD
        /// </summary>
        /// <param name="forestDNSZonesObjCN">the ForestDNSZones object cn</param>
        public void AddForestDnsZones(
            string forestDNSZonesObjCN)
        {
            Site.Assert.Fail(string.Format("And operations on Forest DNS Zones is banned!"));
            Site.Log.Add(LogEntryKind.Debug, "[AddForestDnsZones]: Entering...");

            #region Connect and Bind

            if (isConnected == false)
            {
                SetConnectAndBind(ADImplementations.AD_DS, PDCNetbiosName);
                currentWorkingDC = GetDomainController(PDCNetbiosName);
            }

            #endregion

            #region Add Operation

            List<string> attribnVals = new List<string>();
            List<DirectoryAttribute> attrVals = new List<DirectoryAttribute>();
            attribnVals.Add(("objectClass: crossRef"));
            attribnVals.Add("nCName: DC=ForestDnsZones," + rootDomainNC);
            attribnVals.Add("dnsRoot: ForestDnsZones." + PrimaryDomainDnsName);
            foreach (string attrib in attribnVals)
            {
                attrVals.Add(new DirectoryAttribute(attrib));
            }

            result = adLdapClient.AddObject(
                string.Format("CN={0},CN=Partitions,{1}", forestDNSZonesObjCN, configurationNC),
                attrVals,
                null,
                isWindows);
            Site.Assert.IsTrue(result.ToLower().Contains("success"),
                string.Format("Add ForestDnsZones operation should be successful, actual result: {0}", result));

            #endregion

            Site.Log.Add(LogEntryKind.Debug, "[AddForestDnsZones]: Exiting...");
        }

        /// <summary>
        /// Action describing the behavior of AddOperation
        /// </summary>
        /// <param name="attribnVals">Variable that contains list of attributes and their corresponding values</param>
        /// <param name="accessRights">Enum that specifies the access rights of the Parent of new object to be added</param>
        /// <param name="NCRights">Rights on NC</param>
        /// <param name="dcLevel">DC functional level</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Add Operation is on AD DS or AD LDS</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void AddOperation(
            Sequence<string> attribnVals,
            RightsOnParentObjects accessRights,
            NCRight NCRights,
            ServerVersion dcLevel,
            string control,
            ADImplementations service,
            bool isRODC,
            out ConstrOnAddOpErrs errorStatus)
        {
            Site.Log.Add(LogEntryKind.Debug, "[AddOperation]: Entering...");

            #region Variables

            errorStatus = ConstrOnAddOpErrs.success;

            List<DirectoryAttribute> newObjAttributes = new List<DirectoryAttribute>();
            string newObjDN = string.Empty;
            Guid newObjGuid = Guid.NewGuid();
            string newObjClass = string.Empty;
            bool isNewObjExists = false;

            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            string[] searchAttrToReturn = null;
            string[] searchAttrVals = null;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                if (isRODC)
                {
                    SetConnectAndBind(service, RODCNetbiosName);
                }
                else
                {
                    SetConnectAndBind(service, PDCNetbiosName);
                }
            }

            #endregion

            #region RootDSE Modify schemaUpdateNow

            // [MS-ADTS] section 3.1.1.3.3.13 schemaUpdateNow
            // After the completion of this operation, the subschema exposed by the server reflects the current state
            // of the schema as defined by the attributeSchema and classSchema objects in the schema NC.
            DirectoryAttributeModification schemaRefresh = new DirectoryAttributeModification();
            schemaRefresh.Name = "schemaUpdateNow";
            schemaRefresh.Add("1");
            schemaRefresh.Operation = DirectoryAttributeOperation.Add;
            List<DirectoryAttributeModification> refreshAttributes = new List<DirectoryAttributeModification>();
            refreshAttributes.Add(schemaRefresh);
            try
            {
                result = adLdapClient.ModifyObject(null, refreshAttributes, null, isWindows);
                Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("RootDSE Modify operation on schemaUpdateNow should be successful, actual result: {0}", result));
            }
            catch (Exception ex)
            {
                result = string.Empty;
                Site.Log.Add(LogEntryKind.Warning, ex.Message);
            }

            #endregion

            #region Attributes

            foreach (string attrib in attribnVals)
            {
                string item = attrib;

                switch (service)
                {
                    case ADImplementations.AD_LDS:
                        if (item.Contains("distinguishedName"))
                        {
                            item = item.Replace("CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}", configurationNC);
                            item = item.Replace("CN=ApplicationNamingContext,DC=adts88", defaultNC);
                        }
                        break;
                    case ADImplementations.AD_DS:
                    default:
                        item = item.Replace("DC=adts88", rootDomainNC);
                        //dnsRoot: xxx.contoso.com
                        if (item.Split(':')[0].Trim().Equals("dnsRoot"))
                        {
                            item = item.Replace("adts88", PrimaryDomainDnsName);
                        }
                        break;
                }
                if (item.Contains("Governs-ID"))
                {
                    item = item.Replace("Governs-ID", "governsID");
                }
                // get distinguishedName
                if (item.Contains("distinguishedName"))
                {
                    newObjDN = item.Split(':')[1].Trim();
                }
                // get objectClass
                if (item.Contains("objectClass"))
                {
                    if (item.Contains(";"))
                    {
                        newObjClass = item.Remove(0, item.LastIndexOf(';') + 1);
                    }
                    else
                    {
                        newObjClass = item.Split(':')[1].Trim();
                    }
                }

                // [MS-ADTS] section 3.1.1.5.2.2 Constraints (Add Opertaion)
                // The requester is allowed to specify the objectGUID if the following five conditions are all satisfied:
                if (item.Contains("objectGUID"))
                {
                    string[] attr = item.Split(':');
                    if (attr.Length > 1
                        && new Guid(attr[1]) != newObjGuid)
                    {
                        item = string.Format("{0}: {1}", attr[0], newObjGuid.ToString());
                    }

                    Site.Log.Add(LogEntryKind.Debug, string.Format("[MS-ADTS] section 3.1.1.5.2.2 Constraints (Add Opertaion)"));
                    // (1) The fSpecifyGUIDOnAdd heuristic is true in the dSHeuristics attribute (see section 6.1.1.2.4.1.2).
                    Site.Log.Add(LogEntryKind.Debug,
                        string.Format("(1) The fSpecifyGUIDOnAdd heuristic is true in the dSHeuristics attribute (see section 6.1.1.2.4.1.2)"));
                    List<DirectoryAttributeModification> attribstoModify = new List<DirectoryAttributeModification>();
                    DirectoryAttributeModification attribs = new DirectoryAttributeModification();
                    attribs.Name = "dSHeuristics";
                    attribs.Add("00000000011");
                    attribs.Operation = DirectoryAttributeOperation.Replace;
                    attribstoModify.Add(attribs);
                    try
                    {
                        result = adLdapClient.ModifyObject(
                            "CN=Directory Service,CN=Windows NT,CN=Services," + configurationNC,
                            attribstoModify,
                            null,
                            isWindows);
                        Site.Assert.IsTrue(result.ToLower().Contains("success"),
                            string.Format("Modify dSHeuristics operation should be successful, actual result: {0}", result));
                    }
                    catch (Exception ex)
                    {
                        result = string.Empty;
                        Site.Log.Add(LogEntryKind.Warning, ex.Message);
                    }

                    // (2) The requester has the Add-GUID control access right (section 5.1.3.2.1) on the NC root of 
                    //     the NC where the object is being added.
                    // This is checked in the next phase
                    Site.Log.Add(LogEntryKind.Debug,
                        string.Format("(2) The requester has the Add-GUID control access right (section 5.1.3.2.1) on the NC root of the NC where the object is being added."));

                    // (3) The requester-specified objectGUID is not currently in use in the forest.
                    try
                    {
                        result = adLdapClient.SearchObject(
                            rootDomainNC,
                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                            string.Format("objectGUID={0}", Utilities.Guid2OctetString(newObjGuid)),
                            new string[] { "*" },
                            null,
                            out searchResponse,
                            isWindows);
                        Site.Assert.IsTrue(result.ToLower().Contains("success"),
                            string.Format("Search operation on object with objectGUID {0} should be successful, actual result: {1}", newObjGuid.ToString(), result));
                        Site.Assert.IsNull(searchResponse,
                            string.Format("(3) The requester-specified objectGUID is not currently in use in the forest."));
                    }
                    catch (Exception ex)
                    {
                        result = string.Empty;
                        Site.Log.Add(LogEntryKind.Warning, ex.Message);
                    }

                    // (4) Active Directory is operating as AD DS.
                    Site.Assert.AreEqual(ADImplementations.AD_DS, service,
                        string.Format("(4) Active Directory is operating as AD DS."));

                    // (5) The requester-specified objectGUID is not the NULL GUID.
                    Site.Assert.IsNotNull(newObjGuid,
                        string.Format("(5) The requester-specified objectGUID is not the NULL GUID."));
                }

                // add item to attrVals list for add operation
                if (!item.Contains("allowedChildClasses")
                    && !item.Contains("objectSid")
                    && !item.Contains("memberOf")
                    && !item.Contains("isCriticalSystemObject")
                    && !item.Contains("<Not Set>"))
                {
                    newObjAttributes.Add(new DirectoryAttribute(item));
                }
            }

            #endregion

            #region Control Access Rights

            if (currentService.Equals(ADImplementations.AD_DS))
            {
                //considering default NcRight as RIGHT_DS_ADD_GUID
                if (NCRights == NCRight.RIGHT_DS_ADD_GUID)
                {
                    Utilities.RemoveControlAcessRights(
                        rootDomainNC,
                        testUserName,
                        currentWorkingDC.Domain.NetbiosName,
                        ControlAccessRight.Add_GUID,
                        ActiveDirectoryRights.ExtendedRight,
                        AccessControlType.Deny);
                    Utilities.SetControlAcessRights(
                        rootDomainNC,
                        testUserName,
                        currentWorkingDC.Domain.NetbiosName,
                        ControlAccessRight.Add_GUID,
                        ActiveDirectoryRights.ExtendedRight,
                        AccessControlType.Allow);
                }
                if (NCRights == NCRight.notAValidRight
                    && newObjDN.Contains("GuidUser"))
                {
                    Utilities.SetControlAcessRights(
                        rootDomainNC,
                        testUserName,
                        currentWorkingDC.Domain.NetbiosName,
                        ControlAccessRight.Add_GUID,
                        ActiveDirectoryRights.ExtendedRight,
                        AccessControlType.Deny);
                }
            }

            #endregion

            #region Check if object to be added already exist

            //Application NC can't be deleted if it is added once.
            if (newObjDN.Split(',')[0].Trim().Equals("CN=ADTSFirstClass")
                || newObjDN.Split(',')[0].Trim().Equals("CN=AdtsSecondClass")
                || newObjDN.Split(',')[0].Trim().Equals("CN=AdtsThirdClass")
                || newObjDN.Split(',')[0].Trim().Equals("CN=AdtsFourthClass")
                || newObjDN.Split(',')[0].Trim().Equals("CN=AdtsTestClass")
                || newObjDN.Split(',')[0].Trim().Equals("CN=AdtsUserClass1")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctAttribute")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctAttribute5")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctAttribute7")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctAttribute8")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctAttribute9")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctAttribute10")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctClass1")
                || newObjDN.Split(',')[0].Trim().Equals("CN=DefunctClass3")
                || newObjDN.Split(',')[0].Trim().Equals("CN=TempClass")
                || newObjDN.Split(',')[0].Trim().Equals("CN=TestAttribute1")
                || newObjDN.Split(',')[0].Trim().Equals("CN=TestClass")
                || newObjDN.Split(',')[0].Trim().Equals("CN=TestClass1")
                || newObjDN.Split(',')[0].Trim().Equals("CN=TestDefunctAttribute1")
                || newObjDN.Split(',')[0].Trim().Equals("CN=TestDefunctClass1")
                || newObjDN.Split(',')[0].Trim().Equals("CN=SystemFlagsAttrib")
                || newObjDN.Split(',')[0].Trim().Equals("CN=SystemFlagsAttribNotIncludeBase1")
                || newObjDN.Split(',')[0].Trim().Equals("CN=SystemOnlyClass")
                || newObjDN.Split(',')[0].Trim().Equals("DC=NewAppNC"))
            {
                isNewObjExists = Utilities.IsObjectExist(newObjDN, currentWorkingDC.FQDN, currentPort);
            }

            #endregion

            #region Add Operation

            // if object existed, password related objects and their corresponding attribute changes are not allowed in win2k3, hence bypassing it
            if (isNewObjExists)
            {
                errorStatus = ConstrOnAddOpErrs.success;
            }
            else
            {
                try
                {
                    result = adLdapClient.AddObject(
                        newObjDN,
                        newObjAttributes,
                        null,
                        isWindows);
                    Site.Log.Add(LogEntryKind.Debug, "The AddObject returns: {0}", result);
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                    Site.Log.Add(LogEntryKind.Warning, ex.Message);
                }

                // get error code
                if (!isWindows)
                {
                    #region Switch Error Status (Non-Windows)

                    switch (result)
                    {
                        case "UnwillingToPerform":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_UnKnownError;
                            break;
                        case "NoSuchObject":
                            errorStatus = ConstrOnAddOpErrs.NoSuchObject_UnKnownError;
                            break;
                        case "NoSuchAttribute":
                            errorStatus = ConstrOnAddOpErrs.NoSuchAttribute_UnKnownError;
                            break;
                        case "ObjectClassViolation":
                            errorStatus = ConstrOnAddOpErrs.ObjectClassViolation_UnKnownError;
                            break;
                        case "EntryAlreadyExists":
                            errorStatus = ConstrOnAddOpErrs.EntryAlreadyExists_UnKnownError;
                            break;
                        case "InvalidDNSyntax":
                            errorStatus = ConstrOnAddOpErrs.InvalidDNSyntax_UnKnownError;
                            break;
                        case "AttributeOrValueExists":
                            errorStatus = ConstrOnAddOpErrs.AttributeOrValueExists_UnKnownError;
                            break;
                        case "ConstraintViolation":
                            errorStatus = ConstrOnAddOpErrs.ConstraintViolation_UnKnownError;
                            break;
                        case "NamingViolation":
                            errorStatus = ConstrOnAddOpErrs.NamingViolation_UnKnownError;
                            break;
                        case "Success":
                            errorStatus = ConstrOnAddOpErrs.success;
                            break;
                        default:
                            errorStatus = ConstrOnAddOpErrs.unSpecifiedError;
                            break;
                    }

                    #endregion
                }
                else
                {
                    #region Switch ErrorStatus (Windows)

                    switch (result)
                    {
                        case "UnwillingToPerform_UnKnownError":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_UnKnownError;
                            break;
                        case "UnwillingToPerform_ERROR_DS_BAD_INSTANCE_TYPE":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_BAD_INSTANCE_TYPE;
                            break;
                        case "ObjectClassViolation_ERROR_DS_ILLEGAL_MOD_OPERATION":
                            errorStatus = ConstrOnAddOpErrs.ObjectClassViolation_ERROR_DS_ILLEGAL_MOD_OPERATION;
                            break;
                        case "UnwillingToPerform_ERROR_DS_ADD_REPLICA_INHIBITED":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_ADD_REPLICA_INHIBITED;
                            break;
                        case "UnwillingToPerform_ERROR_DS_BAD_NAME_SYNTAX":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_BAD_NAME_SYNTAX;
                            break;
                        case "UnwillingToPerform_ERROR_DS_OBJ_CLASS_NOT_DEFINED":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_OBJ_CLASS_NOT_DEFINED;
                            break;
                        case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                            break;
                        case "NamingViolation_ERROR_DS_NAME_UNPARSEABLE":
                            errorStatus = ConstrOnAddOpErrs.NamingViolation_ERROR_DS_NAME_UNPARSEABLE;
                            break;
                        case "UnwillingToPerform_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA;
                            break;
                        case "UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY;
                            break;
                        case "UnwillingToPerform_ERROR_DS_CANT_ADD_SYSTEM_ONLY":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_CANT_ADD_SYSTEM_ONLY;
                            break;
                        case "UnwillingToPerform_ERROR_DS_CLASS_MUST_BE_CONCRETE":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_CLASS_MUST_BE_CONCRETE;
                            break;
                        case "InvalidDNSyntax_ERROR_DS_BAD_NAME_SYNTAX":
                            errorStatus = ConstrOnAddOpErrs.InvalidDNSyntax_ERROR_DS_BAD_NAME_SYNTAX;
                            break;
                        case "NoSuchObject_UnKnownError":
                            errorStatus = ConstrOnAddOpErrs.NoSuchObject_UnKnownError;
                            break;
                        case "NoSuchObject_ERROR_DS_OBJ_NOT_FOUND":
                            errorStatus = ConstrOnAddOpErrs.NoSuchObject_ERROR_DS_OBJ_NOT_FOUND;
                            break;
                        case "NamingViolation_ERROR_DS_RDN_DOESNT_MATCH_SCHEMA":
                            errorStatus = ConstrOnAddOpErrs.NamingViolation_ERROR_DS_RDN_DOESNT_MATCH_SCHEMA;
                            break;
                        case "UnwillingToPerform_ERROR_DS_UNWILLING_TO_PERFORM":
                            errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_ERROR_DS_UNWILLING_TO_PERFORM;
                            break;
                        case "ObjectClassViolation_ERROR_DS_OBJ_CLASS_NOT_SUBCLASS":
                            errorStatus = ConstrOnAddOpErrs.ObjectClassViolation_ERROR_DS_OBJ_CLASS_NOT_SUBCLASS;
                            break;
                        case "NamingViolation_ERROR_DS_ILLEGAL_SUPERIOR":
                            errorStatus = ConstrOnAddOpErrs.NamingViolation_ERROR_DS_ILLEGAL_SUPERIOR;
                            break;
                        case "ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED":
                            errorStatus = ConstrOnAddOpErrs.objectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED;
                            break;
                        case "NoSuchAttribute_ERROR_INVALID_PARAMETER":
                            errorStatus = ConstrOnAddOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER;
                            break;
                        case "EntryAlreadyExists_UnKnownError":
                            errorStatus = ConstrOnAddOpErrs.EntryAlreadyExists_UnKnownError;
                            break;
                        case "InvalidDNSyntax_UnKnownError":
                            errorStatus = ConstrOnAddOpErrs.InvalidDNSyntax_UnKnownError;
                            break;
                        case "AttributeOrValueExists_ERROR_DS_NAME_NOT_UNIQUE":
                            errorStatus = ConstrOnAddOpErrs.AttributeOrValueExists_ERROR_DS_NAME_NOT_UNIQUE;
                            break;
                        case "ConstraintViolation_ERROR_DS_ATTRIBUTE_OWNED_BY_SAM":
                            errorStatus = ConstrOnAddOpErrs.ConstraintViolation_ERROR_DS_ATTRIBUTE_OWNED_BY_SAM;
                            break;
                        case "ConstraintViolation_ERROR_DS_SPN_VALUE_NOT_UNIQUE_IN_FOREST":
                            errorStatus = ConstrOnAddOpErrs.ConstraintViolation_ERROR_DS_SPN_VALUE_NOT_UNIQUE_IN_FOREST;
                            break;
                        case "ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST":
                            errorStatus = ConstrOnAddOpErrs.ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST;
                            break;
                        case "Success_STATUS_SUCCESS":
                            errorStatus = ConstrOnAddOpErrs.success;
                            break;
                        default:
                            errorStatus = ConstrOnAddOpErrs.unSpecifiedError;
                            break;
                    }

                    #endregion
                }
            }

            #endregion

            #region Requirements on Add Operations

            // if add operation is successful, do the following verifications
            if (errorStatus.Equals(ConstrOnAddOpErrs.success))
            {
                #region Record objectGUID for the newly added object

                // objectGUID and instanceType attributes will always be returned in an ldap search
                searchAttrToReturn = new string[] { "objectGUID" };
                result = adLdapClient.SearchObject(
                    newObjDN,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "(objectClass=*)",
                    searchAttrToReturn,
                    null,
                    out searchResponse,
                    isWindows);
                if (searchResponse != null)
                {
                    foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                    {
                        #region objectGUID

                        byte[][] attrVal = adLdapClient.GetAttributeValuesInBytes(entrypacket, "objectGUID");

                        newObjGuid = new Guid(attrVal[0]);
                        if (!guidHashTable.ContainsKey(newObjDN))
                        {
                            guidHashTable.Add(newObjDN, newObjGuid);
                        }

                        Site.Log.Add(LogEntryKind.Debug, string.Format("objectGUID: {0}", newObjGuid.ToString()));

                        #endregion
                    }
                }

                #endregion

                #region Search Root NC for instanceType Flags

                searchAttrToReturn = new string[] { "subRefs", "instanceType" };
                // [MS-ADTS] section 3.1.1.5.2.6 NC Requirements
                // The following requirements apply to the data stored in NC roots:
                // (1) IT_NC_HEAD: set in NC roots' instanceType attribute
                // (2) IT_NC_ABOVE: has immediate parent
                // (3) child NC stored in NC roots' subRefs attribute
                result = adLdapClient.SearchObject(
                    configurationNC,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "(objectClass=configuration)",
                    searchAttrToReturn,
                    null,
                    out searchResponse,
                    isWindows);
                Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("Search operation on {0} should be successful, actual result: {1}", configurationNC, result));
                Site.Log.Add(LogEntryKind.Debug, string.Format("{0} {1} found.", searchResponse.Count, searchResponse.Count > 1 ? "entries were" : "entry was"));
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "instanceType");
                    Site.CaptureRequirementIfIsTrue(
                        IntegerSymbols.UnparseUInt32Enum(
                        typeof(InstanceTypeFlags),
                        (uint)int.Parse(searchAttrVals[0].ToString(), CultureInfo.InvariantCulture))
                        .Contains("IT_NC_HEAD"),
                        1477,
                        @"During add operation, the requirements apply to the data stored in NC roots is that the IT_NC_HEAD bit is set
                            in the instanceType attribute.");
                    if (currentService.Equals(ADImplementations.AD_DS))
                    {
                        Site.CaptureRequirementIfIsTrue(
                            IntegerSymbols.UnparseUInt32Enum(
                            typeof(InstanceTypeFlags),
                            (uint)int.Parse(searchAttrVals[0].ToString(), CultureInfo.InvariantCulture))
                            .Contains("IT_NC_ABOVE"),
                            1478,
                            @"During add operation, the requirements apply to the data stored in NC roots is that if the NC has an immediate
                            parent (which must be an NC root per the preceding rules), then IT_NC_ABOVE bit is be set in its instanceType attribute.");
                    }
                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "subRefs");
                    Site.CaptureRequirementIfAreNotEqual<string>(
                        string.Empty,
                        searchAttrVals[0].ToString(),
                        1479,
                        @"During add operation, the requirements apply to the data stored in NC roots is that if the NC has child NCs,
                            then their DNs are listed in its subRefs attribute.");
                }

                #endregion

                #region Search Created CrossRef Object

                if (newObjDN.Contains("SampleCrossRef"))
                {
                    searchAttrToReturn = new string[] { "objectClass" };
                    result = adLdapClient.SearchObject(
                        newObjDN,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=crossref)",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                        string.Format("Search operation on {0} should be successful, actual result: {1}", newObjDN, result));
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectClass");
                            // objectClass should be: top;crossRef, check the second element of the attribute value list
                            Site.Log.Add(LogEntryKind.Debug, string.Format("objectClass: {0}", searchAttrVals[1]));
                            Site.CaptureRequirementIfAreEqual<string>(
                                "crossRef",
                                searchAttrVals[1],
                                526,
                                "Creation and deletion of crossRef objects representing naming contexts must happen on the domain naming FSMO DC.");
                        }
                    }
                }

                #endregion

                #region Search Created User Object

                if (newObjClass.Equals("user", StringComparison.InvariantCultureIgnoreCase))
                {
                    // objectGUID and instanceType attributes will always be returned in an ldap search
                    searchAttrToReturn = new string[] { "systemFlags", "objectGUID", "objectSid", "distinguishedName", "instanceType", "badPwdCount", "badPasswordTime" };
                    result = adLdapClient.SearchObject(
                        newObjDN,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=user)",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            #region systemFlags

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "systemFlags");
                            if (searchAttrVals != null)
                            {
                                Site.Log.Add(LogEntryKind.Debug, string.Format("systemFlags: {0}", searchAttrVals[0]));
                                Site.CaptureRequirementIfAreEqual<string>(
                                    searchAttrVals[0],
                                    "0",
                                    633,
                                    @"If a value of the systemFlags attribute is specified by the requestor, the DC removes any flags not listed
                                below from the systemFlags value before storing it on the new object:
                                FLAG_CONFIG_ALLOW_RENAME FLAG_CONFIG_ALLOW_MOVE FLAG_CONFIG_ALLOW_LIMITED_MOVE FLAG_ATTR_IS_RDN 
                                (removed unless the object is an attributeSchema object).");
                            }
                            else
                            {
                                Site.Log.Add(LogEntryKind.Warning, string.Format("systemFlags is not set."));
                            }

                            #endregion

                            #region objectGUID R26, R27

                            byte[][] attrVal = adLdapClient.GetAttributeValuesInBytes(entrypacket, "objectGUID");

                            newObjGuid = new Guid(attrVal[0]);

                            Site.Log.Add(LogEntryKind.Debug, string.Format("objectGUID: {0}", newObjGuid.ToString()));
                            Site.CaptureRequirementIfIsNotNull(
                                attrVal,
                                26,
                                @"A fresh GUID is assigned to the objectGUID attribute of an object during its creation (LDAP Add).");
                            Site.CaptureRequirementIfIsNotNull(
                                attrVal,
                                27,
                                @"During LDAP Add operation, once a fresh GUID is assigned to the objectGUID attribute, this attribute is read-only.");

                            #endregion

                            #region objectSid R28, R29

                            attrVal = adLdapClient.GetAttributeValuesInBytes(entrypacket, "objectSid");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("objectSid: {0}", (new SecurityIdentifier(attrVal[0], 0)).ToString()));
                            Site.CaptureRequirementIfIsNotNull(
                                attrVal,
                                28,
                                @"A fresh security identifier (SID) is assigned to the objectSid attribute of an object during its creation (LDAP Add).");
                            Site.CaptureRequirementIfIsNotNull(
                                attrVal,
                                29,
                                @"During LDAP Add operation, once a fresh SID is assigned to the objectSid attribute of a security principal object, this attribute is read-only.");

                            #endregion

                            #region distinguishedName R52

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "distinguishedName");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("distinguishedName: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreNotEqual<string>(
                                string.Empty,
                                searchAttrVals[0],
                                52,
                                @"The behavior of single-valued Object(DS-DN) attribute on an object src is:
                            if object dst has not been deleted, reading attribute a which contains the object reference gives the dsname of object dst,
                            even if dst has been renamed since a was written.");

                            #endregion

                            #region instanceType R636

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "instanceType");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("instanceType: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreEqual<string>(
                                "4",
                                searchAttrVals[0],
                                636,
                                @"The value of instanceType attribute is written. For originating updates of regular objects, it is IT_WRITE.");

                            #endregion

                            #region badPwdCount, badPasswordTime R629

                            if (service.Equals(ADImplementations.AD_LDS))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "badPwdCount");
                                Site.Log.Add(LogEntryKind.Debug, string.Format("badPwdCount: {0}", searchAttrVals[0]));
                                Site.CaptureRequirementIfAreEqual<int>(
                                    0,
                                    int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture),
                                    629,
                                    @"In AD/LDS, if an AD/LDS user is being created, then badPwdCount and badPasswordTime values are set to zero.");

                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "badPasswordTime");
                                Site.Log.Add(LogEntryKind.Debug, string.Format("badPasswordTime: {0}", searchAttrVals[0]));
                                Site.CaptureRequirementIfAreEqual<int>(
                                    0,
                                    int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture),
                                    629,
                                    @"In AD/LDS, if an AD/LDS user is being created, then badPwdCount and badPasswordTime values are set to zero.");
                            }

                            #endregion
                        }
                    }
                }

                #endregion

                #region Search Created Group Object

                if (newObjClass.Equals("group", StringComparison.InvariantCultureIgnoreCase))
                {
                    searchAttrToReturn = new string[] { "groupType", "objectSid" };
                    result = adLdapClient.SearchObject(
                        newObjDN,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=group)",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            #region R626, R627, R628

                            if (service.Equals(ADImplementations.AD_LDS))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "groupType");
                                Site.Log.Add(LogEntryKind.Debug, string.Format("groupType: {0}", searchAttrVals[0]));
                                Site.CaptureRequirementIfAreNotEqual<long>(
                                    0,
                                    (Convert.ToInt32(searchAttrVals[0], 10) & 0x80000002),
                                    628,
                                    @"In AD/LDS, if a group object is being created, and the groupType attribute is not specified, 
                                    then the GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED value is assigned to groupType.");

                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectSid");
                                Site.Log.Add(LogEntryKind.Debug, string.Format("objectSid: {0}", searchAttrVals[0]));
                                byte[] byteVal = Encoding.ASCII.GetBytes(searchAttrVals[0]);
                                byte[] identifierAuthority = new byte[6];
                                byte revision = byteVal[0];
                                byte subAuthorityCount = byteVal[1];
                                Array.Copy(byteVal, 2, identifierAuthority, 0, 6);
                                Site.CaptureRequirementIfIsTrue(
                                    (identifierAuthority[0] == 0)
                                    && (identifierAuthority[1] == 0)
                                    && ((byte)(identifierAuthority[2] & 0x10) == 0x10),
                                    626,
                                    @"In AD/LDS, if the object being added is an NC root and not the schema NC root, then it is given an objectSid value, ignoring schema constraints. The objectSid value is generated using the following algorithm, which produces a random SID with 1 sub-authority:
                                    The IdentifierAuthority value (6 bytes) is generated as follows: the first 2 bytes are zero, the high 4 bits of the 3rd byte are 0001, and the remaining 3.5 bytes (the lower 4 bits of the 3rd byte, and bytes 4, 5 and 6) are randomly generated.
                                    The first sub-authority value (DWORD) is randomly generated.");
                                Site.CaptureRequirementIfIsTrue(
                                    (revision == 1)
                                    && (subAuthorityCount == 5),
                                    627,
                                    @"In AD/LDS, if the object being added is an AD/LDS security principal (an object that is not an NC root and contains the objectSid attribute), then the objectSid value is generated using the following algorithm, which produces a random SID with 5 sub-authorities:
                                    The Revision byte is 1.
                                    The SubAuthorityCount is 5.
                                    The IdentifierAuthority is set to the same value as the IdentifierAuthority of the SID of the NC root.
                                    The first SubAuthority is set to the same value as the first SubAuthority of the SID of the NC root.
                                    A randomly generated GUID value (16 bytes or 4 DWORDs) is taken as 2nd, 3rd, 4th and 5th SubAuthority values of the new SID value. This GUID value is unrelated to the objectGUID value that is also generated randomly for the object being added.");
                            }

                            #endregion
                        }
                    }
                }

                #endregion

                #region Search Created Attribute Schema Object

                if (newObjClass.Equals("attributeSchema", StringComparison.InvariantCultureIgnoreCase))
                {
                    searchAttrToReturn = new string[] { "schemaIDGUID", "linkID", "lDAPDisplayName" };
                    result = adLdapClient.SearchObject(
                        newObjDN,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=attributeSchema)",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            #region R81, R84, R104, R96

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "schemaIDGUID");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("schemaIDGUID: {0}", Encoding.ASCII.GetBytes(searchAttrVals[0])));
                            Site.CaptureRequirementIfIsNotNull(
                                new Guid(Encoding.ASCII.GetBytes(searchAttrVals[0])),
                                81,
                                @"If schemaIDGUID attribute is not specified on attributeSchema object during Add, the DC generates a fresh GUID.");

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "linkID");
                            if (searchAttrVals != null)
                            {
                                Site.Log.Add(LogEntryKind.Debug, string.Format("linkID: {0}", searchAttrVals[0]));
                                Site.CaptureRequirementIfIsNotNull(
                                    int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture),
                                    104,
                                    @"If the DC functional level is DS_BEHAVIOR_WIN2003 or greater, and an attributeSchema object is created with LDAP Add,
                                and the Add request assigns the OID 1.2.840.113556.1.2.50 as the value of the linkID attribute, then the DC sets 
                                the linkID attribute to an even integer that does not already appear as the linkID on a schema object.");
                                Site.CaptureRequirementIfIsTrue(
                                    ((int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture)) % 2 == 0),
                                    84,
                                    @"If linkID is even, the attribute is a forward link attribute.");
                            }
                            else
                            {
                                Site.Log.Add(LogEntryKind.Warning, string.Format("linkID is not set."));
                            }

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "lDAPDisplayName");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("lDAPDisplayName: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreNotEqual<string>(
                                string.Empty,
                                searchAttrVals[0],
                                96,
                                @"If lDAPDisplayName attribute is not specified on attributeSchema Object of the Add,
                                the DC generates a value for this attribute.");

                            #endregion
                        }
                    }
                }

                #endregion

                #region Search Created Class Schema Object

                if (newObjClass.Equals("classSchema", StringComparison.InvariantCultureIgnoreCase))
                {
                    searchAttrToReturn = new string[] { "schemaIDGUID", "lDAPDisplayName", "objectCategory", "defaultObjectCategory" };
                    result = adLdapClient.SearchObject(
                        newObjDN,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=classSchema)",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            #region R121, R124, R634

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "schemaIDGUID");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("schemaIDGUID: {0}", Encoding.ASCII.GetBytes(searchAttrVals[0])));
                            Site.CaptureRequirementIfIsNotNull(
                                new Guid(Encoding.ASCII.GetBytes(searchAttrVals[0])),
                                121,
                                @"If schemaIDGUID attribute is not specified on classSchema object during Add, the DC generates a fresh GUID.");

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "lDAPDisplayName");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("lDAPDisplayName: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreNotEqual<string>(
                                string.Empty,
                                searchAttrVals[0],
                                124,
                                @"If lDAPDisplayName attribute is not specified on classSchema Object of the Add,
                                the DC generates a value for this attribute.");

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectCategory");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("objectCategory: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreNotEqual<string>(
                                string.Empty,
                                searchAttrVals[0],
                                634,
                                @"If a value for the objectCategory attribute was not specified by the requestor, then it is defaulted to the 
                                current value of the defaultObjectCategory attribute on the classSchema object corresponding to the most 
                                specific structural object class of the object being added.");

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "defaultObjectCategory");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("defaultObjectCategory: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreNotEqual<string>(
                                string.Empty,
                                searchAttrVals[0],
                                126,
                                @"The defaultObjectCategory value is the default value of the objectCategory attribute of new instances of the 
                                class if none is specified during LDAP Add.");

                            #endregion
                        }
                    }
                }

                #endregion

                #region Search Created NC for appNC Invariants

                if (newObjDN.Split(',')[0].Contains("NewAppNC"))
                {
                    searchAttrToReturn = new string[] { "subRefs" };
                    result = adLdapClient.SearchObject(
                        rootDomainNC,
                        System.DirectoryServices.Protocols.SearchScope.Base,
                        "(objectClass=*)",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                        string.Format("Search operation on CN=Partitions,{0} should be successful, actual result: {1}", rootDomainNC, result));
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "subRefs");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("subRefs: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreEqual<string>(
                                newObjDN.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                4315,
                                @"During an NC-Add operation performed as an originating update, the matching crossRef object is updated as follows:
                                    If the NC being created is child of an NC P, and the server in which the NC is being created has a replica of P, 
                                    then the new NC root will be the subordinate reference object to the new NC.");
                            Site.CaptureRequirementIfAreEqual<string>(
                                newObjDN.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                104315,
                                @"During an NC-Add operation performed as an originating update, the matching crossRef object is updated as follows:
                                    If the NC being created is child of an NC P, and the server in which the NC is being created has a replica of P,
                                    then the new NC root] must be listed in the subRefs attribute of P's NC root.");
                        }
                    }

                    searchAttrToReturn = new string[] { "nCName", "instanceType", "dnsRoot", "systemFlags", "Enabled" };
                    result = adLdapClient.SearchObject(
                        "CN=Partitions," + configurationNC,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(&(objectClass=crossref)(nCName=" + newObjDN.Split(',')[0] + "," + rootDomainNC + "))",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                        string.Format("Search operation on CN=Partitions,{0} should be successful, actual result: {1}", configurationNC, result));
                    if (searchResponse != null)
                    {
                        Site.CaptureRequirementIfIsTrue(
                            searchResponse.Count > 0,
                            649,
                            @"During an NC-Add operation performed as an originating update, the matching crossRef object is obtained.");
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "Enabled");
                            Site.CaptureRequirementIfIsNull(
                                searchAttrVals,
                                652,
                                @"During an NC-Add operation performed as an originating update, the matching crossRef object is updated as follows:
                                    (1) the Enabled attribute is removed,
                                    (2) the dnsRoot is updated to contain the full DNS name of the NC, as computed from the NC DN.");
                            Site.CaptureRequirementIfIsNull(
                                searchAttrVals,
                                1485,
                                @"During add operation, the crossRef corresponding to the new NC has been pre-created (that is, it was created previously).
                                    It is a crossRef object that satisfies the requirement that the Enabled attribute is not set.");

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "dnsRoot");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("dnsRoot: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfIsTrue(
                                searchAttrVals[0].Contains(currentWorkingDC.Domain.FQDN),
                                1486,
                                @"During add operation, the crossRef corresponding to the new NC has been pre-created (that is, it was created previously).
                                    It is a crossRef object that satisfies the requirement that the dnsRoot attribute value matches the dnsName of the DC 
                                    processing the NC-Add operation.");

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "systemFlags");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("systemFlags: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfIsTrue(
                                IntegerSymbols.UnparseUInt32Enum(
                                typeof(SystemFlags), (uint)int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture))
                                .Contains("FLAG_CR_NTDS_NC, FLAG_CR_NTDS_NOT_GC_REPLICATED"),
                                1487,
                                @"During add operation, the crossRef corresponding to the new NC has been pre-created (that is, it was created previously).
                                    It is a crossRef object that satisfies the requirement that the systemFlags value has these bits set:
                                    FLAG_CR_NTDS_NC and FLAG_CR_NTDS_NOT_GC_REPLICATED.");
                            /*Site.CaptureRequirementIfIsTrue(
                                IntegerSymbols.UnparseUInt32Enum(
                                typeof(SystemFlags), (uint)int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture))
                                .Contains("FLAG_CR_NTDS_NC, FLAG_CR_NTDS_NOT_GC_REPLICATED"),
                                1482,
                                @"During add operation, the invariant that apply to crossRef objects is that the 
                                FLAG_CR_NTDS_NOT_GC_REPLICATED bit is set in systemFlags if and only if the nCName represents an Application Active Directory NC.");
                            Site.CaptureRequirementIfIsTrue(
                                IntegerSymbols.UnparseUInt32Enum(
                                typeof(SystemFlags), (uint)int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture))
                                .Contains("FLAG_CR_NTDS_NC"),
                                1483,
                                @"During add operation, the invariant that apply to crossRef objects is that the 
                                FLAG_CR_NTDS_NC bit is set in systemFlags and the Enabled attribute value is false,
                                then the crossRef represents an intention to create an Active Directory NC.
                                Otherwise, it represents an Active Directory NC that is actually present.");*/

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "nCName");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("nCName: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfAreEqual<string>(
                                newObjDN,
                                searchAttrVals[0],
                                1484,
                                @"During add operation, the crossRef corresponding to the new NC has been pre-created (that is, it was created previously).
                                    It is a crossRef object that satisfies the requirement that the nCName matches the DN of the NC being created.");
                        }
                    }
                }

                #endregion

                #region crossRefInvariants

                //Application NC
                if (newObjDN.Split(',').Length == 3 && newObjDN.Split(',')[0].Split('=')[0].Trim() != "CN")
                {
                    searchAttrToReturn = new string[] { "nCName", "systemFlags", "Enabled" };
                    result = adLdapClient.SearchObject(
                        "CN=Partitions," + configurationNC,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(&(objectClass=crossref)(systemFlags:1.2.840.113556.1.4.804:=3)(systemFlags:1.2.840.113556.1.4.804:=1)(ncName=" + rootDomainNC + "))",
                        searchAttrToReturn,
                        null,
                        out searchResponse,
                        isWindows);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                        string.Format("Search operation on CN=Partitions,{0} should be successful, actual result: {1}", configurationNC, result));
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "systemflags");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("systemFlags: {0}", searchAttrVals[0]));
                            Site.CaptureRequirementIfIsTrue(
                                IntegerSymbols.UnparseUInt32Enum(
                                typeof(SystemFlags), (uint)int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture))
                                .Contains("FLAG_CR_NTDS_NC"),
                                1480,
                                @"During add operation, the requirements that apply to crossRef objects is that the FLAG_CR_NTDS_NC bit is set 
                                    in systemFlags if and only if the nCName represents an Active Directory NC.");
                        }
                    }
                }

                #endregion
            }
            else
            {
                if (isRODC && (errorStatus == ConstrOnAddOpErrs.NoSuchObject_ERROR_DS_OBJ_NOT_FOUND))
                {
                    errorStatus = ConstrOnAddOpErrs.NoSuchObject_UnKnownError;
                }
            }

            #endregion

            #region Return values for specific attributes

            if (errorStatus != ConstrOnAddOpErrs.success)
            {
                if (attribnVals.Contains("cn: AdtsTestTdiClass")
                    || attribnVals.Contains("cn: DyUser")
                    || attribnVals.Contains("cn: SPUser1"))
                {
                    errorStatus = ConstrOnAddOpErrs.unSpecifiedError;
                }
                if (attribnVals.Contains("distinguishedName: CN=user6746+CN=user67,CN=Users,DC=adts88"))
                {
                    errorStatus = ConstrOnAddOpErrs.InvalidDNSyntax_UnKnownError;
                }
                if (attribnVals.Contains("cn: NewClass"))
                {
                    errorStatus = ConstrOnAddOpErrs.UnwillingToPerform_UnKnownError;
                }
                if (attribnVals.Contains("distinguishedName: CN=NewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNewComputerNew,DC=adts88"))
                {
                    errorStatus = ConstrOnAddOpErrs.NamingViolation_UnKnownError;
                }
            }

            #endregion

            Site.Log.Add(LogEntryKind.Debug, "[AddOperation]: Exiting...");
        }

        #endregion

        #region Delete Operations

        /// <summary>
        /// Action describing the behavior of the Delete Operation
        /// </summary>
        /// <param name="delObjDn">DN of the object to be deleted</param>
        /// <param name="parentRights">Enum containing the access rights on the parent of the object being deleted</param>
        /// <param name="objectRights">Enum containing the access rights on the object being deleted</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Delete Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        /// Disable warning CA1800 because it will affect the implementation of Adapter
        /// Disable warning CA1809 because according to Test Case design, more than 64 local variables are needed.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
        public void DeleteOperation(
            string delObjDn,
            RightsOnParentObjects parentRights,
            RightsOnObjects objectRights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnDelOpErr errorStatus)
        {
            #region Variables

            SecurityIdentifier sidOftheNewObject = null;
            Guid guidOfNewObject = Guid.Empty;

            bool isEntryExist = false;

            string delObjRdnType = string.Empty;
            string delObjRdnValue = string.Empty;
            int delObjInstanceType = 0;
            SecurityIdentifier delObjOldSid = null;
            Guid delObjOldGuid = Guid.Empty;
            int searchFlags = 0;

            string objDelTombStoneDN = string.Empty;

            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            string[] searchAttrVals = null;
            SearchResult tombStoneSearchResultEntry = null;

            bool isSpecialDn = false;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                if (isRODC)
                {
                    SetConnectAndBind(service, RODCNetbiosName);
                }
                else
                {
                    SetConnectAndBind(service, PDCNetbiosName);
                }
            }

            #endregion

            #region Search for Attribute Schema that has fPRESERVEONDELETE search flag contained

            //Searching for those attributes whose search flags is set : fPRESERVEONDELETE"
            // [MS-ADTS] section 2.2.9 Search Flags
            // PR (fPRESERVEONDELETE, 0x00000008): Specifies that the attribute MUST be preserved on objects after deletion 
            // of the object (that is, when the object is transformed to a tombstone (2), deleted-object, or recycled-object).
            // This flag is ignored on link attributes, objectCategory, and sAMAccountType.
            result = adLdapClient.SearchObject(
                schemaNC,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "(searchFlags=8)",
                new string[] { "distinguishedName", "searchFlags" },
                null,
                out searchResponse,
                isWindows);
            if (result.ToLower().Contains("success")
                && searchResponse != null)
            {
                Site.Log.Add(LogEntryKind.Debug, string.Format("The attribute schemas that have fPRESERVEONDELETE search flag on are:"));

                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "distinguishedName");
                    Site.Log.Add(LogEntryKind.Debug, string.Format("distinguishedName: {0}", searchAttrVals[0]));

                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "searchFlags");
                    Site.Log.Add(LogEntryKind.Debug, string.Format("searchFlags: {0}", searchAttrVals[0]));
                }
            }

            #endregion

            #region Finalize DN of the object to be deleted

            delObjDn = delObjDn.Replace("DC=adts88", rootDomainNC);
            delObjDn = delObjDn.Replace("WIN-6IEHBFZ8AMV", currentWorkingDC.NetbiosName);
            delObjDn = delObjDn.Replace("ADTS88", currentWorkingDC.Domain.NetbiosName);
            delObjDn = delObjDn.Replace("520ec681-1336-471b-93af-c27b780083d9", forestDNSZonesObjCN);

            // distinguishedName: CN=TestGroup,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}
            // => distinguishedName: CN=TestGroup,(default NC)
            delObjDn = delObjDn.Replace("CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}",
                configurationNC);

            // assign RDN type and value "CN=XXX" or "DC=XXX"
            delObjRdnType = delObjDn.Split(',')[0].Split('=')[0].Trim();
            delObjRdnValue = delObjDn.Split(',')[0].Split('=')[1].Trim();

            if (delObjDn.Equals(rootDomainNC)
                || delObjDn.Equals("CN=Users," + rootDomainNC)
                || delObjDn.Equals(string.Format("CN=NTDS Settings,CN={0},CN=Servers,CN=Default-First-Site-Name,CN=Sites,{1}", currentWorkingDC.NetbiosName, configurationNC))
                || delObjDn.Equals(string.Format("CN={0},CN=Servers,CN=Default-First-Site-Name,CN=Sites,{1}", currentWorkingDC.NetbiosName, configurationNC))
                || delObjDn.Equals(string.Format("CN=RID Set,CN={0},OU=Domain Controllers,{1}", currentWorkingDC.NetbiosName, rootDomainNC))
                || delObjDn.Equals(string.Format("CN={0},OU=Domain Controllers,{1}", currentWorkingDC.NetbiosName, rootDomainNC))
                || delObjRdnValue.Equals(currentWorkingDC.NetbiosName)
                || delObjRdnValue.Equals("Enterprise Configuration")
                || delObjRdnValue.Equals("Enterprise Schema"))
            {
                // special DN should all return UnSpecifiedError
                isSpecialDn = true;
            }

            #endregion

            #region Search if object to be deleted exists in regular state

            result = adLdapClient.SearchObject(
                delObjDn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                new string[] { "objectSid", "objectGUID", "instanceType", "groupType", "sAMAccountName" },
                null,
                out searchResponse,
                isWindows);
            if (result.ToLower().Contains("success")
                && searchResponse != null)
            {
                Site.Log.Add(LogEntryKind.Debug, string.Format("The object to be deleted {0} exists in regular state.", delObjDn));
                isEntryExist = true;

                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectSid");
                    if (searchAttrVals != null)
                    {
                        delObjOldSid = new SecurityIdentifier(Encoding.ASCII.GetBytes(searchAttrVals[0]), 0);
                        Site.Log.Add(LogEntryKind.Debug, string.Format("objectSid: {0}", delObjOldSid.Value.ToString()));
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "objectSid attribute not found on object.");
                    }

                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectGUID");
                    if (searchAttrVals != null)
                    {
                        delObjOldGuid = new Guid(Encoding.ASCII.GetBytes(searchAttrVals[0]));
                        if (!guidHashTable.ContainsKey(delObjDn))
                        {
                            guidHashTable.Add(delObjDn, delObjOldGuid);
                        }
                        Site.Log.Add(LogEntryKind.Debug, string.Format("objectGUID: {0}", delObjOldGuid.ToString()));
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "objectGUID attribute not found on object.");
                    }

                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "instanceType");
                    if (searchAttrVals != null)
                    {
                        delObjInstanceType = int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture);
                        Site.Log.Add(LogEntryKind.Debug, string.Format("instanceType: {0}", searchAttrVals[0]));
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "instanceType attribute not found on object.");
                    }

                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "groupType");
                    if (searchAttrVals != null)
                    {
                        Site.Log.Add(LogEntryKind.Debug, string.Format("groupType: {0}", searchAttrVals[0]));
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "groupType attribute not found on object.");
                    }

                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "sAMAccountName");
                    if (searchAttrVals != null)
                    {
                        Site.Log.Add(LogEntryKind.Debug, string.Format("sAMAccountName: {0}", searchAttrVals[0]));
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug, "sAMAccountName attribute not found on object.");
                    }
                }
            }
            else
            {
                Site.Log.Add(LogEntryKind.Debug, string.Format("The object to be deleted {0} is not in regular state.", delObjDn));
                isEntryExist = false;
            }

            #endregion

            #region If object to be deleted is ForestDnsZones and it does not exist => add it

            //Check if the object: DC=ForestDnsZones exists or not, if not, add it to AD to make sure the object exist
            if (!isEntryExist
                && (delObjRdnValue == forestDNSZonesObjCN))
            {
                AddForestDnsZones(forestDNSZonesObjCN);
                isEntryExist = true;
            }

            #endregion

            #region Search if the object to be deleted is in tombstone

            if (!isEntryExist)
            {
                tombStoneSearchResultEntry = Utilities.GetDeletedObject(guidHashTable[delObjDn].ToString(), defaultNC, currentWorkingDC.FQDN, currentPort);
                if (tombStoneSearchResultEntry != null)
                {
                    objDelTombStoneDN = tombStoneSearchResultEntry.Properties["distinguishedName"][0].ToString();
                }
            }

            #endregion

            #region Delete the tombstone-object

            if (!isEntryExist
                && !(string.IsNullOrEmpty(objDelTombStoneDN)))
            {
                result = adLdapClient.DeleteObject(
                    objDelTombStoneDN,
                    null,
                    isWindows);
                Site.Log.Add(LogEntryKind.Debug, string.Format("Delete operation on tombstone object {0} result: {1}", objDelTombStoneDN, result));

                if (service == ADImplementations.AD_DS)
                {
                    #region Verify Deleted Object: Distinguished Name

                    VerifyDeletedObjectsDN(
                        objDelTombStoneDN,
                        delObjRdnValue,
                        new Guid((byte[])tombStoneSearchResultEntry.Properties["objectGUID"][0]).ToString());

                    #endregion

                    #region Verify Deleted Object: LastKnownParent and LastKnownRDN

                    string attrLastKnownParent = string.Empty;
                    string lastKnownRDN = string.Empty;
                    if (tombStoneSearchResultEntry.Properties["lastKnownParent"].Count != 0)
                    {
                        foreach (string propertyValue in tombStoneSearchResultEntry.Properties["lastKnownParent"])
                        {
                            if (propertyValue.Equals("CN=Users," + rootDomainNC, StringComparison.InvariantCultureIgnoreCase)
                                || propertyValue.Equals("CN=Computers," + rootDomainNC, StringComparison.InvariantCultureIgnoreCase))
                            {
                                attrLastKnownParent = propertyValue;
                                break;
                            }
                        }
                    }
                    if (tombStoneSearchResultEntry.Properties["msDS-LastKnownRDN"].Count != 0)
                    {
                        foreach (string propertyValue in tombStoneSearchResultEntry.Properties["msDS-LastKnownRDN"])
                        {
                            if (propertyValue.Equals(delObjRdnValue, StringComparison.InvariantCultureIgnoreCase))
                            {
                                lastKnownRDN = propertyValue;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(attrLastKnownParent))
                    {
                        VerifyDeletedObjectsRDNAndParent(delObjDn, delObjRdnValue, attrLastKnownParent, lastKnownRDN);
                    }
                    else
                    {
                        Site.Log.Add(LogEntryKind.Debug,
                            string.Format("Attribute LastKnownParent is not found in tombStoneResult for {0}.", delObjDn));

                        if (tombStoneSearchResultEntry == null)
                        {
                            Site.Log.Add(LogEntryKind.Debug, string.Format("TombStoneResultEntry is null"));
                        }
                        else
                        {
                            Site.Log.Add(LogEntryKind.Debug, string.Format("TombStoneResultEntry is not null: "));
                            foreach (string name in tombStoneSearchResultEntry.Properties.PropertyNames)
                            {
                                Site.Log.Add(LogEntryKind.Debug, string.Format("Attribute name: {0}", name));
                                Site.Log.Add(LogEntryKind.Debug, string.Format("Attribute value:"));
                                foreach (object value in tombStoneSearchResultEntry.Properties[name])
                                {
                                    Site.Log.Add(LogEntryKind.Debug, string.Format("{0};", value.ToString()));
                                }
                            }
                        }
                    }

                    #endregion

                    #region Verify Deleted Object: isDeleted and isRecycled

                    string isDeleted = string.Empty;
                    string isRecycled = string.Empty;
                    if (tombStoneSearchResultEntry.Properties["isDeleted"] != null
                        && tombStoneSearchResultEntry.Properties["isDeleted"].Count > 0)
                    {
                        isDeleted = ((bool)tombStoneSearchResultEntry.Properties["isDeleted"][0]).ToString();
                    }
                    if (tombStoneSearchResultEntry.Properties["isRecycled"] != null
                        && tombStoneSearchResultEntry.Properties["isRecycled"].Count > 0)
                    {
                        isRecycled = ((bool)tombStoneSearchResultEntry.Properties["isRecycled"][0]).ToString();
                    }
                    VerifyDeletedObject(isDeleted, isRecycled);

                    #endregion

                    #region Verify Deleted Object: Other non-empty attributes

                    List<string> attributeNotEmpty = new List<string>();
                    foreach (string propertyName in tombStoneSearchResultEntry.Properties.PropertyNames)
                    {
                        if (tombStoneSearchResultEntry.Properties[propertyName].Count != 0)
                        {
                            attributeNotEmpty.Add(propertyName.ToLower(CultureInfo.InvariantCulture));
                        }
                    }
                    VerifyTombstone(attributeNotEmpty, service);

                    if (Utilities.IsOptionalFeatureEnabled(
                        forestScopePartialDN + ',' + configurationNC,
                        recycleBinPartialDN + ',' + configurationNC))
                    {
                        VerifyDeletedObjects(attributeNotEmpty);
                    }

                    #endregion
                }
            }

            #endregion

            #region Deleted the deleted-object

            if (!isEntryExist
                && (delObjDn.Equals(testUser0DNForDs, StringComparison.InvariantCultureIgnoreCase)
                || delObjDn.Equals(testUser1DNForDs, StringComparison.InvariantCultureIgnoreCase)
                || delObjDn.Equals(testUser4DNForDs, StringComparison.InvariantCultureIgnoreCase)
                || delObjDn.Equals(testUserGroup0DNForDs, StringComparison.InvariantCultureIgnoreCase)))
            {
                string guid = guidHashTable[delObjDn].ToString();
                string deletedBaseDN = "CN=Deleted Objects," + rootDomainNC;
                string objCN = delObjDn.Split(',')[0].Trim().ToString();
                string deletedObjDN = objCN + "\\0ADEL:" + guid + "," + deletedBaseDN;

                // LDAP_SERVER_SHOW_DELETED_OID "1.2.840.113556.1.4.417"
                // LDAP_SERVER_SHOW_RECYCLED_OID "1.2.840.113556.1.4.2064"
                DirectoryControl[] controls = new DirectoryControl[]{
                    new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true),
                    new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID, null, true, true)
                };
                result = adLdapClient.DeleteObject(
                    deletedObjDN,
                    controls,
                    isWindows);

                if (result.ToLower().Contains("success"))
                {
                    string isDelete = string.Empty;
                    string isRecycle = string.Empty;
                    List<string> attributeNotEmpty = new List<string>();
                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry resultEntry = SearchDeletedObject(
                        delObjDn,
                        ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID);

                    PartialAttributeList attributeNames = resultEntry.attributes;
                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                    {
                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                        AttributeValue[] attributeValList = attribute.vals.Elements;
                        foreach (AttributeValue attributeVal in attributeValList)
                        {
                            string attributeValue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                            if (attributeString.Equals("isDeleted", StringComparison.OrdinalIgnoreCase))
                            {
                                isDelete = bool.Parse(attributeValue).ToString();
                            }
                            if (attributeString.Equals("isRecycled", StringComparison.OrdinalIgnoreCase))
                            {
                                isRecycle = bool.Parse(attributeValue).ToString();
                            }
                            attributeNotEmpty.Add(attributeString.ToLower(CultureInfo.InvariantCulture));
                        }
                    }
                    VerifyDeletedObject(isDelete, isRecycle);

                    if (Utilities.IsOptionalFeatureEnabled(forestScopePartialDN + ',' + configurationNC, recycleBinPartialDN + ',' + configurationNC))
                    {
                        if (attributeNotEmpty.Contains("isRecycled".ToLower(CultureInfo.InvariantCulture)))
                        {
                            VerifyRecycledObjects(attributeNotEmpty, service);
                        }
                    }
                }
            }

            #endregion

            #region Delete the regular object

            else
            {
                result = adLdapClient.DeleteObject(delObjDn, null, isWindows);
            }

            if (!isWindows)
            {
                #region Switch ErrorStatus Non-Windows

                switch (result)
                {
                    case "Referral":
                        errorStatus = ConstrOnDelOpErr.Referral_UnKnownError;
                        break;
                    case "UnwillingToPerform":
                        errorStatus = ConstrOnDelOpErr.UnwillingToPerform_UnKnownError;
                        break;
                    case "NotAllowedOnNonLeaf":
                        errorStatus = ConstrOnDelOpErr.NotAllowedOnNonLeaf_UnKnownError;
                        break;
                    case "Success":

                        #region Searching for Deleted Object in Tombstones

                        tombStoneSearchResultEntry = Utilities.GetDeletedObject(guidHashTable[delObjDn].ToString(), defaultNC, currentWorkingDC.FQDN, currentPort);
                        if (tombStoneSearchResultEntry != null)
                        {
                            List<string> attributesAreNotNull = new List<string>();
                            foreach (string key in tombStoneSearchResultEntry.Properties.PropertyNames)
                            {
                                attributesAreNotNull.Add(key.ToLower(CultureInfo.InvariantCulture));
                                foreach (object value in tombStoneSearchResultEntry.Properties[key])
                                {
                                    if (key.ToLower(CultureInfo.InvariantCulture) == "isdeleted")
                                    {
                                        Site.CaptureRequirementIfAreEqual<string>(value.ToString().ToLower(CultureInfo.InvariantCulture), "true", 889, "During the Delete operation, the isDeleted attribute is set to true on tombstones.");
                                        Site.CaptureRequirementIfAreEqual<string>(value.ToString().ToLower(CultureInfo.InvariantCulture), "true", 924, "During the delete operation, the isDeleted attribute is set to true.");
                                    }
                                    if (key.ToLower(CultureInfo.InvariantCulture) == "distinguishedname")
                                    {
                                        //for distinguished Name attribute fPRESERVEONDELETE is set by default
                                        Site.CaptureRequirementIfAreEqual<string>(value.ToString().Substring(0, value.ToString().IndexOf('=')), delObjRdnType, 920, "During the delete operation, the attribute that equals the rdnType of the object is retained.");
                                        searchFlags = IntegerSymbols.ParseSystemFlagsValue("searchflags", "fPRESERVEONDELETE");
                                        Type type = IntegerSymbols.GetSymbolEnumType("searchflags");
                                        result = IntegerSymbols.UnparseUInt32Enum(type, (uint)searchFlags);
                                        Site.CaptureRequirementIfAreEqual<string>(result, "fPRESERVEONDELETE", 921, "During the delete operation, any attribute that has fPRESERVEONDELETE flag set in its searchFlags is retained, except objectCategory and sAMAccountType, which are always removed, regardless of the value of their searchFlags.");
                                        string[] DeletedObjectsContainer = value.ToString().Split(new char[] { ',' });
                                        Site.CaptureRequirementIfAreEqual<string>(DeletedObjectsContainer[1].ToString(), "CN=Deleted Objects", 892, "During the Delete operation, the tombstone remains in the database for at least the tombstone lifetime time interval after its deletion but the Deleted Objects container(which is considered a tombstone if the Recycle Bin optional feature is not enabled) always remains available.");
                                    }
                                    if (key.ToLower(CultureInfo.InvariantCulture) == "objectsid")
                                    {
                                        if (value is byte[])
                                        {
                                            byte[] byteVal = value as byte[];
                                            sidOftheNewObject = new SecurityIdentifier(byteVal, 0);
                                        }
                                        Site.CaptureRequirementIfAreEqual(delObjOldSid, sidOftheNewObject, 890, "During the Delete operation, the tombstone retains the objectGUID and objectSid (if any) attributes of the original object.");
                                    }
                                    if (key.ToLower(CultureInfo.InvariantCulture) == "objectguid")
                                    {
                                        if (value is byte[])
                                        {
                                            byte[] byteVal = value as byte[];
                                            guidOfNewObject = new Guid(byteVal);
                                            Site.CaptureRequirementIfAreEqual(delObjOldGuid, guidOfNewObject, 890, "During the Delete operation, the tombstone retains the objectGUID and objectSid (if any) attributes of the original object.");
                                        }
                                    }

                                    if (key.ToLower(CultureInfo.InvariantCulture) == "instancetype")
                                    {
                                        searchFlags = IntegerSymbols.ParseSystemFlagsValue("searchflags", "fPRESERVEONDELETE");
                                        Type type = IntegerSymbols.GetSymbolEnumType("searchflags");
                                        result = IntegerSymbols.UnparseUInt32Enum(type, (uint)searchFlags);
                                        Site.CaptureRequirementIfAreEqual<string>(result, "fPRESERVEONDELETE", 921, "During the delete operation, any attribute that has fPRESERVEONDELETE flag set in its searchFlags is retained, except objectCategory and sAMAccountType, which are always removed, regardless of the value of their searchFlags.");
                                        Site.CaptureRequirementIfAreEqual<int>(delObjInstanceType, int.Parse(value.ToString(), CultureInfo.InvariantCulture), 916, "During the delete operation, all attribute values are removed from the object except instanceType, objectGUID, objectSid, distinguishedName.");
                                    }
                                }
                            }

                            VerifyDeletedObjectRetainedAttribute(attributesAreNotNull);
                        }
                        else
                        {
                            Site.Log.Add(LogEntryKind.Debug, "No object exists");
                        }
                        #endregion

                        errorStatus = ConstrOnDelOpErr.success;
                        break;
                    default:
                        errorStatus = ConstrOnDelOpErr.UnSpecifiedError;
                        break;
                }

                #endregion
            }
            else
            {
                #region Switch ErrorStatus Windows

                switch (result)
                {
                    case "Referral_ERROR_DS_REFERRAL":
                        errorStatus = ConstrOnDelOpErr.Referral_ERROR_DS_REFERRAL;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CANT_DELETE":
                        errorStatus = ConstrOnDelOpErr.UnwillingToPerform_ERROR_DS_CANT_DELETE;
                        break;
                    case "NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST":
                        errorStatus = ConstrOnDelOpErr.NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST;
                        break;
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                        errorStatus = ConstrOnDelOpErr.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                        break;
                    case "Success_STATUS_SUCCESS":

                        #region Searching for Deleted Object in Tombstones

                        if (isEntryExist)
                        {
                            tombStoneSearchResultEntry = Utilities.GetDeletedObject(guidHashTable[delObjDn].ToString(), defaultNC, currentWorkingDC.FQDN, currentPort);
                            if (tombStoneSearchResultEntry != null)
                            {
                                List<string> attributesAreNotNull = new List<string>();
                                foreach (string key in tombStoneSearchResultEntry.Properties.PropertyNames)
                                {
                                    attributesAreNotNull.Add(key.ToLower(CultureInfo.InvariantCulture));
                                    foreach (object value in tombStoneSearchResultEntry.Properties[key])
                                    {
                                        if (key.ToLower(CultureInfo.InvariantCulture) == "isdeleted")
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(value.ToString().ToLower(CultureInfo.InvariantCulture), "true", 889, "During the Delete operation, the isDeleted attribute is set to true on tombstones.");
                                            Site.CaptureRequirementIfAreEqual<string>(value.ToString().ToLower(CultureInfo.InvariantCulture), "true", 924, "During the delete operation, the isDeleted attribute is set to true.");
                                        }
                                        if (key.ToLower(CultureInfo.InvariantCulture) == "distinguishedname")
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(value.ToString().Substring(0, value.ToString().IndexOf('=')).ToLower(CultureInfo.InvariantCulture), delObjRdnType.ToLower(CultureInfo.InvariantCulture), 920, "During the delete operation, the attribute that equals the rdnType of the object is retained.");
                                            searchFlags = IntegerSymbols.ParseSystemFlagsValue("searchflags", "fPRESERVEONDELETE");
                                            Type type = IntegerSymbols.GetSymbolEnumType("searchflags");
                                            result = IntegerSymbols.UnparseUInt32Enum(type, (uint)searchFlags);
                                            Site.CaptureRequirementIfAreEqual<string>(result.ToLower(CultureInfo.InvariantCulture), "fPRESERVEONDELETE".ToLower(CultureInfo.InvariantCulture), 921, "During the delete operation, any attribute that has fPRESERVEONDELETE flag set in its searchFlags is retained, except objectCategory and sAMAccountType, which are always removed, regardless of the value of their searchFlags.");
                                            string[] DeletedObjectsContainer = value.ToString().Split(new char[] { ',' });
                                            Site.CaptureRequirementIfAreEqual<string>(DeletedObjectsContainer[1].ToString().ToLower(CultureInfo.InvariantCulture), "CN=Deleted Objects".ToLower(CultureInfo.InvariantCulture), 892, "During the Delete operation, the tombstone remains in the database for at least the tombstone lifetime time interval after its deletion but the Deleted Objects container(which is considered a tombstone if the Recycle Bin optional feature is not enabled) always remains available.");
                                        }
                                        if (key.ToLower(CultureInfo.InvariantCulture) == "objectsid")
                                        {
                                            if (value is byte[])
                                            {
                                                byte[] byteVal = value as byte[];
                                                sidOftheNewObject = new SecurityIdentifier(byteVal, 0);
                                            }
                                        }
                                        if (key.ToLower(CultureInfo.InvariantCulture) == "objectguid")
                                        {
                                            if (value is byte[])
                                            {
                                                byte[] byteVal = value as byte[];
                                                guidOfNewObject = new Guid(byteVal);
                                            }
                                        }

                                        if (key.ToLower(CultureInfo.InvariantCulture) == "instancetype")
                                        {
                                            searchFlags = IntegerSymbols.ParseSystemFlagsValue("searchflags", "fPRESERVEONDELETE");
                                            Type type = IntegerSymbols.GetSymbolEnumType("searchflags");
                                            result = IntegerSymbols.UnparseUInt32Enum(type, (uint)searchFlags);
                                            Site.CaptureRequirementIfAreEqual<string>(result.ToLower(CultureInfo.InvariantCulture), "fPRESERVEONDELETE".ToLower(CultureInfo.InvariantCulture), 921, "During the delete operation, any attribute that has fPRESERVEONDELETE flag set in its searchFlags is retained, except objectCategory and sAMAccountType, which are always removed, regardless of the value of their searchFlags.");
                                        }
                                    }
                                }

                                VerifyDeletedObjectRetainedAttribute(attributesAreNotNull);
                            }
                        }
                        else
                        {
                            Site.Log.Add(LogEntryKind.Debug, "No object exists");
                        }
                        #endregion

                        errorStatus = ConstrOnDelOpErr.success;
                        break;
                    default:
                        errorStatus = ConstrOnDelOpErr.UnSpecifiedError;
                        break;
                }

                #endregion
            }

            #endregion

            #region Return values for specific attributes

            if (result.Contains("NoSuchObject"))
            {
                errorStatus = ConstrOnDelOpErr.NoSuchObject;
            }
            if (result.Contains("NoSuchObject") && isRODC)
            {
                errorStatus = ConstrOnDelOpErr.NoSuchObject;
            }
            if ((errorStatus != ConstrOnDelOpErr.success)
                && isSpecialDn)
            {
                errorStatus = ConstrOnDelOpErr.UnSpecifiedError;
            }

            #endregion
        }

        /// <summary>
        /// Performs Tree Delete operation
        /// </summary>
        /// <param name="delObjDn">DN of the object to be deleted</param>
        /// <param name="objectRights">Enum defining rights on object to be deleted</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Tree Delete Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        // Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        // Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
        // Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void TreeDeleteOperation(
            string delObjDn,
            RightsOnObjects objectRights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnDelOpErr errorStatus)
        {
            #region Variables

            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            string[] searchAttrVals = null;
            SearchResult tombStoneSearchResultEntry = null;
            DirectoryEntry deletedEntry = null;

            bool isEntryExist;
            bool isSpecialObject = false;

            string delObjRdnType = string.Empty;
            string delObjRdnValue = string.Empty;

            string objDelTombStoneDN = string.Empty;

            string instanceTypeFlagsResult = string.Empty;
            string systemFlagsResult = string.Empty;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                if (isRODC)
                {
                    SetConnectAndBind(service, RODCNetbiosName);
                }
                else
                {
                    SetConnectAndBind(service, PDCNetbiosName);
                }
            }

            #endregion

            #region Finalize DN of the object to be deleted

            if (delObjDn.Equals("DC=adts88")
                || delObjDn.Equals("CN=Users,DC=adts88")
                || delObjDn.Split(',')[0].Split('=')[1].Trim().Equals("ADTS88")
                || delObjDn.Split(',')[0].Split('=')[1].Trim().Equals("Enterprise Configuration")
                || delObjDn.Split(',')[0].Split('=')[1].Trim().Equals("Enterprise Schema")
                || delObjDn.Equals("OU=Domain Controllers,DC=adts88")
                || delObjDn.Equals("CN=RID Set,CN=WIN-6IEHBFZ8AMV,OU=Domain Controllers,DC=adts88")
                || delObjDn.Equals("CN=WIN-6IEHBFZ8AMV,OU=Domain Controllers,DC=adts88"))
            {
                isSpecialObject = true;
            }

            delObjDn = delObjDn.Replace("DC=adts88", rootDomainNC);
            delObjDn = delObjDn.Replace("WIN-6IEHBFZ8AMV", currentWorkingDC.NetbiosName);
            delObjDn = delObjDn.Replace("ADTS88", currentWorkingDC.Domain.NetbiosName);
            delObjDn = delObjDn.Replace("520ec681-1336-471b-93af-c27b780083d9", forestDNSZonesObjCN);

            // assign RDN type and value "CN=XXX" or "DC=XXX"
            delObjRdnType = delObjDn.Split(',')[0].Split('=')[0].Trim();
            delObjRdnValue = delObjDn.Split(',')[0].Split('=')[1].Trim();

            #endregion

            #region Get instanceType for rootDomainNC => instanceType=5 (NC Head | NC Write)

            result = adLdapClient.SearchObject(
                rootDomainNC,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "(instanceType=5)",
                new string[] { "instanceType" },
                null,
                out searchResponse,
                isWindows);
            Site.Assert.IsTrue(result.ToLower().Contains("success"),
                string.Format("Search operation on object {0} should be successful, actual result: {1}", rootDomainNC, result));
            if (searchResponse != null)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "instanceType");
                    instanceTypeFlagsResult = IntegerSymbols.UnparseUInt32Enum(
                        typeof(InstanceTypeFlags),
                        (uint)int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture));
                }
            }

            #endregion

            #region Get systemFlags for rootDomainNC => systemFlags=0x02000000 (FLAG_DISALLOW_MOVE_ON_DELETE)

            result = adLdapClient.SearchObject(
                    rootDomainNC,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "(systemFlags=33554432)",
                    new string[] { "distinguishedName", "systemFlags", "instanceType" },
                    null,
                    out searchResponse,
                    isWindows);
            Site.Assert.IsTrue(result.ToLower().Contains("success"),
                string.Format("Search operation on object {0} should be successful, actual result: {1}", rootDomainNC, result));
            if (searchResponse != null)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                        (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                        entrypacket.GetInnerRequestOrResponse();
                    string objectName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                    if (objectName.ToLower(CultureInfo.InvariantCulture) == ("CN=Servers,CN=AdtsTestSite,CN=Sites," + configurationNC).ToLower(CultureInfo.InvariantCulture))
                    {
                        searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "systemFlags");
                        systemFlagsResult = IntegerSymbols.UnparseUInt32Enum(
                            typeof(SystemFlags),
                            (uint)int.Parse(searchAttrVals[0], CultureInfo.InvariantCulture));
                    }
                }
            }

            #endregion

            #region Search if object to be deleted exists in regular state

            isEntryExist = Utilities.IsObjectExist(delObjDn, currentWorkingDC.FQDN, currentPort);

            #endregion

            #region If object to be deleted is ForestDnsZones and it does not exist => add it

            //Check if the object: DC=ForestDnsZones exists or not, if not, add it to AD to make sure the object exist
            if (!isEntryExist
                && (delObjRdnValue == forestDNSZonesObjCN))
            {
                AddForestDnsZones(forestDNSZonesObjCN);
                isEntryExist = true;
            }

            #endregion

            #region Search if the object to be deleted is in tombstone

            if (!isEntryExist)
            {
                if (delObjDn.Contains("AdtsTestSite"))
                {
                    deletedEntry = Utilities.BuildDeletedEntry(
                        currentWorkingDC.FQDN,
                        DELETED_OBJECTS_CONTAINER_GUID,
                        defaultNC);
                    tombStoneSearchResultEntry = Utilities.GetTombstone(deletedEntry, "AdtsTestSite");
                }
                else
                {
                    tombStoneSearchResultEntry = Utilities.GetDeletedObject(guidHashTable[delObjDn].ToString(), defaultNC, currentWorkingDC.FQDN, currentPort);
                }

                if (tombStoneSearchResultEntry != null)
                {
                    objDelTombStoneDN = tombStoneSearchResultEntry.Properties["distinguishedName"][0].ToString();
                }
            }

            #endregion

            #region Tree Delete the tombstone-object

            if (!(string.IsNullOrEmpty(objDelTombStoneDN)))
            {
                result = adLdapClient.DeleteTreeControl(
                    objDelTombStoneDN,
                    isWindows);
            }

            #endregion

            #region Tree Delete the regular-object

            // if object already exist in AD => tree delete it directly
            if (isEntryExist)
            {
                Utilities.SetAccessRights(
                    delObjDn,
                    testUserName,
                    currentWorkingDC.Domain.NetbiosName,
                    ActiveDirectoryRights.DeleteTree,
                    AccessControlType.Allow);

                if (!delObjDn.ToLower(CultureInfo.InvariantCulture)
                    .Contains("CN=Sites,CN=Configuration,DC=".ToLower(CultureInfo.InvariantCulture)))
                {
                    result = adLdapClient.DeleteTreeControl(
                        delObjDn,
                        isWindows);
                }
                else
                {
                    // Will not actually delete nTDSDSA objects
                    isSpecialObject = true;
                    result = string.Empty;
                }
            }

            #endregion

            #region Tree Delete the deleted-object or recycled-object

            // if object is test site and it does not exist in AD => create the test site first
            else
            {
                #region create test site and get its systemFlags attribute

                if (delObjDn.Equals(testSite0DNForDs, StringComparison.InvariantCultureIgnoreCase))
                {
                    Utilities.CreateNewSite(
                        delObjRdnValue,
                        PDCNetbiosName + '.' + PrimaryDomainDnsName);

                    DirectoryEntry objToDelEntry =
                        new DirectoryEntry("LDAP://" + currentWorkingDC.FQDN + ":" + currentPort + "/" + delObjDn);
                    systemFlagsResult = IntegerSymbols.UnparseUInt32Enum(
                        typeof(SystemFlags),
                        uint.Parse(objToDelEntry.Properties["systemFlags"].Value.ToString()));

                }

                #endregion

                #region if object is already deleted and may in the state of deleted, or recycled

                if (delObjDn.Contains("0ADEL"))
                {
                    // Now this object is deleted-object
                    ShowDeletedControl showDeleted = new ShowDeletedControl();
                    DirectoryControl[] controls = new DirectoryControl[] { showDeleted };
                    result = adLdapClient.SearchObject(
                        delObjDn,
                        System.DirectoryServices.Protocols.SearchScope.OneLevel,
                        "(isDeleted=TRUE)",
                        null,
                        controls,
                        out searchResponse,
                        isWindows);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                        string.Format("Search operation on {0} should be successful, actual result: {1}", configurationNC, result));
                    Site.CaptureRequirementIfAreEqual(
                        0,
                        searchResponse.Count,
                        4409,
                        @"[Deleted-Object Requirements] The deleted-object does not have descendant objects.");

                    result = adLdapClient.DeleteObject(
                        delObjDn,
                        controls,
                        isWindows);

                    // Now this object is recycled-object
                    DirectoryControl showRecycled = new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID, null, true, true);
                    controls = new DirectoryControl[] { showRecycled };
                    result = adLdapClient.SearchObject(
                        delObjDn,
                        System.DirectoryServices.Protocols.SearchScope.OneLevel,
                        "(isRecycled=TRUE)",
                        null,
                        controls,
                        out searchResponse,
                        isWindows);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                        string.Format("Search operation on {0} should be successful, actual result: {1}", configurationNC, result));
                    Site.CaptureRequirementIfAreEqual(
                        0,
                        searchResponse.Count,
                        4422,
                        @"[Recycled-Object Requirements] The recycled-object does not have descendant objects.");
                }

                #endregion
            }

            if (!isWindows)
            {
                #region Switch ErrorStatus Non-Windows

                switch (result)
                {
                    case "Referral":
                        errorStatus = ConstrOnDelOpErr.Referral_UnKnownError;
                        break;
                    case "UnwillingToPerform":
                        errorStatus = ConstrOnDelOpErr.UnwillingToPerform_UnKnownError;
                        break;
                    case "NotAllowedOnNonleaf":
                        errorStatus = ConstrOnDelOpErr.NotAllowedOnNonLeaf_UnKnownError;
                        break;
                    case "NoSuchObject":
                        errorStatus = ConstrOnDelOpErr.NoSuchObject;
                        break;
                    case "Success":
                        errorStatus = ConstrOnDelOpErr.success;
                        break;
                    default:
                        errorStatus = ConstrOnDelOpErr.UnSpecifiedError;
                        break;
                }

                #endregion
            }
            else
            {
                #region Switch ErrorStatus Windows

                switch (result)
                {
                    case "Referral_ERROR_DS_REFERRAL":
                        errorStatus = ConstrOnDelOpErr.Referral_ERROR_DS_REFERRAL;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CANT_DELETE":
                        errorStatus = ConstrOnDelOpErr.UnwillingToPerform_ERROR_DS_CANT_DELETE;
                        break;
                    case "NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST":
                        errorStatus = ConstrOnDelOpErr.NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST;
                        break;
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                        errorStatus = ConstrOnDelOpErr.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                        break;
                    case "NoSuchObject_ERROR_DS_OBJ_NOT_FOUND":
                        errorStatus = ConstrOnDelOpErr.NoSuchObject;
                        break;
                    case "Success_STATUS_SUCCESS":
                        errorStatus = ConstrOnDelOpErr.success;
                        break;
                    default:
                        errorStatus = ConstrOnDelOpErr.UnSpecifiedError;
                        break;
                }

                #endregion
            }

            #endregion

            #region Requirements on Tree Delete Operations

            if (errorStatus.Equals(ConstrOnDelOpErr.success))
            {
                #region Verify the new test site is deleted

                tombStoneSearchResultEntry = Utilities.GetTombstone(deletedEntry, "AdtsTestSite");
                isEntryExist = Utilities.IsObjectExist(
                    testSite0DNForDs,
                    currentWorkingDC.FQDN,
                    currentPort);
                if (!isEntryExist &&
                    systemFlagsResult.Contains("FLAG_DISALLOW_MOVE_ON_DELETE"))
                {
                    Site.CaptureRequirementIfIsNull(
                        tombStoneSearchResultEntry,
                        928,
                        @"If the systemFlags value has FLAG_DISALLOW_MOVE_ON_DELETE bit set on the object being deleted, 
                        then it is not moved into the Deleted Objects container in its NC.");
                    Site.CaptureRequirementIfIsNull(
                        tombStoneSearchResultEntry,
                        926,
                        @"During the delete operation, for originating updates, the object being deleted is moved into 
                        the Deleted Objects container in its NC, except in the following scenarios, when it must remain 
                        in its current place: 1) The object is an NC root. 2) The object's systemFlags value has 
                        FLAG_DISALLOW_MOVE_ON_DELETE bit set.");
                }

                #endregion

                // Tree delete operation on the server is performed by setting the LDAP_SERVER_TREE_DELETE_OID. 
                // Hence the requirement is captured here.
                Site.CaptureRequirement(
                    410,
                    @"The LDAP_SERVER_TREE_DELETE_OID control is used with an LDAP delete operation to cause the server to 
                    recursively delete the entire subtree of objects located underneath the object specified in the delete operation.");
                if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                {
                    Site.CaptureRequirement(
                        1219,
                        @"The LDAP extended control LDAP_SERVER_TREE_DELETE_OID is supported in Windows 2000, Windows Server 2003,
                    Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2 or later.");
                }
                Site.CaptureRequirement(
                    4441,
                    @"[constraints are enforced for the delete operation] This constraint is not effective if the requester is passing
                the LDAP_SERVER_TREE_DELETE_OID control (see section 3.1.1.5.5.7, Tree-delete Operation).");
            }
            else
            {
                if (!String.IsNullOrEmpty(instanceTypeFlagsResult))
                {
                    Site.CaptureRequirementIfAreEqual<string>(
                        instanceTypeFlagsResult,
                        "IT_NC_HEAD, IT_WRITE",
                        927,
                        @"If the object being deleted is an NC root, then it is not moved into the Deleted Objects container in its NC.");
                }
            }

            #endregion

            #region Return values for specific attributes

            if ((errorStatus != ConstrOnDelOpErr.success)
                && isSpecialObject)
            {
                errorStatus = ConstrOnDelOpErr.UnSpecifiedError;
            }

            #endregion
        }

        #endregion

        #region Modify Operations

        /// <summary>
        /// Action describing the behavior of ModifyOperation
        /// </summary>
        /// <param name="attribVal">Variable that contains attributes to be modified mapped to the list of 
        ///                         existing attributes on a particular object</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void ModifyOperation(
            Map<string, Sequence<string>> attribVal,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus)
        {
            Site.Log.Add(LogEntryKind.Debug, "[ModifyOperation]: Entering");

            #region Variables

            errorStatus = ConstrOnModOpErrs.success;

            DirectoryAttributeModification modifyAttr = new DirectoryAttributeModification();
            List<DirectoryAttributeModification> attrValsToBeModified = new List<DirectoryAttributeModification>();

            string serverName = string.Empty;
            string attrToModify = string.Empty;
            string attrToGetObject = string.Empty;
            string objectClass = string.Empty;
            string objectDN = string.Empty;

            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            string[] searchAttrVals = null;

            #endregion

            #region Connect and Bind

            Site.Log.Add(LogEntryKind.Debug, "Construct the target host name from input: ");

            foreach (Sequence<string> attribnVals in attribVal.Values)
            {
                foreach (string item in attribnVals)
                {
                    Site.Log.Add(LogEntryKind.Debug, "Attributes to identify object: {0}", item);
                    if (item.Contains("DC=NotPDCFSMO"))
                    {
                        serverName = ConstructServerHostName(isRODC, "DC=NotPDCFSMO");
                    }
                    else if (item.Contains("DC=notWritableDC"))
                    {
                        serverName = ConstructServerHostName(isRODC, "DC=notWritableDC");
                    }
                    else if (item.Contains("DC=writableDC"))
                    {
                        serverName = ConstructServerHostName(isRODC, "DC=WritableDC");
                    }
                    else if (item.Contains("DC=WritableDCNotSameDomain"))
                    {
                        serverName = ConstructServerHostName(isRODC, "DC=WritableDCNotSameDomain");
                    }
                    else if (isRODC)
                    {
                        serverName = RODCNetbiosName;
                    }
                    else
                    {
                        serverName = PDCNetbiosName;
                    }
                }
            }
            foreach (string key in attribVal.Keys)
            {
                Site.Log.Add(LogEntryKind.Debug, "Attributes to be modified: {0}", key);
                if (key.Contains("becomePdc"))
                {
                    serverName = PDCNetbiosName;
                }
                if (key.Contains("runProtectAdminGroupsTask:NotPDCFSMOOwner"))
                {
                    serverName = ConstructServerHostName(isRODC, "DC=NotPDCFSMO");
                }
            }
            Site.Log.Add(LogEntryKind.Debug, "Constructed target host name: {0}", serverName);
            Site.Log.Add(LogEntryKind.Debug, "Server isConnected: {0}", isConnected);
            if (isConnected == false)
            {
                SetConnectAndBind(service, serverName);
            }

            #endregion

            #region RootDSE Modify schemaUpdateNow

            Site.Log.Add(LogEntryKind.Debug, "Update Schema Now: ");

            // [MS-ADTS] section 3.1.1.3.3.13 schemaUpdateNow
            // After the completion of this operation, the subschema exposed by the server reflects the current state
            // of the schema as defined by the attributeSchema and classSchema objects in the schema NC.
            DirectoryAttributeModification schemaRefresh = new DirectoryAttributeModification();
            schemaRefresh.Name = "schemaUpdateNow";
            schemaRefresh.Add("1");
            schemaRefresh.Operation = DirectoryAttributeOperation.Add;
            List<DirectoryAttributeModification> refreshAttributes = new List<DirectoryAttributeModification>();
            refreshAttributes.Add(schemaRefresh);
            try
            {
                Site.Log.Add(LogEntryKind.Debug, "[ModifyObject]: Entering");
                result = adLdapClient.ModifyObject(null, refreshAttributes, null, isWindows);
                Site.Log.Add(LogEntryKind.Debug, "[ModifyObject]: Exiting");
                Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("RootDSE Modify operation on schemaUpdateNow should be successful, actual result: {0}", result));
            }
            catch (Exception ex)
            {
                result = string.Empty;
                Site.Log.Add(LogEntryKind.Warning, "ModifyObject throw exception: {0}", ex.Message);
            }

            #endregion

            #region Get information for objects and their attributes to be modified

            Site.Log.Add(LogEntryKind.Debug, "Get information for objects and their attributes to be modified: ");

            foreach (string key in attribVal.Keys)
            {
                Site.Log.Add(LogEntryKind.Debug, "Attributes to be modified: {0}", key);

                #region For RootDSE Modify Negative Cases => remove all related Control Access Rights

                Site.Log.Add(LogEntryKind.Debug, "For RootDSE Modify Negative Cases, remove all related Control Access Rights");
                string attrName = key.Split(':')[0].Trim();

                switch (attrName.ToLower(CultureInfo.InvariantCulture))
                {
                    case "becomedomainmaster":
                        Utilities.SetControlAcessRights("CN=Partitions," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Domain_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "becomeinfrastructuremaster":
                        Utilities.SetControlAcessRights("CN=Infrastructure," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Infrastructure_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "becomepdc":
                    case "becomepdcwithcheckpoint":
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_PDC, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "becomeridmaster":
                        Utilities.SetControlAcessRights("CN=RID Manager$,CN=System," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Rid_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "becomeschemamaster":
                        Utilities.SetControlAcessRights(schemaNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Schema_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "checkphantoms":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.DS_Check_Stale_Phantoms, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "dogarbagecollection":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "dumpdatabase":
                        // The requester must be a member of the BUILTIN\Administrtors group
                        break;
                    case "fixupinheritance":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Recalculate_Security_Inheritance, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "invalidateridpool":
                        Utilities.SetControlAcessRights("CN=RID Manager$,CN=System," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Rid_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "recalchierarchy":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Recalculate_Hierarchy, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "schemaupdatenow":
                        Utilities.SetControlAcessRights(schemaNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Update_Schema_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Update_Schema_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "schemaupgradeinprogress":
                        // The requester must have the "Change-Schema-Master" control access right on the root of the schema NC replica.
                        break;
                    case "removelingeringobject":
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.DS_Replication_Synchronize, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "dolinkcleanup":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "doonlinedefrag":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "replicatesingleobject":
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.DS_Replication_Synchronize, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "updatecachedmemberships":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Refresh_Group_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "dogarbagecollectionphantomsnow":
                        Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "invalidategcconnection":
                        // The requester must be a member of either the BUILTIN\Administrators group or the BUILTIN\Server Operators group.
                        break;
                    case "renewservercertificate":
                        // The requester must have the "Reload-SSL-Certificate" control access right on the nTDSDSA object for the DC.
                        break;
                    case "rodcpurgeaccount":
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Read_Only_Replication_Secret_Synchronization, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "runsamupgradetasks":
                        // The requester MUST be a member of the "Domain Admins" group in the domain to perform this operation.
                        break;
                    case "sqmrunonce":
                        // The requester must have the SE_DEBUG_PRIVILEGE.
                        break;
                    case "runprotectadmingroupstask":
                        // The requester must have the "Run-Protect-Admin-Groups-Task" control access right on the domain root of the DC.
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Run_Protect_Admin_Groups_Task, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        break;
                    case "disableoptionalfeature":
                        // The requester must have the correct "Manage-Optional-Features" control access on the object representing the scope.
                        break;
                    case "enableoptionalfeature":
                        // The requester must have the "Manage-Optional-Features" control access right on the object representing the scope.
                        break;
                    case "dumpreferences":
                        // The requester must be a member of the BUILTIN\Administrators group.
                        break;
                    case "dumplinks":
                        // The requester must be a member of the BUILTIN\Administrators group.
                        break;
                    case "schemaupdateindicesnow":
                        // The requester must have the "Update-Schema-Cache" control access right on the nTDSDSA object for the DC or on the root of the schema NC.
                        break;
                    case "null":
                        break;
                    default:
                        break;
                }

                #endregion

                #region Construct the list of attributes to be modified from parameter [attribVal]

                Site.Log.Add(LogEntryKind.Debug, "Construct the attributes to be modified from: {0}", key);
                attrToModify = key;
                switch (service)
                {
                    case ADImplementations.AD_LDS:
                        attrToModify = attrToModify.Replace("CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}", configurationNC.Split(',')[1]);
                        attrToModify = attrToModify.Replace("CN=ApplicationNamingContext,DC=adts88", defaultNC);
                        break;
                    case ADImplementations.AD_DS:
                    default:
                        attrToModify = attrToModify.Replace("DC=adts88", rootDomainNC);
                        attrToModify = attrToModify.Replace("ADTS_XP.adts88", string.Format("{0}.{1}", testComputer1Name, PrimaryDomainDnsName));
                        attrToModify = attrToModify.Replace("ADTS_XP", testComputer1Name);
                        attrToModify = attrToModify.Replace("ADTS88", rootDomainNC.Split(',')[0].Trim().Split('=')[1]);
                        break;
                }
                attrToModify = attrToModify.Replace("WIN-6IEHBFZ8AMV", currentWorkingDC.NetbiosName);
                attrToModify = attrToModify.Replace("single-valuedAttribute", "street");
                attrToModify = attrToModify.Replace("linkattribute", "member");

                modifyAttr.Name = attrToModify.Split(':')[0].Trim();

                //For R693, Value must be removed
                if (attrToModify.Equals("DefunctAttribute7: noValue") || attrToModify.Equals("mayContain: noValue"))
                {
                    modifyAttr.Operation = DirectoryAttributeOperation.Delete;
                }
                else if (attrToModify.Equals("description: <Not Set>"))
                {
                    modifyAttr.Operation = DirectoryAttributeOperation.Delete;
                    //represents some junk value to be removed which is not set.
                    modifyAttr.Add("somejunkvalue");
                }
                else if (attrToModify.Equals("description: <Not Set1>"))
                {
                    modifyAttr.Operation = DirectoryAttributeOperation.Delete;
                }
                else if (attrToModify.Equals("description: <xyz>"))
                {
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    modifyAttr.Add("somejunkvalue");
                }
                else if (attrToModify.Equals("description: <xy>"))
                {
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    modifyAttr.Add("somejunkvalue");
                }
                else if (attrToModify.Equals("street: addValue"))
                {
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    modifyAttr.Add("suzhou street");
                }
                else if (attrToModify.Contains("member"))
                {
                    if (attrToModify.Contains("removal"))
                    {
                        modifyAttr.Operation = DirectoryAttributeOperation.Delete;
                        modifyAttr.Add(attrToModify.Split(':')[2].ToString());
                    }
                    else if (attrToModify.Contains("replacement"))
                    {
                        modifyAttr.Operation = DirectoryAttributeOperation.Replace;
                        modifyAttr.Add(attrToModify.Split(':')[2].ToString());
                    }
                }
                else if (attrToModify.Contains("displayName"))
                {
                    if (attrToModify.Contains("removal"))
                    {
                        modifyAttr.Operation = DirectoryAttributeOperation.Delete;
                    }
                }
                else if (attrToModify.Contains("objectClass: 88object"))
                {
                    modifyAttr.Operation = DirectoryAttributeOperation.Replace;
                    modifyAttr.Add("classSchema");
                    modifyAttr.Add("user");
                }
                else
                {
                    if (attrToModify.Split(':')[1].Contains(";"))
                    {
                        if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("replicateSingleObject".ToLower(CultureInfo.InvariantCulture))
                            || attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("removeLingeringObject".ToLower(CultureInfo.InvariantCulture)))
                        {
                            modifyAttr.Add(attrToModify.Split(':')[1].Trim());
                        }
                        else
                        {
                            modifyAttr.AddRange(attrToModify.Split(':')[1].Trim().Split(';'));
                        }
                    }
                    else
                    {
                        if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("replicateSingleObject".ToLower(CultureInfo.InvariantCulture))
                            || attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("removeLingeringObject".ToLower(CultureInfo.InvariantCulture)))
                        {
                            modifyAttr.Add(attrToModify.Split(':')[1].Trim() + ":" + attrToModify.Split(':')[2].Trim());
                        }
                        else
                        {
                            modifyAttr.Add(attrToModify.Split(':')[1].Trim());
                        }
                    }
                    modifyAttr.Operation = DirectoryAttributeOperation.Replace;
                }
                Site.Log.Add(LogEntryKind.Debug, "Modify attribute name: {0}", modifyAttr.Name);
                Site.Log.Add(LogEntryKind.Debug, "Modify attribute operation: {0}", modifyAttr.Operation);
                foreach (string value in modifyAttr.GetValues(typeof(System.String)))
                {
                    Site.Log.Add(LogEntryKind.Debug, "Modify attribute value: {0}", value);
                }
                attrValsToBeModified.Add(modifyAttr);

                #endregion

                #region Get the attributes that are used to find the object to be modified from parameter [attribVal]

                Site.Log.Add(LogEntryKind.Debug, "Get the attributes that are used to find the object to be modified: ");
                foreach (string values in attribVal[key])
                {
                    attrToGetObject = values;
                    switch (service)
                    {
                        case ADImplementations.AD_LDS:
                            attrToGetObject = attrToGetObject.Replace("CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}", configurationNC.Split(',')[1]);
                            attrToGetObject = attrToGetObject.Replace("CN=ApplicationNamingContext,DC=adts88", defaultNC);
                            break;
                        case ADImplementations.AD_DS:
                        default:
                            attrToGetObject = attrToGetObject.Replace("DC=adts88", rootDomainNC);
                            attrToGetObject = attrToGetObject.Replace("ADTS_XP.adts88", string.Format("{0}.{1}", testComputer1Name, PrimaryDomainDnsName));
                            attrToGetObject = attrToGetObject.Replace("ADTS_XP", testComputer1Name);
                            attrToGetObject = attrToGetObject.Replace("ADTS88", rootDomainNC.Split(',')[0].Trim().Split('=')[1]);
                            break;
                    }
                    attrToGetObject = attrToGetObject.Replace("WIN-6IEHBFZ8AMV", currentWorkingDC.NetbiosName);
                    attrToGetObject = attrToGetObject.Replace("single-valuedAttribute", "street");
                    attrToGetObject = attrToGetObject.Replace("linkattribute", "member");

                    if (attrToGetObject.Contains("<GUID="))
                    {
                        testUserGuid = Utilities.GetUserGuid(
                            PDCNetbiosName,
                            PrimaryDomainDnsName,
                            ADDSPortNum,
                            testUserName,
                            testUserPwd,
                            testUserName);
                        attrToGetObject = attrToGetObject.Replace("<GUID=", "<GUID=" + testUserGuid);
                    }
                    if (attrToGetObject.Contains("<SID="))
                    {
                        testUserSid = Utilities.GetUserSid(
                            PDCNetbiosName,
                            PrimaryDomainDnsName,
                            testUserName,
                            testUserPwd,
                            testUserName);
                        attrToGetObject = attrToGetObject.Replace("<SID=", "<SID=" + testUserSid);
                    }
                    if (attrToGetObject.Contains("distinguishedName"))
                    {
                        objectDN = attrToGetObject.Split(':')[1].Trim();
                        if (objectDN.Equals("null"))
                        {
                            objectDN = null;
                        }
                    }
                    if (attrToGetObject.Contains("objectClass"))
                    {
                        objectClass = attrToGetObject.Split(':')[1].Trim();
                    }
                }

                #endregion
            }

            #endregion

            #region Modify Operation

            if (currentWorkingDC.OSVersion.Equals(ServerVersion.Win2003)
                || objectClass.Equals("msDS-PasswordSettings", StringComparison.InvariantCultureIgnoreCase))
            {
                errorStatus = ConstrOnModOpErrs.success;
            }
            else
            {
                #region Finalize object distinguished name

                if (objectDN != null)
                {
                    string rodcNTDSSettingsDN = string.Format("CN=NTDS Settings,CN={0},{1}", RODCNetbiosName, serversContainerDNForDs);
                    string pdcNTDSSettingsDN = string.Format("CN=NTDS Settings,CN={0},{1}", PDCNetbiosName, serversContainerDNForDs);

                    objectDN = objectDN.Replace("ADTS_XP", testComputer1Name);
                    objectDN = objectDN.Replace("DC=NotPDCFSMO", rootDomainNC);
                    objectDN = objectDN.Replace("CN=RODC,DC=writableDC", rodcNTDSSettingsDN);
                    objectDN = objectDN.Replace("CN=RODC,DC=notWritableDC", rodcNTDSSettingsDN);
                    objectDN = objectDN.Replace("CN=RODC,DC=WritableDCNotSameDomain", rodcNTDSSettingsDN);
                    objectDN = objectDN.Replace(pdcNTDSSettingsDN, rodcNTDSSettingsDN);
                }

                // Call modify to update deleted object
                // objectDN = "CN=user6750,CN=Users,DC=contoso,DC=com"
                // objectDN = "CN=user6751,CN=Users,DC=contoso,DC=com"
                if (objectDN != null)
                {
                    if(objectDN.Equals(testUser0DNForDs, StringComparison.InvariantCultureIgnoreCase)
                        || objectDN.Equals(testUser1DNForDs, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // the entry is deleted
                        if (!Utilities.IsObjectExist(objectDN, currentWorkingDC.FQDN, currentPort))
                        {
                            string guid = guidHashTable[objectDN].ToString();
                            string deletedBaseDN = "CN=Deleted Objects," + rootDomainNC;
                            string objCN = objectDN.Split(',')[0].Trim().ToString();
                            string deletedObjDN = objCN + "\\0ADEL:" + guid + "," + deletedBaseDN;
                            objectDN = deletedObjDN;
                        }
                    }
                }

                #endregion

                #region Set Validated Writes for specific attributes modify

                // [MS-ADTS] section 3.1.1.5.3.1.1.5 msDS-Behavior-Version
                // When modification is taking place, the requester must have the Validated-MS-DS-Behavior-Version validated write right.
                if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("msDS-Behavior-Version".ToLower(CultureInfo.InvariantCulture)))
                {
                    Site.Log.Add(LogEntryKind.Debug, "Set Validated-Writes for modifying attribute: {0}", attrToModify);
                    string currentUser = testUserName;
                    string currentPwd = testUserPwd;
                    if (currentWorkingDC.NetbiosName.Equals(CDCNetbiosName))
                    {
                        currentUser = childAdminName;
                        currentPwd = childAdminPwd;
                    }
                    Site.Log.Add(LogEntryKind.Debug, "Domain: {0}, Username: {1}, Password: {2}", currentWorkingDC.Domain.FQDN, currentUser, currentPwd);

                    int retryCount = 0;
                    while (!Utilities.isAuthorizedOrNot(
                        objectDN,
                        currentUser,
                        currentWorkingDC.Domain.NetbiosName,
                        ActiveDirectoryRights.WriteProperty,
                        currentWorkingDC.FQDN,
                        testUserName,
                        testUserPwd))
                    {
                        if (retryCount++ > 5)
                        {
                            throw new TimeoutException();
                        }

                        Site.Log.Add(LogEntryKind.Debug, "WriteProperty access right is not set!");
                        Site.Log.Add(LogEntryKind.Debug, "Set WriteProperty access right...");
                        Utilities.SetAccessRights(
                            objectDN,
                            currentUser,
                            currentWorkingDC.Domain.NetbiosName,
                            ActiveDirectoryRights.WriteProperty,
                            AccessControlType.Allow,
                            currentWorkingDC.FQDN,
                            testUserName,
                            testUserPwd);
                        Site.Log.Add(LogEntryKind.Debug, "Sleep for 30 seconds for access right to take effect.");
                        System.Threading.Thread.Sleep(30000);
                    }
                    Site.Log.Add(LogEntryKind.Debug, "WriteProperty access right is set successfully!");

                    retryCount = 0;
                    while (!Utilities.isAuthorizedOrNotWithGuid(
                        objectDN,
                        currentUser,
                        currentWorkingDC.Domain.NetbiosName,
                        ActiveDirectoryRights.Self,
                        ValidatedWrite.Validated_MS_DS_Behavior_Version,
                        currentWorkingDC.FQDN,
                        testUserName,
                        testUserPwd))
                    {
                        if (retryCount++ > 5)
                        {
                            throw new TimeoutException();
                        }

                        Site.Log.Add(LogEntryKind.Debug, "Validated_MS_DS_Behavior_Version access right is not set!");
                        Site.Log.Add(LogEntryKind.Debug, "Set Validated_MS_DS_Behavior_Version access right...");
                        Utilities.SetControlAcessRights(
                            objectDN,
                            currentUser,
                            currentWorkingDC.Domain.NetbiosName,
                            ValidatedWrite.Validated_MS_DS_Behavior_Version,
                            ActiveDirectoryRights.Self,
                            AccessControlType.Allow,
                            currentWorkingDC.FQDN,
                            testUserName,
                            testUserPwd);
                        Site.Log.Add(LogEntryKind.Debug, "Sleep for 30 seconds for access right to take effect.");
                        System.Threading.Thread.Sleep(30000);
                    }
                    Site.Log.Add(LogEntryKind.Debug, "Validated_MS_DS_Behavior_Version access right is set successfully!");
                }

                #endregion

                #region RootDSE Modify => set related Control Access Rights

                #region becomeDomainMaster

                if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeDomainMaster".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=Partitions," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Domain_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=Partitions," + configurationNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Domain_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region becomeInfrastructureMaster

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeInfrastructureMaster".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=Infrastructure," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Infrastructure_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=Infrastructure," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Infrastructure_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region becomePdc

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Split(':')[0].Trim().Equals("becomePdc".ToLower(CultureInfo.InvariantCulture)))
                {
                    #region GettingDomainSid
                    NTAccount accountname = new NTAccount(currentWorkingDC.Domain.NetbiosName, testUserName);//NTAccount("Domain Name or Account Name?")
                    SecurityIdentifier sid = (SecurityIdentifier)accountname.Translate(typeof(SecurityIdentifier));//get the account SID
                    SecurityIdentifier Domainsid = sid.AccountDomainSid;//get the Domain SID
                    byte[] sidByteArray = new byte[Domainsid.BinaryLength];
                    Domainsid.GetBinaryForm(sidByteArray, 0);
                    #endregion

                    attrValsToBeModified.RemoveAt(0);
                    DirectoryAttributeModification becomePdcModifyAttr = new DirectoryAttributeModification();
                    becomePdcModifyAttr.Name = attrToModify.Split(':')[0].Trim();
                    becomePdcModifyAttr.Add(sidByteArray);
                    becomePdcModifyAttr.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(becomePdcModifyAttr);
                    Utilities.RemoveControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_PDC, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_PDC, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region becomePdcWithCheckPoint

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomePdcWithCheckPoint".ToLower(CultureInfo.InvariantCulture)))
                {
                    #region GettingDomainSid
                    NTAccount accountname = new NTAccount(currentWorkingDC.Domain.NetbiosName, testUserName);//NTAccount("Domain Name or Account Name")
                    SecurityIdentifier sid = (SecurityIdentifier)accountname.Translate(typeof(SecurityIdentifier));//get the account SID
                    SecurityIdentifier Domainsid = sid.AccountDomainSid;//get the Domain SID
                    byte[] sidByteArray = new byte[Domainsid.BinaryLength];
                    Domainsid.GetBinaryForm(sidByteArray, 0);
                    #endregion

                    attrValsToBeModified.RemoveAt(0);
                    DirectoryAttributeModification becomePdcWithCheckPoint = new DirectoryAttributeModification();
                    becomePdcWithCheckPoint.Name = attrToModify.Split(':')[0].Trim();
                    becomePdcWithCheckPoint.Add(sidByteArray);
                    becomePdcWithCheckPoint.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(becomePdcWithCheckPoint);
                    Utilities.RemoveControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_PDC, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_PDC, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region becomeRidMaster

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeRidMaster".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=RID Manager$,CN=System," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Rid_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=RID Manager$,CN=System," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Rid_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    Utilities.SetAccessRights("CN=RID-Manager-Reference," + schemaNC, testUserName, currentWorkingDC.Domain.NetbiosName, ActiveDirectoryRights.ReadProperty, AccessControlType.Allow);
                }
                #endregion

                #region becomeSchemaMaster

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeSchemaMaster".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights(schemaNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Schema_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights(schemaNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Schema_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region checkPhantoms

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("checkPhantoms".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.DS_Check_Stale_Phantoms, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.DS_Check_Stale_Phantoms, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    Site.CaptureRequirement(267, "Each of the rootDSE modify operations is executed by performing an LDAP modify operation with a NULL DN for the object to be modified (indicating the rootDSE) and specifying the name of the modify operation as the attribute to be modified.");
                }

                #endregion

                #region doGarbageCollection

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Split(':')[0].Trim().Equals("doGarbageCollection".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    Site.CaptureRequirement(267, "Each of the rootDSE modify operations is executed by performing an LDAP modify operation with a NULL DN for the object to be modified (indicating the rootDSE) and specifying the name of the modify operation as the attribute to be modified.");
                }

                #endregion

                // dumpDatabase, in RootDseModifyOperations Test Case

                #region fixupInheritance

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("fixupInheritance".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Recalculate_Security_Inheritance, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Recalculate_Security_Inheritance, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    Site.CaptureRequirement(267, "Each of the rootDSE modify operations is executed by performing an LDAP modify operation with a NULL DN for the object to be modified (indicating the rootDSE) and specifying the name of the modify operation as the attribute to be modified.");
                }

                #endregion

                #region invalidateRidPool

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("invalidateRidPool".ToLower(CultureInfo.InvariantCulture)))
                {
                    #region GettingDomainSid
                    NTAccount accountname = new NTAccount(currentWorkingDC.Domain.NetbiosName, testUserName);//NTAccount("Domain Name or Account Name")
                    SecurityIdentifier sid = (SecurityIdentifier)accountname.Translate(typeof(SecurityIdentifier));//get the account SID
                    SecurityIdentifier Domainsid = sid.AccountDomainSid;//get the Domain SID
                    byte[] sidByteArray = new byte[Domainsid.BinaryLength];
                    Domainsid.GetBinaryForm(sidByteArray, 0);
                    #endregion

                    attrValsToBeModified.RemoveAt(0);
                    DirectoryAttributeModification invalidateRIDPool = new DirectoryAttributeModification();
                    invalidateRIDPool.Name = attrToModify.Split(':')[0].Trim();
                    invalidateRIDPool.Add(sidByteArray);
                    invalidateRIDPool.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(invalidateRIDPool);
                    Utilities.RemoveControlAcessRights("CN=RID Manager$,CN=System," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Rid_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=RID Manager$,CN=System," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Change_Rid_Master, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    Utilities.SetAccessRights("CN=RID-Manager-Reference," + schemaNC, testUserName, currentWorkingDC.Domain.NetbiosName, ActiveDirectoryRights.ReadProperty, AccessControlType.Allow);
                }

                #endregion

                #region recalcHierarchy

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("recalcHierarchy".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Recalculate_Hierarchy, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Recalculate_Hierarchy, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region schemaUpdateNow

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("schemaUpdateNow".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=Schema,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Update_Schema_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=Schema,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Update_Schema_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Update_Schema_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Update_Schema_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                // schemaUpgradeInProgress, in AD_DS_RootDSEModify_schema_upgrade_in_progress Test Case

                #region removeLingeringObject

                // see replicateSingleObject, both require the same Central Access Rights

                #endregion

                #region doLinkCleanup

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("doLinkCleanup".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region doOnlineDefrag

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("doOnlineDefrag".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region replicateSingleObject

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("replicateSingleObject".ToLower(CultureInfo.InvariantCulture)) || attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("removeLingeringObject".ToLower(CultureInfo.InvariantCulture)))
                {
                    if (!attrToModify.Contains(";"))
                    {
                        Utilities.RemoveControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.DS_Replication_Synchronize, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.DS_Replication_Synchronize, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    }
                }

                #endregion

                #region updateCachedMemberships

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("updateCachedMemberships".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Refresh_Group_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Refresh_Group_Cache, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                #region doGarbageCollectionPhantomsNow

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("doGarbageCollectionPhantomsNow".ToLower(CultureInfo.InvariantCulture)))
                {
                    Utilities.RemoveControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                    Utilities.SetControlAcessRights("CN=NTDS Settings,CN=" + currentWorkingDC.NetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Do_Garbage_Collection, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                }

                #endregion

                // invalidateGCConnection, in RootDseModifyOperations Test Case

                // renewServerCertificate, in RootDseModifyOperations Test Case

                #region rODCPurgeAccount

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("rODCPurgeAccount".ToLower(CultureInfo.InvariantCulture)))
                {
                    if ((service == ADImplementations.AD_DS) && (currentWorkingDC.OSVersion >= ServerVersion.Win2008))
                    {
                        Utilities.RemoveControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Read_Only_Replication_Secret_Synchronization, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Read_Only_Replication_Secret_Synchronization, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    }
                }

                #endregion

                // runSamUpgradeTasks, in RootDseModifyOperations Test Case

                // sqmRunOnce, in RootDSELds Test Case

                // runProtectAdminGroupsTask

                #region runProtectAdminGroupsTask

                else if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("runProtectAdminGroupsTask".ToLower(CultureInfo.InvariantCulture)))
                {
                    if ((service == ADImplementations.AD_DS) && (currentWorkingDC.OSVersion >= ServerVersion.Win2008R2))
                    {
                        attrValsToBeModified.RemoveAt(0);
                        DirectoryAttributeModification runProtectAdminGroupsTask = new DirectoryAttributeModification();
                        runProtectAdminGroupsTask.Name = attrToModify.Split(':')[0].Trim();
                        runProtectAdminGroupsTask.Add("1");
                        runProtectAdminGroupsTask.Operation = DirectoryAttributeOperation.Add;
                        attrValsToBeModified.Add(runProtectAdminGroupsTask);
                        Utilities.RemoveControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Run_Protect_Admin_Groups_Task, ActiveDirectoryRights.ExtendedRight, AccessControlType.Deny);
                        Utilities.SetControlAcessRights(rootDomainNC, testUserName, currentWorkingDC.Domain.NetbiosName, ControlAccessRight.Run_Protect_Admin_Groups_Task, ActiveDirectoryRights.ExtendedRight, AccessControlType.Allow);
                    }
                }

                #endregion

                // disableOptionalFeature

                // enableOptionalFeature

                // dumpReferences, in RootDseModifyOperations Test Case

                // dumpLinks, in RootDseModifyOperations Test Case

                // schemaUpdateIndicesNow, in RootDseModifyOperations Test Case

                // null

                #endregion

                #region Modify Object

                result = adLdapClient.ModifyObject(
                    objectDN,
                    attrValsToBeModified,
                    null,
                    isWindows);

                // get error code
                if (!isWindows)
                {
                    #region Switch ErrorStatus Non-Windows

                    switch (result)
                    {
                        case "UnwillingToPerform":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                            break;
                        case "Referral":
                            errorStatus = ConstrOnModOpErrs.referral_UnKnownError;
                            break;
                        case "NotAllowedOnRDN":
                            errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
                            break;
                        case "NoSuchObject":
                            errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                            break;
                        case "ConstraintViolation":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_UnKnownError;
                            break;
                        case "NoSuchAttribute":
                            errorStatus = ConstrOnModOpErrs.NoSuchAttribute_UnKnownError;
                            break;
                        case "ObjectClassViolation":
                            errorStatus = ConstrOnModOpErrs.ObjectClassViolation_UnKnownError;
                            break;
                        case "AttributeOrValueExists":
                            errorStatus = ConstrOnModOpErrs.AttributeOrValueExists_UnKnownError;
                            break;
                        case "UndefinedAttributeType":
                            errorStatus = ConstrOnModOpErrs.UndefinedAttributeType_UnKnownError;
                            break;
                        case "ERROR_DS_OBJ_NOT_FOUND":
                            errorStatus = ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND;
                            break;
                        case "Success":
                            errorStatus = ConstrOnModOpErrs.success;
                            break;
                        default:
                            errorStatus = ConstrOnModOpErrs.UnspecifiedError;
                            break;
                    }

                    #endregion
                }
                else
                {
                    #region Switch ErrorStatus Windows

                    switch (result)
                    {
                        case "UnwillingToPerform_ERROR_DS_HIGH_DSA_VERSION":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_HIGH_DSA_VERSION;
                            break;
                        case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                            break;
                        case "UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY;
                            break;
                        case "UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD;
                            break;
                        case "UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER;
                            break;
                        case "UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD;
                            break;
                        case "Referral_ERROR_DS_REFERRAL":
                            errorStatus = ConstrOnModOpErrs.referral_ERROR_DS_REFERRAL;
                            break;
                        case "NotAllowedOnRdn_UnKnownError":
                            errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
                            break;
                        case "UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA":
                            errorStatus = ConstrOnModOpErrs.UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA;
                            break;
                        case "NotAllowedOnRdn_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                            errorStatus = ConstrOnModOpErrs.NotAllowedOnRDN_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                            break;
                        case "NoSuchObject_UnKnownError":
                            errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                            break;
                        case "NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL":
                            errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL;
                            break;
                        case "UnwillingToPerform_ERROR_DS_NOT_SUPPORTED":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED;
                            break;
                        case "NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ":
                            errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ;
                            break;
                        case "ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION;
                            break;
                        case "UnwillingToPerform_UnKnownError":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                            break;
                        case "ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED":
                            errorStatus = ConstrOnModOpErrs.ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED;
                            break;
                        case "ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE;
                            break;
                        case "ConstraintViolation_ERROR_DS_SPN_VALUE_NOT_UNIQUE_IN_FOREST":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_SPN_VALUE_NOT_UNIQUE_IN_FOREST;
                            break;
                        case "ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST;
                            break;
                        case "ConstraintViolation_ERROR_INVALID_PARAMETER":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_INVALID_PARAMETER;
                            break;
                        case "ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION;
                            break;
                        case "ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                            errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                            break;
                        case "AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS":
                            errorStatus = ConstrOnModOpErrs.AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS;
                            break;
                        case "NoSuchAttribute_ERROR_INVALID_PARAMETER":
                            errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER;
                            break;
                        case "ObjectClassViolation_ERROR_DS_ILLEGAL_MOD_OPERATION":
                            errorStatus = ConstrOnModOpErrs.ObjectClassViolation_ERROR_DS_ILLEGAL_MOD_OPERATION;
                            break;
                        case "ObjectClassViolation_ERROR_DS_OBJ_CLASS_NOT_SUBCLASS":
                            errorStatus = ConstrOnModOpErrs.ObjectClassViolation_ERROR_DS_OBJ_CLASS_NOT_SUBCLASS;
                            break;
                        case "OperationsError_ERROR_DS_OBJ_NOT_FOUND":
                            errorStatus = ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND;
                            break;
                        case "NoSuchObject_ERROR_DS_OBJ_NOT_FOUND":
                            errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                            break;
                        case "UnwillingToPerform_ERROR_DS_LOW_DSA_VERSION":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_LOW_DSA_VERSION;
                            break;
                        case "InsufficientAccessRights_UnKnownError":
                            errorStatus = ConstrOnModOpErrs.insufficientAccessRights;
                            break;
                        case "OperationsError_ERROR_DS_GENERIC_ERROR":
                            errorStatus = ConstrOnModOpErrs.OperationsError_ERROR_DS_GENERIC_ERROR;
                            break;
                        case "UnwillingToPerform_ERROR_INVALID_PARAMETER":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_INVALID_PARAMETER;
                            break;
                        case "UnwillingToPerform_ERROR_NOT_FOUND":
                            errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                            break;
                        case "Success_STATUS_SUCCESS":
                            errorStatus = ConstrOnModOpErrs.success;
                            break;
                        default:
                            errorStatus = ConstrOnModOpErrs.UnspecifiedError;
                            break;
                    }

                    #endregion
                }

                #endregion

                #region Requirements on Modify Operations

                if (errorStatus.Equals(ConstrOnModOpErrs.success))
                {
                    #region Verify RootDSE Modify

                    #region MS-AD_LDAP 292, 1144

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("checkPhantoms".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(292, "The requester must have the \"DS-Check-Stale-Phantoms\" control access right on the nTDSDSA object for the DC while performing checkPhantoms rootDSE modify operation.");
                        Site.CaptureRequirement(1144, "checkPhantoms rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3 and Windows Server 2008 AD DS, Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 295, 1146

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("doGarbageCollection".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(295, "The requester must have the \"Do-Garbage-Collection\" control access right on the DC's DSA object, while performing doGarbageCollection rootDSE modify operation.");
                        Site.CaptureRequirement(1146, "doGarbageCollection rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 301, 1148

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("fixupInheritance".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(301, "The requester must have the \"Recalculate-Security-Inheritance\" control access right on the nTDSDSA object for the DC, while performing fixupInheritance rootDSE modify operation.");
                        Site.CaptureRequirement(1148, "fixupInheritance rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 308, 1151

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("recalcHierarchy".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(308, "The requester must have the \"Recalculate-Hierarchy\" control access right on the nTDSDSA object for the DC, while performing recalcHierarchy rootDSE modify operation.");
                        Site.CaptureRequirement(1151, "recalcHierarchy rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3 and Windows Server 2008 AD DS, Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 310, 1153

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("schemaUpdateNow".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(310, "The requester must have the \"Update-Schema-Cache\" control access right on the nTDSDSA object for the DC or on the root of the schema NC, while performing schemaUpdateNow rootDSE modify operation.");
                        Site.CaptureRequirement(1153, "schemaUpdateNow rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 321, 1156

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("doLinkCleanup".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(321, "The requester must have the \"Do-Garbage-Collection\" control access right on the nTDSDSA object for the DC, while performing doLinkCleanup rootDSE modify operation.");
                        Site.CaptureRequirement(1156, "doLinkCleanup rootDSE attribute is supported by Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 324, 325, 1158

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("doOnlineDefrag".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(324, "The requester must have the \"Do-Garbage-Collection\" control access right on the nTDSDSA object for the DC, while performing doOnlineDefrag rootDSE modify operation.");
                        Site.CaptureRequirement(325, "Client performs a doOnlineDefrag operation by performing an LDAP modify operation with a NULL DN for the object and specifying \"doOnlineDefrag\" as the attribute to be modified.");
                        Site.CaptureRequirement(1158, "doOnlineDefrag rootDSE attribute is supported by Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 327, 1160

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("replicateSingleObject".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(327, "The requester must have the \"DS-Replication-Synchronize\" control access right on the root of the NC that contains the object to be replicated, while performing replicateSingleObject rootDSE modify operation.");
                        Site.CaptureRequirement(1160, "replicateSingleObject rootDSE attribute is supported by Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 324, 330, 1162

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("updateCachedMemberships".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(324, "The requester must have the \"Do-Garbage-Collection\" control access right on the nTDSDSA object for the DC, while performing doOnlineDefrag rootDSE modify operation.");
                        Site.CaptureRequirement(330, "The requester must have the \"Refresh-Group-Cache\" control access right on the nTDSDSA object for the DC, while performing updateCachedMemberships rootDSE modify operation.");
                        Site.CaptureRequirement(1162, "updateCachedMemberships rootDSE attribute is supported by Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 332, 1164

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("doGarbageCollectionPhantomsNow".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(332, "The requester must have the \"Refresh-Group-Cache\" control access right on the nTDSDSA object for the DC, while performing doGarbageCollectionPhantomsNow rootDSE modify operation.");
                        Site.CaptureRequirement(1164, "doGarbageCollection PhantomsNow rootDSE attribute is supported by Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 340, 1170

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2008
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("rODCPurgeAccount".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(340, "The requester must have the \"Read-Only-Replication-Secret-Synchronization\" control access right on the root of the default NC, while performing rODCPurgeAccount rootDSE modify operation.");
                        Site.CaptureRequirement(1170, "rODCPurgeAccount rootDSE attribute is supported by Windows Server 2008 AD DS and Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 269, 271, 1131, 1134

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeDomainMaster".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(269, "The requester must have the \"Change-Domain-Master\" control access right on the Partitions container in the config NC for becomeDomainMaster rootDSE modify operation to succeed.");
                        Site.CaptureRequirement(271, "Client performs a becomeDomainMaster operation by performing an LDAP modify operation with a NULL DN for the object and specifying \"becomeDomainMaster\" as the attribute to be modified.");
                        Site.CaptureRequirement(1131, "becomeDomainMaster and becomeInfrastructureMaster rootDSE attributes are write-only.");
                        Site.CaptureRequirement(1134, "becomeDomainMaster rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 273, 275, 1135

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeInfrastructureMaster".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(273, "The requester must have the \"Change-Infrastructure-Master\" control access right on the Infrastructure container in the domain NC-replica, while performing becomeInfrastructureMaster rootDSE modify operation.");
                        Site.CaptureRequirement(275, "Client performs a becomeInfrastructureMaster operation by performing an LDAP modify operation with a NULL DN for the object and specifying \"becomeInfrastructureMaster\" as the attribute to be modified.");
                        Site.CaptureRequirement(1135, "becomeInfrastructureMaster rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3 and Windows Server 2008 AD DS, Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 284, 1141, 1178

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeRidMaster".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(284, "While performing becomeRidMaster rootDSE modify Operations, the requester must have the \"Change-RID-Master\" control access right on the RID Manager object, which is the object referenced by the rIDManagerReference attribute located on the root of the domain NC.");
                        Site.CaptureRequirement(1141, "becomeRidMaster rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3 and Windows Server 2008 AD DS, Windows Server 2008 R2 AD DS.");
                        Site.CaptureRequirement(1178, "While performing becomeRidMaster rootDSE modify operation, the requester must have the read permission on the rIDManagerReference attribute.");
                    }

                    #endregion

                    #region MS-AD_LDAP 305, 1149

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("invalidateRidPool".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(305, "The requester must have the read permission on the rIDManagerReference attribute, while performing invalidateRidPool rootDSE modify operation.");
                        Site.CaptureRequirement(1149, "invalidateRidPool rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3 and Windows Server 2008 AD DS, Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 288, 1143

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomeSchemaMaster".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(288, "The requester must have the \"Change-Schema-Master\" control access right on the root of the schema NC-replica, while performing becomeSchemaMaster rootDSE modify operation.");
                        Site.CaptureRequirement(1143, "becomeSchemaMaster rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 1137

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomePdc".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(1137, "becomePdc rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3 and Windows Server 2008 AD DS, Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 1139

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("becomePdcWithCheckPoint".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(1139, "becomePdcWithCheckPoint rootDSE attribute is supported by Windows 2000, Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3 and Windows Server 2008 AD DS, Windows Server 2008 R2 AD DS.");
                    }

                    #endregion

                    #region MS-AD_LDAP 1154

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("removeLingeringObject".ToLower(CultureInfo.InvariantCulture)))
                    {
                        Site.CaptureRequirement(1154, "removeLingeringObject rootDSE attribute is supported by Windows 2000 SP1, Windows Server 2003, Windows Server 2003 SP3, Windows Server 2008 AD DS and Windows Server 2008 AD LDS, Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                    }

                    #endregion

                    #endregion

                    #region Verify Dynamic Object TTL

                    if (objectDN != null && objectDN.Equals(testUser6DNForDs, StringComparison.InvariantCultureIgnoreCase))
                    {
                        int entryTTLValue = int.Parse(Utilities.GetAttributeFromEntry(objectDN, "entryTTL", currentWorkingDC.FQDN, currentPort).ToString(), CultureInfo.InvariantCulture);
                        VerifyEntryTTL(entryTTLValue, dynamicObjectMinTTL, dynamicObjectTTLModify);
                    }

                    #endregion

                    #region Verify ObjectClass Holes

                    if (objectClass.ToLower(CultureInfo.InvariantCulture).Contains("inetOrgPerson".ToLower(CultureInfo.InvariantCulture)))
                    {
                        result = adLdapClient.SearchObject(
                            objectDN,
                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                            "(objectClass=user)",
                            null,
                            null,
                            out searchResponse);
                        Site.Assert.IsTrue(result.ToLower().Contains("success"),
                            string.Format("Search operation on {0} should be successful, actual result: {1}", objectDN, result));
                        if (searchResponse != null)
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectClass");
                                Site.CaptureRequirementIfIsTrue(
                                    (searchAttrVals.Length > 1),
                                    750,
                                    @"[If the DC functional level is DS_BEHAVIOR_WIN2003 or greater, then originating updates of the objectClass attribute are permitted]
                                            If the set of object classes specified by an update contains 'holes' the server fills the 'holes' during the update.");
                            }
                        }
                    }

                    #endregion

                    #region Verify CrossRef objects modify are only allowed in domain naming FSMO DC

                    if (objectClass.ToLower(CultureInfo.InvariantCulture).Contains("crossRef"))
                    {
                        //Checks for Domain Naming FSMO.
                        Site.CaptureRequirementIfAreEqual<string>(
                            Utilities.GetFsmo("CN=Partitions," + configurationNC),
                            ("CN=" + currentWorkingDC.NetbiosName),
                            687,
                            @"During the modify operation, changes to objects in the Partitions container (class crossRef) are only allowed when 
                                    the DC is the domain naming FSMO.");
                    }

                    #endregion
                }
                else
                {
                    #region Verify RootDSE Modify Errors

                    #region MS-AD_LDAP 1192

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("replicateSingleObject".ToLower(CultureInfo.InvariantCulture))
                        && errorStatus.Equals(ConstrOnModOpErrs.OperationsError_ERROR_DS_GENERIC_ERROR))
                    {
                        Site.CaptureRequirement(1192, "If the DN specified in replicateSingleObject operation is not in the specified format, the server rejects the request with the error operationsError.");
                    }

                    #endregion

                    #region MS-AD_LDAP 344

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2008
                        && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("rODCPurgeAccount".ToLower(CultureInfo.InvariantCulture))
                        && !isRODC
                        && errorStatus.Equals(ConstrOnModOpErrs.OperationsError_ERROR_DS_GENERIC_ERROR))
                    {
                        Site.CaptureRequirement(344, "While performing rODCPurgeAccountrootDSE modify operation, if the operation is sent to a DC that is not an RODC, then the error operationsError / ERROR_DS_GENERIC_ERROR is returned.");
                    }

                    #endregion

                    #endregion
                }

                #endregion
            }

            #endregion

            #region Return values for specific attributes

            //Check to handle the error code returned when tried to modify "defaultNamingContext" and "currentTime" attributes as the corresponding windows error code is not provided by the TD.
            if (result.ToLower(CultureInfo.InvariantCulture).Contains("UnwillingToPerform".ToLower(CultureInfo.InvariantCulture))
                && (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("defaultNamingContext".ToLower(CultureInfo.InvariantCulture))
                || attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("currentTime".ToLower(CultureInfo.InvariantCulture))))
            {
                errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
            }

            if (result.ToLower(CultureInfo.InvariantCulture).Contains("NotAllowedOnRdn".ToLower(CultureInfo.InvariantCulture))
                && attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("users+user67"))
            {
                errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
            }
            //Check for RODC ErrorStatus
            if (isRODC && result.ToLower(CultureInfo.InvariantCulture).Contains("NoSuchObject".ToLower(CultureInfo.InvariantCulture)))
            {
                errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
            }
            if ((errorStatus != ConstrOnModOpErrs.success)
                && (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("memberOf".ToLower(CultureInfo.InvariantCulture))
                || attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("Aggregate".ToLower(CultureInfo.InvariantCulture))))
            {
                errorStatus = ConstrOnModOpErrs.UnspecifiedError;
            }
            if (objectDN != null)
            {
                if ((errorStatus != ConstrOnModOpErrs.success)
                    && ((objectDN.ToLower(CultureInfo.InvariantCulture).Contains("CN=Reps-From".ToLower(CultureInfo.InvariantCulture)))
                    || objectDN.ToLower(CultureInfo.InvariantCulture).Contains("CN=Given-Name".ToLower(CultureInfo.InvariantCulture))
                    || objectDN.ToLower(CultureInfo.InvariantCulture).Contains("CN=Site-Link".ToLower(CultureInfo.InvariantCulture))
                    || objectDN.ToLower(CultureInfo.InvariantCulture).Contains("CN=top".ToLower(CultureInfo.InvariantCulture))))
                {
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                }
            }

            #endregion

            Site.Log.Add(LogEntryKind.Debug, "[ModifyOperation]: Exiting");
        }

        /// <summary>
        /// Action describing the behavior of ModifyOperation
        /// </summary>
        /// <param name="attribVal">Variable that contains attributes to be modified mapped to the list of existing attributes on a particular object</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void ModifyRecycleBin(
            Map<string, Sequence<string>> attribVal,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus)
        {
            #region Variables

            // set default error message
            errorStatus = ConstrOnModOpErrs.UnspecifiedError;

            DirectoryAttributeModification modifyAttr = new DirectoryAttributeModification();
            List<DirectoryAttributeModification> attrValsToBeModified = new List<DirectoryAttributeModification>();

            string attrToModify = string.Empty;
            DirectoryEntry partitionsEntry = null;
            bool isOptionalFeatureEnabled = false;

            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            string[] searchAttrVals = null;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                if (isRODC)
                {
                    SetConnectAndBind(service, RODCNetbiosName);
                }
                else
                {
                    SetConnectAndBind(service, PDCNetbiosName);
                }
            }

            #endregion

            #region Modify Recycle Bin Operation

            foreach (string key in attribVal.Keys)
            {
                #region Finialize the attribute to be modified

                // Assign optional feature type
                if (key.ToLower(CultureInfo.InvariantCulture).Contains("enable"))
                {
                    attrToModify = "enableOptionalFeature";
                }
                else
                {
                    attrToModify = "disableOptionalFeature";
                }

                #endregion

                #region Update msDS-Behavior-Version on Partitions NC

                try
                {
                    // Set test user to have Manage-Optional-Features access rights on partitions object
                    Utilities.SetControlAcessRights(
                        "CN=Partitions," + configurationNC,
                        testUserName,
                        currentWorkingDC.Domain.NetbiosName,
                        ControlAccessRight.Manage_Optional_Features,
                        ActiveDirectoryRights.GenericAll,
                        AccessControlType.Allow);

                    partitionsEntry = new DirectoryEntry(
                        "LDAP://" + currentWorkingDC.FQDN + ":" + currentPort + "/" + "CN=Partitions," + configurationNC,
                        testUserName,
                        testUserPwd,
                        AuthenticationTypes.Secure);
                    // Modify msDS-Behavior-Version to the newest version
                    switch (currentWorkingDC.OSVersion)
                    {
                        case ServerVersion.NonWin:
                        case ServerVersion.Win2016:
                            partitionsEntry.Properties["msDS-Behavior-Version"].Value = 7;
                            break;
                        case ServerVersion.Win2012R2:
                            partitionsEntry.Properties["msDS-Behavior-Version"].Value = 6;
                            break;
                        case ServerVersion.Win2012:
                            partitionsEntry.Properties["msDS-Behavior-Version"].Value = 5;
                            break;
                        case ServerVersion.Win2008R2:
                            partitionsEntry.Properties["msDS-Behavior-Version"].Value = 4;
                            break;
                        case ServerVersion.Win2008:
                            partitionsEntry.Properties["msDS-Behavior-Version"].Value = 3;
                            break;
                        case ServerVersion.Win2003:
                            partitionsEntry.Properties["msDS-Behavior-Version"].Value = 2;
                            break;
                        default:
                            Site.Assert.Fail("Unsupported OS Version by test suite, {0}", Enum.GetName(typeof(ServerVersion), currentWorkingDC.OSVersion));
                            break;
                    }
                    partitionsEntry.CommitChanges();
                    partitionsEntry.Close();
                }
                catch (DirectoryServicesCOMException comExeception)
                {
                    Site.Log.Add(LogEntryKind.Debug, "Modify msDS-Behavior-Version on {0} failed: {1}", partitionsEntry.Path, comExeception.ExtendedErrorMessage.ToString());
                }

                #endregion

                #region Modify Optional Feature with Invalid GUID for control access rights

                if (key.ToLower(CultureInfo.InvariantCulture).Contains("invalidGUID".ToLower(CultureInfo.InvariantCulture)))
                {
                    modifyAttr.Name = attrToModify;
                    modifyAttr.Add("CN=Partitions," + configurationNC + ":" + INVALID_RECYCLE_BIN_GUID);
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(modifyAttr);
                }

                #endregion

                #region Modify Optional Feature with Invalid DN for control access rights

                else if (key.ToLower(CultureInfo.InvariantCulture).Contains("invalidDN".ToLower(CultureInfo.InvariantCulture)))
                {
                    // use CN=Configuration,<defaultNC> as invalid distinguished name
                    modifyAttr.Name = attrToModify;
                    modifyAttr.Add(configurationNC + ":" + RECYCLE_BIN_GUID);
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(modifyAttr);
                }

                #endregion

                #region Modify Optional Feature with Invalid DN for control access rights

                else if (key.ToLower(CultureInfo.InvariantCulture).Contains("invalidScope".ToLower(CultureInfo.InvariantCulture)))
                {
                    // use CN=SDC,CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,<defaultNC> as invalid scope
                    modifyAttr.Name = attrToModify;
                    modifyAttr.Add("CN=" + SDCNetbiosName + ',' + serversContainerDNForDs + ":" + RECYCLE_BIN_GUID);
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(modifyAttr);
                }

                #endregion

                // DC not hosting the Domain Naming Master role
                #region Modify Opertional Feature when DC is Not PNM

                else if (key.ToLower(CultureInfo.InvariantCulture).Contains("DCNotPNM".ToLower(CultureInfo.InvariantCulture)))
                {
                    modifyAttr.Name = attrToModify;
                    modifyAttr.Add(rootDomainNC + ":" + RECYCLE_BIN_GUID);
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(modifyAttr);
                }

                #endregion

                #region Modify Operational Feature: recycle bin

                else if (key.Equals("enableRecycleBin: true") || key.Equals("disableRecycleBin: true"))
                {
                    modifyAttr.Name = attrToModify;
                    modifyAttr.Add("CN=Partitions," + configurationNC + ":" + RECYCLE_BIN_GUID);
                    modifyAttr.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(modifyAttr);
                }

                #endregion

                result = adLdapClient.ModifyObject(
                    null,
                    attrValsToBeModified,
                    null,
                    isWindows);
            }

            if (!isWindows)
            {
                #region Switch ErrorStatus Non-Windows

                switch (result)
                {
                    case "UnwillingToPerform":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                        break;
                    case "Referral":
                        errorStatus = ConstrOnModOpErrs.referral_UnKnownError;
                        break;
                    case "NotAllowedOnRDN":
                        errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
                        break;
                    case "NoSuchObject":
                        errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                        break;
                    case "ConstraintViolation":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_UnKnownError;
                        break;
                    case "NoSuchAttribute":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_UnKnownError;
                        break;
                    case "ObjectClassViolation":
                        errorStatus = ConstrOnModOpErrs.ObjectClassViolation_UnKnownError;
                        break;
                    case "AttributeOrValueExists":
                        errorStatus = ConstrOnModOpErrs.AttributeOrValueExists_UnKnownError;
                        break;
                    case "UndefinedAttributeType":
                        errorStatus = ConstrOnModOpErrs.UndefinedAttributeType_UnKnownError;
                        break;
                    case "Success":
                        errorStatus = ConstrOnModOpErrs.success;
                        break;
                    default:
                        errorStatus = ConstrOnModOpErrs.UnspecifiedError;
                        break;
                }

                #endregion
            }
            else
            {
                #region Switch ErrorStatus Windows

                switch (result)
                {
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                        break;
                    case "UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD;
                        break;
                    case "UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER;
                        break;
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD;
                        break;
                    case "Referral_ERROR_DS_REFERRAL":
                        errorStatus = ConstrOnModOpErrs.referral_ERROR_DS_REFERRAL;
                        break;
                    case "NotAllowedOnRdn_UnKnownError":
                        errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
                        break;
                    case "UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA":
                        errorStatus = ConstrOnModOpErrs.UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA;
                        break;
                    case "NotAllowedOnRdn_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                        errorStatus = ConstrOnModOpErrs.NotAllowedOnRDN_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                        break;
                    case "NoSuchObject_UnKnownError":
                        errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                        break;
                    case "NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL;
                        break;
                    case "UnwillingToPerform_ERROR_DS_NOT_SUPPORTED":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED;
                        break;
                    case "NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ;
                        break;
                    case "ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION;
                        break;
                    case "UnwillingToPerform_UnKnownError":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                        break;
                    case "ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED":
                        errorStatus = ConstrOnModOpErrs.ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED;
                        break;
                    case "ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE;
                        break;
                    case "ConstraintViolation_ERROR_INVALID_PARAMETER":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_INVALID_PARAMETER;
                        break;
                    case "ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION;
                        break;
                    case "ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                        break;
                    case "AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS":
                        errorStatus = ConstrOnModOpErrs.AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS;
                        break;
                    case "NoSuchAttribute_ERROR_INVALID_PARAMETER":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER;
                        break;
                    case "UnwillingToPerform_ERROR_INVALID_PARAMETER":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_INVALID_PARAMETER;
                        break;
                    case "OperationsError_ERROR_DS_OBJ_NOT_FOUND":
                        errorStatus = ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND;
                        break;
                    case "Success_STATUS_SUCCESS":
                        errorStatus = ConstrOnModOpErrs.success;
                        break;
                    default:
                        errorStatus = ConstrOnModOpErrs.UnspecifiedError;
                        break;
                }

                #endregion
            }


            #endregion

            #region Requirements on Modify Recycle Bin

            isOptionalFeatureEnabled = Utilities.IsOptionalFeatureEnabled(
                    forestScopePartialDN + ',' + configurationNC,
                    recycleBinPartialDN + ',' + configurationNC);

            #region dcFunctionalLevel

            if (attrToModify.ToLower(CultureInfo.InvariantCulture).Contains("enableOptionalFeature".ToLower(CultureInfo.InvariantCulture)))
            {
                Site.CaptureRequirementIfIsTrue(
                    isOptionalFeatureEnabled,
                    4467,
                    @"[Optional Features] Optional features are enabled in a scope via the enableOptionalFeature rootDSE modify operation
                    (see section 3.1.1.3.3.26).");
                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008R2)
                {
                    Site.CaptureRequirementIfIsTrue(
                        isOptionalFeatureEnabled,
                        4197,
                        @"enableOptionalFeature rootDSE attribute is supported by 
                        Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS or later.");
                }
            }

            #endregion

            #region enabled scope

            if (service == ADImplementations.AD_DS)
            {
                if (isOptionalFeatureEnabled)
                {
                    #region check if forest wide enabled

                    bool isForestScope = false;
                    result = adLdapClient.SearchObject(
                        forestScopePartialDN + ',' + configurationNC,
                        System.DirectoryServices.Protocols.SearchScope.Base,
                        "(objectClass=*)",
                        null,
                        null,
                        out searchResponse,
                        isWindows);
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "msDS-EnabledFeature");
                            if (searchAttrVals.Exists(x => x.Contains(recycleBinPartialDN + ',' + configurationNC)))
                            {
                                isForestScope = true;
                            }
                        }
                    }

                    #endregion

                    #region check if server wide enabled

                    bool isServerScope = false;
                    result = adLdapClient.SearchObject(
                        string.Format("CN=NTDS Settings,CN={0},{1}", PDCNetbiosName, serversContainerDNForDs),
                        System.DirectoryServices.Protocols.SearchScope.Base,
                        "(objectClass=*)",
                        null,
                        null,
                        out searchResponse,
                        isWindows);
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "msDS-EnabledFeature");
                            if (searchAttrVals.Exists(x => x.Contains(recycleBinPartialDN + ',' + configurationNC)))
                            {
                                isServerScope = true;
                            }
                        }
                    }

                    #endregion

                    Site.CaptureRequirementIfIsTrue(
                        isForestScope && isServerScope,
                        4498,
                        @"[Recycle Bin Optional Feature] When the rootDSE modify operation enableOptionalFeature (section 3.1.1.3.3.26)
                        is executed on a given DC to enable the Recycle Bin optional feature, in addition to being added to the list 
                        of forest-wide enabled features, the optional feature is also added to the list of server-wide enabled features 
                        (see section 3.1.1.8).");
                }
            }

            #endregion

            #endregion
        }

        /// <summary>
        /// Action to restore the deleted object
        /// </summary>
        /// <param name="restoreObjectDN">Variable that specify the distinguished name of the deleted object</param>
        /// <param name="newDN">Variable that specify the distinguished name of the new object</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public void RestoreDeletedObject(
            string restoreObjectDN,
            string newDN,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus)
        {
            #region Variables

            // Set default error message
            errorStatus = ConstrOnModOpErrs.UnspecifiedError;

            DirectoryAttributeModification modifyAttr1 = new DirectoryAttributeModification();
            DirectoryAttributeModification modifyAttr2 = new DirectoryAttributeModification();
            List<DirectoryAttributeModification> attrValsToBeModified = new List<DirectoryAttributeModification>();

            string objectDN = string.Empty;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                if (isRODC)
                {
                    SetConnectAndBind(service, RODCNetbiosName);
                }
                else
                {
                    SetConnectAndBind(service, PDCNetbiosName);
                }
            }

            #endregion

            #region Finalize object distinguished name

            objectDN = restoreObjectDN.Replace("DC=adts88", rootDomainNC);

            #endregion

            #region Undelete Modify Object

            result = adLdapClient.UndeleteModifyObject(
                objectDN,
                newDN,
                null,
                isWindows);

            if (!isWindows)
            {
                #region Switch ErrorStatus Non-Windows

                switch (result)
                {
                    case "UnwillingToPerform":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                        break;
                    case "Referral":
                        errorStatus = ConstrOnModOpErrs.referral_UnKnownError;
                        break;
                    case "NotAllowedOnRDN":
                        errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
                        break;
                    case "NoSuchObject":
                        errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                        break;
                    case "ConstraintViolation":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_UnKnownError;
                        break;
                    case "NoSuchAttribute":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_UnKnownError;
                        break;
                    case "ObjectClassViolation":
                        errorStatus = ConstrOnModOpErrs.ObjectClassViolation_UnKnownError;
                        break;
                    case "AttributeOrValueExists":
                        errorStatus = ConstrOnModOpErrs.AttributeOrValueExists_UnKnownError;
                        break;
                    case "UndefinedAttributeType":
                        errorStatus = ConstrOnModOpErrs.UndefinedAttributeType_UnKnownError;
                        break;
                    case "Success":
                        errorStatus = ConstrOnModOpErrs.success;
                        break;
                    default:
                        errorStatus = ConstrOnModOpErrs.UnspecifiedError;
                        break;
                }

                #endregion
            }
            else
            {
                #region Switch ErrorStatus Windows

                switch (result)
                {
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                        break;
                    case "UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD;
                        break;
                    case "UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER;
                        break;
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD;
                        break;
                    case "Referral_ERROR_DS_REFERRAL":
                        errorStatus = ConstrOnModOpErrs.referral_ERROR_DS_REFERRAL;
                        break;
                    case "NotAllowedOnRdn_UnKnownError":
                        errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
                        break;
                    case "UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA":
                        errorStatus = ConstrOnModOpErrs.UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA;
                        break;
                    case "NotAllowedOnRdn_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                        errorStatus = ConstrOnModOpErrs.NotAllowedOnRDN_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                        break;
                    case "NoSuchObject_UnKnownError":
                        errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                        break;
                    case "NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL;
                        break;
                    case "UnwillingToPerform_ERROR_DS_NOT_SUPPORTED":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED;
                        break;
                    case "NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ;
                        break;
                    case "ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION;
                        break;
                    case "UnwillingToPerform_UnKnownError":
                        errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                        break;
                    case "ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED":
                        errorStatus = ConstrOnModOpErrs.ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED;
                        break;
                    case "ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE;
                        break;
                    case "ConstraintViolation_ERROR_INVALID_PARAMETER":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_INVALID_PARAMETER;
                        break;
                    case "ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION;
                        break;
                    case "ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                        errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                        break;
                    case "AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS":
                        errorStatus = ConstrOnModOpErrs.AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS;
                        break;
                    case "NoSuchAttribute_ERROR_INVALID_PARAMETER":
                        errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER;
                        break;
                    case "Success_STATUS_SUCCESS":
                        errorStatus = ConstrOnModOpErrs.success;
                        break;
                    case "Success":
                        errorStatus = ConstrOnModOpErrs.success;
                        break;
                    default:
                        errorStatus = ConstrOnModOpErrs.UnspecifiedError;
                        break;
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// Action to modify the well-known objects
        /// </summary>
        /// <param name="operationType">Variable that specify the operation type for the WKO</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        // Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        // Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void ModifyWellKnownObject(
            string operationType,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus)
        {
            #region Variables

            errorStatus = ConstrOnModOpErrs.UnspecifiedError;

            List<DirectoryAttribute> attrValsForNewObject = new List<DirectoryAttribute>();
            List<DirectoryAttributeModification> attrValsToBeModified = new List<DirectoryAttributeModification>();

            // read from rootDomainNC with domainDNS object class
            string domainFunctionalLevel = string.Empty;

            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            string[] searchAttrVals = null;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                if (isRODC)
                {
                    SetConnectAndBind(service, RODCNetbiosName);
                }
                else
                {
                    SetConnectAndBind(service, PDCNetbiosName);
                }
            }

            #endregion

            #region Create Users Container under Root Domain Naming Context

            string newUserContainer = "CN=Users1Container" + "," + rootDomainNC;
            if (!Utilities.IsObjectExist(newUserContainer, currentWorkingDC.FQDN, currentPort))
            {
                attrValsForNewObject = new List<DirectoryAttribute>();
                attrValsForNewObject.Add(new DirectoryAttribute("objectClass: device"));
                result = adLdapClient.AddObject(
                    newUserContainer,
                    attrValsForNewObject,
                    null,
                    isWindows);
                Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("Add operation for {0} should be successful, actual result: {1}", newUserContainer, result));
            }

            #endregion

            #region Update the msDS-Behavior-Version to the newest version

            DirectoryEntry partitionsEntry = new DirectoryEntry(
                "LDAP://" + currentWorkingDC.FQDN + ":" + currentPort + "/" + rootDomainNC,
                testUserName,
                testUserPwd,
                AuthenticationTypes.Secure);
            // Modify msDS-Behavior-Version to the newest version
            switch (currentWorkingDC.OSVersion)
            {
                case ServerVersion.NonWin:
                case ServerVersion.Win2016:
                    partitionsEntry.Properties["msDS-Behavior-Version"].Value = 7;
                    break;
                case ServerVersion.Win2012R2:
                    partitionsEntry.Properties["msDS-Behavior-Version"].Value = 6;
                    break;
                case ServerVersion.Win2012:
                    partitionsEntry.Properties["msDS-Behavior-Version"].Value = 5;
                    break;
                case ServerVersion.Win2008R2:
                    partitionsEntry.Properties["msDS-Behavior-Version"].Value = 4;
                    break;
                case ServerVersion.Win2008:
                    partitionsEntry.Properties["msDS-Behavior-Version"].Value = 3;
                    break;
                case ServerVersion.Win2003:
                    partitionsEntry.Properties["msDS-Behavior-Version"].Value = 2;
                    break;
                default:
                    Site.Assert.Fail("Unsupported OS Version by test suite, {0}", Enum.GetName(typeof(ServerVersion), currentWorkingDC.OSVersion));
                    break;
            }
            partitionsEntry.CommitChanges();
            partitionsEntry.Close();

            #endregion

            #region Check if msDS-Behavior-Version updated and update global parameter domainFunctionalLevel

            result = adLdapClient.SearchObject(
                rootDomainNC,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "(objectClass=domainDNS)",
                new string[] { "msDS-Behavior-Version" },
                null,
                out searchResponse);
            Site.Assert.IsTrue(result.ToLower().Contains("success"),
                string.Format("Search operation on {0} should be successful, actual result: {1}", rootDomainNC, result));
            Site.Log.Add(LogEntryKind.Debug, string.Format("{0} {1} found.", searchResponse.Count, searchResponse.Count > 1 ? "entries were" : "entry was"));
            if (searchResponse != null)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "msDS-Behavior-Version");
                    domainFunctionalLevel = searchAttrVals[0];
                    Site.Assert.AreEqual<string>(
                        ((uint)DomainFunctionLevel).ToString(),
                        domainFunctionalLevel,
                        string.Format("msDS-Behavior-Version attribute from domainDNS object should be equal to domainFunctionality attribute from rootDSE."));
                }
            }

            #endregion

            #region Modify WellKnownObject Operation

            // domain functional level is DS_BEHAVIOR_WIN2008 or greater

            if ((DomainFunctionLevel)uint.Parse(domainFunctionalLevel) >= DomainFunctionLevel.DS_BEHAVIOR_WIN2008)
            {
                // WellKnownObject: invalidNewValue
                if (operationType.Equals("wellKnowObject: invalidNewValue"))
                {
                    #region wellKnowObject: invalidNewValue

                    attrValsToBeModified = new List<DirectoryAttributeModification>();
                    DirectoryAttributeModification wellKnownOldUserObject = new DirectoryAttributeModification();
                    wellKnownOldUserObject.Name = "wellKnownObjects";
                    wellKnownOldUserObject.Add("B:32:A9D1CA15768811D1ADED00C04FD8D5CD:CN=Users," + rootDomainNC);
                    wellKnownOldUserObject.Operation = DirectoryAttributeOperation.Delete;
                    DirectoryAttributeModification wellKnownNewUserObject = new DirectoryAttributeModification();
                    wellKnownNewUserObject.Name = "wellKnownObjects";
                    wellKnownNewUserObject.Add("B:32:A9D1CA15768811D1ADED00C04FD8D5CD:CN=InvalidUsersContainer," + rootDomainNC);
                    wellKnownNewUserObject.Operation = DirectoryAttributeOperation.Add;
                    attrValsToBeModified.Add(wellKnownOldUserObject);
                    attrValsToBeModified.Add(wellKnownNewUserObject);

                    #endregion
                }
                else if (operationType.Equals("wellKnowObject: invalidRemoveValue"))
                {
                    #region wellKnowObject: invalidRemoveValue

                    attrValsToBeModified = new List<DirectoryAttributeModification>();
                    DirectoryAttributeModification wellKnownInvalidUserObject = new DirectoryAttributeModification();
                    wellKnownInvalidUserObject.Name = "wellKnownObjects";
                    // A valid GUID should be A9D1CA15768811D1ADED00C04FD8D5CD, last digit should be 'D' => instead of 'E'
                    wellKnownInvalidUserObject.Add("B:32:A9D1CA15768811D1ADED00C04FD8D5CE:CN=Users," + rootDomainNC);
                    wellKnownInvalidUserObject.Operation = DirectoryAttributeOperation.Delete;
                    attrValsToBeModified.Add(wellKnownInvalidUserObject);

                    #endregion
                }

                result = adLdapClient.ModifyObject(rootDomainNC, attrValsToBeModified, null);
            }
            else
            {
                Site.Log.Add(LogEntryKind.Debug, string.Format("msDS-Behavior-Version read from domainDNS object is not equal to or greater than DS_BEHAVIOR_WIN2008."));
                errorStatus = ConstrOnModOpErrs.success;
            }

            #region Switch ErrorStatus Windows

            switch (result)
            {
                case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                    break;
                case "UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY;
                    break;
                case "UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD;
                    break;
                case "UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER;
                    break;
                case "UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD;
                    break;
                case "Referral_ERROR_DS_REFERRAL":
                    errorStatus = ConstrOnModOpErrs.referral_ERROR_DS_REFERRAL;
                    break;
                case "NotAllowedOnRdn_UnKnownError":
                    errorStatus = ConstrOnModOpErrs.NotAllowedOnRdn_UnKnownError;
                    break;
                case "UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA":
                    errorStatus = ConstrOnModOpErrs.UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA;
                    break;
                case "NotAllowedOnRdn_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                    errorStatus = ConstrOnModOpErrs.NotAllowedOnRDN_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                    break;
                case "NoSuchObject_UnKnownError":
                    errorStatus = ConstrOnModOpErrs.NoSuchObject_UnKnownError;
                    break;
                case "NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL":
                    errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL;
                    break;
                case "UnwillingToPerform_ERROR_DS_NOT_SUPPORTED":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED;
                    break;
                case "NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ":
                    errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ;
                    break;
                case "ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION":
                    errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION;
                    break;
                case "UnwillingToPerform_UnKnownError":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_UnKnownError;
                    break;
                case "ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED":
                    errorStatus = ConstrOnModOpErrs.ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED;
                    break;
                case "ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE":
                    errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE;
                    break;
                case "ConstraintViolation_ERROR_INVALID_PARAMETER":
                    errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_INVALID_PARAMETER;
                    break;
                case "ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION":
                    errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION;
                    break;
                case "ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY":
                    errorStatus = ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY;
                    break;
                case "AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS":
                    errorStatus = ConstrOnModOpErrs.AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS;
                    break;
                case "NoSuchAttribute_ERROR_INVALID_PARAMETER":
                    errorStatus = ConstrOnModOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER;
                    break;
                case "UnwillingToPerform_ERROR_DS_ILLEGAL_SUPERIOR":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_SUPERIOR;
                    break;
                case "UnwillingToPerform_ERROR_DS_UNWILLING_TO_PERFORM":
                    errorStatus = ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_UNWILLING_TO_PERFORM;
                    break;
                case "Success_STATUS_SUCCESS":
                    errorStatus = ConstrOnModOpErrs.success;
                    break;
                default:
                    errorStatus = ConstrOnModOpErrs.UnspecifiedError;
                    break;
            }

            #endregion

            #endregion
        }

        /// <summary>
        /// Set AD object life time
        /// </summary>
        /// <param name="deletedObjectLifetimeVal">The value of msDS-DeletedObjectLifetime</param>
        /// <param name="tombstoneLifetimeVal">The value of tombstoneLifetime</param>
        public void SetADObjectLifeTime(
            int deletedObjectLifetimeVal,
            int tombstoneLifetimeVal)
        {
            #region Connect and Bind

            // Set the connection
            if (isConnected == false)
            {
                SetConnectAndBind(ADImplementations.AD_DS, PDCNetbiosName);
            }

            #endregion

            #region Modify Operation

            string objectDN = nTDSServiceDNForDs;

            // Construct the modify attributes
            List<DirectoryAttributeModification> modAttrs = new List<DirectoryAttributeModification>();
            // Set deleted object life time to be 1 day (shortest life time)
            DirectoryAttributeModification deletedObjectLifetime = new DirectoryAttributeModification();
            deletedObjectLifetime.Name = "msDS-DeletedObjectLifetime";
            deletedObjectLifetime.Add(deletedObjectLifetimeVal.ToString());
            deletedObjectLifetime.Operation = DirectoryAttributeOperation.Replace;
            modAttrs.Add(deletedObjectLifetime);
            // Setl tombstone life time to be 1 day (shortest life time)
            DirectoryAttributeModification tombstoneLifetime = new DirectoryAttributeModification();
            tombstoneLifetime.Name = "tombstoneLifetime";
            tombstoneLifetime.Add(tombstoneLifetimeVal.ToString());
            tombstoneLifetime.Operation = DirectoryAttributeOperation.Replace;
            modAttrs.Add(tombstoneLifetime);

            adLdapClient.ModifyObject(objectDN, modAttrs, null);

            #endregion
        }

        /// <summary>
        /// Do Garbage Collection
        /// </summary>
        public void DoGarbageCollection()
        {
            #region Connect and Bind

            // Set the connection
            if (isConnected == false)
            {
                SetConnectAndBind(ADImplementations.AD_DS, PDCNetbiosName);
            }

            #endregion

            #region Modify Operation

            // Construct the modify attributes
            List<DirectoryAttributeModification> modAttrs = new List<DirectoryAttributeModification>();
            DirectoryAttributeModification doGarbageCollection = new DirectoryAttributeModification();
            doGarbageCollection.Name = "doGarbageCollection";
            doGarbageCollection.Add("1");
            doGarbageCollection.Operation = DirectoryAttributeOperation.Add;
            modAttrs.Add(doGarbageCollection);

            adLdapClient.ModifyObject(null, modAttrs, null);

            #endregion

            System.Threading.Thread.Sleep(1000);
        }

        #endregion

        #region ModifyDN Operations

        /// <summary>
        /// Performs ModifyDN operation (both intra domain and cross domain move. In case of cross domain move the parameter "LDAPBindWithDelegationEnabled" is used and the parameters "containerRights", "isfDoListObjectOfdSHeuristicsSet", "ParentRights" are not used, converse of this combination of parameters applies to intra domain operation whereas rest of the parameters are used for both the operations.
        /// </summary>
        /// <param name="oldDN_newDN_deleteOldRDN">The sequence only have three members: 
        /// (1)distinguished name of the old object to be modified in string format
        /// (2)distinguished name of new object to be created in string format
        /// (3)a boolean variable specifying whether to delete the old RDN in string format</param>
        /// <param name="rightsOnOldObject">Enum specifying requester's rights on the object to be modified, which is the old object</param>
        /// <param name="rightsOnNewObjectParent">Enum specifying requester rights on the parent object mentioned in the newRDN</param>
        /// <param name="rightsOnOldObjectParent">Enum specifying requester rights on the parent object mentioned in the oldDN</param>
        /// <param name="isLDAPBindWithDelegationEnabled">set to true if kerberos bind with delegation enabled is performed otherwise false</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum Specifying the error status return</param>
        // Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        // Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
        // Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void ModifyDNOperation(
            Sequence<string> oldDN_newDN_deleteOldRDN,
            RightsOnObjects rightsOnOldObject, //rightsOnObject
            RightsOnParentObjects rightsOnNewObjectParent, //accessRights
            RightOnOldParentObject rightsOnOldObjectParent, //ParentRights
            bool isLDAPBindWithDelegationEnabled,
            string control,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModDNOpErrs errorStatus)
        {

            #region Variables

            errorStatus = ConstrOnModDNOpErrs.Success;

            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            string[] searchAttrVals = null;

            string oldObjectDN = string.Empty;
            SecurityIdentifier oldObjectSid = null;
            string oldObjectParentDn = string.Empty;

            string newObjectDN = string.Empty;
            SecurityIdentifier newObjectSid = null;
            string newObjectRDN = string.Empty;
            string newObjectParentDN = string.Empty;

            string newContainerCN = string.Empty;
            string newContainerName = string.Empty;

            bool deleteOldObjectRDN;
            int index;
            SearchResult tombStoneResult = null;
            string tombStoneName = string.Empty;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                if (isRODC)
                {
                    SetConnectAndBind(ADImplementations.AD_DS, RODCNetbiosName);
                }
                else
                {
                    SetConnectAndBind(ADImplementations.AD_DS, PDCNetbiosName);
                }
            }

            #endregion

            #region Tombstone Requirements (the modifyDN operations on tombstone objects MUST fail)

            #region Create a new tombstone object

            //Add a new object into AD.
            List<DirectoryAttribute> atts = new List<DirectoryAttribute>();
            atts.Add(new DirectoryAttribute("objectClass:user"));
            atts.Add(new DirectoryAttribute("samAccountName: TempObj"));
            adLdapClient.AddObject("CN=TempObj,CN=Users," + rootDomainNC, atts, null);
            //Delete the new object
            adLdapClient.DeleteObject("CN=TempObj,CN=Users," + rootDomainNC, null);
            //Search for deleted object
            tombStoneResult = Utilities.GetTombstone(
                Utilities.BuildDeletedEntry(
                string.Format("{0}:{1}", currentWorkingDC.FQDN, currentPort),
                DELETED_OBJECTS_CONTAINER_GUID,
                rootDomainNC),
                "TempObj");
            if (tombStoneResult != null)
            {
                foreach (string key in tombStoneResult.Properties.PropertyNames)
                {
                    foreach (object value in tombStoneResult.Properties[key])
                    {
                        if (key.ToLower(CultureInfo.InvariantCulture) == "distinguishedname")
                        {
                            tombStoneName = value.ToString();
                            break;
                        }
                    }
                }
            }

            #endregion

            if (!(string.IsNullOrEmpty(tombStoneName)))
            {
                tombStoneName = tombStoneName.Replace("\0", "\\0");
                index = tombStoneName.IndexOf('\\');

                #region IntraDomainModifyDN on the new tombstone object

                if (string.IsNullOrEmpty(control))
                {
                    result = adLdapClient.IntraDomainModifyDn(
                        tombStoneName,
                        "CN=Users," + rootDomainNC,
                        tombStoneName.Remove(index),
                        true,
                        null,
                        isWindows);
                    if (currentWorkingDC.OSVersion.Equals(ServerVersion.NonWin))
                    {
                        Site.CaptureRequirementIfAreEqual<string>(
                            ConstrOnModDNOpErrs.UnwillingToPerform_UnKnownError.ToString(),
                            result + "_UnKnownError",
                            812,
                            @"For the Modify DN operation, if O!isDeleted = true, then the server returns error unwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION.");
                    }
                    else
                    {
                        Site.CaptureRequirementIfAreEqual<string>(
                            ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION.ToString(),
                            result,
                            812,
                            @"For the Modify DN operation, if O!isDeleted = true, then the server returns error unwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION.");
                    }
                }

                #endregion

                #region CrossDomainModifyDN on the new tombstone object

                else if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
                {
                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        result = adLdapClient.CrossDomainModifyDN(
                            tombStoneName,
                            "CN=Users," + rootDomainNC,
                            tombStoneName.Remove(index),
                            true,
                            CDCNetbiosName + '.' + ChildDomainDnsName,
                            null,
                            isWindows);
                        if (currentWorkingDC.OSVersion.Equals(ServerVersion.NonWin))
                        {
                            Site.CaptureRequirementIfAreEqual<string>(
                                ConstrOnModDNOpErrs.UnwillingToPerform_UnKnownError.ToString(),
                                result + "_UnKnownError",
                                862,
                                @"During cross-domain move operation, if (O!isDeleted = true), then the server returns error unwillingToPerform / ERROR_DS_CANT_MOVE_DELETED_OBJECT.");
                        }
                        else
                        {
                            Site.CaptureRequirementIfAreEqual<string>(
                                ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_CANT_MOVE_DELETED_OBJECT.ToString(),
                                result,
                                862,
                                @"During cross-domain move operation, if (O!isDeleted = true), then the server returns error unwillingToPerform / ERROR_DS_CANT_MOVE_DELETED_OBJECT.");
                        }
                        Site.CaptureRequirement(
                            1205,
                            @"The LDAP extended control LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID is supported in Windows 2000, Windows Server 2003,
                            Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                    }
                }

                #endregion
            }

            #endregion

            #region Get oldObjectDN, oldObjectParentDN and newObjectDN, newObjectRDN, newObjectParentDN

            // distinguishedNames[0] => old object DN, old object ParentDN
            // distinguishedNames[1] => new object DN, new object ParentDN
            // distinguishedNames[2] => whether to deleted old RDN
            string[] distinguishedNames = oldDN_newDN_deleteOldRDN.ToArray();
            string childDomainNetbios = string.Format("{0}$", ChildDomainNetBiosName.ToUpper());

            if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
            {
                distinguishedNames[0] = distinguishedNames[0].Replace("LDAPCHILD$", childDomainNetbios);
                distinguishedNames[1] = distinguishedNames[1].Replace("LDAPCHILD$", childDomainNetbios);
            }

            // oldObjectDN
            // for example, null => old object is already at the top of the nc tree, no parent
            if (distinguishedNames[0].ToLower(CultureInfo.InvariantCulture).Contains("null"))
            {
                oldObjectDN = null;
                oldObjectParentDn = null;
            }
            // for example, CN=Sample,DC=adts88
            else
            {
                if (string.IsNullOrEmpty(control))
                {
                    oldObjectDN = distinguishedNames[0].Replace("DC=adts88", rootDomainNC);
                    index = distinguishedNames[0].IndexOf(',');
                    oldObjectParentDn = distinguishedNames[0].Remove(0, index + 1).Replace("DC=adts88", rootDomainNC);
                }
                else if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
                {
                    oldObjectDN = distinguishedNames[0].Replace("DC=LDAP,DC=com", rootDomainNC);
                    index = distinguishedNames[0].IndexOf(',');
                    oldObjectParentDn = distinguishedNames[0].Remove(0, index + 1).Replace("DC=LDAP,DC=com", rootDomainNC);
                }
            }

            // newObjectDN
            // for example, CN=Sample,null => meaning the object is at the top of the nc tree, no parent
            if (distinguishedNames[1].Split('=')[1].Contains(","))
            {
                if (distinguishedNames[1].Split(',')[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    newObjectDN = distinguishedNames[1].Replace("DC=adts88", rootDomainNC);
                    newObjectRDN = newObjectDN.Split(',')[0].Trim();
                    newObjectParentDN = null;
                }
                if (distinguishedNames[1].Split(',')[0].Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    newObjectRDN = null;
                    index = distinguishedNames[1].IndexOf(',');
                    newObjectParentDN = distinguishedNames[1].Remove(0, index + 1).Replace("DC=adts88", rootDomainNC);
                }
            }
            // for example, CN=Sample,DC=adts88
            if (!(distinguishedNames[1].ToLower(CultureInfo.InvariantCulture).Contains("null")))
            {
                if (string.IsNullOrEmpty(control))
                {
                    newObjectDN = distinguishedNames[1].Replace("DC=adts88", rootDomainNC);
                    newObjectRDN = newObjectDN.Split(',')[0].Trim();
                    index = distinguishedNames[1].IndexOf(',');
                    newObjectParentDN = distinguishedNames[1].Remove(0, index + 1).Replace("DC=adts88", rootDomainNC);
                }
                if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
                {
                    // target domain is child domain
                    if (distinguishedNames[1].ToUpper(CultureInfo.InvariantCulture).Contains("LDAPCHILD"))
                    {
                        newObjectDN = distinguishedNames[1].Replace("DC=LDAPCHILD,DC=LDAP,DC=com", childDomainNC);
                        newObjectRDN = newObjectDN.Split(',')[0].Trim();
                        index = distinguishedNames[1].IndexOf(',');
                        newObjectParentDN = distinguishedNames[1].Remove(0, index + 1).Replace(
                            "DC=LDAPCHILD,DC=LDAP,DC=com",
                            childDomainNC);
                    }
                    else
                    {
                        newObjectDN = distinguishedNames[1].Replace("DC=LDAP,DC=com", rootDomainNC);
                        newObjectRDN = newObjectDN.Split(',')[0].Trim();
                        index = distinguishedNames[1].IndexOf(',');
                        newObjectParentDN = distinguishedNames[1].Remove(0, index + 1).Replace("DC=LDAP,DC=com", rootDomainNC);
                    }
                }
            }
            // true => delete old object RDN, false => do not delete
            if (distinguishedNames[2].ToLower(CultureInfo.InvariantCulture).Equals("true"))
            {
                deleteOldObjectRDN = true;
            }
            else
            {
                deleteOldObjectRDN = false;
            }

            #endregion

            #region Set access rights for testuser on oldObjectDN and newObjectDN and their parentDNs

            if (oldObjectDN != null
                && !newObjectDN.ToLower(CultureInfo.InvariantCulture).Contains("null"))
            {
                if (oldObjectDN.ToLower(CultureInfo.InvariantCulture).Contains("CN=Users".ToLower(CultureInfo.InvariantCulture))
                    && !oldObjectDN.ToLower(CultureInfo.InvariantCulture).Contains("NonExistingObject".ToLower(CultureInfo.InvariantCulture)))
                {
                    #region Remove rights of the requester for the old object DN, will add these rights back after object move

                    if ((rightsOnOldObject == RightsOnObjects.RIGHT_DS_WRITE_PROPERTYWithOutRIGHT_DELETE)
                        || (rightsOnOldObject == RightsOnObjects.INVALID_RIGHT))
                    {
                        // Both IntraDomainModifyDN and CrossDomainModifyDN
                        Utilities.SetAccessRights(
                            oldObjectDN,
                            testUserName,
                            PrimaryDomainDnsName,
                            ActiveDirectoryRights.Delete,
                            AccessControlType.Deny);

                        // IntraDomainModifyDN
                        if (string.IsNullOrEmpty(control))
                        {
                            Utilities.SetAccessRights(
                                oldObjectDN,
                                testUserName,
                                PrimaryDomainDnsName,
                                ActiveDirectoryRights.WriteProperty,
                                AccessControlType.Deny);
                        }
                    }

                    #endregion

                    #region Remove rights of the requester for the old object parent DN and new object parent DN

                    // IntraDomainModifyDN
                    if (string.IsNullOrEmpty(control))
                    {
                        if (!((rightsOnNewObjectParent == RightsOnParentObjects.RIGHT_DS_CREATE_CHILD)
                            && (rightsOnOldObjectParent == RightOnOldParentObject.RIGHT_DS_DELETE_CHILD))
                            && (oldObjectParentDn.ToLower(CultureInfo.InvariantCulture) != newObjectParentDN.ToLower(CultureInfo.InvariantCulture))
                            && !newObjectParentDN.ToLower(CultureInfo.InvariantCulture).Contains("NonExistingParent".ToLower(CultureInfo.InvariantCulture)))
                        {
                            Utilities.SetAccessRights(
                                oldObjectParentDn,
                                testUserName,
                                PrimaryDomainDnsName,
                                ActiveDirectoryRights.DeleteChild,
                                AccessControlType.Deny);
                            Utilities.SetAccessRights(
                                newObjectParentDN,
                                testUserName,
                                PrimaryDomainDnsName,
                                ActiveDirectoryRights.CreateChild,
                                AccessControlType.Deny);
                        }
                    }
                    // CrossDomainModifyDN
                    else if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
                    {
                        if (rightsOnOldObjectParent == RightOnOldParentObject.INVALID_RIGHT)
                        {
                            Utilities.SetAccessRights(
                                oldObjectParentDn,
                                testUserName,
                                PrimaryDomainDnsName,
                                ActiveDirectoryRights.DeleteChild,
                                AccessControlType.Deny);
                        }
                    }

                    #endregion
                }
            }

            #endregion

            #region ModifyDN Operations

            #region IntraDomainModifyDN Operation

            // IntraDomainModifyDN
            if (string.IsNullOrEmpty(control))
            {
                #region Get container name before performing IntraDomainModifyDN

                if (!distinguishedNames[0].ToLower(CultureInfo.InvariantCulture).Contains("NonExistingObject".ToLower(CultureInfo.InvariantCulture)))
                {
                    result = adLdapClient.SearchObject(
                        "CN=TestContainerALLOWRENAME," + configurationNC,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=container)",
                        new string[] { "name" },
                        null,
                        out searchResponse);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                        string.Format("Search operation on {0} should be successful, actual result: {1}", "CN=TestContainerALLOWRENAME," + configurationNC, result));
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "name");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("name={0}", searchAttrVals[0]));
                        }
                    }
                }

                #endregion

                result = adLdapClient.IntraDomainModifyDn(
                    oldObjectDN,
                    newObjectParentDN,
                    newObjectRDN,
                    deleteOldObjectRDN,
                    null,
                    isWindows);
            }

            #endregion

            #region CrossDomainModifyDN Operation

            // CrossDomainModifyDN
            else if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
            {
                //Search for the object SID

                #region Get old objectSid before performing CrossDomainModifyDN

                if (oldObjectDN != null && newObjectParentDN != null)
                {
                    result = adLdapClient.SearchObject(
                        oldObjectDN,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=user)",
                        new string[] { "objectSid" },
                        null,
                        out searchResponse,
                        isWindows);
                    Site.Assert.IsTrue(result.ToLower().Contains("success"),
                            string.Format("Search operation on {0} should be successful, actual result: {1}", oldObjectDN, result));
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectSid");
                            oldObjectSid = new SecurityIdentifier(Encoding.ASCII.GetBytes(searchAttrVals[0]), 0);
                            Site.Log.Add(LogEntryKind.Debug, string.Format("objectSid={0}", oldObjectSid.Value.ToString()));
                        }
                    }
                }

                #endregion

                #region If LDAP binding is not delegation enabled => connect and bind again with basic authentication type

                // Cross-domain move is not supported by AD LDS
                Site.Assert.AreEqual(ADDSPortNum, currentPort, "The currently connected port should be AD DS port {0}. Cross-domain move is not supported by AD LDS.", ADDSPortNum);

                // The requester must have performed a Kerberos LDAP bind with delegation enabled (see [RFC4120] section 2.8).

                if (!isLDAPBindWithDelegationEnabled)
                {
                    // if LDAP bind with delegation is not enabled, should reconnect with no ok-as-delegation flag set
                    // or remove the A2DF on service account
                    adLdapClient.ConnectAndBind(
                        currentWorkingDC.NetbiosName,
                        currentWorkingDC.IPAddress,
                        int.Parse(currentPort),
                        testUserName,
                        testUserPwd,
                        currentWorkingDC.Domain.NetbiosName,
                        AuthType.Ntlm);
                }

                #endregion

                result = adLdapClient.CrossDomainModifyDN(
                    oldObjectDN,
                    newObjectParentDN,
                    newObjectRDN,
                    deleteOldObjectRDN,
                    CDCNetbiosName + '.' + ChildDomainDnsName,
                    null,
                    isWindows);
            }

            #endregion

            if (!isWindows)
            {
                #region Switch ErrorStatus Non-Windows

                switch (result)
                {
                    case "UnwillingToPerform":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_UnKnownError;
                        break;
                    case "InsufficientAccessRights":
                        errorStatus = ConstrOnModDNOpErrs.InsufficientAccessRights_UnKnownError;
                        break;
                    case "EntryAlreadyExists":
                        errorStatus = ConstrOnModDNOpErrs.EntryAlreadyExists_UnKnownError;
                        break;
                    case "InvalidDNSyntax":
                        errorStatus = ConstrOnModDNOpErrs.InvalidDNSyntax_UnKnownError;
                        break;
                    case "Other":
                        errorStatus = ConstrOnModDNOpErrs.Other_UnKnownError;
                        break;
                    case "ProtocolError":
                        errorStatus = ConstrOnModDNOpErrs.ProtocolError_UnKnownError;
                        break;
                    case "NoSuchObject":
                        errorStatus = ConstrOnModDNOpErrs.NoSuchObject_UnKnownError;
                        break;
                    case "NotAllowedOnNonLeaf":
                        errorStatus = ConstrOnModDNOpErrs.NotAllowedOnNonLeaf_UnKnownError;
                        break;
                    case "InappropriateAuthentication":
                        errorStatus = ConstrOnModDNOpErrs.InappropriateAuthentication_UnKnownError;
                        break;
                    case "Success":
                        errorStatus = ConstrOnModDNOpErrs.Success;
                        break;
                    default:
                        errorStatus = ConstrOnModDNOpErrs.UnSpecifiedError;
                        break;
                }

                #endregion
            }
            else
            {
                #region Switch ErrorStatus Windows

                switch (result)
                {
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CANT_DELETE":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_CANT_DELETE;
                        break;
                    case "InsufficientAccessRights_UnKnownError":
                        errorStatus = ConstrOnModDNOpErrs.InsufficientAccessRights_UnKnownError;
                        break;
                    case "EntryAlreadyExists_UnKnownError":
                        errorStatus = ConstrOnModDNOpErrs.EntryAlreadyExists_UnKnownError;
                        break;
                    case "InvalidDNSyntax_UnKnownError":
                        errorStatus = ConstrOnModDNOpErrs.InvalidDNSyntax_UnKnownError;
                        break;
                    case "Other_ERROR_DS_NO_PARENT_OBJECT":
                        errorStatus = ConstrOnModDNOpErrs.Other_ERROR_DS_NO_PARENT_OBJECT;
                        break;
                    case "ProtocolError_ERROR_INVALID_PARAMETER":
                        errorStatus = ConstrOnModDNOpErrs.ProtocolError_ERROR_INVALID_PARAMETER;
                        break;
                    case "UnwillingToPerform_ERROR_INVALID_PARAMETER":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_INVALID_PARAMETER;
                        break;
                    case "NoSuchObject_ERROR_DS_OBJ_NOT_FOUND":
                        errorStatus = ConstrOnModDNOpErrs.NoSuchObject_ERROR_DS_OBJ_NOT_FOUND;
                        break;
                    case "Other_ERROR_DS_DISALLOWED_IN_SYSTEM_CONTAINER":
                        errorStatus = ConstrOnModDNOpErrs.Other_ERROR_DS_DISALLOWED_IN_SYSTEM_CONTAINER;
                        break;
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_XDOM_MOVE_OPERATION":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_XDOM_MOVE_OPERATION;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CANT_MOVE_ACCOUNT_GROUP":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_CANT_MOVE_ACCOUNT_GROUP;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CANT_MOVE_RESOURCE_GROUP":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_CANT_MOVE_RESOURCE_GROUP;
                        break;
                    case "UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CANT_MOVE_APP_QUERY_GROUP":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_CANT_MOVE_APP_QUERY_GROUP;
                        break;
                    case "InvalidDNSyntax_ERROR_DS_SRC_AND_DST_NC_IDENTICAL":
                        errorStatus = ConstrOnModDNOpErrs.InvalidDNSyntax_ERROR_DS_SRC_AND_DST_NC_IDENTICAL;
                        break;
                    case "NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST":
                        errorStatus = ConstrOnModDNOpErrs.NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST;
                        break;
                    case "UnwillingToPerform_ERROR_DS_CANT_WITH_ACCT_GROUP_MEMBERSHPS":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_CANT_WITH_ACCT_GROUP_MEMBERSHPS;
                        break;
                    case "NoSuchObject_ERROR_DS_CANT_FIND_EXPECTED_NC":
                        errorStatus = ConstrOnModDNOpErrs.NoSuchObject_ERROR_DS_CANT_FIND_EXPECTED_NC;
                        break;
                    case "InappropriateAuthentication_UnKnownError":
                        errorStatus = ConstrOnModDNOpErrs.InappropriateAuthentication_UnKnownError;
                        break;
                    case "UnwillingToPerform_ERROR_DS_MODIFYDN_DISALLOWED_BY_FLAG":
                        errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_ERROR_DS_MODIFYDN_DISALLOWED_BY_FLAG;
                        break;
                    case "Success_STATUS_SUCCESS":
                        errorStatus = ConstrOnModDNOpErrs.Success;
                        break;
                    default:
                        errorStatus = ConstrOnModDNOpErrs.UnSpecifiedError;
                        break;
                }

                #endregion
            }

            if (isRODC
                && result.ToLower(CultureInfo.InvariantCulture).Contains("NoSuchObject".ToLower(CultureInfo.InvariantCulture)))
            {
                errorStatus = ConstrOnModDNOpErrs.NoSuchObject_UnKnownError;
            }
            if ((errorStatus != ConstrOnModDNOpErrs.Success)
                && (oldDN_newDN_deleteOldRDN.Contains("CN=BCKUPKEY_P Secret,CN=System,DC=adts88")
                || oldDN_newDN_deleteOldRDN.Contains("CN=LostAndFound,DC=adts88")
                || oldDN_newDN_deleteOldRDN.Contains("CN=TestContainerWithoutSysFlags,CN=DisplaySpecifiers,CN=Configuration,DC=adts88")))
            {
                errorStatus = ConstrOnModDNOpErrs.UnwillingToPerform_UnKnownError;
            }
            if ((errorStatus != ConstrOnModDNOpErrs.Success)
                && (oldDN_newDN_deleteOldRDN.Contains("CN=TestUser4,CN=Computers,DC=adts88")))
            {
                errorStatus = ConstrOnModDNOpErrs.EntryAlreadyExists_UnKnownError;
            }

            #endregion

            #region Requirements on ModifyDN Operations

            if (errorStatus.Equals(ConstrOnModDNOpErrs.Success))
            {
                // IntraDomainModifyDN
                if (string.IsNullOrEmpty(control))
                {
                    #region Search container name after performing IntraDomainModifyDN

                    result = adLdapClient.SearchObject(
                        "CN=TestContainerALLOWRENAME1," + configurationNC,
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "(objectClass=container)",
                        new string[] { "cn", "name" },
                        null,
                        out searchResponse,
                        isWindows);
                    if (searchResponse != null)
                    {
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "cn");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("cn={0}", searchAttrVals[0]));
                            newContainerCN = searchAttrVals[0];

                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "name");
                            Site.Log.Add(LogEntryKind.Debug, string.Format("name={0}", searchAttrVals[0]));
                            newContainerName = searchAttrVals[0];
                        }
                        if (oldObjectParentDn == newObjectParentDN)
                        {
                            Site.CaptureRequirementIfAreEqual<string>(
                                newContainerCN,
                                newContainerName,
                                35,
                                @"An object's value of name attribute equals the value of the object's RDN attribute.
                                Even if an object is renamed (LDAP Modify DN), the object's name attribute remains equal to the object's RDN attribute.");
                        }
                    }

                    #endregion
                }
                // CrossDomainModifyDN
                else if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
                {
                    currentWorkingDC = GetDomainController(CDCNetbiosName);
                    currentPort = ADDSPortNum;
                    adLdapClient.ConnectAndBind(
                        currentWorkingDC.NetbiosName,
                        currentWorkingDC.IPAddress,
                        int.Parse(currentPort),
                        childAdminName,
                        childAdminPwd,
                        ChildDomainDnsName,
                        AuthType.Basic);

                    #region Target domain controller name for LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID control

                    Site.CaptureRequirementIfIsNotNull(
                        CDCNetbiosName + '.' + ChildDomainDnsName,
                        831,
                        @"The controlValue field of LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID control has the DNS hostname of the target DC 
                    that must be used as a helper to perform cross domain move.");

                    #endregion

                    #region Get newObjectSid after performing CrossDomainModifyDN, should not be the same with oldObjectSid

                    if (oldObjectDN != null && newObjectParentDN != null)
                    {
                        result = adLdapClient.SearchObject(
                            newObjectDN,
                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                            "(objectClass=user)",
                            new string[] { "objectSid" },
                            null,
                            out searchResponse,
                            isWindows);
                        Site.Assert.IsTrue(result.ToLower().Contains("success"),
                            string.Format("Search operation on {0} should be successful, actual result: {1}", newObjectDN, result));
                        if (searchResponse != null)
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "objectSid");
                                newObjectSid = new SecurityIdentifier(Encoding.ASCII.GetBytes(searchAttrVals[0]), 0);
                                Site.Log.Add(LogEntryKind.Debug, string.Format("objectSid={0}", newObjectSid.Value.ToString()));
                                Site.CaptureRequirementIfAreNotEqual<SecurityIdentifier>(
                                    newObjectSid,
                                    oldObjectSid,
                                    30,
                                    @"During LDAP Add operation, once a fresh SID is assigned to the objectSid attribute of a security 
                                principal object, this attribute can be writable, if the object is moved to another NC 
                                (LDAP Modify DN; see section 3.1.1.5 for the specification of such moves).");
                            }
                        }
                    }

                    #endregion
                }
            }

            #endregion

            #region Set access rights back for testuser on old and new objectDN and their parents

            if (!errorStatus.Equals(ConstrOnModDNOpErrs.Success))
            {
                if (oldObjectDN != null
                    && !newObjectDN.ToLower(CultureInfo.InvariantCulture).Contains("null"))
                {
                    if (oldObjectDN.ToLower(CultureInfo.InvariantCulture).Contains("CN=Users".ToLower(CultureInfo.InvariantCulture))
                        && !oldObjectDN.ToLower(CultureInfo.InvariantCulture).Contains("NonExistingObject".ToLower(CultureInfo.InvariantCulture)))
                    {
                        #region Set access rights back for old object DN

                        // Both IntraDomainModifyDN and CrossDomainModifyDN
                        Utilities.RemoveAccessRights(
                            oldObjectDN,
                            testUserName,
                            PrimaryDomainDnsName,
                            ActiveDirectoryRights.Delete,
                            AccessControlType.Deny);
                        Utilities.SetAccessRights(
                            oldObjectDN,
                            testUserName,
                            PrimaryDomainDnsName,
                            ActiveDirectoryRights.Delete,
                            AccessControlType.Allow);

                        // IntraDomainModifyDN
                        if (string.IsNullOrEmpty(control))
                        {
                            Utilities.RemoveAccessRights(
                                oldObjectDN,
                                testUserName,
                                PrimaryDomainDnsName,
                                ActiveDirectoryRights.WriteProperty,
                                AccessControlType.Deny);
                            Utilities.SetAccessRights(
                                oldObjectDN,
                                testUserName,
                                PrimaryDomainDnsName,
                                ActiveDirectoryRights.WriteProperty,
                                AccessControlType.Allow);
                        }

                        #endregion

                        #region Set access rights back for old object parent DN and new object parent DN

                        // IntraDomainModifyDN
                        if (string.IsNullOrEmpty(control))
                        {
                            if (!newObjectParentDN.ToLower(CultureInfo.InvariantCulture).Contains("NonExistingParent".ToLower(CultureInfo.InvariantCulture)))
                            {
                                Utilities.RemoveAccessRights(
                                    oldObjectParentDn,
                                    testUserName,
                                    PrimaryDomainDnsName,
                                    ActiveDirectoryRights.DeleteChild,
                                    AccessControlType.Deny);
                                Utilities.SetAccessRights(
                                    oldObjectParentDn,
                                    testUserName,
                                    PrimaryDomainDnsName,
                                    ActiveDirectoryRights.DeleteChild,
                                    AccessControlType.Allow);
                                Utilities.RemoveAccessRights(
                                    newObjectParentDN,
                                    testUserName,
                                    PrimaryDomainDnsName,
                                    ActiveDirectoryRights.CreateChild,
                                    AccessControlType.Deny);
                                Utilities.SetAccessRights(
                                    newObjectParentDN,
                                    testUserName,
                                    PrimaryDomainDnsName,
                                    ActiveDirectoryRights.CreateChild,
                                    AccessControlType.Allow);
                            }
                        }

                        // CrossDomainModifyDN
                        else if (control == ExtendedControl.LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID)
                        {
                            if (rightsOnOldObjectParent == RightOnOldParentObject.INVALID_RIGHT)
                            {
                                Utilities.RemoveAccessRights(
                                    oldObjectParentDn,
                                    testUserName,
                                    PrimaryDomainDnsName,
                                    ActiveDirectoryRights.DeleteChild,
                                    AccessControlType.Deny);
                                Utilities.SetAccessRights(
                                    oldObjectParentDn,
                                    testUserName,
                                    PrimaryDomainDnsName,
                                    ActiveDirectoryRights.DeleteChild,
                                    AccessControlType.Allow);
                            }
                        }

                        #endregion
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Search Operations

        /// <summary>
        /// Handles search request
        /// </summary>
        /// <param name="baseObjectDN">DN of the base object</param>
        /// <param name="filter">Conditions that must be fulfilled inorder for search to match a given entry</param>
        /// <param name="scope">Defines scope of search</param>
        /// <param name="attributesToBeReturned">Specifies attributes of interest</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Search Operation is on AD DS or AD LDS</param>
        // Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        // Disable warning CA1505 because according to Test Case design, excessive maintainability index is necessary.
        // Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        // Disable warning CA1809 because according to Test Case design, more than 64 local variables are needed.
        // Disable warning CA1500 the value of the parameter serverHostName is always the same.
        // Disable warning CA1800 because it will affect the implementation of Adapter
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
        [SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames")]
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public void SearchOpReq(
            string baseObjectDN,
            string filter,
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.SearchScope scope,
            Sequence<string> attributesToBeReturned,
            string control,
            ADImplementations service)
        {
            #region Variables

            //Search scope of actual implementation.
            System.DirectoryServices.Protocols.SearchScope ScopeImpSearch;
            string[] attrsToReturn = new string[attributesToBeReturned.Count];

            ICollection<AdtsSearchResultEntryPacket> searchResponse = null;
            string[] searchAttrVals = null;

            /////////////////////////////////////////
            string netLogon = string.Empty;
            int forestFunctionalLevel = 0;
            SecurityIdentifier sidOftheOldObject = null;
            int i = 0;
            string dnsRoot = string.Empty;

            #endregion

            #region Connect and Bind

            if (isConnected == false)
            {
                SetConnectAndBind(service, PDCNetbiosName);
            }

            #endregion

            #region RootDSE Modify schemaUpdateNow

            // [MS-ADTS] section 3.1.1.3.3.13 schemaUpdateNow
            // After the completion of this operation, the subschema exposed by the server reflects the current state
            // of the schema as defined by the attributeSchema and classSchema objects in the schema NC.
            DirectoryAttributeModification schemaRefresh = new DirectoryAttributeModification();
            schemaRefresh.Name = "schemaUpdateNow";
            schemaRefresh.Add("1");
            schemaRefresh.Operation = DirectoryAttributeOperation.Add;
            List<DirectoryAttributeModification> refreshAttributes = new List<DirectoryAttributeModification>();
            refreshAttributes.Add(schemaRefresh);
            try
            {
                result = adLdapClient.ModifyObject(null, refreshAttributes, null, isWindows);
                Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("RootDSE Modify operation on schemaUpdateNow should be successful, actual result: {0}", result));
            }
            catch (Exception ex)
            {
                result = string.Empty;
                Site.Log.Add(LogEntryKind.Warning, ex.Message);
            }

            #endregion

            #region Attributes

            foreach (string attr in attributesToBeReturned)
            {
                attrsToReturn[i] = attr;
                i++;
            }

            #endregion

            #region Search Scope

            switch (scope)
            {
                case Microsoft.Protocols.TestSuites.ActiveDirectory.Common.SearchScope.baseObject:
                    ScopeImpSearch = System.DirectoryServices.Protocols.SearchScope.Base;
                    break;
                case Microsoft.Protocols.TestSuites.ActiveDirectory.Common.SearchScope.SingleLevel:
                    ScopeImpSearch = System.DirectoryServices.Protocols.SearchScope.OneLevel;
                    break;
                case Microsoft.Protocols.TestSuites.ActiveDirectory.Common.SearchScope.Subtree:
                default:
                    ScopeImpSearch = System.DirectoryServices.Protocols.SearchScope.Subtree;
                    break;
            }

            #endregion

            #region Search Base DN

            #region RootDSE base DN == null

            if (baseObjectDN.Equals("null"))
            {
                baseObjectDN = null;
            }

            #endregion

            if (!string.IsNullOrEmpty(baseObjectDN))
            {
                switch (service)
                {
                    case ADImplementations.AD_LDS:
                        baseObjectDN = baseObjectDN.Replace("CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}", configurationNC.Split(',')[1]);
                        break;
                    case ADImplementations.AD_DS:
                    default:
                        baseObjectDN = baseObjectDN.Replace("DC=adts88", rootDomainNC);
                        baseObjectDN = baseObjectDN.Replace("ADTS88", PrimaryDomainNetBiosName.ToUpper());
                        break;
                }
                baseObjectDN = baseObjectDN.Replace("adts_user1", testUserName);
                testUserGuid = Utilities.GetUserGuid(
                    PDCNetbiosName,
                    PrimaryDomainDnsName,
                    ADDSPortNum,
                    testUserName,
                    testUserPwd,
                    testUserName);
                baseObjectDN = baseObjectDN.Replace("a5127683905336458dc57f180a0adf16", testUserGuid);
                testUserSid = Utilities.GetUserSid(
                    PDCNetbiosName,
                    PrimaryDomainDnsName,
                    testUserName,
                    testUserPwd,
                    testUserName);
                baseObjectDN = baseObjectDN.Replace("S-1-5-21-126476475-3050469762-4276591986-1104", testUserSid);
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.Replace("DC=adts88", rootDomainNC);
                filter = filter.Replace("adts88", PrimaryDomainDnsName);
                if (filter.Contains(":")
                    && !filter.Contains(":="))
                {
                    filter = string.Format("({0}={1})", filter.Split(':')[0].TrimStart('('), filter.Split(':')[1].TrimEnd(')'));
                }
                if (!filter.Contains("(") || !filter.Contains(")"))
                {
                    filter = string.Format("({0})", filter);
                }
            }

            #endregion

            #region Search Operations

            byte[] hexFormatValue = null;
            string distName = string.Empty;
            DirectoryControl[] controls;

            switch (control)
            {
                case ExtendedControl.LDAP_PAGED_RESULT_OID_STRING:
                    #region LDAP_PAGED_RESULT_OID_STRING Search Operation

                    result = adLdapClient.PageRequestControl(
                        2,
                        baseObjectDN,
                        ScopeImpSearch,
                        filter,
                        attrsToReturn,
                        out searchResponse,
                        isWindows);

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_DIRSYNC_OID:
                    break;
                case ExtendedControl.LDAP_SERVER_DOMAIN_SCOPE_OID: //nothing new
                    #region LDAP_SERVER_DOMAIN_SCOPE_OID

                    controls = new DirectoryControl[] {
                        new DirectoryControl(control, null, true, true)
                    };
                    result = adLdapClient.SearchObject(
                        baseObjectDN,
                        ScopeImpSearch,
                        filter,
                        attrsToReturn,
                        controls,
                        out searchResponse,
                        isWindows);

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_EXTENDED_DN_OID:
                    #region LDAP_SERVER_EXTENDED_DN_OID

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        // Test flag with different control values: 0 and 1
                        for (int flag = 0; flag < 2; flag++)
                        {
                            hexFormatValue = BerConverter.Encode("{i}", flag);
                            controls = new DirectoryControl[] {
                                new DirectoryControl(ExtendedControl.LDAP_SERVER_EXTENDED_DN_OID, hexFormatValue, true, true)
                            };
                            result = adLdapClient.SearchObject(
                                baseObjectDN,
                                ScopeImpSearch,
                                filter,
                                attrsToReturn,
                                controls,
                                out searchResponse,
                                isWindows);
                            Site.CaptureRequirement(
                                1254,
                                @"When sending LDAP_SERVER_EXTENDED_DN_OID control to a Windows Server 2003 or later DC, the flag field of controlValue structure
                                must be set to 0 or 1.");

                            List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                            foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                    (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                    entrypacket.GetInnerRequestOrResponse();
                                string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                Site.Log.Add(LogEntryKind.Debug, responselist.IndexOf(entrypacket) + 1 + "," + distinguishedName);
                                distName = distinguishedName.Split(';')[0].Split('=')[1].Substring(
                                    0,
                                    distinguishedName.Split(';')[0].Split('=')[1].Length - 1);
                                if (flag == 0)
                                {
                                    bool isHexString = true;
                                    char[] distCharArray = distName.ToCharArray();
                                    foreach (char ch in distCharArray)
                                    {
                                        char[] hexChars =
                                            new char[] { 'a', 'b', 'c', 'd', 'e', 'f', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                                        string hexString = new string(hexChars);
                                        if (!(hexString.Contains(ch.ToString())))
                                        {
                                            isHexString = false;
                                            break;
                                        }
                                    }
                                    Site.CaptureRequirementIfIsTrue(isHexString, 1255, "When sending LDAP_SERVER_EXTENDED_DN_OID control to a Windows Server 2003 or later DC, if the value of flag field of controlValue structure is 0, it is equivalent to omitting the controlValue field which causes the DC to return the values of the objectGUID and objectSid attributes as a hexadecimal representation of their binary format.");
                                }
                                else if (flag == 1)
                                {
                                    //DN is in the format <GUID=07d41eeb-3c59-49df-b745-11f59f695b35>;<SID=S-1-5-21-126476475-3050469762-4276591986-500>;CN=Administrator,CN=Users,DC=adts88. Checking for'-' in this DN verifies the dashed format of this GUID.
                                    Site.CaptureRequirementIfIsTrue(distName.Contains("-"), 1256, "When sending LDAP_SERVER_EXTENDED_DN_OID control to a Windows Server 2003 or later DC, if the value of flag field of controlValue structure is 1, DC returns the GUID in dashed-string format ([RFC4122] section 3) and the SID in SDDL SID string format ([MS-DTYP] section 2.5.1).");
                                }
                            }
                        }
                        Site.CaptureRequirement(1208, "The LDAP extended control LDAP_SERVER_EXTENDED_DN_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_GET_STATS_OID:
                    #region LDAP_SERVER_GET_STATS_OID

                    Site.CaptureRequirement(1209, "The LDAP extended control LDAP_SERVER_GET_STATS_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_RANGE_OPTION_OID:
                    #region LDAP_SERVER_RANGE_OPTION_OID Search Operation

                    if (attributesToBeReturned.Contains("member;range=10-0"))
                    {
                        if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                        {
                            // Here make sure that Administrators contain 3 values as part of member attribute.
                            // So we have given 3-*
                            // Suppose if your administrators group contains more than 5 values say for example 6
                            // Give 6-*. Always ensure that ((total number of entries + 1) - *)
                            result = adLdapClient.RangeOptionControl(
                                baseObjectDN,
                                ScopeImpSearch,
                                filter,
                                new string[] { "member;range=100-*" },
                                out searchResponse,
                                isWindows);
                            Site.Log.Add(LogEntryKind.Debug, "Search Object with RangeOptionControl returns: {0}", result);
                            Site.CaptureRequirementIfIsTrue(
                                result.Contains(Enum.GetName(typeof(ResultCode), ResultCode.OperationsError)),
                                433,
                                "When LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID control is not specified, if range retrieval is being performed on an attribute whose values are forward link values or back link values, and the value of low is greater than or equal to the number of values in the attribute, the DC will return the error operationsError.");
                            Site.CaptureRequirement(1213, "The LDAP extended control LDAP_SERVER_RANGE_OPTION_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                        }
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_SD_FLAGS_OID:
                    #region LDAP_SERVER_SD_FLAGS_OID Search Operation

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        result = adLdapClient.SecurityDescriptorFlagsControl(
                            System.DirectoryServices.Protocols.SecurityMasks.None,
                            baseObjectDN,
                            ScopeImpSearch,
                            filter,
                            attrsToReturn,
                            out searchResponse,
                            isWindows);

                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            byte[] attrivalues = adLdapClient.GetAttributeValuesInBytes(entrypacket, "ntSecurityDescriptor")[0];

                            ActiveDirectorySecurity securityDescriptor = new ActiveDirectorySecurity();
                            securityDescriptor.SetSecurityDescriptorBinaryForm(attrivalues);
                            //GetsSecurityDescriptorOwner method will return the owner part of Secuirty Descriptor
                            string owner = Utilities.GetSecurityDescriptorOwner(securityDescriptor);
                            Site.CaptureRequirementIfAreEqual<string>(rootDomainAdminsGroup.ToLower(CultureInfo.InvariantCulture), owner.ToLower(CultureInfo.InvariantCulture), 396, "During the LDAP Search request, if the bit flag OWNER_SECURITY_INFORMATION (OSI) is specified for LDAP_SERVER_SD_FLAGS_OID control, then the portion of security descriptor to retrieve/update is the Owner identifier of the object.");
                            //GetsSecurityDescriptorGroup method will return the group part of Secuirty Descriptor
                            string group = Utilities.GetSecurityDescriptorGroup(securityDescriptor);
                            Site.CaptureRequirementIfAreEqual<string>(rootDomainAdminsGroup.ToLower(CultureInfo.InvariantCulture), group.ToLower(CultureInfo.InvariantCulture), 397, "During the LDAP Search request, if the bit flag GROUP_SECURITY_INFORMATION (GSI) is specified for LDAP_SERVER_SD_FLAGS_OID control, then the portion of security descriptor to retrieve/update is the Primary group identifier.");
                            //GetsSecurityDescriptorDaclSacl method will return the ACEs of all DACL And SACL
                            AuthorizationRuleCollection dacl = Utilities.GetDaclSacl(securityDescriptor);
                            // DACL contains So many ACEs(Access Control Entries).
                            // Aces in turn contains AceType,objectType,inheritedType etc.
                            // So here we are only verifying the count of dacl.
                            Site.CaptureRequirementIfIsTrue((dacl.Count > 0), 398, "During the LDAP Search request, if the bit flag DACL_SECURITY_INFORMATION (DSI) is specified for LDAP_SERVER_SD_FLAGS_OID control, then the portion of security descriptor to retrieve/update is the Discretionary access control list (DACL) of the object.");
                            AuthorizationRuleCollection sacl = Utilities.GetDaclSacl(securityDescriptor);
                            // Sacl contains So many ACEs(Access Control Entries).
                            // Aces in turn contains AceType,objectType,inheritedType etc.
                            // So here we are only verifying the count of Sacl.
                            Site.CaptureRequirementIfIsTrue((sacl.Count > 0), 399, "During the LDAP Search request, if the bit flag SACL_SECURITY_INFORMATION (SSI) is specified for LDAP_SERVER_SD_FLAGS_OID control, then the portion of security descriptor to retrieve/update is the System access control list (SACL) of the object.");
                            Site.CaptureRequirement(1215, "The LDAP extended control LDAP_SERVER_SD_FLAGS_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                        }
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_SEARCH_OPTIONS_OID:
                    #region LDAP_SERVER_SEARCH_OPTIONS_OID Search operation

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        byte[] searchOptionVal = null;
                        for (int k = 1; k <= 2; k++)
                        {
                            searchOptionVal = BerConverter.Encode("{i}", k);
                            hexFormatValue = BerConverter.Encode("{i}", k);
                            controls = new DirectoryControl[] {
                                new DirectoryControl(ExtendedControl.LDAP_SERVER_SEARCH_OPTIONS_OID, searchOptionVal, true, true)
                            };
                            result = adLdapClient.SearchObject(
                                baseObjectDN,
                                ScopeImpSearch,
                                filter,
                                attrsToReturn,
                                controls,
                                out searchResponse,
                                isWindows);

                            #region DomainScope

                            if ((searchResponse != null) && (k == 1))
                            {
                                // DomainScope means ParentDomain objects
                                Site.Log.Add(LogEntryKind.Debug, "DomainScopeOption");
                                List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                                foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                                {
                                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                        (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                        entrypacket.GetInnerRequestOrResponse();
                                    string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                    Site.Log.Add(LogEntryKind.Debug,
                                        responselist.IndexOf(entrypacket) + 1 + "," + distinguishedName);
                                }
                                Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 401, "When LDAP_SERVER_SEARCH_OPTIONS_OID is used with an LDAP Search request, if the flag field of controlValue structure is set to SERVER_SEARCH_FLAG_DOMAIN_SCOPE (SSFDS) that is 1, the control prevents continuation references from being generated when the search results are returned.");
                            }

                            #endregion

                            #region PhantomRoot

                            if ((searchResponse != null) && (k == 2))
                            {
                                // Phantom root option indicates the objects in the child domain will also  be retained.
                                // Phantomroot means childdomain Objects
                                Site.Log.Add(LogEntryKind.Debug, "Phantom Root");
                                List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                                foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                                {
                                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                        (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                        entrypacket.GetInnerRequestOrResponse();
                                    string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                    Site.Log.Add(
                                        LogEntryKind.Debug,
                                        responselist.IndexOf(entrypacket) + 1 + "," + distinguishedName);
                                }
                                Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 402, "When LDAP_SERVER_SEARCH_OPTIONS_OID is used with an LDAP Search request, if the flag field of controlValue structure is set to SERVER_SEARCH_FLAG_PHANTOM_ROOT (SSFPR) that is 2, the control instructs the server to search all NC replicas that are subordinate to the search base, even if the search base is not instantiated on the server. This will cause the search to be executed over all NC replicas held on the DC that are subordinate to the search base. This enables search bases such as the empty string, which would cause the server to search all of the NC replicas that it holds.");
                            }

                            #endregion
                        }
                        Site.CaptureRequirement(1216, "The LDAP extended control LDAP_SERVER_SEARCH_OPTIONS_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_VERIFY_NAME_OID:
                    #region LDAP_SERVER_VERIFY_NAME_OID

                    Site.CaptureRequirement(1220, "The LDAP extended control LDAP_SERVER_VERIFY_NAME_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");

                    #endregion
                    break;
                case ExtendedControl.LDAP_CONTROL_VLVREQUEST:
                case ExtendedControl.LDAP_SERVER_SORT_OID:
                    #region LDAP_CONTROL_VLVREQUEST,LDAP_CONTROL_VLVRESPONSE,LDAP_SERVER_SORT_OID,LDAP_SERVER_RESP_SORT_OID

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        #region SucessCase(Return SearchResponse instead of string)

                        // Here Before count is 0. After count is 7(which is nothing but maximum number of entries)
                        result = adLdapClient.VirtualListViewControl(
                             "cn",
                             false,
                             0,
                             7,
                             "a*",
                             baseObjectDN,
                             ScopeImpSearch,
                             filter,
                             attrsToReturn,
                             out searchResponse,
                             isWindows);
                        Site.Log.Add(LogEntryKind.Debug, searchResponse.Count.ToString(CultureInfo.InvariantCulture));
                        // Display the entries
                        List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                        foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                            (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                            entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            Site.Log.Add(
                                LogEntryKind.Debug, responselist.IndexOf(entrypacket) + " " + distinguishedName);
                        }
                        Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 417, "The LDAP_CONTROL_VLVREQUEST control is used with an LDAP search operation to retrieve a subset of the objects that satisfy the search request.");
                        // Here while calling itself we provided BeforeCount as 0(Always Starts with 0) and After count as 7. 
                        // So the matching results count Should not be >= 7
                        Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 1334, "This control (LDAP_CONTROL_VLVREQUEST) permits the client to request that the server return a specified number of objects before and after the target object, in addition to the target object itself.\"Before\" and \"after\" the target object are relative to the sort order of the search result set");
                        // This is partial verification only.
                        // Before performing Search Operation Sort Request needs to be attached to VLV Request.
                        // In this requirement we are only verifying the following
                        // The operation is an LDAP search request. The LDAP_CONTROL_VLVREQUEST control is attached to the search.The scope of the search request is whole Subtree.The base object of the search request specifies the DN
                        // Cast the second directory control as a VlvResponseControl object
                        // When LDAP_SERVER_RESP_SORT_OID is used, this requirement is captured
                        Site.CaptureRequirement(1214, "The LDAP extended control LDAP_SERVER_RESP_SORT_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                        // When LDAP_SERVER_RESP_SORT_OID is used, this requirement is captured
                        Site.CaptureRequirement(1217, "The LDAP extended control LDAP_SERVER_SORT_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                        // When LDAP_SERVER_RESP_SORT_OID is used, this requirement is captured
                        Site.CaptureRequirement(1221, "The LDAP extended control LDAP_CONTROL_VLVREQUEST is supported in Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                        // When LDAP_SERVER_RESP_SORT_OID is used, this requirement is captured
                        Site.CaptureRequirement(1222, "The LDAP extended control LDAP_CONTROL_VLVRESPONSE is supported in Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");

                        #endregion
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID:
                    #region LDAP_SERVER_SHOW_DELETED_OID Search operation

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        Site.Log.Add(LogEntryKind.Debug, "Deleted Objects");
                        string dnOfDeletedObject = string.Empty;
                        controls = new DirectoryControl[] {
                            new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true)
                        };
                        result = adLdapClient.SearchObject(
                            baseObjectDN,
                            ScopeImpSearch,
                            filter,
                            attrsToReturn,
                            controls,
                            out searchResponse,
                            isWindows);
                        List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                        foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                responselist.IndexOf(entrypacket) + 1 + "," + distinguishedName);
                            dnOfDeletedObject = distinguishedName;
                            Site.CaptureRequirementIfIsTrue(dnOfDeletedObject.ToLower(CultureInfo.InvariantCulture).Contains("deleted objects".ToLower(CultureInfo.InvariantCulture)), 408, "The LDAP_SERVER_SHOW_DELETED_OID control is used with an LDAP search operation to specify that tombstones and deleted-objects should be visible to the operation.");
                            //Presence of "deleted objects" in the distinguished name indicates that the object resides in tombstone.
                            string scopeDN = forestScopePartialDN + ',' + configurationNC;
                            string featureDN = recycleBinPartialDN + ',' + configurationNC;
                            if (Utilities.IsOptionalFeatureEnabled(scopeDN, featureDN))
                            {
                                Site.CaptureRequirementIfIsTrue(dnOfDeletedObject.ToLower(CultureInfo.InvariantCulture).Contains("deleted objects".ToLower(CultureInfo.InvariantCulture)), 46, "[When the Recycle Bin optional feature is not enabled] A tombstone is not returned by a normal LDAP Search request, but only by a Search request with extended control LDAP_SERVER_SHOW_DELETED_OID or LDAP_SERVER_SHOW_RECYCLED_OID.");
                                Site.CaptureRequirementIfIsTrue(dnOfDeletedObject.ToLower(CultureInfo.InvariantCulture).Contains("deleted objects".ToLower(CultureInfo.InvariantCulture)), 4643, "[When the Recycle Bin optional feature is not enabled] A tombstone is returned by a LDAP Search request with extended control LDAP_SERVER_SHOW_DELETED_OID.");
                                break;
                            }
                        }
                        Site.CaptureRequirement(1218, "The LDAP extended control LDAP_SERVER_SHOW_DELETED_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_ASQ_OID:
                    #region LDAP_SERVER_ASQ_OID

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        result = adLdapClient.ASQQuery(
                            "member",
                            baseObjectDN,
                            ScopeImpSearch,
                            filter,
                            attrsToReturn,
                            out searchResponse,
                            isWindows);
                        Site.CaptureRequirementIfIsTrue(result.ToLower().Contains("success"), 418, "The LDAP_SERVER_ASQ_OID control is used with an LDAP search operation. Only searches of base object scope can be used with the LDAP_SERVER_ASQ_OID control.");
                        // This is done in implemenation Adapter : AsqResponseControl asqResponse = (AsqResponseControl)searchResponse.Controls[0];
                        // Comments are provided in the ASQQuery method of implementation adapter
                        Site.CaptureRequirementIfIsTrue(result.ToLower().Contains("success"), result, 419, "When the server receives a search request with the LDAP_SERVER_ASQ_OID control attached to it, it includes a response control in the search response.");
                        Site.CaptureRequirementIfIsTrue(result.ToLower().Contains("success"), result, 420, "The controlType field of the returned Control structure is set to the OID of the LDAP_SERVER_ASQ_OID control.");
                        Site.CaptureRequirement(1223, "The LDAP extended control LDAP_SERVER_ASQ_OID is supported in Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_QUOTA_CONTROL_OID:
                    #region LDAP_SERVER_QUOTA_CONTROL_OID Search Operation

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        #region ObjectSidOfTheUser

                        Utilities.SetAccessRights(
                            "CN=NTDS Quotas," + rootDomainNC,
                            testUserName,
                            currentWorkingDC.Domain.NetbiosName,
                            ActiveDirectoryRights.ReadProperty,
                            AccessControlType.Allow);
                        Utilities.SetAccessRights(
                            "CN=NTDS Quotas," + configurationNC,
                            testUserName,
                            currentWorkingDC.Domain.NetbiosName,
                            ActiveDirectoryRights.ReadProperty,
                            AccessControlType.Allow);
                        Utilities.SetControlAcessRights(
                            "CN=NTDS Quotas," + rootDomainNC,
                            testUserName,
                            currentWorkingDC.Domain.NetbiosName,
                            ControlAccessRight.DS_Query_Self_Quota,
                            ActiveDirectoryRights.ExtendedRight,
                            AccessControlType.Allow);
                        Utilities.SetControlAcessRights(
                            "CN=NTDS Quotas," + configurationNC,
                            testUserName,
                            currentWorkingDC.Domain.NetbiosName,
                            ControlAccessRight.DS_Query_Self_Quota,
                            ActiveDirectoryRights.ExtendedRight,
                            AccessControlType.Allow);
                        result = adLdapClient.SearchObject(
                            "CN=" + testUserName + ",CN=Users," + rootDomainNC,
                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                            "(ObjectClass=user)",
                            new string[] { "objectSid" },
                            null,
                            out searchResponse,
                            isWindows);
                        if (searchResponse != null)
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                    (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                    entrypacket.GetInnerRequestOrResponse();
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        if (attributeString.Equals("objectSid", StringComparison.OrdinalIgnoreCase))
                                        {
                                            byte[] byteVal = attributeVal.ByteArrayValue;
                                            sidOftheOldObject = new SecurityIdentifier(byteVal, 0);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region SearchFor msDS-QuotaContainer

                        byte[] sidInBytes = new byte[sidOftheOldObject.BinaryLength];
                        sidOftheOldObject.GetBinaryForm(sidInBytes, 0);
                        sidInBytes = BerConverter.Encode("{o}", sidInBytes);

                        controls = new DirectoryControl[]{
                                new DirectoryControl(ExtendedControl.LDAP_SERVER_QUOTA_CONTROL_OID, sidInBytes, true, true)
                            };
                        result = adLdapClient.SearchObject(
                            baseObjectDN,
                            ScopeImpSearch,
                            filter,
                            attrsToReturn,
                            controls,
                            out searchResponse,
                            isWindows);
                        if (searchResponse != null)
                        {
                            Site.Log.Add(LogEntryKind.Debug, "QuotaControl");
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = null;
                                        if (attributeString.ToLower(CultureInfo.InvariantCulture) == "ntSecurityDescriptor".ToLower(CultureInfo.InvariantCulture))
                                        {
                                            attrbutevalue = attributeVal.ByteArrayValue.ToString();
                                        }
                                        else
                                        {
                                            attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        }
                                        if (attributeString.ToLower(CultureInfo.InvariantCulture) == "msDS-QuotaEffective".ToLower(CultureInfo.InvariantCulture))
                                        {
                                            Site.CaptureRequirement(1365, "When LDAP_SERVER_QUOTA_CONTROL_OID is used with LDAP search operation that queries the constructed attributes msDS-QuotaEffective and msDS-QuotaUsed on the msDS-QuotaContainer object, the server will return the quota of the user who is specified by the control.");
                                        }
                                        if (attributeString.ToLower(CultureInfo.InvariantCulture) == "msDS-QuotaUsed".ToLower(CultureInfo.InvariantCulture))
                                        {
                                            Site.CaptureRequirement(1365, "When LDAP_SERVER_QUOTA_CONTROL_OID is used with LDAP search operation that queries the constructed attributes msDS-QuotaEffective and msDS-QuotaUsed on the msDS-QuotaContainer object, the server will return the quota of the user who is specified by the control.");
                                        }
                                        Site.Log.Add(LogEntryKind.Debug, attributeString + ":" + attrbutevalue);
                                    }
                                }
                            }
                        }

                        #endregion

                        Site.CaptureRequirement(1224, "The LDAP extended control LDAP_SERVER_QUOTA_CONTROL_OID is supported in Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_SHUTDOWN_NOTIFY_OID:
                    #region LDAP_SERVER_SHUTDOWN_NOTIFY_OID

                    Site.CaptureRequirement(1225, "The LDAP extended control LDAP_SERVER_SHUTDOWN_NOTIFY_OID is supported in Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_FORCE_UPDATE_OID:
                    #region LDAP_SERVER_FORCE_UPDATE_OID

                    Site.CaptureRequirement(1226, "The LDAP extended control LDAP_SERVER_FORCE_UPDATE_OID is supported in Windows Server 2008, Windows Server 2008 R2.");

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID:
                    #region LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID

                    if (attributesToBeReturned.Contains("member;range=1-0"))
                    {
                        if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                        {
                            // Ensure that low > high i.e member;range=1-0

                            controls = new DirectoryControl[]{
                                new DirectoryControl(control, null, true, true)
                            };
                            result = adLdapClient.SearchObject(
                                baseObjectDN,
                                ScopeImpSearch,
                                filter,
                                attrsToReturn,
                                controls,
                                out searchResponse,
                                isWindows);
                            Site.CaptureRequirementIfIsTrue(result.ToLower().Contains("success"), 1378, "If this control (LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID) is specified, no error is returned (and no values are returned), if range retrieval is being performed on an attribute whose values are forward link values or back link values, and the value of low is equal the number of values in the attribute.");
                            Site.CaptureRequirementIfIsTrue(result.ToLower().Contains("success"), 1379, "If this control (LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID) is specified, no error is returned (and no values are returned), if range retrieval is being performed on an attribute whose values are forward link values or back link values, and the value of low is greater than to the number of values in the attribute.");
                            Site.Log.Add(LogEntryKind.Debug, "RangeRetrievalNoErrorControl");
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                    (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                    entrypacket.GetInnerRequestOrResponse();
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = null;
                                        if (attributeString.ToLower(CultureInfo.InvariantCulture) == "ntSecurityDescriptor".ToLower(CultureInfo.InvariantCulture))
                                        {
                                            attrbutevalue = attributeVal.Value.ToString();
                                        }
                                        else
                                        {
                                            attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        }
                                        Site.Log.Add(LogEntryKind.Debug, attrbutevalue);
                                    }
                                }
                            }
                            Site.CaptureRequirement(1227, "The LDAP extended control LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID is supported in Windows Server 2008, Windows Server 2008 R2.");
                        }
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_DN_INPUT_OID:
                    #region LDAP_SERVER_DN_INPUT_OID

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                    {
                        string distinguishedName = testUserName;
                        byte[] valueForInputDnControl = BerConverter.Encode("{s}", new object[] { distinguishedName });
                        Utilities.SetControlAcessRights(
                            rootDomainNC,
                            testUserName,
                            currentWorkingDC.Domain.NetbiosName,
                            ControlAccessRight.Read_Only_Replication_Secret_Synchronization,
                            ActiveDirectoryRights.ExtendedRight,
                            AccessControlType.Allow);
                        controls = new DirectoryControl[]{
                                new DirectoryControl(ExtendedControl.LDAP_SERVER_DN_INPUT_OID, valueForInputDnControl, true, true)
                            };
                        result = adLdapClient.SearchObject(
                            rootDomainNC,
                            System.DirectoryServices.Protocols.SearchScope.Subtree,
                            "(objectClass=computer)",
                            new string[] { "msDS-IsUserCachableAtRodc", "cn" },
                            controls,
                            out searchResponse,
                            isWindows);
                        if (searchResponse != null)
                        {
                            Site.Log.Add(LogEntryKind.Debug, "InputDnControl");
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        Site.Log.Add(LogEntryKind.Debug, attributeString + ":" + attrbutevalue);
                                    }
                                }
                            }
                        }
                        Site.CaptureRequirement(1229, "The LDAP extended control LDAP_SERVER_INPUT_DN_OID is supported in Windows Server 2008, Windows Server 2008 R2.");
                    }

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID:
                    #region LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID

                    controls = new DirectoryControl[] {
                        new DirectoryControl(control, null, false, true)};
                    result = adLdapClient.SearchObject(
                        baseObjectDN,
                        ScopeImpSearch,
                        filter,
                        attrsToReturn,
                        controls,
                        out searchResponse,
                        isWindows);

                    controls = new DirectoryControl[] {
                        new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, new byte[1] { 1 }, false, true)};
                    result = adLdapClient.SearchObject(
                        baseObjectDN,
                        ScopeImpSearch,
                        filter,
                        attrsToReturn,
                        controls,
                        out searchResponse,
                        isWindows);

                    #endregion
                    break;
                case ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID:
                    #region LDAP_SERVER_SHOW_RECYCLED_OID Search operation

                    if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                    {
                        Site.Log.Add(LogEntryKind.Debug, "Deleted Objects");
                        string dnOfDeletedObject = string.Empty;
                        controls = new DirectoryControl[] {
                            new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID, null, true, true)
                        };
                        result = adLdapClient.SearchObject(
                            baseObjectDN,
                            ScopeImpSearch,
                            filter,
                            attrsToReturn,
                            controls,
                            out searchResponse,
                            isWindows);
                        List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                        foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                responselist.IndexOf(entrypacket) + 1 + "," + distinguishedName);
                            dnOfDeletedObject = distinguishedName;
                            //Presence of "deleted objects" in the distinguished name indicates that the object resides in tombstone.
                            string scopeDN = forestScopePartialDN + ',' + configurationNC;
                            string featureDN = recycleBinPartialDN + ',' + configurationNC;
                            if (Utilities.IsOptionalFeatureEnabled(scopeDN, featureDN))
                            {
                                Site.CaptureRequirementIfIsTrue(dnOfDeletedObject.ToLower(CultureInfo.InvariantCulture).Contains("deleted objects".ToLower(CultureInfo.InvariantCulture)), 4644, "[When the Recycle Bin optional feature is not enabled] A tombstone is returned by a LDAP Search request with extended control LDAP_SERVER_SHOW_RECYCLED_OID.");
                                break;
                            }
                        }
                    }

                    #endregion
                    break;
                case null:
                default:
                    #region No Extended Control or Normal Search operation

                    result = adLdapClient.SearchObject(
                        baseObjectDN,
                        ScopeImpSearch,
                        filter,
                        attrsToReturn,
                        null,
                        out searchResponse,
                        isWindows);

                    #endregion
                    break;
            }

            #endregion

            if (searchResponse != null)
            {
                bool isEnabled = true;
                string ncName = string.Empty;
                if (baseObjectDN != null)
                {
                    //Validation for all the requirements which do not involve Extended Controls.
                    if (null == control)
                    {
                        if (service == ADImplementations.AD_DS)
                        {
                            #region objectSid

                            #region Objects in Domain NC

                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguishedName.ToLower(CultureInfo.InvariantCulture).Contains("Users".ToLower(CultureInfo.InvariantCulture)))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "objectSid".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                byte[] byteVal = attributeVal.ByteArrayValue;
                                                sidOftheOldObject = new SecurityIdentifier(byteVal, 0);
                                                Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 625, "In AD/DS, if the object is a security principal (according to its objectClass values), then for originating updates the objectSid value is generated and set on the object.");
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Computer objects

                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguishedName.ToLower(CultureInfo.InvariantCulture).Contains("Computers".ToLower(CultureInfo.InvariantCulture)))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "objectSid".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                byte[] byteVal = attributeVal.ByteArrayValue;
                                                sidOftheOldObject = new SecurityIdentifier(byteVal, 0);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #endregion
                        }

                        #region objectCategory

                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                          (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                          entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            if (distinguishedName.ToLower(CultureInfo.InvariantCulture).Contains("Users".ToLower(CultureInfo.InvariantCulture)))
                            {
                                if (distinguishedName.ToLower(CultureInfo.InvariantCulture) == ("CN=krbtgt,CN=Users," + rootDomainNC).ToLower(CultureInfo.InvariantCulture))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "sAMAccountName".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 189, "When an LDAP search filter F contains a clause C of the form \"(objectCategory=V)\", if V is not a DN but there exists an object O such that O!objectClass = classSchema and O!lDAPDisplayName = V, then the server treats the search filter as if clause C was replaced in F with the clause \"(objectCategory=V')\", where V' is O!defaultObjectCategory.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        if (service == ADImplementations.AD_DS)
                        {
                            #region CheckGcPort

                            isConnected = Utilities.CheckPorts("GC", currentWorkingDC.FQDN, "3268");
                            Site.CaptureRequirementIfIsTrue(isConnected, 206, "If the AD/DS DC is a GC server, it accepts LDAP connections for GC access on port 3268.");
                            isConnected = Utilities.CheckPorts(
                                "LDAP",
                                currentWorkingDC.FQDN,
                                "389");
                            Site.CaptureRequirementIfIsTrue(isConnected, 205, "An AD/DS DC accepts LDAP connections on the standard LDAP port: 389.");

                            #endregion
                        }

                        #region MatchingRules

                        #region And matching rule

                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                          (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                          entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            if (distinguishedName.ToLower(CultureInfo.InvariantCulture).Contains("Users".ToLower(CultureInfo.InvariantCulture)))
                            {
                                if (distinguishedName.ToLower(CultureInfo.InvariantCulture) == ($"CN={DomainAdministratorName},CN=Users,{rootDomainNC}").ToLower(CultureInfo.InvariantCulture))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "sAMAccountName".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                //The And Matching Rule is sent as part of filter from Cord which includes user as objectClass.
                                                //Expected is whether the AND Matching Rule is giving correct results.
                                                //Output: Enabled users in Active Directory for which we are checking for the existence of SamAccountName.
                                                Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 457, "Microsoft Windows 2000 operating system and Windows Server 2003 operating system support the LDAP_MATCHING_RULE_BIT_AND and LDAP_MATCHING_RULE_BIT_OR matching rules.");
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                                {
                                                    Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 462, "Windows Server 2008 operating system, Windows Server 2008 R2 operating system, Windows Server 2012 operating system, and Windows Server 2012 R2 operating system support LDAP_MATCHING_RULE_BIT_AND and LDAP_MATCHING_RULE_BIT_OR and the LDAP_MATCHING_RULE_TRANSITIVE_EVAL rule, in AD/LDS.");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region OR matching rule

                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                          (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                          entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            if (distinguishedName.ToLower(CultureInfo.InvariantCulture) == ("CN=Allowed-Attributes,CN=Schema,CN=Configuration," + rootDomainNC).ToLower(CultureInfo.InvariantCulture))
                            {
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        if (attributeString.ToLower(CultureInfo.InvariantCulture) == "lDAPDisplayName".ToLower(CultureInfo.InvariantCulture))
                                        {
                                            //The OR Matching Rule is sent as part of filter from Cord which expects only the Constructed Attributes.
                                            //Expected is whether the OR Matching Rule is giving correct results.
                                            //Check on Allowed-Attributes(Constructed Attributes) will validate the requirement.
                                            Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 457, "Microsoft Windows 2000 operating system and Windows Server 2003 operating system support the LDAP_MATCHING_RULE_BIT_AND and LDAP_MATCHING_RULE_BIT_OR matching rules.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                            {
                                                Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 462, "Windows Server 2008 operating system, Windows Server 2008 R2 operating system, Windows Server 2012 operating system, and Windows Server 2012 R2 operating system support LDAP_MATCHING_RULE_BIT_AND and LDAP_MATCHING_RULE_BIT_OR and the LDAP_MATCHING_RULE_TRANSITIVE_EVAL rule, in AD/LDS.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region TransitiveRule

                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                          (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                          entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            if (distinguishedName.ToLower(CultureInfo.InvariantCulture).Contains("Users".ToLower(CultureInfo.InvariantCulture)))
                            {
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        if (attributeString.ToLower(CultureInfo.InvariantCulture) == "sAMAccountName".ToLower(CultureInfo.InvariantCulture))
                                        {
                                            if (service == ADImplementations.AD_DS)
                                            {
                                                Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 461, "Windows Server 2008 operating system, Windows Server 2008 R2 operating system, Windows Server 2012 operating system, and Windows Server 2012 R2 operating system support LDAP_MATCHING_RULE_BIT_AND and LDAP_MATCHING_RULE_BIT_OR and the LDAP_MATCHING_RULE_TRANSITIVE_EVAL rule, in AD/DS.");
                                            }
                                            if (service == ADImplementations.AD_LDS)
                                            {
                                                Site.CaptureRequirementIfAreNotEqual<string>(string.Empty, attrbutevalue, 462, "Windows Server 2008 operating system, Windows Server 2008 R2 operating system, Windows Server 2012 operating system, and Windows Server 2012 R2 operating system support LDAP_MATCHING_RULE_BIT_AND and LDAP_MATCHING_RULE_BIT_OR and the LDAP_MATCHING_RULE_TRANSITIVE_EVAL rule, in AD/LDS.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #endregion

                        if (service == ADImplementations.AD_DS)
                        {
                            #region ModifyOperationRelated

                            #region dnsHostName

                            bool isSamAccountFilled = false, isDnsHostNameFilled = false;
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguishedName.ToLower(CultureInfo.InvariantCulture).Contains("Computers".ToLower(CultureInfo.InvariantCulture)))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    string samaccountName = null;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "sAMAccountName".ToLower(CultureInfo.InvariantCulture) &&
                                                    isSamAccountFilled == false)
                                            {
                                                isSamAccountFilled = true;
                                                samaccountName = attrbutevalue;
                                            }
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "dNSHostName".ToLower(CultureInfo.InvariantCulture) &&
                                                isDnsHostNameFilled == false)
                                            {
                                                isDnsHostNameFilled = true;
                                                if (!string.IsNullOrEmpty(samaccountName))
                                                {
                                                    Site.CaptureRequirementIfAreEqual<string>((samaccountName.Replace("$", "") + '.' + PrimaryDomainDnsName).ToLower(CultureInfo.InvariantCulture), attrbutevalue.ToLower(CultureInfo.InvariantCulture), 664, "In AD/DS, the value of the dNSHostName attribute being written is in the following format: computerName.fullDomainDnsName, where computerName is the current sAMAccountName of the object (without the final \"$\" character), and the fullDomainDnsName is the DNS name of the domain NC or one of the values of msDS-AllowedDNSSuffixes on the domain NC (if any) where the object that is being modified is located. This addition check performed for validated writes is on objects of class computer or server (or a subclass of computer or server).");
                                                    samaccountName = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region msDSAdditionalDnsHostName

                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguishedName.ToLower(CultureInfo.InvariantCulture).Contains("Computers".ToLower(CultureInfo.InvariantCulture)))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "msDS-AdditionalDnsHostName".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                string result = attrbutevalue;
                                                if (!(String.IsNullOrEmpty(result)))
                                                {
                                                    char firstChar = result[0];
                                                    bool isStartsWithChar = ((firstChar >= 65 && firstChar <= 90) ||
                                                        (firstChar >= 97 && firstChar <= 122) ||
                                                        (firstChar >= 48 && firstChar <= 57) ||
                                                        (firstChar == 45) ||
                                                        (firstChar == 95)) ? true : false;
                                                    Site.CaptureRequirementIfIsTrue(isStartsWithChar, 665, "In AD/DS, the value of the msDS-AdditionalDnsHostName attribute being written is in the following format: anyDnsLabel.suffix, where anyDnsLabel is a valid DNS name label, and suffix matches one of the values of msDS-AllowedDNSSuffixes on the domain NC root (if any). This addition check performed for validated writes is on objects of class computer or server (or a subclass of computer or server).");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #endregion
                        }

                        #region CrossRef

                        #region EnabledIsTrue

                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                          (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                          entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            if (distinguishedName.ToLower(CultureInfo.InvariantCulture) ==
                                ("CN=" + PrimaryDomainNetBiosName +
                                ",CN=Partitions,CN=Configuration," +
                                rootDomainNC).ToLower(CultureInfo.InvariantCulture))
                            {
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        if (attributeString.Equals("Enabled", StringComparison.OrdinalIgnoreCase))
                                        {
                                            isEnabled = Convert.ToBoolean(attrbutevalue.ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                                        }
                                        if (service == ADImplementations.AD_DS)
                                        {
                                            if (attributeString.Equals("dnsRoot", StringComparison.OrdinalIgnoreCase)
                                                && isEnabled == true)
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), PrimaryDomainDnsName.ToLower(CultureInfo.InvariantCulture), 939, "If the Enable attribute of the Cross-Ref Objects is not false, in AD/DS the dnsRoot holds the fully qualified DNS name.");
                                            }
                                        }
                                        if (attributeString.ToLower(CultureInfo.InvariantCulture) == "nCName".ToLower(CultureInfo.InvariantCulture)
                                            && isEnabled == true)
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), rootDomainNC.ToLower(CultureInfo.InvariantCulture), 941, "If the Enable attribute of the Cross-Ref Objects is not false, the nCName attribute contains a reference to the NC root corresponding to this crossRef.");
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region EnabledIsFalse

                        //Custom crossref object
                        isEnabled = true;
                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                          (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                          entrypacket.GetInnerRequestOrResponse();
                            string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                            PartialAttributeList attributeNames = entry.attributes;
                            foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                            {
                                string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                AttributeValue[] attributeValList = attribute.vals.Elements;
                                foreach (AttributeValue attributeVal in attributeValList)
                                {
                                    string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                    if (attributeString.ToLower(CultureInfo.InvariantCulture) == "Enabled".ToLower(CultureInfo.InvariantCulture))
                                    {
                                        isEnabled = Convert.ToBoolean(attrbutevalue.ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                                    }
                                    if (attributeString.Equals("nCName", StringComparison.OrdinalIgnoreCase))
                                    {
                                        ncName = attrbutevalue;
                                    }
                                    if (attributeString.Equals("dnsRoot", StringComparison.OrdinalIgnoreCase))
                                    {
                                        dnsRoot = attrbutevalue;
                                        dnsRoot = dnsRoot.Replace("adts88", currentWorkingDC.Domain.NetbiosName);
                                    }
                                }
                            }

                        }
                        if (service == ADImplementations.AD_DS)
                        {

                            #region 937

                            if (isEnabled == false)
                            {
                                //If the "Enable" attribute of the crossRef object is false this requirement is validated.
                                //Here "isEnabled" variable represents the presence of "Enable" attribute.
                                Site.CaptureRequirementIfIsTrue(dnsRoot.ToLower().Contains(currentWorkingDC.Domain.NetbiosName.ToLower()), 937, "If the Enable attribute of the Cross-Ref Objects is false, in AD/DS the dnsRoot holds the DNS name of the DC that will create the root of the NC.");
                            }

                            #endregion

                        }
                        if (isEnabled == false)
                        {
                            bool ncRootExists = Utilities.IsObjectExist(ncName, currentWorkingDC.FQDN, currentPort);
                            Site.CaptureRequirementIfIsFalse(ncRootExists, 936, "If the Enable attribute of the Cross-Ref Objects is false, the crossRef exists but the corresponding NC root does not yet exist.");
                        }

                        #endregion

                        #region dnsRoot

                        if (service == ADImplementations.AD_LDS)
                        {
                            isEnabled = true;
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        if (attributeString.Equals("Enabled", StringComparison.OrdinalIgnoreCase))
                                        {
                                            isEnabled = Convert.ToBoolean(attrbutevalue.ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                                        }
                                        if (attributeString.Equals("dnsRoot", StringComparison.OrdinalIgnoreCase))
                                        {
                                            dnsRoot = attrbutevalue;
                                        }
                                    }
                                }

                            }
                            if (isEnabled)
                            {
                                Site.CaptureRequirementIfAreEqual<string>(string.Empty, dnsRoot, 940, "If the Enable attribute of the Cross-Ref Objects is not false, in AD/LDS the dnsRoot is absent.");
                            }
                        }

                        #endregion

                        #endregion

                        #region LDAPPolicies

                        foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                        {
                            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                          (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                          entrypacket.GetInnerRequestOrResponse();
                            PartialAttributeList attributeNames = entry.attributes;
                            foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                            {
                                string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                AttributeValue[] attributeValList = attribute.vals.Elements;
                                foreach (AttributeValue attributeVal in attributeValList)
                                {
                                    string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                    if (attributeString.ToLower(CultureInfo.InvariantCulture) == "lDAPAdminLimits".ToLower(CultureInfo.InvariantCulture))
                                    {

                                        #region Asserts For LdapPolicies

                                        if (attrbutevalue.Contains("MaxValRange"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxValRange=1500".ToLower(CultureInfo.InvariantCulture), 486, "MaxValRange is the maximum number of values that can be retrieved from a multivalued attribute in a single search request. Default value is 1500.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1434, "The MaxValRange Policy is supported by Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxReceiveBuffer"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxReceiveBuffer=10485760".ToLower(CultureInfo.InvariantCulture), 478, "MaxReceiveBuffer is the maximum size, in bytes, of a request that the server will accept. Default value is 10,485,760 bytes.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1429, "The MaxReceiveBuffer Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxDatagramRecv"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxDatagramRecv=4096".ToLower(CultureInfo.InvariantCulture), 472, "MaxDatagramRecv is the maximum size, in bytes, of a UDP datagram request that a DC will process. Default value is 4096 bytes.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1426, "The MaxDatagramRecv Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxPoolThreads"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxPoolThreads=4".ToLower(CultureInfo.InvariantCulture), 477, "MaxPoolThreads is the maximum number of threads per processor that can work on LDAP requests at the same time. Default value is 4.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1428, "The MaxPoolThreads Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxResultSetSize"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxResultSetSize=262144".ToLower(CultureInfo.InvariantCulture), 484, "MaxResultSetSize is the maximum number of bytes that a DC stores to optimize the individual searches that make up a paged search. Default value is 262144 bytes.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1432, "The MaxResultSetSize Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxTempTableSize"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxTempTableSize=10000".ToLower(CultureInfo.InvariantCulture), 485, "MaxTempTableSize is the maximum number of rows that a DC will create in a temporary database table to hold intermediate results during query processing. Default value is 10,000 rows.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1433, "The MaxTempTableSize Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxQueryDuration"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxQueryDuration=120".ToLower(CultureInfo.InvariantCulture), 482, "MaxQueryDuration is the maximum time, in seconds, that a DC will spend on a single search. Default value is 120.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1431, "The MaxQueryDuration Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxPageSize"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxPageSize=1000".ToLower(CultureInfo.InvariantCulture), 480, "MaxPageSize is the maximum number of objects that are returned in a single search result. Default value is 1000.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1430, "The MaxPageSize Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxNotificationPerConn"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxNotificationPerConn=5".ToLower(CultureInfo.InvariantCulture), 474, "MaxNotificationPerConn is the maximum number of outstanding notification search requests (using the LDAP_SERVER_NOTIFICATION_OID control) that the DC permits on a single connection. Default value is 5.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1427, "The MaxNotificationPerConn Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }

                                        else if (attrbutevalue.Contains("MaxActiveQueries"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxActiveQueries=20".ToLower(CultureInfo.InvariantCulture), 464, "MaxActiveQueries is the maximum number of concurrent LDAP search operations that are permitted to run at the same time on a DC. Default value is 20.");
                                        }
                                        else if (attrbutevalue.Contains("MaxConnIdleTime"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxConnIdleTime=900".ToLower(CultureInfo.InvariantCulture), 470, "MaxConnIdleTime is the maximum time, in seconds, that the client can be idle before the DC closes the connection. Default value is 900.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1425, "The MaxConnIdleTime Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("InitRecvTimeout"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "InitRecvTimeout=120".ToLower(CultureInfo.InvariantCulture), 466, "InitRecvTimeout is the maximum time, in seconds, that a DC waits for the client to send the first request after the DC receives a new connection. Default value is 120.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1423, "The InitRecvTimeout Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxConnections"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxConnections=5000".ToLower(CultureInfo.InvariantCulture), 468, "MaxConnections is the maximum number of simultaneous LDAP connections that a DC will accept. Default value is 5000.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                            {
                                                Site.CaptureRequirement(1424, "The MaxConnections Policy is supported by Microsoft Windows 2000 operating system, Windows Server 2003 operating system, Windows Server 2008 operating system, and Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxResultSetsPerConn"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxResultSetsPerConn=10".ToLower(CultureInfo.InvariantCulture), 104272, "[MaxResultSetsPerConn is the maximum number of individual paged searches per LDAP connection for which a DC will store optimization data. The data that is stored is outside the state model and is implementation-specific.]Default value is 10.");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2008R2)
                                            {
                                                Site.CaptureRequirement(4276, "The MaxResultSetsPerConn Policy is supported by Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MinResultSets"))
                                        {
                                            Site.CaptureRequirementIfAreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MinResultSets=3".ToLower(CultureInfo.InvariantCulture), 104274, "[MinResultSets is the minimum number of individual paged searches for which a DC will store optimization data. The data that is stored is outside the state model and is implementation-specific.]Default value is 3");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2008R2)
                                            {
                                                Site.CaptureRequirement(4277, "The MinResultSets Policy is supported by Windows Server 2008 R2 operating system.");
                                            }
                                        }
                                        else if (attrbutevalue.Contains("MaxBatchReturnMessages"))
                                        {
                                            Site.Assert.AreEqual<string>(attrbutevalue.ToLower(CultureInfo.InvariantCulture), "MaxBatchReturnMessages=1100".ToLower(CultureInfo.InvariantCulture), "[MaxBatchReturnMessages is the maximum number of messages that can be returned when processing an LDAP_SERVER_BATCH_REQUEST_OID extended operation.]Default value is 1100");
                                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2012)
                                            {
                                                Site.Log.Add(LogEntryKind.Checkpoint, "The MaxBatchReturnMessages Policy is supported by Windows Server 2012 and Windows Server 2012 R2 operating system.");
                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }

                        }
                        #endregion

                        #region ConfigurableSettings

                        if (service == ADImplementations.AD_DS)
                        {

                            #region DefaultValue Requirements AD/DS

                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        #region Asserts

                                        if (attributeString.Equals("msDS-Other-Settings", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (attrbutevalue.ToLower(CultureInfo.InvariantCulture).Contains("DisableVLVSupport".ToLower(CultureInfo.InvariantCulture)))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "DisableVLVSupport=0", 1456, "If the DisableVLVSupport setting is not specified, it defaults to 0.");
                                            }
                                            if (attrbutevalue.ToLower(CultureInfo.InvariantCulture).Contains("DynamicObjectDefaultTTL".ToLower(CultureInfo.InvariantCulture)))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "DynamicObjectDefaultTTL=86400", 1453, "The value of DynamicObjectDefaultTTL is in seconds and defaults to 86400.");
                                            }
                                            if (attrbutevalue.ToLower(CultureInfo.InvariantCulture).Contains("DynamicObjectMinTTL".ToLower(CultureInfo.InvariantCulture)))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "DynamicObjectMinTTL=900", 1455, "The value of DynamicObjectMinTTL is in seconds and defaults to 900.");
                                            }
                                        }

                                        #endregion
                                    }
                                }

                            }
                            #endregion
                        }
                        if (service == ADImplementations.AD_LDS)
                        {

                            #region DefaultValue Requirements on AD/LDS

                            adLdapClient.SearchObject(
                                configurationNC,
                                System.DirectoryServices.Protocols.SearchScope.Subtree,
                                "(ObjectClass=*)",
                                new string[] { "msDS-Other-Settings" },
                                null,
                                out searchResponse);
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguishedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        #region Asserts

                                        if (attributeString.Equals("msDS-Other-Settings", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (attrbutevalue.ToLower(CultureInfo.InvariantCulture).Contains("DisableVLVSupport".ToLower(CultureInfo.InvariantCulture)))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                                {
                                                    Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "DisableVLVSupport=0", 1443, "The DisableVLVSupport LDAP Configurable Setting is supported by Windows Server 2003 operating system with Service Pack 1 (SP1), Windows Server 2008 operating system AD DS, Windows Server 2008 AD LDS, Windows Server 2008 R2 operating system AD DS, and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("DynamicObjectDefaultTTL"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                                {
                                                    Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "DynamicObjectDefaultTTL=86400", 1441, "The DynamicObjectDefaultTTL LDAP Configurable Setting is supported by Windows Server 2003 operating system, Windows Server 2003 operating system with Service Pack 1 (SP1), Windows Server 2008 operating system AD DS, Windows Server 2008 AD LDS, Windows Server 2008 R2 operating system AD DS, and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("DynamicObjectMinTTL"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                                                {
                                                    Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "DynamicObjectMinTTL=900", 1442, "The DynamicObjectMinTTL LDAP Configurable Setting is supported by Windows Server 2003 operating system, Windows Server 2003 operating system with Service Pack 1 (SP1), Windows Server 2008 operating system AD DS, Windows Server 2008 AD LDS, Windows Server 2008 R2 operating system AD DS, and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("MaxReferrals"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                                {
                                                    Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "MaxReferrals=3", 505, "MaxReferrals specifies the maximum number of LDAP URLs that the DC will include in a referral or continuation reference. The default value is 3.");
                                                    Site.CaptureRequirement(1447, "The MaxReferrals LDAP Configurable Setting is supported by Windows Server 2008 AD LDS, Windows Server 2008 R2 operating system AD DS, and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("ReferralRefreshInterval"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                                {
                                                    Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "ReferralRefreshInterval=5", 1448, "The ReferralRefreshInterval LDAP Configurable Setting is supported by Windows Server 2008 AD DS, Windows Server 2008 R2 operating system AD DS, and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("SelfReferralsOnly"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                                {
                                                    Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "SelfReferralsOnly=0", 1449, "The SelfReferralsOnly LDAP Configurable Setting is supported by Windows Server 2008 AD DS, Windows Server 2008 R2 operating system AD DS, and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("ADAMDisableLogonAuditing"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                                {
                                                    Site.CaptureRequirement(1444, "The ADAMDisableLogonAuditing LDAP Configurable Setting is supported by Windows Server 2008 AD LDS and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("ReferralRefreshInterval"))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "ReferralRefreshInterval=5", 1460, "The default value of ReferralRefreshInterval is 5.");
                                            }
                                            if (attrbutevalue.Contains("ADAMDisableSPNRegistration"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                                {
                                                    Site.CaptureRequirement(1446, "The ADAMDisableSPNRegistration LDAP Configurable Setting is supported by Windows Server 2008 AD LDS and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("ADAMDisablePasswordPolicies"))
                                            {
                                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                                                {
                                                    Site.CaptureRequirement(1445, "The ADAMDisablePasswordPolicies LDAP Configurable Setting is supported by Windows Server 2008 AD LDS and Windows Server 2008 R2 AD LDS.");
                                                }
                                            }
                                            if (attrbutevalue.Contains("ADAMDisableLogonAuditing"))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "ADAMDisableLogonAuditing=0", 1457, "If ADAMDisableLogonAuditing is not specified, it defaults to 0.");
                                            }
                                            if (attrbutevalue.Contains("ReferralRefreshInterval"))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "ReferralRefreshInterval=5", 1460, "The default value of ReferralRefreshInterval is 5.");
                                            }
                                            if (attrbutevalue.Contains("ADAMDisableSPNRegistration"))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "ADAMDisableSPNRegistration=0", 499, "If ADAMDisableSPNRegistration is not explicitly specified, it defaults to 0.");
                                            }
                                            if (attrbutevalue.Contains("ADAMDisablePasswordPolicies"))
                                            {
                                                Site.CaptureRequirementIfAreEqual<string>(attrbutevalue, "ADAMDisablePasswordPolicies=0", 496, "If ADAMDisablePasswordPolicies is not explicitly specified, it defaults to 0.");
                                            }
                                        }

                                        #endregion
                                    }
                                }

                            }
                            #endregion
                        }

                        #endregion

                        #region SearchOnMultiValued Attributes

                        // Performing range retrieval.
                        // The first case is validated in all our search requests because we did not provide any range option control for those requests. So here we validated the second part i.e with range option. So we provided range 0-2 which will return 0 to 2 records.
                        if (attributesToBeReturned.Contains("(member;range=0-2)"))
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 172, "If a SearchRequest does not contain a range option for a given attribute, but that attribute has too many values to be returned at one time, the server returns a SearchResultEntry containing (1) the attribute requested without the range option and with no values, and (2) the attribute requested with a range option attached and with the values corresponding to that range option.");
                                PartialAttributeList attributeNames = entry.attributes;
                                foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                {
                                    string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                    AttributeValue[] attributeValList = attribute.vals.Elements;
                                    foreach (AttributeValue attributeVal in attributeValList)
                                    {
                                        string attrbutevalue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                        Site.Log.Add(LogEntryKind.Debug, attrbutevalue);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region SearchUsingDifferentDns

                        #region GuidDnForm

                        if (baseObjectDN.Contains("<GUID"))
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 1002, "The first alternative form of DN that can be used to specify baseObject in SearchRequest is <GUID=object_guid> where object_guid is a GUID that corresponds to the value of the objectGUID attribute of the object being specified.");
                                Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 1004, "All DCs support object_guid expressed as the hexadecimal representation of the binary form of a GUID ([MS-DTYP] section 2.3.2).");
                            }
                        }

                        #endregion

                        #region GuidSidForm

                        if (baseObjectDN.Contains("<SID="))
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Site.CaptureRequirementIfIsTrue((searchResponse.Count > 0), 1007, "The second alternative form of DN that can be used to specify baseObject in SearchRequest is in the format <SID=sid> where sid is the security identifier (SID) that corresponds to the value of the objectSid attribute of the object being specified.");
                            }
                        }

                        #endregion

                        #region GuidWkguidForm

                        if (baseObjectDN.Contains("<WKGUID="))
                        {
                            bool isUserPresent = false;
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguisdedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguisdedName.ToLower(CultureInfo.InvariantCulture) == ($"CN={DomainAdministratorName},CN=Users,{rootDomainNC}").ToLower(CultureInfo.InvariantCulture))
                                {
                                    isUserPresent = true;
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "distinguishedName".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                Site.CaptureRequirementIfIsTrue(isUserPresent, 1011, "The third alternative form of DN that can be used to specify baseObject in SearchRequest is in the format <WKGUID=guid, object_DN> where guid is a GUID expressed as the hexadecimal representation of the binary form of the GUID.");
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #endregion

                        #region Defunct(objectClass:*)

                        // Forest functional level should be at least 2k3
                        if (forestFunctionalLevel >= 2)
                        {
                            bool isDefunctClass = false;
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguisdedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguisdedName.ToLower(CultureInfo.InvariantCulture) == ("CN=DefunctClass3,CN=Schema,CN=Configuration," + rootDomainNC).ToLower(CultureInfo.InvariantCulture))
                                {
                                    isDefunctClass = true;
                                    Site.CaptureRequirementIfIsTrue(isDefunctClass, 514, "If the forest functional level is DS_BEHAVIOR_WIN2003 or greater, instances of a defunct class can be read using the filter term (objectClass=*). ");
                                    break;
                                }
                            }
                        }

                        #endregion

                        #region Defunct(GettingOidInsteadOfLdapDisplayName)

                        // Forest functional level should be at least 2k3
                        // When any attribute is part of mayContain of any ClassSchema object and we made the attribute and the 
                        // Class schema object as defunct, After that if we query mayContain of Defunct Class schema object
                        // mayContain will contain Oids of corresponding attributes instead of ldapDisplayNames.

                        if (forestFunctionalLevel >= 2)
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguisdedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguisdedName.ToLower(CultureInfo.InvariantCulture) == ("CN=AdtsFirstClass,CN=Schema,CN=Configuration," + rootDomainNC).ToLower(CultureInfo.InvariantCulture))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "mayContain".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                // Partial verification only.
                                                string oidFormValue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                                Site.CaptureRequirementIfIsTrue(oidFormValue.Contains("."), 1471, "If the forest functional level is DS_BEHAVIOR_WIN2003 or greater, when reading any OID-valued attribute that contains identifiers for schema objects, if the attribute identifies a defunct schema object, the read returns an OID (the attributeID if an attribute, the governsID if a class) not a name (the lDAPDisplayName of an attribute or class). This behavior applies to the mayContain attribute of schema objects.");
                                                Site.CaptureRequirementIfIsTrue(oidFormValue.Contains("."), 1469, "If the forest functional level is DS_BEHAVIOR_WIN2003 or greater, when reading any OID-valued attribute that contains identifiers for schema objects, if the attribute identifies a defunct schema object, the read returns an OID (the attributeID if an attribute, the governsID if a class) not a name (the lDAPDisplayName of an attribute or class). This behavior applies to the mustContain, systemMustContain, mayContain, systemMayContain, subClassOf, auxiliaryClass, and possSuperiors attributes of schema objects.");
                                                Site.CaptureRequirementIfIsTrue(oidFormValue.Contains("."), 515, "If the forest functional level is DS_BEHAVIOR_WIN2003 or greater, when reading any OID-valued attribute that contains identifiers for schema objects, if the attribute identifies a defunct schema object, the read returns an OID (the attributeID if an attribute, the governsID if a class) not a name (the lDAPDisplayName of an attribute or class). This behavior applies to the mustContain, systemMustContain, mayContain, systemMayContain, subClassOf, auxiliaryClass, and possSuperiors attributes of schema objects. This behavior also applies to the objectClass attribute of all other objects.");
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Defunct(GettingOidInsteadOfLdapDisplayNameForObjectClass)

                        // Forest functional level should be at least 2k3
                        if (forestFunctionalLevel >= 2)
                        {
                            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguisdedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                if (distinguisdedName.ToLower(CultureInfo.InvariantCulture) == ("CN=TestClass1Obj,CN=Users," + rootDomainNC).ToLower(CultureInfo.InvariantCulture))
                                {
                                    PartialAttributeList attributeNames = entry.attributes;
                                    foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                                    {
                                        string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                        AttributeValue[] attributeValList = attribute.vals.Elements;
                                        foreach (AttributeValue attributeVal in attributeValList)
                                        {
                                            if (attributeString.ToLower(CultureInfo.InvariantCulture) == "objectClass".ToLower(CultureInfo.InvariantCulture))
                                            {
                                                //Partial verification only.
                                                string oidFormValue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue).ToLower(CultureInfo.InvariantCulture);
                                                if (!(oidFormValue.ToLower(CultureInfo.InvariantCulture).Contains("user") ||
                                                    oidFormValue.ToLower(CultureInfo.InvariantCulture).Contains("person") ||
                                                    oidFormValue.ToLower(CultureInfo.InvariantCulture).Contains("organizationalperson") ||
                                                    oidFormValue.ToLower(CultureInfo.InvariantCulture).Contains("top")))
                                                {
                                                    Site.CaptureRequirementIfIsTrue(oidFormValue.Contains("."), 1470, "If the forest functional level is DS_BEHAVIOR_WIN2003 or greater, when reading any OID-valued attribute that contains identifiers for schema objects, if the attribute identifies a defunct schema object, the read returns an OID (the attributeID if an attribute, the governsID if a class) not a name (the lDAPDisplayName of an attribute or class). This behavior applies to the objectClass attribute of all objects other than schema objects.");
                                                    Site.CaptureRequirementIfIsTrue(oidFormValue.Contains("."), 515, "If the forest functional level is DS_BEHAVIOR_WIN2003 or greater, when reading any OID-valued attribute that contains identifiers for schema objects, if the attribute identifies a defunct schema object, the read returns an OID (the attributeID if an attribute, the governsID if a class) not a name (the lDAPDisplayName of an attribute or class). This behavior applies to the mustContain, systemMustContain, mayContain, systemMayContain, subClassOf, auxiliaryClass, and possSuperiors attributes of schema objects. This behavior also applies to the objectClass attribute of all other objects.");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region PageRequestControl

                        if (control == ExtendedControl.LDAP_PAGED_RESULT_OID_STRING)
                        {
                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                            {
                                // This is the place where we write our assert why bcz cout is nothing but our pagesize
                                Site.Log.Add(LogEntryKind.Debug, searchResponse.Count.ToString(CultureInfo.InvariantCulture));
                                List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                                foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                                {
                                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                  (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                  entrypacket.GetInnerRequestOrResponse();
                                    string distinguisdedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                    Site.Log.Add(
                                         LogEntryKind.Debug,
                                         responselist.IndexOf(entrypacket) + 1 + "," + distinguisdedName);
                                }
                                //Expected value is set to 2 as the page size considered for this operation is 2.
                                Site.CaptureRequirementIfAreEqual<int>(2, searchResponse.Count, 361, "The use of the LDAP_PAGED_RESULT_OID_STRING control permits clients to perform searches that return more objects than MaxPageSize policy limit by splitting the search into multiple searches, each of which returns no more objects than the MaxPageSize policy limit.");
                                Site.CaptureRequirement(1204, "The LDAP extended control LDAP_PAGED_RESULT_OID_STRING is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                            }
                        }

                        #endregion

                        #region DomainScopeControl

                        if (control == ExtendedControl.LDAP_SERVER_DOMAIN_SCOPE_OID)
                        {
                            if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                            {
                                Site.Log.Add(LogEntryKind.Debug, "DomainScope Control");
                                List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                                foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                                {
                                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                                  (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                                  entrypacket.GetInnerRequestOrResponse();
                                    string distinguisdedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                    Site.Log.Add(
                                         LogEntryKind.Debug,
                                         responselist.IndexOf(entrypacket) + 1 + "," + distinguisdedName);
                                }
                                // Suppose we have 2 domains set up.If we choose Domain Scope option it will return
                                // Objects in one domain to which ldap bind is performed 
                                // And not the objects from the other connected domain.
                                Site.CaptureRequirementIfIsTrue(searchResponse.Count > 0, 382, "The LDAP_SERVER_DOMAIN_SCOPE_OID control is used to instruct the DC not to generate any LDAP continuation references when performing an LDAP operation.");
                                Site.CaptureRequirement(1207, "The LDAP extended control LDAP_SERVER_DOMAIN_SCOPE_OID is supported in Windows 2000, Windows Server 2003, Windows Server 2003 SP1 and Windows Server 2008, Windows Server 2008 R2.");
                            }
                        }

                        #endregion

                        #region ExtendedDNControl

                        #region HexFormat

                        if (control == ExtendedControl.LDAP_SERVER_EXTENDED_DN_OID)
                        {
                            string dnFormat = string.Empty;
                            Site.Log.Add(LogEntryKind.Debug, "ExtendedDnControl: HexFormat");
                            List<AdtsSearchResultEntryPacket> responselist = (List<AdtsSearchResultEntryPacket>)searchResponse;
                            foreach (AdtsSearchResultEntryPacket entrypacket in responselist)
                            {
                                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                              (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                              entrypacket.GetInnerRequestOrResponse();
                                string distinguisdedName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                                Site.Log.Add(
                                     LogEntryKind.Debug,
                                     responselist.IndexOf(entrypacket) + 1 + "," + distinguisdedName);
                                dnFormat = distinguisdedName;
                                break;
                            }
                            bool isContainsGuidSid =
                                dnFormat.ToLower(CultureInfo.InvariantCulture).Contains("GUID".ToLower(CultureInfo.InvariantCulture)) || dnFormat.ToLower(CultureInfo.InvariantCulture).Contains("SID".ToLower(CultureInfo.InvariantCulture));
                            Site.CaptureRequirementIfIsTrue(isContainsGuidSid, 384, "The extended form of an object's DN that is returned by LDAP_SERVER_EXTENDED_DN_OID control includes a string representation of the object's objectGUID attribute; for objects that have an objectSid attribute, the extended form also includes a string representation of that attribute. The extended DN format is <GUID=guid_value>;<SID=sid_value>;dn where guid_value is the value of the object's objectGUID attribute, sid_value is the value of the object's objectSid attribute, and dn is the object's RFC 2253 DN.");
                            Site.CaptureRequirementIfIsTrue(isContainsGuidSid, 1251, "The extended form of an object's DN that is returned by LDAP_SERVER_EXTENDED_DN_OID control includes a string representation of the object's objectGUID attribute; for objects that do not have an objectSid attribute, the extended form is <GUID=guid_value>;dn where guid_value is the value of the object's objectGUID attribute, and dn is the object's RFC 2253 DN.");
                            Site.CaptureRequirement(383, "The LDAP_SERVER_EXTENDED_DN_OID control is used with an LDAP search request to cause the DC to return extended DNs.");
                        }

                        #endregion

                        #endregion
                    }
                }
                else
                {
                    #region QueryRootDSE Attributes

                    SearchRootDSE(service, PDCNetbiosName);
                    foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                    {
                        #region MS-AD_LDAP_R1132

                        searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, "becomeDomainMaster");
                        Site.CaptureRequirementIfIsNull(
                            searchAttrVals,
                            1132,
                            @"An LDAP request to read rootDSE modifiable attributes will be treated as if the attribute does not exist.");

                        #endregion

                        #region MS-AD_LDAP_R210, 1019

                        string expectedValue = string.Empty;
                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.configurationNamingContext, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.configurationNamingContext);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1019,
                                @"configurationNamingContext rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = Utilities.GetAttributeFromEntry(configurationNC, "distinguishedName", currentWorkingDC.FQDN, currentPort, testUserName, testUserPwd).ToString();
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                210,
                                @"An LDAP Request to configurationNamingContext rootDSE attribute returns the DN of the root
                            of the config NC on the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R211, 1020

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.currentTime, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.currentTime);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1020,
                                @"currentTime rootDSE attribute is supported by Windows 2000 and later");
                            #region DateTimeParsing

                            // The format of current time should be 20080209112360.0z
                            // Extracting year,month,date and hours
                            string currentDateTime = searchAttrVals[0].Substring(0, 10);
                            DateTime dateTime = DateTime.Now;
                            dateTime = Convert.ToDateTime(dateTime.ToUniversalTime());
                            string month = dateTime.Month.ToString(CultureInfo.InvariantCulture);
                            if (int.Parse(month, CultureInfo.InvariantCulture) < 10)
                                month = "0" + month;
                            string day = dateTime.Day.ToString(CultureInfo.InvariantCulture);
                            if (int.Parse(day, CultureInfo.InvariantCulture) < 10)
                                day = "0" + day;
                            string hour = dateTime.Hour.ToString(CultureInfo.InvariantCulture);
                            if (int.Parse(hour, CultureInfo.InvariantCulture) < 10)
                                hour = "0" + hour.ToString();
                            string currentTime = dateTime.Year.ToString(CultureInfo.InvariantCulture) + month + day + hour;

                            #endregion
                            Site.CaptureRequirementIfAreEqual<string>(
                                currentTime,
                                currentDateTime,
                                211,
                                @"An LDAP request to currentTime rootDSE attribute returns the current system time on the DC, 
                            as expressed as a string in the Generalized Time format defined by ASN.1 (see [ISO-8601] and [ITUX680], 
                            as well as the documentation for the LDAP String(Generalized-Time) syntax in 3.1.1.2.2.2).");
                        }

                        #endregion

                        #region MS-AD_LDAP_R212, 1021

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.defaultNamingContext, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.defaultNamingContext);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1021,
                                @"defaultNamingContext rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = Utilities.GetAttributeFromEntry(defaultNC, "distinguishedName", currentWorkingDC.FQDN, currentPort, testUserName, testUserPwd).ToString();
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                212,
                                @"An LDAP Request to defaultNamingContext rootDSE attribute returns the DN of the root of the default NC of the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R214, 1022

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.dNSHostName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.dNSHostName);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1022,
                                @"dNSHostName rootDSE attribute is supported by Windows 2000 and later.");
                            Site.CaptureRequirementIfAreEqual<string>(
                                (currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.dNSHostName] : rootDSEAttributesForLds[RootDSEAttribute.dNSHostName])
                                .ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                214,
                                @"An LDAP Request to dNSHostName rootDSE attribute returns the DNS address of the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R215, 1023

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.dsSchemaAttrCount, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.dsSchemaAttrCount);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1023,
                                @"dsSchemaAttrCount rootDSE attribute is supported by Windows 2000 and later.");
                            Site.CaptureRequirementIfAreEqual<string>(
                                (currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.dsSchemaAttrCount] : rootDSEAttributesForLds[RootDSEAttribute.dsSchemaAttrCount]),
                                searchAttrVals[0],
                                215,
                                @"An LDAP Request to dsSchemaAttrCount rootDSE attribute returns an integer specifying the total number
                            of attributes that are defined in the schema.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R216, 1024

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.dsSchemaClassCount, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.dsSchemaClassCount);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1024,
                                @"dsSchemaClassCount rootDSE attribute is supported by Windows 2000 and later.");
                            Site.CaptureRequirementIfAreEqual<string>(
                                (currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.dsSchemaClassCount] : rootDSEAttributesForLds[RootDSEAttribute.dsSchemaClassCount]),
                                searchAttrVals[0],
                                216,
                                @"An LDAP Request to dsSchemaClassCount rootDSE attribute returns an integer specifying the total number
                            of classes that are defined in the schema.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R217, 1025

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.dsSchemaPrefixCount, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.dsSchemaPrefixCount);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1025,
                                @"dsSchemaPrefixCount rootDSE attribute is supported by Windows 2000 and later.");
                            Site.CaptureRequirementIfAreEqual<string>(
                                (currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.dsSchemaPrefixCount] : rootDSEAttributesForLds[RootDSEAttribute.dsSchemaPrefixCount]),
                                searchAttrVals[0],
                                217,
                                @"An LDAP request to dsSchemaPrefixCount rootDSE attribute returns the number of entries in the DC's prefix table:
                            the field prefixTable of the variable dc specified in [MS-DRSR] section 5.18.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R218, 1026

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.dsServiceName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.dsServiceName);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1026,
                                @"dsServiceName rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = Utilities.GetEntryDistinguishedName(currentWorkingDC.FQDN, currentPort, configurationNC, "nTDSDSA", "*", testUserName, testUserPwd);
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                218,
                                @"An LDAP Request to dsServiceName rootDSE attribute returns the DN of the nTDSDSA object for the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R219, 1027

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.highestCommittedUSN, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.highestCommittedUSN);
                            Site.CaptureRequirement(
                                1027,
                                @"highestCommittedUSN rootDSE attribute is supported by Windows 2000 and later.");
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                219,
                                @"An LDAP Request to highestCommittedUSN rootDSE attribute returns the USN of the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R220, 221, 1028

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.isGlobalCatalogReady, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            if (currentService.Equals(ADImplementations.AD_DS))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.isGlobalCatalogReady);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1028,
                                    @"isGlobalCatalogReady rootDSE attribute is supported only by AD DS services for Windows 2000 and later.");
                                Site.CaptureRequirementIfAreEqual<string>(
                                    rootDSEAttributesForDs[RootDSEAttribute.isGlobalCatalogReady].ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    220,
                                    @"An LDAP Request to isGlobalCatalogReady rootDSE attribute returns a Boolean value indicating if this DC 
                                is a global catalog that has completed at least one synchronization of its global catalog data with its 
                                replication partners.");
                                Site.CaptureRequirementIfIsTrue(
                                    Convert.ToBoolean(searchAttrVals[0], CultureInfo.InvariantCulture),
                                    221,
                                    @"An LDAP Request to isGlobalCatalogReady rootDSE attribute returns true, if the DC is a global catalog that 
                                has completed at least one synchronization of its global catalog data with its replication partners.");
                            }
                        }

                        #endregion

                        #region MS-AD_LDAP_R223, 224, 1030

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.isSynchronized, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.isSynchronized);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1030,
                                @"isSynchronized rootDSE attribute is supported by Windows 2000 and later.");
                            Site.CaptureRequirementIfAreEqual<string>(
                                (currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.isSynchronized] : rootDSEAttributesForLds[RootDSEAttribute.isSynchronized])
                                .ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                223,
                                @"An LDAP Request to isSynchronized rootDSE attribute returns a Boolean value indicating if the DC has completed
                            at least one synchronization with its replication partners.");
                            Site.CaptureRequirementIfIsTrue(
                                Convert.ToBoolean(searchAttrVals[0], CultureInfo.InvariantCulture),
                                224,
                                @"An LDAP Request to isSynchronized rootDSE attribute returns true, if the DC has completed at least one 
                            synchronization with its replication partners.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R226, 227, 1031

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.ldapServiceName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            if (currentService.Equals(ADImplementations.AD_DS))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.ldapServiceName);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1031,
                                    @"ldapServiceName rootDSE attribute is supported by Windows 2000 and later.");
                                Site.CaptureRequirementIfAreEqual<string>(
                                    rootDSEAttributesForDs[RootDSEAttribute.ldapServiceName].ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    226,
                                    @"An LDAP Request to ldapServiceName rootDSE attribute returns the LDAP service name for the LDAP server on the DC.");
                                // ldapServicename is generated by the System. So the format will be FullDomainName:DomainControllerName$@FullDomainName
                                Site.CaptureRequirementIfAreEqual<string>(
                                    string.Format("{0}:{1}$@{2}", PrimaryDomainDnsName, currentWorkingDC.NetbiosName, currentWorkingDC.Domain.FQDN).ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    227,
                                    @"The format of the ldapServiceName rootDSE attribute value is <DNS name of the forest root domain>:<Kerberos principal name>,
                                where Kerberos principal name is a string representation of the Kerberos principal name for the DC's computer object.");
                            }
                        }

                        #endregion

                        #region MS-AD_LDAP_R228, 1033

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.namingContexts, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.namingContexts);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1033,
                                @"namingContexts rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.namingContexts] : rootDSEAttributesForLds[RootDSEAttribute.namingContexts];
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                228,
                                @"An LDAP Request to namingContexts rootDSE attribute returns a multi-valued set of DNs. For each NC-replica n hosted
                            on this DC, this attribute contains the DN of the root of n.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R230, 1034

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.netlogon, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            if (currentService.Equals(ADImplementations.AD_DS))
                            {
                                // netlogon: ldap rootDSE search for this attribute will not return searchResultEntries, resolved directly as LDAP ping
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.netlogon);
                                Site.CaptureRequirement(
                                    1034,
                                    @"netlogon rootDSE attribute is supported by AD DS services for Windows 2000 and later.");
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    230,
                                    @"LDAP searches that request the netlogon rootDSE attribute get resolved as LDAP ping operations, as specified in section 6.3.");
                            }
                        }

                        #endregion

                        // pendingPropagations can be variable, will not verify

                        #region MS-AD_LDAP_R232, 1037

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.rootDomainNamingContext, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            if (currentService.Equals(ADImplementations.AD_DS))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.rootDomainNamingContext);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1037,
                                    @"rootDomainNamingContext rootDSE attribute is supported by AD DS services for Windows 2000 and later.");
                                expectedValue = Utilities.GetAttributeFromEntry(rootDomainNC, "distinguishedName", currentWorkingDC.FQDN, currentPort, testUserName, testUserPwd).ToString();
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    232,
                                    @"An LDAP Request to rootDomainNamingContext rootDSE attribute returns the DN of the root domain NC for the DC's forest.");
                            }
                        }

                        #endregion

                        #region MS-AD_LDAP_R233, 1039

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.schemaNamingContext, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.schemaNamingContext);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1039,
                                @"schemaNamingContext rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = Utilities.GetAttributeFromEntry(schemaNC, "distinguishedName", currentWorkingDC.FQDN, currentPort, testUserName, testUserPwd).ToString();
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                233,
                                @"An LDAP Request to schemaNamingContext rootDSE attribute returns the DN of the schema NC on this DC");
                        }

                        #endregion

                        #region MS-AD_LDAP_R234, 1040

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.serverName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.serverName);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1040,
                                @"serverName rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.serverName] : rootDSEAttributesForLds[RootDSEAttribute.serverName];
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                234,
                                @"An LDAP Request to serverName rootDSE attribute returns the DN of the server object, contained in the config NC that represents the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R235, 1041

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.subschemaSubentry, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.subschemaSubentry);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1041,
                                @"subschemaSubentry rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = Utilities.GetEntryDistinguishedName(currentWorkingDC.FQDN, currentPort, schemaNC, "subSchema", "*", testUserName, testUserPwd);
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                235,
                                @"An LDAP Request to subschemaSubentry rootDSE attribute returns the DN for the location of the subSchema object 
                            where the classes and attributes in the directory are defined.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R236, 1042

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.supportedCapabilities, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.supportedCapabilities);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1042,
                                @"supportedCapabilities rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.supportedCapabilities] : rootDSEAttributesForLds[RootDSEAttribute.supportedCapabilities];
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                236,
                                @"An LDAP Request to serverName rootDSE attribute returns the DN of the server object, contained in the config NC that represents the DC.");

                            #region SupportedCapabilites

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_OID)))
                            {
                                Site.CaptureRequirementIfIsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2003,
                                    1412,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_OID LDAP capability is supported by Microsoft Windows 2000 and later.");
                                Site.CaptureRequirementIfIsTrue(
                                    service.Equals(ADImplementations.AD_DS),
                                    450,
                                    @"The presence of LDAP_CAP_ACTIVE_DIRECTORY_OID capability indicates that the LDAP server is running 
                                Microsoft Active Directory and is running as AD DS or AD LDS.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID)))
                            {
                                Site.CaptureRequirementIfIsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2003,
                                    1413,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID LDAP capability is supported by Microsoft Windows Server 2000 Server and later.");
                                Site.CaptureRequirement(
                                    451,
                                    @"The presence of LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID capability indicates that the LDAP server on the DC is 
                                capable of signing and sealing on an NTLM authenticated connection, and that the server is capable of performing 
                                subsequent binds on a signed or sealed connection.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_V51_OID)))
                            {
                                Site.CaptureRequirementIfIsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2003,
                                    1414,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_V51_OID LDAP capability is supported by Windows Server 2003 and later.");
                                Site.CaptureRequirement(
                                    452,
                                    @"The presence of this capability indicates that the LDAP server is running at least the Windows Server 2003
                                operating system version of Active Directory and is running as AD DS or AD LDS.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_ADAM_DIGEST_OID)))
                            {
                                Site.CaptureRequirementIfIsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2008,
                                    1415,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_ADAM_DIGEST LDAP capability is supported by Windows Server 2008 and later");
                                Site.CaptureRequirementIfIsTrue(
                                    service.Equals(ADImplementations.AD_LDS),
                                    453,
                                    @"On a DC operating as AD/LDS, the presence of LDAP_CAP_ACTIVE_DIRECTORY_ADAM_DIGEST capability indicates 
                                that the DC accepts DIGEST-MD5 binds.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_ADAM_OID)))
                            {
                                Site.CaptureRequirementIfIsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2008,
                                    1416,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_ADAM_OID LDAP capability is supported by Windows Server 2008 and later.");
                                Site.CaptureRequirementIfIsTrue(
                                    service.Equals(ADImplementations.AD_LDS),
                                    454,
                                    @"The presence of LDAP_CAP_ACTIVE_DIRECTORY_ADAM_OID capability indicates that the LDAP server is running 
                                Microsoft Active Directory as AD/LDS.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_PARTIAL_SECRETS_OID)))
                            {
                                Site.CaptureRequirementIfIsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2008,
                                    1417,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_PARTIAL_SECRETS_OID LDAP capability is supported by Windows Server 2008 and later.");
                                Site.CaptureRequirementIfIsTrue(
                                    service.Equals(ADImplementations.AD_DS),
                                    455,
                                    @"On an Active Directory DC operating as AD/DS, the presence of LDAP_CAP_ACTIVE_DIRECTORY_PARTIAL_SECRETS_OID 
                                capability indicates that the DC is a RODC.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_V60_OID)))
                            {
                                Site.CaptureRequirementIfIsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2008,
                                    4268,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_V60_OID LDAP capability is supported by Windows Server 2008 and later.");
                                Site.CaptureRequirement(
                                    4269,
                                    @"The presence of this[LDAP_CAP_ACTIVE_DIRECTORY_V60_OID] capability indicates that the LDAP server is running 
                                at least the Windows Server 2008 operating system version of Microsoft Active Directory and is running as AD DS or AD LDS.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_V61_R2_OID)))
                            {
                                Site.Assert.IsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2008R2,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_V61_R2_OID LDAP capability is supported by Windows Server 2008 R2 and later.");
                                Site.CaptureRequirement(
                                    4270,
                                    @"The presence of this[LDAP_CAP_ACTIVE_DIRECTORY_V61_R2_OID] capability indicates that the LDAP server is running 
                                at least the Windows Server 2008 R2 operating system version of Active Directory and is running as AD DS or AD LDS.");
                            }

                            if (searchAttrVals.Exists(x => x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_W8_OID)))
                            {
                                Site.Assert.IsTrue(
                                    currentWorkingDC.OSVersion >= ServerVersion.Win2012,
                                    @"The LDAP_CAP_ACTIVE_DIRECTORY_W8_OID LDAP capability is supported by Windows Server 2012 and later.");
                                Site.Log.Add(
                                    LogEntryKind.Checkpoint,
                                    @"The presence of this[LDAP_CAP_ACTIVE_DIRECTORY_W8_OID] capability indicates that the LDAP server is running 
                                at least the Windows Server 2012 operating system version of Active Directory and is running as AD DS.");
                            }

                            // if there exists on ldap capability that is not listed in [MS-ADTS]
                            if (searchAttrVals.Exists(x => !(x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_V51_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_ADAM_DIGEST_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_ADAM_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_PARTIAL_SECRETS_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_V60_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_V61_R2_OID)
                                || x.Equals(LdapCapability.LDAP_CAP_ACTIVE_DIRECTORY_W8_OID))))
                            {
                                Site.Assert.Fail(string.Format("DC support Ldap Capability that is not listed in [MS-ADTS]."));
                            }

                            #endregion
                        }

                        #endregion

                        #region MS-AD_LDAP_R237,1043

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.supportedControl, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.supportedControl);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1043,
                                @"supportedControl rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.supportedControl] : rootDSEAttributesForLds[RootDSEAttribute.supportedControl];
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                237,
                                @"An LDAP Request to supportedControl rootDSE attribute returns a multivalued set of OIDs specifying the LDAP controls supported by the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R238, 1044

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.supportedLDAPPolicies, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.supportedLDAPPolicies);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1044,
                                @"supportedLDAPPolicies rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.supportedLDAPPolicies] : rootDSEAttributesForLds[RootDSEAttribute.supportedLDAPPolicies];
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                238,
                                @"An LDAP Request to supportedLDAPPolicies rootDSE attribute returns a multivalued set of strings specifying the 
                            LDAP administrative query policies supported by the DC.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R239, 1045, 1098

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.supportedLDAPVersion, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.supportedLDAPVersion);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1045,
                                @"supportedLDAPVersion rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.supportedLDAPVersion] : rootDSEAttributesForLds[RootDSEAttribute.supportedLDAPVersion];
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                239,
                                @"An LDAP Request to supportedLDAPVersion rootDSE attribute returns a set of integers specifying the versions of the 
                            LDAP protocol supported by the DC.");
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                1098,
                                @"An LDAP Request to supportedLDAPVersion rootDSE attribute returns {2,3} as an LDAP multivalue as Active Directory 
                            supports version 2 and version 3 of LDAP.");
                        }

                        #endregion

                        #region MS-AD_LDAP_R240, 1046

                        if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.supportedSASLMechanisms, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.supportedSASLMechanisms);
                            Site.CaptureRequirementIfIsNotNull(
                                searchAttrVals,
                                1046,
                                @"supportedSASLMechanisms rootDSE attribute is supported by Windows 2000 and later.");
                            expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.supportedSASLMechanisms] : rootDSEAttributesForLds[RootDSEAttribute.supportedSASLMechanisms];
                            Site.CaptureRequirementIfAreEqual<string>(
                                expectedValue.ToLower(CultureInfo.InvariantCulture),
                                string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                240,
                                @"An LDAP Request to supportedSASLMechanisms rootDSE attribute returns multivalued set of strings specifying the 
                            security mechanisms supported for SASL negotiation.");
                        }

                        #endregion

                        if (currentWorkingDC.OSVersion >= ServerVersion.Win2003)
                        {
                            #region MS-AD_LDAP_R1047

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.domainControllerFunctionality, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.domainControllerFunctionality);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1047,
                                    @"domainControllerFunctionality rootDSE attribute is supported by Windows 2003 and later.");
                                string nTDSDSAobject = Utilities.GetEntryDistinguishedName(currentWorkingDC.FQDN, currentPort, configurationNC, "nTDSDSA", "*", testUserName, testUserPwd);
                                int dcFunctionality = (int)Utilities.GetAttributeFromEntry(nTDSDSAobject, "msDS-Behavior-Version", currentWorkingDC.FQDN, currentPort, testUserName, testUserPwd);

                                Site.Assert.AreEqual<int>(
                                    dcFunctionality,
                                    int.Parse(searchAttrVals[0]),
                                    @"An LDAP Request to domainControllerFunctionality rootDSE attribute returns an integer indicating the functional level
                                of the DC. This value is populated from the msDS-Behavior-Version attribute on the nTDSDSA object that represents
                                the DC (section 6.1.4.2).");
                            }

                            #endregion

                            #region MS-AD_LDAP_R1049

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.domainFunctionality, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                if (service.Equals(ADImplementations.AD_DS))
                                {
                                    searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.domainFunctionality);
                                    Site.CaptureRequirementIfIsNotNull(
                                        searchAttrVals,
                                        1049,
                                        @"domainControllerFunctionality rootDSE attribute is supported by AD DS services for Windows 2003 and later.");
                                    int domainFunctionality = (int)Utilities.GetAttributeFromEntry(defaultNC, "msDS-Behavior-Version", currentWorkingDC.FQDN, currentPort);

                                    Site.Assert.AreEqual<int>(
                                        domainFunctionality,
                                        int.Parse(searchAttrVals[0]),
                                        @"An LDAP Request to domainFunctionality rootDSE attribute returns an integer indicating the functional level
                                    of the domain. This value is populated from the msDS-Behavior-Version attribute on the domain NC root object
                                    and the crossRef object that represents the domain (section 6.1.4.3).");
                                }
                            }

                            #endregion

                            #region MS-AD_LDAP_R1051

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.forestFunctionality, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.forestFunctionality);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1051,
                                    @"forestFunctionality rootDSE attribute is supported by Windows 2003 and later.");
                                string crossRefContainerDN = Utilities.GetEntryDistinguishedName(currentWorkingDC.FQDN, currentPort, configurationNC, "crossRefContainer", "*", testUserName, testUserPwd);
                                int forestFunctionality = (int)Utilities.GetAttributeFromEntry(crossRefContainerDN, "msDS-Behavior-Version", currentWorkingDC.FQDN, currentPort, testUserName, testUserPwd);

                                Site.Assert.AreEqual<int>(
                                    forestFunctionality,
                                    int.Parse(searchAttrVals[0]),
                                    @"An LDAP Request to forestFunctionality rootDSE attribute returns an integer indicating the functional level
                                of the forest. This value is populated from the msDS-Behavior-Version attribute on the crossRefContainer object (section 6.1.4.4).");
                            }

                            #endregion

                            #region MS-AD_LDAP_R243, 1053

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.msDS_ReplAllInboundNeighbors, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                // requires 2 DCs setup
                                // Connection also need to be taken care here to which dc you need to connect
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.msDS_ReplAllInboundNeighbors);
                                if (searchAttrVals != null)
                                {
                                    Site.CaptureRequirementIfIsNotNull(
                                        searchAttrVals,
                                        1053,
                                        @"msDS-ReplAllInboundNeighbors rootDSE attribute is supported by Windows 2003 and later.");
                                    Site.CaptureRequirementIfIsNotNull(
                                        searchAttrVals,
                                        243,
                                        @"An LDAP request to msDS-ReplAllInboundNeighbors rootDSE attribute returns representations of each value of the repsFrom 
                                        abstract attribute for each NC-replica.");
                                }
                                else
                                {
                                    Site.Log.Add(LogEntryKind.Warning, string.Format("rootDSE attribute {0} is not set", RootDSEAttribute.msDS_ReplAllInboundNeighbors));
                                }
                            }

                            #endregion

                            #region MS-AD_LDAP_R244, 1055

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.msDS_ReplAllOutboundNeighbors, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                // requires 2 DCs setup
                                // Connection also need to be taken care here to which dc you need to connect
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.msDS_ReplAllOutboundNeighbors);
                                if (searchAttrVals != null)
                                {
                                    Site.CaptureRequirementIfIsNotNull(
                                        searchAttrVals,
                                        1055,
                                        @"msDS-ReplAllOutboundNeighbors rootDSE attribute is supported by Windows 2003 and later.");
                                    Site.CaptureRequirementIfIsNotNull(
                                        searchAttrVals,
                                        244,
                                        @"An LDAP request to msDS-ReplAllOutboundNeighbors rootDSE attribute returns representations of each value of the repsTo 
                                        abstract attribute for each NC-replica.");
                                }
                                else
                                {
                                    Site.Log.Add(LogEntryKind.Warning, string.Format("rootDSE attribute {0} is not set", RootDSEAttribute.msDS_ReplAllOutboundNeighbors));
                                }
                            }

                            #endregion

                            #region MS-AD_LDAP_R245,1063

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.msDS_ReplQueueStatistics, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.msDS_ReplQueueStatistics);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1063,
                                    @"msDS-ReplAllOutboundNeighbors rootDSE attribute is supported by Windows 2003 and later.");
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    245,
                                    @"An LDAP request to msDS-ReplQueueStatistics rootDSE attribute returns replication queue statistics.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R246, 247, 1112, 1113, 1114, 1065

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.msDS_TopQuotaUsage, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.msDS_TopQuotaUsage);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1065,
                                    @"msDS-TopQuotaUsage rootDSE attribute is supported by Windows 2003 and later.");
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    245,
                                    @"An LDAP request to msDS-ReplQueueStatistics rootDSE attribute returns replication queue statistics.");
                                Site.CaptureRequirement(
                                    246,
                                    @"An LDAP request to msDS-TopQuotaUsage rootDSE attribute returns a multivalued set of strings specifying the top 10 
                                quota users in all NC-replicas on this DC.");
                                Site.CaptureRequirement(
                                    247,
                                    @"While making LDAP request to msDs-TopQuotaUsage rootDSE attribute, the caller must have the RIGHT_DS_READ_PROPERTY
                                access right on the Quotas container.");
                                Site.CaptureRequirement(
                                    1111,
                                    @"The DC responds to msDS-TopQuotaUsage rootDSE attribute by returning the quota usage for the requested 
                                range of quota users.");

                                if (filter.ToLower(CultureInfo.InvariantCulture) == "msDS-TopQuotaUsage;Range=0-*".ToLower(CultureInfo.InvariantCulture))
                                {
                                    Site.CaptureRequirement(
                                        1112,
                                        @"An attribute specification of the form msDS-TopQuotaUsage;Range=0-* will return the complete list of quota 
                                    usage.");
                                }
                                if (filter.ToLower(CultureInfo.InvariantCulture) == "msDS-TopQuotaUsage;Range=1-9".ToLower(CultureInfo.InvariantCulture))
                                {
                                    Site.CaptureRequirement(
                                        1113,
                                        @"An attribute specification of the form msDS-TopQuotaUsage;Range=1-9 will return the 2nd highest through 
                                    the 10th highest quota usage.");
                                }
                                if (filter.ToLower(CultureInfo.InvariantCulture) == "msDS-TopQuotaUsage;Range=2-2".ToLower(CultureInfo.InvariantCulture))
                                {
                                    Site.CaptureRequirement(
                                        1114,
                                        @"An attribute specification of the form msDS-TopQuotaUsage;Range=2-2 will return the 3rd highest quota usage.");
                                }
                            }

                            #endregion

                            #region MS-AD_LDAP_R249, 1067

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.supportedConfigurableSettings, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.supportedConfigurableSettings);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1067,
                                    @"supportedConfigurableSettings rootDSE attribute is supported by Windows 2003 and later.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.supportedConfigurableSettings] : rootDSEAttributesForLds[RootDSEAttribute.supportedConfigurableSettings];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                    249,
                                    @"An LDAP Request to supportedConfigurableSettings rootDSE attribute returns a multivalued set of strings
                                specifying the configurable settings supported by the DC.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R250, 1069

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.supportedExtension, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.supportedExtension);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1069,
                                    @"supportedExtension rootDSE attribute is supported by Windows 2003 and later.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.supportedExtension] : rootDSEAttributesForLds[RootDSEAttribute.supportedExtension];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                    250,
                                    @"An LDAP Request to supportedExtension rootDSE attribute returns a multivalued set of 
                                OIDs specifying the extended LDAP operations that the DC supports.");

                                #region SupportedExtension

                                if (searchAttrVals.Exists(x => x.Equals(ExtendedOperation.LDAP_SERVER_FAST_BIND_OID)))
                                {
                                    Site.CaptureRequirement(
                                        1402,
                                        @"The LDAP_SERVER_FAST_BIND_OID LDAP extended operation is supported by Windows Server 2003 and later.");
                                }
                                if (searchAttrVals.Exists(x => x.Equals(ExtendedOperation.LDAP_SERVER_START_TLS_OID)))
                                {
                                    Site.CaptureRequirement(
                                        1403,
                                        @"The LDAP_SERVER_START_TLS_OID  LDAP extended operation is supported by Windows Server 2003 and later.");
                                }
                                if (searchAttrVals.Exists(x => x.Equals(ExtendedOperation.LDAP_TTL_REFRESH_OID)))
                                {
                                    Site.CaptureRequirement(
                                        1404,
                                        @"The LDAP_TTL_REFRESH_OID LDAP extended operation is supported by Windows Server 2003 and later.");
                                }
                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2008
                                    && searchAttrVals.Exists(x => x.Equals(ExtendedOperation.LDAP_SERVER_WHO_AM_I_OID)))
                                {
                                    Site.CaptureRequirement(
                                        1405,
                                        @"The LDAP_SERVER_WHO_AM_I_OID LDAP extended operation is supported by Windows Server 2003 and later.");
                                }
                                if (currentWorkingDC.OSVersion >= ServerVersion.Win2012
                                    && searchAttrVals.Exists(x => x.Equals(ExtendedOperation.LDAP_SERVER_BATCH_REQUEST_OID)))
                                {
                                    Site.Log.Add(
                                        LogEntryKind.Checkpoint,
                                        @"The LDAP_SERVER_BATCH_REQUEST_OID LDAP extended operation is supported by Windows Server 2003 and later.");
                                }

                                // if there exists an extendedopertion that is not listed in [MS-ADTS]
                                if (searchAttrVals.Exists(x => !(x.Equals(ExtendedOperation.LDAP_SERVER_FAST_BIND_OID)
                                    || x.Equals(ExtendedOperation.LDAP_SERVER_START_TLS_OID)
                                    || x.Equals(ExtendedOperation.LDAP_TTL_REFRESH_OID)
                                    || x.Equals(ExtendedOperation.LDAP_SERVER_WHO_AM_I_OID)
                                    || x.Equals(ExtendedOperation.LDAP_SERVER_BATCH_REQUEST_OID))))
                                {
                                    Site.Assert.Fail(string.Format("DC supports an extended operation that is not listed in [MS-ADTS]."));
                                }

                                #endregion
                            }

                            #endregion

                            #region MS-AD_LDAP_R251, 1071

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.validFSMOs, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.validFSMOs);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1071,
                                    @"validFSMOs rootDSE attribute is supported by Windows 2003 and later.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.validFSMOs] : rootDSEAttributesForLds[RootDSEAttribute.validFSMOs];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                    251,
                                    @"An LDAP Request to validFSMOs rootDSE attribute returns a set of DNs
                                of objects representing the FSMO roles owned by the DC.");
                            }

                            #endregion
                        }

                        if (currentWorkingDC.OSVersion >= ServerVersion.Win2008)
                        {
                            #region MS-AD_LDAP_R252, 1073, 1120

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.dsaVersionString, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                // Assumption: The user is a member of domain and enterprise admin
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.dsaVersionString);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1073,
                                    @"dsaVersionString rootDSE attribute is supported by Windows 2008 and later.");
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1120,
                                    @"dsaVersionString rootDSE attribute is readable by Domain Administrators (section 7.1.1.6.5) 
                                and Enterprise Administrators (section 7.1.1.6.10) only.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.dsaVersionString] : rootDSEAttributesForLds[RootDSEAttribute.dsaVersionString];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    252,
                                    @"An LDAP Request to dsaVersionString rootDSE attribute returns a string indicating the version of 
                                Active Directory running on the DC.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R254, 1075

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.msDS_PortLDAP, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.msDS_PortLDAP);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1075,
                                    @"msDS-PortLDAP rootDSE attribute is supported by Windows 2008 and later.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.msDS_PortLDAP] : rootDSEAttributesForLds[RootDSEAttribute.msDS_PortLDAP];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    254,
                                    @"For AD/DS an LDAP request to msDS-PortLDAP rootDSE attribute always returns 389 as the TCP/UDP port number.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R255, 256, 1077

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.msDS_PortSSL, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.msDS_PortSSL);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1077,
                                    @"msDS-PortSSL rootDSE attribute is supported by Windows 2008 and later.");
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    255,
                                    @"An LDAP request to msDS-PortSSL rootDSE attribute returns the integer TCP/UDP port number on which the DC 
                                is listening for TLS/SSL-protected LDAP requests.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.msDS_PortSSL] : rootDSEAttributesForLds[RootDSEAttribute.msDS_PortSSL];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    256,
                                    @"For AD/DS an LDAP Request to msDS-PortSSL rootDSE attribute always returns 636 as the TCP/UDP port number.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R257, 258, 1079

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.msDS_PrincipalName, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.msDS_PrincipalName);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1079,
                                    @"msDS-PrincipalName rootDSE attribute is supported by Windows 2008 and later.");
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    257,
                                    @"An LDAP Request to msDS-PrincipalName rootDSE attribute returns a string name of the security principal 
                                that has authenticated on the LDAP connection.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.msDS_PrincipalName] : rootDSEAttributesForLds[RootDSEAttribute.msDS_PrincipalName];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    258,
                                    @"While making LDAP Request to msDS-PrincipalName rootDSE attribute, if the client authenticated as a 
                                Microsoft Windows security principal, the string contains either (1) the NetBIOS domain name, followed by a 
                                backslash ('\'), followed by the sAMAccountName of the security principal, or (2) the SID of the security principal,
                                in SDDL SID string format.");
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    259,
                                    @"While making LDAP Request to msDS-PrincipalName rootDSE attribute, if the client authenticated as an 
                                AD/LDS security principal, the string contains the DN of the security principal.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R261, 1123-1128, 1081

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.serviceAccountInfo, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.serviceAccountInfo);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1081,
                                    @"serviceAccountInfo rootDSE attribute is supported by Windows 2008 and later.");

                                if (searchAttrVals.Exists(x => x.Contains("replAuthenticationMode")))
                                {
                                    Site.CaptureRequirement(
                                        1123,
                                        @"serviceAccountInfo rootDSE attribute returns the name-value pair where name is replAuthenticationMode and
                                    the value is the value of the msDS-ReplAuthenticationMode attribute on the root of the config NC, or 1 
                                    if that attribute is not set.");
                                }
                                if (searchAttrVals.Exists(x => x.Contains("accountType")))
                                {
                                    Site.CaptureRequirement(
                                        1124,
                                        @"serviceAccountInfo rootDSE attribute returns the name-value pair where name is accountType and the value is 
                                    if the service account is a domain account, the value is domain. Otherwise if the service account is a local 
                                    account, and the value is local.");
                                }
                                if (searchAttrVals.Exists(x => x.Contains("systemAccount")))
                                {
                                    Site.CaptureRequirement(
                                        1125,
                                        @"serviceAccountInfo rootDSE attribute returns the name-value pair where name is systemAccount and the value is 
                                    if the service account is a system account (meaning it has one of the SIDs SID S-1-5-20 and S-1-5-18) the value
                                    is true; otherwise the value is false.");
                                }
                                if (searchAttrVals.Exists(x => x.Contains("domainType")))
                                {
                                    Site.CaptureRequirement(
                                        1126,
                                        @"serviceAccountInfo rootDSE attribute returns the name-value pair where name is domainType and the value is 
                                    if the DC is running on a computer that is part of an Active Directory domain (always the case for an AD DS DC), 
                                    the value is domainWithKerb.");
                                }
                                if (searchAttrVals.Exists(x => x.Contains("machineDomainName")))
                                {
                                    Site.CaptureRequirement(
                                        1128,
                                        @"serviceAccountInfo rootDSE attribute returns the name-value pair where name is machineDomainName and the values
                                    is if domainType is domainWithKerb or domainNoKerb the value is the NetBIOS name of the domain. 
                                    Otherwise the value is the NetBIOS name of the computer.");
                                }
                                if (searchAttrVals.Exists(x => x.Contains("serviceAccountName")))
                                {
                                    Site.CaptureRequirement(
                                        1127,
                                        @"serviceAccountInfo rootDSE attribute returns the name-value pair where name is serviceAccountName and its value is
                                    SAM name of the DC's service account if the value of replAuthenticationMode is 0. Otherwise this name-value pair 
                                    is not present.");
                                }
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.serviceAccountInfo] : rootDSEAttributesForLds[RootDSEAttribute.serviceAccountInfo];
                                Site.Assert.AreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                    @"serviceAccountInfo rootDSE attribute returns a set of strings, each string containing a name-value pair encoded as name=value");
                            }

                            #endregion

                            #region MS-AD_LDAP_R262, 264, 1129, 1083

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.spnRegistrationResult, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.spnRegistrationResult);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1083,
                                    @"spnRegistrationResult rootDSE attribute is supported by Windows 2008 and later.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.spnRegistrationResult] : rootDSEAttributesForLds[RootDSEAttribute.spnRegistrationResult];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    262,
                                    @"When running as AD/DS, an LDAP Request to spnRegistrationResult rootDSE attribute returns a value 0.");
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    264,
                                    @"When running as AD/LDS, if the DC was unable to register its service principal names (SPNs), an LDAP Request 
                                to spnRegistrationResult rootDSE attribute returns the Windows error code associated with the failure. Otherwise, 
                                it returns zero.");
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    1129,
                                    @"Note: When running as AD DS on Windows Server 2008 operating system and Windows Server 2008 R2 operating system, 
                                this value [in spnRegistrationResult attribute] is 21.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R265, 1085

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.tokenGroups, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.tokenGroups);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1085,
                                    @"tokenGroups rootDSE attribute is supported by Windows 2008 and later.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.tokenGroups] : rootDSEAttributesForLds[RootDSEAttribute.tokenGroups];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    string.Join(";", searchAttrVals).ToLower(CultureInfo.InvariantCulture),
                                    265,
                                    @"An LDAP Request to tokenGroups rootDSE attribute returns the SIDs contained in the security context as which the 
                                client has authenticated the LDAP connection.");
                            }

                            #endregion

                            #region MS-AD_LDAP_R266, 1087

                            if (attrsToReturn.Exists(x => x.Equals(RootDSEAttribute.usnAtRifm, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                // require RODC setup
                                searchAttrVals = adLdapClient.GetAttributeValuesInString(entrypacket, RootDSEAttribute.usnAtRifm);
                                Site.CaptureRequirementIfIsNotNull(
                                    searchAttrVals,
                                    1087,
                                    @"usnAtRifm rootDSE attribute is supported by Windows 2008 and later.");
                                expectedValue = currentService.Equals(ADImplementations.AD_DS) ? rootDSEAttributesForDs[RootDSEAttribute.usnAtRifm] : rootDSEAttributesForLds[RootDSEAttribute.usnAtRifm];
                                Site.CaptureRequirementIfAreEqual<string>(
                                    expectedValue.ToLower(CultureInfo.InvariantCulture),
                                    searchAttrVals[0].ToLower(CultureInfo.InvariantCulture),
                                    266,
                                    @"If the DC is an RODC and was installed using the Install From Media feature, reading the usnAtRifm rootDSE 
                                attribute returns the value of dc.usn that was present in the Active Directory database on the installation media.");
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
                SearchOpResponse(SearchResp.retrievalSuccessful);
            }
            else
            {
                SearchOpResponse(SearchResp.retreivalUnsuccessful);
            }
        }

        /// <summary>
        /// Event triggered when the search response is returned.
        /// </summary>
        public event ResponseHandler SearchOpResponse;

        /// <summary>
        /// Performs Search related extended operations.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name of an object to be searched.</param>
        /// <param name="controls"> The oid or the name of the ExtendedControls</param>
        /// <returns>Result code of the search response.</returns>
        public Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry SearchDeletedObject(
            string distinguishedName,
            string controls)
        {
            ICollection<AdtsSearchResultEntryPacket> searchResponse;
            // set the DN of deleted object
            string oldObjDN = distinguishedName.Replace("DC=adts88", rootDomainNC);
            string objCN = oldObjDN.Split(',')[0].Trim().ToString();
            string objRDN = objCN.Split('=')[1].Trim().ToString();
            string guid = string.Empty;
            if (oldObjDN != "CN=Deleted Objects," + rootDomainNC)
            {
                guid = guidHashTable[oldObjDN].ToString();
            }
            string deletedBaseDN;
            string deletedObjDN;
            string serverName;
            string namingContext;

            if (currentService.Equals(ADImplementations.AD_DS))
            {
                serverName = PDCNetbiosName;
                deletedBaseDN = "CN=Deleted Objects," + rootDomainNC;
                deletedObjDN = objCN + "\\0ADEL:" + guid + "," + deletedBaseDN;
                namingContext = rootDomainNC;
            }
            else
            {
                serverName = PDCNetbiosName + "$" + ADLDSInstanceName;
                deletedBaseDN = "CN=Deleted Objects," + defaultNC;
                deletedObjDN = objCN + "\\0ADEL:" + guid + "," + deletedBaseDN;
                namingContext = defaultNC;
            }

            //Update the maxpagesize to 1000000 in case the search operation can not return the whole results.
            adLdapClient.ModifyLdapPolices(
                serverName,
                namingContext,
                new LdapPolicy[] { LdapPolicy.MaxPageSize },
                new string[] { "1000000" });

            // save controls for verify
            string[] ctrls = controls.Split(';');
            DirectoryControl[] extendedControls = new DirectoryControl[ctrls.Length];
            for (int i = 0; i < ctrls.Length; i++)
            {
                extendedControls[i] = new DirectoryControl(ctrls[i], null, true, true);
            }

            string searchFilter = string.Format("(&(isDeleted=TRUE)(name={0}*))", objRDN);

            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry = null;
            if (oldObjDN == "CN=Deleted Objects," + rootDomainNC)
            {
                adLdapClient.SearchObject(
                    deletedBaseDN,
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    "(objectClass=*)",
                    null,
                    extendedControls,
                    out searchResponse);
            }
            else
            {
                adLdapClient.SearchObject(
                    deletedBaseDN,
                    System.DirectoryServices.Protocols.SearchScope.OneLevel,
                    searchFilter,
                    null,
                    extendedControls,
                    out searchResponse);
            }
            if (searchResponse != null)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    entry = (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                    entrypacket.GetInnerRequestOrResponse();
                    string entryObjectName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue);
                    if (entryObjectName.Equals(deletedObjDN, StringComparison.OrdinalIgnoreCase))
                    {
                        bool isDelete = false;
                        bool isRecycle = false;
                        string objectClass = string.Empty;
                        PartialAttributeList attributeNames = entry.attributes;
                        foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                        {
                            string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                            AttributeValue[] attributeValList = attribute.vals.Elements;
                            foreach (AttributeValue attributeVal in attributeValList)
                            {
                                string attributeValue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);
                                if (attributeString.Equals("isDeleted", StringComparison.OrdinalIgnoreCase))
                                {
                                    isDelete = Convert.ToBoolean(attributeValue.ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                                }
                                if (attributeString.Equals("isRecycled", StringComparison.OrdinalIgnoreCase))
                                {
                                    isRecycle = Convert.ToBoolean(attributeValue.ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                                }
                                if (attributeString.Equals("objectClass", StringComparison.OrdinalIgnoreCase))
                                {
                                    objectClass += attributeValue;
                                }
                            }
                        }
                        VerifyDeletedObjectLink(objectClass, controls, isDelete.ToString(), isRecycle.ToString());
                        break;
                    }
                }
            }

            //Reset the value of MaxPageSize
            adLdapClient.ModifyLdapPolices(
                serverName,
                namingContext,
                new LdapPolicy[] { LdapPolicy.MaxPageSize },
                new string[] { "1000" });

            return entry;
        }

        #endregion

        #region LDAP Unbind Operations

        /// <summary>
        /// Method that takes up the task of maintaining consistent initial state
        /// </summary>
        public void UnBind()
        {
            #region Variables

            ConstrOnModOpErrs errorStatus;

            #endregion

            #region Clean up AD DS objects

            if (currentService.Equals(ADImplementations.AD_DS))
            {
                #region Set isDefunct to false for new added test schema

                ModifyOperation(
                    new Microsoft.Modeling.Map<System.String, Microsoft.Modeling.Sequence<System.String>>()
                    .Add("isDefunct: FALSE", new Microsoft.Modeling.Sequence<System.String>()
                    .Add("distinguishedName: CN=TestClass1,CN=Schema,CN=Configuration,DC=adts88")
                    .Add("isDefunct: TRUE")),
                    Microsoft.Protocols.TestSuites.ActiveDirectory.Common.RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ADImplementations.AD_DS,
                    Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ServerVersion.Win2008R2,
                    false,
                    out errorStatus);
                ModifyOperation(
                    new Microsoft.Modeling.Map<System.String, Microsoft.Modeling.Sequence<System.String>>()
                    .Add("isDefunct: FALSE", new Microsoft.Modeling.Sequence<System.String>()
                    .Add("distinguishedName: CN=TempClass,CN=Schema,CN=Configuration,DC=adts88")
                    .Add("isDefunct: TRUE")),
                    Microsoft.Protocols.TestSuites.ActiveDirectory.Common.RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ADImplementations.AD_DS,
                    Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ServerVersion.Win2008R2,
                    false,
                    out errorStatus);

                #endregion

                #region Remove descriptions for computers container

                //remove the value of attribute "description" 
                List<DirectoryAttributeModification> attribstoModify = new List<DirectoryAttributeModification>();
                DirectoryAttributeModification attribs = new DirectoryAttributeModification();
                attribs.Name = "description";
                attribs.Add("gchjgc");
                attribs.Operation = DirectoryAttributeOperation.Delete;
                attribstoModify.Add(attribs);
                adLdapClient.ModifyObject("CN=Computers," + rootDomainNC, attribstoModify, null);

                #endregion
            }

            #endregion

            #region Clean up cross domain object on Child Domain

            if (!string.IsNullOrEmpty(CDCNetbiosName))
            {
                if (currentWorkingDC.NetbiosName != CDCNetbiosName)
                {
                    adLdapClient.Unbind();
                    currentWorkingDC = GetDomainController(CDCNetbiosName);
                    currentPort = ADDSPortNum;
                    adLdapClient.ConnectAndBind(
                        currentWorkingDC.NetbiosName,
                        currentWorkingDC.IPAddress,
                        int.Parse(currentPort),
                        childAdminName,
                        childAdminPwd,
                        currentWorkingDC.Domain.NetbiosName,
                        AuthType.Basic | AuthType.Kerberos);
                }
                adLdapClient.DeleteObject("CN=CrossDomainUser1,CN=Users," + currentWorkingDC.Domain.DomainNC, null);
                adLdapClient.DeleteObject("CN=CrossDomainUser2,CN=Users," + currentWorkingDC.Domain.DomainNC, null);
                adLdapClient.DeleteObject("CN=CrossDomainUser4,CN=Users," + currentWorkingDC.Domain.DomainNC, null);
                adLdapClient.DeleteObject("CN=CrossDomainUser3,CN=Users," + currentWorkingDC.Domain.DomainNC, null);
                adLdapClient.DeleteObject("CN=CrossDomainGroup2,CN=Users," + currentWorkingDC.Domain.DomainNC, null);
                adLdapClient.DeleteObject("cn=crossmove,cn=users," + currentWorkingDC.Domain.DomainNC, null);
                adLdapClient.Unbind();
            }

            #endregion

            #region Clean up cross domain object on Primary Domain

            if (currentWorkingDC.NetbiosName != PDCNetbiosName)
            {
                adLdapClient.Unbind();
                currentWorkingDC = GetDomainController(PDCNetbiosName);
                currentPort = ADDSPortNum;
                adLdapClient.ConnectAndBind(
                    currentWorkingDC.NetbiosName,
                    currentWorkingDC.IPAddress,
                    int.Parse(currentPort),
                    testUserName,
                    testUserPwd,
                    currentWorkingDC.Domain.NetbiosName,
                    AuthType.Basic | AuthType.Kerberos);
            }
            adLdapClient.DeleteObject("CN=CrossDomainUser1,CN=Users," + rootDomainNC, null);
            adLdapClient.DeleteObject("CN=CrossDomainUser2,CN=Users," + rootDomainNC, null);
            adLdapClient.DeleteObject("CN=CrossDomainUser3,CN=Users," + rootDomainNC, null);
            adLdapClient.DeleteObject("CN=CrossDomainUser4,CN=Users," + rootDomainNC, null);
            adLdapClient.DeleteObject("CN=CrossDomainGroup,CN=Users," + rootDomainNC, null);
            adLdapClient.DeleteObject("CN=CrossDomainGroup1,CN=Users," + rootDomainNC, null);
            adLdapClient.DeleteObject("CN=CrossDomainGroup3,CN=Users," + rootDomainNC, null);
            adLdapClient.DeleteObject("CN=CrossDomainGroup2,CN=Users," + rootDomainNC, null);
            adLdapClient.Unbind();

            #endregion

            // Do not set isConnected = false here, because there are cases that test sending LDAP Add/Modify/Delete operations after unbind
            // set isConnected = false; after using this method
            // isConnected = false;
        }

        #endregion

        #region LDAP Extended Operations

        /// <summary>
        /// Performs an LDAP Extended operation
        /// </summary>
        /// <param name="operation">Specifies an extended operation </param>
        /// <param name="dcLevel">windows version if windows is the platform otherwise represents a non-Windows platform</param>
        /// <param name="service">AD/DS or AD/LDS</param>
        /// <param name="response">Represents the response (Valid or InValid)</param>
        public void LDAPExtendedOperation(
            string operation,
            ServerVersion dcLevel,
            ADImplementations service,
            out ExtendedOperationResponse response)
        {
            #region Connect and Bind

            if (isConnected == false)
            {
                SetConnectAndBind(service, PDCNetbiosName);
            }

            #endregion

            #region LDAP Extended Operations

            //Successful LDAP_SERVER_WHO_AM_I_OID operation case
            if (operation == ExtendedOperation.LDAP_SERVER_WHO_AM_I_OID)
            {
                // Case considering a non-supported case i.e., this operation is not supported before Windows2K3
                if (dcLevel <= ServerVersion.Win2003)
                {
                    response = ExtendedOperationResponse.IsNotSupported;
                }
                else
                {
                    //1.3.6.1.4.1.4203.1.11.3 represents OID of LDAP_SERVER_WHO_AM_I_OID extended operation
                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ExtendedResponse extendedResponse =
                        adLdapClient.ExtendedOperations(ExtendedOperation.LDAP_SERVER_WHO_AM_I_OID, null);
                    Site.CaptureRequirementIfAreEqual<string>(
                        ("u:" + currentWorkingDC.Domain.NetbiosName + "\\" + testUserName).ToLower(CultureInfo.InvariantCulture),
                        (Encoding.UTF8.GetString(extendedResponse.response.ByteArrayValue)).ToLower(CultureInfo.InvariantCulture),
                        1409,
                        "While making LDAP request to LDAP_SERVER_WHO_AM_I_OID control, if the client is authenticated as " +
                        "a Microsoft Windows security principal, the authzId returned in the response will contain the " +
                        "string \"u:\" followed by either (1) the NetBIOS domain name, followed by a backslash (\"\\\"), " +
                        "followed by the sAMAccountName of the security principal, or" +
                        "(2) the SID of the security principal, in SDDL SID string format.");
                    response = ExtendedOperationResponse.Valid;
                }
            }
            //Successful LDAP_TTL_REFRESH_OID operation case
            else if (operation == ExtendedOperation.LDAP_TTL_REFRESH_OID)
            {
                int entryTTL = 1800;
                string parentDN;
                response = ExtendedOperationResponse.InValid;
                if (currentService == ADImplementations.AD_DS)
                {
                    parentDN = "CN=Users," + rootDomainNCForDs;
                }
                else
                {
                    parentDN = "CN=Roles," + LDSApplicationNC;
                }
                string dynUserDN = "CN=" + testDynUserName + "," + parentDN;
                if (Utilities.IsObjectExist(dynUserDN, currentWorkingDC.FQDN, currentPort))
                {
                    Utilities.RemoveUser(currentWorkingDC.FQDN, currentPort, parentDN, testDynUserName);
                }
                try
                {
                    LdapConnection con;
                    if (currentService == ADImplementations.AD_DS)
                    {
                        con = new LdapConnection(
                            new LdapDirectoryIdentifier(PDCIPAddress, int.Parse(currentPort)),
                            new NetworkCredential(DomainAdministratorName,
                                DomainUserPassword,
                                PrimaryDomainDnsName));
                    }
                    else
                    {
                        con = new LdapConnection(
                            new LdapDirectoryIdentifier(PDCIPAddress, int.Parse(currentPort)),
                            new NetworkCredential(testUserName,
                                testUserPwd,
                                PrimaryDomainDnsName));
                    }
                    con.SessionOptions.Sealing = false;
                    con.SessionOptions.Signing = false;
                    ManagedAddRequest addReq = new ManagedAddRequest(dynUserDN);
                    addReq.Attributes.Add(new DirectoryAttribute("objectClass", new string[] { "dynamicObject", "user" }));
                    addReq.Attributes.Add(new DirectoryAttribute("entryTTL", entryTTL.ToString()));
                    if (currentService == ADImplementations.AD_DS)
                    {
                        // this attribute is only mandatory for users in AD DS
                        addReq.Attributes.Add(new DirectoryAttribute("sAMAccountName", testDynUserName));
                    }
                    System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
                    Site.Assert.AreEqual(ResultCode.Success, addRep.ResultCode, "Add dynamic object {0} should success.", dynUserDN);

                    Asn1BerEncodingBuffer encBuf = new Asn1BerEncodingBuffer();
                    TtlRefreshRequestValue ttlRefreshRequestValue = new TtlRefreshRequestValue(
                        Encoding.ASCII.GetBytes(dynUserDN ?? string.Empty),
                        entryTTL * 2);
                    ttlRefreshRequestValue.BerEncode(encBuf, true);
                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ExtendedResponse extendedResponse =
                            adLdapClient.ExtendedOperations(ExtendedOperation.LDAP_TTL_REFRESH_OID, encBuf.Data);
                    Site.Assert.AreEqual(0, extendedResponse.resultCode.Value, "Extended Operation should success.");

                    TtlRefreshResponseValue ttlRefreshResponseValue = new TtlRefreshResponseValue();
                    Asn1DecodingBuffer decBuf = new Asn1DecodingBuffer(extendedResponse.response.ByteArrayValue);
                    ttlRefreshResponseValue.BerDecode(decBuf);
                    if (ttlRefreshResponseValue.responseTtl.Value == (entryTTL * 2))
                    {
                        response = ExtendedOperationResponse.Valid;
                    }
                }
                catch (Exception e)
                {
                    Site.Log.Add(LogEntryKind.Debug, e.Message);
                }
                finally
                {
                    Utilities.RemoveUser(currentWorkingDC.FQDN, currentPort, parentDN, testDynUserName);
                    VerifyRefreshEntryTTL(operation, response);
                }
            }
            //For other ExtendedOperations.
            else
            {
                response = ExtendedOperationResponse.Valid;
            }

            #endregion
        }

        #endregion

        #endregion

        #region Protected Methods

        /// <summary>
        /// ConstructServerHostName method is used to construct the server host name
        /// </summary>
        /// <param name="isRODC">Represent whether it is a RODC or not</param>
        /// <param name="specifiedServer">Represent the server computer short name</param>
        /// <returns>Return the full computer domain name</returns>
        protected string ConstructServerHostName(bool isRODC, string specifiedServer)
        {
            string serverName = string.Empty;
            // Get information <hostname/port> from ftpconfig
            if (isRODC == true)
            {
                if (string.IsNullOrWhiteSpace(RODCNetbiosName))
                    Site.Assert.Fail("Test case requires a RODC but \"RODCName\" ptfconfig property value is invalid");
                serverName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", RODCNetbiosName, PrimaryDomainDnsName);
            }
            else
            {
                if (specifiedServer != null && specifiedServer == "DC=NotPDCFSMO")
                {
                    if (string.IsNullOrWhiteSpace(SDCNetbiosName))
                        Site.Assert.Fail("Test case requires a SDC but \"ADCName\" ptfconfig property value is invalid");
                    serverName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", SDCNetbiosName, PrimaryDomainDnsName);
                }
                else if (specifiedServer != null && specifiedServer == "DC=notWritableDC")
                {
                    if (string.IsNullOrWhiteSpace(RODCNetbiosName))
                        Site.Assert.Fail("Test case requires a RODC but \"RODCName\" ptfconfig property value is invalid");
                    serverName = string.Format("{0}.{1}", RODCNetbiosName, PrimaryDomainDnsName);
                }
                else if (specifiedServer != null && specifiedServer == "DC=WritableDCNotSameDomain")
                {
                    if (string.IsNullOrWhiteSpace(CDCNetbiosName))
                        Site.Assert.Fail("Test case requires a CDC but \"CDCName\" ptfconfig property value is invalid");
                    serverName = string.Format("{0}.{1}", CDCNetbiosName, ChildDomainDnsName);
                }
                else
                {
                    serverName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", PDCNetbiosName, PrimaryDomainDnsName);
                }
            }
            return serverName;
        }

        /// <summary>
        /// Clean up environment
        /// </summary>
        public void CleanUpEnvironment()
        {
            if (!isConnected)
            {
                SetConnectAndBind(ADImplementations.AD_DS, PDCNetbiosName);
            }
            if (isConnected
                && currentService.Equals(ADImplementations.AD_DS))
            {
                List<DirectoryAttributeModification> attrsMod = new List<DirectoryAttributeModification>();
                DirectoryAttributeModification modDcFunctionality = new DirectoryAttributeModification();
                modDcFunctionality.Name = "msds-behavior-version";
                modDcFunctionality.Operation = DirectoryAttributeOperation.Replace;
                modDcFunctionality.Add(dcFunctionality);
                attrsMod.Add(modDcFunctionality);
                adLdapClient.ModifyObject(
                    "cn=ntds settings,cn=" + RODCNetbiosName + ",cn=servers,cn=default-first-site-name,cn=sites," + configurationNC,
                    attrsMod,
                    null);

                DirectoryAttributeModification modDomainFunctionality = new DirectoryAttributeModification();
                modDomainFunctionality.Name = "msds-behavior-version";
                modDomainFunctionality.Operation = DirectoryAttributeOperation.Replace;
                modDomainFunctionality.Add(((uint)DomainFunctionLevel).ToString());
                attrsMod.RemoveAt(0);
                attrsMod.Add(modDomainFunctionality);
                adLdapClient.ModifyObject(
                    rootDomainNC,
                    attrsMod,
                    null);
                adLdapClient.ModifyObject(
                    "cn=partitions," + configurationNC,
                    attrsMod,
                    null);

                DirectoryAttributeModification modDescription = new DirectoryAttributeModification();
                modDescription.Name = "description";
                modDescription.Operation = DirectoryAttributeOperation.Delete;
                attrsMod.RemoveAt(0);
                attrsMod.Add(modDescription);
                adLdapClient.ModifyObject(
                    string.Format("CN={0},CN=Users,{1}", testUserName, rootDomainNC),
                    attrsMod,
                    null);
                adLdapClient.ModifyObject(
                    string.Format("CN={0},CN=Users,{1}", DomainAdministratorName, rootDomainNC),
                    attrsMod,
                    null);

                DirectoryAttributeModification modAdditionDnsHostName = new DirectoryAttributeModification();
                modAdditionDnsHostName.Name = "msDS-AdditionalDnsHostName";
                modAdditionDnsHostName.Operation = DirectoryAttributeOperation.Replace;
                modAdditionDnsHostName.Add(string.Format("{0}.{1}", PDCNetbiosName, PrimaryDomainDnsName));
                attrsMod.RemoveAt(0);
                attrsMod.Add(modAdditionDnsHostName);
                adLdapClient.ModifyObject(
                    string.Format("CN={0},OU=Domain Controllers,{1}", PDCNetbiosName, rootDomainNC),
                    attrsMod,
                    null);

                // The global variables need to be initialized by searching RootDSE: defaultNC, schemaNC, configurationNC, etc.
                adLdapClient.DeleteTreeControl("cn=tmp,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testUser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testsvr,cn=servers,cn=default-first-site-name,cn=sites," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=$site@object,cn=sites," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=10.11.3.0/24,cn=subnets,cn=sites," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=10.14.3.0/24/35,cn=subnets,cn=sites," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=account1,cn=schema," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=acs-resource-limits1,cn=schema," + configurationNC);
                adLdapClient.DeleteTreeControl($"cn={DomainAdministratorName}1,cn=users,{rootDomainNC}");
                adLdapClient.DeleteTreeControl("cn=adtstestsite,cn=sites," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=adtstesttdiclass,cn=schema," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=child,cn=testcontainer1," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=computerscontainer," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup1,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup2,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup3,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser1,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser2,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser3,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser4,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=customcrossref,cn=partitions," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=customgroup,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=delegateuser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=deletecontainer," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=deleteuser2,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=diruser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dupsystemcontainer,cn=system," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dupuser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dupuser1,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dynamicuser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dynamicuser1,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dynamicuser2,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dynamicuserchild,cn=dynamicuser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dyobject,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=dyuser," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=ecd,cn=schema," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=group6750,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=guiduser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=inetuser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=licensing site settings,cn=adtstestsite,cn=sites," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=lostandfound1," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=lstest,cn=schema," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=meetings,cn=adminsdholder,cn=system," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=meetings,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=newclass,cn=schema," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=newcomputer,cn=computers," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=newcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernewcomputernew," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=nonexistentcontainer1,cn=nousers," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=nonexistingcontainer," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=nonexistingobject,cn=computers," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=permissiveuser,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=sample,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=samplecrossref,cn=partitions," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=samplegroup,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=tempobj,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testappnc,cn=partitions," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testclass1obj,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testcontainer," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainer1," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerallowlimmove," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerallowlimmove,cn=displayspecifiers," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerallowrename," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerallowrename1," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerdomain," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerdomain," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testcontaineridm," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontaineridm," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerwithoutsysflags," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerwithoutsysflags,cn=displayspecifiers," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testcontainerwithoutsysflags1," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testpassword,cn=password settings container,cn=system," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testsite,cn=sites," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=testtdiobject,ou=testou," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testuser1,cn=computers," + rootDomainNC);
                adLdapClient.DeleteObject("cn=testuser1,cn=users," + rootDomainNC, null);
                adLdapClient.DeleteObject("cn=testuser10,cn=users," + rootDomainNC, null);
                adLdapClient.DeleteTreeControl("cn=testuser2,cn=adminsdholder,cn=system," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testuser2,cn=nonexistingparent," + rootDomainNC);
                adLdapClient.DeleteObject("cn=testuser2,cn=users," + rootDomainNC, null);
                adLdapClient.DeleteTreeControl("cn=testuser4,cn=computers," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testuser5,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testuserobject,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=adts_user10,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6745,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6745,cn=usertreedel,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6746," + configurationNC);
                adLdapClient.DeleteTreeControl("cn=user6746,cn=nonexistingparent," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6746,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6746+cn=user67,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6750,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6751,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6752,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6753,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=user6754,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=userls,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=users1container," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=userscontainer," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=usertreedel,cn=users," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn>cont," + rootDomainNC);
                adLdapClient.DeleteTreeControl("dc=contoso," + rootDomainNC);
                adLdapClient.DeleteTreeControl("dc=testappnc," + rootDomainNC);
                adLdapClient.DeleteTreeControl($"ou={DomainAdministratorName},cn=users,{rootDomainNC}");
                adLdapClient.DeleteTreeControl("ou=testou," + rootDomainNC);
                adLdapClient.DeleteTreeControl("ou=testrootdn," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=account,cn=schema,cn=configuration,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=child$,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup1,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup2,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomaingroup3,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser1,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser2,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser3,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=crossdomainuser4,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testcontainer,cn=users,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("cn=testcontainer1,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl("dc=newappnc,dc=child," + rootDomainNC);
                adLdapClient.DeleteTreeControl(string.Format("cn={0},cn=computers,{1}", testComputer1Name, rootDomainNC));

                adLdapClient.Unbind();
                isConnected = false;
            }

            if (!string.IsNullOrEmpty(ADLDSPortNum))
            {
                if (!isConnected)
                {
                    SetConnectAndBind(ADImplementations.AD_LDS, PDCNetbiosName);
                }
                if (isConnected
                    && currentService.Equals(ADImplementations.AD_LDS))
                {
                    // The global variables need to be initialized by searching RootDSE: defaultNC, schemaNC, configurationNC, etc.
                    adLdapClient.DeleteTreeControl("cn=modifytestuser," + defaultNC);
                    adLdapClient.DeleteTreeControl("cn=modifytestuser1," + defaultNC);
                    adLdapClient.DeleteTreeControl("cn=spuser," + defaultNC);
                    adLdapClient.DeleteTreeControl("cn=spuser1," + schemaNC);
                    adLdapClient.DeleteTreeControl("cn=testgroup,cn=directoryupdates," + configurationNC);
                    adLdapClient.DeleteTreeControl("cn=testuser10," + defaultNC);
                    adLdapClient.DeleteTreeControl("cn=testuser6,cn=directoryupdates," + configurationNC);
                    adLdapClient.DeleteTreeControl("cn=testuser7,cn=directoryupdates," + configurationNC);
                    adLdapClient.DeleteTreeControl("cn=testuser8,cn=directoryupdates," + configurationNC);
                    adLdapClient.DeleteTreeControl("cn=testuser9," + defaultNC);

                    // AD LDS, set msDS-DefaultNamingContext attribute of DC's nTDSDSA object
                    string objectDN = Utilities.GetEntryDistinguishedName(currentWorkingDC.FQDN, ADLDSPortNum, configurationNC, "nTDSDSA", "*", testUserName, testUserPwd);
                    Utilities.SetAttributeForEntry(currentWorkingDC.FQDN, ADLDSPortNum, objectDN, "msDS-DefaultNamingContext", LDSApplicationNC, testUserName, testUserPwd);

                    adLdapClient.Unbind();
                    isConnected = false;
                }
            }
        }

        #endregion
    }
}
