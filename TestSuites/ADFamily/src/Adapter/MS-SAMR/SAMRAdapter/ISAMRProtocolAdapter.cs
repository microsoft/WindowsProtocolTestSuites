// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    /// <summary>
    /// Interface for the SAMR protocol Adapter.
    /// </summary>
    public interface ISAMRProtocolAdapter : IAdapter
    {
        /// <summary>
        /// Create SAMR Bind to the server.
        /// </summary>
        /// <param name="serverName">Variable of type string that represents the name of the server.</param>
        /// <param name="domainName">Variable of type string that represents the name of the domain.</param>
        /// <param name="userName">Variable of type string that represents the name of the logon user.</param>
        /// <param name="password">Variable of type string that represents the password of the logon user.</param>
        /// <param name="needSessionKey">Specify whether the SMB session key is required.</param>
        /// <param name="isDCConfig">Specify whether DC is configured or not.</param>
        void SamrBind(
            string serverName,
            string domainName,
            string userName,
            string password,
            bool needSessionKey,
            bool isDCConfig);

        /// <summary>
        /// SamrConnect5 method obtains a handle to a server object.
        /// </summary>
        /// <param name="serverName">Variable of type string that Lockout name of the server for which handle needs to be obtained.</param>
        /// <param name="desiredAccess">Variable of type uint ServerHandleAccess which specify the type of access on server handle.</param>
        /// <param name="serverHandle">Out parameter of type pointer HANDLE that Lockout the server handle.</param>
        /// <param name="inVersion">Variable of type uint inVersion specify the type of Revision information.</param>
        /// <param name="revision">Variable of type uint revision specify the param of Revision information.</param>
        /// <param name="supportedFeatures">Variable of type uint SupportFeatures with specify the type of features the server supported.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrConnect5(
            string serverName,
            uint desiredAccess,
            out IntPtr serverHandle,
            uint inVersion,
            uint revision,
            uint supportedFeatures);

        /// <summary>
        /// SamrLookupDomainInSamServer method obtains the SID of a domain object, given its name.
        /// </summary>
        /// <param name="serverHandle">Variable of type pointer HANDLE that Lockout the server handle.</param>
        /// <param name="domainNameToLookUp">Variable of string domainNameToLookUp which indicate that the domain name to look up.</param>
        /// <param name="domainSid">Out parameter of type _RPS_SID domainSid that Specify the SID of domain.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrLookupDomainInSamServer(
            IntPtr serverHandle,
            string domainNameToLookUp,
            out _RPC_SID domainSid);

        /// <summary>
        /// SamrOpenDomain method obtains a handle to a domain object, given a SID
        /// </summary>
        /// <param name="serverHandle">Variable of type pointer HANDLE that Lockout the server handle.</param>
        /// <param name="desiredAccess">Variable of type uint DomainHandleAccess which specify the type of access on the domain handle that is to be opened.</param>
        /// <param name="domainSid">Variable of type _RPC_SID DomainSid which specify the domain that is to be opened.</param>
        /// <param name="domainHandle">Out parameter of type pointer HANDLE representing the opened domain handle.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrOpenDomain(
            IntPtr serverHandle,
            uint desiredAccess,
            _RPC_SID domainSid,
            out IntPtr domainHandle);

        /// <summary>
        /// SamrLookupNamesInDomain translates a set of account names into a set of RIDs.
        /// </summary>
        /// <param name="domainHandle">Variable of type pointer HANDLE that Lockout the domain handle.</param>
        /// <param name="accountNames">Variable of type List that Lockout the set of account names that are to be translated into set of RIDs.</param>
        /// <param name="accountRids">Out parameter of type List RID that Specify the RID of the accounts</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrLookupNamesInDomain(
            IntPtr domainHandle,
            List<string> accountNames,
            out List<uint> accountRids);

        /// <summary>
        /// SamrOpenGroup method obtains a handle to a group, given a RID.
        /// </summary>
        /// <param name="domainHandle">Variable of type pointer HANDLE that Lockout the domain handle.</param>
        /// <param name="desiredAccess">Variable of type uint GroupHandleAccess which Lockout the type of access on the group handle that is being opened.</param>
        /// <param name="groupRid">Variable of type uint that specify the rid of the group that is to be opened.</param>
        /// <param name="groupHandle">Out parameter of type pointer HANDLE representing the group handle that is opened.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrOpenGroup(
            IntPtr domainHandle,
            uint desiredAccess,
            uint groupRid,
            out IntPtr groupHandle);

        /// <summary>
        /// The SamrEnumerateGroupsInDomain method enumerates all groups.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="enumerationContext">To initiate a new enumeration the client sets EnumerationContext to zero. 
        /// Otherwise the client sets EnumerationContext to a value returned by a previous call to the method. </param>
        /// <param name="buffer">A list of group information.</param>
        /// <param name="preferedMaximumLength">The requested maximum number of bytes to return in Buffer.</param>
        /// <param name="countReturned">The count of domain elements returned in Buffer.</param>
        /// <returns></returns>
        HRESULT SamrEnumerateGroupsInDomain(IntPtr domainHandle,
            ref uint? enumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? buffer,
            uint preferedMaximumLength,
            out uint countReturned);

        /// <summary>
        /// The SamrQueryInformationGroup method obtains attributes from a group object.
        /// </summary>
        /// <param name="groupHandle">An RPC context handle representing a group object.</param>
        /// <param name="groupInfoClass">An enumeration indicating whose attributes to obtain.</param>
        /// <param name="groupInfo">The requested attributes and values to be obtained.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryInformationGroup(
            IntPtr groupHandle,
            _GROUP_INFORMATION_CLASS groupInfoClass,
            out _SAMPR_GROUP_INFO_BUFFER? groupInfo);

        /// <summary>
        /// The SamrSetInformationGroup method updates attributes on a group object.
        /// </summary>
        /// <param name="groupHandle">An RPC context handle representing a group object.</param>
        /// <param name="groupInfoClass">An enumeration indicating which attributes to update.</param>
        /// <param name="groupInfo">The requested attributes and values to update.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrSetInformationGroup(IntPtr groupHandle, _GROUP_INFORMATION_CLASS groupInfoClass, _SAMPR_GROUP_INFO_BUFFER groupInfo);

        /// <summary>
        /// SamrOpenUser method obtains a handle to a user, given a RID.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="desiredAccess">An ACCESS_MASK that indicates the requested access for the returned handle.</param>
        /// <param name="userRid">A RID of a user account.</param>
        /// <param name="userHandle">An RPC context handle representing the user handle that is opened.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrOpenUser(
            IntPtr domainHandle,
            uint desiredAccess,
            uint userRid,
            out IntPtr userHandle);

        /// <summary>
        /// The SamrCreateUser2InDomain method creates a user.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="userName">The value to use as the name of the user.</param>
        /// <param name="accountType">A 32-bit value indicating the type of account to create.</param>
        /// <param name="desiredAccess">The access requested on the UserHandle on output.</param>
        /// <param name="userHandle">An RPC context handle representing the created user object.</param>
        /// <param name="grantedAccess">The access granted on UserHandle.</param>
        /// <param name="relativeId">The RID of the newly created user.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrCreateUser2InDomain(
            IntPtr domainHandle,
            string userName,
            uint accountType,
            uint desiredAccess,
            out IntPtr userHandle,
            out uint grantedAccess,
            out uint relativeId);

        /// <summary>
        /// The SamrCreateUserInDomain method creates a user.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="userName">The value to use as the name of the user.</param>
        /// <param name="desiredAccess">The access requested on the UserHandle on output.</param>
        /// <param name="userHandle">An RPC context handle representing the created user object.</param>
        /// <param name="relativeId">The RID of the newly created user.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrCreateUserInDomain(
                IntPtr domainHandle,
                string userName,
                uint desiredAccess,
                out IntPtr userHandle,
                out uint relativeId);

        /// <summary>
        /// The SamrSetInformationUser2 method queries attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="userInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryInformationUser2(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, out _SAMPR_USER_INFO_BUFFER? userInfo);

        /// <summary>
        /// The SamrSetInformationUser method queries attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="userInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryInformationUser(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, out _SAMPR_USER_INFO_BUFFER? userInfo);

        /// <summary>
        /// The SamrGetGroupsForUser method obtains a listing of groups that a user is a member of.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="groups">An array of RIDs of the groups that the user referenced by UserHandle is a member of.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrGetGroupsForUser(IntPtr userHandle, out _SAMPR_GET_GROUPS_BUFFER? groups);

        /// <summary>
        /// The SamrRidToSid method obtains the SID of an account, given a RID.
        /// </summary>
        /// <param name="objectHandle">An RPC context handle.</param>
        /// <param name="rid">A RID of an account.</param>
        /// <param name="sid">The SID of the account referenced by Rid.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrRidToSid(IntPtr objectHandle, uint rid, out _RPC_SID? sid);

        /// <summary>
        /// The SamrSetInformationUser2 method updates attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to update.</param>
        /// <param name="userInfo">The requested attributes and values to update.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrSetInformationUser2(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, _SAMPR_USER_INFO_BUFFER userInfo);

        /// <summary>
        /// The SamrSetInformationUser method updates attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to update.</param>
        /// <param name="userInfo">The requested attributes and values to update.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrSetInformationUser(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, _SAMPR_USER_INFO_BUFFER userInfo);

        /// <summary>
        /// The SamrDeleteUser method removes a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object to be deleted.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrDeleteUser(ref IntPtr userHandle);

        /// <summary>
        /// The SamrGetDisplayEnumerationIndex2 method obtains an index into an ascending account-name–sorted list of accounts, 
        /// such that the index is the position in the list of the accounts whose account name best matches a client-provided string.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="displayClass">An enumeration indicating which set of objects to return an index into.</param>
        /// <param name="prefix">A string matched against the account name to find a starting point for an enumeration.</param>
        /// <param name="index">A value to use as input to SamrQueryDisplayInformation3 in order to control the accounts that are returned from that method.</param>
        /// <returns></returns>
        HRESULT SamrGetDisplayEnumerationIndex2(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            string prefix,
            out uint index);

        /// <summary>
        /// The SamrGetDisplayEnumerationIndex method obtains an index into an ascending account-name–sorted list of accounts, 
        /// such that the index is the position in the list of the accounts whose account name best matches a client-provided string.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="displayClass">An enumeration indicating which set of objects to return an index into.</param>
        /// <param name="prefix">A string matched against the account name to find a starting point for an enumeration.</param>
        /// <param name="index">A value to use as input to SamrQueryDisplayInformation3 in order to control the accounts that are returned from that method.</param>
        /// <returns></returns>
        HRESULT SamrGetDisplayEnumerationIndex(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            string prefix,
            out uint index);

        /// <summary>
        /// The SamrQueryDisplayInformation3 method obtains a listing of accounts in ascending name-sorted order, starting at a specified index.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="displayClass">An enumeration that indicates the type of accounts, as well as 
        /// the type of attributes on the accounts, to return via the Buffer parameter.</param>
        /// <param name="index">A cursor into an account-name–sorted list of accounts.</param>
        /// <param name="entryCount">The number of accounts that the client is requesting on output.</param>
        /// <param name="preferedMaximumLength">The requested maximum number of bytes to return in Buffer; 
        /// this value overrides EntryCount if this value is reached before EntryCount is reached.</param>
        /// <param name="totalAvailable">The number of bytes required to see a complete listing of accounts specified by the DisplayInformationClass parameter.</param>
        /// <param name="totalReturned">The number of bytes returned.</param>
        /// <param name="buffer">The accounts that are returned.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryDisplayInformation3(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            uint index,
            uint entryCount,
            uint preferedMaximumLength,
            out uint totalAvailable,
            out uint totalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER buffer);

        /// <summary>
        /// The SamrQueryDisplayInformation2 method obtains a listing of accounts in ascending name-sorted order, starting at a specified index.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="displayClass">An enumeration that indicates the type of accounts, as well as 
        /// the type of attributes on the accounts, to return via the Buffer parameter.</param>
        /// <param name="index">A cursor into an account-name–sorted list of accounts.</param>
        /// <param name="entryCount">The number of accounts that the client is requesting on output.</param>
        /// <param name="preferedMaximumLength">The requested maximum number of bytes to return in Buffer; 
        /// this value overrides EntryCount if this value is reached before EntryCount is reached.</param>
        /// <param name="totalAvailable">The number of bytes required to see a complete listing of accounts specified by the DisplayInformationClass parameter.</param>
        /// <param name="totalReturned">The number of bytes returned.</param>
        /// <param name="buffer">The accounts that are returned.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryDisplayInformation2(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            uint index,
            uint entryCount,
            uint preferedMaximumLength,
            out uint totalAvailable,
            out uint totalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER buffer);

        /// <summary>
        /// The SamrQueryDisplayInformation method obtains a listing of accounts in ascending name-sorted order, starting at a specified index.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="displayClass">An enumeration that indicates the type of accounts, as well as 
        /// the type of attributes on the accounts, to return via the Buffer parameter.</param>
        /// <param name="index">A cursor into an account-name–sorted list of accounts.</param>
        /// <param name="entryCount">The number of accounts that the client is requesting on output.</param>
        /// <param name="preferedMaximumLength">The requested maximum number of bytes to return in Buffer; 
        /// this value overrides EntryCount if this value is reached before EntryCount is reached.</param>
        /// <param name="totalAvailable">The number of bytes required to see a complete listing of accounts specified by the DisplayInformationClass parameter.</param>
        /// <param name="totalReturned">The number of bytes returned.</param>
        /// <param name="buffer">The accounts that are returned.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryDisplayInformation(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            uint index,
            uint entryCount,
            uint preferedMaximumLength,
            out uint totalAvailable,
            out uint totalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER buffer);

        /// <summary>
        /// The SamrQuerySecurityObject method queries the access control on a server, domain, user, group, or alias object.
        /// </summary>
        /// <param name="objectHandle">An RPC context handle representing a server, domain, user, group, or alias object.</param>
        /// <param name="securityInformation">A bit field that specifies which fields of SecurityDescriptor the client is requesting to be returned.</param>
        /// <param name="securityDescriptor">A security descriptor expressing accesses that are specific to the ObjectHandle and the owner and group of the object.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQuerySecurityObject(IntPtr objectHandle,
                          SecurityInformation securityInformation,
                                              out _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor);

        /// <summary>
        /// The SamrSetSecurityObject method sets the access control on a server, domain, user, group, or alias object.
        /// </summary>
        /// <param name="objectHandle">An RPC context handle representing a server, domain, user, group, or alias object.</param>
        /// <param name="securityInformation">A bit field that specifies which fields of SecurityDescriptor the client is requesting to be set.</param>
        /// <param name="securityDescriptor">A security descriptor expressing accesses that are specific to the ObjectHandle and the owner and group of the object.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrSetSecurityObject(IntPtr objectHandle,
                          SecurityInformation securityInformation,
                                              _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor);

        /// <summary>
        /// The SamrQueryInformationDomain2 method queries attributes on a domain object.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="domainInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="domainInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryInformationDomain2(IntPtr domainHandle, _DOMAIN_INFORMATION_CLASS domainInfoClass, out _SAMPR_DOMAIN_INFO_BUFFER? domainInfo);

        /// <summary>
        /// The SamrQueryInformationDomain method queries attributes on a domain object.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="domainInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="domainInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrQueryInformationDomain(IntPtr domainHandle, _DOMAIN_INFORMATION_CLASS domainInfoClass, out _SAMPR_DOMAIN_INFO_BUFFER? domainInfo);

        /// <summary>
        /// SamrCloseHandle method closes any handle like server,domain,user, group or alias.
        /// </summary>
        /// <param name="samHandle">Variable of type pointer SAMHANDLE that represent either server,domain,group, alias or user.</param>
        /// <returns>HRESULT.</returns>
        HRESULT SamrCloseHandle(ref IntPtr samHandle);
    }
}
