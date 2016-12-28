// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// IObjectOnServer is an interface, a (simplified) representation of a schema object on the server.
    /// </summary>
    public interface IObjectOnServer
    {
        /// <summary>
        /// This Property returns the name of the Object on server.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// This property returns the Properties of the Object on server.
        /// </summary>
        Dictionary<string, object[]> Properties { get; }
    }

    /// <summary>
    /// IActiveDirectory is an interface, which is derived from IAdapter interface.
    /// This is a managed adapter having methods for initializing of Current AD/DS DomainDN, AD/LDS DomainDN, 
    /// Retrieves all the Schema Classes and the Schema Attributes of Both AD/DS and AD/LDS.
    /// </summary>
    public interface IADDataSchemaAdapter : IAdapter
    {
        /// <summary>
        /// This Method is used to retrieve all the schema classes from AD/DS domain controller.
        /// </summary>
        /// <returns>Returns Enumerated Schema Classes</returns>
        [MethodHelp(@"The GetAllSchemaClasses method is used to get all the schema classes from AD/DS 
                    domain controller.")]
        IEnumerable<IObjectOnServer> GetAllSchemaClasses();

        /// <summary>
        /// This Method is used to retrieve all the schema attributes from AD/DS domain controller. 
        /// </summary>
        /// <returns>Returns Enumerated Schema Attributes</returns>
        [MethodHelp(@"The GetAllSchemaAttributes method is used to get all the schema attributes from AD/DS 
                    domain controller.")]
        IEnumerable<IObjectOnServer> GetAllSchemaAttributes();

        /// <summary>
        /// This Method is used to retrieve all the schema classes from AD/LDS domain controller.
        /// </summary>
        /// <returns>Returns Enumerated AD/LDS Schema Classes</returns>
        [MethodHelp(@"The GetAllLdsSchemaClasses method is used to get all the schema classes from AD/LDS 
                    domain controller")]
        IEnumerable<IObjectOnServer> GetAllLdsSchemaClasses();

        /// <summary>
        /// This Method is used to retrieve all the schema attributes from AD/LDS domain controller. 
        /// </summary>
        /// <returns>Returns Enumerated AD/LDS Schema Attributes</returns>
        [MethodHelp(@"The GetAllLdsSchemaAttributes method is used to get all the schema attributes from AD/LDS 
                    domain controller")]
        IEnumerable<IObjectOnServer> GetAllLdsSchemaAttributes();
        
        /// <summary>
        /// This Method is used to retrieve AD/DS Object for a specified Distinguished Name.
        /// </summary>
        /// <param name="dn">Distinguished Name</param>
        /// <param name="obj">Object of Distinguished Name </param>
        /// <returns>Returns TRUE if Object exists </returns>
        [MethodHelp(@"The GetObjectByDN method is used to retrieve AD/DS Object for a specified 
                    Distinguished Name")]
        bool GetLdsObjectByDN(string dn, out System.DirectoryServices.DirectoryEntry obj);

        /// <summary>
        /// This Method is used to retrieve AD/LDS Object for a specified Distinguished Name.
        /// </summary>
        /// <param name="dn">Distinguished Name</param>
        /// <param name="obj">Object of Distinguished Name </param>
        /// <returns>Returns TRUE if Object exists </returns>
        [MethodHelp(@"The GetLdsObjectByDN method is used to retrieve AD/LDS Object for a specified 
                    Distinguished Name")]
        bool GetObjectByDN(string dn, out System.DirectoryServices.DirectoryEntry obj);
    }
}
