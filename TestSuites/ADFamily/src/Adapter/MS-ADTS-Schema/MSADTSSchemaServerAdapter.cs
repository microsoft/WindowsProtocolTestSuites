// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Text;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;


namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// ActiveDirectory class inherits IActiveDirectory,
    /// and implements all the methods provided by IActiveDirectory.
    /// It also provides creation of the ADAM instance for accessing the AD/LDS objects.
    /// </summary>
    public class ADDataSchemaAdapter : ADCommonServerAdapter, IADDataSchemaAdapter
    {
        public string PartitionPath = "DC=NewApplicationPartition";
        public string Description = "ApplicationNC";
        public string DeletedUserName = "testerSchema";
        public string DeletedGroupName = "testgroupSchema";
        public string adamServerPort;
        public IPAddress PDCIPAddr;
        public string TDXmlPath;
        public string LdsTDXmlPath;
        public string OpenXmlPath;
        public string LdsOpenXmlPath;
        public string LDSServerInstance;
        public bool RunDSTestCases;
        public bool RunLDSTestCases;
        public bool TDI67172Fixed;
        string propertyGroup = "MS_ADTS_Schema.";
        public string SchemaLog = "MS-ADTS-Schema.log";
        public string rootDomainDN;
        public string childDomainDN;
        public string LDSRootObjectName;     

        TimeSpan timeout;

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            timeout = TimeSpan.FromMilliseconds(10000000);
            adamServerPort = string.Format("{0}.{1}:{2}", PDCNetbiosName, PrimaryDomainDnsName, ADLDSPortNum);
            PDCIPAddr = IPAddress.Parse(PDCIPAddress);
            if ((ServerVersion)PDCOSVersion <= ServerVersion.Win2012R2)
            {
                TDXmlPath = GetProperty(propertyGroup + "TDXmlPath");
                LdsTDXmlPath = GetProperty(propertyGroup + "LdsTDXmlPath");
            }
            else
            {
                OpenXmlPath = GetProperty(propertyGroup + "OpenXmlPath2016");
                LdsOpenXmlPath = GetProperty(propertyGroup + "LdsOpenXmlPath2016");
            }
            LDSServerInstance = PDCNetbiosName + @"$" + ADLDSInstanceName;
            RunDSTestCases = GetBoolProperty(propertyGroup + "RunDSTestCases");
            RunLDSTestCases = GetBoolProperty(propertyGroup + "RunLDSTestCases");
            TDI67172Fixed = GetBoolProperty(propertyGroup + "TDI.67172.Fixed");
            rootDomainDN = "DC=" + PrimaryDomainDnsName.Replace(".",",DC=");
            childDomainDN = "DC=" + ChildDomainDnsName.Replace(".", ",DC=");
            if (RunLDSTestCases)
            {
                LDSRootObjectName = Utilities.GetLdsDomainDN(string.Format("{0}.{1}:{2}", PDCNetbiosName, PrimaryDomainDnsName, ADLDSPortNum));
            }
        }

        public override void Reset()
        {
            base.Reset();
        }

        /// <summary>
        /// GetAllSchemaClasses method retrieves all the schema classes from AD/DS domain controller.
        /// </summary>
        public IEnumerable<IObjectOnServer> GetAllSchemaClasses()
        {
            DirectoryEntry entry;
            string distinguishedName = "CN=Schema, CN=Configuration," + rootDomainDN;
            GetObjectByDN(distinguishedName, out entry);
            DirectoryEntries entries = entry.Children;
            foreach (DirectoryEntry classEntry in entries)
            {
                classEntry.RefreshCache(new string[] { "objectClass", "lDAPDisplayName" });
                if (classEntry.Properties["objectClass"].Contains("classSchema"))
                {
                    yield return new ObjectOnServer(classEntry.Properties["lDAPDisplayName"].Value.ToString(), classEntry);
                }
            }
        }


        /// <summary>
        /// GetAllSchemaAttributes method retrieves all the schema Attributes from AD/DS domain controller. 
        /// </summary>
        public IEnumerable<IObjectOnServer> GetAllSchemaAttributes()
        {
            DirectoryEntry entry;
            string distinguishedName = "CN=Schema, CN=Configuration," + rootDomainDN;
            GetObjectByDN(distinguishedName, out entry);
            DirectoryEntries entries = entry.Children;
            foreach (DirectoryEntry classEntry in entries)
            {
                classEntry.RefreshCache(new string[] { "objectClass", "lDAPDisplayName" });
                if (classEntry.Properties["objectClass"].Contains("attributeSchema"))
                {
                    yield return new ObjectOnServer(classEntry.Properties["lDAPDisplayName"].Value.ToString(), classEntry);
                }
            }
        }


        /// <summary>
        /// GetAllLdsSchemaClasses method retrieves all the schema classes from AD/LDS domain controller. 
        /// </summary>
        public IEnumerable<IObjectOnServer> GetAllLdsSchemaClasses()
        {
            DirectoryEntry entry;
            string distinguishedName = "CN=Schema, CN=Configuration," + LDSRootObjectName;
            GetLdsObjectByDN(distinguishedName, out entry);
            DirectoryEntries entries = entry.Children;
            foreach (DirectoryEntry classEntry in entries)
            {
                classEntry.RefreshCache(new string[] { "objectClass", "lDAPDisplayName" });
                if (classEntry.Properties["objectClass"].Contains("classSchema"))
                {
                    yield return new ObjectOnServer(classEntry.Properties["lDAPDisplayName"].Value.ToString(), classEntry);
                }
            }
        }


        /// <summary>
        /// GetAllLdsSchemaAttributes method retrieves all the schema Attributes from AD/LDS domain controller. 
        /// </summary>
        public IEnumerable<IObjectOnServer> GetAllLdsSchemaAttributes()
        {
            DirectoryEntry entry;
            string distinguishedName = "CN=Schema, CN=Configuration," + LDSRootObjectName;
            GetLdsObjectByDN(distinguishedName, out entry);
            DirectoryEntries entries = entry.Children;
            foreach (DirectoryEntry classEntry in entries)
            {
                classEntry.RefreshCache(new string[] { "objectClass", "lDAPDisplayName" });
                if (classEntry.Properties["objectClass"].Contains("attributeSchema"))
                {
                    yield return new ObjectOnServer(classEntry.Properties["lDAPDisplayName"].Value.ToString(), classEntry);
                }
            }
        }


        /// <summary>
        /// GetObjectByDN method is used to retrieve AD/DS Object for a specified Distinguished Name.
        /// </summary>
        /// <param name="disntinguishedName">Distinguished Name</param>
        /// <param name="resultedObject">Object of Distinguished Name</param>
        /// <returns>Returns TRUE if Object exists</returns>
        public bool GetLdsObjectByDN(string disntinguishedName, out DirectoryEntry resultedObject)
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + adamServerPort + "/" + disntinguishedName, ClientUserName, ClientUserPassword);
            try
            {
                var name = entry.Name;
                resultedObject = entry;
                return true;
            }
            catch
            {
                resultedObject = null;
                return false;
            }
        }


        /// <summary>
        /// GetLdsObjectByDN method is used to retrieve AD/LDS Object for a specified Distinguished Name.
        /// </summary>
        /// <param name="disntinguishedName">Distinguished Name</param>
        /// <param name="resultedObject">Object of Distinguished Name</param>
        /// <returns>Returns TRUE if Object exists</returns>
        public bool GetObjectByDN(string disntinguishedName, out DirectoryEntry resultedObject)
        {
            if (DirectoryEntry.Exists("LDAP://" + PDCNetbiosName + "/" + disntinguishedName))
            {
                resultedObject = new DirectoryEntry("LDAP://" + PDCNetbiosName + "/" + disntinguishedName);
                return true;
            }
            else
            {
                resultedObject = null;
                return false;
            }
        }

    }


    /// <summary>
    /// ObjectOnServer class inherits IObjectOnServer, and
    /// Implements all the methods provided by IObjectOnServer.
    /// </summary>
    class ObjectOnServer : IObjectOnServer
    {
        String name;
        DirectoryEntry entry;
        Dictionary<string, object[]> properties;

        internal ObjectOnServer(string name, DirectoryEntry entry)
        {
            this.name = name;
            this.entry = entry;
        }

        ~ObjectOnServer()
        {
            if (entry != null)
            {
                entry.Dispose();
            }
            entry = null;
        }

        public string Name
        {
            get { return name; }
        }


        /// <summary>
        /// Properties property returns the properties of the Object on server.
        /// </summary>
        public Dictionary<string, object[]> Properties
        {
            get
            {
                if (properties != null)
                {
                    return properties;
                }
                properties = new Dictionary<string, object[]>();
                entry.RefreshCache(new string[] { "omobjectclass" });

                int iCount = 1;
                foreach (PropertyValueCollection property in entry.Properties)
                {
                    List<object> list = new List<object>();
                    foreach (object o in property)
                    {
                        list.Add(o);
                    }
                    object[] values = list.ToArray();
                    properties[property.PropertyName.ToLower().Trim()] = values;
                    if (iCount == entry.Properties.Count - 1)
                    {
                        list = new List<object>();

                        PropertyValueCollection colln = entry.Properties["omobjectclass"];
                        if (colln.Value != null)
                        {
                            string str = ToOid((byte[])colln.Value);
                            properties["omobjectclass"] = new object[] { str };
                        }
                        else
                        {
                            properties["omobjectclass"] = new object[] { null };
                        }


                        iCount++;
                    }
                    else
                    {
                        iCount++;
                    }
                }
                entry.Dispose();
                entry = null;
                return properties;
            }
        }


        /// <summary>
        /// ToOid method is used to covert the byte format of GUID to string format.
        /// </summary>
        /// <param name="aOID">Byte form of GUID</param>
        /// <returns>Returns string </returns>
        string ToOid(byte[] aOID)
        {
            StringBuilder strBuilder = new StringBuilder();
            // Pick apart the OID
            byte quotientValue = (byte)(aOID[0] / 40);
            byte remainderValue = (byte)(aOID[0] % 40);
            if (quotientValue > 2)
            {
                // Handle special case for large y if x = 2
                remainderValue += (byte)((quotientValue - 2) * 40);
                quotientValue = 2;
            }
            strBuilder.Append(quotientValue.ToString(CultureInfo.InvariantCulture));
            strBuilder.Append(".");
            strBuilder.Append(remainderValue.ToString(CultureInfo.InvariantCulture));
            ulong valCount = 0;
            for (quotientValue = 1; quotientValue < aOID.Length; quotientValue++)
            {
                valCount = ((valCount << 7) | ((byte)(aOID[quotientValue] & 0x7F)));
                if (!((aOID[quotientValue] & 0x80) == 0x80))
                {
                    strBuilder.Append(".");
                    strBuilder.Append(valCount.ToString(CultureInfo.InvariantCulture));
                    valCount = 0;
                }
            }

            return strBuilder.ToString();
        }
    }

}
