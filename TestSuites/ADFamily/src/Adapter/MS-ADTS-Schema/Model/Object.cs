// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// The representation of AD objects. These are generically represented as a mapping
    /// from attribute LDAP display names to values. An AD object can be used in indexer notation 
    /// (i.e. obj[name] or obj[name] = value). The AD object model provides a 
    /// generic mechanism to deal with constructed attributes, by adding a delegate 
    /// which is used to compute the value of that attribute.
    /// </summary>
    public class ModelObject
    {

        #region State

        /// <summary>
        /// The domain controller where this object lives.
        /// </summary>
        public ModelDomainController dc;

        /// <summary>
        /// The parent object.
        /// </summary>
        public ModelObject parent;

        /// <summary>
        /// The child objects.
        /// </summary>
        public MapContainer<string, ModelObject> childs = new MapContainer<string, ModelObject>();

        /// <summary>
        /// The defined (non-constructed) attributes. In order to access
        /// an attribute value processing constructed attributes, use indexer.
        /// </summary>
        public MapContainer<string, Value> attributes = new MapContainer<string, Value>();

        #endregion

        #region Printing

        /// <summary>
        /// Print the object's definition to the given text writer.
        /// </summary>
        /// <param name="writer">The writer of indented text.</param>
        public void Print(IndentedTextWriter writer)
        {
            writer.WriteLine("object {");
            writer.Indent += 2;

            foreach (string key in constructedAttributeGetters.Keys)
            {
                writer.Write(key);
                writer.Write(" = ");
                Value value = constructedAttributeGetters[key](this);
                    writer.WriteLine(value);
            }
            foreach (string key in attributes.Keys)
            {
                writer.Write(key);
                writer.Write(" : ");
                Value value = attributes[key];
                try
                {
                    AttributeContext context = dc.GetAttributeContext(key);
                    writer.WriteLine(context.syntax.Unparse(context, value));
                }
                catch
                {
                    writer.WriteLine(value);
                }
            }
            foreach (ModelObject obj in childs.Values)
            {
                obj.Print(writer);
            }
            writer.Indent -= 2;
            writer.WriteLine("}");
        }

        /// <summary>
        /// ToString method of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return this[StandardNames.shortName].ToString();            
        }           

        #endregion

        #region Access

        /// <summary>
        /// Indexer for attribute selection, processing constructed attributes.
        /// </summary>
        /// <param name="name">The display name of the attribute</param>
        /// <returns>The value of the attribute which is computed if constructed or retrieved from the 
        /// attribute list otherwise. Is null if the attribute has not value associated.</returns>
        public Value this[string name]
        {
            get
            {
                name = name.ToLower();
                ConstructedAttributeGetter getter;
                if (constructedAttributeGetters.TryGetValue(name, out getter))
                {
                    // This is a constructed attribute and has a special meaning
                    return getter(this);
                }
                else
                {
                    // This is a stored attribute and is handled generically
                    Value result;
                    if (attributes.TryGetValue(name, out result))
                    {
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            set
            {
                name = name.ToLower();
                Assert.IsTrue(
                    !constructedAttributeGetters.ContainsKey(name),
                    "constructed attribute '{0}' cannot be set", 
                    name);
                attributes[name] = value;
            }
        }

        /// <summary>
        /// Accesses an attribute value which must be defined.
        /// </summary>
        /// <param name="name">The name of required attribute.</param>
        /// <returns>Returns the value of required attribute.</returns>
        public Value GetRequiredAttributeValue(string name)
        {
            var result = this[name];
            Assert.IsTrue(result != null, "object must have defined attribute '{0}'", name);
            return result;
        }

        /// <summary>
        /// Gets the object class of this object.
        /// </summary>
        /// <returns>Returns structural class id.</returns>
        public string GetStructuralClassId()
        {
            Value value = GetRequiredAttributeValue(StandardNames.objectClass);
            return (string)value.UnderlyingValues[value.UnderlyingValues.Count - 1];
        }

        /// <summary>
        /// Gets the object classes of this object.
        /// </summary>
        /// <returns>Returns an sequence of all class ids.</returns>
        public Sequence<string> GetAllClassIds()
        {
            return from value in GetRequiredAttributeValue(StandardNames.objectClass).UnderlyingValues
                select (string)value;
        }

        #endregion

        #region Registering constructed attribute getters

        private static MapContainer<string, ConstructedAttributeGetter> constructedAttributeGetters =
            new MapContainer<string, ConstructedAttributeGetter>();

        /// <summary>
        /// Registers a getter for a constructed attribute.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="getter">The delegate to invoke to compute the attribute value.</param>
        public static void RegisterConstructedAttribute(string name, ConstructedAttributeGetter getter)
        {
            constructedAttributeGetters[name.ToLower()] = getter;
        }

        static ModelObject()
        {
            ConstructedAttributes.Install();
        }

        #endregion
    }

    /// <summary>
    /// A delegate which defines the semantics of a constructed attribute.
    /// </summary>
    /// <param name="obj">Model object</param>
    /// <returns>Value of the constructed attribute.</returns>
    public delegate Value ConstructedAttributeGetter(ModelObject obj);
}
