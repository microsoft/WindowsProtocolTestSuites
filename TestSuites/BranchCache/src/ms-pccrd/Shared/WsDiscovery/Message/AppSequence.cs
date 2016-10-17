// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System.Xml;
    using System.Xml.Serialization;
    
    /// <summary>
    /// The enum of the app sequence type.
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public partial class AppSequenceType
    {
        /// <summary>
        /// Instance ID
        /// </summary>
        private uint instanceId;

        /// <summary>
        /// Sequence ID
        /// </summary>
        private string sequenceId;

        /// <summary>
        /// Message number
        /// </summary>
        private uint messageNumber;

        /// <summary>
        /// Collection of all filed attributes
        /// </summary>
        [System.NonSerialized]
        private XmlAttribute[] anyAttrFiled;

        /// <summary>
        /// Gets or sets the Instance ID
        /// </summary>
        [XmlAttribute]
        public uint InstanceId
        {
            get { return this.instanceId; }
            set { this.instanceId = value; }
        }

        /// <summary>
        /// Gets or sets the  Sequence ID
        /// </summary>
        [XmlAttribute(DataType = "anyURI")]
        public string SequenceId
        {
            get { return this.sequenceId; }
            set { this.sequenceId = value; }
        }

        /// <summary>
        /// Gets or sets the  Message number
        /// </summary>
        [XmlAttribute]
        public uint MessageNumber
        {
            get { return this.messageNumber; }
            set { this.messageNumber = value; }
        }

        /// <summary>
        /// Gets or sets the  Collection of all filed attributes
        /// </summary>
        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrFiled;
            }

            set
            {
                this.anyAttrFiled = value;
            }
        }
    }
}
