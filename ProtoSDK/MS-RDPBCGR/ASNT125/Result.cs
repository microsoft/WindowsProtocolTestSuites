// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    Result ::= ENUMERATED -- in Connect, response, confirm
    {
        rt-successful (0),
        rt-domain-merging (1),
        rt-domain-not-hierarchical (2),
        rt-no-such-channel (3),
        rt-no-such-domain (4),
        rt-no-such-user (5),
        rt-not-admitted (6),
        rt-other-user-id (7),
        rt-parameters-unacceptable (8),
        rt-token-not-available (9),
        rt-token-not-possessed (10),
        rt-too-many-channels (11),
        rt-too-many-tokens (12),
        rt-too-many-users (13),
        rt-unspecified-failure (14),
        rt-user-rejected (15)
    }
    */
    public class Result : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long rt_successful = 0;

        [Asn1EnumeratedElement]
        public const long rt_domain_merging = 1;

        [Asn1EnumeratedElement]
        public const long rt_domain_not_hierarchical = 2;

        [Asn1EnumeratedElement]
        public const long rt_no_such_channel = 3;

        [Asn1EnumeratedElement]
        public const long rt_no_such_domain = 4;

        [Asn1EnumeratedElement]
        public const long rt_no_such_user = 5;

        [Asn1EnumeratedElement]
        public const long rt_not_admitted = 6;

        [Asn1EnumeratedElement]
        public const long rt_other_user_id = 7;

        [Asn1EnumeratedElement]
        public const long rt_parameters_unacceptable = 8;

        [Asn1EnumeratedElement]
        public const long rt_token_not_available = 9;

        [Asn1EnumeratedElement]
        public const long rt_token_not_possessed = 10;

        [Asn1EnumeratedElement]
        public const long rt_too_many_channels = 11;

        [Asn1EnumeratedElement]
        public const long rt_too_many_tokens = 12;

        [Asn1EnumeratedElement]
        public const long rt_too_many_users = 13;

        [Asn1EnumeratedElement]
        public const long rt_unspecified_failure = 14;

        [Asn1EnumeratedElement]
        public const long rt_user_rejected = 15;

        public Result() : base()
        {

        }

        public Result(long? val) : base(val)
        {

        }
    }
}
