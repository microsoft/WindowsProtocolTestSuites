// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    /// <summary>
    /// Detector to detect SQOS 
    /// </summary>
    public partial class FSDetector
    {
        /// <summary>
        /// Check if the server supports SQOS.
        /// </summary>
        public DetectResult CheckSqosSupport(ref DetectionInfo info)
        {
            DetectResult result = DetectResult.UnSupported;
            logWriter.AddLog(DetectLogLevel.Information, "Check SMB2 dialect. The MaxSupportedDialectRevision is: " + info.smb2Info.MaxSupportedDialectRevision);
            if (info.smb2Info.MaxSupportedDialectRevision < DialectRevision.Smb311)
            {
                return result;
            }

            logWriter.AddLog(DetectLogLevel.Information, "Check share existence: " + info.BasicShareName);
            if (DetectShareExistence(info, info.BasicShareName) != DetectResult.Supported)
            {
                return DetectResult.DetectFail;
            }

            #region copy test VHD file to share
            string vhdOnSharePath = Path.Combine(info.targetShareFullPath, vhdName);

            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    CopyTestVHD(info.targetShareFullPath, vhdOnSharePath);
                }
                else
                {
                    CopyTestVHDByClient(info, vhdName);
                }
            }
            catch (Exception e)
            {
                logWriter.AddLog(DetectLogLevel.Information, @"Detect SQOS failed with exception: " + e.Message);
                return DetectResult.DetectFail;

            }
            #endregion

            #region SQOS dialect 1.1

            bool versionTestRes = TestSqosVersion(info.BasicShareName, SQOS_PROTOCOL_VERSION.Sqos11);
            if (versionTestRes)
            {
                info.SqosVersion = SQOS_PROTOCOL_VERSION.Sqos11;
                result = DetectResult.Supported;
                logWriter.AddLog(DetectLogLevel.Information, "SQOS dialect 1.1 is supported");
                return result;
            }
            else
            {
                logWriter.AddLog(DetectLogLevel.Information, "The server doesn't support SQOS version 2.");
            }
            #endregion

            #region SQOS dialect 1.0
            versionTestRes = TestSqosVersion(info.BasicShareName, SQOS_PROTOCOL_VERSION.Sqos10);
            if (versionTestRes)
            {
                info.SqosVersion = SQOS_PROTOCOL_VERSION.Sqos10;
                result = DetectResult.Supported;
                logWriter.AddLog(DetectLogLevel.Information, "SQOS dialect 1.0 is supported");
                return result;
            }
            else
            {
                result = DetectResult.UnSupported;
                logWriter.AddLog(DetectLogLevel.Information, @"The server doesn't support SQOS.");
            }
            #endregion

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                DeleteTestVHD(info.targetShareFullPath, vhdOnSharePath);
            }
            else
            {
                DeleteTestVHDByClient(info, vhdName);
            }

            return result;
        }

        private bool TestSqosVersion(string share, SQOS_PROTOCOL_VERSION version)
        {
            using (SqosClient client = new SqosClient(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                client.ConnectToVHD(
                    SUTName, SUTIpAddress, Credential.DomainName, Credential.AccountName, Credential.Password, SecurityPackageType,
                    true, share, vhdName);
                SqosRequestPacket sqosRequest = new SqosRequestPacket(version == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
                    (ushort)version,
                    SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_LOGICAL_FLOW_ID | SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_GET_STATUS,
                    Guid.NewGuid(),
                    Guid.Empty,
                    Guid.Empty,
                    string.Empty,
                    string.Empty
                    );
                SqosResponsePacket sqosResponse;
                uint status = client.SendAndReceiveSqosPacket(
                    sqosRequest,
                    out sqosResponse);

                if (status != Smb2Status.STATUS_SUCCESS)
                    return false;

                return (ushort)version == sqosResponse.Header.ProtocolVersion;
            }
        }
    }
}