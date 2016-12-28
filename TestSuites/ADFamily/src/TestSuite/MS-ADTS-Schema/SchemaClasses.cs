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
    /// This file is the source file for Validation of the TestCase2 and TestCase3.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region SchemaClasses 

        /// <summary>
        /// This method validates SchemaClasses Syntactically.
        /// </summary>
        public void ValidateSchemaClasses()
        {
            bool hasErrors = false;
            HashSet<string> seenClasses = new HashSet<string>();
            foreach (IObjectOnServer serverObject in adAdapter.GetAllSchemaClasses())
            {
                ModelObject classObject;
                if (!dcModel.TryGetClass(serverObject.Name, out classObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schema class '{0}' exists on server but not in model", 
                        serverObject.Name);
                }
                else
                {
                    CompareObjects(classObject, serverObject, ref hasErrors);
                }
                seenClasses.Add(serverObject.Name.ToLower());
            }
            foreach (string className in dcModel.classMap.Keys)
            {
                if (!seenClasses.Contains(className))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "schema class '{0}' exists in model but not on server", 
                        className);
                }
            }
            DataSchemaSite.Assert.IsTrue(
                !hasErrors, 
                "server and model schema classes must coincide (see log for details)");
        }

        #endregion

        #region LDSSchemaClasses

        /// <summary>
        /// This method validates LDSSchemaClasses Syntactically. 
        /// </summary>
        public void ValidateLdsSchemaClasses()
        {
            bool hasErrors = false;
            HashSet<string> seenClasses = new HashSet<string>();
            foreach (IObjectOnServer serverObject in adAdapter.GetAllLdsSchemaClasses())
            {
                ModelObject classObject;
                if (!adamModel.TryGetClass(serverObject.Name.ToLower(), out classObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schema class '{0}' exists on server but not in model", 
                        serverObject.Name);
                }
                else
                {
                    CompareObjects(classObject, serverObject, ref hasErrors);
                }
                seenClasses.Add(serverObject.Name.ToLower());
            }
            foreach (string className in adamModel.classMap.Keys)
            {
                if (!seenClasses.Contains(className))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schema class '{0}' exists in model but not on server", 
                        className);
                }
            }
            DataSchemaSite.Assert.IsTrue(
                !hasErrors, 
                "server and model schema classes must coincide (see log for details)");
        }
        #endregion
    }
}