// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    /// <summary>
    /// Represents list of keytab items.
    /// Provides methods to load keytab file into keytab items and query from keytab items.
    /// </summary>
    public class KeyManager
    {
        private List<KeytabItem> keytabItems;

        /// <summary>
        /// Initializes Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.KeyManager class.
        /// </summary>
        public KeyManager()
        {
            keytabItems = new List<KeytabItem>();
        }

        /// <summary>
        /// Query key
        /// </summary>
        /// <param name="principal">Principal name</param>
        /// <param name="realm">Realm</param>
        /// <param name="type">Key type</param>
        /// <param name="kvno">Key version number</param>
        /// <returns>Return the EncryptionKey if the key exists. Return null if not found.</returns>
        public EncryptionKey QueryKey(string principal, string realm, EncryptionType type, uint kvno = 0)
        {
            foreach (var item in keytabItems)
            {
                // In File Sharing test suite, realm name is treated as case insensitive.
                // Ignore kvno if it is 0
                if (item.Principal == principal
                    && string.Equals(item.Realm, realm, StringComparison.OrdinalIgnoreCase) 
                    && item.KeyType == type && (kvno == 0 || item.Kvno == kvno))
                    return item.Key;
                
            }
            return null;
        }

        /// <summary>
        /// Check whether the given principal exists.
        /// </summary>
        /// <param name="principal">The principal to check</param>
        /// <param name="realm">The realm of the given principal</param>
        /// <returns>true if the given principal exists; otherwise, false</returns>
        public bool CheckPrincipalExistence(string principal, string realm)
        {
            return keytabItems.Any(item => item.Principal == principal && item.Realm == realm);
        }

        /// <summary>
        /// Load keys from keytab file.
        /// </summary>
        /// <param name="filename">Filename</param>
        public void LoadKeytab(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[fs.Length];
            fs.Read(buf, 0, (int)fs.Length);
            Keytab kt = Keytab.Decode(buf);
            foreach (var entry in kt.entries)
            {
                string principal = entry.Components[0].ToString() +
                    (entry.NumComponents > 1 ? ("/" + entry.Components[1].ToString()) : "");
                KeytabItem item = new KeytabItem()
                {
                    Principal = principal,
                    Realm = entry.Realm.ToString(),
                    Kvno = entry.Kvno,
                    KeyType = (EncryptionType)entry.Key.type,
                    Key = new EncryptionKey(new KerbInt32((long)entry.Key.type), new Asn1OctetString(entry.Key.Data.Data))
                };
                keytabItems.Add(item);

                // For Windows keytab compatibility
                // Computer name is used instead of service principal name in Windows AD database.
                // Add cifs SPN manually when encountering computer name.
                if (principal.EndsWith("$"))
                {
                    string cifsPrincipal = string.Format("cifs/{0}.{1}", principal.Remove(principal.Length - 1).ToLower(), entry.Realm.ToString().ToLower());
                    item = new KeytabItem()
                    {
                        Principal = cifsPrincipal,
                        Realm = entry.Realm.ToString(),
                        Kvno = entry.Kvno,
                        KeyType = (EncryptionType)entry.Key.type,
                        Key = new EncryptionKey(new KerbInt32((long)entry.Key.type), new Asn1OctetString(entry.Key.Data.Data))
                    };
                    keytabItems.Add(item);
                }
            }
            fs.Close();
        }

        /// <summary>
        /// Make keys for given encryption types.
        /// </summary>
        /// <param name="principal">Principal name</param>
        /// <param name="realm">Realm</param>
        /// <param name="password">Password</param>
        /// <param name="salt">Salt</param>
        /// <param name="type">Encryption type</param>
        public void MakeKey(string principal, string realm, string password, string salt, EncryptionType type)
        {
            EncryptionKey key = KerberosUtility.MakeKey(type, password, salt);
            var existingKey = QueryKey(principal, realm, type);
            if (existingKey != null) throw new Exception("Key already exist.");
            KeytabItem item = new KeytabItem()
            {
                Principal = principal,
                Realm = realm,
                Kvno = 0, // Set to 0 for self generated keys.
                KeyType = type,
                Key = key
            };
            keytabItems.Add(item);
        }
        /// <summary>
        /// Make keys for all supported encryption types.
        /// </summary>
        /// <param name="principal">Principal name</param>
        /// <param name="realm">Realm</param>
        /// <param name="password">Password</param>
        /// <param name="salt">Salt</param>
        public void MakeKey(string principal, string realm, string password, string salt)
        {
            MakeKey(principal, realm, password, salt, EncryptionType.AES128_CTS_HMAC_SHA1_96);
            MakeKey(principal, realm, password, salt, EncryptionType.AES256_CTS_HMAC_SHA1_96);
            MakeKey(principal, realm, password, salt, EncryptionType.DES_CBC_CRC);
            MakeKey(principal, realm, password, salt, EncryptionType.DES_CBC_MD5);
            MakeKey(principal, realm, password, salt, EncryptionType.RC4_HMAC);
            MakeKey(principal, realm, password, salt, EncryptionType.RC4_HMAC_EXP);
        }
    }

    /// <summary>
    /// Represents a keytab item
    /// </summary>
    public class KeytabItem
    {
        public string Principal { get; set; }
        public string Realm { get; set; }
        public uint Kvno { get; set; }
        public EncryptionType KeyType { get; set; }
        public EncryptionKey Key { get; set; }
        public override string ToString()
        {
            return Principal + "@" + Realm + "," + KeyType.ToString();
        }
    }

    /// <summary>
    /// Represents the content of a keytab file, which consists of a 16 bit file_format_version
    /// and list of KeytabEntry.
    /// </summary>
    public class Keytab
    {
        public UInt16 FormatVersion;
        public KeytabEntry[] entries;
        public static Keytab Decode(byte[] buffer)
        {
            List<KeytabEntry> entryList = new List<KeytabEntry>();
            KeytabDecodeBuffer decodeBuffer = new KeytabDecodeBuffer(buffer);
            Keytab keytab = new Keytab 
            {
                FormatVersion = decodeBuffer.DecodeUInt16(), 
                entries = new KeytabEntry[1]
            };
            while (!decodeBuffer.EOS)
            {
                entryList.Add(KeytabEntry.Decode(decodeBuffer, keytab.FormatVersion));
            }
            keytab.entries = new KeytabEntry[entryList.Count];
            for (int i = 0; i < entryList.Count; i++)
            {
                keytab.entries[i] = entryList[i];
            }
            return keytab;
        }
    }

    /// <summary>
    /// Represents the KeytabEntry structure.
    /// </summary>
    public class KeytabEntry
    {
        public System.Int32 Size;
        public System.UInt16 NumComponents;
        public CountedOctetString Realm;
        public CountedOctetString[] Components;
        public System.UInt32 NameType;
        public System.UInt32 TimeStamp;
        public KeyBlock Key;

        byte Vno8;
        uint? Vno;

        public uint Kvno
        {
            get
            {
                if (Vno.HasValue)
                {
                    return Vno.Value;
                }
                else
                {
                    return (uint)Vno8;
                }
            }
        }

        /// <summary>
        /// Decode the KeytabDecodeBuffer into KeytabEntry.
        /// Reference: http://www.gnu.org/software/shishi/manual/html_node/The-Keytab-Binary-File-Format.html
        /// </summary>
        public static KeytabEntry Decode(KeytabDecodeBuffer buffer, ushort formatVersion)
        {
            KeytabEntry entry = new KeytabEntry();
            entry.Size = buffer.DecodeInt32();
            buffer.EntrySize = entry.Size;

            //* sub 1 if version 0x501 *
            entry.NumComponents = buffer.DecodeUInt16();
            if (formatVersion == 0x501)
            {
                entry.NumComponents--;
            }
            entry.Realm = CountedOctetString.Decode(buffer);
            entry.Components = new CountedOctetString[entry.NumComponents];
            for (int i = 0; i < entry.NumComponents; i++)
            {
                entry.Components[i] = CountedOctetString.Decode(buffer);
            }

            // not present if version 0x501
            if (formatVersion != 0x501)
            {
                entry.NameType = buffer.DecodeUInt32();
            }
            entry.TimeStamp = buffer.DecodeUInt32();
            entry.Vno8 = buffer.DecodeByte();
            entry.Key = KeyBlock.Decode(buffer);
            if (buffer.EntrySize >= 4)
            {
                entry.Vno = buffer.DecodeUInt32();
            }
            if (buffer.EntrySize > 0)
            {
                buffer.MoveIndexToNext();
            }
            return entry;
        }
    }

    /// <summary>
    /// Represents the binary content of a keytab entry.
    /// Provides methods to decode it into certain types.
    /// </summary>
    public class KeytabDecodeBuffer
    {
        byte[] buffer;
        int index;
        public int EntrySize;
        public bool EOS
        {
            get
            {
                if (buffer == null || buffer.Length == 0) return true;
                if (index >= buffer.Length) return true;
                return false;
            }
        }

        public void MoveIndexToNext()
        {
            index += EntrySize;
            EntrySize = 0;
        }

        public KeytabDecodeBuffer(byte[] buffer)
        {
            this.buffer = buffer;
            index = 0;
        }
        internal ushort DecodeUInt16()
        {
            if (EntrySize > 0) EntrySize -= 2;
            byte[] buf = new byte[2];
            buf[1] = buffer[index++];
            buf[0] = buffer[index++];
            return BitConverter.ToUInt16(buf, 0);
        }

        internal byte[] DecodeByteArray(ushort length)
        {
            if (EntrySize > 0) EntrySize -= length;
            byte[] buf = new byte[length];
            Array.Copy(buffer, index, buf, 0, length);
            index += length;
            return buf;
        }

        internal int DecodeInt32()
        {
            if (EntrySize > 0) EntrySize -= 4;
            byte[] buf = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                buf[3 - i] = buffer[index++];
            }
            return BitConverter.ToInt32(buf, 0);
        }

        internal uint DecodeUInt32()
        {
            if (EntrySize > 0) EntrySize -= 4;
            byte[] buf = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                buf[3 - i] = buffer[index++];
            }
            return BitConverter.ToUInt32(buf, 0);
        }

        internal byte DecodeByte()
        {
            if (EntrySize > 0) EntrySize -= 1;
            return buffer[index++];
        }
    }

    /// <summary>
    /// The keyblock structure consists of a 16 bit value indicating the keytype 
    /// and is followed by a counted_octet_string containing the key.
    /// </summary>
    public class KeyBlock
    {
        public UInt16 type;
        public CountedOctetString Data;
        public static KeyBlock Decode(KeytabDecodeBuffer buffer)
        {
            KeyBlock keyBlock = new KeyBlock();
            keyBlock.type = buffer.DecodeUInt16();
            keyBlock.Data = CountedOctetString.Decode(buffer);
            return keyBlock;
        }
    }

    /// <summary>
    /// Represents a CountedOctetString class, which is simply an array of bytes prefixed with a 16 bit length.
    /// </summary>
    public class CountedOctetString
    {
        public System.UInt16 Length;
        public byte[] Data;

        internal static CountedOctetString Decode(KeytabDecodeBuffer buffer)
        {
            CountedOctetString octStr = new CountedOctetString();
            octStr.Length = buffer.DecodeUInt16();
            octStr.Data = buffer.DecodeByteArray(octStr.Length);
            return octStr;
        }

        public override string ToString()
        {
            string str = Encoding.ASCII.GetString(Data);
            return str;
        }

        public string ByteString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Data.Length; i++)
                {
                    sb.AppendFormat("{0} ", Data[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
