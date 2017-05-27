// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for methods used for common usage.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region CompareObjects

        /// <summary>
        /// CompareObjects method is used to compare the objects from server and the model.
        /// </summary>
        /// <param name="modelObject">Model Object</param>
        /// <param name="serverObject">Server Object</param>
        /// <param name="hasErrors">Error tracking variable</param>
        /// <returns>Void</returns>
        void CompareObjects(ModelObject modelObject, IObjectOnServer serverObject, ref bool hasErrors)
        {
            foreach (string attr in modelObject.attributes.Keys)
            {
                //SDs for DomainDNS and SamDomain differ because of an ACE that is added during DCPromo using the expanded binary SID instead of its string representation of “RO” to maintain compatibility with downlevel OS’s.  
                //Because of this, the SDs represented in the TD will be logically consistent with the SD in the server but will never be 100% identical. 
                if ((attr == "defaultsecuritydescriptor")
                    && ((serverObject.Name == "domainDNS")
                    || (serverObject.Name == "msSPP-ActivationObject")
                    || (serverObject.Name == "msSPP-ActivationObjectsContainer")
                    || (serverObject.Name == "samDomain")))
                {
                    continue;
                }

                //skip possibleinferiors temporarily since the calculation of possibleinferiors seems wrong
                if (attr == "possibleinferiors")
                {
                    continue;
                }

                Value value = modelObject[attr];
                if (!serverObject.Properties.ContainsKey(attr))
                {
                    DataSchemaSite.Log.Add(LogEntryKind.Warning, "attribute '{0}' must be in server object '{1}'", attr, serverObject.Name);
                    hasErrors = true;
                    //TDI 67172 is identified as a product bug and will be fixed in Blue+1
                    if(!adAdapter.TDI67172Fixed)
                    {
                        if ((serverObject.Name == "msDS-GeoCoordinatesAltitude")
                            || (serverObject.Name == "msDS-GeoCoordinatesLatitude")
                            || (serverObject.Name == "msDS-GeoCoordinatesLongitude"))
                        {
                            hasErrors = false;
                        }
                    }
                    continue;
                }
                AttributeContext context;
                if (!dcModel.TryGetAttributeContext(attr, out context))
                {
                    // Inconsistency should be already reported
                    continue;
                }

                //Remove the line break in the value of defaultsecuritydescriptor and defaultobjectcategory
                if ((attr == "defaultsecuritydescriptor")
                    || (attr == "defaultobjectcategory"))
                {
                    value = new Value(context.syntax, value.UnderlyingValues[0].ToString().Replace("\r", String.Empty));
                }
                Value serverValue = new Value(context.syntax, serverObject.Properties[attr]);
                if (!context.Equals(value, serverValue))
                {
                    if (modelObject[StandardNames.attributeID] != null)
                    {
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning,
                            "In Attribute {4} attribute '{0}' (syntax {3}) has different value '{1}' on server than in model '{2}'",
                            attr,
                            context.Unparse(serverValue),
                            context.Unparse(value),
                            context.syntax.Name,
                            modelObject[StandardNames.cn]);
                    }
                    else
                    {
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning,
                            "In Class {4} attribute '{0}' (syntax {3}) has different value '{1}' on server than in model '{2}'",
                             attr,
                             context.Unparse(serverValue),
                             context.Unparse(value),
                             context.syntax.Name,
                             modelObject[StandardNames.cn]);
                    }
                    hasErrors = true;
                }
            }
        }

        #endregion

        #region msds-isgc
        /// <summary>
        /// VerifyMsdsIsGC method is used to verify the value of options of given directory entry is even or odd.
        /// </summary>
        /// <param name="reqEntry">DirectoryEntry</param>
        /// <returns>Returns true if the options field contains odd value.</returns>
        public bool VerifyMsdsIsGC(DirectoryEntry reqEntry)
        {
            PropertyValueCollection objectClassValue = reqEntry.Properties["objectclass"];
            if (objectClassValue.Contains("nTDSDSA"))
            {
                int optionValue = (int)reqEntry.Properties["options"].Value;
                optionValue = optionValue & 1;

                if (optionValue == 1)
                    return true;
            }

            if (objectClassValue.Contains("server"))
            {
                string childDN = String.Empty;
                DirectoryEntry TN = reqEntry.Children.Find("CN=NTDS Settings");
                childDN = TN.Properties["distinguishedname"].Value.ToString();

                if (childDN.Equals("CN=NTDS Settings," + reqEntry.Properties[StandardNames.distinguishedName].Value.ToString()))
                {
                    return VerifyMsdsIsGC(TN);
                }
            }

            if (objectClassValue.Contains("computer"))
            {
                //Get server object with its serverReferenceBL attribute.
                DirectoryEntry TS = null;
                if (!adAdapter.GetObjectByDN(reqEntry.Properties["serverReferenceBL"].Value.ToString(), out TS))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false, 
                        reqEntry.Properties["serverReferenceBL"].Value.ToString()
                        + " Object is not found in server");
                }

                if (TS != null)
                {
                    return VerifyMsdsIsGC(TS);
                }

            }
            return false;
        }

        #endregion
        
        #region SystemFlags

        /// <summary>
        /// ParseSystemFlagsValue method is used to convert the string notation of flag to int value.
        /// </summary>
        /// <param name="strFlag">String form of flag.</param>
        /// <returns>return the int value of the given flag.</returns>
        public Int32 ParseSystemFlagsValue(string strFlag)
        {
            Type flagType = IntegerSymbols.GetSymbolEnumType("SystemFlags");
            uint result;
            if (strFlag.Contains("|"))
            {
                strFlag = strFlag.Replace('|', ',');
            }
            result = (uint)Enum.Parse(flagType, strFlag, true);
            return (int)result;
        }

        /// <summary>
        /// ParseSystemFlagsValue method is used to convert the value of flag to object.
        /// </summary>
        /// <param name="strFlag">String form of flag.</param>
        /// <returns>return the int value of the given flag.</returns>
        public string ParseSystemFlagsValue(int valueFlag)
        {
            Type flagType = IntegerSymbols.GetSymbolEnumType("SystemFlags");
            string result = Enum.ToObject(flagType, valueFlag).ToString();
            return result;
        }
        #endregion

        #region UserAccountControl Bit

        /// <summary>
        /// ParseUserAccountControlValue is used to convert the string notation of User AccountControl to int value.
        /// </summary>
        /// <param name="strUserActControl">User AccountControl</param>
        /// <returns>Integer value of the User AccountControl Value</returns>
        public Int32 ParseUserAccountControlValue(string strUserActControl)
        {
            Type flagType = IntegerSymbols.GetSymbolEnumType("UserAccountControl");
            uint result;
            if (strUserActControl.Contains("|"))
            {
                strUserActControl = strUserActControl.Replace('|', ',');
            }
            result = (uint)Enum.Parse(flagType, strUserActControl, true);
            return (int)result;
        }

        #endregion

        public bool GetLDAPObject(string distinguishedName, string serverName,
           string filter, System.DirectoryServices.Protocols.SearchScope scope, string[] reqAttributes,
           out System.DirectoryServices.Protocols.SearchResponse result)
        {
            LdapConnection connection = null;
            try
            {
                if (serverOS < OSVersion.WinSvr2008R2)
                {
                    connection = new LdapConnection(new LdapDirectoryIdentifier(serverName));
                }
                else
                {
                    connection = new LdapConnection(new LdapDirectoryIdentifier(serverName + "." + adAdapter.PrimaryDomainDnsName));
                }
                connection.Bind();

                SearchRequest request = new SearchRequest(distinguishedName, filter, scope, reqAttributes);

                result = (SearchResponse)connection.SendRequest(request);

                connection.Dispose();

                return true;

            }
            catch (Exception)
            {
                connection.Dispose();
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Verify if the source data is a correct schedule structure
        /// </summary>
        /// <param name="source">Source data</param>
        /// <returns>Result of verifing</returns>
        private bool VerifyScheduleStructure(byte[] source)
        {
            uint schedule_interval = 0;
            if (source != null)
            {
                int offset = 0;
                //First 4 bytes is total length and next 4 bytes are ignored
                int totalLength = BitConverter.ToInt32(source, offset);
                offset += 8;
                if (totalLength != source.Length)
                {
                    return false;
                }
                //Number of schedule array is 4 bytes.
                int arrayNumber = BitConverter.ToInt32(source, offset);
                offset += 4;
                //This value MUST be 1 according to TD
                if (arrayNumber != 1)
                {
                    return false;
                }
                //Schedule type is 4bytes
                uint scheduleType = BitConverter.ToUInt32(source, offset);
                offset += 4;
                if (scheduleType != schedule_interval)
                {
                    return false;
                }
                //Schedule offset is 4 bytes
                int schduleOffset = BitConverter.ToInt32(source, offset);
                offset += 4;
                if (schduleOffset != offset)
                {
                    return false;
                }
                //According to TD, the left data MUST be 7*24 = 168 bytes. It contains replication timer.
                if (totalLength - schduleOffset != 168)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

