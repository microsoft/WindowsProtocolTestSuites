// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Microsoft.Protocols.TestTools;
using ProtocolMessageStructures;
using ActiveDs;


namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    /// <summary>
    /// This class contains theHelper methods to validate the requirements of AD Security.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public static class ADTSHelper
    {
        #region Variables

        //instance of LdapConnection.
        static LdapConnection connection;
        //Specifies an instance of AddResponse.
        static AddResponse addResponse;
        //Specifies an instance of ModifyResponse.
        static ModifyResponse modifyResponse;
        //Specifies an instance of DeleteResponse.
        static DeleteResponse deleteResponse;
        //Specifies an instance of SearchResponse.
        static SearchResponse searchResponse;
        //Specifies the result code
        static string resultCode;
        //This is used to get the client user name
        static string clientUserName = TestClassBase.BaseTestSite.Properties.Get("Common.ClientUserName");

        #endregion

        #region AddObject
        /// <summary>
        /// AddObject method adds an object to the Active Directory.
        /// </summary>
        /// <param name="distinguishedName">The distinguishedName of the new object in the directory.</param>
        /// <param name="attributes">This parameter contains the list of attributes and their values of an object to be added.</param>
        /// <param name="ldapConnection">This is the connection to Microsoft Active Directory Domain Services through which Add request will be sent.</param>
        /// <returns>Result code of the add response.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string AddObject(string distinguishedName,
                                       List<DirectoryAttribute> attributes,
                                       LdapConnection ldapConnection)
        {
            try
            {
                AddRequest addRequest = new AddRequest();
                addRequest.DistinguishedName = distinguishedName;
                foreach (DirectoryAttribute attr in attributes)
                {
                    addRequest.Attributes.Add(attr);
                }
                addResponse = (AddResponse)ldapConnection.SendRequest(addRequest);
                if (addResponse != null)
                {
                    return ResultCode.Success.ToString();
                }
            }
            catch (DirectoryOperationException ex)
            {
                resultCode = ex.Response.ResultCode.ToString();

            }
            return resultCode;
        }

        #endregion

        #region ModifyObject

        /// <summary>
        /// ModifyObject method modifies an existing object in the Active Directory.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name of the object to be modified.</param>
        /// <param name="attributesToModify">This parameter lists out the attributes and its values of an object to be modified.</param>
        /// <param name="ldapConnection">This is the connection to Microsoft Active Directory Domain Services through which modify request will be sent.</param>
        /// <returns>Result code of the modify response.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static string ModifyObject(string distinguishedName,
                                          List<DirectoryAttribute> attributesToModify,
                                          LdapConnection ldapConnection)
        {
            modifyResponse = null;
            try
            {
                ModifyRequest modifyRequest = new ModifyRequest();
                modifyRequest.DistinguishedName = distinguishedName;
                foreach (DirectoryAttributeModification attr in attributesToModify)
                {
                    modifyRequest.Modifications.Add(attr);
                }
                modifyResponse = (ModifyResponse)ldapConnection.SendRequest(modifyRequest);
                if (modifyResponse != null && modifyResponse.ResultCode == ResultCode.Success)
                {
                    return ResultCode.Success.ToString();
                }
            }
            catch (DirectoryOperationException ex)
            {
                resultCode = ex.Response.ResultCode.ToString();

            }

            catch (LdapException ex)
            {
                resultCode = ex.ErrorCode.ToString();

            }

            return resultCode;
        }

        #endregion

        #region DeleteObject

        /// <summary>
        /// DeleteObject method deletes an existing object from the Active Directory.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name of an object to be deleted.</param>
        /// <param name="ldapConnection">This is the connection to Microsoft Active Directory Domain Services through which delete request will be sent.</param>
        /// <returns>Result code of the delete response.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string DeleteObject(string distinguishedName,
                                          LdapConnection ldapConnection)
        {
            deleteResponse = null;
            try
            {
                DeleteRequest deleteRequest = new DeleteRequest();
                deleteRequest.DistinguishedName = distinguishedName;
                deleteResponse = (DeleteResponse)ldapConnection.SendRequest(deleteRequest);
                if (deleteResponse != null && deleteResponse.ResultCode == ResultCode.Success)
                {
                    return ResultCode.Success.ToString();
                }
            }
            catch (DirectoryOperationException ex)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);
                resultCode = ex.Response.ResultCode.ToString();
            }
            return resultCode;
        }


        #endregion

        #region SearchObject

        /// <summary>
        /// SearchObject method searches for an object in the Active Directory.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name of an object to be searched.</param>
        /// <param name="ldapSearchFilter">An LDAP search filter.</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed.</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation.</param>
        /// <param name="ldapConnection">This is the connection to Microsoft Active Directory Domain Services through which search request will be sent.</param>
        /// <returns>Result code of the search response.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static SearchResponse SearchObject(string distinguishedName,
                                                  string ldapSearchFilter,
                                                  System.DirectoryServices.Protocols.SearchScope searchScope,
                                                  string[] attributesToReturn,
                                                  LdapConnection ldapConnection)
        {

            SearchRequest searchRequest = new SearchRequest(distinguishedName, ldapSearchFilter, searchScope, attributesToReturn);

            try
            {
                searchResponse = (SearchResponse)ldapConnection.SendRequest((DirectoryRequest)searchRequest);

                if (searchResponse != null)
                {
                    return searchResponse;
                }
            }
            catch (DirectoryOperationException ex)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);
                resultCode = ex.Response.ResultCode.ToString();
            }

            return searchResponse;
        }

        #endregion

        #region SetAccessRights

        /// <summary>
        /// SetAccessRights method sets the particular access right for a particular user on a particular AD Container/Object
        /// </summary>
        /// <param name="dc">The computer name of the DC.</param>
        /// <param name="dn">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="userPassword">The password of the domain user</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="accessRight">The name of the access right to be set</param>
        /// <param name="controlType">Allow/Deny particular ActiveDirectoryRights</param>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool SetAccessRights(string dc,
                                           string dn,
                                           string domainUser,
                                           string userPassword,
                                           string domain,
                                           ActiveDirectoryRights accessRight,
                                           AccessControlType controlType)
        {
            DirectoryEntry entry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}",dc,dn), 
                domainUser, userPassword,
                AuthenticationTypes.Secure);
            ActiveDirectorySecurity sd = entry.ObjectSecurity;

            NTAccount accountName = new NTAccount(domain, domainUser);

            IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);

            sd.AddAccessRule(myRule);
            entry.ObjectSecurity.AddAccessRule(myRule);
            entry.CommitChanges();

            return true;
        }

        #endregion

        #region SetAccessRightsAllow

        /// <summary>
        /// SetAccessRights method sets the particular access right for a particular user on a particular AD Container/Object
        /// </summary>
        /// <param name="dn">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="whom">name of whom to set the object</param>
        /// <param name="userPassword">The password of the domain user</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="accessRight">The name of the access right to be set</param>
        /// <param name="controlType">Allow/Deny particular ActiveDirectoryRights</param>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool SetAccessRightsAllow( string dc,
                                           string dn,
                                           string domainUser,
                                           string userPassword,
                                           string whom,
                                           string domain,
                                           ActiveDirectoryRights accessRight,
                                           AccessControlType controlType)
        {
            DirectoryEntry entry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dc, dn),
                domainUser, userPassword,
                AuthenticationTypes.Secure); 
            ActiveDirectorySecurity sd = entry.ObjectSecurity;
            int retryCount = 0;
            NTAccount accountName;
            IdentityReference acctSID;
            retry:
            try
            {
                accountName = new NTAccount(domain, whom);
                acctSID = accountName.Translate(typeof(SecurityIdentifier));
            }
            catch (IdentityNotMappedException e)
            {
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    string.Format("Translate account name error: {0}", e.Message));
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    string.Format("Wait 1 minute and retry:{0}", retryCount));
                retryCount++;
                System.Threading.Thread.Sleep(60000);
                if (retryCount < 10) goto retry;
                else throw;
            }
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value),
                                                                             accessRight,
                                                                             controlType);

            sd.AddAccessRule(myRule);
            entry.ObjectSecurity.AddAccessRule(myRule);
            entry.CommitChanges();

            return true;
        }

        #endregion

        #region SetControlAccessRights
        /// <summary>
        /// SetControlAcessRights method sets the control access right to the object depending upon the right requested.
        /// </summary>
        /// <param name="dn">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="userPassword">The password of the domain user</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="controlAccessRightGuid">Guid of the control access right</param>
        /// <param name="right">Extended right</param>
        /// <param name="accessControl">allow or deny the right</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool SetControlAcessRights(string dc,
                                                 string dn,
                                                 string domainUser,
                                                 string userPassword,
                                                 string domain,
                                                 Guid controlAccessRightGuid,
                                                 ActiveDirectoryRights right,
                                                 AccessControlType accessControl)
        {
            DirectoryEntry user;
            try
            {
                user = new DirectoryEntry(
                   string.Format("LDAP://{0}/{1}", dc, dn),
                   clientUserName, userPassword,
                   AuthenticationTypes.Secure);
                string temp = user.Properties["name"].Value.ToString();
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, e.Message);
                return false;
            }
            ActiveDirectorySecurity userSecurity = user.ObjectSecurity;
            ActiveDirectoryAccessRule myRule = null;
            int retryCount = 0;
        SetControlAcessRightsRetry:
            try
            {
                NTAccount accountName = new NTAccount(domain, domainUser);
                IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
                myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value),
                                                                                 right,
                                                                                 accessControl,
                                                                                 controlAccessRightGuid);
            }
            catch (IdentityNotMappedException notMappedException)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Debug,
                    string.Format("Exception occurs when translate {0}\\{1} to SID. Retry {2}", domain, domainUser, retryCount));
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Debug,
                    string.Format("Exception :{0}", notMappedException.Message));
                System.Threading.Thread.Sleep(5000);
                if (retryCount++ < 10) goto SetControlAcessRightsRetry;
                else throw;
            }
            userSecurity.AddAccessRule(myRule);
            user.ObjectSecurity.AddAccessRule(myRule);
            user.CommitChanges();
            return true;
        }



        #endregion

        #region ProcessSearchRespone
        /// <summary>
        /// Method is used to process the search response received using ldap
        /// </summary>
        /// <param name="searchResponse">search response which was received from the server as part of ldap query</param>
        /// <param name="searchString">search string in order to process the response and select the object with this search string</param>
        /// <param name="attributeName">name of the attribute retrieved from the response</param>
        /// <param name="value">value of the above mentioned attribute</param>
        /// <returns>returns true if successfully process the search response else false</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public static bool ProcessSearchRespone(SearchResponse searchResponse,
                                                string searchString,
                                                string attributeName,
                                                out System.Object value)
        {
            value = null;
            foreach (SearchResultEntry searchResult in searchResponse.Entries)
            {
                bool isRequiredobject = false;
                string[] searchSubString = searchString.Split(new char[] { '.', ',' });
                isRequiredobject = searchResult.DistinguishedName.ToUpper().Contains(searchSubString[0].ToUpper());
                if (isRequiredobject)
                {
                    SearchResultAttributeCollection attributeCollection = searchResult.Attributes;
                    foreach (DirectoryAttribute attribute in attributeCollection.Values)
                    {
                        for (int i = 0; i < attribute.Count; i++)
                        {
                            if (attribute.Name.ToUpper() == attributeName.ToUpper())
                            {
                                string[] tempVal = (string[])attribute.GetValues(searchString.GetType());
                                value = tempVal[tempVal.Length - 1];
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
    }

        /// <summary>
        /// Checks whether the search response has the the specified value.
        /// </summary>
        /// <param name="searchResponse"></param>
        /// <param name="searchString"></param>
        /// <param name="attributeName"></param>
        /// <param name="expectedValue"></param>
        /// <param name="actualValue"></param>
        /// <returns></returns>
        public static bool SearchResponseHasValue(SearchResponse searchResponse,
                                                   string searchString,
                                                   string attributeName,
                                                   string expectedValue,
                                                   out string msg)
        {
            bool result = false;
            msg = "";
            foreach (SearchResultEntry searchResult in searchResponse.Entries)
            {
                string[] dn = searchResult.DistinguishedName.ToUpper().Split(',');
                if ((dn[0].Split('=')[1]).ToUpper() != searchString.ToUpper()) continue;
                if (!searchResult.Attributes.Contains(attributeName))
                {
                    msg = string.Format("Attribute {0} not found.", attributeName);
                    return false;
                }
                msg = "Actual values: ";
                foreach (string value in searchResult.Attributes[attributeName].GetValues(typeof(string)))
                {
                    msg += value + " ";
                    if (value == expectedValue) result = true;
                }
            }
            return result;
        }
                                    

        #endregion

        #region SecurityDescriptor
        /// <summary>
        /// This methods searches nTSecurityDescriptor.
        /// </summary>
        /// <param name="hostOrDomainName">specifies host name.</param>
        /// <param name="targetOu">specifies target group name</param>
        /// <param name="ldapSearchFilter">specifies ldapsearchfilter</param>
        /// <param name="sdSearchResult">specifies the result </param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool SecurityDescriptorControl(string hostOrDomainName,
                                                     string targetOu,
                                                     string ldapSearchFilter,
                                                     out string sdSearchResult)
        {
            try
            {


                LdapConnection connection = new LdapConnection(hostOrDomainName);


                SearchRequest searchRequest = new SearchRequest(targetOu,
                                                                ldapSearchFilter,
                                                                System.DirectoryServices.Protocols.SearchScope.Subtree,
                                                                null);
                SecurityDescriptorFlagControl searchFlag = new SecurityDescriptorFlagControl(System.DirectoryServices.Protocols.SecurityMasks.Owner);

                searchRequest.Controls.Add(searchFlag);

                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);


                foreach (SearchResultEntry entry in searchResponse.Entries)
                {


                    {
                        SearchResultAttributeCollection attributes = entry.Attributes;
                        foreach (DirectoryAttribute attribute in attributes.Values)
                        {


                            if (attribute.Count != 1)
                            {
                                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "Attributes count is not equal to one");
                            }
                            // used to track where we are in the loop when displaying  
                            // byte arrays stored in multi-valued attributes
                            // count the number of values associated with this attribute
                            for (int i = 0; i < attribute.Count; i++)
                            {
                                if (attribute[i] is string)
                                {
                                    //this is for formatting
                                    if (attribute.Count == 1)
                                    {
                                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "The value of attribute", attribute[i]);

                                    }
                                    else
                                    {
                                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "The value of attribute", attribute[i]);
                                    }
                                }
                                else if (attribute[i] is int)
                                {
                                    if (attribute.Count == 1)
                                    {
                                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "The value of attribute", attribute[i]);

                                    }
                                    else
                                    {
                                        TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "The value of attribute", attribute[i]);
                                    }
                                }
                                else if (attribute[i] is Byte[])
                                {

                                    byte[] x = attribute[i] as byte[];
                                    string s = string.Empty;
                                    foreach (byte b in x)
                                        s = s + Convert.ToString(b);
                                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, s.ToString());

                                }
                            }
                        }
                    }
                }

                sdSearchResult = "Success";
                return true;
            }
            catch (DirectoryOperationException s)
            {

                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, s.Response.ErrorMessage.ToString());
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, s.Response.ResultCode.ToString());
                sdSearchResult = s.Response.ResultCode.ToString();
                return false;
            }
        }
        #endregion

        #region ExtendedOperations

        /// <summary>
        /// This method is used to enable Fast Bind mechanism for Ldap connection
        /// </summary>
        /// <param name="requestName">name of the request</param>
        /// <param name="requestValue">value of the request in bytes</param>
        /// <param name="ldapConnection">specifies the ldap connection which needs to enable fast bind.</param>
        /// <returns>returns extended response</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static ExtendedResponse ExtendedOperations(string requestName,
                                                          byte[] requestValue,
                                                          LdapConnection ldapConnection)
        {
            ExtendedResponse extendedResponse = null;
            try
            {
                ExtendedRequest extendedRequest = new ExtendedRequest(requestName, requestValue);
                extendedResponse = (ExtendedResponse)ldapConnection.SendRequest(extendedRequest);
                return extendedResponse;
            }

            catch (DirectoryOperationException s)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, s.Response.ErrorMessage.ToString());
                return extendedResponse;
            }

        }
        #endregion

        #region RemoveAccessRights
        /// <summary>
        /// RemoveAccessRights method removes the particular access right for a particular user on a particular AD Container/Object
        /// </summary>
        /// <param name="dn">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="userPassword">The password of the domain user</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="accessRight">The name of the access right to be removed </param>
        /// <param name="controlType">Allow/Deny particular ActiveDirectoryRights</param>
        /// <returns> Nothing</returns>

        public static bool RemoveAccessRights(string dc,
                                              string dn,
                                              string domainUser,
                                              string userPassword,
                                              string domain,
                                              ActiveDirectoryRights accessRight,
                                              AccessControlType controlType)
        {
            DirectoryEntry entry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dc, dn),
                domainUser, userPassword,
                AuthenticationTypes.Secure); 
            ActiveDirectorySecurity sd = entry.ObjectSecurity;
            NTAccount accountName = new NTAccount(domain, clientUserName);

            IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);

            sd.RemoveAccessRule(myRule);
            entry.ObjectSecurity.RemoveAccessRule(myRule);
            entry.CommitChanges();

            return true;
        }
        #endregion

        #region RemoveControlAccessRights
        /// <summary>
        /// RemoveControlAccessRights method removes the control access right to the object depending upon the right requested.
        /// </summary>
        /// <param name="dn">The distinguished name of the Container/Object.</param>
        /// <param name="domainUser">The name of the user to whom the permissions to be set</param>
        /// <param name="userPassword">The password of the domain user</param>
        /// <param name="domain">The name of the domain to which user is belongs to </param>
        /// <param name="controlAccessRightGuid">Guid of the control access right</param>
        /// <param name="right">Extended right</param>
        /// <param name="accessControl">allow or deny the right</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool RemoveControlAcessRights(string dc,
                                                    string dn,
                                                    string domainUser,
                                                    string userPassword,
                                                    string domain,
                                                    Guid controlAccessRightGuid,
                                                    ActiveDirectoryRights right,
                                                    AccessControlType accessControl)
        {
            DirectoryEntry entry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dc, dn),
                domainUser, userPassword,
                AuthenticationTypes.Secure); 
            ActiveDirectorySecurity userSecurity = entry.ObjectSecurity;
            NTAccount accountName = new NTAccount(domain, domainUser);

            IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value),
                                                                                 right,
                                                                                 accessControl,
                                                                                 controlAccessRightGuid);

            userSecurity.RemoveAccessRule(myRule);
            entry.ObjectSecurity.RemoveAccessRule(myRule);
            entry.CommitChanges();

            return true;
        }

        #endregion

        #region unwillingToPerform

        /// <summary>
        /// This method enables fast bind on already established connection with other type of bind.
        /// </summary>
        /// <param name="connection">Specifies already established ldap connection to enable fast bind mechanism</param>
        /// <param name="requestName">name of the request</param>
        /// <param name="requestValue">value of the request in bytes</param>
        /// <returns>returns o if success else 53 </returns>
        public static uint EnableFastBindonEstablishedConnection(LdapConnection connection,
                                                                 string requestName,
                                                                 byte[] requestValue)
        {
            connection.AuthType = AuthType.Basic;

            // If unspecified the protocolVersion number, by default it will take 2. We have to validate LDAP V3. 
            // So the version has to set explicitly to 3
            connection.SessionOptions.ProtocolVersion = 3;
            connection.Timeout = new TimeSpan(0, 0, 120);
            //Basic Binding.
            connection.Bind();

            //Send LDAP_SERVER_FAST_BIND_OID_LDAP.
            ExtendedOperations(requestName, requestValue, connection);
            //FastBind using ExtendedOperations
            connection.Bind();

            return (uint)errorstatus.success;
        }

        #endregion

        #region passwordChange



        /// <summary>
        /// This method is use to change the password of the user forcefully
        /// </summary>
        /// <param name="connection">connection </param>
        /// <param name="userDN">Distinguish name of the user whose password to set</param>
        /// <param name="password">new password</param>
        /// <returns>returns true if password is changed successfully.</returns>
        public static bool SetPassword(LdapConnection connection,
                                       string userDN,
                                       string password)
        {

            try
            {
                connection = ConnectionBind(connection);


                DirectoryAttributeModification pwdMod = new DirectoryAttributeModification();
                pwdMod.Name = "unicodePwd";
                pwdMod.Add(GetPasswordData(password));
                pwdMod.Operation = DirectoryAttributeOperation.Replace;

                ModifyRequest request = new ModifyRequest(userDN, pwdMod);

                DirectoryResponse response = connection.SendRequest(request);

                return true;
            }
            catch (DirectoryException ex)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);
                return false;
            }

        }





        /// <summary>
        /// This method is use to change the password of the user forcefully
        /// </summary>
        /// <param name="connection">connection </param>
        /// <param name="userDN">Distinguish name of the user whose password to set</param>
        /// <param name="password">new password</param>
        /// <returns>returns true if password is changed successfully.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static bool ForchangePassword(LdapConnection connection,
                                             string userDN,
                                             string password)
        {
            try
            {

                DirectoryAttributeModification pwdMod = new DirectoryAttributeModification();
                pwdMod.Name = "unicodePwd";
                pwdMod.Add(GetPasswordData(password));
                pwdMod.Operation = DirectoryAttributeOperation.Replace;

                ModifyRequest request = new ModifyRequest(userDN, pwdMod);

                DirectoryResponse response = connection.SendRequest(request);

                return true;
            }
            catch (DirectoryException ex)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);
                return false;
            }

        }



        /// <summary>
        /// Changes the old password
        /// </summary>
        /// <param name="connection">specifies current ldap connection</param>
        /// <param name="userDN">specifies distinguish name </param>
        /// <param name="oldPassword">specifies old password</param>
        /// <param name="newPassword">specifies new password</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static bool PasswordChange(LdapConnection connection,
                                          string userDN,
                                          string oldPassword,
                                          string newPassword)
        {

            try
            {


                //Delete old password
                DirectoryAttributeModification deleteOldPwd = new DirectoryAttributeModification();
                deleteOldPwd.Name = "unicodePwd";
                deleteOldPwd.Add(GetPasswordData(oldPassword));


                //Add new password
                DirectoryAttributeModification addNewPwd = new DirectoryAttributeModification();
                addNewPwd.Name = "unicodePwd";
                addNewPwd.Add(GetPasswordData(newPassword));
                addNewPwd.Operation = DirectoryAttributeOperation.Replace;

                ModifyRequest request = new ModifyRequest(userDN,
                                                          deleteOldPwd,
                                                          addNewPwd);

                DirectoryResponse response = connection.SendRequest(request);

                return true;
            }
            catch (DirectoryException ex)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);
                return false;
            }

        }

        #region ConnectionBind
        /// <summary>
        /// This method binds connection for ForceChangePassword
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private static LdapConnection ConnectionBind(LdapConnection connection)
        {
            //Specifies Server name
            string adServer = TestClassBase.BaseTestSite.Properties["Common.WritableDC1.NetbiosName"] + "." + TestClassBase.BaseTestSite.Properties["Common.PrimaryDomain.DNSName"];
            //Specifies current user
            string currentUser = TestClassBase.BaseTestSite.Properties["Common.ClientUserName"];
            //Specifies user password
            string currentUserPassword = TestClassBase.BaseTestSite.Properties["Common.ClientUserPassword"];

            //Create new connection
            //used for Admin Force change password
            connection = new LdapConnection(new LdapDirectoryIdentifier(
                                    adServer + ":" + (int)Port.LDAP_SSL_PORT));

            //Credentials of current user
            connection.Credential = new NetworkCredential(currentUser, currentUserPassword);
            //AuthType As Negotiate
            connection.AuthType = AuthType.Negotiate;

            //Bind
            connection.Bind();
            return connection;
        }
        #endregion


        /// <summary>
        /// Changes the old password
        /// </summary>
        /// <param name="connection">specifies current ldap connection</param>
        /// <param name="userDN">specifies distinguish name </param>
        /// <param name="oldPassword">specifies old password</param>
        /// <param name="newPassword">specifies new password</param>
        /// <returns></returns>
        public static bool ChangePassword(LdapConnection connection,
                                          string userDN,
                                          string oldPassword,
                                          string newPassword)
        {
            try
            {
                connection = ConnectionBind(connection);

                //Delete old password
                DirectoryAttributeModification deleteOldPwd = new DirectoryAttributeModification();
                deleteOldPwd.Name = "unicodePwd";
                deleteOldPwd.Add(GetPasswordData(oldPassword));


                //Add new password
                DirectoryAttributeModification addNewPwd = new DirectoryAttributeModification();
                addNewPwd.Name = "unicodePwd";
                addNewPwd.Add(GetPasswordData(newPassword));
                addNewPwd.Operation = DirectoryAttributeOperation.Replace;

                ModifyRequest request = new ModifyRequest(userDN,
                                                          deleteOldPwd,
                                                          addNewPwd);

                DirectoryResponse response = connection.SendRequest(request);

                return true;
            }
            catch (DirectoryException ex)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, ex.Message);
                return false;
            }

        }

        /// <summary>
        /// This methods encrypts the password. 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static byte[] GetPasswordData(string password)
        {
            string formattedPassword;

            formattedPassword = String.Format("\"{0}\"", password);

            return (Encoding.Unicode.GetBytes(formattedPassword));
        }

        #endregion

        #region RemoveAllACE

        /// <summary>
        /// Removes all ACE rights for the ntSecurityDescriptor for specified user.
        /// </summary>
        /// <param name="distinguishedname">specifies dn</param>
        /// <param name="username">user name</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool RemoveAllACE(string dc,
                                        string distinguishedname,
                                        string username,
                                        string password)
        {
            DirectoryEntry dirEntry = new DirectoryEntry(
                 string.Format("LDAP://{0}/{1}", dc, distinguishedname),
                 username, password,
                 AuthenticationTypes.Secure);
            IADsSecurityDescriptor sd;
            //edit by Rina
            try
            {
                sd = (IADsSecurityDescriptor)dirEntry.Properties["nTSecurityDescriptor"].Value;
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                string errorMessage = e.Message;
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "RemoveAllACE : " + errorMessage);
                throw;
            }
            IADsAccessControlList dacl = (IADsAccessControlList)sd.DiscretionaryAcl;

            foreach (AccessControlEntry ace in dacl)
            {
                dacl.RemoveAce(ace);
            }

            sd.DiscretionaryAcl = dacl;
            dirEntry.Properties["nTSecurityDescriptor"].Value = sd;
            dirEntry.CommitChanges();


            return true;
        }

        #endregion

        #region NullSecurityDescriptor
        /// <summary>
        /// This method is used to set null values for ACE on nTSecurityDescriptor.
        /// </summary>
        /// <param name="distinguishedname">specifies dn</param>
        /// <param name="username">specifies user name</param>
        /// <param name="password">specifies password</param>
        /// <returns>true if success</returns>
        public static bool NullSecurityDescriptor(string dc,
                                                  string distinguishedname,
                                                  string username,
                                                  string password)
        {
            DirectoryEntry dirEntry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dc, distinguishedname),
                username, password,
                AuthenticationTypes.Secure);
            IADsSecurityDescriptor sd = (IADsSecurityDescriptor)dirEntry.Properties["nTSecurityDescriptor"].Value;
            IADsAccessControlList dacl = (IADsAccessControlList)sd.DiscretionaryAcl;

            sd.DiscretionaryAcl = null;
            dirEntry.Properties["nTSecurityDescriptor"].Value = sd;

            dirEntry.CommitChanges();
            return true;

        }

        #endregion

        #region NoDacl

        /// <summary>
        /// This method is used to set no values for ACE on nTSecurityDescriptor.
        /// </summary>
        /// <param name="distinguishedname">specifies distinguishedname</param>
        /// <param name="username">specifies user name</param>
        /// <param name="password">specifies password</param>
        /// <returns>true if success</returns>
        public static bool NoDacl(string dc,
                                  string distinguishedname,
                                  string username,
                                  string password)
        {
            DirectoryEntry dirEntry = new DirectoryEntry(
                string.Format("LDAP://{0}/{1}", dc, distinguishedname),
                username, password,
                AuthenticationTypes.Secure);
            IADsSecurityDescriptor sd = (IADsSecurityDescriptor)dirEntry.Properties["nTSecurityDescriptor"].Value;
            IADsAccessControlList dacl = (IADsAccessControlList)sd.DiscretionaryAcl;

            int DPbit = sd.Control;

            DPbit = (DPbit & (~4));

            if ((DPbit & 4) != 4)
            {
                sd.Control = DPbit;
            }

            dirEntry.Properties["nTSecurityDescriptor"].Value = sd;
            dirEntry.CommitChanges();
            return true;

        }

        #endregion

        #region Add Operation
        /// <summary>
        /// CreateActiveDirUser
        /// This action is used to create a new user in DC
        /// </summary>
        /// <param name="userName">Variable contains name of the user in DC</param>
        /// <param name="password">Variable contains password of the username</param>
        /// <returns>Returns GUID of the user</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Guid CreateActiveDirUser(string dc,
                                               name userName,
                                               string password,
                                               string domainDN)
        {
            Guid oGuid = Guid.Empty;
            string strPath = string.Empty;
            //gets the parsed string for distinguisdh name.
            try
            {

                if (userName == name.nameMapsMoreThanOneObject)
                {
                    // creating User in Domain NC
                    strPath = string.Format("LDAP://{0}/CN=Users,{1}", dc, domainDN);
                }
                else
                    // creating security principle in Configuration NC
                    strPath = string.Format("LDAP://{0}/CN=Configuration,{1}", dc, domainDN);

                DirectoryEntry entry = new DirectoryEntry(strPath);
                DirectoryEntry newUser = entry.Children.Add("CN=" + MS_ADTS_SecurityRequirementsValidator.TempUser0, "user");

                // Checking whether the temporary User (from config file) is already exists or not.
                if (!DirectoryEntry.Exists(string.Format("LDAP://{0}/CN={1},CN=Users,{2}", dc, MS_ADTS_SecurityRequirementsValidator.TempUser0, domainDN)))
                {
                    newUser.Properties["samAccountName"].Value = MS_ADTS_SecurityRequirementsValidator.TempUser0;
                    newUser.CommitChanges();
                    oGuid = newUser.Guid;

                    newUser.Invoke("SetPassword", new Object[] { password });
                    newUser.CommitChanges();
                    newUser.Close();
                }
            }

            catch (DirectoryServicesCOMException)
            {
                // creating security principle in Schema NC
                strPath = string.Format("LDAP://{0}/CN=Schema,CN=Configuration,{1}", dc, domainDN);
                try
                {
                    DirectoryEntry entry = new DirectoryEntry(strPath);
                    DirectoryEntry newUser = entry.Children.Add("CN=" + MS_ADTS_SecurityRequirementsValidator.TempUser0, "user");

                    // Checking whether User is already exists or not.
                    if (!DirectoryEntry.Exists(string.Format("LDAP://{0}/CN={1},CN=Users,{2}", dc, MS_ADTS_SecurityRequirementsValidator.TempUser0, domainDN)))
                    {
                        newUser.Properties["samAccountName"].Value = MS_ADTS_SecurityRequirementsValidator.TempUser0;
                        newUser.CommitChanges();
                        oGuid = newUser.Guid;

                        newUser.Invoke("SetPassword", new Object[] { password });
                        newUser.CommitChanges();
                        newUser.Close();
                    }
                }

                catch (DirectoryServicesCOMException e)
                {
                    // MS-ADTS-Security_R238
                    TestClassBase.BaseTestSite.CaptureRequirementIfAreNotEqual<int>(e.ErrorCode, 0, 238, @"AD/DS restricts 
                        security principals to the domain NC.");
                }
            }
            return oGuid;
        }
        /// <summary>
        /// Craete Active Directory User
        /// </summary>
        /// <param name="username">user name to create in AD</param>
        /// <param name="password">password of the user</param>
        /// <returns>returns true if successful</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool CreateActiveDirectoryUser(string dc,
                                                     string username,
                                                     string password,
                                                     string domainDN)
        {
            string strPath = string.Empty;
            strPath = string.Format("LDAP://{0}/CN=Users,{1}", dc, domainDN);
            DirectoryEntry entry = new DirectoryEntry(strPath);
            DirectoryEntry newUser = entry.Children.Add("CN=" + username, "user");

            // Checking whether the temporary User (from config file) is already exists or not.
            if (!DirectoryEntry.Exists(string.Format("LDAP://{0}/CN={1},CN=Users,{2}", dc, username, domainDN)))
            {
                newUser.Properties["samAccountName"].Value = username;
                newUser.CommitChanges();

                newUser.Invoke("SetPassword", new Object[] { password });
                newUser.CommitChanges();
                newUser.Close();
            }
            return true;
        }
        #endregion Add Operation

        #region Modify Operation
        /// <summary>
        /// ModifyOperation
        /// This action is used to perform LDAP modify operation 
        /// </summary>
        /// <param name="hostName">Variable contains DNS domain name</param>
        /// <param name="userName">Variable contains name of the user in DC</param>
        /// <param name="adType">Indicate if DS or LDS used</param>
        /// <param name="clientUserName">The user name to authenticate the operation</param>
        /// <param name="clientUserPassword">The password to the user.</param>
        /// <param name="domainDnsName">The domain DNS name.</param>
        /// <param name="serverVersion">The OS version of the server.</param>
        public static void ModifyOperation(string hostName,
                                           name userName,
                                           ADTypes adType,
                                           string clientUserName,
                                           string clientUserPassword,
                                           string domainDnsName,
                                           Common.ServerVersion serverVersion)
        {
            string distinguishedName = string.Empty;
            string attributeName = string.Empty;
            string newAttributeValue = string.Empty;
            string domainDN = ParseDomainName(domainDnsName);
            #region Establishing connection for ModifyOperation
            connection = new LdapConnection(hostName + ":" + (uint)Port.LDAP_PORT);
            connection.AuthType = AuthType.Basic;
            NetworkCredential credential = new NetworkCredential(clientUserName, clientUserPassword, domainDnsName);
            connection.Credential = credential;
            connection.Bind();
            #endregion Establishing connection for ModifyOperation

            distinguishedName = "CN=" + MS_ADTS_SecurityRequirementsValidator.TempUser0 + ",CN=Users," + domainDN;

            attributeName = "userPrincipalName";
            newAttributeValue = clientUserName + "@" + domainDnsName;
            ModifyRequest modReq = new ModifyRequest(distinguishedName,
                                             DirectoryAttributeOperation.Replace,
                                             attributeName,
                                             newAttributeValue);
            ModifyResponse modRes = (ModifyResponse)connection.SendRequest(modReq);

            if (userName == name.anonymousUser)
            {
                distinguishedName = "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," + domainDN;
                attributeName = "dSHeuristics";
                newAttributeValue = "0000002";
            }
            else if ((serverVersion > Common.ServerVersion.Win2003) && (ADTypes.AD_DS != adType))
            {
                distinguishedName = "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," + domainDN;
                attributeName = "dSHeuristics";
                newAttributeValue = "0000000000002";
            }

            modReq = new ModifyRequest(distinguishedName,
                                       DirectoryAttributeOperation.Replace,
                                       attributeName,
                                       newAttributeValue);
            modRes = (ModifyResponse)connection.SendRequest(modReq);

        }

        #endregion Modify Operation

        #region ParseDomainName
        /// <summary>
        /// This method returns the parsed string used in Distinguished name from Domain name.
        /// </summary>
        /// <param name="domainName">specifies domain name</param>
        /// <returns>parsed string of the domain name</returns>
        public static string ParseDomainName(string domainName)
        {
            string[] temp = domainName.Split('.');
            string parsedDomainNme = "";
            foreach (string item in temp)
            {
                parsedDomainNme += "DC=" + item + ",";
            }
            return parsedDomainNme.Substring(0, parsedDomainNme.Length - 1);
        }

        #endregion

        #region CreateSecurityPrincipleObject

        /// <summary>
        /// This method Creates Security Principal object
        /// </summary>
        /// <param name="hostname">specifies host name</param>
        /// <param name="portnum">specifies port number of ADAM</param>
        /// <param name="distinguishedname">specifies  distinguish name</param>
        /// <param name="username">specifies  user name</param>
        /// <param name="password">specifies  password.</param>
        /// <returns>on success return true</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool CreateSecurityPrincipleObject(string hostname,
                                                         int portnum,
                                                         string distinguishedname,
                                                         string username,
                                                         string password,
                                                         bool isSSLTLSEnabled = false)
        {
            DirectoryEntry objADAM;  // Directory object.

            string strPath = "LDAP://" + hostname + ":" + portnum + "/" + distinguishedname;

            if (isSSLTLSEnabled)
            {
                objADAM = new DirectoryEntry(strPath, username, password, AuthenticationTypes.SecureSocketsLayer);
            }
            else
            {
                objADAM = new DirectoryEntry(strPath, username, password);
            }
            objADAM.RefreshCache();

            DirectoryEntry newUser = objADAM.Children.Add("CN=" + MS_ADTS_SecurityRequirementsValidator.TempUser0, "user");

            // Checking whether User is already exists or not.
            try
            {
                newUser.CommitChanges();
            }
            catch
            {
                return false;
            }

            newUser.Close();

            objADAM.CommitChanges();

            //if success
            return true;

        }
        #endregion

        #region RootDSESearch

        /// <summary>
        /// perform search operation on Root DSE
        /// </summary>
        /// <param name="portNum">specifies port number</param>
        /// <param name="tls">specifies whether tls is enabled or not</param>
        /// <returns>return search response</returns>
        public static SearchResponse RootDSESearchOperation(uint portNum,
                                                            bool tls)
        {

            string hostName = TestClassBase.BaseTestSite.Properties.Get("Common.PrimaryDomain.DNSName");
            string ldapSearchFilter = "objectclass=*";
            string[] attributesToReturn = new string[10];

            if (((portNum == (uint)Port.LDAP_PORT) || (portNum == (uint)Port.LDAP_GC_PORT)) && tls)
            {
                attributesToReturn = new string[] { "supportedCapabilities" };
            }


            SearchRequest searchRequest = new SearchRequest(null,
                                                            ldapSearchFilter,
                                                            System.DirectoryServices.Protocols.SearchScope.Base,
                                                            attributesToReturn);

            SearchResponse searchResponse = (SearchResponse)new LdapConnection(hostName).SendRequest(searchRequest);

            return searchResponse;
        }

        #endregion

        #region TLSProtection

        /// <summary>
        /// TLSProtection
        /// This action is used to start transport layer security
        /// </summary>
        /// <param name="options">LdapSessionOptions object to configure session settings on the connection</param>
        /// <param name="connection">Variable defines the instance of LDAP connection</param>
        /// <returns>Returns Success if the method is successful       
        /// Returns unwillingToPerform if TLS is enabled over protected LDAP ports</returns>

        public static uint TLSProtection(LdapSessionOptions options,
                                         LdapConnection connection)
        {

            uint startTLS = 0;

            try
            {
                options = connection.SessionOptions;
                options.StartTransportLayerSecurity(null);
            }
            catch (TlsOperationException errMsg)
            {
                TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, errMsg.Message); 
                startTLS = (uint)errMsg.Response.ResultCode;
            }
            return startTLS;

        }
        #endregion TLSProtection
    }

    public static class GroupNames
    {
        public const string DomainAdmins = "Domain Admins";
        public const string DomainUsers = "Domain Users";
        public const string EnterpriseAdmins = "Enterprise Admins";
        public const string SchemaAdmins = "Schema Admins";
        public const string GroupPolicyCreatorOwners = "Group Policy Creator Owners";
    }
}