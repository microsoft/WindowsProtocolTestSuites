// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The class of the probe message.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public partial class ProbeType
    {
        /// <summary>
        /// Probe types
        /// </summary>
        private string typesField;

        /// <summary>
        /// Scopes type
        /// </summary>
        private ScopesType scopesField;

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
        /// Initializes a new instance of the <see cref="ProbeType"/> class without parameter
        /// </summary>
        public ProbeType()
        {
            this.scopesField = null;
            this.typesField = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeType"/> class
        /// </summary>
        /// <param name="types">Probe types</param>
        /// <param name="scopes">Scopes type</param>
        /// <param name="matchBy">Match by</param>
        public ProbeType(string types, string[] scopes, string matchBy)
        {
            this.typesField = types;
            this.scopesField = new ScopesType(scopes, matchBy);
        }

        /// <summary>
        /// Gets or sets Probe types
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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
        /// Gets or sets Scopes type
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
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
        /// Gets or sets Any element
        /// </summary>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
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
