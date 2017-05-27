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
    /// This file is the source file for Validation of the TestCase4 and TestCase5.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region SchemaAttributes Validation.

        /// <summary>
        /// This method validates SchemaAttributes Syntactically.
        /// </summary>
        public void ValidateSchemaAttributes()
        {
            bool hasErrors = false;
            HashSet<string> seenAttributes = new HashSet<string>();
            foreach (IObjectOnServer serverObject in adAdapter.GetAllSchemaAttributes())
            {
                ModelObject attributeObject;
                if (!dcModel.TryGetAttribute(serverObject.Name, out attributeObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schema attribute '{0}' exists on server but not in model", 
                        serverObject.Name);
                }
                else
                {
                    CompareObjects(attributeObject, serverObject, ref hasErrors);
                }
                seenAttributes.Add(serverObject.Name.ToLower());
            }
            foreach (string attributeName in dcModel.attributeMap.Keys)
            {
                if (!seenAttributes.Contains(attributeName))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schema attribute '{0}' exists in model but not on server", 
                        attributeName);
                }
            }
            DataSchemaSite.Assert.IsTrue(
                !hasErrors, 
                "server and model schema attributes must coincide (see log for details)");
        }

        #endregion

        #region LdsSchemaAttributes Implementation.

        /// <summary>
        /// This method validates LdsSchemaClasses Syntactically.
        /// </summary>
        public void ValidateLdsSchemaAttributes()
        {
            bool hasErrors = false;
            HashSet<string> seenAttributes = new HashSet<string>();
            foreach (IObjectOnServer serverObject in adAdapter.GetAllLdsSchemaAttributes())
            {
                ModelObject attributeObject;
                if (!adamModel.TryGetAttribute(serverObject.Name.ToLower(), out attributeObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "schema attribute '{0}' exists on server but not in model",
                        serverObject.Name);
                }
                else
                {
                    CompareObjects(attributeObject, serverObject, ref hasErrors);
                }
                seenAttributes.Add(serverObject.Name.ToLower());
            }
            foreach (string attributeName in adamModel.attributeMap.Keys)
            {
                if (!seenAttributes.Contains(attributeName))
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schema attribute '{0}' exists in model but not on server", 
                        attributeName);
            }
            DataSchemaSite.Assert.IsTrue(
                !hasErrors, 
                "server and model schema attributes must coincide (see log for details)");
        }

        #endregion
    }
}