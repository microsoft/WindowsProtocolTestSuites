// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class TreeDeleteEx : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer countOfObjectsToDelete { get; set; }

        public TreeDeleteEx()
        {
            countOfObjectsToDelete = null;
        }

        /// <summary>
        /// This constructor sets all elements to references to the 
        /// given objects
        /// </summary>
        public TreeDeleteEx(Asn1Integer countOfObjectsToDelete_)
        {
            countOfObjectsToDelete = countOfObjectsToDelete_;
        }

        /// <summary>
        /// This constructor allows primitive data to be passed for all 
        /// primitive elements.  It will create new object wrappers for 
        /// the primitive data and set other elements to references to 
        /// the given objects 
        /// </summary>
        public TreeDeleteEx(long countOfObjectsToDelete_)
        {
            countOfObjectsToDelete = new Asn1Integer(countOfObjectsToDelete_);
        }

        public void Init()
        {
            countOfObjectsToDelete = null;
        }
    }
}
