// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public class DrsrHelper
    {
        public int NetBIOS_MaxLen = 15;
        public const string MaxGuid = "ffffffff-ffff-ffff-ffff-ffffffffffff";

        /// <summary>
        /// Get the NetBIOS name from a FQDN name (max length is 15 bytes).
        /// </summary>
        /// <param name="fqdnName">The FQDN name</param>
        /// <returns>The NebBIOS name of the given FQDN name</returns>
        public static string GetNetbiosNameFromDNSName(string fqdnName)
        {
            string[] nameArr = fqdnName.Split('.');
            return (nameArr[0].Length > 15) ? nameArr[0].Substring(0, 15) : nameArr[0];
        }

        /// <summary>
        /// Get the DSName of FSMO Role object
        /// </summary>
        /// <param name="domain">Root Domain</param>
        /// <param name="role">The FSMO role</param>
        /// <returns>DSName of the FSMO role object</returns>
        public static DSNAME GetFsmoRoleObject(DsDomain domain, FSMORoles role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compute the digest of the input Guid sequence
        /// </summary>
        /// <param name="guidSeq">The Guid sequence.</param>
        /// <returns>The digest of Guid sequence.</returns>
        public static byte[] ComputeMD5Digest(Guid[] guidSeq)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();

            List<byte> guidByteSeq = new List<byte>();
            foreach (Guid g in guidSeq)
            {
                guidByteSeq.AddRange(g.ToByteArray());
            }

            byte[] hash = md5.ComputeHash(guidByteSeq.ToArray());

            return hash;
        }

        /// <summary>
        /// Convert byte array to DRS_EXTENSIONS_INT
        /// </summary>
        /// <param name="data">data source</param>
        /// <returns>DRS_EXTENSIONS_INT</returns>
        public static DRS_EXTENSIONS_INT? ConvertByteToDrext(byte[] data)
        {
            DRS_EXTENSIONS_INT ret = new DRS_EXTENSIONS_INT();
            bool passed = false;
            do
            {
                ret.cb = (uint)data.Length;
                ret.dwFlags = BitConverter.ToUInt32(data, 4);
                byte[] tmp = new byte[16];
                Array.Copy(data, 8, tmp, 0, 16);
                ret.SiteObjGuid = new Guid(tmp);
                ret.Pid = BitConverter.ToInt32(data, 24);
                ret.dwReplEpoch = BitConverter.ToUInt32(data, 28);
                ret.dwFlagsExt = BitConverter.ToUInt32(data, 32);
                tmp = new byte[16];
                Array.Copy(data, 36, tmp, 0, 16);
                ret.ConfigObjGUID = new Guid(tmp);
                passed = true;
            }
            while (false);

            if (passed)
                return ret;
            else
                return null;
        }

        /// <summary>
        /// convert DN into FQDN
        /// </summary>
        /// <param name="dn">DistinguishedName</param>
        /// <returns>FQDN</returns>
        public static string GetFQDNFromDN(string dn)
        {
            string[] strs = dn.Split(',');
            string output = "";
            for (int i = 0; i < strs.Length - 1; ++i)
            {
                output += (strs[i].Split('=')[1] + ".");
            }
            output += (strs[strs.Length - 1].Split('=')[1]);
            return output;
        }

        /// <summary>
        /// convert FQDN into DN
        /// </summary>
        /// <param name="fqdn">FQDN</param>
        /// <returns>DistinguishedName</returns>
        public static string GetDNFromFQDN(string fqdn)
        {
            if (!fqdn.Contains("."))
                return null;

            string[] strs = fqdn.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            string output = "";
            for (int i = 0; i < strs.Length; i++)
            {
                output += "DC=";
                output += strs[i];
                if (i != strs.Length - 1)
                    output += ",";
            }
            return output;
        }

        /// <summary>
        /// get common name from distinguished name
        /// </summary>
        /// <param name="dn">DN</param>
        /// <returns>Common name.</returns>
        public static string GetCNFromDN(string dn)
        {
            string[] strs = dn.Split(new string[] { "=", "," }, StringSplitOptions.RemoveEmptyEntries);
            return strs[1];
        }

        /// <summary>
        /// get the DN of the specified NC.
        /// </summary>
        /// <param name="domain">Domain that contains the NC.</param>
        /// <param name="ncType">The naming context type.</param>
        /// <returns>The DN of the specified NC. Null if the NC is not existed.</returns>
        public static string GetNamingContextDN(DsDomain domain, NamingContext ncType)
        {
            DSNAME ncDsName;
            if (ncType == NamingContext.ConfigNC)
            {
                ncDsName = domain.ConfigNC;
            }
            else if (ncType == NamingContext.SchemaNC)
            {
                ncDsName = domain.SchemaNC;
            } 
            else if (ncType == NamingContext.DomainNC)
            {
                if (domain is AddsDomain)
                {
                    ncDsName = ((AddsDomain)domain).DomainNC;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (domain is AdldsDomain)
                {
                    ncDsName = ((AdldsDomain)domain).AppNCs[0];
                }
                else
                {
                    return null;
                }
            }

            return LdapUtility.ConvertUshortArrayToString(ncDsName.StringName);
        }

        /// <summary>
        /// get the DSName of the specified NC.
        /// </summary>
        /// <param name="domain">Domain that contains the NC.</param>
        /// <param name="ncType">The naming context type.</param>
        /// <returns>The DSName of the specified NC. Null if the NC is not existed.</returns>
        public static DSNAME GetNamingContextDSName(DsDomain domain, NamingContext ncType)
        {
            DSNAME ncDsName = new DSNAME(); 
            if (ncType == NamingContext.ConfigNC)
            {
                ncDsName = domain.ConfigNC;
            }
            else if (ncType == NamingContext.SchemaNC)
            {
                ncDsName = domain.SchemaNC;
            }
            else if (ncType == NamingContext.DomainNC)
            {
                if (domain is AddsDomain)
                {
                    ncDsName = ((AddsDomain)domain).DomainNC;
                }
            }
            else
            {
                if (domain is AdldsDomain)
                {
                    ncDsName = ((AdldsDomain)domain).AppNCs[0];
                }
            }

            return ncDsName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        public static bool AreDNsSame(string first, string second)
        {
            string[] src = first.ToLower().Split(new string[] { "=", "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] comp = second.ToLower().Split(new string[] { "=", "," }, StringSplitOptions.RemoveEmptyEntries);

            if (src.Length != comp.Length)
                return false;

            for (int i = 0; i < src.Length; i++)
            {
                if (src[i].Trim() != comp[i].Trim())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// //Returns a SHA1 hash of the value of msDS-UpdateScript of Partitions container.
        /// </summary>
        public static byte[] PrepareScriptHashBody(string updateScript)
        {
            byte[] unicodeBytes = System.Text.Encoding.Unicode.GetBytes(updateScript);
            System.Security.Cryptography.SHA1 sha = System.Security.Cryptography.SHA1CryptoServiceProvider.Create();
            byte[] hash1 = sha.ComputeHash(unicodeBytes);
            return hash1;
        }

        /// <summary>
        /// Returns a SHA1 hash of the value formed by appending the GUID {0916C8E3-3431-4586-AF77-44BD3B16F961}
        /// to the value of msDS-UpdateScript of Partitions container.
        /// </summary>
        public static byte[] PrepareScriptHashSignature(string updateScript)
        {
            byte[] unicodeBytes = System.Text.Encoding.Unicode.GetBytes(updateScript);
            System.Security.Cryptography.SHA1 sha = System.Security.Cryptography.SHA1CryptoServiceProvider.Create();
            Guid g = new Guid("0916C8E3-3431-4586-AF77-44BD3B16F961");
            List<byte> rawByteList = new List<byte>();
            rawByteList.AddRange(unicodeBytes);
            rawByteList.AddRange(g.ToByteArray());

            byte[] hash1 = sha.ComputeHash(rawByteList.ToArray());
            return hash1;
        }

        /// <summary>
        /// Compares two string arrays. Arrays are considered equal
        /// if strings within the two array have 1-to-1 mapping.
        /// The comparison is case-sensitive.
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <returns></returns>
        public static bool IsStringArrayEqual(string[] arr1, string[] arr2)
        {
            // sort the arrays first
            Array.Sort(arr1);
            Array.Sort(arr2);
            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; ++i)
            {
                if (arr1[i].Trim() != arr2[i].Trim())
                    return false;
            }
            return true;
        }

        public static bool IsDSNameArrayEqual(DSNAME[] arr1, DSNAME[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            // for now we just take the GUID part.
            string[] guidArr1 = new string[arr1.Length];
            string[] guidArr2 = new string[arr2.Length];

            for (int i = 0; i < arr1.Length; ++i)
            {
                guidArr1[i] = arr1[i].Guid.ToString();
                guidArr2[i] = arr2[i].Guid.ToString();
            }

            return IsStringArrayEqual(guidArr1, guidArr2);
        }

        /// <summary>
        /// Compare two byte arrays. Return true if the arrays are the same. False otherwise.
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static bool IsByteArrayEqual(byte[] b1, byte[] b2)
        {
            if (b1.Length != b2.Length)
                return false;
            for (int i = 0; i < b1.Length; ++i)
            {
                if (b1[i] != b2[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Generate the parent DN from the child DN.
        /// </summary>
        /// <param name="childDN">The child DN.</param>
        /// <returns>The parent DN.</returns>
        public static string GetParentDNFromChildDN(string childDN)
        {
            if(childDN == null)return null;
            string parentDN;
            int idx = childDN.IndexOf(',');
            parentDN = childDN.Substring(idx + 1);
            return parentDN;
        }

        /// <summary>
        /// Get the the RDN from a DN.
        /// </summary>
        /// <param name="dn">The DN.</param>
        /// <returns>The RDN</returns>
        public static string GetRDNFromDN(string dn)
        {
            string rdn;
            int idx = dn.IndexOf(',');
            rdn = dn.Substring(0, idx);
            return rdn;
        }

        /// <summary>
        /// Convert the ushort array of name in DSNAME to string.
        /// </summary>
        /// <param name="data">The ushort array.</param>
        /// <returns>The string name.</returns>
        public static string ConvertUshortArrayToString(ushort[] data)
        {
            byte[] asBytes = new byte[(data.Length - 1) * sizeof(ushort)];
            Buffer.BlockCopy(data, 0, asBytes, 0, asBytes.Length - 1);
            return Encoding.Unicode.GetString(asBytes);
        }

        /// <summary>
        /// Get the IPv4 address of the specified host
        /// </summary>
        /// <param name="dnsName">DNS name of the host.</param>
        /// <returns>IPv4 address. Null if not found.</returns>
        public static string GetIPAddress(string dnsName)
        {
            System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(dnsName);
            System.Net.IPAddress[] addrs = hostEntry.AddressList;
            foreach (System.Net.IPAddress addr in addrs)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return addr.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Generate the Generic Security Service (GSS) Kerberos authentication token for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The GSS Kerberos authentication token in type of DRS_SecBuffer.</returns>
        public static DRS_SecBufferDesc GetAuthenticationToken(string account, string pwd, string domain, string spn)
        {
            ClientSecurityContextAttribute contextAttr =
                ClientSecurityContextAttribute.Connection |
                //ClientSecurityContextAttribute.Integrity |
                ClientSecurityContextAttribute.Confidentiality 
                //|
                //ClientSecurityContextAttribute.UseSessionKey
                //|
                //ClientSecurityContextAttribute.Delegate
                ;

            SspiClientSecurityContext securityContext = new SspiClientSecurityContext(
                SecurityPackageType.Kerberos,
                new AccountCredential(domain, account, pwd),
                spn,
                contextAttr,
                SecurityTargetDataRepresentation.SecurityNativeDrep);

            //securityContext.Initialize(null);

            string kdcIpAddr = GetIPAddress(domain);
            byte[] token = GenerateGssApToken(kdcIpAddr, account, pwd, domain, spn, KerberosAccountType.User);

            DRS_SecBufferDesc pClientCreds = new DRS_SecBufferDesc();
            pClientCreds.ulVersion = ulVersion_Values.V1;
            //pClientCreds.Buffers = null;
            pClientCreds.cBuffers = 1;
            pClientCreds.Buffers = new DRS_SecBuffer[1];
            DRS_SecBuffer secBuf = new DRS_SecBuffer();
            secBuf.BufferType = BufferType_Values.SECBUFFER_TOKEN;
            secBuf.pvBuffer = token;
            secBuf.cbBuffer = (uint)secBuf.pvBuffer.Length;
            pClientCreds.Buffers[0] = secBuf;

            return pClientCreds;
        }

        /// <summary>
        /// Generate a GCC AP token for the given account and SPN.
        /// </summary>
        /// <param name="kdcIpAddr">KDC IP address</param>
        /// <param name="account">Account Name.</param>
        /// <param name="pwd">Password of the account.</param>
        /// <param name="domain">Domain name.</param>
        /// <param name="spn">SPN</param>
        /// <param name="aType">Account type</param>
        /// <returns>Token</returns>
        public static byte[] GenerateGssApToken(string kdcIpAddr, string account, string pwd, string domain, string spn, KerberosAccountType aType)
        {
            KerberosTestClient client = new KerberosTestClient(
               domain, 
               account, 
               pwd, 
               KerberosAccountType.User, 
               kdcIpAddr, 
               88, 
               Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.TransportType.TCP,
               (KerberosConstValue.OidPkt)Enum.Parse(typeof(KerberosConstValue.OidPkt), "MSKerberosToken"));

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE | KdcOptions.OK_AS_DELEGATE;
            client.SendAsRequest(options, null);

            //Recieve preauthentication required error
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.PaEncTimeStamp paEncTimeStamp = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, client.Context.CName.Password, client.Context.CName.Salt);
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.PaPacRequest paPacRequest = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.PaPacRequest(true);
            Microsoft.Protocols.TestTools.StackSdk.Asn1.Asn1SequenceOf<PA_DATA> seqOfPaData = new Microsoft.Protocols.TestTools.StackSdk.Asn1.Asn1SequenceOf<PA_DATA>(new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            client.SendTgsRequest(spn, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();


            // client.ChangeRealm(childDomain, childDcIp, 88, Microsoft.Protocols.TestTools.StackSdk.Security.Kerberos.TransportType.TCP);
            // client.SendTgsRequest(spn, options);
            // KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.AuthorizationData data = null;
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.EncryptionKey subkey = Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.ApOptions.None,
                data,
                subkey,
                Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.ChecksumFlags.None,
                KerberosConstValue.GSSToken.GSSAPI
                );

            return token;
        }        

        public static ENTINFLIST CreateENTINFLIST(ENTINF[] entInfArray)
        {
            ENTINFLIST list = new ENTINFLIST();

            for (int i = entInfArray.Length - 1; i >= 0; i--)
            {
                if (i == entInfArray.Length - 1)
                {
                    list.pNextEntInf = null;
                    list.Entinf = entInfArray[i];
                }
                else
                {
                    ENTINFLIST[] nextList = new ENTINFLIST[1];
                    nextList[0] = list;
                    list.pNextEntInf = nextList;
                    list.Entinf = entInfArray[i];
                }
            }

            return list;

        }

        /// <summary>
        /// Validate if the specified sid is null sid (all data bytes are 0).
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <returns>True if null sid, otherwise false.</returns>
        public static bool IsNullSid(NT4SID sid)
        {
            if (sid.Data == null) return true;
            foreach (byte bd in sid.Data)
            {
                if (bd != 0) return false;
            }
            return true;
        }

        public static string GetFSMORoleCN(DsDomain dsdomain,FSMORoles fsmoRole)
        {
            string dn = null;
            if (dsdomain is AddsDomain)
            {
                string defaultNC = LdapUtility.ConvertUshortArrayToString(
                   ((AddsDomain)dsdomain).DomainNC.StringName);

                switch (fsmoRole)
                {
                    case FSMORoles.PDC:
                        {
                            dn = defaultNC;
                            break;
                        }
                    case FSMORoles.RidAllocation:
                        {
                            dn = "CN=RID Manager$,CN=System," + defaultNC;
                            break;
                        }
                    case FSMORoles.Infrastructure:
                        {
                            dn = "CN=Infrastructure," + defaultNC;
                            break;
                        }
                    default:
                        break;
                }
            }else
            {
                switch (fsmoRole)
                {
                    case FSMORoles.Schema:
                        {
                            dn = LdapUtility.ConvertUshortArrayToString(
                        dsdomain.SchemaNC.StringName);
                            break;
                        }
                    case FSMORoles.DomainNaming:
                        {
                            dn = "CN=Partitions," + LdapUtility.ConvertUshortArrayToString(
                        dsdomain.ConfigNC.StringName);
                            break;
                        }
                    default:
                        break;
                }
               
            }          

            
                
            return dn;
        }

        /// <summary>
        /// Compare if two NT4SID equals with each other
        /// </summary>
        /// <param name="sid1">Sid 1</param>
        /// <param name="sid2">Sid 2</param>
        /// <returns>True if the two sids equal with each other, otherwise false.</returns>
        public static bool NT4SID_Equals(NT4SID sid1, NT4SID sid2)
        {
            if (IsNullSid(sid1) && IsNullSid(sid2))
            {
                return true;
            }
            else if (IsNullSid(sid1) || IsNullSid(sid2))
            {
                return false;
            }

            for (int i = 0; i < sid1.Data.Length; i++)
            {
                if (sid1.Data[i] != sid2.Data[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}
