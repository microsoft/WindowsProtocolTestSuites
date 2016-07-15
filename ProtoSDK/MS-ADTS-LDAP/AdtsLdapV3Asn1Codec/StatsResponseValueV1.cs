// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	StatsResponseValueV1 ::= SEQUENCE {
	    threadCountTag            INTEGER
	    threadCount               INTEGER
	    coreTimeTag               INTEGER
	    coreTime                  INTEGER
	    callTimeTag               INTEGER
	    callTime                  INTEGER
	    searchSubOperationsTag    INTEGER
	    searchSubOperations       INTEGER
	}

    */
    public class StatsResponseValueV1 : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer threadCountTag { get; set; }
        
        [Asn1Field(1)]
        public Asn1Integer threadCount { get; set; }
        
        [Asn1Field(2)]
        public Asn1Integer coreTimeTag { get; set; }
        
        [Asn1Field(3)]
        public Asn1Integer coreTime { get; set; }
        
        [Asn1Field(4)]
        public Asn1Integer callTimeTag { get; set; }
        
        [Asn1Field(5)]
        public Asn1Integer callTime { get; set; }
        
        [Asn1Field(6)]
        public Asn1Integer searchSubOperationsTag { get; set; }
        
        [Asn1Field(7)]
        public Asn1Integer searchSubOperations { get; set; }
        
        public StatsResponseValueV1()
        {
            this.threadCountTag = null;
            this.threadCount = null;
            this.coreTimeTag = null;
            this.coreTime = null;
            this.callTimeTag = null;
            this.callTime = null;
            this.searchSubOperationsTag = null;
            this.searchSubOperations = null;
        }
        
        public StatsResponseValueV1(
         Asn1Integer threadCountTag,
         Asn1Integer threadCount,
         Asn1Integer coreTimeTag,
         Asn1Integer coreTime,
         Asn1Integer callTimeTag,
         Asn1Integer callTime,
         Asn1Integer searchSubOperationsTag,
         Asn1Integer searchSubOperations)
        {
            this.threadCountTag = threadCountTag;
            this.threadCount = threadCount;
            this.coreTimeTag = coreTimeTag;
            this.coreTime = coreTime;
            this.callTimeTag = callTimeTag;
            this.callTime = callTime;
            this.searchSubOperationsTag = searchSubOperationsTag;
            this.searchSubOperations = searchSubOperations;
        }
    }
}

