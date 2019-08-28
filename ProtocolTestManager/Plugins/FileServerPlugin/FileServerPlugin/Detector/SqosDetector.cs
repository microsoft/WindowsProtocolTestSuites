// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;
using System.IO;

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
            logWriter.AddLog(LogLevel.Information, "Share name: " + info.targetShareFullPath);

            DetectResult result = DetectResult.UnSupported;
            if (info.smb2Info.MaxSupportedDialectRevision < DialectRevision.Smb311)
            {
                return result;
            }

            #region copy test VHD file to share
            string vhdOnSharePath = Path.Combine(info.targetShareFullPath, vhdName);
            CopyTestVHD(info.targetShareFullPath, vhdOnSharePath);
            #endregion

            #region SQOS dialect 1.1

            bool versionTestRes = TestSqosVersion(info.BasicShareName, SQOS_PROTOCOL_VERSION.Sqos11);
            if (versionTestRes)
            {
                info.SqosVersion = SQOS_PROTOCOL_VERSION.Sqos11;
                result = DetectResult.Supported;
                logWriter.AddLog(LogLevel.Information, "SQOS dialect 1.1 is supported");
                return result;
            }
            else
            {
                logWriter.AddLog(LogLevel.Information, "The server doesn't support SQOS version 2.");
            }
            #endregion

            #region SQOS dialect 1.0
            versionTestRes = TestSqosVersion(info.BasicShareName, SQOS_PROTOCOL_VERSION.Sqos10);
            if (versionTestRes)
            {
                info.SqosVersion = SQOS_PROTOCOL_VERSION.Sqos10;
                result = DetectResult.Supported;
                logWriter.AddLog(LogLevel.Information, "SQOS dialect 1.0 is supported");
                return result;
            }
            else
            {
                result = DetectResult.UnSupported;
                logWriter.AddLog(LogLevel.Information, @"The server doesn't support SQOS.");
            }
            #endregion

            DeleteTestVHD(info.targetShareFullPath, vhdOnSharePath);
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