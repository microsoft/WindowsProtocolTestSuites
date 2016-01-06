// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    /// <summary>
    /// Samr client
    /// </summary>
    public class SamrClient : ISamrRpcAdapter
    {
        //actual rpc adapter
        private ISamrRpcAdapter rpc;

        #region Constructor
        /// <summary>
        /// Constructor of SamrClient
        /// </summary>
        public SamrClient()
        {
            rpc = new SamrRpcAdapter();
        }
        #endregion


        #region ISamrRpcAdapter methods

        /// <summary>
        /// Gets session key.
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                return rpc.SessionKey;
            }
        }

        /// <summary>
        /// RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpc.Handle;
            }
        }

        /// <summary>
        /// Bind to SAMR RPC server.
        /// </summary>
        /// <param name="protocolSequence">
        /// RPC protocol sequence.
        /// </param>
        /// <param name="networkAddress">
        /// RPC network address.
        /// </param>
        /// <param name="endpoint">
        /// RPC endpoint.
        /// </param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by under layer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Timeout
        /// </param>
        public void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            rpc.Bind(protocolSequence,
                            networkAddress,
                            endpoint,
                            transportCredential,
                            securityContext,
                            authenticationLevel,
                            timeout);
        }

        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            rpc.Unbind();
        }

        /// <summary>
        ///  The SamrConnect method returns a handle to a server
        ///  object. Opnum: 0 
        /// </summary>
        /// <param name="ServerName">
        ///  The first character of the NETBIOS name of the responder;
        ///  this parameter MAYServerName is ignored on receipt.
        ///   be ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle upon output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect(string ServerName, out System.IntPtr ServerHandle, uint DesiredAccess)
        {
            return rpc.SamrConnect(ServerName, out ServerHandle, DesiredAccess);
        }

        /// <summary>
        ///  The SamrCloseHandle method closes (that is, releases
        ///  server-side resources used by) any context handle obtained
        ///  from this RPC interface. Opnum: 1 
        /// </summary>
        /// <param name="SamHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  any context handle returned from this interface.
        /// </param>
        ///<returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCloseHandle(ref IntPtr SamHandle)
        {
            return rpc.SamrCloseHandle(ref SamHandle);
        }

        /// <summary>
        ///  The SamrSetSecurityObject method sets the access control
        ///  on a server, domain, user, group, or alias object.
        ///  Opnum: 2 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server, domain, user, group, or alias object.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bit field that indicates the fields of SecurityDescriptor
        ///  that are requested to be set. The SECURITY_INFORMATION
        ///  type is defined in [MS-DTYP] section. The following
        ///  bits are valid; all other bits MUST be zero on send
        ///  and ignored on receipt.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  A security descriptor expressing access that is specific
        ///  to the ObjectHandle.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetSecurityObject(System.IntPtr ObjectHandle,
            SecurityInformation_Values SecurityInformation,
            _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor)
        {
            return rpc.SamrSetSecurityObject(ObjectHandle,
                        SecurityInformation,
                        SecurityDescriptor);
        }

        /// <summary>
        ///  The SamrQuerySecurityObject method queries the access
        ///  control on a server, domain, user, group, or alias
        ///  object. Opnum: 3 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server, domain, user, group, or alias object.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bit field that specifies which fields of SecurityDescriptor
        ///  the client is requesting to be returned. The SECURITY_INFORMATION
        ///  type is defined in [MS-DTYP] section. The following
        ///  bits are valid; all other bits MUST be zero on send
        ///  and ignored on receipt.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  A security descriptor expressing accesses that are specific
        ///  to the ObjectHandle and the owner and group of the
        ///  object. [MS-DTYP] section  contains the specification
        ///  for a valid security descriptor.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQuerySecurityObject(System.IntPtr ObjectHandle,
            SamrQuerySecurityObject_SecurityInformation_Values SecurityInformation,
            out _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor)
        {
            return rpc.SamrQuerySecurityObject(ObjectHandle, SecurityInformation, out SecurityDescriptor);
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 4 
        /// </summary>
        public void Opnum4NotUsedOnWire()
        {
            rpc.Opnum4NotUsedOnWire();
        }

        /// <summary>
        ///  The SamrLookupDomainInSamServer method obtains the SID
        ///  of a domain object, given the object's name. Opnum
        ///  : 5 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="Name">
        ///  A UTF-16 encoded string.
        /// </param>
        /// <param name="DomainId">
        ///  A SID value of a domain that matches the Name passed
        ///  in. The match must be exact (no wildcards are permitted).
        ///  See message processing later in this section for more
        ///  details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrLookupDomainInSamServer(System.IntPtr ServerHandle,
            _RPC_UNICODE_STRING Name, out _RPC_SID? DomainId)
        {
            return rpc.SamrLookupDomainInSamServer(ServerHandle, Name, out DomainId);
        }

        /// <summary>
        ///  The SamrEnumerateDomainsInSamServer method obtains a
        ///  listing of all domains hosted by the server side of
        ///  this protocol. Opnum: 6 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A listing of domain information, as described in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateDomainsInSamServer(System.IntPtr ServerHandle,
            ref System.UInt32? EnumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out System.UInt32 CountReturned)
        {
            return rpc.SamrEnumerateDomainsInSamServer(ServerHandle,
                ref EnumerationContext, out Buffer, PreferedMaximumLength, out CountReturned);
        }

        /// <summary>
        ///  The SamrOpenDomain method obtains a handle to a domain
        ///  object, given a SID. Opnum: 7 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK. See section  for a list of domain access
        ///  values.
        /// </param>
        /// <param name="DomainId">
        ///  A SID value of a domain hosted by the server side of
        ///  this protocol.
        /// </param>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenDomain(System.IntPtr ServerHandle,
            uint DesiredAccess, _RPC_SID? DomainId, out System.IntPtr DomainHandle)
        {
            return rpc.SamrOpenDomain(ServerHandle, DesiredAccess, DomainId, out DomainHandle);
        }

        /// <summary>
        ///  The SamrQueryInformationDomain method obtains attributes
        ///  from a domain object. Opnum: 8 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  DomainInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationDomain(System.IntPtr DomainHandle,
            _DOMAIN_INFORMATION_CLASS DomainInformationClass,
            //[Switch("DomainInformationClass")]
            out _SAMPR_DOMAIN_INFO_BUFFER? Buffer)
        {
            return rpc.SamrQueryInformationDomain(DomainHandle, DomainInformationClass, out Buffer);
        }

        /// <summary>
        ///  The SamrSetInformationDomain method updates attributes
        ///  on a domain object. Opnum: 9 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a list of possible values.
        /// </param>
        /// <param name="DomainInformation">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationDomain(System.IntPtr DomainHandle,
            _DOMAIN_INFORMATION_CLASS DomainInformationClass,
            //[Switch("DomainInformationClass")] 
            _SAMPR_DOMAIN_INFO_BUFFER DomainInformation)
        {
            return rpc.SamrSetInformationDomain(DomainHandle, DomainInformationClass, DomainInformation);
        }

        /// <summary>
        ///  The SamrCreateGroupInDomain method creates a group object
        ///  within a domain. Opnum: 10 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the group. Details on
        ///  how this value maps to the data model are provided
        ///  later in this section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the GroupHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created group.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateGroupInDomain(System.IntPtr DomainHandle,
            _RPC_UNICODE_STRING Name,
            uint DesiredAccess,
            out System.IntPtr GroupHandle,
            out System.UInt32 RelativeId)
        {
            return rpc.SamrCreateGroupInDomain(DomainHandle, Name, DesiredAccess, out GroupHandle, out RelativeId);
        }

        /// <summary>
        ///  The SamrEnumerateGroupsInDomain method enumerates all
        ///  groups. Opnum: 11 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A list of group information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateGroupsInDomain(System.IntPtr DomainHandle,
            ref System.UInt32? EnumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out System.UInt32 CountReturned)
        {
            return rpc.SamrEnumerateGroupsInDomain(DomainHandle,
                ref EnumerationContext, out Buffer, PreferedMaximumLength, out CountReturned);
        }

        /// <summary>
        ///  The SamrCreateUserInDomain method creates a user. Opnum
        ///  : 12 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the user. See the message
        ///  processing shown later in this section for details
        ///  on how this value maps to the data model.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the UserHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created user.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateUserInDomain(System.IntPtr DomainHandle,
            _RPC_UNICODE_STRING Name,
            uint DesiredAccess,
            out System.IntPtr UserHandle,
            out System.UInt32 RelativeId)
        {
            return rpc.SamrCreateUserInDomain(DomainHandle, Name, DesiredAccess, out UserHandle, out RelativeId);
        }

        /// <summary>
        ///  The SamrEnumerateUsersInDomain method enumerates all
        ///  users. Opnum: 13 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="UserAccountControl">
        ///  A filter value to be used on the userAccountControl
        ///  attribute.
        /// </param>
        /// <param name="Buffer">
        ///  A list of user information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateUsersInDomain(System.IntPtr DomainHandle,
            ref System.UInt32? EnumerationContext,
            uint UserAccountControl,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out System.UInt32 CountReturned)
        {
            return rpc.SamrEnumerateUsersInDomain(DomainHandle,
                ref EnumerationContext, UserAccountControl, out Buffer, PreferedMaximumLength, out CountReturned);
        }

        /// <summary>
        ///  The SamrCreateAliasInDomain method creates an alias.
        ///  Opnum: 14 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="AccountName">
        ///  The value to use as the name of the alias. Details on
        ///  how this value maps to the data model are provided
        ///  later in this section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the AliasHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateAliasInDomain(System.IntPtr DomainHandle,
            _RPC_UNICODE_STRING AccountName,
            uint DesiredAccess,
            out System.IntPtr AliasHandle,
            out System.UInt32 RelativeId)
        {
            return rpc.SamrCreateAliasInDomain(DomainHandle,
                AccountName, DesiredAccess, out AliasHandle, out RelativeId);
        }

        /// <summary>
        ///  The SamrEnumerateAliasesInDomain method enumerates all
        ///  aliases. Opnum: 15 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A list of alias information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateAliasesInDomain(System.IntPtr DomainHandle,
            ref System.UInt32? EnumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out System.UInt32 CountReturned)
        {
            return rpc.SamrEnumerateAliasesInDomain(DomainHandle,
                ref EnumerationContext, out Buffer, PreferedMaximumLength, out CountReturned);
        }

        /// <summary>
        ///  The SamrGetAliasMembership method obtains the union
        ///  of all aliases that a given set of SIDs is a member
        ///  of. Opnum: 16 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="SidArray">
        ///  A list of SIDs.
        /// </param>
        /// <param name="Membership">
        ///  The union of all aliases (represented by RIDs) that
        ///  all SIDs in SidArray are a member of.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetAliasMembership(System.IntPtr DomainHandle,
            _SAMPR_PSID_ARRAY SidArray,
            out _SAMPR_ULONG_ARRAY Membership)
        {
            return rpc.SamrGetAliasMembership(DomainHandle, SidArray, out Membership);
        }

        /// <summary>
        ///  The SamrLookupNamesInDomain method translates a set
        ///  of account names into a set of RIDs. Opnum: 17 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Count">
        ///  The number of elements in Names. The maximum value of
        ///  1,000 is chosen to limit the amount of memory that
        ///  the client can force the server to allocate.
        /// </param>
        /// <param name="Names">
        ///  An array of strings that are to be mapped to RIDs.
        /// </param>
        /// <param name="RelativeIds">
        ///  An array of RIDs of accounts that correspond to the
        ///  elements in Names.
        /// </param>
        /// <param name="Use">
        ///  An array of SID_NAME_USE enumeration values that describe
        ///  the type of account for each entry in RelativeIds.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrLookupNamesInDomain(System.IntPtr DomainHandle,
            uint Count,
            //[Length("Count")] [Size("1000")] 
            _RPC_UNICODE_STRING[] Names,
            out _SAMPR_ULONG_ARRAY RelativeIds,
            out _SAMPR_ULONG_ARRAY Use)
        {
            return rpc.SamrLookupNamesInDomain(DomainHandle, Count, Names, out RelativeIds, out Use);
        }

        /// <summary>
        ///  The SamrLookupIdsInDomain method translates a set of
        ///  RIDs into account names. Opnum: 18 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Count">
        ///  The number of elements in RelativeIds. The maximum value
        ///  of 1,000 is chosen to limit the amount of memory that
        ///  the client can force the server to allocate.
        /// </param>
        /// <param name="RelativeIds">
        ///  An array of RIDs that are to be mapped to account names.
        /// </param>
        /// <param name="Names">
        ///  A structure containing an array of account names that
        ///  correspond to the elements in RelativeIds.
        /// </param>
        /// <param name="Use">
        ///  A structure containing an array of SID_NAME_USE enumeration
        ///  values that describe the type of account for each entry
        ///  in RelativeIds.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrLookupIdsInDomain(System.IntPtr DomainHandle,
            uint Count,
            //[Length("Count")] [Size("1000")] 
            uint[] RelativeIds,
            out _SAMPR_RETURNED_USTRING_ARRAY Names,
            out _SAMPR_ULONG_ARRAY Use)
        {
            return rpc.SamrLookupIdsInDomain(DomainHandle, Count, RelativeIds, out Names, out Use);
        }

        /// <summary>
        ///  The SamrOpenGroup method obtains a handle to a group,
        ///  given a RID. Opnum: 19 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of group
        ///  access values.
        /// </param>
        /// <param name="GroupId">
        ///  A RID of a group.
        /// </param>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenGroup(System.IntPtr DomainHandle,
            uint DesiredAccess, uint GroupId, out System.IntPtr GroupHandle)
        {
            return rpc.SamrOpenGroup(DomainHandle, DesiredAccess, GroupId, out GroupHandle);
        }

        /// <summary>
        ///  The SamrQueryInformationGroup method obtains attributes
        ///  from a group object. Opnum: 20 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="GroupInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationGroup(System.IntPtr GroupHandle,
            _GROUP_INFORMATION_CLASS GroupInformationClass,
            //[Switch("GroupInformationClass")]
            out _SAMPR_GROUP_INFO_BUFFER? Buffer)
        {
            return rpc.SamrQueryInformationGroup(GroupHandle, GroupInformationClass, out Buffer);
        }

        /// <summary>
        ///  The SamrSetInformationGroup method updates attributes
        ///  on a group object. Opnum: 21 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="GroupInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationGroup(System.IntPtr GroupHandle,
            _GROUP_INFORMATION_CLASS GroupInformationClass,
            //[Switch("GroupInformationClass")] 
            _SAMPR_GROUP_INFO_BUFFER Buffer)
        {
            return rpc.SamrSetInformationGroup(GroupHandle, GroupInformationClass, Buffer);
        }

        /// <summary>
        ///  The SamrAddMemberToGroup method adds a member to a group.
        ///  Opnum: 22 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID representing an account to add to the group's
        ///  membership list.
        /// </param>
        /// <param name="Attributes">
        ///  The characteristics of the membership relationship.
        ///  See section  for legal values and semantics.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrAddMemberToGroup(System.IntPtr GroupHandle, uint MemberId, uint Attributes)
        {
            return rpc.SamrAddMemberToGroup(GroupHandle, MemberId, Attributes);
        }

        /// <summary>
        ///  The SamrDeleteGroup method removes a group object. Opnum
        ///  : 23 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrDeleteGroup(ref System.IntPtr GroupHandle)
        {
            return rpc.SamrDeleteGroup(ref GroupHandle);
        }

        /// <summary>
        ///  The SamrRemoveMemberFromGroup method removes a member
        ///  from a group. Opnum: 24 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID representing an account to remove from the group's
        ///  membership list.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMemberFromGroup(System.IntPtr GroupHandle, uint MemberId)
        {
            return rpc.SamrRemoveMemberFromGroup(GroupHandle, MemberId);
        }

        /// <summary>
        ///  The SamrGetMembersInGroup method reads the members of
        ///  a group. Opnum: 25 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="Members">
        ///  A structure containing an array of RIDs, as well as
        ///  an array of attribute values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetMembersInGroup(System.IntPtr GroupHandle, out _SAMPR_GET_MEMBERS_BUFFER? Members)
        {
            return rpc.SamrGetMembersInGroup(GroupHandle, out Members);
        }

        /// <summary>
        ///  The SamrSetMemberAttributesOfGroup method sets the attributes
        ///  of a member relationship. Opnum: 26 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID that represents a member of a group (which is
        ///  a user or machine account).
        /// </param>
        /// <param name="Attributes">
        ///  The characteristics of the membership relationship.
        ///  For legal values, see section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetMemberAttributesOfGroup(System.IntPtr GroupHandle, uint MemberId, uint Attributes)
        {
            return rpc.SamrSetMemberAttributesOfGroup(GroupHandle, MemberId, Attributes);
        }

        /// <summary>
        ///  The SamrOpenAlias method obtains a handle to an alias,
        ///  given a RID. Opnum: 27 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of alias
        ///  access values.
        /// </param>
        /// <param name="AliasId">
        ///  A RID of an alias.
        /// </param>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenAlias(System.IntPtr DomainHandle,
            uint DesiredAccess, uint AliasId, out System.IntPtr AliasHandle)
        {
            return rpc.SamrOpenAlias(DomainHandle, DesiredAccess, AliasId, out AliasHandle);
        }

        /// <summary>
        ///  The SamrQueryInformationAlias method obtains attributes
        ///  from an alias object. Opnum: 28 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="AliasInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationAlias(System.IntPtr AliasHandle,
            _ALIAS_INFORMATION_CLASS AliasInformationClass,
            //[Switch("AliasInformationClass")] 
            out _SAMPR_ALIAS_INFO_BUFFER? Buffer)
        {
            return rpc.SamrQueryInformationAlias(AliasHandle, AliasInformationClass, out Buffer);
        }

        /// <summary>
        ///  The SamrSetInformationAlias method  updates attributes
        ///  on an alias object. Opnum: 29 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="AliasInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationAlias(System.IntPtr AliasHandle,
            _ALIAS_INFORMATION_CLASS AliasInformationClass,
            //[Switch("AliasInformationClass")] 
            _SAMPR_ALIAS_INFO_BUFFER Buffer)
        {
            return rpc.SamrSetInformationAlias(AliasHandle, AliasInformationClass, Buffer);
        }

        /// <summary>
        ///  The SamrDeleteAlias method removes an alias object.
        ///  Opnum: 30 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrDeleteAlias(ref System.IntPtr AliasHandle)
        {
            return rpc.SamrDeleteAlias(ref AliasHandle);
        }

        /// <summary>
        ///  The SamrAddMemberToAlias method adds a member to an
        ///  alias. Opnum: 31 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MemberId">
        ///  The SID of an account to add to the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrAddMemberToAlias(System.IntPtr AliasHandle, _RPC_SID MemberId)
        {
            return rpc.SamrAddMemberToAlias(AliasHandle, MemberId);
        }

        /// <summary>
        ///  The SamrRemoveMemberFromAlias method removes a member
        ///  from an alias. Opnum: 32 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MemberId">
        ///  The SID of an account to remove from the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMemberFromAlias(System.IntPtr AliasHandle, _RPC_SID MemberId)
        {
            return rpc.SamrRemoveMemberFromAlias(AliasHandle, MemberId);
        }

        /// <summary>
        ///  The SamrGetMembersInAlias method obtains the membership
        ///  list of an alias. Opnum: 33 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="Members">
        ///  A structure containing an array of SIDs that represent
        ///  the membership list of the alias referenced by AliasHandle.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetMembersInAlias(System.IntPtr AliasHandle, out _SAMPR_PSID_ARRAY Members)
        {
            return rpc.SamrGetMembersInAlias(AliasHandle, out Members);
        }

        /// <summary>
        ///  The SamrOpenUser method obtains a handle to a user,
        ///  given a RID. Opnum: 34 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of user
        ///  access values.
        /// </param>
        /// <param name="UserId">
        ///  A RID of a user account.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenUser(System.IntPtr DomainHandle,
            uint DesiredAccess, uint UserId, out System.IntPtr UserHandle)
        {
            return rpc.SamrOpenUser(DomainHandle, DesiredAccess, UserId, out UserHandle);
        }

        /// <summary>
        ///  The SamrDeleteUser method removes a user object. Opnum
        ///  : 35 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrDeleteUser(ref System.IntPtr UserHandle)
        {
            return rpc.SamrDeleteUser(ref UserHandle);
        }

        /// <summary>
        ///  The SamrQueryInformationUser method obtains attributes
        ///  from a user object. Opnum: 36 
        /// </summary>
        /// <param name="UserHandle">
        ///  UserHandle parameter.
        /// </param>
        /// <param name="UserInformationClass">
        ///  UserInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationUser(System.IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            //[Switch("UserInformationClass")] 
            out _SAMPR_USER_INFO_BUFFER? Buffer)
        {
            return rpc.SamrQueryInformationUser(UserHandle, UserInformationClass, out Buffer);
        }

        /// <summary>
        ///  The SamrSetInformationUser method updates attributes
        ///  on a user object. Opnum: 37 
        /// </summary>
        /// <param name="UserHandle">
        ///  UserHandle parameter.
        /// </param>
        /// <param name="UserInformationClass">
        ///  UserInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationUser(System.IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            //[Switch("UserInformationClass")]
            _SAMPR_USER_INFO_BUFFER Buffer)
        {
            return rpc.SamrSetInformationUser(UserHandle, UserInformationClass, Buffer);
        }

        /// <summary>
        ///  The SamrChangePasswordUser method changes the password
        ///  of a user object. Opnum: 38 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="LmPresent">
        ///  If this parameter is zero, the LmOldEncryptedWithLmNew
        ///  and LmNewEncryptedWithLmOld fields MUST be ignored
        ///  by the server; otherwise these fields MUST be processed.
        /// </param>
        /// <param name="OldLmEncryptedWithNewLm">
        ///  The LM hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the LM hash of the new password for the target
        ///  user (as presented by the client in the LmNewEncryptedWithLmOld
        ///  parameter).
        /// </param>
        /// <param name="NewLmEncryptedWithOldLm">
        ///  The LM hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_LM_OWF_PASSWORD, where the key is the
        ///  LM hash of the existing password for the target user
        ///  (as presented by the client in the LmOldEncryptedWithLmNew
        ///  parameter).
        /// </param>
        /// <param name="NtPresent">
        ///  If this parameter is zero, NtOldEncryptedWithNtNew and
        ///  NtNewEncryptedWithNtOld MUST be ignored by the server;
        ///  otherwise these fields MUST be processed. 
        /// </param>
        /// <param name="OldNtEncryptedWithNewNt">
        ///  The NT hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of  ENCRYPTED_NT_OWF_PASSWORD, where
        ///  the key is the NT hash of the new password for the
        ///  target user (as presented by the client).
        /// </param>
        /// <param name="NewNtEncryptedWithOldNt">
        ///  The NT hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_NT_OWF_PASSWORD, where the key is the
        ///  NT hash of the existing password for the target user
        ///  (as presented by the client).
        /// </param>
        /// <param name="NtCrossEncryptionPresent">
        ///  If this parameter is zero, NtNewEncryptedWithLmNew MUST
        ///  be ignored; otherwise, this field MUST be processed.
        /// </param>
        /// <param name="NewNtEncryptedWithNewLm">
        ///  The NT hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_NT_OWF_PASSWORD, where the key is the
        ///  LM hash of the new password for the target user (as
        ///  presented by the client).
        /// </param>
        /// <param name="LmCrossEncryptionPresent">
        ///  If this parameter is zero, LmNewEncryptedWithNtNew MUST
        ///  be ignored; otherwise, this field MUST be processed.
        /// </param>
        /// <param name="NewLmEncryptedWithNewNt">
        ///  The LM hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_LM_OWF_PASSWORD, where the key is the
        ///  NT hash of the new password for the target user (as
        ///  presented by the client).
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrChangePasswordUser(System.IntPtr UserHandle,
            byte LmPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? OldLmEncryptedWithNewLm,
            _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithOldLm,
            byte NtPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? OldNtEncryptedWithNewNt,
            _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithOldNt,
            byte NtCrossEncryptionPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithNewLm,
            byte LmCrossEncryptionPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithNewNt)
        {
            return rpc.SamrChangePasswordUser(UserHandle, LmPresent, OldLmEncryptedWithNewLm, NewLmEncryptedWithOldLm,
                NtPresent, OldNtEncryptedWithNewNt, NewNtEncryptedWithOldNt, NtCrossEncryptionPresent,
                NewNtEncryptedWithNewLm, LmCrossEncryptionPresent, NewLmEncryptedWithNewNt);
        }

        /// <summary>
        ///  The SamrGetGroupsForUser method obtains a listing of
        ///  groups that a user is a member of. Opnum: 39 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="Groups">
        ///  An array of RIDs of the groups that the user referenced
        ///  by UserHandle is a member of.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetGroupsForUser(System.IntPtr UserHandle, out _SAMPR_GET_GROUPS_BUFFER? Groups)
        {
            return rpc.SamrGetGroupsForUser(UserHandle, out Groups);
        }

        /// <summary>
        ///  The SamrQueryDisplayInformation method obtains a list
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 40 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <param name="EntryCount">
        ///  EntryCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  PreferredMaximumLength parameter.
        /// </param>
        /// <param name="TotalAvailable">
        ///  TotalAvailable parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  TotalReturned parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryDisplayInformation(System.IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            uint Index,
            uint EntryCount,
            uint PreferredMaximumLength,
            out System.UInt32 TotalAvailable,
            out System.UInt32 TotalReturned,
            //[Switch("DisplayInformationClass")]
            out _SAMPR_DISPLAY_INFO_BUFFER Buffer)
        {
            return rpc.SamrQueryDisplayInformation(DomainHandle, DisplayInformationClass, Index, EntryCount,
                PreferredMaximumLength, out TotalAvailable, out TotalReturned, out Buffer);
        }

        /// <summary>
        ///  The SamrGetDisplayEnumerationIndex method obtains an
        ///  index into an account-namesorted list of accounts.
        ///  Opnum: 41 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Prefix">
        ///  Prefix parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetDisplayEnumerationIndex(System.IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            _RPC_UNICODE_STRING Prefix,
            out System.UInt32 Index)
        {
            return rpc.SamrGetDisplayEnumerationIndex(DomainHandle, DisplayInformationClass, Prefix, out Index);
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 42 
        /// </summary>
        public void Opnum42NotUsedOnWire()
        {
            rpc.Opnum42NotUsedOnWire();
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 43 
        /// </summary>
        public void Opnum43NotUsedOnWire()
        {
            rpc.Opnum43NotUsedOnWire();
        }

        /// <summary>
        ///  The SamrGetUserDomainPasswordInformation method obtains
        ///  select password policy information (without requiring
        ///  a domain handle). Opnum: 44 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="PasswordInformation">
        ///  Password policy information from the user's domain.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetUserDomainPasswordInformation(System.IntPtr UserHandle,
            out _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation)
        {
            return rpc.SamrGetUserDomainPasswordInformation(UserHandle, out PasswordInformation);
        }

        /// <summary>
        ///  The SamrRemoveMemberFromForeignDomain method removes
        ///  a member from all aliases. Opnum: 45 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="MemberSid">
        ///  The SID to remove from the membership.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMemberFromForeignDomain(System.IntPtr DomainHandle, _RPC_SID? MemberSid)
        {
            return rpc.SamrRemoveMemberFromForeignDomain(DomainHandle, MemberSid);
        }

        /// <summary>
        ///  The SamrQueryInformationDomain2 method obtains attributes
        ///  from a domain object. Opnum: 46 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationDomain2(System.IntPtr DomainHandle,
            _DOMAIN_INFORMATION_CLASS DomainInformationClass,
            //[Switch("DomainInformationClass")] 
            out _SAMPR_DOMAIN_INFO_BUFFER? Buffer)
        {
            return rpc.SamrQueryInformationDomain2(DomainHandle, DomainInformationClass, out Buffer);
        }

        /// <summary>
        ///  The SamrQueryInformationUser2 method obtains attributes
        ///  from a user object. Opnum: 47 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="UserInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a list of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationUser2(System.IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            //[Switch("UserInformationClass")] 
            out _SAMPR_USER_INFO_BUFFER? Buffer)
        {
            return rpc.SamrQueryInformationUser2(UserHandle, UserInformationClass, out Buffer);
        }

        /// <summary>
        ///  The SamrQueryDisplayInformation2 method obtains a list
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 48 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <param name="EntryCount">
        ///  EntryCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  PreferredMaximumLength parameter.
        /// </param>
        /// <param name="TotalAvailable">
        ///  TotalAvailable parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  TotalReturned parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryDisplayInformation2(System.IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            uint Index,
            uint EntryCount,
            uint PreferredMaximumLength,
            out System.UInt32 TotalAvailable,
            out System.UInt32 TotalReturned,
            //[Switch("DisplayInformationClass")]
            out _SAMPR_DISPLAY_INFO_BUFFER Buffer)
        {
            return rpc.SamrQueryDisplayInformation2(DomainHandle,
                DisplayInformationClass,
                Index,
                EntryCount,
                PreferredMaximumLength,
                out TotalAvailable,
                out TotalReturned,
                out Buffer);
        }

        /// <summary>
        ///  The SamrGetDisplayEnumerationIndex2 method obtains an
        ///  index into an account-namesorted list of accounts,
        ///  such that the index is the position in the list of
        ///  the accounts whose account name best matches a client-provided
        ///  string. Opnum: 49 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  An enumeration indicating which set of objects to return
        ///  an index into (for a subsequent SamrQueryDisplayInformation3
        ///  method call).
        /// </param>
        /// <param name="Prefix">
        ///  A string matched against the account name to find a
        ///  starting point for an enumeration. The Prefix parameter
        ///  enables the client to obtain a listing of an account
        ///  from SamrQueryDisplayInformation3  such that the accounts
        ///  are returned in alphabetical order with respect to
        ///  their account name, starting with the account name
        ///  that most closely matches Prefix. See details later
        ///  in this section.
        /// </param>
        /// <param name="Index">
        ///  A value to use as input to SamrQueryDisplayInformation3
        ///   in order to control the accounts that are returned
        ///  from that method.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetDisplayEnumerationIndex2(System.IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            _RPC_UNICODE_STRING Prefix,
            out System.UInt32 Index)
        {
            return rpc.SamrGetDisplayEnumerationIndex2(DomainHandle, DisplayInformationClass, Prefix, out Index);
        }

        /// <summary>
        ///  The SamrCreateUser2InDomain method creates a user. Opnum
        ///  : 50 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the user. See the message
        ///  processing shown later in this section for details
        ///  on how this value maps to the data model.
        /// </param>
        /// <param name="AccountType">
        ///  A 32-bit value indicating the type of account to create.
        ///  See the message processing shown later in this section
        ///  for possible values.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the UserHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="GrantedAccess">
        ///  The access granted on UserHandle.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created user.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateUser2InDomain(System.IntPtr DomainHandle,
            _RPC_UNICODE_STRING? Name,
            uint AccountType,
            uint DesiredAccess,
            out System.IntPtr UserHandle,
            out System.UInt32 GrantedAccess,
            out System.UInt32 RelativeId)
        {
            return rpc.SamrCreateUser2InDomain(DomainHandle, Name,
                AccountType, DesiredAccess, out UserHandle, out GrantedAccess, out RelativeId);
        }

        /// <summary>
        ///  The SamrQueryDisplayInformation3 method obtains a listing
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 51 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  An enumeration (see section) that indicates the type
        ///  of accounts, as well as the type of attributes on the
        ///  accounts, to return via the Buffer parameter.
        /// </param>
        /// <param name="Index">
        ///  A cursor into an account-namesorted list of accounts.
        /// </param>
        /// <param name="EntryCount">
        ///  The number of accounts that the client is requesting
        ///  on output.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer;
        ///  this value overrides EntryCount if this value is reached
        ///  before EntryCount is reached.
        /// </param>
        /// <param name="TotalAvailable">
        ///  The number of bytes required to see a complete listing
        ///  of accounts specified by the DisplayInformationClass
        ///  parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  The number of bytes returned. This value is estimated
        ///  and is not accurate.  clients do not rely on the accuracy
        ///  of this value.
        /// </param>
        /// <param name="Buffer">
        ///  The accounts that are returned.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryDisplayInformation3(System.IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            uint Index,
            uint EntryCount,
            uint PreferredMaximumLength,
            out System.UInt32 TotalAvailable,
            out System.UInt32 TotalReturned,
            //[Switch("DisplayInformationClass")] 
            out _SAMPR_DISPLAY_INFO_BUFFER Buffer)
        {
            return rpc.SamrQueryDisplayInformation3(DomainHandle, DisplayInformationClass, Index,
                EntryCount, PreferredMaximumLength, out TotalAvailable, out TotalReturned, out Buffer);
        }

        /// <summary>
        ///  The SamrAddMultipleMembersToAlias method adds multiple
        ///  members to an alias. Opnum: 52 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MembersBuffer">
        ///  A structure containing a list of SIDs to add as members
        ///  to the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrAddMultipleMembersToAlias(System.IntPtr AliasHandle, _SAMPR_PSID_ARRAY? MembersBuffer)
        {
            return rpc.SamrAddMultipleMembersToAlias(AliasHandle, MembersBuffer);
        }

        /// <summary>
        ///  The SamrRemoveMultipleMembersFromAlias method removes
        ///  multiple members from an alias. Opnum: 53 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MembersBuffer">
        ///  A structure containing a list of SIDs to remove from
        ///  the alias's membership list.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMultipleMembersFromAlias(System.IntPtr AliasHandle, _SAMPR_PSID_ARRAY? MembersBuffer)
        {
            return rpc.SamrRemoveMultipleMembersFromAlias(AliasHandle, MembersBuffer);
        }

        /// <summary>
        ///  The SamrOemChangePasswordUser2 method changes a user's
        ///  password.  Opnum: 54 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ServerName">
        ///  A counted string, encoded in the OEM character set,
        ///  containing the NETBIOS name of the server; this parameter
        ///  MAY servers ignore the ServerName parameter.  be ignored
        ///  by the server.
        /// </param>
        /// <param name="UserName">
        ///  A counted string, encoded in the OEM character set,
        ///  containing the name of the user whose password is to
        ///  be changed; see message processing later in this section
        ///  for details on how this value is used as a database
        ///  key to locate the account that is the target of this
        ///  password change operation.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldLm">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the LM hash of the existing password for the target
        ///  user (as presented by the client). The clear text password
        ///  MUST be encoded in an OEM code page character set (as
        ///  opposed to UTF-16).
        /// </param>
        /// <param name="OldLmOwfPasswordEncryptedWithNewLm">
        ///  The LM hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the LM hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldLm (see
        ///  the preceding description for decryption details).
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOemChangePasswordUser2(System.IntPtr BindingHandle,
            _RPC_STRING ServerName,
            _RPC_STRING UserName,
            _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm,
            _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewLm)
        {
            return rpc.SamrOemChangePasswordUser2(BindingHandle, ServerName,
                UserName, NewPasswordEncryptedWithOldLm, OldLmOwfPasswordEncryptedWithNewLm);
        }

        /// <summary>
        ///  The SamrUnicodeChangePasswordUser2 method changes a
        ///  user account's password. Opnum: 55 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ServerName">
        ///  A null-terminated string containing the NETBIOS name
        ///  of the server; this parameter MAY servers ignore the
        ///  ServerName parameter.  be ignored by the server.
        /// </param>
        /// <param name="UserName">
        ///  The name of the user. See the message processing later
        ///  in this section for details on how this value is used
        ///  as a database key to locate the account that is the
        ///  target of this password change operation.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldNt">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the NT hash of the existing password for the target
        ///  user (as presented by the client in the OldNtOwfPasswordEncryptedWithNewNt
        ///  parameter). 
        /// </param>
        /// <param name="OldNtOwfPasswordEncryptedWithNewNt">
        ///  The NT hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the NT hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldNt.
        /// </param>
        /// <param name="LmPresent">
        ///  If this parameter is zero, NewPasswordEncryptedWithOldLm
        ///  and OldLmOwfPasswordEncryptedWithOldLm MUST be ignored;
        ///  otherwise these fields MUST be processed.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldLm">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the LM hash of the existing password for the target
        ///  user (as presented by the client).
        /// </param>
        /// <param name="OldLmOwfPasswordEncryptedWithNewNt">
        ///  The LM hash the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the NT hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldNt.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrUnicodeChangePasswordUser2(System.IntPtr BindingHandle,
            _RPC_UNICODE_STRING ServerName,
            _RPC_UNICODE_STRING UserName,
            _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldNt,
            _ENCRYPTED_LM_OWF_PASSWORD OldNtOwfPasswordEncryptedWithNewNt,
            byte LmPresent,
            _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm,
            _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewNt)
        {
            return rpc.SamrUnicodeChangePasswordUser2(BindingHandle, ServerName,
                UserName, NewPasswordEncryptedWithOldNt, OldNtOwfPasswordEncryptedWithNewNt,
                LmPresent, NewPasswordEncryptedWithOldLm, OldLmOwfPasswordEncryptedWithNewNt);
        }

        /// <summary>
        ///  The SamrGetDomainPasswordInformation method obtains
        ///  select password policy information (without authenticating
        ///  to the server). Opnum: 56 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="Unused">
        ///  A string value that is unused by the protocol. It is
        ///  ignored by the server. The client MAY clients set this
        ///  value to be the NULL-terminated NETBIOS name of the
        ///  server. set any value.
        /// </param>
        /// <param name="PasswordInformation">
        ///  Password policy information from the account domain.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetDomainPasswordInformation(System.IntPtr BindingHandle,
            _RPC_UNICODE_STRING Unused,
            out _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation)
        {
            return rpc.SamrGetDomainPasswordInformation(BindingHandle, Unused, out PasswordInformation);
        }

        /// <summary>
        ///  The SamrConnect2 method returns a handle to a server
        ///  object. Opnum: 57 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect2(string ServerName, out System.IntPtr ServerHandle, uint DesiredAccess)
        {
            return rpc.SamrConnect2(ServerName, out ServerHandle, DesiredAccess);
        }

        /// <summary>
        ///  The SamrSetInformationUser2 method updates attributes
        ///  on a user object. Opnum: 58 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="UserInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationUser2(System.IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            //[Switch("UserInformationClass")] 
            _SAMPR_USER_INFO_BUFFER Buffer)
        {
            return rpc.SamrSetInformationUser2(UserHandle, UserInformationClass, Buffer);
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 59 
        /// </summary>
        public void Opnum59NotUsedOnWire()
        {
            rpc.Opnum59NotUsedOnWire();
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 60 
        /// </summary>
        public void Opnum60NotUsedOnWire()
        {
            rpc.Opnum60NotUsedOnWire();
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 61 
        /// </summary>
        public void Opnum61NotUsedOnWire()
        {
            rpc.Opnum61NotUsedOnWire();
        }

        /// <summary>
        ///  The SamrConnect4 method obtains a handle to a server
        ///  object. Opnum: 62 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="ClientRevision">
        ///  Indicates the revision (for this protocol) of the client.
        ///  See the Revision field of SAMPR_REVISION_INFO_V1 for
        ///  possible values.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect4(string ServerName,
            out System.IntPtr ServerHandle, uint ClientRevision, uint DesiredAccess)
        {
            return rpc.SamrConnect4(ServerName, out ServerHandle, ClientRevision, DesiredAccess);
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 63 
        /// </summary>
        public void Opnum63NotUsedOnWire()
        {
            rpc.Opnum63NotUsedOnWire();
        }

        /// <summary>
        ///  The SamrConnect5 method obtains a handle to a server
        ///  object. Opnum: 64 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. For a listing of possible values,
        ///  see section.
        /// </param>
        /// <param name="InVersion">
        ///  Indicates which field of the InRevisionInfo union is
        ///  used.
        /// </param>
        /// <param name="InRevisionInfo">
        ///  Revision information. For details, see the definition
        ///  of the SAMPR_REVISION_INFO_V1 structure, which is contained
        ///  in the SAMPR_REVISION_INFO union.
        /// </param>
        /// <param name="OutVersion">
        ///  Indicates which field of the OutRevisionInfo union is
        ///  used.
        /// </param>
        /// <param name="OutRevisionInfo">
        ///  Revision information. For details, see the definition
        ///  of the SAMPR_REVISION_INFO_V1 structure, which is contained
        ///  in the SAMPR_REVISION_INFO union.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect5(string ServerName,
            uint DesiredAccess,
            uint InVersion,
            //[Switch("InVersion")] 
            SAMPR_REVISION_INFO InRevisionInfo,
            out System.UInt32 OutVersion,
            //[Switch("*OutVersion")] 
            out SAMPR_REVISION_INFO OutRevisionInfo,
            out System.IntPtr ServerHandle)
        {
            return rpc.SamrConnect5(ServerName, DesiredAccess, InVersion, InRevisionInfo, out OutVersion,
                out OutRevisionInfo, out ServerHandle);
        }

        /// <summary>
        ///  The SamrRidToSid method obtains the SID of an account,
        ///  given a RID. Opnum: 65 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section. The
        ///  message processing shown later in this section contains
        ///  details on which types of ObjectHandle are accepted
        ///  by the server.
        /// </param>
        /// <param name="Rid">
        ///  A RID of an account.
        /// </param>
        /// <param name="Sid">
        ///  The SID of the account referenced by Rid.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRidToSid(System.IntPtr ObjectHandle, uint Rid, out _RPC_SID? Sid)
        {
            return rpc.SamrRidToSid(ObjectHandle, Rid, out Sid);
        }

        /// <summary>
        ///  The SamrSetDSRMPassword method sets a local recovery
        ///  password. Opnum: 66 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="Unused">
        ///  A string value. This value is not used in the protocol
        ///  and is ignored by the server.
        /// </param>
        /// <param name="UserId">
        ///  A RID of a user account. See the message processing
        ///  later in this section for details on restrictions on
        ///  this value.
        /// </param>
        /// <param name="EncryptedNtOwfPassword">
        ///  The NT hash of the new password (as presented by the
        ///  client) encrypted according to the specification of
        ///  ENCRYPTED_NT_OWF_PASSWORD, where the key is the User ID.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetDSRMPassword(System.IntPtr BindingHandle, _RPC_UNICODE_STRING Unused,
            uint UserId, _ENCRYPTED_LM_OWF_PASSWORD EncryptedNtOwfPassword)
        {
            return rpc.SamrSetDSRMPassword(BindingHandle, Unused, UserId, EncryptedNtOwfPassword);
        }

        /// <summary>
        ///  The SamrValidatePassword method validates an application
        ///  password against the locally stored policy. Opnum :
        ///  67 
        /// </summary>
        /// <param name="Handle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ValidationType">
        ///  The password policy validation requested.
        /// </param>
        /// <param name="InputArg">
        ///  The password-related material to validate.
        /// </param>
        /// <param name="OutputArg">
        ///  The result of the validation.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrValidatePassword(System.IntPtr Handle,
            _PASSWORD_POLICY_VALIDATION_TYPE ValidationType,
            //[Switch("ValidationType")]
            _SAM_VALIDATE_INPUT_ARG InputArg,
            //[Switch("ValidationType")] 
            out _SAM_VALIDATE_OUTPUT_ARG? OutputArg)
        {
            return rpc.SamrValidatePassword(Handle, ValidationType, InputArg, out OutputArg);
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 68 
        /// </summary>
        public void Opnum68NotUsedOnWire()
        {
            rpc.Opnum68NotUsedOnWire();
        }

        /// <summary>
        ///  Reserved for local use. Opnum: 69 
        /// </summary>
        public void Opnum69NotUsedOnWire()
        {
            rpc.Opnum69NotUsedOnWire();
        }
        #endregion


        #region IDisposable Members
        /// <summary>
        /// Disposes rpc adapter.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (rpc != null)
                {
                    rpc.Dispose();
                    rpc = null;
                }
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~SamrClient()
        {
            Dispose(false);
        }
        #endregion
    }
}
