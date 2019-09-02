// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Principal;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public class OIDUtility
    {
        static byte[] FromBinaryString(string binStr)
        {
            // 2 bytes at a time
            byte[] re = new byte[binStr.Length / 2];
            for (int i = 0; i < binStr.Length; i += 2)
            {
                string s = binStr.Substring(i, 2);
                re[i / 2] = (byte)(Convert.ToByte(s, 16));
            }
            return re;
        }
        public static byte[] ToBinary(string st)
        {
            List<byte> bl = new List<byte>();
            for (int i = 0; i < st.Length; ++i)
            {
                bl.Add((byte)st[i]);
            }
            return bl.ToArray();
        }

        static byte[] CatBinary(byte[] o, byte b)
        {
            List<byte> bl = new List<byte>();
            foreach (byte i in o)
            {
                bl.Add(i);
            }
            bl.Add(b);
            return bl.ToArray();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
        static string ToStringOID(byte[] o)
        {
            string re = "";
            int o1 = o[0] / 40;
            re += o1.ToString() + ".";
            int o2 = o[0] % 40;
            re += o2.ToString() + ".";
            int p = 0;
            for (int pos = 1; pos < o.Length; ++pos)
            {
                int v = o[pos] & 0x80;
                p += (o[pos] & ~0x80);
                if (v > 0)
                {
                    p <<= 7;
                }
                else
                {
                    re += p.ToString() + ".";
                    p = 0;
                }
            }

            // remove the trailing "."
            if (re.Length > 0)
            {
                re = re.Substring(0, re.Length - 1);
            }
            return re;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt32(System.String)")]
        static byte[] ToBinaryOID(string s)
        {
            List<byte> bl = new List<byte>();
            string[] subs = s.Split('.');

            int i1 = Convert.ToInt32(subs[0]);
            int i2 = Convert.ToInt32(subs[1]);
            byte b1 = (byte)(i1 * 40 + i2);

            bl.Add(b1);
            int j = 0;
            for (int i = 2; i < subs.Length; ++i)
            {
                j = bl.Count;
                if (subs[i].Length == 0)
                    break;
                int v = Convert.ToInt32(subs[i]);
                if (v < 0x80)
                {
                    bl.Add((byte)v);
                    continue;
                }

                // v is greater than 0x80
                // last octet doesn't have 8th bit set
                bool isLast = true;
                while (v >= 0x80)
                {
                    int p = (v & 0x7f) + 0x80;
                    v >>= 7;
                    if (isLast)
                    {
                        isLast = false;
                        p = p - 0x80;
                    }
                    bl.Insert(j, (byte)p);
                }
                bl.Insert(j, (byte)(v + 0x80));
            }

            return bl.ToArray();
        }

        static byte[] SubBinary(byte[] data, int start, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(data, start, result, 0, length);
            return result;
        }

        public static string OidFromAttrid(SCHEMA_PREFIX_TABLE t, uint attr)
        {
            uint upperWord, lowerWord;
            byte[] binaryOID = null;

            upperWord = attr / 65536;
            lowerWord = attr % 65536;

            for (int i = 0; i < t.PrefixCount; ++i)
            {
                if (t.pPrefixEntry[i].ndx == upperWord)
                {
                    if (lowerWord < 128)
                        binaryOID = CatBinary(t.pPrefixEntry[i].prefix.elements, (byte)lowerWord);
                    else
                    {
                        if (lowerWord >= 32768)
                            lowerWord -= 32768;
                        binaryOID = CatBinary(t.pPrefixEntry[i].prefix.elements, (byte)(((lowerWord / 128) % 128) + 128));
                        binaryOID = CatBinary(binaryOID, (byte)(lowerWord % 128));
                    }
                }
            }

            if (binaryOID == null)
                return null;

            return ToStringOID(binaryOID);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToUInt32(System.String)")]
        public static uint MakeAttid(SCHEMA_PREFIX_TABLE t, string o)
        {
            string lastValueString;
            uint lastValue, lowerWord;
            byte[] binaryOID, oidPrefix;
            uint attr;
            uint pos = 0;

            string[] ss = o.Split('.');
            lastValueString = ss[ss.Length - 1];
            lastValue = Convert.ToUInt32(lastValueString);

            binaryOID = ToBinaryOID(o);

            if (lastValue < 128)
                oidPrefix = SubBinary(binaryOID, 0, binaryOID.Length - 1);
            else
                oidPrefix = SubBinary(binaryOID, 0, binaryOID.Length - 2);

            bool fToAdd = true;
            for (uint i = 0; i < t.PrefixCount; ++i)
            {
                if (DrsrHelper.IsByteArrayEqual(t.pPrefixEntry[i].prefix.elements, oidPrefix))
                {
                    fToAdd = false;
                    pos = i;
                    break;
                }
            }

            if (fToAdd)
            {
                pos = (uint)t.PrefixCount;
                AddPrefixTableEntry(ref t, oidPrefix);
            }

            lowerWord = lastValue % 16384;
            if (lastValue >= 16384)
                lowerWord += 32768;
            uint upperWord = t.pPrefixEntry[pos].ndx;
            attr = upperWord * 65536 + lowerWord;

            return attr;
        }

        static void AddPrefixTableEntry(ref SCHEMA_PREFIX_TABLE t, byte[] s)
        {
            PrefixTableEntry[] nt = new PrefixTableEntry[t.PrefixCount + 1];
            Array.Copy(t.pPrefixEntry, nt, t.PrefixCount);
            // Find a prefix index that's not in use.
            Random rnd = new Random();
            uint idx = 0;
            bool found = false;
            while (!found)
            {
                found = true;
                idx = (uint)rnd.Next(0, 65535);
                for (int i = 0; i < t.PrefixCount; ++i)
                {
                    if (idx == t.pPrefixEntry[i].ndx)
                    {
                        found = false;
                        break;
                    }
                }
            }

            nt[nt.Length + 1] = new PrefixTableEntry();
            nt[nt.Length + 1].ndx = idx;
            nt[nt.Length + 1].prefix.length = (uint)s.Length;
            nt[nt.Length + 1].prefix.elements = s;
            t.PrefixCount++;
            t.pPrefixEntry = nt;
        }

        static PrefixTableEntry MakePrefixTableEntry(string oid, uint index)
        {
            PrefixTableEntry entry = new PrefixTableEntry();
            entry.ndx = index;
            byte[] bo = ToBinary(oid);
            entry.prefix.length = (uint)bo.Length;
            entry.prefix.elements = bo;
            return entry;
        }

        public static SCHEMA_PREFIX_TABLE CreatePrefixTable()
        {
            List<PrefixTableEntry> entries = new List<PrefixTableEntry>();
            entries.Add(MakePrefixTableEntry("\x55\x4", 0));
            entries.Add(MakePrefixTableEntry("\x55\x6", 1));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x02", 2));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x03", 3));
            entries.Add(MakePrefixTableEntry("\x60\x86\x48\x01\x65\x02\x02\x01", 4));

            entries.Add(MakePrefixTableEntry("\x60\x86\x48\x01\x65\x02\x02\x03", 5));
            entries.Add(MakePrefixTableEntry("\x60\x86\x48\x01\x65\x02\x01\x05", 6));
            entries.Add(MakePrefixTableEntry("\x60\x86\x48\x01\x65\x02\x01\x04", 7));
            entries.Add(MakePrefixTableEntry("\x55\x5", 8));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x04", 9));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x05", 10));
            entries.Add(MakePrefixTableEntry("\x09\x92\x26\x89\x93\xF2\x2C\x64", 19));
            entries.Add(MakePrefixTableEntry("\x60\x86\x48\x01\x86\xF8\x42\x03", 20));
            entries.Add(MakePrefixTableEntry("\x09\x92\x26\x89\x93\xF2\x2C\x64\x01", 21));
            entries.Add(MakePrefixTableEntry("\x60\x86\x48\x01\x86\xF8\x42\x03\x01", 22));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x05\xB6\x58", 23));
            entries.Add(MakePrefixTableEntry("\x55\x15", 24));
            entries.Add(MakePrefixTableEntry("\x55\x12", 25));
            entries.Add(MakePrefixTableEntry("\x55\x14", 26));
            entries.Add(MakePrefixTableEntry("\x2B\x06\x01\x04\x01\x8B\x3A\x65\x77", 27));
            entries.Add(MakePrefixTableEntry("\x60\x86\x48\x01\x86\xF8\x42\x03\x02", 28));
            entries.Add(MakePrefixTableEntry("\x2B\x06\x01\x04\x01\x81\x7A\x01", 29));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x0D\x01\x09", 30));
            entries.Add(MakePrefixTableEntry("\x09\x92\x26\x89\x93\xF2\x2C\x64\x04", 31));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x06\x17", 32));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x06\x12\x01", 33));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x06\x12\x02", 34));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x06\x0D\x03", 35));
            entries.Add(MakePrefixTableEntry("\x2A\x86\x48\x86\xF7\x14\x01\x06\x0D\x04", 36));
            entries.Add(MakePrefixTableEntry("\x2B\x06\x01\x01\x01\x01", 37));
            entries.Add(MakePrefixTableEntry("\x2B\x06\x01\x01\x01\x02", 38));


            SCHEMA_PREFIX_TABLE table = new SCHEMA_PREFIX_TABLE();
            table.PrefixCount = (uint)entries.Count;
            table.pPrefixEntry = entries.ToArray();
            return table;
        }

        static DSNAME ParseObjectDsDn(string object_dn)
        {
            string pattern = @"<GUID=([a-fA-F0-9\-]+)>;(<SID=([a-fA-F0-9]+)>;)?(.*)";
            Regex r = new Regex(pattern);
            Match m = r.Match(object_dn);

            Guid guid = Guid.Empty;
            string guidStr = m.Groups[1].Value;
            if (guidStr.Contains("-"))
            {
                guid = new Guid(guidStr);
            }
            else
            {
                byte[] guidBinary = FromBinaryString(m.Groups[1].Value);
                guid = new Guid(guidBinary);
            }

            string dn = m.Groups[4].Value;
            DSNAME dsName = DrsuapiClient.CreateDsName(dn, guid, null);

            string sidStr = m.Groups[3].Value;
            if (sidStr.Length > 0)
            {
                byte[] sidBinary = FromBinaryString(sidStr);
                SecurityIdentifier secId = new SecurityIdentifier(sidBinary, 0);
                dsName.SidLen = (uint)secId.BinaryLength;
                // Sid.Data is always 28 bytes long
                Array.Copy(sidBinary, dsName.Sid.Data, sidBinary.Length);
            }
            return dsName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt64(System.String)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt32(System.String)")]
        public static ATTRVAL ATTRVALFromValue(DsServer dc, string v, string attrSyntax, SCHEMA_PREFIX_TABLE prefixTable)
        {
            ATTRVAL attrVal = new ATTRVAL();
            switch (attrSyntax)
            {
                case "2.2.5.8": // Boolean
                    {
                        attrVal.valLen = 4;
                        attrVal.pVal = new byte[4];
                        if (v == "TRUE")
                            attrVal.pVal[0] = 1;
                        else
                            attrVal.pVal[0] = 0;
                        break;
                    }
                case "2.5.5.9": // Enumeration, Integer
                    {
                        attrVal.valLen = 4;
                        int intValue = Convert.ToInt32(v);
                        attrVal.pVal = BitConverter.GetBytes(intValue);
                        break;
                    }
                case "2.5.5.16":    // LargeInteger
                    {
                        attrVal.valLen = 8;
                        long intValue = Convert.ToInt64(v);
                        attrVal.pVal = BitConverter.GetBytes(intValue);
                        break;
                    }
                case "2.5.5.13":    // Object (Presentation-Address)
                    {
                        System.Text.UnicodeEncoding utf16 = new System.Text.UnicodeEncoding();
                        byte[] data = utf16.GetBytes(v);

                        attrVal.valLen = (uint)data.Length + 4;
                        attrVal.pVal = new byte[attrVal.valLen];

                        byte[] intBytes = BitConverter.GetBytes(attrVal.valLen);

                        Array.Copy(intBytes, 0, attrVal.pVal, 0, intBytes.Length);
                        Array.Copy(data, 0, attrVal.pVal, intBytes.Length, data.Length);

                        break;
                    }
                case "2.5.5.4":     // String (Teletex)
                case "2.5.5.5":     // String (IA5), String (Printable)
                case "2.5.5.6":    // String (Numeric)
                case "2.5.5.10":    // Object (Replica-Link), string (Octet)
                    {
                        attrVal.pVal = ToBinary(v);
                        attrVal.valLen = (uint)attrVal.pVal.Length;
                        break;
                    }
                case "2.5.5.2":     // String (Object-Identifier)
                    {
                        if (v is string && v.Contains(".") == false)
                        {
                            // Look in the Schema NC for the object class and it's governsID.
                            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
                            string schemaNc = rootDse.schemaNamingContext;

                            v = LdapUtility.GetAttributeValueInString(
                                dc,
                                schemaNc,
                                "governsId",
                                "(&(objectClass=classSchema)(lDAPDisplayName=" + v + "))",
                                System.DirectoryServices.Protocols.SearchScope.Subtree);
                        }

                        attrVal.valLen = 4;
                        uint attid = MakeAttid(prefixTable, v);
                        attrVal.pVal = BitConverter.GetBytes(attid);
                        break;
                    }
                case "2.5.5.12":    // String (Unicode)
                    {
                        System.Text.UnicodeEncoding utf16 = new System.Text.UnicodeEncoding();
                        attrVal.pVal = utf16.GetBytes(v);
                        attrVal.valLen = (uint)attrVal.pVal.Length;
                        break;
                    }
                case "2.5.5.11":    // String (UTC-Time), String (Generalized-Time)
                    {
                        DateTime t;
                        string timePattern = "yyyyMMddHHmmss.'0Z'";
                        DateTime.TryParseExact(
                            v,
                            timePattern,
                            null,
                            System.Globalization.DateTimeStyles.None,
                            out t);

                        DateTime dsTime = new DateTime(1601, 1, 1, 0, 0, 0);
                        TimeSpan diff = t - dsTime;
                        attrVal.valLen = 8;
                        attrVal.pVal = BitConverter.GetBytes((ulong)diff.TotalSeconds);

                        break;
                    }
                case "2.5.5.1":     // Object (DS-DN)
                    {
                        DSNAME dsName = ParseObjectDsDn(v);

                        attrVal.pVal = TypeMarshal.ToBytes<DSNAME>(dsName);
                        attrVal.valLen = (uint)attrVal.pVal.Length;
                    }
                    break;
                case "2.5.5.14":    // Object (DN-String), Object (Access-Point)
                    {
                        string pattern = @"S:([0-9a-fA-F]+):(.*):(.*)";
                        Regex r = new Regex(pattern);
                        Match m = r.Match(v);

                        uint char_count = (uint)Convert.ToInt32(m.Groups[1].Value, 16);

                        SYNTAX_ADDRESS sa = new SYNTAX_ADDRESS();
                        sa.dataLen = (2 * char_count) + 4;

                        System.Text.UnicodeEncoding utf16 = new System.Text.UnicodeEncoding();

                        sa.byteVal = utf16.GetBytes(m.Groups[2].Value);
                        byte[] saBin = TypeMarshal.ToBytes<SYNTAX_ADDRESS>(sa);

                        DSNAME dsdn = ParseObjectDsDn(m.Groups[3].Value);
                        byte[] dsdnBin = TypeMarshal.ToBytes<DSNAME>(dsdn);

                        // Add padding
                        uint p = 4 - dsdn.structLen % 4;
                        if (p == 4)
                            p = 0;
                        byte[] padding = new byte[p];

                        attrVal.pVal = new byte[dsdnBin.Length + p + saBin.Length];
                        Array.Copy(dsdnBin, attrVal.pVal, dsdnBin.Length);
                        Array.Copy(saBin, 0, attrVal.pVal, dsdnBin.Length + p, saBin.Length);

                        attrVal.valLen = (uint)attrVal.pVal.Length;
                    }
                    break;
                case "2.5.5.7":     // Object (DN-Binary), Object (OR-Name)
                    {
                        string pattern = @"B:([0-9a-fA-F]+):([0-9a-fA-F]+):(.*)";
                        Regex r = new Regex(pattern);
                        Match m = r.Match(v);

                        uint char_count = (uint)Convert.ToInt32(m.Groups[1].Value, 16);

                        SYNTAX_ADDRESS sa = new SYNTAX_ADDRESS();
                        sa.dataLen = char_count;
                        sa.byteVal = FromBinaryString(m.Groups[2].Value);
                        byte[] saBin = TypeMarshal.ToBytes<SYNTAX_ADDRESS>(sa);

                        DSNAME dsdn = ParseObjectDsDn(m.Groups[3].Value);
                        byte[] dsdnBin = TypeMarshal.ToBytes<DSNAME>(dsdn);

                        // Add padding
                        uint p = 4 - dsdn.structLen % 4;
                        if (p == 4)
                            p = 0;
                        byte[] padding = new byte[p];

                        attrVal.pVal = new byte[dsdnBin.Length + p + saBin.Length];
                        Array.Copy(dsdnBin, attrVal.pVal, dsdnBin.Length);
                        Array.Copy(saBin, 0, attrVal.pVal, dsdnBin.Length + p, saBin.Length);

                        attrVal.valLen = (uint)attrVal.pVal.Length;
                    }
                    break;
                case "2.5.5.15":    // String (NT-Sec-Desc)
                case "2.5.5.17":    // String (SID)
                    {
                        attrVal.pVal = FromBinaryString(v);
                        attrVal.valLen = (uint)attrVal.pVal.Length;
                    }
                    break;
                
                default:
                    break;
            }
            return attrVal;
        }
    }
}
