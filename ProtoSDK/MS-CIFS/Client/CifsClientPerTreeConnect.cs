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
    /// the class of CifsClientPerTreeConnect which is used to contain the properties of CIFS ClientPerTreeConnect.
    /// </summary>
    public class CifsClientPerTreeConnect : TreeConnect
    {
        #region fields

        private string shareName;

        #endregion


        #region properties

        /// <summary>
        /// The share name corresponding to this tree connection.
        /// </summary>
        public string ShareName
        {
            get
            {
                return this.shareName;
            }
            set
            {
                this.shareName = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsClientPerTreeConnect()
            : base()
        {
            this.ShareName = string.Empty;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public CifsClientPerTreeConnect(CifsClientPerTreeConnect smbClientPerTreeConnect)
            : base(smbClientPerTreeConnect)
        {
            lock (smbClientPerTreeConnect)
            {
                this.ShareName = smbClientPerTreeConnect.ShareName;
            }
        }


        /// <summary>
        /// clone this instance.
        /// using for the context to copy the instances.
        /// if need to inherit from this class, this method must be overridden.
        /// </summary>
        /// <returns>a copy of this instance</returns>
        protected internal virtual CifsClientPerTreeConnect Clone()
        {
            return new CifsClientPerTreeConnect(this);
        }


        #endregion
    }
}