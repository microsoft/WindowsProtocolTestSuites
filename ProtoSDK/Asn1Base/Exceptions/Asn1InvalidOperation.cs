// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is thrown when user trying to do some invalid operations.
    /// </summary>
    public class Asn1InvalidOperation : Asn1Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1InvalidOperation exception by the default error message.
        /// </summary>
        public Asn1InvalidOperation()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1InvalidOperation exception by the given error message.
        /// </summary>
        public Asn1InvalidOperation(string str)
            : base(str)
        {

        }
    }
}
