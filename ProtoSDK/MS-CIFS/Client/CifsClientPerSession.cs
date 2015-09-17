// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the class of CifsClientPerSession which is used to contain the properties of CIFS ClientPerSession.
    /// </summary>
    public class CifsClientPerSession : Session
    {
        #region fields

        // in request:
        private ushort maxBufferSize;
        private ushort maxMpxCount;
        private ushort vcNumber;
        private uint sessionKeyOfNegotiated;
        private Capabilities capabilities;
        private CifsUserAccount userAccount;
        private string clientNativeOs;
        private string clientNativeLanMan;

        // in response:
        ActionValues action;
        private string serverNativeOs;
        private string serverNativeLanMan;
        private string serverPrimaryDomain;

        // others:
        private Collection<Open> openSearchTable;

        #endregion


        #region Properties in base class Session


        /// <summary>
        /// A table of opens of files or named pipes, as specified in section 3.3.1.10, that have been opened
        /// by this authenticated session. This table MUST be uniquely indexed by Open.FileId and MUST support
        /// enumeration of all entries in the table.
        /// </summary>
        internal Collection<Open> OpenFileTable
        {
            get
            {
                return this.openTable;
            }
            set
            {
                this.openTable = value;
            }
        }


        /// <summary>
        /// A table of opened searches.
        /// </summary>
        internal Collection<Open> OpenSearchTable
        {
            get
            {
                return this.openSearchTable;
            }
            set
            {
                this.openSearchTable = value;
            }
        }


        /// <summary>
        /// A table of tree connects that have been established by this authenticated session to shares on this
        /// server. This table MUST be uniquely indexed by TreeConnect.TreeId and MUST allow enumeration of all
        /// entries in the table.
        /// </summary>
        internal Collection<TreeConnect> TreeConnectTable
        {
            get
            {
                return this.treeConnectTable;
            }
            set
            {
                this.treeConnectTable = value;
            }
        }


        /// <summary>
        /// The security context of the user that established the session.
        /// </summary>
        public byte[] UserSecurityContext
        {
            get
            {
                return base.SecurityContext;
            }
            set
            {
                base.SecurityContext = value;
            }
        }


        /// <summary>
        /// get the opened file list of session
        /// </summary>
        public ReadOnlyCollection<Open> OpenFileList
        {
            get
            {
                return new ReadOnlyCollection<Open>(this.openTable);
            }
        }


        #endregion


        #region Properties in ADM

        /// <summary>
        /// A session can be in one of three states:
        /// InProgress �C A session setup is in progress for this session.
        /// Valid �C The session is valid and a session key and Uid are available for this session.
        /// Expired �C The Kerberos ticket for this session has expired and the session needs to be re-established.
        /// </summary>
        public SessionStateValue SessionState
        {
            get
            {
                return (SessionStateValue)base.State;
            }
            set
            {
                base.State = (uint)value;
            }
        }


        /// <summary>
        /// The 2-byte Uid for this session. The Uid is returned by the server in 
        /// the SMB header of session setup response. All subsequent SMB requests 
        /// for this user on this connection must use this Uid
        /// </summary>
        public ushort SessionUid
        {
            get
            {
                return (ushort)base.SessionId;
            }
            set
            {
                base.SessionId = (ulong)value;
            }
        }

        #endregion


        #region Properties in request

        /// <summary>
        /// The maximum size, in bytes, of the largest SMB message that the client can receive.
        /// </summary>
        public ushort MaxBufferSize
        {
            get
            {
                return this.maxBufferSize;
            }
            set
            {
                this.maxBufferSize = value;
            }
        }


        /// <summary>
        /// The maximum number of pending multiplexed requests supported by the client. This value MUST be
        /// less than or equal to the MaxMpxCount value provided by the server in the SMB_COM_NEGOTIATE response.
        /// </summary>
        public ushort MaxMpxCount
        {
            get
            {
                return this.maxMpxCount;
            }
            set
            {
                this.maxMpxCount = value;
            }
        }


        /// <summary>
        /// The number of this VC (virtual circuit) between the client and the server.
        /// </summary>
        public ushort VcNumber
        {
            get
            {
                return this.vcNumber;
            }
            set
            {
                this.vcNumber = value;
            }
        }


        /// <summary>
        /// The client MUST set this to be equal to the SessionKey field in the SMB_COM_NEGOTIATE response
        /// for this SMB connection.
        /// </summary>
        public uint SessionKeyOfNegotiated
        {
            get
            {
                return this.sessionKeyOfNegotiated;
            }
            set
            {
                this.sessionKeyOfNegotiated = value;
            }
        }


        /// <summary>
        /// A 32-bit field providing a set of client capability indicators. The client uses this field to report its
        /// own set of capabilities to the server. 
        /// </summary>
        public Capabilities Capabilities
        {
            get
            {
                return this.capabilities;
            }
            set
            {
                this.capabilities = value;
            }
        }


        /// <summary>
        /// the user information to setup session against the server.
        /// </summary>
        public CifsUserAccount UserAccount
        {
            get
            {
                return this.userAccount;
            }
            set
            {
                this.userAccount = value;
            }
        }


        /// <summary>
        /// A string representing the native operating system of the CIFS client.
        /// </summary>
        public string ClientNativeOs
        {
            get
            {
                return this.clientNativeOs;
            }
            set
            {
                this.clientNativeOs = value;
            }
        }


        /// <summary>
        /// A string that represents the native LAN manager type of the client.
        /// </summary>
        public string ClientNativeLanMan
        {
            get
            {
                return this.clientNativeLanMan;
            }
            set
            {
                this.clientNativeLanMan = value;
            }
        }

        #endregion


        #region Properties in response

        /// <summary>
        /// A 16-bit field which indicates the authentication actions. 
        /// </summary>
        public ActionValues Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }


        /// <summary>
        /// A string that represents the native operating system of the server
        /// </summary>
        public string ServerNativeOs
        {
            get
            {
                return this.serverNativeOs;
            }
            set
            {
                this.serverNativeOs = value;
            }
        }


        /// <summary>
        /// A string that represents the native LAN Manager type of the server.
        /// </summary>
        public string ServerNativeLanMan
        {
            get
            {
                return this.serverNativeLanMan;
            }
            set
            {
                this.serverNativeLanMan = value;
            }
        }


        /// <summary>
        /// A string representing the primary domain or workgroup name of the server.
        /// </summary>
        public string ServerPrimaryDomain
        {
            get
            {
                return this.serverPrimaryDomain;
            }
            set
            {
                this.serverPrimaryDomain = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsClientPerSession()
            : base()
        {
            // in request:
            this.MaxBufferSize = 0;
            this.MaxMpxCount = 0;
            this.VcNumber = 0;
            this.SessionKeyOfNegotiated = 0;
            this.Capabilities = Capabilities.NONE;
            this.UserAccount = new CifsUserAccount(string.Empty, string.Empty, string.Empty);
            this.ClientNativeOs = string.Empty;
            this.ClientNativeLanMan = string.Empty;

            // in response:
            this.Action = ActionValues.NONE;
            this.ServerNativeOs = string.Empty;
            this.ServerNativeLanMan = string.Empty;
            this.ServerPrimaryDomain = string.Empty;

            // others:
            this.OpenSearchTable = new Collection<Open>();
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public CifsClientPerSession(CifsClientPerSession session)
            : base(session)
        {
            lock (session)
            {
                // in request:
                this.MaxBufferSize = session.MaxBufferSize;
                this.MaxMpxCount = session.MaxMpxCount;
                this.VcNumber = session.VcNumber;
                this.SessionKeyOfNegotiated = session.SessionKeyOfNegotiated;
                this.Capabilities = session.Capabilities;
                this.UserAccount = new CifsUserAccount(session.UserAccount);
                this.ClientNativeOs = session.ClientNativeOs;
                this.ClientNativeLanMan = session.ClientNativeLanMan;

                // in response:
                this.Action = session.Action;
                this.ServerNativeOs = session.ServerNativeOs;
                this.ServerNativeLanMan = session.ServerNativeLanMan;
                this.ServerPrimaryDomain = session.ServerPrimaryDomain;

                // others:
                this.OpenSearchTable = new Collection<Open>();
                foreach (CifsClientPerOpenSearch open in session.openSearchTable)
                {
                    this.OpenSearchTable.Add(new CifsClientPerOpenSearch(open));
                }

                // base class Session:
                this.OpenFileTable = new Collection<Open>();
                foreach (CifsClientPerOpenFile open in session.openTable)
                {
                    this.OpenFileTable.Add(open.Clone());
                }
                this.TreeConnectTable = new Collection<TreeConnect>();
                foreach (CifsClientPerTreeConnect treeConnect in session.treeConnectTable)
                {
                    this.TreeConnectTable.Add(treeConnect.Clone());
                }
            }
        }


        /// <summary>
        /// clone this instance.
        /// using for the context to copy the instances.
        /// if need to inherit from this class, this method must be overridden.
        /// </summary>
        /// <returns>a copy of this instance</returns>
        protected internal virtual CifsClientPerSession Clone()
        {
            return new CifsClientPerSession(this);
        }

        #endregion
    }
}