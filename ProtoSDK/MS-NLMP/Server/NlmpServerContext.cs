// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// The server context of nlmp. This class is used to hold the state variables of TD or Sdk.
    /// </summary>
    public class NlmpServerContext : NlmpContext
    {
        #region Fields From Sdk

        /// <summary>
        /// the password of user. sdk can not retrieve the password from any user account database. sdk just accept a
        /// user password as the public password for all user.
        /// </summary>
        private NlmpClientCredential clientCredential;

        /// <summary>
        /// whether the nlmp server joined a domain. if true, the nlmp server is a domain server.
        /// </summary>
        private bool isDomainJoined;

        #endregion

        #region Fields From TD

        /// <summary>
        /// The set of server configuration flags (section 2.2.2.5) that specify the full set of capabilities of the
        /// server.
        /// </summary>
        private NegotiateTypes cfgFlg;

        /// <summary>
        /// A string that indicates the fully qualified domain name (FQDN (2)) of the server's domain.
        /// </summary>
        private string dnsDomainName;

        /// <summary>
        /// A string that indicates the FQDN (2) of the server's forest.
        /// </summary>
        private string dnsForestName;

        /// <summary>
        /// A string that indicates the FQDN (1) of the server.
        /// </summary>
        private string dnsMachineName;

        /// <summary>
        /// A string that indicates the NetBIOS name of the user's domain.
        /// </summary>
        private string nbDomainName;

        /// <summary>
        /// A string that indicates the NetBIOS machine name of the server.
        /// </summary>
        private string nbMachineName;

        /// <summary>
        /// A Boolean setting that disables the server from generating challenges and responding to NTLM_NEGOTIATE
        /// messages.
        /// </summary>
        private bool serverBlock;

        /// <summary>
        /// A Boolean setting that requires the server to use 128-bit encryption.
        /// </summary>
        private bool serverRequire128bitEncryption;

        /// <summary>
        /// The gss_channel_bindings_struct ([RFC2744] section 3.11). This value is supplied by the application and
        /// used by the protocol. This value is optional.
        /// </summary>
        private object serverChannelBindingsUnhashed;

        /// <summary>
        /// A Boolean setting from the application requiring channel binding.
        /// </summary>
        private bool applicationRequiresCBT;

        #endregion

        #region Properties From Sdk

        /// <summary>
        /// the password of user. sdk can not retrieve the password from any user account database. sdk just accept a
        /// user password as the public password for all user.
        /// </summary>
        public NlmpClientCredential ClientCredential
        {
            get
            {
                return this.clientCredential;
            }
            set
            {
                this.clientCredential = value;
            }
        }


        /// <summary>
        /// whether the nlmp server joined a domain. if true, the nlmp server is a domain server.
        /// </summary>
        public bool IsDomainJoined
        {
            get
            {
                return this.isDomainJoined;
            }
            set
            {
                this.isDomainJoined = value;
            }
        }


        #endregion

        #region Properties From TD, Internal to Protocol

        /// <summary>
        /// The set of server configuration flags (section 2.2.2.5) that specify the full set of capabilities of the
        /// server.
        /// </summary>
        public NegotiateTypes CfgFlg
        {
            get
            {
                return this.cfgFlg;
            }
            set
            {
                this.cfgFlg = value;
            }
        }


        /// <summary>
        /// A string that indicates the fully qualified domain name (FQDN (2)) of the server's domain.
        /// </summary>
        public string DnsDomainName
        {
            get
            {
                return this.dnsDomainName;
            }
            set
            {
                this.dnsDomainName = value;
            }
        }


        /// <summary>
        /// A string that indicates the FQDN (2) of the server's forest.
        /// </summary>
        public string DnsForestName
        {
            get
            {
                return this.dnsForestName;
            }
            set
            {
                this.dnsForestName = value;
            }
        }


        /// <summary>
        /// A string that indicates the FQDN (1) of the server.
        /// </summary>
        public string DnsMachineName
        {
            get
            {
                return this.dnsMachineName;
            }
            set
            {
                this.dnsMachineName = value;
            }
        }


        /// <summary>
        /// A string that indicates the NetBIOS name of the user's domain.
        /// </summary>
        public string NbDomainName
        {
            get
            {
                return this.nbDomainName;
            }
            set
            {
                this.nbDomainName = value;
            }
        }


        /// <summary>
        /// A string that indicates the NetBIOS machine name of the server.
        /// </summary>
        public string NbMachineName
        {
            get
            {
                return this.nbMachineName;
            }
            set
            {
                this.nbMachineName = value;
            }
        }


        /// <summary>
        /// A Boolean setting that disables the server from generating challenges and responding to NTLM_NEGOTIATE
        /// messages.
        /// </summary>
        public bool ServerBlock
        {
            get
            {
                return this.serverBlock;
            }
            set
            {
                this.serverBlock = value;
            }
        }


        /// <summary>
        /// A Boolean setting that requires the server to use 128-bit encryption.
        /// </summary>
        public bool ServerRequire128bitEncryption
        {
            get
            {
                return this.serverRequire128bitEncryption;
            }
            set
            {
                this.serverRequire128bitEncryption = value;
            }
        }


        #endregion

        #region Properties From TD, Exposed to Application

        /// <summary>
        /// The Datagram option indicates that the connectionless mode of NTLM is to be used. If the Datagram option is
        /// selected by the server, connectionless mode is used, and NTLM performs a bitwise OR operation with the
        /// following NTLM Negotiate bit flags into the CfgFlg internal variable:NTLMSSP_NEGOTIATE_DATAGRAM.
        /// </summary>
        public override bool Datagram
        {
            set
            {
                if (value)
                {
                    this.CfgFlg |= NegotiateTypes.NTLMSSP_NEGOTIATE_DATAGRAM;
                }
            }
        }


        /// <summary>
        /// The gss_channel_bindings_struct ([RFC2744] section 3.11). This value is supplied by the application and
        /// used by the protocol. This value is optional.
        /// </summary>
        public object ServerChannelBindingsUnhashed
        {
            get
            {
                return this.serverChannelBindingsUnhashed;
            }
            set
            {
                this.serverChannelBindingsUnhashed = value;
            }
        }


        /// <summary>
        /// A Boolean setting from the application requiring channel binding.
        /// </summary>
        public bool ApplicationRequiresCBT
        {
            get
            {
                return this.applicationRequiresCBT;
            }
            set
            {
                this.applicationRequiresCBT = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal NlmpServerContext()
            : base()
        {
        }


        #endregion
    }
}
