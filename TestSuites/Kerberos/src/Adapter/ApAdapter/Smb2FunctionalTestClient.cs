// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    /// <summary>
    /// Delegate to check response header and payload
    /// </summary>
    /// <typeparam name="T">Type of response payload</typeparam>
    /// <param name="responseHeader">Response header to be checked</param>
    /// <param name="response">Response payload to be checked</param>
    public delegate void ResponseChecker<T>(Packet_Header responseHeader, T response);

    /// <summary>
    /// SMB2 client for functional testing. 
    /// By default signing is enabled and encryption is disabled.
    /// </summary>
    public class Smb2FunctionalTestClient
    {
        #region Private fields

        private Smb2Client client;

        private ulong messageId;
        private ulong sessionId;
        private ushort grantedCredit;
        private byte[] serverGssToken;

        #endregion

        #region Constructor
        public Smb2FunctionalTestClient(TimeSpan timeout)
        {
            client = new Smb2Client(timeout);
        }
        #endregion

        #region Properties
        public Smb2Client Smb2Client
        {
            get
            {
                return client;
            }
        }
        #endregion

        #region Connect and Disconnect
        public void ConnectToServerOverTCP(IPAddress serverIp)
        {
            client.ConnectOverTCP(serverIp);
        }

        public void ConnectToServerOverTCP(IPAddress serverIp, IPAddress clientIp)
        {
            client.ConnectOverTCP(serverIp, clientIp);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        #endregion

        #region Negotiate
        public uint Negotiate(DialectRevision[] dialects, SecurityMode_Values securityMode, Capabilities_Values capabilityValue, Guid clientGuid, out DialectRevision selectedDialect, ushort creditRequest = 1)
        {
            Packet_Header header;
            NEGOTIATE_Response negotiateResponse;

            uint status = client.Negotiate(
                0,
                creditRequest,
                Packet_Header_Flags_Values.NONE,
                messageId++,
                dialects,
                securityMode,
                capabilityValue,
                clientGuid,
                out selectedDialect,
                out serverGssToken,
                out header,
                out negotiateResponse);

            grantedCredit = header.CreditRequestResponse;

            return status;
        }
        #endregion

        #region Session Setup and Logoff

        public uint SessionSetup(
            SESSION_SETUP_Request_SecurityMode_Values securityMode,
            SESSION_SETUP_Request_Capabilities_Values capabilities,
            SecurityPackageType securityPackageType,
            string serverName,
            byte[] token,
            out byte[] serverGssToken,
            ushort creditRequest = 64)
        {
            return SessionSetup(
                Packet_Header_Flags_Values.NONE,
                SESSION_SETUP_Request_Flags.NONE,
                securityMode,
                capabilities,
                0,
                securityPackageType,
                serverName,
                token,
                out serverGssToken,
                creditRequest);
        }

        private uint SessionSetup(
            Packet_Header_Flags_Values headerFlags,
            SESSION_SETUP_Request_Flags sessionSetupFlags,
            SESSION_SETUP_Request_SecurityMode_Values securityMode,
            SESSION_SETUP_Request_Capabilities_Values capabilities,
            ulong previousSessionId,
            SecurityPackageType securityPackageType,
            string serverName,
            byte[] token,
            out byte[] serverGssToken,
            ushort creditRequest = 64)
        {
            Packet_Header header;
            SESSION_SETUP_Response sessionSetupResponse;

            uint status;

            status = client.SessionSetup(
                1,
                creditRequest,
                headerFlags,
                messageId++,
                sessionId,
                sessionSetupFlags,
                securityMode,
                capabilities,
                previousSessionId,
                token,
                out sessionId,
                out serverGssToken,
                out header,
                out sessionSetupResponse);

            return status;
        }

        public uint LogOff(ushort creditRequest = 1)
        {
            Packet_Header header;
            LOGOFF_Response logoffResponse;

            uint status = client.LogOff(
                1,
                creditRequest,
                Packet_Header_Flags_Values.NONE,
                messageId++,
                sessionId,
                out header,
                out logoffResponse);

            return status;
        }

        #endregion
        
        #region Tree Connect and Disconnect

        public uint TreeConnect(
            string uncSharePath,
            out uint treeId,
            ResponseChecker<TREE_CONNECT_Response> checker = null,
            ushort creditRequest = 1)
        {
            Packet_Header header;
            TREE_CONNECT_Response treeConnectResponse;

            uint status = client.TreeConnect(
                    1,
                    creditRequest,
                    Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    uncSharePath,
                    out treeId,
                    out header,
                    out treeConnectResponse);

            InnerResponseChecker(checker, header, treeConnectResponse);

            grantedCredit = header.CreditRequestResponse;

            return status;
        }

        public uint TreeDisconnect(
            uint treeId,
            ResponseChecker<TREE_DISCONNECT_Response> checker = null,
            ushort creditRequest = 1)
        {
            Packet_Header header;
            TREE_DISCONNECT_Response treeDisconnectResponse;

            uint status = client.TreeDisconnect(
                1,
                creditRequest,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                sessionId,
                treeId,
                out header,
                out treeDisconnectResponse);

            InnerResponseChecker(checker, header, treeDisconnectResponse);

            return status;
        }

        #endregion

        #region Create and Close

        public uint Create(
            uint treeId,
            string fileName,
            CreateOptions_Values createOptions,
            out FILEID fileId,
            out Smb2CreateContextResponse[] serverCreateContexts,
            RequestedOplockLevel_Values requestedOplockLevel_Values = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
            Smb2CreateContextRequest[] createContexts = null,
            Packet_Header_Flags_Values headerFlag = Packet_Header_Flags_Values.FLAGS_SIGNED,
            AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
            ShareAccess_Values shareAccess = ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
            ResponseChecker<CREATE_Response> checker = null,
            ushort creditRequest = 64)
        {
            Packet_Header header;
            CREATE_Response createResponse;

            uint status = client.Create(
                1,
                creditRequest,
                headerFlag,
                messageId++,
                sessionId,
                treeId,
                fileName,
                accessMask,
                shareAccess,
                createOptions,
                CreateDisposition_Values.FILE_OPEN_IF,
                File_Attributes.NONE,
                ImpersonationLevel_Values.Impersonation,
                SecurityFlags_Values.NONE,
                requestedOplockLevel_Values,
                createContexts,
                out fileId,
                out serverCreateContexts,
                out header,
                out createResponse);

            InnerResponseChecker(checker, header, createResponse);

            grantedCredit = header.CreditRequestResponse;

            return status;
        }

        public uint Close(uint treeId, FILEID fileId, ResponseChecker<CLOSE_Response> checker = null, ushort creditRequest = 1)
        {
            Packet_Header header;
            CLOSE_Response closeResponse;

            uint status = client.Close(
                1,
                creditRequest,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                sessionId,
                treeId,
                fileId,
                Flags_Values.NONE,
                out header,
                out closeResponse);

            InnerResponseChecker(checker, header, closeResponse);

            grantedCredit = header.CreditRequestResponse;

            return status;
        }

        #endregion

        #region Encryption and Signing Settings

        public void SetSessionSigningAndEncryption(bool enableSigning, bool enableEncryption, byte[] sessionkey = null)
        {
            client.GenerateCryptoKeys(sessionId, sessionkey, enableSigning, enableEncryption);           
        }

        #endregion

        #region private method

        void InnerResponseChecker<T>(ResponseChecker<T> checker, Packet_Header header, T response)
        {
            if (checker != null)
            {
                checker(header, response);
            }
            else
            {
                TestClassBase.BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "{0} should succeed", header.Command);
            }
        }

        #endregion

    }
}
