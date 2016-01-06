// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	TreeDeleteExRequestValue ::= SEQUENCE {
	    countOfObjectsToDelete    INTEGER
	}

    */
    public class TreeDeleteExRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer countOfObjectsToDelete { get; set; }
        
        public TreeDeleteExRequestValue()
        {
            this.countOfObjectsToDelete = null;
        }
        
        public TreeDeleteExRequestValue(
         Asn1Integer countOfObjectsToDelete)
        {
            this.countOfObjectsToDelete = countOfObjectsToDelete;
        }
    }
}

