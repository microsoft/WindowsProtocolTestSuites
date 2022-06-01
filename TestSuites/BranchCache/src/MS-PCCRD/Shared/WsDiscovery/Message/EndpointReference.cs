// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The struct of the endpoint refernce type.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class EndpointReferenceType
    {
        /// <summary>
        /// Reference address
        /// </summary>
        private AttributedURI addressField;

        /// <summary>
        /// Reference properties
        /// </summary>
        private ReferencePropertiesType referencePropertiesField;

        /// <summary>
        /// Reference parameters
        /// </summary>
        private ReferenceParametersType referenceParametersField;

        /// <summary>
        /// Port type
        /// </summary>
        private AttributedQName portTypeField;

        /// <summary>
        /// Service name
        /// </summary>
        private ServiceNameType serviceNameField;

        /// <summary>
        /// Any elements collecion
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlElement[] anyField;

        /// <summary>
        /// Any attribute collection
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlAttribute[] anyAttrField;

        /// <summary>
        /// Gets or sets the Reference address
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AttributedURI Address
        {
            get
            {
                return this.addressField;
            }

            set
            {
                this.addressField = value;
            }
        }

        /// <summary>
        /// Gets or sets the Reference properties
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ReferencePropertiesType ReferenceProperties
        {
            get
            {
                return this.referencePropertiesField;
            }

            set
            {
                this.referencePropertiesField = value;
            }
        }

        /// <summary>
        /// Gets or sets the Reference parameters
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ReferenceParametersType ReferenceParameters
        {
            get
            {
                return this.referenceParametersField;
            }

            set
            {
                this.referenceParametersField = value;
            }
        }

        /// <summary>
        /// Gets or sets the Port type
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public AttributedQName PortType
        {
            get
            {
                return this.portTypeField;
            }

            set
            {
                this.portTypeField = value;
            }
        }

        /// <summary>
        /// Gets or sets the Service name
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public ServiceNameType ServiceName
        {
            get
            {
                return this.serviceNameField;
            }

            set
            {
                this.serviceNameField = value;
            }
        }

        /// <summary>
        /// Gets or sets the Any elements collection
        /// </summary>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 5)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }

            set
            {
                this.anyField = value;
            }
        }

        /// <summary>
        /// Gets or sets the Any attribute collection
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
    }
}

