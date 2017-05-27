// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// The plain text layout of _LSAPR_TRUSTED_DOMAIN_AUTH_BLOB.AuthBlob
    /// </summary>
    internal partial struct LsaTrustedDomainAuthBlob
    {
        /// <summary>
        /// leads with 512 bytes of random data
        /// </summary>
        [Size("512")]
        public byte[] randomData;

        /// <summary>
        /// Specifies the count of entries present in the CurrentOutgoingAuthInfos field.
        /// </summary>
        [Size("4")]
        public uint CountOutgoingAuthInfos;

        /// <summary>
        /// Specifies the byte offset from the beginning of CountOutgoingAuthInfos to the start of the                  
        /// CurrentOutgoingAuthInfos field.
        /// </summary>
        [Size("4")]
        public uint ByteOffsetCurrentOutgoingAuthInfo;

        /// <summary>
        /// Specifies the byte offset from the beginning of CountOutgoingAuthInfos to the start of the
        /// PreviousOutgoingAuthInfos field. If the difference between ByteOffsetPreviousOutgoingAuthInfo
        /// and OutgoingAuthInfoSize is 0, this field MUST be ignored; this also means that the
        /// PreviousOutgoingAuthInfos field has zero entries.
        /// </summary>
        [Size("4")]
        public uint ByteOffsetPreviousOutgoingAuthInfo;

        /// <summary>
        /// Contains an array of CountOutgoingAuthInfos of LSAPR_AUTH_INFORMATION (section 2.2.7.17) entries
        /// in self-relative format. Each LSAPR_AUTH_INFORMATION entry in the array MUST be 4-byte aligned.
        /// When it is necessary to insert unused padding bytes into a buffer for data alignment, such bytes
        /// MUST be set to 0.
        /// </summary>
        [Size("ByteOffsetPreviousOutgoingAuthInfo - ByteOffsetCurrentOutgoingAuthInfo")]
        public byte[] CurrentOutgoingAuthInfos;

        /// <summary>
        /// Contains an array of CountOutgoingAuthInfos LSAPR_AUTH_INFORMATION entries in self-relative format.
        /// See the comments for the ByteOffsetPreviousOutgoingAuthInfo field to determine when this field is
        /// present. Each LSAPR_AUTH_INFORMATION entry in the array MUST be 4-byte aligned. When it is necessary
        /// to insert unused padding bytes into a buffer for data alignment, such bytes MUST be set to 0.
        /// </summary>
        [Size("OutgoingAuthInfoSize - ByteOffsetPreviousOutgoingAuthInfo")]
        public byte[] PreviousOutgoingAuthInfos;

        /// <summary>
        /// Specifies the count of entries present in the CountIncomingAuthInfos field.
        /// </summary>
        [Size("4")]
        public uint CountIncomingAuthInfos;

        /// <summary>
        /// Specifies the byte offset from the beginning of CountIncomingAuthInfos to the start of the
        /// CurrentIncomingAuthInfos field.
        /// </summary>
        [Size("4")]
        public uint ByteOffsetCurrentIncomingAuthInfo;

        /// <summary>
        /// Specifies the byte offset from the beginning of CountIncomingAuthInfos to the start of the
        /// PreviousIncomingAuthInfos field. If the difference between ByteOffsetPreviousIncomingAuthInfo
        /// and IncomingAuthInfoSize is 0, this field MUST be ignored; this also means that the
        /// PreviousIncomingAuthInfos field has zero entries.
        /// </summary>
        [Size("4")]
        public uint ByteOffsetPreviousIncomingAuthInfo;

        /// <summary>
        /// Contains an array of CountIncomingAuthInfos LSAPR_AUTH_INFORMATION entries in self-relative format.
        /// Each LSAPR_AUTH_INFORMATION entry in the array MUST be 4-byte aligned. When it is necessary to insert
        /// unused padding bytes into a buffer for data alignment, such bytes MUST be set to 0.
        /// </summary>
        [Size("ByteOffsetPreviousIncomingAuthInfo - ByteOffsetCurrentIncomingAuthInfo")]
        public byte[] CurrentIncomingAuthInfos;

        /// <summary>
        /// Contains an array of CountIncomingAuthInfos LSAPR_AUTH_INFORMATION entries in self-relative format.
        /// See the comments for the ByteOffsetPreviousIncomingAuthInfo field to determine when this field is
        /// present. Each LSAPR_AUTH_INFORMATION entry in the array MUST be 4-byte aligned. When it is necessary
        /// to insert unused padding bytes into a buffer for data alignment, such bytes MUST be set to 0.
        /// </summary>
        [Size("IncomingAuthInfoSize - ByteOffsetPreviousIncomingAuthInfo")]
        public byte[] PreviousIncomingAuthInfos;

        /// <summary>
        /// Specifies the size, in bytes, of the sub-portion of the structure from the beginning of the
        /// CountOutgoingAuthInfos field through the end of the of the PreviousOutgoingAuthInfos field.
        /// </summary>
        [Size("4")]
        public uint OutgoingAuthInfoSize;

        /// <summary>
        /// Specifies the size, in bytes, of the sub-portion of the structure from the beginning of the 
        /// CountIncomingAuthInfos field through the end of the of the PreviousIncomingAuthInfos field.
        /// </summary>
        [Size("4")]
        public uint IncomingAuthInfoSize;
    }
}
