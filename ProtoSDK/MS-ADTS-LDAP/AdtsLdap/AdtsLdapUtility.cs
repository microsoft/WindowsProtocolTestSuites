// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// A helpful class for user to covert some special structures, etc.
    /// </summary>
    public static class AdtsLdapUtility
    {
        /// <summary>
        /// Two null bytes represents the null terminators of Unicode string.
        /// </summary>
        internal static readonly byte[] nullTerminator = { 0, 0 };

        /// <summary>
        /// A string "data" equals the member name of the blobs.
        /// </summary>
        internal const string Data = "data";


        /// <summary>
        /// Converts a memory of byte array to a struct object, then update the offset for next.
        /// </summary>
        /// <typeparam name="T">Constrained to uint,long,OpType,Guid,FILETIME.</typeparam>
        /// <param name="source">A byte array containing the destination object.</param>
        /// <param name="offset">
        ///         <para>in : The offset to the start of source byte array.</para>
        ///         <para>out: The offset to the end of source byte array.</para>
        /// </param>
        /// <param name="destination">The target object converted from byte array.</param>
        /// <exception cref="System.ArgumentNullException">thrown if source is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        internal static void BytesToObject<T>(byte[] source, ref uint offset, out T destination) where T : struct
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            destination = default(T);
            int sizeDestination = Marshal.SizeOf(destination);
            destination = TypeMarshal.ToStruct<T>(ArrayUtility.SubArray(source, (int)offset, sizeDestination));
            offset += (uint)sizeDestination;
        }


        /// <summary>
        /// Gets a byte array containing n Unicode string from a buffer
        /// </summary>
        /// <param name="inputBuffer">A byte array containing n string</param>
        /// <param name="offset">The offset in inputBuffer to search for strings</param>
        /// <param name="numberOfStrings">The number of strings to get</param>
        /// <returns>A byte array containing n strings</returns>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        internal static byte[] SubArrayWithStrings(byte[] inputBuffer, ref uint offset, uint numberOfStrings)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            uint offsetSave = offset;
            for (; numberOfStrings > 0; offset += 2)
            {
                if ((inputBuffer[offset] | inputBuffer[offset + 1]) == 0)
                {
                    numberOfStrings--;
                }
            }
            return ArrayUtility.SubArray(inputBuffer, (int)offsetSave, (int)offset - (int)offsetSave);
        }


        /// <summary>
        /// Decode a byte array to NETLOGON_LOGON_QUERY structure.
        /// </summary>
        /// <param name="buffer">Byte array format data</param>
        /// <returns>The NETLOGON_LOGON_QUERY structure</returns>
        public static NETLOGON_LOGON_QUERY DecodeNetlogonLogonQuery(byte[] buffer)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
            {
                NETLOGON_LOGON_QUERY netlogonQuery = new NETLOGON_LOGON_QUERY();
                int index;
                netlogonQuery.Opcode = (AdtsOperationCode)br.ReadUInt16();
                index = (int)br.BaseStream.Position;
                netlogonQuery.ComputerName = GetAsciiName(buffer, ref index);
                // Align to an even byte boundary
                if (index % 2 != 0)
                {
                    index++;
                }
                netlogonQuery.MailslotName = GetAsciiName(buffer, ref index);
                netlogonQuery.UnicodeComputerName = GetUnicodeName(buffer, ref index);
                br.BaseStream.Position = index;
                netlogonQuery.NtVersion = (NETLOGON_NT_VERSION)br.ReadUInt32();
                netlogonQuery.LmNtToken = br.ReadUInt16();
                netlogonQuery.Lm20Token = br.ReadUInt16();

                return netlogonQuery;
            }
        }


        /// <summary>
        /// Decode a byte array to NETLOGON_PRIMARY_RESPONSE structure.
        /// </summary>
        /// <param name="buffer">Byte array format data</param>
        /// <returns>The NETLOGON_PRIMARY_RESPONSE structure</returns>
        public static NETLOGON_PRIMARY_RESPONSE DecodeNetlogonPrimaryResponse(byte[] buffer)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
            {
                NETLOGON_PRIMARY_RESPONSE netlogonPrimaryResponse = new NETLOGON_PRIMARY_RESPONSE();
                int index;
                netlogonPrimaryResponse.Opcode = (AdtsOperationCode)br.ReadUInt16();
                index = (int)br.BaseStream.Position;
                netlogonPrimaryResponse.PrimaryDCName = GetAsciiName(buffer, ref index);
                // Align to an even byte boundary
                if (index % 2 != 0)
                {
                    index++;
                }
                netlogonPrimaryResponse.UnicodePrimaryDCName = GetUnicodeName(buffer, ref index);
                netlogonPrimaryResponse.UnicodeDomainName = GetUnicodeName(buffer, ref index);
                br.BaseStream.Position = index;
                netlogonPrimaryResponse.NtVersion = (NETLOGON_NT_VERSION)br.ReadUInt32();
                netlogonPrimaryResponse.LmNtToken = br.ReadUInt16();
                netlogonPrimaryResponse.Lm20Token = br.ReadUInt16();

                return netlogonPrimaryResponse;
            }
        }


        /// <summary>
        /// Decode a byte array to NETLOGON_SAM_LOGON_RESPONSE_NT40 structure.
        /// </summary>
        /// <param name="buffer">Byte array format data</param>
        /// <returns>The NETLOGON_SAM_LOGON_RESPONSE_NT40 structure</returns>
        public static NETLOGON_SAM_LOGON_RESPONSE_NT40 DecodeNetlogonSamLogonResponseNT40(byte[] buffer)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
            {
                NETLOGON_SAM_LOGON_RESPONSE_NT40 netlogonSamLogonResponseNT40 = new NETLOGON_SAM_LOGON_RESPONSE_NT40();
                int index;

                netlogonSamLogonResponseNT40.Opcode = (AdtsOperationCode)br.ReadUInt16();
                index = (int)br.BaseStream.Position;
                netlogonSamLogonResponseNT40.UnicodeLogonServer = GetUnicodeName(buffer, ref index);
                netlogonSamLogonResponseNT40.UnicodeUserName = GetUnicodeName(buffer, ref index);
                netlogonSamLogonResponseNT40.UnicodeDomainName = GetUnicodeName(buffer, ref index);
                br.BaseStream.Position = index;
                netlogonSamLogonResponseNT40.NtVersion = (NETLOGON_NT_VERSION)br.ReadUInt32();
                netlogonSamLogonResponseNT40.LmNtToken = br.ReadUInt16();
                netlogonSamLogonResponseNT40.Lm20Token = br.ReadUInt16();

                return netlogonSamLogonResponseNT40;
            }
        }


        /// <summary>
        /// Decode a byte array to NETLOGON_SAM_LOGON_RESPONSE structure.
        /// </summary>
        /// <param name="buffer">Byte array format data</param>
        /// <returns>The NETLOGON_SAM_LOGON_RESPONSE structure</returns>
        public static NETLOGON_SAM_LOGON_RESPONSE DecodeNetlogonSamLogonResponse(byte[] buffer)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
            {
                NETLOGON_SAM_LOGON_RESPONSE netlogonSamLogonResponse = new NETLOGON_SAM_LOGON_RESPONSE();
                int index;

                netlogonSamLogonResponse.Opcode = (AdtsOperationCode)br.ReadUInt16();
                index = (int)br.BaseStream.Position;
                netlogonSamLogonResponse.UnicodeLogonServer = GetUnicodeName(buffer, ref index);
                netlogonSamLogonResponse.UnicodeUserName = GetUnicodeName(buffer, ref index);
                netlogonSamLogonResponse.UnicodeDomainName = GetUnicodeName(buffer, ref index);
                netlogonSamLogonResponse.DomainGuid = new Guid(ArrayUtility.SubArray(buffer, index, 16));
                index += 16;
                netlogonSamLogonResponse.NullGuid = new Guid(ArrayUtility.SubArray(buffer, index, 16));
                index += 16;
                netlogonSamLogonResponse.DnsForestName = DecompressName(buffer, ref index);
                netlogonSamLogonResponse.DnsDomainName = DecompressName(buffer, ref index);
                netlogonSamLogonResponse.DnsHostName = DecompressName(buffer, ref index);
                br.BaseStream.Position = index;
                netlogonSamLogonResponse.DcIpAddress = new IPAddress(br.ReadUInt32());
                netlogonSamLogonResponse.Flags = (DS_FLAG)br.ReadUInt32();
                netlogonSamLogonResponse.NtVersion = (NETLOGON_NT_VERSION)br.ReadUInt32();
                netlogonSamLogonResponse.LmNtToken = br.ReadUInt16();
                netlogonSamLogonResponse.Lm20Token = br.ReadUInt16();

                return netlogonSamLogonResponse;
            }
        }


        /// <summary>
        /// Decode a byte array to NETLOGON_SAM_LOGON_RESPONSE_EX structure.
        /// </summary>
        /// <param name="buffer">Byte array format data</param>
        /// <param name="ntVer"></param>
        /// <returns>The NETLOGON_SAM_LOGON_RESPONSE_EX structure</returns>
        public static NETLOGON_SAM_LOGON_RESPONSE_EX DecodeNetlogonSamLogonResponseEx(
            byte[] buffer,
            NETLOGON_NT_VERSION ntVer)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
            {
                NETLOGON_SAM_LOGON_RESPONSE_EX netlogonSamLogonResponseEx = new NETLOGON_SAM_LOGON_RESPONSE_EX();
                int index;

                netlogonSamLogonResponseEx.Opcode = (AdtsOperationCode)br.ReadUInt16();
                netlogonSamLogonResponseEx.Sbz = br.ReadUInt16();
                netlogonSamLogonResponseEx.Flags = (DS_FLAG)br.ReadUInt32();
                netlogonSamLogonResponseEx.DomainGuid = new Guid(br.ReadBytes(16));
                index = (int)br.BaseStream.Position;
                netlogonSamLogonResponseEx.DnsForestName = DecompressName(buffer, ref index);
                netlogonSamLogonResponseEx.DnsDomainName = DecompressName(buffer, ref index);
                netlogonSamLogonResponseEx.DnsHostName = DecompressName(buffer, ref index);
                netlogonSamLogonResponseEx.NetbiosDomainName = DecompressName(buffer, ref index);
                netlogonSamLogonResponseEx.NetbiosComputerName = DecompressName(buffer, ref index);
                netlogonSamLogonResponseEx.UserName = DecompressName(buffer, ref index);
                netlogonSamLogonResponseEx.DcSiteName = DecompressName(buffer, ref index);
                netlogonSamLogonResponseEx.ClientSiteName = DecompressName(buffer, ref index);
                br.BaseStream.Position = index;
                if ((ntVer & NETLOGON_NT_VERSION.NETLOGON_NT_VERSION_5EX_WITH_IP) != 0)
                {
                    netlogonSamLogonResponseEx.DcSockAddrSize = br.ReadByte();
                    netlogonSamLogonResponseEx.DcSockAddr = TypeMarshal.ToStruct<AdtsSocketAddress>(br.ReadBytes(16));
                }
                if ((ntVer & NETLOGON_NT_VERSION.NETLOGON_NT_VERSION_WITH_CLOSEST_SITE) != 0)
                {
                    //TDI? cannot find this field in response packet.
                    //index = (int)br.BaseStream.Position;
                    //netlogonSamLogonResponseEx.NextClosestSiteName = DecompressName(buf, ref index);
                    //br.BaseStream.Position = index;
                }
                netlogonSamLogonResponseEx.NtVersion = (NETLOGON_NT_VERSION)br.ReadUInt32();
                netlogonSamLogonResponseEx.LmNtToken = br.ReadUInt16();
                netlogonSamLogonResponseEx.Lm20Token = br.ReadUInt16();

                return netlogonSamLogonResponseEx;
            }
        }


        /// <summary>
        /// Get an ascii string from byte array at a specified position.
        /// </summary>
        /// <param name="buffer">The byte array that contains the required name.</param>
        /// <param name="current">The position that user gets the data from.</param>
        /// <returns>The expected string.</returns>
        public static string GetAsciiName(byte[] buffer, ref int current)
        {
            int index = current;
            while (buffer[current] != 0)
            {
                current++;
            }
            string result = Encoding.Unicode.GetString(ArrayUtility.SubArray(buffer, index, current - index));
            current++;
            return result;
        }


        /// <summary>
        /// Get a Unicode string from byte array at a specified position.
        /// </summary>
        /// <param name="buffer">The byte array that contains the required name.</param>
        /// <param name="current">The position that user gets the data from.</param>
        /// <returns>The expected string.</returns>
        public static string GetUnicodeName(byte[] buffer, ref int current)
        {
            int index = current;
            while (buffer[current] != 0 || buffer[current + 1] != 0)
            {
                current += 2;
            }
            string result = Encoding.Unicode.GetString(ArrayUtility.SubArray(buffer, index, current - index));
            current += 2;
            return result;
        }


        /// <summary>
        /// Convert a compressed data to original string.
        /// </summary>
        /// <param name="buffer">Byte array of the structure.</param>
        /// <param name="current">Index to the buffer that contains first byte of current compressed data.</param>
        /// <returns>The decompressed string.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when fail to decompress the data.</exception>
        public static string DecompressName(byte[] buffer, ref int current)
        {
            int localCurrent = 0;
            bool firstLabel = true;
            StringBuilder sb = new StringBuilder();
            while (current < buffer.Length)
            {
                int labelSize = buffer[current];
                // Use \0 to mark the end of current compressed data.
                if (labelSize == '\0')
                {
                    if (localCurrent != 0)
                    {
                        current = localCurrent;
                    }
                    else
                    {
                        current++;
                    }
                    return sb.ToString();
                }
                else if ((labelSize & 0xC0) != 0)
                {
                    current++;
                    // Check the value of localCurrent to avoid be overriden in the following 0xC0
                    // i.e. contoso.com\0child@Sut01@
                    if (localCurrent == 0)
                    {
                        localCurrent = current + 1;
                    }
                    labelSize = buffer[current];
                    if (labelSize > buffer.Length)
                    {
                        throw new InvalidOperationException("Label size is not correct.");
                    }
                    current = labelSize;
                }
                else
                {
                    if ((current + labelSize) >= buffer.Length)
                    {
                        throw new InvalidOperationException("Label size is not correct.");
                    }
                    if (firstLabel)
                    {
                        firstLabel = false;
                    }
                    else
                    {
                        sb.Append('.');
                    }
                    current++;
                    sb.Append(Encoding.ASCII.GetString(ArrayUtility.SubArray(buffer, current, labelSize)));
                    current += labelSize;
                }
            }
            throw new InvalidOperationException("Fail to decompress the data.");
        }


        /// <summary>
        /// Parse an ldap object from byte array to string
        /// </summary>
        /// <param name="ldapSyntax">The ldap syntax.</param>
        /// <param name="buffer">The byte array object.</param>
        /// <returns>Result in string format.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the ldapSyntax is not supported.</exception>
        public static string Parse(AdtsLdapSyntax ldapSyntax, byte[] buffer)
        {
            string result;

            switch (ldapSyntax)
            {
                case AdtsLdapSyntax.Boolean:
                case AdtsLdapSyntax.Enumeration:
                case AdtsLdapSyntax.Integer:
                case AdtsLdapSyntax.LargeInteger:
                case AdtsLdapSyntax.ObjectForAccessPoint:
                case AdtsLdapSyntax.ObjectForDNString:
                case AdtsLdapSyntax.ObjectForORName:
                case AdtsLdapSyntax.ObjectForDNBinary:
                case AdtsLdapSyntax.ObjectForDSDN:
                case AdtsLdapSyntax.ObjectForPresentationAddress:
                case AdtsLdapSyntax.StringForCase:
                case AdtsLdapSyntax.StringForIA5:
                case AdtsLdapSyntax.StringForObjectIdentifier:
                case AdtsLdapSyntax.StringForPrintable:
                case AdtsLdapSyntax.ObjectForReplicaLink:
                case AdtsLdapSyntax.StringForOctet:
                case AdtsLdapSyntax.StringForNumeric:
                case AdtsLdapSyntax.StringForTeletex:
                case AdtsLdapSyntax.StringForUnicode:
                case AdtsLdapSyntax.StringForUTCTime:
                case AdtsLdapSyntax.StringForGeneralizedTime:
                    result = Encoding.UTF8.GetString(buffer);
                    break;

                case AdtsLdapSyntax.StringForNTSecDesc:
                    RawSecurityDescriptor ntSecurityDescriptor = new RawSecurityDescriptor(buffer, 0);
                    result = ntSecurityDescriptor.GetSddlForm(AccessControlSections.All);
                    break;

                case AdtsLdapSyntax.StringForSid:
                    SecurityIdentifier sid = new SecurityIdentifier(buffer, 0);
                    result = sid.Value;
                    break;

                default:
                    throw new NotSupportedException("The specified syntax is not supported.");
            }

            return result;
        }


        /// <summary>
        /// parse well known objects
        /// </summary>
        /// <param name="description">a wellknown object's description</param>
        /// <param name="dn">distinguish name of the wellknown object</param>
        /// <param name="guid">guid of the wellknown object</param>
        public static void ParseWellKnownObject(string description, out string dn, out Guid guid)
        {
            Regex regex = new Regex(
                @"^ B : (?<char_count> \d+ ) : (?<binary_value> [0-9a-fA-F]* ) : (?<object_DN> .* ) $",
                RegexOptions.IgnorePatternWhitespace);
            Match match = regex.Match(description);

            if (!match.Success)
            {
                throw new InvalidOperationException();
            }
            int charCount = Convert.ToInt32(match.Groups["char_count"].Value, CultureInfo.InvariantCulture);
            string binaryValueStr = match.Groups["binary_value"].Value;
            if (binaryValueStr.Length != charCount)
            {
                throw new InvalidOperationException();
            }
            byte[] binary = new byte[charCount / 2];
            for (int i = 0, j = 0; i < charCount; i += 2, j++)
            {
                binary[j] = Convert.ToByte(binaryValueStr.Substring(i, 2), 16);
            }
            dn = match.Groups["object_DN"].Value;
            guid = new Guid(binary);
        }
    }
}