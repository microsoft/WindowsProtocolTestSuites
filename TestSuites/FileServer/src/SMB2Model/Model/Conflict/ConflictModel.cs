// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Conflict;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.Conflict
{
    /// <summary>
    /// This model is designed to test the conflicting scenario: 
    /// Two clients do file operations to the same file by connecting to two Nodes of the scaleout file server.
    /// The file operations may have conflict.
    /// 1. Prepare a test file and initialize two clients. 
    /// 2. The first client does some file operation from one node to the test file successfully.
    /// 3. The second client does some file operation from another node to the same test file.
    /// 4. Verify the response to step 3.
    /// </summary>
    public static class ConflictModel
    {
        #region States
        public static FileState State = FileState.Initial;
        public static RequestType SecondRequest;
        #endregion

        #region Rules
        /// <summary>
        /// 1. Prepare the test file
        /// The two clients connects to the two Nodes of the scaleout file server, including: Negotiate, SessionSetup, TreeConnect
        /// </summary>
        [Rule]
        public static void Preparation()
        {
            Condition.IsTrue(State == FileState.Initial);
        }

        /// <summary>
        /// Conflict requests sent from the two clients
        /// </summary>
        /// <param name="firstRequest">Type of the specified request from the first client</param>
        /// <param name="secondRequest">Type of the specified request from the second client</param> 
        [Rule]
        public static void ConflictRequest(RequestType firstRequest, RequestType secondRequest)
        {
            // For RequestType.Lease, only containg one request (Create with Lease context)
            // for the second client, so DeleteAfter is not applicable.
            Condition.IfThen(firstRequest == RequestType.UncommitedDelete, secondRequest != RequestType.Lease);

            // DeleteAfter is the same as Delete for second request
            Condition.IsTrue(secondRequest != RequestType.UncommitedDelete);
            switch (firstRequest)
            {
                case RequestType.ExclusiveLock:
                    State = FileState.Locked;
                    break;
                case RequestType.Lease:
                    State = FileState.LeaseGranted;
                    break;
                case RequestType.UncommitedDelete:
                    State = FileState.ToBeDeleted;
                    break;
                case RequestType.Delete:
                    State = FileState.Deleted;
                    break;
                // No state changed
                case RequestType.Write:
                case RequestType.Read:
                default:
                    break;
            }

            SecondRequest = secondRequest;
        }

        /// <summary>
        /// Response to the conflicting request 
        /// </summary>
        /// <param name="status">SMB2 status in SMB2 header</param>
        /// <param name="leaseBreakState">Indicates if lease break notification is received</param>
        [Rule]
        public static void ConflictResponse(ModelSmb2Status status, LeaseBreakState leaseBreakState)
        {
            switch (State)
            {
                case FileState.Initial:
                    ModelHelper.Log(LogType.TestInfo, "The state of the file is not changed. So any request from the second client should succeed.");
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                    break;
                case FileState.Locked:
                    switch (SecondRequest)
                    {
                        case RequestType.ExclusiveLock:
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.14.2: If the range being locked is already locked by another open in a way " +
                                "that does not allow this open to take a lock on the range, and if SMB2_LOCKFLAG_FAIL_IMMEDIATELY is set, " +
                                "the server MUST fail the request with STATUS_LOCK_NOT_GRANTED.");
                            ModelHelper.Log(LogType.TestInfo, "The file is already locked and SMB2_LOCKFLAG_FAIL_IMMEDIATELY is set.");
                            ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                            Condition.IsTrue(status == ModelSmb2Status.STATUS_LOCK_NOT_GRANTED);
                            break;
                        case RequestType.Lease:
                            ModelHelper.Log(LogType.TestInfo, "Lease succeed even the file is locked.");
                            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                            break;
                        case RequestType.Delete:
                            ModelHelper.Log(LogType.TestInfo, "Delete succeed even the file is locked.");
                            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                            break;
                        case RequestType.Write:
                            ModelHelper.Log(LogType.TestInfo, "Write fails because the file is locked.");
                            ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                            Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_LOCK_CONFLICT);
                            break;
                        case RequestType.Read:
                            ModelHelper.Log(LogType.TestInfo, "Read fails because the file is locked.");
                            ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                            Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_LOCK_CONFLICT);
                            break;
                        default:
                            break;
                    }
                    break;
                case FileState.LeaseGranted:
                    ModelHelper.Log(
                        LogType.TestInfo, 
                        "A lease to this file is granted to the first open, but it will not fail the operation from the second client");
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                    break;
                case FileState.ToBeDeleted:
                    ModelHelper.Log(LogType.TestInfo, "The file is not deleted, and it will be deleted only if the second open is closed.");
                    ModelHelper.Log(LogType.TestInfo, "So the other operation ahead of Close request from the second client will succeed.");
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                    break;
                case FileState.Deleted:
                    ModelHelper.Log(LogType.TestInfo, "The file is deleted, so any operation to the non-existed file fails.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    break;
                default:
                    break;
            }

            if (State == FileState.LeaseGranted
                && (SecondRequest == RequestType.Write || SecondRequest == RequestType.ExclusiveLock))
            {
                ModelHelper.Log(LogType.TestInfo, "A lease to this file is granted to the first open. ");
                ModelHelper.Log(LogType.TestInfo, "The {0} request from second client should break the lease state.", SecondRequest);
                Condition.IsTrue(leaseBreakState == LeaseBreakState.LeaseBreakExisted);
            }
            else
            {
                if (State == FileState.LeaseGranted)
                {
                    ModelHelper.Log(
                        LogType.TestInfo, 
                        "A lease is granted to the first open, but the {0} request from the second client cannot break the state.", SecondRequest);
                }
                else
                {
                    ModelHelper.Log(LogType.TestInfo, "No lease is granted to the first open, so no lease break.");
                }
                Condition.IsTrue(leaseBreakState == LeaseBreakState.NoLeaseBreak);
            }
        }

        #endregion
    }
}
