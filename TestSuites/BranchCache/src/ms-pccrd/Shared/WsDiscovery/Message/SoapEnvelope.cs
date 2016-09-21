// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// The version of the soap message
    /// </summary>
    public enum SoapMessageVersion
    {
        /// <summary>
        /// Default value
        /// </summary>
        Default = 0,

        /// <summary>
        /// Represents Soap 1.1
        /// </summary>
        Soap11 = 1,

        /// <summary>
        /// Represents Soap 1.2
        /// </summary>
        Soap12 = 2,

        /// <summary>
        /// Represents Soap 1.1 and Ws-Addressing 1.0
        /// </summary>
        Soap11Wsa10 = 3,

        /// <summary>
        /// Soap 1.1 and Ws-Addressing submitted August 2004
        /// </summary>
        Soap11Wsa2004 = 4,

        /// <summary>
        /// Soap 1.2 and Ws-Addressing 1.0
        /// </summary>
        Soap12Wsa10 = 5,

        /// <summary>
        /// Soap 1.2 and Ws-Addressing submitted August 2004
        /// </summary>
        Soap12Wsa2004 = 6,
    }

    /// <summary>
    /// The elements of the soap envelope.
    /// </summary>
    [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class SoapEnvelope
    {
        /// <summary>
        /// The header of the soap message
        /// </summary>
        private SoapHeader header;

        /// <summary>
        /// The body of the soap message
        /// </summary>
        private SoapBody body;

        /// <summary>
        /// The version of the soap message
        /// </summary>
        private SoapMessageVersion messageVersion;

        /// <summary>
        /// Initializes a new instance of the SoapEnvelope class.
        /// </summary>
        /// <param name="messageVersion">The version of the soap message</param>
        /// <param name="header">The header of the soap message</param>
        /// <param name="body">The body of the soap message</param>
        public SoapEnvelope(SoapMessageVersion messageVersion, SoapHeader header, SoapBody body)
        {
            this.messageVersion = messageVersion;
            this.header = header;
            this.body = body;
        }

        /// <summary>
        /// Initializes a new instance of the SoapEnvelope class.
        /// </summary>
        /// <param name="body">The body of the soap message</param>
        public SoapEnvelope(SoapBody body)
        {
            this.header = new WsdHeader();
            this.body = body;
        }

        /// <summary>
        /// Initializes a new instance of the SoapEnvelope class.
        /// </summary>
        public SoapEnvelope()
        {
        }

        /// <summary>
        /// Gets or sets the header of the soap message
        /// </summary>
        public SoapHeader Header
        {
            get
            {
                return this.header;
            }

            set
            {
                this.header = value;
            }
        }

        /// <summary>
        /// Gets or sets the body of the soap message
        /// </summary>
        public SoapBody Body
        {
            get
            {
                return this.body;
            }

            set
            {
                this.body = value;
            }
        }

        /// <summary>
        /// Gets or sets the version of the soap message
        /// </summary>
        [XmlIgnore]
        public SoapMessageVersion MessageVersion
        {
            get
            {
                return this.messageVersion;
            }

            set
            {
                this.messageVersion = value;
            }
        }

        /// <summary>
        /// Serialize the soap message
        /// </summary>
        /// <param name="envelope">The soap envelope</param>
        /// <returns>Serialized value</returns>
        public static string Serialize(SoapEnvelope envelope)
        {
            string result = null;

            XmlAttributes attrs = new XmlAttributes();

            XmlElementAttribute attr = new XmlElementAttribute();
            attr.ElementName = "Body";
            attr.Type = envelope.body.GetType();
            attrs.XmlElements.Add(attr);

            XmlAttributes attrs2 = new XmlAttributes();
            XmlElementAttribute attr2 = new XmlElementAttribute();
            attr2.ElementName = "Header";
            attr2.Type = envelope.header.GetType();

            attrs2.XmlElements.Add(attr2);

            XmlAttributeOverrides attros = new XmlAttributeOverrides();
            attros.Add(typeof(SoapEnvelope), "Body", attrs);
            attros.Add(typeof(SoapEnvelope), "Header", attrs2);

            XmlSerializer a = new XmlSerializer(typeof(SoapEnvelope), attros);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("soap", "http://www.w3.org/2003/05/soap-envelope");
            ns.Add("wsa", "http://schemas.xmlsoap.org/ws/2004/08/addressing");
            ns.Add("wsd", "http://schemas.xmlsoap.org/ws/2005/04/discovery");
            ns.Add("PeerDist", "http://schemas.microsoft.com/p2p/2007/09/PeerDistributionDiscovery");

            using (MemoryStream ms = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);
                writer.Formatting = Formatting.None;
                a.Serialize(writer, envelope, ns);
                writer.Close();
                result = Encoding.UTF8.GetString(ms.ToArray());
            }

            return result;
        }
    }

    /// <summary>
    /// The header of the soap message
    /// </summary>
    [XmlRoot(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public abstract class SoapHeader
    {
    }

    /// <summary>
    /// The header of the wsdiscovery message
    /// </summary>
    [XmlRoot(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public class WsdHeader : SoapHeader
    {
        /// <summary>
        /// To URI
        /// </summary>
        private AttributedURI to = null;

        /// <summary>
        /// Action URI
        /// </summary>
        private AttributedURI action = null;

        /// <summary>
        /// Message ID
        /// </summary>
        private AttributedURI messageID = null;

        /// <summary>
        /// Reply to
        /// </summary>
        private EndpointReferenceType replyTo = null;

        /// <summary>
        /// Relates to
        /// </summary>
        private AttributedURI relatesTo = null;

        /// <summary>
        /// App sequence
        /// </summary>
        private AppSequenceType appSequence = null;

        /// <summary>
        /// Any element
        /// </summary>
        [System.NonSerialized]
        private XmlElement[] anyField;

        /// <summary>
        /// Any attribute
        /// </summary>
        [System.NonSerialized]
        private XmlAttribute[] anyAttrField;

        /// <summary>
        /// Initializes a new instance of the WsdHeader class.
        /// </summary>
        public WsdHeader()
        {
            this.to = new AttributedURI();
            this.to.Value = "urn:schemas-xmlsoap-org:ws:2005:04:discovery";
            this.action = new AttributedURI();
            this.action.Value = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe";
            this.messageID = new AttributedURI();
            this.messageID.Value = "urn:uuid:" + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the To URI
        /// </summary>
        public AttributedURI To
        {
            get
            {
                return this.to;
            }

            set
            {
                this.to = value;
            }
        }

        /// <summary>
        /// Gets or sets the Action URI
        /// </summary>
        public AttributedURI Action
        {
            get
            {
                return this.action;
            }

            set
            {
                this.action = value;
            }
        }

        /// <summary>
        /// Gets or sets the Message ID
        /// </summary>
        public AttributedURI MessageID
        {
            get
            {
                return this.messageID;
            }

            set
            {
                this.messageID = value;
            }
        }

        /// <summary>
        /// Gets or sets the Reply to
        /// </summary>
        public EndpointReferenceType ReplyTo
        {
            get
            {
                return this.replyTo;
            }

            set
            {
                this.replyTo = value;
            }
        }

        /// <summary>
        /// Gets or sets the Relates to
        /// </summary>
        public AttributedURI RelatesTo
        {
            get
            {
                return this.relatesTo;
            }

            set
            {
                this.relatesTo = value;
            }
        }

        /// <summary>
        /// Gets or sets the App sequence
        /// </summary>
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public AppSequenceType AppSequence
        {
            get
            {
                return this.appSequence;
            }

            set
            {
                this.appSequence = value;
            }
        }

        /// <summary>
        /// Gets or sets the Any element
        /// </summary>
        [XmlAnyElementAttribute]
        public XmlElement[] Any
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
        /// Gets or sets the Any attribute
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

    /// <summary>
    /// The body of the soap message
    /// </summary>
    public abstract class SoapBody
    {
    }
}
