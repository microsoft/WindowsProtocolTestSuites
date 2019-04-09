// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerConnection
    {
        public List<ulong> CommandSequenceWindow;

        public Dictionary<ulong, Smb2ServerRequest> RequestList;

        public SESSION_SETUP_Request_Capabilities_Values ClientCapabilities;

        public ushort NegotiateDialect;

        public Dictionary<ulong, Smb2ServerRequest> AsyncCommandList;

        public string Dialect;

        public bool ShouldSign;

        public string ClientName;

        public uint MaxTransactSize;

        public bool SupportsMultiCredit;

        public Smb2TransportType TransportName;

        public Dictionary<ulong, Smb2ServerSession> SessionTable;

        // --------
        // SMB2.1
        // --------

        public Guid ClientGuid;
    }
}
