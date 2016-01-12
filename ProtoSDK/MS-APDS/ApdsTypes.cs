// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Apds
{
    /// <summary>
    ///  The DIGEST_VALIDATION_RESP message is a response to
    ///  a DIGEST_VALIDATION_REQ message.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-APDS\9999546b-5de9-4177-8654-80cc76cd9650.xml
    //  </remarks>
    public partial struct DIGEST_VALIDATION_RESP
    {

        /// <summary>
        ///  A 32-bit unsigned integer that MUST specify the Digest
        ///  validation message type. This member MUST be 0x0000000A.
        /// </summary>        
        public MessageType_Values MessageType;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the version
        ///  of the Digest validation protocol. The protocol version
        ///  defined in this document is 1. The value of this member
        ///  MUST be 0x0001.
        /// </summary>        
        public Version_Values Version;

        /// <summary>
        ///  An unused 16-bit unsigned integer. MUST be set to zero
        ///  when sent and MUST be ignored on receipt.
        /// </summary>        
        public Pad2_Values Pad2;

        /// <summary>
        ///  A 32-bit unsigned integer specifying if the Digest authentication
        ///  data sent in the DIGEST_VALIDATION_REQ (section) was
        ///  successfully verified by the domain controller. On
        ///  successful validation, the Status field MUST be set
        ///  to STATUS_SUCCESS. On failure, it MUST be set to STATUS_LOGON_FAILURE.
        ///  Both values are as specified in [MS-ERREF] section
        ///  .
        /// </summary>        
        public uint Status;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the number
        ///  of bytes of the SessionKey field in the DIGEST_VALIDATION_RESP
        ///  message (section) plus a terminating null character.
        ///  It MUST be equal to 33.
        /// </summary>        
        public ushort SessionKeyLength;

        /// <summary>
        ///  An unused 16-bit unsigned integer. MUST be set to zero
        ///  when sent and MUST be ignored on receipt.
        /// </summary>        
        public Pad3_Values Pad3;

        /// <summary>
        ///  A 32-bit unsigned integer that MUST specify the number
        ///  of bytes of the AuthData field in the DIGEST_VALIDATION_RESP
        ///  message (section ).
        /// </summary>        
        public uint AuthDataSize;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the number
        ///  of bytes of the AccountName field in the DIGEST_VALIDATION_RESP
        ///  message (section ).
        /// </summary>        
        public ushort AcctNameSize;

        /// <summary>
        ///  A 16-bit unsigned integer field reserved for future
        ///  use. MUST be set to zero when sent and MUST be ignored
        ///  on receipt.
        /// </summary>        
        public Reserved1_Values Reserved1;

        /// <summary>
        ///  A 32-bit unsigned integer that MUST specify the number
        ///  of bytes in the entire DIGEST_VALIDATION_RESP message
        ///  (section ).
        /// </summary>        
        public uint MessageSize;

        /// <summary>
        ///  A 32-bit unsigned integer field reserved for future
        ///  use. MUST be set to zero when sent and MUST be ignored
        ///  on receipt.
        /// </summary>        
        public Reserved3_Values Reserved3;

        /// <summary>
        ///  A 32-byte buffer that MUST contain the Digest SessionKey
        ///  ([RFC2617] section 3.2.2.2).
        /// </summary>
        [StaticSize(32, StaticSizeMode.Elements)]
        public byte[] SessionKey;

        /// <summary>
        ///  A single byte to terminate the SessionKey. MUST be set
        ///  to zero.
        /// </summary>
        public SessionKey_NULL_terminator_Values SessionKey_NULL_terminator;

        /// <summary>
        ///  An unused 7-byte padding. The value of each byte MUST
        ///  be set to zero when sent and MUST be ignored on receipt.
        /// </summary>
        [StaticSize(7, StaticSizeMode.Elements)]
        public Pad4_Values[] Pad4;

        /// <summary>
        ///  An unused 64-bit unsigned integer. MUST be set to zero
        ///  when sent and MUST be ignored on receipt.
        /// </summary>        
        public Pad1_Values Pad1;

        /// <summary>
        ///  The AuthData field MUST contain a PACTYPE structure
        ///  ([MS-PAC] section ). The length of the PACTYPE structure
        ///  MUST be specified by the AuthDataSize field. The length
        ///  of this field MUST be 0 if the value of the Status
        ///  field is STATUS_LOGON_FAILURE.
        /// </summary>
        [Size("AuthDataSize")]
        public byte[] AuthData;

        /// <summary>
        ///  The AccountName field MUST contain the NetBIOS name
        ///  of the user's account. The length of AccountName MUST
        ///  be specified by the AcctNameSize field.
        /// </summary>
        [Size("AcctNameSize")]
        public byte[] AccountName;
    }

    /// <summary>
    /// An unsigned 32-bit value describing the message type. This member MUST be set to 0x0000000A.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum MessageType_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0x0000000A,
    }

    /// <summary>
    /// A 16-bit unsigned integer that MUST specify the version of the Digest validation protocol. 
    /// The protocol version defined in this document is 1. The value of this member MUST be 0x0001.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum Version_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0x0001,
    }

    /// <summary>
    /// An unused 16-bit unsigned integer. MUST be set to zero when sent and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum Pad2_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// An unused 16-bit unsigned integer. MUST be set to zero when sent and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum Pad3_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// A 16-bit unsigned integer field reserved for future use. MUST be set to zero when sent 
    /// and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum Reserved1_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// A 32-bit unsigned integer field reserved for future use. MUST be set to zero when sent 
    /// and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum Reserved3_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// A single byte to terminate the SessionKey. MUST be set to zero.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SessionKey_NULL_terminator_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// An unused 7-byte padding. The value of each byte MUST be set to zero when sent 
    /// and MUST be ignored on receipt
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Pad4_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// An unused 64-bit unsigned integer. MUST be set to zero when sent 
    /// and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum Pad1_Values : ulong
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    ///  The following diagram shows the format of the message
    ///  used for PAC validation.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-APDS\c283e05d-a611-4166-a1e6-d4e55ef68ce2.xml
    //  </remarks>
    public partial struct KERB_VERIFY_PAC_REQUEST
    {

        /// <summary>
        ///  An unsigned 32-bit value describing the message type.
        ///  This member MUST be set to 0x00000003.
        /// </summary>        
        public KERB_VERIFY_PAC_REQUEST_MessageType_Values MessageType;

        /// <summary>
        ///  An unsigned 32-bit value that MUST contain the signature
        ///  length of the PAC_SIGNATURE_DATA Signature value ([MS-PAC]
        ///  section) for the Server Signature ([MS-PAC] section
        ///  ) in the privilege attribute certificate (PAC).
        /// </summary>        
        public uint ChecksumLength;

        /// <summary>
        ///  An unsigned 32-bit value that MUST contain the PAC_SIGNATURE_DATA
        ///  SignatureType value ([MS-PAC] section) for the Key
        ///  Distribution Center (KDC) Signature ([MS-PAC] section
        ///  ) in the PAC.
        /// </summary>        
        public uint SignatureType;

        /// <summary>
        ///  An unsigned 32-bit value that MUST contain the signature
        ///  length of the PAC_SIGNATURE_DATA Signature value ([MS-PAC]
        ///  section) in the KDC Signature ([MS-PAC] section )
        ///  in the PAC.
        /// </summary>        
        public uint SignatureLength;

        /// <summary>
        ///  The PAC_SIGNATURE_DATA Signature value ([MS-PAC] section
        ///  ) for the Server Signature ([MS-PAC] section) in the
        ///  PAC. It MUST be followed by the PAC_SIGNATURE_DATA
        ///  Signature value ([MS-PAC] section) for the KDC Signature
        ///  ([MS-PAC] section) in the PAC.
        /// </summary>
        [Size("ChecksumLength + SignatureLength")]
        public byte[] ChecksumAndSignature;
    }

    /// <summary>
    /// An unsigned 32-bit value describing the message type. 
    /// This member MUST be set to 0x00000003.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum KERB_VERIFY_PAC_REQUEST_MessageType_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0x00000003,
    }

    /// <summary>
    ///  Payload fields
    /// </summary>
    //  <remarks>
    //   .\TD\MS-APDS\d710846e-2bb6-4cef-b9e5-a0b3027cc23b.xml
    //  </remarks>
    public partial struct DIGEST_VALIDATION_REQ_Payload
    {

        /// <summary>
        ///  The user name value from the digest-response message
        ///  that MUST be as specified in [RFC2617] section 3.2.2.
        /// </summary>
        public string Username;

        /// <summary>
        ///  The realm value that MUST be as specified in [RFC2617]
        ///  section 3.2.1.
        /// </summary>
        public string Realm;

        /// <summary>
        ///  The nonce value from the digest-challenge message that
        ///  MUST be as specified in [RFC2617] section 3.2.1.
        /// </summary>
        public string Nonce;

        /// <summary>
        ///  The cnonce value from the digest-response message that
        ///  MUST be as specified in [RFC2617] section 3.2.2.
        /// </summary>        
        public string CNonce;

        /// <summary>
        ///  The nc-value from the digest-response message, that
        ///  MUST be as specified in [RFC2617], section 3.2.2.
        /// </summary>
        public string NonceCount;

        /// <summary>
        ///  The algorithm value from the digest-response message
        ///  that MUST be as specified in [RFC2617] section 3.2.1.
        /// </summary>
        public string Algorithm;

        /// <summary>
        ///  The QOP value from the digest-response message that
        ///  MUST be as specified in [RFC2617] section 3.2.2.
        /// </summary>
        public string QOP;

        /// <summary>
        ///  Method by which Digest authentication information MUST
        ///  be transmitted as part of the HTTP1.1 protocol. The
        ///  string value is GET or PUT if Digest authentication
        ///  is used for the HTTP1.1 protocol [RFC2617]. The string
        ///  value is AUTHENTICATE if Digest authentication is used
        ///  as an SASL mechanism [RFC2617].
        /// </summary>
        public string Method;

        /// <summary>
        ///  The digest-URI value from the digest-response message
        ///  that MUST be as specified in [RFC2617] section 3.2.2.
        /// </summary>
        public string URI;

        /// <summary>
        ///  The response value from the digest-response message
        ///  that MUST be as specified in [RFC2617] section 3.2.2.
        /// </summary>
        public string Response;

        /// <summary>
        ///  The H (entity-body) value that MUST be as specified
        ///  in [RFC2617] section 3.2.2.3.
        /// </summary>
        public string Hentity;

        /// <summary>
        ///  The Authzid value from the digest-response message that
        ///  MUST be as specified in [RFC2831] section 2.1.2.
        /// </summary>
        public string Authzid;

        /// <summary>
        ///  A Unicode string that MUST specify the user account
        ///  name.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  A Unicode string that MUST specify the domain to which
        ///  the user account belongs.
        /// </summary>
        public string Domain;

        /// <summary>
        ///  A Unicode string that MUST specify the NetBIOS name
        ///  of the server that sent the DIGEST_VALIDATION_REQ message
        ///  (section ).
        /// </summary>
        public string ServerName;

        /// <summary>
        /// Read null terminate string
        /// </summary>
        /// <param name="payloadBytes">payload byte array</param>
        /// <param name="offset">reading offset value</param>
        /// <param name="textEncoding">text encoding type</param>
        /// <returns></returns>
        private static string ReadNullTerminateString(
            byte[] payloadBytes,
            ref uint offset,
            Encoding textEncoding
            )
        {
            if (offset >= payloadBytes.Length - 1)
            {
                throw new ArgumentException(
                    "invalid offset value",
                    "offset");
            }

            int readBytes = 0;
            int count = (int)(payloadBytes.Length - offset);
            byte[] nullBytes = textEncoding.GetBytes(ApdsUtility.NULL_TERMINATOR_STRING);
            StringBuilder sb = new StringBuilder();
            using (BinaryReader binaryReader = new BinaryReader(
                    new MemoryStream(payloadBytes, (int)offset, count)))
            {
                while (true)
                {
                    byte[] data = binaryReader.ReadBytes(nullBytes.Length);
                    readBytes += nullBytes.Length;
                    if (data == null)
                    {
                        throw new ArgumentException(
                            "invalid payload byte arrays",
                            "payloadBytes");
                    }

                    if (ArrayUtility.CompareArrays<byte>(data, nullBytes))
                    {
                        break;
                    }
                    else
                    {
                        sb.Append(textEncoding.GetString(data));
                    }
                }
            }

            offset = offset + (uint)readBytes;
            return sb.ToString();
        }

        /// <summary>
        /// Convert byte array to payload structure
        /// </summary>
        /// <param name="payloadData"></param>
        /// <returns>payload structure</returns>
        public static DIGEST_VALIDATION_REQ_Payload Parse(byte[] payloadData)
        {
            if (payloadData == null)
            {
                throw new ArgumentNullException("payloadData");
            }

            DIGEST_VALIDATION_REQ_Payload payload = new DIGEST_VALIDATION_REQ_Payload();
            Encoding payloadTextEncoding = Encoding.GetEncoding(ApdsUtility.ISO_8859_1);

            uint offset = 0;
            payload.Username = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Realm = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Nonce = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.CNonce = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.NonceCount = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Algorithm = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.QOP = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Method = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.URI = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Response = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Hentity = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Authzid = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);

            payloadTextEncoding = Encoding.Unicode;
            payload.AccountName = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.Domain = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);
            payload.ServerName = ReadNullTerminateString(payloadData, ref offset, payloadTextEncoding);

            return payload;
        }

        /// <summary>
        /// Convert payload to byte array
        /// </summary>
        /// <returns>byte array of payload structure</returns>
        public byte[] GetBytes()
        {
            byte[] payload;
            byte[] value;

            // strings MUST be encoded by using [ISO/IEC-8859-1], unless specified as Unicode
            Encoding payloadTextEncoding = Encoding.GetEncoding(ApdsUtility.ISO_8859_1);

            payload = ApdsUtility.ConvertStringToByteArray(Username, payloadTextEncoding);
            value = ApdsUtility.ConvertStringToByteArray(Realm, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(Nonce, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(CNonce, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(NonceCount, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(Algorithm, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(QOP, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(Method, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(URI, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(Response, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(Hentity, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(Authzid, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);

            // the last three strings of payload are Unicode strings, 
            // account name, domain, server name
            payloadTextEncoding = Encoding.Unicode;
            value = ApdsUtility.ConvertStringToByteArray(AccountName, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(Domain, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);
            value = ApdsUtility.ConvertStringToByteArray(ServerName, payloadTextEncoding);
            payload = ArrayUtility.ConcatenateArrays(payload, value);

            return payload;
        }
    }

    /// <summary>
    ///  The DIGEST_VALIDATION_REQ message defines a request
    ///  to validate the input from the Digest Protocol Extensions[MS-DPSP]
    ///  and retrieve user authorization information.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-APDS\d710846e-2bb6-4cef-b9e5-a0b3027cc23b.xml
    //  </remarks>
    public partial struct DIGEST_VALIDATION_REQ
    {

        /// <summary>
        ///  A 32-bit unsigned integer that defines the Digest validation
        ///  message type. This member MUST be set to 0x0000001A.
        /// </summary>        
        public DIGEST_VALIDATION_REQ_MessageType_Values MessageType;

        /// <summary>
        ///  A 16-bit unsigned integer that defines the version of
        ///  the Digest validation protocol. The protocol version
        ///  defined in this document is 1 (the value of this member
        ///  MUST be 0x0001).
        /// </summary>        
        public DIGEST_VALIDATION_REQ_Version_Values Version;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the total
        ///  number of bytes in the DIGEST_VALIDATION_REQ message
        ///  ().
        /// </summary>        
        public ushort MsgSize;

        /// <summary>
        ///  A 16-bit unsigned integer that specifies the Digest
        ///  protocol used, which MUST be one of the following:
        /// </summary>        
        public DigestType_Values DigestType;

        /// <summary>
        ///  A 16-bit unsigned integer specifying the Quality of
        ///  Protection (QoP) requested by the Digest client ([RFC2617]
        ///  section 3.2.1 and [RFC2831] section 2.1.2.1) that MUST
        ///  be one of the following:
        /// </summary>        
        public QopType_Values QopType;

        /// <summary>
        ///  A 16-bit unsigned integer specifying the algorithm value
        ///  specified by the Digest client in the digest-challenge
        ///  message ([RFC2617] section 3.2.1) that MUST be one
        ///  of the following values:
        /// </summary>        
        public AlgType_Values AlgType;

        /// <summary>
        ///  A 16-bit unsigned integer specifying the type of encoding
        ///  used for username and password fields that MUST be
        ///  one of the following (as specified in [RFC2831] section
        ///  2.1.1 and [MS-DPSP] section ):
        /// </summary>        
        public CharsetType_Values CharsetType;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the number
        ///  of bytes in the Payload field of the DIGEST_VALIDATION_REQ
        ///  message, and MUST NOT exceed the total size in MsgSize.
        /// </summary>        
        public ushort CharValuesLength;

        /// <summary>
        ///  A 16-bit unsigned integer specifying the format of the
        ///  user AccountName field (in the DIGEST_VALIDATION_REQ
        ///  message), and MUST be one of the following:
        /// </summary>        
        public NameFormat_Values NameFormat;

        /// <summary>
        ///  A two-byte set of bit flags providing additional instructions
        ///  for processing the DIGEST_VALIDATION_REQ message (section
        ///  ) by the DC. The Flags field is constructed from one
        ///  or more bit flags from the following table, with the
        ///  exception of the constraint on bit C.All other bits
        ///  MUST be set to zero and MUST be ignored upon receipt.
        /// </summary>
        public DIGEST_VALIDATION_FLAGS Flags;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the length
        ///  of the AccountName field in the Payload buffer.
        /// </summary>        
        public ushort AccountNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the length
        ///  of the Domain field in the Payload buffer.
        /// </summary>        
        public ushort DomainLength;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST specify the length
        ///  of the ServerName field in the Payload buffer.
        /// </summary>        
        public ushort ServerNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer field reserved for future
        ///  use. MUST be set to zero when sent and MUST be ignored
        ///  on receipt.
        /// </summary>        
        public DIGEST_VALIDATION_REQ_Reserved3_Values Reserved3;

        /// <summary>
        ///  A 16-bit unsigned integer field reserved for future
        ///  use. MUST be set to zero when sent and MUST be ignored
        ///  on receipt.
        /// </summary>        
        public Reserved4_Values Reserved4;

        /// <summary>
        ///  An unused, 64-bit unsigned integer. MUST be set to zero
        ///  when sent and MUST be ignored on receipt.
        /// </summary>        
        public DIGEST_VALIDATION_REQ_Pad1_Values Pad1;

        /// <summary>
        /// nested digest validation req payload
        /// </summary>
        [Size("CharValuesLength")]
        public byte[] Payload;
    }

    /// <summary>
    /// A 32-bit unsigned integer that defines the Digest validation message type. 
    /// This member MUST be set to 0x0000001A.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum DIGEST_VALIDATION_REQ_MessageType_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0x0000001A,
    }

    /// <summary>
    /// A 16-bit unsigned integer that defines the version of the Digest validation protocol. 
    /// The protocol version defined in this document is 1 (the value of this member MUST be 0x0001).
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum DIGEST_VALIDATION_REQ_Version_Values : ushort
    {

        /// <summary>
        ///  Version 1
        /// </summary>
        Default = 0x0001,
    }

    /// <summary>
    /// A 16-bit unsigned integer that specifies the Digest protocol used, 
    /// which MUST be one of the following:
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]    
    public enum DigestType_Values : ushort
    {

        /// <summary>
        ///  Using the Digest authentication mechanism [RFC2617]
        ///  for the HTTP/1.1 Protocol.
        /// </summary>
        Basic = 0x0003,

        /// <summary>
        ///  Using Digest authentication as a Simple Authentication
        ///  and Security Layer (SASL) mechanism [RFC2831].
        /// </summary>
        SASL = 0x0004,
    }

    /// <summary>
    /// A 16-bit unsigned integer specifying the Quality of Protection (QoP) 
    /// requested by the Digest client ([RFC2617] section 3.2.1 and [RFC2831] section 2.1.2.1)
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]    
    public enum QopType_Values : ushort
    {

        /// <summary>
        ///  The Digest client did not specify a QoP. For backward
        ///  compatibility with Digest Access authentication [RFC2069],
        ///  Digest authentication ([RFC2617] made the QoP optional.
        /// </summary>
        None = 0x0001,

        /// <summary>
        ///  Authentication only. Represents auth ([RFC2617] section
        ///  3.2.1 and [RFC2831] section 2.1.1).
        /// </summary>
        Authenticate = 0x0002,

        /// <summary>
        ///  Authentication and integrity protection. Represents
        ///  auth-int ([RFC2617] section 3.2.1 and [RFC2831] section
        ///  2.1.1).
        /// </summary>
        AuthenticateAndIntegrity = 0x0003,

        /// <summary>
        ///  Authentication with integrity protection and encryption.
        ///  Represents auth-conf ([RFC2831] section 2.1.1).
        /// </summary>
        AuthenticateWithIntegrityProtectionAndEncryption = 0x0004,
    }

    /// <summary>
    /// A 16-bit unsigned integer specifying the algorithm value 
    /// specified by the Digest client in the digest-challenge message ([RFC2617] section 3.2.1)
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]    
    public enum AlgType_Values : ushort
    {

        /// <summary>
        ///  MD5 assumed; the algorithm was not present ([RFC2617]
        ///  section 3.2.1).
        /// </summary>
        NotPresentAndMD5Assumed = 0x0001,

        /// <summary>
        ///  MD5 value to produce the digest and checksum ([RFC2617]
        ///  section 2.1.1).
        /// </summary>
        MD5DigestAndChecksum = 0x0002,

        /// <summary>
        ///  MD5-sess value to produce the digest and checksum ([RFC2617]
        ///  section 3.2.1 and [RFC2831] section 2.1.1).
        /// </summary>
        MD5SessDigestAndChecksum = 0x0003,
    }

    /// <summary>
    /// A 16-bit unsigned integer specifying the type of encoding used 
    /// for username and password fields that MUST be one of the following 
    /// (as specified in [RFC2831] section 2.1.1 and [MS-DPSP] section 2.2)
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]    
    public enum CharsetType_Values : ushort
    {

        /// <summary>
        ///  ISO8859-1 encoding is used for username and password
        ///  fields.
        /// </summary>
        ISO8859_1 = 0x0001,

        /// <summary>
        ///  UTF-8 encoding is used for username and password fields.
        /// </summary>
        UTF8 = 0x0002,
    }

    /// <summary>
    /// A 16-bit unsigned integer specifying the format of the user AccountName field 
    /// (in the DIGEST_VALIDATION_REQ message)
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum NameFormat_Values : ushort
    {

        /// <summary>
        ///  Digest server cannot determine the format of the user's
        ///  AccountName.
        /// </summary>
        None = 0x0000,

        /// <summary>
        ///  A format determined to be the SAM account name ([MS-ADA3]).
        /// </summary>
        SAM = 0x0001,

        /// <summary>
        ///  A format determined to be the user principal name (UPN)
        ///  for the account ([MS-ADA3]).
        /// </summary>
        UPN = 0x0002,

        /// <summary>
        ///  A format determined to be NetBIOS ([MS-ADA3]).
        /// </summary>
        NetBios = 0x0003,
    }

    /// <summary>
    /// A 16-bit unsigned integer field reserved for future use. 
    /// MUST be set to zero when sent and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum DIGEST_VALIDATION_REQ_Reserved3_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// A 16-bit unsigned integer field reserved for future use. 
    /// MUST be set to zero when sent and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum Reserved4_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// An unused, 64-bit unsigned integer. MUST be set to zero when sent and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum DIGEST_VALIDATION_REQ_Pad1_Values : ulong
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        Default = 0,
    }

    /// <summary>
    /// two-byte set of bit flags providing additional instructions 
    /// for processing the DIGEST_VALIDATION_REQ message(section 2.2.3.1) by the DC
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]    
    public enum DIGEST_VALIDATION_FLAGS : ushort
    {

        /// <summary>
        /// None 
        /// </summary>
        None = 0,

        /// <summary>
        /// A flag: 
        /// The format of Username and Realm (carried in the Payload field of DIGEST_VALIDATION_REQ)
        /// MUST be determined by the DC
        /// </summary>
        FormatOfUserNameAndRealmIsDeterminedByDC = 0x0001,

        /// <summary>
        /// B flag: 
        /// The optional Authzid field ([RFC2831] section 2.1.2) is not set and 
        /// carried in the Payload buffer in the DIGEST_VALIDATION_REQ message (section 2.2.3.1).
        /// </summary>
        OptionalAuthzIdIsSetAndCarriedInPayload = 0x0002,

        /// <summary>
        /// C flag: 
        /// Indicates that this request is from a server, 
        /// so group memberships are to be expanded for the Account's PAC. 
        /// This bit MUST NOT be set 
        /// if this request is forwarded from a server's domain to user account's domain.
        /// </summary>
        RequestIsSentFromServer = 0x0004,

        /// <summary>
        /// D flag: 
        /// Indicates if a single backslash is found in the username value ([RFC2617] section 3.2.2).
        /// </summary>    
        SingleBackSlashIsFoundInUserNameValue = 0x0008,

        /// <summary>
        ///  E flag: 
        ///  Indicates the DC will attempt to validate the request 
        ///  with an un-escaped backslash ([MS-DPSP] section 2.2).
        /// </summary>
        DcWillAttemptToValidateRequestWithUnEscapedBackSlash = 0x0010
    }
}
