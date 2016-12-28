// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;

    using Microsoft.Modeling;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;

    /// <summary>
    /// Interfaces class of Protocol Adapter methods.
    /// </summary>
    public interface INrpcServerAdapter : IAdapter
    {
        #region Get Property

        /// <summary>
        /// This method is used to get the SUT's platform.
        /// </summary>
        /// <param name="sutPlatform">The SUT's platform.</param>
        void GetPlatform(out PlatformType sutPlatform);

        /// <summary>
        /// This method is used to get the client account type.
        /// </summary>
        /// <param name="isAdministrator">Whether the client account is an administrator account.</param>
        void GetClientAccountType(out bool isAdministrator);

        /// <summary>
        /// Switch current user account.
        /// </summary>
        /// <param name="hasSufficientPrivileges">Whether the user account has sufficient privileges.</param>
        void SwitchUserAccount(bool hasSufficientPrivileges);
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
        ///  A set of bit flags that provide additional data that is used to process the request.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrGetDcNameEx2(
            ComputerType sutType,
            AccounterNameType accountType,
            uint allowableAccountControlBits,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            SiteNameType siteType,
            uint flags);

        /// <summary>
        ///  The DsrGetDcNameEx returns information about a domain controller in the specified domain and site. 
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <param name="domainGuidType">The type of domain's GUID.</param>
        /// <param name="siteType">The type of the site that the DC must be located.</param>
        /// <param name="flags">A set of bit flags that provide additional data used to process the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrGetDcNameEx(
            ComputerType sutType,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            SiteNameType siteType,
            uint flags);

        /// <summary>
        ///  The DsrGetDcName returns information about a domain controller in the specified domain.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The name of the domain queried.</param>
        /// <param name="domainGuidType">The type of domain's GUID.</param>
        /// <param name="siteGuid">The name of the site that the DC must be located.</param>
        /// <param name="flags">A set of bit flags that provide additional data used to process the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrGetDcName(
            ComputerType sutType,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            SiteGuidType siteGuid,
            uint flags);

        /// <summary>
        /// The NetrGetDCName method retrieves the NetBIOS name of the PDC for the specified domain.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The type of domain to be queried.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrGetDCName(ComputerType sutType, DomainNameType domainType);

        /// <summary>
        ///  The NetrGetAnyDCName method retrieves the name of a domain controller in the specified primary 
        ///  or directly trusted domain. 
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrGetAnyDCName(ComputerType sutType, DomainNameType domainType);

        /// <summary>
        /// The DsrGetSiteName method returns the site name for the specified computer that receives this call.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrGetSiteName(ComputerType sutType);

        /// <summary>
        /// The DsrGetDcSiteCoverageW method returns a list of sites covered by a domain controller.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrGetDcSiteCoverageW(ComputerType sutType);

        /// <summary>
        /// The DsrAddressToSiteNamesW method translates a list of socket addresses into their corresponding site names.
        /// </summary>
        /// <param name="sutType"> The type of SUT computer receiving the request.</param>
        /// <param name="socketAddresses">A list of socket addresses.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrAddressToSiteNamesW(ComputerType sutType, Set<SocketAddressType> socketAddresses);

        /// <summary>
        ///  The DsrAddressToSiteNamesExW method translates a list of socket addresses into their corresponding 
        ///  site names and subnet names.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="socketAddresses">A list of socket addresses.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrAddressToSiteNamesExW(ComputerType sutType, Set<SocketAddressType> socketAddresses);

        /// <summary>
        ///  The DsrDeregisterDnsHostRecords method deletes all of the DNS SRV records registered by 
        ///  a specified domain controller.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The type of fully qualified domain.</param>
        /// <param name="domainGuidType">The domain GUID.</param>
        /// <param name="dsaGuidType">The type of DC's TDSDSA object's GUID.</param>
        /// <param name="dnsHostType">The type of domain whose records will be deregistered.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT DsrDeregisterDnsHostRecords(
            ComputerType sutType,
            DomainNameType domainType,
            DomainGuidType domainGuidType,
            DsaGuidType dsaGuidType,
            ComputerType dnsHostType);

        #endregion

        #region Secure Channel Establishment and Maintance Methods

        /// <summary>
        ///  The NetrServerReqChallenge method receives a client challenge and returns a SUT challenge.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <returns> The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrServerReqChallenge(ComputerType primaryDcType, ComputerType clientType);

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
        HRESULT NetrServerAuthenticate3(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidClientCredentialUsed,
            uint negotiateFlags);

        /// <summary>
        /// The NetrServerAuthenticate2 method mutually authenticates the client and the SUT and establishes 
        /// the session key to be used for the secure channel message protection between the client and the SUT.
        /// </summary>
        /// <param name="primaryDcType">The type of the primary DC.</param>
        /// <param name="accountType">
        ///  The name of the account that contains the secret key (password) shared between the client and the SUT.
        ///  </param>
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
        HRESULT NetrServerAuthenticate2(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidClientCredentialUsed,
            uint negotiateFlags);

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
        HRESULT NetrServerAuthenticate(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidClientCredentialUsed);

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
        HRESULT NetrServerPasswordSet2(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed);

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
        HRESULT NetrServerPasswordSet(
            ComputerType primaryDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed);

        /// <summary>
        ///  The NetrServerTrustPasswordsGet method returns the encrypted current and previous passwords
        ///  for an account in the domain.
        /// </summary>
        /// <param name="trustedDcType">The type of a trusted DC.</param>
        /// <param name="accountType"> 
        ///  The name of the client account in the domain for which the trust password must be returned.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value that indicates the type of the secure channel being 
        ///  established by this call.
        /// </param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrServerTrustPasswordsGet(
            ComputerType trustedDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed);

        /// <summary>
        ///  The NetrLogonGetDomainInfo method returns information that describes the current domain 
        ///  to which the specified client belongs.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="clientType">The type of the client computer issuing the request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <param name="level">The information level requested by the client.</param>
        ///<param name="workStationInfo">The information about the client workstation.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonGetDomainInfo(
            ComputerType sutType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            uint level,
            AbstractNetLogonWorkStationInformation workStationInfo);

        /// <summary>
        /// The NetrLogonGetCapabilities method is used by the client to confirm the SUT capabilities 
        /// after a secure channel has been established.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="clientType">The type of the client computer issuing the request.</param>
        /// <param name="isValidAuthenticatorUsed">Whether using valid client Authenticator.</param>
        /// <param name="queryLevel">
        ///  The level of information that will be returned from the SUT.
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonGetCapabilities(
            ComputerType sutType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            uint queryLevel);

        #endregion

        #region Pass-Through Authentication Methods

        /// <summary>
        ///  The NetrLogonSamLogonEx method handles logon requests for the SAM accounts 
        ///  and allows generic pass-through authentication.
        /// </summary>
        /// <param name="useSecureRpc">Whether secure RPC channel is used.</param>
        /// <param name="logonServerType">The NetBIOS name of the SUT that will handle the logon request.</param>
        /// <param name="clientType">The type of client sending the logon request.</param>
        /// <param name="logonLevel">
        ///   A NETLOGON_LOGON_INFO_CLASS structure that specifies the type of the logon information
        ///   passed in the LogonInformation parameter.
        ///  </param>
        /// <param name="logonInformationType">The type of logon request information.</param>
        /// <param name="validationLevel"> 
        ///A NETLOGON_VALIDATION_INFO_CLASS enumerated value that contains the validation level 
        ///  requested by the client.
        /// </param>
        /// <param name="extraFlags">A set of bit flags that specify delivery settings.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonSamLogonEx(
            bool useSecureRpc,
            ComputerType logonServerType,
            ComputerType clientType,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            uint extraFlags);

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
        HRESULT NetrLogonSamLogonWithFlags(
            ComputerType logonServerType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            uint extraFlags);

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
        HRESULT NetrLogonSamLogon(
            ComputerType logonServerType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel);

        /// <summary>
        /// The NetrLogonSamLogoff method handles logoff requests for the SAM accounts.
        /// </summary>
        /// <param name="logonServerType">The type of SUT logged on.</param>
        /// <param name="clientType"> The type of the client calling this request.</param>
        /// <param name="authenticatorType">The type of authenticator.</param>
        /// <param name="isReturnAuthenticatorNull">Whether the ReturnAuthenticator passed in is NULL.</param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, 
        ///  that specifies the type of logon information passed in the LogonInformation parameter.
        /// </param>
        /// <param name="logonInformationType">The type of logon request information.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonSamLogoff(
            ComputerType logonServerType,
            ComputerType clientType,
            AuthenticatorType authenticatorType,
            bool isReturnAuthenticatorNull,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            LogonInformationType logonInformationType);

        #endregion

        #region Message Protection Methods

        /// <summary>
        /// The NetrLogonGetTrustRid method is used to obtain the RID of the account, and the account's password is used 
        /// by domain controllers in the specified domain to establish the secure channel.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonGetTrustRid(ComputerType sutType, DomainNameType domainType);

        /// <summary>
        ///  The NetrLogonComputeServerDigest method computes a cryptographic digest of a message 
        ///  by using the MD5 message-digest algorithm.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="ridType">The type of RID of the machine account for which the digest is to be computed.</param>
        /// <param name="messageType">The type of message to compute the digest.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonComputeServerDigest(
            ComputerType sutType,
            RidType ridType,
            MessageType messageType);

        /// <summary>
        ///  The NetrLogonComputeClientDigest method is used by a client to compute a cryptographic digest 
        ///  of a message by using the MD5 message-digest algorithm.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="domainType">The type of domain.</param>
        /// <param name="messageType">The type of message to compute the digest.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonComputeClientDigest(
            ComputerType sutType,
            DomainNameType domainType,
            MessageType messageType);

        /// <summary>
        ///  The NetrLogonSetServiceBits method is used to notify Netlogon whether 
        ///  a domain controller is running specified services.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="serviceBitsOfInterest"> 
        ///  A set of bit flags used as a mask to indicate
        ///  which service's state (running or not running) is being set by this call.
        /// </param>
        /// <param name="serviceBits">
        ///  A set of bit flags used as a mask to indicate whether 
        ///  the service indicated by ServiceBitsOfInterest is running or not. 
        /// </param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonSetServiceBits(
            ComputerType sutType,
            uint serviceBitsOfInterest,
            uint serviceBits);

        /// <summary>
        /// The NetrLogonGetTimeServiceParentDomain method returns the name of the parent domain of the current domain.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonGetTimeServiceParentDomain(ComputerType sutType);

        #endregion

        #region  Administrative Services Methods

        /// <summary>
        /// The NetrLogonControl2Ex method executes Windows-specific administrative actions 
        /// that pertain to the Netlogon SUT operation. 
        /// It is used to query the status and control the actions of the Netlogon SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="queryLevel">Information query level requested by the client.</param>
        /// <param name="dataType">The type of specific data required by the query.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonControl2Ex(
            ComputerType sutType,
            uint functionCode,
            uint queryLevel,
            NetlogonControlDataInformationType dataType);

        /// <summary>
        /// The NetrLogonControl2 method executes Windows-specific administrative actions 
        /// that pertain to the Netlogon SUT operation. 
        /// It is used to query the status and control the actions of the Netlogon SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="queryLevel">Information query level requested by the client.</param>
        /// <param name="dataType">The type of specific data required by the query.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonControl2(
            ComputerType sutType,
            uint functionCode,
            uint queryLevel,
            NetlogonControlDataInformationType dataType);

        /// <summary>
        /// The NetrLogonControl method executes Windows-specific administrative actions 
        /// that pertain to the Netlogon SUT operation. 
        /// It is used to query the status and control the actions of the Netlogon SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <param name="functionCode">The control operation to be performed.</param>
        /// <param name="queryLevel">Information query level requested by the client.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonControl(
            ComputerType sutType,
            uint functionCode,
            uint queryLevel);

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
        HRESULT DsrEnumerateDomainTrusts(ComputerType sutType, uint flags);

        /// <summary>
        /// The NetrEnumerateTrustedDomainsEx method returns a list of trusted domains from a specified SUT.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrEnumerateTrustedDomainsEx(ComputerType sutType);

        /// <summary>
        /// The NetrEnumerateTrustedDomains method returns a set of trusted domain names.
        /// </summary>
        /// <param name="sutType">The type of the SUT computer receiving the request.</param>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrEnumerateTrustedDomains(ComputerType sutType);

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
        HRESULT DsrGetForestTrustInformation(
            ComputerType sutType,
            DomainNameType trustedDomainName,
            uint flags);

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
        HRESULT NetrServerGetTrustInfo(
            ComputerType trustedDcType,
            AccounterNameType accountType,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ComputerType clientType,
            bool isValidAuthenticatorUsed);

        #endregion

        #region Obsolete Methods

        /// <summary>
        /// The NetrLogonUasLogoff method is for the support of LAN Manager products 
        /// and should be rejected with an error code.
        /// </summary>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonUasLogon();

        /// <summary>
        /// The NetrAccountDeltas method was for the support of LAN Manager products 
        /// and should be rejected with an error code.
        /// </summary>
        /// <returns>The method returns 0x00000000 if success; otherwise, it returns a nonzero error code.</returns>
        HRESULT NetrLogonUasLogoff();

        /// <summary>
        /// The NetrAccountDeltas method was for the support of LAN Manager products 
        /// and should be rejected with an error code.
        /// </summary>
        /// <returns>The Netlogon SUT returns STATUS_NOT_IMPLEMENTED.</returns>
        HRESULT NetrAccountDeltas();

        /// <summary>
        ///The NetrAccountSync method was for the support of LAN Manager products
        /// and SHOULD be rejected with an error code.
        /// </summary>
        /// <returns>The Netlogon SUT returns STATUS_NOT_IMPLEMENTED.</returns>
        HRESULT NetrAccountSync();

        #endregion
    }
}
