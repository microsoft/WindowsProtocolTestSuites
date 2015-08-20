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
    /// FileAccessCollection provides a container for all SMB2 protocol related
    /// context in an SMB2 server or client.
    /// </summary>
    public class CifsClientCollection : FileAccessCollection
    {
        #region fields

        private Collection<Open> globalOpenFileTable;
        private Collection<Open> globalOpenSearchTable;

        #endregion


        #region properties

        /// <summary>
        /// A table containing all the files opened by remote clients on the server.
        /// </summary>
        public Collection<Open> GlobalOpenFileTable
        {
            get 
            { 
                return this.globalOpenFileTable; 
            }
            set 
            { 
                this.globalOpenFileTable = value; 
            }
        }


        /// <summary>
        /// A table containing all the searches opened by remote clients on the server.
        /// </summary>
        public Collection<Open> GlobalOpenSearchTable
        {
            get 
            { 
                return this.globalOpenSearchTable; 
            }
            set 
            { 
                this.globalOpenSearchTable = value; 
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsClientCollection()
        {
            this.GlobalOpenFileTable = new Collection<Open>();
            this.GlobalOpenSearchTable = new Collection<Open>();
        }

        #endregion
    }
}