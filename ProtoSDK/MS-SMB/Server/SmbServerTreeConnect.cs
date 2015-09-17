// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the tree connect of smb server.
    /// </summary>
    public class SmbServerTreeConnect : IFileServiceServerTreeConnect
    {
        #region Fields of TD

        /// <summary>
        /// A numeric value that uniquely identifies a tree connect represented as a TID in the SMB header.
        /// </summary>
        private ushort treeId;

        /// <summary>
        /// A numeric value that indicates the number of files that are currently opened on TreeConnect.
        /// </summary>
        private int openCount;

        #endregion

        #region Fields of Sdk

        /// <summary>
        /// the open table
        /// </summary>
        private Collection<SmbServerOpen> openTable;

        /// <summary>
        /// The session on which the treeconnect was connected.
        /// </summary>
        private SmbServerSession session;

        /// <summary>
        /// the path of treeconnect
        /// </summary>
        private string path;

        #endregion

        #region Properties of TD

        /// <summary>
        /// A numeric value that uniquely identifies a tree connect represented as a TID in the SMB header.
        /// </summary>
        public ushort TreeId
        {
            get
            {
                return this.treeId;
            }
            set
            {
                this.treeId = value;
            }
        }


        /// <summary>
        /// A numeric value that indicates the number of files that are currently opened on TreeConnect.
        /// </summary>
        public int OpenCount
        {
            get
            {
                return this.openCount;
            }
            set
            {
                this.openCount = value;
            }
        }


        /// <summary>
        /// the path of treeconnect
        /// </summary>
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        #endregion

        #region Properties of Sdk

        /// <summary>
        /// The session on which the treeconnect was connected.
        /// </summary>
        public SmbServerSession Session
        {
            get
            {
                return this.session;
            }
            set
            {
                this.session = value;
            }
        }


        /// <summary>
        /// get the opens of treeconnect
        /// </summary>
        public ReadOnlyCollection<SmbServerOpen> OpenList
        {
            get
            {
                return new ReadOnlyCollection<SmbServerOpen>(this.openTable);
            }
        }


        /// <summary>
        /// get the opens of treeconnect
        /// </summary>
        public ReadOnlyCollection<IFileServiceServerOpen> OpenTable
        {
            get
            {
                Collection<IFileServiceServerOpen> ret = new Collection<IFileServiceServerOpen>();
                foreach (SmbServerOpen open in this.openTable)
                {
                    ret.Add(open);
                }
                return new ReadOnlyCollection<IFileServiceServerOpen>(ret);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal SmbServerTreeConnect()
        {
            this.openTable = new Collection<SmbServerOpen>();
        }


        #endregion

        #region Access Opens

        /// <summary>
        /// get the identitied open
        /// </summary>
        /// <param name="smbFid">the identigy of open</param>
        /// <returns>the identitied open</returns>
        internal SmbServerOpen GetOpen(ushort smbFid)
        {
            lock (this.openTable)
            {
                foreach (SmbServerOpen open in this.openTable)
                {
                    if (open.SmbFid == smbFid)
                    {
                        return open;
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// add open to treeconnect
        /// </summary>
        /// <param name="open">the open to add</param>
        /// <exception cref="InvalidOperationException">the open have exist in the treeconnect!</exception>
        internal void AddOpen(SmbServerOpen open)
        {
            lock (this.openTable)
            {
                if (this.openTable.Contains(open))
                {
                    throw new InvalidOperationException("the open have exist in the treeconnect!");
                }

                this.openTable.Add(open);
            }
        }


        /// <summary>
        /// remove the open.if does not exists, do nothing.
        /// </summary>
        /// <param name="open">the open to remove</param>
        internal void RemoveOpen(SmbServerOpen open)
        {
            lock (this.openTable)
            {
                if (this.openTable.Contains(open))
                {
                    this.openTable.Remove(open);
                }
            }
        }


        #endregion


        /// <summary>
        /// Inherit from FileServiceServerTreeConnect, equals to Session.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        IFileServiceServerSession IFileServiceServerTreeConnect.Session
        {
            get
            {
                return this.session;
            }
        }


        /// <summary>
        /// Inherit from FileServiceServerTreeConnect, equals to Path.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        string IFileServiceServerTreeConnect.Name
        {
            get 
            { 
                return this.path;
            }
        }


        /// <summary>
        /// Inherit from FileServiceServerTreeConnect, equals to TreeId.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        int IFileServiceServerTreeConnect.TreeConnectId
        {
            get 
            { 
                return this.treeId; 
            }
        }
    }
}
