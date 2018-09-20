// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.CreateClose
{
    [TestClass]
    public class CreateClose : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client1;
        private Smb2FunctionalClient client2;
        private string sharePath;
        private string fileName;
        #endregion

        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Clean up
        protected override void TestInitialize()
        {
            base.TestInitialize();
            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client2 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client2.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
        }

        protected override void TestCleanup()
        {
            if (client1 != null)
            {
                client1.Disconnect();
            }
            if (client2 != null)
            {
                client2.Disconnect();
            }
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.CreateClose)]
        [Description("This case is designed to test whether server can handle Create and Delete operations on file correctly.")]
        public void BVT_CreateClose_CreateAndCloseFile()
        {
            // Client1 create a file
            OperateFileOrDirectory(
                client1, 
                false,   //Not a directory
                false,   //FILE_DELETE_ON_CLOSE flag not set
                false,   //Use Administrator
                FileNameType.NotExistedValidFileName, 
                true); //Valid AccessMask

            //Client2 delete the file created by client1
            OperateFileOrDirectory(
                client2,
                false, //Not a directory
                true,  //FILE_DELETE_ON_CLOSE flag set
                false, //Use Administrator
                FileNameType.ExistedValidFileName,
                true); //Valid AccessMask
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.CreateClose)]
        [Description("This case is designed to test whether server can handle Create and Delete operations on directory correctly.")]
        public void BVT_CreateClose_CreateAndCloseDirectory()
        {
            //Client1 create a directory
            OperateFileOrDirectory(
                client1,
                true,   //A directory
                false,  //FILE_DELETE_ON_CLOSE flag not set
                false,  //Use Administrator
                FileNameType.NotExistedValidFileName,
                true); //Valid AccessMask

            //Client2 delete the directory created by client1
            OperateFileOrDirectory(
                client2,
                true,  //A directory
                true,  //FILE_DELETE_ON_CLOSE flag set
                false, //Use Administrator
                FileNameType.ExistedValidFileName,
                true); //Valid AccessMask
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.CreateClose)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This case is designed to test whether server can handle Create operation with symbolic link in middle of file path.")]
        public void CreateClose_SymbolicLinkInMiddle()
        {
            //Client1 create a file
            OperateFileOrDirectory(
                client1, 
                false,  //Not a directory
                false,  //FILE_DELETE_ON_CLOSE flag not set
                false,  //Use Administrator
                FileNameType.SymbolicLinkInMiddle, 
                true);  //Valid AccessMask
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.CreateClose)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This case is designed to test whether server can handle Create operation with symbolic link at last of file path.")]
        public void CreateClose_SymbolicLinkAtLast()
        {
            //Client1 create a directory
            OperateFileOrDirectory(
                client1,
                true,   //A directory
                false,  //FILE_DELETE_ON_CLOSE flag not set
                false,  //Use Administrator
                FileNameType.SymbolicLinkAtLast,
                true); //Valid AccessMask
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.CreateClose)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This case is designed to test whether server can handle Create operation with other invalid symbolic link.")]
        public void CreateClose_InvalidSymbolicLink()
        {
            //Client1 create a file
            OperateFileOrDirectory(
                client1,
                false, //Not a directory
                false, //FILE_DELETE_ON_CLOSE flag not set
                false, //Use Administrator
                FileNameType.InvalidSymbolicLink,
                true); //Valid AccessMask
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.CreateClose)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This case is designed to test whether server can handle file deletion request when desired access does not include DELETE or GENERIC_ALL.")]
        public void CreateClose_DeleteFile_DesiredAccessNotIncludeDeleteOrGenericAll()
        {
            //Client1 create a file
            OperateFileOrDirectory(
                client1,
                false,  //Not a directory
                true,   //FILE_DELETE_ON_CLOSE flag set
                false,  //Use Administrator
                FileNameType.NotExistedValidFileName,
                false); //Invalid AccessMask
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Create or delete a file or directory with a given name.
        /// </summary>
        /// <param name="client">the functional client used to create</param>
        /// <param name="isDirectory">true for file and false for directory</param>
        /// <param name="isDeleteFlagSet">true for delete flag set</param>
        /// <param name="isNonAdmin">true for non admin account credential</param>
        /// <param name="fileNametype">the file name type: ValidFileName, SymbolicLinkInMiddle, SymbolicLinkAtLast, InvalidSymbolicLink</param>
        /// <param name="isValidAccessMask">true for valid access mask, which should contain DELETE or GENERIC_ALL</param>
        private void OperateFileOrDirectory(Smb2FunctionalClient client, bool isDirectory, bool isDeleteFlagSet, bool isNonAdmin, FileNameType fileNameType, bool isValidAccessMask)
        {
            CreateOptions_Values createOption;
            CreateDisposition_Values createDisposition;
            if (isDirectory)
            {
                createOption = CreateOptions_Values.FILE_DIRECTORY_FILE;
            }
            else
            {
                createOption = CreateOptions_Values.FILE_NON_DIRECTORY_FILE;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT.");
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            AccountCredential accountCredential = isNonAdmin ? TestConfig.NonAdminAccountCredential : TestConfig.AccountCredential;
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, accountCredential, false);
            uint treeId;
            client.TreeConnect(sharePath, out treeId);
            FILEID fileId;
            Smb2CreateContextResponse[] createContextResponse;

            AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE;
            accessMask = isValidAccessMask ? accessMask : AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE;
            // The delete flag is set in the following situations: 1. Delete an existed file; 2. Test CreateOptions_Values.FILE_DELETE_ON_CLOSE combined with DesiredAccess
            createOption = isDeleteFlagSet ? (createOption | CreateOptions_Values.FILE_DELETE_ON_CLOSE) : createOption;
            // The createDisposition is set to FILE_OPEN if the file already existed; else, if it's the first time to create a file, this field should be set to FILE_CREATE
            createDisposition = (fileNameType == FileNameType.ExistedValidFileName) ? CreateDisposition_Values.FILE_OPEN : CreateDisposition_Values.FILE_CREATE;
            fileName = GetFileName(isDirectory, fileNameType);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends CREATE request with create option: {0} and create disposition: {1}", createOption, createDisposition);
            uint status = client.Create(
                treeId,
                fileName,
                createOption,
                out fileId,
                out createContextResponse,
                accessMask: accessMask,
                createDisposition: createDisposition,
                checker: (header, response) =>
                {
                    CheckCreateResponse(isNonAdmin, createOption, accessMask, header, response, fileNameType);
                });

            AddTestFileName(sharePath, fileName);

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF.");
                client.Close(treeId, fileId);
            }

            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        /// <summary>
        /// Get file name
        /// </summary>
        /// <param name="isDirectory">true for directory</param>
        /// <param name="fileNameType">file name type</param>
        /// <returns>The file name</returns>
        private string GetFileName(bool isDirectory, FileNameType fileNameType)
        {
            switch (fileNameType)
            {
                case FileNameType.NotExistedValidFileName:
                    fileName = isDirectory ? "CreateClose_Directory_" + Guid.NewGuid() : "CreateClose_File_" + Guid.NewGuid() + ".txt";
                    break;
                case FileNameType.ExistedValidFileName:
                    break;
                case FileNameType.SymbolicLinkInMiddle:
                    fileName = TestConfig.SymboliclinkInSubFolder + "\\CreateClose" + Guid.NewGuid();
                    break;
                case FileNameType.SymbolicLinkAtLast:
                    fileName = TestConfig.SymboliclinkInSubFolder;
                    break;
                case FileNameType.InvalidSymbolicLink:
                    fileName = TestConfig.Symboliclink + "\\CreateClose" + Guid.NewGuid();
                    break;
                default:
                    throw new ArgumentException("fileNameType");
            }

            return fileName;
        }

        /// <summary>
        /// Check the status code of create response
        /// </summary>
        /// <param name="isNonAdmin">true for non admin credential</param>
        /// <param name="createOption">The create option set in create request</param>
        /// <param name="accessMask">The access mark set in create request</param>
        /// <param name="header">Header of create response</param>
        /// <param name="response">create response</param>
        /// <param name="fileNameType">file name type</param>
        private void CheckCreateResponse(bool isNonAdmin, CreateOptions_Values createOption, AccessMask accessMask, Packet_Header header, CREATE_Response response, FileNameType fileNameType)
        {
            switch (fileNameType)
            {
                case FileNameType.SymbolicLinkInMiddle:
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_STOPPED_ON_SYMLINK,
                            header.Status,
                            "3.3.5.9: If any intermediate component of the path specified in the create request is a symbolic link, " +
                            "the server MUST return an error as specified in section 2.2.2.1. " +
                            "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                        break;
                    }
                case FileNameType.SymbolicLinkAtLast:
                    {
                        if (!createOption.HasFlag(CreateOptions_Values.FILE_OPEN_REPARSE_POINT))
                        {
                            BaseTestSite.Assert.AreEqual(
                                Smb2Status.STATUS_STOPPED_ON_SYMLINK,
                                header.Status,
                                "3.3.5.9: If the final component of the path is a symbolic link, the server behavior depends on whether the flag FILE_OPEN_REPARSE_POINT was specified in the CreateOptions field of the request. " +
                                "If FILE_OPEN_REPARSE_POINT was specified, the server MUST open the underlying file or directory and return a handle to it. " +
                                "Otherwise, the server MUST return an error as specified in section 2.2.2.1. " + 
                                "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                        }
                        break;
                    }
                case FileNameType.InvalidSymbolicLink:
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_STOPPED_ON_SYMLINK,
                            header.Status,
                            "3.3.5.9: If the underlying object store returns a failure indicating that the attempted open operation failed due to the presence of a symbolic link in the target path name, " +
                            "the server MUST fail the create operation with the error code STATUS_STOPPED_ON_SYMLINK. " +
                            "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                        break;
                    }
                case FileNameType.NotExistedValidFileName:
                    {
                        if (createOption.HasFlag(CreateOptions_Values.FILE_DELETE_ON_CLOSE) 
                            && !(accessMask.HasFlag(AccessMask.DELETE) || accessMask.HasFlag(AccessMask.GENERIC_ALL)))
                        {
                            if (testConfig.Platform == Platform.NonWindows)
                            {
                                BaseTestSite.Assert.AreNotEqual(
                                    Smb2Status.STATUS_SUCCESS,
                                    header.Status,
                                    "3.3.5.9: If the FILE_DELETE_ON_CLOSE flag is set in CreateOptions and any of the following conditions is TRUE, the server SHOULD<242> fail the request with STATUS_ACCESS_DENIED. " +
                                    "DesiredAccess does not include DELETE or GENERIC_ALL. " +
                                    "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                            }
                            else if (testConfig.Platform == Platform.WindowsServer2008 
                                || testConfig.Platform == Platform.WindowsServer2008R2)
                            {
                                //TD does not specify the behavior of windows 2008 and 2008R2, not check here
                            } 
                            else if(testConfig.Platform == Platform.WindowsServer2012)
                            {
                                //For platform windows 2012
                                BaseTestSite.Assert.AreEqual(
                                    Smb2Status.STATUS_INVALID_PARAMETER,
                                    header.Status,
                                    "3.3.5.9: If the FILE_DELETE_ON_CLOSE flag is set in CreateOptions and any of the following conditions is TRUE, the server SHOULD<242> fail the request with STATUS_ACCESS_DENIED. " +
                                    "DesiredAccess does not include DELETE or GENERIC_ALL. " +
                                    "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                            }
                            else 
                            {
                                //For platform windows 2012R2 and above
                                BaseTestSite.Assert.AreEqual(
                                    Smb2Status.STATUS_ACCESS_DENIED,
                                    header.Status,
                                    "3.3.5.9: If the FILE_DELETE_ON_CLOSE flag is set in CreateOptions and any of the following conditions is TRUE, the server SHOULD<242> fail the request with STATUS_ACCESS_DENIED. " +
                                    "DesiredAccess does not include DELETE or GENERIC_ALL. " +
                                    "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                            }
                        }
                        else if (createOption.HasFlag(CreateOptions_Values.FILE_DELETE_ON_CLOSE) && isNonAdmin)
                        {
                            //NonAdminAccountCredential does not include DELETE or GENERIC_ALL in MaximalAccess
                            if (testConfig.Platform == Platform.NonWindows)
                            {
                                BaseTestSite.Assert.AreNotEqual(
                                    Smb2Status.STATUS_SUCCESS,
                                    header.Status,
                                    "3.3.5.9: If the FILE_DELETE_ON_CLOSE flag is set in CreateOptions and any of the following conditions is TRUE, the server SHOULD<242> fail the request with STATUS_ACCESS_DENIED. " +
                                    "Treeconnect.MaximalAccess does not include DELETE or GENERIC_ALL. " +
                                    "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                            }
                            else if (testConfig.Platform == Platform.WindowsServer2008
                                || testConfig.Platform == Platform.WindowsServer2008R2)
                            {
                                //TD does not specify te behavior of windows 2008 and 2008R2, not check here
                            }
                            else 
                            {
                                //For platform win2012 and 2012R2
                                BaseTestSite.Assert.AreEqual(
                                    Smb2Status.STATUS_ACCESS_DENIED,
                                    header.Status,
                                    "3.3.5.9: If the FILE_DELETE_ON_CLOSE flag is set in CreateOptions and any of the following conditions is TRUE, the server SHOULD<242> fail the request with STATUS_ACCESS_DENIED. " +
                                    "Treeconnect.MaximalAccess does not include DELETE or GENERIC_ALL. " +
                                    "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                            }
                        }
                        else
                        {
                            BaseTestSite.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "{0} should be successful, actually server returns with {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                        }
                        break;
                    }
                case FileNameType.ExistedValidFileName:
                    {
                        BaseTestSite.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "{0} should be successful, actually server returns with {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    }
                    break;
                default:
                    throw new ArgumentException("fileNameType");
            }
        }
        #endregion
    }
}
