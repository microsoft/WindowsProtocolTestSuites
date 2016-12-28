// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// A class defining standard names.
    /// </summary>
    public static class StandardNames
    {
        /// <summary>
        /// Standard name of the attributeSchema.
        /// </summary>
        public const string attributeSchema = "attributeSchema";

        /// <summary>
        /// Standard name of the attributeSchemaGovernsId.
        /// </summary>
        public const string attributeSchemaGovernsId = "1.2.840.113556.1.3.14";

        /// <summary>
        /// Standard name of the attributeID.
        /// </summary>
        public const string attributeID = "attributeID";

        /// <summary>
        /// Standard name of the attributeSyntax.
        /// </summary>
        public const string attributeSyntax = "attributeSyntax";

        /// <summary>
        /// Standard name of the cn.
        /// </summary>
        public const string cn = "cn";

        /// <summary>
        /// Standard name of the classSchema.
        /// </summary>
        public const string classSchema = "classSchema";

        /// <summary>
        /// Standard name of the classSchemaGovernsId.
        /// </summary>
        public const string classSchemaGovernsId = "1.2.840.113556.1.3.13";

        /// <summary>
        /// Standard name of the configuration.
        /// </summary>
        public const string configuration = "configuration";

        /// <summary>
        /// Standard name of the configurationGovernsId.
        /// </summary>
        public const string configurationGovernsId = "1.2.840.113556.1.5.12";

        /// <summary>
        /// Standard name of the defaultHidingValue.
        /// </summary>
        public const string defaultHidingValue = "defaultHidingValue";

        /// <summary>
        /// Standard name of the defaultObjectCategory.
        /// </summary>
        public const string defaultObjectCategory = "defaultObjectCategory";

        /// <summary>
        /// Standard name of the distinguishedName.
        /// </summary>
        public const string distinguishedName = "distinguishedName";
        
        /// <summary>
        /// Standard name of the dMD.
        /// </summary>
        public const string dMD = "dMD";

        /// <summary>
        /// Standard name of the dMDGovernsId.
        /// </summary>
        public const string dMDGovernsId = "1.2.840.113556.1.3.9";

        /// <summary>
        /// Standard name of the domainDNS.
        /// </summary>
        public const string domainDNS = "domainDNS";

        /// <summary>
        /// Standard name of the domainDNSGovernsId.
        /// </summary>
        public const string domainDNSGovernsId = "1.2.840.113556.1.5.67";

        /// <summary>
        /// Standard name of the extendedCharsAllowed.
        /// </summary>
        public const string extendedCharsAllowed = "extendedCharsAllowed";

        /// <summary>
        /// Standard name of the governsId.
        /// </summary>
        public const string governsId = "governsId";

        /// <summary>
        /// Standard name of the isMemberOfPartialAttributeSet.
        /// </summary>
        public const string isMemberOfPartialAttributeSet = "isMemberOfPartialAttributeSet";

        /// <summary>
        /// Standard name of the isSingleValued.
        /// </summary>
        public const string isSingleValued = "isSingleValued";

        /// <summary>
        /// Standard name of the ldapDisplayName.
        /// </summary>
        public const string ldapDisplayName = "ldapDisplayName";

        /// <summary>
        /// Standard name of the linkID.
        /// </summary>
        public const string linkID = "linkID";

        /// <summary>
        /// Standard name of the mAPIID.
        /// </summary>
        public const string mAPIID = "mAPIID";

        /// <summary>
        /// Standard name of the mayContain.
        /// </summary>
        public const string mayContain = "mayContain";

        /// <summary>
        /// Standard name of the mustContain.
        /// </summary>
        public const string mustContain = "mustContain";

        /// <summary>
        /// Standard name of the name.
        /// </summary>
        public const string name = "name";

        /// <summary>
        /// Standard name of the objectClass.
        /// </summary>
        public const string objectClass = "objectClass";

        /// <summary>
        /// Standard name of the objectClassCategory.
        /// </summary>
        public const string objectClassCategory = "objectClassCategory";

        /// <summary>
        /// Standard name of the objectGUID.
        /// </summary>
        public const string objectGUID = "objectGUID";

        /// <summary>
        /// Standard name of the objectSid.
        /// </summary>
        public const string objectSid = "objectSid";

        /// <summary>
        /// Standard name of the oMSyntax.
        /// </summary>
        public const string oMSyntax = "oMSyntax";

        /// <summary>
        /// Standard name of the oMObjectClass.
        /// </summary>
        public const string oMObjectClass = "oMObjectClass";

        /// <summary>
        /// Standard name of the parent.
        /// </summary>
        public const string parent = "parent";

        /// <summary>
        /// Standard name of the possSuperiors.
        /// </summary>
        public const string possSuperiors = "possSuperiors";

        /// <summary>
        /// Standard name of the rangeLower.
        /// </summary>
        public const string rangeLower = "rangeLower";

        /// <summary>
        /// Standard name of the rangeUpper.
        /// </summary>
        public const string rangeUpper = "rangeUpper";

        /// <summary>
        /// Standard name of the rDNAttID.
        /// </summary>
        public const string rDNAttID = "rDNAttID";

        /// <summary>
        /// Standard name of the rdnType.
        /// </summary>
        public const string rdnType = "rdnType";

        /// <summary>
        /// Standard name of the schemaIDGUID.
        /// </summary>
        public const string schemaIDGUID = "schemaIDGUID";

        /// <summary>
        /// Standard name of the shortName.
        /// </summary>
        public const string shortName = "$shortName"; // internal attribute

        /// <summary>
        /// Standard name of the subClassOf.
        /// </summary>
        public const string subClassOf = "subClassOf";

        /// <summary>
        /// Standard name of the systemFlags.
        /// </summary>
        public const string systemFlags = "systemFlags";

        /// <summary>
        /// Standard name of the systemMayContain.
        /// </summary>
        public const string systemMayContain = "systemMayContain";

        /// <summary>
        /// Standard name of the systemMustContain.
        /// </summary>
        public const string systemMustContain = "systemMustContain";

        /// <summary>
        /// Standard name of the systemPossSuperiors.
        /// </summary>
        public const string systemPossSuperiors = "systemPossSuperiors";

        /// <summary>
        /// Standard name of the systemOnly.
        /// </summary>
        public const string systemOnly = "systemOnly";

        /// <summary>
        /// Standard name of the top.
        /// </summary>
        public const string top = "top";
        /// <summary>
        /// Standard name of the topGovernsId.
        /// </summary>
        public const string topGovernsId = "2.5.6.0";

        /// <summary>
        /// Standard name of the possibleInferiors.
        /// </summary>
        public const string possibleInferiors = "possibleInferiors";
    }


    /// <summary>
    /// A set of helper methods for dealing with distinguished names etc.
    /// </summary>
    public static class NameHelper
    {
        /// <summary>
        /// Normalizes a distinguished name.
        /// </summary>
        /// <param name="name">The name of DN.</param>
        /// <returns>Returns normalized DN.</returns>
        public static string NormalizeDN(string name)
        {
             return name.Replace(" ", String.Empty);
        }

        /// <summary>
        /// Creates a DN from RDN and parent DN. The parent DN can be null.
        /// </summary>
        /// <param name="rdn">The name of RDN.</param>
        /// <param name="parent">Parent DN.</param>
        /// <returns>Returns DN.</returns>
        public static string MakeDN(string rdn, string parent)
        {
            if (parent == null)
            {
                return rdn;
            }
            else
            {
                return rdn + "," + parent;
            }
        }


        /// <summary>
        /// Returns the parent DN of the given DN, or null if it has no parent.
        /// </summary>
        /// <param name="name">The name of parent DN.</param>
        /// <returns>Returns parnet DN.</returns>
        public static string GetDNParent(string name)
        {
            int i = name.IndexOf(",");

            if (i >= 0)
            {
                return name.Substring(i + 1);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the RDN of the given DN. 
        /// </summary>
        /// <param name="name">The name of DN.</param>
        /// <returns>Returns the RDN of the given DN.</returns>
        public static string GetDNRDN(string name)
        {
            int i = name.IndexOf(",");

            if (i >= 0)
            {
                return name.Substring(0, i);
            }
            else
            {
                return name;
            }
        }


        /// <summary>
        /// Returns the DNS of the given DN. If "dc=n1,dc=n2" is the DN, the DNS is "n1.n2" (and accordingly
        /// for arbitrary number of components). Throws exception if name components are not "dc=...".
        /// </summary>
        /// <param name="name">The name od DN.</param>
        /// <returns>Returns the DNS of the given DN.</returns>
        public static string GetDNDNS(string name)
        {
            StringBuilder res = new StringBuilder();

            while (true)
            {
                string rdn = GetDNRDN(name);

                if (rdn.StartsWith("dc="))
                {
                    res.Append(rdn.Substring("dc=".Length));
                }
                name = GetDNParent(name);

                if (name == null)
                {
                    break;
                }
                else
                {
                    res.Append(".");
                }
            }

            return res.ToString();
        }
    }
}
