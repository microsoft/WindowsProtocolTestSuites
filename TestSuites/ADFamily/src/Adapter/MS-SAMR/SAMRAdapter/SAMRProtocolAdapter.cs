// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

using Entry = System.DirectoryServices;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMRProtocolAdapter : ADCommonServerAdapter, ISAMRProtocolAdapter
    {
        #region Adapter and Utitilies Instance

        /// <summary>
        /// This is an instance of Utilities, which contains helper functions that adapter requires.
        /// </summary>
        public Utilities utilityObject = null;

        /// <summary>
        /// This is an instance of SAMR RPC Adapter from SDK.
        /// </summary>
        private static ISamrRpcAdapter _samrRpcAdapter = null;

        /// <summary>
        /// This is an instance of the SAMR Protocol Adapter.
        /// </summary>
        private static SAMRProtocolAdapter _samrProtocolAdapter = null;

        /// <summary>
        /// Private constructor for SAMR Protocol Adapter
        /// </summary>
        private SAMRProtocolAdapter() { }

        /// <summary>
        /// Static instance for AD SAMR Adapter
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        new public static SAMRProtocolAdapter Instance(ITestSite site)
        {
            if (_samrProtocolAdapter == null)
            {
                _samrProtocolAdapter = new SAMRProtocolAdapter();
                _samrProtocolAdapter.Initialize(site);
            }
            return _samrProtocolAdapter;
        }


        public static SamrRpcAdapter RpcAdapter
        {
            get
            {
                return _samrRpcAdapter as SamrRpcAdapter;
            }
        }
        #endregion

        #region Variable Declarations

        /// <summary>
        /// Specify the protocol's name.
        /// </summary>
        private const string docShortName = "MS-SAMR";

        /// <summary>
        /// The configuration property group.
        /// </summary>
        private const string propertyGroup = "MS_SAMR.";

        /// <summary>
        /// Protocol uses Rpc over named pipe or Rpc over TCP/IP protocol sequence.
        /// </summary>
        private ProtocolSequence rpcConnectType;

        /// <summary>
        /// Time out value, in seconds, for rpce binding
        /// </summary>
        private int timeout;

        /// <summary>
        /// Specify if the server is a DC or not.
        /// </summary>
        public bool isDC;

        /// <summary>
        /// The FQDN of the primary domain.
        /// </summary>
        public string primaryDomainFqdn;

        /// <summary>
        /// The NetBIOS name of the primary domain.
        /// </summary>
        public string primaryDomainNetBIOSName;

        /// <summary>
        /// The Valid SID for the primary domain.
        /// </summary>
        public string primaryDomainSid;

        /// <summary>
        /// The DN of the primary domain naming context.
        /// </summary>
        public string primaryDomainDN;

        /// <summary>
        /// The DN of the user container in the primary domain.
        /// </summary>
        public string primaryDomainUserContainerDN;

        /// <summary>
        /// The DN of the computer container in the primary domain.
        /// </summary>
        public string primaryDomainComputerContainerDN;

        /// <summary>
        /// The DN of the domain controller container in the primary domain.
        /// </summary>
        public string primaryDomainControllerContainerDN;

        /// <summary>
        /// The FQDN of the PDC.
        /// </summary>
        public string pdcFqdn;

        /// <summary>
        /// The NetBIOS name of the PDC.
        /// </summary>
        public string pdcNetBIOSName;

        /// <summary>
        /// The DN of the computer object for PDC.
        /// </summary>
        public string pdcComputerObjectDN;

        /// <summary>
        /// The Fqdn of the Domain Member Server.
        /// </summary>
        public string domainMemberFqdn;

        /// <summary>
        /// The NetBIOS name of the Domain Member Server.
        /// </summary>
        public string domainMemberNetBIOSName;

        /// <summary>
        /// The name of the DM local administrator.
        /// </summary>
        public string DMAdminName;

        /// <summary>
        /// The password of DM local administrator.
        /// </summary>
        public string DMAdminPassword;

        /// <summary>
        /// The FQDN of the child domain.
        /// </summary>
        public string childDomainFqdn;

        /// <summary>
        /// The NetBIOS name of the child domain.
        /// </summary>
        public string childDomainNetBIOSName;

        /// <summary>
        /// The DN of the child domain naming context.
        /// </summary>
        public string childDomainDN;

        /// <summary>
        /// The Sid of Domain Administrator
        /// </summary>
        public SecurityIdentifier DomainAdministratorSid;

        /// <summary>
        /// Global method status.
        /// </summary>
        public int globalMethodStatus = (int)Utilities.STATUS_ERROR;

        #endregion

        #region Initialization

        /// <summary>
        /// This method is used to initialize the adapter for SAMR
        /// </summary>
        /// <param name="testSite">An instance of class ITestthis.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            this.Site.DefaultProtocolDocShortName = docShortName;
            _samrRpcAdapter = new SamrRpcAdapter();
            utilityObject = new Utilities();

            rpcConnectType = GetEnumProperty<ProtocolSequence>(propertyGroup + "Transport");
            timeout = GetIntProperty(propertyGroup + "TimeoutSeconds");
            DMAdminName = GetProperty(propertyGroup + "DMAdminName");
            DMAdminPassword = GetProperty(propertyGroup + "DMAdminPassword");

            if (timeout < 10)
            {
                timeout = 10;
            }

            primaryDomainFqdn = PrimaryDomainDnsName;
            primaryDomainNetBIOSName = PrimaryDomainNetBiosName;
            primaryDomainDN = "DC=" + PrimaryDomainDnsName.Replace(".", ",DC=");
            primaryDomainSid = PrimaryDomainSID;

            childDomainFqdn = ChildDomainDnsName;
            childDomainNetBIOSName = ChildDomainNetBiosName;
            childDomainDN = "DC=" + ChildDomainDnsName.Replace(".", ",DC=");

            pdcNetBIOSName = PDCNetbiosName;
            pdcFqdn = PDCNetbiosName + "." + PrimaryDomainDnsName;
            pdcComputerObjectDN = string.Format("CN=NTDS Settings,CN={0},CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,{1}", pdcNetBIOSName, primaryDomainDN); 

            domainMemberNetBIOSName = DMNetbiosName;
            domainMemberFqdn = DMNetbiosName + "." + PrimaryDomainDnsName;

            primaryDomainUserContainerDN = "CN=Users," + primaryDomainDN;
            primaryDomainComputerContainerDN = "CN=Computers," + primaryDomainDN;
            primaryDomainControllerContainerDN = "OU=Domain Controllers," + primaryDomainDN;
            
            DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/CN={1},{2}", pdcFqdn, DomainAdministratorName, primaryDomainUserContainerDN));
            DomainAdministratorSid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0);
        }

        #endregion

        #region Reset

        /// <summary>
        /// Reset the adapter.
        /// </summary>
        public override void Reset()
        {
            try
            {
                if (_samrRpcAdapter != null)
                {
                    if (_samrRpcAdapter.Handle != IntPtr.Zero)
                    {
                        _samrRpcAdapter.Unbind();
                    }
                    _samrRpcAdapter.Dispose();
                }
            }
            catch
            { }
            _samrRpcAdapter = new SamrRpcAdapter();
            base.Reset();

            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Release managed resources and unmanaged resources.
        /// </summary>
        /// <param name="disposing"> Indicate if managed resources will be released.
        /// If this parameter is "TRUE", both of managed resources and unmanaged resources will be released;
        /// Otherwise, only unmanaged resources will be released.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_samrRpcAdapter != null)
                {
                    _samrRpcAdapter.Dispose();
                    _samrRpcAdapter = null;
                }
                if (_samrProtocolAdapter != null)
                {
                    _samrProtocolAdapter = null;
                }
            }

            base.Dispose(disposing);

            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region Protocol Adapter Implementation

        /// <summary>
        /// Create SAMR Bind to the server.
        /// </summary>
        /// <param name="serverName">Variable of type string that represents the name of the server.</param>
        /// <param name="domainName">Variable of type string that represents the name of the domain.</param>
        /// <param name="userName">Variable of type string that represents the name of the logon user.</param>
        /// <param name="password">Variable of type string that represents the password of the logon user.</param>
        /// <param name="needSessionKey">Specify whether the SMB session key is required.</param>
        /// <param name="isDCConfig">Specify whether DC is configured or not.</param>
        public void SamrBind(
            string serverName,
            string domainName,
            string userName,
            string password,
            bool needSessionKey,
            bool isDCConfig)
        {
            isDC = isDCConfig;

            AccountCredential transportCredential =
                new AccountCredential(domainName, userName, password);
            ClientSecurityContextAttribute contextAttributes = ClientSecurityContextAttribute.Connection
                | ClientSecurityContextAttribute.DceStyle
                | ClientSecurityContextAttribute.Integrity
                | ClientSecurityContextAttribute.ReplayDetect
                | ClientSecurityContextAttribute.SequenceDetect
                | ClientSecurityContextAttribute.Confidentiality
                | ClientSecurityContextAttribute.UseSessionKey;

            if (isDC
                && (!needSessionKey)
                && (rpcConnectType.Equals(ProtocolSequence.RpcOverTcp)))
            {
                ushort[] endpoints = SamrUtility.QuerySamrTcpEndpoint(serverName);
                ushort tcpEndpoint = endpoints[0];
                _samrRpcAdapter.Bind(
                    RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                    serverName,
                    tcpEndpoint.ToString(),
                    transportCredential,
                    new SspiClientSecurityContext(SecurityPackageType.Negotiate,
                        transportCredential,
                        serverName,
                        contextAttributes,
                        SecurityTargetDataRepresentation.SecurityNetworkDrep),
                    RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY,
                    TimeSpan.FromSeconds(timeout));
            }
            else
            {
                _samrRpcAdapter.Bind(
                    RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                    serverName,
                    SamrUtility.SAMR_RPC_OVER_NP_WELLKNOWN_ENDPOINT,
                    transportCredential,
                    null,
                    RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE,
                    TimeSpan.FromSeconds(timeout));
            }
        }

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
        public HRESULT SamrConnect5(
            string serverName,
            uint desiredAccess,
            out IntPtr serverHandle,
            uint inVersion = 1u,
            uint revision = 3u,
            uint supportedFeatures = 0u)
        {
            // local Variables.
            int methodStatus;
            uint outVersion;
            SAMPR_REVISION_INFO outRevisionInfo;

            // Set the revison information.
            SAMPR_REVISION_INFO[] inRevisionInfo = new SAMPR_REVISION_INFO[1];
            inRevisionInfo[0] = new SAMPR_REVISION_INFO();
            inRevisionInfo[0].V1.Revision = (_SAMPR_REVISION_INFO_V1_Revision_Values)revision;
            inRevisionInfo[0].V1.SupportedFeatures = (SupportedFeatures_Values)supportedFeatures;

            methodStatus = _samrRpcAdapter.SamrConnect5(
                serverName,
                desiredAccess,
                inVersion,
                inRevisionInfo[0],
                out outVersion,
                out outRevisionInfo,
                out serverHandle);

            return (HRESULT)methodStatus;
        }

        /// <summary>
        /// SamrLookupDomainInSamServer method obtains the SID of a domain object, given its name.
        /// </summary>
        /// <param name="serverHandle">Variable of type pointer HANDLE that Lockout the server handle.</param>
        /// <param name="domainNameToLookUp">Variable of string domainNameToLookUp which indicate that the domain name to look up.</param>
        /// <param name="domainSid">Out parameter of type _RPS_SID domainSid that Specify the SID of domain.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrLookupDomainInSamServer(
            IntPtr serverHandle,
            string domainNameToLookUp,
            out _RPC_SID domainSid)
        {
            // local Variables.
            int methodStatus;
            _RPC_SID? domainLookSamId;

            _RPC_UNICODE_STRING samServerName = new _RPC_UNICODE_STRING();
            samServerName.Length = (ushort)((domainNameToLookUp.Length) * 2);
            samServerName.MaximumLength = samServerName.Length;
            samServerName.Buffer = new ushort[samServerName.MaximumLength / 2];
            utilityObject.GetUnicodeString(domainNameToLookUp, ref samServerName.Buffer);

            methodStatus = _samrRpcAdapter.SamrLookupDomainInSamServer(
                (IntPtr)serverHandle,
                samServerName,
                out domainLookSamId);

            domainSid = ConvertSidToInstance(domainLookSamId);

            return (HRESULT)methodStatus;
        }


        /// <summary>
        /// SamrOpenDomain method obtains a handle to a domain object, given a SID
        /// </summary>
        /// <param name="serverHandle">Variable of type pointer HANDLE that Lockout the server handle.</param>
        /// <param name="desiredAccess">Variable of type uint DomainHandleAccess which specify the type of access on the domain handle that is to be opened.</param>
        /// <param name="domainSid">Variable of type _RPC_SID DomainSid which specify the domain that is to be opened.</param>
        /// <param name="domainHandle">Out parameter of type pointer HANDLE representing the opened domain handle.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrOpenDomain(
            IntPtr serverHandle,
            uint desiredAccess,
            _RPC_SID domainSid,
            out IntPtr domainHandle)
        {
            //local Variables.
            int methodStatus;

            methodStatus = _samrRpcAdapter.SamrOpenDomain(
                (IntPtr)serverHandle,
                desiredAccess,
                domainSid,
                out domainHandle);

            if (!isDC && (Utilities.STATUS_SUCCESS != (uint)methodStatus))
            {
                return HRESULT.STATUS_ACCESS_DENIED;
            }

            return (HRESULT)methodStatus;
        }

        /// <summary>
        /// SamrLookupNamesInDomain translates a set of account names into a set of RIDs.
        /// </summary>
        /// <param name="domainHandle">Variable of type pointer HANDLE that Lockout the domain handle.</param>
        /// <param name="accountNames">Variable of type List that Lockout the set of account names that are to be translated into set of RIDs.</param>
        /// <param name="accountRids">Out parameter of type List RID that Specify the RID of the accounts</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrLookupNamesInDomain(
            IntPtr domainHandle,
            List<string> accountNames,
            out List<uint> accountRids)
        {
            //local Variables.
            int methodStatus;
            _RPC_UNICODE_STRING[] ArrLook = new _RPC_UNICODE_STRING[] { };
            _SAMPR_ULONG_ARRAY namesLookRIDS = new _SAMPR_ULONG_ARRAY();
            _SAMPR_ULONG_ARRAY namesLookUSE = new _SAMPR_ULONG_ARRAY();

            string[] namesLookup = new string[accountNames.Count];
            accountNames.CopyTo(namesLookup, 0);

            string[] lookNamesArray = new string[namesLookup.Length];
            ArrLook = new _RPC_UNICODE_STRING[lookNamesArray.Length];

            for (int loopcount = 0; loopcount < namesLookup.Length; loopcount++)
            {
                ArrLook[loopcount].Length = (ushort)(namesLookup[loopcount].Length * 2);
                ArrLook[loopcount].MaximumLength = (ushort)(ArrLook[loopcount].Length);
                ArrLook[loopcount].Buffer = new ushort[ArrLook[loopcount].MaximumLength / 2];
                utilityObject.GetUnicodeString(namesLookup[loopcount], ref ArrLook[loopcount].Buffer);
            }

            methodStatus = _samrRpcAdapter.SamrLookupNamesInDomain(
                (IntPtr)domainHandle,
                (uint)(namesLookup.Length),
                ArrLook,
                out namesLookRIDS,
                out namesLookUSE);

            accountRids = new List<uint>();
            for (int loopcount = 0; loopcount < namesLookRIDS.Count; loopcount++)
            {
                accountRids.Add(namesLookRIDS.Element[loopcount]);
            }

            return (HRESULT)methodStatus;
        }

        #region Group

        /// <summary>
        /// SamrOpenGroup method obtains a handle to a group, given a RID.
        /// </summary>
        /// <param name="domainHandle">Variable of type pointer HANDLE that Lockout the domain handle.</param>
        /// <param name="desiredAccess">Variable of type uint GroupHandleAccess which Lockout the type of access on the group handle that is being opened.</param>
        /// <param name="groupRid">Variable of type uint that specify the rid of the group that is to be opened.</param>
        /// <param name="groupHandle">Out parameter of type pointer HANDLE representing the group handle that is opened.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrOpenGroup(
            IntPtr domainHandle,
            uint desiredAccess,
            uint groupRid,
            out IntPtr groupHandle)
        {
            return (HRESULT)_samrRpcAdapter.SamrOpenGroup(domainHandle, desiredAccess, groupRid, out groupHandle);
        }

        /// <summary>
        /// SamrOpenGroup method obtains a handle to a group, given a RID.
        /// </summary>
        /// <param name="domainHandle">Variable of type pointer HANDLE that Lockout the domain handle.</param>
        /// <param name="desiredAccess">Variable of type uint GroupHandleAccess which Lockout the type of access on the group handle that is being opened.</param>
        /// <param name="groupRid">Variable of type uint that specify the rid of the group that is to be opened.</param>
        /// <param name="groupHandle">Out parameter of type pointer HANDLE representing the group handle that is opened.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrOpenAlias(
            IntPtr domainHandle,
            uint desiredAccess,
            uint aliasRid,
            out IntPtr aliasHandle)
        {
            return (HRESULT)_samrRpcAdapter.SamrOpenAlias(domainHandle, desiredAccess, aliasRid, out aliasHandle);
        }

        /// <summary>
        /// The SamrCreateGroupInDomain method creates a group.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="groupName">The value to use as the name of the group.</param>
        /// <param name="desiredAccess">The access requested on the GroupHandle on output.</param>
        /// <param name="userHandle">An RPC context handle representing the created group object.</param>
        /// <param name="relativeId">The RID of the newly created user.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrCreateGroupInDomain(
                IntPtr domainHandle,
                string groupName,
                uint desiredAccess,
                out IntPtr groupHandle,
                out uint relativeId)
        {
            _RPC_UNICODE_STRING rpcGroupName = StringToRpcUnicodeString(groupName);
            return (HRESULT)_samrRpcAdapter.SamrCreateGroupInDomain(
                (IntPtr)domainHandle,
                rpcGroupName,
                desiredAccess,
                out groupHandle,
                out relativeId);
        }

        /// <summary>
        /// The SamrCreateGroupInDomain method creates an alias.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="aliasName">The value to use as the name of the alias.</param>
        /// <param name="desiredAccess">The access requested on the GroupHandle on output.</param>
        /// <param name="aliasHandle">An RPC context handle representing the created alias object.</param>
        /// <param name="relativeId">The RID of the newly created alias.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrCreateAliasInDomain(
                IntPtr domainHandle,
                string aliasName,
                uint desiredAccess,
                out IntPtr aliasHandle,
                out uint relativeId)
        {
            _RPC_UNICODE_STRING rpcAliasName = StringToRpcUnicodeString(aliasName);
            return (HRESULT)_samrRpcAdapter.SamrCreateAliasInDomain(
                (IntPtr)domainHandle,
                rpcAliasName,
                desiredAccess,
                out aliasHandle,
                out relativeId);
        }

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
        public HRESULT SamrEnumerateGroupsInDomain(IntPtr domainHandle,
            ref uint? enumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? buffer,
            uint preferedMaximumLength,
            out uint countReturned
            )
        {
            return (HRESULT)_samrRpcAdapter.SamrEnumerateGroupsInDomain(domainHandle,
                ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
        }

        /// <summary>
        /// The SamrQueryInformationGroup method obtains attributes from a group object.
        /// </summary>
        /// <param name="groupHandle">An RPC context handle representing a group object.</param>
        /// <param name="groupInfoClass">An enumeration indicating whose attributes to obtain.</param>
        /// <param name="groupInfo">The requested attributes and values to be obtained.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrQueryInformationGroup(IntPtr groupHandle, _GROUP_INFORMATION_CLASS groupInfoClass, out _SAMPR_GROUP_INFO_BUFFER? groupInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryInformationGroup(groupHandle, groupInfoClass, out groupInfo);
        }

        /// <summary>
        /// The SamrQueryInformationAlias method obtains attributes from a group object.
        /// </summary>
        /// <param name="groupHandle">An RPC context handle representing a group object.</param>
        /// <param name="groupInfoClass">An enumeration indicating whose attributes to obtain.</param>
        /// <param name="groupInfo">The requested attributes and values to be obtained.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrQueryInformationAlias(IntPtr aliasHandle, _ALIAS_INFORMATION_CLASS aliasInfoClass, out _SAMPR_ALIAS_INFO_BUFFER? aliasInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryInformationAlias(aliasHandle, aliasInfoClass, out aliasInfo);
        }

        /// <summary>
        /// The SamrSetInformationGroup method updates attributes on a group object.
        /// </summary>
        /// <param name="groupHandle">An RPC context handle representing a group object.</param>
        /// <param name="groupInfoClass">An enumeration indicating which attributes to update.</param>
        /// <param name="groupInfo">The requested attributes and values to update.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrSetInformationGroup(IntPtr groupHandle, _GROUP_INFORMATION_CLASS groupInfoClass, _SAMPR_GROUP_INFO_BUFFER groupInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrSetInformationGroup(groupHandle, groupInfoClass, groupInfo);
        }

        /// <summary>
        /// The SamrSetInformationAlias method updates attributes on a group object.
        /// </summary>
        /// <param name="groupHandle">An RPC context handle representing a group object.</param>
        /// <param name="groupInfoClass">An enumeration indicating which attributes to update.</param>
        /// <param name="groupInfo">The requested attributes and values to update.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrSetInformationAlias(IntPtr aliasHandle, _ALIAS_INFORMATION_CLASS aliasInfoClass, _SAMPR_ALIAS_INFO_BUFFER aliasInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrSetInformationAlias(aliasHandle, aliasInfoClass, aliasInfo);
        }

        /// <summary>
        /// The SamrDeleteGroup method removes a group object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a group object to be deleted.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrDeleteGroup(ref IntPtr groupHandle)
        {
            return (HRESULT)_samrRpcAdapter.SamrDeleteGroup(ref groupHandle);
        }

        /// <summary>
        /// The SamrDeleteGroup method removes a group object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a group object to be deleted.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrDeleteAlias(ref IntPtr aliasHandle)
        {
            return (HRESULT)_samrRpcAdapter.SamrDeleteAlias(ref aliasHandle);
        }

        #endregion

        #region User

        /// <summary>
        /// SamrOpenUser method obtains a handle to a user, given a RID.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="desiredAccess">An ACCESS_MASK that indicates the requested access for the returned handle.</param>
        /// <param name="userRid">A RID of a user account.</param>
        /// <param name="userHandle">An RPC context handle representing the user handle that is opened.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrOpenUser(
            IntPtr domainHandle,
            uint desiredAccess,
            uint userRid,
            out IntPtr userHandle)
        {
            return (HRESULT)_samrRpcAdapter.SamrOpenUser(domainHandle, desiredAccess, userRid, out userHandle);
        }

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
        public HRESULT SamrCreateUser2InDomain(
            IntPtr domainHandle,
            string userName,
            uint accountType,
            uint desiredAccess,
            out IntPtr userHandle,
            out uint grantedAccess,
            out uint relativeId)
        {
            _RPC_UNICODE_STRING rpcUserName = StringToRpcUnicodeString(userName);
            return (HRESULT)_samrRpcAdapter.SamrCreateUser2InDomain(
                (IntPtr)domainHandle,
                rpcUserName,
                accountType,
                desiredAccess,
                out userHandle,
                out grantedAccess,
                out relativeId);
        }

        /// <summary>
        /// The SamrCreateUserInDomain method creates a user.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="userName">The value to use as the name of the user.</param>
        /// <param name="desiredAccess">The access requested on the UserHandle on output.</param>
        /// <param name="userHandle">An RPC context handle representing the created user object.</param>
        /// <param name="relativeId">The RID of the newly created user.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrCreateUserInDomain(
                IntPtr domainHandle,
                string userName,
                uint desiredAccess,
                out IntPtr userHandle,
                out uint relativeId)
        {
            _RPC_UNICODE_STRING rpcUserName = StringToRpcUnicodeString(userName);
            return (HRESULT)_samrRpcAdapter.SamrCreateUserInDomain(
                (IntPtr)domainHandle,
                rpcUserName,
                desiredAccess,
                out userHandle,
                out relativeId);
        }

        /// <summary>
        /// The SamrQueryInformationUser2 method queries attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="userInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrQueryInformationUser2(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, out _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryInformationUser2(userHandle, userInfoClass, out userInfo);
        }

        /// <summary>
        /// The SamrSetInformationUser method queries attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="userInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrQueryInformationUser(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, out _SAMPR_USER_INFO_BUFFER? userInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryInformationUser(userHandle, userInfoClass, out userInfo);
        }

        /// <summary>
        /// The SamrGetGroupsForUser method obtains a listing of groups that a user is a member of.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="groups">An array of RIDs of the groups that the user referenced by UserHandle is a member of.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrGetGroupsForUser(IntPtr userHandle, out _SAMPR_GET_GROUPS_BUFFER? groups)
        {
            return (HRESULT)_samrRpcAdapter.SamrGetGroupsForUser(userHandle, out groups);
        }

        /// <summary>
        /// The SamrRidToSid method obtains the SID of an account, given a RID.
        /// </summary>
        /// <param name="objectHandle">An RPC context handle.</param>
        /// <param name="rid">A RID of an account.</param>
        /// <param name="sid">The SID of the account referenced by Rid.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrRidToSid(IntPtr objectHandle, uint rid, out _RPC_SID? sid)
        {
            return (HRESULT)_samrRpcAdapter.SamrRidToSid(objectHandle, rid, out sid);
        }

        /// <summary>
        /// The SamrSetInformationUser2 method updates attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to update.</param>
        /// <param name="userInfo">The requested attributes and values to update.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrSetInformationUser2(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, _SAMPR_USER_INFO_BUFFER userInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrSetInformationUser2(userHandle, userInfoClass, userInfo);
        }

        /// <summary>
        /// The SamrSetInformationUser method updates attributes on a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object.</param>
        /// <param name="userInfoClass">An enumeration indicating which attributes to update.</param>
        /// <param name="userInfo">The requested attributes and values to update.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrSetInformationUser(IntPtr userHandle, _USER_INFORMATION_CLASS userInfoClass, _SAMPR_USER_INFO_BUFFER userInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrSetInformationUser(userHandle, userInfoClass, userInfo);
        }


        /// <summary>
        /// The SamrDeleteUser method removes a user object.
        /// </summary>
        /// <param name="userHandle">An RPC context handle representing a user object to be deleted.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrDeleteUser(ref IntPtr userHandle)
        {
            return (HRESULT)_samrRpcAdapter.SamrDeleteUser(ref userHandle);
        }

        /// <summary>
        /// The SamrGetDisplayEnumerationIndex2 method obtains an index into an ascending account-name–sorted list of accounts, 
        /// such that the index is the position in the list of the accounts whose account name best matches a client-provided string.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="displayClass">An enumeration indicating which set of objects to return an index into.</param>
        /// <param name="prefix">A string matched against the account name to find a starting point for an enumeration.</param>
        /// <param name="index">A value to use as input to SamrQueryDisplayInformation3 in order to control the accounts that are returned from that method.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrGetDisplayEnumerationIndex2(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            string prefix,
            out uint index)
        {
            _RPC_UNICODE_STRING unicodePrefix = StringToRpcUnicodeString(prefix);
            return (HRESULT)_samrRpcAdapter.SamrGetDisplayEnumerationIndex2(domainHandle, displayClass, unicodePrefix, out index);
        }

        /// <summary>
        /// The SamrGetDisplayEnumerationIndex method obtains an index into an ascending account-name–sorted list of accounts, 
        /// such that the index is the position in the list of the accounts whose account name best matches a client-provided string.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="displayClass">An enumeration indicating which set of objects to return an index into.</param>
        /// <param name="prefix">A string matched against the account name to find a starting point for an enumeration.</param>
        /// <param name="index">A value to use as input to SamrQueryDisplayInformation3 in order to control the accounts that are returned from that method.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrGetDisplayEnumerationIndex(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            string prefix,
            out uint index)
        {
            _RPC_UNICODE_STRING unicodePrefix = StringToRpcUnicodeString(prefix);
            return (HRESULT)_samrRpcAdapter.SamrGetDisplayEnumerationIndex(domainHandle, displayClass, unicodePrefix, out index);
        }

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
        public HRESULT SamrQueryDisplayInformation3(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            uint index,
            uint entryCount,
            uint preferedMaximumLength,
            out uint totalAvailable,
            out uint totalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER buffer)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryDisplayInformation3(
                domainHandle, displayClass, index, entryCount, preferedMaximumLength,
                out totalAvailable, out totalReturned, out buffer);
        }

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
        public HRESULT SamrQueryDisplayInformation2(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            uint index,
            uint entryCount,
            uint preferedMaximumLength,
            out uint totalAvailable,
            out uint totalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER buffer)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryDisplayInformation2(
                domainHandle, displayClass, index, entryCount, preferedMaximumLength,
                out totalAvailable, out totalReturned, out buffer);
        }

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
        public HRESULT SamrQueryDisplayInformation(
            IntPtr domainHandle,
            _DOMAIN_DISPLAY_INFORMATION displayClass,
            uint index,
            uint entryCount,
            uint preferedMaximumLength,
            out uint totalAvailable,
            out uint totalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER buffer)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryDisplayInformation(
                domainHandle, displayClass, index, entryCount, preferedMaximumLength,
                out totalAvailable, out totalReturned, out buffer);
        }

        /// <summary>
        /// The SamrQuerySecurityObject method queries the access control on a server, domain, user, group, or alias object.
        /// </summary>
        /// <param name="objectHandle">An RPC context handle representing a server, domain, user, group, or alias object.</param>
        /// <param name="securityInformation">A bit field that specifies which fields of SecurityDescriptor the client is requesting to be returned.</param>
        /// <param name="securityDescriptor">A security descriptor expressing accesses that are specific to the ObjectHandle and the owner and group of the object.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrQuerySecurityObject(IntPtr objectHandle,
                          SecurityInformation securityInformation,
                                              out _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor)
        {
            return (HRESULT)_samrRpcAdapter.SamrQuerySecurityObject(objectHandle,
                (SamrQuerySecurityObject_SecurityInformation_Values)securityInformation,
                out securityDescriptor);
        }

        /// <summary>
        /// The SamrSetSecurityObject method sets the access control on a server, domain, user, group, or alias object.
        /// </summary>
        /// <param name="objectHandle">An RPC context handle representing a server, domain, user, group, or alias object.</param>
        /// <param name="securityInformation">A bit field that specifies which fields of SecurityDescriptor the client is requesting to be set.</param>
        /// <param name="securityDescriptor">A security descriptor expressing accesses that are specific to the ObjectHandle and the owner and group of the object.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrSetSecurityObject(IntPtr objectHandle,
                          SecurityInformation securityInformation,
                                              _SAMPR_SR_SECURITY_DESCRIPTOR? securityDescriptor)
        {
            return (HRESULT)_samrRpcAdapter.SamrSetSecurityObject(objectHandle,
                (SecurityInformation_Values)securityInformation,
                securityDescriptor);
        }

        /// <summary>
        /// The SamrEnumerateUsersInDomain method enumerates all users.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="enumerationContext">To initiate a new enumeration the client sets EnumerationContext to zero. 
        /// Otherwise the client sets EnumerationContext to a value returned by a previous call to the method. </param>
        /// <param name="userAccountControl">A filter value to be used on the userAccountControl attribute.</param>
        /// <param name="buffer">A list of user information.</param>
        /// <param name="preferedMaximumLength">The requested maximum number of bytes to return in Buffer.</param>
        /// <param name="countReturned">The count of domain elements returned in Buffer.</param>
        /// <returns></returns>
        public HRESULT SamrEnumerateUsersInDomain(IntPtr domainHandle,
            ref uint? enumerationContext,
            uint userAccountControl,
            out _SAMPR_ENUMERATION_BUFFER? buffer,
            uint preferedMaximumLength,
            out uint countReturned
            )
        {
            return (HRESULT)_samrRpcAdapter.SamrEnumerateUsersInDomain(domainHandle,
                ref enumerationContext, userAccountControl, out buffer, preferedMaximumLength, out countReturned);
        }

        /// <summary>
        /// SamrCloseHandle method closes any handle like server,domain,user, group or alias.
        /// </summary>
        /// <param name="samHandle">Variable of type pointer SAMHANDLE that represent either server,domain,group, alias or user.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrCloseHandle(ref IntPtr samHandle)
        {
            return (HRESULT)_samrRpcAdapter.SamrCloseHandle(ref samHandle);
        }

        #endregion

        #region Domain

        /// <summary>
        /// The SamrQueryInformationDomain2 method queries attributes on a domain object.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="domainInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="domainInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrQueryInformationDomain2(IntPtr domainHandle, _DOMAIN_INFORMATION_CLASS domainInfoClass, out _SAMPR_DOMAIN_INFO_BUFFER? domainInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryInformationDomain2(domainHandle, domainInfoClass, out domainInfo);
        }

        /// <summary>
        /// The SamrQueryInformationDomain method queries attributes on a domain object.
        /// </summary>
        /// <param name="domainHandle">An RPC context handle representing a domain object.</param>
        /// <param name="domainInfoClass">An enumeration indicating which attributes to query.</param>
        /// <param name="domainInfo">The requested attributes and values to return.</param>
        /// <returns>HRESULT.</returns>
        public HRESULT SamrQueryInformationDomain(IntPtr domainHandle, _DOMAIN_INFORMATION_CLASS domainInfoClass, out _SAMPR_DOMAIN_INFO_BUFFER? domainInfo)
        {
            return (HRESULT)_samrRpcAdapter.SamrQueryInformationDomain(domainHandle, domainInfoClass, out domainInfo);
        }

        #endregion

        #region private help methods

        /// <summary>
        /// Convert nullable types _RPC_SID to an instance value.
        /// </summary>
        /// <param name="domainLookSamId">Nullable value of _RPC_SID.</param>
        /// <returns>The equivalent value of the nullable vale of _RPC_SID.</returns>
        private _RPC_SID ConvertSidToInstance(_RPC_SID? domainLookSamId)
        {
            _RPC_SID newid = new _RPC_SID();
            if (domainLookSamId == null)
            {
                domainLookSamId = new _RPC_SID();
                newid = (_RPC_SID)domainLookSamId;
            }
            else
            {
                newid = (_RPC_SID)domainLookSamId;
            }
            return newid;
        }

        /// <summary>
        /// Convert string values to _RPC_UNICODE_STRING values.
        /// </summary>
        /// <param name="userName">The value of string param.</param>
        /// <returns>The equivalent values to _RPC_UNICODE_STRING types.</returns>
        public _RPC_UNICODE_STRING StringToRpcUnicodeString(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName", "The input parameter is NULL.");
            }
            _RPC_UNICODE_STRING _rpc_unicode_string = new _RPC_UNICODE_STRING();
            _rpc_unicode_string.Length = _rpc_unicode_string.MaximumLength = (ushort)(userName.Length * 2);
            _rpc_unicode_string.Buffer = new ushort[userName.Length];
            for (int i = 0; i < userName.Length; i++)
            {
                _rpc_unicode_string.Buffer[i] = Convert.ToUInt16(userName[i]);
            }
            return _rpc_unicode_string;
        }

        #endregion

        #endregion

        public HRESULT SamrGetUserDomainPasswordInformation(IntPtr userHandle, out _USER_DOMAIN_PASSWORD_INFORMATION passwordInformation)
        {
            //local Variables.
            int methodStatus;

            methodStatus = _samrRpcAdapter.SamrGetUserDomainPasswordInformation(
                userHandle,
                out passwordInformation);

            return (HRESULT)methodStatus;
        }
    }
}
