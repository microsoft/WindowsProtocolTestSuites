// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3 {

    public class BatchRequestValue : Asn1SequenceOf<Asn1OctetString>
    {
      public BatchRequestValue () : base()
      {
         this.Elements = null;
      }

      /// <summary>
      /// This constructor initializes the internal array to hold the 
      /// given number of elements.  The element values must be manually 
      /// populated.
      /// </summary>
      public BatchRequestValue (int numRecords) : base()
      {
         this.Elements = new Asn1OctetString [numRecords];
      }

      /// <summary>
      /// This constructor initializes the internal array to hold the 
      /// given the array.  
      /// </summary>
      public BatchRequestValue(Asn1OctetString[] elements_)
          : base(elements_)
      {
      }
   }
}
