// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using System;
using System.Threading;
using System.Diagnostics;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileAccessTestCases : PtfTestClassBase
    {

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccess)]
        [Description("This test case is designed to verify that server sends STATUS_SUCCESS when a named pipe is opened successfully.")]
        public void BVT_FileAccess_OpenNamedPipe()
        {
            string comment = "If open operation was successful, expect STATUS_SUCCESS";
            OpenNamedPipe((uint)MessageStatus.SUCCESS, comment, TestCondition.NONE);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccess)]
        [Description("This test case is designed to verify that server sends STATUS_OBJECT_NAME_NOT_FOUND when a named pipe open request is sent with invalid path name.")]
        public void BVT_FileAccess_OpenNamedPipe_InvalidPathName()
        {
            string comment = "Search ParentFile.DirectoryList for a Link where Link.Name matches PathName. If no such link is found, the operation MUST be failed with STATUS_OBJECT_NAME_NOT_FOUND.";
            OpenNamedPipe((uint)MessageStatus.OBJECT_NAME_NOT_FOUND, comment, TestCondition.INVALID_PATH);
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccess)]
        [Description("This test case is designed to verify that server sends STATUS_ACCESS_DENIED when a open named pipe request is sent with DesiredAccess as zero.")]
        public void FileAccess_OpenNamedPipe_DesiredAccessZero()
        {
            string comment = "If DesiredAccess is zero, or if any of the bits in the mask 0x0CE0FE00 are set, the operation MUST be failed with STATUS_ACCESS_DENIED.";
            OpenNamedPipe((uint)MessageStatus.ACCESS_DENIED, comment, TestCondition.DESIRED_ACCESS_ZERO);
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccess)]
        [Description("This test case is designed to verify that server sends STATUS_ACCESS_DENIED when a open named pipe request is sent with FILE_READ_DATA while it was created with FILE_PIPE_INBOUND.")]
        public void FileAccess_OpenNamedPipe_Inbound()
        {
            string comment = "If NamedPipeConfiguration of existing file is FILE_PIPE_INBOUND, and Open.GrantedAccess contains FILE_READ_DATA.";
            OpenNamedPipe((uint)MessageStatus.ACCESS_DENIED, comment, TestCondition.PIPE_CONFIG_INBOUND);
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccess)]
        [Description("This test case is designed to verify that server sends STATUS_ACCESS_DENIED when a open named pipe request is sent with FILE_WRITE_DATA while it was created with FILE_PIPE_OUTBOUND.")]
        public void FileAccess_OpenNamedPipe_Outbound()
        {
            string comment = "If NamedPipeConfiguration of existing file is FILE_PIPE_OUTBOUND, and OpenGrantedAccess contains FILE_WRITE_DATA.";
            OpenNamedPipe((uint)MessageStatus.ACCESS_DENIED, comment, TestCondition.PIPE_CONFIG_OUTBOUND);
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccess)]
        [Description("This test case is designed to verify that server sends STATUS_PIPE_NOT_AVAILABLE when a open named pipe request is sent when named pipe has no active listener.")]
        public void FileAccess_OpenNamedPipe_InactiveListener()
        {
            string comment = "The operation MUST be failed with STATUS_PIPE_NOT_AVAILABLE if existing file has no active listeners.";
            OpenNamedPipe((uint)MessageStatus.STATUS_PIPE_NOT_AVAILABLE, comment, TestCondition.INACTIVE_LISTENER);
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccess)]
        [Description("This test case is designed to verify that server sends STATUS_SUCCESS when a named pipe is opened using different path name case.")]
        public void FileAccess_OpenNamedPipe_IsCaseInsensitive()
        {
            //MS - SMB2 < 296 > Section 3.3.5.9: Windows performs the following open / create mappings from SMB2 parameters
            //to the object store as described in [MS-FSA] section 2.1.5.1 Server Requests an Open of a File.
            //IsCaseInsensitive | TRUE | Windows - based SMB2 servers always handle path names as case -insensitive.
            string comment = "The object store MUST search for a filename matching Open.FileName. If IsCaseInsensitive is TRUE, " +
                "then the search MUST be case-insensitive; otherwise, it MUST be case-sensitive.";
            OpenNamedPipe((uint)MessageStatus.SUCCESS, comment, TestCondition.IS_CASE_INSENSITIVE);
        }

        public void OpenNamedPipe(uint expectedStatus, string comment, TestCondition condition)
        {
            //Establish connection with server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Initiate SMB2 tree connection with SHARE_TYPE_PIPE");

            Smb2FunctionalClient client = new Smb2FunctionalClient(this.fsaAdapter.TestConfig.Timeout, this.fsaAdapter.TestConfig, BaseTestSite);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client to create a file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            client.ConnectToServer(this.fsaAdapter.TestConfig.UnderlyingTransport, this.fsaAdapter.TestConfig.SutComputerName, this.fsaAdapter.TestConfig.SutIPAddress);
            client.Negotiate(this.fsaAdapter.TestConfig.RequestDialects, this.fsaAdapter.TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(
                this.fsaAdapter.TestConfig.DefaultSecurityPackage,
                this.fsaAdapter.TestConfig.SutComputerName,
                this.fsaAdapter.TestConfig.AccountCredential,
                this.fsaAdapter.TestConfig.UseServerGssToken);

            //Tree connect with share type as PIPE
            uint treeId;
            string sutComputerName= this.fsaAdapter.TestConfig.SutComputerName;
            string uncSharePath = "\\\\" + sutComputerName + "\\IPC$";
            client.TreeConnect(uncSharePath, out treeId,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        ShareType_Values.SHARE_TYPE_PIPE,
                        response.ShareType,
                        "Share type should be SMB2_SHARE_TYPE_PIPE, actually server returns {0}", response.ShareType);
                });

            //Create a named pipe
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Open a named pipe");
            FILEID fileId;
            string fileName = "PIPE"+this.fsaAdapter.ComposeRandomFileName(8);
            string pipeName = fileName;
            PipeDirection pipeDirection = PipeDirection.InOut;

            CreateOptions_Values createOptions = CreateOptions_Values.FILE_NON_DIRECTORY_FILE;
            AccessMask requestedAccess = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE;
            File_Attributes fileAttribute = File_Attributes.NONE;
            if (condition == TestCondition.INVALID_PATH)
            {
                pipeName = "InvalidName";
            }
            else if (condition == TestCondition.DESIRED_ACCESS_ZERO)
            {
                requestedAccess = 0;
            }
            else if (condition == TestCondition.PIPE_CONFIG_INBOUND)
            {
                requestedAccess = AccessMask.GENERIC_READ;
                pipeDirection = PipeDirection.In;
            }
            else if (condition == TestCondition.PIPE_CONFIG_OUTBOUND)
            {
                pipeDirection = PipeDirection.Out;
                requestedAccess = AccessMask.GENERIC_WRITE;
            }
            else if (condition == TestCondition.IS_CASE_INSENSITIVE)
            {
                pipeName = pipeName.ToLower();
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Lower case: " + pipeName);
            }

            Process process = new();
            try
            {
                string direction = pipeDirection == PipeDirection.In ? "In" 
                    : pipeDirection == PipeDirection.Out ? "Out" : "InOut";
                process = this.fsaAdapter.CreateNamedPipeAsync(pipeName, direction);
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                BaseTestSite.Assert.Fail("Create Named Pipe failed: "+ex.Message);
            }

            //Open a named pipe
            uint status, inactiveListernerStatus=5;

            if (condition == TestCondition.INACTIVE_LISTENER)
            {
                inactiveListernerStatus = client.Create(
                treeId,
                fileName,
                createOptions,
                out fileId,
                out _,
                checker: (header, response) => { },
                shareAccess: ShareAccess_Values.FILE_SHARE_READ,
                accessMask: requestedAccess,
                fileAttributes: fileAttribute,
                createDisposition: CreateDisposition_Values.FILE_CREATE);
            }

            status = client.Create(
            treeId,
            fileName,
            createOptions,
            out fileId,
            out _,
            checker: (header, response) => { },
            shareAccess: ShareAccess_Values.FILE_SHARE_READ,
            accessMask: requestedAccess,
            fileAttributes: fileAttribute,
            createDisposition: CreateDisposition_Values.FILE_CREATE);

            //Create named pipe asserts            
            string error = process.StandardError.ReadToEnd();
            BaseTestSite.Assert.IsTrue(string.IsNullOrEmpty(error), "Create Named Pipe must complete without error:"+error?.ToString());

            //Open named pipe asserts 
            if (condition == TestCondition.INACTIVE_LISTENER)
            {
                BaseTestSite.Assert.AreEqual(
               (uint)MessageStatus.SUCCESS,
                inactiveListernerStatus,
               "Initial open must succeed, so that named pipe would have no available instance for other connections.");
            }

            BaseTestSite.Assert.AreEqual(expectedStatus, status, comment);

            //Disconnect connection
            if (status == Smb2Status.STATUS_SUCCESS)
            {
                client.Close(treeId, fileId);
            }

            client.TreeDisconnect(treeId);
            client.LogOff();

        }

        public enum TestCondition : uint
        {
            NONE,
            INVALID_PATH,
            DATA_FILE,
            DESIRED_ACCESS_ZERO,
            PIPE_CONFIG_INBOUND,
            PIPE_CONFIG_OUTBOUND,
            INACTIVE_LISTENER,
            IS_CASE_INSENSITIVE,
            REPARSE_POINT
        }
    }

}