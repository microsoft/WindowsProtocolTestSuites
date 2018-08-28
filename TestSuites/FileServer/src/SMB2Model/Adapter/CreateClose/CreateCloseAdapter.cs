// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreateClose
{
    public class CreateCloseAdapter : ModelManagedAdapterBase, ICreateCloseAdapter
    {
        #region Fields
        private CreateCloseConfig createCloseConfig;
        private Smb2FunctionalClient testClient;
        private uint treeId;
        private FILEID fileID;
        private CreateContextType Create_ContextType;
        public const int mask = 0x0ce0fe00;

        #endregion

        #region Events
        public event CreateResponseEventHandler CreateResponse;
        public event CloseResponseEventHandler CloseResponse;
        #endregion

        #region Initialization & Reset
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            if (testClient != null)
            {
                testClient.Disconnect();
                testClient = null;
            }

            base.Reset();
        }
        #endregion

        #region Actions
        public void ReadConfig(out CreateCloseConfig c)
        {
            c = new CreateCloseConfig
            {
                MaxSmbVersionServerSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                Platform = testConfig.Platform
            };

            createCloseConfig = c;

            Site.Log.Add(LogEntryKind.Debug, c.ToString());
        }

        public void SetupConnection(ModelDialectRevision dialect)
        {
            testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClient.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);

            testClient.Negotiate(Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(dialect)), testConfig.IsSMB1NegotiateEnabled);

            testClient.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            string share = testConfig.BasicFileShare;
            testClient.TreeConnect(
                Smb2Utility.GetUncPath(testConfig.SutComputerName, share),
                out treeId);
        }

        public void CreateRequest(
            CreateFileNameType fileNameType,
            CreateOptionsFileOpenReparsePointType fileOpenReparsePointType,
            CreateOptionsFileDeleteOnCloseType fileDeleteOnCloseType,
            CreateContextType contextType,
            ImpersonationLevelType impersonationType,
            CreateFileType fileType)
        {
            #region Header
            Packet_Header_Flags_Values headerFlag = testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

            #endregion

            #region File Name
            string fileName = GetFileName(fileNameType);
            #endregion

            #region CreateOptions
            CreateOptions_Values createOptions = fileType == CreateFileType.DirectoryFile ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE;
            if (fileOpenReparsePointType == CreateOptionsFileOpenReparsePointType.FileOpenReparsePointSet)
                createOptions |= CreateOptions_Values.FILE_OPEN_REPARSE_POINT;
            if (fileDeleteOnCloseType == CreateOptionsFileDeleteOnCloseType.FileDeteteOnCloseSet)
                createOptions |= CreateOptions_Values.FILE_DELETE_ON_CLOSE;
            #endregion

            #region CreateContexts
            Smb2CreateContextRequest[] createContexts = new Smb2CreateContextRequest[] { };
            Create_ContextType = contextType;
            switch (contextType)
            {
                case CreateContextType.NoCreateContext:
                    break;
                case CreateContextType.InvalidCreateContext:
                case CreateContextType.InvalidCreateContextSize:
                    testClient.BeforeSendingPacket(ReplacePacketByInvalidCreateContext);
                    break;
                case CreateContextType.ValidCreateContext:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_QUERY_ON_DISK_ID);

                    createContexts = createContexts.Append(new Smb2CreateQueryOnDiskId());
                    break;

                default:
                    throw new ArgumentException("contextType");
            }
            #endregion

            #region ImpersonationLevel
            ImpersonationLevel_Values impersonation = ImpersonationLevel_Values.Impersonation;
            if (impersonationType == ImpersonationLevelType.InvalidImpersonationLevel)
            {
                impersonation = (ImpersonationLevel_Values)0x00000004; //Non-existed impersonation level
            }

            #endregion

            #region DesiredAccess
            AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE;
            #endregion

            Smb2CreateContextResponse[] contextResponse;

            uint status = testClient.Create(
                treeId,
                fileName,
                createOptions,
                headerFlag,
                out fileID,
                out contextResponse,
                createContexts: createContexts,
                accessMask: accessMask,
                checker: (header, response) => { },
                impersonationLevel: impersonation);

            AddTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare), fileName);
            CreateResponse((ModelSmb2Status)status, createCloseConfig);
        }

        public void CloseRequest(
            CloseFlagType closeFlagType,
            FileIdVolatileType volatileType,
            FileIdPersistentType persistentType)
        {
            Flags_Values flags = (closeFlagType == CloseFlagType.PostQueryAttribSet) ? Flags_Values.CLOSE_FLAG_POSTQUERY_ATTRIB : Flags_Values.NONE;

            FILEID file;
            file.Persistent = (persistentType == FileIdPersistentType.EqualToOpenDurableFileID) ? fileID.Persistent : (fileID.Persistent - 1);
            file.Volatile = (volatileType == FileIdVolatileType.ValidFileIdVolatile) ? fileID.Volatile : 0;

            QueryResponseStatus queryResponse = QueryResponseStatus.QueryResponseNotExist;
            uint status = testClient.Close(
                treeId,
                file,
                (header, response) =>
                {
                    if (response.FileAttributes != File_Attributes.NONE)
                        queryResponse = QueryResponseStatus.QueryResponseExist;
                },
                flags);

            CloseResponse((ModelSmb2Status)status, queryResponse);
        }

        #endregion

        #region helper functions
        private string GetFileName(CreateFileNameType fileNameType)
        {
            string fileName;
            switch (fileNameType)
            {
                case CreateFileNameType.StartWithPathSeparator:
                    fileName = testConfig.PathSeparator + this.CurrentTestCaseName + "_" + Guid.NewGuid();
                    break;
                case CreateFileNameType.OtherInvalidFileName:
                    // [MS-FSCC] Section 2.1.5.2   Filename
                    // All Unicode characters are legal in a filename except the following:
                    // The characters
                    // " \ / : | < > * ?
                    // Control characters, ranging from 0x00 through 0x1F.
                    fileName = "*" + "CreateClose" + this.CurrentTestCaseName + "_" + Guid.NewGuid();
                    break;
                case CreateFileNameType.ValidFileName:
                    fileName = this.CurrentTestCaseName + "_" + Guid.NewGuid();
                    break;
                default:
                    throw new ArgumentException("fileNameType");
            }

            return fileName;
        }

        private void ReplacePacketByInvalidCreateContext(Smb2Packet packet)
        {
            Smb2CreateRequestPacket request = packet as Smb2CreateRequestPacket;
            if (request == null)
                return;

            string name = null;
            byte[] dataBuffer = null;
            if (Create_ContextType == CreateContextType.InvalidCreateContext)
            {
                // <235> Section 3.3.5.9: Windows Vista, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 ignore create contexts having a NameLength 
                // greater than 4 and ignores create contexts with length of 4 that are not specified in section 2.2.13.2.
                // So use three characters' name here
                name = "Inv";
            }
            else if (Create_ContextType == CreateContextType.InvalidCreateContextSize)
            {
                // Use SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST to test InvalidCreateContextSize since it contains a data buffer.
                name = CreateContextNames.SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST;
                var createQueryMaximalAccessRequestStruct = new CREATE_QUERY_MAXIMAL_ACCESS_REQUEST
                {
                    Timestamp = new _FILETIME()
                };
                dataBuffer = TypeMarshal.ToBytes(createQueryMaximalAccessRequestStruct);
            }
            else
            {
                throw new ArgumentException("contextType");
            }

            var nameBuffer = Encoding.ASCII.GetBytes(name);

            var createContextStruct = new CREATE_CONTEXT();
            createContextStruct.Buffer = new byte[0];

            createContextStruct.NameOffset = 16;
            createContextStruct.NameLength = (ushort)nameBuffer.Length;
            createContextStruct.Buffer = createContextStruct.Buffer.Concat(nameBuffer).ToArray();

            if (dataBuffer != null && dataBuffer.Length > 0)
            {
                Smb2Utility.Align8(ref createContextStruct.Buffer);
                createContextStruct.DataOffset = (ushort)(16 + createContextStruct.Buffer.Length);
                createContextStruct.DataLength = (uint)dataBuffer.Length;
                createContextStruct.Buffer = createContextStruct.Buffer.Concat(dataBuffer).ToArray();
            }

            byte[] createContextValuesBuffer = new byte[0];
            Smb2Utility.Align8(ref createContextValuesBuffer);
            createContextValuesBuffer = createContextValuesBuffer.Concat(TypeMarshal.ToBytes(createContextStruct)).ToArray();

            if (Create_ContextType == CreateContextType.InvalidCreateContextSize)
            {
                // Change DataLength to invalid here, after marshalling.
                createContextValuesBuffer[12] += 1;
            }

            Smb2Utility.Align8(ref request.Buffer);
            request.PayLoad.CreateContextsOffset = (uint)(request.BufferOffset + request.Buffer.Length);
            request.PayLoad.CreateContextsLength = (uint)createContextValuesBuffer.Length;
            request.Buffer = request.Buffer.Concat(createContextValuesBuffer).ToArray();
        }
        #endregion

    }
}
