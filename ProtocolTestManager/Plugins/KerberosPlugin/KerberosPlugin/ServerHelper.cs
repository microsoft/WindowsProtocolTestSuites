// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.ActiveDirectory;

namespace Microsoft.Protocols.TestManager.KerberosPlugin
{
    public enum NetError
    {
        NERR_Success = 0,
        NERR_BASE = 2100,
        NERR_UnknownDevDir = (NERR_BASE + 16),
        NERR_DuplicateShare = (NERR_BASE + 18),
        NERR_BufTooSmall = (NERR_BASE + 23),
    }

    public static class ServerHelper
    {
        #region Native API

        const uint MAX_PREFERRED_LENGTH = 0xFFFFFFFF;
        const int NERR_Success = 0;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHARE_INFO_1
        {
            public string shi1_netname;
            public uint shi1_type;
            public string shi1_remark;
            public SHARE_INFO_1(string sharename, uint sharetype, string remark)
            {
                this.shi1_netname = sharename;
                this.shi1_type = sharetype;
                this.shi1_remark = remark;
            }
            public override string ToString()
            {
                return shi1_netname;
            }
        }

        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int NetShareEnum(
             StringBuilder ServerName,
             int level,
             ref IntPtr bufPtr,
             uint prefmaxlen,
             ref int entriesread,
             ref int totalentries,
             ref int resume_handle
             );

        [DllImport("Netapi32", CharSet = CharSet.Auto)]
        static extern int NetApiBufferFree(IntPtr Buffer);

        #endregion

        public static bool URLExists(string url)
        {
            //try
            //{

            //    HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            //    request.Method = "HEAD";
            //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //    return (response.StatusCode == HttpStatusCode.OK);

            //}
            //catch
            //{
            //    return false;
            //}
            return true;
        }

        public static string[] EnumShares(string serverName, string userName, string domainName, string password)
        {
            string[] shareList = null;

            try
            {
                using (new ImpersonationHelper(userName, domainName, password))
                {
                    List<string> ShareInfos = new List<string>();
                    int entriesread = 0;
                    int totalentries = 0;
                    int resume_handle = 0;
                    IntPtr bufPtr = IntPtr.Zero;
                    int nStructSize = Marshal.SizeOf(typeof(SHARE_INFO_1));
                    StringBuilder server = new StringBuilder(serverName);

                    if (NetShareEnum(server, 1, ref bufPtr, MAX_PREFERRED_LENGTH, ref entriesread, ref totalentries, ref resume_handle)
                        == (int)NetError.NERR_Success)
                    {
                        IntPtr currentPtr = bufPtr;

                        for (int i = 0; i < entriesread; i++)
                        {
                            SHARE_INFO_1 shi1 = (SHARE_INFO_1)Marshal.PtrToStructure(currentPtr, typeof(SHARE_INFO_1));
                            ShareInfos.Add(shi1.shi1_netname);
                            currentPtr = new IntPtr(currentPtr.ToInt64() + nStructSize);
                        }

                        NetApiBufferFree(bufPtr);
                        shareList = ShareInfos.ToArray();
                    }
                }
            }
            catch (ImersonationFailureException ex)
            {
                throw new ApplicationException(string.Format("Failed to impersontate user {0}/{1} when enumerate share on server {2}", domainName, userName, serverName), ex);
            }
            catch
            {
                throw new ApplicationException(string.Format("Failed to enumerate share on server {0}", serverName));
            }

            return shareList;
        }


        public static string GetDomainFunctionalLevel(string domainName, string domainAdminName, string domainAdminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(domainAdminName, domainAdminPwd, domainName);
            connection.Credential = cred;
            string attributeName = "domainControllerFunctionality";
            
            string attributeValue = null;
            object[] attribute = null;
            try
            {
                SearchRequest searchRequest = new SearchRequest(null, "(objectClass=*)", SearchScope.Base, attributeName);
                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                attribute = attributes[attributeName].GetValues(typeof(string));
            }            
            catch(Exception ex)
            {
                throw new Exception("Failed to query Domain Controller Functionality.", ex);
            }

            if (attribute.Length > 1)
            {
                throw new Exception("Attribute has more than one entry.");
            }
            attributeValue = Convert.ToString(attribute[0]);

            return attributeValue;
        }
        /// <summary>
        /// get specified attribute of an account 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountType"></param>
        /// <param name="attributeName"></param>
        /// <param name="domainName"></param>
        /// <param name="domainAdminName"></param>
        /// <param name="domainAdminPwd"></param>
        /// <returns></returns>
        public static string GetAccountAttribute(string accountName, string accountType, string attributeName, string domainName, string domainAdminName, string domainAdminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(domainAdminName, domainAdminPwd, domainName);
            connection.Credential = cred;

            //get domain DN name
            string[] domainDn = domainName.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < domainDn.Length; i++)
            {
                sb.Append("dc=");
                sb.Append(domainDn[i]);
                if (i != domainDn.Length - 1)
                {
                    sb.Append(",");
                }
            }
            string dn = sb.ToString();
            string targetOu = "CN=" + accountType + "," + dn;
            string filter = "CN=" + accountName;
            string[] attributesToReturn = new string[] { attributeName };

