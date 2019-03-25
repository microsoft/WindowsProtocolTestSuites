// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using System;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    /// Facade of Pac utility.
    /// </summary>
    public static class PacUtility
    {
        /// <summary>
        /// The length of NTLM password length, defined in TD section 2.6.4.
        /// </summary>
        public const int NtlmPasswordLength = 16;

        /// <summary>
        /// The package name of NTLM, defined in TD endnote 9. 
        /// </summary>
        public const string NtlmPackageName = "NTLM";

        #region Public Methods



        public static PacType DecodePacType(byte[] buffer)
        {
            return new PacType(buffer);
        }



        /// <summary>
        /// Creates an empty KerbValidationInfo instance.
        /// </summary>
        /// <param name="native">native structure contains kerb validation information.</param>
        /// <returns>The created KerbValidationInfo instance.</returns>
        public static KerbValidationInfo CreateKerbValidationInfoBuffer(KERB_VALIDATION_INFO native)
        {
            KerbValidationInfo kerbInfo = new KerbValidationInfo();
            kerbInfo.NativeKerbValidationInfo = native;

            return kerbInfo;
        }


        /// <summary>
        /// Creates an PacClientInfo instance using the specified clientId and name.
        /// </summary>
        /// <param name="clientId">A FileTime that contains the Kerberos initial 
        /// ticket-granting ticket TGT authentication time, as specified in [RFC4120] 
        /// section 5.3.</param>
        /// <param name="name">The client's account name</param>
        /// <returns>The created PacClientInfo instance.</returns>
        public static PacClientInfo CreateClientInfoBuffer(_FILETIME clientId, string name)
        {
            PacClientInfo clientInfo = new PacClientInfo();
            clientInfo.NativePacClientInfo.ClientId = clientId;
            clientInfo.NativePacClientInfo.Name = name.ToCharArray();
            clientInfo.NativePacClientInfo.NameLength = (ushort)(name.Length * 2);

            return clientInfo;
        }


        /// <summary>
        /// Creates an UpnDnsInfo instance using the specified flag, upn and dnsDomain.
        /// </summary>
        /// <param name="flag">U means The user account object does not have the 
        /// userPrincipalName attribute ([MS-ADA3] section 2.348) set.</param>
        /// <param name="upn">The UPN information.</param>
        /// <param name="dnsDomain">The DNS information.</param>
        /// <returns>The created UpnDnsInfo instance.</returns>
        public static UpnDnsInfo CreateUpnDnsInfoBuffer(UPN_DNS_INFO_Flags_Values flag, string upn, string dnsDomain)
        {
            UpnDnsInfo upnDnsInfo = new UpnDnsInfo();
            upnDnsInfo.NativeUpnDnsInfo.Flags = flag;

            upnDnsInfo.Upn = upn;
            upnDnsInfo.DnsDomain = dnsDomain;

            return upnDnsInfo;
        }


        /// <summary>
        /// Creates an S4uDelegationInfo instance using the specified s4U2proxyTarget and 
        /// s4UTransitedServices.
        /// </summary>
        /// <param name="s4U2proxyTarget">the name of the principal to whom the application 
        /// can be constraint delegated.</param>
        /// <param name="s4UTransitedServices">The list of all services that configured to 
        /// be delegated.</param>
        /// <returns>The created S4uDelegationInfo instance.</returns>
        public static S4uDelegationInfo CreateS4uInfoBuffer(
            string s4U2proxyTarget,
            params string[] s4UTransitedServices)
        {
            S4uDelegationInfo s4uInfo = new S4uDelegationInfo();
            s4uInfo.NativeS4uDelegationInfo.S4U2proxyTarget = DtypUtility.ToRpcUnicodeString(s4U2proxyTarget);
            s4uInfo.NativeS4uDelegationInfo.TransitedListSize = (uint)s4UTransitedServices.Length;

            _RPC_UNICODE_STRING[] transitedServiceArray = new _RPC_UNICODE_STRING[s4UTransitedServices.Length];
            for (int i = 0; i < transitedServiceArray.Length; i++)
            {
                transitedServiceArray[i] = DtypUtility.ToRpcUnicodeString(s4UTransitedServices[i]);
            }
            s4uInfo.NativeS4uDelegationInfo.S4UTransitedServices = transitedServiceArray;

            return s4uInfo;
        }


        /// <summary>
        /// Creates an PacCredentialInfo instance using the specified Type and credentials.
        /// </summary>
        /// <param name="type">Encryption Type.</param>
        /// <param name="key">The encrypt key.</param>
        /// <param name="credentials">A list of security package supplemental credentials.</param>
        /// <returns>The created PacCredentialInfo instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Type is not defined.</exception>
        public static PacCredentialInfo CreateCredentialInfoBuffer(
            EncryptionType_Values type,
            byte[] key,
            _SECPKG_SUPPLEMENTAL_CRED[] credentials)
        {
            PacCredentialInfo credentialInfo = new PacCredentialInfo();
            credentialInfo.NativePacCredentialInfo.EncryptionType = type;

            _PAC_CREDENTIAL_DATA credentialData = new _PAC_CREDENTIAL_DATA();
            credentialData.CredentialCount = (uint)credentials.Length;
            credentialData.Credentials = credentials;

            credentialInfo.Encrypt(credentialData, key);

            return credentialInfo;
        }


        /// <summary>
        /// Creates an PacType instance using the specified signature types and PacInfoBuffers
        /// </summary>
        /// <param name="serverSignatureType">Server signature signatureType.</param>
        /// <param name="serverSignKey">The server signature key used to generate server signature.</param>
        /// <param name="kdcSignatureType">KDC signature Type.</param>
        /// <param name="kdcSignKey">The KDC signature key used to generate KDC signature.</param>
        /// <param name="pacInfoBuffers">A list of PacInfoBuffer used to create the PacType.
        /// Note: DO NOT include the signatures (server signature and KDC signature)!</param>
        /// <returns>The created PacType instance.</returns>
        public static PacType CreatePacType(
            PAC_SIGNATURE_DATA_SignatureType_Values serverSignatureType,
            byte[] serverSignKey,
            PAC_SIGNATURE_DATA_SignatureType_Values kdcSignatureType,
            byte[] kdcSignKey,
            params PacInfoBuffer[] pacInfoBuffers)
        {
            return new PacType(
                serverSignatureType,
                serverSignKey,
                kdcSignatureType,
                kdcSignKey,
                pacInfoBuffers);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Unmarshals data from an NDR-encoded byte array to an object.
        /// </summary>
        /// <typeparam name="T">Type of the decoded object.</typeparam>
        /// <param name="buffer">The byte array containing NDR-encoded data.</param>
        /// <param name="index">Begin of encoded data.</param>
        /// <param name="count">Size of encoded data.</param>
        /// <param name="formatStringOffset">Format string offset of the data Type.</param>
        /// <returns>The object containing data in the byte array.</returns>
        internal static T NdrUnmarshal<T>(byte[] buffer, int index, int count, int formatStringOffset, bool force32Bit = true, int alignment = 4) where T : struct
        {
            using (PickleMarshaller marshaller = new PickleMarshaller(FormatString.Pac))
            {
                return marshaller.Decode<T>(buffer, index, count, formatStringOffset, force32Bit, alignment);
            }
        }


        /// <summary>
        /// Marshals data from an object to an NDR-encoded byte array.
        /// </summary>
        /// <param name="obj">The pointer to an unmanaged block of memory
        /// which contains the content of object.</param>
        /// <param name="formatStringOffset">Format string offset of the data Type.</param>
        /// <returns>The byte array of NDR-encoded data.</returns>
        internal static byte[] NdrMarshal(IntPtr obj, int formatStringOffset)
        {
            using (PickleMarshaller marshaller = new PickleMarshaller(FormatString.Pac))
            {
                return marshaller.Encode(obj, formatStringOffset);
            }
        }


        /// <summary>
        /// Read an object from the specified buffer using Channel.
        /// </summary>
        /// <typeparam name="T">The type of the object. Vary length members
        /// must have attributes "StaticSize" or "Size" defined.</typeparam>
        /// <param name="buffer">The buffer contains object content.</param>
        /// <returns>The object read from buffer.</returns>
        internal static T MemoryToObject<T>(byte[] buffer)
        {
            return MemoryToObject<T>(buffer, 0, buffer.Length);
        }


        /// <summary>
        /// Read an object from the specified buffer using Channel,
        /// beginning from <paramref name="index"/> of the <paramref name="buffer"/>,
        /// totally read <paramref name="count"/> bytes.
        /// </summary>
        /// <typeparam name="T">The type of the object. Vary length members
        /// must have attributes "StaticSize" or "Size" defined.</typeparam>
        /// <param name="buffer">The buffer contains object content.</param>
        /// <param name="index">The zero-based index from which to begin reading.</param>
        /// <param name="count">The maximum number of bytes to be read from the buffer.</param>
        /// <returns>The object read from the buffer.</returns>
        internal static T MemoryToObject<T>(byte[] buffer, int index, int count)
        {
            using (MemoryStream stream = new MemoryStream(buffer, index, count, false))
            {
                using (Channel channel = new Channel(null, stream))
                {
                    return channel.Read<T>();
                }
            }
        }


        /// <summary>
        /// Write an object to the specified buffer using Channel.
        /// </summary>
        /// <typeparam name="T">The type of the object. Vary length members
        /// must have attributes "StaticSize" or "Size" defined.</typeparam>
        /// <param name="obj">The object to be written to the buffer.</param>
        /// <returns>The buffer contains object content.</returns>
        internal static byte[] ObjectToMemory<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (Channel channel = new Channel(null, stream))
                {
                    channel.Write(obj);
                }

                byte[] buffer = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        /// <summary>
        /// Align offset to specified align unit, for example 8.
        /// </summary>
        /// <param name="offset">An int value to be aligned to specified boundary.</param>
        /// <param name="alignUnit">The boundary unit, for example 8.
        /// Must not be Zero (0).</param>
        /// <returns>The aligned offset.</returns>
        internal static int AlignTo(int offset, int alignUnit)
        {
            if (alignUnit == 0)
            {
                throw new ArgumentNullException("alignUnit");
            }

            if (offset % alignUnit != 0)
            {
                int reminder = offset % alignUnit;
                offset += (alignUnit - reminder);
            }
            return offset;
        }
        #endregion
    }
}
