// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// This class is used to represent the Constructed attributes.
    /// </summary>
    public static class ConstructedAttributes
    {
        /// <summary>
        /// Computes the distinguished name of an object.
        /// </summary>
        /// <param name="obj">A sequence of ModelObject.</param>
        /// <returns>Value of distinguised name.</returns>
        public static Value DistinguishedName(ModelObject obj)
        {
            return new Value(Syntax.StringUnicode, ComputeName(StandardNames.distinguishedName, obj));
        }

        /// <summary>
        /// An internal attribute for a shorter version of the DN, leaving out the DC name.
        /// </summary>
        /// <param name="obj">A sequence of ModelObject.</param>
        /// <returns>Value of short name.</returns>
        public static Value ShortName(ModelObject obj)
        {
            return new Value(Syntax.StringUnicode, ComputeName(StandardNames.shortName, obj));
        }

        static string ComputeName(string attr, ModelObject obj)
        {
            if (obj.dc.domainReplica != null && obj == obj.dc.domainReplica.root)
            {
                if (attr == StandardNames.shortName)
                {
                    return "<rootdomaindn>";
                }
                else
                {
                    return obj.dc.domainReplica.dn;
                }
            }
            string rdnName = obj.dc.GetRDNAttributeName(obj);
            string rdn = (string)obj.GetRequiredAttributeValue(rdnName);
            ModelObject parent = ParentObject(obj);

            if (parent != null)
            {
                return String.Format("{0}={1},{2}", rdnName, rdn, parent[attr]);
            }
            else
            {
                return String.Format("{0}={1}", rdnName, rdn);
            }
        }

        static ModelObject ParentObject(ModelObject obj)
        {
            ModelObject result;
            if (obj.dc.domainReplica != null && obj == obj.dc.domainReplica.root)
            {
                result = null;
            }
            else if (obj.dc.applicationReplica != null && obj == obj.dc.applicationReplica.root)
            {
                result = null;
            }
            else if (obj == obj.dc.configReplica.root)
            {
                if (obj.dc.domainReplica != null)
                {
                    result = obj.dc.domainReplica.root;
                }
                else
                {
                    result = null;
                }
            }
            else if (obj == obj.dc.schemaReplica.root)
            {
                result = obj.dc.configReplica.root;
            }
            else
            {
                result = obj.parent;
            }
            return result;
        }

        /// <summary>
        /// Computes the parent object.
        /// </summary>
        /// <param name="obj">Model object</param>
        /// <returns>On succes it returns the parent object of this object, otherwise null.</returns>
        public static Value Parent(ModelObject obj)
        {
            ModelObject result = ParentObject(obj);
            if (result != null)
            {
                return new AttributeContext(obj.dc, "<parent>", Syntax.ObjectDSDN).Parse((string)result[StandardNames.distinguishedName]);
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// Registers all constructed attributes.
        /// </summary>
        public static void Install()
        {
            ModelObject.RegisterConstructedAttribute(StandardNames.distinguishedName, DistinguishedName);
            ModelObject.RegisterConstructedAttribute(StandardNames.parent, Parent);
            ModelObject.RegisterConstructedAttribute(StandardNames.shortName, ShortName);
        }

        /// <summary>
        /// This method is used to get allowed attributes of a particular object.
        /// </summary>
        /// <param name="ldapDisplayName">LDAP display name of the particular object.</param>
        /// <param name="dcModel">System Model object</param>
        /// <returns>On success it returns the List of allowed attributes, else null.</returns>
        public static List<string> GetAllowedAttributes(string ldapDisplayName, ModelDomainController dcModel)
        {
            List<string> testAllowedAttributes = new List<string>();
            List<ModelObject> reqEntryObjectClassValues = new List<ModelObject>();
            ModelObject reqEntry = null;

            //Get the model object corresponding to ldapDisplayName passed to this method.
            while (reqEntry == null)
            {
                dcModel.TryGetClass(ldapDisplayName, out reqEntry);
                if (reqEntry == null)
                {
                    dcModel.TryGetAttribute(ldapDisplayName, out reqEntry);
                }
            }

            if (reqEntry.attributes.Keys.Contains("objectclass"))
            {
                foreach (string value in reqEntry.attributes["objectclass"].UnderlyingValues)
                {
                    ModelObject tempObject = null;

                    while (tempObject == null)
                    {
                        dcModel.TryGetClass(value, out tempObject);
                    }
                    reqEntryObjectClassValues.Add(tempObject);
                }
            }

            //First condition, take all entries in SchemaNC.
            MapContainer<string, ModelObject> allSchemaEntries = dcModel.schemaReplica.root.childs;
            foreach (KeyValuePair<string, ModelObject> schemaEntry in allSchemaEntries)
            {
                ModelObject entry = schemaEntry.Value;

                testAllowedAttributes.Add((string)entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0));
            }

            

            //All Conditions
            foreach (KeyValuePair<string, ModelObject> schemaEntry in allSchemaEntries)
            {
                ModelObject entry = schemaEntry.Value;
                string tempLdapName = (string)entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0);

                #region 2. objectClass value must be attributeSchema.

                object[] objectClassValues = entry.attributes["objectclass"].UnderlyingValues.ToArray();
                string structuralClassName = (string)objectClassValues[objectClassValues.Length - 1];

                if (!structuralClassName.ToLower().Equals("attributeschema"))
                {
                    if (testAllowedAttributes.Contains(tempLdapName))
                    {
                        testAllowedAttributes.Remove(tempLdapName);
                        continue;
                    }
                }
                #endregion

                #region 3. Link id is even or it should not be present
                
                #endregion

                #region 4. SystemFlag bit 0X4 should not be set.
                
                #endregion

                #region 5. there exists C in TO!objectClass such that O is in CLASSATTS(C)).

                ConstructedAttributeHelper helper = new ConstructedAttributeHelper();
                bool isExist = true;

                foreach (ModelObject classObj in reqEntryObjectClassValues)
                {
                    helper.classAttributesList = new List<string>();
                    List<string> attributeList = helper.ClassAttributes(classObj, dcModel);
                    if (attributeList.Contains(tempLdapName.ToLower()))
                    {
                        isExist = true;
                        break;
                    }
                    else
                        isExist = false;
                }
                if (!isExist && testAllowedAttributes.Contains(tempLdapName))
                {
                    testAllowedAttributes.Remove(tempLdapName);
                }

                #endregion
            }

            return testAllowedAttributes;
        }


        /// <summary>
        /// This method is used to get the possible inferiors of the particula object.
        /// </summary>
        /// <param name="ldapDisplayName">LDAP display name of the particular object.</param>
        /// <param name="dcModel">System Model object</param>
        /// <returns>On success it returns the List of allowed attributes, else null.</returns>
        public static List<string> GetPossibleInferiors(string ldapDisplayName,ModelDomainController dcModel)
        {
            List<string> testPossInferiors = new List<string>();
            ModelObject reqEntry = null;

            //Get the model object corresponding to ldapDisplayName passed to this method.
            while (reqEntry == null)
            {
                dcModel.TryGetClass(ldapDisplayName, out reqEntry);
            }


            //First condition, take all entries in SchemaNC.
            MapContainer<string, ModelObject> allSchemaEntries = dcModel.schemaReplica.root.childs;
            foreach (KeyValuePair<string, ModelObject> schemaEntry in allSchemaEntries)
            {
                ModelObject entry = schemaEntry.Value;
                testPossInferiors.Add((string)entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0));
            }

            //Get required entry's class name.
            string className = (string)reqEntry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0);

            //All Conditions
            foreach (KeyValuePair<string, ModelObject> schemaEntry in allSchemaEntries)
            {
                ModelObject entry = schemaEntry.Value;
                string tempLdapName = (string)entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0);

                #region 2. objectClass value must be classSchema.

                object[] objectClassValues = entry.attributes["objectclass"].UnderlyingValues.ToArray();
                string structuralClassName = (string)objectClassValues[objectClassValues.Length - 1];

                if (!structuralClassName.ToLower().Equals("classschema"))
                {
                    if (testPossInferiors.Contains(tempLdapName))
                    {
                        testPossInferiors.Remove(tempLdapName);
                        continue;
                    }
                }
                #endregion

                #region 3. entry must not systemOnly.

                if(entry.attributes.Keys.Contains("systemonly"))
                {
                    bool isSystemOnly = (bool)entry.attributes["systemonly"].UnderlyingValues.ElementAt(0);

                    if (isSystemOnly)
                    {
                        if (testPossInferiors.Contains(tempLdapName))
                        {
                            testPossInferiors.Remove(tempLdapName);
                            continue;
                        }
                    }
                }

                #endregion

                #region 4. objectClassCategory must not be 2 or 3.
                if(entry.attributes.Keys.Contains("objectclasscategory"))
                {
                    int value = (int)entry.attributes["objectclasscategory"].UnderlyingValues.ElementAt(0);

                    if (value == 2 || value == 3)
                    {
                        if (testPossInferiors.Contains(tempLdapName))
                        {
                            testPossInferiors.Remove(tempLdapName);
                            continue;
                        }
                    }
                }

                #endregion 

                #region 5. entry's class name must be in POSSSUPERIORS(allSchemaNCObjects).

                if(entry.attributes.Keys.Contains("governsid"))
                {
                    ConstructedAttributeHelper helper = new ConstructedAttributeHelper();
                    helper.possSuperiorList = new List<string>();
                    //helper.AuxiliaryClasses(entry, true, dcModel);
                    List<string> possSuperiors = helper.GetPossSuperiorsList(entry, dcModel);

                    if (possSuperiors.Contains(className.ToLower()))
                    {
                        continue;
                    }
                    else
                    {
                        if (testPossInferiors.Contains(tempLdapName))
                        {
                            testPossInferiors.Remove(tempLdapName);
                        }
                        continue;
                    }
                }
                #endregion

            }
            return testPossInferiors;
        }


        public static List<string> GetMsdsAuxiliaryClasses(string ldapDisplayName, ModelDomainController dcModel)
        {
            List<string> testMsdsAuxiliaryClasses = new List<string>();
            List<string> objectClassValues = new List<string>();
            ModelObject reqEntry = null;

            //Get the model object corresponding to ldapDisplayName passed to this method.
            while (reqEntry == null)
            {
                dcModel.TryGetClass(ldapDisplayName, out reqEntry);
            }

            //Get object class of this entry.
            foreach (string entry in reqEntry.attributes["objectclass"].UnderlyingValues)
            {
                objectClassValues.Add(entry);
            }

            //First condition, take all entries in SchemaNC.
            MapContainer<string, ModelObject> allSchemaEntries = dcModel.schemaReplica.root.childs;

            //All Conditions
            foreach (KeyValuePair<string, ModelObject> schemaEntry in allSchemaEntries)
            {
                ModelObject entry = schemaEntry.Value;

                if (objectClassValues.Contains(entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0).ToString().ToLower()))
                {
                    ConstructedAttributeHelper helper = new ConstructedAttributeHelper();
                    ModelObject tempModelObject = null;
                    dcModel.TryGetClass(objectClassValues.Last(), out tempModelObject);
                    List<string> tempSuperClasses = helper.GetSuperClassesList(tempModelObject, true, dcModel);
                    tempSuperClasses.Add(entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0).ToString());

                    if (!tempSuperClasses.Contains(entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0).ToString()))
                        testMsdsAuxiliaryClasses.Add(entry.attributes["ldapdisplayname"].UnderlyingValues.ElementAt(0).ToString().ToLower());
                }
            }

            return testMsdsAuxiliaryClasses;
        }
    }
}
