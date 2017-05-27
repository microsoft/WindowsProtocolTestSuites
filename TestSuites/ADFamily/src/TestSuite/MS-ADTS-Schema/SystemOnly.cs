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
    /// This file is the source file for Validation of the TestCase6 and TestCase7.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        //ObjectListToCheck is a string array contains all objects whose 'SystemOnly' attribute value is TRUE
        string[] objectListToCheck = { "objectClass", "attributeID", "schemaIDGUID", "msDS-IntId", "mAPIID",
            "attributeSyntax", "oMSyntax","oMObjectClass","isSingleValued", "systemFlags","systemOnly", "governsID", "rDNAttID",
            "subClassOf", "systemMustContain", "systemMayContain", "systemPossSuperiors",
            "systemAuxiliaryClass", "objectClassCategory"};
        bool isDS;
        #region SystemOnly Validation.
        // <summary>
        /// This method validates the requirements under 
        /// SystemOnly Scenario.
        /// </summary>
        public void ValidateSystemOnly()
        {
            isDS = true;

            #region Logging the Requirements for Scenario S1

            foreach (string entry in objectListToCheck)
            {
                CaptureSystemOnly(entry);
            }

            #endregion
        }

        #endregion

        #region LDSSystemOnly Validation.

        // <summary>
        /// This method validates the requirements under 
        /// LDSSystemOnly Scenario.
        /// </summary>
        public void ValidateLDSSystemOnly()
        {
            isDS = false;

            #region Logging the Requirements for Scenario S2

            foreach (string entry in objectListToCheck)
            {
                //Excluding the mAPIID in AD/LDS as mAPIID does not exists in AD/LDS
                if (entry == "mAPIID")
                {
                    continue;
                }
                CaptureSystemOnly(entry);
            }

            #endregion
        }

        #endregion

        #region Common Method

        /// <summary>
        /// CaptureSystemOnly is a Common method for Both AD/DS and AD/LDS, 
        /// which takes the Object entry from ObjectList and
        /// Verify the SystemOnly Value is set to TRUE or not
        /// </summary>
        /// <param name="entry">entry from the objectList</param>
        private void CaptureSystemOnly(string entry)
        {
            // Creating attribute model Object
            ModelObject attributeObject; 

            //Checking For AD/DS or AD/LDS.
            if (isDS)
            {
                dcModel.attributeMap.TryGetValue(entry.ToLower(), out attributeObject);
            }
            else
            {
                adamModel.attributeMap.TryGetValue(entry.ToLower(), out attributeObject);
            }
            bool value = (bool)attributeObject[StandardNames.systemOnly].UnderlyingValues.ToArray()[0];
            
            switch (entry)
            {
                case "objectClass":
                    //MS-ADTS-Schema_R74
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        74, 
                        "The objectClass attribute of the attributeSchema is System-only.");
                    //MS-ADTS-Schema_R158
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        158, 
                        "The objectClass attribute of the classSchema is System-only.");
                    break;

                case "attributeID":
                    //MS-ADTS-Schema_R78
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        78, 
                        "The attributeID attribute of the attributeSchema is System-only.");
                    break;

                case "schemaIDGUID":
                    //MS-ADTS-Schema_R82
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        82, 
                        "The schemaIDGUID attribute of the attributeSchema is System-only.");
                    //MS-ADTS-Schema_R163
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        163, 
                        "The schemaIDGUID attribute of the classSchema is System-only.");
                    break;

                case "msDS-IntId":
                    //MS-ADTS-Schema_R84
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        84, 
                        "The msDS-IntId attribute of the attributeSchema is System-only.");
                    //MS-ADTS-Schema_R165
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        165, 
                        "The msDS-IntId attribute of the classSchema is System-only.");
                    break;

                case "mAPIID":
                    //MS-ADTS-Schema_R88
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        88, 
                        "The mAPIID attribute of the attributeSchema is System-only.");
                    break;

                case "attributeSyntax":
                    //MS-ADTS-Schema_R89
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        89, 
                        "The attributeSyntax attribute of the attributeSchema is System-only.");
                    break;

                case "oMSyntax":
                    //MS-ADTS-Schema_R90
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        90, 
                        "The oMSyntax attribute of the attributeSchema is System-only.");
                    break;

                case "oMObjectClass":
                    //MS-ADTS-Schema_R91
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        91, 
                        "The oMObjectClass attribute of the attributeSchema is System-only.");
                    break;

                case "isSingleValued":
                    //MS-ADTS-Schema_R94
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        94, 
                        "The isSingleValued attribute of the attributeSchema is System-only.");
                    break;

                case "systemFlags":
                    //MS-ADTS-Schema_R98
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        98, 
                        "The systemFlags attribute of the attributeSchema is System-only.");
                    //MS-ADTS-Schema_R183
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        183, 
                        "The systemFlags attribute of the classSchema is System-only.");
                    break;

                case "systemOnly":
                    //MS-ADTS-Schema_R99
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        99, 
                        "The systemOnly attribute of the attributeSchema is System-only.");
                    //MS-ADTS-Schema_R184
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        184, 
                        "The systemOnly attribute of the classSchema is System-only.");
                    break;

                case "governsID":
                    //MS-ADTS-Schema_R162
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value,
                        162, 
                        "The governsID attribute of the classSchema is System-only.");
                    break;

                case "rDNAttID":
                    //MS-ADTS-Schema_R168
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        168, 
                        "The rDNAttID attribute of the classSchema is System-only.");
                    break;

                case "subClassOf":
                    //MS-ADTS-Schema_R170
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        170, 
                        "The subClassOf attribute of the classSchema is System-only.");
                    break;

                case "systemMustContain":
                    //MS-ADTS-Schema_R172
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        172, 
                        "The systemMustContain attribute of the classSchema is System-only.");
                    break;

                case "systemMayContain":
                    //MS-ADTS-Schema_R174
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        174, 
                        "The systemMayContain attribute of the classSchema is System-only.");
                    break;

                case "systemPossSuperiors":
                    //MS-ADTS-Schema_R176
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        176, 
                        "The systemPossSuperiors attribute of the classSchema is System-only.");
                    break;

                case "systemAuxiliaryClass":
                    //MS-ADTS-Schema_R178
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        178, 
                        "The systemAuxiliaryClass attribute of the classSchema is System-only.");
                    break;

                case "objectClassCategory":
                    //MS-ADTS-Schema_R180
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        value, 
                        180, 
                        "The objectClassCategory attribute of the classSchema is System-only.");
                    break;
            }
        }

        #endregion
    }
}