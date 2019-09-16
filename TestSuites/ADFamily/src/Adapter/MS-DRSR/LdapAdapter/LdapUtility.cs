// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Text;  // for Encoding
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Security.Principal;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{

    public struct LDAP_PROPERTY_META_DATA
    {
        /// <summary>
        ///  Attribute whose value was revealed.
        /// </summary>
        [CLSCompliant(false)]
        public uint attrType;

        /// <summary>
        ///  The version of the attribute values, starting at 1 and
        ///  increasing by one with each originating update.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        ///  The time at which the originating update was performed.
        /// </summary>
        public long timeChanged;

        /// <summary>
        ///  The invocationId of the DC that performed the originating
        ///  update.
        /// </summary>
        public Guid uuidDsaOriginating;

        /// <summary>
        ///  The USN of the DC assigned to the originating update.
        /// </summary>
        public long usnOriginating;

        /// <summary>
        /// USN of local update
        /// </summary>
        public long usnProperty;

    }

    public struct LDAP_PROPERTY_META_DATA_VECTOR
    {
        public ulong dwVersion;
        public LDAP_PROPERTY_META_DATA_VECTOR_V1 V1;
    }

    public struct LDAP_PROPERTY_META_DATA_VECTOR_V1
    {
        public ulong cNumProps;

        [Inline()]
        [Size("cNumProps")]
        public LDAP_PROPERTY_META_DATA[] rgMetaData;
    }

    public class RootDSE
    {
        public string defaultNamingContext;
        public string configurationNamingContext;
        public string schemaNamingContext;
        public string rootDomainNamingContext;

        public string serverName;
        public string dnsHostName;

        public int domainControllerFunctionality;
        public int domainFunctionality;
        public int forestFunctionality;
        public string dsServiceName;

        public bool isGcReady;
    }

    public class LdapUtility
    {
        public static string GetBinaryString(byte[] b)
        {
            StringBuilder escapedGuid = new StringBuilder();
            for (uint i = 0; i < b.Length; ++i)
                escapedGuid.AppendFormat(@"\{0:x2}", b[i]);
            return escapedGuid.ToString();
        }

        public static string GetObjectDnByGuid(DsServer dc, string baseDn, Guid guid)
        {
            return GetAttributeValueInString(
                dc,
                baseDn,
                "distinguishedName",
                "(&(objectClass=*)(objectGuid=" + GetBinaryString(guid.ToByteArray()) + "))",
                System.DirectoryServices.Protocols.SearchScope.Subtree
                );
        }

        public static string GetObjectDnBySid(DsServer dc, string baseDn, NT4SID sid)
        {
            return GetAttributeValueInString(
                dc,
                baseDn,
                "distinguishedName",
                "(&(objectClass=*)(objectSid=" + GetBinaryString(sid.Data) + "))",
                System.DirectoryServices.Protocols.SearchScope.Subtree
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt32(System.String)")]
        public static DSNAME? GetPrimaryGroup(
            DsServer dc,
            string name,
            string baseDn)
        {
            // Construct a primary group SID
            int primaryGroupId = Convert.ToInt32(GetAttributeValueInString(dc, name, "primaryGroupID"));
            byte[] userSid = GetAttributeValueInBytes(dc, name, "objectSid");

            StringBuilder escapedGroupSid = new StringBuilder();
            for (uint i = 0; i < userSid.Length - 4; ++i)
            {
                escapedGroupSid.AppendFormat(@"\{0:x2}", userSid[i]);
            }

            for (uint i = 0; i < 4; ++i)
            {
                escapedGroupSid.AppendFormat(@"\{0:x2}", (primaryGroupId & 0xff));
                primaryGroupId >>= 8;
            }

            string tfilter = "(&(objectClass=group)(objectSid=" + escapedGroupSid.ToString() + "))";
            string dn = GetAttributeValueInString(dc, baseDn, "distinguishedName", tfilter, System.DirectoryServices.Protocols.SearchScope.Subtree);

            return CreateDSNameForObject(dc, dn);
        }

        public static bool StampLessThanOrEqualUTD(DS_REPL_ATTR_META_DATA stamp, UPTODATE_VECTOR_V1_EXT utd)
        {
            for (int i = 0; i < utd.cNumCursors; ++i)
            {
                if (utd.rgCursors[i].uuidDsa == stamp.uuidLastOriginatingDsaInvocationID
                    && utd.rgCursors[i].usnHighPropUpdate >= stamp.usnOriginatingChange)
                    return true;
            }
            return false;
        }

        public static bool IsObjectExist(DsServer dc, string objDN)
        {
            SearchResultEntryCollection results;

            ResultCode r = Search(
                dc,
                objDN,
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                null,
                out results);
            if (r != ResultCode.Success)
                return false;

            return true;
        }

        public static bool IsUserExist(DsServer dc, string uname, string objDN)
        {
            SearchResultEntryCollection results;

            ResultCode r = Search(
                dc,
                objDN,
                "(samaccountname=" + uname + ")",
                System.DirectoryServices.Protocols.SearchScope.Base,
                null,
                out results);
            if (r != ResultCode.Success)
                return false;

            return true;
        }

        /// <summary>
        /// Get an object name with a proper numerical suffix so that the new object name doesn't present
        /// in the container. 
        /// 
        /// e.g. if objDn is "User" and there's an object CN=User 0 in the container, then this method
        /// should return "User 1". (note there's a space between "User" and "1")
        /// </summary>
        /// <param name="srv">DC</param>
        /// <param name="containerDn">Container of the object.</param>
        /// <param name="objDn">The object name without a numerical suffix.</param>
        /// <param name="suffix">When return, contains the numerical suffix to the new object.</param>
        /// <returns>The new object DN</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
        public static string GetAvailableSuffix(DsServer srv, string containerDn, string objDn, out int suffix)
        {
            for (int _suffix = 0; ; _suffix++)
            {
                string newObjName = objDn + "-" + _suffix.ToString();
                string newObjDn = "CN=" + newObjName + "," + containerDn;
                // Search the AD database to verify if the newObjDn already exists
                if (!IsObjectExist(srv, newObjDn))
                {
                    // There's no object under the name newObjDn, 
                    // that means we have a vacant object name to use.
                    suffix = _suffix;
                    return newObjName;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
        public static string GetAvailableSuffixForUser(DsServer srv, string containerDn, string objDn, out int suffix)
        {
            for (int _suffix = 0; ; _suffix++)
            {
                string newObjName = objDn + "-" + _suffix.ToString();
                string newObjDn = "CN=" + newObjName + "," + containerDn;
                // Search the AD database to verify if the newObjDn already exists
                if (!IsUserExist(srv, newObjName, newObjDn))
                {
                    // There's no object under the name newObjDn, 
                    // that means we have a vacant object name to use.
                    suffix = _suffix;
                    return newObjName;
                }
            }
        }

        public static DSNAME CreateDSNameForObject(DsServer srv, string dn)
        {
            // Get the GUID first
            Guid? guid = LdapUtility.GetObjectGuid(srv, dn);

            string sid = GetObjectStringSid(srv, dn);
            return DrsuapiClient.CreateDsName(dn, guid.Value, sid);
        }

        public static string GetObjectStringSid(DsServer srv, string dn)
        {
            byte[] data = GetAttributeValueInBytes(srv, dn, "objectSid");
            if (data == null)
                return null;

            SecurityIdentifier sid = new SecurityIdentifier(data, 0);
            return sid.ToString();
        }

        public static string GetObjectStringSidHistory(DsServer srv, string dn)
        {
            byte[] data = GetAttributeValueInBytes(srv, dn, "SidHistory");
            if (data == null)
                return null;

            SecurityIdentifier sid = new SecurityIdentifier(data, 0);
            return sid.ToString();
        }

        public static string ConvertUshortArrayToString(ushort[] data)
        {
            if (data == null)
                return null;

            byte[] asBytes = new byte[(data.Length - 1) * sizeof(ushort)];
            Buffer.BlockCopy(data, 0, asBytes, 0, asBytes.Length - 1);
            return Encoding.Unicode.GetString(asBytes);
        }

        /// <summary>
        /// Convert string to ushort array in Unicode mode
        /// </summary>
        /// <param name="source">the byte array to be converted.</param>
        /// <returns> the ushort array.</returns>
        public static ushort[] ConvertUnicodeStringToUshortArray(
            string source)
        {
            ushort[] target;

            if (source == null)
            {
                target = new ushort[0];
            }
            else
            {
                byte[] sourceBytes = Encoding.Unicode.GetBytes(source);
                target = new ushort[(int)Math.Ceiling((double)sourceBytes.Length / 2)];
                Buffer.BlockCopy(sourceBytes, 0, target, 0, sourceBytes.Length);
            }
            return target;
        }

        public static string GetDnFromNcType(DsServer srv, NamingContext ncType)
        {
            string baseDn = "";
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            switch (ncType)
            {
                case NamingContext.DomainNC:
                    baseDn = rootDse.defaultNamingContext;
                    break;
                case NamingContext.ConfigNC:
                    baseDn = rootDse.configurationNamingContext;
                    break;
                case NamingContext.SchemaNC:
                    baseDn = rootDse.schemaNamingContext;
                    break;
                default:
                    throw new NotImplementedException();
                //break;
            }
            return baseDn;
        }
        /// <summary>
        /// Travel down the base DN to find the object DN matching a given filter.
        /// </summary>
        /// <param name="srv">The DC the LDAP connection is connected to.</param>
        /// <param name="baseDn">The base DN.</param>
        /// <param name="filter">LDAP filter string.</param>
        /// <returns>An object DN matching the filter. This DN must be a part of the base DN.</returns>
        public static string FindObjectNameWithFilter(DsServer srv, string baseDn, string filter)
        {
            string[] rDns = baseDn.Split(',');
            if (rDns.Length <= 1)
                return null;

            for (int i = 0; i < rDns.Length; ++i)
            {
                string newDn = "";
                for (int j = i; j < rDns.Length - 1; ++j)
                    newDn += (rDns[j] + ", ");
                newDn += rDns[rDns.Length - 1];

                string name = GetAttributeValueInString(
                    srv,
                    newDn,
                    "distinguishedName",
                    filter,
                    System.DirectoryServices.Protocols.SearchScope.Base);
                if (name != null)
                    return newDn;
            }

            return null;
        }

        /// <summary>
        /// Get the RootDSE object of an LDAP connection.
        /// </summary>
        /// <param name="srv">The DC the LDAP connection is connected to.</param>
        /// <returns>The RootDSE object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToUpper"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt32(System.Object)")]
        public static RootDSE GetRootDSE(DsServer srv)
        {
            string rootDseDn = "";  // empty DN means rootDSE
            RootDSE rootDse = new RootDSE();

            rootDse.defaultNamingContext = GetAttributeValueInString(srv, rootDseDn, "defaultNamingContext");
            rootDse.configurationNamingContext = GetAttributeValueInString(srv, rootDseDn, "configurationNamingContext");
            rootDse.schemaNamingContext = GetAttributeValueInString(srv, rootDseDn, "schemaNamingContext");
            rootDse.rootDomainNamingContext = GetAttributeValueInString(srv, rootDseDn, "rootDomainNamingContext");

            rootDse.serverName = GetAttributeValueInString(srv, rootDseDn, "serverName");
            rootDse.dnsHostName = GetAttributeValueInString(srv, rootDseDn, "dnsHostName");

            rootDse.domainControllerFunctionality = Convert.ToInt32(GetAttributeValueInString(srv, rootDseDn, "domainControllerFunctionality"));
            rootDse.domainFunctionality = Convert.ToInt32(GetAttributeValueInString(srv, rootDseDn, "domainFunctionality"));
            rootDse.forestFunctionality = Convert.ToInt32(GetAttributeValueInString(srv, rootDseDn, "forestFunctionality"));
            rootDse.dsServiceName = GetAttributeValueInString(srv, rootDseDn, "dsServiceName");

            string gcReady = GetAttributeValueInString(srv, rootDseDn, "isGlobalCatalogReady");
            if (gcReady == null || gcReady.ToUpper() != "TRUE")
                rootDse.isGcReady = false;
            else
                rootDse.isGcReady = true;

            return rootDse;
        }

        public static Guid? GetObjectGuid(DsServer srv, string dn)
        {
            if (dn == null)
                return null;

            byte[] data = GetAttributeValueInBytes(srv, dn, "objectGuid");
            if (data != null)
                return new Guid(data);

            return null;
        }

        public static ResultCode Search(
            DsServer dc,
            string baseDn,
            string ldapFilter,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string[] attributesToReturn,
            out SearchResultEntryCollection results
            )
        {
            SearchResponse response = null;
            try
            {
                SearchRequest request = new SearchRequest(
                    baseDn,
                    ldapFilter,
                    searchScope,
                    attributesToReturn
                    );
                response = (SearchResponse)dc.LdapConn.SendRequest(request);
            }
            catch (DirectoryOperationException e)
            {
                results = null;
                return e.Response.ResultCode;
            }
            results = response.Entries;
            return response.ResultCode;

        }

        public static object[] GetAttributeValuesOfType(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            Type valuesType)
        {
            SearchResultEntryCollection results = null;
            ResultCode ret = Search(
                dc,
                dn,
                ldapFilter,
                searchScope,
                new string[] { attributeName },
                out results);

            if (ret != ResultCode.Success)
                return null;

            foreach (SearchResultEntry e in results)
            {
                DirectoryAttribute attr = e.Attributes[attributeName];
                if (attr == null)
                    return null;
                else
                    return attr.GetValues(valuesType);
            }

            return null;
        }

        public static string[] GetAttributeValuesString(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base)
        {
            return (string[])(GetAttributeValuesOfType(dc, dn, attributeName, ldapFilter, searchScope, typeof(string)));
        }

        public static byte[][] GetAttributeValuesBytes(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base)
        {
            return (byte[][])(GetAttributeValuesOfType(dc, dn, attributeName, ldapFilter, searchScope, typeof(byte[])));
        }

        public static string GetAttributeValueInString(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base)
        {
            string[] attrs = GetAttributeValuesString(dc, dn, attributeName, ldapFilter, searchScope);
            return attrs?[0];
        }

        public static byte[] GetAttributeValueInBytes(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base)
        {
            byte[][] attrs = GetAttributeValuesBytes(dc, dn, attributeName, ldapFilter, searchScope);
            return attrs?[0];
        }

        public static LdapConnection CreateConnection(
            string hostDnsName,
            DsUser user,
            ref DsServer srv,
            bool forceCreate = false,
            AuthType authType = AuthType.Kerberos)
        {
            if (!forceCreate && srv.LdapConn != null)
            {
                // If there is an active connection, return it.
                return srv.LdapConn;
            }

            LdapConnection conn = new LdapConnection(hostDnsName);
            if (authType == AuthType.Basic)
            {
                conn.AuthType = AuthType.Basic;
                conn.Credential = new NetworkCredential(user.Domain.NetbiosName + "\\" + user.Username, user.Password/*, user.Domain.Name*/);
            }
            else
            {
                conn.Credential = new NetworkCredential(user.Username, user.Password, user.Domain.DNSName);
            }
            conn.Timeout = new TimeSpan(0, 5, 0);
            // Bind the connection to the server
            conn.Bind();
            srv.LdapConn = conn;
            return conn;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToInt32(System.String)")]
        public static int GetPort(string hostDnsName)
        {
            return Convert.ToInt32(hostDnsName.Split(':')[1]);
        }

        // Create a DirectoryEntry using the given credential.
        public static DirectoryEntry CreateDirectoryEntry(
            DsServer dc,
            string path,
            AuthenticationTypes authType,
            bool enforceKdc = false
            )
        {

            string newPath = path;
            // Insert KDC into the path if a KDC is designated.
            if (enforceKdc)
            {
                newPath = path.Replace("LDAP://", "LDAP://" + dc.DnsHostName + "/");
            }
            return new DirectoryEntry(newPath, dc.Domain.Admin.Username, dc.Domain.Admin.Password, authType);
        }

        public static PROPERTY_META_DATA? AttrStamp(DsServer dc, string objDn, uint attrTyp)
        {
            byte[] metaDataBer = GetAttributeValueInBytes(dc, objDn, "replPropertyMetaData");
            if (metaDataBer == null)
                return null;

            PROPERTY_META_DATA_VECTOR metaData = TypeMarshal.ToStruct<PROPERTY_META_DATA_VECTOR>(metaDataBer);

            for (ulong i = 0; i < metaData.V1.cNumProps; ++i)
            {
                PROPERTY_META_DATA d = metaData.V1.rgMetaData[i];
                if (d.attrType == attrTyp)
                    return d;
            }

            return null;
        }

        public static bool StampLessThanOrEqualUTD(PROPERTY_META_DATA? stamp, UPTODATE_VECTOR_V1_EXT utd)
        {
            if (stamp == null)
                return false;

            for (int i = 0; i < utd.cNumCursors; ++i)
            {
                if (utd.rgCursors[i].uuidDsa == stamp.Value.propMetadataExt.uuidDsaOriginating
                    && utd.rgCursors[i].usnHighPropUpdate >= stamp.Value.propMetadataExt.usnOriginating)
                    return true;
            }
            return false;
        }

        public static int CompareGuid(Guid g1, Guid g2)
        {
            byte[] b1 = g1.ToByteArray();
            byte[] b2 = g2.ToByteArray();
            for (int i = 0; i < b1.Length; ++i)
            {
                if (b1[i] > b2[i])
                    return 1;
                else if (b1[i] < b2[i])
                    return -1;
            }
            return 0;
        }

        public static void InsertGuidToList(List<Guid> list, Guid guid)
        {
            // Insert the guid to the proper position in the ascending GUID list
            int sz = list.Count;
            if (sz == 0)
            {
                list.Add(guid);
                return;
            }
            for (int i = 0; i < sz; ++i)
            {
                if (CompareGuid(guid, list[i]) == 0)
                    return;

                if (CompareGuid(guid, list[i]) < 0)
                {
                    list.Insert(i, guid);
                    return;
                }
            }

            // If we reach here, add the guid to the end of the list
            list.Add(guid);
        }

        /// <summary>
        /// get SAM Account name for object
        /// </summary>
        /// <param name="svr">dc</param>
        /// <param name="dn">object dn</param>
        /// <returns>SAM name</returns>
        public static string GetObjectSAMNameFromDN(DsServer svr, string dn)
        {
            return GetAttributeValueInString(svr, dn, "samaccountname");
        }

        /// <summary>
        /// get dn from SAM account name
        /// </summary>
        /// <param name="svr">dc</param>
        /// <param name="sam">sam</param>
        /// <returns>dn</returns>
        public static string GetObjectDNFromSAMName(DsServer svr, string sam)
        {
            SearchResultEntryCollection srec = null;
            ResultCode ret = Search(svr, svr.Domain.Name, "(samaccountname=" + sam + ")", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "distinguishedname" }, out srec);
            if (ret != ResultCode.Success)
                return null;

            if (srec.Count == 0)
                return null;

            return srec[0].Attributes["distinguishedname"][0].ToString();
        }

        public static string UserNameFromNT4AccountName(string nt4AccountName)
        {
            int backSlash = nt4AccountName.IndexOf('\\');
            if (backSlash < 0)
                return null;

            return nt4AccountName.Substring(backSlash + 1);
        }

        public static string DomainNameFromNT4AccountName(string nt4AccountName)
        {
            int backSlash = nt4AccountName.IndexOf('\\');
            if (backSlash < 0)
                return null;

            return nt4AccountName.Substring(0, backSlash);
        }

        public static LDAP_PROPERTY_META_DATA[] GetMetaData(DsServer dc, string objDn)
        {
            byte[] metaDataBer = GetAttributeValueInBytes(dc, objDn, "replPropertyMetaData");
            if (metaDataBer == null)
                return null;

            LDAP_PROPERTY_META_DATA_VECTOR metaData = TypeMarshal.ToStruct<LDAP_PROPERTY_META_DATA_VECTOR>(metaDataBer);

            return metaData.V1.rgMetaData;
        }

        public static uint attrTyp(DsServer dc, string attrName)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();
            string attrOid = GetAttributeValueInString(
                    dc,
                    rootDse.schemaNamingContext,
                    "attributeID",
                    "(lDAPDisplayName=" + attrName + ")",
                    System.DirectoryServices.Protocols.SearchScope.OneLevel
                    );


            uint attrTyp = OIDUtility.MakeAttid(prefixTable, attrOid);

            return attrTyp;
        }

        public static bool ChangeUserPassword(DsServer dc, string cn, string passWord)
        {

            string baseDn = LdapUtility.GetDnFromNcType(dc, NamingContext.DomainNC);

            DirectoryContext userContext = new DirectoryContext(DirectoryContextType.Domain, dc.Domain.DNSName, dc.Domain.Admin.Username, dc.Domain.Admin.Password);

            Domain userDomain = Domain.GetDomain(userContext);
            using(DirectoryEntry userRootEntry = userDomain.GetDirectoryEntry())
            {

            cn = cn.Replace(",CN=Users," + baseDn, "");

            DirectoryEntry userEntry = null;
            bool found = false;
            while (!found)
            {
                System.Threading.Thread.Sleep(5000);
                found = true;
                try
                {
                    userEntry = userRootEntry.Children.Find("CN=Users").Children.Find(cn);
                }
                catch (Exception /* e */)
                {
                    found = false;
                }

                if (found)
                    break;
            }

            using (userEntry)
            {
                userEntry.Invoke("SetPassword", new object[] { passWord });
                userEntry.CommitChanges();
            }

            return true;
                }

        }

    }
}
