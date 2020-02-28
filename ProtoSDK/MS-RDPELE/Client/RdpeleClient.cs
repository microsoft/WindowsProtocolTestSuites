// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
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
            if (packet == null)
                return null;
            if (packet is Server_License_Error_Pdu_Valid_Client)
            {
                licensePdu = new TS_LICENSE_PDU(rdpbcgrClient.context);
                licensePdu.commonHeader = ((Server_License_Error_Pdu_Valid_Client)packet).commonHeader;
                licensePdu.preamble = ((Server_License_Error_Pdu_Valid_Client)packet).preamble;
                licensePdu.LicensingMessage.LicenseError = ((Server_License_Error_Pdu_Valid_Client)packet).validClientMessage;
            }
            else if (packet is RdpelePdu)
            {
                licensePdu = new TS_LICENSE_PDU(rdpbcgrClient.context);
                licensePdu.commonHeader = ((RdpelePdu)packet).commonHeader;
                licensePdu.preamble = ((RdpelePdu)packet).preamble;
                licensePdu.FromBytes(((RdpelePdu)packet).rdpeleData);

                if (licensePdu.preamble.bMsgType == bMsgType_Values.LICENSE_REQUEST)
                {
                    // Save server random and decode cert to get public key for future use.
                    serverRandom = licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerRandom;

                    // According to [MS-RDPELE] section 2.2.2.1
                    // The terminal server can choose not to send the certificate by setting the wblobLen field in the Licensing Binary BLOB structure to 0. 
                    // If encryption is in effect and is already protecting RDP traffic, the licensing protocol MAY<3> choose not to send the server certificate 
                    // (for RDP security measures, see [MS-RDPBCGR] sections 5.3 and 5.4). If the licensing protocol chooses not to send the server certificate, 
                    // then the client uses the public key obtained from the server certificate sent as part of Server Security Data in the 
                    // Server MCS Connect Response PDU (see [MS-RDPBCGR] section 2.2.1.4).
                    if (licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerCertificate.wBlobLen == 0)
                    {
                        publicExponent = rdpbcgrClient.context.ServerExponent;
                        modulus = rdpbcgrClient.context.ServerModulus;
                    }
                    else
                    {
                        int index = 0;
                        SERVER_CERTIFICATE cert = RdpbcgrDecoder.DecodeServerCertificate(
                            licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerCertificate.blobData,
                            ref index,
                            (uint)licensePdu.LicensingMessage.ServerLicenseRequest.Value.ServerCertificate.blobData.Length);
                        RdpbcgrDecoder.DecodePubicKey(cert, out publicExponent, out modulus);
                    }
                }
                else if (licensePdu.preamble.bMsgType == bMsgType_Values.PLATFORM_CHALLENGE)
                {
                    // Decrypt platform challenge for future use.
                    byte[] encryptedPlatformChallenge = licensePdu.LicensingMessage.ServerPlatformChallenge.Value.EncryptedPlatformChallenge.blobData;
                    platformChallenge = RC4(encryptedPlatformChallenge);
                    if (platformChallenge == null)
                    {
                        throw new Exception("The decrpyted PlatformChallenge should not be NULL!");
                    }

                    if (!VerifyServerMAC(licensePdu.LicensingMessage.ServerPlatformChallenge.Value.MACData, platformChallenge))
                    {
                        throw new Exception("The MACData of PLATFORM_CHALLENGE from Server is invalid!");
                    }
                }
                else if (licensePdu.preamble.bMsgType == bMsgType_Values.NEW_LICENSE)
                {
                    // Decrypt the license info for future use.
                    var decryptedLicenseInfo = RC4(licensePdu.LicensingMessage.ServerNewLicense.Value.EncryptedLicenseInfo.blobData);
                    newLicenseInfo = TypeMarshal.ToStruct<NEW_LICENSE_INFO>(decryptedLicenseInfo);

                    if (!VerifyServerMAC(licensePdu.LicensingMessage.ServerNewLicense.Value.MACData, decryptedLicenseInfo))
                    {
                        throw new Exception("The MACData of SERVER_NEW_LICENSE from Server is invalid!");
                    }
                }
                else if (licensePdu.preamble.bMsgType == bMsgType_Values.UPGRADE_LICENSE)
                {
                    // Decrypt the license info for future use.
                    var decryptedLicenseInfo = RC4(licensePdu.LicensingMessage.ServerUgradeLicense.Value.EncryptedLicenseInfo.blobData);
                    upgradedLicenseInfo = TypeMarshal.ToStruct<NEW_LICENSE_INFO>(decryptedLicenseInfo);

                    if (!VerifyServerMAC(licensePdu.LicensingMessage.ServerUgradeLicense.Value.MACData, decryptedLicenseInfo))
                    {
                        throw new Exception("The MACData of SERVER_UPGRADE_LICENSE from Server is invalid!");
                    }
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
        public void SendClientNewLicenseRequest(KeyExchangeAlg preferredKeyExchangeAlg, uint platformId, string clientUserName, string clientMachineName)
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
            if (macSaltKey == null)
            {
                throw new Exception("The generated MAC-salt-key should not be NULL!");
            }

            if (licensingEncryptionKey == null)
            {
                throw new Exception("The generated LicensingEncryptionKey should not be NULL!");
            }

            try
            {
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
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Construct and send CLIENT_LICENSE_INFO
        /// Meanwhile, generate the two licensing keys: licensing encryption key and MAC salt key.
        /// </summary>
        public void SendClientLicenseInformation(KeyExchangeAlg preferredKeyExchangeAlg, uint platformId, byte[] licenseInfo, CLIENT_HARDWARE_ID clientHardwareID)
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
            if (macSaltKey == null)
            {
                throw new Exception("The generated MAC-salt-key should not be NULL!");
            }

            if (licensingEncryptionKey == null)
            {
                throw new Exception("The generated LicensingEncryptionKey should not be NULL!");
            }

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
        /// Verify if the MACData received from server is valid or not.
        /// </summary>
        /// <param name="receivedMAC">The MACData received from Server</param>
        /// <param name="decryptedData">The decrypted data from Server, which is protected by MACData</param>
        /// <returns>If the received MAC is valid or not</returns>
        public bool VerifyServerMAC(byte[] receivedMAC, byte[] decryptedData)
        {
            var calculatedMAC = EncryptionAlgorithm.GenerateNonFIPSDataSignature(macSaltKey, decryptedData, 16 * 8);
            return Enumerable.SequenceEqual(receivedMAC, calculatedMAC);
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
