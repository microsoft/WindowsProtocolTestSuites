// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat
{
    using System;

    using Microsoft.Modeling;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
    using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

    /// <summary>
    /// This class implements the ILsatAdapter interface.
    /// </summary>
    public partial class LsatAdapter : ADCommonServerAdapter, ILsatAdapter
    {
        #region Field

        #region Const field

        /// <summary>
        /// Value when all reserved bits present in the Access mask are set.
        /// </summary>
        public const uint InvalidDesiredAccess = 0x0CE0E000;

        /// <summary>
        /// Access mask value which has access to translate names into sids or vice versa.
        /// </summary>
        public const uint PolicyLookupNames = 0x00000800;

        #endregion

        #region Static field

        /// <summary>
        /// Instance of ITestSite.
        /// </summary>
        private static ITestSite lsatTestSite;

        /// <summary>
        /// Instance of LsaClient.
        /// </summary>
        private static LsaClient lsatClientStack;

        /// <summary>
        /// Value of mapped count.
        /// </summary>
        private static uint? mappedCounts = 0;

        /// <summary>
        /// Value of user principal supports or not.
        /// </summary>
        private static bool isUserPrincipalSupports;

        /// <summary>
        /// Value of domain name.
        /// </summary>
        private static string nameOfDomain;

        /// <summary>
        /// Value of sids are initialized or not.
        /// </summary>
        private static bool areSidsInitialized;

        /// <summary>
        /// Contains SID forms in buffer.
        /// </summary>
        private static _LSAPR_SID_ENUM_BUFFER[] sidEnumBuff = new _LSAPR_SID_ENUM_BUFFER[1];

        /// <summary>
        /// RPC names.
        /// </summary>
        private static _RPC_UNICODE_STRING[] rpcNames = new _RPC_UNICODE_STRING[1];

        /// <summary >
        /// RPC account names.
        /// </summary>
        private static _RPC_UNICODE_STRING[] rpcAccountNames = new _RPC_UNICODE_STRING[1];

        /// <summary>
        /// Lsa names.
        /// </summary>
        private static _LSA_UNICODE_STRING[] lsaNames = new _LSA_UNICODE_STRING[1];

        /// <summary>
        /// Contains the corresponding SID forms for security principal names in the Names parameter.
        /// </summary>
        private static _LSAPR_TRANSLATED_SIDS_EX2? translatedSids3 = new _LSAPR_TRANSLATED_SIDS_EX2();

        /// <summary>
        /// Contains the well known SID forms for security principal names in the Names parameter.
        /// </summary>
        private static _LSAPR_TRANSLATED_SIDS_EX2? translatedWellknownSids = new _LSAPR_TRANSLATED_SIDS_EX2();

        /// <summary>
        /// Contains the names for security principal names in the Names parameter.
        /// </summary>
        private static _LSAPR_TRANSLATED_NAMES_EX? translatedNames2 = new _LSAPR_TRANSLATED_NAMES_EX();

        /// <summary>
        /// Contains the names for security principal names in the Names parameter.
        /// </summary>
        private static _LSAPR_TRANSLATED_NAMES? translatedNames1 = new _LSAPR_TRANSLATED_NAMES();

        #endregion

        #region Private field

        /// <summary>
        /// Policy handle.
        /// </summary>
        private System.IntPtr? policyHandle = IntPtr.Zero;

        /// <summary>
        /// Current user name.
        /// </summary>
        private string currentUserName;

        /// <summary>
        /// Server name.
        /// </summary>
        private string serverName;

        /// <summary>
        /// Method status.
        /// </summary>
        private uint methodStatus;

        /// <summary>
        /// Desired access.
        /// </summary>
        private uint desiredAccess;

        /// <summary>
        /// Number of security principal names.
        /// </summary>
        private uint count;

        /// <summary>
        /// Count of sids.
        /// </summary>
        private uint countOfSids;

        /// <summary>
        /// Flags specified by the caller that control the lookup operation.
        /// </summary>
        private LookupOptions_Values lookUpOptions = LookupOptions_Values.NamesBesidesLocalComputer;

        /// <summary>
        /// Defines different scopes for searches during translation.
        /// </summary>
        private uint lookUpLevel;

        /// <summary>
        /// Version of the client, which implies the client's capabilities.
        /// </summary>
        private uint clientRevision;

        /// <summary>
        /// Timeout for bind and all future requests.
        /// </summary>
        private TimeSpan timeout = TimeSpan.FromMilliseconds(1000000);

        /// <summary>
        /// A flag that identified whether the class is disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// A flag that identified whether the SUT is domain controller.
        /// </summary>
        private bool isDomainController;

        /// <summary>
        /// Object attributes.
        /// </summary>
        private _LSAPR_OBJECT_ATTRIBUTES objectAttributes = new _LSAPR_OBJECT_ATTRIBUTES();

        /// <summary>
        /// Referenced domain list.
        /// </summary>
        private _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains = new _LSAPR_REFERENCED_DOMAIN_LIST?();

        /// <summary>
        /// Well known RPC names.
        /// </summary>
        private _RPC_UNICODE_STRING[] wellknownRpcNames = new _RPC_UNICODE_STRING[1];

        /// <summary>
        /// RPC names relative IDs.
        /// </summary>
        private _RPC_UNICODE_STRING[] rpcNamesRelativeIDs = new _RPC_UNICODE_STRING[1];

        /// <summary>
        /// An instance of LSAPR_TRANSLATED_SIDS_EX structure which defines a set of translated SIDs.
        /// </summary>
        private _LSAPR_TRANSLATED_SIDS_EX? translatedSids2 = new _LSAPR_TRANSLATED_SIDS_EX();

        /// <summary>
        /// An instance of LSAPR_TRANSLATED_SIDS structure which defines a set of translated SIDs.
        /// </summary>
        private _LSAPR_TRANSLATED_SIDS? translatedSids1 = new _LSAPR_TRANSLATED_SIDS();

        #endregion

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets lsatTestSite.
        /// </summary>
        internal static ITestSite LsatTestSite
        {
            get { return lsatTestSite; }
            set { lsatTestSite = value; }
        }

        /// <summary>
        /// Gets or sets lsatClientStack.
        /// </summary>
        internal static LsaClient LsatClientStack
        {
            get { return lsatClientStack; }
            set { lsatClientStack = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the User Principal is supported.
        /// </summary>
        internal static bool IsUserPrincipalSupports
        {
            get { return isUserPrincipalSupports; }
            set { isUserPrincipalSupports = value; }
        }

        /// <summary>
        /// Gets or sets nameOfDomain.
        /// </summary>
        internal static string NameOfDomain
        {
            get { return nameOfDomain; }
            set { nameOfDomain = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Sids are initialized.
        /// </summary>
        internal static bool AreSidsInitialized
        {
            get { return areSidsInitialized; }
            set { areSidsInitialized = value; }
        }

        /// <summary>
        /// Gets or sets sidEnumBuff.
        /// </summary>
        internal static _LSAPR_SID_ENUM_BUFFER[] SidEnumBuff
        {
            get { return sidEnumBuff; }
            set { sidEnumBuff = value; }
        }

        /// <summary>
        /// Gets or sets rpcNames.
        /// </summary>
        internal static _RPC_UNICODE_STRING[] RpcNames
        {
            get { return rpcNames; }
            set { rpcNames = value; }
        }

        /// <summary >
        /// Gets or sets rpcAccountNames.
        /// </summary>
        internal static _RPC_UNICODE_STRING[] RpcAccountNames
        {
            get { return rpcAccountNames; }
            set { rpcAccountNames = value; }
        }

        /// <summary>
        /// Gets or sets lsaNames.
        /// </summary>
        internal static _LSA_UNICODE_STRING[] LsaNames
        {
            get { return lsaNames; }
            set { lsaNames = value; }
        }

        /// <summary>
        /// Gets or sets mappedCounts.
        /// </summary>
        internal static uint? MappedCounts
        {
            get { return mappedCounts; }
            set { mappedCounts = value; }
        }

        /// <summary>
        /// Gets or sets translatedSids3.
        /// </summary>
        internal static _LSAPR_TRANSLATED_SIDS_EX2? TranslatedSids3
        {
            get { return translatedSids3; }
            set { translatedSids3 = value; }
        }

        /// <summary>
        /// Gets translatedWellknownSids.
        /// </summary>
        internal static _LSAPR_TRANSLATED_SIDS_EX2? TranslatedWellknownSids
        {
            get { return translatedWellknownSids; }
        }

        /// <summary>
        /// Gets or sets translatedNames2.
        /// </summary>
        internal static _LSAPR_TRANSLATED_NAMES_EX? TranslatedNames2
        {
            get { return translatedNames2; }
            set { translatedNames2 = value; }
        }

        /// <summary>
        ///  Gets or sets translatedNames1.
        /// </summary>
        internal static _LSAPR_TRANSLATED_NAMES? TranslatedNames1
        {
            get { return translatedNames1; }
            set { translatedNames1 = value; }
        }

        #endregion

        #region Initialize
        /// <summary>
        /// The request timeout that the Server waits for request from the Client, in milliseconds
        /// </summary>
        public const double RequestTimeout = 1000000;

        public string SUTServerName;
        public bool IsDomainController;
        string propertyGroup = "MS_LSAT.";
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            SUTServerName = this.GetProperty(propertyGroup + "SUT.Server.Computer.Name");
            IsDomainController = this.GetBoolProperty(propertyGroup + "SUT.IsDomainController");
        }

        /// <summary>
        /// Method to do all initial settings and bind to the server.
        /// </summary>
        /// <param name="anonymousAccess">Specifies whether the requestor is anonymous or not.</param>
        /// <param name="numOfHandles">Specifies the maximum number of handles that can be 
        /// opened by OpenPolicy and OpenPolcy2 methods at any instant of time.</param>
        /// <returns>Specifies the type of the server whether it is a DomainController or a Non-DomainController.</returns>
        public ProtocolServerConfig Initialize(
            AnonymousAccess anonymousAccess,
            uint numOfHandles)
        {
            LsatTestSite = TestClassBase.BaseTestSite;
            LsatClientStack = new LsaClient();

            Site.DefaultProtocolDocShortName = "MS-LSAT";
            this.timeout = TimeSpan.FromMilliseconds(RequestTimeout);
            this.serverName = SUTServerName;
            this.isDomainController = IsDomainController;
            // 0x00000001 implies a Client that is running an operating system released before Windows 2000;
            // 0x00000002 implies that the Client is running an operating system version of Windows 2000 or a later release!
            // Use const value here, as we will no longer support OS version earlier than Windows 2000
            this.clientRevision = 2;
            NameOfDomain = PrimaryDomainDnsName;
            this.currentUserName = DomainAdministratorName;
            string password = DomainUserPassword;

            LsatUtilities.InitializeWellKnownSecuritySIDs();

            AccountCredential accountCredential = new AccountCredential(
                NameOfDomain,
                this.currentUserName,
                password);

            // [MS-LSAT] section 2.1
            // The server SHOULD<4> reject calls that do not use an authentication level of 
            // RPC_C_AUTHN_LEVEL_NONE, RPC_C_AUTHN_LEVEL_PKT_INTEGRITY, or RPC_C_AUTHN_LEVEL_PKT_PRIVACY
            // ([MS-RPCE] section 2.2.1.1.8).
            LsatClientStack.BindOverNamedPipe(
                this.serverName,
                accountCredential,
                null,
                RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE,
                this.timeout);

            NtStatus returnValue = NtStatus.STATUS_SUCCESS;
            string systemName = string.Empty;

            this.desiredAccess = PolicyLookupNames;
            this.objectAttributes.RootDirectory = null;

            returnValue = LsatClientStack.LsarOpenPolicy2(
                systemName,
                this.objectAttributes,
                ACCESS_MASK.POLICY_LOOKUP_NAMES,
                out this.policyHandle);

            if (returnValue == NtStatus.STATUS_SUCCESS)
            {
                if (this.isDomainController)
                {
                    this.count = LsatUtilities.WellknownSecurityPrincipalMaxCount;

                    LsatUtilities.InitializeWellknownPrincipalNames(
                        this.count,
                        LsatUtilities.WellknownSecurityPrincipals,
                        ref this.wellknownRpcNames);

                    returnValue = LsatClientStack.LsarLookupNames3(
                        this.policyHandle.Value,
                        this.count,
                        this.wellknownRpcNames,
                        out this.referencedDomains,
                        ref translatedWellknownSids,
                        _LSAP_LOOKUP_LEVEL.LsapLookupWksta,
                        ref mappedCounts,
                        this.lookUpOptions,
                        (ClientRevision_Values)this.clientRevision);

                    LsatUtilities.ConvertSidsToString();

                    #region Verify Requirement

                    this.VerifyADTSRelatedRequirements1();

                    #endregion

                    this.count = LsatUtilities.RidMaxCount;

                    LsatUtilities.InitializeWellknownPrincipalNames(
                        this.count,
                        LsatUtilities.BuiltinRelativeDomainNames,
                        ref this.rpcNamesRelativeIDs);

                    returnValue = LsatClientStack.LsarLookupNames(
                        this.policyHandle.Value,
                        this.count,
                        this.rpcNamesRelativeIDs,
                        out this.referencedDomains,
                        ref this.translatedSids1,
                        _LSAP_LOOKUP_LEVEL.LsapLookupWksta,
                        ref mappedCounts);

                    #region Verify Requirement

                    this.VerifyADTSRelatedRequirements2();

                    #endregion
                } // End of if (isDomainController)

                this.count = LsatUtilities.TestsuiteMaxUserCount;

                LsatUtilities.InitializeAccountNames(DomainAdministratorName);

                returnValue = LsatClientStack.LsarLookupNames3(
                    this.policyHandle.Value,
                    this.count,
                    RpcAccountNames,
                    out this.referencedDomains,
                    ref translatedSids3,
                    _LSAP_LOOKUP_LEVEL.LsapLookupWksta,
                    ref mappedCounts,
                    this.lookUpOptions,
                    ClientRevision_Values.Known);

                if (returnValue == NtStatus.STATUS_SUCCESS)
                {
                    LsatUtilities.GetSids();
                    AreSidsInitialized = true;
                }
            } // End of if (returnValue == LsatUtilities.STATUS_SUCCESS)

            if (this.isDomainController)
            { 
                return ProtocolServerConfig.DomainController;
            }
            else
            {
                return ProtocolServerConfig.NonDomainController;
            }
        }

        #endregion

        #region LsarOpenPolicy

        /// <summary>
        /// Method to create a policy handle with the given permissions.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value.</param>
        /// <param name="accessMask">Contains the access to be given to the policyHandle.</param>
        /// <param name="openPolicyHandle">Output Parameter which contains Valid or Invalid or Null value.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns InvalidParameter if the parameters passed to the method are not valid.
        /// Returns AccessDenied if the caller is anonymous and the Server is a non-DomainController.</returns>
        public ErrorStatus OpenPolicy(
            RootDirectory rootDirectory,
            uint accessMask,
            out Handle openPolicyHandle)
        {
            ushort[] systemName = new ushort[] { 'k' };

            this.policyHandle = IntPtr.Zero;
            this.objectAttributes.RootDirectory = new byte[1];

            if (rootDirectory == RootDirectory.Null)
            {
                this.objectAttributes.RootDirectory = null;
            }
            else
            {
                this.objectAttributes.RootDirectory = new byte[1] { Convert.ToByte(LsatUtilities.InvalidRootDirectory) };
            }

            this.desiredAccess = (uint)accessMask;

            this.methodStatus = (uint)LsatClientStack.LsarOpenPolicy(
                systemName,
                this.objectAttributes,
                (ACCESS_MASK)this.desiredAccess,
                out this.policyHandle);

            #region Verify Requirement

            this.VerifyLsarOpenPolicy(rootDirectory);

            #endregion

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                openPolicyHandle = Handle.Valid;

                return ErrorStatus.Success;
            }
            else
            {
                openPolicyHandle = Handle.Null;

                if (this.methodStatus == LsatUtilities.StatusAccessDenied)
                {
                    return ErrorStatus.AccessDenied;
                }
                else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
                {
                    return ErrorStatus.InvalidParameter;
                }
                else
                {
                    return ErrorStatus.Unknown;
                }
            }
        }

        #endregion

        #region LsarOpenPolicy2

        /// <summary>
        /// Method to create a policy handle with the given permissions.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value.</param>
        /// <param name="accessMask">Contains the access to be given to the policyHandle.</param>
        /// <param name="openPolicyHandle2">Output Parameter which contains Valid or Invalid or Null value.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns InvalidParameter if the parameters passed to the method are not valid.
        /// Returns AccessDenied if the caller is anonymous and the Server is a non-DomainController.</returns>
        public ErrorStatus OpenPolicy2(
            RootDirectory rootDirectory,
            uint accessMask,
            out Handle openPolicyHandle2)
        {
            this.policyHandle = IntPtr.Zero;
            this.objectAttributes.RootDirectory = new byte[1];

            if (rootDirectory == RootDirectory.Null)
            {
                this.objectAttributes.RootDirectory = null;
            }
            else
            {
                this.objectAttributes.RootDirectory = new byte[1] { Convert.ToByte(LsatUtilities.InvalidRootDirectory) };
            }

            this.desiredAccess = (uint)accessMask;

            this.methodStatus = (uint)LsatClientStack.LsarOpenPolicy2(
                this.serverName,
                this.objectAttributes,
                (ACCESS_MASK)this.desiredAccess,
                out this.policyHandle);

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                openPolicyHandle2 = Handle.Valid;

                return ErrorStatus.Success;
            }
            else
            {
                openPolicyHandle2 = Handle.Null;

                if (this.methodStatus == LsatUtilities.StatusAccessDenied)
                {
                    return ErrorStatus.AccessDenied;
                }
                else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
                {
                    return ErrorStatus.InvalidParameter;
                }
                else
                {
                    return ErrorStatus.Unknown;
                }
            }
        }

        #endregion

        #region LsarClose

        /// <summary>
        /// Method to close the policy handle which is already opened by OpenPolicy or OpenPolicy2 method.
        /// </summary>
        /// <param name="handleToBeClosed">PolicyHandle which is already opened by LsarOpenPolicy or LsarOpenPolicy2 
        /// method.</param>
        /// <param name="handleAfterClose">OutPut Parameter contains Null if the method is successful else Invalid.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns AccessDenied if the caller is not authenticated.</returns>
        public ErrorStatus Close(int handleToBeClosed, out Handle handleAfterClose)
        {
            if (this.policyHandle != IntPtr.Zero)
            {
                this.methodStatus = (uint)LsatClientStack.LsarClose(ref this.policyHandle);
            }

            if (this.policyHandle == IntPtr.Zero)
            {
                handleAfterClose = Handle.Null;

                return ErrorStatus.Success;
            }
            else
            {
                handleAfterClose = Handle.Invalid;

                return ErrorStatus.InvalidHandle;
            }
        }

        #endregion

        #region LsarGetUserName

        /// <summary>
        /// Method to get the current user name and the domain name in which the user is in.
        /// </summary>
        /// <param name="authentication">Specifies whether the user is authenticated or not.</param>
        /// <param name="nameOfTheUser">It is an output parameter 
        /// where it specifies whether the userName is valid or not.</param>
        /// <param name="nameOfTheDomain">It is an output parameter 
        /// where it specifies whether the domainName is valid or not.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns AccessDenied if the caller is not authenticated.</returns>
        public ErrorStatus GetUserName(
            User authentication,
            out Name nameOfTheUser,
            out Name nameOfTheDomain)
        {
            string principalName = string.Empty;
            string netbiosName = string.Empty;
            _RPC_UNICODE_STRING? userName = null;
            _RPC_UNICODE_STRING? domainName = null;

            this.methodStatus = (uint)LsatClientStack.LsarGetUserName(
                this.serverName,
                ref userName,
                ref domainName);

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                principalName = LsatUtilities.ConvertUnicodesToString(userName.Value.Buffer);

                if (domainName != null)
                {
                    netbiosName = LsatUtilities.ConvertUnicodesToString(domainName.Value.Buffer);
                }

                #region Verify Requirement

                this.VerifyLsarGetUserName(principalName, netbiosName);

                #endregion

                nameOfTheUser = Name.valid;
                nameOfTheDomain = Name.valid;

                return ErrorStatus.Success;
            }
            else if (this.methodStatus == LsatUtilities.StatusAccessDenied)
            {
                nameOfTheUser = Name.Invalid;
                nameOfTheDomain = Name.Invalid;

                return ErrorStatus.AccessDenied;
            }
            else
            {
                nameOfTheUser = Name.Invalid;
                nameOfTheDomain = Name.Invalid;

                return ErrorStatus.Unknown;
            }
        }

        #endregion

        #region WinLookupNames2

        /// <summary>
        /// Win32 Method which internally calls LsarLookupNames4 method.
        /// </summary>
        /// <param name="handle">Specifies whether the handle is valid or invalid.</param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns InvalidServerState if the server is a Non-DomainController.
        /// Returns SomeNotMapped if some of the given names are not translated into their SID form.
        /// Returns NoneMapped if none of the names are translated into their SID form.
        /// Returns AccessDenied if the handle is Invalid.</returns>
        public ErrorStatus WinLookUpNames2(
            LSAHandle handle,
            Set<string> secPrincipalNames,
            out TranslatedSid translateSids,
            out MappedCount mapCount)
        {
            bool isAccess;
            LsaClient lsatClientStackTcp = new LsaClient();
            System.IntPtr? lsaPolicyHandle = IntPtr.Zero;
            _LSAPR_REFERENCED_DOMAIN_LIST? lsaReferencedDomains = new _LSAPR_REFERENCED_DOMAIN_LIST?();
            _LSAPR_TRANSLATED_SIDS_EX2? translatedSids = new _LSAPR_TRANSLATED_SIDS_EX2();

            IsUserPrincipalSupports = false;
            this.count = LsatUtilities.TestsuiteMaxUserCount;

            if (handle == LSAHandle.Valid)
            {
                isAccess = true;
            }
            else
            {
                isAccess = false;
            }

            if (secPrincipalNames.Contains("UserPrincipalName1")
                    && secPrincipalNames.Contains("FullQualifiedName1")
                    && secPrincipalNames.Contains("UnQualifiedName1")
                    && secPrincipalNames.Contains("IsolatedName1"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.Success;
            }
            else if ((secPrincipalNames.Contains("UserPrincipalName1")
                          || secPrincipalNames.Contains("FullQualifiedName1")
                          || secPrincipalNames.Contains("UnQualifiedName1")
                          || secPrincipalNames.Contains("IsolatedName1"))
                          && secPrincipalNames.Contains("DoesNotExist"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.SomeNotMapped;
            }
            else
            {
                LsatUtilities.NamesValidity = LsatUtilities.NoneMapped;
            }

            LsatUtilities.InitializeLookupNames();

            this.objectAttributes = new _LSAPR_OBJECT_ATTRIBUTES();

            NrpcNegotiateFlags nrpcNegotiateFlags = NrpcNegotiateFlags.DoesNotRequireValidationLevel2
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

            MachineAccountCredential accountCredential = new MachineAccountCredential(
                NameOfDomain,
                ENDPOINTNetbiosName,
                ENDPOINTPassword);

            if (this.isDomainController)
            {
                ushort[] endpoints = LsaUtility.QueryLsaTcpEndpoint(this.serverName);

                if (isAccess)
                {
                    NrpcClientSecurityContext securityContext = new NrpcClientSecurityContext(
                        NameOfDomain,
                        this.serverName,
                        accountCredential,
                        true,
                        nrpcNegotiateFlags);

                    lsatClientStackTcp.BindOverTcp(
                        this.serverName,
                        endpoints[0],
                        securityContext,
                        RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY,
                        this.timeout);
                }
                else 
                {
                    lsatClientStackTcp.BindOverTcp(
                        this.serverName,
                        endpoints[0],
                        null,
                        RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE,
                        this.timeout);
                }

                _RPC_UNICODE_STRING[] rpcNames = new _RPC_UNICODE_STRING[LsaNames.Length];

                for (int i = 0; i < rpcNames.Length; i++)
                {
                    rpcNames[i].Length = LsaNames[i].Length;
                    rpcNames[i].MaximumLength = LsaNames[i].MaximumLength;
                    rpcNames[i].Buffer = LsaNames[i].Buffer;
                }

                this.methodStatus = (uint)lsatClientStackTcp.LsarLookupNames4(
                    lsatClientStackTcp.Handle,
                    (uint)rpcNames.Length,
                    rpcNames,
                    out lsaReferencedDomains,
                    ref translatedSids,
                    _LSAP_LOOKUP_LEVEL.LsapLookupWksta,
                    ref mappedCounts,
                    LookupOptions_Values.NamesBesidesLocalComputer,
                    ClientRevision_Values.Unknown);

                if (lsatClientStackTcp.Handle != IntPtr.Zero)
                {
                    if (isAccess)
                    {
                        #region Verify Requirements

                        this.VerifyLsarLookupNames4(translatedSids);

                        #endregion
                    }
                }

                if (this.methodStatus == LsatUtilities.StatusSuccess)
                {
                    translateSids = TranslatedSid.Valid;
                    mapCount = MappedCount.Valid;

                    return ErrorStatus.Success;
                }
                else if (this.methodStatus == LsatUtilities.StatusSomeNotMapped)
                {
                    translateSids = TranslatedSid.Invalid;
                    mapCount = MappedCount.Invalid;

                    return ErrorStatus.SomeNotMapped;
                }
                else if (this.methodStatus == LsatUtilities.StatusInvalidServerState)
                {
                    translateSids = TranslatedSid.Invalid;
                    mapCount = MappedCount.Invalid;

                    return ErrorStatus.InvalidServerState;
                }
                else if (this.methodStatus == LsatUtilities.StatusAccessDenied)
                {
                    translateSids = TranslatedSid.Invalid;
                    mapCount = MappedCount.Invalid;

                    return ErrorStatus.AccessDenied;
                }
                else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
                {
                    translateSids = TranslatedSid.Invalid;
                    mapCount = MappedCount.Invalid;

                    return ErrorStatus.InvalidParameter;
                }
                else if (this.methodStatus == LsatUtilities.StatusNoneMapped)
                {
                    translateSids = TranslatedSid.Invalid;
                    mapCount = MappedCount.Invalid;

                    return ErrorStatus.NoneMapped;
                }
                else
                {
                    translateSids = TranslatedSid.Invalid;
                    mapCount = MappedCount.Invalid;

                    return ErrorStatus.Unknown;
                }
            }
            else
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.InvalidServerState;
             }
        }

        #endregion

        #region LsarLookupNames3

        /// <summary>
        /// Method to translate the given set of names into their SID form.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="optionOfLookup">Specifies the flag value whether the MSB is set or not.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalNames are translated into their SID form.
        /// Returns InvalidParameter if the method IsLookUpNameValid(secPrincipalNames)
        /// return false or lookUpLevel is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalNames into their 
        /// SID form.
        /// Returns SomeNotMapped if some of the given secPrincipalNames are not translated into their SID form.
        /// Returns NoneMapped if none of the secPrincipalNames are translated into their SID form.</returns>
        public ErrorStatus LookUpNames3(
            int handle,
            Set<string> secPrincipalNames,
            LookUpOption optionOfLookup,
            LookUpLevel levelOfLookup,
            out TranslatedSid translateSids,
            out MappedCount mapCount)
        {
            this.referencedDomains = new _LSAPR_REFERENCED_DOMAIN_LIST();
            TranslatedSids3 = new _LSAPR_TRANSLATED_SIDS_EX2();
            RpcNames = new _RPC_UNICODE_STRING[1];
            IsUserPrincipalSupports = false;

            this.count = LsatUtilities.TestsuiteMaxUserCount;

            if (secPrincipalNames.Contains("InvalidUserPrincipalName")
                    || secPrincipalNames.Contains("InvalidFullQualifiedName")
                    || secPrincipalNames.Contains("InvalidUnQualifiedName")
                    || secPrincipalNames.Contains("InvalidIsolatedName"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.Invalid;
            }
            else if (secPrincipalNames.Contains("UserPrincipalName1")
                         && secPrincipalNames.Contains("FullQualifiedName1")
                         && secPrincipalNames.Contains("UnQualifiedName1")
                         && secPrincipalNames.Contains("IsolatedName1"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.Success;
            }
            else if ((secPrincipalNames.Contains("UserPrincipalName1")
                         || secPrincipalNames.Contains("FullQualifiedName1")
                         || secPrincipalNames.Contains("UnQualifiedName1")
                         || secPrincipalNames.Contains("IsolatedName1"))
                         && secPrincipalNames.Contains("DoesNotExist"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.SomeNotMapped;
            }
            else
            {
                LsatUtilities.NamesValidity = LsatUtilities.NoneMapped;
            }

            if (optionOfLookup == LookUpOption.MSBSet)
            {
                this.lookUpOptions = LookupOptions_Values.NamesOnlyLocalAccountDatabaseExceptUPNs;
            }
            else
            {
                this.lookUpOptions = LsatUtilities.MsbNotSet;
                IsUserPrincipalSupports = true;
            }

            if (levelOfLookup == LookUpLevel.Invalid)
            {
                this.lookUpLevel = LsatUtilities.InvalidLookupLevel;
            }
            else
            {
                this.lookUpLevel = (uint)levelOfLookup;
            }

            LsatUtilities.InitializeNames();

            MappedCounts = 0;

            this.methodStatus = (uint)LsatClientStack.LsarLookupNames3(
                this.policyHandle.Value,
                this.count,
                RpcNames,
                out this.referencedDomains,
                ref translatedSids3,
                (_LSAP_LOOKUP_LEVEL)this.lookUpLevel,
                ref mappedCounts,
                this.lookUpOptions,
                (ClientRevision_Values)this.clientRevision);

            if (this.policyHandle != IntPtr.Zero)
            {
                #region Verify Requirement

                ErrorStatus verifyResult = this.VerifyLsarLookupNames3Names(optionOfLookup, levelOfLookup, out translateSids, out mapCount);

                if (verifyResult == ErrorStatus.NoneMapped)
                {
                    translateSids = TranslatedSid.Invalid;
                    mapCount = MappedCount.Invalid;

                    return ErrorStatus.NoneMapped;
                }

                this.VerifyLsarLookupNames3TranslatedSids(levelOfLookup);

                this.VerifyLsarLookupNames3Message(levelOfLookup);

                #endregion
            }

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                translateSids = TranslatedSid.Valid;
                mapCount = MappedCount.Valid;

                return ErrorStatus.Success;
            }
            else if (this.methodStatus == LsatUtilities.StatusSomeNotMapped)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.SomeNotMapped;
            }
            else if (this.methodStatus == LsatUtilities.StatusAccessDenied)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.AccessDenied;
            }
            else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.InvalidParameter;
            }
            else if (this.methodStatus == LsatUtilities.StatusNoneMapped)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.NoneMapped;
            }
            else
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.Unknown;
            }
        }

        #endregion

        #region LsarLookupNames2

        /// <summary>
        /// Method to translate the given set of names into their SID form.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalNames are translated into their SID form.
        /// Returns InvalidParameter if the method IsLookUpNameValid(secPrincipalNames) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalNames into their 
        /// SID form.
        /// Returns SomeNotMapped if some of the given secPrincipalNames are not translated into their SID form.
        /// Returns NoneMapped if none of the secPrincipalNames are translated into their SID form.</returns>
        public ErrorStatus LookUpNames2(
            int handle,
            Set<string> secPrincipalNames,
            LookUpLevel levelOfLookup,
            out TranslatedSid translateSids,
            out MappedCount mapCount)
        {
            this.referencedDomains = new _LSAPR_REFERENCED_DOMAIN_LIST();
            this.translatedSids2 = new _LSAPR_TRANSLATED_SIDS_EX();
            RpcNames = new _RPC_UNICODE_STRING[1];

            if (secPrincipalNames.Contains("InvalidUserPrincipalName")
                    || secPrincipalNames.Contains("InvalidFullQualifiedName")
                    || secPrincipalNames.Contains("InvalidUnQualifiedName")
                    || secPrincipalNames.Contains("InvalidIsolatedName"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.Invalid;
            }
            else if (secPrincipalNames.Contains("UserPrincipalName1")
                         && secPrincipalNames.Contains("FullQualifiedName1")
                         && secPrincipalNames.Contains("UnQualifiedName1")
                         && secPrincipalNames.Contains("IsolatedName1"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.Success;
            }
            else if ((secPrincipalNames.Contains("UserPrincipalName1")
                          || secPrincipalNames.Contains("FullQualifiedName1")
                          || secPrincipalNames.Contains("UnQualifiedName1")
                          || secPrincipalNames.Contains("IsolatedName1"))
                          && secPrincipalNames.Contains("DoesNotExist"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.SomeNotMapped;
            }
            else
            {
                LsatUtilities.NamesValidity = LsatUtilities.NoneMapped;
            }

            this.count = LsatUtilities.TestsuiteMaxUserCount;
            MappedCounts = 0;
            IsUserPrincipalSupports = true;

            LsatUtilities.InitializeNames();

            this.lookUpOptions = LookupOptions_Values.NamesOnlyLocalAccountDatabaseExceptUPNs;

            if (levelOfLookup == LookUpLevel.Invalid)
            {
                this.lookUpLevel = 0;
            }
            else
            {
                this.lookUpLevel = (uint)levelOfLookup;
            }

            this.methodStatus = (uint)LsatClientStack.LsarLookupNames2(
                this.policyHandle.Value,
                this.count,
                RpcNames,
                out this.referencedDomains,
                ref this.translatedSids2,
                (_LSAP_LOOKUP_LEVEL)this.lookUpLevel,
                ref mappedCounts,
                this.lookUpOptions,
                (ClientRevision_Values)this.clientRevision);

            if (this.policyHandle != IntPtr.Zero)
            {
                #region Verify Requirement

                this.VerifyLsarLookupNames2Names(levelOfLookup);

                this.VerifyLsarLookupNames2TranslatedSids(levelOfLookup);

                this.VerifyLsarLookupNames2Message(levelOfLookup);

                #endregion
            }

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                translateSids = TranslatedSid.Valid;
                mapCount = MappedCount.Valid;

                return ErrorStatus.Success;
            }
            else if (this.methodStatus == LsatUtilities.StatusSomeNotMapped)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.SomeNotMapped;
            }
            else if (this.methodStatus == LsatUtilities.StatusAccessDenied)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.AccessDenied;
            }
            else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.InvalidParameter;
            }
            else if (this.methodStatus == LsatUtilities.StatusNoneMapped)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.NoneMapped;
            }
            else
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.Unknown;
            }
        }

        #endregion

        #region LsarLookupNames

        /// <summary>
        /// Method to translate the given set of names into their SID form.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalNames are translated into their SID form.
        /// Returns InvalidParameter if the method IsLookUpNameValid(secPrincipalNames) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalNames into their 
        /// SID form.
        /// Returns SomeNotMapped if some of the given secPrincipalNames are not translated into their SID form.
        /// Returns NoneMapped if none of the secPrincipalNames are translated into their SID form.</returns>
        public ErrorStatus LookUpNames(
            int handle,
            Set<string> secPrincipalNames,
            LookUpLevel levelOfLookup,
            out TranslatedSid translateSids,
            out MappedCount mapCount)
        {
            this.referencedDomains = new _LSAPR_REFERENCED_DOMAIN_LIST();
            this.translatedSids1 = new _LSAPR_TRANSLATED_SIDS();
            RpcNames = new _RPC_UNICODE_STRING[1];

            if (secPrincipalNames.Contains("InvalidUserPrincipalName")
                    || secPrincipalNames.Contains("InvalidFullQualifiedName")
                    || secPrincipalNames.Contains("InvalidUnQualifiedName")
                    || secPrincipalNames.Contains("InvalidIsolatedName"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.Invalid;
            }
            else if (secPrincipalNames.Contains("UserPrincipalName1")
                         && secPrincipalNames.Contains("FullQualifiedName1")
                         && secPrincipalNames.Contains("UnQualifiedName1")
                         && secPrincipalNames.Contains("IsolatedName1"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.Success;
            }
            else if ((secPrincipalNames.Contains("UserPrincipalName1")
                          || secPrincipalNames.Contains("FullQualifiedName1")
                          || secPrincipalNames.Contains("UnQualifiedName1")
                          || secPrincipalNames.Contains("IsolatedName1"))
                          && secPrincipalNames.Contains("DoesNotExist"))
            {
                LsatUtilities.NamesValidity = LsatUtilities.SomeNotMapped;
            }
            else
            {
                LsatUtilities.NamesValidity = LsatUtilities.NoneMapped;
            }

            this.count = LsatUtilities.TestsuiteMaxUserCount;
            MappedCounts = 0;
            IsUserPrincipalSupports = false;

            LsatUtilities.InitializeNames();

            if (levelOfLookup == LookUpLevel.Invalid)
            {
                this.lookUpLevel = 0;
            }
            else
            {
                this.lookUpLevel = (uint)levelOfLookup;
            }

            this.methodStatus = (uint)LsatClientStack.LsarLookupNames(
                this.policyHandle.Value,
                this.count, 
                RpcNames,
                out this.referencedDomains,
                ref this.translatedSids1,
                (_LSAP_LOOKUP_LEVEL)this.lookUpLevel,
                ref mappedCounts);

            if (this.policyHandle != IntPtr.Zero)
            {
                #region Verify Requirement

                this.VerifyLsarLookupNamesNames(levelOfLookup);

                this.VerifyLsarLookupNamesTranslatedSids(levelOfLookup);

                this.VerifyLsarLookUpNamesMessage(levelOfLookup);

                #endregion
            }

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                translateSids = TranslatedSid.Valid;
                mapCount = MappedCount.Valid;

                return ErrorStatus.Success;
            }
            else if (this.methodStatus == LsatUtilities.StatusSomeNotMapped)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.SomeNotMapped;
            }
            else if (this.methodStatus == LsatUtilities.StatusAccessDenied)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.AccessDenied;
            }
            else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.InvalidParameter;
            }
            else if (this.methodStatus == LsatUtilities.StatusNoneMapped)
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.NoneMapped;
            }
            else
            {
                translateSids = TranslatedSid.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.Unknown;
            }
        }

        #endregion

        #region LsarLookupSids2

        /// <summary>
        /// Method to translate the given set of SIDs into human readable names.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalSids">Contains the SIDs to be translated into their Name form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateNames">An output parameter contains Valid if all the SIDs are translated
        /// into their Name form else Invalid.</param>
        /// <param name="mapCount">An output parameter contains Valid if all the SIDs are translated 
        /// into their Name form else Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalSids are translated into their Name form.
        /// Returns InvalidParameter if the method IsLookUpSidValid(secPrincipalSids) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalSids into their 
        /// Name form.
        /// Returns SomeNotMapped if some of the given secPrincipalSids are not translated into their Name form.
        /// Returns NoneMapped if none of the secPrincipalSids are translated into their Name form.</returns>
        public ErrorStatus LookUpSids2(
            int handle,
            Set<string> secPrincipalSids,
            LookUpLevel levelOfLookup,
            out TranstlatedNames translateNames,
            out MappedCount mapCount)
        {
            this.referencedDomains = new _LSAPR_REFERENCED_DOMAIN_LIST();
            SidEnumBuff = new _LSAPR_SID_ENUM_BUFFER[1];
            TranslatedNames2 = new _LSAPR_TRANSLATED_NAMES_EX();
            this.countOfSids = LsatUtilities.TestsuiteMaxUserCount;

            if (secPrincipalSids.Contains("InvalidSecurityPrincipalSid")
                    || secPrincipalSids.Contains("InvalidDomainSid"))
            {
                LsatUtilities.SidsValidity = LsatUtilities.Invalid;
            }
            else if ((secPrincipalSids.Contains("SecurityPrincipalSid1")
                          || secPrincipalSids.Contains("DomainSid1")) 
                          && secPrincipalSids.Contains("DoesNotExist"))
            {
                LsatUtilities.SidsValidity = LsatUtilities.SomeNotMapped;
            }
            else if (secPrincipalSids.Contains("SecurityPrincipalSid1")
                         && secPrincipalSids.Contains("DomainSid1"))
            {
                LsatUtilities.SidsValidity = LsatUtilities.Success;
            }
            else
            {
                LsatUtilities.SidsValidity = LsatUtilities.NoneMapped;
            }

            LsatUtilities.InitializeSids();

            MappedCounts = 0;

            if (levelOfLookup == LookUpLevel.Invalid)
            {
                this.lookUpLevel = 0;
            }
            else
            {
                this.lookUpLevel = (uint)levelOfLookup;
            }

            _LSAPR_SID_ENUM_BUFFER? sidEnumBuffValue = new _LSAPR_SID_ENUM_BUFFER();
            sidEnumBuffValue = SidEnumBuff[0];

            this.methodStatus = (uint)LsatClientStack.LsarLookupSids2(
                this.policyHandle.Value,
                sidEnumBuffValue,
                out this.referencedDomains,
                ref translatedNames2,
                (_LSAP_LOOKUP_LEVEL)this.lookUpLevel,
                ref mappedCounts,
                (uint)this.lookUpOptions,
                (ClientRevision_Values)this.clientRevision);
            
            #region Verify Requitement

            this.VerifyLsarLookupSids2Parameters(levelOfLookup);

            this.VerifyLsarLookupSids2TranslatedNames(levelOfLookup);

            this.VerifyLsarLookupSids2Message(levelOfLookup);

            #endregion

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                translateNames = TranstlatedNames.Valid;
                mapCount = MappedCount.Valid;

                return ErrorStatus.Success;
            }
            else if (this.methodStatus == LsatUtilities.StatusSomeNotMapped)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.SomeNotMapped;
            }
            else if (this.methodStatus == LsatUtilities.StatusAccessDenied)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.AccessDenied;
            }
            else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.InvalidParameter;
            }
            else if (this.methodStatus == LsatUtilities.StatusNoneMapped)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.NoneMapped;
            }
            else
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.Unknown;
            }
        }

        #endregion

        #region LsarLookupSids

        /// <summary>
        /// Method to translate the given set of SIDs into human readable names.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalSids">Contains the SIDs to be translated into their Name form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateNames">An output parameter contains Valid if all the SIDs are translated
        /// into their Name form else Invalid.</param>
        /// <param name="mapCount">An output parameter contains Valid if all the SIDs are translated 
        /// into their Name form else Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalSids are translated into their Name form.
        /// Returns InvalidParameter if the method IsLookUpSidValid(secPrincipalSids) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalSids into their 
        /// Name form.
        /// Returns SomeNotMapped if some of the given secPrincipalSids are not translated into their Name form.
        /// Returns NoneMapped if none of the secPrincipalSids are translated into their Name form.</returns>
        public ErrorStatus LookUpSids(
            int handle,
            Set<string> secPrincipalSids,
            LookUpLevel levelOfLookup,
            out TranstlatedNames translateNames,
            out MappedCount mapCount)
        {
            this.referencedDomains = new _LSAPR_REFERENCED_DOMAIN_LIST();
            TranslatedNames1 = new _LSAPR_TRANSLATED_NAMES();
            SidEnumBuff = new _LSAPR_SID_ENUM_BUFFER[1];
            this.countOfSids = LsatUtilities.TestsuiteMaxUserCount;

            if (secPrincipalSids.Contains("InvalidSecurityPrincipalSid")
                    || secPrincipalSids.Contains("InvalidDomainSid"))
            {
                LsatUtilities.SidsValidity = LsatUtilities.Invalid;
            }
            else if ((secPrincipalSids.Contains("SecurityPrincipalSid1") 
                          || secPrincipalSids.Contains("DomainSid1"))
                          && secPrincipalSids.Contains("DoesNotExist"))
            {
                LsatUtilities.SidsValidity = LsatUtilities.SomeNotMapped;
            }
            else if (secPrincipalSids.Contains("SecurityPrincipalSid1")
                         && secPrincipalSids.Contains("DomainSid1"))
            {
                LsatUtilities.SidsValidity = LsatUtilities.Success;
            }
            else
            {
                LsatUtilities.SidsValidity = LsatUtilities.NoneMapped;
            }

            LsatUtilities.InitializeSids();

            MappedCounts = 0;

            if (levelOfLookup == LookUpLevel.Invalid)
            {
                this.lookUpLevel = 0;
            }
            else
            {
                this.lookUpLevel = (uint)levelOfLookup;
            }

            _LSAPR_SID_ENUM_BUFFER? sidEnumBuffTemp = SidEnumBuff[0];

            this.methodStatus = (uint)LsatClientStack.LsarLookupSids(
                this.policyHandle.Value,
                sidEnumBuffTemp,
                out this.referencedDomains,
                ref translatedNames1,
                (_LSAP_LOOKUP_LEVEL)this.lookUpLevel,
                ref mappedCounts);

            if (this.policyHandle != IntPtr.Zero)
            {
                #region Verify Requirement

                this.VerifyLsarLookupSidsParameters(levelOfLookup);

                this.VerifyLsarLookupSidsTranslatedNames(levelOfLookup);

                this.VerifyLsarLookupSidsMessage(levelOfLookup);

                #endregion
            }

            if (this.methodStatus == LsatUtilities.StatusSuccess)
            {
                translateNames = TranstlatedNames.Valid;
                mapCount = MappedCount.Valid;

                return ErrorStatus.Success;
            }
            else if (this.methodStatus == LsatUtilities.StatusSomeNotMapped)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.SomeNotMapped;
            }
            else if (this.methodStatus == LsatUtilities.StatusAccessDenied)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.AccessDenied;
            }
            else if (this.methodStatus == LsatUtilities.StatusInvalidParameter)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.InvalidParameter;
            }
            else if (this.methodStatus == LsatUtilities.StatusNoneMapped)
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.NoneMapped;
            }
            else
            {
                translateNames = TranstlatedNames.Invalid;
                mapCount = MappedCount.Invalid;

                return ErrorStatus.Unknown;
            }
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Dispose method.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Override Reset method that is called before each test case runs.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            if (LsatClientStack != null)
            {
                LsatClientStack.Dispose();
                LsatClientStack = null;
            }
        }

        /// <summary>
        /// Override dispose function.
        /// </summary>
        /// <param name="disposing">Release managed resources or not, true to release, false if not.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                    if (LsatClientStack != null)
                    {
                        LsatClientStack.Dispose();
                        LsatClientStack = null;
                    }
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
