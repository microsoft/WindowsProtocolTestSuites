// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    class Smb3Client
    {
        private Smb2Client client;
        private ulong messageId;
        private Guid clientGuid;
        private ulong sessionId;
        private byte[] clientGssToken;
        private byte[] serverGssToken;

        public Smb3Client()
        {
            client = new Smb2Client(new TimeSpan(0, 0, 10));


            messageId = 0;

            clientGuid = Guid.NewGuid();

            sessionId = 0;

            clientGssToken = null;
            serverGssToken = null;
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
            DialectRevision selectedDialect;

            Packet_Header packetHeader;

            NEGOTIATE_Response resonse;

            PreauthIntegrityHashID[] preauthHashAlgs = null;
            EncryptionAlgorithm[] encryptionAlgs = null;
            if (requestDialects.Contains(DialectRevision.Smb311))
            {
                // initial negotiation context for SMB 3.1.1 dialect
                preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgs = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_CCM, EncryptionAlgorithm.ENCRYPTION_AES128_GCM };
            }

            uint status = client.Negotiate(
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                GetMessageId(),
                requestDialects,
                SecurityMode_Values.NONE,
                Capabilities_Values.NONE,
                clientGuid,
                out selectedDialect,
                out clientGssToken,
                out packetHeader,
                out resonse,
                0,
                preauthHashAlgs,
                encryptionAlgs
                );

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("Negotiate failed with {0:X08}.", status));
            }
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
                sspiClientGss.Initialize(clientGssToken);
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
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    GetMessageId(),
                    0,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NONE,
                    SESSION_SETUP_Request_Capabilities_Values.NONE,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out packetHeader,
                    out sessionSetupResponse
                    );

                if (status != Smb2Status.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    break;
                }

                sspiClientGss.Initialize(clientGssToken);
            }

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("SessionSetup failed with {0:X08}.", status));
            }

        }


        public void TreeConnect(string path, out uint treeId)
        {
            Packet_Header packetHeader;

            TREE_CONNECT_Response response;

            uint status = client.TreeConnect(
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                GetMessageId(),
                sessionId,
                path,
                out treeId,
                out packetHeader,
                out response
                );


            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("TreeConnect failed with {0:X08}.", status));
            }
        }
    }
}
