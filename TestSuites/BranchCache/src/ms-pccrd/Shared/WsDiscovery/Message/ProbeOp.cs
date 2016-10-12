// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage
{
    using System.Xml;
    using System.Xml.Serialization;
    
    /// <summary>
    /// The class of the probe message.
    /// </summary>
    public class ProbeOp : SoapBody
    {
        /// <summary>
        /// The class of the probe message.
        /// </summary>
        private ProbeType probe;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeOp"/> class
        /// </summary>
        /// <param name="probe">the probe message</param>
        public ProbeOp(ProbeType probe)
        {
            this.probe = probe;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeOp"/> class without parameter
        /// </summary>
        public ProbeOp()
        {
        }

        /// <summary>
        /// Gets or sets the class of the probe message.
        /// </summary>
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
        public ProbeType Probe
        {
            get
            {
                return this.probe;
            }

            set
            {
                this.probe = value;
            }
        }
    }
}
