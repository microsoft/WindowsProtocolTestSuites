// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    /// <summary>
    /// Detector to detect RSVD 
    /// </summary>
    public partial class FSDetector
    {
        private string initiatorHostName = "Client01";

        /// <summary>
        /// Check if the server supports RSVD.
        /// </summary>
        public DetectResult CheckRsvdSupport(ref DetectionInfo info)
        {
            DetectResult result = DetectResult.DetectFail;
            logWriter.AddLog(DetectLogLevel.Information, "Share path: " + info.targetShareFullPath);

            #region Copy test VHD file to the target share to begin detecting RSVD

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
                logWriter.AddLog(DetectLogLevel.Information, @"Detect RSVD failed with exception: " + e.Message);
                return result;
            }
            #endregion

            try
            {
                #region RSVD version 2
                bool versionTestRes = TestRsvdVersion(vhdName + fileNameSuffix, info.BasicShareName, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);
                if (versionTestRes)
                {
                    info.RsvdVersion = RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2;
                    result = DetectResult.Supported;
                    logWriter.AddLog(DetectLogLevel.Information, "RSVD version 2 is supported");
                    return result;
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Information, "The server doesn't support RSVD version 2.");
                }
                #endregion

                #region RSVD version 1
                versionTestRes = TestRsvdVersion(vhdName + fileNameSuffix, info.BasicShareName, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);
                if (versionTestRes)
                {
                    info.RsvdVersion = RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1;
                    result = DetectResult.Supported;
                    logWriter.AddLog(DetectLogLevel.Information, "RSVD version 1 is supported");
                    return result;
                }
                else
                {
                    result = DetectResult.UnSupported;
                    logWriter.AddLog(DetectLogLevel.Information, @"The server doesn't support RSVD.");
                }
                #endregion

            }
            catch (Exception e)
            {
                logWriter.AddLog(DetectLogLevel.Information, @"Detect RSVD failed with exception: " + e.Message);
            }

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

        /// <summary>
        /// Connect to the share VHD file.
        /// </summary>
        private bool TestRsvdVersion(
            string vhdxName,
            string share,
            RSVD_PROTOCOL_VERSION version)
        {
            using (RsvdClient client = new RsvdClient(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                CREATE_Response response;
                Smb2CreateContextResponse[] serverContexts;
                client.Connect(SUTName, SUTIpAddress, Credential.DomainName, Credential.AccountName, Credential.Password, SecurityPackageType, true, share);

                Smb2CreateContextRequest[] contexts;
                if (version == RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2)
                {
                    contexts = new Smb2CreateContextRequest[]
                    {
                        new Smb2CreateSvhdxOpenDeviceContextV2
                        {
                            Version = (uint)RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2,
                            OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER,
                            InitiatorHostName = initiatorHostName,
                            InitiatorHostNameLength = (ushort)(initiatorHostName.Length * 2),  // InitiatorHostName is a null-terminated Unicode UTF-16 string 
                            VirtualDiskPropertiesInitialized = 0,
                            ServerServiceVersion = 0,
                            VirtualSectorSize = 0,
                            PhysicalSectorSize = 0,
                            VirtualSize = 0
                        }
                    };
                    foreach (Smb2CreateSvhdxOpenDeviceContextV2 context in contexts)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, @"OpenSharedVirtualDisk request was sent with context: ");
                        logWriter.AddLog(DetectLogLevel.Information, @"Version: " + context.Version.ToString());
                        logWriter.AddLog(DetectLogLevel.Information, @"OriginatorFlags: " + context.OriginatorFlags.ToString());
                        logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostName: " + context.InitiatorHostName.ToString());
                        logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostNameLength: " + context.InitiatorHostNameLength.ToString());

                    }
                }
                else
                {
                    contexts = new Smb2CreateContextRequest[]
                    {
                        new Smb2CreateSvhdxOpenDeviceContext
                        {
                            Version = (uint)RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1,
                            OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER,
                            InitiatorHostName = initiatorHostName,
                            InitiatorHostNameLength = (ushort)(initiatorHostName.Length * 2)  // InitiatorHostName is a null-terminated Unicode UTF-16 string 
                        }
                    };
                    foreach (Smb2CreateSvhdxOpenDeviceContext context in contexts)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, @"OpenSharedVirtualDisk request was sent with context: ");
                        logWriter.AddLog(DetectLogLevel.Information, @"Version: " + context.Version.ToString());
                        logWriter.AddLog(DetectLogLevel.Information, @"OriginatorFlags: " + context.OriginatorFlags.ToString());
                        logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostName: " + context.InitiatorHostName.ToString());
                        logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostNameLength: " + context.InitiatorHostNameLength.ToString());
                    }
                }

                uint status;
                status = client.OpenSharedVirtualDisk(
                    vhdxName,
                    TestTools.StackSdk.FileAccessService.FsCreateOption.FILE_NO_INTERMEDIATE_BUFFERING,
                    contexts,
                    out serverContexts,
                    out response);

                logWriter.AddLog(DetectLogLevel.Information, @"Get OpenSharedVirtualDisk response with status: " + status);

                if (serverContexts == null)
                {
                    logWriter.AddLog(DetectLogLevel.Information, @"The response does not contain any server contexts.");
                }
                else
                {
                    foreach (Smb2CreateContextResponse ctx in serverContexts)
                    {
                        Type type = ctx.GetType();
                        if (type.Name == "Smb2CreateSvhdxOpenDeviceContextResponse")
                        {
                            logWriter.AddLog(DetectLogLevel.Information, @"Server response context is Smb2CreateSvhdxOpenDeviceContextResponse. ");

                            Smb2CreateSvhdxOpenDeviceContextResponse openDeviceContext = ctx as Smb2CreateSvhdxOpenDeviceContextResponse;
                            logWriter.AddLog(DetectLogLevel.Information, @"Version is: " + openDeviceContext.Version.ToString());
                            logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostName is: " + openDeviceContext.InitiatorHostName.ToString());
                            logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostNameLength is: " + openDeviceContext.InitiatorHostNameLength.ToString());
                        }

                        if (type.Name == "Smb2CreateSvhdxOpenDeviceContextResponseV2")
                        {
                            logWriter.AddLog(DetectLogLevel.Information, @"Server response context is Smb2CreateSvhdxOpenDeviceContextResponseV2. ");

                            Smb2CreateSvhdxOpenDeviceContextResponseV2 openDeviceContext = ctx as Smb2CreateSvhdxOpenDeviceContextResponseV2;
                            logWriter.AddLog(DetectLogLevel.Information, @"Version is: " + openDeviceContext.Version.ToString());
                            logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostName is: " + openDeviceContext.InitiatorHostName.ToString());
                            logWriter.AddLog(DetectLogLevel.Information, @"InitiatorHostNameLength is: " + openDeviceContext.InitiatorHostNameLength.ToString());
                        }
                    }
                }

                bool result = false;

                if (status != Smb2Status.STATUS_SUCCESS)
                {
                    result = false;
                    logWriter.AddLog(DetectLogLevel.Information, @"Get status " + status + @" from server response.");
                    logWriter.AddLog(DetectLogLevel.Information, @"The RSVD version " + version + @" is found not supported by server.");
                    return result;
                }

                result = CheckOpenDeviceContext(serverContexts, version);
                client.CloseSharedVirtualDisk();

                return result;
            }
        }

        private bool CheckOpenDeviceContext(Smb2CreateContextResponse[] servercreatecontexts, RSVD_PROTOCOL_VERSION expectVersion)
        {
            if (servercreatecontexts == null)
            {
                if (expectVersion == RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1)
                {
                    // <10> Section 3.2.5.1:  Windows Server 2012 R2 without [MSKB-3025091] doesn't return SVHDX_OPEN_DEVICE_CONTEXT_RESPONSE.
                    // So if the open device context returns success, but without SVHDX_OPEN_DEVICE_CONTEXT_RESPONSE, the SUT may still support RSVD version 1.
                    return true;
                }
                else
                {
                    return false;
                }
            }

            try
            {
                foreach (var context in servercreatecontexts)
                {
                    Type type = context.GetType();
                    if (type.Name == "Smb2CreateSvhdxOpenDeviceContextResponse")
                    {
                        Smb2CreateSvhdxOpenDeviceContextResponse openDeviceContext = context as Smb2CreateSvhdxOpenDeviceContextResponse;
                        if ((openDeviceContext != null) && (openDeviceContext.Version == (uint)expectVersion))
                        {
                            return true;
                        }
                    }

                    if (type.Name == "Smb2CreateSvhdxOpenDeviceContextResponseV2")
                    {
                        Smb2CreateSvhdxOpenDeviceContextResponseV2 openDeviceContext = context as Smb2CreateSvhdxOpenDeviceContextResponseV2;
                        if ((openDeviceContext != null) && (openDeviceContext.Version == (uint)expectVersion))
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        private void ConnectShareByNetUse(string sharePath)
        {
            Process process = new Process();
            process.StartInfo.FileName = "net.exe";
            process.StartInfo.Arguments = $"use {sharePath} {Credential.Password} /user:{Credential.DomainName}\\{Credential.AccountName}";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        private void CopyTestVHDByClient(DetectionInfo info, string vhdName)
        {
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId = default;
                ulong sessionId = default;
                uint treeId = default;
                FILEID fileId = default;

                try
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Try to connect share {0}.", info.BasicShareName));
                    ConnectToShare(info.BasicShareName, info, client, out messageId, out sessionId, out treeId);
                }
                catch
                {
                    // Show error to user.
                    logWriter.AddLog(DetectLogLevel.Error, "Did not find shares on SUT. Please check the share setting and SUT password.");
                }

                var packetHeader = (info.smb2Info.IsRequireMessageSigning || info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

                try
                {
                    var status = client.Create(
                        1,
                        1,
                        packetHeader,
                        messageId++,
                        sessionId,
                        treeId,
                        vhdName,
                        AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                        ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        CreateDisposition_Values.FILE_OPEN_IF,
                        File_Attributes.NONE,
                        ImpersonationLevel_Values.Impersonation,
                        SecurityFlags_Values.NONE,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                        null,
                        out fileId,
                        out _,
                        out _,
                        out _);

                    if (status == Smb2Status.STATUS_SUCCESS)
                    {
                        var content = File.ReadAllBytes(GetVhdSourcePath());
                        var messageCount = content.Length % 65536 == 0 ? content.Length / 65536 : content.Length / 65536 + 1; // Simplify credit calculation.
                        var offset = 0ul;

                        while (messageCount > 0)
                        {
                            status = client.Write(
                                1,
                                1,
                                packetHeader,
                                messageId++,
                                sessionId,
                                treeId,
                                offset,
                                fileId,
                                Channel_Values.CHANNEL_NONE,
                                WRITE_Request_Flags_Values.None,
                                new byte[0],
                                content.Skip((int)offset).Take(65536).ToArray(),
                                out _,
                                out _);

                            if (status == Smb2Status.STATUS_SUCCESS)
                            {
                                offset += 65536;
                                messageCount--;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (status != Smb2Status.STATUS_SUCCESS)
                        {
                            LogFailedStatus("WRITE", status);
                            throw new Exception("WRITE failed with " + Smb2Status.GetStatusCode(status));
                        }
                    }
                    else
                    {
                        LogFailedStatus("CREATE", status);
                        throw new Exception("CREATE failed with " + Smb2Status.GetStatusCode(status));
                    }
                }
                finally
                {
                    try
                    {
                        LogOffSession(client, packetHeader, messageId, sessionId, treeId, fileId);
                    }
                    catch (Exception ex)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, string.Format("Exception thrown when logging off the share, reason {0}.", ex.Message));
                    }
                }
            }
        }

        private void DeleteTestVHDByClient(DetectionInfo info, string vhdName)
        {
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId = default;
                ulong sessionId = default;
                uint treeId = default;
                FILEID fileId = default;

                try
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Try to connect share {0}.", info.BasicShareName));
                    ConnectToShare(info.BasicShareName, info, client, out messageId, out sessionId, out treeId);
                }
                catch
                {
                    // Show error to user.
                    logWriter.AddLog(DetectLogLevel.Error, "Did not find shares on SUT. Please check the share setting and SUT password.");
                }

                var packetHeader = (info.smb2Info.IsRequireMessageSigning || info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

                try
                {
                    var status = client.Create(
                        1,
                        1,
                        packetHeader,
                        messageId++,
                        sessionId,
                        treeId,
                        vhdName,
                        AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                        ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                        CreateDisposition_Values.FILE_OPEN_IF,
                        File_Attributes.NONE,
                        ImpersonationLevel_Values.Impersonation,
                        SecurityFlags_Values.NONE,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                        null,
                        out fileId,
                        out _,
                        out _,
                        out _);

                    if (status != Smb2Status.STATUS_SUCCESS)
                    {
                        LogFailedStatus("CREATE", status);
                        throw new Exception("CREATE failed with " + Smb2Status.GetStatusCode(status));
                    }
                }
                finally
                {
                    try
                    {
                        LogOffSession(client, packetHeader, messageId, sessionId, treeId, fileId);
                    }
                    catch (Exception ex)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, string.Format("Exception thrown when logging off the share, reason {0}.", ex.Message));
                    }
                }
            }
        }

        private void LogOffSession(Smb2Client client, Packet_Header_Flags_Values packetHeader, ulong messageId, ulong sessionId, uint treeId, FILEID fileId)
        {
            if (fileId.Persistent != 0 || fileId.Volatile != 0)
            {
                client.Close(
                    1,
                    1,
                    packetHeader,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out _,
                    out _);
            }

            client.TreeDisconnect(
                1,
                1,
                packetHeader,
                messageId++,
                sessionId,
                treeId,
                out _,
                out _);
            client.LogOff(
                1,
                1,
                packetHeader,
                messageId++,
                sessionId,
                out _,
                out _);
        }

        private void CopyTestVHD(string sharePath, string vhdOnSharePath)
        {
            try
            {
                ConnectShareByNetUse(sharePath);

                if (File.Exists(vhdOnSharePath))
                {
                    return;
                }

                string vhdxPath = GetVhdSourcePath();
                File.Copy(vhdxPath, vhdOnSharePath);
            }
            catch (Exception e)
            {
                throw new Exception("Copy test VHD file failed: " + e.Message);
            }
        }

        private void DeleteTestVHD(string sharePath, string vhdOnSharePath)
        {
            try
            {
                ConnectShareByNetUse(sharePath);

                if (!File.Exists(vhdOnSharePath))
                {
                    return;
                }

                File.Delete(vhdOnSharePath);
            }
            catch (Exception e)
            {
                throw new Exception("Delete test VHD file failed: " + e.Message);
            }
        }

        private string GetVhdSourcePath()
        {
            string res = string.Empty;
            string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string vhdxPath = Path.Combine(Directory.GetParent(assemblyPath).FullName, "Plugin", "data", vhdName);
            if (File.Exists(vhdxPath))
            {
                res = vhdxPath;
            }

            return res;
        }
    }
}