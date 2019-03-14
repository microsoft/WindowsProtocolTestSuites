// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerSession
    {
        public ulong SessionId;

        public SessionState State;

        public bool IsAnonymous;

        public bool IsGuest;

        // SecurityContext

        // SessionKey

        public bool ShouldSign;

        public Dictionary<ulong, Smb2ServerOpen> OpenTable;

        public Dictionary<uint, Smb2ServerTreeConnect> TreeConnectTable;

        // ExpirationTime

        public Smb2ServerConnection Connection;

        public uint SessionGlobalId;

        // CreationTime

        // IdleTime

        // UserName

        // --------
        // SMB2.2
        // --------
        public HashSet<Smb2ServerChannel> ChannelList;
    }

    public enum SessionState
    {
        InProgress,
        Valid,
        Expired
    }
}
