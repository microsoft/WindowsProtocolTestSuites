// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpele
{
    public class RdpeleClient : IDisposable
    {
        /// <summary>
        /// Use RdpbcgrClient to send and receive the license pdu.
        /// </summary>
        private RdpbcgrClient rdpbcgrClient;

        private byte[] preMasterSecret;
        private byte[] publicExponent;
        private byte[] modulus;

        private byte[] clientRandom;
        private byte[] serverRandom;

        private PROPRIETARYSERVERCERTIFICATE? proprietaryCert;
        private X509_CERTIFICATE_CHAIN? x509CertChain;

        public RdpeleClient(RdpbcgrClient rdpbcgrClient)
        {
            this.rdpbcgrClient = rdpbcgrClient;
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// Expect the specified pdu from RDP server within the timeout.
        /// </summary>
        public TS_LICENSE_PDU ExpectPdu(TimeSpan timeout)
        {
            TS_LICENSE_PDU licensePdu = null;
            StackPacket packet = rdpbcgrClient.ExpectPdu(timeout);
            if (packet != null && packet is RdpelePdu)
            {
                licensePdu = new TS_LICENSE_PDU(rdpbcgrClient.context);
                licensePdu.commonHeader = ((RdpelePdu)packet).commonHeader;
                licensePdu.preamble = ((RdpelePdu)packet).preamble;
                licensePdu.FromBytes(((RdpelePdu)packet).rdpeleData);

                if (licensePdu.preamble.bMsgType == bMsgType_Values.LICENSE_REQUEST)
                {
                    serverRandom = licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerRandom;
                    DecodeCert(licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerCertificate.blobData);
                }
            }

            return licensePdu;
        }

        /// <summary>
        /// Construct and send CLIENT_NEW_LICENSE_REQUEST
        /// </summary>
        public void SendClientNewLicenseRequest(uint preferredKeyExchangeAlg, uint platformId, string clientUserName, string clientMachineName)
        {
            CLIENT_NEW_LICENSE_REQUEST request = new CLIENT_NEW_LICENSE_REQUEST();
            request.PreferredKeyExchangeAlg = preferredKeyExchangeAlg;
            request.PlatformId = platformId;

            var random = new Random();
            clientRandom = new byte[32];
            random.NextBytes(clientRandom);
            request.ClientRandom = clientRandom;

            preMasterSecret = new byte[48];
            random.NextBytes(preMasterSecret);
            request.EncryptedPreMasterSecret.blobData = RdpbcgrUtility.GenerateEncryptedRandom(preMasterSecret, publicExponent, modulus);
            request.EncryptedPreMasterSecret.wBlobLen = (ushort)request.EncryptedPreMasterSecret.blobData.Length;
            request.EncryptedPreMasterSecret.wBlobType = wBlobType_Values.BB_RANDOM_BLOB;

            request.ClientUserName.wBlobType = wBlobType_Values.BB_CLIENT_USER_NAME_BLOB;
            request.ClientUserName.blobData = Encoding.UTF8.GetBytes(clientUserName);
            request.ClientUserName.wBlobLen = (ushort)request.ClientUserName.blobData.Length;

            request.ClientMachineName.wBlobType = wBlobType_Values.BB_CLIENT_MACHINE_NAME_BLOB;
            request.ClientMachineName.blobData = Encoding.UTF8.GetBytes(clientMachineName);
            request.ClientMachineName.wBlobLen = (ushort)request.ClientMachineName.blobData.Length;

            TS_LICENSE_PDU pdu = new TS_LICENSE_PDU(rdpbcgrClient.context);
            RdpbcgrUtility.FillCommonHeader(rdpbcgrClient.context, ref pdu.commonHeader, TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT);
            pdu.LicensingMessage.ClientNewLicenseRequest = request;
            pdu.preamble.bMsgType = bMsgType_Values.NEW_LICENSE_REQUEST;
            pdu.preamble.bVersion = bVersion_Values.PREAMBLE_VERSION_3_0 | bVersion_Values.EXTENDED_ERROR_MSG_SUPPORTED;

            var bytes = pdu.ToBytes();
            rdpbcgrClient.SendBytes(bytes);
        }

        public void SendClientLicenseInformation()
        {

        }

        public void SendClientPlatformChallengeResponse()
        {

        }

        /// <summary>
        /// Decode the cert from the RDP server and get the public key, in case to encrypt PreMasterSecret;
        /// </summary>
        private void DecodeCert(byte[] certBlob)
        {
            int index = 0;
            uint version = RdpbcgrDecoder.ParseUInt32(certBlob, ref index, false);
            if (version == (uint)SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1)
            {
                // proprietary server certificate
                proprietaryCert = RdpbcgrDecoder.ParseProprietaryServerCertificate(certBlob, ref index);
                RSA_PUBLIC_KEY rsaPublicKey = proprietaryCert.Value.PublicKeyBlob;
                publicExponent = BitConverter.GetBytes(rsaPublicKey.pubExp);
                modulus = rsaPublicKey.modulus;
            }
            else if (version == (uint)SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_2)
            {
                // X509 certificate chain
                x509CertChain = RdpbcgrDecoder.ParseX509CertificateChain(certBlob, ref index, certBlob.Length - index);
                var x509Cert = new X509Certificate2(x509CertChain.Value.CertBlobArray[3].abCert);
                var publicKey = x509Cert.PublicKey.EncodedKeyValue.RawData;
                DecodeX509RSAPublicKey(publicKey);
            }
            else
            {
                throw new Exception($"Invalid certChainVersion: {version}");
            }
        }

        private void DecodeX509RSAPublicKey(byte[] publicKey)
        {
            // An RSA public key should be represented with the ASN.1 type RSAPublicKey:
            //    RSAPublicKey::= SEQUENCE {
            //        modulus         INTEGER,  --n
            //        publicExponent  INTEGER   --e
            //    }

            if (publicKey == null)
            {
                throw new Exception("publicKey should not be null!");
            }

            // 0x30 stands for "SEQUENCE", 0x02 stands for "INTEGER". publicKey[1] is the size of the SEQUENCE which we don't care.
            if (publicKey[0] != 0x30 || publicKey[2] != 0x02)
            {
                throw new Exception("Invalid publicKey!");
            }

            int modulusLength = publicKey[3]; // publicKey[3] 
            modulus = new byte[modulusLength];
            Buffer.BlockCopy(publicKey, 4, modulus, 0, modulusLength);

            // 0x02 stands for "INTEGER"
            if (publicKey[4 + modulusLength] != 0x02)
            {
                throw new Exception("Invalid publicKey!");
            }

            int publicExponentLength = publicKey[5 + modulusLength];
            publicExponent = new byte[publicExponentLength];
            Buffer.BlockCopy(publicKey, 6 + modulusLength, publicExponent, 0, publicExponentLength);
        }
    }
}
