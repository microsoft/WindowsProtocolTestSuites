// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model
{
    /// <summary>
    /// Type of Log
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// The log is copied from TD
        /// </summary>
        Requirement,

        /// <summary>
        /// The log describes the real info of the case
        /// </summary>
        TestInfo,

        /// <summary>
        /// The log adds a tag into the generated test case for test case classification
        /// </summary>
        TestTag
    }

    public static class ModelHelper
    {
        /// <summary>
        /// Determine negotiate dialect according to server and client dialect
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">Max SMB version which the client supports.</param>
        /// <param name="maxSmbVersionServerSupported">Max SMB version which the server supports.</param>
        /// <returns>Negotiated dialect</returns>
        public static DialectRevision DetermineNegotiateDialect(ModelDialectRevision maxSmbVersionClientSupported, ModelDialectRevision maxSmbVersionServerSupported)
        {
            Condition.IsTrue(maxSmbVersionClientSupported <= maxSmbVersionServerSupported);

            return (DialectRevision)(uint)maxSmbVersionClientSupported;
        }

        /// <summary>
        /// Output model state information
        /// </summary>
        /// <param name="state">state information</param>
        public static void OutputModelStateInfo(string state)
        {
            Requirement.Capture(state);
        }

        /// <summary>
        /// Add logs for Model
        /// </summary>
        /// <param name="logtype">Type of log, the log has different prefixes according to different log types</param>
        /// <param name="log">The content of log</param>
        /// <param name="args">Arguments in log content</param>
        public static void Log(LogType logtype, string log, params object[] args)
        {
            switch (logtype)
            {
                case LogType.Requirement:
                    Requirement.Capture(string.Format("\"[MS-SMB2] {0}\"", string.Format(log, args)));
                    break;
                case LogType.TestInfo:
                    Requirement.Capture(string.Format("\"[TestInfo] {0}\"", string.Format(log, args)));
                    break;
                case LogType.TestTag:
                    Requirement.Capture(string.Format("\"[TestTag] {0}\"", string.Format(log, args)));
                    break;
            }
        }

        /// <summary>
        /// Retrieve request as a specific type and reset input request.
        /// 
        /// In this method, there is Condition: whether the request can be retrieve as a specific type.
        /// </summary>
        /// <typeparam name="T">Specific type inherited from ModelSMB2Request</typeparam>
        /// <param name="request">SMB2 request</param>
        /// <returns>Request with specific type</returns>
        public static T RetrieveOutstandingRequest<T>(ref ModelSMB2Request request) where T : ModelSMB2Request
        {
            T t = request as T;
            Condition.IsTrue(t != null);
            request = null;

            return t;
        }

        /// <summary>
        /// Check must error.
        /// </summary>
        /// <param name="status">The error code returned by SUT.</param>
        /// <param name="expectedErrorCode">Specified error code according to TD.</param>
        /// <param name="checkMustError">Indicates whether to check that the error code returned by server matches MUST error code in TD strictly.</param>
        public static void CheckMustError(ModelSmb2Status status, ModelSmb2Status expectedErrorCode, MustErrorCheckLevel checkMustError)
        {
            if (checkMustError == MustErrorCheckLevel.CheckMustError)
            {
                Condition.IsTrue(status == expectedErrorCode);
            }
            else
            {
                Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
            }
        }
    }

    public static class TestTag
    {
        /// <summary>
        /// Unexpected field values according to technical document.
        /// </summary>
        public const string UnexpectedFields = "UnexpectedFields";

        /// <summary>
        /// Invalid SessionId, TreeId, FileId and other identifiers according to technical document.
        /// </summary>
        public const string InvalidIdentifier = "InvalidIdentifier";

        /// <summary>
        /// Specific data length is out of boundary according to technical document.
        /// </summary>
        public const string OutOfBoundary = "OutOfBoundary";

        /// <summary>
        /// To evaluate SMB2 server's compatibility according to technical document.
        /// SMB2 server should be able to handle below scenarios gracefully:
        /// (1) A specific feature is not implemented/supported, server should response accordingly without crash.
        /// (2) Specific operation is not allowed according to previous operations, server will response with correct error code.
        /// (3) For a request with complex field combinations, server will handle it gracefully.
        /// (4) For detrimental actions, server will terminate SMB2 connection without crash.
        /// </summary>
        public const string Compatibility = "Compatibility";

        /// <summary>
        /// Unexpected create context, negotiate context and etc. according to technical document.
        /// </summary>
        public const string UnexpectedContext = "UnexpectedContext";
    }
}
