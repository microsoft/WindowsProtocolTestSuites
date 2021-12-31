// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiService;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter
{
    /// <summary>
    /// SUT control adapter to create, delete and modify files on remote server.
    /// </summary>
    public class WspSutAdapter : ManagedAdapterBase, IWspSutAdapter
    {
        /// <summary>
        /// The WSP test site used for getting config and logging.
        /// </summary>
        private ITestSite wspTestSite;

        /// <summary>
        /// Name of the connected server.
        /// </summary>
        private string serverName = null;

        /// <summary>
        /// Name of the connected user.
        /// </summary>
        private string userName = null;

        /// <summary>
        /// Domain of the connected user.
        /// </summary>
        private string domainName = null;

        /// <summary>
        /// Password of the connected user.
        /// </summary>
        private string password = null;

        /// <summary>
        /// Security package used for authentication.
        /// </summary>
        private string securityPackage = null;

        /// <summary>
        /// Whether the client will use server-initiated SPNEGO authentication.
        /// </summary>
        private bool useServerGssToken = false;

        /// <summary>
        /// Name of the connected share.
        /// </summary>
        private string shareName = null;

        /// <summary>
        /// Path to the remote share.
        /// </summary>
        private string sharePath = null;

        /// <summary>
        /// Initial file content of a newly created file.
        /// </summary>
        private const string initialFileContent = "newfile";

        /// <summary>
        /// The new file content of a file to be modified.
        /// </summary>
        private const string newFileContent = "newfile to modify";

        /// <summary>
        /// Timeout of the SMB2Client.
        /// </summary>
        private TimeSpan smb2ClientTimeout = default(TimeSpan);

        /// <summary>
        /// Smb2Client used for send and receive messages.
        /// </summary>
        private Smb2Client smb2Client = null;

        /// <summary>
        /// The default flags value in an SMB2 message.
        /// </summary>
        private Packet_Header_Flags_Values defaultFlags = Packet_Header_Flags_Values.NONE;

        /// <summary>
        /// The message ID for the SMB2 message sequencing.
        /// </summary>
        private ulong messageId = 0;

        /// <summary>
        /// The session ID for the SMB2 session.
        /// </summary>
        private ulong sessionId = 0;

        /// <summary>
        /// The tree ID of the share.
        /// </summary>
        private uint treeId = 0;

        /// <summary>
        /// The file ID of the current file.
        /// </summary>
        private FILEID fileId = default(FILEID);

        private const uint STATUS_NO_MORE_FILES = 0x80000006;

        private const uint queryDirectoryRequestBufferSize = 0x10000;

        private const byte fileBasicInformationClassValue = 0x04;

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            wspTestSite = testSite;
            serverName = wspTestSite.Properties.Get("ServerComputerName");
            userName = wspTestSite.Properties.Get("UserName");
            domainName = wspTestSite.Properties.Get("DomainName");
            password = wspTestSite.Properties.Get("Password");
            securityPackage = wspTestSite.Properties.Get("SupportedSecurityPackage");
            useServerGssToken = bool.Parse(wspTestSite.Properties.Get("UseServerGssToken"));
            shareName = wspTestSite.Properties.Get("ShareName");
            sharePath = $"\\\\{serverName}\\{shareName}";
            smb2ClientTimeout = TimeSpan.FromSeconds(int.Parse(wspTestSite.Properties.Get("SMB2ClientTimeout")));
        }

        /// <summary>
        /// This method is to create a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be created.</param>
        public int CreateFile(string fileName)
        {
            int result;
            var filePath = GetSimplifiedFilePath(fileName);

            try
            {
                ConnectToShare();

                LocateParentDirectory(filePath);
                Create(
                    filePath: filePath,
                    desiredAccess: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    createOptions: CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    createDispositions: CreateDisposition_Values.FILE_OPEN_IF);
                Write(initialFileContent);
                Close();

                result = 0;
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown during CreateFile: {ex}");
                result = 1;
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        /// <summary>
        /// This method is to delete a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        public int DeleteFile(string fileName)
        {
            int result;
            var filePath = GetSimplifiedFilePath(fileName);

            try
            {
                ConnectToShare();

                LocateParentDirectory(filePath);
                Create(
                    filePath: filePath,
                    desiredAccess: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    createOptions: CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    createDispositions: CreateDisposition_Values.FILE_OPEN_IF);
                Close();

                result = 0;
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown during DeleteFile: {ex}");
                result = 1;
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        /// <summary>
        /// This method is to modify a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be modified.</param>
        public int ModifyFile(string fileName)
        {
            int result;
            var filePath = GetSimplifiedFilePath(fileName);

            try
            {
                ConnectToShare();

                LocateParentDirectory(filePath);
                Create(
                    filePath: filePath,
                    desiredAccess: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    createOptions: CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    createDispositions: CreateDisposition_Values.FILE_OPEN);
                Write(newFileContent);
                Close();

                result = 0;
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown during ModifyFile: {ex}");
                result = 1;
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        /// <summary>
        /// This method is to modify the attributes of a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be modified.</param>
        /// <param name="isReadonly">The file is read-only.</param>
        /// <param name="isHidden">The file is hidden.</param>
        public int ModifyFileAttributes(string fileName, bool isReadonly, bool isHidden)
        {
            int result;
            var filePath = GetSimplifiedFilePath(fileName);

            try
            {
                ConnectToShare();

                LocateParentDirectory(filePath);
                Create(
                    filePath: filePath,
                    desiredAccess: AccessMask.FILE_READ_ATTRIBUTES | AccessMask.FILE_WRITE_ATTRIBUTES,
                    createOptions: CreateOptions_Values.NONE,
                    createDispositions: CreateDisposition_Values.FILE_OPEN);
                SetInfo(isReadonly, isHidden);
                Close();

                result = 0;
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown during ModifyFileAttributes: {ex}");
                result = 1;
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        private string GetSimplifiedFilePath(string fileName)
        {
            var filePath = $"{sharePath}\\Data\\Test\\{fileName}";
            return Path.GetFullPath(filePath).Substring(sharePath.Length + 1);
        }

        /// <summary>
        /// Check the SMB2 status code and throw specific exceptions if the status code is not a successful status code.
        /// </summary>
        /// <param name="status">The SMB2 status code to be checked.</param>
        /// <param name="opName">The name of the SMB2 operation to be checked.</param>
        private void CheckStatusCode(uint status, string opName)
        {
            if (status != Smb2Status.STATUS_SUCCESS &&
                status != Smb2Status.STATUS_PENDING &&
                status != Smb2Status.STATUS_MORE_PROCESSING_REQUIRED &&
                status != STATUS_NO_MORE_FILES &&
                status != Smb2Status.STATUS_END_OF_FILE)
            {
                throw new InvalidOperationException($"Operation \"{opName}\" failed with error code {Smb2Status.GetStatusCode(status)}.");
            }
        }

        private void Create(string filePath, AccessMask desiredAccess, CreateOptions_Values createOptions, CreateDisposition_Values createDispositions)
        {
            var status = smb2Client.Create(
                creditCharge: 1,
                creditRequest: 1,
                flags: defaultFlags,
                messageId: messageId++,
                sessionId: sessionId,
                treeId: treeId,
                path: filePath,
                desiredAccess: desiredAccess,
                shareAccess: ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                createOptions: createOptions,
                createDispositions: createDispositions,
                fileAttributes: File_Attributes.NONE,
                impersonationLevel: ImpersonationLevel_Values.Impersonation,
                securityFlag: SecurityFlags_Values.NONE,
                requestedOplockLevel: RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                createContexts: null,
                out fileId,
                out _,
                out _,
                out _);
            CheckStatusCode(status, nameof(Smb2Client.Create));
        }

        private void Write(string content)
        {
            var status = smb2Client.Write(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    sessionId: sessionId,
                    treeId: treeId,
                    offset: 0,
                    fileId: fileId,
                    channel: Channel_Values.CHANNEL_NONE,
                    writeFlags: WRITE_Request_Flags_Values.None,
                    writeChannelInfo: new byte[0],
                    Encoding.UTF8.GetBytes(content),
                    out _,
                    out _);
            CheckStatusCode(status, nameof(Smb2Client.Write));
        }

        public void SetInfo(bool isReadonly, bool isHidden)
        {
            var fileAttribute = File_Attributes.FILE_ATTRIBUTE_NORMAL | File_Attributes.FILE_ATTRIBUTE_ARCHIVE;
            if (isReadonly)
            {
                fileAttribute |= File_Attributes.FILE_ATTRIBUTE_READONLY;
            }

            if (isHidden)
            {
                fileAttribute |= File_Attributes.FILE_ATTRIBUTE_HIDDEN;
            }

            var fileInfo = new Fscc.FileBasicInformation();
            fileInfo.FileAttributes = (uint)fileAttribute;
            var fileInfoBuffer = TypeMarshal.ToBytes(fileInfo);

            var status = smb2Client.SetInfo(
                creditCharge: 1,
                creditRequest: 1,
                flags: defaultFlags,
                messageId: messageId++,
                sessionId: sessionId,
                treeId: treeId,
                infoType: SET_INFO_Request_InfoType_Values.SMB2_0_INFO_FILE,
                fileInfoClass: fileBasicInformationClassValue, // FileBasicInformation
                additionalInfo: SET_INFO_Request_AdditionalInformation_Values.NONE,
                fileId: fileId,
                inputBuffer: fileInfoBuffer,
                out _,
                out _);
            CheckStatusCode(status, nameof(Smb2Client.SetInfo));
        }

        private void Close()
        {
            var status = smb2Client.Close(
                creditCharge: 1,
                creditRequest: 1,
                flags: defaultFlags,
                messageId: messageId++,
                sessionId: sessionId,
                treeId: treeId,
                fileId: fileId,
                closeFlags: Flags_Values.NONE,
                out _,
                out _);
            CheckStatusCode(status, nameof(Smb2Client.Close));

            fileId = default(FILEID);
        }

        private void ConnectToShare()
        {
            smb2Client = new Smb2Client(smb2ClientTimeout);

            if (IPAddress.TryParse(serverName, out var serverIp))
            {
                smb2Client.ConnectOverTCP(serverIp);
            }
            else
            {
                var serverHostEntry = Dns.GetHostEntry(serverName);
                smb2Client.ConnectOverTCP(serverHostEntry.AddressList[0]);
            }

            var validDialects = new DialectRevision[]
            {
                DialectRevision.Smb2002,
                DialectRevision.Smb21,
                DialectRevision.Smb30,
                DialectRevision.Smb302,
                DialectRevision.Smb311
            };

            var preauthIntegrityHashIDs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            var encryptionAlgorithms = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_GCM, EncryptionAlgorithm.ENCRYPTION_AES128_CCM };
            var status = smb2Client.Negotiate(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    // Will negotiate highest dialect server supports  
                    dialects: validDialects,
                    securityMode: SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    capabilities: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_ENCRYPTION | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                    clientGuid: Guid.NewGuid(),
                    out var selectedDialect,
                    out var serverGssToken,
                    out Packet_Header _,
                    out var negotiateResponse,
                    preauthHashAlgs: preauthIntegrityHashIDs,
                    encryptionAlgs: encryptionAlgorithms);
            CheckStatusCode(status, nameof(Smb2Client.Negotiate));

            var sessionSiginingRequired = negotiateResponse.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);
            if (sessionSiginingRequired)
            {
                defaultFlags |= Packet_Header_Flags_Values.FLAGS_SIGNED;
            }

            var usedSecurityPackageType = (SecurityPackageType)Enum.Parse(typeof(SecurityPackageType), securityPackage);
            var sspiClientGss = new SspiClientSecurityContext(
                usedSecurityPackageType,
                new AccountCredential(domainName, userName, password),
                Smb2Utility.GetCifsServicePrincipalName(serverName),
                ClientSecurityContextAttribute.None,
                SecurityTargetDataRepresentation.SecurityNativeDrep);

            if (usedSecurityPackageType == SecurityPackageType.Negotiate && useServerGssToken)
            {
                sspiClientGss.Initialize(serverGssToken);
            }
            else
            {
                sspiClientGss.Initialize(null);
            }

            do
            {
                status = smb2Client.SessionSetup(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: Packet_Header_Flags_Values.NONE,
                    messageId: messageId++,
                    sessionId: sessionId,
                    sessionSetupFlags: SESSION_SETUP_Request_Flags.NONE,
                    securityMode: SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    capabilities: SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                    previousSessionId: 0,
                    clientGssToken: sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out _,
                    out _);
                CheckStatusCode(status, nameof(Smb2Client.SessionSetup));

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            var treeConnectSigningRequired = sessionSiginingRequired || (selectedDialect >= DialectRevision.Smb311);
            smb2Client.GenerateCryptoKeys(
                sessionId,
                sspiClientGss.SessionKey,
                treeConnectSigningRequired,
                false);

            status = smb2Client.TreeConnect(
                creditCharge: 1,
                creditRequest: 1,
                flags: treeConnectSigningRequired ? defaultFlags | Packet_Header_Flags_Values.FLAGS_SIGNED : defaultFlags,
                messageId: messageId++,
                sessionId: sessionId,
                sharePath,
                out treeId,
                out _,
                out _);
            CheckStatusCode(status, nameof(Smb2Client.TreeConnect));

            smb2Client.EnableSessionSigningAndEncryption(sessionId, sessionSiginingRequired, false);
        }

        private void LocateParentDirectory(string filePath)
        {
            var filePathParts = filePath.Split('\\', StringSplitOptions.RemoveEmptyEntries)[..^1];
            var currentPath = string.Empty;
            var existingDirs = new List<string>();
            for (int index = 0; index < filePathParts.Length; index++)
            {
                var currentPart = filePathParts[index];
                currentPath += index != 0 ? "\\" : "";
                currentPath += currentPart;

                if (index != 0)
                {
                    if (!existingDirs.Any(dir => dir == currentPart))
                    {
                        throw new DirectoryNotFoundException($"The directory {sharePath}\\{currentPath} does not exist.");
                    }

                    existingDirs = new List<string>();
                }

                Create(
                    filePath: currentPath,
                    desiredAccess: AccessMask.FILE_LIST_DIRECTORY | AccessMask.FILE_READ_ATTRIBUTES,
                    createOptions: CreateOptions_Values.FILE_DIRECTORY_FILE,
                    createDispositions: CreateDisposition_Values.FILE_OPEN);

                uint status;
                do
                {
                    status = smb2Client.QueryDirectory(
                        creditCharge: 1,
                        creditRequest: 1,
                        flags: defaultFlags,
                        messageId: messageId++,
                        sessionId: sessionId,
                        treeId: treeId,
                        fileInfoClass: FileInformationClass_Values.FileIdBothDirectoryInformation,
                        queryDirectoryFlags: QUERY_DIRECTORY_Request_Flags_Values.NONE,
                        fileIndex: 0,
                        fileId: fileId,
                        fileName: "*", // Query all items.
                        maxOutputBufferLength: queryDirectoryRequestBufferSize,
                        out var outputBuffer,
                        out _,
                        out _);
                    CheckStatusCode(status, nameof(Smb2Client.QueryDirectory));

                    if (outputBuffer != null && outputBuffer.Length > 0)
                    {
                        var fileInfos = UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);
                        var dirNames = fileInfos.Where(info => (info.FileCommonDirectoryInformation.FileAttributes & (uint)File_Attributes.FILE_ATTRIBUTE_DIRECTORY) == (uint)File_Attributes.FILE_ATTRIBUTE_DIRECTORY)
                                                .Select(info => Encoding.Unicode.GetString(info.FileName));
                        existingDirs.AddRange(dirNames);
                    }
                }
                while (status != STATUS_NO_MORE_FILES);

                Close();
            }
        }

        private static IList<T> UnmarshalFileInformationArray<T>(byte[] buffer) where T : struct
        {
            if (buffer == null || buffer.Length <= 0)
            {
                throw new ArgumentException("The input buffer could not be null or empty.");
            }

            IList<T> fileInfos = new List<T>();
            int offset = 0;
            while (offset < buffer.Length)
            {
                var tempBuffer = new byte[buffer.Length - offset];
                Buffer.BlockCopy(buffer, offset, tempBuffer, 0, buffer.Length - offset);
                T fileInfo = TypeMarshal.ToStruct<T>(tempBuffer);
                fileInfos.Add(fileInfo);

                // The first 32-bit of the structure is NextEntryOffset
                var nextEntryOffset = BinaryPrimitives.ReadInt32LittleEndian(buffer.AsSpan(offset));
                if (nextEntryOffset == 0)
                {
                    break; //If there are no subsequent structures, the NextEntryOffset field MUST be 0.
                }
                else
                {
                    offset += nextEntryOffset;
                }
            }
            return fileInfos;
        }

        private void Disconnect()
        {
            if (smb2Client == null || !smb2Client.IsConnected)
            {
                return;
            }

            if (fileId.Persistent != 0 || fileId.Volatile != 0)
            {
                Close();
            }

            if (treeId != 0)
            {
                var status = smb2Client.TreeDisconnect(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    sessionId: sessionId,
                    treeId: treeId,
                    out _,
                    out _);
                CheckStatusCode(status, nameof(Smb2Client.TreeDisconnect));

                treeId = 0;

            }

            if (sessionId != 0)
            {
                var status = smb2Client.LogOff(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    sessionId: sessionId,
                    out _,
                    out _);
                CheckStatusCode(status, nameof(Smb2Client.LogOff));

                sessionId = 0;
            }

            defaultFlags = Packet_Header_Flags_Values.NONE;
            messageId = 0;
            smb2Client.Disconnect();
            smb2Client = null;
        }
    }
}