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
    /// Represents an LDAP filter expression.
    /// </summary>
    public abstract class Filter : CompoundValue
    {
        /// <summary>
        /// Determines match of object against filter.
        /// </summary>
        /// <param name="obj">The object of model.</param>
        /// <returns>Returns whether match of object against filter.</returns>
        public abstract bool Match(ModelObject obj);


        /// <summary>
        /// The And filter.
        /// </summary>
        public class And : Filter
        {
            /// <summary>
            /// OperandsAnd
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
            public readonly Sequence<Filter> operandsAnd;

            /// <summary>
            /// And filter
            /// </summary>
            /// <param name="operands">Filter operands</param>
            public And(params Filter[] operands)
            {
                operandsAnd = new Sequence<Filter>(operands);
            }

            /// <summary>
            /// Match method.
            /// </summary>
            /// <param name="obj">The second value object.</param>
            /// <returns>On success it returns true.</returns>
            public override bool Match(ModelObject obj)
            {
                foreach (Filter filter in operandsAnd)
                {
                    if (!filter.Match(obj))
                    {
                        return false;
                    }
                }

                return true;
            }
        }


        /// <summary>
        /// The Or filter.
        /// </summary>
        public class Or : Filter
        {
            /// <summary>
            /// OperandsOr
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
            public readonly Sequence<Filter> operandsOr;

            /// <summary>
            /// Or method.
            /// </summary>
            /// <param name="operands">Or operation operands.</param>
            public Or(params Filter[] operands)
            {
                operandsOr = new Sequence<Filter>(operands);
            }

            /// <summary>
            /// Mathc method.
            /// </summary>
            /// <param name="obj">The second value.</param>
            /// <returns>On success it returns true.</returns>
            public override bool Match(ModelObject obj)
            {
                foreach (Filter filter in operandsOr)
                {
                    if (filter.Match(obj))
                    {
                        return true;
                    }
                }

                return false;
            }
        }


        /// <summary>
        /// The Not filter.
        /// </summary>
        public class Not : Filter
        {
            /// <summary>
            /// operandsNot
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
            public readonly Filter operandsNot;

            /// <summary>
            /// Not operation
            /// </summary>
            /// <param name="operand">Operand</param>
            public Not(Filter operand)
            {
                operandsNot = operand;
            }

            /// <summary>
            /// Mathc method.
            /// </summary>
            /// <param name="obj">The second value.</param>
            /// <returns>On success it returns true.</returns>
            public override bool Match(ModelObject obj)
            {
                return !operandsNot.Match(obj);
            }
        }


        /// <summary>
        /// The equality match filter.
        /// </summary>
        public class EqualityMatch : Filter
        {
            /// <summary>
            /// AttributeForEqual
            /// </summary>
            public readonly string attributeForEqual;

            /// <summary>
            /// ValueForEqual
            /// </summary>
            public readonly string valueForEqual;

            /// <summary>
            /// Equality match.
            /// </summary>
            /// <param name="attribute">Attribute for equal.</param>
            /// <param name="value">The value of the attribute.</param>
            public EqualityMatch(string attribute, string value)
            {
                this.attributeForEqual = attribute;
                this.valueForEqual = value;
            }

            /// <summary>
            /// Mathc method.
            /// </summary>
            /// <param name="obj">The second value.</param>
            /// <returns>On success it returns true.</returns>
            public override bool Match(ModelObject obj)
            {
               Value assignedValue = obj[attributeForEqual];

               if (assignedValue == null)
               {
                   return false;
               }
                AttributeContext context = obj.dc.GetAttributeContext(attributeForEqual);
                Value parsedValue = context.Parse(valueForEqual);
                return context.Equals(parsedValue, assignedValue);
            }
        }

        /// <summary>
        /// The substrings filter.
        /// </summary>
        public class Substrings : Filter
        {
            /// <summary>
            /// AttributeForSubStr
            /// </summary>
            public readonly string attributeForSubStr;

            /// <summary>
            /// InitialForSubStr
            /// </summary>
            public readonly string initialForSubStr;

            /// <summary>
            /// Any
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
            public readonly Sequence<string> any;

            /// <summary>
            /// FinalForSubStr
            /// </summary>
            public readonly string finalForSubStr;

            /// <summary>
            /// Substrings
            /// </summary>
            /// <param name="attribute">Attribute</param>
            /// <param name="initial">Initial</param>
            /// <param name="any">Any</param>
            /// <param name="final">Final</param>
            public Substrings(string attribute, string initial, string[] any, string final)
            {
                this.attributeForSubStr = attribute;
                this.initialForSubStr = initial;
                this.any = new Sequence<string>(any);
                this.finalForSubStr = final;
            }

            /// <summary>
            /// Substrings
            /// </summary>
            /// <param name="attribute">Attribute</param>
            /// <param name="initial">Initial</param>
            public Substrings(string attribute, string initial)
            {
                this.attributeForSubStr = attribute;
                this.initialForSubStr = initial;
                this.any = null;
                this.finalForSubStr = null;
            }

            /// <summary>
            /// Match
            /// </summary>
            /// <param name="obj">The object of model.</param>
            /// <returns>Returns true if the object matches.</returns>
            public override bool Match(ModelObject obj)
            {
                Value assignedValue = obj[attributeForSubStr];
                if (assignedValue == null)
                {
                    return false;
                }
                string stringRepr = (string)assignedValue;
                if (stringRepr == null)
                {
                    return false;
                }
                stringRepr = stringRepr.ToLower();
                if (initialForSubStr != null && !stringRepr.StartsWith(initialForSubStr.ToLower()))
                {
                    return false;
                }
                if (finalForSubStr != null && !stringRepr.EndsWith(finalForSubStr.ToLower()))
                {
                    return false;
                }
                if (any != null)
                {
                    foreach (string s in any)
                    {
                        if (stringRepr.IndexOf(s.ToLower()) < 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// The presence filter.
        /// </summary>
        public class Present : Filter
        {
            /// <summary>
            /// Attribute Presence.
            /// </summary>
            public readonly string attributePresence;

            /// <summary>
            /// Justify whether the attribute is present.
            /// </summary>
            /// <param name="attribute">Attribute.</param>
            public Present(string attribute)
            {
                this.attributePresence = attribute;
            }


            /// <summary>
            /// Justify whether the object matches the present attribute.
            /// </summary>
            /// <param name="obj">The object of model.</param>
            /// <returns>Returns true if the object matches.</returns>
            public override bool Match(ModelObject obj)
            {
                return obj[attributePresence] != null;
            }
        }

    }


    /// <summary>
    /// Represents LDAP search scope.
    /// </summary>
    public enum SearchScope
    {
        /// <summary>
        /// BaseObject
        /// </summary>
        BaseObject,

        /// <summary>
        /// SingleLevel
        /// </summary>
        SingleLevel,

        /// <summary>
        /// WholeTree
        /// </summary>
        WholeTree
    }

    /// <summary>
    /// A structure to represent the result of search.
    /// </summary>
    public class SearchResult : CompoundValue
    {
        /// <summary>
        /// ObjectName
        /// </summary>
        public string objectName;

        /// <summary>
        /// AttributeList
        /// </summary>
        public Map<string, string> attributeList;

        /// <summary>
        /// Search relative attributes for object name.
        /// </summary>
        /// <param name="objectName">The name of object.</param>
        /// <param name="attributeList">The list of attributes.</param>
        public SearchResult(string objectName, Map<string,string> attributeList)
        {
            this.objectName = objectName;
            this.attributeList = attributeList;
        }
    }


    public partial class ModelDomainController
    {

        /// <summary>
        /// Performs LDAP search.
        /// </summary>
        /// <param name="baseObjectName">The base name of object.</param>
        /// <param name="scope">The scope of search.</param>
        /// <param name="sizeLimit">The size of limit.</param>
        /// <param name="typesOnly">There are only types.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="selection">Selection.</param>
        /// <param name="searchResult">Search result.</param>
        /// <returns>Returns performing result of searching LDAP.</returns>
        public ModelResult Search(string baseObjectName, SearchScope scope, 
                int sizeLimit, bool typesOnly, Filter filter, Sequence<string> selection,
                out Sequence<SearchResult> searchResult)
        {
            searchResult = null;
            int count = 0;
            IEnumerable<ModelObject> objects;
            ModelObject obj = TryFindObject(baseObjectName);

            if (obj == null)
            {
                return new ModelResult(ResultCode.NoSuchObject, "cannot find '{0}'", baseObjectName);
            }
            switch (scope)
            {
                case SearchScope.BaseObject:
                    objects = new Sequence<ModelObject>(obj);
                    break;
                case SearchScope.SingleLevel:
                    objects = obj.childs.Values;
                    break;
                default:
                    objects = GetChildsAndObject(obj);
                    break;
            }
            searchResult = new Sequence<SearchResult>();
            foreach (ModelObject sobj in objects)
            {
                if (count >= sizeLimit)
                {
                    return new ModelResult(ResultCode.SizeLimitExceeded);
                }
                if (filter.Match(sobj))
                {
                    searchResult = searchResult.Add(SelectResult(sobj, typesOnly, selection));
                }
            }

            return ModelResult.Success;
        }

        SearchResult SelectResult(ModelObject obj, bool typesOnly, Sequence<string> selection)
        {
            Set<string> includedAttributes;

            if (selection.Count == 0)
            {
                includedAttributes = new Set<string>(obj.attributes.Keys);
            }
            else if (selection.Count == 1 && selection[0] == "1.1")
            {
                includedAttributes = new Set<string>();
            }
            else
            {
                includedAttributes = new Set<string>();

                foreach (string sel in selection)
                {
                    if (sel == "*")
                    {
                        includedAttributes = includedAttributes.Union(new Set<string>(obj.attributes.Keys));
                    }
                    else
                    {
                        includedAttributes = includedAttributes.Add(sel.ToLower().Trim());
                    }
                }
            }
            MapContainer<string, string> map = new MapContainer<string,string>();

            foreach (string attr in obj.attributes.Keys)
            {
                if (includedAttributes.Contains(attr))
                {
                    if (typesOnly)
                    {
                        map[attr] = null;
                    }
                    else
                        map[attr] = obj.attributes[attr].ToString();
                }
            }
            return new SearchResult((string)obj[StandardNames.distinguishedName], map.ToMap());
        }

    }
}
