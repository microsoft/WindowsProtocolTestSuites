// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Provides custom attributes for reflection objects that support them.
    /// </summary>
    internal class TypeCustomAttributeProvider : ICustomAttributeProvider
    {
        //attribute list
        private List<Attribute> attributeList;


        /// <summary>
        /// Initialize an instance of TypeCustomAttributeProvider.
        /// </summary>
        /// <param name="attributes">A list of custom attributes.</param>
        public TypeCustomAttributeProvider(params Attribute[] attributes)
        {
            attributeList = new List<Attribute>();
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] != null)
                {
                    attributeList.Add(attributes[i]);
                }
            }
        }


        #region ICustomAttributeProvider Members

        /// <summary>
        /// Returns an array of all of the custom attributes defined on this member, 
        /// excluding named attributes, or an empty array if there are no custom attributes.
        /// </summary>
        /// <param name="inherit">
        /// When true, look up the hierarchy chain for the inherited custom attribute. 
        /// Ignored.
        /// </param>
        /// <returns>
        /// An array of Objects representing custom attributes, or an empty array.
        /// </returns>
        public object[] GetCustomAttributes(bool inherit)
        {
            return attributeList.ToArray();
        }


        /// <summary>
        /// Returns an array of custom attributes defined on this member, 
        /// identified by type, or an empty array if there are no custom 
        /// attributes of that type.
        /// </summary>
        /// <param name="attributeType">
        /// The type of the custom attributes.
        /// </param>
        /// <param name="inherit">
        /// When true, look up the hierarchy chain for the inherited custom attribute. 
        /// Ignored.
        /// </param>
        /// <returns>
        /// An array of Objects representing custom attributes, or an empty array.
        /// </returns>
        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            List<object> resultAttributeList = new List<object>();
            for (int i = 0; i < attributeList.Count; i++)
            {
                if (attributeList[i].GetType() == attributeType)
                {
                    resultAttributeList.Add(attributeList[i]);
                }
            }
            return resultAttributeList.ToArray();
        }


        /// <summary>
        /// Indicates whether one or more instance of attributeType is defined on this member.
        /// </summary>
        /// <param name="attributeType">The type of the custom attributes.</param>
        /// <param name="inherit">
        /// When true, look up the hierarchy chain for the inherited custom attribute.
        /// </param>
        /// <returns>true if the attributeType is defined on this member; false otherwise.</returns>
        public bool IsDefined(Type attributeType, bool inherit)
        {
            for (int i = 0; i < attributeList.Count; i++)
            {
                if (attributeList[i].GetType() == attributeType)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }

}
