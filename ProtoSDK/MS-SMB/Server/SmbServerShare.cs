// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// share of smb server
    /// </summary>
    public class SmbServerShare
    {
        #region Fields

        /// <summary>
        /// The name of the share.
        /// </summary>
        private string name;

        /// <summary>
        /// A local server name to which a shared resource attaches.
        /// </summary>
        private string serverName;

        /// <summary>
        /// The optional support bits for the share; the format is specified in section 2.2.7.
        /// </summary>
        private int optionalSupport;

        /// <summary>
        /// The type of share (that is, disk, printer, named pipe).
        /// </summary>
        private string shareType;

        /// <summary>
        /// The security descriptor that describes which users can access resources on that share and their 
        /// corresponding access levels. Share.ShareSecurityDescriptor is of type SECURITY_DESCRIPTOR as specified in 
        /// [MS-DTYP] (section 2.4.6).
        /// </summary>
        private byte[] shareSecurityDescriptor;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the share.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }


        /// <summary>
        /// A local server name to which a shared resource attaches.
        /// </summary>
        public string ServerName
        {
            get
            {
                return this.serverName;
            }
            set
            {
                this.serverName = value;
            }
        }


        /// <summary>
        /// The optional support bits for the share; the format is specified in section 2.2.7.
        /// </summary>
        public int OptionalSupport
        {
            get
            {
                return this.optionalSupport;
            }
            set
            {
                this.optionalSupport = value;
            }
        }


        /// <summary>
        /// The type of share (that is, disk, printer, named pipe).
        /// </summary>
        public string ShareType
        {
            get
            {
                return this.shareType;
            }
            set
            {
                this.shareType = value;
            }
        }


        /// <summary>
        /// The security descriptor that describes which users can access resources on that share and their 
        /// corresponding access levels. Share.ShareSecurityDescriptor is of type SECURITY_DESCRIPTOR as specified in 
        /// [MS-DTYP] (section 2.4.6).
        /// </summary>
        public byte[] ShareSecurityDescriptor
        {
            get
            {
                return this.shareSecurityDescriptor;
            }
            set
            {
                this.shareSecurityDescriptor = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal SmbServerShare()
        {
        }


        #endregion
    }
}
