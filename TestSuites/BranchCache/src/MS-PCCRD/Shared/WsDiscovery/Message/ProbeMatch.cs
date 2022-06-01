// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The class of the probe match message.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public partial class ProbeMatchType
    {
        /// <summary>
        /// Endpoint reference
        /// </summary>
        private EndpointReferenceType endpointReferenceField;

        /// <summary>
        /// Probe match type
        /// </summary>
        private string typesField;

        /// <summary>
        /// Matched scopes type
        /// </summary>
        private ScopesType scopesField;

        /// <summary>
        /// XAddress value
        /// </summary>
        private string xAddrsField;

        /// <summary>
        /// Metadata version
        /// </summary>
        private uint metadataVersionField;

        /// <summary>
        /// Any element
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlElement[] anyField;

        /// <summary>
        /// Any attribute
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlAttribute[] anyAttrField;

        /// <summary>
        /// Gets or sets Endpoint reference
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing", Order = 0)]
        public EndpointReferenceType EndpointReference
        {
            get
            {
                return this.endpointReferenceField;
            }

            set
            {
                this.endpointReferenceField = value;
            }
        }

        /// <summary>
        /// Gets or sets Probe match type
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Types
        {
            get
            {
                return this.typesField;
            }

            set
            {
                this.typesField = value;
            }
        }

        /// <summary>
        /// Gets or sets Matched scopes type
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ScopesType Scopes
        {
            get
            {
                return this.scopesField;
            }

            set
            {
                this.scopesField = value;
            }
        }

        /// <summary>
        /// Gets or sets XAddress value
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string XAddrs
        {
            get
            {
                return this.xAddrsField;
            }

            set
            {
                this.xAddrsField = value;
            }
        }

        /// <summary>
        /// Gets or sets Metadata version
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public uint MetadataVersion
        {
            get
            {
                return this.metadataVersionField;
            }

            set
            {
                this.metadataVersionField = value;
            }
        }

        /// <summary>
        /// Gets or sets Any element
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
        /// Gets or sets Any attribute
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
