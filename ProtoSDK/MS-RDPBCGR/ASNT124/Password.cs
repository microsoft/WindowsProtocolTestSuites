// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    Password ::= SEQUENCE
    {
        numeric SimpleNumericString,
        text SimpleTextString OPTIONAL,
        ...,
        unicodeText TextString OPTIONAL
    }
    */
    public class Password : Asn1Sequence
    {
        [Asn1Field(0)]
        public SimpleNumericString numeric { get; set; }
        
        [Asn1Field(1, Optional = true)]
        public SimpleTextString text { get; set; }

        [Asn1Extension]
        public object obj = null;

        public Password()
        {
            this.numeric = null;
            this.text = null;
        }
        
        public Password(
         SimpleNumericString numeric,
         SimpleTextString text)
        {
            this.numeric = numeric;
            this.text = text;
        }

        public Password(string str)
            : this(new SimpleNumericString(str), null)
        {
        }
    }
}

