// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerShare
    {
        public string Name;

        public string ServerName;

        public string LocalPath;

        public string ConnectionSecurity;

        public string FileSecurity;

        public CscFlags CscFlags;

        public bool IsDfs;

        public bool DoAccessBasedDirectoryEnumeration;

        public bool AllowNamespaceCaching;

        public bool ForceSharedDelete;

        public bool RestrictExclusiveOpens;

        public uint MaxUsers;

        public uint CurrentUsers;

        public bool ForceLevel2Oplock;

        public bool HashEnabled;

        // --------
        // SMB2.2
        // --------
        public bool IsCA;
    }

    public enum CscFlags
    {
        ManualCaching,
        AutomaticCachingFiles,
        AutomaticCachingFilesPrograms,
        NoOfflineCaching
    }
}
