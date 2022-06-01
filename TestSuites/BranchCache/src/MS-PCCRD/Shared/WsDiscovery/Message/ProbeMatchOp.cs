// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage
{
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// The collection of the probe matche message.
    /// </summary>
    public class ProbeMatchOp : SoapBody
    {
        /// <summary>
        /// The collection of the probe matche message.
        /// </summary>
        private ProbeMatchesType probeMatches;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeMatchOp"/> class
        /// </summary>
        public ProbeMatchOp()
        {
           this.probeMatches = new ProbeMatchesType();
        }

        /// <summary>
        /// Gets or sets the collection of the probe matche message.
        /// </summary>
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public ProbeMatchesType ProbeMatches
        {
            get
            {
                return this.probeMatches;
            }

            set
            {
                this.probeMatches = value;
            }
        }
    }
}
