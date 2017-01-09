// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3 {

    public class SearchHints_element : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPOID hintId { get; set; }

        [Asn1Field(1)]
        public Asn1OctetString hintValue { get; set; }

      public SearchHints_element ()
      {
          hintId = null;
          hintValue = null;
      }

      /// <summary>
      /// This constructor sets all elements to references to the 
      /// given objects
      /// </summary>
      public SearchHints_element (         LDAPOID hintId_,         Asn1OctetString hintValue_      )
      {
         hintId = hintId_;
         hintValue = hintValue_;
      }

      /// <summary>
      /// This constructor allows primitive data to be passed for all 
      /// primitive elements.  It will create new object wrappers for 
      /// the primitive data and set other elements to references to 
      /// the given objects 
      /// </summary>
      public SearchHints_element (LDAPOID hintId_,         byte[] hintValue_      )
      {
         hintId = hintId_;
         hintValue = new Asn1OctetString (hintValue_);
      }
   }
}
