// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// This enumeration defines the possible values for server configuration
    /// whether it is DC or Non DC
    /// </summary>
    public enum ProtocolServerConfig
    {
        /// <summary>
        /// Server configuration is Domain Controller.
        /// </summary>
        DomainController,

        /// <summary>
        /// Server configuration is Non Domain Controller.
        /// </summary>
        NonDomainController,

        /// <summary>
        /// Server configuration is Read Only Domain Controller.
        /// </summary>
        ReadOnlyDomainController,

        /// <summary>
        /// Server configuration is Primary Domain Controller.
        /// </summary>
        PrimaryDomainController
    }

    /// <summary>
    /// This enumeration defines the Object type used in DeleteObject method.
    /// </summary>
    public enum ObjectEnum
    {
        /// <summary>
        /// The Object type is AccountObject.
        /// </summary>
        AccountObject,       

        /// <summary>
        /// The Object type is SecretObject.
        /// </summary>
        SecretObject,

        /// <summary>
        /// The Object type is TrustDomainObject.
        /// </summary>
        TrustDomainObject
    }

    /// <summary>
    /// This enumeration defines the possible values for setting access to anonymous requester 
    /// </summary>
    public enum AnonymousAccess
    {
        /// <summary>
        /// setting access to anonymous requester is Disable.
        /// </summary>
        Disable,

        /// <summary>
        /// setting access to anonymous requester is Enable.
        /// </summary>
        Enable,
    }

    /// <summary>
    /// This enumeration defines the possible values for RootDirectory field which
    /// is passed in OpenPolicy/OpenPolicy2 
    /// </summary>
    public enum RootDirectory
    {
        /// <summary>
        /// RootDirectory field which is passed in OpenPolicy/OpenPolicy2 is Null.
        /// </summary>
        Null,

        /// <summary>
        /// RootDirectory field which is passed in OpenPolicy/OpenPolicy2 is not Null.
        /// </summary>
        NonNull
    }

    /// <summary>
    /// This enumeration defines the possible values for the handle returned from the actions 
    /// OpenPolicy2 or OpenPolicy and Close 
    /// </summary>
    public enum Handle
    {
        /// <summary>
        /// the handle returned from the actions OpenPolicy2 or OpenPolicy and Close is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the handle returned from the actions OpenPolicy2 or OpenPolicy and Close is InValid.
        /// </summary>
        Invalid,

        /// <summary>
        /// the handle returned from the actions OpenPolicy2 or OpenPolicy and Close is Null.
        /// </summary>
        Null
    }

    /// <summary>
    /// This enumeration defines the possible values for return values  returned by the actions
    /// </summary>
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32",
        Justification = "Disable warning CA1028 because System.Int32. cannot match the enumeration design")]
    public enum ErrorStatus : uint
    {
        /// <summary>
        /// A process has requested access to an object, but has not been granted those access rights.
        /// </summary>
        AccessDenied = 0xC0000022,

        /// <summary>
        /// An invalid parameter was passed to a service or function.
        /// </summary>
        InvalidParameter = 0xC000000D,

        /// <summary>
        /// An invalid HANDLE was specified.
        /// </summary>
        InvalidHandle = 0xC0000008,

        /// <summary>
        /// The request is not supported.
        /// </summary>
        NotSupported = 0xC00000BB,

        /// <summary>
        /// This operation is not supported on a computer running Windows Server 2003 for Small Business Server.
        /// </summary>
        NotSupportedOnSBS = 0xC0000300,

        /// <summary>
        /// The requested operation is not implemented.
        /// </summary>
        NotImplemented = 0xC0000002,

        /// <summary>
        /// Object Name not found.
        /// </summary>
        ObjectNameNotFound = 0xC0000034,

        /// <summary>
        /// Object Name already exists.
        /// </summary>
        ObjectNameCollision = 0xC0000035,

        /// <summary>
        /// Insufficient system resources exist to complete the API.
        /// </summary>
        InsufficientResources = 0xC000009A,

        /// <summary>
        /// A specified privilege does not exist.
        /// </summary>
        NoSuchPrivilege = 0xC0000060,

        /// <summary>
        /// The specified Domain did not exist.
        /// </summary>
        NoSuchDomain = 0xC00000DF,

        /// <summary>
        /// A specified name string is too long for its intended use.
        /// </summary>
        NameTooLong = 0xC0000106,

        /// <summary>
        /// The requested operation requires a directory service, and none was available.
        /// </summary>
        DirectoryServiceRequired = 0xC00002B1,

        /// <summary>
        /// Indicates the Domain was in the wrong state to perform the desired operation.
        /// </summary>
        InvalidDomainState = 0xC00000DD,

        /// <summary>
        /// This operation is only allowed for the Primary Domain Controller of the domain.
        /// </summary>
        InvalidDomainRole = 0xC00000DE,

        /// <summary>
        /// The specified information class is not a valid information class for the specified object.
        /// </summary>
        InvalidInfoClass = 0xC0000003,

        /// <summary>
        /// Indicates the SID structure is not valid.
        /// </summary>
        InvalidSid = 0xC0000078,

        /// <summary>
        /// Indicates the SECURITY_DESCRIPTOR structure is not valid.
        /// </summary>
        InvalidSecurityDescr = 0xC0000079,

        /// <summary>
        /// This operation cannot be performed on the current domain.
        /// </summary>
        CurrentDomainNotAllowed = 0xC00002E9,

        /// <summary>
        /// No more entries are available from an enumeration operation.
        /// </summary>
        NoMoreEntries = 0x8000001A,

        /// <summary>
        /// Returned by enumeration APIs to indicate more information is available to successive calls.
        /// </summary>
        MoreEntries = 0x00000105,

        /// <summary>
        /// Operation is Success completed.
        /// </summary>
        Success = 0x00000000,

        /// <summary>
        /// The object was not found.
        /// </summary>
        NotFound = 0xC0000225,

        /// <summary>
        /// An invalid parameter was passed to a service or function as the first argument.
        /// </summary>
        InvalidParameter1 = 0xC00000EF,

        /// <summary>
        /// Indicates a revision number encountered or specified is not one known by the service. 
        /// It may be a more recent revision than the service is aware of.
        /// </summary>
        Unknown = 0xC0000058,

        /// <summary>
        /// Ther returned value is an Unknown Error.
        /// </summary>
        ErrorUnKnown,

        /// <summary>
        /// ImplementationSpecific retruned value.
        /// </summary>
        ImplementationSpecific
    }

    /// <summary>
    /// This enumeration defines the possible values for the policy information type 
    /// </summary>
    public enum InformationClass
    {
        /// <summary>
        /// Information about audit log.
        /// </summary>
        PolicyAuditLogInformation = 1,
        
        /// <summary>
        /// Auditing options.
        /// </summary> 
        PolicyAuditEventsInformation = 2,

        /// <summary>
        /// Primary domain information.
        /// </summary>
        PolicyPrimaryDomainInformation = 3,

        /// <summary>
        /// Obsolete information class.
        /// </summary>
        PolicyPdAccountInformation = 4,

        /// <summary>
        /// Account domain information.
        /// </summary>
        PolicyAccountDomainInformation = 5,

        /// <summary>
        /// Server role information.
        /// </summary>
        PolicyLsaServerRoleInformation = 6,

        /// <summary>
        /// Replica source information.
        /// </summary>
        PolicyReplicaSourceInformation = 7,

        /// <summary>
        /// Obsolete information class.
        /// </summary>
        PolicyDefaultQuotaInformation = 8,

        /// <summary>
        /// Obsolete information class.
        /// </summary>
        PolicyModificationInformation = 9,

        /// <summary>
        /// Audit log behavior.
        /// </summary>
        PolicyAuditFullSetInformation = 10,

        /// <summary>
        /// Audit log state.
        /// </summary>
        PolicyAuditFullQueryInformation = 11,

        /// <summary>
        /// DNS domain information.
        /// </summary>
        PolicyDnsDomainInformation = 12,

        /// <summary>
        /// DNS domain information.
        /// </summary>
        PolicyDnsDomainInformationInt = 13,

        /// <summary>
        /// Local account domain information.
        /// </summary>
        PolicyLocalAccountDomainInformation = 14,

        /// <summary>
        /// Machine account information.
        /// </summary>
        PolicyMachineAccountInformation = 15,

        /// <summary>
        /// Not used in this protocol. Present to mark the end of the enumeration.
        /// </summary>
        PolicyLastEntry = 16,

        /// <summary>
        /// Invalid InformationClass.
        /// </summary>
        Invalid = 0
    }

    /// <summary>
    /// This enumeration defines the possible values for the policy domain information type 
    /// </summary>
    public enum DomainInformationClass
    {
        /// <summary>
        /// Null DomainInformationClass .
        /// </summary>
        Null,

        /// <summary>
        /// PolicyDomainQualityOfServiceInformation constant .
        /// </summary>
        PolicyDomainQualityOfServiceInformation,

        /// <summary>
        /// PolicyDomainEfsInformation constant .
        /// </summary>
        PolicyDomainEfsInformation,

        /// <summary>
        /// PolicyDomainKerberosTicketInformation constant.
        /// </summary>
        PolicyDomainKerberosTicketInformation,

        /// <summary>
        /// Invalid DomainInformationClass .
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for the trusted information type 
    /// </summary>
    public enum TrustedInformationClass
    {
        /// <summary>
        /// Null Trusted Information.
        /// </summary>
        None = 0,

        /// <summary>
        /// TrustedDomainNameInformation constant .
        /// </summary>
        TrustedDomainNameInformation = 1,
        
        /// <summary>
        /// TrustedControllersInformation constant .
        /// </summary>
        TrustedControllersInformation = 2,

        /// <summary>
        /// TrustedPosixOffsetInformation constant .
        /// </summary>
        TrustedPosixOffsetInformation = 3,
        
        /// <summary>
        /// TrustedPasswordInformation constant .
        /// </summary>
        TrustedPasswordInformation = 4,

        /// <summary>
        /// TrustedDomainInformationBasic constant .
        /// </summary>
        TrustedDomainInformationBasic = 5,

        /// <summary>
        /// TrustedDomainInformationEx constant .
        /// </summary>
        TrustedDomainInformationEx = 6,

        /// <summary>
        /// TrustedDomainAuthInformation constant .
        /// </summary>
        TrustedDomainAuthInformation = 7,

        /// <summary>
        /// TrustedDomainFullInformation constant .
        /// </summary>
        TrustedDomainFullInformation = 8,

        /// <summary>
        /// TrustedDomainAuthInformationInternal constant .
        /// </summary>
        TrustedDomainAuthInformationInternal = 9,
        
        /// <summary>
        /// TrustedDomainFullInformationInternal constant .
        /// </summary>
        TrustedDomainFullInformationInternal = 10,
        
        /// <summary>
        /// TrustedDomainInformationEx2Internal constant .
        /// </summary>
        TrustedDomainInformationEx2Internal = 11,

        /// <summary>
        /// TrustedDomainFullInformation2Internal constant .
        /// </summary>
        TrustedDomainFullInformation2Internal = 12,        

        /// <summary>
        /// TrustedDomainSupportedEncryptionTypes constant .
        /// </summary>
        TrustedDomainSupportedEncryptionTypes = 13,

        /// <summary>
        /// Invalid TrustedInformationClass.
        /// </summary>
        Invalid = 14
    }

    /// <summary>
    /// This enumeration defines the possible values for the information returned from the actions 
    /// that query policy information 
    /// </summary>
    public enum PolicyInformation
    {
        /// <summary>
        /// the information returned from the actions query policy information is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the information returned from the actions query policy information is InValid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for the trusted domain information returned from the actions 
    /// that query trusted domain information
    /// </summary>
    public enum TrustedDomainInformation
    {
        /// <summary>
        /// the trusted domain information returned from query trusted domain information is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the trusted domain information returned from query trusted domain information is InValid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration is for validating the passed in account accountSid 
    /// </summary>
    public enum AccountSid
    {
        /// <summary>
        /// passed in account accountSid is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// passed in account accountSid is Invalid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for the account privileges returned from the action
    /// EnumeratePrivilegesAccount
    /// </summary>
    public enum AccountPrivilege
    {
        /// <summary>
        /// the account privileges returned from EnumeratePrivilegesAccount is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the account privileges returned from EnumeratePrivilegesAccount is InValid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for the system access right returned from the action 
    /// GetSystemAccessAccount 
    /// </summary>
    public enum SystemAccessAccount
    {
        /// <summary>
        /// the system access right returned from GetSystemAccessAccount is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the system access right returned from GetSystemAccessAccount is InValid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for the user rights returned from the action 
    /// EnumerateAccountRights 
    /// </summary>
    public enum userRight
    {
        /// <summary>
        /// the user rights returned from EnumerateAccountRights is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the user rights returned from EnumerateAccountRights is InValid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for the secret handle returned from the actions 
    /// CreateSecret/OpenSecret 
    /// </summary>
    public enum SecretHandle
    {
        /// <summary>
        /// the secret handle returned from CreateSecret/OpenSecret is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the secret handle returned from CreateSecret/OpenSecret is Invalid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration is for validating Cipher values passed in action SetSecret
    /// </summary>
    public enum CipherValue
    {
        /// <summary>
        /// Cipher values passed in action SetSecret is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// Cipher values passed in action SetSecret is Invalid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration is for validating encrypted values passed in action StorePrivateData and 
    /// defines possible values for the encrypted current and old values returned from the action
    /// QuerySecret and for the encrypted data returned from the action RetrievePrivateData
    /// </summary>
    public enum EncryptedValue
    {
        /// <summary>
        /// EncryptedValue is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// EncryptedValue is Invalid.
        /// </summary>
        Invalid,

        /// <summary>
        /// EncryptedValue is Null.
        /// </summary>
        Null
    }

    /// <summary>
    /// This enumeration defines possible values for the current and old values
    /// set time returned from the action QuerySecret 
    /// </summary>
    public enum ValueSetTime
    {
        /// <summary>
        /// current and old values set time returned from the action QuerySecret is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// current and old values set time returned from the action QuerySecret is Invalid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration is for validating the passed in Domain accountSid 
    /// </summary>
    public enum DomainSid
    {
        /// <summary>
        /// the passed in Domain accountSid is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the passed in Domain accountSid is Invalid. 
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines possible values for the forest trust information
    /// returned from the action QueryForestTrustInformation 
    /// </summary>
    public enum ForestTrustInfo
    {
        /// <summary>
        /// the forest trust information returned from the action QueryForestTrustInformation is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the forest trust information returned from the action QueryForestTrustInformation is InValid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration is for validating the passed in names 
    /// </summary>
    public enum ValidString
    {
        /// <summary>
        /// Passed in names is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// Passed in Names is Invalid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines possible values for forest funtional levels 
    /// </summary>
    public enum ForestFunctionalLevel
    {
        /// <summary>
        /// forest funtional levels is DS_BEHAVIOR_WIN2000
        /// </summary>
        DS_BEHAVIOR_WIN2000,

        /// <summary>
        /// forest funtional levels is DS_BEHAVIOR_WIN2003
        /// </summary>
        DS_BEHAVIOR_WIN2003,

        /// <summary>
        /// forest funtional levels is DS_BEHAVIOR_WIN2008
        /// </summary>
        DS_BEHAVIOR_WIN2008,

        /// <summary>
        /// forest funtional levels is DS_BEHAVIOR_WIN2008R2
        /// </summary>
        DS_BEHAVIOR_WIN2008R2
    }

    /// <summary>
    /// This enumeration defines possible values for privilege luid returned from 
    /// action LookUpPrivilegeValue and also for validating the passed in luid in action 
    /// LookUpPrivilegeName
    /// </summary>
    public enum PrivilegeLUID
    {
        /// <summary>
        /// Valid PrivilegeLuid 
        /// </summary>
        Valid,

        /// <summary>
        /// InValid PrivilegeLuid 
        /// </summary>
        Invalid,

        /// <summary>
        /// NotPresent PrivilegeLuid 
        /// </summary>
        NotPresentLuid
    }

    /// <summary>
    /// This enumeration defines possible values for collision information returned from 
    /// action SetForestTrustInfformation 
    /// </summary>
    public enum CollisionInfo
    {
        /// <summary>
        /// collision information returned from SetForestTrustInfformation is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// collision information returned from SetForestTrustInfformation is InValid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines possible values for trusted domain objects returned from 
    /// actions EnumerateTrustedDomainsEx and EnumerateTrustedDomains.
    /// </summary>
    public enum EnumBuffer
    {
        /// <summary>
        /// trusted domain objects returned from EnumerateTrustedDomainsEx and EnumerateTrustedDomains is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// trusted domain objects returned from EnumerateTrustedDomainsEx and EnumerateTrustedDomains is Valid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines possible values for security information passed in actions
    /// QuerySecurityObject and SetSecurityObject
    /// </summary>
    [FlagsAttribute]
    public enum SecurityInfo
    {
        /// <summary>
        /// Owner Portion of the  security descriptor.
        /// </summary>
        OwnerSecurityInformation = 0x00000001,

        /// <summary>
        /// Group portion of the security descriptor. 
        /// </summary>
        GroupSecurityInformation = 0x00000002,

        /// <summary>
        /// DACL portion of the security descriptor.
        /// </summary>
        DACLSecurityInformation = 0x00000004,

        /// <summary>
        /// SACL portion of the security descriptor.
        /// </summary>
        SACLSecurityInformation = 0x00000008
    }

    /// <summary>
    /// This enumeration defines possible values for security descriptor returned from action
    /// QuerySecurityObject and for validating security descriptor passed in action SetSecurityObject
    /// </summary>
    public enum SecurityDescriptor
    {
        /// <summary>
        /// SecurityDescriptor is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// SecurityDescriptor is Invalid.
        /// </summary>
        Invalid,

        /// <summary>
        /// SecurityDescriptor is Null.
        /// </summary>
        Null
    }

    /// <summary>
    /// This enumeration defines possible values different operating systems used in the model
    /// </summary>
    public enum Server
    {
        /// <summary>
        /// Windows Server NT, which is the SUT in our test scope.
        /// </summary>
        WindowsNT,

        /// <summary>
        /// Windows Server 2000, which is the SUT in our test scope.
        /// </summary>
        Windows2000,

        /// <summary>
        /// Windows Server SmallBusinessServer2k3, which is the SUT in our test scope.
        /// </summary>
        SmallBusinessServer2k3,

        /// <summary>
        /// Windows Server 2k3, which is the SUT in our test scope.
        /// </summary>
        Windows2k3,

        /// <summary>
        /// Windows Server 2k3r2, which is the SUT in our test scope.
        /// </summary>
        Windows2k3r2,

        /// <summary>
        /// Windows Server XP, which is the SUT in our test scope.
        /// </summary>
        WindowsXP,

        /// <summary>
        /// Windows Server 2k8, which is the SUT in our test scope.
        /// </summary>
        Windows2k8,

        /// <summary>
        /// Windows Server Vista, which is the SUT in our test scope.
        /// </summary>
        WindowsVista,

        /// <summary>
        /// Windows Server 2k8r2, which is the SUT in our test scope.
        /// </summary>
        Windows2k8r2,

        /// <summary>
        /// Windows Server 7, which is the SUT in our test scope.
        /// </summary>
        Windows7,

        /// <summary>
        /// Windows Server 2k12, which is the SUT in our test scope.
        /// </summary>
        Windows2k12,
        /// <summary>
        /// Windows Server 2k12r2, which is the SUT in our test scope.
        /// </summary>
        Windows2k12r2,

        /// <summary>
        /// Windows Server v1803, which is the SUT in our test scope.
        /// </summary>
        WindowsV1803,
        /// <summary>
        /// NonWindows Platform.
        /// </summary>
        NonWindows
    }

    /// <summary>
    /// This enumeration defines possible value for the type of reponse.
    /// </summary>
    public enum enumerateResponse
    {
        /// <summary>
        /// Enumerate All privileges known to the system.
        /// </summary>
        EnumerateAll,

        /// <summary>
        /// Enumerate some privileges known to the system.
        /// </summary>
        EnumerateSome,

        /// <summary>
        /// Enumerate none privileges known to the system.
        /// </summary>
        EnumerateNone
    }

    /// <summary>
    /// This enumeration defines possible value for the type of logon right.
    /// </summary>
    public enum TypeOfLogonRight
    {
        /// <summary>
        /// SeInteractiveLogonRight LogonRight
        /// </summary>
        SeInteractiveLogonRight,

        /// <summary>
        /// SeNetworkLogonRight LogonRight
        /// </summary>
        SeNetworkLogonRight,

        /// <summary>
        /// SeBatchLogonRight LogonRight
        /// </summary>
        SeBatchLogonRight,

        /// <summary>
        /// SeServiceLogonRight LogonRight
        /// </summary>
        SeServiceLogonRight,

        /// <summary>
        /// SeProxyLogonRight LogonRight
        /// </summary>
        SeProxyLogonRight,

        /// <summary>
        /// SeDenyInteractiveLogonRight LogonRight
        /// </summary>
        SeDenyInteractiveLogonRight,

        /// <summary>
        /// SeDenyNetworkLogonRight LogonRight
        /// </summary>
        SeDenyNetworkLogonRight,

        /// <summary>
        /// SeDenyBatchLogonRight LogonRight
        /// </summary>
        SeDenyBatchLogonRight,

        /// <summary>
        /// SeDenyServiceLogonRight LogonRight
        /// </summary>
        SeDenyServiceLogonRight,

        /// <summary>
        /// SeRemoteInteractiveLogonRight LogonRight
        /// </summary>
        SeRemoteInteractiveLogonRight,

        /// <summary>
        /// SeDenyRemoteInteractiveLogonRight LogonRight
        /// </summary>
        SeDenyRemoteInteractiveLogonRight,
    }

    /// <summary>
    /// This enumeration defines possible value for the type of SID.
    /// </summary>
    public enum TypeOfSID
    {
        /// <summary>
        /// the type of SID : NewSID.
        /// </summary>
        NewSID,

        /// <summary>
        /// the type of SID : UnKnownSID.
        /// </summary>
        UnKnownSID,

        /// <summary>
        /// the type of SID : ExistSID.
        /// </summary>
        ExistSID
    }

    /// <summary>
    /// This enumeration defines possible value for the type of user right.
    /// </summary>
    public enum TypeOfUserRight
    {        
        /// <summary>
        /// the type of user right : Valid
        /// </summary>
        ValidUserRight,

        /// <summary>
        /// the type of user right : InValid
        /// </summary>
        InvalidUserRight,

        /// <summary>
        /// the type of user right : NoPrivilegeWithAccount 
        /// </summary>
        NoPrivilegeWithAccount
    }

    /// <summary>
    /// This enumeration defines possible value for the result of check name.
    /// </summary>
    public enum ResOfNameChecked
    {
        /// <summary>
        /// the result of check name is Valid.
        /// </summary>
        Valid,

        /// <summary>
        /// the result of check name is Invalid.
        /// </summary>
        InValid,

        /// <summary>
        /// the result of check name is Already. 
        /// </summary>
        Already,

        /// <summary>
        /// the result of check name is TooLong.
        /// </summary>
        TooLong,

        /// <summary>
        /// the result of check name is NotPresent.
        /// </summary>
        NotPresent,

        /// <summary>
        /// the result of check name is Local system.
        /// </summary>
        LocalSystem,

        /// <summary>
        /// the result of check name is GlobalSecretName.
        /// </summary>
        GlobalSecretName
    }
    
    /// <summary>
    /// This enumeration defines possible value for the type of PrivilegeType.
    /// </summary>
    public enum PrivilegeType
    {
        /// <summary>
        /// Valid Privilege Type. 
        /// </summary>
        Valid,

        /// <summary>
        /// Invalid Privilege Type.
        /// </summary>
        InValid,

        /// <summary>
        /// NoSuchPrivilege Privilege Type.
        /// </summary>
        NoSuchPrivilege
    }

    /// <summary>
    /// This enumeration defines possible value for the type of account right.
    /// </summary>
    public enum AccountRight
    {
        /// <summary>
        /// type of account right SeAuditPrivilege
        /// </summary>
        SeAuditPrivilege,

        /// <summary>
        /// type of account right SeChangeNotifyPrivilege
        /// </summary>
        SeChangeNotifyPrivilege,

        /// <summary>
        /// type of account right SeImpersonatePrivilege
        /// </summary>
        SeImpersonatePrivilege,

        /// <summary>
        /// type of account right SeCreateGlobalPrivilege
        /// </summary>
        SeCreateGlobalPrivilege
    }

    /// <summary>
    /// This enumeration defines possible value for the type of domain.
    /// </summary>
    public enum DomainType
    {
        /// <summary>
        /// A Valid Domain Name.
        /// </summary>
        ValidDomainName,

        /// <summary>
        /// The Domain is the Current Domain.
        /// </summary>
        CurrentDomain,

        /// <summary>
        /// No such Domain Name.
        /// </summary>
        NoDomainName,

        /// <summary>
        /// The Domain is Domain Controller Name
        /// </summary>
        DCName,

        /// <summary>
        /// The Domain is a new DomainName for setDomainInfo.
        /// </summary>
        newDomainNameforSetDomainInfo,

        /// <summary>
        /// The Domain is Decret Name to Check.
        /// </summary>
        SecretNameToCheck
    }

    /// <summary>
    /// This enumeration defines possible value for the type of direction.
    /// </summary>
    public enum DirectionType
    {
        /// <summary>
        /// The direction Type is Incoming.
        /// </summary>
        Incoming,

        /// <summary>
        /// The direction Type is IncomingPrev.
        /// </summary>
        IncomingPrev,

        /// <summary>
        /// The direction Type is Outgoing.
        /// </summary>
        Outgoing,

        /// <summary>
        /// The direction Type is OutgoingPrev.
        /// </summary>
        OutgoingPrev
    }

    /// <summary>
    /// This structure defines possible values for trusted domain object
    /// information passed in for many trusted domain object actions
    /// </summary>
    public struct TRUSTED_DOMAIN_INFORMATION_EX
    {
        /// <summary>
        /// Value to store the Trusted Domain Object Name.
        /// </summary>
        public string TrustDomainName;

        /// <summary>
        /// Value to store the Trusted Domain Object SID.
        /// </summary>
        public string TrustDomain_Sid;

        /// <summary>
        /// Value to store the Trusted Domain Object NetBiosName.
        /// </summary>
        public string TrustDomain_NetBiosName;

        /// <summary>
        /// Value to store the Trusted Domain Object Type.
        /// </summary>
        public uint TrustType;

        /// <summary>
        /// Value to store the Trusted Domain Object Directory.
        /// </summary>
        public uint TrustDir;

        /// <summary>
        /// Value to store the Trusted Domain Object Attribute.
        /// </summary>
        public uint TrustAttr;
    }

    /// <summary>
    /// This structure defines possible values for trusted domain object
    /// authentication information passed in for some trusted domain object actions
    /// </summary>
    public struct TRUSTED_DOMAIN_AUTH_INFORMATION
    {
        /// <summary>
        /// trusted domain object authentication information passed in for some trusted domain object actions is 
        /// IncomingAuthInfo.
        /// </summary>
        public uint IncomingAuthInfos;

        /// <summary>
        /// trusted domain object authentication information passed in for some trusted domain object actions is 
        /// OutgoingAuthInfos.
        /// </summary>
        public uint OutgoingAuthInfos;
    }

    /// <summary>
    /// This enumeration defines possible value for the type of trust direction.
    /// </summary>
    public enum TrustDirection 
    { 
        /// <summary>
        /// The trusted is Disabled.
        /// </summary>
        DISABLED = 0x00000000,

        /// <summary>
        /// The trusted is Inbound.
        /// </summary>       
        INBOUND = 0x00000001,
        
        /// <summary>
        /// The trusted is Outbound.
        /// </summary>
        OUTBOUND = 0x00000002,
        
        /// <summary>
        /// The trusted is Bidirectional.
        /// </summary>
        BIDIRECTIONAL = 0x00000003
    }

    /// <summary>
    /// This enumeration defines structure of Trusted Domain Object Information.
    /// </summary>
    public struct TDOInformation
    {
        /// <summary>
        /// Value to store if the Trusted Domain Object Support Forest Information.
        /// </summary>
        public bool doesTdoSupportForestInformation;

        /// <summary>
        /// Value to store the Trusted Domain Object Handle Number.
        /// </summary>
        public int intTdoHandleNumber;

        /// <summary>
        /// Value to store if the forest information is Present.
        /// </summary>
        public bool isForestInformationPresent;

        /// <summary>
        /// Value to store the Domain SID.
        /// </summary>
        public string strDomainSid;

        /// <summary>
        /// Value to store the DNS Name.
        /// </summary>
        public string strTdoDnsName;

        /// <summary>
        /// Value to store the NetBiosName.
        /// </summary>
        public string strTdoNetBiosName;

        /// <summary>
        /// Value to store the Trusted Domain Object Desired Access Mask.
        /// </summary>
        public uint uintTdoDesiredAccess;

        /// <summary>
        /// Value to store the Trusted Domain Object Attribute.
        /// </summary>
        public uint uintTrustAttr;

        /// <summary>
        /// Value to store the Trusted Domain Object Directory.
        /// </summary>
        public uint uintTrustDir;

        /// <summary>
        /// Value to store the Trusted Domain Object Type.
        /// </summary>
        public uint uintTrustType;
    }

    /// <summary>
    /// This enumeration defines possible value for the type of SID authority.
    /// </summary>
    public enum Value_Values
    {
        /// <summary>
        ///  The  authority is the NULL SID authority. It  defines
        ///  only the NULL  well-known-SID: S-1-0-0.
        /// </summary>
        NULL_SID_AUTHORITY = 0x00,

        /// <summary>
        ///  The authority is the World  SID authority. It only defines
        ///  the Everyone well-known-SID: S-1-1-0.
        /// </summary>
        WORLD_SID_AUTHORITY = 0x01,

        /// <summary>
        ///  The authority is the Local  SID authority. It defines
        ///  only the Local  well-known-SID: S-1-2-0.
        /// </summary>
        LOCAL_SID_AUTHORITY = 0x02,

        /// <summary>
        ///  The authority is the Creator SID authority. It defines
        ///  the Creator Owner, Creator Group, and Creator  Owner
        ///  Server well-known-SIDs: S-1-3-0, S-1-3-1, and S-1-3-2.
        ///  These SIDs  are used as placeholders in an access control
        ///  list (ACL) and are replaced by the  user, group, and
        ///  machine SIDs of the security principal.
        /// </summary>
        CREATOR_SID_AUTHORITY = 0x03,

        /// <summary>
        ///  Not used.
        /// </summary>
        NON_UNIQUE_AUTHORITY = 0x04,

        /// <summary>
        ///  The authority is the  security subsystem SID authority.
        ///  It defines all  other SIDs in the forest.
        /// </summary>
        NT_AUTHORITY = 0x05,
    }

     /// <summary>
    /// This enumeration defines the status of the Active Directory Domain Control on DC
    /// </summary>
    public enum ServiceStatus
    {
        /// <summary>
        ///  the status of the Active Directory Domain Control on DC is Running
        /// </summary>
        Started,

        /// <summary>
        ///  the status of the Active Directory Domain Control on DC is Stopped.
        /// </summary>
        Stopped,
    }
}