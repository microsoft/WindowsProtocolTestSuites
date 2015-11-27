// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    PA-PAC-OPTIONS ::=
    KerberosFlags
     -- Claims (0)
     -- Branch Aware (1)
     -- Forward to Full DC (2)
    */
    public class PA_PAC_OPTIONS : KerberosFlags
    {
        
        public PA_PAC_OPTIONS()
            : base()
        {
        }

        public PA_PAC_OPTIONS(string s)
            : base(s)
        {
        }
    }
}

