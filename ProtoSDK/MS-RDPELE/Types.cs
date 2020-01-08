// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpele
{
    /// <summary>
    /// [MS-RDPELE] 2.2.2 Licensing PDU (TS_LICENSING_PDU)
    /// </summary>
    public class TS_LICENSE_PDU : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  A licensing preamble (see [MS-RDPBCGR] section 2.2.1.12.1.1) structure containing header information. 
        ///  The bMsgType field of the preamble structure specifies the type of the licensing message that follows the preamble.        
        /// </summary>
        public LICENSE_PREAMBLE preamble;

        /// <summary>
        ///  A variable-length licensing message whose structure depends on the value of the bMsgType field in the preamble structure. 
        /// </summary>
        public LicensingMessage LicensingMessage;

        public TS_LICENSE_PDU(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        public override byte[] ToBytes()
        {
            byte[] licensingMessageBytes = null;
            switch (preamble.bMsgType)
            {
                case bMsgType_Values.None:
                    break;
                case bMsgType_Values.LICENSE_REQUEST:
                    licensingMessageBytes = TypeMarshal.ToBytes<SERVER_LICENSE_REQUEST>(LicensingMessage.ServerLicenseRequest.Value);
                    break;
                case bMsgType_Values.PLATFORM_CHALLENGE:
                    licensingMessageBytes = TypeMarshal.ToBytes<SERVER_PLATFORM_CHALLENGE>(LicensingMessage.ServerPlatformChallenge.Value);
                    break;
                case bMsgType_Values.NEW_LICENSE:
                    licensingMessageBytes = TypeMarshal.ToBytes<SERVER_NEW_LICENSE>(LicensingMessage.ServerNewLicense.Value);
                    break;
                case bMsgType_Values.UPGRADE_LICENSE:
                    licensingMessageBytes = TypeMarshal.ToBytes<SERVER_UPGRADE_LICENSE>(LicensingMessage.ServerUgradeLicense.Value);
                    break;
                case bMsgType_Values.LICENSE_INFO:
                    licensingMessageBytes = TypeMarshal.ToBytes<CLIENT_LICENSE_INFO>(LicensingMessage.ClientLicenseInfo.Value);
                    break;
                case bMsgType_Values.NEW_LICENSE_REQUEST:
                    licensingMessageBytes = TypeMarshal.ToBytes<CLIENT_NEW_LICENSE_REQUEST>(LicensingMessage.ClientNewLicenseRequest.Value);
                    break;
                case bMsgType_Values.PLATFORM_CHALLENGE_RESPONSE:
                    licensingMessageBytes = TypeMarshal.ToBytes<CLIENT_PLATFORM_CHALLENGE_RESPONSE>(LicensingMessage.ClientPlatformChallengeResponse.Value);
                    break;
                case bMsgType_Values.ERROR_ALERT:
                    licensingMessageBytes = TypeMarshal.ToBytes<LICENSE_ERROR_MESSAGE>(LicensingMessage.LicenseError.Value);
                    break;
                default:
                    break;
            }

            preamble.wMsgSize = (ushort)(licensingMessageBytes.Length + TypeMarshal.GetBlockMemorySize<LICENSE_PREAMBLE>(preamble));
            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, TypeMarshal.ToBytes<LICENSE_PREAMBLE>(preamble).Concat(licensingMessageBytes).ToArray(), context);
            return RdpbcgrUtility.ToBytes(totalBuffer.ToArray());
        }

        public void FromBytes(byte[] data)
        {
            switch (preamble.bMsgType)
            {
                case bMsgType_Values.None:
                    break;
                case bMsgType_Values.LICENSE_REQUEST:
                    LicensingMessage.ServerLicenseRequest = TypeMarshal.ToStruct<SERVER_LICENSE_REQUEST>(data);
                    break;
                case bMsgType_Values.PLATFORM_CHALLENGE:
                    LicensingMessage.ServerPlatformChallenge = TypeMarshal.ToStruct<SERVER_PLATFORM_CHALLENGE>(data);
                    break;
                case bMsgType_Values.NEW_LICENSE:
                    LicensingMessage.ServerNewLicense = TypeMarshal.ToStruct<SERVER_NEW_LICENSE>(data);
                    break;
                case bMsgType_Values.UPGRADE_LICENSE:
                    LicensingMessage.ServerUgradeLicense = TypeMarshal.ToStruct<SERVER_UPGRADE_LICENSE>(data);
                    break;
                case bMsgType_Values.LICENSE_INFO:
                    LicensingMessage.ClientLicenseInfo = TypeMarshal.ToStruct<CLIENT_LICENSE_INFO>(data);
                    break;
                case bMsgType_Values.NEW_LICENSE_REQUEST:
                    LicensingMessage.ClientNewLicenseRequest = TypeMarshal.ToStruct<CLIENT_NEW_LICENSE_REQUEST>(data);
                    break;
                case bMsgType_Values.PLATFORM_CHALLENGE_RESPONSE:
                    LicensingMessage.ClientPlatformChallengeResponse = TypeMarshal.ToStruct<CLIENT_PLATFORM_CHALLENGE_RESPONSE>(data);
                    break;
                case bMsgType_Values.ERROR_ALERT:
                    LicensingMessage.LicenseError = TypeMarshal.ToStruct<LICENSE_ERROR_MESSAGE>(data);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.1.1 Product Information (PRODUCT_INFO)
    /// The Product Information packet contains the details of the product license that is required for connecting to the terminal server. 
    /// </summary>
    public struct PRODUCT_INFO
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the license version information. 
        /// The high-order word contains the major version of the operating system on which the terminal server is running, 
        /// while the low-order word contains the minor version.
        /// </summary>
        public uint dwVersion;

        /// <summary>
        /// An unsigned 32-bit integer that contains the number of bytes in the pbCompanyName field, including the terminating null character. 
        /// This value MUST be greater than zero.
        /// </summary>
        public uint cbCompanyName;

        /// <summary>
        /// Contains a null-terminated Unicode string that specifies the company name.
        /// </summary>
        [Size("cbCompanyName/2")]
        public string pbCompanyName;

        /// <summary>
        /// An unsigned 32-bit integer that contains the number of bytes in the pbProductId field, including the terminating null character. 
        /// This value MUST be greater than zero.
        /// </summary>
        public uint cbProductId;

        /// <summary>
        /// Contains a null-terminated Unicode string that identifies the type of the license that is required by the terminal server.
        /// It MAY have the following string value.
        /// ___________________________________________
        /// Value     | Meaning
        /// ———————————————————————————————————————————
        /// "A02"	  | Per device or per user license
        /// ———————————————————————————————————————————
        /// </summary>
        [Size("cbProductId/2")]
        public string pbProductId;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.1.2.1 Scope (SCOPE)
    /// The Scope packet contains the name of an entity that issued a client license.
    /// </summary>
    public struct SCOPE
    {
        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2) of type BB_SCOPE_BLOB (0x000E). 
        /// This BLOB contains the name of a license issuer in null-terminated ANSI characters, 
        /// as specified in [ISO/IEC-8859-1], string format, with an implementation-specific valid code page.
        /// </summary>
        public LICENSE_BINARY_BLOB Scope;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.1.2 Scope List (SCOPE_LIST)
    /// </summary>
    public struct SCOPE_LIST
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the number of elements in the ScopeArray field.
        /// </summary>
        public uint ScopeCount;

        /// <summary>
        /// An array of Scope structures containing ScopeCount elements. 
        /// </summary>
        [Size("ScopeCount")]
        public SCOPE[] ScopeArray;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.3.1 Client Hardware Identification (CLIENT_HARDWARE_ID)
    /// The Client Hardware Identification packet is used for uniquely identifying a Remote Desktop client for the purpose of issuing a license. 
    /// </summary>
    public struct CLIENT_HARDWARE_ID
    {
        /// <summary>
        /// The content and format of this field are the same as the PlatformId field of the Client New License Request.
        /// </summary>
        public uint PlatformId;

        /// <summary>
        /// A 32-bit unsigned integer containing client hardware-specific data. 
        /// This field MUST contain a number that helps the server uniquely identify the client.
        /// </summary>
        public uint Data1;

        /// <summary>
        /// A 32-bit unsigned integer containing client hardware-specific data. 
        /// This field MUST contain a number that helps the server uniquely identify the client.
        /// </summary>
        public uint Data2;

        /// <summary>
        /// A 32-bit unsigned integer containing client hardware-specific data. 
        /// This field MUST contain a number that helps the server uniquely identify the client.
        /// </summary>
        public uint Data3;

        /// <summary>
        /// A 32-bit unsigned integer containing client hardware-specific data. 
        /// This field MUST contain a number that helps the server uniquely identify the client.
        /// </summary>
        public uint Data4;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.5.1 Platform Challenge Response Data (PLATFORM_CHALLENGE_RESPONSE_DATA)
    /// The Platform Challenge Response Data packet contains information pertaining to the client's license handling capabilities 
    /// and the Client Platform Challenge data sent by the server in the Server Platform Challenge.
    /// </summary>
    public struct PLATFORM_CHALLENGE_RESPONSE_DATA
    {
        /// <summary>
        /// A 16-bit unsigned integer that contains the platform challenge version.
        /// This field MUST be set to 0x0100.
        /// </summary>
        public ushort wVersion;

        /// <summary>
        /// A 16-bit unsigned integer that represents the operating system type of the client
        /// </summary>
        public ushort wClientType;

        /// <summary>
        /// A 16-bit unsigned integer. This field represents the capability of the client to handle license data.
        /// </summary>
        public ushort wLicenseDetailLevel;

        /// <summary>
        /// A 16-bit unsigned integer that indicates the number of bytes of binary data contained in the pbChallenge field.
        /// </summary>
        public ushort cbChallenge;

        /// <summary>
        /// Contains the decrypted Client Platform Challenge data sent by the server in the Server Platform Challenge message.
        /// </summary>
        [Size("cbChallenge")]
        public byte[] pbChallenge;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.6.1 New License Information (NEW_LICENSE_INFO)
    /// The New License Information packet contains the actual client license and associated indexing information.
    /// </summary>
    public struct NEW_LICENSE_INFO
    {
        /// <summary>
        /// The content and format of this field are the same as the dwVersion field of the Product Information (section 2.2.2.1.1) structure.
        /// </summary>
        public uint dwVersion;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of bytes in the string contained in the pbScope field.
        /// </summary>
        public uint cbScope;

        /// <summary>
        /// Contains the NULL-terminated ANSI character set string giving the name of the issuer of this license. 
        /// </summary>
        [Size("cbScope")]
        public byte[] pbScope;

        /// <summary>
        /// The content and format of this field are the same as the cbCompanyName field of the Product Information structure.
        /// </summary>
        public uint cbCompanyName;

        /// <summary>
        /// The content and format of this field are the same as the pbCompanyName field of the Product Information structure.
        /// </summary>
        [Size("cbCompanyName/2")]
        public string pbCompanyName;

        /// <summary>
        /// The content and format of this field are the same as the cbProductId field of the Product Information structure.
        /// </summary>
        public uint cbProductId;

        /// <summary>
        /// The content and format of this field are the same as the pbProductId field of the Product Information structure.
        /// </summary>
        [Size("cbProductId/2")]
        public string pbProductId;

        /// <summary>
        /// A 32-bit unsigned integer that contains the number of bytes of binary data in the pbLicenseInfo field.
        /// </summary>
        public uint cbLicenseInfo;

        /// <summary>
        /// This field contains the CAL issued to the client by the license server. 
        /// </summary>
        [Size("cbLicenseInfo")]
        public byte[] pbLicenseInfo;
    }

    /// <summary>
    /// A variable-length licensing message whose structure depends on the value of the bMsgType field in the preamble structure. 
    /// </summary>
    [Union("preamble.bMsgType")]
    public struct LicensingMessage
    {
        [Case("bMsgType_Values.LICENSE_REQUEST")]
        public SERVER_LICENSE_REQUEST? ServerLicenseRequest;

        [Case("bMsgType_Values.NEW_LICENSE")]
        public CLIENT_NEW_LICENSE_REQUEST? ClientNewLicenseRequest;

        [Case("bMsgType_Values.LICENSE_INFO")]
        public CLIENT_LICENSE_INFO? ClientLicenseInfo;

        [Case("bMsgType_Values.PLATFORM_CHALLENGE")]
        public SERVER_PLATFORM_CHALLENGE? ServerPlatformChallenge;

        [Case("bMsgType_Values.PLATFORM_CHALLENGE_RESPONSE")]
        public CLIENT_PLATFORM_CHALLENGE_RESPONSE? ClientPlatformChallengeResponse;

        [Case("bMsgType_Values.UPGRADE_LICENSE")]
        public SERVER_UPGRADE_LICENSE? ServerUgradeLicense;

        [Case("bMsgType_Values.NEW_LICENSE")]
        public SERVER_NEW_LICENSE? ServerNewLicense;

        [Case("bMsgType_Values.ERROR_ALERT")]
        public LICENSE_ERROR_MESSAGE? LicenseError;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.1	Server License Request (SERVER_LICENSE_REQUEST)
    /// The Server License Request packet is sent to the client to initiate the RDP licensing handshake.
    /// </summary>
    public struct SERVER_LICENSE_REQUEST
    {
        /// <summary>
        /// A 32-byte array containing a random number. 
        /// </summary>
        [StaticSize(32)]
        public byte[] ServerRandom;

        /// <summary>
        /// A variable-length Product Information structure. 
        /// This structure contains the details of the product license required for connecting to the terminal server.
        /// </summary>
        public PRODUCT_INFO ProductInfo;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2) of type BB_KEY_EXCHG_ALG_BLOB (0x000D). 
        /// </summary>
        public LICENSE_BINARY_BLOB KeyExchangeList;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2) of type BB_CERTIFICATE_BLOB (0x0003). 
        /// </summary>
        public LICENSE_BINARY_BLOB ServerCertificate;

        /// <summary>
        /// A variable-length Scope List structure that contains a list of entities that issued the client license. 
        /// </summary>
        public SCOPE_LIST ScopeList;
    }

    /// <summary>
    /// A 32-bit unsigned integer that indicates the key exchange algorithm chosen by the client. 
    /// </summary>
    public enum KeyExchangeAlg: uint
    {
        None,

        /// <summary>
        /// indicates an RSA-based key exchange with a 512-bit asymmetric key
        /// </summary>
        KEY_EXCHANGE_ALG_RSA = 0x00000001
    }

    /// <summary>
    /// The most significant byte of the PlatformId field contains the operating system version of the client.
    /// </summary>
    public enum Client_OS_ID: uint
    {
        None,

        /// <summary>
        /// The client operating system version is 3.51.
        /// </summary>
        CLIENT_OS_ID_WINNT_351 = 0x01000000,

        /// <summary>
        /// The client operating system version is 4.00.
        /// </summary>
        CLIENT_OS_ID_WINNT_40 = 0x02000000,

        /// <summary>
        /// The client operating system version is 5.00.
        /// </summary>
        CLIENT_OS_ID_WINNT_50 = 0x03000000,

        /// <summary>
        /// The client operating system version is 5.20 or later.
        /// </summary>
        CLIENT_OS_ID_WINNT_POST_52 = 0x04000000
    }

    /// <summary>
    /// The second most significant byte of the PlatformId field identifies the ISV that provided the client image.
    /// </summary>
    public enum Client_Image_ID: uint
    {
        None,

        /// <summary>
        /// The ISV for the client image is Microsoft.
        /// </summary>
        CLIENT_IMAGE_ID_MICROSOFT = 0x00010000,

        /// <summary>
        /// The ISV for the client image is Citrix.
        /// </summary>
        CLIENT_IMAGE_ID_CITRIX = 0x00020000
    }
    /// <summary>
    /// [MS-RDPELE] 2.2.2.2	Client New License Request (CLIENT_NEW_LICENSE_REQUEST)
    /// The Client New License Request packet is sent to a server when the client cannot find a license matching the product information 
    /// provided in the Server License Request message. 
    /// </summary>
    public struct CLIENT_NEW_LICENSE_REQUEST
    {
        /// <summary>
        /// A 32-bit unsigned integer that indicates the key exchange algorithm chosen by the client. 
        /// It MUST be set to KEY_EXCHANGE_ALG_RSA (0x00000001), which indicates an RSA-based key exchange with a 512-bit asymmetric key.
        /// </summary>
        public KeyExchangeAlg PreferredKeyExchangeAlg;

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This field is composed of two identifiers: the operating system identifier and the independent software vendor (ISV) identifier. 
        /// </summary>
        public uint PlatformId;

        /// <summary>
        /// A 32-byte random number generated by the client using a cryptographically secure pseudo-random number generator. 
        /// </summary>
        [StaticSize(32)]
        public byte[] ClientRandom;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2) of type BB_RANDOM_BLOB (0x0002). 
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedPreMasterSecret;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2) of type BB_CLIENT_USER_NAME_BLOB (0x000F).
        /// </summary>
        public LICENSE_BINARY_BLOB ClientUserName;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2) of type BB_CLIENT_MACHINE_NAME_BLOB (0x0010). 
        /// </summary>
        public LICENSE_BINARY_BLOB ClientMachineName;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.3	Client License Information (CLIENT_LICENSE_INFO)
    /// The Client License Information packet is sent by a client that already has a license issued to it 
    /// in response to the Server License Request (section 2.2.2.1) message.
    /// </summary>
    public struct CLIENT_LICENSE_INFO
    {
        /// <summary>
        /// The content and format of this field are the same as the PreferredKeyExchangeAlg field of the Client New License Request (section 2.2.2.2) message.
        /// </summary>
        public KeyExchangeAlg PreferredKeyExchangeAlg;

        /// <summary>
        /// The content and format of this field are the same as the PlatformId field of the Client New License Request message.
        /// </summary>
        public uint PlatformId;

        /// <summary>
        /// The content and format of this field are the same as the ClientRandom field of the Client New License Request message.
        /// </summary>
        [StaticSize(32)]
        public byte[] ClientRandom;

        /// <summary>
        /// The content and format of this field are the same as the EncryptedPreMasterSecret field of the Client New License Request message.
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedPreMasterSecret;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2) of type BB_DATA_BLOB (0x0001). 
        /// This BLOB contains the CAL (see the pbLicenseInfo field in section 2.2.2.6.1) that is retrieved from the client's license store.
        /// </summary>
        public LICENSE_BINARY_BLOB LicenseInfo;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2). 
        /// This BLOB contains a Client Hardware Identification (section 2.2.2.3.1) structure encrypted with the licensing encryption keys (see section 5.1.2), 
        /// using RC4 (for instructions on how to perform the encryption, see section 5.1.3).
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedHWID;

        /// <summary>
        /// An array of 16 bytes containing an MD5 digest (Message Authentication Code (MAC)) 
        /// that is generated over the unencrypted Client Hardware Identification structure. 
        /// </summary>
        [StaticSize(16)]
        public byte[] MACData;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.4	Server Platform Challenge (SERVER_PLATFORM_CHALLENGE)
    /// The Server Platform Challenge packet is sent from the server to the client after receiving the Client New License Request (section 2.2.2.2) 
    /// or certain cases of Client License Information (section 2.2.2.3). 
    /// </summary>
    public struct SERVER_PLATFORM_CHALLENGE
    {
        /// <summary>
        /// Reserved
        /// </summary>
        public uint ConnectFlags;

        /// <summary>
        /// A Licensing Binary BLOB structure (see [MS-RDPBCGR] section 2.2.1.12.1.2). 
        /// This BLOB contains the encrypted server platform challenge data.
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedPlatformChallenge;

        /// <summary>
        /// An array of 16 bytes containing an MD5 digest (MAC) generated over the unencrypted platform challenge BLOB. 
        /// </summary>
        [StaticSize(16)]
        public byte[] MACData;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.5	Client Platform Challenge Response (CLIENT_PLATFORM_CHALLENGE_RESPONSE)
    /// The Client Platform Challenge Response packet is sent by the client in response to the Server Platform Challenge (section 2.2.2.4) message.
    /// </summary>
    public struct CLIENT_PLATFORM_CHALLENGE_RESPONSE
    {
        /// <summary>
        /// A LICENSE_BINARY_BLOB<13> structure (as specified in [MS-RDPBCGR] section 2.2.1.12.1.2) of wBlobType BB_ENCRYPTED_DATA_BLOB (0x0009). 
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedPlatformChallengResponse;

        /// <summary>
        /// A LICENSE_BINARY_BLOB structure (as specified in [MS-RDPBCGR] section 2.2.1.12.1.2) of wBlobType BB_ENCRYPTED_DATA_BLOB (0x0009). 
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedHWID;

        /// <summary>
        /// An array of 16 bytes containing an MD5 digest (MAC) generated over the Platform Challenge Response Data and decrypted Client Hardware Identification. 
        /// </summary>
        [StaticSize(16)]
        public byte[] MACData;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.6	Server Upgrade License (SERVER_UPGRADE_LICENSE)
    /// The Server Upgrade License packet is sent from the server to the client 
    /// if the client presents an existing license and the server determines that this license SHOULD be upgraded. 
    /// </summary>
    public struct SERVER_UPGRADE_LICENSE
    {
        /// <summary>
        /// A LICENSE_BINARY_BLOB structure (as specified in [MS-RDPBCGR] section 2.2.1.12.1.2) of wBlobType BB_ENCRYPTED_DATA_BLOB (0x0009).
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedLicenseInfo;

        /// <summary>
        /// An array of 16 bytes containing an MD5 digest (Message Authentication Code) generated over the unencrypted New License Information structure. 
        /// </summary>
        [StaticSize(16)]
        public byte[] MACData;
    }

    /// <summary>
    /// [MS-RDPELE] 2.2.2.7	Server New License (SERVER_NEW_LICENSE)
    /// The Server New License message is sent from the server to the client when a new license is issued to the client. 
    /// The structure and the content of this message are the same as the Server Upgrade License message.
    /// </summary>
    public struct SERVER_NEW_LICENSE
    {
        /// <summary>
        /// A LICENSE_BINARY_BLOB structure (as specified in [MS-RDPBCGR] section 2.2.1.12.1.2) of wBlobType BB_ENCRYPTED_DATA_BLOB (0x0009).
        /// </summary>
        public LICENSE_BINARY_BLOB EncryptedLicenseInfo;

        /// <summary>
        /// An array of 16 bytes containing an MD5 digest (Message Authentication Code) generated over the unencrypted New License Information structure. 
        /// </summary>
        [StaticSize(16)]
        public byte[] MACData;
    }
}
