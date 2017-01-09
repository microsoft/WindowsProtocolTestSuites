// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase12 and TestCase13.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region IsSingleValued Validation.

        /// <summary>
        /// This method validates the requirements under 
        /// IsSingleValued Scenario.
        /// </summary>
        public void ValidateIsSingleValued()
        {            
            DirectoryEntry dirEntry = new DirectoryEntry();
            //Verifying the singleValued attribute for specific object.
            if (!adAdapter.GetObjectByDN("CN=Guest,CN=Users," + adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false, 
                    "CN=Guest,CN=Users,"
                    +adAdapter.rootDomainDN 
                    + " Object is not found in server");
            }
            IsSingleValued(dirEntry);
        }
        #endregion

        #region LDSIsSingleValued Validation.
        /// <summary>
        /// This method validates the requirements under 
        /// LDSIsSingleValued Scenario.
        /// </summary>
        public void ValidateLDSIsSingleValued()
        {
            DirectoryEntry dirEntry = new DirectoryEntry();
            //Verifying the singleValued attribute for specific object.
            if (!adAdapter.GetLdsObjectByDN("CN=Services,CN=Configuration," + adAdapter.LDSRootObjectName,
                out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false, 
                    "CN=Services,CN=Configuration," 
                    + adAdapter.LDSRootObjectName 
                    + " Object is not found in server");
            }
            IsSingleValued(dirEntry);
        }

        #endregion

        bool SingleValued(ModelObject obj, string attribute)
        {
            if (obj == null)
            {
                DataSchemaSite.Assert.IsNotNull(obj,attribute + " is not existing in Model");
            }
            bool value = bool.Parse(obj[StandardNames.isSingleValued].ToString());
            return value;
        }
        void IsSingleValued(DirectoryEntry dirEntry)
        {
            //Count variables for verifying the number of values occurred so far.
            int count = 0, serverCount;
            ModelObject obj = dcModel.GetAttribute(StandardNames.cn);
            if (SingleValued(obj, StandardNames.cn))
            {
                count = 1;
            }
            serverCount = dirEntry.Properties[StandardNames.cn].Count;
            //MS-ADTS-Schema_R92
            DataSchemaSite.CaptureRequirementIfIsTrue(
                count == serverCount,  
                92,
                "The attribute isSingleValued is true, if this attribute is a single-valued.");

            ModelObject obj1 = dcModel.GetAttribute(StandardNames.objectClass);
            if (!SingleValued(obj1, StandardNames.objectClass))
            {
                //For multivalued attributes the values will be more than 2.
                count = 2;     
            }
            serverCount = dirEntry.Properties[StandardNames.objectClass].Count;
            //MS-ADTS-Schema_R93
            DataSchemaSite.CaptureRequirementIfIsTrue(
                count <= serverCount,  
                93,
                "The attribute isSingleValued is false, if this attribute is a multi-valued.");
        }
        
    }
}