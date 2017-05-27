// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Modeling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerGlobal
    {
        public bool RequireMessageSigning;

        // ServerEnabled

        public MapContainer<string, Smb2ServerShare> ShareList;

        public MapContainer<ulong, Smb2ServerOpen> GlobalOpenTable;

        public MapContainer<ulong, Smb2ServerSession> GlobalSessionTable;

        public MapContainer<string, Smb2ServerConnection> ConnectionList;

        // ServerGuid

        // ServerStartTime

        public bool IsDfsCapable;

        public uint ServerSideCopyMaxNumberofChunks;

        public uint ServerSideCopyMaxChunkSize;

        public uint ServerSideCopyMaxDataSize;

        // -------
        // SMB 2.1
        // -------

        public ServerHashLevel ServerHashLevel;

        public MapContainer<Guid, Smb2ServerLeaseTable> GlobalLeaseTableList;

        public uint MaxResiliencyTimeout;

        // ResilientOpenScavengerExpiryTime
    }

    public enum ServerHashLevel
    {
        HashEnableAll,
        HashDisableAll,
        HashEnableShare
    }
}
