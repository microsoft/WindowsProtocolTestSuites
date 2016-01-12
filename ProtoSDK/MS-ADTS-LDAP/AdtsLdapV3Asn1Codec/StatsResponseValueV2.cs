// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	StatsResponseValueV2 ::= SEQUENCE {
	    threadCountTag        INTEGER
	    threadCount           INTEGER
	    callTimeTag           INTEGER
	    callTime              INTEGER
	    entriesReturnedTag    INTEGER
	    entriesReturned       INTEGER
	    entriesVisitedTag     INTEGER
	    entriesVisited        INTEGER
	    filterTag             INTEGER
	    filter                OCTET STRING
	    indexTag              INTEGER
	    index                 OCTET STRING
	}

    */
    public class StatsResponseValueV2 : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer threadCountTag { get; set; }
        
        [Asn1Field(1)]
        public Asn1Integer threadCount { get; set; }
        
        [Asn1Field(2)]
        public Asn1Integer callTimeTag { get; set; }
        
        [Asn1Field(3)]
        public Asn1Integer callTime { get; set; }
        
        [Asn1Field(4)]
        public Asn1Integer entriesReturnedTag { get; set; }
        
        [Asn1Field(5)]
        public Asn1Integer entriesReturned { get; set; }
        
        [Asn1Field(6)]
        public Asn1Integer entriesVisitedTag { get; set; }
        
        [Asn1Field(7)]
        public Asn1Integer entriesVisited { get; set; }
        
        [Asn1Field(8)]
        public Asn1Integer filterTag { get; set; }
        
        [Asn1Field(9)]
        public Asn1OctetString filter { get; set; }
        
        [Asn1Field(10)]
        public Asn1Integer indexTag { get; set; }
        
        [Asn1Field(11)]
        public Asn1OctetString index { get; set; }
        
        public StatsResponseValueV2()
        {
            this.threadCountTag = null;
            this.threadCount = null;
            this.callTimeTag = null;
            this.callTime = null;
            this.entriesReturnedTag = null;
            this.entriesReturned = null;
            this.entriesVisitedTag = null;
            this.entriesVisited = null;
            this.filterTag = null;
            this.filter = null;
            this.indexTag = null;
            this.index = null;
        }
        
        public StatsResponseValueV2(
         Asn1Integer threadCountTag,
         Asn1Integer threadCount,
         Asn1Integer callTimeTag,
         Asn1Integer callTime,
         Asn1Integer entriesReturnedTag,
         Asn1Integer entriesReturned,
         Asn1Integer entriesVisitedTag,
         Asn1Integer entriesVisited,
         Asn1Integer filterTag,
         Asn1OctetString filter,
         Asn1Integer indexTag,
         Asn1OctetString index)
        {
            this.threadCountTag = threadCountTag;
            this.threadCount = threadCount;
            this.callTimeTag = callTimeTag;
            this.callTime = callTime;
            this.entriesReturnedTag = entriesReturnedTag;
            this.entriesReturned = entriesReturned;
            this.entriesVisitedTag = entriesVisitedTag;
            this.entriesVisited = entriesVisited;
            this.filterTag = filterTag;
            this.filter = filter;
            this.indexTag = indexTag;
            this.index = index;
        }
    }
}

