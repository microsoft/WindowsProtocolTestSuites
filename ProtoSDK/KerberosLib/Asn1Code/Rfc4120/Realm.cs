// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    Realm           ::= KerberosString
    */
    public class Realm : KerberosString
    {
        
        public Realm()
            : base()
        {
        }

        public Realm(string s)
            : base(s)
        {

        }
    }
}

