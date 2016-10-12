// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The collection of the scope.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public partial class ScopesType
    {
        /// <summary>
        /// Match by
        /// </summary>
        private string matchByField;

        /// <summary>
        /// The collection of the scope attribute.
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlAttribute[] anyAttrField;

        /// <summary>
        /// Text value
        /// </summary>
        private string[] textField;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopesType"/> class without parameter
        /// </summary>
        public ScopesType()
        {
            this.matchByField = null;
            this.textField = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopesType"/> class
        /// </summary>
        /// <param name="scopes">Scopes value</param>
        public ScopesType(string[] scopes)
        {
            this.textField = scopes;
            this.matchByField = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopesType"/> class
        /// </summary>
        /// <param name="scopes">Scopes value</param>
        /// <param name="matchBy">Match by</param>
        public ScopesType(string[] scopes, string matchBy)
        {
            this.textField = scopes;
            this.matchByField = matchBy;
        }

        /// <summary>
        /// Gets or sets Match by
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string MatchBy
        {
            get
            {
                return this.matchByField;
            }

            set
            {
                this.matchByField = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of the scope attribute.
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
        /// Gets or sets text value
        /// </summary>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
        public string[] Text
        {
            get
            {
                return this.textField;
            }

            set
            {
                this.textField = value;
            }
        }
    }
}
