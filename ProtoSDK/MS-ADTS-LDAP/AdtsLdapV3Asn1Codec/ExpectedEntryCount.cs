// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class ExpectedEntryCount : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer searchEntriesMin { get; set; }

        [Asn1Field(1)]
        public Asn1Integer searchEntriesMax { get; set; }

        public ExpectedEntryCount()
        {
            searchEntriesMin = null;
            searchEntriesMax = null;
        }

        /// <summary>
        /// This constructor sets all elements to references to the 
        /// given objects
        /// </summary>
        public ExpectedEntryCount(Asn1Integer searchEntriesMin_, Asn1Integer searchEntriesMax_)
        {
            searchEntriesMin = searchEntriesMin_;
            searchEntriesMax = searchEntriesMax_;
        }

        /// <summary>
        /// This constructor allows primitive data to be passed for all 
        /// primitive elements.  It will create new object wrappers for 
        /// the primitive data and set other elements to references to 
        /// the given objects 
        /// </summary>
        public ExpectedEntryCount(long searchEntriesMin_, long searchEntriesMax_)
        {
            searchEntriesMin = new Asn1Integer(searchEntriesMin_);
            searchEntriesMax = new Asn1Integer(searchEntriesMax_);
        }
    }
}
