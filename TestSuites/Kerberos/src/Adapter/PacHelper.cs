// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public class PacHelper
    {

        public static string ConvertUnicode2String(ushort[] unicodeString)
        {
            string decodeString = null;
            foreach (ushort unicodeChar in unicodeString)
            {
                decodeString += char.ConvertFromUtf32(unicodeChar).ToString();
            }
            return decodeString;
        }

        public static string GetDomainDnFromDomainName(string domainName)
        {
            string[] strs = domainName.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strs.Length; i++)
            {
                sb.Append("dc=");
                sb.Append(strs[i]);
                if (i != strs.Length - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        public class commonUserFields
        {
            public long? LogonTime;
            public long? LogonHours;
            public long? AccountExpires;
            public long? LogoffTime;
            public long? KickOffTime;
            public long? PasswordLastSet;
            public long? dBCSPwd;
            public long? unicodePwd;
            public long? PasswordCanChange;
            public uint? userAccountControl;
            public long? PasswordMustChange;
            public ushort? LogonCount;
            public ushort? BadPwdCount;
            public string objectSid;
            public uint? userId;
            public uint? primaryGroupId;

            public uint groupCount;
            public uint[] groupIds;

            public uint[] domainSid;

            public commonUserFields() { }
        }

        public static long? GetAttributeFileTime(SearchResultAttributeCollection attributes, string attrName)
        {
            object[] obj = null;
            try
            {
                obj = attributes[attrName].GetValues(Type.GetType("System.String"));
            }
            catch
            {
                return null;
            }
            if (obj.Length > 1)
            {
                throw new Exception("Attribute has more than one entry.");
            }

            return Convert.ToInt64(obj[0]);
        }

        public static long? GetLogoffTime(long? logonHours, long? accountExpires)
        {
            long? logoffTime;

            if (logonHours == null && accountExpires == null)
            {
                return null;
            }
            else if (logonHours == null)
            {
                return accountExpires;
            }
            else if (accountExpires == null)
            {
                return logonHours;
            }
            
            logoffTime = (logonHours < accountExpires) ? logonHours : accountExpires;
            return logoffTime;
        }

        public static long? GetKickoffTime(long? logoffTime)
        {
            long? kickoffTime;

            if (logoffTime == null)
            {
                kickoffTime = null;
            }
            else
            { 
                //cannot find forcelogoff in AD
                //kickoffTime = logoffTime + forcelogoffTime;
                kickoffTime = logoffTime;
            }
            return kickoffTime;
        }

        public static long? GetPasswordCanChange(long? dBCSPwd, long? unicodePwd, long? passwordLastSet)
        {
            long? passwordCanChange;

            if (passwordLastSet != null)
            {
                //Fix me
                //need to configure effective-minimumPasswordAge attribute to 1 day, can use Get-ADDefaultDomainPasswordPolicy to check
                //or use LDAP to search DN: DC=CONTOSO,DC=COM to get minPwdAge attribute
                passwordCanChange = System.DateTime.FromFileTime(passwordLastSet.Value).AddDays(1).ToFileTime();
            }
            else
            {
                passwordCanChange = null;
            }
            //Fix me
            //unclear in SAMR 3.1.5.14.3
            //if (dBCSPwd == null || unicodePwd == null || either of these equal to the respective hash of a zero-length string)
            //{
            //    passwordCanChange = 0;
            //}
            return passwordCanChange;
        }

       
        public static object getAttributeValue(SearchResultAttributeCollection attributes, string attrName)
        {
            object[] obj = null;
            try
            {
                obj = attributes[attrName].GetValues(Type.GetType("System.String"));
            }
            catch
            {
                return null;
            }
            if (obj.Length > 1)
            {
                throw new Exception("Attribute has more than one entry.");
            }

            return obj[0];
        }
        

        public static long? GetPasswordMustChange(uint? userAccountControl, long? passwordLastSet)
        {
            long? passwordMustChange;

            if (userAccountControl != null)
            {
                if (((AdtsUserAccountControl)userAccountControl & AdtsUserAccountControl.ADS_UF_DONT_EXPIRE_PASSWD) == AdtsUserAccountControl.ADS_UF_DONT_EXPIRE_PASSWD
                    || ((AdtsUserAccountControl)userAccountControl & AdtsUserAccountControl.ADS_UF_SMARTCARD_REQUIRED) == AdtsUserAccountControl.ADS_UF_SMARTCARD_REQUIRED
                    || ((AdtsUserAccountControl)userAccountControl & AdtsUserAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT) == AdtsUserAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT
                    || ((AdtsUserAccountControl)userAccountControl & AdtsUserAccountControl.ADS_UF_WORKSTATION_TRUST_ACCOUNT) == AdtsUserAccountControl.ADS_UF_WORKSTATION_TRUST_ACCOUNT
                    || ((AdtsUserAccountControl)userAccountControl & AdtsUserAccountControl.ADS_UF_SERVER_TRUST_ACCOUNT) == AdtsUserAccountControl.ADS_UF_SERVER_TRUST_ACCOUNT)
                {
                    passwordMustChange = 0x7FFFFFFFFFFFFFFF;
                }
                else if (passwordLastSet == null)
                {
                    passwordMustChange = 0;
                }
                else
                {
                    //Fix me
                    //By default, the Effective-MaxmiumPasswordAge is set to 42 days
                    passwordMustChange = System.DateTime.FromFileTime(passwordLastSet.Value).AddDays(42).ToFileTime();
                }
            }
            else
            {
                passwordMustChange = null;
            }

            return passwordMustChange;
        }

        public static ushort? GetAttributeUshort(SearchResultAttributeCollection attributes, string attrName)
        {
            object[] obj = null;
            try
            {
                obj = attributes[attrName].GetValues(Type.GetType("System.String"));
            }
            catch
            {
                //unable to get attrName attribute, value unset
                return null;
            }
            if (obj.Length > 1)
            {
                throw new Exception("Attribute has more than one entry.");
            }

            return Convert.ToUInt16(obj[0]);
        }

        public static string GetobjectSid(SearchResultAttributeCollection attributes)
        {
            object[] obj = null;
            try
            {
                obj = attributes["objectSid"].GetValues(Type.GetType("System.Byte[]"));
            }
            catch
            {
                //unable to get objectSid attribute, value unset
                return null;
            }
            if (obj.Length > 1)
            {
                throw new Exception("objectSid has more than one entry.");
            }

            SecurityIdentifier objectSid = new SecurityIdentifier((byte[])obj[0], 0);
            return objectSid.ToString();
        }

        public static uint GetGroupCount(SearchResultAttributeCollection attributes)
        {
            //There is one built-in group: Domain Users group
            return (uint)attributes["memberOf"].Count + 1;
        }

        public static uint[] GetGroupIds(SearchResultAttributeCollection attributes, string domainName, NetworkCredential cred)
        {
            LdapConnection connection = new LdapConnection(domainName);
            connection.Credential = cred;
            int groupCount = attributes["memberOf"].Count + 1;
            uint[] rid = new uint[groupCount];

            //Fix me
            //The built-in groupmembership Domain Users Rid = 513
            rid[0] = 513;
            for (int i = 1; i < groupCount; i++)
            {
                string dn = GetDomainDnFromDomainName(domainName);
                string targetOu = "cn=Users," + dn;
                string[] filter = attributes["memberOf"][i - 1].ToString().Split(',');
                SearchRequest searchRequest = new SearchRequest(targetOu, filter[0], SearchScope.Subtree, "objectSid");
                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                if (searchResponse.Entries.Count > 1)
                {
                    throw new Exception("There are more than one entries with the same groupName.");
                }
                SearchResultAttributeCollection groupAttributes = searchResponse.Entries[0].Attributes;
                string[] tmp = GetobjectSid(groupAttributes).Split('-');
                rid[i] = Convert.ToUInt32(tmp[tmp.Length - 1]);
            }

            return rid;
        }

        public static uint[] GetDomainSid(string domainName, NetworkCredential cred)
        {
            LdapConnection connection = new LdapConnection(domainName);
            connection.Credential = cred;
            string dn = GetDomainDnFromDomainName(domainName);
            string filter = "(objectClass=*)";
            SearchRequest searchRequest = new SearchRequest(dn, filter, SearchScope.Base, "objectSid");
            SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            if (searchResponse.Entries.Count > 1)
            {
                throw new Exception("There are more than one entries with the same domain distinguishedName.");
            }
            SearchResultAttributeCollection domainObjAttributes = searchResponse.Entries[0].Attributes;
            string[] tmp = GetobjectSid(domainObjAttributes).Split('-');
            uint[] domainSid = new uint[tmp.Length - 3];
            for (int i=0; i<tmp.Length-3; i++)
            {
                domainSid[i] = Convert.ToUInt32(tmp[i + 3]);
            }

            return domainSid;
        }

        public static commonUserFields GetCommonUserFields(string domainName, string userName, NetworkCredential cred)
        {
            LdapConnection connection = new LdapConnection(domainName);
            connection.Credential = cred;
            string dn = GetDomainDnFromDomainName(domainName);
            string targetOu = "cn=Users," + dn;
            string filter = "cn=" + userName;
            string[] attributesToReturn = new string[] { "lastLogon", "logonHours", "accountExpires", "pwdLastSet", "dBCSPwd", "unicodePwd", "userAccountControl", "logonCount", "badPwdCount", "objectSid", "primaryGroupID", "memberOf" };
            commonUserFields userFields = new commonUserFields();

            SearchRequest searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, attributesToReturn);
            
            SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            if (searchResponse.Entries.Count > 1)
            {
                throw new Exception("There are more than one entries with the same userName.");
            }
            SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;

            userFields.LogonTime = GetAttributeFileTime(attributes, "lastLogon");
            userFields.LogonHours = GetAttributeFileTime(attributes, "logonHours");
            userFields.AccountExpires = GetAttributeFileTime(attributes, "accountExpires");
            userFields.LogoffTime = GetLogoffTime(userFields.LogonHours, userFields.AccountExpires);
            userFields.KickOffTime = GetKickoffTime(userFields.LogoffTime);
            userFields.PasswordLastSet = GetAttributeFileTime(attributes, "pwdLastSet");
            userFields.dBCSPwd = GetAttributeFileTime(attributes, "dBCSPwd");
            userFields.unicodePwd = GetAttributeFileTime(attributes, "unicodePwd");
            userFields.PasswordCanChange = GetPasswordCanChange(userFields.dBCSPwd, userFields.unicodePwd, userFields.PasswordLastSet);

            object attributeValue = null;
            attributeValue =  getAttributeValue(attributes, "userAccountControl");
            userFields.userAccountControl = (uint?) Convert.ToInt32(attributeValue);
                
            userFields.PasswordMustChange = GetPasswordMustChange(userFields.userAccountControl, userFields.PasswordLastSet);
            userFields.LogonCount = GetAttributeUshort(attributes, "logonCount");
            userFields.BadPwdCount = GetAttributeUshort(attributes, "badPwdCount");
            userFields.objectSid = GetobjectSid(attributes);
            if (userFields.objectSid == null)
            {
                userFields.userId = null;
            }
            else
            {
                string[] tmp = userFields.objectSid.Split('-');
                userFields.userId = Convert.ToUInt32(tmp[tmp.Length - 1]);
            }
             
            attributeValue = getAttributeValue(attributes, "primaryGroupID");
            userFields.primaryGroupId = (uint?)Convert.ToInt32(attributeValue);

            userFields.groupCount = GetGroupCount(attributes);
            if (userFields.groupCount > 0)
            {
                userFields.groupIds = GetGroupIds(attributes, domainName, cred);
            }

            userFields.domainSid = GetDomainSid(domainName, cred);
            return userFields;
        }

        public static uint[] GetResourceGroupIds(string domainName, NetworkCredential cred, uint resourceGroupCount, Group[] resourceGroups)
        {
            LdapConnection connection = new LdapConnection(domainName);
            connection.Credential = cred;
            uint[] rid = new uint[resourceGroupCount];

            for (int i = 0; i < resourceGroupCount; i++)
            {
                string dn = GetDomainDnFromDomainName(domainName);
                string targetOu = dn;
                string filter = "cn=" + resourceGroups[i].GroupName;
                SearchRequest searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, "objectSid");
                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                if (searchResponse.Entries.Count > 1)
                {
                    throw new Exception("There are more than one entries with the same resourceGroupName.");
                }
                SearchResultAttributeCollection groupAttributes = searchResponse.Entries[0].Attributes;
                string[] tmp = GetobjectSid(groupAttributes).Split('-');
                rid[i] = Convert.ToUInt32(tmp[tmp.Length - 1]);
            }

            return rid;
        }

        public static KERB_SID_AND_ATTRIBUTES[] GetResourceGroupExtraSids(string domainName, NetworkCredential cred, uint resourceGroupCount, Group[] resourceGroups)
        {
            LdapConnection connection = new LdapConnection(domainName);
            connection.Credential = cred;
            KERB_SID_AND_ATTRIBUTES[] resourceGroupExtraSids = new KERB_SID_AND_ATTRIBUTES[resourceGroupCount];

            for (int i = 0; i < resourceGroupCount; i++)
            {
                string dn = GetDomainDnFromDomainName(domainName);
                string targetOu = dn;
                string filter = "cn=" + resourceGroups[i].GroupName;
                SearchRequest searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, "objectSid");
                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                if (searchResponse.Entries.Count > 1)
                {
                    throw new Exception("There are more than one entries with the same resourceGroupName.");
                }
                SearchResultAttributeCollection groupAttributes = searchResponse.Entries[0].Attributes;
                string[] tmp = GetobjectSid(groupAttributes).Split('-');
                
                _RPC_SID resourceGroupSid = new _RPC_SID();
                resourceGroupSid.Revision = 0x01;
                resourceGroupSid.IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                resourceGroupSid.IdentifierAuthority.Value = new byte[] { 0, 0, 0, 0, 0, 5 };
                resourceGroupSid.SubAuthorityCount = Convert.ToByte(tmp.Length - 3);
                resourceGroupSid.SubAuthority = new uint[tmp.Length - 3];
                for (int j = 3; j < tmp.Length; j++)
                {
                    resourceGroupSid.SubAuthority[j - 3] = Convert.ToUInt32(tmp[j]);
                }
                
                resourceGroupExtraSids[i] = new KERB_SID_AND_ATTRIBUTES();
                resourceGroupExtraSids[i].Attributes = Attributes_Values.Mandatory | Attributes_Values.EnabledByDefault | Attributes_Values.Enabled | Attributes_Values.Resource;
                resourceGroupExtraSids[i].SID = new _RPC_SID[1];
                resourceGroupExtraSids[i].SID[0] = resourceGroupSid;
            }
            return resourceGroupExtraSids;
        }

        public static bool ExtraSidsAreEqual(KERB_SID_AND_ATTRIBUTES sid1, KERB_SID_AND_ATTRIBUTES sid2)
        {
            if (sid1.Attributes != sid2.Attributes)
            {
                return false;
            }
            if (sid1.SID[0].Revision != sid2.SID[0].Revision)
            { 
                return false; 
            }
            
            if (!sid1.SID[0].IdentifierAuthority.Value.SequenceEqual(sid2.SID[0].IdentifierAuthority.Value))
            {
                return false;
            }

            if (sid1.SID[0].SubAuthorityCount != sid2.SID[0].SubAuthorityCount)
            {
                return false;
            }

            if (!sid1.SID[0].SubAuthority.SequenceEqual(sid2.SID[0].SubAuthority))
            {
                return false;
            }

            return true;
        }
    }   
}
