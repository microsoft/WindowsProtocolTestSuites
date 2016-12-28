// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class DirsyncExRep : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer moreResults { get; set; }

        [Asn1Field(1)]
        public Asn1Integer unused { get; set; }

        [Asn1Field(2)]
        public Asn1OctetString cookieServer { get; set; }

        public DirsyncExRep()
        {
            moreResults = null;
            unused = null;
            cookieServer = null;
        }

        /// <summary>
        /// This constructor sets all elements to references to the 
        /// given objects
        /// </summary>
        public DirsyncExRep(Asn1Integer moreResults_, Asn1Integer unused_, Asn1OctetString cookieServer_)
        {
            moreResults = moreResults_;
            unused = unused_;
            cookieServer = cookieServer_;
        }

        /// <summary>
        /// This constructor allows primitive data to be passed for all 
        /// primitive elements.  It will create new object wrappers for 
        /// the primitive data and set other elements to references to 
        /// the given objects 
        /// </summary>
        public DirsyncExRep(long moreResults_, long unused_, byte[] cookieServer_)
            : base()
        {
            moreResults = new Asn1Integer(moreResults_);
            unused = new Asn1Integer(unused_);
            cookieServer = new Asn1OctetString(cookieServer_);
        }
    }
}
