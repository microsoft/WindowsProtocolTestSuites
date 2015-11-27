// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    PA-SUPPORTED-ENCTYPES ::= Int32 –- Supported Encryption Types Bit Field --
    */
    public class PA_SUPPORTED_ENCTYPES : KerbInt32
    {
        
        public PA_SUPPORTED_ENCTYPES()
            : base()
        {
        }

        public PA_SUPPORTED_ENCTYPES(long val): base(val)
        {
        }
    }
}

