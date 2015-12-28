// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    Note. This class is manually defined.
    ContextFlags ::= BIT STRING {
       delegFlag       (0),
       mutualFlag      (1),
       replayFlag      (2),
       sequenceFlag    (3),
       anonFlag        (4),
       confFlag        (5),
       integFlag       (6)
    } (SIZE (32))
    */
    public class ContextFlags : Asn1BitString
    {
        public static readonly int delegFlag = 0;
        public static readonly int mutualFlag = 1;
        public static readonly int replayFlag = 2;
        public static readonly int sequenceFlag = 3;
        public static readonly int anonFlag = 4;
        public static readonly int confFlag = 5;
        public static readonly int integFlag = 6;

        public ContextFlags()
            : base()
        {
            ByteArrayValue = new byte[4];
        }

        protected override bool VerifyConstraints()
        {
            return this.Length == 32;
        }
    }
}