// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// LSA client.
    /// You can call following methods from this class.<para/>
    /// RPC bind methods.<para/>
    /// LSA RPC methods.<para/>
    /// </summary>
    public class LsaClient : IDisposable
    {
        //Client context
        private LsaClientContext context;

        //Actual rpc adapter
        private ILsaRpcAdapter rpc;



        #region Constructor

        /// <summary>
        /// Constructor, initialize a LSA client.<para/>
        /// Create the instance will not connect to server, 
        /// you should call one of BindOverTcp or BindOverNamedPipe 
        /// to actually connect to LSA server.
        /// </summary>
        public LsaClient()
        {
            context = new LsaClientContext();
            rpc = new LsaRpcAdapter();
        }

        #endregion


        #region RPC bind methods

        /// <summary>
        /// RPC bind over named pipe, using well-known endpoint "\PIPE\lsarpc".
        /// </summary>
        /// <param name="serverName">LSA server machine name.</param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by under layer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="authenticationLevel">
        /// Authentication level for RPC. 
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        [CLSCompliant(false)]
        public void BindOverNamedPipe(
            string serverName,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            rpc.Bind(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                serverName,
                LsaUtility.LSA_RPC_OVER_NP_WELLKNOWN_ENDPOINT,
                transportCredential,
                securityContext,
                authenticationLevel,
                timeout);

            LsaRpcAdapter lsaRpcAdapter = rpc as LsaRpcAdapter;
            if (lsaRpcAdapter != null)
            {
                context.rpceTransportContext = lsaRpcAdapter.rpceClientTransport.Context;
            }
        }


        /// <summary>
        /// RPC bind over TCP/IP, using specified endpoint and authenticate provider.
        /// </summary>
        /// <param name="serverName">LSA server machine name.</param>
        /// <param name="endpoint">RPC endpoints, it's the port on TCP/IP.</param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="authenticationLevel">
        /// Authentication level for RPC. 
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        [CLSCompliant(false)]
        public void BindOverTcp(
            string serverName,
            ushort endpoint,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            rpc.Bind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                endpoint.ToString(),
                null,
                securityContext,
                authenticationLevel,
                timeout);

            LsaRpcAdapter lsaRpcAdapter = rpc as LsaRpcAdapter;
            if (lsaRpcAdapter != null)
            {
                context.rpceTransportContext = lsaRpcAdapter.rpceClientTransport.Context;
            }
        }


        /// <summary>
        /// Get the RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpc.Handle;
            }
        }

        #endregion


        #region Properties - Context

        /// <summary>
        /// LSA client context.
        /// </summary>
        public LsaClientContext Context
        {
            get
            {
                return context;
            }
        }

        /// <summary>
        /// Gets session key.
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                byte[] retVal = null;
                FileServiceClientContext fsTransportContext =
                    this.context.RpceTransportContext.FileServiceTransportContext;
                SmbClientContext smbContext = fsTransportContext as SmbClientContext;
                if (smbContext != null
                    && smbContext.Connection != null
                    && smbContext.Connection.SessionTable != null
                    && smbContext.Connection.SessionTable.Count > 0)
                {
                    retVal = smbContext.Connection.SessionTable[0].SessionKey4Smb;
                }

                Smb2ClientGlobalContext smb2Context = fsTransportContext as Smb2ClientGlobalContext;
                if (smb2Context != null
                    && smb2Context.ConnectionTable != null
                    && smb2Context.ConnectionTable.Count > 0)
                {
                    Smb2ClientConnection[] connections =
                        new Smb2ClientConnection[smb2Context.ConnectionTable.Count];
                    smb2Context.ConnectionTable.Values.CopyTo(connections, 0);

                    if (connections[0].SessionTable != null && connections[0].SessionTable.Count > 0)
                    {
                        Smb2ClientSession[] sessions =
                            new Smb2ClientSession[connections[0].SessionTable.Count];

                        connections[0].SessionTable.Values.CopyTo(sessions, 0);

                        retVal = sessions[0].SessionKey;
                    }
                }
                return retVal;
            }
        }

        #endregion


        #region LSA RPC methods

        /// <summary>
        ///  The LsarClose method frees the resources held by a context
        ///  handle that was opened earlier. After response, the
        ///  context handle is no longer usable, and any subsequent
        ///  uses of this handle MUST fail. Opnum: 0 
        /// </summary>
        /// <param name="objectHandle">
        ///  The context handle to be freed.On response, it MUST be set to 0.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarClose(ref IntPtr? objectHandle)
        {
            return rpc.LsarClose(ref objectHandle);
        }


        /// <summary>
        ///  Opnum1NotUsedOnWire method. Opnum: 1 
        /// </summary>
        public void Opnum1NotUsedOnWire()
        {
            rpc.Opnum1NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum5NotUsedOnWire method. Opnum: 5 
        /// </summary>
        public void Opnum5NotUsedOnWire()
        {
            rpc.Opnum5NotUsedOnWire();
        }


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
        /// <param name="systemName">
        ///  SystemName parameter does not have any effect on message 
        ///  processing in any environment. It MUST be ignored on receipt.
        /// </param>
        /// <param name="objectAttributes">
        ///  ObjectAttributes parameter does not have any effect on message 
        ///  processing in any environment. All fields MUST be ignored except 
        ///  RootDirectory which MUST be NULL.
        /// </param>
        /// <param name="desiredAccess">
        ///  DesiredAccess parameter that specifies the requested access rights
        ///  that MUST be granted on the returned PolicyHandle if the request
        ///  is successful.
        /// </param>
        /// <param name="policyHandle">
        ///  An RPC context handle that represents a reference to the abstract 
        ///  data model of a policy object.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarOpenPolicy(
            ushort[] systemName,
            _LSAPR_OBJECT_ATTRIBUTES? objectAttributes,
            ACCESS_MASK desiredAccess,
            out IntPtr? policyHandle)
        {
            return rpc.LsarOpenPolicy(
                systemName,
                objectAttributes,
                desiredAccess,
                out policyHandle);
        }


        /// <summary>
        ///  Opnum9NotUsedOnWire method. Opnum: 9 
        /// </summary>
        public void Opnum9NotUsedOnWire()
        {
            rpc.Opnum9NotUsedOnWire();
        }


        /// <summary>
        ///  The LsarLookupNames method translates a batch of security
        ///  principal names to their SID form. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  14 
        /// </summary>
        /// <param name="policyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="count">
        ///  Number of names in the Names array.The windowsRPC server
        ///  and RPC client limit the Count field of this structure
        ///  to 1,000 (using the range primitive defined in [MS-RPCE])
        ///  in windows_xp_sp2, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. windows_nt_3_1, windows_nt_3_5,
        ///  windows_nt_3_51, windows_nt_4_0, windows_2000, and
        ///  windows_xp do not enforce this restriction.
        /// </param>
        /// <param name="names">
        ///  Contains the security principal names to translate,
        ///  as specified in section 3.1.4.5.
        /// </param>
        /// <param name="referencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="translatedSids">
        ///  On successful return, contains the corresponding SID
        ///  forms for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="lookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="mappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to their SID forms. This
        ///  parameter has no effect on message processing in any
        ///  environment. It MUST be ignored on input.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarLookupNames(
            IntPtr policyHandle,
            uint count,
            _RPC_UNICODE_STRING[] names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains,
            ref _LSAPR_TRANSLATED_SIDS? translatedSids,
            _LSAP_LOOKUP_LEVEL lookupLevel,
            ref System.UInt32? mappedCount)
        {
            return rpc.LsarLookupNames(
                policyHandle,
                count,
                names,
                out referencedDomains,
                ref translatedSids,
                lookupLevel,
                ref mappedCount);
        }


        /// <summary>
        ///  The LsarLookupSids method translates a batch of security
        ///  principalSIDs to their name forms. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  15 
        /// </summary>
        /// <param name="policyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="sidEnumBuffer">
        ///  Contains the SIDs to be translated. The SIDs in this
        ///  structure can be that of users, groups, computers,
        ///  windows-defined well-known security principals, or
        ///  domains.
        /// </param>
        /// <param name="referencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="translatedNames">
        ///  On successful return, contains the corresponding name
        ///  form for security principalSIDs in the SidEnumBuffer
        ///  parameter. It MUST be ignored on input.
        /// </param>
        /// <param name="lookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="mappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to their Name forms. It MUST
        ///  be ignored on input.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarLookupSids(
            IntPtr policyHandle,
            _LSAPR_SID_ENUM_BUFFER? sidEnumBuffer,
            out _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains,
            ref _LSAPR_TRANSLATED_NAMES? translatedNames,
            _LSAP_LOOKUP_LEVEL lookupLevel,
            ref System.UInt32? mappedCount)
        {
            return rpc.LsarLookupSids(
                policyHandle,
                sidEnumBuffer,
                out referencedDomains,
                ref translatedNames,
                lookupLevel,
                ref mappedCount);
        }


        /// <summary>
        ///  Opnum21NotUsedOnWire method. Opnum: 21 
        /// </summary>
        public void Opnum21NotUsedOnWire()
        {
            rpc.Opnum21NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum22NotUsedOnWire method. Opnum: 22 
        /// </summary>
        public void Opnum22NotUsedOnWire()
        {
            rpc.Opnum22NotUsedOnWire();
        }


        /// <summary>
        ///  The LsarOpenPolicy2 method opens a context handle to
        ///  the RPC server. Opnum: 44 
        /// </summary>
        /// <param name="systemName">
        ///  SystemName parameter does not have any effect on message 
        ///  processing in any environment. It MUST be ignored on receipt.
        /// </param>
        /// <param name="objectAttributes">
        ///  ObjectAttributes parameter does not have any effect on message 
        ///  processing in any environment. All fields MUST be ignored except 
        ///  RootDirectory which MUST be NULL.
        /// </param>
        /// <param name="desiredAccess">
        ///  DesiredAccess parameter that specifies the requested access rights
        ///  that MUST be granted on the returned PolicyHandle if the request
        ///  is successful.
        /// </param>
        /// <param name="policyHandle">
        ///  An RPC context handle that represents a reference to the abstract 
        ///  data model of a policy object.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarOpenPolicy2(
            string systemName,
            _LSAPR_OBJECT_ATTRIBUTES? objectAttributes,
            ACCESS_MASK desiredAccess,
            out IntPtr? policyHandle)
        {
            return rpc.LsarOpenPolicy2(
                systemName,
                objectAttributes,
                desiredAccess,
                out policyHandle);
        }


        /// <summary>
        ///  The LsarGetUserName method returns the name and the
        ///  domain name of the security principal that is invoking
        ///  the method. Opnum: 45 
        /// </summary>
        /// <param name="systemName">
        ///  This parameter has no effect on message processing in
        ///  any environment. It MUST be ignored.
        /// </param>
        /// <param name="userName">
        ///  On return, contains the name of the security principal
        ///  that is making the call. The string MUST be of the
        ///  form sAMAccountName. On input, this parameter MUST
        ///  be ignored. The RPC_UNICODE_STRING structure is defined
        ///  in [MS-DTYP] section 2.3.6.
        /// </param>
        /// <param name="domainName">
        ///  On return, contains the domain name of the security
        ///  principal that is invoking the method. This string
        ///  MUST be a NetBIOS name. On input, this parameter MUST
        ///  be ignored.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarGetUserName(
            string systemName,
            ref _RPC_UNICODE_STRING? userName,
            ref _RPC_UNICODE_STRING? domainName)
        {
            return rpc.LsarGetUserName(
                systemName,
                ref userName,
                ref domainName);
        }


        /// <summary>
        ///  Opnum52NotUsedOnWire method. Opnum: 52 
        /// </summary>
        public void Opnum52NotUsedOnWire()
        {
            rpc.Opnum52NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum56NotUsedOnWire method. Opnum: 56 
        /// </summary>
        public void Opnum56NotUsedOnWire()
        {
            rpc.Opnum56NotUsedOnWire();
        }


        /// <summary>
        ///  The LsarLookupSids2 method translates a batch of security
        ///  principalSIDs to their name forms. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  57 
        /// </summary>
        /// <param name="policyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="sidEnumBuffer">
        ///  Contains the SIDs to be translated. The SIDs in this
        ///  structure can be that of users, groups, computers,
        ///  windows-defined well-known security principals, or
        ///  domains.
        /// </param>
        /// <param name="referencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="translatedNames">
        ///  On successful return, contains the corresponding name
        ///  forms for security principalSIDs in the SidEnumBuffer
        ///  parameter. It MUST be ignored on input.
        /// </param>
        /// <param name="lookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="mappedCount">
        ///  On return, contains the number of names that are translated
        ///  completely to their Name forms. It MUST be ignored
        ///  on input.
        /// </param>
        /// <param name="lookupOptions">
        ///  Flags that control the lookup operation. This parameter
        ///  is reserved for future use and SHOULDThe windowsRPC
        ///  client sets LookupOptions to 0. be set to 0.
        /// </param>
        /// <param name="clientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarLookupSids2(
            IntPtr policyHandle,
            _LSAPR_SID_ENUM_BUFFER? sidEnumBuffer,
            out _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains,
            ref _LSAPR_TRANSLATED_NAMES_EX? translatedNames,
            _LSAP_LOOKUP_LEVEL lookupLevel,
            ref System.UInt32? mappedCount,
            uint lookupOptions,
            ClientRevision_Values clientRevision)
        {
            return rpc.LsarLookupSids2(
                policyHandle,
                sidEnumBuffer,
                out referencedDomains,
                ref translatedNames,
                lookupLevel,
                ref mappedCount,
                lookupOptions,
                clientRevision);
        }


        /// <summary>
        ///  The LsarLookupNames2 method translates a batch of security
        ///  principal names to their SID form. It also returns
        ///  the domains that these names are a part of.A windowsRPC
        ///  server can optionally be configured to deny this call,
        ///  and the error returned in this case is STATUS_NOT_SUPPORTED.
        ///  Opnum: 58 
        /// </summary>
        /// <param name="policyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="count">
        ///  Number of security principal names to look up.The windowsRPC
        ///  server and RPC client limit the Count field of this
        ///  structure to 1,000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </param>
        /// <param name="names">
        ///  Contains the security principal names to translate,
        ///  as specified in section 3.1.4.5.
        /// </param>
        /// <param name="referencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="translatedSids">
        ///  On successful return, contains the corresponding SID
        ///  forms for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="lookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="mappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to the SID form. This parameter
        ///  has no effect on message processing in any environment.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="lookupOptions">
        ///  Flags that control the lookup operation. For possible
        ///  values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <param name="clientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarLookupNames2(
            IntPtr policyHandle,
            uint count,
            _RPC_UNICODE_STRING[] names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains,
            ref _LSAPR_TRANSLATED_SIDS_EX? translatedSids,
            _LSAP_LOOKUP_LEVEL lookupLevel,
            ref System.UInt32? mappedCount,
            LookupOptions_Values lookupOptions,
            ClientRevision_Values clientRevision)
        {
            return rpc.LsarLookupNames2(
                policyHandle,
                count,
                names,
                out referencedDomains,
                ref translatedSids,
                lookupLevel,
                ref mappedCount,
                lookupOptions,
                clientRevision);
        }


        /// <summary>
        ///  Opnum60NotUsedOnWire method. Opnum: 60 
        /// </summary>
        public void Opnum60NotUsedOnWire()
        {
            rpc.Opnum60NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum61NotUsedOnWire method. Opnum: 61 
        /// </summary>
        public void Opnum61NotUsedOnWire()
        {
            rpc.Opnum61NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum62NotUsedOnWire method. Opnum: 62 
        /// </summary>
        public void Opnum62NotUsedOnWire()
        {
            rpc.Opnum62NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum63NotUsedOnWire method. Opnum: 63 
        /// </summary>
        public void Opnum63NotUsedOnWire()
        {
            rpc.Opnum63NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum64NotUsedOnWire method. Opnum: 64 
        /// </summary>
        public void Opnum64NotUsedOnWire()
        {
            rpc.Opnum64NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum65NotUsedOnWire method. Opnum: 65 
        /// </summary>
        public void Opnum65NotUsedOnWire()
        {
            rpc.Opnum65NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum66NotUsedOnWire method. Opnum: 66 
        /// </summary>
        public void Opnum66NotUsedOnWire()
        {
            rpc.Opnum66NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum67NotUsedOnWire method. Opnum: 67 
        /// </summary>
        public void Opnum67NotUsedOnWire()
        {
            rpc.Opnum67NotUsedOnWire();
        }


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
        /// <param name="policyHandle">
        ///  Context handle obtained by an LsarOpenPolicy or LsarOpenPolicy2
        ///  call.
        /// </param>
        /// <param name="count">
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
        /// <param name="names">
        ///  Contains the security principal names to translate,
        ///  as specified in section 3.1.4.5.
        /// </param>
        /// <param name="referencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="translatedSids">
        ///  On successful return, contains the corresponding SID
        ///  forms for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="lookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="mappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to the SID form. This parameter
        ///  has no effect on message processing in any environment.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="lookupOptions">
        ///  Flags that control the lookup operation. For possible
        ///  values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <param name="clientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarLookupNames3(
            IntPtr policyHandle,
            uint count,
            _RPC_UNICODE_STRING[] names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains,
            ref _LSAPR_TRANSLATED_SIDS_EX2? translatedSids,
            _LSAP_LOOKUP_LEVEL lookupLevel,
            ref System.UInt32? mappedCount,
            LookupOptions_Values lookupOptions,
            ClientRevision_Values clientRevision)
        {
            return rpc.LsarLookupNames3(
                policyHandle,
                count,
                names,
                out referencedDomains,
                ref translatedSids,
                lookupLevel,
                ref mappedCount,
                lookupOptions,
                clientRevision);
        }


        /// <summary>
        ///  Opnum69NotUsedOnWire method. Opnum: 69 
        /// </summary>
        public void Opnum69NotUsedOnWire()
        {
            rpc.Opnum69NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum70NotUsedOnWire method. Opnum: 70 
        /// </summary>
        public void Opnum70NotUsedOnWire()
        {
            rpc.Opnum70NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum71NotUsedOnWire method. Opnum: 71 
        /// </summary>
        public void Opnum71NotUsedOnWire()
        {
            rpc.Opnum71NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum72NotUsedOnWire method. Opnum: 72 
        /// </summary>
        public void Opnum72NotUsedOnWire()
        {
            rpc.Opnum72NotUsedOnWire();
        }


        /// <summary>
        ///  Opnum75NotUsedOnWire method. Opnum: 75 
        /// </summary>
        public void Opnum75NotUsedOnWire()
        {
            rpc.Opnum75NotUsedOnWire();
        }


        /// <summary>
        ///  The LsarLookupSids3 method translates a batch of security
        ///  principalSIDs to their name forms. It also returns
        ///  the domains that these names are a part of. Opnum :
        ///  76 
        /// </summary>
        /// <param name="rpcHandle">
        ///  An RPC binding handle, as described in [C706]. RPC binding
        ///  handles are used by RPC internally and are not transmitted
        ///  over the network.This handle can be obtained by calling
        ///  RPC runtime binding routines. For more information,
        ///  see [MSDN-RPCDB].
        /// </param>
        /// <param name="sidEnumBuffer">
        ///  Contains the SIDs to be translated. The SIDs in this
        ///  structure can be that of users, groups, computers,
        ///  windows-defined well-known security principals, or
        ///  domains.
        /// </param>
        /// <param name="referencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="translatedNames">
        ///  On successful return, contains the corresponding name
        ///  forms for security principalSIDs in the SidEnumBuffer
        ///  parameter. It MUST be ignored on input.
        /// </param>
        /// <param name="lookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="mappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to their Name forms. It MUST
        ///  be ignored on input.
        /// </param>
        /// <param name="lookupOptions">
        ///  Flags that control the lookup operation. This parameter
        ///  is reserved for future use; it MUST be set to 0 and
        ///  ignored on receipt.
        /// </param>
        /// <param name="clientRevision">
        ///  Version of the client, which implies the client's capabilities.
        ///  For possible values and their meanings, see section 3.1.4.5.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarLookupSids3(
            IntPtr rpcHandle,
            _LSAPR_SID_ENUM_BUFFER? sidEnumBuffer,
            out _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains,
            ref _LSAPR_TRANSLATED_NAMES_EX? translatedNames,
            _LSAP_LOOKUP_LEVEL lookupLevel,
            ref System.UInt32? mappedCount,
            uint lookupOptions,
            ClientRevision_Values clientRevision)
        {
            return rpc.LsarLookupSids3(
                rpcHandle,
                sidEnumBuffer,
                out referencedDomains,
                ref translatedNames,
                lookupLevel,
                ref mappedCount,
                lookupOptions,
                clientRevision);
        }


        /// <summary>
        ///  The LsarLookupNames4 method translates a batch of security
        ///  principal names to their SID form. It also returns
        ///  the domains of which these security principals are
        ///  a part. Opnum: 77 
        /// </summary>
        /// <param name="rpcHandle">
        ///  This value is used by RPC internally and is not transmitted
        ///  over the network, as specified in [C706]. This handle
        ///  can be obtained by calling RPC runtime binding routines.
        ///  For more information, see [MSDN-RPCDB].
        /// </param>
        /// <param name="count">
        ///  Number of security principal names to look up.The windowsRPC
        ///  server and RPC client limit the Count field of this
        ///  structure to 1,000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </param>
        /// <param name="names">
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
        /// <param name="referencedDomains">
        ///  On successful return, contains the domain information
        ///  for the domain to which each security principal belongs.
        ///  The domain information includes a NetBIOS domain name
        ///  and a domainSID for each entry in the list.
        /// </param>
        /// <param name="translatedSids">
        ///  On successful return, contains the corresponding SID
        ///  form for security principal names in the Names parameter.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="lookupLevel">
        ///  Specifies what scopes are to be used during translation,
        ///  as specified in section 2.2.16.
        /// </param>
        /// <param name="mappedCount">
        ///  On successful return, contains the number of names that
        ///  are translated completely to the SID form. This parameter
        ///  is left as an input parameter for backward compatibility
        ///  and has no effect on message processing in any environment.
        ///  It MUST be ignored on input.
        /// </param>
        /// <param name="lookupOptions">
        ///  Flags specified by the caller that control the lookup
        ///  operation. The value MUST be one of the following.
        /// </param>
        /// <param name="clientRevision">
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
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        [CLSCompliant(false)]
        public NtStatus LsarLookupNames4(
            IntPtr rpcHandle,
            uint count,
            _RPC_UNICODE_STRING[] names,
            out _LSAPR_REFERENCED_DOMAIN_LIST? referencedDomains,
            ref _LSAPR_TRANSLATED_SIDS_EX2? translatedSids,
            _LSAP_LOOKUP_LEVEL lookupLevel,
            ref System.UInt32? mappedCount,
            LookupOptions_Values lookupOptions,
            ClientRevision_Values clientRevision)
        {
            return rpc.LsarLookupNames4(
                rpcHandle,
                count,
                names,
                out referencedDomains,
                ref translatedSids,
                lookupLevel,
                ref mappedCount,
                lookupOptions,
                clientRevision);
        }


        #region LSAD
        /// <summary>
        ///  The LsarEnumeratePrivileges method is invoked to enumerate
        ///  all privileges known to the system. This method can
        ///  be called multiple times to return its output in fragments.
        ///  Opnum: 2 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="enumerationContext">
        ///  A pointer to a context value that is used to resume
        ///  enumeration, if necessary.
        /// </param>
        /// <param name="enumerationBuffer">
        ///  A pointer to a structure that will contain the results
        ///  of the enumeration.
        /// </param>
        /// <param name="preferedMaximumLength">
        ///  The preferred maximum length of returned data, in bytes.
        ///  This is not a strict upper limit but serves as a guide.
        ///  It is valid for the actual amount of data returned
        ///  to be greater than this value.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarEnumeratePrivileges(
            System.IntPtr policyHandle,
            ref System.UInt32? enumerationContext,
            out _LSAPR_PRIVILEGE_ENUM_BUFFER? enumerationBuffer,
            uint preferedMaximumLength)
        {
            return rpc.LsarEnumeratePrivileges(
                policyHandle,
                ref enumerationContext,
                out enumerationBuffer,
                preferedMaximumLength);
        }


        /// <summary>
        ///  The LsarQuerySecurityObject method is invoked to query
        ///  security information that is assigned to a database
        ///  object. It returns the security descriptor of the object.
        ///  Opnum: 3 
        /// </summary>
        /// <param name="objectHandle">
        ///  An open object handle of any type.
        /// </param>
        /// <param name="securityInformation">
        ///  A bitmask specifying which portions of the security
        ///  descriptor the caller is interested in.
        /// </param>
        /// <param name="securityDescriptor">
        ///  Used to return the security descriptor containing the
        ///  elements requested by the caller.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQuerySecurityObject(
            System.IntPtr objectHandle,
            SECURITY_INFORMATION securityInformation,
            out _LSAPR_SR_SECURITY_DESCRIPTOR? securityDescriptor)
        {
            return rpc.LsarQuerySecurityObject(
                objectHandle,
                securityInformation,
                out securityDescriptor);
        }


        /// <summary>
        ///  The LsarSetSecurityObject method is invoked to set a
        ///  security descriptor on an object. Opnum: 4 
        /// </summary>
        /// <param name="objectHandle">
        ///  An open handle to an existing object.
        /// </param>
        /// <param name="securityInformation">
        ///  A bitmask specifying which portions of the security
        ///  descriptor are to be set.
        /// </param>
        /// <param name="securityDescriptor">
        ///  The security descriptor to be set.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetSecurityObject(
            System.IntPtr objectHandle,
            SECURITY_INFORMATION securityInformation,
            _LSAPR_SR_SECURITY_DESCRIPTOR? securityDescriptor)
        {
            return rpc.LsarSetSecurityObject(
                objectHandle,
                securityInformation,
                securityDescriptor);
        }


        /// <summary>
        ///  The LsarQueryInformationPolicy method is invoked to
        ///  query values that represent the server's information
        ///  policy. Opnum: 7 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="informationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is requesting.
        /// </param>
        /// <param name="policyInformation">
        ///  A parameter that references policy information structure
        ///  on return.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQueryInformationPolicy(
            System.IntPtr policyHandle,
            _POLICY_INFORMATION_CLASS informationClass,
            out _LSAPR_POLICY_INFORMATION? policyInformation)
        {
            return rpc.LsarQueryInformationPolicy(
                policyHandle,
                informationClass,
                out policyInformation);
        }


        /// <summary>
        ///  The LsarSetInformationPolicy method is invoked to set
        ///  a policy on the server. Opnum: 8 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="informationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is setting.
        /// </param>
        /// <param name="policyInformation">
        ///  Data that represents the policy being set.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetInformationPolicy(
            System.IntPtr policyHandle,
            _POLICY_INFORMATION_CLASS informationClass,
            _LSAPR_POLICY_INFORMATION? policyInformation)
        {
            return rpc.LsarSetInformationPolicy(
                policyHandle,
                informationClass,
                policyInformation);
        }


        /// <summary>
        ///  The LsarCreateAccount method is invoked to create a
        ///  new account object in the server's database. Opnum
        ///  : 10 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="accountSid">
        ///  The security identifier (SID) of the account to be created.
        /// </param>
        /// <param name="desiredAccess">
        ///  A bitmask specifying accesses to be granted to the newly
        ///  created and opened account at this time.
        /// </param>
        /// <param name="accountHandle">
        ///  Used to return a handle to the newly created account
        ///  object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarCreateAccount(
            System.IntPtr policyHandle,
            _RPC_SID? accountSid,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? accountHandle)
        {
            return rpc.LsarCreateAccount(
                policyHandle,
                accountSid,
                desiredAccess,
                out accountHandle);
        }


        /// <summary>
        ///  The LsarEnumerateAccounts method is invoked to request
        ///  a list of account objects in the server's database.
        ///  The method can be called multiple times to return its
        ///  output in fragments. Opnum: 11 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="enumerationContext">
        ///  A pointer to a context value that is used to resume
        ///  enumeration, if necessary.
        /// </param>
        /// <param name="enumerationBuffer">
        ///  A pointer to a structure that will contain the results
        ///  of the enumeration.
        /// </param>
        /// <param name="preferedMaximumLength">
        ///  The preferred maximum length of returned data, in bytes.
        ///  This is not a strict upper limit, but serves as a guide.
        ///  It is valid for the actual amount of data that is returned
        ///  to be greater than this value.The windows implementation
        ///  of this method may exceed the preferred maximum length
        ///  specified by the caller.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarEnumerateAccounts(
            System.IntPtr policyHandle,
            ref System.UInt32? enumerationContext,
            out _LSAPR_ACCOUNT_ENUM_BUFFER? enumerationBuffer,
            uint preferedMaximumLength)
        {
            return rpc.LsarEnumerateAccounts(
                policyHandle,
                ref enumerationContext,
                out enumerationBuffer,
                preferedMaximumLength);
        }


        /// <summary>
        ///  The LsarCreateTrustedDomain method is invoked to create
        ///  an object of type trusted domain in the server's database.
        ///  Opnum: 12 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Information about the new trusted domain object to be
        ///  created.
        /// </param>
        /// <param name="desiredAccess">
        ///  An access mask that specifies the desired access to
        ///  the trusted domain object handle.
        /// </param>
        /// <param name="trustedDomainHandle">
        ///  Used to return the handle for the newly created trusted
        ///  domain object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarCreateTrustedDomain(System.IntPtr policyHandle,
            _LSAPR_TRUST_INFORMATION? trustedDomainInformation,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? trustedDomainHandle)
        {
            return rpc.LsarCreateTrustedDomain(
                policyHandle,
                trustedDomainInformation,
                desiredAccess,
                out trustedDomainHandle);
        }


        /// <summary>
        ///  The LsarEnumerateTrustedDomains method is invoked to
        ///  request a list of trusted domain objects in the server's
        ///  database. The method can be called multiple times to
        ///  return its output in fragments. Opnum: 13 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="enumerationContext">
        ///  A pointer to a context value that is used to resume
        ///  enumeration, if necessary.
        /// </param>
        /// <param name="enumerationBuffer">
        ///  A pointer to a structure that will contain the results
        ///  of the enumeration.
        /// </param>
        /// <param name="preferedMaximumLength">
        ///  The preferred maximum length of returned data, in bytes.
        ///  This is not a strict upper limit but serves as a guide.
        ///  It is valid for the actual amount of data returned
        ///  to be greater than this value.The windows implementation
        ///  of this method might exceed the maximum desired length
        ///  specified by the caller.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarEnumerateTrustedDomains(
            System.IntPtr policyHandle,
            ref System.UInt32? enumerationContext,
            out _LSAPR_TRUSTED_ENUM_BUFFER? enumerationBuffer,
            uint preferedMaximumLength)
        {
            return rpc.LsarEnumerateTrustedDomains(
                policyHandle,
                ref enumerationContext,
                out enumerationBuffer,
                preferedMaximumLength);
        }


        /// <summary>
        ///  The LsarCreateSecret method is invoked to create a new
        ///  secret object in the server's database. Opnum: 16
        ///  
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="secretName">
        ///  The name of the secret object to be created.
        /// </param>
        /// <param name="desiredAccess">
        ///  A bitmask that specifies accesses to be granted to the
        ///  newly created and opened secret object at this time.
        /// </param>
        /// <param name="secretHandle">
        ///  Used to return a handle to the newly created account
        ///  object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarCreateSecret(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? secretName,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? secretHandle)
        {
            return rpc.LsarCreateSecret(
                policyHandle,
                secretName,
                desiredAccess,
                out secretHandle);
        }


        /// <summary>
        ///  The LsarOpenAccount method is invoked to obtain a handle
        ///  to an account object. Opnum: 17 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="accountSid">
        ///  A SID of the account to be opened.
        /// </param>
        /// <param name="desiredAccess">
        ///  A bitmask specifying accesses to be granted to the opened
        ///  account at this time.
        /// </param>
        /// <param name="accountHandle">
        ///  Used to return a handle to the opened account object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarOpenAccount(
            System.IntPtr policyHandle,
            _RPC_SID? accountSid,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? accountHandle)
        {
            return rpc.LsarOpenAccount(
                policyHandle,
                accountSid,
                desiredAccess,
                out accountHandle);
        }


        /// <summary>
        ///  The LsarEnumeratePrivilegesAccount method is invoked
        ///  to retrieve a list of privileges granted to an account
        ///  on the server. Opnum: 18 
        /// </summary>
        /// <param name="accountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="privileges">
        ///  Used to return a list of privileges granted to the account.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarEnumeratePrivilegesAccount(
            System.IntPtr accountHandle,
            out _LSAPR_PRIVILEGE_SET? privileges)
        {
            return rpc.LsarEnumeratePrivilegesAccount(
                accountHandle,
                out privileges);
        }


        /// <summary>
        ///  The LsarAddPrivilegesToAccount method is invoked to
        ///  add new privileges to an existing account object. Opnum
        ///  : 19 
        /// </summary>
        /// <param name="accountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="privileges">
        ///  Contains a list of privileges to add to the account.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarAddPrivilegesToAccount(
            System.IntPtr accountHandle,
            _LSAPR_PRIVILEGE_SET? privileges)
        {
            return rpc.LsarAddPrivilegesToAccount(
                accountHandle,
                privileges);
        }


        /// <summary>
        ///  The LsarRemovePrivilegesFromAccount method is invoked
        ///  to remove privileges from an account object. Opnum
        ///  : 20 
        /// </summary>
        /// <param name="accountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="allPrivileges">
        ///  If this parameter is not FALSE (0), all privileges will
        ///  be stripped from the account object.
        /// </param>
        /// <param name="privileges">
        ///  Contains a (possibly empty) list of privileges to remove
        ///  from the account object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarRemovePrivilegesFromAccount(
            System.IntPtr accountHandle,
            byte allPrivileges,
            _LSAPR_PRIVILEGE_SET? privileges)
        {
            return rpc.LsarRemovePrivilegesFromAccount(
                accountHandle,
                allPrivileges,
                privileges);
        }


        /// <summary>
        ///  The LsarGetSystemAccessAccount method is invoked to
        ///  retrieve system access account flags for an account
        ///  object. System access account flags are described as
        ///  part of the account object data model, as specified
        ///  in section. Opnum: 23 
        /// </summary>
        /// <param name="accountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="systemAccess">
        ///  Used to return a bitmask of access flags associated
        ///  with the account.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarGetSystemAccessAccount(
            System.IntPtr accountHandle,
            out System.UInt32? systemAccess)
        {
            return rpc.LsarGetSystemAccessAccount(
                accountHandle,
                out systemAccess);
        }


        /// <summary>
        ///  The LsarSetSystemAccessAccount method is invoked to
        ///  set system access account flags for an account object.
        ///  Opnum: 24 
        /// </summary>
        /// <param name="accountHandle">
        ///  An open account object handle obtained from either LsarCreateAccount
        ///  or LsarOpenAccount.
        /// </param>
        /// <param name="systemAccess">
        ///  A bitmask containing the account flags to be set on
        ///  the account.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetSystemAccessAccount(
            System.IntPtr accountHandle,
            System.UInt32? systemAccess)
        {
            return rpc.LsarSetSystemAccessAccount(
                accountHandle,
                systemAccess);
        }


        /// <summary>
        ///  The LsarOpenTrustedDomain method is invoked to obtain
        ///  a handle to a trusted domain object. Opnum: 25 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainSid">
        ///  A security identifier of the trusted domain that is
        ///  being opened.
        /// </param>
        /// <param name="desiredAccess">
        ///  A bitmask of access rights to open the object with.
        /// </param>
        /// <param name="trustedDomainHandle">
        ///  Used to return the trusted domain object handle.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarOpenTrustedDomain(
            System.IntPtr policyHandle,
            _RPC_SID? trustedDomainSid,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? trustedDomainHandle)
        {
            return rpc.LsarOpenTrustedDomain(
                policyHandle,
                trustedDomainSid,
                desiredAccess,
                out trustedDomainHandle);
        }


        /// <summary>
        ///  The LsarQueryInfoTrustedDomain method is invoked to
        ///  retrieve information about the trusted domain object.
        ///  Opnum: 26 
        /// </summary>
        /// <param name="trustedDomainHandle">
        ///  An open trusted domain object handle.
        /// </param>
        /// <param name="informationClass">
        ///  One of the TRUSTED_INFORMATION_CLASS values indicating
        ///  the type of information the caller is interested in.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Used to return requested information about the trusted
        ///  domain object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQueryInfoTrustedDomain(
            System.IntPtr trustedDomainHandle,
            _TRUSTED_INFORMATION_CLASS informationClass,
            out _LSAPR_TRUSTED_DOMAIN_INFO? trustedDomainInformation)
        {
            return rpc.LsarQueryInfoTrustedDomain(
                trustedDomainHandle,
                informationClass,
                out trustedDomainInformation);
        }


        /// <summary>
        ///  The LsarSetInformationTrustedDomain method is invoked
        ///  to set information on a trusted domain object. Opnum
        ///  : 27 
        /// </summary>
        /// <param name="trustedDomainHandle">
        ///  A handle to a trusted domain object.
        /// </param>
        /// <param name="informationClass">
        ///  A value indicating the type of information requested
        ///  by the caller.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Used to supply the information to be set.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetInformationTrustedDomain(
            System.IntPtr trustedDomainHandle,
            _TRUSTED_INFORMATION_CLASS informationClass,
            _LSAPR_TRUSTED_DOMAIN_INFO? trustedDomainInformation)
        {
            return rpc.LsarSetInformationTrustedDomain(
                trustedDomainHandle,
                informationClass,
                trustedDomainInformation);
        }


        /// <summary>
        ///  The LsarOpenSecret method is invoked to obtain a handle
        ///  to an existing secret object. Opnum: 28 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="secretName">
        ///  The name of the secret object to open.
        /// </param>
        /// <param name="desiredAccess">
        ///  The requested type of access.
        /// </param>
        /// <param name="secretHandle">
        ///  Used to return the handle to the opened secret object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarOpenSecret(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? secretName,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? secretHandle)
        {
            return rpc.LsarOpenSecret(
                policyHandle,
                secretName,
                desiredAccess,
                out secretHandle);
        }


        /// <summary>
        ///  The LsarSetSecret method is invoked to set the current
        ///  and old values of the secret object. Opnum: 29 
        /// </summary>
        /// <param name="secretHandle">
        ///  An open secret object handle.
        /// </param>
        /// <param name="encryptedCurrentValue">
        ///  A binary large object (BLOB) representing a new encrypted
        ///  cipher value. It is valid for this parameter to be
        ///  NULL, in which case the value is deleted from the server's
        ///  policy database.
        /// </param>
        /// <param name="encryptedOldValue">
        ///  A BLOB representing the encrypted old value. It is valid
        ///  for this parameter to be NULL, in which case the current
        ///  value in the policy database is copied.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetSecret(
            System.IntPtr secretHandle,
            _LSAPR_CR_CIPHER_VALUE? encryptedCurrentValue,
            _LSAPR_CR_CIPHER_VALUE? encryptedOldValue)
        {
            return rpc.LsarSetSecret(
                secretHandle,
                encryptedCurrentValue,
                encryptedOldValue);
        }


        /// <summary>
        ///  The LsarQuerySecret method is invoked to retrieve the
        ///  current and old (or previous) value of the secret object.
        ///  Opnum: 30 
        /// </summary>
        /// <param name="secretHandle">
        ///  An open secret object handle.
        /// </param>
        /// <param name="encryptedCurrentValue">
        ///  Used to return the encrypted current value of the secret
        ///  object.
        /// </param>
        /// <param name="currentValueSetTime">
        ///  Used to return the time when the current value was set.
        /// </param>
        /// <param name="encryptedOldValue">
        ///  A BLOB representing the encrypted old value. It is valid
        ///  for this parameter to be NULL, in which case the current
        ///  value in the policy database is copied.
        /// </param>
        /// <param name="oldValueSetTime">
        ///  The time corresponding to the instant that the old value
        ///  was last changed.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQuerySecret(
            System.IntPtr secretHandle,
            ref _LSAPR_CR_CIPHER_VALUE? encryptedCurrentValue,
            ref _LARGE_INTEGER? currentValueSetTime,
            ref _LSAPR_CR_CIPHER_VALUE? encryptedOldValue,
            ref _LARGE_INTEGER? oldValueSetTime)
        {
            return rpc.LsarQuerySecret(
                secretHandle,
                ref encryptedCurrentValue,
                ref currentValueSetTime,
                ref encryptedOldValue,
                ref oldValueSetTime);
        }


        /// <summary>
        ///  The LsarLookupPrivilegeValue method is invoked to map
        ///  the name of a privilege into a locally unique identifier
        ///  (LUID) by which the privilege is known on the server.
        ///  The locally unique value of the privilege can then
        ///  be used in subsequent calls to other methods, such
        ///  as LsarAddPrivilegesToAccount. Opnum: 31 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="name">
        ///  A string containing the name of a privilege.
        /// </param>
        /// <param name="value">
        ///  Used to return a LUID assigned by the server to the
        ///  privilege by this name.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarLookupPrivilegeValue(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? name,
            out _LUID? value)
        {
            return rpc.LsarLookupPrivilegeValue(
                policyHandle,
                name,
                out value);
        }


        /// <summary>
        ///  The LsarLookupPrivilegeName method is invoked to map
        ///  the LUID of a privilege into a string name by which
        ///  the privilege is known on the server. Opnum: 32 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="value">
        ///  A LUID that the caller wishes to map to a string name.
        /// </param>
        /// <param name="name">
        ///  Used to return the string name corresponding to the
        ///  supplied LUID.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarLookupPrivilegeName(
            System.IntPtr policyHandle,
            _LUID? value,
            out _RPC_UNICODE_STRING? name)
        {
            return rpc.LsarLookupPrivilegeName(
                policyHandle,
                value,
                out name);
        }


        /// <summary>
        ///  The LsarLookupPrivilegeDisplayName method is invoked
        ///  to map the name of a privilege into a display text
        ///  string in the caller's language. Opnum: 33 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="name">
        ///  A string containing the name of a privilege.
        /// </param>
        /// <param name="clientLanguage">
        ///  An identifier of the client's language.
        /// </param>
        /// <param name="clientSystemDefaultLanguage">
        ///  An identifier of the default language of the caller's
        ///  machine.
        /// </param>
        /// <param name="displayName">
        ///  Used to return the display name of the privilege in
        ///  the language pointed to by the LanguageReturned value.
        /// </param>
        /// <param name="languageReturned">
        ///  An identifier of the language in which DisplayName was
        ///  returned.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarLookupPrivilegeDisplayName(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? name,
            short clientLanguage,
            short clientSystemDefaultLanguage,
            out _RPC_UNICODE_STRING? displayName,
            out System.UInt16? languageReturned)
        {
            return rpc.LsarLookupPrivilegeDisplayName(
                policyHandle,
                name,
                clientLanguage,
                clientSystemDefaultLanguage,
                out displayName,
                out languageReturned);
        }


        /// <summary>
        ///  The LsarDeleteObject method is invoked to delete an
        ///  open account object, secret object, or trusted domain
        ///  object. Opnum: 34 
        /// </summary>
        /// <param name="objectHandle">
        ///  A handle to an open object of the correct type to be
        ///  deleted. After successful completion of the call, the
        ///  handle value cannot be reused.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarDeleteObject(
            ref System.IntPtr? objectHandle)
        {
            return rpc.LsarDeleteObject(
                ref objectHandle);
        }


        /// <summary>
        ///  The LsarEnumerateAccountsWithUserRight method is invoked
        ///  to return a list of account objects that have the user
        ///  right equal to the passed-in value. Opnum: 35 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="userRight">
        ///  The name of the right to use in enumeration.
        /// </param>
        /// <param name="enumerationBuffer">
        ///  Used to return the list of account objects that have
        ///  the specified right.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarEnumerateAccountsWithUserRight(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? userRight,
            out _LSAPR_ACCOUNT_ENUM_BUFFER? enumerationBuffer)
        {
            return rpc.LsarEnumerateAccountsWithUserRight(
                policyHandle,
                userRight,
                out enumerationBuffer);
        }


        /// <summary>
        ///  The LsarEnumerateAccountRights method is invoked to
        ///  retrieve a list of rights associated with an existing
        ///  account. Opnum: 36 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="accountSid">
        ///  A SID of the account object that the caller is inquiring
        ///  about.
        /// </param>
        /// <param name="userRights">
        ///  Used to return a list of right names associated with
        ///  the account.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarEnumerateAccountRights(
            System.IntPtr policyHandle,
            _RPC_SID? accountSid,
            out _LSAPR_USER_RIGHT_SET? userRights)
        {
            return rpc.LsarEnumerateAccountRights(
                policyHandle,
                accountSid,
                out userRights);
        }


        /// <summary>
        ///  The LsarAddAccountRights method is invoked to add new
        ///  rights to an account object. If the account object
        ///  does not exist, the system will attempt to create one.
        ///  Opnum: 37 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="accountSid">
        ///  A security identifier of an account to add the rights
        ///  to.
        /// </param>
        /// <param name="userRights">
        ///  A set of right names to add to the account.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarAddAccountRights(
            System.IntPtr policyHandle,
            _RPC_SID? accountSid,
            _LSAPR_USER_RIGHT_SET? userRights)
        {
            return rpc.LsarAddAccountRights(
                policyHandle,
                accountSid,
                userRights);
        }


        /// <summary>
        ///  The LsarRemoveAccountRights method is invoked to remove
        ///  rights from an account object. Opnum: 38 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="accountSid">
        ///  A security descriptor of an account object.
        /// </param>
        /// <param name="allRights">
        ///  If this field is not set to 0, all rights will be removed.
        /// </param>
        /// <param name="userRights">
        ///  A set of rights to remove from the account.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarRemoveAccountRights(
            System.IntPtr policyHandle,
            _RPC_SID? accountSid,
            byte allRights,
            _LSAPR_USER_RIGHT_SET? userRights)
        {
            return rpc.LsarRemoveAccountRights(
                policyHandle,
                accountSid,
                allRights,
                userRights);
        }


        /// <summary>
        ///  The LsarQueryTrustedDomainInfo method is invoked to
        ///  retrieve information on a trusted domain object. Opnum
        ///  : 39 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainSid">
        ///  A security descriptor of the trusted domain object.
        /// </param>
        /// <param name="informationClass">
        ///  Identifies the type of information the caller is interested
        ///  in.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Used to return the information on the trusted domain
        ///  object to the caller.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQueryTrustedDomainInfo(
            System.IntPtr policyHandle,
            _RPC_SID? trustedDomainSid,
            _TRUSTED_INFORMATION_CLASS informationClass,
            out _LSAPR_TRUSTED_DOMAIN_INFO? trustedDomainInformation)
        {
            return rpc.LsarQueryTrustedDomainInfo(
                policyHandle,
                trustedDomainSid,
                informationClass,
                out trustedDomainInformation);
        }


        /// <summary>
        ///  The LsarSetTrustedDomainInfo method is invoked to set
        ///  information on a trusted domain object. In some cases,
        ///  if the trusted domain object does not exist, it will
        ///  be created. Opnum: 40 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainSid">
        ///  A SID of the trusted domain object to be modified.
        /// </param>
        /// <param name="informationClass">
        ///  Identifies the type of information to be set on the
        ///  trusted domain object.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Information to be set on the trusted domain object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetTrustedDomainInfo(
            System.IntPtr policyHandle,
            _RPC_SID? trustedDomainSid,
            _TRUSTED_INFORMATION_CLASS informationClass,
            _LSAPR_TRUSTED_DOMAIN_INFO? trustedDomainInformation)
        {
            return rpc.LsarSetTrustedDomainInfo(
                policyHandle,
                trustedDomainSid,
                informationClass,
                trustedDomainInformation);
        }


        /// <summary>
        ///  The LsarDeleteTrustedDomain method is invoked to delete
        ///  a trusted domain object. Opnum: 41 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainSid">
        ///  A security descriptor of the trusted domain object to
        ///  be deleted.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarDeleteTrustedDomain(
            System.IntPtr policyHandle,
            _RPC_SID? trustedDomainSid)
        {
            return rpc.LsarDeleteTrustedDomain(
                policyHandle,
                trustedDomainSid);
        }


        /// <summary>
        ///  The LsarStorePrivateData method is invoked to store
        ///  a secret value. Opnum: 42 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="keyName">
        ///  The name under which private data will be stored.
        /// </param>
        /// <param name="encryptedData">
        ///  The secret value to be stored.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarStorePrivateData(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? keyName,
            _LSAPR_CR_CIPHER_VALUE? encryptedData)
        {
            return rpc.LsarStorePrivateData(
                policyHandle,
                keyName,
                encryptedData);
        }


        /// <summary>
        ///  The LsarRetrievePrivateData method is invoked to retrieve
        ///  a secret value. Opnum: 43 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="keyName">
        ///  The name identifying the secret value to be retrieved.
        /// </param>
        /// <param name="encryptedData">
        ///  Receives the encrypted value of the secret object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarRetrievePrivateData(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? keyName,
            ref _LSAPR_CR_CIPHER_VALUE? encryptedData)
        {
            return rpc.LsarRetrievePrivateData(
                policyHandle,
                keyName,
                ref encryptedData);
        }


        /// <summary>
        ///  The LsarQueryInformationPolicy2 method is invoked to
        ///  query values that represent the server's security policy.
        ///  Opnum: 46 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="informationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is requesting.
        /// </param>
        /// <param name="policyInformation">
        ///  A parameter that references policy information structure
        ///  on return.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQueryInformationPolicy2(
            System.IntPtr policyHandle,
            _POLICY_INFORMATION_CLASS informationClass,
            out _LSAPR_POLICY_INFORMATION? policyInformation)
        {
            return rpc.LsarQueryInformationPolicy2(
                policyHandle,
                informationClass,
                out policyInformation);
        }


        /// <summary>
        ///  The LsarSetInformationPolicy2 method is invoked to set
        ///  a policy on the server. Opnum: 47 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="informationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is setting.
        /// </param>
        /// <param name="policyInformation">
        ///  Data that represents policy being set.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetInformationPolicy2(
            System.IntPtr policyHandle,
            _POLICY_INFORMATION_CLASS informationClass,
            _LSAPR_POLICY_INFORMATION? policyInformation)
        {
            return rpc.LsarSetInformationPolicy2(
                policyHandle,
                informationClass,
                policyInformation);
        }


        /// <summary>
        ///  The LsarQueryTrustedDomainInfoByName method is invoked
        ///  to retrieve information about a trusted domain object
        ///  by its string name. Opnum: 48 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainName">
        ///  The name of the trusted domain object to query.
        /// </param>
        /// <param name="informationClass">
        ///  One of the TRUSTED_INFORMATION_CLASS values identifying
        ///  the type of information the caller is interested in.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Used to return the information requested by the caller.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQueryTrustedDomainInfoByName(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? trustedDomainName,
            _TRUSTED_INFORMATION_CLASS informationClass,
            out _LSAPR_TRUSTED_DOMAIN_INFO? trustedDomainInformation)
        {
            return rpc.LsarQueryTrustedDomainInfoByName(
                policyHandle,
                trustedDomainName,
                informationClass,
                out trustedDomainInformation);
        }


        /// <summary>
        ///  The LsarSetTrustedDomainInfoByName method is invoked
        ///  to set information about a trusted domain object by
        ///  its string name. Opnum: 49 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainName">
        ///  The name of the trusted domain object to set information
        ///  on.
        /// </param>
        /// <param name="informationClass">
        ///  One of the TRUSTED_INFORMATION_CLASS values indicating
        ///  the type of information the caller is trying to set.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  The data being set.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetTrustedDomainInfoByName(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? trustedDomainName,
            _TRUSTED_INFORMATION_CLASS informationClass,
            _LSAPR_TRUSTED_DOMAIN_INFO? trustedDomainInformation)
        {
            return rpc.LsarSetTrustedDomainInfoByName(
                policyHandle,
                trustedDomainName,
                informationClass,
                trustedDomainInformation);
        }


        /// <summary>
        ///  The LsarEnumerateTrustedDomainsEx method is invoked
        ///  to enumerate trusted domain objects in the server's
        ///  database. The method is designed to be invoked multiple
        ///  times to retrieve the data in fragments. Opnum: 50
        ///  
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="enumerationContext">
        ///  Used to keep track of the state of the enumeration in
        ///  cases where the caller obtains its information in several
        ///  fragments.
        /// </param>
        /// <param name="enumerationBuffer">
        ///  Contains a fragment of requested information.
        /// </param>
        /// <param name="preferedMaximumLength">
        ///  A value serving as a hint to the server as to the maximum
        ///  amount of data fragment a client would like to receive.
        /// </param>
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [CLSCompliant(false)]
        public NtStatus LsarEnumerateTrustedDomainsEx(
            System.IntPtr policyHandle,
            ref System.UInt32? enumerationContext,
            out _LSAPR_TRUSTED_ENUM_BUFFER_EX? enumerationBuffer,
            uint preferedMaximumLength)
        {
            return rpc.LsarEnumerateTrustedDomainsEx(
                policyHandle,
                ref enumerationContext,
                out enumerationBuffer,
                preferedMaximumLength);
        }


        /// <summary>
        ///  The LsarCreateTrustedDomainEx method is invoked to create
        ///  a new trusted domain object. Opnum: 51 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Information about the new trusted domain object to be
        ///  created.
        /// </param>
        /// <param name="authenticationInformation">
        ///  Encrypted authentication information for the new trusted
        ///  domain object.
        /// </param>
        /// <param name="desiredAccess">
        ///  An access mask that specifies desired access to the
        ///  trusted domain object handle.
        /// </param>
        /// <param name="trustedDomainHandle">
        ///  Used to return the handle for the newly created trusted
        ///  domain object.
        /// </param>
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [CLSCompliant(false)]
        public NtStatus LsarCreateTrustedDomainEx(
            System.IntPtr policyHandle,
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX? trustedDomainInformation,
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION? authenticationInformation,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? trustedDomainHandle)
        {
            return rpc.LsarCreateTrustedDomainEx(
                policyHandle,
                trustedDomainInformation,
                authenticationInformation,
                desiredAccess,
                out trustedDomainHandle);
        }


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
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="informationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is requesting.
        /// </param>
        /// <param name="policyDomainInformation">
        ///  A parameter that references policy information structure
        ///  on return.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQueryDomainInformationPolicy(
            System.IntPtr policyHandle,
            _POLICY_DOMAIN_INFORMATION_CLASS informationClass,
            out _LSAPR_POLICY_DOMAIN_INFORMATION? policyDomainInformation)
        {
            return rpc.LsarQueryDomainInformationPolicy(
                policyHandle,
                informationClass,
                out policyDomainInformation);
        }


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
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="informationClass">
        ///  A parameter that specifies what type of information
        ///  the caller is setting.
        /// </param>
        /// <param name="policyDomainInformation">
        ///  Data representing policy being set.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetDomainInformationPolicy(
            System.IntPtr policyHandle,
            _POLICY_DOMAIN_INFORMATION_CLASS informationClass,
            _LSAPR_POLICY_DOMAIN_INFORMATION? policyDomainInformation)
        {
            return rpc.LsarSetDomainInformationPolicy(
                policyHandle,
                informationClass,
                policyDomainInformation);
        }


        /// <summary>
        ///  The LsarOpenTrustedDomainByName method is invoked to
        ///  open a trusted domain object handle by supplying the
        ///  name of the trusted domain. Opnum: 55 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainName">
        ///  The name of the trusted domain object.
        /// </param>
        /// <param name="desiredAccess">
        ///  The type of access requested by the caller.
        /// </param>
        /// <param name="trustedDomainHandle">
        ///  Used to return the opened trusted domain handle.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarOpenTrustedDomainByName(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? trustedDomainName,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? trustedDomainHandle)
        {
            return rpc.LsarOpenTrustedDomainByName(
                policyHandle,
                trustedDomainName,
                desiredAccess,
                out trustedDomainHandle);
        }


        /// <summary>
        ///  The LsarCreateTrustedDomainEx2 method is invoked to
        ///  create a new trusted domain object.small_business_server_2003does
        ///  not support this message. Attempts to create a trusted
        ///  domain object in this environment causes the server
        ///  to return STATUS_NOT_SUPPORTED_ON_SBS. Opnum: 59 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainInformation">
        ///  Information about the new trusted domain object to be
        ///  created.
        /// </param>
        /// <param name="authenticationInformation">
        ///  Encrypted authentication information for the new trusted
        ///  domain object.
        /// </param>
        /// <param name="desiredAccess">
        ///  An access mask specifying desired access to the trusted
        ///  domain object handle.
        /// </param>
        /// <param name="trustedDomainHandle">
        ///  Used to return the handle for the newly created trusted
        ///  domain object.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarCreateTrustedDomainEx2(
            System.IntPtr policyHandle,
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX? trustedDomainInformation,
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL? authenticationInformation,
            ACCESS_MASK desiredAccess,
            out System.IntPtr? trustedDomainHandle)
        {
            return rpc.LsarCreateTrustedDomainEx2(
                policyHandle,
                trustedDomainInformation,
                authenticationInformation,
                desiredAccess,
                out trustedDomainHandle);
        }


        /// <summary>
        ///  The LsarQueryForestTrustInformation method is invoked
        ///  to retrieve information about a trust relationship
        ///  with another forest. Opnum: 73 
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainName">
        ///  The name of the trusted domain to query.
        /// </param>
        /// <param name="highestRecordType">
        ///  The highest ordinal number of foresttrust record type
        ///  that the caller understands.
        /// </param>
        /// <param name="forestTrustInfo">
        ///  Used to return the forest trust information.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarQueryForestTrustInformation(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? trustedDomainName,
            _LSA_FOREST_TRUST_RECORD_TYPE highestRecordType,
            out _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo)
        {
            return rpc.LsarQueryForestTrustInformation(
                policyHandle,
                trustedDomainName,
                highestRecordType,
                out forestTrustInfo);
        }


        /// <summary>
        ///  The LsarSetForestTrustInformation method is invoked
        ///  to establish a trust relationship with another forest
        ///  by attaching a set of records called the forest trust
        ///  information to the trusted domain object. Opnum: 74
        ///  
        /// </summary>
        /// <param name="policyHandle">
        ///  An RPC context handle obtained from either LsarOpenPolicy
        ///  or LsarOpenPolicy2.
        /// </param>
        /// <param name="trustedDomainName">
        ///  The name of the trusted domain object on which to set
        ///  the forest trust information.
        /// </param>
        /// <param name="highestRecordType">
        ///  The highest ordinal foresttrust record type that the
        ///  caller understands.
        /// </param>
        /// <param name="forestTrustInfo">
        ///  The forest trust information that the caller is trying
        ///  to set on the trusted domain object.
        /// </param>
        /// <param name="checkOnly">
        ///  If not 0, the operation is read-only and does not alter
        ///  the state of the server's database.
        /// </param>
        /// <param name="collisionInfo">
        ///  Used to return information about collisions between
        ///  different sets of forest trust information in the server's
        ///  database.
        /// </param>
        [CLSCompliant(false)]
        public NtStatus LsarSetForestTrustInformation(
            System.IntPtr policyHandle,
            _RPC_UNICODE_STRING? trustedDomainName,
            _LSA_FOREST_TRUST_RECORD_TYPE highestRecordType,
            _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo,
            byte checkOnly,
            out _LSA_FOREST_TRUST_COLLISION_INFORMATION? collisionInfo)
        {
            return rpc.LsarSetForestTrustInformation(
                policyHandle,
                trustedDomainName,
                highestRecordType,
                forestTrustInfo,
                checkOnly,
                out collisionInfo);
        }

        #endregion
        #endregion


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (rpc != null)
                {
                    try
                    {
                        rpc.Unbind();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        rpc.Dispose();
                        rpc = null;
                    }
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~LsaClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