            SearchRequest searchRequest = null;
            SearchResponse searchResponse = null;
            string attributeValue = null;

            try
            {
                searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, attributesToReturn);

                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                object[] attribute = null;

                try
                {
                    attribute = attributes[attributeName].GetValues(Type.GetType("System.String"));
                }
                catch
                {
                    return null;
                }
                if (attribute.Length > 1)
                {
                    throw new Exception("Attribute has more than one entry.");
                }

                attributeValue = Convert.ToString(attribute[0]);

            }
            catch (Exception ex)
            {
                throw new Exception("Requst attribute failed with targetOU: " + targetOu + ", filter: " + filter + ", attribute: msDS-Behavior-Version. " + ex.Message);
            }

            return attributeValue;
        }

        /// <summary>
        /// get a bunch of accounts with same type, such as computers, or users at once
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="domainName"></param>
        /// <param name="domainAdminName"></param>
        /// <param name="domainAdminPwd"></param>
        /// <returns></returns>
        public static string GetAccounts(string accountType, string domainName, string domainAdminName, string domainAdminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(domainAdminName, domainAdminPwd, domainName);
            connection.Credential = cred;

            //get domain DN name
            string[] domainDn = domainName.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < domainDn.Length; i++)
            {
                sb.Append("dc=");
                sb.Append(domainDn[i]);
                if (i != domainDn.Length - 1)
                {
                    sb.Append(",");
                }
            }
            string dn = sb.ToString();
            string targetOu = "CN=" + accountType + "," + dn;
            //string[] accounts = new string[] { };

            SearchRequest searchRequest = null;
            SearchResponse searchResponse = null;
            string attributeValue = null;
            string filter = "CN=*";
            string attributeName = "users";
            string[] accounts = new string[] { attributeName };

            try
            {
                searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, accounts);

                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                //object[] attribute = null;

                //try
                //{
                //    attribute = attributes[attributeName].GetValues(Type.GetType("System.String"));
                //}
                //catch
                //{
                //    return null;
                //}
                //if (attribute.Length > 1)
                //{
                //    throw new Exception("Attribute has more than one entry.");
                //}

                //attributeValue = Convert.ToString(attribute[0]);

            }
            catch (Exception ex)
            {
                throw new Exception("Requst attribute failed with targetOU: " + targetOu + ", filter: " + filter + ", attribute: msDS-Behavior-Version. " + ex.Message);
            }

            return attributeValue;
        }
        public static string GetDCAttribute(string computerName, string attributeName, string domainName, string domainAdminName, string domainAdminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(domainAdminName, domainAdminPwd, domainName);
            connection.Credential = cred;

            //get domain DN name
            string[] domainDn = domainName.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < domainDn.Length; i++)
            {
                sb.Append("dc=");
                sb.Append(domainDn[i]);
                if (i != domainDn.Length - 1)
                {
                    sb.Append(",");
                }
            }
            string dn = sb.ToString();
            string targetOu = "OU=Domain Controllers," + dn;
            string filter = "CN=" + computerName;
            string[] attributesToReturn = new string[] { attributeName };

            SearchRequest searchRequest = null;
            SearchResponse searchResponse = null;
            string attributeValue = null;

            try
            {
                searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, attributesToReturn);

                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                object[] attribute = null;

                try
                {
                    attribute = attributes[attributeName].GetValues(Type.GetType("System.String"));
                }
                catch
                {
                    return null;
                }
                if (attribute.Length > 1)
                {
                    throw new Exception("Attribute has more than one entry.");
                }

                attributeValue = Convert.ToString(attribute[0]);

            }
            catch (Exception ex)
            {
                throw new Exception("Requst attribute failed with targetOU: " + targetOu + ", filter: " + filter + ", attribute: " + attributeName + ". " + ex.Message);
            }

            return attributeValue;
        }

        public static string GetDCIP(string domainName)
        {
            try
            {
                DirectoryContext mycontext = new DirectoryContext(DirectoryContextType.Domain, domainName);
                DomainController dc = DomainController.FindOne(mycontext);
                return dc.IPAddress;
            }
            catch
            {
                return null;
            }
        }

        public static string GetComputerIP(string computerName)
        {
            IPHostEntry host;
            string localIP = string.Empty;
            host = Dns.GetHostEntry(computerName);
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        #region Config Helper APIs
        public static List<string> ConstructValueList(string defaultValue, params string[] possibleValues)
        {
            List<string> values = new List<string>();
            values.Add(defaultValue);
            if (possibleValues != null)
            {
                foreach (var value in possibleValues)
                {
                    if (value != defaultValue) values.Add(value);
                }
            }
            return values;
        }

        public static List<string> ConstructValueListUsingPtfConfig(string propertyName, params string[] possibleValues)
        {
            List<string> values = new List<string>();
            string defaultValue = Detector.DetectorUtil.GetPropertyValue(propertyName);
            values.Add(defaultValue);
            if (possibleValues != null)
            {
                foreach (var value in possibleValues)
                {
                    if (value != defaultValue) values.Add(value);
                }
            }
            return values;
        }
        #endregion
    }

}
