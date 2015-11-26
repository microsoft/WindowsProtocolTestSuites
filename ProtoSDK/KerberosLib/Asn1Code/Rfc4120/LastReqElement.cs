// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    LastReq         ::=     SEQUENCE OF SEQUENCE {
        lr-type         [0] Int32,
        lr-value        [1] KerberosTime
    }
    */
    public class LastReqElement : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 lr_type { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public KerberosTime lr_value { get; set; }
        
        public LastReqElement()
        {
            this.lr_type = null;
            this.lr_value = null;
        }
        
        public LastReqElement(
         KerbInt32 param0,
         KerberosTime param1)
        {
            this.lr_type = param0;
            this.lr_value = param1;
        }
    }
}

