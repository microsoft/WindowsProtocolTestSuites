// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;

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

        public MapContainer<ulong, Smb2ServerOpen> OpenTable;

        public MapContainer<uint, Smb2ServerTreeConnect> TreeConnectTable;

        // ExpirationTime

        public Smb2ServerConnection Connection;

        public uint SessionGlobalId;

        // CreationTime

        // IdleTime

        // UserName

        // --------
        // SMB2.2
        // --------
        public SetContainer<Smb2ServerChannel> ChannelList;
    }

    public enum SessionState
    {
        InProgress,
        Valid,
        Expired
    }
}
