// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    public partial class ModelDomainController
    {
        /// <summary>
        /// Adds a set of object definitions to the schema. The format of the schema
        /// is a sequence of lines where objects are separated by empty lines.
        /// Each line of an object definition is a pair of an attribute display name and an
        /// attribute value, separated by ':'. Takes care of "bootstrapping" the contents of
        /// the schema, resolving cyclic dependencies between attribute definitions and objects. 
        /// </summary>
        /// <param name="schema">The schema, represented as explained above.</param>
        /// <param name="valueSubstitution">A substitution of strings which should be applied to each value in the 
        /// schema. For example, may map '&lt;rootdomaindn&gt;' as used in schemas to represent the root name of the 
        /// current DC to an actual value</param>
        /// <param name="serverVersion">Specify the OS version of the server</param>
        /// <returns>Result of the Load Schema method.</returns>
        public ModelResult LoadSchema(IEnumerable<string> schema, Map<string, string> valueSubstitution, OSVersion serverVersion)
        {
            SequenceContainer<ModelObject> newObjects = new SequenceContainer<ModelObject>();
            UnresolvedSyntax unresolvedSyntax = new UnresolvedSyntax();

            #region Pass 1: read in objects resolving only builtin attributes
            // We need this phase for bootstrapping, as we don't have the
            // attributes available which are used in the objects. Thus
            // in this phase, we just parse attribute values of a certain set
            // of builting attributes which are required to get attribute syntax
            // definitions.
            InitializeBuiltinAttributes();
            ModelObject tempobj = null;
            ModelObject obj = null;
            
            foreach (string rawLine in schema)
            {
                string line = (rawLine == null)?null:rawLine.Trim();

                if (String.IsNullOrEmpty(line))
                {
                    // Object ends here
                    if (obj != null)
                    {
                        if (obj[StandardNames.attributeID] == null && obj[StandardNames.governsId] == null)
                        {
                            if (obj.attributes.Count == 1)
                            {
                                // If the server is Windows 2000
                                if (serverVersion == OSVersion.WinSvr2000)
                                {
                                    string attr = obj.attributes.Keys.ElementAt(0);

                                    if (tempobj.attributes.Keys.Contains(attr))
                                    {
                                        tempobj.attributes.Keys.Remove(attr);
                                        tempobj.attributes.Add(obj.attributes.ElementAt(0));
                                    }
                                    else
                                    {
                                        tempobj.attributes.Add(obj.attributes.ElementAt(0));
                                    }
                                    newObjects.RemoveAt(newObjects.Count - 1);
                                    newObjects.Add(tempobj);
                                }
                            }
                            else
                            {
                                Checks.Fail("attributeId/governsId is mandatory for each object {0}.", obj[StandardNames.cn]);
                            }
                        }
                        else if (obj[StandardNames.attributeID] != null)
                        {
                            AddAttribute(obj);
                            newObjects.Add(obj);
                        }
                        else if (obj[StandardNames.governsId] != null)
                        {
                            // This is a class definition. Fill in class map.
                            AddClass(obj);
                            newObjects.Add(obj);
                        }
                        tempobj = obj;
                        obj = null;
                    }
                }
                else
                {
                    if (obj == null)
                    {
                        obj = new ModelObject();
                    }
                    obj.dc = this;

                    string attr, valueString;
                    string[] splits = line.Split(new char[] { ':' }, 2);

                    if (splits == null || splits.Length != 2)
                    {
                        Checks.Fail("invalid schema line '{0}'", line);
                        continue;
                    }
                    attr = splits[0].Trim().ToLower();
                    valueString = Substitute(valueSubstitution, splits[1].Trim());
                    AttributeContext parsingContext;

                    if (!builtinAttributeSyntax.TryGetValue(attr, out parsingContext))
                    {
                        parsingContext = new AttributeContext(this, attr, unresolvedSyntax);
                    }
                    obj[attr] = parsingContext.Parse(valueString);
                }
            }
            #endregion

            #region Pass 2: resolve syntax of all new objects
            // As we have now attribute definitions which map to the syntax, we can parse
            // All values in the new objects.
            foreach (ModelObject newObj in newObjects)
            {
                foreach (string attr in newObj.attributes.Keys)
                {
                    AttributeContext parsingContext = GetAttributeContext(attr);
                    Value value = newObj[attr];

                    if (value.Syntax is UnresolvedSyntax)
                    {
                        newObj[attr] = parsingContext.Parse((string)value);
                    }
                    else
                    {
                        Checks.IsTrue(
                            parsingContext.syntax == value.Syntax,
                            "bultin syntax assignment of '{0}' must match syntax declared in schema", 
                            attr);
                    }
                }
            }
            #endregion

            #region Pass 3: add objects
            // Now all definitions are complete. Add them to schema replica root.
            foreach (ModelObject newObj in newObjects)
            {
                AddChild(schemaReplica.root, newObj);
            }
            #endregion

            #region Pass 4: check consistency
            // The tree is finally setup. Now check for consistency
            Consistency.Check(newObjects.ToSequence());
            #endregion

            if (Checks.HasDiagnostics)
            {
                return new ModelResult(ResultCode.ConstraintViolation, Checks.GetAndClearLog(), Checks.GetAndClearDiagnostics());
            }
            else
            {
                //return Result.Success;
                ModelResult tempRes = new ModelResult(ResultCode.Success);
                tempRes.logMessage = Checks.GetAndClearLog();
                return tempRes;
            }

        }


        /// <summary>
        /// Substitute relative attribute key and value.
        /// </summary>
        /// <param name="substitution">The substitution of key and value pair.</param>
        /// <param name="s">A string.</param>
        /// <returns>Returns substitute string.</returns>
        string Substitute(Map<string, string> substitution, string s)
        {
            if (String.IsNullOrEmpty(s) || substitution == null)
            {
                return s;
            }
            foreach (KeyValuePair<string, string> kp in substitution)
            {
                s = s.Replace(kp.Key, kp.Value);
            }

            return s;
        }


        /// <summary>
        /// Adds a schema attribute.
        /// </summary>
        /// <param name="obj">The object of model.</param>
        void AddAttribute(ModelObject obj)
        {
            string attrId = (string)obj[StandardNames.attributeID];
            string displayName = ((string)obj[StandardNames.ldapDisplayName]).ToLower();
            Checks.IsTrue(
                !attributeMap.ContainsKey(displayName),
                "attribute '{0}' must not be declared twice", 
                displayName);
            attributeMap[displayName] = obj;
            Checks.IsTrue(
                !attributeIdToDisplayNameMap.ContainsKey(attrId), 
                "attribute '{0}' has ambigious id '{1}'", 
                displayName, 
                attrId);
            attributeIdToDisplayNameMap[attrId] = displayName;

            // Add object class if not defined by schema
            if (obj[StandardNames.objectClass] == null)
            {
                obj[StandardNames.objectClass] = new AttributeContext(
                    this,
                    StandardNames.objectClass, 
                    Syntax.StringObjectIdentifier, 
                    false,
                    null).Parse(StandardNames.top + ";" + StandardNames.attributeSchema);
            }

        }

        /// <summary>
        /// Adds a schema class.
        /// </summary>
        /// <param name="obj">The object of model.</param>
        void AddClass(ModelObject obj)
        {
            string governsId = (string)obj[StandardNames.governsId];
            string displayName = ((string)obj[StandardNames.ldapDisplayName]).ToLower();
            Checks.IsTrue(!classMap.ContainsKey(displayName), "class '{0}' must not be declared twice", displayName);
            classMap[displayName] = obj;
            Checks.IsTrue(
                !classGovernsIdToDisplayNameMap.ContainsKey(governsId), 
                "class '{0}' has ambigious id '{1}'", 
                displayName, 
                governsId);
            classGovernsIdToDisplayNameMap[governsId] = displayName;

            // Add object class if not defined by schema
            if (obj[StandardNames.objectClass] == null)
            {
                obj[StandardNames.objectClass] =new AttributeContext(
                    this,
                    StandardNames.objectClass, 
                    Syntax.StringObjectIdentifier, 
                    false, 
                    null).Parse(StandardNames.top + ";" + StandardNames.classSchema);
            }
        }

        /// <summary>
        /// The syntax of a set of attributes which are required to bootstrap the schema.
        /// </summary>
        MapContainer<string, AttributeContext> builtinAttributeSyntax;

        void InitializeBuiltinAttributes()
        {
            if (builtinAttributeSyntax != null)
            {
                return;
            }
            builtinAttributeSyntax = new MapContainer<string, AttributeContext>();
            builtinAttributeSyntax[StandardNames.attributeID.ToLower()] =
                new AttributeContext(this, StandardNames.attributeID, Syntax.Lookup("2.5.5.2", 6, null));
            builtinAttributeSyntax[StandardNames.attributeSyntax.ToLower()] =
                new AttributeContext(this, StandardNames.attributeSyntax, Syntax.Lookup("2.5.5.2", 6, null));
            builtinAttributeSyntax[StandardNames.governsId.ToLower()] =
                new AttributeContext(this, StandardNames.governsId, Syntax.Lookup("2.5.5.2", 6, null));
            builtinAttributeSyntax[StandardNames.isSingleValued.ToLower()] =
                new AttributeContext(this, StandardNames.isSingleValued, Syntax.Boolean);
            builtinAttributeSyntax[StandardNames.ldapDisplayName.ToLower()] =
                new AttributeContext(this, StandardNames.ldapDisplayName, Syntax.Lookup("2.5.5.12", 64, null));
            builtinAttributeSyntax[StandardNames.objectClass.ToLower()] =
                new AttributeContext(this, StandardNames.objectClass, Syntax.Lookup("2.5.5.2", 6, null), false, null);
            builtinAttributeSyntax[StandardNames.oMObjectClass.ToLower()] =
                new AttributeContext(this, StandardNames.oMObjectClass, Syntax.Lookup("2.5.5.10", 4, null));
            builtinAttributeSyntax[StandardNames.oMSyntax.ToLower()] =
                new AttributeContext(this, StandardNames.oMSyntax, Syntax.Lookup("2.5.5.9", 2, null));
            builtinAttributeSyntax[StandardNames.rDNAttID.ToLower()] =
                new AttributeContext(this, StandardNames.rDNAttID, Syntax.Lookup("2.5.5.2", 6, null));
            builtinAttributeSyntax[StandardNames.subClassOf.ToLower()] =
                new AttributeContext(this, StandardNames.subClassOf, Syntax.Lookup("2.5.5.2", 6, null), false, null);
            builtinAttributeSyntax[StandardNames.cn.ToLower()] =
                new AttributeContext(this, StandardNames.cn, Syntax.Lookup("2.5.5.12", 64, null));
            builtinAttributeSyntax[StandardNames.schemaIDGUID.ToLower()] =
                new AttributeContext(this, StandardNames.schemaIDGUID, Syntax.Lookup("2.5.5.10", 4, null));
            builtinAttributeSyntax[StandardNames.systemOnly.ToLower()] =
                new AttributeContext(this, StandardNames.systemOnly, Syntax.Boolean);
            builtinAttributeSyntax[StandardNames.rangeLower.ToLower()] =
                new AttributeContext(this, StandardNames.rangeLower, Syntax.Lookup("2.5.5.9", 2, null));
            builtinAttributeSyntax[StandardNames.rangeUpper.ToLower()] =
                new AttributeContext(this, StandardNames.rangeUpper, Syntax.Lookup("2.5.5.9", 2, null));
            builtinAttributeSyntax[StandardNames.mAPIID.ToLower()] =
               new AttributeContext(this, StandardNames.mAPIID, Syntax.Lookup("2.5.5.9", 2, null));
            builtinAttributeSyntax[StandardNames.isMemberOfPartialAttributeSet.ToLower()] =
                new AttributeContext(this, StandardNames.isMemberOfPartialAttributeSet, Syntax.Boolean);
            builtinAttributeSyntax[StandardNames.extendedCharsAllowed.ToLower()] =
                new AttributeContext(this, StandardNames.extendedCharsAllowed, Syntax.Boolean);
            builtinAttributeSyntax[StandardNames.linkID.ToLower()] =
                new AttributeContext(this, StandardNames.linkID, Syntax.Lookup("2.5.5.9", 2, null));
            builtinAttributeSyntax[StandardNames.mayContain.ToLower()] =
                 new AttributeContext(this, StandardNames.mayContain, Syntax.Lookup("2.5.5.2", 6, null), false, null);
            builtinAttributeSyntax[StandardNames.systemMayContain.ToLower()] =
                new AttributeContext(this, StandardNames.systemMayContain, Syntax.Lookup("2.5.5.2", 6, null), false, null);
            builtinAttributeSyntax[StandardNames.possSuperiors.ToLower()] =
                new AttributeContext(this, StandardNames.possSuperiors, Syntax.Lookup("2.5.5.2", 6, null), false, null);
            builtinAttributeSyntax[StandardNames.defaultHidingValue.ToLower()] =
                new AttributeContext(this, StandardNames.defaultHidingValue, Syntax.Boolean);
            builtinAttributeSyntax[StandardNames.defaultObjectCategory.ToLower()] =
                new AttributeContext(this, StandardNames.defaultObjectCategory, Syntax.Lookup("2.5.5.1", 127, "1.3.12.2.1011.28.0.714"));          
        }

        #region Syntax for unresolved values

        /// <summary>
        /// A syntax which is used for values which haven't yet been parsed during bootstrap of the schema.
        /// </summary>
        public class UnresolvedSyntax : StringBasedSyntax
        {
            /// <summary>
            /// The attribute value is empty during bootstrap of the schema.
            /// </summary>
            public override string AttributeSyntax
            {
                get { return String.Empty; }
            }

            /// <summary>
            /// The OM value is empty during bootstrap of the schema.
            /// </summary>
            public override int OMSyntax
            {
                get { return 0; }
            }
        }

        #endregion


    }
}
