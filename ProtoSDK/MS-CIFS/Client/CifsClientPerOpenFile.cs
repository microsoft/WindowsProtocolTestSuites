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
    /// the class of CifsClientPerOpenFile which is used to contain the properties of CIFS Per Open.
    /// </summary>
    public class CifsClientPerOpenFile : Open
    {
        #region Fields

        private ushort fileHandle;
        private string fileName; 

        #endregion


        #region Properties

        /// <summary>
        /// The file handle (FID) identifying this opened file, directory, or device
        /// as returned by the server in the response to the open or create request. 
        /// Several SMB commands (including SMB_COM_OPEN_ANDX and SMB_COM_NT_CREATE_ANDX)
        /// can be used to open a file, directory, or device.
        /// </summary>
        public ushort FileHandle
        {
            get
            {
                return this.fileHandle;
            }
            set
            {
                this.fileHandle = value;
            }
        }


        /// <summary>
        /// The file name of the opened file.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsClientPerOpenFile()
            : base()
        {
            this.FileHandle = 0;
            this.FileName = string.Empty;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public CifsClientPerOpenFile(CifsClientPerOpenFile smbClientPerOpen)
            : base(smbClientPerOpen)
        {
            lock (smbClientPerOpen)
            {
                this.fileHandle = smbClientPerOpen.fileHandle;
                this.fileName = smbClientPerOpen.fileName;
            }
        }


        /// <summary>
        /// clone this instance.
        /// using for the context to copy the instances.
        /// if need to inherit from this class, this method must be overridden.
        /// </summary>
        /// <returns>a copy of this instance</returns>
        protected internal virtual CifsClientPerOpenFile Clone()
        {
            return new CifsClientPerOpenFile(this);
        }


        #endregion
    }
}