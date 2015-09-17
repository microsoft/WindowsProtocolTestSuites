// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the connection of smb. inherit form cifs connection. 
    /// </summary>
    public class SmbClientConnection : CifsClientPerConnection
    {
        #region Fields

        /// <summary>
        /// gssApi 
        /// </summary>
        private ClientSecurityContext gssApi;

        /// <summary>
        /// This value MUST contain the authentication token being returned to the client, as specified in section  
        /// 3.2.4.2.3 and [RFC4178]. 
        /// </summary>
        private byte[] securityBlob;

        #endregion

        #region Extended of Properties

        /// <summary>
        /// The capabilities of the server, as specified in section 2.2.3.   The capabilities indirectly reflect the 
        /// negotiated dialect for this connection. 
        /// </summary>
        public new Capabilities ServerCapabilities
        {
            get
            {
                return (Capabilities)base.ServerCapabilities;
            }
            set
            {
                base.ServerCapabilities = (Cifs.Capabilities)value;
            }
        }


        /// <summary>
        /// A value that indicates the signing policy of the server. This value can be Disabled, Enabled, or 
        /// Required. 
        /// </summary>
        public new SignState ServerSigningState
        {
            get
            {
                switch (base.ServerSigningState)
                {
                    case SignStateValue.DISABLED:
                        return SignState.DISABLED;

                    case SignStateValue.ENABLED:
                        return SignState.ENABLED;

                    case SignStateValue.REQUIRED:
                        return SignState.REQUIRED;

                    default:
                        return SignState.NONE;
                }
            }
            set
            {
                base.ServerSigningState = (SignStateValue)value;
            }
        }


        /// <summary>
        /// A Boolean that determines whether the target server uses share passwords  instead of user accounts. 
        /// </summary>
        public bool UsesSharePasswords
        {
            get
            {
                return base.ShareLevelAccessControl;
            }
            set
            {
                base.ShareLevelAccessControl = value;
            }
        }


        /// <summary>
        /// A Boolean that determines whether the client should encrypt the password, while using implicit NTLM 
        /// authenticate. 
        /// </summary>
        private bool isClientEncryptPasswords;

        /// <summary>
        /// A Boolean that determines whether the client should encrypt the password, while using implicit NTLM 
        /// authenticate. 
        /// </summary>
        public bool IsClientEncryptPasswords
        {
            get
            {
                return this.isClientEncryptPasswords;
            }
            set
            {
                this.isClientEncryptPasswords = value;
            }
        }


        #endregion

        #region Properties from stack sdk design

        /// <summary>
        /// GssApi 
        /// </summary>
        public ClientSecurityContext GssApi
        {
            get
            {
                return this.gssApi;
            }
            set
            {
                this.gssApi = value;
            }
        }


        /// <summary>
        /// This value MUST contain the authentication token being returned to the client, as specified in section  
        /// 3.2.4.2.3 and [RFC4178]. 
        /// </summary>
        public byte[] SecurityBlob
        {
            get
            {
                return this.securityBlob;
            }
            set
            {
                this.securityBlob = value;
            }
        }


        /// <summary>
        /// A table of authenticated sessions that have been established on this transport connection. 
        /// The table MUST be uniquely indexed by Session.SessionId, and MUST support enumeration of 
        /// every entry in the table.
        /// </summary>
        public new ReadOnlyCollection<SmbClientSession> SessionTable
        {
            get
            {
                Collection<SmbClientSession> sessions = new Collection<SmbClientSession>();
                foreach (CifsClientPerSession session in base.SessionTable)
                {
                    sessions.Add(new SmbClientSession(session));
                }

                return new ReadOnlyCollection<SmbClientSession>(sessions);
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        internal SmbClientConnection()
            : base()
        {
            this.GssApi = null;
            this.SecurityBlob = null;
        }


        /// <summary>
        /// Deep copy constructor. if need to copy the connection instance, you must call the Clone method. its sub 
        /// class inherit from this, and need to provide more features. 
        /// </summary>
        protected SmbClientConnection(SmbClientConnection connection)
            : base(connection)
        {
            lock (connection)
            {
                this.GssApi = connection.GssApi;
                if (connection.SecurityBlob != null)
                {
                    this.SecurityBlob = new byte[connection.SecurityBlob.Length];
                    Array.Copy(connection.SecurityBlob, this.SecurityBlob, connection.SecurityBlob.Length);
                }
            }
        }


        /// <summary>
        /// clone this instance. using for the context to copy the instances. if need to inherit from this class, this 
        /// method must be overrided. 
        /// </summary>
        /// <returns>a copy of this instance </returns>
        protected override CifsClientPerConnection Clone()
        {
            return new SmbClientConnection(this);
        }


        #endregion

        #region Methods

        /// <summary>
        /// dispose the gssapi
        /// </summary>
        internal void DisposeGssApi()
        {
            if (this.GssApi == null)
            {
                return;
            }

            SspiClientSecurityContext sspiSecurityContext = this.GssApi as SspiClientSecurityContext;
            if (sspiSecurityContext != null)
            {
                sspiSecurityContext.Dispose();
            }
        }

        #endregion
    }
}
