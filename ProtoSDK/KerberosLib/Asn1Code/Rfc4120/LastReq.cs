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
    public class LastReq : Asn1SequenceOf<LastReqElement>
    {
        
        public LastReq()
            : base()
        {
            this.Elements = null;
        }
        
        public LastReq(LastReqElement[] val)
            : base(val)
        {
        }
    }
}

