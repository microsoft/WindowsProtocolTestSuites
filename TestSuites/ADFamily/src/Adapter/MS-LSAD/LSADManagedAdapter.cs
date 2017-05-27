// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
    
    /// <summary>
    /// Implement methods of interface ILsadManagedAdapter and ManagedAdapterBase.
    /// </summary>
    public partial class LsadManagedAdapter : ADCommonServerAdapter, ILsadManagedAdapter
    {
        #region variables

        /// <summary>
        /// client of LSAD
        /// </summary>
        internal static LsaClient lsadClientStack;

        /// <summary>
        /// domain NetBIOS Name
        /// </summary>
        internal static string domain;

        /// <summary>
        /// Full domain Name
        /// </summary>
        internal static string fullDomain;

        /// <summary>
        /// Instance of ILsadManagedAdapter
        /// </summary>
        internal static ILsadManagedAdapter lsadAdapter;

        /// <summary>
        /// Domain SrvGUID in lower case
        /// </summary>
        internal static string DomainGUID;

        /// <summary>
        /// string type of server name
        /// </summary>
        private string strServerName;

        /// <summary>
        /// ushort array type of server name
        /// </summary>
        private ushort[] serverName;

        /// <summary>
        /// user name
        /// </summary>
        private string userName;
                
        /// <summary>
        /// time out in milliseconds
        /// </summary>
        private TimeSpan timeout;

        /// <summary>
        /// flag used to detect disposed or not
        /// </summary>
        private bool disposed;
        #endregion

        #region const values
        /// <summary>
        /// Set the user account name.
        /// </summary>
        public static string DomainUserName = "user";
        /// <summary>
        /// Give the Name of the Trusted Domain Object that needs to be created.
        /// </summary>
        internal static string ValidDomainName = "TestDomain13";
        /// <summary>
        /// Set any Random Trusted Domain Object name that won't exist on the domain.
        /// </summary>
        internal static string NoDomainName = "NoDom11";
        /// <summary>
        /// Set Domain Object for SetdomainInfo name that won't exist on the domain.
        /// </summary>
        internal static string NewDomainNameforSetdomainInfo = "Wipro11";
        /// <summary>
        /// This is the windows hard coded key which should be used when RPC security mechanism is used.
        /// </summary>
        internal static string HardCodedKey = "53797374656d4c696272617279445443";
        /// <summary>
        /// Global Secret Name.
        /// </summary>
        internal static string GlobalSecretName = "G$test.com";
        /// <summary>
        /// Values For LSAPR_AUTH_INFORMATION
        /// Values for AuthType
        /// Ignore this Type             = 0
        /// Derived RC4HMAC Key          = 1
        /// Plain Text Password          = 2
        /// Plain Text Password Version  = 3
        /// </summary>
        internal static uint CountOfIncomingAuthInfos = 1;
        internal static uint CountOfOutgoingAuthInfos = 1;
        internal static uint IncomingAuthType = 2;
        internal static string IncomingAuthInfo = "Incoming";
        internal static string PrevIncomingAuthInfo = "IncomingPrev";
        internal static string OutgoingAuthInfo = "Outgoing";
        internal static string PrevOutgoingAuthInfo = "Incoming";
        internal static string NewPassword = "NoPassword";
        internal static string OldPassword = "Password";
        /// <summary>
        /// Any string to store as forest information.
        /// </summary>
        internal static string ForestDefaultData = "ForestInformation";
        /// <summary>
        /// Give the SID of the Trusted Domain Object that needs to be created.
        /// </summary>
        internal static string ValidSid = "S-1-5-21-10-16-49";
        /// <summary>
        /// Give the SID in this format so that it will force the invalid condition.i.e S-0-x-y-z.
        /// </summary>
        internal static string InvalidSid = "S-0-4-20-10-16-49";
        /// <summary>
        /// Give a Random SID that are not assigned to any of the Trusted Domain Objects.
        /// </summary>
        internal static string NoSid = "S-1-5-21-19-16-49";
        /// <summary>
        /// These values are input for setting LocalAccountDomainInformation. Applicable only for Windows Vista.
        /// This value is to give input to SID of Account domain. It must not be null.
        /// </summary>
        internal static string GetLocalAccountDomainSid = "S-1-5-21-1498679869-1072665971-3056187453";
        /// <summary>
        /// These values specifies SID of domain. Must follow RPC_SID validation.
        /// </summary>
        internal static string GetAccountDomainSidNonDC = "S-1-5-21-1377892947-3406335109-783424903";
        /// <summary>
        /// Values for Information Class PolicyAuditEventsInformation.
        /// AuditingMode value sets to 1 means auditing is enabled else if this value is set to 0 then auditing is disabled.
        /// </summary>
        internal static uint AuditingMode = 1;
        /// <summary>
        /// MaximumAuditEventCount must not be 0 and should always be less than 8.It is count of number of elements in AuditOption array.
        /// </summary>
        internal uint MaximumAuditEventCount = 1; 
        /// <summary>
        /// These values are for setting PolicyReplicaSourceInformation Information class. It is an obsolete one.
        /// </summary>
        internal static string ReplicasourceName = "0";
        internal static string ReplicaAccountName = "0";
        /// <summary>
        /// These values are used to set PolicyDomainEfsInfo Information class which communicates about a counted binary byte array.
        /// This value specifies count of bytes in EfsBlob.
        /// </summary>
        internal static uint InfoLength = 639;
        /// <summary>
        /// Input for creating EfsBlob(Array of Bytes) of InfoLength size.
        /// </summary>
        internal static uint KeyCount = 16777216;
        /// <summary>
        /// Input for Key blob used in EfsBlob.
        /// </summary>
        internal static uint Length1 = 1996619776;
        internal static uint Length2 = 1929510912;
        internal static uint SIDOffset = 469762048;
        internal static uint CertificateLength = 989986816;
        internal static uint CertificateOffset = 939524096;
        internal static uint Certificate = 908764277;
        /// <summary>
        /// These values are used to set PolicyKerberoseTicketInformation Information class. 
        /// It communicates policy information about Kerberose ticket provider.
        /// This value is optionalflag that affects validation performed during authentication.
        /// </summary>
        internal static uint Authenticationoption = 128;
        /// <summary>
        /// This value is in unit of 10^(-7) seconds.It specifies maximum ticket lifetime for service ticket.
        /// </summary>
        internal static long Maxservicequadpart = 360000000000;
        /// <summary>
        /// This value is in unit of 10^(-7) seconds and corresponds to maximum ticket lifetime for TGT(Ticket -granting ticket).
        /// </summary>
        internal static long Maxticketquadpart = 360000000000;
        /// <summary>
        /// This value is in unit of 10^(-7) seconds and corresponds to maximum renewable time.
        /// </summary>
        internal static long MaxRenewquadpart = 6048000000000;
        /// <summary>
        /// This value is in unit of 10^(-7) seconds and corresponds to acceptable clock skew.
        /// </summary>
        internal static long MaxClockskewquad = 3000000000; 
        /// <summary>
        /// SID SubAuthorityCount value.
        /// </summary>
        internal static byte SIDCount = 4;
        /// <summary>
        /// RPC_SID.SubAuthority array size.
        /// </summary>
        internal static int SID = 4;
        /// <summary>
        /// Passing new SID value to Account object.
        /// </summary>
        internal static string NewSID = "15-10-10-10";
        /// <summary>
        /// Passing exist SID value to Account object.
        /// </summary>
        internal static string ExistSID = "51-50-50-50";
        /// <summary>
        /// Passing unknown SID value to Account object.
        /// </summary>
        internal static string UnKnownSID = "82-77-77-77";
        /// <summary>
        /// These are the privilege name inputs to privilege objects opnums.
        /// This is the input to check whether the privilege is valid.
        /// </summary>
        internal static string ValidPrivilegeName = "SeAssignPrimaryTokenPrivilege";
        /// <summary>
        /// This is the input to check whether the privilege is invalid or not.
        /// </summary>
        internal static string InValidPrivilegeName = "abcdef";
        /// <summary>
        /// This is the input to check whether the privilege exists or not.
        /// </summary>
        internal static string NoSuchPrivilegeName = "abcdef";
        /// <summary>
        /// These are the secrets inputs that are to be given to the encrypted current value
        /// and to encrypt old value as well as the encrypted data that is to be stored for
        /// LsarStorePrivateData.
        /// </summary>
        internal static string InputCurrentMessage = "current";
        internal static string InputOldMessage = "old";
        internal static string InputEncryptedMessage = "encryptedMessage";
        /// <summary>
        /// These are the LUID inputs to privilege objects opnums.
        /// </summary>
        internal static int ValidInputPrivilegeLUIDHighPart = 0;
        internal static uint ValidInputPrivilegeLUIDLowPart = 20;
        /// <summary>
        /// This input is to judge whether current privilege name is valid.
        /// </summary>
        internal static string PrivilegeName = "SeDebugPrivilege";
        /// <summary>
        /// This is the valid secret name to be given.
        /// </summary>
        internal static string ValidName = "newObject412";
        /// <summary>
        /// This parameter is given for the query secret opnum to return access denied which is windows specific.
        /// </summary>
        internal static string local_system = "$MACHINE.ACC";
        /// <summary>
        /// This parameter is given to check the windows behavior of creating secret to get INVALID_PARAMETER.
        /// </summary>
        internal static string InValidName = "G$$\\globalinavlid";
        /// <summary>
        /// This parameter is given to check the return value of OBJECT_NAME_NOT_FOUND which can be any name and should be there in the server.
        /// </summary>
        internal static string NotPresentName = "somenewname";
        /// <summary>
        /// This parameter is given to check the windows behavior of creating secret to get NAME_TOO_LONG which should be greater than 128 characters.
        /// </summary>
        internal static string TooLongName = "thisisthenewobjectwhichisoflenghtbigcharacterssothinkitwillworkwithourexceptioninthprogramofcreatesecretobjectsoflsadprotocolsbutitdidntworkwattododocischangedhelpedmesomething";
        /// <summary>
        /// Type:bool! Usage: Is supported or not!
        /// </summary>
        internal static bool LsarSetSecretIsSupported = true;
        internal static bool LsarQuerySecretIsSupported = true;
        internal static bool LsarStorePrivateDataIsSupported = true;
        internal static bool LsarRetrievePrivateDataIsSupported = true;
        internal static bool EFSIfIsSuproted = true;
        /// <summary>
        /// All the switch property elements of the SHOULD/MAY should be in this section.
        /// These properties are to specify that if requirements are implemented. If the requirement is implemented,
        /// the value of the property should be set to true, otherwise set to false.
        /// Type:bool! Usage: Is implemented or not!
        /// </summary>
        internal static string R471Implementation = "true";
        internal static string R753Implementation = "true";
        internal static string R1049Implementation = "false";
        internal static string R1056Implementation = "true";
        internal static string R101042Implementation = "true";
        internal static string R201042Implementation = "true";
        internal static string R301042Implementation = "true";
        internal static string R401042Implementation = "true";
        internal static string R946Implementation = "true";
        internal static string R949Implementation = "true";
        internal static string R952Implementation = "true";

        #endregion

        string propertyGroup = "MS_LSAD.";

        static LsadManagedAdapter _adapter = null;

        #region Publicevents

        /// <summary>
        /// event ResponseHandler of EnumerateAccounts
        /// </summary>
        public event ResponseHandler EnumerateAccounts;

        /// <summary>
        /// event ResponseHandler of EnumerateTrustedDomainsEx
        /// </summary>
        public event ResponseHandler EnumerateTrustedDomainsEx;

        /// <summary>
        /// event ResponseHandler of EnumerateTrustedDomains
        /// </summary>
        public event ResponseHandler EnumerateTrustedDomains;

        /// <summary>
        /// event ResponseHandler of EnumeratePrivileges
        /// </summary>
        public event ResponseHandler EnumeratePrivileges;

        #endregion Publicevents

        /// <summary>
        /// Gets current session key.
        /// </summary>
        /// <returns> Current session key </returns>
        public byte[] SessionKey
        {
            get
            {
                return lsadClientStack.SessionKey;
            }
        }

        /// <summary>
        /// Initialize the protocol adapter.
        /// </summary>
        /// <param name="testSite">The test site.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            ISUTControlAdapterInstance = Site.GetAdapter<ILsadSutControlAdapter>();
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// override Reset method that is called before each test case runs
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            if (lsadClientStack != null)
            {
                lsadClientStack.Dispose();
                lsadClientStack = null;
            }
        }

        /// <summary>
        /// Static instance for Lsad Managed Server Adapter
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static new LsadManagedAdapter Instance(ITestSite site)
        {
            if (_adapter == null)
            {
                _adapter = new LsadManagedAdapter();
                _adapter.Initialize(site);
            }
            return _adapter;
        }

        #region bool GetSUTOSVersion

        /// <summary>
        /// Return of Get SUT's OS version
        /// </summary>
        /// <param name="sutOSVersion">Represent server's platform type</param>
        /// <returns>The call must return true which indicates success</returns>
        public bool GetSUTOSVersion(out Server sutOSVersion)
        {
            switch (PDCOSVersion)
            {
                case ServerVersion.Win2003: 
                    sutOSVersion = Server.Windows2k3; 
                    break;
                case ServerVersion.Win2008: 
                    sutOSVersion = Server.Windows2k8; 
                    break;
                case ServerVersion.Win2008R2:
                    sutOSVersion = Server.Windows2k8r2;
                    break;
                case ServerVersion.Win2012:
                    sutOSVersion = Server.Windows2k12;
                    break;
                case ServerVersion.Win2012R2:
                    sutOSVersion = Server.Windows2k12r2;
                    break;
                case ServerVersion.NonWin:
                case ServerVersion.Invalid:
                default:
                    sutOSVersion = Server.NonWindows;
                    break;
            }
            return true;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// The Initialize is invoked to initialize the environment for server.
        /// </summary>
        /// <param name="serverConfig">Values for server configuration whether it is DC or Non DCalues</param>
        /// <param name="anonymousAccess">Values for setting access to anonymous requester</param>
        /// <param name="windowsServer">SUT's OS version</param>
        /// <param name="noOfHandles">Specifies the maximum number of handles 
        /// that can be opened by OpenPolicy and OpenPolcy2 methods at any instant of time</param>
        /// <param name="isDomainAdmin">Set true if the user is Domain Admin, else set false</param>
        public void Initialize(
            ProtocolServerConfig serverConfig,
            AnonymousAccess anonymousAccess,
            Server windowsServer,
            int noOfHandles,
            bool isDomainAdmin)
        {
            lsadClientStack = new LsaClient();
            lsadAdapter = Site.GetAdapter<ILsadManagedAdapter>();

            if (serverConfig == ProtocolServerConfig.DomainController
                    || serverConfig == ProtocolServerConfig.PrimaryDomainController)
            {
                isDC = true;
                this.strServerName = this.PDCNetbiosName;
            }
            else if (serverConfig == ProtocolServerConfig.ReadOnlyDomainController)
            {
                isDC = true;
                this.strServerName = this.RODCNetbiosName;
            }
            else if (serverConfig == ProtocolServerConfig.NonDomainController)
            {
                isDC = false;
                this.strServerName = this.DMNetbiosName;
            }
            if (isDomainAdmin)
            {
                this.userName = this.DomainAdministratorName;
                IsInDomainAdminsGroup = true;
            }
            else if (!isDomainAdmin)
            {
                this.userName = DomainUserName;
                IsInDomainAdminsGroup = false;
            }

            fullDomain = this.PrimaryDomainDnsName;
            domain = this.PrimaryDomainNetBiosName;
            this.timeout = TimeSpan.FromMilliseconds(GetDoubleProperty(propertyGroup + "TimeoutMilliseconds"));
            secretNameOfSecretObject = ValidName;                    
            isWindows = !ServerVersion.NonWin.Equals(PDCOSVersion) && !ServerVersion.Invalid.Equals(PDCOSVersion);
            this.disposed = false;
            this.serverName = utilities.ConversionfromStringtoushortArray(this.strServerName);
            DomainGUID = this.PrimaryDomainSrvGUID.ToLower();

            AccountCredential transportCredential = new AccountCredential(string.Empty, this.userName, this.DomainUserPassword);
            lsadClientStack.BindOverNamedPipe(
                this.strServerName,
                transportCredential,
                null,
                RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE,
                this.timeout);

            base.Initialize(Site);
            lsadUUID = constLsadUUID;
            lsadendPoint = constLsadendPoint;
            lsadProtocolSequence = constLsadProtocolSequence;

            stPolicyInformation.PHandle = 0;
            stPolicyInformation.AccessforHandle = ACCESS_MASK.ACCOUNT_ADJUST_PRIVILEGES;
            serverPlatform = windowsServer;

            ////SID SubAuthorityCount value.
            objAccountSid[0].SubAuthorityCount = SIDCount;
            
            ////_RPC_SID.SubAuthority array size.
            objAccountSid[0].SubAuthority = new uint[SID];
            htAccHandle.Clear();
            htAddAccRight.Clear();
            checkTrustHandle = false;
            isitSetTrustedDomainInfo = false;

            trustObjectCreateinformation.doesTdoSupportForestInformation = false;
            trustObjectCreateinformation.intTdoHandleNumber = 0;
            trustObjectCreateinformation.isForestInformationPresent = false;
            trustObjectCreateinformation.strDomainSid = string.Empty;
            trustObjectCreateinformation.strTdoDnsName = string.Empty;
            trustObjectCreateinformation.strTdoNetBiosName = string.Empty;
            trustObjectCreateinformation.uintTdoDesiredAccess = 0;
            trustObjectCreateinformation.uintTrustAttr = 0;
            trustObjectCreateinformation.uintTrustDir = 0;
            trustObjectCreateinformation.uintTrustType = 0;
            domainState = serverConfig;

            stSecretInformation.strNameOfSecretObject = string.Empty;
            stSecretInformation.UIntSecretHandleAccessCount = 1;
        }

        #endregion

        /// <summary>
        /// override dispose function
        /// </summary>
        /// <param name="disposing">release managed resources or not, true to release, false if not</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // release managed resources
                    if (lsadClientStack != null)
                    {
                        lsadClientStack.Dispose();
                        lsadClientStack = null;
                    }
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}