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
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// A PTF test class
    /// This file is the source file for Initialization of the class variables, 
    /// Creating the instances for Adapters and validation of requirements by calling
    /// the respective Testcase methods.
    /// </summary>
    [TestClass]
    public partial class DataSchemaTestSuite : TestClassBase
    {
        #region Variables

        static ModelDomainController dcModel;
        static ModelDomainController adamModel;
        static ModelResult DSSchemaLoadResult;
        static ModelResult LDSSchemaLoadResult;
        static ADDataSchemaAdapter adAdapter;

        /// <summary>
        /// Temporary creation of Site Variable
        /// </summary>
        public static ITestSite DataSchemaSite = null;
        static OSVersion serverOS; 

        #endregion

        #region Test Suite Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
    
            // Initializing the ITestSite object
            if (null == DataSchemaSite)
            {
                DataSchemaSite = TestClassBase.BaseTestSite;
            }
            adAdapter = new ADDataSchemaAdapter();
            adAdapter.Initialize(DataSchemaSite);

            //Model for AD/DS
            dcModel = new ModelDomainController(adAdapter.rootDomainDN, null);
            
            //Protocol Short Name.
            DataSchemaSite.DefaultProtocolDocShortName = "MS-ADTS-Schema";

            //Specifying the windows version
            ServerVersion serverVersion = (ServerVersion)adAdapter.PDCOSVersion;
            if (serverVersion == ServerVersion.Win2003) serverOS = OSVersion.WinSvr2003;
            else if (serverVersion == ServerVersion.Win2008) serverOS = OSVersion.WinSvr2008;
            else if (serverVersion == ServerVersion.Win2008R2) serverOS = OSVersion.WinSvr2008R2;
            else if (serverVersion == ServerVersion.Win2012) serverOS = OSVersion.WinSvr2012;
            else if (serverVersion == ServerVersion.Win2012R2) serverOS = OSVersion.WinSvr2012R2;
            else if (serverVersion == ServerVersion.Win2016) serverOS = OSVersion.Win2016;
            else if (serverVersion == ServerVersion.Winv1803) serverOS = OSVersion.Winv1803;
            else serverOS = OSVersion.OtherOS;

            //Storing the XML paths.
            string[] tdSources;
            if (serverVersion <= ServerVersion.Win2012R2)
            {
                tdSources = adAdapter.TDXmlPath.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                tdSources = adAdapter.OpenXmlPath.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            Map<string, string> rootDomainDNSubs = new Map<string, string>().Add("<RootDomainDN>", adAdapter.rootDomainDN);
            DSSchemaLoadResult = dcModel.LoadSchema(
                SchemaReader.ReadSchema(
                tdSources,
                null,
                true,
                serverOS),
                rootDomainDNSubs,
                serverOS);

            if (adAdapter.RunLDSTestCases)
            {
                string[] ldsTdSource;
                if (serverVersion <= ServerVersion.Win2012R2)
                {
                    ldsTdSource = adAdapter.LdsTDXmlPath.Split(',');
                }
                else
                {
                    ldsTdSource = adAdapter.LdsOpenXmlPath.Split(',');
                }
                //Model for AD/LDS
                adamModel = new ModelDomainController(adAdapter.LDSRootObjectName, null);
                Map<string, string> adamRootDomainDNSubs = new Map<string, string>().Add("<RootDomainDN>", adAdapter.LDSRootObjectName);

                LDSSchemaLoadResult = adamModel.LoadSchema(
                    SchemaReader.ReadSchema(
                    ldsTdSource,
                    null,
                    true,
                    serverOS),
                    adamRootDomainDNSubs,
                    serverOS);

            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Case Initialization

        protected override void TestInitialize()
        {
            if (!DSSchemaLoadResult.IsSuccess)
            {
                DataSchemaSite.Log.Add(
                    LogEntryKind.Warning,
                    "Inconsistency detected during ds schema load: {0}",
                    DSSchemaLoadResult.resultCode.ToString());
            }
            DataSchemaSite.Log.Add(
                LogEntryKind.Comment,
                "read {0} attribute definitions, {1} class definitions in DS",
                dcModel.attributeMap.Count,
                dcModel.classMap.Count);
            if (adAdapter.RunLDSTestCases)
            {
                if (!LDSSchemaLoadResult.IsSuccess)
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "Inconsistency detected during lds schema load: {0}",
                        LDSSchemaLoadResult.resultCode.ToString());
                }
                DataSchemaSite.Log.Add(
                    LogEntryKind.Comment,
                    "read {0} attribute definitions, {1} class definitions in LDS",
                    adamModel.attributeMap.Count,
                    adamModel.classMap.Count);
            }
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Test Case1 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_SchemaInternalConsistency()
        {
            ValidateSchemaInternalConsistency();
        }

        /// <summary>
        /// Test Case2 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
        public void Schema_SchemaClasses()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateSchemaClasses();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case3 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LdsSchemaClasses()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLdsSchemaClasses();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case4 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_SchemaAttributes()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateSchemaAttributes();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case5 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LdsSchemaAttributes()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLdsSchemaAttributes();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case6 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_SystemOnly()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateSystemOnly();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case7 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSSystemOnly()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSSystemOnly();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case8 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_UniqueID()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateUniqueID();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case9 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSUniqueID()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSUniqueID();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case10 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_Syntaxes()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateSyntaxes();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case11 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSSyntaxes()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSSyntaxes();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case12 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_IsSingleValued()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateIsSingleValued();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case13 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSIsSingleValued()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSIsSingleValued();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case14 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_ConsistencyRules()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateConsistencyRules();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case15 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSConsistencyRules()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSConsistencyRules();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case16 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_SchemaModifications()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateSchemaModifications();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }
        /// <summary>
        /// Test Case17 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSSchemaModifications()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSSchemaModifications();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case18 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        public void Schema_ConstructedAttributesValidation()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateConstructedAttributes();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }
        /// <summary>
        /// Test Case19 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSConstructedAttributesValidation()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSConstructedAttributes();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case20 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
        public void Schema_QueryNC()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateQueryNC();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case21 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSQueryNC()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSQueryNC();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case22 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_CrossRefContainer()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateCrossRefContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case23 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSCrossRefContainer()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSCrossRefContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case24 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_SitesContainer()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateSitesContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case25 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSSitesContainer()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSSitesContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case26 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        public void Schema_ServerContainer()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateServerContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case27 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSServerContainer()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSServerContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case28 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_ServiceAndQueryPolicyContainer()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateServiceAndQueryPolicyContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case29 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSServiceAndQueryPolicyContainer()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSServiceAndQueryPolicyContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case30 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_WellKnownSecurityPrincipal()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateWellKnownSecurityPrincipal();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case31 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
        public void Schema_ExtendedRights()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateExtendedRights();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case32 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSExtendedRights()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSExtendedRights();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case33 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_ForestUpdatesContainer()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateForestUpdatesContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case34 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_WellKnownObjects()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateWellKnownObjects();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }


        /// <summary>
        /// Test Case35 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSWellKnownObjects()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSWellKnownObjects();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case36 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSRoleContainer()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSRoleContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Test Case37 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_WellKnownSecurityDomainPrincipal()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateWellKnownSecurityDomainPrincipal();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }


        /// <summary>
        /// Test Case38 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSWellKnownSecurityDomainPrincipal()
        {
            if (adAdapter.RunLDSTestCases)
            {
                ValidateLDSWellKnownSecurityDomainPrincipal();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }
        /// <summary>
        /// Test Case39 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_ReadOnlyDomainController()
        {
            if (adAdapter.RunDSTestCases)
            {
                //validate RODC requirements
                ValidateReadOnlyDomainController();
            }
        }

        /// <summary>
        /// Test Case40 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
        public void Schema_FunctionalLevel()
        {
            //Validate functional requirements
            ValidateFunctionalLevel();
        }

        /// <summary>
        /// Test Case41 Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_DRSRRequirement()
        {
            //Validate DRSR requirments
            DRSRRequirementValidation();
        }


        /// <summary>
        /// Optional Feature Validation
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_OptionalFeature()
        {
            //Beginning from windows server 2008 R2, AD Schema supports Optional Feature
            if (serverOS >= OSVersion.WinSvr2008R2 && adAdapter.RunDSTestCases)
            {
                ValidateOptionalFeature();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// LDS Optional Feature Validation
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_LDSOptionalFeature()
        {
            //Beginning from windows server 2008 R2, AD Schema supports Optional Feature
            if (serverOS >= OSVersion.WinSvr2008R2 && adAdapter.RunLDSTestCases)
            {
                ValidateLDSOptionalFeature();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/LDS Test Cases are skipped from execution");
            }
        }

        /// <summary>
        /// Domain Updates Container Validation 
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-Schema")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void Schema_DomainUpdatesContainer()
        {
            if (adAdapter.RunDSTestCases)
            {
                ValidateDomainUpdatesContainer();
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "AD/DS Test Cases are skipped from execution");
            }
        }

        #endregion
    }
}




