// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc
{
    /// <summary>
    /// A DFS client sends a DFS referral request using the REQ_GET_DFS_REFERRAL message.
    /// The format of this message is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct REQ_GET_DFS_REFERRAL
    {
        /// <summary>
        /// A 16-bit integer that indicates the highest DFS referral version understood by the client.
        /// </summary>
        public ushort MaxReferralLevel;

        /// <summary>
        /// A byte array representing a null-terminated Unicode string specifying the path to be resolved. 
        /// The specified path MUST NOT be case-sensitive.
        /// Its format depends on the type of referral request: Domain referral:
        /// The path MUST be an empty string (containing 
        /// just the null terminator). 
        /// Non empty strings MUST begin with one "\"
        /// client MUST use DFS referral
        /// version 3 or later for a domain referral request.
        /// </summary>
        public byte[] RequestFileName;
    }

    /// <summary>
    /// A DFS server responds to a DFS client referral request with the
    /// RESP_GET_DFS_REFERRAL message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RESP_GET_DFS_REFERRAL
    {
        /// <summary>
        /// A 16-bit integer indicating the number of bytes in the
        /// prefix of the referral request path that is matched
        /// in the referral response.
        /// </summary>
        public ushort PathConsumed;

        /// <summary>
        /// A 16-bit integer indicating the number of referral entries
        /// immediately following the referral header
        /// </summary>
        public ushort NumberOfReferrals;

        /// <summary> 
        /// Only the R, S, and T bits are defined and used. 
        /// The other bits MUST be set to 0 by the server and
        /// ignored upon receipt by the client
        /// </summary>
        public ReferralHeaderFlags ReferralHeaderFlags;

        /// <summary>
        /// As many DFS_REFERRAL_V1, DFS_REFERRAL_V2, DFS_REFERRAL_V3 or DFS_REFERRAL_V4 structures as
        /// indicated by the NumberOfReferrals field.
        /// </summary>
        public Object ReferralEntries;
    }


    /// <summary>
    /// A 32-bit field representing a series of flags that are combined
    ///  by using the bitwise OR operation. Only the R, S, and T bits are defined and used.
    /// </summary>
    [Flags()]
    public enum ReferralHeaderFlags : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The R bit MUST be set to 1 if all of the targets in the referral 
        /// entries returned are DFS root targets capable of handling DFS 
        /// referral requests and set to 0 otherwise.
        /// </summary>
        R = 0x00000001,

        /// <summary>
        /// The S bit MUST be set to 1 if all of the targets in the referral
        /// response can be accessed without requiring further referral
        /// requests and set to 0 otherwise.
        /// </summary>
        S = 0x00000002,

        /// <summary>
        /// The T bit MUST be set to 1 if DFS client target failback is
        /// enabled for all targets in this referral response. 
        /// This value MUST be set to 0 by the server and ignored by
        /// the client for all DFS referral versions except DFS referral version 4.
        /// </summary>
        T = 0x00000004,
    }


    /// <summary>
    /// The format of the version 1 referral entry is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DFS_REFERRAL_V1
    {
        /// <summary>
        /// A 16-bit integer indicating the version number of the
        /// referral entry. MUST always be 0x0001 for DFS_REFERRAL_V1.
        /// </summary>
        public ushort VersionNumber;

        /// <summary>
        /// A 16-bit integer indicating the total size of the referral entry in bytes.
        /// </summary>
        public ushort Size;

        /// <summary>
        /// A 16-bit integer indicating the type of server hosting the target.
        /// This field MUST be set to 0x0001 if DFS root targets are returned,
        /// and to 0x0000 otherwise. Note that sysvol targets are not DFS root 
        /// targets; the field MUST be set to 0x0000 for a sysvol referral response.
        /// </summary>
        public ushort ServerType;

        /// <summary>
        /// A series of bit flags. MUST be set to 0x0000 and ignored on receipt.
        /// </summary>
        public ushort ReferralEntryFlags;

        /// <summary>
        /// Unicode character string that specifies a DFS target.
        /// The string does not contain null-terminator.
        /// </summary>
        public string ShareName;

        /// <summary>
        /// Returns a string of the values of the fields of a v1 referral
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Version Number: {0}, ServerType: {1}, ShareName: {2}", VersionNumber, ServerType, ShareName);
        }
    }


    /// <summary>
    /// The format of the version 2 referral entry is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DFS_REFERRAL_V2
    {
        /// <summary>
        /// A 16-bit integer indicating the version number of the referral entry.
        /// MUST always be 0x0002 for DFS_REFERRAL_V2
        /// </summary>
        public ushort VersionNumber;

        /// <summary>
        /// A 16-bit integer indicating the total size of the referral entry in bytes
        /// </summary>
        public ushort Size;

        /// <summary>
        /// A 16-bit integer indicating the type of server hosting the target. 
        /// This field MUST be set to 0x0001 if DFS root targets are returned, 
        /// and to 0x0000 otherwise. Note that sysvol targets are not DFS root 
        /// targets; the field MUST be set to 0x0000 for a sysvol referral 
        /// response.
        /// </summary>
        public ushort ServerType;

        /// <summary>
        /// MUST be set to 0x0000 by the server and ignored on receipt by the client.
        /// </summary>
        public ushort ReferralEntryFlags;

        /// <summary>
        /// MUST be set to 0x00000000 by the server and ignored by the client
        /// </summary>
        public uint Proximity;

        /// <summary>
        /// A 32-bit integer indicating the time-out value, in seconds, of the 
        /// DFS root or DFS link. MUST be set to the time-out value of the
        /// DFS root or the DFS link in the DFS metadata for which 
        /// the referral response is being sent.
        /// When there is more than one referral entry, the TimeToLive
        /// of each referral entry MUST be the same
        /// </summary>
        public uint TimeToLive;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from 
        /// the beginning of this referral entry to the DFS path that corresponds to 
        /// the DFS root or the DFS link for which target information is returned.
        /// </summary>
        public ushort DFSPathOffset;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from the beginning of this referral 
        /// entry to the DFS path that corresponds to the DFS root or the DFS
        /// link for which target information is returned. This path MAY either be the same as the 
        /// path as pointed to by the DFSPathOffset field or be an 8.3 name. In the former case, 
        /// the string referenced MAY be the same as that in the DFSPathOffset field or a duplicate copy.
        /// </summary>
        public ushort DFSAlternatePathOffset;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from beginning of this
        /// referral entry to the DFS target path that correspond 
        /// to this entry.
        /// </summary>
        public ushort NetworkAddressOffset;

        /// <summary>
        /// Unicode character string that specifies DFS root or the DFS link
        /// The string does not contain null-terminator.
        /// </summary>
        public string DFSPath;

        /// <summary>
        /// Unicode character string that specifies DFS root or the DFS link
        /// The string does not contain null-terminator.
        /// </summary>
        public string DFSAlternatePath;

        /// <summary>
        /// Unicode character string that specifies DFS target path
        /// The string does not contain null-terminator.
        /// </summary>
        public string DFSTargetPath;

        /// <summary>
        /// Returns a string of the values of the fields of a v2 referral 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Version Number: {0}, ServerType: {1}, TimeToLive: {2}, DFSPath: {3}, DFSTargetPath {4}", VersionNumber, ServerType, TimeToLive, DFSPath, DFSTargetPath);
        }
    }


    /// <summary>
    /// The format of the version 2 referral entry is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DFS_REFERRAL_V3V4_NonNameListReferral
    {
        /// <summary>
        /// A 16-bit integer indicating the version number of the referral entry. MUST be set to 0x0003, specifying
        /// DFS_REFERRAL_V3.
        /// </summary>
        public ushort VersionNumber;

        /// <summary>
        /// A 16-bit integer indicating the total size of the referral entry, in bytes.
        /// </summary>
        public ushort Size;

        /// <summary>
        /// A 16-bit integer indicating the type of server hosting the target.
        /// </summary>
        public ushort ServerType;

        /// <summary>
        /// A 16-bit field representing a series of flags that are combined by using the bitwise OR operation.
        /// </summary>
        public ReferralEntryFlags_Values ReferralEntryFlags;

        /// <summary>
        /// A 32-bit integer indicating the time-out value, in seconds, of the DFS root or DFS link.
        /// </summary>
        public uint TimeToLive;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from the beginning of this referral entry to the DFS path
        /// that corresponds to the DFS root or DFS link for which target information is returned.
        /// </summary>
        public ushort DFSPathOffset;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from the beginning of this referral entry to the DFS path
        /// that corresponds to the DFS root or DFS link for which target information is returned.
        /// </summary>
        public ushort DFSAlternatePathOffset;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from the beginning of this referral entry to the DFS
        /// target path that corresponds to this entry.
        /// </summary>
        public ushort NetworkAddressOffset;

        /// <summary>
        /// These 16 bytes MUST always be set to 0 by the server and ignored by the client.
        /// </summary>
        public Guid ServiceSiteGuid;

        /// <summary>
        /// Unicode character string that specifies DFS root or the DFS link
        /// The string does not contain null-terminator.
        /// </summary>
        public string DFSPath;

        /// <summary>
        /// Unicode character string that specifies DFS root or the DFS link
        /// The string does not contain null-terminator.
        /// </summary>
        public string DFSAlternatePath;

        /// <summary>
        /// Unicode character string that specifies DFS target path
        /// The string does not contain null-terminator.
        /// </summary>
        public string DFSTargetPath;

        /// <summary>
        /// Returns a string of the values of the fields of a v3 or v4 referral with name list flag not set.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Version Number: {0}, ServerType: {1}, TimeToLive: {2}, DFSPath: {3}, DFSTargetPath: {4}", VersionNumber, ServerType, TimeToLive, DFSPath, DFSTargetPath);
        }

    }


    /// <summary>
    /// The format of the version 2 referral entry is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DFS_REFERRAL_V3V4_NameListReferral
    {
        /// <summary>
        /// A 16-bit integer indicating the version number of the referral entry. MUST be set to 0x0003, specifying
        /// DFS_REFERRAL_V3.
        /// </summary>
        public ushort VersionNumber;

        /// <summary>
        /// A 16-bit integer indicating the total size of the referral entry, in bytes.
        /// </summary>
        public ushort Size;

        /// <summary>
        /// A 16-bit integer indicating the type of server hosting the target.
        /// </summary>
        public ushort ServerType;

        /// <summary>
        /// A 16-bit field representing a series of flags that are combined by using the bitwise OR operation.
        /// </summary>
        public ReferralEntryFlags_Values ReferralEntryFlags;

        /// <summary>
        /// A 32-bit integer indicating the time-out value, in seconds, of the DFS root or DFS link.
        /// </summary>
        public uint TimeToLive;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from the beginning of the referral entry to a domain name.
        /// </summary>
        public ushort SpecialNameOffset;

        /// <summary>
        /// A 16-bit integer indicating the number of DCs being returned for a DC referral request.
        /// </summary>
        public ushort NumberOfExpandedNames;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from the beginning of this referral entry to the first DC
        /// name string returned in response to a DC referral request.
        /// </summary>
        public ushort ExpandedNameOffset;

        /// <summary>
        /// The server MAY insert zero or 16 padding bytes that MUST be ignored by the client.
        /// </summary>
        public byte[] Padding;

        /// <summary>
        /// Unicode character string that specifies the Domain name
        /// The string does not contain null-terminator.
        /// </summary>
        public string SpecialName;

        /// <summary>
        /// Unicode character strings that specifies Dc name array
        /// The strings doe not contain null-terminator.
        /// </summary>
        public string[] DCNameArray;

        /// <summary>
        /// Returns a string of the values of the fields of a v3 or v4 referral with namelist flag set
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Version Number: {0}, ServerType: {1}, TimeToLive: {2}, SpecialName: {3}, NumberOfExpandedNames: {4}", VersionNumber, ServerType, TimeToLive, SpecialName, NumberOfExpandedNames);
        }

    }


    /// <summary>
    /// Possible values for ReferralEntryFlags field.
    /// </summary>
    [Flags()]
    public enum ReferralEntryFlags_Values : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0,

        /// <summary>
        /// MUST be set for a domain referral response or a DC referral response.
        /// </summary>
        NameListReferral = 0x0002,

        /// <summary>
        /// T (TargetSetBoundary): MUST be set if the target corresponding to this referral entry is the first target
        /// of a target set.
        /// </summary>
        T = 0x0004,
    }

    /// <summary>
    /// Referral Entry Types
    /// </summary>
    public enum ReferralEntryType_Values : ushort
    {
        DFS_REFERRAL_V1 = 0x0001,

        DFS_REFERRAL_V2 = 0x0002,

        DFS_REFERRAL_V3 = 0x0003,

        DFS_REFERRAL_V4 = 0x0004,
    }

    /// <summary>
    /// Structure for REQ_GET_DFS_REFERRAL_EX message.
    /// </summary>
    public struct REQ_GET_DFS_REFERRAL_EX
    {
        /// <summary>
        /// A 16-bit integer that indicates the highest DFS referral version understood by the client. 
        /// The DFS referral versions specified by this document are 1 through 4 inclusive. 
        /// A DFS client MUST support DFS referral version 1 through the version number set in this field. 
        /// The referral response messages are referral version dependent and are specified in section 2.2.5
        /// </summary>
        public ushort MaxReferralLevel;

        /// <summary>
        /// 
        /// </summary>
        public REQ_GET_DFS_REFERRAL_RequestFlags RequestFlags;

        /// <summary>
        /// A 32-bit integer that specifies the length of the RequestData field
        /// </summary>
        public uint RequestDataLength;

        /// <summary>
        /// Struct which always contains the requested file name and requested file name length
        /// If request flags are set to 0x1, will also contain site name and site name length
        /// </summary>
        public REQ_GET_DFS_REFERRAL_RequestData RequestData;
    }

    /// <summary>
    /// Structure for the RequestData field of a REQ_GET_DFS_REFERRAL_EX structure 
    /// </summary>
    public struct REQ_GET_DFS_REFERRAL_RequestData
    {

        /// <summary>
        /// A 16-bit integer value that specifies the length of the RequestFileName string in the referral request
        /// </summary>
        [StaticSize(2)]
        public ushort RequestFileNameLength;

        /// <summary>
        /// A Unicode string specifying the path to be resolved. The specified path MUST be 
        /// interpreted in a case-insensitive manner. Its format depends on the type of referral request, as specified in section 3.1.4.2
        /// </summary>
        [Size("RequestFileNameLength")]
        public byte[] RequestFileName;

        /// <summary>
        /// An optional 16-bit integer value that specifies the length of the SiteName string in the referral request
        /// Exists when request flags field of REQ_GET_DFS_REFERRAL_EX is set to "0x1"
        /// </summary>
        public ushort SiteNameLength;

        /// <summary>
        /// A Unicode string specifying the name of the site to which the DFS client computer belongs. 
        /// The length of this string is determined by the value of the SiteNameLength field
        /// Exists when request flags field of REQ_GET_DFS_REFERRAL_EX is set to "0x1"
        /// </summary>
        [Size("SiteNameLength")]
        public byte[] SiteName;
    }

    [Flags]
    public enum REQ_GET_DFS_REFERRAL_RequestFlags : ushort
    {
        None = 0,

        /// <summary>
        /// The SiteName bit MUST be set to 1 if the packet contains the site name of the client
        /// </summary>
        SiteName = 0x00000001,
    }

    /// <summary>
    /// DfscConsts contains the const variable used by test cases
    /// </summary>
    public static class DfscConsts
    {
        /// <summary>
        /// This is for sysvol referral
        /// </summary>
        public const string SysvolShare = "SYSVOL";

        /// <summary>
        /// This is for sysvol referral
        /// </summary>
        public const string NetlogonShare = "NETLOGON";
    }

    /// <summary>
    /// DFS Referral response type
    /// </summary>

    public enum ReferralResponseType
    {
        DCReferralResponse,
        DomainReferralResponse,
        SysvolReferralResponse,
        RootTarget,
        LinkTarget,
        Interlink
    }
}
