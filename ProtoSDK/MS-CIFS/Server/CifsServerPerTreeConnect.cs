// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the class of CifsServerPerTreeConnect which is used to contain the properties of CIFS ClientPerTreeConnect.
    /// </summary>
    public class CifsServerPerTreeConnect : IFileServiceServerTreeConnect
    {
        #region Fields
        private IFileServiceServerSession session;
        private Dictionary<ushort, IFileServiceServerOpen> openTable;
        private int treeConnectId;
        private int treeGlobalId;
        private DateTime creationTime;
        private Dictionary<ushort, CifsServerPerOpenSearch> openSearchTable;
        private string name;

        #endregion

        #region Properties

        /// <summary>
        /// The SMB connection associated with this tree connect.
        /// </summary>
        public CifsServerPerConnection Connection
        {
            get
            {
                return this.Session.Connection as CifsServerPerConnection;
            }
        }


        /// <summary>
        /// The SMB session associated with this tree connect.
        /// </summary>
        public IFileServiceServerSession Session
        {
            get
            {
                return this.session;
            }
        }


        /// <summary>
        /// TID of the tree connect
        /// </summary>
        public int TreeConnectId
        {
            get
            {
                return this.treeConnectId;
            }
        }


        /// <summary>
        /// The time that the tree connect was established.
        /// This is a 32-bit unsigned integer in little-endian byte order indicating the number of seconds since 
        /// Jan 1, 1970, 00:00:00.0.
        /// </summary>
        public DateTime CreationTime
        {
            get
            {
                return this.creationTime;
            }
        }


        /// <summary>
        /// A numeric value obtained by registration with the Server Service Remote Protocol.
        /// </summary>
        public int TreeGlobalId
        {
            get
            {
                return this.treeGlobalId;
            }
        }


        /// <summary>
        /// A numeric value that indicates the number of files that are currently opened on TreeConnect.
        /// </summary>
        public int OpenCount
        {
            get
            {
                return this.OpenTable.Count;
            }
        }


        /// <summary>
        /// All the open files in this tree connect.
        /// </summary>
        public ReadOnlyCollection<IFileServiceServerOpen> OpenTable
        {
            get
            {
                lock (this.openTable)
                {
                    return new ReadOnlyCollection<IFileServiceServerOpen>(
                        new List<IFileServiceServerOpen>(this.openTable.Values));
                }
            }
        }


        /// <summary>
        /// all the open searches in this tree connect.
        /// </summary>
        public ReadOnlyCollection<CifsServerPerOpenSearch> OpenSearchTable
        {
            get
            {
                lock (this.openSearchTable)
                {
                    return new ReadOnlyCollection<CifsServerPerOpenSearch>(
                        new List<CifsServerPerOpenSearch>(this.openSearchTable.Values));
                }
            }
        }


        /// <summary>
        /// A name for the shared resource on this server.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        #endregion

        #region Access OpenTable Methods

        /// <summary>
        /// Add an open file into this OpenTable.
        /// </summary>
        /// <param name="open">The open to add.</param>
        /// <exception cref="System.ArgumentNullException">the open is null</exception>
        /// <exception cref="System.ArgumentException">the open already exists</exception>
        public void AddOpen(IFileServiceServerOpen open)
        {
            lock (this.openTable)
            {
                this.openTable.Add((ushort)open.FileId, open);
            }
        }


        /// <summary>
        /// Remove an open file from this OpenTable.
        /// </summary>
        /// <param name="fileId">The open file id to remove.</param>
        public void RemoveOpen(ushort fileId)
        {
            lock (this.openTable)
            {
                this.openTable.Remove(fileId);
            }
        }


        /// <summary>
        /// Get the specified file from this OpenTable.
        /// </summary>
        /// <param name="fileId">The file id of the Open to remove.</param>
        public CifsServerPerOpenFile GetOpen(ushort fileId)
        {
            lock (this.openTable)
            {
                if (this.openTable.ContainsKey(fileId))
                {
                    return this.openTable[fileId] as CifsServerPerOpenFile;
                }
                return null;
            }
        }

        #endregion

        #region Access OpenSearchTable Methods

        /// <summary>
        /// Add an open search into this OpenSearchTable.
        /// </summary>
        /// <param name="openSearch">The openSearch to add.</param>
        /// <exception cref="System.ArgumentNullException">the openSearch is null</exception>
        /// <exception cref="System.ArgumentException">the openSearch already exists</exception>
        public void AddOpenSearch(CifsServerPerOpenSearch openSearch)
        {
            lock (this.openSearchTable)
            {
                this.openSearchTable.Add(openSearch.FindSID, openSearch);
            }
        }


        /// <summary>
        /// Get the specified search from this OpenTable.
        /// </summary>
        /// <param name="searchId">The open search id to remove.</param>
        public CifsServerPerOpenSearch GetOpenSearch(ushort searchId)
        {
            lock (this.openSearchTable)
            {
                if (this.openSearchTable.ContainsKey(searchId))
                {
                    return this.openSearchTable[searchId];
                }
                return null;
            }
        }


        /// <summary>
        /// Remove an open search from this OpenSearchTable.
        /// </summary>
        /// <param name="searchId">The open search id to remove.</param>
        public void RemoveOpenSearch(ushort searchId)
        {
            lock (this.openSearchTable)
            {
                this.openSearchTable.Remove(searchId);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// public Constructor
        /// </summary>
        public CifsServerPerTreeConnect(
            IFileServiceServerSession session,
            string name,
            int treeConnectId,
            int treeGlobalId,
            DateTime creationTime)
        {
            this.session = session;
            this.name = name;
            this.treeConnectId = treeConnectId;
            this.treeGlobalId = treeGlobalId;
            this.creationTime = creationTime;
            this.openTable = new Dictionary<ushort, IFileServiceServerOpen>();
            this.openSearchTable = new Dictionary<ushort, CifsServerPerOpenSearch>();
        }

        #endregion
    }
}