// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    NegTokenResp ::= SEQUENCE {
       negState       [0] ENUMERATED {
           accept-completed    (0),
           accept-incomplete   (1),
           reject              (2),
           request-mic         (3)
       }                                 OPTIONAL,
         -- REQUIRED in the first reply from the target
       supportedMech   [1] MechType      OPTIONAL,
         -- present only in the first reply from the target
       responseToken   [2] OCTET STRING  OPTIONAL,
       mechListMIC     [3] OCTET STRING  OPTIONAL,
       ...
   }
    */
    public class NegState : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long accept_completed = 0;
        
        [Asn1EnumeratedElement]
        public const long accept_incomplete = 1;
        
        [Asn1EnumeratedElement]
        public const long reject = 2;
        
        [Asn1EnumeratedElement]
        public const long request_mic = 3;
        
        public NegState()
            : base()
        {
        }
        
        public NegState(long val)
            : base()
        {
        }
    }
}

