// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Text;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// delegate to handle the search operation
    /// </summary>
    /// <param name="response">Enum that represents the search response</param>
    public delegate void ResponseHandler(SearchResp response);

    /// <summary>
    /// Interface that holds the methods to define the LDAP operations
    /// </summary>
    public interface IAD_LDAPModelAdapter : IAdapter
    {
        /// <summary>
        /// Initializes the abstract data model with required objects and classes.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Reset the abstract data model to original state
        /// </summary>
        new void Reset();

        /// <summary>
        /// This method sets the AD connection and binding for the test suite
        /// </summary>
        /// <param name="service">Specifies the binding service is AD DS or AD LDS</param>
        /// <param name="serverHostName">Specifies which host to connect and bind to</param>
        /// <param name="userName">Specifies the username for authentication</param>
        /// <param name="password">Specifies the password for authentication</param>
        void SetConnectAndBind(
            ADImplementations service,
            string serverHostName,
            string userName = null,
            string password = null);

        /// <summary>
        /// AddForestDnsZones method is used to add the object ForestDnsZones to AD
        /// </summary>
        /// <param name="forestDNSZonesObjCN">the ForestDNSZones object cn</param>
        void AddForestDnsZones(
            string forestDNSZonesObjCN);

        /// <summary>
        /// Action describing the behavior of AddOperation
        /// </summary>
        /// <param name="attribnVals">Variable that contains list of attributes and their corresponding values</param>
        /// <param name="AccessRights">Enum that specifies the access rights of the Parent of new object to be added</param>
        /// <param name="NCRights">Rights on NC</param>
        /// <param name="dcLevel">DC functional level</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Add Operation is on AD DS or AD LDS</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        void AddOperation(
            Sequence<string> attribnVals,
            RightsOnParentObjects AccessRights,
            NCRight NCRights,
            ServerVersion dcLevel,
            string control,
            ADImplementations service,
            bool isRODC,
            out ConstrOnAddOpErrs errorStatus);

        /// <summary>
        /// Action describing the behavior of the Delete Operation
        /// </summary>
        /// <param name="objtoDel">DN of the object to be deleted</param>
        /// <param name="parentRights">Enum containing the access rights on the parent of the object being deleted</param>
        /// <param name="objectRights">Enum containing the access rights on the object being deleted</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Delete Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        void DeleteOperation(
            string objtoDel,
            RightsOnParentObjects parentRights,
            RightsOnObjects objectRights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnDelOpErr errorStatus);

        /// <summary>
        /// Action describing the behavior of ModifyOperation
        /// </summary>
        /// <param name="attribVal">Variable that contains attributes to be modified mapped to the list of existing attributes on a particular object</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        /// Disable CA1006, because according to current test suite design, there is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        void ModifyOperation(
            Map<string, Sequence<string>> attribVal,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus);

        /// <summary>
        /// Handles search request
        /// </summary>
        /// <param name="baseObjectDN">DN of the base object</param>
        /// <param name="filter">Conditions that must be fulfilled inorder for search to match a given entry</param>
        /// <param name="scope">Defines scope of search</param>
        /// <param name="attributesToBeReturned">specifies attributes of interest</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Search Operation is on AD DS or AD LDS</param>
        /// Disable CA1006, because according to current test suite design, there is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        void SearchOpReq(
            string baseObjectDN,
            string filter,
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.SearchScope scope,
            Sequence<string> attributesToBeReturned,
            string control,
            ADImplementations service);

        /// <summary>
        /// Event triggered when the search response is returned.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ResponseHandler SearchOpResponse;

        /// <summary>
        /// Performs Tree Delete operation
        /// </summary>
        /// <param name="objtoDel">DN of the object to be deleted</param>
        /// <param name="objectRights">Enum defining rights on object to be deleted</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Tree Delete Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        void TreeDeleteOperation(
            string objtoDel,
            RightsOnObjects objectRights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnDelOpErr errorStatus);

        /// <summary>
        /// Performs ModifyDN operation (both intra domain and cross domain move. 
        /// In case of cross domain move the parameter "LDAPBindWithDelegationEnabled" is used 
        /// And the parameters "containerRights", "isfDoListObjectOfdSHeuristicsSet", "ParentRights" are not used, 
        /// Converse of this combination of parameters applies to intra domain operation whereas rest of the parameters 
        /// Are used for both the operations.
        /// </summary>
        /// <param name="distinguishedNames">Distinguished name of the object to be modified, the new distinguished name 
        /// And Boolean variable specifying whether to delete the old RDN</param>
        /// <param name="rightsOnObject">Enum specifying requester's rights on the modifying object</param>
        /// <param name="accessRights">Enum specifying requester rights on the parent object mentioned in the newRDN</param>
        /// <param name="ParentRights">Enum specifying requester rights on the parent object mentioned in the oldDN</param>
        /// <param name="LDAPBindWithDelegationEnabled">set to true if kerberos bind with delegation enabled is 
        /// performed otherwise false</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum Specifying the error status returned</param>
        void ModifyDNOperation(
            Sequence<string> distinguishedNames,
            RightsOnObjects rightsOnObject,
            RightsOnParentObjects accessRights,
            RightOnOldParentObject ParentRights,
            bool LDAPBindWithDelegationEnabled,
            string control,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModDNOpErrs errorStatus);

        /// <summary>
        /// Performs an LDAP Extended operation
        /// </summary>
        /// <param name="operation">Specifies an extended operation </param>
        /// <param name="dcLevel">Windows version if windows is the platform otherwise represents a non-Windows platform</param>
        /// <param name="service">AD/DS or AD/LDS</param>
        /// <param name="response">Represents the response (Valid or InValid)</param>
        void LDAPExtendedOperation(
            string operation,
            ServerVersion dcLevel,
            ADImplementations service,
            out ExtendedOperationResponse response);

        /// <summary>
        /// Method that takes up the task of maintaining consistent initial state
        /// </summary>
        void UnBind();

        /// <summary>
        /// Performs Search deleted object operations.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name of an object to be searched.</param>
        /// <param name="controls"> The oid or the name of the ExtendedControls</param>
        /// <returns>The searched result entry.</returns>
        Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry SearchDeletedObject(
            string distinguishedName,
            string controls);

        /// <summary>
        /// Action describing the behavior of ModifyOperation
        /// </summary>
        /// <param name="attribVal">Variable that contains attributes to be modified mapped to the list of existing attributes on a particular object</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        /// Disable CA1006, because according to current test suite design, there is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        void ModifyRecycleBin(
            Map<string, Sequence<string>> attribVal,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus);

        /// <summary>
        /// Action to restore the deleted object
        /// </summary>
        /// <param name="restoreObjectDN">Variable that specify the distinguished name of the deleted object</param>
        /// <param name="newDN">Variable that specify the distinguished name of the new object</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        void RestoreDeletedObject(
            string restoreObjectDN,
            string newDN,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus);

        /// <summary>
        /// Action to modify the well-known objects
        /// </summary>
        /// <param name="operationType">Variable that specify the operation type for the WKO</param>
        /// <param name="rights">Enum that specifies the access rights on attribute to be modified</param>
        /// <param name="control">Defines an Extended control</param>
        /// <param name="service">Specifies whether the Modify Operation is on AD DS or AD LDS</param>
        /// <param name="dcLevel">Enum to specify specific Windows or Non-windows</param>
        /// <param name="isRODC">Specifies if the DC is RODC</param>
        /// <param name="errorStatus">Enum variable specifying the error status</param>
        void ModifyWellKnownObject(
            string operationType,
            RightsOnAttributes rights,
            string control,
            ADImplementations service,
            ServerVersion dcLevel,
            bool isRODC,
            out ConstrOnModOpErrs errorStatus);

        /// <summary>
        /// Set AD object life time
        /// </summary>
        /// <param name="deletedObjectLifetimeVal">The value of msDS-DeletedObjectLifetime</param>
        /// <param name="tombstoneLifetimeVal">The value of tombstoneLifetime</param>
        void SetADObjectLifeTime(
            int deletedObjectLifetimeVal,
            int tombstoneLifetimeVal);

        /// <summary>
        /// Do Garbage Collection
        /// </summary>
        void DoGarbageCollection();
    }
}
