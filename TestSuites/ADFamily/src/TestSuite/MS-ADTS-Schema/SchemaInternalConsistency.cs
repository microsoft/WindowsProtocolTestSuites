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
    /// This file is the source file for Validation of the TestCase1.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region SchemaInternalConsistency Validation
        /// <summary>
        /// This method validates SchemaInternalConsistency. 
        /// </summary>
        public void ValidateSchemaInternalConsistency()
        {
            // Dump schema and errors to file
            DataSchemaSite.Log.Add(LogEntryKind.Comment, "dumping diagnostics and schema to {0}", adAdapter.SchemaLog);
            StreamWriter writer = new StreamWriter(adAdapter.SchemaLog);
            writer.WriteLine(DSSchemaLoadResult.diagnosticMessage);
            dcModel.Print(new IndentedTextWriter(writer));
            writer.Close();
            if (!DSSchemaLoadResult.IsSuccess)
            {
                string[] diagLines = DSSchemaLoadResult.diagnosticMessage.Split(
                    new string[] { "\r\n" }, 
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in diagLines)
                {
                    DataSchemaSite.Log.Add(LogEntryKind.Comment, line);
                }
                DataSchemaSite.Assert.Fail(
                    "schema loaded from TD must not have inconsistencies (see also {0})",
                    adAdapter.SchemaLog);
            }
            DataSchemaSite.Log.Add(
                LogEntryKind.Comment, 
                @"The SchemaInternalConsistency test case is for validating whether the System
                Model is loaded correctly or not. Also this test case will print the structure of Loaded Virtual 
                Information Tree in a separate log file. There are no ADTS requirements associated with validating
                the Schema Internal Consistency. ");
        }

        #endregion
    }
}