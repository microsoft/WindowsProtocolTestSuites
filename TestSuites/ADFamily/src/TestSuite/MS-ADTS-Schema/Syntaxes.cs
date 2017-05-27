// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ActiveDs;
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
    /// This file is the source file for Validation of the TestCase10 and TestCase11.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        //Variables for representing the context and object of the attributes.
        AttributeContext attrContext;
        ModelObject attributeObject, classObject;

        //Variables for each syntax.
        bool integer = false, boolean = false, enumeration = false, largeInt = false, accessPoint = false,
            DNString = false, ORName = false, DNBinary = false, DSDN = false, preAddr = false,
            repLink = false, strCase = false, strIA5 = false, NTSecDesc = false, numeric = false,
            objIdentifier = false, octet = false, printable = false, sid = false, teletext = false,
            unicode = false, UTCTime = false, genTime = false;

        //Variables for particular attributes validation.
        bool intRange = false, larIntRange = false, rangeLower32 = false, rangeUpper32 = false;
        bool extCharsAllow = false, rDNSyntax = false, attrType = false;

        //To store the values for attributes AttributeSyntax, OMSyntax, OMObjectClass.
        Value serverValueAttrSyntax = null, serverValueOMSyntax = null, serverValueOMObjectClass = null;

        //Variable for verifying the count of Hex value obtained by binary value of wellknownobject.
        bool hexCount = false;

        //Variable for verifying the format of Object(DN-Binary).
        bool binaryFormat = false;

        //Variable for storing the converted byte to hex value.
        string byteToHex = null;

        #region Syntaxes 

        /// <summary>
        /// This method validates the requirements under 
        /// Syntaxes Scenario
        /// </summary>
        public void ValidateSyntaxes()
        {
            isDS = true;
            foreach (IObjectOnServer serverObject in adAdapter.GetAllSchemaAttributes())
            {
                if (!dcModel.TryGetAttribute(serverObject.Name, out attributeObject))
                {
                     DataSchemaSite.Log.Add(
                         LogEntryKind.Warning, 
                         "schema attribute '{0}'"
                         + " exists on server but not in model", 
                         serverObject.Name);
                     continue;
                }
                //Syntaxes for these attributes are not defined in the ADTS document.
                if (serverObject.Name == "presentationAddress" 
                    || serverObject.Name == "repsFrom" 
                    || serverObject.Name == "repsTo")
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "Syntax for '{0}' is not defined",
                        serverObject.Name);
                    continue;
                }
                dcModel.TryGetAttributeContext(serverObject.Name, out attrContext);
                foreach (string attr in attributeObject.attributes.Keys)
                {
                    AttributeContext modelAttrContext;
                    Value values = attributeObject[attr];
                    dcModel.TryGetAttributeContext(attr, out modelAttrContext);
                    //Verifying the attributes for IntegerSyntax.
                    if (modelAttrContext.syntax.Name.ToString() == "IntegerSyntax")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int32"))
                        {
                            intRange = true;
                        }
                    }
                    //Verifying the attributes for LargeIntegerSyntax.
                    if (modelAttrContext.syntax.Name.ToString() == "LargeIntegerSyntax")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int64"))
                        {
                            larIntRange = true;
                        }
                    }
                    if (attr == "rangelower")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int32"))
                        {
                            rangeLower32 = true;
                        }

                    }
                    if (attr == "rangeupper")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int32"))
                        {
                            rangeUpper32 = true;
                        }
                    }
                }
                serverValueAttrSyntax = new Value(attrContext.syntax, 
                    serverObject.Properties[StandardNames.attributeSyntax.Trim().ToLower()]);
                serverValueOMSyntax = new Value(attrContext.syntax,
                    serverObject.Properties[StandardNames.oMSyntax.Trim().ToLower()]);
                serverValueOMObjectClass = new Value(attrContext.syntax,
                    serverObject.Properties[StandardNames.oMObjectClass.Trim().ToLower()]);
                VerifySyntaxes(attrContext, serverObject);
            }
            //Verifying the attribute rDNAttID in all the schema classes.
            foreach (IObjectOnServer serverObject in adAdapter.GetAllSchemaClasses())
            {
                if (!dcModel.TryGetClass(serverObject.Name, out classObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "schema class '{0}' exists on server"
                        + " but not in model", 
                        serverObject.Name);
                    continue;
                }
                Value values = classObject["rDNAttID"];
                if (classObject.attributes.Keys.Contains("rdnattid"))
                {
                    Array attributeType = values.UnderlyingValues.ToArray();
                    foreach (string rdnAttr in attributeType)
                    {
                        dcModel.TryGetAttributeContext(rdnAttr, out attrContext);
                        if (attrContext.syntax.Name.ToString() != "StringUnicodeSyntax")
                        {
                            rDNSyntax = true;
                            attrType = true;
                        }
                    }
                }
            }
            //Directory Entry for the domain NC.
            DirectoryEntry dirEntry;
            if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
            }
            //wellKnownObjects for domain NC and config NC.
            Object wKnownDomainObjects = dirEntry.Properties["wellKnownObjects"].Value;

            // GUID value of well-known container's.
            foreach (IADsDNWithBinary wkObjects in (IEnumerable)wKnownDomainObjects)
            {
                byte[] bytes = (byte[])wkObjects.BinaryValue;

                //Each byte in Object(DN-Binary) syntax is represented by a pair of hexadecimal characters in binary_value.
                byteToHex = BitConverter.ToString(bytes);

                byteToHex = byteToHex.Replace("-", String.Empty);

                //Format of the Object(DN-Binary) syntax. 
                string wellKnownObjectFormat = "B:" + byteToHex.Length + ":" + byteToHex + ":" + wkObjects.DNString;
                if (wkObjects == null)
                {
                     binaryFormat = true;
                }

                if (byteToHex.Length != 32)
                {
                    hexCount = true;
                }
            }
            SyntaxRequirements();
        }

        #endregion

        #region LDSSyntaxes Validation.

        /// <summary>
        /// This method validates the requirements under 
        /// LDSSyntaxes Scenario.
        /// </summary>
        public void ValidateLDSSyntaxes()
        {
            isDS = false;
              foreach (IObjectOnServer serverObject in adAdapter.GetAllLdsSchemaAttributes())
            {
                if (!adamModel.TryGetAttribute(serverObject.Name, out attributeObject))
                {
                    DataSchemaSite.Log.Add(LogEntryKind.Warning,
                        "schema attribute '{0}' exists on server but not in model", serverObject.Name);
                    continue;
                }
                //Syntaxes for these attributes are not defined in the ADTS document.
                if (
                    serverObject.Name == "presentationAddress" 
                    || serverObject.Name == "repsFrom" 
                    || serverObject.Name == "repsTo")
                {
                    DataSchemaSite.Log.Add(LogEntryKind.Warning, "Syntax for '{0}' is not defined", serverObject.Name);
                    continue;
                }
                adamModel.TryGetAttributeContext(serverObject.Name, out attrContext);
                foreach (string attr in attributeObject.attributes.Keys)
                {
                    AttributeContext modelAttrContext;
                    Value values = attributeObject[attr];
                    adamModel.TryGetAttributeContext(attr, out modelAttrContext);
                    //Verifying the attributes for IntegerSyntax.
                    if (modelAttrContext.syntax.Name.ToString() == "IntegerSyntax")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int32"))
                        {
                            intRange = true;
                        }
                    }
                    //Verifying the attributes for LargeIntegerSyntax.
                    if (modelAttrContext.syntax.Name.ToString() == "LargeIntegerSyntax")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int64"))
                        {
                            larIntRange = true;
                        }
                    }
                    if (attr == "rangelower")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int32"))
                        {
                            rangeLower32 = true;
                        }

                    }
                    if (attr == "rangeupper")
                    {
                        string str = values.UnderlyingValues[0].GetType().ToString();
                        if (!str.Equals("System.Int32"))
                        {
                            rangeUpper32 = true;
                        }
                    }
                }
                serverValueAttrSyntax = new Value(attrContext.syntax,
                    serverObject.Properties[StandardNames.attributeSyntax.Trim().ToLower()]);
                serverValueOMSyntax = new Value(attrContext.syntax,
                    serverObject.Properties[StandardNames.oMSyntax.Trim().ToLower()]);
                serverValueOMObjectClass = new Value(attrContext.syntax,
                    serverObject.Properties[StandardNames.oMObjectClass.Trim().ToLower()]);
                VerifySyntaxes(attrContext, serverObject);
            }
            
            //Verifying the attribute rDNAttID in all the schema classes.
            foreach (IObjectOnServer serverObject in adAdapter.GetAllLdsSchemaClasses())
            {
                if (!adamModel.TryGetClass(serverObject.Name, out classObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "schema class '{0}' exists on server but not in model", 
                        serverObject.Name);
                    continue;
                }
                Value values = classObject["rDNAttID"];
                if (classObject.attributes.Keys.Contains("rdnattid"))
                {
                    Array attributeType = values.UnderlyingValues.ToArray();
                    foreach (string rdnAttr in attributeType)
                    {
                        adamModel.TryGetAttributeContext(rdnAttr, out attrContext);
                        if (attrContext.syntax.Name.ToString() != "StringUnicodeSyntax")
                        {
                            rDNSyntax = true;
                            attrType = true;
                        }
                    }
                }
            }

            //Directory Entry for the domain NC.
            DirectoryEntry dirEntry;
            if (!adAdapter.GetLdsObjectByDN("CN = Configuration," + adAdapter.LDSRootObjectName, out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false, 
                    "CN = Configuration," 
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            //WellKnownObjects for domain NC and config NC.
            Object wKnownDomainObjects = dirEntry.Properties["wellKnownObjects"].Value;

            //.GUID value of well-known container's.
            foreach (IADsDNWithBinary wkObjects in (IEnumerable)wKnownDomainObjects)
            {
                byte[] bytes = (byte[])wkObjects.BinaryValue;

                //Each byte in Object(DN-Binary) syntax is represented by a pair of hexadecimal characters in binary_value.
                byteToHex = BitConverter.ToString(bytes);

                byteToHex = byteToHex.Replace("-", String.Empty);

                //Format of the Object(DN-Binary) syntax. 
                string wellKnownObjectFormat = "B:" + byteToHex.Length + ":" + byteToHex + ":" + wkObjects.DNString;
                if (wkObjects == null)
                {
                     binaryFormat = true;
                }

                if (byteToHex.Length != 32)
                {
                    hexCount = true;
                }
            }
            SyntaxRequirements();
       }

        #endregion

        #region SyntaxVerification

        private void VerifySyntaxes(AttributeContext attrContext, IObjectOnServer serverObject)
        {
            switch(attrContext.syntax.Name.ToString())
            {
                //Verifying the attributes for IntegerSyntax.
                case "IntegerSyntax":
                    if (
                        serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.9" 
                        || serverValueOMSyntax.UnderlyingValues[0].ToString() != "2")
                    {
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning, 
                            "Syntax for '{0}' is not as expected",
                            serverObject.Name);
                        integer = true;
                    }
                    break;
                //Verifying the attributes for BooleanSyntax.
                case "BooleanSyntax":
                    if (
                        serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.8" 
                        || serverValueOMSyntax.UnderlyingValues[0].ToString() != "1")
                    {
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning, 
                            "Syntax for '{0}' is not as expected",
                            serverObject.Name);
                        boolean = true;
                    }
                    break;
                //Verifying the attributes for EnumerationSyntax.
                case "EnumerationSyntax":
                    if (
                        serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.9" 
                        || serverValueOMSyntax.UnderlyingValues[0].ToString() != "10")
                    {
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning, 
                            "Syntax for '{0}' is not as expected",
                            serverObject.Name);
                        enumeration = true;
                    }
                    break;
                    //Verifying the attributes for LargeIntegerSyntax.
                case "LargeIntegerSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.16"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "65")
                        {
                            DataSchemaSite.Log.Add(LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected", serverObject.Name);
                            largeInt = true;
                        }
                    break;
                    //Verifying the attributes for ObjectAccessPointSyntax.
                case "ObjectAccessPointSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.14"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "127"
                            || serverValueOMObjectClass.UnderlyingValues[0].ToString() != "1.3.12.2.1011.28.0.702")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            accessPoint = true;
                        }
                        break;
                    //Verifying the attributes for ObjectDNStringSyntax.
                case "ObjectDNStringSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.14"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "127"
                            || serverValueOMObjectClass.UnderlyingValues[0].ToString() != "1.2.840.113556.1.1.1.12")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            DNString = true;
                        }
                        break;
                    //Verifying the attributes for ObjectORNameSyntax.
                case "ObjectORNameSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.7"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "127"
                            || serverValueOMObjectClass.UnderlyingValues[0].ToString() != "2.6.6.1.2.5.11.29")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            ORName = true;
                        }
                        break;
                    //Verifying the attributes for ObjectDNBinarySyntax.
                case "ObjectDNBinarySyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.7"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "127"
                            || serverValueOMObjectClass.UnderlyingValues[0].ToString() != "1.2.840.113556.1.1.1.11")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            DNBinary = true;
                        }
                        break;
                    //Verifying the attributes for ObjectDSDNSyntax.
                case "ObjectDSDNSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.1"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "127"
                            || serverValueOMObjectClass.UnderlyingValues[0].ToString() != "1.3.12.2.1011.28.0.714")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            DSDN = true;
                        }
                        break;
                    //Verifying the attributes for ObjectPresentationAddressSyntax.
                case "ObjectPresentationAddressSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.13"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "127"
                            || serverValueOMObjectClass.UnderlyingValues[0].ToString() != "1.3.12.2.1011.28.0.732")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            preAddr = true;
                        }
                        break;
                    //Verifying the attributes for ObjectReplicaLinkSyntax.
                case "ObjectReplicaLinkSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.10"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "127" 
                            || serverValueOMObjectClass.UnderlyingValues[0].ToString() != "1.2.840.113556.1.1.1.6")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            repLink = true;
                        }
                        break;
                    //Verifying the attributes for StringCaseSyntax.
                case "StringCaseSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.3"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "27")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            strCase = true;
                        }
                        break;
                    //Verifying the attributes for StringIA5Syntax.
                case "StringIA5Syntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.5"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "22")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            strIA5 = true;
                        }
                        break;
                    //Verifying the attributes for StringNTSecDescSyntax.
                case "StringNTSecDescSyntax":
                        if (
                            serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.15"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "66")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            NTSecDesc = true;
                        }
                        break;
                    //Verifying the attributes for StringNumericSyntax.
                case "StringNumericSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.6"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "18")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            numeric = true;
                        }
                        break;
                    //Verifying the attributes for StringObjectIdentifierSyntax.
                case "StringObjectIdentifierSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.2"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "6")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            objIdentifier = true;
                        }
                        break;
                    //Verifying the attributes for StringOctetSyntax.
                case "StringOctetSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.10"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "4")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            octet = true;
                        }
                        break;
                        //Verifying the attributes for StringPrintableSyntax.
                case "StringPrintableSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.5"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "19")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            printable = true;
                        }
                        break;
                    //Verifying the attributes for StringSidSyntax.
                case "StringSidSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.17"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "4")
                        {
                            DataSchemaSite.Log.Add(LogEntryKind.Warning, "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            sid = true;
                        }
                        break;
                    //Verifying the attributes for StringTeletexSyntax.
                case "StringTeletexSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.4"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "20")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            teletext = true;
                        }
                        break;
                    //Verifying the attributes for StringUnicodeSyntax.
                case "StringUnicodeSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.12"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "64")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            unicode = true;
                        }
                        break;
                    //Verifying the attributes for StringUTCTimeSyntax.
                case "StringUTCTimeSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.11"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "23")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            UTCTime = true;
                        }
                        break;
                    //Verifying the attributes for StringGeneralizedTimeSyntax.
                case "StringGeneralizedTimeSyntax":
                        if (serverValueAttrSyntax.UnderlyingValues[0].ToString() != "2.5.5.11"
                            || serverValueOMSyntax.UnderlyingValues[0].ToString() != "24")
                        {
                            DataSchemaSite.Log.Add(
                                LogEntryKind.Warning, 
                                "Syntax for '{0}' is not as expected",
                                serverObject.Name);
                            genTime = true;
                        }
                        break;
                    //Verifying the attribute extendedCharsAllowed.
                default:
                    if (
                        attrContext.syntax.Name.ToString() != "StringIA5Syntax"
                        && attrContext.syntax.Name.ToString() != "StringNumericSyntax"
                        && attrContext.syntax.Name.ToString() != "StringTeletexSyntax"
                        && attrContext.syntax.Name.ToString() != "StringPrintableSyntax")
                    {
                        if (attributeObject.attributes.Keys.Contains("extendedcharsallowed"))
                        {
                            extCharsAllow = true;
                        }
                    }
                    break;
            }
       }

        #endregion

        #region RequirementsCapture

        private void SyntaxRequirements()
        {
            
            //MS-ADTS-Schema_R17
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !boolean, 
                17,
                "For Boolean LDAP syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.8 and 1.");

            //MS-ADTS-Schema_R18
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !enumeration, 
                18,
                "For Enumeration LDAP syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.9 and 10.");

            // MS-ADTS-Schema_R19
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !integer, 
                19,
                "For Integer LDAP syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.9 and 2.");

            // MS-ADTS-Schema_R20
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !largeInt, 
                20,
                "For LargeInteger LDAP syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.16 and 65.");

            // MS-ADTS-Schema_R21
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !accessPoint, 
                21,
                @"For Object(Access-Point) LDAP syntax name corresponding attributeSyntax, oMSyntax and 
                oMObjectClass are 2.5.5.14, 127 and 1.3.12.2.1011.28.0.702.");

            // MS-ADTS-Schema_R22
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !DNString, 
                22,
                @"For Object(DN-String) LDAP syntax name corresponding attributeSyntax, oMSyntax and 
                oMObjectClass are 2.5.5.14, 127 and 1.2.840.113556.1.1.1.12.");

            // MS-ADTS-Schema_R23
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !ORName, 
                23,
                @"For Object(OR-Name) LDAP syntax name corresponding attributeSyntax, oMSyntax and 
                oMObjectClass are 2.5.5.7, 127 and 2.6.6.1.2.5.11.29.");

            // MS-ADTS-Schema_R24
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !DNBinary, 
                24,
                @"For Object(DN-Binary) LDAP syntax name corresponding attributeSyntax, oMSyntax and 
                oMObjectClass are 2.5.5.7, 127 and 1.2.840.113556.1.1.1.11.");

            // MS-ADTS-Schema_R25
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !DSDN, 
                25,
                @"For Object(DS-DN) LDAP syntax name corresponding attributeSyntax, oMSyntax and oMObjectClass
                are 2.5.5.1, 127 and 1.3.12.2.1011.28.0.714.");

            // MS-ADTS-Schema_R26
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !preAddr, 
                26,
                @"For Object(Presentation-Address) LDAP syntax name corresponding attributeSyntax, oMSyntax 
                and oMObjectClass are 2.5.5.13, 127 and 1.3.12.2.1011.28.0.732.");

            // MS-ADTS-Schema_R27
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !repLink, 
                27,
                @"For Object(Replica-Link) LDAP syntax name corresponding attributeSyntax, oMSyntax and 
                oMObjectClass are 2.5.5.10, 127 and 1.2.840.113556.1.1.1.6.");

            // MS-ADTS-Schema_R28
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !strCase, 
                28,
                "For String(Case) LDAP syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.3 and 27.");

            // MS-ADTS-Schema_R29
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !strIA5, 
                29,
                "For String(IA5) LDAP syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.5 and 22.");

            // MS-ADTS-Schema_R30
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !NTSecDesc, 
                30,
                @"For String(NT-Sec-Desc) LDAP syntax name corresponding attributeSyntax and oMSyntax are
                2.5.5.15 and 66.");

            // MS-ADTS-Schema_R31
            DataSchemaSite.CaptureRequirementIfIsTrue(!numeric, 31,
                @"For String(Numeric) LDAP syntax name corresponding attributeSyntax and oMSyntax are 
                2.5.5.6 and 18.");

            // MS-ADTS-Schema_R32
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !objIdentifier, 
                32,
                @"For String(Object-Identifier) LDAP syntax name corresponding attributeSyntax and 
                oMSyntax are 2.5.5.2 and 6.");

            // MS-ADTS-Schema_R33
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !octet, 
                33, 
                @"For String(Octet) LDAP syntax 
                name corresponding attributeSyntax and oMSyntax are 2.5.5.10 and 4.");

            // MS-ADTS-Schema_R34
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !printable, 
                34, 
                @"For String(Printable) LDAP
                syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.5 and 19.");

            // MS-ADTS-Schema_R35
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !sid, 
                35, 
                @"For String(Sid) LDAP syntax 
                name corresponding attributeSyntax and oMSyntax are 2.5.5.17 and 4.");

            // MS-ADTS-Schema_R36
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !teletext, 
                36,
                @"For String(Teletex) LDAP syntax name corresponding attributeSyntax and oMSyntax 
                are 2.5.5.4 and 20.");

            // MS-ADTS-Schema_R37
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !unicode, 
                37,
                @"For String(Unicode) LDAP syntax name corresponding attributeSyntax and oMSyntax 
                are 2.5.5.12 and 64.");

            // MS-ADTS-Schema_R38
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !UTCTime, 
                38,
                "For String(UTC-Time) LDAP syntax name corresponding attributeSyntax and oMSyntax are 2.5.5.11 and 23.");

            // MS-ADTS-Schema_R39
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !genTime, 
                39,
                @"For String(Generalized-Time) LDAP syntax name corresponding attributeSyntax and
                oMSyntax are 2.5.5.11 and 24.");

            // MS-ADTS-Schema_R40
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !intRange, 
                40,
                "The Integer syntax in Active Directory is restricted to 32-bit integers.");

            // MS-ADTS-Schema_R41
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !larIntRange, 
                41,
                "The LargeInteger syntax in Active Directory is restricted to 64-bit integers.");

            // MS-ADTS-Schema_R49
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !binaryFormat, 
                49,
                @"A value with Object(DN-Binary) syntax is a UTF-8 string is in the format 
                'B:char_count:binary_value:object_DN'.");

            // MS-ADTS-Schema_R50
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !hexCount, 
                50,
                @"In Object(DN-Binary) syntax char_count is the number (in decimal) of hexadecimal digits in 
                binary_value, binary_value is the hexadecimal representation of a binary value, object_DN is a DN in 
                Object(DS-DN) form, and all remaining characters are string literals.");

            // MS-ADTS-Schema_R51
            DataSchemaSite.CaptureRequirementIfIsTrue(
                byteToHex != null, 
                51,
                @"Each byte in Object(DN-Binary) syntax is represented by a pair of hexadecimal characters in 
                binary_value, with the first character of each pair corresponding to the most-significant nibble 
                of the byte.");

            // MS-ADTS-Schema_R95
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !extCharsAllow, 
                95,
                @"The attribute extendedCharsAllowed of the attributeSchema is applicable to 
                attributes of syntax String(IA5), String(Numeric), String(Teletex), String(Printable).");

            // MS-ADTS-Schema_R113
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !rangeLower32, 
                113,
                "rangeLower attribute of the attributeSchema is a 32-bit integer.");

            // MS-ADTS-Schema_R123
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !rangeUpper32, 
                123,
                "rangeUpper attribute of the attributeSchema is a 32-bit integer.");

            // MS-ADTS-Schema_R156
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !attrType, 
                156,
                "The RDN attribute is of syntax String(Unicode).");

            // MS-ADTS-Schema_R271
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !rDNSyntax, 
                271,
                @"Active Directory imposes the additional restriction that the AttributeType
                used must be of String(Unicode) syntax.");
        }

        #endregion
    }
}