// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{
   public interface IFRS2ServerControllerAdapter:IAdapter
    {

         /// <summary>
        /// Retrieve Object with attributes is to retrieve server Object Variables.
        /// </summary>
        /// <param name="distinguishedName"> ServerDistinguished Name</param>
        /// <param name="serverName">Server Name</param>
        /// <param name="ldapFilter">Fileter String</param>
        /// <param name="attributes">Attributes to be queried</param>
        /// <param name="scope">Search Scope</param>
        /// <param name="searchResponse">Search Response</param>
        /// <returns></returns>
       void RetrieveObjectwithattributes(string distinguishedName, string serverName, string ldapFilter, string[] attributes, System.DirectoryServices.Protocols.SearchScope scope, out  System.DirectoryServices.Protocols.SearchResponse searchResponse);
       
        /// <summary>
        /// AdValidation is used to validate AD requirements.
        /// This method verifies the particular property of the object exists or not.
        /// </summary>
        /// <param name="GuidString">Distinguished Name</param>
        /// <param name="GuidClass">ladpFilter</param>
        /// <param name="refString">attribute</param>
        /// <returns></returns>
       bool AdValidation(String GuidString, string GuidClass, string refString);
         /// <summary>
        /// Adavalidation method is used to Validate AD related Requiremnts.
        /// This method compares integer type attribute values of the specified object
        /// </summary>
        /// <param name="GuidString">Distinguished Name</param>
        /// <param name="GuidClass">ladpFilter</param>
        /// <param name="refString">Attribute</param>
        /// <param name="comparreValue">ExprectedValue</param>
        /// <returns></returns>
       bool AdValidation(String GuidString, string GuidClass, string refString, int comparreValue);
         /// <summary>
        /// Adavalidation method is used to Validate AD related Requiremnts.
        /// This method compares boolean type attribute values of the specified object
        /// </summary>
        /// <param name="GuidString">Distinguished Name</param>
        /// <param name="GuidClass">ladpFilter</param>
        /// <param name="refString">Attribute</param>
        /// <param name="comparreValue">ExprectedValue</param>
        /// <returns></returns>

        bool AdValidation(String GuidString, string GuidClass, string refString, bool comparreValue);
         /// <summary>
        /// SetAccessRights method sets the particular access right for a particular user on a particualr AD Container/Object
        /// </summary>
        /// <param name="DN">The distinguished name of the Container/Object.</param>
        /// <param name="user">The name of the user to whom the permissions to be set</param>
        /// <param name="domain">The name of the domain to which user is belongs to</param>
        /// <param name="accessRight">The name of the access right to be set</param>
        /// <param name="controlType">Allow/Deny particular ActiveDirecotyRights</param>
        /// <returns></returns>
       bool SetACLs(string DN, string user, string domain, ActiveDirectoryRights accessRight, AccessControlType controlType);
          /// <summary>
        /// InitializeGuid method is for initializeing ConnectionId,ContentSetId, ReplicationId
        /// </summary>
        /// <param name="GuidString">DistiguishedName</param>
        /// <param name="GuidClass">ldaptFilter</param>
        /// <returns></returns>
       Guid InitializeGuid(String GuidString, string GuidClass);
       /// <summary>
       /// GetDFSRFilters is to get the msDFSR fileFilters and Directory filters.
       /// </summary>
       /// <param name="connectionId">ConnectionSet id of the Replication Folder</param>
       /// <param name="filterString">msDFSR Filter.</param>
       /// <returns></returns>
       string GetDFSRFilters(int connectionId, string filterString);

       Guid GetNtdsConnectionGuid(string path, string fromServer);
      
      


       

     
       


    }
}
