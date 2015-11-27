// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    AD-MANDATORY-FOR-KDC    ::= AuthorizationData
    */
    public class AD_MANDATORY_FOR_KDC : AuthorizationData
    {
        
        public AD_MANDATORY_FOR_KDC()
            : base()
        {
        }

        public AD_MANDATORY_FOR_KDC(AuthorizationDataElement[] val)
            : base(val)
        {
        }
    }
}

