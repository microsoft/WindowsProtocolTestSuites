// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Modeling;
    using Microsoft.Protocols.TestTools;

    #region Delegates

/// <summary>
/// Define the event ResponseHandle of enumerateResponse.
/// </summary>
    /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2</param>
    /// <param name="actionResponse">A pointer to a context value that is used to resume
    /// enumeration, if necessary</param>
    public delegate void ResponseHandler(int handleInput, enumerateResponse actionResponse);
   
    #endregion Delegates

    /// <summary>
    /// Define all actions of the interface ILsadManagedAdapter.
    /// </summary>
    public interface ILsadManagedAdapter : IAdapter
    {
        /// <summary>
        /// event ResponseHandler of EnumerateAccounts
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = @"Disable warning
        CA1009 because event handler methods with only two parameters (sende and e) cannot matchthe design of LSAD
        adapter codes which need special parameters for Model logic.")]
        event ResponseHandler EnumerateAccounts;

        /// <summary>
        /// event ResponseHandler of EnumerateTrustedDomainsEx
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = @"Disable warning
        CA1009 because event handler methods with only two parameters (sende and e) cannot match the design of LSAD
        adapter codes which need special parameters for Model logic.")]
        event ResponseHandler EnumerateTrustedDomainsEx;

        /// <summary>
        /// event ResponseHandler of EnumerateTrustedDomains
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = @"Disable warning
        CA1009 because event handler methods with only two parameters (sende and e) cannot match the design of LSAD
        adapter codes which need special parameters for Model logic.")]
        event ResponseHandler EnumerateTrustedDomains;

        /// <summary>
        /// event ResponseHandler of EnumeratePrivileges
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = @"Disable warning
        CA1009 because event handler methods with only two parameters (sende and e) cannot match the design of LSAD
        adapter codes which need special parameters for Model logic.")]
        event ResponseHandler EnumeratePrivileges;

        /// <summary>
        /// Gets current session key.
        /// </summary>
        /// <returns> Current session key </returns>
        byte[] SessionKey
        {
            get;
        }

        /// <summary>
        /// Return of Get SUT's OS version
        /// </summary>
        /// <param name="sutOSVersion">Represent server's platform type</param>
        /// <returns>The call must return true which indicates success</returns>
        bool GetSUTOSVersion(out Server sutOSVersion);

        /// <summary>
        /// The Initialize method is invoked to initialize the environment for server.
        /// </summary>
        /// <param name="serverConfig">Values for server configuration whether it is DC or Non DCalues</param>
        /// <param name="anonymousAccess">Values for setting access to anonymous requester
        /// that can be opened by OpenPolicy and OpenPolcy2 methods at any instant of time</param>
        /// <param name="windowsServer">SUT's OS version</param>
        /// <param name="noOfHandles">Specifies the maximum number of handles 
        /// that can be opened by OpenPolicy and OpenPolcy2 methods at any instant of time</param>
        /// <param name="isDomainAdmin">Set true if the user is Domain Admin, else set false</param>
        void Initialize(
            ProtocolServerConfig serverConfig, 
            AnonymousAccess anonymousAccess,
            Server windowsServer, 
            int noOfHandles,
            bool isDomainAdmin);

        /// <summary>
        /// This method is used to open a policy handle with required access.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value </param>
        /// <param name="desiredAccess">Contains the access to be given to the policyHandle</param>
        /// <param name="policyHandle">Output Parameter which contains Valid or Invalid or Null value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller is anonymous 
        /// and the Server is a non-DomainController</returns>
        ErrorStatus OpenPolicy2(
            RootDirectory rootDirectory, 
            uint desiredAccess, 
            out Handle policyHandle);

        /// <summary>
        /// This method is used to open a policy handle with required access.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value </param>
        /// <param name="desiredAccess">Contains the access to be given to the policyHandle</param>
        /// <param name="policyHandle">Output Parameter which contains Valid or Invalid or Null value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller is anonymous 
        /// and the Server is a non-DomainController</returns>
        ErrorStatus OpenPolicy(
            RootDirectory rootDirectory, 
            uint desiredAccess, 
            out Handle policyHandle);

        /// <summary>
        /// This method is used to set policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be set</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid
        /// Returns NotImplemented if the passed in information type cannot be set</returns>
        ErrorStatus SetInformationPolicy2(
            int handleInput, 
            InformationClass informationType);

        /// <summary>
        /// This method is used to set policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be set</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid
        /// Returns NotImplemented if the passed in information type cannot be set</returns>
        ErrorStatus SetInformationPolicy(
            int handleInput, 
            InformationClass informationType);

        /// <summary>
        /// This method is used to query policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be queried</param>
        /// <param name="policyInformation">Output Parameter which contains policy object information 
        /// which has been queried</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid</returns>
        ErrorStatus QueryInformationPolicy2(
            int handleInput, 
            InformationClass informationType, 
            out PolicyInformation policyInformation);

        /// <summary>
        /// This method is used to query policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be queried</param>
        /// <param name="policyInformation">Output Parameter which contains policy object information 
        /// which has been queried</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid</returns>
        ErrorStatus QueryInformationPolicy(
            int handleInput, 
            InformationClass informationType, 
            out PolicyInformation policyInformation);

        /// <summary>
        /// This method is used to set policy object domain information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object domain information to be set</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid</returns>
        ErrorStatus SetDomainInformationPolicy(
            int handleInput, 
            DomainInformationClass informationType);

        /// <summary>
        /// This method is used to query policy object domain information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object domain information to be queried</param>
        /// <param name="policyInformation">Output Parameter which contains policy object domain information 
        /// which has been queried</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid
        /// Returns ObjectNameNotFound if no value has been set for policy object domain information.</returns>
        ErrorStatus QueryDomainInformationPolicy(
            int handleInput, 
            DomainInformationClass informationType, 
            out PolicyInformation policyInformation);

        /// <summary>
        /// The CreateAccount method is invoked to create a new account object in the server's database.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="desiredAccess">Contains the access to be given to the account Handle</param>
        /// <param name="sid">It is to check that passed in account sid is valid</param>
        /// <param name="accountSid">Contains the account sid to be created</param>
        /// <param name="accountHandle">Outparam which contains valid or invalid account handle</param>
        /// <returns>Result of create account</returns>
        ErrorStatus CreateAccount(
            int handleInput, 
            uint desiredAccess, 
            AccountSid sid, 
            string accountSid, 
            out Handle accountHandle);

        /// <summary>
        /// The OpenAccount method is invoked to obtain a handle to an account object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="securityDescrAllows">Contains the access to be given to the account Handle</param>
        /// <param name="sid">It is to check that passed in account sid is valid</param>
        /// <param name="accountSid">Contains the account sid to be opened</param>
        /// <param name="accountHandle">Outparam which contains valid or invalid account handle</param>
        /// <returns>Result of open account</returns>
        ErrorStatus OpenAccount(
            int handleInput, 
            bool securityDescrAllows, 
            AccountSid sid, 
            string accountSid, 
            out Handle accountHandle);

        /// <summary>
        /// The EnumeratePrivilegesAccount method is invoked to retrieve a list of privileges 
        /// granted to an account on the server.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="privileges">Outparam which contains valid or invalid 
        /// privileges enumerated from an account object</param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in account handle is not valid</returns>
        ErrorStatus EnumeratePrivilegesAccount(
            int handleInput, 
            out Set<AccountPrivilege> privileges);

        /// <summary>
        /// The AddPrivilegesToAccount method is invoked to add new privileges to an existing account object.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="privilege">Contains the set of privileges to be added to an account object</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in account handle is not valid</returns>
        ErrorStatus AddPrivilegesToAccount(
            int handleInput, 
            Set<string> privilege);

        /// <summary>
        /// The RemovePrivilegesFromAccount method is invoked to remove privileges from an account object.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="allPrivileges">If this parameter is not FALSE, all privileges 
        /// will be stripped from the account object.</param>
        /// <param name="privilege">Contains the set of privileges to be removed from an account object </param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in account handle is not valid</returns>
        ErrorStatus RemovePrivilegesFromAccount(
            int handleInput, 
            bool allPrivileges, 
            Set<string> privilege);

        /// <summary>
        /// The GetSystemAccessAccount method is invoked to retrieve system access account flags 
        /// for an account object. System access account flags are described as part of the account object 
        /// data model, as specified in section.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="accessAccount">Out param which contains the set of system access rights to be retrieved
        /// from an account object </param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in account handle is not valid</returns>
        ErrorStatus GetSystemAccessAccount(
            int handleInput,
            out SystemAccessAccount accessAccount);

        /// <summary>
        /// The SetSystemAccessAccount method is invoked to set system access account flags 
        /// for an account object.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="systemAccess">Contains the set of system access rights to be 
        /// set to an account object </param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in account handle is not valid</returns>
        ErrorStatus SetSystemAccessAccount(
            int handleInput,
            uint systemAccess);

        /// <summary>
        /// The EnumerateAccountsWithUserRight method is invoked to return a list of account objects
        /// that have the user right equal to the passed-in value. 
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="userRight">Contains the user right of account object that is enumerated</param>
        /// <param name="right">It is for validation of passed in user right whether it is valid or invalid</param>
        /// <param name="enumerationBuffer">Out param which contains the list of account objects that have
        /// the user right equal to the passed-in value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchPrivilege if the supplied user right is not recognized by the server
        /// Returns NoMoreEntries if no account was found with the specified user right</returns>
        ErrorStatus EnumerateAccountsWithUserRight(
            int handleInput, 
            string userRight, 
            ValidString right, 
            out AccountSid enumerationBuffer);

        /// <summary>
        /// The EnumerateAccountRights method is invoked to retrieve a list of rights 
        /// associated with an existing account.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="sid">It is for validation of passed in account sid whether it is valid or invalid</param>
        /// <param name="accountSid">Contains account sid of an account object whose rights are retrieved</param>
        /// <param name="userRights">Out param which contains the list of rights associated 
        /// with an existing account</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameNotFound if the specified account object does not exist</returns>
        ErrorStatus EnumerateAccountRights(
            int handleInput, 
            AccountSid sid, 
            string accountSid,
            out userRight userRights);

        /// <summary>
        /// The AddAccountRights method is invoked to add new rights to an account object.  
        /// If the account object does not exist, the system will attempt to create one.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="accountSid">Contains account sid of an account object</param>
        /// <param name="sid">It is for validation of passed in account sid whether it is valid or invalid</param>
        /// <param name="accountRights">Contains the list of user rights to be added to an account object</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchPrivilege if user rights passed were not recognized</returns>
        ErrorStatus AddAccountRights(
            int handleInput, 
            string accountSid, 
            AccountSid sid, 
            Set<string> accountRights);

        /// <summary>
        /// The RemoveAccountRights method is invoked to remove rights from an account object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="accountSid">Contains account sid of an account object</param>
        /// <param name="sid">It is for validation of passed in account sid whether it is valid or invalid</param>
        /// <param name="allRights">If this field is not set to 0, all rights will be removed.</param>
        /// <param name="accountRights">Contains the list of user rights to be added to an account object</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchPrivilege if user rights passed were not recognized
        /// Returns ObjectNameNotFound if an account with passed in account sid does not exist
        /// Returns NotSupported if the operation is not supported by server</returns>
        ErrorStatus RemoveAccountRights(
            int handleInput, 
            string accountSid, 
            AccountSid sid,
            int allRights, 
            Set<string> accountRights);

        /// <summary>
        ///  The CreateSecret method is invoked to create a new secret object in the server's database.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="secretName">Contains the secret name of an secret object to be created</param>
        /// <param name="desiredAccess">Contains the access to be given to the secretHandle</param>
        /// <param name="secretHandle">Outparam which contains valid or invalid secret handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameCollision if passed in account sid already exists
        /// Returns NameTooLong if the length of specified secret name exceeds
        /// the maximum set by the server</returns>
        ErrorStatus CreateSecret(
            int handleInput,
            string secretName,
            uint desiredAccess,
            out SecretHandle secretHandle);

        /// <summary>
        ///  The OpenSecret method is invoked to obtain a handle to an existing secret object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="daclAllows">It is to check whether DACL allows the passed in desired access</param>
        /// <param name="secretName">Contains the secret name of an secret object to be opened</param>
        /// <param name="secretHandle">Outparam which contains valid or invalid secret handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameCollision if secret with the specified name was not found</returns>
        ErrorStatus OpenSecret(
            int handleInput,
            bool daclAllows,
            string secretName,
            out SecretHandle secretHandle);

        /// <summary>
        ///  The SetSecret method is invoked to set the current and old values of the secret object.
        /// </summary>
        /// <param name="handleInput">Contains secret handle obtained from CreateSecret/OpenSecret </param>
        /// <param name="currentValue">Contains current value that is to be set to an secret object</param>
        /// <param name="oldValue">Contains old value that is to be set to an secret object</param>
        /// <param name="isValueNull">If isValueNull is 1: parameter of currentvalue and old value are not null;
        /// if isValueNull is 2: parameter of currentvalue is null;
        /// if isValueNull is 3: parameter of oldvalue is null;</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in secret handle is not valid</returns>
        ErrorStatus SetSecret(
            int handleInput,
            CipherValue currentValue,
            CipherValue oldValue,
            int isValueNull);

        /// <summary>
        ///  The QuerySecret method is invoked to retrieve the current and old (or previous) value of
        ///  the secret object.
        /// </summary>
        /// <param name="handleInput">Contains secret handle obtained from CreateSecret/OpenSecret </param>
        /// <param name="encryptedCurrentValue">Out param which contains current value that 
        /// is to be retrieved from a secret object</param>
        /// <param name="currentValueSetTime">Out param which contains current value set time that 
        /// is to be retrieved from a secret object</param>
        /// <param name="encryptedOldValue">Out param which contains old value that 
        /// is to be retrieved from a secret object</param>
        /// <param name="oldValueSetTime">Out param which contains old value set time that 
        /// is to be retrieved from a secret object</param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in secret handle is not valid</returns>
        ErrorStatus QuerySecret(
            int handleInput,
            out EncryptedValue encryptedCurrentValue,
            out ValueSetTime currentValueSetTime,
            out EncryptedValue encryptedOldValue, 
            out ValueSetTime oldValueSetTime);

        /// <summary>
        ///  The StorePrivateData method is invoked to store a secret value.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="secretName">Contains Secret name of an secret object</param>
        /// <param name="encryptedData">Contains data that is to be stored in a secret object</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid</returns>
        ErrorStatus StorePrivateData(
            int handleInput,
            string secretName,
            EncryptedValue encryptedData);

        /// <summary>
        ///  The RetrievePrivateData method is invoked to retrieve a secret value.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="name">It is for validating the secret name passed whether it is valid or invalid</param>
        /// <param name="secretName">Contains Secret name of an secret object</param>
        /// <param name="encryptedData">Out param which contains data that is to be retrieved from a 
        /// secret object</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// returns ObjectNameNotFound if the specified secret name was not found</returns>
        ErrorStatus RetrievePrivateData(
            int handleInput,
            ValidString name,
            string secretName,
            out EncryptedValue encryptedData);

        /// <summary>
        ///  The OpenTrustedDomain method is invoked to
        ///  open a trusted domain object handle by supplying the
        ///  SID of the trusted domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainSid">Contains the sid of trusted domain object</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="daclAllows">It is for checking whether DACL allows the requested access</param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchDomain if the specified trusted domain does not exist</returns>
        ErrorStatus OpenTrustedDomain(
            int handleInput,
            string trustedDomainSid,
            DomainSid sid,
            bool daclAllows,
            out Handle trustHandle);

        /// <summary>
        ///  The CreateTrustedDomainEx2 method is invoked to
        ///  create a trusted domain object by supplying the
        ///  name,SID and authentication information for the trusted domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        /// Returns InvalidDomainState if the operation cannot complete
        /// in current state of the domain
        /// Returns DirectoryServiceRequired if the active directory is not available on the server
        /// Returns InvalidSid if the security identifier of the trusted domain is not valid
        /// Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        /// Returns ObjectNameCollision if another trusted domain object already exists
        /// that matches some of the identifying information of the supplied information.</returns>
        ErrorStatus CreateTrustedDomainEx2(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            ValidString name,
            DomainSid sid,
            ForestFunctionalLevel forestFuncLevel,
            bool isRootDomain,
            uint desiredAccess,
            out Handle trustHandle);

        /// <summary>
        ///  The CreateTrustedDomainEx method is invoked to
        ///  create a new trusted domain object by supplying
        ///  the Name and SID of the domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomaininfo">Contains the information of trusted domain object</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="authInfo">Contains the authentication information of trusted domain object</param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        /// Returns InvalidDomainState if the operation cannot complete
        /// in current state of the domain
        /// Returns DirectoryServiceRequired if the active directory is not available on the server
        /// Returns InvalidSid if the security identifier of the trusted domain is not valid
        /// Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        /// Returns ObjectNameCollision if another trusted domain object already exists
        /// that matches some of the identifying information of the supplied information.</returns>
        ErrorStatus CreateTrustedDomainEx(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomaininfo,
            ValidString name,
            DomainSid sid,
            ForestFunctionalLevel forestFuncLevel,
            bool isRootDomain,
            uint desiredAccess,
            TRUSTED_DOMAIN_AUTH_INFORMATION authInfo,
            out Handle trustHandle);

        /// <summary>
        ///  The CreateTrustedDomain method is invoked to create
        ///  an object of type trusted domain in the server's database.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        /// Returns InvalidDomainState if the operation cannot complete
        /// in current state of the domain
        /// Returns DirectoryServiceRequired if the active directory is not available on the server
        /// Returns InvalidSid if the security identifier of the trusted domain is not valid
        /// Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        /// Returns ObjectNameCollision if another trusted domain object already exists
        /// that matches some of the identifying information of the supplied information.</returns>
        ErrorStatus CreateTrustedDomain(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            DomainSid sid,
            bool isRootDomain,
            ValidString name,
            uint desiredAccess,
            out Handle trustHandle);

        /// <summary>
        ///  The OpenTrustedDomainByName method is invoked to
        ///  obtain a trusted domain object handle by supplying the
        ///  name of the trusted domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainName">Contains the name of trusted domain object</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameNotFound if trusted domain object by passed in name was not found</returns>
        ErrorStatus OpenTrustedDomainByName(
            int handleInput,
            string trustedDomainName,
            ValidString name,
            uint desiredAccess,
            out Handle trustHandle);

        /// <summary>
        ///  The QueryInfoTrustedDomain method is invoked to
        ///  retrieve information about the trusted domain object.
        ///  Identifies the Trusted Domain Object by an Open Trusted Domain Handle
        /// </summary>
        /// <param name="handleInput">Contains trust handle obtained from 
        /// CreateTrustedDomainEx2/CreateTrustedDomainEx/CreateTrustedDomain </param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustDomainInfo">Outparam which contains valid or invalid trusted
        /// domain object information</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns InvalidInfoClass if the InformationClass argument is outside
        /// the allowed range</returns>
        ErrorStatus QueryInfoTrustedDomain(
            int handleInput,
            TrustedInformationClass trustedInformation,
            ForestFunctionalLevel forestFuncLevel,
            out TrustedDomainInformation trustDomainInfo);

        /// <summary>
        ///  The SetInformationTrustedDomain method is invoked
        ///  to set information on a trusted domain object. Identifies
        ///  the Trusted Domain Object by an open Trusted Domain Handle.
        /// </summary>
        /// <param name="handleInput">Contains trust handle obtained from 
        /// CreateTrustedDomainEx2/CreateTrustedDomainEx/CreateTrustedDomain </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns InvalidDomainState if the domain is in the wrong state
        /// to do the stated operation</returns>
        ErrorStatus SetInformationTrustedDomain(
           int handleInput,
           TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
           ForestFunctionalLevel forestFuncLevel,
           TrustedInformationClass trustedInformation,
           bool isRootDomain);

        /// <summary>
        ///  The QueryTrustedDomainInfoByName method is invoked
        ///  to retrieve information about a trusted domain object.
        ///  Object is identified by its Domain Name.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainName">Contains the trusted domain object name</param>
        /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="trustDomainInfo">Out param which contains valid or invalid
        /// trusted domain information</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameNotFound if the trusted domain
        /// with specified name could not be found</returns>
        ErrorStatus QueryTrustedDomainInfoByName(
            int handleInput,
            string trustedDomainName,
            ValidString name,
            TrustedInformationClass trustedInformation,
            out TrustedDomainInformation trustDomainInfo);

        /// <summary>
        ///  The SetTrustedDomainInfoByName method is invoked to set information 
        ///  about a trusted domain object.
        ///  The Object is identified by its Domain name.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameNotFound if the trusted domain
        /// with specified name could not be found</returns>
        ErrorStatus SetTrustedDomainInfoByName(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            ValidString name,
            ForestFunctionalLevel forestFuncLevel,
            TrustedInformationClass trustedInformation,
            bool isRootDomain);

        /// <summary>
        ///  The QueryTrustedDomainInfo method is invoked to retrieve information 
        ///  on a trusted domain object. The Object is identified via the SID 
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="sid">It is for validating the trusted domain accountSid whether it is valid or invalid</param>
        /// <param name="trustedDomainSid">Contains the trusted domain object accountSid</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustDomainInfo">Out param which contains valid or invalid
        /// trusted domain information</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NotSupported if the specified information class is not supported
        /// Returns NoSuchDomain if specified trusted domain object does not exist</returns>
        ErrorStatus QueryTrustedDomainInfo(
            int handleInput,
            DomainSid sid,
            string trustedDomainSid,
            TrustedInformationClass trustedInformation,
            ForestFunctionalLevel forestFuncLevel,
            out TrustedDomainInformation trustDomainInfo);

        /// <summary>
        ///  The SetTrustedDomainInfo method is invoked to set information on a trusted domain object. 
        ///  The SID of the object is used to identify it.
        ///  In some cases, if the trusted domain object does not exist, it will be created. 
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="sid">It is for validating the trusted domain accountSid whether it is valid or invalid</param>        
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="authInfo">Contains the authentication information of
        /// trusted domain object information</param>
        /// <param name="desiredAccess">Contains the access required for trust handle</param>
        /// <param name="daclAllows">It is to check whether DACL allows the requested access</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchDomain if the specified trusted domain object
        /// does not exist
        /// Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        /// Returns InvalidDomainState if the operation cannot complete
        /// in current state of the domain
        /// Returns DirectoryServiceRequired if the active directory is not available on the server
        /// Returns InvalidSid if the security identifier of the trusted domain is not valid
        /// Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        /// Returns ObjectNameCollision if another trusted domain object already exists
        /// that matches some of the identifying information of the supplied information</returns>
        ErrorStatus SetTrustedDomainInfo(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            DomainSid sid,
            ForestFunctionalLevel forestFuncLevel,
            TrustedInformationClass trustedInformation,
            bool isRootDomain,
            TRUSTED_DOMAIN_AUTH_INFORMATION authInfo,
            uint desiredAccess,
            bool daclAllows);

        /// <summary>
        ///  The SetForestTrustInformation method is invoked to establish a trust 
        ///  relationship with another forest by attaching a set of records called the 
        ///  forest trust information to the trusted domain object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainName">Contains the trusted domain name</param>
        /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
        /// <param name="highestRecordType">Contains highest record type</param>
        /// <param name="recordCount">Contains record count</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>        
        /// <param name="collisionInfo">Out param which contains the collision information</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchDomain if the specified trusted domain object
        /// does not exist
        /// Returns InvalidDomainState if the operation cannot complete
        /// in current state of the domain
        /// Returns InvalidDomainRole if the server is not primary domain controller</returns>
         ErrorStatus SetForestTrustInformation(
             int handleInput,
             string trustedDomainName,
             ValidString name,
             int highestRecordType,
             int recordCount,
             ForestFunctionalLevel forestFuncLevel,
             bool isRootDomain,
             out CollisionInfo collisionInfo);

         /// <summary>
         ///  The QueryForestTrustInformation method is invoked to retrieve
         ///  forest information on a Trusted Domain Object.
         /// </summary>
         /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
         /// <param name="trustedDomainName">Contains the trusted domain name</param>
         /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
         /// <param name="highestRecordType">Contains highest record type</param>
         /// <param name="recordCount">Contains record count</param>
         /// <param name="forestFuncLevel">Contains forest functional levels</param>
         /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
         /// <param name="trustInfo">Out param which contains the trust information</param>
         /// <returns>Returns Success if the method is successful
         /// Returns InvalidParameter if the parameters passed to the method are not valid
         /// Returns AccessDenied if the caller does not have the permissions to 
         /// perform this operation
         /// Returns InvalidHandle if the passed in policy handle is not valid
         /// Returns NoSuchDomain if the specified trusted domain object
         /// does not exist
         /// Returns InvalidDomainState if the operation cannot complete
         /// in current state of the domain</returns>
        ErrorStatus QueryForestTrustInformation(
            int handleInput,
            string trustedDomainName,
            ValidString name,
            int highestRecordType,
            int recordCount,
            ForestFunctionalLevel forestFuncLevel,
            bool isRootDomain,
            out ForestTrustInfo trustInfo);

        /// <summary>
        ///  The DeleteTrustedDomain method is invoked to delete a trusted domain object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2</param>
        /// <param name="trustedDomainSid">Contains the sid of trusted domain object</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchDomain if the specified trusted domain does not exist</returns>
        ErrorStatus DeleteTrustedDomain(
            int handleInput,
            string trustedDomainSid,
            DomainSid sid);

        /// <summary>
        ///  The LookupPrivilegeValue method is invoked to map the name of a privilege 
        ///  into a locally unique identifier (luid) by which it is known on the server.
        ///  The locally unique value of the privilege can then be used in subsequent calls 
        ///  to other methods, such as LsarAddPrivilegesToAccount.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="name">It is for validating the privilege name passed in </param>
        /// <param name="privilegeName">Contains privilege name </param>
        /// <param name="luid">Out param contains valid or invalid 
        /// luid of the passed in privilege name </param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns InvalidParameter if one or more of the supplied parameters was invalid
        /// Returns NoSuchPrivilege if the privilege name is not recognized by the server</returns>
        ErrorStatus LookupPrivilegeValue(
            int handleInput,
            ValidString name,
            string privilegeName,
            out PrivilegeLUID luid);

        /// <summary>
        ///  The LookupPrivilegeName method is invoked to map the luid of a privilege 
        ///  into a string name by which it is known on the server.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="luid">It is for validating the privilege luid passed in </param>
        /// <param name="privilegeLuid">Contains privilege luid </param>
        /// <param name="privilegeName">Out param contains valid or invalid 
        /// privilegeName of the passed in privilege luid </param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns InvalidParameter if one or more of the supplied parameters was invalid
        /// Returns NoSuchPrivilege if the privilege luid is not recognized by the server</returns>
        ErrorStatus LookupPrivilegeName(
            int handleInput,
            PrivilegeLUID luid,
            string privilegeLuid,
            out ValidString privilegeName);

        /// <summary>
        ///  The LookupPrivilegeDisplayName method is invoked to map the name 
        ///  of a privilege into a display text string in the caller's language.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="name">It is for validating the privilege name passed in </param>
        /// <param name="privilegeName">Contains privilege name </param>
        /// <param name="displayName">Out param contains valid or invalid 
        /// display text of the passed in privilegename </param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns InvalidParameter if one or more of the supplied parameters was invalid
        /// Returns NoSuchPrivilege if the privilege luid is not recognized by the server</returns>
        ErrorStatus LookupPrivilegeDisplayName(
            int handleInput,
            ValidString name,
            string privilegeName,
            out ValidString displayName);

        /// <summary>
        ///  The QuerySecurityObject method is invoked to query security information that is assigned
        ///  to a database object. It returns the security descriptor of the object.
        /// </summary>
        /// <param name="handleInput">Contains any object handle </param>
        /// <param name="securityInfo">Contains security descriptor information </param>
        /// <param name="securityDescriptor">Out param contains valid or invalid 
        /// security descriptor </param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns NotSupported if the request is not supported</returns>
        ErrorStatus QuerySecurityObject(
            int handleInput,
            SecurityInfo securityInfo,
            out SecurityDescriptor securityDescriptor);

        /// <summary>
        ///  The SetSecurityObject method is invoked to set a security descriptor on an object.
        /// </summary>
        /// <param name="handleInput">Contains any object handle </param>
        /// <param name="securityInfo">Contains security descriptor information type</param>
        /// <param name="securityDescriptor">Contains security descriptor to be set</param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns NotSupported if the request is not supported for this object
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns InvalidSecurityDescr if the supplied security descriptor is invalid</returns>
        ErrorStatus SetSecurityObject(
            int handleInput,
            SecurityInfo securityInfo,
            SecurityDescriptor securityDescriptor);

        /// <summary>
        ///  The DeleteObject method is invoked to delete an open account object, 
        ///  secret object, or trusted domain object.
        /// </summary>
        /// <param name="handleInput">Contains any object handle </param>
        /// <param name="usedObject">Type of object which is be deleted</param>
        /// <param name="handleOutput">Out param which contains null handle value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns InvalidParameter if the parameters passed to the method are not valid</returns>
        ErrorStatus DeleteObject(
            int handleInput,
            ObjectEnum usedObject,
            out Handle handleOutput);

        /// <summary>
        ///  The Close method frees the resources held by a context handle 
        ///  that was opened earlier. After response, the context handle will no longer be usable
        ///  and any subsequent uses of this handle will fail.
        /// </summary>
        /// <param name="handleToBeClosed">Contains any object handle to be closed </param>
        /// <param name="handleAfterClose">Out param which contains null handle value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidHandle if the passed in is a valid object handle</returns>
        ErrorStatus Close(
            int handleToBeClosed,
            out Handle handleAfterClose);

        /// <summary>
        /// The EnumerateAccountsRequest method is invoked to enumerate the account request.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="enumerationContext">A pointer to a context value that is used to resume
        /// enumeration, if necessary</param>
        void EnumerateAccountsRequest(
            int handleInput,   
            int enumerationContext);

        /// <summary>
        /// The EnumerateTrustedDomainsExRequest method is invoked to enumerate the request from trust domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2</param>
        /// <param name="enumerationContext"> A pointer to a context value that is used to resume
        /// enumeration, if necessary.</param>
        void EnumerateTrustedDomainsExRequest(
            int handleInput, 
            int enumerationContext);

        /// <summary>
        /// The EnumerateTrustedDomainsRequest method is invoked to enumerate the request from trust domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2</param>
        /// <param name="enumerationContext"> A pointer to a context value that is used to resume
        /// enumeration, if necessary.</param>
        void EnumerateTrustedDomainsRequest(
            int handleInput, 
            int enumerationContext);

        /// <summary>
        /// The EnumeratePrivilegesRequest method is invoked to enumerate the privilege request.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="enumerationContext"> A pointer to a context value that is used to resume
        /// enumeration, if necessary.</param>
       void EnumeratePrivilegesRequest(
           int handleInput,
           int enumerationContext);
    }
}
