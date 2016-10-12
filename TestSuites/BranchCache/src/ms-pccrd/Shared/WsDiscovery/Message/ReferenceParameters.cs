// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml.Serialization;

    /// <summary>
    /// The collection of the reference parameter.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class ReferenceParametersType
    {
        /// <summary>
        /// The collection of the reference parameter.
        /// </summary>
        [System.NonSerialized]
        private System.Xml.XmlElement[] anyField;

        /// <summary>
        /// Gets or sets the collection of the reference parameter.
        /// </summary>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
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
    }
}
