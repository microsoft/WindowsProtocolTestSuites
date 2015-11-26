// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    PA-AUTHENTICATION-SET ::= SEQUENCE OF PA-AUTHENTICATION-SET-ELEM
    */
    public class PA_AUTHENTICATION_SET : Asn1SequenceOf<PA_AUTHENTICATION_SET_ELEM>
    {
        
        public PA_AUTHENTICATION_SET()
            : base()
        {
            this.Elements = null;
        }
        
        public PA_AUTHENTICATION_SET(PA_AUTHENTICATION_SET_ELEM[] val)
            : base(val)
        {
        }
    }
}

