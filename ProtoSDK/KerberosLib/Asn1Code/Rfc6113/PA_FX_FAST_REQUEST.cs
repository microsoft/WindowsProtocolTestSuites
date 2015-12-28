// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    PA-FX-FAST-REQUEST ::= CHOICE {
          armored-data [0] KrbFastArmoredReq,
          ...
      }
    */
    public class PA_FX_FAST_REQUEST : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long armored_data = 0;
        [Asn1ChoiceElement(armored_data), Asn1Tag(Asn1TagType.Context, 0)]
        protected KrbFastArmoredReq field0 { get; set; }
        
        public PA_FX_FAST_REQUEST()
            : base()
        {
        }
        
        public PA_FX_FAST_REQUEST(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

