// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Net;
using System.Text;
using System.Threading;
using System.Linq;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public partial class LdapAdapter : ManagedAdapterBase, ILdapAdapter
    {

        #region Base Ops
        /// <summary>
        /// Check if the LDAP service on a remote server is available.
        /// </summary>
        /// <param name="serverNetworkAddress">The network address of the remote server.</param>
        /// <returns>True if LDAP service on the remote server is available, false otherwise.</returns>
        public bool IsReachable(string serverNetworkAddress)
        {
            LdapConnection conn = null;
            try
            {
                conn = new LdapConnection(serverNetworkAddress);
                conn.Bind();
            }
            catch (LdapException)
            {
                return false;
            }
            finally
            {
                conn.Dispose();
            }
            return true;
        }

        /// <summary>
        /// Add an object to the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to add.</param>
        /// <param name="objectClass">objectClass attribute of the object to add.</param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode AddObject(DsServer dc, string dn, string objectClass)
        {

            AddRequest request = new AddRequest(dn, objectClass);
            AddResponse response = (AddResponse)dc.LdapConn.SendRequest(request);

            return response.ResultCode;
        }

        /// <summary>
        /// Add an object to the directory, with attributes.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to add.</param>
        /// <param name="attributes">Collection of attributes to be added to the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode AddObjectWithAttributes(
                DsServer dc,
                string dn,
                DirectoryAttributeCollection attributes
                )
        {

            DirectoryAttribute[] attrs = new DirectoryAttribute[attributes.Count];
            for (int i = 0; i < attributes.Count; ++i)
            {
                attrs[i] = attributes[i];
            }

            AddRequest request = new AddRequest(dn, attrs);
            AddResponse response = (AddResponse)dc.LdapConn.SendRequest(request);

            return response.ResultCode;
        }

        /// <summary>
        /// Move an object to a new parent DN, and change the object name if necessary.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to move/rename.</param>
        /// <param name="newParentDn">The parent DN the object is to be located.</param>
        /// <param name="newObjectName">The new name of the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode MoveRenameObject(
                DsServer dc,
                string dn,
                string newParentDn,
                string newObjectName
                )
        {
            ModifyDNRequest request = new ModifyDNRequest(dn, newParentDn, newObjectName);
            ModifyDNResponse response = (ModifyDNResponse)dc.LdapConn.SendRequest(request);

            return response.ResultCode;

        }

        /// <summary>
        /// Delete an object to the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to delete.</param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode DeleteObject(DsServer dc, string dn)
        {
            Site.Log.Add(LogEntryKind.Checkpoint, "Start to delete object: " + dn);
            DeleteRequest request = new DeleteRequest(dn);
            request.Controls.Add(new TreeDeleteControl());
            DeleteResponse response = (DeleteResponse)dc.LdapConn.SendRequest(request);
            Site.Log.Add(LogEntryKind.Checkpoint, "ResultCode: " + response.ResultCode.ToString());
            return response.ResultCode;
        }

        /// <summary>
        /// Add an attribute to an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to add.
        /// </param>
        /// <param name="attribute">The attribute to be added to the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode AddAttribute(
                DsServer dc,
                string dn,
                DirectoryAttribute attribute
                )
        {
            ModifyRequest request;

            DirectoryAttributeModification mods = new DirectoryAttributeModification();
            mods.Name = attribute.Name;
            foreach (object i in attribute)
            {
                if (i is string)
                {
                    mods.Add((string)i);
                }
                else if (i is byte[])
                {
                    mods.Add((byte[])i);
                }
                else if (i is Uri)
                {
                    mods.Add((Uri)i);
                }
            }

            try
            {
                mods.Operation = DirectoryAttributeOperation.Add;
                request = new ModifyRequest(dn, mods);
                ModifyResponse response = (ModifyResponse)dc.LdapConn.SendRequest(request);

                return response.ResultCode;
            }
            catch (DirectoryOperationException)
            {
                // If the attribute exists, update the value
                mods.Operation = DirectoryAttributeOperation.Replace;
                request = new ModifyRequest(dn, mods);
                ModifyResponse response = (ModifyResponse)dc.LdapConn.SendRequest(request);

                return response.ResultCode;

            }
        }

        /// <summary>
        /// Add attribute(s) to an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to add.
        /// </param>
        /// <param name="attributes">
        /// Collection of attributes to be added to the object.
        /// </param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode AddAttributes(
                DsServer dc,
                string dn,
                DirectoryAttributeCollection attributes
                )
        {

            ModifyRequest request;

            ResultCode r = ResultCode.Other;
            foreach (DirectoryAttribute attribute in attributes)
            {
                DirectoryAttributeModification mods = new DirectoryAttributeModification();
                mods.Name = attribute.Name;
                foreach (object i in attribute)
                {
                    if (i is string)
                    {
                        mods.Add((string)i);
                    }
                    else if (i is byte[])
                    {
                        mods.Add((byte[])i);
                    }
                    else if (i is Uri)
                    {
                        mods.Add((Uri)i);
                    }
                }

                try
                {
                    mods.Operation = DirectoryAttributeOperation.Add;
                    request = new ModifyRequest(dn, mods);
                    ModifyResponse response = (ModifyResponse)dc.LdapConn.SendRequest(request);

                    r = response.ResultCode;
                }
                catch (DirectoryOperationException)
                {

                    // If the attribute exists, update the value
                    mods.Operation = DirectoryAttributeOperation.Replace;
                    request = new ModifyRequest(dn, mods);
                    ModifyResponse response = (ModifyResponse)dc.LdapConn.SendRequest(request);

                    r = response.ResultCode;

                }
            }

            return r;
        }

        /// <summary>
        /// Modify an attribute of an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to modify.
        /// </param>
        /// <param name="attribute">
        /// The attribute to be modified in the object.
        /// </param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode ModifyAttribute(
                DsServer dc,
                string dn,
                DirectoryAttribute attribute
                )
        {

            ModifyRequest request;
            DirectoryAttributeModification mods = new DirectoryAttributeModification();
            mods.Name = attribute.Name;
            foreach (object i in attribute)
            {
                if (i is string)
                {
                    mods.Add((string)i);
                }
                else if (i is byte[])
                {
                    mods.Add((byte[])i);
                }
                else if (i is Uri)
                {
                    mods.Add((Uri)i);
                }
            }
            mods.Operation = DirectoryAttributeOperation.Replace;
            request = new ModifyRequest(dn, mods);
            ModifyResponse response = null;
            try
            {
                response = (ModifyResponse)dc.LdapConn.SendRequest(request);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.EntryAlreadyExists)
                {
                    return ResultCode.Success;
                }
                else
                {
                    return e.Response.ResultCode;
                }
            }

            return response.ResultCode;

        }

        /// <summary>
        /// Modify attributes of an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to modify.
        /// </param>
        /// <param name="attributes">
        /// Collection of attributes to be modified in the object.
        /// </param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode ModifyAttributes(
                DsServer dc,
                string dn,
                DirectoryAttributeCollection attributes
                )
        {
            ModifyRequest request;
            ResultCode r = ResultCode.Other;
            foreach (DirectoryAttribute attribute in attributes)
            {
                DirectoryAttributeModification mods = new DirectoryAttributeModification();
                mods.Name = attribute.Name;
                foreach (object i in attribute)
                {
                    if (i is string)
                    {
                        mods.Add((string)i);
                    }
                    else if (i is byte[])
                    {
                        mods.Add((byte[])i);
                    }
                    else if (i is Uri)
                    {
                        mods.Add((Uri)i);
                    }
                }


                mods.Operation = DirectoryAttributeOperation.Replace;
                request = new ModifyRequest(dn, mods);
                ModifyResponse response = (ModifyResponse)dc.LdapConn.SendRequest(request);

                r = response.ResultCode;
            }

            return r;
        }

        public byte[] GetAttributeValueInBytes(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base)
        {
            return LdapUtility.GetAttributeValueInBytes(dc, dn, attributeName, ldapFilter, searchScope);
        }

        public string GetAttributeValueInString(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base)
        {
            return LdapUtility.GetAttributeValueInString(dc, dn, attributeName, ldapFilter, searchScope);
        }

        /// <summary>
        /// Delete an attribute from an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to delete.
        /// </param>
        /// <param name="attributeName">The attribute name to be deleted from the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode DeleteAttribute(DsServer dc, string dn, string attributeName)
        {

            ModifyRequest request = new ModifyRequest(
                dn,
                DirectoryAttributeOperation.Delete,
                attributeName);
            ModifyResponse response = (ModifyResponse)dc.LdapConn.SendRequest(request);
            return response.ResultCode;

        }

        /// <summary>
        /// Delete attributes from an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to delete.
        /// </param>
        /// <param name="attributeNames">
        /// The array of attribute names to be deleted from the object.
        /// </param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode DeleteAttributes(
                DsServer dc,
                string dn,
                string[] attributeNames
                )
        {
            ResultCode re = ResultCode.Other;

            foreach (string attrName in attributeNames)
            {
                ModifyRequest request = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Delete,
                    attrName);
                ModifyResponse response = (ModifyResponse)dc.LdapConn.SendRequest(request);
                re = response.ResultCode;
            }

            return re;
        }

        /// <summary>
        /// Get the user DN of a DsUser object.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="user">The DsUser object.</param>
        /// <returns>The DN of the user.</returns>
        public string GetUserDn(DsServer dc, DsUser user)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            return GetAttributeValueInString(
                dc,
                rootDse.defaultNamingContext,
                "distinguishedName",
                "(&(objectClass=user)(name=" + user.Username + "))",
                System.DirectoryServices.Protocols.SearchScope.Subtree);

        }

        public ResultCode AddObjectToGroup(DsServer dc, string objDn, string grpDn)
        {
            string[] members = LdapUtility.GetAttributeValuesString(dc, grpDn, "member");
            string[] newMembers = null;
            if (members != null)
            {
                newMembers = new string[members.Length + 1];
                Array.Copy(members, newMembers, members.Length);
            }
            else
            {
                newMembers = new string[1];
            }
            newMembers[newMembers.Length - 1] = objDn;

            return ModifyAttribute(dc, grpDn, new DirectoryAttribute("member", newMembers));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        public ResultCode RemoveObjectFromGroup(DsServer dc, string obj, string grpDn)
        {
            string[] members = LdapUtility.GetAttributeValuesString(dc, grpDn, "member");
            string objDn = obj.ToLower();

            if (members == null)
            {
                return ResultCode.Success;
            }
            DirectoryAttributeModification mods = new DirectoryAttributeModification();
            mods.Operation = DirectoryAttributeOperation.Delete;
            mods.Name = "member";
            mods.Add(System.Text.Encoding.UTF8.GetBytes(obj));
            ModifyRequest request = new ModifyRequest(grpDn, mods);
            ModifyResponse response = null;
            try
            {
                response = (ModifyResponse)dc.LdapConn.SendRequest(request);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.EntryAlreadyExists)
                {
                    return ResultCode.Success;
                }
                else
                {
                    return e.Response.ResultCode;
                }
            }

            return response.ResultCode;
        }

        /// <summary>
        /// Get the NgcKey of a computer object.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="dn">The computer object.</param>
        /// <returns>The NgcKey of the computer.</returns>
        public string GetNgcKey(DsServer dc, string dn)
        {
            string attrName = "msDS-KeyCredentialLink";
            string filter = "(objectClass=*)";
            System.DirectoryServices.Protocols.SearchScope scope = System.DirectoryServices.Protocols.SearchScope.Subtree;
            string binVal = GetAttributeValueInString(dc, dn, attrName, filter, scope);

            string[] result = binVal.Split(new string[] {":"}, StringSplitOptions.RemoveEmptyEntries);
            return result[result.Length - 1];
        }

        /// <summary>
        /// Set the msDS-KeyCredentialLink of a computer object according to the given Ngc Key.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="dn">The computer object.</param>
        /// <param name="ngcKey">The Ngc Key.</param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode SetNgcKey(DsServer dc, string dn, string ngcKey)
        {
            string kcl;
            SHA256 sha256Hash = SHA256.Create();

            byte[] binNgcKey = Encoding.Unicode.GetBytes(ngcKey);

            DateTime now = DateTime.Now;
            DateTime dsTime = new DateTime(1601, 1, 1, 0, 0, 0);
            TimeSpan diff = now - dsTime;
            byte[] binNow = BitConverter.GetBytes((long)(diff.TotalSeconds * 1e7)); // 100 times nanosecond

            byte[] version = BitConverter.GetBytes(0x00000200);

            // KeyID
            byte[] KeyID_len = BitConverter.GetBytes((short)32);
            byte[] KeyID_id = new byte[] { (byte)1 };
            byte[] KeyID_val = sha256Hash.ComputeHash(Encoding.Unicode.GetBytes(ngcKey));

            // Key Hash
            byte[] KeyHash_len = BitConverter.GetBytes((short)32);
            byte[] KeyHash_id = new byte[] { (byte)2 };
            byte[] KeyHash_val; // Update later until all flieds finished

            // Key Material
            byte[] KeyMaterial_len = BitConverter.GetBytes((short)binNgcKey.Length);
            byte[] KeyMaterial_id = new byte[] { (byte)3 };
            byte[] KeyMaterial_val = binNgcKey;

            // Key Usage
            byte[] KeyUsage_len = BitConverter.GetBytes((short)1);
            byte[] KeyUsage_id = new byte[] { (byte)4 };
            byte[] KeyUsage_val = new byte[] { 1 };

            // Key Source
            byte[] KeySource_len = BitConverter.GetBytes((short)1);
            byte[] KeySource_id = new byte[] { (byte)5 };
            byte[] KeySource_val = new byte[] { 0 };

            // Key Approximate Last Logon Time Stamp
            byte[] KeyALLTS_len = BitConverter.GetBytes((short)8);
            byte[] KeyALLTS_id = new byte[] { (byte)8 };
            byte[] KeyALLTS_val = binNow;

            // Key Creation Time
            byte[] KeyCreationTime_len = BitConverter.GetBytes((short)8);
            byte[] KeyCreationTime_id = new byte[] { (byte)9 };
            byte[] KeyCreationTime_val = binNow;

            // hash the content
            IEnumerable<byte> content = KeyMaterial_len.Concat(KeyMaterial_id).Concat(KeyMaterial_val)
                                        .Concat(KeyUsage_len).Concat(KeyUsage_id).Concat(KeyUsage_val)
                                        .Concat(KeySource_len).Concat(KeySource_id).Concat(KeySource_val)
                                        .Concat(KeyALLTS_len).Concat(KeyALLTS_id).Concat(KeyALLTS_val)
                                        .Concat(KeyCreationTime_len).Concat(KeyCreationTime_id).Concat(KeyCreationTime_val);
            KeyHash_val = sha256Hash.ComputeHash(content.ToArray());

            IEnumerable<byte> result = version
                                        .Concat(KeyID_len).Concat(KeyID_id).Concat(KeyID_val)
                                        .Concat(KeyHash_len).Concat(KeyHash_id).Concat(KeyHash_val)
                                        .Concat(content);

            // compose the string value representing the DNBinary
            byte[] binVal = result.ToArray();
            StringBuilder hex = new StringBuilder(binVal.Length * 2);
            foreach (byte b in binVal)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            string dnBinary = hex.ToString();

            kcl = string.Format("B:{0}:{1}:{2}", dnBinary.Length, dnBinary, ngcKey);

            return ModifyAttribute(dc, dn, new DirectoryAttribute("msDS-KeyCredentialLink", kcl));
        }

        #endregion

        #region Search Ops
        /// <summary>
        /// Search for an object in the directory, returns it's attributes as requested.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="baseDn">The base DN of the search request.</param>
        /// <param name="ldapFilter">The LDAP filter string to filter the search result.</param>
        /// <param name="searchScope">The search scope to limit the search result scope.</param>
        /// <param name="attributesToReturn">
        /// An array of strings representing the attribute names that will be returned
        /// from the search request.
        /// </param>
        /// <param name="attributes">
        /// When return, contains the collection of attributes of the base DN,
        /// based on the provided search options.
        /// </param>
        /// <returns>Result code of the LDAP response.</returns>
        public ResultCode Search(
                DsServer dc,
                string baseDn,
                string ldapFilter,
                System.DirectoryServices.Protocols.SearchScope searchScope,
                string[] attributesToReturn,
                out SearchResultEntryCollection results
                )
        {
            return LdapUtility.Search(dc, baseDn, ldapFilter, searchScope, attributesToReturn, out results);
        }

        public ResultCode ControlledSearch(
                DsServer dc,
                string baseDn,
                string ldapFilter,
                System.DirectoryServices.Protocols.SearchScope searchScope,
                string[] attributesToReturn,
                DirectoryControlCollection controls,
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
                foreach (DirectoryControl c in controls)
                {
                    request.Controls.Add(c);
                }
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

        /// <summary>
        /// Check if the object exists on the DC.
        /// </summary>
        /// <param name="dc">The DC.</param>
        /// <param name="objDN">DN of the object to be verified.</param>
        /// <returns>True if the object already exists, false otherwise.</returns>
        public bool IsObjectExist(DsServer dc, string objDN)
        {
            return LdapUtility.IsObjectExist(dc, objDN);
        }

        public bool IsObjectExistInDeletedObjectsContainer(DsServer dc, string objDN)
        {
            SearchResultEntryCollection results;

            DirectoryControlCollection cc = new DirectoryControlCollection();
            cc.Add(new ShowDeletedControl());

            ResultCode r = ControlledSearch(
                dc,
                objDN,
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                null,
                cc,
                out results);
            if (r == ResultCode.NoSuchObject)
                return false;

            return true;
        }
        #endregion

        #region Access Control Ops
        /// <summary>
        /// Grant access permission to a user on a specific object.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="user">The user that will be grant access to.</param>
        /// <param name="dn">The DN of the object to revoke control access on.</param>
        /// <param name="accessRight">The name of the control access right to be revoked.</param>
        /// <param name="controlType">Type of access.</param>
        /// <returns>True if access is granted, False otherwise.</returns>
        public bool GrantAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType
                )
        {


            using (DirectoryEntry entry = LdapUtility.CreateDirectoryEntry(
                dc,
                "LDAP://" + dn,
                AuthenticationTypes.Secure     // Enforce Kerberos
                ))
            {

                ActiveDirectorySecurity sd = entry.ObjectSecurity;
                NTAccount accountName = new NTAccount(user.Domain.Name, user.Username);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule
                    = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);
                sd.AddAccessRule(myRule);
                entry.ObjectSecurity.AddAccessRule(myRule);
                entry.CommitChanges();
                entry.RefreshCache();
                entry.Close();
                return true;
            }
        }
        /// <summary>
        /// Revoke access permission of a user from a specific object.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="user">The user that will be grant access to.</param>
        /// <param name="dn">The DN of the object to revoke control access on.</param>
        /// <param name="accessRight">The name of the control access right to be revoked.</param>
        /// <param name="controlType">Type of access.</param>
        /// <returns>True if access is revoked, False otherwise.</returns>
        public bool RevokeAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType
                )
        {

            using (DirectoryEntry entry = LdapUtility.CreateDirectoryEntry(
                 dc,
                 "LDAP://" + dn,
                 AuthenticationTypes.Secure     // Enforce Kerberos
                 ))
            {


                ActiveDirectorySecurity sd = entry.ObjectSecurity;
                NTAccount accountName = new NTAccount(user.Domain.Name, user.Username);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule
                    = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);
                sd.RemoveAccessRule(myRule);
                entry.ObjectSecurity.RemoveAccessRule(myRule);
                entry.CommitChanges();
                entry.RefreshCache();
                entry.Close();
                return true;
            }
        }

        /// <summary>
        /// Grant control access to a user on a specific object.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="user">The user that will be grant access to.</param>
        /// <param name="dn">The DN of the object to revoke control access on.</param>
        /// <param name="accessRight">The name of the control access right to be revoked.</param>
        /// <param name="controlType">Type of access.</param>
        /// <param name="controlAccessRightGuid">Guid of the control access right.</param>
        /// <returns>True if access is granted, False otherwise.</returns>
        public bool GrantControlAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType,
                Guid controlAccessRightGuid
        )
        {

            using (DirectoryEntry entry = LdapUtility.CreateDirectoryEntry(
                 dc,
                 "LDAP://" + dn,
                 AuthenticationTypes.Secure,     // Enforce Kerberos
                 true
                 ))
            {

                ActiveDirectorySecurity sd = entry.ObjectSecurity;
                NTAccount accountName = new NTAccount(user.Domain.DNSName, user.Username);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(
                        new SecurityIdentifier(acctSID.Value),
                        accessRight,
                        controlType,
                        controlAccessRightGuid
                );
                sd.AddAccessRule(myRule);
                entry.ObjectSecurity.AddAccessRule(myRule);
                entry.CommitChanges();
                entry.RefreshCache();
                entry.Close();
                return true;
            }
        }

        /// <summary>
        /// Revoke access permission of a user from a specific object.
        /// </summary>
        /// <param name="dc.LdapConn">The LDAP dc.LdapConnection object to the directory server.</param>
        /// <param name="domainName">The domain name in which the requested object is located.</param>
        /// <param name="userName">The user whose control access will be revoked.</param>
        /// <param name="userPassword">Password of the user.</param>
        /// <param name="dn">The DN of the object to revoke control access on.</param>
        /// <param name="accessRight">The name of the control access right to be revoked.</param>
        /// <param name="controlType">Type of access.</param>
        /// <param name="controlAccessRightGuid">Guid of the control access right.</param>
        /// <returns>True if access is revoked, False otherwise.</returns>
        public bool RevokeControlAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType,
                Guid controlAccessRightGuid
        )
        {

            using (DirectoryEntry entry = LdapUtility.CreateDirectoryEntry(
                 dc,
                 "LDAP://" + dn,
                 AuthenticationTypes.Secure     // Enforce Kerberos
                 ))
            {

                ActiveDirectorySecurity sd = entry.ObjectSecurity;
                NTAccount accountName = new NTAccount(user.Domain.Name, user.Username);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(
                        new SecurityIdentifier(acctSID.Value),
                        accessRight,
                        controlType,
                        controlAccessRightGuid
                );
                sd.RemoveAccessRule(myRule);
                entry.ObjectSecurity.RemoveAccessRule(myRule);
                entry.CommitChanges();
                entry.RefreshCache();
                entry.Close();
                return true;
            }
        }

        #endregion


        /// <summary>
        /// Get the DsDomain structure for a specific domain from a DC server.
        /// </summary>
        /// <param name="serverDnsName">The DNS host name of the DC in the domain.</param>
        /// <param name="user">The user with access to the domain info.</param>
        /// <returns>The domain information.</returns>
        public DsDomain GetDomainInfo(string serverDnsName, DsUser user)
        {
            // Test if the domainDnsName contains a ':' to split the hostname from the port.
            // If the ':' exists, it might be an AD LDS.
            DsDomain d = null;

            // We create an AddsServer just to keep the LDAP connection;
            DsServer srv = new AddsServer();
            LdapUtility.CreateConnection(serverDnsName, user, ref srv);

            // Get the rootDSE object
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);

            bool isDs = !serverDnsName.Contains(":");
            if (isDs)
            {
                d = new AddsDomain();
                // Domain NC
                ((AddsDomain)d).DomainNC
                    = LdapUtility.CreateDSNameForObject(srv, rootDse.defaultNamingContext);
            }
            else
            {
                d = new AdldsDomain();
                ((AdldsDomain)d).ConfigNC = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            }

            // Common fields
            d.Admin = user;
            d.Name = rootDse.defaultNamingContext;
            if (isDs)
            {
                d.DNSName = DrsrHelper.GetFQDNFromDN(rootDse.defaultNamingContext);
                d.NetbiosName = DrsrHelper.GetNetbiosNameFromDNSName(d.DNSName);
            }

            d.ConfigNC = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);    // Config NC
            d.SchemaNC = LdapUtility.CreateDSNameForObject(srv, rootDse.schemaNamingContext);   // Schema NC

            // Domain functinal level
            d.FunctionLevel = (DrsrDomainFunctionLevel)rootDse.domainFunctionality;

            // FSMO role owners
            GetFsmoRoleOwners(srv, ref d);
            return d;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.LastIndexOf(System.String)")]
        public DsServer GetDCInfo(string serverDnsName, DsUser user)
        {
            bool isDc = !serverDnsName.Contains(":");
            DsServer srv = null;
            if (isDc)
            {
                srv = new AddsServer();
            }
            else
            {
                srv = new AdldsServer();
                ((AdldsServer)srv).Port = LdapUtility.GetPort(serverDnsName);
            }

            // Connect this server using LDAP
            LdapConnection conn = LdapUtility.CreateConnection(serverDnsName, user, ref srv);

            // Get the rootDSE object
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);

            // Basic
            srv.Domain = GetDomainInfo(serverDnsName, user);
            srv.NetbiosName = GetAttributeValueInString(srv, rootDse.serverName, "name");
            srv.DnsHostName = rootDse.dnsHostName;

            // Server
            srv.ServerObjectName = rootDse.serverName;
            srv.ServerObjectGuid = LdapUtility.GetObjectGuid(srv, rootDse.serverName).Value;

            // NTDS Settings
            srv.NtdsDsaObjectName = rootDse.dsServiceName;
            srv.InvocationId = new Guid(LdapUtility.GetAttributeValueInBytes(srv, rootDse.dsServiceName, "invocationid"));
            srv.NtdsDsaObjectGuid = LdapUtility.GetObjectGuid(srv, rootDse.dsServiceName).Value;

            if (isDc)
                srv.DsaNetworkAddress = srv.NtdsDsaObjectGuid.ToString() + "._msdcs." + srv.Domain.DNSName;
            else
                srv.DsaNetworkAddress = srv.NtdsDsaObjectGuid.ToString();

            if (isDc)
            {
                // Computer
                srv.ComputerObjectName = GetAttributeValueInString(
                    srv,
                    "OU=Domain Controllers," + rootDse.defaultNamingContext,
                    "distinguishedName",
                    "(&(objectClass=computer)(dNSHostName=" + rootDse.dnsHostName + "))",
                    System.DirectoryServices.Protocols.SearchScope.Subtree);

                srv.ComputerObjectGuid = LdapUtility.GetObjectGuid(srv, srv.ComputerObjectName).Value;
            }
            else
            {
                ((AdldsServer)srv).InstanceID = new Guid(rootDse.configurationNamingContext.Substring(rootDse.configurationNamingContext.LastIndexOf("CN=") + "CN=".Length));
            }

            // Site
            srv.Site = new DsSite();
            srv.Site.DN = LdapUtility.FindObjectNameWithFilter(srv, rootDse.serverName, "(objectClass=site)");
            srv.Site.CN = srv.Site.DN.Split(new char[] { ',', '=' }, StringSplitOptions.RemoveEmptyEntries)[1];

            if (srv.Site.DN != null)
                srv.Site.Guid = LdapUtility.GetObjectGuid(srv, srv.Site.DN).Value;

            // FSMO roles
            srv.FsmoRoles = FSMORoles.None;
            if (isDc)
            {
                if (srv.Domain.FsmoRoleOwners[FSMORoles.PDC] == srv.NtdsDsaObjectName)
                    srv.FsmoRoles |= FSMORoles.PDC;
                if (srv.Domain.FsmoRoleOwners[FSMORoles.RidAllocation] == srv.NtdsDsaObjectName)
                    srv.FsmoRoles |= FSMORoles.RidAllocation;

                if (srv.Domain.FsmoRoleOwners[FSMORoles.Infrastructure] == srv.NtdsDsaObjectName)
                    srv.FsmoRoles |= FSMORoles.Infrastructure;
            }
            if (srv.Domain.FsmoRoleOwners[FSMORoles.Schema] == srv.NtdsDsaObjectName)
                srv.FsmoRoles |= FSMORoles.Schema;
            if (srv.Domain.FsmoRoleOwners[FSMORoles.DomainNaming] == srv.NtdsDsaObjectName)
                srv.FsmoRoles |= FSMORoles.DomainNaming;

            srv.DCFunctional = rootDse.domainControllerFunctionality;

            return srv;
        }

        public REPS_FROM[] GetRepsFrom(DsServer dc, NamingContext nc)
        {
            string baseDn = LdapUtility.GetDnFromNcType(dc, nc);

            return GetRepsFrom(dc, baseDn);
        }

        public REPS_FROM[] GetRepsFrom(DsServer dc, string nc)
        {
            string baseDn = nc;

            SearchResultEntryCollection results = null;
            ResultCode ret = Search(
                dc,
                baseDn,
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "repsFrom" },
                out results);

            if (ret != ResultCode.Success)
                return null;

            foreach (SearchResultEntry e in results)
            {
                // if there is no other DC, results will still have one entry with no repsFrom attribute
                if (!e.Attributes.Contains("repsFrom"))
                    continue;
                DirectoryAttribute attr = e.Attributes["repsFrom"];
                REPS_FROM[] rets = new REPS_FROM[attr.Count];
                for (int i = 0; i < attr.Count; ++i)
                {
                    byte[] data = (byte[])attr[i];
                    if (data == null)
                        continue;

                    rets[i] = TypeMarshal.ToStruct<REPS_FROM>(data);
                    rets[i].usnVec.usnHighObjUpdate >>= 32;
                    rets[i].usnVec.usnHighPropUpdate >>= 32;

                }

                return rets;
            }

            return null;
        }

        public REPS_TO[] GetRepsTo(DsServer dc, NamingContext nc)
        {
            string baseDn = LdapUtility.GetDnFromNcType(dc, nc);

            SearchResultEntryCollection results = null;
            ResultCode ret = Search(
                dc,
                baseDn,
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "repsTo" },
                out results);

            if (ret != ResultCode.Success)
                return null;

            foreach (SearchResultEntry e in results)
            {
                DirectoryAttribute attr = e.Attributes["repsTo"];
                REPS_TO[] rets = new REPS_TO[attr.Count];
                for (int i = 0; i < attr.Count; ++i)
                {
                    byte[] data = (byte[])attr[i];
                    if (data == null)
                        continue;

                    rets[i] = TypeMarshal.ToStruct<REPS_TO>(data);
                }

                return rets;
            }

            return null;
        }

        public UPTODATE_VECTOR_V1_EXT GetReplUTD(DsServer dc, NamingContext nc)
        {
            string baseDn = LdapUtility.GetDnFromNcType(dc, nc);
            byte[] data = GetAttributeValueInBytes(dc, baseDn, "replUpToDateVector");

            // replUpToDateVector contains a UPTODATE_VECTOR_V2_EXT structure.
            // We make a V1 structure instead.

            UPTODATE_VECTOR_V1_EXT utdv1 = new UPTODATE_VECTOR_V1_EXT();
            utdv1.dwVersion = UPTODATE_VECTOR_V1_EXT_dwVersion_Values.V1;
            if (data == null)
            {
                utdv1.cNumCursors = 0;
            }
            else
            {
                UPTODATE_VECTOR_V2_EXT utd = TypeMarshal.ToStruct<UPTODATE_VECTOR_V2_EXT>(data);
                utdv1.cNumCursors = utd.cNumCursors;
                utdv1.rgCursors = new UPTODATE_CURSOR_V1[utdv1.cNumCursors];
                for (int i = 0; i < utdv1.cNumCursors; ++i)
                {
                    // The only difference between V1 and V2 is the cursor's version:
                    // UPTODATE_CURSOR_V1 doesn't have the field "timeLastSyncSuccess",
                    // while UPTODATE_CURSOR_V2 has.
                    utdv1.rgCursors[i].uuidDsa = utd.rgCursors[i].uuidDsa;
                    utdv1.rgCursors[i].usnHighPropUpdate = utd.rgCursors[i].usnHighPropUpdate;
                }
            }

            return utdv1;
        }

        public bool GetFsmoRoleOwners(DsServer dc, ref DsDomain domain)
        {
            // We get the names of the owners by querying the "fSMORoleOwner" attribute
            // of each NC.
            const string fsmoAttr = "fSMORoleOwner";

            // AD LDS doesn't have domain NC (PDC)
            if (domain is AddsDomain)
            {
                string defaultNC = LdapUtility.ConvertUshortArrayToString(
                    ((AddsDomain)domain).DomainNC.StringName);
                string pdcOwner = GetAttributeValueInString(dc, defaultNC, fsmoAttr);
                domain.FsmoRoleOwners[FSMORoles.PDC] = pdcOwner;

                // RID pool manager master
                string ridNC = "CN=RID Manager$, CN=System, " + defaultNC;
                domain.FsmoRoleOwners[FSMORoles.RidAllocation] = GetAttributeValueInString(dc, ridNC, fsmoAttr);

                // Infrastructure master
                string infraNC = "CN=Infrastructure, " + defaultNC;
                domain.FsmoRoleOwners[FSMORoles.Infrastructure] = GetAttributeValueInString(dc, infraNC, fsmoAttr);
            }

            // Schema master
            string schemaNC = LdapUtility.ConvertUshortArrayToString(
                domain.SchemaNC.StringName);
            domain.FsmoRoleOwners[FSMORoles.Schema] = GetAttributeValueInString(dc, schemaNC, fsmoAttr);

            // Domain naming master
            string namingNC = "CN=Partitions, " + LdapUtility.ConvertUshortArrayToString(
                domain.ConfigNC.StringName);
            domain.FsmoRoleOwners[FSMORoles.DomainNaming] = GetAttributeValueInString(dc, namingNC, fsmoAttr);

            return true;
        }

        /// <summary>
        /// Get the objectGUID sequence
        /// </summary>
        /// <param name="NC">name of the specific NC</param>
        /// <param name="UTDFilter">Up-to-Date vector filter</param>
        /// <param name="startGUID">The start range of the GUID</param>
        /// <param name="count">Total numbers of GUIDs requested.</param>
        /// <param name="GuidSequence">When return, contains the GUID sequence requested.</param>
        /// <param name="MD5digest">When return, contains the MD5 digest of the GUID sequence.</param>
        /// <returns>True if success, false otherwise</returns>
        public Guid[] GetObjectGuidSequence(
            DsServer dc,
            NamingContext NC,
            UPTODATE_VECTOR_V1_EXT UTDFilter,
            Guid startGUID,
            int count)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string schemaNc = rootDse.schemaNamingContext;
            uint whenCreatedAttrTyp = 0;


            const int pageSize = 20;
            const string ldapFilter = "(objectClass=*)";

            SearchRequest searchRequest = new SearchRequest(
                LdapUtility.GetDnFromNcType(dc, NC),
                ldapFilter,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                null);
            PageResultRequestControl pageRequest = new PageResultRequestControl(pageSize);

            searchRequest.Controls.Add(pageRequest);

            SearchOptionsControl searchOptions = new SearchOptionsControl(
                System.DirectoryServices.Protocols.SearchOption.DomainScope);

            searchRequest.Controls.Add(searchOptions);
            searchRequest.Controls.Add(new ShowDeletedControl());

            int pageCount = 0;
            List<Guid> o = new List<Guid>();
            bool firstObj = true;

            Guid gMin = startGUID;
            Guid gMax = new Guid(DrsrHelper.MaxGuid);

            while (true)
            {
                pageCount++;

                SearchResponse searchResponse
                    = (SearchResponse)dc.LdapConn.SendRequest(searchRequest);

                if (searchResponse.Controls.Length != 1 ||
                    !(searchResponse.Controls[0] is PageResultResponseControl))
                {
                    return null;
                }

                PageResultResponseControl pageResponse
                    = (PageResultResponseControl)searchResponse.Controls[0];

                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    // do something.
                    if (firstObj)
                    {
                        // For performance concern, we first find the "whenCreated"'s
                        // corresponding attrTyp by querying one arbitrary object once.
                        byte[] metaDataBer = GetAttributeValueInBytes(
                            dc,
                            entry.DistinguishedName,
                            "replPropertyMetaData"
                            );

                        if (metaDataBer == null)
                        {
                            site.Log.Add(LogEntryKind.CheckFailed, "object " + entry.DistinguishedName + " does not have replPropertyMetaData attribute in LDAP query");
                            break;
                        }

                        firstObj = false;
                        PROPERTY_META_DATA_VECTOR metaData
                            = TypeMarshal.ToStruct<PROPERTY_META_DATA_VECTOR>(metaDataBer);

                        SCHEMA_PREFIX_TABLE table = OIDUtility.CreatePrefixTable();

                        for (ulong i = 0; i < metaData.V1.cNumProps; ++i)
                        {
                            PROPERTY_META_DATA d = metaData.V1.rgMetaData[i];
                            string oid = OIDUtility.OidFromAttrid(table, d.attrType);

                            if (oid == null) continue;

                            string name = GetAttributeValueInString(
                                dc,
                                schemaNc,
                                "lDAPDisplayName",
                                "(attributeID=" + oid + ")",
                                System.DirectoryServices.Protocols.SearchScope.OneLevel
                                );

                            if (name == "whenCreated")
                            {
                                whenCreatedAttrTyp = d.attrType;
                                Console.WriteLine("ATTRTYP for whenCreated found: {0}", whenCreatedAttrTyp);
                                break;
                            }
                        }
                    }

                    if (LdapUtility.StampLessThanOrEqualUTD(
                        LdapUtility.AttrStamp(dc, entry.DistinguishedName, whenCreatedAttrTyp), UTDFilter)
                        )
                    {
                        Guid guid = new Guid((byte[])entry.Attributes["objectGuid"][0]);
                        if (LdapUtility.CompareGuid(guid, startGUID) > 0)
                        {
                            if (o.Count < count)
                            {
                                LdapUtility.InsertGuidToList(o, guid);
                            }
                            else
                            {
                                if (LdapUtility.CompareGuid(guid, gMin) <= 0
                                    || LdapUtility.CompareGuid(guid, gMax) >= 0)
                                    continue;

                                LdapUtility.InsertGuidToList(o, guid);
                                o = o.GetRange(0, count);
                            }
                            gMax = o[o.Count - 1];
                        }

                    }
                }

                if (pageResponse.Cookie.Length == 0)
                    break;

                pageRequest.Cookie = pageResponse.Cookie;
            }

            return o.ToArray();
        }

        public string[] GetServicePrincipalName(DsServer dc)
        {
            site.Log.Add(LogEntryKind.Checkpoint, "search all SPNs of machine: {0}", dc.ComputerObjectName);
            return LdapUtility.GetAttributeValuesString(dc, dc.ComputerObjectName, "servicePrincipalName");
        }

        public string[] GetServicePrincipalName(DsServer dc, string dn)
        {
            site.Log.Add(LogEntryKind.Checkpoint, "search all SPNs of machine: {0}", dn);
            return LdapUtility.GetAttributeValuesString(dc, dn, "servicePrincipalName");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToUInt64(System.String)")]
        public ulong GetRidAllocationPoolFromDSA(DsServer dc, string dn)
        {
            string serverReference = GetAttributeValueInString(dc, dn, "serverReference");
            string ridSetReference = GetAttributeValueInString(dc, serverReference, "ridSetReferences");

            if (ridSetReference != null)
            {
                string allocPool = GetAttributeValueInString(dc, ridSetReference, "rIDAllocationPool");
                return Convert.ToUInt64(allocPool);
            }
            return 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToUInt64(System.String)")]
        public ulong GetRidAllocationPoolFromRIDManager(DsServer dc, string dn)
        {
            string allocPool = GetAttributeValueInString(dc, dn, "rIDAvailablePool");
            return Convert.ToUInt64(allocPool);

        }

        /// <summary>
        /// Get the DSNAME of an object.
        /// </summary>
        /// <param name="dc">The DC to query the DSNAME from.</param>
        /// <param name="dn">The DN of the object.</param>
        /// <returns>DSNAME of the object.</returns>
        public DSNAME? GetDsName(DsServer dc, string dn)
        {
            if (dn != null)
            {
                return LdapUtility.CreateDSNameForObject(dc, dn);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the attributes of an object which can be replicated.
        /// </summary>
        /// <param name="dc">The DC to query the attributes from.</param>
        /// <param name="dn">The DN of the object.</param>
        /// <returns>Array of LDAP display names of the attributes.</returns>
        public string[] GetObjectReplicatedAttributes(DsServer dc, string dn)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            // first, figure out the attributes of the object
            SearchResultEntryCollection results;
            DirectoryControlCollection cc = new DirectoryControlCollection();
            cc.Add(new SecurityDescriptorFlagControl());
            ResultCode c = ControlledSearch(
                dc,
                dn,
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                null,  // setting this to null returns all attributes associated.
                cc,
                out results);

            List<string> replAttrs = new List<string>();
            if (c == ResultCode.Success)
            {
                foreach (SearchResultEntry e in results)
                {
                    foreach (DirectoryAttribute attr in e.Attributes.Values)
                    {
                        // Next, find the schema of every attribute and see if it's
                        // systemFlags attribute contains ADS_SYSTEMFLAG_ATTR_NOT_REPLICATED = 0x1,
                        // if so, the attribute is not replicated.
                        SearchResultEntryCollection replResult;

                        string ldapFilter = "(&"
                               + "(lDAPDisplayName=" + attr.Name + ")"
                               + "(objectClass=attributeSchema)"
                               + "(systemFlags:1.2.840.113556.1.4.804:=1))";
                        ResultCode r = Search(
                            dc,
                            rootDse.schemaNamingContext,
                            ldapFilter,
                            System.DirectoryServices.Protocols.SearchScope.OneLevel,
                            null,
                            out replResult);

                        if (r == ResultCode.Success && replResult.Count == 0)
                        {
                            replAttrs.Add(attr.Name);
                        }
                    }
                }
            }

            return replAttrs.ToArray();
        }

        /// <summary>
        /// Add a site object to the config NC.
        /// Test against config NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added site object.</returns>
        public string TestAddSiteObj(DsServer dc)
        {
            const string siteContainer = "CN=Sites";
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string baseDn = siteContainer + "," + rootDse.configurationNamingContext;

            int suffix = 0;
            string newSite = LdapUtility.GetAvailableSuffix(dc, baseDn, "Site", out suffix);

            string objDn = "CN=" + newSite + "," + baseDn;
            // Perform add
            ResultCode r = AddObject(dc, objDn, "site");
            if (r == ResultCode.Success)
                return objDn;

            return null;
        }

        /// <summary>
        /// Add a schema object to the schema NC.
        /// Test against schema NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added schema object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
        public string TestAddSchemaObj(DsServer dc)
        {
            // Schema needs some tricks.
            const string attributeSyntax = "2.5.5.3";      // Case-sensitive string
            const string oMSyntax = "27";                  // General string
            const string isSingleValued = "TRUE";

            int suffix = 0;
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            // names
            string cn = LdapUtility.GetAvailableSuffix(dc, rootDse.schemaNamingContext, "PET-Schema", out suffix);
            string objDn = "CN=" + cn + "," + rootDse.schemaNamingContext;

            string lDAPDisplayName = "pET-Schema" + suffix.ToString();

            // The OID of an attribute should be unique.
            // We use Microsoft prefix: 1.3.6.1.4.1.311, combined with sub-cat of 100.1 plus
            // the value in TestObjCounter, so if TestObjCounter=1, the OID of the new
            // schema would be: 1.3.5.1.4.1.311.100.1.1
            string OID = "1.3.6.1.4.1.311" + ".100.1." + suffix.ToString();

            // Perform add
            DirectoryAttributeCollection attrs = new DirectoryAttributeCollection();
            attrs.Add(new DirectoryAttribute("objectClass", "attributeSchema"));
            attrs.Add(new DirectoryAttribute("cn", cn));
            attrs.Add(new DirectoryAttribute("lDAPDisplayName", lDAPDisplayName));
            attrs.Add(new DirectoryAttribute("oMSyntax", oMSyntax));
            attrs.Add(new DirectoryAttribute("isSingleValued", isSingleValued));
            attrs.Add(new DirectoryAttribute("attributeSyntax", attributeSyntax));
            attrs.Add(new DirectoryAttribute("attributeID", OID));

            ResultCode r = AddObjectWithAttributes(dc, objDn, attrs);
            if (r == ResultCode.Success)
                return objDn;

            return null;
        }

        /// <summary>
        /// Add a user object to the default NC.
        /// Test against default NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added user object.</returns>
        public string TestAddUserObj(DsServer dc)
        {
            site.Log.Add(LogEntryKind.Checkpoint, "Start create temp user");
            const string userContainer = "CN=Users";
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            string baseDn = userContainer + "," + rootDse.defaultNamingContext;
            ResultCode r = ResultCode.Unavailable;
            string userName = null;
            int suffix = 0;
            string objDn = null;
            // Find a user name to add
            for (int i = 0; i < 2; i++)
            {
                userName = LdapUtility.GetAvailableSuffixForUser(dc, baseDn, "User", out suffix);

                objDn = "CN=" + userName + "," + baseDn;
                // Perform add
                try
                {
                    DeleteObject(dc, objDn);
                }
                catch
                {
                }
                site.Log.Add(LogEntryKind.Checkpoint, "Creating temp user: " + objDn);
                try
                {
                    r = AddObject(dc, objDn, "user");
                    if (r == ResultCode.Success)
                        break;
                }
                catch
                {
                }
            }
            if (r == ResultCode.Success)
            {
                site.Log.Add(LogEntryKind.Checkpoint, "Created temp user: " + objDn);
                DirectoryAttribute displayNameAttr = new DirectoryAttribute("displayName", userName);
                DirectoryAttribute sAMAccountNameAttr = new DirectoryAttribute("sAMAccountName", userName);
                DirectoryAttributeCollection attrs = new DirectoryAttributeCollection();
                attrs.Add(displayNameAttr);
                attrs.Add(sAMAccountNameAttr);
                ResultCode r2 = AddAttributes(dc, objDn, attrs);
                if (r2 != ResultCode.Success)
                    return null;
                return objDn;
            }
            site.Log.Add(LogEntryKind.Checkpoint, "Failed to create temp user: " + objDn);
            return null;
        }

        /// <summary>
        /// Add a computer object to the default NC.
        /// Test against default NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added computer object.</returns>
        public string TestAddComputerObj(DsServer dc)
        {
            site.Log.Add(LogEntryKind.Checkpoint, "Start create temp computer");
            const string userContainer = "CN=Computers";
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            string baseDn = userContainer + "," + rootDse.defaultNamingContext;
            ResultCode r = ResultCode.Unavailable;
            string computerName = null;
            int suffix = 0;
            string objDn = null;
            // Find a computer name to add
            for (int i = 0; i < 2; i++)
            {
                computerName = LdapUtility.GetAvailableSuffix(dc, baseDn, "Computer", out suffix);

                objDn = "CN=" + computerName + "," + baseDn;
                // Perform add
                try
                {
                    DeleteObject(dc, objDn);
                }
                catch
                {
                }
                site.Log.Add(LogEntryKind.Checkpoint, "Creating temp computer: " + objDn);
                try
                {
                    r = AddObject(dc, objDn, "computer");
                    if (r == ResultCode.Success)
                        break;
                }
                catch
                {
                }
            }
            if (r == ResultCode.Success)
            {
                site.Log.Add(LogEntryKind.Checkpoint, "Created temp computer: " + objDn);
                DirectoryAttribute displayNameAttr = new DirectoryAttribute("displayName", computerName);
                DirectoryAttribute sAMAccountNameAttr = new DirectoryAttribute("sAMAccountName", computerName);
                DirectoryAttributeCollection attrs = new DirectoryAttributeCollection();
                attrs.Add(displayNameAttr);
                attrs.Add(sAMAccountNameAttr);
                ResultCode r2 = AddAttributes(dc, objDn, attrs);
                if (r2 != ResultCode.Success)
                    return null;
                return objDn;
            }
            site.Log.Add(LogEntryKind.Checkpoint, "Failed to create temp computer: " + objDn);
            return null;
        }

        public string TestAddGroupObj(DsServer dc)
        {
            site.Log.Add(LogEntryKind.Checkpoint, "Start create temp group");
            const string userContainer = "CN=Users";
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            string baseDn = userContainer + "," + rootDse.defaultNamingContext;
            ResultCode r = ResultCode.Unavailable;
            string userName = null;
            int suffix = 0;
            string objDn = null;
            // Find a group name to add
            for (int i = 0; i < 2; i++)
            {
                userName = LdapUtility.GetAvailableSuffixForUser(dc, baseDn, "DrsrTempGroup", out suffix);

                objDn = "CN=" + userName + "," + baseDn;
                // Perform add
                try
                {
                    DeleteObject(dc, objDn);
                }
                catch
                {
                }
                site.Log.Add(LogEntryKind.Checkpoint, "Creating temp group: " + objDn);
                try
                {
                    r = AddObject(dc, objDn, "group");
                    if (r == ResultCode.Success)
                    {
                        site.Log.Add(LogEntryKind.Checkpoint, "Created temp group: " + objDn);
                        return objDn;
                    }
                }
                catch
                {
                }
            }
            site.Log.Add(LogEntryKind.Checkpoint, "Failed to create temp user: " + objDn);
            return null;
        }
        /// <summary>
        /// Add an app object to the app NC.
        /// Test against app NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added app object.</returns>
        public string TestAddAppObject(DsServer dc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Construct a new crossRef object in type of ENTINF.
        /// </summary>
        /// <param name="dc">The DC server holding a Domain Naming Master role.</param>
        /// <param name="dn">DN of the crossRef object.</param>
        /// <param name="cn">CN of the crossRef object.</param>
        /// <param name="ncName">ncName of the crossRef object.</param>
        /// <param name="dnsRoot">dnsRoot of the NCcrossRef object.<param>
        /// <returns>The ENTINF object.</returns>
        public ENTINF ConstructNewCrossRefObject(DsServer dc, string dn, string cn, string ncName, string dnsRoot)
        {

            ENTINF newENTINF = new ENTINF();
            newENTINF.pName = null;
            newENTINF.ulFlags = 0;


            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            DirectoryAttributeCollection attrCol = new DirectoryAttributeCollection();

            DirectoryAttribute attr0 = new DirectoryAttribute("cn", cn);
            attrCol.Add(attr0);
            Guid newguid = Guid.Empty;
            byte[] guidByte = newguid.ToByteArray();
            string guidStr = "";
            StringBuilder strBld = new StringBuilder();
            for (int i = 0; i < guidByte.Length; ++i)
            {
                strBld.AppendFormat(@"{0:x2}", guidByte[i]);
            }

            guidStr = strBld.ToString();

            ncName = "<GUID=" + guidStr + ">;" + ncName;

            DirectoryAttribute attr1 = new DirectoryAttribute("nCName", ncName);
            attrCol.Add(attr1);
            DirectoryAttribute attr2 = new DirectoryAttribute("dNSRoot", dnsRoot);
            attrCol.Add(attr2);
            DirectoryAttribute attr3 = new DirectoryAttribute("objectClass", "1.2.840.113556.1.3.11");//top; crossRef
            attrCol.Add(attr3);
            DirectoryAttribute attr4 = new DirectoryAttribute("systemFlags", "3");// FLAG_CR_NTDS_NC | FLAG_CR_NTDS_DOMAIN
            attrCol.Add(attr4);


            // Create a prefix table
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();
            List<ATTR> attrs = new List<ATTR>();
            foreach (DirectoryAttribute a in attrCol)
            {
                string attrName = a.Name;
                string attrOid = GetAttributeValueInString(
                    dc,
                    rootDse.schemaNamingContext,
                    "attributeID",
                    "(lDAPDisplayName=" + attrName + ")",
                    System.DirectoryServices.Protocols.SearchScope.OneLevel
                    );
                string attrSyx = GetAttributeValueInString(
                    dc,
                    rootDse.schemaNamingContext,
                    "attributeSyntax",
                    "(lDAPDisplayName=" + attrName + ")",
                    System.DirectoryServices.Protocols.SearchScope.OneLevel
                    );

                if (attrOid != null)
                {
                    uint attrTyp = OIDUtility.MakeAttid(prefixTable, attrOid);
                    List<ATTRVAL> attrVals = new List<ATTRVAL>();
                    for (int i = 0; i < a.Count; ++i)
                    {
                        object v = a[i];
                        if (v is byte[])
                        {
                            attrVals.Add(DrsuapiClient.CreateATTRVAL((byte[])v));
                        }
                        else if (v is string)
                        {
                            attrVals.Add(OIDUtility.ATTRVALFromValue(dc, (string)v, attrSyx, prefixTable));
                        }
                    }
                    ATTRVALBLOCK attrValBlock = DrsuapiClient.CreateATTRVALBLOCK(attrVals.ToArray());

                    ATTR attr = DrsuapiClient.CreateATTR(attrTyp, attrValBlock);
                    attrs.Add(attr);
                }
            }

            ATTRBLOCK attrBlock = new ATTRBLOCK();
            attrBlock.pAttr = attrs.ToArray();
            attrBlock.attrCount = attrs.ToArray() == null ? 0 : (uint)attrs.Count;

            newENTINF.AttrBlock = attrBlock;

            DSNAME objName = DrsuapiClient.CreateDsName(dn, Guid.Empty, null);
            newENTINF.pName = objName;
            newENTINF.ulFlags = 0;

            return newENTINF;
        }

        /// <summary>
        /// Construct an ENTINF structure with the specified attributes for an object
        /// </summary>
        /// <param name="dc">The DC which holds the object.</param>
        /// <param name="dn">The DN of the object.</param>
        /// <param name="attCollection">The attribute collection of the object.</param>
        /// <returns>A ENTINF structure contains the attributes of object.</returns>
        public ENTINF ConstructENTINF(DsServer dc, string dn, DirectoryAttributeCollection attCollection)
        {
            ENTINF newENTINF = new ENTINF();
            newENTINF.pName = null;
            newENTINF.ulFlags = 0;

            DSNAME objName;
            objName = DrsuapiClient.CreateDsName(dn, Guid.Empty, null);
            newENTINF.pName = objName;
            newENTINF.ulFlags = 0;
            newENTINF.pName = objName;

            #region Covert DirectoryAttributeCollection to ATTRBLOCK
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);

            // Create a prefix table
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();
            List<ATTR> attrs = new List<ATTR>();
            foreach (DirectoryAttribute a in attCollection)
            {
                string attrName = a.Name;
                string attrOid = GetAttributeValueInString(
                    dc,
                    rootDse.schemaNamingContext,
                    "attributeID",
                    "(lDAPDisplayName=" + attrName + ")",
                    System.DirectoryServices.Protocols.SearchScope.OneLevel
                    );
                string attrSyx = GetAttributeValueInString(
                    dc,
                    rootDse.schemaNamingContext,
                    "attributeSyntax",
                    "(lDAPDisplayName=" + attrName + ")",
                    System.DirectoryServices.Protocols.SearchScope.OneLevel
                    );

                if (attrOid != null)
                {
                    uint attrTyp = OIDUtility.MakeAttid(prefixTable, attrOid);
                    List<ATTRVAL> attrVals = new List<ATTRVAL>();
                    for (int i = 0; i < a.Count; ++i)
                    {
                        object v = a[i];
                        if (v is byte[])
                        {
                            attrVals.Add(DrsuapiClient.CreateATTRVAL((byte[])v));
                        }
                        else if (v is string)
                        {
                            attrVals.Add(OIDUtility.ATTRVALFromValue(dc, (string)v, attrSyx, prefixTable));
                        }
                    }
                    ATTRVALBLOCK attrValBlock = DrsuapiClient.CreateATTRVALBLOCK(attrVals.ToArray());

                    ATTR attr = DrsuapiClient.CreateATTR(attrTyp, attrValBlock);
                    attrs.Add(attr);
                }
            }

            ATTRBLOCK attrBlock = new ATTRBLOCK();
            attrBlock.pAttr = attrs.ToArray();
            attrBlock.attrCount = attrs.ToArray() == null ? 0 : (uint)attrs.Count;
            #endregion

            newENTINF.AttrBlock = attrBlock;

            return newENTINF;
        }


        /// <summary>
        /// Add an attribute value to the ENTINF instance.
        /// </summary>
        /// <param name="dc">DC to query schema info.</param>
        /// <param name="entInf">The ENTINF to be added attribute into.</param>
        /// <param name="att">The attribute to be added.</param>
        public void AddAttribute(DsServer dc, ref ENTINF entInf, DirectoryAttribute att)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();

            string attrName = att.Name;
            string attrOid = GetAttributeValueInString(
                dc,
                rootDse.schemaNamingContext,
                "attributeID",
                "(lDAPDisplayName=" + attrName + ")",
                System.DirectoryServices.Protocols.SearchScope.OneLevel
                );
            string attrSyx = GetAttributeValueInString(
                dc,
                rootDse.schemaNamingContext,
                "attributeSyntax",
                "(lDAPDisplayName=" + attrName + ")",
                System.DirectoryServices.Protocols.SearchScope.OneLevel
                );

            if (attrOid != null)
            {
                uint attrTyp = OIDUtility.MakeAttid(prefixTable, attrOid);
                List<ATTRVAL> attrVals = new List<ATTRVAL>();
                for (int i = 0; i < att.Count; ++i)
                {
                    object v = att[i];
                    if (v is byte[])
                    {
                        attrVals.Add(DrsuapiClient.CreateATTRVAL((byte[])v));
                    }
                    else if (v is string)
                    {
                        attrVals.Add(OIDUtility.ATTRVALFromValue(dc, (string)v, attrSyx, prefixTable));
                    }
                }
                ATTRVALBLOCK attrValBlock = DrsuapiClient.CreateATTRVALBLOCK(attrVals.ToArray());

                ATTR attr = DrsuapiClient.CreateATTR(attrTyp, attrValBlock);


                List<ATTR> attrList = new List<ATTR>();
                if (entInf.AttrBlock.pAttr != null)
                {
                    attrList.AddRange(entInf.AttrBlock.pAttr);
                }
                attrList.Add(attr);
                entInf.AttrBlock.pAttr = attrList.ToArray();
                entInf.AttrBlock.attrCount = (uint)attrList.Count;
            }

        }

        /// <summary>
        /// Get the ENTINF of an object.
        /// </summary>
        /// <param name="dc">The DC server holding a Domain Naming Master role.<</param>
        /// <param name="dn">The DN of the object.</param>
        /// <returns>The ENTINF object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        public ENTINF? GetENTINF(DsServer dc, string dn, bool forCrossMove = false)
        {
            ENTINF ei = new ENTINF();
            // We need DSNAME
            DSNAME dsName = LdapUtility.CreateDSNameForObject(dc, dn);
            if (dsName.structLen == 0)
                return null;
            ei.pName = dsName;

            ei.ulFlags = 0;

            // Get all attributes of the object
            SearchResultEntryCollection results = null;
            DirectoryControlCollection cc = new DirectoryControlCollection();
            cc.Add(new ExtendedDNControl());
            ResultCode r = ControlledSearch(
                dc,
                dn,
                "(objectClass=*)",
                System.DirectoryServices.Protocols.SearchScope.Base,
                null,
                cc,
                out results);

            if (results == null)
                return null;

            // Create a prefix table
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();

            // Root DSE
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            foreach (SearchResultEntry e in results)
            {
                List<ATTR> attrs = new List<ATTR>();
                foreach (DirectoryAttribute a in e.Attributes.Values)
                {
                    string attrName = a.Name;
                    if (forCrossMove)
                    {
                        //in cross domain case, SAM, SID related unqie attributes are not allowed
                        if (a.Name.ToLower() == "objectsid" || a.Name.ToLower() == "samaccounttype" || a.Name.ToLower() == "samaccountname")
                            continue;
                    }
                    string attrOid = GetAttributeValueInString(
                        dc,
                        rootDse.schemaNamingContext,
                        "attributeID",
                        "(lDAPDisplayName=" + attrName + ")",
                        System.DirectoryServices.Protocols.SearchScope.OneLevel
                        );
                    string attrSyx = GetAttributeValueInString(
                        dc,
                        rootDse.schemaNamingContext,
                        "attributeSyntax",
                        "(lDAPDisplayName=" + attrName + ")",
                        System.DirectoryServices.Protocols.SearchScope.OneLevel
                        );

                    if (attrOid != null)
                    {
                        uint attrTyp = OIDUtility.MakeAttid(prefixTable, attrOid);
                        List<ATTRVAL> attrVals = new List<ATTRVAL>();
                        if (a.Name.ToLower() == "objectclass" && forCrossMove)
                        {
                            //cross domain move case require 1st object class to be a meaningful one, so just reverse the order
                            for (int i = a.Count - 1; i >= 0; i--)
                            {
                                attrVals.Add(OIDUtility.ATTRVALFromValue(dc, (string)a[i], attrSyx, prefixTable));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < a.Count; ++i)
                            {
                                object v = a[i];
                                if (v is byte[])
                                {
                                    attrVals.Add(DrsuapiClient.CreateATTRVAL((byte[])v));
                                }
                                else if (v is string)
                                {
                                    attrVals.Add(OIDUtility.ATTRVALFromValue(dc, (string)v, attrSyx, prefixTable));
                                }
                            }
                        }
                        ATTRVALBLOCK attrValBlock = DrsuapiClient.CreateATTRVALBLOCK(attrVals.ToArray());

                        ATTR attr = DrsuapiClient.CreateATTR(attrTyp, attrValBlock);
                        attrs.Add(attr);
                    }
                }

                ATTRBLOCK attrBlock = new ATTRBLOCK();
                attrBlock.pAttr = attrs.ToArray();
                attrBlock.attrCount = attrs.ToArray() == null ? 0 : (uint)attrs.Count;

                ei.AttrBlock = attrBlock;
            }
            return ei;
        }

        /// <summary>
        /// Construct a client credential.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="user">The user as the client.</param>
        /// <returns>The security buffer descriptor.</returns>
        public DRS_SecBufferDesc ConstructClientCredential(DsServer dc, DsUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the parent object of a given object.
        /// </summary>
        /// <param name="child">The object who's parent is being queried.</param>
        /// <returns>DSName of the parent object.</returns>
        public string GetParentObjectDn(string childDn)
        {
            int l = childDn.IndexOf(',');

            // we're at the root, returning RootDse
            if (l == 0)
                return "";

            return childDn.Substring(l + 1);
        }

        /// <summary>
        /// List all DC names in the current domain.
        /// </summary>
        /// <param name="dc">A GC in the domain.</param>
        /// <param name="domainDn">The DN of the domain.</param>
        /// <returns>DC names in the domain.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        public string[] ListDCNamesInDomain(DsServer dc, string domainDn)
        {
            RootDSE rootDse = LdapUtility.GetRootDSE(dc);
            string ldapFilter = "(&(objectClass=nTDSDSA)(msDS-HasDomainNCs=" + domainDn + "))";

            SearchResultEntryCollection results = null;
            ResultCode re = Search(
                dc,
                "CN=Sites," + rootDse.configurationNamingContext,
                ldapFilter,
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "distinguishedName" },
                out results);

            if (re != ResultCode.Success || results == null)
                return null;

            List<string> dcs = new List<string>();
            foreach (SearchResultEntry e in results)
            {
                string ntdsdsa = e.DistinguishedName;
                string server = GetParentObjectDn(ntdsdsa);

                string t = server.Split(',')[0];
                string serverName = t.Substring(t.IndexOf("=") + 1);
                dcs.Add(serverName);
            }
            return dcs.ToArray();
        }

        public new ITestSite Site
        {
            get
            {
                return site;
            }
            set
            {
                site = value;
            }
        }
    }
}

