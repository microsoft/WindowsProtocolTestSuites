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
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using System.Security.Cryptography;

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

        private byte[] macSaltKey;
        private byte[] licensingEncryptionKey;

        private byte[] platformChallenge;

        private PROPRIETARYSERVERCERTIFICATE? proprietaryCert;
        private X509_CERTIFICATE_CHAIN? x509CertChain;

        private NEW_LICENSE_INFO? newLicenseInfo;
        private NEW_LICENSE_INFO? upgradedLicenseInfo;

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
                    // Save server random and decode cert to get public key for future use.
                    serverRandom = licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerRandom;
                    DecodeCert(licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerCertificate.blobData);
                }
                else if (licensePdu.preamble.bMsgType == bMsgType_Values.PLATFORM_CHALLENGE)
                {
                    // Decrypt platform challenge for future use.
                    byte[] encryptedPlatformChallenge = licensePdu.LicensingMessage.ServerPlatformChallenge.Value.EncryptedPlatformChallenge.blobData;
                    platformChallenge = RC4(encryptedPlatformChallenge);
                }
                else if (licensePdu.preamble.bMsgType == bMsgType_Values.NEW_LICENSE)
                {
                    // Decrypt the license info for future use.
                    var decryptedLicenseInfo = RC4(licensePdu.LicensingMessage.ServerNewLicense.Value.EncryptedLicenseInfo.blobData);
                    newLicenseInfo = TypeMarshal.ToStruct<NEW_LICENSE_INFO>(decryptedLicenseInfo);
                }
                else if (licensePdu.preamble.bMsgType == bMsgType_Values.UPGRADE_LICENSE)
                {
                    // Decrypt the license info for future use.
                    var decryptedLicenseInfo = RC4(licensePdu.LicensingMessage.ServerUgradeLicense.Value.EncryptedLicenseInfo.blobData);
                    upgradedLicenseInfo = TypeMarshal.ToStruct<NEW_LICENSE_INFO>(decryptedLicenseInfo);
                }
                else
                {
                    throw new Exception($"The received PDU type should not be {licensePdu.preamble.bMsgType}!");
                }
            }

            return licensePdu;
        }

        /// <summary>
        /// Construct and send CLIENT_NEW_LICENSE_REQUEST
        /// Meanwhile, generate the two licensing keys: licensing encryption key and MAC salt key.
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

            // Generate licensingEncryptionKey and macSaltKey
            EncryptionAlgorithm.GenerateLicensingKeys(preMasterSecret, clientRandom, serverRandom, out macSaltKey, out licensingEncryptionKey);

            request.EncryptedPreMasterSecret.blobData = RdpbcgrUtility.GenerateEncryptedRandom(preMasterSecret, publicExponent, modulus);
            request.EncryptedPreMasterSecret.wBlobLen = (ushort)request.EncryptedPreMasterSecret.blobData.Length;
            request.EncryptedPreMasterSecret.wBlobType = wBlobType_Values.BB_RANDOM_BLOB;

            request.ClientUserName.wBlobType = wBlobType_Values.BB_CLIENT_USER_NAME_BLOB;
            request.ClientUserName.blobData = Encoding.UTF8.GetBytes(clientUserName);
            request.ClientUserName.wBlobLen = (ushort)request.ClientUserName.blobData.Length;

            request.ClientMachineName.wBlobType = wBlobType_Values.BB_CLIENT_MACHINE_NAME_BLOB;
            request.ClientMachineName.blobData = Encoding.UTF8.GetBytes(clientMachineName);
            request.ClientMachineName.wBlobLen = (ushort)request.ClientMachineName.blobData.Length;

            TS_LICENSE_PDU pdu = ConstructLicensePDU(bMsgType_Values.NEW_LICENSE_REQUEST, new LicensingMessage { ClientNewLicenseRequest = request });
            var bytes = pdu.ToBytes();
            rdpbcgrClient.SendBytes(bytes);
        }

        /// <summary>
        /// Construct and send CLIENT_LICENSE_INFO
        /// Meanwhile, generate the two licensing keys: licensing encryption key and MAC salt key.
        /// </summary>
        public void SendClientLicenseInformation(uint preferredKeyExchangeAlg, uint platformId, byte[] licenseInfo, CLIENT_HARDWARE_ID clientHardwareID)
        {
            CLIENT_LICENSE_INFO info = new CLIENT_LICENSE_INFO();
            info.PreferredKeyExchangeAlg = preferredKeyExchangeAlg;
            info.PlatformId = platformId;

            info.LicenseInfo.blobData = licenseInfo;
            info.LicenseInfo.wBlobLen = (ushort)licenseInfo.Length;
            info.LicenseInfo.wBlobType = wBlobType_Values.BB_DATA_BLOB;

            var random = new Random();
            clientRandom = new byte[32];
            random.NextBytes(clientRandom);
            info.ClientRandom = clientRandom;

            preMasterSecret = new byte[48];
            random.NextBytes(preMasterSecret);

            EncryptionAlgorithm.GenerateLicensingKeys(preMasterSecret, clientRandom, serverRandom, out macSaltKey, out licensingEncryptionKey);

            info.EncryptedPreMasterSecret.blobData = RdpbcgrUtility.GenerateEncryptedRandom(preMasterSecret, publicExponent, modulus);
            info.EncryptedPreMasterSecret.wBlobLen = (ushort)info.EncryptedPreMasterSecret.blobData.Length;
            info.EncryptedPreMasterSecret.wBlobType = wBlobType_Values.BB_RANDOM_BLOB;

            CLIENT_HARDWARE_ID hardwareID = new CLIENT_HARDWARE_ID();
            hardwareID = clientHardwareID;

            info.EncryptedHWID.wBlobType = wBlobType_Values.BB_ENCRYPTED_DATA_BLOB;
            var hardwareIDBytes = TypeMarshal.ToBytes<CLIENT_HARDWARE_ID>(hardwareID);
            info.EncryptedHWID.blobData = RC4(hardwareIDBytes);
            info.EncryptedHWID.wBlobLen = (ushort)info.EncryptedHWID.blobData.Length;

            // MACData (16 bytes): An array of 16 bytes containing an MD5 digest (Message Authentication Code (MAC)) 
            // that is generated over the unencrypted Client Hardware Identification structure. 
            info.MACData = EncryptionAlgorithm.GenerateNonFIPSDataSignature(macSaltKey, hardwareIDBytes, 16 * 8); // n is 16 * 8 bits.

            TS_LICENSE_PDU pdu = ConstructLicensePDU(bMsgType_Values.LICENSE_INFO, new LicensingMessage { ClientLicenseInfo = info });
            rdpbcgrClient.SendBytes(pdu.ToBytes());
        }

        /// <summary>
        /// Construct and send CLIENT_PLATFORM_CHALLENGE_RESPONSE with encrypted Platform Challeng Response
        /// </summary>
        public void SendClientPlatformChallengeResponse(CLIENT_HARDWARE_ID clientHardwareID)
        {
            CLIENT_PLATFORM_CHALLENGE_RESPONSE response = new CLIENT_PLATFORM_CHALLENGE_RESPONSE();

            PLATFORM_CHALLENGE_RESPONSE_DATA challengeData = new PLATFORM_CHALLENGE_RESPONSE_DATA();
            challengeData.pbChallenge = platformChallenge;
            challengeData.cbChallenge = (ushort)platformChallenge.Length;
            challengeData.wClientType = 0x0100;
            challengeData.wVersion = 0x0100;
            challengeData.wLicenseDetailLevel = 0x0003;
            response.EncryptedPlatformChallengResponse.wBlobType = wBlobType_Values.BB_ENCRYPTED_DATA_BLOB;
            var challengeDataBytes = TypeMarshal.ToBytes<PLATFORM_CHALLENGE_RESPONSE_DATA>(challengeData);
            response.EncryptedPlatformChallengResponse.blobData = RC4(challengeDataBytes);
            response.EncryptedPlatformChallengResponse.wBlobLen = (ushort)response.EncryptedPlatformChallengResponse.blobData.Length;

            CLIENT_HARDWARE_ID hardwareID = new CLIENT_HARDWARE_ID();
            hardwareID = clientHardwareID;

            response.EncryptedHWID.wBlobType = wBlobType_Values.BB_ENCRYPTED_DATA_BLOB;
            var hardwareIDBytes = TypeMarshal.ToBytes<CLIENT_HARDWARE_ID>(hardwareID);
            response.EncryptedHWID.blobData = RC4(hardwareIDBytes);
            response.EncryptedHWID.wBlobLen = (ushort)response.EncryptedHWID.blobData.Length;

            // MACData (16 bytes): An array of 16 bytes containing an MD5 digest (MAC) generated over the Platform Challenge Response Data and decrypted Client Hardware Identification. 
            var data = challengeDataBytes.Concat(hardwareIDBytes).ToArray();
            response.MACData = EncryptionAlgorithm.GenerateNonFIPSDataSignature(macSaltKey, data, 16 * 8); // n is 16 * 8 bits.

             TS_LICENSE_PDU pdu = ConstructLicensePDU(bMsgType_Values.PLATFORM_CHALLENGE_RESPONSE, new LicensingMessage { ClientPlatformChallengeResponse = response });
            rdpbcgrClient.SendBytes(pdu.ToBytes());
        }

        public NEW_LICENSE_INFO? GetNewLicenseInfo() { return newLicenseInfo; }
        public NEW_LICENSE_INFO? GetUpgradedLicenseInfo() { return upgradedLicenseInfo; }

        /// <summary>
        /// Decode the cert from the RDP server and get the public key, in order to encrypt PreMasterSecret
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

            // ASN.1 uses big-endian, but the Proprietary Server Certificate uses little-endian, to utilize them, save little-endian format for both cases.
            Array.Reverse(publicExponent);
            Array.Reverse(modulus);
        }

        /// <summary>
        /// RC4 is used to encrypt and decrypt data according to [MS-RDPELE] 5.1.3 & 5.1.4
        /// </summary>
        private byte[] RC4(byte[] input)
        {
            RC4CryptoServiceProvider rc4Enc = new RC4CryptoServiceProvider();
            ICryptoTransform rc4Encrypt = rc4Enc.CreateEncryptor(licensingEncryptionKey, null);
            byte[] output = new byte[input.Length];
            rc4Encrypt.TransformBlock(input, 0, input.Length, output, 0);
            return output;
        }

        private TS_LICENSE_PDU ConstructLicensePDU(bMsgType_Values messageType, LicensingMessage message)
        {
            TS_LICENSE_PDU pdu = new TS_LICENSE_PDU(rdpbcgrClient.context);
            RdpbcgrUtility.FillCommonHeader(rdpbcgrClient.context, ref pdu.commonHeader, TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT);
            pdu.LicensingMessage = message;
            pdu.preamble.bMsgType = messageType;
            pdu.preamble.bVersion = bVersion_Values.PREAMBLE_VERSION_3_0 | bVersion_Values.EXTENDED_ERROR_MSG_SUPPORTED;
            return pdu;
        }
    }
}
