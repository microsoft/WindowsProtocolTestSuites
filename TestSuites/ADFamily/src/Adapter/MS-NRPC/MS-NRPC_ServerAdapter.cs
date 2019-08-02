// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.Modeling;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
    using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
    using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
    using Microsoft.Protocols.TestTools.StackSdk.PrintService.Rprn;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
    using System.DirectoryServices.Protocols;
    using System.Threading;
    using System.Security.Principal;

    /// <summary>
    /// Implementation class of Protocol Adapter methods.
    /// </summary>
    public partial class NrpcServerAdapter : ADCommonServerAdapter, INrpcServerAdapter 
    {
        #region Private Variables

        /// <summary>
        /// The length of Clientchallengedata structure.
        /// (a constant value is used here).
        /// </summary>
        private const int ClientChallengeDataLength = 8;

        /// <summary>
        /// nrpcClient instance of NrpcClient.
        /// </summary>
        private NrpcClient nrpcClient;

        /// <summary>
        /// An instance of NrpcCustomClient.
        /// </summary>
        private NrpcCustomClient nrpcCustomClient;

        /// <summary>
        /// Interface of the SutControlAdapter.
        /// </summary>
        private INrpcServerSutControlAdapter sutControlAdapter;

        /// <summary>
        /// Primary Domain NetBiosname.
        /// </summary>
        private string primaryDomainNetBiosName;

        /// <summary>
        /// The user name in Trust domain.
        /// </summary>
        private string trustDomainUserName;

        /// <summary>
        /// Trust Domain NetBios Name.
        /// </summary>
        private string trustDomainNetBiosName;

        /// <summary>
        /// Trust DC Name.
        /// </summary>
        private string trustDCName;

        /// <summary>
        /// Trust DC NetBios Name.
        /// </summary>
        private string trustDCNetBiosName;

        /// <summary>
        /// Primary DC Name.
        /// </summary>
        private string primaryDCName;

        /// <summary>
        /// Primary DC NetBios Name.
        /// </summary>
        private string primaryDCNetBiosName;

        /// <summary>
        /// SecondaryDCName
        /// </summary>
        private string secondaryDCName;

        private string secondaryDCNetBiosName;
        /// <summary>
        /// TCP endpoints.
        /// </summary>
        private ushort endPoint;

        /// <summary>
        /// TimeOut for bind.
        /// </summary>
        public TimeSpan timeOut;

        /// <summary>
        /// Current SUT Operating System.
        /// </summary>
        private PlatformType currentSutOperatingSystem;

        /// <summary>
        /// The negotiated flag for secure channel.
        /// </summary>
        private uint negotiatedFlag;

        private string primaryDomainDN;

        private string trustDomainDN;

        private string defaultSiteName;

        public static string pdcFQDN { get; private set; }

        public static string tdcFQDN { get; private set; }

        enum TDCExist { NotSet, Exist, Nonexist };
        private TDCExist tdcExist = TDCExist.NotSet;
        #endregion
        #region Static Configuration Variables
        /// <summary>
        /// The user name of a non-admin domain user. The value is "NrpcNonAdminUser".
        /// </summary>
        public const string NonAdminDomainUserName = "NrpcNonAdminUser";

        /// <summary>
        /// The password of user NrpcNonAdminUser. The value is "Password01!".
        /// </summary>
        public const string NonAdminDomainUserPassword = "Password01!";

        /// <summary>
        ///  The domain name which has a legal NetBIOS format but does not exist. The value is "nonexistdn".
        /// </summary>
        public const string NonExistDomainName = "nonexistdn";

        /// <summary>
        /// The name which is in an illegal format for NetBIOS name or in FQDN name. The value is "invalid/domain*name".
        /// </summary>
        public const string InvalidFormatDomainName = "invalid/domain*name";

        /// <summary>
        /// The name of a non-exist machine. The value is "nonexistmachine"
        /// </summary>
        public const string NonExistComputerName = "nonexistmachine";

        /// <summary>
        /// A non-exist site name. The value is "A-Non-Exist-Site-Name"
        /// </summary>
        public const string NonExistSiteName = "A-Non-Exist-Site-Name";

        /// <summary>
        /// An invalid user. The value is "!@@#$"
        /// </summary>
        public const string InvalidUser = "!@@#$";

        /// <summary>
        /// A non-exist domain GUID. The value is "00000000-0000-0000-0000-000000000000".
        /// </summary>
        public static readonly Guid NonExistDomainGuid = Guid.Empty;

        /// <summary>
        /// The message to be computed digest. The value is "abcdefg"
        /// </summary>
        public const string MessageForDigest = "abcdefg";

        /// <summary>
        /// A string which is not a valid IP address. The value is "19216871".
        /// </summary>
        public const string InValidIpAddrs = "19216871";

        /// <summary>
        /// Account for users whose primary account is in trust domain. 
        /// This account provides user the access to the domain, but not to any domain that trusts the domain.
        /// The value is "administrator".
        /// </summary>
        public const string TrustDomainUserAccount = "administrator";

        /// <summary>
        /// The negotiate flags used in S4 and S8 to initialize NrpcClient!Keep this value as default.
        /// </summary>
        public const int NegotiateFlags = 16644;

        /// <summary>
        /// The RID of the domain administrator. The value is 500.
        /// </summary>
        public const int DomainAdminRid = 500;

        /// <summary>
        /// A non-exist rid. The value is 9999.
        /// </summary>
        public const uint NotExistRid = 9999;

        /// <summary>
        /// Indicate whether the client uses security context or not. The value is false.
        /// </summary>
        public bool IsSecurityContext = false;

        /// <summary>
        /// The previous password for client machine account exists on PDC
        /// </summary>
        public bool PreviousEndpointPasswordExists = true;

        public bool PreviousDmPasswordExists = true;

        public bool IsClosestSiteDCAvailable = false;

        public bool IsNextClosestSiteDCAvailable = false;

        /// <summary>
        /// A DWORD that contains an implementation-specific debug flag. [MS-NRPC] section 2.2.1.7.1. The value is 0;
        /// </summary>
        public const uint DebugFlag = 0;

        #region Parameters in Unused Code
        /// <summary>
        /// The domain GUIDs list for trusted domain!The domain GUIDs should be separated with comma.
        /// The value of this variable is used in a method which is never called.
        /// </summary>
        public const string DomainGuidsForTrustedDomain = "00000000-0000-0000-0000-000000000000,ded37bc9-a141-4a7f-9da3-1b374944459c";

        /// <summary>
        /// The domain Sids list for trusted domain!The Sids should be separated with comma.
        /// The value of this variable is used in a method which is never called.
        /// </summary>
        public const string DomainSidsForTrustedDomain = "S-1-5-21-2092069645-1977021516-3004030302,S-1-5-21-1839082891-1574071360-805505941";

        /// <summary>
        /// The machine account Rids list for trusted domain!The Rids should be separated with comma.
        /// The value of this variable is used in a method which is never called.
        /// </summary>
        public const string MachineAccountRidForTrustedDomain = "1001,1001";

        /// <summary>
        /// The flag list for trusted domain!The flags should be separated with comma.
        /// </summary>
        public const string FlagsForTrustedDomain = "2,29";

        /// <summary>
        /// The parent index list for trusted domain!The parent indexes should be separated with comma.
        /// </summary>
        public const string ParentIndexForTrustedDomain = "0,0";

        /// <summary>
        /// The trust type list for trusted domain!The trust types should be separated with comma.
        /// </summary>
        public const string TrustTypeForTrustedDomain = "2,2";

        /// <summary>
        /// The trust attribute list for trusted domain!The trust attributes should be separated with comma.
        /// </summary>
        public const string TrustAttributesForTrustedDomain = "8,0";

        /// <summary>
        /// The list of domain NetBIOS names for trusted domain!The domain NetBIOS names should be separated with comma.
        /// </summary>
        public const string NetBIOSDomainNamesForTrustedDomain = "domain1,domain2";
        #endregion
        #endregion

        #region Parameters queryed using LDAP

        public string DomainAdminUPN
        {
            get
            {
                return DomainAdministratorName + "@" + PrimaryDomainDnsName;
            }
        }


        private string _pdcSid = null;
        /// <summary>
        /// The Sid of the Primary domain
        /// </summary>
        public string PdcSid
        {
            get
            {
                if (_pdcSid != null) return _pdcSid;
                byte[] result = Utilities.GetAttributeFromEntry(
                    string.Format("CN={0},OU=Domain Controllers,{1}", PDCNetbiosName, primaryDomainDN),
                    "objectSid",
                    PDCNetbiosName,
                    ADDSPortNum,
                    DomainAdministratorName,
                    DomainUserPassword) as byte[];
                _pdcSid = new SecurityIdentifier(result, 0).Value;
                return _pdcSid;
            }
        }

        private string _trustDomainSid = null;
        /// <summary>
        /// The Sid of the Trusted domain
        /// </summary>
        public string TrustDomainSid
        {
            get
            {
                if (_trustDomainSid != null) return _trustDomainSid;
                _trustDomainSid = TdcSid .Substring(0, TdcSid.LastIndexOf('-'));
                return _trustDomainSid;
            }
        }

        private int tdcRid = -1;
        /// <summary>
        /// The Relative ID of the TDC.
        /// </summary>
        public int TdcRid
        {
            get
            {
                if (tdcRid > 0) return tdcRid;
                tdcRid = int.Parse(TdcSid.Substring(TdcSid.LastIndexOf('-') + 1));
                return tdcRid;
            }
        }

        private string _tdcSid = null;
        /// <summary>
        /// The SID of TDC.
        /// </summary>
        public string TdcSid 
        {
            get
            {
                if (_tdcSid != null) return _tdcSid;
                byte[] result = Utilities.GetAttributeFromEntry(
                    string.Format("CN={0},OU=Domain Controllers,{1}", TDCNetbiosName, trustDomainDN),
                    "objectSid",
                    TDCNetbiosName,
                    ADDSPortNum,
                    DomainAdministratorName,
                    DomainUserPassword) as byte[];
                _tdcSid = new SecurityIdentifier(result, 0).Value;
                return _tdcSid;
            }
        }

        private int _primaryGroupRid = -1;
        /// <summary>
        /// The RID of the administrator's Primary Group
        /// </summary>
        public int PrimaryGroupRid
        {
            get
            {
                if (_primaryGroupRid > 0) return _primaryGroupRid;
                var result = Utilities.GetAttributeFromEntry(
                    string.Format("CN={0},CN=Users,{1}", DomainAdministratorName, primaryDomainDN),
                    "primaryGroupID",
                    PDCNetbiosName,
                    ADDSPortNum,
                    DomainAdministratorName,
                    DomainUserPassword);
                _primaryGroupRid = (int)result;
                return _primaryGroupRid;
            }
        }

        private int _pdcRid = -1;
        /// <summary>
        /// The RID of PDC
        /// </summary>
        public int PdcRid
        {
            get
            {
                if (_pdcRid > 0) return _pdcRid;
                _pdcRid = int.Parse(PdcSid.Substring(PdcSid.LastIndexOf('-') + 1));
                return _pdcRid;

            }
        }

        private int endpointRid = -1;

        /// <summary>
        /// The Relative ID of the Endpoint.
        /// </summary>
        public int EndpointRid
        {
            get
            {
                if (endpointRid > 0) return endpointRid;
                byte[] result = Utilities.GetAttributeFromEntry(
                    string.Format("CN={0},CN=Computers,{1}", ENDPOINTNetbiosName, primaryDomainDN),
                    "objectSid",
                    PDCNetbiosName,
                    ADDSPortNum,
                    DomainAdministratorName,
                    DomainUserPassword) as byte[];
                string sid = new SecurityIdentifier(result, 0).Value;
                endpointRid = int.Parse(sid.Substring(sid.LastIndexOf('-') + 1));
                return endpointRid;
            }
        }

        private int clientSupportedEncryptionTypes = -1;

        public int ClientSupportedEncryptionTypes
        {
            get
            {
                if (clientSupportedEncryptionTypes != -1) return clientSupportedEncryptionTypes;
                var result = Utilities.GetAttributeFromEntry(
                    string.Format("CN={0},CN=Computers,{1}", ENDPOINTNetbiosName, primaryDomainDN),
                    "msDS-SupportedEncryptionTypes",
                    PDCNetbiosName,
                    ADDSPortNum,
                    DomainAdministratorName,
                    DomainUserPassword);
                clientSupportedEncryptionTypes = (int)result;
                return clientSupportedEncryptionTypes;
            }
        }

        private int pdcSupportedEncryptionTypes = -1;

        public int PdcSupportedEncryptionTypes
        {
            get
            {
                if (pdcSupportedEncryptionTypes != -1) return pdcSupportedEncryptionTypes;
                var result = Utilities.GetAttributeFromEntry(
                   string.Format("CN={0},OU=Domain Controllers,{1}", PDCNetbiosName, primaryDomainDN),
                   "msDS-SupportedEncryptionTypes",
                   PDCNetbiosName,
                   ADDSPortNum,
                   DomainAdministratorName,
                   DomainUserPassword);
                pdcSupportedEncryptionTypes = (int)result;
                return pdcSupportedEncryptionTypes;
            }
        }

        private Guid trustDomainGuid = Guid.Empty;

        public Guid TrustDomainGuid
        {
            get
            {
                if (!trustDomainGuid.Equals(Guid.Empty)) return trustDomainGuid;
                byte[] result = Utilities.GetAttributeFromEntry(
                  trustDomainDN,
                  "objectGUID",
                  TDCNetbiosName,
                  ADDSPortNum,
                  DomainAdministratorName,
                  DomainUserPassword) as byte[];
                trustDomainGuid = new Guid(result);
                return trustDomainGuid;
            }
        }

        private Guid pdcDsaGuid = Guid.Empty;

        public Guid PdcDsaGuid
        {
            get
            {
                if (!pdcDsaGuid.Equals(Guid.Empty)) return pdcDsaGuid;
                byte[] result = Utilities.GetAttributeFromEntry(
                  string.Format("CN=NTDS Settings,CN={0},CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,{1}", PDCNetbiosName, primaryDomainDN),
                  "objectGUID",
                  PDCNetbiosName,
                  ADDSPortNum,
                  DomainAdministratorName,
                  DomainUserPassword) as byte[];
                pdcDsaGuid = new Guid(result);
                return pdcDsaGuid;
            }
        }


        private Guid siteGuid = Guid.Empty;

        public Guid SiteGuid
        {
            get
            {
                if (!siteGuid.Equals(Guid.Empty)) return siteGuid;
                byte[] result = Utilities.GetAttributeFromEntry(
                  string.Format("CN=Default-First-Site-Name,CN=Sites,CN=Configuration,{0}", trustDomainDN),
                  "objectGUID",
                  PDCNetbiosName,
                  ADDSPortNum,
                  DomainAdministratorName,
                  DomainUserPassword) as byte[];
                siteGuid = new Guid(result);
                return siteGuid;
            }
        }

        private int trustedDomainCount = -1;

        public int TrustedDomainCount
        {
            get
            {
                if (trustedDomainCount > 0) return trustedDomainCount;
                trustedDomainCount = Utilities.GetEntriesCount(
                    PDCNetbiosName,
                    ADDSPortNum,
                    "CN=System," + primaryDomainDN,
                    "trustedDomain",
                    DomainAdministratorName,
                    DomainUserPassword);
                return trustedDomainCount;

            }
        }


        public string[] ExpectedSiteNamesInPrimaryDomain { get; private set; }

        private string clientOsVersion = null;

        public string ClientOsVersion
        {
            get
            {
                if (clientOsVersion != null) return clientOsVersion;

                clientOsVersion = (string)Utilities.GetAttributeFromEntry(
                  string.Format("CN={0},CN=Computers,{1}", ENDPOINTNetbiosName, primaryDomainDN),
                  "operatingSystem",
                  PDCNetbiosName,
                  ADDSPortNum,
                  DomainAdministratorName,
                  DomainUserPassword);
                return clientOsVersion;
            }
        }
        #endregion

        static bool m_oneTimeBootMark = false;
        #region Public Override Methods

        /// <summary>
        ///  Initialize the parameters for validation adapter.
        /// </summary>
        /// <param name="testSite">
        ///  Provides logging, assertions, and SUT adapters for test code into its execution context.
        /// </param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            this.sutControlAdapter = Site.GetAdapter<INrpcServerSutControlAdapter>();
            pdcFQDN = PDCNetbiosName + "." + PrimaryDomainDnsName;
            tdcFQDN = TDCNetbiosName + "." + TrustDomainDnsName;
            if (tdcExist == TDCExist.NotSet)
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                try
                {
                    var reply = ping.Send(tdcFQDN, 1000);
                    if (reply.Status == System.Net.NetworkInformation.IPStatus.Success) tdcExist = TDCExist.Exist;
                    else tdcExist = TDCExist.Nonexist;
                }
                catch (System.Net.NetworkInformation.PingException)
                {
                    tdcExist = TDCExist.Nonexist; 
                }
            }
            if (tdcExist == TDCExist.Nonexist) tdcFQDN = "";
            if (ChangedNetlogonService || !m_oneTimeBootMark)
            {
                sutControlAdapter.RestoreNetlogonService(pdcFQDN, tdcFQDN);
                resetChangedNetlogonService();
            }

            if (ChangedNonDcAccountStatus || !m_oneTimeBootMark)
            {
                this.sutControlAdapter.ChangeNonDCMachineAccountStatus(true);
                resetChangedNonDcAccountStatus();
            }
            Site.DefaultProtocolDocShortName = "MS-NRPC";
            this.primaryDomainNetBiosName = this.PrimaryDomainNetBiosName;
            this.trustDomainNetBiosName = TrustDomainNetBiosName;
            this.trustDomainUserName = Site.Properties["MS_NRPC.SUT.Login.TrustDomainUserAccount"];
            DomainAdministratorName = this.DomainAdministratorName;
            if (double.Parse(Site.Properties["MS_NRPC.Adapter.BindTimeOut"], CultureInfo.InvariantCulture) < 10000)
            {
                this.timeOut = TimeSpan.FromMilliseconds(10000);
            }
            else
            {
                this.timeOut = TimeSpan.FromMilliseconds(double.Parse(Site.Properties["MS_NRPC.Adapter.BindTimeOut"], CultureInfo.InvariantCulture));
            }

            this.trustDCName = this.TDCNetbiosName + "." + this.TrustDomainDnsName;
            this.trustDCNetBiosName = this.TDCNetbiosName.ToLower(CultureInfo.InvariantCulture);
            this.primaryDCName = this.PDCNetbiosName + "." + this.PrimaryDomainDnsName;
            this.primaryDCNetBiosName = this.PDCNetbiosName.ToLower(CultureInfo.InvariantCulture);
            this.secondaryDCName = Site.Properties["Common.WritableDC2.NetbiosName"].ToLower(CultureInfo.InvariantCulture)
                + "." + this.PrimaryDomainDnsName;
            this.secondaryDCNetBiosName = Site.Properties["Common.WritableDC2.NetbiosName"].ToLower(CultureInfo.InvariantCulture);
            this.GetPlatform(out this.currentSutOperatingSystem);
            m_oneTimeBootMark = true;
            primaryDomainDN = Utilities.DomainDnsNameToDN(PrimaryDomainDnsName);
            trustDomainDN = Utilities.DomainDnsNameToDN(TrustDomainDnsName);
            defaultSiteName = Site.Properties["MS_NRPC.Adapter.DefaultSiteName"];

            string siteObject = string.Format(
                "CN={0},CN=Sites,CN=Configuration,{1}",
                "Site1",
                primaryDomainDN);
            if (!Utilities.IsObjectExist(siteObject, PDCNetbiosName, ADDSPortNum))
            {
                Utilities.CreateNewSite(
                    PDCNetbiosName,
                    "Site1",
                    PrimaryDomainDnsName,
                    DomainAdministratorName,
                    DomainUserPassword);
                Thread.Sleep(timeOut);
            }

            ExpectedSiteNamesInPrimaryDomain = Utilities.SearchSites(
                PDCNetbiosName,
                int.Parse(ADDSPortNum),
                PrimaryDomainDnsName,
                DomainAdministratorName,
                DomainUserPassword);


            string userObject = string.Format(
                "CN={0},CN=Users,{1}",
                NonAdminDomainUserName,
                primaryDomainDN);
            if (!Utilities.IsObjectExist(userObject, PDCNetbiosName, ADDSPortNum))
            {
                Utilities.NewUser(
                    PDCNetbiosName,
                    ADDSPortNum,
                    "CN=Users," + primaryDomainDN,
                    NonAdminDomainUserName,
                    NonAdminDomainUserPassword,
                    DomainAdministratorName,
                    DomainUserPassword);
                 Thread.Sleep(timeOut);
            }
            var u = ClientOsVersion;
        }

        /// <summary>
        /// Reset the Adapter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Reliability",
            "CA2001:AvoidCallingProblematicMethods",
            MessageId = "System.GC.Collect")]
        public override void Reset()
        {
            base.Reset();

            if (this.nrpcClient != null)
            {
                this.nrpcClient.Dispose();
                this.nrpcClient = null;
            }

            if (this.nrpcCustomClient != null)
            {
                this.nrpcCustomClient.Dispose();
                this.nrpcCustomClient = null;
            }

            NrpcClient.CleanAll();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }

        #endregion


        #region Public nrpc adapter methods

        #region Get Property

        /// <summary>
        /// This method is used to get the SUT's platform.
        /// </summary>
        /// <param name="sutPlatform">The SUT's platform.</param>
        public void GetPlatform(out PlatformType sutPlatform)
        {
            sutPlatform = getPlatform();
            //for those old server os, they do not support SMB2 so no need to enable it
            if (sutPlatform <= PlatformType.WindowsServer2003)
                Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce.RpceUtility.DisableSmb2 = true;
        }

        /// <summary>
        /// This method is used to get the client account type.
        /// </summary>
        /// <param name="isAdministrator">Whether the client account is an administrator account.</param>
        public void GetClientAccountType(out bool isAdministrator)
        {
            // Deprecated. Always output true for compatibility.
            isAdministrator = true;
        }

        /// <summary>
        /// Switch current user account.
        /// </summary>
        /// <param name="hasSufficientPrivileges">Whether the user account has sufficient privileges.</param>
        public void SwitchUserAccount(bool hasSufficientPrivileges)
        {
            // Deprecated
            throw new NotImplementedException();
        }
        #endregion

        #region DC Location Methods

        /// <summary>
        ///  The DsrGetDcNameEx2 method returns information about a domain controller in the specified domain and site.
        ///  The method will also verify that the responding DC database contains an account if AccountName is specified 
        ///  The SUT that receives this call is not required to be a DC.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="accountType">The type of account.</param>
        /// <param name="allowableAccountControlBits">
        ///  A set of bit flags that list properties of the AccountName account.
        /// </param>
        /// <param name="domainType">The type of domain.</param>
        /// <param name="domainGuidType">The type of domain's GUID.</param>
        /// <param name="siteType">The type of the site that the DC must be located.</param>
        /// <param name="flags">
        /// A set of bit flags that provide additional data that is used to process the request.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrGetDcNameEx2(
            ComputerType sutType,
            AccounterNameType accountType,
            uint allowableAccountControlBits,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            SiteNameType siteType,
            uint flags)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            string accountName = this.GetAccountNameFromType(accountType);
            string domainName = this.GetDomainNameFromType(domainType);
            string siteName = this.GetSiteNameFromType(siteType);

            Guid? domainGuids = null;
            this.GetDomainGuidFromType(domainGuidType, out domainGuids);
            if (!domainGuids.HasValue && domainGuidType == DomainGuidType.TrustedDomainGuid)
                Site.Assert.Fail("Environment not set to test trust domain");
            _DOMAIN_CONTROLLER_INFOW? domainControllerInfos = null;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.DsrGetDcNameEx2(
                sutName,
                accountName,
                (NrpcAllowableAccountControlBits)allowableAccountControlBits,
                domainName,
                domainGuids,
                siteName,
                (NrpcDsrGetDcNameFlags)flags,
                out domainControllerInfos);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            if (domainControllerInfos != null)
            {
                _DOMAIN_CONTROLLER_INFOW domainControllerInfo = domainControllerInfos.Value;

                bool isRecipientDc = false;
                if (ComputerType.PrimaryDc == sutType)
                {
                    isRecipientDc = true;
                }

                VerifyDsrGetDcNameEx2Response(result, domainType, (uint)flags, isRecipientDc, domainControllerInfo);
            }

            return result;
        }


        /// <summary>
        ///  The DsrGetDcNameEx returns information about a domain controller in the specified domain and site. 
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <param name="domainGuidType">The type of domain's GUID.</param>
        /// <param name="siteType">The type of the site that the DC must be located.</param>
        /// <param name="flags">
        ///  A set of bit flags that provide additional data used to process the request.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrGetDcNameEx(
            ComputerType sutType,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            SiteNameType siteType,
            uint flags)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(domainType);
            string siteName = this.GetSiteNameFromType(siteType);

            Guid? domainGuids = null;
            this.GetDomainGuidFromType(domainGuidType, out domainGuids);
            if (!domainGuids.HasValue && domainGuidType == DomainGuidType.TrustedDomainGuid)
                Site.Assert.Fail("Environment not set to test trust domain");
            _DOMAIN_CONTROLLER_INFOW? domainControllerInfos = null;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.DsrGetDcNameEx(
                 sutName,
                 domainName,
                 (Guid?)domainGuids,
                 siteName,
                 (NrpcDsrGetDcNameFlags)flags,
                 out domainControllerInfos);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            if (domainControllerInfos != null)
            {
                _DOMAIN_CONTROLLER_INFOW domainControllerInfo = domainControllerInfos.Value;

                bool isRecipientDc = false;
                if (ComputerType.PrimaryDc == sutType)
                {
                    isRecipientDc = true;
                }

                VerifyDsrGetDcNameExResponse(result, domainType, (uint)flags, isRecipientDc, domainControllerInfo);
            }


            return result;
        }


        /// <summary>
        ///  The DsrGetDcName returns information about a domain controller in the specified domain.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The name of the domain queried.</param>
        /// <param name="domainGuidType">The type of domain's GUID.</param>
        /// <param name="siteGuid">The name of the site that the DC must be located.</param>
        /// <param name="flags">
        ///  A set of bit flags that provide additional data used to process the request.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrGetDcName(
            ComputerType sutType,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            SiteGuidType siteGuid,
            uint flags)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(domainType);
            Guid? domainGuids = null;

            // This parameter will be NULL and ignored upon receiption. 
            Guid? siteGuids = null;
            _DOMAIN_CONTROLLER_INFOW? domainControllerInfos = null;

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            this.GetDomainGuidFromType(domainGuidType, out domainGuids);
            if (!domainGuids.HasValue && domainGuidType == DomainGuidType.TrustedDomainGuid)
                Site.Assert.Fail("Environment not set to test trust domain");
            this.GetSiteGuidFromType(siteGuid, out siteGuids);

            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.DsrGetDcName(
                 sutName,
                 domainName,
                 domainGuids,
                 siteGuids,
                 (NrpcDsrGetDcNameFlags)flags,
                 out domainControllerInfos);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            if (domainControllerInfos != null)
            {
                _DOMAIN_CONTROLLER_INFOW domainControllerInfo = domainControllerInfos.Value;

                bool isRecipientDc = false;
                if (ComputerType.PrimaryDc == sutType || ComputerType.TrustDc == sutType)
                {
                    isRecipientDc = true;
                }

                VerifyDsrGetDcNameResponse(result, domainType, (uint)flags, isRecipientDc, domainControllerInfo);
            }

            return result;
        }


        /// <summary>
        /// The NetrGetDCName method retrieves the NetBIOS name of the PDC for the specified domain.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="domainType">The type of domain to be queried.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrGetDCName(ComputerType sutType, DomainNameType domainType)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(domainType);
            string outBuffer = null;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrGetDCName(
                 sutName,
                 domainName,
                 out outBuffer);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            VerifyBufferInNetrGetDCName(result, domainType, outBuffer);

            return result;
        }


        /// <summary>
        ///  The NetrGetAnyDCName method retrieves the name of a domain controller in the specified primary 
        ///  or directly trusted domain. 
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrGetAnyDCName(ComputerType sutType, DomainNameType domainType)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(domainType);

            NrpcClient nrpcProxy = null;
            this.InitNrpcClient(this.IsSecurityContext, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy = this.nrpcClient;
            }
            else
            {
                nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            string outBuffer = null;
            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcProxy.NetrGetAnyDCName(
                 sutName,
                 domainName,
                 out outBuffer);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcProxy.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyBufferInNetrGetAnyDCName(result, domainType, outBuffer);

            return result;
        }


        /// <summary>
        /// The DsrGetSiteName method returns the site name for the specified computer that receives this call.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrGetSiteName(ComputerType sutType)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            string siteName = null;

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.DsrGetSiteName(sutName, out siteName);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            if (siteName != null)
            {
                VerifyDsrGetSiteName(sutType, siteName, result);
            }

            return result;
        }


        /// <summary>
        /// The DsrGetDcSiteCoverageW method returns a list of sites covered by a domain controller.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrGetDcSiteCoverageW(ComputerType sutType)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            _NL_SITE_NAME_ARRAY? siteNames = null;

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.DsrGetDcSiteCoverageW(sutName, out siteNames);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            if (siteNames != null)
            {
                _NL_SITE_NAME_ARRAY siteName = siteNames.Value;
                VerifySiteNamesInDsrGetDcSiteCoverageW(siteName, ExpectedSiteNamesInPrimaryDomain, result);
            }

            return result;
        }


        /// <summary>
        /// The DsrAddressToSiteNamesW method translates a list of socket addresses into their corresponding site names.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="socketAddresses">A list of socket addresses.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrAddressToSiteNamesW(ComputerType sutType, Set<SocketAddressType> socketAddresses)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            uint entryCount = (uint)socketAddresses.Count;
            _NL_SITE_NAME_ARRAY? siteNames = null;

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            _NL_SOCKET_ADDRESS[] socketAddressList = this.GetSocketAddressArray(socketAddresses);

            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.DsrAddressToSiteNamesW(
                 sutName,
                 entryCount,
                 socketAddressList,
                 out siteNames);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            if (siteNames != null)
            {
                _NL_SITE_NAME_ARRAY[] siteNamesArray = new _NL_SITE_NAME_ARRAY[1];
                siteNamesArray[0] = (_NL_SITE_NAME_ARRAY)siteNames;
                SocketAddressType[] socketAddresstype = socketAddresses.ToArray();
                VerifySiteNamesInDsrAddressToSiteNamesW(entryCount, siteNamesArray, socketAddresstype[0], result);
            }

            if (siteNames != null)
            {
                _NL_SITE_NAME_ARRAY siteNameArray = siteNames.Value;
                VerifyMembersInNLSiteNameArray(siteNameArray, result);
            }

            return result;
        }

        void checkIfMachineExists(ComputerType type)
        {
            if (type == ComputerType.NonDcServer && string.IsNullOrEmpty(this.SDCNetbiosName))
                Site.Assert.Fail("NonDcServer does not exist in PTFConfig, so the test case won't be executed");

            if (type == ComputerType.TrustDc && string.IsNullOrEmpty(this.TDCNetbiosName))
                Site.Assert.Fail("Trusted DC does not exist in PTFConfig, so the test case won't be executed");
        }

        /// <summary>
        ///  The DsrAddressToSiteNamesExW method translates a list of socket addresses into their corresponding 
        ///  site names and subnet names.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="socketAddresses">A list of socket addresses.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrAddressToSiteNamesExW(ComputerType sutType, Set<SocketAddressType> socketAddresses)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);

            string sutName = this.GetComputerNameFromType(sutType);
            uint entryCount = (uint)socketAddresses.Count;
            _NL_SITE_NAME_EX_ARRAY? siteNames = null;

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            _NL_SOCKET_ADDRESS[] socketAddressList = this.GetSocketAddressArray(socketAddresses);

            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.DsrAddressToSiteNamesExW(
                 sutName,
                 entryCount,
                 socketAddressList,
                 out siteNames);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }

            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            if (siteNames != null)
            {
                _NL_SITE_NAME_EX_ARRAY[] siteNamesArray = new _NL_SITE_NAME_EX_ARRAY[1];
                siteNamesArray[0] = (_NL_SITE_NAME_EX_ARRAY)siteNames;
                SocketAddressType[] socketAddresstype = socketAddresses.ToArray();

                VerifySiteNamesInDsrAddressToSiteNamesExW(entryCount, siteNamesArray, socketAddresstype[0], result);
            }

            if (siteNames != null)
            {
                _NL_SITE_NAME_EX_ARRAY siteNameArray = siteNames.Value;
                VerifyNLSiteNameExArray(result, siteNameArray);
            }

            return result;
        }


        /// <summary>
        ///  The DsrDeregisterDnsHostRecords method deletes all of the DNS SRV records registered by 
        ///  a specified domain controller.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="domainType">The type of fully qualified domain.</param>
        /// <param name="domainGuidType">The domain GUID.</param>
        /// <param name="dsaGuidType">The type of DC's TDSDSA object's GUID.</param>
        /// <param name="dnsHostType">
        ///  The type of domain whose records will be deregistered.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrDeregisterDnsHostRecords(
            ComputerType sutType,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            DsaGuidType dsaGuidType,
            ComputerType dnsHostType)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(domainType);
            string dnsHostName = this.GetComputerNameFromType(dnsHostType);
            Guid? domainGuids = null;
            Guid? dsaGuids = null;

            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName) && null != this.nrpcClient)
            {
                this.nrpcClient.Dispose();
                this.nrpcClient = null;
            }

            NrpcClient bindOverClient = NrpcClient.CreateNrpcClient(this.PrimaryDomainDnsName);

            AccountCredential accountCredential = new AccountCredential(
                this.PrimaryDomainDnsName,
                DomainAdministratorName,
                this.DomainUserPassword);

            try
            {
                bindOverClient.BindOverNamedPipe(sutName, accountCredential, null, this.timeOut);
            }
            catch (Exception err)
            {
                Site.Log.Add(LogEntryKind.Debug, "Failed to bind NamedPipe to " + sutName + " due to reason: " + err.Message);
                Site.Assert.Fail("Failed on init NRPC client on transport NamedPipe");
            }
            this.GetDomainGuidFromType(domainGuidType, out domainGuids);
            if (!domainGuids.HasValue && domainGuidType == DomainGuidType.TrustedDomainGuid)
                Site.Assert.Fail("Environment not set to test trust domain");
            this.GetDsaGuidFromType(dsaGuidType, out dsaGuids);

            HRESULT result = 0;
            try
            {
                result = (HRESULT)bindOverClient.DsrDeregisterDnsHostRecords(
                 sutName,
                 domainName,
                 domainGuids,
                 dsaGuids,
                 dnsHostName);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                bindOverClient.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = bindOverClient.Context.RpceTransportContext.InterfaceId.ToString();

            bindOverClient.Dispose();

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyDsrDeregisterDnsHostRecordsResponse(result);

            return result;
        }

        #endregion

        #region Secure Channel Establishment and Maintenance Methods

        /// <summary>
        ///  The NetrServerReqChallenge method receives a client challenge and returns a SUT challenge.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <returns> The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerReqChallenge(ComputerType primaryDcType, ComputerType clientType)
        {
            checkIfMachineExists(primaryDcType);
            this.InitNrpcClient(this.IsSecurityContext);
            string sutName = this.GetComputerNameFromType(primaryDcType);
            string clientName = this.GetComputerNameFromType(clientType);

            _NETLOGON_CREDENTIAL? serverChallengesLocal = null;

            _NETLOGON_CREDENTIAL clientChallengesLocal = new _NETLOGON_CREDENTIAL();

            // According to the TD the length of clientChallenge.data has to be 8.
            clientChallengesLocal.data = NrpcUtility.GenerateNonce(ClientChallengeDataLength);

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
                this.nrpcClient.NetrServerReqChallenge(
                    this.primaryDCNetBiosName,
                    clientName,
                    clientChallengesLocal,
                    out serverChallengesLocal);
            }

            // The client calls NetrServerReqChallenge.
            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrServerReqChallenge(
                 sutName,
                 clientName,
                 clientChallengesLocal,
                 out serverChallengesLocal);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrServerReqChallengeResponse(result);

            return result;
        }


        /// <summary>
        ///  The NetrServerAuthenticate3 method mutually authenticates the client and the SUT and establishes 
        ///  the session key to be used for the secure channel message protection between the client and the SUT.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="accountType">
        ///  The name of the account that contains the secret key (password) shared between the client and the SUT.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of the secure channel
        ///  established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidClientCredentialUsed">
        ///  A value that specifies whether client uses valid credential or not.
        /// </param>
        /// <param name="negotiateFlags">
        ///  A 32-bit set of bit flags in little-endian format that indicates features supported.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerAuthenticate3(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidClientCredentialUsed,
            uint negotiateFlags)
        {
            checkIfMachineExists(primaryDcType);
            HRESULT result = HRESULT.ERROR_FAILURE;
            string primaryName = this.GetComputerNameFromType(primaryDcType);
            string clientName = this.GetComputerNameFromType(clientType);
            string accountName = this.GetAccountNameFromType(accountType);
            uint? accountRid = null;
            string sharedSecret = this.ENDPOINTPassword;

            // Store the negotiate flag for secure channel, this flag will be used in NetrLogonGetCapabilities method.
            this.negotiatedFlag = negotiateFlags;

            NrpcComputeNetlogonCredentialAlgorithm credentialAlgorithm;
            byte[] sessionKey;

            this.GenerateSessionKey(negotiateFlags, sharedSecret, out credentialAlgorithm, out sessionKey);

            // InitNrpcClient(true, TransportType.TcpIp, (NrpcNegotiateFlags)negotiateFlags);
            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If primaryName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (primaryName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    primaryName);
            }

            nrpcClientBindOverNamedPipe.Context.SessionKey = sessionKey;
            nrpcClientBindOverNamedPipe.Context.SharedSecret = sharedSecret;

            _NETLOGON_CREDENTIAL clientCredential = new _NETLOGON_CREDENTIAL();
            _NETLOGON_CREDENTIAL? serverCredential = null;

            clientCredential.data = NrpcUtility.ComputeNetlogonCredential(
                credentialAlgorithm,
                this.nrpcClient.Context.ClientChallenge,
                nrpcClientBindOverNamedPipe.Context.SessionKey);
            if (!isValidClientCredentialUsed)
            {
                // Make the client credential invalid.
                clientCredential.data[0] += 1;
            }

            NrpcNegotiateFlags? flags = (NrpcNegotiateFlags)negotiateFlags;
            //try
            //{
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrServerAuthenticate3(
                    primaryName,
                 accountName,
                    secureChannelType,
                    clientName,
                    clientCredential,
                    out serverCredential,
                    ref flags,
                    out accountRid);
            //}
            //catch (Exception e)
            //{
            //    Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            //}
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!primaryName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrServerAuthenticate3Response(
                result,
                serverCredential,
                (uint)flags.Value,
                accountRid.Value);

            return result;
        }


        /// <summary>
        /// The NetrServerAuthenticate2 method mutually authenticates the client and the SUT and establishes 
        /// the session key to be used for the secure channel message protection between the client and the SUT.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="accountType">
        ///  The name of the account that contains the secret key (password) shared between the client and the SUT.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of the secure channel being 
        ///  established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidClientCredentialUsed">
        ///  A value that specifies whether client uses valid credential or not.
        /// </param>
        /// <param name="negotiateFlags">
        ///  A 32-bit set of bit flags in little-endian format that indicates features supported.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerAuthenticate2(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidClientCredentialUsed,
            uint negotiateFlags)
        {
            checkIfMachineExists(primaryDcType);
            HRESULT result = HRESULT.ERROR_FAILURE;
            string primaryName = this.GetComputerNameFromType(primaryDcType);
            string clientName = this.GetComputerNameFromType(clientType);
            string accountName = this.GetAccountNameFromType(accountType);
            string sharedSecret = this.ENDPOINTPassword;

            // Store the negotiate flag for secure channel, this flag will be used in NetrLogonGetCapabilities method.
            this.negotiatedFlag = negotiateFlags;

            NrpcComputeNetlogonCredentialAlgorithm credentialAlgorithm;
            byte[] sessionKey;

            this.GenerateSessionKey(negotiateFlags, sharedSecret, out credentialAlgorithm, out sessionKey);

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If primaryName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (primaryName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    primaryName);
            }

            nrpcClientBindOverNamedPipe.Context.SessionKey = sessionKey;
            nrpcClientBindOverNamedPipe.Context.SharedSecret = sharedSecret;

            _NETLOGON_CREDENTIAL clientCredential = new _NETLOGON_CREDENTIAL();
            _NETLOGON_CREDENTIAL? serverCredential = null;

            clientCredential.data = NrpcUtility.ComputeNetlogonCredential(
                credentialAlgorithm,
                this.nrpcClient.Context.ClientChallenge,
                nrpcClientBindOverNamedPipe.Context.SessionKey);
            if (!isValidClientCredentialUsed)
            {
                // Make the client credential invalid.
                clientCredential.data[0] += 1;
            }

            NrpcNegotiateFlags? flags = (NrpcNegotiateFlags)negotiateFlags;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrServerAuthenticate2(
                    primaryName,
                    accountName,
                    secureChannelType,
                    clientName,
                    clientCredential,
                    out serverCredential,
                    ref flags);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }

            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!primaryName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrServerAuthenticate2Response(result, (uint)flags.Value);

            return result;
        }


        /// <summary>
        ///  The NetrServerAuthenticate method mutually authenticates the client and the SUT and establishes 
        ///  the session key to be used for the secure channel message protection between the client and the SUT.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="accountType">
        ///  The name of the account that contains the secret key (password) shared between the client and the SUT.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of the secure channel being 
        ///  established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidClientCredentialUsed">
        ///  A value that specifies whether client uses valid credential or not.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerAuthenticate(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidClientCredentialUsed)
        {
            checkIfMachineExists(primaryDcType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string primaryName = this.GetComputerNameFromType(primaryDcType);
            string clientName = this.GetComputerNameFromType(clientType);
            string accountName = this.GetAccountNameFromType(accountType);
            string sharedSecret = this.ENDPOINTPassword;

            // Store the negotiate flag for secure channel, this flag will be used in NetrLogonGetCapabilities method.
            this.negotiatedFlag = 0;

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If primaryName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (primaryName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    primaryName);
            }

            NrpcComputeSessionKeyAlgorithm sessionKeyAlgorithm =
                NrpcComputeSessionKeyAlgorithm.DES;
            NrpcComputeNetlogonCredentialAlgorithm credentialAlgorithm =
                NrpcComputeNetlogonCredentialAlgorithm.AES128;
            this.GetEncryptAlgorithmsFromNegotiateFlag(
                (uint)nrpcClientBindOverNamedPipe.Context.NegotiateFlags,
                out sessionKeyAlgorithm,
                out credentialAlgorithm);

            #region session key

            byte[] sessionKey = new byte[16];
            bool isWeakKey = true;
            bool rebind = false;

            while (isWeakKey)
            {
                if (rebind)
                {
                    // If the DES key is weak, rebind to get generate a new key.
                    InitNrpcClient(this.IsSecurityContext);

                    _NETLOGON_CREDENTIAL? serverChallenges = null;
                    _NETLOGON_CREDENTIAL clientChallenges = new _NETLOGON_CREDENTIAL();

                    // According to the TD the length of clientChallenge.data has to be 8.
                    clientChallenges.data = NrpcUtility.GenerateNonce(ClientChallengeDataLength);

                    this.nrpcClient.NetrServerReqChallenge(
                        this.primaryDCNetBiosName,
                        clientName,
                        clientChallenges,
                        out serverChallenges);

                    if (primaryName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
                    {
                        nrpcClientBindOverNamedPipe = this.nrpcClient;
                    }
                }

                sessionKey = NrpcUtility.ComputeSessionKey(
                    sessionKeyAlgorithm,
                    sharedSecret,
                    this.nrpcClient.Context.ClientChallenge,
                    this.nrpcClient.Context.ServerChallenge);

                if (NrpcComputeNetlogonCredentialAlgorithm.DESECB == credentialAlgorithm)
                {
                    byte[] key1 = ArrayUtility.SubArray(sessionKey, 0, 7);
                    byte[] key2 = ArrayUtility.SubArray(sessionKey, 7, 7);
                    isWeakKey = DES.IsWeakKey(NrpcUtility.InitLMKey(key1))
                        || DES.IsWeakKey(NrpcUtility.InitLMKey(key2))
                        || DES.IsSemiWeakKey(NrpcUtility.InitLMKey(key2))
                        || DES.IsSemiWeakKey(NrpcUtility.InitLMKey(key1));
                }
                else
                {
                    isWeakKey = false;
                }

                if (isWeakKey)
                {
                    // If the key is weak, need to rebind to get a new key.
                    rebind = true;
                }
            }

            #endregion

            nrpcClientBindOverNamedPipe.Context.SharedSecret = this.ENDPOINTPassword;
            nrpcClientBindOverNamedPipe.Context.SessionKey = sessionKey;

            _NETLOGON_CREDENTIAL clientCredential = new _NETLOGON_CREDENTIAL();
            _NETLOGON_CREDENTIAL? serverCredential = null;
            clientCredential.data = NrpcUtility.ComputeNetlogonCredential(
                credentialAlgorithm,
                this.nrpcClient.Context.ClientChallenge,
                nrpcClientBindOverNamedPipe.Context.SessionKey);

            if (!isValidClientCredentialUsed)
            {
                // Make the client credential invalid.
                clientCredential.data[0] += 1;
            }
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrServerAuthenticate(
                    primaryName,
                    accountName,
                    secureChannelType,
                    clientName,
                    clientCredential,
                    out serverCredential);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }

            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!primaryName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            return result;
        }


        /// <summary>
        ///  The NetrServerPasswordSet2 method allows the client to set a new clear text password for an account used by
        ///  the domain controller (as specified in section 1.5) for setting up the secure channel from the client.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="accountType">The name of the account whose password is being changed.</param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of the secure channel being 
        ///  established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerPasswordSet2(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed)
        {
            checkIfMachineExists(primaryDcType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string primaryDcName = this.GetComputerNameFromType(primaryDcType);
            string accountName = this.GetAccountNameFromType(accountType);
            string clientName = this.GetComputerNameFromType(clientType);

            _NL_TRUST_PASSWORD trustPassword = this.nrpcClient.CreateNlTrustPassword(this.ENDPOINTPassword, true);
            _NL_TRUST_PASSWORD encryptedTrustPassword = this.nrpcClient.EncryptNlTrustPassword(trustPassword);
            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If primaryDCname not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (primaryDcName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    primaryDcName);
            }

            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid, authenticator verification fails. 
                // In this situation. STATUS_ACCESS_DENIED error will be returned.
                clientAuthenticator.Credential.data[0] += 1;
            }
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrServerPasswordSet2(
                    primaryDcName,
                    accountName,
                    secureChannelType,
                    clientName,
                    clientAuthenticator,
                    out serverAuthenticator,
                    encryptedTrustPassword);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!primaryDcName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrServerPasswordSet2Response(
                result,
                serverAuthenticator,
                trustPassword,
                this.ENDPOINTPassword);

            return result;
        }


        /// <summary>
        ///  The NetrServerPasswordSet method sets a new one-way function (OWF) of a password for an account used 
        ///  by the domain controller (as detailed in section 1.5) for setting up the secure channel from the client.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="accountType"> The name of the account whose password will be changed.</param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of the secure channel being 
        ///  established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerPasswordSet(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed)
        {
            checkIfMachineExists(primaryDcType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string primaryDcName = this.GetComputerNameFromType(primaryDcType);
            string accountName = this.GetAccountNameFromType(accountType);
            string clientName = this.GetComputerNameFromType(clientType);

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If primaryDCname not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (primaryDcName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    primaryDcName);
            }

            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();

            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid, authenticator verification fails. 
                // In this situation. STATUS_ACCESS_DENIED error will be returned.
                clientAuthenticator.Credential.data[0] += 1;
            }

            _LM_OWF_PASSWORD? uasNewPassword = this.nrpcClient.EncryptUasNewPassword(this.ENDPOINTPassword);
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrServerPasswordSet(
                    primaryDcName,
                    accountName,
                    secureChannelType,
                    clientName,
                    clientAuthenticator,
                    out serverAuthenticator,
                    uasNewPassword);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            byte[] sessionKey = nrpcClientBindOverNamedPipe.Context.SessionKey;

            if (!primaryDcName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrServerPasswordSetResponse(
                result,
                serverAuthenticator,
                uasNewPassword.Value,
                this.ENDPOINTPassword,
                sessionKey);

            return result;
        }


        /// <summary>
        ///  The NetrServerTrustPasswordsGet method returns the encrypted current and previous passwords
        ///  for an account in the domain.
        /// </summary>
        /// <param name="trustedDcType">The type of a trusted DC.</param>
        /// <param name="accountType"> 
        ///  The name of the client account in the domain for which the trust password MUST be returned.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of the secure channel being 
        ///  established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerTrustPasswordsGet(
            ComputerType trustedDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed)
        {
            checkIfMachineExists(trustedDcType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string trustedDCName = this.GetComputerNameFromType(trustedDcType);
            string accountName = this.GetAccountNameFromType(accountType);
            string computerName = this.GetComputerNameFromType(clientType);

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If trustedDCName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (trustedDCName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    trustedDCName);
            }

            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();

            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid, authenticator verification fails. 
                // In this situation. STATUS_ACCESS_DENIED error will be returned.
                clientAuthenticator.Credential.data[0] += 1;
            }

            _NETLOGON_AUTHENTICATOR? serverAuthenticator = null;
            _NT_OWF_PASSWORD? encryptedOldOwfPassword = null;
            _NT_OWF_PASSWORD? encryptedNewOwfPassword = null;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrServerTrustPasswordsGet(
                    trustedDCName,
                    accountName,
                    secureChannelType,
                    computerName,
                    clientAuthenticator,
                    out serverAuthenticator,
                    out encryptedNewOwfPassword,
                    out encryptedOldOwfPassword);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            byte[] sessionKey = nrpcClientBindOverNamedPipe.Context.SessionKey;

            if (!trustedDCName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrServerTrustPasswordsGetResponse(
                result,
                serverAuthenticator,
                encryptedNewOwfPassword,
                encryptedOldOwfPassword,
                secureChannelType,
                sessionKey);

            return result;
        }


        /// <summary>
        ///  The NetrLogonGetDomainInfo method returns information that describes the current domain 
        ///  to which the specified client belongs.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="clientType">The type of the client computer issuing the request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <param name="level">The information level requested by the client.</param>
        /// <param name="workStationInfo">The information about the client workstation.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonGetDomainInfo(
            ComputerType sutType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            uint level,
            AbstractNetLogonWorkStationInformation workStationInfo)
        {
            checkIfMachineExists(sutType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            // csdVersion seems do not need any value.
            string csdVersion = null;
            ushort servicePackMajor = 0;
            ushort servicePackMinor = 0;

            string sutName = this.GetComputerNameFromType(sutType);
            string clientName = this.GetComputerNameFromType(clientType);

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();
            Version versionData = Environment.OSVersion.Version;

            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid, authenticator verification fails. 
                // In this situation. STATUS_ACCESS_DENIED error will be returned.
                clientAuthenticator.Credential.data[0] += 1;
            }

            OSVERSIONINFOEX clientOsVersion = this.nrpcClient.CreateOsVersionInfoEx(
               (uint)versionData.Major,
               (uint)versionData.Minor,
               (uint)versionData.Build,
               RprnProcessorArchitecture.PROCESSOR_ARCHITECTURE_INTEL,
               csdVersion,
               servicePackMajor,
               servicePackMinor,
               RprnProductSuiteFlags.VER_SUITE_WH_SERVER,
               workStationInfo.WorkStationInfo.ClientOSType);

            string clientDnsHostName = this.ENDPOINTNetbiosName + "." + this.PrimaryDomainDnsName; 
            _NETLOGON_WORKSTATION_INFORMATION workStaBuffer;
            if (!workStationInfo.WorkStationInfo.IsOsVersionNull)
            {
                workStaBuffer = this.nrpcClient.CreateNetlogonWorkstationInformation(
                (Level_Values)level,
                clientDnsHostName,
                defaultSiteName,
                clientOsVersion,
                workStationInfo.WorkStationInfo.IsOsNameNull ? string.Empty : ClientOsVersion,
                (NrpcWorkstationFlags)workStationInfo.WorkStationInfo.WorkStationFlags,
                (uint)ClientSupportedEncryptionTypes);
            }
            else
            {
                workStaBuffer = this.nrpcClient.CreateNetlogonWorkstationInformation(
                (Level_Values)level,
                clientDnsHostName,
                defaultSiteName,
                null,
                workStationInfo.WorkStationInfo.IsOsNameNull ? string.Empty : ClientOsVersion,
                (NrpcWorkstationFlags)workStationInfo.WorkStationInfo.WorkStationFlags,
                (uint)ClientSupportedEncryptionTypes);
            }

            _NETLOGON_DOMAIN_INFORMATION? domBuffer = null;

            string dnsHostName = this.sutControlAdapter.GetDnsHostNameAttributeOfClient();
            serverAuthenticator = null;
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrLogonGetDomainInfo(
                    sutName,
                    clientName,
                    clientAuthenticator,
                    ref serverAuthenticator,
                    (Level_Values)level,
                    workStaBuffer,
                    out domBuffer);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            // dnsHostName will be set to null if NetrLogonGetDomainInfo is executed, 
            // call SetDnsHostNameAttributeOfClient to reset dnsHostName for client computer on DC.
            bool setDnsHostNameSuccess = this.sutControlAdapter.SetDnsHostNameAttributeOfClient(dnsHostName);

            if (!setDnsHostNameSuccess)
            {
                Site.Log.Add(LogEntryKind.Debug, "Reset client DnsHostName for client failed.");
            }

            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrLogonGetDomainInfoResponse(
                result,
                serverAuthenticator,
                (Level_Values)level,
                domBuffer,
                workStaBuffer,
                dnsHostName,
                this.sutControlAdapter.GetServicePrincipalNameAttribute(),
                this.sutControlAdapter.GetOperatingSystemAttribute());

            if ((Level_Values)level == Level_Values.NetlogonDomainInfo)
            {
                VerifyNetlogonDomainInfo(result, domBuffer);
            }

            return result;
        }


        /// <summary>
        /// The NetrLogonGetCapabilities method is used by the client to confirm the SUT capabilities 
        /// after a secure channel has been established.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="clientType">The type of the client computer issuing the request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <param name="queryLevel">
        ///  The level of information that will be returned from the SUT.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonGetCapabilities(
            ComputerType sutType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            uint queryLevel)
        {
            checkIfMachineExists(sutType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string sutName = this.GetComputerNameFromType(sutType);
            string clientName = this.GetComputerNameFromType(clientType);

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If sutName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();
            _NETLOGON_CAPABILITIES? capabilities = null;

            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid, authenticator verification fails. 
                // In this situation. STATUS_ACCESS_DENIED error will be returned.
                clientAuthenticator.Credential.data[0] += 1;
            }
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrLogonGetCapabilities(
                    sutName,
                    clientName,
                    clientAuthenticator,
                    ref serverAuthenticator,
                    queryLevel,
                    out capabilities);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            _NETLOGON_CAPABILITIES? serverCapabilities = null;

            if (capabilities != null)
            {
                _NETLOGON_CAPABILITIES tempCapability = new _NETLOGON_CAPABILITIES();
                tempCapability.ServerCapabilities = capabilities.Value.ServerCapabilities;
                serverCapabilities = tempCapability;
            }

            VerifyNetrLogonGetCapabilitiesResponse(
                result,
                serverAuthenticator,
                queryLevel,
                serverCapabilities,
                this.negotiatedFlag);

            return result;
        }

        #endregion

        #region Pass-Through Authentication Methods

        /// <summary>
        ///  The NetrLogonSamLogonEx method handles logon requests for the SAM accounts 
        ///  and allows generic pass-through authentication.
        /// </summary>
        /// <param name="useSecureRpc">Whether secure RPC channel is used.</param>
        /// <param name="logonServerType"> The NetBIOS name of the SUT that will handle the logon request.</param>
        /// <param name="clientType">The type of client sending the logon request.</param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure that specifies the type of the logon information
        ///  passed in the LogonInformation parameter.
        /// </param>
        /// <param name="logonInformationType">The type of logon request information.</param>
        /// <param name="validationLevel"> 
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type that contains the validation level 
        ///  requested by the client.
        /// </param>
        /// <param name="extraFlags">A set of bit flags that specify delivery settings.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonSamLogonEx(
            bool useSecureRpc,
            ComputerType logonServerType,
            ComputerType clientType,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            uint extraFlags)
        {
            checkIfMachineExists(logonServerType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string logonServerName = this.GetComputerNameFromType(logonServerType);
            string clientName = this.GetComputerNameFromType(clientType);

            // Initial a NrpcClient instance to bind
            NrpcClient nrpcProxy = NrpcClient.CreateNrpcClient(this.PrimaryDomainDnsName);

            _NETLOGON_LEVEL? encryptedLogonLevel = null;

            if (!useSecureRpc)
            {
                if (logonInformationType != LogonInformationType.Null)
                {
                    encryptedLogonLevel = this.GetEncryptedLogonLevel(logonLevel);
                }

                if (null != this.nrpcClient)
                {
                    this.nrpcClient.Dispose();
                    this.nrpcClient = null;
                }

                #region credential

                AccountCredential accountCredential = new AccountCredential(
                    this.PrimaryDomainDnsName,
                    DomainAdministratorName,
                    this.DomainUserPassword);

                #endregion

                nrpcProxy.Context.NegotiateFlags = NrpcNegotiateFlags.SupportsGenericPassThroughAuthentication
                                                   | NrpcNegotiateFlags.SupportsStrongKeys;
                nrpcProxy.Context.SealSecureChannel = false;

                SspiClientSecurityContext sspiSecu = new SspiClientSecurityContext(
                    SecurityPackageType.Negotiate,
                    accountCredential,
                    this.primaryDCName,
                    ClientSecurityContextAttribute.MutualCredValidataion,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);
                try
                {
                    nrpcProxy.BindOverNamedPipe(this.primaryDCName, accountCredential, sspiSecu, this.timeOut);
                }
                catch (Exception e)
                {
                    Site.Log.Add(LogEntryKind.Debug, "Failed to bind NamedPipe to " + this.primaryDCName + " due to reason: " + e.Message);
                    Site.Assert.Fail("Failed on init NRPC client on transport NamedPipe");
                }
            }
            else
            {
                if (logonServerName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName)
                    || logonServerType == ComputerType.NonExistComputer)
                {
                    this.InitNrpcClient(
                        true,
                        TransportType.NamedPipe,
                        (NrpcNegotiateFlags)NegotiateFlags);
                    nrpcProxy = this.nrpcClient;
                }
                else
                {
                    nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                                    false,
                                    this.GetNrpcNegotiateFlags(),
                                    logonServerName);
                }

                if (logonInformationType != LogonInformationType.Null)
                {
                    encryptedLogonLevel = this.GetEncryptedLogonLevel(logonLevel);
                }
            }

            _NETLOGON_VALIDATION? validationInfomation = null;
            byte? authoritative = null;
            NrpcNetrLogonSamLogonExtraFlags? netExtraFalgs = (NrpcNetrLogonSamLogonExtraFlags)extraFlags;

            // Client calls NetrLogonSamLogonEx
            try
            {
                result = (HRESULT)nrpcProxy.NetrLogonSamLogonEx(
                        nrpcProxy.Handle,
                        logonServerName,
                        clientName,
                        logonLevel,
                        encryptedLogonLevel,
                        validationLevel,
                        out validationInfomation,
                        out authoritative,
                        ref netExtraFalgs);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcProxy.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();

            byte[] sessionKey = nrpcProxy.Context.SessionKey;

            if (useSecureRpc && (!(logonServerName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName)
                                   || logonServerType == ComputerType.NonExistComputer)))
            {
                nrpcProxy.Dispose();
            }

            if (null != nrpcClientSecurityContext && useSecureRpc)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrLogonSamLogonExResponse(
                result,
                logonLevel,
                encryptedLogonLevel.Value,
                netExtraFalgs.Value,
                (NrpcNetrLogonSamLogonExtraFlags)extraFlags,
                sessionKey);

            return result;
        }


        /// <summary>
        /// The NetrLogonSamLogonWithFlags method handles logon requests for the SAM accounts.
        /// </summary>
        /// <param name="logonServerType">The type of SUT logged on.</param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, 
        ///  that specifies the type of logon information passed in the LogonInformation parameter.
        /// </param>
        /// <param name="logonInformationType">The type of logon request information.</param>
        /// <param name="validationLevel"> 
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type 
        ///  that contains the validation level requested by the client.
        /// </param>
        /// <param name="extraFlags">A set of bit flags that specify delivery settings.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonSamLogonWithFlags(
            ComputerType logonServerType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            uint extraFlags)
        {
            checkIfMachineExists(logonServerType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string logonServerName = this.GetComputerNameFromType(logonServerType);
            string clientName = this.GetComputerNameFromType(clientType);

            _NETLOGON_LEVEL? encryptedLogonLevel = null;

            // Init nrpc client which binding over TCP with SecurityContext
            this.InitNrpcClient(
                true,
                TransportType.NamedPipe,
                (NrpcNegotiateFlags)NegotiateFlags);

            if (logonInformationType != LogonInformationType.Null)
            {
                _NETLOGON_LEVEL logon_Level = new _NETLOGON_LEVEL();

                logon_Level = this.nrpcClient.CreateNetlogonLevel(
                               logonLevel,
                               NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                               this.PrimaryDomainDnsName,
                               DomainAdministratorName,
                               this.DomainUserPassword);

                // Client calls EncryptNetlogonLevel
                encryptedLogonLevel = this.nrpcClient.EncryptNetlogonLevel(
                   logonLevel,
                   logon_Level);
            }

            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If logonServerName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (logonServerName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    false,
                    this.GetNrpcNegotiateFlags(),
                    logonServerName);
            }

            _NETLOGON_VALIDATION? validationInfomation = null;
            byte? authoritative = null;
            NrpcNetrLogonSamLogonExtraFlags? netExtraFalgs = (NrpcNetrLogonSamLogonExtraFlags)extraFlags;

            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid, authenticator verification fails. 
                // In this situation, STATUS_ACCESS_DENIED error will be returned.
                clientAuthenticator.Credential.data[0] += 1;
            }

            // get the LastLogon PwdLastSet and LogonCount attribute from AD.
            long expectedPasswordLastSet = this.sutControlAdapter.GetPwdLastSetAttribute(NonAdminDomainUserName, NonAdminDomainUserPassword);
            ushort expectedLogonCount = ushort.Parse(this.sutControlAdapter.GetLogonCountAttribute(), CultureInfo.InvariantCulture);

            // Client calls NetrLogonSamLogonWithFlags
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrLogonSamLogonWithFlags(
                    logonServerName,
                    clientName,
                    clientAuthenticator,
                    ref serverAuthenticator,
                    logonLevel,
                    encryptedLogonLevel,
                    validationLevel,
                    out validationInfomation,
                    out authoritative,
                    ref netExtraFalgs);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            byte[] sessionKey = nrpcClientBindOverNamedPipe.Context.SessionKey;

            if (!logonServerName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            // verify NETLOGON_AUTHENTICATOR structure.
            VerifyNetlogonAuthenticator(result, serverAuthenticator);

            // get the attribute from AD.
            string expectedEffectiveName = this.sutControlAdapter.GetSamAccountNameAttribute();
            string expectedLogonScript = this.sutControlAdapter.GetScriptPathAttribute();
            string expectedProfilePath = this.sutControlAdapter.GetProfilePathAttribute();
            string expectedHomeDirectory = this.sutControlAdapter.GetHomeDirectoryAttribute();
            string expectedHomeDirectoryDrive = this.sutControlAdapter.GetHomeDriveAttribute();
            ushort expectedBadPasswordCount = ushort.Parse(this.sutControlAdapter.GetBadPwdCountAttribute(), CultureInfo.InvariantCulture);

            // Verify NetrLogon SamLogon With Flags response
            VerifyNetrLogonSamLogonWithFlagsResponse(
                result,
                serverAuthenticator,
                logonLevel,
                encryptedLogonLevel.Value,
                validationLevel,
                validationInfomation,
                expectedPasswordLastSet,
                expectedEffectiveName,
                expectedLogonScript,
                expectedProfilePath,
                expectedHomeDirectory,
                expectedHomeDirectoryDrive,
                expectedBadPasswordCount,
                expectedLogonCount,
                sessionKey);

            return result;
        }


        /// <summary>
        /// The NetrLogonSamLogon method handles logon requests for the SAM accounts.
        /// </summary>
        /// <param name="logonServerType">The type of SUT logged on.</param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, 
        ///  that specifies the type of logon information passed in the LogonInformation parameter.
        /// </param>
        /// <param name="logonInformationType">The type of logon request information.</param>
        /// <param name="validationLevel">
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type 
        ///  that contains the validation level requested by the client.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonSamLogon(
            ComputerType logonServerType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel)
        {
            checkIfMachineExists(logonServerType);
            HRESULT result = HRESULT.ERROR_FAILURE;
            string logonServerName = this.GetComputerNameFromType(logonServerType);
            string clientName = this.GetComputerNameFromType(clientType);
            _NETLOGON_LEVEL? encryptedLogonLevel = null;

            // Init nrpc client which binding over namedpipe with SecurityContext
            this.InitNrpcClient(
                true,
                TransportType.NamedPipe,
                (NrpcNegotiateFlags)NegotiateFlags);

            if (logonInformationType != LogonInformationType.Null)
            {
                encryptedLogonLevel = this.GetEncryptedLogonLevel(logonLevel);
            }

            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();

            NrpcClient nrpcClientBindOverNamedPipe = null;

            // If logonServerName not contains primary DC NetBios Name, BindOverNamedPipe to get STATUS_NOT_SUPPORTED.
            if (logonServerName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe = this.nrpcClient;
            }
            else
            {
                nrpcClientBindOverNamedPipe = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    logonServerName);
            }

            _NETLOGON_VALIDATION? validationInformation = null;
            byte? authoritative = null;
            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid, authenticator verification fails.
                // In this situation, STATUS_ACCESS_DENIED error will be returned.
                clientAuthenticator.Credential.data[0] += 1;
            }

            // Client calls NetrLogonSamLogon
            try
            {
                result = (HRESULT)nrpcClientBindOverNamedPipe.NetrLogonSamLogon(
                    logonServerName,
                    clientName,
                    clientAuthenticator,
                    ref serverAuthenticator,
                    logonLevel,
                    encryptedLogonLevel,
                    validationLevel,
                    out validationInformation,
                    out authoritative);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcClientBindOverNamedPipe.Context.RpceTransportContext.InterfaceId.ToString();

            if (!logonServerName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcClientBindOverNamedPipe.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            return result;
        }


        /// <summary>
        /// The NetrLogonSamLogoff method handles logoff requests for the SAM accounts.
        /// </summary>
        /// <param name="logonServerType">The type of SUT logged on.</param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="authenticatorType">The type of authenticator.</param>
        /// <param name="isReturnAuthenticatorNull">Whether the ReturnAuthenticator passed in is NULL.</param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure
        ///  that specifies the type of logon information passed in the LogonInformation parameter.
        /// </param>
        /// <param name="logonInformationType">The type of logon request information.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonSamLogoff(
            ComputerType logonServerType,
            ComputerType clientType,
            AuthenticatorType authenticatorType,
            bool isReturnAuthenticatorNull,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType)
        {
            checkIfMachineExists(logonServerType);
            HRESULT result = HRESULT.ERROR_FAILURE;

            string logonServerName = this.GetComputerNameFromType(logonServerType);
            string clientName = this.GetComputerNameFromType(clientType);

            _NETLOGON_LEVEL? encryptedLogonLevel = null;

            // Update encrypted logon level according to the type
            encryptedLogonLevel = this.GetEncryptedLogonLevel(logonLevel);

            _NETLOGON_AUTHENTICATOR? serverAuthenticator = null;
            _NETLOGON_AUTHENTICATOR? clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();

            // Update client authenticator according to the type
            if (authenticatorType == AuthenticatorType.Null)
            {
                clientAuthenticator = null;
            }
            else if (authenticatorType == AuthenticatorType.Invalid)
            {
                // Makes the authenticator becomes invalid.
                ((_NETLOGON_AUTHENTICATOR)clientAuthenticator).Credential.data[0] += 1;
            }

            // If the returned authenticator is not null, create an empty for it
            if (!isReturnAuthenticatorNull)
            {
                serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();
            }

            this.nrpcClient.Dispose();

            // nrpc security context when response is received
            NrpcCustomClientSecurityContext secuContextInResp = null;

            // interface id in response
            string interfaceId = null;

            this.nrpcCustomClient = new NrpcCustomClient(this.PrimaryDomainDnsName);
            NrpcCustomClientSecurityContext secuContext = null;

            if (this.IsSecurityContext)
            {
                this.nrpcCustomClient.Context.NegotiateFlags = this.GetNrpcNegotiateFlags();
                MachineAccountCredential machineCredential = new MachineAccountCredential(
                    this.PrimaryDomainDnsName,
                    this.ENDPOINTNetbiosName,
                    this.ENDPOINTPassword);
                secuContext = new NrpcCustomClientSecurityContext(
                    this.PrimaryDomainDnsName,
                    this.primaryDCNetBiosName,
                    machineCredential,
                    true,
                    this.nrpcCustomClient.Context.NegotiateFlags);
            }

            AccountCredential accountCredential = new AccountCredential(
                this.PrimaryDomainDnsName,
                DomainAdministratorName,
                this.DomainUserPassword);
            if (this.currentSutOperatingSystem == PlatformType.WindowsServer2008 && logonServerName.Contains("."))
            {
                logonServerName = logonServerName.Remove(logonServerName.IndexOf('.'));
            }

            this.nrpcCustomClient.BindOverNamedPipe(
                logonServerName,
                accountCredential,
                secuContext,
                this.timeOut);
            try
            {
                result = (HRESULT)this.nrpcCustomClient.NetrLogonSamLogoff(
                    logonServerName,
                    clientName,
                    clientAuthenticator,
                    ref serverAuthenticator,
                    logonLevel,
                    encryptedLogonLevel,
                    (LogonInformationType.Null == logonInformationType));
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            secuContextInResp = this.nrpcCustomClient.Context.RpceTransportContext.SecurityContext
                                as NrpcCustomClientSecurityContext;
            interfaceId = this.nrpcCustomClient.Context.RpceTransportContext.InterfaceId.ToString();

            this.nrpcCustomClient.Dispose();

            if (null != secuContextInResp)
            {
                VerifySignatureRelatedRequirement(secuContextInResp);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            // Verify NetrLogonSamLogoff Response information.
            VerifyNetrLogonSamLogoffResponse(result, serverAuthenticator);

            return result;
        }

        #endregion

        #region Domain Trust Methods

        /// <summary>
        /// The DsrEnumerateDomainTrusts method returns an enumerated list of domain trusts
        /// filtered by a set of flags from the specified SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="flags">
        /// A set of bit flags that specify properties that MUST be true for a domain trust
        /// to be part of the returned domain name list.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrEnumerateDomainTrusts(ComputerType sutType, uint flags)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);
            _NETLOGON_TRUSTED_DOMAIN_ARRAY? trustedDomains = null;

            InitNrpcClient(this.IsSecurityContext);

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.DsrEnumerateDomainTrusts(
                 sutName,
                 (NrpcDsrEnumerateDomainTrustsFlags)flags,
                 out trustedDomains);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            VerifyDsrEnumerateDomainTrusts(sutType, trustedDomains, result);

            return result;
        }


        /// <summary>
        /// The NetrEnumerateTrustedDomainsEx method returns a list of trusted domains from a specified SUT
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrEnumerateTrustedDomainsEx(ComputerType sutType)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);
            string sutName = this.GetComputerNameFromType(sutType);
            _NETLOGON_TRUSTED_DOMAIN_ARRAY? trustedDomains = null;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrEnumerateTrustedDomainsEx(sutName, out trustedDomains);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            VerifyNetrEnumerateTrustedDomainsEx(trustedDomains, result);

            return result;
        }


        /// <summary>
        /// The NetrEnumerateTrustedDomains method returns a set of trusted domain names.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrEnumerateTrustedDomains(ComputerType sutType)
        {
            checkIfMachineExists(sutType);
            InitNrpcClient(this.IsSecurityContext);
            string sutName = this.GetComputerNameFromType(sutType);
            _DOMAIN_NAME_BUFFER? domainNameBuffer = null;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrEnumerateTrustedDomains(sutName, out domainNameBuffer);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            VerifyNetrEnumerateTrustedDomains(domainNameBuffer, result);

            return result;
        }


        /// <summary>
        /// The DsrGetForestTrustInformation method retrieves the trust information for the forest of the specified 
        /// domain controller or for a forest trusted by the forest of the specified DC.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="trustedDomainName"> 
        /// The DNS or NetBIOS name of the trusted domain for which the forest trust information is to be gathered.
        /// </param>
        /// <param name="flags">
        /// A set of bit flags that specify additional applications for the forest trust information.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT DsrGetForestTrustInformation(
            ComputerType sutType,
            DomainNameType trustedDomainName,
            uint flags)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(trustedDomainName);

            NrpcClient nrpcProxy = null;
            this.InitNrpcClient(this.IsSecurityContext, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy = this.nrpcClient;
            }
            else
            {
                nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            TestTools.StackSdk.ActiveDirectory.Lsa._LSA_FOREST_TRUST_INFORMATION? forestTrustInfo = null;
            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcProxy.DsrGetForestTrustInformation(
                 sutName,
                 domainName,
                 (NrpcDsrGetForestTrustInformationFlags)flags,
                 out forestTrustInfo);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcProxy.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyDsrGetForestTrustInformation(sutType, domainName, result);

            return result;
        }


        /// <summary>
        /// The NetrServerGetTrustInfo method returns an information block from a specified SUT. 
        /// The information includes encrypted current and previous passwords for a particular account 
        /// and additional trust data.
        /// </summary>
        /// <param name="trustedDcType">The type of a trusted DC.</param>
        /// <param name="accountType">The client account in the domain.</param>
        /// <param name="secureChannelType">
        /// A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of 
        /// the secure channel established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using the valid Authenticator.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrServerGetTrustInfo(
            ComputerType trustedDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed)
        {
            checkIfMachineExists(trustedDcType);
            string trustDcName = this.GetComputerNameFromType(trustedDcType);
            string accountName = this.GetAccountNameFromType(accountType);
            string clientName = this.GetComputerNameFromType(clientType);

            this.InitNrpcClient(true, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());

            byte[] sessionKey = this.nrpcClient.Context.SessionKey;

            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();

            if (!isValidAuthenticatorUsed)
            {
                // Make the credential become invalid.
                clientAuthenticator.Credential.data[0] += 1;
            }

            _NETLOGON_AUTHENTICATOR? returnAuthenticator;

            _NT_OWF_PASSWORD? encryptNewOwfPassword = null;
            _NT_OWF_PASSWORD? encryptOldOwfPassword = null;
            _NL_GENERIC_RPC_DATA? trustInfo = null;

            NrpcClient nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    trustDcName);

            HRESULT result = 0;
            returnAuthenticator = null;
            try
            {
                result = (HRESULT)nrpcProxy.NetrServerGetTrustInfo(
                    trustDcName,
                    accountName,
                    secureChannelType,
                    clientName,
                    clientAuthenticator,
                    out returnAuthenticator,
                    out encryptNewOwfPassword,
                    out encryptOldOwfPassword,
                    out trustInfo);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcProxy.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();

            nrpcProxy.Dispose();

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            _NL_GENERIC_RPC_DATA[] netlogonGenericRpcData = null;
            if (trustInfo != null)
            {
                netlogonGenericRpcData = new _NL_GENERIC_RPC_DATA[] { trustInfo.Value };
            }

            if (PDCOSVersion < ServerVersion.Win2012)
            {
                VerifyNetrServerGetTrustInfo(
                    trustedDcType,
                    secureChannelType,
                    encryptNewOwfPassword,
                    encryptOldOwfPassword,
                    netlogonGenericRpcData,
                    result,
                    sessionKey);
            }

            VerifyNetlogonAuthenticator(result, returnAuthenticator);

            return result;
        }

        #endregion

        #region Message Protection Methods

        /// <summary>
        /// The NetrLogonGetTrustRid method is used to obtain the RID of the account, and the account's password is used 
        /// by domain controllers in the specified domain to establish the secure channel
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonGetTrustRid(ComputerType sutType, DomainNameType domainType)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(domainType);
            uint? rid = null;

            InitNrpcClient(this.IsSecurityContext, TransportType.TcpIp);
            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrLogonGetTrustRid(
                sutName,
                domainName,
                out rid);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            return result;
        }


        /// <summary>
        ///  The NetrLogonComputeServerDigest method computes a cryptographic digest of a message 
        ///  by using the MD5 message-digest algorithm
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="ridType">The type of RID of the machine account for which the digest is to be computed.</param>
        /// <param name="messageType">The type of message to compute the digest.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonComputeServerDigest(
            ComputerType sutType,
            RidType ridType,
            MessageType messageType)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);

            NrpcClient nrpcProxy = null;
            this.InitNrpcClient(this.IsSecurityContext, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy = this.nrpcClient;
            }
            else
            {
                nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            byte[] messageArray = Encoding.Unicode.GetBytes(MessageForDigest);
            byte[] newServerMessageDigest = null;
            byte[] oldServerMessageDigest = null;
            uint ridLocal = 0;

            switch (ridType)
            {
                case RidType.NonExist:
                    ridLocal = NotExistRid;
                    break;
                case RidType.RidOfNonDcMachineAccount:
                    // should be the rid of Client computer
                    ridLocal = (uint)EndpointRid;
                    break;
                case RidType.RidOfNonMachineAccount:
                    ridLocal = DomainAdminRid;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", ridType.ToString()));
            }

            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcProxy.NetrLogonComputeServerDigest(
                sutName,
                ridLocal,
                messageArray,
                (uint)messageArray.Length,
                out newServerMessageDigest,
                out oldServerMessageDigest);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcProxy.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            if (PDCOSVersion < ServerVersion.Win2012 )
            {
                VerifyNetrLogonComputeServerDigest(
                    ridType,
                    messageArray,
                    newServerMessageDigest,
                    oldServerMessageDigest,
                    result);
            }
            return result;
        }


        /// <summary>
        ///  The NetrLogonComputeClientDigest method is used by a client to compute a cryptographic digest 
        ///  of a message by using the MD5 message-digest algorithm.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <param name="messageType">The type of message to compute the digest.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonComputeClientDigest(
            ComputerType sutType,
            DomainNameType domainType,
            MessageType messageType)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = this.GetDomainNameFromType(domainType);

            NrpcClient nrpcProxy = null;
            this.InitNrpcClient(this.IsSecurityContext, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy = this.nrpcClient;
            }
            else
            {
                nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            byte[] messageArray = Encoding.Unicode.GetBytes(MessageForDigest);
            byte[] newClientMessageDigest = null;
            byte[] oldClientMessageDigest = null;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)nrpcProxy.NetrLogonComputeClientDigest(
                sutName,
                domainName,
                messageArray,
                (uint)messageArray.Length,
                out newClientMessageDigest,
                out oldClientMessageDigest);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcProxy.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrLogonComputeClientDigest(
                sutType,
                messageArray,
                newClientMessageDigest,
                oldClientMessageDigest,
                result);

            return result;
        }


        /// <summary>
        ///  The NetrLogonSetServiceBits method is used to notify Netlogon whether 
        ///  a domain controller is running specified services
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="serviceBitsOfInterest"> 
        ///  A set of bit flags used as a mask to indicate
        ///  which service's state (running or not running) is being set by this call.
        /// </param>
        /// <param name="serviceBits">
        ///  A set of bit flags used as a mask to indicate whether 
        ///  the service indicated by ServiceBitsOfInterest is running or not. 
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonSetServiceBits(
            ComputerType sutType,
            uint serviceBitsOfInterest,
            uint serviceBits)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrLogonSetServiceBits(
                 sutName,
                 serviceBitsOfInterest,
                 serviceBits);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            return result;
        }


        /// <summary>
        /// The NetrLogonGetTimeServiceParentDomain method returns the name of the parent domain of the current domain
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonGetTimeServiceParentDomain(ComputerType sutType)
        {
            checkIfMachineExists(sutType);
            if (null != this.nrpcClient)
            {
                this.nrpcClient.Dispose();
                this.nrpcClient = null;
            }

            string sutName = this.GetComputerNameFromType(sutType);
            string domainName = null;
            PdcSameSite_Values? pdcSameSite = null;

            // InitNrpcClient(isSecurityContext).
            this.nrpcCustomClient = new NrpcCustomClient(this.PrimaryDomainDnsName);
            NrpcCustomClientSecurityContext secuContext = null;
            if (this.IsSecurityContext)
            {
                this.nrpcCustomClient.Context.NegotiateFlags = this.GetNrpcNegotiateFlags();
                MachineAccountCredential machineCredential = new MachineAccountCredential(
                    this.PrimaryDomainDnsName,
                    this.ENDPOINTNetbiosName,
                    this.ENDPOINTPassword);
                secuContext = new NrpcCustomClientSecurityContext(
                    this.PrimaryDomainDnsName,
                    this.primaryDCNetBiosName,
                    machineCredential,
                    true,
                    this.nrpcCustomClient.Context.NegotiateFlags);
            }

            AccountCredential accountCredential = new AccountCredential(
                this.PrimaryDomainDnsName,
                DomainAdministratorName,
                this.DomainUserPassword);

            if (this.currentSutOperatingSystem == PlatformType.WindowsServer2008 && sutName.Contains("."))
            {
                sutName = sutName.Remove(sutName.IndexOf('.'));
            }
            try
            {
                this.nrpcClient.BindOverNamedPipe(
                    sutName,
                    accountCredential,
                    secuContext,
                    this.timeOut);
            }
            catch (Exception e)
            {
                Site.Log.Add(LogEntryKind.Debug, "Failed to bind NamedPipe to " + sutName + " due to reason: " + e.Message);
                Site.Assert.Fail("Failed on init NRPC client on transport NamedPipe");
            }
            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcCustomClient.NetrLogonGetTimeServiceParentDomain(
                 sutName,
                 out domainName,
                 out pdcSameSite);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                this.nrpcCustomClient.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = this.nrpcCustomClient.Context.RpceTransportContext.InterfaceId.ToString();

            this.nrpcCustomClient.Dispose();

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrLogonGetTimeServiceParentDomain(result);

            return result;
        }

        #endregion

        #region  Administrative Services Methods

        /// <summary>
        /// The NetrLogonControl2Ex method executes Windows-specific administrative actions 
        /// that pertain to the Netlogon SUT operation. 
        /// It is used to query the status and control the actions of the Netlogon SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="queryLevel">Information query level requested by the client.</param>
        /// <param name="dataType">The type of specific data required by the query.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonControl2Ex(
            ComputerType sutType,
            uint functionCode,
            uint queryLevel,
            NetlogonControlDataInformationType dataType)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);

            _NETLOGON_CONTROL_DATA_INFORMATION? controlData = this.GetControlData(functionCode, dataType);
            _NETLOGON_CONTROL_QUERY_INFORMATION? controlQueryInfomation = null;

            HRESULT result = HRESULT.ERROR_FAILURE;

            this.InitNrpcClient(true, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());

            // NRPC security context when response is received.
            NrpcCustomClientSecurityContext secuContextInResp = null;
            if (queryLevel > 4 || queryLevel < 1)
            {
                this.nrpcClient.Dispose();
                this.nrpcCustomClient = new NrpcCustomClient(this.PrimaryDomainDnsName);
                NrpcCustomClientSecurityContext secuContext = null;
                if (this.IsSecurityContext)
                {
                    this.nrpcCustomClient.Context.NegotiateFlags = this.GetNrpcNegotiateFlags();
                    MachineAccountCredential machineCredential = new MachineAccountCredential(
                        this.PrimaryDomainDnsName,
                        this.ENDPOINTNetbiosName,
                        this.ENDPOINTPassword);
                    secuContext = new NrpcCustomClientSecurityContext(
                        this.PrimaryDomainDnsName,
                        this.primaryDCNetBiosName,
                        machineCredential,
                        true,
                        this.nrpcCustomClient.Context.NegotiateFlags);
                }

                AccountCredential accountCredential = new AccountCredential(
                    this.PrimaryDomainDnsName,
                    DomainAdministratorName,
                    this.DomainUserPassword);
                if (this.currentSutOperatingSystem == PlatformType.WindowsServer2008 && sutName.Contains("."))
                {
                    sutName = sutName.Remove(sutName.IndexOf('.'));
                }

                this.nrpcCustomClient.BindOverNamedPipe(
                    sutName,
                    accountCredential,
                    secuContext,
                    this.timeOut);

                result = (HRESULT)this.nrpcCustomClient.CustomNetrLogonControl2Ex(
                    sutName,
                    functionCode,
                    queryLevel,
                    controlData,
                    out controlQueryInfomation);
                secuContextInResp = this.nrpcCustomClient.Context.RpceTransportContext.SecurityContext
                                    as NrpcCustomClientSecurityContext;
                this.nrpcCustomClient.Dispose();
            }
            else
            {
                NrpcClient nrpcProxy = null;
                nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                    this.IsSecurityContext,
                    this.GetNrpcNegotiateFlags(),
                    sutName);

                try
                {
                    result = (HRESULT)nrpcProxy.NetrLogonControl2Ex(
                            sutName,
                            (FunctionCode_Values)functionCode,
                            (QueryLevel_Values)queryLevel,
                            controlData,
                            out controlQueryInfomation);
                }
                catch (Exception e)
                {
                    Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
                }
                secuContextInResp = nrpcProxy.Context.RpceTransportContext.SecurityContext
                                    as NrpcCustomClientSecurityContext;

                if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
                {
                    nrpcProxy.Dispose();
                }
            }

            if (null != secuContextInResp)
            {
                VerifySignatureRelatedRequirement(secuContextInResp);
            }

            VerifyNetrLogonControl2Ex(
                sutType,
                (FunctionCode_Values)functionCode,
                (QueryLevel_Values)queryLevel,
                controlQueryInfomation,
                result);

            return result;
        }

        /// <summary>
        /// The NetrLogonControl2 method executes Windows-specific administrative actions 
        /// that pertain to the Netlogon SUT operation. 
        /// It is used to query the status and control the actions of the Netlogon SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving in the request.</param>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="queryLevel">Information query level requested by the client.</param>
        /// <param name="dataType">The type of specific data required by the query.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonControl2(
            ComputerType sutType,
            uint functionCode,
            uint queryLevel,
            NetlogonControlDataInformationType dataType)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);
            _NETLOGON_CONTROL_DATA_INFORMATION? controlData = null;
            _NETLOGON_CONTROL_QUERY_INFORMATION? controlQueryInfomation = null;
            controlData = this.GetControlData(functionCode, dataType);

            HRESULT result = HRESULT.ERROR_FAILURE;

            this.InitNrpcClient(this.IsSecurityContext, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());

            // NRPC context in response.
            NrpcCustomClientSecurityContext returnContext = null;

            // Interface id in response message.
            string interfaceId = null;

            if (queryLevel > 4 || queryLevel < 1)
            {
                this.nrpcClient.Dispose();
                this.nrpcCustomClient = new NrpcCustomClient(this.PrimaryDomainDnsName);
                NrpcCustomClientSecurityContext secuContext = null;
                if (this.IsSecurityContext)
                {
                    this.nrpcCustomClient.Context.NegotiateFlags = this.GetNrpcNegotiateFlags();
                    MachineAccountCredential machineCredential = new MachineAccountCredential(
                        this.PrimaryDomainDnsName,
                        this.ENDPOINTNetbiosName,
                        this.ENDPOINTPassword);
                    secuContext = new NrpcCustomClientSecurityContext(
                        this.PrimaryDomainDnsName,
                        this.primaryDCNetBiosName,
                        machineCredential,
                        true,
                        this.nrpcCustomClient.Context.NegotiateFlags);
                }

                AccountCredential accountCredential = new AccountCredential(
                    this.PrimaryDomainDnsName,
                    DomainAdministratorName,
                    this.DomainUserPassword);
                if (this.currentSutOperatingSystem == PlatformType.WindowsServer2008 && sutName.Contains("."))
                {
                    sutName = sutName.Remove(sutName.IndexOf('.'));
                }

                this.nrpcCustomClient.BindOverNamedPipe(
                    sutName,
                    accountCredential,
                    secuContext,
                    this.timeOut);

                result = (HRESULT)this.nrpcCustomClient.CustomNetrLogonControl2(
                    sutName,
                    functionCode,
                    queryLevel,
                    controlData,
                    out controlQueryInfomation);

                returnContext = this.nrpcCustomClient.Context.RpceTransportContext.SecurityContext
                                as NrpcCustomClientSecurityContext;
                interfaceId = this.nrpcCustomClient.Context.RpceTransportContext.InterfaceId.ToString();
                this.nrpcCustomClient.Dispose();
            }
            else
            {
                NrpcClient nrpcProxy = null;

                if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
                {
                    nrpcProxy = this.nrpcClient;
                }
                else
                {
                    this.nrpcClient.Dispose();
                    nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                        this.IsSecurityContext,
                        this.GetNrpcNegotiateFlags(),
                        sutName);
                }

                try
                {
                    result = (HRESULT)nrpcProxy.NetrLogonControl2(
                            sutName,
                            (FunctionCode_Values)functionCode,
                            (QueryLevel_Values)queryLevel,
                            controlData,
                            out controlQueryInfomation);
                }
                catch (Exception e)
                {
                    Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
                }
                returnContext = nrpcProxy.Context.RpceTransportContext.SecurityContext
                                as NrpcCustomClientSecurityContext;
                interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();
                if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
                {
                    nrpcProxy.Dispose();
                }
            }

            if (null != returnContext)
            {
                VerifySignatureRelatedRequirement(returnContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);
            VerifyNetrLogonControl2(
                sutType,
                (FunctionCode_Values)functionCode,
                (QueryLevel_Values)queryLevel,
                controlQueryInfomation,
                result);

            return result;
        }


        /// <summary>
        /// The NetrLogonControl method executes Windows-specific administrative actions 
        /// that pertain to the Netlogon SUT operation. 
        /// It is used to query the status and control the actions of the Netlogon SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="queryLevel">Information query level requested by the client.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonControl(
            ComputerType sutType,
            uint functionCode,
            uint queryLevel)
        {
            checkIfMachineExists(sutType);
            string sutName = this.GetComputerNameFromType(sutType);

            _NETLOGON_CONTROL_QUERY_INFORMATION? controlQueryInfomation = null;
            NrpcClient nrpcProxy = null;
            this.InitNrpcClient(false, TransportType.NamedPipe, this.GetNrpcNegotiateFlags());

            // In this NRPC call, secure context cann't be used, otherwise, ACCESS_DENIED error will be returned.
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy = this.nrpcClient;
            }
            else
            {
                nrpcProxy = this.InitNrpcClientBindOverNamedPipe(
                    false,
                    this.GetNrpcNegotiateFlags(),
                    sutName);
            }

            HRESULT result = HRESULT.E_FAIL;
            try
            {
                result = (HRESULT)nrpcProxy.NetrLogonControl(
                    sutName,
                    (FunctionCode_Values)functionCode,
                    (QueryLevel_Values)queryLevel,
                    out controlQueryInfomation);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            NrpcCustomClientSecurityContext nrpcClientSecurityContext =
                nrpcProxy.Context.RpceTransportContext.SecurityContext as NrpcCustomClientSecurityContext;

            string interfaceId = nrpcProxy.Context.RpceTransportContext.InterfaceId.ToString();

            if (!sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName))
            {
                nrpcProxy.Dispose();
            }

            if (null != nrpcClientSecurityContext)
            {
                VerifySignatureRelatedRequirement(nrpcClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(interfaceId);

            VerifyNetrLogonControl(
                sutType,
                (FunctionCode_Values)functionCode,
                (QueryLevel_Values)queryLevel,
                controlQueryInfomation,
                result);

            return result;
        }

        #endregion

        #region Obsolete Methods

        /// <summary>
        /// The NetrLogonUasLogoff method is for the support of LAN Manager products 
        /// and should be rejected with an error code.
        /// </summary>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonUasLogon()
        {
            string sutName = this.GetComputerNameFromType(ComputerType.PrimaryDc);
            string userName = this.GetAccountNameFromType(AccounterNameType.NormalDomainUserAccount);
            string workStation = this.GetComputerNameFromType(ComputerType.Client);

            this.InitNrpcClient(
                this.IsSecurityContext,
                TransportType.NamedPipe,
                (NrpcNegotiateFlags)NegotiateFlags);

            _NETLOGON_VALIDATION_UAS_INFO? validationInformation = null;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrLogonUasLogon(
                 sutName,
                 userName,
                 workStation,
                 out validationInformation);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }

            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());
            VerifyNetrLogonUasLogon(result);

            // Since it is obsolete method, the return value is always success as test cases required.
            if (result != HRESULT.ERROR_SUCCESS)
            {
                result = HRESULT.ERROR_SUCCESS;
            }

            return result;
        }


        /// <summary>
        /// The NetrAccountDeltas method was for the support of LAN Manager products 
        /// and should be rejected with an error code.
        /// </summary>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        public HRESULT NetrLogonUasLogoff()
        {
            string sutName = this.GetComputerNameFromType(ComputerType.PrimaryDc);
            string userName = this.GetAccountNameFromType(AccounterNameType.NormalDomainUserAccount);
            string workStation = this.GetComputerNameFromType(ComputerType.Client);

            this.InitNrpcClient(
               this.IsSecurityContext,
               TransportType.NamedPipe,
               (NrpcNegotiateFlags)NegotiateFlags);

            _NETLOGON_LOGOFF_UAS_INFO? logoffInformation;

            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrLogonUasLogoff(
                 sutName,
                 userName,
                 workStation,
                 out logoffInformation);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());
            VerifyNetrLogonUasLogoff(result);

            // Since it is obsolete method, the return value is always success as test cases required.
            if (result != HRESULT.ERROR_SUCCESS)
            {
                result = HRESULT.ERROR_SUCCESS;
            }

            return result;
        }


        /// <summary>
        /// The NetrAccountDeltas method was for the support of LAN Manager products 
        /// and should be rejected with an error code.
        /// </summary>
        /// <returns>The Netlogon SUT returns STATUS_NOT_IMPLEMENTED.</returns>
        public HRESULT NetrAccountDeltas()
        {
            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();

            byte[] buffer;
            uint? countReturned = null;
            uint? totalEntries = null;
            _UAS_INFO_0? nextRecordId = null;
            _UAS_INFO_0? recordID = this.nrpcClient.CreateUasInfo0(this.ENDPOINTNetbiosName, 0, 0);

            // The first 0 means set the reference parameter to zero.
            // The second 0 means set the level parameter to zero.
            // The third 0 means set BufferSize parameter to zero.
            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrAccountDeltas(
                 this.nrpcClient.Context.PrimaryName,
                 this.ENDPOINTNetbiosName,
                 clientAuthenticator,
                 ref serverAuthenticator,
                 recordID,
                 0,
                 0,
                 out buffer,
                 0,
                 out countReturned,
                 out totalEntries,
                 out nextRecordId);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());
            return result;
        }


        /// <summary>
        /// The NetrAccountSync method was for the support of LAN Manager products 
        /// and should be rejected with an error code.
        /// </summary>
        /// <returns>The Netlogon SUT returns STATUS_NOT_IMPLEMENTED.</returns>
        public HRESULT NetrAccountSync()
        {
            _NETLOGON_AUTHENTICATOR clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();
            uint? nextReference = null;
            byte[] buffer = null;
            uint? countReturned = null;
            uint? totalEntries = null;
            _UAS_INFO_0? lastRecordId = null;

            // The first 0 means set the reference parameter to zero.
            // The second 0 means set the level parameter to zero.
            // The third 0 means set BufferSize parameter to zero.
            HRESULT result = 0;
            try
            {
                result = (HRESULT)this.nrpcClient.NetrAccountSync(
                 this.nrpcClient.Context.PrimaryName,
                 this.ENDPOINTNetbiosName,
                 clientAuthenticator,
                 ref serverAuthenticator,
                 0,
                 0,
                 out buffer,
                 0,
                 out countReturned,
                 out totalEntries,
                 out nextReference,
                 out lastRecordId);
            }
            catch (Exception e)
            {
                Site.Assert.Fail(System.Reflection.MethodInfo.GetCurrentMethod().Name + " throws transport exception: " + e.Message);
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifySignatureRelatedRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                  as NrpcCustomClientSecurityContext);
            }

            VerifyTransportRelatedRequirements(this.nrpcClient.Context.RpceTransportContext.InterfaceId.ToString());

            return result;
        }

        #endregion

        #endregion

        #region Protected Override Methods

        /// <summary>
        ///  Dispose(bool disposing) executes in two distinct scenarios.  If disposing
        ///  equals true, the method has been called directly or indirectly by a user's
        ///  code. Managed and unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">
        ///  If disposing equals to false, the method has been called by the runtime from inside the finalize
        ///  and other objects should not be referenced. Only unmanaged resources can be disposed. 
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Reliability",
            "CA2001:AvoidCallingProblematicMethods",
            MessageId = "System.GC.Collect")]
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (this.nrpcClient != null)
            {
                this.nrpcClient.Dispose();
                this.nrpcClient = null;
            }

            if (this.nrpcCustomClient != null)
            {
                this.nrpcCustomClient.Dispose();
                this.nrpcCustomClient = null;
            }
            NrpcClient.CleanAll();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion

        #region Private nrpc adapter methods

        #region Init NRPC client

        /// <summary>
        /// Init NRPC client instance BindOverNamedPipe.
        /// </summary>
        /// <param name="isSecuredTransport">Whether the transport is secure or not.</param>
        /// <param name="negotiateFlags">The negotiate flags.</param>
        /// <param name="sutName">The name of the SUT to be bound.</param>
        /// <returns>NrpcClient instance</returns>
        private NrpcClient InitNrpcClientBindOverNamedPipe(
            bool isSecuredTransport,
            NrpcNegotiateFlags negotiateFlags,
            string sutName)
        {
            if (sutName.ToLower(CultureInfo.InvariantCulture).Contains(this.primaryDCNetBiosName) && null != this.nrpcClient)
            {
                this.nrpcClient.Dispose();
                this.nrpcClient = null;
            }

            NrpcClient nrpcClientBindOverNamedPipe = NrpcClient.CreateNrpcClient(this.PrimaryDomainDnsName);
            NrpcCustomClientSecurityContext secuContext = null;

            // If using secured transport, initialize securityContext with the correct credential.
            if (isSecuredTransport)
            {
                // Set the negotiate flags.
                nrpcClientBindOverNamedPipe.Context.NegotiateFlags = negotiateFlags;
                MachineAccountCredential machineCredential = new MachineAccountCredential(
                    this.PrimaryDomainDnsName,
                    this.ENDPOINTNetbiosName,
                    this.ENDPOINTPassword);
                secuContext = new NrpcCustomClientSecurityContext(
                    this.PrimaryDomainDnsName,
                    this.primaryDCNetBiosName,
                    machineCredential,
                    true,
                    nrpcClientBindOverNamedPipe.Context.NegotiateFlags);
            }

            AccountCredential accountCredential = new AccountCredential(
                this.PrimaryDomainDnsName,
                DomainAdministratorName,
                this.DomainUserPassword);

            if (this.currentSutOperatingSystem == PlatformType.WindowsServer2008
                && sutName.Contains("."))
            {
                sutName = sutName.Remove(sutName.IndexOf('.'));
            }

            try
            {
                nrpcClientBindOverNamedPipe.BindOverNamedPipe(
                    sutName,
                    accountCredential,
                    secuContext,
                    this.timeOut);
            }
            catch (ArgumentException e)
            {
                Site.Log.Add(LogEntryKind.Debug, "Failed to bind NamedPipe to " + sutName);
                Site.Assert.Fail("Failed to init NRPC client on transport NamedPipe");
                Site.Assume.Inconclusive(
                    @"This test case is not running due to binding failed.
                    The detailed error message is: {0}",
                    e.Message);
            }

            if (null != nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext)
            {
                VerifyNlAuthMessageTokenRequirement(nrpcClientBindOverNamedPipe.Context.RpceTransportContext.SecurityContext
                                                    as NrpcCustomClientSecurityContext);
            }

            return nrpcClientBindOverNamedPipe;
        }

        /// <summary>
        /// Init NRPC client instance.
        /// </summary>
        /// <param name="isSecuredTransport">The transport is secure or not.</param>
        /// <param name="transportType">The transport type.</param>
        /// <param name="negotiateFlags">The negotiate flags.</param>
        private void InitNrpcClient(
            bool isSecuredTransport,
            TransportType transportType,
            NrpcNegotiateFlags negotiateFlags)
        {
            if (this.nrpcClient != null)
            {
                this.nrpcClient.Dispose();
                this.nrpcClient = null;
            }

            this.nrpcClient = NrpcClient.CreateNrpcClient(this.PrimaryDomainDnsName);

            NrpcCustomClientSecurityContext secuContext = null;

            // If using secured transport, we need to initialize securityContext with correct credential.
            if (isSecuredTransport)
            {
                // Set the negotiate Flags.
                this.nrpcClient.Context.NegotiateFlags = negotiateFlags;

                MachineAccountCredential machineCredential = new MachineAccountCredential(
                    this.PrimaryDomainDnsName,
                    this.ENDPOINTNetbiosName,
                    this.ENDPOINTPassword);

                secuContext = new NrpcCustomClientSecurityContext(
                    this.PrimaryDomainDnsName,
                    this.primaryDCNetBiosName,
                    machineCredential,
                    ((this.nrpcClient.Context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                     == NrpcNegotiateFlags.SupportsAESAndSHA2) ? true : false,
                    this.nrpcClient.Context.NegotiateFlags);
            }

            try
            {
                // To bind the SUT with correct transport.
                switch (transportType)
                {
                    case TransportType.TcpIp:
                        ushort[] endPointList = NrpcUtility.QueryNrpcTcpEndpoint(this.primaryDCNetBiosName);

                        if (endPointList.Length == 0 || endPointList == null)
                        {
                            throw new InvalidCastException(
                                string.Format(CultureInfo.InvariantCulture, 
                                "The Primary Domain controller DNS name {0} is not recognized!",
                                this.primaryDCName.ToString()));
                        }

                        // Get the security Context.
                        this.endPoint = endPointList[0];
                        this.nrpcClient.BindOverTcp(this.primaryDCNetBiosName, this.endPoint, secuContext, this.timeOut);
                        break;

                    case TransportType.NamedPipe:
                        AccountCredential accountCredential = new AccountCredential(
                            this.PrimaryDomainDnsName,
                            DomainAdministratorName,
                            this.DomainUserPassword);
                        this.nrpcClient.BindOverNamedPipe(
                            this.primaryDCNetBiosName,
                            accountCredential,
                            secuContext,
                            this.timeOut);
                        break;

                    default:
                        throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", transportType));
                }
            }
            catch
            {
                if (secuContext != null)
                    secuContext.Dispose();
                Site.Assert.Fail("Failed to init NRPC client on transport " + transportType.ToString());
            }
            if (null != this.nrpcClient.Context.RpceTransportContext.SecurityContext)
            {
                VerifyNlAuthMessageTokenRequirement(this.nrpcClient.Context.RpceTransportContext.SecurityContext
                                                    as NrpcCustomClientSecurityContext);
            }
        }

        /// <summary>
        /// Init NRPC client instance.
        /// </summary>
        /// <param name="isSecuredTransport">The transport is secure or not.</param>
        /// <param name="transportType">The transport type.</param>
        private void InitNrpcClient(bool isSecuredTransport, TransportType transportType)
        {
            this.InitNrpcClient(isSecuredTransport, transportType, this.GetNrpcNegotiateFlags());
        }


        /// <summary>
        ///  Initialize the NRPC client.
        /// </summary>
        /// <param name="isSecContext">
        ///  Initialize the NRPC client. The value is true if the client uses security context bind.
        /// </param>
        private void InitNrpcClient(bool isSecContext)
        {
            InitNrpcClient(isSecContext, TransportType.TcpIp);
        }


        /// <summary>
        /// The method is used to get the value of NrpcNegotiateFlags.
        /// </summary>
        /// <returns>return the value of NrpcNegotiateFlags.</returns>
        private NrpcNegotiateFlags GetNrpcNegotiateFlags()
        {
            NrpcNegotiateFlags negotiateFlags = NrpcNegotiateFlags.DoesNotRequireValidationLevel2
               | NrpcNegotiateFlags.SupportsConcurrentRpcCalls
               | NrpcNegotiateFlags.SupportsCrossForestTrusts
               | NrpcNegotiateFlags.SupportsGenericPassThroughAuthentication
               | NrpcNegotiateFlags.SupportsNetrLogonGetDomainInfo
               | NrpcNegotiateFlags.SupportsNetrLogonSendToSam
               | NrpcNegotiateFlags.SupportsNetrServerPasswordSet2
               | NrpcNegotiateFlags.SupportsRC4
               | NrpcNegotiateFlags.SupportsRefusePasswordChange
               | NrpcNegotiateFlags.SupportsRodcPassThroughToDifferentDomains
               | NrpcNegotiateFlags.SupportsSecureRpc
               | NrpcNegotiateFlags.SupportsStrongKeys
               | NrpcNegotiateFlags.SupportsTransitiveTrusts;
            if (((int)getPlatform() >= ((int)PlatformType.WindowsServer2008)))
                negotiatedFlag = negotiatedFlag | (uint)NrpcNegotiateFlags.SupportsAESAndSHA2;

            return negotiateFlags;
        }

        #endregion

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// This method is used to get the platform of the targeting OS.
        /// </summary>
        /// <returns>returns a PlatformType enum indicating the OS platform</returns>
        private PlatformType getPlatform()
        {
            return PlatformType.WindowsServer2008R2;
        }

        /// <summary>
        /// Generate client session key based on negotiate flags and input sharedSecret.
        /// </summary>
        /// <param name="negotiateFlags"> The negotiate flags.</param>
        /// <param name="sharedSecret"> The shared secret.</param>
        /// <param name="credentialAlgorithm"> The credential algorithm will be used.</param>
        /// <param name="sessionKey"> The generated session key.</param>
        private void GenerateSessionKey(
            uint negotiateFlags,
            string sharedSecret,
            out NrpcComputeNetlogonCredentialAlgorithm credentialAlgorithm,
            out byte[] sessionKey)
        {
            NrpcComputeSessionKeyAlgorithm sessionKeyAlgorithm =
                            NrpcComputeSessionKeyAlgorithm.DES;
            credentialAlgorithm =
                NrpcComputeNetlogonCredentialAlgorithm.AES128;
            this.GetEncryptAlgorithmsFromNegotiateFlag(
                negotiateFlags,
                out sessionKeyAlgorithm,
                out credentialAlgorithm);

            sessionKey = new byte[16];
            bool isWeakKey = true;
            bool rebind = false;

            while (isWeakKey)
            {
                if (rebind)
                {
                    // If the DES key is weak, rebind to get generate a new key.
                    InitNrpcClient(this.IsSecurityContext);
                    _NETLOGON_CREDENTIAL? serverChallenges = null;
                    _NETLOGON_CREDENTIAL clientChallenges = new _NETLOGON_CREDENTIAL();

                    // According to the TD the length of clientChallenge.data has to be 8.
                    clientChallenges.data = NrpcUtility.GenerateNonce(ClientChallengeDataLength);

                    this.nrpcClient.NetrServerReqChallenge(
                        this.primaryDCNetBiosName,
                        this.GetComputerNameFromType(ComputerType.Client),
                        clientChallenges,
                        out serverChallenges);
                }

                sessionKey = NrpcUtility.ComputeSessionKey(
                    sessionKeyAlgorithm,
                    sharedSecret,
                    this.nrpcClient.Context.ClientChallenge,
                    this.nrpcClient.Context.ServerChallenge);

                if (NrpcComputeNetlogonCredentialAlgorithm.DESECB == credentialAlgorithm)
                {
                    byte[] key1 = ArrayUtility.SubArray(sessionKey, 0, 7);
                    byte[] key2 = ArrayUtility.SubArray(sessionKey, 7, 7);
                    isWeakKey = DES.IsWeakKey(NrpcUtility.InitLMKey(key1))
                        || DES.IsWeakKey(NrpcUtility.InitLMKey(key2))
                        || DES.IsSemiWeakKey(NrpcUtility.InitLMKey(key2))
                        || DES.IsSemiWeakKey(NrpcUtility.InitLMKey(key1));
                }
                else
                {
                    isWeakKey = false;
                }

                if (isWeakKey)
                {
                    // If the key is weak, need to rebind to get a new key.
                    rebind = true;
                }
            }
        }


        /// <summary>
        /// This method is used to determin which algorithm should be use according to the negotiate flag.
        /// </summary>
        /// <param name="negotiateFlags">Negotiate flags.</param>
        /// <param name="sessionKeyAlgorithm">Algorithm use to compute session key.</param>
        /// <param name="credentialAlgorithm">Algorithm use to compute credential.</param>
        private void GetEncryptAlgorithmsFromNegotiateFlag(
            uint negotiateFlags,
            out NrpcComputeSessionKeyAlgorithm sessionKeyAlgorithm,
            out NrpcComputeNetlogonCredentialAlgorithm credentialAlgorithm)
        {
            // If AES support is negotiated between client and SUT.
            // HMACSHA256 algorithm is choose to compute the sessionKey.
            // AES algorithm is choosed to compute the credential.

            // Else if strong Key support is negotiated between client and SUT.
            // MD5 algorithm is choosed to compute the session key.
            // DES algorithm in ECB mode is choosed to compute the credential.

            // Else strong key is not negotiated between client and SUT.
            // DES algorithm in ECB mode is choosed to compute the session key.
            // DES algorithm in ECB mode is choosed to compute the credential.
            if ((negotiateFlags & (uint)NegotiateFlagsType.W) != 0)
            {
                sessionKeyAlgorithm = NrpcComputeSessionKeyAlgorithm.HMACSHA256;
                credentialAlgorithm = NrpcComputeNetlogonCredentialAlgorithm.AES128;
            }
            else if ((negotiateFlags & (uint)NegotiateFlagsType.O) != 0)
            {
                sessionKeyAlgorithm = NrpcComputeSessionKeyAlgorithm.MD5;
                credentialAlgorithm = NrpcComputeNetlogonCredentialAlgorithm.DESECB;
            }
            else
            {
                sessionKeyAlgorithm = NrpcComputeSessionKeyAlgorithm.DES;
                credentialAlgorithm = NrpcComputeNetlogonCredentialAlgorithm.DESECB;
            }
        }

        /// <summary>
        ///This method is used to get the specified site name from the SiteNameType.
        /// </summary>
        /// <param name="siteNameType">A SiteNameType Enum which specify site guid type.</param>
        /// <returns>specified site name</returns>
        private string GetSiteNameFromType(SiteNameType siteNameType)
        {
            string siteName = null;
            switch (siteNameType)
            {
                case SiteNameType.Null:
                    siteName = null;
                    break;
                case SiteNameType.NonExistSiteName:
                    siteName = NonExistSiteName;
                    break;
                case SiteNameType.SiteNameOne:
                    siteName = defaultSiteName;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", siteNameType.ToString()));
            }

            return siteName;
        }


        /// <summary>
        /// This method is used to get the specified dsa guid from the DsaGuidType.
        /// </summary>
        /// <param name="dsaGuidType">A DsaGuidType enum which specify dsa guid type, the value can be set null.</param>
        /// <param name="dsaGuid">the guid store the specify dsa guid</param>
        private void GetDsaGuidFromType(DsaGuidType dsaGuidType, out Guid? dsaGuid)
        {
            dsaGuid = null;
            switch (dsaGuidType)
            {
                case DsaGuidType.DsaGuidOne:
                    dsaGuid = PdcDsaGuid;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", dsaGuidType.ToString()));
            }
        }


        /// <summary>
        /// This method is used to get the SocketAddress.
        /// </summary>
        /// <param name="socketAddresses">A list of socket addresses.</param>
        /// <returns>return an instance of a _NL_SOCKET_ADDRESS structure.</returns>
        private _NL_SOCKET_ADDRESS[] GetSocketAddressArray(Set<SocketAddressType> socketAddresses)
        {
            string ipaddresses = InValidIpAddrs;
            _NL_SOCKET_ADDRESS[] socketAddressList = null;

            SocketAddressType[] socketAddresstype = socketAddresses.ToArray();


            string primarydcBiosName = this.PDCNetbiosName;

            // Retrieve NRPC dynamic TCP endpoint of a SUT.
            ushort[] endPointList = NrpcUtility.QueryNrpcTcpEndpoint(primarydcBiosName);
            switch (socketAddresstype[0])
            {
                case SocketAddressType.Ipv4SocketAddress:
                    ipaddresses = this.PDCIPAddress;
                    break;
                case SocketAddressType.Ipv6SocketAddress:
                    ipaddresses = Site.Properties["MS_NRPC.SUT.Primary.ValidAddrs.IP.V6"];
                    break;
                default:
                    break;
            }

            int portNumber = endPointList[0];
            IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Parse(ipaddresses), portNumber);
            _NL_SOCKET_ADDRESS socketAddress = this.nrpcClient.CreateNlSocketAddress(ipendPoint);

            if (socketAddress.iSockaddrLength == 0)
            {
                throw new InvalidCastException(
                      string.Format(CultureInfo.InvariantCulture, "The Ip EndPoint {0} is not recognized!", ipendPoint.ToString()));
            }
            else
            {
                socketAddressList = new _NL_SOCKET_ADDRESS[] { socketAddress };
            }

            return socketAddressList;
        }


        /// <summary>
        ///This method is used to get the specified site guid from the SiteGuidType.
        /// </summary>
        /// <param name="siteGuidType">
        /// A SiteGuidType Enum which specify site guid type, the value can be set null.
        /// </param>
        /// <param name="siteGuid">the guid store the specify site guid.</param>
        private void GetSiteGuidFromType(SiteGuidType siteGuidType, out Guid? siteGuid)
        {
            siteGuid = null;
            switch (siteGuidType)
            {
                case SiteGuidType.Null:
                    siteGuid = null;
                    break;
                case SiteGuidType.NonNull:
                    siteGuid = SiteGuid;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", siteGuidType.ToString()));
            }
        }


        /// <summary>
        ///This method is used to get the specifed domain guid from the DomainGuidType.
        /// </summary>
        /// <param name="domainGuidType">A DomainGuidType Enum which specify domain guid type.</param>
        /// <param name="domainGuid">the guid store the specify domain guid. The value can be set null.</param>
        private void GetDomainGuidFromType(DomainGuidType domainGuidType, out Guid? domainGuid)
        {
            domainGuid = null;
            switch (domainGuidType)
            {
                case DomainGuidType.Null:
                    domainGuid = null;
                    break;
                case DomainGuidType.NonExistDomainGuid:
                    domainGuid = NonExistDomainGuid;
                    break;
                case DomainGuidType.PrimaryDomainGuid:
                    domainGuid = new Guid(Site.Properties["Common.PrimaryDomain.ServerGUID"]);
                    break;
                case DomainGuidType.TrustedDomainGuid:
                    domainGuid = TrustDomainGuid;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", domainGuidType.ToString()));
            }
        }


        /// <summary>
        ///This method is used to get the specified domain name from the DomainNameType.
        /// </summary>
        /// <param name="domainType">A DomainNameType Enum which specify domain name type.</param>
        /// <returns>The specified domain name</returns>
        private string GetDomainNameFromType(DomainNameType domainType)
        {
            string domainName;

            switch (domainType)
            {
                case DomainNameType.Null:
                    domainName = null;
                    break;
                case DomainNameType.TrustedDomainName:
                    domainName = this.TrustDomainDnsName;
                    break;
                case DomainNameType.NetBiosFormatDomainName:
                    domainName = this.primaryDomainNetBiosName;
                    break;
                case DomainNameType.InvalidFormatDomainName:
                    domainName = InvalidFormatDomainName;
                    break;
                case DomainNameType.NonExistDomainName:
                    domainName = NonExistDomainName;
                    break;
                case DomainNameType.FqdnFormatDomainName:
                    domainName = this.PrimaryDomainDnsName;
                    break;
                case DomainNameType.EmptyDomainName:
                    domainName = string.Empty;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", domainType.ToString()));
            }

            return domainName;
        }


        /// <summary>
        /// This method is used to get the specified computer name from the ComputerNameType.
        /// </summary>
        /// <param name="computerType">The type of computer which is used.</param>
        /// <returns>The specified computer name.</returns>
        private string GetComputerNameFromType(ComputerType computerType)
        {
            string computerName;

            switch (computerType)
            {
                case ComputerType.Null:
                    computerName = null;
                    break;
                case ComputerType.PrimaryDc:
                    computerName = this.PDCNetbiosName + "." + this.PrimaryDomainDnsName;
                    break;
                case ComputerType.NonExistComputer:
                    computerName = NonExistComputerName;
                    break;
                case ComputerType.NonDcServer:
                    computerName = this.DMNetbiosName;
                    break;
                case ComputerType.Client:
                    computerName = this.ENDPOINTNetbiosName;
                    break;
                case ComputerType.TrustDc:
                    computerName = this.trustDCName;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", computerType.ToString()));
            }

            return computerName;
        }


        /// <summary>
        /// This method is used to get the specified account name from the AccounterNameType.
        /// </summary>
        /// <param name="accountType">An AccounterNameType Enum which specify account name type.</param>
        /// <returns>The specified account name.</returns>
        private string GetAccountNameFromType(AccounterNameType accountType)
        {
            string accountName;

            switch (accountType)
            {
                case AccounterNameType.AnotherDomainUserAccount:
                    accountName = TrustDomainUserAccount;
                    break;
                case AccounterNameType.DomainMemberComputerAccount:
                    accountName = this.ENDPOINTNetbiosName;
                    break;
                case AccounterNameType.DomainMemberComputerAccountEndWithPeriod:
                    accountName = this.ENDPOINTNetbiosName + ".";
                    break;
                case AccounterNameType.InvalidAccount:
                    accountName = InvalidUser;
                    break;
                case AccounterNameType.NormalDomainUserAccount:
                    accountName = this.DomainAdministratorName;
                    break;
                case AccounterNameType.Null:
                    accountName = null;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format(CultureInfo.InvariantCulture, "The type {0} is not recognized!", accountType.ToString()));
            }

            if (this.PDCIsWindows
                && (accountType == AccounterNameType.BdcComputerAccount
                    || accountType == AccounterNameType.DomainMemberComputerAccount
                    || accountType == AccounterNameType.RodcComputerAccount))
            {
                // In Windows, all machine account names are the name of the machine with a "$" (dollar sign) appended.
                accountName += "$";
            }

            return accountName;
        }


        /// <summary>
        /// Check if a string matches the format of DNS name.
        /// </summary>
        /// <param name="name">Any string.</param>
        /// <returns>True if name is a DNS name.</returns>
        private bool HelperIsDnsName(string name)
        {
            bool isDnsName = true;
            /* RFC1035(section 3.1)
             * Domain names in messages are expressed in terms of a sequence of labels.
             * Each label is represented as a one octet length field followed by that
             * number of octets.  Since every domain name ends with the null label of
             * the root, a domain name is terminated by a length byte of zero. The
             * high order two bits of every length octet must be zero, and the
             * remaining six bits of the length field limit the label to 63 octets or
             * less.
             * 
             * To simplify implementations, the total length of a domain name (i.e.,
             * label octets and label length octets) is restricted to 255 octets or
             * less.
             * 
             * Although labels can contain any 8 bit values in octets that make up a
             * label, it is strongly recommended that labels follow the preferred
             * syntax described elsewhere in this memo, which is compatible with
             * existing host naming conventions.  Name servers and resolvers must
             * compare labels in a case-insensitive manner (i.e., A=a), assuming ASCII
             * with zero parity.  Non-alphabetic codes must match exactly.
             */

            string[] dnsLabels = name.Split('.');
            int wholeLength = 0;

            if (dnsLabels.Length < 2)
            {
                isDnsName = false;
                return isDnsName;
            }

            foreach (string label in dnsLabels)
            {
                if (label.Length == 0 || label.Length >= 64)
                {
                    isDnsName = false;
                    return isDnsName;
                }

                wholeLength += label.Length;
            }

            if (wholeLength == 0 || wholeLength >= 256)
            {
                isDnsName = false;
                return isDnsName;
            }

            return isDnsName;
        }


        /// <summary>
        /// Check if a byte array is a zero byte array.
        /// </summary>
        /// <param name="array">Any byte array.</param>
        /// <returns>Return true if a byte array is a zero byte array.</returns>
        private bool HelperIsZeroByteArray(byte[] array)
        {
            foreach (byte b in array)
            {
                if (b != (byte)0)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Compare elements in same index in each array.
        /// </summary>
        /// <param name="first">The first array to be compared.</param>
        /// <param name="second">The second array to be compared.</param>
        /// <returns>Returns true if all elements in same index are equal, false otherwise.</returns>
        private bool ElementsEqual(Array first, Array second)
        {
            // Return false if element count doesn't equal.
            if (first.Length != second.Length)
            {
                return false;
            }

            for (int i = 0; i < first.Length; ++i)
            {
                // If two of the elements don't match.
                if (!first.GetValue(i).Equals(second.GetValue(i)))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Validate rawData, to see whether it can be decoded as
        /// MULTI-SZ format. This method checks the following rules:
        /// 1. MULTI-SZ format is a Unicode, UTF-16 string. This means
        ///    this method checks two bytes at a time.
        /// 2. Each substring is separated from adjacent substrings by the UTF-16
        ///    null character, 0x0000.
        /// 3. After the final substring, the MULTI-SZ format string is terminated
        ///    by two UTF-16 null characters.
        /// </summary>
        /// <param name="rawData">The data to be validated.</param>
        /// <returns>True if rawData is of MULTI-SZ format, false otherwise.</returns>
        private bool IsMultiSzFormat(byte[] rawData)
        {
            // The end char in data.
            const char EndChar = '\0';

            // Is the UTF-16 null character (0x0000) found in rawData?
            bool isNullFound = false;
            int length = rawData.Length;

            for (int i = 0; i < length; i += 2)
            {
                // This method checks two bytes at a time,
                // i is the first byte, (i+1) is the second byte.
                if ((i + 1) >= length)
                {
                    // Rule #1: If the second byte doesn't exist, this is
                    // not a Unicode format.
                    return false;
                }

                char character = (char)((rawData[i] << 8) | rawData[i + 1]);

                if (character == EndChar)
                {
                    isNullFound = true;
                }
            }

            // Rule #2
            if (!isNullFound)
            {
                return false;
            }

            // The last two UTF-16 null characters must exist,
            // Two UTF-16 null characters takes four bytes.
            Site.Assert.IsTrue(length >= 4, "Verify MULTI-SZ format: The last two UTF-16 null characters must exist");

            // (length-2) and (length-1) constructs the last UTF-16 null character.
            char lastChar = (char)((rawData[length - 2] << 8) | rawData[length - 1]);

            // (length-4) and (length-3) constructs the last but one UTF-16 null character.
            char lastButOneChar = (char)((rawData[length - 4] << 8) | rawData[length - 3]);

            // Rule #3
            return (EndChar == lastButOneChar) && (EndChar == lastChar);
        }


        /// <summary>
        /// Convert the _RPC_UNICODE_STRING to string.
        /// </summary>
        /// <param name="rpcUnicodeString">Input _RPC_UNICODE_STRING.</param>
        /// <returns>The returned string.</returns>
        private string GetRpcUnicodeString(_RPC_UNICODE_STRING rpcUnicodeString)
        {
            string str = string.Empty;

            if (rpcUnicodeString.Buffer == null)
            {
                return null;
            }

            foreach (ushort ch in rpcUnicodeString.Buffer)
            {
                str += ((char)ch).ToString();
            }

            return str;
        }


        /// <summary>
        /// Convert the OLD_LARGE_INTEGER Structure to Int64.
        /// </summary>
        /// <param name="oldLargeInteger">Input OLD_LARGE_INTEGER value.</param>
        /// <returns>The returned Int64 value.</returns>
        private long GetInt64ValueFromOldLargeInteger(_OLD_LARGE_INTEGER oldLargeInteger)
        {
            long actualIntValue = 0;

            // Get high part byte.
            byte[] highPartBytes = BitConverter.GetBytes(oldLargeInteger.HighPart);

            // Copy high part value to the high 4 byte of 8 byte array.
            byte[] tmp = new byte[8];
            highPartBytes.CopyTo(tmp, 4);

            // Get the Int64 value.
            actualIntValue = BitConverter.ToInt64(tmp, 0) + oldLargeInteger.LowPart;

            return actualIntValue;
        }

        /// <summary>
        /// Get string value of the given _RPC_SID structure
        /// </summary>
        /// <param name="rpcSid"></param>
        /// <returns></returns>
        private string GetStringFromRpcSid(_RPC_SID rpcSid)
        {
            // The ObjectSid started with 'S'.
            string strSid = "S";

            // The separator between the two pars in Sid.
            string strDash = "-";

            strSid += strDash + rpcSid.Revision.ToString(CultureInfo.InvariantCulture);
            strSid += strDash + rpcSid.IdentifierAuthority.Value[5].ToString(CultureInfo.InvariantCulture);

            foreach (uint u in rpcSid.SubAuthority)
            {
                strSid += strDash + u.ToString(CultureInfo.InvariantCulture);
            }
            return strSid;
        }


        /// <summary>
        /// This method removes the leading backslashes of
        /// given string, if it exists.
        /// </summary>
        /// <param name="name">Input name.</param>
        /// <returns>Name without leading backslashes.</returns>
        private string RemoveLeadingBackslash(string name)
        {
            // If name starts with two backslashes, the third character
            // starts at index 2, otherwise return name directly.
            return name.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase) ? name.Substring(2) : name;
        }

        #region Decryption and Encryption Helper Methods

        /// <summary>
        /// This method is used to construct an encrypted _NT_OWF_PASSWORD from plain _NT_OWF_PASSWORD.
        /// </summary>
        /// <param name="password">Input password.</param>
        /// <param name="key">Session key used for encrypt the password.</param>
        /// <returns>_NT_OWF_PASSWORD structure.</returns>
        private _NT_OWF_PASSWORD GetEncryptedNtOwfPasswordStructure(string password, byte[] key)
        {
            const int DataSize = 2;
            int index = 0;

            byte[] ntlmOwfV1 = NlmpUtility.NtOWF(NlmpVersion.v1, null, null, password);
            _ENCRYPTED_LM_OWF_PASSWORD samrEncryptedLmOwfPassword = SamrCryptography.EncryptBlockWithKey(ntlmOwfV1, key);
            _LM_OWF_PASSWORD encryptedLmOwfPwd = new _LM_OWF_PASSWORD();
            encryptedLmOwfPwd.data = NrpcUtility.CreateCypherBlocks(samrEncryptedLmOwfPassword.data);
            _NT_OWF_PASSWORD nt = new _NT_OWF_PASSWORD();
            nt.data = new _CYPHER_BLOCK[DataSize];

            nt.data[index].data = encryptedLmOwfPwd.data[index].data;
            index++;
            nt.data[index].data = encryptedLmOwfPwd.data[index].data;

            return nt;
        }


        /// <summary>
        /// This method is used to verify two OWF structures are equal.
        /// </summary>
        /// <param name="expectedNtOwfPassword">The expected NT_OWF_PASSWORD value.</param>
        /// <param name="actualNtOwfPassword">
        /// The actual NT_OWF_PASSWORD value which should be equal to the expected NT_OWF_PASSWORD value.
        /// </param>
        /// <returns>True indicates equals, otherwise NOT equal.</returns>
        private bool VerifyOwfStructureEqual(
            _NT_OWF_PASSWORD? expectedNtOwfPassword,
            _NT_OWF_PASSWORD? actualNtOwfPassword)
        {
            if (expectedNtOwfPassword.Value.data.Length != actualNtOwfPassword.Value.data.Length)
            {
                return false;
            }

            for (int index = 0; index < expectedNtOwfPassword.Value.data.Length; index++)
            {
                // expectedNtOwfPassword.Value.data is the type "_CYPHER_BLOCK[]"
                // _CYPHER_BLOCK.data: An encrypted eight-character string, byte[8].
                // Convert the eight bytes to UInt64 at the started index 0 of _CYPHER_BLOCK.data.
                if (BitConverter.ToUInt64(expectedNtOwfPassword.Value.data[index].data, 0)
                    != BitConverter.ToUInt64(actualNtOwfPassword.Value.data[index].data, 0))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region IsBufferCorrectForTrustedDomain

        /// <summary>
        /// Judge whether the gotten buffer contains Flags ParentIndex, TrustType, TrustAttributes of the trusted domain
        /// </summary>
        /// <param name="buffer">The gotten buffer data of NETLOGON_ONE_DOMAIN_INFO.TrustExtension structure.</param>
        /// <param name="flags">Flags of trusted domain.</param>
        /// <param name="parentIndex">ParentIndex of trusted domain.</param>
        /// <param name="trustType">TrustType of trusted domain.</param>
        /// <param name="trustAttributes">TrustAttributes of trusted domain.</param>
        /// <returns>The returned bool.</returns>
        private bool IsBufferCorrectForTrustedDomain(
            ushort[] buffer,
            uint flags,
            uint parentIndex,
            uint trustType,
            uint trustAttributes)
        {
            bool result = true;
            List<uint> temp = new List<uint>();

            temp.Add(flags);
            temp.Add(parentIndex);
            temp.Add(trustType);
            temp.Add(trustAttributes);

            byte[] bytesInTemp = MarshalHelper.GetBytes(temp.ToArray(), false);
            byte[] bytesInBuffer = MarshalHelper.GetBytes(buffer, false);
            for (int i = 0; i < bytesInBuffer.Length; i = i + 4)
            {
                for (int j = i; j < i + 2; j++)
                {
                    byte save = bytesInBuffer[j];
                    bytesInBuffer[j] = bytesInBuffer[j + 2];
                    bytesInBuffer[j + 2] = save;
                }
            }

            if (bytesInTemp.Length != bytesInBuffer.Length)
            {
                result = false;
                return result;
            }

            for (int i = 0; i < bytesInTemp.Length; i++)
            {
                if (bytesInTemp[i] != bytesInBuffer[i])
                {
                    result = false;
                    return result;
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// This method gets encrpyted logon level by given _NETLOGON_LOGON_INFO_CLASS.
        /// </summary>
        /// <param name="logonLevelInfoClass">Logon level info class.</param>
        /// <returns>The encrypted logon level.</returns>
        private _NETLOGON_LEVEL GetEncryptedLogonLevel(_NETLOGON_LOGON_INFO_CLASS logonLevelInfoClass)
        {
            _NETLOGON_LEVEL encryptedLogonLevel;

            // Client calls CreateNetlogonLevel.
            _NETLOGON_LEVEL logonLevel = this.nrpcClient.CreateNetlogonLevel(
                logonLevelInfoClass,
                NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                this.primaryDomainNetBiosName,
                DomainAdministratorName,
                this.DomainUserPassword);

            // Client calls EncryptNetlogonLevel.
            encryptedLogonLevel = this.nrpcClient.EncryptNetlogonLevel(
               logonLevelInfoClass,
               logonLevel);

            return encryptedLogonLevel;
        }


        /// <summary>
        /// Construct logonData parameter for NetlogonGenericInformation.
        /// </summary>
        /// <param name="logonDataString">LogonData string from ptfconfig file.</param>
        /// <returns>Byte array of logonData.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private byte[] ConstructGenericLogonData(string logonDataString)
        {
            string[] strDataArry = logonDataString.Split(',');
            byte[] logonData = new byte[strDataArry.Length];

            for (int i = 0; i < strDataArry.Length; i++)
            {
                logonData[i] = byte.Parse(strDataArry[i].Trim(), CultureInfo.InvariantCulture);
            }

            return logonData;
        }


        /// <summary>
        /// Get control data information by given function code and data type.
        /// </summary>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="dataType">Contains specific data required by the query.</param>
        /// <returns>Control data information.</returns>
        private _NETLOGON_CONTROL_DATA_INFORMATION? GetControlData(
            uint functionCode,
            NetlogonControlDataInformationType dataType)
        {
            _NETLOGON_CONTROL_DATA_INFORMATION? controlData = null;

            switch (dataType)
            {
                case NetlogonControlDataInformationType.Null:
                    break;
                case NetlogonControlDataInformationType.NoValidDomainNameContained:
                    controlData = this.GenerateControlDataInfo(
                        (FunctionCode_Values)functionCode,
                        NonExistDomainName,
                        this.trustDomainUserName,
                        DebugFlag);
                    break;
                case NetlogonControlDataInformationType.Valid:
                    controlData = this.GenerateControlDataInfo(
                        (FunctionCode_Values)functionCode,
                        this.TrustDomainDnsName,
                        this.trustDomainUserName,
                        DebugFlag);
                    break;
                default:
                    throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, 
                        "NetlogonControlDataInformationType {0} is not recognized!",
                        dataType.ToString()));
            }

            if ((FunctionCode_Values)functionCode != FunctionCode_Values.NETLOGON_CONTROL_REDISCOVER
                && (FunctionCode_Values)functionCode != FunctionCode_Values.NETLOGON_CONTROL_TC_QUERY
                && (FunctionCode_Values)functionCode != FunctionCode_Values.NETLOGON_CONTROL_CHANGE_PASSWORD
                && (FunctionCode_Values)functionCode != FunctionCode_Values.NETLOGON_CONTROL_TC_VERIFY
                && (FunctionCode_Values)functionCode != FunctionCode_Values.NETLOGON_CONTROL_SET_DBFLAG
                && (FunctionCode_Values)functionCode != FunctionCode_Values.NETLOGON_CONTROL_FIND_USER)
            {
                controlData = null;
            }

            return controlData;
        }


        /// <summary>
        /// This method is used to generate a _NETLOGON_CONTROL_DATA_INFORMATION structure
        /// for the Administrative Services Methods.
        /// </summary>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="trustedDomainName">
        /// A pointer to a null-terminated Unicode string that contains a trusted domain name.
        /// Switched on the DWORD values 0x00000005, 0x00000006, 0x00000009, and 0x0000000A.
        /// </param>
        /// <param name="userName">
        /// A pointer to null-terminated Unicode string that contains a user name.
        /// Switched on the DWORD value 0x00000008.
        /// </param>
        /// <param name="debugFlag">
        /// A DWORD that contains an implementation-specific debug flag.
        /// Switched on the value 0x0000FFFE.
        /// </param>
        /// <returns>_NETLOGON_CONTROL_DATA_INFORMATION structure.</returns>
        private _NETLOGON_CONTROL_DATA_INFORMATION GenerateControlDataInfo(
            FunctionCode_Values functionCode,
            string trustedDomainName,
            string userName,
            uint debugFlag)
        {
            _NETLOGON_CONTROL_DATA_INFORMATION controlData = new _NETLOGON_CONTROL_DATA_INFORMATION();

            switch (functionCode)
            {
                // [case(5,6,9,10)]
                // [string] wchar_t* TrustedDomainName.
                // TrustedDomainName: A pointer to a null-terminated Unicode string that contains a trusted domain name.
                // Switched on the DWORD values 0x00000005, 0x00000006, 0x00000009, and 0x0000000A.
                case FunctionCode_Values.NETLOGON_CONTROL_REDISCOVER:
                case FunctionCode_Values.NETLOGON_CONTROL_TC_QUERY:
                case FunctionCode_Values.NETLOGON_CONTROL_CHANGE_PASSWORD:
                case FunctionCode_Values.NETLOGON_CONTROL_TC_VERIFY:
                    controlData.TrustedDomainName = trustedDomainName;
                    break;

                // [case(65534)]
                // DWORD DebugFlag.
                // DebugFlag: A DWORD that contains an implementation-specific debug flag.
                // Switched on the value 0x0000FFFE.
                case FunctionCode_Values.NETLOGON_CONTROL_SET_DBFLAG:
                    controlData.DebugFlag = debugFlag;
                    break;

                // [case(8)]
                // [string] wchar_t* UserName.
                // UserName: A pointer to null-terminated Unicode string that contains a user name.
                // Switched on the DWORD value 0x00000008.
                case FunctionCode_Values.NETLOGON_CONTROL_FIND_USER:
                    controlData.UserName = userName;
                    break;
                default:
                    break;
            }

            return controlData;
        }


        /// <summary>
        /// Convert an array to a string which contains
        /// all array element values.
        /// </summary>
        /// <param name="array">The array to be converted.</param>
        /// <returns>The string of array elements.</returns>
        private string ArrayToString(Array array)
        {
            if (array == null)
            {
                return "(null)";
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < array.Length; ++i)
            {
                builder.Append(array.GetValue(i).ToString());

                // Append element spliter: " ".
                builder.Append(" ");
            }

            return builder.ToString();
        }


        /// <summary>
        /// This method is used to compute the message digest for the message protection methods.
        /// </summary>
        /// <param name="password">Shared secret between client and domain.</param>
        /// <param name="message">Message in bytes.</param>
        /// <returns>Message digest.</returns>
        private byte[] GetMessageDigest(string password, byte[] message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] messageForSS =
                Microsoft.Protocols.TestTools.StackSdk.Security.Samr.SamrCryptography.GetHashWithNTOWFv1(
                password);
            MD5CryptoServiceProvider messageDigestAlgorithm5Service = new MD5CryptoServiceProvider();
            byte[] messageDigestAlgorithm5Context = new byte[messageForSS.Length + message.Length];

            // Copy all bytes from messageForSS starting at the index 0 to messageDigestAlgorithm5Context starting at 
            // the index 0.
            Buffer.BlockCopy(messageForSS, 0, messageDigestAlgorithm5Context, 0, messageForSS.Length);

            // Append all bytes from message starting at the index 0 to messageDigestAlgorithm5Context.
            Buffer.BlockCopy(message, 0, messageDigestAlgorithm5Context, messageForSS.Length, message.Length);
            return messageDigestAlgorithm5Service.ComputeHash(messageDigestAlgorithm5Context);
        }


        /// <summary>
        /// Splits a given block into two blocks with specified block size.
        /// </summary>
        /// <param name="block">The array to be splitted.</param>
        /// <param name="firstBlockSize">Size of firstBlock.</param>
        /// <param name="secondBlockSize">Size of secondBlock.</param>
        /// <param name="firstBlock">The splitter first block, starts at index 0.</param>
        /// <param name="secondBlock">The splitter second block, starts right after firstBlock.</param>
        private void SplitBlock(
            byte[] block,
            int firstBlockSize,
            int secondBlockSize,
            out byte[] firstBlock,
            out byte[] secondBlock)
        {
            if (block == null)
            {
                throw new ArgumentNullException("block");
            }

            if (block.Length < firstBlockSize + secondBlockSize)
            {
                throw new ArgumentException("The size of the block is less than block1Size + block2Size!");
            }

            firstBlock = new byte[firstBlockSize];
            secondBlock = new byte[secondBlockSize];

            // Copy firstBlockSize bytes to firstBlock.
            Array.Copy(block, 0, firstBlock, 0, firstBlock.Length);

            // Copy the consecutive secondBlockSize bytes to secondBlock.
            Array.Copy(block, firstBlock.Length, secondBlock, 0, secondBlock.Length);
        }


        /// <summary>
        /// Transforms a underivedKeySize-byte key to a 8-byte key.
        /// </summary>
        /// <param name="inputKey">The input underivedKeySize-byte key.</param>
        /// <returns>The derived 8-byte key.</returns>
        private byte[] TransformKey(byte[] inputKey)
        {
            if (inputKey == null)
            {
                throw new ArgumentNullException("inputKey");
            }

            // The bit mask whose least-significant bit is 0.
            const int BitMask = 0xFE;

            // The following transform algorithm is defined in MS-SAMR TD Section 2.2.11.1.2.
            byte[] outputKey = new byte[8];
            outputKey[0] = (byte)(inputKey[0] >> 0x01);
            outputKey[1] = (byte)(((inputKey[0] & 0x01) << 6) | (inputKey[1] >> 2));
            outputKey[2] = (byte)(((inputKey[1] & 0x03) << 5) | (inputKey[2] >> 3));
            outputKey[3] = (byte)(((inputKey[2] & 0x07) << 4) | (inputKey[3] >> 4));
            outputKey[4] = (byte)(((inputKey[3] & 0x0F) << 3) | (inputKey[4] >> 5));
            outputKey[5] = (byte)(((inputKey[4] & 0x1F) << 2) | (inputKey[5] >> 6));
            outputKey[6] = (byte)(((inputKey[5] & 0x3F) << 1) | (inputKey[6] >> 7));
            outputKey[7] = (byte)(inputKey[6] & 0x7F);

            // The inputKey is expanded to 8 bytes by inserting a 0-bit after every 
            // seventh bit.
            for (int i = 0; i < outputKey.Length; i++)
            {
                outputKey[i] = (byte)((outputKey[i] << 1) & BitMask);
            }

            return outputKey;
        }


        /// <summary>
        /// The method is used to decrypt plain Nt hash from the cipher OWFv1 structure.
        /// </summary>
        /// <param name="cipherOwf">Encrypted OWF structure.</param>
        /// <param name="sessionKey">Session Key.</param>
        /// <returns>Plain Nt Hash.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Maintainability",
            "CA1500:VariableNamesShouldNotMatchFieldNames",
            MessageId = "sessionKey")]
        private byte[] GetPlainNtHashFromEncryptedOwfPassword(_LM_OWF_PASSWORD cipherOwf, byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }

            // The block size for DES ECB encryption.
            const int SamrEncryptionBlockSize = 8;

            // Underived key size.
            const int UnderivedKeySize = 7;

            // Used to store plain NT hash.
            List<byte> block = new List<byte>();

            byte[] firstKey = null;
            byte[] secondKey = null;

            // Split keys into 2 underivedKeySize size blocks.
            this.SplitBlock(sessionKey, UnderivedKeySize, UnderivedKeySize, out firstKey, out secondKey);

            // Derive keys.
            byte[] firstTransformedKey = this.TransformKey(firstKey);
            byte[] secondTransformedKey = this.TransformKey(secondKey);

            // Do Decryption.
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Key = firstTransformedKey;
            des.Padding = PaddingMode.Zeros;

            ICryptoTransform desDecryptor = des.CreateDecryptor();
            byte[] firstPlainBlock = desDecryptor.TransformFinalBlock(
                cipherOwf.data[0].data, 0, SamrEncryptionBlockSize);
            des.Key = secondTransformedKey;
            desDecryptor = des.CreateDecryptor();
            byte[] secondPlainBlock = desDecryptor.TransformFinalBlock(
                cipherOwf.data[1].data, 0, SamrEncryptionBlockSize);

            block.AddRange(firstPlainBlock);
            block.AddRange(secondPlainBlock);

            return block.ToArray();
        }


        /// <summary>
        /// The method is used to decrypt plain Nt hash from the cipher OWFv1 structure.
        /// </summary>
        /// <param name="cipherOwf">Encrypted OWF structure.</param>
        /// <param name="sessionKey">Session Key.</param>
        /// <returns>Plain Nt Hash.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Maintainability",
            "CA1500:VariableNamesShouldNotMatchFieldNames",
            MessageId = "sessionKey")]
        private byte[] GetPlainNtHashFromEncryptedOwfPassword(_NT_OWF_PASSWORD cipherOwf, byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }

            // The block size for DES ECB encryption.
            const int SamrEncryptionBlockSize = 8;

            // Underived key size.
            const int UnderivedKeySize = 7;

            // Used to store plain NT hash.
            List<byte> block = new List<byte>();

            byte[] firstKey = null;
            byte[] secondKey = null;

            // Split keys into 2 underivedKeySize size blocks.
            this.SplitBlock(sessionKey, UnderivedKeySize, UnderivedKeySize, out firstKey, out secondKey);

            // Derive keys.
            byte[] firstTransformedKey = this.TransformKey(firstKey);
            byte[] secondTransformedKey = this.TransformKey(secondKey);

            // Do Decryption.
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Key = firstTransformedKey;
            des.Padding = PaddingMode.Zeros;

            ICryptoTransform desDecryptor = des.CreateDecryptor();
            byte[] firstPlainBlock = desDecryptor.TransformFinalBlock(
                cipherOwf.data[0].data, 0, SamrEncryptionBlockSize);
            des.Key = secondTransformedKey;
            desDecryptor = des.CreateDecryptor();
            byte[] secondPlainBlock = desDecryptor.TransformFinalBlock(
                cipherOwf.data[1].data, 0, SamrEncryptionBlockSize);

            block.AddRange(firstPlainBlock);
            block.AddRange(secondPlainBlock);

            return block.ToArray();
        }

        #endregion
        private bool ChangedNetlogonService
        {
            get
            {
                return System.IO.File.Exists(@"c:\temp\changednetlogonservicestatus.txt");
            }
        }

        private bool ChangedNonDcAccountStatus
        {
            get
            {
                return System.IO.File.Exists(@"c:\temp\changednondcaccountstatus.txt");
            }
        }

        private void resetChangedNetlogonService()
        {
            string file = @"c:\temp\changednetlogonservicestatus.txt";
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);
        }

        private void resetChangedNonDcAccountStatus()
        {
            String file = @"c:\temp\changednondcaccountstatus.txt";
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);
        }
    }
}