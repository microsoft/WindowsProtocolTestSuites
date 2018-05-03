// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    class Smb3Client : IDisposable
    {
        private Smb2Client client;
        private ulong messageId;
        private Guid clientGuid;
        private DialectRevision selectedDialect;
        private ulong sessionId;
        private byte[] serverGssToken;
        private int creditAvailable;
        bool signingRequired;
        bool encryptionEnabled;

        public Smb3Client()
        {
            client = new Smb2Client(new TimeSpan(0, 0, 20));


            messageId = 0;

            clientGuid = Guid.NewGuid();

            sessionId = 0;

            serverGssToken = null;

            creditAvailable = 0;

        }



        private ushort RequestAndConsumeCredit()
        {
            if (creditAvailable < 0)
            {
                throw new InvalidOperationException("Not enough credit!");
            }

            if (creditAvailable == 0)
            {
                creditAvailable--;
                return 1;
            }
            else
            {
                creditAvailable--;
                return 0;
            }
        }

        private void UpdateCredit(Packet_Header header)
        {
            creditAvailable += header.CreditRequestResponse;

            if (creditAvailable < 0)
            {
                throw new InvalidOperationException("Not enough credit!");
            }
        }



        public void Connect(IPAddress serverIp, IPAddress clientIp)
        {
            client.ConnectOverTCP(serverIp, clientIp);
        }

        public void Connect(IPAddress serverIp)
        {
            client.ConnectOverTCP(serverIp);
        }

        private ulong GetMessageId()
        {
            ulong result = messageId;
            messageId++;
            return result;
        }

        public void Negotiate(DialectRevision[] requestDialects)
        {
            Packet_Header packetHeader;

            NEGOTIATE_Response response;

            PreauthIntegrityHashID[] preauthHashAlgs = null;
            EncryptionAlgorithm[] encryptionAlgs = null;
            if (requestDialects.Contains(DialectRevision.Smb311))
            {
                // initial negotiation context for SMB 3.1.1 dialect
                preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgs = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_CCM, EncryptionAlgorithm.ENCRYPTION_AES128_GCM };
            }

            uint status = client.Negotiate(
                0,
                RequestAndConsumeCredit(),
                Packet_Header_Flags_Values.NONE,
                GetMessageId(),
                requestDialects,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                clientGuid,
                out selectedDialect,
                out serverGssToken,
                out packetHeader,
                out response,
                0,
                preauthHashAlgs,
                encryptionAlgs
                );

            UpdateCredit(packetHeader);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("Negotiate failed with {0:X08}.", status));
            }

            signingRequired = response.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);
        }

        public void SessionSetup(SecurityPackageType authentication, string domain, string serverName, string userName, string password)
        {
            var sspiClientGss = new SspiClientSecurityContext(
                   authentication,
                   new AccountCredential(domain, userName, password),
                   Smb2Utility.GetCifsServicePrincipalName(serverName),
                   ClientSecurityContextAttribute.None,
                   SecurityTargetDataRepresentation.SecurityNativeDrep
                   );


            if (authentication == SecurityPackageType.Negotiate)
            {
                sspiClientGss.Initialize(serverGssToken);
            }
            else
            {
                sspiClientGss.Initialize(null);
            }

            Packet_Header packetHeader;

            SESSION_SETUP_Response sessionSetupResponse;

            uint status;

            while (true)
            {
                status = client.SessionSetup(
                    0,
                    RequestAndConsumeCredit(),
                    Packet_Header_Flags_Values.NONE,
                    GetMessageId(),
                    sessionId,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    SESSION_SETUP_Request_Capabilities_Values.NONE,
                    sessionId,
                    sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out packetHeader,
                    out sessionSetupResponse
                    );

                UpdateCredit(packetHeader);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) && serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }

                if (status != Smb2Status.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    break;
                }
            }

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("SessionSetup failed with {0:X08}.", status));
            }

            encryptionEnabled = sessionSetupResponse.SessionFlags.HasFlag(SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA);

            if (!encryptionEnabled)
            {
                signingRequired = true;
            }

            client.EnableSessionSigningAndEncryption(sessionId, signingRequired, encryptionEnabled);

            client.GenerateCryptoKeys(
                sessionId,
                sspiClientGss.SessionKey,
                signingRequired,
                encryptionEnabled
                );

        }


        public void TreeConnect(string path, out uint treeId)
        {
            Packet_Header packetHeader;

            TREE_CONNECT_Response response;


            uint status = client.TreeConnect(
                0,
                RequestAndConsumeCredit(),
                signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                GetMessageId(),
                sessionId,
                path,
                out treeId,
                out packetHeader,
                out response
                );

            UpdateCredit(packetHeader);


            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("TreeConnect failed with {0:X08}.", status));
            }
        }

        public void IoCtl(uint treeId, CtlCode_Values ctlCode, FILEID fileId, IOCTL_Request_Flags_Values flag)
        {
            Packet_Header packetHeader;
            IOCTL_Response response;

            byte[] input;
            byte[] output;

            uint status = client.IoCtl(
                0,
                RequestAndConsumeCredit(),
                signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                GetMessageId(),
                sessionId,
                treeId,
                ctlCode,
                fileId,
                0,
                null,
                4096,
                flag,
                out input,
                out output,
                out packetHeader,
                out response
                );

            UpdateCredit(packetHeader);


            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("IoCtl failed with {0:X08}.", status));
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
