// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;

namespace Microsoft.Protocols.TestSuites.FileSharing.SQOS.TestSuite
{
    [TestClass]
    public class SqosNegative : SqosTestBase
    {
        public enum VariableType
        {
            InitiatorName,
            InitiatorNodeName
        }

        public enum InvalidOffsetType
        {
            Small,
            Large
        }

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

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is to test if server can handle an SQOS request with an invalid protocol version correctly")]
        public void Sqos_InvalidProtocolVersion()
        {
            ConnectToVHD();
            SqosResponsePacket sqosResponse;
            ushort invalidProtocolVersion = 0xFFFF;
            SqosRequestPacket sqosRequest = new SqosRequestPacket(TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
                invalidProtocolVersion,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_LOGICAL_FLOW_ID,
                Guid.NewGuid(),
                TestConfig.SqosPolicyId,
                Guid.NewGuid(),
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends an SQOS request with an invalid protocol version ({0}) and expects STATUS_REVISION_MISMATCH",
                invalidProtocolVersion);
            uint status = client.SendAndReceiveSqosPacket(
                sqosRequest,
                out sqosResponse);
            BaseTestSite.Assert.AreEqual(
                (uint)NtStatus.STATUS_REVISION_MISMATCH,
                status,
                "3.2.5.1: If Request.ProtocolVersion does not equal 0x0100, the server MUST fail the request with error STATUS_REVISION_MISMATCH.");
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is to test if server can handle an SQOS request with an invalid Option correctly")]
        public void Sqos_InvalidOption()
        {
            ConnectToVHD();
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends an SQOS request with an invalid option (0) and expects STATUS_INVALID_PARAMETER");
            SqosResponsePacket sqosResponse;
            SqosRequestPacket sqosRequest = new SqosRequestPacket(TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
                (ushort)TestConfig.SqosClientDialect,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_NONE, // Invalid Option
                Guid.NewGuid(),
                TestConfig.SqosPolicyId,
                Guid.NewGuid(),
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName);

            uint status = client.SendAndReceiveSqosPacket(
                sqosRequest,
                out sqosResponse);

            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_INVALID_PARAMETER,
                status,
                "3.2.5.1: If Request.Options does not include at least one of the flags defined in section 2.2.2.2, " +
                "the server MUST fail the request with error STATUS_INVALID_PARAMETER.");
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is to test if server can handle an SQOS request with an invalid policy id correctly")]
        public void Sqos_InvalidPolicyId()
        {
            ConnectToVHD();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request with an invalid policy Id and expects STATUS_INVALID_PARAMETER");
            // Section 3.2.5.1.2
            // The server MUST fail the request with error STATUS_INVALID_PARAMETER if it determines that any of the following fields has an invalid value<4>:
            //	Request.PolicyID

            // According to the footnote<4>, Windows Server vNext fails the request with STATUS_INVALID_PARAMETER 
            // if Request.Limit is greater than 0 and Request.PolicyID is not equal to a NULL GUID
            Guid invalidPolicyId = Guid.NewGuid(); // Set Request.PolicyID to be a non-zero but invalid Guid.
            SqosResponsePacket sqosResponse;
            SqosRequestPacket sqosRequest = new SqosRequestPacket(TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
                (ushort)TestConfig.SqosClientDialect,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_PROBE_POLICY,
                Guid.NewGuid(),
                invalidPolicyId,
                Guid.NewGuid(),
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName,
                1); // Set Request.Limit to be greater than 0

            uint status = client.SendAndReceiveSqosPacket(
                sqosRequest,
                out sqosResponse);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                status,
                "Server should return STATUS_INVALID_PARAMETER when Request.Limit is greater than 0 and Request.PolicyID is invalid and not equal to a NULL GUID");
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is to test if server can handle an SQOS request correctly when Request.Reservation is greater than Request.Limit")]
        public void Sqos_ReservationGreaterThanLimit()
        {
            ConnectToVHD();
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends an SQOS request when Request.Reservation is greater than Request.Limit and expects STATUS_INVALID_PARAMETER");
            SqosResponsePacket sqosResponse;
            // Section 3.2.5.1.2
            // The server MUST fail the request with error STATUS_INVALID_PARAMETER if it determines that any of the following fields has an invalid value<4>:
            //	Request.Limit
            //	Request.Reservation

            SqosRequestPacket sqosRequest = new SqosRequestPacket(TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
                (ushort)TestConfig.SqosClientDialect,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_PROBE_POLICY,
                Guid.NewGuid(),
                TestConfig.SqosPolicyId,
                Guid.NewGuid(),
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName,
                // According to the footnote<4>, Windows Server vNext fails the request with STATUS_INVALID_PARAMETER 
                // if Request.Limit is greater than 0 and Request.Reservation is greater than Request.Limit
                1,  // Set Request.Limit to be greater than 0
                2); // Set Request.Reservation is greater than Request.Limit

            uint status = client.SendAndReceiveSqosPacket(
                sqosRequest,
                out sqosResponse);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                status,
                "Server should return STATUS_INVALID_PARAMETER when Request.Limit is greater than 0 and Request.Reservation is greater than Request.Limit");
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is to test if server can handle an SQOS request with an invalid small InitiatorNameOffset correctly")]
        public void Sqos_InvalidInitiatorNameOffset_Small()
        {
            InvalidOffset(VariableType.InitiatorName, InvalidOffsetType.Small);
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is to test if server can handle an SQOS request with an invalid large InitiatorNameOffset correctly")]
        public void Sqos_InvalidInitiatorNameOffset_Large()
        {
            InvalidOffset(VariableType.InitiatorName, InvalidOffsetType.Large);
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is to test if server can handle an SQOS request with an invalid small InitiatorNodeNameOffset correctly")]
        public void Sqos_InvalidInitiatorNodeNameOffset_Small()
        {
            InvalidOffset(VariableType.InitiatorNodeName, InvalidOffsetType.Small);
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is to test if server can handle an SQOS request with an invalid large InitiatorNodeNameOffset correctly")]
        public void Sqos_InvalidInitiatorNodeNameOffset_Large()
        {
            InvalidOffset(VariableType.InitiatorNodeName, InvalidOffsetType.Large);
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("This test case is to test if server can handle an SQOS request with SET_POLICY option to an open which is not associated to a logical flow")]
        public void Sqos_SetPolicyToNonAssociatedLogicalFlow()
        {
            ConnectToVHD();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request to set policy, but the open is not associated to a logical flow");
            SqosResponsePacket sqosResponse;
            SqosRequestPacket sqosRequest = new SqosRequestPacket(TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
                (ushort)TestConfig.SqosClientDialect,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_POLICY,
                Guid.NewGuid(),
                TestConfig.SqosPolicyId,
                Guid.NewGuid(),
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName);

            uint status = client.SendAndReceiveSqosPacket(
                sqosRequest,
                out sqosResponse);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_NOT_FOUND,
                status,
                "3.2.5.1.2: If Request.Options includes the STORAGE_QOS_CONTROL_FLAG_SET_POLICY and the Open is not associated to a logical flow, " +
                "the server MUST fail the request with error STATUS_NOT_FOUND");
        }

        [TestMethod]
        [TestCategory(TestCategories.Sqos)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("This test case is to test if server can handle an SQOS version 1.0 request with ProtocolVersion field is set to 1.1")]
        public void Sqos_InvalidRequestType()
        {
            ConnectToVHD();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request to set policy, uses a version 1.0 request structure, but sets ProtocolVersion field to 1.1");
            SqosResponsePacket sqosResponse;
            SqosRequestPacket sqosRequest = new SqosRequestPacket(SqosRequestType.V10,
                (ushort)SQOS_PROTOCOL_VERSION.Sqos11,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_POLICY,
                Guid.NewGuid(),
                TestConfig.SqosPolicyId,
                Guid.NewGuid(),
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName);

            uint status = client.SendAndReceiveSqosPacket(
                sqosRequest,
                out sqosResponse);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                status,
                "The server should return STATUS_INVALID_PARAMETER");
        }

        private void InvalidOffset(VariableType variableType, InvalidOffsetType offsetType)
        {
            ConnectToVHD();
            SqosResponsePacket sqosResponse;
            SqosRequestPacket sqosRequest = new SqosRequestPacket(TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
                (ushort)TestConfig.SqosClientDialect,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_PROBE_POLICY,
                Guid.NewGuid(),
                TestConfig.SqosPolicyId,
                Guid.NewGuid(),
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName);
            ushort invalidOffset = 0;
            int nameLength = variableType == VariableType.InitiatorName ? TestConfig.SqosInitiatorName.Length : TestConfig.SqosInitiatorNodeName.Length;
            int requestSize = sqosRequest.ToBytes().Length;
            if (offsetType == InvalidOffsetType.Large)
            {
                // Set the offset to make the sum of Length and offset be greater than RequestSize. 
                invalidOffset = (ushort)(requestSize - nameLength + 1);
            }

            if (variableType == VariableType.InitiatorName)
            {
                sqosRequest.InitiatorNameOffset = invalidOffset;
            }
            else
            {
                sqosRequest.InitiatorNodeNameOffset = invalidOffset;
            }

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends an SQOS request with {0}Offset set to {1} and expects STATUS_INVALID_PARAMETER",
                variableType,
                invalidOffset);
            uint status = client.SendAndReceiveSqosPacket(
                sqosRequest,
                out sqosResponse);
            string failReason = offsetType == InvalidOffsetType.Small ?
                String.Format(
                    "if Request.{0}Length:{1} is greater than 0 and Request.{0}Offset:{2} is less than 104.", variableType, nameLength, invalidOffset) :
                String.Format(
                    "if (Request.{0}Length:{1} + Request.{0}Offset:{2}) is greater than RequestSize:{3}.", variableType, nameLength, invalidOffset, requestSize);

            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_INVALID_PARAMETER,
                status,
                "3.2.5.1.2: The server MUST fail the request with error STATUS_INVALID_PARAMETER " +
                failReason);
        }
    }
}
