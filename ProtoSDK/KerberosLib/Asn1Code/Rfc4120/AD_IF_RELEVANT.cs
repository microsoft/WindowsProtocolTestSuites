// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    AD-IF-RELEVANT          ::= AuthorizationData
    */
    public class AD_IF_RELEVANT : AuthorizationData
    {
        
        public AD_IF_RELEVANT()
            : base()
        {
        }

        public AD_IF_RELEVANT(AuthorizationDataElement[] val)
            : base(val)
        {
        }
    }
}

