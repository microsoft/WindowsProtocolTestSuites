// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreateClose;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.CreateClose
{
    public static class CreateCloseModel
    {
        #region States
        /// <summary>
        /// Server configuration related to model
        /// </summary>
        public static CreateCloseConfig Config;

        /// <summary>
        /// The state of the model.
        /// </summary>
        public static ModelState State = ModelState.Uninitialized;

        public static ModelSMB2Request Request = null;

        /// <summary>
        /// Indicates if Open is created by Create Request
        /// </summary>
        public static bool HasOpen;

        #endregion

        #region Rules
        /// <summary>
        /// The call for reading configuration.
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
        }

        /// <summary>
        /// The return for reading configuration.
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(CreateCloseConfig c)
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
            Condition.IsNotNull(c);
            Config = c;
            State = ModelState.Initialized;
            
            // Force SE to expand Config.MaxSmbVersionServerSupported
            Condition.IsTrue(c.MaxSmbVersionServerSupported == ModelDialectRevision.Smb2002 ||
                             c.MaxSmbVersionServerSupported == ModelDialectRevision.Smb21 ||
                             c.MaxSmbVersionServerSupported == ModelDialectRevision.Smb30 ||
                             c.MaxSmbVersionServerSupported == ModelDialectRevision.Smb302);
        }

        /// <summary>
        /// Setup connection by performing following
        ///     1. Negotiate
        ///     2. SessionSetup
        ///     3. TreeConnect
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">Dialect version of client</param>
        [Rule]
        public static void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported)
        {
            Condition.IsTrue(State == ModelState.Initialized);

            // Then maxSmbVersionClientSupported can not be larger than Config.MaxSmbVersionServerSupported (by DetermineNegotiateDialect)
            ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported, Config.MaxSmbVersionServerSupported);

            State = ModelState.Connected;
        }

        /// <summary>
        /// SMB2 Create Request
        /// </summary>
        /// <param name="fileNameType">Indicates the type of the file name</param>
        /// <param name="fileOpenReparsePointType">Indicates if FileOpenReparsePoint is set in CreateOptions field</param>
        /// <param name="fileDeleteOnCloseType">Indicates if FileDeleteOnClose is set in CreateOptions field</param>
        /// <param name="contextType">Indicates the type of CreateContext</param>
        /// <param name="impersonationType">Indicates if impersonation level is valid or not</param>
        /// <param name="fileType">Indicates if the file created is a directory or not</param>
        [Rule]
        public static void CreateRequest(
            CreateFileNameType fileNameType,
            CreateOptionsFileOpenReparsePointType fileOpenReparsePointType,
            CreateOptionsFileDeleteOnCloseType fileDeleteOnCloseType,
            CreateContextType contextType,
            ImpersonationLevelType impersonationType,
            CreateFileType fileType)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);

            Combination.Isolated(fileNameType == CreateFileNameType.StartWithPathSeparator);
            Combination.Isolated(fileNameType == CreateFileNameType.OtherInvalidFileName);

            // Reduce expanded cases
            Combination.Pairwise(fileNameType, fileOpenReparsePointType, fileDeleteOnCloseType, contextType, impersonationType, fileType);

            Combination.Isolated(contextType == CreateContextType.InvalidCreateContext);
            Combination.Isolated(contextType == CreateContextType.InvalidCreateContextSize);
            Combination.Isolated(impersonationType == ImpersonationLevelType.InvalidImpersonationLevel);


            Request = new ModelCreateRequest(fileNameType, fileOpenReparsePointType, fileDeleteOnCloseType, contextType, impersonationType);
        }

        /// <summary>
        /// SMB2 Create Response
        /// </summary>
        /// <param name="status">Status of response</param>
        /// <param name="c">To expand config</param>
        [Rule]
        public static void CreateResponse(ModelSmb2Status status, CreateCloseConfig c)
        {
            Condition.IsTrue(Config.Platform == c.Platform);
            Condition.IsTrue(State == ModelState.Connected);

            ModelCreateRequest createRequest = ModelHelper.RetrieveOutstandingRequest<ModelCreateRequest>(ref Request);

            if (createRequest.NameType == CreateFileNameType.StartWithPathSeparator)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.9: If the file name length is greater than zero and the first character is a path separator character, the server MUST fail the request with STATUS_INVALID_PARAMETER.");
                ModelHelper.Log(LogType.TestInfo, "The first character in the file name of the Create Request is a path separator.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                return;
            }

            if (createRequest.NameType == CreateFileNameType.OtherInvalidFileName)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.9: If the file name fails to conform with the specification of a relative pathname in [MS-FSCC] section 2.1.5, the server MUST fail the request with STATUS_OBJECT_NAME_INVALID.");
                ModelHelper.Log(LogType.TestInfo, "The file name of the Create Request contains illegal character.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_INVALID);
                return;
            }

            if (createRequest.ContextType == CreateContextType.InvalidCreateContext)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.9: The server SHOULD fail any request having a create context not specified in section 2.2.13.2, with a STATUS_INVALID_PARAMETER error.");
                ModelHelper.Log(LogType.TestInfo, "The create context of Create Request is invalid");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);
                if (Config.Platform != Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is Windows");
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                }
                else
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows");
                    Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                }
                return;
            }

            // Create Context Validation: 
            if (createRequest.ContextType == CreateContextType.InvalidCreateContextSize)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.9: If the size of each individual create context is not equal to the DataLength of the create context, the server MUST fail the request with STATUS_INVALID_PARAMETER.");
                ModelHelper.Log(LogType.TestInfo, "The size of the create context in the Create Request is invalid.");
                ModelHelper.Log(LogType.TestTag, TestTag.OutOfBoundary);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                return;
            }

            if (createRequest.ImpersonationType == ImpersonationLevelType.InvalidImpersonationLevel && Config.Platform != Platform.WindowsServer2008)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.9: If the ImpersonationLevel in the request is not one of the values specified in section 2.2.13, the server SHOULD fail the request with STATUS_BAD_IMPERSONATION_LEVEL.");
                ModelHelper.Log(LogType.TestInfo, "The ImpersonationLevel of the Create Request is invalid.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);

                // <236> Section 3.3.5.9: Windows Vista and Windows Server 2008 do not fail the request if the ImpersonationLevel in the request is not one of the values specified in section 2.2.13.
                if (Config.Platform != Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is Windows");
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_BAD_IMPERSONATION_LEVEL);
                }
                else
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows");
                    Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                }
                return;
            }

            ModelHelper.Log(LogType.Requirement, "3.3.5.9: If the open is successful, the server MUST allocate an open object for this open and insert it into Session.OpenTable and GlobalOpenTable. ");
            ModelHelper.Log(LogType.TestInfo, "The Create Request doesn't contain any invalid fields, the open is created successfully and server should return STATUS_SUCCESS");
            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
            HasOpen = true;
        }

        /// <summary>
        /// SMB2 Close Request
        /// </summary>
        /// <param name="closeFlagType">Indicates if SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is set in the Flags field</param>
        /// <param name="volatileType">Indicates if FileId.Volatile is valid or not</param>
        /// <param name="persistentType">Indicates if FileId.Persistent is valid or not</param>
        [Rule]
        public static void CloseRequest(
            CloseFlagType closeFlagType,
            FileIdVolatileType volatileType,
            FileIdPersistentType persistentType)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);

            Combination.Isolated(volatileType == FileIdVolatileType.InvalidFileIdVolatile);
            Combination.Isolated(persistentType == FileIdPersistentType.NotEqualToOpenDurableFileID);

            Request = new ModelCloseRequest(closeFlagType, volatileType, persistentType);
        }

        /// <summary>
        /// SMB2 Close Response
        /// </summary>
        /// <param name="status">Status of response</param>
        /// <param name="queryResponseStatus">Indicates if response of querying the attributes is contained or not</param>
        [Rule]
        public static void CloseResponse(ModelSmb2Status status, QueryResponseStatus queryResponseStatus)
        {
            Condition.IsTrue(State == ModelState.Connected);

            ModelCloseRequest closeRequest = ModelHelper.RetrieveOutstandingRequest<ModelCloseRequest>(ref Request);

            string log = null;

            if (closeRequest.VolatileType == FileIdVolatileType.InvalidFileIdVolatile)
            {
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                log = "FileId.Volatile of the Close Request is invalid, so the server cannot locate the open using FileId.Volatile as the lookup key.";
            }
            else if (closeRequest.PersistentType == FileIdPersistentType.NotEqualToOpenDurableFileID)
            {
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                log = "FileId.Persistent is not equal to Open.DurableFileId.";
            }
            else if (!HasOpen)
            {                
                log = "The Open was not be created or was closed before.";
            }

            if (log != null)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.10: Next, the server MUST locate the open being closed by performing a lookup in the Session.OpenTable, using FileId.Volatile of the request as the lookup key. " +
                    "If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED.");
                ModelHelper.Log(LogType.TestInfo, log);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_CLOSED);
                return;
            }

            ModelHelper.Log(LogType.TestInfo, "The Close Request doesn't contain any invalid fields, the open is closed successfully and server should return STATUS_SUCCESS");
            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
            HasOpen = false;

            if (closeRequest.CloseType == CloseFlagType.PostQueryAttribSet)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.10: If SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is set in the Flags field of the request, the server MUST query the attributes of the file after the close.");
                ModelHelper.Log(LogType.TestInfo, "SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is set in the Flags field of the Close Request.");
                Condition.IsTrue(queryResponseStatus == QueryResponseStatus.QueryResponseExist);
            }
            else
            {
                ModelHelper.Log(LogType.TestInfo, "SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is not set, then the server should not query the attribute of the file after the close");
                Condition.IsTrue(queryResponseStatus == QueryResponseStatus.QueryResponseNotExist);
            }
        }
        #endregion
    }
}
