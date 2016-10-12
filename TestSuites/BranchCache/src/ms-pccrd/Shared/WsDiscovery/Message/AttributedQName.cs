// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The struct of the attrbuted qualified name.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class AttributedQName
    {
        /// <summary>
        /// Collection of all filed attributes
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlAttribute[] anyAttrField;

        /// <summary>
        /// Attrbuted qualified name
        /// </summary>
        private System.Xml.XmlQualifiedName valueField;

        /// <summary>
        /// Gets or sets the Collection of all filed attributes
        /// </summary>
        [System.Xml.Serialization.XmlAnyAttributeAttribute]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }

            set
            {
                this.anyAttrField = value;
            }
        }

        /// <summary>
        /// Gets or sets the Attributed Qualified Name
        /// </summary>
        [System.Xml.Serialization.XmlTextAttribute]
        public System.Xml.XmlQualifiedName Value
        {
            get
            {
                return this.valueField;
            }

            set
            {
                this.valueField = value;
            }
        }
    }
}

