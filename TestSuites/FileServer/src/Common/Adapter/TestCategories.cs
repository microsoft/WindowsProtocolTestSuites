// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    public static class TestCategories
    {
        public const string Bvt = "BVT";
        public const string Model = "Model";
        public const string Failover = "Failover";

        public const string NonSmb = "NonSmb";
        public const string Smb2002 = "Smb2002";
        public const string Smb21 = "Smb21";
        public const string Smb30 = "Smb30";
        public const string Smb302 = "Smb302";
        public const string Smb311 = "Smb311";
        public const string Dfsc = "DFSC";
        public const string Swn = "SWN";
        // For FSRVP test cases which require cluster environment
        public const string Fsrvp = "FSRVP";
        // For FSRVP test cases which do not require cluster environment
        public const string FsrvpNonClusterRequired = "FSRVPNonClusterRequired";
        public const string RsvdVersion1 = "RSVDVersion1";
        public const string RsvdVersion2 = "RSVDVersion2";
        public const string Fsa = "FSA";
        public const string Sqos = "SQOS";
        public const string HvrsSmb = "HVRS-SMB";
        public const string HvrsFsa = "HVRS-FSA";

        public const string Auth = "Auth";
        public const string ShareAccessCheck = "ShareAccessCheck";
        public const string FolderAccessCheck = "FolderAccessCheck";
        public const string FileAccessCheck = "FileAccessCheck";
        public const string CBAC = "CBAC";
        public const string KerberosAuthentication = "KerberosAuthentication";

        // SMB2&3 features
        // Note: For one case, only one feature category could be set
        public const string FsctlLmrRequestResiliency = "FsctlLmrRequestResiliency";
        public const string FsctlFileLevelTrim = "FsctlFileLevelTrim";
        public const string FsctlValidateNegotiateInfo = "FsctlValidateNegotiateInfo";
        public const string FsctlSetGetIntegrityInformation = "FsctlSetGetIntegrityInformation";
        public const string FsctlOffloadReadWrite = "FsctlOffloadReadWrite";
        public const string FsctlEnumerateSnapShots = "FsctlEnumerateSnapShots";

        public const string Negotiate = "Negotiate";
        public const string Session = "Session";
        public const string Tree = "Tree";
        public const string Credit = "Credit";
        public const string Signing = "Signing";
        public const string Encryption = "Encryption";
        public const string Compression = "Compression";

        public const string OplockOnShareWithoutForceLevel2OrSOFS = "OplockOnShareWithoutForceLevel2OrSOFS";
        public const string OplockOnShareWithoutForceLevel2WithSOFS = "OplockOnShareWithoutForceLevel2WithSOFS";
        public const string OplockOnShareWithForceLevel2WithoutSOFS = "OplockOnShareWithForceLevel2WithoutSOFS";
        public const string OplockOnShareWithForceLevel2AndSOFS = "OplockOnShareWithForceLevel2AndSOFS";

        public const string DurableHandleV1BatchOplock = "DurableHandleV1BatchOplock";
        public const string DurableHandleV1LeaseV1 = "DurableHandleV1LeaseV1";

        public const string DurableHandleV2BatchOplock = "DurableHandleV2BatchOplock";
        public const string DurableHandleV2LeaseV1 = "DurableHandleV2LeaseV1";
        public const string DurableHandleV2LeaseV2 = "DurableHandleV2LeaseV2";

        // For PersistentHandle cases which require cluster environment
        public const string PersistentHandle = "PersistentHandle";
        // For PersistentHandle cases which do not require cluster environment
        public const string PersistentHandleNonClusterRequired = "PersistentHandleNonClusterRequired";

        public const string AppInstanceId = "AppInstanceId";
        public const string AppInstanceVersion = "AppInstanceVersion";
        public const string Replay = "Replay";
        public const string CreateClose = "CreateClose";
        public const string DirectoryLeasing = "DirectoryLeasing";
        public const string LeaseV1 = "LeaseV1";
        public const string LeaseV2 = "LeaseV2";
        public const string MultipleChannel = "MultipleChannel";
        public const string Compound = "Compound";
        public const string ChangeNotify = "ChangeNotify";
        public const string QueryAndSetFileInfo = "QueryAndSetFileInfo";
        public const string LockUnlock = "LockUnlock";
        public const string QueryDir = "QueryDir";
        public const string QueryInfo = "QueryInfo";

        // For some advanced test cases which cover more than 1 feature and require cluster environment.
        public const string CombinedFeature = "CombinedFeature";

        // For some advanced test cases which cover more than 1 feature and do not require cluster environment.
        public const string CombinedFeatureNonClusterRequired = "CombinedFeatureNonClusterRequired";

        // For functional test cases
        public const string MixedOplockLease = "MixedOplockLease";
        public const string OperateOneFileFromTwoNodes = "OperateOneFileFromTwoNodes";

        // MS-FSA features
        public const string ChangeNotification = "ChangeNotification";
        public const string CreateFile = "CreateFile";
        public const string OpenFile = "OpenFile";
        public const string CloseFile = "CloseFile";
        public const string IoCtlRequest = "IoCtlRequest";
        public const string LockAndUnlock = "LockAndUnlock";
        public const string QueryFileInformation = "QueryFileInformation";
        public const string SetFileInformation = "SetFileInformation";
        public const string QueryFileSystemInformation = "QueryFileSystemInformation";
        public const string SetFileSystemInformation = "SetFileSystemInformation";
        public const string QueryQuotaInformation = "QueryQuotaInformation";
        public const string SetQuotaInformation = "SetQuotaInformation";
        public const string QuerySecurityInformation = "QuerySecurityInformation";
        public const string SetSecurityInformation = "SetSecurityInformation";
        public const string QueryDirectory = "QueryDirectory";
        public const string ReadFile = "ReadFile";
        public const string WriteFile = "WriteFile";
        public const string FlushCachedData = "FlushCachedData";
        public const string AlternateDataStream = "AlternateDataStream";
        public const string FileAccess = "FileAccess";

        /// <summary>
        /// Indicate this test case is positive
        /// </summary>
        public const string Positive = "Positive";

        /// <summary>
        /// Unexpected field values according to technical document.
        /// </summary>
        public const string UnexpectedFields = "UnexpectedFields";

        /// <summary>
        /// Invalid SessionId, TreeId, FileId and other identifiers according to technical document.
        /// </summary>
        public const string InvalidIdentifier = "InvalidIdentifier";

        /// <summary>
        /// Specific data length is out of boundary according to technical document.
        /// </summary>
        public const string OutOfBoundary = "OutOfBoundary";

        /// <summary>
        /// To evaluate SMB2 server's compatibility according to technical document.
        /// SMB2 server should be able to handle below scenarios gracefully:
        /// (1) A specific feature is not implemented/supported, server should response accordingly without crash.
        /// (2) Specific operation is not allowed according to previous operations, server will response with correct error code.
        /// (3) For a request with complex field combinations, server will handle it gracefully.
        /// (4) For detrimental actions, server will terminate SMB2 connection without crash.
        /// </summary>
        public const string Compatibility = "Compatibility";

        /// <summary>
        /// Unexpected create context, negotiate context and etc. according to technical document.
        /// </summary>
        public const string UnexpectedContext = "UnexpectedContext";
    }
}
