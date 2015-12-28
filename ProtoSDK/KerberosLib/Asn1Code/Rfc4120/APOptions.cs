// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    APOptions       ::= KerberosFlags
        -- reserved(0),
        -- use-session-key(1),
    -- mutual-required(2)
    */
    public class APOptions : KerberosFlags
    {
        
        public APOptions()
            : base()
        {
        }

        public APOptions(string s)
            : base(s)
        {

        }
    }
}

