// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
// Interface definition of the LDAP adapter
// Like Zhu (likezh), 2012/6/18
//

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.AccessControl;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public interface ILdapAdapter : IAdapter
    {

        new ITestSite Site { get; set; }

        #region Base Ops
        /// <summary>
        /// Check if the LDAP service on a remote server is available.
        /// </summary>
        /// <param name="serverNetworkAddress">The network address of the remote server.</param>
        /// <returns>True if LDAP service on the remote server is available, false otherwise.</returns>
        bool IsReachable(string serverNetworkAddress);

        /// <summary>
        /// Add an object to the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to add.</param>
        /// <param name="objectClass">objectClass attribute of the object to add.</param>
        /// <returns>Result code of the LDAP response.</returns>
        ResultCode AddObject(DsServer dc, string dn, string objectClass);

        /// <summary>
        /// Add an object to the directory, with attributes.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to add.</param>
        /// <param name="attributes">Collection of attributes to be added to the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        ResultCode AddObjectWithAttributes(
                DsServer dc,
                string dn,
                DirectoryAttributeCollection attributes
                );

        /// <summary>
        /// Move an object to a new parent DN, and change the object name if necessary.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to move/rename.</param>
        /// <param name="newParentDn">The parent DN the object is to be located.</param>
        /// <param name="newObjectName">The new name of the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        ResultCode MoveRenameObject(
                DsServer dc,
                string dn,
                string newParentDn,
                string newObjectName
                );

        /// <summary>
        /// Delete an object to the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object to delete.</param>
        /// <returns>Result code of the LDAP response.</returns>
        ResultCode DeleteObject(DsServer dc, string dn);

        /// <summary>
        /// Add an attribute to an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to add.
        /// </param>
        /// <param name="attribute">The attribute to be added to the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        ResultCode AddAttribute(
                DsServer dc,
                string dn,
                DirectoryAttribute attribute
                );

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
        ResultCode AddAttributes(
                DsServer dc,
                string dn,
                DirectoryAttributeCollection attributes
                );

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
        ResultCode ModifyAttribute(
                DsServer dc,
                string dn,
                DirectoryAttribute attribute
                );

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
        ResultCode ModifyAttributes(
                DsServer dc,
                string dn,
                DirectoryAttributeCollection attributes
                );

        /// <summary>
        /// Get the byte array value of a single-valued attribute
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object the attribute is to modify.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The value of the given attribute of the object.</returns>
        byte[] GetAttributeValueInBytes(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base);

        /// <summary>
        /// Get the string value of a single-valued attribute
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">The distinguished name of the object the attribute is to modify.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The value of the given attribute of the object.</returns>
        string GetAttributeValueInString(
            DsServer dc,
            string dn,
            string attributeName,
            string ldapFilter = "(objectClass=*)",
            System.DirectoryServices.Protocols.SearchScope searchScope
                = System.DirectoryServices.Protocols.SearchScope.Base);

        /// <summary>
        /// Delete an attribute from an object in the directory.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="dn">
        /// The distinguished name of the object the attribute is to delete.
        /// </param>
        /// <param name="attributeName">The attribute name to be deleted from the object.</param>
        /// <returns>Result code of the LDAP response.</returns>
        ResultCode DeleteAttribute(DsServer dc, string dn, string attributeName);

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
        ResultCode DeleteAttributes(
                DsServer dc,
                string dn,
                string[] attributeNames
                );

        /// <summary>
        /// Add an object to a given group.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="objDn">The DN of the object.</param>
        /// <param name="grpDn">The DN of the group the object to be added to.</param>
        /// <returns>Result code from the LDAP response.</returns>
        ResultCode AddObjectToGroup(DsServer dc, string objDn, string grpDn);

        /// <summary>
        /// remove a user object from a user group.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="objDn">The DN of the user object.</param>
        /// <param name="grpDn">The DN of the group the object to be removed from.</param>
        /// <returns>Result code from the LDAP response.</returns>
        ResultCode RemoveObjectFromGroup(DsServer dc, string objDn, string grpDn);

        /// <summary>
        /// Get the Ngc Key of a computer object.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="dn">The computer object.</param>
        /// <returns>The Ngc Key of the computer.</returns>
        string GetNgcKey(DsServer dc, string dn);

        /// <summary>
        /// Set the msDS-KeyCredentialLink of a computer object according to the given Ngc Key.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="dn">The computer object.</param>
        /// <param name="ngcKey">The Ngc Key.</param>
        /// <returns>Result code of the LDAP response.</returns>
        ResultCode SetNgcKey(DsServer dc, string dn, string ngcKey);

        #endregion

        /// <summary>
        /// Get the user DN of a DsUser object.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="user">The DsUser object.</param>
        /// <returns>The DN of the user.</returns>
        string GetUserDn(DsServer dc, DsUser user);

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
        ResultCode Search(
                DsServer dc,
                string baseDn,
                string ldapFilter,
                System.DirectoryServices.Protocols.SearchScope searchScope,
                string[] attributesToReturn,
                out SearchResultEntryCollection results
                );

        ResultCode ControlledSearch(
                DsServer dc,
                string baseDn,
                string ldapFilter,
                System.DirectoryServices.Protocols.SearchScope searchScope,
                string[] attributesToReturn,
                DirectoryControlCollection controls,
                out SearchResultEntryCollection results
                );
        /// <summary>
        /// Check if the object exists on the DC.
        /// </summary>
        /// <param name="dc">The DC.</param>
        /// <param name="objDN">DN of the object to be verified.</param>
        /// <returns>True if the object already exists, false otherwise.</returns>
        bool IsObjectExist(DsServer dc, string objDN);

        bool IsObjectExistInDeletedObjectsContainer(DsServer dc, string objDN);
        #endregion

        #region Access Control Ops
        /// <summary>
        /// Grant access permission to a user on a specific object.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="user">The user that will be grant access to.</param>
        /// <param name="dn">The DN of the object to grant access on.</param>
        /// <param name="accessRight">The name of the access right to be granted.</param>
        /// <param name="controlType">Type of access.</param>
        /// <returns>True if access is granted, False otherwise.</returns>
        bool GrantAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType
                );

        /// <summary>
        /// Revoke access permission of a user from a specific object.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="user">The user that will be grant access to.</param>
        /// <param name="dn">The DN of the object to revoke access from .</param>
        /// <param name="accessRight">The name of the access right to be revoked.</param>
        /// <param name="controlType">Type of access.</param>
        /// <returns>True if access is revoked, False otherwise.</returns>
        bool RevokeAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType
                );

        /// <summary>
        /// Grant control access to a user on a specific object.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="user">The user that will be grant access to.</param>
        /// <param name="dn">The DN of the object to grant access on.</param>
        /// <param name="accessRight">The name of the control access right to be granted.</param>
        /// <param name="controlType">Type of access.</param>
        /// <param name="controlAccessRightGuid">Guid of the control access right.</param>
        /// <returns>True if access is granted, False otherwise.</returns>
        bool GrantControlAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType,
                Guid controlAccessRightGuid
        );

        /// <summary>
        /// Revoke access permission of a user from a specific object.
        /// </summary>
        /// <param name="dc">The directory server to which the LDAP connection will be made to.</param>
        /// <param name="user">The user that will be grant access to.</param>
        /// <param name="dn">The DN of the object to revoke control access on.</param>
        /// <param name="accessRight">The name of the control access right to be revoked.</param>
        /// <param name="controlType">Type of access.</param>
        /// <param name="controlAccessRightGuid">Guid of the control access right.</param>
        /// <returns>True if access is revoked, False otherwise.</returns>
        bool RevokeControlAccess(
                DsServer dc,
                DsUser user,
                string dn,
                ActiveDirectoryRights accessRight,
                AccessControlType controlType,
                Guid controlAccessRightGuid
        );

        #endregion

        #region Replication Methods Ops

        /// <summary>
        /// Get the DsDomain structure for a specific domain from a DC server.
        /// </summary>
        /// <param name="serverDnsName">The DNS host name of the DC in the domain.</param>
        /// <param name="user">The user with access to the domain info.</param>
        /// <returns>The domain information.</returns>
        DsDomain GetDomainInfo(string serverDnsName, DsUser user);

        /// <summary>
        /// Get the domain controller information.
        /// </summary>
        /// <param name="serverDnsName">The DNS host name of the DC.</param>
        /// <param name="user">The user with access to the DC.</param>
        /// <returns>The DC information.</returns>
        DsServer GetDCInfo(string serverDnsName, DsUser user);

        /// <summary>
        /// Get the repsFrom attribute of a NC replica.
        /// </summary>
        /// <param name="dc">The DC where the repsFrom attribute is located.</param>
        /// <param name="nc">The naming context where the repsFrom attribute is located.</param>
        /// <returns>The repsFrom of the given NC replica.</returns>
        REPS_FROM[] GetRepsFrom(DsServer dc, NamingContext nc);

        /// <summary>
        /// Get the repsFrom attribute of a NC replica.
        /// </summary>
        /// <param name="dc">The DC where the repsFrom attribute is located.</param>
        /// <param name="nc">The naming context where the repsFrom attribute is located.</param>
        /// <returns>The repsFrom of the given NC replica.</returns>
        REPS_FROM[] GetRepsFrom(DsServer dc, string nc);

        /// <summary>
        /// Get the repsTo attribute of a NC replica.
        /// </summary>
        /// <param name="dc">The DC where the repsFrom attribute is located.</param>
        /// <param name="nc">The naming context where the repsFrom attribute is located.</param>
        /// <returns>The repsTo of the given NC replica.</returns>
        REPS_TO[] GetRepsTo(DsServer dc, NamingContext nc);

        /// <summary>
        /// Get the Up-To-Date vector of a NC replica.
        /// </summary>
        /// <param name="dc">The DC where the repsFrom attribute is located.</param>
        /// <param name="nc">The naming context where the repsFrom attribute is located.</param>
        /// <returns>The Up-To-Date vector of the given NC replica.</returns>
        UPTODATE_VECTOR_V1_EXT GetReplUTD(DsServer dc, NamingContext nc);

        /// <summary>
        /// Fill the FsmoRoleOwners field in DsDomain.
        /// </summary>
        /// <param name="dc">A DC in the domain.</param>
        /// <param name="domain">The domain object to fill the FSMO role owners.</param>
        /// <returns>True if succeeded, false otherwise.</returns>
        bool GetFsmoRoleOwners(DsServer dc, ref DsDomain domain);


        /// <summary>
        /// Get the objectGUID sequence
        /// </summary>
        /// <param name="dc">The DC to query the GUIDs from.</param>
        /// <param name="NC">name of the specific NC.</param>
        /// <param name="UTDFilter">Up-to-Date vector filter.</param>
        /// <param name="startGUID">The start range of the GUID.</param>
        /// <param name="count">Total numbers of GUIDs requested.</param>
        /// <returns>The GUID sequence requested.</returns>
        Guid[] GetObjectGuidSequence(
            DsServer dc,
            NamingContext NC,
            UPTODATE_VECTOR_V1_EXT UTDFilter,
            Guid startGUID,
            int count);

        /// <summary>
        /// Get the service principal names of the DC.
        /// </summary>
        /// <param name="dc">The DC to query SPNs from.</param>
        /// <returns>An array of strings, each represents on SPN of the DC.</returns>
        string[] GetServicePrincipalName(DsServer dc);

        /// <summary>
        /// Get the service principal names of the DC.
        /// </summary>
        /// <param name="dc">The DC to query SPNs from.</param>
        /// <param name="dc">The dn to query SPNs from.</param>
        /// <returns>An array of strings, each represents on SPN of the DC.</returns>
        string[] GetServicePrincipalName(DsServer dc, string dn);

        /// <summary>
        /// Get the RID allocation pool of an object.
        /// </summary>
        /// <param name="dc">The DC to query RID allocation pool from.</param>
        /// <param name="dn">The DN of the DSA object.</param>
        /// <returns>the RID allocation pool</returns>
        ulong GetRidAllocationPoolFromDSA(DsServer dc, string dn);

        /// <summary>
        /// Get the RID allocation pool of an object.
        /// </summary>
        /// <param name="dc">The DC to query RID allocation pool from.</param>
        /// <param name="dn">The DN of the RID manager.</param>
        /// <returns>the RID allocation pool</returns>
        ulong GetRidAllocationPoolFromRIDManager(DsServer dc, string dn);

        /// <summary>
        /// Get the DSNAME of an object.
        /// </summary>
        /// <param name="dc">The DC to query the DSNAME from.</param>
        /// <param name="dn">The DN of the object.</param>
        /// <returns>DSNAME of the object.</returns>
        DSNAME? GetDsName(DsServer dc, string dn);

        /// <summary>
        /// Get the attributes of an object which can be replicated.
        /// </summary>
        /// <param name="dc">The DC to query the attributes from.</param>
        /// <param name="dn">The DN of the object.</param>
        /// <returns>Array of LDAP display names of the attributes.</returns>
        string[] GetObjectReplicatedAttributes(DsServer dc, string dn);

        /// <summary>
        /// Construct a new crossRef object in type of ENTINF.
        /// </summary>
        /// <param name="dc">The DC server holding a Domain Naming Master role.</param>
        /// <param name="dn">DN of the crossRef object.</param>
        /// <param name="cn">CN of the crossRef object.</param>
        /// <param name="ncName">ncName of the crossRef object.</param>
        /// <param name="dnsRoot">dnsRoot of the NCcrossRef object.<param>
        /// <returns>The ENTINF object.</returns>
        ENTINF ConstructNewCrossRefObject(DsServer dc, string dn, string cn, string ncName, string dnsRoot);

        /// <summary>
        /// Add an attribute value to the ENTINF instance.
        /// </summary>
        /// <param name="dc">DC to query schema info.</param>
        /// <param name="entInf">The ENTINF to be added attribute into.</param>
        /// <param name="att">The attribute to be added.</param>
        void AddAttribute(DsServer dc, ref ENTINF entInf, DirectoryAttribute att);

        /// <summary>
        /// Get the ENTINF of an object.
        /// </summary>
        /// <param name="dc">The DC server holding a Domain Naming Master role.<</param>
        /// <param name="dn">The DN of the object.</param>
        /// <returns>The ENTINF object.</returns>
        ENTINF? GetENTINF(DsServer dc, string dn, bool forCrossMove = false);

        /// <summary>
        /// Construct a client credential.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="user">The user as the client.</param>
        /// <returns>The security buffer descriptor.</returns>
        DRS_SecBufferDesc ConstructClientCredential(DsServer dc, DsUser user);
        #endregion

        #region Add- methods for testing purpose
        /// <summary>
        /// Add a site object to the config NC.
        /// Test against config NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added site object.</returns>
        string TestAddSiteObj(DsServer dc);

        /// <summary>
        /// Add a schema object to the schema NC.
        /// Test against schema NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added schema object.</returns>
        string TestAddSchemaObj(DsServer dc);

        /// <summary>
        /// Add a user object to the default NC.
        /// Test against default NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added user object.</returns>
        string TestAddUserObj(DsServer dc);

        /// <summary>
        /// Add a computer object to the default NC.
        /// Test against default NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added computer object.</returns>
        string TestAddComputerObj(DsServer dc);

        string TestAddGroupObj(DsServer dc);

        /// <summary>
        /// Add an app object to the app NC.
        /// Test against app NC.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <returns>DN of the newly added app object.</returns>
        string TestAddAppObject(DsServer dc);
        #endregion

        #region Lookups

        /// <summary>
        /// Get group memberships for a given user.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="names">The DSName of the object whose reverse membership is being requested.</param>
        /// <param name="operationType">Type of evaluation.</param>
        /// <param name="limitingDomain">Domain filter.</param>
        /// <returns>The filtered group membership.</returns>
        DSNAME[] GetMemberships(
            DsServer dc,
            DSNAME names,
            REVERSE_MEMBERSHIP_OPERATION_TYPE operationType,
            DSNAME? limitingDomain);

        /// <summary>
        /// Check if the server is a GC.
        /// </summary>
        /// <param name="dc">The DC to be checked.</param>
        /// <returns>True if the server is a GC, false otherwise.</returns>
        bool IsGc(DsServer dc);

        /// <summary>
        /// Get all site objects in the forest where the DC is located.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All site objects in the forest.</returns>
        DsSite[] ListSites(DsServer dc);

        /// <summary>
        /// Create a site object.
        /// </summary>
        /// <param name="dc">The DC in the same forest as the site.</param>
        /// <param name="dn">The DN of the site.</param>
        /// <returns>A DsSite object.</returns>
        DsSite GetSite(DsServer dc, string dn);

        /// <summary>
        /// Get the DN, DNS host name and serverReference objects of the server.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="dn">The DN of the server.</param>
        /// <param name="dsaObjDn">When return, contains the NTDS DSA object DN.</param>
        /// <param name="dnsHostName">When return, contains the DNS host name of the server.</param>
        /// <param name="serverReference">When return, contains the serverReference object of the server.</param>
        void ListInfoForServer(
            DsServer dc,
            string dn,
            out string dsaObjDn,
            out string dnsHostName,
            out string serverReference);

        /// <summary>
        /// Get all domains in the forest.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All domain objects in the forest.</returns>
        DsDomain[] ListDomains(DsServer dc);

        /// <summary>
        /// Get all NCs in the forest.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All NC DNs in the forest.</returns>
        string[] ListNCs(DsServer dc);

        /// <summary>
        /// Get all DCs in a given site.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="siteDn">The site to query servers from.</param>
        /// <returns>All servers in the site.</returns>
        DsServer[] ListServersWithDcsInSite(DsServer dc, string siteDn);

        /// <summary>
        /// Get all GC servers in the forest.
        /// </summary>
        /// <param name="dc">The DC in the forest.</param>
        /// <returns>All GC servers in the forest.</returns>
        DsServer[] ListGcServers(DsServer dc);

        /// <summary>
        /// Get all DCs of a specific domain in a specific site.
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="domainDn">The specific domain.</param>
        /// <param name="siteDn">The specific site.</param>
        /// <returns>DCs in the specific domain in the specific site.</returns>
        DsServer[] ListServersForDomainInSite(DsServer dc, string domainDn, string siteDn);

        /// <summary>
        /// Get the parent object of a given object.
        /// </summary>
        /// <param name="childDn">The object DN who's parent is being queried.</param>
        /// <returns>DN of the parent object.</returns>
        string GetParentObjectDn(string childDn);

        /// <summary>
        /// Lookup names in given format, return the name in another given format
        /// </summary>
        /// <param name="dc">The DC to talk to.</param>
        /// <param name="flags">DS_NAME_FLAG flags.</param>
        /// <param name="formatOffered">The format of the name in `name'.</param>
        /// <param name="formatDesired">the format of the name in the return value.</param>
        /// <param name="name">Input name to translate.</param>
        /// <returns>A DS_NAME_RESULT_ITEMW structure containing the translated name.</returns>
        DS_NAME_RESULT_ITEMW LookupNames(
            DsServer dc,
            uint flags,
            uint formatOffered,
            uint formatDesired,
            string name);

        /// <summary>
        /// List all DC names in the current domain.
        /// </summary>
        /// <param name="dc">A GC in the domain.</param>
        /// <param name="domainDn">The DN of the domain.</param>
        /// <returns>DC names in the domain.</returns>
        string[] ListDCNamesInDomain(DsServer dc, string domainDn);
        #endregion

        /// <summary>
        /// Construct an ENTINF structure with the specified attributes for an object 
        /// </summary>
        /// <param name="dc">The DC which holds the object.</param>
        /// <param name="dn">The DN of the object.</param>
        /// <param name="attCollection">The attribute collection of the object.</param>
        /// <returns>A ENTINF structure contains the attributes of object.</returns>
        ENTINF ConstructENTINF(DsServer dc, string dn, DirectoryAttributeCollection attCollection);
    }
}
