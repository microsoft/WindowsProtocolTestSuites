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
    /// the class of CifsClientPerOpenSearch which is used to contain the properties of Per Open Search.
    /// </summary>
    public class CifsClientPerOpenSearch : Open
    {
        #region Fields

        private ushort searchID;
        private string searchName;

        #endregion


        #region Properties

        /// <summary>
        /// The search handle (SID) identifying a search opened using the TRANS2_FIND_FIRST2 subcommand.
        /// </summary>
        public ushort SearchID
        {
            get
            {
                return this.searchID;
            }
            set
            {
                this.searchID = value;
            }
        }


        /// <summary>
        /// The search name of the opened search.
        /// </summary>
        public string SearchName
        {
            get
            {
                return this.searchName;
            }
            set
            {
                this.searchName = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsClientPerOpenSearch()
            : base()
        {
            this.SearchID = 0;
            this.SearchName = string.Empty;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public CifsClientPerOpenSearch(CifsClientPerOpenSearch smbClientPerOpen)
            : base(smbClientPerOpen)
        {
            lock (smbClientPerOpen)
            {
                this.searchID = smbClientPerOpen.searchID;
                this.searchName = smbClientPerOpen.searchName;
            }
        }

        #endregion
    }
}