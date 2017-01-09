// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    SicilyBindResponse_resultCode ::=  ENUMERATED {
	                     success                     (0),
	                     protocolError               (2),
	                     adminLimitExceeded          (11),
	                     inappropriateAuthentication (48),
	                     invalidCredentials          (49),
	                     busy                        (51),
	                     unavailable                 (52),
	                     unwillingToPerform          (53),
	                     other                       (80) }
    */
    public class SicilyBindResponse_resultCode : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long success = 0;
        
        [Asn1EnumeratedElement]
        public const long protocolError = 1;
        
        [Asn1EnumeratedElement]
        public const long adminLimitExceeded = 2;
        
        [Asn1EnumeratedElement]
        public const long inappropriateAuthentication = 3;
        
        [Asn1EnumeratedElement]
        public const long invalidCredentials = 4;
        
        [Asn1EnumeratedElement]
        public const long busy = 5;
        
        [Asn1EnumeratedElement]
        public const long unavailable = 6;
        
        [Asn1EnumeratedElement]
        public const long unwillingToPerform = 7;
        
        [Asn1EnumeratedElement]
        public const long other = 8;
        
        public SicilyBindResponse_resultCode()
            : base()
        {
        }
        
        public SicilyBindResponse_resultCode(long val)
            : base(val)
        {
        }
    }
}

