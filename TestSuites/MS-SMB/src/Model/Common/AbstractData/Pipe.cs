// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to store information of a pipe..
    /// </summary>
    public class SmbPipe 
    {
        /// <summary>
        /// Pipe name. 
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </summary>
        public string name;

        /// <summary>
        /// Share type.
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// Tree ID.
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the named pipe is opened or not.
        /// </summary>
        public bool isOpened;

        /// <summary>
        /// The access right for this pipe.
        /// </summary>
        public int accessRights;

        /// <summary>
        /// It indicates whether the client reads mode for the named pipe.
        /// </summary>
        public bool isRequireBlockingPipeState;

        /// <summary>
        /// It indicates whether named pipe reads requests block.
        /// </summary>
        public bool isRequireReadModePipeState;

        /// <summary>
        /// It indicates Bytes of data in the name pipe.
        /// </summary>
        public int avaiableDataBytes;

        /// <summary>
        /// It indicates the read mode of a name pipe when it is created.
        /// </summary>
        public bool createTimeReadMode;


        /// <summary>
        /// SMB pipe constructor.
        /// </summary>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="name">
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="accessRights">Access right.</param>
        /// <param name="isOpened">It indicates whether the named pipe is opened or not.</param>
        /// Disable warning CA1805 because model need to initialize 'avaiableDataBytes' for test case design.
        [SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        public SmbPipe(ShareType shareType, string name, int treeId, int accessRights, bool isOpened)
        {
            this.shareType = shareType;
            this.name = name;
            this.treeId = treeId;
            this.accessRights = accessRights;
            this.isOpened = isOpened;
            isRequireBlockingPipeState = true;
            isRequireReadModePipeState = Parameter.isMessageModePipe;
            createTimeReadMode = Parameter.isMessageModePipe;
            avaiableDataBytes = 0;
        }
    }


    /// <summary>
    /// SMB mailslot
    /// </summary>
    public class SmbMailslot
    {
        /// <summary>
        /// Pipe name. 
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </summary>
        public string pipeName;

        /// <summary>
        /// Tree ID.
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the mailslot is opened or not.
        /// </summary>
        public bool isOpened;

        /// <summary>
        /// Access rights.
        /// </summary>
        public int accessRights;

        /// <summary>
        /// Share type.
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// Avaiable data bytes.
        /// </summary>
        public int avaiableDataBytes;

        /// <summary>
        /// SMB mailslot.
        /// </summary>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="pipeName">
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// Disable warning CA1805 because model need to initialize 'avaiableDataBytes' for test case design.
        [SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        public SmbMailslot(ShareType shareType, string pipeName, int treeId)
        {
            this.shareType = shareType;
            this.pipeName = pipeName;
            this.treeId = treeId;
            this.avaiableDataBytes = 0;
        }


        /// <summary>
        /// SMB mailslot.
        /// </summary>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="pipeName">
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="accessRights">Access right.</param>
        /// <param name="isOpened">It indicates whether the mailslot is opened or not.</param>
        /// Disable warning CA1805 because model need to initialize 'avaiableDataBytes' for test case design.
        [SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        public SmbMailslot(ShareType shareType, string pipeName, int treeId, int accessRights, bool isOpened)
        {
            this.shareType = shareType;
            this.pipeName = pipeName;
            this.treeId = treeId;
            this.accessRights = accessRights;
            this.isOpened = isOpened;
            this.avaiableDataBytes = 0;
        }
    }
}
