// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The configuration of smb2 server
    /// </summary>
    public class Smb2ServerConfig
    {
        public bool IsSmb2002Implemented;

        public bool IsSmb21Implemented;

        public bool IsSmb3Implemented;

        public bool IsRequireMessageSigning;

        public bool IsDfsCapable;

        public bool IsLeasingSupported;

        public bool IsMultiChannelSupported;

        public bool IsDirectoryLeasingSupported;

        public bool IsPersistantHandlesSupported;

        public uint ServerSideCopyMaxNumberofChunks;

        public uint ServerSideCopyMaxChunkSize;

        public uint ServerSideCopyMaxDataSize;

        public uint MaxResiliencyTimeout;

        public uint MaxTransactSizeMultiCredit;

        public uint MaxTransactSize;

        public uint MaxReadSizeMultiCredit;

        public uint MaxReadSize;

        public uint MaxWriteSizeMultiCredit;

        public uint MaxWriteSize;
    }
}
