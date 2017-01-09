// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class DirsyncExReq : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer flags { get; set; }

        [Asn1Field(1)]
        public Asn1Integer maxbytes { get; set; }

        [Asn1Field(2)]
        public Asn1OctetString cookie { get; set; }

        public DirsyncExReq()
        {
            flags = null;
            maxbytes = null;
            cookie = null;
        }

        /// <summary>
        /// This constructor sets all elements to references to the 
        /// given objects
        /// </summary>
        public DirsyncExReq(Asn1Integer flags_, Asn1Integer maxbytes_, Asn1OctetString cookie_)
        {
            flags = flags_;
            maxbytes = maxbytes_;
            cookie = cookie_;
        }

        /// <summary>
        /// This constructor allows primitive data to be passed for all 
        /// primitive elements.  It will create new object wrappers for 
        /// the primitive data and set other elements to references to 
        /// the given objects 
        /// </summary>
        public DirsyncExReq(long flags_, long maxbytes_, byte[] cookie_)
        {
            flags = new Asn1Integer(flags_);
            maxbytes = new Asn1Integer(maxbytes_);
            cookie = new Asn1OctetString(cookie_);
        }
    }
}
