// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// LSA RPC interface
    /// </summary>   
    [CLSCompliant(false)]
    public partial interface ILsaRpcAdapter : IDisposable
    {
        /// <summary>
        /// Bind to LSA RPC server.
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
        /// used by underlayer transport (SMB/SMB2). 
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
        [CLSCompliant(false)]
        void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout);


        /// <summary>
        /// RPC unbind.
        /// </summary>
        void Unbind();


        /// <summary>
        /// RPC handle.
        /// </summary>
        IntPtr Handle
        {
            get;
        }


        /// <summary>
        ///  The LsarClose method frees the resources held by a context
        ///  handle that was opened earlier. After response, the
        ///  context handle is no longer usable, and any subsequent
        ///  uses of this handle MUST fail. Opnum: 0 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  The context handle to be freed.On response, it MUST be set to 0.
        /// </param>
        NtStatus LsarClose(ref System.IntPtr? ObjectHandle);


        /// <summary>
        ///  Opnum1NotUsedOnWire method. Opnum: 1 
        /// </summary>
        void Opnum1NotUsedOnWire();


        /// <summary>
        ///  Opnum5NotUsedOnWire method. Opnum: 5 
        /// </summary>
        void Opnum5NotUsedOnWire();


        /// <summary>
        ///  The LsarOpenPolicy method is exactly the same as LsarOpenPolicy2,
        ///  except that the SystemName parameter in this method
        ///  contains only one character instead of a full string.
        ///  This is because its syntactical definition lacks the
        ///  [string] RPC annotation present in LsarOpenPolicy2,
        ///  as specified in [C706]. RPC data types are specified
        ///  in [MS-RPCE] section 2.2.4.1.The SystemName parameter has
        ///  no effect on message processing in any environment.
        ///  It MUST be ignored. Opnum: 6 
        /// </summary>
        /// <param name="SystemName">
        ///  SystemName parameter does not have any effect on message 
        ///  processing in any environment. It MUST be ignored on receipt.
        /// </param>
        /// <param name="ObjectAttributes">
        ///  ObjectAttributes parameter does not have any effect on message 
        ///  processing in any environment. All fields MUST be ignored except 
        ///  RootDirectory which MUST be NULL.
        /// </param>
        /// <param name="DesiredAccess">
        ///  DesiredAccess parameter that specifies the requested access rights
        ///  that MUST be granted on the returned PolicyHandle if the request
        ///  is successful.
        /// </param>
        /// <param name="PolicyHandle">
        ///  An RPC context handle that represents a reference to the abstract 
        ///  data model of a policy object.
        /// </param>
        NtStatus LsarOpenPolicy(
            ushort[] SystemName,
            _LSAPR_OBJECT_ATTRIBUTES? ObjectAttributes,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? PolicyHandle);


        /// <summary>
        ///  Opnum9NotUsedOnWire method. Opnum: 9 
        /// </summary>
        void Opnum9NotUsedOnWire();


        /// <summary>
        ///  The LsarLookupNames method translates a batch of security
        ///  principal names to their SID form. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  14 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="Count">
        ///  Number of names in the Names array.The windowsRPC server
        ///  and RPC client limit the Count field of this structure
        ///  to 1,000 (using the range primitive defined in [MS-RPCE])
        ///  in windows_xp_sp2, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. windows_nt_3_1, windows_nt_3_5,
        ///  windows_nt_3_51, windows_nt_4_0, windows_2000, and
        ///  windows_xp do not enforce this restriction.
        /// </param>
        /// <param name="Names">
        ///  Contains the security principal names to translate,
        ///  as specified in section 3.1.4.5.
        /// </param>
        /// <param name="ReferencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="TranslatedSids">
        ///  On successful return, contains the corresponding SID
        ///  forms for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="LookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="MappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to their SID forms. This
        ///  parameter has no effect on message processing in any
        ///  environment. It MUST be ignored on input.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupNames(
            System.IntPtr PolicyHandle,
            uint Count,
            _RPC_UNICODE_STRING[] Names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? ReferencedDomains,
            ref _LSAPR_TRANSLATED_SIDS? TranslatedSids,
            _LSAP_LOOKUP_LEVEL LookupLevel,
            ref System.UInt32? MappedCount);


        /// <summary>
        ///  The LsarLookupSids method translates a batch of security
        ///  principalSIDs to their name forms. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  15 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="SidEnumBuffer">
        ///  Contains the SIDs to be translated. The SIDs in this
        ///  structure can be that of users, groups, computers,
        ///  windows-defined well-known security principals, or
        ///  domains.
        /// </param>
        /// <param name="ReferencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="TranslatedNames">
        ///  On successful return, contains the corresponding name
        ///  form for security principalSIDs in the SidEnumBuffer
        ///  parameter. It MUST be ignored on input.
        /// </param>
        /// <param name="LookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="MappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to their Name forms. It MUST
        ///  be ignored on input.
        /// </param>
        NtStatus LsarLookupSids(
            System.IntPtr PolicyHandle,
            _LSAPR_SID_ENUM_BUFFER? SidEnumBuffer,
            out _LSAPR_REFERENCED_DOMAIN_LIST? ReferencedDomains,
            ref _LSAPR_TRANSLATED_NAMES? TranslatedNames,
            _LSAP_LOOKUP_LEVEL LookupLevel,
            ref System.UInt32? MappedCount);


        /// <summary>
        ///  Opnum21NotUsedOnWire method. Opnum: 21 
        /// </summary>
        void Opnum21NotUsedOnWire();


        /// <summary>
        ///  Opnum22NotUsedOnWire method. Opnum: 22 
        /// </summary>
        void Opnum22NotUsedOnWire();


        /// <summary>
        ///  The LsarOpenPolicy2 method opens a context handle to
        ///  the RPC server. Opnum: 44 
        /// </summary>
        /// <param name="SystemName">
        ///  SystemName parameter does not have any effect on message 
        ///  processing in any environment. It MUST be ignored on receipt.
        /// </param>
        /// <param name="ObjectAttributes">
        ///  ObjectAttributes parameter does not have any effect on message 
        ///  processing in any environment. All fields MUST be ignored except 
        ///  RootDirectory which MUST be NULL.
        /// </param>
        /// <param name="DesiredAccess">
        ///  DesiredAccess parameter that specifies the requested access rights
        ///  that MUST be granted on the returned PolicyHandle if the request
        ///  is successful.
        /// </param>
        /// <param name="PolicyHandle">
        ///  An RPC context handle that represents a reference to the abstract 
        ///  data model of a policy object.
        /// </param>
        NtStatus LsarOpenPolicy2(
            string SystemName,
            _LSAPR_OBJECT_ATTRIBUTES? ObjectAttributes,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? PolicyHandle);


        /// <summary>
        ///  The LsarGetUserName method returns the name and the
        ///  domain name of the security principal that is invoking
        ///  the method. Opnum: 45 
        /// </summary>
        /// <param name="SystemName">
        ///  This parameter has no effect on message processing in
        ///  any environment. It MUST be ignored.
        /// </param>
        /// <param name="UserName">
        ///  On return, contains the name of the security principal
        ///  that is making the call. The string MUST be of the
        ///  form sAMAccountName. On input, this parameter MUST
        ///  be ignored. The RPC_UNICODE_STRING structure is defined
        ///  in [MS-DTYP] section 2.3.6.
        /// </param>
        /// <param name="DomainName">
        ///  On return, contains the domain name of the security
        ///  principal that is invoking the method. This string
        ///  MUST be a NetBIOS name. On input, this parameter MUST
        ///  be ignored.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarGetUserName(
            string SystemName,
            ref _RPC_UNICODE_STRING? UserName,
            ref _RPC_UNICODE_STRING? DomainName);


        /// <summary>
        ///  Opnum52NotUsedOnWire method. Opnum: 52 
        /// </summary>
        void Opnum52NotUsedOnWire();


        /// <summary>
        ///  Opnum56NotUsedOnWire method. Opnum: 56 
        /// </summary>
        void Opnum56NotUsedOnWire();


        /// <summary>
        ///  The LsarLookupSids2 method translates a batch of security
        ///  principalSIDs to their name forms. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  57 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="SidEnumBuffer">
        ///  Contains the SIDs to be translated. The SIDs in this
        ///  structure can be that of users, groups, computers,
        ///  windows-defined well-known security principals, or
        ///  domains.
        /// </param>
        /// <param name="ReferencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="TranslatedNames">
        ///  On successful return, contains the corresponding name
        ///  forms for security principalSIDs in the SidEnumBuffer
        ///  parameter. It MUST be ignored on input.
        /// </param>
        /// <param name="LookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="MappedCount">
        ///  On return, contains the number of names that are translated
        ///  completely to their Name forms. It MUST be ignored
        ///  on input.
        /// </param>
        /// <param name="LookupOptions">
        ///  Flags that control the lookup operation. This parameter
        ///  is reserved for future use and SHOULDThe windowsRPC
        ///  client sets LookupOptions to 0. be set to 0.
        /// </param>
        /// <param name="ClientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupSids2(
            System.IntPtr PolicyHandle,
            _LSAPR_SID_ENUM_BUFFER? SidEnumBuffer,
            out _LSAPR_REFERENCED_DOMAIN_LIST? ReferencedDomains,
            ref _LSAPR_TRANSLATED_NAMES_EX? TranslatedNames,
            _LSAP_LOOKUP_LEVEL LookupLevel,
            ref System.UInt32? MappedCount,
            uint LookupOptions,
            ClientRevision_Values ClientRevision);


        /// <summary>
        ///  The LsarLookupNames2 method translates a batch of security
        ///  principal names to their SID form. It also returns
        ///  the domains that these names are a part of.A windowsRPC
        ///  server can optionally be configured to deny this call,
        ///  and the error returned in this case is STATUS_NOT_SUPPORTED.
        ///  Opnum: 58 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="Count">
        ///  Number of security principal names to look up.The windowsRPC
        ///  server and RPC client limit the Count field of this
        ///  structure to 1,000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </param>
        /// <param name="Names">
        ///  Contains the security principal names to translate,
        ///  as specified in section 3.1.4.5.
        /// </param>
        /// <param name="ReferencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="TranslatedSids">
        ///  On successful return, contains the corresponding SID
        ///  forms for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="LookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="MappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to the SID form. This parameter
        ///  has no effect on message processing in any environment.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="LookupOptions">
        ///  Flags that control the lookup operation. For possible
        ///  values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <param name="ClientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupNames2(
            System.IntPtr PolicyHandle,
            uint Count,
            _RPC_UNICODE_STRING[] Names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? ReferencedDomains,
            ref _LSAPR_TRANSLATED_SIDS_EX? TranslatedSids,
            _LSAP_LOOKUP_LEVEL LookupLevel,
            ref System.UInt32? MappedCount,
            LookupOptions_Values LookupOptions,
            ClientRevision_Values ClientRevision);


        /// <summary>
        ///  Opnum60NotUsedOnWire method. Opnum: 60 
        /// </summary>
        void Opnum60NotUsedOnWire();


        /// <summary>
        ///  Opnum61NotUsedOnWire method. Opnum: 61 
        /// </summary>
        void Opnum61NotUsedOnWire();


        /// <summary>
        ///  Opnum62NotUsedOnWire method. Opnum: 62 
        /// </summary>
        void Opnum62NotUsedOnWire();


        /// <summary>
        ///  Opnum63NotUsedOnWire method. Opnum: 63 
        /// </summary>
        void Opnum63NotUsedOnWire();


        /// <summary>
        ///  Opnum64NotUsedOnWire method. Opnum: 64 
        /// </summary>
        void Opnum64NotUsedOnWire();


        /// <summary>
        ///  Opnum65NotUsedOnWire method. Opnum: 65 
        /// </summary>
        void Opnum65NotUsedOnWire();


        /// <summary>
        ///  Opnum66NotUsedOnWire method. Opnum: 66 
        /// </summary>
        void Opnum66NotUsedOnWire();


        /// <summary>
        ///  Opnum67NotUsedOnWire method. Opnum: 67 
        /// </summary>
        void Opnum67NotUsedOnWire();


        /// <summary>
        ///  The LsarLookupNames3 method translates a batch of security
        ///  principal names to their SID form. It also returns
        ///  the domains that these names are a part of.All versions
        ///  of windows that implement this method (LsarLookupNames3)
        ///  also implement LsarLookupNames4 (both in terms of client
        ///  and server); hence, this method does not need to be
        ///  implemented to interoperate with windows clients or
        ///  servers. The choice of which method to call depends
        ///  on whether the client has a local security authority
        ///  (LSA) policy handle or an RPC binding handle. Complete
        ///  compatibility with windows supports both calls. Opnum
        ///  : 68 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="Count">
        ///  Number of security principal names to look up.The windows
        ///  implementation of the RPC server and RPC client limits
        ///  the Count field of this structure to 1,000 (using the
        ///  range primitive defined in [MS-RPCE]) in windows_xp_sp2,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. windows_nt_3_1, windows_nt_3_5, windows_nt_3_51,
        ///  windows_nt_4_0, windows_2000, and windows_xp do not
        ///  enforce this restriction.
        /// </param>
        /// <param name="Names">
        ///  Contains the security principal names to translate,
        ///  as specified in section 3.1.4.5.
        /// </param>
        /// <param name="ReferencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="TranslatedSids">
        ///  On successful return, contains the corresponding SID
        ///  forms for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="LookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="MappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to the SID form. This parameter
        ///  has no effect on message processing in any environment.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="LookupOptions">
        ///  Flags that control the lookup operation. For possible
        ///  values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <param name="ClientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupNames3(
            System.IntPtr PolicyHandle,
            uint Count,
            _RPC_UNICODE_STRING[] Names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? ReferencedDomains,
            ref _LSAPR_TRANSLATED_SIDS_EX2? TranslatedSids,
            _LSAP_LOOKUP_LEVEL LookupLevel,
            ref System.UInt32? MappedCount,
            LookupOptions_Values LookupOptions,
            ClientRevision_Values ClientRevision);


        /// <summary>
        ///  Opnum69NotUsedOnWire method. Opnum: 69 
        /// </summary>
        void Opnum69NotUsedOnWire();


        /// <summary>
        ///  Opnum70NotUsedOnWire method. Opnum: 70 
        /// </summary>
        void Opnum70NotUsedOnWire();


        /// <summary>
        ///  Opnum71NotUsedOnWire method. Opnum: 71 
        /// </summary>
        void Opnum71NotUsedOnWire();


        /// <summary>
        ///  Opnum72NotUsedOnWire method. Opnum: 72 
        /// </summary>
        void Opnum72NotUsedOnWire();


        /// <summary>
        ///  Opnum75NotUsedOnWire method. Opnum: 75 
        /// </summary>
        void Opnum75NotUsedOnWire();


        /// <summary>
        ///  The LsarLookupSids3 method translates a batch of security
        ///  principalSIDs to their name forms. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  76 
        /// </summary>
        /// <param name="RpcHandle">
        ///  An RPC binding handle, as described in [C706]. RPC binding
        ///  handles are used by RPC internally and are not transmitted
        ///  over the network.This handle can be obtained by calling
        ///  RPC runtime binding routines. For more information,
        ///  see [MSDN-RPCDB].
        /// </param>
        /// <param name="SidEnumBuffer">
        ///  Contains the SIDs to be translated. The SIDs in this
        ///  structure can be that of users, groups, computers,
        ///  windows-defined well-known security principals, or
        ///  domains.
        /// </param>
        /// <param name="ReferencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="TranslatedNames">
        ///  On successful return, contains the corresponding name
        ///  forms for security principalSIDs in the SidEnumBuffer
        ///  parameter. It MUST be ignored on input.
        /// </param>
        /// <param name="LookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="MappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to their Name forms. It MUST
        ///  be ignored on input.
        /// </param>
        /// <param name="LookupOptions">
        ///  Flags that control the lookup operation. This parameter
        ///  is reserved for future use; it MUST be set to 0 and
        ///  ignored on receipt.
        /// </param>
        /// <param name="ClientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        NtStatus LsarLookupSids3(
            System.IntPtr RpcHandle,
            _LSAPR_SID_ENUM_BUFFER? SidEnumBuffer,
            out _LSAPR_REFERENCED_DOMAIN_LIST? ReferencedDomains,
            ref _LSAPR_TRANSLATED_NAMES_EX? TranslatedNames,
            _LSAP_LOOKUP_LEVEL LookupLevel,
            ref System.UInt32? MappedCount,
            uint LookupOptions,
            ClientRevision_Values ClientRevision);


        /// <summary>
        ///  The LsarLookupNames4 method translates a batch of security
        ///  principal names to their SID form. It also returns
        ///  the domains of which these security principals are
        ///  a part. Opnum: 77 
        /// </summary>
        /// <param name="RpcHandle">
        ///  This value is used by RPC internally and is not transmitted
        ///  over the network, as specified in [C706]. This handle
        ///  can be obtained by calling RPC runtime binding routines.
        ///  For more information, see [MSDN-RPCDB].
        /// </param>
        /// <param name="Count">
        ///  Number of security principal names to look up.The windowsRPC
        ///  server and RPC client limit the Count field of this
        ///  structure to 1,000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </param>
        /// <param name="Names">
        ///  Contains the security principal names to translate.
        ///  The RPC_UNICODE_STRING structure is defined in [MS-DTYP]
        ///  section 2.3.6.The following name forms MUST be supported:User
        ///  principal names (UPNs), such as user_name@example.example.com.Fully
        ///  qualified account names based on either DNS or NetBIOS
        ///  names. For example: example.example.com\user_name or
        ///  example\user_name, where the generalized form is domain\user
        ///  account name, and domain is either the fully qualified
        ///  DNS name or the NetBIOS name of the trusted domain.Unqualified
        ///  or isolated names, such as user_name.The comparisons
        ///  used by the RPC server MUST NOT be case-sensitive,
        ///  so case for inputs is not important.
        /// </param>
        /// <param name="ReferencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="TranslatedSids">
        ///  On successful return, contains the corresponding SID
        ///  form for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="LookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="MappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to the SID form. This parameter
        ///  is left as an input parameter for backward compatibility
        ///  and has no effect on message processing in any environment.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="LookupOptions">
        ///  Flags specified by the caller that control the lookup
        ///  operation. The value MUST be one of the following.
        /// </param>
        /// <param name="ClientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  The value MUST be one of the following.For windows,
        ///  usage of 0x00000001 for ClientRevision implies a client
        ///  that is running an operating system released before
        ///  windows_2000 (windows_nt_3_1, windows_nt_3_5, windows_nt_3_51,
        ///  and windows_nt_4_0). Usage of 0x00000002 implies that
        ///  the client is running an operating system version of
        ///  windows_2000 or a later release (windows_2000, windows_xp,
        ///  windows_server_2003, windows_vista, or windows_server_2008,
        ///  windows_vista, windows_server_2008, windows_7, or windows_server_7).
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupNames4(
            System.IntPtr RpcHandle,
            uint Count,
            _RPC_UNICODE_STRING[] Names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? ReferencedDomains,
            ref _LSAPR_TRANSLATED_SIDS_EX2? TranslatedSids,
            _LSAP_LOOKUP_LEVEL LookupLevel,
            ref System.UInt32? MappedCount,
            LookupOptions_Values LookupOptions,
            ClientRevision_Values ClientRevision);

        #region LSAD
        /// <summary>
        ///  The LsarEnumeratePrivileges method is invoked to enumerate
        ///  all privileges known to the system. This method can
        ///  be called multiple times to return its output in fragments.
        ///  Opnum: 2 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="EnumerationContext">
        ///  A pointer to a context value that is used to resume
        ///  enumeration, if necessary.
        /// </param>
        /// <param name="EnumerationBuffer">
        ///  A pointer to a structure that will contain the results
        ///  of the enumeration.
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The preferred maximum length of returned data, in bytes.
        ///  This is not a strict upper limit but serves as a guide.
        ///  It is valid for the actual amount of data returned
        ///  to be greater than this value.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarEnumeratePrivileges(
            System.IntPtr PolicyHandle,
            ref System.UInt32? EnumerationContext,
            out _LSAPR_PRIVILEGE_ENUM_BUFFER? EnumerationBuffer,
            uint PreferedMaximumLength);

        /// <summary>
        ///  The LsarQuerySecurityObject method is invoked to query
        ///  security information that is assigned to a database
        ///  object. It returns the security descriptor of the object.
        ///  Opnum: 3 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An open object handle of any type.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bitmask specifying which portions of the security
        ///  descriptor the caller is interested in.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  Used to return the security descriptor containing the
        ///  elements requested by the caller.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarQuerySecurityObject(
            System.IntPtr ObjectHandle,
            SECURITY_INFORMATION SecurityInformation,
            out _LSAPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor);

        /// <summary>
        ///  The LsarSetSecurityObject method is invoked to set a
        ///  security descriptor on an object. Opnum: 4 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An open handle to an existing object.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bitmask specifying which portions of the security
        ///  descriptor are to be set.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  The security descriptor to be set.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarSetSecurityObject(
            System.IntPtr ObjectHandle,
            SECURITY_INFORMATION SecurityInformation,
            _LSAPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor);

        /// <summary>
        ///  The LsarQueryInformationPolicy method is invoked to
        ///  query values that represent the server's information
        ///  policy. Opnum: 7 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="InformationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is requesting.
        /// </param>
        /// <param name="PolicyInformation">
        ///  A parameter that references policy information structure
        ///  on return.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarQueryInformationPolicy(
            System.IntPtr PolicyHandle,
            _POLICY_INFORMATION_CLASS InformationClass,
            //[Switch("InformationClass")]
            out _LSAPR_POLICY_INFORMATION? PolicyInformation);

        /// <summary>
        ///  The LsarSetInformationPolicy method is invoked to set
        ///  a policy on the server. Opnum: 8 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="InformationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is setting.
        /// </param>
        /// <param name="PolicyInformation">
        ///  Data that represents the policy being set.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarSetInformationPolicy(
            System.IntPtr PolicyHandle,
            _POLICY_INFORMATION_CLASS InformationClass,
            //[Switch("InformationClass")]
            _LSAPR_POLICY_INFORMATION? PolicyInformation);

        /// <summary>
        ///  The LsarCreateAccount method is invoked to create a
        ///  new account object in the server's database. Opnum
        ///  : 10 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="AccountSid">
        ///  The security identifier (SID) of the account to be created.
        /// </param>
        /// <param name="DesiredAccess">
        ///  A bitmask specifying accesses to be granted to the newly
        ///  created and opened account at this time.
        /// </param>
        /// <param name="AccountHandle">
        ///  Used to return a handle to the newly created account
        ///  object.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarCreateAccount(
            System.IntPtr PolicyHandle,
            _RPC_SID? AccountSid,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? AccountHandle);

        /// <summary>
        ///  The LsarEnumerateAccounts method is invoked to request
        ///  a list of account objects in the server's database.
        ///  The method can be called multiple times to return its
        ///  output in fragments. Opnum: 11 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="EnumerationContext">
        ///  A pointer to a context value that is used to resume
        ///  enumeration, if necessary.
        /// </param>
        /// <param name="EnumerationBuffer">
        ///  A pointer to a structure that will contain the results
        ///  of the enumeration.
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The preferred maximum length of returned data, in bytes.
        ///  This is not a strict upper limit, but serves as a guide.
        ///  It is valid for the actual amount of data that is returned
        ///  to be greater than this value.The windows implementation
        ///  of this method may exceed the preferred maximum length
        ///  specified by the caller.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarEnumerateAccounts(
            System.IntPtr PolicyHandle,
            ref System.UInt32? EnumerationContext,
            out _LSAPR_ACCOUNT_ENUM_BUFFER? EnumerationBuffer,
            uint PreferedMaximumLength);

        /// <summary>
        ///  The LsarCreateTrustedDomain method is invoked to create
        ///  an object of type trusted domain in the server's database.
        ///  Opnum: 12 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Information about the new trusted domain object to be
        ///  created.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An access mask that specifies the desired access to
        ///  the trusted domain object handle.
        /// </param>
        /// <param name="TrustedDomainHandle">
        ///  Used to return the handle for the newly created trusted
        ///  domain object.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarCreateTrustedDomain(System.IntPtr PolicyHandle,
            _LSAPR_TRUST_INFORMATION? TrustedDomainInformation,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? TrustedDomainHandle);

        /// <summary>
        ///  The LsarEnumerateTrustedDomains method is invoked to
        ///  request a list of trusted domain objects in the server's
        ///  database. The method can be called multiple times to
        ///  return its output in fragments. Opnum: 13 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="EnumerationContext">
        ///  A pointer to a context value that is used to resume
        ///  enumeration, if necessary.
        /// </param>
        /// <param name="EnumerationBuffer">
        ///  A pointer to a structure that will contain the results
        ///  of the enumeration.
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The preferred maximum length of returned data, in bytes.
        ///  This is not a strict upper limit but serves as a guide.
        ///  It is valid for the actual amount of data returned
        ///  to be greater than this value.The windows implementation
        ///  of this method might exceed the maximum desired length
        ///  specified by the caller.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarEnumerateTrustedDomains(
            System.IntPtr PolicyHandle,
            ref System.UInt32? EnumerationContext,
            out _LSAPR_TRUSTED_ENUM_BUFFER? EnumerationBuffer,
            uint PreferedMaximumLength);


        /// <summary>
        ///  The LsarCreateSecret method is invoked to create a new
        ///  secret object in the server's database. Opnum: 16
        ///  
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="SecretName">
        ///  The name of the secret object to be created.
        /// </param>
        /// <param name="DesiredAccess">
        ///  A bitmask that specifies accesses to be granted to the
        ///  newly created and opened secret object at this time.
        /// </param>
        /// <param name="SecretHandle">
        ///  Used to return a handle to the newly created account
        ///  object.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarCreateSecret(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? SecretName,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? SecretHandle);

        /// <summary>
        ///  The LsarOpenAccount method is invoked to obtain a handle
        ///  to an account object. Opnum: 17 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="AccountSid">
        ///  A SID of the account to be opened.
        /// </param>
        /// <param name="DesiredAccess">
        ///  A bitmask specifying accesses to be granted to the opened
        ///  account at this time.
        /// </param>
        /// <param name="AccountHandle">
        ///  Used to return a handle to the opened account object.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarOpenAccount(
            System.IntPtr PolicyHandle,
            _RPC_SID? AccountSid,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? AccountHandle);

        /// <summary>
        ///  The LsarEnumeratePrivilegesAccount method is invoked
        ///  to retrieve a list of privileges granted to an account
        ///  on the server. Opnum: 18 
        /// </summary>
        /// <param name="AccountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="Privileges">
        ///  Used to return a list of privileges granted to the account.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarEnumeratePrivilegesAccount(
            System.IntPtr AccountHandle,
            out _LSAPR_PRIVILEGE_SET? Privileges);

        /// <summary>
        ///  The LsarAddPrivilegesToAccount method is invoked to
        ///  add new privileges to an existing account object. Opnum
        ///  : 19 
        /// </summary>
        /// <param name="AccountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="Privileges">
        ///  Contains a list of privileges to add to the account.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarAddPrivilegesToAccount(
            System.IntPtr AccountHandle,
            _LSAPR_PRIVILEGE_SET? Privileges);

        /// <summary>
        ///  The LsarRemovePrivilegesFromAccount method is invoked
        ///  to remove privileges from an account object. Opnum
        ///  : 20 
        /// </summary>
        /// <param name="AccountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="AllPrivileges">
        ///  If this parameter is not FALSE (0), all privileges will
        ///  be stripped from the account object.
        /// </param>
        /// <param name="Privileges">
        ///  Contains a (possibly empty) list of privileges to remove
        ///  from the account object.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarRemovePrivilegesFromAccount(
            System.IntPtr AccountHandle,
            byte AllPrivileges,
            _LSAPR_PRIVILEGE_SET? Privileges);

        /// <summary>
        ///  The LsarGetSystemAccessAccount method is invoked to
        ///  retrieve system access account flags for an account
        ///  object. System access account flags are described as
        ///  part of the account object data model, as specified
        ///  in section. Opnum: 23 
        /// </summary>
        /// <param name="AccountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="SystemAccess">
        ///  Used to return a bitmask of access flags associated
        ///  with the account.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarGetSystemAccessAccount(
            System.IntPtr AccountHandle,
            out System.UInt32? SystemAccess);

        /// <summary>
        ///  The LsarSetSystemAccessAccount method is invoked to
        ///  set system access account flags for an account object.
        ///  Opnum: 24 
        /// </summary>
        /// <param name="AccountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="SystemAccess">
        ///  A bitmask containing the account flags to be set on
        ///  the account.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarSetSystemAccessAccount(
            System.IntPtr AccountHandle,
            System.UInt32? SystemAccess);

        /// <summary>
        ///  The LsarOpenTrustedDomain method is invoked to obtain
        ///  a handle to a trusted domain object. Opnum: 25 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainSid">
        ///  A security identifier of the trusted domain that is
        ///  being opened.
        /// </param>
        /// <param name="DesiredAccess">
        ///  A bitmask of access rights to open the object with.
        /// </param>
        /// <param name="TrustedDomainHandle">
        ///  Used to return the trusted domain object handle.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarOpenTrustedDomain(
            System.IntPtr PolicyHandle,
            _RPC_SID? TrustedDomainSid,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? TrustedDomainHandle);

        /// <summary>
        ///  The LsarQueryInfoTrustedDomain method is invoked to
        ///  retrieve information about the trusted domain object.
        ///  Opnum: 26 
        /// </summary>
        /// <param name="TrustedDomainHandle">
        ///  An open trusted domain object handle.
        /// </param>
        /// <param name="InformationClass">
        ///  One of the TRUSTED_INFORMATION_CLASS values indicating
        ///  the type of information the caller is interested in.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Used to return requested information about the trusted
        ///  domain object.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarQueryInfoTrustedDomain(
            System.IntPtr TrustedDomainHandle,
            _TRUSTED_INFORMATION_CLASS InformationClass,
            //[Switch("InformationClass")]
            out _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation);

        /// <summary>
        ///  The LsarSetInformationTrustedDomain method is invoked
        ///  to set information on a trusted domain object. Opnum
        ///  : 27 
        /// </summary>
        /// <param name="TrustedDomainHandle">
        ///  A handle to a trusted domain object.
        /// </param>
        /// <param name="InformationClass">
        ///  A value indicating the type of information requested
        ///  by the caller.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Used to supply the information to be set.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarSetInformationTrustedDomain(
            System.IntPtr TrustedDomainHandle,
            _TRUSTED_INFORMATION_CLASS InformationClass,
            //[Switch("InformationClass")]
            _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation);

        /// <summary>
        ///  The LsarOpenSecret method is invoked to obtain a handle
        ///  to an existing secret object. Opnum: 28 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="SecretName">
        ///  The name of the secret object to open.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The requested type of access.
        /// </param>
        /// <param name="SecretHandle">
        ///  Used to return the handle to the opened secret object.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarOpenSecret(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? SecretName,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? SecretHandle);

        /// <summary>
        ///  The LsarSetSecret method is invoked to set the current
        ///  and old values of the secret object. Opnum: 29 
        /// </summary>
        /// <param name="SecretHandle">
        ///  An open secret object handle.
        /// </param>
        /// <param name="EncryptedCurrentValue">
        ///  A binary large object (BLOB) representing a new encrypted
        ///  cipher value. It is valid for this parameter to be
        ///  NULL, in which case the value is deleted from the server's
        ///  policy database.
        /// </param>
        /// <param name="EncryptedOldValue">
        ///  A BLOB representing the encrypted old value. It is valid
        ///  for this parameter to be NULL, in which case the current
        ///  value in the policy database is copied.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarSetSecret(
            System.IntPtr SecretHandle,
            _LSAPR_CR_CIPHER_VALUE? EncryptedCurrentValue,
            _LSAPR_CR_CIPHER_VALUE? EncryptedOldValue);

        /// <summary>
        ///  The LsarQuerySecret method is invoked to retrieve the
        ///  current and old (or previous) value of the secret object.
        ///  Opnum: 30 
        /// </summary>
        /// <param name="SecretHandle">
        ///  An open secret object handle.
        /// </param>
        /// <param name="EncryptedCurrentValue">
        ///  Used to return the encrypted current value of the secret
        ///  object.
        /// </param>
        /// <param name="CurrentValueSetTime">
        ///  Used to return the time when the current value was set.
        /// </param>
        /// <param name="EncryptedOldValue">
        ///  A BLOB representing the encrypted old value. It is valid
        ///  for this parameter to be NULL, in which case the current
        ///  value in the policy database is copied.
        /// </param>
        /// <param name="OldValueSetTime">
        ///  The time corresponding to the instant that the old value
        ///  was last changed.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarQuerySecret(
            System.IntPtr SecretHandle,
            ref _LSAPR_CR_CIPHER_VALUE? EncryptedCurrentValue,
            ref _LARGE_INTEGER? CurrentValueSetTime,
            ref _LSAPR_CR_CIPHER_VALUE? EncryptedOldValue,
            ref _LARGE_INTEGER? OldValueSetTime);

        /// <summary>
        ///  The LsarLookupPrivilegeValue method is invoked to map
        ///  the name of a privilege into a locally unique identifier
        ///  (LUID) by which the privilege is known on the server.
        ///  The locally unique value of the privilege can then
        ///  be used in subsequent calls to other methods, such
        ///  as LsarAddPrivilegesToAccount. Opnum: 31 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="Name">
        ///  A string containing the name of a privilege.
        /// </param>
        /// <param name="Value">
        ///  Used to return a LUID assigned by the server to the
        ///  privilege by this name.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupPrivilegeValue(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? Name,
            out _LUID? Value);

        /// <summary>
        ///  The LsarLookupPrivilegeName method is invoked to map
        ///  the LUID of a privilege into a string name by which
        ///  the privilege is known on the server. Opnum: 32 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="Value">
        ///  A LUID that the caller wishes to map to a string name.
        /// </param>
        /// <param name="Name">
        ///  Used to return the string name corresponding to the
        ///  supplied LUID.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupPrivilegeName(
            System.IntPtr PolicyHandle,
            _LUID? Value,
            out _RPC_UNICODE_STRING? Name);

        /// <summary>
        ///  The LsarLookupPrivilegeDisplayName method is invoked
        ///  to map the name of a privilege into a display text
        ///  string in the caller's language. Opnum: 33 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="Name">
        ///  A string containing the name of a privilege.
        /// </param>
        /// <param name="ClientLanguage">
        ///  An identifier of the client's language.
        /// </param>
        /// <param name="ClientSystemDefaultLanguage">
        ///  An identifier of the default language of the caller's
        ///  machine.
        /// </param>
        /// <param name="DisplayName">
        ///  Used to return the display name of the privilege in
        ///  the language pointed to by the LanguageReturned value.
        /// </param>
        /// <param name="LanguageReturned">
        ///  An identifier of the language in which DisplayName was
        ///  returned.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarLookupPrivilegeDisplayName(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? Name,
            short ClientLanguage,
            short ClientSystemDefaultLanguage,
            out _RPC_UNICODE_STRING? DisplayName,
            out System.UInt16? LanguageReturned);

        /// <summary>
        ///  The LsarDeleteObject method is invoked to delete an
        ///  open account object, secret object, or trusted domain
        ///  object. Opnum: 34 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  A handle to an open object of the correct type to be
        ///  deleted. After successful completion of the call, the
        ///  handle value cannot be reused.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarDeleteObject(
            ref System.IntPtr? ObjectHandle);

        /// <summary>
        ///  The LsarEnumerateAccountsWithUserRight method is invoked
        ///  to return a list of account objects that have the user
        ///  right equal to the passed-in value. Opnum: 35 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="UserRight">
        ///  The name of the right to use in enumeration.
        /// </param>
        /// <param name="EnumerationBuffer">
        ///  Used to return the list of account objects that have
        ///  the specified right.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarEnumerateAccountsWithUserRight(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? UserRight,
            out _LSAPR_ACCOUNT_ENUM_BUFFER? EnumerationBuffer);

        /// <summary>
        ///  The LsarEnumerateAccountRights method is invoked to
        ///  retrieve a list of rights associated with an existing
        ///  account. Opnum: 36 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="AccountSid">
        ///  A SID of the account object that the caller is inquiring
        ///  about.
        /// </param>
        /// <param name="UserRights">
        ///  Used to return a list of right names associated with
        ///  the account.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarEnumerateAccountRights(
            System.IntPtr PolicyHandle,
            _RPC_SID? AccountSid,
            out _LSAPR_USER_RIGHT_SET? UserRights);

        /// <summary>
        ///  The LsarAddAccountRights method is invoked to add new
        ///  rights to an account object. If the account object
        ///  does not exist, the system will attempt to create one.
        ///  Opnum: 37 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="AccountSid">
        ///  A security identifier of an account to add the rights
        ///  to.
        /// </param>
        /// <param name="UserRights">
        ///  A set of right names to add to the account.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarAddAccountRights(
            System.IntPtr PolicyHandle,
            _RPC_SID? AccountSid,
            _LSAPR_USER_RIGHT_SET? UserRights);

        /// <summary>
        ///  The LsarRemoveAccountRights method is invoked to remove
        ///  rights from an account object. Opnum: 38 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="AccountSid">
        ///  A security descriptor of an account object.
        /// </param>
        /// <param name="AllRights">
        ///  If this field is not set to 0, all rights will be removed.
        /// </param>
        /// <param name="UserRights">
        ///  A set of rights to remove from the account.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarRemoveAccountRights(
            System.IntPtr PolicyHandle,
            _RPC_SID? AccountSid,
            byte AllRights,
            _LSAPR_USER_RIGHT_SET? UserRights);

        /// <summary>
        ///  The LsarQueryTrustedDomainInfo method is invoked to
        ///  retrieve information on a trusted domain object. Opnum
        ///  : 39 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainSid">
        ///  A security descriptor of the trusted domain object.
        /// </param>
        /// <param name="InformationClass">
        ///  Identifies the type of information the caller is interested
        ///  in.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Used to return the information on the trusted domain
        ///  object to the caller.
        /// </param>
        [CLSCompliant(false)]
        NtStatus LsarQueryTrustedDomainInfo(
            System.IntPtr PolicyHandle,
            _RPC_SID? TrustedDomainSid,
            _TRUSTED_INFORMATION_CLASS InformationClass,
            out _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation);

        /// <summary>
        ///  The LsarSetTrustedDomainInfo method is invoked to set
        ///  information on a trusted domain object. In some cases,
        ///  if the trusted domain object does not exist, it will
        ///  be created. Opnum: 40 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainSid">
        ///  A SID of the trusted domain object to be modified.
        /// </param>
        /// <param name="InformationClass">
        ///  Identifies the type of information to be set on the
        ///  trusted domain object.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Information to be set on the trusted domain object.
        /// </param>
        NtStatus LsarSetTrustedDomainInfo(
            System.IntPtr PolicyHandle,
            _RPC_SID? TrustedDomainSid,
            _TRUSTED_INFORMATION_CLASS InformationClass,
            _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation);

        /// <summary>
        ///  The LsarDeleteTrustedDomain method is invoked to delete
        ///  a trusted domain object. Opnum: 41 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainSid">
        ///  A security descriptor of the trusted domain object to
        ///  be deleted.
        /// </param>
        NtStatus LsarDeleteTrustedDomain(
            System.IntPtr PolicyHandle,
            _RPC_SID? TrustedDomainSid);

        /// <summary>
        ///  The LsarStorePrivateData method is invoked to store
        ///  a secret value. Opnum: 42 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="KeyName">
        ///  The name under which private data will be stored.
        /// </param>
        /// <param name="EncryptedData">
        ///  The secret value to be stored.
        /// </param>
        NtStatus LsarStorePrivateData(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? KeyName,
            _LSAPR_CR_CIPHER_VALUE? EncryptedData);

        /// <summary>
        ///  The LsarRetrievePrivateData method is invoked to retrieve
        ///  a secret value. Opnum: 43 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="KeyName">
        ///  The name identifying the secret value to be retrieved.
        /// </param>
        /// <param name="EncryptedData">
        ///  Receives the encrypted value of the secret object.
        /// </param>
        NtStatus LsarRetrievePrivateData(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? KeyName,
            ref _LSAPR_CR_CIPHER_VALUE? EncryptedData);

        /// <summary>
        ///  The LsarQueryInformationPolicy2 method is invoked to
        ///  query values that represent the server's security policy.
        ///  Opnum: 46 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="InformationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is requesting.
        /// </param>
        /// <param name="PolicyInformation">
        ///  A parameter that references policy information structure
        ///  on return.
        /// </param>
        NtStatus LsarQueryInformationPolicy2(
            System.IntPtr PolicyHandle,
            _POLICY_INFORMATION_CLASS InformationClass,
            out _LSAPR_POLICY_INFORMATION? PolicyInformation);

        /// <summary>
        ///  The LsarSetInformationPolicy2 method is invoked to set
        ///  a policy on the server. Opnum: 47 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="InformationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is setting.
        /// </param>
        /// <param name="PolicyInformation">
        ///  Data that represents policy being set.
        /// </param>
        NtStatus LsarSetInformationPolicy2(
            System.IntPtr PolicyHandle,
            _POLICY_INFORMATION_CLASS InformationClass,
            _LSAPR_POLICY_INFORMATION? PolicyInformation);

        /// <summary>
        ///  The LsarQueryTrustedDomainInfoByName method is invoked
        ///  to retrieve information about a trusted domain object
        ///  by its string name. Opnum: 48 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainName">
        ///  The name of the trusted domain object to query.
        /// </param>
        /// <param name="InformationClass">
        ///  One of the TRUSTED_INFORMATION_CLASS values identifying
        ///  the type of information the caller is interested in.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Used to return the information requested by the caller.
        /// </param>
        NtStatus LsarQueryTrustedDomainInfoByName(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? TrustedDomainName,
            _TRUSTED_INFORMATION_CLASS InformationClass,
            out _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation);

        /// <summary>
        ///  The LsarSetTrustedDomainInfoByName method is invoked
        ///  to set information about a trusted domain object by
        ///  its string name. Opnum: 49 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainName">
        ///  The name of the trusted domain object to set information
        ///  on.
        /// </param>
        /// <param name="InformationClass">
        ///  One of the TRUSTED_INFORMATION_CLASS values indicating
        ///  the type of information the caller is trying to set.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  The data being set.
        /// </param>
        NtStatus LsarSetTrustedDomainInfoByName(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? TrustedDomainName,
            _TRUSTED_INFORMATION_CLASS InformationClass,
            _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation);

        /// <summary>
        ///  The LsarEnumerateTrustedDomainsEx method is invoked
        ///  to enumerate trusted domain objects in the server's
        ///  database. The method is designed to be invoked multiple
        ///  times to retrieve the data in fragments. Opnum: 50
        ///  
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="EnumerationContext">
        ///  Used to keep track of the state of the enumeration in
        ///  cases where the caller obtains its information in several
        ///  fragments.
        /// </param>
        /// <param name="EnumerationBuffer">
        ///  Contains a fragment of requested information.
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  A value serving as a hint to the server as to the maximum
        ///  amount of data fragment a client would like to receive.
        /// </param>
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        NtStatus LsarEnumerateTrustedDomainsEx(
            System.IntPtr PolicyHandle,
            ref System.UInt32? EnumerationContext,
            out _LSAPR_TRUSTED_ENUM_BUFFER_EX? EnumerationBuffer,
            uint PreferedMaximumLength);

        /// <summary>
        ///  The LsarCreateTrustedDomainEx method is invoked to create
        ///  a new trusted domain object. Opnum: 51 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Information about the new trusted domain object to be
        ///  created.
        /// </param>
        /// <param name="AuthenticationInformation">
        ///  Encrypted authentication information for the new trusted
        ///  domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An access mask that specifies desired access to the
        ///  trusted domain object handle.
        /// </param>
        /// <param name="TrustedDomainHandle">
        ///  Used to return the handle for the newly created trusted
        ///  domain object.
        /// </param>
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        NtStatus LsarCreateTrustedDomainEx(
            System.IntPtr PolicyHandle,
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX? TrustedDomainInformation,
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION? AuthenticationInformation,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? TrustedDomainHandle);

        /// <summary>
        ///  The LsarQueryDomainInformationPolicy method is invoked
        ///  to retrieve policy settings in addition to those exposed
        ///  through LsarQueryInformationPolicy and LsarSetInformationPolicy2.
        ///  Despite the term "Domain" in the name of the method,
        ///  processing of this message occurs with local data,
        ///  and furthermore, there is no requirement that this
        ///  data have any relationship with the LSA information
        ///  in the domain to which the machine is joined. Opnum
        ///  : 53 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="InformationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is requesting.
        /// </param>
        /// <param name="PolicyDomainInformation">
        ///  A parameter that references policy information structure
        ///  on return.
        /// </param>
        NtStatus LsarQueryDomainInformationPolicy(
            System.IntPtr PolicyHandle,
            _POLICY_DOMAIN_INFORMATION_CLASS InformationClass,
            out _LSAPR_POLICY_DOMAIN_INFORMATION? PolicyDomainInformation);

        /// <summary>
        ///  The LsarSetDomainInformationPolicy method is invoked
        ///  to change policy settings in addition to those exposed
        ///  through LsarQueryInformationPolicy and LsarSetInformationPolicy2.
        ///  Despite the term "Domain" in the name of the method,
        ///  processing of this message occurs with local data.
        ///  Also, there is no requirement that this data have any
        ///  relationship with the LSA information in the domain
        ///  in which the machine is joined. Opnum: 54 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="InformationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is setting.
        /// </param>
        /// <param name="PolicyDomainInformation">
        ///  Data representing policy being set.
        /// </param>
        NtStatus LsarSetDomainInformationPolicy(
            System.IntPtr PolicyHandle,
            _POLICY_DOMAIN_INFORMATION_CLASS InformationClass,
            _LSAPR_POLICY_DOMAIN_INFORMATION? PolicyDomainInformation);

        /// <summary>
        ///  The LsarOpenTrustedDomainByName method is invoked to
        ///  open a trusted domain object handle by supplying the
        ///  name of the trusted domain. Opnum: 55 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainName">
        ///  The name of the trusted domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The type of access requested by the caller.
        /// </param>
        /// <param name="TrustedDomainHandle">
        ///  Used to return the opened trusted domain handle.
        /// </param>
        NtStatus LsarOpenTrustedDomainByName(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? TrustedDomainName,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? TrustedDomainHandle);


        /// <summary>
        ///  The LsarCreateTrustedDomainEx2 method is invoked to
        ///  create a new trusted domain object.small_business_server_2003does
        ///  not support this message. Attempts to create a trusted
        ///  domain object in this environment causes the server
        ///  to return STATUS_NOT_SUPPORTED_ON_SBS. Opnum: 59 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainInformation">
        ///  Information about the new trusted domain object to be
        ///  created.
        /// </param>
        /// <param name="AuthenticationInformation">
        ///  Encrypted authentication information for the new trusted
        ///  domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An access mask specifying desired access to the trusted
        ///  domain object handle.
        /// </param>
        /// <param name="TrustedDomainHandle">
        ///  Used to return the handle for the newly created trusted
        ///  domain object.
        /// </param>
        NtStatus LsarCreateTrustedDomainEx2(
            System.IntPtr PolicyHandle,
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX? TrustedDomainInformation,
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL? AuthenticationInformation,
            ACCESS_MASK DesiredAccess,
            out System.IntPtr? TrustedDomainHandle);

        /// <summary>
        ///  The LsarQueryForestTrustInformation method is invoked
        ///  to retrieve information about a trust relationship
        ///  with another forest. Opnum: 73 
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainName">
        ///  The name of the trusted domain to query.
        /// </param>
        /// <param name="HighestRecordType">
        ///  The highest ordinal number of foresttrust record type
        ///  that the caller understands.
        /// </param>
        /// <param name="ForestTrustInfo">
        ///  Used to return the forest trust information.
        /// </param>
        NtStatus LsarQueryForestTrustInformation(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? TrustedDomainName,
            _LSA_FOREST_TRUST_RECORD_TYPE HighestRecordType,
            out _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo);

        /// <summary>
        ///  The LsarSetForestTrustInformation method is invoked
        ///  to establish a trust relationship with another forest
        ///  by attaching a set of records called the forest trust
        ///  information to the trusted domain object. Opnum: 74
        ///  
        /// </summary>
        /// <param name="PolicyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="TrustedDomainName">
        ///  The name of the trusted domain object on which to set
        ///  the forest trust information.
        /// </param>
        /// <param name="HighestRecordType">
        ///  The highest ordinal foresttrust record type that the
        ///  caller understands.
        /// </param>
        /// <param name="ForestTrustInfo">
        ///  The forest trust information that the caller is trying
        ///  to set on the trusted domain object.
        /// </param>
        /// <param name="CheckOnly">
        ///  If not 0, the operation is read-only and does not alter
        ///  the state of the server's database.
        /// </param>
        /// <param name="CollisionInfo">
        ///  Used to return information about collisions between
        ///  different sets of forest trust information in the server's
        ///  database.
        /// </param>
        NtStatus LsarSetForestTrustInformation(
            System.IntPtr PolicyHandle,
            _RPC_UNICODE_STRING? TrustedDomainName,
            _LSA_FOREST_TRUST_RECORD_TYPE HighestRecordType,
            _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo,
            byte CheckOnly,
            out _LSA_FOREST_TRUST_COLLISION_INFORMATION? CollisionInfo);
        #endregion LSAD
    }
}
