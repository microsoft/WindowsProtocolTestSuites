// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Cifs server global tables.
    /// </summary>
    internal class CifsServerGlobalTables
    {
        #region Fields

        private Dictionary<int, CifsServerPerTreeConnect> globalTreeConnectTable;
        private Dictionary<int, CifsServerPerSession> globalSessionTable;
        private Dictionary<object, CifsServerPerConnection> connectionTable;
        private Dictionary<int, CifsServerPerOpenFile> globalOpenFileTable;
        private Dictionary<int, CifsServerPerOpenSearch> globalOpenSearchTable;
        private int nextTreeConnectGlobalIndex;
        private int nextSessionGlobalIndex;
        private int nextOpenGlobalIndex;
        private int nextOpenSearchGlobalIndex;

        #endregion

        #region Properties

        /// <summary>
        /// GlobalSessionTable indexed by fileGlobalId.
        /// </summary>
        internal Dictionary<int, CifsServerPerOpenFile> GlobalOpenTable
        {
            get
            {
                return this.globalOpenFileTable;
            }
        }


        /// <summary>
        /// GlobalOpenSearchTable indexed by searchGlobalId.
        /// </summary>
        internal Dictionary<int, CifsServerPerOpenSearch> GlobalOpenSearchTable
        {
            get
            {
                return this.globalOpenSearchTable;
            }
        }


        /// <summary>
        /// GlobalSessionTable indexed by treeGlobalId.
        /// </summary>
        internal Dictionary<int, CifsServerPerTreeConnect> GlobalTreeConnectTable
        {
            get
            {
                return this.globalTreeConnectTable;
            }
        }


        /// <summary>
        /// GlobalSessionTable indexed by sessionGlobalId.
        /// </summary>
        internal Dictionary<int, CifsServerPerSession> GlobalSessionTable
        {
            get
            {
                return this.globalSessionTable;
            }
        }


        /// <summary>
        /// A list of all open connections on the server, indexed by the identify.
        /// </summary>
        internal Dictionary<object, CifsServerPerConnection> ConnectionTable
        {
            get
            {
                return this.connectionTable;
            }
        }

        /// <summary>
        /// next tree global id
        /// </summary>
        internal int NextTreeConnectGlobalIndex
        {
            get
            {
                return this.nextTreeConnectGlobalIndex;
            }
            set
            {
                this.nextTreeConnectGlobalIndex = value;
            }
        }


        /// <summary>
        /// next session global id
        /// </summary>
        internal int NextSessionGlobalIndex
        {
            get
            {
                return this.nextSessionGlobalIndex;
            }
            set
            {
                this.nextSessionGlobalIndex = value;
            }
        }


        /// <summary>
        /// next global open id
        /// </summary>
        internal int NextOpenGlobalIndex
        {
            get
            {
                return this.nextOpenGlobalIndex;
            }
            set
            {
                this.nextOpenGlobalIndex = value;
            }
        }


        /// <summary>
        /// next global open search id
        /// </summary>
        internal int NextOpenSearchGlobalIndex
        {
            get
            {
                return this.nextOpenSearchGlobalIndex;
            }
            set
            {
                this.nextOpenSearchGlobalIndex = value;
            }
        }


        #endregion

        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsServerGlobalTables()
        {
            this.connectionTable = new Dictionary<object,CifsServerPerConnection>();
            this.globalSessionTable = new Dictionary<int,CifsServerPerSession>();
            this.globalTreeConnectTable = new Dictionary<int,CifsServerPerTreeConnect>();
            this.globalOpenFileTable = new Dictionary<int,CifsServerPerOpenFile>();
            this.globalOpenSearchTable = new Dictionary<int,CifsServerPerOpenSearch>();
            this.nextSessionGlobalIndex = 1;
            this.nextTreeConnectGlobalIndex = 1;
            this.nextOpenGlobalIndex = 1;
            this.nextOpenSearchGlobalIndex =1;
        }

        #endregion
    }
}