// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using System.DirectoryServices.Protocols;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Machine object class
    /// </summary>
    public abstract class DsMachine
    {
        /// <summary>
        /// Domain of the machine.
        /// </summary>
        public DsDomain Domain { get; set; }

        /// <summary>
        ///  NetBIOS name of the machine.
        /// </summary>
        public string NetbiosName { get; set; }

        /// <summary>
        ///  DNS host name of the machine.
        /// </summary>
        public string DnsHostName { get; set; }

        /// <summary>
        ///  DN of the computerobject that corresponds to the machine.
        /// </summary>
        public string ComputerObjectName { get; set; }

        /// <summary>
        ///  objectGUID of the computerobject that corresponds to
        ///  the machine.
        /// </summary>
        public Guid ComputerObjectGuid { get; set; }

        /// <summary>
        /// LDAP connection to the machine.
        /// </summary>
        public LdapConnection LdapConn { get; set; }
    }


    /// <summary>
    /// Domain Controller (DC) Class
    /// </summary>
    public class DsServer : DsMachine
    {
        /// <summary>
        /// where this server is located
        /// </summary>
        public DsSite Site{ get; set; }

        /// <summary>
        ///  DN of the serverobject that corresponds to the DC.
        /// </summary>
        public string ServerObjectName { get; set; }

        /// <summary>
        ///  DN of the nTDSDSAobject that corresponds to the DC.
        /// </summary>
        public string NtdsDsaObjectName { get; set; }

        /// <summary>
        /// Dsa NetworkAddress of server
        /// </summary>
        public string DsaNetworkAddress { get; set; }

        /// <summary>
        ///  objectGUID of the serverobject that corresponds to the
        ///  DC.
        /// </summary>
        public Guid ServerObjectGuid { get; set; }

        /// <summary>
        ///  objectGUID of the nTDSDSAobject that corresponds to
        ///  the DC.
        /// </summary>
        public Guid NtdsDsaObjectGuid { get; set; }

        /// <summary>
        /// Invocation ID of the DC.
        /// </summary>
        public Guid InvocationId { get; set; }

        /// <summary>
        /// The FSMO roles of this server.
        /// </summary>
        public FSMORoles FsmoRoles = FSMORoles.None;

        /// <summary>
        /// true if it's windows
        /// </summary>
        public bool IsWindows;

        /// <summary>
        /// DC function in rootDSE
        /// </summary>
        public int DCFunctional { get; set; }
    }

    /// <summary>
    /// The AD DS server
    /// </summary>
    public class AddsServer : DsServer
    {
        /// <summary>
        /// Indicates if the server is a Global Catalog server.
        /// </summary>
        public bool IsGcServer { get; set; }

        /// <summary>
        /// Indicates if the server is a readonly domain controller.
        /// </summary>
        public bool IsRODC { get; set; }

    }

    /// <summary>
    /// The AD LDS server
    /// </summary>
    public class AdldsServer : DsServer
    {
        /// <summary>
        /// Port number of the LDS instance.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// LDS instance GUID
        /// </summary>
        public Guid InstanceID { get; set; }
    }

    /// <summary>
    /// non-DC client machine
    /// </summary>
    public class ClientMachine : DsMachine
    {

    }

}
