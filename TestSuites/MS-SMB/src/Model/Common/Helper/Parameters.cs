// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// Parameter.    
    /// All the overall variables definition will be defined here, except the portion variable.
    /// </summary>
    public static class Parameter
    {
        #region Parameters used for test environment setup

        /// <summary>
        /// Define client platform.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static Platform clientPlatform = Platform.WinVista;

        /// <summary>
        /// Define System Under Test (the SUT) platform.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static Platform sutPlatform = Platform.Win2K8;

        /// <summary>
        /// Define share This is used to represent the name of the resource.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> shareFileNames = new List<string> { "Share1", "Share2", "DFSShare", "QuotaShare" };

        /// <summary>
        /// Define share printer name.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> sharePrinterNames = new List<string> { "PrinterShare1" };

        /// <summary>
        /// Define share pipe name.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> sharePipeNames = new List<string> { "IPC$" };

        /// <summary>
        /// Define share devide name.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> shareDeviceNames = new List<string> { string.Empty };

        /// <summary>
        /// File names to use.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> fileNames = new List<string>{
            "Test1.txt",
            "Test2.txt",
            "ExistTest.txt",
            @"Dir1\Test1.txt"};

        /// <summary>
        /// Dir names to use.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> dirNames = new List<string> { string.Empty, "Dir1", "Dir2" };

        /// <summary>
        /// Namedpipe names to use.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> pipeNames = new List<string> { "Pipe1" };

        /// <summary>
        /// Mailslot names to use.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static List<string> mailslotNames = new List<string> { "Mailslot1" };

        #endregion.

        #region Parameters used to maintain the SUT configuration

        /// <summary>
        /// Set supportStream to false.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportStream;

        /// <summary>
        /// Set SupportInfoLevelPassThrough to false.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportInfoLevelPassThrough;

        /// <summary>
        /// Set supportDFS to false.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportDfs;

        /// <summary>
        /// Set RAP the SUT status.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isRapServerActive = true;

        /// <summary>
        /// Set supportQuota to false.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportQuota;

        /// <summary>
        /// It indicates whether the SUT support unique file ID values.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportUniqueFileId;

        /// <summary>
        /// Set supportPreviousVersion to false.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportPreviousVersion;

        /// <summary>
        /// Set supportResumeKey to false.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportResumeKey;

        /// <summary>
        /// Set supportCopyChunk to false.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportCopyChunk;

        /// <summary>
        /// Whether the SUT support NT SMBs.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSupportNtSmb;

        /// <summary>
        /// Define FS Type.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static FileSystemType fsType = FileSystemType.Ntfs;

        /// <summary>
        /// the SUT Message Signing Policy.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static SignState sutSignState = SignState.Disabled;

        /// <summary>
        /// The mode of created name pipe.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isMessageModePipe;

        /// <summary>
        /// Whether testing Set_Quota Status_Access_Denied.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isSetQuotaAccessDenied;

        #endregion

        #region Parameters used to maintain the client state

        /// <summary>
        /// The total bytes can be written to a file.
        /// </summary>
        /// Disable warning CA2211 because model need to change this variable according to Test Case design.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static int totalBytesWritten;

        #endregion
    }
}
