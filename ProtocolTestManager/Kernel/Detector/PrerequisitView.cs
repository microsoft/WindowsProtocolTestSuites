// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the view of prerequisit.
    /// </summary>
    public class PrerequisitView
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<PrerequisitProperty> Properties { get; set; }
    }

    /// <summary>
    /// This class defines the property structure of prerequisit.
    /// </summary>
    public class PrerequisitProperty
    {
        /// <summary>
        /// Name of property.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Type of property, Text or Choice.
        /// </summary>
        public string PropertyType
        {
            get
            {
                if (PropertyValues == null || PropertyValues.Count <= 1)
                    return "Text";
                return "Choice";
            }
        }
        private List<string> propertyValues;

        public List<string> PropertyValues
        {
            get { return propertyValues; }
            set
            {
                propertyValues = value;
                if (PropertyValues != null && PropertyValues.Count > 0)
                    Value = propertyValues[0];
                else Value = "";

            }
        }
        public string Value { get; set; }
    }
}
