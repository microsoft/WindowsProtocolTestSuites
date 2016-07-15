// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     MatchingRuleId ::= LDAPString
    */
    public class MatchingRuleId : LDAPString
    {
        public MatchingRuleId()
            : base()
        {
        }

        public MatchingRuleId(string val)
            : base(val)
        {
        }
    }
}

