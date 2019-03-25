// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerOpen
    {
        public FILEID FileId;

        public ulong FileGlobalId;

        public ulong DurableFileId;

        public Smb2ServerSession Session;

        public Smb2ServerTreeConnect TreeConnect;

        public Smb2ServerConnection Connection;

        // LocalOpen

        public AccessMask GrantedAccess;

        public OplockLevel_Values OplockLevel;

        public OplockState OplockState;

        // OplockTimeout

        public bool IsDurable;

        // DurableOpenTimeout

        public _SID DurableOwner;

        public ulong EnumerationLocation;

        public string EnumerationSearchPattern;

        public uint CurrentEaIndex;

        public uint CurrentQuotaIndex;

        public uint LockCount;

        public string PathName;

        // --------
        // SMB2.1
        // --------

        public Smb2ServerLease Lease;

        public bool IsResilient;

        // ResiliencyTimeout

        // ResilientOpenTimeout

        public List<byte> LockSequenceArray;

        // --------
        // SMB2.2
        // --------

        public Guid CreateGuid;

        public Guid AppInstanceId;

        public bool IsPersistent;
    }

    public enum OplockState
    {
        Held,
        Breaking,
        None
    }
}
