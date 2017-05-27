// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
using Microsoft.Protocols.TestSuites.Frs2DataTypes;
using FRS2Model;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using Microsoft.Modeling;
using System.Security.AccessControl;
using System.Security.Principal;
using BKUPParser;
using FileStreamDataParser;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{
    public class Frs2TsException : Exception
    {

    }
    public enum SUTOSVersion
    {
        Invalid = 0,
        Win2008 = 1,
        Win2008R2 = 2,
        Win2012 = 3,
        Win2012R2 = 4,
        NonWindows = 99
    }

    public interface IConfigStoreInitializerAdapter : IAdapter
    {
        void UpdateConfigStore(ITestSite site);
    }

    /// <summary>
    /// this helper class is used when use MS-FR2_TestSuite.ptfconfig as source of ConfigStore. When this different data source, just use your owner initializer
    /// </summary>
    public class StandaloneTestSuiteConfigStoreInitializer : ManagedAdapterBase, IConfigStoreInitializerAdapter
    {
        public void UpdateConfigStore(ITestSite s)
        {
            string[] keys = s.Properties.AllKeys;
            List<string> unusedKeys = new List<string>();
            foreach (string ss in keys)
            {
                string refactorString = ss.Replace("-", "_");
                refactorString = refactorString[0].ToString().ToUpper() + refactorString.Substring(1);
                FieldInfo fi = typeof(ConfigStore).GetField(refactorString);
                if (fi != null)
                {
                    if (fi.FieldType == typeof(string))
                        fi.SetValue(null, s.Properties[ss]);
                    else if (fi.FieldType == typeof(bool))
                        fi.SetValue(null, bool.Parse(s.Properties[ss]));
                    else if (fi.FieldType == typeof(int))
                        fi.SetValue(null, int.Parse(s.Properties[ss]));
                    else if (fi.FieldType == typeof(Guid))
                        fi.SetValue(null, new Guid(s.Properties[ss]));
                }
                else if (ss.ToLower().StartsWith("isreqr") && ss.ToLower().EndsWith("implemented"))
                    ConfigStore.WillingToCheckReq.Add(int.Parse(ss.ToLower().Replace("isreqr", "").Replace("implemented", "")), bool.Parse(s.Properties[ss]));
                else if (ss.StartsWith("MAY-MS-FRS2_R"))
                    ConfigStore.WillingToCheckReq.Add(int.Parse(ss.Replace("MAY-MS-FRS2_R", "")), bool.Parse(s.Properties[ss]));
                else
                    unusedKeys.Add(ss);
            }
            IFRS2ServerControllerAdapter serverControllerAdapter = s.GetAdapter<IFRS2ServerControllerAdapter>();
            bool bResult = false;
            if (bool.TryParse(s.Properties["testSYSVOL"], out bResult))
                ConfigStore.IsTestSYSVOL = bResult;
            else
                s.Assert.Fail("Failed to read testSYSVOL bool type value");
            int iResult = -1;
            if (int.TryParse(s.Properties["OSVersion"], out iResult))
                ConfigStore.OSVer = (SUTOSVersion)iResult;
            else
                s.Assert.Fail("Failed to read OSVersion int type value");

            ConfigStore.DomainDnsName = s.Properties["DomainName"];
            ConfigStore.DomainNetbiosName = ConfigStore.DomainDnsName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0];
            // Check if SDC is set in ptfconfig
            if (string.IsNullOrWhiteSpace(s.Properties["SDCName"]))
            {
                s.Assume.Fail("The test requires a Secondary writable DC in the environment. Please set the corresponding field in PTF config. ");
            }
            ConfigStore.RepPartnerNetbiosName = s.Properties["SDCName"];
            ConfigStore.RepPartnerPassword = s.Properties["SDCPassword"];
            ConfigStore.SutIp = s.Properties["ServerComputerIp"];
            ConfigStore.SutLdapAddress = ConfigStore.SutIp + ":389";
            ConfigStore.SutDnsName = s.Properties["ServerName"];

            ConfigStore.AdminName = s.Properties["Username"];
            ConfigStore.AdminPassword = s.Properties["password"];
            string path = FRS2Helpers.ComputeNTDSConnectionPath(ConfigStore.DomainDnsName, ConfigStore.RepPartnerNetbiosName);
            ConfigStore.RepPartnerNTDSConnectionGUID = serverControllerAdapter.InitializeGuid(path, "(objectclass=ntdsconnection)");
            ConfigStore.TransportProtocol = s.Properties["Protocol"];
            if (ConfigStore.TransportProtocol == null || ConfigStore.TransportProtocol == "")
                s.Assert.Fail("Invalid transport protocol in configuration");
            ConfigStore.FRS2RpcUuid = s.Properties["ObjectUUID"];
            ConfigStore.SPNUuid = s.Properties["uuid"];
            ConfigStore.SutDN = s.Properties["ServerDistinguishedName"];
        }
    }

    /// <summary>
    /// test environment configuration data cache, should use this avoid repeat reading PTFConfig with hardcode strings
    /// </summary>
    public static class ConfigStore
    {
        public static Dictionary<int, bool> WillingToCheckReq = new Dictionary<int, bool>();

        public static Dictionary<int, bool> TestSuiteIssues = new Dictionary<int, bool>();

        public static string HashRequested;

        public static string InvalidGUID;

        public static string CreditsAvailable;

        public static string Reqnoblocked764;

        public static string Offset;

        public static string Length;

        public static string BufferSize;

        public static string SleepTime;

        public static string FileNameStreamValue;

        public static string AlternateBackupStreamCount;

        public static string FileData;

        public static string StaticPort;

        public static string AuthenticationSVC;

        public static string ReplicationDirectoryName;

        public static string FileName;

        public static string Reqnoblocked536;

        public static string DomainDnsName;

        public static string SutLdapAddress;

        public static string DomainNetbiosName;

        public static bool IsTestSYSVOL;

        public static Guid RepPartnerNTDSConnectionGUID;

        public static SUTOSVersion OSVer;

        public static string SutDnsName;

        public static string AdminName;

        public static string AdminPassword;

        public static string RepPartnerNetbiosName;

        public static string RepPartnerPassword;

        public static string TransportProtocol;

        public static string SutIp;

        public static string SPNUuid;

        public static string SutDN;

        public static string FRS2RpcUuid;

        public static string Ms_DFSRLocalSettings;

        public static string Ms_DFSRSubscriber;

        public static string Ms_DFSR_Connection;

        public static string Ms_DFSR_Subscription;

        public static string Ms_DFSR_Subscription1;

        public static string MsDFSR_Content;

        public static string Ms_DFSR_GlobalSettings;

        public static string Ms_DFSR_Content;

        public static string Ms_DFSR_Topology;

        public static string Ms_DFSR_Member;

        public static string ReplicaId1;

        public static string ReplicaId2;

        public static string ReplicaId3;

        public static string ReplicaId4;

        public static string ReplicaId5;

        public static string ContentId1;

        public static string ContentId2;

        public static string ContentId3;

        public static string ContentId4;

        public static string ContentId5;

        public static string ContentId6;

        public static string ConnectionId1;

        public static string ConnectionId2;

        public static string ConnectionId3;

        public static string ConnectionId4;

        public static string ConnectionId5;

        public static bool IsWindows;

        public static bool Should;

        public static bool Win2k8;

        public static bool Win2K8R2;

        public static bool MAY_MS_FRS2_R346;

        public static bool MAY_MS_FRS2_R13;

        public static bool MAY_MS_FRS2_R14;

        public static bool MAY_MS_FRS2_R32;

        public static bool MAY_MS_FRS2_R31;
    }

    public struct SEC_WINNT_AUTH_IDENTITY
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string User;

        public uint UserLength;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domain;


        public uint DomainLength;


        [MarshalAs(UnmanagedType.LPWStr)]
        public string StringPassword;


        public uint PasswordLength;

        public uint Flags;
    }
    /// <summary>
    /// FRS2ManagedAdapter implements ManagedAdapterBase, IFRS2ManagedAdapter and overrides all the methods of the 
    /// Interfaces
    /// </summary>
    public class FRS2ManagedAdapter : ManagedAdapterBase, IFRS2ManagedAdapter
    {
        #region variable declarations

        /// <summary>
        /// iFrs2Adapter is an instance of IFRS2ManagedAdapter.
        /// </summary>
        /// 
        public static IFRS2ManagedAdapter iFrs2Adapter = null;
        /// <summary>
        /// serverControllerAdapter is an instance of IFRS2ServerControllerAdapter.
        /// </summary>
        public static IFRS2ServerControllerAdapter serverControllerAdapter = null;

        /// <summary>
        /// This is the current file getting replicated.
        /// </summary>
        public string ReplicationFileName;

        /// <summary>
        /// The minimum size of the buffer passed to RdcGetFileData
        /// </summary>
        public const int XPRESS_RDC_MIN_GET_DATA_BUFFER_SIZE_WITH_FILE_HEADER = 9236;
        /// <summary>
        /// The maximum number of fragments in the list of fragments
        /// </summary>
        public const int XPRESS_RDC_MAX_NB_NEEDS_FOR_COMPRESSION = 128;
        /// <summary>
        /// The size of the compression block
        /// </summary>
        public const int X_CONFIG_XPRESS_BLOCK_SIZE = 8192;
        /// <summary>
        /// The GUID portion of the file's UID. Same as the database GUID of the replicated folder where this file originated.
        /// </summary>
        Guid uidDbGuid = new Guid();
        /// <summary>
        /// The VSN portion of the file's UID. This is assigned when the file is created.
        /// </summary>
        ulong uidVersion = 0;
        //AsyncPoll request flag
        public bool flagAsyncpollReq = false;
        /// <summary>
        /// The clientRdcDesired will be set to 1 or 0 based on client request for RDCDesired
        /// parameter in IntializeFIleTransferAsync and is used for validating the requirements.
        /// </summary>
        public static int clientRdcDesired = 0;
        /// <summary>
        /// boolean flag represents asyncpoll request status.
        /// </summary>
        public bool flagAsyncpollRequested = false;
        /// <summary>
        /// Version chain vector response payload envelope
        /// </summary>
        public static _FRS_ASYNC_RESPONSE_CONTEXT asyncResponse;
        /// <summary>
        /// Async Response Pointer
        /// </summary>
        public static _FRS_ASYNC_RESPONSE_CONTEXT_POINTER asyncResponsePtr;
        /// <summary>
        ///An entry of a version chain vector
        /// </summary>
        public static _FRS_VERSION_VECTOR[] versionVector;
        /// <summary>
        /// An array of structures that contains file metadata related to a particular
        /// file being processed by Distributed File System Replication (DFS-R)
        /// </summary>
        _FRS_UPDATE[] frsUpdate;
        /// <summary>
        /// Pointer to FrsUpdate
        /// </summary>
        IntPtr frsUpdatePtr;
        /// <summary>
        /// The number of bytes returned by the server.
        /// </summary>
        public static uint sizeRead;
        /// <summary>
        /// The RDC file info returned by the server in Initializefiletransferasync opnum.
        /// </summary>
        _FRS_RDC_FILEINFO RDCFileInfo;
        /// <summary>
        ///  Represents the Update count
        /// </summary>
        uint updateCount;
        /// <summary>
        ///Authenticatio Levels 
        /// </summary>
        uint authLevels;
        /// <summary>
        /// Specifies the AuthenticationService
        /// </summary>
        uint authService;
        /// <summary>
        /// The flagRdcSourceNeeds is used to check whether RDCPushSourceNeeds is 
        /// called before RDCGetfileData opnum.
        /// </summary>
        public static bool flagRdcSourceNeeds = false;

        ITestSite FRSSite;
        /// <summary>
        /// Context handle that represents the requested file replication operation.
        /// </summary>
        System.IntPtr servContext;
        /// <summary>
        /// RPC Binding Handle
        /// </summary>
        IntPtr bindingHandle;
        /// <summary>
        /// Interface UUID
        /// </summary>
        public string interfaceUUIDdrsuapi;
        /// <summary>
        /// Asynchronous Thread.
        /// </summary>
        Thread AsyncThread;
        /// <summary>
        /// A GUID represents the Replication Group Id.
        /// </summary>
        public static Guid replicationSetId = new Guid();

        string objectUUID;
        /// <summary>
        /// A GUID represents the ConnectionSetId.
        /// </summary>
        public static Guid ConnectionSetId = new Guid();
        /// <summary>
        /// A GUID represents the ContentSet Id.
        /// </summary>
        public static Guid contentsetId = new Guid();
        /// <summary>
        /// updateCancelflag is set when UpdateCancel is called.
        /// </summary>
        public bool updateCancelflag = false;
        /// <summary>
        /// updateCancelgvsnDatabaseId is represents the updateCancelgvsnDatBase Id.
        /// </summary>
        public static Guid updateCancelgvsnDatabaseId = new Guid();
        /// <summary>
        /// Update Cancel GVSN Version.
        /// </summary>
        public static ulong updateCancelgvsnVersion;

        public static ulong vvGenerationCheck = 0;
        /// <summary>
        /// Version Vector Generation
        /// </summary>
        public static ulong vvGenerat = 0;
        public static int ReqUpdatesFlag = 0;
        /// <summary>
        /// isEndofFile is set to 1 if the file to be transfered is ended. 
        /// </summary>
        public int isEndofFile;
        /// <summary>
        /// flagTradTestCaseCheck used to flip the test code from model generated test case to traditional test case.
        /// </summary>
        public static bool flagTradTestCaseCheck = false;
        /// <summary>
        /// flagTradTestCaseForPushSourceNeeds used to flip the test code from model generated test case to traditional test case.
        /// and is used for excuting the failure cases for RDCPushSourceNeeds
        /// </summary>
        public static bool flagTradTestCaseForPushSourceNeeds = false;
        /// <summary>
        /// flagTradTestCaseForRdcGetSig used to flip the test code from model generated test case to traditional test case.
        /// and is used for excuting the failure cases for RDCGetSignatures.
        /// </summar
        public static bool flagTradTestCaseForRdcGetSig = false;
        /// <summary>
        /// flagTradTestCaseForRdcClose used to flip the test code from model generated test case to traditional test case.
        /// and is used for excuting the failure cases for RDCClose.
        /// </summary>
        public static bool flagTradTestCaseForRdcClose = false;
        /// <summary>
        /// flagTradTestCaseForRdcGetSigfail used to flip the test code from model generated test case to traditional test case.
        /// and is used for excuting the failure case when RDCdesired=false is set for RDCGetsignature.
        /// </summary>
        public static bool flagTradTestCaseForRdcGetSigfail = false;
        /// <summary>
        /// flagTradTestCaseForRdcGetSigfailForLevel used to flip the test code from model generated test case to traditional test case.
        /// and is used for excuting the failure case when level set to out of range for RDCGetsignature.
        /// </summary>
        public static bool flagTradTestCaseForRdcGetSigfailForLevel = false;
        /// <summary>
        /// flagTradTestCaseForPushSourceNeedsForNeedCount used to flip the test code from model generated test case to traditional test case.
        /// and is used for excuting the test case with need count set to zero.
        /// </summary>
        public static bool flagTradTestCaseForPushSourceNeedsForNeedCount = false;

        public int countOfAsyncCall = 1;

        #endregion

        #region Event Declaration
        /// <summary>
        /// AsyncPollHandler event corresponds to AsyncPoll Response
        /// </summary>
        public event AsyncPollHandler AsyncPollResponseEvent;
        /// <summary>
        /// InitializeFileTransferhandler event corresponds to InitialiseFileTransfer Response.
        /// </summary>
        public event InitializeFileTransferAsyncHandler InitializeFileTransferAsyncEvent;
        /// <summary>
        /// RequestUpdatesHandler event corresponds to RequestUpdates Response
        /// </summary>
        public event RequestUpdatesHandler RequestUpdatesEvent;
        /// <summary>
        /// RawGetFileDataHandler event corresponds to RawGetFileData Response
        /// </summary>
        public event RawGetFileDataResponseHandler RawGetFileDataResponseEvent;
        /// <summary>
        /// RdcGetFileDataHandler event corresponds to RawGetFileData Response
        /// </summary>
        public event RdcGetFileDataHandler RdcGetFileDataEvent;
        /// <summary>
        /// RequestRecordsHandler event corresponds to RequestRecords Response
        /// </summary>
        public event RequestRecordsHandler RequestRecordsEvent;
        #endregion

        #region Initialize
        /// <summary>
        /// Overridden 'Initialize' member of ManagedAdapterBase
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            FRS2Helpers.Initialize(testSite);
            WindowsDFSRSControlAdapter.Site = testSite;
            bool adFlags = false;
            IConfigStoreInitializerAdapter initer = testSite.GetAdapter<IConfigStoreInitializerAdapter>();
            initer.UpdateConfigStore(testSite);
            FRSSite = TestClassBase.BaseTestSite;
            FRSSite.DefaultProtocolDocShortName = "MS-FRS2";
            iFrs2Adapter = FRSSite.GetAdapter<IFRS2ManagedAdapter>();
            serverControllerAdapter = FRSSite.GetAdapter<IFRS2ServerControllerAdapter>();
            interfaceUUIDdrsuapi = ConfigStore.SPNUuid;
            String replicationString = ConfigStore.ReplicaId1;
            if (!ConfigStore.IsTestSYSVOL)
                replicationSetId = serverControllerAdapter.InitializeGuid(replicationString,
                                                  "(objectClass=msDFSR-ReplicationGroup)");
            String Contentstring = ConfigStore.ContentId1;
            if (!ConfigStore.IsTestSYSVOL)
                contentsetId = serverControllerAdapter.InitializeGuid(Contentstring,
                                            "(objectClass=msDFSR-ContentSet)");

            //Server Name
            string serverName = ConfigStore.SutDnsName;
            string servicePrincipalName = null;
            string[] att = { "servicePrincipalName" };
            System.DirectoryServices.Protocols.SearchResponse respObj;
            serverControllerAdapter.RetrieveObjectwithattributes(
                                        ConfigStore.SutDN,
                                         ConfigStore.SutIp,
                                          "(objectClass=*)",
                                          att,
                                          System.DirectoryServices.Protocols.SearchScope.Base,
                                          out respObj);

            string[] servicePrincpalNames = new string[respObj.Entries[0].Attributes["servicePrincipalName"].Count];

            for (int count = 0; count < respObj.Entries[0].Attributes["servicePrincipalName"].Count; count++)
            {
                servicePrincpalNames[count] = (string)(respObj.Entries[0].Attributes["servicePrincipalName"][count]);
                if (servicePrincpalNames[count].ToLower().Contains(interfaceUUIDdrsuapi))
                {
                    servicePrincipalName = servicePrincpalNames[count];
                }
            }
            objectUUID = ConfigStore.FRS2RpcUuid;

            authLevels = (uint)RPC_C_AUTHN_LEVEL.RPC_C_AUTHN_LEVEL_PKT_PRIVACY;
            authService = Convert.ToUInt32(ConfigStore.AuthenticationSVC);
            bindingHandle = RPCBindingServer(ConfigStore.SutIp, servicePrincipalName, ConfigStore.TransportProtocol, objectUUID, authLevels, authService);


            if (!ConfigStore.IsTestSYSVOL)
            {
                #region Requirement Validation

                #region MS-FRS2_R254

                //Verifying In msDFSR-LocalSettings: Exactly one top-level DFS-R LocalSettings
                //object MUST exist for each computer that is configured for replication.
                //Verified by quering active directory DFSR_LocalSettings.
                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSRLocalSettings,
                                        "(objectClass=msDFSR-LocalSettings)",
                                        "objectGUID");

                FRSSite.CaptureRequirementIfIsTrue(
                                                                    adFlags,
                                                                     254,
                                                                    "In msDFSR-LocalSettings: Exactly one top-level" +
                                                                    "DFS-R LocalSettings object MUST exist for each " +
                                                                    "computer that is configured for replication.");

                #endregion

                #region MS-FRS2_R259

                bool flag259 = serverControllerAdapter.SetACLs(ConfigStore.Ms_DFSRLocalSettings,
                                         ConfigStore.AdminName,
                                         ConfigStore.DomainNetbiosName,
                                         ActiveDirectoryRights.Delete,
                                         AccessControlType.Deny);


                if (ConfigStore.WillingToCheckReq[256])
                {   //In msDFSR-LocalSettings, ACLs may be set on this msDFSR-LocalSettings and inherited to children
                    FRSSite.CaptureRequirementIfIsTrue(flag259, 256, "In msDFSR-LocalSettings, ACLs may be set on this msDFSR-LocalSettings and inherited to children");
                }


                if (ConfigStore.IsWindows == true)
                {
                    //In the Windows implementation, ACLs are set on msDFSR-LocalSettings to protect changing or disclosing configuration information.<7>

                    FRSSite.CaptureRequirementIfIsTrue(flag259,
                                                    259,
                                                    @"In the Windows implementation, ACLs are set on msDFSR-LocalSettings 
                                                          to protect changing or disclosing configuration information.<7>");
                }
                #endregion

                bool flag317 = serverControllerAdapter.SetACLs(ConfigStore.Ms_DFSR_Content,
                                    ConfigStore.AdminName,
                                    ConfigStore.DomainNetbiosName,
                                    ActiveDirectoryRights.Delete,
                                    AccessControlType.Deny);

                if (ConfigStore.WillingToCheckReq[317])
                {
                    //Verifying whether ACLs are set on msDFSR-Content and inherited on child objects.
                    FRSSite.CaptureRequirementIfIsTrue(flag317,
                                                     317,
                                                    "ACLs MAY be set on msDFSR-Content and inherited on child objects.");
                }

                if (ConfigStore.IsWindows == true)
                {
                    //Verifying Windows doesnot set ACLs on msDFSR-Content and inherited on child Objects.
                    FRSSite.CaptureRequirementIfIsTrue(flag317, 318, "Windows doesnot set ACLs on msDFSR-Content and inherited on child Objects.");


                }


                #region MS-FRS2_R262
                //Verifying In msDFSR-Subscriber, the  msDFSR-MemberReference MUST exist.
                //Verified by quering ActiveDirectory for msDFSR-MemberReference in msDFSR-subscriber
                adFlags = serverControllerAdapter.AdValidation(
                                      ConfigStore.Ms_DFSRSubscriber,
                                       "(objectClass=msDFSR-Subscriber)",
                                        "msDFSR-MemberReference");
                FRSSite.CaptureRequirementIfIsTrue(
                                               adFlags,
                                               262,
                                               "In msDFSR-Subscriber, the  msDFSR-MemberReference" +
                                               "MUST exist.");

                #endregion

                #region MS-FRS2_R264
                //Verifying In msDFSR-Subscriber: At most, one msDFSR-Member object MUST be referenced 
                //from an msDFSR-Subscriber object.
                //Verifeid by quering for msDFSR-Member in msDFSR-subscriber object.
                adFlags = serverControllerAdapter.AdValidation(
                                    ConfigStore.Ms_DFSRSubscriber,
                                         "(objectClass=msDFSR-Subscriber)",
                                         "msDFSR-MemberReference");
                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                               264,
                                               "In msDFSR-Subscriber: At most, one msDFSR-Member object MUST" +
                                               "be referenced from an msDFSR-Subscriber object.");

                #endregion

                #region MS-FRS2_R268
                //Verifying In msDFSR-Subscription, the msDFSR-ContentSetGUID MUST exist.
                //Verfied by quering ActiveDirectory for msDFSR_ContentSet Guid in msDFSR-Subscription.
                adFlags = serverControllerAdapter.AdValidation(
                                      ConfigStore.Ms_DFSR_Subscription,
                                        "(objectClass=msDFSR-Subscription)",
                                        "msDFSR-ContentSetGUID");
                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                               268,
                                               "In msDFSR-Subscription, the " +
                                               "msDFSR-ContentSetGUID MUST exist");

                #endregion

                #region MS-FRS2_R270
                //Verifying In msDFSR-Subscription, the msDFSR-RootPath MUST exist.
                //Verified by quering ActiveDirecotry for msDFSR-RootPath in msDFSR-Subscription object.
                adFlags = serverControllerAdapter.AdValidation(
                                        ConfigStore.Ms_DFSR_Subscription,
                                         "(objectClass=msDFSR-Subscription)",
                                          "msDFSR-RootPath");
                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                               270,
                                               "In msDFSR-Subscription, the msDFSR-RootPath MUST exist.");

                #endregion

                adFlags = serverControllerAdapter.AdValidation(
                                        ConfigStore.Ms_DFSR_Subscription,
                                        "(objectClass=msDFSR-Subscription)",
                                        "msDFSR-Options",
                                        0);

                if (ConfigStore.Should)
                {
                    #region MS-FRS2_R280
                    //Verifying In msDFSR-Subscription for msDFSR-Options all other
                    //bits except,0X1 are ignored and SHOULD be set to 0.

                    FRSSite.CaptureRequirementIfIsTrue(
                                                       adFlags,
                                                       280,
                                                       "In msDFSR-Subscription for msDFSR-Options all " +
                                                       "other bits except,0X1 are ignored" +
                                                       "and SHOULD be set to 0.");

                    #endregion
                }

                #region MS-FRS2_R281
                if (ConfigStore.IsWindows == true)
                {
                    //Verifying In msDFSR-Options, Windows does ignore  all other bits except, 0X1 and set them to 0.
                    FRSSite.CaptureRequirementIfIsTrue(
                                                    adFlags,
                                                    281,
                                                    @"In msDFSR-Options, Windows does ignore  all other 
                                                           bits except, 0X1 and set them to 0.");
                }
                #endregion

                #region MS-FRS2_R289
                //Verifying  At most, one msDFSR-Content object MUST be referenced from an
                //msDFSR-Subscription object.
                //Verified by quering ActiveDirectory for msDFSR-content object
                //in msDFSR-Subscripton Object.
                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_Subscription,
                                       "(objectClass=msDFSR-Subscription)",
                                       "msDFSR-ContentSetGuid");

                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                               289,
                                               " At most, one msDFSR-Content object MUST be " +
                                               "referenced from an msDFSR-Subscription object.");

                #endregion

                #region MS-FRS2_R288
                //Verifying Each msDFSR-Subscription object MUST contain a reference to one msDFSR-Content object.
                //Verifyied by quering the AD for msDFSR-Content object in msDFSR-Subscription.
                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                                288,
                                                @"Each msDFSR-Subscription object MUST contain a reference to 
                                                         one msDFSR-Content object.");
                #endregion

                #region MS-FRS2_R292
                //Verifying ACLs MAY be set on msDFSR-GlobalSettings and inherited on child objects.
                //Verified by setting Delete rights to ms-DFSR-GlobalSettings object, if it succeeds hence the requirement is verified.

                bool flag292 = serverControllerAdapter.SetACLs(ConfigStore.Ms_DFSR_GlobalSettings,
                                     ConfigStore.AdminName,
                                     ConfigStore.DomainNetbiosName,
                                     ActiveDirectoryRights.Delete,
                                     AccessControlType.Deny);

                if (ConfigStore.WillingToCheckReq[292])
                {
                    FRSSite.CaptureRequirementIfIsTrue(flag292,
                                                     292,
                                                     " ACLs MAY be set on msDFSR-GlobalSettings and inherited on child objects.");
                }

                if (ConfigStore.IsWindows == true)
                {
                    //Verifying Windows does not set ACLs on msDFSR-GlobalSettings and inherited on child objects.
                    FRSSite.CaptureRequirementIfIsTrue(flag292,
                                                     293,
                                                     "Windows does not set ACLs on msDFSR-GlobalSettings and inherited on child objects");

                }
                #endregion

                #region MS-FRS2_R294
                //Verifying There MUST be exactly one msDFSR-GlobalSettings object for every domain where
                // DFS-R is configured for replication.
                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_GlobalSettings,
                                       "(objectClass=msDFSR-GlobalSettings)",
                                       "objectGUID");

                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                               294,
                                               "There MUST be exactly one msDFSR-GlobalSettings object" +
                                               "for every domain where DFS-R is configured for replication.");
                #endregion

                #region MS-FRS2_R298
                //Verifying The msDFSR-ReplicationGroupType field in
                //msDFSR-ReplicationGroup MUST exist.
                //Verified by Quering msDFSR-ReplicationGrouptype field in 
                //msDFSR-ReplicationGroup.
                adFlags = serverControllerAdapter.AdValidation(
                                     ConfigStore.ReplicaId1,
                                     "(objectClass=msDFSR-ReplicationGroup)",
                                     "msDFSR-ReplicationGroupType");

                FRSSite.CaptureRequirementIfIsTrue(
                                               adFlags,
                                               298,
                                               "The msDFSR-ReplicationGroupType field in " +
                                               "msDFSR-ReplicationGroup MUST exist.");

                #endregion

                #region MS-FRS2_R303
                //Verifying The objectGUID field in msDFSR-ReplicationGroup MUST exist.
                //Verified by Quering objectGUID  field in msDFSR-ReplicationGroup.
                adFlags = serverControllerAdapter.AdValidation(
                                           ConfigStore.ReplicaId1,
                                            "(objectClass=msDFSR-ReplicationGroup)",
                                            "objectGUID");

                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                               303,
                                               "The objectGUID field in msDFSR-ReplicationGroup MUST" +
                                               "exist.");

                #endregion

                adFlags = serverControllerAdapter.AdValidation(
                                         ConfigStore.ReplicaId1,
                                          "(objectClass=msDFSR-ReplicationGroup)",
                                          "msDFSR-Options",
                                           1);


                if (ConfigStore.Should)
                {
                    #region MS-FRS2_R307
                    //Verifying In msDFSR-Options field all other bits except 0X1,
                    //are ignored and SHOULD be set to 0.
                    //Verified by quering msDFSR-Options field in msDFSR-ReplicationGroup object.


                    FRSSite.CaptureRequirementIfIsTrue(
                                                   adFlags,
                                                   307,
                                                   "In msDFSR-Options field all other bits except 0X1," +
                                                   "are ignored and SHOULD be set to 0.");

                    #endregion
                }

                #region MS-FRS2_R308
                //Verifying In msDFSR-Options field, Windows ignore all other bits except 0X1,
                //and does set to them to 0.
                //Verified by Quering the AD for msDFSR-Options in msDFSR-Replication.
                if (ConfigStore.IsWindows)
                {
                    if (ConfigStore.IsTestSYSVOL)
                    {
                        FRSSite.CaptureRequirementIfIsFalse(
                                                        adFlags,
                                                        308,
                                                        @"In msDFSR-Options field, Windows ignore all other bits 
                                                            except 0X1, and does set to them to 0.");
                    }
                    else
                    {
                        FRSSite.CaptureRequirementIfIsTrue(
                                                                adFlags,
                                                                308,
                                                                @"In msDFSR-Options field, Windows ignore all other bits 
                                                                        except 0X1, and does set to them to 0.");
                    }
                }
                #endregion

                #region MS-FRS2_R311
                //Verifying Each msDFSR-ReplicationGroup MUST appear as a 
                //reference under msDFSR-GlobalSettings. 
                //Verfied by quering for objectGuid of an msDFSR_ReplicationGroup which is under msDFSR-GlobalSettings.
                adFlags = serverControllerAdapter.AdValidation(
                                         ConfigStore.ReplicaId1,
                                          "(objectClass=msDFSR-ReplicationGroup)",
                                           "ObjectGUID");
                FRSSite.CaptureRequirementIfIsTrue(
                                               adFlags,
                                               311,
                                               "Each msDFSR-ReplicationGroup MUST appear as a reference " +
                                               "under msDFSR-GlobalSettings. ");

                #endregion

                #region MS-FRS2_R312
                //Verifiyng The msDFSR-ReplicationGroup MUST contain an msDFSR-Content and an msDFSR-Topology reference. 
                //Verified by quering GUIDs of  Content and Topolgy  objects of the ReplicationGroup.
                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_Content,
                                       "(objectClass=msDFSR-Content)",
                                       "ObjectGUID");
                if (adFlags)
                {
                    adFlags = serverControllerAdapter.AdValidation(
                                            ConfigStore.Ms_DFSR_Topology,
                                            "(objectClass=msDFSR-Topology)",
                                            "ObjectGUID");

                    FRSSite.CaptureRequirementIfIsTrue(
                                                       adFlags,
                                                       312,
                                                       "The msDFSR-ReplicationGroup MUST contain an msDFSR-Content" +
                                                       "and an msDFSR-Topology reference. ");

                }
                #endregion

                #region MS-FRS2_R313

                //Verifiying The msDFSR-ReplicationGroup MUST be at most one 
                //msDFSR-Content and at most one msDFSR-Topology child object 
                //under each msDFSR-ReplicationGroup object.
                //Verified by quering Content and Topology obects GUIDs of the
                //ReplicationGroup.

                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_Content,
                                        "(objectClass=msDFSR-Content)",
                                        "ObjectGUID");
                if (adFlags)
                {
                    adFlags = serverControllerAdapter.AdValidation(
                                           ConfigStore.Ms_DFSR_Topology,
                                           "(objectClass=msDFSR-Topology)",
                                           "ObjectGUID");

                    FRSSite.CaptureRequirementIfIsTrue(
                                                        adFlags,
                                                        313,
                                                        "The msDFSR-ReplicationGroup MUST be at most one msDFSR-Content" +
                                                        "and at most one msDFSR-Topology child object under each" +
                                                        "msDFSR-ReplicationGroup object.");

                }
                #endregion

                adFlags = serverControllerAdapter.AdValidation(
                                   ConfigStore.Ms_DFSR_Content,
                                   "(objectClass=msDFSR-Content)",
                                   "ObjectGUID");
                if (ConfigStore.WillingToCheckReq[315])
                {
                    #region MS-FRS2_R315
                    //Verifiying Each msDFSR-Content MAY contain references to one or more msDFSR-ContentSet objects. 
                    //Verified by quering ActiveDirectory for ObjectGuid of ms-DFSR-content.

                    FRSSite.CaptureRequirementIfIsTrue(
                                                       adFlags,
                                                       315,
                                                       "Each msDFSR-Content MAY contain references " +
                                                       "to one or more msDFSR-ContentSet objects. ");

                    #endregion

                }

                #region MS-FRS2_R316
                //Verifying In the Windows implementation, each msDFSR-Content does not contain references
                //to one or more msDFSR-ContentSet objects. 
                if (ConfigStore.IsWindows)
                {
                    FRSSite.CaptureRequirementIfIsTrue(
                                                     adFlags,
                                                     316,
                                                     "Verifying In the Windows implementation, each msDFSR-Content" +
                                                     "does not contain references to one or more msDFSR-ContentSet objects. ");
                }
                #endregion
                #region MS-FRS2_R322
                //Verifying The objectGUID attribute of the msDFSR-ContentSet object MUST exist.
                //Verified by quering objectGuid of the msDFSR-contentSet object.
                adFlags = serverControllerAdapter.AdValidation(
                                         ConfigStore.Ms_DFSR_Content,
                                         "(objectClass=msDFSR-Content)",
                                         "objectGuid");

                FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                                322,
                                                "The objectGUID attribute of the " +
                                               "msDFSR-ContentSet object MUST exist.");

                #endregion

                adFlags = serverControllerAdapter.AdValidation(
                                      ConfigStore.Ms_DFSR_Member,
                                      "(objectClass=msDFSR-Member)",
                                      "ObjectGUID");

                if (ConfigStore.WillingToCheckReq[335])
                {
                    #region MS-FRS2_R335
                    //Verifying Each msDFSR-Topology MAY contain references to one or more msDFSR-Member objects.


                    FRSSite.CaptureRequirementIfIsTrue(
                                                      adFlags,
                                                      335,
                                                      "Each msDFSR-Topology MAY contain references" +
                                                      "to one or more msDFSR-Member objects.");

                    #endregion
                }
                //Verifying In Windows implementation each msDFSR-Topology does not contain references to one or more msDFSR-Member objects.
                if (ConfigStore.IsWindows)
                {
                    FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                                     336,
                                                     "In Windows implementation each msDFSR-Topology" +
                                                     "does not contain references to one or more msDFSR-Member objects.");
                }


                #region MS-FRS2_R338
                //Verifying Each computer that participates
                //in a replication group MUST have one corresponding member object.
                //Verified by Quering ObjectGuid of the msDFSR-Memeber 

                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_Member,
                                        "(objectClass=msDFSR-Member)",
                                        "ObjectGUID");

                FRSSite.CaptureRequirementIfIsTrue(
                                               adFlags,
                                               338,
                                               "Each computer that participates in a replication group" +
                                               "MUST have one corresponding member object.");

                #endregion

                #region MS-FRS2_R340
                //Verifying The objectGUID attribute of the msDFSR-Member MUST exist.
                //Verified by Quering objectGUID attribute of the msDFSR-Member object.
                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_Member,
                                       "(objectClass=msDFSR-Member)",
                                       "ObjectGUID");
                FRSSite.CaptureRequirementIfIsTrue(
                                                adFlags,
                                                340,
                                               "The objectGUID attribute of the msDFSR-Member MUST exist.");

                #endregion

                #region MS-FRS2_R342
                //Verifying The msDFSR-ComputerReference attribute of the msDFSR-Member MUST exist.
                //Verified by Quering ActiveDirectory  msDFSR-computerReference of the msDFSR-Member object.
                adFlags = serverControllerAdapter.AdValidation(
                                        ConfigStore.Ms_DFSR_Member,
                                        "(objectClass=msDFSR-Member)",
                                        "msDFSR-ComputerReference");

                FRSSite.CaptureRequirementIfIsTrue(
                                               adFlags,
                                               342,
                                               "The msDFSR-ComputerReference attribute of the msDFSR-Member MUST exist.");

                #endregion

                adFlags = serverControllerAdapter.AdValidation(
                                           ConfigStore.Ms_DFSR_Connection,
                                           "(objectClass=msDFSR-Connection)",
                                           "objectGuid");


                if (Convert.ToBoolean(ConfigStore.MAY_MS_FRS2_R346) == true)
                {
                    #region MS-FRS2_R346
                    //Verifying An msDFSR-Member object MAY contain more than one msDFSR-Connection object with the same partner.


                    FRSSite.CaptureRequirementIfIsTrue(
                                                        adFlags,
                                                        346,
                                                        "An msDFSR-Member object MAY contain more than one " +
                                                        "msDFSR-Connection object" +
                                                        "with the same partner.");


                    #endregion
                }
                #region MS-FRS2_R352
                //Verifying The objectGUID attribute of the msDFSR-Connection object MUST exist.
                //Verified by quering AD for ObjectGUID of the msDFSR-connection Object.
                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_Connection,
                                       "(objectClass=msDFSR-Connection)",
                                       "ObjectGUID");
                if (!ConfigStore.IsTestSYSVOL)
                {
                    FRSSite.CaptureRequirementIfIsTrue(
                                                   adFlags,
                                                   352,
                                                   "The objectGUID attribute of the msDFSR-Connection object MUST exist.");

                }
                #endregion

                #region MS-FRS2_R354

                //Verifying the FromServer attribute of the msDFSR-Connection MUST exist.
                //Verified by Quering AD for the FromServer attribute of the msDFSR_Connection object.
                adFlags = serverControllerAdapter.AdValidation(
                                       ConfigStore.Ms_DFSR_Connection,
                                        "(objectClass=msDFSR-Connection)",
                                         "FromServer");

                if (!ConfigStore.IsTestSYSVOL)
                {
                    FRSSite.CaptureRequirementIfIsTrue(
                                                   adFlags,
                                                   354,
                                                   "The FromServer attribute of the msDFSR-Connection MUST exist.");
                }

                #endregion

                if (Convert.ToBoolean(ConfigStore.Should))
                {
                    #region MS-FRS2_R364
                    //Verifying the msDFSR-Options attribute of the msDFSR-Connection, bits[except 0x1]  SHOULD be set to 0.
                    //Verified by AD for msDFSR-option attribute of msDFSR-Connection object. if options  field is one
                    //hence the requirement is verified.
                    adFlags = serverControllerAdapter.AdValidation(
                                           ConfigStore.Ms_DFSR_Connection,
                                           "(objectClass=msDFSR-Connection)",
                                           "FromServer",
                                           1);

                    if (!ConfigStore.IsTestSYSVOL)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(
                                                           adFlags,
                                                           364,
                                                           "The msDFSR-Options attribute of the msDFSR-Connection, bits[except 0x1]  SHOULD be set to 0.");
                    }
                    #endregion
                }
                #region MS-FRS2_R365
                //Verifying For the attribute  msDFSR-Options of the msDFSR-Connection,
                //Windows does ignore all other bits[except 0x1].
                if (ConfigStore.IsWindows)
                {
                    if (!ConfigStore.IsTestSYSVOL)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(
                                                       adFlags,
                                                       365,
                                                       @"For the attribute  msDFSR-Options of the msDFSR-Connection, 
                                                                     Windows does ignore all other bits[except 0x1].");
                    }
                }



                #endregion

                #region MS-FRS2_R1268
                //Verifying msDFSR-ReadOnly flag must be set to 1 if the replicated folder is configured read-only .
                //Verfied by Quering the ActiveDirectory for msDFSR-Readonly object in msDFSR-Subscription.
                if (ConfigStore.Win2k8)
                {
                    adFlags = serverControllerAdapter.AdValidation(
                                     ConfigStore.Ms_DFSR_Subscription,
                                      "(objectClass=msDFSR-Subscription)",
                                      "msDFSR-ReadOnly",
                    true);
                }
                else
                {

                    adFlags = serverControllerAdapter.AdValidation(
                                            ConfigStore.Ms_DFSR_Subscription,
                                            "(objectClass=msDFSR-Subscription)",
                                            "msDFSR-ReadOnly",
                                          (ConfigStore.IsTestSYSVOL ? false : true));               
                }
                FRSSite.CaptureRequirementIfIsTrue(
                                                adFlags,
                                                1268,
                                                @"msDFSR-ReadOnly flag must be set to 1 if the replicated folder is configured read-only .");
                #endregion

                #region MS-FRS2_R1269
                //Verifying msDFSR-ReadOnly flag must be set to 0 for regular replicated folders. 
                adFlags = serverControllerAdapter.AdValidation(
                                        ConfigStore.Ms_DFSR_Subscription1,
                                        "(objectClass=msDFSR-Subscription)",
                                        "msDFSR-ReadOnly", false);

                FRSSite.CaptureRequirementIfIsTrue(
                                            adFlags,
                                            1269,
                                            @"msDFSR-ReadOnly flag must be set to 0 for regular replicated folders. ");
                #endregion

                #region MS-FRS2_R1272
                //Verifying whether The msDFSR-ReadOnly flag is optional and if not set, DFSR MUST default to treating the replicated folder as a read-write replicated folder. 
                FRSSite.CaptureRequirementIfIsTrue(
                                           adFlags,
                                           1272,
                                           @"The msDFSR-ReadOnly flag is optional and if not set, DFSR MUST default to treating the replicated folder as a read-write replicated folder. ");
                #endregion

                #region MS-FRS2_R1273
                if (ConfigStore.IsWindows)
                {
                    adFlags = serverControllerAdapter.AdValidation(
                                        ConfigStore.Ms_DFSR_Subscription,
                                        "(objectClass=msDFSR-Subscription)",
                                        "msDFSR-ReadOnly", (ConfigStore.IsTestSYSVOL ? false : true));               

                    //Verifying The msDFSR-ReadOnly flag is set to 1 on windows Server 2008 read
                    //only domain controllers which are using DFSR for SYSVOL replication <9> 
                    if (ConfigStore.Win2k8)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                                            1273,
                                                            "The msDFSR-ReadOnly flag is set to 1 on windows Server 2008 read only domain controllers which are using DFSR for SYSVOL replication <9>");
                    }
                #endregion

                    if (ConfigStore.Win2K8R2)
                    {
                        //Verifying On Windows server 2008R2, msDFSR-ReadOnly flag is set to 1 to configure a replicated folder as readOnly<9> 
                        FRSSite.CaptureRequirementIfIsTrue(adFlags,
                                                          1274,
                                                          "On Windows server 2008R2, msDFSR-ReadOnly flag is set to 1 to configure a replicated folder as readOnly<9> ");

                    }
                }

                #endregion
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initialization is the pre-requiFRSSite for all the scenarios. In order to invoke a protocol call, Initialization must
        /// be called. During the course of Test Suite execution, Initialization must be called only once. Also for this Test Suite,
        /// the accepting state condition enforced is Initialization must be called.
        ///
        /// In order to execute the TS, some configurations/test-objects should be present on the server. Details documented in 
        /// Test Deployement Environment Guide.
        ///
        /// For the model, an equivalent metadata representation is needed. Initialization populates the Maps, which hold the metadata
        /// that are needed for behavior analysis, test case generation and requirements capturing.
        ///
        ///
        /// Initialization populates the following maps.
        /// (1).
        /// Map<int, connectionProperty> serverConn --> Some connections should be established in the server. Stores the connectionIDs 
        ///                                             and whether the corresponding connection is enabled or disabled. 
        ///                                             Key --> connectionID: Value --> connectionProperty.
        ///                                             
        /// (2).
        /// Map<int, FromServer> fromServConn --> Stores the connectionIDs and status of FromServer (TD Section 2.3.11 DN of the  
        ///                                       inbound partner(other msDFSR-Member object). MUST exist)
        ///                                       Key --> connectionID: Value --> status of FromServer
        /// (3).
        /// Map<string,int> replGrpConn --> Some replication groups must be created on the server with respect to the connections.
        ///                                 Key --> Replication Group name (used as string identifiers in Model, adpater maps actual 
        ///                                 replication group name from ptfconfig): Value --> connectionID
        /// (4).
        /// Map<string,ReplicationGroupTypes> replGrpType --> Stores Replication group type property, whether sysvol or protection for the
        ///                                                   replication groups.
        ///													  Key --> Replication Group Name: Value --> Replication group Type (whether
        ///                                                                                             sysvol or protection)
        /// (5).
        /// Map<int, string> folderReplGrp --> Folders need to exist on the server. The folders belong to the replication groups.
        ///                                    Key --> FolderID (same as contentSetID.(used as int identifiers in Model, adpater maps 
        ///                                    abstracted FolderID values to actual FolderIDs/Folder Names from ptfconfig)
        /// (6).
        /// Map<int, accessLevels> folderAccLevel --> The access levels of folders need to be stored.
        ///                                           Key --> FolderID: Value --> Access Level of the folder
        ///
        /// (7).
        /// Map<int, connectionProperty> folderDFRS --> Maintains the ms-DFSR-Enabled property whether enabled or disabled for the
        ///                                             replicated folder
        ///                                             Key --> FolderID: Value --> ms-DFSR connection property enabled or disabled
        ///
        /// EstablishConnection/EstablishSession/AsyncPoll --> In order to reduce test-cases, we are using only valid values for these
        ///                                                    methods. Scenario 7 checks for all possible combinations for these
        ///                                                    methods. Scheme is enforced in order to cut down on redundant test-cases,
        ///                                                    which will be the same for scenario 7 and for other scenarios if not 
        ///                                                    constrained
        /// In model connectionIDs/folderIDs/replication groups and other properties and conectionProperty are used as abstract 
        /// identifiers Since the signature of methods are the same in both Model and Adapter, the test case details are shared 
        /// by both. The adapter maps the actual (real-time test values) depending upon the abstract identifier value that has come
        /// from the cord/test-case.
        /// </summary>
        /// <param name="serverConn">Stores the connectionIDs 
        /// and whether the corresponding connection is enabled or disabled. 
        /// Key --> connectionID: Value --> connectionProperty</param>
        /// <param name="fromServConn">Stores the connectionIDs and status of FromServer (TD Section 2.3.11 DN of the  
        /// inbound partner(other msDFSR-Member object). MUST exist)
        /// Key --> connectionID: Value --> status of FromServer</param>
        /// <param name="replGrpConn">Some replication groups must be created on the server with respect to the connections.
        /// Key --> Replication Group name (used as string identifiers in Model, adpater maps actual 
        /// replication group name from ptfconfig): Value --> connectionID</param>
        /// <param name="replGrpType">Stores Replication group type property, whether sysvol or protection for the
        /// replication groups. Key --> Replication Group Name: Value --> Replication group Type (whether
        /// sysvol or protection)</param>
        /// <param name="folderReplGrp">Folders need to exist on the server. The folders belong to the replication groups.
        /// Key --> FolderID (same as contentSetID.(used as int identifiers in Model, adpater maps 
        /// abstracted FolderID values to actual FolderIDs/Folder Names from ptfconfig)</param>
        /// <param name="folderAccLevel">The access levels of folders need to be stored.
        /// Key --> FolderID: Value --> Access Level of the folder</param>
        /// <param name="folderDFRS">Maintains the ms-DFSR-Enabled property whether enabled or disabled for the
        /// replicated folder. Key --> FolderID: Value --> ms-DFSR connection property enabled or disabled</param>
        /// <returns>error_status_t</returns>
        public error_status_t Initialization(
                                            OSVersion osVersion,
                                            Map<int, connectionProperty> serverConn,
                                            Map<int, FromServer> fromServConn,
                                            Map<string, int> replGrpConn,
                                            Map<string, ReplicationGroupTypes> replGrpType,
                                            Map<int, string> folderReplGrp,
                                            Map<int, accessLevels> folderAccLevel,
                                            Map<int, connectionProperty> folderDFRS)
        {
            GeneralInitialize();
            return error_status_t.ERROR_SUCCESS;
        }
        #endregion

        /// <summary>
        /// Check if SDC is set up successfully after FRS2 adapter is initialized
        /// </summary>
        public static void PreCheck()
        {
            if (string.IsNullOrWhiteSpace(ConfigStore.RepPartnerNetbiosName))
            {
                TestClassBase.BaseTestSite.Assume.Fail("The test requires a Secondary writable DC in the environment. Please set the corresponding field in PTF config.");
            }
            if (ConfigStore.RepPartnerNTDSConnectionGUID.Equals(Guid.Empty))
            {
                TestClassBase.BaseTestSite.Assume.Fail("Failed to connect to: {0}.", ConfigStore.RepPartnerNetbiosName);
            }
        }

        public void GeneralInitialize()
        {
            //reset any possible cross test case state variables
            updateCancelflag = false;
            m_inShutdown = false;
            if (AsyncThread != null)
            {
                try
                {
                    AsyncThread.Abort();
                }
                catch
                {
                }
            }
            WindowsDFSRSControlAdapter.StopDFSRS(ConfigStore.RepPartnerNetbiosName);
        }

        protected override void Dispose(bool disposing)
        {
            if (AsyncThread != null)
            {
                try
                {
                    AsyncThread.Abort();
                }
                catch
                {
                }
            }
            base.Dispose(disposing);
        }


        #region RpcBinding

        /// <summary>
        /// RPCBindingServer is used to bind to the Server
        /// </summary>
        /// <param name="serverName">Server computer name.</param>
        /// <param name="serverPrincipalName">Server Prinicipal name</param>
        /// <param name="transportProtocol">Transport Protocol</param>
        /// <param name="objectUUID">Object UUID</param>
        /// <param name="authnLevel">Authentication Levels</param>
        /// <param name="authnSvc">Authentication Service</param>
        /// <returns></returns>
        public static IntPtr RPCBindingServer(string serverName,
                                              string serverPrincipalName,
                                              string transportProtocol,
                                              string objectUUID,
                                              UInt32 authnLevel,
                                              UInt32 authnSvc)
        {
            IntPtr stringBindingPtr = IntPtr.Zero;
            string bindingString = string.Empty;

            uint status = RPCBinding.RpcStringBindingCompose(objectUUID,
                                                           transportProtocol,
                                                           serverName,
                                                           null,
                                                           null,
                                                           out bindingString);

            // if the RpcStringBindingCompose is succesfull
            if (0 == status)
            {
                // Rpc String binding compose
                status = RPCBinding.RpcBindingFromStringBinding(bindingString, out stringBindingPtr);
            }
            // if rpc binding from String binding is successful
            if (0 == status)
            {
                UInt32 authzlevel = 0;

                SEC_WINNT_AUTH_IDENTITY secAuthIdentity = new SEC_WINNT_AUTH_IDENTITY();
                // when test SYSVOL, need auth as SDC
                if (ConfigStore.IsTestSYSVOL)
                {
                    secAuthIdentity.Domain = ConfigStore.DomainDnsName;
                    secAuthIdentity.User = ConfigStore.RepPartnerNetbiosName + "$";
                    secAuthIdentity.StringPassword = ConfigStore.RepPartnerPassword;
                    secAuthIdentity.DomainLength = (uint)secAuthIdentity.Domain.Length;
                    secAuthIdentity.UserLength = (uint)secAuthIdentity.User.Length;
                    secAuthIdentity.PasswordLength = (uint)secAuthIdentity.StringPassword.Length;
                    secAuthIdentity.Flags = 2;
                }
                status = RPCBinding.RpcBindingSetAuthInfo(stringBindingPtr, serverPrincipalName, authnLevel, authnSvc, ref secAuthIdentity, authzlevel);
            }
            return stringBindingPtr;
        }

        #endregion

        Guid getConnectionSetId(int connectionId)
        {
            if (!ConfigStore.IsTestSYSVOL)
                return new Guid(typeof(ConfigStore).GetField("ConnectionId" + connectionId.ToString()).GetValue(null).ToString());
            else
            {
                // for connection 1, assume use second DC to connect PDC
                Guid ret = Guid.Empty;
                try
                {
                    if (connectionId == 1)
                    {
                        ret = ConfigStore.RepPartnerNTDSConnectionGUID;
                    }
                    else
                        ret = Guid.NewGuid();

                    return ret;
                }
                finally
                {
                    FRSSite.Log.Add(LogEntryKind.Checkpoint, "Selected connection is: " + ret);
                }
            }
        }

        void printRpcRetCode(uint t)
        {
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "RPC call return code: " + t);
        }

        #region CheckConnectivity
        /// <summary>
        /// This method is used for a client to check whether the server 
        /// is reachable and has been configured to replicate with the server.
        /// </summary>
        /// <param name="replicationID">From the GUID of the replication group</param>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <returns>error_status_t</returns>
        public error_status_t CheckConnectivity(string replicationID, int connectionId)
        {
            ConnectionSetId = getConnectionSetId(connectionId);
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "Selected connection id is: " + ConnectionSetId);
            switch (replicationID)
            {
                case "A": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId1,
                                        "(objectClass=msDFSR-ReplicationGroup)");
                    break;
                case "B": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId2,
                                      "(objectClass=msDFSR-ReplicationGroup)");
                    break;
                case "C": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId3,
                                      "(objectClass=msDFSR-ReplicationGroup)");
                    break;
                case "D": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId4,
                                        "(objectClass=msDFSR-ReplicationGroup)");
                    break;

                case "P": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId5,
                                       "(objectClass=msDFSR-ReplicationGroup)"); break;
            }

            uint RetVal = GenerateMessages.CheckConnectivity(bindingHandle, replicationSetId, ConnectionSetId);
            printRpcRetCode(RetVal);
            if ((uint)error_status_t.ERROR_SUCCESS == RetVal)
            {
                return error_status_t.ERROR_SUCCESS;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }

        #endregion

        #region EstablishConnection

        /// <summary>
        /// This method establishes a logical connection from a client to a server.
        /// </summary>
        /// <param name="replicationID">From the GUID of the replication group</param>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="downStreamProtocolVersion">Identifies the version of the protocol implemented by the client.</param>
        /// <param name="upstreamProtocolVersion">Receives the server's protocol version number. Expected values are the same as for downstreamProtocolVersion</param>
        /// <param name="upstreamFlags">A flags bitmask.</param>
        /// <returns>error_status_t</returns>

        public error_status_t EstablishConnection(string replicationID,
                                                   int connectionId,
                                                   ProtocolVersion downStreamProtocolVersion,
                                                   out ProtocolVersionReturned upstreamProtocolVersion,
                                                   out UpstreamFlagValueReturned upstreamFlags)
        {
            uint upFlags = 0;
            uint downstreamFlags = 0;
            uint upstreamProtoVersion = 0;
            uint downversion = 0;
            Guid serverConnectioId = new Guid();

            ConnectionSetId = getConnectionSetId(connectionId);

            switch (replicationID)
            {
                case "A": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId1,
                                            "(objectClass=msDFSR-ReplicationGroup)");
                    break;
                case "B": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId2,
                                           "(objectClass=msDFSR-ReplicationGroup)");
                    break;
                case "C": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId3,
                                           "(objectClass=msDFSR-ReplicationGroup)");
                    break;
                case "D": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId4,
                                            "(objectClass=msDFSR-ReplicationGroup)");
                    break;

                case "P": replicationSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ReplicaId5,
                                  "(objectClass=msDFSR-ReplicationGroup)"); break;
            }

            downversion = (uint)downStreamProtocolVersion;


            uint retVal = 0;
            retVal = GenerateMessages.EstablishConnection(bindingHandle,
                                                            replicationSetId,
                                                            ConnectionSetId,
                                                                  downversion,
                                                            downstreamFlags,
                                                            out upstreamProtoVersion,
                                                            out upFlags);
            printRpcRetCode(retVal);

            if ((uint)error_status_t.ERROR_SUCCESS == retVal)
            {

                #region MS-FRS2_R3
                //
                //While Creating the RPCBinding Handle, it was specifed athuntication levels and authentication services 
                //as an in parameter to RpcBindingSetAuthInfo Method and this call is succeeded,hence this requirement is verified.
                //
                FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                        retVal,
                                                        3,
                                                        "All traffic MUST be authenticated and encrypted using LAN Manager" +
                                                        "or Kerberos over TCP/IP, which requires that the client specify to" +
                                                        "use the protocol sequence associated with RPC over TCP/IP, and " +
                                                        "requires the client to specify packet privacy and authentication" +
                                                        "negotiation.");
                #endregion

                #region MS-FRS2_R4
                //
                //While Creating the RPCBinding Handle, it was specifed athuntication levels and 
                //authentication services as an in parameter to RpcBindingSetAuthInfo Method and 
                //this call is succeeded,hence this requirement is verified.
                //
                FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                        retVal,
                                                        4,
                                                        "Both client and server MUST require authentication and " +
                                                        "encryption.");
                #endregion

                #region MS-FRS2_R6
                //While creating the bindingHandle, it is specified authentication and authorization levels as in 
                //parameter to RpcBindingSetAuthInfo method and this call is succeeded hence the requirement is verified
                FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                        retVal,
                                                        6,
                                                        "Server MUST require authentication and encryption.");
                #endregion

                #region MS-FRS2_R7
                //
                //While creating the RPC Bidning Handle, this value is passed as an inparameter to RpcStringBindingCompose
                //method to specify the transport protocol sequence and this call is succeded hence verified.
                //
                if (ConfigStore.TransportProtocol.Equals("ncacn_ip_tcp"))
                {
                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                            retVal,
                                                            7,
                                                            "[For authentication and encryption] Protocol sequence is set to"
                                                            + "ncacn_ip_tcp");
                }
                #endregion

                #region MS-FRS2_R8
                //
                //While creating the Binding handle, This value is passed as a parameter to RpcStringBindingCompose method
                //to specify the End point Guid and this call is succeded hence verified.
                //
                if (objectUUID.Equals("5bc1ed07-f5f5-485f-9dfd-6fd0acf9a23c"))
                {

                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                           retVal,
                                                           8,
                                                           "[For authentication and encryption] DFSR_ENDPOINT_GUID is set to"
                                                           + "5bc1ed07-f5f5-485f-9dfd-6fd0acf9a23c.");
                }
                #endregion

                if (authLevels == (int)RPC_C_AUTHN_LEVEL.RPC_C_AUTHN_LEVEL_PKT_PRIVACY)
                {
                    #region MS-FRS2_R9
                    //
                    //Verifying Authentication level is set to RPC_C_AUTHN_LEVEL_PKT_PRIVACY
                    // Verified by calling RpcBindingSetAuthInfo method with authentication level set to RPC_C_AUTHN_LEVEL_PKT_PRIVACY
                    // if the EstablishConnection Opnum return success then the requirement is verified.
                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                            retVal,
                                                            9,
                                                            "[For authentication and encryption] Authentication level is set to"
                                                            + "RPC_C_AUTHN_LEVEL_PKT_PRIVACY");
                    #endregion
                }
                if (authService == (int)RPC_C_AUTHN_SVC.RPC_C_AUTHN_GSS_NEGOTIATE)
                {
                    #region MS-FRS2_R10
                    // Verifying Authentication service is set to RPC_C_AUTHN_GSS_NEGOTIATE.
                    // Verified by calling RpcBindingSetAuthInfo method with authentication level set to RPC_C_AUTHN_GSS_NEGOTIATE.
                    // if the EstablishConnection Opnum return success then the requirement is verified.

                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                              retVal,
                                                              10,
                                                              "[For authentication and encryption] Authentication service is set to"
                                                              + "RPC_C_AUTHN_GSS_NEGOTIATE.");
                    #endregion
                }
                else if (authService == (int)RPC_C_AUTHN_SVC.RPC_C_AUTHN_GSS_KERBEROS)
                {
                    #region MS-FRS2_R11
                    //Verifying Authentication service is set to  RPC_C_AUTHN_GSS_KERBEROS.
                    // Verified by calling RpcBindingSetAuthInfo method with authentication level set to  RPC_C_AUTHN_GSS_KERBEROS..
                    // if the EstablishConnection Opnum return success then the requirement is verified.


                    FRSSite.CaptureRequirement(11, "[For authentication and encryption] Authentication service is set to"
                                                + "RPC_C_AUTHN_GSS_KERBEROS");
                    #endregion
                }
                else if (authService == (int)RPC_C_AUTHN_SVC.RPC_C_AUTHN_WINNT)
                {
                    #region MS-FRS2_R12
                    //Verifyng Authentication service is set to RPC_C_AUTHN_WINNT.
                    // Verified by calling RpcBindingSetAuthInfo method with authentication level set to  RPC_C_AUTHN_WINNT.
                    // if the EstablishConnection Opnum return success then the requirement is verified.

                    FRSSite.CaptureRequirement(12, "[For authentication and encryption] Authentication service is set to"
                                                + "RPC_C_AUTHN_WINNT.");
                    #endregion
                }
                #region MS-FRS2_R13
                if (Convert.ToBoolean(ConfigStore.MAY_MS_FRS2_R13) == true)
                {

                    //Verifying A server MAY specify a static port for all DFS-R RPC traffic
                    FRSSite.CaptureRequirement(13, "A server MAY specify a static port for all DFS-R RPC traffic");
                }
                #endregion

                if (Convert.ToBoolean(ConfigStore.MAY_MS_FRS2_R14) == true)
                {

                    #region MS-FRS2_R14
                    //Verifying A server MAY use dynamic endpoints and rely on the endpoint mapper to relay inbound requests
                    //that use the endpoint GUID into the DFS-R service.
                    //Verified by setting the pszEndpoint variable to null in the RPCBindingHandle method. if the call succeeds
                    //hence verified.
                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                            retVal,
                                                            14,
                                                            "A server MAY use dynamic endpoints " +
                                                            "and rely on the endpoint mapper to relay " +
                                                            "inbound requests that use the endpoint GUID" +
                                                            "into the DFS-R service.");
                    #endregion
                }

                if (ConfigStore.IsWindows == true)
                {
                    #region MS-FRS2_R15
                    //Verifying In the Windows implementation the default behavior of a server is to 
                    //use dynamic endpoints.<1>
                    //Verified by setting the pszEndpoint variable to null 
                    //in the RPCBindingHandle method and the  call succeeds
                    //hence verified.
                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                            retVal,
                                                            15,
                                                            "In the Windows implementation" +
                                                            "the default behavior of a server is to " +
                                                            "use dynamic endpoints.<1>");
                    #endregion
                }
                #region MS-FRS2_R16
                if (ConfigStore.IsWindows == true)
                {
                    if (Convert.ToBoolean(ConfigStore.StaticPort))
                    {
                        //Verifying In the Windows implementation Static ports can be specified on a connection using the attribute DNSHostName.<1>
                        FRSSite.CaptureRequirementIfAreEqual<uint>(
                            0,
                            retVal,
                            16,
                            "In the Windows implementation Static ports can be specified on a connection using the attribute DNSHostName.<1>");

                    }
                }
                #endregion

                if (Convert.ToBoolean(ConfigStore.MAY_MS_FRS2_R32) == true)
                {
                    #region MS-FRS2_R32
                    //Verifying A server MAY use the endpoint UUID to register a dynamic endpoint.
                    //Verified by setting the UUID to  5bc1ed07-f5f5-485f-9dfd-6fd0acf9a23c in RpcStringBindingCompose.
                    //if the call succeeds hence verified.
                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                            retVal,
                                                            32,
                                                            "A server MAY use the endpoint UUID to " +
                                                            "register a dynamic endpoint.");
                    #endregion
                }

                if (Convert.ToBoolean(ConfigStore.MAY_MS_FRS2_R31) == true)
                {
                    #region MS-FRS2_R31
                    //Verifying A server MAY bind on a static port.
                    FRSSite.CaptureRequirementIfAreEqual<uint>
                        (0,
                        retVal,
                        31,
                        "A server MAY bind on a static port.");
                    #endregion
                }

                if (ConfigStore.IsWindows == true)
                {
                    #region MS-FRS2_R33
                    //Verifying In the Windows implementation, the default behavior of a Windows-based server 
                    //is to use dynamic endpoints.<2>
                    //Verified by setting the endpoits to null in RpcStringBindingCompose method.If the call 
                    //succeeds hence verified.
                    FRSSite.CaptureRequirementIfAreEqual<uint>(0,
                                                            retVal,
                                                            33,
                                                            "In the Windows implementation, the " +
                                                            "default behavior of a Windows-based server " +
                                                            "is to use dynamic endpoints.<2>");
                    #endregion
                }
                #region MS-FRS2_R34
                if (ConfigStore.IsWindows == true)
                {
                    if (Convert.ToBoolean(ConfigStore.StaticPort))
                    {
                        //Verifying In the Windows implementation, static ports can be specified on a connection 
                        //using the attribute DNSHostName.<2>
                        FRSSite.CaptureRequirementIfAreEqual<uint>
                            (0,
                             retVal,
                             34,
                             "In the Windows implementation, static ports can be specified on a connection using the attribute DNSHostName.<2>");

                    }
                }
                #endregion
                if (ConfigStore.IsWindows == true)
                {
                    if (ConfigStore.Win2k8)
                    {
                        //Verifying Windows Server 2008 identify its DFS-R protocol version as 
                        //FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER.
                        if ((uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER == downversion)
                        {
                            FRSSite.CaptureRequirement(40, "<WB>Windows Server 2008 identify its DFS-R protocol version as" +
                                                          "FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER.");
                        }
                    }


                    if (ConfigStore.Win2K8R2)
                    {
                        //Verifying Windows Server 2008 R2 identify its DFS-R protocol version as 
                        //FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER.
                        if ((uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER == downversion)
                        {
                            FRSSite.CaptureRequirement(41, "<WB>Windows Server 2008 R2 identify its DFS-R protocol version as" +
                                                             "FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER.");
                        }
                    }
                }


                #region MS-FRS2_R72

                //Verifying TRANSPORT_SUPPORTS_RDC_SIMILARITY bitmask
                //flag value is usedto indicate to a client that a DFS-R server 
                //is capable of using the similarity features of RDC 
                FRSSite.CaptureRequirementIfAreEqual<uint>(1,
                                                        upFlags,
                                                        72,
                                                        "TRANSPORT_SUPPORTS_RDC_SIMILARITY bitmask flag value is used to indicate" +
                                                       "to a client that a DFS-R server is capable of using the similarity features of RDC ");
                #endregion

                #region MS-FRS2_R430
                //This is a partial requirement. The client can set downstreamFlags to either 0 or non-zero, but
                //server responds in the same way for both the conditions.
                FRSSite.CaptureRequirementIfAreEqual<uint>(0, retVal, 430, "[EstablishConnection] Reply from the server is" +
                                                        "the same whether 0 or non-zero is used for downstreamFlags.");
                #endregion MS-FRS2_R430

                #region MS-FRS2_R431
                //The upstreamProtocolVersion is an out-parameter and the server fills it protocol version in this
                //parameter.
                switch (ConfigStore.OSVer)
                {
                    case SUTOSVersion.Win2008:
                    case SUTOSVersion.Win2008R2:
                        FRSSite.CaptureRequirementIfAreEqual<uint>
                            ((uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, upstreamProtoVersion, 431,
                            "[EstablishConnection]upstreamProtocolVersion receives the version of the DFS-R protocol" +
                            "implemented by the server.");
                        break;
                    case SUTOSVersion.Win2012:
                        FRSSite.CaptureRequirementIfAreEqual<uint>
                 ((uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_WIN8_SERVER, upstreamProtoVersion, 431,
                 "[EstablishConnection]upstreamProtocolVersion receives the version of the DFS-R protocol" +
                 "implemented by the server.");
                        break;
                    case SUTOSVersion.Win2012R2:
                        FRSSite.CaptureRequirementIfAreEqual<uint>
                 ((uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_WIN2012R2_SERVER, upstreamProtoVersion, 431,
                 "[EstablishConnection]upstreamProtocolVersion receives the version of the DFS-R protocol" +
                 "implemented by the server.");
                        break;
                }
                #endregion MS-FRS2_R431

                #region MS-FRS2_R432
                //Verifying The server MUST set the TRANSPORT_SUPPORTS_RDC_SIMILARITY  bit flag if the server 
                //supports RDC similarity 

                FRSSite.CaptureRequirementIfAreEqual<uint>(1,
                                                    upFlags,
                                                    432,
                                                    "The server MUST set the TRANSPORT_SUPPORTS_RDC_SIMILARITY " +
                                                    "bit flag if the server supports RDC similarity ");
                #endregion

                #region MS-FRS2_R367
                //Verifying Each msDFSR-Connection object MUST NOT use the same msDFSR-Member object as the client and server.
                //Verified by comparing server Connectiond id and Client Connection id and if they are not equal and the 
                //call is success hence the requirement is verified.
                FRSSite.CaptureRequirementIfIsTrue(serverConnectioId != ConnectionSetId && retVal == 0,
                                                 367,
                                                 @"Each msDFSR-Connection object MUST NOT use the same msDFSR-Member object as the client and server.");
                #endregion


                //Out parameter upstramFlags
                if (((upFlags == (uint)_SupportsRDC.SupportsRDC) || (upFlags == (uint)_SupportsRDC.notSupportsRDC)))
                {
                    upstreamFlags = UpstreamFlagValueReturned.Valid;
                }
                else
                {
                    upstreamFlags = UpstreamFlagValueReturned.Invalid;
                }
                //out parameter upStreamProcolVersion.
                upstreamProtocolVersion = ProtocolVersionReturned.Invalid;
                FRSSite.Log.Add(LogEntryKind.Checkpoint, "Current OS Ver: " + ConfigStore.OSVer);
                FRSSite.Log.Add(LogEntryKind.Checkpoint, "Returned OS Ver: " + upstreamProtoVersion);
                switch (ConfigStore.OSVer)
                {
                    case SUTOSVersion.Win2012:
                        if (upstreamProtoVersion == (uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_WIN8_SERVER)
                            upstreamProtocolVersion = ProtocolVersionReturned.Valid;
                        else
                        {
                            return error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION;
                        }
                        break;
                    case SUTOSVersion.Win2012R2:
                        if (upstreamProtoVersion == (uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_WIN2012R2_SERVER)
                            upstreamProtocolVersion = ProtocolVersionReturned.Valid;
                        else
                        {
                            return error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION;
                        }
                        break;
                    case SUTOSVersion.Win2008:
                        if (upstreamProtoVersion == (uint)ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER)
                            upstreamProtocolVersion = ProtocolVersionReturned.Valid;
                        else
                        {
                            return error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION;
                        }
                        break;
                }
                return error_status_t.ERROR_SUCCESS;
            }
            else
            {
                upstreamFlags = UpstreamFlagValueReturned.Invalid;
                upstreamProtocolVersion = ProtocolVersionReturned.Invalid;
                //return (error_status_t)retVal;

                if (retVal == (uint)error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION)
                {
                    return error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION;
                }
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region EstablishSession

        /// <summary>
        /// Enabling condition for EstablishSession success case
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentId">The GUID of the configured replicated folder.</param>
        /// <returns>error_status_t</returns>

        public error_status_t EstablishSession(int connectionId, int contentId)
        {
            ConnectionSetId = getConnectionSetId(connectionId);

            switch (contentId)
            {
                case 1: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId1,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 2: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId2,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 3: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId3,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 4: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId4,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 6: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId6,
                                    "(objectClass=msDFSR-ContentSet)");
                    break;
            }
            uint retVal = GenerateMessages.EstablishSession(bindingHandle,
                                                            ConnectionSetId,
                                                            contentsetId);
            printRpcRetCode(retVal);
            if ((uint)error_status_t.ERROR_SUCCESS == retVal)
            {
                #region MS-FRS2_R249
                //Verifying Configuration Objects are used in the RPC messages and MUST be known
                //to both server and client in order for partners to establish trust, communication, 
                //and which folders are replicated among them.

                //Establish connection and EstablishSession opnums have the in parameters ConnectionId,ContentSetId,
                //ReplicatonSetId, and the opnums are successful hence the requirement is verified.
                FRSSite.CaptureRequirementIfIsTrue(retVal == 0,
                                                 249,
                                                 @"Configuration Objects are used in the RPC messages
                                                 and MUST be known to both server and client in order 
                                                 for partners to establish trust, communication, and which 
                                                 folders are replicated among them.");
                #endregion
            }
            switch (retVal)
            {
                case (uint)Error.FRS_ERROR_CONNECTION_INVALID:
                    return error_status_t.FRS_ERROR_CONNECTION_INVALID;
                case (uint)Error.FRS_ERROR_CONTENTSET_READ_ONLY:
                    return error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY;
                case (uint)Error.FRS_ERROR_SUCCESS:
                    return error_status_t.ERROR_SUCCESS;
                default:
                    return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        /// <summary>
        /// Overriden Reset method of ManagedAdapterBase
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }

        uint m_asyncRetVal = 0;
        #region AsyncPoll
        /// <summary>
        /// This method used to register an asynchronous callback for a server to provide version chain vectors.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <returns>error_status_t</returns>
        // public static _FRS_ASYNC_RESPONSE_CONTEXT response;
        public error_status_t AsyncPoll(int connectionId)
        {
            int retryCapTime = 4;
            do
            {
                retryCapTime--;
                ConnectionSetId = getConnectionSetId(connectionId);
                m_asyncRetVal = 0;
                // uint retVal = 0;
                flagAsyncpollReq = false;
                //Asyncpoll Thread.
                if (AsyncThread != null)
                {
                    try
                    {
                        AsyncThread.Abort();
                    }
                    catch
                    {
                    }
                }
                AsyncThread = new Thread(delegate()
                {
                    try
                    {
                        m_asyncRetVal = GenerateMessages.FRS2AsyncPoll(bindingHandle, ConnectionSetId, out asyncResponsePtr);

                        FRSSite.Log.Add(LogEntryKind.Checkpoint, "Received asyncPoll response retval: {0}", m_asyncRetVal.ToString());
                        asyncResponse = FRS2Helpers.GetAsyncResponse(asyncResponsePtr);
                        FRSSite.Log.Add(LogEntryKind.Checkpoint, "Received asyncResponse.status: {0}", asyncResponse.status);
                        FRSSite.Log.Add(LogEntryKind.Checkpoint, "Received asyncResponse.sequencenumber: {0}", asyncResponse.sequenceNumber);
                        FRSSite.Log.Add(LogEntryKind.Checkpoint, "Received asyncResponse.result.versionVectorCount: {0}", asyncResponse.result.versionVectorCount);
                        FRSSite.Log.Add(LogEntryKind.Checkpoint, "Received asyncResponse.result.epoqueVectorCount: {0}", asyncResponse.result.epoqueVectorCount);
                        FRSSite.Log.Add(LogEntryKind.Checkpoint, "Received asyncResponse.result.vvGeneration: {0}", asyncResponse.result.vvGeneration);

                    }
                    catch (Exception e)
                    {
                        if (e is ThreadAbortException)
                            return;
                        FRSSite.Assert.Fail("Failed on AsyncPoll: {0}", (e.InnerException == null ? e.Message : e.InnerException.Message));
                    }
                    flagAsyncpollReq = true;
                });
                AsyncThread.Start(); //Starting the Thread.
                System.Threading.Thread.Sleep(Convert.ToInt32(ConfigStore.SleepTime));
                flagAsyncpollRequested = true;
                FRSSite.Log.Add(LogEntryKind.Checkpoint, "AsyncPoll return value is {0}", m_asyncRetVal);
                if (m_asyncRetVal == 9033)
                {
                    FRSSite.Log.Add(LogEntryKind.Checkpoint, "Receive a shutdown");
                    continue;
                }
                if (m_asyncRetVal == (uint)Error.FRS_ERROR_SUCCESS)
                {
                    return error_status_t.ERROR_SUCCESS;
                }
                else
                {
                    return error_status_t.ERROR_FAIL;
                }
            } while (retryCapTime > 0);

            if (m_asyncRetVal == (uint)Error.FRS_ERROR_SUCCESS)
            {
                return error_status_t.ERROR_SUCCESS;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region RequestRecords
        /// <summary>
        /// This method used to retrieve UIDs and GVSNs that a server persists.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentId">The GUID of the configured replicated folder.</param>
        /// <returns>error_status_t</returns>

        public error_status_t RequestRecords(int connectionId, int contentId)
        {
            System.UInt32 maxRecords = 20;
            System.UInt32 numRecords;
            System.UInt32 numBytes;
            System.UInt32 maxRecordsClientSpecified = maxRecords;
            IntPtr compressedRec = IntPtr.Zero;
            _RECORDS_STATUS recStatus;


            ConnectionSetId = getConnectionSetId(connectionId);


            switch (contentId)
            {
                case 1: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId1,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 2: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId2,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 3: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId3,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 4: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId4,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 5: contentsetId = new Guid(ConfigStore.ContentId5);
                    break;
            }

            uint retVal = GenerateMessages.FRS2RequestRecords(bindingHandle,
                                                              ConnectionSetId,
                                                              contentsetId,
                                                              uidDbGuid,
                                                              uidVersion,
                                                              ref maxRecords,
                                                              out numRecords,
                                                              out numBytes,
                                                              ref compressedRec,
                                                              out recStatus);

            printRpcRetCode(retVal);
            _FRS_ID_GVSN[] frsIdGvsn = FRS2Helpers.DecompressRecords(compressedRec);

            RecordsStatus recordStatus = RecordsStatus.RECORDS_STATUS_MORE;


            if (retVal == (uint)Error.FRS_ERROR_SUCCESS)
            {
                switch (recStatus)
                {
                    case _RECORDS_STATUS.RECORDS_STATUS_DONE:
                        recordStatus = RecordsStatus.RECORDS_STATUS_DONE;
                        break;

                    case _RECORDS_STATUS.RECORDS_STATUS_MORE:
                        recordStatus = RecordsStatus.RECORDS_STATUS_MORE;
                        break;
                }

                //Raise the Request Records Event before possible failed checker
                if (RequestRecordsEvent != null)
                    RequestRecordsEvent(recordStatus);

                //Verifying that in Windows Server 2008, and Windows Server 2008 R2 implementations of 
                //DFS-R will never return more than 1365 = 64 KB / size of(FRS_ID_GVSN) records even if the client 
                //specifies a larger maxRecords parameter value.<WB>
                if (ConfigStore.IsWindows)
                {
                    FRSSite.CaptureRequirementIfIsTrue(numRecords < 1365,
                                                    569,
                                                    "<WB>Windows Server 2003 R2, Windows Server " +
                                                    "2008, and Windows Server 2008 R2 implementations " +
                                                    "of DFS-R will never return more than 1365 = 64 KB / " +
                                                    "size of(FRS_ID_GVSN) records even if the client specifies " +
                                                    "a larger maxRecords parameter value.<WB>");
                }



                //Verifying that the maxRecords returned by the server never exceeds the one specified by the client.
                FRSSite.CaptureRequirementIfIsTrue((maxRecords <= maxRecordsClientSpecified), 568,
                "[RequestRecords]maxRecords is the maximum number of records that the server may send to the client." +
                "The server returns the lesser of the client-specified value and the maximum number of records that" +
                "the server is capable of sending.");

                if (ConfigStore.IsWindows)
                {
                    if (ConfigStore.Win2k8)
                    {
                        //For Windows Server 2008
                        //Verifying that the maxRecords returned by the server never exceeds 1365 records.

                        FRSSite.CaptureRequirementIfIsTrue((maxRecords <= 1365), 571,
                        "<WB> Windows Server 2008 implementation of DFS-R will never return more than 1365 = 64 KB / size"
                        + "of(FRS_ID_GVSN) records even if the client specifies a larger maxRecords parameter value.<WB>");
                    }

                    if (ConfigStore.Win2K8R2)
                    {
                        //For Windows Server 2008 R2
                        //Verifying that the maxRecords returned by the server never exceeds 1365 records.
                        FRSSite.CaptureRequirementIfIsTrue((maxRecords <= 1365), 572,
                        "<WB> Windows Server 2008 R2 implementation of DFS-R will never return more than 1365 = 64 KB / size"
                        + "of(FRS_ID_GVSN) records even if the client specifies a larger maxRecords parameter value.<WB>");
                    }
                }
                //Verifying that the server sends records as requested by the client.
                //Here, we are checking that the maxRecords returned by the server is less than that spesified by the
                //client
                FRSSite.CaptureRequirementIfIsTrue((maxRecords <= maxRecordsClientSpecified), 593,
                "[RequestRecords]Upon successfully validating the records request the server MUST send as many of" +
                "the requested records as possible, up to a limit of the lesser of the client-specified maxRecords" +
                "and the maximum number of records the server is capable of sending.");

                if (recStatus == _RECORDS_STATUS.RECORDS_STATUS_DONE)
                {
                    //[RequestRecords]When all updates have been supplied in the RequestRecords call, 
                    //the server MUST be able to resend all updates again if another round of RequestRecords arrives.
                    FRSSite.CaptureRequirementIfIsNotNull(compressedRec, 599,
                        "[RequestRecords]When all updates have been supplied in the RequestRecords call, " +
                        "the server MUST be able to resend all updates again if another round of RequestRecords" +
                        " arrives.");

                }
                return error_status_t.ERROR_SUCCESS;
            }

            else if (retVal == (uint)0x00002344)
            {
                return error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region RequestUpdates

        /// <summary>
        /// This method is used to obtain file metadata in the form of updates from a server.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentSetId">The GUID of the configured replicated folder.</param>
        /// <param name="requestType">The value from the UPDATE_REQUEST_TYPE enumeration that indicates the type of replication updates requested.</param>
        /// <param name="versionVectDiff">Array of version Vectors</param>
        /// <returns>error_status_t</returns>

        public error_status_t RequestUpdates(int connectionId,
                                             int contentId,
                                             versionVectorDiff versionVectDiff)
        {
            uint creditsAvailable = Convert.ToUInt32(ConfigStore.CreditsAvailable);
            int hashRequested = Convert.ToInt32(ConfigStore.HashRequested);
            _UPDATE_REQUEST_TYPE updateRequestType = _UPDATE_REQUEST_TYPE.UPDATE_REQUEST_ALL;

            uint versionVectorDiffCount = asyncResponse.result.versionVectorCount;

            versionVector = new _FRS_VERSION_VECTOR[versionVectorDiffCount];

            _UPDATE_STATUS FrsUpdateStatus = _UPDATE_STATUS.UPDATE_STATUS_MORE;
            IntPtr gvsnDbGuid;
            System.UInt64 gvsnVersion;
            bool flags = true; ;
            frsUpdatePtr = Marshal.AllocHGlobal(15000);
            updateCount = 0;
            gvsnDbGuid = IntPtr.Zero;
            gvsnVersion = 0;
            if (versionVectDiff == versionVectorDiff.valid)
            {
                versionVector = asyncResponse.result.versionVector;
            }
            else
            {
                versionVector = new _FRS_VERSION_VECTOR[1];
                versionVector[0].dbGuid = new Guid(ConfigStore.InvalidGUID);
                versionVector[0].high = 0;
                versionVector[0].low = 1;
                versionVectorDiffCount = 1;
            }

            ConnectionSetId = getConnectionSetId(connectionId);

            switch (contentId)
            {
                case 1: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId1,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 2: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId2,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 3: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId3,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 4: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId4,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 5: contentsetId = new Guid(ConfigStore.ContentId5);
                    break;

            }

            updateRequestType = _UPDATE_REQUEST_TYPE.UPDATE_REQUEST_ALL;

            uint retVal = GenerateMessages.FRS2RequestUpdates(bindingHandle,
                                                              ConnectionSetId,
                                                              contentsetId,
                                                              creditsAvailable,
                                                              hashRequested,
                                                              updateRequestType,
                                                              versionVectorDiffCount,
                                                              versionVector,
                                                              frsUpdatePtr,
                                                              ref updateCount,
                                                              ref FrsUpdateStatus,
                                                              ref gvsnDbGuid,
                                                              ref gvsnVersion);
            printRpcRetCode(retVal);
            FRS2Helpers.getUpdates(frsUpdatePtr, updateCount, out frsUpdate);
            if (frsUpdate.Length == 0)
                FRSSite.Log.Add(LogEntryKind.Checkpoint, "No file update returned");
            FilePresense filePresent = FilePresense.fileDeleted;
            UPDATE_STATUS updateStatus;
            Marshal.FreeHGlobal(frsUpdatePtr);
            if (retVal == (uint)Error.FRS_ERROR_SUCCESS)
            {
                if (updateCount != 0)
                {
                    if (frsUpdate[0].present == present_Values.V2)
                    {
                        filePresent = FilePresense.fileExists;
                    }
                    else
                    {
                        filePresent = FilePresense.fileDeleted;
                    }
                }
                if (FrsUpdateStatus == _UPDATE_STATUS.UPDATE_STATUS_DONE)
                {

                    updateStatus = UPDATE_STATUS.UPDATE_STATUS_DONE;
                }
                else
                {
                    updateStatus = UPDATE_STATUS.UPDATE_STATUS_MORE;
                }

                //if (!flagTradTestCaseCheck && !flagTradTestCaseForPushSourceNeeds &&
                //    !flagTradTestCaseForRdcClose && !flagTradTestCaseForRdcGetSig &&
                //    !flagTradTestCaseForRdcGetSigfail && !flagTradTestCaseForRdcGetSigfailForLevel
                //    && !flagTradTestCaseForPushSourceNeedsForNeedCount)
                //{
                if (RequestUpdatesEvent != null)
                    RequestUpdatesEvent(filePresent, updateStatus);
                //}


                #region RequirementValidation
                if (updateCount != 0)
                {

                    flags = true;
                    for (int j = 0; j < frsUpdate[0].rdcSimilarity.Length; j++)
                    {
                        if (frsUpdate[0].rdcSimilarity[j] != 0)
                        {
                            flags = false;
                            break;
                        }
                    }

                    #region MS-FRS2_R158
                    //Verifying The value of the rdcSimilarity field in FRS_UPDATE structure
                    //will be all zeros if the similarity data was not computed.
                    FRSSite.CaptureRequirementIfIsTrue(flags,
                                                    158,
                                                    "The value of the rdcSimilarity field in FRS_UPDATE " +
                                                    "structure will be all zeroes if the similarity data was not computed. ");
                    #endregion

                    flags = true;
                    for (int i = 0; i < updateCount; i++)
                    {
                        if (frsUpdate[i].present != present_Values.V1 && frsUpdate[i].present != present_Values.V2)
                        {
                            flags = false;
                            break;
                        }
                    }
                    #region MS-FRS2_R144

                    //Verifying The value of the present field in the FRS_UPDATE structure MUST be either 0 or 1. 
                    FRSSite.CaptureRequirementIfIsTrue(flags,
                                                    144,
                                                    "The value of the present field in the FRS_UPDATE structure MUST be either 0 or 1. ");
                    #endregion

                    for (int i = 0; i < updateCount; i++)
                    {

                        #region MS-FRS2_R148
                        //Verifying The value of the nameConflict field in FRS_UPDATE structure MUST 0.
                        FRSSite.CaptureRequirementIfIsTrue((frsUpdate[i].nameConflict == 0) || (frsUpdate[i].nameConflict == 1),
                                                         148,
                                                         "The value of the nameConflict field in FRS_UPDATE structure MUST be either 0 or 1.");
                        #endregion

                        #region MS-FRS2_R150
                        //Verifying The nameConflict field in FRS_UPDATE structure MUST be 0 if present field is 1.
                        if (frsUpdate[i].present == present_Values.V2)
                        {
                            FRSSite.CaptureRequirementIfIsTrue(
                                                            frsUpdate[i].nameConflict == 0,
                                                            150,
                                                            @"The nameConflict field in FRS_UPDATE structure MUST be 0 if present field is 1.");
                        }

                        #endregion

                    }

                    flags = true;
                    for (int i = 0; i < updateCount; i++)
                    {
                        if (frsUpdate[i].gvsnVersion == 0)
                        {
                            flags = false;
                            break;
                        }
                    }
                    #region MS-FRS2_R164
                    //Verifying The gvsnVersion field in FRS_UPDATE structure is assigned when the file was last updated.
                    FRSSite.CaptureRequirementIfIsTrue(flags, 164,
                                                    "The gvsnVersion field in FRS_UPDATE structure is assigned when the file was last updated.");
                    #endregion

                    flags = true;
                    for (int i = 0; i < updateCount; i++)
                    {
                        if (frsUpdate[i].flags != 0)
                        {
                            flags = false;
                            break;
                        }
                    }
                    #region MS-FRS2_R169
                    //Verifying In FRS_UPDATE Structure, flags MUST be 0. 
                    FRSSite.CaptureRequirementIfIsTrue(flags,
                                                    169,
                                                    "In FRS_UPDATE Structure, flags MUST be 0. ");
                    #endregion

                    #region MS-FRS2_495
                    //Verifying Upon successfully validating the update request, the server MUST send as many 
                    //of the requested updates as fit in the frsUpdate buffer. 
                    //Verified by checking the value of the pdatecount, if it is nonzero hence the requirement is verified.
                    if (retVal == 0)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(updateCount != 0,
                                                        495,
                                                        @"Upon successfully validating the update request, the server MUST send 
                                                as many of the requested updates as fit in the frsUpdate buffer. ");
                    }
                    #endregion

                    #region MS-FRS2_R908
                    //Server responds in the way as client requests.
                    FRSSite.CaptureRequirement(908,
                                            "A DFS-R server MUST be agnostic to the specific way that a client " +
                                            "chooses to throttle processing updates.");
                    #endregion

                    string fileType = serverControllerAdapter.GetDFSRFilters(contentId, "msDFSR-FileFilter");

                    flags = true;
                    for (int i = 0; i < updateCount; i++)
                    {
                        if (frsUpdate[i].name.ToString().Contains(fileType))
                        {
                            flags = false;
                        }
                    }
                    //Verified by quering AD for the DFSR-FileFilter and checking for the file extention in the update structure.
                    #region MS-FRS2_R326
                    //Verifying In msDFSR-FileFilter  attribute in msDFSR-ContentSet, any file whose name matches any of the filters SHOULD be excluded from replication. 


                    if (ConfigStore.Should)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(flags,
                                                           326,
                                                           @"In msDFSR-FileFilter  attribute in msDFSR-ContentSet,
                                                            any file whose name matches any of the filters SHOULD be excluded from replication. ");


                    }
                    #endregion

                    #region MS-FRS2_R327
                    //Verifying In msDFSR-File attribute in msDFSR-ContentSet, Windows does exclude the file 
                    //from replication whose name matches any of the filters. if the name field in the update
                    //structure does not have the specified file extention hence the requirement is verified.
                    if (ConfigStore.IsWindows)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(flags,
                                                           327,
                                                           @"In msDFSR-File attribute in msDFSR-ContentSet, Windows does exclude the file from replication 
                                                    whose name matches any of the filters. ");
                    }

                    #endregion

                    string directoryName = serverControllerAdapter.GetDFSRFilters(contentId, "msDFSR-DirectoryFilter");

                    flags = true; ;
                    for (int i = 0; i < updateCount; i++)
                    {
                        if (frsUpdate[i].name.ToString().Equals(directoryName))
                        {
                            flags = false;
                        }
                    }
                    #region MS-FRS2_R331

                    if (ConfigStore.Should)
                    {
                        //Verifying In msDFSR-DirectoryFilter attribute in msDFSR-ContentSet,
                        //any folder whose name matches any of the filters SHOULD be excluded from replication.
                        //Verified by comparing the name attribute in frsupdate structure with the msDFSR-DirectoryFilter
                        //if the Directory name is not found, then the requirement is verified.
                        FRSSite.CaptureRequirementIfIsTrue(flags,
                                                          331,
                                                          @"In msDFSR-DirectoryFilter attribute in msDFSR-ContentSet,
                                                                    any folder whose name matches any of the filters SHOULD
                                                                    be excluded from replication.");
                    }
                    #endregion

                    #region MS-FRS2_R332
                    if (ConfigStore.IsWindows)
                    {
                        //Verifying In msDFSR-DirectoryFilter attribute in msDFSR-ContentSet,
                        //Windows does exculde the folder from replication whose name matches any of the filters.
                        FRSSite.CaptureRequirementIfIsTrue(flags,
                                                                   332,
                                                                   @"In msDFSR-DirectoryFilter attribute in msDFSR-ContentSet, 
                                                                    Windows does exculde the folder from replication whose name
                                                                    matches any of the filters.");
                    }
                    #endregion

                    for (int i = 0; i < updateCount; i++)
                    {
                        #region MS-FRS2_R148
                        //Verifying The value of the nameConflict field in FRS_UPDATE structure MUST 0.
                        FRSSite.CaptureRequirementIfIsTrue((frsUpdate[i].nameConflict == 0) || (frsUpdate[i].nameConflict == 1),
                                                         148,
                                                         "The value of the nameConflict field in FRS_UPDATE structure MUST be either 0 or 1.");

                        #endregion

                        #region MS-FRS2_R150
                        //Verifying The nameConflict field in FRS_UPDATE structure MUST be 0 if present field is 1.
                        if (frsUpdate[i].present == present_Values.V2)
                        {
                            FRSSite.CaptureRequirementIfIsTrue(
                                                            frsUpdate[i].nameConflict == 0,
                                                            150,
                                                            @"The nameConflict field in FRS_UPDATE structure MUST be 0 if present field is 1.");
                        }

                        #endregion

                    }
                }

                #endregion

                return error_status_t.ERROR_SUCCESS;
            }
            else if (retVal == (uint)Error.FRS_ERROR_CONTENTSET_NOT_FOUND)
            {
                return error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region RequestVersionVector

        /// <summary>
        /// This method is used to obtain the version chain vector persisted on a server.
        /// </summary>
        /// <param name="sequenceNo">a unique Sequence number</param>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentSetId">The GUID of the configured replicated folder.</param>
        /// <param name="requestType">The value from the VERSION_REQUEST_TYPE enumeration that describes the type of replication sync to perform.</param>
        /// <param name="changeType">The value from the VERSION_CHANGE_TYPE enumeration that indicates whether to notify change only, change delta, or send the entire version chain vector.</param>
        /// <param name="vvGeneration">vvGeneration</param>
        /// <returns>error_status_t</returns>

        public error_status_t RequestVersionVector(int sequenceNo,
                                            int connectionId,
                                            int contentId,
                                            VERSION_REQUEST_TYPE requestType,
                                            VERSION_CHANGE_TYPE changeType,
                                            VVGeneration vvGeneration)
        {
            ulong versionVectGenarat;
            _VERSION_REQUEST_TYPE versionRequestType = _VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC;
            _VERSION_CHANGE_TYPE versionChangeType = _VERSION_CHANGE_TYPE.CHANGE_ALL;
            vvGenerationCheck = vvGenerat;

            ConnectionSetId = getConnectionSetId(connectionId);

            switch (contentId)
            {
                case 1: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId1,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 2: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId2,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 3: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId3,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 4: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId4,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 5: contentsetId = new Guid(ConfigStore.ContentId5);
                    break;
                case 6: contentsetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId6,
                                      "(objectClass=msDFSR-ContentSet)");
                    break;
            }

            switch (requestType)
            {
                case VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC:
                    versionRequestType = _VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC;
                    break;

                case VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC:
                    versionRequestType = _VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC;
                    break;
                case VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC:
                    versionRequestType = _VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC;
                    break;
            }

            switch (changeType)
            {
                case VERSION_CHANGE_TYPE.CHANGE_ALL:
                    versionChangeType = _VERSION_CHANGE_TYPE.CHANGE_ALL;
                    break;

                case VERSION_CHANGE_TYPE.CHANGE_NOTIFY:
                    versionChangeType = _VERSION_CHANGE_TYPE.CHANGE_NOTIFY;
                    break;
            }
            if (vvGeneration == VVGeneration.ValidValue)
            {
                versionVectGenarat = vvGenerat;
            }
            else
            {
                versionVectGenarat = 9812;
            }
            uint sequenceNumber = (uint)sequenceNo;
            uint retVal = GenerateMessages.FRSRequestVersionVector(bindingHandle,
                                                                   sequenceNumber,
                                                                   ConnectionSetId,
                                                                   contentsetId,
                                                                   versionRequestType,
                                                                   versionChangeType,
                                                                   versionVectGenarat);

            printRpcRetCode(retVal);
            if (retVal == 87
                && (versionRequestType == _VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC || versionRequestType == _VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC) && (versionVectGenarat != 0 || versionChangeType != _VERSION_CHANGE_TYPE.CHANGE_ALL)
                && ConfigStore.IsTestSYSVOL)
                return error_status_t.ERROR_INVALID_PARAMETER;

            int count = 0;
            while (flagAsyncpollReq == false && count <= 10)
            {
                Thread.Sleep(1000);
                count++;
            }

            //should before all possible failed requirement checker and condition branch
            //if (!flagTradTestCaseCheck && !flagTradTestCaseForPushSourceNeeds &&
            //   !flagTradTestCaseForRdcClose && !flagTradTestCaseForRdcGetSig
            //   && !flagTradTestCaseForRdcGetSigfail && !flagTradTestCaseForRdcGetSigfailForLevel
            //   && !flagTradTestCaseForPushSourceNeedsForNeedCount)
            //{
            if (AsyncPollResponseEvent != null)
            {
                if (retVal == 0)
                {
                    AsyncPollResponseEvent(VVGeneration.ValidValue);
                }
                else
                {
                    AsyncPollResponseEvent(VVGeneration.InvalidValue);
                }
            }


            if ((!flagAsyncpollReq) && (flagAsyncpollRequested))
            {
                AsyncThread.Abort();
            }
            else
            {
                if (retVal == 0)
                {
                    if (flagAsyncpollRequested)
                    {
                        if (asyncResponse.status == 9033 || m_asyncRetVal == 9033)
                        {
                            //in shutdown, for now unknown reason
                            m_inShutdown = true;
                            FRSSite.Assert.Fail("in shutdown");
                        }

                        #region MS-FRS2_R555
                        //Upon successfull validation of Asyncpoll request server responds back with AsyncResponse.
                        FRSSite.CaptureRequirement(555,
                                                "[AsyncPoll]Upon successfully validating the asynchronous poll " +
                                                "request the server MUST register the callback with the specified outbound connection.");
                        #endregion

                        #region MS-FRS2_R1100
                        if (asyncResponse.result.versionVectorCount != 0)
                        {
                            //Verifying that whether the VSN is greater than 8 as 0-8 are reserved.
                            FRSSite.CaptureRequirementIfIsTrue((asyncResponse.result.versionVector[0].low >= 9) &&
                                                             (asyncResponse.result.versionVector[0].high >= 9),
                                                              1100,
                                                             "Version sequence numbers 0 - 8 are reserved; " +
                                                             "therefore, the versions of all other UIDs that " +
                                                             "correspond to replicated files MUST start with " +
                                                             "at least version 9.");
                        }
                        #endregion

                        #region MS-FRS2_R543
                        //Verifying The asynchronous response from the server that corresponds 
                        //to a version vector request MUST contain the same sequence number that
                        //was created by the client. 

                        FRSSite.CaptureRequirementIfAreEqual<uint>(sequenceNumber, asyncResponse.sequenceNumber,
                                                         543,
                                                         @"The asynchronous response from the server that corresponds to a
                                                        version vector request MUST contain the same
                                                        sequence number that was created by the client. ");
                        #endregion

                        #region MS-FRS2_R533
                        if (versionRequestType == _VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC && versionChangeType == _VERSION_CHANGE_TYPE.CHANGE_NOTIFY)
                        {
                            FRSSite.CaptureRequirementIfAreNotEqual<ulong>(asyncResponse.result.vvGeneration, 0,
                                                            533,
                                                            "[RequestVersionVector]When requestType is NORMAL_SYNC and "
                                                            + "changeType is CHANGE_NOTIFY:" +
                                                              "The server communicates its version vector " +
                                                              "time stamp information to the client when it " +
                                                              "responds to AsyncPoll requests.");
                        }

                        #endregion
                    }
                }


                #region Requirement Validation
                bool asyncReqFlag = true;
                if (retVal == 0)
                {

                    #region MS_FRS2_R536
                    //When requestType is NORMAL_SYNC and changeType is CHANGE_NOTIFY:
                    //The server MUST NOT provide any version vector with the callback.
                    if (versionRequestType == _VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC && versionChangeType == _VERSION_CHANGE_TYPE.CHANGE_NOTIFY)
                    {
                        if (Convert.ToBoolean(ConfigStore.Reqnoblocked536) == false)
                        {
                            FRSSite.CaptureRequirementIfIsTrue(
                                                            asyncResponse.result.versionVector.Equals(null),
                                                              536,
                                                             "When requestType is NORMAL_SYNC and changeType is" +
                                                             "CHANGE_NOTIFY:The server MUST NOT provide any" +
                                                             "version vector with the callback.");
                        }
                    }
                    #endregion

                    if (asyncResponse.result.versionVectorCount != 0)
                    {
                        for (int i = 0; i < asyncResponse.result.versionVectorCount; i++)
                        {
                            if (asyncResponse.result.versionVector[i].low > asyncResponse.result.versionVector[i].high)
                            {
                                asyncReqFlag = false;
                                break;
                            }
                        }
                        #region MS-FRS2_R125

                        //Verifying High value MUST be greater than low.
                        //Verified by checking in each version vector if low is high.
                        FRSSite.CaptureRequirementIfIsTrue(asyncReqFlag, 125, "High value MUST be greater than low.");

                        #endregion
                    }


                    bool asyncEpoqueFlag = true;
                    for (int i = 0; i < asyncResponse.result.epoqueVectorCount; i++)
                    {
                        if (asyncResponse.result.epoqueVector[i].machine != Guid.Empty)
                        {
                            asyncEpoqueFlag = false;
                            break;
                        }
                    }
                    #region MS-FRS2_R133
                    //Verifying In FRS_EPOQUE_VECTOR structure, machine field MUST be 0.
                    FRSSite.CaptureRequirementIfIsTrue(asyncEpoqueFlag,
                                                    133,
                                                    "In FRS_EPOQUE_VECTOR structure, machine field MUST be 0.");


                    #endregion


                    asyncEpoqueFlag = true;

                    for (int i = 0; i < asyncResponse.result.epoqueVectorCount; i++)
                    {
                        if (asyncResponse.result.epoqueVector[i].epoque.Equals(null))
                        {
                            asyncEpoqueFlag = false;
                            break;
                        }
                    }

                    #region MS-FRS2_R136

                    //In FRS_EPOQUE_VECTOR structure, epoque field MUST be zero..

                    FRSSite.CaptureRequirementIfIsTrue(asyncEpoqueFlag,
                                                    136,
                                                    "In FRS_EPOQUE_VECTOR structure, epoque field MUST be zero.");


                    #endregion

                }
                #endregion

                #region UpdateCancelrequirements
                if (updateCancelflag == true)
                {
                    _FRS_VERSION_VECTOR[] versionVector = new _FRS_VERSION_VECTOR[asyncResponse.result.versionVectorCount];
                    bool flagdbGuid = false;
                    bool flagVSN = false;
                    for (int versionVectorCount = 0; versionVectorCount < asyncResponse.result.versionVectorCount; versionVectorCount++)
                    {
                        Guid versionVectordbGuid = versionVector[versionVectorCount].dbGuid;
                        ulong low = versionVector[versionVectorCount].low;
                        ulong high = versionVector[versionVectorCount].high;

                        if (versionVectordbGuid == updateCancelgvsnDatabaseId)
                        {
                            flagdbGuid = true;
                        }
                        for (ulong vsn = low + 1; vsn <= high; vsn++)
                        {
                            if (updateCancelgvsnVersion == vsn)
                            {
                                flagVSN = true;
                            }
                        }
                    }

                    //Verifying that the server records the GVSN which was passed by the server in the UpdateCancel opnum.
                    //Here, firstly flagUpdateCancel is checked for TRUE, which signifies that the UpdateCancel opnum has
                    //been successfull. Then the returned GVSN in the AsyncPoll response is matched with the GVSN which
                    //was passed in the UpdateCancel opnum.
                    FRSSite.CaptureRequirementIfIsTrue((flagdbGuid == true) && (flagVSN == true), 612,
                                                   "[UpdateCancel]The server MUST record the GVSN from the call ");

                    //Verifying that the server includes the GVSN in the response which the client had send in the
                    //UpdateCancel opnum.
                    FRSSite.CaptureRequirementIfIsTrue((flagdbGuid == true) && (flagVSN == true), 613,
                        "[UpdateCancel]The server MUST include the GVSN, supplied in the UpdateCancel method call when" +
                        "completing subsequent or outstanding RequestVersionVector method calls for the replicated folder.");
                }
                #endregion UpdateCancelrequirements
            }

            if (retVal == (uint)Error.FRS_ERROR_SUCCESS)
            {
                return error_status_t.ERROR_SUCCESS;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region UpdateCancel

        /// <summary>
        /// This method is used by a client to indicate to a server that it could not process an update.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="cancelData">The FRS_UPDATE_CANCEL_DATA structure that describes an update to cancel.</param>
        /// <param name="contentSetFieldCANCEL_DATA"></param>
        /// <returns>error_status_t</returns>

        public error_status_t UpdateCancel(int connectionId,
                                           FRS_UPDATE_CANCEL_DATA Data,
                                           int contentSetFieldCANCEL_DATA)
        {
            ConnectionSetId = getConnectionSetId(connectionId);

            _FRS_UPDATE_CANCEL_DATA cancelData = new _FRS_UPDATE_CANCEL_DATA();
            _FRS_UPDATE frsupdate = new _FRS_UPDATE();
            frsupdate.attributes = 0;
            frsupdate.clock = new _FILETIME();
            frsupdate.contentSetId = new Guid();
            frsupdate.createTime = new _FILETIME();
            frsupdate.fence = new _FILETIME();
            frsupdate.flags = new flags_Values();
            frsupdate.gvsnDbGuid = new Guid();
            frsupdate.gvsnVersion = 0;
            frsupdate.hash = new byte[20];
            frsupdate.name = new short[262];
            frsupdate.nameConflict = 0;
            frsupdate.parentDbGuid = new Guid();
            frsupdate.parentVersion = 0;
            frsupdate.present = 0;
            frsupdate.rdcSimilarity = new byte[16];
            frsupdate.uidDbGuid = new Guid();
            frsupdate.uidVersion = 0;

            cancelData.blockingUpdate = frsupdate;
            switch (contentSetFieldCANCEL_DATA)
            {
                case 1: cancelData.contentSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId1,
                                       "(objectClass=msDFSR-ContentSet)");
                    break;
                case 2: cancelData.contentSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId2,
                                     "(objectClass=msDFSR-ContentSet)");
                    break;
                case 3: cancelData.contentSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId3,
                                     "(objectClass=msDFSR-ContentSet)");
                    break;
                case 4: cancelData.contentSetId = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId4,
                                     "(objectClass=msDFSR-ContentSet)");
                    break;
                case 5: cancelData.contentSetId = new Guid(ConfigStore.ContentId5);
                    break;

            }
            cancelData.gvsnDatabaseId = frsUpdate[0].gvsnDbGuid;
            cancelData.uidDatabaseId = new Guid();
            cancelData.parentDatabaseId = new Guid(); //frsUpdate[0].parentDbGuid;
            if (Data == FRS_UPDATE_CANCEL_DATA.valid)
            {
                cancelData.gvsnVersion = frsUpdate[0].gvsnVersion;
            }
            else
            {
                cancelData.gvsnVersion = 0;
            }
            cancelData.uidVersion = 0;
            cancelData.parentVersion = 0;
            cancelData.cancelType = cancelType_Values.UNSPECIFIED;
            cancelData.isUidValid = 0;
            cancelData.isParentUidValid = 0;
            cancelData.isBlockerValid = 0;
            IntPtr cancelDataPtr = FRS2Helpers.GetUpdateCancelData(cancelData);

            uint retVal = GenerateMessages.FRS2UpdateCancel(bindingHandle,
                                                              ConnectionSetId,
                                                              cancelDataPtr);
            printRpcRetCode(retVal);
            if (retVal == (uint)Error.FRS_ERROR_SUCCESS)
            {
                updateCancelflag = true;
                return error_status_t.ERROR_SUCCESS;
            }
            else if (retVal == (uint)Error.FRS_ERROR_CONTENTSET_NOT_FOUND)
            {
                return error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region InitializeFileTransferAsync

        /// <summary>
        /// This method is used by a client to start a file download.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="frsUpdateContentSetID">ContentSetId</param>
        /// <returns>error_status_t</returns>
        public error_status_t InitializeFileTransferAsync(int connectionId,
                                                   int frsUpdateContentSetID,
                                                    bool rdcCheck)
        {

            _FRS_RDC_FILEINFO[] fileInfo = new _FRS_RDC_FILEINFO[10];
            int rdcDesired;
            _FRS_REQUESTED_STAGING_POLICY stagingPolicy = _FRS_REQUESTED_STAGING_POLICY.SERVER_DEFAULT;
            _FRS_REQUESTED_STAGING_POLICY stagingPolicyCopy = stagingPolicy;
            IntPtr frsFileInfo = Marshal.AllocHGlobal(10000);
            IntPtr dataBufferPtr = Marshal.AllocHGlobal(10000);
            IntPtr test1 = Marshal.AllocHGlobal(700);
            uint sizeRead;
            uint bufferSize = Convert.ToUInt32(ConfigStore.BufferSize);
            Guid updateGuid = new Guid();
            switch (frsUpdateContentSetID)
            {
                case 1: updateGuid = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId1,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 2: updateGuid = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId2,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 3: updateGuid = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId3,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 4: updateGuid = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId4,
                                        "(objectClass=msDFSR-ContentSet)");
                    break;
                case 5: updateGuid = new Guid(ConfigStore.ContentId5);
                    break;
                case 6: updateGuid = serverControllerAdapter.InitializeGuid(ConfigStore.ContentId6,
                                    "(objectClass=msDFSR-ContentSet)");
                    break;
            }
            if (!flagTradTestCaseCheck && !flagTradTestCaseForPushSourceNeeds &&
                    !flagTradTestCaseForRdcClose && !flagTradTestCaseForRdcGetSig
                    && !flagTradTestCaseForRdcGetSigfail && !flagTradTestCaseForRdcGetSigfailForLevel
                    && !flagTradTestCaseForPushSourceNeedsForNeedCount)
            {
                if (frsUpdate.Length < 2 && ConfigStore.IsTestSYSVOL)
                    FRSSite.Assert.Fail("Should not come here");
                frsUpdate[1].contentSetId = updateGuid;
            }
            else
            {
                frsUpdate[countOfAsyncCall].contentSetId = updateGuid;
            }

            ConnectionSetId = getConnectionSetId(connectionId);

            if (rdcCheck == true)
            {
                rdcDesired = 1;
                clientRdcDesired = 1;
            }
            else
            {
                rdcDesired = 0;
                clientRdcDesired = 0;

            }
            _FRS_UPDATE reqUpdate;
            if (!flagTradTestCaseCheck && !flagTradTestCaseForPushSourceNeeds &&
                    !flagTradTestCaseForRdcClose && !flagTradTestCaseForRdcGetSig
                    && !flagTradTestCaseForRdcGetSigfail && !flagTradTestCaseForRdcGetSigfailForLevel
                    && !flagTradTestCaseForPushSourceNeedsForNeedCount)
            {
                reqUpdate = frsUpdate[1];
            }
            else
            {
                reqUpdate = frsUpdate[countOfAsyncCall];
            }

            byte[] frsUpdatebuff = FRS2Helpers.FrsUpdateStructToByteArray(reqUpdate);
            IntPtr frsUpdatePtr = Marshal.AllocHGlobal(frsUpdatebuff.Length);
            Marshal.Copy(frsUpdatebuff, 0, frsUpdatePtr, frsUpdatebuff.Length);

            uint retVal = GenerateMessages.FRS2InitializeFileTransferAsync(bindingHandle,
                                                                 ConnectionSetId,
                                                                 frsUpdatePtr,
                                                                 rdcDesired,
                                                                 ref stagingPolicy,
                                                                 out servContext,
                                                                 frsFileInfo,
                                                                 dataBufferPtr,
                                                                 bufferSize,
                                                                 out sizeRead,
                                                                 out isEndofFile);

            if (retVal == 0)
            {
                RDCFileInfo = FRS2Helpers.GetRdcFileInfo(frsFileInfo);
                Marshal.FreeHGlobal(frsFileInfo);

                ServerContext serverContext;
                RDC_Sig_Level signatureLevels;
                bool isEOF;
                if (servContext == IntPtr.Zero)
                {
                    serverContext = ServerContext.InvalidContext;
                }
                else
                {
                    serverContext = ServerContext.ValidContext;
                }
                if (RDCFileInfo.rdcSignatureLevels == 0)
                {
                    signatureLevels = RDC_Sig_Level.forRAWFileLevel;
                }
                else
                {
                    signatureLevels = RDC_Sig_Level.forRDCFileLevel;
                }
                if (isEndofFile == 0)
                {
                    isEOF = false;
                }
                else
                {
                    isEOF = true;
                }
                //if (!flagTradTestCaseCheck && !flagTradTestCaseForPushSourceNeeds &&
                //    !flagTradTestCaseForRdcClose && !flagTradTestCaseForRdcGetSig &&
                //    !flagTradTestCaseForRdcGetSigfail && !flagTradTestCaseForRdcGetSigfailForLevel
                //     && !flagTradTestCaseForPushSourceNeedsForNeedCount)
                //{
                if (InitializeFileTransferAsyncEvent != null)
                    InitializeFileTransferAsyncEvent(serverContext, signatureLevels, isEOF);
                //}

                if (rdcCheck == true)
                {
                    // for SYSVOL, by default the file is too small so no need RDC Chunk
                    if (!ConfigStore.IsTestSYSVOL)
                    {
                        //Verifying whether the value of RDC_CHUNKER_ALGORITHM  enumeration is 1 or not.
                        FRSSite.CaptureRequirementIfIsTrue((RDCFileInfo.rdcFilterParameters.rdcChunkerAlgorithm == 1),
                                                        81, "When the value of RDC_CHUNKER_ALGORITHM  enumeration is 1 it" +
                                                        " implies, RDC_FILTERMAX means FilterMax chunker algorithm is used.");

                        //Verifying whether the RDC_FILTERMAX = 1 value is sent when an RDC_CHUNKER_ALGORITHM enum value is required.
                        FRSSite.CaptureRequirementIfIsTrue((RDCFileInfo.rdcFilterParameters.rdcChunkerAlgorithm == 1),
                                                        82, "RDC_FILTERMAX value MUST be sent whenever an RDC_CHUNKER_ALGORITHM enum value is required.");

                        //Verifying whether in the FRS_RDC_PARAMETERS structure, the rdcChunkerAlgorithm field is RDC_FILTERMAX, 
                        //for compatibility.
                        FRSSite.CaptureRequirementIfIsTrue((RDCFileInfo.rdcFilterParameters.rdcChunkerAlgorithm == 1),
                                                        208, "In FRS_RDC_PARAMETERS structure, rdcChunkerAlgorithm field MUST" +
                                                             " be RDC_FILTERMAX,for compatibility.");
                    }
                }

                _FRS_UPDATE[] retUpdates = new _FRS_UPDATE[1];
                _FRS_UPDATE retFRSUpdates = new _FRS_UPDATE();
                FRS2Helpers.getUpdates(frsUpdatePtr, 1, out retUpdates);
                retFRSUpdates = retUpdates[0];
                Marshal.FreeHGlobal(frsUpdatePtr);


                if (isEndofFile == 1 && ConfigStore.IsWindows)
                {
                    byte[] dataBuffer = new byte[sizeRead];
                    long bufferAddress = (long)dataBufferPtr;
                    for (int i = 0; i < sizeRead; i++)
                    {
                        dataBuffer[i] = Marshal.ReadByte((IntPtr)bufferAddress);
                        bufferAddress += sizeof(byte);
                    }


                    ASCIIEncoding encoding = new ASCIIEncoding();
                    FSCCAdapter objParser = new FSCCAdapter();
                    Helper objHelper = new Helper();

                    ReplicatedFileStructure objDataBuffer = new ReplicatedFileStructure();
                    bool ParserResult = objParser.ValidateDataBuffer(dataBuffer, out objDataBuffer);

                    if (ParserResult)
                    {
                        #region MS-FRS2_R822

                        FRSSite.CaptureRequirementIfIsTrue(
                            ParserResult && (sizeRead <= bufferSize),
                            822, "In the Custom Marshaling Format if a stream" +
                            " requires multiple chunks, only the last header" +
                            " from that stream MUST have the" +
                            " HEADER_FLAGS_END_OF_STREAM bit set");

                        #endregion

                        #region MS-FRS2_R824

                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            824, "In the Custom Marshaling Format, the data MUST be tightly packed.");

                        #endregion

                        #region MS-FRS2_R825

                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            825, "In the Custom Marshaling Format There" +
                            " MUST NOT be any additional bytes of padding.");

                        #endregion

                        int countOfStreams = objDataBuffer.streamdata.Count;

                        #region MS-FRS2_R848

                        //Implicitly verified by the parser since, the parser
                        //reads the header before evaluating the stream.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            848, "In the Compressed Data Format Each block of data MUST have" +
                            " a data block header.");

                        #endregion

                        byte[] sig = objHelper.GetSubArray(
                                                objDataBuffer.signature.BlockSignature,
                                                0,
                                                4);

                        string signature = encoding.GetString(sig);

                        #region MS-FRS2_R849

                        FRSSite.CaptureRequirementIfAreEqual(4,
                                                          sig.Length,
                                                          849,
                                                          "In the Compressed Data Format, the data stream header" +
                                                          " MUST consist of a 4-byte signature");

                        #endregion

                        #region MS-FRS2_R851

                        FRSSite.CaptureRequirementIfAreEqual<string>("FRSX", signature,
                            851, "In the Compressed Data Format, the data stream header" +
                            " MUST consist of a 4-byte signature");

                        #endregion

                        #region MS-FRS2_R853

                        FRSSite.CaptureRequirementIfAreNotEqual((UInt32)0,
                            objDataBuffer.signature.BlockCompressedSize,
                            853, "In the Compressed Data Format The compressed size MUST NOT" +
                            " be equal to 0 for Block Compressed Size field.");

                        #endregion

                        #region MS-FRS2_R855

                        FRSSite.CaptureRequirementIfIsTrue(
                            objDataBuffer.signature.BlockUncompressedSize <= 8192,
                            855, "In the Compressed Data Format The uncompressed size MUST" +
                            " be less than or equal to 8192 bytes for Block Uncompressed Size. ");

                        #endregion

                        #region MS-FRS2_R856

                        FRSSite.CaptureRequirementIfIsTrue(
                            objDataBuffer.signature.BlockCompressedSize <=
                            objDataBuffer.signature.BlockUncompressedSize,
                            856, "In the Compressed Data Format The compressed size MUST be" +
                            " less than or equal to the uncompressed size.");

                        #endregion

                        #region MS-FRS2_R859

                        //Validated in the parser.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            859, "In the Compressed Data Format The uncompressed size MUST" +
                            " be equal to 8192 bytes, except for the last block of a file" +
                            " transfer, which may be smaller, and except when this data is" +
                            " from RdcGetFileData");

                        #endregion

                        Validate_FSCC_BKUP_Requirements(objDataBuffer, ParserResult);
                    }
                }
                else if ((isEndofFile == 0) && (rdcDesired == 0))
                {
                    //If EndofFile is not reached then the dataBuffer
                    //can not be fed to the parser since the FLAT
                    //Stream data would be splitted and entire data
                    //will not present in this dataBuffer.
                    #region MS-FRS2_R823

                    FRSSite.CaptureRequirementIfIsTrue(
                        sizeRead == bufferSize,
                        823,
                        "In the Custom Marshaling Format all other" +
                        " headers MUST have the flags set to 0 besides" +
                        " HEADER_FLAGS_END_OF_STREAM for sending multiple chunks.");

                    #endregion
                }

                #region RequirementValidation

                #region MS-FRS2_R757
                if (ConfigStore.IsWindows)
                {
                    //Verifing frsUpdate fields except the UID are cleared (zeroed out).<30>
                    FRSSite.CaptureRequirementIfIsNotNull(reqUpdate.uidDbGuid,
                                               757,
                                               @"frsUpdate fields except the UID are cleared (zeroed out).<30>");
                }
                #endregion

                #region MS-FRS2_R791
                //Verifying Upon successfully validating the file transfer request, the server MUST retrieve 
                //the file data associated with the requested UID in the supplied update. 
                FRSSite.CaptureRequirementIfIsNotNull(fileInfo, 791, "Upon successfully validating the file transfer request, the server MUST retrieve" +
                                       "the file data associated with the requested UID in the supplied update.  ");
                #endregion

                #region MS-FRS2_R792
                //Verifying that the RDCSignaturelevels are equal to "0" implies raw file transfer 
                if (RDCFileInfo.rdcSignatureLevels == 0)
                {
                    FRSSite.CaptureRequirement(792, "[InitializeFileTransferAsync]" +
                                             "The server MUST then [upon retrieving the file " +
                                             "data associated with the requested UID in the supplied update] " +
                                            "send the file data in the way that the client has specified if " +
                                            "possible (not using RDC).");
                }
                #endregion

                #region MS-FRS2_R793
                //Verifying that the RDCSignaturelevels are equal to "0" implies raw file transfer 
                if (RDCFileInfo.rdcSignatureLevels != 0)
                {
                    FRSSite.CaptureRequirement(793,
                                            "[InitializeFileTransferAsync]" +
                                            "The server MUST then [upon retrieving the file " +
                                            "data associated with the requested UID in the supplied update] " +
                                            "send the file data in the way that the client has specified if " +
                                            "possible (using RDC).");
                }
                #endregion

                #region MS-FRS2_R796
                //Verifying The server MUST provide the file metadata that is associated with 
                //the file that it serves. It does so by providing its own view of 
                //the update associated with the requested UID in the return valueof frsUpdate.
                FRSSite.CaptureRequirementIfIsNotNull(fileInfo, 796, "The server MUST provide the file metadata that is associated with " +
                                        "the file that it serves. It does so by providing its own view of " +
                                        "the update associated with the requested UID in the return value" +
                                        "of frsUpdate.");
                #endregion

                if (!ConfigStore.WillingToCheckReq[798])
                {
                    #region MS-FRS2_R798
                    //Verifying that the RDCSignaturelevels are equal to "0" implies raw file transfer 
                    if (bufferSize == 0)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(((retVal != 0) || ((sizeRead == 0) && (retVal == 0))), 798,
                                                         "[InitializeFileTransferAsync]If bufferSize is " +
                                                         "zero then the server MAY complete the call " +
                                                         "successfully with sizeRead set to zero, or fail " +
                                                         "the call with an implementation-defined failure value.");
                    }
                    #endregion
                }

                #region MS-FRS2_R1279
                //Verifying  The default behavior of a Windows Server 2008 and Windows 7  
                //[if bufferSize is zero] is to complete the request successfully
                //and return sizeRead equal to zero. 
                if (ConfigStore.IsWindows)
                {
                    if (bufferSize == 0)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(retVal == 0,
                                                         1279,
                                                         @" The default behavior of Windows Server 2008 and" +
                                                        "Windows 7  [if bufferSize is zero] is to complete the request" +
                                                        "successfully and return sizeRead equal to zero.");
                    }
                }
                #endregion

                #region MS-FRS2_R799
                if (ConfigStore.IsWindows)
                {
                    //Verifying  The default behavior of a Windows-based server 
                    //[if bufferSize is zero] is to complete the request successfully
                    //and return sizeRead equal to zero. 
                    if (bufferSize == 0)
                    {
                        FRSSite.CaptureRequirementIfIsTrue(retVal == 0,
                                                         799,
                                                         @"WB> The default behavior of a Windows-based server [if bufferSize is zero] is to 
                                                        complete the request successfully and return sizeRead equal to zero. </WB>");
                    }
                }
                #endregion

                #region MS-FRS2_R965
                // Verifying if In RDC File Transfer if a server determines that the file to be served is not 
                //suitable for the RDC protocol, it sets rdcSignatureLevels to 0 in the FRS_RDC_FILEINFO structure.
                if (RDCFileInfo.rdcSignatureLevels == 0)
                {
                    FRSSite.CaptureRequirement(965,
                                            "In RDC File Transfer if a server determines " +
                                            "that the file to be served is not suitable " +
                                            "for the RDC protocol, it sets rdcSignatureLevels " +
                                            "to 0 in the FRS_RDC_FILEINFO structure.");
                }
                #endregion


                if (ConfigStore.Should)
                {
                    #region MS-FRS2_R1113
                    //The server should honour the staging policy set by the client and return success.
                    FRSSite.CaptureRequirement(1113,
                                            "For  stagingPolicy Parameter: The server SHOULD honor the " +
                                            "InitializeFileTransferAsync request.");
                    #endregion

                }
                else
                {
                    #region MS-FRS2_R1277
                    //The Windows Server does honour the staging policy set by the client and return success.
                    FRSSite.CaptureRequirement(1277,
                                            "[For stagingPolicy Parameter]" +
                                            "Windows Server does honor the InitializeFileTransferAsync request.");
                    #endregion
                }


                #region MS-FRS2_R1116
                //Verifying If the server context handle returned by InitializeFileTransferAsync is set to 0, 
                //the entire contents of the downloaded file fit in the buffer provided as part of the output 
                //parameters of InitializeFileTransferAsync. 
                if (servContext == IntPtr.Zero)
                {
                    FRSSite.CaptureRequirementIfIsTrue((sizeRead < bufferSize) && (sizeRead != 0),
                                                     1116,
                                                     "If the server context handle returned by InitializeFileTransferAsync" +
                                                     "is set to 0, the entire contents of the downloaded file fit " +
                                                     "in the buffer provided as part of the output parameters of " +
                                                     "InitializeFileTransferAsync. ");
                }
                #endregion

                #region MS-FRS2_R771
                if (isEndofFile == 1)
                {
                    //Verifying The value of isEndOfFile is TRUE if the end of the specified file has been reached 
                    //and there is no more file data to replicate to the client; otherwise, the value is FALSE.
                    //Verified by comparing the  sizeRead with BufferSize, if the sizeRead is Less than BufferSize, it means
                    //that no more data available to replicate to the client.
                    FRSSite.CaptureRequirementIfIsTrue(sizeRead < bufferSize,
                                                     771,
                                                     @"The value of isEndOfFile is TRUE if the end of the specified file
                                                       has been reached and there is no more file data to replicate to 
                                                       the client; otherwise, the value is FALSE.");

                }
                #endregion

                #region MS-FRS2_R764
                if (!Convert.ToBoolean(ConfigStore.Reqnoblocked764))
                {
                    if (rdcCheck == true)
                    {
                        //Verifying If the client-supplied value of rdcDesired is TRUE and the client-supplied value of stagingPolicy 
                        //is SERVER_DEFAULT, then the server MUST set stagingPolicy to STAGING_REQUIRED. 
                        FRSSite.CaptureRequirementIfIsTrue(stagingPolicyCopy == _FRS_REQUESTED_STAGING_POLICY.SERVER_DEFAULT &&
                                                         stagingPolicy == _FRS_REQUESTED_STAGING_POLICY.STAGING_REQUIRED,
                                                         764,
                                                         @"If the client-supplied value of rdcDesired is TRUE and the client-supplied 
                                                            value of stagingPolicy is SERVER_DEFAULT, then the server MUST set stagingPolicy
                                                             to STAGING_REQUIRED. ");
                    }
                }
                #endregion

                #region MS-FRS2_R772
                if (isEndofFile == 1)
                {
                    //Verifying the value of isEndOfFile is TRUE if the end of the specified file has been reached and 
                    //there is no more file data to replicate to the client.
                    FRSSite.CaptureRequirementIfIsTrue((bufferSize > sizeRead),
                                                     772,
                                                     @"The value of isEndOfFile is TRUE if the end of the specified file has 
                                                      been reached and there is no more file data to replicate to the client.");
                }
                #endregion

                #region MS-FRS2_R773
                if (isEndofFile == 0)
                {
                    //Verifying The value of isEndOfFile is FALSE if the end of the specified file has not been reached
                    //and there is more file data to replicate to the client.
                    FRSSite.CaptureRequirementIfIsTrue((sizeRead == bufferSize) || (sizeRead == 0),
                                                      773,
                                                      @"The value of isEndOfFile is FALSE if the end of the specified file 
                                                        has not been reached and there is more file data to replicate to the client.");
                }
                #endregion
                #region MS-FRS2_R785
                if (frsUpdate[0].present == present_Values.V1)
                {
                    //Verifying If the file on the server has been deleted and if the corresponding file metadata
                    //has been updated with the present flag set to 0 then the server MUST fail the call with an
                    //implementation-defined failure value.
                    FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0,
                                                   785,
                                                   @"If the file on the server has been deleted and if the corresponding file 
                                                metadata has been updated with the present flag set to 0 then the server
                                                MUST fail the call with an implementation-defined failure value.");
                }
                #endregion

                #region MS-FRS2_R226
                if (RDCFileInfo.rdcSignatureLevels == 0)
                {
                    //Verifying whether the value of 0 indicates that a non-RDC file transfer is required. 
                    FRSSite.CaptureRequirement(226,
                                            @"A value of 0 indicates that a non-RDC file transfer is required.");
                }
                #endregion

                if (ConfigStore.Should)
                {
                    #region MS-FRS2_R217

                    //Verifying The server SHOULD make the estimate of onDiskFileSize field in FRS_RDC_FILEINFO 
                    //structure as accurate as possible, but the protocol does not require that it be exact.
                    FRSSite.CaptureRequirementIfIsNotNull(RDCFileInfo.onDiskFileSize,
                                                     217,
                                                     "The server SHOULD make the estimate of onDiskFileSize " +
                                                     "field in FRS_RDC_FILEINFO structure as accurate as " +
                                                     "possible, but the protocol does not require that it be" +
                                                     "exact.");
                    #endregion
                }

                if (ConfigStore.Should)
                {
                    #region MS-FRS_R219
                    //Verifying The server SHOULD make the estimate of fileSizeEstimate field in FRS_RDC_FILEINFO structure 
                    //as accurate as possible, but the protocol does not require that it be exact.
                    FRSSite.CaptureRequirementIfIsNotNull(RDCFileInfo.fileSizeEstimate,
                                                    219,
                                                    "The server SHOULD make the estimate of fileSizeEstimate" +
                                                    "field in FRS_RDC_FILEINFO structure as accurate as possible," +
                                                    "but the protocol does not require that it be exact.");
                    #endregion
                }


                //Verifying The compressionAlgorithm field in FRS_RDC_FILEINFO structure MUST be
                //set to RDC_UNCOMPRESSED.
                bool flag = false;
                if (RDCFileInfo.compressionAlgorithm == _RDC_FILE_COMPRESSION_TYPES.RDC_UNCOMPRESSED)
                {
                    flag = true;

                    #region MS-FRS2_R228

                    FRSSite.CaptureRequirementIfIsTrue(flag,
                                               228,
                                               "The compressionAlgorithm field in FRS_RDC_FILEINFO " +
                                               "structure MUST be set to RDC_UNCOMPRESSED.");

                    #endregion

                    #region MS-FRS2_R74
                    //Verifying When the value of RDC_FILE_COMPRESSION_TYPES enumeration is 0 it implies,
                    //RDC_UNCOMPRESSED means data is not compressed. 
                    FRSSite.CaptureRequirementIfIsTrue(flag,
                                                    74,
                                                    "When the value of RDC_FILE_COMPRESSION_TYPES enumeration is 0" +
                                                    "it implies, RDC_UNCOMPRESSED means data is not compressed. ");
                    #endregion

                    #region MS-FRS2_R75
                    //Verifying RDC_UNCOMPRESSED MUST be sent whenever an RDC_FILE_COMPRESSION_TYPES enum value is required.
                    FRSSite.CaptureRequirementIfIsTrue(flag,
                                                    75,
                                                    "RDC_UNCOMPRESSED MUST be set whenever an RDC_FILE_COMPRESSION_TYPES"
                                                     + "enum value is required.");
                    #endregion
                }

                #region MS-FRS2_R221
                //Verifying The rdcVersion field in FRS_RDC_FILEINFO structure MUST be 1.
                FRSSite.CaptureRequirementIfIsTrue((RDCFileInfo.rdcVersion == 1),
                                                 221,
                                                "The rdcVersion field in FRS_RDC_FILEINFO structure MUST be 1.");
                #endregion

                #region MS-FRS2_R223
                FRSSite.CaptureRequirementIfIsTrue((RDCFileInfo.rdcMinimumCompatibleVersion == 1),
                                                223,
                                                "The rdcMinimumCompatibleVersion field in FRS_RDC_FILEINFO" +
                                                "structure MUST be 1.");
                #endregion

                #endregion
            }
            Marshal.FreeHGlobal(dataBufferPtr);
            switch (retVal)
            {

                case (uint)Error.FRS_ERROR_CONNECTION_INVALID:
                    return error_status_t.FRS_ERROR_CONNECTION_INVALID;

                case (uint)Error.FRS_ERROR_CONTENTSET_NOT_FOUND:
                    return error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND;

                default: return error_status_t.ERROR_SUCCESS;
            }
        }
        #endregion

        #region RawGetFileData

        /// <summary>
        /// This method is used for to transfer successive segments from a file.
        /// </summary>
        /// <returns>error_status_t</returns>

        public error_status_t RawGetFileData()
        {
            IntPtr serverContext;
            IntPtr invalidServerContext = Marshal.AllocHGlobal(1);
            uint retVal = 1;
            uint bufferSize = Convert.ToUInt32(ConfigStore.BufferSize);
            uint sizeRead = 0;
            byte[] dataBuffer = new byte[bufferSize];

            if (flagTradTestCaseCheck)
            {
                serverContext = invalidServerContext;
            }
            else
            {
                serverContext = servContext;
            }

            if (RDCFileInfo.rdcSignatureLevels == 0 && isEndofFile == 0)
            {
                retVal = GenerateMessages.RawGetFileData(ref serverContext,
                                                              dataBuffer,
                                                              bufferSize,
                                                              out sizeRead,
                                                              out isEndofFile);

                printRpcRetCode(retVal);

                if (isEndofFile == 1)
                {
                    RawGetFileDataResponseEvent(true);
                }
                else
                {
                    RawGetFileDataResponseEvent(false);
                }
                Marshal.FreeHGlobal(invalidServerContext);
                if (retVal == 0)
                {
                    if (ConfigStore.WillingToCheckReq[633])
                    {
                        #region MS-FRS2_R633
                        //Verifying If bufferSize is zero then the server MAY complete the call successfully 
                        //with sizeRead set to zero, or fail the call with an implementation-defined failure value. 
                        FRSSite.CaptureRequirementIfIsTrue(bufferSize == 0,
                                                         633,
                                                         @"If bufferSize is zero then the server MAY complete the call 
                                                          successfully with sizeRead set to zero, or fail the call with 
                                                          an implementation-defined failure value. ");
                        #endregion
                    }

                    #region MS-FRS2_R634
                    //Verifying If bufferSize is zero then the server MAY complete 
                    //the call successfully with sizeRead set to zero. 
                    if (bufferSize == 0)
                    {
                        FRSSite.CaptureRequirementIfIsTrue((sizeRead == 0) && (retVal == 0),
                                                          634,
                                                          @" The default behavior of Windows Server 2008 and " +
                                                          "Windows 7  [if bufferSize is zero] is to complete the" +
                                                          "request successfully and return sizeRead equal to zero.");
                    }
                    #endregion

                    if (ConfigStore.IsWindows == true)
                    {
                        #region MS-FRS2_R636
                        if (bufferSize == 0)
                        {
                            //Verifying <WB> The default behavior of a Windows-based server [if bufferSize is zero] 
                            //is to complete the RawGetFileData request successfully and return sizeRead equal to zero. </WB>
                            FRSSite.CaptureRequirementIfIsTrue((sizeRead == 0) && (retVal == 0),
                                                             636,
                                                             @"<WB> The default behavior of a Windows-based server [if bufferSize is zero] 
                                                              is to complete the RawGetFileData request successfully and 
                                                             return sizeRead equal to zero. </WB>");
                        }
                        #endregion
                    }
                }

                if (!flagTradTestCaseCheck)
                {
                    uint retValForRawGetFileDataSecondCall = GenerateMessages.RawGetFileData(ref serverContext,
                                                                dataBuffer,
                                                                bufferSize,
                                                                out sizeRead,
                                                                out isEndofFile);

                    //Verifying if the server has already completed transferring the file associated with the server context,
                    //the server fails the call with an implementation-defined failure value.
                    FRSSite.CaptureRequirementIfIsTrue(retValForRawGetFileDataSecondCall != 0, 630,
                        "[RawGetFileData]If the server has already completed transferring the file associated with the server" +
                        "context, the server MUST fail the call with an implementation-defined failure value.");

                    //Verifying if a client issues multiple simultaneous calls to RawGetFileData taking a server context as 
                    //an input parameter with the same server context, then the server ensures that only the first call is
                    //processed and all other calls are failed with an implementation-defined failure value.
                    FRSSite.CaptureRequirementIfIsTrue(retValForRawGetFileDataSecondCall != 0, 804,
                        "If a client issues multiple simultaneous calls to RawGetFileData taking a server context as an input" +
                        "parameter with the same server context, then the server MUST ensure that only the first call is" +
                        "processed and all other calls are failed with an implementation-defined failure value.");
                }

                if (flagTradTestCaseCheck)
                {
                    //Verifying RawGetFileData method returns a nonzero error code on failure.
                    FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0,
                                                    623,
                                                    "RawGetFileData method MUST return a " +
                                                    "nonzero error code on failure.");

                    //Verifying if he specified server context was not obtained by a previous call to 
                    //InitializeFileTransferAsync, the server fails the call with an implementation-defined failure value.
                    FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 629,
                    "[RawGetFileData] If the specified server context was not obtained by a previous call to" +
                    "InitializeFileTransferAsync, the server MUST fail the call with an" +
                    "implementation-defined failure value.");
                }               
            }

            if (retVal == 0)
            {
                return error_status_t.ERROR_SUCCESS;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region RdcGetSignatures

        /// <summary>
        /// This method is used to obtain RDCsignature data from a server.
        /// </summary>
        /// <param name="offSet">The zero-based offset, in bytes, at which to retrieve data from the file.</param>
        /// <returns>error_status_t</returns>

        public error_status_t RdcGetSignatures(offset offSet)
        {
            byte level;
            IntPtr invalidServContext = Marshal.AllocHGlobal(1);
            ulong offset = Convert.ToUInt64(ConfigStore.Offset);
            uint length = Convert.ToUInt32(ConfigStore.Length);
            byte[] buffer = new byte[length];
            IntPtr bufferPtr = Marshal.AllocHGlobal(10000);
            IntPtr serverContext;

            if (flagTradTestCaseForRdcGetSig)
            {
                serverContext = invalidServContext;
            }
            else
            {
                serverContext = servContext;
            }

            if (flagTradTestCaseForRdcGetSigfailForLevel)
            {
                level = (byte)(RDCFileInfo.rdcSignatureLevels + 1);
            }
            else
            {
                level = RDCFileInfo.rdcSignatureLevels;
            }
            if (serverContext == IntPtr.Zero && isEndofFile == 1 && ConfigStore.IsTestSYSVOL)
                FRSSite.Assert.Fail("Initialize Transfer Async already returns EndOfFile. Should not come here");

            if (serverContext == IntPtr.Zero || serverContext == invalidServContext)
            {
                FRSSite.Assert.Fail("TestCase: input a zero or invalid context handle for RPC method will throw local exception instead of error from remote server");
            }
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "level:" + level);
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "length:" + length);
            //FRSSite.Assert.AreNotEqual<byte>(0, level, "TestCase: level parameter of RdcGetSignatures MUST be from 1 to 8");
            //FRSSite.Assert.AreNotEqual<uint>(0, length, "TestCase: length parameter of RdcGetSignatures MUST be from 1 to 65536");
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "offset:" + offset);
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "bufferPtr:" + bufferPtr);
            uint retVal = GenerateMessages.RdcGetSignatures(serverContext,
                                                    level,
                                                    offset,
                                                   bufferPtr,
                                                    length,
                                                    out sizeRead);

            if (flagTradTestCaseForRdcGetSigfail)
            {
                #region MS-FRS2_R647
                //When the client requests for RDC desired to false the server will go for Raw file transfer. 
                //If the client sends RdcGetSignatures() the server returns a nonzero error code.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0,
                                              647,
                                              @"RdcGetSignatures method MUST return a nonzero error code on failure.");
                #endregion

                #region MS-FRS2_R657
                //When the client requests for RDC desired to false the server will go for Raw file transfer. 
                //If the client sends RdcGetSignatures() the server returns a nonzero error code.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0,
                                              657,
                                              @"[RdcGetSignatures]If the specified server context was retrieved via a 
                                                  call to InitializeFileTransferAsync in which the client set the rdcDesired 
                                                  parameter to FALSE, then  the server MUST fail the call with an implementation-defined 
                                                 failure value. ");
                #endregion

            }

            if (flagTradTestCaseForRdcGetSigfailForLevel)
            {
                #region MS-FRS2_R660
                //When flag flagTradTestCaseForRdcGetSigfailForLevel is set the level in RDCGetSignatire is sent 
                //out of the range [1,x] where x is rdcSignatureLevels.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0,
                                              660,
                                              @"[RdcGetSignatures]If level is not in the range [1, x], where x is the 
                                                 value of the rdcSignatureLevels field of the rdcFileInfo output parameter
                                                 that was returned by the InitializeFileTransferAsync call  associated 
                                                 with the specified server context then the server MUST fail the call with
                                                 an implementation-defined failure value. ");
                #endregion
            }

            if ((level == RDCFileInfo.rdcSignatureLevels) && (!flagTradTestCaseForRdcGetSigfail))
            {
                #region MS-FRS2_R225
                //Verifying the rdcSignatureLevels field in FRS_RDC_FILEINFO structure is the level that the server
                //MUST allow the client to get signatures at least to rdcSignatureLevels depth.
                //Verified by passing signature leves specified by the server in RDC File Info structure,
                //if the call succeess then the requirement is verified.
                FRSSite.CaptureRequirementIfIsTrue(retVal == 0,
                                                225,
                                                @"The rdcSignatureLevels field in FRS_RDC_FILEINFO structure is 
                                                the level that the server MUST allow the client to get signatures 
                                                at least to rdcSignatureLevels depth.");
                #endregion
            }

            if (retVal == 0)
            {
                #region MS-FRS2_R662
                //Verifying whether on successfully executing RdcGetSignatures the buffer returned is not empty
                //as the size read gives the number of bytes of data returned by server.
                FRSSite.CaptureRequirementIfIsTrue(sizeRead > 0,
                                                662,
                                                "[RdcGetSignatures]Upon successfully validating the signature request, the server returns a buffer of RDC signature information for the specified level and the specified file. ");
                #endregion
            }

            if (retVal == 0)
            {
                #region MS-FRS2_R660
                //Verifying If level is not in the range [1, x], where x is the value of the rdcSignatureLevels field of the 
                //rdcFileInfo output parameter that was returned by the InitializeFileTransferAsync call  associated 
                //with the specified server context then the server MUST fail the call with an implementation-defined 
                //failure value.

                if (level > RDCFileInfo.rdcSignatureLevels)
                {
                    FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0,
                                                   660,
                                                   "If level is not in the range [1, x], where x is the value of the rdcSignatureLevels field of the rdcFileInfo " +
                                                   "output parameter that was returned by the InitializeFileTransferAsync call  associated with the specified server " +
                                                   "context then the server MUST fail the call with an implementation-defined failure value.");
                }
                #endregion

                #region MS-FRS2_R663
                //Verifying the server MUST return as many bytes as requested, except when the end of file is reached. 
                //If the end of the file is not reached then the sizeRead is equal to length.
                //if the end of the file is reached then the sizeRead is less than length.
                FRSSite.CaptureRequirementIfIsTrue(sizeRead <= length,
                                                 663,
                                                 @"The server MUST return as many bytes as requested, except when the end of file is reached. ");
                #endregion

                #region MS-FRS2_R664
                //Verifying When the end of file is reached the server MUST return as many bytes as remain
                //in the file from the specified offset, which MAY be 0 bytes. 
                FRSSite.CaptureRequirementIfIsTrue(sizeRead <= length,
                                                 664,
                                                 @"When the end of file is reached the server MUST return as many bytes
                                            as remain in the file from the specified offset, which MAY be 0 bytes. ");
                #endregion
            }

            if (flagTradTestCaseForRdcGetSig)
            {
                //Verifying if the specified server context was not obtained by a previous call to 
                //InitializeFileTransferAsync, the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 655,
                    "[RdcGetSignatures] If the specified server context was not obtained by a previous call to" +
                    "InitializeFileTransferAsync, the server MUST fail the call with an" +
                    "implementation-defined failure value.");

                //Verifying RdcGetSignatures method returns a nonzero error code on failure.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 647,
                    "RdcGetSignatures method MUST return a nonzero error code on failure.");
            }

            if (!flagTradTestCaseForRdcGetSig)
            {
                uint retValForRdcGetSig = GenerateMessages.RdcGetSignatures(serverContext,
                                                                                level,
                                                                                offset,
                                                                                bufferPtr,
                                                                                length,
                                                                                out sizeRead);
                //Verifying if the server has already completed transferring the file associated with the server context,
                //the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRdcGetSig != 0, 656,
                    "[RdcGetSignatures]If the server has already completed transferring the file associated with the server" +
                    "context, the server MUST fail the call with an implementation-defined failure value.");

                //Verifying if a client issues multiple simultaneous calls to RdcGetSignatures taking a server context as 
                //an input parameter with the same server context, then the server ensures that only the first call is
                //processed and all other calls are failed with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRdcGetSig != 0, 805,
                    "If a client issues multiple simultaneous calls to RdcGetFileData taking a server context as an input" +
                    "parameter with the same server context, then the server MUST ensure that only the first call is" +
                    "processed and all other calls are failed with an implementation-defined failure value.");
            }
            

            Marshal.FreeHGlobal(bufferPtr);
            if ((error_status_t)retVal == error_status_t.ERROR_SUCCESS)
            {
                return error_status_t.ERROR_SUCCESS;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region RdcPushSourceNeeds

        /// <summary>
        /// This method is used to register requests for file ranges on a server.
        /// </summary>
        /// <returns>error_status_t</returns>

        public error_status_t RdcPushSourceNeeds()
        {
            _FRS_RDC_SOURCE_NEED[] sourceNeeds = new _FRS_RDC_SOURCE_NEED[2];
            IntPtr invalidServContext = Marshal.AllocHGlobal(1);
            IntPtr serverContext;

            sourceNeeds[0].needOffset = 0;
            sourceNeeds[0].needSize = sizeRead - (sizeRead / 2);
            uint needCount;
            sourceNeeds[1].needOffset = sizeRead / 2;
            sourceNeeds[1].needSize = sizeRead - (sizeRead / 2);

            if (flagTradTestCaseForPushSourceNeeds)
            {
                serverContext = invalidServContext;
            }
            else
            {
                serverContext = servContext;
            }

            if (flagTradTestCaseForPushSourceNeedsForNeedCount)
            {
                needCount = 0;
            }
            else
            {
                needCount = 2;
            }
            if (serverContext == IntPtr.Zero)
            {
                FRSSite.Assert.Fail("TestCase: input a zero context handle for RPC method will throw local exception instead of error from remote server");
            }
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "serverContext:" + serverContext);
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "sourceNeeds:" + sourceNeeds);
            FRSSite.Log.Add(LogEntryKind.Checkpoint, "needCount:" + needCount);
            uint retVal = GenerateMessages.RdcPushSourceNeeds(serverContext, sourceNeeds, needCount);

            if (retVal == 0 && (!flagTradTestCaseForPushSourceNeedsForNeedCount))
            {
                flagRdcSourceNeeds = true;

                #region MS-FRS2_R684
                //Verifying The number of RDC source needs queued by the server MUST NOT exceed CONFIG_RDC_NEED_QUEUE_SIZE. 
                FRSSite.CaptureRequirementIfIsTrue(sourceNeeds.Length <= 20,
                                                 684,
                                                 "The number of RDC source needs queued by the server MUST NOT exceed CONFIG_RDC_NEED_QUEUE_SIZE. ");
                #endregion

                #region MS-FRS2_R665
                //On requesting random signatures server is allowing the client to read randomly from all 
                //available signature streams.
                FRSSite.CaptureRequirementIfAreEqual<uint>(retVal, 0,
                                                665,
                                                "[RdcGetSignatures]The server MUST allow the client to read randomly from all available signature streams.");
                #endregion
            }

            if (ConfigStore.WillingToCheckReq[691])
            {
                FRSSite.CaptureRequirement(691, "if need count is zero then the server may complete the call successfully  ,or fail the call  with  an implementation defined failure value.");
            }

            if (flagTradTestCaseForPushSourceNeeds)
            {
                //Verifying if the specified server context was not obtained by a previous call to 
                //InitializeFileTransferAsync, the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 678,
                    "[RdcPushSourceNeeds] If the specified server context was not obtained by a previous call to" +
                    "InitializeFileTransferAsync, the server MUST fail the call with an" +
                    "implementation-defined failure value.");

                //Verifying if the server has already completed transferring the file associated with the server context,
                //the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 679,
                    "[RdcPushSourceNeeds]If the server has already completed transferring the file associated with the server" +
                    "context, the server MUST fail the call with an implementation-defined failure value.");
            }

            if (!flagTradTestCaseForPushSourceNeeds)
            {
                uint retValForPushSourceNeedsSecondCall = GenerateMessages.RdcPushSourceNeeds(serverContext, sourceNeeds, needCount);

                //Verifying if a client issues multiple simultaneous calls to RdcPushSourceNeeds taking a server context as 
                //an input parameter with the same server context, then the server ensures that only the first call is
                //processed and all other calls are failed with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForPushSourceNeedsSecondCall != 0, 806,
                    "If a client issues multiple simultaneous calls to RdcGetFileData taking a server context as an input" +
                    "parameter with the same server context, then the server MUST ensure that only the first call is" +
                    "processed and all other calls are failed with an implementation-defined failure value.");
            }

            if (flagTradTestCaseForPushSourceNeedsForNeedCount)
            {
                if (ConfigStore.IsWindows)
                {
                    #region MS-FRS2_R1278
                    //In Windows if needcount is set to zero, PushSourceneed should return zero.
                    FRSSite.CaptureRequirementIfAreEqual<uint>(retVal, 0,
                                                    1278,
                                                    @"The default behavior of Windows 2008 and Windows 7 
                                                    [if needCount is zero ] is to complete the RdcPushSourceNeeds
                                                    request successfully.");
                    #endregion
                }
            }
            
            Marshal.FreeHGlobal(invalidServContext);
            if (retVal == 0)
            {
                return (error_status_t)retVal;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region RdcGetFileData

        /// <summary>
        /// This method is used to obtain file ranges whose requests have previously been registered on a server.
        /// </summary>
        /// <returns>error_status_t</returns>

        public error_status_t RdcGetFileData(BufferSize Buffersize)
        {
            IntPtr serverContext;
            uint sizeRead = 0;
            IntPtr invalidServerContext = Marshal.AllocHGlobal(1);

            uint bufferSize = 0;
            if (Buffersize == BufferSize.validBufSize)
            {
                bufferSize = 9326 + sizeRead / 2;
            }
            else
            {
                bufferSize = 0;
            }

            if (flagTradTestCaseCheck)
            {
                serverContext = invalidServerContext;
            }
            else
            {
                serverContext = servContext;
            }

            byte[] dataBuffer = new byte[bufferSize];
            IntPtr bufferPtr = Marshal.AllocHGlobal((int)bufferSize);
            uint retVal = GenerateMessages.RdcGetFileData(serverContext,
                                                          bufferPtr,
                                                          bufferSize,
                                                          out sizeRead);
            printRpcRetCode(retVal);
            if (sizeRead == 0)
            {

                RdcGetFileDataEvent(SizeReturned.zeroValue);
            }
            else
            {
                RdcGetFileDataEvent(SizeReturned.nonZeroValue);
            }

            #region MS-FRS2_R714
            //On successfull call to RDCGetFileData verifying that Buffer returned and the Size returned 
            //by the server are not empty which implies server returned the file data requested.
            if (retVal == 0 && (flagRdcSourceNeeds == true))
            {
                FRSSite.CaptureRequirementIfIsTrue((bufferPtr != null) && (sizeRead != 0), 714,
                                                 "Upon successfully validating the RDC file data request,"
                                                   + " the server serves file data"
                                                 + "from the source needs that were queued by RdcPushSourceNeeds.");
            }
            #endregion

            #region MS-FRS2_R712
            //If bufferSize is less than XPRESS_RDC_MIN_GET_DATA_BUFFER_SIZE_WITH_FILE_HEADER ((0x4)+(0x2410))
            //then the server MUST fail the call with an implementation-defined failure value. 
            if (bufferSize < XPRESS_RDC_MIN_GET_DATA_BUFFER_SIZE_WITH_FILE_HEADER)
            {
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0,
                                              712,
                                              "If bufferSize is less than XPRESS_RDC_MIN_GET_DATA_BUFFER_SIZE_WITH_FILE_HEADER ((0x4)+(0x2410))" +
                                              "then the server MUST fail the call with an implementation-defined failure value. ");
            }
            #endregion

            #region MS-FRS2_R717
            //Verifying If the client has not called RdcPushSourceNee
            // ds before calling RdcFileGetData then the server
            // MUST complete the call successfully and set sizeReturned to zero. 
            if ((flagRdcSourceNeeds == false) && (retVal == 0))
            {
                FRSSite.CaptureRequirementIfIsTrue((sizeRead == 0),
                                                717,
                                                @"If the client has not called RdcPushSourceNee
                                                    ds before calling RdcFileGetData then the server
                                                    MUST complete the call successfully and set sizeReturned to zero. ");

            }

            #endregion

            if ((flagRdcSourceNeeds == true) && (Buffersize == BufferSize.validBufSize))
            {
                //FileHeader of the Buffer returned
                long ptrPos = (long)bufferPtr;

                Int32 fileHeader = Marshal.ReadInt32((IntPtr)ptrPos);
                ptrPos = ptrPos + sizeof(Int32);

                Int32 fragmentHeader = Marshal.ReadInt32((IntPtr)ptrPos);
                int numberOfFragments = (int)fragmentHeader;
                ptrPos = ptrPos + sizeof(Int32);

                #region MS-FRS2_R1262
                //verifying that Fragment Header value MUST be no greater than 128 .[XPRESS_RDC_MAX_NB_NEEDS_FOR_COMPRESSION]
                FRSSite.CaptureRequirementIfIsTrue(fragmentHeader <= XPRESS_RDC_MAX_NB_NEEDS_FOR_COMPRESSION, 1262,
                                                    "In the RdcGetFileData (Opnum 11) Fragment Header value MUST be no "
                                                   + "greater than 128 .[XPRESS_RDC_MAX_NB_NEEDS_FOR_COMPRESSION]");
                #endregion

                #region collectingFragments
                Int32[] blockOffset = new Int32[numberOfFragments];
                Int32[] fragmentSize = new Int32[numberOfFragments];
                byte[] data = new byte[fragmentSize.Length];

                bool flagMSFRS2R1263 = false;
                bool flagMSFRS2R1264 = false;
                bool flagMSFRS2R1265 = false;
                bool flagMSFRS2R1266 = false;

                for (int i = 0; i < numberOfFragments; i++)
                {
                    blockOffset[i] = Marshal.ReadInt32((IntPtr)ptrPos);
                    ptrPos = ptrPos + sizeof(Int32);

                    //verifying the valid range of BlockOffset field is 0 to X_CONFIG_XPRESS_BLOCK_SIZE-1 
                    //and setting the flagMSFRS2R726 = true.
                    if ((0 <= (blockOffset[i])) && ((blockOffset[i]) <= (X_CONFIG_XPRESS_BLOCK_SIZE - 1)))
                    {
                        flagMSFRS2R1263 = true;
                    }

                    fragmentSize[i] = Marshal.ReadInt32((IntPtr)ptrPos);
                    ptrPos = ptrPos + sizeof(Int32);

                    //Verifying that the uncompressed bytes start from BlockOffset and 
                    //include at most, X_CONFIG_XPRESS_BLOCK_SIZE-blockOffset-1 bytes where fragmentSize
                    // gives the size of uncompressed data size.
                    if (fragmentSize[i] <= X_CONFIG_XPRESS_BLOCK_SIZE - blockOffset[i] - 1)
                    {
                        flagMSFRS2R1264 = true;
                    }

                    //Verifying whether the valid range of FragmentSize field is 1 to X_CONFIG_XPRESS_BLOCK_SIZE.
                    if ((1 <= fragmentSize[i]) && (fragmentSize[i] <= X_CONFIG_XPRESS_BLOCK_SIZE - blockOffset[i] - 1))
                    {
                        flagMSFRS2R1265 = true;
                    }

                    //Verifying whether the sum of BlockOffset and FragmentSize 
                    //MUST be less or equal to X_CONFIG_XPRESS_BLOCK_SIZE.
                    if ((fragmentSize[i] + blockOffset[i]) <= X_CONFIG_XPRESS_BLOCK_SIZE)
                    {
                        flagMSFRS2R1266 = true;
                    }

                    //size of the data will be fragmentsize - sizeofblockoffset - sizeoffragmentsize.
                    for (int j = 0; j < ((fragmentSize[i]) - ((sizeof(Int32)) - (sizeof(Int32)))); j++)
                    {
                        data[i] = Marshal.ReadByte((IntPtr)ptrPos);
                        ptrPos = ptrPos + sizeof(byte);
                    }

                }

                #endregion

                //If the number of fragements is zero there will be no fragment block.
                //hence checking for numberoffragements greater than zero.
                if (numberOfFragments > 0)
                {
                    #region MS-FRS2_R1262
                    //verified the valid range of BlockOffset field is 0 to X_CONFIG_XPRESS_BLOCK_SIZE-1 
                    //and setting the flagMSFRS2R726 = true.
                    FRSSite.CaptureRequirementIfIsTrue(flagMSFRS2R1263, 1263,
                                                    "In the RdcGetFileData (Opnum 11)The valid range of BlockOffset"
                                                  + " field is 0 to X_CONFIG_XPRESS_BLOCK_SIZE-1");
                    #endregion

                    #region MS-FRS2_R1263
                    //verified the valid range of BlockOffset field is 0 to X_CONFIG_XPRESS_BLOCK_SIZE-1 
                    //and setting the flagMSFRS2R726 = true.
                    FRSSite.CaptureRequirementIfIsTrue(flagMSFRS2R1263, 1263,
                                                    "In the RdcGetFileData (Opnum 11)The valid range of BlockOffset"
                                                  + " field is 0 to X_CONFIG_XPRESS_BLOCK_SIZE-1");
                    #endregion

                    #region MS-FRS2_R1264
                    //Verified that the uncompressed bytes start from BlockOffset and 
                    //include at most, X_CONFIG_XPRESS_BLOCK_SIZE-blockOffset-1 bytes where fragmentSize
                    // gives the size of uncompressed data size.
                    FRSSite.CaptureRequirementIfIsTrue(flagMSFRS2R1264, 1264,
                                                    "In the RdcGetFileData (Opnum 11)The uncompressed bytes start from"
                                                  + " BlockOffset and include at most, X_CONFIG_XPRESS_BLOCK_SIZE-blockOffset-1 bytes.");
                    #endregion

                    #region MS-FRS2_R1265
                    //Verified whether the valid range of FragmentSize field is 1 to X_CONFIG_XPRESS_BLOCK_SIZE.
                    FRSSite.CaptureRequirementIfIsTrue(flagMSFRS2R1265, 1265,
                                                    "In the RdcGetFileData (Opnum 11)The valid range of FragmentSize field"
                                                  + "is 1 to X_CONFIG_XPRESS_BLOCK_SIZE.");
                    #endregion

                    #region MS-FRS2_R1266
                    //Verified whether the sum of BlockOffset and FragmentSize 
                    //MUST be less or equal to X_CONFIG_XPRESS_BLOCK_SIZE.
                    FRSSite.CaptureRequirementIfIsTrue(flagMSFRS2R1266, 1266,
                                                    "In the RdcGetFileData (Opnum 11)for each fragment, the sum of"
                                                   + " BlockOffset and FragmentSize MUST be less or equal to "
                                                   + "X_CONFIG_XPRESS_BLOCK_SIZE.");
                    #endregion
                }
            }

            if (!flagTradTestCaseCheck)
            {
                // sC = servContext;
                uint retValForRdcGetFileDataSecondCall = GenerateMessages.RdcGetFileData(serverContext,
                                                            bufferPtr,
                                                            bufferSize,
                                                            out sizeRead);

                //Verifying if the server has already completed transferring the file associated with the server context,
                //the server fails the call with an implementation-defined failure value.
                //Verifying if the server has already completed transferring the file associated with the server context,
                //the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRdcGetFileDataSecondCall != 0, 708,
                    "[RdcGetFileData]If the server has already completed transferring the file associated with the server" +
                    "context, the server MUST fail the call with an implementation-defined failure value.");

                //Verifying if a client issues multiple simultaneous calls to RdcGetFileData taking a server context as 
                //an input parameter with the same server context, then the server ensures that only the first call is
                //processed and all other calls are failed with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRdcGetFileDataSecondCall != 0, 807,
                    "If a client issues multiple simultaneous calls to RdcGetFileData taking a server context as an input" +
                    "parameter with the same server context, then the server MUST ensure that only the first call is" +
                    "processed and all other calls are failed with an implementation-defined failure value.");
            }

            if (flagTradTestCaseCheck)
            {
                //Verifying if the specified server context was not obtained by a previous call to 
                //InitializeFileTransferAsync, the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 707,
                    "[RdcGetFileData] If the specified server context was not obtained by a previous call to" +
                    "InitializeFileTransferAsync, the server MUST fail the call with an" +
                    "implementation-defined failure value.");

            }
            
            Marshal.FreeHGlobal(invalidServerContext);
            if (retVal == 0)
            {
                return (error_status_t)retVal;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion

        #region RdcClose
        /// <summary>
        /// This method informs the server that the server context information can be released.
        /// </summary>
        /// <returns>error_status_t</returns>

        public error_status_t RdcClose()
        {
            IntPtr invalidServContext = Marshal.AllocHGlobal(1);
            IntPtr serverContext;

            if (flagTradTestCaseForRdcClose)
            {
                serverContext = IntPtr.Zero;//invalidServContext;
            }
            else
            {
                serverContext = servContext;
            }
            if (serverContext == IntPtr.Zero)
            {
                FRSSite.Assert.Fail("TestCase: input a zero context handle for RPC method will throw local exception instead of error from remote server");
            }
            uint retVal = GenerateMessages.RdcClose(ref serverContext);
            Marshal.FreeHGlobal(invalidServContext);

            if (flagTradTestCaseForRdcClose)
            {
                //Verifying RdcClose method returns a nonzero error code on failure.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 738, "RdcClose method MUST return a nonzero error code on failure.");

                //Verifying if the specified server context was not obtained by a previous call to 
                //InitializeFileTransferAsync, the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 742,
                    "[RdcClose]If the specified server context was not obtained by a previous call to" +
                    "InitializeFileTransferAsync, the server MUST fail the call with an" +
                    "implementation-defined failure value.");
            }

            if (!flagTradTestCaseForRdcClose)
            {
                uint retValForRdcClose = GenerateMessages.RdcClose(ref serverContext);

                ////Verifying if the server has already closed the server context, the server fails the call with an 
                ////implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRdcClose != 0, 743, "[RdcClose]If the server has already closed the server context, the" +
                                                "server MUST fail the call with an implementation-defined failure value.");

            }
            
            return error_status_t.ERROR_SUCCESS;
        }
        #endregion

        #region RawGetFileDataAsync
        /// <summary>
        /// The RawGetFileDataAsync method is used instead of calling RawGetFileData multiple times to obtain file data.
        /// </summary>
        /// <returns>error_status_t</returns>

        public error_status_t RawGetFileDataAsync()
        {

            IntPtr bufferPtr = Marshal.AllocHGlobal(50000);
            IntPtr serverContext = IntPtr.Zero;
            IntPtr sC = IntPtr.Zero;

            if (flagTradTestCaseCheck)
            {
                serverContext = IntPtr.Zero;
            }
            else
            {
                serverContext = servContext;
            }
            uint retVal = GenerateMessages.FRS2RawGetFileDataAsync(serverContext, out bufferPtr);
            printRpcRetCode(retVal);

            if (!flagTradTestCaseCheck)
            {

                uint retValForRawGetFileDataAsyncSecondCall = GenerateMessages.FRS2RawGetFileDataAsync(serverContext, out bufferPtr);

                //Verifying if the server has already completed transferring the file associated with the server context,
                //the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRawGetFileDataAsyncSecondCall != 0, 873,
                    "[RawGetFileDataAsync]If the server has already completed transferring the file associated with the server" +
                    "context, the server MUST fail the call with an implementation-defined failure value.");

                //Verifying if a client issues multiple simultaneous calls to RawGetFileDataAsync taking a server context as 
                //an input parameter with the same server context, then the server ensures that only the first call is
                //processed and all other calls are failed with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRawGetFileDataAsyncSecondCall != 0, 808,
                    "If a client issues multiple simultaneous calls to RawGetFileDataAsync taking a server context as an input" +
                    "parameter with the same server context, then the server MUST ensure that only the first call is" +
                    "processed and all other calls are failed with an implementation-defined failure value.");
            }

            if (flagTradTestCaseCheck)
            {
                //Verifying RawGetFileDataAsync method returns a nonzero error code on failure.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 866,
                                                                    "RawGetFileDataAsync method MUST return a " +
                                                                    "nonzero error code on failure.");

                //Verifying if the specified server context was not obtained by a previous call to 
                //InitializeFileTransferAsync, the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 872,
                "[RawGetFileDataAsync] If the specified server context was not obtained by a previous call to" +
                "InitializeFileTransferAsync, the server MUST fail the call with an" +
                "implementation-defined failure value.");
            }
            
            Marshal.FreeHGlobal(bufferPtr);

            if (retVal == 0)
                return error_status_t.ERROR_SUCCESS;
            else return error_status_t.ERROR_FAIL;
        }
        #endregion

        #region RdcGetFileDataAsync
        /// <summary>
        /// The RdcGetFileDataAsync method is used instead of calling RawGetFileData multiple times to obtain file data.
        /// </summary>
        /// <returns>error_status_t</returns>
        public error_status_t RdcGetFileDataAsync()
        {
            IntPtr pipe = IntPtr.Zero;
            IntPtr serverContext = IntPtr.Zero;

            if (flagTradTestCaseCheck)
            {
                serverContext = IntPtr.Zero;
            }
            else
            {
                serverContext = servContext;
            }

            uint retVal = GenerateMessages.FRS2RdcGetFileDataAsync(serverContext, out pipe);

            if (!flagTradTestCaseCheck)
            {
                uint retValForRdcGetFileDataAsyncSecondCall = GenerateMessages.FRS2RdcGetFileDataAsync(serverContext, out pipe);

                //Verifying if the server has already completed transferring the file associated with the server context,
                //the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRdcGetFileDataAsyncSecondCall != 0, 892,
                    "[RdcGetFileDataAsync]If the server has already completed transferring the file associated with the server" +
                    "context, the server MUST fail the call with an implementation-defined failure value.");

                //Verifying if a client issues multiple simultaneous calls to RdcGetFileDataAsync taking a server context as 
                //an input parameter with the same server context, then the server ensures that only the first call is
                //processed and all other calls are failed with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfIsTrue(retValForRdcGetFileDataAsyncSecondCall != 0, 809,
                "If a client issues multiple simultaneous calls to RdcGetFileDataAsync taking a server context as an input" +
                "parameter with the same server context, then the server MUST ensure that only the first call is" +
                "processed and all other calls are failed with an implementation-defined failure value.");
            }

            if (flagTradTestCaseCheck)
            {
                //Verifying RdcGetFileDataAsync method returns a nonzero error code on failure.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 883,
                                        "RdcGetFileDataAsync method MUST return a nonzero error code on failure.");

                //Verifying if the specified server context was not obtained by a previous call to 
                //InitializeFileTransferAsync, the server fails the call with an implementation-defined failure value.
                FRSSite.CaptureRequirementIfAreNotEqual<uint>(retVal, 0, 891,
                "[RdcGetFileDataAsync] If the specified server context was not obtained by a previous call to" +
                "InitializeFileTransferAsync, the server MUST fail the call with an" +
                "implementation-defined failure value.");
            }
            
            if ((error_status_t)retVal == error_status_t.ERROR_SUCCESS)
            {
                return error_status_t.ERROR_SUCCESS;
            }
            else
            {
                return error_status_t.ERROR_FAIL;
            }
        }
        #endregion



        /// <summary>
        /// BkupFsccValidation() is used to validate the BKUP and FSCC requirements related to the FRS2 protocol.
        /// </summary>
        public void BkupFsccValidation()
        {
            RequestUpdates(5, 6, versionVectorDiff.valid);

            for (int count = 0; count < frsUpdate.Length; count++)
            {
                ASCIIEncoding enc = new ASCIIEncoding();
                Helper objHelper = new Helper();

                int fileNameLength = 0;
                for (int i = 0; frsUpdate[count].name[i] != 0; i++)
                {
                    fileNameLength++;
                }
                byte[] name = new byte[fileNameLength];

                for (int i = 0; frsUpdate[count].name[i] != 0; i++)
                {
                    name[i] = (byte)frsUpdate[count].name[i];
                }

                ReplicationFileName = enc.GetString(name);
                countOfAsyncCall = count;
                InitializeFileTransferAsync(5, 6, false);
            }
        }

        /// <summary>
        /// Initilizes the flag value for Traditional Test Case
        /// The model events will not be triggered in traditional test case.
        /// </summary>
        public void SetTraditionalTestFlag()
        {
            flagTradTestCaseCheck = true;
        }

        /// <summary>
        ///Initilizes the flag "flagTradTestCaseForRdcGetSig" value for Traditional Test Case
        /// The model events will not be triggered in traditional test case. 
        /// </summary>
        public void SetRdcGetSigTestFlag()
        {
            flagTradTestCaseForRdcGetSig = true;
        }

        /// <summary>
        /// Initilizes the flag "flagTradTestCaseForPushSourceNeeds" value for Traditional Test Case
        /// The model events will not be triggered in traditional test case. 
        /// </summary>
        public void SetPushSourceNeedsTestFlag()
        {
            flagTradTestCaseForPushSourceNeeds = true;
        }

        /// <summary>
        /// Initializes the flag "flagTradTestCaseForPushSourceNeedsForNeedCount" value for Traditional Test Case
        /// The model events will not be triggered in traditional test case and need count is set to zero. 
        /// </summary>
        public void SetPushSourceNeedsTestFlagForNeedCount()
        {
            flagTradTestCaseForPushSourceNeedsForNeedCount = true;
        }
        /// <summary>
        /// Initilizes the flag "flagTradTestCaseForRdcClose" value for Traditional Test Case
        /// The model events will not be triggered in traditional test case. 
        /// </summary>
        public void SetRdcCloseTestFlag()
        {
            flagTradTestCaseForRdcClose = true;
        }

        /// <summary>
        /// Initializes the flag "flagTradTestCaseForRdcGetSigfailForLevel" value for Traditional Test Case
        /// The model events will not be triggered in traditional test case. 
        /// </summary>
        public void SetRdcGetSigFailTestFlag()
        {
            flagTradTestCaseForRdcGetSigfail = true;
        }

        /// <summary>
        /// Initializes the flag "flagTradTestCaseForRdcGetSigfail" value for Traditional Test Case
        /// The model events will not be triggered in traditional test case. 
        /// </summary>
        public void SetRdcGetSigLevelTestFlag()
        {
            flagTradTestCaseForRdcGetSigfailForLevel = true;
        }

        /// <summary>
        /// This method is used to validate 
        /// the FSCC and BKUP Requirements.
        /// </summary>
        /// <param name="objDataBuffer">
        /// This is the filled output replicated
        /// file structure returned by the parser.
        /// </param>
        /// <param name="ParserResult">
        /// This is the bool value equal to the
        /// status of the parser after parsing
        /// the dataBuffer returned by the
        /// InitializeFileTranserAsync Opnum.
        /// </param>
        public void Validate_FSCC_BKUP_Requirements(ReplicatedFileStructure objDataBuffer,
                                                     bool ParserResult)
        {
            ITestSite FRSSite = TestClassBase.BaseTestSite;

            //Changing DocShortName to log the FSCC Requirements
            FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

            Helper objHelper = new Helper();
            FSCCAdapter objAdapter = new FSCCAdapter();

            int countOfStreams = objDataBuffer.streamdata.Count;

            foreach (int i in objDataBuffer.streamdata.Keys)
            {
                #region MS-FRS2_R820

                //Changing DocShortName to log the FRS2 Requirements
                FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                //Verified by the parser.
                //In ReadData method of FileStreamDataParser, the parser makes
                //the boolean return value to false if the value of stream
                //type is not a valid value.
                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                    Enum.IsDefined(typeof(FileStreamDataParser.StreamType_Values),
                    objDataBuffer.streamdata[i].Header.streamType),
                    820, "In the Custom Marshaling Format for" +
                    " Flags all other bits MUST be set to 0.");

                //Reverting DocShortName back to FSCC
                FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                #endregion

                switch (objDataBuffer.streamdata[i].Header.streamType)
                {
                    case (UInt32)FileStreamDataParser.StreamType_Values.MS_TYPE_META_DATA:

                        #region MS-FRS2_R811

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        //The method ValidateMetaData of FileStreamDataParser validates
                        //the data buffer for valid meta data stream.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            811, "In the Custom Marshaling Format streamType:" +
                            " An enumeration, which MUST be MS_TYPE_META_DATA" +
                            " Meaning 0x00000001.");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS-FRS2_R827

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        //The method ValidateMetaData of FileStreamDataParser validates
                        //the data buffer for valid meta data stream.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                            objDataBuffer.streamdata[i].MetaData.Version == 3,
                            827, "In the Custom Marshaling Format version is" +
                            " the marshaler version. It MUST be 3 when" +
                            " stream type is MS_TYPE_META_DATA");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS-FRS2_R831

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        //The method ValidateMetaData of FileStreamDataParser validates
                        //the data buffer for valid meta data stream.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                            objDataBuffer.streamdata[i].MetaData.Reserved1[0] == 0 &&
                            objDataBuffer.streamdata[i].MetaData.Reserved1[1] == 0,
                            831, "In the Custom Marshaling Format Reserved MUST be 0" +
                            "when stream type is MS_TYPE_META_DATA");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS-FRS2_R836

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        //The method ValidateMetaData of FileStreamDataParser validates
                        //the data buffer for valid meta data stream.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                            objDataBuffer.streamdata[i].MetaData.Reserved2[0] == 0 &&
                            objDataBuffer.streamdata[i].MetaData.Reserved2[1] == 0,
                            836, " In the Custom Marshaling Format Reserved2 MUST" +
                            " be 0  when stream type is MS_TYPE_META_DATA");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS_TYPE_META_DATA

                        UInt64 FileCreationTime = objHelper.Get__FILETIMEValue(
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.CreationTime.dwLowDateTime,
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.CreationTime.dwHighDateTime);

                        DateTime CreationTime = new DateTime((Int64)FileCreationTime);

                        if (ReplicationFileName == ConfigStore.FileName + ".txt" &&
                            ConfigStore.Win2k8)
                        {
                            fileCheck fileChk = fileCheck.FileExists;

                            DateTime CreationTime_Actual = objHelper.GetDateTime(
                                ConfigStore.ReplicationDirectoryName,
                                ConfigStore.FileName,
                              ConfigStore.SutIp,
                                ConfigStore.DomainNetbiosName,
                                ConfigStore.AdminName,
                              ConfigStore.AdminPassword,
                                fileDateTimeType.CreationDate,
                                ref fileChk);

                            //The time returned by the server is in GMT,
                            //while the one queried using WMI query represents
                            //the time in EST, there's an offset of 7 hours which
                            //needs to be subtracted from the queried time. Also,
                            //the year according to FILETIME structure is number of
                            //years since 1 Jan, 1601 while the queried time shows
                            //the actual year, hance the offset 1600 is to be added
                            //to years before comparing them.

                            #region Validating Requirement MS-FSCC_R10162

                            if (fileCheck.FileExists == fileChk)
                            {
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                    (CreationTime.Year + 1600) == CreationTime_Actual.Year &&
                                    CreationTime.Month == CreationTime_Actual.Month &&
                                    CreationTime.Day == CreationTime_Actual.Day &&
                                    (CreationTime.Hour - 7) == CreationTime_Actual.Hour &&
                                    CreationTime.Minute == CreationTime_Actual.Minute &&
                                    CreationTime.Second == CreationTime_Actual.Second, 10162,
                                    "[In FileBasicInformation] CreationTime (8 bytes):" +
                                    " All dates and times in this message are in" +
                                    " absolute system-time format, which is represented" +
                                    " as a FILETIME  structure.");
                            }

                            #endregion
                        }


                        #region Validating Requirement MS-FSCC_R10003

                        //Implicitly verified in the Parser.
                        //The parser reads two UINT32 values as prescribed
                        //by the FILETIME structure in the TD. 
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10003,
                                "The data length of dwLowDateTime in the" +
                                " FILETIME structure is 32 bits.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10005

                        //Implicitly verified in the Parser.
                        //The parser reads two UINT32 values as prescribed
                        //by the FILETIME structure in the TD. 
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10005,
                                "The data length of dwHighDateTime in the" +
                                " FILETIME structure is 32 bits.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10163

                        //Verified in the Parser.
                        //The FILETIME structure consists of two UINT32 values which forms
                        //the high and low parts of the absolute system-time, which
                        //represents the number of ticks since Jan 1, 1601.
                        FRSSite.CaptureRequirementIfIsTrue(FileCreationTime > 0, 10163,
                            "A valid time for this [CreationTime] field" +
                            " [of FileBasicInformation] is an integer greater than 0.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10160

                        //Implicitly verified in the Parser.
                        //The parser reads two UINT32 values as prescribed
                        //by the FILETIME structure in the TD. 
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10160,
                                "The length of CreationTime field of" +
                                " FileBasicInformation is 64 bits. ");

                        #endregion

                        UInt64 FileLastAccessTime = objHelper.Get__FILETIMEValue(
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.LastAccessTime.dwLowDateTime,
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.LastAccessTime.dwHighDateTime);

                        DateTime LastAccessTime = new DateTime((Int64)FileLastAccessTime);

                        if (ReplicationFileName == ConfigStore.FileName + ".txt" &&
                            ConfigStore.Win2k8)
                        {
                            fileCheck fileChk = fileCheck.FileExists;

                            DateTime LastAccessTime_Actual = objHelper.GetDateTime(
                                ConfigStore.ReplicationDirectoryName,
                                ConfigStore.FileName,
                              ConfigStore.SutIp,
                                ConfigStore.DomainNetbiosName,
                                ConfigStore.AdminName,
                              ConfigStore.AdminPassword,
                                fileDateTimeType.LastAccessed,
                                ref fileChk);

                            //The time returned by the server is in GMT,
                            //while the one queried using WMI query represents
                            //the time in EST, there's an offset of 7 hours which
                            //needs to be subtracted from the queried time. Also,
                            //the year according to FILETIME structure is number of
                            //years since 1 Jan, 1601 while the queried time shows
                            //the actual year, hance the offset 1600 is to be added
                            //to years before comparing them.

                            #region Validating Requirement MS-FSCC_R10169

                            if (fileCheck.FileExists == fileChk)
                            {
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                    (LastAccessTime.Year + 1600) == LastAccessTime_Actual.Year &&
                                    LastAccessTime.Month == LastAccessTime_Actual.Month &&
                                    LastAccessTime.Day == LastAccessTime_Actual.Day &&
                                    (LastAccessTime.Hour - 7) == LastAccessTime_Actual.Hour &&
                                    LastAccessTime.Minute == LastAccessTime_Actual.Minute &&
                                    LastAccessTime.Second == LastAccessTime_Actual.Second, 10169,
                                    "[In FileBasicInformation] LastAccessTime (8 bytes):" +
                                    " It contains the last time the file was accessed" +
                                    " in the format of a FILETIME structure.");
                            }

                            #endregion
                        }

                        #region Validating Requirement MS-FSCC_R10170

                        //Verified in the Parser.
                        //The FILETIME structure consists of two UINT32 values which forms
                        //the high and low parts of the absolute system-time, which
                        //represents the number of ticks since Jan 1, 1601.
                        FRSSite.CaptureRequirementIfIsTrue(FileLastAccessTime > 0, 10170,
                            "[In FileBasicInformation] LastAccessTime (8 bytes):" +
                            " A valid time for this field is an integer greater than 0.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10168

                        //Implicitly verified in the Parser.
                        //The parser reads two UINT32 values as prescribed
                        //by the FILETIME structure in the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10168,
                                "The length of LastAccessTime field of" +
                                " FileBasicInformation is 64 bits. ");

                        #endregion

                        UInt64 FileLastWriteTime = objHelper.Get__FILETIMEValue(
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.LastWriteTime.dwLowDateTime,
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.LastWriteTime.dwHighDateTime);

                        DateTime LastWriteTime = new DateTime((Int64)FileLastWriteTime);

                        if (ReplicationFileName == ConfigStore.FileName + ".txt" &&
                            ConfigStore.Win2k8)
                        {
                            fileCheck fileChk = fileCheck.FileExists;

                            DateTime LastWriteTime_Actual = objHelper.GetDateTime(
                                ConfigStore.ReplicationDirectoryName,
                                ConfigStore.FileName,
                              ConfigStore.SutIp,
                                ConfigStore.DomainNetbiosName,
                                ConfigStore.AdminName,
                              ConfigStore.AdminPassword,
                                fileDateTimeType.LastModified,
                                ref fileChk);

                            //The time returned by the server is in GMT,
                            //while the one queried using WMI query represents
                            //the time in EST, there's an offset of 7 hours which
                            //needs to be subtracted from the queried time. Also,
                            //the year according to FILETIME structure is number of
                            //years since 1 Jan, 1601 while the queried time shows
                            //the actual year, hance the offset 1600 is to be added
                            //to years before comparing them.

                            #region Validating Requirement MS-FSCC_R10177

                            if (fileCheck.FileExists == fileChk)
                            {
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                    (LastWriteTime.Year + 1600) == LastWriteTime_Actual.Year &&
                                    LastWriteTime.Month == LastWriteTime_Actual.Month &&
                                    LastWriteTime.Day == LastWriteTime_Actual.Day &&
                                    (LastWriteTime.Hour - 7) == LastWriteTime_Actual.Hour &&
                                    LastWriteTime.Minute == LastWriteTime_Actual.Minute &&
                                    LastWriteTime.Second == LastWriteTime_Actual.Second, 10177,
                                    "[In FileBasicInformation] LastWriteTime (8 bytes):" +
                                    " It contains the last time information was written" +
                                    " to the file in the format of a FILETIME structure.");

                                #region Validating Requirement MS-FSCC_R10185

                                //The LastWriteTime represents the time when the file was
                                //last changed, and the value of ChangeTime is equal
                                //to the LastWriteTime.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                    (LastWriteTime.Year + 1600) == LastWriteTime_Actual.Year &&
                                    LastWriteTime.Month == LastWriteTime_Actual.Month &&
                                    LastWriteTime.Day == LastWriteTime_Actual.Day &&
                                    (LastWriteTime.Hour - 7) == LastWriteTime_Actual.Hour &&
                                    LastWriteTime.Minute == LastWriteTime_Actual.Minute &&
                                    LastWriteTime.Second == LastWriteTime_Actual.Second, 10185,
                                    "[In FileBasicInformation] ChangeTime (8 bytes):" +
                                    " It contains the last time the file was changed" +
                                    " in the format of a FILETIME structure.");

                                #endregion
                            }

                            #endregion
                        }

                        #region Validating Requirement MS-FSCC_R10178

                        //Verified in the Parser.
                        //The FILETIME structure consists of two UINT32 values which forms
                        //the high and low parts of the absolute system-time, which
                        //represents the number of ticks since Jan 1, 1601.
                        FRSSite.CaptureRequirementIfIsTrue(FileLastWriteTime > 0, 10178,
                            "[In FileBasicInformation] LastWriteTime (8 bytes):" +
                            " A valid time for this field is an integer greater than 0.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10176

                        //Implicitly verified in the Parser.
                        //The parser reads two UINT32 values as prescribed
                        //by the FILETIME structure in the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10176,
                                "The length of LastWriteTime field of" +
                                " FileBasicInformation is 64 bits. ");

                        #endregion

                        UInt64 FileChangeTime = objHelper.Get__FILETIMEValue(
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.ChangeTime.dwLowDateTime,
                            objDataBuffer.streamdata[i].MetaData.FileBasicInfo.ChangeTime.dwHighDateTime);

                        DateTime ChangeTime = new DateTime((Int64)FileChangeTime);

                        #region Validating Requirement MS-FSCC_R10186

                        //Verified in the Parser.
                        //The FILETIME structure consists of two UINT32 values which forms
                        //the high and low parts of the absolute system-time, which
                        //represents the number of ticks since Jan 1, 1601.
                        FRSSite.CaptureRequirementIfIsTrue(FileChangeTime > 0, 10186,
                            "[In FileBasicInformation] ChangeTime (8 bytes):" +
                            "  A valid time for this field is an integer greater than 0.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10184

                        //Implicitly verified in the Parser.
                        //The parser reads two UINT32 values as prescribed
                        //by the FILETIME structure in the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10184,
                                "The length of ChangeTime field of" +
                                " FileBasicInformation is 64 bits. ");

                        #endregion

                        UInt32 fileAttribute = objDataBuffer.streamdata[i].MetaData.FileBasicInfo.FileAttribute;
                        bool checkAttribute = objAdapter.CheckIfValidFileAttribute(fileAttribute);

                        #region Validating Requirement MS-FSCC_R10193

                        FRSSite.CaptureRequirementIfIsTrue(checkAttribute,
                            10193, "[In FileBasicInformation] FileAttribute" +
                            " (4 bytes): It contains the file attributes" +
                            " [as specified in section 2.6].");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10192

                        //Verified in the Parser in CheckIfValidFileAttribute method.
                        //The parser verifies the value of FileAttribute returned
                        //by the server against the values prescribed by the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10192,
                                "The length of FileAttributes field" +
                                " of FileBasicInformation is 32 bits. ");

                        #endregion

                        int attributeCount = 0;
                        Dictionary<int, FileAttribute> attributeList = new Dictionary<int, FileAttribute>();

                        attributeList = objAdapter.FileAttributeList(checkAttribute,
                                                                      fileAttribute,
                                                                      out attributeCount);

                        #region Validating Requirement MS-FSCC_R10248

                        //The attribute count greater than zero indcates
                        //that a combination of file attributes is used.
                        FRSSite.CaptureRequirementIfIsTrue(attributeCount > 0,
                            10248, "They [File Attributes] can be used in any" +
                            " combination unless noted in the description of" +
                            " the attribute's meaning.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10249

                        //The value 0x00000000 is an invalid file attribute
                        //value, which is checked in the parser.
                        FRSSite.CaptureRequirementIfIsFalse(
                            attributeList.ContainsValue(FileAttribute.FILE_ATTRIBUTE_INVALID),
                            10249, " File Attributes: There is no file attribute" +
                            " with the value 0x00000000 because a value of 0x00000000" +
                            " in the FileAttribute field means that the file" +
                            " attributes for this file MUST NOT be changed when" +
                            " setting basic information for the file.");

                        #endregion

                        for (int index = 0; (attributeCount > 0) && (index < attributeList.Count); index++)
                        {
                            //The file attributes cannot be predetermined for all files.
                            //The requirements corresponding to each file attribute will
                            //be logged, based on the case in which they fall.
                            switch (attributeList[index])
                            {
                                #region FILE_ATTRIBUTE Validation

                                case FileAttribute.FILE_ATTRIBUTE_ARCHIVE:

                                    #region Validating Requirement MS-FSCC_R10250

                                    FRSSite.CaptureRequirement(10250, "File Attributes:" +
                                        " FILE_ATTRIBUTE_ARCHIVE 0x00000020 means" +
                                        " a file or directory that needs to be" +
                                        " archived. Applications use this attribute" +
                                        " to mark files for backup or removal.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_COMPRESSED:

                                    #region Validating Requirement MS-FSCC_R10251

                                    FRSSite.CaptureRequirement(10251, "File Attributes:" +
                                        " FILE_ATTRIBUTE_COMPRESSED 0x00000800 means" +
                                        " a file or directory that is compressed." +
                                        " For a file, all of the data in the file" +
                                        " is compressed. For a directory, compression" +
                                        " is the default for newly created files and" +
                                        " subdirectories.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_DIRECTORY:

                                    #region Validating Requirement MS-FSCC_R10252

                                    FRSSite.CaptureRequirement(10252, "File Attributes:" +
                                        " FILE_ATTRIBUTE_DIRECTORY 0x00000010 means" +
                                        " this item is a directory.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_ENCRYPTED:

                                    #region Validating Requirement MS-FSCC_R10253

                                    FRSSite.CaptureRequirement(10253, "File Attributes:" +
                                        " FILE_ATTRIBUTE_ENCRYPTED 0x00004000 means" +
                                        " a file or directory that is encrypted." +
                                        " For a file, all data streams in the file" +
                                        " are encrypted. For a directory, encryption" +
                                        " is the default for newly created files" +
                                        " and subdirectories.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_HIDDEN:

                                    #region Validating Requirement MS-FSCC_R10254

                                    FRSSite.CaptureRequirement(10254, "File Attributes:" +
                                        " FILE_ATTRIBUTE_HIDDEN 0x00000002 means" +
                                        " a file or directory that is hidden." +
                                        " Files and directories marked with this" +
                                        " attribute do not appear in an ordinary" +
                                        " directory listing.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_NORMAL:
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_NOT_CONTENT_INDEXED:

                                    #region Validating Requirement MS-FSCC_R10258

                                    FRSSite.CaptureRequirement(10258, "File Attributes:" +
                                        " FILE_ATTRIBUTE_NOT_CONTENT_INDEXED 0x00002000" +
                                        " means a file or directory that is not" +
                                        " indexed by the content indexing service.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_OFFLINE:

                                    #region Validating Requirement MS-FSCC_R10284

                                    FRSSite.CaptureRequirement(10284, "File Attributes:" +
                                        " FILE_ATTRIBUTE_OFFLINE 0x00001000 means" +
                                        " that the data in this file is not" +
                                        " available immediately. This attribute" +
                                        " indicates that the file data is physically" +
                                        " moved to offline storage. This attribute" +
                                        " is used by Remote Storage, which is" +
                                        " hierarchical storage management software.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_READONLY:

                                    #region Validating Requirement MS-FSCC_R10260

                                    FRSSite.CaptureRequirement(10260, "File Attributes:" +
                                        " FILE_ATTRIBUTE_READONLY 0x00000001 means" +
                                        " a file or directory that is read-only." +
                                        " For a file, applications can read the" +
                                        " file but cannot write to it or delete it." +
                                        " For a directory, applications cannot" +
                                        " delete it, but applications can create" +
                                        " and delete files from that directory.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_REPARSE_POINT:

                                    #region Validating Requirement MS-FSCC_R10261

                                    FRSSite.CaptureRequirement(10261, "File Attributes:" +
                                        " FILE_ATTRIBUTE_REPARSE_POINT 0x00000F" +
                                        " means a file or directory that has an" +
                                        " associated reparse point.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_SPARSE_FILE:

                                    #region Validating Requirement MS-FSCC_R10262

                                    FRSSite.CaptureRequirement(10262, "File Attributes:" +
                                        " FILE_ATTRIBUTE_SPARSE_FILE 0x00000200" +
                                        " means a file that is a sparse file.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_SYSTEM:

                                    #region Validating Requirement MS-FSCC_R10263

                                    FRSSite.CaptureRequirement(10263, "File Attributes:" +
                                        " FILE_ATTRIBUTE_SYSTEM 0x00000004 means" +
                                        " a file or directory that the operating" +
                                        " system uses a part of or uses exclusively.");

                                    #endregion
                                    break;

                                case FileAttribute.FILE_ATTRIBUTE_TEMPORARY:

                                    #region Validating Requirement MS-FSCC_R10264

                                    FRSSite.CaptureRequirement(10264, "File Attributes:" +
                                        " FILE_ATTRIBUTE_TEMPORARY 0x00000100" +
                                        " means a file that is being used for" +
                                        " temporary storage. The operating" +
                                        " system may choose to store this" +
                                        " file's data in memory rather than" +
                                        " on mass storage, writing the data" +
                                        " to mass storage only if data remains" +
                                        " in the file when the file is closed.");

                                    #endregion
                                    break;

                                #endregion
                            }
                        }

                        #region Validating Requirement MS-FSCC_R10246

                        UInt64 primaryDataStreamSize = objDataBuffer.streamdata[i].MetaData.primaryDataStreamSize;

                        FRSSite.CaptureRequirementIfIsTrue(primaryDataStreamSize >= 0,
                            10246, "[In FileStandardInformation]The value of this" +
                            " field [EndOfFile (8 bytes)] MUST be greater than" +
                            " or equal to 0.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10194

                        //Verified in the Parser in ValidateMetaData method.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10194,
                                "[In FileBasicInformation] Reserved" +
                                " (4 bytes): A 32-bit field. ");

                        #endregion

                        #endregion
                        break;

                    case (UInt32)FileStreamDataParser.StreamType_Values.MS_TYPE_COMPRESSION_DATA:

                        #region MS-FRS2_R812

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            812, "In the Custom Marshaling Format streamType:" +
                            " An enumeration, which MUST be" +
                            " MS_TYPE_COMPRESSION_DATA Meaning 0x00000002");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS-FSCC_R10200

                        //The parser validates that the compression type
                        //is a valid value in ValidateCompressionData method.                        
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10200,
                                "The length of CompressionFormat field of" +
                                " FileCompressionInformation is 16 bits. ");

                        #endregion

                        #region MS_TYPE_COMPRESSION_DATA

                        if (ConfigStore.IsWindows)
                        {
                            #region Validating Requirement MS-FSCC_R10203

                            //The parser validates that the compression type
                            //is a valid value in ValidateCompressionData method.
                            FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10203,
                                "Windows 2000, Windows XP, Windows Server 2003," +
                                " Windows Vista, and Windows Server 2008" +
                                " implement only one compression algorithm," +
                                " LZNT1. COMPRESSION_FORMAT_DEFAULT is therefore" +
                                " equivalent to COMPRESSION_FORMAT_LZNT1.");

                            #endregion
                        }
                        else
                        {
                            if (ConfigStore.WillingToCheckReq[10202])
                            {
                                #region Validating Requirement MS-FSCC_R10202

                                //Verified in the Parser.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10202,
                                    "An implementation can associate any local" +
                                    " compression algorithm with the values" +
                                    " [COMPRESSION_FORMAT_NONE 0x0000," +
                                    " COMPRESSION_FORMAT_DEFAULT 0x0001," +
                                    " COMPRESSION_FORMAT_LZNT1 0x0002] described" +
                                    " in the following table because the compressed" +
                                    " data does not travel across the wire in the context" +
                                    " of FSCTL, FileInformation class, or FileSystemInformation" +
                                    " class requests or replies.");

                                #endregion
                            }
                        }

                        switch (objDataBuffer.streamdata[i].CompressionData.CompressionFormat)
                        {
                            case FileStreamDataParser.CompressionFormat_Values.COMPRESSION_FORMAT_NONE:

                                #region Validating Requirement MS-FSCC_R10204

                                //Verified in the Parser.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10204,
                                    "[In FileCompressionInformation] CompressionFormat" +
                                    " (2 bytes): If the file or directory is not" +
                                    " compressed, this field is set to" +
                                    " COMPRESSION_FORMAT_NONE 0x0000.");

                                #endregion
                                break;

                            case FileStreamDataParser.CompressionFormat_Values.COMPRESSION_FORMAT_DEFAULT:

                                #region Validating Requirement MS-FSCC_R10205

                                //Verified in the Parser.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10205,
                                    "[In FileCompressionInformation] CompressionFormat" +
                                    " (2 bytes):If the file or directory is" +
                                    " compressed by using the default compression" +
                                    " algorithm, this field is set to" +
                                    " COMPRESSION_FORMAT_DEFAULT 0x0001.");

                                #endregion
                                break;

                            case FileStreamDataParser.CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1:

                                #region Validating Requirement MS-FSCC_R10206

                                //Verified in the Parser.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10206,
                                    "[In FileCompressionInformation] CompressionFormat" +
                                    " (2 bytes):If the file or directory is" +
                                    " compressed by using the LZNT1 compression" +
                                    " algorithm, this field is set to" +
                                    " COMPRESSION_FORMAT_LZNT1 0x0002.");

                                #endregion
                                break;

                            default:
                                break;
                        }

                        #endregion

                        #region MS-FRS2_R840 and MS-FRS2_R839

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        if (ConfigStore.IsWindows)
                        {
                            #region Validating Requirement MS-FRS2_R840

                            //The parser validates that the compression type
                            //is a valid value in ValidateCompressionData method.
                            FRSSite.CaptureRequirementIfIsTrue(
                                Enum.IsDefined(typeof(FileStreamDataParser.CompressionFormat_Values),
                                objDataBuffer.streamdata[i].CompressionData.CompressionFormat),
                                840, "In the Custom Marshaling Format in the windows" +
                                " implementation format field is the value of" +
                                " the CompressionFormat field of the" +
                                " FILE_COMPRESSION_INFORMATION structure" +
                                "  when stream type is MS_TYPE_COMPRESSION_DATA");

                            #endregion
                        }
                        else
                        {
                            if (ConfigStore.WillingToCheckReq[839])
                            {
                                #region Validating Requirement MS-FSCC_R839

                                //Verified in the Parser.
                                FRSSite.CaptureRequirementIfIsTrue(
                                    Enum.IsDefined(typeof(FileStreamDataParser.CompressionFormat_Values),
                                    objDataBuffer.streamdata[i].CompressionData.CompressionFormat),
                                    839, "In the Custom Marshaling Format format" +
                                    "  SHOULD be the value of the" +
                                    " CompressionFormat field of the" +
                                    " FILE_COMPRESSION_INFORMATION structure," +
                                    " as specified in [MS-FSCC] when stream type" +
                                    " is MS_TYPE_COMPRESSION_DATA");

                                #endregion
                            }
                        }

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion
                        break;

                    case (UInt32)FileStreamDataParser.StreamType_Values.MS_TYPE_REPARSE_DATA:

                        #region MS-FRS2_R813

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        //The parser verifies that the streamType value of
                        //Custom Marshaling Format is MS_TYPE_REPARSE_DATA Meaning 0x00000003.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            813, "In the Custom Marshaling Format streamType:" +
                            " An enumeration, which MUST be" +
                            " MS_TYPE_REPARSE_DATA Meaning 0x00000003.");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS-FRS2_R842

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser in ValidateReparseData method.
                        //The parser verifies that the reparse point structure
                        //adheres to the syntax as specified in the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            842, "In the Custom Marshaling Format, the data that" +
                            " follows a header tagged by MS_TYPE_REPARSE_DATA MUST" +
                            " be of a format compatible with the reply format of" +
                            " FSCTL_GET_REPARSE_POINT, as specified when stream" +
                            " type is MS_TYPE_REPARSE_DATA");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS_TYPE_REPARSE_DATA

                        #region Validating Requirement MS-FSCC_R10152

                        //Verified by the parser in ValidateReparseData method.
                        //The parser verifies that the reparse point structure
                        //adheres to the syntax as specified in the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10152,
                            "The message [FSCTL_GET_REPARSE_POINT Reply]" +
                            " contains a REPARSE_GUID_DATA_BUFFER or a" +
                            " REPARSE_DATA_BUFFER data element.");

                        #endregion

                        UInt32 reparseTag = objDataBuffer.streamdata[i].ReparseData.ReparseTag;

                        #region Validating Requirement MS-FSCC_R10153

                        //Verified by the parser in ValidateReparseData method.
                        //The parser verifies that the reparse point structure
                        //adheres to the syntax as specified in the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10153,
                            "Both the REPARSE_GUID_DATA_BUFFER and the" +
                            " REPARSE_DATA_BUFFER structures in" +
                            " FSCTL_GET_REPARSE_POINT Reply message begin" +
                            " with a ReparseTag field.");

                        #endregion

                        #region Validating Requirement MS-FSCC_R10010

                        //Verified by the parser in ValidateReparseData method.
                        //The parser verifies that the reparse point structure
                        //adheres to the syntax as specified in the TD.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10010,
                            "The length of ReparseTag field of" +
                            " REPARSE_DATA_BUFFER is 32 bits. ");

                        #endregion

                        //HighBit Set, REPARSE_DATA_BUFFER
                        if ((reparseTag & 0x80000000) == 0x80000000)
                        {
                            if (Enum.IsDefined(typeof(FileStreamDataParser.ReparseTag_Values), reparseTag))
                            {
                                #region MS-FSCC_R10007

                                FRSSite.CaptureRequirementIfIsTrue(
                                    (reparseTag & 0x80000000) == 0x80000000, 10007,
                                    "[REPARSE_DATA_BUFFER] This reparse data buffer" +
                                    " MUST be used only with reparse tag values whose" +
                                    " high bit is set to 1");

                                #endregion

                                REPARSE_DATA_BUFFER repData = new REPARSE_DATA_BUFFER();

                                repData.ReparseDataLength =
                                    objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.ReparseDataLength;

                                #region MS-FSCC_R10013

                                //Verified by the parser in ValidateReparseData method.
                                //The parser verifies that the reparse point structure
                                //adheres to the syntax as specified in the TD.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10013,
                                    " The length of ReparseDataLength field of" +
                                    " REPARSE_DATA_BUFFER is 16 bits. ");

                                #endregion

                                repData.Reserved =
                                    objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.Reserved;

                                #region MS-FSCC_R10015

                                FRSSite.CaptureRequirementIfAreEqual(2, repData.Reserved.Length,
                                    10015, "[REPARSE_DATA_BUFFER]  Reserved" +
                                    " (2 bytes): A 16-bit field. ");

                                #endregion

                                #region MS-FSCC_R10052

                                FRSSite.CaptureRequirementIfAreEqual(2, repData.Reserved.Length,
                                    10052, "[In Symbolic Link Reparse Data" +
                                    " buffer]Reserved (2 bytes):A 16-bit field. ");

                                #endregion

                                #region MS-FSCC_R10018 & MS-FSCC_R10017

                                if (ConfigStore.IsWindows)
                                {
                                    #region Validating Requirement MS-FSCC_R10018

                                    FRSSite.CaptureRequirementIfIsTrue(
                                        (repData.Reserved[0] == 0x00) && (repData.Reserved[1] == 0x00),
                                        10018, "[REPARSE_DATA_BUFFER]  Reserved (2 bytes):" +
                                        " Windows sets this field to 0.");

                                    #endregion
                                }
                                else
                                {
                                    if (ConfigStore.WillingToCheckReq[10017])
                                    {
                                        #region Validating Requirement MS-FSCC_R10017

                                        FRSSite.CaptureRequirementIfIsTrue(
                                            (repData.Reserved[0] == 0x00) && (repData.Reserved[1] == 0x00),
                                            10017, "[REPARSE_DATA_BUFFER]  Reserved (2 bytes):" +
                                            " This field SHOULD be set to 0.");

                                        #endregion
                                    }
                                }

                                #endregion

                                #region MS-FSCC_R10057 & MS-FSCC_R10056

                                if (ConfigStore.IsWindows)
                                {
                                    #region MS-FSCC_R10055

                                    FRSSite.CaptureRequirementIfIsTrue(
                                        (repData.Reserved[0] == 0x00) && (repData.Reserved[1] == 0x00),
                                        10055, "[In Symbolic Link Reparse Data" +
                                        " Buffer]Reserved (2 bytes): Windows does" +
                                        " set this field to 0.");

                                    #endregion
                                }
                                else
                                {
                                    if (ConfigStore.WillingToCheckReq[10054])
                                    {
                                        #region MS-FSCC_R10054

                                        FRSSite.CaptureRequirementIfIsTrue(
                                            (repData.Reserved[0] == 0x00) && (repData.Reserved[1] == 0x00),
                                            10054, "[In Symbolic Link Reparse Data" +
                                            " buffer]Reserved (2 bytes): It SHOULD" +
                                            " be set to 0.");

                                        #endregion
                                    }
                                }

                                #endregion

                                #region MS-FSCC_R10021

                                //Verified by the parser in ValidateReparseData method.
                                //The parser verifies that the reparse point structure
                                //adheres to the syntax as specified in the TD.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10021,
                                    "[REPARSE_DATA_BUFFER] DataBuffer (variable):" +
                                    " A variable-length byte array of size determined" +
                                    " by ReparseDatalength field of REPARSE_DATA_BUFFER.");

                                #endregion

                                #region MS-FSCC_R10045

                                //Verified by the parser in ValidateReparseData method.
                                //The parser verifies that the reparse point structure
                                //adheres to the syntax as specified in the TD.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10045,
                                    "[In Symbolic Link Reparse Data buffer] This" +
                                    " reparse data buffer MUST be used only with" +
                                    " reparse tag values whose high bit is set to 1.");

                                #endregion

                                if (reparseTag == (UInt32)FileStreamDataParser.ReparseTag_Values.IO_REPARSE_TAG_SYMLINK)
                                {
                                    #region MS-FSCC_R10049

                                    //Tag assigned to MS
                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10049,
                                        " The length of ReparseDataLength field of" +
                                        " Symbolic Link Reparse Data buffer is 16 bits. ");

                                    #endregion

                                    #region MS-FSCC_R10011

                                    //Tag assigned to MS
                                    FRSSite.CaptureRequirementIfAreEqual(
                                        FileStreamDataParser.ReparseTag_Values.IO_REPARSE_TAG_SYMLINK,
                                        (FileStreamDataParser.ReparseTag_Values)reparseTag, 10011,
                                        "[REPARSE_DATA_BUFFER]  ReparseTag (4 bytes):" +
                                        " it contains the reparse point tag that" +
                                        " uniquely identifies the owner of the reparse point.");

                                    #endregion

                                    #region MS-FSCC_R10043

                                    //Verified in the Parser.
                                    FRSSite.CaptureRequirementIfIsTrue(
                                        (reparseTag & 0x80000000) == 0x80000000, 10043,
                                        "[In Symbolic Link Reparse Data buffer]" +
                                        " This reparse data buffer MUST be used" +
                                        " only with reparse tag values whose high" +
                                        " bit is set to 1.");

                                    #endregion

                                    #region MS-FSCC_R10046

                                    FRSSite.CaptureRequirementIfAreEqual(
                                        FileStreamDataParser.ReparseTag_Values.IO_REPARSE_TAG_SYMLINK,
                                        (FileStreamDataParser.ReparseTag_Values)reparseTag, 10046,
                                        "[In Symbolic Link Reparse Data buffer]ReparseTag" +
                                        " (4 bytes): It containing the reparse" +
                                        " point tag that uniquely identifies" +
                                        " the owner (that is, the implementer of" +
                                        " the filter driver associated with this" +
                                        " ReparseTag) of the reparse point.");

                                    #endregion

                                    #region MS-FSCC_R10047

                                    FRSSite.CaptureRequirementIfAreEqual(
                                        FileStreamDataParser.ReparseTag_Values.IO_REPARSE_TAG_SYMLINK,
                                        (FileStreamDataParser.ReparseTag_Values)reparseTag, 10047,
                                        "[In Symbolic Link Reparse Data buffer]ReparseTag" +
                                        " (4 bytes): This value MUST be 0xA000000C," +
                                        " a reparse point tag assigned to Microsoft.");

                                    #endregion

                                    repData.symbolicLinkData.SubstituteNameOffset =
                                    objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameOffset;

                                    #region MS-FSCC_R10058

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10058,
                                        " The length of SubstituteNameOffset field" +
                                        " of Symbolic Link Reparse Data buffer is 16 bits. ");

                                    #endregion

                                    #region MS-FSCC_R10059

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10059,
                                        "[In Symbolic Link Reparse Data Buffer]" +
                                        " SubstituteNameOffset (2 bytes):  It MUST" +
                                        " contain the offset, in bytes, of the" +
                                        " substitute name string in the PathBuffer" +
                                        " array, computed as an offset from byte" +
                                        " 0 of PathBuffer.");

                                    #endregion

                                    repData.symbolicLinkData.SubstituteNameLength =
                                        objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameLength;

                                    #region MS-FSCC_R10062

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10062,
                                        " The length of SubstituteNameLength" +
                                        " field of Symbolic Link Reparse" +
                                        " Data buffer is 16 bits. ");

                                    #endregion

                                    #region MS-FSCC_R10064

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                        repData.symbolicLinkData.SubstituteNameLength / 2 ==
                                        objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteName.Length,
                                        10064, "[In Symbolic Link Reparse Data Buffer]" +
                                        " SubstituteNameLength (2 bytes): If this" +
                                        " string is NULL-terminated, SubstituteNameLength" +
                                        " does not include the Unicode NULL character.");

                                    #endregion

                                    repData.symbolicLinkData.PrintNameOffset =
                                        objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameOffset;

                                    #region Validating Requirement MS-FSCC_R10066

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10066,
                                        "The length of PrintNameOffset field of" +
                                        " Symbolic Link Reparse Data buffer is 16 bits. ");

                                    #endregion

                                    #region Validating Requirement MS-FSCC_R10067

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10067,
                                        "[In Symbolic Link Reparse Data Buffer]" +
                                        " PrintNameOffset (2 bytes): It contains" +
                                        " the offset of the print name string in" +
                                        " the PathBuffer array, computed as an" +
                                        " offset from byte 0 of PathBuffer.");

                                    #endregion

                                    repData.symbolicLinkData.PrintNameLength =
                                        objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameLength;

                                    #region Validating Requirement MS-FSCC_R10070

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10070,
                                        " The length of PrintNameLength field of" +
                                        " Symbolic Link Reparse Data buffer is 16 bits. ");

                                    #endregion

                                    #region Validating Requirement MS-FSCC_R10071

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10071,
                                        "[In Symbolic Link Reparse Data Buffer]" +
                                        " PrintNameLength (2 bytes): It contains" +
                                        " the length of the print name string.");

                                    #endregion

                                    #region Validating Requirement MS-FSCC_R10072

                                    //Verified in the Parser.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                        repData.symbolicLinkData.PrintNameLength / 2 == objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.symbolicLinkData.PrintName.Length,
                                        10072, "[In Symbolic Link Reparse Data Buffer]" +
                                        " PrintNameLength (2 bytes): If this string" +
                                        " [print name string] is NULL-terminated," +
                                        " PrintNameLength does not include the" +
                                        " Unicode NULL character.");

                                    #endregion

                                    repData.symbolicLinkData.Flags =
                                        objDataBuffer.streamdata[i].ReparseData.DATA_BUFFER.symbolicLinkData.Flags;

                                    #region Validating Requirement MS-FSCC_R10073

                                    //Verified by the parser in ValidateReparseData method.
                                    //The parser verifies that the reparse point structure
                                    //adheres to the syntax as specified in the TD.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10073,
                                        "[In Symbolic Link Reparse Data Buffer]Flags" +
                                        " (4 bytes): A 32-bit bit field. ");

                                    #endregion

                                    #region Validating Requirement MS-FSCC_R10076

                                    FRSSite.CaptureRequirementIfIsTrue(
                                        Enum.IsDefined(typeof(REPAESE_DATA_BUFFER_SYMLINK_Flags),
                                        repData.symbolicLinkData.Flags), 10076,
                                        "[In Symbolic Link Reparse Data Buffer]Flags" +
                                        " (4 bytes):This field contains one of the" +
                                        " values in the following table." +
                                        " [0x00000000,0x00000001]");

                                    #endregion

                                    switch (repData.symbolicLinkData.Flags)
                                    {
                                        case (UInt32)REPAESE_DATA_BUFFER_SYMLINK_Flags.SYMLINK_FLAG_FULL:

                                            #region Validating Requirement MS-FSCC_R10077

                                            FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10077,
                                                "[In Symbolic Link Reparse Data Buffer]Flags" +
                                                " (4 bytes): When this field is set to" +
                                                " 0x00000000, the path name given in the" +
                                                " SubstituteName field contains a full" +
                                                " Windows NT path name.");

                                            #endregion
                                            break;

                                        case (UInt32)REPAESE_DATA_BUFFER_SYMLINK_Flags.SYMLINK_FLAG_RELATIVE:

                                            #region Validating Requirement MS-FSCC_R10078

                                            FRSSite.CaptureRequirementIfIsTrue(ParserResult, 10078,
                                                "[In Symbolic Link Reparse Data" +
                                                " Buffer]Flags (4 bytes):When this" +
                                                " field is set to SYMLINK_FLAG_RELATIVE" +
                                                " (0x00000001), the given path name" +
                                                " is relative to the source.");

                                            #endregion
                                            break;
                                    }

                                    #region Validating Requirement MS-FSCC_R10014

                                    //Verified in the Parser.
                                    FRSSite.CaptureRequirementIfAreEqual<ushort>(
                                        (ushort)(repData.symbolicLinkData.SubstituteNameLength +
                                        repData.symbolicLinkData.PrintNameLength +
                                        (4 + (2 + 2) + (2 + 2))), repData.ReparseDataLength, 10014,
                                        "[REPARSE_DATA_BUFFER]  ReparseDataLength (2 bytes):" +
                                        "  It contains the size, in bytes, of the reparse data" +
                                        " in the DataBuffer member.");

                                    #endregion

                                    #region Validating Requirement MS-FSCC_R10051

                                    //Verified in the Parser.
                                    FRSSite.CaptureRequirementIfAreEqual<ushort>(
                                        (ushort)(repData.symbolicLinkData.SubstituteNameLength +
                                        repData.symbolicLinkData.PrintNameLength +
                                        (4 + (2 + 2) + (2 + 2))), repData.ReparseDataLength, 10051,
                                        "[In Symbolic Link Reparse Data" +
                                        " Buffer]ReparseDataLength (2 bytes): This" +
                                        " value is the length of the data starting" +
                                        " at the SubstituteNameOffset field (or" +
                                        " SubstituteNameLength + PrintNameLength + 12).");

                                    #endregion
                                }
                            }
                        }

                        #endregion
                        break;

                    case (UInt32)FileStreamDataParser.StreamType_Values.MS_TYPE_FLAT_DATA:

                        #region MS-FRS2_R814

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        //The parser verifies that the streamType value of
                        //Custom Marshaling Format is MS_TYPE_FLAT_DATA Meaning 0x00000004.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            814, "In the Custom Marshaling Format streamType:" +
                            " An enumeration, which MUST be" +
                            " MS_TYPE_FLAT_DATA Meaning 0x00000004.");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion

                        #region MS_TYPE_FLAT_DATA
                        //Logging BKUP Requirements

                        //Changing DocShortName for logging MS-BKUP Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-BKUP";

                        #region MS-BKUP_R20024

                        FRSSite.CaptureRequirementIfIsTrue(
                            Enum.IsDefined(typeof(BKUPParser.dwStreamAttributes_Values),
                            objDataBuffer.streamdata[i].FlatData.dwStreamAttributes), 20024,
                            "The value of dwStreamAttributes field of WIN32_STREAM_ID" +
                            " structure MUST be the bitwise OR of zero or more" +
                            " of the values, 0x00000000, 0x00000002 and 0x00000008.");

                        #endregion

                        #region MS-BKUP_R20025

                        FRSSite.CaptureRequirementIfIsTrue(
                            Enum.IsDefined(typeof(BKUPParser.dwStreamAttributes_Values),
                            objDataBuffer.streamdata[i].FlatData.dwStreamAttributes) &&
                            ParserResult, 20025,
                            "Other bits of dwStreamAttributes field of WIN32_STREAM_ID" +
                            " structure are unused and MUST be 0 and ignored on receipt.");

                        #endregion

                        #region MS-BKUP_R20022

                        FRSSite.CaptureRequirementIfIsTrue(
                            Enum.IsDefined(typeof(BKUPParser.dwStreamAttributes_Values),
                            objDataBuffer.streamdata[i].FlatData.dwStreamAttributes) &&
                            ParserResult, 20022,
                            "The length of dwStreamAttributes field of" +
                            "  WIN32_STREAM_ID structure is 32-bits.");

                        #endregion

                        switch (objDataBuffer.streamdata[i].FlatData.dwStreamAttributes)
                        {
                            case BKUPParser.dwStreamAttributes_Values.STREAM_NORMAL_ATTRIBUTE:

                                #region MS-BKUP_R20026

                                //This requirement is logged, when the parser
                                //encounters a normal attribute type backup stream.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult, 20026,
                                    "The value 0x00000000 for dwStreamAttributes" +
                                    " field of WIN32_STREAM_ID structure indicates" +
                                    " that the backup stream has no special attributes.");

                                #endregion
                                break;

                            case BKUPParser.dwStreamAttributes_Values.STREAM_CONTAINS_SECURITY:
                                break;

                            case BKUPParser.dwStreamAttributes_Values.STREAM_SPARSE_ATTRIBUTE:
                                break;

                            case BKUPParser.dwStreamAttributes_Values.STREAM_COMBINED_ATTRIBUTE:
                                break;
                        }

                        #region MS-BKUP_R20001

                        FRSSite.CaptureRequirementIfIsTrue(
                            Enum.IsDefined(typeof(BKUPParser.dwStreamId_Values),
                            objDataBuffer.streamdata[i].FlatData.dwStreamId), 20001,
                            "An implementation that creates a backup stream for" +
                            " an NT backup file for use over-the-wire MUST use" +
                            " only the dwStreamId values when creating a backup stream.");

                        #endregion

                        #region MS-BKUP_R20002

                        //The negative case can not be validated since the server
                        //always sends valid backup stream ID values.
                        FRSSite.CaptureRequirementIfIsTrue(
                            Enum.IsDefined(typeof(BKUPParser.dwStreamId_Values),
                            objDataBuffer.streamdata[i].FlatData.dwStreamId) &&
                            ParserResult, 20002, "An implementation reading" +
                            " an NT backup file, and then creating a file" +
                            " based on it, MUST fail if it's not one of the" +
                            " dwStreamId values.");

                        #endregion

                        #region MS-BKUP_R20007

                        //The BKUP parser would return false if there is
                        //no header as described in WIN32_STREAM_ID structure. 
                        FRSSite.CaptureRequirementIfIsTrue(
                            ParserResult, 20007, "The WIN32_STREAM_ID structure" +
                            " is a header that precedes each backup stream" +
                            " in the NT backup file.");

                        #endregion

                        #region MS-BKUP_R20011

                        FRSSite.CaptureRequirementIfIsTrue(
                            Enum.IsDefined(typeof(BKUPParser.dwStreamId_Values),
                            objDataBuffer.streamdata[i].FlatData.dwStreamId), 20011,
                            "The value of dwStreamId field of WIN32_STREAM_ID" +
                            " structure MUST be one of the following {0x00000004," +
                            " 0x00000001, 0x00000002, 0x00000005, 0x00000007," +
                            " 0x00000008, 0x00000003, 0x00000009, 0x0000000A }");

                        #endregion

                        #region MS-BKUP_R20009

                        //Parser reads stream ID values as 32 bit integer value,
                        //if the value is not among the valid values then
                        //the parser would return false.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                            Enum.IsDefined(typeof(BKUPParser.dwStreamId_Values),
                            objDataBuffer.streamdata[i].FlatData.dwStreamId), 20009,
                            "The length of dwStreamId field of  WIN32_STREAM_ID" +
                            " structure is 32-bits.");

                        #endregion

                        #region MS-BKUP_R20030

                        //Parser reads the Size field of WIN32_STREAM_ID structure
                        //as 64 bits (UInt64), if the value is not among the valid values
                        //then the parser would return false.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 20030,
                            "The length of Size field of WIN32_STREAM_ID" +
                            " structure is 64 bits.");

                        #endregion

                        #region MS-BKUP_R20035

                        //Parser reads dwStreamNameSize value as 32 bit integer value,
                        //The parser would return false if there arise a byte
                        //mismatch while reading the other field values,
                        //the parser returns true only when all the fields
                        //are checked for their syntax.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult, 20035,
                            "The length of dwStreamNameSize field of" +
                            " WIN32_STREAM_ID structure is 32 bits.");

                        #endregion

                        switch (objDataBuffer.streamdata[i].FlatData.dwStreamId)
                        {
                            case BKUPParser.dwStreamId_Values.ALTERNATE_DATA:

                                #region MS-BKUP_R20012

                                //To validate this requirement an Alternate
                                //File Stream file is created on the server,
                                //which when replicated yields this backup stream.
                                FRSSite.CaptureRequirementIfIsTrue(
                                    ParserResult, 20012, "The value" +
                                    " ALTERNATE_DATA: 0x00000004 for dwStreamId" +
                                    " field of WIN32_STREAM_ID structure" +
                                    " indicates that the backup stream contains" +
                                    " Alternative data streams.");

                                #endregion

                                #region MS-BKUP_R20038

                                FRSSite.CaptureRequirementIfIsTrue(
                                    (objDataBuffer.streamdata[i].FlatData.dwStreamNameSize > 0) &&
                                    (objDataBuffer.streamdata[i].FlatData.dwStreamNameSize <= 65536),
                                    20038, "For StreamID ALTERNATE_DATA, the" +
                                    " value of dwStreamNameSize field of" +
                                    " WIN32_STREAM_ID structure MUST be in the range 0?5536");

                                #endregion

                                #region MS-BKUP_R20039

                                FRSSite.CaptureRequirementIfIsTrue(
                                    (objDataBuffer.streamdata[i].FlatData.dwStreamNameSize % 2) == 0,
                                    20039, "For StreamID ALTERNATE_DATA, the" +
                                    " value of dwStreamNameSize field of" +
                                    " WIN32_STREAM_ID structure MUST be an" +
                                    " integral multiple of two.");

                                #endregion

                                #region MS-BKUP_R20040

                                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                    objDataBuffer.streamdata[i].FlatData.cStreamName.Length / 2 ==
                                    objDataBuffer.streamdata[i].FlatData.strStreamName.Length,
                                    20040, "cStreamName field of" +
                                    " WIN32_STREAM_ID structure is a Unicode" +
                                    " string that specifies the name of the alternate stream.");

                                #endregion

                                #region MS-BKUP_R20041

                                //If the length of the string constructed from
                                //the byte array of UNICODE characters is half
                                //the size of byte array then it can be validated
                                //that the string is not null terminated.
                                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                    (objDataBuffer.streamdata[i].FlatData.cStreamName.Length ==
                                    (objDataBuffer.streamdata[i].FlatData.strStreamName.Length * 2)),
                                    20041, "cStreamName field of" +
                                    " WIN32_STREAM_ID structure MUST NOT be null-terminated.");

                                #endregion

                                #region MS-BKUP_R20043

                                //ALTERNATE_DATA value is used only when there
                                //exists an alternate stream structure which
                                //is verified by the parser.
                                FRSSite.CaptureRequirementIfIsTrue(
                                    ParserResult, 20043, "The ALTERNATE_DATA" +
                                    " dwStreamId field value MUST be used when" +
                                    " recording named streams.");

                                #endregion

                                #region MS-BKUP_R20044

                                FRSSite.CaptureRequirementIfIsTrue(
                                    (objDataBuffer.streamdata[i].FlatData.cStreamName.Length > 0) &&
                                    ParserResult, 20044, "ALTERNATE_DATA MUST" +
                                    " have a nonzero length name in the" +
                                    " cStreamName field of the WIN32_STREAM_ID" +
                                    " structure.");

                                #endregion

                                #region MS-BKUP_R20047 and MS-BKUP_R20046

                                if (ConfigStore.IsWindows)
                                {
                                    #region MS-FRS2_R20047

                                    //Verified in the Parser.
                                    FRSSite.CaptureRequirementIfIsTrue(
                                        objDataBuffer.streamdata[i].FlatData.strStreamName.StartsWith(":") &&
                                        objDataBuffer.streamdata[i].FlatData.strStreamName.EndsWith(":$DATA"),
                                        20047, "In Windows, The NTFS file system" +
                                        " requires that the name of each" +
                                        " ALTERNATE_DATA stream in an NT backup" +
                                        " file begin with the colon character (:)." +
                                        " NTFS appends \":$DATA\" to the name of" +
                                        " any alternate stream that does not already" +
                                        " end that way, so it handles" +
                                        " a.txt:AlternateStream and a.txt:AlternateStream:$DATA identically.");

                                    #endregion
                                }
                                else
                                {
                                    if (ConfigStore.WillingToCheckReq[20046])
                                    {
                                        #region MS-FSCC_R20046

                                        //Verified in the Parser.
                                        FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                            objDataBuffer.streamdata[i].FlatData.strStreamName.StartsWith(":") &&
                                            objDataBuffer.streamdata[i].FlatData.strStreamName.EndsWith(":$DATA"),
                                            20046, "If the implementation does" +
                                            " not support named streams or does" +
                                            " not support the cStreamName that" +
                                            " is specified in the ALTERNATE_DATA" +
                                            " structure, the implementation MAY" +
                                            " ignore the stream or MAY choose some" +
                                            " other way to store the data.");

                                        #endregion
                                    }
                                }

                                #endregion

                                #region MS-BKUP_R20104

                                //This reads the number of alternate streams
                                //that are expected to be replicated.
                                //This value would change depending upon the
                                //alternate streams created in a file.
                                UInt32 streamCount = UInt32.Parse(
                                                        ConfigStore.AlternateBackupStreamCount);

                                FRSSite.CaptureRequirementIfAreEqual(streamCount,
                                    objDataBuffer.streamdata[i].FlatData.AlternateData.streamCount,
                                    20104, "If the file has any" +
                                    " named streams, the code that creates the" +
                                    " NT backup file MUST generate one" +
                                    " ALTERNATE_DATA backup stream for each" +
                                    " named stream in the file.");

                                #endregion

                                #region MS-BKUP_R20105

                                string streamName = ConfigStore.FileNameStreamValue;

                                if (!streamName.StartsWith(":"))
                                {
                                    streamName = ":" + streamName;
                                }

                                if (!streamName.EndsWith("$DATA"))
                                {
                                    streamName = streamName + ":$DATA";
                                }

                                FRSSite.CaptureRequirementIfAreEqual(streamName,
                                    objDataBuffer.streamdata[i].FlatData.strStreamName,
                                    20105, "The ALTERNATE_DATA stream MUST have" +
                                    " its cStreamName equal to that of the" +
                                    " corresponding named stream of the file.");

                                #endregion
                                break;

                            case BKUPParser.dwStreamId_Values.DATA:

                                #region MS-BKUP_R20013

                                //To validate this requirement an Data File is
                                //created on the server, which when replicated
                                //yields this data backup stream.
                                FRSSite.CaptureRequirementIfIsTrue(
                                    ParserResult, 20013, "The value DATA:" +
                                    " 0x00000001 for dwStreamId field of" +
                                    " WIN32_STREAM_ID structure indicates that" +
                                    " the backup stream contains Standard data.");

                                #endregion

                                #region MS-BKUP_R20050

                                if (ReplicationFileName == ConfigStore.FileName + ".txt")
                                {
                                    string fileData_Expected = ConfigStore.FileData;
                                    string fileData_Actual = objDataBuffer.streamdata[i].FlatData.Data.strData;

                                    //File data can not be predicted, but if known the
                                    //above lines of code can be uncommented and a
                                    //corresponding ptfConfig entry can be put to
                                    //validate this requirement.
                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                        (fileData_Expected == fileData_Actual),
                                        20050, "When creating a backup file," +
                                        " the bytes of the main stream MUST be copied" +
                                        " without modification to the data portion" +
                                        " of a data backup stream.");
                                }

                                #endregion

                                #region MS-BKUP_R20094

                                FRSSite.CaptureRequirementIfIsTrue(ParserResult &&
                                    objDataBuffer.streamdata[i].FlatData.Data.data.Length > 0,
                                    20094, "If the main stream of the file is" +
                                    " not empty, the code that creates the NT" +
                                    " backup file MUST generate a DATA backup" +
                                    " stream for it.");

                                #endregion
                                break;

                            case BKUPParser.dwStreamId_Values.OBJECT_ID:
                                break;

                            case BKUPParser.dwStreamId_Values.REPARSE_DATA:

                                #region MS-BKUP_R20017

                                //To validate this requirement an Reparse File is
                                //created on the server, which when replicated
                                //yields this reparse backup stream.
                                FRSSite.CaptureRequirementIfIsTrue(
                                    ParserResult, 20017, "The value REPARSE_DATA:" +
                                    " 0x00000008 for dwStreamId field of" +
                                    " WIN32_STREAM_ID structure indicates that" +
                                    " the backup stream contains Reparse points.");

                                #endregion

                                #region MS-BKUP_R20068

                                //Data is verified in the parser and,
                                //stored in output structure.
                                FRSSite.CaptureRequirementIfIsTrue(
                                    ParserResult, 20068, "The data portion of a" +
                                    " reparse point backup stream is the contents" +
                                    " of the reparse point, which MUST be a" +
                                    " REPARSE_DATA_BUFFER or REPARSE_GUID_DATA_BUFFER" +
                                    " structure.");

                                #endregion

                                #region MS-FSCC_R20093 & MS-FSCC_R20092

                                //To validate this requirement an Reparse File is
                                //created on the server, which when replicated
                                //yields this reparse backup stream.
                                if (ConfigStore.IsWindows)
                                {
                                    #region MS-FSCC_R20093

                                    FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                                        20093, "Windows NT backup file generates" +
                                        " a REPARSE_DATA backup stream.");

                                    #endregion
                                }
                                else
                                {
                                    if (ConfigStore.WillingToCheckReq[20092])
                                    {
                                        #region MS-FSCC_R20092

                                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                                            20092, "If the file system supports" +
                                            " it [REPARSE_DATA] and the file has" +
                                            " a reparse point, then the code" +
                                            " creating the NT backup file SHOULD" +
                                            " generate a REPARSE_DATA backup" +
                                            " stream containing it.");

                                        #endregion
                                    }
                                }

                                #endregion
                                break;

                            case BKUPParser.dwStreamId_Values.SECURITY_DATA:
                                break;
                        }

                        #region MS-BKUP_R20031

                        //This is validated by the parser, it reads the number of
                        //bytes represented by the value indicated by the Size field.
                        FRSSite.CaptureRequirementIfIsTrue(
                            ParserResult, 20031, "Size field of WIN32_STREAM_ID" +
                            " structure specifies the length of the data portion" +
                            " of the backup stream; this length MUST NOT include" +
                            " the length of the header.");

                        #endregion

                        #region MS-BKUP_R20032

                        //This is validated by the parser, it reads Size number of
                        //bytes after the header and checks if there are more bytes
                        //to be read. If there are more bytes to be read then that
                        //would constitute the next backup stream.
                        FRSSite.CaptureRequirementIfIsTrue(
                            ParserResult, 20032, "The consecutive backup streams" +
                            " within the NT backup file, if any, MUST start at" +
                            " Size + dwStreamNameSize bytes beyond the end of" +
                            " the WIN32_STREAM_ID structure.");

                        #endregion

                        if (objDataBuffer.streamdata[i].FlatData.dwStreamNameSize > 0)
                        {
                            #region MS-BKUP_R20036

                            FRSSite.CaptureRequirementIfIsTrue(
                                objDataBuffer.streamdata[i].FlatData.dwStreamNameSize ==
                                objDataBuffer.streamdata[i].FlatData.cStreamName.Length,
                                20036, "dwStreamNameSize field of" +
                                " WIN32_STREAM_ID structure specifies the length" +
                                " of the alternate stream name, in bytes.");

                            #endregion
                        }
                        else
                        {
                            #region MS-BKUP_R20036

                            FRSSite.CaptureRequirementIfIsTrue(
                                (objDataBuffer.streamdata[i].FlatData.dwStreamNameSize == 0)
                                && (objDataBuffer.streamdata[i].FlatData.cStreamName == null),
                                20036, "dwStreamNameSize field of" +
                                " WIN32_STREAM_ID structure specifies the length" +
                                " of the alternate stream name, in bytes.");

                            #endregion
                        }

                        if (objDataBuffer.streamdata[i].FlatData.dwStreamId !=
                            BKUPParser.dwStreamId_Values.ALTERNATE_DATA)
                        {
                            #region MS-BKUP_R20037

                            FRSSite.CaptureRequirementIfIsTrue(
                                objDataBuffer.streamdata[i].FlatData.dwStreamNameSize == 0,
                                20037, "dwStreamNameSize field of" +
                                " WIN32_STREAM_ID structure specifies the length" +
                                " of the alternate stream name, in bytes.");

                            #endregion
                        }

                        #region MS-BKUP_R20042

                        //This is validated by the parser, it reads the number of
                        //bytes represented by the value indicated by the Size field.
                        FRSSite.CaptureRequirementIfIsTrue(
                            ParserResult, 20042, "Size field of WIN32_STREAM_ID" +
                            " structure is the length in bytes, of the data" +
                            " that MUST follow the header.");

                        #endregion

                        #region MS-BKUP_R20082

                        //This is validated by the parser, which ensure that the
                        //byte array contains integral number of backup streams
                        //and would return a false if the backup stream structure
                        //is not complete/syntactically correct.
                        FRSSite.CaptureRequirementIfIsTrue(
                            ParserResult, 20082, "The contents of the NT backup" +
                            " file MUST be a number of backup streams, with no" +
                            " padding or other bytes between them.");

                        #endregion

                        #region MS-BKUP_R20083

                        //This is validated by the parser, which ensure that the
                        //byte array contains integral number of backup streams
                        //and would return a false if the backup stream structure
                        //is not complete/syntactically correct.
                        FRSSite.CaptureRequirementIfIsTrue(
                            ParserResult, 20083, "Each backup stream MUST consist" +
                            " of a WIN32_STREAM_ID followed by the appropriate" +
                            " backup stream data.");

                        #endregion

                        //Reverting the DocShortName back to MS-FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion
                        break;

                    case (UInt32)FileStreamDataParser.StreamType_Values.MS_TYPE_SECURITY_DATA:

                        #region MS-FRS2_R815

                        //Changing DocShortName to log the FRS2 Requirements
                        FRSSite.DefaultProtocolDocShortName = "MS-FRS2";

                        //Verified by the parser.
                        FRSSite.CaptureRequirementIfIsTrue(ParserResult,
                            815, "In the Custom Marshaling Format streamType:" +
                            " An enumeration, which MUST be" +
                            " MS_TYPE_SECURITY_DATA Meaning 0x00000006.");

                        //Reverting DocShortName back to FSCC
                        FRSSite.DefaultProtocolDocShortName = "MS-FSCC";

                        #endregion
                        break;
                }
            }

            //Reverting the DocShortName back to FRS2
            FRSSite.DefaultProtocolDocShortName = "MS-FRS2";
        }


        bool m_inShutdown = false;
        public bool InShutdown
        {
            get { return m_inShutdown; }
            set
            {
                m_inShutdown = value;
            }
        }
    }
}


