// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The collection of the probe matche message.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public partial class ProbeMatchesType
    {
        /// <summary>
        /// The collection of the probe matche message.
        /// </summary>
        private ProbeMatchType[] probeMatchField;

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
        /// Gets or sets the collection of the probe matche.
        /// </summary>
        [XmlElementAttribute("ProbeMatch", Order = 0)]
        public ProbeMatchType[] ProbeMatch
        {
            get
            {
                return this.probeMatchField;
            }

            set
            {
                this.probeMatchField = value;
            }
        }

        /// <summary>
        /// Gets or sets Any element
        /// </summary>
        [XmlAnyElementAttribute(Order = 1)]
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
        [XmlAnyAttributeAttribute]
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
