// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The collection of the ServiceNameType
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class ServiceNameType
    {
        /// <summary>
        /// Port name
        /// </summary>
        private string portNameField;

        /// <summary>
        /// The collection of the service name attribute.
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlAttribute[] anyAttrField;

        /// <summary>
        /// Service name value
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlQualifiedName valueField;

        /// <summary>
        /// Gets or sets Port name
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string PortName
        {
            get
            {
                return this.portNameField;
            }

            set
            {
                this.portNameField = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of the service name attribute.
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
        /// Gets or sets Service name value
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
