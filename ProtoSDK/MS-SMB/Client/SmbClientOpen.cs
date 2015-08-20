// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the open of smb. inherit form smb open file.
    /// </summary>
    public class SmbClientOpen  : CifsClientPerOpenFile
    {
        #region Fields

        /// <summary>
        /// The TreeConnect on which this Open is valid.
        /// </summary>
        private SmbClientTreeConnect treeconnect;

        /// <summary>
        /// The session object on which this Open is valid.
        /// </summary>
        private SmbClientSession session;

        #endregion

        #region Properties

        /// <summary>
        /// The TreeConnect on which this Open is valid.
        /// </summary>
        public SmbClientTreeConnect Treeconnect
        {
            get
            {
                return this.treeconnect;
            }
            set
            {
                this.treeconnect = value;
            }
        }


        /// <summary>
        /// The session object on which this Open is valid.
        /// </summary>
        public SmbClientSession Session
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


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        internal SmbClientOpen()
            : base()
        {
        }


        /// <summary>
        /// Deep copy constructor. if need to copy the connection instance, you must call the Clone method. its sub 
        /// class inherit from this, and need to provide more features. 
        /// </summary>
        protected SmbClientOpen(SmbClientOpen open)
            : base(open)
        {
            this.treeconnect = open.treeconnect;
            this.session = open.session;
        }


        /// <summary>
        /// Constructor with base class.
        /// </summary>
        internal SmbClientOpen(CifsClientPerOpenFile open)
            : base(open)
        {
        }


        /// <summary>
        /// clone this instance. using for the context to copy the instances. if need to inherit from this class, this 
        /// method must be overrided. 
        /// </summary>
        /// <returns>a copy of this instance </returns>
        protected override CifsClientPerOpenFile Clone()
        {
            return new SmbClientOpen(this);
        }


        #endregion
    }
}
