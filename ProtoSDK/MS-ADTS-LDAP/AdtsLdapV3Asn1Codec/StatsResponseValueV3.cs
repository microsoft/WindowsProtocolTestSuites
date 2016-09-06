// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	StatsResponseValueV3 ::= SEQUENCE {
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
	    pagesReferencedTag    INTEGER
	    pagesReferenced       INTEGER
	    pagesReadTag          INTEGER
	    pagesRead             INTEGER
	    pagesPrereadTag       INTEGER
	    pagesPreread          INTEGER
	    pagesDirtiedTag       INTEGER
	    pagesDirtied          INTEGER
	    pagesRedirtiedTag     INTEGER
	    pagesRedirtied        INTEGER
	    logRecordCountTag     INTEGER
	    logRecordCount        INTEGER
	    logRecordBytesTag     INTEGER
	    logRecordBytes        INTEGER
	}

    */
    public class StatsResponseValueV3 : Asn1Sequence
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
        
        [Asn1Field(12)]
        public Asn1Integer pagesReferencedTag { get; set; }
        
        [Asn1Field(13)]
        public Asn1Integer pagesReferenced { get; set; }
        
        [Asn1Field(14)]
        public Asn1Integer pagesReadTag { get; set; }
        
        [Asn1Field(15)]
        public Asn1Integer pagesRead { get; set; }
        
        [Asn1Field(16)]
        public Asn1Integer pagesPrereadTag { get; set; }
        
        [Asn1Field(17)]
        public Asn1Integer pagesPreread { get; set; }
        
        [Asn1Field(18)]
        public Asn1Integer pagesDirtiedTag { get; set; }
        
        [Asn1Field(19)]
        public Asn1Integer pagesDirtied { get; set; }
        
        [Asn1Field(20)]
        public Asn1Integer pagesRedirtiedTag { get; set; }
        
        [Asn1Field(21)]
        public Asn1Integer pagesRedirtied { get; set; }
        
        [Asn1Field(22)]
        public Asn1Integer logRecordCountTag { get; set; }
        
        [Asn1Field(23)]
        public Asn1Integer logRecordCount { get; set; }
        
        [Asn1Field(24)]
        public Asn1Integer logRecordBytesTag { get; set; }
        
        [Asn1Field(25)]
        public Asn1Integer logRecordBytes { get; set; }
        
        public StatsResponseValueV3()
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
            this.pagesReferencedTag = null;
            this.pagesReferenced = null;
            this.pagesReadTag = null;
            this.pagesRead = null;
            this.pagesPrereadTag = null;
            this.pagesPreread = null;
            this.pagesDirtiedTag = null;
            this.pagesDirtied = null;
            this.pagesRedirtiedTag = null;
            this.pagesRedirtied = null;
            this.logRecordCountTag = null;
            this.logRecordCount = null;
            this.logRecordBytesTag = null;
            this.logRecordBytes = null;
        }
        
        public StatsResponseValueV3(
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
         Asn1OctetString index,
         Asn1Integer pagesReferencedTag,
         Asn1Integer pagesReferenced,
         Asn1Integer pagesReadTag,
         Asn1Integer pagesRead,
         Asn1Integer pagesPrereadTag,
         Asn1Integer pagesPreread,
         Asn1Integer pagesDirtiedTag,
         Asn1Integer pagesDirtied,
         Asn1Integer pagesRedirtiedTag,
         Asn1Integer pagesRedirtied,
         Asn1Integer logRecordCountTag,
         Asn1Integer logRecordCount,
         Asn1Integer logRecordBytesTag,
         Asn1Integer logRecordBytes)
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
            this.pagesReferencedTag = pagesReferencedTag;
            this.pagesReferenced = pagesReferenced;
            this.pagesReadTag = pagesReadTag;
            this.pagesRead = pagesRead;
            this.pagesPrereadTag = pagesPrereadTag;
            this.pagesPreread = pagesPreread;
            this.pagesDirtiedTag = pagesDirtiedTag;
            this.pagesDirtied = pagesDirtied;
            this.pagesRedirtiedTag = pagesRedirtiedTag;
            this.pagesRedirtied = pagesRedirtied;
            this.logRecordCountTag = logRecordCountTag;
            this.logRecordCount = logRecordCount;
            this.logRecordBytesTag = logRecordBytesTag;
            this.logRecordBytes = logRecordBytes;
        }
    }
}

