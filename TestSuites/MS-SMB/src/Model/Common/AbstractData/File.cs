// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to store information of a file.
    /// </summary>
    public class SmbFile
    {
        /// <summary>
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// This is used to represent the name of the resource. 
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </summary>
        public string name;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// Access right.
        /// </summary>
        public int accessRights;

        /// <summary>
        /// It indicates whether the file is opened or not.
        /// </summary>
        public bool isOpened;

        /// <summary>
        /// To store search handler ID.
        /// </summary>
        public SetContainer<int> searchHandlerList = new SetContainer<int>();

        /// <summary>
        /// This container stores the previous version.
        /// </summary>
        public Set<int> previousVersionToken = new Set<int>();


        /// <summary>
        /// SMB File constructor.
        /// </summary>
        public SmbFile()
        {
            shareType = ShareType.Disk;
            name = String.Empty;
        }


        /// <summary>
        /// SMB file constructor.
        /// </summary>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="name">The name of the file.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="accessRights">Access right.</param>
        public SmbFile(ShareType shareType, string name, int treeId, int accessRights)
        {
            this.shareType = shareType;
            this.name = name;
            this.treeId = treeId;
            this.accessRights = accessRights;
        }

        /// <summary>
        /// SMB file constructor.
        /// </summary>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="name">This is used to represent the name of the resource.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="accessRights">Access right.</param>
        /// <param name="isOpened">It indicates whether the file is opened or not.</param>
        public SmbFile(ShareType shareType, string name, int treeId, int accessRights, bool isOpened)
        {
            this.shareType = shareType;
            this.name = name;
            this.treeId = treeId;
            this.accessRights = accessRights;
            this.isOpened = isOpened;
        }
    }
}
