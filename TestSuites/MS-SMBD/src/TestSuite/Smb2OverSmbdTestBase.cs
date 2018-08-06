// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class Smb2OverSmbdTestBase : SmbdTestBase
    {
        /// <summary>
        /// Establish SMB2 connection over RDMA and open file
        /// 1. Connect to server over RDMA
        /// 2. SMBD Negotiation over RDMA
        /// 3. Establish SMB2 session and open file with specific dialect
        /// </summary>
        /// <param name="fileName">File name to open</param>
        /// <param name="negotiatedDialects">Optional to set the SMB2 dialects used for SMB2 connection</param>
        protected virtual void EstablishConnectionAndOpenFile(string fileName, DialectRevision[] negotiatedDialects = null)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Establish SMB2 connection over RDMA and open file " + fileName);

            // Connect to server over RDMA
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            // SMBD Negotiate
            status = smbdAdapter.SmbdNegotiate();
            if (status == NtStatus.STATUS_NOT_SUPPORTED)
            {
                BaseTestSite.Assert.Inconclusive("Requested SMB dialects are not supported.");
            }
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);

            status = smbdAdapter.Smb2EstablishSessionAndOpenFile(fileName, negotiatedDialects);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 establish session and open file is {0}", status);
        }

        public void ValidateFileContent(byte[] expectedContent)
        {
            uint sizePerRead = (uint)smbdAdapter.Smb2MaxReadSize;
            uint expectedFileSize = (uint)expectedContent.Length;

            // SMB2 Read file
            SmbdBufferDescriptorV1 descp;
            NtStatus status = smbdAdapter.SmbdRegisterBuffer(
                sizePerRead,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            // send each read request according SMB2 read file limit
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            uint offset = 0;
            while (offset < expectedFileSize)
            {
                byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

                uint length = sizePerRead;
                if (offset + length > expectedFileSize)
                {
                    length = expectedFileSize - offset;
                }

                READ_Response readResponse;
                byte[] readData;
                status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    (UInt64)offset,
                    (uint)length,
                    channelInfo,
                    out readResponse,
                    out readData);

                BaseTestSite.Assert.AreEqual(
                    NtStatus.STATUS_SUCCESS,
                    status,
                    "SMB2 READ over RDMA should succeed.");

                byte[] readFileContent = new byte[length];
                smbdAdapter.SmbdReadRegisteredBuffer(readFileContent, descp);
                BaseTestSite.Assert.IsTrue(
                    SmbdUtilities.CompareByteArray(expectedContent.Skip((int)offset).Take((int)length).ToArray(), readFileContent),
                    "Check content of file");

                offset += sizePerRead;
            }
        }
    }
}
