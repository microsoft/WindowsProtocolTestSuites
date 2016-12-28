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
    /// This file is the source file for Validation of the TestCase14 and TestCase15.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region AD/DS ConsistencyRules validation

        /// <summary>
        /// This method validates the requirements under 
        /// ConsistencyRules Scenario.
        /// </summary>
        public void ValidateConsistencyRules()
        {
            #region MS-ADTS-Schema_R73
            //The objectClass attribute of the attributeSchema equals the sequence [top, attributeSchema ]
            //Expected Sequence setting...
            Sequence<string> expectedSeq = new Sequence<string>();
            Sequence<string> actualSeq = new Sequence<string>();
            DirectoryEntry serverObject;
            string requiredObjectDN = String.Empty;

            expectedSeq.Add("top");
            expectedSeq.Add("classSchema");

            //Get attributeSchema from Server.
            requiredObjectDN = "CN=Attribute-Schema,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            if (!adAdapter.GetObjectByDN(requiredObjectDN, out serverObject))
            {
                DataSchemaSite.Assume.IsTrue(false, requiredObjectDN + " Object is not found in server");
            }
            foreach (string valueString in serverObject.Properties[StandardNames.objectClass.ToLower()])
            {
                actualSeq.Add(valueString.ToLower());
            }

            //MS-ADTS-Schema_R73.
            DataSchemaSite.CaptureRequirementIfAreEqual<Sequence<string>>(
                expectedSeq,
                actualSeq,
                73,
                "The objectClass attribute of the attributeSchema equals the sequence [top, classSchema ].");

            #endregion

            #region MS-ADTS-Schema_R157
            //The objectClass attribute of the classSchema equals the sequence [top, classSchema ]. 
            //Get classSchema from Server.
            requiredObjectDN = "CN=Class-Schema,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            if (!adAdapter.GetObjectByDN(requiredObjectDN, out serverObject))
            {
                DataSchemaSite.Assume.IsTrue(false, requiredObjectDN + " Object is not found in server");
            }
            actualSeq = new Sequence<string>();
            foreach (string valueString in serverObject.Properties[StandardNames.objectClass.ToLower()])
            {
                actualSeq.Add(valueString.ToLower());
            }

            //MS-ADTS-Schema_R157.
            DataSchemaSite.CaptureRequirementIfAreEqual<Sequence<string>>(
                expectedSeq,
                actualSeq,
                157,
                "The objectClass attribute of the classSchema equals the sequence [top, classSchema ].");

            #endregion

            #region MS-ADTS-Schema_R128-133,136-142,179

            //Inheritance rule requirements
            IEnumerable<IObjectOnServer> serverObjects = adAdapter.GetAllSchemaClasses();
            if (serverObjects == null)
            {
                DataSchemaSite.Assume.IsNotNull(serverObjects, "Class objects are not existing in Server");
            }

            DataSchemaSite.Log.Add(LogEntryKind.Comment, "Begin ValidateInheritanceRequirements");
            //This method will validate the requirements MS-ADTS-Schema_R128-133,136-142,179.
            ValidateInheritanceRequirements(serverObjects,true);

            #endregion

            #region MS-ADTS-Schema_R143-146

            //Covering ObjectClass requirements 
            //Get domain NC for validation.
            DirectoryEntry domainEntry;
            if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out domainEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
            }
            DataSchemaSite.Log.Add(LogEntryKind.Comment, "Begin ValidateObjectClassRequirements");
            //This method validates teh requirements MS-ADTS-Schema_R143-146
            ValidateObjectClassRequirements(domainEntry.Children, true);

            #endregion

            #region MS-ADTS-Schema_R149,175
            //Coverring StructureRule requirements 
            DataSchemaSite.Log.Add(LogEntryKind.Comment, "Begin ValidateStructureRulesRequirements");
            //This method validates the requirements MS-ADTS-Schema_R149,175
            ValidateStructureRulesRequirements(serverObjects, true);

            #endregion

            #region MS-ADTS-Schema_R152,153
            //Covering ContentRule requirements 
            DataSchemaSite.Log.Add(LogEntryKind.Comment, "Begin ValidateContentRulesRequirements");
            //This method validates the requirements MS-ADTS-Schema_R152,153.
            ValidateContentRulesRequirements(domainEntry.Children, serverObjects);

            #endregion

            #region MS-ADTS-Schema_R177
            //The systemAuxiliaryClass attribute of the classSchema Specifies governsIDs of the classes that can 
            //be parents of the class within an NC tree, where the parent-child relationships are required for 
            //system operation.
            DirectoryEntry classSchemaObj;
            bool isAuxiliary = false;
            SetContainer<string> auxiliaryClassValue = new SetContainer<string>();

            //Get class-schema class from server.
            if (!adAdapter.GetObjectByDN("CN=Class-Schema,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN, out classSchemaObj))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Class-Schema,CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }

            //Collect its auxiliary class value.
            if (classSchemaObj.Properties.Contains("auxiliaryclass"))
            {
                //AuxiliaryClassValue.Add
                foreach (string value in classSchemaObj.Properties["auxiliaryclass"])
                {
                    auxiliaryClassValue.Add(value);
                }
            }

            //Collect its system auxiliary class value.
            if (classSchemaObj.Properties.Contains("systemauxiliaryclass"))
            {
                //AuxiliaryClassValue.Add
                foreach (string value in classSchemaObj.Properties["systemauxiliaryclass"])
                {
                    auxiliaryClassValue.Add(value);
                }
            }

            if (auxiliaryClassValue.Count != 0)
            {
                //For each auxiliary class...
                foreach (string auxClass in auxiliaryClassValue)
                {
                    isAuxiliary = false;
                    foreach (IObjectOnServer serverObj in serverObjects)
                    {
                        //Get it from server.
                        if (serverObj.Name.Equals(auxClass))
                        {
                            //Get server object governsID.
                            string governsID = (string)serverObj.Properties[StandardNames.governsId.ToLower()][0];

                            //It should be eqal to the auxiliary class name of the class.
                            if (governsID.Equals(auxClass))
                            {
                                isAuxiliary = true;
                                continue;
                            }
                        }
                    }
                    if (isAuxiliary)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //MS-ADTS-Schema_R177.
            DataSchemaSite.Log.Add(LogEntryKind.Comment, "SystemAuxiliaryClass TDI is resolved");

            #endregion
        }

        #endregion

        #region AD/LDS ConsistencyRules validation

        /// <summary>
        /// This method validates the requirements under 
        /// LDSConsistencyRules Scenario.
        /// </summary>
        public void ValidateLDSConsistencyRules()
        {
            #region MS-ADTS-Schema_R73

            //The objectClass attribute of the attributeSchema equals the sequence [top, attributeSchema ]
            //Expected Sequence setting...
            Sequence<object> actualSeq = new Sequence<object>();
            Sequence<object> expectedSeq = new Sequence<object>();
            ModelObject objectFromModel = null;
            string requiredObjectDN = String.Empty;
            DirectoryEntry serverObject;
            expectedSeq = expectedSeq.Add("top".ToLower()).Add("classSchema".ToLower());

            //Get attributeSchema from Server.
            requiredObjectDN = "CN=Attribute-Schema,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            if (!adAdapter.GetLdsObjectByDN(requiredObjectDN, out serverObject))
            {
                DataSchemaSite.Assume.IsTrue(false, requiredObjectDN + " Object is not found in server");
            }
            actualSeq = new Sequence<object>();
            foreach (string valueString in serverObject.Properties[StandardNames.objectClass.ToLower()])
            {
                actualSeq = actualSeq.Add(valueString.ToLower());
            }
            //MS-ADTS-Schema_R73.
            DataSchemaSite.CaptureRequirementIfAreEqual<Sequence<object>>(
                expectedSeq,
                actualSeq,
                73,
                "The objectClass attribute of the attributeSchema equals the sequence [top, classSchema ].");

            #endregion

            #region MS-ADTS-Schema_R157
            //The objectClass attribute of the classSchema equals the sequence [top, classSchema ]. 
            //Expected Sequence setting...
            expectedSeq = new Sequence<object>();
            expectedSeq = expectedSeq.Add("top".ToLower()).Add("classSchema".ToLower());

            //Get classSchema from Server.
            requiredObjectDN = "CN=Class-Schema,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            if (!adAdapter.GetLdsObjectByDN(requiredObjectDN, out serverObject))
            {
                DataSchemaSite.Assume.IsTrue(false, requiredObjectDN + " Object is not found in server");
            }
            actualSeq = new Sequence<object>();
            foreach (string valueString in serverObject.Properties[StandardNames.objectClass.ToLower()])
            {
                actualSeq = actualSeq.Add(valueString.ToLower());
            }

            //MS-ADTS-Schema_R157.
            DataSchemaSite.CaptureRequirementIfAreEqual<Sequence<object>>(
                expectedSeq,
                actualSeq,
                157,
                "The objectClass attribute of the classSchema equals the sequence [top, classSchema ].");

            #endregion

            #region MS-ADTS-Schema_R169

            //The subClassOf attribute of the classSchema Specifies governsID of the superclass of the class.
            //Get classSchema from Model.
            objectFromModel = adamModel.GetClass("classSchema");
            if (objectFromModel == null)
            {
                DataSchemaSite.Assume.IsNotNull(objectFromModel, "Class classSchema is not existing in Model");
            }
            string expectedValue = (string)objectFromModel[StandardNames.subClassOf].UnderlyingValues.ToArray()[0];

            #endregion

            #region MS-ADTS-Schema_R129-142,179

            //Inheritance requirements
            IEnumerable<IObjectOnServer> serverObjects = (IEnumerable<IObjectOnServer>)adAdapter.GetAllLdsSchemaClasses();
            if (serverObjects == null)
            {
                DataSchemaSite.Assume.IsNotNull(serverObjects, "Class objects are not existing in Model");
            }

            //This method will validate the requirements MS-ADTS-Schema_R129-142,179
            ValidateInheritanceRequirements(serverObjects,false);

            serverObjects = null;

            #endregion

            #region MS-ADTS-Schema_R143-146

            //Covering ObjectClass requirements 
            //Get domain NC for validation.
            DirectoryEntry domainEntry;
            if (!adAdapter.GetLdsObjectByDN("CN=Configuration," + adAdapter.LDSRootObjectName, out domainEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }

            //This method validates teh requirements MS-ADTS-Schema_R143-146
            ValidateObjectClassRequirements(domainEntry.Children, false);
            domainEntry = null;

            #endregion

            #region MS-ADTS-Schema_R149,175

            //Coverring StructureRule requirements 
            serverObjects = adAdapter.GetAllLdsSchemaClasses();
            if (serverObjects == null)
            {
                DataSchemaSite.Assume.IsNotNull(serverObjects, "Class objects are not existing in Model");
            }

            //This method validates the requirements MS-ADTS-Schema_R149,175
            ValidateStructureRulesRequirements(serverObjects, false);

            #endregion

            #region MS-ADTS-Schema_R152,153
            //Covering ContentRule requirements 

            //Get the domain NC objects.
            if (!adAdapter.GetLdsObjectByDN("CN=Configuration," + adAdapter.LDSRootObjectName, out domainEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Configuration Object is not found in server");
            }

            //This method validates the requirements MS-ADTS-Schema_R152,153.
            ValidateContentRulesRequirements(domainEntry.Children, serverObjects);

            #endregion

            #region MS-ADTS-Schema_R177
            //The systemAuxiliaryClass attribute of the classSchema Specifies governsIDs of the classes that can 
            //be parents of the class within an NC tree, where the parent-child relationships are required for 
            //system operation.

            DirectoryEntry classSchemaObj;
            bool isAuxiliary = false;
            SetContainer<string> auxiliaryClassValue = new SetContainer<string>();

            //Get class-schema class from server.
            if (!adAdapter.GetLdsObjectByDN(
                "CN=Class-Schema,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName,
                out classSchemaObj))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Class-Schema,CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }

            //Collect its auxiliary class value.
            if (classSchemaObj.Properties.Contains("auxiliaryclass"))
            {
                //auxiliaryClassValue.Add
                foreach (string value in classSchemaObj.Properties["auxiliaryclass"])
                {
                    auxiliaryClassValue.Add(value);
                }
            }

            //Collect its system auxiliary class value.
            if (classSchemaObj.Properties.Contains("systemauxiliaryclass"))
            {
                //AuxiliaryClassValue.Add
                foreach (string value in classSchemaObj.Properties["systemauxiliaryclass"])
                {
                    auxiliaryClassValue.Add(value);
                }
            }

            if (auxiliaryClassValue.Count != 0)
            {
                //For each auxiliary class...
                foreach (string auxClass in auxiliaryClassValue)
                {
                    isAuxiliary = false;
                    foreach (IObjectOnServer serverObj in serverObjects)
                    {
                        //Get it from server.
                        if (serverObj.Name.Equals(auxClass))
                        {
                            //Get server object governsID.
                            string governsID = (string)serverObj.Properties[StandardNames.governsId.ToLower()][0];

                            //It should be eqal to the auxiliary class name of the class.
                            if (governsID.Equals(auxClass))
                            {
                                isAuxiliary = true;
                                continue;
                            }
                        }
                    }
                    if (isAuxiliary)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            #endregion
        }

        #endregion

        public void ValidateInheritanceRequirements(IEnumerable<IObjectOnServer> serverObjects, bool isADDS = true)
        {
            bool categoryOk = false;

            //For each server object
            foreach (IObjectOnServer serverObj in serverObjects)
            {
                string superClassName = (string)serverObj.Properties[StandardNames.subClassOf.ToLower()][0];

                #region AbstractEx

                //MS-ADTS-Schema_R129.
                if (serverObj.Name.ToLower().Equals("applicationsettings"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("top"),
                        129,
                        @"applicationSettings class (objectClassCategory: 2) is an abstract class,"
                        + " is inherited from top class (objectClassCategory: 2) which is also an abstract class.");
                    continue;
                }

                //MS-ADTS-Schema_R130.
                if (serverObj.Name.ToLower().Equals("connectionpoint"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("leaf"),
                        130,
                        @"connectionPoint class (objectClassCategory: 2) is an abstract class, is"
                        + " inherited from leaf class (objectClassCategory: 2) which is also an abstract class.");
                    continue;
                }

                //MS-ADTS-Schema_R131.
                if (serverObj.Name.ToLower().Equals("leaf"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("top"),
                        131,
                        @"leaf class (objectClassCategory: 2) is an abstract class, is inherited"
                        + " from top class (objectClassCategory: 2) which is also an abstract class.");
                    continue;
                }

                #endregion

                #region AuxEx

                //MS-ADTS-Schema_R133.
                if (serverObj.Name.ToLower().Equals("domainrelatedobject"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("top"),
                        133,
                        @"domainRelatedObject class (objectClassCategory: 3), bootableDevice class 
                        (objectClassCategory: 3) are auxiliary classes, which are inherited from top class 
                        (objectClassCategory: 2) which is an abstract class.");
                    continue;
                }

                //MS-ADTS-Schema_R133.
                if (serverObj.Name.ToLower().Equals("bootabledevice"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("top"),
                        133,
                        @"domainRelatedObject class (objectClassCategory: 3), bootableDevice class 
                        (objectClassCategory: 3) are auxiliary classes, which are inherited from top class 
                        (objectClassCategory: 2) which is an abstract class.");
                    continue;
                }

                //MS-ADTS-Schema_R134.
                if (serverObj.Name.ToLower().Equals("msds-bindableobject"))
                {
                    if (superClassName.ToLower().Equals("securityprincipal"))
                    {
                        superClassName = "Security-Principal";
                    }
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("securityprincipal"),
                        134,
                        @"msDS-BindableObject class (objectClassCategory: 3), is an auxiliary class, is inherited from 
                        securityPrincipal class (objectClassCategory: 3) which is an auxiliary class.");
                    continue;
                }

                #endregion

                #region StructEx

                //MS-ADTS-Schema_R136.
                if (serverObj.Name.ToLower().Equals("categoryregistration"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("leaf"),
                        136,
                        @"categoryRegistration class (objectClassCategory: 1) is a structural class,"
                        + " is inherited from leaf class (objectClassCategory: 2) which is an abstract class.");
                    continue;
                }

                //MS-ADTS-Schema_R137.
                if (serverObj.Name.ToLower().Equals("computer"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("user"),
                        137,
                        @"computer class (objectClassCategory: 1) is a structural class, is inherited from user class"
                        + " (objectClassCategory: 1) which is also a structural class.");
                    continue;
                }

                //MS-ADTS-Schema_R138.
                if (serverObj.Name.ToLower().Equals("msds-quotacontainer"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("top"),
                        138,
                        @"msDS-QuotaContainer class (objectClassCategory: 1) is a structural class,"
                        + " is inherited from top class (objectClassCategory: 2) which is an abstract class.");
                    continue;
                }

                #endregion

                #region 88ClassEx

                //MS-ADTS-Schema_R140.
                if (serverObj.Name.ToLower().Equals("organizationalperson"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("person"),
                        140,
                        @"organizationalPerson class (objectClassCategory: 0) is a 88 class, is inherited from "
                        + "person class (objectClassCategory: 0) which is also a 88 class.");
                    continue;
                }

                //MS-ADTS-Schema_R141.
                if (serverObj.Name.ToLower().Equals("person"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("top"),
                        141,
                        @"person class (objectClassCategory: 0) is an 88 class, is inherited from top class "
                        + "(objectClassCategory: 2) which is an abstract class.");
                    continue;
                }

                //MS-ADTS-Schema_R142.
                if (serverObj.Name.ToLower().Equals("groupofnames"))
                {
                    //Find super class.
                    DirectoryEntry superClassObj = null;
                    if (isADDS)
                    {
                        adAdapter.GetObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.rootDomainDN), out superClassObj);
                    }
                    else
                    {
                        adAdapter.GetLdsObjectByDN(string.Format("CN={0},CN=Schema,CN=Configuration,{1}", superClassName, adAdapter.LDSRootObjectName), out superClassObj);
                    }
                    string superClassObjName = superClassObj.Properties["lDAPDisplayName"].Value.ToString().ToLower();
                    superClassObj.Close();
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        superClassObjName.Equals("top"),
                        142,
                        @"groupOfNames class (objectClassCategory: 0) is an 88 class, is inherited from top class"
                        + " (objectClassCategory: 2) which is an abstract class.");
                    continue;
                }

                #endregion

                #region MS-ADTS-Schema_R179

                if (serverObj.Name.ToLower().Equals("classschema"))
                {
                    int category = int.Parse(serverObj.Properties["objectclasscategory"][0].ToString());
                    if (category >= 0 && category <= 3)
                    {
                        categoryOk = true;
                    }
                    //MS-ADTS-Schema_R179.
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        categoryOk,
                        179,
                        "The objectClassCategory attribute of the classSchema object is any of the following. "
                    + "0: 88 Class; 1: Structural class; 2: Abstract class; 3: Auxiliary class.");
                    continue;
                }

                #endregion
            }

        }

        public void ValidateObjectClassRequirements(DirectoryEntries childrens, bool isADDS)
        {
            bool isObjectClassMultiValued = false;
            bool isObjectClassAppeared = false;
            bool isObjectClassTop = false;
            bool isObjectClassStructural = false;

            foreach (DirectoryEntry child in childrens)
            {
                //Get objectClass value.
                PropertyValueCollection propValColln = child.Properties[StandardNames.objectClass];

                //If it is not existing Requirement 130 will fail.
                if (propValColln != null)
                {
                    isObjectClassAppeared = true;
                }

                //It is multivalued. so it should be more than one value.
                if (propValColln.Count >= 1)
                {
                    isObjectClassMultiValued = true;
                }

                //Get objectClassValue in array.
                string[] objectClassElements = new string[propValColln.Count];
                int loopVariable = 0;
                foreach (string propVal in (IEnumerable<object>)propValColln.Value)
                {
                    objectClassElements[loopVariable++] = propVal;
                }

                //The first element is always top.
                if (objectClassElements[0].ToLower().Equals("top"))
                {
                    isObjectClassTop = true;
                }

                //Get last element.
                string structuralClassName = (string)child.Properties["objectCategory"].Value;
                DirectoryEntry lastClassObject;
                if (isADDS)
                {
                    if (!adAdapter.GetObjectByDN(structuralClassName, out lastClassObject))
                    {
                        DataSchemaSite.Assume.IsTrue(false, structuralClassName + " Object is not found in server");
                    }
                }
                else
                {
                    if (!adAdapter.GetLdsObjectByDN(structuralClassName, out lastClassObject))
                    {
                        DataSchemaSite.Assume.IsTrue(false, structuralClassName + " Object is not found in server");
                    }
                }
                ObjectClassCategory lastClassCategory = (ObjectClassCategory)(int)lastClassObject.
                    Properties[StandardNames.objectClassCategory.ToLower()][0];

                //The last element should be structural class.
                if (lastClassCategory == ObjectClassCategory.StructuralClass)
                {
                    isObjectClassStructural = true;
                }

                if (isObjectClassAppeared && isObjectClassMultiValued && isObjectClassStructural && isObjectClassTop)
                    continue;
                else
                    break;

            }
            //MS-ADTS-Schema_R143.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassMultiValued
                && isObjectClassAppeared,
                143,
                "Attribute objectClass is a multivalued attribute that appears on all the objects in the directory.");

            //MS-ADTS-Schema_R144.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassMultiValued
                && isObjectClassAppeared,
                144,
                @"When instantiating a structural class, the objectClass attribute of the new object contains a 
                sequence of class names");

            //MS-ADTS-Schema_R145.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassTop,
                145,
                "In the ObjectClass first element is always class top.");

            //MS-ADTS-Schema_R146.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassStructural,
                146,
                "In the ObjectClass last element is the name of the structural class that was instantiated.");
        }

        public void ValidateStructureRulesRequirements(IEnumerable<IObjectOnServer> serverObjects, bool isADDS)
        {
            bool isAllowedParent = false;
            List<string> sampleClasses = new List<string>();
            sampleClasses.Add("organization");
            sampleClasses.Add("attributeSchema");
            sampleClasses.Add("account");
            sampleClasses.Add("classSchema");
            foreach (IObjectOnServer serverObj in serverObjects)
            {
                if (sampleClasses.Contains(serverObj.Name))
                {
                    //Get possSuperior value
                    Sequence<string> possSuperiors = new Sequence<string>();
                    if (serverObj.Properties.ContainsKey(StandardNames.possSuperiors.ToLower()))
                    {
                        foreach (string element in serverObj.Properties[StandardNames.possSuperiors.ToLower()])
                        {
                            possSuperiors = possSuperiors.Add(element);
                        }
                    }
                    if (serverObj.Properties.ContainsKey(StandardNames.systemPossSuperiors.ToLower()))
                    {
                        foreach (string element in serverObj.Properties[StandardNames.systemPossSuperiors.ToLower()])
                        {
                            possSuperiors = possSuperiors.Add(element);
                        }
                    }

                    //For each possSuperior.
                    foreach (string possSuperior in possSuperiors)
                    {
                        isAllowedParent = false;

                        //Get distinguishedName of this possSuperior from model to get this object from server.
                        ModelObject possSuperiorObject;
                        if (isADDS)
                        {
                            possSuperiorObject = dcModel.GetClass(possSuperior);
                        }
                        else
                        {
                            possSuperiorObject = adamModel.GetClass(possSuperior);
                        }
                        if (possSuperiorObject == null)
                        {
                            DataSchemaSite.Assume.IsNotNull(
                                possSuperiorObject,
                                "Class "
                                + possSuperior
                                + " is not existing in Model");
                        }
                        string possSuperiorDN = (string)possSuperiorObject[StandardNames.distinguishedName].UnderlyingValues[0];

                        //Get this possSuperior from Server
                        DirectoryEntry possSuperiorServerObject;
                        if (isADDS)
                        {
                            if (!adAdapter.GetObjectByDN(possSuperiorDN, out possSuperiorServerObject))
                            {
                                DataSchemaSite.Assume.IsTrue(false, possSuperiorDN + " Object is not found in server");
                            }
                        }
                        else
                        {
                            if (!adAdapter.GetLdsObjectByDN(possSuperiorDN, out possSuperiorServerObject))
                            {
                                DataSchemaSite.Assume.IsTrue(false, possSuperiorDN + " Object is not found in server");
                            }
                        }

                        //Get possibleInfeiror value of possSuperiorClass.
                        possSuperiorServerObject.RefreshCache(new string[] { "possibleinferiors" });
                        PropertyValueCollection possInferiorVal = possSuperiorServerObject.
                            Properties[StandardNames.possibleInferiors.ToLower()];

                        //In all possSuperior object, this class name should exist in its possibleInferiors.
                        foreach (string possInfe in possInferiorVal)
                        {
                            if (possInfe.Equals(serverObj.Name))
                            {
                                isAllowedParent = true;
                                break;
                            }
                        }
                        if (!isAllowedParent)
                        {
                            break;
                        }
                    }
                    if (!isAllowedParent)
                        break;
                }
            }
            //MS-ADTS-Schema_R149.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isAllowedParent,
                149,
                "The union of values in possSuperiors and systemPossSuperiors specifies the classes that "
                + "are allowed to be parents of an object instance of the class in question.");
        }

        public void ValidateContentRulesRequirements(DirectoryEntries childrens, IEnumerable<IObjectOnServer> serverObjects)
        {
            bool isContent = true;

            //Setting some excluded attributes.
            SetContainer<string> ExcludedAttributes = new SetContainer<string>();
            ExcludedAttributes.Add("creationTime");
            ExcludedAttributes.Add("forceLogoff");
            ExcludedAttributes.Add("lockoutDuration");
            ExcludedAttributes.Add("lockOutObservationWindow");
            ExcludedAttributes.Add("lockoutThreshold");
            ExcludedAttributes.Add("maxPwdAge");
            ExcludedAttributes.Add("minPwdAge");
            ExcludedAttributes.Add("minPwdLength");
            ExcludedAttributes.Add("modifiedCountAtLastProm");
            ExcludedAttributes.Add("nextRid");
            ExcludedAttributes.Add("pwdProperties");
            ExcludedAttributes.Add("pwdHistoryLength");
            ExcludedAttributes.Add("objectSid");
            ExcludedAttributes.Add("serverState");
            ExcludedAttributes.Add("uASCompat");
            ExcludedAttributes.Add("modifiedCount");


            //For each domain NC object...
            foreach (DirectoryEntry entry in childrens)
            {
                SetContainer<string> mustContain = new SetContainer<string>();
                SetContainer<string> mayContain = new SetContainer<string>();

                //Get super class chain.
                object[] superClasses = (object[])entry.Properties[StandardNames.objectClass.ToLower()].Value;

                //For each super class...
                foreach (string superClass in superClasses)
                {
                    foreach (IObjectOnServer serverObj in serverObjects)
                    {
                        //Get the object from server.
                        if (serverObj.Name.StartsWith(superClass))
                        {
                            //Collect all must and may contain attribute value.
                            if (serverObj.Properties.ContainsKey(StandardNames.mustContain.ToLower()))
                            {
                                foreach (object value in serverObj.Properties[StandardNames.mustContain.ToLower()])
                                {
                                    mustContain.Add((string)value);
                                }
                            }
                            if (serverObj.Properties.ContainsKey(StandardNames.systemMustContain.ToLower()))
                            {
                                foreach (object value in serverObj.Properties[StandardNames.systemMustContain.ToLower()])
                                {
                                    mustContain.Add((string)value);
                                }
                            }

                            if (serverObj.Properties.ContainsKey(StandardNames.mayContain.ToLower()))
                            {
                                foreach (object value in serverObj.Properties[StandardNames.mayContain.ToLower()])
                                {
                                    mayContain.Add((string)value);
                                }
                            }
                            if (serverObj.Properties.ContainsKey(StandardNames.systemMayContain.ToLower()))
                            {
                                foreach (object value in serverObj.Properties[StandardNames.systemMayContain.ToLower()])
                                {
                                    mayContain.Add((string)value);
                                }
                            }
                            break;
                        }
                    }
                }

                //For all property name of this object, it should be present in either must or may contain 
                //attribute list. 
                foreach (string attribute in entry.Properties.PropertyNames)
                {
                    if (!ExcludedAttributes.Contains(attribute))
                    {
                        //This attribute is in must contain list.
                        if (mustContain.Contains(attribute))
                            mustContain.Remove(attribute);
                        //This attribute is not in must and may contian list.
                        else if (!mayContain.Contains(attribute))
                        {
                            isContent = false;
                            break;
                        }
                    }
                }

                //This is for checking whether some must contain attributes are missed.
                if (mustContain.Count > 0)
                {
                    isContent = false;
                }

                if (!isContent)
                {
                    break;
                }
            }
            //MS-ADTS-Schema_R152.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isContent,
                152,
                @"In the content rules union of values in the mustContain and systemMustContain attributes specifies the 
                attributes that are required to be present on an object instance of the class in question.");

            //MS-ADTS-Schema_R153.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isContent,
                153,
                @"In the content rules  the union of values in the mustContain, systemMustContain, mayContain, and 
                systemMayContain attributes specifies the attributes that are allowed to be present on an object 
                instance of the class in question.");
        }
    }
}
